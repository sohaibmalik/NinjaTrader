using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

namespace SilentListner
{
    public partial class ConnectionMonitor : Form
    {
       

        public ConnectionMonitor()
        {
            InitializeComponent();
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

      

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine("Checking");
            if (!CheckForInternetConnection())
            {
                this.Show();
                logListBox.Items.Add(DateTime.Now.ToShortTimeString() + " Connection Failed.Resetting Network");
                Process.Start(@"C:\Users\Pieter\Dropbox\Alsi Trade App\Batch Commands\ResetNetwork.bat");
            }
        }

      

        private void ConnectionMonitor_Load(object sender, EventArgs e)
        {
            this.Hide();
            Refresh();
        }

        private void ConnectionMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (true)
            {
                e.Cancel = true;
                this.Hide();                      
            }
        
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {                       
            this.Show();
        }


       

       
    }
}
