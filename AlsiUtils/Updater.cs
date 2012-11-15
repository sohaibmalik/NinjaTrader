using System;
using System.Diagnostics;
using AlsiUtils.Data_Objects;

namespace AlsiUtils
{
    public class Updater
    {
        /// <summary>
        /// Full DataBase Update from Webdata
        /// </summary>
        /// <param name="ContractName">Contract name : example "DEC12ALSI"</param>
        /// <param name="CustomConnectionString">Custom Connection String, this will set Current Connection String</param>
        public static void FullHistoricUpdate(string ContractName, string CustomConnectionString)
        {
            GlobalObjects.CustomConnectionString = CustomConnectionString;
            DateTime Last = DataBase.getLastUpToDate();
            DebugClass.WriteLine(Last);
            DateTime Now = DateTime.UtcNow.AddHours(2);
            DebugClass.WriteLine("FULL UPDATE");
            DebugClass.WriteLine("Start Date " + Last.Date + "   End Date " + Now.Date);
            Debug.WriteLine("FULL UPDATE");
            Debug.WriteLine("Start Date " + Last.Date + "   End Date " + Now.Date);
            GlobalObjects.Prices.Clear();
            GlobalObjects.Prices = WebData.getHistoricalMinute_FromWEB(Last, Now, 1, ContractName);
            UpdatePricesToImportMinute();

        }

        private static void UpdatePricesToImportMinute()
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.Connection.ConnectionString = GlobalObjects.CustomConnectionString;
            dc.ClearImportTable();
            decimal progress = 0;
            decimal totProgress = GlobalObjects.Prices.Count;

            foreach (PointData price in GlobalObjects.Prices)
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
            GlobalObjects.Prices.Clear();
            dc.UpadteImport();
            dc.CleanUp();

        }


    }
}
