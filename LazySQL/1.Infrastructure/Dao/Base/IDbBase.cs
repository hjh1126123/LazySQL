using System.Data;

namespace LazySQL.Infrastructure.Dao.Base
{
    public interface IDBBase
    {
        /// <summary>  
        /// 数据库连接字符串  
        /// </summary>  
        string ConnStr { get; set; }

        /// <summary>  
        /// <span style="font-family: Arial, Helvetica, sans-serif;">建立Connection对象</span>  
        /// </summary>  
        /// <returns>Connection对象</returns>  
        IDbConnection CreateConnection();

        /// <summary>  
        /// 根据连接字符串建立Connection对象  
        /// </summary>  
        /// <param name="strConn">连接字符串</param>  
        /// <returns>Connection对象</returns>  
        IDbConnection CreateConnection(string strConn);

        /// <summary>  
        /// 建立Command对象  
        /// </summary>  
        /// <returns>Command对象</returns>  
        IDbCommand CreateCommand();

        /// <summary>  
        /// 建立DataAdapter对象  
        /// </summary>  
        /// <returns>DataAdapter对象</returns>  
        IDbDataAdapter CreateDataAdapter();

        /// <summary>  
        /// 根据Connection建立Transaction  
        /// </summary>  
        /// <param name="myConn">Connection对象</param>  
        /// <returns>Transaction对象</returns>  
        IDbTransaction CreateTransaction(IDbConnection myConn);

        /// <summary>  
        /// 根据Command对象建立DataReader  
        /// </summary>  
        /// <param name="myComm">Command对象</param>  
        /// <returns>DataReader对象</returns>  
        IDataReader CreateDataReader(IDbCommand myComm);
    }
}
