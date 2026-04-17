using System;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.Odbc;
using Oracle.ManagedDataAccess.Client;
using System.Data.OleDb;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Expressions;
using FTFrame.DBClient.DataBaseType;
using Microsoft.Data.Sqlite;

namespace FTFrame.DBClient
{
    public class DB : IDisposable
    {
        public int CommandTimeOut = 300;
        public DataBase CurDataBaseType;
        public object DBConnection;
        private DBInterface db;
        private DataBase ConnType = DBConst.DataBaseType;
        private bool IsOpen = false;
        public DB()
        {
            CurDataBaseType = ConnType;
            switch (ConnType)
            {
                case DataBase.MySql:
                    db = new DBMySql();
                    break;
                case DataBase.SqlServer:
                    db = new DBSqlServer();
                    break;
                case DataBase.Sqlite:
                    db = new DBSqlite();
                    break;
                case DataBase.Oracle:
                    db = new DBOracle();
                    break;
                default:
                    break;
            }
            DBConnection = db.DBConnection;
            Open();
        }
        public DB(bool open)
        {
            CurDataBaseType = ConnType;
            switch (ConnType)
            {
                case DataBase.MySql:
                    db = new DBMySql();
                    break;
                case DataBase.SqlServer:
                    db = new DBSqlServer();
                    break;
                case DataBase.Sqlite:
                    db = new DBSqlite();
                    break;
                case DataBase.Oracle:
                    db = new DBOracle();
                    break;
                default:
                    break;
            }
            DBConnection = db.DBConnection;
            if(open)Open();
        }
        public DB(string ConnString)
        {
            CurDataBaseType = ConnType;
            switch (ConnType)
            {
                case DataBase.MySql:
                    db = new DBMySql();
                    break;
                case DataBase.SqlServer:
                    db = new DBSqlServer();
                    break;
                case DataBase.Sqlite:
                    db = new DBSqlite();
                    break;
                case DataBase.Oracle:
                    db = new DBOracle();
                    break;
                default:
                    break;
            }
            DBConnection = db.DBConnection;
            Open(ConnString);
        }
        public DB(DataBase DBType, string ConnString)
        {
            CurDataBaseType = DBType;
            switch (DBType)
            {
                case DataBase.MySql:
                    db = new DBMySql();
                    break;
                case DataBase.SqlServer:
                    db = new DBSqlServer();
                    break;
                case DataBase.Sqlite:
                    db = new DBSqlite();
                    break;
                case DataBase.Oracle:
                    db = new DBOracle();
                    break;
                default:
                    break;
            }
            DBConnection = db.DBConnection;
            Open(ConnString);
        }
        public void Open()
        {
            Open(DBConst.DBConnString);
        }
        public void Open(string ConnStr)
        {
            if (!IsOpen)
            {
                IsOpen = true;
                db.Open(ConnStr);
            }
        }

