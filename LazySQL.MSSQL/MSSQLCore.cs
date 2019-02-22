using LazySQL.Extends;
using LazySQL.MSSQL.CoreFactory;

namespace LazySQL.MSSQL
{
    public class MSSQLCore : ICore
    {
        public MSSQLCore()
        {
            coreFactory = new MSSQLCoreFactory();
        }
    }
}
