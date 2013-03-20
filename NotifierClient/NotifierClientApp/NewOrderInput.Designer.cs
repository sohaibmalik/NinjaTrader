namespace NotifierClientApp
{
    partial class NewOrderInput
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
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.reasonComboBox = new System.Windows.Forms.ComboBox();
            this.volNum = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.priceSubmitTextBox = new System.Windows.Forms.TextBox();
            this.priceMatchedTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.timeTimePicker = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.volNum)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Date-Time";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(168, 52);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(45, 19);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Buy";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(225, 53);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(43, 19);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Sell";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // reasonComboBox
            // 
            this.reasonComboBox.FormattingEnabled = true;
            this.reasonComboBox.Location = new System.Drawing.Point(111, 85);
            this.reasonComboBox.Name = "reasonComboBox";
            this.reasonComboBox.Size = new System.Drawing.Size(163, 23);
            this.reasonComboBox.TabIndex = 4;
            // 
            // volNum
            // 
            this.volNum.Location = new System.Drawing.Point(111, 50);
            this.volNum.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.volNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.volNum.Name = "volNum";
            this.volNum.Size = new System.Drawing.Size(50, 23);
            this.volNum.TabIndex = 5;
            this.volNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Reason";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Volume";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Price Insert";
            // 
            // priceSubmitTextBox
            // 
            this.priceSubmitTextBox.Location = new System.Drawing.Point(111, 122);
            this.priceSubmitTextBox.MaxLength = 5;
            this.priceSubmitTextBox.Name = "priceSubmitTextBox";
            this.priceSubmitTextBox.Size = new System.Drawing.Size(163, 23);
            this.priceSubmitTextBox.TabIndex = 9;
            this.priceSubmitTextBox.TextChanged += new System.EventHandler(this.priceSubmitTextBox_TextChanged);
            // 
            // priceMatchedTextBox
            // 
            this.priceMatchedTextBox.Location = new System.Drawing.Point(111, 159);
            this.priceMatchedTextBox.MaxLength = 5;
            this.priceMatchedTextBox.Name = "priceMatchedTextBox";
            this.priceMatchedTextBox.Size = new System.Drawing.Size(163, 23);
            this.priceMatchedTextBox.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 163);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "Price Matched";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(187, 200);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(87, 27);
            this.saveButton.TabIndex = 12;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker.Location = new System.Drawing.Point(111, 12);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(83, 23);
            this.dateTimePicker.TabIndex = 13;
            // 
            // timeTimePicker
            // 
            this.timeTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timeTimePicker.Location = new System.Drawing.Point(200, 12);
            this.timeTimePicker.Name = "timeTimePicker";
            this.timeTimePicker.ShowUpDown = true;
            this.timeTimePicker.Size = new System.Drawing.Size(92, 23);
            this.timeTimePicker.TabIndex = 14;
            // 
            // NewOrderInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 236);
            this.Controls.Add(this.timeTimePicker);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.priceMatchedTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.priceSubmitTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.volNum);
            this.Controls.Add(this.reasonComboBox);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "NewOrderInput";
            this.Text = "NewOrderInput";
            this.Load += new System.EventHandler(this.NewOrderInput_Load);
            ((System.ComponentModel.ISupportInitialize)(this.volNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.ComboBox reasonComboBox;
        private System.Windows.Forms.NumericUpDown volNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox priceSubmitTextBox;
        private System.Windows.Forms.TextBox priceMatchedTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.DateTimePicker timeTimePicker;
    }
}