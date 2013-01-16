#region

using System;
using System.ComponentModel;
using System.Web.UI;
using System.Collections.Generic;
using Plotter.Core;
using Plotter.Helper;

#endregion

namespace Plotter.Samples.Dygraph.ManyPoints
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
            //
            // Build a random curve
            //
            int numberOfPoints = int.Parse(TextBoxPoints.Text);
            Curve curve = CheckBoxEnableErrorBands.Checked
                ? CurveHelper.GetRandomCurveWithErrorBands(numberOfPoints)
                : CurveHelper.GetRandomCurve(numberOfPoints);

            //
            // Plot our random curve
            //
            Dygraph.EnableErrorBands = CheckBoxEnableErrorBands.Checked;
            Dygraph.ShowRoller = CheckBoxShowRoller.Checked;
            Dygraph.RollPeriod = int.Parse(TextBoxRollingPeriod.Text);
            Dygraph.ShowRangeSelector = CheckBoxShowRangeSelector.Checked;
            Dygraph.Curves = new BindingList<Curve> { curve };
        }

        #endregion
    }
}