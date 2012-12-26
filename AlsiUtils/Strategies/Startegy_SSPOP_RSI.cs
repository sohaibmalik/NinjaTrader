using System;
using System.Collections.Generic;
using System.Diagnostics;
using AlsiUtils.Indicators;
using System.Linq;

namespace AlsiUtils.Strategies
{
    public class Startegy_SSPOP_RSI
    {
        private static Parameter_SS_RSI _p;
        private static List<SlowStoch> _SS;
        private static List<Rsi> _RSI;
        private static List<VariableIndicator> _rsi_to_Var = new List<VariableIndicator>();
        private static List<EMA> _RSI_MA;
        private static List<SMA> _RSI_MA2;
        private static List<TradeStrategy> _T;

        public static void SsPopStrategy(Strategies.Parameter_SS_RSI Parameters, List<Price> price)
        {
            _p = Parameters;
            _SS = Factory_Indicator.createSlowStochastic(Parameters.Fast_K, Parameters.Slow_K, Parameters.Slow_D, price);
            _RSI = Factory_Indicator.createRSI(Parameters.RSI, price);

            foreach (Rsi r in _RSI)
            {
                VariableIndicator v = new VariableIndicator()
                {
                    TimeStamp = r.TimeStamp,
                    Price_Close = r.Price_Close,
                    Price_High = r.Price_High,
                    Price_Low = r.Price_Low,
                    Price_Open = r.Price_Open,
                    Value = r.RSI,
                };
                _rsi_to_Var.Add(v);
            }

            _RSI_MA = Factory_Indicator.createEMA(Parameters.RSI_MA, _rsi_to_Var);
            _RSI_MA2 = Factory_Indicator.createSMA(Parameters.RSI_MA2, _rsi_to_Var);
            //  foreach (SMA s in _RSI_MA2) Debug.WriteLine(s.Timestamp + " " + s.Price_Close + " RSI " + s.CustomValue + "  MA2 " + s.Sma);                       
            DateTime sd = getStartDate();
            //testDate();
            CutToSize(sd);
            TradeStrategy _strategy = new TradeStrategy(price, Parameters, _SS[0].TimeStamp, CalcTriggers);
           
            _T = _strategy.getStrategyList();
            // for (int x = 0; x < _T.Count; x++) DP(x);
            _strategy.ClearList();

            _SS.Clear();
            _RSI.Clear();
            _rsi_to_Var.Clear();
            _RSI_MA.Clear();
            _RSI_MA2.Clear();
            _T.Clear();



           

        }

        private static DateTime getStartDate()
        {
            List<DateTime> dt = new List<DateTime>();
            DateTime c = new DateTime();

            foreach (var s in _SS)
            {
                foreach (var r in _RSI)
                {
                    foreach (var rm2 in _RSI_MA2)
                    {
                        foreach (var rm in _RSI_MA)
                        {
                            if (rm.TimeStamp == rm2.TimeStamp)
                                c = rm.TimeStamp;
                            break;
                        }
                        if (rm2.TimeStamp == r.TimeStamp)
                            c = rm2.TimeStamp;
                    }
                    if (r.TimeStamp == c) break;
                }

                //    Debug.WriteLine("Loop " + c);

                break;
            }


            return c;

        }

        private static void testDate()
        {
            var d = (from s in _SS
                     from r in _RSI
                     from ra in _RSI_MA
                     from ra2 in _RSI_MA2
                     where s.TimeStamp == r.TimeStamp && r.TimeStamp == ra.TimeStamp && r.TimeStamp == ra2.TimeStamp
                     select s.TimeStamp).First();


            Debug.WriteLine("Linq " + d);
        }

        private static void CutToSize(DateTime startDate)
        {
            int del = -1;
            for (int x = 0; x < _RSI.Count; x++) if (_RSI[x].TimeStamp < startDate) del++;
            for (int x = 0; x <= del; x++) _RSI.RemoveAt(0);
            del = -1;
            for (int x = 0; x < _SS.Count; x++) if (_SS[x].TimeStamp < startDate) del++;
            for (int x = 0; x <= del; x++) _SS.RemoveAt(0);
            del = -1;
            for (int x = 0; x < _RSI_MA.Count; x++) if (_RSI_MA[x].TimeStamp < startDate) del++;
            for (int x = 0; x <= del; x++) _RSI_MA.RemoveAt(0);
            del = -1;
            for (int x = 0; x < _RSI_MA2.Count; x++) if (_RSI_MA2[x].TimeStamp < startDate) del++;
            for (int x = 0; x <= del; x++) _RSI_MA2.RemoveAt(0);


            // Debug.WriteLine("First " + _RSI[0].Timestamp + "  " + _RSI_MA[0].Timestamp + "  " + _SS[0].Timestamp + " " + _RSI_MA2[0].Timestamp);

        }

        private static void DP(int x)
        {

            if (true)//(_T[x].Timestamp.Month == 9 && _T[x].Timestamp.Day > 18)
            {
                //if(_T[x].ActualTrade!=TradeStrategy.Trigger.None)              

                Debug.WriteLine(_T[x].TimeStamp + " SS " + Math.Round(_SS[x].D, 2) + "  RSI " + Math.Round(_RSI[x].RSI, 2) + "  RsiMA " + Math.Round(_RSI_MA[x].Ema, 2) +
                    "  RsiMA2 " + Math.Round(_RSI_MA2[x].Sma, 2) +
                    "  Close: " + _T[x].Price_Close +
                    "  Actual " + _T[x].ActualTrade);

            }
        }

        public static void CalcTriggers(List<TradeStrategy> strategy, int x)
        {
            if (_SS[x - 1].D < _p.Open_80 && _SS[x].D > _p.Open_80
                || (_RSI_MA2[x].Sma > _p.RSI_MidLine_Long && _RSI_MA[x - 1].Ema < _RSI_MA2[x - 1].Sma && _RSI_MA[x].Ema > _RSI_MA2[x].Sma)
                )
                strategy[x].TradeTrigger = Trade.Trigger.OpenLong;



            if (_SS[x - 1].D > _p.Open_20 && _SS[x].D < _p.Open_20
                || (_RSI_MA2[x].Sma < _p.RSI_MidLine_Short && _RSI_MA[x - 1].Ema > _RSI_MA2[x - 1].Sma && _RSI_MA[x].Ema < _RSI_MA2[x].Sma)
                )
                strategy[x].TradeTrigger = Trade.Trigger.OpenShort;




            if (_SS[x - 1].D > _p.Close_80 && _SS[x].D < _p.Close_80
               || (_RSI_MA2[x].Sma > _p.RSI_CloseLong && _RSI_MA[x - 1].Ema > _RSI_MA2[x - 1].Sma && _RSI_MA[x].Ema < _RSI_MA2[x].Sma)
              )
                strategy[x].TradeTrigger = Trade.Trigger.CloseLong;



            if (_SS[x - 1].D < _p.Close_20 && _SS[x].D > _p.Close_20
                  || (_RSI_MA2[x].Sma < _p.RSI_CloseShort && _RSI_MA[x - 1].Ema < _RSI_MA2[x - 1].Sma && _RSI_MA[x].Ema > _RSI_MA2[x].Sma)
                )
                strategy[x].TradeTrigger = Trade.Trigger.CloseShort;

        }


    }
}
