using LazySQL.Extends;
using LazySQL.System;


namespace LazySQL.MSSQL
{
    public class MSSQLFactory : IFactory
    {
        public MSSQLFactory()
        {
            Core = new MSSQLCore();
        }

        public override void AddConnection(string name, string connText, int initCount, int capacity, int maxCondition)
        {
            base.AddConnection(name, connText, initCount, capacity, maxCondition);
            SystemMediator.Instance.ObjectSystem.PoolAdd(name, new MSSQLPool(connText, initCount, capacity));
        }
    }
}
