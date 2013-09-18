using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System;
using System.Text;

namespace AlsiUtils
{
    public class ProfitAlgoLayer
    {
        private List<ProfitAlgoLayer.TakeProfitTrade> _FullTradeList = new List<ProfitAlgoLayer.TakeProfitTrade>();
        private List<CompletedTrade> _CompletedTrades = new List<CompletedTrade>();
        private double NewTotalRunningPL;

        public ProfitAlgoLayer(List<Trade> FullTradeList, List<CompletedTrade> CompletedTrades)
        {
            _CompletedTrades = CompletedTrades;
            AdjustVolume(FullTradeList);

            foreach (var a in FullTradeList)
            {
                var b = new TakeProfitTrade((Trade)a.Clone());
                _FullTradeList.Add(b);
            }

        }

        public List<ProfitAlgoLayer.TakeProfitTrade> GetFullTradeList()
        {
            SetOHLC_IntraTrade_SingleTradePL();
            return _FullTradeList;
        }

        #region Prepare OHLC and BoilBands

        private List<Price> OHLC_LIST = new List<Price>();
        private List<BollingerBand> BOIL = new List<BollingerBand>();

        public void Calculate()
        {
            //int C = _CompletedTrades.Count;
            //for (int i = 1; i < C; i++)
            //{
            //    var pl = from x in _FullTradeList
            //             where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp
            //             select x;

            //   // var tpl = pl.ToList();
            //    //if (tpl.Count > 0)
            //    //{
            //    //    //SetOpenCloseRawTriggers(tpl);
            //    //    //SetOpenCloseTriggers(tpl);
            //    //    //CalcProfLoss(tpl);
            //    //}
            //}

            StreamWriter sw = new StreamWriter(@"D:\OHLC.csv");
            //SetOHLC_TradeOnly();
            SetOHLC_IntraTrade_RunningPL();

            // BOIL = AlsiUtils.Factory_Indicator.createBollingerBand(21, 1.5, OHLC_LIST, TicTacTec.TA.Library.Core.MAType.Kama);
            var EM = AlsiUtils.Factory_Indicator.createAdaptiveMA_MAMA(0.01, 0.01, OHLC_LIST);
            var adx_DMI = AlsiUtils.Factory_Indicator.createADX(75, OHLC_LIST);
            var RSI = AlsiUtils.Factory_Indicator.createRSI(20, OHLC_LIST);

            var ohlcWriter = new StreamWriter(@"D:\RawAlgoOHLCV.csv");
            foreach (var r in OHLC_LIST)
            {
                var trade = _FullTradeList.Where(z => z.TimeStamp == r.TimeStamp).First();
                var v = orgVol.Where(x => x.Stamp == r.TimeStamp);
                var vol = (v.Count() == 0) ? 0 : v.First().Vol;
                ohlcWriter.WriteLine(r.TimeStamp.Date.ToShortDateString() +
                    "," + r.TimeStamp.ToShortTimeString() +
                    "," + r.Open +
                    "," + r.High +
                    "," + r.Low +
                    "," + r.Close +
                    "," + vol +
                    ","+trade.Reason );
            }

            ohlcWriter.Close();


            foreach (var r in adx_DMI)
            {
                var em = EM.Where(z => z.TimeStamp == r.TimeStamp);
                var rs = RSI.Where(z => z.TimeStamp == r.TimeStamp);
                var vol = orgVol.Where(z => z.Stamp == r.TimeStamp);

                var volume = (vol.Count() == 0) ? 1 : vol.First().Vol;
                var topband = r.DI_Up;
                var midband = (em.Count() == 0) ? 0 : em.First().Mama;
                var lowband = r.DI_Down;

                var b1 = (rs.Count() == 0) ? 0 : rs.First().RSI;

                var b2 = 0;
                var b3 = 0;
                var b4 = 0;
                var trade = _FullTradeList.Where(z => z.TimeStamp == r.TimeStamp).First();

                sw.WriteLine(trade.Reason + "," + r.TimeStamp + "," + r.Price_Close + "," + midband + "," + topband + "," + lowband + "," + volume + "," + b1 + "," + b2 + "," + b3 + "," + b4);
            }
            sw.Close();



            //var D = new DateTime(2000, 01, 01, 12, 00, 00);
            //foreach (var s in OHLC_LIST)
            //{
            //    D = s.TimeStamp;
            //    var data = new StringBuilder(String.Format("{0:yyyy.MM.dd}", D));
            //    data.Append("," + String.Format("{0:HH:mm}", D));
            //    data.Append("," + s.Open);
            //    data.Append("," + s.High);
            //    data.Append("," + s.Low);
            //    data.Append("," + s.Close);
            //    data.Append("," + (D.Minute + 1));

            //    sw.WriteLine(data);
            //}
            // sw.Close();
            //	CalcBoilTriggers();
        }
        private void SetOHLC_TradeOnly()
        {
            var tpl = new List<ProfitAlgoLayer.TakeProfitTrade>();
            //	StreamWriter sr = new StreamWriter(@"d:\ohlcPL.txt");
            int C = _CompletedTrades.Count;
            double totProfit = 0;
            for (int i = 1; i < C; i++)
            {
                var pl = from x in _FullTradeList
                         where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp
                         select x;

                tpl = pl.ToList();
                if (tpl.Count > 1)
                {
                    //VERKEERD SIEN SETOHLC_INTRATRADE
                    //var start = tpl.First().CurrentPrice;
                    //var o = totProfit;
                    //var h = (tpl.Max(z => z.CurrentPrice) - start) + totProfit;
                    //var l = (tpl.Min(z => z.CurrentPrice) - start) + totProfit;
                    //var c = tpl.Last().RunningProfit;

                    //var P = new Price();
                    //P.TimeStamp = tpl.Last().TimeStamp;
                    //P.Open = o;
                    //P.High = h;
                    //P.Low = l;
                    //totProfit += tpl.Last().RunningProfit;
                    //P.Close = totProfit;
                    //OHLC_LIST.Add(P);
                    //Debug.WriteLine(P.Close);
                    //	Debug.WriteLine(i + "," + (o + tpl[0].RunningTotalProfit_New) + "," + (h + tpl[0].RunningTotalProfit_New) + "," + (l + tpl[0].RunningTotalProfit_New) + "," + (c + tpl[0].RunningTotalProfit_New));
                    //	Debug.WriteLine(i + "," + P.Open + "," + "," + P.High + "," + P.Low + "," + P.Close);
                }
            }
            //sr.Close();
        }

