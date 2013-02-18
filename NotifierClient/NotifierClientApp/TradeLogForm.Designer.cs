namespace NotifierClientApp
{
    partial class TradeLogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TradeLogForm));
            this.tradeListView = new System.Windows.Forms.ListView();
            this.columnTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnBuySell = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnReason = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnVolume = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnMatched = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnPriceMatched = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.infoBox = new System.Windows.Forms.GroupBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.matchedTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox = new System.Windows.Forms.CheckBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.infoBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tradeListView
            // 
            this.tradeListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnTime,
            this.columnBuySell,
            this.columnPrice,
            this.columnReason,
            this.columnVolume,
            this.columnMatched,
            this.columnPriceMatched});
            this.tradeListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tradeListView.FullRowSelect = true;
            this.tradeListView.GridLines = true;
            this.tradeListView.Location = new System.Drawing.Point(0, 0);
            this.tradeListView.MultiSelect = false;
            this.tradeListView.Name = "tradeListView";
            this.tradeListView.Size = new System.Drawing.Size(604, 605);
            this.tradeListView.TabIndex = 0;
            this.tradeListView.UseCompatibleStateImageBehavior = false;
            this.tradeListView.View = System.Windows.Forms.View.Details;
            this.tradeListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.tradeListView_ItemSelectionChanged);
            // 
            // columnTime
            // 
            this.columnTime.Text = "Time";
            this.columnTime.Width = 144;
            // 
            // columnBuySell
            // 
            this.columnBuySell.Text = "BuySell";
            this.columnBuySell.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnBuySell.Width = 55;
            // 
            // columnPrice
            // 
            this.columnPrice.Text = "Price";
            this.columnPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnPrice.Width = 91;
            // 
            // columnReason
            // 
            this.columnReason.Text = "Reason";
            this.columnReason.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnReason.Width = 102;
            // 
            // columnVolume
            // 
            this.columnVolume.Text = "Volume";
            this.columnVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnVolume.Width = 55;
            // 
            // columnMatched
            // 
            this.columnMatched.Text = "Matched";
            this.columnMatched.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnMatched.Width = 62;
            // 
            // columnPriceMatched
            // 
            this.columnPriceMatched.Text = "Matched Price";
            this.columnPriceMatched.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnPriceMatched.Width = 91;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.infoBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 500);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(604, 105);
            this.panel1.TabIndex = 1;
            // 
            // infoBox
            // 
            this.infoBox.Controls.Add(this.deleteButton);
            this.infoBox.Controls.Add(this.checkBox);
            this.infoBox.Controls.Add(this.saveButton);
            this.infoBox.Controls.Add(this.matchedTextBox);
            this.infoBox.Controls.Add(this.label1);
            this.infoBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoBox.Location = new System.Drawing.Point(0, 0);
            this.infoBox.Name = "infoBox";
            this.infoBox.Size = new System.Drawing.Size(602, 103);
            this.infoBox.TabIndex = 1;
            this.infoBox.TabStop = false;
            this.infoBox.Text = "Trade Info";
            // 
            // saveButton
            // 
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(521, 74);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // matchedTextBox
            // 
            this.matchedTextBox.Location = new System.Drawing.Point(102, 58);
            this.matchedTextBox.Name = "matchedTextBox";
            this.matchedTextBox.Size = new System.Drawing.Size(88, 23);
            this.matchedTextBox.TabIndex = 2;
            this.matchedTextBox.TextChanged += new System.EventHandler(this.matchedTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Matched Price";
            // 
            // checkBox
            // 
            this.checkBox.AutoSize = true;
            this.checkBox.Location = new System.Drawing.Point(102, 33);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(73, 19);
            this.checkBox.TabIndex = 4;
            this.checkBox.Text = "Matched";
            this.checkBox.UseVisualStyleBackColor = true;
            // 
            // deleteButton
            // 
            this.deleteButton.Enabled = false;
            this.deleteButton.Location = new System.Drawing.Point(521, 45);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // TradeLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 605);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tradeListView);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TradeLogForm";
            this.Text = "Trade Log";
            this.Load += new System.EventHandler(this.TradeLogForm_Load);
            this.panel1.ResumeLayout(false);
            this.infoBox.ResumeLayout(false);
            this.infoBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView tradeListView;
        private System.Windows.Forms.ColumnHeader columnTime;
        private System.Windows.Forms.ColumnHeader columnBuySell;
        private System.Windows.Forms.ColumnHeader columnReason;
        private System.Windows.Forms.ColumnHeader columnVolume;
        private System.Windows.Forms.ColumnHeader columnMatched;
        private System.Windows.Forms.ColumnHeader columnPriceMatched;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox infoBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox matchedTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader columnPrice;
        private System.Windows.Forms.CheckBox checkBox;
        private System.Windows.Forms.Button deleteButton;

    }
}