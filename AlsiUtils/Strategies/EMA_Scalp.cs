using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlsiUtils.Indicators;
using System.Diagnostics;

namespace AlsiUtils.Strategies
{
    public static class EMA_Scalp
    {
        private static List<TradeStrategy> _T;

        private static List<EMA> A_1;

        private static List<EMA> A_6;

        private static List<EMA> B_1;

        private static List<EMA> B_6;

        private static List<EMA> E1;

        public static void EmaScalp(Parameter_EMA_Scalp P, List<Price> price)
        {


            A_1 = Factory_Indicator.createEMA(P.A_EMA1, price);
            // A_2 = Factory_Indicator.createEMA(P.A_EMA2, price);
            // A_3 = Factory_Indicator.createEMA(P.A_EMA3, price);
            // A_4 = Factory_Indicator.createEMA(P.A_EMA4, price);
            // A_5 = Factory_Indicator.createEMA(P.A_EMA5, price);
            A_6 = Factory_Indicator.createEMA(P.A_EMA6, price);

            B_1 = Factory_Indicator.createEMA(P.B_EMA1, price);
            // B_2 = Factory_Indicator.createEMA(P.B_EMA2, price);
            // B_3 = Factory_Indicator.createEMA(P.B_EMA3, price);
            // B_4 = Factory_Indicator.createEMA(P.B_EMA4, price);
            // B_5 = Factory_Indicator.createEMA(P.B_EMA5, price);
            B_6 = Factory_Indicator.createEMA(P.B_EMA6, price);

            E1 = Factory_Indicator.createEMA(P.C_EMA, price);


            DateTime sd = E1[0].Timestamp;
            CutToSize(sd);
            TradeStrategy _strategy = new TradeStrategy(price, P, B_6[0].Timestamp, CalcTriggers);
            SumStats s = _strategy.Calculate();
            _T = _strategy.getStrategyList();
            for (int x = 0; x < _T.Count; x++) DP(x);
            _strategy.ClearList();

            A_1.Clear();
            /* A_2.Clear();
             A_3.Clear();
             A_4.Clear();
             A_5.Clear();
             */
            A_6.Clear();

            B_1.Clear();
            /* B_2.Clear();
             B_3.Clear();
             B_4.Clear();
             B_5.Clear();
             */
            B_6.Clear();

            E1.Clear();
            _T.Clear();



            //return s;
            if (true)//(s.Total_Avg_PL > 21)
            {
               
                Debug.WriteLine(P.A_EMA1 + "  " + P.A_EMA6 + "  " + P.B_EMA1 + "  " + P.B_EMA6 + "   " + P.C_EMA + " tp:" + P.TakeProfit);
                Debug.WriteLine("Trades " + s.TradeCount);
                Debug.WriteLine("Total " + s.TotalProfit);
                Debug.WriteLine("Avg " + s.Total_Avg_PL);
                Debug.WriteLine("Win % " + s.Pct_Prof);
                Debug.WriteLine("Loss % " + s.Pct_Loss);
                Debug.WriteLine("==========================================");
            }

        }


