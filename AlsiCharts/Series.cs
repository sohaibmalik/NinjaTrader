using System.Collections.Generic;
using System.Drawing;

namespace AlsiCharts
{
    public class Series
    {
        public List<double> Data { get; set; }
        public string YaxixLabel { get; set; }
        public int YaxisNumber { get; set; }
        public string Unit { get; set; }
        public string YaxisTitleColorHEX { get; set; }
        public string YaxisUnitColorHEX { get; set; }
        public Color YaxisTitleColor
        {
            set
            {
                YaxisTitleColorHEX = ColorToHEX(value);
            }
        }
        public Color YaxisUnitColor
        {
            set
            {
                YaxisUnitColorHEX = ColorToHEX(value);
            }
        }
        public LineStyles LineStyle { get; set; }
        public DashStyles DashStyle { get; set; }
        public bool AxisOppositeSide { get; set; }

        public Series()
        {
            Data = new List<double>();
        }

        private string ColorToHEX(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public enum LineStyles
        {
            spline,
            column,
            bar,
            line,
        }

        public enum DashStyles
        {
            Solid,
            ShortDash,
            ShortDot,
            ShortDashDot,
            ShortDashDotDot,
            Dot,
            Dash,
            LongDash,
            DashDot,
            LongDashDot,
            LongDashDotDot,

        }

        
    }
}
