using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Tool;
using FTFrame.DBClient;
using System.Linq;
using System.IO;
using System.Xml;
using System.Collections;
using CoreHttp = Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FTFrame.Project.Core
{
    /// <summary>
    /// Based on the actual situation of database design, define the logic for obtaining primary keys, retaining fields, and storing them
    /// </summary>
    public static class DBSuit
    {
        /// <summary>
        /// Obtain a read-only database connection string, and define rules when there are multiple read-only databases.
        /// If 'Data List' or 'Data Getting' with data operations (as Batch updates,Execute after getting) cannot be used as read-only
        /// </summary>
        /// <returns></returns>
        public static (DBClient.DataBase DataBaseType, string ConnString) ReadOnlyConnection(HttpContext context)
        {
            // Multiple read-only databases can be specified according to rules
            // Connection strings can be obtained from the configuration file, for example:ConfigHelper.GetConfigValue("Project:ConnString_V1")
            return (DBConst.DataBaseType,SysConst.ConnString_ReadOnly);
        }
        /// <summary>
        /// Obtain the basic database connection string (read-write) and define rules
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static (DBClient.DataBase DataBaseType, string ConnString) BaseConnection(HttpContext context)
        {
            return (DBConst.DataBaseType, SysConst.ConnString);
        }
        public static DB DBReadOnly()
        {
            return new DB(DBConst.DataBaseType, SysConst.ConnString_ReadOnly);
        }
        /// <summary>
        /// Important:Rules for defining primary key values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fidCol"></param>
        /// <returns></returns>
        public static (string KeyName, Enums.KeyType KeyType) Key(string tableName, string fidCol = null)
        {
            switch (tableName.ToLower())
            {
                case string r when (r.StartsWith("cn_") || r.StartsWith("ef_")):
                    return ("id", Enums.KeyType.AutoIncrement);
                case string r when (r == "ft_test_table"):
                    return ("fid", Enums.KeyType.Guid);
                default:
                    if (string.IsNullOrEmpty(fidCol)) return ("id", Enums.KeyType.SnowId);
                    else return (fidCol, Enums.KeyType.Guid);
            }
        }
        /// <summary>
        /// If no data table name specified, infer primary key columns and types
        /// Only in data list show
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="mainTable"></param>
        /// <returns></returns>
        public static (string KeyName, Enums.KeyType KeyType) Key(this DR dr, string mainTable = null)
        {
            (string KeyName, Enums.KeyType KeyType) key = (null, Enums.KeyType.Guid);
            if (mainTable != null) key = Key(mainTable);
            if (key.KeyName != null && key.KeyName != "NullKey" && dr.FieldIndex(key.KeyName) >= 0) return key;
            //other
            return dr.GetDataTypeName(0) == "bigint" ? (dr.GetName(1), Enums.KeyType.Guid) : (dr.GetName(0), Enums.KeyType.Guid);
        }
        /// <summary>
        /// Primary key value generation rules
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static object KeyGenerate(string tableName)
        {
            var key = Key(tableName);
            if (key.KeyType == Enums.KeyType.SnowId) return Utils.Str.SnowId();
            if (key.KeyType == Enums.KeyType.Guid) return str.GetCombID();
            return "NullKeyValue";
        }
        /// <summary>
        /// Preprocessing for obtaining output values of database field types
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="colName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static (string val, bool quotation) CommonValue(this DR dr, string colName, string tableName = null, int? colOrdinal = null)
        {
            if (dr.IsDBNull(colOrdinal ?? dr.GetOrdinal(colName)))//IsDBNull?
            {
                //return ("", true);
                return ("null", false);
            }
            var val = dr.GetValue(colOrdinal ?? dr.GetOrdinal(colName));
            return ValueQuotation(val);
        }
        public static (string val, bool quotation) ValueQuotation(object val)
        {
            switch (val.GetType().Name.ToLower())
            {
                case "date":
                    return (str.GetDate((DateTime)val), true);
                case "datetime":
                    return (str.GetDateTime((DateTime)val), true);
                case "int":
                case "int32":
                case "decimal":
                case "double":
                case "float":
                case "single":
                case "int16":
                case "int64":
                case "byte":
                case "sbyte":
                case "bool":
                case "long":
                case "ulong":
                case "short":
                case "ushort":
                case "uint":
                    return (val.ToString(), false);
                default:
                    return (val.ToString(), true);
            }
        }
        /// <summary>
        /// Default fields and value definitions when adding data to the interface
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static Dictionary<string, object> DefaultColsWhenAddForApi(string tableName, Dictionary<string, object> reqDic)
        {
            var dic = new Dictionary<string, object>();
            tableName = tableName.ToLower();
            try
            {
                if(tableName.StartsWith("tb_"))
                {
                    dic.Add("_add_time", str.GetDateTime());
                    dic.Add("_add_user", User.UserID(reqDic));
                    dic.Add("_update_time", str.GetDateTime());
                    dic.Add("_update_user", User.UserID(reqDic));
                    dic.Add("_available_flag", 1);
                }
                else if (tableName.StartsWith("cn_") || tableName.StartsWith("ef_"))
                {
                    dic.Add("create_time", str.GetDateTime());
                    dic.Add("create_by", User.UserID(reqDic));
                    dic.Add("update_time", str.GetDateTime());
                    dic.Add("update_by", User.UserID(reqDic));
                    dic.Add("is_delete_flag", 1);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dic;
        }
        /// <summary>
        /// Default fields and value definitions when modifying data through interfaces
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static Dictionary<string, object> DefaultColsWhenModForApi(string tableName, Dictionary<string, object> reqDic)
        {
            var dic = new Dictionary<string, object>();
            tableName = tableName.ToLower();
            try
            {
                if (tableName.StartsWith("tb_"))
                {
                    dic.Add("_update_time", str.GetDateTime());
                    dic.Add("_update_user", User.UserID(reqDic));
                }
                else if (tableName.StartsWith("cn_") || tableName.StartsWith("ef_"))
                {
                    dic.Add("update_time", str.GetDateTime());
                    dic.Add("update_by", User.UserID(reqDic));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dic;
        }
        /// <summary>
        /// Default fields and value definitions when adding data to a page
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static Dictionary<string, object> DefaultColsWhenAddForPage(string tableName)
        {
            return DefaultColsWhenAddForApi(tableName,null);
        }
        /// <summary>
        /// Default fields and value definitions when modifying data on a page
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static Dictionary<string, object> DefaultColsWhenModForPage(string tableName)
        {
            return DefaultColsWhenModForApi(tableName, null);
        }

    }
}
