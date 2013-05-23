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
            StreamReader sr = new StreamReader(@"D:\ohlcPLAll.txt");

            while ((line = sr.ReadLine()) != null)
            {
                var str = line.Split(',');
                var p = new Price()
                {
                    Open=double.Parse(str[1]),
                    High = double.Parse(str[2]),
                    Low = double.Parse(str[3]),
                    Close = double.Parse(str[4]),


                };
                price.Add(p);
            }
            sr.Close();

            var rsi = AlsiUtils.Factory_Indicator.createBollingerBand(20, 2, price, TicTacTec.TA.Library.Core.MAType.Ema);

            StreamWriter sw = new StreamWriter(@"D:\indicator.txt");
            foreach (var r in rsi) sw.WriteLine(r.Price_Close +","+r.Upper+","+r.Lower);
            sw.Close();

            Close();
           
        }
    }
}
