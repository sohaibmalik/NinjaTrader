using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils.Strategies
{
    public class SumStats
    {
        public double TotalProfit { get; set; }
        public double Total_Avg_PL { get; set; }
        public double Avg_Prof { get; set; }
        public double Avg_Loss { get; set; }
        public double Pct_Prof { get; set; }
        public double Pct_Loss { get; set; }
        public double TradeCount { get; set; }
        public double PL_Ratio { get; set; }
    }
}
