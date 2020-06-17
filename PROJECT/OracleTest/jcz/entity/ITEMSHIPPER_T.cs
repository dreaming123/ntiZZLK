using System;
using System.Linq;
using System.Text;

namespace entity
{
    ///<summary>
    ///
    ///</summary>
    public partial class ITEMSHIPPER_T
    {
           public ITEMSHIPPER_T(){


           }
           /// <summary>
           /// Desc:仓库id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string WAREHOUSEID {get;set;}

           /// <summary>
           /// Desc:主键
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string RELATIONID {get;set;}

           /// <summary>
           /// Desc:物料id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string ITEMID {get;set;}

           /// <summary>
           /// Desc:供应商id
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string SHIPPERID {get;set;}

           /// <summary>
           /// Desc:用户id
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string USERID {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? CREATETIME {get;set;}

           /// <summary>
           /// Desc:保留字段1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD1 {get;set;}

           /// <summary>
           /// Desc:保留字段2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD2 {get;set;}

           /// <summary>
           /// Desc:保留字段3
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD3 {get;set;}

           /// <summary>
           /// Desc:保留字段4
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD4 {get;set;}

           /// <summary>
           /// Desc:保留字段5
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD5 {get;set;}

           /// <summary>
           /// Desc:保留字段6
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD6 {get;set;}

           /// <summary>
           /// Desc:保留字段7
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD7 {get;set;}

    }
}
