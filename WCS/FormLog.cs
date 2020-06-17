using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BLL;
using DataGridViewAutoFilter;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace WCS
{
    public partial class FormLog : Form
    {
        private BLLConvery tm = new BLLConvery();
        private int pageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["pagesize"]);    //每页显示行数
        private int nMax = 0;         //总记录数
        private int pageCount = 1;    //页数＝总记录数/每页显示行数
        private int pageCurrent = 1;   //当前页号
        private int nCurrent = 0;      //当前记录行
        private int gv_index = 0;
        DataSet ds = new DataSet();
        DataTable dtInfo = new DataTable();

        #region 自动关闭弹出框
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public const int WM_CLOSE = 0x10;

        private void StartKiller()
        {
            Timer timer = new Timer();
            timer.Interval = 500;
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            KillMessageBox();
            //停止Timer   
            ((Timer)sender).Stop();
        }

        private void KillMessageBox()
        {
            //依MessageBox的標題,找出MessageBox的視窗   
            IntPtr ptr = FindWindow(null, "提示");
            if (ptr != IntPtr.Zero)
            {
                //找到則關閉MessageBox視窗   
                PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }
        #endregion

        private bool mb_YesNo(string str, string tt)
        {
            if (MessageBox.Show(str, tt, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { return true; }
            else
            { return false; }
        }

        public FormLog()
        {
            InitializeComponent();
        }

        private void formTaskManager_Load(object sender, EventArgs e)
        {
            BindData(0);
            dtp_updatestart.Value = DateTime.ParseExact("00:00:00", "HH:mm:ss", null);
            dtp_updateend.Value = DateTime.ParseExact("23:59:59", "HH:mm:ss", null);
        }
        private void BindData(int load)
        {
            if (load == 1)
            {
                dtInfo = tm.GetEventLog(txt_key.Text.ToString(), dtp_updatestart.Value, dtp_updateend.Value, cb_choose.Checked ? 1 : 0);
                if (dtInfo != null)
                {
                    nMax = dtInfo.Rows.Count;
                    pageCount = (nMax / pageSize);
                    if ((nMax % pageSize) > 0)
                    { pageCount++; }
                    pageCurrent = 1;
                    nCurrent = 0;
                }
            }
            if (dtInfo != null)
            {
                int nStartPos = 1;
                int nEndPos = 0;
                DataTable dtTemp = dtInfo.Clone();
                if (nMax > 0)
                {
                    if (pageCurrent == pageCount)
                    {
                        nEndPos = nMax;
                    }
                    else
                    {
                        nEndPos = pageSize * pageCurrent;
                    }
                    txtCurrentPage.Text = Convert.ToString(pageCurrent);
                }
                else
                {
                    nEndPos = 0;
                    txtCurrentPage.Text = "0";
                }
                nCurrent = pageSize * (pageCurrent - 1);
                nStartPos = nCurrent;
                lblPageCount.Text = "/ " + pageCount.ToString();

                //从元数据源复制记录行
                for (int i = nStartPos; i < nEndPos; i++)
                {
                    dtTemp.ImportRow(dtInfo.Rows[i]);
                    nCurrent++;
                }
                bdsInfo.DataSource = dtTemp;
                bdnInfo.BindingSource = bdsInfo;
                gv_taskmanager.DataSource = bdsInfo;
            }
        }

        private void tsmi_copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(Clipboard.GetDataObject());
        }

        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (pageCount > 1)
            {
                if (e.ClickedItem.Text == "上一页")
                {
                    pageCurrent--;
                    if (pageCurrent <= 0)
                    {
                        pageCurrent++;
                        return;
                    }
                    else
                    {
                        DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(gv_taskmanager);
                        BindData(0);
                    }
                }
                if (e.ClickedItem.Text == "下一页")
                {
                    pageCurrent++;
                    if (pageCurrent > pageCount)
                    {
                        pageCurrent--;
                        return;
                    }
                    else
                    {
                        DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(gv_taskmanager);
                        BindData(0);
                    }
                }
                if (e.ClickedItem.Text == "首页")
                {
                    pageCurrent = 1;
                    DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(gv_taskmanager);
                    BindData(0);
                }
                if (e.ClickedItem.Text == "末页")
                {
                    pageCurrent = pageCount;
                    DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(gv_taskmanager);
                    BindData(0);
                }
            }
            if (e.ClickedItem.Text == "刷新")
            {
                DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(gv_taskmanager);
                BindData(1);
            }
        }

        private void gv_taskmanager_BindingContextChanged(object sender, EventArgs e)
        {
            if (gv_taskmanager.DataSource == null)
            { return; }
            foreach (DataGridViewColumn col in gv_taskmanager.Columns)
            {
                col.HeaderCell = new DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell);
            }
            gv_taskmanager.AutoResizeColumns();
        }

        private void gv_taskmanager_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            string filterstatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(gv_taskmanager);
            filterstatus = filterstatus.Replace("of", "条数据被找到,共");
            toolStripTextBox1.Text = filterstatus.Replace("records found", "条");
        }

        private void gv_taskmanager_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                bool mouseOver = e.CellBounds.Contains(gv_taskmanager.PointToClient(Cursor.Position));
                LinearGradientBrush brush = new LinearGradientBrush(
                   e.CellBounds,
                   mouseOver ? Color.PeachPuff : Color.LightGray,
                   Color.LightBlue,
                   LinearGradientMode.Vertical);

                using (brush)
                {
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    Rectangle border = e.CellBounds;
                    border.Width -= 1;
                    e.Graphics.DrawRectangle(Pens.Gray, border);
                }

                e.PaintContent(e.CellBounds);
                e.Handled = true;
            }
            else
            {
                object prevIndex = gv_taskmanager.Tag;
                if ((prevIndex == null || !prevIndex.Equals(e.RowIndex)) && this.gv_taskmanager[2, e.RowIndex].Value.ToString() == "0")
                {
                    gv_taskmanager.Tag = e.RowIndex;
                    gv_taskmanager.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }

        private void gv_taskmanager_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                gv_taskmanager.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, ((pageSize * (pageCurrent - 1)) + (e.RowIndex + 1)).ToString(),
                gv_taskmanager.RowHeadersDefaultCellStyle.Font,
                rectangle,
                gv_taskmanager.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(gv_taskmanager);
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            BindData(1);
        }

        private void cb_choose_CheckedChanged(object sender, EventArgs e)
        {
            BindData(1);
        }

        private void txt_key_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BindData(1);
        }

        private void gv_taskmanager_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Clipboard.SetDataObject(gv_taskmanager[e.ColumnIndex, e.RowIndex].Value.ToString());
            MessageBox.Show(this.gv_taskmanager[0, this.gv_taskmanager.CurrentCell.RowIndex].Value.ToString(), "明细(已复制)");
        }
    }
}