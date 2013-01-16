#region

using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using Adam.JSGenerator;
using Plotter.Core;
using Plotter.Helper;
using System.Collections.Generic;

#endregion

[assembly: TagPrefix("Plotter.Controls.JQPlotControl", "asp")]
[assembly: WebResource("Plotter.Controls.JQPlotControl._excanvas.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.JQPlotControl._jquery-1.6.2.min.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.JQPlotControl._jquery.jqplot.min.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.JQPlotControl._jqplot.dateAxisRenderer.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.JQPlotControl._jquery.jqplot.min.css", "text/css")]
[assembly: CLSCompliant(true)]

namespace Plotter.Controls.JQPlotControl
{
    /// <summary>
    ///   jqPlot control.
    /// </summary>
    [ToolboxData("<{0}:JQPlot runat=server></{0}:JQPlot>")]
    [ToolboxBitmap(typeof(JQPlot), "ico")]
    public class JQPlot : PlotterControl
    {
        #region Fields

        // const //
        private const string ScriptIncludeJQueryKey = "_JQuery";
        private const string ScriptIncludeJQPlotKey = "_jqPlot";
        private const string ScriptIncludeExcanvasKey = "_excanvas";
        private const string ScriptIncludeJQPlotdateAxisRendererKey = "_dateAxisRenderer";
        private bool _enableAnimation;
        private bool _showLabels;
        private string _xAxisFormat;
        private bool _fill;
        private bool _fillToZero;
        private bool _smooth;
        private Placement _legendPlacement;

        #endregion

        #region Constructors

