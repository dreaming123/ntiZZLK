using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.IO;

namespace GenSong
{
    public delegate void DNotifyCRC(List<CRNObject> cob, List<CRNStatus> cs, string eNotify, object oNotify);//定义一个消息代理传值给UIL

    public class GenSongSRMRun : CRNExcuteBase
    {
        public event DNotifyCRC ENotifyCRCEventHandler;//传递给UIL层各种消息的事件  
        public delegate void DNotify(string type, string msg);
        public event DNotify EDNotify;
        public static CRNTask crc = new CRNTask();

        public void Start(string zone)
        {
            DataSet ds = dal.GetEquipmentInfo(plcNumber,zone);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //初始化Gensong堆垛机列表
                    if (dt.Rows[i]["equipmenttype"].ToString() == "SC" && dt.Rows[i]["equipmentfactory"].ToString() == "GenSong")
                    {
                        AddSRM(dt.Rows[i]["equipmentname"].ToString(), dt.Rows[i]["equipmentid"].ToString(), int.Parse(dt.Rows[i]["equipmentport"].ToString()), int.Parse(dt.Rows[i]["equipmentaisle"].ToString()), dt.Rows[i]["equipmentfield3"].ToString(), dt.Rows[i]["equipmentfield4"].ToString());
                    }
                }
                foreach (CRNObject co in CRNOBList)
                {
                    ((GenSongSRM)co.BllSrm).ENotifyTypeEventHandler += new DNotifyType(BllSrm_ENotifyTypeEventHandler);
                    ((GenSongSRM)co.BllSrm).ENotify += new GenSongSRM.Notify(BllSrm_ENotify);
                    co.connectstatus = ((GenSongSRM)co.BllSrm).Run();
                    System.Threading.Thread.Sleep(5000);//miniload sleep
                }
                if (CRNOBList.Count > 0)
                {
                    //TaskThread = new Thread(new ThreadStart(TimeScan));
                    TaskThread = new Thread(new ParameterizedThreadStart(TimeScan));
                    TaskThread.IsBackground = true;
                    //TaskThread.Start();
                    TaskThread.Start(zone);
                }
                if (File.Exists(".\\SCAlarm.txt"))
                {
                    FileStream fs = new FileStream(".\\SCAlarm.txt", FileMode.Open, FileAccess.Read);
                    StreamReader sr = new StreamReader(fs);
                    string strline = sr.ReadToEnd();
                    string[] msg = strline.Split(new char[] { ';' });
                    string[] msg2;
                    for (int i = 0; i < msg.Length; i++)
                    {
                        msg2 = msg[i].Split(new char[] { ',' });
                        if (!AlarmHash.Contains(msg2[0]) && msg2.Length > 1)
                            AlarmHash.Add(msg2[0], msg2[1]);
                    }
                }
            }
        }

        public void CloseThread()
        {
            if (TaskThread.ThreadState != ThreadState.Aborted)
                TaskThread.Abort();
        }

        public void ReStartThread(string zone)
        {
            if (TaskThread.ThreadState != ThreadState.Running)
            {
                //TaskThread = new Thread(new ThreadStart(TimeScan));
                TaskThread = new Thread(new ParameterizedThreadStart(TimeScan));
                TaskThread.IsBackground = true;
                TaskThread.Start(zone);
            }
        }

        private void TimeScan(object parm)
        {
            while (true)
            {
                try
                {
                    string zone = parm.ToString();
                    DataTable dt = crc.GetTask(zone);//获取SC任务
                    if (dt != null && dt.Rows.Count>0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            CRNObject co = CRNOBList.Find(
                                delegate(CRNObject info)
                                {
                                    return (info.ScNo == "CRN00" + row["num"].ToString().Substring(5, 1) && info.zone == zone);
                                }
                            );
                            if (co != null)
                            {
                                //((GenSongSRM)co.BllSrm).SendTask(row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString());
                                ((GenSongSRM)co.BllSrm).SendTask(row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7].ToString(), row[8].ToString());
                            }
                        }                        
                    }
                    Thread.Sleep(1000);
                }
                catch
                {
                }
            }
        }

        public bool SendTask(string num, string taskno, string fromstation, string tostation, int type,string zone)
        {
            string msg = "";
            CRNObject co = CRNOBList.Find(
                delegate(CRNObject info)
                {
                    return info.ScNo == num && info.zone == zone;
                }
            );
            if (co != null)
            {
                if (type != 0)
                    return ((GenSongSRM)co.BllSrm).SendCommand(type, taskno, fromstation, tostation);
                else
                    return ((GenSongSRM)co.BllSrm).SendCode(int.Parse(fromstation), int.Parse(taskno), tostation, ref msg);
            }
            return false;
        }
        public bool SendTask(string num, string taskno, string fromstation, string tostation, string secfromstation, string sectostation, int type, string zone)
        {
            string msg = "";
            CRNObject co = CRNOBList.Find(
                delegate(CRNObject info)
                {
                    return info.ScNo == num && info.zone == zone;
                }
            );
            if (co != null)
            {
                if (type != 0)
                    return ((GenSongSRM)co.BllSrm).SendCommand(num,type, taskno, fromstation, tostation,secfromstation,sectostation);
                else
                    return ((GenSongSRM)co.BllSrm).SendCode(int.Parse(fromstation), int.Parse(taskno), tostation, ref msg);
            }
            return false;
        }
        private void AddSRM(string scno, string ipaddress, int port, int aisle, string equipmentid,string zone)
        {            
            CRNObject newcrc = new CRNObject();
            GenSongSRM srm = new GenSongSRM(scno, ipaddress, port, aisle, zone);
            newcrc.BllSrm = srm;
            newcrc.ScNo = scno;
            newcrc.zone = zone;
            newcrc.IpAddress = ipaddress;
            newcrc.Port = port;
            CRNOBList.Add(newcrc);
        }

        private void BllSrm_ENotifyTypeEventHandler(string srmno, string port, string eNotify, object oNotify)
        {
            if (ENotifyCRCEventHandler != null)
                ENotifyCRCEventHandler(CRNOBList, CRNList, eNotify, oNotify);
        }

        private void BllSrm_ENotify(string type, string msg)
        {
            if (EDNotify != null)
                EDNotify(type, msg);
        }

    }
}
