using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using AlsiUtils.Data_Objects;
using AlsiUtils;
using AlsiTrade_Backend;
using System.Threading;
namespace NinjaTest
{
    public partial class Form3 : Form
    {
        LuanchScalp luanch;
        EmaSettings ema;
        public Form3()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

        }
        
        public void UpdatePos(string pos)
        {
            positionLabel.Text = pos;

        }
        public void UpdateDisplay(string msg)
        {
            richTextBox1.AppendText(msg);
           
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            richTextBox1.BackColor = Color.Black;
            richTextBox1.ForeColor = Color.LightGreen;
            richTextBox1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
             ema = new EmaSettings()
            {
                A1_start=(int)A1Start.Value,
                A1_end = (int)A1End.Value,

                A2_start = (int)A1Start.Value,
                A2_end = (int)A1End.Value,

                B1_start = (int)B1Start.Value,
                B1_end = (int)B1End.Value,

                B2_start = (int)B2Start.Value,
                B2_end = (int)B2End.Value,

                C1_start = (int)C1Start.Value,
                C1_end = (int)C1End.Value,
              

            };



            backgroundWorker1.RunWorkerAsync();
       

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            luanch = new LuanchScalp(this, ema);
            luanch.RunMultiple();
            //luanch.RunSingleNEW();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            button2.BackColor = Color.Green;
        }

       

    }

}