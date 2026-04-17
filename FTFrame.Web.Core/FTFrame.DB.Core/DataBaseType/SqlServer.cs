using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace FTFrame.DBClient.DataBaseType
{
    public class DBSqlServer : DBInterface
    {
        SqlConnection db;
        public object DBConnection
        {
            get { return db; }
        }
        public void Open(string ConnStr)
        {
            db = new SqlConnection(ConnStr);
            db.Open();
        }
        public object GetObject(string sqlstr, string colname, int commandTimeOut)
        {
            object obj = null;
            using (var cmd = new SqlCommand(sqlstr, db))
            {
                cmd.CommandTimeout = commandTimeOut;
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        if (colname == null)
                        {
                            obj = rdr.IsDBNull(0) ? null : rdr.GetValue(0);
                        }
                        else
                        {
                            obj = rdr.IsDBNull(rdr.GetOrdinal(colname)) ? null : rdr.GetValue(rdr.GetOrdinal(colname));
                        }
                    }
                    rdr.Close();
                }
            }
            return obj;
        }
        public string GetString(string sqlstr, string colname, int commandTimeOut)
        {
            string obj = null;
            using (var cmd = new SqlCommand(sqlstr, db))
            {
                cmd.CommandTimeout = commandTimeOut;
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        if (colname == null)
                        {
                            if (!rdr.IsDBNull(0))
                                obj = rdr.GetValue(0).ToString();
                        }
                        else
                        {
                            if (!rdr.IsDBNull(rdr.GetOrdinal(colname)))
                                obj = rdr.GetValue(rdr.GetOrdinal(colname)).ToString();
                        }
                    }
                    rdr.Close();
                }
            }
            return obj;
        }
        public int GetInt(string sqlstr, int commandTimeOut)
        {
            int obj = 0;
            using (var cmd = new SqlCommand(sqlstr, db))
            {
                cmd.CommandTimeout = commandTimeOut;
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                            obj = int.Parse(rdr.GetValue(0).ToString());
                    }
                    rdr.Close();
                }
            }
            return obj;
        }
        public decimal GetDecimal(string sqlstr, int commandTimeOut)
        {
            decimal obj = 0;
            using (var cmd = new SqlCommand(sqlstr, db))
            {
                cmd.CommandTimeout = commandTimeOut;
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                            obj = rdr.GetDecimal(0);
                    }
                    rdr.Close();
                }
            }
            return obj;
        }
        public DataTable GetDataTable(string sqlstr)
        {
            DataTable dt = new DataTable();
            new SqlDataAdapter(sqlstr, db).Fill(dt);
            return dt;
        }
        public DataSet GetDataSet(string sql, int StartRecord, int MaxRecord, string SrcTable)
        {
            DataSet ds = new DataSet();
            if (MaxRecord == -1 && SrcTable == null)
            {
                new SqlDataAdapter(sql, db).Fill(ds);
            }
            else if (MaxRecord == -1 && SrcTable != null)
            {
                new SqlDataAdapter(sql, db).Fill(ds, SrcTable);
            }
            else
            {
                new SqlDataAdapter(sql, db).Fill(ds, StartRecord, MaxRecord, SrcTable);
            }
            return ds;
        }
        public DR OpenRecord(string sqlstr,int commandTimeOut)
        {
            DR rdr = null;
            using (SqlCommand cmd = new SqlCommand(sqlstr,db))
            {
                cmd.CommandTimeout = commandTimeOut;
                rdr = new DR(new DRSqlServer(cmd.ExecuteReader()),DataBase.SqlServer);
            }
            return rdr;
        }
        public DR OpenRecord(string sqlstr, ST st, int commandTimeOut)
        {
            DR rdr = null;
            using (SqlCommand cmd = new SqlCommand(sqlstr, db, ((SqlTransaction)(st.st))))
            {
                cmd.CommandTimeout = commandTimeOut;
                rdr = new DR(new DRSqlServer(cmd.ExecuteReader()), DataBase.SqlServer);
            }
            return rdr;
        }
        public object ExecuteReader(string sqlstr, int commandTimeOut)
        {
            object or = null; 
            using (SqlCommand cmd = new SqlCommand(sqlstr, db))
            {
                cmd.CommandTimeout = commandTimeOut;
                or = cmd.ExecuteReader();
            }
            return or;
        }
        public object ExecuteReader(string sqlstr, ST st, int commandTimeOut)
        {
            object or = null;
            using (SqlCommand cmd = new SqlCommand(sqlstr, db, ((SqlTransaction)(st.st))))
            {
                cmd.CommandTimeout = commandTimeOut;
                or = cmd.ExecuteReader();
            }
            return or;
        }
        public Task<int> ExecSqlAsync(string sqlstr, int commandTimeOut)
        {
            using (SqlCommand cmd = new SqlCommand(sqlstr, db))
            {
                cmd.CommandTimeout = commandTimeOut;
                return cmd.ExecuteNonQueryAsync();
            }
        }
        public int ExecSql(string sqlstr, int commandTimeOut)
        {
            using (SqlCommand cmd = new SqlCommand(sqlstr,db))
            {
                cmd.CommandTimeout = commandTimeOut;
                return cmd.ExecuteNonQuery();
            }
        }
        public int ExecSql(string sqlstr, PR para, int commandTimeOut)
        {
            using (SqlCommand cmd = new SqlCommand(sqlstr, db))
            {
                cmd.Parameters.Add((SqlParameter)para.pr);
                cmd.CommandTimeout = commandTimeOut;
                return cmd.ExecuteNonQuery();
            }
        }
        public int ExecSql(string sqlstr, IEnumerable<PR> paras, int commandTimeOut)
        {
            using (SqlCommand cmd = new SqlCommand(sqlstr, db))
            {
                foreach (PR para in paras) cmd.Parameters.Add((SqlParameter)para.pr);
                cmd.CommandTimeout = commandTimeOut;
                return cmd.ExecuteNonQuery();
            }
        }
        public int ExecSql(string sqlstr, ST transaction, int commandTimeOut)
        {
            using (SqlCommand cmd = new SqlCommand(sqlstr, db, ((SqlTransaction)(transaction.st))))
            {
                cmd.CommandTimeout = commandTimeOut;
                return cmd.ExecuteNonQuery();
            }
        }
        public int ExecSql(string sqlstr, ST transaction, PR para, int commandTimeOut)
        {
            using (SqlCommand cmd = new SqlCommand(sqlstr, db, ((SqlTransaction)(transaction.st))))
            {
                cmd.Parameters.Add((SqlParameter)para.pr);
                cmd.CommandTimeout = commandTimeOut;
                return cmd.ExecuteNonQuery();
            }
        }
        public int ExecSql(string sqlstr, ST transaction, IEnumerable<PR> paras, int commandTimeOut)
        {
            using (SqlCommand cmd = new SqlCommand(sqlstr, db, ((SqlTransaction)(transaction.st))))
            {
                foreach (PR para in paras) cmd.Parameters.Add((SqlParameter)para.pr);
                cmd.CommandTimeout = commandTimeOut;
                return cmd.ExecuteNonQuery();
            }
        }
        public ST GetTransaction()
        {
            return new ST(db.BeginTransaction(IsolationLevel.ReadUncommitted), DataBase.SqlServer);
        }
        public ST GetTransaction(IsolationLevel isolationLevel)
        {
            return new ST(db.BeginTransaction(isolationLevel),DataBase.SqlServer);
        }
        public void Close()
        {
            db.Close();
            db.Dispose();
        }
    }
    public class DRSqlServer : DRInterface
    {
        private SqlDataReader dr;
        public DRSqlServer(SqlDataReader reader)
        {
            dr = reader;
        }
        public object DataReader
        {
            get { return dr; }
        }

        public int FieldCount => dr.FieldCount;

        public bool HasRows => dr.HasRows;

        public bool IsClosed => dr.IsClosed;

        public bool Read()
        {
            return dr.Read();
        }

        public string GetString(int i)
        {
            return dr.GetString(i);
        }

        public int GetInt16(int i)
        {
            return dr.GetInt16(i);
        }

        public int GetInt32(int i)
        {
            return dr.GetInt32(i);
        }

        public long GetInt64(int i)
        {
            return dr.GetInt64(i);
        }

        public int GetOrdinal(string col)
        {
            return dr.GetOrdinal(col);
        }
        public int FieldIndex(string FieldName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).ToLower() == FieldName.ToLower()) return i;
            }
            return -1;
        }

        public string GetName(int i)
        {
            return dr.GetName(i);
        }

        public object GetValue(int i)
        {
            return dr.GetValue(i);
        }

        public bool IsDBNull(int i)
        {
            return dr.IsDBNull(i);
        }

        public Type GetFieldType(int i)
        {
            return dr.GetFieldType(i);
        }

        public string GetDataTypeName(int i)
        {
            return dr.GetDataTypeName(i);
        }

        public DateTime GetDateTime(int i)
        {
            return dr.GetDateTime(i);
        }

        public decimal GetDecimal(int i)
        {
            return dr.GetDecimal(i);
        }

        public double GetDouble(int i)
        {
            return dr.GetDouble(i);
        }

        public double GetFloat(int i)
        {
            return dr.GetFloat(i);
        }

        public void Close()
        {
            dr.Close();
            dr.Dispose();
        }
    }
}
