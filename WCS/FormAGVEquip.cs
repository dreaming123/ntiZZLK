using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAccess;
using BLL;
using DataGridViewAutoFilter;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace WCS
{
    public partial class FormAGVEquip : Form
    {
        private DALDATA dal = new DALDATA();
        private BLLAGV agv = new BLLAGV();
        private delegate void GridViewCallBack(DataTable dt);

        public FormAGVEquip()
        {
            InitializeComponent();
        }

        private void FormAGVEquip_Load(object sender, EventArgs e)
        {
            agv.GetConnectStatus += new BLLAGV.Sys_conn(agv_GetConnectStatus);
            agv.GetTask += new BLLAGV.NotifyTask(agv_GetTask);
            agv.AgvRun();
        }

        private void agv_GetConnectStatus(string command, object state)
        {
            if (command == "Connect")
            {
                lb_connect.Text = "NT8000联机";
                lb_connect.BackColor = Color.Lime;
            }
            else
            {
                lb_connect.Text = "NT8000脱机";
                lb_connect.BackColor = Color.Gray;
            }
        }

        private void agv_GetTask(DataTable dt)
        {
            ShowData(dt);
            btn_auto.BackColor = (btn_auto.BackColor == Color.Lime ? Color.Silver : Color.Lime);
        }

        private void ShowData(DataTable dt)
        {
            if (gv_taskmanager.InvokeRequired)
            {
                GridViewCallBack gvcb = new GridViewCallBack(ShowData);
                this.Invoke(gvcb, new object[] { dt });
            }
            else
            {
                bdsInfo.DataSource = dt;
                gv_taskmanager.DataSource = bdsInfo;
            }
        }

        private void btn_addtask_Click(object sender, EventArgs e)
        {
            
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

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                gv_taskmanager.RowHeadersDefaultCellStyle.Font,
                rectangle,
                gv_taskmanager.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void btn_auto_Click(object sender, EventArgs e)
        {
            if (btn_auto.Text == "自动(&A)")
            {
                btn_auto.Text = "手动(&H)";
                btn_auto.BackColor = Color.Silver;
                btn_sendcode.Enabled = true;
                agv.SetTimer(false);
            }
            else
            {
                btn_auto.Text = "自动(&A)";
                btn_sendcode.Enabled = false;
                agv.SetTimer(true);
            }
        }
    }
}
