using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AlsiUtils;

namespace NinjaTest
{
    public partial class Form2 : Form
    {
       
       
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
          

            string f = @"D:\alsiHist.txt";
            double lineCount = File.ReadLines(f).Count();
            progressBar1.Value = 10;
            long count = 0;
            using (StreamReader r = new StreamReader(f))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    if (count > 0) stringtoPrice(line);
                    count++;
                }
            }
            progressBar1.Value = 50;
            insertBW.RunWorkerAsync();
        }

        private void BulkInsert()
        {
            #region Create Data Table in Memory

            DataTable RawTicks = new DataTable("TickData");
            //RawTicks.Columns.Add("N", typeof(int));
            RawTicks.Columns.Add("Stamp", typeof(DateTime));
            RawTicks.Columns.Add("O", typeof(int));
            RawTicks.Columns.Add("H", typeof(int));
            RawTicks.Columns.Add("L", typeof(int));
            RawTicks.Columns.Add("C", typeof(int));
            RawTicks.Columns.Add("V", typeof(int));
            RawTicks.Columns.Add("Instrument", typeof(string));
            insertBW.ReportProgress(60);
            #endregion

            #region Populate data Table


            foreach (Price p in AllHisto) RawTicks.Rows.Add(p.TimeStamp, p.Open, p.High, p.Low, p.Close, p.Volume, p.InstrumentName);

            insertBW.ReportProgress(70);
            #endregion


            #region Clear SQl Table
            // currentTaskStatusLabel.Text = "Writing to Data Base";
            //PC
            string ccc = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = ccc;
            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.Connection.ConnectionString = ccc;
           // dc.CleanTick();

            #endregion

            #region Bulk copy to SQL
            DataSet tickDataSet = new DataSet("AllTicks");
            tickDataSet.Tables.Add(RawTicks);
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString =ccc;
            myConnection.Open();
            SqlBulkCopy bulkcopy = new SqlBulkCopy(myConnection);
            bulkcopy.BulkCopyTimeout = 10000000;
            bulkcopy.DestinationTableName = "ImportMinute";
            bulkcopy.WriteToServer(RawTicks);
            //Debug.WriteLine("Ticks Written To DATASET " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            insertBW.ReportProgress(100);

            #endregion

            #region Cleanup
            RawTicks.Dispose();
            myConnection.Close();
            //dc.CleanUp();
            #endregion
        }

        private List<Price> AllHisto = new List<Price>();
        private void stringtoPrice(string l)
        {

            var sp = l.Split(',');

            int day = int.Parse(sp[0].ToString().Substring(0, 2));
            int month = int.Parse(sp[0].ToString().Substring(3, 2));
            int year = int.Parse(sp[0].ToString().Substring(6, 4));
            int hour = int.Parse(sp[1].ToString().Substring(0, 2));
            int min = int.Parse(sp[1].ToString().Substring(3, 2));
            var open = double.Parse(sp[2].ToString());
            var high = double.Parse(sp[3].ToString());
            var low = double.Parse(sp[4].ToString());
            var close = double.Parse(sp[5].ToString());
            var vol = int.Parse(sp[6].ToString());

            AlsiUtils.Price p = new Price();
            DateTime dt = new DateTime(year, month, day, hour, min, 0);
            p.TimeStamp = dt.AddMinutes(-1);
            p.Open = open;
            p.High = high;
            p.Low = low;
            p.Close = close;
            p.Volume = vol;
            p.InstrumentName = "Hist";
            AllHisto.Add(p);



        }

        private void insertBW_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BulkInsert();
        }

        private void insertBW_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void insertBW_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("DONE");
        }



    }
}
