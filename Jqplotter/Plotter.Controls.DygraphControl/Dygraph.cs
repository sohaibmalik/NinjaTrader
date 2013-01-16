#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using Adam.JSGenerator;
using Plotter.Core;
using Plotter.Helper;
using Point = Plotter.Core.Point;

#endregion

[assembly: TagPrefix("Plotter.Controls.DygraphControl", "asp")]
[assembly: WebResource("Plotter.Controls.DygraphControl._dygraph-combined.js", "text/javascript")]
[assembly: WebResource("Plotter.Controls.DygraphControl._excanvas.js", "text/javascript")]
[assembly: CLSCompliant(true)]

namespace Plotter.Controls.DygraphControl
{
    /// <summary>
    ///   Dygraph control.
    /// </summary>
    [ToolboxData("<{0}:Dygraph runat=server></{0}:Dygraph>")]
    [ToolboxBitmap(typeof(Dygraph), "Dygraph.ico")]
    public class Dygraph : PlotterControl
    {
        #region Fields

        private const string ScriptIncludeDygraphKey = "_dygraph";
        private const string ScriptIncludeExcanvasKey = "_excanvas";
        private bool _animatedZooms;
        private decimal _circleSize;
        private bool _enableErrorBands;
        private Color _gridLineColor;
        private int _rangeSelectorHeight;
        private Color _rangeSelectorPlotFillColor;
        private Color _rangeSelectorPlotStrokeColor;
        private int _rollPeriod;
        private bool _showLabels;
        private bool _showRangeSelector;
        private bool _showRoller;
        private decimal _strokeWidth;
        private string _title;
        private float _yHighRange;
        private string _yLabel;
        private float _yLowRange;

        private string _labelsDivId;
        private string _labelsDivStyle;
        private int _labelsDivWidth;
        private bool _labelsSeparateLines;
        private bool _labelsShowZeroValues;

