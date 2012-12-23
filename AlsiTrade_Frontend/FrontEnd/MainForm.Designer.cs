namespace FrontEnd
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.settingsTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataTabControl = new System.Windows.Forms.TabControl();
            this.liveTradesTabPage = new System.Windows.Forms.TabPage();
            this.historicalTradesTabPage = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.label1 = new System.Windows.Forms.Label();
            this.emaA1TextBox = new System.Windows.Forms.TextBox();
            this.emaA2TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.emaB1TextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.emaB2TextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.emaCTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.takeProfTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.stopLossTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tradeVolTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.emaGroupBox = new System.Windows.Forms.GroupBox();
            this.riskGroupBox = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.hiSatInstrumentTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.otsInstrumentTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.settingsTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.dataTabControl.SuspendLayout();
            this.emaGroupBox.SuspendLayout();
            this.riskGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.settingsTabControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataTabControl);
            this.splitContainer1.Size = new System.Drawing.Size(1066, 484);
            this.splitContainer1.SplitterDistance = 355;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // settingsTabControl
            // 
            this.settingsTabControl.Controls.Add(this.tabPage1);
            this.settingsTabControl.Controls.Add(this.tabPage2);
            this.settingsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsTabControl.Location = new System.Drawing.Point(0, 0);
            this.settingsTabControl.Name = "settingsTabControl";
            this.settingsTabControl.SelectedIndex = 0;
            this.settingsTabControl.Size = new System.Drawing.Size(355, 484);
            this.settingsTabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.riskGroupBox);
            this.tabPage1.Controls.Add(this.emaGroupBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(347, 456);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(347, 458);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataTabControl
            // 
            this.dataTabControl.Controls.Add(this.liveTradesTabPage);
            this.dataTabControl.Controls.Add(this.historicalTradesTabPage);
            this.dataTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTabControl.Location = new System.Drawing.Point(0, 0);
            this.dataTabControl.Name = "dataTabControl";
            this.dataTabControl.SelectedIndex = 0;
            this.dataTabControl.Size = new System.Drawing.Size(706, 484);
            this.dataTabControl.TabIndex = 0;
            // 
            // liveTradesTabPage
            // 
            this.liveTradesTabPage.Location = new System.Drawing.Point(4, 24);
            this.liveTradesTabPage.Name = "liveTradesTabPage";
            this.liveTradesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.liveTradesTabPage.Size = new System.Drawing.Size(698, 456);
            this.liveTradesTabPage.TabIndex = 0;
            this.liveTradesTabPage.Text = "Live";
            this.liveTradesTabPage.UseVisualStyleBackColor = true;
            // 
            // historicalTradesTabPage
            // 
            this.historicalTradesTabPage.Location = new System.Drawing.Point(4, 22);
            this.historicalTradesTabPage.Name = "historicalTradesTabPage";
            this.historicalTradesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.historicalTradesTabPage.Size = new System.Drawing.Size(698, 458);
            this.historicalTradesTabPage.TabIndex = 1;
            this.historicalTradesTabPage.Text = "History";
            this.historicalTradesTabPage.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 508);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1066, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1066, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ema A1";
            // 
            // emaA1TextBox
            // 
            this.emaA1TextBox.Location = new System.Drawing.Point(59, 25);
            this.emaA1TextBox.Name = "emaA1TextBox";
            this.emaA1TextBox.Size = new System.Drawing.Size(20, 23);
            this.emaA1TextBox.TabIndex = 1;
            // 
            // emaA2TextBox
            // 
            this.emaA2TextBox.Location = new System.Drawing.Point(58, 55);
            this.emaA2TextBox.Name = "emaA2TextBox";
            this.emaA2TextBox.Size = new System.Drawing.Size(20, 23);
            this.emaA2TextBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ema A2";
            // 
            // emaB1TextBox
            // 
            this.emaB1TextBox.Location = new System.Drawing.Point(59, 85);
            this.emaB1TextBox.Name = "emaB1TextBox";
            this.emaB1TextBox.Size = new System.Drawing.Size(20, 23);
            this.emaB1TextBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Ema B1";
            // 
            // emaB2TextBox
            // 
            this.emaB2TextBox.Location = new System.Drawing.Point(59, 115);
            this.emaB2TextBox.Name = "emaB2TextBox";
            this.emaB2TextBox.Size = new System.Drawing.Size(20, 23);
            this.emaB2TextBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Ema B2";
            // 
            // emaCTextBox
            // 
            this.emaCTextBox.Location = new System.Drawing.Point(59, 145);
            this.emaCTextBox.Name = "emaCTextBox";
            this.emaCTextBox.Size = new System.Drawing.Size(20, 23);
            this.emaCTextBox.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 148);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Ema C";
            // 
            // takeProfTextBox
            // 
            this.takeProfTextBox.Location = new System.Drawing.Point(86, 54);
            this.takeProfTextBox.Name = "takeProfTextBox";
            this.takeProfTextBox.Size = new System.Drawing.Size(31, 23);
            this.takeProfTextBox.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 15);
            this.label6.TabIndex = 10;
            this.label6.Text = "Take Profit";
            // 
            // stopLossTextBox
            // 
            this.stopLossTextBox.Location = new System.Drawing.Point(86, 83);
            this.stopLossTextBox.Name = "stopLossTextBox";
            this.stopLossTextBox.Size = new System.Drawing.Size(31, 23);
            this.stopLossTextBox.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 15);
            this.label7.TabIndex = 12;
            this.label7.Text = "Stop Loss";
            // 
            // tradeVolTextBox
            // 
            this.tradeVolTextBox.Location = new System.Drawing.Point(86, 25);
            this.tradeVolTextBox.Name = "tradeVolTextBox";
            this.tradeVolTextBox.Size = new System.Drawing.Size(20, 23);
            this.tradeVolTextBox.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 15);
            this.label8.TabIndex = 14;
            this.label8.Text = "Volume";
            // 
            // emaGroupBox
            // 
            this.emaGroupBox.Controls.Add(this.label2);
            this.emaGroupBox.Controls.Add(this.label1);
            this.emaGroupBox.Controls.Add(this.emaA1TextBox);
            this.emaGroupBox.Controls.Add(this.emaA2TextBox);
            this.emaGroupBox.Controls.Add(this.label3);
            this.emaGroupBox.Controls.Add(this.emaB1TextBox);
            this.emaGroupBox.Controls.Add(this.label4);
            this.emaGroupBox.Controls.Add(this.emaCTextBox);
            this.emaGroupBox.Controls.Add(this.emaB2TextBox);
            this.emaGroupBox.Controls.Add(this.label5);
            this.emaGroupBox.Location = new System.Drawing.Point(9, 6);
            this.emaGroupBox.Name = "emaGroupBox";
            this.emaGroupBox.Size = new System.Drawing.Size(98, 177);
            this.emaGroupBox.TabIndex = 16;
            this.emaGroupBox.TabStop = false;
            this.emaGroupBox.Text = "EMA";
            // 
            // riskGroupBox
            // 
            this.riskGroupBox.Controls.Add(this.label8);
            this.riskGroupBox.Controls.Add(this.tradeVolTextBox);
            this.riskGroupBox.Controls.Add(this.stopLossTextBox);
            this.riskGroupBox.Controls.Add(this.label6);
            this.riskGroupBox.Controls.Add(this.takeProfTextBox);
            this.riskGroupBox.Controls.Add(this.label7);
            this.riskGroupBox.Location = new System.Drawing.Point(113, 6);
            this.riskGroupBox.Name = "riskGroupBox";
            this.riskGroupBox.Size = new System.Drawing.Size(132, 117);
            this.riskGroupBox.TabIndex = 17;
            this.riskGroupBox.TabStop = false;
            this.riskGroupBox.Text = "Risk";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 15);
            this.label9.TabIndex = 18;
            this.label9.Text = "HiSat Instrument";
            // 
            // hiSatInstrumentTextBox
            // 
            this.hiSatInstrumentTextBox.Location = new System.Drawing.Point(107, 27);
            this.hiSatInstrumentTextBox.Name = "hiSatInstrumentTextBox";
            this.hiSatInstrumentTextBox.Size = new System.Drawing.Size(100, 23);
            this.hiSatInstrumentTextBox.TabIndex = 19;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(90, 15);
            this.label10.TabIndex = 20;
            this.label10.Text = "OTS Instrument";
            // 
            // otsInstrumentTextBox
            // 
            this.otsInstrumentTextBox.Location = new System.Drawing.Point(107, 56);
            this.otsInstrumentTextBox.Name = "otsInstrumentTextBox";
            this.otsInstrumentTextBox.Size = new System.Drawing.Size(100, 23);
            this.otsInstrumentTextBox.TabIndex = 21;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.otsInstrumentTextBox);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.hiSatInstrumentTextBox);
            this.groupBox1.Location = new System.Drawing.Point(9, 189);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(219, 93);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Contract Name";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1066, 530);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.settingsTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.dataTabControl.ResumeLayout(false);
            this.emaGroupBox.ResumeLayout(false);
            this.emaGroupBox.PerformLayout();
            this.riskGroupBox.ResumeLayout(false);
            this.riskGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl dataTabControl;
        private System.Windows.Forms.TabPage liveTradesTabPage;
        private System.Windows.Forms.TabPage historicalTradesTabPage;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.TabControl settingsTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox emaCTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox emaB2TextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox emaB1TextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox emaA2TextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox emaA1TextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox emaGroupBox;
        private System.Windows.Forms.TextBox tradeVolTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox stopLossTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox takeProfTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox otsInstrumentTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox hiSatInstrumentTextBox;
        private System.Windows.Forms.GroupBox riskGroupBox;
        
    }
}