        private void SetOHLC_IntraTrade_RunningPL()
        {
            var tpl = new List<ProfitAlgoLayer.TakeProfitTrade>();
          //     StreamWriter sr = new StreamWriter(@"d:\ohlcPL.txt");
            int C = _CompletedTrades.Count;
            double totProfit = 0;
            double start = 0;
            var dc = new AlsiDBDataContext();
            var M = dc.OHLC_5_Minutes.ToList();
            for (int i = 1; i < C; i++)
            {
                var pl5 = from x in _FullTradeList
                          where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp
                          select x;



                tpl = pl5.ToList();

                if (tpl.Count > 1)
                {


                    foreach (var v in tpl)
                    {
                        var f = M.Where(z => z.Stamp == v.TimeStamp).First();
                        var market = new Price(f.Stamp, f.O, f.H, f.L, f.C, f.Instrument);


                        start = tpl.First().TradedPrice;

                        double o = 0;
                        if (tpl[0].Reason == Trade.Trigger.OpenShort) o = (start - market.Open) + totProfit;
                        if (tpl[0].Reason == Trade.Trigger.OpenLong) o = (market.Open - start) + totProfit;

                        double h = 0;
                        if (tpl[0].Reason == Trade.Trigger.OpenShort) h = (start - market.High) + totProfit;
                        if (tpl[0].Reason == Trade.Trigger.OpenLong) h = (market.High - start) + totProfit;

                        double l = 0;
                        if (tpl[0].Reason == Trade.Trigger.OpenShort) l = (start - market.Low) + totProfit;
                        if (tpl[0].Reason == Trade.Trigger.OpenLong) l = (market.Low - start) + totProfit;

                        double c = 0;
                        if (tpl[0].Reason == Trade.Trigger.OpenShort) c = (start - market.Close) + totProfit;
                        if (tpl[0].Reason == Trade.Trigger.OpenLong) c = (market.Close - start) + totProfit;



                        var P = new Price();
                        P.TimeStamp = market.TimeStamp;
                        P.Open = o;
                        P.High = h;
                        P.Low = l;
                        P.Close = c;
                        if (v.TimeStamp == tpl.Last().TimeStamp)
                        {
                            totProfit += tpl.Last().RunningProfit;
                        }

                        OHLC_LIST.Add(P);

                        //sr.WriteLine(
                        //    P.Open
                        //    + "," + P.High
                        //        + "," + P.Low
                        //         + "," + P.Close
                        //          + "," + 0
                        //          + "," + market.Open
                        //           + "," + market.High
                        //            + "," + market.Low
                        //             + "," + market.Close
                        //             + "," + totProfit
                                     
                        //             );

                    }
                    //Debug.WriteLine(P.Close);
                    //	Debug.WriteLine(i + "," + (o + tpl[0].RunningTotalProfit_New) + "," + (h + tpl[0].RunningTotalProfit_New) + "," + (l + tpl[0].RunningTotalProfit_New) + "," + (c + tpl[0].RunningTotalProfit_New));
                    //	Debug.WriteLine(i + "," + P.Open + "," + "," + P.High + "," + P.Low + "," + P.Close);
                }
            }
            // sr.Close();
        }

