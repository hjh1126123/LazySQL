using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace LazySQL.Core.CoreSystem
{
    /// <summary>
    /// 仓库存储系统
    /// </summary>
    public class StorageSystem : ISystem
    {
        public enum RETURNTYPE
        {
            DATA,
            EXECUTE_NON_MODEL
        }

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
        public StorageSystem(SystemMediator systemMediator) : base(systemMediator)
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

        #region DT委托添加

        private void NoParamsFuncDTAddDT(string funcName, MethodInfo methodInfo)
        {
            lock(noParams_DT)
            {
                if (!noParamsFuncDT.ContainsKey(funcName))
                {
                    noParamsFuncDT.Add(funcName, (Func<DataTable>)Delegate.CreateDelegate(typeof(Func<DataTable>), methodInfo));
                }
            }
        }

        private void OneParamsFuncDTAddDT(string funcName, MethodInfo methodInfo)
        {
            lock(oneParams_DT)
            {
                if (!oneParamsFuncDT.ContainsKey(funcName))
                {
                    oneParamsFuncDT.Add(funcName, (Func<string, DataTable>)Delegate.CreateDelegate(typeof(Func<string, DataTable>), methodInfo));
                }
            }
        }

        private void TwoParamsFuncDTAddDT(string funcName, MethodInfo methodInfo)
        {
            lock(twoParams_DT)
            {
                if (!twoParamsFuncDT.ContainsKey(funcName))
                {
                    twoParamsFuncDT.Add(funcName, (Func<string, string, DataTable>)Delegate.CreateDelegate(typeof(Func<string, string, DataTable>), methodInfo));
                }
            }
        }

        private void ThreeParamsFuncDTAddDT(string funcName, MethodInfo methodInfo)
        {
            lock(threeParams_DT)
            {
                if (!threeParamsFuncDT.ContainsKey(funcName))
                {
                    threeParamsFuncDT.Add(funcName, (Func<string, string, string, DataTable>)Delegate.CreateDelegate(typeof(Func<string, string, string, DataTable>), methodInfo));
                }
            }
        }

        private void FourParamsFuncDTAddDT(string funcName, MethodInfo methodInfo)
        {
            lock(fourParams_DT)
            {
                if (!fourParamsFuncDT.ContainsKey(funcName))
                {
                    fourParamsFuncDT.Add(funcName, (Func<string, string, string, string, DataTable>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, DataTable>), methodInfo));
                }
            }
        }

        private void FiveParamsFuncDTAddDT(string funcName, MethodInfo methodInfo)
        {
            lock(fiveParams_DT)
            {
                if (!fiveParamsFuncDT.ContainsKey(funcName))
                {
                    fiveParamsFuncDT.Add(funcName, (Func<string, string, string, string, string, DataTable>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, DataTable>), methodInfo));
                }
            }
        }

        private void SixParamsFuncDTAddDT(string funcName, MethodInfo methodInfo)
        {
            lock(sixParams_DT)
            {
                if (!sixParamsFuncDT.ContainsKey(funcName))
                {
                    sixParamsFuncDT.Add(funcName, (Func<string, string, string, string, string, string, DataTable>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, string, DataTable>), methodInfo));
                }
            }
        }

        private void SevenParamsFuncDTAddDT(string funcName, MethodInfo methodInfo)
        {
            lock(sevenParams_DT)
            {
                if (!sevenParamsFuncDT.ContainsKey(funcName))
                {
                    sevenParamsFuncDT.Add(funcName, (Func<string, string, string, string, string, string, string, DataTable>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, string, string, DataTable>), methodInfo));
                }
            }
        }

        private void EightParamsFuncDTAddDT(string funcName, MethodInfo methodInfo)
        {
            lock(eightParams_DT)
            {
                if (!eightParamsFuncDT.ContainsKey(funcName))
                {
                    eightParamsFuncDT.Add(funcName, (Func<string, string, string, string, string, string, string, string, DataTable>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, string, string, string, DataTable>), methodInfo));
                }
            }
        }

        private void NineParamsFuncDTAddDT(string funcName, MethodInfo methodInfo)
        {
            lock(nineParams_DT)
            {
                if (!nineParamsFuncDT.ContainsKey(funcName))
                {
                    nineParamsFuncDT.Add(funcName, (Func<string, string, string, string, string, string, string, string, string, DataTable>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, string, string, string, string, DataTable>), methodInfo));
                }
            }
        }

        private void TenParamsFuncDTAddDT(string funcName, MethodInfo methodInfo)
        {
            lock(tenParams_DT)
            {
                if (!tenParamsFuncDT.ContainsKey(funcName))
                {
                    tenParamsFuncDT.Add(funcName, (Func<string, string, string, string, string, string, string, string, string, string, DataTable>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, string, string, string, string, string, DataTable>), methodInfo));
                }
            }
        }

        #endregion

        #region BL委托添加

        private void NoParamsFuncDTAddExecuteNonModel(string funcName, MethodInfo methodInfo)
        {
            lock (noParams_BL)
            {
                if (!noParamsFuncExecuteNonModel.ContainsKey(funcName))
                {
                    noParamsFuncExecuteNonModel.Add(funcName, (Func<ExecuteNonModel>)Delegate.CreateDelegate(typeof(Func<ExecuteNonModel>), methodInfo));
                }
            }
        }

        private void OneParamsFuncDTAddExecuteNonModel(string funcName, MethodInfo methodInfo)
        {
            lock (oneParams_BL)
            {
                if (!oneParamsFuncExecuteNonModel.ContainsKey(funcName))
                {
                    oneParamsFuncExecuteNonModel.Add(funcName, (Func<string, ExecuteNonModel>)Delegate.CreateDelegate(typeof(Func<string, ExecuteNonModel>), methodInfo));
                }
            }
        }

        private void TwoParamsFuncDTAddExecuteNonModel(string funcName, MethodInfo methodInfo)
        {
            lock (twoParams_BL)
            {
                if (!twoParamsFuncExecuteNonModel.ContainsKey(funcName))
                {
                    twoParamsFuncExecuteNonModel.Add(funcName, (Func<string, string, ExecuteNonModel>)Delegate.CreateDelegate(typeof(Func<string, string, ExecuteNonModel>), methodInfo));
                }
            }
        }

        private void ThreeParamsFuncDTAddExecuteNonModel(string funcName, MethodInfo methodInfo)
        {
            lock (threeParams_BL)
            {
                if (!threeParamsFuncExecuteNonModel.ContainsKey(funcName))
                {
                    threeParamsFuncExecuteNonModel.Add(funcName, (Func<string, string, string, ExecuteNonModel>)Delegate.CreateDelegate(typeof(Func<string, string, string, ExecuteNonModel>), methodInfo));
                }
            }
        }

        private void FourParamsFuncDTAddExecuteNonModel(string funcName, MethodInfo methodInfo)
        {
            lock (fourParams_BL)
            {
                if (!fourParamsFuncExecuteNonModel.ContainsKey(funcName))
                {
                    fourParamsFuncExecuteNonModel.Add(funcName, (Func<string, string, string, string, ExecuteNonModel>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, ExecuteNonModel>), methodInfo));
                }
            }            
        }

        private void FiveParamsFuncDTAddExecuteNonModel(string funcName, MethodInfo methodInfo)
        {
            lock (fiveParams_BL)
            {
                if (!fiveParamsFuncExecuteNonModel.ContainsKey(funcName))
                {
                    fiveParamsFuncExecuteNonModel.Add(funcName, (Func<string, string, string, string, string, ExecuteNonModel>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, ExecuteNonModel>), methodInfo));
                }
            }
        }

        private void SixParamsFuncDTAddExecuteNonModel(string funcName, MethodInfo methodInfo)
        {
            lock(sixParams_BL)
            {
                if (!sixParamsFuncExecuteNonModel.ContainsKey(funcName))
                {
                    sixParamsFuncExecuteNonModel.Add(funcName, (Func<string, string, string, string, string, string, ExecuteNonModel>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, string, ExecuteNonModel>), methodInfo));
                }
            }
        }

        private void SevenParamsFuncDTAddExecuteNonModel(string funcName, MethodInfo methodInfo)
        {
            lock(sevenParams_BL)
            {
                if (!sevenParamsFuncExecuteNonModel.ContainsKey(funcName))
                {
                    sevenParamsFuncExecuteNonModel.Add(funcName, (Func<string, string, string, string, string, string, string, ExecuteNonModel>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, string, string, ExecuteNonModel>), methodInfo));
                }
            }
        }

        private void EightParamsFuncDTAddExecuteNonModel(string funcName, MethodInfo methodInfo)
        {
            lock(eightParams_BL)
            {
                if (!eightParamsFuncExecuteNonModel.ContainsKey(funcName))
                {
                    eightParamsFuncExecuteNonModel.Add(funcName, (Func<string, string, string, string, string, string, string, string, ExecuteNonModel>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, string, string, string, ExecuteNonModel>), methodInfo));
                }
            }
        }

        private void NineParamsFuncDTAddExecuteNonModel(string funcName, MethodInfo methodInfo)
        {
            lock(nineParams_BL)
            {
                if (!nineParamsFuncExecuteNonModel.ContainsKey(funcName))
                {
                    nineParamsFuncExecuteNonModel.Add(funcName, (Func<string, string, string, string, string, string, string, string, string, ExecuteNonModel>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, string, string, string, string, ExecuteNonModel>), methodInfo));
                }
            }
        }

        private void TenParamsFuncDTAddExecuteNonModel(string funcName, MethodInfo methodInfo)
        {
            lock(tenParams_BL)
            {
                if (!tenParamsFuncExecuteNonModel.ContainsKey(funcName))
                {
                    tenParamsFuncExecuteNonModel.Add(funcName, (Func<string, string, string, string, string, string, string, string, string, string, ExecuteNonModel>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, string, string, string, string, string, ExecuteNonModel>), methodInfo));
                }
            }
        }

        #endregion

        /// <summary>
        /// 存储方法
        /// </summary>
        /// <param name="funcName">方法名</param>
        /// <param name="methodInfo">方法</param>
        /// <param name="paramsCount">方法字段数量</param>
        /// <param name="rETURNTYPE">返回类型</param>
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
                            NoParamsFuncDTAddDT(funcName, methodInfo);
                            break;
                        case 1:
                            OneParamsFuncDTAddDT(funcName, methodInfo);
                            break;
                        case 2:
                            TwoParamsFuncDTAddDT(funcName, methodInfo);
                            break;

                        case 3:
                            ThreeParamsFuncDTAddDT(funcName, methodInfo);
                            break;

                        case 4:
                            FourParamsFuncDTAddDT(funcName, methodInfo);
                            break;

                        case 5:
                            FiveParamsFuncDTAddDT(funcName, methodInfo);
                            break;

                        case 6:
                            SixParamsFuncDTAddDT(funcName, methodInfo);
                            break;

                        case 7:
                            SevenParamsFuncDTAddDT(funcName, methodInfo);
                            break;

                        case 8:
                            EightParamsFuncDTAddDT(funcName, methodInfo);
                            break;

                        case 9:
                            NineParamsFuncDTAddDT(funcName, methodInfo);
                            break;

                        case 10:
                            TenParamsFuncDTAddDT(funcName, methodInfo);
                            break;
                    }
                    break;

                case RETURNTYPE.EXECUTE_NON_MODEL:
                    switch (paramsCount)
                    {
                        case 0:
                            NoParamsFuncDTAddExecuteNonModel(funcName, methodInfo);
                            break;
                        case 1:
                            OneParamsFuncDTAddExecuteNonModel(funcName, methodInfo);
                            break;

                        case 2:
                            TwoParamsFuncDTAddExecuteNonModel(funcName, methodInfo);
                            break;

                        case 3:
                            ThreeParamsFuncDTAddExecuteNonModel(funcName, methodInfo);
                            break;

                        case 4:
                            FourParamsFuncDTAddExecuteNonModel(funcName, methodInfo);
                            break;

                        case 5:
                            FiveParamsFuncDTAddExecuteNonModel(funcName, methodInfo);
                            break;

                        case 6:
                            SixParamsFuncDTAddExecuteNonModel(funcName, methodInfo);
                            break;

                        case 7:
                            SevenParamsFuncDTAddExecuteNonModel(funcName, methodInfo);
                            break;

                        case 8:
                            EightParamsFuncDTAddExecuteNonModel(funcName, methodInfo);
                            break;

                        case 9:
                            NineParamsFuncDTAddExecuteNonModel(funcName, methodInfo);
                            break;

                        case 10:
                            TenParamsFuncDTAddExecuteNonModel(funcName, methodInfo);
                            break;
                    }
                    break;
            }
        }
    }
}
