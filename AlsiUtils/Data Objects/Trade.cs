using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AlsiUtils
{
    public partial class Trade : ICloneable
    {
        public double TradedPrice { get; set; }
        public DateTime TimeStamp { get; set; }
        public Trigger TradeTrigger { get; set; }
        public Trigger Reason { get; set; }
        public Trade.Trigger TradeTriggerGeneral { get; set; }
        public int TradeVolume { get; set; }
        public BuySell BuyorSell { get; set; }
        public string Notes { get; set; }
        public bool Position { get; set; }
        public double RunningProfit { get; set; }
        public double TotalRunningProfit { get; set; }
        public Direction CurrentDirection { get; set; }
        public double CurrentPrice { get; set; }
        public double TotalPL { get; set; }
        public string IndicatorNotes { get; set; }
        public Color ForeColor { get; set; }
        public Color BackColor { get; set; }
        public string InstrumentName { get; set; }
        public int TradeCount { get; set; }
        public AlsiUtils.Data_Objects.RegressionExt Extention { get; set; }
        public Price OHLC { get; set; }

        //ExcelOrders
        public string xlRef { get; set; }
        public bool xlMatched { get; set; }


        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public Trade()
        {
            Extention = new Data_Objects.RegressionExt();
            OHLC = new Price();

        }
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
            None,
            Open,
            Close,
            OpenLong,
            CloseLong,
            OpenShort,
            CloseShort,
            StopLoss,
            StopLossLong,
            StopLossShort,
            TakeProfit,
            TakeProfitLong,
            TakeProfitShort,
            EndOfDayClose,
            EndOfDayCloseLong,
            EndOfDayCloseShort,
            ContractExpires,
            Reverse,
            ReverseLong,
            ReverseShort,
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

        /// <summary>
        /// Ovverride For 5 Min Interval
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            string r = Reason.ToString();
            if (this.Reason == 0) r = "";
            string msg = "Trade Details:  " + r + " \n" + TimeStamp.AddMinutes(4) + "  " + BuyorSell + "  " + TradeVolume + " @" + TradedPrice + "\n";
            return msg;
        }

        public static List<Trade> TradesOnly(List<Trade> AllTrades)
        {
            var tradesonly = new List<Trade>();

            foreach (var z in AllTrades)
            {
                if (z.Reason == Trigger.CloseLong || z.Reason == Trigger.CloseShort || z.Reason == Trigger.OpenLong || z.Reason == Trigger.OpenShort)
                    tradesonly.Add(z);
            }

            return tradesonly;
        }




    }
}
