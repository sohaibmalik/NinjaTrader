using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlsiUtils;
namespace AlsiUtils.Data_Objects
{
   public static class GlobalObjects
    {
       public static List<PointData> Prices = new List<PointData>();
       public static string CustomConnectionString;
       public static Trade LastTrade; //Last Trade form long list Carry over to temp list when trading
       public static bool LastTradeOfTheDay;
    }
}
