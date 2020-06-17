using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using BLL;
using COMRW;
//using System.Data;
//using System.Runtime.InteropServices;
using TimeSpanCount;

namespace COMResolution
{
    public class CRLConveryControl : CRLBase
    {
        //public delegate void GetEquipment_Inquery(List<CRLStationInfo> stationinfo, bool flag);
        public delegate void GetEquipment_Inquery(List<CRLStationInfo> stationinfo, string time);
        public event GetEquipment_Inquery GetEquipMentInfo;
        private byte[] ConveryBuffer;
        private PerformanceTimer pft = new PerformanceTimer();

        //public CRLConveryControl(string ip)
        //{
        //    _ip = ip;
        //    mPLC = new RWLOPCServerl(_ip);
        //}

        //public bool InitOPCData(DataSet ds)
        //{
        //    if (ds != null)
        //    {
        //        DataTable dt = ds.Tables[0];
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            if (dt.Rows[i]["userid"].ToString() == _ip)
        //            {
        //                if (dt.Rows[i]["groupname"].ToString() == "Convery")
        //                {
        //                    CRLStationInfo si = new CRLStationInfo();
        //                    si.stationno = int.Parse(dt.Rows[i]["stationno"].ToString());
        //                    si.stationtype = int.Parse(dt.Rows[i]["stationtype"].ToString());
        //                    si.LocationX = int.Parse(dt.Rows[i]["field1"].ToString());
        //                    si.LocationY = int.Parse(dt.Rows[i]["field2"].ToString());
        //                    si.Width = SplitWidth(dt.Rows[i]["remark"].ToString())[0];
        //                    si.Height = SplitWidth(dt.Rows[i]["remark"].ToString())[1];
        //                    si.value = int.Parse(dt.Rows[i]["value"].ToString());
        //                    si.config = dt.Rows[i]["field3"].ToString();
        //                    StationInfo.Add(si);

        //                    CRLOPCItem findco = ItemInfo.Find(
        //                        delegate(CRLOPCItem ii)
        //                        {
        //                            return ii.groupname == "Convery";
        //                        }
        //                    );
        //                    if (findco == null)
        //                    {
        //                        CRLOPCItem coi = new CRLOPCItem();
        //                        coi.groupname = dt.Rows[i]["groupname"].ToString();
        //                        coi.ipaddress = dt.Rows[i]["userid"].ToString();
        //                        coi.DBAddress = int.Parse(dt.Rows[i]["objects"].ToString());
        //                        coi.start = int.Parse(dt.Rows[i]["value"].ToString());
        //                        coi.length = int.Parse(dt.Rows[i]["signaltype"].ToString());
        //                        ItemInfo.Add(coi);
        //                    }
        //                    else
        //                        findco.length += 20;//leo 30 to 20
        //                }
        //                else
        //                {
        //                    CRLOPCItem co = new CRLOPCItem();
        //                    co.groupname = dt.Rows[i]["groupname"].ToString();
        //                    co.ipaddress = dt.Rows[i]["userid"].ToString();
        //                    co.DBAddress = int.Parse(dt.Rows[i]["objects"].ToString());
        //                    co.start = int.Parse(dt.Rows[i]["value"].ToString());
        //                    co.length = int.Parse(dt.Rows[i]["stationno"].ToString());
        //                    ItemInfo.Add(co);
        //                }
        //            }
        //        }
        //        return true;
        //    }
        //    return false;
        //}

