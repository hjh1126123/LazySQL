using System.Data;
using System.Data.OleDb;

namespace LazySQL.Infrastructure.Dao.Base
{
    public class OLEDBBase : IDBBase
    {
        public string ConnStr { get; set; }

        public IDbCommand CreateCommand()
        {
            return new OleDbCommand();
        }

        public IDbConnection CreateConnection()
        {
            return new OleDbConnection(ConnStr);
        }

        public IDbConnection CreateConnection(string strConn)
        {
            return new OleDbConnection(strConn);
        }

        public IDbDataAdapter CreateDataAdapter()
        {
            return new OleDbDataAdapter();
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
