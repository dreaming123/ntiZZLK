namespace WCS
{
    partial class FormSRMEquipB
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
            this.pic = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtb_msg = new System.Windows.Forms.RichTextBox();
            this.rtb_conn = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pic
            // 
            this.pic.Dock = System.Windows.Forms.DockStyle.Top;
            this.pic.Location = new System.Drawing.Point(0, 0);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(1093, 513);
            this.pic.TabIndex = 0;
            this.pic.TabStop = false;
            this.pic.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pic_MouseClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rtb_msg);
            this.groupBox1.Controls.Add(this.rtb_conn);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 513);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1093, 187);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作记录";
            // 
            // rtb_msg
            // 
            this.rtb_msg.BackColor = System.Drawing.Color.Silver;
            this.rtb_msg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_msg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_msg.Location = new System.Drawing.Point(544, 17);
            this.rtb_msg.Name = "rtb_msg";
            this.rtb_msg.Size = new System.Drawing.Size(546, 167);
            this.rtb_msg.TabIndex = 3;
            this.rtb_msg.Text = "";
            // 
            // rtb_conn
            // 
            this.rtb_conn.BackColor = System.Drawing.Color.Silver;
            this.rtb_conn.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_conn.Dock = System.Windows.Forms.DockStyle.Left;
            this.rtb_conn.Location = new System.Drawing.Point(3, 17);
            this.rtb_conn.Name = "rtb_conn";
            this.rtb_conn.Size = new System.Drawing.Size(541, 167);
            this.rtb_conn.TabIndex = 2;
            this.rtb_conn.Text = "";
            // 
            // FormSRMEquip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(1093, 700);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pic);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSRMEquip";
            this.Text = "三、四区堆垛机";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormSTATaskM_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pic;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rtb_conn;
        private System.Windows.Forms.RichTextBox rtb_msg;

    }
}