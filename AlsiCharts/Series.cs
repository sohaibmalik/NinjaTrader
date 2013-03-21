using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiCharts
{
    public class Series
    {
        public List<double> Data{ get; set; }
        public List<string> XaxisLabels { get; set; }
        public string YaxixLabel { get; set; }
        public string Unit { get; set; }
        public string Color { get; set; }
        public string LineType { get; set; }
        public string DashType { get; set; }
    }
}
