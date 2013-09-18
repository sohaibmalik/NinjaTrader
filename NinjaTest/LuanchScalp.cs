using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using AlsiUtils.Data_Objects;
using AlsiUtils;
using AlsiTrade_Backend;

namespace NinjaTest
{
    public class LuanchScalp
    {
        private static Form3 form;
        private AlsiUtils.Statistics S;
        private static double maxprof = 0;
        private static int takep = 0;
        private static int takel = 0;
        private static EmaSettings ema;
        private AlsiUtils.Strategies.Parameter_EMA_Scalp E = new AlsiUtils.Strategies.Parameter_EMA_Scalp();
        
        public LuanchScalp(Form3 form3,EmaSettings emaSett)
        {
            form = form3;
            S = new AlsiUtils.Statistics();
            S.OnStatsCaculated += new Statistics.StatsCalculated(S_OnStatsCaculated);
            E = new AlsiUtils.Strategies.Parameter_EMA_Scalp();
          ema=emaSett ;
        }

        public LuanchScalp()
        {
           
         
        }
        
        void S_OnStatsCaculated(object sender, Statistics.StatsCalculatedEvent e)
        {
            string position = E.A_EMA1 + "-" + E.A_EMA2 + "     " + E.B_EMA1 + "-" + E.B_EMA2 + "      " + E.C_EMA + "\n" + e.SumStats.Total_Avg_PL;
            form.UpdatePos(position);

            if (e.SumStats.Total_Avg_PL>10 || e.SumStats.Total_Avg_PL<-10)//(e.SumStats.TotalProfit > maxprof)
            {
                //maxprof = e.SumStats.TotalProfit;
                StringBuilder stat = new StringBuilder("");
              

                Debug.WriteLine("============STATS==============");
                Debug.WriteLine("Total PL " + e.SumStats.TotalProfit);
                Debug.WriteLine("# Trades " + e.SumStats.TradeCount);
                Debug.WriteLine("Tot Avg PL " + e.SumStats.Total_Avg_PL);
                Debug.WriteLine("Prof % " + e.SumStats.Pct_Prof);
                Debug.WriteLine("Loss % " + e.SumStats.Pct_Loss);
                Debug.WriteLine("Take P " + takep);
                Debug.WriteLine("Take L " + takel);

                stat.Append("============STATS==============");
                stat.Append("\nTotal PL " + e.SumStats.TotalProfit);
                stat.Append("\n# Trades " + e.SumStats.TradeCount);
                stat.Append("\nTot Avg PL " + e.SumStats.Total_Avg_PL);
                stat.Append("\nProf % " + e.SumStats.Pct_Prof);
                stat.Append("\nLoss % " + e.SumStats.Pct_Loss);
                stat.Append("\nTake P " + takep);
                stat.Append("\nTake L " + takel);

                form.UpdateDisplay(stat.ToString());

                SimDBDataContext dc = new SimDBDataContext();
                tbl2Min tbl = new tbl2Min()
                {
                    Trades = (int)e.SumStats.TradeCount,
                    TotalPL = (int)e.SumStats.TotalProfit,
                    AvgPL = e.SumStats.Total_Avg_PL,
                    Win = e.SumStats.Pct_Prof,
                    Loose = e.SumStats.Pct_Loss,
                    E_A1 = E.A_EMA1,
                    E_A2 = E.A_EMA2,
                    E_B1 = E.B_EMA1,
                    E_B2 = E.B_EMA2,
                    E_C = E.C_EMA,
                    Period = 2012,
                    CloseEndDay = "True",

                };
                  dc.tbl2Mins.InsertOnSubmit(tbl);
                  dc.SubmitChanges();
            }
        }

