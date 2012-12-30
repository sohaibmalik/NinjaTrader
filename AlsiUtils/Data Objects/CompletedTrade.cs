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





    }
}
