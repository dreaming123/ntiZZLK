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
using DataAccess;
using System.Threading;
using System.Drawing.Drawing2D;
using BLL;

namespace WCS
{
    public partial class FormSRMEquip : Form
    {
        #region 自动关闭弹出框
        //[DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        //private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        //public const int WM_CLOSE = 0x10;

        //private void StartKiller()
        //{
        //    Timer timer = new Timer();
        //    timer.Interval = 500;
        //    timer.Tick += new EventHandler(Timer_Tick);
        //    timer.Start();
        //}

        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    KillMessageBox();
        //    //停止Timer   
        //    ((Timer)sender).Stop();
        //}

        //private void KillMessageBox()
        //{
        //    //依MessageBox的標題,找出MessageBox的視窗   
        //    IntPtr ptr = FindWindow(null, " ");
        //    if (ptr != IntPtr.Zero)
        //    {
        //        //找到則關閉MessageBox視窗   
        //        PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        //    }
        //}
        #endregion

        BLLSRMRun bsb = new BLLSRMRun();
        private delegate void PictureCallBack(List<CRCObject> cob, List<CRCStatus> css);
        private delegate void RichBoxCallBack(RichTextBox rb, string msg);
        string taskmode = "自动";

        public FormSRMEquip()
        {
            InitializeComponent();
        }

        private void FormSTATaskM_Load(object sender, EventArgs e)
        {
            bsb.ENotifyCRCEventHandler += new DNotifyCRC(bsb_ENotifyCRCEventHandler);
            bsb.EDNotify += new BLLSRMRun.DNotify(bsb_EDNotify);
            bsb.Start("A");
        }

        private void bsb_ENotifyCRCEventHandler(List<CRCObject> cob, List<CRCStatus> css, string eNotify, object oNotify)
        {
            try
            {
                DrawConnect(cob, css);
            }
            catch { }
            Thread.Sleep(3000);    
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
                rb.AppendText(System.DateTime.Now.ToString() + ": " + msg + "\n");
        }

        private void DrawConnect(List<CRCObject> cob, List<CRCStatus> css)
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

