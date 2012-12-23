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
        MarketOrder marketOrder;
        public Form4()
        {
            InitializeComponent();
        }



        private void Form4_Load(object sender, EventArgs e)
        {
            AlsiUtils.DataBase.SetConnectionString();
            u2 = new UpdateTimer(GlobalObjects.TimeInterval.Minute_2);
            p = new PrepareForTrade();
            marketOrder = new MarketOrder();
            p.onPriceSync += new PrepareForTrade.PricesSynced(p_onPriceSync);          
            u2.onStartUpdate += new UpdateTimer.StartUpdate(u2_onStartUpdate);
            marketOrder.onOrderSend += new MarketOrder.OrderSend(marketOrder_onOrderSend);
            marketOrder.onOrderMatch += new MarketOrder.OrderMatch(marketOrder_onOrderMatch);
        }

        void marketOrder_onOrderMatch(object sender, MarketOrder.OrderMatchEvent e)
        {
            Debug.WriteLine("MATCHED : " + e.Success);
            Debug.WriteLine("Trade : " + e.Trade.TimeStamp + "  " + e.Trade.BuyorSell + "  " + e.Trade.CurrentDirection + "  " + e.Trade.TradeVolume);
        }

        void marketOrder_onOrderSend(object sender, MarketOrder.OrderSendEvent e)
        {
            Debug.WriteLine("Success : " + e.Success);
            Debug.WriteLine("Trade : " + e.Trade.TimeStamp + "  " + e.Trade.BuyorSell + "  " + e.Trade.CurrentDirection + "  " + e.Trade.TradeVolume);
        }



        void p_onPriceSync(object sender, PrepareForTrade.PricesSyncedEvent e)
        {
            Debug.WriteLine("Prices Synced : " + e.ReadyForTradeCalcs);
            var trades =new List<AlsiUtils.Trade>();//RunCalcs.RunEMAScalpLiveTrade(GlobalObjects.TimeInterval.Minute_2 ,false);
            var lt = trades.Last();           
            Debug.WriteLine("====================");
            Debug.WriteLine("===Current Trade");
            Debug.WriteLine(lt.TimeStamp + "  " + lt.CurrentPrice + "  " + lt.CurrentDirection  + "  " + lt.BuyorSell);          
            marketOrder.SendOrderToMarket(lt, 2, "ALSI Contract");

        }

        void u2_onStartUpdate(object sender, UpdateTimer.StartUpDateEvent e)
        {
            Debug.WriteLine(e.Message + "  " + e.Interval);
            Debug.WriteLine("Getting priced");
            p.GetPricesFromWeb(GlobalObjects.TimeInterval.Minute_2);          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            p.GetPricesFromWeb(GlobalObjects.TimeInterval.Minute_2);    
           
        }

      

    }
}
