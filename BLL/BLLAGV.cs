using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Data;
using DataAccess;
using COMResolution;

namespace BLL
{
    public class BLLAGV
    {
        private DALDATA dal = new DALDATA();

        private CRLAgvControl agvclass;
        private Timer ScanDbTimer = new Timer();

        public delegate void Sys_conn(string command, object state);
        public event Sys_conn GetConnectStatus;

        public delegate void NotifyTask(DataTable dt);
        public event NotifyTask GetTask;

        private int maxcount = Convert.ToInt16(System.Configuration.ConfigurationSettings.AppSettings["AgvMaxTaskCount"]);

        public BLLAGV()
        {
            agvclass = new CRLAgvControl();
            agvclass.Notify += new UpdateAGVStatus(agvclass_Notify);

            ScanDbTimer.Interval = Convert.ToInt16(System.Configuration.ConfigurationSettings.AppSettings["SCanDBTime"]);
            ScanDbTimer.Elapsed += new ElapsedEventHandler(ScanDbTimer_Elapsed);
        }

        public void AgvRun()
        {
            agvclass.Run();
        }

        public void SetTimer(bool flag)
        {
            ScanDbTimer.Enabled = flag;
        }

        private void ScanDbTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DataTable dt = dal.GetTaskInfo(3, 0, "", "", "", "", "", "", "");

