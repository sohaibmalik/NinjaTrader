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

        public static List<Trade> EmaScalp(Parameter_EMA_Scalp P, List<Price> price,bool tradeOnly)
        {


            A_1 = Factory_Indicator.createEMA(P.A_EMA1, price);
            A_6 = Factory_Indicator.createEMA(P.A_EMA2, price);

            B_1 = Factory_Indicator.createEMA(P.B_EMA1, price);
            B_6 = Factory_Indicator.createEMA(P.B_EMA2, price);

            E1 = Factory_Indicator.createEMA(P.C_EMA, price);

            DateTime sd = E1[0].Timestamp;
            CutToSize(sd);
            TradeStrategy _strategy = new TradeStrategy(price, P, B_6[0].Timestamp, CalcTriggers);
            SumStats s = _strategy.Calculate();
            _T = _strategy.getStrategyList();
            for (int x = 0; x < _T.Count; x++) DP(x);

            //_strategy.ClearList(); //FOR SIMULATOR

            return GetTradeData(tradeOnly); // REAL TRADING

           // Clear();//FOR SIMULATOR



            //return s;
            if (true)//s.Total_Avg_PL >15 || s.Total_Avg_PL <-15)
            {

                Debug.WriteLine(P.A_EMA1 + "  " + P.A_EMA2 + "  " + P.B_EMA1 + "  " + P.B_EMA2 + "   " + P.C_EMA + " tp:" + P.TakeProfit);
                Debug.WriteLine("Trades " + s.TradeCount);
                Debug.WriteLine("Total " + s.TotalProfit);
                Debug.WriteLine("Avg " + s.Total_Avg_PL);
                Debug.WriteLine("Win % " + s.Pct_Prof);
                Debug.WriteLine("Loss % " + s.Pct_Loss);
                Debug.WriteLine("EOF Close " + P.CloseEndofDay);
                Debug.WriteLine("Period " + P.Period);
                Debug.WriteLine("==========================================");

                SimDBDataContext dc = new SimDBDataContext();
                tbl5Min n = new tbl5Min
                {
                    Trades = (int)s.TradeCount,
                    TotalPL = (int)s.TotalProfit,
                    Win = s.Pct_Prof,
                    Loose = s.Pct_Loss,
                    AvgPL = s.Total_Avg_PL,
                    E_A1 = P.A_EMA1,
                    E_A2 = P.A_EMA2,
                    E_B1 = P.B_EMA1,
                    E_B2 = P.B_EMA2,
                    E_C = P.C_EMA,
                    CloseEndDay = P.CloseEndofDay.ToString(),

                };
                // dc.tbl5Mins.InsertOnSubmit(n);
                // dc.SubmitChanges();
            }

           

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
            for (int x = 0; x < A_1.Count; x++) if (A_1[x].Timestamp < startDate) del++;
            for (int x = 0; x <= del; x++) A_1.RemoveAt(0);
            del = -1;
            for (int x = 0; x < A_6.Count; x++) if (A_6[x].Timestamp < startDate) del++;
            for (int x = 0; x <= del; x++) A_6.RemoveAt(0);

            del = -1;
            for (int x = 0; x < B_1.Count; x++) if (B_1[x].Timestamp < startDate) del++;
            for (int x = 0; x <= del; x++) B_1.RemoveAt(0);
            del = -1;
            for (int x = 0; x < B_6.Count; x++) if (B_6[x].Timestamp < startDate) del++;
            for (int x = 0; x <= del; x++) B_6.RemoveAt(0);
        }

        private static void DP(int x)
        {

            if (false)//_T[x].ActualTrade != Trade.Trigger.None)
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
                     + "  RunPL:," + _T[x].RunningProfit
                     + "," + _T[x].TotalProfit
                     + "," + _T[x].Reason
                    );


            }
        }

        public static List<Trade> GetTradeData(bool TradesOnly)
        {
            List<Trade> trades = new List<Trade>();
            foreach (var v in _T)
            {
                var t = new Trade()
                {
                    InstrumentName = v.InstrumentName,
                    BuyorSell = TradeStrategy.GetBuySell(v.ActualTrade),
                    CurrentDirection = v.TradeDirection,
                    Position = v.Position,
                    RunningProfit = v.RunningProfit,
                    TimeStamp = v.Timestamp,
                    TotalPL = v.TotalProfit,
                    TradedPrice = v.TradedPrice,
                    Reason =v.ActualTrade,
                    CurrentPrice = v.Price_Close,
                    
                };

                if (TradesOnly)
                {
                    if (v.ActualTrade != Trade.Trigger.None) trades.Add(t);
                }
                else
                {
                    trades.Add(t);
                }
            }

            Clear();
            return trades;
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

    }
}