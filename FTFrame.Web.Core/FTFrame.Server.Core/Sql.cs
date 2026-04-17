using System;
using System.Collections.Generic;
using System.Text;
using FTFrame.Tool;
using FTFrame.DBClient;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.IO;
using FTFrame.Server.Core.Tool;
using System.Xml;
using System.Collections;

namespace FTFrame.Server.Core
{
    public class Sql
    {
        public static bool PublishDataTable(DB db, string siteid, string uploadpath)
        {
            log.Debug("Start", "[PublishData]");
            if (!File.Exists(uploadpath + "\\formtable.xml"))
            {
                log.Debug("formtable.xml not exists", "[PublishData]");
                return true;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(uploadpath + "\\formtable.xml");
            try
            {
                foreach (XmlNode table in doc.SelectNodes("/formdata/table"))
                {
                    string sql = null;
                    string tablename = table.Attributes["id"].Value;
                    bool isalltable = tablename.StartsWith("@");
                    if (isalltable)
                    {
                        tablename = tablename.Substring(1);
                    }
                    else
                    {
                        tablename = "ft_" + siteid + "_f_" + table.Attributes["id"].Value;
                    }
                    DR dr = db.OpenRecord(Sql.TableExists(tablename));
                    bool TableExists = dr.HasRows;
                    dr.Close();
                    ArrayList rows = new ArrayList();
                    ArrayList rowids = new ArrayList();
                    foreach (XmlNode datarow in table.SelectNodes("row"))
                    {
                        Model.DataRow row = new Model.DataRow();
                        row.rowid = datarow.Attributes["id"].Value;
                        row.datatype = datarow.SelectSingleNode("datatype").Attributes["name"].Value;
                        row.length = int.Parse(datarow.SelectSingleNode("datatype/length").InnerText);
                        row.numpoint = int.Parse(datarow.SelectSingleNode("datatype/numpoint").InnerText);
                        row.allownull = bool.Parse(datarow.SelectSingleNode("allownull").InnerText);
                        row.Default = datarow.SelectSingleNode("default").InnerText.Trim();
                        row.primary = bool.Parse(datarow.SelectSingleNode("primary").InnerText);
                        row.index = bool.Parse(datarow.SelectSingleNode("index").InnerText);
                        rows.Add(row);
                        rowids.Add(row.rowid);
                    }
                    //4.0 to 4.1
                    if (!rowids.Contains("flowpos"))
                    {
                        Model.DataRow row = new Model.DataRow();
                        row.rowid = "flowpos";
                        row.datatype = "int";
                        row.length = 8;
                        row.numpoint = 0;
                        row.allownull = true;
                        row.Default = "NULL";
                        row.primary = false;
                        row.index = true;
                        rows.Add(row);
                    }
                    if (!TableExists)
                    {
                        if (SysConst.DataBaseType.Equals(DataBase.MySql))
                        {
                            string primary = "";
                            string index = "";
                            sql = "CREATE TABLE " + tablename + "(";
                            foreach (Model.DataRow row in rows)
                            {
                                sql += "" + row.rowid + " " + Sql.DataTypeConvert(row.datatype, row.length, row.numpoint) + " " + ((!row.allownull) ? "NOT NULL" : "");
                                if (!row.datatype.Equals("text"))
                                {
                                    sql += " default " + (row.Default.Equals("NULL") ? "NULL" : ("'" + row.Default + "'"));
                                }
                                sql += ",";
                                if (row.primary)
                                {
                                    primary += "," + row.rowid;
                                }
                                //text字段不能作为索引
                                if (row.index && !row.datatype.Equals("text"))
                                {
                                    index += "KEY " + row.rowid + "_index (" + row.rowid + "),";
                                }
                            }
                            if (!primary.Equals(""))
                            {
                                primary = "PRIMARY KEY  (" + primary.Substring(1) + "),";
                            }
                            string sqlhead = sql + primary + index;
                            if (sqlhead.EndsWith(",")) sqlhead = sqlhead.Substring(0, sqlhead.Length - 1);
                            sql = sqlhead + ")";
                            log.Debug(sql, "CREATE");
                            db.ExecSql(sql);
                        }
                        else if (SysConst.DataBaseType.Equals(DataBase.SqlServer))
                        {
                            string primary = "";
                            sql = "CREATE TABLE [" + tablename + "](";
                            foreach (Model.DataRow row in rows)
                            {
                                sql += "[" + row.rowid + "] " + Sql.DataTypeConvert(row.datatype, row.length, row.numpoint) + " " + ((!row.allownull) ? "NOT NULL" : "NULL");
                                if (!row.Default.Equals("NULL"))
                                {
                                    sql += " default '" + row.Default + "'";
                                }
                                sql += ",";
                                if (row.primary)
                                {
                                    if (!primary.Equals("")) primary += ",";
                                    primary += "[" + row.rowid + "] ASC";
                                }
                                //sqlserver 手动加索引
                            }
                            primary = "CONSTRAINT [PK_ft_" + siteid + "_" + tablename + "] PRIMARY KEY CLUSTERED(" + primary + ")";
                            primary += "WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]";
                            sql = sql + primary + ") ON [PRIMARY]";
                            log.Debug(sql, "CREATE");
                            db.ExecSql(sql);
                        }
                    }
                    else
                    {
                        if (SysConst.DataBaseType.Equals(DataBase.MySql))
                        {
                            //以下更改表结构需手动完成：更改索引、主键、删除列、非常规数据类型不更改
                            sql = TableStructSql(tablename);
                            dr = db.OpenRecord(sql);
                            Hashtable curDataTable = new Hashtable();
                            while (dr.Read())
                            {
                                curDataTable.Add(dr.GetString("Field"), new string[] { dr.GetString("Type"), dr.GetString("Null"), (dr.IsDBNull("Default") ? "NULL" : dr.GetString("Default")) });
                            }
                            dr.Close();
                            foreach (Model.DataRow row in rows)
                            {
                                if (curDataTable.ContainsKey(row.rowid.ToLower()))
                                {
                                    string Type = ((string[])(curDataTable[row.rowid.ToLower()]))[0];
                                    string Null = ((string[])(curDataTable[row.rowid.ToLower()]))[1];
                                    string Default = ((string[])(curDataTable[row.rowid.ToLower()]))[2];
                                    bool IsColumnSame = false;
                                    if (Sql.IsBaseDataType(Type))
                                    {
                                        if (Type.Equals(Sql.DataTypeConvert(row.datatype, row.length, row.numpoint)))
                                        {
                                            if ((Null.Equals("NO") && !row.allownull) || (Null.Equals("YES") && row.allownull))
                                            {
                                                if (Default.Equals(row.Default))
                                                {
                                                    IsColumnSame = true;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        IsColumnSame = true;//非常规类型字段为用户自定义字段，不做变更
                                    }
                                    if (!IsColumnSame)
                                    {
                                        sql = "ALTER TABLE " + tablename + " CHANGE " + row.rowid + " " + row.rowid + " " + Sql.DataTypeConvert(row.datatype, row.length, row.numpoint) + " " + ((!row.allownull) ? "NOT NULL" : "");
                                        if (!row.datatype.Equals("text"))
                                        {
                                            sql += " default " + (row.Default.Equals("NULL") ? "NULL" : ("'" + row.Default + "'"));
                                        }
                                        log.Debug(sql, "CHANGE");
                                        db.ExecSql(sql);
                                    }
                                }
                                else
                                {
                                    sql = "ALTER TABLE " + tablename + " ADD " + row.rowid + " " + Sql.DataTypeConvert(row.datatype, row.length, row.numpoint) + " " + ((!row.allownull) ? "NOT NULL" : "");
                                    if (!row.datatype.Equals("text"))
                                    {
                                        sql += " default " + (row.Default.Equals("NULL") ? "NULL" : ("'" + row.Default + "'"));
                                    }
                                    log.Debug(sql, "ADD");
                                    db.ExecSql(sql);
                                }
                            }
                        }
                        else if (SysConst.DataBaseType.Equals(DataBase.SqlServer))
                        {
                            //以下更改表结构需手动完成：更改索引、主键、删除列、默认值、非常规数据类型不更改
                            sql = TableStructSql(tablename);
                            dr = db.OpenRecord(sql);
                            Hashtable curDataTable = new Hashtable();
                            while (dr.Read())
                            {
                                curDataTable.Add(dr.GetString("ColumnName").ToLower(), new string[] { dr.GetString("Type"), dr.GetStringForce("length"), dr.GetStringForce("xprec"), dr.GetStringForce("xscale"), dr.GetStringForce("isnullable"), dr.GetStringForce("cdefault") });
                            }
                            dr.Close();
                            foreach (Model.DataRow row in rows)
                            {
                                if (curDataTable.ContainsKey(row.rowid.ToLower()))
                                {
                                    string Type = ((string[])(curDataTable[row.rowid.ToLower()]))[0];
                                    int Length = int.Parse(((string[])(curDataTable[row.rowid.ToLower()]))[1]);
                                    int Xprec = int.Parse(((string[])(curDataTable[row.rowid.ToLower()]))[2]);
                                    int Xscale = int.Parse(((string[])(curDataTable[row.rowid.ToLower()]))[3]);
                                    bool IsNull = int.Parse(((string[])(curDataTable[row.rowid.ToLower()]))[4]) == 1;
                                    string Default = ((string[])(curDataTable[row.rowid.ToLower()]))[5];
                                    bool IsColumnSame = false;
                                    if (Sql.IsBaseDataTypeSqlServer(Type))
                                    {
                                        IsColumnSame = IsColumnSameSqlServer(Type, Length, Xprec, Xscale, IsNull, Default, row.datatype, row.length, row.numpoint, row.Default, row.allownull);
                                    }
                                    else
                                    {
                                        IsColumnSame = true;//非常规类型字段为用户自定义字段，不做变更
                                    }
                                    if (!IsColumnSame)
                                    {
                                        //存在默认值的字段修改时会报错
                                        sql = "ALTER TABLE " + tablename + " ALTER COLUMN " + row.rowid + " " + Sql.DataTypeConvert(row.datatype, row.length, row.numpoint) + " " + ((!row.allownull) ? "NOT NULL" : "NULL");
                                        log.Debug(sql, "CHANGE");
                                        db.ExecSql(sql);
                                    }
                                }
                                else
                                {
                                    sql = "ALTER TABLE " + tablename + " ADD " + row.rowid + " " + Sql.DataTypeConvert(row.datatype, row.length, row.numpoint) + " " + ((!row.allownull) ? "NOT NULL" : "NULL");
                                    if (!row.Default.Equals("NULL"))
                                    {
                                        sql += " default '" + row.Default + "'";
                                    }
                                    log.Debug(sql, "ADD");
                                    db.ExecSql(sql);
                                }
                            }
                        }
                    }
                    if (!isalltable)
                    {
                        tablename = tablename + "_dy";
                        dr = db.OpenRecord(Sql.TableExists(tablename));
                        TableExists = dr.HasRows;
                        dr.Close();
                        if (!TableExists)
                        {
                            if (SysConst.DataBaseType.Equals(DataBase.MySql))
                            {
                                sql = "CREATE TABLE " + tablename + "(";
                                sql += "fid varchar(36) NOT NULL default '',";
                                sql += "eid varchar(16) NOT NULL default '',";
                                sql += "etype varchar(16) default NULL,";
                                sql += "evalue text default NULL,";
                                sql += "erate int(8) NOT NULL default 1,";
                                sql += "PRIMARY KEY  (fid,eid,erate),";
                                sql += "KEY  fid_index(fid),";
                                sql += "KEY  eid_index(eid),";
                                sql += "KEY  etype_index(etype),";
                                sql += "KEY  erate_index(erate)";
                                sql += ")";
                                log.Debug(sql, "CREATE");
                                db.ExecSql(sql);
                            }
                            else if (SysConst.DataBaseType.Equals(DataBase.SqlServer))
                            {
                                sql = "CREATE TABLE [" + tablename + "](";
                                sql += "[fid] [varchar](36) NOT NULL,";
                                sql += "[eid] [varchar](36) NOT NULL,";
                                sql += "[etype] [varchar](16) NULL,";
                                sql += "[evalue] [text] NULL,";
                                sql += "[erate] [int] NOT NULL default 1,";
                                sql += "CONSTRAINT [PK_ft_" + siteid + "_" + tablename + "] PRIMARY KEY CLUSTERED (";
                                sql += "[fid] ASC,[eid] ASC,[erate] ASC";
                                sql += ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]";
                                sql += ") ON [PRIMARY]";
                                log.Debug(sql, "CREATE");
                                db.ExecSql(sql);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
            log.Debug("End", "[PublishData]");
            return true;
        }
        public static bool isTableExists(string tablename)
        {
            bool b = false;
            DB db = new DB();
            db.Open();
            try
            {
                DR r = db.OpenRecord(TableExists(tablename));
                b = r.HasRows;
                r.Close();
                db.Close();
                return b;
            }
            catch (Exception e)
            {
                log.Error("isTableExists Error return false " + e.Message);
                db.Close();
                return false;
            }
        }
        public static string TableExists(string tablename)
        {
            string sql = null;
            switch (SysConst.DataBaseType)
            {
                case DataBase.MySql:
                    sql = "SHOW TABLES LIKE '" + tablename + "'";
                    break;
                case DataBase.SqlServer:
                    sql = "select name from sysobjects where name<>'dtproperties' and xtype='U' and name='" + tablename + "'";
                    break;
                case DataBase.Sqlite:
                    sql = "select name from sqlite_master where type='table' and name='" + tablename + "'";
                    break;
                case DataBase.Oracle:
                    break;
                case DataBase.DB2:
                    break;
                case DataBase.Sybase:
                    break;
            }
            return sql;
        }
        public static bool IsBaseDataTypeSqlServer(string datatype)
        {
            datatype = datatype.ToLower();
            return datatype.Equals("nvarchar") || datatype.Equals("smallint") || datatype.Equals("int") || datatype.Equals("datetime") || datatype.Equals("text") || datatype.Equals("decimal");
        }
        public static bool IsColumnSameSqlServer(string Type, int Length, int Xprec, int Xscale, bool IsNull, string Default, string RowType, int RowLength, int RowNumber, string RowDefault, bool RowIsNUll)
        {
            if (IsNull != RowIsNUll) return false;
            bool IsColumnSame = false;
            switch (RowType)
            {
                case "varchar":
                    IsColumnSame = (Type.Equals("nvarchar") && Length == RowLength * 2);
                    break;
                case "tinyint":
                    IsColumnSame = (Type.Equals("smallint"));
                    break;
                case "int":
                    IsColumnSame = (Type.Equals("int"));
                    break;
                case "decimal":
                    IsColumnSame = (Type.Equals("decimal") && Xprec == RowLength && Xscale == RowNumber);
                    break;
                case "text":
                    IsColumnSame = (Type.Equals("text"));
                    break;
                case "datetime":
                    IsColumnSame = (Type.Equals("datetime"));
                    break;
            }
            return IsColumnSame;
        }
        public static bool IsBaseDataType(string datatype)
        {
            ArrayList al = new ArrayList();
            string s = DataTypeConvert("varchar", 0, 0);
            al.Add(s.Substring(0, s.IndexOf('(')));
            s = DataTypeConvert("tinyint", 0, 0);
            al.Add(s.Substring(0, s.IndexOf('(')));
            s = DataTypeConvert("int", 0, 0);
            al.Add(s.Substring(0, s.IndexOf('(')));
            s = DataTypeConvert("decimal", 0, 0);
            al.Add(s.Substring(0, s.IndexOf('(')));
            s = DataTypeConvert("text", 0, 0);
            al.Add(s);
            s = DataTypeConvert("datetime", 0, 0);
            al.Add(s);
            if (datatype.IndexOf('(') > 0) datatype = datatype.Substring(0, datatype.IndexOf('('));
            return al.Contains(datatype);
        }
        public static string DataTypeConvertBase(string basetype)
        {
            switch (basetype)
            {
                case "string": return "varchar";
                case "int": return "int";
                case "number": return "decimal";
                case "text": return "text";
                case "datetime": return "datetime";
            }
            return null;
        }
        public static string DataTypeConvert(string datatype, int length, int numpoint)
        {
            string newdatatype = null;
            switch (SysConst.DataBaseType)
            {
                case DataBase.MySql:
                    switch (datatype)
                    {
                        case "varchar":
                            newdatatype = "varchar(" + length + ")";
                            break;
                        case "tinyint":
                            newdatatype = "tinyint(" + length + ")";
                            break;
                        case "int":
                            newdatatype = "int(" + length + ")";
                            break;
                        case "decimal":
                            newdatatype = "decimal(" + length + "," + numpoint + ")";
                            break;
                        case "text":
                            newdatatype = "text";
                            break;
                        case "datetime":
                            newdatatype = "datetime";
                            break;
                    }
                    break;
                case DataBase.SqlServer:
                    switch (datatype)
                    {
                        case "varchar":
                            newdatatype = "[nvarchar](" + length + ")";
                            break;
                        case "tinyint":
                            newdatatype = "[smallint]";
                            break;
                        case "int":
                            newdatatype = "[int]";
                            break;
                        case "decimal":
                            newdatatype = "[decimal](" + length + "," + numpoint + ")";
                            break;
                        case "text":
                            newdatatype = "[text]";
                            break;
                        case "datetime":
                            newdatatype = "[datetime]";
                            break;
                    }
                    break;
                case DataBase.Oracle:
                    break;
                case DataBase.DB2:
                    break;
                case DataBase.Sybase:
                    break;
            }
            return newdatatype;
        }
        public static string TableStructSql(string tablename)
        {
            switch (SysConst.DataBaseType)
            {
                case DataBase.MySql:
                    return "describe " + tablename;
                case DataBase.SqlServer:
                    string s = "SELECT     syscolumns.name     AS     ColumnName,systypes.name     AS     Type,     syscolumns.length,syscolumns.xprec,syscolumns.xscale,syscolumns.isnullable,syscolumns.cdefault";
                    s += " FROM     sysobjects";
                    s += " INNER     JOIN syscolumns     ON     sysobjects.id     =     syscolumns.id ";
                    s += " INNER     JOIN systypes     ON     syscolumns.xtype     =     systypes.xtype ";
                    s += " WHERE     (sysobjects.xtype     =     'U')";
                    s += " AND     (sysobjects.name     <>     'dtproperties')";
                    s += " AND     (sysobjects.name     =     '" + tablename + "')";
                    s += " AND     (systypes.name     <>     'sysname')";
                    s += " AND     (systypes.status     <>     3)";
                    s += " GROUP     BY     syscolumns.name,     sysobjects.name,     syscolumns.xtype,systypes.name,     syscolumns.length,syscolumns.xprec,syscolumns.xscale,syscolumns.isnullable,syscolumns.cdefault";
                    return s;
            }
            return null;
        }
        public static string GetSqlForRemoveSameCols(string oriSql)
        {
            //@*@
            if (oriSql.IndexOf("@*@") < 0) return oriSql;
            string _oriSql = oriSql.Replace("@*@", "*");
            StringBuilder selCols = new StringBuilder(100);
            string sql = "";
            if (SysConst.DataBaseType== DataBase.SqlServer)
            {
                sql = "select top 0 * from (" + _oriSql + ") t123456";
            }
            else if (SysConst.DataBaseType == DataBase.MySql)
            {
                sql = _oriSql + " limit 0";
            }
            using (DB db=new DB())
            {
                db.Open();
                using (DR dr=db.OpenRecord(sql))
                {
                    Dictionary<string, int> dic = new Dictionary<string, int>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        string key = dr.GetName(i).ToLower();
                        if (!key.StartsWith("alias_"))
                        {
                            if (!dic.ContainsKey(key)) dic.Add(key, 1);
                            else dic[key] += 1;
                        }
                    }
                    var dicList = dic.Where(r => r.Value == 1).Select(r => r.Key).ToList();
                    for (int i = 0; i < dicList.Count; i++)
                    {
                        if (i > 0) selCols.Append(",");
                        selCols.Append(dicList[i]);
                    }
                    dicList.Clear();
                    dic.Clear();
                }
            }
            return oriSql.Replace("@*@", selCols.ToString());
        }
    }
}
