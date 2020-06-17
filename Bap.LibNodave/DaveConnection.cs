using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Bap.LibNodave
{
    
    public class DaveConnection : PseudoPointer
    {
        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern IntPtr daveNewConnection(IntPtr di, int MPI, int rack, int slot);
        public DaveConnection(DaveInterface di,int MPI,int rack,int slot)
        {
            Pointer = daveNewConnection(di.Pointer, MPI, rack, slot);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveConnectPLC(IntPtr dc);
        public int ConnectPLC()
        {
            return (int)daveConnectPLC(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveDisconnectPLC(IntPtr dc);
        public int DisconnectPLC()
        {
            return (int)daveDisconnectPLC(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveReadBytes(IntPtr dc, int area, int DBnumber, int start, int len, byte[] buffer);
        public int ReadBytes(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer)
        {
            return (int)daveReadBytes(Pointer, (int)area, DBnumber, start, len, buffer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveReadManyBytes(IntPtr dc, int area, int DBnumber, int start, int len, byte[] buffer);
        public int ReadManyBytes(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer)
        {
            return (int)daveReadManyBytes(Pointer, (int)area, DBnumber, start, len, buffer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveReadBits(IntPtr dc, int area, int DBnumber, int start, int len, byte[] buffer);
        public int ReadBits(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer)
        {
            return (int)daveReadBits(Pointer, (int)area, DBnumber, start, len, buffer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveWriteBytes(IntPtr dc, int area, int DBnumber, int start, int len, byte[] buffer);
        public int WriteBytes(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer)
        {
            return (int)daveWriteBytes(Pointer, (int)area, DBnumber, start, len, buffer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveWriteManyBytes(IntPtr dc,  int area, int DBnumber, int start, int len, byte[] buffer);
        public int WriteManyBytes(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer)
        {
            return (int)daveWriteManyBytes(Pointer, (int)area, DBnumber, start, len, buffer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveWriteBits(IntPtr dc, int area, int DBnumber, int start, int len, byte[] buffer);
        public int WriteBits(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer)
        {
            return (int)daveWriteBits(Pointer, (int)area, DBnumber, start, len, buffer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveGetS32(IntPtr dc);
        public int GetS32()
        {
            return daveGetS32(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern uint daveGetU32(IntPtr dc);
        public uint GetU32()
        {
            return daveGetU32(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern short daveGetS16(IntPtr dc);
        public short GetS16()
        {
            return daveGetS16(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern ushort daveGetU16(IntPtr dc);
        public ushort GetU16()
        {
            return daveGetU16(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern sbyte daveGetS8(IntPtr dc);
        public sbyte GetS8()
        {
            return daveGetS8(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern byte daveGetU8(IntPtr dc);
        public byte GetU8()
        {
            return daveGetU8(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern float daveGetFloat(IntPtr dc);
        public float GetFloat()
        {
            return daveGetFloat(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveGetCounterValue(IntPtr dc);
        public int GetCounterValue()
        {
            return daveGetCounterValue(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern float daveGetSeconds(IntPtr dc);
        public float GetSeconds()
        {
            return daveGetSeconds(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveGetS32At(IntPtr dc, int pos);
        public int GetS32At(int pos)
        {
            return daveGetS32At(Pointer, pos);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern uint daveGetU32At(IntPtr dc, int pos);
        public uint GetU32At(int pos)
        {
            return daveGetU32At(Pointer, pos);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern short daveGetS16At(IntPtr dc, int pos);
        public short GetS16At(int pos)
        {
            return daveGetS16At(Pointer, pos);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern ushort daveGetU16At(IntPtr dc, int pos);
        public ushort GetU16At(int pos)
        {
            return daveGetU16At(Pointer, pos);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern sbyte daveGetS8At(IntPtr dc, int pos);
        public sbyte GetS8At(int pos)
        {
            return daveGetS8At(Pointer, pos);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern byte daveGetU8At(IntPtr dc, int pos);
        public byte GetU8At(int pos)
        {
            return daveGetU8At(Pointer, pos);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern float daveGetFloatAt(IntPtr dc, int pos);
        public float GetFloatAt(int pos)
        {
            return daveGetFloatAt(Pointer, pos);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveGetCounterValueAt(IntPtr dc, int pos);
        public int GetCounterValueAt(int pos)
        {
            return daveGetCounterValueAt(Pointer, pos);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern float daveGetSecondsAt(IntPtr dc, int pos);
        public float GetSecondsAt(int pos)
        {
            return daveGetSecondsAt(Pointer, pos);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveGetAnswLen(IntPtr dc);
        public int GetAnswLen()
        {
            return daveGetAnswLen(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveGetMaxPDULen(IntPtr dc);
        public int GetMaxPDULen()
        {
            return daveGetMaxPDULen(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int davePrepareReadRequest(IntPtr dc, IntPtr p);
        public ReadPDU PrepareReadRequest()
        {
            ReadPDU p = new ReadPDU();
            davePrepareReadRequest(Pointer, p.Pointer);
            return p;
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int davePrepareWriteRequest(IntPtr dc, IntPtr p);
        public WritePDU PrepareWriteRequest()
        {
            WritePDU p = new WritePDU();
            davePrepareWriteRequest(Pointer, p.Pointer);
            return p;
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveExecReadRequest(IntPtr dc, IntPtr p, IntPtr rl);
        public int ExecReadRequest(ReadPDU p, ResultSet rl)
        {
            return daveExecReadRequest(Pointer, p.Pointer, rl.Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveExecWriteRequest(IntPtr dc, IntPtr p, IntPtr rl);
        public int ExecWriteRequest(WritePDU p, ResultSet rl)
        {
            return daveExecWriteRequest(Pointer, p.Pointer, rl.Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveUseResult(IntPtr dc, IntPtr rs, int number);
        public int UseResult(ResultSet rs, int number)
        {
            return daveUseResult(Pointer, rs.Pointer, number);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveReadSZL(IntPtr dc, int id, int index, byte[] ddd, int len);
        public int ReadSZL(int id, int index, byte[] ddd, int len)
        {
            return daveReadSZL(Pointer, id, index, ddd, len);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveStart(IntPtr dc);
        public int Start()
        {
            return daveStart(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveStop(IntPtr dc);
        public int Stop()
        {
            return daveStop(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveForce200(IntPtr dc, int area, int start, int val);
        public int Force200(PlcMemoryArea area, int start, int val)
        {
            return daveForce200(Pointer, (int)area, start, val);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveForceDisconnectIBH(IntPtr dc, int src, int dest, int MPI);
        public int ForceDisconnectIBH(int src, int dest, int MPI)
        {
            return daveForceDisconnectIBH(Pointer, src, dest, MPI);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveGetResponse(IntPtr dc);
        public int GetResponse()
        {
            return daveGetResponse(Pointer);
        }

        [DllImport("libnodave.dll"/*, PreserveSig=false */ )]
        protected static extern int daveSendMessage(IntPtr dc, IntPtr p);
        public int SendMessage(PDU p)
        {
            return daveSendMessage(Pointer, p.Pointer);
        }

        [DllImport("libnodave.dll")]
        protected static extern int daveGetProgramBlock(IntPtr dc, int blockType, int number, byte[] buffer, ref int length);
        public int GetProgramBlock(S7BlockType blockType, int number, byte[] buffer, ref int length)
        {
            Console.WriteLine("length:" + length);
            int a = daveGetProgramBlock(Pointer, (int)blockType, number, buffer, ref length);
            Console.WriteLine("length:" + length);
            return a;
        }

        [DllImport("libnodave.dll")]
        protected static extern int daveListBlocksOfType(IntPtr dc, int blockType, byte[] buffer);
        public int ListBlocksOfType(S7BlockType blockType, byte[] buffer)
        {
            return daveListBlocksOfType(Pointer, (int)blockType, buffer);
        }
    }
}
