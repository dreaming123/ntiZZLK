using System;
using System.Linq;
using System.Text;

namespace entity
{
    ///<summary>
    ///
    ///</summary>
    public partial class SHIPPERITEMRELATION
    {
           public SHIPPERITEMRELATION(){


           }
           /// <summary>
           /// Desc:主键
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string RELATIONID {get;set;}

           /// <summary>
           /// Desc:仓库id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string WAREHOUSEID {get;set;}

           /// <summary>
           /// Desc:供应商id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string SHIPPERID {get;set;}

           /// <summary>
           /// Desc:物料id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string ITEMID {get;set;}

           /// <summary>
           /// Desc:用户
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string USERID {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string CREATETIME {get;set;}

           /// <summary>
           /// Desc:保留字段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD1 {get;set;}

           /// <summary>
           /// Desc:保留字段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD2 {get;set;}

           /// <summary>
           /// Desc:保留字段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD3 {get;set;}

           /// <summary>
           /// Desc:保留字段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD4 {get;set;}

           /// <summary>
           /// Desc:保留字段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD5 {get;set;}

           /// <summary>
           /// Desc:保留字段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD6 {get;set;}

           /// <summary>
           /// Desc:保留字段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD7 {get;set;}

    }
}
