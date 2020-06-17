using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bap.LibNodave
{
    public class PlcDataHelper
    {
        public static short GetS16From(byte[] b, int pos)
        {
            if (BitConverter.IsLittleEndian)
            {
                byte[] b1 = new byte[2];
                b1[1] = b[pos + 0];
                b1[0] = b[pos + 1];
                return BitConverter.ToInt16(b1, 0);
            }
            else
                return BitConverter.ToInt16(b, pos);
        }

        public static ushort GetU16From(byte[] b, int pos)
        {
            if (BitConverter.IsLittleEndian)
            {
                byte[] b1 = new byte[2];
                b1[1] = b[pos + 0];
                b1[0] = b[pos + 1];
                return BitConverter.ToUInt16(b1, 0);
            }
            else
                return BitConverter.ToUInt16(b, pos);
        }

        public static int GetS32From(byte[] b, int pos)
        {
            if (BitConverter.IsLittleEndian)
            {
                byte[] b1 = new byte[4];
                b1[3] = b[pos];
                b1[2] = b[pos + 1];
                b1[1] = b[pos + 2];
                b1[0] = b[pos + 3];
                return BitConverter.ToInt32(b1, 0);
            }
            else
                return BitConverter.ToInt32(b, pos);
        }

        public static uint GetU32From(byte[] b, int pos)
        {
            if (BitConverter.IsLittleEndian)
            {
                byte[] b1 = new byte[4];
                b1[3] = b[pos];
                b1[2] = b[pos + 1];
                b1[1] = b[pos + 2];
                b1[0] = b[pos + 3];
                return BitConverter.ToUInt32(b1, 0);
            }
            else
                return BitConverter.ToUInt32(b, pos);
        }

        public static float GetFloatFrom(byte[] b, int pos)
        {
            if (BitConverter.IsLittleEndian)
            {
                byte[] b1 = new byte[4];
                b1[3] = b[pos];
                b1[2] = b[pos + 1];
                b1[1] = b[pos + 2];
                b1[0] = b[pos + 3];
                return BitConverter.ToSingle(b1, 0);
            }
            else
                return BitConverter.ToSingle(b, pos);
        }

        public static short[] GetS16Array(byte[] b)
        {
            List<short> sl = new List<short>();
            for (int i = 0; i < b.Length - 2; i += 2)
            {
                short s = GetS16From(b, i);
                sl.Add(s);
            }
            return sl.ToArray();
        }

        public static ushort[] GetU16Array(byte[] b)
        {
            List<ushort> sl = new List<ushort>();
            for (int i = 0; i < b.Length - 2; i += 2)
            {
                ushort s = GetU16From(b, i);
                sl.Add(s);
            }
            return sl.ToArray();
        }

        public static int[] GetS32Array(byte[] b)
        {
            List<int> sl = new List<int>();
            for (int i = 0; i < b.Length - 4; i += 4)
            {
                int s = GetS32From(b, i);
                sl.Add(s);
            }
            return sl.ToArray();
        }

        public static uint[] GetU32Array(byte[] b)
        {
            List<uint> sl = new List<uint>();
            for (int i = 0; i < b.Length - 4; i += 4)
            {
                uint s = GetU32From(b, i);
                sl.Add(s);
            }
            return sl.ToArray();
        }

        public static byte[] SetPlcData(short s)
        {
            if (BitConverter.IsLittleEndian)
                return BitConverter.GetBytes(s).Reverse().ToArray();
            else
                return BitConverter.GetBytes(s);
        }

        public static byte[] SetPlcData(ushort us)
        {
            if (BitConverter.IsLittleEndian)
                return BitConverter.GetBytes(us).Reverse().ToArray();
            else
                return BitConverter.GetBytes(us);
        }

        public static byte[] SetPlcData(int i)
        {
            if (BitConverter.IsLittleEndian)
                return BitConverter.GetBytes(i).Reverse().ToArray();
            else
                return BitConverter.GetBytes(i);
        }

        public static byte[] SetPlcData(uint ui)
        {
            if (BitConverter.IsLittleEndian)
                return BitConverter.GetBytes(ui).Reverse().ToArray();
            else
                return BitConverter.GetBytes(ui);
        }

        public static byte[] SetPlcData(float f)
        {
            if (BitConverter.IsLittleEndian)
                return BitConverter.GetBytes(f).Reverse().ToArray();
            else
                return BitConverter.GetBytes(f);
        }

        public static byte[] SetPlcData(float[] fa)
        {
            List<byte> lst = new List<byte>();
            foreach (float f in fa)
            {
                lst.AddRange(SetPlcData(f));
            }
            return lst.ToArray();
        }

        public static byte[] SetPlcData(short[] sa)
        {
            List<byte> lst = new List<byte>();
            foreach (short s in sa)
            {
                lst.AddRange(SetPlcData(s));
            }
            return lst.ToArray();
        }

        public static byte[] SetPlcData(ushort[] usa)
        {
            List<byte> lst = new List<byte>();
            foreach (ushort us in usa)
            {
                lst.AddRange(SetPlcData(us));
            }
            return lst.ToArray();
        }

        public static byte[] SetPlcData(int[] ia)
        {
            List<byte> lst = new List<byte>();
            foreach (int i in ia)
            {
                lst.AddRange(SetPlcData(i));
            }
            return lst.ToArray();
        }

        public static byte[] SetPlcData(uint[] uia)
        {
            List<byte> lst = new List<byte>();
            foreach (uint ui in uia)
            {
                lst.AddRange(SetPlcData(ui));
            }
            return lst.ToArray();
        }

        [DllImport("libnodave.dll", EntryPoint = "toPLCfloat"/*, PreserveSig=false */ )]
        private static extern float toPLCfloat(float f);
        public static float ToPLCFloat(float f)
        {
            return toPLCfloat(f);
        }

        [DllImport("libnodave.dll", EntryPoint = "daveToPLCfloat" /*, PreserveSig=false */ )]
        private static extern int daveToPLCfloat(float f);
        public static int DaveToPLCFloat(float f)
        {
            return daveToPLCfloat(f);
        }

        [DllImport("libnodave.dll", EntryPoint = "daveSwapIed_32"/*, PreserveSig=false */ )]
        private static extern int daveSwapIed_32(int i);
        public static int SwapIed_32(int i)
        {
            return daveSwapIed_32(i);
        }

        [DllImport("libnodave.dll", EntryPoint = "daveSwapIed_16"/*, PreserveSig=false */ )]
        private static extern int daveSwapIed_16(int i);
        public static int SwapIed_16(int i)
        {
            return daveSwapIed_16(i);
        }
        


    }
}