        // const //

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Dygraph control.
        /// </summary>
        public Dygraph()
        {
            Title = string.Empty;
            CircleSize = 4.25m;
            StrokeWidth = 0.75m;
            GridLineColor = Color.Gray;
            ShowRoller = false;
            ShowLabels = false;
            EnableErrorBands = false;
            YLabel = string.Empty;
            RollPeriod = 0;
            ShowRangeSelector = false;
            RangeSelectorHeight = 30;
            RangeSelectorPlotStrokeColor = Color.Yellow;
            RangeSelectorPlotFillColor = Color.LightYellow;
            AnimatedZooms = false;
            YLowRange = float.NaN;
            YHighRange = float.NaN;
            _labelsDivWidth = 250;
            _labelsSeparateLines = false;
            _labelsShowZeroValues = true;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Title. Empty by default.
        /// </summary>
        [Bindable(true)]
        [Localizable(true)]
        [Description("Title. Empty by default.")]
        public string Title
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_Title"];
                    return o == null ? string.Empty : (string)o;
                }
                return _title;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_Title"] = value;
                _title = value;
            }
        }

        /// <summary>
        ///   Circle size. 4.25 by default.
        /// </summary>
        [Bindable(true)]
        [Description("Circle size. 4.25 by default.")]
        public decimal CircleSize
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_CircleSize"];
                    return o == null ? 4.25m : (decimal)o;
                }
                return _circleSize;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_CircleSize"] = value;
                _circleSize = value;
            }
        }

        /// <summary>
        ///   Stroke Width. 0.75 by default.
        /// </summary>
        [Bindable(true)]
        [Description("Stroke Width. 0.75 by default.")]
        public decimal StrokeWidth
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_StrokeWidth"];
                    return o == null ? 0.75m : (decimal)o;
                }
                return _strokeWidth;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_StrokeWidth"] = value;
                _strokeWidth = value;
            }
        }

        /// <summary>
        ///   Grid Line Color. Gray by default.
        /// </summary>
        [Bindable(true)]
        [Description("Grid Line Color. Gray by default.")]
        public Color GridLineColor
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_GridLineColor"];
                    return o == null ? Color.Gray : (Color)o;
                }
                return _gridLineColor;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_GridLineColor"] = value;
                _gridLineColor = value;
            }
        }

        /// <summary>
        ///   Indicates whether error bands are enabled or not. False by default.
        /// </summary>
        [Bindable(true)]
        [Description("Indicates whether error bands are enabled or not. False by default.")]
        public bool EnableErrorBands
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_EnableErrorBands"];
                    return o != null && (bool)o;
                }
                return _enableErrorBands;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_EnableErrorBands"] = value;
                _enableErrorBands = value;
            }
        }

        /// <summary>
        ///   Indicates whether roller should be enabled or not. False by default.
        /// </summary>
        [Bindable(true)]
        [Description("Indicates whether error bands should be enabled or not. False by default.")]
        public bool ShowRoller
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_ShowRoller"];
                    return o != null && (bool)o;
                }
                return _showRoller;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_ShowRoller"] = value;
                _showRoller = value;
            }
        }

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
                    object o = ViewState["Plotter.Controls.DygraphControl_ShowLabels"];
                    return o != null && (bool)o;
                }
                return _showLabels;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_ShowLabels"] = value;
                _showLabels = value;
            }
        }

        /// <summary>
        ///   Indicates whether the range selector is shown or not. False by default. When activated this option disables zooming with the cursor.
        /// </summary>
        [Bindable(true)]
        [Description(
            "Indicates whether the range selector is shown or not. False by default. When activated this option disables zooming with the cursor."
            )]
        public bool ShowRangeSelector
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_ShowRangeSelector"];
                    return o != null && (bool)o;
                }
                return _showRangeSelector;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_ShowRangeSelector"] = value;
                _showRangeSelector = value;
            }
        }

        /// <summary>
        ///   The Height of the range selector. 30 pixels by default.
        /// </summary>
        [Bindable(true)]
        [Description("The Height of the range selector. 30 pixels by default.")]
        public int RangeSelectorHeight
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_RangeSelectorHeight"];
                    return o == null ? 30 : (int)o;
                }
                return _rangeSelectorHeight;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_RangeSelectorHeight"] = value;
                _rangeSelectorHeight = value;
            }
        }

        /// <summary>
        ///   The color of the RangeSelector plot stroke. Yellow by default.
        /// </summary>
        [Bindable(true)]
        [Description("The color of the RangeSelector plot stroke. Yellow by default.")]
        public Color RangeSelectorPlotStrokeColor
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_RangeSelectorPlotStrokeColor"];
                    return o == null ? Color.Yellow : (Color)o;
                }
                return _rangeSelectorPlotStrokeColor;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_RangeSelectorPlotStrokeColor"] = value;
                _rangeSelectorPlotStrokeColor = value;
            }
        }

        /// <summary>
        ///   The RangeSelector plot fill color. LightYellow by default.
        /// </summary>
        [Bindable(true)]
        [Description("The RangeSelector plot fill color. LightYellow by default.")]
        public Color RangeSelectorPlotFillColor
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_RangeSelectorPlotFillColor"];
                    return o == null ? Color.LightYellow : (Color)o;
                }
                return _rangeSelectorPlotFillColor;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_RangeSelectorPlotFillColor"] = value;
                _rangeSelectorPlotFillColor = value;
            }
        }

        /// <summary>
        ///   Indicates whether the zoom animation is enabled or not. False by default.
        /// </summary>
        [Bindable(true)]
        [Description("Indicates whether the zoom animation is enabled or not. False by default.")]
        public bool AnimatedZooms
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_AnimatedZooms"];
                    return o != null && (bool)o;
                }
                return _animatedZooms;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_AnimatedZooms"] = value;
                _animatedZooms = value;
            }
        }

        /// <summary>
        ///   Y axis Label. Empty by default.
        /// </summary>
        [Bindable(true)]
        [Localizable(true)]
        [Description("Y axis Label. Empty by default.")]
        public string YLabel
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_YLabel"];
                    return o == null ? string.Empty : (string)o;
                }
                return _yLabel;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_YLabel"] = value;
                _yLabel = value;
            }
        }

        /// <summary>
        ///   Rolling Period. 0 By default.
        /// </summary>
        [Bindable(true)]
        [Localizable(true)]
        [Description("Roll Period.")]
        public int RollPeriod
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_RollPeriod"];
                    return o == null ? 0 : (int)o;
                }
                return _rollPeriod;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_RollPeriod"] = value;
                _rollPeriod = value;
            }
        }

        /// <summary>
        ///   The vertical low range of the graph. Low range of the input is shown by default.
        /// </summary>
        [Bindable(true)]
        [Localizable(true)]
        [Description("Optional. The vertical low range of the graph. The lowest range of the input is shown by default."
            )]
        public float YLowRange
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_YLowRange"];
                    return o == null ? float.NaN : (float)o;
                }
                return _yLowRange;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_YLowRange"] = value;
                _yLowRange = value;
            }
        }


        /// <summary>
        ///   The vertical high range of the graph. Highest range of the input is shown by default.
        /// </summary>
        [Bindable(true)]
        [Localizable(true)]
        [Description(
            "Optional. The vertical high range of the graph. The highest range of the input is shown by default.")]
        public float YHighRange
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_YHighRange"];
                    return o == null ? float.NaN : (float)o;
                }
                return _yHighRange;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_YHighRange"] = value;
                _yHighRange = value;
            }
        }

        /// <summary>
        /// Show data labels in an external div, rather than on the graph. Defaults to null. 
        /// </summary>
        [Bindable(true)]
        [Localizable(true)]
        [Description("Show data labels in an external div, rather than on the graph. Defaults to null.")]
        public string LabelsDivId
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_LabelsDivId"];
                    return o == null ? string.Empty : (string)o;
                }
                return _labelsDivId;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_LabelsDivId"] = value;
                _labelsDivId = value;
            }
        }

        /// <summary>
        /// Additional styles to apply to the currently-highlighted points div. For example, "fontWeight: bold;" will make the labels bold. Defaults to null.
        /// </summary>
        [Bindable(true)]
        [Localizable(true)]
        [Description("Additional styles to apply to the currently-highlighted points div. For example, 'fontWeight: bold;' will make the labels bold. Defaults to null.")]
        public string LabelsDivStyle
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_LabelsDivStyle"];
                    return o == null ? string.Empty : (string)o;
                }
                return _labelsDivStyle;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_LabelsDivStyle"] = value;
                _labelsDivStyle = value;
            }
        }

        /// <summary>
        /// Width (in pixels) of the div which shows information on the currently-highlighted points. Defaults to 250 pixels.
        /// </summary>
        [Bindable(true)]
        [Localizable(true)]
        [Description("Width (in pixels) of the div which shows information on the currently-highlighted points. Defaults to 250 pixels.")]
        public int LabelsDivWidth
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_LabelsDivWidth"];
                    return o == null ? 0 : (int)o;
                }
                return _labelsDivWidth;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_LabelsDivWidth"] = value;
                _labelsDivWidth = value;
            }
        }

        /// <summary>
        ///  Put br between lines in the label string. Often used in conjunction with labelsDiv. Defaults to false.
        /// </summary>
        [Bindable(true)]
        [Description("Put br between lines in the label string. Often used in conjunction with labelsDiv. Defaults to false.")]
        public bool LabelsSeparateLines
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_LabelsSeparateLines"];
                    return o != null && (bool)o;
                }
                return _labelsSeparateLines;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_LabelsSeparateLines"] = value;
                _labelsSeparateLines = value;
            }
        }

        /// <summary>
        /// Show zero value labels in the labelsDiv. Defaults to true.
        /// </summary>
        [Bindable(true)]
        [Description("Show zero value labels in the labelsDiv. Defaults to true.")]
        public bool LabelsShowZeroValues
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState["Plotter.Controls.DygraphControl_LabelsShowZeroValues"];
                    return o != null && (bool)o;
                }
                return _labelsShowZeroValues;
            }
            set
            {
                if (EnableViewState) ViewState["Plotter.Controls.DygraphControl_LabelsShowZeroValues"] = value;
                _labelsShowZeroValues = value;
            }
        }


        #endregion

        #region Overridden methods

        /// <summary>
        ///   Retrieves the curves' ViewState key.
        /// </summary>
        /// <returns> The curves' ViewState key. </returns>
        protected override string GetCurvesViewStateKey()
        {
            return "Plotter.Controls.DygraphControl_Curves";
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
            WebHelper.RegisterExcanvas(Page, ScriptIncludeExcanvasKey, type, "Plotter.Controls.DygraphControl._excanvas.js");
            WebHelper.RegisterClientScriptResourceInclude(Page, ScriptIncludeDygraphKey, type, "Plotter.Controls.DygraphControl._dygraph-combined.js");
        }

        /// <summary>
        ///   Retrieves the Curves as a Javascript Expression.
        /// </summary>
        /// <returns> The Javascript expression of the curves. </returns>
        protected override Expression GetCurves()
        {
            Point[] points = Curves.SelectMany(curve => curve.Points).ToArray();
            ArrayExpression curves = points.Any()
            ? JS.Array(
                from point in points
                group point by point.X into g
                orderby g.Key
                let gcount = g.Count()
                select JS.Array(
                    new Expression[] { JS.New(JS.Expression("Date"), g.Key.Year, g.Key.Month - 1, g.Key.Day, g.Key.Hour, g.Key.Minute, g.Key.Second, g.Key.Millisecond) }
                .Concat((EnableErrorBands ? (from p in g select JS.Array(p.YMin, p.Y, p.YMax)) : (from p in g select JS.Array(p.Y))))
                .Concat((from i in Enumerable.Range(1, Curves.Count - gcount) where gcount < Curves.Count select JS.Array()))))
            : JS.Array(JS.Array(0));
            return curves;
        }

        /// <summary>
        ///   Retrieves the Options as a Javascript Expression.
        /// </summary>
        /// <returns> The Javascript expression of the options. </returns>
        protected override Expression GetOptions()
        {
            //
            // Calculate the ValueRange.
            //
            List<Point> points = new List<Point>();
            bool yLowRangeIsNan = float.IsNaN(YLowRange);
            bool yHighRangeIsNan = float.IsNaN(YHighRange);
            if (yLowRangeIsNan || yHighRangeIsNan) points = Curves.SelectMany(c => c.Points).ToList();
            if (yLowRangeIsNan) YLowRange = points.Count == 0 ? 0 : points.Min(p => EnableErrorBands ? p.YMin : p.Y);
            if (yHighRangeIsNan) YHighRange = points.Count == 0 ? 10 : points.Max(p => EnableErrorBands ? p.YMax : p.Y);
            ArrayExpression valueRange = JS.Array(YLowRange, YHighRange);

            //
            // Return the options as a Javascript Expression.
            //
            ObjectLiteralExpression options = JS.Object(new
            {
                customBars = EnableErrorBands,
                gridLineColor = ColorTranslator.ToHtml(GridLineColor),
                highlightCircleSize = CircleSize,
                strokeWidth = StrokeWidth,
                title = Title,
                ylabel = YLabel,
                rollPeriod = ShowRoller ? RollPeriod : 0,
                showRoller = ShowRoller,
                showRangeSelector = ShowRangeSelector,
                rangeSelectorHeight = RangeSelectorHeight,
                rangeSelectorPlotStrokeColor =
                ColorTranslator.ToHtml(RangeSelectorPlotStrokeColor),
                rangeSelectorPlotFillColor = ColorTranslator.ToHtml(RangeSelectorPlotFillColor),
                animatedZooms = AnimatedZooms,
                valueRange
            });

            if (ShowLabels)
            {
                // Project the curves' labels into a javascript Expression if necessary
                ArrayExpression labels = JS.Array(Curves.Select(c => c.Label));

                // insert "Date" at the index 0
                labels.Elements.Insert(0, JS.String("Date"));

                // Add labels
                options.Properties.Add(new KeyValuePair<Expression, Expression>(JS.Expression("legend"), "always"));
                options.Properties.Add(new KeyValuePair<Expression, Expression>(JS.Expression("labels"), labels));

                // Add legend options
                if (!string.IsNullOrEmpty(LabelsDivId))
                {
                    options.Properties.Add(new KeyValuePair<Expression, Expression>(JS.Expression("labelsDiv"), LabelsDivId));
                }

                options.Properties.Add(new KeyValuePair<Expression, Expression>(JS.Expression("labelsDivWidth"), LabelsDivWidth));
                options.Properties.Add(new KeyValuePair<Expression, Expression>(JS.Expression("labelsSeparateLines"), LabelsSeparateLines));
                options.Properties.Add(new KeyValuePair<Expression, Expression>(JS.Expression("labelsShowZeroValues"), LabelsShowZeroValues));

                if (!string.IsNullOrEmpty(LabelsDivStyle))
                {
                    options.Properties.Add(new KeyValuePair<Expression, Expression>(JS.Expression("labelsDivStyles"), LabelsDivStyleToExpression()));
                }
            }

            return options;
        }

        /// <summary>
        ///   Retrieves the main script of the plotter.
        /// </summary>
        /// <returns> The main script of the plotter.</returns>
        protected override Script GetScript()
        {
            return JS.Script(JS.New(JS.Expression("Dygraph"), JS.Expression("document").Dot("getElementById").Call(ClientID), GetCurves(), GetOptions()));
        }

        #endregion

        #region Helpers

        private ObjectLiteralExpression LabelsDivStyleToExpression()
        {
            // a1:v1; a2:v2; => {'a1':'v1', 'a2':'v2'}
            string[] styles = LabelsDivStyle.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            ObjectLiteralExpression stylesObj = JS.Object();

            foreach (string style in styles)
            {
                string[] vals = style.Split(':');
                stylesObj.Properties.Add(new KeyValuePair<Expression, Expression>(vals[0].Trim(), vals[1].Trim()));
            }

            return stylesObj;
        }

        #endregion
    }
}