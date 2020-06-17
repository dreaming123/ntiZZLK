namespace GenSong.UI
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txt_to = new System.Windows.Forms.TextBox();
            this.txt_from = new System.Windows.Forms.TextBox();
            this.txt_taskno = new System.Windows.Forms.TextBox();
            this.txt_num = new System.Windows.Forms.TextBox();
            this.cmbCommandType = new System.Windows.Forms.ComboBox();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_Send = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbltype = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.txt_to2 = new System.Windows.Forms.TextBox();
            this.txt_from2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txt_to2);
            this.tabPage1.Controls.Add(this.txt_from2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.txt_to);
            this.tabPage1.Controls.Add(this.txt_from);
            this.tabPage1.Controls.Add(this.txt_taskno);
            this.tabPage1.Controls.Add(this.txt_num);
            this.tabPage1.Controls.Add(this.cmbCommandType);
            this.tabPage1.Controls.Add(this.btn_cancel);
            this.tabPage1.Controls.Add(this.btn_Send);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.lbltype);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(315, 328);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "手动指令";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txt_to
            // 
            this.txt_to.Location = new System.Drawing.Point(145, 155);
            this.txt_to.Name = "txt_to";
            this.txt_to.Size = new System.Drawing.Size(100, 21);
            this.txt_to.TabIndex = 11;
            // 
            // txt_from
            // 
            this.txt_from.Location = new System.Drawing.Point(145, 122);
            this.txt_from.Name = "txt_from";
            this.txt_from.Size = new System.Drawing.Size(100, 21);
            this.txt_from.TabIndex = 10;
            // 
            // txt_taskno
            // 
            this.txt_taskno.Location = new System.Drawing.Point(145, 90);
            this.txt_taskno.Name = "txt_taskno";
            this.txt_taskno.Size = new System.Drawing.Size(100, 21);
            this.txt_taskno.TabIndex = 9;
            // 
            // txt_num
            // 
            this.txt_num.Location = new System.Drawing.Point(145, 58);
            this.txt_num.Name = "txt_num";
            this.txt_num.Size = new System.Drawing.Size(100, 21);
            this.txt_num.TabIndex = 8;
            // 
            // cmbCommandType
            // 
            this.cmbCommandType.FormattingEnabled = true;
            this.cmbCommandType.Location = new System.Drawing.Point(145, 27);
            this.cmbCommandType.Name = "cmbCommandType";
            this.cmbCommandType.Size = new System.Drawing.Size(100, 20);
            this.cmbCommandType.TabIndex = 7;
            this.cmbCommandType.SelectedValueChanged += new System.EventHandler(this.cmbCommandType_SelectedValueChanged);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Font = new System.Drawing.Font("SimSun", 10F);
            this.btn_cancel.Location = new System.Drawing.Point(173, 259);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 30);
            this.btn_cancel.TabIndex = 6;
            this.btn_cancel.Text = "取消(&C)";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_Send
            // 
            this.btn_Send.Font = new System.Drawing.Font("SimSun", 10F);
            this.btn_Send.Location = new System.Drawing.Point(59, 258);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(75, 30);
            this.btn_Send.TabIndex = 5;
            this.btn_Send.Text = "发送(&S)";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(53, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "货叉1目标地址：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(53, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "货叉1起始地址：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(77, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "任务号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(77, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "堆垛机：";
            // 
            // lbltype
            // 
            this.lbltype.AutoSize = true;
            this.lbltype.Location = new System.Drawing.Point(67, 30);
            this.lbltype.Name = "lbltype";
            this.lbltype.Size = new System.Drawing.Size(65, 12);
            this.lbltype.TabIndex = 0;
            this.lbltype.Text = "命令类型：";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(323, 354);
            this.tabControl1.TabIndex = 0;
            // 
            // txt_to2
            // 
            this.txt_to2.Location = new System.Drawing.Point(145, 226);
            this.txt_to2.Name = "txt_to2";
            this.txt_to2.Size = new System.Drawing.Size(100, 21);
            this.txt_to2.TabIndex = 15;
            // 
            // txt_from2
            // 
            this.txt_from2.Location = new System.Drawing.Point(145, 190);
            this.txt_from2.Name = "txt_from2";
            this.txt_from2.Size = new System.Drawing.Size(100, 21);
            this.txt_from2.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 230);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "货叉2目标地址：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(52, 199);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "货叉2起始地址：";
            // 
            // FormSend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 331);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormSend";
            this.Text = "GenSong";
            this.Load += new System.EventHandler(this.FormSend_Load);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txt_to;
        private System.Windows.Forms.TextBox txt_from;
        private System.Windows.Forms.TextBox txt_taskno;
        private System.Windows.Forms.TextBox txt_num;
        private System.Windows.Forms.ComboBox cmbCommandType;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbltype;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TextBox txt_to2;
        private System.Windows.Forms.TextBox txt_from2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;

    }
}