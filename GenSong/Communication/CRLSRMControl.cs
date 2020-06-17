using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenSong
{
    public class CRLSRMControl : CRNExcuteBase
    {
        private RWLSocketClient OSrmSocket = new RWLSocketClient();


        public delegate void AnalyseMessageEventHandler(NotifyCommand command, string mesgs);//声明AnysisData解析收到数据时事件的代理，传给BLL层

        public event AnalyseMessageEventHandler EAnlyseData;//解析收到数据并传给BLL


        private string m_serverIP, m_strSRMNo, m_MessageRecv;//堆垛机IP，字段名
        private int m_serverPort;
        private SCBase scb;

        private readonly static object obj_Lock = new object();
        private int g_nDatagramCounter;//发送报文统计
        private int g_nDatagramSequenceNumber;//发送报文顺序号

        private int g_nRTM_TMSequenceNumber;//发送任务指令时，记住报文顺序号
        private string g_nTaskNo;//发送任务指令时，记住任务号
        private LOG.Log log = null;

        public CRLSRMControl(string strsrmno, string ipaddress, int port, SCBase scbs)//构造函数 
        {
            m_serverIP = ipaddress;
            m_serverPort = port;
            m_strSRMNo = strsrmno;
            scb = scbs;
            log = new LOG.Log("--" + m_strSRMNo + "~" + port.ToString(), ".\\堆垛机报文日志\\");
            try
            {
                //订阅一个Socket收到数据的事件
                OSrmSocket.Notify += new NotifyCommandHander(OSrmSocket_Notify);

            }
            catch (Exception ex)
            {
                //log.WriteLog(m_strSRMNo + ": SRM_Notify初始化函数异常" + ex.Message);
            }
        }

        public void Initialized(string SRMNo)//初始化操作 根据协议初始化流程，通信中断后重启也要启用初始化操作    
        {
            g_nDatagramCounter = -1;//报文统计从0条开始
            g_nDatagramSequenceNumber = -1;//初始化第一条报文
            ReStart(SRMNo);  //1启动设置
        }

        public bool Connection() //建立Socket连接
        {
            try
            {
                if (OSrmSocket.Connected)//如果连接上，先关闭，然后再连接
                    OSrmSocket.Close();
                //if (OSrmSocket.Connect(m_serverIP, m_serverPort))
                if (OSrmSocket.Connect("192.168.56.33", 3000))
                {
                    return true;
                }
                else
                {
                    //OnEAnlyseData(NotifyCommand.Break, m_strSRMNo + " SCS Socket连接失败!");
                    return false;
                }

            }
            catch (Exception ex)
            {
                OnEAnlyseData(NotifyCommand.Break, m_strSRMNo + " SCS Socket连接异常" + ex.Message);
                return false;
            }
        }

        public bool DisConnect()//关闭Socket连接
        {
            if (OSrmSocket != null)
            {
                OSrmSocket.Close();
                //OnEAnlyseData(NotifyCommand.Break, m_strSRMNo + " WCS Socket连接关闭!");
                return true;
            }
            else
            {
                //OnEAnlyseData(NotifyCommand.Break, m_strSRMNo + " WCS Socket连接关闭失败!");
                return false;
            }
        }

        public bool Startup()//启动SRM通信程序
        {
            if (this.Connection())
            {
                this.Initialized(m_strSRMNo);
                return true;
            }
            else
            {
                //OnEAnlyseData(NotifyCommand.Break, m_strSRMNo + "Socket中断，连接失败，Startup失败！");
                return false;
            }
        }

        private void OSrmSocket_Notify(NotifyCommand Command, object msg)//信息处理
        {
            //log.WriteLog("********************************************");
            //log.WriteLog("SCS->WCS:" + msg.ToString());
            //log.WriteLog("********************************************");
            msg = (object)msg.ToString().Replace("\0", "").Trim();
            switch (Command)
            {
                case NotifyCommand.Break:
                    this.DisConnect();
                    OnEAnlyseData(NotifyCommand.Break, m_strSRMNo + msg.ToString());
                    break;
                case NotifyCommand.Connect:
                    OnEAnlyseData(NotifyCommand.Connect, m_strSRMNo + msg.ToString());
                    break;
                case NotifyCommand.RecvData:
                    m_MessageRecv = msg.ToString();
                    AnalyseMessage(m_MessageRecv);
                    break;
                case NotifyCommand.SendData:
                    OnEAnlyseData(NotifyCommand.SendData, msg.ToString());
                    break;
                default:
                    break;
            }
        }

        private void OnEAnlyseData(NotifyCommand command, string mesgs) //收到数据触发报文解析事件
        {
            if (EAnlyseData != null)
            {
                EAnlyseData(command, mesgs);
            }
        }



        private string FormatFillEnd(string strq)
        {
            return strq.PadRight(30, ' ');
        }

        #region //状态请求
        /// <summary>
        /// 状态请求
        /// </summary>
        /// <param name="strSrmNu"></param>
        public bool RequestSRMState()
        {
            string strMsg = "";
            byte[] sendBuffer = scb.RequestSRMState();
            if (OSrmSocket.Connected)
            {
                try
                {
                    OSrmSocket.Send(sendBuffer);
                    char[] cr = System.Text.ASCIIEncoding.ASCII.GetChars(sendBuffer);
                    for (int j = 0; j < cr.Length; j++)
                        strMsg = strMsg + cr[j].ToString();
                    OnEAnlyseData(NotifyCommand.SendData, "WCS->SCS:AY:" + strMsg);//通知发送成功
                    log.WriteLog("********************************************");
                    log.WriteLog("WCS->SCS:" + strMsg);//通知发送成功
                    log.WriteLog("********************************************");
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            else
            {
                OnEAnlyseData(NotifyCommand.Break, "WCS->SCS:AY:" + m_strSRMNo + "系统异常，Socket通信连接中断，状态请求不能发送！");//通知显示发送不成功
                //log.WriteLog("********************************************");
                //log.WriteLog("WCS->SCS:AY:" + m_strSRMNo + "系统异常，Socket通信连接中断，状态请求不能发送！");
                //    log.WriteLog("********************************************");
                return false;
            }

        }
        #endregion
        #region //重启连接
        /// <summary>
        /// 重启连接
        /// </summary>
        /// <param name="strSrmNu"></param>
        public bool ReStart(string strSrmNu)
        {
            //if (OSrmSocket.Connected)
            //{
            //    RequestSRMState();

            //}
            //else
            //{
            //    OnEAnlyseData(NotifyCommand.RecvData, "WCS->SCS:AY:" + "重新闲着");//通知发送成功
            //    OSrmSocket.Close();
            //    //log.WriteLog("********************************************");
            //    //log.WriteLog("WCS->SCS:AY:" + "重新闲着");
            //    //  log.WriteLog("********************************************");
            //}
            return OSrmSocket.Connected;
        }
        #endregion
        #region //出入库任务
        /// <summary>
        /// 出入库任务
        /// </summary>
        /// <param name="SRMNo"></param>
        /// <param name="SourcePosition"></param>
        /// <param name="Destination"></param>
        /// <param name="PalletNo"></param>
        /// <param name="TaskNo"></param>
        public bool RSTaskMsg(string SRMNo, string SourcePosition, string Destination, string order, string TaskNo)
        {
            string strMsg = "";
            string num = "CRN001";
            byte[] sendBuffer = scb.SendTaskMsg(num, SourcePosition, Destination, order, TaskNo);//报文长度30字节   
            //byte[] sendBuffer = scb.SendTaskMsg(num, SourcePosition, Destination, order, TaskNo);//报文长度30字节    
            try
            {
                strMsg = System.Text.ASCIIEncoding.ASCII.GetString(sendBuffer);
                OnEAnlyseData(NotifyCommand.SendData, "WCS->SCS:AY:" + strMsg);
                log.WriteLog("WCS->SCS:AY:" + strMsg.Trim());
                return OSrmSocket.Send(sendBuffer);
            }
            catch (Exception ex)
            {
                OnEAnlyseData(NotifyCommand.Break, "WCS->SCS:AY:" + "发送出入库任务时产生异常" + ex.Message);
                log.WriteLog("WCS->SCS:AY:" + "发送出入库任务时产生异常" + ex.Message);
                return false;
            }
        }
        public bool RSTaskMsg(string SRMNo, string SourcePosition, string Destination, string SecSourcePosition, string SecDestination, string order, string TaskNo)
        {
            string strMsg = "";
            //string num = "CRN003";
            string num = SRMNo;
            byte[] sendBuffer = scb.SendTaskMsg(num, SourcePosition, Destination, SecSourcePosition,SecDestination, order, TaskNo);//报文长度30字节   
            //byte[] sendBuffer = scb.SendTaskMsg(num, SourcePosition, Destination, order, TaskNo);//报文长度30字节    
            try
            {
                strMsg = System.Text.ASCIIEncoding.ASCII.GetString(sendBuffer);
                OnEAnlyseData(NotifyCommand.SendData, "WCS->SCS:AY:" + strMsg);
                log.WriteLog("WCS->SCS:AY:" + strMsg.Trim());
                return OSrmSocket.Send(sendBuffer);
            }
            catch (Exception ex)
            {
                OnEAnlyseData(NotifyCommand.Break, "WCS->SCS:AY:" + "发送出入库任务时产生异常" + ex.Message);
                log.WriteLog("WCS->SCS:AY:" + "发送出入库任务时产生异常" + ex.Message);
                return false;
            }
        }
        #endregion
        #region //紧急停止
        /// <summary>
        /// 紧急停止
        /// </summary>
        /// <param name="SRMNo"></param>
        /// <param name="SourcePosition"></param>
        /// <param name="Destination"></param>
        /// <param name="PalletNo"></param>
        /// <param name="TaskNo"></param>
        public void TaskEStop()
        {
            string strMsg = "";
            byte[] sendBuffer = new byte[30];//报文长度30字节
            for (int i = 0; i < 30; i++)
                sendBuffer[i] = 0x20;
            sendBuffer[0] = 0x02;
            sendBuffer[3] = 0x03;
            Array.Copy(Encoding.ASCII.GetBytes("HE"), 0, sendBuffer, 1, 2);
            try
            {

                OSrmSocket.Send(sendBuffer);
                string sendMes = Encoding.ASCII.GetString(sendBuffer);
                OnEAnlyseData(NotifyCommand.SendData, "WCS->SCS:AY:" + sendMes);
                log.WriteLog("********************************************");
                log.WriteLog("WCS->SCS:AY:" + sendMes);//发送成功
                // log.WriteLog("********************************************");
            }
            catch (Exception ex)
            {
                OnEAnlyseData(NotifyCommand.Break, "WCS->SCS:AY:" + "异常，异常信息：" + ex.Message);
                log.WriteLog("********************************************");
                log.WriteLog("WCS->SCS:AY:" + "异常，异常信息：" + ex.Message);
                // log.WriteLog("********************************************");
            }
        }
        #endregion
        #region //取消停止
        /// <summary>
        /// 取消停止
        /// </summary>
        /// <param name="SRMNo"></param>
        /// <param name="SourcePosition"></param>
        /// <param name="Destination"></param>
        /// <param name="PalletNo"></param>
        /// <param name="TaskNo"></param>
        public void TaskCancelEStop()
        {
            string strMsg = "";
            byte[] sendBuffer = new byte[30];//报文长度132字节
            for (int i = 0; i < 30; i++)
                sendBuffer[i] = 0x20;
            sendBuffer[0] = 0x02;
            sendBuffer[3] = 0x03;
            Array.Copy(Encoding.ASCII.GetBytes("HC"), 0, sendBuffer, 1, 2);
            try
            {

                OSrmSocket.Send(sendBuffer);
                string sendMes = Encoding.ASCII.GetString(sendBuffer);
                OnEAnlyseData(NotifyCommand.SendData, "WCS->SCS:AY:" + sendMes);
                log.WriteLog("********************************************");
                log.WriteLog("WCS->SCS:AY:" + sendMes);//发送成功
                // log.WriteLog("********************************************");
            }
            catch (Exception ex)
            {
                OnEAnlyseData(NotifyCommand.Break, "WCS->SCS:AY:" + "异常，异常信息：" + ex.Message);
                log.WriteLog("********************************************");
                log.WriteLog("WCS->SCS:AY:" + "异常，异常信息：" + ex.Message);
                // log.WriteLog("********************************************");
            }
        }
        #endregion
        #region
        /// <summary>
        /// 回到原点
        /// </summary>
        /// <param name="SRMNo"></param>
        /// <param name="SourcePosition"></param>
        /// <param name="Destination"></param>
        /// <param name="PalletNo"></param>
        /// <param name="TaskNo"></param>
        public void SCBackOrigin()
        {
            string strMsg = "";
            byte[] sendBuffer = new byte[4];
            for (int i = 0; i < 4; i++)
                sendBuffer[i] = 0x30;
            sendBuffer[0] = 0x02;
            sendBuffer[3] = 0x03;
            Array.Copy(Encoding.ASCII.GetBytes("HP"), 0, sendBuffer, 1, 2);
            try
            {

                OSrmSocket.Send(sendBuffer);
                string sendMes = Encoding.ASCII.GetString(sendBuffer);
                OnEAnlyseData(NotifyCommand.SendData, "WCS->SCS:AY:" + sendMes);
                log.WriteLog("********************************************");
                log.WriteLog("WCS->SCS:AY:" + sendMes);
            }
            catch (Exception ex)
            {
                OnEAnlyseData(NotifyCommand.Break, "WCS->SCS:AY:" + "异常，异常信息：" + ex.Message);
                log.WriteLog("********************************************");
                log.WriteLog("WCS->SCS:AY:" + "异常，异常信息：" + ex.Message);
            }
        }
        #endregion
        #region//响应报文
        /// <summary>
        /// 响应报文
        /// </summary>
        /// <param name="recvMsg"></param>
        public void ResponseMsg(string recvMsg)
        {
            string responMsg = string.Empty;
            byte[] sendBuffer = new byte[recvMsg.Length];//报文长度到第一个空数组的长度
            if (g_nDatagramCounter > 9999)
                g_nDatagramCounter = 0;
            if (g_nDatagramSequenceNumber > 9999)
                g_nDatagramSequenceNumber = 0;
            try
            {
                if (Convert.ToInt16(recvMsg.Substring(10, 6)) == (Convert.ToInt16(recvMsg.Substring(58, 6)) + 64))//校验，判断数据包正确性
                {
                    //1-64字节 是报头
                    responMsg = recvMsg.Substring(0, 2); //"TI";1-2 版本号 
                    responMsg += recvMsg.Substring(2, 2); //"21";3-4 协议号
                    responMsg += recvMsg.Substring(4, 4);// FormatFillHead("0000", g_nDatagramCounter.ToString());//5-8 报文计数器 报文数统计，自增变量
                    responMsg += "00";// recvMsg.Substring(8, 2); //"00";//9-10 返回值
                    responMsg += recvMsg.Substring(10, 6);// "000132";//11-16 报文长度 =64+68
                    //responMsg += "000001";//17-22 

                    responMsg += recvMsg.Substring(22, 6);//源结点 变量

                    responMsg += recvMsg.Substring(16, 6);//23-28 目的结点 变量
                    responMsg += recvMsg.Substring(28, 4);
                    responMsg += "AK";// 33-34 应答
                    responMsg += "WCS001";//35-40 源服务器
                    responMsg += recvMsg.Substring(34, 6);// SRMNo;// 41-46 目的服务器
                    responMsg += recvMsg.Substring(46, 6);//47-52 操作号 
                    responMsg += "000001";//53-58 数据块数 总为1
                    responMsg += recvMsg.Substring(58, 6);//59-64 数据块长度
                    responMsg += recvMsg.Substring(64, recvMsg.Length - 64);
                    string strresponMsg = FormatFillEnd(responMsg);
                    sendBuffer = System.Text.Encoding.ASCII.GetBytes(strresponMsg.ToCharArray());
                }
                else
                {
                    string teleerr = "数据包检测不一致!";
                    OnEAnlyseData(NotifyCommand.SendData, "SCS->WCS:AY:" + teleerr.Substring(0, teleerr.Length));
                    //log.WriteLog("********************************************");
                    //log.WriteLog("SCS->WCS:AY:" + teleerr.Substring(0, teleerr.Length));
                    //  log.WriteLog("********************************************");
                }

                if (OSrmSocket.Send(sendBuffer))
                {
                    string sendMes = Encoding.ASCII.GetString(sendBuffer);
                    OnEAnlyseData(NotifyCommand.SendData, "WCS->SCS:AK:" + sendMes);
                    //log.WriteLog("********************************************");
                    //log.WriteLog("WCS->SCS:AK:" + sendMes);//通知发送成功
                    //   log.WriteLog("********************************************");
                }

            }
            catch (Exception ex)
            {
                OnEAnlyseData(NotifyCommand.Break, "WCS->SCS:AK:" + "异常，异常信息：" + ex.Message);
                //log.WriteLog("********************************************");
                //log.WriteLog("WCS->SCS:AK:" + "异常，异常信息：" + ex.Message);
                // log.WriteLog("********************************************");
            }
        }
        #endregion
        #region //报文解析
        private string ASCIIToString(string asciicode)//ASCII码转换成字符串
        {
            string str = "";
            try
            {
                for (int i = 0; i < asciicode.Length; i++)
                {
                    str += "[" + Convert.ToUInt16(Convert.ToChar(asciicode.Substring(i, 1))).ToString() + "]";//[]之间表示一个字节
                }
                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("CRL CRLSRMControl ASCIIToString Error!" + ex.Message);
            }
        }
        private int StringToASCII(string str)//string转成ASCll
        {
            string restr = "";
            char[] charstr = str.ToCharArray();
            for (int i = 0; i < str.Length; i++)
            {
                restr += ((int)charstr[i]).ToString();
            }
            if (restr != "")
            {
                restr = "4" + restr;
                return int.Parse(restr);
            }
            else
                return 0;
        }
        /// <summary>
        /// 报文解析 根据返回值触发相应的事件  需要完善
        /// </summary>
        /// <param name="asciicode">堆垛机号</param>        
        /// <returns></returns>
        private void AnalyseMessage(string revmsg)//报文解析
        {
            try
            {
                string[] submsg = revmsg.Split('');
                string subString = "";
                int j = 0,n=0;
                //for (int i = 0; i < submsg.Length; i++)
                if (submsg.Length >= 129)
                    n = 129;
                else
                    n = submsg.Length;
                for (int i = 0; i < n; i++)
                {
                    j = submsg[i].LastIndexOf("");
                    if (j >= 0)
                    {
                        subString = submsg[i].Substring(j, (submsg[i].Length - j)) + '';
                        OnEAnlyseData(NotifyCommand.RecvData, subString.Trim());
                        AnalyseSubMessage(subString);
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteLog("********************************************");
                log.WriteLog("CRL CRLSRMControl AnalyseMessager Error!" + ex.Message);
            }

        }
        /// <summary>
        /// 多条报文可能接在一起，分开处理
        /// </summary>
        /// <param name="subMsg"></param>
        private void AnalyseSubMessage(string subMsg)
        {
            string telegramHeader = subMsg.Substring(1, 2);//报文头
            string telegramBody = "";//取报体
            if (telegramHeader == "LA")//堆垛机状态反馈
            {
                //telegramBody = subMsg.Substring(3, 24);
                telegramBody = subMsg.Substring(3, 39);
                //log.WriteLog("********************************************");
                //log.WriteLog("SCS->WCS:AK:[" + telegramHeader + "]" + telegramBody);
            }
            else if (telegramHeader == "LB")//堆垛机收到任务确认反馈
            {
                if (subMsg.Substring(3, 1) == "1")
                {
                    dal.UpdateTaskStatus(subMsg.Substring(4, 5), "SC", "2", "1");
                    dal.UpdateTaskStatus(subMsg.Substring(9, 5), "SC", "2", "1");
                }
                telegramBody = subMsg.Substring(3, 13);
                log.WriteLog("********************************************");
                log.WriteLog("SCS->WCS:AK:[" + telegramHeader + "]" + telegramBody);
            }
            else
            {
                //if (subMsg.IndexOf('\0') > 64)
                //{
                //    telegramBody = subMsg.Substring(64, subMsg.IndexOf('\0') - 64);
                //}
                //else
                //{
                //    telegramBody = "";
                //}
            }
        }

        #endregion

        private void AddSequenceNumber()
        {
            lock (obj_Lock)
            {
                g_nDatagramSequenceNumber++;
                g_nDatagramCounter++;
            }
        }
        private string RandomPalletNo(string strPalletNo)//随机产生托盘号
        {
            try
            {
                if (strPalletNo.Length > 8)
                {
                    System.Random rdm = new Random();
                    return rdm.Next(100000, 999999).ToString();
                }
                else
                {
                    return strPalletNo;
                }
            }
            catch (Exception ex)
            {
                log.WriteLog("随机产生托盘号异常,信息" + ex.Message);
                return "XXXXXXXX";
            }
        }
        /// <summary>
        /// 在收到MCT发送来的作业任务状态改变包(LD)后，上位机接收后回复给MCT表示已接收
        /// </summary>
        /// <param name="SRMNo"></param>
        /// <param name="SourcePosition"></param>
        /// <param name="Destination"></param>
        /// <param name="PalletNo"></param>
        /// <param name="TaskNo"></param>
        public void ReturnLDMsg(string msg)
        {
            byte[] sendBuffer = new byte[17];
            for (int i = 0; i < 17; i++)
                sendBuffer[i] = 0x30;
            sendBuffer[0] = 0x02;
            sendBuffer[16] = 0x03;
            Array.Copy(Encoding.ASCII.GetBytes("LF"), 0, sendBuffer, 1, 2);
            Array.Copy(Encoding.ASCII.GetBytes(msg), 0, sendBuffer, 3, 13);
            try
            {

                OSrmSocket.Send(sendBuffer);
                string sendMes = Encoding.ASCII.GetString(sendBuffer);
                log.WriteLog(System.DateTime.Now.ToString("yyyyMMdd") + "业务逻辑.txt", "WCS->SCS:AY:" + sendMes);
            }
            catch (Exception ex)
            {
                log.WriteLog(System.DateTime.Now.ToString("yyyyMMdd") + "业务逻辑.txt", "WCS->SCS:AY:" + "异常，异常信息：" + ex.Message);
            }
        }
    }
}