        private void SetOHLC_IntraTrade_SingleTradePL()
        {
            var tpl = new List<ProfitAlgoLayer.TakeProfitTrade>();
                 StreamWriter sr = new StreamWriter(@"d:\ohlcPL2.csv");
            int C = _CompletedTrades.Count;
            double totProfit = 0;
            double start = 0;
            var dc = new AlsiDBDataContext();
            var M = dc.OHLC_5_Minutes.ToList();
            for (int i = 1; i < C; i++)
            {
                var pl5 = from x in _FullTradeList
                          where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp
                          select x;



                tpl = pl5.ToList();

                if (tpl.Count > 1)
                {


                    foreach (var v in tpl)
                    {
                        var f = M.Where(z => z.Stamp == v.TimeStamp).First();
                        var market = new Price(f.Stamp, f.O, f.H, f.L, f.C, f.Instrument);


                        start = tpl.First().TradedPrice;

                        double o = 0;
                        if (tpl[0].Reason == Trade.Trigger.OpenShort) o = (start - market.Open) ;
                        if (tpl[0].Reason == Trade.Trigger.OpenLong) o = (market.Open - start) ;

                        double h = 0;
                        if (tpl[0].Reason == Trade.Trigger.OpenShort) h = (start - market.High) ;
                        if (tpl[0].Reason == Trade.Trigger.OpenLong) h = (market.High - start) ;

                        double l = 0;
                        if (tpl[0].Reason == Trade.Trigger.OpenShort) l = (start - market.Low) ;
                        if (tpl[0].Reason == Trade.Trigger.OpenLong) l = (market.Low - start) ;

                        double c = 0;
                        if (tpl[0].Reason == Trade.Trigger.OpenShort) c = (start - market.Close) ;
                        if (tpl[0].Reason == Trade.Trigger.OpenLong) c = (market.Close - start) ;



                        var P = new Price();
                        P.TimeStamp = market.TimeStamp;
                        P.Open = o;
                        P.High = h;
                        P.Low = l;
                        P.Close = c;
                        if (v.TimeStamp == tpl.Last().TimeStamp)
                        {
                            totProfit += tpl.Last().RunningProfit;
                        }

                        OHLC_LIST.Add(P);

                        sr.WriteLine(P.TimeStamp.Date 
                            +","+P.TimeStamp
                         +","+v.Reason
                            + "," + P.Open                           
                            + "," + P.High
                                + "," + P.Low
                                 + "," + P.Close
                                  + "," + 0
                                  + "," + market.Open
                                   + "," + market.High
                                    + "," + market.Low
                                     + "," + market.Close
                                     + "," + totProfit

                                     );

                    }
                    //Debug.WriteLine(P.Close);
                    //	Debug.WriteLine(i + "," + (o + tpl[0].RunningTotalProfit_New) + "," + (h + tpl[0].RunningTotalProfit_New) + "," + (l + tpl[0].RunningTotalProfit_New) + "," + (c + tpl[0].RunningTotalProfit_New));
                    //	Debug.WriteLine(i + "," + P.Open + "," + "," + P.High + "," + P.Low + "," + P.Close);
                }
            }
             sr.Close();
        }

