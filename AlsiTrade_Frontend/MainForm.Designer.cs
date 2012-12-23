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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.otsInstrumentTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.hiSatInstrumentTextBox = new System.Windows.Forms.TextBox();
            this.riskGroupBox = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tradeVolTextBox = new System.Windows.Forms.TextBox();
            this.stopLossTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.takeProfTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.emaGroupBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.emaA1TextBox = new System.Windows.Forms.TextBox();
            this.emaA2TextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.emaB1TextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.emaCTextBox = new System.Windows.Forms.TextBox();
            this.emaB2TextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.orderDetailGroupBox = new System.Windows.Forms.GroupBox();
            this.onlyTradesRadioButton = new System.Windows.Forms.RadioButton();
            this.fullTradesRadioButton = new System.Windows.Forms.RadioButton();
            this.timeframeGroupBox = new System.Windows.Forms.GroupBox();
            this._10minRadioButton = new System.Windows.Forms.RadioButton();
            this._5minRadioButton = new System.Windows.Forms.RadioButton();
            this._2minRadioButton = new System.Windows.Forms.RadioButton();
            this.timespanGroupBox = new System.Windows.Forms.GroupBox();
            this.startDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label12 = new System.Windows.Forms.Label();
            this.recentRadioButton = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.AllHistoRadioButton = new System.Windows.Forms.RadioButton();
            this.endDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.runHistCalcButton = new System.Windows.Forms.Button();
            this.dataTabControl = new System.Windows.Forms.TabControl();
            this.liveTradesTabPage = new System.Windows.Forms.TabPage();
            this.historicalTradesTabPage = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.settingsTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.riskGroupBox.SuspendLayout();
            this.emaGroupBox.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.orderDetailGroupBox.SuspendLayout();
            this.timeframeGroupBox.SuspendLayout();
            this.timespanGroupBox.SuspendLayout();
            this.dataTabControl.SuspendLayout();
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
            // otsInstrumentTextBox
            // 
            this.otsInstrumentTextBox.Location = new System.Drawing.Point(107, 56);
            this.otsInstrumentTextBox.Name = "otsInstrumentTextBox";
            this.otsInstrumentTextBox.Size = new System.Drawing.Size(100, 23);
            this.otsInstrumentTextBox.TabIndex = 21;
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
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(90, 15);
            this.label10.TabIndex = 20;
            this.label10.Text = "OTS Instrument";
            // 
            // hiSatInstrumentTextBox
            // 
            this.hiSatInstrumentTextBox.Location = new System.Drawing.Point(107, 27);
            this.hiSatInstrumentTextBox.Name = "hiSatInstrumentTextBox";
            this.hiSatInstrumentTextBox.Size = new System.Drawing.Size(100, 23);
            this.hiSatInstrumentTextBox.TabIndex = 19;
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
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 15);
            this.label8.TabIndex = 14;
            this.label8.Text = "Volume";
            // 
            // tradeVolTextBox
            // 
            this.tradeVolTextBox.Location = new System.Drawing.Point(86, 25);
            this.tradeVolTextBox.Name = "tradeVolTextBox";
            this.tradeVolTextBox.Size = new System.Drawing.Size(20, 23);
            this.tradeVolTextBox.TabIndex = 15;
            // 
            // stopLossTextBox
            // 
            this.stopLossTextBox.Location = new System.Drawing.Point(86, 83);
            this.stopLossTextBox.Name = "stopLossTextBox";
            this.stopLossTextBox.Size = new System.Drawing.Size(31, 23);
            this.stopLossTextBox.TabIndex = 13;
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
            // takeProfTextBox
            // 
            this.takeProfTextBox.Location = new System.Drawing.Point(86, 54);
            this.takeProfTextBox.Name = "takeProfTextBox";
            this.takeProfTextBox.Size = new System.Drawing.Size(31, 23);
            this.takeProfTextBox.TabIndex = 11;
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ema A2";
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Ema B1";
            // 
            // emaB1TextBox
            // 
            this.emaB1TextBox.Location = new System.Drawing.Point(59, 85);
            this.emaB1TextBox.Name = "emaB1TextBox";
            this.emaB1TextBox.Size = new System.Drawing.Size(20, 23);
            this.emaB1TextBox.TabIndex = 5;
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
            // emaB2TextBox
            // 
            this.emaB2TextBox.Location = new System.Drawing.Point(59, 115);
            this.emaB2TextBox.Name = "emaB2TextBox";
            this.emaB2TextBox.Size = new System.Drawing.Size(20, 23);
            this.emaB2TextBox.TabIndex = 7;
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
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.orderDetailGroupBox);
            this.tabPage2.Controls.Add(this.timeframeGroupBox);
            this.tabPage2.Controls.Add(this.timespanGroupBox);
            this.tabPage2.Controls.Add(this.runHistCalcButton);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(347, 456);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // orderDetailGroupBox
            // 
            this.orderDetailGroupBox.Controls.Add(this.onlyTradesRadioButton);
            this.orderDetailGroupBox.Controls.Add(this.fullTradesRadioButton);
            this.orderDetailGroupBox.Location = new System.Drawing.Point(9, 264);
            this.orderDetailGroupBox.Name = "orderDetailGroupBox";
            this.orderDetailGroupBox.Size = new System.Drawing.Size(255, 74);
            this.orderDetailGroupBox.TabIndex = 9;
            this.orderDetailGroupBox.TabStop = false;
            this.orderDetailGroupBox.Text = "Detail";
            // 
            // onlyTradesRadioButton
            // 
            this.onlyTradesRadioButton.AutoSize = true;
            this.onlyTradesRadioButton.Location = new System.Drawing.Point(9, 48);
            this.onlyTradesRadioButton.Name = "onlyTradesRadioButton";
            this.onlyTradesRadioButton.Size = new System.Drawing.Size(82, 17);
            this.onlyTradesRadioButton.TabIndex = 1;
            this.onlyTradesRadioButton.Text = "Trades Only";
            this.onlyTradesRadioButton.UseVisualStyleBackColor = true;
            // 
            // fullTradesRadioButton
            // 
            this.fullTradesRadioButton.AutoSize = true;
            this.fullTradesRadioButton.Checked = true;
            this.fullTradesRadioButton.Location = new System.Drawing.Point(9, 23);
            this.fullTradesRadioButton.Name = "fullTradesRadioButton";
            this.fullTradesRadioButton.Size = new System.Drawing.Size(76, 17);
            this.fullTradesRadioButton.TabIndex = 0;
            this.fullTradesRadioButton.TabStop = true;
            this.fullTradesRadioButton.Text = "Full Details";
            this.fullTradesRadioButton.UseVisualStyleBackColor = true;
            // 
            // timeframeGroupBox
            // 
            this.timeframeGroupBox.Controls.Add(this._10minRadioButton);
            this.timeframeGroupBox.Controls.Add(this._5minRadioButton);
            this.timeframeGroupBox.Controls.Add(this._2minRadioButton);
            this.timeframeGroupBox.Location = new System.Drawing.Point(8, 154);
            this.timeframeGroupBox.Name = "timeframeGroupBox";
            this.timeframeGroupBox.Size = new System.Drawing.Size(256, 103);
            this.timeframeGroupBox.TabIndex = 8;
            this.timeframeGroupBox.TabStop = false;
            this.timeframeGroupBox.Text = "Timeframe";
            // 
            // _10minRadioButton
            // 
            this._10minRadioButton.AutoSize = true;
            this._10minRadioButton.Location = new System.Drawing.Point(10, 73);
            this._10minRadioButton.Name = "_10minRadioButton";
            this._10minRadioButton.Size = new System.Drawing.Size(77, 17);
            this._10minRadioButton.TabIndex = 2;
            this._10minRadioButton.Text = "10 Minutes";
            this._10minRadioButton.UseVisualStyleBackColor = true;
            // 
            // _5minRadioButton
            // 
            this._5minRadioButton.AutoSize = true;
            this._5minRadioButton.Checked = true;
            this._5minRadioButton.Location = new System.Drawing.Point(10, 48);
            this._5minRadioButton.Name = "_5minRadioButton";
            this._5minRadioButton.Size = new System.Drawing.Size(71, 17);
            this._5minRadioButton.TabIndex = 1;
            this._5minRadioButton.TabStop = true;
            this._5minRadioButton.Text = "5 Minutes";
            this._5minRadioButton.UseVisualStyleBackColor = true;
            // 
            // _2minRadioButton
            // 
            this._2minRadioButton.AutoSize = true;
            this._2minRadioButton.Location = new System.Drawing.Point(10, 23);
            this._2minRadioButton.Name = "_2minRadioButton";
            this._2minRadioButton.Size = new System.Drawing.Size(71, 17);
            this._2minRadioButton.TabIndex = 0;
            this._2minRadioButton.Text = "2 Minutes";
            this._2minRadioButton.UseVisualStyleBackColor = true;
            // 
            // timespanGroupBox
            // 
            this.timespanGroupBox.Controls.Add(this.startDateTimePicker);
            this.timespanGroupBox.Controls.Add(this.label12);
            this.timespanGroupBox.Controls.Add(this.recentRadioButton);
            this.timespanGroupBox.Controls.Add(this.label11);
            this.timespanGroupBox.Controls.Add(this.AllHistoRadioButton);
            this.timespanGroupBox.Controls.Add(this.endDateTimePicker);
            this.timespanGroupBox.Location = new System.Drawing.Point(8, 6);
            this.timespanGroupBox.Name = "timespanGroupBox";
            this.timespanGroupBox.Size = new System.Drawing.Size(256, 142);
            this.timespanGroupBox.TabIndex = 7;
            this.timespanGroupBox.TabStop = false;
            this.timespanGroupBox.Text = "Timespan";
            // 
            // startDateTimePicker
            // 
            this.startDateTimePicker.Location = new System.Drawing.Point(44, 75);
            this.startDateTimePicker.Name = "startDateTimePicker";
            this.startDateTimePicker.Size = new System.Drawing.Size(200, 23);
            this.startDateTimePicker.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 110);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(27, 15);
            this.label12.TabIndex = 6;
            this.label12.Text = "End";
            // 
            // recentRadioButton
            // 
            this.recentRadioButton.AutoSize = true;
            this.recentRadioButton.Checked = true;
            this.recentRadioButton.Location = new System.Drawing.Point(44, 22);
            this.recentRadioButton.Name = "recentRadioButton";
            this.recentRadioButton.Size = new System.Drawing.Size(114, 19);
            this.recentRadioButton.TabIndex = 1;
            this.recentRadioButton.TabStop = true;
            this.recentRadioButton.Text = "MasterMin Table";
            this.recentRadioButton.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 81);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 15);
            this.label11.TabIndex = 5;
            this.label11.Text = "Start";
            // 
            // AllHistoRadioButton
            // 
            this.AllHistoRadioButton.AutoSize = true;
            this.AllHistoRadioButton.Location = new System.Drawing.Point(44, 47);
            this.AllHistoRadioButton.Name = "AllHistoRadioButton";
            this.AllHistoRadioButton.Size = new System.Drawing.Size(109, 19);
            this.AllHistoRadioButton.TabIndex = 2;
            this.AllHistoRadioButton.Text = "AllHistory Table";
            this.AllHistoRadioButton.UseVisualStyleBackColor = true;
            // 
            // endDateTimePicker
            // 
            this.endDateTimePicker.Location = new System.Drawing.Point(44, 104);
            this.endDateTimePicker.Name = "endDateTimePicker";
            this.endDateTimePicker.Size = new System.Drawing.Size(200, 23);
            this.endDateTimePicker.TabIndex = 4;
            // 
            // runHistCalcButton
            // 
            this.runHistCalcButton.Location = new System.Drawing.Point(8, 377);
            this.runHistCalcButton.Name = "runHistCalcButton";
            this.runHistCalcButton.Size = new System.Drawing.Size(75, 23);
            this.runHistCalcButton.TabIndex = 0;
            this.runHistCalcButton.Text = "Run";
            this.runHistCalcButton.UseVisualStyleBackColor = true;
            this.runHistCalcButton.Click += new System.EventHandler(this.runHistCalcButton_Click);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.riskGroupBox.ResumeLayout(false);
            this.riskGroupBox.PerformLayout();
            this.emaGroupBox.ResumeLayout(false);
            this.emaGroupBox.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.orderDetailGroupBox.ResumeLayout(false);
            this.orderDetailGroupBox.PerformLayout();
            this.timeframeGroupBox.ResumeLayout(false);
            this.timeframeGroupBox.PerformLayout();
            this.timespanGroupBox.ResumeLayout(false);
            this.timespanGroupBox.PerformLayout();
            this.dataTabControl.ResumeLayout(false);
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
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker endDateTimePicker;
        private System.Windows.Forms.DateTimePicker startDateTimePicker;
        private System.Windows.Forms.RadioButton AllHistoRadioButton;
        private System.Windows.Forms.RadioButton recentRadioButton;
        private System.Windows.Forms.Button runHistCalcButton;
        private System.Windows.Forms.GroupBox timeframeGroupBox;
        private System.Windows.Forms.RadioButton _10minRadioButton;
        private System.Windows.Forms.RadioButton _5minRadioButton;
        private System.Windows.Forms.RadioButton _2minRadioButton;
        private System.Windows.Forms.GroupBox timespanGroupBox;
        private System.Windows.Forms.GroupBox orderDetailGroupBox;
        private System.Windows.Forms.RadioButton onlyTradesRadioButton;
        private System.Windows.Forms.RadioButton fullTradesRadioButton;
        
    }
}

