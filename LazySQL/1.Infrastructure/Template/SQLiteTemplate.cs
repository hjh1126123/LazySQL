using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace LazySQL.Infrastructure
{
    /// <summary>
    /// 本类为SQLite数据库帮助静态类,使用时只需直接调用即可,无需实例化
    /// </summary>
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

        readonly Dictionary<string,SQLiteConnection> sQLiteConnection;
        private SQLiteTemplate()
        {
            sQLiteConnection = new Dictionary<string, SQLiteConnection>();
        }

        /// <summary>
        /// 添加连接库
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="connText">连接字符串</param>
        public void AddConnection(string name,string connText)
        {
            if (sQLiteConnection == null)
                return;

            if (!sQLiteConnection.ContainsKey(name))
            {
                sQLiteConnection.Add(name, new SQLiteConnection(connText));
            }
            else
            {
                sQLiteConnection[name] = new SQLiteConnection(connText);
            }
        }

        #endregion

        #region 执行数据库操作(新增、更新或删除)，返回影响行数

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>所受影响的行数</returns>
        public ExecuteNonModel ExecuteNonQuery(string connName, StringBuilder commandText, List<SQLiteParameter> cmdParms)
        {
            int result = 0;
            if (connName == null || connName.Length == 0)
                throw new ArgumentNullException("connName");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, sQLiteConnection[connName], ref trans, true, CommandType.Text, commandText.ToString(), cmdParms.ToArray());
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
                if (sQLiteConnection != null)
                {
                    if (sQLiteConnection[connName].State == ConnectionState.Open)
                    {
                        sQLiteConnection[connName].Close();
                    }
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
        public DataTable ExecuteDataTable(string connName, StringBuilder commandText, List<SQLiteParameter> cmdParms)
        {
            if (connName == null || connName.Length == 0)
                throw new ArgumentNullException("connName");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            DataTable dt = new DataTable();            
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, sQLiteConnection[connName], ref trans, false, CommandType.Text, commandText.ToString(), cmdParms.ToArray());
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
                if (sQLiteConnection != null)
                {
                    if (sQLiteConnection[connName].State == ConnectionState.Open)
                    {
                        sQLiteConnection[connName].Close();
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
        private void PrepareCommand(SQLiteCommand cmd
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
