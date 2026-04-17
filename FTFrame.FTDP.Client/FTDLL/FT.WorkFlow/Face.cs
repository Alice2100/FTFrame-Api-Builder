using System;
using System.Collections.Generic;
using System.Text;
using FTFrame;
using FTFrame.Obj;
using FTFrame.DBClient;
using FTFrame.Tool;
using FTFrame.Base;
using System.Web;
using System.Collections;

namespace FT.Com.WorkFlow.Face
{
    public class Info:ComponentInfo
    {
        //基本信息
        public Component.Info Information()
        {
            Component.Info info = new Component.Info();
            info.name = "workflow";
            info.caption="自定义审批流程";
            info.devdes="FTFrame";
            info.appdes="实现审批流程自定义，并且可供其他组件调用";
            info.version= "1.0";
            info.trade= "通用";
            info.apptype="系统类";
            info.icon="";
            info.showhtml="<b>可灵活自定义的审批工作流</b>";
            info.showurl= "res/show.html";
            return info;
        }
        //目录菜单
        public ArrayList Menu()
        {
            Component.DirTree dt = new Component.DirTree();
            dt.AddPage("a.aspx", "页面a",null);
            Component.DirTree cate1 = dt.AddCate("b", "目录b", null);
            cate1.AddPage("b.aspx", "页面b", null);
            cate1.AddSet("set1", "基本设置", null);
            cate1.AddDic("dic1", "字典设置", null);
            return dt.Tree();
        }
        //资源树（用于分配权限）
        public ArrayList Resource()
        {
            Component.DirTree dt = new Component.DirTree();
            dt.AddPage("a.aspx", "页面a", null);
            Component.DirTree cate1 = dt.AddCate("b", "目录b", null);
            cate1.AddPage("b.aspx", "页面b", null);
            cate1.AddSet("set1", "基本设置", null);
            cate1.AddDic("dic1", "字典设置", null);
            cate1.AddPage("c.aspx", "页面c", null);
            return dt.Tree();
        }
        //参数设置定义
        public ArrayList Set()
        {
            /*
             注：FTFrame.Component 实现组件的Init、Code等与原先Project对接的接口，新组件要在这里注册下
             * ComponentFunc下方法GetSetValue为得到参数设置的值，GetDicOptions和GetDicSql为得到字典options和sql
             */
            Component.Set set = new Component.Set();
            set.AddSet("set1", "key1","设置1", null, Component.SetType.Text, Component.SetValidate.Int, null, null,null);
            set.AddSet("set1", "key2", "设置2", null, Component.SetType.Select, Component.SetValidate.NoEmpty, null, 
                new Component.SelVal[] { new Component.SelVal("请选择", ""), new Component.SelVal("上海", "sh"), new Component.SelVal("深圳", "sz") }
               , null);
            return set.SetList();
        }
        //字典设置定义
        public ArrayList Dic()
        {
            /*
             注：FTFrame.Component 实现组件的Init、Code等与原先Project对接的接口，新组件要在这里注册下
             * ComponentFunc下方法GetSetValue为得到参数设置的值，GetDicOptions和GetDicSql为得到字典options和sql
             */
            Component.Dic dic = new Component.Dic();
            dic.AddDic("dic1", "d1", "字典一", "字典一说明", new Component.DicInfo[] { new Component.DicInfo("code1", "名称1", 1), new Component.DicInfo("code2", "名称2", 2) });
            dic.AddDic("dic1", "d2", "字典二", "字典二说明",null);
            return dic.DicList();
        }
        //依赖组件定义
        public string[] Depand()
        {
            return new string[] { };
        }
    }
    public class Init : ComponentInit
    {
        /// <summary>
        /// 应用启动时（非安装）初始化
        /// </summary>
        public void Initialize()
        {

        }
        /// <summary>
        /// 创建组件数据结构
        /// </summary>
        /// <param name="connstr">具备执行脚本权限的连接串</param>
        /// <param name="schema">[dbo]被替换成[schema]</param>
        /// <returns></returns>
        public string Create(string connstr,string schema)
        {
            string ExTemp = "";
            DB db = new DB();
            db.Open(connstr);
            ST st = db.GetTransaction();
            try
            {
                string SqlScript = Script_Create();
                string[] Items = SqlScript.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                string sql = "";
                foreach (string Item in Items)
                {
                    if (Item.Trim().Equals("GO"))
                    {
                        if (!sql.Trim().Equals(""))
                        {
                            sql = sql.Replace("[dbo]", "[" + schema + "]");
                            ExTemp = sql;
                            db.execSql(sql, st);
                        }
                        sql = "";
                    }
                    else
                    {
                        sql += " \r\n " + Item.Trim() + " ";
                    }
                }
                if (!sql.Trim().Equals(""))
                {
                    sql = sql.Replace("[dbo]", "[" + schema + "]");
                    ExTemp = sql;
                    db.execSql(sql, st);
                }
                st.Commit();
                return null;
            }
            catch (Exception ex)
            {
                st.Rollback();
                log.Error(ExTemp);
                log.Error(ex);
                return ex.Message;
            }
            finally
            {
                db.Close();
            }
        }
        /// <summary>
        /// 删除组件数据结构
        /// </summary>
        /// <param name="connstr">具备执行脚本权限的连接串</param>
        /// <param name="schema">[dbo]被替换成[schema]</param>
        /// <returns></returns>
        public string Destroy(string connstr, string schema)
        {
            string ExTemp = "";
            DB db = new DB();
            db.Open(connstr);
            ST st = db.GetTransaction();
            try
            {
                string SqlScript = Script_Destroy();
                string[] Items = SqlScript.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                string sql = "";
                foreach (string Item in Items)
                {
                    if (Item.Trim().Equals("GO"))
                    {
                        if (!sql.Trim().Equals(""))
                        {
                            sql = sql.Replace("[dbo]", "[" + schema + "]");
                            ExTemp = sql;
                            db.execSql(sql, st);
                        }
                        sql = "";
                    }
                    else
                    {
                        sql += " \r\n " + Item.Trim() + " ";
                    }
                }
                if (!sql.Trim().Equals(""))
                {
                    sql = sql.Replace("[dbo]", "[" + schema + "]");
                    ExTemp = sql;
                    db.execSql(sql, st);
                }
                st.Commit();
                return null;
            }
            catch (Exception ex)
            {
                st.Rollback();
                log.Error(ExTemp);
                log.Error(ex);
                return ex.Message;
            }
            finally
            {
                db.Close();
            }
        }
        /// <summary>
        /// 重建新的结构，会初始化数据
        /// </summary>
        /// <param name="connstr">具备执行脚本权限的连接串</param>
        /// <param name="schema">[dbo]被替换成[schema]</param>
        /// <returns></returns>
        public string Rebuild(string connstr, string schema)
        {
            string restr=Destroy(connstr, schema);
            if (restr != null) return restr;
            restr = Create(connstr, schema);
            return restr;
        }
        /// <summary>
        /// 创建该组件时的脚本，需使用IF NOT EXISTS
        /// </summary>
        /// <returns></returns>
        public string Script_Create()
        {
            switch (SysConst.DataBaseType)
            {
                case DataBase.SqlServer: return Res.Create_SqlServer;
                case DataBase.MySql: return Res.Create_MySql;
                case DataBase.Oracle: return Res.Create_Oracle;
            }
            return Component.NoDefineScript;
        }
        /// <summary>
        /// 删除该组件时的脚本，需使用IF  EXISTS
        /// </summary>
        /// <returns></returns>
        public string Script_Destroy()
        {
            switch (SysConst.DataBaseType)
            {
                case DataBase.SqlServer: return Res.Destroy_SqlServer;
                case DataBase.MySql: return Res.Destroy_MySql;
                case DataBase.Oracle: return Res.Destroy_Oracle;
            }
            return Component.NoDefineScript;
        }
    }
}
