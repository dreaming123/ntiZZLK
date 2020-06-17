using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

namespace COMRW
{
    public delegate void PLCNotify(string Command, object state);

    public class PLCSocketClient
    {
        public event PLCNotify PlcNotify;
        private int bytelength = 390;

        private TcpClient cliSocket;
        private Thread ComThread;
        private bool g_brun;
        private readonly object qLock = new object();
        private int TimeOut = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["TimeOut"].ToString());

        public PLCSocketClient(int length)
        {
            g_brun = false;
            bytelength = length;
        }

        public PLCSocketClient()
        {
            g_brun = false;
        }

        public void Close()
        {
            g_brun = false;

            if (ComThread != null)
            {
                if (ComThread.ThreadState == ThreadState.Running)
                {
                    ComThread.Abort();
                }
                Thread.Sleep(50);
            }

            if (cliSocket != null)
            {
                if (Connected)
                {
                    cliSocket.Client.Shutdown(SocketShutdown.Both);
                }
                cliSocket.Client.Close();
            }
            GC.Collect();
        }

        public bool Connected
        {
            get
            {
                if (cliSocket != null) //注意此值得含义
                    return cliSocket.Connected;
                else
                    return false;
            }
        }

        public bool Connect(string strServer, int nPort)
        {
            try
            {
                if (System.String.IsNullOrEmpty(strServer) || nPort == 0)
                { return false; }
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(strServer), nPort);
                cliSocket = TimeOutSocket.Connect(iep, TimeOut);
                if (cliSocket != null)
                {
                    //PlcNotify("Connect", " 连接成功！");
                    g_brun = true;
                  
                    //Send(new byte[] { 0x53, 0x35, 0x10, 0x01, 0x03, 0x03, 0x03, 0x08, 0x01, 0x0B, 0x00, 0x00, 0x00, 0x14, 0xFF, 0x02 });
                    ComThread = new Thread(new ThreadStart(Recv));
                    ComThread.Start();
                    return true;
                }
                else
                {
                    PlcNotify("Break", " 连接失败！");
                    return false;
                }
            }
            catch (SocketException ex)
            {
                PlcNotify("Break", " 连接失败,异常:" + ex.Message);
                return false;
            }
        }
        public bool Connect_2001(string strServer, int nPort)
        {
            try
            {
                if (System.String.IsNullOrEmpty(strServer) || nPort == 0)
                { return false; }
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(strServer), nPort);
                cliSocket = TimeOutSocket.Connect(iep, TimeOut);
                if (cliSocket != null)
                {
                    return true;
                }
                else
                {
                    PLCNotifyEvent("Break", " 连接失败！");
                    return false;
                }
            }
            catch (SocketException ex)
            {
                PLCNotifyEvent("Break", " 连接失败,异常:" + ex.Message);
                return false;
            }
        }
        public bool CheckConnect(string strServer, int nPort)
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(strServer), nPort);
            cliSocket = TimeOutSocket.Connect(iep, TimeOut);
            if (cliSocket != null)
                return true;
            else
                return false;
        }

        public bool Send(byte[] bytData)
        {
            //0x53, 0x35, 0x10, 0x01, 0x03, 0x03, 0x03, 0x08, 0x01, 0x0B, 0x00, 0x00, 0x00, 0x14, 0xFF, 0x02
            try
            {
                lock (qLock)
                {
                    if (cliSocket.Client.Connected)
                    {
                        int i = cliSocket.Client.Send(bytData);
                        if (i > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        Close();
                        PlcNotify("Break", "SendData Fail!");
                        return false;
                    }
                }
            }
            catch (SocketException ex)
            {
                Close();
                PlcNotify("Break", ex.Message);
                return false;
            }
        }

        public void Recv()
        {
            byte[] bytBuffer = new byte[bytelength];// 报文长度
            while (g_brun)
            {
                try
                {
                    if (cliSocket != null)
                    {
                        if (cliSocket.Client.Connected)
                        {
                            Send(new byte[] { 0x53, 0x35, 0x10, 0x01, 0x03, 0x05, 0x03, 0x08, 0x01, 0x0A, 0x00, 0x00, 0x00, 0x96, 0xFF, 0x02 });
                            Thread.Sleep(200);
                            for (int i = 0; i < bytBuffer.Length; i++)
                            {
                                bytBuffer[i] = 0;
                            }
                            int nLen = cliSocket.Client.Receive(bytBuffer);
                            if (nLen > 0)
                            {
                                PLCNotifyEvent("RecvData", bytBuffer);
                            }
                            else
                            {
                                //连接断开
                                Close();
                                PLCNotifyEvent("Break", " 接收到的报文数据长度为0！");
                                break;
                            }
                        }
                    }
                }
                catch (SocketException ex)
                {
                    Close();
                    PLCNotifyEvent("Break", " 连接中断！" + ex.Message);
                }
                Thread.Sleep(800);
            }
        }

        private void PLCNotifyEvent(string Command, object oState)
        {
            if (this.PlcNotify != null)
            {
                PlcNotify(Command, oState);
            }
        }
    }
}
