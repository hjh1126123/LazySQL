using LazySQL.Core;
using LazySQL.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml;

namespace LazySQL
{
    class FactoryConfig
    {
        public Assembly Assembly { get; private set; }
        public ConcurrentDictionary<string, int> MaxCondition { get; private set; }
        public ConcurrentDictionary<string, IPool> DBPools { get; private set; }
        public FactoryConfig()
        {
            MaxCondition = new ConcurrentDictionary<string, int>();
            DBPools = new ConcurrentDictionary<string, IPool>();
        }

        public void SetAssembly(Assembly assembly)
        {
            Assembly = assembly;
        }
    }

    public abstract class IFactory
    {
        #region 继承需配置项

        //继承要赋值
        protected ICore Core { get; set; }

        //添加连接池，继承请重写
        public virtual void AddConnection(string name, string connText, int initCount, int capacity, int maxCondition)
        {
            factoryConfig.MaxCondition.AddOrUpdate(name, maxCondition, (key, oldValue) => maxCondition);

            //IPool pool = new IPool(name, initCount, capacity);
            //DBPools.AddOrUpdate(name, pool, (key, oldValue) => pool);
        }

        #endregion

        protected ConcurrentDictionary<string, IPool> DBPools;

        FactoryConfig factoryConfig;

        /// <summary>
        /// 初始化工厂配置
        /// </summary>
        public IFactory()
        {
            factoryConfig = new FactoryConfig();
            DBPools = new ConcurrentDictionary<string, IPool>();
        }

        /// <summary>
        /// 母版方法
        /// </summary>
        /// <param name="connName">连接库</param>
        /// <param name="name">名称</param>
        /// <param name="path">xml路径</param>
        /// <param name="action">扩展方法</param>
        private void Method(string connName, string name, string path, Action<XmlNode> action)
        {
            Stream stream;
            if (factoryConfig.Assembly == null)
                stream = ReflectionHelper.GetInstance().GetManifestResourceStream(path);
            else
                stream = ReflectionHelper.GetInstance().GetManifestResourceStream(path, factoryConfig.Assembly);

            if (factoryConfig.Assembly != null)
                stream = ReflectionHelper.GetInstance().GetManifestResourceStream(path, factoryConfig.Assembly);

            try
            {
                XmlDocument xmlDocument = XmlHelper.GetInstance().GetXml(stream);
                XmlNode sqlNode = XmlHelper.GetInstance().GetNode(xmlDocument, "SQL");

                action(sqlNode);
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
        /// <param name="connName">数据池名称</param>
        /// <param name="MethodName">方法名称</param>
        /// <param name="xmlPath">xml内存文件地址</param>
        public void BuildMethod(string connName, string MethodName, string xmlPath)
        {
            if (Core == null)
                return;

            Method(connName, MethodName, xmlPath, (sql) =>
            {
                Core.CoreBuild(connName, MethodName, sql, factoryConfig.MaxCondition[connName]);
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
            if (Core == null)
                return;

            Method(connName, name, xmlPath, (sql) =>
            {
                Core.CoreOutPut(connName, name, sql, factoryConfig.MaxCondition[connName], outPutPath);
            });
        }

        /// <summary>
        /// 设置程序集
        /// </summary>
        /// <param name="assembly">程序集</param>
        public void SetAssembly(Assembly assembly)
        {
            if (assembly == null)
                return;

            factoryConfig.SetAssembly(assembly);
        }

        /// <summary>
        /// 执行方法，返回DataTable
        /// </summary>
        /// <param name="name">方法名</param>
        /// <param name="args">方法参数</param>
        /// <returns></returns>
        public DataTable Method_DataTable(string name, params string[] args)
        {
            if (Core == null)
                return null;

            return Core.SystemMediator.FuncDT(name, args);
        }

        /// <summary>
        /// 执行方法，返回Bool
        /// </summary>
        /// <param name="name">方法名</param>
        /// <param name="args">方法参数</param>
        /// <returns></returns>
        public ExecuteNonModel Method_ExecuteNonModel(string name, params string[] args)
        {
            if (Core == null)
                return null;

            return Core.SystemMediator.FuncExecuteNonModel(name, args);
        }
    }
}
