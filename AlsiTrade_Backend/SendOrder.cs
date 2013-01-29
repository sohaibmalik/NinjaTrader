using System;
using AlsiUtils;
using ExcelLink;

namespace AlsiTrade_Backend
{
    public class MarketOrder
    {
        ExcelOrder e = new ExcelOrder();
        public MarketOrder()
        {

            e.onMatch += new OrderMatched(e_onMatch);
        }

        public void SendOrderToMarket(Trade trade)
        {
            if (trade.BuyorSell != Trade.BuySell.None)
            {
                double price = adjustPriceToStrategy(trade);

                ExcelLink.xlTradeOrder o = new xlTradeOrder()
                {
                    BS = trade.BuyorSell,
                    Price = price,
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

        private double adjustPriceToStrategy(Trade trade)
        {
            double last = trade.CurrentPrice;
            double bid = HiSat.LivePrice.Bid;
            double offer = HiSat.LivePrice.Offer;
            double spread = WebSettings.TradeApproach.Spread;

            if (trade.BuyorSell == Trade.BuySell.Buy)
            {
                if (WebSettings.TradeApproach.Mode == WebSettings.TradeApproach.TradeMode.Hit) return offer;
                if (WebSettings.TradeApproach.Mode == WebSettings.TradeApproach.TradeMode.Aggressive) return last + spread;
            }


            if (trade.BuyorSell == Trade.BuySell.Sell)
            {
                if (WebSettings.TradeApproach.Mode == WebSettings.TradeApproach.TradeMode.Hit) return bid;
                if (WebSettings.TradeApproach.Mode == WebSettings.TradeApproach.TradeMode.Aggressive) return last - spread;
            }

            return last;
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
                InstrumentName = et.Contract,
                xlRef = et.Reference,

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
