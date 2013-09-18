namespace NotifierClientApp
{
    partial class AdminForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminForm));
            this.userListView = new System.Windows.Forms.ListView();
            this.Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.admin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // userListView
            // 
            this.userListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Name,
            this.admin});
            this.userListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userListView.FullRowSelect = true;
            this.userListView.Location = new System.Drawing.Point(0, 0);
            this.userListView.MultiSelect = false;
            this.userListView.Name = "userListView";
            this.userListView.Size = new System.Drawing.Size(184, 372);
            this.userListView.SmallImageList = this.imageList1;
            this.userListView.TabIndex = 0;
            this.userListView.UseCompatibleStateImageBehavior = false;
            this.userListView.View = System.Windows.Forms.View.Details;
            this.userListView.DoubleClick += new System.EventHandler(this.userListView_DoubleClick);
            // 
            // Name
            // 
            this.Name.Text = "User";
            this.Name.Width = 120;
            // 
            // admin
            // 
            this.admin.Text = "Admin";
            this.admin.Width = 59;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "red.ico");
            this.imageList1.Images.SetKeyName(1, "green.ico");
            this.imageList1.Images.SetKeyName(2, "Empty.ico");
            this.imageList1.Images.SetKeyName(3, "yellow.ico");
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Interval = 10000;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(114, 26);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 372);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.userListView);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         
            this.Load += new System.EventHandler(this.AdminForm_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView userListView;
        private System.Windows.Forms.ColumnHeader Name;
        private System.Windows.Forms.ColumnHeader admin;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    }
}