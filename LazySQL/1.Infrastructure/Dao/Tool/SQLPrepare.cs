using System;
using System.Data;

namespace LazySQL.Infrastructure.Dao.Tool
{
    public class SQLPrepare
    {
        /// <summary>
        /// 预编译
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="mustCloseConnection"></param>
        public void PrepareCommand(IDbCommand command, IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDbDataParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            command.Connection = connection;
            command.CommandText = commandText;
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("这个事物的连接对象是空的", "transaction");
                command.Transaction = transaction;
            }
            command.CommandType = commandType;
            if (commandParameters != null)
            {
                if (command == null) throw new ArgumentNullException("command");
                if (commandParameters != null)
                {
                    foreach (var p in commandParameters)
                    {
                        if (p != null)
                        {
                            if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) &&
                                (p.Value == null))
                            {
                                p.Value = DBNull.Value;
                            }
                            command.Parameters.Add(p);
                        }
                    }
                }
            }
        }
    }
}
