using LazySQL.Infrastructure.Dao.Base;
using LazySQL.Infrastructure.Dao.Tool;

namespace LazySQL.Infrastructure.Dao
{
    public class DaoMain
    {
        private static DaoMain _instance;
        public static DaoMain GetInstance()
        {
            if (_instance == null)
                _instance = new DaoMain();

            return _instance;
        }
        private DaoMain() { }

        private IDBBase idbBase;
        public IDBBase IdbBase
        {
            get => idbBase;
        }

        private SQLPrepare sQLPrepare;
        public SQLPrepare GetSQLPrepare()
        {
            if (sQLPrepare == null)
                sQLPrepare = new SQLPrepare();

            return sQLPrepare;
        }

        private string SQLConnection;

        public void Restiger(string SQLConnection)
        {
            this.SQLConnection = SQLConnection;
        }


        public void BuildDB(DBType dBType)
        {
            switch (dBType)
            {
                case DBType.MSSQLSERVER:
                    idbBase = new MSSQLBase();
                    idbBase.ConnStr = SQLConnection;
                    break;
                case DBType.ODBC:
                    idbBase = new ODBCBase();
                    idbBase.ConnStr = SQLConnection;
                    break;
                case DBType.OLEDB:
                    idbBase = new OLEDBBase();
                    idbBase.ConnStr = SQLConnection;
                    break;
            }
        }
    }
}
