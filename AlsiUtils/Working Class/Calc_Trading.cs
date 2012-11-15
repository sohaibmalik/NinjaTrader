using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using Nist;

namespace AlsiUtils
{
    public class Calc_Trading
    {
        static public SummaryStats createSummaryStatistic(string detail, string value)
        {
            SummaryStats SS = new SummaryStats();
            SS.Detail = detail;
            SS.Value = value;

            return SS;
        }

     
        
        public static List<PointData> convertTickToMinute(List<Tick> TickData)
        {
            DateTime start = DateTime.Now;

            List<PointData> minuteData = new List<PointData>();
            List<Tick> tickData = TickData;
            //string path = Properties.Settings.Default.TickTxtFilePath;





            List<int> open = new List<int>();
            List<int> close = new List<int>();
            List<DateTime> minuteRawTime = new List<DateTime>();
            List<int> low = new List<int>();
            List<int> high = new List<int>();



            //OPEN
            //DateTime start_open = DateTime.Now;
            foreach (Tick p in tickData)
            {
                int yearT = p.Stamp.Year;
                int monthT = p.Stamp.Month;
                int dayT = p.Stamp.Day;
                int hourT = p.Stamp.Hour;
                int minuteT = p.Stamp.Minute;

                DateTime d = new DateTime(yearT, monthT, dayT, hourT, minuteT, 0);
                if (!minuteRawTime.Contains(d))
                {
                    minuteRawTime.Add(d);
                    open.Add(p.Price);
                }
            }
            //DateTime finish_open = DateTime.Now;
            //TimeSpan duration_open = finish_open - start_open;
            //Debug.WriteLine("[OPEN] Convertion Time Ticks to Minutes : " + duration_open.Seconds + ":" + duration_open.Milliseconds);


            //CLOSE
            //DateTime start_close = DateTime.Now;
            tickData.Reverse();
            minuteRawTime.Clear();
            foreach (Tick p in tickData)
            {
                int yearT = p.Stamp.Year;
                int monthT = p.Stamp.Month;
                int dayT = p.Stamp.Day;
                int hourT = p.Stamp.Hour;
                int minuteT = p.Stamp.Minute;

                DateTime d = new DateTime(yearT, monthT, dayT, hourT, minuteT, 0);
                if (!minuteRawTime.Contains(d))
                {
                    minuteRawTime.Add(d);
                    close.Add(p.Price);
                }
            }
            //DateTime finish_close = DateTime.Now;
            //TimeSpan duration_close = finish_open - start_open;
            //Debug.WriteLine("[Close] Convertion Time Ticks to Minutes : " + duration_close.Seconds + ":" + duration_close.Milliseconds);



            //HIGH+LOW
            DateTime start_HL = DateTime.Now;
            close.Reverse();
            minuteRawTime.Reverse();
            tickData.Reverse();



            int ind = minuteRawTime.Count - 1;

            for (int x = 0; x <= ind; x++)
            {

                int yy = minuteRawTime[x].Year;
                int mm = minuteRawTime[x].Month;
                int dy = minuteRawTime[x].Day;
                int hh = minuteRawTime[x].Hour;
                int mn = minuteRawTime[x].Minute;
                int ss = minuteRawTime[x].Second;

                DateTime dd = new DateTime(yy, mm, dy, hh, mn, ss);


                int min = 100000;
                int max = 1;
                foreach (Tick p in tickData)
                {
                    int yearT = p.Stamp.Year;
                    int monthT = p.Stamp.Month;
                    int dayT = p.Stamp.Day;
                    int hourT = p.Stamp.Hour;
                    int minuteT = p.Stamp.Minute;


                    if (dd.Year == yearT && dd.Month == monthT && dd.Day == dayT && dd.Hour == hourT && dd.Minute == minuteT)
                    {
                        if (p.Price > max) max = p.Price;
                        if (p.Price < min) min = p.Price;
                    }

                }
                low.Add(min);
                high.Add(max);

            }


            for (int x = 0; x <= ind; x++)
            {

                //Debug.WriteLine(minuteRawTime[x] + " Open " + open[x] + " Close " + close[x] + " High " + high[x] + " Low " + low[x]);
                PointData m = new PointData();
                m.Close = close[x];
                m.Open = open[x];
                m.High = high[x];
                m.Low = low[x];
                m.TimeStamp = minuteRawTime[x];
                minuteData.Add(m);
            }

            //DateTime finish_HL = DateTime.Now;
            //TimeSpan duration_HL = finish_HL - start_HL;
            //Debug.WriteLine("[HIGH LOW] Convertion Time Ticks to Minutes : " + duration_HL.Seconds + ":" + duration_HL.Milliseconds);

            //Debug.WriteLine(minuteData[0].TimeStamp + " Open : {0} High : {1} Low : {2}  Close : {3}", minuteData[0].Open, minuteData[0].High, minuteData[0].Low, minuteData[0].Close);

            DateTime finish = DateTime.Now;
            TimeSpan duration = finish - start;
            Debug.WriteLine("Convertion Time Ticks to Minutes : " + duration.Seconds + ":" + duration.Milliseconds);

            return minuteData;

        }
        
        /// <summary>
        /// Sync the Clock to Internet Time
        /// </summary>
        /// <returns>True if Successfull False if Failed</returns>
        static public bool RefreshTime()
        {
            bool synced = false;

            try
            {
                NistClock c = new NistClock();
                c.SynchronizeLocalClock();
                synced = true;
            }
            catch
            {
               
            }

            return synced;
        }
    }
}
