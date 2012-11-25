using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils.Strategies
{
    public class SumStats
    {
        public double TotalProfit { get; set; }
        public decimal Total_Avg_PL { get; set; }
        public decimal Avg_Prof { get; set; }
        public decimal  Avg_Loss { get; set; }
        public decimal Pct_Prof { get; set; }
        public decimal Pct_Loss { get; set; }
        public double TradeCount { get; set; }
        public decimal PL_Ratio { get; set; }
    }
}
