using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Drawing;

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

        public virtual void ShowChartInBrowser(FileInfo file)
        {
            
            StreamWriter sr = new StreamWriter(file.FullName);
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
    }
}
