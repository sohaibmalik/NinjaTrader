using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AlsiUtils.Strategies;
using TicTacTec.TA.Library;

namespace AlsiUtils.Strategies
{
    public static class Strategy_Bollinger
    {
        private static Parameter_Bollinger _p;
        private static List<BollingerBand> _BB;
        private static List<TradeStrategy> _T;

        public static void BollingerStrategy(Strategies.Parameter_Bollinger Parameters, List<Price> price)
        {
            _p = Parameters;
            Parameter_General pg = new Parameter_General();
            pg.StopLoss = -250;
            pg.TakeProfit = 250;

            _BB = Factory_Indicator.createBollingerBand(_p.N, _p.P, price, Core.MAType.Sma);
            //TradeStrategy _strategy = new TradeStrategy(price, pg, _BB[0].Timestamp, CalcTriggers);

           // _strategy.Calculate();
           // _T = _strategy.getStrategyList();
           // for (int x = 0; x < _T.Count; x++) DP(x);
           // _strategy.ClearList();
        }


        private static void DP(int x)
        {
            Debug.WriteLine(_T[x].Timestamp + "  MID " + _BB[x].Mid + "  Close: " + _T[x].Price_Close + "   Traded:" + _T[x].TradedPrice + "   actual:" + _T[x].ActualTrade + "   position:" + _T[x].Position + "  Prof " + _T[x].RunningProfit + "   " + _T[x].Reason + "  trigger: " + _T[x].TradeTrigger + "  * " + _T[x].markedObjectB + "  Dir " + _T[x].TradeDirection);
        }
        public static void CalcTriggers(List<TradeStrategy> strategy, int x)
        {

            //if (_SS[x - 1].D < _p.Open_80 && _SS[x].D > _p.Open_80)
            //    strategy[x].TradeTrigger = TradeStrategy.Trigger.OpenLong;

            //if (_SS[x - 1].D > _p.Close_80 && _SS[x].D < _p.Close_80)
            //    strategy[x].TradeTrigger = TradeStrategy.Trigger.CloseLong;

            //if (_SS[x - 1].D > _p.Open_20 && _SS[x].D < _p.Open_20)
            //    strategy[x].TradeTrigger = TradeStrategy.Trigger.OpenShort;

            //if (_SS[x - 1].D < _p.Close_20 && _SS[x].D > _p.Close_20)
            //    strategy[x].TradeTrigger = TradeStrategy.Trigger.CloseShort;

            if (_BB[x - 1].Price_Close > _BB[x - 1].Lower && _BB[x].Price_Close < _BB[x].Lower) strategy[x].TradeTrigger = Trade.Trigger.OpenShort;
            if (_BB[x - 1].Price_Close < _BB[x - 1].Upper && _BB[x].Price_Close > _BB[x].Upper) strategy[x].TradeTrigger = Trade.Trigger.OpenLong;

            if (_BB[x - 1].Price_Close > _BB[x - 1].Mid && _BB[x].Price_Close < _BB[x].Mid) strategy[x].TradeTrigger = Trade.Trigger.CloseLong;
            if (_BB[x - 1].Price_Close < _BB[x - 1].Mid && _BB[x].Price_Close > _BB[x].Mid) strategy[x].TradeTrigger = Trade.Trigger.CloseShort;

        }

    }
}
