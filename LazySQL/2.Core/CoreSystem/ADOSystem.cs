using System.Collections.Generic;
using System.Data;

namespace LazySQL.Core.CoreSystem
{
    public class ADOSystem : ISystem
    {
        Dictionary<string, IDbConnection> dbConnections;
        public ADOSystem(SystemMediator systemMediator) : base(systemMediator)
        {
            dbConnections = new Dictionary<string, IDbConnection>();
        }

        

        public void AddConnection(string connName,IDbConnection dbConnection)
        {
            if (dbConnections.ContainsKey(connName))
                dbConnections[connName] = dbConnection;
            else
                dbConnections.Add(connName, dbConnection);
        }
    }
}
