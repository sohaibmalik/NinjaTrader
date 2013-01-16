using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AlsiUtils.Indicators;

namespace AlsiUtils.Strategies
{
    public class TradeStrategy2 : Indicator
    {

        private static List<TradeStrategy2> _O = new List<TradeStrategy2>();
        private static List<TradeStrategy2> _C = new List<TradeStrategy2>();
        private static List<TradeStrategy2> _ocA = new List<TradeStrategy2>();
        private static bool[] _Position_a;
        private static List<Price> _Prices;
        private DateTime _Start;
        private static Parameter_General _p = new Parameter_General();

        public delegate void Triggers_Delegate(List<TradeStrategy2> TradeStraterty, int Index);
        public static Triggers_Delegate _TriggersOpen;
        public static Triggers_Delegate _TriggersClose;

        public TradeStrategy2()
        {

        }
        public TradeStrategy2(List<Price> Prices, DateTime Start, Parameter_General parameter, Triggers_Delegate OpenTriggers, Triggers_Delegate CloseTriggers)
        {
            _p.StopLoss = parameter.StopLoss;
            _p.TakeProfit = parameter.TakeProfit;
            _TriggersOpen = OpenTriggers;
            _TriggersClose = CloseTriggers;
            _p.CloseEndofDay = parameter.CloseEndofDay;
            _Prices = Prices;
            _Start = Start;



        }


        public void Calculate()
        {
            Prepare();
            OpenCloseTriggers();
            Positions();
            Triggers_Merge_1();
            Directions();
            Directions_Triggers_adjust_a();
            Directoins_ajust_b();


            for (int x = 1; x < _O.Count; x++) Print(x);

        }

        private void Prepare()
        {
            #region Prices
            foreach (Price s in _Prices)
            {
                if (s.TimeStamp >= _Start)
                {
                    TradeStrategy2 sto = new TradeStrategy2
                    {
                        TimeStamp = s.TimeStamp,
                        Price_Open = s.Open,
                        Price_Close = s.Close,
                        Price_High = s.High,
                        Price_Low = s.Low,
                        InstrumentName = s.InstrumentName
                    };
                    _O.Add(sto);
                    TradeStrategy2 stc = new TradeStrategy2
                    {
                        TimeStamp = s.TimeStamp,
                        Price_Open = s.Open,
                        Price_Close = s.Close,
                        Price_High = s.High,
                        Price_Low = s.Low,
                        InstrumentName = s.InstrumentName
                    };
                    _C.Add(stc);

                    TradeStrategy2 oca = new TradeStrategy2
                    {
                        TimeStamp = s.TimeStamp,
                        Price_Open = s.Open,
                        Price_Close = s.Close,
                        Price_High = s.High,
                        Price_Low = s.Low,
                        InstrumentName = s.InstrumentName
                    };
                    _ocA.Add(oca);
                }
            }
            #endregion

            _Position_a = new bool[_O.Count];

            _O[0].Position = false;
            _O[0].TradeDirection = Trade.Direction.None;
            _O[0].TradeTrigger = Trade.Trigger.None;

            _C[0].Position = false;
            _C[0].TradeDirection = Trade.Direction.None;
            _C[0].TradeTrigger = Trade.Trigger.None;

            _ocA[0].Position = false;
            _ocA[0].TradeDirection = Trade.Direction.None;
            _ocA[0].TradeTrigger = Trade.Trigger.None;


            for (int x = 1; x < _O.Count; x++) _O[x].TradeTrigger = Trade.Trigger.None;
            for (int x = 1; x < _C.Count; x++) _C[x].TradeTrigger = Trade.Trigger.None;
        }

        private void OpenCloseTriggers()
        {
            for (int x = 1; x < _O.Count; x++) _TriggersOpen(_O, x);
            for (int x = 1; x < _C.Count; x++) _TriggersClose(_C, x);
        }

        private void Positions()
        {
            for (int x = 1; x < _O.Count; x++)
            {
                _Position_a[x] = _Position_a[x - 1];
                if (_C[x].TradeTrigger != Trade.Trigger.None) _Position_a[x] = false;
                if (_O[x].TradeTrigger != Trade.Trigger.None) _Position_a[x] = true;


            }
        }

        private void Triggers_Merge_1()
        {
            for (int x = 1; x < _O.Count; x++)
            {
                var trigger = new Trade.Trigger();
                trigger = Trade.Trigger.None;

                if (_O[x].TradeTrigger != Trade.Trigger.None) trigger = _O[x].TradeTrigger;
                if (_C[x].TradeTrigger != Trade.Trigger.None) trigger = _C[x].TradeTrigger;

                if (_O[x].TradeTrigger != Trade.Trigger.None && _C[x].TradeTrigger != Trade.Trigger.None)
                {
                    if (_O[x].TradeTrigger == Trade.Trigger.OpenLong) trigger = Trade.Trigger.ReverseLong;
                    if (_O[x].TradeTrigger == Trade.Trigger.OpenShort) trigger = Trade.Trigger.ReverseShort;
                }

                var s = _ocA[x];
                s.Position = _Position_a[x];
                s.TradeTrigger = trigger;

            }
        }

