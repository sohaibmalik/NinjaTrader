using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;


namespace AlsiUtils
{


    public class DataBase
    {


        #region AlsiTrade


        public static void InsertLog(string SourceApp, string Subject, string Detail, Color LineColor)
        {
            try
            {
                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;
                Log l = new Log()
                {
                    SourceApp = SourceApp,
                    Time = DateTime.UtcNow.AddHours(2),
                    Subject = Subject,
                    Details = Detail,
                    Colour = LineColor.ToKnownColor().ToString()
                };
                dc.Logs.InsertOnSubmit(l);
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Cannot write Log");
                Debug.WriteLine(ex.Message);
            }





        }

        static public void InsertTradeLog(Trade TradeObject)
        {
            try
            {
                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;

                TradeLog l = new TradeLog
                {

                    Time = TradeObject.TimeStamp,
                    BuySell = TradeObject.BuyorSell,
                    Reason = TradeObject.TradeReason,
                    Price = (int)TradeObject.TradedPrice,
                    Volume = TradeObject.TradeVolume,
                    ForeColor = TradeObject.ForeColor.ToKnownColor().ToString(),
                    BackColor = TradeObject.BackColor.ToKnownColor().ToString()

                };
                dc.TradeLogs.InsertOnSubmit(l);
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Cannot write Log");
                Debug.WriteLine(ex.Message);
            }





        }

        static public List<LogData> GetLogFromDatabase(string Filter, int numberofLogs)
        {
            List<LogData> logdata = new List<LogData>();
            try
            {


                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;

                var count = (from p in dc.Logs
                             where SqlMethods.Like(Filter, p.SourceApp)
                             select (p.Time)).Count();


                var result = (from q in dc.Logs
                              where SqlMethods.Like(Filter, q.SourceApp)
                              orderby q.Time ascending
                              select new { q.Time, q.Subject, q.Details, q.Colour })
                                            .Skip(count - numberofLogs)
                                         .Take(numberofLogs);

                foreach (var v in result)
                {
                    LogData l = new LogData();
                    l.TimeStamp = v.Time;
                    l.Subject = v.Subject;
                    l.LogDetail = v.Details;
                    l.LineColor = Color.FromName(v.Colour);
                    logdata.Add(l);
                }

                return logdata;
            }
            catch (Exception ex)
            {

                return logdata;
            }

        }

        static public List<Trade> GetTradeLogFromDatabase(int numberofLogs)
        {
            List<Trade> logdata = new List<Trade>();
            try
            {


                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;

                var count = (from p in dc.TradeLogs
                             select (p.Time)).Count();


                var result = (from q in dc.TradeLogs
                              orderby q.Time ascending
                              select new { q.Time, q.BuySell, q.Reason, q.Price, q.Volume, q.ForeColor, q.BackColor })
                                            .Skip(count - numberofLogs)
                                         .Take(numberofLogs);

                foreach (var v in result)
                {
                    Trade l = new Trade();
                    l.TimeStamp = (DateTime)v.Time;
                    l.BuyorSell = v.BuySell;
                    l.TradeReason = v.Reason;
                    l.TradedPrice = (double)v.Price;
                    l.TradeVolume = (int)v.Volume;
                    l.ForeColor = Color.FromName(v.ForeColor);
                    l.BackColor = Color.FromName(v.BackColor);
                    logdata.Add(l);
                }

                return logdata;
            }
            catch (Exception ex)
            {

                return logdata;
            }

        }


        /// <summary>
        /// Use This Function To get All prices from Masterminute Historical Data..For Historical Clacs
        /// </summary>
        /// <param name="numberOfPeriods"></param>
        /// <param name="reverseList"></param>
        /// <returns></returns>
        static public List<Price> readDataFromDataBase_10_MIN_MasterMinute(int numberOfPeriods, bool reverseList)
        {
            try
            {
                List<Price> prices = new List<Price>();

                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;
                dc.OHLC_10();

                var count = (from p in dc.OHLC_10_Minutes
                             select (p.Stamp)).Count();

                //Console.WriteLine("10 Minute Data Count : " + count);


                var result = from q in dc.OHLC_10_Minutes
                                         .Skip(count - numberOfPeriods)
                                         .Take(numberOfPeriods)
                             select new { q.Stamp, q.O, q.H, q.L, q.C, q.Instrument };

                foreach (var v in result)
                {
                    Price p = new Price();
                    p.Close = v.C;
                    p.Open = v.O;
                    p.High = v.H;
                    p.Low = v.L;
                    p.TimeStamp = v.Stamp;
                    p.InstrumentName = v.Instrument;
                    prices.Add(p);

                }

                if (reverseList) prices.Reverse();
                return prices;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("readDataFromDataBase_10_MIN  Database Busy : " + ex.Message);
                return null;
            }
        }

