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

[assembly: TagPrefix("Plotter.Controls.FlotrControl", "asp")]
[assembly: WebResource("Plotter.Controls.FlotrControl._flotr.css", "text/css")]
[assembly: WebResource("Plotter.Controls.FlotrControl._excanvas.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.FlotrControl._date.format.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.FlotrControl._dateformatter.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.FlotrControl._prototype.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.FlotrControl._flotr-0.2.0-alpha.js", "text/javascript")]
[assembly: CLSCompliant(true)]

namespace Plotter.Controls.FlotrControl
{
    /// <summary>
    ///   Flotr control.
    /// </summary>
    [ToolboxData("<{0}:Flotr runat=server></{0}:Flotr>")]
    [ToolboxBitmap(typeof(Flotr), "Flotr.ico")]
    public class Flotr : PlotterControl
    {
        #region Fields

        private bool _showSpreadsheet;

        // const //
        private const string ScriptIncludeExcanvasKey = "_Excanvas";
        private const string ScriptIncludeDateFormatKey = "_DateFormat";
        private const string ScriptIncludeDateFormatterKey = "_DateFormatter";
        private const string ScriptIncludePrototypeKey = "_Prototype";
        private const string ScriptIncludeFlotrKey = "_Flotr";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Flotr control.
        /// </summary>
        public Flotr()
        {
            _showSpreadsheet = false;
        }

        #endregion

        #region Properties

        /// <summary>
        ///  Indicates whether spreadsheet and CSV download are enabled.
        /// </summary>
        [Bindable(true)]
        [Description("Indicates whether spreadsheet and CSV download are enabled. False by default.")]
        public bool ShowSpreadsheet
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.FlotrControl_ShowLabels"];
                    return o != null && (bool)o;
                }
                return _showSpreadsheet;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.FlotrControl_ShowLabels"] = value;
                _showSpreadsheet = value;
            }
        }


        #endregion

        #region Overridden Methods

        /// <summary>
        ///   Retrieves the curves' ViewState key.
        /// </summary>
        /// <returns> The curves' ViewState key. </returns>
        protected override string GetCurvesViewStateKey()
        {
            return "Plotter.Controls.FlotrControl_Curves";
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
            WebHelper.RegisterExcanvas(Page, ScriptIncludeExcanvasKey, type, "Plotter.Controls.FlotrControl._excanvas.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludeDateFormatKey, type, "Plotter.Controls.FlotrControl._date.format.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludeDateFormatterKey, type, "Plotter.Controls.FlotrControl._dateformatter.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludePrototypeKey, type, "Plotter.Controls.FlotrControl._prototype.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludeFlotrKey, type, "Plotter.Controls.FlotrControl._flotr-0.2.0-alpha.js");
            if (ShowSpreadsheet) WebHelper.RegisterCSSInclude(Page, type, "Plotter.Controls.FlotrControl._flotr.css");
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
                xaxis = new { tickFormatter = JS.Expression("dateFormatter") },
                spreadsheet = new { show = ShowSpreadsheet }
            });
        }

        /// <summary>
        ///   Retrieves the main script of the plotter.
        /// </summary>
        /// <returns> The main script of the plotter. </returns>
        protected override Script GetScript()
        {
            return JS.Script(JS.Expression("Flotr").Dot("draw").Call(JS.Expression("$('" + ClientID + "')"), GetCurves(), GetOptions()));
        }

        #endregion

        #region Helpers

        /// <summary>
        ///   Projects a curve into a Flotr Javascript expression.
        /// </summary>
        /// <param name="curve"> The curve. </param>
        /// <returns> The Flotr Javascript expression of the curve. </returns>
        private Expression GetExpression(Curve curve)
        {
            return string.IsNullOrEmpty(curve.Label)
                       ? JS.Object(new { data = GetMatrix(curve) })
                       : JS.Object(new { label = curve.Label, data = GetMatrix(curve) });
        }

        /// <summary>
        ///   Projects the points of the curve into a Flotr Javascript Expression.
        /// </summary>
        /// <param name="curve"> The curve. </param>
        /// <returns> The Flotr Javascript expression of the points. </returns>
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