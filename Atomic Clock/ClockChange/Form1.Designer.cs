namespace ClockChange
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.numericUpDownHOUR = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMINUTE = new System.Windows.Forms.NumericUpDown();
            this.setButton = new System.Windows.Forms.Button();
            this.numericUpDownSECOND = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHOUR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMINUTE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSECOND)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDownHOUR
            // 
            this.numericUpDownHOUR.Location = new System.Drawing.Point(15, 52);
            this.numericUpDownHOUR.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numericUpDownHOUR.Name = "numericUpDownHOUR";
            this.numericUpDownHOUR.Size = new System.Drawing.Size(52, 23);
            this.numericUpDownHOUR.TabIndex = 0;
            this.numericUpDownHOUR.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // numericUpDownMINUTE
            // 
            this.numericUpDownMINUTE.Location = new System.Drawing.Point(75, 52);
            this.numericUpDownMINUTE.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericUpDownMINUTE.Name = "numericUpDownMINUTE";
            this.numericUpDownMINUTE.Size = new System.Drawing.Size(52, 23);
            this.numericUpDownMINUTE.TabIndex = 1;
            // 
            // setButton
            // 
            this.setButton.Location = new System.Drawing.Point(15, 97);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(165, 79);
            this.setButton.TabIndex = 2;
            this.setButton.Text = "Set";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // numericUpDownSECOND
            // 
            this.numericUpDownSECOND.Location = new System.Drawing.Point(134, 52);
            this.numericUpDownSECOND.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericUpDownSECOND.Name = "numericUpDownSECOND";
            this.numericUpDownSECOND.Size = new System.Drawing.Size(52, 23);
            this.numericUpDownSECOND.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Hour";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Minute";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(134, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Second";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 182);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(165, 79);
            this.button1.TabIndex = 7;
            this.button1.Text = "Sych Atomic";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(207, 286);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownSECOND);
            this.Controls.Add(this.setButton);
            this.Controls.Add(this.numericUpDownMINUTE);
            this.Controls.Add(this.numericUpDownHOUR);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Time Machine";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHOUR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMINUTE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSECOND)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDownHOUR;
        private System.Windows.Forms.NumericUpDown numericUpDownMINUTE;
        private System.Windows.Forms.Button setButton;
        private System.Windows.Forms.NumericUpDown numericUpDownSECOND;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
    }
}