                foreach (CRCObject cco in cob)
                {
                    CRCStatus ccs = css.Find(
                            delegate(CRCStatus crcs)
                            {
                                return crcs.CRCNum == cco.ScNo && cco.Port == 4000;
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

                foreach (CRCObject co in cob)
                {
                    CRCStatus findcs = css.Find(
                            delegate(CRCStatus info)
                            {
                                return info.CRCNum == co.ScNo && co.Port == 4000;
                            }
                        );
                    if (findcs != null)
                    {
                        int num = int.Parse(findcs.CRCNum.Substring(3, 3)) - 1;
                        int cell = 170;
                        //卫星小车载体
                        g.FillRectangle(brush = new SolidBrush(co.connectstatus == 1 ? Color.Green : Color.Gray), 20 + num * cell, 311, 120, 14);
                        //卫星小车滚轮
                        g.DrawEllipse(pen = new Pen(brush = new SolidBrush(Color.Black), 1), 25 + num * cell, 317, 9, 9);
                        g.FillEllipse(brush = new SolidBrush(Color.Black), 25 + num * cell, 317, 9, 9);
                        g.DrawEllipse(pen = new Pen(brush = new SolidBrush(Color.Black), 1), 125 + num * cell, 317, 9, 9);
                        g.FillEllipse(brush = new SolidBrush(Color.Black), 125 + num * cell, 317, 9, 9);
                        //卫星小车主臂1
                        g.FillRectangle(brush = new SolidBrush(co.connectstatus == 1 ? Color.Green : Color.Gray), 70 + num * cell, 15, 16, 304);
                        //卫星小车电柜2
                        g.FillRectangle(brush = new SolidBrush(Color.SlateGray), 100 + num * cell, 251, 22, 60);
                        g.DrawString(findcs.CRCNum.Substring(3, 3), new Font("宋体", 10, FontStyle.Bold), brush = new SolidBrush(Color.Blue), 60 + num * cell, 309);
                        //卫星小车爬梯3456
                        for (int i = 15; i < 303; i = i + 11)
                        {
                            g.DrawRectangle(pen = new Pen(brush = new SolidBrush(Color.SlateGray), 1), 87 + num * cell, i, 10, 10);
                        }
                        //爬梯与电柜的连接体7
                        g.DrawLine(pen = new Pen(brush = new SolidBrush(Color.Black), 4), 85 + num * cell, 251, 100 + num * cell, 251);
                        //卫星小车货叉
                        g.DrawRectangle(pen = new Pen(brush = new SolidBrush(Color.Black), 4), 22 + num * cell, 300, 114, 3);//(200 - 25 * int.Parse(findcs.RackLevel)) + 150
                        g.DrawRectangle(pen = new Pen(brush = new SolidBrush(Color.Black), 4), 133 + num * cell, 290, 3, 10);
                        g.DrawRectangle(pen = new Pen(brush = new SolidBrush(Color.Black), 4), 22 + num * cell, 290, 3, 10);
                       // g.DrawRectangle(pen = new Pen(brush = new SolidBrush(Color.Black), 4), 60 + num * cell, (200 - 25 * int.Parse(findcs.RackLevel)) + 100, 20, 4);

                        //货物及任务号
                        if (findcs.TaskNo != "0")
                        {
                            if (int.Parse(findcs.FunctionReport) > 120)
                                g.FillRectangle(brush = new SolidBrush(Color.Chocolate), 29 + num * cell, (200 - 25 * int.Parse(findcs.RackLevel)) + 65, 100, 60);
                            g.DrawString(findcs.TaskNo.ToString(), new Font("宋体", 10, FontStyle.Bold), brush = new SolidBrush(Color.Blue), 62 + num * cell, (200 - 25 * int.Parse(findcs.RackLevel)) + 110);
                        }
                        else
                            g.FillRectangle(brush = new SolidBrush(Color.Silver), 25 + num * cell, (200 - 25 * int.Parse(findcs.RackLevel)) + 85, 30, 22);
                        //状态信息及报警信息
                        if (findcs.LabelInfo != null)
                        {
                            string[] info = findcs.LabelInfo.Split(new char[] { '.' });
                            for (int i = 0; i < 5; i++)
                            {
                                //g.DrawString(info[i], new Font("宋体", 10, FontStyle.Regular), brush = new SolidBrush(Color.Blue), 20 + num * cell, 333 + 20 * i);
                                if (i != 2)
                                    g.DrawString(info[i], new Font("宋体", 9, FontStyle.Regular), brush = new SolidBrush(Color.Blue), 20 + num * cell, 333 + 20 * i);
                                else
                                {
                                    if (info[2] != "模式:自动;就绪;正常;远程;")
                                        g.DrawString(info[i], new Font("宋体", 9, FontStyle.Regular), brush = new SolidBrush(Color.OrangeRed), 20 + num * cell, 333 + 20 * i);
                                    else
                                        g.DrawString(info[i], new Font("宋体", 9, FontStyle.Regular), brush = new SolidBrush(Color.Blue), 20 + num * cell, 333 + 20 * i);
                                }
                            }
                            if (info.Length > 5)
                            {
                                for (int j = 0; j < info.Length-5; j++)
                                {
                                    g.DrawString(info[5+j], new Font("宋体", 9, FontStyle.Bold), brush = new SolidBrush(Color.Red), 20 + num * cell, 433 + 15 * j);
                                }
                                //for (int j = 0; j < info[info.Length - 1].Length / 15; j++)
                                //{
                                //    g.DrawString(info[info.Length - 1].Substring(j * 15, 15), new Font("宋体", 10, FontStyle.Bold), brush = new SolidBrush(Color.Red), 20 + num * cell, 333 + 20 * (info.Length - 1) + 15 * j);
                                //}
                            }
                        }
                    }
                }

                brush.Dispose();
                pen.Dispose();
                g.Dispose();
                GC.Collect();
            }
        }

        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.X / 41 == 0 && e.Y / 25 == 0)
                {
                    if (taskmode == "自动")
                        bsb.CloseThread();
                    else
                        bsb.ReStartThread();
                    taskmode = (taskmode == "自动" ? "手动" : "自动");
                }

                //if (e.Y > 290 && e.Y < 433)
                if (e.Y > 14 && e.Y < 433)
                {
                    FormSend fs = null;
                    for (int i = 22; i < 1400; i += 170)
                    {
                        if (e.X > i && e.X < i + 114 && taskmode == "手动")
                        {
                            fs = new FormSend((i - 22) / 170 + 1);
                            fs.ShowDialog();
                        }
                    }
                }
            }
            catch { }
        }
    }
}
