using System;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Collections.Generic;

namespace LazySQL.Infrastructure
{
    /// <summary>
    /// 本类为SQLite数据库帮助静态类,使用时只需直接调用即可,无需实例化
    /// </summary>
    public class SQLiteTemplate
    {
        #region 执行数据库操作(新增、更新或删除)，返回影响行数

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>所受影响的行数</returns>
        public bool ExecuteNonQuery(string connectionString, StringBuilder commandText, List<SQLiteParameter> cmdParms)
        {
            int result = 0;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                SQLiteTransaction trans = null;
                PrepareCommand(cmd, con, ref trans, true, CommandType.Text, commandText.ToString(), cmdParms.ToArray());
                try
                {
                    result = cmd.ExecuteNonQuery();
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        #endregion

        #region 执行数据库查询，返回DataTable对象

        /// <summary>
        /// 执行数据库查询，返回DataTable对象
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <returns>DataTable对象</returns>
        public DataTable ExecuteDataTable(string connectionString, StringBuilder commandText, List<SQLiteParameter> cmdParms)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            DataTable dt = new DataTable();
            SQLiteConnection con = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, con, ref trans, false, CommandType.Text, commandText.ToString(), cmdParms.ToArray());
            try
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return dt;
        }

        #endregion

        #region 预处理Command对象,数据库链接,事务,需要执行的对象,参数等的初始化

        /// <summary>
        /// 预处理Command对象,数据库链接,事务,需要执行的对象,参数等的初始化
        /// </summary>
        /// <param name="cmd">Command对象</param>
        /// <param name="conn">Connection对象</param>
        /// <param name="trans">Transcation对象</param>
        /// <param name="useTrans">是否使用事务</param>
        /// <param name="cmdType">SQL字符串执行类型</param>
        /// <param name="cmdText">SQL Text</param>
        /// <param name="cmdParms">SQLiteParameters to use in the command</param>
        private static void PrepareCommand(SQLiteCommand cmd
            , SQLiteConnection conn
            , ref SQLiteTransaction trans
            , bool useTrans
            , CommandType cmdType
            , string cmdText
            , params SQLiteParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (useTrans)
            {
                trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                cmd.Transaction = trans;
            }


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
