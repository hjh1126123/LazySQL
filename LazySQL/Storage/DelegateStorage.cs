using System;
using System.Collections.Generic;
using System.Data;

namespace LazySQL.Storage
{
    public class DelegateStorage
    {
        #region DT委托

        public readonly Dictionary<string, Func<DataTable>> noParamsFuncDT;
        public readonly Dictionary<string, Func<string, DataTable>> oneParamsFuncDT;
        public readonly Dictionary<string, Func<string, string, DataTable>> twoParamsFuncDT;
        public readonly Dictionary<string, Func<string, string, string, DataTable>> threeParamsFuncDT;
        public readonly Dictionary<string, Func<string, string, string, string, DataTable>> fourParamsFuncDT;
        public readonly Dictionary<string, Func<string, string, string, string, string, DataTable>> fiveParamsFuncDT;
        public readonly Dictionary<string, Func<string, string, string, string, string, string, DataTable>> sixParamsFuncDT;
        public readonly Dictionary<string, Func<string, string, string, string, string, string, string, DataTable>> sevenParamsFuncDT;
        public readonly Dictionary<string, Func<string, string, string, string, string, string, string, string, DataTable>> eightParamsFuncDT;
        public readonly Dictionary<string, Func<string, string, string, string, string, string, string, string, string, DataTable>> nineParamsFuncDT;
        public readonly Dictionary<string, Func<string, string, string, string, string, string, string, string, string, string, DataTable>> tenParamsFuncDT;

        #endregion

        #region ExecuteNonModel委托

        public readonly Dictionary<string, Func<ExecuteNonModel>> noParamsFuncExecuteNonModel;
        public readonly Dictionary<string, Func<string, ExecuteNonModel>> oneParamsFuncExecuteNonModel;
        public readonly Dictionary<string, Func<string, string, ExecuteNonModel>> twoParamsFuncExecuteNonModel;
        public readonly Dictionary<string, Func<string, string, string, ExecuteNonModel>> threeParamsFuncExecuteNonModel;
        public readonly Dictionary<string, Func<string, string, string, string, ExecuteNonModel>> fourParamsFuncExecuteNonModel;
        public readonly Dictionary<string, Func<string, string, string, string, string, ExecuteNonModel>> fiveParamsFuncExecuteNonModel;
        public readonly Dictionary<string, Func<string, string, string, string, string, string, ExecuteNonModel>> sixParamsFuncExecuteNonModel;
        public readonly Dictionary<string, Func<string, string, string, string, string, string, string, ExecuteNonModel>> sevenParamsFuncExecuteNonModel;
        public readonly Dictionary<string, Func<string, string, string, string, string, string, string, string, ExecuteNonModel>> eightParamsFuncExecuteNonModel;
        public readonly Dictionary<string, Func<string, string, string, string, string, string, string, string, string, ExecuteNonModel>> nineParamsFuncExecuteNonModel;
        public readonly Dictionary<string, Func<string, string, string, string, string, string, string, string, string, string, ExecuteNonModel>> tenParamsFuncExecuteNonModel;

        #endregion

        public DelegateStorage()
        {
            #region 实例化BL委托存储

            noParamsFuncExecuteNonModel = new Dictionary<string, Func<ExecuteNonModel>>();
            oneParamsFuncExecuteNonModel = new Dictionary<string, Func<string, ExecuteNonModel>>();
            twoParamsFuncExecuteNonModel = new Dictionary<string, Func<string, string, ExecuteNonModel>>();
            threeParamsFuncExecuteNonModel = new Dictionary<string, Func<string, string, string, ExecuteNonModel>>();
            fourParamsFuncExecuteNonModel = new Dictionary<string, Func<string, string, string, string, ExecuteNonModel>>();
            fiveParamsFuncExecuteNonModel = new Dictionary<string, Func<string, string, string, string, string, ExecuteNonModel>>();
            sixParamsFuncExecuteNonModel = new Dictionary<string, Func<string, string, string, string, string, string, ExecuteNonModel>>();
            sevenParamsFuncExecuteNonModel = new Dictionary<string, Func<string, string, string, string, string, string, string, ExecuteNonModel>>();
            eightParamsFuncExecuteNonModel = new Dictionary<string, Func<string, string, string, string, string, string, string, string, ExecuteNonModel>>();
            nineParamsFuncExecuteNonModel = new Dictionary<string, Func<string, string, string, string, string, string, string, string, string, ExecuteNonModel>>();
            tenParamsFuncExecuteNonModel = new Dictionary<string, Func<string, string, string, string, string, string, string, string, string, string, ExecuteNonModel>>();

            #endregion

            #region 实例化DT委托存储

            noParamsFuncDT = new Dictionary<string, Func<DataTable>>();
            oneParamsFuncDT = new Dictionary<string, Func<string, DataTable>>();
            twoParamsFuncDT = new Dictionary<string, Func<string, string, DataTable>>();
            threeParamsFuncDT = new Dictionary<string, Func<string, string, string, DataTable>>();
            fourParamsFuncDT = new Dictionary<string, Func<string, string, string, string, DataTable>>();
            fiveParamsFuncDT = new Dictionary<string, Func<string, string, string, string, string, DataTable>>();
            sixParamsFuncDT = new Dictionary<string, Func<string, string, string, string, string, string, DataTable>>();
            sevenParamsFuncDT = new Dictionary<string, Func<string, string, string, string, string, string, string, DataTable>>();
            eightParamsFuncDT = new Dictionary<string, Func<string, string, string, string, string, string, string, string, DataTable>>();
            nineParamsFuncDT = new Dictionary<string, Func<string, string, string, string, string, string, string, string, string, DataTable>>();
            tenParamsFuncDT = new Dictionary<string, Func<string, string, string, string, string, string, string, string, string, string, DataTable>>();

            #endregion
        }
    }
}
