using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;

namespace LazySQL.Infrastructure
{
    public class MSSQLTemplate
    {
        private static MSSQLTemplate _instance;
        public static MSSQLTemplate Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MSSQLTemplate();

                return _instance;
            }
        }

        #region 基础参数

        ConcurrentDictionary<string, DBPool> DBPools;

        #endregion

        #region 基础参数控制

        public void AddPool(string name, string conn, int initCount, int capacity)
        {
            DBPool dBPool = new DBPool(conn, initCount, capacity, DB.MSSQL);
            DBPools.AddOrUpdate(name, dBPool, (key, oldValue) => dBPool);
        }

        #endregion

        #region 构造方法

        private MSSQLTemplate()
        {
            DBPools = new ConcurrentDictionary<string, DBPool>();
        }

        #endregion

        /// <summary>
        /// 执行返回操作条数(通过connStr)(携带事务)
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string name, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlConnection conn = DBPools[name].GetConnection<SqlConnection>();
            SqlCommand cmd = new SqlCommand();
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
                throw ex;
            }
            finally
            {
                DBPools[name].FreeConnection(conn);
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
        public DataTable ExecuteDataTable(string name, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlConnection conn = DBPools[name].GetConnection<SqlConnection>();            
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            try
            {
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, commandParameters);
                cmd.Parameters.Clear();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt.Dispose();
                cmd.Dispose();
                da.Dispose();
                DBPools[name].FreeConnection(conn);
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