        public void RunSingle()
        {
            //Laptop
            // string css = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            //PC
            string css = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = css;

            //  DateTime s = new DateTime(2006, 01, 01);
            //  DateTime e = new DateTime(2006, 12, 15);

            DateTime s = new DateTime(2012, 1, 1);
            DateTime e = new DateTime(2013, 12, 29);


            var prices = AlsiUtils.DataBase.readDataFromDataBase(GlobalObjects.TimeInterval.Minute_2, AlsiUtils.DataBase.dataTable.MasterMinute,
               s, e, false);
            Debug.WriteLine("Start Date " + prices[0].TimeStamp);

            //for (int x = 2; x < 50; x++)
            //{

            AlsiUtils.Strategies.Parameter_EMA_Scalp E = new AlsiUtils.Strategies.Parameter_EMA_Scalp()
            {
                A_EMA1 = 16,
                A_EMA2 = 17,
                B_EMA1 = 43,
                B_EMA2 = 45,
                C_EMA = 52,
                TakeProfit = 450,
                StopLoss = -300,
                CloseEndofDay = false,
                Period = prices.Count,

            };
            //takep = E.TakeProfit;
            //takel = E.StopLoss;
            AlsiUtils.Statistics S = new AlsiUtils.Statistics();
            S.OnStatsCaculated += new AlsiUtils.Statistics.StatsCalculated(S_OnStatsCaculated);
            var Trades = AlsiUtils.Strategies.EMA_Scalp.EmaScalp(E, prices, false);

            Trades = S.CalcBasicTradeStats_old(Trades);
            var NewTrades = AlsiUtils.Strategies.TradeStrategy.Expansion.ApplyRegressionFilter(10, Trades);
            NewTrades = S.CalcExpandedTradeStats(NewTrades);

           // PrintTradesonly(NewTrades);

           
        }

        public void RunSingleNEW()
        {
            //Laptop
            // string css = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            //PC
            string css = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = css;

            //  DateTime s = new DateTime(2006, 01, 01);
            //  DateTime e = new DateTime(2006, 12, 15);

            DateTime s = new DateTime(2011, 12, 15);
            DateTime e = new DateTime(2013, 09, 29);

          

            var prices = AlsiUtils.DataBase.readDataFromDataBase(GlobalObjects.TimeInterval.Minute_5, AlsiUtils.DataBase.dataTable.MasterMinute,
               s, e, false);
            Debug.WriteLine("Start Date " + prices[0].TimeStamp);

            //for (int x = 2; x < 50; x++)
            //{

            AlsiUtils.Strategies.Parameter_EMA_Scalp E = new AlsiUtils.Strategies.Parameter_EMA_Scalp()
            {
                A_EMA1 = 18,
                A_EMA2 = 19,
                B_EMA1 = 25,
                B_EMA2 = 26,
                C_EMA = 32,
                TakeProfit = 25,
                StopLoss = -25,
                CloseEndofDay = true,
                Period = prices.Count,
            };

           var Trades =AlsiUtils.Strategies.EmaSalp2.EmaScalp(E, prices, false);
           S = new AlsiUtils.Statistics();
           S.OnStatsCaculated += new AlsiUtils.Statistics.StatsCalculated(S_OnStatsCaculated);
           
            //var Trades = AlsiUtils.Strategies.EMA_Scalp.EmaScalp(E, prices, false);

            Trades = S.CalcBasicTradeStats(Trades);
           // var NewTrades = AlsiUtils.Strategies.TradeStrategy.Expansion.ApplyRegressionFilter(10, Trades);
           // NewTrades = S.CalcExpandedTradeStats(NewTrades);

            // PrintTradesonly(Trades);


        }

        public void RunMultiple()
        {

            //Laptop
            //string css = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";

            //PC
            string css = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = css;

            DateTime s = new DateTime(2012, 08, 01);
            DateTime end = new DateTime(2013, 01, 01);




            var prices = AlsiUtils.DataBase.readDataFromDataBase(GlobalObjects.TimeInterval.Minute_2, AlsiUtils.DataBase.dataTable.MasterMinute,
               s, end, false);
            Debug.WriteLine("Start Date " + prices[0].TimeStamp);

            for (int x= ema.A1_start; x <= ema.A1_end; x++)
            {
                for (int y = ema.A2_start ; y <= ema.A2_end ; y++)
                {

                    for (int a = ema.B1_start; a <= ema.B1_end; a++)
                    {

                        for (int b = ema.B1_start; b <= ema.B2_end; b++)
                        {

                            for (int z = ema.C1_start; z <= ema.C1_end; z++)
                            {

                                if (x < y && y < a && a < b && b < z)
                                {
                                    E = new AlsiUtils.Strategies.Parameter_EMA_Scalp()
                                    {
                                        A_EMA1 = x,
                                        A_EMA2 = y,
                                        B_EMA1 = a,
                                        B_EMA2 = b,
                                        C_EMA = z,
                                        TakeProfit = 1000,
                                        StopLoss = -1000,
                                        CloseEndofDay = true,
                                        Period = 2012,
                                    };


                                   // var Trades = AlsiUtils.Strategies.EMA_Scalp.EmaScalp(E, prices, false);
                                    var Trades = AlsiUtils.Strategies.EmaSalp2.EmaScalp(E, prices, false);
                                    if (Trades.Count != 0) S.CalcBasicTradeStats(Trades);
                                    //if (Trades.Count != 0) S.CalcBasicTradeStats_old(Trades);

                                }
                            }
                        }
                    }
                }
            }


        }
        
