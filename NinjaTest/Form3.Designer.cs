namespace NinjaTest
{
    partial class Form3
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
            this.A1Start = new System.Windows.Forms.NumericUpDown();
            this.maLabel = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.positionLabel = new System.Windows.Forms.Label();
            this.A1End = new System.Windows.Forms.NumericUpDown();
            this.A2End = new System.Windows.Forms.NumericUpDown();
            this.A2Start = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.B2End = new System.Windows.Forms.NumericUpDown();
            this.B2Start = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.B1End = new System.Windows.Forms.NumericUpDown();
            this.B1Start = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.C1End = new System.Windows.Forms.NumericUpDown();
            this.C1Start = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.A1Start)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.A1End)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.A2End)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.A2Start)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2End)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2Start)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1End)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1Start)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.C1End)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.C1Start)).BeginInit();
            this.SuspendLayout();
            // 
            // A1Start
            // 
            this.A1Start.Location = new System.Drawing.Point(49, 67);
            this.A1Start.Name = "A1Start";
            this.A1Start.Size = new System.Drawing.Size(43, 23);
            this.A1Start.TabIndex = 23;
            this.A1Start.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // maLabel
            // 
            this.maLabel.AutoSize = true;
            this.maLabel.Location = new System.Drawing.Point(23, 69);
            this.maLabel.Name = "maLabel";
            this.maLabel.Size = new System.Drawing.Size(21, 15);
            this.maLabel.TabIndex = 17;
            this.maLabel.Text = "A1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(26, 14);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(87, 27);
            this.button2.TabIndex = 15;
            this.button2.Text = "startSim";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.richTextBox1.Location = new System.Drawing.Point(218, 14);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(606, 362);
            this.richTextBox1.TabIndex = 28;
            this.richTextBox1.Text = "";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // positionLabel
            // 
            this.positionLabel.AutoSize = true;
            this.positionLabel.Location = new System.Drawing.Point(12, 303);
            this.positionLabel.Name = "positionLabel";
            this.positionLabel.Size = new System.Drawing.Size(50, 15);
            this.positionLabel.TabIndex = 31;
            this.positionLabel.Text = "Position";
            // 
            // A1End
            // 
            this.A1End.Location = new System.Drawing.Point(100, 67);
            this.A1End.Name = "A1End";
            this.A1End.Size = new System.Drawing.Size(43, 23);
            this.A1End.TabIndex = 34;
            this.A1End.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // A2End
            // 
            this.A2End.Location = new System.Drawing.Point(100, 98);
            this.A2End.Name = "A2End";
            this.A2End.Size = new System.Drawing.Size(43, 23);
            this.A2End.TabIndex = 37;
            this.A2End.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // A2Start
            // 
            this.A2Start.Location = new System.Drawing.Point(49, 98);
            this.A2Start.Name = "A2Start";
            this.A2Start.Size = new System.Drawing.Size(43, 23);
            this.A2Start.TabIndex = 36;
            this.A2Start.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 15);
            this.label2.TabIndex = 35;
            this.label2.Text = "A2";
            // 
            // B2End
            // 
            this.B2End.Location = new System.Drawing.Point(100, 160);
            this.B2End.Name = "B2End";
            this.B2End.Size = new System.Drawing.Size(43, 23);
            this.B2End.TabIndex = 43;
            this.B2End.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // B2Start
            // 
            this.B2Start.Location = new System.Drawing.Point(49, 160);
            this.B2Start.Name = "B2Start";
            this.B2Start.Size = new System.Drawing.Size(43, 23);
            this.B2Start.TabIndex = 42;
            this.B2Start.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 162);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 15);
            this.label1.TabIndex = 41;
            this.label1.Text = "B2";
            // 
            // B1End
            // 
            this.B1End.Location = new System.Drawing.Point(100, 129);
            this.B1End.Name = "B1End";
            this.B1End.Size = new System.Drawing.Size(43, 23);
            this.B1End.TabIndex = 40;
            this.B1End.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // B1Start
            // 
            this.B1Start.Location = new System.Drawing.Point(49, 129);
            this.B1Start.Name = "B1Start";
            this.B1Start.Size = new System.Drawing.Size(43, 23);
            this.B1Start.TabIndex = 39;
            this.B1Start.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 15);
            this.label3.TabIndex = 38;
            this.label3.Text = "B1";
            // 
            // C1End
            // 
            this.C1End.Location = new System.Drawing.Point(100, 191);
            this.C1End.Name = "C1End";
            this.C1End.Size = new System.Drawing.Size(43, 23);
            this.C1End.TabIndex = 46;
            this.C1End.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // C1Start
            // 
            this.C1Start.Location = new System.Drawing.Point(49, 191);
            this.C1Start.Name = "C1Start";
            this.C1Start.Size = new System.Drawing.Size(43, 23);
            this.C1Start.TabIndex = 45;
            this.C1Start.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 15);
            this.label5.TabIndex = 44;
            this.label5.Text = "C1";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 387);
            this.Controls.Add(this.C1End);
            this.Controls.Add(this.C1Start);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.B2End);
            this.Controls.Add(this.B2Start);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.B1End);
            this.Controls.Add(this.B1Start);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.A2End);
            this.Controls.Add(this.A2Start);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.A1End);
            this.Controls.Add(this.positionLabel);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.A1Start);
            this.Controls.Add(this.maLabel);
            this.Controls.Add(this.button2);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form3";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.Form3_Load);
            ((System.ComponentModel.ISupportInitialize)(this.A1Start)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.A1End)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.A2End)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.A2Start)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2End)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B2Start)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1End)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.B1Start)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.C1End)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.C1Start)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown A1Start;
        private System.Windows.Forms.Label maLabel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label positionLabel;
        private System.Windows.Forms.NumericUpDown A1End;
        private System.Windows.Forms.NumericUpDown A2End;
        private System.Windows.Forms.NumericUpDown A2Start;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown B2End;
        private System.Windows.Forms.NumericUpDown B2Start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown B1End;
        private System.Windows.Forms.NumericUpDown B1Start;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown C1End;
        private System.Windows.Forms.NumericUpDown C1Start;
        private System.Windows.Forms.Label label5;

    }
}