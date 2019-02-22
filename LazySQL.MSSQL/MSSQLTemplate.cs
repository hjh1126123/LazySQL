using LazySQL.Extends;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LazySQL.MSSQL
{
    public class MSSQLTemplate : ITemplate<SqlParameter>
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

        public override DataTable ExecuteDataTable(string name, StringBuilder cmdText, List<SqlParameter> commandParameters)
        {        
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(cmdText.ToString()))
                throw new ArgumentNullException("commandText");

            SqlConnection sQLiteConnection = pools[name].GetConnection<SqlConnection>();

            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
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

        public override ExecuteNonModel ExecuteNonQuery(string name, StringBuilder cmdText, List<SqlParameter> commandParameters)
        {
            int result = 0;
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(cmdText.ToString()))
                throw new ArgumentNullException("commandText");

            SqlConnection sQLiteConnection = pools[name].GetConnection<SqlConnection>();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction sqlTransaction = sQLiteConnection.BeginTransaction(IsolationLevel.ReadCommitted);


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
