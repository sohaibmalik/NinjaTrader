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
            TradeStrategy _strategy = new TradeStrategy(price, Parameters, _SS[0].TimeStamp, CalcTriggers);

            _strategy.Calculate();
            _T = _strategy.getStrategyList();
           // for (int x = 0; x < _T.Count; x++) DP(x);
            //_strategy.ClearList();
            
        }


        private static void DP(int x)
        {
           
            if(_T[x].TimeStamp.Month == 9 && _T[x].TimeStamp.Day >18)
            {
                //if(_T[x].ActualTrade!=TradeStrategy.Trigger.None)
                Debug.WriteLine(_T[x].TimeStamp + "  " + _SS[x].D + "  Close: " + _T[x].Price_Close + "   Traded:" + _T[x].TradedPrice + "   actual:" + _T[x].ActualTrade + "   position:" + _T[x].Position + "    Prof " + _T[x].RunningProfit + "  Reason: " + _T[x].Reason + "  trigger: " + _T[x].TradeTrigger + "  A* " + _T[x].markedObjectA + "  B* " + _T[x].markedObjectB + "  Dir " + _T[x].TradeDirection + "   Prof : " + _T[x].TotalProfit + "--------------------------------------------------- " + _T[x].InstrumentName);
            }
        }

        public static void CalcTriggers(List<TradeStrategy> strategy, int x)
        {


            if (_SS[x - 1].D < _p.Open_80 && _SS[x].D > _p.Open_80)
                strategy[x].TradeTrigger = Trade.Trigger.OpenLong;

            if (_SS[x - 1].D > _p.Close_80 && _SS[x].D < _p.Close_80)
                strategy[x].TradeTrigger = Trade.Trigger.CloseLong;

            if (_SS[x - 1].D > _p.Open_20 && _SS[x].D < _p.Open_20)
                strategy[x].TradeTrigger = Trade.Trigger.OpenShort;

            if (_SS[x - 1].D < _p.Close_20 && _SS[x].D > _p.Close_20)
                strategy[x].TradeTrigger = Trade.Trigger.CloseShort;




        }


      

    }
}
