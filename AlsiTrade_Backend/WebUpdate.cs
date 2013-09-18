using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlsiUtils;
using System.Diagnostics;
using System.ComponentModel;


namespace AlsiTrade_Backend
{

    public class WebUpdate
    {

        private static AlsiWebService.AlsiNotifyService service;
        public static List<tblEmail> _EmailList = new List<tblEmail>();
        public static List<tblSM> _SMSList = new List<tblSM>();

        public WebUpdate()
        {
            service = new AlsiWebService.AlsiNotifyService();
          
            GetEmailList();
             GetSmsList();
        }


        public static void GetEmailList()
        {
            var dc = new AlsiWebDataContext();
            _EmailList = dc.tblEmails.OrderBy(z => z.Email_ID).ToList();
        }

        public static void GetSmsList()
        {
            var dc = new AlsiWebDataContext();
            _SMSList = dc.tblSMs.OrderBy(z => z.SMS_ID).ToList();
        }

        public static void CheckUncheckEmailListUser(int UserID)
        {
            var dc = new AlsiWebDataContext();
            var user = dc.tblEmails.Where(z => z.Email_ID == UserID).First();
            user.Active = !user.Active;
            dc.SubmitChanges();
            GetEmailList();
        }
        public static void CheckUncheckSmsListUser(int UserID)
        {
            var dc = new AlsiWebDataContext();
            var user = dc.tblSMs.Where(z => z.SMS_ID == UserID).First();
            user.Active = !user.Active;
            dc.SubmitChanges();
            GetSmsList();
        }

        public static void DeleteUserFromEmailList(int UserID)
        {
            var dc = new AlsiWebDataContext();
            var user = dc.tblEmails.Where(z => z.Email_ID == UserID).First();
            dc.tblEmails.DeleteOnSubmit(user);
            dc.SubmitChanges();
            GetEmailList();
        }

        public static void DeleteUserFromSmsList(int UserID)
        {
            var dc = new AlsiWebDataContext();
            var user = dc.tblSMs.Where(z => z.SMS_ID == UserID).First();
            dc.tblSMs.DeleteOnSubmit(user);
            dc.SubmitChanges();
            GetSmsList();
        }

        public static bool InsertNewUsertoEmailList(tblEmail user)
        {
            var dc = new AlsiWebDataContext();
            var insert = (!dc.tblEmails.Any(z => z.Email == user.Email));
            if (insert)
            {
                user.Active = true;
                dc.tblEmails.InsertOnSubmit(user);
                dc.SubmitChanges();
                GetEmailList();
            }
            return insert;
        }

        public static bool InsertNewUsertoSmsList(tblSM user)
        {
            var dc = new AlsiWebDataContext();
            var insert = (!dc.tblSMs.Any(z => z.TelNr == user.TelNr));
            if (insert)
            {
                user.Active = true;
                dc.tblSMs.InsertOnSubmit(user);
                dc.SubmitChanges();
                GetSmsList();
            }
            return insert;
        }

        public void ReportStatus()
        {
            var b = new AlsiWebService.Boodskap()
            {
                TimeStamp = DateTime.UtcNow.AddHours(2),
                Message = AlsiWebService.Messages.isAlive,
                Message_Custom = "AlsiTrade"

            };

            service.InsertMessage(b);
        }

        public bool CheckForManualClose()
        {
            return  service.GetManualTradeTrigger();                      
        }

        public static void SetManualTradeTrigger(bool bb)
        {
            //tells the service to reset
            service.TriggerManualTrade(bb);
        }

        public static void SendOrder(Trade t, bool Matched)
        {
            var o = new AlsiWebService.xlTradeOrder();
            if (t.BuyorSell == Trade.BuySell.Buy) o.BS = AlsiWebService.BuySell.Buy;
            if (t.BuyorSell == Trade.BuySell.Sell) o.BS = AlsiWebService.BuySell.Sell;
            o.Timestamp = t.TimeStamp;
            o.Volume = t.TradeVolume;
            o.Price = t.TradedPrice;
            o.Contract = t.InstrumentName;
            o.Status = Matched ? AlsiWebService.orderStatus.Completed : AlsiWebService.orderStatus.Ready;
            service.InsertNewOrder(o);
            var s = new AlsiWebService.AlsiNotifyService();
            s.InsertNewOrder(o);

            if (t.Reason == Trade.Trigger.CloseLong || t.Reason == Trade.Trigger.CloseLong) WebSettings.General.MANUAL_CLOSE_TRIGGER = true;
        }

