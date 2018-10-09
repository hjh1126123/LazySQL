using LazySQL.Action;
using System;
using System.Data;
using System.Reflection;

namespace SimpleSqlLite
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ActionMain.Instance.GetFactory().SetAssembly(Assembly.GetExecutingAssembly());

                ActionMain.Instance.GetFactory().AddConnection("t", @"Data Source=" + @"db\sqlliteTest.db;Initial Catalog=sqlite;Integrated Security=True;Max Pool Size=10", 10);

                //输出自动生成的脚本文件到目录Debug/output
                //ActionMain.Instance.GetFactory().OutPut("t", "user", $"HJHGo.TestSQLLite.xml","output");

                ActionMain.Instance.GetFactory().Build("t", "user", $"SimpleSqlLite.TestSQLLite.xml");

                DataTable dataTable = ActionMain.Instance.GetSystem().Method_DataTable("user");

                Console.WriteLine(dataTable.DTString());

                Console.Read();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
