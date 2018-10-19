## LazySQL 2 ！！！！

### 建议丢弃上一版本LazySQL，它将不再维护和更新

> 在LazySQL的基础上进行重构，从通过对代码标记生成代码到从XML上定制代码，让你在轻松构建数据层的同时，增强代码的可读性，标记往往会让model的构建更为复杂

#### 主打

> 是否为每次拼接sql的时候感到烦恼，看到一长串的if-else是否觉得头疼，lazySQL将会解救你，因为你不需要再去判断是否存在这个值，是否要查询这个条件，是否要修改这个值，你只需要定义一个xml，其他的lazySql都帮你做了

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
<SQLLITE query="insert">
  <parameters>
    <Parameter NAME="user"></Parameter>
    <Parameter NAME="pwd"></Parameter>
    <Parameter NAME="power"></Parameter>
  </parameters>
  <sql>
    <![CDATA[insert into user (user,pwd,power,sysTime) values ({0}datetime(CURRENT_TIMESTAMP,'localtime'))]]>
  </sql>
</SQLLITE>

```

> SimpleQuery.xml

```xml

<?xml version="1.0" encoding="utf-8" ?>
<SQLLITE query="select">
  <parameters>
    <Parameter name="user"></Parameter>
    <Parameter name="pwd"></Parameter>    
    <Parameter name="id"></Parameter>
  </parameters>
  <sql>
    select * from user where 1=1 {0?}
  </sql>
</SQLLITE>

```

> SimpleUpdate.xml

```xml

<?xml version="1.0" encoding="utf-8" ?>
<SQLLITE query="update">
  <parameters>
    <Parameter name="user"></Parameter>
    <Parameter name="pwd"></Parameter>
    <Parameter name="power"></Parameter>
    <Parameter name="id" index="1"></Parameter>
  </parameters>
  <sql>
    <![CDATA[UPDATE user SET {0!}sysTime=datetime(CURRENT_TIMESTAMP,'localtime') where 1=1 {1?}]]>
  </sql>
</SQLLITE>

```

##### 那他们会自动生成如何的c#代码，举个例子

> 如SimpleQuery.xml

```c#

public class user
