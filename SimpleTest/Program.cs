using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Configuration;

using LazySQL.Infrastructure;

using System.Data.SqlClient;
using System.Data;

namespace SimpleTest
{
    class Program
    {
        const string commText = @"SELECT COUNT(*) AS Total , MONTH(T.[InDate]) CreateDate FROM( SELECT DISTINCT (UserCode) AS[UserCode], InDate FROM      E_OverseasExpress WHERE     InDate >= '2018-01-01' AND InDate < '2019-01-01' AND DelState = '0' GROUP BY  InDate, UserCode) T GROUP BY MONTH(T.[InDate]) ORDER BY MONTH(T.[InDate])";

        static void Main(string[] args)
        {
            string sqlConnText = ConfigurationManager.AppSettings["MSSQLConnection"];

            //Task.Run(() =>
            //{
            //    DateTime dateTime = DateTime.Now;

            //    DBPool dBPool = new DBPool(sqlConnText, 50, 200, DB.MSSQL);

            //    Console.WriteLine($"初始化对象池花费时间,花费时间{(DateTime.Now - dateTime).TotalMilliseconds}毫秒");

            //    int count = 100;


            //    dateTime = DateTime.Now;

            //    for (var i = 0; i < count; i++)
            //    {
            //        //DateTime dateTimeOneTime = DateTime.Now;
            //        SqlConnection sqlConnection = dBPool.GetConnection();
            //        SqlCommand cmd = new SqlCommand(commText, sqlConnection);
            //        SqlDataAdapter adapter = new SqlDataAdapter();
            //        DataTable dt = new DataTable();
            //        try
            //        {
            //            adapter.SelectCommand = cmd;
            //            adapter.Fill(dt);

            //            //Console.WriteLine(dt.DTString());
            //        }
            //        catch (Exception ex)
            //        {
            //            throw ex;
            //        }
            //        finally
            //        {
            //            dt.Dispose();
            //            adapter.Dispose();
            //            cmd.Dispose();
            //            dBPool.FreeConnection(sqlConnection);
            //        }
            //        //Console.WriteLine($"执行一次SQL,花费时间{(DateTime.Now - dateTimeOneTime).TotalMilliseconds}毫秒");

            //        //Thread.Sleep(1000);
            //    }

            //    Console.WriteLine($"使用对象池执行{count}次SQL,花费时间{(DateTime.Now - dateTime).TotalMilliseconds}毫秒");

            //    dateTime = DateTime.Now;

            //    for (var i = 0; i < count; i++)
            //    {
            //        //DateTime dateTimeOneTime = DateTime.Now;
            //        SqlConnection sqlConnection = dBPool.GetConnection();
            //        SqlCommand cmd = new SqlCommand(commText, sqlConnection);
            //        SqlDataAdapter adapter = new SqlDataAdapter();
            //        DataTable dt = new DataTable();
            //        try
            //        {
            //            adapter.SelectCommand = cmd;
            //            adapter.Fill(dt);

            //            //Console.WriteLine(dt.DTString());
            //        }
            //        catch (Exception ex)
            //        {
            //            throw ex;
            //        }
            //        finally
            //        {
            //            dt.Dispose();
            //            adapter.Dispose();
            //            cmd.Dispose();
            //            dBPool.FreeConnection(sqlConnection);
            //        }
            //        //Console.WriteLine($"执行一次SQL,花费时间{(DateTime.Now - dateTimeOneTime).TotalMilliseconds}毫秒");

            //        //Thread.Sleep(1000);
            //    }

            //    Console.WriteLine($"第二次使用对象池执行{count}次SQL,花费时间{(DateTime.Now - dateTime).TotalMilliseconds}毫秒");

            //    //dateTime = DateTime.Now;

            //    //for (var i = 0; i < count; i++)
            //    //{
            //    //    //DateTime dateTimeOneTime = DateTime.Now;
            //    //    SqlConnection sqlConnection = new SqlConnection(sqlConnText);
            //    //    SqlCommand cmd = new SqlCommand(commText, sqlConnection);
            //    //    SqlDataAdapter adapter = new SqlDataAdapter();
            //    //    DataTable dt = new DataTable();
            //    //    try
            //    //    {
            //    //        sqlConnection.Open();
            //    //        adapter.SelectCommand = cmd;
            //    //        adapter.Fill(dt);

            //    //        //Console.WriteLine(dt.DTString());
            //    //    }
            //    //    catch (Exception ex)
            //    //    {
            //    //        throw ex;
            //    //    }
            //    //    finally
            //    //    {
            //    //        dt.Dispose();
            //    //        adapter.Dispose();
            //    //        cmd.Dispose();
            //    //        sqlConnection.Dispose();
            //    //    }
            //    //    //Console.WriteLine($"执行一次SQL,花费时间{(DateTime.Now - dateTimeOneTime).TotalMilliseconds}毫秒");

            //    //    //Thread.Sleep(1000);
            //    //}

            //    //Console.WriteLine($"不使用对象池执行{count}次SQL,花费时间{(DateTime.Now - dateTime).TotalMilliseconds}毫秒");

            //    //dateTime = DateTime.Now;

            //    //for (var i = 0; i < count; i++)
            //    //{
            //    //    //DateTime dateTimeOneTime = DateTime.Now;
            //    //    SqlConnection sqlConnection = new SqlConnection(sqlConnText);
            //    //    SqlCommand cmd = new SqlCommand(commText, sqlConnection);
            //    //    SqlDataAdapter adapter = new SqlDataAdapter();
            //    //    DataTable dt = new DataTable();
            //    //    try
            //    //    {
            //    //        sqlConnection.Open();
            //    //        adapter.SelectCommand = cmd;
            //    //        adapter.Fill(dt);

            //    //        //Console.WriteLine(dt.DTString());
            //    //    }
            //    //    catch (Exception ex)
            //    //    {
            //    //        throw ex;
            //    //    }
            //    //    finally
            //    //    {
            //    //        dt.Dispose();
            //    //        adapter.Dispose();
            //    //        cmd.Dispose();
            //    //        sqlConnection.Dispose();
            //    //    }
            //    //    //Console.WriteLine($"执行一次SQL,花费时间{(DateTime.Now - dateTimeOneTime).TotalMilliseconds}毫秒");

            //    //    //Thread.Sleep(1000);
            //    //}

            //    //Console.WriteLine($"第二次不使用对象池执行{count}次SQL,花费时间{(DateTime.Now - dateTime).TotalMilliseconds}毫秒");
            //});

            Console.ReadLine();
        }
    }
}
