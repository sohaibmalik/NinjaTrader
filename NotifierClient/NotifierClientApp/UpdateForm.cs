using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace NotifierClientApp
{
    public partial class UpdateForm : Form
    {
        private string link;

        public UpdateForm(string Link)
        {
            InitializeComponent();
            link = Link;
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            linkLabel1.Text = link;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(link);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
