using LazySQL.Core;
using LazySQL.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace LazySQL.Action.Modules
{
    public class LazySqlFactory
    {
        private static LazySqlFactory _instance;
        /// <summary>
        /// 工厂单例
        /// </summary>
        /// <returns></returns>
        public static LazySqlFactory GetInstance()
        {
            if (_instance == null)
                _instance = new LazySqlFactory();

            return _instance;
        }

        #region 工厂配置项

        class FactoryConfig
        {
            public Assembly Assembly { get; private set; }
            public Dictionary<string, int> MaxCondition { get; set; }
            public Dictionary<string, string> SQLConnDict { get; set; }
            public FactoryConfig()
            {
                MaxCondition = new Dictionary<string, int>();
                SQLConnDict = new Dictionary<string, string>();
            }

            public void SetAssembly(Assembly assembly)
            {
                Assembly = assembly;
            }
        }
        FactoryConfig factoryConfig;

        #endregion

        /// <summary>
        /// 初始化工厂配置
        /// </summary>
        private LazySqlFactory()
        {
            factoryConfig = new FactoryConfig();
        }

        /// <summary>
        /// 母版方法
        /// </summary>
        /// <param name="connName">连接库</param>
        /// <param name="name">名称</param>
        /// <param name="path">xml路径</param>
        /// <param name="action">扩展方法</param>
        private void Method(string connName, string name, string path, Action<XmlNode, XmlNode, XmlNode, XmlNode> action)
        {
            Stream stream = ReflectionHelper.GetInstance().GetManifestResourceStream(path);
            if (factoryConfig.Assembly != null)
                stream = ReflectionHelper.GetInstance().GetManifestResourceStream(path, factoryConfig.Assembly);

            try
            {
                XmlDocument xmlDocument = XmlHelper.GetInstance().GetXml(stream);

                XmlNode MSSQLXML = XmlHelper.GetInstance().GetNode(xmlDocument, "MSSQL");
                XmlNode MYSQLXML = XmlHelper.GetInstance().GetNode(xmlDocument, "MYSQL");
                XmlNode ORACLEXML = XmlHelper.GetInstance().GetNode(xmlDocument, "ORACLESQL");
                XmlNode SQLLITE = XmlHelper.GetInstance().GetNode(xmlDocument, "SQLLITE");

                action(MSSQLXML, MYSQLXML, ORACLEXML, SQLLITE);
            }
            catch (Exception ex)
            {
                throw new Exception($"错误消息：{ex.Message},错误方法：{ex.TargetSite}");
            }
            finally
            {
                stream.Dispose();
            }
        }

        /// <summary>
        /// 构建代码
        /// </summary>
        /// <param name="connName">连接库</param>
        /// <param name="name">存储名称</param>
        /// <param name="xmlPath">xml资源路径</param>
        public void BuildMethod(string connName, string name, string xmlPath)
        {
            Method(connName, name, xmlPath, (ms, my, oracl, sqllite) =>
            {
                if (ms != null)
                    CoreMain.GetInstance().CoreBuild(factoryConfig.SQLConnDict[connName], name, ms, factoryConfig.MaxCondition[connName], Core.DBType.MSSQL);

                if (sqllite != null)
                    CoreMain.GetInstance().CoreBuild(factoryConfig.SQLConnDict[connName], name, sqllite, factoryConfig.MaxCondition[connName], Core.DBType.SQLLITE);
            });
        }

        /// <summary>
        /// 导出脚本
        /// </summary>
        /// <param name="connName">连接库</param>
        /// <param name="name">名称</param>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="outPutPath">导出地址</param>
        public void ExportScript(string connName, string name, string xmlPath, string outPutPath)
        {
            Method(connName, name, xmlPath, (ms, my, oracl, sqllite) =>
            {
                if (ms != null)
                    CoreMain.GetInstance().CoreOutPut(factoryConfig.SQLConnDict[connName], name, ms, factoryConfig.MaxCondition[connName], Core.DBType.MSSQL, outPutPath);

                if (sqllite != null)
                    CoreMain.GetInstance().CoreOutPut(factoryConfig.SQLConnDict[connName], name, sqllite, factoryConfig.MaxCondition[connName], Core.DBType.SQLLITE, outPutPath);
            });
        }


        /// <summary>
        /// 添加连接库
        /// </summary>
        /// <param name="name">连接库名称</param>
        /// <param name="connText">连接字符串</param>
        /// <param name="maxCondition">XML所能拥有的最大动态条件查询组</param>
        public void AddConnection(string name, string connText, int maxCondition)
        {
            if (factoryConfig.SQLConnDict.ContainsKey(name))
            {
                factoryConfig.SQLConnDict[name] = connText;
            }
            else
            {
                factoryConfig.SQLConnDict.Add(name, connText);
            }

            if (factoryConfig.MaxCondition.ContainsKey(name))
            {
                factoryConfig.MaxCondition[name] = maxCondition;
            }
            else
            {
                factoryConfig.MaxCondition.Add(name, maxCondition);
            }
        }

        /// <summary>
        /// 设置Data层程序集
        /// </summary>
        /// <param name="assembly">程序集</param>
        public void SetAssembly(Assembly assembly)
        {
            factoryConfig.SetAssembly(assembly);
        }
    }
}
