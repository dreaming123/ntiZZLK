using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.IO;
using COMResolution;
using System.Runtime.Remoting;

namespace BLL
{
    public delegate void DNotifyCRCB(List<CRCObjectB> cob, List<CRCStatusB> cs, string eNotify, object oNotify);//定义一个消息代理传值给UIL

    public class BLLSRMRunB : BLLBase
    {
        public event DNotifyCRCB ENotifyCRCEventHandler;//传递给UIL层各种消息的事件  
        public delegate void DNotify(string type, string msg);
        public event DNotify EDNotify;

        public delegate void AsyncDelegate();
 
        public void Start(string zone)
        {
            DataSet ds = dal.GetEquipmentInfoB(zone);
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["equipmenttype"].ToString() == "SC" && dt.Rows[i]["equipmentfactory"].ToString() == "DAMBACH")
                        {
                            AddSRM(dt.Rows[i]["equipmentname"].ToString(), dt.Rows[i]["equipmentid"].ToString(), int.Parse(dt.Rows[i]["equipmentport"].ToString()), int.Parse(dt.Rows[i]["equipmentaisle"].ToString()));
                        }
                    }
                    foreach (CRCObjectB co in crclistB)
                    {
                        co.BllSrm.ENotifyTypeEventHandler += new DNotifyTypeB(BllSrm_ENotifyTypeEventHandler);
                        co.BllSrm.ENotify+=new BLLSRMB.Notify(BllSrm_ENotify);
                        co.connectstatus = co.BllSrm.Run();
                        System.Threading.Thread.Sleep(100);
                    }
                    if (crclist.Count > 0)
                    {
                        TaskThread = new Thread(new ThreadStart(TimeScan));
                        TaskThread.Start();
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
        }

        public void CloseThread()
        {
            if (TaskThread.ThreadState != ThreadState.Aborted)
                TaskThread.Abort();
        }

        public void ReStartThread()
        {
            if (TaskThread.ThreadState != ThreadState.Running)
            {
                TaskThread = new Thread(new ThreadStart(TimeScan));
                TaskThread.Start();
            }
        }

        private void TimeScan()
        {
            while (true)
            {
                DataTable dt = crcb.GetTask();
                foreach (DataRow row in dt.Rows)
                {
                    string equipmentnumber = row["num"].ToString();
                    if (equipmentnumber.Length == 1)
                    {
                        equipmentnumber = "CRC00" + equipmentnumber;
                    }
                    else
                    {
                        equipmentnumber = "CRC0" + equipmentnumber;
                    }
                    CRCObjectB co = crclistB.Find(
                       delegate(CRCObjectB info)
                       {
                           return (info.ScNo == equipmentnumber && info.Port == 2000);
                           //return (info.ScNo == row["num"].ToString() && info.Port == 2000);
                       }
                   );
                    CRCStatusB cs = CRCItemsB.Find(
                      delegate(CRCStatusB info)
                      {
                          return (info.CRCNum == equipmentnumber);
                          //return (info.CRCNum == row["num"].ToString());
                      }
                  );
                    if (co != null && cs.FunctionReport == CR_NO_FUNC.ToString())
                    {
                        //string[] info_4099 = ReadStationInfo(4099);
                        //string[] info_4100 = ReadStationInfo(4100);
                        //if ((row[2].ToString() == "4078" && info_4099[1] == "1" && info_4099[2] == "1") || (row[3].ToString() == "4057" && info_4100[1] == "2" && info_4100[2] == "2") || (row[3].ToString() == "4078" && info_4099[2] == "2" && info_4099[1] == "2") || (row[2].ToString() == "4057" && info_4100[2] == "1" && info_4100[1] == "1") || row[2].ToString().Length == 6 && row[2].ToString().Length == 6)
                        //co.BllSrm.SendTask(row["num"].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[6].ToString(), row[7].ToString());
                        co.BllSrm.SendTask(row["num"].ToString(), row[1].ToString(), row[2].ToString().Replace("A", "").Replace("B", ""), row[3].ToString().Replace("A", "").Replace("B", ""), row[4].ToString(), row[6].ToString());

                    }
                }
                //AsyncDelegate dlgt = ScanOutCommand;
                //dlgt.BeginInvoke(null, null);
                //blc.GetOutLanewayGono();                ////blc.GetOutBoundTask(); //blc.TimeScanOutLanway();
                Thread.Sleep(2000);                //Thread.Sleep(5000);
            }
        }
        private void ScanOutCommand()
        {
            //blc.GetOutLanewayGono();
            blc.GetOutBoundTask();
            //blc.TimeScanOutLanway();
        }
        public bool SendTask(string num, string taskno, string fromstation, string tostation, int type)
        {
            CRCObjectB co = crclistB.Find(
                delegate(CRCObjectB info)
                {
                    return (info.ScNo == num && info.Port == 2000);
                }
            );
            if (co != null)
            {
                if (type == 1)
                    //return co.BllSrm.SendTask(num, taskno, fromstation, tostation, "0", "", "");
                    return co.BllSrm.SendTask(num, taskno, fromstation, tostation, "0", "");
                    //return false;
                else
                    if (fromstation.Length == 4)
                    {
                        string codeh = fromstation.Substring(0, 1);
                        string code = fromstation.Substring(1, 3);
                        return co.BllSrm.SendCode(num, int.Parse(code), int.Parse(codeh), int.Parse(taskno), tostation);
                    }
                    else
                    {
                        return co.BllSrm.SendCode(num, int.Parse(fromstation), 0, int.Parse(taskno), tostation);
                    }
            }
            return false;
        }

        private void AddSRM(string scno, string ipaddress, int port, int aisle)
        {
            CRCObjectB newcrc = new CRCObjectB();
            BLLSRMB srm = new BLLSRMB(scno, ipaddress, port, aisle);
            newcrc.BllSrm = srm;
            newcrc.ScNo = scno;
            newcrc.IpAddress = ipaddress;
            newcrc.Port = port;
            crclistB.Add(newcrc);
        }

        private void BllSrm_ENotifyTypeEventHandler(string srmno, string port, string eNotify, object oNotify)
        {
            if (ENotifyCRCEventHandler != null)
                ENotifyCRCEventHandler(crclistB, CRCItemsB, eNotify, oNotify);
        }

        private void BllSrm_ENotify(string type, string msg)
        {
            if (EDNotify != null)
                EDNotify(type, msg);
        }
    }
}
