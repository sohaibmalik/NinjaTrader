using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AlsiTrade_Backend;
using Communicator;
using AlsiUtils;
using System.Diagnostics;

namespace FrontEnd
{
    public partial class test : Form
    {
        public test()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var d = GetAlgoTime();
            Debug.WriteLine(d);
           
        }

        private void test_Load(object sender, EventArgs e)
        {
            var d = GetAlgoTime();
            Debug.WriteLine(d);
        }

        public static DateTime GetAlgoTime()
        {
            var dt = DateTime.UtcNow.AddHours(2);
            int _5min = (dt.Minute) % 5;
            int _1min = dt.Minute % 10;
            var _temin = dt.AddMinutes(-_1min).AddSeconds(-dt.Second);

            int m = 0;
            if (_5min == 0) m = dt.Minute - 5;
            else
                m = dt.Minute - 5 - _5min;

            dt = dt.AddMinutes(-dt.Minute).AddMinutes(m).AddSeconds(-dt.Second);
            return dt;
        }
        
    }
}
