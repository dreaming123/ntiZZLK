using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;

namespace Bap.LibNodave
{
    public class PlcTcpClient : IPlcIO
    {
        private FD mFds;
        private DaveInterface mDI = null;
        private static DaveConnection mConn = null;
        private TcpClient cliSocket;
        private object ob = new object();

        public int TcpPort { get; private set; }
        public string IpAddress { get; private set; }
        public int PlcRack { get; private set; }
        public int PlcSlot { get; private set; }
        public string Name { get; private set; }
        public bool IsConnected { get; private set; }

        public PlcTcpClient(string ip, int port, int rack = 0, int slot = 1, string name = "PlcTcpClient")
        {
            IpAddress = ip;
            TcpPort = port;
            PlcRack = rack;
            PlcSlot = slot;
            Name = name;
            IsConnected = false;
        }

        public int ConnectPLC()
        {
            if (IsConnected) return -1;
            //PLC连不上时临时使用
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(IpAddress), TcpPort);
            cliSocket = TimeOutSocket.Connect(iep, 1000);
            if (cliSocket != null)
            {
                mFds.rfd = OpenSocket(102, IpAddress);
                if (mFds.rfd <= 0) return mFds.rfd; //FD一般是不可能为0的，0、1、2、3、4等估计早被操作系统或者其他程序占用了

                mFds.wfd = mFds.rfd;
                mDI = new DaveInterface(mFds, "Test", 0, Protocols.ISOTCP, ProfiBusSpeed.Speed187k);
                mDI.Timeout = 1;
                mConn = new DaveConnection(mDI, 0, PlcRack, PlcSlot);
                int rc = mConn.ConnectPLC();
                if (rc == 0)
                {
                    IsConnected = true;
                    return rc;
                }
                return rc;
            }
            return -1;
        }

        public void DisConnectPLC()
        {
            if (!IsConnected) return;


            if (mConn != null)
            {
                mConn.DisconnectPLC();
                mConn.Dispose();
            }

            if (mDI != null)
                mDI.Dispose();

            CloseSocket(mFds.rfd);
            mConn = null;
            mDI = null;
            IsConnected = false;
        }

        public int ReadBytes(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer)
        {
            return mConn.ReadBytes(area, DBnumber, start, len, buffer);
        }

        public int ReadManyBytes(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer)
        {
            try
            {
                lock (ob)
                {
                    return mConn.ReadManyBytes(area, DBnumber, start, len, buffer);
                }
            }
            catch
            { return 0; }
        }

        public int ReadBits(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer)
        {
            lock (ob)
            {
                return mConn.ReadBits(area, DBnumber, start, len, buffer);
            }
        }

        public int WriteBytes(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer)
        {
            return mConn.WriteBytes(area, DBnumber, start, len, buffer);
        }

        public int WriteManyBytes(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer)
        {
            lock (ob)
            {
                return mConn.WriteManyBytes(area, DBnumber, start, len, buffer);
            }
        }

        public int WriteBits(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer)
        {
            lock (ob)
            {
                return mConn.WriteBits(area, DBnumber, start, len, buffer);
            }
        }


        /// <summary>
        /// int
        /// </summary>
        /// <returns></returns>
        public int GetS32()
        {
            return mConn.GetS32();
        }

        /// <summary>
        /// uint
        /// </summary>
        /// <returns></returns>
        public uint GetU32()
        {
            return mConn.GetU32();
        }

        /// <summary>
        /// short
        /// </summary>
        /// <returns></returns>
        public short GetS16()
        {
            return mConn.GetS16();
        }

        /// <summary>
        /// ushort
        /// </summary>
        /// <returns></returns>
        public ushort GetU16()
        {
            return mConn.GetU16();
        }

        /// <summary>
        /// sbyte
        /// </summary>
        /// <returns></returns>
        public sbyte GetS8()
        {
            return mConn.GetS8();
        }

        /// <summary>
        /// byte
        /// </summary>
        /// <returns></returns>
        public byte GetU8()
        {
            return mConn.GetU8();
        }

        public byte GetU8At(int pos)
        {
            return mConn.GetU8At(pos);
        }

        public float GetFloat()
        {
            return mConn.GetFloat();
        }

        public int GetCounterValue()
        {
            return mConn.GetCounterValue();
        }

        public float GetSeconds()
        {
            return mConn.GetSeconds();
        }

        public int GetS32At(int pos)
        {
            return mConn.GetS32At(pos);
        }

        public uint GetU32At(int pos)
        {
            return mConn.GetU32At(pos);
        }

        public short GetS16At(int pos)
        {
            return mConn.GetS16At(pos);
        }

        public ushort GetU16At(int pos)
        {
            return mConn.GetU16At(pos);
        }

        public sbyte GetS8At(int pos)
        {
            return mConn.GetS8At(pos);
        }

        public float GetFloatAt(int pos)
        {
            return mConn.GetFloatAt(pos);
        }

        public int GetCounterValueAt(int pos)
        {
            return mConn.GetCounterValueAt(pos);
        }

        public float GetSecondsAt(int pos)
        {
            return mConn.GetSecondsAt(pos);
        }

        public int GetAnswLen()
        {
            return mConn.GetAnswLen();
        }

        public int GetMaxPDULen()
        {
            return mConn.GetMaxPDULen();
        }

        public ReadPDU PrepareReadRequest()
        {
            return mConn.PrepareReadRequest();
        }

        public WritePDU PrepareWriteRequest()
        {
            return mConn.PrepareWriteRequest();
        }

        public int ExecReadRequest(ReadPDU p, ResultSet rl)
        {
            return mConn.ExecReadRequest(p, rl);
        }

        public int ExecWriteRequest(WritePDU p, ResultSet rl)
        {
            return mConn.ExecWriteRequest(p, rl);
        }

        public int UseResult(ResultSet rs, int number)
        {
            return mConn.UseResult(rs, number);
        }

        public int ReadSZL(int id, int index, byte[] ddd, int len)
        {
            return mConn.ReadSZL(id, index, ddd, len);
        }

        public int Start()
        {
            return mConn.Start();
        }

        public int Stop()
        {
            return mConn.Stop();
        }

        public int Force200(PlcMemoryArea area, int start, int val)
        {
            return mConn.Force200(area, start, val);
        }

        public int ForceDisconnectIBH(int src, int dest, int MPI)
        {
            return mConn.ForceDisconnectIBH(src, dest, MPI);
        }

        public int GetResponse()
        {
            return mConn.GetResponse();
        }

        public int SendMessage(PDU p)
        {
            return mConn.SendMessage(p);
        }

        public int GetProgramBlock(S7BlockType blockType, int number, byte[] buffer, ref int length)
        {
            return mConn.GetProgramBlock(blockType, number, buffer, ref length);
        }

        public int ListBlocksOfType(S7BlockType blockType, byte[] buffer)
        {
            return mConn.ListBlocksOfType(blockType, buffer);
        }

        public void Dispose()
        {
            DisConnectPLC();
        }

        [DllImport("libnodave.dll", EntryPoint = "closeSocket"/*, PreserveSig=false */ )]
        private static extern int CloseSocket(int port);

        [DllImport("libnodave.dll", EntryPoint = "openSocket"/*, PreserveSig=false */ )]
        private static extern int OpenSocket(
            int port,
            [MarshalAs(UnmanagedType.LPStr)] string ipAddress);

    }


}
