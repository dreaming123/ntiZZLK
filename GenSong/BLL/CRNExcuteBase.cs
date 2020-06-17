using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using System.Threading;
using System.Collections;
using COMResolution;

namespace GenSong
{
    public class CRNExcuteBase
    {
        public static string CR_GETC_WAIT_H = "112";//准备取货
        public static string CR_GETC_POS_RUNNING = "113";//取货前的移动
        public static string CR_GETC_GET_RUNNING = "114";//开始取货
        public static string CR_GETC_EXECUTED = "121";//取货完成
        public static string CR_PUTC_WAIT_H = "128";//准备放货
        public static string CR_PUTC_POS_RUNNING = "129";//放货前的移动
        public static string CR_PUTC_PUT_RUNNING = "130";//开始放货
        public static string CR_PUTC_EXECUTED = "137";//放货完成
        public string plcNumber = System.Configuration.ConfigurationSettings.AppSettings["PLCNUMBER"].ToString();

        public static DALDATA dal = new DALDATA();
        public static List<CRNObject> CRNOBList = new List<CRNObject>();
        public static CRNTask crc = new CRNTask();
        public static Thread TaskThread;
        public static Hashtable AlarmHash = Hashtable.Synchronized(new Hashtable());//堆垛机详细报警信息存储
        public static List<CRNStatus> CRNList = new List<CRNStatus>();

        public CRLConveryControl ConveryClass = new CRLConveryControl();
    }
}
