namespace NotifierClientApp
{
    partial class ChangeUserNameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeUserNameForm));
            this.OKButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.usernameInputTextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(13, 63);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(96, 63);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter new username";
            // 
            // usernameInputTextbox
            // 
            this.usernameInputTextbox.Location = new System.Drawing.Point(13, 30);
            this.usernameInputTextbox.Name = "usernameInputTextbox";
            this.usernameInputTextbox.Size = new System.Drawing.Size(158, 20);
            this.usernameInputTextbox.TabIndex = 3;
            this.usernameInputTextbox.TextChanged += new System.EventHandler(this.usernameInputTextbox_TextChanged);
            // 
            // ChangeUserNameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(191, 98);
            this.ControlBox = false;
            this.Controls.Add(this.usernameInputTextbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.OKButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChangeUserNameForm";
            this.Text = "Change Username";
            this.Load += new System.EventHandler(this.ChangeUserNameForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox usernameInputTextbox;
    }
}