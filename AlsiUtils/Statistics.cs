using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using AlsiUtils.Indicators;
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

        public static List<Trade> RegressionAnalysis_OnPL(int N, List<Trade> Trades)
        {
            List<Indicators.VariableIndicator> V = new List<Indicators.VariableIndicator>();
            var tradeonlyList = Trades.Where(z => z.Reason == Trade.Trigger.CloseLong || z.Reason == Trade.Trigger.CloseShort);
            foreach (var v in tradeonlyList)
            {
                var varindicator = new Indicators.VariableIndicator()
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

        public static List<CompletedTrade> TakeProfit_Exiguous_SlowStoch(List<Trade> FullTradeList, int FK, int SK, int D)
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


                var trade = new CompletedTrade();
                trade.OpenTrade = t.OpenTrade;
                                              
                    var period = SlowStoch.Where(ss => ss.TimeStamp == timeframe.TimeStamp);

                    bool TookProfitLong = false;
                    bool TookProfitShort = false;
                    DateTime lowJumpUp = ts.TimeStamp;
                    DateTime highDipDown = ts.TimeStamp;

                    if (t.OpenTrade.Reason == Trade.Trigger.OpenLong)
                    {
                        bool touchedHigh = period.Any(z => z.Slow_D > 85);
                        DateTime highTime;

                        bool dipped;

                        if (touchedHigh)
                        {
                            highTime = period.Where(z => z.Slow_D > 85).First().TimeStamp;
                            var x = period.Where(z => z.TimeStamp > highTime && z.Slow_D < 85);
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
                        var touchedLow = period.Any(z => z.Slow_D < 15);
                        DateTime lowTime;

                        bool jumped;
                        if (touchedLow)
                        {
                            lowTime = period.Where(z => z.Slow_D < 15).First().TimeStamp;
                            var x = period.Where(z => z.TimeStamp > lowTime && z.Slow_D > 5);
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
                      
                    }
                    else
                        if (TookProfitShort)
                        {
                            trade.CloseTrade.TimeStamp = lowJumpUp;
                        }
                        else
                            trade.CloseTrade = t;
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

    }
}
