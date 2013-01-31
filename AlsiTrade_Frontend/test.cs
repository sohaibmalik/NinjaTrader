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
        }

        void E_onMatch(object sender, OrderMatchEventArgs e)
        {
            listBox1.Items.Add(e.LastOrder.Price + "  " + e.LastOrder.Reference + "   " + e.LastOrder.Status);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            E.Connect();
            foreach (var t in E.ReadAllInputOrders())
            {
                listBoxINSERT.Items.Add(t.Price + "  " + t.Reference);
            }
            E.Disconnect();
        }

        private void test_Load(object sender, EventArgs e)
        {
         
            ExcelLink.xlTradeOrder xl;
           var Es= E.GetMatchedOrders(out xl);
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            E.Connect();
            foreach (var t in E.ReadAllMatchedOrders())
            {
                listBoxMATCHED.Items.Add(t.Price + "  " + t.Reference);
            }
            E.Disconnect();
        }

      
        
    }
}
