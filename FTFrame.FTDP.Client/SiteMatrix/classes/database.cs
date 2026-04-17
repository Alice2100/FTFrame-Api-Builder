using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using FTDPClient.consts;
using FTDPClient.forms;
using System.Linq;
using FTDPClient.functions;
using Microsoft.Data.Sqlite;

namespace FTDPClient.database
{
	/// <summary>
	/// database 的摘要说明。
	/// </summary>
	public class DBOle:IDisposable
	{
		private OleDbConnection db; 
		public void Open(string ConnString)
		{
			db=new OleDbConnection(ConnString);
			db.Open();
		}
		public object GetObject(string sqlstr)
		{	
			object obj;
			OleDbCommand cmd=new OleDbCommand(sqlstr,db);
			OleDbDataReader rdr=cmd.ExecuteReader();
			if(rdr.Read())
			{
				obj=rdr.GetValue(0);
				rdr.Close();
				return obj;
			}
			else
			{
				rdr.Close();
			return null;
			}
		}
		public object GetObject(string sqlstr,string colname)
		{	
			object obj;
			OleDbCommand cmd=new OleDbCommand(sqlstr,db);
			OleDbDataReader rdr=cmd.ExecuteReader();
			if(rdr.Read())
			{
			obj=rdr.GetValue(rdr.GetOrdinal(colname));
			rdr.Close();
			return obj;
			}
			else
			{
				rdr.Close();
			return null;
			}
		}
		public string GetString(string sqlstr)
		{	
			string obj;
			OleDbCommand cmd=new OleDbCommand(sqlstr,db);
			OleDbDataReader rdr=cmd.ExecuteReader();
			if(rdr.Read())
			{
				obj=rdr.GetValue(0).ToString();
				rdr.Close();
				return obj;
			}
			else
			{

rdr.Close();
			return null;
			}
		}
		public string GetString(string sqlstr,string colname)
		{	
			string obj;
			OleDbCommand cmd=new OleDbCommand(sqlstr,db);
			OleDbDataReader rdr=cmd.ExecuteReader();
			if(rdr.Read())
			{
				obj=rdr.GetValue(rdr.GetOrdinal(colname)).ToString();
				rdr.Close();
				return obj;
			}
			else
			{
				rdr.Close();
			return null;
			}
		}
		public int GetInt32(string sqlstr)
		{	
			int obj;
			OleDbCommand cmd=new OleDbCommand(sqlstr,db);
			OleDbDataReader rdr=cmd.ExecuteReader();
			if(rdr.Read())
			{
				obj=int.Parse(rdr.GetValue(0).ToString());
				rdr.Close();
				return obj;
			}
			else
			{
			rdr.Close();
			return 0;
			}
		}
		public OleDbDataReader OpenRecord(string sqlstr)
		{
            OleDbCommand cmd=new OleDbCommand(sqlstr,db);
			OleDbDataReader rdr=cmd.ExecuteReader();
			return rdr;
		}
		public int execSql(string sqlstr)
		{
			int i;
			OleDbCommand cmd=new OleDbCommand(sqlstr,db);
			i=cmd.ExecuteNonQuery();
			cmd.Dispose();
			return i;
		}
		public void Close()
		{
			
				if(db.State.Equals(ConnectionState.Open))
				{
					db.Close();
				}
			
		}

        public void Dispose()
        {
            Close();
        }

        public ConnectionState State
        {
        get {
            return db.State;
            }

        }
	}
	public class DROle:IDisposable
	{
		private OleDbDataReader r;
		public DROle(OleDbDataReader rdr)
		{
		r=rdr;
		}
		public string getString(int ordinal)
		{
		return r.GetString(ordinal);
		}
		public string getString(string columename)
		{
		return r.GetString(r.GetOrdinal(columename));
		}
        public object getValue(int ordinal)
        {
            return r.GetValue(ordinal);
        }
		public int FieldCount
		{
			get { return r.FieldCount; }
		}
		public string GetName(int index)
		{
			return r.GetName(index);
		}
		public object getValue(string columename)
        {
            return r.GetValue(r.GetOrdinal(columename));
        }
		public int getInt32(int ordinal)
		{
			return r.GetInt32(ordinal);
		}
		public int getInt32(string columename)
		{
			return r.GetInt32(r.GetOrdinal(columename));
		}
        public int getInt16(int ordinal)
        {
            return r.GetInt16(ordinal);
        }
        public int getInt16(string columename)
        {
            return r.GetInt16(r.GetOrdinal(columename));
        }
		public bool Read()
		{
		return r.Read();
		}
		public void Close()
		{
		r.Close();
		}
		public void Dispose()
		{
			Close();
		}
	}
	public class DBFunc
	{
		public static Dictionary<string,string> GetColumnNoteForSqlite(DB conn,string tablename)
        {
            //获取sqlite注释
            var zhuShiDic = new Dictionary<string, string>();
            var DDL = "";
            string sql = "SELECT sql FROM sqlite_master WHERE type='table' AND name = '" + tablename + "'";
            var dr = conn.OpenRecord(sql);
            if (dr.Read())
            {
                DDL = dr.GetString(0);
                dr.Close();
            }
			else
			{
				dr.Close();
				return zhuShiDic;
            }
            var rows = DDL.Split(new string[] { Environment.NewLine, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim());
            foreach (var row in rows)
            {
                var index = row.IndexOf("--");
                if (index > 0)
                {
                    var zhushi = row.Substring(index + 2).Trim();
                    if (row.StartsWith("\""))
                    {
                        var rs = row.Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries);
                        if (rs.Length >= 2)
                        {
                            var columnname = rs[0].Trim().ToLower();
                            if (!zhuShiDic.ContainsKey(columnname)) zhuShiDic.Add(columnname, zhushi);
                        }
                    }
                }
            }
            return zhuShiDic;
        }
		public static string DBTypeString(globalConst.DBType dbtype)
		{
            var _dbtype = "";
            switch (dbtype)
            {
                case globalConst.DBType.MySql:
                    _dbtype = "mysql"; break;
                case globalConst.DBType.SqlServer:
                    _dbtype = "sqlserver"; break;
                case globalConst.DBType.Sqlite:
                    _dbtype = "sqlite"; break;
            }
			return _dbtype;
        }
	}
}
