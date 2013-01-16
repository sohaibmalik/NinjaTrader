﻿#region

using System;
using System.ComponentModel;
using System.Web.UI;
using Plotter.Core;
using Plotter.Helper;

#endregion

namespace Plotter.Samples.GoogleChart.MasterPage
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
            Curve curve = CurveHelper.GetRandomCurve(100);

            //
            // Plot our random curve
            //
            GoogleChart.Curves = new BindingList<Curve> {curve};
        }

        #endregion
    }
}