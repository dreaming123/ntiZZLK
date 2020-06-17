using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LEDControl_UDP
{
    public class LED_UDP
    {
        private LED_UDP_Control LED_Control = new LED_UDP_Control();
        public ushort nDevID;
        public ushort UDPPort;
        public string LED_IP;

        private string Password;           //设备密码默认为空
        private int SecTimeOut;           // 超时上限,单位秒,默认为1
        private int RetryTimes;              // 重试次数，默认为2

        private int IsOpen;   //初始为0
        public int Index; //节目表编号,发送文件时对应的节目单
        const bool bSCL2008 = true;   //是否为SCL2008控制器，一般不需修改此值

        private object lock1 = new object();
        private object lock2 = new object();
        public struct ProgItemType
        {
            public int Flag;
            public int PicFInde;
            public Int64 Effect;
            public Int64 SpeedStay;
            public Int64 Schedule;
        }
        /// <summary>
        /// 实例化，必须指定IP地址，端口，设备编号，这些用来创建socket
        /// </summary>
        /// <param name="IP">IP地址</param>
        /// <param name="Port">端口</param>
        /// <param name="DevID">设备编号</param>
        public LED_UDP(string IP, ushort Port, ushort DevID)
        {
            LED_IP = IP;
            UDPPort = Port;
            nDevID = DevID;
            Password = "";   //设备密码默认为空
            SecTimeOut = 2;           // 超时上限,单位秒,默认为2
            RetryTimes = 2;              // 重试次数，默认为2
            Index = 0;           //默认是0，则C:\P00目录下的节目单
            IsOpen = 0;   //初始为false

        }
        public LED_UDP(string IP, ushort Port, ushort DevID, string Pwd, int secTimeOut, int retryTimes, int index)
        {
            LED_IP = IP;
            UDPPort = Port;
            nDevID = DevID;


            Password = Pwd;
            SecTimeOut = secTimeOut;
            RetryTimes = retryTimes;
            Index = index;
            IsOpen = 0;   //初始为false

        }

        public int open()
        {
            IsOpen = LED_Control.LED_NetInitial(nDevID, Password, LED_IP, SecTimeOut, RetryTimes, UDPPort);
            return IsOpen;
        }

        public bool close()
        {
            if (IsOpen == 1)   //已经打开
            {
                if (LED_Control.Close(nDevID)) return true;
                else
                    return false;
            }
            else   //未打开，return true
                return true;
        }
        /// <summary>
        /// 只是发送文件，发送到C盘根目录下
        /// </summary>
        /// <param name="str"></param>
        /// <param name="LocalTempFile"></param>
        /// <returns></returns>
        public bool sendText(string str)
        {
            bool resalut = false;
            LED_Control.LED_NetInitial(nDevID, Password, LED_IP, SecTimeOut, RetryTimes, UDPPort);

            //ProgItemType type = new ProgItemType();
            //type.Flag = 0;
            //type.PicFInde = 4;
            //type.Effect = 4;
            //type.SpeedStay = 0x7;
            //type.Schedule = 0xFFFFFFFF;
            //int size = Marshal.SizeOf(typeof(ProgItemType));
            //byte[] buffer = new byte[size];
            //System.IntPtr bufferIntPtr = Marshal.AllocHGlobal(size);
            //Marshal.StructureToPtr(type, bufferIntPtr, false);
            //ushort count = 1;
            //bool flag = LED_Control.LED_SenProgList(nDevID, bufferIntPtr, count);
            //if (flag)
            //{
            //    LED_Control.LED_NetInitial(nDevID, Password, LED_IP, SecTimeOut, RetryTimes, UDPPort);
            //    resalut = LED_Control.ShowString(nDevID, str);
            //    LED_Control.Close(nDevID);
            //}
            resalut = LED_Control.Led_TxtToXMPXFile(nDevID, str, 2, 1);
            LED_Control.Close(nDevID);
            return resalut;
        }

        public void Reset()
        {
            LED_Control.Reset(nDevID);
        }
    }
}