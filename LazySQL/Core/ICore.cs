using LazySQL.Core.CoreFactory;
using LazySQL.Core.CoreSystem;
using System;
using System.Xml;

namespace LazySQL.Core
{
    public abstract class ICore
    {
        public SystemMediator SystemMediator
        {
            get
            {
                return SystemMediator.Instance;
            }
        }

        protected ICoreFactory coreFactory;        

        /// <summary>
        /// 脚本导出
        /// </summary>
        /// <param name="connectionText">连接字段</param>
        /// <param name="name">脚本名称</param>
        /// <param name="xmlNode">脚本相关XML节点</param>
        /// <param name="maxCondition">最大条件字段(越小构建速率越快)</param>
        /// <param name="coreFactory">从什么类型工厂中构建核心</param>
        /// <param name="path">导出地址</param>
        public void CoreOutPut(string connectionText, string name, XmlNode xmlNode, int maxCondition, string path)
        {
            try
            {
                coreFactory.OutPutCSharp(connectionText, name, xmlNode, maxCondition, path);
            }
            catch (Exception ex)
            {
                throw ex;
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
        public void CoreBuild(string connectionText, string name, XmlNode xmlNode, int maxCondition)
        {
            try
            {
                coreFactory.Production(connectionText, name, xmlNode, maxCondition);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
