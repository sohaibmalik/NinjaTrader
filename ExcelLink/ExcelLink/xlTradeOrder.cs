using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelLink
{
   public class xlTradeOrder
    {
      public xlTradeOrder()
      {
          Status = orderStatus.Ready;
          Principle = "ATO071";
          Dealer = "DJF";
          Member = "RMDM";
          Type = "NOR";
          Exchange = "SXFIN";
      }
       public string Contract { get; set; }
       public BuySell BS { get; set; }
       public long Price { get; set; }
       public int Volume { get; set; }
       public orderStatus Status { get; set; }
       public string Principle { get; set;}
       public string Member { get; set; }
       public string Type { get; set; }
       public string Exchange { get; set; }
       public string Dealer { get; set; }

      public enum BuySell
       {
          Buy=1,
           Sell=2
       }

       public enum orderStatus
       {
           Ready=1,
           Completed=2,
           Cancelled=3,
           Active=4,
           None=5,
       }
    }
}
