using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlsiUtils;
namespace AlsiUtils.Data_Objects
{
   public static class GlobalObjects
    {
       public static List<Price> Points = new List<Price>();
       public static List<Price> Prices = new List<Price>();
       public static string CustomConnectionString;
       public static Trade LastTrade; //Last Trade form long list Carry over to temp list when trading
       public static bool LastTradeOfTheDay;

       public enum TimeInterval
       {
           Minute_1=1,
           Minute_2=2,
           Minute_5=5,
           Minute_10=10,

       }
    }
}
