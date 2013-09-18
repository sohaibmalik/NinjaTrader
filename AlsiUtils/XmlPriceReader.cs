using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using AlsiUtils.Data_Objects;

namespace AlsiUtils
{
    public class XmlPriceReader
    {

        public static List<Price> getPriceFromXML(FileInfo file, string InstrumentCode)
        {
            List<DateTime> datum = new List<DateTime>();
            List<int> open = new List<int>();
            List<int> high = new List<int>();
            List<int> low = new List<int>();
            List<int> close = new List<int>();
            List<int> volume = new List<int>();
            List<Price> p = new List<Price>();

            #region Messy

            using (XmlReader reader = XmlReader.Create(file.FullName))
            {
                while (reader.Read())
                {
                    // Only detect start elements.
                    if (reader.IsStartElement())
                    {
                        //Timestamp		
                        if (reader.Name == "Date")
                        {
                            {
                                string attribute_date = reader["Date"];
                                if (attribute_date != null)
                                {
                                    Debug.WriteLine("  Has attribute name: " + attribute_date);
                                }
                                // Next read will contain text.
                                if (reader.Read())
                                {
                                    string ds = reader.Value.Trim();
                                    DateTime d = DateTime.Parse(ds);
                                    datum.Add(d);
                                }
                            }
                        }

                        //OPEN
                        if (reader.Name == "Open")
                        {
                            string attribute_open = reader["Open"];
                            if (attribute_open != null)
                            {
                                Debug.WriteLine("  Has attribute name: " + attribute_open);
                            }
                            // Next read will contain text.
                            if (reader.Read())
                            {
                                string o = reader.Value.Trim();
                                open.Add(int.Parse(o));

                            }
                        }

                        //HIGH
                        if (reader.Name == "High")
                        {
                            string attribute_high = reader["High"];
                            if (attribute_high != null)
                            {
                                Debug.WriteLine("  Has attribute name: " + attribute_high);
                            }
                            // Next read will contain text.
                            if (reader.Read())
                            {
                                string h = reader.Value.Trim();
                                high.Add(int.Parse(h));
                            }
                        }

                        //LOW
                        if (reader.Name == "Low")
                        {
                            string attribute_low = reader["Low"];
                            if (attribute_low != null)
                            {
                                Debug.WriteLine("  Has attribute name: " + attribute_low);
                            }
                            // Next read will contain text.
                            if (reader.Read())
                            {
                                string l = reader.Value.Trim();
                                low.Add(int.Parse(l));
                            }
                        }

                        //CLOSE
                        if (reader.Name == "Close")
                        {
                            string attribute_close = reader["Close"];
                            if (attribute_close != null)
                            {
                                Debug.WriteLine("  Has attribute name: " + attribute_close);
                            }
                            // Next read will contain text.
                            if (reader.Read())
                            {
                                string c = reader.Value.Trim();
                                close.Add(int.Parse(c));
                            }
                        }





                        //VOLUME
                        if (reader.Name == "Volume")
                        {
                            string attribute_vol = reader["Volume"];
                            if (attribute_vol != null)
                            {
                                Debug.WriteLine("  Has attribute name: " + attribute_vol);
                            }
                            // Next read will contain text.
                            if (reader.Read())
                            {
                                string v = reader.Value.Trim();
                                volume.Add(int.Parse(v));
                            }
                        }


                    }

                }

            }
            #endregion



            for (int x = 0; x < datum.Count; x++)
            {
                var pp = new Price
                {
                    TimeStamp = datum[x],
                    Open = open[x],
                    High = high[x],
                    Low = low[x],
                    Close = close[x],
                    Volume = volume[x],
                    InstrumentName = InstrumentCode
                };
                p.Add(pp);
            }

            return p;
        }

        public static void UpdatetoMinuteImport()
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.Connection.ConnectionString = GlobalObjects.CustomConnectionString;
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

        public static void exampleofUse()
        {
            //PC
            string css = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = css;
            FileInfo f = new FileInfo(@"D:\WK48.XML");

            var price = AlsiUtils.XmlPriceReader.getPriceFromXML(f, "DEC12ALSI");
            AlsiUtils.Data_Objects.GlobalObjects.Points = price;
            AlsiUtils.XmlPriceReader.UpdatetoMinuteImport();
        }
    }
}
