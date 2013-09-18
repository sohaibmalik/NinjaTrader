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
                double price = WebSettings.TradeApproach.AdjustPriceToStrategy(trade, HiSat.LivePrice.Bid, HiSat.LivePrice.Offer);

                ExcelLink.xlTradeOrder o = new xlTradeOrder()
                {
                    BS = trade.BuyorSell,
                    Price = price,
                    Volume = trade.TradeVolume,
                    Contract = trade.InstrumentName,

                };

                try
                {

                    if (ManualTrade.CanCloseTrade(trade))
                    {
                        WebUpdate.SetManualTradeTrigger(false);
                        e.Connect();
                        e.WriteOrder(o);
                        e.Disconnect();
                        OrderSendEvent oe = new OrderSendEvent();
                        oe.Success = true;
                        oe.Trade = trade;
                        onOrderSend(this, oe);
                        e.StartCheckWhenOrderCompletedTimer(10000);
                    }
                
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

        public void SendOrderToMarketMANUALCLOSE(Trade trade)
        {
            double price = trade.BuyorSell == Trade.BuySell.Buy ? HiSat.LivePrice.Offer : HiSat.LivePrice.Bid;           
          
                ExcelLink.xlTradeOrder o = new xlTradeOrder()
                {
                    BS = trade.BuyorSell,
                    Price = price,
                    Volume = trade.TradeVolume,
                    Contract = WebSettings.General.OTS_INST,

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
        

        void e_onMatch(object sender, OrderMatchEventArgs e)
        {
            var et = e.LastOrder;

            
            Trade t = new Trade()
            {
                TimeStamp = e.Matched ? e.LastOrder.TimeStamp : DateTime.UtcNow.AddHours(2),
                TradedPrice = et.Price,
                TradeVolume = et.Volume,
                BuyorSell = et.BS,
                InstrumentName = et.Contract,
                xlRef = et.Reference,
                xlMatched = e.Matched,

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
