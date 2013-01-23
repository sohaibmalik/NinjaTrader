using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AlsiUtils.Indicators;

namespace AlsiUtils.Strategies
{
    public class TradeStrategy2 : Indicator, ICloneable
    {

        private static List<TradeStrategy2> _O = new List<TradeStrategy2>();
        private static List<TradeStrategy2> _C = new List<TradeStrategy2>();
        private static List<TradeStrategy2> _ocA = new List<TradeStrategy2>();
        private static List<TradeStrategy2> _ocB = new List<TradeStrategy2>();
        private static List<TradeStrategy2> _ocC = new List<TradeStrategy2>();
        private static List<TradeStrategy2> _ocD = new List<TradeStrategy2>();

        private static bool[] _Position_a;
        
        private static OpenPriority _openPriority;
        private static ClosePriority _closePriority;
        private static List<Price> _Prices;
        private DateTime _Start;
        private static Parameter_General _p = new Parameter_General();

        public delegate void Triggers_Delegate(List<TradeStrategy2> TradeStraterty, int Index);
        public static Triggers_Delegate _TriggersOpen;
        public static Triggers_Delegate _TriggersClose;



        public object Clone()
        {
            return this.MemberwiseClone();
        }

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


        public List<TradeStrategy2> getStrategyList()
        {
            return _ocD;
        }


        public void Calculate()
        {
            //Debug.WriteLine(GC.GetTotalMemory(true));

            #region Phase 1

            Prepare();
            OpenCloseTriggers();
            Positions();
            //Triggers_Merge_and_Create_A_List();
            //Directions();
            //Directions_Triggers_adjust_a();
            // Directions_ajust_b();
            //ClearListToFreeMemory(_O);
            //ClearListToFreeMemory(_C);
            //Create_B_List();
            //ClearListToFreeMemory(_ocA);

            //RunningProfits();
            //MarkTrades_A();
            #endregion

            #region Phase 2 - Stoploss & TakeProfit

            //Create_C_List();
            //ClearListToFreeMemory(_ocB);
            //StopLoss_and_TakeProfit_Triggers();
            //Directions_Triggers_adjust_b();
            //Mark_Actual_Trades();
            //Create_D_List();
            //ClearListToFreeMemory(_ocC);


            #endregion

            #region Phase 3 - Final Version

            //ResetPL();
            //FillInDetails();
            //CalcPL();

            #endregion

            var List = _ocA;
           for (int x = 1; x < List.Count; x++) Print(List, x);

        }

        #region Phase 1

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
            //get Open and Close triggers
            for (int x = 1; x < _O.Count; x++) _TriggersOpen(_O, x);
            for (int x = 1; x < _C.Count; x++) _TriggersClose(_C, x);
        }

        private void Positions()
        {
          
            for (int x = 1; x < _O.Count; x++)
            {
              
                //_Position_a[x] = _Position_a[x - 1];
                // if (_C[x].TradeTrigger != Trade.Trigger.None) _Position_a[x] = false;
                // if (_O[x].TradeTrigger != Trade.Trigger.None) _Position_a[x] = true;
            }


            
        }

