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





    }
}
