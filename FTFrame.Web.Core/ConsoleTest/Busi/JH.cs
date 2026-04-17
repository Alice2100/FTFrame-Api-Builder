using System;
using System.Collections.Generic;
using FTFrame.DBClient;
using System.Linq;
using System.Text;
using FTFrame.Tool;
using FTFrame.BLL.Model;
namespace ConsoleTest.Busi
{
    public class JH
    {
        public void Busi_Test()
        {
            //DB 初始化
            DBConst.DataBaseType = DataBase.MySql;
            DBConst.DBConnString = "";
            DBConst.ModelDllName = "ConsoleTest";
            //执行测试
            Cacu_Index_Level_1();
        }
        //只重新计算当月和上月
        string[] periodList = new string[] {
                    DateTime.Now.ToString("yyyyMM"),
                    DateTime.Now.AddMonths(-1).ToString("yyyyMM")
                };
        private string periodSql()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < periodList.Length; i++)
            {
                if (i > 0) sb.Append(",");
                sb.Append("'" + str.D2DD(periodList[i]) + "'");
            }
            return "(" + sb.ToString() + ")";
        }
        /// <summary>
        /// 计算_个人一级指标
        /// </summary>
        public void Cacu_Index_Level_1()
        {
            try
            {
                using var db = new DB();
                //删除一级指标计算数据
                db.Delete<ca_index_level_1>(r => periodList.Contains(r.period));
                //ex_member数据
                var exDatas = db.SelectListBySQLForTuple<ex_examine, ex_member>("select a.examine_code,a.evaluate_code,a.period,b.* from ex_examine a" +
                    " inner join ex_member b on a.id=b.examine_id" +
                    " where a.period in " + periodSql() + " and b.is_examined=1" +
                    " and a.is_del=0 and b.is_del=0"
                    , "id", "asc");
                //获取所有member_id
                var memberIds = exDatas.Select(r => r.Item2.member_id).Distinct().ToList();
                //获取所有时期
                var periods = exDatas.Select(r => r.Item1.period).Distinct().ToList();
                //获取所有体检方式
                var examineCodes = exDatas.Select(r => r.Item1.examine_code).Distinct().ToList();
                //获取所有评价方式
                var evaluateCodes = exDatas.Select(r => r.Item1.evaluate_code).Distinct().ToList();
                //按以上四个条件取评价主表数据
                periods.ForEach(period =>
                {
                    examineCodes.ForEach(examineCode =>
                    {
                        evaluateCodes.ForEach(evaluateCode =>
                        {
                            memberIds.ForEach(memberId =>
                            {
                                var exData = exDatas.Where(r => r.Item1.period == period && r.Item1.examine_code == examineCode && r.Item1.evaluate_code == evaluateCode && r.Item2.member_id == memberId).ToList();
                                if (exData.Count() > 0)
                                {
                                    var indexScore = new Dictionary<string, decimal>();
                                    exData.ForEach(r =>
                                    {
                                        var examine_member_id = r.Item2.id;
                                        var exScores = db.SelectList<ex_member_index>(s => s.examine_member_id == examine_member_id && s.is_del == 0, s => s.OrderBy(t => t.id)).ToList();
                                        exScores.ForEach(s =>
                                        {
                                            var index_level_1 = db.SelectOneBySQL<bs_index_level_1>($"select a.index_code from bs_index_level_1 a left join bs_index_level_2 b on b.pid=a.id left join bs_index_level_3 c on c.pid=b.id where c.id='{s.index_level_3_id}'");
                                            if (index_level_1 != null)
                                            {
                                                //一级指标分数先按三级指标分数相加
                                                if (!indexScore.ContainsKey(index_level_1.index_code)) indexScore.Add(index_level_1.index_code, s.point.Value);
                                                else indexScore[index_level_1.index_code] += s.point.Value;
                                            }
                                        });
                                    });
                                    //多人评价取平均值
                                    foreach (var key in indexScore.Keys)
                                    {
                                        indexScore[key] = indexScore[key] / exData.Count();
                                    }
                                    //插入到数据库
                                    foreach (var key in indexScore.Keys)
                                    {
                                        db.Add(new ca_index_level_1()
                                        {
                                            id = Str.SnowId(),
                                            evaluate_code = evaluateCode,
                                            examine_code = examineCode,
                                            index_code = key,
                                            member_id = memberId,
                                            period = period,
                                            point = indexScore[key],
                                            add_time = DateTime.Now,
                                        });
                                        log.Debug($"ca_index_level_1 inserted {period} {key} {memberId} {examineCode} {evaluateCode}");
                                    }
                                }
                            });
                        });
                    });
                });
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        /// <summary>
        /// 计算_个人单项指标分数（按体检类型）
        /// </summary>
        public void Cacu_Index_Examine()
        {
            try
            {
                using var db = new DB();
                //删除计算数据
                db.Delete<ca_index_examine>(r => periodList.Contains(r.period));
                foreach (var period in periodList)
                {
                    var data = db.SelectList<ca_index_level_1>(r => r.period == period, r => r.OrderBy(s => s.id));
                    //获取所有member_id
                    var memberIds = data.Select(r => r.member_id).Distinct().ToList();
                    //获取所有一级指标
                    var indexCodes = data.Select(r => r.index_code).Distinct().ToList();
                    //获取所有体检方式
                    var examineCodes = data.Select(r => r.examine_code).Distinct().ToList();
                    memberIds.ForEach(memberId =>
                    {
                        //按不同评价类型相加则为指标综合分
                        indexCodes.ForEach(indexCode =>
                        {
                            examineCodes.ForEach(examineCode =>
                            {
                                var point = data.Where(r => r.member_id == memberId && r.index_code == indexCode && r.examine_code == examineCode).Select(r => r.point).Sum();
                                if (point != null)
                                {
                                    //插入到数据库
                                    db.Add(new ca_index_examine()
                                    {
                                        id = Str.SnowId(),
                                        index_code = indexCode,
                                        member_id = memberId,
                                        examine_code = examineCode,
                                        period = period,
                                        point = point,
                                        add_time = DateTime.Now,
                                    });
                                    log.Debug($"ca_index_examine inserted {period} {indexCode} {memberId} {examineCode}");
                                }
                            });
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        /// <summary>
        /// 计算_个人指标综合分数
        /// </summary>
        public void Cacu_Index()
        {
            try
            {
                using var db = new DB();
                //删除一级指标计算数据
                db.Delete<ca_index>(r => periodList.Contains(r.period));
                foreach (var period in periodList)
                {
                    var data = db.SelectList<ca_index_level_1>(r => r.period == period, r => r.OrderBy(s => s.id));
                    //获取所有member_id
                    var memberIds = data.Select(r => r.member_id).Distinct().ToList();
                    //获取所有一级指标
                    var indexCodes = data.Select(r => r.index_code).Distinct().ToList();
                    memberIds.ForEach(memberId =>
                    {
                        //按不同评价类型、体检类型相加则为指标综合分
                        indexCodes.ForEach(indexCode =>
                        {
                            var point = data.Where(r => r.member_id == memberId && r.index_code == indexCode).Select(r => r.point).Sum();
                            if (point != null)
                            {
                                //插入到数据库
                                db.Add(new ca_index()
                                {
                                    id = Str.SnowId(),
                                    index_code = indexCode,
                                    member_id = memberId,
                                    period = period,
                                    point = point,
                                    add_time = DateTime.Now,
                                });
                                log.Debug($"ca_index inserted {period} {indexCode} {memberId}");
                            }
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        /// <summary>
        /// 计算_部门指标综合分数
        /// </summary>
        public void Cacu_Index_Department()
        {
            try
            {
                using var db = new DB();
                //删除计算数据
                db.Delete<ca_index_department>(r => periodList.Contains(r.period));
                foreach (var period in periodList)
                {
                    var deptIds = db.SelectList<sys_dept>(r => r.status == "0" && r.del_flag == "0", r => r.OrderBy(s => s.dept_id)).Select(r => r.dept_id).ToList();
                    deptIds.ForEach(deptId =>
                    {
                        var data = db.SelectListBySQL<ca_index>("select a.* from ca_index a" +
                        " left join bs_member b on a.member_id=b.id" +
                        " left join sys_user u on b.user_id=u.user_id" +
                        " where a.period='" + str.D2DD(period) + "' and u.dept_id=" + deptId
                        , "id", "asc");
                        //获取一级指标
                        var indexCodes = data.Select(r => r.index_code).Distinct().ToList();
                        //部门人员平均分为部门综合分
                        indexCodes.ForEach(indexCode =>
                        {
                            var point = data.Where(r => r.index_code == indexCode).Select(r => r.point).Average();
                            if (point != null)
                            {
                                //插入到数据库
                                db.Add(new ca_index_department()
                                {
                                    id = Str.SnowId(),
                                    index_code = indexCode,
                                    department_id = deptId,
                                    period = period,
                                    point = point,
                                    add_time = DateTime.Now,
                                });
                                log.Debug($"ca_index_department inserted {period} {indexCode} {deptId}");
                            }
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        /// <summary>
        /// 计算_全院指标综合分数
        /// </summary>
        public void Cacu_Index_Corp()
        {
            try
            {
                using var db = new DB();
                //删除计算数据
                db.Delete<ca_index_corp>(r => periodList.Contains(r.period));
                foreach (var period in periodList)
                {
                    var data = db.SelectList<ca_index_department>(r => r.period == period, r => r.OrderBy(s => s.id)).ToList();
                    //获取一级指标
                    var indexCodes = data.Select(r => r.index_code).Distinct().ToList();
                    //各部门平均分为全院综合分
                    indexCodes.ForEach(indexCode =>
                    {
                        var point = data.Where(r => r.index_code == indexCode).Select(r => r.point).Average();
                        if (point != null)
                        {
                            //插入到数据库
                            db.Add(new ca_index_corp()
                            {
                                id = Str.SnowId(),
                                index_code = indexCode,
                                corp_id = 0,
                                period = period,
                                point = point,
                                add_time = DateTime.Now,
                            });
                            log.Debug($"ca_index_corp inserted {period} {indexCode} 0");
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        /// <summary>
        /// 计算_个人单项综合分数（按体检类型）
        /// </summary>
        public void Cacu_Score_Examine()
        {
            try
            {
                using var db = new DB();
                //删除计算数据
                db.Delete<ca_score_examine>(r => periodList.Contains(r.period));
                foreach (var period in periodList)
                {
                    var data = db.SelectList<ca_index_level_1>(r => r.period == period, r => r.OrderBy(s => s.id));
                    //获取所有member_id
                    var memberIds = data.Select(r => r.member_id).Distinct().ToList();
                    //获取所有一级指标
                    var indexCodes = data.Select(r => r.index_code).Distinct().ToList();
                    //获取所有体检方式
                    var examineCodes = data.Select(r => r.examine_code).Distinct().ToList();
                    memberIds.ForEach(memberId =>
                    {
                        //按所有一级指标分数加和为单项综合分
                        examineCodes.ForEach(examineCode =>
                        {
                            var point = data.Where(r => r.member_id == memberId && r.examine_code == examineCode).Select(r => r.point).Sum();
                            if (point != null)
                            {
                                //插入到数据库
                                db.Add(new ca_score_examine()
                                {
                                    id = Str.SnowId(),
                                    member_id = memberId,
                                    examine_code = examineCode,
                                    period = period,
                                    point = point,
                                    add_time = DateTime.Now,
                                });
                                log.Debug($"ca_score_examine inserted {period} {memberId} {examineCode}");
                            }
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        /// <summary>
        /// 计算_个人综合分数
        /// </summary>
        public void Cacu_Score()
        {
            try
            {
                using var db = new DB();
                //删除计算数据
                db.Delete<ca_score>(r => periodList.Contains(r.period));
                foreach (var period in periodList)
                {
                    var data = db.SelectList<ca_score_examine>(r => r.period == period, r => r.OrderBy(s => s.id));
                    //获取所有member_id
                    var memberIds = data.Select(r => r.member_id).Distinct().ToList();
                    //获取所有体检方式
                    var examineCodes = data.Select(r => r.examine_code).Distinct().ToList();
                    memberIds.ForEach(memberId =>
                    {
                        //按所有单项综合分加和为综合分
                        var point = data.Where(r => r.member_id == memberId).Select(r => r.point).Sum();
                        if (point != null)
                        {
                            //插入到数据库
                            db.Add(new ca_score()
                            {
                                id = Str.SnowId(),
                                member_id = memberId,
                                evaluate_rank_code = "00",//计算部门分数时再刷新人员在部门中的评价等级
                                period = period,
                                point = point,
                                add_time = DateTime.Now,
                            });
                            log.Debug($"ca_score inserted {period} {memberId}");
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        /// <summary>
        /// 计算_部门综合分数
        /// </summary>
        public void Cacu_Score_Department()
        {
            try
            {
                using var db = new DB();
                //删除计算数据
                db.Delete<ca_score_department>(r => periodList.Contains(r.period));
                foreach (var period in periodList)
                {
                    var deptIds = db.SelectList<sys_dept>(r => r.status == "0" && r.del_flag == "0", r => r.OrderBy(s => s.dept_id)).Select(r => r.dept_id).ToList();
                    deptIds.ForEach(deptId =>
                    {
                        var data = db.SelectListBySQL<ca_score>("select a.* from ca_score a" +
                        " left join bs_member b on a.member_id=b.id" +
                        " left join sys_user u on b.user_id=u.user_id" +
                        " where a.period='" + str.D2DD(period) + "' and u.dept_id=" + deptId
                        , "id", "asc");
                        //部门人员平均分为部门综合分
                        var point = data.Select(r => r.point).Average();
                        if (point != null)
                        {
                            //更新ca_score评价等级
                            var orderedScore = data.OrderByDescending(r => r.point).ToList();
                            for (int i = 0; i < orderedScore.Count; i++)
                            {
                                var rate = 100 * Convert.ToDecimal(i) / Convert.ToDecimal(orderedScore.Count);
                                var rank_code = "00";
                                switch (rate)
                                {
                                    case decimal rt when rt < 25://优秀 从配置表中读取
                                        rank_code = "01";
                                        break;
                                    case decimal rt when rt < 50://良好 从配置表中读取
                                        rank_code = "02";
                                        break;
                                    default://一般 从配置表中读取
                                        rank_code = "03";
                                        break;
                                }
                                orderedScore[i].evaluate_rank_code = rank_code;
                                db.Update<ca_score>(orderedScore[i], r => r.id == orderedScore[i].id, new string[] { "evaluate_rank_code" }, "id");
                            }
                            //插入到数据库
                            db.Add(new ca_score_department()
                            {
                                id = Str.SnowId(),
                                department_id = deptId,
                                period = period,
                                point = point,
                                add_time = DateTime.Now,
                            });
                            log.Debug($"ca_score_department inserted {period} {deptId}");
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        /// <summary>
        /// 定期自动生成未评价的体检数据
        /// </summary>
        /// <param name="period"></param>
        public void Gene_Examine_Member(string period = "")
        {
            //因为生成逻辑还未定 并且 不同时期的指标模板会有所不同，再未设置指标模板的情况下是否要自动生成还未明确
        }
        public void Test()
        {
            log.Debug("Test Runed");
        }
    }
}
