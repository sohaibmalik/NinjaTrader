using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AlsiUtils
{
    public abstract class Point
    {
        public DateTime TimeStamp { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
    }
}
