using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LazySQL.Infrastructure
{
    public class ReflectionHelper
    {
        private static ReflectionHelper _instance;
        public static ReflectionHelper GetInstance()
        {
            if (_instance == null)
                _instance = new ReflectionHelper();

            return _instance;
        }
        private ReflectionHelper() { }

        #region Get

        /// <summary>
        /// 通过反射获取属性名称和类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Dictionary<string, Type> GetTypeFromModel<T>()
        {
            Dictionary<string, Type> keyValuePairs = new Dictionary<string, Type>();
            PropertyInfo[] pros = typeof(T).GetProperties();
            for (int i = 0; i < pros.Length; i++)
            {
                keyValuePairs.Add(pros[i].Name, pros[i].PropertyType);
            }
            return keyValuePairs;
        }

        /// <summary>
        /// 通过反射获取值
        /// </summary>
        /// <param name="model">要获取的值的对象</param>
        /// <returns>字典数组,key为属性名称,Values为值</returns>
        public Dictionary<string, string> GetValueFromModel(object model)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            PropertyInfo[] pros = model.GetType().GetProperties();
            for (int i = 0; i < pros.Length; i++)
            {
                var proValue = pros[i].GetValue(model);
                if (proValue != null)
                    dict.Add(pros[i].Name, proValue.ToString());
            }
            return dict;
        }

        /// <summary>
        /// 通过反射获取属性上特性的值 一般对象
        /// </summary>
        /// <typeparam name="Model">要操作的对象</typeparam>
        /// <typeparam name="Attribute">指定特性类</typeparam>
        /// <param name="proName">对象类的属性名称</param>
        /// <param name="atrName">特性类的属性名称</param>
        /// <returns>特性的值</returns>
        public string GetAtrValueFromProWhenNormal<Model, Attribute>(string proName, string atrName) where Attribute : class, new()
        {
            Type objType = typeof(Model);
            Attribute atr = objType.GetProperty(proName).GetCustomAttribute(typeof(Attribute), true) as Attribute;
            if (atr != null)
            {
                var atrValue = atr.GetType().GetProperty(atrName).GetValue(atr);
                if (atrValue != null)
                    return atrValue.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 通过反射获取属性上特性的值 枚举组
        /// </summary>
        /// <typeparam name="Model">要操作的对象</typeparam>
        /// <typeparam name="Attribute">指定特性类</typeparam>
        /// <param name="proName">对象类的属性名称</param>
        /// <param name="atrName">特性类的属性名称</param>
        /// <returns></returns>
        public List<string> GetAtrValueFromProWhenEnum<Model, Attribute>(string proName, string atrName) where Attribute : class, new()
        {
            Type objType = typeof(Model);
            List<string> listStr = new List<string>();
            Attribute atr = objType.GetProperty(proName).GetCustomAttribute(typeof(Attribute), true) as Attribute;
            if (atr != null)
            {
                var atrValue = atr.GetType().GetProperty(atrName).GetValue(atr) as Array;

                if (atrValue != null)
                {
                    foreach (var en in atrValue)
                    {
                        listStr.Add(en.ToString());
                    }
                    if (listStr.Count > 0)
                    {
                        return listStr;
                    }
                }
            }
            return listStr;
        }

        /// <summary>
        /// 通过反射获取类上特性的值 一般对象
        /// </summary>
        /// <typeparam name="Model">要操作的对象</typeparam>
        /// <typeparam name="Attribute">指定特性类</typeparam>
        /// <param name="atrName">特性类的属性名称</param>
        /// <returns></returns>
        public string GetAtrValueFromClassWhenNormal<Model, Attribute>(string atrName) where Attribute : class, new()
        {
            Attribute atr = typeof(Model).GetCustomAttribute(typeof(Attribute), true) as Attribute;
            if (atr != null)
            {
                var atrValue = atr.GetType().GetProperty(atrName).GetValue(atr);
                if (atrValue != null)
                    return atrValue.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 通过反射获取类上特性的值 枚举组
        /// </summary>
        /// <typeparam name="Model">要操作的对象</typeparam>
        /// <typeparam name="Attribute">指定特性类</typeparam>
        /// <param name="atrName">特性类的属性名称</param>
        /// <returns></returns>
        public List<string> GetAtrValueFromClassWhenEnum<Model, Attribute>(string atrName) where Attribute : class, new()
        {
            List<string> listStr = new List<string>();
            Attribute atr = typeof(Model).GetCustomAttribute(typeof(Attribute), true) as Attribute;
            if (atr != null)
            {
                var atrValue = atr.GetType().GetProperty(atrName).GetValue(atr) as Array;
                if (atrValue != null)
                {
                    foreach (var en in atrValue)
                    {
                        listStr.Add(en.ToString());
                    }
                    if (listStr.Count > 0)
                    {
                        return listStr;
                    }
                }
            }
            return listStr;
        }

        /// <summary>
        /// 扫描某一个命名空间(Full)下的类型
        /// </summary>
        /// <param name="spaceName">命名空间</param>
        /// <param name="version">版本</param>
        /// <param name="culture"></param>
        /// <param name="publicKeyToken"></param>
        /// <returns></returns>
        public List<Type> ScanTypeInANameSpace(string spaceName, string version, string culture = "neutral", string publicKeyToken = null)
        {
            return Assembly.Load($"{spaceName},Version={version},Culture={culture},PublicKeyToken={(publicKeyToken ?? "null")}").GetTypes().ToList();
        }

        /// <summary>
        /// 获取所有的类
        /// </summary>
        /// <param name="assemblyFullName">命名空间(Full)</param>
        /// <returns></returns>
        public Type[] GetAllClass(string assemblyFullName)
        {
            return Assembly.Load(assemblyFullName).GetTypes();
        }

        /// <summary>
        /// 通过反射获取嵌入资源
        /// </summary>
        /// <param name="path">内存文件地址</param>
        /// <returns></returns>
        public Stream GetManifestResourceStream(string path)
        {
            return Assembly.GetEntryAssembly().GetManifestResourceStream(path);
        }

        /// <summary>
        /// 通过反射获取嵌入资源
        /// </summary>
        /// <param name="path">内存文件地址</param>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public Stream GetManifestResourceStream(string path,Assembly assembly)
        {
            return assembly.GetManifestResourceStream(path);
        }

        #endregion

        #region invoke

        /// <summary>
        /// 反射执行方法
        /// </summary>
        /// <param name="instantiation"></param>
        /// <param name="method"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Invoke(object instantiation, object method,params object[] models)
        {
            MethodInfo methodInfo = method as MethodInfo;
            if (method == null)
                return "方法转换失败";
            try
            {
                return methodInfo.Invoke(instantiation, models).ToString();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Convert

        /// <summary>
        /// 通过反射将数据模型数组转换为DataTable
        /// </summary>
        /// <typeparam name="Model">数据模型类型</typeparam>
        /// <param name="listM">数据模型数组</param>
        /// <param name="tableName">数据表名</param>
        /// <param name="exceptCol">需要除外的列</param>
        /// <returns></returns>
        public DataTable ModelConvertedToDataTable<Model>(List<Model> listM, string tableName, params string[] exceptCol)
        {
            DataTable tempDt = new DataTable(tableName);
            List<PropertyInfo> pros = typeof(Model).GetProperties().ToList();
            List<PropertyInfo> tempPros = new List<PropertyInfo>();
            foreach (var exceptStr in exceptCol)
            {
                List<PropertyInfo> iEnumerablePro = pros.Where(pro => pro.Name.Contains(exceptStr)).ToList();
                foreach (var pro in iEnumerablePro)
                {
                    pros.Remove(pro);
                }
            }
            foreach (var pro in pros)
            {
                tempDt.Columns.Add(new DataColumn(pro.Name));
            }
            foreach (var m in listM)
            {
                DataRow row = tempDt.NewRow();
                foreach (var pro in pros)
                {
                    var proValue = pro.GetValue(m);
                    if (proValue != null)
                    {
                        row[pro.Name] = proValue;
                    }
                }
                tempDt.Rows.Add(row);
            }
            return tempDt;
        }

        /// <summary>
        /// 获取 MethodInfo 并将其转换为 (object)hashtable 并返回
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="fullName">类完成名称</param>
        /// <returns></returns>
        public Hashtable MethodInfosConvertToObject(Assembly assembly, string fullName)
        {            
            MethodInfo[] methodInfos = assembly.CreateInstance(fullName).GetType().GetMethods();

            Hashtable hashMod = new Hashtable();
            foreach (var methodInfo in methodInfos)
            {
                hashMod.Add(methodInfo.Name, methodInfo);
            }

            return new Hashtable
            {
                { assembly.CreateInstance(fullName), hashMod }
            };
        }

        /// <summary>
        /// 转换为委托
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public Delegate CreateDelegateFromMethodInfo(Type type, object instance, object method)
        {
            return Delegate.CreateDelegate(type, instance, method as MethodInfo);
        }

        #endregion
    }
}
