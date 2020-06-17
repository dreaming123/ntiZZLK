using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bap.LibNodave;

namespace COMRW
{
    public class RWLOPCServerl
    {
        PlcTcpClient mPLC = null;

        public RWLOPCServerl(string ip)
        {
            mPLC = new PlcTcpClient(ip, 102);
        }

        public bool Connect()
        {
            int i = mPLC.ConnectPLC();
            if (i == 0)
                return true;
            return false;
        }

        public bool Write(int DBNumber, int start, int len, byte[] msg)
        {
            if (mPLC.IsConnected)
            {
                int i = mPLC.WriteManyBytes(PlcMemoryArea.DB, DBNumber, start, len, msg);
                if (i == 0)
                    return true;
            }
            return false;
        }
        public bool WriteBits(int DBNumber, int start, int len, byte[] msg)
        {
            if (mPLC.IsConnected)
            {
                int i = mPLC.WriteBits(PlcMemoryArea.DB, DBNumber, start, len, msg);
                if (i == 0)
                    return true;
            }
            return false;
        }
        public bool Read(int DBNumber, int start, int len, ref byte[] msg)
        {
            if (mPLC.IsConnected)
            {
                int i = mPLC.ReadManyBytes(PlcMemoryArea.DB, DBNumber, start, len, msg);
                if (i == 0)
                    return true;
            }
            return false;
        }
        
        public void DisConnect()
        {
            mPLC.DisConnectPLC();
        }
    }
}
