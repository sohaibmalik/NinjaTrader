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
            DateTime Now = DateTime.UtcNow.AddHours(2);          
            GlobalObjects.Points.Clear();
            GlobalObjects.Points = HiSat.HistData.GetHistoricalMINUTE_FromWEB(Last, Now, 1, ContractName);
          
            UpdatePricesToImportMinute();

        }

        public static void UpdatePricesToImportMinute()
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.ClearImportTable();
            decimal progress = 0;
            decimal totProgress = GlobalObjects.Points.Count;

            foreach (Price price in GlobalObjects.Points)
            {
                int open = (int)price.Open;
                int high = (int)price.High;
                int low = (int)price.Low;
                int close = (int)price.Close;
                int volume = (int)price.Volume;

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

        public static void UpdatePricesToImportMinuteForTradeUpdate()
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.ClearImportTable();
            decimal progress = 0;
            decimal totProgress = GlobalObjects.Points.Count;

            foreach (Price price in GlobalObjects.Points)
            {
                int open = (int)price.Open;
                int high = (int)price.High;
                int low = (int)price.Low;
                int close = (int)price.Close;
                int volume = (int)price.Volume;

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
            dc.CleanUp();

        }

        public static void UpdatePricesToTempTable()
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.Connection.ConnectionString = GlobalObjects.CustomConnectionString;
            dc.ClearTempTable();
            decimal progress = 0;
            decimal totProgress = GlobalObjects.Points.Count;

            foreach (Price price in GlobalObjects.Points)
            {
                int open = (int)price.Open;
                int high = (int)price.High;
                int low = (int)price.Low;
                int close = (int)price.Close;
                int volume = (int)price.Volume;

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
