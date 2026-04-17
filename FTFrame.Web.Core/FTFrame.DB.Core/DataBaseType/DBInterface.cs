using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTFrame.DBClient.DataBaseType
{
    public interface DBInterface
    {
        object DBConnection { get; }
        void Open(string ConnStr);
        object GetObject(string sqlstr, string colname, int commandTimeOut);
        string GetString(string sqlstr, string colname, int commandTimeOut);
        int GetInt(string sqlstr, int commandTimeOut);
        decimal GetDecimal(string sqlstr, int commandTimeOut);
        DataTable GetDataTable(string sql);
        DataSet GetDataSet(string sql, int StartRecord, int MaxRecord, string SrcTable);
        object ExecuteReader(string sqlstr, int commandTimeOut);
        object ExecuteReader(string sqlstr, ST st, int commandTimeOut);
        DR OpenRecord(string sqlstr, int commandTimeOut);
        DR OpenRecord(string sqlstr, ST st, int commandTimeOut);
        Task<int> ExecSqlAsync(string sqlstr, int commandTimeOut);
        int ExecSql(string sqlstr, int commandTimeOut);
        int ExecSql(string sqlstr, PR para, int commandTimeOut);
        int ExecSql(string sqlstr, IEnumerable<PR> paras, int commandTimeOut);
        int ExecSql(string sqlstr, ST transaction, int commandTimeOut);
        int ExecSql(string sqlstr, ST transaction, PR para, int commandTimeOut);
        int ExecSql(string sqlstr, ST transaction, IEnumerable<PR> paras, int commandTimeOut);
        void Close();
        ST GetTransaction();
        ST GetTransaction(IsolationLevel isolationLevel);
    }
    public interface DRInterface
    {
        object DataReader { get; }
        bool Read();
        string GetString(int i);
        int GetInt16(int i);
        int GetInt32(int i);
        long GetInt64(int i);
        int GetOrdinal(string col);

        int FieldIndex(string col);
        string GetName(int i);
        object GetValue(int i);
        bool IsDBNull(int i);
        Type GetFieldType(int i);
        string  GetDataTypeName(int i);
        DateTime GetDateTime(int i);
        decimal GetDecimal(int i);
        double GetDouble(int i);
        double GetFloat(int i);
        int FieldCount { get; }
        bool HasRows { get; }
        void Close();
        bool IsClosed { get; }
    }
}
