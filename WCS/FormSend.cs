using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WCS
{
    public partial class FormSend : Form
    {
        BLL.BLLSRMRun bsb = new BLL.BLLSRMRun();
        BLL.BLLSRMRunB bsbb = new BLL.BLLSRMRunB();

        public FormSend(int index)
        {
            InitializeComponent();
            string ihead = "CRC00";
            if (index > 9)
            {
                ihead = "CRC0";
            }
            txt_num.Text = ihead + index.ToString();
            txt_from.Text = "0";
            txt_to.Text = "0";
            //if (txt_num.Text == "CRC001")
            //{
            //    txt_from.Text = "101241";
            //    txt_to.Text = "1077";
            //}
        }

        private void FormSend_Load(object sender, EventArgs e)
        {

        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            int num = int.Parse(txt_num.Text.Replace("CRC", ""));
            if (num>8)
            {
                bsbb.SendTask(txt_num.Text, txt_taskno.Text, txt_from.Text, txt_to.Text, (rb_sendtask.Checked ? 1 : 0));
            }
            else
            {
                bsb.SendTask(txt_num.Text, txt_taskno.Text, txt_from.Text, txt_to.Text, (rb_sendtask.Checked ? 1 : 0));
            }
           // bsb.SendTask(txt_num.Text, txt_taskno.Text, txt_from.Text, txt_to.Text, (rb_sendtask.Checked ? 1 : 0));
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rb_sendcode_CheckedChanged(object sender, EventArgs e)
        {

              
            lb_from.Text = "指令代码:";
            txt_from.Text = "0";
            txt_to.Text = "0";
            //if (txt_num.Text == "CRC001")
            //    txt_to.Text = "1077";
            label3.Text = "指令代码：112(取货)、128(放货)、0";
        }

        private void rb_sendtask_CheckedChanged(object sender, EventArgs e)
        {
            lb_from.Text = "起始地址:";
            label3.Text = "";
            txt_from.Text = "0";
            txt_to.Text = "0";
            //if (txt_num.Text == "CRC001")
            //{
            //    txt_from.Text = "101241";
            //    txt_to.Text = "1077";
            //}
        }
    }
}
