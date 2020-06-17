using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OracleTest.Demo;
using entity;
using OracleTest.jcz.dao;
namespace JCZ
{
    public partial class Form1 : Form
    {
        test a = new test();
        public Form1()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {

           var db=  OracleDB.GetInstance();
         
           var date=db.Queryable<WCS_TASKINFO>().ToList();

            string ss = date[0].ToString();
           
            foreach (WCS_TASKINFO d in date)
                this.richTextBox1.Text += "\r"+d.TASKID;


           var a =   db.Queryable<WCS_TASKINFO>().First();
            var dt = db.Ado.GetDataTable("select count(1) from WCS_TASKINFO");
            if ( Convert.ToInt32(dt.Rows[0][0])>0)
            {
                this.richTextBox1.Text += "\r" + dt.Rows[0][0];
            }


            this.richTextBox1.Text += "\r" + dt.Rows[0][0];
            WCS_TASKINFO ins = new WCS_TASKINFO();
            ins.TASKID = "444444";
            ins.WAREHOUSEID = "44444";
            db.Insertable(ins).ExecuteCommand();

          
         

        }

        private void button2_Click(object sender, EventArgs e)
        {
            a.getTableEntity();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            var db = OracleDB.GetInstance();
            int test=  db.Ado.GetInt("select  9 from dual");    
           string test2 =  db.Ado.GetString("select  9 from dual");
        }
    }
}
