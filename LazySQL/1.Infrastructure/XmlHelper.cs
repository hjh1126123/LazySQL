using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace LazySQL.Infrastructure
{
    public class XmlHelper
    {
        private static XmlHelper _instance;
        public static XmlHelper GetInstance()
        {
            if (_instance == null)
                _instance = new XmlHelper();

            return _instance;
        }
        private XmlHelper() { }

        /// <summary>
        /// 从xml文件中创建XmlDoc处理类
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public XmlDocument GetXml(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            return xmlDocument;
        }

        /// <summary>
        /// 从流中获取XML
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public XmlDocument GetXml(Stream stream)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);

            return xmlDocument;
        }

        /// <summary>
        /// 从xml字符串中常见xmlDoc处理类
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public XmlDocument GetXmlInXmlStr(string xmlData)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlData);

            return xmlDocument;
        }

        /// <summary>
        /// 从xml中获取根节点
        /// </summary>
        /// <param name="xml">xml文档</param>
        /// <param name="nodeName">根对象名称</param>
        /// <returns></returns>
        public XmlNode GetNode(XmlDocument xml, string nodeName)
        {
            return xml.SelectSingleNode(nodeName);
        }

        /// <summary>
        /// 返回节点中第一个匹配xmlNodeName的节点
        /// </summary>
        /// <param name="xmlNode">父节点</param>
        /// <param name="xmlNodeName">名称</param>
        /// <returns></returns>
        public XmlNode GetNode(XmlNode xmlNode, string xmlNodeName)
        {
            XmlNodeList xmlNodeList = xmlNode.ChildNodes;
            for (var i = 0; i < xmlNodeList.Count; i++)
            {
                if (xmlNodeList[i].Name.Equals(xmlNodeName))
                {
                    return xmlNodeList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 返回节点中第index个匹配xmlNodeName的节点
        /// </summary>
        /// <param name="xmlNode">父节点</param>
        /// <param name="xmlNodeName">名称</param>
        /// <param name="index">序号</param>
        /// <returns></returns>
        public XmlNode GetNode(XmlNode xmlNode, string xmlNodeName, int index)
        {
            XmlNodeList xmlNodeList = xmlNode.ChildNodes;
            int count = 0;
            for (var i = 0; i < xmlNodeList.Count; i++)
            {
                if (xmlNodeList[i].Name.Equals(xmlNodeName))
                {
                    count++;
                    if (count == index)
                    {
                        return xmlNodeList[i];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取某xml节点中的所有子节点
        /// </summary>
        /// <param name="xmlNode">xml节点</param>
        /// <returns></returns>
        public XmlNodeList GetAllNode(XmlNode xmlNode)
        {
            return xmlNode.ChildNodes;
        }

        /// <summary>
        /// 获取某节点某特性的值(可不区分大小写，但是建议全小写或全大写，效率很快)
        /// </summary>
        /// <param name="xmlNode">节点</param>
        /// <param name="AttributeName">特性名称</param>
        /// <returns></returns>
        public string GetNodeAttribute(XmlNode xmlNode, string AttributeName)
        {
            var xmlnode = xmlNode as XmlElement;
            string Attr = xmlnode.GetAttribute(AttributeName);
            if (string.IsNullOrWhiteSpace(Attr))
            {
                Attr = xmlnode.GetAttribute(AttributeName.ToLower());
            }
            if (string.IsNullOrWhiteSpace(Attr))
            {
                Attr = xmlnode.GetAttribute(AttributeName.ToUpper());
            }
            if (string.IsNullOrWhiteSpace(Attr))
            {
                List<string> tmpList = AttributeName.StringAllCombination();
                foreach(var str in tmpList)
                {
                    Attr = xmlnode.GetAttribute(str);
                    if (!string.IsNullOrWhiteSpace(Attr))
                        break;
                }
            }
            return Attr;
        }

        /// <summary>
        /// 保存xml至文件
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="path"></param>
        public void SaveXml(XmlDocument doc, string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            try
            {
                doc.Save(fileStream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fileStream.Dispose();
            }
        }
    }
}
