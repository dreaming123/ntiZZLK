using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Reflection;

namespace GenSong
{
    public delegate void DNotifyType(string srmno, string srmport, string eNotify, object oNotify);//定义一个消息代理传值给UIL

    public class GenSongSRM : CRNExcuteBase
    {
        private CRLSRMControl OSRM;
        //private CRLConveryControl bll = new CRLConveryControl();
        private string m_strSRMNo, m_strport, m_ip,m_zone;
        private SCBase scb;
        private LOG.Log log;
        private int aisle;
        private int i_TaskNu = 0;
        private Timer CheckTimer = null;
        private Timer GetStatusTimer = null;

        public event DNotifyType ENotifyTypeEventHandler;//传递给UIL层各种消息的事件  

        public delegate void Notify(string type, string msg);
        public event Notify ENotify;

        public Timer CheckConnectTimer
        {
            get
            {
                if (CheckTimer == null)
                {
                    CheckTimer = new Timer();
                }
                return CheckTimer;
            }
        }

        public Timer GetSCStatusTimer
        {
            get
            {
                if (GetStatusTimer == null)
                { GetStatusTimer = new Timer(); }
                return GetStatusTimer;
            }
        }

        public int i_Flag = 4;

        public GenSongSRM(string strsrmno, string ipaddress, int port, int row,string zone)//构造函数
        {
            m_strSRMNo = strsrmno;
            m_strport = port.ToString();
            m_ip = ipaddress;
            m_zone = zone;
            aisle = row * 2;
            scb = new GenSongSC();
            log = new LOG.Log(strsrmno + "-业务逻辑", ".\\堆垛机业务逻辑日志\\");
            OSRM = new CRLSRMControl(m_strSRMNo, ipaddress, port, scb);
            OSRM.EAnlyseData += new CRLSRMControl.AnalyseMessageEventHandler(OSRM_AnalyseMessageEvent);

            CheckConnectTimer.Interval = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["CheckConnectTimer"]);
            CheckConnectTimer.Elapsed += new ElapsedEventHandler(CheckConnectTimer_Elapsed);//若连接失败则检测网络直到连接上了

            //GetSCStatusTimer.Interval = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["GetStatusTime"]);
            //GetSCStatusTimer.Elapsed += new ElapsedEventHandler(GetSCStatusTimer_Elapsed);//获取堆垛机信息Timer事件
        }

        public int Run()//启动
        {
            if (OSRM.Startup())
            {
                CheckConnectTimer.Stop();
                //GetSCStatusTimer.Stop();
                //CRNStatus status = new CRNStatus();
                //status.CRCNum = m_strSRMNo;
                //status.Zone = m_zone;
                //status.AisleNumber = aisle.ToString();
                //CRNList.Add(status);
                ShowText("C", "连接成功，IP地址" + m_ip + ",堆垛机编号" + m_strSRMNo + ",端口" + m_strport);
                //log.WriteLog("********************************************");
                //log.WriteLog("连接成功，IP地址" + m_ip + ",堆垛机编号" + m_strSRMNo + ",端口" + m_strport);
                //log.WriteLog("********************************************");
                return 1;
            }
            else
            {
                CheckConnectTimer.Start();
                //GetSCStatusTimer.Stop();
                return 0;
            }
        }

