## LazySQL 2 ！！！！

> 在LazySQL的经验上进行重构，从通过对model进行标记生成代码到从XML上定制代码，让你在轻松构建数据层的同时，增强代码的可读性，因为标记会让model的构建更为复杂

#### 主要作用

> 它极大的简化了DAL层的代码容量（你在DAL层几乎只需要写SQL和几行注册lazySQL的代码，无需理会任何ADO操作，以及字符串拼接等这些烦人且重复性高的工作，这些，LazySQL都帮你自动完成了），LazySQL并不是简单的helper(如果是这样，它将不能让效率与便捷并存），我更愿意叫它底层工具库，因为它不是提供一些写好的万能方法，让你去调用，它的工作是帮你自动生成执行效率最高的数据访问代码，然后对其进行编译，并存在内存当中.

#### 版本现状
- [x] 无配置
- [x] 使用简单
- [x] 自制数据连接对象池(实际测试，比简单的在配置开启连接池要快上15%~25%)
- [x] 多池(数据连接对象池)支持
- [x] 使用参数化查询
- [x] 事务
- [x] 线程安全
- [x] SQLLite支持
- [x] MSSQL支持
- [ ] MYSQL支持[building...]

#### 不建议

> 不建议在IIS或者任何需要高度重启的项目中使用，因为他在第一次执行的时候，它将会执行如下操作:<br/>生成连接池→根据XML自动创建代码→编译代码→存入内存<br/>通常时间不会超过3秒，但依然是不舒服的体验

#### 注意点

- 在AddConnection的时候，将会生成对象池

- 在BuildMethod的时候，将会自动生成代码以及编译代码并存入内存

> 上述操作建议单次执行，作者通常将它们写在单例模式的构造方法中

#### 快速开始

#### 跃跃欲试的同学，建议下载工程项目，直接运行sqlliteTest,里面有较为完整的范例

#### 后续的文档更新将在个人主页上更新（当然个人主页也在Building）

##### 首先是c#代码

```c#

//在通常情况下，只需要引入这个命名空间

using LazySQL.Action;
using System.Data;

class Main(){

//定义当前程序集
ActionMain.Instance.GetFactory().SetAssembly(Assembly.GetExecutingAssembly());

//添加数据库连接字符串
ActionMain.Instance.GetFactory().AddConnection("t", @"Data Source=" + @"db\sqlliteTest.db;Initial Catalog=sqlliteTest;Integrated Security=True;Max Pool Size=10", 10, 100, 10, DB_TYPE.SQLLITE);

//生成代码，并定义调用名称userQuery
ActionMain.Instance.GetFactory().BuildMethod("t", "userQuery", $"SimpleSqlLite.SimpleQuery.xml");
ActionMain.Instance.GetFactory().BuildMethod("t", "userInsert", $"SimpleSqlLite.SimpleInsert.xml");
ActionMain.Instance.GetFactory().BuildMethod("t", "userUpdate", $"SimpleSqlLite.SimpleUpdate.xml");

//执行查询语句，参数（调用名称，查询参数1，查询参数2，查询参数3），返回DataTable
DataTable dataTable = ActionMain.Instance.GetSystem().Method_DataTable("userQuery", "", "", "");

//执行插入语句，参数（调用名称，插入值1，插入值2，插入值3），返回ExecuteNonModel（含，错误信息，插入成功与否，受影响数量）
ExecuteNonModel NonModel = ActionMain.Instance.GetSystem().Method_ExecuteNonModel("userInsert", $"hjh{DateTime.Now.ToString("yyyyMMddHHmmss")}", DateTime.Now.Ticks.ToString(), "1");

//执行更新语句，参数（调用名称，修改值1，修改值2，修改值3，条件值1），返回ExecuteNonModel（含，错误信息，插入成功与否，受影响数量）
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
    <![CDATA[select * from user where 1=1 {0?}]]>
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
                return sqlLiteT.ExecuteDataTable("t", StrbSQL, aList);
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

##### 现在说说LazySql最重点的xml参数吧，用以下俩个模板来说明

```xml

<SQLLITE query="select">
  <parameters>
    <Parameter name="user"></Parameter>
    <Parameter name="pwd"></Parameter>    
    <Parameter name="id"></Parameter>
  </parameters>
  <sql>
    <![CDATA[select * from user where 1=1 {0?}]]>
  </sql>
</SQLLITE>

```

> 因为在LazySQL做了内部处理，所以理论上所有的节点(sqllite,parameters...)包括特性(例如上面的query,name...)都可以不区分大小写(c#获取xml节点本身是严格区分大小写的，为了方便xml文档的编写做了这一处理，因为在这里面不会存在俩个节点名称相同的情况)，但是为了代码生成效率，还是建议全部小写，或者全部大写，因为这样没必要去得出字母的所有大小写的排列组合，然后遍历

- SQLLITE，这个将标识该XML服务的数据库对象是谁，目前支持的是(MSSQL和SQLLITE)，将会在后续扩展

- query，这个将标识该XML返回类型是什么，目前支持的是（select——返回DataTable和other——返回ExecuteNonModel），将会根据后续需求扩展

- parameters，所有的参数都将在parameters里面，目前支持最大参数数量为10，可轻松扩展增加

- Parameter，则定义各种参数，包括查询条件，插入值，更新值等，这些都与你定义的特性有关，后面会列出Parameter主要存在特性

- sql，显而易见，你的数据库操作语句都应该写在这一节点下，在xml中建议使用<![CDATA[]]>将SQL语句包裹起来，这可以避免将大于号，小于号等敏感字符进行转义，降低代码可读性，当然sql也有一定的注意事项（这很重要），这将在后面列出.

##### 在XML中Parameter相关特性

名称 | 必须存在? | 描述 | 写法
---|---|---|---
name | 是 | 该特性与你自动生成代码息息相关，当target没有定义的时候，它必须是数据库字段名 | name = "some_name"
symbol | 否 | 该特性决定该参数的比值符，当该特性没有定义的时候默认为'='符号，它除了 '>' , '<' , '=' 等标准比值符以外，也可以用 'in' , 'not in', 'like' 等特殊比值符，当使用特殊比值符的时候，你必须给它安装一个模板 | symbol = ">=" 或者 symbol = "in"
target | 否 | 该特性指向你的数据库字段，当你有 name 为 datetime_Start 和 datetime_end 俩个参数，但是它们实际上都只是想查询datetime这一字段时，那么可以用target = "datetime" | target = "some_field"
template | 当symbol为非常规比值符的时候必须存在 | 当代码自动生成的时候，会将模板内的'\*'，替换为target或者name的内容，'\*'是在里面必不可少的一部分 | template = "(\*)"
value | 否 | 该特性将指定这一个参数会在何处被填充，当不存在该特性值的时候，将默认为value = "0" | value = "1"

##### 在XML中Sql相关说明

> 你肯定很好奇这些{0},{0?},{0!}是用来干嘛的，虽然也觉得你们应该已经猜到了，因为我就是从string.format中获取的灵感。这些标记是xml中最核心的部分，它指示了参数应该在何处插入，这与上述参数的value特性息息相关，所有的value = "0"的参数都将在{0}处被填充

> 那么!,?又是什么呢？这是一个填充规则，用下面表格来描述

特殊符号 | 描述 | 写法 | 生成字符串参考
---|---|---|---
无符号 | 它将在每个字段填充后加上"," | {0} | value1,value2,value3...
? | 它将在每个字段填充前加上"And" | {0?} | and value1 = 1 and value2 = 1 and value3 = 1...
! | 它在字段填充的时候改为了适用于update set 相关的写法 | {0!} | value1 = 1, value2 = 2, value3 = 1, ...

> 上面的填充参考不是一定的，它将对应数据库不同进行调整，内部使用工厂模式来生成，扩展性还是不错的

###### 注意！！

> 看到上面生成的代码，你们应该能猜到，会有一个BUG存在，那就是直接 where {0?} ，这样是会报错的，因为它生成的语句将会是 where and value 1 = 1，这是一个需要注意的地方，所以必须给它提供一个占位符，正确写法应该是 where 1=1 {0?}，这样就不会有任何错误，同样的，update 和 insert，直接写也会报错，因为最后面会多了一个逗号，我都在最后面增加了一个系统时间的参数（这也是符合逻辑的，因为插入数据的时间，和更新数据的时间，应该是要被记录的）

##### 为什么我不在LazySQL内部处理这些多一个"And"，多一个"，"的问题

> 因为我思来想去，都只能在执行生成的代码的时候进行判断，并不能在生成与编译代码的时候进行判断。确实能够给每一个自动生成的代码块加上这一块判断的代码逻辑，但是这会导致这段代码效率降低，所以我放弃了这一个计划，直接规范sql写法会来的容易许多

##### 如何使用

> 下载工程项目，拥有俩个数据库的范例，sqllite的可以直接运行调试

##### 欢迎提出任何扩展性建议和反馈任何BUG，我都会很开心的

##### 如果你能star一下，我会更开心
