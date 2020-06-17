using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace LEDControl_UDP
{
    /// <summary>
    /// 控制LED。成员函数的常见参数说明：nDevID设备编号；drv驱动器编号（A盘为0，C盘为2）；返回值为true代表成功false为失败
    /// </summary>
    public class LED_UDP_Control
    {
        public ushort Left = 0;		// 区域左   , Left coordinate of the area.
        public ushort Top = 0;		// 区域上   , Top  coordinate of the area.
        public ushort Width = 256;		// 区域宽   , Width  of the area.
        public ushort Height = 64;		// 区域高   , Height of the area.
        public ushort Color = 1;		// 颜色     , Text color
        public ushort XPos = 0;		// x坐标    , X coordinate for output text.
        public ushort YPos = 0;		// y坐标    , Y coordinate for output text.
        public Byte Option = 2;
        public ushort PicFIndex = 4;

        //初始化
        [DllImport("MiniLED.DLL", CallingConvention = CallingConvention.Cdecl)]
        private static extern int MC_NetInitial(ushort nDevID, string Password, string RemoteIP, int SecTimeOut, int Retry, ushort UDPPort);
        public int LED_NetInitial(ushort nDevID, string Password, string RemoteIP, int SecTimeOut, int Retry, ushort UDPPort)
        {
            return MC_NetInitial(nDevID, Password, RemoteIP, SecTimeOut, Retry, UDPPort);
        }

        //实时显示文字
        [DllImport("MiniLED.DLL", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool MC_ShowString(ushort ID, ushort Left, ushort Top, ushort Width, ushort Height, ushort XPos, ushort YPos, ushort Color, string Str, Byte Option);
        public bool ShowString(ushort nDevID, string ShowText)
        {
            return MC_ShowString(nDevID, this.Left, this.Top, this.Width, this.Height, this.XPos, this.YPos, this.Color, "^F1" + ShowText, Option);
        }

        //查找局域网内的控制卡
        [DllImport("MiniLED.DLL", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool MC_SeekStart(ushort delay, string ip, ushort port, string mask);

        public bool SeekStart(string LED_IP, ushort port)
        {
            return MC_SeekStart(20, LED_IP, port, "255.255.255.0");
        }

        //发送节目列表
        [DllImport("MiniLED.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool MC_SendProgList(ushort ID, IntPtr ProgList, ushort ProgCount);
        public bool LED_SenProgList(ushort ID, IntPtr ProgList, ushort ProgCount)
        {
            return MC_SendProgList(ID, ProgList, ProgCount);
        }
        //关闭通讯
        [DllImport("MiniLED.DLL", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool MC_Close(ushort nDevid);
        public bool Close(ushort nDevid)
        {
            return MC_Close(nDevid);
        }

        [DllImport("MiniLED.DLL", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool MC_SeekClose();
        public bool SeekCLose()
        {
            return MC_SeekClose();
        }
        ////把文字先转换为图片再发
        //[DllImport("MiniLED.DLL", CallingConvention = CallingConvention.Cdecl)]
        //private static extern bool MC_TxtToXMPXFile(ushort ndevid, ushort picfindex, ushort width, ushort height, ushort color, string showstring, char encode, char mode);
        //public bool TxtToXMPXFile(ushort nDevID, string ShowText)
        //{
        //    return MC_TxtToXMPXFile(nDevID, 0, this.Width, this.Height, this.Color, ShowText, '2', '1');
        //}

        ////发送RAM文本
        //[DllImport("MiniLED.DLL", CallingConvention = CallingConvention.Cdecl)]
        //private static extern bool MC_SendText(ushort devid, ushort txtindex, ushort color, string showstring, char encode);
        //public bool SendText(ushort nDevID, string ShowText)
        //{
        //    return MC_SendText(nDevID, 1, this.Color, ShowText, '1');
        //}
        //控制卡复位
        [DllImport("MiniLED.DLL", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool MC_Reset(ushort id, ushort func);
        public bool Reset(ushort nDevID)
        {
            return MC_Reset(nDevID, 0);
        }
        [DllImport("MiniLED.DLL", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool MC_BroadcastSeek(string ip);
        public bool BroadcastSeek(string LED_IP)
        {
            return MC_BroadcastSeek(LED_IP);
        }
        //
        [DllImport("MiniLED.DLL", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool MC_TxtToXMPXFile(ushort ID, ushort PicFIndex, ushort Width, ushort Height, ushort Color, string Str, Byte Encode, Byte Mode);
        public bool Led_TxtToXMPXFile(ushort ID, string Str, Byte Encode, Byte Mode)
        {
            return MC_TxtToXMPXFile(ID, PicFIndex, Width, Height, Color, Str, Encode, Mode);
        }
    }
}