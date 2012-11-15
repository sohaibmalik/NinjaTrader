using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AlsiUtils.Strategies
{
    public class Strategy_SSPOP
    {
        private static Parameter_SlowStoch _p;
        private static List<SlowStoch> _SS;
        private static List<TradeStrategy> _T;
        public static void SsPopStrategy(Strategies.Parameter_SlowStoch Parameters, List<Price> price)
        {
            _p = Parameters;
            _SS = Factory_Indicator.createSlowStochastic(Parameters.Fast_K, Parameters.Slow_K, Parameters.Slow_D, price);
            TradeStrategy _strategy = new TradeStrategy(price, Parameters, _SS[0].Timestamp, CalcTriggers);

            _strategy.Calculate();
            _T = _strategy.getStrategyList();
           // for (int x = 0; x < _T.Count; x++) DP(x);
            //_strategy.ClearList();
            test();
        }


        private static void DP(int x)
        {
           
            if(_T[x].Timestamp.Month == 9 && _T[x].Timestamp.Day >18)
            {
                //if(_T[x].ActualTrade!=TradeStrategy.Trigger.None)
                Debug.WriteLine(_T[x].Timestamp + "  " + _SS[x].D + "  Close: " + _T[x].Price_Close + "   Traded:" + _T[x].TradedPrice + "   actual:" + _T[x].ActualTrade + "   position:" + _T[x].Position + "    Prof " + _T[x].RunningProfit + "  Reason: " + _T[x].Reason + "  trigger: " + _T[x].TradeTrigger + "  A* " + _T[x].markedObjectA + "  B* " + _T[x].markedObjectB + "  Dir " + _T[x].TradeDirection + "   Prof : " + _T[x].TotalProfit + "--------------------------------------------------- " + _T[x].InstrumentName);
            }
        }

        public static void CalcTriggers(List<TradeStrategy> strategy, int x)
        {


            if (_SS[x - 1].D < _p.Open_80 && _SS[x].D > _p.Open_80)
                strategy[x].TradeTrigger = TradeStrategy.Trigger.OpenLong;

            if (_SS[x - 1].D > _p.Close_80 && _SS[x].D < _p.Close_80)
                strategy[x].TradeTrigger = TradeStrategy.Trigger.CloseLong;

            if (_SS[x - 1].D > _p.Open_20 && _SS[x].D < _p.Open_20)
                strategy[x].TradeTrigger = TradeStrategy.Trigger.OpenShort;

            if (_SS[x - 1].D < _p.Close_20 && _SS[x].D > _p.Close_20)
                strategy[x].TradeTrigger = TradeStrategy.Trigger.CloseShort;




        }


        private static void test()
        {
            var overnightpos = from ovn in _T
                               where ovn.Timestamp.Hour == 17 && ovn.Timestamp.Minute == 20 && ovn.Position
                               select ovn;


            double Oloss_Closs = 0;
            double Oprof_Cprof = 0;
            double Oloss_Cprof = 0;
            double Oprof_Closs = 0;
            double N_Closs = 0;
            double N_Prof = 0;
            double Diff = 0;

            int count = 0;
            foreach (var v in overnightpos)
            {
                count++;
                var closepos = (from t in _T
                                where t.Timestamp > v.Timestamp && !t.Position
                                select t).First();


                if (v.RunningProfit < 0 && closepos.RunningProfit < v.RunningProfit) Oloss_Closs++;
                if (v.RunningProfit > 0 && closepos.RunningProfit < v.RunningProfit) Oprof_Closs++;
                if (v.RunningProfit > 0 && closepos.RunningProfit > v.RunningProfit) Oprof_Cprof++;
                if (v.RunningProfit < 0 && closepos.RunningProfit > v.RunningProfit) Oloss_Cprof++;
                if (v.RunningProfit == 0 && closepos.RunningProfit > 0) N_Prof++;
                if (v.RunningProfit == 0 && closepos.RunningProfit < 0) N_Closs++;

                double diff = closepos.RunningProfit - v.RunningProfit;

                // if (v.RunningProfit < 0) ;
                //else
                Diff += diff;

                Debug.WriteLine("Overnight " + v.Timestamp + "  Pos:" + v.TradeDirection + "  " + v.Price_Close + "      " + v.RunningProfit);
                Debug.WriteLine("Close " + closepos.Timestamp + "  Pos:" + closepos.TradeDirection + "  " + closepos.Price_Close + "      " + closepos.RunningProfit);
                Debug.WriteLine(count + " -------------------------------------------------");
                Debug.WriteLine("OL CL " + Oloss_Closs);
                Debug.WriteLine("OL CP " + Oloss_Cprof);
                Debug.WriteLine("OP CL " + Oprof_Closs);
                Debug.WriteLine("OP CP " + Oprof_Cprof);
                Debug.WriteLine("N CL " + N_Closs);
                Debug.WriteLine("N CP " + N_Prof);
                Debug.WriteLine("Diff This Trade " + diff);
                Debug.WriteLine("Diff " + Diff);
                Debug.WriteLine("--------------------------------------------------------");
            }


            Debug.WriteLine("============================================================");
            Debug.WriteLine("Vovernight Count : " + overnightpos.Count());
            Debug.WriteLine("OL CL " + Oloss_Closs);
            Debug.WriteLine("OL CP " + Oloss_Cprof);
            Debug.WriteLine("OP CL " + Oprof_Closs);
            Debug.WriteLine("OP CP " + Oprof_Cprof);
            Debug.WriteLine("N CL " + N_Closs);
            Debug.WriteLine("N CP " + N_Prof);
            Debug.WriteLine("Diff " + Diff);
        }

    }
}
