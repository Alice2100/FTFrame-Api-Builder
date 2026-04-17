using System;
using System.Data;
using System.Data.OleDb;
using SiteMatrix.functions;
namespace SiteMatrix.database
{
	/// <summary>
	/// database 的摘要说明。
	/// </summary>
	public class DB
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
        public ConnectionState State
        {
        get {
            return db.State;
            }

        }
	}
	public class DR
	{
		private OleDbDataReader r;
		public DR(OleDbDataReader rdr)
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
	}
}
