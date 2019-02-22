using LazySQL.MSSQL;
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
            try
            {
                MSSQLFactory mSSQLFactory = new MSSQLFactory();

                mSSQLFactory.SetAssembly(Assembly.GetExecutingAssembly());

                mSSQLFactory.AddConnection("t", @"...", 10, 100, 10);

                mSSQLFactory.BuildMethod("t", "userQuery", $"SimpleMsSql.SimpleQuery.xml");

                DataTable dataTable = mSSQLFactory.Method_DataTable("userQuery");
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
