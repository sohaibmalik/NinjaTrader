using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClockChange
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.UtcNow.AddHours(2).ToString("hh:mm:ss.fff tt");
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            timer1.Interval = 100;
            timer1.Start();
        }
    }
}
