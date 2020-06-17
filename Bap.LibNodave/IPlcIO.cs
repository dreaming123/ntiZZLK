using System;
namespace Bap.LibNodave
{
    public interface IPlcIO : IDisposable
    {
        int ConnectPLC();
        void DisConnectPLC();
        int ExecReadRequest(ReadPDU p, ResultSet rl);
        int ExecWriteRequest(WritePDU p, ResultSet rl);
        int Force200(PlcMemoryArea area, int start, int val);
        int ForceDisconnectIBH(int src, int dest, int MPI);
        int GetAnswLen();
        int GetCounterValue();
        int GetCounterValueAt(int pos);
        float GetFloat();
        float GetFloatAt(int pos);
        int GetMaxPDULen();
        int GetProgramBlock(S7BlockType blockType, int number, byte[] buffer, ref int length);
        int GetResponse();
        short GetS16();
        short GetS16At(int pos);
        int GetS32();
        int GetS32At(int pos);
        sbyte GetS8();
        sbyte GetS8At(int pos);
        float GetSeconds();
        float GetSecondsAt(int pos);
        ushort GetU16();
        ushort GetU16At(int pos);
        uint GetU32();
        uint GetU32At(int pos);
        byte GetU8();
        byte GetU8At(int pos);
        string IpAddress { get; }
        bool IsConnected { get; }
        int ListBlocksOfType(S7BlockType blockType, byte[] buffer);
        string Name { get; }
        int PlcRack { get; }
        int PlcSlot { get; }
        ReadPDU PrepareReadRequest();
        WritePDU PrepareWriteRequest();
        int ReadBits(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer);
        int ReadBytes(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer);
        int ReadManyBytes(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer);
        int ReadSZL(int id, int index, byte[] ddd, int len);
        int SendMessage(PDU p);
        int Start();
        int Stop();
        int TcpPort { get; }
        int UseResult(ResultSet rs, int number);
        int WriteBits(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer);
        int WriteBytes(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer);
        int WriteManyBytes(PlcMemoryArea area, int DBnumber, int start, int len, byte[] buffer);
    }
}
