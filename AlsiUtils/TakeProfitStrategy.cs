using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AlsiUtils
{
    public class TakeProfitStrategy
    {
        private List<Trade> _FullTradeList = new List<Trade>();

        public void TakeProfit(List<Trade> FullTradeListIn, int tp, int period)
        {
            foreach (var a in FullTradeListIn)
            {
                Trade b = (Trade)a.Clone();
                _FullTradeList.Add(b);
            }
            var profold = FullTradeListIn.Where(z => z.Reason == Trade.Trigger.CloseShort || z.Reason == Trade.Trigger.CloseLong).Last().TotalPL;

          //  var TakeProfitList = new List<CompletedTrade>();
            var TO = Trade.TradesOnly(_FullTradeList);
            var CompletedList = CompletedTrade.CreateList(TO);
            double newProf = 0;



            ////reset 2 vol running prof
            //var doublevoladjusted = FullTradeList.Where(z => z.TradeVolume == 2);
            //foreach (var d in doublevoladjusted)
            //{
            //    d.RunningProfit = (d.RunningProfit / 2);
            //    // Debug.WriteLine(d.TimeStamp + "   " + d.Reason + "  " + d.CurrentPrice + "  " + d.TradedPrice + "  " + d.RunningProfit + "  " + d.TradeVolume);
            //}

            ////reset running pl + traded price
            //foreach (var v in FullTradeList)
            //{
            //    v.TradedPrice = 0;
            //    //  v.TotalPL = 0;
            //}


            foreach (var t in CompletedList)
            {
                var timeframe = from x in _FullTradeList
                                where x.TimeStamp >= t.OpenTrade.TimeStamp && x.TimeStamp <= t.CloseTrade.TimeStamp
                                select x;


                var TList = timeframe.ToList();
                var trade = new CompletedTrade();
                trade.OpenTrade = TList[0];
                trade.CloseTrade = TList.Last();
                if (TList.Count < period) ;
                else
                {
                            for (int x = period; x < TList.Count; x++)
                            {

                                if (TList[x].RunningProfit - TList[x - period].RunningProfit >= tp)
                                {
                                    //mark changes
                                    var adjusted = timeframe.Where(z => z.TimeStamp > TList[x].TimeStamp);
                                    foreach (var a in adjusted) a.Notes = "reset";


                                    trade.CloseTrade = TList[x];
                                    trade.CloseTrade.RunningProfit = trade.CloseTrade.RunningProfit * TList[0].TradeVolume;

                                    if (trade.OpenTrade.Reason == Trade.Trigger.OpenShort)
                                    {
                                        trade.CloseTrade.Reason = Trade.Trigger.CloseShort;
                                        trade.CloseTrade.BuyorSell = Trade.BuySell.Buy;
                                        trade.CloseTrade.TradeVolume = trade.OpenTrade.TradeVolume;
                                        trade.CloseTrade.Position = false;

                                    }
                                    if (trade.OpenTrade.Reason == Trade.Trigger.OpenLong)
                                    {
                                        trade.CloseTrade.Reason = Trade.Trigger.CloseLong;
                                        trade.CloseTrade.BuyorSell = Trade.BuySell.Sell;
                                        trade.CloseTrade.TradeVolume = trade.OpenTrade.TradeVolume;
                                        trade.CloseTrade.Position = false;

                                    }
                                    //TakeProfitList.Add(trade);
                                    break;
                                }

                                //TakeProfitList.Add(trade);
                            }

                }

            }

            foreach (var t in _FullTradeList)
            {
                if (t.Notes == "reset")
                {
                    t.RunningProfit = 0;
                    t.Reason = Trade.Trigger.None;
                    t.Position = false;
                    t.CurrentDirection = Trade.Direction.None;
                    t.Notes = null;
                    t.BuyorSell = Trade.BuySell.None;
                    t.TradeVolume = 0;
                }
                t.TotalPL = 0;
            }


            int wintrade = 0;
            int loosetrade = 0;

            foreach (var t in _FullTradeList)
            {
                if (t.Reason == Trade.Trigger.CloseLong || t.Reason == Trade.Trigger.CloseShort)
                {
                    newProf += t.RunningProfit;
                    if (t.RunningProfit > 0) wintrade++;
                    if (t.RunningProfit < 0) loosetrade++;
                }
              //  if (t.TimeStamp.Year == 2013 && t.TimeStamp.Month == 2 && t.TimeStamp.Day > 19)
                 //   Debug.WriteLine(t.TimeStamp + " " + t.CurrentPrice + "  " + t.Reason + " " + t.RunningProfit + " " + t.Notes + "  " + t.Position + "  " + t.CurrentDirection + "  " + newProf + "  " + t.TradeVolume);

            }


            //var totalOriginal = TO.Sum(z => z.RunningProfit);
            //var improve = Math.Round(((newProf - totalOriginal) / totalOriginal * 100), 2);

            //Debug.WriteLine("TP," + tp + ",Period," + period + "," + newProf + "," + improve);

            #region Last Trade
            
            ////calculate last trade
            //if (TakeProfitList.Last().CloseTrade == TakeProfitList.Last().OpenTrade)
            //{
            //    Debug.WriteLine("Open Position Found..Calculating..");
            //    var timeframe = (from x in FullTradeList
            //                     where x.TimeStamp >= CompletedList.Last().OpenTrade.TimeStamp
            //                     select x).ToList();

            //    if (timeframe.Count > period)
            //    {
            //        var trade = TakeProfitList.Last();
            //        for (int x = period; x < timeframe.Count; x++)
            //        {
            //            if (timeframe[x].RunningProfit - timeframe[x - period].RunningProfit >= tp)
            //            {
            //                Debug.WriteLine("Take profit Live : " + timeframe[x]);
            //                trade.CloseTrade = timeframe[x];
            //                timeframe.Where(z => z.TimeStamp == timeframe[x].TimeStamp).First().RunningProfit = timeframe[x].RunningProfit;
            //                if (trade.OpenTrade.Reason == Trade.Trigger.OpenShort)
            //                {
            //                    trade.CloseTrade.Reason = Trade.Trigger.CloseShort;
            //                    trade.CloseTrade.BuyorSell = Trade.BuySell.Buy;
            //                    trade.CloseTrade.TradeVolume = trade.OpenTrade.TradeVolume;
            //                    trade.CloseTrade.Position = false;
            //                    trade.CloseTrade.CurrentDirection = Trade.Direction.None;

            //                }
            //                if (trade.OpenTrade.Reason == Trade.Trigger.OpenLong)
            //                {
            //                    trade.CloseTrade.Reason = Trade.Trigger.CloseLong;
            //                    trade.CloseTrade.BuyorSell = Trade.BuySell.Sell;
            //                    trade.CloseTrade.TradeVolume = trade.OpenTrade.TradeVolume;
            //                    trade.CloseTrade.Position = false;
            //                    trade.CloseTrade.CurrentDirection = Trade.Direction.None;
            //                }
            //                break;
            //            }
            //        }
            //    }
            //}
            #endregion

      
            //    //if (t.Reason == Trade.Trigger.CloseLong || t.Reason == Trade.Trigger.CloseShort)
            //    //{
            //    //    totlapl += (t.RunningProfit * t.TradeVolume);
            //    //    t.TotalPL = totlapl;
            //    //}


            //    if (t.Reason != Trade.Trigger.None)//(t.TimeStamp.Year == 2012 && t.TimeStamp.Month == 6 && t.TimeStamp.Day > 15)
            //        Debug.WriteLine(t.TimeStamp + " " + t.CurrentPrice + "  " + t.Reason + " " + t.RunningProfit + " " + t.Notes + "  " + t.Position + "  " + t.CurrentDirection + "  " + t.TotalPL + "  " + t.TradeVolume + " " + t.TradedPrice);


            //}
           // Debug.WriteLine("Win " + wintrade);
           // Debug.WriteLine("Loose " + loosetrade);
            var imp=Math.Round((newProf-profold)/profold *100,2);
            Debug.WriteLine("TP,"+tp+",P,"+period+","+newProf+"  "+profold +"  " +imp);
            var q = new TProf
            {
                Improve=(decimal)imp,
                NewTotalPL=(int)newProf,
                PrevTotalPL=(int)profold,
                Period=period,
                TakeProf=tp,
                Note="2008",
            };
            
            var dc = new SimDBDataContext();
            dc.TProfs.InsertOnSubmit(q);
            dc.SubmitChanges();
        }

    }
}
