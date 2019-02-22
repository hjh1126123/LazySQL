using LazySQL.SQLite;
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
                SQLiteFactory sQLiteFactory = new SQLiteFactory();

                sQLiteFactory.SetAssembly(Assembly.GetExecutingAssembly());

                sQLiteFactory.AddConnection("t", @"Data Source=" + @"db\sqlliteTest.db;Initial Catalog=sqlliteTest;Integrated Security=True;Max Pool Size=10", 10, 100, 10);

                //sQLiteFactory.ExportScript("t", "userQuery", $"SimpleSqlLite.SimpleQuery.xml", "output");

                sQLiteFactory.BuildMethod("t", "userQuery", $"SimpleSqlLite.SimpleQuery.xml");
                sQLiteFactory.BuildMethod("t", "userInsert", $"SimpleSqlLite.SimpleInsert.xml");
                sQLiteFactory.BuildMethod("t", "userUpdate", $"SimpleSqlLite.SimpleUpdate.xml");


                ExecuteNonModel insertNonModel = sQLiteFactory.Method_ExecuteNonModel("userInsert", $"hjh{DateTime.Now.ToString("HHmmss")}", DateTime.Now.Ticks.ToString(), "1");                
                if (insertNonModel.Success)
                {
                    Console.WriteLine("插入数据成功");
                }
                else
                {
                    Console.WriteLine($"插入数据失败,失败原因,{insertNonModel.Message}");                    
                }

                ExecuteNonModel updateNonModel = sQLiteFactory.Method_ExecuteNonModel("userUpdate", "", DateTime.Now.Ticks.ToString(), "", "27", "a", "b", "c", "d");
                if (updateNonModel.Success)
                {
                    Console.WriteLine("修改成功");
                }

                DataTable dataTable = sQLiteFactory.Method_DataTable("userQuery", "", "", "");
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
