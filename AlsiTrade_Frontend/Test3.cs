using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AlsiUtils;
using System.IO;
using System.Diagnostics;

namespace FrontEnd
{
    public partial class Test3 : Form
    {
        public Test3()
        {
            InitializeComponent();
        }

        private void Test3_Load(object sender, EventArgs e)
        {
            var price=new List<Price>();


            string line = string.Empty;
            StreamReader sr = new StreamReader(@"D:\rsi.txt");

            while ((line = sr.ReadLine()) != null)
            {
                var p = new Price()
                {
                    Close=double.Parse(line),
                };
                price.Add(p);
            }
            sr.Close();

            var rsi = AlsiUtils.Factory_Indicator.createEMA(75,price);

            StreamWriter sw = new StreamWriter(@"D:\DATATEST.txt");
            foreach (var r in rsi) sw.WriteLine(r.Ema);
            sw.Close();

            Close();
           
        }
    }
}