        private void CheckConnectTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (OSRM.Connection())
            {
                ShowText("C", "Socket连接成功，IP地址" + m_ip + ",堆垛机编号" + m_strSRMNo + ",端口" + m_strport);
                // log.WriteLog("********************************************");
                // log.WriteLog("Socket连接成功，IP地址" + m_ip + ",堆垛机编号" + m_strSRMNo + ",端口" + m_strport);
                // log.WriteLog("********************************************");
                //OSRM.Initialized(m_strSRMNo);
                CheckConnectTimer.Stop();
            }
            else
            {
                CheckConnectTimer.Start();
                //GetSCStatusTimer.Stop();
                log.WriteLog("Socket连接失败，IP地址" + m_ip + ",堆垛机编号" + m_strSRMNo + ",端口" + m_strport);
                ShowText("C", "Socket连接失败，IP地址" + m_ip + ",堆垛机编号" + m_strSRMNo + ",端口" + m_strport);
            }
        }

        private void GetSCStatusTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //if (!OSRM.RequestSRMState())
            if (!OSRM.ReStart(m_strSRMNo))
            {
                CheckConnectTimer.Start();
                //GetSCStatusTimer.Stop();
            }
            //if (i_Flag >= 5)
            //{
            //    if (!OSRM.SRMHeartCheck(m_strSRMNo))//发送心跳
            //    {
            //        ShowText("C", m_strSRMNo + "系统异常，Socket通信连接中断，心跳指令不能发送！");
            //        CheckConnectTimer.Start();
            //        GetSCStatusTimer.Stop();
            //    }
            //    i_Flag = 1;
            //}
            //i_Flag++;
        }

        private void OSRM_AnalyseMessageEvent(NotifyCommand command, string msg)
        {
            CRNObject findcob = CRNOBList.Find(
                delegate(CRNObject info)
                {
                    //return (info.ScNo == m_strSRMNo && info.Port == int.Parse(m_strport) && info.zone == m_zone);
                    return (info.ScNo == m_strSRMNo && info.Port == int.Parse(m_strport) && info.zone == m_zone);
                    //"CRN00" + msg.Substring(41, 1)
                }
            );
            try
            {
                string alarmMsg = "";
                if (command == NotifyCommand.Connect)
                {
                    CheckConnectTimer.Stop();
                    findcob.connectstatus = 1;
                    alarmMsg = crc.UpdateSCStatus(m_strSRMNo, "Connect", m_strSRMNo.Substring(5, 1), m_zone);
                    ShowText("C", msg);
                }
                else if (command == NotifyCommand.RecvData)
                {
                    string mesgs = msg;
                    //if (mesgs.Substring(1, 2) == "LA")
                    //{
                    //    //GetSCStatusTimer.Start();//建立连接结束，开始心跳检测
                    //    mesgs.Substring(0, mesgs.Length);
                    //}
                    //string num = "CRN00" + (int.Parse(msg.Substring(41, 1))+2);//m_strSRMNo;
                    string num = m_strSRMNo;
                    if (mesgs.Substring(1, 2) == "LD")
                    {
                        OSRM.ReturnLDMsg(mesgs.Substring(3, 13));
                    }
                    if (m_strSRMNo == "CRN006")
                        num = "CRN00" + mesgs.Substring(mesgs.Length - 2, 1);
                    alarmMsg = crc.UpdateSCStatus(num, mesgs, m_strSRMNo.Substring(5, 1), m_zone);
                  
                    if (!string.IsNullOrEmpty(alarmMsg))
                    {
                        ShowText("", alarmMsg);
                        if(mesgs.Substring(1, 2) != "LA")
                         log.WriteLog(alarmMsg);
                    }
                }
                else if (command == NotifyCommand.SendData)
                {
                    //ShowText("", msg);
                }
                else if (command == NotifyCommand.Break)
                {
                    CheckConnectTimer.Start();
                    alarmMsg = crc.UpdateSCStatus(m_strSRMNo, "Break", m_strSRMNo.Substring(5, 1),m_zone);
                    findcob.connectstatus = 0;
                    if (!string.IsNullOrEmpty(alarmMsg))
                        ShowText("", alarmMsg);
                }
                if (ENotifyTypeEventHandler != null)
                    ENotifyTypeEventHandler(m_strSRMNo, m_strport, "", msg);
            }
            catch (Exception ex)
            {
                //log.WriteLog("********************************************");
                //log.WriteLog("错误信息：" + ex.Message);
                //log.WriteLog("********************************************");
            }
        }

        public bool SendCode(int type, int taskno, string station, ref string msg)//发送命令
        {
            try
            {
                string[] info = new string[14];
                string barcode = string.Empty;
                //if (station.Length != 4 && station.Length != 11)
                //{
                //    msg = "源地址格式不正确!";
                //    return false;
                //}
                //string[] str = crc.GetCRCStatus(m_strSRMNo);//获取堆垛机状态
                //if (str[13] != "自动模式")
                //{
                //    msg = "堆垛机不为自动模式";
                //    return false;
                //}
                //if (station.Length == 4)//入库
                //{
                //    //info = bll.ReadStationInfo(int.Parse(station));
                //    //if (info != null)
                //    //    barcode = (int.Parse(info[2] == null ? "0" : info[2]) * 65536 + int.Parse(info[3] == null ? "0" : info[3])).ToString();
                //    //string[] ss = dal.GetToLocationNumber(taskno.ToString());
                //    info[4] = ss[0];
                //    info[5] = ss[1].Substring(0, ss[1].Length - 1) + "00" + ss[1].Substring(ss[1].Length - 1, 1);

                //}
                //else//出库
                //{
                //    if (type == int.Parse(CR_GETC_WAIT_H))//取货
                //    {
                //        if (station.Length != 11)
                //        {
                //            msg = "源地址格式错误！";
                //            return false;
                //        }
                //        barcode = dal.GetBarcode(taskno.ToString());
                //        string addr = crc.GetTargetAddr(m_strSRMNo);
                //        info[4] = station.ToString().StartsWith("S") ? station.Substring(0, station.Length - 1) + station.Substring(station.Length - 1, 1).PadLeft(3, '0') : "S" + station.Substring(0, station.Length - 1) + station.Substring(station.Length - 1, 1).PadLeft(3, '0');
                //        info[5] = addr.StartsWith("S") ? addr.Substring(0, addr.Length - 1) + addr.Substring(addr.Length - 1, 1).PadLeft(3, '0') : addr;

                //    }
                //}
                if (info == null || barcode.Length == 0)
                    return false;
                OSRM.RSTaskMsg(m_strSRMNo, GetSRMSourceID(info[4]), GetSRMSourceID(info[5]), "11", taskno.ToString().PadLeft(8, '0'));
                i_TaskNu = taskno;
                log.WriteLog("********************************************");
                log.WriteLog("SendCode发送指令:" + type.ToString() + "；任务号" + taskno.ToString() + "；站台" + station + "成功");
                log.WriteLog("********************************************");
                ShowText("S", "SendCode发送指令:" + type.ToString() + "；任务号" + taskno.ToString() + "；站台" + station + "成功");
                return true;
            }
            catch (Exception ee)
            {
                log.WriteLog("********************************************");
                log.WriteLog("SendCode发送指令异常:" + type.ToString() + "/" + taskno.ToString() + "/" + station + "  " + ee.Message);
                log.WriteLog("********************************************");
                ShowText("S", "SendCode发送指令异常:" + type.ToString() + "/" + taskno.ToString() + "/" + station + "  " + ee.Message);
                return false;
            }
        }

        public bool SendTask(string taskno, string fromstation, string tostation, string statostation, string palletno)
        {
            try
            {
                string msg = string.Empty;
                if (crc.UpdateCRCTaskInfo(m_strSRMNo, fromstation, tostation, statostation, taskno,m_zone))
                {
                    bool sendflag = OSRM.RSTaskMsg(m_strSRMNo, GetSRMSourceID(fromstation), GetSRMSourceID(tostation), "11", taskno.PadLeft(8, '0'));
                    if (sendflag)
                    {
                        log.WriteLog("自动发送任务成功,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation);
                        ShowText("S", "自动发送任务成功,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation);
                        dal.UpdateTaskStatus(taskno, "SC", "2", "1");
                    }
                    else
                    {
                        log.WriteLog("自动发送任务发送失败,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation + "失败原因：" + msg);
                        ShowText("S", "自动发送任务发送失败,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation + "失败原因：" + msg);
                    }
                    return sendflag;
                }
                else
                {
                    log.WriteLog("自动发送任务更新堆垛机状态失败,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation);
                    ShowText("S", "自动发送任务更新堆垛机状态失败,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation);
                    return false;
                }
            }
            catch (Exception ee)
            {
                log.WriteLog("自动发送任务异常:" + taskno + "(" + fromstation + "-->" + tostation + ") " + ee.ToString());
                ShowText("S", "自动发送任务异常:" + taskno + "(" + fromstation + "-->" + tostation + ") " + ee.ToString());
                return false;
            }
        }//+0603
        public bool SendTask(string taskno, string fromstation, string tostation, string sectaskno, string secfromstation, string sectostation, string statostation, string palletno)
        {
            try
            {
                string msg = string.Empty;
                if (crc.UpdateCRCTaskInfo(m_strSRMNo, GetSRMSourceID(fromstation), GetSRMSourceID(tostation), GetSRMSourceIDSec(secfromstation), GetSRMSourceIDSec(sectostation), statostation, taskno, sectaskno, m_zone))
                {
                    string srmtaskno = taskno + sectaskno;
                    //bool sendflag = OSRM.RSTaskMsg(m_strSRMNo, GetSRMSourceID(fromstation), GetSRMSourceID(tostation), "11", taskno.PadLeft(8, '0'));
                    bool sendflag = OSRM.RSTaskMsg(m_strSRMNo, GetSRMSourceID(fromstation), GetSRMSourceID(tostation), GetSRMSourceIDSec(secfromstation), GetSRMSourceIDSec(sectostation), "11", taskno+sectaskno);
                    if (sendflag)
                    {
                        log.WriteLog("自动发送任务成功,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation);
                        ShowText("S", "自动发送任务成功,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation);
                        //dal.UpdateTaskStatus(taskno, "SC", "2", "1");
                        //dal.UpdateTaskStatus(sectaskno, "SC", "2", "1");
                    }
                    else
                    {
                        log.WriteLog("自动发送任务发送失败,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation + "失败原因：" + msg);
                        ShowText("S", "自动发送任务发送失败,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation + "失败原因：" + msg);
                    }
                    return sendflag;
                }
                else
                {
                    log.WriteLog("自动发送任务更新堆垛机状态失败,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation);
                    ShowText("S", "自动发送任务更新堆垛机状态失败,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation);
                    return false;
                }
            }
            catch (Exception ee)
            {
                log.WriteLog("自动发送任务异常:" + taskno + "(" + fromstation + "-->" + tostation + ") " + ee.ToString());
                ShowText("S", "自动发送任务异常:" + taskno + "(" + fromstation + "-->" + tostation + ") " + ee.ToString());
                return false;
            }
        }
        private void ShowText(string type, string msg)
        {
            if (ENotify != null)
                ENotify(type, msg);
        }

        private string GetSRMSourceID(string address)//根据源地址转换成SRM源地址
        {
            string add = "";
            if (address.Length == 4)            
            {
                //return ConveryClass.ReadSRMConfig(int.Parse(address));
                if (address == "5211")
                    add = "02093001";
                //if (address == "5211")
                //    add = "02094001";
                if (address == "5212")
                    add = "04093001";
                //if (address == "5212")
                //    add = "04094001"; 
                if (address == "5215")
                    add = "06093001";
                //if (address == "5215")
                //    add = "06094001";

                if (address == "5145")
                    add = "01093002";
                //if (address == "5345")
                //    add = "02094002";
                if (address == "5144")
                    add = "03093002";
                //if (address == "5344")
                //    add = "04094002";
                if (address == "5141")
                    add = "05093002";
                //if (address == "5341")
                //    add = "06094002";
            }
            else if (address.Length == 10)
            {
                //B0060580071
                //add = int.Parse(address.Substring(2, 2)) % 2 == 0 ? "02" : "01";
                //add += address.Substring(4, 6) + "1";
                add = address.Substring(1, 8);
            }
            else
            {
                add = address;
            }
            return add;
        }

        private string GetSRMSourceIDSec(string address)//根据源地址转换成SRM源地址
        {
            string add = "";
            if (address.Length == 4)
            {
                //return ConveryClass.ReadSRMConfig(int.Parse(address));
                if (address == "5211")
                    add = "02094001";
                //if (address == "5211")
                //    add = "02094001";
                if (address == "5212")
                    add = "04094001";
                //if (address == "5212")
                //    add = "04094001";
                if (address == "5215")
                    add = "06094001";
                //if (address == "5215")
                //    add = "06094001";

                if (address == "5145")
                    add = "01094002";
                //if (address == "5345")
                //    add = "02094002";
                if (address == "5144")
                    add = "03094002";
                //if (address == "5344")
                //    add = "04094002";
                if (address == "5141")
                    add = "05094002";
                //if (address == "5341")
                //    add = "06094002";
            }
            else if (address.Length == 10)
            {
                //B0060580071
                //add = int.Parse(address.Substring(2, 2)) % 2 == 0 ? "02" : "01";
                //add += address.Substring(4, 6) + "1";
                add = address.Substring(1, 8);
            }
            else
            {
                add = address;
            }
            return add;
        }

        private string ConvertToPalletNu(string num)
        {
            if (num.Length < 6)
            {
                return int.Parse(num).ToString("D6");//需要改
            }
            else
            {
                return num.Substring(0, 6);
            }

        }

        public bool SendCommand(int type, string taskno, string fromstation, string tostation)
        {
            bool flag = false;
            switch (type)
            {
                case 1: if (crc.UpdateCRCTaskInfo(m_strSRMNo, fromstation, tostation, "0", taskno,m_zone)) { OSRM.RSTaskMsg(m_strSRMNo, GetSRMSourceID(fromstation), GetSRMSourceID(tostation), "01", taskno.PadLeft(8, '0')); flag = true; } break;
                case 2: if (crc.UpdateCRCTaskInfo(m_strSRMNo, fromstation, tostation, "0", taskno,m_zone)) { OSRM.RSTaskMsg(m_strSRMNo, GetSRMSourceID(fromstation), GetSRMSourceID(tostation), "02", taskno.PadLeft(8, '0')); flag = true; } break;
                case 3: if (crc.UpdateCRCTaskInfo(m_strSRMNo, fromstation, tostation, "0", taskno,m_zone)) { OSRM.RSTaskMsg(m_strSRMNo, GetSRMSourceID(fromstation), GetSRMSourceID(tostation), "00", taskno.PadLeft(8, '0')); flag = true; } break;
                case 4: OSRM.TaskEStop(); flag = true; break;
                case 5: OSRM.TaskCancelEStop(); flag = true; break;
                case 6: if (crc.UpdateCRCTaskInfo(m_strSRMNo, fromstation, tostation, "0", taskno, m_zone)) { OSRM.RSTaskMsg(m_strSRMNo, GetSRMSourceID(fromstation), GetSRMSourceID(tostation), "11", taskno.PadLeft(8, '0')); flag = true; } break;
                case 7: OSRM.SCBackOrigin(); flag = true; break;
                default: break;
            }
            return flag;
        }
        public bool SendCommand(string num,int type, string taskno, string fromstation, string tostation,string secfromstation, string sectostation)
        {
            bool flag = false;

            return OSRM.RSTaskMsg(num, GetSRMSourceID(fromstation), GetSRMSourceID(tostation), GetSRMSourceIDSec(secfromstation), GetSRMSourceIDSec(sectostation), "11", taskno);
            //switch (type)
            //{
            //    case 1: if (crc.UpdateCRCTaskInfo(m_strSRMNo, fromstation, tostation, secfromstation, sectostation, "0", taskno, m_zone)) { OSRM.RSTaskMsg(m_strSRMNo, GetSRMSourceID(fromstation), GetSRMSourceID(tostation), "01", taskno.PadLeft(8, '0')); flag = true; } break;
            //    case 2: if (crc.UpdateCRCTaskInfo(m_strSRMNo, fromstation, tostation, secfromstation, sectostation, "0", taskno, m_zone)) { OSRM.RSTaskMsg(m_strSRMNo, GetSRMSourceID(fromstation), GetSRMSourceID(tostation), "02", taskno.PadLeft(8, '0')); flag = true; } break;
            //    case 3: if (crc.UpdateCRCTaskInfo(m_strSRMNo, fromstation, tostation, secfromstation, sectostation, "0", taskno, m_zone)) { OSRM.RSTaskMsg(m_strSRMNo, GetSRMSourceID(fromstation), GetSRMSourceID(tostation), "00", taskno.PadLeft(8, '0')); flag = true; } break;
            //    case 4: OSRM.TaskEStop(); flag = true; break;
            //    case 5: OSRM.TaskCancelEStop(); flag = true; break;
            //    case 6: if (crc.UpdateCRCTaskInfo(m_strSRMNo, fromstation, tostation, secfromstation, sectostation, "0", taskno, m_zone)) { OSRM.RSTaskMsg(m_strSRMNo, GetSRMSourceID(fromstation), GetSRMSourceID(tostation), "11", taskno.PadLeft(8, '0')); flag = true; } break;
            //    case 7: OSRM.SCBackOrigin(); flag = true; break;
            //    default: break;
            //}
            //return flag;
        }

    }
}
