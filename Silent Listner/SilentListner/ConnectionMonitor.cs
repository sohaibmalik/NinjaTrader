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

        public bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {                 
                    if (HasDisconnected)
                    {
                        HasDisconnected = false;
                        restartOTSTimer.Start();
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        private bool HasDisconnected;
        private void updateTimer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine("Checking");
            if (!CheckForInternetConnection())
            {
                this.Show();
                logListBox.Items.Add(DateTime.Now.ToShortTimeString() + " Connection Failed.Resetting Network");
                Process.Start(@"C:\Users\Pieter\Dropbox\Alsi Trade App\Batch Commands\ResetNetwork.bat");
                HasDisconnected = true;
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

        private void restartOTSTimer_Tick(object sender, EventArgs e)
        {
            logListBox.Items.Add(DateTime.Now.ToShortTimeString() + " Restarting OTS");
            Process.Start(@"C:\Users\Pieter\Dropbox\Alsi Trade App\Batch Commands\RestartOTS.bat");
            restartOTSTimer.Stop();
        }





    }
}
