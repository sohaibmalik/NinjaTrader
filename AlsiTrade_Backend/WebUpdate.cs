using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlsiUtils;

namespace AlsiTrade_Backend
{
    
    public class WebUpdate
    {
      private static  AlsiWebService.AlsiNotifyService service;

        public WebUpdate()
        {
            service = new AlsiWebService.AlsiNotifyService();
        }

        public  void ReportStatus()
        {
            var b = new AlsiWebService.Boodskap()
            {
                TimeStamp=DateTime.UtcNow.AddHours(2),
                Message=AlsiWebService.Messages.isAlive,
                Message_Custom="AlsiTrade"                              

            };

            service.InsertMessage(b);
        }

        public static void SendOrder(Trade t,bool Matched)
        {
            var o = new AlsiWebService.xlTradeOrder();
           
            o.Timestamp=t.TimeStamp;
            o.Volume=t.TradeVolume;
            o.Price=t.TradedPrice;
            o.Contract=t.InstrumentName;
            o.Status = Matched ? AlsiWebService.orderStatus.Completed : AlsiWebService.orderStatus.Ready;
            service.InsertNewOrder(o);

            var s = new AlsiWebService.AlsiNotifyService();
            s.InsertNewOrder(o);
        }

       
        public static void SendOrderToWebDB(Trade trade)
        {
            var dc = new WebDbDataContext();
            string bs = "none";
            if(trade.BuyorSell==Trade.BuySell.Buy)bs="Buy";
             if(trade.BuyorSell==Trade.BuySell.Sell)bs="Sell";
           WebTradeLog wtl = new WebTradeLog()
            {
                Time=trade.TimeStamp,
                BuySell=trade.BuyorSell.ToString(),
                Price=(int)trade.TradedPrice,
                Reason=trade.Reason.ToString(),
                Volume=trade.TradeVolume,
               
            };
            dc.WebTradeLogs.InsertOnSubmit(wtl);
            dc.SubmitChanges();
        }
    }
}
