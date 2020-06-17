using System;
using System.Linq;
using System.Text;

namespace OracleTest.Demo
{
    ///<summary>
    ///
    ///</summary>
    public partial class ITEM
    {
           public ITEM(){


           }
           /// <summary>
           /// Desc:？？ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string WAREHOUSEID {get;set;}

           /// <summary>
           /// Desc:物料ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string ITEMID {get;set;}

           /// <summary>
           /// Desc:物料？？
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string CODE {get;set;}

           /// <summary>
           /// Desc:物料外部代？
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string EXTERIORCODE {get;set;}

           /// <summary>
           /// Desc:物料名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string NAME {get;set;}

           /// <summary>
           /// Desc:物料？格
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string DESCRIPTION {get;set;}

           /// <summary>
           /// Desc:是否需要？？
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? NEEDCHECKING {get;set;}

           /// <summary>
           /// Desc:允？收？缺？率
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? RECEIVINGPERCENT {get;set;}

           /// <summary>
           /// Desc:允？？点缺？率
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? COUNTPERCENT {get;set;}

           /// <summary>
           /// Desc:？？？本
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string LABELSAMPLE {get;set;}

           /// <summary>
           /// Desc:价格
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string PRICEID {get;set;}

           /// <summary>
           /// Desc:？位
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string UNITID {get;set;}

           /// <summary>
           /// Desc:？主ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string SHIPPERID {get;set;}

           /// <summary>
           /// Desc:状？代？
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string STATECODE {get;set;}

           /// <summary>
           /// Desc:唯一？？
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string SAMECODE {get;set;}

           /// <summary>
           /// Desc:容器物料ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string CONTAINERITEMID {get;set;}

           /// <summary>
           /// Desc:小数位数
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? DIGIT {get;set;}

           /// <summary>
           /// Desc:？位？
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ROUND {get;set;}

           /// <summary>
           /// Desc:托？？量
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? EACHPALLETFILLQTY {get;set;}

           /// <summary>
           /// Desc:？？方式
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? PALLETMODE {get;set;}

           /// <summary>
           /// Desc:？？ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string PARENTID {get;set;}

           /// <summary>
           /// Desc:醇/固化？？
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? STAYTIME {get;set;}

           /// <summary>
           /// Desc:？地
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string ORIGIN {get;set;}

           /// <summary>
           /// Desc:等？
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string ITEMGRADE {get;set;}

           /// <summary>
           /// Desc:年？
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? GIVENYEAR {get;set;}

           /// <summary>
           /// Desc:？
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? LENGTH {get;set;}

           /// <summary>
           /// Desc:？
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? WIDTH {get;set;}

           /// <summary>
           /// Desc:高
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? HEIGHT {get;set;}

           /// <summary>
           /// Desc:重量
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? WEIGHT {get;set;}

           /// <summary>
           /// Desc:工程？号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string CADNO {get;set;}

           /// <summary>
           /// Desc:？型/属性代？
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string TYPECODE {get;set;}

           /// <summary>
           /// Desc:存？区域
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string ZONECODE {get;set;}

           /// <summary>
           /// Desc:用？ID 
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string USERID {get;set;}

           /// <summary>
           /// Desc:？建？？
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime CREATETIME {get;set;}

           /// <summary>
           /// Desc:牌号？？
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD1 {get;set;}

           /// <summary>
           /// Desc:？牌号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD2 {get;set;}

           /// <summary>
           /// Desc:？留字段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD3 {get;set;}

           /// <summary>
           /// Desc:？留字段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD4 {get;set;}

           /// <summary>
           /// Desc:？留字段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD5 {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? MAXSTAYTIME {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string ACT_STATUS {get;set;}

           /// <summary>
           /// Desc:特殊托？？量
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? EACHPALLETFILLQTY_SP {get;set;}

    }
}
