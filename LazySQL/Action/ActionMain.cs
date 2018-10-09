using LazySQL.Action.Modules;

namespace LazySQL.Action
{
    public enum RETURN_TYPE
    {
        DATA_TABLE,
        BOOL
    }

    /// <summary>
    /// lazySQL行动类，使用lazySQL用它足矣
    /// </summary>
    public class ActionMain
    {
        private static ActionMain _instance;
        /// <summary>
        /// lazySql行动单例
        /// </summary>
        /// <returns></returns>
        public static ActionMain Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ActionMain();

                return _instance;
            }
        }

        /// <summary>
        /// 获取工厂
        /// </summary>
        /// <returns></returns>
        public LazySqlFactory GetFactory()
        {
            return LazySqlFactory.GetInstance();
        }

        /// <summary>
        /// 获取系统
        /// </summary>
        /// <returns></returns>
        public LazySqlSystem GetSystem()
        {
            return LazySqlSystem.GetInstance();
        }
    }
}
