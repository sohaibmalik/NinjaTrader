using AlsiUtils;
using AlsiUtils.Data_Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AlgoSecondLayer
{
    public class Utils
    {
        public static void PrintAllProperties(object obj)
        {
            Console.WriteLine("===========Print new object====================");
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(obj);
                Console.WriteLine("{0}={1}", name, value);
            }
            Console.WriteLine("===============================================");
        }

        public static void Fix()
        {

            string SIMcontext = @"Data Source=85.214.244.19;Initial Catalog=ALSI_SIM;User ID=SimLogin;Password=boeboe;MultipleActiveResultSets=True";
            var dc = new AlsiSimDataContext(SIMcontext);

            foreach (var v in dc.tblResult_5Min_SSPOPs)
            {
                var c = v.Sequence.IndexOf(',');
                var x = v.Sequence.Substring(c + 1);
                var q = new tblResult_5Min_SSPOP()
                {
                    Notes = "New",
                    Sequence = x,
                    Profit = v.Profit,

                };
                try
                {
                    dc.tblResult_5Min_SSPOPs.InsertOnSubmit(q);
                    dc.SubmitChanges();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Skipping " + q.Sequence);
                }
            }
        }

        public static void CountPermutations()
        {


            #region Full
            //var sr = new StreamWriter(@"d:\seq.txt");

            //for (int fastK = 5; fastK < 30; fastK+=1)
            //    for (int slowK = 5; slowK < 30; slowK+=1)
            //        for (int slowD = 5; slowD < 30; slowD+=1)
            //            for (int u_75 = 65; u_75 < 85; u_75+=5)
            //                for (int l_25 = 15; l_25 < 45; l_25+=5)
            //                    for (int lim_high = 70; lim_high < 90; lim_high+=5)
            //                        for (int lim_low = 10; lim_low < 30; lim_low+=5)
            //                            for (int stoploss = 50; stoploss < 200; stoploss += 25)
            //                                for (int profit = 50; profit < 200; profit += 25)
            //                                {
            //                                    if (lim_high > u_75 && lim_low < l_25)
            //                                    {
            //                                        StringBuilder s = new StringBuilder();
            //                                        s.Append(fastK + "," + slowK + "," + slowD + "," + u_75 + "," + l_25 + "," + lim_high + "," + lim_low + "," + stoploss + "," + profit);
            //                                        if (fastK != slowK)
            //                                            sr.WriteLine(s);

            //                                    }
            //                                }
            //sr.Close();
            #endregion
             
            List<string> Seq = new List<string>();


            string SIMcontext = @"Data Source=85.214.244.19;Initial Catalog=ALSI_SIM;User ID=SimLogin;Password=boeboe;MultipleActiveResultSets=True";
            var dc = new AlsiSimDataContext(SIMcontext);

            DataTable MinData = new DataTable("tblSequence");
            MinData.Columns.Add("Sequence", typeof(string));
            MinData.Columns.Add("Started", typeof(bool));
            MinData.Columns.Add("Completed", typeof(bool));

            var max = 97200000;

            var s = 0;
            var e = 100;

            var lineCount = 0;
            Console.WriteLine("Start reading File");
            using (var reader = File.OpenText(@"D:\seq.txt"))
            {
                while (reader.ReadLine() != null)
                {
                    lineCount++;
                    if (lineCount <= e && lineCount > s)
                        MinData.Rows.Add(reader.ReadLine(), false, false);
                    else
                        if(lineCount>e+1)
                        break;
                }
            }

            Console.WriteLine("Sending data to databse");

            DataSet DataSet = new DataSet("Dataset");
            DataSet.Tables.Add(MinData);
            SqlConnection myConnection = new SqlConnection(SIMcontext);
            myConnection.Open();
            SqlBulkCopy bulkcopy = new SqlBulkCopy(myConnection);
            bulkcopy.BulkCopyTimeout = 500000;
            bulkcopy.DestinationTableName = "tblSequence";
            bulkcopy.WriteToServer(MinData);
            MinData.Dispose();
            myConnection.Close();
        }

        public static void SendDatatoDatabase()
        {
            var max = 97200000;
            var current = 32001000;
            var interval = 200000;
            var s = 32001000;
            var e = s + interval;

            string SIMcontext = @"Data Source=85.214.244.19;Initial Catalog=ALSI_SIM;User ID=SimLogin;Password=boeboe;MultipleActiveResultSets=True";
            var dc = new AlsiSimDataContext(SIMcontext);

        Top:
           

            if (current < max)
            {
                DataTable MinData = new DataTable("tblSequence");
                MinData.Columns.Add("Sequence", typeof(string));
                MinData.Columns.Add("Started", typeof(bool));
                MinData.Columns.Add("Completed", typeof(bool));



                var lineCount = 0;
                Console.WriteLine("Start reading File");
                using (var reader = File.OpenText(@"D:\seq.txt"))
                {
                    while (reader.ReadLine() != null)
                    {
                        lineCount++;
                        if (lineCount <= e && lineCount > s)
                            MinData.Rows.Add(reader.ReadLine(), false, false);
                        else
                            if (lineCount > e + 1)
                                break;
                    }
                }
                current = e;
                s = e;
                e = e + interval;

                Console.WriteLine("Sending data to databse lines " + e);
                Debug.WriteLine("Sending data to databse lines " + e);

                DataSet DataSet = new DataSet("Dataset");
                DataSet.Tables.Add(MinData);
                SqlConnection myConnection = new SqlConnection(SIMcontext);
                myConnection.Open();
                SqlBulkCopy bulkcopy = new SqlBulkCopy(myConnection);
                bulkcopy.BulkCopyTimeout = 500000;
                bulkcopy.DestinationTableName = "tblSequence";
                bulkcopy.WriteToServer(MinData);
                MinData.Dispose();
                myConnection.Close();

                goto Top;
            }
        }

        public static string GetRandomSequence()
        {
            string SIMcontext = @"Data Source=85.214.244.19;Initial Catalog=ALSI_SIM;User ID=SimLogin;Password=boeboe;MultipleActiveResultSets=True";
            var dc = new AlsiSimDataContext(SIMcontext);

            var rt = dc.GetRandomTable();
            string q = "None";
            try
            {
                q = rt.Select(x => x.Sequence).First();
            }
            catch (Exception ex)
            {
            }
                return q;
        }

        public static void PrintTrades()
        {
            var sr = new StreamReader(@"D:\ohlcPL.csv");

        }


        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }
    }
}
