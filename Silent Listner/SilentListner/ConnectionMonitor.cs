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
using System.Runtime.InteropServices;

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
                        logListBox.Items.Add(DateTime.Now.ToShortTimeString() + " Restarting OTS");
                        Process.Start(@"C:\Users\Pieter\Dropbox\Alsi Trade App\Batch Commands\RestartOTS.bat");
                        restartOTSTimer.Start();
                        updateTimer.Stop();
                       
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
                Debug.WriteLine("Internet Failed ...");             
                           
            
            }
           
            if (!FindWindow("Excel Link"))
            {

                this.Show();              
                //HasDisconnected = true;
                Debug.WriteLine("OTS Failed ...");
                updateTimer.Stop();
                restartOTSTimer.Start();
                logListBox.Items.Add(DateTime.Now.ToShortTimeString() + " OTS Failed ...");
                logListBox.Items.Add(DateTime.Now.ToShortTimeString() + " Restarting OTS...");
                Process.Start(@"C:\Users\Pieter\Dropbox\Alsi Trade App\Batch Commands\RestartOTS.bat");
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
            this.Show(); 
            Debug.WriteLine("Resuming Checks");
            logListBox.Items.Add(DateTime.Now.ToShortTimeString() + " Resuming Checks");
            restartOTSTimer.Stop();
            updateTimer.Start();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        public static bool FindWindow(string WindowName)
        {
            IntPtr hwnd = FindWindow(null, WindowName);        
            if ((int)hwnd == 0) return false;
            return true;
       
        }
      


      



    }
}
