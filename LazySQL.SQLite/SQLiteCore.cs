using LazySQL.Extends;
using LazySQL.SQLite.CoreFactory;

namespace LazySQL.SQLite
{
    public class SQLiteCore : ICore
    {
        public SQLiteCore()
        {
            coreFactory = new SQLiteCoreFactory();
        }
    }
}
