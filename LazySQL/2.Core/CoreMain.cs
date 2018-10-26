using LazySQL.Core.CoreFactory;
using LazySQL.Core.CoreSystem;
using LazySQL.Infrastructure;
using System;
using System.Xml;

namespace LazySQL.Core
{
    public class CoreMain
    {
        public static CoreMain _instance;
        /// <summary>
        /// 获取核心单例
        /// </summary>
        /// <returns></returns>
        public static CoreMain GetInstance()
        {
            if (_instance == null)
                _instance = new CoreMain();

            return _instance;
        }

        #region 各大核心工厂

        MSCoreFactory mSCoreFactory;
        SqlLiteFactory sqlLiteFactory;

        #endregion

        private CoreMain()
        {
            mSCoreFactory = new MSCoreFactory(SystemMediator.GetInstance());
            sqlLiteFactory = new SqlLiteFactory(SystemMediator.GetInstance());
        }

        /// <summary>
        /// 脚本导出
        /// </summary>
        /// <param name="connectionText">连接字段</param>
        /// <param name="name">脚本名称</param>
        /// <param name="xmlNode">脚本相关XML节点</param>
        /// <param name="maxCondition">最大条件字段(越小构建速率越快)</param>
        /// <param name="coreFactory">从什么类型工厂中构建核心</param>
        /// <param name="path">导出地址</param>
        public void CoreOutPut(string connectionText, string name, XmlNode xmlNode, int maxCondition, DB_TYPE coreFactory, string path)
        {
            try
            {
                switch (coreFactory)
                {
                    case DB_TYPE.MSSQL:
                        mSCoreFactory.OutPutCSharp(connectionText, name, xmlNode, maxCondition, path);
                        break;

                    case DB_TYPE.ODBC:
                        break;

                    case DB_TYPE.OLEDB:
                        break;

                    case DB_TYPE.SQLLITE:
                        sqlLiteFactory.OutPutCSharp(connectionText, name, xmlNode, maxCondition, path);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex.ThrowMineFormat(this, "CoreBuild", name);
            }
        }

        /// <summary>
        /// 脚本构建
        /// </summary>
        /// <param name="connectionText">连接字段</param>
        /// <param name="name">脚本名称</param>
        /// <param name="xmlNode">脚本相关XML节点</param>
        /// <param name="maxCondition">最大条件字段(越小构建速率越快)</param>
        /// <param name="coreFactory">从什么类型工厂中构建核心</param>
        public void CoreBuild(string connectionText, string name, XmlNode xmlNode, int maxCondition, DB_TYPE coreFactory)
        {
            try
            {
                switch (coreFactory)
                {
                    case DB_TYPE.MSSQL:
                        mSCoreFactory.Production(connectionText, name, xmlNode, maxCondition);
                        break;

                    case DB_TYPE.ODBC:
                        break;

                    case DB_TYPE.OLEDB:
                        break;

                    case DB_TYPE.SQLLITE:
                        sqlLiteFactory.Production(connectionText, name, xmlNode, maxCondition);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex.ThrowMineFormat(this, "CoreBuild", name);
            }
        }

        /// <summary>
        /// 获取核心系统介绍商
        /// </summary>
        /// <returns></returns>
        public SystemMediator CoreSystemMediatorGet()
        {
            return SystemMediator.GetInstance();
        }
    }
}