        private void Triggers_Merge_and_Create_A_List()
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
            var A = _ocA;
            for (int x = 1; x < A.Count; x++)
            {

                A[x].TradeDirection = A[x - 1].TradeDirection;

                if (A[x].TradeTrigger == Trade.Trigger.OpenLong ||
                    A[x].TradeTrigger == Trade.Trigger.ReverseLong)
                    A[x].TradeDirection = Trade.Direction.Long;

                if (A[x].TradeTrigger == Trade.Trigger.OpenShort ||
                   A[x].TradeTrigger == Trade.Trigger.ReverseShort)
                    A[x].TradeDirection = Trade.Direction.Short;

                if (A[x].Position == false) A[x].TradeDirection = Trade.Direction.None;

            }
        }

        private void Directions_Triggers_adjust_a()
        {
            var A = _ocA;
            for (int x = 1; x < A.Count; x++)
            {


                if (A[x - 1].TradeDirection == Trade.Direction.Long && (A[x].TradeTrigger == Trade.Trigger.OpenShort || A[x].TradeTrigger == Trade.Trigger.CloseShort))
                {
                    A[x].TradeTrigger = Trade.Trigger.None;
                    A[x].TradeDirection = Trade.Direction.Long;
                }

                if (A[x - 1].TradeDirection == Trade.Direction.Short && (A[x].TradeTrigger == Trade.Trigger.OpenLong || A[x].TradeTrigger == Trade.Trigger.CloseLong))
                {
                    A[x].TradeTrigger = Trade.Trigger.None;
                    A[x].TradeDirection = Trade.Direction.Short;
                }


                if (A[x - 1].TradeDirection == Trade.Direction.Long && A[x].TradeTrigger == Trade.Trigger.OpenLong) A[x].TradeTrigger = Trade.Trigger.None;
                if (A[x - 1].TradeDirection == Trade.Direction.Short && A[x].TradeTrigger == Trade.Trigger.OpenShort) A[x].TradeTrigger = Trade.Trigger.None;
                if (A[x - 1].TradeDirection == Trade.Direction.Long && A[x].TradeTrigger == Trade.Trigger.ReverseLong) A[x].TradeTrigger = Trade.Trigger.None;
                if (A[x - 1].TradeDirection == Trade.Direction.Short && A[x].TradeTrigger == Trade.Trigger.ReverseShort) A[x].TradeTrigger = Trade.Trigger.None;

                if ((A[x - 1].TradeTrigger == Trade.Trigger.CloseShort || A[x - 1].TradeTrigger == Trade.Trigger.CloseLong) &&
                    (A[x].TradeTrigger == Trade.Trigger.ReverseShort || A[x].TradeTrigger == Trade.Trigger.ReverseLong)
                    ) A[x].TradeTrigger = _O[x].TradeTrigger;

                if (A[x - 1].TradeDirection == Trade.Direction.None && (A[x].TradeTrigger == Trade.Trigger.ReverseShort || A[x].TradeTrigger == Trade.Trigger.ReverseLong))
                    A[x].TradeTrigger = _O[x].TradeTrigger;



                if (A[x - 1].TradeDirection == Trade.Direction.None && (A[x].TradeTrigger == Trade.Trigger.CloseShort || A[x].TradeTrigger == Trade.Trigger.CloseLong))
                    A[x].TradeTrigger = Trade.Trigger.None;



            }
        }

        private void Directions_ajust_b()
        {
            var A = _ocA;
            for (int x = 1; x < A.Count; x++)
            {
                A[x].TradeDirection = Trade.Direction.None;
                A[x].TradeDirection = A[x - 1].TradeDirection;


                if (A[x].TradeTrigger == Trade.Trigger.OpenLong || A[x].TradeTrigger == Trade.Trigger.ReverseLong) A[x].TradeDirection = Trade.Direction.Long;
                if (A[x].TradeTrigger == Trade.Trigger.OpenShort || A[x].TradeTrigger == Trade.Trigger.ReverseShort) A[x].TradeDirection = Trade.Direction.Short;
                if (A[x].TradeTrigger == Trade.Trigger.CloseLong || A[x].TradeTrigger == Trade.Trigger.CloseShort) A[x].TradeDirection = Trade.Direction.None;


                if (A[x].TradeTrigger == Trade.Trigger.EndOfDayClose && A[x].TradeDirection == Trade.Direction.Long)
                {
                    A[x].TradeTrigger = Trade.Trigger.EndOfDayCloseLong;
                    A[x].TradeDirection = Trade.Direction.None;
                }
                if (A[x].TradeTrigger == Trade.Trigger.EndOfDayClose && A[x].TradeDirection == Trade.Direction.Short)
                {
                    A[x].TradeTrigger = Trade.Trigger.EndOfDayCloseShort;
                    A[x].TradeDirection = Trade.Direction.None;
                }





            }
        }

        private void Create_B_List()
        {
            foreach (var a in _ocA)
            {
                TradeStrategy2 b = (TradeStrategy2)a.Clone();
                _ocB.Add(b);
            }
        }

        private void RunningProfits()
        {

            var B = _ocB;
            for (int x = 1; x < B.Count; x++)
            {
                B[x].TradedPrice = B[x - 1].TradedPrice;
                if (B[x].TradeTrigger != Trade.Trigger.None) B[x].TradedPrice = B[x].Price_Close;

                if (B[x].TradeDirection == Trade.Direction.Long) B[x].RunningProfit = B[x].Price_Close - B[x].TradedPrice;
                if (B[x].TradeDirection == Trade.Direction.Short) B[x].RunningProfit = B[x].TradedPrice - B[x].Price_Close;

                if (B[x].TradeTrigger == Trade.Trigger.CloseLong || B[x].TradeTrigger == Trade.Trigger.ReverseShort || B[x].TradeTrigger == Trade.Trigger.EndOfDayCloseLong)
                    B[x].RunningProfit = B[x].TradedPrice - B[x - 1].TradedPrice;

                if (B[x].TradeTrigger == Trade.Trigger.CloseShort || B[x].TradeTrigger == Trade.Trigger.ReverseLong || B[x].TradeTrigger == Trade.Trigger.EndOfDayCloseShort)
                    B[x].RunningProfit = B[x - 1].TradedPrice - B[x].TradedPrice;

            }
        }

        private void MarkTrades_A()
        {
            var B = _ocB;
            for (int x = 1; x < B.Count; x++)
            {
                B[x].TradeTriggerGeneral = Trade.Trigger.None;
                if (B[x].TradeTrigger == Trade.Trigger.OpenLong || B[x].TradeTrigger == Trade.Trigger.OpenShort) B[x].TradeTriggerGeneral = Trade.Trigger.Open;
                if (B[x].TradeTrigger == Trade.Trigger.CloseLong || B[x].TradeTrigger == Trade.Trigger.CloseShort) B[x].TradeTriggerGeneral = Trade.Trigger.Close;
                if (B[x].TradeTrigger == Trade.Trigger.ReverseLong || B[x].TradeTrigger == Trade.Trigger.ReverseShort) B[x].TradeTriggerGeneral = Trade.Trigger.Reverse;
                if (B[x].TradeTrigger == Trade.Trigger.EndOfDayCloseLong || B[x].TradeTrigger == Trade.Trigger.EndOfDayCloseShort || B[x].TradeTrigger == Trade.Trigger.EndOfDayClose) B[x].TradeTriggerGeneral = Trade.Trigger.EndOfDayClose;
            }
        }

        #endregion

        #region Phase 2 - Stoploss & TakeProfit

        private void Create_C_List()
        {
            foreach (var a in _ocB)
            {
                TradeStrategy2 b = (TradeStrategy2)a.Clone();
                _ocC.Add(b);
            }
        }

        private void StopLoss_and_TakeProfit_Triggers()
        {
            var C = _ocC;
            for (int x = 1; x < C.Count; x++)
            {

                //Take Profit
                if (C[x].RunningProfit > _p.TakeProfit)
                    if (C[x].TradeTrigger == Trade.Trigger.None)
                    {
                        {
                            C[x].TradedPrice = C[x].Price_Close;
                            C[x].TradeTriggerGeneral = Trade.Trigger.TakeProfit;
                            if (C[x].TradeDirection == Trade.Direction.Long) C[x].TradeTrigger = Trade.Trigger.TakeProfitLong;
                            if (C[x].TradeDirection == Trade.Direction.Short) C[x].TradeTrigger = Trade.Trigger.TakeProfitShort;
                        }
                    }

                //Stoploss
                if (C[x].RunningProfit < _p.StopLoss)
                {
                    if (C[x].TradeTrigger == Trade.Trigger.None)
                    {
                        C[x].TradedPrice = C[x].Price_Close;
                        C[x].TradeTriggerGeneral = Trade.Trigger.StopLoss;
                        if (C[x].TradeDirection == Trade.Direction.Long) C[x].TradeTrigger = Trade.Trigger.StopLossLong;
                        if (C[x].TradeDirection == Trade.Direction.Short) C[x].TradeTrigger = Trade.Trigger.StopLossShort;
                    }
                }
            }
        }

        private void Directions_Triggers_adjust_b()
        {
            var C = _ocC;
            //  var L = C;

            bool stopOrtake = false;
            for (int x = 1; x < C.Count; x++)
            {

                if (C[x].TradeTriggerGeneral == Trade.Trigger.Open) stopOrtake = false;
                if ((stopOrtake) && (C[x].TradeTriggerGeneral == Trade.Trigger.TakeProfit || C[x].TradeTriggerGeneral == Trade.Trigger.StopLoss))
                    C[x].TradeTrigger = Trade.Trigger.None;
                if ((!stopOrtake) && (C[x].TradeTriggerGeneral == Trade.Trigger.TakeProfit || C[x].TradeTriggerGeneral == Trade.Trigger.StopLoss))
                    stopOrtake = true;

                if ((C[x].TradeTriggerGeneral == Trade.Trigger.Close || C[x].TradeTriggerGeneral == Trade.Trigger.Reverse ||
                    C[x].TradeTriggerGeneral == Trade.Trigger.EndOfDayClose) && stopOrtake) C[x].TradeTrigger = Trade.Trigger.None;
                //  Debug.WriteLine(L[x].TimeStamp + "  " + L[x].TradeTrigger + "   " + L[x].Price_Close + "  " + L[x].RunningProfit + "   " + L[x].TradeDirection + "   " + L[x].Position + "  *******  " + L[x].TradeTriggerGeneral + "   " + stopOrtake);
            }


        }

        private void Mark_Actual_Trades()
        {
            var C = _ocC;

            for (int x = 1; x < C.Count; x++)
            {
                if (C[x].TradeTrigger == Trade.Trigger.OpenLong) C[x].ActualTrade = Trade.Trigger.OpenLong;
                if (C[x].TradeTrigger == Trade.Trigger.OpenShort) C[x].ActualTrade = Trade.Trigger.OpenShort;
                if (C[x].TradeTrigger == Trade.Trigger.ReverseLong) C[x].ActualTrade = Trade.Trigger.ReverseLong;
                if (C[x].TradeTrigger == Trade.Trigger.ReverseShort) C[x].ActualTrade = Trade.Trigger.ReverseShort;

                if (C[x].TradeTrigger == Trade.Trigger.CloseLong ||
                    C[x].TradeTrigger == Trade.Trigger.EndOfDayCloseLong ||
                    C[x].TradeTrigger == Trade.Trigger.TakeProfitLong ||
                   C[x].TradeTrigger == Trade.Trigger.StopLossLong
                    ) C[x].ActualTrade = Trade.Trigger.CloseLong;

                if (C[x].TradeTrigger == Trade.Trigger.CloseShort ||
                   C[x].TradeTrigger == Trade.Trigger.EndOfDayCloseShort ||
                   C[x].TradeTrigger == Trade.Trigger.TakeProfitShort ||
                  C[x].TradeTrigger == Trade.Trigger.StopLossShort
                   ) C[x].ActualTrade = Trade.Trigger.CloseShort;

            }
        }

        private void Create_D_List()
        {
            foreach (var a in _ocC)
            {
                TradeStrategy2 b = (TradeStrategy2)a.Clone();
                _ocD.Add(b);
            }
        }

        #endregion

        #region Phase 3 - Final Version

        private void ResetPL()
        {
            var D = _ocD;
            for (int x = 1; x < D.Count; x++)
            {
                if (D[x].ActualTrade == Trade.Trigger.None)
                {
                    D[x].TradedPrice = 0;
                    D[x].RunningProfit = 0;
                    D[x].TradeTrigger = Trade.Trigger.None;
                }


                D[x].TradeTriggerGeneral = Trade.Trigger.None;
                D[x].Position = false;
                D[x].TradeDirection = Trade.Direction.None;
            }
        }

        private void FillInDetails()
        {
            var D = _ocD;
            for (int x = 1; x < D.Count; x++)
            {
                D[x].TradeDirection = D[x - 1].TradeDirection;
                D[x].Position = D[x - 1].Position;
                // D[x].TradedPrice=D[x-1].TradedPrice;

                //  if (D[x].TradeDirection == Trade.Direction.Long && D[x].ActualTrade == Trade.Trigger.None) D[x].RunningProfit = D[x].Price_Close - D[x].TradedPrice;
                //if (D[x].TradeDirection == Trade.Direction.Short && D[x].ActualTrade == Trade.Trigger.None) D[x].RunningProfit = D[x].TradedPrice - D[x].Price_Close;

                if (D[x].ActualTrade == Trade.Trigger.OpenShort || D[x].ActualTrade == Trade.Trigger.ReverseShort)
                {
                    D[x].TradeDirection = Trade.Direction.Short;
                    D[x].Position = true;

                }
                if (D[x].ActualTrade == Trade.Trigger.OpenLong || D[x].ActualTrade == Trade.Trigger.ReverseLong)
                {
                    D[x].TradeDirection = Trade.Direction.Long;
                    D[x].Position = true;
                }

                if (D[x].ActualTrade == Trade.Trigger.CloseLong || D[x].ActualTrade == Trade.Trigger.CloseShort ||
                    D[x].ActualTrade == Trade.Trigger.EndOfDayCloseLong || D[x].ActualTrade == Trade.Trigger.EndOfDayCloseShort ||
                    D[x].ActualTrade == Trade.Trigger.StopLossLong || D[x].ActualTrade == Trade.Trigger.StopLossShort ||
                    D[x].ActualTrade == Trade.Trigger.TakeProfitLong || D[x].ActualTrade == Trade.Trigger.TakeProfitShort ||
                    D[x].ActualTrade == Trade.Trigger.ReverseLong || D[x].ActualTrade == Trade.Trigger.ReverseShort
                    )
                {
                    D[x].TradeDirection = Trade.Direction.None;
                    D[x].Position = false;
                    D[x].TradedPrice = D[x].Price_Close;
                    D[x].TradeTriggerGeneral = Trade.Trigger.Close;

                }


            }
        }

        private void CalcPL()
        {
            var D = _ocD;
            double pl = 0;
            for (int x = 1; x < D.Count; x++)
            {
                if (D[x].TradeTriggerGeneral == Trade.Trigger.Close)
                {
                    pl += D[x].RunningProfit;
                    D[x].TotalProfit = pl;
                }
            }
        }

        #endregion



        private void ClearListToFreeMemory(List<TradeStrategy2> List)
        {
            List.Clear();
        }

        private void Print(List<TradeStrategy2> L, int x)
        {
            var O = _O;
            var C = _C;
            
           // if (L[x].ActualTrade  != Trade.Trigger.None)
         
            Debug.WriteLine(
                  L[x].TimeStamp + "  " +
                L[x].Price_Close + "  " +
                O[x].TradeTrigger + "  " +
                C[x].TradeTrigger + " ----->   " +
                L[x].TradeTrigger + "  " +
                L[x].TradeDirection + "  " +
                L[x].Position + "   * " +
                _Position_a[x]

           );
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
        public Trade.Trigger TradeTriggerGeneral { get; set; }

        /// <summary>
        /// Should only be Open/Close Long/Short ReverseLong/ReverseShort
        /// </summary>
        public Trade.Trigger ActualTrade { get; set; }
        public Trade.TradeReason Reason { get; set; }
        public double RunningProfit { get; set; }
        public string InstrumentName { get; set; }
        public double TradedPrice { get; set; }
        public double TotalProfit { get; set; }
        public double TradeCount { get; set; }
        public OpenPriority PriorityOpen { get; set; }
        public ClosePriority PriorityClose { get; set; }

        public enum OpenPriority
        {
            IgnoreOnNewOpen,
            ReverseOnNewOpen,
            CloseOnNewOpen,
        }

        public enum ClosePriority
        {
            IgnoreOnClose,
            CloseOnClose,
        }
        //public class Expansion
        //{
        //    List<TradeStrategy2> _ST = _ocD;
        //    private static void Apply_2nd_AlgoLayer(int EMA)
        //    {
        //        List<VariableIndicator> _st = new List<VariableIndicator>();
        //        foreach (var t in _ST)
        //        {
        //            if (t.TotalProfit != 0)
        //            {
        //                VariableIndicator v = new VariableIndicator()
        //                {
        //                    TimeStamp = t.TimeStamp,
        //                    Value = t.TotalProfit,
        //                };
        //                _st.Add(v);
        //            }
        //        }

        //        var ema = Factory_Indicator.createEMA(EMA, _st);
        //        double newprof = ema[0].CustomValue;
        //        TradeStrategy2 mt = null;
        //        bool cantradeA = false;
        //        bool cantradeB = false;
        //        bool closepos = false;
        //        bool first = true;
        //        int count = 0;
        //        foreach (var v in ema)
        //        {
        //            count++;
        //            // if (count > 30) break;          

        //            if (first) ;
        //            mt = _ST.Where(z => z.TimeStamp == v.TimeStamp).First();

        //            if (!first) closepos = (mt.ActualTrade == Trade.Trigger.CloseShort || mt.ActualTrade == Trade.Trigger.CloseLong);
        //            if (closepos && cantradeA) cantradeB = true;
        //            else
        //                cantradeB = false;

        //            cantradeA = (v.CustomValue > v.Ema);

        //            if (!first && closepos && cantradeB) newprof += mt.RunningProfit;

        //            //Debug.WriteLine(((cantradeA) ? "**" : "") + ((cantradeB) ? "**" : "") + v.Timestamp + " " + v.CustomValue + " " + v.Ema +
        //            //    " TradeA :" + cantradeA + "  TradeB :" + cantradeB + " Prof " + newprof +
        //            //    "   " + ((!first && mt != null) ? mt.ActualTrade.ToString() : ""));



        //            first = false;
        //        }

        //        Debug.WriteLine(EMA + "  " + newprof);
        //        Debug.WriteLine("----------------------------------------------");


        //    }

        //    public static List<CompletedTrade> ApplyRegressionFilter(int N, List<Trade> Trades)
        //    {
        //        var CloseTradesOnly = Statistics.RegressionAnalysis_OnPL(N, Trades);
        //        var CompTrades = Trade.TradesOnly(Trades);
        //        var CompleteList = CompletedTrade.CreateList(CompTrades);

        //        for (int x = 2; x < CompleteList.Count; x++)
        //        {
        //            if (CompleteList[x - 2].CloseTrade.Extention.Slope < CompleteList[x - 1].CloseTrade.Extention.Slope
        //                && CompleteList[x - 1].CloseTrade.Extention.Slope < 0)
        //                CompleteList[x].OpenTrade.Extention.OrderVol = 2;
        //        }
        //        foreach (var t in CompleteList)
        //        {
        //            if (t.OpenTrade.Extention.OrderVol == 2) t.CloseTrade.Extention.OrderVol = 2;
        //            else
        //            {
        //                t.CloseTrade.Extention.OrderVol = 1;
        //                t.OpenTrade.Extention.OrderVol = 1;
        //            }
        //        }

        //        return CompleteList;
        //    }

        //}


    }
}
