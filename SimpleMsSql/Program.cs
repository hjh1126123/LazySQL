using LazySQL.MSSQL;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

                mSSQLFactory.AddConnection("","",10,10,10);
                mSSQLFactory.BuildMethod();
                mSSQLFactory.Method_DataTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
