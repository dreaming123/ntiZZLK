using System;
using System.Linq;
using System.Text;

namespace entity
{
    ///<summary>
    ///
    ///</summary>
    public partial class WCS_JKINTERFACE
    {
           public WCS_JKINTERFACE(){


           }
           /// <summary>
           /// Desc:序号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? SEQUENCENO {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string SN {get;set;}

           /// <summary>
           /// Desc:命令代号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string COMMANDCODE {get;set;}

           /// <summary>
           /// Desc:站台号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string STATIONNUMBER {get;set;}

           /// <summary>
           /// Desc:牌号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string ITEMCODE {get;set;}

           /// <summary>
           /// Desc:批次号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string LOTNUMBER {get;set;}

           /// <summary>
           /// Desc:烟箱号1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string BOX1 {get;set;}

           /// <summary>
           /// Desc:烟箱号2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string BOX2 {get;set;}

           /// <summary>
           /// Desc:任务号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? TASKNO {get;set;}

           /// <summary>
           /// Desc:错误代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string ERRORNUMBER {get;set;}

           /// <summary>
           /// Desc:数量
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? QTY {get;set;}

           /// <summary>
           /// Desc:状态
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string STATUS {get;set;}

    }
}
