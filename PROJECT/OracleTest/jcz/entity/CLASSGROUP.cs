using System;
using System.Linq;
using System.Text;

namespace entity
{
    ///<summary>
    ///
    ///</summary>
    public partial class CLASSGROUP
    {
           public CLASSGROUP(){


           }
           /// <summary>
           /// Desc:仓库ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string WAREHOUSEID {get;set;}

           /// <summary>
           /// Desc:主键ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string CLASSID {get;set;}

           /// <summary>
           /// Desc:自动生成的班次
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string SHIFTCODE {get;set;}

           /// <summary>
           /// Desc:车间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string WORKSHOPID {get;set;}

           /// <summary>
           /// Desc:班组
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string TEAMDATE {get;set;}

           /// <summary>
           /// Desc:机台编号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string MACHINENUMBER {get;set;}

           /// <summary>
           /// Desc:交接班状态
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string CLASSSTATE {get;set;}

           /// <summary>
           /// Desc:开始时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? BEGIN_TIME {get;set;}

           /// <summary>
           /// Desc:结束时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? END_TIME {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? CREATETIME {get;set;}

           /// <summary>
           /// Desc:备注
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD1 {get;set;}

           /// <summary>
           /// Desc:上一班的班次
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD2 {get;set;}

           /// <summary>
           /// Desc:下一班的班次
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD3 {get;set;}

           /// <summary>
           /// Desc:生产日期
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD4 {get;set;}

           /// <summary>
           /// Desc:上位系统的班次
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FIELD5 {get;set;}

    }
}
