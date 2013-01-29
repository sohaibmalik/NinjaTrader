using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiTrade_Backend.HiSat
{
    public static class LivePrice
    {
        public static double Bid { get; set; }
        public static double Offer { get; set; }
        public static double Last { get; set; }
        public static bool EndOfDay { get; set; }
    }
}
