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
            this.ordersListView = new System.Windows.Forms.ListView();
            this.headerTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerContract = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerBS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerQty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.updateBW = new System.ComponentModel.BackgroundWorker();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.getAllOrderButton = new System.Windows.Forms.Button();
            this.clearAllOrderButton = new System.Windows.Forms.Button();
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
            this.ordersListView.Dock = System.Windows.Forms.DockStyle.Right;
            this.ordersListView.FullRowSelect = true;
            this.ordersListView.GridLines = true;
            this.ordersListView.Location = new System.Drawing.Point(255, 0);
            this.ordersListView.Name = "ordersListView";
            this.ordersListView.Size = new System.Drawing.Size(619, 411);
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
            // updateTimer
            // 
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // getAllOrderButton
            // 
            this.getAllOrderButton.Location = new System.Drawing.Point(13, 13);
            this.getAllOrderButton.Name = "getAllOrderButton";
            this.getAllOrderButton.Size = new System.Drawing.Size(75, 23);
            this.getAllOrderButton.TabIndex = 1;
            this.getAllOrderButton.Text = "Get All";
            this.getAllOrderButton.UseVisualStyleBackColor = true;
            this.getAllOrderButton.Click += new System.EventHandler(this.getAllOrderButton_Click);
            // 
            // clearAllOrderButton
            // 
            this.clearAllOrderButton.Location = new System.Drawing.Point(12, 376);
            this.clearAllOrderButton.Name = "clearAllOrderButton";
            this.clearAllOrderButton.Size = new System.Drawing.Size(75, 23);
            this.clearAllOrderButton.TabIndex = 2;
            this.clearAllOrderButton.Text = "Clear";
            this.clearAllOrderButton.UseVisualStyleBackColor = true;
            this.clearAllOrderButton.Click += new System.EventHandler(this.clearAllOrderButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(874, 411);
            this.Controls.Add(this.clearAllOrderButton);
            this.Controls.Add(this.getAllOrderButton);
            this.Controls.Add(this.ordersListView);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form1";
            this.Text = "Web Notifier";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

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
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Button getAllOrderButton;
        private System.Windows.Forms.Button clearAllOrderButton;

    }
}

