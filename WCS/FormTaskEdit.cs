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
    public partial class FormTaskEdit : Form
    {
        private string _tasktype;
        private string _taskno;
        private int _taskstatus = 0;
        private int _runstatus = 0;
        private string _equipmentnumber;
        public static int ischange = 0;

        private DALDATA dal = new DALDATA();

        public FormTaskEdit(string tasktype, string taskno, string taskstatus, string runstatus,string equipmentnumber)
        {
            InitializeComponent();
            _tasktype = tasktype;
            _taskno = taskno;
            _equipmentnumber = equipmentnumber;
            switch (taskstatus)
            {
                case "未开始": _taskstatus = 0; break;
                case "已完成": _taskstatus = 1; break;
                case "执行中": _taskstatus = 2; break;
            }
            if (runstatus == "可执行")
                _runstatus = 1;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要修改任务" + txt_taskno.Text + "吗?", "修改", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //int num = 2;
                //if (_tasktype == "SC")
                //    num = int.Parse(_equipmentnumber.Substring(2, 1));
                if (cbb_taskstatus.SelectedIndex == 1 && _taskstatus == 0)
                {
                    MessageBox.Show("未开始任务不能直接改成已完成！", "提示");
                    ischange = 2;
                }
                //else if (_tasktype == "GLB")
                //{
                //    MessageBox.Show("不能直接修改主任务！", "提示");
                //    ischange = 2;
                //}
                else
                {
                    if (dal.UpdateTaskStatus(_taskno, _tasktype, cbb_taskstatus.SelectedIndex.ToString(), cbb_runstatus.SelectedIndex.ToString())=="1")
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

        private void FormTaskEdit_Load(object sender, EventArgs e)
        {
            txt_taskno.Text = _taskno;
            cbb_taskstatus.SelectedIndex = _taskstatus;
            cbb_runstatus.SelectedIndex = _runstatus;
        }
    }
}