        private void AdjustVolume(List<Trade> trade)
        {
            int count = trade.Count;
            for (int x = 1; x < count; x++)
            {   //set traded price
                if (trade[x].Reason == Trade.Trigger.OpenLong || trade[x].Reason == Trade.Trigger.OpenShort)
                    trade[x].TradedPrice = trade[x].CurrentPrice;
                else
                    trade[x].TradedPrice = trade[x - 1].TradedPrice;

                var v = new OriginalVolume()
                {
                    Stamp = trade[x].TimeStamp,
                    Vol = trade[x].TradeVolume
                };
                orgVol.Add(v);
            }


            //clear vol to 1
            double tp = 0;
            for (int x = 1; x < count; x++)
            {

                trade[x].TradeVolume = 1;
                trade[x].TotalPL = 0;
                trade[x].RunningProfit = 0;

                if (trade[x].Position && trade[x].CurrentDirection == Trade.Direction.Long)
                    trade[x].RunningProfit = trade[x].CurrentPrice - trade[x].TradedPrice;

                if (trade[x].Position & trade[x].CurrentDirection == Trade.Direction.Short)
                    trade[x].RunningProfit = trade[x].TradedPrice - trade[x].CurrentPrice;


                if (trade[x - 1].Position && !trade[x].Position)
                {
                    if (trade[x - 1].CurrentDirection == Trade.Direction.Long)
                        trade[x].RunningProfit = trade[x].CurrentPrice - trade[x - 1].TradedPrice;

                    if (trade[x - 1].CurrentDirection == Trade.Direction.Short)
                        trade[x].RunningProfit = trade[x - 1].TradedPrice - trade[x].CurrentPrice;
                }

                if (trade[x].Reason == Trade.Trigger.CloseLong || trade[x].Reason == Trade.Trigger.CloseShort)
                {
                    tp += trade[x].RunningProfit;
                    trade[x].TotalPL = tp;
                    trade[x].TotalRunningProfit = tp;
                }
                else
                    trade[x].TotalRunningProfit = tp + trade[x].RunningProfit;

            }


        }

        private List<OriginalVolume> orgVol = new List<OriginalVolume>();
        internal class OriginalVolume
        {
            public int Vol { get; set; }
            public DateTime Stamp { get; set; }
        }

        private void CalcProfLoss(List<TakeProfitTrade> tpt)
        {
            int i = tpt.Count;
            double pl = 0;

            tpt[0].RunningTotalProfit_New = NewTotalRunningPL;
            for (int x = 1; x < i; x++)
            {
                double tickpl = tpt[x].RunningProfit - tpt[x - 1].RunningProfit;

                if (tpt[x - 1].InPosition)
                {
                    pl += tickpl;
                    tpt[x].RunningProfit_New = pl;
                }
                tpt[x].RunningTotalProfit_New = NewTotalRunningPL + pl;
            }
            NewTotalRunningPL += pl;

        }
        public class TakeProfitTrade : Trade
        {
            public bool InPosition { get; set; }
            public TradeActions TradeAction_Raw { get; set; }
            public TradeActions TradeAction { get; set; }
            public double RunningProfit_New { get; set; }
            public double RunningTotalProfit_New { get; set; }
            public bool Trigger_Close { get; set; }
            public bool Trigger_Open { get; set; }
            public bool ReverseTrade { get; set; }
            public BollingerBand BB { get; set; }
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
                this.TotalRunningProfit = trade.TotalRunningProfit;
                this.TradedPrice = trade.TradedPrice;
                this.TradeVolume = trade.TradeVolume;

            }
            public enum TradeActions
            {
                None,
                CloseTrade,
                OpenTrade,
            }

        }

        #endregion

        #region Boil Triggers

        private void CalcBoilTriggers()
        {
            for (int x = 1; x < BOIL.Count; x++)
            {
                var trade = _FullTradeList.Where(z => z.TimeStamp == BOIL[x].TimeStamp).First();
                if (BOIL[x].Price_Close > BOIL[x].Upper) trade.ReverseTrade = true;
                trade.BB = BOIL[x];
            }
            foreach (var d in _FullTradeList.Where(x => x.BB == null)) d.BB = new BollingerBand();
            foreach (var d in _FullTradeList)
            {
                //	d.RunningProfit = 0;
            }
            var C = _FullTradeList.Where(x => x.Reason != Trade.Trigger.None).ToList();
            var c = C.Count();
            for (int x = 1; x < c; x++)
            {
                if (C[x - 1].ReverseTrade == true)
                {
                    C[x].TradeVolume = -2;
                }
                if (x > 1 && C[x - 2].ReverseTrade) C[x].TradeVolume = -2;

            }

            foreach (var r in _FullTradeList.Where(z => z.Reason != Trade.Trigger.None))
            {
                //Debug.WriteLine(r.TimeStamp + " " + r.CurrentPrice + "  " + r.BB.Price_Close + "  " + r.ReverseTrade  + "  " + r.Reason + "   " +r.TradeVolume 
                //+"  "+r.RunningProfit + "   " 	);

                //Debug.WriteLine(r.RunningProfit);
            }

        }

        #endregion
    }
}
