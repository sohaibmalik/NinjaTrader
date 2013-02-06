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
        private ExcelLink.ExcelOrder E = new ExcelOrder();

        public test()
        {
            InitializeComponent();
            E.onMatch += new OrderMatched(E_onMatch);
            CheckForIllegalCrossThreadCalls = false;
        }

        void E_onMatch(object sender, OrderMatchEventArgs e)
        {
            this.BackColor = Color.Green;
            label1.Text = e.LastOrder.Price.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            xlTradeOrder ee = new xlTradeOrder();
            ee.BS = Trade.BuySell.Buy;
            ee.Price = 1233;
            ee.TimeStamp = DateTime.Now;
            ee.Status = xlTradeOrder.orderStatus.Ready;
            E.Connect();
            E.WriteOrder(ee);
            E.Disconnect();

            E.StartCheckWhenOrderCompletedTimer(5000);
        }

      
            

       

       

      
        
    }
}
