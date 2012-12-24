using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlsiUtils;
using AlsiUtils.Data_Objects;
using System.Diagnostics;

namespace AlsiTrade_Backend
{
    public class UpdateDB
    {
        /// <summary>
        /// Full DataBase Update from Webdata
        /// </summary>
        /// <param name="ContractName">Contract name : example "DEC12ALSI"</param>
        /// <param name="CustomConnectionString">Custom Connection String, this will set Current Connection String</param>
        public static void FullHistoricUpdate_MasterMinute(string ContractName)
        {
           
            AlsiDBDataContext dc = new AlsiDBDataContext();

            DateTime Last = dc.MasterMinutes.AsEnumerable().Last().Stamp;
            DebugClass.WriteLine(Last);
            DateTime Now = DateTime.UtcNow.AddHours(2);
            DebugClass.WriteLine("FULL UPDATE");
            DebugClass.WriteLine("Start Date " + Last.Date + "   End Date " + Now.Date);
            Debug.WriteLine("FULL UPDATE");
            Debug.WriteLine("Start Date " + Last.Date + "   End Date " + Now.Date);
            GlobalObjects.Points.Clear();
            GlobalObjects.Points = HiSat.HistData.GetHistoricalMINUTE_FromWEB(Last, Now, 1, ContractName);
            UpdatePricesToImportMinute();

        }

        public static void UpdatePricesToImportMinute()
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.Connection.ConnectionString = GlobalObjects.CustomConnectionString;
            dc.ClearImportTable();
            decimal progress = 0;
            decimal totProgress = GlobalObjects.Points.Count;

            foreach (PointData price in GlobalObjects.Points)
            {
                int open = price.Open;
                int high = price.High;
                int low = price.Low;
                int close = price.Close;
                int volume = price.Volume;

                ImportMinute c = new ImportMinute
                {
                    Stamp = price.TimeStamp,
                    O = open,
                    H = high,
                    L = low,
                    C = close,
                    V = volume,
                    Instrument = price.InstrumentName
                };


                dc.ImportMinutes.InsertOnSubmit(c);
                dc.SubmitChanges();
                progress++;

                int p = Convert.ToInt16(100 * (progress / totProgress));



            }
            GlobalObjects.Points.Clear();
            dc.UpadteImport();
            dc.CleanUp();

        }

        public static void UpdatePricesToTempTable()
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.Connection.ConnectionString = GlobalObjects.CustomConnectionString;
            dc.ClearTempTable();
            decimal progress = 0;
            decimal totProgress = GlobalObjects.Points.Count;

            foreach (PointData price in GlobalObjects.Points)
            {
                int open = price.Open;
                int high = price.High;
                int low = price.Low;
                int close = price.Close;
                int volume = price.Volume;

                OHLC_Temp  c = new OHLC_Temp
                {
                    Stamp = price.TimeStamp,
                    O = open,
                    H = high,
                    L = low,
                    C = close,                  
                };


                dc.OHLC_Temps.InsertOnSubmit(c);
                dc.SubmitChanges();
                progress++;

                int p = Convert.ToInt16(100 * (progress / totProgress));



            }
            GlobalObjects.Points.Clear();
           
            

        }
    }
}
