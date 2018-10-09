using System.Data;
using System.Data.SqlClient;

namespace LazySQL.Infrastructure.Dao.Base
{
    public class MSSQLBase : IDBBase
    {
        public string ConnStr { get; set; }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(ConnStr);
        }

        public IDbConnection CreateConnection(string strConn)
        {
            return new SqlConnection(strConn);
        }

        public IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        public IDbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }

        public IDbTransaction CreateTransaction(IDbConnection myConn)
        {
            return myConn.BeginTransaction();
        }

        public IDataReader CreateDataReader(IDbCommand myComm)
        {
            return myComm.ExecuteReader();
        }
    }
}
