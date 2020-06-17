using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BLL;
using COMResolution;
using System.IO;

namespace WCS
{
    public partial class FormSTAEquip : Form
    {
        private BLLConvery bll = new BLLConvery();
        private delegate void PictureCallBack(List<CRLStationInfo> stationinfo,string moinfo);
        private delegate void LabelCallBack(bool msg);
        private delegate void RichBoxCallBack(string msg);

        private int cell = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["CellWidth"].ToString());
        private int size = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["FontSize"].ToString());
        private string[] stationinfo = null;

        public FormSTAEquip()
        {
            InitializeComponent();
        }

        private void FormSTAEquip_Load(object sender, EventArgs e)
        {
            bll.GetEquipMentInfo += new BLLConvery.GetEquipment_Inquery(bll_GetEquipMentInfo);
            bll.ComputeInquery += new BLLConvery.Compute_Inquery(bll_ComputeInquery);
            bll.Run();
        }

        private void bll_ComputeInquery(string msg)
        {
            ShowText(msg);
        }

        //private void bll_GetEquipMentInfo(List<CRLStationInfo> stationinfo, bool flag, string msg)
        //{
        //    DrawLocation(stationinfo);
        //    bll_Sys_Conn(flag);
        //    ShowText(msg);
        //}
        private void bll_GetEquipMentInfo(List<CRLStationInfo> stationinfo, string msg, string moinfo, string time, int pos)
        {
            DrawLocation(stationinfo, moinfo);
            bll_Sys_Conn(true); //ShowScanTime(time);
            ShowText(msg);
        }

        private void DrawLocation(List<CRLStationInfo> stationinfo,string moinfo)
        {
            if (pic.InvokeRequired)
            {
                PictureCallBack dlgtRichtxb = new PictureCallBack(DrawLocation);
                this.Invoke(dlgtRichtxb, new object[] { stationinfo,moinfo });
            }
            else
            {
                Graphics g = pic.CreateGraphics();
                SolidBrush brush;
                Pen pen;

                g.DrawLine(pen = new Pen(brush = new SolidBrush(Color.Silver), 2), 0, 0, pic.Width, 0);

                for (int i = cell; i < pic.Width; i += cell)
                {
                    g.DrawLine(pen = new Pen(brush = new SolidBrush(Color.Silver), 1), 0, i, pic.Width, i);
                }

                for (int j = 0; j < pic.Width; j += cell)
                {
                    g.DrawLine(pen = new Pen(brush = new SolidBrush(Color.Silver), 1), j, 0, j, pic.Height);
                }

                foreach (CRLStationInfo si in stationinfo)
                {
                    if (si.stationno == si.tostation&&si.stationtype!=0&&si.taskno>0)
                    {
                        g.FillRectangle(brush = new SolidBrush(Color.Chocolate), si.LocationX * cell, si.LocationY * cell, (cell - 1) * si.Width, (cell - 1) * si.Height);
                    }
                    else
                        g.FillRectangle(brush = new SolidBrush(si.taskno == 0 ? Color.Gray : Color.Lime), si.LocationX * cell, si.LocationY * cell, (cell - 1) * si.Width, (cell - 1) * si.Height);
                    g.DrawString(si.stationno.ToString(), new Font("宋体", size, FontStyle.Regular), brush = new SolidBrush(Color.Black), si.LocationX * cell, si.LocationY * cell + cell / 3);
                }

                brush.Dispose();
                pen.Dispose();
                g.Dispose();
                GC.Collect();
            }
        }

        private void bll_Sys_Conn(bool msg)
        {
            if (lb_heartbeat.InvokeRequired)
            {
                LabelCallBack lcb = new LabelCallBack(bll_Sys_Conn);
                this.Invoke(lcb, new object[] { msg });
            }
            else
            {
                if (msg)
                    lb_heartbeat.BackColor = (lb_heartbeat.BackColor == Color.DarkGray ? Color.Lime : Color.DarkGray);
                else
                    lb_heartbeat.BackColor = Color.DimGray;
            }
        }

        private void ShowText(string msg)
        {
            if (rtb_showinfo.InvokeRequired)
            {
                RichBoxCallBack rb = new RichBoxCallBack(ShowText);
                this.Invoke(rb, new object[] { msg });
            }
            else
            {
                if (rtb_showinfo.Text.Length > 1500)
                    rtb_showinfo.Clear();
                if (!System.String.IsNullOrEmpty(msg))
                    rtb_showinfo.AppendText(msg + "\r\n");
            }
        }
        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    //stationinfo = bll.ReadStationInfo(e.X / cell, e.Y / cell);
                    //if (stationinfo != null)
                    //{
                    //    tb_taskno.Text = stationinfo[1] == "" ? "0" : stationinfo[1] == null ? "0" : stationinfo[1];
                    //    tb_goodstype.Text = stationinfo[3] == "" ? "0" : stationinfo[3] == null ? "0" : stationinfo[3];
                    //    tb_barcode1.Text = stationinfo[6] == "" ? "0" : stationinfo[6] == null ? "0" : stationinfo[6];
                    //    tb_barcode2.Text = stationinfo[7] == "" ? "0" : stationinfo[7] == null ? "0" : stationinfo[7];
                    //    tb_fromstation.Text = stationinfo[4] == "" ? "0" : stationinfo[4] == null ? "0" : stationinfo[4];
                    //    tb_tostation.Text = stationinfo[5] == "" ? "0" : stationinfo[5] == null ? "0" : stationinfo[5];
                    //    tb_stationno.Text = stationinfo[0] == "" ? "0" : stationinfo[0] == null ? "0" : stationinfo[0];
                    //    txt_field1.Text = stationinfo[2] == "" ? "0" : stationinfo[2] == null ? "0" : stationinfo[2];
                    //    txt_field2.Text = stationinfo[8] == "" ? "0" : stationinfo[8] == null ? "0" : stationinfo[8];
                    //    txt_field3.Text = stationinfo[9] == "" ? "0" : stationinfo[9] == null ? "0" : stationinfo[9];
                    //    btn_ok.Enabled = true;
                    //}
                }
            }
            catch { }
        }
        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    stationinfo = bll.ReadStationInfo(e.X / cell, e.Y / cell);
                    string weighthead="0",weightbody="0";
                    if (stationinfo != null)
                    {
                        if (stationinfo[9] != "")                
                            weighthead = stationinfo[9];
                        if (stationinfo[7] != "")
                            weightbody = stationinfo[7];
                        tb_taskno.Text = stationinfo[1] == "" ? "0" : stationinfo[1] == null ? "0" : stationinfo[1];
                        tb_goodstype.Text = stationinfo[3] == "" ? "0" : stationinfo[3] == null ? "0" : stationinfo[3];
                        tb_barcode1.Text = stationinfo[6] == "" ? "0" : stationinfo[6] == null ? "0" : stationinfo[6];
                        tb_barcode2.Text = weighthead + "." + weightbody;//stationinfo[9] == "" ? "0" : stationinfo[9] == null ? "0" : stationinfo[9];
                        tb_fromstation.Text = stationinfo[4] == "" ? "0" : stationinfo[4] == null ? "0" : stationinfo[4];
                        tb_tostation.Text = stationinfo[5] == "" ? "0" : stationinfo[5] == null ? "0" : stationinfo[5];
                        tb_stationno.Text = stationinfo[0] == "" ? "0" : stationinfo[0] == null ? "0" : stationinfo[0];
                        txt_field1.Text = stationinfo[2] == "" ? "0" : stationinfo[2] == null ? "0" : stationinfo[2];
                        txt_field2.Text = stationinfo[7] == "" ? "0" : stationinfo[7] == null ? "0" : stationinfo[7];
                        txt_field3.Text = stationinfo[8] == "" ? "0" : stationinfo[8] == null ? "0" : stationinfo[8];
                        btn_ok.Enabled = true;
                        //string filename = ".\\CONFIG.txt";
                        //if (File.Exists(filename))
                        //{
                        //    FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                        //    StreamReader sr = new StreamReader(fs);
                        //    string strline = sr.ReadToEnd();
                        //    if (strline.Contains(tb_stationno.Text))
                        //        btn_ok.Enabled = true;
                        //    else
                        //        btn_ok.Enabled = false;
                        //}
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (MessageBox.Show("确定要转移信息吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (stationinfo != null)
                        {
                            stationinfo = bll.ReadStationInfo(int.Parse(stationinfo[0]));
                            string[] str = new string[11];
                            for (int i = 1; i < 11; i++)
                            { str[i] = "0"; }
                            str[0] = stationinfo[0];
                            //str[10] = stationinfo[10];
                            string[] info = bll.ReadStationInfo(e.X / cell, e.Y / cell);
                            if (info[0]!=str[0])
                            {                                
                                bll.WriteStationInfo(str);//清空旧站台信息
                                str[1] = stationinfo[1];  //taskno
                                str[2] = (int.Parse(stationinfo[6]) / 65536).ToString();
                                str[3] = (int.Parse(stationinfo[6]) % 65536).ToString();
                                str[4] = stationinfo[3];  //goodstype
                                str[5] = stationinfo[4];  //fromstation
                                str[6] = stationinfo[5];  //tostation
                                str[7] = stationinfo[9];  //checkinfo
                                str[8] = stationinfo[7];  //field2
                                str[9] = stationinfo[8];  //field3
                                str[10] = stationinfo[2]; //field1
                                str[0] = info[0];   //stationno
                                //for (int i = 1; i < 11; i++)
                                //    str[i] = stationinfo[i];
                                if (bll.WriteStationInfo(str))
                                {
                                    tb_taskno.Text = stationinfo[1] == "" ? "0" : stationinfo[1] == null ? "0" : stationinfo[1];
                                    tb_goodstype.Text = stationinfo[3] == "" ? "0" : stationinfo[3] == null ? "0" : stationinfo[3];
                                    tb_barcode1.Text = stationinfo[6] == "" ? "0" : stationinfo[6] == null ? "0" : stationinfo[6];
                                    tb_barcode2.Text = (stationinfo[9] == "" ? "0" : stationinfo[9] == null ? "0" : stationinfo[9]) + "." + (stationinfo[7] == "" ? "0" : stationinfo[7] == null ? "0" : stationinfo[7]);
                                    tb_fromstation.Text = stationinfo[4] == "" ? "0" : stationinfo[4] == null ? "0" : stationinfo[4];
                                    tb_tostation.Text = stationinfo[5] == "" ? "0" : stationinfo[5] == null ? "0" : stationinfo[5];
                                    tb_stationno.Text = stationinfo[0] == "" ? "0" : stationinfo[0] == null ? "0" : stationinfo[0];
                                    txt_field1.Text = stationinfo[2] == "" ? "0" : stationinfo[2] == null ? "0" : stationinfo[2];
                                    txt_field2.Text = stationinfo[7] == "" ? "0" : stationinfo[7] == null ? "0" : stationinfo[7];
                                    txt_field3.Text = stationinfo[8] == "" ? "0" : stationinfo[8] == null ? "0" : stationinfo[8];
                                    btn_ok.Enabled = true;
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                if (!System.String.IsNullOrEmpty(tb_stationno.Text))
                {
                    int upbarcode1 = 0;
                    int upbarcode2 = 0;
                    string[] weightvalue = tb_barcode2.Text.Split(new char[] { '.' });
                    if (tb_barcode1.Text.Trim().Length >= 8)
                    {
                        upbarcode1 = int.Parse(tb_barcode1.Text.ToString().Substring(tb_barcode1.Text.ToString().Length - 8, 8)) / 65536;
                        upbarcode2 = int.Parse(tb_barcode1.Text.ToString().Substring(tb_barcode1.Text.ToString().Length - 8, 8)) % 65536;
                    }
                    string[] str = new string[11];
                    str[0] = tb_stationno.Text;
                    str[1] = tb_taskno.Text;
                    //int upbarcode1 = int.Parse(tb_barcode1.Text.ToString().Substring(tb_barcode1.Text.ToString().Length - 8, 8)) / 65536;
                    //int upbarcode2 = int.Parse(tb_barcode1.Text.ToString().Substring(tb_barcode1.Text.ToString().Length - 8, 8)) % 65536;
                    str[2] = upbarcode1.ToString();
                    str[3] = upbarcode2.ToString();
                    str[4] = tb_goodstype.Text;
                    str[5] = tb_fromstation.Text;
                    str[6] = tb_tostation.Text;
                    str[7] = weightvalue[0];//tb_barcode2.Text;//checkinfo
                    str[8] = weightvalue[1];//txt_field2.Text;
                    str[9] = txt_field3.Text;
                    str[10] = txt_field1.Text;
                    //Global.TxtBarcode = tb_barcode1.Text.Trim().ToString();
                    if (bll.WriteStationInfo(str))
                        btn_ok.Enabled = false;
                }
            }
            catch { }
        }
    }
}
