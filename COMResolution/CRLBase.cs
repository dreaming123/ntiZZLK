using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COMRW;
using System.Threading;

namespace COMResolution
{
    public abstract class CRLBase
    {
        public static List<CRLStationInfo> StationInfo = new List<CRLStationInfo>();
        public static List<CRLOPCItem> ItemInfo = new List<CRLOPCItem>();

        //public RWLOPCServerl mPLC;
        public static Dictionary<string, RWLOPCServerl> PLCList = new Dictionary<string, RWLOPCServerl>();//PLC列表
        public Dictionary<string, bool> PLCFlag = new Dictionary<string, bool>();//PLC状态

        public List<string> PLCReLink = new List<string>();

        public string _ip;

        public Thread ComThread;//通讯线程
    }
}