        private void Directions()
        {
            for (int x = 1; x < _O.Count; x++)
            {

                _ocA[x].TradeDirection = _ocA[x - 1].TradeDirection;

                if (_ocA[x].TradeTrigger == Trade.Trigger.OpenLong ||
                    _ocA[x].TradeTrigger == Trade.Trigger.ReverseLong)
                    _ocA[x].TradeDirection = Trade.Direction.Long;

                if (_ocA[x].TradeTrigger == Trade.Trigger.OpenShort ||
                   _ocA[x].TradeTrigger == Trade.Trigger.ReverseShort)
                    _ocA[x].TradeDirection = Trade.Direction.Short;

                if (_ocA[x].Position == false) _ocA[x].TradeDirection = Trade.Direction.None;

            }
        }

        private void Directions_Triggers_adjust_a()
        {
            for (int x = 1; x < _O.Count; x++)
            {
                if (_ocA[x - 1].TradeDirection == Trade.Direction.Long && (_ocA[x].TradeTrigger == Trade.Trigger.OpenShort || _ocA[x].TradeTrigger == Trade.Trigger.CloseShort))
                {
                    _ocA[x].TradeTrigger = Trade.Trigger.None;
                    _ocA[x].TradeDirection = Trade.Direction.Long;
                }

                if (_ocA[x - 1].TradeDirection == Trade.Direction.Short && (_ocA[x].TradeTrigger == Trade.Trigger.OpenLong || _ocA[x].TradeTrigger == Trade.Trigger.CloseLong))
                {
                    _ocA[x].TradeTrigger = Trade.Trigger.None;
                    _ocA[x].TradeDirection = Trade.Direction.Short;
                }


                if (_ocA[x - 1].TradeDirection == Trade.Direction.Long && _ocA[x].TradeTrigger == Trade.Trigger.OpenLong) _ocA[x].TradeTrigger = Trade.Trigger.None;
                if (_ocA[x - 1].TradeDirection == Trade.Direction.Short && _ocA[x].TradeTrigger == Trade.Trigger.OpenShort) _ocA[x].TradeTrigger = Trade.Trigger.None;
                if (_ocA[x - 1].TradeDirection == Trade.Direction.Long && _ocA[x].TradeTrigger == Trade.Trigger.ReverseLong) _ocA[x].TradeTrigger = Trade.Trigger.None;
                if (_ocA[x - 1].TradeDirection == Trade.Direction.Short && _ocA[x].TradeTrigger == Trade.Trigger.ReverseShort) _ocA[x].TradeTrigger = Trade.Trigger.None;

                if ((_ocA[x - 1].TradeTrigger == Trade.Trigger.CloseShort || _ocA[x - 1].TradeTrigger == Trade.Trigger.CloseLong) &&
                    (_ocA[x].TradeTrigger == Trade.Trigger.ReverseShort || _ocA[x].TradeTrigger == Trade.Trigger.ReverseLong)
                    ) _ocA[x].TradeTrigger = _O[x].TradeTrigger;

                if (_ocA[x - 1].TradeDirection == Trade.Direction.None && (_ocA[x].TradeTrigger == Trade.Trigger.CloseShort || _ocA[x].TradeTrigger == Trade.Trigger.CloseLong))
                    _ocA[x].TradeTrigger = Trade.Trigger.None;
            }
        }

        private void Directoins_ajust_b()
        {
            for (int x = 1; x < _O.Count; x++)
            {
                _ocA[x].TradeDirection = Trade.Direction.None;
                _ocA[x].TradeDirection = _ocA[x - 1].TradeDirection;
                if (_ocA[x].TradeTrigger == Trade.Trigger.OpenLong || _ocA[x].TradeTrigger == Trade.Trigger.ReverseLong) _ocA[x].TradeDirection = Trade.Direction.Long;
                if (_ocA[x].TradeTrigger == Trade.Trigger.OpenShort || _ocA[x].TradeTrigger == Trade.Trigger.ReverseShort) _ocA[x].TradeDirection = Trade.Direction.Short;
            }
        }











        private void Print(int x)
        {
            //if(_ocA[x].TradeTrigger!=Trade.Trigger.None )
            Debug.WriteLine(_O[x].TimeStamp + " " + _O[x].TradeTrigger + "  " + _C[x].TradeTrigger + "  " + _ocA[x].TradeTrigger + "  " + _ocA[x].TradeDirection);
        }









