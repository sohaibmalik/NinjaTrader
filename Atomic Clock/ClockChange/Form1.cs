using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace ClockChange
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void setButton_Click(object sender, EventArgs e)
        {
            int hour =(int) numericUpDownHOUR.Value;
            int min = (int)numericUpDownMINUTE.Value;
            int sec = (int)numericUpDownSECOND.Value;

            string command = "Time " + hour.ToString() + ":" + min.ToString() + ":" + sec.ToString();
            StreamWriter sr = new StreamWriter(@"C:\time.bat");
            sr.WriteLine(command);
            sr.Close();
            Process.Start(@"C:\time.bat");
            System.Threading.Thread.Sleep(1000);
            File.Delete(@"C:\time.bat");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var f = new Form2();
            f.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.BackColor = SynchTime() ? Color.Green : Color.Orange;
            
        }

        private bool SynchTime()
        {
            bool synched = false;

            try
            {
                Nist.NistClock c = new Nist.NistClock();
                c.SynchronizeLocalClock();
                synched = true;
            }
            catch { }

            return synched; 
        }
    }
}
