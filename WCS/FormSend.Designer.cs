namespace WCS
{
    partial class FormSend
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
            this.rb_sendtask = new System.Windows.Forms.RadioButton();
            this.rb_sendcode = new System.Windows.Forms.RadioButton();
            this.lb_from = new System.Windows.Forms.Label();
            this.txt_from = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_to = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_num = new System.Windows.Forms.TextBox();
            this.btn_send = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.lb_taskno = new System.Windows.Forms.Label();
            this.txt_taskno = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rb_sendtask
            // 
            this.rb_sendtask.AutoSize = true;
            this.rb_sendtask.Checked = true;
            this.rb_sendtask.Location = new System.Drawing.Point(12, 12);
            this.rb_sendtask.Name = "rb_sendtask";
            this.rb_sendtask.Size = new System.Drawing.Size(71, 16);
            this.rb_sendtask.TabIndex = 0;
            this.rb_sendtask.TabStop = true;
            this.rb_sendtask.Tag = "0";
            this.rb_sendtask.Text = "发送任务";
            this.rb_sendtask.UseVisualStyleBackColor = true;
            this.rb_sendtask.CheckedChanged += new System.EventHandler(this.rb_sendtask_CheckedChanged);
            // 
            // rb_sendcode
            // 
            this.rb_sendcode.AutoSize = true;
            this.rb_sendcode.Location = new System.Drawing.Point(126, 12);
            this.rb_sendcode.Name = "rb_sendcode";
            this.rb_sendcode.Size = new System.Drawing.Size(71, 16);
            this.rb_sendcode.TabIndex = 1;
            this.rb_sendcode.Tag = "1";
            this.rb_sendcode.Text = "发送指令";
            this.rb_sendcode.UseVisualStyleBackColor = true;
            this.rb_sendcode.CheckedChanged += new System.EventHandler(this.rb_sendcode_CheckedChanged);
            // 
            // lb_from
            // 
            this.lb_from.AutoSize = true;
            this.lb_from.Location = new System.Drawing.Point(12, 98);
            this.lb_from.Name = "lb_from";
            this.lb_from.Size = new System.Drawing.Size(59, 12);
            this.lb_from.TabIndex = 1;
            this.lb_from.Text = "起始地址:";
            // 
            // txt_from
            // 
            this.txt_from.Location = new System.Drawing.Point(89, 95);
            this.txt_from.Name = "txt_from";
            this.txt_from.Size = new System.Drawing.Size(137, 21);
            this.txt_from.TabIndex = 4;
            this.txt_from.Tag = "4";
            this.txt_from.Text = "201241";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "目标地址:";
            // 
            // txt_to
            // 
            this.txt_to.Location = new System.Drawing.Point(89, 122);
            this.txt_to.Name = "txt_to";
            this.txt_to.Size = new System.Drawing.Size(137, 21);
            this.txt_to.TabIndex = 5;
            this.txt_to.Tag = "5";
            this.txt_to.Text = "101241";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "堆垛机编号:";
            // 
            // txt_num
            // 
            this.txt_num.Enabled = false;
            this.txt_num.Location = new System.Drawing.Point(89, 41);
            this.txt_num.Name = "txt_num";
            this.txt_num.Size = new System.Drawing.Size(137, 21);
            this.txt_num.TabIndex = 2;
            this.txt_num.Tag = "2";
            this.txt_num.Text = "CRC001";
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(14, 149);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(83, 28);
            this.btn_send.TabIndex = 6;
            this.btn_send.Tag = "6";
            this.btn_send.Text = "发送(&S)";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(143, 149);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(83, 28);
            this.btn_cancel.TabIndex = 7;
            this.btn_cancel.Tag = "7";
            this.btn_cancel.Text = "取消(&C)";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // lb_taskno
            // 
            this.lb_taskno.AutoSize = true;
            this.lb_taskno.Location = new System.Drawing.Point(12, 71);
            this.lb_taskno.Name = "lb_taskno";
            this.lb_taskno.Size = new System.Drawing.Size(47, 12);
            this.lb_taskno.TabIndex = 1;
            this.lb_taskno.Text = "任务号:";
            // 
            // txt_taskno
            // 
            this.txt_taskno.Location = new System.Drawing.Point(89, 68);
            this.txt_taskno.Name = "txt_taskno";
            this.txt_taskno.Size = new System.Drawing.Size(137, 21);
            this.txt_taskno.TabIndex = 3;
            this.txt_taskno.Tag = "3";
            this.txt_taskno.Text = "1000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(13, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = " ";
            // 
            // FormSend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 189);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.txt_to);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_taskno);
            this.Controls.Add(this.lb_taskno);
            this.Controls.Add(this.txt_num);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_from);
            this.Controls.Add(this.lb_from);
            this.Controls.Add(this.rb_sendcode);
            this.Controls.Add(this.rb_sendtask);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSend";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "发送指令";
            this.Load += new System.EventHandler(this.FormSend_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rb_sendtask;
        private System.Windows.Forms.RadioButton rb_sendcode;
        private System.Windows.Forms.Label lb_from;
        private System.Windows.Forms.TextBox txt_from;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_to;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_num;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Label lb_taskno;
        private System.Windows.Forms.TextBox txt_taskno;
        private System.Windows.Forms.Label label3;
    }
}