using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils
{
	public  class SummaryStats
	{
	    public string Detail { get; set; }
	    public string Value { get; set; }
	    public Period Period { get; set; }
	    public int Year { get; set; }
	    public int Month { get; set; }
	    public int Week { get; set; }
	    public double Average { get; set; }
	    public int Sum { get; set; }
	    public int Count { get; set; }

        public double TotalProfit { get; set; }
        public decimal Total_Avg_PL { get; set; }
        public decimal Avg_Prof { get; set; }
        public decimal Avg_Loss { get; set; }
        public decimal Pct_Prof { get; set; }
        public decimal Pct_Loss { get; set; }
        public double TradeCount { get; set; }
        public decimal PL_Ratio { get; set; }
	}
}
