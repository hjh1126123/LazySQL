using LazySQL.Action;
using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

namespace SimpleSqlLite
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ActionMain.Instance.GetFactory().SetAssembly(Assembly.GetExecutingAssembly());

                ActionMain.Instance.GetFactory().AddConnection("t", @"Data Source=" + @"db\sqlliteTest.db;Initial Catalog=sqlliteTest;Integrated Security=True;Max Pool Size=10", 10);

                //输出自动生成的脚本文件到目录Debug/output
                ActionMain.Instance.GetFactory().ExportScript("t", "userQuery", $"SimpleSqlLite.SimpleQuery.xml", "output");
                ActionMain.Instance.GetFactory().ExportScript("t", "userInsert", $"SimpleSqlLite.SimpleInsert.xml", "output");
                ActionMain.Instance.GetFactory().ExportScript("t", "userUpdate", $"SimpleSqlLite.SimpleUpdate.xml", "output");

                //userInsertClass.userInsert($"hjh{DateTime.Now.ToString()}", "123456", "1");

                //ActionMain.Instance.GetFactory().BuildMethod("t", "userQuery", $"SimpleSqlLite.SimpleQuery.xml");
                //ActionMain.Instance.GetFactory().BuildMethod("t", "userInsert", $"SimpleSqlLite.SimpleInsert.xml");



                //Task.Factory.StartNew(() =>
                //{
                //    bool success = ActionMain.Instance.GetSystem().Method_Bool("userInsert", $"hjh{DateTime.Now.ToString()}", "", "1");
                //    if (success)
                //    {
                //        Console.WriteLine("插入数据成功");
                //    }
                //});

                //Task.Factory.StartNew(() =>
                //{
                //    DataTable dataTable = ActionMain.Instance.GetSystem().Method_DataTable("userQuery");
                //    Console.WriteLine(dataTable.DTString());
                //});

                //Console.Read();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
