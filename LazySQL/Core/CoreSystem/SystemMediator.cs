using System.Data;

namespace LazySQL.Core.CoreSystem
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

        public StorageSystem StorageSystem { get; private set; }

        #endregion

        private SystemMediator()
        {
            #region 系统实例化

            StorageSystem = new StorageSystem(this);

            #endregion
        }


        #region 使用存储委托

        public ExecuteNonModel FuncExecuteNonModel(string name, params string[] pars)
        {
            switch (pars.Length)
            {
                case 0:
                    return StorageSystem.noParamsFuncExecuteNonModel[name]();

                case 1:
                    return StorageSystem.oneParamsFuncExecuteNonModel[name](pars[0]);

                case 2:
                    return StorageSystem.twoParamsFuncExecuteNonModel[name](pars[0], pars[1]);

                case 3:
                    return StorageSystem.threeParamsFuncExecuteNonModel[name](pars[0], pars[1], pars[2]);

                case 4:
                    return StorageSystem.fourParamsFuncExecuteNonModel[name](pars[0], pars[1], pars[2], pars[3]);

                case 5:
                    return StorageSystem.fiveParamsFuncExecuteNonModel[name](pars[0], pars[1], pars[2], pars[3], pars[4]);

                case 6:
                    return StorageSystem.sixParamsFuncExecuteNonModel[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5]);

                case 7:
                    return StorageSystem.sevenParamsFuncExecuteNonModel[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6]);

                case 8:
                    return StorageSystem.eightParamsFuncExecuteNonModel[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6], pars[7]);

                case 9:
                    return StorageSystem.nineParamsFuncExecuteNonModel[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6], pars[7], pars[8]);

                case 10:
                    return StorageSystem.tenParamsFuncExecuteNonModel[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6], pars[7], pars[8], pars[9]);
            }
            return new ExecuteNonModel()
            {
                Message = "参数超过10",
                Success = false,
                Result = 0
            };
        }

        public DataTable FuncDT(string name, params string[] pars)
        {
            switch (pars.Length)
            {
                case 0:
                    return StorageSystem.noParamsFuncDT[name]();

                case 1:
                    return StorageSystem.oneParamsFuncDT[name](pars[0]);

                case 2:
                    return StorageSystem.twoParamsFuncDT[name](pars[0], pars[1]);

                case 3:
                    return StorageSystem.threeParamsFuncDT[name](pars[0], pars[1], pars[2]);

                case 4:
                    return StorageSystem.fourParamsFuncDT[name](pars[0], pars[1], pars[2], pars[3]);

                case 5:
                    return StorageSystem.fiveParamsFuncDT[name](pars[0], pars[1], pars[2], pars[3], pars[4]);

                case 6:
                    return StorageSystem.sixParamsFuncDT[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5]);

                case 7:
                    return StorageSystem.sevenParamsFuncDT[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6]);

                case 8:
                    return StorageSystem.eightParamsFuncDT[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6], pars[7]);

                case 9:
                    return StorageSystem.nineParamsFuncDT[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6], pars[7], pars[8]);

                case 10:
                    return StorageSystem.tenParamsFuncDT[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6], pars[7], pars[8], pars[9]);
            }
            return null;
        }

        #endregion
    }
}
