namespace FrontEnd
{
    partial class StartupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartupForm));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.pcStringRadioButton = new System.Windows.Forms.RadioButton();
            this.laptopStringRadioButton = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 12);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(572, 33);
            this.progressBar1.TabIndex = 0;
            // 
            // pcStringRadioButton
            // 
            this.pcStringRadioButton.AutoSize = true;
            this.pcStringRadioButton.Location = new System.Drawing.Point(13, 80);
            this.pcStringRadioButton.Name = "pcStringRadioButton";
            this.pcStringRadioButton.Size = new System.Drawing.Size(14, 13);
            this.pcStringRadioButton.TabIndex = 2;
            this.pcStringRadioButton.UseVisualStyleBackColor = true;
            this.pcStringRadioButton.CheckedChanged += new System.EventHandler(this.pcStringRadioButton_CheckedChanged);
            // 
            // laptopStringRadioButton
            // 
            this.laptopStringRadioButton.AutoSize = true;
            this.laptopStringRadioButton.Location = new System.Drawing.Point(12, 104);
            this.laptopStringRadioButton.Name = "laptopStringRadioButton";
            this.laptopStringRadioButton.Size = new System.Drawing.Size(14, 13);
            this.laptopStringRadioButton.TabIndex = 3;
            this.laptopStringRadioButton.UseVisualStyleBackColor = true;
            this.laptopStringRadioButton.CheckedChanged += new System.EventHandler(this.laptopStringRadioButton_CheckedChanged);
            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 53);
            this.ControlBox = false;
            this.Controls.Add(this.laptopStringRadioButton);
            this.Controls.Add(this.pcStringRadioButton);
            this.Controls.Add(this.progressBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StartupForm";
            this.Text = "Loading...";
            this.Load += new System.EventHandler(this.StartupForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RadioButton pcStringRadioButton;
        private System.Windows.Forms.RadioButton laptopStringRadioButton;
    }
}