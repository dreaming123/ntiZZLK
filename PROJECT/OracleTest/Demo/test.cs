using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  OracleTest.jcz.dao;
namespace OracleTest.Demo
{
    public class test
    {
                SqlSugarClient db = new SqlSugarClient(
            new ConnectionConfig()
            {
                ConnectionString = "Data Source = 10.1.11.73:1521 / orcl; User ID = rwms; Password=ntidba",
                DbType = DbType.Oracle,//设置数据库类型
                IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
            });


        public List<StudentModel>  fistTest()
        {
            var db = OracleDB.GetInstance();
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" +
                db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };

            //var list = db.Queryable<StudentModel>()
            //    .Mapper(it => it.Persons, it => it.orgId) //支持多个自动映射一起用
            //                                              Orm就知道从 myorder里面的orgId去关联 persons里面的orgid
            //    .ToList();

            List<StudentModel> a = db.Queryable<StudentModel>().ToList();
            return a;
            //  Console.WriteLine(a[0].Name);
        }
        //创建实体
        public  void getTableEntity()
        {
         // db.DbFirst.CreateClassFile("F:\\华美冷库\\张晗\\ZZLK_WCS1117\\PROJECT\\OracleTest\\jcz\\entity", "entity");
           db.DbFirst.Where("WCS_taskinfo").CreateClassFile("F:\\华美冷库\\张晗\\ZZLK_WCS1117\\PROJECT\\OracleTest\\jcz\\entity", "entity");
        }


       
            

    }



    //如果实体类名称和表名不一致可以加上SugarTable特性指定表名
    [SugarTable("ITEM")]
    public class StudentModel
    {
        //指定主键和自增列，当然数据库中也要设置主键和自增列才会有效
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public string code { get; set; }
        public string Name { get; set; }
    }

    


    

}
