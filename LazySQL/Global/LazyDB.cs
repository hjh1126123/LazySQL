using System;
using LazySQL.MSSQL;
namespace LazySQL.DB
{
	public class LazyDB
	{
		public void Init()
		{
			MSSQLFactory mSSQLFactory = new MSSQLFactory();
			mSSQLFactory.AddConnection("myDB","Data Source=120.24.80.151;Initial Catalog=hdb21;Persist Security Info=True;User ID=sahdb;Password=www.baidu.com;MultipleActiveResultSets=true",10,100,10);
			mSSQLFactory.BuildMethod();
			mSSQLFactory.Method_DataTable();
					}
	}
}
