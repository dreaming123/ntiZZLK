using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using COMRW;

namespace COMResolution
{
    public delegate void UpdateAGVStatus(string Command, object state);

    public class CRLAgvControl
    {
        private RWLSocketClient nt8000 = new RWLSocketClient();
        private Timer HeartBeatTimer = new Timer();
        private string ServerAddr = "";
        private int ServerPort = 0;
        private LOG.Log log = new LOG.Log("AGV数据解析日志", ".\\");

        public event UpdateAGVStatus Notify;

        public CRLAgvControl()
        {
            ServerAddr = System.Configuration.ConfigurationSettings.AppSettings["NT8000IP"];
            ServerPort = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["NT8000Port"]);

            nt8000.Notify += new NotifyCommandHander(nt8000_Notify);

            HeartBeatTimer.Interval = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["CheckConnectTimer"]);
            HeartBeatTimer.Elapsed += new ElapsedEventHandler(HeartBeatTimer_Elapsed);
        }

        public void Run()
        {
            if (!nt8000.Connect(ServerAddr, ServerPort))
                HeartBeatTimer.Start();
        }

        private void HeartBeatTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (nt8000.Connect(ServerAddr, ServerPort))
                HeartBeatTimer.Stop();
        }

        private void nt8000_Notify(string command, object data)
        {
            if (command == "RecvData")
            {
                byte[][] b = SplitMsgFromRecvData((byte[])data);
                for (int i = 0; i < b.GetLength(0); i++)
                {
                    if (b[i] != null)
                        HandleRecvMsgRecord(b[i]);
                }
            }
            else
            {
                if (Notify != null)
                {
                    if (command == "Break")
                        HeartBeatTimer.Start();
                    Notify(command, data);
                }
            }
        }

        private void HandleRecvMsgRecord(byte[] data)
        {
            //处理分离出的信息记录
            try
            {
                byte[] Buffer;
                switch ((int)data[7])
                {
                    //Normal Message
                    case 1:
                        {
                            Buffer = new byte[data.Length - 8];
                            Array.Copy(data, 8, Buffer, 0, data.Length - 8);
                            HandleRecvMsg(Buffer);
                            break;
                        }

                    //Disconnect Link
                    case 2:
                        {
                            break;
                        }
                    //Heart-Beat Poll
                    case 4:
                        {
                            nt8000.Send(PackMsg(5, new Byte[0]));
                            break;
                        }
                    //Heart-Beat Ack
                    case 5:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                log.WriteLog("HandleRecvMsgRecord异常:" + ex.Message);
            }
        }

        private void HandleRecvMsg(byte[] msg)
        {
            if (Notify != null)
            {
                char ch = (char)msg[1];
                Notify(ch.ToString(), msg);
            }
        }

        private byte[][] SplitMsgFromRecvData(byte[] data)
        {
            //从接收缓存中分离出指令信息
            byte[][] Buffer = new byte[9][];
            try
            {
                int i = 0;
                int j = 0;
                while (i < data.Length)
                {
                    if ((int)data[i] == 0x87)
                    {
                        if ((int)data[++i] == 0xcd)
                        {
                            if ((data[++i] * 256 + data[++i]) == 8)
                            {
                                int k = 0;
                                k = 8 + data[i + 1] * 256 + data[i + 2];
                                Buffer[j] = new byte[k];
                                Array.Copy(data, i - 3, Buffer[j], 0, k);
                                i += k - 3;
                                j++;
                                if (j >= 9) break;
                            }
                            else
                            {
                                ++i;
                                continue;
                            }
                        }
                        else
                        {
                            ++i;
                            continue;
                        }
                    }
                    else
                    {
                        ++i;
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteLog("AGV控制模块SplitMsgFromRecvData函数发生异常:" + ex.Message);
            }
            return Buffer;
        }

        public bool SendTask(int taskno, int fromstation, int tostation)
        {
            byte[] msg = new byte[14];
            msg[0] = 0x00;
            msg[1] = (byte)'q';
            msg[2] = 0x00;
            msg[3] = 0x0a;
            msg[4] = 0x01;
            msg[5] = 0x80;
            msg[6] = 0x00;
            msg[7] = 0x01;
            Array.Copy(GetHexBytes(taskno), 0, msg, 8, 2);
            Array.Copy(GetHexBytes(fromstation), 0, msg, 10, 2);
            Array.Copy(GetHexBytes(tostation), 0, msg, 12, 2);
            if (nt8000.Connected)
                return nt8000.Send(PackMsg(1, msg));
            return false;
        }

        public bool SendmMessage(byte[] msg, int tostation)
        {
            byte[] retmsg = new byte[10];
            retmsg[0] = 0x00;
            retmsg[1] = (byte)'m';
            retmsg[2] = 0x00;
            retmsg[3] = 0x06;
            Array.Copy(msg, 4, retmsg, 4, 2);
            retmsg[6] = 0x00;
            if (tostation == 1)
                retmsg[7] = 0x05;
            else
                retmsg[7] = 0x01;
            Array.Copy(GetHexBytes(tostation), 0, retmsg, 8, 2);
            if (nt8000.Connected)
                return nt8000.Send(PackMsg(1, retmsg));
            return false;
        }

        public bool SendjMessage(int index)
        {
            byte[] retmsg = new byte[3];
            retmsg[0] = (byte)'j';
            Array.Copy(GetHexBytes(index), 0, retmsg, 1, 2);
            if (nt8000.Connected)
                return nt8000.Send(PackMsg(1, retmsg));
            return false;
        }

        public bool SendjMessageByCarNo(int carno)
        {
            byte[] retmsg = new byte[4];
            retmsg[0] = (byte)'j';
            retmsg[1] = 0x00;
            retmsg[2] = 0x00;
            retmsg[3] = (byte)carno;
            if (nt8000.Connected)
                return nt8000.Send(PackMsg(1, retmsg));
            return false;
        }

        private byte[] PackMsg(int functionCode, byte[] Msg)
        {
            //封装信息
            byte[] Buffer = new byte[8 + Msg.Length];
            Buffer[0] = 0x87;
            Buffer[1] = 0xcd;
            Buffer[2] = 0x00;
            Buffer[3] = 0x08;
            Buffer[4] = GetHexBytes(Msg.Length)[0];
            Buffer[5] = GetHexBytes(Msg.Length)[1];
            Buffer[6] = GetHexBytes(functionCode)[0];
            Buffer[7] = GetHexBytes(functionCode)[1];
            Array.Copy(Msg, 0, Buffer, 8, Msg.Length);
            return Buffer;
        }

        private byte[] GetHexBytes(int i)
        {
            //将一个整数转化为长度为2的byte型数组
            byte[] Buffer = new byte[2];
            Buffer[0] = (byte)(i / 256);
            Buffer[1] = (byte)(i % 256);
            return Buffer;
        }
    }
}
