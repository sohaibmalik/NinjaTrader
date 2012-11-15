using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils
{
	 public class PointData
	{
	     public DateTime TimeStamp { get; set; }

	     public int Open { get; set; }

	     public int High { get; set; }

	     public int Low { get; set; }

	     public int Close { get; set; }

	     public int Volume { get; set; }

	     public string InstrumentName { get; set; }
	}
}
