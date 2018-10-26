using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace LazySQL.Infrastructure
{
    public class SQLiteTemplate
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

        #region 基础参数

        ConcurrentDictionary<string, DBPool> DBPools;

        #endregion

        #region 基础参数控制

        public void AddPool(string name, string conn, int initCount, int capacity)
        {
            DBPool dBPool = new DBPool(conn, initCount, capacity, DB.SQLLITE);
            DBPools.AddOrUpdate(name, dBPool, (key, oldValue) => dBPool);
        }

        #endregion

        #region 构造方法

        private SQLiteTemplate()
        {
            DBPools = new ConcurrentDictionary<string, DBPool>();
        }

        #endregion

        #region 执行数据库操作(新增、更新或删除)，返回影响行数

        /// <summary>
        /// 执行写入操作
        /// </summary>
        /// <param name="name">数据库名称</param>
        /// <param name="commandText"></param>
        /// <param name="cmdParms"></param>
        /// <returns>ExecuteNonModel</returns>
        public ExecuteNonModel ExecuteNonQuery(string name, StringBuilder commandText, List<SQLiteParameter> cmdParms)
        {
            int result = 0;
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteConnection sQLiteConnection = DBPools[name].GetConnection<SQLiteConnection>();

            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, sQLiteConnection, out SQLiteTransaction trans, CommandType.Text, commandText.ToString(), cmdParms.ToArray());
            try
            {
                result = cmd.ExecuteNonQuery();
                trans.Commit();
                return new ExecuteNonModel()
                {
                    Result = result,
                    Message = $"执行成功",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                cmd.Dispose();
                DBPools[name].FreeConnection(sQLiteConnection);
            }
        }

        #endregion

        #region 执行数据库查询，返回DataTable对象

        /// <summary>
        /// 执行读取操作
        /// </summary>
        /// <param name="name">数据库名称</param>
        /// <param name="commandText">访问字符串</param>
        /// <param name="cmdParms">访问参数</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string name, StringBuilder commandText, List<SQLiteParameter> cmdParms)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteConnection sQLiteConnection = DBPools[name].GetConnection<SQLiteConnection>();

            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();

            PrepareCommand(cmd, sQLiteConnection, CommandType.Text, commandText.ToString(), cmdParms.ToArray());
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

        #endregion

        #region 预处理Command对象,数据库链接,事务,需要执行的对象,参数等的初始化

        /// <summary>
        /// 带事务
        /// </summary>
        private void PrepareCommand(SQLiteCommand cmd
            , SQLiteConnection conn
            , out SQLiteTransaction trans
            , CommandType cmdType
            , string cmdText
            , params SQLiteParameter[] cmdParms)
        {
            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SQLiteParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// 不带事务
        /// </summary>
        private void PrepareCommand(SQLiteCommand cmd
            , SQLiteConnection conn
            , CommandType cmdType
            , string cmdText
            , params SQLiteParameter[] cmdParms)
        {
            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SQLiteParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion

    }
}
