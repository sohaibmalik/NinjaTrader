using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlsiUtils;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using AlsiTrade_Backend.HiSat;
using System.Data;
using System.Data.SqlClient;
using Nist;
using Communicator;
using System.ComponentModel;
using AlsiUtils.Data_Objects;
using AlsiUtils;
using AlsiUtils.Strategies;
namespace AlsiTrade_Backend
{

    public class DoStuff
    {
        private static BackgroundWorker BW;

        public static void ExportToText(List<Trade> Trades)
        {

            SaveFileDialog fs = new SaveFileDialog();
            fs.ShowDialog();
            FileInfo f;
            try
            {
                f = new FileInfo(fs.FileName);
            }
            catch
            {
                return;
            }




            StreamWriter sr = new StreamWriter(f.FullName);
            foreach (var t in Trades)
            {
                sr.WriteLine(
                    t.TimeStamp + ","
                   + t.Reason + ","
                   + t.BuyorSell + ","
                   +t.TradeVolume +","
                   + t.CurrentDirection + ","
                   + t.CurrentPrice + ","
                   + t.Position + ","
                   + t.RunningProfit + ","
                   + t.TotalPL + ","
                   + t.InstrumentName
                    );
            }
            sr.Close();
            MessageBox.Show("Export Complete");
        }

        public static void TickBulkCopy(string InstrumentName, DateTime Start)
        {
            DateTime _start = DateTime.Now.AddDays(-5);
            DateTime UseThisDate = Start > _start ? _start : Start;
            AlsiDBDataContext dc = new AlsiDBDataContext();
            var tickData = HistData.GetHistoricalTICK_FromWEB(UseThisDate, DateTime.UtcNow.AddHours(2), InstrumentName);

            DataTable RawTicks = new DataTable("TickData");
            RawTicks.Columns.Add("N", typeof(long));
            RawTicks.Columns.Add("Stamp", typeof(DateTime));
            RawTicks.Columns.Add("Price", typeof(int));

            foreach (var p in tickData) RawTicks.Rows.Add(1, p.TimeStamp, p.Close);
            dc.CleanTick();

            #region BulkCopy

            DataSet tickDataSet = new DataSet("AllTicks");
            tickDataSet.Tables.Add(RawTicks);
            SqlConnection myConnection = new SqlConnection(AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString);
            myConnection.Open();
            SqlBulkCopy bulkcopy = new SqlBulkCopy(myConnection);
            bulkcopy.DestinationTableName = "RawTick";
            bulkcopy.WriteToServer(RawTicks);

            Debug.WriteLine("Tick Bulk Copy Complete");

            RawTicks.Dispose();
            myConnection.Close();
            dc.CleanUp();

            #endregion
        }

