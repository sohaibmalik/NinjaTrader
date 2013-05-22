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
            StreamReader sr = new StreamReader(@"D:\PLDATA.csv");

            while ((line = sr.ReadLine()) != null)
            {
                var p = new Price()
                {
                    Close=double.Parse(line),
                };
                price.Add(p);
            }
            sr.Close();

						var rsi = AlsiUtils.Factory_Indicator.createAroon(20, price);

            StreamWriter sw = new StreamWriter(@"D:\DATATEST.txt");
            foreach (var r in rsi) sw.WriteLine(r.Price_Close +","+r.Aroon_Up+","+r.Aroon_Down);
            sw.Close();

            Close();
           
        }
    }
}
