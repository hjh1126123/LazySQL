using LazySQL.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace LazySQL.System
{
    /// <summary>
    /// 仓库存储系统
    /// </summary>
    public class DelegateSystem : ISystem
    {
        public enum RETURNTYPE
        {
            DATA,
            EXECUTE_NON_MODEL
        }

        DelegateStorage delegateStorage;

        #region 各委托对应锁

        #region DT委托锁

        private object noParams_DT;
        private object oneParams_DT;
        private object twoParams_DT;
        private object threeParams_DT;
        private object fourParams_DT;
        private object fiveParams_DT;
        private object sixParams_DT;
        private object sevenParams_DT;
        private object eightParams_DT;
        private object nineParams_DT;
        private object tenParams_DT;

        #endregion

        #region BL委托锁

        private object noParams_BL;
        private object oneParams_BL;
        private object twoParams_BL;
        private object threeParams_BL;
        private object fourParams_BL;
        private object fiveParams_BL;
        private object sixParams_BL;
        private object sevenParams_BL;
        private object eightParams_BL;
        private object nineParams_BL;
        private object tenParams_BL;

        #endregion

        #endregion

        /// <summary>
        /// 仓库存储系统
        /// </summary>
        /// <param name="systemMediator">系统模块中间层</param>
        public DelegateSystem(SystemMediator systemMediator) : base(systemMediator)
        {
            delegateStorage = new DelegateStorage();

            #region 实例化锁

            #region DT锁

            noParams_DT = new object();
            oneParams_DT = new object();
            twoParams_DT = new object();
            threeParams_DT = new object();
            fourParams_DT = new object();
            fiveParams_DT = new object();
            sixParams_DT = new object();
            sevenParams_DT = new object();
            eightParams_DT = new object();
            nineParams_DT = new object();
            tenParams_DT = new object();

            #endregion

            #region BL锁

            noParams_BL = new object();
            oneParams_BL = new object();
            twoParams_BL = new object();
            threeParams_BL = new object();
            fourParams_BL = new object();
            fiveParams_BL = new object();
            sixParams_BL = new object();
            sevenParams_BL = new object();
            eightParams_BL = new object();
            nineParams_BL = new object();
            tenParams_BL = new object();

            #endregion

            #endregion
        }

        private void Add<_func, _dict>(string funcName, MethodInfo methodInfo, object _lock, _dict dict)
            where _dict : IDictionary
        {
            lock (_lock)
            {
                if (!dict.Contains(funcName))
                {
                    dict.Add(funcName, Delegate.CreateDelegate(typeof(_func), methodInfo));
                }
            }
        }

        public void SaveMethodInfo(string funcName, MethodInfo methodInfo, int paramsCount, Type returnType)
        {
            RETURNTYPE rETURNTYPE = RETURNTYPE.DATA;
            if (returnType == typeof(DataTable))
            {
                rETURNTYPE = RETURNTYPE.DATA;
            }
            else if (returnType == typeof(ExecuteNonModel))
            {
                rETURNTYPE = RETURNTYPE.EXECUTE_NON_MODEL;
            }

            switch (rETURNTYPE)
            {
                case RETURNTYPE.DATA:
                    switch (paramsCount)
                    {
                        case 0:
                            Add<Func<DataTable>
                                , Dictionary<string, Func<DataTable>>>
                                (funcName, methodInfo, noParams_DT, delegateStorage.noParamsFuncDT);
                            break;
                        case 1:
                            Add<Func<string, DataTable>
                                , Dictionary<string, Func<string, DataTable>>>
                                (funcName, methodInfo, noParams_DT, delegateStorage.oneParamsFuncDT);
                            break;
                        case 2:
                            Add<Func<string, string, DataTable>
                                , Dictionary<string, Func<string, string, DataTable>>>
                                (funcName, methodInfo, noParams_DT, delegateStorage.twoParamsFuncDT);
                            break;

                        case 3:
                            Add<Func<string, string, string, DataTable>
                                , Dictionary<string, Func<string, string, string, DataTable>>>
                                (funcName, methodInfo, noParams_DT, delegateStorage.threeParamsFuncDT);
                            break;

                        case 4:
                            Add<Func<string, string, string, string, DataTable>
                               , Dictionary<string, Func<string, string, string, string, DataTable>>>
                               (funcName, methodInfo, noParams_DT, delegateStorage.fourParamsFuncDT);
                            break;

                        case 5:
                            Add<Func<string, string, string, string, string, DataTable>
                              , Dictionary<string, Func<string, string, string, string, string, DataTable>>>
                              (funcName, methodInfo, noParams_DT, delegateStorage.fiveParamsFuncDT);
                            break;

                        case 6:
                            Add<Func<string, string, string, string, string, string, DataTable>
                              , Dictionary<string, Func<string, string, string, string, string, string, DataTable>>>
                              (funcName, methodInfo, noParams_DT, delegateStorage.sixParamsFuncDT);
                            break;

                        case 7:
                            Add<Func<string, string, string, string, string, string, string, DataTable>
                              , Dictionary<string, Func<string, string, string, string, string, string, string, DataTable>>>
                              (funcName, methodInfo, noParams_DT, delegateStorage.sevenParamsFuncDT);
                            break;

                        case 8:
                            Add<Func<string, string, string, string, string, string, string, string, DataTable>
                              , Dictionary<string, Func<string, string, string, string, string, string, string, string, DataTable>>>
                              (funcName, methodInfo, noParams_DT, delegateStorage.eightParamsFuncDT);
                            break;

                        case 9:
                            Add<Func<string, string, string, string, string, string, string, string, string, DataTable>
                              , Dictionary<string, Func<string, string, string, string, string, string, string, string, string, DataTable>>>
                              (funcName, methodInfo, noParams_DT, delegateStorage.nineParamsFuncDT);
                            break;

                        case 10:
                            Add<Func<string, string, string, string, string, string, string, string, string, string, DataTable>
                              , Dictionary<string, Func<string, string, string, string, string, string, string, string, string, string, DataTable>>>
                              (funcName, methodInfo, noParams_DT, delegateStorage.tenParamsFuncDT);
                            break;
                    }
                    break;

                case RETURNTYPE.EXECUTE_NON_MODEL:
                    switch (paramsCount)
                    {
                        case 0:
                            Add<Func<ExecuteNonModel>
                                , Dictionary<string, Func<ExecuteNonModel>>>
                                (funcName, methodInfo, noParams_DT, delegateStorage.noParamsFuncExecuteNonModel);
                            break;
                        case 1:
                            Add<Func<string, ExecuteNonModel>
                                , Dictionary<string, Func<string, ExecuteNonModel>>>
                                (funcName, methodInfo, noParams_DT, delegateStorage.oneParamsFuncExecuteNonModel);
                            break;

                        case 2:
                            Add<Func<string, string, ExecuteNonModel>
                                , Dictionary<string, Func<string, string, ExecuteNonModel>>>
                                (funcName, methodInfo, noParams_DT, delegateStorage.twoParamsFuncExecuteNonModel);
                            break;

                        case 3:
                            Add<Func<string, string, string, ExecuteNonModel>
                               , Dictionary<string, Func<string, string, string, ExecuteNonModel>>>
                               (funcName, methodInfo, noParams_DT, delegateStorage.threeParamsFuncExecuteNonModel);
                            break;

                        case 4:
                            Add<Func<string, string, string, string, ExecuteNonModel>
                               , Dictionary<string, Func<string, string, string, string, ExecuteNonModel>>>
                               (funcName, methodInfo, noParams_DT, delegateStorage.fourParamsFuncExecuteNonModel);
                            break;

                        case 5:
                            Add<Func<string, string, string, string, string, ExecuteNonModel>
                               , Dictionary<string, Func<string, string, string, string, string, ExecuteNonModel>>>
                               (funcName, methodInfo, noParams_DT, delegateStorage.fiveParamsFuncExecuteNonModel);
                            break;

                        case 6:
                            Add<Func<string, string, string, string, string, string, ExecuteNonModel>
                               , Dictionary<string, Func<string, string, string, string, string, string, ExecuteNonModel>>>
                               (funcName, methodInfo, noParams_DT, delegateStorage.sixParamsFuncExecuteNonModel);
                            break;

                        case 7:
                            Add<Func<string, string, string, string, string, string, string, ExecuteNonModel>
                               , Dictionary<string, Func<string, string, string, string, string, string, string, ExecuteNonModel>>>
                               (funcName, methodInfo, noParams_DT, delegateStorage.sevenParamsFuncExecuteNonModel);
                            break;

                        case 8:
                            Add<Func<string, string, string, string, string, string, string, string, ExecuteNonModel>
                               , Dictionary<string, Func<string, string, string, string, string, string, string, string, ExecuteNonModel>>>
                               (funcName, methodInfo, noParams_DT, delegateStorage.eightParamsFuncExecuteNonModel);
                            break;

                        case 9:
                            Add<Func<string, string, string, string, string, string, string, string, string, ExecuteNonModel>
                               , Dictionary<string, Func<string, string, string, string, string, string, string, string, string, ExecuteNonModel>>>
                               (funcName, methodInfo, noParams_DT, delegateStorage.nineParamsFuncExecuteNonModel);
                            break;

                        case 10:
                            Add<Func<string, string, string, string, string, string, string, string, string, string, ExecuteNonModel>
                               , Dictionary<string, Func<string, string, string, string, string, string, string, string, string, string, ExecuteNonModel>>>
                               (funcName, methodInfo, noParams_DT, delegateStorage.tenParamsFuncExecuteNonModel);
                            break;
                    }
                    break;
            }
        }

        public ExecuteNonModel FuncExecuteNonModel(string name, params string[] pars)
        {
            switch (pars.Length)
            {
                case 0:
                    return delegateStorage.noParamsFuncExecuteNonModel[name]();

                case 1:
                    return delegateStorage.oneParamsFuncExecuteNonModel[name](pars[0]);

                case 2:
                    return delegateStorage.twoParamsFuncExecuteNonModel[name](pars[0], pars[1]);

                case 3:
                    return delegateStorage.threeParamsFuncExecuteNonModel[name](pars[0], pars[1], pars[2]);

                case 4:
                    return delegateStorage.fourParamsFuncExecuteNonModel[name](pars[0], pars[1], pars[2], pars[3]);

                case 5:
                    return delegateStorage.fiveParamsFuncExecuteNonModel[name](pars[0], pars[1], pars[2], pars[3], pars[4]);

                case 6:
                    return delegateStorage.sixParamsFuncExecuteNonModel[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5]);

                case 7:
                    return delegateStorage.sevenParamsFuncExecuteNonModel[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6]);

                case 8:
                    return delegateStorage.eightParamsFuncExecuteNonModel[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6], pars[7]);

                case 9:
                    return delegateStorage.nineParamsFuncExecuteNonModel[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6], pars[7], pars[8]);

                case 10:
                    return delegateStorage.tenParamsFuncExecuteNonModel[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6], pars[7], pars[8], pars[9]);
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
                    return delegateStorage.noParamsFuncDT[name]();

                case 1:
                    return delegateStorage.oneParamsFuncDT[name](pars[0]);

                case 2:
                    return delegateStorage.twoParamsFuncDT[name](pars[0], pars[1]);

                case 3:
                    return delegateStorage.threeParamsFuncDT[name](pars[0], pars[1], pars[2]);

                case 4:
                    return delegateStorage.fourParamsFuncDT[name](pars[0], pars[1], pars[2], pars[3]);

                case 5:
                    return delegateStorage.fiveParamsFuncDT[name](pars[0], pars[1], pars[2], pars[3], pars[4]);

                case 6:
                    return delegateStorage.sixParamsFuncDT[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5]);

                case 7:
                    return delegateStorage.sevenParamsFuncDT[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6]);

                case 8:
                    return delegateStorage.eightParamsFuncDT[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6], pars[7]);

                case 9:
                    return delegateStorage.nineParamsFuncDT[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6], pars[7], pars[8]);

                case 10:
                    return delegateStorage.tenParamsFuncDT[name](pars[0], pars[1], pars[2], pars[3], pars[4], pars[5], pars[6], pars[7], pars[8], pars[9]);
            }
            return null;
        }
    }
}