        public static void SyncOnlineDbTradeHistory(List<Trade> TradeOnly)
        {
            //var dc = new AlsiUtils.AlsiWebDataContext();

            //DateTime? lastWeb = new DateTime(1990, 01, 01);
            //if (dc.TradeHistories.Count() > 0)
            //{
            //    lastWeb = dc.TradeHistories.Max(z => z.TimeStamp);
            //}

            //var dellist = dc.TradeHistories.Where(z => z.TimeStamp >= lastWeb);
            //dc.TradeHistories.DeleteAllOnSubmit(dellist);
            //dc.SubmitChanges();

            //var synclist = TradeOnly.Where(z => z.TimeStamp >= lastWeb);

            //foreach (var t in synclist)
            //{
            //    var th = new TradeHistory()
            //    {
            //        TimeStamp = t.TimeStamp,
            //        BuySell = t.BuyorSell.ToString(),
            //        Reason = t.Reason.ToString(),
            //        Price = (int)t.TradedPrice,
            //        Volume = t.TradeVolume,
            //        Trade_Profit = (int)t.RunningProfit
            //    };

            //    dc.TradeHistories.InsertOnSubmit(th);
            //    Debug.WriteLine(th.TimeStamp + "  " + th.Volume);
            //}


            //dc.SubmitChanges();
        }

        public static void SendOrderToWebDB(Trade trade)
        {
            var dc = new AlsiWebDataContext();
            string bs = "none";
            if (trade.BuyorSell == Trade.BuySell.Buy) bs = "Buy";
            if (trade.BuyorSell == Trade.BuySell.Sell) bs = "Sell";
            if (!CheckDbCount(dc, trade)) return;

            if (!trade.xlMatched)
            {
                WebTradeLog wtl = new WebTradeLog()
                {
                    Time = trade.TimeStamp,
                    BuySell = trade.BuyorSell.ToString(),
                    Price = (int)trade.TradedPrice,
                    Reason = trade.Reason.ToString(),
                    Volume = trade.TradeVolume,
                    PriceMatched = 0,
                    Matched = false,
                };
                dc.WebTradeLogs.InsertOnSubmit(wtl);
                dc.SubmitChanges();
            }
            else
            {
                int c = dc.WebTradeLogs.Count();
                var last = dc.WebTradeLogs.Skip(c - 1).Take(1).Single();
                last.Time = trade.TimeStamp;
                last.Matched = true;
                dc.SubmitChanges();
            }

        }

       

        private static bool CheckDbCount(AlsiWebDataContext dc, Trade trade)
        {
            int c = dc.WebTradeLogs.Count();
            if (c > 0)
            {
                //var  last = dc.WebTradeLogs.Skip(c - 10).Take(10);
            }
            else
            {//create new if db is empty
                WebTradeLog wtl = new WebTradeLog()
                {
                    Time = trade.TimeStamp,
                    BuySell = trade.BuyorSell.ToString(),
                    Price = (int)trade.TradedPrice,
                    Reason = trade.Reason.ToString(),
                    Volume = trade.TradeVolume,
                    PriceMatched = (int)trade.TradedPrice,
                    Matched = false,
                };
                dc.WebTradeLogs.InsertOnSubmit(wtl);
                dc.SubmitChanges();
                return false;
            }
            return true;
        }

        #region Clear Lists

        public static void ClearEmailList()
        {
            var dc = new AlsiWebDataContext();
            var delall = dc.tblEmails;
            dc.tblEmails.DeleteAllOnSubmit(delall);
            dc.SubmitChanges();
        }

        public static void ClearTradeLog()
        {
            var dc = new AlsiWebDataContext();
            var delall = dc.WebTradeLogs;
            dc.WebTradeLogs.DeleteAllOnSubmit(delall);
            dc.SubmitChanges();
        }

        public static void ClearTadeHistory()
        {
            //var dc = new AlsiWebDataContext();
            //var delall = dc.TradeHistories;
            //dc.TradeHistories.DeleteAllOnSubmit(delall);
            //dc.SubmitChanges();
        }


        #endregion


      

    }
}
