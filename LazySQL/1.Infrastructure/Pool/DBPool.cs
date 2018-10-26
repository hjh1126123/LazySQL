using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace LazySQL.Infrastructure
{
    public enum DB
    {
        MSSQL,
        SQLLITE
    }

    public class DBPool
    {
        private class MSSQL_Object : IDynamicObject
        {
            private SqlConnection _SqlConn;

            public MSSQL_Object()
            {
                _SqlConn = null;
            }

            #region IDynamicObject Members

            public void Create(Object param)
            {
                String strConn = (String)param;
                _SqlConn = new SqlConnection(strConn);
                _SqlConn.Open();
            }

            public Object GetInnerObject()
            { 
                return _SqlConn;
            }

            public bool IsValidate()
            {
                return (_SqlConn != null
                    && _SqlConn.GetHashCode() > 0
                    && _SqlConn.State == ConnectionState.Open);
            }

            public void Release()
            {
                _SqlConn.Close();
            }

            #endregion
        }

        private class SQLite_Object : IDynamicObject
        {
            private SQLiteConnection _SqlConn;

            public SQLite_Object()
            {
                _SqlConn = null;
            }

            #region IDynamicObject Members

            public void Create(Object param)
            {
                String strConn = (String)param;
                _SqlConn = new SQLiteConnection(strConn);
                _SqlConn.Open();
            }

            public Object GetInnerObject()
            {
                return _SqlConn;
            }

            public bool IsValidate()
            {
                return (_SqlConn != null
                    && _SqlConn.GetHashCode() > 0
                    && _SqlConn.State == ConnectionState.Open);
            }

            public void Release()
            {
                _SqlConn.Close();
            }

            #endregion
        }        

        private ObjectPool _Connections;

        /// <summary>
        /// 初始化对象池
        /// </summary>
        /// <param name="connection">连接字段</param>
        /// <param name="initcount">初始化连接数</param>
        /// <param name="capacity">连接数最大值</param>
        /// <param name="dBType">数据库类型</param>
        public DBPool(string connection, int initcount, int capacity, DB dBType)
        {
            if (connection == null || connection == "" || initcount < 0 || capacity < 1)
            {
                throw (new Exception("Invalid parameter!"));
            }
            switch (dBType)
            {
                case DB.MSSQL:
                    _Connections = new ObjectPool(typeof(MSSQL_Object), connection, initcount, capacity);
                    break;

                case DB.SQLLITE:
                    _Connections = new ObjectPool(typeof(SQLite_Object), connection, initcount, capacity);
                    break;
            }            
        }

        /// <summary>
        /// 获取数据连接对象
        /// </summary>
        /// <returns></returns>
        public T GetConnection<T>()
        {
            return (T)_Connections.GetOne();
        }

        /// <summary>
        /// 回收数据连接对象
        /// </summary>
        /// <param name="sqlConn"></param>
        public void FreeConnection<T>(T sqlConn)
        {
            _Connections.FreeObject(sqlConn);
        }

        /// <summary>
        /// 释放对象池
        /// </summary>
        public void Release()
        {
            _Connections.Release();
        }

        /// <summary>
        /// 对象池对象数量
        /// </summary>
        public int Count
        {
            get { return _Connections.CurrentSize; }
        }

        /// <summary>
        /// 对象池正在使用的对象数量
        /// </summary>
        public int UsingCount
        {
            get { return _Connections.ActiveCount; }
        }

        /// <summary>
        /// 减少对象池最大容量大小
        /// </summary>
        /// <param name="size">容量大小</param>
        /// <returns></returns>
        public int DecreaseSize(int size)
        {
            return _Connections.DecreaseSize(size);
        }
    }
}
