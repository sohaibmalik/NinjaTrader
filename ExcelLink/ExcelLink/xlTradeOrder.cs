using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlsiUtils;
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
       public  Trade.BuySell BS { get; set; }
       public double Price { get; set; }
       public int Volume { get; set; }
       public orderStatus Status { get; set; }
       public string Principle { get; set;}
       public string Member { get; set; }
       public string Type { get; set; }
       public string Exchange { get; set; }
       public string Dealer { get; set; }
       public string Reference { get; set; }

       public void GetReference()
       {
           Reference=BS.ToString()+Price.ToString()+DateTime.Now.Day.ToString();
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
