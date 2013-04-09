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
            this.changeUserNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tradeLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pricesStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.appStatusBW = new System.ComponentModel.BackgroundWorker();
            this.chatInputTextBox = new System.Windows.Forms.TextBox();
            this.chatSendButton = new System.Windows.Forms.Button();
            this.userListView = new System.Windows.Forms.ListView();
            this.userCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chatUsersList = new System.Windows.Forms.ImageList(this.components);
            this.nameSelectedLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chatHistoryListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.userStatusUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ordersToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.tradeLogToolStripMenuItem,
            this.adminToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(619, 24);
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
            this.getAllOrderToolStripMenuItem.Enabled = false;
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
            this.statusUpdateDelaySecToolStripMenuItem,
            this.changeUserNameToolStripMenuItem});
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
            // changeUserNameToolStripMenuItem
            // 
            this.changeUserNameToolStripMenuItem.Name = "changeUserNameToolStripMenuItem";
            this.changeUserNameToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.changeUserNameToolStripMenuItem.Text = "Change Username";
            this.changeUserNameToolStripMenuItem.Click += new System.EventHandler(this.changeUserNameToolStripMenuItem_Click);
            // 
            // tradeLogToolStripMenuItem
            // 
            this.tradeLogToolStripMenuItem.Name = "tradeLogToolStripMenuItem";
            this.tradeLogToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.tradeLogToolStripMenuItem.Text = "TradeLog";
            this.tradeLogToolStripMenuItem.Click += new System.EventHandler(this.tradeLogToolStripMenuItem_Click);
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
            this.statusStrip1.Size = new System.Drawing.Size(619, 22);
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
            this.pricesStatusLabel.Size = new System.Drawing.Size(164, 17);
            this.pricesStatusLabel.Text = "Click here to update Alsi price";
            this.pricesStatusLabel.Click += new System.EventHandler(this.pricesStatusLabel_Click);
            // 
            // StatusUpdateTimer
            // 
            this.StatusUpdateTimer.Tick += new System.EventHandler(this.app1Statustimer_Tick);
            // 
            // appStatusBW
            // 
            this.appStatusBW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.appStatusBW_DoWork);
            // 
            // chatInputTextBox
            // 
            this.chatInputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chatInputTextBox.Enabled = false;
            this.chatInputTextBox.Location = new System.Drawing.Point(141, 496);
            this.chatInputTextBox.Multiline = true;
            this.chatInputTextBox.Name = "chatInputTextBox";
            this.chatInputTextBox.Size = new System.Drawing.Size(434, 38);
            this.chatInputTextBox.TabIndex = 6;
            this.chatInputTextBox.Text = "Still under contruction";
            this.chatInputTextBox.TextChanged += new System.EventHandler(this.chatInputTextBox_TextChanged);
            // 
            // chatSendButton
            // 
            this.chatSendButton.BackColor = System.Drawing.Color.Transparent;
            this.chatSendButton.BackgroundImage = global::NotifierClientApp.Properties.Resources.Messages_icon;
            this.chatSendButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.chatSendButton.Location = new System.Drawing.Point(581, 496);
            this.chatSendButton.Name = "chatSendButton";
            this.chatSendButton.Size = new System.Drawing.Size(34, 38);
            this.chatSendButton.TabIndex = 7;
            this.chatSendButton.UseVisualStyleBackColor = false;
            this.chatSendButton.Click += new System.EventHandler(this.chatSendButton_Click);
            // 
            // userListView
            // 
            this.userListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.userCol});
            this.userListView.Location = new System.Drawing.Point(3, 325);
            this.userListView.Margin = new System.Windows.Forms.Padding(0);
            this.userListView.MultiSelect = false;
            this.userListView.Name = "userListView";
            this.userListView.Size = new System.Drawing.Size(135, 168);
            this.userListView.SmallImageList = this.chatUsersList;
            this.userListView.TabIndex = 8;
            this.userListView.UseCompatibleStateImageBehavior = false;
            this.userListView.View = System.Windows.Forms.View.Details;
            this.userListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.userListView_ItemChecked);
            this.userListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.userListView_ItemSelectionChanged);
            // 
            // userCol
            // 
            this.userCol.Text = "Online users";
            this.userCol.Width = 135;
            // 
            // chatUsersList
            // 
            this.chatUsersList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("chatUsersList.ImageStream")));
            this.chatUsersList.TransparentColor = System.Drawing.Color.Transparent;
            this.chatUsersList.Images.SetKeyName(0, "red.ico");
            this.chatUsersList.Images.SetKeyName(1, "green.ico");
            this.chatUsersList.Images.SetKeyName(2, "Empty.ico");
            this.chatUsersList.Images.SetKeyName(3, "imageres_1024.ico");
            this.chatUsersList.Images.SetKeyName(4, "yellow.ico");
            // 
            // nameSelectedLabel
            // 
            this.nameSelectedLabel.AutoSize = true;
            this.nameSelectedLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.nameSelectedLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameSelectedLabel.Location = new System.Drawing.Point(88, 0);
            this.nameSelectedLabel.Name = "nameSelectedLabel";
            this.nameSelectedLabel.Size = new System.Drawing.Size(41, 38);
            this.nameSelectedLabel.TabIndex = 9;
            this.nameSelectedLabel.Text = "Send:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            this.tableLayoutPanel1.Controls.Add(this.nameSelectedLabel, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 496);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(132, 38);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // chatHistoryListView
            // 
            this.chatHistoryListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.chatHistoryListView.Enabled = false;
            this.chatHistoryListView.Location = new System.Drawing.Point(142, 325);
            this.chatHistoryListView.Name = "chatHistoryListView";
            this.chatHistoryListView.Size = new System.Drawing.Size(473, 168);
            this.chatHistoryListView.TabIndex = 11;
            this.chatHistoryListView.UseCompatibleStateImageBehavior = false;
            this.chatHistoryListView.View = System.Windows.Forms.View.Details;
            this.chatHistoryListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.chatHistoryListView_ItemSelectionChanged);
            this.chatHistoryListView.MouseHover += new System.EventHandler(this.chatHistoryListView_MouseHover);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Message";
            this.columnHeader1.Width = 450;
            // 
            // userStatusUpdateTimer
            // 
            this.userStatusUpdateTimer.Enabled = true;
            this.userStatusUpdateTimer.Interval = 5000;
            this.userStatusUpdateTimer.Tick += new System.EventHandler(this.userStatusUpdateTimer_Tick);
            // 
            // Notify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(619, 560);
            this.Controls.Add(this.chatHistoryListView);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.userListView);
            this.Controls.Add(this.chatSendButton);
            this.Controls.Add(this.chatInputTextBox);
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
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
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
        private System.Windows.Forms.ToolStripStatusLabel pricesStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem adminToolStripMenuItem;
        private System.Windows.Forms.TextBox chatInputTextBox;
        private System.Windows.Forms.Button chatSendButton;
        private System.Windows.Forms.ListView userListView;
        private System.Windows.Forms.ColumnHeader userCol;
        private System.Windows.Forms.ImageList chatUsersList;
        private System.Windows.Forms.Label nameSelectedLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView chatHistoryListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Timer userStatusUpdateTimer;
        private System.Windows.Forms.ToolStripMenuItem changeUserNameToolStripMenuItem;
       

    }
}

