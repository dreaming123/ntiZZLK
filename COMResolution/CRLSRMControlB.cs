using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COMRW;

namespace COMResolution
{
    public class CRLSRMControlB : CRLBase
    {
        public delegate void AnalyseMessageEventHandler(string CommandType, object msg);//B1声明Anysisdata 解析收到数据时事件的代理，传给BLL层
        public event AnalyseMessageEventHandler AnalyseMessageEvent;

        private RWLSocketClient OSrmSocket = new RWLSocketClient();

        private string m_serverIP, m_strSRMNo, m_MessageRecv;//卫星小车IP，字段名
        private int m_serverPort;
        private LOG.Log log;

        public CRLSRMControlB(string strsrmno, string ipaddress, int port)//构造函数 
        {
            m_serverIP = ipaddress;
            m_serverPort = port;
            m_strSRMNo = strsrmno;
            log = new LOG.Log("--" + m_strSRMNo + "~" + port.ToString(), ".\\卫星小车报文日志\\");
            try
            {
                //订阅一个Socket收到数据的事件
                OSrmSocket.Notify += new NotifyCommandHander(OSrmSocket_Notify);//    
            }
            catch (Exception ex)
            {
                log.WriteLog(m_strSRMNo + ": SRM_Notify初始化函数异常" + ex.Message);
            }
        }

        //建立Socket连接
        public bool Startup()
        {
            try
            {
                return OSrmSocket.Connect(m_serverIP, m_serverPort);
            }
            catch (Exception ex)
            {
                log.WriteLog(m_strSRMNo + ": CRL CRLSRMControlB Connection Error!" + ex.Message);
                return false;
            }
        }

        //发送报文
        public bool SendMsg(byte[] msg)
        {
            return OSrmSocket.Send(msg);
        }

        private void OSrmSocket_Notify(string Command, object state)//信息处理
        {
            if (AnalyseMessageEvent != null)
            {
                string strmsg = "";
                if (Command == "RecvData")
                {
                    byte[] byt = (byte[])state;
                    if (m_serverPort == 6000)
                    {
                        for (int j = 18; j < 142; j++)
                        {
                            if (byt[j].ToString() != "0")
                                strmsg += (j - 16).ToString() + ":" + byt[j].ToString() + ";";
                        }
                        if (!System.String.IsNullOrEmpty(strmsg))
                            log.WriteLog(strmsg);
                    }
                    else
                    {
                        for (int i = 0; i < 74; i++)
                        {
                            strmsg += byt[i].ToString() + ";";
                        }
                        if (m_strSRMNo != "HC001" && m_serverPort == 4000 && Command == "RecvData" && byt[22].ToString() != "0")
                        {
                            if (m_MessageRecv != "任务号:" + byt[20].ToString() + "," + byt[21].ToString() + "; 低位指令代码:" + byt[22].ToString() + "; 高位指令代码:" + byt[38].ToString()
                                + "; 函数模式:" + byt[23].ToString() + "; 堆垛机模式:" + byt[38].ToString() + "; 动作点:" + byt[26].ToString() + "; 巷道:" + byt[27].ToString() + "; 行:" + byt[28].ToString()
                                + "; 列:" + byt[29].ToString() + "; 列前后:" + byt[30].ToString() + "; 高度:" + byt[31].ToString() + "; 深度:" + byt[32].ToString())
                            {
                                m_MessageRecv = "任务号:" + byt[20].ToString() + "," + byt[21].ToString() + "; 低位指令代码:" + byt[22].ToString() + "; 高位指令代码:" + byt[38].ToString()
                                + "; 函数模式:" + byt[23].ToString() + "; 堆垛机模式:" + byt[38].ToString() + "; 动作点:" + byt[26].ToString() + "; 巷道:" + byt[27].ToString() + "; 行:" + byt[28].ToString()
                                + "; 列:" + byt[29].ToString() + "; 列前后:" + byt[30].ToString() + "; 高度:" + byt[31].ToString() + "; 深度:" + byt[32].ToString();
                                log.WriteLog(m_MessageRecv);
                            }
                        }
                    }
                }
                else if (Command == "Break" || Command == "Connect")
                    log.WriteLog(Command + ": " + state.ToString());
                AnalyseMessageEvent(Command, strmsg);
            }
        }
    }
}
