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

        private LazySqlFactory()
        {
            factoryConfig = new FactoryConfig();
        }

        /// <summary>
        /// 构建代码
        /// </summary>
        /// <param name="connName">连接库</param>
        /// <param name="name">存储名称</param>
        /// <param name="path">xml资源路径</param>
        public void Build(string connName, string name, string path)
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

                if (MSSQLXML != null)
                    CoreMain.GetInstance().CoreBuild(factoryConfig.SQLConnDict[connName], name, MSSQLXML, factoryConfig.MaxCondition[connName], Core.DBType.MSSQL);

                if (SQLLITE != null)
                    CoreMain.GetInstance().CoreBuild(factoryConfig.SQLConnDict[connName], name, SQLLITE, factoryConfig.MaxCondition[connName], Core.DBType.SQLLITE);
            }
            catch (Exception ex)
            {
                throw new Exception($"LazySqlFactory.Build({connName},{name},{path})错误，错误消息${ex.Message}");
            }
            finally
            {
                stream.Dispose();
            }
        }

        /// <summary>
        /// 导出代码
        /// </summary>
        /// <param name="connName">连接库</param>
        /// <param name="name">生成名称</param>
        /// <param name="path">xml资源路径</param>
        /// <param name="outPutPath">导出路径</param>
        public void OutPut(string connName, string name, string path, string outPutPath)
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

                if (MSSQLXML != null)
                    CoreMain.GetInstance().CoreOutPut(factoryConfig.SQLConnDict[connName], name, MSSQLXML, factoryConfig.MaxCondition[connName], Core.DBType.MSSQL, outPutPath);

                if (SQLLITE != null)
                    CoreMain.GetInstance().CoreOutPut(factoryConfig.SQLConnDict[connName], name, SQLLITE, factoryConfig.MaxCondition[connName], Core.DBType.SQLLITE, outPutPath);
            }
            catch (Exception ex)
            {
                throw new Exception($"LazySqlFactory.Build({connName},{name},{path})错误，错误消息${ex.Message}");
            }
            finally
            {
                stream.Dispose();
            }
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
