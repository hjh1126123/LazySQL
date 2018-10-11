using LazySQL.Action;
using System;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace SimpleMsSql
{
    class Program
    {
        static void Main(string[] args)
        {
            ActionMain.Instance.GetFactory().SetAssembly(Assembly.GetExecutingAssembly());

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["test"];

            ActionMain.Instance.GetFactory().AddConnection("test", settings.ConnectionString, 10);

            //输出自动生成的脚本文件到目录Debug/output
            //ActionMain.Instance.GetFactory().ExportScript("t", "user", $"SimpleMsSql.SimpleQuery.xml", "output");

            ActionMain.Instance.GetFactory().BuildMethod("t", "user", $"SimpleMsSql.SimpleQuery.xml");

            DataTable dataTable = ActionMain.Instance.GetSystem().Method_DataTable("user");

            Console.WriteLine(dataTable.DTString());

            Console.Read();
        }
    }
}
