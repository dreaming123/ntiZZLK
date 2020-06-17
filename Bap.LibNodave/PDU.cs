using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Bap.LibNodave
{
    /// <summary>
    /// 协议数据单元PDU（Protocol Data Unit）
    /// </summary>
    public class PDU : PseudoPointer
    {
        [DllImport("libnodave.dll", EntryPoint = "daveNewPDU"/*, PreserveSig=false */ )]
        protected static extern IntPtr daveNewPDU();
        public PDU()
        {
            Pointer = daveNewPDU();
        }
    }

    public class ReadPDU : PDU
    {

        [DllImport("libnodave.dll", EntryPoint = "daveAddVarToReadRequest"/*, PreserveSig=false */ )]
        protected static extern void daveAddVarToReadRequest(IntPtr p, int area, int DBnum, int start, int bytes);
        public void AddVarToReadRequest(PlcMemoryArea area, int DBnum, int start, int bytes)
        {
            daveAddVarToReadRequest(Pointer, (int)area, DBnum, start, bytes);
        }

        [DllImport("libnodave.dll", EntryPoint = "daveAddBitVarToReadRequest"/*, PreserveSig=false */ )]
        protected static extern void daveAddBitVarToReadRequest(IntPtr p, int area, int DBnum, int start, int bytes);
        public void AddBitVarToReadRequest(PlcMemoryArea area, int DBnum, int start, int bytes)
        {
            daveAddBitVarToReadRequest(Pointer, (int)area, DBnum, start, bytes);
        }
    }

    public class WritePDU : PDU
    {

        [DllImport("libnodave.dll", EntryPoint = "daveAddVarToWriteRequest"/*, PreserveSig=false */ )]
        protected static extern void daveAddVarToWriteRequest(IntPtr p, int area, int DBnum, int start, int bytes, byte[] buffer);
        public void AddVarToWriteRequest(PlcMemoryArea area, int DBnum, int start, int bytes, byte[] buffer)
        {
            daveAddVarToWriteRequest(Pointer, (int)area, DBnum, start, bytes, buffer);
        }

        [DllImport("libnodave.dll", EntryPoint = "daveAddBitVarToWriteRequest"/*, PreserveSig=false */ )]
        protected static extern void daveAddBitVarToWriteRequest(IntPtr p, int area, int DBnum, int start, int bytes, byte[] buffer);
        public void AddBitVarToWriteRequest(PlcMemoryArea area, int DBnum, int start, int bytes, byte[] buffer)
        {
            daveAddBitVarToWriteRequest(Pointer, (int)area, DBnum, start, bytes, buffer);
        }
    }


}
