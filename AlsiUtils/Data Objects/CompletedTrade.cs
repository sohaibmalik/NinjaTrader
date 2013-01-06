using System.Collections.Generic;
using System.Linq;

namespace AlsiUtils
{
    public class CompletedTrade
    {
        public Trade OpenTrade { get; set; }
        public Trade CloseTrade { get; set; }

        public CompletedTrade()
        {
            OpenTrade = new Trade();
            CloseTrade = new Trade();
        }

        /// <summary>
        /// Convert List of Trades to List of Completed Trades
        /// </summary>
        /// <param name="TradesOnly"></param>
        /// <returns></returns>
        public static List<CompletedTrade> CreateList(List<Trade> TradesOnly)
        {
            var CompList = new List<CompletedTrade>();
            var even = TradesOnly.Count % 2;
            int back = even == 2 ? 0 : 1;

            for (int x = 0; x < TradesOnly.Count - back; x += 2)
            {
                CompletedTrade t = new CompletedTrade();
                t.OpenTrade = TradesOnly[x];
                t.CloseTrade = TradesOnly[x + 1];
                CompList.Add(t);
            }

            if (TradesOnly.Last() != CompList.Last().CloseTrade)
            {
                CompletedTrade last = new CompletedTrade();

                last.OpenTrade = TradesOnly.Last();
                last.CloseTrade.TimeStamp = last.OpenTrade.TimeStamp;
                last.CloseTrade.BuyorSell = Trade.BuySell.None;
                last.CloseTrade.Reason = Trade.Trigger.None;
                last.CloseTrade.CurrentDirection = last.OpenTrade.CurrentDirection;
                last.CloseTrade.TradedPrice = last.OpenTrade.TradedPrice;
                CompList.Add(last);
            }



            return CompList;
        }

        /// <summary>
        /// Convert Completed Trade List back to List of Trades
        /// </summary>
        /// <param name="CompletedTrades"></param>
        /// <returns></returns>
        public static List<Trade> CreateList(List<CompletedTrade> CompletedTrades)
        {
            List<Trade> tradeList = new List<Trade>(); 
            foreach (var t in CompletedTrades)
            {
                t.OpenTrade.TradeVolume = t.OpenTrade.Extention.OrderVol;
                t.CloseTrade.TradeVolume = t.CloseTrade.Extention.OrderVol;

                t.OpenTrade.RunningProfit = t.OpenTrade.Extention.newRunningProf;
                t.OpenTrade.TotalPL = t.OpenTrade.Extention.newTotalProfit;

                t.CloseTrade.RunningProfit = t.CloseTrade.Extention.newRunningProf;
                t.CloseTrade.TotalPL = t.CloseTrade.Extention.newTotalProfit;

                tradeList.Add(t.OpenTrade);
                tradeList.Add(t.CloseTrade);
            }

            return tradeList;
        }



    }
}
