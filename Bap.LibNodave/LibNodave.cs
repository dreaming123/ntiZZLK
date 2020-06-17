using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Bap.LibNodave
{
    public class LibNodave
    {
        public static int daveMPIReachable = 0x30;
        public static int daveMPIunused = 0x10;
        public static int davePartnerListSize = 126;
        public static readonly int daveMaxRawLen = 2048;

        [DllImport("libnodave.dll", EntryPoint = "daveStrerror")]
        private static extern IntPtr daveStrerror(int res);
        public static string DaveStrError(int res)
        {
            return Marshal.PtrToStringAuto(daveStrerror((int)res));
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        private static extern void daveSetDebug(int newDebugLevel);
        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        private static extern int daveGetDebug();
        public static DebugLevel Debug
        {
            get { return (DebugLevel)daveGetDebug(); }
            set { daveSetDebug((int)value); }
        }

        [DllImport("libnodave.dll", EntryPoint = "setPort"/*, PreserveSig=false */ )]
        private static extern int setPort(
            [MarshalAs(UnmanagedType.LPStr)] string portName,
            [MarshalAs(UnmanagedType.LPStr)] string baud,
            int parity);
        public int SetPort(string portName, string baud, int parity)
        {
            return (int)setPort(portName, baud, parity);
        }

        [DllImport("libnodave.dll", EntryPoint = "closePort" /*, PreserveSig=false */ )]
        private static extern int closePort(int port);
        public static int ClosePort(int port)
        {
            return closePort(port);
        }

        [DllImport("libnodave.dll", EntryPoint = "openS7online" /*, PreserveSig=false */ )]
        private static extern int openS7online(
        [MarshalAs(UnmanagedType.LPStr)] string portName
        );
        public static int OpenS7OnLine(string portName)
        {
            return openS7online(portName);
        }

        [DllImport("libnodave.dll", EntryPoint = "closeS7online" /*, PreserveSig=false */ )]
        private static extern int closeS7online(int port);
        public static int CloseS7Online(int port)
        {
            return closeS7online(port);
        }

        [DllImport("libnodave.dll", EntryPoint = "daveAreaName"/*, PreserveSig=false */ )]
        private static extern int daveAreaName(int area);
        [DllImport("libnodave.dll", EntryPoint = "daveBlockName"/*, PreserveSig=false */ )]
        private static extern int daveBlockName(int blockType);
        [DllImport("libnodave.dll", EntryPoint = "daveStringCopy"/*, PreserveSig=false */ )]
        private static extern void daveStringCopy(int i, byte[] c);

        public static string BlockName(S7BlockType blockType)
        {
            byte[] s = new byte[255];
            int i = daveBlockName((int)blockType);
            daveStringCopy(i, s);
            string st = "";
            i = 0;
            while (s[i] != 0)
            {
                st = st + (char)s[i];
                i++;
            }
            return st;
        }

        public static string AreaName(S7BlockType blockType)
        {
            byte[] s = new byte[255];
            int i = daveAreaName((int)blockType);
            daveStringCopy(i, s);
            string st = "";
            i = 0;
            while (s[i] != 0)
            {
                st = st + (char)s[i];
                i++;
            }
            return st;
        }

 


    }
}
