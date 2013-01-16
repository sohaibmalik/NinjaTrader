#region

using System;
using System.Web.UI;
using Plotter.Core;
using Plotter.Helper;
using System.Threading;

#endregion

namespace Plotter.Samples.Dygraph.CustomLegend
{
    public partial class Default : Page
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                for (int i = 0; i < 10; i++)
                {
                    AddRandomDraw();
                    Thread.Sleep(20); // to get different random numbers (Random's seed relies on system clock)
                }
            }
        }

        protected void ButtonAddCurve_Click(object sender, EventArgs e)
        {
            //
            // Add a random curve to the existings curves (the ViewState is enabled)
            //
            AddRandomDraw();
        }

        #endregion

        #region Helpers

        protected void AddRandomDraw()
        {
            //
            // Build a random curve
            //
            Curve curve = CurveHelper.GetRandomCurve(5);

            //
            // Plot our random curve
            //
            Dygraph.Curves.Add(curve);
        }

        #endregion
    }
}