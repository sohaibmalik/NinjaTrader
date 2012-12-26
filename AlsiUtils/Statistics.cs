using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AlsiUtils
{

    public enum Period
    {
        Weekly,
        TwoWeekly,
        Monthly


    }


    public class Statistics
    {
        /// <summary>
        /// Calculates the Frequency Distribution
        /// </summary>
        /// <param name="Input">Input List of Numbers</param>
        /// <param name="Buckets">List of Interval Numbers eg 0,5,10,15</param>
        /// <param name="Frequency">List of Frequencies that Matches the Buckets</param>
        /// <param name="ShortTail">Count of Numbers Lesser that falls outside the Smallest Bucket</param>
        /// <param name="Longtail">Count of Numbers Greater that falls outside the Biggest Bucket</param>
        public static void FreqDist(List<double> Input, List<double> Buckets, out List<int> Frequency, out int ShortTail, out int Longtail)
        {
            try
            {

                Input.Sort();
                int b = Buckets.Count - 1;

                int[] freq = new int[b];

                foreach (int d in Input)
                {
                    for (int i = 0; i < b; i++)
                    {
                        if (d >= Buckets[i] && d < Buckets[i + 1])
                        {
                            freq[i]++;
                            break;
                        }
                    }

                }




                //greater than highest
                int longtail = (from LT in Input
                                where LT > Buckets[Buckets.Count - 1]
                                select LT).Count();

                int shorttail = (from ST in Input
                                 where ST < Buckets[0]
                                 select ST).Count();



                Frequency = freq.ToList();
                ShortTail = shorttail;
                Longtail = longtail;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("Buckets or Input Might Be Empty");
                Frequency = null;
                ShortTail = 0;
                Longtail = 0;
            }
        }



        public static List<Trade> TradeList_Stats = new List<Trade>();
        public static List<SummaryStats> SummaryProfitLoss(List<Trade> Trades, Period Period)
        {

            List<SummaryStats> statsList = new List<SummaryStats>();



            if (TradeList_Stats.Count == 0)
            {
                Debug.WriteLine("Loading");
                foreach (Trade t in Trades)
                {
                    if (t.RunningProfit != 0) TradeList_Stats.Add(t);

                }
            }
            else
            {
                Debug.WriteLine("NOT Loading Again");
            }


            #region Weekly

            if (Period == Period.Weekly)
            {
                var weekly = from q in TradeList_Stats
                             group q by new
                             {
                                 Y = q.TimeStamp.Year,
                                 M = q.TimeStamp.Month,
                                 W = Math.Floor((decimal)q.TimeStamp.DayOfYear / 7) + 1,
                                 //D=(DateTime)q.TimeStamp
                             }
                                 into FGroup
                                 orderby FGroup.Key.Y, FGroup.Key.M, FGroup.Key.W
                                 select new
                                 {
                                     Year = FGroup.Key.Y,
                                     Month = FGroup.Key.M,
                                     Week = FGroup.Key.W,
                                     Count = FGroup.Count(),
                                     SumPrice = (int)FGroup.Sum(t => t.RunningProfit),
                                     AvPrice = (double)FGroup.Average(t => t.RunningProfit),
                                     //Time = FGroup.Key.D
                                 };

                foreach (var v in weekly)
                {
                    SummaryStats stat = new SummaryStats();
                    stat.Detail = "Weekly Profit and Loss Summary";
                    stat.Period = Period;
                    stat.Year = (int)v.Year;
                    stat.Month = (int)v.Month;
                    stat.Week = (int)v.Week;
                    stat.Count = (int)v.Count;
                    stat.Sum = (int)v.SumPrice;
                    stat.Average = (double)v.AvPrice;
                    statsList.Add(stat);

                    Debug.WriteLine(v.Year + "   " + v.Month + "  " + v.Week + "  " + v.Count + "  " + v.SumPrice + "  " + v.AvPrice);
                }

            }

            #endregion

            #region Fortnight

            if (Period == Period.TwoWeekly)
            {

                var fortnight = from q in TradeList_Stats
                                group q by new
                                {
                                    Y = q.TimeStamp.Year,
                                    M = q.TimeStamp.Month,
                                    W = q.TimeStamp.Day <= 15 ? 1 : 2,
                                    //D=(DateTime)q.TimeStamp
                                }
                                    into FGroup
                                    orderby FGroup.Key.Y, FGroup.Key.M, FGroup.Key.W
                                    select new
                                    {
                                        Year = FGroup.Key.Y,
                                        Month = FGroup.Key.M,
                                        Week = FGroup.Key.W,
                                        Count = FGroup.Count(),
                                        SumPrice = (int)FGroup.Sum(t => t.RunningProfit),
                                        AvPrice = (double)FGroup.Average(t => t.RunningProfit),
                                        //Time = FGroup.Key.D
                                    };

                foreach (var v in fortnight)
                {
                    SummaryStats stat = new SummaryStats();
                    stat.Detail = "Fortnightly Profit and Loss Summary";
                    stat.Period = Period;
                    stat.Year = (int)v.Year;
                    stat.Month = (int)v.Month;
                    stat.Week = (int)v.Week;
                    stat.Count = (int)v.Count;
                    stat.Sum = (int)v.SumPrice;
                    stat.Average = (double)v.AvPrice;
                    statsList.Add(stat);
                    Debug.WriteLine(v.Year + "   " + v.Month + "  " + v.Week + "  " + v.Count + "  " + v.SumPrice + "  " + v.AvPrice);
                }


            }

            #endregion

            #region Monthly

            if (Period == Period.Monthly)
            {
                var monthly = from q in TradeList_Stats
                              group q by new
                              {
                                  Y = q.TimeStamp.Year,
                                  M = q.TimeStamp.Month,

                                  //D=(DateTime)q.TimeStamp
                              }
                                  into FGroup
                                  orderby FGroup.Key.Y, FGroup.Key.M
                                  select new
                                  {
                                      Year = FGroup.Key.Y,
                                      Month = FGroup.Key.M,
                                      Count = FGroup.Count(),
                                      SumPrice = (int)FGroup.Sum(t => t.RunningProfit),
                                      AvPrice = (double)FGroup.Average(t => t.RunningProfit),
                                      //Time = FGroup.Key.D
                                  };

                foreach (var v in monthly)
                {
                    SummaryStats stat = new SummaryStats();
                    stat.Detail = "Monthly Profit and Loss Summary";
                    stat.Period = Period;
                    stat.Year = (int)v.Year;
                    stat.Month = (int)v.Month;
                    stat.Week = 0;
                    stat.Count = (int)v.Count;
                    stat.Sum = (int)v.SumPrice;
                    stat.Average = (double)v.AvPrice;
                    statsList.Add(stat);
                    Debug.WriteLine(v.Year + "   " + v.Month + "  " + v.Count + "  " + v.SumPrice + "  " + v.AvPrice);
                }
            }
            #endregion


            return statsList;

        }


        public static void StandardDevOnPL(List<Trade> Trades)
        {
            var viList = new List<Indicators.VariableIndicator>();
            var list = Trades.Where(z => z.Reason == Trade.Trigger.CloseLong || z.Reason == Trade.Trigger.CloseShort);
            foreach (var l in list)
            {
                var VI = new Indicators.VariableIndicator()
                {
                    Value = l.TotalPL,
                    TimeStamp = l.TimeStamp,

                };
                viList.Add(VI);
            }

            var SDTDEV = Factory_Indicator.creatStandardDeviation(1, 10, viList);
            foreach (var v in SDTDEV)
                Debug.WriteLine(v.N + "," + v.SingleStdev + "," + v.CustomValue + "," + v.StdDev);
        }

        public static List<Trade> RegressionAnalysis_OnPL(int N,List<Trade> Trades)
        {
            List<Indicators.VariableIndicator> V = new List<Indicators.VariableIndicator>();
            var tradeonlyList=Trades.Where(z=>z.Reason==Trade.Trigger.CloseLong || z.Reason==Trade.Trigger.CloseShort);
            foreach (var v in tradeonlyList)
            {
                var varindicator = new Indicators.VariableIndicator()
                {
                    TimeStamp=v.TimeStamp,
                    Value=v.TotalPL,

                };

                V.Add(varindicator);
            }
            var reg = Factory_Indicator.createRegression(N, V);

           // var avg= reg.Sum(z => z.Slope) / reg.Count;
           // Debug.WriteLine(avg);
            foreach (var t in tradeonlyList)
            {
                foreach (var e in reg)
                {
                    if (e.TimeStamp == t.TimeStamp)
                    {                       
                        t.Extention.Regression  = e.Regression;                      
                        t.Extention.Slope  = e.Slope;
                    }

                }

            }
            return tradeonlyList.ToList();
          //  foreach (var t in tradeonlyList) Debug.WriteLine(t.TimeStamp + "," + t.TotalPL + "," + t.Variable_1 + "," + t.Variable_2);
        }
               


        public List<Trade> CalcBasicTradeStats(List<Trade> Trades)
        {

            double totalProfit = 0;
            double tradeCount = 0;
            double prof_count = 0;
            double loss_count = 0;
            double prof_sum = 0;
            double loss_sum = 0;


            for (int x = 1; x < Trades.Count; x++)
            {

                if (Trades[x].Reason == Trade.Trigger.CloseLong || Trades[x].Reason == Trade.Trigger.CloseShort ||
                    Trades[x].Reason == Trade.Trigger.StopLoss || Trades[x].Reason == Trade.Trigger.TakeProfit ||
                    Trades[x].Reason == Trade.Trigger.EndOfDayCloseLong || Trades[x].Reason == Trade.Trigger.EndOfDayCloseShort
                    )
                {
                    totalProfit += Trades[x].RunningProfit;
                    tradeCount++;

                    if (Trades[x].RunningProfit <= 0)
                    {
                        loss_count++;
                        loss_sum += Trades[x].RunningProfit;
                    }

                    if (Trades[x].RunningProfit > 0)
                    {
                        prof_count++;
                        prof_sum += Trades[x].RunningProfit;
                    }


                    Trades[x].TotalPL = totalProfit;
                    Trades[x].TradeCount = (int)tradeCount;
                }

                if (Trades[x].Reason == Trade.Trigger.OpenLong || Trades[x].Reason == Trade.Trigger.OpenShort) Trades[x].TotalPL = totalProfit;
            }

            decimal avg_pl = (decimal)Math.Round((totalProfit / tradeCount), 5);
            decimal loss_pct = (decimal)Math.Round((loss_count / tradeCount) * 100, 2);
            decimal prof_pct = (decimal)Math.Round((prof_count / tradeCount) * 100, 2);
            decimal pl_ratio = (decimal)Math.Round((prof_sum / loss_sum), 2);
            decimal avg_profit = (decimal)Math.Round((prof_count / prof_sum), 2);
            decimal avg_loss = (decimal)Math.Round((loss_count / loss_sum), 2);

            SummaryStats s = new SummaryStats()
            {
                TotalProfit = totalProfit,
                TradeCount = tradeCount,
                Total_Avg_PL = avg_pl,
                Pct_Prof = prof_pct,
                Pct_Loss = loss_pct,
                Avg_Loss = avg_loss,
                Avg_Prof = avg_profit,

            };

            StatsCalculatedEvent sc = new StatsCalculatedEvent();
            sc.SumStats = s;
            OnStatsCaculated(this, sc);

            return Trades;
        }

        public static void OverNightPosAnalysis(List<Trade> Trades)
        {
            var overnightpos = from ovn in Trades
                               where ovn.TimeStamp.Hour == 17 && ovn.TimeStamp.Minute == 20 && ovn.Position
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
                var closepos = (from t in Trades
                                where t.TimeStamp > v.TimeStamp && !t.Position
                                select t).AsEnumerable().First();


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

                Debug.WriteLine("Overnight " + v.TimeStamp + "  Pos:" + v.CurrentDirection + "  " + v.CurrentPrice + "      " + v.RunningProfit);
                Debug.WriteLine("Close " + closepos.TimeStamp + "  Pos:" + closepos.CurrentDirection + "  " + closepos.CurrentPrice + "      " + closepos.RunningProfit);
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
            Debug.WriteLine("Overnight Count : " + overnightpos.Count());
            Debug.WriteLine("OL CL " + Oloss_Closs);
            Debug.WriteLine("OL CP " + Oloss_Cprof);
            Debug.WriteLine("OP CL " + Oprof_Closs);
            Debug.WriteLine("OP CP " + Oprof_Cprof);
            Debug.WriteLine("N CL " + N_Closs);
            Debug.WriteLine("N CP " + N_Prof);
            Debug.WriteLine("Diff " + Diff);
        }


        public event StatsCalculated OnStatsCaculated;
        public delegate void StatsCalculated(object sender, StatsCalculatedEvent e);
        public class StatsCalculatedEvent : EventArgs
        {
            public SummaryStats SumStats;
        }


    }
}
