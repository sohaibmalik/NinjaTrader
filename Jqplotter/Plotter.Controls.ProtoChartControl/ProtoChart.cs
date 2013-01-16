#region

using System;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using Adam.JSGenerator;
using Plotter.Core;
using Plotter.Helper;
using System.ComponentModel;

#endregion

[assembly: TagPrefix("Plotter.Controls.ProtoChartControl", "asp")]
[assembly: WebResource("Plotter.Controls.ProtoChartControl._excanvas.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.ProtoChartControl._date.format.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.ProtoChartControl._dateformatter.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.ProtoChartControl._prototype.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.ProtoChartControl._ProtoChart.js", "text/javascript")]
[assembly: CLSCompliant(true)]

namespace Plotter.Controls.ProtoChartControl
{
    /// <summary>
    ///   ProtoChart control.
    /// </summary>
    [ToolboxData("<{0}:ProtoChart runat=server></{0}:ProtoChart>")]
    [ToolboxBitmap(typeof(ProtoChart), "ProtoChart.ico")]
    public class ProtoChart : PlotterControl
    {
        #region Fields

        private bool _showSpreadsheet;

        // const //
        private const string ScriptIncludeExcanvasKey = "_Excanvas";
        private const string ScriptIncludeDateFormatKey = "_DateFormat";
        private const string ScriptIncludeDateFormatterKey = "_DateFormatter";
        private const string ScriptIncludePrototypeKey = "_Prototype";
        private const string ScriptIncludeProtoChartKey = "_ProtoChart";

        #endregion

        #region Overridden Methods

        /// <summary>
        ///   Retrieves the curves' ViewState key.
        /// </summary>
        /// <returns> The curves' ViewState key. </returns>
        protected override string GetCurvesViewStateKey()
        {
            return "Plotter.Controls.ProtoChartControl_Curves";
        }

        /// <summary>
        ///   Raises the PreRender event.
        /// </summary>
        /// <param name="e"> An EventArgs object that contains the event data. </param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Type type = GetType();
            WebHelper.EmulateIE(Page);
            WebHelper.RegisterExcanvas(Page, ScriptIncludeExcanvasKey, type, "Plotter.Controls.ProtoChartControl._excanvas.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludeDateFormatKey, type, "Plotter.Controls.ProtoChartControl._date.format.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludeDateFormatterKey, type, "Plotter.Controls.ProtoChartControl._dateformatter.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludePrototypeKey, type, "Plotter.Controls.ProtoChartControl._prototype.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludeProtoChartKey, type, "Plotter.Controls.ProtoChartControl._ProtoChart.js");
        }

        /// <summary>
        ///   Retrieves the Curves as a Javascript Expression.
        /// </summary>
        /// <returns> The Javascript expression of the curves. </returns>
        protected override Expression GetCurves()
        {
            return Curves.Count > 0 ? JS.Array(Curves.Select(GetExpression)) : JS.Array(JS.Array());
        }

        /// <summary>
        ///   Retrieves the Options as a Javascript Expression.
        /// </summary>
        /// <returns> The Javascript expression of the options. </returns>
        protected override Expression GetOptions()
        {
            return JS.Object(new
            {
                xaxis = new { mode = "time", tickFormatter = JS.Expression("dateFormatter") }
            });
        }

        /// <summary>
        ///   Retrieves the main script of the plotter.
        /// </summary>
        /// <returns> The main script of the plotter. </returns>
        protected override Script GetScript()
        {
            return JS.Script(JS.New(JS.Expression("Proto").Dot("Chart"), JS.Expression("$('" + ClientID + "')"), GetCurves(), GetOptions()));
        }

        #endregion

        #region Helpers

        /// <summary>
        ///   Projects a curve into a ProtoChart Javascript expression.
        /// </summary>
        /// <param name="curve"> The curve. </param>
        /// <returns> The ProtoChart Javascript expression of the curve. </returns>
        private Expression GetExpression(Curve curve)
        {
            return string.IsNullOrEmpty(curve.Label)
                       ? JS.Object(new { data = GetMatrix(curve) })
                       : JS.Object(new { label = curve.Label, data = GetMatrix(curve) });
        }

        /// <summary>
        ///   Projects the points of the curve into a ProtoChart Javascript Expression.
        /// </summary>
        /// <param name="curve"> The curve. </param>
        /// <returns> The ProtoChart Javascript expression of the points. </returns>
        private ArrayExpression GetMatrix(Curve curve)
        {
            return
                JS.Array(
                    curve.Points.Select(
                        point => JS.Array(JS.New(JS.Expression("Date"),
                               point.X.Year,
                               point.X.Month-1,
                               point.X.Day,
                               point.X.Hour,
                               point.X.Minute,
                               point.X.Second,
                               point.X.Millisecond), point.Y)));
        }

        #endregion
    }
}