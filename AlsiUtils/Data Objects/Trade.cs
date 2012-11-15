using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AlsiUtils
{
	public class Trade
	{
	    public double TradedPrice { get; set; }

	    public DateTime TimeStamp { get; set; }

	    public string TradeReason { get; set; }

	    public int TradeVolume { get; set; }

	    public string BuyorSell { get; set; }

	    public string Notes { get; set; }

	    public bool Position { get; set; }

	    public double RunningProfit { get; set; }

	    public string CurrentDirection { get; set; }

	    public double CurrentPrice { get; set; }

	    public double TotalPL { get; set; }

	    public string IndicatorNotes { get; set; }

	    public Color ForeColor { get; set; }

	    public Color BackColor { get; set; }

	    public string InstrumentName { get; set; }
	}
}
