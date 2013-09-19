using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlsiUtils;
using AlsiUtils.Data_Objects;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

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
          
            var dateL = dc.MasterMinutes.AsEnumerable().Where(z => z.Instrument == ContractName);
            if (dateL.Count() == 0) throw new Exception("Contract name has no match in database.\n" + ContractName + " cannot be found");

            DateTime Last = dateL.Last().Stamp;
            DateTime Now = DateTime.UtcNow.AddHours(2);
            GlobalObjects.Points.Clear();
            GlobalObjects.Points = HiSat.HistData.GetHistoricalMINUTE_FromWEB(Last, Now, 1, ContractName);

            UpdatePricesToImportMinute();

        }

        public static void UpdatePricesToImportMinute()
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.ClearImportTable();
            dc.Clean_OHLC_Temp();
            DataTable MinData = new DataTable("MinData");
            MinData.Columns.Add("Stamp", typeof(DateTime));
            MinData.Columns.Add("O", typeof(int));
            MinData.Columns.Add("L", typeof(int));
            MinData.Columns.Add("H", typeof(int));
            MinData.Columns.Add("C", typeof(int));
            MinData.Columns.Add("V", typeof(int));
            MinData.Columns.Add("Instrument", typeof(string));

            foreach (var p in GlobalObjects.Points) MinData.Rows.Add(p.TimeStamp, p.Open, p.High, p.Low, p.Close, p.Volume, p.InstrumentName);


            #region BulkCopy

            DataSet minuteDataSet = new DataSet("minuteDataset");
            minuteDataSet.Tables.Add(MinData);
            SqlConnection myConnection = new SqlConnection(AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString);
            myConnection.Open();
            SqlBulkCopy bulkcopy = new SqlBulkCopy(myConnection);
            bulkcopy.DestinationTableName = "ImportMinute";
            bulkcopy.WriteToServer(MinData);
            Debug.WriteLine("Tick Bulk Copy Complete");
            MinData.Dispose();
            myConnection.Close();

            #endregion


            dc.UpadteImport();
            dc.CleanUp();

        }

        public static void MergeTempWithHisto(GlobalObjects.TimeInterval T)
        {


            var dc = new AlsiDBDataContext();
            switch (T)
            {
                case (GlobalObjects.TimeInterval.Minute_5):
                    //Must be 2000 prices in db for accurate calcs
                    if (dc.OHLC_5_Minutes.Count() < 20100) throw new IndexOutOfRangeException();
                    var last1000Prices = dc.OHLC_5_Minutes.Skip(Math.Max(0, dc.OHLC_5_Minutes.Count() - 20000)).Take(20000);
                    dc.Clean_OHLC_Temp_2();


                    DataTable MinData = new DataTable("MinData");
                    MinData.Columns.Add("Stamp", typeof(DateTime));
                    MinData.Columns.Add("O", typeof(int));
                    MinData.Columns.Add("L", typeof(int));
                    MinData.Columns.Add("H", typeof(int));
                    MinData.Columns.Add("C", typeof(int));
                

                    foreach (var p in last1000Prices) MinData.Rows.Add(p.Stamp, p.O, p.H, p.L, p.C);


                    #region BulkCopy

                    DataSet minuteDataSet = new DataSet("minuteDataset");
                    minuteDataSet.Tables.Add(MinData);
                    SqlConnection myConnection = new SqlConnection(AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString);
                    myConnection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(myConnection);
                    bulkcopy.DestinationTableName = "OHLC_Temp_2";
                    bulkcopy.WriteToServer(MinData);
                    Debug.WriteLine("Tick Bulk Copy Complete");
                    MinData.Dispose();
                    myConnection.Close();

                    #endregion

                    dc.MergeTemp();
                    Debug.WriteLine("Merged Data");
                    break;
            }


        }

        public static void _1MinDataToImportMinute(List<Price> MinutePrices)
        {
            var dc = new AlsiUtils.AlsiDBDataContext();
            dc.ClearImportTable();
            dc.Clean_OHLC_Temp();
            DataTable MinData = new DataTable("MinData");
            MinData.Columns.Add("Stamp", typeof(DateTime));
            MinData.Columns.Add("O", typeof(int));
            MinData.Columns.Add("L", typeof(int));
            MinData.Columns.Add("H", typeof(int));
            MinData.Columns.Add("C", typeof(int));
            MinData.Columns.Add("V", typeof(int));

            foreach (var p in MinutePrices) MinData.Rows.Add(p.TimeStamp, p.Open, p.High, p.Low, p.Close, p.Volume);


            #region BulkCopy

            DataSet minuteDataSet = new DataSet("minuteDataset");
            minuteDataSet.Tables.Add(MinData);
            SqlConnection myConnection = new SqlConnection(AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString);
            myConnection.Open();
            SqlBulkCopy bulkcopy = new SqlBulkCopy(myConnection);
            bulkcopy.DestinationTableName = "ImportMinute";
            bulkcopy.WriteToServer(MinData);
            Debug.WriteLine("Tick Bulk Copy Complete");
            MinData.Dispose();
            myConnection.Close();

            #endregion
        }

        public static void UpdatePricesToTempTable()
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.Connection.ConnectionString = GlobalObjects.CustomConnectionString;
            dc.Clean_OHLC_Temp();

            DataTable MinData = new DataTable("MinData");
            MinData.Columns.Add("Stamp", typeof(DateTime));
            MinData.Columns.Add("O", typeof(int));
            MinData.Columns.Add("L", typeof(int));
            MinData.Columns.Add("H", typeof(int));
            MinData.Columns.Add("C", typeof(int));
          

            foreach (var p in GlobalObjects.Points) MinData.Rows.Add(p.TimeStamp, p.Open, p.High, p.Low, p.Close);

            #region BulkCopy

            DataSet minuteDataSet = new DataSet("minuteDataset");
            minuteDataSet.Tables.Add(MinData);
            SqlConnection myConnection = new SqlConnection(AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString);
            myConnection.Open();
            SqlBulkCopy bulkcopy = new SqlBulkCopy(myConnection);
            bulkcopy.DestinationTableName = "OHLC_Temp";
            bulkcopy.WriteToServer(MinData);
            Debug.WriteLine("Tick Bulk Copy Complete");
            MinData.Dispose();
            myConnection.Close();

            #endregion

        }

        public static void ClearTradeLog()
        {
            var dc = new AlsiDBDataContext();
            var delAll = dc.TradeLogs;
            dc.TradeLogs.DeleteAllOnSubmit(delAll);
            dc.SubmitChanges();
        }

    }
}

