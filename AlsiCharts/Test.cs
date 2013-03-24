using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace AlsiCharts
{
   public class Test
    {
      public void TestMultiAxis_3()
       {
           AlsiCharts.MultiAxis_3 c = new MultiAxis_3();

           for (int x = 0; x < 10; x++)
           {
               c.XaxisLabels.Add(x.ToString());
               c.Series_A.Data.Add(x * (10 - 100));
               c.Series_B.Data.Add(x * 1000);
               c.Series_C.Data.Add(x * 3.1245);
           }

           c.Height = 700;
           c.LegendBackColor = Color.White;
           c.Series_A.LineStyle = Series.LineStyles.line;
           c.Series_B.LineStyle = Series.LineStyles.line;
           c.Series_C.LineStyle = Series.LineStyles.line;

           c.Series_A.YaxisNumber = 0;
           c.Series_B.YaxisNumber = 1;
           c.Series_C.YaxisNumber = 2;

           c.Series_A.AxisOppositeSide = true;
           c.Series_B.AxisOppositeSide = false;
           c.Series_C.AxisOppositeSide = true;

           c.Series_A.Unit = "Unit AA";
           c.Series_B.Unit = "Unit BB";
           c.Series_C.Unit = "Unit CC";

           c.Series_A.YaxixLabel = "Label A";
           c.Series_B.YaxixLabel = "Label B";
           c.Series_C.YaxixLabel = "Label C";

           c.Series_A.YaxisTitleColor = Color.Red;
           c.Series_A.YaxisUnitColor = Color.Purple;
           c.Series_B.YaxisTitleColor = Color.Pink;
           c.Series_B.YaxisUnitColor = Color.Blue;
           c.Series_C.YaxisTitleColor = Color.Green;
           c.Series_C.YaxisUnitColor = Color.Orange;

           c.PopulateScript();
           c.ShowChartInBrowser(new FileInfo(@"D:\abc.html"));
       }

      
    }
}
