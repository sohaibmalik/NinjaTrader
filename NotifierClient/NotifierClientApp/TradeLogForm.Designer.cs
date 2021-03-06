﻿namespace NotifierClientApp
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
            this.uploadButton = new System.Windows.Forms.Button();
            this.chartSizeTextBox = new System.Windows.Forms.TextBox();
            this.chartButton = new System.Windows.Forms.Button();
            this.newButton = new System.Windows.Forms.Button();
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
            this.tradeListView.DoubleClick += new System.EventHandler(this.tradeListView_DoubleClick);
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
            this.infoBox.Controls.Add(this.uploadButton);
            this.infoBox.Controls.Add(this.chartSizeTextBox);
            this.infoBox.Controls.Add(this.chartButton);
            this.infoBox.Controls.Add(this.newButton);
            this.infoBox.Controls.Add(this.deleteButton);
            this.infoBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoBox.Location = new System.Drawing.Point(0, 0);
            this.infoBox.Name = "infoBox";
            this.infoBox.Size = new System.Drawing.Size(602, 103);
            this.infoBox.TabIndex = 1;
            this.infoBox.TabStop = false;
            // 
            // uploadButton
            // 
            this.uploadButton.Location = new System.Drawing.Point(11, 74);
            this.uploadButton.Name = "uploadButton";
            this.uploadButton.Size = new System.Drawing.Size(66, 23);
            this.uploadButton.TabIndex = 9;
            this.uploadButton.Text = "Upload";
            this.uploadButton.UseVisualStyleBackColor = true;
            this.uploadButton.Visible = false;
            this.uploadButton.Click += new System.EventHandler(this.uploadButton_Click);
            // 
            // chartSizeTextBox
            // 
            this.chartSizeTextBox.Location = new System.Drawing.Point(12, 44);
            this.chartSizeTextBox.Name = "chartSizeTextBox";
            this.chartSizeTextBox.Size = new System.Drawing.Size(35, 23);
            this.chartSizeTextBox.TabIndex = 8;
            this.chartSizeTextBox.Text = "750";
            this.chartSizeTextBox.TextChanged += new System.EventHandler(this.chartSizeTextBox_TextChanged);
            // 
            // chartButton
            // 
            this.chartButton.Location = new System.Drawing.Point(11, 14);
            this.chartButton.Name = "chartButton";
            this.chartButton.Size = new System.Drawing.Size(75, 23);
            this.chartButton.TabIndex = 7;
            this.chartButton.Text = "Draw Chart";
            this.chartButton.UseVisualStyleBackColor = true;
            this.chartButton.Click += new System.EventHandler(this.chartButton_Click);
            // 
            // newButton
            // 
            this.newButton.Location = new System.Drawing.Point(521, 14);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(75, 23);
            this.newButton.TabIndex = 6;
            this.newButton.Text = "New";
            this.newButton.UseVisualStyleBackColor = true;
            this.newButton.Click += new System.EventHandler(this.newButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Enabled = false;
            this.deleteButton.Location = new System.Drawing.Point(521, 43);
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
        private System.Windows.Forms.ColumnHeader columnPrice;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button newButton;
        private System.Windows.Forms.Button chartButton;
        private System.Windows.Forms.TextBox chartSizeTextBox;
        private System.Windows.Forms.Button uploadButton;

    }
}