using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenSong.UI
{
    public partial class FormSend : Form
    {
        GenSongSRMRun bsb = new GenSongSRMRun();
        CRNTask crc = new CRNTask();
        //private SCBase scb;
        private GenSongSC scb;
        private string _zone = null;
        public FormSend(int index,string zone)
        {
            InitializeComponent();
            txt_num.Text = "CRN00" + index.ToString();
            _zone = zone;
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            int cmdType = 0;
            //if (rbtn_type.Checked)
            //{
            string strCommandType = cmbCommandType.Text.Trim();
            if (strCommandType == "--请选择--")
            {
                MessageBox.Show("请选择命令！");
                return;
            }

            if (strCommandType == "仅取货")
                cmdType = 1;
            else if (strCommandType == "仅放货")
                cmdType = 2;
            else if (strCommandType == "仅行走")
                cmdType = 3;
            else if (strCommandType == "紧急停止")
                cmdType = 4;
            else if (strCommandType == "取消停止")
                cmdType = 5;
            else if (strCommandType == "发送任务")
                cmdType = 6;
            else if (strCommandType == "回原点")
                cmdType = 7;
            //}
            if (cmdType == 0)
                return;
            //bsb.SendTask(txt_num.Text.Trim(), txt_taskno.Text.Trim(), txt_from.Text.Trim(), txt_to.Text.Trim(), cmdType,_zone);
            //bsb.SendTask(txt_num.Text.Trim(), txt_taskno.Text.Trim(), txt_from.Text.Trim(), txt_to.Text.Trim(), cmdType, _zone);
            bsb.SendTask(txt_num.Text.Trim(), txt_taskno.Text.Trim(), txt_from.Text.Trim(), txt_to.Text.Trim(), txt_from2.Text.Trim(), txt_to2.Text.Trim(), cmdType, _zone);           
            //this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSend_Load(object sender, EventArgs e)
        {
            BindComboBox();
            string[] status = crc.GetCRCStatus(txt_num.Text.Trim(), _zone);
            if (status != null)
            {
                txt_taskno.Text = status[3] == "0" ? "1000" : status[3];
                txt_from.Text = GetAdd("F", txt_num.Text.Trim(), status[4]);
                txt_from2.Text = GetAdd("F", txt_num.Text.Trim(), status[4]);
                txt_to.Text = GetAdd("T", txt_num.Text.Trim(), status[5]);
                txt_to2.Text = GetAdd("T", txt_num.Text.Trim(), status[5]);
            }
        }
        private void BindComboBox()
        {
            string[] type = System.Configuration.ConfigurationSettings.AppSettings["CRNCommandType"].ToString().Split(',');
            if (type == null || type.Length == 0)
                return;
            cmbCommandType.DataSource = type;
            cmbCommandType.SelectedIndex = 0;
        }
        private string GetAdd(string type, string num, string add)
        {
            string address = "";
            if (type == "F")
            {
                switch (num)
                {
                    case "CRN001": address = string.IsNullOrEmpty(add) ? "01001001" : add; break;
                    case "CRN002": address = string.IsNullOrEmpty(add) ? "03001001" : add; break;
                    case "CRN003": address = string.IsNullOrEmpty(add) ? "05001001" : add; break;
                }
            }
            else
            {
                switch (num)
                {
                    case "CRN001": address = string.IsNullOrEmpty(add) ? "01001001" : add; break;
                    case "CRN002": address = string.IsNullOrEmpty(add) ? "03001001" : add; break;
                    case "CRN003": address = string.IsNullOrEmpty(add) ? "05001001" : add; break;
                }
            }
            return address;
        }

        private void cmbCommandType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbCommandType.SelectedValue.ToString() == "修改任务")
            {
                txt_from.Text = "G" + txt_num.Text.Trim().Substring(4, 2);
                txt_from.Enabled = false;
            }
            else if (cmbCommandType.SelectedValue.ToString() == "发送任务")
            {
                string[] status = crc.GetCRCStatus(txt_num.Text.Trim(),_zone);
                if (status != null)
                {
                    txt_from.Enabled = true; ;
                    txt_from.Text = GetAdd("F", txt_num.Text.Trim(), status[4]);
                }
            }
        }
    }
}
