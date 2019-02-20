using LazySQL.Infrastructure;
using System;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Collections.Generic;

namespace LazySQL.SQLite
{
    public class SQLiteTemplate : ITemplate<SQLiteParameter>
    {
        public SQLiteTemplate()
        {
        }

        /// <summary>
        /// 添加池
        /// </summary>
        /// <param name="name">池名称</param>
        /// <param name="conn">池连接对象</param>
        /// <param name="initCount">初始连接数</param>
        /// <param name="capacity">最大连接数</param>
        public override void AddPool(string name, string conn, int initCount, int capacity)
        {
            SQLitePool sQLLitePool = new SQLitePool(conn, initCount, capacity);
            DBPools.AddOrUpdate(name, sQLLitePool, (key, oldValue) => sQLLitePool);
        }

        /// <summary>
        /// 执行读取操作
        /// </summary>
        /// <param name="name">池名称</param>
        /// <param name="commandText">请求字段</param>
        /// <param name="cmdParms">请求参数</param>
        /// <returns>DataTable</returns>
        public override DataTable ExecuteDataTable(string name, StringBuilder cmdText, List<SQLiteParameter> commandParameters)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(cmdText.ToString()))
                throw new ArgumentNullException("commandText");

            SQLiteConnection sQLiteConnection = DBPools[name].GetConnection<SQLiteConnection>();

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
                DBPools[name].FreeConnection(sQLiteConnection);
            }
        }

        /// <summary>
        /// 执行写入操作
        /// </summary>
        /// <param name="name">池名称</param>
        /// <param name="cmdText">请求字段</param>
        /// <param name="commandParameters">请求参数</param>
        /// <returns></returns>
        public override ExecuteNonModel ExecuteNonQuery(string name, StringBuilder cmdText, List<SQLiteParameter> commandParameters)
        {
            int result = 0;
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(cmdText.ToString()))
                throw new ArgumentNullException("commandText");

            SQLiteConnection sQLiteConnection = DBPools[name].GetConnection<SQLiteConnection>();
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
                DBPools[name].FreeConnection(sQLiteConnection);
            }
        }
    }
}
