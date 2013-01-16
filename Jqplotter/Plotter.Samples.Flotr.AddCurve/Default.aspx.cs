#region

using System;
using System.Web.UI;
using Plotter.Core;
using Plotter.Helper;

#endregion

namespace Plotter.Samples.Flotr.AddCurve
{
    public partial class Default : Page
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) AddRandomCurve();
        }

        protected void ButtonAddCurve_Click(object sender, EventArgs e)
        {
            //
            // Add a random curve to the existings curves (the ViewState is enabled)
            //
            AddRandomCurve();
        }

        #endregion

        #region Helpers

        protected void AddRandomCurve()
        {
            //
            // Build a random curve
            //
            Curve curve = CurveHelper.GetRandomCurve(100);

            //
            // Plot our random curve
            //
            Flotr.Curves.Add(curve);
        }

        #endregion
    }
}