        public static Trade.BuySell GetBuySell(Trade.Trigger trigger)
        {
            switch (trigger)
            {
                case Trade.Trigger.CloseLong:
                    return Trade.BuySell.Sell;
                    break;

                case Trade.Trigger.CloseShort:
                    return Trade.BuySell.Buy;
                    break;

                case Trade.Trigger.OpenLong:
                    return Trade.BuySell.Buy;
                    break;

                case Trade.Trigger.OpenShort:
                    return Trade.BuySell.Sell;
                    break;

                case Trade.Trigger.EndOfDayCloseLong:
                    return Trade.BuySell.Sell;
                    break;

                case Trade.Trigger.EndOfDayCloseShort:
                    return Trade.BuySell.Buy;
                    break;

            }
            return Trade.BuySell.None;
        }

        public bool Position { get; set; }
        public Trade.Direction TradeDirection { get; set; }
        public Trade.Trigger TradeTrigger { get; set; }
        public Trade.Trigger ActualTrade { get; set; }
        public Trade.TradeReason Reason { get; set; }
        public double RunningProfit { get; set; }
        public string InstrumentName { get; set; }

        public double TradedPrice { get; set; }
        public bool markedObjectA { get; set; }
        public bool markedObjectB { get; set; }

        public double TotalProfit { get; set; }
        public double TradeCount { get; set; }



        public class Expansion
        {
            //private static void Apply_2nd_AlgoLayer(int EMA)
            //{
            //    List<VariableIndicator> _st = new List<VariableIndicator>();
            //    foreach (var t in _ST)
            //    {
            //        if (t.TotalProfit != 0)
            //        {
            //            VariableIndicator v = new VariableIndicator()
            //            {
            //                TimeStamp = t.TimeStamp,
            //                Value = t.TotalProfit,
            //            };
            //            _st.Add(v);
            //        }
            //    }

            //    var ema = Factory_Indicator.createEMA(EMA, _st);
            //    double newprof = ema[0].CustomValue;
            //    TradeStrategy2 mt = null;
            //    bool cantradeA = false;
            //    bool cantradeB = false;
            //    bool closepos = false;
            //    bool first = true;
            //    int count = 0;
            //    foreach (var v in ema)
            //    {
            //        count++;
            //        // if (count > 30) break;          

            //        if (first) ;
            //        mt = _ST.Where(z => z.TimeStamp == v.TimeStamp).First();

            //        if (!first) closepos = (mt.ActualTrade == Trade.Trigger.CloseShort || mt.ActualTrade == Trade.Trigger.CloseLong);
            //        if (closepos && cantradeA) cantradeB = true;
            //        else
            //            cantradeB = false;

            //        cantradeA = (v.CustomValue > v.Ema);

            //        if (!first && closepos && cantradeB) newprof += mt.RunningProfit;

            //        //Debug.WriteLine(((cantradeA) ? "**" : "") + ((cantradeB) ? "**" : "") + v.Timestamp + " " + v.CustomValue + " " + v.Ema +
            //        //    " TradeA :" + cantradeA + "  TradeB :" + cantradeB + " Prof " + newprof +
            //        //    "   " + ((!first && mt != null) ? mt.ActualTrade.ToString() : ""));



            //        first = false;
            //    }

            //    Debug.WriteLine(EMA + "  " + newprof);
            //    Debug.WriteLine("----------------------------------------------");


            //}

            //public static List<CompletedTrade> ApplyRegressionFilter(int N, List<Trade> Trades)
            //{
            //    var CloseTradesOnly = Statistics.RegressionAnalysis_OnPL(N, Trades);
            //    var CompTrades = Trade.TradesOnly(Trades);
            //    var CompleteList = CompletedTrade.CreateList(CompTrades);

            //    for (int x = 2; x < CompleteList.Count; x++)
            //    {
            //        if (CompleteList[x - 2].CloseTrade.Extention.Slope < CompleteList[x - 1].CloseTrade.Extention.Slope
            //            && CompleteList[x - 1].CloseTrade.Extention.Slope < 0)
            //            CompleteList[x].OpenTrade.Extention.OrderVol = 2;
            //    }
            //    foreach (var t in CompleteList)
            //    {
            //        if (t.OpenTrade.Extention.OrderVol == 2) t.CloseTrade.Extention.OrderVol = 2;
            //        else
            //        {
            //            t.CloseTrade.Extention.OrderVol = 1;
            //            t.OpenTrade.Extention.OrderVol = 1;
            //        }
            //    }

            //    return CompleteList;
            //}

        }
    }
}
