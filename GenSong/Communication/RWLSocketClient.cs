using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace GenSong
{
    public delegate void NotifyCommandHander(NotifyCommand Command, object state);
    public class RWLSocketClient
    {

        public event NotifyCommandHander Notify;
        private int bytelength = 1024;

        private TcpClient cliSocket;
        private Thread ComThread;
        private bool g_brun;
        private readonly object qLock = new object();
        private int TimeOut = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["TimeOut"].ToString());
        StringBuilder strRecvMessage = new StringBuilder();
        public RWLSocketClient(int length)
        {
            g_brun = false;
            bytelength = length;
        }

        public RWLSocketClient()
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

            if (cliSocket != null && cliSocket.Client != null)
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
                if (cliSocket != null && cliSocket.Client != null)
                    return cliSocket.Client.Connected;
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
                //cliSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //cliSocket.Connect(iep);
                //cliSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //cliSocket.Connect(iep);
                cliSocket = TimeOutSocket.Connect(iep, 100);
                if (cliSocket != null && cliSocket.Client != null)
                {
                    RaiseNotifyEvent(NotifyCommand.Connect, " Socket 连接成功！");
                    g_brun = true;
                    ComThread = new Thread(new ThreadStart(Recv));//处理收到的数据
                    ComThread.Start();
                    return true;
                }
                else
                {
                    RaiseNotifyEvent(NotifyCommand.Break, " Socket 连接失败！");
                    return false;
                }
            }
            catch (SocketException ex)
            {
                RaiseNotifyEvent(NotifyCommand.Break, " 连接失败,异常:" + ex.Message);
                return false;
            }
        }
        //发送数据
        public bool Send(byte[] bytData)
        {
            try
            {
                lock (qLock)
                {
                    if (cliSocket != null && cliSocket.Client != null)
                    {
                        if (cliSocket.Client.Connected)
                        {
                            cliSocket.Client.Send(bytData);
                            return true;
                        }
                        else
                        {
                            Close();
                            RaiseNotifyEvent(NotifyCommand.Break, "SendData Fail!");
                            return false;
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                Close();
                RaiseNotifyEvent(NotifyCommand.Break, ex.Message);
                return false;
            }
            return false;
        }

        //接收数据
        public void Recv()
        {
            byte[] bytBuffer = new byte[bytelength];// 报文长度
            while (g_brun)
            {
                try
                {
                    if (cliSocket != null && cliSocket.Client != null)
                    {
                        if (cliSocket.Client.Connected)
                        {
                            //Thread.Sleep(500);
                            for (int i = 0; i < bytBuffer.Length; i++)
                            {
                                bytBuffer[i] = 0;
                            }
                            int nLen = cliSocket.Client.Receive(bytBuffer);
                            if (nLen > 0)
                            {
                                strRecvMessage.AppendLine(System.Text.Encoding.ASCII.GetString(bytBuffer));//将接收的数据转换成字符串
                                RaiseNotifyEvent(NotifyCommand.RecvData, strRecvMessage.ToString());
                                strRecvMessage.Remove(0, strRecvMessage.Length);
                            }
                            else
                            {
                                //连接断开
                                Close();
                                RaiseNotifyEvent(NotifyCommand.Break, " 接收到的报文数据长度为0！");
                                break;
                            }
                        }
                    }
                }
                catch (SocketException ex)
                {
                    Close();
                    RaiseNotifyEvent(NotifyCommand.Break, " 连接中断！" + ex.Message);
                }
                Thread.Sleep(100);
            }
        }

        private void RaiseNotifyEvent(NotifyCommand Command, object oState)
        {
            if (this.Notify != null)
            {
                Notify(Command, oState);
            }
        }
    }
}
