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
        public static List<EmailList> _EmailList = new List<EmailList>();

        public WebUpdate()
        {
            service = new AlsiWebService.AlsiNotifyService();
            GetEmailList();
        }


        public static void GetEmailList()
        {
            var dc = new WebDbDataContext();
            _EmailList = dc.EmailLists.OrderBy(z => z.ID).ToList();
        }

        public static void CheckUncheckEmailListUser(int UserID)
        {
            var dc = new WebDbDataContext();
            var user = dc.EmailLists.Where(z => z.ID == UserID).First();
            user.Active = !user.Active;
            dc.SubmitChanges();
            GetEmailList();
        }

        public static void DeleteUserFromEmailList(int UserID)
        {
            var dc = new WebDbDataContext();
            var user = dc.EmailLists.Where(z => z.ID == UserID).First();
            dc.EmailLists.DeleteOnSubmit(user);
            dc.SubmitChanges();
            GetEmailList();
        }

        public static bool InsertNewUsertoEmailList(EmailList user)
        {
            var dc = new WebDbDataContext();
            var insert = (!dc.EmailLists.Any(z => z.Email == user.Email));
            if (insert)
            {
                user.Active = true;
                dc.EmailLists.InsertOnSubmit(user);
                dc.SubmitChanges();
                GetEmailList();
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
        }

        public static void SyncOnlineDbTradeHistory(List<Trade> TradeOnly)
        {
            var dc = new AlsiUtils.WebDbDataContext();

            DateTime? lastWeb = new DateTime(1990, 01, 01);
            if (dc.TradeHistories.Count() > 0)
            {
                lastWeb = dc.TradeHistories.Max(z => z.TimeStamp);
            }

            var dellist = dc.TradeHistories.Where(z => z.TimeStamp >= lastWeb);
            dc.TradeHistories.DeleteAllOnSubmit(dellist);
            dc.SubmitChanges();

            var synclist = TradeOnly.Where(z => z.TimeStamp >= lastWeb);

            foreach (var t in synclist)
            {
                var th = new TradeHistory()
                {
                    TimeStamp = t.TimeStamp,
                    BuySell = t.BuyorSell.ToString(),
                    Reason = t.Reason.ToString(),
                    Price = (int)t.TradedPrice,
                    Volume = t.TradeVolume,
                    Trade_Profit = (int)t.RunningProfit
                };

                dc.TradeHistories.InsertOnSubmit(th);
                Debug.WriteLine(th.TimeStamp + "  " + th.Volume);
            }


            dc.SubmitChanges();
        }

        public static void SendOrderToWebDB(Trade trade)
        {
            var dc = new WebDbDataContext();
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


        private static bool CheckDbCount(WebDbDataContext dc, Trade trade)
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
            var dc = new WebDbDataContext();
            var delall = dc.EmailLists;
            dc.EmailLists.DeleteAllOnSubmit(delall);
            dc.SubmitChanges();
        }

        public static void ClearTradeLog()
        {
            var dc = new WebDbDataContext();
            var delall = dc.WebTradeLogs;
            dc.WebTradeLogs.DeleteAllOnSubmit(delall);
            dc.SubmitChanges();
        }

        public static void ClearTadeHistory()
        {
            var dc = new WebDbDataContext();
            var delall = dc.TradeHistories;
            dc.TradeHistories.DeleteAllOnSubmit(delall);
            dc.SubmitChanges();
        }


        #endregion


    }
}
