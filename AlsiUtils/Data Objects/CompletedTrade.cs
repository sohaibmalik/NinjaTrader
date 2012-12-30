using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

       public static List<CompletedTrade> CreateList(List<Trade>TradesOnly)
       {
          var CompList = new List<CompletedTrade>();
          var even = TradesOnly.Count % 2;
          int back = even == 2 ? 0 : 1;
          
              for (int x = 0; x < TradesOnly.Count-back; x +=2)
              {
                  CompletedTrade t = new CompletedTrade();
                  t.OpenTrade = TradesOnly[x];
                  t.CloseTrade = TradesOnly[x + 1];
                  CompList.Add(t);
              }

              if (TradesOnly.Last() != CompList.Last().CloseTrade)
              {
                 CompletedTrade t = new CompletedTrade();
                 t.OpenTrade = TradesOnly.Last();

                 CompletedTrade c = new CompletedTrade();
                 t.CloseTrade = t.OpenTrade;
                 t.CloseTrade.BuyorSell = Trade.BuySell.None;
                 t.CloseTrade.Reason = Trade.Trigger.None;
                 t.CloseTrade.CurrentDirection = t.OpenTrade.CurrentDirection;
                 t.CloseTrade.TradedPrice = 0;
                 CompList.Add(t);
              }



            return CompList;
          }
         
        
         
       

    }
}
