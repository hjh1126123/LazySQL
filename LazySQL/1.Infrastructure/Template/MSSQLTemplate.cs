using System;
using System.Data;
using System.Data.SqlClient;

namespace LazySQL.Infrastructure
{
    public class MSSQLTemplate
    {
        /// <summary>
        /// 执行返回操作条数(通过connStr)(携带事务)
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string connectionString, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            conn.Open();
            SqlTransaction sqlTransaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                PrepareCommand(cmd, conn, sqlTransaction, CommandType.Text, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                sqlTransaction.Commit();
                return val;
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                throw ex.ThrowMineFormat(this, "ExecuteNonQuery", connectionString, cmdText, cmd.CommandText);
            }
            finally
            {
                conn.Dispose();
                cmd.Dispose();
                sqlTransaction.Dispose();
            }
        }

        /// <summary>
        /// 执行返回数据表
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string connectionString, string cmdText, params SqlParameter[] commandParameters)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, commandParameters);
                cmd.Parameters.Clear();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex.ThrowMineFormat(this, "ExecuteDataTable", connectionString, cmdText, cmd.CommandText);
            }
            finally
            {
                dt.Dispose();
                cmd.Dispose();
                da.Dispose();
                conn.Dispose();
            }
        }

        /// <summary>
        /// 预处理
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        private void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}