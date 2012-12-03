namespace NotifierClientApp
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ordersListView = new System.Windows.Forms.ListView();
            this.headerTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerContract = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerBS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerQty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.updateBW = new System.ComponentModel.BackgroundWorker();
            this.OrderUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ordersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getAllOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orderUpdateIntervalMsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OrderUpdateToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.statusUpdateIntervalMsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusUpdateToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.statusUpdateDelaySecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DelayUpdateToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.appStatusBW = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ordersListView
            // 
            this.ordersListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerTime,
            this.headerContract,
            this.headerBS,
            this.headerQty,
            this.headerPrice,
            this.headerStatus});
            this.ordersListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ordersListView.FullRowSelect = true;
            this.ordersListView.GridLines = true;
            this.ordersListView.Location = new System.Drawing.Point(0, 24);
            this.ordersListView.Name = "ordersListView";
            this.ordersListView.Size = new System.Drawing.Size(613, 376);
            this.ordersListView.TabIndex = 0;
            this.ordersListView.UseCompatibleStateImageBehavior = false;
            this.ordersListView.View = System.Windows.Forms.View.Details;
            // 
            // headerTime
            // 
            this.headerTime.Text = "Time";
            this.headerTime.Width = 100;
            // 
            // headerContract
            // 
            this.headerContract.Text = "Contract";
            this.headerContract.Width = 100;
            // 
            // headerBS
            // 
            this.headerBS.Text = "Buy Sell";
            this.headerBS.Width = 100;
            // 
            // headerQty
            // 
            this.headerQty.Text = "Qty";
            this.headerQty.Width = 100;
            // 
            // headerPrice
            // 
            this.headerPrice.Text = "Price";
            this.headerPrice.Width = 100;
            // 
            // headerStatus
            // 
            this.headerStatus.Text = "Status";
            this.headerStatus.Width = 100;
            // 
            // updateBW
            // 
            this.updateBW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.updateBW_DoWork);
            // 
            // OrderUpdateTimer
            // 
            this.OrderUpdateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ordersToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(613, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ordersToolStripMenuItem
            // 
            this.ordersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getAllOrderToolStripMenuItem,
            this.clearHistoryToolStripMenuItem});
            this.ordersToolStripMenuItem.Name = "ordersToolStripMenuItem";
            this.ordersToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.ordersToolStripMenuItem.Text = "Orders";
            // 
            // getAllOrderToolStripMenuItem
            // 
            this.getAllOrderToolStripMenuItem.Name = "getAllOrderToolStripMenuItem";
            this.getAllOrderToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.getAllOrderToolStripMenuItem.Text = "Get All Orders";
            this.getAllOrderToolStripMenuItem.Click += new System.EventHandler(this.getAllOrderToolStripMenuItem_Click);
            // 
            // clearHistoryToolStripMenuItem
            // 
            this.clearHistoryToolStripMenuItem.Name = "clearHistoryToolStripMenuItem";
            this.clearHistoryToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.clearHistoryToolStripMenuItem.Text = "Clear History";
            this.clearHistoryToolStripMenuItem.Click += new System.EventHandler(this.clearHistoryToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.orderUpdateIntervalMsToolStripMenuItem,
            this.statusUpdateIntervalMsToolStripMenuItem,
            this.statusUpdateDelaySecToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // orderUpdateIntervalMsToolStripMenuItem
            // 
            this.orderUpdateIntervalMsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OrderUpdateToolStripTextBox});
            this.orderUpdateIntervalMsToolStripMenuItem.Name = "orderUpdateIntervalMsToolStripMenuItem";
            this.orderUpdateIntervalMsToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.orderUpdateIntervalMsToolStripMenuItem.Text = "Order Update Interval ms";
            // 
            // OrderUpdateToolStripTextBox
            // 
            this.OrderUpdateToolStripTextBox.Name = "OrderUpdateToolStripTextBox";
            this.OrderUpdateToolStripTextBox.Size = new System.Drawing.Size(100, 23);
            this.OrderUpdateToolStripTextBox.TextChanged += new System.EventHandler(this.OrderUpdateToolStripTextBox_TextChanged);
            // 
            // statusUpdateIntervalMsToolStripMenuItem
            // 
            this.statusUpdateIntervalMsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusUpdateToolStripTextBox});
            this.statusUpdateIntervalMsToolStripMenuItem.Name = "statusUpdateIntervalMsToolStripMenuItem";
            this.statusUpdateIntervalMsToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.statusUpdateIntervalMsToolStripMenuItem.Text = "Status Update Interval ms";
            // 
            // StatusUpdateToolStripTextBox
            // 
            this.StatusUpdateToolStripTextBox.Name = "StatusUpdateToolStripTextBox";
            this.StatusUpdateToolStripTextBox.Size = new System.Drawing.Size(100, 23);
            this.StatusUpdateToolStripTextBox.TextChanged += new System.EventHandler(this.StatusUpdateToolStripTextBox_TextChanged);
            // 
            // statusUpdateDelaySecToolStripMenuItem
            // 
            this.statusUpdateDelaySecToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DelayUpdateToolStripTextBox});
            this.statusUpdateDelaySecToolStripMenuItem.Name = "statusUpdateDelaySecToolStripMenuItem";
            this.statusUpdateDelaySecToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.statusUpdateDelaySecToolStripMenuItem.Text = "Status Update Delay sec";
            // 
            // DelayUpdateToolStripTextBox
            // 
            this.DelayUpdateToolStripTextBox.Name = "DelayUpdateToolStripTextBox";
            this.DelayUpdateToolStripTextBox.Size = new System.Drawing.Size(100, 23);
            this.DelayUpdateToolStripTextBox.TextChanged += new System.EventHandler(this.DelayUpdateToolStripTextBox_TextChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel1,
            this.statusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 378);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(613, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel1
            // 
            this.statusLabel1.Name = "statusLabel1";
            this.statusLabel1.Size = new System.Drawing.Size(10, 17);
            this.statusLabel1.Text = ".";
            // 
            // statusLabel2
            // 
            this.statusLabel2.Name = "statusLabel2";
            this.statusLabel2.Size = new System.Drawing.Size(10, 17);
            this.statusLabel2.Text = ".";
            // 
            // StatusUpdateTimer
            // 
            this.StatusUpdateTimer.Tick += new System.EventHandler(this.app1Statustimer_Tick);
            // 
            // appStatusBW
            // 
            this.appStatusBW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.appStatusBW_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(613, 400);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ordersListView);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Web Notifier";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView ordersListView;
        private System.Windows.Forms.ColumnHeader headerTime;
        private System.Windows.Forms.ColumnHeader headerContract;
        private System.Windows.Forms.ColumnHeader headerBS;
        private System.Windows.Forms.ColumnHeader headerQty;
        private System.Windows.Forms.ColumnHeader headerPrice;
        private System.Windows.Forms.ColumnHeader headerStatus;
        private System.ComponentModel.BackgroundWorker updateBW;
        private System.Windows.Forms.Timer OrderUpdateTimer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ordersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getAllOrderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearHistoryToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel2;
        private System.Windows.Forms.Timer StatusUpdateTimer;
        private System.ComponentModel.BackgroundWorker appStatusBW;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orderUpdateIntervalMsToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox OrderUpdateToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem statusUpdateIntervalMsToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox StatusUpdateToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem statusUpdateDelaySecToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox DelayUpdateToolStripTextBox;

    }
}

