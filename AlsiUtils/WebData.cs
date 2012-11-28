using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using MoreLinq;

namespace AlsiUtils
{
    public class WebData
    {


        #region WebClient


        public static List<PointData> getHistoricalMinute_FromWEB(DateTime StartDate, DateTime EndDate, int TimeFrame, string ContractName)
        {
            string startdate = StartDate.ToString("yyyyMMdd");
            string endddate = EndDate.ToString("yyyyMMdd");

            string URL = "http://www.hisat.co.za/updater/minuteUpdate.aspx?Instrument=" + ContractName + "&StartDate=" + startdate + @"&EndDate=" + endddate + @"&Password=PieterF&Username=PFOUCHE&compression=" + TimeFrame;
            Debug.WriteLine(URL);


            List<PointData> Data = new List<PointData>();
            List<string> rawData = new List<string>();


            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
            req.AutomaticDecompression = DecompressionMethods.GZip;
            WebResponse resp = req.GetResponse();
            Stream s = resp.GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.ASCII);

            while (sr.Peek() >= 0)
            {

                string _rawData = sr.ReadLine();
                if (!rawData.Contains(_rawData)) rawData.Add(_rawData);
            }












            foreach (string ss in rawData)
            {

                PointData p = new PointData();
                List<string> data = new List<string>(ss.Split(','));
                string date = data[0];

                int y = int.Parse(date.Substring(0, 4));
                int mm = int.Parse(date.Substring(4, 2));
                int d = int.Parse(date.Substring(6, 2));

                string time = data[1];
                int h = int.Parse(time.Substring(0, 2));
                int m = int.Parse(time.Substring(3, 2));


                p.TimeStamp = new DateTime(y, mm, d, h, m, 0);
                p.Open = int.Parse(data[2]);
                p.High = int.Parse(data[3]);
                p.Low = int.Parse(data[4]);
                p.Close = int.Parse(data[5]);
                p.Volume = int.Parse(data[6]);
                p.InstrumentName = ContractName;
                Data.Add(p);

            }
            
         return Data.DistinctBy(t => t.TimeStamp).ToList();
            
        }

        public static List<PointData> getHistoricalTick_FromWEB(DateTime StartDate, DateTime EndDate, string ContractName)
        {
            List<PointData> Data = new List<PointData>();

            try
            {
                string startdate = StartDate.ToString("yyyyMMdd");
                string endddate = EndDate.ToString("yyyyMMdd");

                string URL = "http://www.hisat.co.za/updater/tickUpdate.aspx?Instrument=" + ContractName + "&StartDate=" + startdate + @"&EndDate=" + endddate + @"&Password=PieterF&Username=PFOUCHE";



                List<string> rawData = new List<string>();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);



                req.AutomaticDecompression = DecompressionMethods.GZip;
                WebResponse resp = req.GetResponse();
                Stream s = resp.GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.ASCII);


                while (sr.Peek() >= 0)
                {

                    string _rawData = sr.ReadLine();
                    if (!rawData.Contains(_rawData)) rawData.Add(_rawData);
                }



                for (int x = 0; x < rawData.Count; x++)
                {

                    PointData p = new PointData();
                    List<string> data = new List<string>(rawData[x].Split(','));
                    string date = data[0];

                    int y = int.Parse(date.Substring(0, 4));
                    int mm = int.Parse(date.Substring(4, 2));
                    int d = int.Parse(date.Substring(6, 2));

                    string time = data[1];
                    int h = int.Parse(time.Substring(0, 2));
                    int m = int.Parse(time.Substring(3, 2));
                    int ss = int.Parse(time.Substring(6, 2));


                    p.TimeStamp = new DateTime(y, mm, d, h, m, ss, 0);
                    p.Close = int.Parse(data[2]);
                    p.Volume = int.Parse(data[3]);


                    Data.Add(p);
                }


                int countMili = 0;
                for (int x = 0; x < Data.Count - 1; x++)
                {


                    if (Data[x + 1].TimeStamp.Second == Data[x].TimeStamp.Second)
                    {
                        countMili++;
                        Data[x].TimeStamp = Data[x].TimeStamp.AddMilliseconds(countMili);
                    }
                    else
                    //if (Data[x].TimeStamp.Second!=Data[x+1].TimeStamp.Second)
                    {
                        countMili++;
                        Data[x].TimeStamp = Data[x].TimeStamp.AddMilliseconds(countMili);
                        countMili = -1;
                    }

                }
                Data[Data.Count - 1].TimeStamp = Data[Data.Count - 1].TimeStamp.AddMilliseconds(100);
                return Data;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Data;
            }


        }


      
        #endregion
    }
}
