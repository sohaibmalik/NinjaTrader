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
                    Timestamp = r.Timestamp,
                    Price_Close = r.Price_Close,
                    Price_High = r.Price_High,
                    Price_Low = r.Price_Low,
                    Price_Open = r.Price_Open,
                    Value = r.RSI,
                };
                _rsi_to_Var.Add(v);
            }

            _RSI_MA = Factory_Indicator.createEMA(Parameters.RSI_MA, _rsi_to_Var);

            // foreach (EMA s in _RSI_MA) Debug.WriteLine(s.Timestamp + " " + s.Price_Close + " RSI " + s.CustomValue + "  MA " + s.Ema);                       
            DateTime sd = getStartDate();
           // testDate();
            CutToSize(sd);
            TradeStrategy _strategy = new TradeStrategy(price, Parameters, _SS[0].Timestamp, CalcTriggers);

            _strategy.Calculate();
            _T = _strategy.getStrategyList();
             //for (int x = 0; x < _T.Count; x++) DP(x);
            //_strategy.ClearList();

        }

        private static DateTime getStartDate()
        {
            List<DateTime> dt = new List<DateTime>();
            DateTime c = new DateTime();

            foreach (var s in _SS)
            {
                foreach (var r in _RSI)
                {
                    foreach (var rm in _RSI_MA)
                    {
                        if (rm.Timestamp == r.Timestamp)
                            c = rm.Timestamp;
                        break;
                    }

                    if (r.Timestamp == c) break;
                }

                //Debug.WriteLine("Loop " + c);

                break;
            }


            return c;

        }

        private static void  testDate()
        {
            var d = (from s in _SS
                     from r in _RSI
                     from ra in _RSI_MA
                     where s.Timestamp == r.Timestamp && r.Timestamp == ra.Timestamp
                     select s.Timestamp).First();

           // Debug.WriteLine("Linq " + d);
        }

        private static void CutToSize(DateTime startDate)
        {
            int del = -1;
            for (int x = 0; x < _RSI.Count; x++) if (_RSI[x].Timestamp < startDate) del++;
            for (int x = 0; x <= del; x++) _RSI.RemoveAt(0);
            del = -1;
            for (int x = 0; x < _SS.Count; x++) if (_SS[x].Timestamp < startDate) del++;
            for (int x = 0; x <= del; x++) _SS.RemoveAt(0);
            del = -1;
            for (int x = 0; x < _RSI_MA.Count; x++) if (_RSI_MA[x].Timestamp < startDate) del++;
            for (int x = 0; x <= del; x++) _RSI_MA.RemoveAt(0);

           // Debug.WriteLine("First " + _RSI[0].Timestamp + "  " + _RSI_MA[0].Timestamp + "  " + _SS[0].Timestamp);

        }

        private static void DP(int x)
        {

            if (true)//(_T[x].Timestamp.Month == 9 && _T[x].Timestamp.Day > 18)
            {
                //if(_T[x].ActualTrade!=TradeStrategy.Trigger.None)              

                Debug.WriteLine(_T[x].Timestamp + "  " + Math.Round(_SS[x].D, 2) + " RSI " + Math.Round(_RSI[x].RSI, 2) + "  RsiMA " + Math.Round(_RSI_MA[x].Ema, 2) + "  Close: " + _T[x].Price_Close +
                    "  Actual " + _T[x].ActualTrade);

            }
        }

        public static void CalcTriggers(List<TradeStrategy> strategy, int x)
        {
            if (_SS[x - 1].D < _p.Open_80 && _SS[x].D > _p.Open_80
               
                )
                strategy[x].TradeTrigger = TradeStrategy.Trigger.OpenLong;          

            if (_SS[x - 1].D > _p.Open_20 && _SS[x].D < _p.Open_20
                         
                )
                strategy[x].TradeTrigger = TradeStrategy.Trigger.OpenShort;








            if (_SS[x - 1].D > _p.Close_80 && _SS[x].D < _p.Close_80
                 || _RSI[x].RSI > _p.RSI_UpperLine
              )
                strategy[x].TradeTrigger = TradeStrategy.Trigger.CloseLong;



            if (_SS[x - 1].D < _p.Close_20 && _SS[x].D > _p.Close_20
                 ||_RSI[x].RSI < _p.RSI_LowerLine
                )
                strategy[x].TradeTrigger = TradeStrategy.Trigger.CloseShort;

        }


    }
}
