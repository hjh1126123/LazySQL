using System;

namespace LazySQL.Infrastructure
{
    public class IPool
    {
        protected ObjectPool _Connections;

        /// <summary>
        /// 初始化对象池
        /// </summary>
        /// <param name="connection">连接字段</param>
        /// <param name="initcount">初始化连接数</param>
        /// <param name="capacity">连接数最大值</param>
        /// <param name="dBType">数据库类型</param>
        public IPool(string connection, int initcount, int capacity)
        {
            if (connection == null || connection == "" || initcount < 0 || capacity < 1)
            {
                throw (new Exception("Invalid parameter!"));
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
