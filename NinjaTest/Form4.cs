using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AlsiTrade_Backend;
using AlsiUtils;
namespace NinjaTest
{
    public partial class Form4 : Form
    {
        private WebUpdate service = new WebUpdate();

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            AlsiUtils.DataBase.SetConnectionString();
            DoStuff.GetDataFromTick.DoYourThing("MAR13ALSI");
            timer1.Interval = 2000;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            service.ReportStatus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }
    }
}
