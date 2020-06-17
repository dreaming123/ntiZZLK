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
    public partial class FormPickTaskM : Form
    {
        private BLLConvery tm = new BLLConvery();
        private int pageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["pagesize"]);    //每页显示行数
        private int nMax = 0;         //总记录数
        private int pageCount = 1;    //页数＝总记录数/每页显示行数
        private int pageCurrent = 1;   //当前页号
        private int nCurrent = 0;      //当前记录行
        private int gv_index = 0;
        string str;
        private bool canOperate;
        //private bool outFirst = true;
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

        public FormPickTaskM()
        {
            InitializeComponent();           
        }

        private void formTaskManager_Load(object sender, EventArgs e)
        {
            dtp_updatestart.Value = DateTime.ParseExact("00:00:00", "HH:mm:ss", null);
            dtp_updateend.Value = DateTime.ParseExact("23:59:59", "HH:mm:ss", null);
            BindData(1);     
        }
        private void BindData(int load)
        {
            if (load == 1)
            {
                dtInfo = tm.GetOutrequest(txt_itemcode.Text, txt_itemname.Text, txt_barcode.Text, dtp_updatestart.Value, dtp_updateend.Value);
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
        private void tsmi_delete_Click(object sender, EventArgs e)
        {
            if (canOperate)
            {
                if (gv_taskmanager.SelectedRows.Count > 0)
                {
                    if (MessageBox.Show("确定要删除任务编号为：" + gv_taskmanager[0, gv_taskmanager.CurrentCell.RowIndex].Value.ToString() + "的任务吗 ?", "删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (tm.DeleteSubTask(gv_taskmanager[0, gv_taskmanager.CurrentCell.RowIndex].Value.ToString()))
                        {
                            BindData(1);
                            MessageBox.Show("删除成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show("删除任务失败，具体原因请查看日志!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
                MessageBox.Show("没有数据可以进行该操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
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

        private void gv_taskmanager_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                canOperate = false;
                if (e.RowIndex >= 0 && e.ColumnIndex > -1)
                {
                    canOperate = true;
                    gv_taskmanager.ClearSelection();
                    gv_taskmanager.Rows[e.RowIndex].Selected = true;
                    gv_index = e.RowIndex;
                    //gv_sctask.CurrentCell = gv_sctask.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                    Clipboard.SetDataObject(this.gv_taskmanager[e.ColumnIndex, e.RowIndex].Value.ToString());
                }
            }
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

        private void btn_addtask_Click(object sender, EventArgs e)
        {
            if (!System.String.IsNullOrEmpty(txt_from.Text.ToString()) && !System.String.IsNullOrEmpty(txt_to.Text.ToString()))
            {
                if (MessageBox.Show("确定要添加吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (tm.AddTask(txt_from.Text.ToString(), txt_to.Text.ToString()))
                    {
                        BindData(1);
                        StartKiller();
                        MessageBox.Show("添加成功!", "提示");
                    }
                    else
                    {
                        StartKiller();
                        MessageBox.Show("添加失败!", "提示");
                    }
                }
            }
            else
            {
                StartKiller();
                MessageBox.Show("请填写完整资料!", "提示");
            }
        }

        private void txt_wmstaskno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BindData(1);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gv_taskmanager.SelectedRows.Count > 0)
            {
                FormPickTaskEdit formpicktaskedit = new FormPickTaskEdit(gv_taskmanager["field3", gv_taskmanager.CurrentCell.RowIndex].Value.ToString(), gv_taskmanager["itemname", gv_taskmanager.CurrentCell.RowIndex].Value.ToString(),
                    gv_taskmanager["itemcode", gv_taskmanager.CurrentCell.RowIndex].Value.ToString(), gv_taskmanager["requestsequence", gv_taskmanager.CurrentCell.RowIndex].Value.ToString(), gv_taskmanager["state", gv_taskmanager.CurrentCell.RowIndex].Value.ToString());

                formpicktaskedit.ShowDialog();

                if (FormPickTaskEdit.ischange == 1)
                {
                    MessageBox.Show("修改成功!", "提示");
                    BindData(1);
                    FormPickTaskEdit.ischange = 0;
                }
                else if (FormPickTaskEdit.ischange == 0)
                    MessageBox.Show("修改失败!", "提示");
            }
        }
        //
        private void rb_glbtask_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
                BindData(1);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            BindData(1);
        }

        private void rb_stv_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
                BindData(1);
        }

       
    }
}