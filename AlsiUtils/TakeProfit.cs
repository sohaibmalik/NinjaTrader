using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace AlsiUtils
{

    public class TakeProfit
    {
        private List<TakeProfit.TakeProfitTrade> _FullTradeList = new List<TakeProfit.TakeProfitTrade>();
        private List<CompletedTrade> _CompletedTrades = new List<CompletedTrade>();

        public TakeProfit(List<Trade> FullTradeList, List<CompletedTrade> CompletedTrades)
        {
            _CompletedTrades = CompletedTrades;
            foreach (var a in FullTradeList)
            {

                var b = new TakeProfitTrade((Trade)a.Clone());
                b.StopLoss_UpperLevel_1 = 150;
                b.StopLoss_UpperLevel_2 = 300;
                b.StopLoss_LoweLevel_1 = -150;
                _FullTradeList.Add(b);
            }

        }

        public void Calculate()
        {
            SetCenterLine();
            foreach (var d in _FullTradeList.Skip(1000).Take(1000))
            {
                var data = d.TimeStamp + "," + d.RunningProfit + "," + d.StopLoss_RunningTotalProfit + "," + d.StopLoss_CenterLine + "," + d.StopLoss_TakeProfitLevel_1
                   + "," + d.StopLoss_TakeProfitLevel_2 + "," + d.StopLoss_StopLossLevel_1;
                Debug.WriteLine(data);

            }
        }

        private void SetCenterLine()
        {
            int C = _CompletedTrades.Count;
            for (int i = 1; i < C; i++)
            {
                var pl = from x in _FullTradeList
                         where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp
                         select x;

                var z = pl.ToList();
                var r = z.First().TotalPL;
                var count = pl.Count() - 1;
                for (int q = 0; q < count; q++)
                {
                    z[q].StopLoss_CenterLine = r;
                    z[q].StopLoss_StopLossLevel_1 = r + z[q].StopLoss_LoweLevel_1;
                    z[q].StopLoss_TakeProfitLevel_1 = r + z[q].StopLoss_UpperLevel_1;
                    z[q].StopLoss_TakeProfitLevel_2 = r + z[q].StopLoss_UpperLevel_2;
                    z[q].StopLoss_RunningTotalProfit = r + z[q].RunningProfit;
                }

            }


        }

        public class TakeProfitTrade : Trade
        {

            public double StopLoss_UpperLevel_1;
            public double StopLoss_UpperLevel_2;
            public double StopLoss_LoweLevel_1;

            public double StopLoss_RunningTotalProfit { get; set; }
            public double StopLoss_TakeProfitLevel_1 { get; set; }
            public double StopLoss_TakeProfitLevel_2 { get; set; }
            public double StopLoss_StopLossLevel_1 { get; set; }
            public double StopLoss_CenterLine { get; set; }

            public bool TakeProfit_Triggered_1 { get; set; }
            public bool TakeProfit_Triggered_2 { get; set; }
            public bool StopLoss_Triggered_1 { get; set; }


         

            public TakeProfitTrade(Trade trade)
            {
                this.BuyorSell = trade.BuyorSell;
                this.CurrentDirection = trade.CurrentDirection;
                this.CurrentPrice = trade.CurrentPrice;
                this.Extention = trade.Extention;
                this.IndicatorNotes = trade.IndicatorNotes;
                this.InstrumentName = trade.InstrumentName;
                this.Notes = trade.Notes;
                this.OHLC = trade.OHLC;
                this.Position = trade.Position;
                this.Reason = trade.Reason;
                this.RunningProfit = trade.RunningProfit;
                this.TimeStamp = trade.TimeStamp;
                this.TotalPL = trade.TotalPL;
                this.TradedPrice = trade.TradedPrice;
                this.TradeVolume = trade.TradeVolume;                

            }

         

        }

    }
}
