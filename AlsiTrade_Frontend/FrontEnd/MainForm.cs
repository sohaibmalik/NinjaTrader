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
using AlsiUtils.Strategies;
using System.Diagnostics;

namespace FrontEnd
{
    public partial class MainForm : Form
    {
        private PrepareForTrade p;
        private UpdateTimer U5;
        MarketOrder marketOrder;
        GlobalObjects.TimeInterval _Interval;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _Interval = GlobalObjects.TimeInterval.Minute_5;
            AlsiUtils.DataBase.SetConnectionString();
            U5 = new UpdateTimer(_Interval);
            p = new PrepareForTrade();
            marketOrder = new MarketOrder();
            p.onPriceSync+=new PrepareForTrade.PricesSynced(p_onPriceSync);
            U5.onStartUpdate +=new UpdateTimer.StartUpdate(U5_onStartUpdate);
            marketOrder.onOrderSend +=new MarketOrder.OrderSend(marketOrder_onOrderSend);
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

        void U5_onStartUpdate(object sender, UpdateTimer.StartUpDateEvent e)
        {
            Debug.WriteLine(e.Message + "  " + e.Interval);
            Debug.WriteLine("Getting priced");
            p.GetPricesFromWeb(_Interval);    
        }

        void p_onPriceSync(object sender, PrepareForTrade.PricesSyncedEvent e)
        {
            Debug.WriteLine("Prices Synced : " + e.ReadyForTradeCalcs);
            var trades = RunCalcs.RunEMAScalpLiveTrade(GetParameters(),_Interval, false);
            var lt = trades.Last();
            Debug.WriteLine("====================");
            Debug.WriteLine("===Current Trade");
            Debug.WriteLine(lt.TimeStamp + "  " + lt.CurrentPrice + "  " + lt.CurrentDirection + "  " + lt.BuyorSell);
            marketOrder.SendOrderToMarket(lt, 2, "ALSI Contract");
        }

      private Parameter_EMA_Scalp GetParameters()
        {
            AlsiUtils.Strategies.Parameter_EMA_Scalp E = new AlsiUtils.Strategies.Parameter_EMA_Scalp()
            {
                A_EMA1 = 10,
                A_EMA2 = 14,
                B_EMA1 = 24,
                B_EMA2 = 28,
                C_EMA = 30,
                TakeProfit = 500,
                StopLoss = -500,
                CloseEndofDay = false,
                Period = GlobalObjects.Prices.Count,
            };

            return E;
        }
    }
}
