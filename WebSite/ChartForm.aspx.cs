using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Plotter.Core;
using Plotter.Helper;
using AlsiUtils;
using System.Linq;
using System.Data.Linq;

public partial class ChartForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsCallback)
        {
            //http://www.codeproject.com/articles/432860/asp-net-plotter-toolkit-html5-charting-for-all
            AlsiDBDataContext dc = new AlsiDBDataContext();
            var data = dc.OHLC_5_Minutes.Take(25000).ToList();
            Label1.Text = data.Count.ToString();
            Curve curve = new Curve();
            var points = new Point[data.Count];

            for (int x = 0; x < data.Count; x++)
            {
                var p = new Point(data[x].Stamp, data[x].C);
                points[x] = p;
            }


            BindingList<Curve> Curves = new BindingList<Curve> { curve };

            curve.Points = points;
            curve.Label = "Dfdfdfdff";

            Dygraph1.Curves = Curves;
         
            Dygraph1.YLowRange = data.Min(z => z.C);//if error check db
            Dygraph1.YHighRange = data.Max(z => z.C);
        }
        
    }
}