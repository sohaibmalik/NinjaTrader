using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using AlsiUtils.Data_Objects;

namespace AlsiUtils
{


    public class DataBase
    {
        public static void SetConnectionString(string ConnectionString)
        {
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = ConnectionString;
        }


        public static void SetConnectionString()
        {

            //Laptop
            // string css = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            //PC
            string css = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";

            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = css;

        }

        public static bool TestSqlConnection(string CCS)
        {
            bool alive = true;
            try
            {
                AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = CCS;
                AlsiDBDataContext dc = new AlsiDBDataContext();
                dc.Connection.ConnectionString = CCS;
                dc.Connection.Open();
            }
            catch
            { alive = false; }
            return alive;
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
                    BuySell = TradeObject.BuyorSell.ToString(),
                    Reason = TradeObject.Reason.ToString(),
                    Notes = TradeObject.IndicatorNotes.ToString(),
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
                    // l.BuyorSell = v.BuySell;
                    // l.TradeReason = v.Reason;
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

        static public List<Price> readDataFromDataBase(GlobalObjects.TimeInterval T, dataTable TD, DateTime Start, DateTime End, bool reverseList)
        {

            List<Price> prices = new List<Price>();

            AlsiDBDataContext dc = new AlsiDBDataContext();


            if (TD == dataTable.Temp)
            {
                var result = from q in dc.OHLC_Temps
                             where q.Stamp > Start && q.Stamp < End
                             select new { q.Stamp, q.O, q.H, q.L, q.C, };

                foreach (var v in result)
                {
                    Price p = new Price();
                    p.Close = v.C;
                    p.Open = v.O;
                    p.High = v.H;
                    p.Low = v.L;
                    p.TimeStamp = v.Stamp;
                    prices.Add(p);

                }
                dc.Clean_OHLC_Temp();
                if (reverseList) prices.Reverse();
                return prices;
            }

            if (TD == dataTable.AllHistory)
            {
                if (T == GlobalObjects.TimeInterval.Minute_2)
                {
                    var firstinDB = dc.OHLC_2_Minutes.AsEnumerable().First().Stamp;
                    var lastinDB = dc.OHLC_2_Minutes.AsEnumerable().Last().Stamp;
                    if (firstinDB > Start) dc.OHLC_2_AllHistory();
                    if (lastinDB > End) dc.OHLC_2_AllHistory();
                }
                if (T == GlobalObjects.TimeInterval.Minute_5)
                {
                    var firstinDB = dc.OHLC_5_Minutes.AsEnumerable().First().Stamp;
                    var lastinDB = dc.OHLC_5_Minutes.AsEnumerable().Last().Stamp;
                    if (firstinDB > Start) dc.OHLC_5_AllHistory();
                    if (lastinDB < End) dc.OHLC_5_AllHistory();

                }
                if (T == GlobalObjects.TimeInterval.Minute_10)
                {
                    var firstinDB = dc.OHLC_10_Minutes.AsEnumerable().First().Stamp;
                    var lastinDB = dc.OHLC_10_Minutes.AsEnumerable().Last().Stamp;
                    if (firstinDB > Start) dc.OHLC_10_AllHistory();
                    if (lastinDB < End) dc.OHLC_10_AllHistory();
                }
            }
            if (TD == dataTable.MasterMinute)
            {
                if (T == GlobalObjects.TimeInterval.Minute_2)
                {
                    var min2 = dc.OHLC_2_Minutes;
                    if (min2.Count() == 0)
                    {
                        dc.OHLC_2();
                    }
                    else
                    {
                        var firstinDB = dc.OHLC_2_Minutes.AsEnumerable().First().Stamp;
                        var lastinDB = dc.OHLC_2_Minutes.AsEnumerable().Last().Stamp;
                        if (firstinDB > Start) dc.OHLC_2();
                        if (lastinDB < End) dc.OHLC_2();
                    }
                }

                if (T == GlobalObjects.TimeInterval.Minute_5)
                {
                    var min5 = dc.OHLC_5_Minutes;
                    if (min5.Count() == 0)
                    {
                        dc.OHLC_5();
                    }
                    else
                    {
                        var firstinDB = min5.AsEnumerable().First().Stamp;
                        var lastinDB = min5.AsEnumerable().Last().Stamp;
                        if (firstinDB > Start) dc.OHLC_5();
                        if (lastinDB < End) dc.OHLC_5();
                    }
                }

                if (T == GlobalObjects.TimeInterval.Minute_10)
                {
                    var min10 = dc.OHLC_10_Minutes;
                    if (min10.Count() == 0)
                    {
                        dc.OHLC_10();
                    }
                    else
                    {
                        var firstinDB = dc.OHLC_10_Minutes.AsEnumerable().First().Stamp;
                        var lastinDB = dc.OHLC_10_Minutes.AsEnumerable().Last().Stamp;
                        if (firstinDB > Start) dc.OHLC_10();
                        if (lastinDB < End) dc.OHLC_10();
                    }
                }
            }





            if (T == GlobalObjects.TimeInterval.Minute_2)
            {
                var result = from q in dc.OHLC_2_Minutes
                             where q.Stamp > Start && q.Stamp < End
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
            }

            if (T == GlobalObjects.TimeInterval.Minute_5)
            {
                var result = from q in dc.OHLC_5_Minutes
                             where q.Stamp > Start && q.Stamp < End
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
            }
            if (T == GlobalObjects.TimeInterval.Minute_10)
            {
                var result = from q in dc.OHLC_10_Minutes
                             where q.Stamp > Start && q.Stamp < End
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
            }






            if (reverseList) prices.Reverse();
            return prices;


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








        static public void insertTicks(DateTime Stamp, int Price)
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();
            int p = Price;

            RawTick c = new RawTick
            {
                Stamp = Stamp,
                Price = p

            };


            dc.RawTicks.InsertOnSubmit(c);
            dc.SubmitChanges();

        }




        public enum dataTable
        {
            Temp,
            MasterMinute,
            AllHistory,
        }

    }
}
