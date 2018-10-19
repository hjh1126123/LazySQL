## LazySQL 2 ！！！！

> 在LazySQL的基础上进行重构，从通过对代码标记生成代码到从XML上定制代码，让你在轻松构建数据层的同时，增强代码的可读性，标记往往会让model的构建更为复杂

#### 特性(Feature)

- [x] 脚本导出(export script)
- [x] 多数据库支持(multiple db support)
- [x] 使用参数化查询(use parameterized queries)
- [x] 在第一次生成代码编译后，调用效率与直接调用Ado.net组件所写的方法不相上下(After first generated , it same invocation efficiency as that of calling the method written by the Ado.net)
- [x] SQLLite支持(sqllite support)
- [x] MSSQL支持(mssql support)
- [ ] MYSQL支持(mysql support)[正在制作  building...]

#### 原理

> 从xml上获取参数后，利用codedom自动生成代码，将其编译后将方法缓存入内存中，方便以后调用


#### 快速开始

##### 首先是c#代码

```c#

//在通常情况下，只需要引入这个命名空间
using LazySQL.Action;
using System.Data;

class Main(){

//定义当前程序集
ActionMain.Instance.GetFactory().SetAssembly(Assembly.GetExecutingAssembly());

//添加数据库连接字符串
ActionMain.Instance.GetFactory().AddConnection("t", @"Data Source=" + @"db\sqlliteTest.db;Initial Catalog=sqlliteTest;Integrated Security=True;Max Pool Size=10", 10);

//生成代码，并定义调用名称userQuery
ActionMain.Instance.GetFactory().BuildMethod("t", "userQuery", $"SimpleSqlLite.SimpleQuery.xml");
ActionMain.Instance.GetFactory().BuildMethod("t", "userInsert", $"SimpleSqlLite.SimpleInsert.xml");
ActionMain.Instance.GetFactory().BuildMethod("t", "userUpdate", $"SimpleSqlLite.SimpleUpdate.xml");

//执行查询语句，参数（调用名称，查询参数1，查询参数2，查询参数3），返回DataTable
DataTable dataTable = ActionMain.Instance.GetSystem().Method_DataTable("userQuery", "", "", "");

//执行插入语句，参数（调用名称，插入值1，插入值2，插入值3），返回ExecuteNonModel（内含，错误信息，插入成功与否，受影响数量）
ExecuteNonModel NonModel = ActionMain.Instance.GetSystem().Method_ExecuteNonModel("userInsert", $"hjh{DateTime.Now.ToString("yyyyMMddHHmmss")}", DateTime.Now.Ticks.ToString(), "1");

//执行更新语句，参数（调用名称，修改值1，修改值2，修改值3，条件值1），返回ExecuteNonModel（内含，错误信息，插入成功与否，受影响数量）
ExecuteNonModel NonModel = ActionMain.Instance.GetSystem().Method_ExecuteNonModel("userUpdate", "", DateTime.Now.Ticks.ToString(), "", "27");

}

```

##### 现在让我们来看看xml是如何定义的

> SimpleInsert.xml

```xml

<?xml version="1.0" encoding="utf-8" ?>
<SQLLITE Query="insert">
  <Parameters>
    <Parameter NAME="user"></Parameter>
    <Parameter NAME="pwd"></Parameter>
    <Parameter NAME="power"></Parameter>
  </Parameters>
  <SQL>
    insert into user (user,pwd,power,sysTime) values ({0}datetime(CURRENT_TIMESTAMP,'localtime'))
  </SQL>
</SQLLITE>

```

> SimpleQuery.xml

```xml

<?xml version="1.0" encoding="utf-8" ?>
<SQLLITE Query="select">
  <Parameters>
    <Parameter name="user"></Parameter>
    <Parameter name="pwd"></Parameter>    
    <Parameter name="id"></Parameter>
  </Parameters>
  <SQL>
    select * from user where 1=1 {0?}
  </SQL>
</SQLLITE>

```

> SimpleUpdate.xml

```xml

<?xml version="1.0" encoding="utf-8" ?>
<SQLLITE Query="update">
  <Parameters>
    <Parameter name="user"></Parameter>
    <Parameter name="pwd"></Parameter>
    <Parameter name="power"></Parameter>
    <Parameter name="id" index="1"></Parameter>
  </Parameters>
  <SQL>
    UPDATE user SET {0!}sysTime=datetime(CURRENT_TIMESTAMP,'localtime') where 1=1 {1?}
  </SQL>
</SQLLITE>

```

##### 那他们会自动生成如何的c#代码，举个例子

> 如SimpleQuery.xml

```c#

public class userQueryClass
    {
        
        public static System.Data.DataTable userQuery(string user, string pwd, string id)
        {
            System.Text.StringBuilder StrbSQL = new System.Text.StringBuilder();
            LazySQL.Infrastructure.SQLiteTemplate sqlLiteT = new LazySQL.Infrastructure.SQLiteTemplate();
            try
            {
                System.Collections.Generic.List<System.Data.SQLite.SQLiteParameter> aList = new System.Collections.Generic.List<System.Data.SQLite.SQLiteParameter>();
                StrbSQL.Append("select * from user where ");
                System.Text.StringBuilder par0 = new System.Text.StringBuilder();
                if (!string.IsNullOrWhiteSpace(user))
                {
                    par0.Append("user = @user");
                    System.Data.SQLite.SQLiteParameter userPar = new System.Data.SQLite.SQLiteParameter("@user",user);
                    aList.Add(userPar);
                    par0.Append(" AND ");
                }
                if (!string.IsNullOrWhiteSpace(pwd))
                {
                    par0.Append("pwd = @pwd");
                    System.Data.SQLite.SQLiteParameter pwdPar = new System.Data.SQLite.SQLiteParameter("@pwd",pwd);
                    aList.Add(pwdPar);
                    par0.Append(" AND ");
                }
                if (!string.IsNullOrWhiteSpace(id))
                {
                    par0.Append("id = @id");
                    System.Data.SQLite.SQLiteParameter idPar = new System.Data.SQLite.SQLiteParameter("@id",id);
                    aList.Add(idPar);
                }
                StrbSQL.Append(par0);
                
                //执行内置方法（为何这里是调用LazySQL内置方法呢，而不是自动生成完整的数据库操作代码呢？因为数据库操作在大多数情况下是有通用方法的，这可以让我更容易去扩展项目，也可以让所有使用我这个开源项目的人，更容易去扩展自己的LazySql，因为比起去扩展codedom操作来说，扩展Ado.net操作要轻松的多）
                return sqlLiteT.ExecuteDataTable("Data Source=db\\sqlliteTest.db;Initial Catalog=sqlliteTest;Integrated Security=Tru" +
                        "e;Max Pool Size=10", StrbSQL, aList);
            }
            catch (System.Exception ex)
            {
                throw ex;;
            }
        }
    }

```

> 任何生成的代码你都是看不见的，因为他已经被编译，并且存储到内存内了，除非你利用

```c#

//添加数据库连接字符串
ActionMain.Instance.GetFactory().AddConnection("t", @"Data Source=" + @"db\sqlliteTest.db;Initial Catalog=sqlliteTest;Integrated Security=True;Max Pool Size=10", 10);

//导出SimpleQuery.xml生成的代码，参数（数据库连接字符串名称）
ActionMain.Instance.GetFactory().ExportScript("t", "userQuery", $"SimpleSqlLite.SimpleQuery.xml", "output");

```

> 将其导出

> 所以遇到BUG了不要急，将其生成的代码导出，并反馈给我，下一个小版本它将会被修复
