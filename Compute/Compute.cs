using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Compute
{
    [ServiceContract]

    public interface ICompute
    {
        [OperationContract]
        string WCS_I_CreateOutBoudTaskCP_P(string wmstaskno, string sourcetext, string robotlineid, string barcode);//创建出库任务

        [OperationContract]
        void WCS_I_BufferToPick_P(string robotlineID, string barcode);//缓存位至拣选位

        [OperationContract]
        void WCS_I_DisToPick_P(string robotlineID, string barcode);//散盘位至拣选位

        [OperationContract]
        void ConnectCheck();//检测连接

        [OperationContract]
        int WCS_I_PickCount_P(String robotLineID, int count);//拣选指定道口指定数量的件烟

        [OperationContract]
        int WCS_I_ResidualBack_P(String robotLineID, string barcode);//余料回库

        [OperationContract]
        int WCS_I_PalletBack_P(String robotLineID, string barcode);//空托盘回库

        [OperationContract]
        void WCS_I_PalletToDis_P(String robotLineID, string barcode);//托盘回散盘位
    }
}