        public static List<Price> convertTickToMinute(List<AlsiUtils.RawTick> TickData)
        {
            DateTime start = DateTime.Now;

            List<Price> minuteData = new List<Price>();

            List<double> open = new List<double>();
            List<double> close = new List<double>();
            List<DateTime> minuteRawTime = new List<DateTime>();
            List<double> low = new List<double>();
            List<double> high = new List<double>();



            //OPEN
            //DateTime start_open = DateTime.Now;
            foreach (var p in TickData)
            {
                int yearT = p.Stamp.Value.Year;
                int monthT = p.Stamp.Value.Month;
                int dayT = p.Stamp.Value.Day;
                int hourT = p.Stamp.Value.Hour;
                int minuteT = p.Stamp.Value.Minute;

                DateTime d = new DateTime(yearT, monthT, dayT, hourT, minuteT, 0);
                if (!minuteRawTime.Contains(d))
                {
                    minuteRawTime.Add(d);
                    open.Add((double)p.Price);
                }
            }
            //DateTime finish_open = DateTime.Now;
            //TimeSpan duration_open = finish_open - start_open;
            //Debug.WriteLine("[OPEN] Convertion Time Ticks to Minutes : " + duration_open.Seconds + ":" + duration_open.Milliseconds);


            //CLOSE
            //DateTime start_close = DateTime.Now;
            TickData.Reverse();
            minuteRawTime.Clear();
            foreach (var p in TickData)
            {
                int yearT = p.Stamp.Value.Year;
                int monthT = p.Stamp.Value.Month;
                int dayT = p.Stamp.Value.Day;
                int hourT = p.Stamp.Value.Hour;
                int minuteT = p.Stamp.Value.Minute;

                DateTime d = new DateTime(yearT, monthT, dayT, hourT, minuteT, 0);
                if (!minuteRawTime.Contains(d))
                {
                    minuteRawTime.Add(d);
                    close.Add((double)p.Price);
                }
            }
            //DateTime finish_close = DateTime.Now;
            //TimeSpan duration_close = finish_open - start_open;
            //Debug.WriteLine("[Close] Convertion Time Ticks to Minutes : " + duration_close.Seconds + ":" + duration_close.Milliseconds);



            //HIGH+LOW
            DateTime start_HL = DateTime.Now;
            close.Reverse();
            minuteRawTime.Reverse();
            TickData.Reverse();



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


                double min = 100000;
                double max = 1;
                foreach (var p in TickData)
                {
                    int yearT = p.Stamp.Value.Year;
                    int monthT = p.Stamp.Value.Month;
                    int dayT = p.Stamp.Value.Day;
                    int hourT = p.Stamp.Value.Hour;
                    int minuteT = p.Stamp.Value.Minute;


                    if (dd.Year == yearT && dd.Month == monthT && dd.Day == dayT && dd.Hour == hourT && dd.Minute == minuteT)
                    {
                        if (p.Price > max) max = (double)p.Price;
                        if (p.Price < min) min = (double)p.Price;
                    }

                }
                low.Add(min);
                high.Add(max);

            }


            for (int x = 0; x <= ind; x++)
            {

                //Debug.WriteLine(minuteRawTime[x] + " Open " + open[x] + " Close " + close[x] + " High " + high[x] + " Low " + low[x]);
                Price m = new Price();
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

        public static Trade GetLastTrade(Parameter_EMA_Scalp Parameter, GlobalObjects.TimeInterval t)
        {
            var _Stats = new AlsiUtils.Statistics();
            var dt = DataBase.dataTable.MasterMinute;

            var _tempTradeList = AlsiTrade_Backend.RunCalcs.RunEMAScalp(Parameter, t, true, DateTime.Now.AddMonths(-1), DateTime.Now.AddHours(12), dt);
            var _trades = _Stats.CalcBasicTradeStats(_tempTradeList);
            _trades.Reverse();

            return _trades[0];
        }

        public static Trade Apply2ndAlgoVolume(List<Trade> Trades)
        {
            var NewTrades = AlsiUtils.Strategies.TradeStrategy.Expansion.ApplyRegressionFilter(11, Trades);            
            var Algo2nd_LastOrder = CompletedTrade.CreateList(NewTrades).Last();
            var CurrentTrade = Trades.Last();
            if (Algo2nd_LastOrder.TimeStamp == CurrentTrade.TimeStamp)
                CurrentTrade.TradeVolume = Algo2nd_LastOrder.TradeVolume;

            return CurrentTrade;
        }

        /// <summary>
        /// Sync the Clock to Internet Time
        /// </summary>
        /// <returns>True if Successfull False if Failed</returns>
        static public bool SynchronizeTime()
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

        #region Email

        private static Trade _EmailTrade;
        private static EmailMsg _Msg;
        public static void SendEmail(Trade trade, EmailMsg Msg)
        {
            _Msg = Msg;
            _EmailTrade = trade;
            BW = new BackgroundWorker();
            BW.DoWork += new DoWorkEventHandler(BW_DoWork);
            BW.RunWorkerAsync();
        }

        static void BW_DoWork(object sender, DoWorkEventArgs e)
        {
            SendMail();
        }

        #endregion
        private static void SendMail()
        {

            Gmail.SendEmail("pieterf33@gmail.com", _Msg.Title, _Msg.Body, null, "pietpiel27@gmail.com", "1rachelle", "Alsi Trade System", false);
        }

    }
}
