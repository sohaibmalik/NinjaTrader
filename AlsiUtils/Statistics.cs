using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AlsiUtils.Data_Objects;

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


        public static List<SummaryStats> SummaryProfitLoss(List<Trade> Trades, Period Period)
        {

            var TradeList_Stats = new List<Trade>();
            List<SummaryStats> statsList = new List<SummaryStats>();
            if (TradeList_Stats.Count == 0)
                foreach (Trade t in Trades) if (t.RunningProfit != 0) TradeList_Stats.Add(t);

            try
            {

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
                                         Count = FGroup.Count(z => z.Reason != Trade.Trigger.None),
                                         FirstTradeDate = FGroup.First().TimeStamp,
                                         LastTradeDate = FGroup.Last().TimeStamp,
                                         AvPrice = (double)FGroup.Where(z => z.Reason == Trade.Trigger.CloseShort || z.Reason == Trade.Trigger.CloseLong)
                                          .Average(t => t.RunningProfit),
                                         SumPrice = (int)FGroup.Where(z => z.Reason == Trade.Trigger.CloseShort || z.Reason == Trade.Trigger.CloseLong)
                                         .Sum(t => t.RunningProfit),
                                         marketmovement = (FGroup.Last().CurrentPrice) - (FGroup.First().CurrentPrice),
                                         Prices = FGroup.Select(z => z.CurrentPrice),

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
                        stat.FirstTrade = v.FirstTradeDate;
                        stat.LastTrade = v.LastTradeDate;
                        stat.MarketMovement = v.marketmovement;
                        stat.StandardDeviation = StandardDeviation(v.Prices.ToList());
                        statsList.Add(stat);

                        //    Debug.WriteLine(v.Year + "   " + v.Month + "  " + v.Week + "  " + v.Count + "  " + v.SumPrice + "  " + v.AvPrice);
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
                                            Count = FGroup.Count(z => z.Reason != Trade.Trigger.None),
                                            FirstTradeDate = FGroup.First().TimeStamp,
                                            LastTradeDate = FGroup.Last().TimeStamp,
                                            AvPrice = (double)FGroup.Where(z => z.Reason == Trade.Trigger.CloseShort || z.Reason == Trade.Trigger.CloseLong)
                                          .Average(t => t.RunningProfit),
                                            SumPrice = (int)FGroup.Where(z => z.Reason == Trade.Trigger.CloseShort || z.Reason == Trade.Trigger.CloseLong)
                                            .Sum(t => t.RunningProfit),
                                            marketmovement = (FGroup.Last().CurrentPrice) - (FGroup.First().CurrentPrice),
                                            Prices = FGroup.Select(z => z.CurrentPrice),
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
                        stat.FirstTrade = v.FirstTradeDate;
                        stat.LastTrade = v.LastTradeDate;
                        stat.MarketMovement = v.marketmovement;
                        stat.StandardDeviation = StandardDeviation(v.Prices.ToList());
                        statsList.Add(stat);
                        // Debug.WriteLine(v.Year + "   " + v.Month + "  " + v.Week + "  " + v.Count + "  " + v.SumPrice + "  " + v.AvPrice);
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
                                          FirstTradeDate = FGroup.First().TimeStamp,
                                          LastTradeDate = FGroup.Last().TimeStamp,
                                          Count = FGroup.Count(z => z.Reason != Trade.Trigger.None),
                                          AvPrice = (double)FGroup.Where(z => z.Reason == Trade.Trigger.CloseShort || z.Reason == Trade.Trigger.CloseLong)
                                          .Average(t => t.RunningProfit),
                                          SumPrice = (int)FGroup.Where(z => z.Reason == Trade.Trigger.CloseShort || z.Reason == Trade.Trigger.CloseLong)
                                          .Sum(t => t.RunningProfit),
                                          marketmovement = (FGroup.Last().CurrentPrice) - (FGroup.First().CurrentPrice),
                                          Prices = FGroup.Select(z => z.CurrentPrice),
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
                        stat.FirstTrade = v.FirstTradeDate;
                        stat.LastTrade = v.LastTradeDate;
                        stat.MarketMovement = v.marketmovement;
                        stat.StandardDeviation = StandardDeviation(v.Prices.ToList());
                        statsList.Add(stat);
                        //  Debug.WriteLine(v.Year + "   " + v.Month + "  " + v.Count + "  " + v.SumPrice + "  " + v.AvPrice);
                    }
                }
                #endregion
            }
            catch { return statsList; }
            return statsList;

        }

        public static void StandardDevOnPL(List<Trade> Trades)
        {
            var viList = new List<VariableIndicator>();
            var list = Trades.Where(z => z.Reason == Trade.Trigger.CloseLong || z.Reason == Trade.Trigger.CloseShort);
            foreach (var l in list)
            {
                var VI = new VariableIndicator()
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

        public static List<Trade> RegressionAnalysis_OnPL(int N, List<Trade> Trades)
        {
            List<VariableIndicator> V = new List<VariableIndicator>();
            var tradeonlyList = Trades.Where(z => z.Reason == Trade.Trigger.CloseLong || z.Reason == Trade.Trigger.CloseShort);
            foreach (var v in tradeonlyList)
            {
                var varindicator = new VariableIndicator()
                {
                    TimeStamp = v.TimeStamp,
                    Value = v.TotalPL,

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
                        t.Extention.Regression = e.Regression;
                        t.Extention.Slope = e.Slope;
                        t.Extention.OrderVol = 1;
                    }

                }

            }





            return tradeonlyList.ToList();

        }


        public List<Trade> CalcBasicTradeStats_old(List<Trade> Trades)
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

            try
            {
                StatsCalculatedEvent sc = new StatsCalculatedEvent();
                sc.SumStats = s;
                OnStatsCaculated(this, sc);
            }
            catch
            {
            }
            return Trades;
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

                if (Trades[x].TradeTriggerGeneral == Trade.Trigger.Close)
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

            try
            {
                StatsCalculatedEvent sc = new StatsCalculatedEvent();
                sc.SumStats = s;
                OnStatsCaculated(this, sc);
            }
            catch
            {
            }
            return Trades;
        }

        public List<CompletedTrade> CalcExpandedTradeStats(List<CompletedTrade> Trades)
        {
            double totalProfit = 0;
            double tradeCount = Trades.Count;
            double prof_count = 0;
            double loss_count = 0;
            double prof_sum = 0;
            double loss_sum = 0;



            foreach (var t in Trades)
            {
                t.OpenTrade.Extention.newTotalProfit = totalProfit;

                if (t.CloseTrade.Reason == Trade.Trigger.CloseLong)
                {
                    double prof = (t.CloseTrade.TradedPrice - t.OpenTrade.TradedPrice) * t.OpenTrade.Extention.OrderVol;
                    t.CloseTrade.Extention.newRunningProf = prof;
                    totalProfit += prof;
                    t.CloseTrade.Extention.newTotalProfit = totalProfit;
                    if (prof > 0)
                    {
                        prof_sum += prof;
                        prof_count++;
                    }
                    else
                    {
                        loss_sum += prof;
                        loss_count++;
                    }
                }

                if (t.CloseTrade.Reason == Trade.Trigger.CloseShort)
                {
                    double prof = (t.OpenTrade.TradedPrice - t.CloseTrade.TradedPrice) * t.OpenTrade.Extention.OrderVol;
                    t.CloseTrade.Extention.newRunningProf = prof;
                    totalProfit += prof;
                    t.CloseTrade.Extention.newTotalProfit = totalProfit;
                    if (prof > 0)
                    {
                        prof_sum += prof;
                        prof_count++;
                    }
                    else
                    {
                        loss_sum += prof;
                        loss_count++;
                    }
                }
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

            try
            {
                StatsCalculatedEvent sc = new StatsCalculatedEvent();
                sc.SumStats = s;
                OnStatsCaculated(this, sc);
            }
            catch
            {
            }
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

        public static double StandardDeviation(List<double> values)
        {
            double avg = values.Average();
            return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
        }

        public static List<CompletedTrade> IntratradeToCandle(List<Trade> FullTradeList)
        {
            var TO = Trade.TradesOnly(FullTradeList);
            var r = CompletedTrade.CreateList(TO);

            foreach (var to in r)
            {
                var end = to.CloseTrade.TimeStamp;
                if (to.CloseTrade.Reason == Trade.Trigger.None) end = DateTime.Now;

                var pl = from x in FullTradeList
                         where x.TimeStamp >= to.OpenTrade.TimeStamp && x.TimeStamp <= end
                         select x.RunningProfit;

                var minmax = new
                {
                    low = pl.Min(),
                    high = pl.Max(),
                    close = pl.Last(),
                    fist = pl.Skip(1).First(),
                };

                to.CloseTrade.OHLC.Low = minmax.low;
                to.CloseTrade.OHLC.High = minmax.high;
                to.CloseTrade.OHLC.Close = minmax.close;
                to.CloseTrade.OHLC.Open = minmax.fist;
            }

            return r;
        }

        public static List<CompletedTrade> TakeProfit_Exiguous(List<Trade> FullTradeList, double TakeProfit, double StopLoss)
        {
            var TakeProfitList = new List<CompletedTrade>();
            var TO = Trade.TradesOnly(FullTradeList);
            var CompletedList = CompletedTrade.CreateList(TO);


            foreach (var t in CompletedList)
            {
                var pl = from x in FullTradeList
                         where x.TimeStamp >= t.OpenTrade.TimeStamp && x.TimeStamp <= t.CloseTrade.TimeStamp
                         select x;


                var trade = new CompletedTrade();
                trade.OpenTrade = t.OpenTrade;

                //Check profit
                Trade profitTrade;
                Trade lossTrade;
                if (pl.Any(z => z.RunningProfit > TakeProfit))
                    profitTrade = pl.Where(z => z.RunningProfit > TakeProfit).First();
                else
                    profitTrade = t.CloseTrade;



                // check loss
                if (pl.Any(z => z.RunningProfit < StopLoss))
                    lossTrade = pl.Where(z => z.RunningProfit < StopLoss).First();
                else
                    lossTrade = t.CloseTrade;

                if (lossTrade.TimeStamp > profitTrade.TimeStamp)
                    trade.CloseTrade = profitTrade;
                else
                    trade.CloseTrade = lossTrade;


                if (trade.OpenTrade.Reason == Trade.Trigger.OpenLong) trade.CloseTrade.Reason = Trade.Trigger.CloseLong;
                if (trade.OpenTrade.Reason == Trade.Trigger.OpenShort) trade.CloseTrade.Reason = Trade.Trigger.CloseShort;


                TakeProfitList.Add(trade);

            }


            return TakeProfitList;
        }


        public static List<CompletedTrade> TakeProfit_Exiguous_GoldenBoil(List<Trade> FullTradeList, int N, double stdev, int triggercount)
        {
            var TakeProfitList = new List<CompletedTrade>();
            var TO = Trade.TradesOnly(FullTradeList);
            var CompletedList = CompletedTrade.CreateList(TO);

            var GoldenBoil = Factory_Indicator.createBollingerBand(N, stdev, GlobalObjects.Points, TicTacTec.TA.Library.Core.MAType.Sma);


            foreach (var t in CompletedList)
            {
                var timeframe = from x in FullTradeList
                                where x.TimeStamp >= t.OpenTrade.TimeStamp && x.TimeStamp <= t.CloseTrade.TimeStamp
                                select x;


                var trade = new CompletedTrade();
                trade.OpenTrade = t.OpenTrade;

                int boilTriggerCount = 0;
                foreach (var ts in timeframe)
                {
                    if (t.OpenTrade.Reason == Trade.Trigger.OpenShort)
                        if (ts.CurrentPrice < GoldenBoil.Where(z => z.TimeStamp == ts.TimeStamp).First().Lower) boilTriggerCount++;
                    if (t.OpenTrade.Reason == Trade.Trigger.OpenLong)
                        if (ts.CurrentPrice > GoldenBoil.Where(z => z.TimeStamp == ts.TimeStamp).First().Upper) boilTriggerCount++;
                    if (boilTriggerCount == triggercount)
                    {
                        trade.CloseTrade = ts;
                        break;
                    }
                    trade.CloseTrade = ts;
                }

                if (trade.OpenTrade.Reason == Trade.Trigger.OpenLong) trade.CloseTrade.Reason = Trade.Trigger.CloseLong;
                if (trade.OpenTrade.Reason == Trade.Trigger.OpenShort) trade.CloseTrade.Reason = Trade.Trigger.CloseShort;


                TakeProfitList.Add(trade);

            }


            return TakeProfitList;
        }

        public static List<CompletedTrade> TakeProfit_Exiguous_SlowStoch_K(List<Trade> FullTradeList, int FK, int SK, int D, double H, double L)
        {
            var TakeProfitList = new List<CompletedTrade>();
            var TO = Trade.TradesOnly(FullTradeList);
            var CompletedList = CompletedTrade.CreateList(TO);

            var SlowStoch = Factory_Indicator.createSlowStochastic(FK, SK, D, GlobalObjects.Points);


            foreach (var t in CompletedList)
            {
                var timeframe = from x in FullTradeList
                                where x.TimeStamp >= t.OpenTrade.TimeStamp && x.TimeStamp <= t.CloseTrade.TimeStamp
                                select x;

                var period = from x in SlowStoch
                             where x.TimeStamp >= t.OpenTrade.TimeStamp && x.TimeStamp <= t.CloseTrade.TimeStamp
                             select x;

                // foreach (var v in period) Debug.WriteLine(v.TimeStamp + "   " + v.D);

                var trade = new CompletedTrade();
                trade.OpenTrade = t.OpenTrade;


                bool TookProfitLong = false;
                bool TookProfitShort = false;
                DateTime lowJumpUp = t.CloseTrade.TimeStamp;
                DateTime highDipDown = t.CloseTrade.TimeStamp;

                if (t.OpenTrade.Reason == Trade.Trigger.OpenLong)
                {
                    bool touchedHigh = period.Where(z => z.K > H).Any();
                    DateTime highTime;

                    bool dipped;

                    if (touchedHigh)
                    {
                        highTime = period.Where(z => z.K > H).Select(a => a).First().TimeStamp;
                        var x = period.Where(z => z.TimeStamp > highTime && z.K < H);
                        dipped = x.Any();
                        if (dipped)
                        {
                            highDipDown = x.First().TimeStamp;
                            TookProfitLong = true;
                        }
                        else
                            TookProfitLong = false;
                    }
                }

                if (t.OpenTrade.Reason == Trade.Trigger.OpenShort)
                {
                    // foreach (var v in period) Debug.WriteLine(v.TimeStamp + "   " + v.D);
                    var touchedLow = period.Where(z => z.K < L).Any();
                    DateTime lowTime;

                    bool jumped;
                    if (touchedLow)
                    {

                        lowTime = (period.Where(z => z.K < L)).Select(a => a).First().TimeStamp;
                        var x = period.Where(z => z.TimeStamp > lowTime && z.K > L);
                        jumped = x.Any();
                        if (jumped)
                        {
                            lowJumpUp = x.First().TimeStamp;
                            TookProfitShort = true;
                        }
                        else
                            TookProfitShort = false;
                    }

                }

                if (TookProfitLong)
                {
                    trade.CloseTrade.TimeStamp = highDipDown;
                    trade.CloseTrade.RunningProfit = timeframe.Where(z => z.TimeStamp == highDipDown).First().RunningProfit;
                }
                else
                    if (TookProfitShort)
                    {
                        trade.CloseTrade.TimeStamp = lowJumpUp;
                        trade.CloseTrade.RunningProfit = timeframe.Where(z => z.TimeStamp == lowJumpUp).First().RunningProfit;
                    }
                    else
                        trade.CloseTrade = t.CloseTrade;
                //if (t.OpenTrade.Reason == Trade.Trigger.OpenShort)
                //    if (ts.CurrentPrice < GoldenBoil.Where(z => z.TimeStamp == ts.TimeStamp).First().Lower) boilTriggerCount++;
                //if (t.OpenTrade.Reason == Trade.Trigger.OpenLong)
                //    if (ts.CurrentPrice > GoldenBoil.Where(z => z.TimeStamp == ts.TimeStamp).First().Upper) boilTriggerCount++;
                //if (boilTriggerCount == triggercount)
                //{
                //    trade.CloseTrade = ts;
                //    break;
                //}
                //trade.CloseTrade = ts;


                if (trade.OpenTrade.Reason == Trade.Trigger.OpenLong) trade.CloseTrade.Reason = Trade.Trigger.CloseLong;
                if (trade.OpenTrade.Reason == Trade.Trigger.OpenShort) trade.CloseTrade.Reason = Trade.Trigger.CloseShort;


                TakeProfitList.Add(trade);

            }


            return TakeProfitList;
        }

        public static List<CompletedTrade> TakeProfit_Exiguous_SlowStoch_D(List<Trade> FullTradeList, int FK, int SK, int D, double H, double L)
        {
            var TakeProfitList = new List<CompletedTrade>();
            var TO = Trade.TradesOnly(FullTradeList);
            var CompletedList = CompletedTrade.CreateList(TO);

            var SlowStoch = Factory_Indicator.createSlowStochastic(FK, SK, D, GlobalObjects.Points);


            foreach (var t in CompletedList)
            {
                var timeframe = from x in FullTradeList
                                where x.TimeStamp >= t.OpenTrade.TimeStamp && x.TimeStamp <= t.CloseTrade.TimeStamp
                                select x;

                var period = from x in SlowStoch
                             where x.TimeStamp >= t.OpenTrade.TimeStamp && x.TimeStamp <= t.CloseTrade.TimeStamp
                             select x;

                // foreach (var v in period) Debug.WriteLine(v.TimeStamp + "   " + v.D);

                var trade = new CompletedTrade();
                trade.OpenTrade = t.OpenTrade;


                bool TookProfitLong = false;
                bool TookProfitShort = false;
                DateTime lowJumpUp = t.CloseTrade.TimeStamp;
                DateTime highDipDown = t.CloseTrade.TimeStamp;

                if (t.OpenTrade.Reason == Trade.Trigger.OpenLong)
                {
                    bool touchedHigh = period.Where(z => z.D > H).Any();
                    DateTime highTime;

                    bool dipped;

                    if (touchedHigh)
                    {
                        highTime = period.Where(z => z.D > H).Select(a => a).First().TimeStamp;
                        var x = period.Where(z => z.TimeStamp > highTime && z.D < H);
                        dipped = x.Any();
                        if (dipped)
                        {
                            highDipDown = x.First().TimeStamp;
                            TookProfitLong = true;
                        }
                        else
                            TookProfitLong = false;
                    }
                }

                if (t.OpenTrade.Reason == Trade.Trigger.OpenShort)
                {
                    // foreach (var v in period) Debug.WriteLine(v.TimeStamp + "   " + v.D);
                    var touchedLow = period.Where(z => z.D < L).Any();
                    DateTime lowTime;

                    bool jumped;
                    if (touchedLow)
                    {

                        lowTime = (period.Where(z => z.D < L)).Select(a => a).First().TimeStamp;
                        var x = period.Where(z => z.TimeStamp > lowTime && z.D > L);
                        jumped = x.Any();
                        if (jumped)
                        {
                            lowJumpUp = x.First().TimeStamp;
                            TookProfitShort = true;
                        }
                        else
                            TookProfitShort = false;
                    }

                }

                if (TookProfitLong)
                {
                    trade.CloseTrade.TimeStamp = highDipDown;
                    trade.CloseTrade.RunningProfit = timeframe.Where(z => z.TimeStamp == highDipDown).First().RunningProfit;
                }
                else
                    if (TookProfitShort)
                    {
                        trade.CloseTrade.TimeStamp = lowJumpUp;
                        trade.CloseTrade.RunningProfit = timeframe.Where(z => z.TimeStamp == lowJumpUp).First().RunningProfit;
                    }
                    else
                        trade.CloseTrade = t.CloseTrade;
                //if (t.OpenTrade.Reason == Trade.Trigger.OpenShort)
                //    if (ts.CurrentPrice < GoldenBoil.Where(z => z.TimeStamp == ts.TimeStamp).First().Lower) boilTriggerCount++;
                //if (t.OpenTrade.Reason == Trade.Trigger.OpenLong)
                //    if (ts.CurrentPrice > GoldenBoil.Where(z => z.TimeStamp == ts.TimeStamp).First().Upper) boilTriggerCount++;
                //if (boilTriggerCount == triggercount)
                //{
                //    trade.CloseTrade = ts;
                //    break;
                //}
                //trade.CloseTrade = ts;


                if (trade.OpenTrade.Reason == Trade.Trigger.OpenLong) trade.CloseTrade.Reason = Trade.Trigger.CloseLong;
                if (trade.OpenTrade.Reason == Trade.Trigger.OpenShort) trade.CloseTrade.Reason = Trade.Trigger.CloseShort;


                TakeProfitList.Add(trade);

            }


            return TakeProfitList;
        }

        public static List<CompletedTrade> TakeProfit_Exiguous_AvgProfitPeriod(List<Price> EndOfDay, List<Trade> FullTradeList, int I, int ATR_Range, double ATR_Factor)
        {
            var TakeProfitList = new List<CompletedTrade>();
            var TO = Trade.TradesOnly(FullTradeList);
            var CompletedList = CompletedTrade.CreateList(TO);

            var ATR = Factory_Indicator.createATR(ATR_Range, EndOfDay);

            double pl = 0;
            double count = 0;
            double index = 0;
            double tempPL = 0;
            double TotalImprovement = 0;
            List<TestData> TD = new List<TestData>();
            foreach (var t in CompletedList)
            {
                var timeframe = from x in FullTradeList
                                where x.TimeStamp >= t.OpenTrade.TimeStamp && x.TimeStamp <= t.CloseTrade.TimeStamp
                                select x;


                var TList = timeframe.ToList();


                double A = 0;
                double B = 0;
                for (int x = 0; x < TList.Count; x++)
                    if (x > I && TList.First().TradeVolume != 2)
                    {
                        var adxYesterday = ATR.Where(z => z.TimeStamp.Date.AddDays(-1) == TList[x].TimeStamp.Date);
                        if (adxYesterday.ToList().Count() == 0) break;
                        var Z = Math.Round((adxYesterday.First().AvgTrueRange * ATR_Factor), 2);
                        if (TList[x].RunningProfit < Z) break;
                        A = TList[x].RunningProfit;
                        B = TList.Last().RunningProfit;
                        tempPL += TList[x].RunningProfit;
                        Debug.WriteLine(x + "  " + TList[x].TimeStamp + " vol : " + TList.First().TradeVolume + " ADRxFact " + Z + "         " + A + " VS " + B + " Diff : " + ((A - B) * TList.First().TradeVolume) + " Running " + TotalImprovement);
                        TotalImprovement += ((A - B)); //*TList.First().TradeVolume);

                    }

            }




            Debug.WriteLine("TOTAL IMPROVE I(" + I + ")" + "  ATR_Range(" + ATR_Range + ") ATR_Factor(" + ATR_Factor + ") ====>>  " + TotalImprovement);


            return CompletedList;
        }

        public static List<Trade> TakeProfit_Exiguous_(List<Trade> FullTradeList, int tp, int period)
        {
            var TakeProfitList = new List<CompletedTrade>();
            var TO = Trade.TradesOnly(FullTradeList);
            var CompletedList = CompletedTrade.CreateList(TO);
            double newProf = 0;
            List<TestData> TD = new List<TestData>();


            //reset 2 vol running prof
            var doublevoladjusted = FullTradeList.Where(z => z.TradeVolume == 2);
            foreach (var d in doublevoladjusted)
            {
                d.RunningProfit = (d.RunningProfit / 2);
                // Debug.WriteLine(d.TimeStamp + "   " + d.Reason + "  " + d.CurrentPrice + "  " + d.TradedPrice + "  " + d.RunningProfit + "  " + d.TradeVolume);
            }

            //reset running pl + traded price
            foreach (var v in FullTradeList)
            {
                v.TradedPrice = 0;
                //  v.TotalPL = 0;
            }


            foreach (var t in CompletedList)
            {
                var timeframe = from x in FullTradeList
                                where x.TimeStamp >= t.OpenTrade.TimeStamp && x.TimeStamp <= t.CloseTrade.TimeStamp
                                select x;


                var TList = timeframe.ToList();
                var trade = new CompletedTrade();
                trade.OpenTrade = TList[0];
                trade.CloseTrade = TList.Last();
                if (TList.Count < period) ;
                else
                {
                    for (int x = period; x < TList.Count; x++)
                    {


                        if (TList[x].RunningProfit - TList[x - period].RunningProfit >= tp)
                        {
                            //mark changes
                            var adjusted = timeframe.Where(z => z.TimeStamp > TList[x].TimeStamp);
                            foreach (var a in adjusted) a.Notes = "reset";


                            trade.CloseTrade = TList[x];


                            if (trade.OpenTrade.Reason == Trade.Trigger.OpenShort)
                            {
                                trade.CloseTrade.Reason = Trade.Trigger.CloseShort;
                                trade.CloseTrade.BuyorSell = Trade.BuySell.Buy;
                                trade.CloseTrade.TradeVolume = trade.OpenTrade.TradeVolume;
                                trade.CloseTrade.Position = false;

                            }
                            if (trade.OpenTrade.Reason == Trade.Trigger.OpenLong)
                            {
                                trade.CloseTrade.Reason = Trade.Trigger.CloseLong;
                                trade.CloseTrade.BuyorSell = Trade.BuySell.Sell;
                                trade.CloseTrade.TradeVolume = trade.OpenTrade.TradeVolume;
                                trade.CloseTrade.Position = false;

                            }
                            TakeProfitList.Add(trade);
                            break;
                        }

                        TakeProfitList.Add(trade);
                    }

                }

            }

            var totalOriginal = TO.Sum(z => z.RunningProfit);
            var improve = Math.Round(((newProf - totalOriginal) / totalOriginal * 100), 2);

            Debug.WriteLine("TP," + tp + ",Period," + period + "," + newProf + "," + improve);


            //calculate last trade
            if (TakeProfitList.Last().CloseTrade == TakeProfitList.Last().OpenTrade)
            {
                Debug.WriteLine("Open Position Found..Calculating..");
                var timeframe = (from x in FullTradeList
                                 where x.TimeStamp >= CompletedList.Last().OpenTrade.TimeStamp
                                 select x).ToList();

                if (timeframe.Count > period)
                {
                    var trade = TakeProfitList.Last();
                    for (int x = period; x < timeframe.Count; x++)
                    {
                        if (timeframe[x].RunningProfit - timeframe[x - period].RunningProfit >= tp)
                        {
                            Debug.WriteLine("Take profit Live : " + timeframe[x]);
                            trade.CloseTrade = timeframe[x];
                            timeframe.Where(z => z.TimeStamp == timeframe[x].TimeStamp).First().RunningProfit = timeframe[x].RunningProfit;
                            if (trade.OpenTrade.Reason == Trade.Trigger.OpenShort)
                            {
                                trade.CloseTrade.Reason = Trade.Trigger.CloseShort;
                                trade.CloseTrade.BuyorSell = Trade.BuySell.Buy;
                                trade.CloseTrade.TradeVolume = trade.OpenTrade.TradeVolume;
                                trade.CloseTrade.Position = false;
                                trade.CloseTrade.CurrentDirection = Trade.Direction.None;

                            }
                            if (trade.OpenTrade.Reason == Trade.Trigger.OpenLong)
                            {
                                trade.CloseTrade.Reason = Trade.Trigger.CloseLong;
                                trade.CloseTrade.BuyorSell = Trade.BuySell.Sell;
                                trade.CloseTrade.TradeVolume = trade.OpenTrade.TradeVolume;
                                trade.CloseTrade.Position = false;
                                trade.CloseTrade.CurrentDirection = Trade.Direction.None;
                            }
                            break;
                        }
                    }
                }
            }

            foreach (var t in FullTradeList)
            {
                if (t.Notes == "reset")
                {
                    t.RunningProfit = 0;
                    t.Reason = Trade.Trigger.None;
                    t.Position = false;
                    t.CurrentDirection = Trade.Direction.None;
                    t.Notes = null;
                    t.BuyorSell = Trade.BuySell.None;
                    t.TradeVolume = 0;

                }
                //if (t.Reason == Trade.Trigger.CloseLong || t.Reason == Trade.Trigger.CloseShort)
                //{
                //    totlapl += (t.RunningProfit * t.TradeVolume);
                //    t.TotalPL = totlapl;
                //}


                //if (t.Reason != Trade.Trigger.None)//(t.TimeStamp.Year == 2012 && t.TimeStamp.Month == 6 && t.TimeStamp.Day > 15)
                //    Debug.WriteLine(t.TimeStamp + " " + t.CurrentPrice + "  " + t.Reason + " " + t.RunningProfit + " " + t.Notes + "  " + t.Position + "  " + t.CurrentDirection + "  " + t.TotalPL + "  " + t.TradeVolume + " " + t.TradedPrice);


            }



            return null;
        }



        internal class TestData
        {
            public DateTime Date { get; set; }
            public double MaxProfitAtIndex { get; set; }
            public int Index { get; set; }
            public List<Trade> TradeList { get; set; }
            public int Vol { get; set; }

        }


    }

}

