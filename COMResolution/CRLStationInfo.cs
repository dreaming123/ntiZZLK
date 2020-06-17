using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMResolution
{
    public class CRLStationInfo
    {
        public int stationno;//站台号
        public int stationtype;//站台类型
        public int LocationX;
        public int LocationY;
        public int Width;
        public int Height;
        public string config;
        public int value;
        public int length;
        public string key_ip;
        public int DBAddress;
        public bool cFlag;

        public int taskno;//任务号
        public int barcode1;//托盘条码1
        public int barcode2;//托盘条码2
        public int goodstype;//货物类型
        public int fromstation;//起始地址
        public int tostation;//目标地址
        public int checkinfo;//货型检测
        public int field1;//备用1
        //public int field2;//备用2 重量2
        //public int field3;//备用3 重量1
        public int weight1;//重量1
        public int weight2;//重量2
    }
    public class Global
    {
        //public static string IsSequence="N";//="N";
        //public static string IsSequence = System.Configuration.ConfigurationSettings.AppSettings["IsBCtoA"];
        public static string TxtBarcode="";
        public static bool outFirst = true;
        public static int sendflag = 0;
    }
}
