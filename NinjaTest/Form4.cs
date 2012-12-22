using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AlsiTrade_Backend;
using AlsiUtils.Data_Objects;
using System.Diagnostics;

namespace NinjaTest
{
    public partial class Form4 : Form
    {
        private PrepareForTrade p;
        private UpdateTimer u1, u2;
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
           
            u2 = new UpdateTimer(GlobalObjects.TimeInterval.Minute_5);
            p = new PrepareForTrade();
            p.onPriceSync += new PrepareForTrade.PricesSynced(p_onPriceSync);


          
            u2.onStartUpdate += new UpdateTimer.StartUpdate(u2_onStartUpdate);

           
        }

        void p_onPriceSync(object sender, PrepareForTrade.PricesSyncedEvent e)
        {
            Debug.WriteLine("Prices Synced : " + e.ReadyForTradeCalcs);
        }

        void u2_onStartUpdate(object sender, UpdateTimer.StartUpDateEvent e)
        {
            Debug.WriteLine(e.Message + "  " + e.Interval);
            Debug.WriteLine("Getting priced");
            p.GetPricesFromWeb(GlobalObjects.TimeInterval.Minute_5);
          
        }

      

    }
}
