using LazySQL.Infrastructure;
using System;
using System.Data;
using System.Data.SQLite;

namespace LazySQL.SQLite
{
    public class SQLitePool : IPool
    {
        /// <summary>
        /// 连接对象
        /// </summary>
        private class ObjSqllite : IDynamicObject
        {
            private SQLiteConnection _SqlConn;

            public ObjSqllite()
            {
                _SqlConn = null;
            }

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
        }

        /// <summary>
        /// 初始化连接池
        /// </summary>
        /// <param name="connection">连接字段</param>
        /// <param name="initcount">初始连接数</param>
        /// <param name="capacity">最大连接数</param>
        public SQLitePool(string connection, int initcount, int capacity) : base(connection, initcount, capacity)
        {
            _Connections = new ObjectPool(typeof(ObjSqllite), connection, initcount, capacity);
        }
    }
}
