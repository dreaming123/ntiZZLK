namespace WCS
{
    partial class FormPickTaskEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbb_runstatus = new System.Windows.Forms.ComboBox();
            this.txt_taskno = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.txt_Itemname = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_Itemcode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_Requestno = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbb_runstatus
            // 
            this.cbb_runstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_runstatus.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbb_runstatus.ForeColor = System.Drawing.Color.Blue;
            this.cbb_runstatus.FormattingEnabled = true;
            this.cbb_runstatus.Items.AddRange(new object[] {
            "未开始",
            "执行中",
            "已完成"});
            this.cbb_runstatus.Location = new System.Drawing.Point(128, 158);
            this.cbb_runstatus.Name = "cbb_runstatus";
            this.cbb_runstatus.Size = new System.Drawing.Size(170, 24);
            this.cbb_runstatus.TabIndex = 8;
            // 
            // txt_taskno
            // 
            this.txt_taskno.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_taskno.ForeColor = System.Drawing.Color.Blue;
            this.txt_taskno.Location = new System.Drawing.Point(128, 11);
            this.txt_taskno.Name = "txt_taskno";
            this.txt_taskno.ReadOnly = true;
            this.txt_taskno.Size = new System.Drawing.Size(170, 26);
            this.txt_taskno.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(22, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 19);
            this.label3.TabIndex = 10;
            this.label3.Text = "执行状态：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = "出库顺序号：";
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(38, 196);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(100, 30);
            this.btn_ok.TabIndex = 12;
            this.btn_ok.Text = "确定(&S)";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(161, 196);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(100, 30);
            this.btn_cancel.TabIndex = 12;
            this.btn_cancel.Text = "取消(&C)";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // txt_Itemname
            // 
            this.txt_Itemname.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_Itemname.ForeColor = System.Drawing.Color.Blue;
            this.txt_Itemname.Location = new System.Drawing.Point(128, 48);
            this.txt_Itemname.Name = "txt_Itemname";
            this.txt_Itemname.ReadOnly = true;
            this.txt_Itemname.Size = new System.Drawing.Size(170, 26);
            this.txt_Itemname.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 19);
            this.label2.TabIndex = 13;
            this.label2.Text = " 物料名称：";
            // 
            // txt_Itemcode
            // 
            this.txt_Itemcode.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_Itemcode.ForeColor = System.Drawing.Color.Blue;
            this.txt_Itemcode.Location = new System.Drawing.Point(128, 85);
            this.txt_Itemcode.Name = "txt_Itemcode";
            this.txt_Itemcode.ReadOnly = true;
            this.txt_Itemcode.Size = new System.Drawing.Size(170, 26);
            this.txt_Itemcode.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(12, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 19);
            this.label4.TabIndex = 15;
            this.label4.Text = " 品牌编码：";
            // 
            // txt_Requestno
            // 
            this.txt_Requestno.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_Requestno.ForeColor = System.Drawing.Color.Blue;
            this.txt_Requestno.Location = new System.Drawing.Point(128, 121);
            this.txt_Requestno.Name = "txt_Requestno";
            this.txt_Requestno.ReadOnly = true;
            this.txt_Requestno.Size = new System.Drawing.Size(170, 26);
            this.txt_Requestno.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(12, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 19);
            this.label5.TabIndex = 17;
            this.label5.Text = "要料顺序号：";
            // 
            // FormPickTaskEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 239);
            this.Controls.Add(this.txt_Requestno);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_Itemcode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_Itemname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.cbb_runstatus);
            this.Controls.Add(this.txt_taskno);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPickTaskEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "任务修改";
            this.Load += new System.EventHandler(this.FormPickTaskEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbb_runstatus;
        private System.Windows.Forms.TextBox txt_taskno;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.TextBox txt_Itemname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_Itemcode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_Requestno;
        private System.Windows.Forms.Label label5;
    }
}