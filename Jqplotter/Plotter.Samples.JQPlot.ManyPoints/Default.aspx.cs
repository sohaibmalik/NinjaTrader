#region

using System;
using System.ComponentModel;
using System.Web.UI;
using System.Collections.Generic;
using Plotter.Core;
using Plotter.Helper;

#endregion

namespace Plotter.Samples.JQPlot.ManyPoints
{
    public partial class Default : Page
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) RandomDraw();
        }

        protected void ButtonRedraw_Click(object sender, EventArgs e)
        {
            RandomDraw();
        }

        #endregion

        #region Helpers

        protected void RandomDraw()
        {
            int numberOfPoints = int.Parse(TextBoxPoints.Text);
            //
            // Build a random curve
            //
            Curve curve = CurveHelper.GetRandomCurve(numberOfPoints);

            //
            // Plot our random curve
            //
            JQPlot.Curves = new BindingList<Curve> { curve };
        }

        #endregion
    }
}