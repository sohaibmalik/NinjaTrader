namespace NotifierClientApp
{
    partial class Notify
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Notify));
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
            this.tradeLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getPricesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pricesStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.appStatusBW = new System.ComponentModel.BackgroundWorker();
            this.listView1 = new System.Windows.Forms.ListView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listView2 = new System.Windows.Forms.ListView();
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
            this.ordersListView.FullRowSelect = true;
            this.ordersListView.GridLines = true;
            this.ordersListView.Location = new System.Drawing.Point(3, 24);
            this.ordersListView.Name = "ordersListView";
            this.ordersListView.Size = new System.Drawing.Size(612, 298);
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
            this.settingsToolStripMenuItem,
            this.tradeLogToolStripMenuItem,
            this.getPricesToolStripMenuItem,
            this.adminToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(618, 24);
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
            this.getAllOrderToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.getAllOrderToolStripMenuItem.Text = "Get Recent Live Orders";
            this.getAllOrderToolStripMenuItem.Click += new System.EventHandler(this.getAllOrderToolStripMenuItem_Click);
            // 
            // clearHistoryToolStripMenuItem
            // 
            this.clearHistoryToolStripMenuItem.Name = "clearHistoryToolStripMenuItem";
            this.clearHistoryToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
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
            // tradeLogToolStripMenuItem
            // 
            this.tradeLogToolStripMenuItem.Name = "tradeLogToolStripMenuItem";
            this.tradeLogToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.tradeLogToolStripMenuItem.Text = "TradeLog";
            this.tradeLogToolStripMenuItem.Click += new System.EventHandler(this.tradeLogToolStripMenuItem_Click);
            // 
            // getPricesToolStripMenuItem
            // 
            this.getPricesToolStripMenuItem.Name = "getPricesToolStripMenuItem";
            this.getPricesToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.getPricesToolStripMenuItem.Text = "Get Prices";
            this.getPricesToolStripMenuItem.Click += new System.EventHandler(this.getPricesToolStripMenuItem_Click);
            // 
            // adminToolStripMenuItem
            // 
            this.adminToolStripMenuItem.Name = "adminToolStripMenuItem";
            this.adminToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.adminToolStripMenuItem.Text = "Admin";
            this.adminToolStripMenuItem.Click += new System.EventHandler(this.adminToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel1,
            this.pricesStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 538);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(618, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel1
            // 
            this.statusLabel1.Name = "statusLabel1";
            this.statusLabel1.Size = new System.Drawing.Size(10, 17);
            this.statusLabel1.Text = ".";
            // 
            // pricesStatusLabel
            // 
            this.pricesStatusLabel.BackColor = System.Drawing.Color.Lime;
            this.pricesStatusLabel.Name = "pricesStatusLabel";
            this.pricesStatusLabel.Size = new System.Drawing.Size(13, 17);
            this.pricesStatusLabel.Text = "..";
            // 
            // StatusUpdateTimer
            // 
            this.StatusUpdateTimer.Tick += new System.EventHandler(this.app1Statustimer_Tick);
            // 
            // appStatusBW
            // 
            this.appStatusBW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.appStatusBW_DoWork);
            // 
            // listView1
            // 
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.Location = new System.Drawing.Point(141, 325);
            this.listView1.Margin = new System.Windows.Forms.Padding(0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(474, 168);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(141, 496);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(429, 38);
            this.textBox1.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImage = global::NotifierClientApp.Properties.Resources.Messages_icon;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.Location = new System.Drawing.Point(581, 496);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(34, 38);
            this.button1.TabIndex = 7;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // listView2
            // 
            this.listView2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView2.Location = new System.Drawing.Point(3, 325);
            this.listView2.Margin = new System.Windows.Forms.Padding(0);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(135, 168);
            this.listView2.TabIndex = 8;
            this.listView2.UseCompatibleStateImageBehavior = false;
            // 
            // Notify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(618, 560);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ordersListView);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Notify";
            this.Text = "Web Notifier";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Notify_FormClosing);
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
        private System.Windows.Forms.Timer StatusUpdateTimer;
        private System.ComponentModel.BackgroundWorker appStatusBW;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orderUpdateIntervalMsToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox OrderUpdateToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem statusUpdateIntervalMsToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox StatusUpdateToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem statusUpdateDelaySecToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox DelayUpdateToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem tradeLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getPricesToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel pricesStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem adminToolStripMenuItem;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listView2;

    }
}

