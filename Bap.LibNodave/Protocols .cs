using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bap.LibNodave
{
   /// <summary>
   /// 和PLC的通信协议
   /// </summary>
    public enum Protocols : int
    {
        /// <summary>
        /// MPI for S7 300/400 
        /// </summary>
        MPI = 0,

        /// <summary>
        /// MPI for S7 300/400【安德鲁电缆】
        /// </summary>
        MPI2 = 1,	

        /// <summary>
        /// MPI for S7 300/400 Step 7 Version experimental
        /// </summary>
        MPI3 = 2,

        /// <summary>
        /// MPI for S7 300/400 "Andrew's version" with STX 
        /// </summary>
        MPI4 = 3,	

        /// <summary>
        /// PPI for S7 200 
        /// </summary>
        PPI = 10,	

        /// <summary>
        /// S5 via programming interface
        /// </summary>
        AS511 = 20,	

        /// <summary>
        ///  use s7onlinx.dll for transport
        /// </summary>
        S7OnLine = 50,

        /// <summary>
        /// ISO over TCP
        /// </summary>
        ISOTCP = 122,	

        /// <summary>
        /// 使用CP243的工业以太网
        /// </summary>
        ISOTCP243 = 123,

        /// <summary>
        /// MPI with IBH NetLink MPI to ethernet gateway 
        /// </summary>
        MPI_IBH = 223,	

        /// <summary>
        /// PPI with IBH NetLink PPI to ethernet gateway
        /// </summary>
        PPI_IBH = 224,	

        /// <summary>
        /// 使用Libnodave定义的协议数据单元PDU（Protocol Data Unit）
        /// </summary>
        UserTransport = 255
    }
}
