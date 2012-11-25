namespace NinjaTest
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
            this.button2 = new System.Windows.Forms.Button();
            this.BW = new System.ComponentModel.BackgroundWorker();
            this.rsiLabel = new System.Windows.Forms.Label();
            this.maLabel = new System.Windows.Forms.Label();
            this.ma2Label = new System.Windows.Forms.Label();
            this.midLLabel = new System.Windows.Forms.Label();
            this.midSlabel = new System.Windows.Forms.Label();
            this.cLLabel = new System.Windows.Forms.Label();
            this.cSLabel = new System.Windows.Forms.Label();
            this.rsiStart = new System.Windows.Forms.NumericUpDown();
            this.rsiEnd = new System.Windows.Forms.NumericUpDown();
            this.maEnd = new System.Windows.Forms.NumericUpDown();
            this.maStart = new System.Windows.Forms.NumericUpDown();
            this.ma2End = new System.Windows.Forms.NumericUpDown();
            this.ma2Start = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.rsiStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rsiEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ma2End)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ma2Start)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "startSim";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // BW
            // 
            this.BW.WorkerSupportsCancellation = true;
            this.BW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BW_DoWork);
            // 
            // rsiLabel
            // 
            this.rsiLabel.AutoSize = true;
            this.rsiLabel.Location = new System.Drawing.Point(29, 56);
            this.rsiLabel.Name = "rsiLabel";
            this.rsiLabel.Size = new System.Drawing.Size(28, 13);
            this.rsiLabel.TabIndex = 2;
            this.rsiLabel.Text = "RSI ";
            // 
            // maLabel
            // 
            this.maLabel.AutoSize = true;
            this.maLabel.Location = new System.Drawing.Point(29, 82);
            this.maLabel.Name = "maLabel";
            this.maLabel.Size = new System.Drawing.Size(23, 13);
            this.maLabel.TabIndex = 3;
            this.maLabel.Text = "MA";
            // 
            // ma2Label
            // 
            this.ma2Label.AutoSize = true;
            this.ma2Label.Location = new System.Drawing.Point(29, 113);
            this.ma2Label.Name = "ma2Label";
            this.ma2Label.Size = new System.Drawing.Size(29, 13);
            this.ma2Label.TabIndex = 4;
            this.ma2Label.Text = "MA2";
            // 
            // midLLabel
            // 
            this.midLLabel.AutoSize = true;
            this.midLLabel.Location = new System.Drawing.Point(29, 141);
            this.midLLabel.Name = "midLLabel";
            this.midLLabel.Size = new System.Drawing.Size(30, 13);
            this.midLLabel.TabIndex = 5;
            this.midLLabel.Text = "MidL";
            // 
            // midSlabel
            // 
            this.midSlabel.AutoSize = true;
            this.midSlabel.Location = new System.Drawing.Point(29, 174);
            this.midSlabel.Name = "midSlabel";
            this.midSlabel.Size = new System.Drawing.Size(31, 13);
            this.midSlabel.TabIndex = 6;
            this.midSlabel.Text = "MidS";
            // 
            // cLLabel
            // 
            this.cLLabel.AutoSize = true;
            this.cLLabel.Location = new System.Drawing.Point(29, 204);
            this.cLLabel.Name = "cLLabel";
            this.cLLabel.Size = new System.Drawing.Size(16, 13);
            this.cLLabel.TabIndex = 7;
            this.cLLabel.Text = "Cl";
            // 
            // cSLabel
            // 
            this.cSLabel.AutoSize = true;
            this.cSLabel.Location = new System.Drawing.Point(29, 238);
            this.cSLabel.Name = "cSLabel";
            this.cSLabel.Size = new System.Drawing.Size(19, 13);
            this.cSLabel.TabIndex = 8;
            this.cSLabel.Text = "Cs";
            // 
            // rsiStart
            // 
            this.rsiStart.Location = new System.Drawing.Point(123, 49);
            this.rsiStart.Name = "rsiStart";
            this.rsiStart.Size = new System.Drawing.Size(37, 20);
            this.rsiStart.TabIndex = 9;
            // 
            // rsiEnd
            // 
            this.rsiEnd.Location = new System.Drawing.Point(166, 49);
            this.rsiEnd.Name = "rsiEnd";
            this.rsiEnd.Size = new System.Drawing.Size(37, 20);
            this.rsiEnd.TabIndex = 10;
            // 
            // maEnd
            // 
            this.maEnd.Location = new System.Drawing.Point(166, 75);
            this.maEnd.Name = "maEnd";
            this.maEnd.Size = new System.Drawing.Size(37, 20);
            this.maEnd.TabIndex = 12;
            // 
            // maStart
            // 
            this.maStart.Location = new System.Drawing.Point(123, 75);
            this.maStart.Name = "maStart";
            this.maStart.Size = new System.Drawing.Size(37, 20);
            this.maStart.TabIndex = 11;
            // 
            // ma2End
            // 
            this.ma2End.Location = new System.Drawing.Point(166, 106);
            this.ma2End.Name = "ma2End";
            this.ma2End.Size = new System.Drawing.Size(37, 20);
            this.ma2End.TabIndex = 14;
            // 
            // ma2Start
            // 
            this.ma2Start.Location = new System.Drawing.Point(123, 106);
            this.ma2Start.Name = "ma2Start";
            this.ma2Start.Size = new System.Drawing.Size(37, 20);
            this.ma2Start.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 296);
            this.Controls.Add(this.ma2End);
            this.Controls.Add(this.ma2Start);
            this.Controls.Add(this.maEnd);
            this.Controls.Add(this.maStart);
            this.Controls.Add(this.rsiEnd);
            this.Controls.Add(this.rsiStart);
            this.Controls.Add(this.cSLabel);
            this.Controls.Add(this.cLLabel);
            this.Controls.Add(this.midSlabel);
            this.Controls.Add(this.midLLabel);
            this.Controls.Add(this.ma2Label);
            this.Controls.Add(this.maLabel);
            this.Controls.Add(this.rsiLabel);
            this.Controls.Add(this.button2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rsiStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rsiEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ma2End)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ma2Start)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.ComponentModel.BackgroundWorker BW;
        private System.Windows.Forms.Label rsiLabel;
        private System.Windows.Forms.Label maLabel;
        private System.Windows.Forms.Label ma2Label;
        private System.Windows.Forms.Label midLLabel;
        private System.Windows.Forms.Label midSlabel;
        private System.Windows.Forms.Label cLLabel;
        private System.Windows.Forms.Label cSLabel;
        private System.Windows.Forms.NumericUpDown rsiStart;
        private System.Windows.Forms.NumericUpDown rsiEnd;
        private System.Windows.Forms.NumericUpDown maEnd;
        private System.Windows.Forms.NumericUpDown maStart;
        private System.Windows.Forms.NumericUpDown ma2End;
        private System.Windows.Forms.NumericUpDown ma2Start;

    }
}

