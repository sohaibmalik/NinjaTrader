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
using ExcelLink;

namespace FrontEnd
{
    public partial class test : Form
    {
        public Trade _lastTrade;
        public test()
        {
            InitializeComponent();
        }

        private void test_Load(object sender, EventArgs e)
        {
            _lastTrade = new Trade
            {
               TradedPrice=1234,
               BuyorSell=Trade.BuySell.Buy,
               Reason=Trade.Trigger.OpenLong,
               TimeStamp=DateTime.Now,            
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

     

      
            

       

       

      
        
    }
}