            if (GetTask != null)
                GetTask(dt);
            if (dt != null)
            {
                int executingcount = 0, canexecutecount = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["taskstatus"].ToString() == "执行中")
                    {
                        dt.Rows.Remove(dt.Rows[i]);
                        executingcount++;
                    }
                }
                canexecutecount = maxcount - executingcount;
                if (canexecutecount > dt.Rows.Count)
                    canexecutecount = dt.Rows.Count;
                if (canexecutecount > 0)
                {
                    for (int i = 0; i < canexecutecount; i++)
                    {
                        if (dt.Rows[i]["taskstatus"].ToString() == "未开始")
                        {
                            if (agvclass.SendTask(int.Parse(dt.Rows[i]["taskno"].ToString()), int.Parse(dt.Rows[i]["fromstation"].ToString()), int.Parse(dt.Rows[i]["tostation"].ToString())))
                                NotifyConnect("WCS --> NT8000 (q-message) :" + dt.Rows[i]["taskno"].ToString() + "  " + dt.Rows[i]["fromstation"].ToString() + " --> " + dt.Rows[i]["tostation"].ToString(), null);
                            else
                                NotifyConnect("Break", null);
                        }
                    }
                }
            }
        }

        public void SendjMessage(int carno)
        {
            agvclass.SendjMessageByCarNo(carno);
        }

        private void agvclass_Notify(string command, object data)
        {
            switch (command)
            {
                case "Connect":
                    ScanDbTimer.Start();
                    NotifyConnect(command, data);
                    break;
                case "Break":
                    ScanDbTimer.Stop();
                    NotifyConnect(command, data);
                    break;
                case "b":
                    bMessageHandler((byte[])data);
                    break;
                case "s":
                    sMessageHandler((byte[])data);
                    break;
                case "E": EMessageHandle((byte[])data);
                    break;
                case "r": rMessageHandler((byte[])data);
                    break;
                default: NotifyConnect(command, null);
                    break;
            }
        }

        private void EMessageHandle(byte[] msg)
        {
            Int32 unitId = msg[4] * 256 + msg[5];
            Int32 LGVNo = msg[6] * 256 + msg[7];
            Int32 EventCode = msg[18] * 256 * 256 * 256 + msg[19] * 256 * 256 + msg[20] * 256 + msg[21];
            NotifyConnect("NT8000 --> WCS (E-message) " + LGVNo.ToString() + ": " + EventCode, null);
        }

        private void bMessageHandler(byte[] msg)
        {
            //string index = msg[4].ToString("X2").Trim() + msg[5].ToString("X2").Trim();
            //string status = msg[7].ToString("X2").Trim();
            //string ACKNo = msg[8].ToString("X2").Trim();
            //int ikey = 0;
            //if (msg.Length > 11)
            //    ikey = msg[10] * 256 + msg[11];

            //switch (status)
            //{
            //    case "01": dal.UpdateAGVIndex(ikey.ToString(), index); dal.UpdateTaskStatus(index, "0", "9"); break;
            //    case "02":
            //        dal.UpdateTaskStatus(index, ACKNo, "8");
            //        if (ikey > 0)
            //            dal.UpdateAGVIndex(ikey.ToString(), index);
            //        break;
            //    case "07":
            //        if (ACKNo == "01" || ACKNo == "00")
            //            agvclass.SendmMessage(msg, 1);
            //        break;
            //}
            //NotifyConnect("NT8000 --> WCS (b-Message) :" + status, null);
        }

        private void sMessageHandler(byte[] msg)
        {
            string index = msg[4].ToString("X2").Trim() + msg[5].ToString("X2").Trim();
            string magic = msg[8].ToString("X2").Trim() + msg[9].ToString("X2").Trim();
            string Carno = msg[12].ToString();

            //switch (magic)
            //{
            //    case "1000": NotifyConnect("NT8000 --> WCS (s-message) 冷启动", null); break;
            //    case "1001": NotifyConnect("NT8000 --> WCS (s-message) 热启动", null); break;
            //    case "1002": NotifyConnect("NT8000 --> WCS (s-message) 任务在取货之前结束", null); break;
            //    case "1003": NotifyConnect("NT8000 --> WCS (s-message) 空车在CarWash状态", null); break;
            //    case "1004": NotifyConnect("NT8000 --> WCS (s-message) 载货车在CarWash状态", null); break;
            //    case "1005": NotifyConnect("NT8000 --> WCS (s-message) 小车在系统中成功移动", null); break;
            //    case "1006": NotifyConnect("NT8000 --> WCS (s-message) 小车在系统中不能移动报错", null); break;
            //    case "1007": NotifyConnect("NT8000 --> WCS (s-message) WCS发送任务的参数有问题,NT8000不接受", null); break;
            //    case "0C00": NotifyConnect("NT8000 --> WCS (s-message) 小车到达搬运目的地", null); break;

            //    case "0C01":
            //        NotifyConnect("NT8000 --> WCS (s-message) " + Carno + "小车开始取货", null);
            //        dal.UpdateTaskStatus(index, Carno, "1");//准备取货
            //        break;

            //    case "0C02": NotifyConnect("NT8000 --> WCS (s-message) " + Carno + "小车取到货物", null);
            //        dal.UpdateTaskStatus(index, Carno, "2");//取货完成
            //        break;

            //    case "0C03": NotifyConnect("NT8000 --> WCS (s-message) 人工状态下小车取到货物", null); break;
            //    case "0C04": NotifyConnect("NT8000 --> WCS (s-message) 小车空取货", null); break;

            //    case "0C10": NotifyConnect("NT8000 --> WCS (s-message) " + Carno + "小车准备放货", null);
            //        dal.UpdateTaskStatus(index, Carno, "3");//准备放货
            //        break;

            //    case "0C11": NotifyConnect("NT8000 --> WCS (s-message) " + Carno + "小车放货完成", null);
            //        dal.UpdateTaskStatus(index, Carno, "4");//放货完成
            //        break;

            //    case "0C12": NotifyConnect("NT8000 --> WCS (s-message) 人工状态下下车放货完成", null); break;
            //    case "0C13": NotifyConnect("NT8000 --> WCS (s-message) 目标站台位被占领,发生重复存放", null); break;
            //    case "0B00": NotifyConnect("NT8000 --> WCS (s-message) 自动开始充电指令", null); break;
            //    case "0B01": NotifyConnect("NT8000 --> WCS (s-message) 人工开始充电指令", null); break;
            //    case "0B02": NotifyConnect("NT8000 --> WCS (s-message) 充电指令结束", null); break;
            //    case "0B10": NotifyConnect("NT8000 --> WCS (s-message) 小车走到自动充电位", null); break;
            //    case "0B11": NotifyConnect("NT8000 --> WCS (s-message) 充电站开始充电", null); break;
            //    case "0BE1": NotifyConnect("NT8000 --> WCS (s-message) 充电指令执行不成功", null); break;
            //    case "0BE2": NotifyConnect("NT8000 --> WCS (s-message) 充电取消", null); break;
            //    default: NotifyConnect("NT8000 --> WCS (s-message) " + magic, null); break;
            //}
            agvclass.SendmMessage(msg, 1);
        }

        private void rMessageHandler(byte[] msg)
        {
            string index = msg[4].ToString("X2").Trim() + msg[5].ToString("X2").Trim();

            //NotifyConnect("NT8000 --> WCS (r-message) ", null);

            //DataTable dt = dal.GetSubTaskInfo();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (dt.Rows[i]["agvindex"].ToString() == index)
            //    {
            //        agvclass.SendTask(int.Parse(dt.Rows[i]["taskno"].ToString()), int.Parse(dt.Rows[i]["fromstation"].ToString()), int.Parse(dt.Rows[i]["tostation"].ToString()));
            //        NotifyConnect("cold start ReSend WCS --> NT8000 (q-message) :" + dt.Rows[i]["taskno"].ToString() + "  " + dt.Rows[i]["fromstation"].ToString() + " --> " + dt.Rows[i]["tostation"].ToString(), null);
            //    }
            //}
        }

        public bool AddTask(string fromstation, string tostation)
        {
            ////return dal.InsertTask(fromstation, tostation);
            return false;
        }

        private void NotifyConnect(string command, object state)
        {
            if (GetConnectStatus != null)
                GetConnectStatus(command, state);
        }
    }
}
