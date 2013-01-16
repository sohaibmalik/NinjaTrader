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

[assembly: TagPrefix("Plotter.Controls.Flotr2Control", "asp")]
[assembly: WebResource("Plotter.Controls.Flotr2Control._excanvas.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.Flotr2Control._date.format.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.Flotr2Control._dateformatter.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.Flotr2Control._flotr2.js", "text/javascript")]
[assembly: CLSCompliant(true)]

namespace Plotter.Controls.Flotr2Control
{
    /// <summary>
    ///   Flotr2 control.
    /// </summary>
    [ToolboxData("<{0}:Flotr2 runat=server></{0}:Flotr2>")]
    [ToolboxBitmap(typeof(Flotr2), "Flotr2.ico")]
    public class Flotr2 : PlotterControl
    {
        #region Fields

        // const //
        private const string ScriptIncludeExcanvasKey = "_Excanvas";
        private const string ScriptIncludeDateFormatKey = "_DateFormat";
        private const string ScriptIncludeDateFormatterKey = "_DateFormatter";
        private const string ScriptIncludeFlotr2Key = "_Flotr2";

        #endregion

        #region Overridden Methods

        /// <summary>
        ///   Retrieves the curves' ViewState key.
        /// </summary>
        /// <returns> The curves' ViewState key. </returns>
        protected override string GetCurvesViewStateKey()
        {
            return "Plotter.Controls.Flotr2Control_Curves";
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
            WebHelper.RegisterExcanvas(Page, ScriptIncludeExcanvasKey, type, "Plotter.Controls.Flotr2Control._excanvas.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludeDateFormatKey, type, "Plotter.Controls.Flotr2Control._date.format.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludeDateFormatterKey, type, "Plotter.Controls.Flotr2Control._dateformatter.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludeFlotr2Key, type, "Plotter.Controls.Flotr2Control._flotr2.js");
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
                xaxis = new { mode = "time", tickFormatter = JS.Expression("dateFormatter") },
            });
        }

        /// <summary>
        ///   Retrieves the main script of the plotter.
        /// </summary>
        /// <returns> The main script of the plotter. </returns>
        protected override Script GetScript()
        {
            return JS.Script(JS.Expression("Flotr").Dot("draw").Call(JS.Expression("document").Dot("getElementById").Call(ClientID), GetCurves(), GetOptions()));
        }

        #endregion

        #region Helpers

        /// <summary>
        ///   Projects a curve into a Flotr2 Javascript expression.
        /// </summary>
        /// <param name="curve"> The curve. </param>
        /// <returns> The Flotr2 Javascript expression of the curve. </returns>
        private Expression GetExpression(Curve curve)
        {
            return string.IsNullOrEmpty(curve.Label)
                       ? JS.Object(new { data = GetMatrix(curve) })
                       : JS.Object(new { label = curve.Label, data = GetMatrix(curve) });
        }

        /// <summary>
        ///   Projects the points of the curve into a Flotr2 Javascript Expression.
        /// </summary>
        /// <param name="curve"> The curve. </param>
        /// <returns> The Flotr2 Javascript expression of the points. </returns>
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