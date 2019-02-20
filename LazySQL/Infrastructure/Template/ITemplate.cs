using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LazySQL.Infrastructure
{
    public abstract class ITemplate<Paramter>
    {
        protected ConcurrentDictionary<string, IPool> DBPools;
        public ITemplate()
        {
            DBPools = new ConcurrentDictionary<string, IPool>();
        }

        public abstract void AddPool(string name, string conn, int initCount, int capacity);

        public abstract ExecuteNonModel ExecuteNonQuery(string name, StringBuilder cmdText, List<Paramter> commandParameters);

        public abstract DataTable ExecuteDataTable(string name, StringBuilder cmdText, List<Paramter> commandParameters);

        /// <summary>
        /// 预处理
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        protected void PrepareCommand(IDbCommand cmd, IDbConnection conn, IDbTransaction trans, CommandType cmdType, string cmdText, Paramter[] cmdParms)
        {
            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (Paramter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}
