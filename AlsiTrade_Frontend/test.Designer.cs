namespace FrontEnd
{
    partial class test
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.listBoxINSERT = new System.Windows.Forms.ListBox();
            this.listBoxMATCHED = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Inserted";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(616, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Matched";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listBoxINSERT
            // 
            this.listBoxINSERT.FormattingEnabled = true;
            this.listBoxINSERT.Location = new System.Drawing.Point(12, 58);
            this.listBoxINSERT.Name = "listBoxINSERT";
            this.listBoxINSERT.Size = new System.Drawing.Size(198, 264);
            this.listBoxINSERT.TabIndex = 2;
            // 
            // listBoxMATCHED
            // 
            this.listBoxMATCHED.FormattingEnabled = true;
            this.listBoxMATCHED.Location = new System.Drawing.Point(493, 41);
            this.listBoxMATCHED.Name = "listBoxMATCHED";
            this.listBoxMATCHED.Size = new System.Drawing.Size(198, 264);
            this.listBoxMATCHED.TabIndex = 3;
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(254, 45);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(198, 277);
            this.listBox1.TabIndex = 4;
            // 
            // test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 411);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.listBoxMATCHED);
            this.Controls.Add(this.listBoxINSERT);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "test";
            this.Text = "test";
            this.Load += new System.EventHandler(this.test_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox listBoxINSERT;
        private System.Windows.Forms.ListBox listBoxMATCHED;
        private System.Windows.Forms.ListBox listBox1;
    }
}