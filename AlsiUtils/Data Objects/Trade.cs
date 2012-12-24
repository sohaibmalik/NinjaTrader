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

        public Trigger Reason { get; set; }

        public int TradeVolume { get; set; }

        public BuySell BuyorSell { get; set; }

        public string Notes { get; set; }

        public bool Position { get; set; }

        public double RunningProfit { get; set; }

        public Direction CurrentDirection { get; set; }

        public double CurrentPrice { get; set; }

        public double TotalPL { get; set; }

        public string IndicatorNotes { get; set; }

        public Color ForeColor { get; set; }

        public Color BackColor { get; set; }

        public string InstrumentName { get; set; }


        public enum BuySell
        {          
            Buy,
            Sell,
            None,
        }

        public enum Direction
        {
            Long = 1,
            Short = 2,
            None = 3,
        }

        public enum Trigger
        {
            OpenLong = 1,
            CloseLong = 2,
            OpenShort = 3,
            CloseShort = 4,
            None = 5,
            StopLoss = 6,
            TakeProfit = 7,
            EndOfDayCloseLong = 8,
            EndOfDayCloseShort = 9,
            ContractExpires = 10
        }

        public enum TradeReason
        {
            Normal = 1,
            StopLoss = 6,
            TakeProfit = 7,
            EndOfDayCloseLong = 8,
            EndOfDayCloseShort = 9,
            ContractExpires = 10
        }
       
        public static string BuySellToString(BuySell BS)
        {
            if (BS == BuySell.Buy) return "Buy";
            if (BS == BuySell.Sell) return "Sell";
            if (BS == BuySell.None) return "None";
            return "";
        }

        public static BuySell BuySellFromString(string BS)
        {
            if (BS == "Buy") return BuySell.Buy;
            if (BS == "Sell") return BuySell.Sell;
            if (BS == "None") return BuySell.None;
            return BuySell.None;
        }


    }
}
