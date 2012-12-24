using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlsiUtils;
using ExcelLink;
using System.Diagnostics;

namespace AlsiTrade_Backend
{
    public class MarketOrder
    {
        ExcelOrder e  = new ExcelOrder();
        public MarketOrder()
        {
            
            e.onMatch += new OrderMatched(e_onMatch);
        }

        public void SendOrderToMarket(Trade trade)
        {
            if (trade.BuyorSell != Trade.BuySell.None)
            {

                ExcelLink.xlTradeOrder o = new xlTradeOrder()
                {
                    BS = trade.BuyorSell,
                    Price = trade.CurrentPrice,
                    Volume = trade.TradeVolume,
                    Contract = trade.InstrumentName,

                };

                try
                {
                    e.Connect();
                    e.WriteOrder(o);
                    e.Disconnect();
                    OrderSendEvent oe = new OrderSendEvent();
                    oe.Success = true;
                    oe.Trade = trade;
                    onOrderSend(this, oe);
                    e.StartCheckWhenOrderCompletedTimer(10000);

                }
                catch (Exception ex)
                {
                    OrderSendEvent oe = new OrderSendEvent();
                    oe.Success = false;
                    oe.Trade = trade;
                    onOrderSend(this, oe);
                }
            }
        }



        void e_onMatch(object sender, OrderMatchEventArgs e)
        {
            var et = e.Order;
            Trade t = new Trade()
            {
                TimeStamp = DateTime.UtcNow.AddHours(2),
                TradedPrice = et.Price,
                TradeVolume = et.Volume,
                BuyorSell = et.BS,

            };
            OrderMatchEvent ome = new OrderMatchEvent();
            ome.Success = true;
            ome.Trade = t;
            onOrderMatch(this, ome);
        }

        public event OrderSend onOrderSend;
        public delegate void OrderSend(object sender, OrderSendEvent e);
        public class OrderSendEvent : EventArgs
        {
            public Trade Trade;
            public bool Success;
        }

        public event OrderMatch onOrderMatch;
        public delegate void OrderMatch(object sender, OrderMatchEvent e);
        public class OrderMatchEvent : EventArgs
        {
            public Trade Trade;
            public bool Success;
        }


    }
}
