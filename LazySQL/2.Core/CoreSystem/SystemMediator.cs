using System;
using System.Data;

namespace LazySQL.Core.CoreSystem
{
    public class SystemMediator
    {
        private static SystemMediator _instance;
        public static SystemMediator GetInstance()
        {
            if (_instance == null)
                _instance = new SystemMediator();

            return _instance;
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

        public bool FuncBool(string name, params string[] pars)
        {
            switch (pars.Length)
            {
                case 0:
                    return StorageSystem.noParamsFuncBL[name]();

                case 1:
                    return StorageSystem.oneParamsFuncBL[name](pars[0]);

                case 2:
                    return StorageSystem.twoParamsFuncBL[name](pars[0], pars[1]);

                case 3:
                    return StorageSystem.threeParamsFuncBL[name](pars[0], pars[1], pars[2]);

                case 4:
                    return StorageSystem.fourParamsFuncBL[name](pars[0], pars[1], pars[2], pars[3]);

                case 5:
                    return StorageSystem.fiveParamsFuncBL[name](pars[0], pars[1], pars[2], pars[3], pars[4]);

                case 6:
                    return StorageSystem.sixParamsFuncBL[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5]);

                case 7:
                    return StorageSystem.sevenParamsFuncBL[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6]);

                case 8:
                    return StorageSystem.eightParamsFuncBL[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6], pars[7]);

                case 9:
                    return StorageSystem.nineParamsFuncBL[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6], pars[7], pars[8]);

                case 10:
                    return StorageSystem.tenParamsFuncBL[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6], pars[7], pars[8], pars[9]);
            }
            return false;
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
