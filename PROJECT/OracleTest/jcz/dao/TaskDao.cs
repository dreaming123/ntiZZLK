using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using entity;
using OrmTest;
namespace OracleTest.jcz
{
    class TaskDao
    {
        public SqlSugarClient db { get; set; }

        public TaskDao()
        {
             this.db = new SqlSugarClient(
       new ConnectionConfig()
       {
           ConnectionString = "Data Source = 10.1.11.73:1521 / orcl; User ID = huamei; Password=ntidba",
           DbType = DbType.Oracle,//设置数据库类型
                IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
            });
        }

       public void exc()
        {
           var getAll = db.Queryable<WCS_TASKINFO>().ToList();
        }




        private static SqlSugarClient GetInstance()
        {
            return new SqlSugarClient(new ConnectionConfig()
            {
                DbType = SqlSugar.DbType.Oracle,
                ConnectionString = Config.ConnectionString,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                AopEvents = new AopEvents
                {
                    OnLogExecuting = (sql, p) =>
                    {
                        Console.WriteLine(sql);
                        Console.WriteLine(string.Join(",", p?.Select(it => it.ParameterName + ":" + it.Value)));
                    }
                }
            });
        }

     
           


    }
}
