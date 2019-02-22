using LazySQL.Extends;
using LazySQL.System;

namespace LazySQL.SQLite
{
    public class SQLiteFactory : IFactory
    {
        public SQLiteFactory()
        {
            Core = new SQLiteCore();
        }

        public override void AddConnection(string name, string connText, int initCount, int capacity, int maxCondition)
        {
            base.AddConnection(name, connText, initCount, capacity, maxCondition);
            SystemMediator.Instance.ObjectSystem.PoolAdd(name, new SQLitePool(connText, initCount, capacity));
        }
    }
}
