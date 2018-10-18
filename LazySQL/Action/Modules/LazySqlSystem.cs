using LazySQL.Core.CoreSystem;
using LazySQL.Infrastructure;
using System.Data;

namespace LazySQL.Action.Modules
{
    public class LazySqlSystem
    {
        private static LazySqlSystem _instance;
        /// <summary>
        /// 系统单例
        /// </summary>
        /// <returns></returns>
        public static LazySqlSystem GetInstance()
        {
            if (_instance == null)
                _instance = new LazySqlSystem();

            return _instance;
        }

        /// <summary>
        /// 执行方法，返回DataTable
        /// </summary>
        /// <param name="name">方法名</param>
        /// <param name="args">方法参数</param>
        /// <returns></returns>
        public DataTable Method_DataTable(string name, params string[] args)
        {
            return SystemMediator.GetInstance().FuncDT(name, args);
        }

        /// <summary>
        /// 执行方法，返回Bool
        /// </summary>
        /// <param name="name">方法名</param>
        /// <param name="args">方法参数</param>
        /// <returns></returns>
        public ExecuteNonModel Method_ExecuteNonModel(string name, params string[] args)
        {
            return SystemMediator.GetInstance().FuncExecuteNonModel(name, args);
        }
    }
}
