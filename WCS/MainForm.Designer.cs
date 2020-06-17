namespace WCS
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.ToolStripSystemRenderer toolStripSystemRenderer1 = new System.Windows.Forms.ToolStripSystemRenderer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTool = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNote = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCount = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSupport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiVesion = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyWCS = new System.Windows.Forms.NotifyIcon(this.components);
            this.NicontextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.TabControl1 = new MdiTabControl.TabControl();
            this.menuStrip1.SuspendLayout();
            this.NicontextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSystem,
            this.tsmiTool,
            this.tsmiHelp});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1084, 25);
            this.menuStrip1.TabIndex = 35;
            // 
            // tsmiSystem
            // 
            this.tsmiSystem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmiSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiExit});
            this.tsmiSystem.Name = "tsmiSystem";
            this.tsmiSystem.Size = new System.Drawing.Size(59, 21);
            this.tsmiSystem.Text = "系统(&S)";
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(139, 22);
            this.tsmiExit.Text = "退出系统(&E)";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // tsmiTool
            // 
            this.tsmiTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNote,
            this.tsmiCount});
            this.tsmiTool.Name = "tsmiTool";
            this.tsmiTool.Size = new System.Drawing.Size(59, 21);
            this.tsmiTool.Text = "工具(&T)";
            // 
            // tsmiNote
            // 
            this.tsmiNote.Image = ((System.Drawing.Image)(resources.GetObject("tsmiNote.Image")));
            this.tsmiNote.Name = "tsmiNote";
            this.tsmiNote.Size = new System.Drawing.Size(130, 22);
            this.tsmiNote.Text = "计算器(&C)";
            this.tsmiNote.Click += new System.EventHandler(this.tsmiNote_Click);
            // 
            // tsmiCount
            // 
            this.tsmiCount.Image = ((System.Drawing.Image)(resources.GetObject("tsmiCount.Image")));
            this.tsmiCount.Name = "tsmiCount";
            this.tsmiCount.Size = new System.Drawing.Size(130, 22);
            this.tsmiCount.Text = "记事本(&N)";
            this.tsmiCount.Click += new System.EventHandler(this.tsmiCount_Click);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDoc,
            this.tsmiSupport,
            this.tsmiVesion});
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(61, 21);
            this.tsmiHelp.Text = "帮助(&H)";
            // 
            // tsmiDoc
            // 
            this.tsmiDoc.Image = ((System.Drawing.Image)(resources.GetObject("tsmiDoc.Image")));
            this.tsmiDoc.Name = "tsmiDoc";
            this.tsmiDoc.Size = new System.Drawing.Size(141, 22);
            this.tsmiDoc.Text = "帮助文档(&H)";
            // 
            // tsmiSupport
            // 
            this.tsmiSupport.Name = "tsmiSupport";
            this.tsmiSupport.Size = new System.Drawing.Size(141, 22);
            this.tsmiSupport.Text = "技术支持(&C)";
            // 
            // tsmiVesion
            // 
            this.tsmiVesion.Name = "tsmiVesion";
            this.tsmiVesion.Size = new System.Drawing.Size(141, 22);
            this.tsmiVesion.Text = "版本信息(&V)";
            this.tsmiVesion.Click += new System.EventHandler(this.tsmiVesion_Click);
            // 
            // notifyWCS
            // 
            this.notifyWCS.ContextMenuStrip = this.NicontextMenu;
            this.notifyWCS.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyWCS.Icon")));
            this.notifyWCS.Text = "WCS";
            this.notifyWCS.Visible = true;
            this.notifyWCS.DoubleClick += new System.EventHandler(this.notifyWCS_DoubleClick);
            // 
            // NicontextMenu
            // 
            this.NicontextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.hideToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.NicontextMenu.Name = "NicontextMenu";
            this.NicontextMenu.Size = new System.Drawing.Size(95, 70);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.showToolStripMenuItem.Text = "显示";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // hideToolStripMenuItem
            // 
            this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            this.hideToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.hideToolStripMenuItem.Text = "隐藏";
            this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.closeToolStripMenuItem.Text = "退出";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "14.gif");
            this.imageList1.Images.SetKeyName(1, "5.gif");
            this.imageList1.Images.SetKeyName(2, "fleet.gif");
            this.imageList1.Images.SetKeyName(3, "driver.gif");
            this.imageList1.Images.SetKeyName(4, "privilege.gif");
            this.imageList1.Images.SetKeyName(5, "person.gif");
            this.imageList1.Images.SetKeyName(6, "password.gif");
            this.imageList1.Images.SetKeyName(7, "role.gif");
            this.imageList1.Images.SetKeyName(8, "x30.gif");
            this.imageList1.Images.SetKeyName(9, "x12.gif");
            this.imageList1.Images.SetKeyName(10, "x3.gif");
            this.imageList1.Images.SetKeyName(11, "user.gif");
            this.imageList1.Images.SetKeyName(12, "vehicle.gif");
            this.imageList1.Images.SetKeyName(13, "x25.gif");
            this.imageList1.Images.SetKeyName(14, "x32.gif");
            this.imageList1.Images.SetKeyName(15, "oil.gif");
            this.imageList1.Images.SetKeyName(16, "routine.gif");
            this.imageList1.Images.SetKeyName(17, "unit.gif");
            this.imageList1.Images.SetKeyName(18, "unitGroup.gif");
            this.imageList1.Images.SetKeyName(19, "x4.gif");
            this.imageList1.Images.SetKeyName(20, "x5.gif");
            this.imageList1.Images.SetKeyName(21, "x7.gif");
            this.imageList1.Images.SetKeyName(22, "x16.gif");
            this.imageList1.Images.SetKeyName(23, "x19.gif");
            this.imageList1.Images.SetKeyName(24, "x21.gif");
            this.imageList1.Images.SetKeyName(25, "x22.gif");
            this.imageList1.Images.SetKeyName(26, "x29.gif");
            this.imageList1.Images.SetKeyName(27, "scNew.bmp");
            // 
            // TabControl1
            // 
            this.TabControl1.Alignment = MdiTabControl.TabControl.TabAlignment.Top;
            this.TabControl1.AutoScroll = true;
            this.TabControl1.BackColor = System.Drawing.SystemColors.Control;
            this.TabControl1.CausesValidation = false;
            this.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl1.Location = new System.Drawing.Point(0, 25);
            this.TabControl1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.TabControl1.MenuRenderer = toolStripSystemRenderer1;
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.TabControl1.Size = new System.Drawing.Size(1084, 504);
            this.TabControl1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.TabControl1.TabBackHighColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.TabControl1.TabBackLowColorDisabled = System.Drawing.SystemColors.GradientInactiveCaption;
            this.TabControl1.TabBorderEnhanced = true;
            this.TabControl1.TabBorderEnhanceWeight = MdiTabControl.TabControl.Weight.Soft;
            this.TabControl1.TabCloseButtonImage = null;
            this.TabControl1.TabCloseButtonImageDisabled = null;
            this.TabControl1.TabCloseButtonImageHot = null;
            this.TabControl1.TabCloseButtonSize = new System.Drawing.Size(14, 14);
            this.TabControl1.TabHeight = 20;
            this.TabControl1.TabIconSize = new System.Drawing.Size(0, 0);
            this.TabControl1.TabIndex = 37;
            this.TabControl1.TabOffset = -8;
            this.TabControl1.TabPadLeft = 20;
            this.TabControl1.TabPadRight = 7;
            this.TabControl1.TabsDirection = MdiTabControl.TabControl.FlowDirection.LeftToRight;
            this.TabControl1.TabTop = 1;
            this.TabControl1.GetTabRegion += new MdiTabControl.TabControl.GetTabRegionEventHandler(this.TabControl1_GetTabRegion);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 529);
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "华美冷库WCS系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.NicontextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiSystem;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolStripMenuItem tsmiTool;
        private System.Windows.Forms.ToolStripMenuItem tsmiNote;
        private System.Windows.Forms.ToolStripMenuItem tsmiCount;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiDoc;
        private System.Windows.Forms.ToolStripMenuItem tsmiSupport;
        private System.Windows.Forms.ToolStripMenuItem tsmiVesion;
        private System.Windows.Forms.NotifyIcon notifyWCS;
        private System.Windows.Forms.ContextMenuStrip NicontextMenu;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        protected MdiTabControl.TabControl TabControl1;
    }
}

