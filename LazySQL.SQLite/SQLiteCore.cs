using LazySQL.Core;

namespace LazySQL.SQLite
{
    public class SQLiteCore : ICore
    {
        public SQLiteCore()
        {
            coreFactory = new SQLiteCoreFactory(SystemMediator);
        }
    }
}