        static public List<Price> readDataFromDataBase_5_MIN_MasterMinute(int numberOfPeriods, bool reverseList)
        {
            try
            {
                List<Price> prices = new List<Price>();

                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;
                dc.OHLC_5();

                var count = (from p in dc.OHLC_5_Minutes
                             select (p.Stamp)).Count();

                //Console.WriteLine("10 Minute Data Count : " + count);


                var result = from q in dc.OHLC_5_Minutes
                                         .Skip(count - numberOfPeriods)
                                         .Take(numberOfPeriods)
                             select new { q.Stamp, q.O, q.H, q.L, q.C, q.Instrument };

                foreach (var v in result)
                {
                    Price p = new Price();
                    p.Close = v.C;
                    p.Open = v.O;
                    p.High = v.H;
                    p.Low = v.L;
                    p.TimeStamp = v.Stamp;
                    p.InstrumentName = v.Instrument;
                    prices.Add(p);

                }

                if (reverseList) prices.Reverse();
                return prices;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("readDataFromDataBase_5_MIN  Database Busy : " + ex.Message);
                return null;
            }
        }

        static public List<Price> readDataFromDataBase_5_MIN_AllHistory(int numberOfPeriods, bool reverseList)
        {
            try
            {
                List<Price> prices = new List<Price>();

                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;
                dc.OHLC_5_AllHistory();

                var count = (from p in dc.OHLC_5_Minutes
                             select (p.Stamp)).Count();

                //Console.WriteLine("10 Minute Data Count : " + count);


                var result = from q in dc.OHLC_5_Minutes
                                         .Skip(count - numberOfPeriods)
                                         .Take(numberOfPeriods)
                             select new { q.Stamp, q.O, q.H, q.L, q.C, q.Instrument };

                foreach (var v in result)
                {
                    Price p = new Price();
                    p.Close = v.C;
                    p.Open = v.O;
                    p.High = v.H;
                    p.Low = v.L;
                    p.TimeStamp = v.Stamp;
                    p.InstrumentName = v.Instrument;
                    prices.Add(p);

                }

                if (reverseList) prices.Reverse();
                return prices;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("readDataFromDataBase_5_MIN  Database Busy : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Use This Function To get All Historical Data since 1997 ..For Historical Clacs
        /// </summary>
        /// <param name="numberOfPeriods"></param>
        /// <param name="reverseList">to get all data between Start and End : -1 else specify number of periods</param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns>List of 10 minute prices</returns>
        static public List<Price> readDataFromDataBase_10_MIN_ALLHistory(int numberOfPeriods, bool reverseList)
        {
           
            try
            {
                List<Price> prices = new List<Price>();
                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;
                dc.OHLC_10_AllHistory();

                var count = (from p in dc.OHLC_10_Minutes
                             select (p.Stamp)).Count();

                //Console.WriteLine("10 Minute Data Count : " + count);

                

                var result = from q in dc.OHLC_10_Minutes
                                         .Skip(count - numberOfPeriods)
                                         .Take(numberOfPeriods)
                             select new { q.Stamp, q.O, q.H, q.L, q.C, q.Instrument };

                foreach (var v in result)
                {
                    Price p = new Price();
                    p.Close = v.C;
                    p.Open = v.O;
                    p.High = v.H;
                    p.Low = v.L;
                    p.TimeStamp = v.Stamp;
                    p.InstrumentName = v.Instrument;
                    prices.Add(p);

                }

                if (reverseList) prices.Reverse();
                return prices;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("readDataFromDataBase_10_MIN  Database Busy : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Use This Function To get Temp Data For Trading
        /// </summary>
        /// <param name="numberOfPeriods"></param>
        /// <param name="reverseList"></param>
        /// <returns></returns>
        static public List<Price> readDataFromDataBase_10_MIN_TempTable(bool reverseList)
        {
            try
            {
                List<Price> prices = new List<Price>();

                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;

                //var count = (from p in dc.OHLC_10_Minutes
                //             select (p.Stamp)).Count();

                //Console.WriteLine("10 Minute Data Count : " + count);


                var result = from q in dc.OHLC_10_Minute_Temps
                             select new { q.Stamp, q.O, q.H, q.L, q.C };




                foreach (var v in result)
                {
                    Price p = new Price();
                    p.Close = v.C;
                    p.Open = v.O;
                    p.High = v.H;
                    p.Low = v.L;
                    p.TimeStamp = v.Stamp;
                    p.InstrumentName = "Temp Instrument";
                    prices.Add(p);
                }

                if (reverseList) prices.Reverse();
                return prices;



            }
            catch (Exception ex)
            {
                Debug.WriteLine("readDataFromDataBase_10_MIN_TEMP  Database Busy : " + ex.Message);
                return null;
            }
        }

        static public List<Price> readDataFromDataBase_1_MIN_MasterMinute(int numberOfPeriods, bool reverseList)
        {
            try
            {
                List<Price> prices = new List<Price>();

                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;
               

                var count = (from p in dc.MasterMinutes
                             select (p.Stamp)).Count();

                //Console.WriteLine("10 Minute Data Count : " + count);


                var result = from q in dc.MasterMinutes
                                         .Skip(count - numberOfPeriods)
                                         .Take(numberOfPeriods)
                             select new { q.Stamp, q.O, q.H, q.L, q.C, q.Instrument };

                foreach (var v in result)
                {
                    Price p = new Price();
                    p.Close = v.C;
                    p.Open = v.O;
                    p.High = v.H;
                    p.Low = v.L;
                    p.TimeStamp = v.Stamp;
                    p.InstrumentName = v.Instrument;
                    prices.Add(p);

                }

                if (reverseList) prices.Reverse();
                return prices;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("readDataFromDataBase_10_MIN  Database Busy : " + ex.Message);
                return null;
            }
        }

        
        static public void get_10min_high_low_from_ticks(out double High, out double Low)
        {
            try
            {
                //get 10 min high

                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;

                var last = (from q in dc.OHLC_10_Minutes
                            orderby q.Stamp descending
                            select new { q.Stamp, q.L }).Take(2);


                List<DateTime> dt = new List<DateTime>();


                foreach (var v in last)
                {
                    dt.Add(v.Stamp);
                    //Debug.WriteLine("LAST : " + v);

                }



                var high = (from q in dc.RawTicks
                            where q.Stamp > dt[0]
                            select q.Price).Max();

                var low = (from q in dc.RawTicks
                           where q.Stamp > dt[0]
                           select q.Price).Min();

                //Debug.WriteLine("High From Tick " + high);
                //Debug.WriteLine("Low From Tick " + low);

                High = Convert.ToDouble(high);
                Low = Convert.ToDouble(low);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("get_10min_high_low_from_ticks DATA BASE BUSY : " + ex.Message);
                High = 0;
                Low = 0;
            }
        }

        static public void get_10min_high_low_from_ticks_Prev_TimeFrame(out double High, out double Low)
        {
            try
            {
                //get 10 min high

                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;

                var last = (from q in dc.OHLC_10_Minutes
                            orderby q.Stamp descending
                            select new { q.Stamp, q.L, q.H, q.C }).Take(2);


                List<DateTime> dt = new List<DateTime>();

                High = 0;
                Low = 0;
                foreach (var v in last)
                {
                    dt.Add(v.Stamp);
                    //Debug.WriteLine(v.Stamp + "  Last " + v.C + "  High : " + v.H + "  Low : " + v.L);
                    High = Convert.ToDouble(v.H);
                    Low = Convert.ToDouble(v.L);
                }




            }
            catch (Exception ex)
            {
                Debug.WriteLine("get_10min_high_low_from_ticks DATA BASE BUSY : " + ex.Message);
                High = 0;
                Low = 0;
            }
        }
        #endregion


        #region DataManager

        static public DateTime getLastUpToDate()
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;
            dc.OHLC_10();

            var count = (from p in dc.OHLC_10_Minutes
                         select (p.Stamp)).Count();

            //Debug.WriteLine("10 Minute Data Count : " + count);


            var lastdate = from q in dc.OHLC_10_Minutes
                                     .Skip(count - 1)
                                     .Take(1)
                           select new { q.Stamp };

            DateTime last = new DateTime();

            foreach (var v in lastdate)
            {
                last = v.Stamp;
            }


            return last;

        }

        static public List<Tick> readDataFromDataBase_Tick(int numberOfPeriods, bool reverseList, out int DbRecordCount)
        {
            try
            {
                List<Tick> prices = new List<Tick>();

                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;
                var count = (from p in dc.RawTicks
                             select (p.Stamp)).Count();


                //Debug.WriteLine("1 Minute Data Count : " + count);
                DbRecordCount = count;

                var result = from q in dc.RawTicks
                                         .Skip(count - numberOfPeriods)
                                         .Take(numberOfPeriods)
                             orderby q.Stamp ascending
                             select new { q.Stamp, q.Price };

                foreach (var v in result)
                {
                    Tick p = new Tick();
                    p.Stamp = Convert.ToDateTime(v.Stamp);
                    p.Price = Convert.ToInt32(v.Price);

                    prices.Add(p);
                }

                if (reverseList) prices.Reverse();
                return prices;
            }
            catch (Exception ex)
            {
                DbRecordCount = 0;
                return null;
            }
        }

        static public List<Tick> readDataFromDataBase_Tick(bool reverseList, out int DbRecordCount)
        {
            try
            {
                List<Tick> prices = new List<Tick>();

                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;
                var count = (from p in dc.RawTicks
                             select (p.Stamp)).Count();



                DbRecordCount = count;

                var result = from q in dc.RawTicks
                             orderby q.Stamp ascending
                             select new { q.Stamp, q.Price };

                foreach (var v in result)
                {
                    Tick p = new Tick();
                    p.Stamp = Convert.ToDateTime(v.Stamp);
                    p.Price = Convert.ToInt32(v.Price);

                    prices.Add(p);
                }

                if (reverseList) prices.Reverse();
                return prices;
            }
            catch (Exception ex)
            {
                DbRecordCount = 0;
                return null;
            }
        }

        static public List<Tick> readDataFromDataBase_Tick(bool reverseList, out int DbRecordCount, DateTime StartTime)
        {
            try
            {
                List<Tick> prices = new List<Tick>();

                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;
                var count = (from p in dc.RawTicks
                             where p.Stamp > StartTime
                             select (p.Stamp)).Count();



                DbRecordCount = count;

                var result = from q in dc.RawTicks
                             where q.Stamp > StartTime
                             orderby q.Stamp ascending
                             select new { q.Stamp, q.Price };

                foreach (var v in result)
                {
                    Tick p = new Tick();
                    p.Stamp = Convert.ToDateTime(v.Stamp);
                    p.Price = Convert.ToInt32(v.Price);

                    prices.Add(p);
                }

                if (reverseList) prices.Reverse();
                return prices;
            }
            catch (Exception ex)
            {
                DbRecordCount = 0;
                return null;
            }
        }

        static public List<PointData> readDataFromDataBase_1Minute(int numberOfPeriods, bool reverseList)
        {
            List<PointData> prices = new List<PointData>();

            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;
            var count = (from p in dc.MasterMinutes
                         select (p.Stamp)).Count();

            Debug.WriteLine("1 Minute Data Count : " + count);


            var result = from q in dc.MasterMinutes
                                     .Skip(count - numberOfPeriods)
                                     .Take(numberOfPeriods)
                         select new { q.Stamp, q.O, q.H, q.L, q.C, q.V, q.Instrument };

            foreach (var v in result)
            {
                PointData p = new PointData();
                p.TimeStamp = v.Stamp;
                p.Close = v.C;
                p.Open = v.O;
                p.High = v.H;
                p.Low = v.L;
                p.Volume = Convert.ToInt32(v.V);
                p.InstrumentName = v.Instrument;
                prices.Add(p);
            }

            if (reverseList) prices.Reverse();
            return prices;
        }

        static public List<PointData> readDataFromDataBase_10Minute(int numberOfPeriods, bool reverseList)
        {
            List<PointData> prices = new List<PointData>();

            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;
            dc.OHLC_10();
            var count = (from p in dc.OHLC_10_Minutes
                         select (p.Stamp)).Count();

            Debug.WriteLine("10 Minute Data Count : " + count);


            var result = from q in dc.OHLC_10_Minutes
                                     .Skip(count - numberOfPeriods)
                                     .Take(numberOfPeriods)
                         select new { q.Stamp, q.O, q.H, q.L, q.C, q.Instrument };

            foreach (var v in result)
            {
                PointData p = new PointData();
                p.TimeStamp = v.Stamp;
                p.Close = v.C;
                p.Open = v.O;
                p.High = v.H;
                p.Low = v.L;
                p.InstrumentName = v.Instrument;
                prices.Add(p);
            }

            if (reverseList) prices.Reverse();
            return prices;
        }

        static public List<PointData> readDataFromDataBase_10Minute_Temp(bool reverseList)
        {
            List<PointData> prices = new List<PointData>();

            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;

            var result = from q in dc.OHLC_10_Minute_Temps
                         select new { q.Stamp, q.O, q.H, q.L, q.C };

            foreach (var v in result)
            {
                PointData p = new PointData();
                p.TimeStamp = v.Stamp;
                p.Close = v.C;
                p.Open = v.O;
                p.High = v.H;
                p.Low = v.L;

                prices.Add(p);
            }

            if (reverseList) prices.Reverse();
            return prices;
        }



        /// <summary>
        /// Insert Ticks into RAwTICK Table
        /// </summary>
        /// <param name="Stamp">Single TimeStamp</param>
        /// <param name="Price">Singe Price</param>
        static public void insertTicks(DateTime Stamp, int Price)
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;
            int p = Price;

            RawTick c = new RawTick
            {
                Stamp = Stamp,
                Price = p

            };


            dc.RawTicks.InsertOnSubmit(c);
            dc.SubmitChanges();

        }

        static public void insertMinuteDataToMinteImport(List<PointData> MinuteData)
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;
            dc.ClearImportTable();
            foreach (PointData p in MinuteData)
            {
                ImportMinute i = new ImportMinute()
                {
                    Stamp = p.TimeStamp,
                    O = p.Open,
                    H = p.High,
                    L = p.Low,
                    C = p.Close,
                    V = p.Volume

                };


                dc.ImportMinutes.InsertOnSubmit(i);
                dc.SubmitChanges();
            }


        }

        static public List<LogData> GetLogFromDatabase(int numberofLogs)
        {
            List<LogData> logdata = new List<LogData>();
            try
            {


                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;

                var count = (from p in dc.Logs
                             where SqlMethods.Like("Data Management", p.SourceApp)
                             select (p.Time)).Count();


                var result = (from q in dc.Logs
                              where SqlMethods.Like("Data Management", q.SourceApp)
                              orderby q.Time ascending
                              select new { q.Time, q.Subject, q.Details, q.Colour })
                                            .Skip(count - numberofLogs)
                                         .Take(numberofLogs);

                foreach (var v in result)
                {
                    LogData l = new LogData();
                    l.TimeStamp = v.Time;
                    l.Subject = v.Subject;
                    l.LogDetail = v.Details;
                    l.LineColor = Color.FromName(v.Colour);
                    logdata.Add(l);
                }

                return logdata;
            }
            catch (Exception ex)
            {

                return logdata;
            }

        }

        public static void InsertDebugLog(DateTime time, string DebugMsg, string SenderApp)
        {
            try
            {
                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;

                DebugLog l = new DebugLog()
                {
                    DateTime = time,
                    Sender = SenderApp,
                    Debug = DebugMsg

                };
                dc.DebugLogs.InsertOnSubmit(l);
                dc.SubmitChanges();
            }
            catch { }
        }

        #endregion


    }
}
