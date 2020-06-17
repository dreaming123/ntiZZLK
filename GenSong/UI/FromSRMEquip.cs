using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using DataGridViewAutoFilter;
using System.Drawing.Drawing2D;
using System.Threading;

namespace GenSong.UI
{
    public partial class FromSRMEquip : Form
    {
        #region 自动关闭弹出框
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public const int WM_CLOSE = 0x10;

        private void StartKiller()
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 500;
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            KillMessageBox();
            //停止Timer   
            ((System.Windows.Forms.Timer)sender).Stop();
        }

        private void KillMessageBox()
        {
            //依MessageBox的標題,找出MessageBox的視窗   
            IntPtr ptr = FindWindow(null, " ");
            if (ptr != IntPtr.Zero)
            {
                //找到則關閉MessageBox視窗   
                PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }
        #endregion

        string strPalletNu = string.Empty;
        GenSongSRMRun bsb = new GenSongSRMRun();
        private delegate void PictureCallBack(List<CRNObject> cob, List<CRNStatus> css);
        private delegate void RichBoxCallBack(RichTextBox rb, string msg);
        string taskmode = "手动";
        string _zone = null;
        public FromSRMEquip(string zone)
        {
            InitializeComponent();
            string fromName = "MiniLoad堆垛机";//null;
            //switch (zone)
            //{
            //    case "A": fromName = "堆垛机"; break;
            //    case "B": fromName = "高温陈化"; break;
            //    case "C": fromName = "化成区"; break;
            //    case "D": fromName = "常温陈化"; break;
            //    case "E": fromName = "冲放电区"; break;
            //    case "F": fromName = "高温老化区"; break;
            //    case "G": fromName = "电池降温区"; break;
            //    case "H": fromName = "分容区"; break;
            //    case "I": fromName = "常温存放"; break;
            //    case "J": fromName = "自放电"; break;
            //    default: fromName = "堆垛机"; break;
            //}
            this.Text = fromName;
            _zone = zone;
        }

        private void FromSRMEquip_Load(object sender, EventArgs e)
        {
            bsb.ENotifyCRCEventHandler += new DNotifyCRC(bsb_ENotifyCRCEventHandler);
            bsb.EDNotify += new GenSongSRMRun.DNotify(bsb_EDNotify);
            bsb.Start(_zone);
        }
        private void bsb_ENotifyCRCEventHandler(List<CRNObject> cob, List<CRNStatus> css, string eNotify, object oNotify)
        {
            try
            {
                DrawConnect(cob, css);
            }
            catch { }
        }
        private void bsb_EDNotify(string type, string msg)
        {
            if (type == "C")
                ShowText(rtb_conn, msg);
            else
                ShowText(rtb_msg, msg);
        }


        private void ShowText(RichTextBox rb, string msg)
        {
            if (rb.InvokeRequired)
            {
                RichBoxCallBack rbcb = new RichBoxCallBack(ShowText);
                this.Invoke(rbcb, new object[] { rb, msg });
            }
            else
            {
                if (rb.TextLength > 5000)
                    rb.Clear();
                rb.AppendText(System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ": " + msg + "\r\n");
            }
        }

        private void DrawConnect(List<CRNObject> cob, List<CRNStatus> css)
        {
            try
            {
                if (pic.InvokeRequired)
                {
                    PictureCallBack pc = new PictureCallBack(DrawConnect);
                    this.Invoke(pc, new object[] { cob, css });
                }
                else
                {
                    bool refresh = false;
                    Graphics g = pic.CreateGraphics();

                    SolidBrush brush;
                    Pen pen;
                    foreach (CRNObject cco in cob)
                    {
                        CRNStatus ccs = css.Find(
                                delegate(CRNStatus crcs)
                                {
                                    return crcs.CRCNum == cco.ScNo && crcs.Zone == _zone;
                                }
                            );
                        if (ccs != null)
                        {
                            if (ccs.Change)
                                refresh = true;
                        }
                    }
                    if (refresh)
                        pic.Refresh();
                    g.DrawRectangle(pen = new Pen(brush = new SolidBrush(Color.Black), 1), 2, 2, 40, 24);
                    g.FillRectangle(brush = new SolidBrush(Color.SkyBlue), 4, 4, 37, 21);
                    g.DrawString(taskmode, new Font("宋体", 12, FontStyle.Bold), brush = new SolidBrush(taskmode == "自动" ? Color.Blue : Color.Red), 4, 6);

                    foreach (CRNObject co in cob)
                    {
                        CRNStatus findcs = css.Find(
                                delegate(CRNStatus info)
                                {
                                    return info.CRCNum == co.ScNo && info.Zone == _zone;
                                }
                            );
                        if (findcs != null)
                        {
                            int num = int.Parse(findcs.CRCNum.Substring(3, 3)) - 1;
                            int cell = 225;
                            //堆垛机载体
                            g.FillRectangle(brush = new SolidBrush(co.connectstatus == 1 ? Color.Green : Color.Gray), 20 + num * cell, 241, 120, 12);
                            //堆垛机滚轮
                            g.DrawEllipse(pen = new Pen(brush = new SolidBrush(Color.Black), 1), 25 + num * cell, 247, 9, 9);
                            g.FillEllipse(brush = new SolidBrush(Color.Black), 25 + num * cell, 247, 9, 9);
                            g.DrawEllipse(pen = new Pen(brush = new SolidBrush(Color.Black), 1), 125 + num * cell, 247, 9, 9);
                            g.FillEllipse(brush = new SolidBrush(Color.Black), 125 + num * cell, 247, 9, 9);
                            //堆垛机主臂
                            g.FillRectangle(brush = new SolidBrush(co.connectstatus == 1 ? Color.Green : Color.Gray), 70 + num * cell, 45, 16, 204);
                            //堆垛机电柜
                            g.FillRectangle(brush = new SolidBrush(Color.SlateGray), 100 + num * cell, 181, 22, 60);
                            g.DrawString(findcs.CRCNum.Substring(5, 1), new Font("宋体", 12, FontStyle.Bold), brush = new SolidBrush(Color.Blue), 103 + num * cell, 185);
                            //堆垛机爬梯
                            for (int i = 45; i < 243; i = i + 11)
                            {
                                g.DrawRectangle(pen = new Pen(brush = new SolidBrush(Color.SlateGray), 1), 87 + num * cell, i, 10, 10);
                            }
                            //爬梯与电柜的连接体
                            g.DrawLine(pen = new Pen(brush = new SolidBrush(Color.Black), 4), 85 + num * cell, 201, 100 + num * cell, 201);
                            //堆垛机货叉
                            g.DrawRectangle(pen = new Pen(brush = new SolidBrush(Color.Black), 4), 22 + num * cell, 240, 41, 3);
                            g.DrawRectangle(pen = new Pen(brush = new SolidBrush(Color.Black), 4), 60 + num * cell, 220, 3, 20);
                            g.DrawRectangle(pen = new Pen(brush = new SolidBrush(Color.Black), 4), 60 + num * cell, 230, 20, 4);
                            //g.DrawRectangle(pen = new Pen(brush = new SolidBrush(Color.Black), 4), 22 + num * cell, (130 - 11 * int.Parse(string.IsNullOrEmpty(findcs.RackLevel) ? "0" : findcs.RackLevel)) + 110, 41, 3);
                            //g.DrawRectangle(pen = new Pen(brush = new SolidBrush(Color.Black), 4), 60 + num * cell, (130 - 11 * int.Parse(string.IsNullOrEmpty(findcs.RackLevel) ? "0" : findcs.RackLevel)) + 90, 3, 20);
                            //g.DrawRectangle(pen = new Pen(brush = new SolidBrush(Color.Black), 4), 60 + num * cell, (130 - 11 * int.Parse(string.IsNullOrEmpty(findcs.RackLevel) ? "0" : findcs.RackLevel)) + 100, 20, 4);

                            //货物及任务号
                            if (int.Parse(findcs.TaskNo) != 0)
                            {
                                if (int.Parse(findcs.FunctionReport) > 113)
                                    g.FillRectangle(brush = new SolidBrush(Color.Chocolate), 25 + num * cell, 215, 30, 22);//(130 - 11 * int.Parse(string.IsNullOrEmpty(findcs.RackLevel) ? "0" : findcs.RackLevel)) + 85
                                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;//恢复实线  
                                pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;//定义线尾的样式为箭头  
                                pen.Width = 6;
                                if (int.Parse(findcs.FunctionReport) == 114) //取货箭头 
                                    g.DrawLine(pen, 2 + num * cell, 215, 17 + num * cell, 215);
                                else if (int.Parse(findcs.FunctionReport) == 130)//放货箭头
                                    g.DrawLine(pen, 17 + num * cell, 215, 2 + num * cell, 215);
                                else
                                    g.FillRectangle(brush = new SolidBrush(Color.Silver), 2 + num * cell, 215, 15, 10);

                                //g.DrawString(findcs.Palletnu.ToString(), new Font("宋体", 11, FontStyle.Bold), brush = new SolidBrush(Color.Blue), 13 + num * cell, (130 - 11 * int.Parse(string.IsNullOrEmpty(findcs.RackLevel) ? "0" : findcs.RackLevel)) + 88);
                                //strPalletNu = findcs.Palletnu.ToString();
                            }
                            else
                                g.FillRectangle(brush = new SolidBrush(Color.Silver), 25 + num * cell, 141, 30, 22);
                            //状态信息及报警信息
                            if (findcs.LabelInfo != null)
                            {
                                string[] info = findcs.LabelInfo.Split(new char[] { '.' });
                                for (int i = 0; i < info.Length - 1; i++)
                                {
                                    if (i == 4 && info[i].Contains("停"))
                                        g.DrawString(info[i], new Font("宋体", 10, FontStyle.Regular), brush = new SolidBrush(Color.Red), 20 + num * cell, 263 + 20 * i);
                                    else if (i == 3 && (info[i] == "手动操作" || info[i].Contains("未") || info[i] == "状态:报警停机" || info[i] == "状态:Socket断开！"))
                                        g.DrawString(info[i], new Font("宋体", 10, FontStyle.Regular), brush = new SolidBrush(Color.Red), 20 + num * cell, 263 + 20 * i);
                                    else
                                        g.DrawString(info[i], new Font("宋体", 10, FontStyle.Regular), brush = new SolidBrush(Color.Blue), 20 + num * cell, 263 + 20 * i);
                                }
                                //for (int j = 0; j < info[info.Length - 1].Length/8; j++)
                                //{
                                if (!string.IsNullOrEmpty(info[info.Length - 1]) && info[info.Length - 1].ToString() != "无故障")
                                {
                                    string[] alarminfo = info[info.Length - 1].Split('|');
                                    g.DrawString(alarminfo[0], new Font("宋体", 10, FontStyle.Regular), brush = new SolidBrush(Color.Red), 20 + num * cell, 263 + 20 * (info.Length - 1) + 8);
                                }
                                //}
                            }
                        }
                    }
                    brush.Dispose();
                    pen.Dispose();
                    g.Dispose();
                    GC.Collect();
                }
            }
            catch
            {
            }
        }

        private void pic_MouseClick_1(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.X / 41 == 0 && e.Y / 25 == 0)
                {
                    if (taskmode == "自动")
                        bsb.CloseThread();
                    else
                        bsb.ReStartThread(_zone);
                    taskmode = (taskmode == "自动" ? "手动" : "自动");
                }

                if (e.Y > 14 && e.Y < 305)
                {
                    FormSend fs = null;
                    for (int i = 70; i < pic.Width; i += 225)
                    {
                        if (e.X > i && e.X < i + 16 && taskmode == "手动")
                        {
                            fs = new FormSend((i - 70) / 225 + 1,_zone);
                            //fs = new FormSend(1, _zone);
                            fs.ShowDialog();
                        }
                    }
                }
            }
            catch { }
        }
    }
}
