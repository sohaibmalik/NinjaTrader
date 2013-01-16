#region

using System;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using Adam.JSGenerator;
using Plotter.Core;
using Plotter.Helper;

#endregion

[assembly: TagPrefix("Plotter.Controls.FlotControl", "asp")]
[assembly: WebResource("Plotter.Controls.FlotControl._excanvas.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.FlotControl._jquery-1.6.2.min.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.FlotControl._jquery.flot.js", "text/javascript")]
[assembly: CLSCompliant(true)]

namespace Plotter.Controls.FlotControl
{
    /// <summary>
    ///   Flot control.
    /// </summary>
    [ToolboxData("<{0}:Flot runat=server></{0}:Flot>")]
    [ToolboxBitmap(typeof(Flot), "Flot.ico")]
    public class Flot : PlotterControl
    {
        #region Fields

        // const //
        private const string ScriptIncludeExcanvasKey = "_excanvas";
        private const string ScriptIncludeFlotKey = "_flot";
        private const string ScriptIncludeJQueryKey = "_JQuery";

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Overridden Methods

        /// <summary>
        ///   Retrieves the curves' ViewState key.
        /// </summary>
        /// <returns> The curves' ViewState key. </returns>
        protected override string GetCurvesViewStateKey()
        {
            return "Plotter.Controls.FlotControl_Curves";
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
            WebHelper.RegisterExcanvas(Page, ScriptIncludeExcanvasKey, type, "Plotter.Controls.FlotControl._excanvas.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludeJQueryKey, type, "Plotter.Controls.FlotControl._jquery-1.6.2.min.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludeFlotKey, type, "Plotter.Controls.FlotControl._jquery.flot.js");
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
            return JS.Object(new { xaxis = new { mode = "time", timeformat = "%y/%m/%d %H:%M:%S %P" } });
        }

        /// <summary>
        ///   Retrieves the main script of the plotter.
        /// </summary>
        /// <returns> The main script of the plotter. </returns>
        protected override Script GetScript()
        {
            return JS.Script(JS.Expression("$.plot").Call(JS.Expression("$('#" + ClientID + "')"), GetCurves(), GetOptions()));
        }

        #endregion

        #region Helpers

        /// <summary>
        ///   Projects a curve into a Flot Javascript expression.
        /// </summary>
        /// <param name="curve"> The curve. </param>
        /// <returns> The Flot Javascript expression of the curve. </returns>
        private Expression GetExpression(Curve curve)
        {
            return string.IsNullOrEmpty(curve.Label)
                       ? JS.Object(new { data = GetMatrix(curve) })
                       : JS.Object(new { label = curve.Label, data = GetMatrix(curve) });
        }

        /// <summary>
        ///   Projects the points of the curve into a Flot Javascript Expression.
        /// </summary>
        /// <param name="curve"> The curve. </param>
        /// <returns> The Flot Javascript expression of the points. </returns>
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