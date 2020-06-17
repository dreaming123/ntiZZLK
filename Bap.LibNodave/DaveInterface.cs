using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Bap.LibNodave
{
    public class DaveInterface : PseudoPointer
    {
        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        private static extern IntPtr daveNewInterface(
            [MarshalAs(UnmanagedType.Struct)] FD fd,
            [MarshalAs(UnmanagedType.LPStr)] string name,
            int localMPI,
            int useProto,
            int speed);
        public DaveInterface(FD fd, string name, int localMPI, Protocols useProto, ProfiBusSpeed speed)
        {
            Pointer = daveNewInterface(fd, name, localMPI, (int)useProto, (int)speed);
        }


        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveInitAdapter(IntPtr di);
        public int InitAdapter()
        {
            return (int)daveInitAdapter(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveListReachablePartners(IntPtr di, byte[] buffer);
        public int ListReachablePartners(byte[] buffer)
        {
            return (int)daveListReachablePartners(Pointer, buffer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern void daveSetTimeout(IntPtr di, int time);
        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveGetTimeout(IntPtr di);
        public int Timeout
        {
            get{return daveGetTimeout(Pointer);}
            set { daveSetTimeout(Pointer, value); }
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern IntPtr daveDisconnectAdapter(IntPtr di);
        public IntPtr disconnectAdapter()
        {
            return daveDisconnectAdapter(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern string daveGetName(IntPtr di);
        public string Name
        {
            get
            {
                return daveGetName(Pointer);
            }
        }

    }
}
 