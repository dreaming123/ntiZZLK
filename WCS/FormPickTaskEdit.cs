using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAccess;

namespace WCS
{
    public partial class FormPickTaskEdit : Form
    {
        private string _feild3;
        private string _itemname;
        private string _itemcode;
        private string _requestsequence;
        private int _taskstatus = 0;       
        public static int ischange = 0;

        private DALDATA dal = new DALDATA();

        public FormPickTaskEdit(string feild3, string itemname, string itemcode, string requestsequence, string state)
        {
            InitializeComponent();
            _feild3 = feild3;
            _itemname = itemname;
            _itemcode = itemcode;
            _requestsequence = requestsequence;
            switch (state)
            {
                case "New": _taskstatus = 0; break;
                case "Finished": _taskstatus = 1; break;
                case "Executing": _taskstatus = 2; break;
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要修改任务" + txt_taskno.Text + "吗?", "修改", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (cbb_runstatus.SelectedIndex == 1 && _taskstatus == 0)
                {
                    MessageBox.Show("未开始任务不能直接改成已完成！", "提示");
                    ischange = 2;
                }
                else
                {
                    if (dal.UpdateOutrequestState(_requestsequence,_feild3,cbb_runstatus.SelectedIndex.ToString()))
                        ischange = 1;
                }
                this.Close();
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            ischange = 2;
            this.Close();
        }

        private void FormPickTaskEdit_Load(object sender, EventArgs e)
        {
            txt_taskno.Text = _feild3;
            txt_Itemname.Text = _itemname;
            txt_Itemcode.Text = _itemcode;
            txt_Requestno.Text = _requestsequence;
            //cbb_taskstatus.SelectedIndex = _taskstatus;
            cbb_runstatus.SelectedIndex = _taskstatus;
        }
    }
}