        public object GetObject(string sqlstr, string colname)
        {
            return db.GetObject(sqlstr, colname, CommandTimeOut);
        }
        public object GetObject(string sqlstr)
        {
            return GetObject(sqlstr, null);
        }
        public string GetStringForceNoNull(string sqlstr)
        {
            return GetStringForceNoNull(sqlstr, null);
        }
        public string GetStringForceNoNull(string sqlstr, string colname)
        {
            object obj = GetObject(sqlstr, colname);
            return obj == null ? "" : obj.ToString();
        }
        public string GetString(string sqlstr, string colname)
        {
            return db.GetString(sqlstr, colname, CommandTimeOut);
        }
        public string GetString(string sqlstr)
        {
            return GetString(sqlstr, null);
        }
        public int GetInt(string sqlstr)
        {
            return db.GetInt(sqlstr, CommandTimeOut);
        }
        public decimal GetDecimal(string sqlstr)
        {
            return db.GetDecimal(sqlstr, CommandTimeOut);
        }
        public DataRowCollection Rows(string sql)
        {
            return GetDataTable(sql).Rows;
        }
        public DataTable GetDataTable(string sql)
        {
            return db.GetDataTable(sql);
        }
        public DataSet GetDataSet(string sql)
        {
            return GetDataSet(sql, -1, -1, null);
        }
        public DataSet GetDataSet(string sql, string SrcTable)
        {
            return GetDataSet(sql, -1, -1, SrcTable);
        }
        public DataSet GetDataSet(string sql, int StartRecord, int MaxRecord, string SrcTable)
        {
            return db.GetDataSet(sql, StartRecord, MaxRecord, SrcTable);
        }
        public DR OpenRecord(string sqlstr)
        {
            return db.OpenRecord(sqlstr, CommandTimeOut);
        }
        public DR OpenRecord(string sqlstr, ST st)
        {
            return db.OpenRecord(sqlstr, st, CommandTimeOut);
        }
        public object ExecuteReader(string sqlstr)
        {
            return db.ExecuteReader(sqlstr, CommandTimeOut);
        }
        public object ExecuteReader(string sqlstr, ST st)
        {
            return db.ExecuteReader(sqlstr, st, CommandTimeOut);
        }
        public Task<int> ExecSqlAsync(string sqlstr)
        {
            return db.ExecSqlAsync(sqlstr, CommandTimeOut);
        }
        public int ExecSql(string sqlstr)
        {
            return db.ExecSql(sqlstr, CommandTimeOut);
        }
        public int ExecSql(string sqlstr, PR para)
        {
            return db.ExecSql(sqlstr, para, CommandTimeOut);
        }
        public int ExecSql(string sqlstr, IEnumerable<PR> paras)
        {
            return db.ExecSql(sqlstr, paras, CommandTimeOut);
        }
        public int ExecSql(string sqlstr, ST transaction)
        {
            return db.ExecSql(sqlstr, transaction, CommandTimeOut);
        }
        public int ExecSql(string sqlstr, ST transaction, PR para)
        {
            return db.ExecSql(sqlstr, transaction, para, CommandTimeOut);
        }
        public int ExecSql(string sqlstr, ST transaction, IEnumerable<PR> paras)
        {
            return db.ExecSql(sqlstr, transaction, paras, CommandTimeOut);
        }
        /// <summary>
        /// 得到数据表主键
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static string GetTableKeyName(Type t)
        {
            foreach (PropertyInfo pInfo in CacheHelper.ObjectProperties(t.GetType().FullName))
            {
                if (pInfo.GetCustomAttributes<System.ComponentModel.DataAnnotations.KeyAttribute>().Count() > 0)
                {
                    return pInfo.Name;
                }
            }
            return "fid";
        }
        /// <summary>
        /// 实体对象新增到数据库
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="UpdateCols"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int Add(object Entity, IEnumerable<string> UpdateCols = null, string TableName = null, ST transaction = null)
        {
            return Add(Entity, UpdateCols, TableName, this, transaction);
        }
        /// <summary>
        /// 多个实体对象新增到数据库
        /// </summary>
        /// <param name="ListEntity"></param>
        /// <param name="UpdateCols"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int AddMore(IEnumerable<object> Entitys, IEnumerable<string> UpdateCols = null, string TableName = null, ST transaction = null)
        {
            return AddMore(Entitys, UpdateCols, TableName, this, transaction);
        }
        /// <summary>
        /// 实体对象更新
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="KeyName"></param>
        /// <param name="UpdateCols"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int UpdateByKey(object Entity, string KeyName = null, IEnumerable<string> UpdateCols = null, string TableName = null, ST transaction = null)
        {
            return UpdateByKey(Entity, KeyName, UpdateCols, TableName, this, transaction);
        }
        /// <summary>
        /// 多个实体对象更新
        /// </summary>
        /// <param name="ListEntity"></param>
        /// <param name="KeyName"></param>
        /// <param name="UpdateCols"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int UpdateMore(IEnumerable<object> Entitys, string KeyName = null, IEnumerable<string> UpdateCols = null, string TableName = null, ST transaction = null)
        {
            return UpdateMore(Entitys, KeyName, UpdateCols, TableName, this, transaction);
        }
        /// <summary>
        /// 实体对象删除
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="KeyName"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int Delete(object Entity, string KeyName = null, string TableName = null, ST transaction = null)
        {
            return Delete(Entity, KeyName, TableName, this, transaction);
        }
        /// <summary>
        /// 多个实体对象删除
        /// </summary>
        /// <param name="ListEntity"></param>
        /// <param name="KeyName"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int DeleteMore(IEnumerable<object> Entitys, string KeyName = null, string TableName = null, ST transaction = null)
        {
            return DeleteMore(Entitys, KeyName, TableName, this, transaction);
        }
        /// <summary>
        /// 查询单个实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="KeyValue"></param>
        /// <param name="KeyName"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public T SelectOne<T>(string KeyValue, IEnumerable<string> SelectCols = null, string KeyName = null, string TableName = null, ST transaction = null) where T : new()
        {
            return SelectOne<T>(KeyValue, SelectCols, KeyName, TableName, this, transaction);
        }
        /// <summary>
        /// 通过SQL查询单个实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="QuerySQL"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public T SelectOneBySQL<T>(string QuerySQL, ST transaction = null) where T : new()
        {
            return SelectOneBySQL<T>(QuerySQL, this, transaction);
        }
        public Dictionary<string, object> SelectOneBySQLForDic(string QuerySQL, ST transaction = null)
        {
            return SelectOneBySQLForDic(QuerySQL, this, transaction);
        }
        public Tuple<T1, T2> SelectOneBySQLForTuple<T1, T2>(string QuerySQL, ST transaction = null) where T1 : new() where T2 : new()
        {
            return SelectOneBySQLForTuple<T1, T2>(QuerySQL, this, transaction);
        }
        public Tuple<T1, T2, T3> SelectOneBySQLForTuple<T1, T2, T3>(string QuerySQL, ST transaction = null) where T1 : new() where T2 : new() where T3 : new()
        {
            return SelectOneBySQLForTuple<T1, T2, T3>(QuerySQL, this, transaction);
        }
        public Tuple<T1, T2, T3, T4> SelectOneBySQLForTuple<T1, T2, T3, T4>(string QuerySQL, ST transaction = null) where T1 : new() where T2 : new() where T3 : new() where T4 : new()
        {
            return SelectOneBySQLForTuple<T1, T2, T3, T4>(QuerySQL, this, transaction);
        }
        /// <summary>
        /// 查询多个实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="WhereCondition"></param>
        /// <param name="OrderBy"></param>
        /// <param name="OrderType"></param>
        /// <param name="PageSize"></param>
        /// <param name="CurPageNum"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public List<T> SelectList<T>(string WhereCondition, string OrderBy, string OrderType, IEnumerable<string> SelectCols = null, int PageSize = 0, int CurPageNum = 1, string TableName = null, ST transaction = null) where T : new()
        {
            return SelectList<T>(WhereCondition, OrderBy, OrderType, SelectCols, PageSize, CurPageNum, TableName, this, transaction);
        }
        /// <summary>
        /// 通过SQL查询多个实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="QuerySQL"></param>
        /// <param name="OrderBy"></param>
        /// <param name="OrderType"></param>
        /// <param name="PageSize"></param>
        /// <param name="CurPageNum"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public List<T> SelectListBySQL<T>(string QuerySQL, string OrderBy, string OrderType, int PageSize = 0, int CurPageNum = 1, ST transaction = null) where T : new()
        {
            return SelectListBySQL<T>(QuerySQL, OrderBy, OrderType, PageSize, CurPageNum, this, transaction);
        }
        public List<Dictionary<string, object>> SelectListBySQLForDic(string QuerySQL, string OrderBy, string OrderType, int PageSize = 0, int CurPageNum = 1, ST transaction = null)
        {
            return SelectListBySQLForDic(QuerySQL, OrderBy, OrderType, PageSize, CurPageNum,  this, transaction);
        }
        public List<Tuple<T1, T2>> SelectListBySQLForTuple<T1, T2>(string QuerySQL, string OrderBy, string OrderType, int PageSize = 0, int CurPageNum = 1, ST transaction = null) where T1 : new() where T2 : new()
        {
            return SelectListBySQLForTuple<T1, T2>(QuerySQL, OrderBy, OrderType, PageSize, CurPageNum, this, transaction);
        }
        public List<Tuple<T1, T2, T3>> SelectListBySQLForTuple<T1, T2, T3>(string QuerySQL, string OrderBy, string OrderType, int PageSize = 0, int CurPageNum = 1, ST transaction = null) where T1 : new() where T2 : new() where T3 : new()
        {
            return SelectListBySQLForTuple<T1, T2, T3>(QuerySQL, OrderBy, OrderType, PageSize, CurPageNum, this, transaction);
        }
        public List<Tuple<T1, T2, T3, T4>> SelectListBySQLForTuple<T1, T2, T3, T4>(string QuerySQL, string OrderBy, string OrderType, int PageSize = 0, int CurPageNum = 1, ST transaction = null) where T1 : new() where T2 : new() where T3 : new() where T4 : new()
        {
            return SelectListBySQLForTuple<T1, T2, T3, T4>(QuerySQL, OrderBy, OrderType, PageSize, CurPageNum, this, transaction);
        }
        /// <summary>
        /// 通过SQL查询记录条数
        /// </summary>
        /// <param name="QuerySQL"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int SelectCount(string QuerySQL, ST transaction = null)
        {
            return SelectCount(QuerySQL, this, transaction);
        }
        /// <summary>
        /// 通过实体类型和条件查询记录条数
        /// </summary>
        /// <param name="t"></param>
        /// <param name="WhereCondition"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int SelectCount(Type t, string WhereCondition, string TableName = null, ST transaction = null)
        {
            return SelectCount(t, WhereCondition, TableName, this, transaction);
        }
        /// <summary>
        /// 通过实体类型和条件查询记录条数
        /// </summary>
        /// <param name="t"></param>
        /// <param name="WhereCondition"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int SelectCount<T>(string WhereCondition, string TableName = null, ST transaction = null) where T : new()
        {
            return SelectCount<T>(WhereCondition, TableName, this, transaction);
        }
        /// <summary>
        /// 实体对象新增到数据库
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="UpdateCols"></param>
        /// <param name="TableName"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int Add(object Entity, IEnumerable<string> UpdateCols = null, string TableName = null, DB db = null, ST transaction = null)
        {
            TableName = TableName ?? Entity.GetType().Name;
            string sql = "insert into " + TableName.Replace("'", "") + " ";
            StringBuilder cols = new StringBuilder();
            StringBuilder vals = new StringBuilder();
            List<PR> prs = new List<PR>();
            foreach (System.Reflection.PropertyInfo item in CacheHelper.ObjectProperties(Entity.GetType().FullName))
            {
                if (UpdateCols == null || UpdateCols.Where(s => s.ToLower() == item.Name.ToLower()).Count() > 0)
                {
                    cols.Append("," + item.Name);
                    vals.Append(",@" + item.Name);
                    prs.Add(new PR("@" + item.Name, item.GetValue(Entity)));
                }
            }
            if (prs.Count <= 0) return 0;
            sql += "(" + cols.ToString().Substring(1) + ")values(" + vals.ToString().Substring(1) + ")";
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                        return db.ExecSql(sql, prs.ToArray());
                    else
                        return db.ExecSql(sql, transaction, prs.ToArray());
                }
            }
            else
            {
                if (transaction == null)
                    return db.ExecSql(sql, prs.ToArray());
                else
                    return db.ExecSql(sql, transaction, prs.ToArray());
            }
        }
        /// <summary>
        /// 多个实体对象新增到数据库
        /// </summary>
        /// <param name="ListEntity"></param>
        /// <param name="UpdateCols"></param>
        /// <param name="TableName"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int AddMore(IEnumerable<object> Entitys, IEnumerable<string> UpdateCols = null, string TableName = null, DB db = null, ST transaction = null)
        {
            List<string> sqls = new List<string>();
            List<PR[]> prss = new List<PR[]>();
            foreach (object obj in Entitys)
            {
                TableName = TableName ?? obj.GetType().Name;
                string sql = "insert into " + TableName.Replace("'", "") + " ";
                StringBuilder cols = new StringBuilder();
                StringBuilder vals = new StringBuilder();
                List<PR> prs = new List<PR>();
                foreach (System.Reflection.PropertyInfo item in CacheHelper.ObjectProperties(obj.GetType().FullName))
                {
                    if (UpdateCols == null || UpdateCols.Where(s => s.ToLower() == item.Name.ToLower()).Count() > 0)
                    {
                        cols.Append("," + item.Name);
                        vals.Append(",@" + item.Name);
                        prs.Add(new PR("@" + item.Name, item.GetValue(obj)));
                    }
                }
                if (prs.Count <= 0) continue;
                sql += "(" + cols.ToString().Substring(1) + ")values(" + vals.ToString().Substring(1) + ")";
                sqls.Add(sql);
                prss.Add(prs.ToArray());
            }

            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        int ri = 0;
                        for (int i = 0; i < sqls.Count; i++)
                        {
                            ri += db.ExecSql(sqls[i], prss[i]);
                        }
                        return ri;
                    }
                    else
                    {
                        int ri = 0;
                        for (int i = 0; i < sqls.Count; i++)
                        {
                            ri += db.ExecSql(sqls[i], transaction, prss[i]);
                        }
                        return ri;
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    int ri = 0;
                    for (int i = 0; i < sqls.Count; i++)
                    {
                        ri += db.ExecSql(sqls[i], prss[i]);
                    }
                    return ri;
                }
                else
                {
                    int ri = 0;
                    for (int i = 0; i < sqls.Count; i++)
                    {
                        ri += db.ExecSql(sqls[i], transaction, prss[i]);
                    }
                    return ri;
                }
            }
        }
        /// <summary>
        /// 实体对象更新
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="KeyName"></param>
        /// <param name="UpdateCols"></param>
        /// <param name="TableName"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int UpdateByKey(object Entity, string KeyName = null, IEnumerable<string> UpdateCols = null, string TableName = null, DB db = null, ST transaction = null)
        {
            KeyName = KeyName ?? GetTableKeyName(Entity.GetType());
            TableName = TableName ?? Entity.GetType().Name;
            StringBuilder sql = new StringBuilder();
            sql.Append("update " + TableName.Replace("'", "") + " set ");
            string keyValue = "";
            int loop = 0;
            List<PR> prs = new List<PR>();
            foreach (System.Reflection.PropertyInfo item in CacheHelper.ObjectProperties(Entity.GetType().FullName))
            {
                if (item.Name.ToLower() != KeyName.ToLower())
                {
                    if (UpdateCols == null || UpdateCols.Where(s => s.ToLower() == item.Name.ToLower()).Count() > 0)
                    {
                        if (loop > 0) sql.Append(",");
                        sql.Append(item.Name + "=@" + item.Name);
                        prs.Add(new PR("@" + item.Name, item.GetValue(Entity)));
                        loop++;
                    }
                }
                else
                {
                    keyValue = item.GetValue(Entity).ToString();
                }
            }
            if (loop == 0) return 0;
            sql.Append(" where " + KeyName + "='" + keyValue.Replace("'", "''") + "'");
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                        return db.ExecSql(sql.ToString(), prs.ToArray());
                    else
                        return db.ExecSql(sql.ToString(), transaction, prs.ToArray());
                }
            }
            else
            {
                if (transaction == null)
                    return db.ExecSql(sql.ToString(), prs.ToArray());
                else
                    return db.ExecSql(sql.ToString(), transaction, prs.ToArray());
            }
        }
        /// <summary>
        /// 根据条件更新多条数据
        /// </summary>
        /// <param name="TempEntity"></param>
        /// <param name="whereSql"></param>
        /// <param name="UpdateCols"></param>
        /// <param name="KeyName"></param>
        /// <param name="TableName"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int UpdateByWhere(object TempEntity, string whereSql, IEnumerable<string> UpdateCols, string KeyName = null, string TableName = null, DB db = null, ST transaction = null)
        {
            //whereSql = whereSql.Replace("'", "''");
            KeyName = KeyName ?? GetTableKeyName(TempEntity.GetType());
            TableName = TableName ?? TempEntity.GetType().Name;
            StringBuilder sql = new StringBuilder();
            sql.Append("update " + TableName.Replace("'", "") + " set ");
            int loop = 0;
            List<PR> prs = new List<PR>();
            foreach (System.Reflection.PropertyInfo item in CacheHelper.ObjectProperties(TempEntity.GetType().FullName))
            {
                if (item.Name.ToLower() != KeyName.ToLower())
                {
                    if (UpdateCols == null || UpdateCols.Where(s => s.ToLower() == item.Name.ToLower()).Count() > 0)
                    {
                        if (loop > 0) sql.Append(",");
                        sql.Append(item.Name + "=@" + item.Name);
                        prs.Add(new PR("@" + item.Name, item.GetValue(TempEntity)));
                        loop++;
                    }
                }
            }
            if (loop == 0) return 0;
            sql.Append(" " + whereSql);
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                        return db.ExecSql(sql.ToString(), prs.ToArray());
                    else
                        return db.ExecSql(sql.ToString(), transaction, prs.ToArray());
                }
            }
            else
            {
                if (transaction == null)
                    return db.ExecSql(sql.ToString(), prs.ToArray());
                else
                    return db.ExecSql(sql.ToString(), transaction, prs.ToArray());
            }
        }
        /// <summary>
        /// 根据条件更新多条数据
        /// </summary>
        /// <param name="TempEntity"></param>
        /// <param name="whereSql"></param>
        /// <param name="UpdateCols"></param>
        /// <param name="KeyName"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int UpdateByWhere(object TempEntity, string whereSql, IEnumerable<string> UpdateCols, string KeyName = null, string TableName = null, ST transaction = null)
        {
            return UpdateByWhere(TempEntity, whereSql, UpdateCols, KeyName, TableName, this, transaction);
        }
        /// <summary>
        /// 根据Lamda表达式的where条件更新多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TempEntity"></param>
        /// <param name="where"></param>
        /// <param name="UpdateCols"></param>
        /// <param name="KeyName"></param>
        /// <param name="TableName"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int Update<T>(object TempEntity, Expression<Func<T, bool>> where, IEnumerable<string> UpdateCols, string KeyName = null, string TableName = null, DB db = null, ST transaction = null) where T : class, new()
        {
            T t = new T();
            TableName = TableName ?? t.GetType().Name;
            string whereSql = LambdaToSqlHelper.GetWhereSql<T>(where);
            return UpdateByWhere(TempEntity, whereSql, UpdateCols, KeyName, TableName, db, transaction);
        }
        /// <summary>
        /// 根据Lamda表达式的where条件更新多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TempEntity"></param>
        /// <param name="where"></param>
        /// <param name="UpdateCols"></param>
        /// <param name="KeyName"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int Update<T>(object TempEntity, Expression<Func<T, bool>> where, IEnumerable<string> UpdateCols, string KeyName = null, string TableName = null, ST transaction = null) where T : class, new()
        {
            return Update<T>(TempEntity, where, UpdateCols, KeyName, TableName, this, transaction);
        }
        /// <summary>
        /// 多个实体对象更新
        /// </summary>
        /// <param name="ListEntity"></param>
        /// <param name="KeyName"></param>
        /// <param name="UpdateCols"></param>
        /// <param name="TableName"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int UpdateMore(IEnumerable<object> Entitys, string KeyName = null, IEnumerable<string> UpdateCols = null, string TableName = null, DB db = null, ST transaction = null)
        {
            if (Entitys.Count() > 0) KeyName = KeyName ?? GetTableKeyName(Entitys.First().GetType());
            List<string> sqls = new List<string>();
            List<PR[]> prss = new List<PR[]>();
            foreach (object obj in Entitys)
            {
                TableName = TableName ?? obj.GetType().Name;
                StringBuilder sql = new StringBuilder();
                sql.Append("update " + TableName.Replace("'", "") + " set ");
                string keyValue = "";
                int loop = 0;
                List<PR> prs = new List<PR>();
                foreach (System.Reflection.PropertyInfo item in CacheHelper.ObjectProperties(obj.GetType().FullName))
                {
                    if (item.Name.ToLower() != KeyName.ToLower())
                    {
                        if (UpdateCols == null || UpdateCols.Where(s => s.ToLower() == item.Name.ToLower()).Count() > 0)
                        {
                            if (loop > 0) sql.Append(",");
                            sql.Append(item.Name + "=@" + item.Name);
                            prs.Add(new PR("@" + item.Name, item.GetValue(obj)));
                            loop++;
                        }
                    }
                    else
                    {
                        keyValue = item.GetValue(obj).ToString();
                    }
                }
                if (loop == 0) continue;
                sql.Append(" where " + KeyName + "='" + keyValue.Replace("'", "''") + "'");
                sqls.Add(sql.ToString());
                prss.Add(prs.ToArray());
            }

            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        int ri = 0;
                        for (int i = 0; i < sqls.Count; i++)
                        {
                            ri += db.ExecSql(sqls[i], prss[i]);
                        }
                        return ri;
                    }
                    else
                    {
                        int ri = 0;
                        for (int i = 0; i < sqls.Count; i++)
                        {
                            ri += db.ExecSql(sqls[i], transaction, prss[i]);
                        }
                        return ri;
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    int ri = 0;
                    for (int i = 0; i < sqls.Count; i++)
                    {
                        ri += db.ExecSql(sqls[i], prss[i]);
                    }
                    return ri;
                }
                else
                {
                    int ri = 0;
                    for (int i = 0; i < sqls.Count; i++)
                    {
                        ri += db.ExecSql(sqls[i], transaction, prss[i]);
                    }
                    return ri;
                }
            }
        }
        /// <summary>
        /// 实体对象删除
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="KeyName"></param>
        /// <param name="TableName"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int Delete(object Entity, string KeyName = null, string TableName = null, DB db = null, ST transaction = null)
        {
            KeyName = KeyName ?? GetTableKeyName(Entity.GetType());
            TableName = TableName ?? Entity.GetType().Name;
            string keyValue = "";
            foreach (System.Reflection.PropertyInfo item in CacheHelper.ObjectProperties(Entity.GetType().FullName))
            {
                if (item.Name.ToLower() == KeyName.ToLower())
                {
                    keyValue = item.GetValue(Entity).ToString();
                }
            }
            string sql = "delete from " + TableName.Replace("'", "") + "  where " + KeyName + "='" + keyValue.Replace("'", "''") + "'";
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                        return db.ExecSql(sql.ToString());
                    else
                        return db.ExecSql(sql.ToString(), transaction);
                }
            }
            else
            {
                if (transaction == null)
                    return db.ExecSql(sql.ToString());
                else
                    return db.ExecSql(sql.ToString(), transaction);
            }
        }
        /// <summary>
        /// 根据Lamda表达式的where条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="TableName"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int Delete<T>(Expression<Func<T, bool>> where, string TableName = null, DB db = null, ST transaction = null) where T : class, new()
        {
            T t = new T();
            TableName = TableName ?? t.GetType().Name;
            string whereSql = LambdaToSqlHelper.GetWhereSql<T>(where);
            return Delete<T>(whereSql, TableName, db, transaction);
        }
        /// <summary>
        /// 根据Lamda表达式的where条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int Delete<T>(Expression<Func<T, bool>> where, string TableName = null, ST transaction = null) where T : class, new()
        {
            return Delete<T>(where, TableName, this, transaction);
        }
        /// <summary>
        /// 根据where条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereSql"></param>
        /// <param name="TableName"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int Delete<T>(string whereSql, string TableName = null, DB db = null, ST transaction = null) where T : class, new()
        {
            T t = new T();
            //whereSql = whereSql.Replace("'", "''");
            TableName = TableName ?? t.GetType().Name;
            string sql = "delete from " + TableName.Replace("'", "") + "  " + whereSql;
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                        return db.ExecSql(sql.ToString());
                    else
                        return db.ExecSql(sql.ToString(), transaction);
                }
            }
            else
            {
                if (transaction == null)
                    return db.ExecSql(sql.ToString());
                else
                    return db.ExecSql(sql.ToString(), transaction);
            }
        }
        /// <summary>
        /// 根据where条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereSql"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int Delete<T>(string whereSql, string TableName = null, ST transaction = null) where T : class, new()
        {
            return Delete<T>(whereSql, TableName, this, transaction);
        }
        /// <summary>
        /// 多个实体对象删除
        /// </summary>
        /// <param name="ListEntity"></param>
        /// <param name="KeyName"></param>
        /// <param name="TableName"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int DeleteMore(IEnumerable<object> Entitys, string KeyName = null, string TableName = null, DB db = null, ST transaction = null)
        {
            if (Entitys.Count() > 0) KeyName = KeyName ?? GetTableKeyName(Entitys.First().GetType());
            List<string> sqls = new List<string>();
            foreach (object obj in Entitys)
            {
                TableName = TableName ?? obj.GetType().Name;
                string keyValue = "";
                List<PR> prs = new List<PR>();
                foreach (System.Reflection.PropertyInfo item in CacheHelper.ObjectProperties(obj.GetType().FullName))
                {
                    if (item.Name.ToLower() == KeyName.ToLower())
                    {
                        keyValue = item.GetValue(obj).ToString();
                    }
                }
                string sql = "delete from " + TableName.Replace("'", "") + "  where " + KeyName + "='" + keyValue.Replace("'", "''") + "'";
                sqls.Add(sql.ToString());
            }

            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        int ri = 0;
                        for (int i = 0; i < sqls.Count; i++)
                        {
                            ri += db.ExecSql(sqls[i]);
                        }
                        return ri;
                    }
                    else
                    {
                        int ri = 0;
                        for (int i = 0; i < sqls.Count; i++)
                        {
                            ri += db.ExecSql(sqls[i], transaction);
                        }
                        return ri;
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    int ri = 0;
                    for (int i = 0; i < sqls.Count; i++)
                    {
                        ri += db.ExecSql(sqls[i]);
                    }
                    return ri;
                }
                else
                {
                    int ri = 0;
                    for (int i = 0; i < sqls.Count; i++)
                    {
                        ri += db.ExecSql(sqls[i], transaction);
                    }
                    return ri;
                }
            }
        }
        /// <summary>
        /// 查询单个实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="KeyValue"></param>
        /// <param name="KeyName"></param>
        /// <param name="TableName"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static T SelectOne<T>(string KeyValue, IEnumerable<string> SelectCols = null, string KeyName = null, string TableName = null, DB db = null, ST transaction = null) where T : new()
        {
            T t = new T();
            bool HasValue = false;
            TableName = TableName ?? t.GetType().Name;
            KeyName = KeyName ?? GetTableKeyName(t.GetType());
            KeyValue = KeyValue ?? "";
            var cols = SelectCols == null ? "*" : string.Join(",", SelectCols);
            string sql = "select "+ cols + " from " + TableName.Replace("'", "") + " where " + KeyName + "='" + KeyValue.Replace("'", "''") + "'";
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        using (DR dr = db.OpenRecord(sql))
                        {
                            if (dr.Read())
                            {
                                t = DRToType<T>(dr);
                                HasValue = true;
                            }
                        }
                    }
                    else
                    {
                        using (DR dr = db.OpenRecord(sql, transaction))
                        {
                            if (dr.Read())
                            {
                                t = DRToType<T>(dr);
                                HasValue = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    using (DR dr = db.OpenRecord(sql))
                    {
                        if (dr.Read())
                        {
                            t = DRToType<T>(dr);
                            HasValue = true;
                        }
                    }
                }
                else
                {
                    using (DR dr = db.OpenRecord(sql, transaction))
                    {
                        if (dr.Read())
                        {
                            t = DRToType<T>(dr);
                            HasValue = true;
                        }
                    }
                }
            }
            return HasValue ? t : default(T);
        }
        /// <summary>
        /// 根据Lamda表达式的where条件查询单个实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="TableName"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static T SelectOne<T>(Expression<Func<T, bool>> where, IEnumerable<string> SelectCols = null, string TableName = null, DB db = null, ST transaction = null) where T : class, new()
        {
            T t = new T();
            bool HasValue = false;
            TableName = TableName ?? t.GetType().Name;
            string whereSql = LambdaToSqlHelper.GetWhereSql<T>(where);
            //whereSql = whereSql.Replace("'", "''");
            var cols = SelectCols == null ? "*" : string.Join(",", SelectCols);
            string sql = "select "+ cols + " from " + TableName.Replace("'", "") + " " + whereSql;
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        using (DR dr = db.OpenRecord(sql))
                        {
                            if (dr.Read())
                            {
                                t = DRToType<T>(dr);
                                HasValue = true;
                            }
                        }
                    }
                    else
                    {
                        using (DR dr = db.OpenRecord(sql, transaction))
                        {
                            if (dr.Read())
                            {
                                t = DRToType<T>(dr);
                                HasValue = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    using (DR dr = db.OpenRecord(sql))
                    {
                        if (dr.Read())
                        {
                            t = DRToType<T>(dr);
                            HasValue = true;
                        }
                    }
                }
                else
                {
                    using (DR dr = db.OpenRecord(sql, transaction))
                    {
                        if (dr.Read())
                        {
                            t = DRToType<T>(dr);
                            HasValue = true;
                        }
                    }
                }
            }
            return HasValue ? t : default(T);
        }
        /// <summary>
        /// 根据Lamda表达式的where条件查询单个实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public T SelectOne<T>(Expression<Func<T, bool>> where, IEnumerable<string> SelectCols = null, string TableName = null, ST transaction = null) where T : class, new()
        {
            return SelectOne<T>(where, SelectCols, TableName, this, transaction);
        }
        /// <summary>
        /// 通过SQL查询单个实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="QuerySQL"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static T SelectOneBySQL<T>(string QuerySQL, DB db = null, ST transaction = null) where T : new()
        {
            T t = new T();
            bool HasValue = false;
            string sql = QuerySQL;
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        using (DR dr = db.OpenRecord(sql))
                        {
                            if (dr.Read())
                            {
                                t = DRToType<T>(dr);
                                HasValue = true;
                            }
                        }
                    }
                    else
                    {
                        using (DR dr = db.OpenRecord(sql, transaction))
                        {
                            if (dr.Read())
                            {
                                t = DRToType<T>(dr);
                                HasValue = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    using (DR dr = db.OpenRecord(sql))
                    {
                        if (dr.Read())
                        {
                            t = DRToType<T>(dr);
                            HasValue = true;
                        }
                    }
                }
                else
                {
                    using (DR dr = db.OpenRecord(sql, transaction))
                    {
                        if (dr.Read())
                        {
                            t = DRToType<T>(dr);
                            HasValue = true;
                        }
                    }
                }
            }
            return HasValue ? t : default(T);
        }
        /// <summary>
        /// 获取数据的字典集
        /// </summary>
        /// <param name="QuerySQL"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static Dictionary<string, object> SelectOneBySQLForDic(string QuerySQL, DB db = null, ST transaction = null)
        {
            bool HasValue = false;
            Dictionary<string, object> dic = null;
            string sql = QuerySQL;
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        using (DR dr = db.OpenRecord(sql))
                        {
                            if (dr.Read())
                            {
                                dic = DRToDic(dr);
                                HasValue = true;
                            }
                        }
                    }
                    else
                    {
                        using (DR dr = db.OpenRecord(sql, transaction))
                        {
                            if (dr.Read())
                            {
                                dic = DRToDic(dr);
                                HasValue = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    using (DR dr = db.OpenRecord(sql))
                    {
                        if (dr.Read())
                        {
                            dic = DRToDic(dr);
                            HasValue = true;
                        }
                    }
                }
                else
                {
                    using (DR dr = db.OpenRecord(sql, transaction))
                    {
                        if (dr.Read())
                        {
                            dic = DRToDic(dr);
                            HasValue = true;
                        }
                    }
                }
            }
            return HasValue ? dic : null;
        }
        /// <summary>
        /// 通过复杂SQL查询单条多个实体对象
        /// </summary>
        /// <typeparam name="Tuple"></typeparam>
        /// <param name="QuerySQL"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static Tuple<T1, T2> SelectOneBySQLForTuple<T1, T2>(string QuerySQL, DB db = null, ST transaction = null) where T1 : new() where T2 : new()
        {
            T1 t1 = new T1();
            T2 t2 = new T2();
            bool HasValue = false;
            string sql = QuerySQL;
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        using (DR dr = db.OpenRecord(sql))
                        {
                            if (dr.Read())
                            {
                                t1 = DRToType<T1>(dr);
                                t2 = DRToType<T2>(dr);
                                HasValue = true;
                            }
                        }
                    }
                    else
                    {
                        using (DR dr = db.OpenRecord(sql, transaction))
                        {
                            if (dr.Read())
                            {
                                t1 = DRToType<T1>(dr);
                                t2 = DRToType<T2>(dr);
                                HasValue = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    using (DR dr = db.OpenRecord(sql))
                    {
                        if (dr.Read())
                        {
                            t1 = DRToType<T1>(dr);
                            t2 = DRToType<T2>(dr);
                            HasValue = true;
                        }
                    }
                }
                else
                {
                    using (DR dr = db.OpenRecord(sql, transaction))
                    {
                        if (dr.Read())
                        {
                            t1 = DRToType<T1>(dr);
                            t2 = DRToType<T2>(dr);
                            HasValue = true;
                        }
                    }
                }
            }
            return HasValue ? new Tuple<T1, T2>(t1, t2) : null;
        }
        public static Tuple<T1, T2, T3> SelectOneBySQLForTuple<T1, T2, T3>(string QuerySQL, DB db = null, ST transaction = null) where T1 : new() where T2 : new() where T3 : new()
        {
            T1 t1 = new T1();
            T2 t2 = new T2();
            T3 t3 = new T3();
            bool HasValue = false;
            string sql = QuerySQL;
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        using (DR dr = db.OpenRecord(sql))
                        {
                            if (dr.Read())
                            {
                                t1 = DRToType<T1>(dr);
                                t2 = DRToType<T2>(dr);
                                t3 = DRToType<T3>(dr);
                                HasValue = true;
                            }
                        }
                    }
                    else
                    {
                        using (DR dr = db.OpenRecord(sql, transaction))
                        {
                            if (dr.Read())
                            {
                                t1 = DRToType<T1>(dr);
                                t2 = DRToType<T2>(dr);
                                t3 = DRToType<T3>(dr);
                                HasValue = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    using (DR dr = db.OpenRecord(sql))
                    {
                        if (dr.Read())
                        {
                            t1 = DRToType<T1>(dr);
                            t2 = DRToType<T2>(dr);
                            t3 = DRToType<T3>(dr);
                            HasValue = true;
                        }
                    }
                }
                else
                {
                    using (DR dr = db.OpenRecord(sql, transaction))
                    {
                        if (dr.Read())
                        {
                            t1 = DRToType<T1>(dr);
                            t2 = DRToType<T2>(dr);
                            t3 = DRToType<T3>(dr);
                            HasValue = true;
                        }
                    }
                }
            }
            return HasValue ? new Tuple<T1, T2, T3>(t1, t2, t3) : null;
        }
        public static Tuple<T1, T2, T3, T4> SelectOneBySQLForTuple<T1, T2, T3, T4>(string QuerySQL, DB db = null, ST transaction = null) where T1 : new() where T2 : new() where T3 : new() where T4 : new()
        {
            T1 t1 = new T1();
            T2 t2 = new T2();
            T3 t3 = new T3();
            T4 t4 = new T4();
            bool HasValue = false;
            string sql = QuerySQL;
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        using (DR dr = db.OpenRecord(sql))
                        {
                            if (dr.Read())
                            {
                                t1 = DRToType<T1>(dr);
                                t2 = DRToType<T2>(dr);
                                t3 = DRToType<T3>(dr);
                                t4 = DRToType<T4>(dr);
                                HasValue = true;
                            }
                        }
                    }
                    else
                    {
                        using (DR dr = db.OpenRecord(sql, transaction))
                        {
                            if (dr.Read())
                            {
                                t1 = DRToType<T1>(dr);
                                t2 = DRToType<T2>(dr);
                                t3 = DRToType<T3>(dr);
                                t4 = DRToType<T4>(dr);
                                HasValue = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    using (DR dr = db.OpenRecord(sql))
                    {
                        if (dr.Read())
                        {
                            t1 = DRToType<T1>(dr);
                            t2 = DRToType<T2>(dr);
                            t3 = DRToType<T3>(dr);
                            t4 = DRToType<T4>(dr);
                            HasValue = true;
                        }
                    }
                }
                else
                {
                    using (DR dr = db.OpenRecord(sql, transaction))
                    {
                        if (dr.Read())
                        {
                            t1 = DRToType<T1>(dr);
                            t2 = DRToType<T2>(dr);
                            t3 = DRToType<T3>(dr);
                            t4 = DRToType<T4>(dr);
                            HasValue = true;
                        }
                    }
                }
            }
            return HasValue ? new Tuple<T1, T2, T3, T4>(t1, t2, t3, t4) : null;
        }
        /// <summary>
        /// 查询多个实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="WhereCondition"></param>
        /// <param name="OrderBy"></param>
        /// <param name="OrderType"></param>
        /// <param name="PageSize"></param>
        /// <param name="CurPageNum"></param>
        /// <param name="TableName"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static List<T> SelectList<T>(string WhereCondition, string OrderBy, string OrderType, IEnumerable<string> SelectCols = null, int PageSize = 0, int CurPageNum = 1, string TableName = null, DB db = null, ST transaction = null) where T : new()
        {
            T t = new T();
            //WhereCondition = " " + WhereCondition.Replace("'", "''");
            TableName = TableName ?? t.GetType().Name;
            //if (WhereCondition.Trim().StartsWith("where")) WhereCondition = WhereCondition.Trim().Substring(5);
            //if (WhereCondition.Trim() != "") WhereCondition = " where " + WhereCondition;
            string sql = "not correct database type";
            var cols = SelectCols == null ? "*" : string.Join(",", SelectCols);
            if (DBConst.DataBaseType.Equals(DataBase.MySql))
            {
                sql = "select "+ cols + " from " + TableName.Replace("'", "") + "  " + WhereCondition + "  order by " + OrderBy.Replace("'", "") + " " + OrderType.Replace("'", "");
                if (PageSize > 0)
                {
                    sql += " limit " + PageSize * (CurPageNum - 1) + "," + PageSize;
                }
            }
            else if (DBConst.DataBaseType.Equals(DataBase.SqlServer))
            {
                sql = "select "+ cols + " from " + TableName.Replace("'", "") + "  " + WhereCondition;
                string orderbystr = "order by " + OrderBy + " " + OrderType;
                if (PageSize > 0)
                {
                    sql = "select * from (select Row_Number() over (" + orderbystr + ") as ft_rownum,* from (" + sql + ") as a) as ftlistt where ft_rownum between " + (PageSize * (CurPageNum - 1) + 1) + " and " + (PageSize * CurPageNum) + "";
                }
                else
                {
                    sql += " " + orderbystr;
                }
            }
            else if (DBConst.DataBaseType.Equals(DataBase.Sqlite))
            {
                sql = "select "+ cols + " from " + TableName.Replace("'", "") + "  " + WhereCondition + "  order by " + OrderBy.Replace("'", "") + " " + OrderType.Replace("'", "");
                if (PageSize > 0)
                {
                    sql += " limit " + PageSize + " offset " + (PageSize * (CurPageNum - 1));
                }
            }
            List<T> ListRecord = new List<T>();
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        using (DR dr = db.OpenRecord(sql))
                        {
                            while (dr.Read())
                            {
                                ListRecord.Add(DRToType<T>(dr));
                            }
                        }
                    }
                    else
                    {
                        using (DR dr = db.OpenRecord(sql, transaction))
                        {
                            while (dr.Read())
                            {
                                ListRecord.Add(DRToType<T>(dr));
                            }
                        }
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    using (DR dr = db.OpenRecord(sql))
                    {
                        while (dr.Read())
                        {
                            ListRecord.Add(DRToType<T>(dr));
                        }
                    }
                }
                else
                {
                    using (DR dr = db.OpenRecord(sql, transaction))
                    {
                        while (dr.Read())
                        {
                            ListRecord.Add(DRToType<T>(dr));
                        }
                    }
                }
            }
            return ListRecord;
        }
        /// <summary>
        /// 根据Lamda表达式的查询多个实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="PageSize"></param>
        /// <param name="CurPageNum"></param>
        /// <param name="TableName"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static List<T> SelectList<T>(Expression<Func<T, bool>> where, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy, IEnumerable<string> SelectCols = null, int PageSize = 0, int CurPageNum = 1, string TableName = null, DB db = null, ST transaction = null) where T : class, new()
        {
            T t = new T();
            TableName = TableName ?? t.GetType().Name;
            string whereSql = LambdaToSqlHelper.GetWhereSql<T>(where);
            string orderbySql = LambdaToSqlHelper.GetOrderBySql<T>(orderBy, "");
            string sql = "not correct database type";
            var cols = SelectCols == null ? "*" : string.Join(",", SelectCols);
            if (DBConst.DataBaseType.Equals(DataBase.MySql))
            {
                sql = "select "+ cols + " from " + TableName.Replace("'", "") + "  " + whereSql + " " + orderbySql;
                if (PageSize > 0)
                {
                    sql += " limit " + PageSize * (CurPageNum - 1) + "," + PageSize;
                }
            }
            else if (DBConst.DataBaseType.Equals(DataBase.SqlServer))
            {
                sql = "select "+ cols + " from " + TableName.Replace("'", "") + " " + whereSql;
                string orderbystr = orderbySql;
                if (PageSize > 0)
                {
                    sql = "select * from (select Row_Number() over (" + orderbystr + ") as ft_rownum,* from (" + sql + ") as a) as ftlistt where ft_rownum between " + (PageSize * (CurPageNum - 1) + 1) + " and " + (PageSize * CurPageNum) + "";
                }
                else
                {
                    sql += " " + orderbystr;
                }
            }
            else if (DBConst.DataBaseType.Equals(DataBase.Sqlite))
            {
                sql = "select "+ cols + " from " + TableName.Replace("'", "") + "  " + whereSql + " " + orderbySql;
                if (PageSize > 0)
                {
                    sql += " limit " + PageSize + " offset " + (PageSize * (CurPageNum - 1));
                }
            }
            List<T> ListRecord = new List<T>();
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        using (DR dr = db.OpenRecord(sql))
                        {
                            while (dr.Read())
                            {
                                ListRecord.Add(DRToType<T>(dr));
                            }
                        }
                    }
                    else
                    {
                        using (DR dr = db.OpenRecord(sql, transaction))
                        {
                            while (dr.Read())
                            {
                                ListRecord.Add(DRToType<T>(dr));
                            }
                        }
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    using (DR dr = db.OpenRecord(sql))
                    {
                        while (dr.Read())
                        {
                            ListRecord.Add(DRToType<T>(dr));
                        }
                    }
                }
                else
                {
                    using (DR dr = db.OpenRecord(sql, transaction))
                    {
                        while (dr.Read())
                        {
                            ListRecord.Add(DRToType<T>(dr));
                        }
                    }
                }
            }
            return ListRecord;
        }
        /// <summary>
        /// 根据Lamda表达式的查询多个实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="PageSize"></param>
        /// <param name="CurPageNum"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public List<T> SelectList<T>(Expression<Func<T, bool>> where, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy, IEnumerable<string> SelectCols = null, int PageSize = 0, int CurPageNum = 1, string TableName = null, ST transaction = null) where T : class, new()
        {
            return SelectList<T>(where, orderBy, SelectCols, PageSize, CurPageNum, TableName, this, transaction);
        }
        /// <summary>
        /// 通过SQL查询多个实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="QuerySQL"></param>
        /// <param name="OrderBy"></param>
        /// <param name="OrderType"></param>
        /// <param name="PageSize"></param>
        /// <param name="CurPageNum"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static List<T> SelectListBySQL<T>(string QuerySQL, string OrderBy, string OrderType, int PageSize = 0, int CurPageNum = 1, DB db = null, ST transaction = null) where T : new()
        {
            string sql = "not correct database type";
            if (DBConst.DataBaseType.Equals(DataBase.MySql))
            {
                sql = QuerySQL + "  order by " + OrderBy.Replace("'", "") + " " + OrderType.Replace("'", "");
                if (PageSize > 0)
                {
                    sql += " limit " + PageSize * (CurPageNum - 1) + "," + PageSize;
                }
            }
            else if (DBConst.DataBaseType.Equals(DataBase.SqlServer))
            {
                sql = QuerySQL;
                string orderbystr = "order by " + OrderBy + " " + OrderType;
                if (PageSize > 0)
                {
                    sql = "select * from (select Row_Number() over (" + orderbystr + ") as ft_rownum,* from (" + sql + ") as a) as ftlistt where ft_rownum between " + (PageSize * (CurPageNum - 1) + 1) + " and " + (PageSize * CurPageNum) + "";
                }
                else
                {
                    sql += " " + orderbystr;
                }
            }
            else if (DBConst.DataBaseType.Equals(DataBase.Sqlite))
            {
                sql = QuerySQL + "  order by " + OrderBy.Replace("'", "") + " " + OrderType.Replace("'", "");
                if (PageSize > 0)
                {
                    sql += " limit " + PageSize + " offset " + (PageSize * (CurPageNum - 1));
                }
            }
            List<T> ListRecord = new List<T>();
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        using (DR dr = db.OpenRecord(sql))
                        {
                            while (dr.Read())
                            {
                                ListRecord.Add(DRToType<T>(dr));
                            }
                        }
                    }
                    else
                    {
                        using (DR dr = db.OpenRecord(sql, transaction))
                        {
                            while (dr.Read())
                            {
                                ListRecord.Add(DRToType<T>(dr));
                            }
                        }
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    using (DR dr = db.OpenRecord(sql))
                    {
                        while (dr.Read())
                        {
                            ListRecord.Add(DRToType<T>(dr));
                        }
                    }
                }
                else
                {
                    using (DR dr = db.OpenRecord(sql, transaction))
                    {
                        while (dr.Read())
                        {
                            ListRecord.Add(DRToType<T>(dr));
                        }
                    }
                }
            }
            return ListRecord;
        }
        /// <summary>
        /// 获取多条数据的字典集
        /// </summary>
        /// <param name="QuerySQL"></param>
        /// <param name="OrderBy"></param>
        /// <param name="OrderType"></param>
        /// <param name="PageSize"></param>
        /// <param name="CurPageNum"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> SelectListBySQLForDic(string QuerySQL, string OrderBy, string OrderType, int PageSize = 0, int CurPageNum = 1, DB db = null, ST transaction = null)
        {
            string sql = "not correct database type";
            if (DBConst.DataBaseType.Equals(DataBase.MySql))
            {
                sql = QuerySQL + "  order by " + OrderBy.Replace("'", "") + " " + OrderType.Replace("'", "");
                if (PageSize > 0)
                {
                    sql += " limit " + PageSize * (CurPageNum - 1) + "," + PageSize;
                }
            }
            else if (DBConst.DataBaseType.Equals(DataBase.SqlServer))
            {
                sql = QuerySQL;
                string orderbystr = "order by " + OrderBy + " " + OrderType;
                if (PageSize > 0)
                {
                    sql = "select * from (select Row_Number() over (" + orderbystr + ") as ft_rownum,* from (" + sql + ") as a) as ftlistt where ft_rownum between " + (PageSize * (CurPageNum - 1) + 1) + " and " + (PageSize * CurPageNum) + "";
                }
                else
                {
                    sql += " " + orderbystr;
                }
            }
            else if (DBConst.DataBaseType.Equals(DataBase.Sqlite))
            {
                sql = QuerySQL + "  order by " + OrderBy.Replace("'", "") + " " + OrderType.Replace("'", ""); 
                if (PageSize > 0)
                {
                    sql += " limit " + PageSize + " offset " + (PageSize * (CurPageNum - 1));
                }
            }
            List<Dictionary<string, object>> ListRecord = new List<Dictionary<string, object>>();
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        using (DR dr = db.OpenRecord(sql))
                        {
                            while (dr.Read())
                            {
                                ListRecord.Add(DRToDic(dr));
                            }
                        }
                    }
                    else
                    {
                        using (DR dr = db.OpenRecord(sql, transaction))
                        {
                            while (dr.Read())
                            {
                                ListRecord.Add(DRToDic(dr));
                            }
                        }
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    using (DR dr = db.OpenRecord(sql))
                    {
                        while (dr.Read())
                        {
                            ListRecord.Add(DRToDic(dr));
                        }
                    }
                }
                else
                {
                    using (DR dr = db.OpenRecord(sql, transaction))
                    {
                        while (dr.Read())
                        {
                            ListRecord.Add(DRToDic(dr));
                        }
                    }
                }
            }
            return ListRecord;
        }
        /// <summary>
        /// 通过SQL查询多条多个实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="QuerySQL"></param>
        /// <param name="OrderBy"></param>
        /// <param name="OrderType"></param>
        /// <param name="PageSize"></param>
        /// <param name="CurPageNum"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static List<Tuple<T1, T2>> SelectListBySQLForTuple<T1, T2>(string QuerySQL, string OrderBy, string OrderType, int PageSize = 0, int CurPageNum = 1, DB db = null, ST transaction = null) where T1 : new() where T2 : new()
        {
            string sql = "not correct database type";
            if (DBConst.DataBaseType.Equals(DataBase.MySql))
            {
                sql = QuerySQL + "  order by " + OrderBy.Replace("'", "") + " " + OrderType.Replace("'", "");
                if (PageSize > 0)
                {
                    sql += " limit " + PageSize * (CurPageNum - 1) + "," + PageSize;
                }
            }
            else if (DBConst.DataBaseType.Equals(DataBase.SqlServer))
            {
                sql = QuerySQL;
                string orderbystr = "order by " + OrderBy + " " + OrderType;
                if (PageSize > 0)
                {
                    sql = "select * from (select Row_Number() over (" + orderbystr + ") as ft_rownum,* from (" + sql + ") as a) as ftlistt where ft_rownum between " + (PageSize * (CurPageNum - 1) + 1) + " and " + (PageSize * CurPageNum) + "";
                }
                else
                {
                    sql += " " + orderbystr;
                }
            }
            else if (DBConst.DataBaseType.Equals(DataBase.Sqlite))
            {
                sql = QuerySQL + "  order by " + OrderBy.Replace("'", "") + " " + OrderType.Replace("'", "");
                if (PageSize > 0)
                {
                    sql += " limit " + PageSize + " offset " + (PageSize * (CurPageNum - 1));
                }
            }
            List<Tuple<T1, T2>> ListRecord = new List<Tuple<T1, T2>>();
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        using (DR dr = db.OpenRecord(sql))
                        {
                            while (dr.Read())
                            {
                                ListRecord.Add(new Tuple<T1, T2>(DRToType<T1>(dr), DRToType<T2>(dr)));
                            }
                        }
                    }
                    else
                    {
                        using (DR dr = db.OpenRecord(sql, transaction))
                        {
                            while (dr.Read())
                            {
                                ListRecord.Add(new Tuple<T1, T2>(DRToType<T1>(dr), DRToType<T2>(dr)));
                            }
                        }
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    using (DR dr = db.OpenRecord(sql))
                    {
                        while (dr.Read())
                        {
                            ListRecord.Add(new Tuple<T1, T2>(DRToType<T1>(dr), DRToType<T2>(dr)));
                        }
                    }
                }
                else
                {
                    using (DR dr = db.OpenRecord(sql, transaction))
                    {
                        while (dr.Read())
                        {
                            ListRecord.Add(new Tuple<T1, T2>(DRToType<T1>(dr), DRToType<T2>(dr)));
                        }
                    }
                }
            }
            return ListRecord;
        }
        public static List<Tuple<T1, T2, T3>> SelectListBySQLForTuple<T1, T2, T3>(string QuerySQL, string OrderBy, string OrderType, int PageSize = 0, int CurPageNum = 1, DB db = null, ST transaction = null) where T1 : new() where T2 : new() where T3 : new()
        {
            string sql = "not correct database type";
            if (DBConst.DataBaseType.Equals(DataBase.MySql))
            {
                sql = QuerySQL + "  order by " + OrderBy.Replace("'", "") + " " + OrderType.Replace("'", "");
                if (PageSize > 0)
                {
                    sql += " limit " + PageSize * (CurPageNum - 1) + "," + PageSize;
                }
            }
            else if (DBConst.DataBaseType.Equals(DataBase.SqlServer))
            {
                sql = QuerySQL;
                string orderbystr = "order by " + OrderBy + " " + OrderType;
                if (PageSize > 0)
                {
                    sql = "select * from (select Row_Number() over (" + orderbystr + ") as ft_rownum,* from (" + sql + ") as a) as ftlistt where ft_rownum between " + (PageSize * (CurPageNum - 1) + 1) + " and " + (PageSize * CurPageNum) + "";
                }
                else
                {
                    sql += " " + orderbystr;
                }
            }
            else if (DBConst.DataBaseType.Equals(DataBase.Sqlite))
            {
                sql = QuerySQL + "  order by " + OrderBy.Replace("'", "") + " " + OrderType.Replace("'", "");
                if (PageSize > 0)
                {
                    sql += " limit " + PageSize + " offset " + (PageSize * (CurPageNum - 1));
                }
            }
            List<Tuple<T1, T2, T3>> ListRecord = new List<Tuple<T1, T2, T3>>();
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        using (DR dr = db.OpenRecord(sql))
                        {
                            while (dr.Read())
                            {
                                ListRecord.Add(new Tuple<T1, T2, T3>(DRToType<T1>(dr), DRToType<T2>(dr), DRToType<T3>(dr)));
                            }
                        }
                    }
                    else
                    {
                        using (DR dr = db.OpenRecord(sql, transaction))
                        {
                            while (dr.Read())
                            {
                                ListRecord.Add(new Tuple<T1, T2, T3>(DRToType<T1>(dr), DRToType<T2>(dr), DRToType<T3>(dr)));
                            }
                        }
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    using (DR dr = db.OpenRecord(sql))
                    {
                        while (dr.Read())
                        {
                            ListRecord.Add(new Tuple<T1, T2, T3>(DRToType<T1>(dr), DRToType<T2>(dr), DRToType<T3>(dr)));
                        }
                    }
                }
                else
                {
                    using (DR dr = db.OpenRecord(sql, transaction))
                    {
                        while (dr.Read())
                        {
                            ListRecord.Add(new Tuple<T1, T2, T3>(DRToType<T1>(dr), DRToType<T2>(dr), DRToType<T3>(dr)));
                        }
                    }
                }
            }
            return ListRecord;
        }
        public static List<Tuple<T1, T2, T3, T4>> SelectListBySQLForTuple<T1, T2, T3, T4>(string QuerySQL, string OrderBy, string OrderType, int PageSize = 0, int CurPageNum = 1, DB db = null, ST transaction = null) where T1 : new() where T2 : new() where T3 : new() where T4 : new()
        {
            string sql = "not correct database type";
            if (DBConst.DataBaseType.Equals(DataBase.MySql))
            {
                sql = QuerySQL + "  order by " + OrderBy.Replace("'", "") + " " + OrderType.Replace("'", "");
                if (PageSize > 0)
                {
                    sql += " limit " + PageSize * (CurPageNum - 1) + "," + PageSize;
                }
            }
            else if (DBConst.DataBaseType.Equals(DataBase.SqlServer))
            {
                sql = QuerySQL;
                string orderbystr = "order by " + OrderBy + " " + OrderType;
                if (PageSize > 0)
                {
                    sql = "select * from (select Row_Number() over (" + orderbystr + ") as ft_rownum,* from (" + sql + ") as a) as ftlistt where ft_rownum between " + (PageSize * (CurPageNum - 1) + 1) + " and " + (PageSize * CurPageNum) + "";
                }
                else
                {
                    sql += " " + orderbystr;
                }
            }
            else if (DBConst.DataBaseType.Equals(DataBase.Sqlite))
            {
                sql = QuerySQL + "  order by " + OrderBy.Replace("'", "") + " " + OrderType.Replace("'", "");
                if (PageSize > 0)
                {
                    sql += " limit " + PageSize + " offset " + (PageSize * (CurPageNum - 1));
                }
            }
            List<Tuple<T1, T2, T3, T4>> ListRecord = new List<Tuple<T1, T2, T3, T4>>();
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                    {
                        using (DR dr = db.OpenRecord(sql))
                        {
                            while (dr.Read())
                            {
                                ListRecord.Add(new Tuple<T1, T2, T3, T4>(DRToType<T1>(dr), DRToType<T2>(dr), DRToType<T3>(dr), DRToType<T4>(dr)));
                            }
                        }
                    }
                    else
                    {
                        using (DR dr = db.OpenRecord(sql, transaction))
                        {
                            while (dr.Read())
                            {
                                ListRecord.Add(new Tuple<T1, T2, T3, T4>(DRToType<T1>(dr), DRToType<T2>(dr), DRToType<T3>(dr), DRToType<T4>(dr)));
                            }
                        }
                    }
                }
            }
            else
            {
                if (transaction == null)
                {
                    using (DR dr = db.OpenRecord(sql))
                    {
                        while (dr.Read())
                        {
                            ListRecord.Add(new Tuple<T1, T2, T3, T4>(DRToType<T1>(dr), DRToType<T2>(dr), DRToType<T3>(dr), DRToType<T4>(dr)));
                        }
                    }
                }
                else
                {
                    using (DR dr = db.OpenRecord(sql, transaction))
                    {
                        while (dr.Read())
                        {
                            ListRecord.Add(new Tuple<T1, T2, T3, T4>(DRToType<T1>(dr), DRToType<T2>(dr), DRToType<T3>(dr), DRToType<T4>(dr)));
                        }
                    }
                }
            }
            return ListRecord;
        }
        /// <summary>
        /// 通过SQL查询记录条数
        /// </summary>
        /// <param name="QuerySQL"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int SelectCount(string QuerySQL, DB db = null, ST transaction = null)
        {
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    if (transaction == null)
                        return db.GetInt(QuerySQL);
                    else
                    {
                        using (DR dr = db.OpenRecord(QuerySQL, transaction))
                        {
                            int val = 0;
                            if (dr.Read())
                            {
                                val = dr.GetIntAll(0);
                            }
                            return val;
                        }
                    }
                }
            }
            else
            {
                if (transaction == null)
                    return db.GetInt(QuerySQL);
                else
                {
                    using (DR dr = db.OpenRecord(QuerySQL, transaction))
                    {
                        int val = 0;
                        if (dr.Read())
                        {
                            val = dr.GetIntAll(0);
                        }
                        return val;
                    }
                }
            }
        }
        /// <summary>
        /// 通过实体类型和条件查询记录条数
        /// </summary>
        /// <param name="t"></param>
        /// <param name="WhereCondition"></param>
        /// <param name="TableName"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int SelectCount(Type t, string WhereCondition, string TableName = null, DB db = null, ST transaction = null)
        {
            //if (WhereCondition.Trim().StartsWith("where")) WhereCondition = WhereCondition.Trim().Substring(5);
            //if (WhereCondition.Trim() != "")WhereCondition = " where " + WhereCondition;
            //WhereCondition = " " + WhereCondition.Replace("'", "''");
            TableName = TableName ?? t.Name;
            string QuerySQL = "select count(*) from " + TableName.Replace("'", "") + "  " + WhereCondition;
            return SelectCount(QuerySQL, db, transaction);
        }
        /// <summary>
        /// 根据Lamda表达式的where条件查询记录条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="TableName"></param>
        /// <param name="db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int SelectCount<T>(Expression<Func<T, bool>> where, string TableName = null, DB db = null, ST transaction = null) where T : class, new()
        {
            T t = new T();
            TableName = TableName ?? t.GetType().Name;
            string whereSql = LambdaToSqlHelper.GetWhereSql<T>(where);
            return SelectCount<T>(whereSql, TableName, db, transaction);
        }
        /// <summary>
        /// 根据Lamda表达式的where条件查询记录条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int SelectCount<T>(Expression<Func<T, bool>> where, string TableName = null, ST transaction = null) where T : class, new()
        {
            return SelectCount<T>(where, TableName, this, transaction);
        }
        /// <summary>
        /// 通过实体类型和条件查询记录条数
        /// </summary>
        /// <param name="t"></param>
        /// <param name="WhereCondition"></param>
        /// <param name="TableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int SelectCount<T>(string WhereCondition, string TableName = null, DB db = null, ST transaction = null) where T : new()
        {
            T t = new T();
            //WhereCondition = " " + WhereCondition.Replace("'", "''");
            TableName = TableName ?? t.GetType().Name;
            //if (WhereCondition.Trim().StartsWith("where")) WhereCondition = WhereCondition.Trim().Substring(5);
            //if (WhereCondition.Trim() != "") WhereCondition = " " + WhereCondition;
            string QuerySQL = "select count(*) from " + TableName.Replace("'", "") + " " + WhereCondition;
            return SelectCount(QuerySQL, db, transaction);
        }
        private static T DRToType<T>(DR dr) where T : new()
        {
            int FieldIndex(DR dr2, string FieldName)
            {
                for (int i = 0; i < dr2.FieldCount; i++)
                {
                    if (dr2.GetName(i).ToLower() == FieldName.ToLower()) return i;
                }
                return -1;
            }
            T t = new T();
            foreach (System.Reflection.PropertyInfo item in CacheHelper.ObjectProperties(t.GetType().FullName))
            {
                int index = FieldIndex(dr, item.Name);
                if (index >= 0)
                {
                    item.SetValue(t, DRChangeType(dr.GetValue(index), item.PropertyType), null);
                }
            }
            return t;
        }
        private static Dictionary<string, object> DRToDic(DR dr)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < dr.FieldCount; i++)
            {
                string key = dr.GetName(i);
                if (!dic.ContainsKey(key))
                {
                    dic.Add(key, dr.GetValue(i));
                }
            }
            return dic;
        }
        private static object DRChangeType(object value, Type convertsionType)
        {
            //判断convertsionType类型是否为泛型，因为nullable是泛型类,
            if (convertsionType.IsGenericType &&
                //判断convertsionType是否为nullable泛型类
                convertsionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null || value.ToString().Length == 0)
                {
                    return null;
                }
                else
                {
                    return Convert.ChangeType(value, convertsionType.GetGenericArguments()[0]);
                }
            }
            return Convert.ChangeType(value, convertsionType);
        }
        public void Dispose()
        {
            Close();
            db = null;
        }
        public void Close()
        {
            db.Close();
        }
        public static int ExecSQL(string sql,string connStr=null)
        {
            int i = 0;
            using (DB mydb = (connStr==null?new DB():new DB(connStr)))
            {
                mydb.Open();
                i = mydb.ExecSql(sql);
                return i;
            }
        }
        public static void ExecSQL(IEnumerable<string> sqls, string connStr = null)
        {
            using (DB mydb = (connStr == null ? new DB() : new DB(connStr)))
            {
                mydb.Open();
                foreach (string sql in sqls)
                {
                    mydb.ExecSql(sql);
                }
            }
        }
        public static DataTable GetDataTable(string sql, DB db = null)
        {
            DataTable dt = null;
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    dt = db.GetDataTable(sql);
                    return dt;
                }
            }
            else
            {
                dt = db.GetDataTable(sql);
                return dt;
            }
        }
        public static DataSet GetDataSet(string sql, DB db = null)
        {
            return GetDataSet(sql, -1, -1, null, db);
        }
        public static DataSet GetDataSet(string sql, string SrcTable, DB db = null)
        {
            return GetDataSet(sql, -1, -1, SrcTable, db);
        }
        public static DataSet GetDataSet(string sql, int StartRecord, int MaxRecord, string SrcTable, DB db = null)
        {
            DataSet ds = new DataSet();
            if (db == null)
            {
                using (db = new DB())
                {
                    db.Open();
                    ds = db.GetDataSet(sql, StartRecord, MaxRecord, SrcTable);
                    return ds;
                }
            }
            else
            {
                ds = db.GetDataSet(sql, StartRecord, MaxRecord, SrcTable);
                return ds;
            }
        }
        public ST GetTransaction()
        {
            return db.GetTransaction();
        }
        public ST GetTransaction(IsolationLevel isolationLevel)
        {
            return db.GetTransaction(isolationLevel);
        }
    }
    public class ST
    {
        public object st;
        private DataBase DBType;//=DBConst.DataBaseType;
        public ST()
        {
        }
        public ST(object sqlst, DataBase dbType)
        {
            DBType = dbType;
            st = sqlst;
        }
        public void Commit()
        {
            switch (DBType)
            {
                case DataBase.MySql:
                    ((MySqlTransaction)st).Commit();
                    break;
                case DataBase.SqlServer:
                    ((SqlTransaction)st).Commit();
                    break;
                case DataBase.Sqlite:
                    ((SqliteTransaction)st).Commit();
                    break;
                case DataBase.Oracle:
                    ((OracleTransaction)st).Commit();
                    break;
                default:
                    break;
            }
        }
        public void Rollback()
        {
            switch (DBType)
            {
                case DataBase.MySql:
                    ((MySqlTransaction)st).Rollback();
                    break;
                case DataBase.SqlServer:
                    ((SqlTransaction)st).Rollback();
                    break;
                case DataBase.Sqlite:
                    ((SqliteTransaction)st).Rollback();
                    break;
                case DataBase.Oracle:
                    ((OracleTransaction)st).Rollback();
                    break;
                default:
                    break;
            }
        }
    }
    public class DR : IDisposable
    {
        private DataBase ConnType;
        private DRInterface rdr;
        public object DataReader;
        public DR()
        {
        }
        public DR(DRInterface MyDataReader, DataBase DBType)
        {
            ConnType = DBType;
            rdr = MyDataReader;
            DataReader = rdr.DataReader;
        }
        public DR(DB db, string sql)
        {
            ConnType = db.CurDataBaseType;
            switch (ConnType)
            {
                case DataBase.MySql:
                    rdr = new DRMySql((MySqlDataReader)db.ExecuteReader(sql));
                    break;
                case DataBase.SqlServer:
                    rdr = new DRSqlServer((SqlDataReader)db.ExecuteReader(sql));
                    break;
                case DataBase.Sqlite:
                    rdr = new DRSqlite((SqliteDataReader)db.ExecuteReader(sql));
                    break;
                case DataBase.Oracle:
                    rdr = new DROracle((OracleDataReader)db.ExecuteReader(sql));
                    break;
            }
            DataReader = rdr.DataReader;
        }
        public DR(DB db, ST st, string sql)
        {
            ConnType = db.CurDataBaseType;
            switch (ConnType)
            {
                case DataBase.MySql:
                    rdr = new DRMySql((MySqlDataReader)db.ExecuteReader(sql, st));
                    break;
                case DataBase.SqlServer:
                    rdr = new DRSqlServer((SqlDataReader)db.ExecuteReader(sql, st));
                    break;
                case DataBase.Sqlite:
                    rdr = new DRSqlite((SqliteDataReader)db.ExecuteReader(sql, st));
                    break;
                case DataBase.Oracle:
                    rdr = new DROracle((OracleDataReader)db.ExecuteReader(sql, st));
                    break;
            }
            DataReader = rdr.DataReader;
        }
        public bool Read()
        {
            return rdr.Read();
        }
        public string GetStringNoNULL(int i)
        {
            if (IsDBNull(i)) return "";
            string s = GetString(i);
            return s == null ? "" : s;
        }
        public string GetStringNoNULL(string col)
        {
            return GetStringNoNULL(GetOrdinal(col));
        }
        public string GetStringForce(int i)
        {
            if (IsDBNull(i) || GetValue(i) == null) return null;
            return GetValue(i).ToString();
        }
        public string GetStringForce(string col)
        {
            return GetStringForce(GetOrdinal(col));
        }
        public string GetStringForceNoNULL(int i)
        {
            if (IsDBNull(i)) return "";
            string s = GetStringForce(i);
            return s == null ? "" : s;
        }
        public string GetStringForceNoNULL(string col)
        {
            return GetStringForceNoNULL(GetOrdinal(col));
        }
        public string GetString(int i)
        {
            return rdr.GetString(i);
        }
        public string GetString(string col)
        {
            return GetString(GetOrdinal(col));
        }
        public void Dispose()
        {
            Close();
            rdr = null;
        }
        public int GetInt16(int i)
        {
            return rdr.GetInt16(i);
        }
        public int GetInt16(string col)
        {
            return GetInt16(GetOrdinal(col));
        }
        public int GetInt32(int i)
        {
            return rdr.GetInt32(i);
        }
        public int GetInt32(string col)
        {
            return GetInt32(GetOrdinal(col));
        }
        public long GetInt64(int i)
        {
            return rdr.GetInt64(i);
        }
        public long GetInt64(string col)
        {
            return GetInt64(GetOrdinal(col));
        }
        public int GetOrdinal(string col)
        {
            return rdr.GetOrdinal(col);
        }
        public int FieldIndex(string col)
        {
            return rdr.FieldIndex(col);
        }
        public string GetName(int i)
        {
            return rdr.GetName(i);
        }
        public object GetValue(int i)
        {
            return rdr.GetValue(i);
        }
        public object GetValue(string col)
        {
            return GetValue(GetOrdinal(col));
        }
        public bool IsDBNull(int i)
        {
            return rdr.IsDBNull(i);
        }
        public bool IsDBNull(string col)
        {
            return IsDBNull(GetOrdinal(col));
        }
        public Type GetFieldType(int i)
        {
            return rdr.GetFieldType(i);
        }
        public Type GetFieldType(string col)
        {
            return GetFieldType(GetOrdinal(col));
        }
        public string GetDataTypeName(int i)
        {
            return rdr.GetDataTypeName(i);
        }
        public string GetDataTypeName(string col)
        {
            return GetDataTypeName(GetOrdinal(col));
        }
        public DateTime GetDateTime(int i)
        {
            return rdr.GetDateTime(i);
        }
        public DateTime GetDateTime(string col)
        {
            return GetDateTime(GetOrdinal(col));
        }
        public decimal GetDecimal(int i)
        {
            return rdr.GetDecimal(i);
        }
        public decimal GetDecimal(string col)
        {
            return GetDecimal(GetOrdinal(col));
        }
        public double GetDouble(int i)
        {
            return rdr.GetDouble(i);
        }
        public double GetDouble(string col)
        {
            return GetDouble(GetOrdinal(col));
        }
        public double GetFloat(int i)
        {
            return rdr.GetFloat(i);
        }
        public double GetFloat(string col)
        {
            return GetDouble(GetOrdinal(col));
        }
        public int GetIntAll(int i)
        {
            return int.Parse(GetValue(i).ToString());
        }
        public int GetIntAll(string col)
        {
            return int.Parse(GetValue(col).ToString());
        }
        public int FieldCount => rdr.FieldCount;
        public bool HasRows => rdr.HasRows;
        public void Close()
        {
            rdr.Close();
        }
        public bool IsClosed => rdr.IsClosed;
    }
    public class PR
    {
        public object pr;
        private DataBase ConnType;
        public PR()
        {
            ConnType = DBConst.DataBaseType;
            switch (ConnType)
            {
                case DataBase.MySql:
                    pr = new MySqlParameter();
                    break;
                case DataBase.SqlServer:
                    pr = new SqlParameter();
                    break;
                case DataBase.Sqlite:
                    pr = new SqliteParameter();
                    break;
                case DataBase.Oracle:
                    pr = new OracleParameter();
                    break;
                default:
                    break;
            }
        }
        public PR(string ParaName, object value)
        {
            ConnType = DBConst.DataBaseType;
            switch (ConnType)
            {
                case DataBase.MySql:
                    pr = new MySqlParameter(ParaName, value);
                    break;
                case DataBase.SqlServer:
                    pr = new SqlParameter(ParaName, value);
                    break;
                case DataBase.Sqlite:
                    pr = new SqliteParameter(ParaName, value);
                    break;
                case DataBase.Oracle:
                    pr = new OracleParameter(ParaName, value);
                    break;
                default:
                    break;
            }
        }
        public PR(DataBase DBType)
        {
            ConnType = DBType;
            switch (ConnType)
            {
                case DataBase.MySql:
                    pr = new MySqlParameter();
                    break;
                case DataBase.SqlServer:
                    pr = new SqlParameter();
                    break;
                case DataBase.Sqlite:
                    pr = new SqliteParameter();
                    break;
                case DataBase.Oracle:
                    pr = new OracleParameter();
                    break;
                default:
                    break;
            }
        }
        public PR(DataBase DBType, string ParaName, object value)
        {
            ConnType = DBType;
            switch (ConnType)
            {
                case DataBase.MySql:
                    pr = new MySqlParameter(ParaName, value);
                    break;
                case DataBase.SqlServer:
                    pr = new SqlParameter(ParaName, value);
                    break;
                case DataBase.Sqlite:
                    pr = new SqliteParameter(ParaName, value);
                    break;
                case DataBase.Oracle:
                    pr = new OracleParameter(ParaName, value);
                    break;
                default:
                    break;
            }
        }
        public void Set(string ParaName, object value)
        {
            switch (ConnType)
            {
                case DataBase.MySql:
                    ((MySqlParameter)pr).ParameterName = ParaName;
                    ((MySqlParameter)pr).Value = value;
                    break;
                case DataBase.SqlServer:
                    ((SqlParameter)pr).ParameterName = ParaName;
                    ((SqlParameter)pr).Value = value;
                    break;
                case DataBase.Sqlite:
                    ((SqliteParameter)pr).ParameterName = ParaName;
                    ((SqliteParameter)pr).Value = value;
                    break;
                case DataBase.Oracle:
                    ((OracleParameter)pr).ParameterName = ParaName;
                    ((OracleParameter)pr).Value = value;
                    break;
                default:
                    break;
            }
        }
    }
}
