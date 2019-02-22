using LazySQL.Extends;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace LazySQL.SQLite
{
    public class SQLiteTemplate : ITemplate<SQLiteParameter>
    {
        private static SQLiteTemplate _instance;
        public static SQLiteTemplate Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SQLiteTemplate();

                return _instance;
            }
        }

        public override DataTable ExecuteDataTable(string name, StringBuilder cmdText, List<SQLiteParameter> commandParameters)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(cmdText.ToString()))
                throw new ArgumentNullException("commandText");

            SQLiteConnection sQLiteConnection = pools[name].GetConnection<SQLiteConnection>();

            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();

            PrepareCommand(cmd, sQLiteConnection, null, CommandType.Text, cmdText.ToString(), commandParameters.ToArray());

            try
            {
                sda.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sda.Dispose();
                cmd.Dispose();
                pools[name].FreeConnection(sQLiteConnection);
            }
        }

        public override ExecuteNonModel ExecuteNonQuery(string name, StringBuilder cmdText, List<SQLiteParameter> commandParameters)
        {
            int result = 0;
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(cmdText.ToString()))
                throw new ArgumentNullException("commandText");

            SQLiteConnection sQLiteConnection = pools[name].GetConnection<SQLiteConnection>();
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteTransaction sqlTransaction = sQLiteConnection.BeginTransaction(IsolationLevel.ReadCommitted);


            PrepareCommand(cmd, sQLiteConnection, sqlTransaction, CommandType.Text, cmdText.ToString(), commandParameters.ToArray());

            try
            {
                result = cmd.ExecuteNonQuery();
                sqlTransaction.Commit();
                return new ExecuteNonModel()
                {
                    Result = result,
                    Message = "操作执行成功",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                return new ExecuteNonModel()
                {
                    Result = result,
                    Message = $"操作执行错误{ex.Message}",
                    Success = false
                };
            }
            finally
            {
                sqlTransaction.Dispose();
                cmd.Dispose();
                pools[name].FreeConnection(sQLiteConnection);
            }
        }
    }
}
