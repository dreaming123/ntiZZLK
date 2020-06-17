using System;
using System.Linq;
using System.Text;

namespace entity
{
    ///<summary>
    ///
    ///</summary>
    public partial class BATCHSPLIT
    {
           public BATCHSPLIT(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string WAREHOUSEID {get;set;}

           /// <summary>
           /// Desc:存放领料单号(BillID)或者配盘单号(BomID)
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string BILLID {get;set;}

           /// <summary>
           /// Desc:单据类型（领料单，配盘单）
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string TYPECODE {get;set;}

           /// <summary>
           /// Desc:物料id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string ITEMID {get;set;}

           /// <summary>
           /// Desc:暂存区中的Containerdetailid
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string CONTAINERDETAILID {get;set;}

           /// <summary>
           /// Desc:批次分解过后的数量
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? DEALQTY {get;set;}

           /// <summary>
           /// Desc:批次号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD1 {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD2 {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD3 {get;set;}

           /// <summary>
           /// Desc:单据的处理状态
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD4 {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? FIELD5 {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ALREADYQTY {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? HAVEUSEDQTQY {get;set;}

    }
}
