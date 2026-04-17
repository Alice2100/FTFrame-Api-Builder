using System;
using System.Data;
using Microsoft.Data.Sqlite;
namespace FTDPClient.database
{
    /// <summary>
    /// database 的摘要说明。
    /// </summary>
    public class DB:IDisposable
    {
        public SqliteConnection db;
        private string _ConnString;
        public DB()
        {

        }
        public DB(string ConnString)
        {
            _ConnString = ConnString;
        }
        public void Open()
        {
            Open(_ConnString);
        }
        public void Open(string ConnString)
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_sqlite3());
            db = new SqliteConnection(ConnString);
            db.Open();
        }
        public object GetObject(string sqlstr)
        {
            object obj;
            SqliteCommand cmd = new SqliteCommand(sqlstr, db);
            SqliteDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                obj = rdr.GetValue(0);
                rdr.Close();
                return obj;
            }
            else
            {
                rdr.Close();
                return null;
            }
        }
        public object GetObject(string sqlstr, string colname)
        {
            object obj;
            SqliteCommand cmd = new SqliteCommand(sqlstr, db);
            SqliteDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                obj = rdr.GetValue(rdr.GetOrdinal(colname));
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
            SqliteCommand cmd = new SqliteCommand(sqlstr, db);
            SqliteDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                obj = rdr.GetValue(0).ToString();
                rdr.Close();
                return obj;
            }
            else
            {

                rdr.Close();
                return null;
            }
        }
        public string GetString(string sqlstr, string colname)
        {
            string obj;
            SqliteCommand cmd = new SqliteCommand(sqlstr, db);
            SqliteDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                obj = rdr.GetValue(rdr.GetOrdinal(colname)).ToString();
                rdr.Close();
                return obj;
            }
            else
            {
                rdr.Close();
                return null;
            }
        }
        public static string GetStringValue(string sql, string connstr)
        {
            DB db = new DB();
            db.Open(connstr);
            SqliteDataReader dr = db.OpenRecord(sql);
            string val = null;
            if (dr.Read())
            {
                val = dr.IsDBNull(0) ? null : dr.GetValue(0).ToString();
            }
            dr.Close();
            db.Close();
            return val;
        }
        public int GetInt32(string sqlstr)
        {
            int obj;
            SqliteCommand cmd = new SqliteCommand(sqlstr, db);
            SqliteDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                obj = int.Parse(rdr.GetValue(0).ToString());
                rdr.Close();
                return obj;
            }
            else
            {
                rdr.Close();
                return 0;
            }
        }
        public int GetInt32NoNull(string sqlstr)
        {
            int obj;
            SqliteCommand cmd = new SqliteCommand(sqlstr, db);
            SqliteDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                obj = 0;
                if(!rdr.IsDBNull(0))
                {
                    if(int.TryParse(rdr.GetValue(0).ToString(),out int rInt))
                    {
                        obj = rInt;
                    }
                }
                rdr.Close();
                return obj;
            }
            else
            {
                rdr.Close();
                return 0;
            }
        }
        public SqliteDataReader OpenRecord(string sqlstr)
        {
            SqliteCommand cmd = new SqliteCommand(sqlstr, db);
            SqliteDataReader rdr = cmd.ExecuteReader();
            return rdr;
        }
        public int execSql(string sqlstr)
        {
            int i;
            SqliteCommand cmd = new SqliteCommand(sqlstr, db);
            i = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return i;
        }
        public int execSql(string sqlstr, SqliteTransaction st)
        {
            int i;
            SqliteCommand cmd = new SqliteCommand(sqlstr, db, st);
            i = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return i;
        }
        public void Close()
        {

            if (db.State.Equals(ConnectionState.Open))
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
            get
            {
                return db.State;
            }

        }
    }
    public class DR:IDisposable
    {
        private SqliteDataReader r;
        public DR(SqliteDataReader rdr)
        {
            r = rdr;
        }
        public string getString(int ordinal)
        {
            return r.GetString(ordinal);
        }
        public bool IsDBNull(int ordinal)
        {
            return r.IsDBNull(ordinal);
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
        public int FieldIndex(string FieldName)
        {
            for (int i = 0; i < r.FieldCount; i++)
            {
                if (r.GetName(i).ToLower() == FieldName.ToLower()) return i;
            }
            return -1;
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
}
