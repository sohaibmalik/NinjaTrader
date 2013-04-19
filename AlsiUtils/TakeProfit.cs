using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace AlsiUtils
{




    public class TakeProfit
    {
        private List<TakeProfit.TakeProfitTrade> _FullTradeList = new List<TakeProfit.TakeProfitTrade>();
        private List<CompletedTrade> _CompletedTrades = new List<CompletedTrade>();
        private double NewTotalRunningPL;
        private List<MAMA> mama;
        private List<VariableIndicator> VarList = new List<VariableIndicator>();
        private bool CurrentAllowTrade;
        private double _m, _f;
        public TakeProfit(List<Trade> FullTradeList, List<CompletedTrade> CompletedTrades, double mama, double fama)
        {
            _CompletedTrades = CompletedTrades;
            _m = 0.01;
            _f = 0.01;

            foreach (var a in FullTradeList)
            {
                var b = new TakeProfitTrade((Trade)a.Clone());

                _FullTradeList.Add(b);
            }

        }

        public void Calculate()
        {
            CalculateA();
            CalculateB();

            //  var pl=_FullTradeList.Where(z=>z.StopLoss_CenterLine!=0).Last().NewTotalRunningProf;
            // if(pl>18000) Debug.WriteLine("mama " + _m + "  fama " + _f + "    " + pl );
            Print();
        }

        private void CalculateA()
        {
            NewTotalRunningPL = 0;
            int C = _CompletedTrades.Count;
            for (int i = 1; i < C; i++)
            {
                var pl = from x in _FullTradeList
                         where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp
                         select x;

                var tpl = pl.ToList();
                AdjustVolume(tpl);
                SetCenterLine(tpl);
            }
            SetMAMA();
        }

        private void CalculateB()
        {

            int C = _CompletedTrades.Count;
            for (int i = 1; i < C; i++)
            {
                var pl = from x in _FullTradeList
                         where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp && x.Mama != null
                         select x;

                var tpl = pl.ToList();
             //   SetCloseTriggers(tpl);
              //  SetOpenTriggers(tpl);
              //  SetOpenCloseRawTriggers(tpl);
              //  SetOpenCloseTriggers(tpl);
              //  CalcProfLoss(tpl);

            }
        }



        private void Print()
        {


            var F = _FullTradeList.Where(z => z.StopLoss_CenterLine != 0);

            var varlist2 = new List<VariableIndicator>();
            foreach (var x in F.Where(z => z.Mama != null))
            {
                var v = new VariableIndicator()
                {
                    TimeStamp = x.TimeStamp,
                    Value = x.Mama.Mama,
                };
                varlist2.Add(v);
            }

            var reg = Factory_Indicator.createRegression(18, varlist2);
            var varlist3 = new List<VariableIndicator>();

            foreach (var rr in reg)
            {
                var q = new VariableIndicator()
                {
                    TimeStamp=rr.TimeStamp,
                    Value=rr.Slope,
                };
                varlist3.Add(q);
            }

            var reg2 = Factory_Indicator.createRegression(9, varlist3);

            var varlist4 = new List<VariableIndicator>();

            foreach (var rr in reg2)
            {
                var qq = new VariableIndicator()
                {
                    TimeStamp = rr.TimeStamp,
                    Value = rr.Slope,
                };
                varlist4.Add(qq);
            }

            var reg3 = Factory_Indicator.createRegression(5, varlist4);

            StreamWriter sr = new StreamWriter(@"d:\tt.txt");
            foreach (var d in F.Skip(100).Take(3000))
            {
                var r = reg.Where(z => z.TimeStamp == d.TimeStamp).First();
                var r2 = reg2.Where(z => z.TimeStamp == d.TimeStamp).First();
                var r3 = reg3.Where(z => z.TimeStamp == d.TimeStamp).First();
                var data = d.TimeStamp + "," + d.Reason + "," + d.RunningProfit + "," + d.StopLoss_RunningTotalProfit
                           + "," + d.Mama.Mama + "," + d.Mama.Fama + "," + d.Trigger_Open + "," + d.Trigger_Close
                           + "," + d.TradeAction_Raw + "," + d.TradeAction + "," + d.AllowTrade + "," + (d.NewTotalRunningProf * 0) + "," + r.Regression + "," + r.Slope + "," + (r2.Slope * 10) + "," + (r3.Slope * 100) + "," + 1;

                sr.WriteLine(data);
            }
            sr.Close();
        }
        private void SetCenterLine(List<TakeProfitTrade> tpt)
        {
            var r = tpt.First().TotalPL;
            var count = tpt.Count();
            for (int q = 0; q < count; q++)
            {
                tpt[q].StopLoss_CenterLine = r;
                tpt[q].StopLoss_RunningTotalProfit = r + tpt[q].RunningProfit;

                //set Variable List
                var v = new VariableIndicator()
                {
                    TimeStamp = tpt[q].TimeStamp,
                    Value = tpt[q].StopLoss_RunningTotalProfit,
                };
                VarList.Add(v);

            }

        }

        private void SetMAMA()
        {
            //0.01,0.01;
            mama = Factory_Indicator.createAdaptiveMA_MAMA(_f, _m, VarList);

            var FTL = _FullTradeList.Where(z => z.StopLoss_CenterLine != 0).ToList();
            var Ftl_Count = FTL.Count;
            var mam_Count = mama.Count;

            //find first mama.time=TRL.time
            var Ftl_Time_Equal_Mama_Time = FTL.Where(f => f.TimeStamp == mama[0].TimeStamp).First().TimeStamp;
            int ftl_Start_Index = 0;

            foreach (var Ft in FTL)
            {
                if (Ft.TimeStamp == Ftl_Time_Equal_Mama_Time) break;
                ftl_Start_Index++;
            }

            for (int x = 0; x < Ftl_Count - ftl_Start_Index; x++) FTL[ftl_Start_Index + x].Mama = mama[x];


        }


        private void AdjustVolume(List<TakeProfitTrade> tpt)
        {
            var r = tpt.First().TotalPL;
            var count = tpt.Count();
            var vol = tpt[0].TradeVolume;
            for (int q = 0; q < count; q++)
            {
                tpt[q].TradeVolume = vol;
                tpt[q].RunningProfit = tpt[q].RunningProfit / vol;
            }
        }
        private void SetCloseTriggers(List<TakeProfitTrade> tpt)
        {
            int i = tpt.Count;

            for (int x = 1; x < i; x++)
            {
                if (tpt[x].Mama.Fama > tpt[x].Mama.Mama && tpt[x - 1].Mama.Fama < tpt[x - 1].Mama.Mama) tpt[x].Trigger_Close = true;

            }
        }
        private void SetOpenTriggers(List<TakeProfitTrade> tpt)
        {

            int i = tpt.Count;

            for (int x = 1; x < i; x++)
            {
                if (tpt[x].Mama.Fama < tpt[x].Mama.Mama && tpt[x - 1].Mama.Fama > tpt[x - 1].Mama.Mama) tpt[x].Trigger_Open = true;

            }
        }
        private void SetOpenCloseRawTriggers(List<TakeProfitTrade> tpt)
        {

            tpt[0].TradeAction_Raw = TakeProfitTrade.TradeActions.None;
            int i = tpt.Count;

            for (int x = 1; x < i; x++)
            {
                tpt[x].TradeAction_Raw = TakeProfitTrade.TradeActions.None;
                if (tpt[x].Trigger_Close) tpt[x].TradeAction_Raw = TakeProfitTrade.TradeActions.CloseTrade;
                if (tpt[x].Trigger_Open) tpt[x].TradeAction_Raw = TakeProfitTrade.TradeActions.OpenTrade;

            }
        }
        private void SetOpenCloseTriggers(List<TakeProfitTrade> tpt)
        {

            tpt[0].TradeAction = TakeProfitTrade.TradeActions.None;
            tpt[0].AllowTrade = CurrentAllowTrade;
            int i = tpt.Count - 0;

            for (int x = 1; x < i; x++)
            {
                //set start
                tpt[x].AllowTrade = tpt[x - 1].AllowTrade;
                //close trade
                if (tpt[x].AllowTrade && tpt[x].TradeAction_Raw == TakeProfitTrade.TradeActions.CloseTrade)
                {
                    tpt[x].AllowTrade = false;
                    CurrentAllowTrade = false;
                    tpt[x].TradeAction = tpt[x].TradeAction_Raw;
                }

                //open trade
                if (!tpt[x].AllowTrade && tpt[x].TradeAction_Raw == TakeProfitTrade.TradeActions.OpenTrade)
                {
                    tpt[x].AllowTrade = true;
                    CurrentAllowTrade = true;
                    tpt[x].TradeAction = tpt[x].TradeAction_Raw;
                }
            }
        }
        private void CalcProfLoss(List<TakeProfitTrade> tpt)
        {
            int i = tpt.Count;
            double pl = 0;
            var vol = 1;// tpt[0].TradeVolume;
            tpt[0].NewTotalRunningProf = NewTotalRunningPL;
            for (int x = 1; x < i; x++)
            {
                double tickpl = tpt[x].RunningProfit - tpt[x - 1].RunningProfit;
                if (tpt[x - 1].AllowTrade) pl += (tickpl * vol);
                tpt[x].NewRunningProf += (pl * vol);
                if (tpt[x - 1].AllowTrade) tpt[x].NewTotalRunningProf = NewTotalRunningPL += (tickpl * vol);
                else
                    tpt[x].NewTotalRunningProf = tpt[x - 1].NewTotalRunningProf;
            }


        }



        public class TakeProfitTrade : Trade
        {
            public bool AllowTrade { get; set; }
            public double StopLoss_CenterLine { get; set; }
            public TradeActions TradeAction_Raw { get; set; }
            public TradeActions TradeAction { get; set; }

            public double NewRunningProf { get; set; }
            public double NewTotalRunningProf { get; set; }
            public double StopLoss_RunningTotalProfit { get; set; }

            public bool Trigger_Close { get; set; }
            public bool Trigger_Open { get; set; }


            public MAMA Mama { get; set; }


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

            public enum TradeActions
            {
                None,
                CloseTrade,
                OpenTrade,

            }


        }



    }




    public class TakeProfit_Range
    {
        private List<TakeProfit_Range.TakeProfitTrade> _FullTradeList = new List<TakeProfit_Range.TakeProfitTrade>();
        private List<CompletedTrade> _CompletedTrades = new List<CompletedTrade>();
        private double NewTotalRunningPL;
        private List<MAMA> mama;
        private List<VariableIndicator> VarList = new List<VariableIndicator>();

        public TakeProfit_Range(List<Trade> FullTradeList, List<CompletedTrade> CompletedTrades)
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


            NewTotalRunningPL = 0;
            int C = _CompletedTrades.Count;
            for (int i = 1; i < C; i++)
            {
                var pl = from x in _FullTradeList
                         where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp
                         select x;

                var tpl = pl.ToList();
                AdjustVolume(tpl);
                SetCenterLine(tpl);
                SetCloseTriggers(tpl);
                SetOpenTriggers(tpl);
                SetOpenCloseRawTriggers(tpl);
                SetOpenCloseTriggers(tpl);
                CalcProfLoss(tpl);
            }
            Print();
        }





        private void Print()
        {
            StreamWriter sr = new StreamWriter(@"d:\tt.txt");
            foreach (var d in _FullTradeList.Where(z => z.StopLoss_CenterLine != 0).Skip(1000))
            {
                var data = d.TimeStamp + "," + d.Reason + "," + d.RunningProfit + "," + d.StopLoss_RunningTotalProfit + "," + d.StopLoss_CenterLine + "," + d.StopLoss_TakeProfitLevel_1
                                + "," + d.StopLoss_TakeProfitLevel_2 + "," + d.StopLoss_StopLossLevel_1
                                + "," + d.Mama.Mama + "," + d.Mama.Fama;
                //+ "," + d.TakeProfit_Trigger1_Close + "," + d.TakeProfit_Trigger2_Close + "," + d.StopLoss_Triggered1_Close
                // + "," + d.TakeProfit_Trigger1_ReEnter + "," + d.TakeProfit_Trigger2_ReEnter + "," + d.StopLoss_Triggered1_ReEnter
                // + "," + d.TradeAction_Raw + "," + d.TradeAction + "," + d.IntraPosition + "," + d.NewRunningProf + "," + d.NewTotalRunningProf +","+d.TradeVolume;
                //	Debug.WriteLine(data);
                //	AlsiUtils.Utilities.PrintAllProperties(d);
                sr.WriteLine(data);
            }
            sr.Close();
        }
        private void SetCenterLine(List<TakeProfitTrade> tpt)
        {
            var r = tpt.First().TotalPL;
            var count = tpt.Count();
            for (int q = 0; q < count; q++)
            {
                tpt[q].StopLoss_CenterLine = r;
                tpt[q].StopLoss_StopLossLevel_1 = r + tpt[q].StopLoss_LoweLevel_1;
                tpt[q].StopLoss_TakeProfitLevel_1 = r + tpt[q].StopLoss_UpperLevel_1;
                tpt[q].StopLoss_TakeProfitLevel_2 = r + tpt[q].StopLoss_UpperLevel_2;
                tpt[q].StopLoss_RunningTotalProfit = r + tpt[q].RunningProfit;

                //set Variable List
                var v = new VariableIndicator()
                {
                    TimeStamp = tpt[q].TimeStamp,
                    Value = tpt[q].StopLoss_RunningTotalProfit,
                };
                VarList.Add(v);

            }

        }




        private void AdjustVolume(List<TakeProfitTrade> tpt)
        {
            var r = tpt.First().TotalPL;
            var count = tpt.Count();
            var vol = 1;// tpt[0].TradeVolume;
            for (int q = 0; q < count; q++)
            {
                tpt[q].TradeVolume = vol;
                tpt[q].RunningProfit = tpt[q].RunningProfit / vol;
            }
        }
        private void SetCloseTriggers(List<TakeProfitTrade> tpt)
        {
            int i = tpt.Count;
            var upper1 = tpt[0].StopLoss_UpperLevel_1 + tpt[0].StopLoss_CenterLine;
            var upper2 = tpt[0].StopLoss_UpperLevel_2 + tpt[0].StopLoss_CenterLine;
            var lower1 = tpt[0].StopLoss_LoweLevel_1 + tpt[0].StopLoss_CenterLine;
            for (int x = 1; x < i; x++)
            {

                //UpperLevel1-Close Trade
                if (tpt[x].StopLoss_RunningTotalProfit < upper1 && tpt[x - 1].StopLoss_RunningTotalProfit > upper1) tpt[x].TakeProfit_Trigger1_Close = true;

                //UpperLevel2-Close Trade
                if (tpt[x].StopLoss_RunningTotalProfit < upper2 && tpt[x - 1].StopLoss_RunningTotalProfit > upper2) tpt[x].TakeProfit_Trigger2_Close = true;

                //LowerLevel1-Close Trade
                if (tpt[x].StopLoss_RunningTotalProfit < lower1 && tpt[x - 1].StopLoss_RunningTotalProfit > lower1) tpt[x].StopLoss_Triggered1_Close = true;

            }
        }
        private void SetOpenTriggers(List<TakeProfitTrade> tpt)
        {
            int i = tpt.Count;
            var upper1 = tpt[0].StopLoss_UpperLevel_1 + tpt[0].StopLoss_CenterLine;
            var upper2 = tpt[0].StopLoss_UpperLevel_2 + tpt[0].StopLoss_CenterLine;
            var lower1 = tpt[0].StopLoss_LoweLevel_1 + tpt[0].StopLoss_CenterLine;
            for (int x = 1; x < i; x++)
            {

                //UpperLevel1-ReEnter Trade
                if (tpt[x].StopLoss_RunningTotalProfit > upper1 && tpt[x - 1].StopLoss_RunningTotalProfit < upper1) tpt[x].TakeProfit_Trigger1_ReEnter = true;

                //UpperLevel2-ReEnter Trade
                if (tpt[x].StopLoss_RunningTotalProfit > upper2 && tpt[x - 1].StopLoss_RunningTotalProfit < upper2) tpt[x].TakeProfit_Trigger2_ReEnter = true;

                //LowerLevel1-AddPosition Trade
                if (tpt[x].StopLoss_RunningTotalProfit > lower1 && tpt[x - 1].StopLoss_RunningTotalProfit < lower1) tpt[x].StopLoss_Triggered1_ReEnter = true;


            }
        }
        private void SetOpenCloseRawTriggers(List<TakeProfitTrade> tpt)
        {

            tpt[0].TradeAction_Raw = TakeProfitTrade.TradeActions.None;

            int i = tpt.Count;

            for (int x = 1; x < i; x++)
            {
                tpt[x].TradeAction_Raw = TakeProfitTrade.TradeActions.None;
                if (tpt[x].TakeProfit_Trigger1_Close || tpt[x].TakeProfit_Trigger2_Close) tpt[x].TradeAction_Raw = TakeProfitTrade.TradeActions.CloseTrade;
                if (tpt[x].TakeProfit_Trigger1_ReEnter) tpt[x].TradeAction_Raw = TakeProfitTrade.TradeActions.OpenTrade;

            }
        }
        private void SetOpenCloseTriggers(List<TakeProfitTrade> tpt)
        {

            tpt[0].TradeAction = TakeProfitTrade.TradeActions.None;
            tpt[0].IntraPosition = true;
            int i = tpt.Count - 1;

            for (int x = 1; x < i; x++)
            {
                //set start
                tpt[x].IntraPosition = tpt[x - 1].IntraPosition;
                //close trade
                if (tpt[x].IntraPosition && tpt[x].TradeAction_Raw == TakeProfitTrade.TradeActions.CloseTrade)
                {
                    tpt[x].IntraPosition = false;
                    tpt[x].TradeAction = tpt[x].TradeAction_Raw;
                }

                //open trade
                if (!tpt[x].IntraPosition && tpt[x].TradeAction_Raw == TakeProfitTrade.TradeActions.OpenTrade)
                {
                    tpt[x].IntraPosition = true;
                    tpt[x].TradeAction = tpt[x].TradeAction_Raw;
                }
            }
        }
        private void CalcProfLoss(List<TakeProfitTrade> tpt)
        {
            int i = tpt.Count;
            double pl = 0;
            var vol = tpt[0].TradeVolume;
            tpt[0].NewTotalRunningProf = NewTotalRunningPL;
            for (int x = 1; x < i; x++)
            {
                double tickpl = tpt[x].RunningProfit - tpt[x - 1].RunningProfit;
                if (tpt[x - 1].IntraPosition) pl += (tickpl * vol);
                tpt[x].NewRunningProf += (pl * vol);
                if (tpt[x - 1].IntraPosition) tpt[x].NewTotalRunningProf = NewTotalRunningPL += (tickpl * vol);
                else
                    tpt[x].NewTotalRunningProf = tpt[x - 1].NewTotalRunningProf;
            }


        }



        public class TakeProfitTrade : Trade
        {
            public bool IntraPosition { get; set; }
            public TradeActions TradeAction_Raw { get; set; }
            public TradeActions TradeAction { get; set; }

            public double NewRunningProf { get; set; }
            public double NewTotalRunningProf { get; set; }

            public double StopLoss_UpperLevel_1;
            public double StopLoss_UpperLevel_2;
            public double StopLoss_LoweLevel_1;

            public double StopLoss_RunningTotalProfit { get; set; }
            public double StopLoss_TakeProfitLevel_1 { get; set; }
            public double StopLoss_TakeProfitLevel_2 { get; set; }
            public double StopLoss_StopLossLevel_1 { get; set; }
            public double StopLoss_CenterLine { get; set; }

            public bool TakeProfit_Trigger1_Close;
            public bool TakeProfit_Trigger2_Close;
            public bool StopLoss_Triggered1_Close;

            public bool TakeProfit_Trigger1_ReEnter;
            public bool TakeProfit_Trigger2_ReEnter;
            public bool StopLoss_Triggered1_ReEnter;

            public MAMA Mama { get; set; }


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

            public enum TradeActions
            {
                None,
                CloseTrade,
                OpenTrade,
                AddPosition,
            }

        }



    }
}
