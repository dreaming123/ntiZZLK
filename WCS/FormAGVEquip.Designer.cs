namespace WCS
{
    partial class FormAGVEquip
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gv_taskmanager = new System.Windows.Forms.DataGridView();
            this.taskno = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.wmstaskno = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.fromstation = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.tostation = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.tasktype = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.taskstatus = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.runstatus = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.barcode = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.userid = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.createtime = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.updatetime = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.bdsInfo = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rtb_info = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txt_from = new System.Windows.Forms.TextBox();
            this.btn_addtask = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txt_to = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lb_connect = new System.Windows.Forms.Label();
            this.txt_tosend = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_sendcode = new System.Windows.Forms.Button();
            this.txt_fromsend = new System.Windows.Forms.TextBox();
            this.btn_auto = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gv_taskmanager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsInfo)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gv_taskmanager);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1150, 406);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "任务";
            // 
            // gv_taskmanager
            // 
            this.gv_taskmanager.AllowUserToAddRows = false;
            this.gv_taskmanager.AllowUserToDeleteRows = false;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.gv_taskmanager.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle17;
            this.gv_taskmanager.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gv_taskmanager.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gv_taskmanager.BackgroundColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gv_taskmanager.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.gv_taskmanager.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gv_taskmanager.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.taskno,
            this.wmstaskno,
            this.fromstation,
            this.tostation,
            this.tasktype,
            this.taskstatus,
            this.runstatus,
            this.barcode,
            this.userid,
            this.createtime,
            this.updatetime});
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gv_taskmanager.DefaultCellStyle = dataGridViewCellStyle19;
            this.gv_taskmanager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gv_taskmanager.Location = new System.Drawing.Point(3, 17);
            this.gv_taskmanager.MultiSelect = false;
            this.gv_taskmanager.Name = "gv_taskmanager";
            this.gv_taskmanager.ReadOnly = true;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gv_taskmanager.RowHeadersDefaultCellStyle = dataGridViewCellStyle20;
            this.gv_taskmanager.RowTemplate.Height = 23;
            this.gv_taskmanager.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gv_taskmanager.Size = new System.Drawing.Size(1144, 386);
            this.gv_taskmanager.TabIndex = 42;
            this.gv_taskmanager.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.gv_taskmanager_CellPainting);
            this.gv_taskmanager.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.gv_taskmanager_RowPostPaint);
            this.gv_taskmanager.BindingContextChanged += new System.EventHandler(this.gv_taskmanager_BindingContextChanged);
            // 
            // taskno
            // 
            this.taskno.DataPropertyName = "taskno";
            this.taskno.HeaderText = "任务编号";
            this.taskno.Name = "taskno";
            this.taskno.ReadOnly = true;
            this.taskno.Width = 96;
            // 
            // wmstaskno
            // 
            this.wmstaskno.DataPropertyName = "wmstaskno";
            this.wmstaskno.HeaderText = "WMS任务号";
            this.wmstaskno.Name = "wmstaskno";
            this.wmstaskno.ReadOnly = true;
            this.wmstaskno.Width = 102;
            // 
            // fromstation
            // 
            this.fromstation.DataPropertyName = "fromstation";
            this.fromstation.HeaderText = "起始地址";
            this.fromstation.Name = "fromstation";
            this.fromstation.ReadOnly = true;
            this.fromstation.Width = 96;
            // 
            // tostation
            // 
            this.tostation.DataPropertyName = "tostation";
            this.tostation.HeaderText = "目标地址";
            this.tostation.Name = "tostation";
            this.tostation.ReadOnly = true;
            this.tostation.Width = 96;
            // 
            // tasktype
            // 
            this.tasktype.DataPropertyName = "tasktype";
            this.tasktype.HeaderText = "任务类型";
            this.tasktype.Name = "tasktype";
            this.tasktype.ReadOnly = true;
            this.tasktype.Width = 96;
            // 
            // taskstatus
            // 
            this.taskstatus.DataPropertyName = "taskstatus";
            this.taskstatus.HeaderText = "任务状态";
            this.taskstatus.Name = "taskstatus";
            this.taskstatus.ReadOnly = true;
            this.taskstatus.Width = 96;
            // 
            // runstatus
            // 
            this.runstatus.DataPropertyName = "runstatus";
            this.runstatus.HeaderText = "执行状态";
            this.runstatus.Name = "runstatus";
            this.runstatus.ReadOnly = true;
            this.runstatus.Width = 96;
            // 
            // barcode
            // 
            this.barcode.DataPropertyName = "barcode";
            this.barcode.HeaderText = "烟包条码";
            this.barcode.Name = "barcode";
            this.barcode.ReadOnly = true;
            this.barcode.Width = 96;
            // 
            // userid
            // 
            this.userid.DataPropertyName = "userid";
            this.userid.HeaderText = "创建方式";
            this.userid.Name = "userid";
            this.userid.ReadOnly = true;
            this.userid.Width = 96;
            // 
            // createtime
            // 
            this.createtime.DataPropertyName = "createtime";
            this.createtime.HeaderText = "创建时间";
            this.createtime.Name = "createtime";
            this.createtime.ReadOnly = true;
            this.createtime.Width = 96;
            // 
            // updatetime
            // 
            this.updatetime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.updatetime.DataPropertyName = "updatetime";
            this.updatetime.HeaderText = "更新时间";
            this.updatetime.Name = "updatetime";
            this.updatetime.ReadOnly = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rtb_info);
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 406);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1150, 294);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "信息";
            // 
            // rtb_info
            // 
            this.rtb_info.BackColor = System.Drawing.Color.Silver;
            this.rtb_info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_info.Location = new System.Drawing.Point(3, 17);
            this.rtb_info.Name = "rtb_info";
            this.rtb_info.Size = new System.Drawing.Size(945, 274);
            this.rtb_info.TabIndex = 63;
            this.rtb_info.Text = "";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(948, 17);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(199, 274);
            this.panel2.TabIndex = 62;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txt_from);
            this.groupBox4.Controls.Add(this.btn_addtask);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.txt_to);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(197, 109);
            this.groupBox4.TabIndex = 45;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "添加";
            // 
            // txt_from
            // 
            this.txt_from.Location = new System.Drawing.Point(74, 20);
            this.txt_from.Name = "txt_from";
            this.txt_from.Size = new System.Drawing.Size(80, 21);
            this.txt_from.TabIndex = 41;
            // 
            // btn_addtask
            // 
            this.btn_addtask.Location = new System.Drawing.Point(74, 71);
            this.btn_addtask.Name = "btn_addtask";
            this.btn_addtask.Size = new System.Drawing.Size(80, 32);
            this.btn_addtask.TabIndex = 9;
            this.btn_addtask.Text = "添加(&A)";
            this.btn_addtask.UseVisualStyleBackColor = true;
            this.btn_addtask.Click += new System.EventHandler(this.btn_addtask_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(9, 46);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 12);
            this.label16.TabIndex = 44;
            this.label16.Text = "目标地址:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(9, 23);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(59, 12);
            this.label17.TabIndex = 42;
            this.label17.Text = "起始地址:";
            // 
            // txt_to
            // 
            this.txt_to.Location = new System.Drawing.Point(74, 44);
            this.txt_to.Name = "txt_to";
            this.txt_to.Size = new System.Drawing.Size(80, 21);
            this.txt_to.TabIndex = 43;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_auto);
            this.groupBox3.Controls.Add(this.txt_fromsend);
            this.groupBox3.Controls.Add(this.btn_sendcode);
            this.groupBox3.Controls.Add(this.lb_connect);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.txt_tosend);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox3.Location = new System.Drawing.Point(0, 109);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(197, 163);
            this.groupBox3.TabIndex = 45;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "状态";
            // 
            // lb_connect
            // 
            this.lb_connect.BackColor = System.Drawing.Color.Lime;
            this.lb_connect.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_connect.Location = new System.Drawing.Point(32, 17);
            this.lb_connect.Name = "lb_connect";
            this.lb_connect.Size = new System.Drawing.Size(122, 23);
            this.lb_connect.TabIndex = 0;
            this.lb_connect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_tosend
            // 
            this.txt_tosend.Location = new System.Drawing.Point(74, 110);
            this.txt_tosend.Name = "txt_tosend";
            this.txt_tosend.Size = new System.Drawing.Size(80, 21);
            this.txt_tosend.TabIndex = 43;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 42;
            this.label3.Text = "起始地址:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 44;
            this.label4.Text = "目标地址:";
            // 
            // btn_sendcode
            // 
            this.btn_sendcode.Enabled = false;
            this.btn_sendcode.Location = new System.Drawing.Point(74, 137);
            this.btn_sendcode.Name = "btn_sendcode";
            this.btn_sendcode.Size = new System.Drawing.Size(80, 22);
            this.btn_sendcode.TabIndex = 9;
            this.btn_sendcode.Text = "发送(&S)";
            this.btn_sendcode.UseVisualStyleBackColor = true;
            // 
            // txt_fromsend
            // 
            this.txt_fromsend.Location = new System.Drawing.Point(74, 86);
            this.txt_fromsend.Name = "txt_fromsend";
            this.txt_fromsend.Size = new System.Drawing.Size(80, 21);
            this.txt_fromsend.TabIndex = 41;
            // 
            // btn_auto
            // 
            this.btn_auto.Location = new System.Drawing.Point(34, 47);
            this.btn_auto.Name = "btn_auto";
            this.btn_auto.Size = new System.Drawing.Size(120, 33);
            this.btn_auto.TabIndex = 45;
            this.btn_auto.Text = "自动(&A)";
            this.btn_auto.UseVisualStyleBackColor = true;
            this.btn_auto.Click += new System.EventHandler(this.btn_auto_Click);
            // 
            // FormAGVEquip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(1150, 700);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormAGVEquip";
            this.Text = "AGV";
            this.Load += new System.EventHandler(this.FormAGVEquip_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gv_taskmanager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsInfo)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.BindingSource bdsInfo;
        private System.Windows.Forms.DataGridView gv_taskmanager;
        private System.Windows.Forms.GroupBox groupBox2;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn taskno;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn wmstaskno;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn fromstation;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn tostation;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn tasktype;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn taskstatus;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn runstatus;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn barcode;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn userid;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn createtime;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn updatetime;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_addtask;
        private System.Windows.Forms.TextBox txt_to;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txt_from;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.RichTextBox rtb_info;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lb_connect;
        private System.Windows.Forms.TextBox txt_fromsend;
        private System.Windows.Forms.Button btn_sendcode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_tosend;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_auto;
    }
}