using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Drawing;
using System.Net;
using System;

namespace AlsiCharts
{
    public abstract class Chart
    {

        public string WebTabTitle { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool SharedTootltip{get;set;}
        public string LegendBackColorHEX { get; set; }
        public Color LegendBackColor
        {
            set
            {
                LegendBackColorHEX = ColorToHEX(value);
            }
        }
        public string OutputFileName { get; set; }

        private string ColorToHEX(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }


        public List<string> XaxisLabels = new List<string>();

        public string Script = "";



        public abstract void PopulateScript();




        public virtual string GetScript()
        {
            StringBuilder script = new StringBuilder(Script);
            script.Replace("*", @"""");
            return script.ToString();
        }

        public virtual void ShowChartInBrowser()
        {
            if (OutputFileName == null) throw new Exception("No output name specified");
            var dir = Path.GetTempPath();
            var file = new FileInfo(dir + @"\" + OutputFileName + ".html");
            StreamWriter sr = new StreamWriter(dir+@"\"+OutputFileName+".html");
            sr.Write(GetScript());
            sr.Close();
            Process.Start(file.FullName);
        }

        public virtual string MakeXaxisLabels()
        {
            if (XaxisLabels.Count == 0) return "'No Labels'";
            else
            {
                var f = XaxisLabels.First();
                var l = XaxisLabels.Last();
                XaxisLabels[0] = "'"+XaxisLabels[0];                                  
                var Labels = string.Join("','", XaxisLabels.ToArray());
                Labels = Labels + "'";
                return Labels;
            }
        }

        public virtual string MakeYaxisData(List<double> Data)
        {
            var stringData=new List<string>();
            foreach (var d in Data) stringData.Add(d.ToString());
            var data = string.Join(",", stringData.ToArray());
            return data;
        }

        public enum LegendPosition
        {

        }

        public virtual void UploadFile()
        {
            try
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://www.alsitm.com/alsitm.com/wwwroot/Charts/" + OutputFileName + ".html");
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential("alsitmadmin_ftp", "1Rachelle");

                // Copy the contents of the file to the request stream.
                var dir = Path.GetTempPath();
                var file = dir + @"\" + OutputFileName + ".html";
                StreamReader sourceStream = new StreamReader(file);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();
                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Debug.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

                response.Close();
            }
            catch { }
        }
    }

    
}