        private static void CutToSize(DateTime startDate)
        {
            int del = -1;
            for (int x = 0; x < A_1.Count; x++) if (A_1[x].Timestamp < startDate) del++;
            for (int x = 0; x <= del; x++) A_1.RemoveAt(0);
            /* del = -1;
             for (int x = 0; x < A_2.Count; x++) if (A_2[x].Timestamp < startDate) del++;
             for (int x = 0; x <= del; x++) A_2.RemoveAt(0);
             del = -1;
             for (int x = 0; x < A_3.Count; x++) if (A_3[x].Timestamp < startDate) del++;
             for (int x = 0; x <= del; x++) A_3.RemoveAt(0);
             del = -1;
             for (int x = 0; x < A_4.Count; x++) if (A_4[x].Timestamp < startDate) del++;
             for (int x = 0; x <= del; x++) A_4.RemoveAt(0);
             del = -1;
             for (int x = 0; x < A_5.Count; x++) if (A_5[x].Timestamp < startDate) del++;
             for (int x = 0; x <= del; x++) A_5.RemoveAt(0);
             * */
            del = -1;
            for (int x = 0; x < A_6.Count; x++) if (A_6[x].Timestamp < startDate) del++;
            for (int x = 0; x <= del; x++) A_6.RemoveAt(0);

            del = -1;
            for (int x = 0; x < B_1.Count; x++) if (B_1[x].Timestamp < startDate) del++;
            for (int x = 0; x <= del; x++) B_1.RemoveAt(0);
            /* del = -1;
             for (int x = 0; x < B_2.Count; x++) if (B_2[x].Timestamp < startDate) del++;
             for (int x = 0; x <= del; x++) B_2.RemoveAt(0);
             del = -1;
             for (int x = 0; x < B_3.Count; x++) if (B_3[x].Timestamp < startDate) del++;
             for (int x = 0; x <= del; x++) B_3.RemoveAt(0);
             del = -1;
             for (int x = 0; x < B_4.Count; x++) if (B_4[x].Timestamp < startDate) del++;
             for (int x = 0; x <= del; x++) B_4.RemoveAt(0);
             del = -1;
             for (int x = 0; x < B_5.Count; x++) if (B_5[x].Timestamp < startDate) del++;
             for (int x = 0; x <= del; x++) B_5.RemoveAt(0);
             * */
            del = -1;
            for (int x = 0; x < B_6.Count; x++) if (B_6[x].Timestamp < startDate) del++;
            for (int x = 0; x <= del; x++) B_6.RemoveAt(0);

            // Debug.WriteLine("First " + A_1[0].Timestamp + "  " + A_6[0].Timestamp + "  " + B_1[0].Timestamp + " " + B_6[0].Timestamp + " " + E1[0].Timestamp);

        }

        private static void DP(int x)
        {

            if (false)//_T[x].ActualTrade != TradeStrategy.Trigger.None)
            {

                Debug.WriteLine(_T[x].Timestamp
                     + "," + _T[x].ActualTrade
                     + "," + _T[x].Price_Close
                     + ", E:" + Math.Round(E1[x].Ema, 2)
                     + "  A1:" + Math.Round(A_1[x].Ema, 2)
                     + "  A6:" + Math.Round(A_6[x].Ema, 2)
                     + "  B1:" + Math.Round(B_1[x].Ema, 2)
                     + "  B6:" + Math.Round(B_6[x].Ema, 2)
                     + " Pos:" + _T[x].Position
                     + "  RunPL:" + _T[x].RunningProfit
                     + "," + _T[x].TotalProfit
                     + "," + _T[x].Reason
                    );


            }
        }

        public static void CalcTriggers(List<TradeStrategy> strategy, int x)
        {


            if (
                (A_1[x - 1].Ema > B_1[x - 1].Ema && A_1[x].Ema < B_1[x].Ema)
                /*|| (A_1[x - 1].Price_Low > E1[x - 1].Ema && A_1[x].Price_Low < E1[x].Ema)
                 * */
                )
                strategy[x].TradeTrigger = TradeStrategy.Trigger.CloseLong;

            if (
                (A_1[x - 1].Ema < B_1[x - 1].Ema && A_1[x].Ema > B_1[x].Ema)
                /*|| (A_1[x - 1].Price_High < E1[x - 1].Ema && A_1[x].Price_High > E1[x].Ema)
                 */
                )
                strategy[x].TradeTrigger = TradeStrategy.Trigger.CloseShort;


            if (
                (A_6[x - 1].Ema < E1[x - 1].Ema && A_6[x].Ema > B_1[x].Ema && A_6[x].Ema > E1[x].Ema)
                || (A_6[x - 1].Ema < B_1[x - 1].Ema && A_6[x].Ema > B_1[x].Ema && A_6[x].Ema > E1[x].Ema)
              /*  || (A_1[x - 1].Price_Low < E1[x - 1].Ema && A_1[x].Price_Low > E1[x].Ema)
                && (A_6[x].Ema > E1[x].Ema)
                */
                )
                strategy[x].TradeTrigger = TradeStrategy.Trigger.OpenLong;

            if (
                (A_6[x - 1].Ema > E1[x - 1].Ema && A_6[x].Ema < E1[x].Ema && A_6[x].Ema < B_6[x].Ema)
                  ||( A_6[x - 1].Ema > B_1[x - 1].Ema && A_6[x].Ema < B_1[x].Ema && A_6[x].Ema < B_6[x].Ema)
                  /* || (A_1[x - 1].Price_High > E1[x - 1].Ema && A_1[x].Price_High < E1[x].Ema)
                &&(A_6[x].Ema < E1[x].Ema)
                */
                )
                strategy[x].TradeTrigger = TradeStrategy.Trigger.OpenShort;


        }

    }
}