        private void TradeToCandle()
        {
            //Laptop
            // string css = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            //PC
            string css = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = css;



            //  DateTime s = new DateTime(2006, 01, 01);
            //  DateTime e = new DateTime(2006, 12, 15);

            DateTime s = new DateTime(2012, 1, 1);
            DateTime e = new DateTime(2013, 12, 29);


            var prices = AlsiUtils.DataBase.readDataFromDataBase(GlobalObjects.TimeInterval.Minute_5, AlsiUtils.DataBase.dataTable.MasterMinute,
               s, e, false);
            Debug.WriteLine("Start Date " + prices[0].TimeStamp);

            //for (int x = 2; x < 50; x++)
            //{

            AlsiUtils.Strategies.Parameter_EMA_Scalp E = new AlsiUtils.Strategies.Parameter_EMA_Scalp()
            {
                A_EMA1 = 16,
                A_EMA2 = 17,
                B_EMA1 = 43,
                B_EMA2 = 45,
                C_EMA = 52,
                TakeProfit = 450,
                StopLoss = -300,
                CloseEndofDay = false,
                Period = prices.Count,

            };
            //  takep = E.TakeProfit;
            // takel = E.StopLoss;
            AlsiUtils.Statistics S = new AlsiUtils.Statistics();
            S.OnStatsCaculated += new AlsiUtils.Statistics.StatsCalculated(S_OnStatsCaculated);
            var Trades = AlsiUtils.Strategies.EMA_Scalp.EmaScalp(E, prices, false);
            Trades = S.CalcBasicTradeStats_old(Trades);

            var rr = Statistics.IntratradeToCandle(Trades);
            var RR = CompletedTrade.CreateList(rr);



            PrintTradesonly(RR);
        }
        
        private static void PrintTradesonly(List<Trade> Trades)
        {
            DateTime s = new DateTime(2012, 1, 1);
            DateTime e = new DateTime(2013, 12, 13);

            foreach (var t in Trades)
            {
                if (t.TimeStamp > s && t.TimeStamp < e)
                    Debug.WriteLine(t.TimeStamp + ","
                        + t.TradedPrice + ","
                        + t.Reason + ","
                        + t.BuyorSell + ","
                        //+ t.TotalPL + ","
                        // + t.Extention.Slope + ","
                        // + t.Extention.OrderVol + ","
                        //+t.RunningProfit+","
                       + t.OHLC.Open + ","
                       + t.OHLC.High + ","
                       + t.OHLC.Low + ","
                       + t.OHLC.Close
                        );
            }
        }

        private static void PrintTradesonly(List<CompletedTrade> Trades)
        {
            DateTime s = new DateTime(2012, 1, 5);
            DateTime e = new DateTime(2013, 12, 13);

            foreach (var t in Trades)
            {
                var O = t.OpenTrade;
                var C = t.CloseTrade;

                if (O.TimeStamp > s && O.TimeStamp < e)
                {
                    Debug.WriteLine(
                        O.TimeStamp + ","
                        + O.TradedPrice + ","
                        + O.Reason + ","
                        + O.BuyorSell + ","
                        + O.TotalPL + ","
                        + O.Extention.Slope + ","
                        + O.Extention.OrderVol + ","
                        );

                    Debug.WriteLine(
                       C.TimeStamp + ","
                       + C.TradedPrice + ","
                       + C.Reason + ","
                       + C.BuyorSell + ","
                       + C.TotalPL + ","
                       + C.Extention.Slope + ","
                       + C.Extention.OrderVol + ","
                       );
                }
            }
        }

    }

    public class EmaSettings

    {
        public int A1_start { get; set; }
        public int A1_end { get; set; }

        public int A2_start { get; set; }
        public int A2_end { get; set; }

        public int B1_start { get; set; }
        public int B1_end { get; set; }

        public int B2_start { get; set; }
        public int B2_end { get; set; }

        public int C1_start { get; set; }
        public int C1_end { get; set; }
    }
}
