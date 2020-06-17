using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COMRW;

namespace COMResolution
{
    public class CRLPLCControl : CRLBase
    {
        public delegate void AnalyseMessageEventHandler(string CommandType, string msg);//B1声明Anysisdata 解析收到数据时事件的代理，传给BLL层
        public event AnalyseMessageEventHandler AnalyseMessageEvent;
        public PLCSocketClient PLCSocket = new PLCSocketClient();
        private LOG.Log log = new LOG.Log("PLC扫描数据解析日志", ".\\");
        private String ServerAddr = String.Empty;
        private int ServerPortRecv = 0, ServerPortSend = 0;
        private readonly object rLock = new object();
        public CRLPLCControl(string ipaddress, int port)
        {
            ServerAddr = ipaddress;
            ServerPortRecv = port;
            try
            {
                //订阅一个Socket收到数据的事件
                PLCSocket.PlcNotify += new PLCNotify(PLCSocket_Notify);
            }
            catch (Exception ex)
            {
                log.WriteLog("PLC_Notify初始化函数异常" + ex.Message);
            }
        }

        //建立Socket连接
        public bool Startup()
        {
            try
            {
                return PLCSocket.Connect(ServerAddr, ServerPortRecv);
            }
            catch (Exception ex)
            {
                log.WriteLog("CRL CRLPLCControl Connection Error!" + ex.Message);
                return false;
            }
        }

        //发送报文
        public bool SendMsg(byte[] msg)
        {
            return PLCSocket.Send(msg);
        }

        private void PLCSocket_Notify(string type, object msg)//信息处理
        {
            lock (rLock)
            {
                Int32 stationno, fromstation, tostation, goodtype, taskno = 0;
                string itemcode = string.Empty;

                string strmsg = string.Empty;
                int itemcodeCount = 0;
                if (type == "RecvData" && msg.ToString().Length > 12)
                {
                    byte[] byt = (byte[])msg;

                    for (int i = 1; i < 7; i++)
                    {
                        strmsg = "";
                        itemcode = "";
                        for (int j = 0; j < 54; j++)
                        {
                            int startPosition = 16 + (i - 1) * 54;

                            if (j / 22 == 1 && j % 22 == 0)
                            {
                                itemcodeCount = 32;
                            }
                            if (itemcodeCount > 0)
                            {
                                itemcode += Convert.ToChar(byt[startPosition + j]);
                                itemcodeCount--;
                            }
                            else
                            {
                                strmsg += byt[startPosition + j].ToString() + ";";
                            }
                        }
                        string[] singlemsg = strmsg.Split(new char[] { ';' });
                        stationno = Int32.Parse(singlemsg[0]) * 256 + Int32.Parse(singlemsg[1]);
                        if (stationno == 5037 || stationno == 5038 || stationno == 5138)
                        {
                            if (AnalyseMessageEvent != null)
                                AnalyseMessageEvent(itemcode, strmsg);
                        }
                    }
                }
                else if (type == "Break")
                {
                    AnalyseMessageEvent(type, "接收到的报文数据长度为0");
                }
            }

        }
    }
}
