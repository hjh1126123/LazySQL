using System.Data;
using System.Data.Odbc;

namespace LazySQL.Infrastructure.Dao.Base
{
    public class ODBCBase : IDBBase
    {
        public string ConnStr { get; set; }

        public IDbCommand CreateCommand()
        {
            return new OdbcCommand();
        }

        public IDbConnection CreateConnection()
        {
            return new OdbcConnection(ConnStr);
        }

        public IDbConnection CreateConnection(string strConn)
        {
            return new OdbcConnection(strConn);
        }

        public IDbDataAdapter CreateDataAdapter()
        {
            return new OdbcDataAdapter();
        }

        public IDataReader CreateDataReader(IDbCommand myComm)
        {
            return myComm.ExecuteReader();
        }

        public IDbTransaction CreateTransaction(IDbConnection myConn)
        {
            return myConn.BeginTransaction();
        }
    }
}