        /// <summary>
        ///   Initialise a new instance of
        /// </summary>
        public JQPlot()
        {
            _showLabels = false;
            _enableAnimation = false;
            _xAxisFormat = "%F %R";
            _fill = false;
            _fillToZero = false;
            _smooth = false;
            _legendPlacement = Placement.Inside;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Indicates whether curves' labels are shown or not. False by default.
        /// </summary>
        [Bindable(true)]
        [Description("Indicates whether curves' labels are shown or not. False by default.")]
        public bool ShowLabels
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.JQPlotControl_ShowLabels"];
                    return o != null && (bool)o;
                }
                return _showLabels;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.JQPlotControl_ShowLabels"] = value;
                _showLabels = value;
            }
        }

        /// <summary>
        ///   Indicates whether the zoom animation is enabled or not. False by default.
        /// </summary>
        [Bindable(true)]
        [Description("Indicates whether the HTML5 animation is enabled or not. False by default.")]
        public bool EnableAnimation
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.JQPlotControl_EnableAnimation"];
                    return o != null && (bool)o;
                }
                return _enableAnimation;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.JQPlotControl_EnableAnimation"] = value;
                _enableAnimation = value;
            }
        }

        /// <summary>
        /// Date time format in X axis. Defaults to "%F %R". Acceptable format code are listed here: http://www.jqplot.com/docs/files/plugins/jqplot-dateAxisRenderer-js.html
        /// </summary>
        [Bindable(true)]
        [Description("Date time format in X axis. Defaults to '%F %R'. Acceptable format code are listed here: http://www.jqplot.com/docs/files/plugins/jqplot-dateAxisRenderer-js.html")]
        public string XAxisFormat
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.JQPlotControl_XAxisFormat"];
                    return o == null ? string.Empty : (string)o;
                }
                return _xAxisFormat;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.JQPlotControl_XAxisFormat"] = value;
                _xAxisFormat = value;
            }
        }

        /// <summary>
        /// Fill under the line. Defaults to false.
        /// </summary>
        [Bindable(true)]
        [Description("Fill under the line. Defaults to false.")]
        public bool Fill
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.JQPlotControl_Fill"];
                    return o != null && (bool)o;
                }
                return _fill;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.JQPlotControl_Fill"] = value;
                _fill = value;
            }
        }

        /// <summary>
        /// Forces filled series to fill toward zero on the fill Axis. Defaults to false.
        /// </summary>
        [Bindable(true)]
        [Description("Forces filled series to fill toward zero on the fill Axis. Defaults to false.")]
        public bool FillToZero
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.JQPlotControl_FillToZero"];
                    return o != null && (bool)o;
                }
                return _fillToZero;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.JQPlotControl_FillToZero"] = value;
                _fillToZero = value;
            }
        }

        /// <summary>
        /// Draw a smoothed (interpolated) line through the data points with automatically computed number of smoothing points. Defaults to false.
        /// </summary>
        [Bindable(true)]
        [Description("Draw a smoothed (interpolated) line through the data points with automatically computed number of smoothing points. Defaults to false.")]
        public bool Smooth
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.JQPlotControl_Smooth"];
                    return o != null && (bool)o;
                }
                return _smooth;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.JQPlotControl_Smooth"] = value;
                _smooth = value;
            }
        }

        /// <summary>
        /// Legend placement. Defaults to Inside.
        /// </summary>
        [Bindable(true)]
        [Description("Legend placement. Defaults to Inside.")]
        public Placement LegendPlacement
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.JQPlotControl_LegendPlacement"];
                    return o == null ? Placement.Inside : (Placement)o;
                }
                return _legendPlacement;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.JQPlotControl_LegendPlacement"] = value;
                _legendPlacement = value;
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
            return "Plotter.Controls.JQPlotControl_Curves";
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
            WebHelper.RegisterExcanvas(Page, ScriptIncludeExcanvasKey, type, "Plotter.Controls.JQPlotControl._excanvas.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludeJQueryKey, type, "Plotter.Controls.JQPlotControl._jquery-1.6.2.min.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludeJQPlotKey, type, "Plotter.Controls.JQPlotControl._jquery.jqplot.min.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludeJQPlotdateAxisRendererKey, type, "Plotter.Controls.JQPlotControl._jqplot.dateAxisRenderer.js");
            WebHelper.RegisterCSSInclude(Page, type, "Plotter.Controls.JQPlotControl._jquery.jqplot.min.css");
        }

        /// <summary>
        ///   Retrieves the Curves as a Javascript Expression.
        /// </summary>
        /// <returns> The Javascript expression of the curves. </returns>
        protected override Expression GetCurves()
        {
            return Curves.Count > 0 ? JS.Array(Curves.Select(GetExpression)) : JS.Array(JS.Array(0));
        }

        /// <summary>
        ///   Retrieves the Options as a Javascript Expression.
        /// </summary>
        /// <returns> The Javascript expression of the options. </returns>
        protected override Expression GetOptions()
        {
            Expression labels = ShowLabels && Curves.Count > 0
                                    ? JS.Array(Curves.Select(c => JS.Object(new { label = c.Label })))
                                    : JS.Array();

            ObjectLiteralExpression options = JS.Object(
            new
            {
                animate = EnableAnimation,
                legend = JS.Object(new { show = ShowLabels, placement = Enum.GetName(typeof(Placement), LegendPlacement).ToLower() }),
                series = labels,
                seriesDefaults = new { fill = Fill, fillToZero = FillToZero, rendererOptions = new { smooth = Smooth } }
            });

            if (Curves.Count > 0 && Curves.SelectMany(c => c.Points).Any())
            {
                options.Properties.Add(new KeyValuePair<Expression, Expression>(JS.Expression("axes"),
                 JS.Object(new
                 {
                     xaxis = new
                     {
                         renderer = JS.Expression("$.jqplot.DateAxisRenderer"),
                         tickOptions = new { formatString = XAxisFormat }
                     }
                 })));
            }

            return options;
        }

        /// <summary>
        ///   Retrieves the main script of the plotter.
        /// </summary>
        /// <returns> The main script of the plotter. </returns>
        protected override Script GetScript()
        {
            return JS.Script(JS.Expression("$.jqplot").Call(ClientID, GetCurves(), GetOptions()));
        }

        #endregion

        #region Helpers

        /// <summary>
        ///   Projects a curve into a jqPlot Javascript expression.
        /// </summary>
        /// <param name="curve"> The curve. </param>
        /// <returns> The jqPlot Javascript expression of the curve. </returns>
        private Expression GetExpression(Curve curve)
        {
            return JS.Array(
                curve.Points.Select(
                    point => JS.Array(
                        JS.New(JS.Expression("Date"),
                                point.X.Year,
                                point.X.Month - 1,
                                point.X.Day,
                                point.X.Hour,
                                point.X.Minute,
                                point.X.Second,
                                point.X.Millisecond), point.Y)));
        }

        #endregion
    }
}