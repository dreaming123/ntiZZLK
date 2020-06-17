using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bap.LibNodave
{
    /// <summary>
    /// ProfiBus speed定义. MPI串口通信的波特率, 并非PLC和PC的通信速度
    /// </summary>
    public enum ProfiBusSpeed : int
    {
        Speed9k = 0,
        Speed19k = 1,
        Speed187k = 2,
        Speed500k = 3,
        Speed1500k = 4,
        Speed45k = 5,
        Speed93k = 6
    }
}
