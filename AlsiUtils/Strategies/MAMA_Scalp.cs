using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace AlsiUtils.Strategies
{
   

    public class MAMA_Scalp
    {
        private static List<TradeStrategy> _T;
        private static List<EMA> A_1;
        private static List<EMA> A_6;
        private static List<EMA> B_1;
        private static List<EMA> B_6;
        private static List<EMA> E1;
        private static List<MAMA> MA;

        public static List<Trade> MAMAScalp(Parameter_MAMA P, List<Price> price, bool tradeOnly)
        {

            A_1 = Factory_Indicator.createEMA(P.A_EMA1, price);
            A_6 = Factory_Indicator.createEMA(P.A_EMA2, price);
            B_1 = Factory_Indicator.createEMA(P.B_EMA1, price);
            B_6 = Factory_Indicator.createEMA(P.B_EMA2, price);
            E1 = Factory_Indicator.createEMA(P.C_EMA, price);

            MA = Factory_Indicator.createAdaptiveMA_MAMA(P.Fast, P.Slow, price);

            //StreamWriter sr = new StreamWriter(@"d:\MAMA.txt");
            //foreach (var m in MA.Skip(1000))
            //{
            //    sr.WriteLine(m.TimeStamp + "," + m.Price_Close + "," + m.Mama + "," + m.Fama);//+","+E1.Where(z=>z.TimeStamp==m.TimeStamp).First().Ema );
            //}
            //sr.Close();

            //Environment.Exit(0);

            DateTime sd = E1[0].TimeStamp;
            
            CutToSize(sd);
            TradeStrategy _strategy = new TradeStrategy(price, P, B_6[0].TimeStamp, CalcTriggers,CalcTriggers2 );

            _strategy.Calculate();
            _T = _strategy.getStrategyList();
            //for (int x = 0; x < _T.Count; x++) DP(x);


            // _strategy.ClearList(); //FOR SIMULATOR
            return GetTradeData(tradeOnly); // REAL TRADING
            //  Clear();//FOR SIMULATOR

            return new List<Trade>();

        }





        private static void Clear()
        {
            A_1.Clear();
            A_6.Clear();

            B_1.Clear();
            B_6.Clear();

            E1.Clear();
            _T.Clear();
        }


        private static void CutToSize(DateTime startDate)
        {
            int del = -1;
            for (int x = 0; x < A_1.Count; x++) if (A_1[x].TimeStamp < startDate) del++;
            for (int x = 0; x <= del; x++) A_1.RemoveAt(0);
            del = -1;
            for (int x = 0; x < A_6.Count; x++) if (A_6[x].TimeStamp < startDate) del++;
            for (int x = 0; x <= del; x++) A_6.RemoveAt(0);

            del = -1;
            for (int x = 0; x < B_1.Count; x++) if (B_1[x].TimeStamp < startDate) del++;
            for (int x = 0; x <= del; x++) B_1.RemoveAt(0);
            del = -1;
            for (int x = 0; x < B_6.Count; x++) if (B_6[x].TimeStamp < startDate) del++;
            for (int x = 0; x <= del; x++) B_6.RemoveAt(0);
        }

        private static void DP(int x)
        {

            if (true)//_T[x].ActualTrade != Trade.Trigger.None)
            {

                Debug.WriteLine(_T[x].TimeStamp
                     + "," + _T[x].ActualTrade 
                     + "," + _T[x].Price_Close
                     + ", E:" + Math.Round(E1[x].Ema, 2)
                     + "  A1:" + Math.Round(A_1[x].Ema, 2)
                     + "  A6:" + Math.Round(A_6[x].Ema, 2)
                     + "  B1:" + Math.Round(B_1[x].Ema, 2)
                     + "  B6:" + Math.Round(B_6[x].Ema, 2)
                     + " Pos:" + _T[x].TradeDirection 
                     + "  RunPL:," + _T[x].RunningProfit
                     + "," + _T[x].TotalProfit
                     + "," + _T[x].Reason
                    );


            }
        }

        public static List<Trade> GetTradeData(bool TradesOnly)
        {
            List<Trade> trades = new List<Trade>();
            for (int x = 0; x < _T.Count; x++)
            {

                var t = new Trade()
                {
                    InstrumentName = _T[x].InstrumentName,
                    BuyorSell = TradeStrategy.GetBuySell(_T[x].ActualTrade),
                    CurrentDirection = _T[x].TradeDirection,
                    Position = _T[x].Position,
                    RunningProfit = _T[x].RunningProfit,
                    TimeStamp = _T[x].TimeStamp,
                    TotalPL = _T[x].TotalProfit,
                    TradedPrice = _T[x].Price_Close,
                    Reason = _T[x].ActualTrade,
                    CurrentPrice = _T[x].Price_Close,
                    TradeVolume=GetVolume(_T[x]),
                    IndicatorNotes = "A1:" + Math.Round(A_1[x].Ema,2) + "  A2:" + Math.Round(A_6[x].Ema,2) + "  B1:" + Math.Round(B_1[x].Ema,2) + "  B2:" + Math.Round(B_6[x].Ema,2) + "  C:" + Math.Round(E1[x].Ema,2)

                };

                if (TradesOnly)
                {
                    if (_T[x].ActualTrade != Trade.Trigger.None) trades.Add(t);
                }
                else
                {
                    trades.Add(t);
                }
              
               // Debug.WriteLine(t.TimeStamp + "," + t.BuyorSell + "," + t.CurrentDirection + "," + _T[x].TradeTrigger + ","  + _T[x].ActualTrade  + "," + _T[x].TradedPrice );
            }

            Clear();
            return trades;
        }

        private static int GetVolume(TradeStrategy T)
        {
            int vol = 0;
            if (T.ActualTrade != Trade.Trigger.None) vol = 1;
            return vol;
        }


        public static void CalcTriggers(List<TradeStrategy> strategy, int x)
        {


            if (
                (A_1[x - 1].Ema > B_1[x - 1].Ema && A_1[x].Ema < B_1[x].Ema)
                )
                strategy[x].TradeTrigger = Trade.Trigger.CloseLong;

            if (
                (A_1[x - 1].Ema < B_1[x - 1].Ema && A_1[x].Ema > B_1[x].Ema)
                )
                strategy[x].TradeTrigger = Trade.Trigger.CloseShort;


            if (
                (A_6[x - 1].Ema < E1[x - 1].Ema && A_6[x].Ema > B_1[x].Ema && A_6[x].Ema > E1[x].Ema)
                || (A_6[x - 1].Ema < B_1[x - 1].Ema && A_6[x].Ema > B_1[x].Ema && A_6[x].Ema > E1[x].Ema)

                )
                strategy[x].TradeTrigger = Trade.Trigger.OpenLong;

            if (
                (A_6[x - 1].Ema > E1[x - 1].Ema && A_6[x].Ema < E1[x].Ema && A_6[x].Ema < B_6[x].Ema)
                  || (A_6[x - 1].Ema > B_1[x - 1].Ema && A_6[x].Ema < B_1[x].Ema && A_6[x].Ema < B_6[x].Ema)
                )
                strategy[x].TradeTrigger = Trade.Trigger.OpenShort;


          

        }

        public static void CalcTriggers2(List<TradeStrategy> strategy, int x)
        {
            if (strategy[x].TradeDirection == Trade.Direction.Long && strategy[x].TradeTrigger == Trade.Trigger.OpenShort)
                strategy[x].TradeTrigger = Trade.Trigger.ReverseShort;

            if (strategy[x].TradeDirection == Trade.Direction.Short && strategy[x].TradeTrigger == Trade.Trigger.OpenLong)
                strategy[x].TradeTrigger = Trade.Trigger.ReverseLong;
        }
     

    }
}