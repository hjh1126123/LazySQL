namespace LazySQL.System
{
    public class SystemMediator
    {
        private static SystemMediator _instance;
        public static SystemMediator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SystemMediator();

                return _instance;
            }
        }

        #region 核心系统

        public DelegateSystem DelegateSystem { get; private set; }
        public ObjectSystem ObjectSystem { get; private set; }

        #endregion

        private SystemMediator()
        {
            #region 系统实例化

            DelegateSystem = new DelegateSystem(this);
            ObjectSystem = new ObjectSystem(this);

            #endregion
        }
    }
}