        public bool InitOPCData(DataSet ds)
        {
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _ip = dt.Rows[i]["userid"].ToString();
                    if (!PLCList.ContainsKey(_ip))
                    {
                        PLCList.Add(_ip, new RWLOPCServerl(_ip));
                        PLCFlag.Add(_ip, false);
                        PLCList[_ip].Connect();
                    }

                    switch (dt.Rows[i]["groupname"].ToString())
                    {
                        case "Convery": ConveryInit(dt.Rows[i]); break;
                        //case "OutPick": OutPickInit(dt.Rows[i]); break;
                        //case "InPick": InPickInit(dt.Rows[i]); break;
                        default: OtherInit(dt.Rows[i]); break;
                    }
                    //if (dt.Rows[i]["groupname"].ToString() == "Convery")
                    //{
                    //    CRLOPCItem findco = ItemInfo.Find(
                    //        delegate(CRLOPCItem ii)
                    //        {
                    //            return ii.groupname == "Convery";
                    //        }
                    //    );
                    //    if (findco == null)
                    //    {
                    //        CRLOPCItem coi = new CRLOPCItem();
                    //        coi.groupname = dt.Rows[i]["groupname"].ToString();
                    //        coi.ipaddress = dt.Rows[i]["userid"].ToString();
                    //        coi.DBAddress = int.Parse(dt.Rows[i]["objects"].ToString());
                    //        coi.start = int.Parse(dt.Rows[i]["value"].ToString());
                    //        coi.length = int.Parse(dt.Rows[i]["signaltype"].ToString());
                    //        ItemInfo.Add(coi);
                    //    }
                    //    else
                    //        findco.length += 20;//leo 30 to 20
                    //}
                    //else
                    //{
                    //    CRLOPCItem co = new CRLOPCItem();
                    //    co.groupname = dt.Rows[i]["groupname"].ToString();
                    //    co.ipaddress = dt.Rows[i]["userid"].ToString();
                    //    co.DBAddress = int.Parse(dt.Rows[i]["objects"].ToString());
                    //    co.start = int.Parse(dt.Rows[i]["value"].ToString());
                    //    co.length = int.Parse(dt.Rows[i]["stationno"].ToString());
                    //    ItemInfo.Add(co);
                    //}
                }
                return true;
            }
            return false;
        }
        private void ConveryInit(DataRow dt)
        {
            CRLStationInfo si = new CRLStationInfo();
            si.stationno = int.Parse(dt["stationno"].ToString());
            si.stationtype = int.Parse(dt["stationtype"].ToString());
            si.LocationX = int.Parse(dt["field1"].ToString());
            si.LocationY = int.Parse(dt["field2"].ToString());
            si.Width = SplitWidth(dt["remark"].ToString())[0];
            si.Height = SplitWidth(dt["remark"].ToString())[1];
            si.value = int.Parse(dt["value"].ToString());
            si.length = int.Parse(dt["signaltype"].ToString());
            si.config = dt["field3"].ToString();
            si.key_ip = dt["userid"].ToString();
            si.DBAddress = int.Parse(dt["objects"].ToString());
            StationInfo.Add(si);            
        }

        private void OtherInit(DataRow dt)
        {
            CRLOPCItem co = new CRLOPCItem();
            co.groupname = dt["groupname"].ToString();
            co.ipaddress = dt["userid"].ToString();
            co.DBAddress = int.Parse(dt["objects"].ToString());
            co.start = int.Parse(dt["value"].ToString());
            co.length = int.Parse(dt["stationno"].ToString());
            ItemInfo.Add(co);
        }

        public void Run()
        {
            //mPLC.Connect();
            ComThread = new Thread(new ThreadStart(TimeScan));
            ComThread.IsBackground = true;
            ComThread.Start();
        }

        private int[] SplitWidth(string str)
        {
            int[] ret = new int[2] { 1, 1 };
            string[] ss = str.Split(new char[] { ',' });
            if (ss != null && ss.Length > 1)
            {
                ret[0] = int.Parse(ss[0]);
                ret[1] = int.Parse(ss[1]);
            }
            return ret;
        }

        private void BindStation(int stationno, byte[] byt)
        {
            CRLStationInfo si = StationInfo.Find(
                delegate(CRLStationInfo ss)
                {
                    return ss.stationno == stationno;
                }
            );

            if (stationno == 1276)
            { }
            if (si != null)//leo 绑定站台信息
            {
                si.taskno = byt[0] * 256 + byt[1];
                si.barcode1 = (byt[2] * 256 + byt[3]) * 65536 + byt[4] * 256 + byt[5];//托盘条码
                si.goodstype = byt[6] * 256 + byt[7];//货物类型（1. 空托盘组 2. 实托盘3.空托盘4. 单烟包）
                si.fromstation = byt[8] * 256 + byt[9];
                si.tostation = byt[10] * 256 + byt[11];
                si.weight1 = byt[12] * 256 + byt[13];
                si.field1 = byt[18] * 256 + byt[19];//备用
                //si.barcode2 = (byt[20] * 256 + byt[21]) * 256 + byt[22] * 256 + byt[23];//下层烟箱号
                si.weight2 = byt[14] * 256 + byt[15];
                //si.field3 = byt[16] * 256 + byt[17];
                si.checkinfo = byt[16] * 256 + byt[17];
            }
        }               

        public string[] ReadStationInfo(int x, int y)
        {
            string[] info = new string[11];
            CRLStationInfo si = StationInfo.Find(
                delegate(CRLStationInfo ss)
                {
                    return ss.LocationX == x && ss.LocationY == y;
                }
            );
            if (si != null)
            {
                info[0] = si.stationno.ToString();
                info[1] = si.taskno.ToString();
                info[2] = si.field1.ToString();
                info[3] = si.goodstype.ToString();
                info[4] = si.fromstation.ToString();
                info[5] = si.tostation.ToString();
                info[6] = si.barcode1 == 0 ? "0" : si.barcode1.ToString().PadLeft(8, '0');
                //info[7] = si.barcode2 == 0 ? "0" : si.barcode2.ToString().PadLeft(4, '0');
                info[7] = si.weight2.ToString();
                info[8] = si.checkinfo.ToString();
                info[9] = si.weight1.ToString();//si.checkinfo.ToString();
            }
            return info;
        }

        public string[] ReadStationInfo(int stationno)
        {
            string[] info = new string[11];
            CRLStationInfo si = StationInfo.Find(
                delegate(CRLStationInfo ss)
                {
                    return ss.stationno == stationno;
                }
            );
            if (si != null)
            {
                info[0] = si.stationno.ToString();
                info[1] = si.taskno.ToString();
                info[2] = si.field1.ToString();
                info[3] = si.goodstype.ToString();
                info[4] = si.fromstation.ToString();
                info[5] = si.tostation.ToString();
                info[6] = si.barcode1 == 0 ? "0" : si.barcode1.ToString().PadLeft(8, '0');
                //info[7] = si.barcode2 == 0 ? "0" : si.barcode2.ToString().PadLeft(4, '0');
                info[7] = si.weight2.ToString();
                info[8] = si.checkinfo.ToString();
                info[9] = si.weight1.ToString();
                info[10] = si.config.ToString();
            }
            return info;
        }

        public bool WriteStationInfo(string[] str)
        {
            CRLStationInfo si = StationInfo.Find(
                delegate(CRLStationInfo ss)
                {
                    return ss.stationno == int.Parse(str[0]);
                }
            );
            if (si != null)
            {
                byte[] byt = new byte[20];//leo 30 to 20
                for (int i = 1; i < str.Length; i++)
                {
                    Array.Copy(GetHexBytes(str[i]), 0, byt, (i - 1) * 2, 2);
                }
                ////mPLC.Connect();
                //if (!mPLC.Write(50, si.value, 20, byt))//leo 30 to 20  PLCList[csi.key_ip]
                //if (PLCList[si.key_ip].Write(50, si.value, 20, byt))
                //{
                //    mPLC.Connect();
                //    return false;
                //}
                if (si.cFlag)
                    PLCList[si.key_ip].Write(50, si.value, 20, byt);
                ////PLC连不上时临时使用
                //si.taskno = int.Parse(str[1]);
                //si.barcode1 = int.Parse(str[2]) * 65536 + int.Parse(str[3]);
                //si.barcode2 = int.Parse(str[3]);
                //si.goodstype = int.Parse(str[4]);
                //si.fromstation = int.Parse(str[5]);
                //si.tostation = int.Parse(str[6]);
                //si.weight1 = int.Parse(str[7]);
                //si.weight2 = int.Parse(str[8]);
                //si.checkinfo = int.Parse(str[9]);
            }
            return true;
        }
        public bool WriteBits(int station, string value, int index)
        {
            CRLStationInfo si = StationInfo.Find(
                delegate(CRLStationInfo ss)
                {
                    return ss.stationno == station;
                }
            );
            //PLCList["192.168.56.11"].WriteBits(50, si.value + index * 2, 2, GetHexBytes(value));
            PLCList["192.168.56.11"].WriteBits(50, 1417, 1, new byte[]{1});
            return true;
        }
        public bool WriteStationDB554(string[] str)
        {
            byte[] byt = new byte[8];
            for (int i = 0; i < 4; i++)
            {
                Array.Copy(GetHexBytes(str[i]), 0, byt, i * 2, 2);
            }
            PLCList["192.168.56.15"].Write(554, 0, 8, byt);             
            return true;
        }
        public bool WriteStationInfo(int station, string value, int index)
        {
            CRLStationInfo si = StationInfo.Find(
                delegate(CRLStationInfo ss)
                {
                    return ss.stationno == station;
                }
            );
            PLCList["192.168.56.11"].Write(50, si.value + index * 2, 2, GetHexBytes(value));            
            return true;
        }
        private void TimeScan()
        {
            while (true)
            {
                ScanConveryInfo();
                //ScanOutPickInfo();
                //ScanInPickInfo();
                System.Threading.Thread.Sleep(500);
            }
        }
        private void ScanConveryInfo()
        {
            pft.Start();
            if (PLCReLink.Count > 0)
                PLCReLink.Clear();
            foreach (CRLStationInfo csi in StationInfo)
            {
                ConveryBuffer = new byte[csi.length];

                if (PLCList[csi.key_ip].Read(csi.DBAddress, csi.value, csi.length, ref ConveryBuffer))
                {
                    BindStation(csi.stationno, ConveryBuffer);
                    csi.cFlag = true;
                }
                else
                {
                    if (!PLCReLink.Contains(csi.key_ip))
                        PLCReLink.Add(csi.key_ip);
                    csi.cFlag = false;
                }
            }
            if (PLCReLink.Count > 0)
            {
                for (int i = 0; i < PLCReLink.Count; i++)
                {
                    PLCList[PLCReLink[i]].DisConnect();
                    PLCList[PLCReLink[i]].Connect();
                }
            }
            pft.Stop();
            if (GetEquipMentInfo != null)
                GetEquipMentInfo(StationInfo,pft.Duration.ToString());
        }
        public byte[] ReadStationInfoBy51(int stationno)
        {
            CRLOPCItem ci = ItemInfo.Find(
                delegate(CRLOPCItem co)
                {
                    return co.groupname == "RFID" && co.length== stationno;
                }
            );
            if (ci != null)
            {
                byte[] buffer = new byte[32];
                byte[] byt = new byte[32];
               // mPLC.Connect();
                if (PLCList[ci.ipaddress].Read(ci.DBAddress, ci.start, 32, ref buffer))
                {
                    Array.Copy(buffer, 0, byt, 0, 32);
                }
                //else
                //{
                //    mPLC.DisConnect();
                //    mPLC.Connect();
                //}
                return byt;
            }
            return null; 
        }
        public byte[] ReadStationInfoBy90(int stationno)
        {
            CRLOPCItem ci = ItemInfo.Find(
                delegate(CRLOPCItem co)
                {
                    return co.groupname == "RFID" && co.length == stationno;
                }
            );
            if (ci != null)
            {
                byte[] buffer = new byte[962];
                byte[] byt = new byte[962];
                //mPLC.Connect();
                //if (mPLC.Read(ci.DBAddress, ci.start, 962, ref buffer))
                if (PLCList[ci.ipaddress].Read(ci.DBAddress, ci.start, 962, ref buffer))
                {
                    Array.Copy(buffer, 0, byt, 0, 962);
                }
                //else
                //{
                //    mPLC.DisConnect();
                //    mPLC.Connect();
                //}
                return byt;
            }
            return null;
        }
        //public byte[] ReadStationInfoBy556(int i)
        //{
        //    byte[] buffer = new byte[10];
        //    byte[] byt = new byte[10];
        //    //mPLC.Connect();
        //    //if (mPLC.Read(556, i*10, 10, ref buffer))
        //    //{
        //    if (PLCList["192.168.56.15"].Read(556, i*10, 10, ref buffer))
        //    {
        //        Array.Copy(buffer, 0, byt, 0, 10);
        //    }
        //    //else
        //    //{
        //    //    mPLC.DisConnect();
        //    //    mPLC.Connect();
        //    //}
        //    return byt;
        //}
       
        public byte[] GetHexBytes(string info)
        {
            if (info == "")
                info = "0";
            //将一个整数转化为长度为2的byte型数组
            byte[] Buffer = new byte[2];
            int i = int.Parse(info);
            Buffer[0] = (byte)(i / 256);
            Buffer[1] = (byte)(i % 256);
            return Buffer;
        }
    }
}
