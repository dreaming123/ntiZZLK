using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using LOG;

namespace WCS
{
    public partial class MainForm : Form
    {
        public const int WM_SYSCOMMAND = 0x112;

        public const int SC_MINIMIZE = 0xF020;
        public const int SC_CLOSE = 0xF060;
        public const int SC_RESTORE = 0xF120;

        string form = System.Configuration.ConfigurationSettings.AppSettings["FormStatus"];

        #region [ API: 记事本 ]

        /// <summary>
        /// 传递消息给记事本
        /// </summary>
        [DllImport("User32.DLL")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, string lParam);

        /// <summary>
        /// 查找句柄
        /// </summary>
        [DllImport("User32.DLL")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        /// <summary>
        /// 记事本需要的常量
        /// </summary>
        public const uint WM_SETTEXT = 0x000C;

        #endregion

        public class UserFunction
        {
            public string functionName { get; set; }
            public string className { get; set; }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void tsmiVesion_Click(object sender, EventArgs e)
        {
            FormAbout fm = new FormAbout();
            fm.Show();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
        }

        private void CloseCtiServer()
        {
            this.Close();
            Application.Exit();
        }

        private void HideCtiServer()
        {
            this.Hide();
        }

        private void ShowCtiServer()
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            this.Activate();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCtiServer();
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideCtiServer();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseCtiServer();
        }

        private void notifyWCS_DoubleClick(object sender, EventArgs e)
        {
            ShowCtiServer();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            CloseCtiServer();
        }

        private void tsmiNote_Click(object sender, EventArgs e)
        {
            #region [ 启动计算器 ]

            System.Diagnostics.Process Proc;

            try
            {
                // 启动计算器
                Proc = new System.Diagnostics.Process();
                Proc.StartInfo.FileName = "calc.exe";
                Proc.StartInfo.UseShellExecute = false;
                Proc.StartInfo.RedirectStandardInput = true;
                Proc.StartInfo.RedirectStandardOutput = true;

                Proc.Start();
            }
            catch
            {
                Proc = null;
            }

            #endregion
        }

        private void tsmiCount_Click(object sender, EventArgs e)
        {
            #region [ 启动记事本 ]

            System.Diagnostics.Process Proc;

            try
            {
                // 启动记事本
                Proc = new System.Diagnostics.Process();
                Proc.StartInfo.FileName = "notepad.exe";
                Proc.StartInfo.UseShellExecute = false;
                Proc.StartInfo.RedirectStandardInput = true;
                Proc.StartInfo.RedirectStandardOutput = true;

                Proc.Start();
            }
            catch
            {
                Proc = null;
            }

            #endregion

            #region [ 传递数据给记事本 ]

            if (Proc != null)
            {
                // 调用 API, 传递数据
                while (Proc.MainWindowHandle == IntPtr.Zero)
                {
                    Proc.Refresh();
                }

                IntPtr vHandle = FindWindowEx(Proc.MainWindowHandle, IntPtr.Zero, "Edit", null);

                // 传递数据给记事本
                SendMessage(vHandle, WM_SETTEXT, 0, "");
            }

            #endregion
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string[] l_form = form.Split(new char[] { ',' });
            int index = -1;
            for (int i = 0; i < l_form.Length; i++)
            {
                if (l_form[i] == "1")
                {
                    switch (i)
                    {
                        case 0:
                            FormSTAEquip formsta = new FormSTAEquip();
                            TabControl1.TabPages.Add(formsta);
                            TabControl1.TabPages[formsta].CloseButtonVisible = false;
                            TabControl1.TabPages.set_IndexOf(TabControl1.TabPages[formsta], ++index);
                            break;
                        //case 1:
                        //    GenSong.UI.FromSRMEquip formA = new GenSong.UI.FromSRMEquip("A");
                        //    TabControl1.TabPages.Add(formA);
                        //    TabControl1.TabPages[formA].CloseButtonVisible = false;
                        //    TabControl1.TabPages.set_IndexOf(TabControl1.TabPages[formA], ++index);
                        //    break;
                        case 1:
                            FormSRMEquip formsrm = new FormSRMEquip();
                            TabControl1.TabPages.Add(formsrm);
                            TabControl1.TabPages[formsrm].CloseButtonVisible = false;
                            TabControl1.TabPages.set_IndexOf(TabControl1.TabPages[formsrm], ++index);
                            break;
                        case 2:
                            FormSRMEquipB formsrmb = new FormSRMEquipB();
                            TabControl1.TabPages.Add(formsrmb);
                            TabControl1.TabPages[formsrmb].CloseButtonVisible = false;
                            TabControl1.TabPages.set_IndexOf(TabControl1.TabPages[formsrmb], ++index);
                            break;
                        //case 2:
                        //    FormAGVEquip formagv = new FormAGVEquip();
                        //    TabControl1.TabPages.Add(formagv);
                        //    TabControl1.TabPages[formagv].CloseButtonVisible = false;
                        //    TabControl1.TabPages.set_IndexOf(TabControl1.TabPages[formagv], ++index);
                        //    break;
                        case 3:
                            FormTaskM formglb = new FormTaskM();
                            TabControl1.TabPages.Add(formglb);
                            TabControl1.TabPages[formglb].CloseButtonVisible = false;
                            TabControl1.TabPages.set_IndexOf(TabControl1.TabPages[formglb], ++index);
                            break;
                        case 4:
                            FormLog formlog = new FormLog();
                            TabControl1.TabPages.Add(formlog);
                            TabControl1.TabPages[formlog].CloseButtonVisible = false;
                            TabControl1.TabPages.set_IndexOf(TabControl1.TabPages[formlog], ++index);
                            break;
                        //case 5:
                        //    FormPickTaskM formagv = new FormPickTaskM();
                        //    TabControl1.TabPages.Add(formagv);
                        //    TabControl1.TabPages[formagv].CloseButtonVisible = false;
                        //    TabControl1.TabPages.set_IndexOf(TabControl1.TabPages[formagv], ++index);
                        //    break;
                    }
                }
            }
            if (index > -1)
                TabControl1.TabPages[0].Select();

            this.Text = "华美冷库WCS系统                                  登录时间：" + DateTime.Now.ToString();
        }

        private void TabControl1_GetTabRegion(object sender, MdiTabControl.TabControl.GetTabRegionEventArgs e)
        {
            e.Points[1] = new Point(e.TabHeight - 2, 2);
            e.Points[2] = new Point(e.TabHeight + 2, 0);
        }

        protected override void WndProc(ref   Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                int wParam = (int)m.WParam;
                //拦截 除最小化 恢复 关闭 以外的消息
                if (wParam != SC_MINIMIZE && wParam != SC_CLOSE && wParam != SC_RESTORE)
                {
                    return;
                }
            }
            base.WndProc(ref   m);
        }
    }
}
