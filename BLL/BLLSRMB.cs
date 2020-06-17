using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Timers;
using COMResolution;

namespace BLL
{
    public delegate void DNotifyTypeB(string srmno, string srmport, string eNotify, object oNotify);//定义一个消息代理传值给UIL

    public class BLLSRMB : BLLBase
    {
        private CRLSRMControlB OSRM;   //构造函数中new

        private string m_strSRMNo, m_strport,m_ip;
        private LOG.Log log;
        private int aisle;

        private Timer CheckTimer = null;
        private Timer GetStatusTimer = null;

        public event DNotifyTypeB ENotifyTypeEventHandler;//传递给UIL层各种消息的事件  

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

        public BLLSRMB(string strsrmno, string ipaddress, int port, int row)//构造函数
        {
            m_strSRMNo = strsrmno;
            m_strport = port.ToString();
            m_ip = ipaddress;
            aisle = row * 2;
            log = new LOG.Log("业务逻辑" + strsrmno, ".\\卫星小车日志\\");
            OSRM = new CRLSRMControlB(m_strSRMNo, ipaddress, port);

            CheckConnectTimer.Interval = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["CheckConnectTimer"]);
            CheckConnectTimer.Elapsed += new ElapsedEventHandler(CheckConnectTimer_Elapsed);//若连接失败则检测网络直到连接上了
            GetSCStatusTimer.Interval = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["GetStatusTime"]);
            GetSCStatusTimer.Elapsed += new ElapsedEventHandler(GetSCStatusTimer_Elapsed);//获取卫星小车信息Timer事件
            OSRM.AnalyseMessageEvent += new CRLSRMControlB.AnalyseMessageEventHandler(OSRM_AnalyseMessageEvent);
        }

        public int Run()//启动
        {
            if (OSRM.Startup())
            {
                if (m_strport == "4000")
                //if (m_strport == "80")
                {
                    CRCStatusB status = new CRCStatusB();
                    status.CRCNum = m_strSRMNo;
                    status.AisleNumber = aisle.ToString();
                    CRCItemsB.Add(status);
                }
                GetSCStatusTimer.Start();
                ShowText("C", "连接成功,IP地址" + m_ip + ",堆垛机编号" + m_strSRMNo + ",端口" + m_strport);
                return 1;
            }
            else
            {
                CheckConnectTimer.Start();
                ShowText("C", "连接失败,IP地址" + m_ip + ",堆垛机编号" + m_strSRMNo + ",端口" + m_strport);
                return 0;
            }
        }

        private void OSRM_AnalyseMessageEvent(string CommandType, object msg)
        {
            CRCObjectB findcob = crclistB.Find(
                delegate(CRCObjectB info)
                {
                    return (info.ScNo == m_strSRMNo && info.Port == int.Parse(m_strport));
                }
            );
            if (m_strport == "4000" && CommandType == "RecvData")
            //if (m_strport == "80" && CommandType == "RecvData")
            {
                if (findcob != null)
                    findcob.connectstatus = (findcob.connectstatus == 1 ? 2 : 1);
                crcb.UpdateCRCStatus(m_strSRMNo, msg.ToString(), aisle.ToString());
                if (!string.IsNullOrEmpty(crcb.GetAlarmInfo(m_strSRMNo, "", false)))
                    log.WriteLog(crcb.GetAlarmInfo(m_strSRMNo, "", false));
                if (ENotifyTypeEventHandler != null)
                    ENotifyTypeEventHandler(m_strSRMNo, m_strport, CommandType, msg);
            }
            else if (m_strport == "6000" && CommandType == "RecvData")
            {
                crcb.GetAlarmInfo(m_strSRMNo, msg.ToString(), true);
            }
            else if (CommandType == "Break")
            {
                if (findcob != null)
                    findcob.connectstatus = 0;
                CheckConnectTimer.Start();
                GetSCStatusTimer.Stop();
                dal.UpdateSCStatusNTI(m_strSRMNo, "Equipment_Error");
                crcb.UpdateCRCStatus(m_strSRMNo, CommandType, aisle.ToString());
                //ShowText("C", "连接中断,IP地址" + m_ip + ",堆垛机编号" + m_strSRMNo + ",端口" + m_strport);
            }
            else if (CommandType == "Connect")
            {
                if (findcob != null)
                    findcob.connectstatus = 1;
                CheckConnectTimer.Stop();
                GetSCStatusTimer.Start();
                ShowText("C", "连接成功,IP地址" + m_ip + ",堆垛机编号" + m_strSRMNo + ",端口" + m_strport);
            }
        }

        private void CheckConnectTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (OSRM.Startup())
            {
                GetSCStatusTimer.Start();
                CheckConnectTimer.Stop();
                ShowText("C", "检测连接成功,IP地址" + m_ip + ",堆垛机编号" + m_strSRMNo + ",端口" + m_strport);
            }
            else
                ShowText("C", "检测连接失败,IP地址" + m_ip + ",堆垛机编号" + m_strSRMNo + ",端口" + m_strport);
        }

        private void GetSCStatusTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (m_strport == "4000")
            //if (m_strport == "80")
            {
                if (!OSRM.SendMsg(MessageHeader_4000))
                {
                    CheckConnectTimer.Start();
                    GetSCStatusTimer.Stop();
                }
            }
            else if (m_strport == "6000")
            {
                if (m_strSRMNo == "HC001")
                {
                    if (!OSRM.SendMsg(MessageHeaderHC_6000))
                    {
                        CheckConnectTimer.Start();
                        GetSCStatusTimer.Stop();
                    }
                }
                else
                {
                    if (!OSRM.SendMsg(MessageHeader_6000))
                    {
                        CheckConnectTimer.Start();
                        GetSCStatusTimer.Stop();
                    }
                }
            }
        }
        public bool SendCode(string num, int type, int typeh, int taskno, string station)
        {
            try
            {
                if (station != null && station.Length != 4 && station.Length != 10)
                    return false;
                byte[] msg = new byte[40];
                Array.Copy(MessageHeader_2000, 0, msg, 0, 16);
                int taskno1 = taskno / 256;
                int taskno2 = taskno % 256;
                int actionpoint = 1;
                int aisle1 = 1;
                msg[16] = 0x01;
                msg[17] = 0x01;//messagecount
                msg[18] = 0x00;
                msg[19] = 0x00;
                msg[20] = (byte)taskno1;
                msg[21] = (byte)taskno2;
                msg[22] = (byte)type;//GETC: 112   PUTC: 128   NOFUNC: 0
                msg[23] = 0x01;
                msg[24] = 0x00;
                msg[25] = 0x01;
                if (station != null && station.Length == 4)
                {
                    //string[] str = ReadStationInfo(int.Parse(station));
                    string[] str = ReadStationInfo(int.Parse(station));
                    actionpoint = int.Parse(str[10]);
                    for (int i = 27; i < 40; i++)
                        msg[i] = 0x00;
                }
                else
                {
                    msg[27] = (byte)((int.Parse(station.Substring(0, 3)) - 1) / 2 + 1);//巷道
                    msg[28] = (byte)(int.Parse(station.Substring(0, 3)) % 2 + 1);// 1,left;2,right                    
                    msg[29] = (byte)((int.Parse(station.Substring(3, 3)) + 1) / 3 + 1);//XF,列                    
                    msg[30] = (byte)((int.Parse(station.Substring(3, 3)) % 3) == 1 ? 3 : (int.Parse(station.Substring(3, 3)) % 3) == 2 ? 1 : 2); ;//列中的的位置
                    if (int.Parse(station.Substring(3, 3)) == 1)
                    {
                        msg[29] = (byte)1;
                        msg[30] = (byte)1;
                    }
                    msg[31] = (byte)(int.Parse(station.Substring(6, 3)));//层,Z
                    msg[32] = (byte)(station.Substring(9, 1) == "1" ? 1 : 2);//深度,Y  

                    for (int i = 33; i < 40; i++)
                        msg[i] = 0x00;
                }
                msg[26] = (byte)actionpoint;
                msg[37] = 0x01;//0x01:100%  0x02:75% 0x03:50%
                //msg[38] = (byte)typeh;
                //msg[39] = 0x00;
                bool sendflag = OSRM.SendMsg(msg);
                if (sendflag)
                {
                    log.WriteLog(num + "SendCode发送指令成功:" + type.ToString() + "/" + taskno.ToString() + "/" + station);
                    ShowText("S", num + "SendCode发送指令成功:" + type.ToString() + "/" + taskno.ToString() + "/" + station);
                }
                else
                {
                    log.WriteLog(num + "SendCode发送指令失败:" + type.ToString() + "/" + taskno.ToString() + "/" + station);
                    ShowText("S", num + "SendCode发送指令失败:" + type.ToString() + "/" + taskno.ToString() + "/" + station);
                }
                return sendflag;
            }
            catch (Exception ee)
            {
                log.WriteLog(num + "SendCode发送指令异常:" + type.ToString() + "/" + taskno.ToString() + "/" + station + "  " + ee.Message);
                ShowText("S", num + "SendCode发送指令异常:" + type.ToString() + "/" + taskno.ToString() + "/" + station + "  " + ee.Message);
                return false;
            }
        }
        public bool SendCode(string num, int type, int taskno, string station)
        {
            try
            {
                if (station != null && station.Length != 4 && station.Length != 10)
                    return false;
                byte[] msg = new byte[44];
                Array.Copy(MessageHeader_2000, 0, msg, 0, 16);
                int taskno1 = taskno / 256;
                int taskno2 = taskno % 256;
                int actionpoint = 1;
                int aisle1 = 1;
                msg[16] = 0x01;
                msg[17] = 0x01;//messagecount
                msg[18] = 0x00;
                msg[19] = 0x00;
                msg[20] = (byte)taskno1;
                msg[21] = (byte)taskno2;
                msg[22] = (byte)type;//GETC: 112   PUTC: 128   NOFUNC: 0
                msg[23] = 0x01;
                msg[24] = 0x00;
                msg[25] = 0x01;  
                if (station != null && station.Length == 4)
                {
                    #region
                    if (int.Parse(station) == 5041)
                    {
                        actionpoint = 2;
                    }
                    if (int.Parse(station) == 5042)
                    {
                        actionpoint = 4;
                    }
                    if (int.Parse(station) == 5068)
                    {
                        actionpoint = 6;
                    }
                    if (int.Parse(station) == 5065)
                    {
                        actionpoint = 8;
                    }      
                    #endregion
                    for (int i = 27; i < 44; i++)
                        msg[i] = 0x00;
                }
                else
                {
                    if (num == "CRC001")
                    {
                        aisle1 = 1;
                    }
                    if (num == "CRC002")
                    {
                        aisle1 = 2;
                    }
                    msg[27] = (byte)(aisle1 / 2 + 1);//2巷道
                    msg[28] = (byte)((aisle1 % 2) == 1 ? 2 : 1);// 1,left;2,right
                    msg[29] = (byte)(int.Parse(station == null ? "0" : station.Substring(1, 2)));//XF,行
                    msg[30] = (byte)(1);//列中的的位置
                    msg[31] = (byte)(int.Parse(station == null ? "0" : station.Substring(0, 1)));//层,Z
                    msg[32] = (byte)getRackDepth((int.Parse(station == null ? "0" : station.Substring(3, 2))), num);//深度,Y轴
                    for (int i = 33; i < 40; i++)
                        msg[i] = 0x00;
                }
                msg[26] = (byte)actionpoint;
                bool sendflag = OSRM.SendMsg(msg);
                if (sendflag)
                {
                    log.WriteLog(num + "SendCode发送指令成功:" + type.ToString() + "/" + taskno.ToString() + "/" + station);
                    ShowText("S", num + "SendCode发送指令成功:" + type.ToString() + "/" + taskno.ToString() + "/" + station);
                }
                else
                {
                    log.WriteLog(num + "SendCode发送指令失败:" + type.ToString() + "/" + taskno.ToString() + "/" + station);
                    ShowText("S", num + "SendCode发送指令失败:" + type.ToString() + "/" + taskno.ToString() + "/" + station);
                }
                return sendflag;
            }
            catch (Exception ee)
            {
                log.WriteLog(num + "SendCode发送指令异常:" + type.ToString() + "/" + taskno.ToString() + "/" + station + "  " + ee.Message);
                ShowText("S", num + "SendCode发送指令异常:" + type.ToString() + "/" + taskno.ToString() + "/" + station + "  " + ee.Message);
                return false;
            }
        }
        //public bool SendTask(string num,string taskno, string fromstation, string tostation, string statostation,string tasktype,string field3)
        public bool SendTask(string num,string taskno, string fromstation, string tostation, string statostation,string tasktype)
        {
            try
            {
                //if (crc.UpdateCRCTaskInfo(m_strSRMNo, fromstation, tostation, statostation, tasktype, field3))
                if (crcb.UpdateCRCTaskInfo(m_strSRMNo, fromstation, tostation, statostation,tasktype))
                {
                    //SendCode(num,CR_NO_FUNC, int.Parse(taskno), 0, fromstation);
                    bool sendflag = SendCode(num,CR_GETC, 0, int.Parse(taskno), fromstation);
                    if (sendflag)
                    {
                        log.WriteLog(num + "SendCode发送任务成功,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation);
                        ShowText("S", num + "SendCode发送任务成功,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation);
                    }
                    else
                    {
                        log.WriteLog(num + "SendCode发送任务发送失败,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation);
                        ShowText("S", num + "SendCode发送任务发送失败,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation);
                    }
                    return sendflag;
                }
                else
                {
                    log.WriteLog(num + "SendCode发送任务更新失败,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation);
                    ShowText("S", num + "SendCode发送任务更新失败,任务号:" + taskno.ToString() + "  起止:" + fromstation + "-->" + tostation);
                    return false;
                }
            }
            catch (Exception ee)
            {
                log.WriteLog(num + "SendTask发送任务异常:" + taskno + "(" + fromstation + "-->" + tostation + ") " + ee.ToString());
                ShowText("S", num + "SendTask发送任务异常:" + taskno + "(" + fromstation + "-->" + tostation + ") " + ee.ToString());
                return false;
            }
        }

        private void ShowText(string type,string msg)
        {
            if (ENotify != null)
                ENotify(type, msg);
        }
        private int getRackDepth(int sourceDepth, string num)
        {
            if (num == "CRC002")
                return sourceDepth;
            return (13 - sourceDepth);

        }
    }
}
