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
            BOOL
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

        #region BL委托

        public readonly Dictionary<string, Func<bool>> noParamsFuncBL;
        public readonly Dictionary<string, Func<string, bool>> oneParamsFuncBL;
        public readonly Dictionary<string, Func<string, string, bool>> twoParamsFuncBL;
        public readonly Dictionary<string, Func<string, string, string, bool>> threeParamsFuncBL;
        public readonly Dictionary<string, Func<string, string, string, string, bool>> fourParamsFuncBL;
        public readonly Dictionary<string, Func<string, string, string, string, string, bool>> fiveParamsFuncBL;
        public readonly Dictionary<string, Func<string, string, string, string, string, string, bool>> sixParamsFuncBL;
        public readonly Dictionary<string, Func<string, string, string, string, string, string, string, bool>> sevenParamsFuncBL;
        public readonly Dictionary<string, Func<string, string, string, string, string, string, string, string, bool>> eightParamsFuncBL;
        public readonly Dictionary<string, Func<string, string, string, string, string, string, string, string, string, bool>> nineParamsFuncBL;
        public readonly Dictionary<string, Func<string, string, string, string, string, string, string, string, string, string, bool>> tenParamsFuncBL;

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

            noParamsFuncBL = new Dictionary<string, Func<bool>>();
            oneParamsFuncBL = new Dictionary<string, Func<string, bool>>();
            twoParamsFuncBL = new Dictionary<string, Func<string, string, bool>>();
            threeParamsFuncBL = new Dictionary<string, Func<string, string, string, bool>>();
            fourParamsFuncBL = new Dictionary<string, Func<string, string, string, string, bool>>();
            fiveParamsFuncBL = new Dictionary<string, Func<string, string, string, string, string, bool>>();
            sixParamsFuncBL = new Dictionary<string, Func<string, string, string, string, string, string, bool>>();
            sevenParamsFuncBL = new Dictionary<string, Func<string, string, string, string, string, string, string, bool>>();
            eightParamsFuncBL = new Dictionary<string, Func<string, string, string, string, string, string, string, string, bool>>();
            nineParamsFuncBL = new Dictionary<string, Func<string, string, string, string, string, string, string, string, string, bool>>();
            tenParamsFuncBL = new Dictionary<string, Func<string, string, string, string, string, string, string, string, string, string, bool>>();

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

        private void NoParamsFuncDTAddBL(string funcName, MethodInfo methodInfo)
        {
            lock (noParams_BL)
            {
                if (!noParamsFuncBL.ContainsKey(funcName))
                {
                    noParamsFuncBL.Add(funcName, (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), methodInfo));
                }
            }
        }

        private void OneParamsFuncDTAddBL(string funcName, MethodInfo methodInfo)
        {
            lock (oneParams_BL)
            {
                if (!oneParamsFuncBL.ContainsKey(funcName))
                {
                    oneParamsFuncBL.Add(funcName, (Func<string, bool>)Delegate.CreateDelegate(typeof(Func<string, bool>), methodInfo));
                }
            }
        }

        private void TwoParamsFuncDTAddBL(string funcName, MethodInfo methodInfo)
        {
            lock (twoParams_BL)
            {
                if (!twoParamsFuncBL.ContainsKey(funcName))
                {
                    twoParamsFuncBL.Add(funcName, (Func<string, string, bool>)Delegate.CreateDelegate(typeof(Func<string, string, bool>), methodInfo));
                }
            }
        }

        private void ThreeParamsFuncDTAddBL(string funcName, MethodInfo methodInfo)
        {
            lock (threeParams_BL)
            {
                if (!threeParamsFuncBL.ContainsKey(funcName))
                {
                    threeParamsFuncBL.Add(funcName, (Func<string, string, string, bool>)Delegate.CreateDelegate(typeof(Func<string, string, string, bool>), methodInfo));
                }
            }
        }

        private void FourParamsFuncDTAddBL(string funcName, MethodInfo methodInfo)
        {
            lock (fourParams_BL)
            {
                if (!fourParamsFuncBL.ContainsKey(funcName))
                {
                    fourParamsFuncBL.Add(funcName, (Func<string, string, string, string, bool>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, bool>), methodInfo));
                }
            }            
        }

        private void FiveParamsFuncDTAddBL(string funcName, MethodInfo methodInfo)
        {
            lock (fiveParams_BL)
            {
                if (!fiveParamsFuncBL.ContainsKey(funcName))
                {
                    fiveParamsFuncBL.Add(funcName, (Func<string, string, string, string, string, bool>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, bool>), methodInfo));
                }
            }
        }

        private void SixParamsFuncDTAddBL(string funcName, MethodInfo methodInfo)
        {
            lock(sixParams_BL)
            {
                if (!sixParamsFuncBL.ContainsKey(funcName))
                {
                    sixParamsFuncBL.Add(funcName, (Func<string, string, string, string, string, string, bool>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, string, bool>), methodInfo));
                }
            }
        }

        private void SevenParamsFuncDTAddBL(string funcName, MethodInfo methodInfo)
        {
            lock(sevenParams_BL)
            {
                if (!sevenParamsFuncBL.ContainsKey(funcName))
                {
                    sevenParamsFuncBL.Add(funcName, (Func<string, string, string, string, string, string, string, bool>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, string, string, bool>), methodInfo));
                }
            }
        }

        private void EightParamsFuncDTAddBL(string funcName, MethodInfo methodInfo)
        {
            lock(eightParams_BL)
            {
                if (!eightParamsFuncBL.ContainsKey(funcName))
                {
                    eightParamsFuncBL.Add(funcName, (Func<string, string, string, string, string, string, string, string, bool>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, string, string, string, bool>), methodInfo));
                }
            }
        }

        private void NineParamsFuncDTAddBL(string funcName, MethodInfo methodInfo)
        {
            lock(nineParams_BL)
            {
                if (!nineParamsFuncBL.ContainsKey(funcName))
                {
                    nineParamsFuncBL.Add(funcName, (Func<string, string, string, string, string, string, string, string, string, bool>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, string, string, string, string, bool>), methodInfo));
                }
            }
        }

        private void TenParamsFuncDTAddBL(string funcName, MethodInfo methodInfo)
        {
            lock(tenParams_BL)
            {
                if (!tenParamsFuncBL.ContainsKey(funcName))
                {
                    tenParamsFuncBL.Add(funcName, (Func<string, string, string, string, string, string, string, string, string, string, bool>)Delegate.CreateDelegate(typeof(Func<string, string, string, string, string, string, string, string, string, string, bool>), methodInfo));
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
            else if (returnType == typeof(bool))
            {
                rETURNTYPE = RETURNTYPE.BOOL;
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

                case RETURNTYPE.BOOL:
                    switch (paramsCount)
                    {
                        case 0:
                            NoParamsFuncDTAddBL(funcName, methodInfo);
                            break;
                        case 1:
                            OneParamsFuncDTAddBL(funcName, methodInfo);
                            break;

                        case 2:
                            TwoParamsFuncDTAddBL(funcName, methodInfo);
                            break;

                        case 3:
                            ThreeParamsFuncDTAddBL(funcName, methodInfo);
                            break;

                        case 4:
                            FourParamsFuncDTAddBL(funcName, methodInfo);
                            break;

                        case 5:
                            FiveParamsFuncDTAddBL(funcName, methodInfo);
                            break;

                        case 6:
                            SixParamsFuncDTAddBL(funcName, methodInfo);
                            break;

                        case 7:
                            SevenParamsFuncDTAddBL(funcName, methodInfo);
                            break;

                        case 8:
                            EightParamsFuncDTAddBL(funcName, methodInfo);
                            break;

                        case 9:
                            NineParamsFuncDTAddBL(funcName, methodInfo);
                            break;

                        case 10:
                            TenParamsFuncDTAddBL(funcName, methodInfo);
                            break;
                    }
                    break;
            }
        }
    }
}
