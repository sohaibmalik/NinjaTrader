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

[assembly: CLSCompliant(true)]

namespace Plotter.Controls.GoogleChartControl
{
    /// <summary>
    ///   GoogleChart control.
    /// </summary>
    [ToolboxData("<{0}:GoogleChart runat=server></{0}:GoogleChart>")]
    [ToolboxBitmap(typeof(GoogleChart), "GoogleChart.ico")]
    public class GoogleChart : PlotterControl
    {
        #region Fields

        // const //
        private const string ScriptIncludeGoogleChartKey = "_GoogleChart";
        private const string JsApiUrl = "http://www.google.com/jsapi";

        #endregion

        #region Overridden methods

        /// <summary>
        ///   Retrieves the curves' ViewState key.
        /// </summary>
        /// <returns> The curves' ViewState key. </returns>
        protected override string GetCurvesViewStateKey()
        {
            return "Plotter.Controls.GoogleChartControl_Curves";
        }

        /// <summary>
        ///   Raises the PreRender event.
        /// </summary>
        /// <param name="e"> An EventArgs object that contains the event data. </param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Type type = GetType();
            WebHelper.RegisterClientScriptInclude(Page, ScriptIncludeGoogleChartKey, type, JsApiUrl);
        }

        /// <summary>
        ///   Retrieves the Curves as a Javascript Expression.
        /// </summary>
        /// <returns> The Javascript expression of the curves. </returns>
        protected override Expression GetCurves()
        {
            Point[] points = Curves.SelectMany(curve => curve.Points).ToArray();

            if (points.Any())
            {

                var rows = (from point in points
                            group point by point.X
                                into g
                                orderby g.Key
                                select new { X = g.Key, Points = g.ToArray() }).ToArray();

                List<Expression> values = new List<Expression>();

                int maxPoints = rows.Max(g => g.Points.Length);

                foreach (var row in rows)
                {
                    List<Expression> arrayContent = new List<Expression>();

                    arrayContent.Add(JS.New(JS.Expression("Date"), row.X.Year, row.X.Month-1, row.X.Day, row.X.Hour, row.X.Minute, row.X.Second, row.X.Millisecond));

                    foreach (var p in row.Points)
                    {
                        arrayContent.Add(p.Y);
                    }

                    if (row.Points.Length < maxPoints)
                    {
                        for (int i = 0; i < maxPoints - row.Points.Length; i++)
                        {
                            arrayContent.Add(JS.Null());
                        }

                    }

                    values.Add(JS.Array(arrayContent));
                }

                return JS.Array(values);
            }
            else
            {
                return JS.Array(JS.Array());
            }
        }

        /// <summary>
        ///   Retrieves the Options as a Javascript Expression.
        /// </summary>
        /// <returns> The Javascript expression of the options. </returns>
        protected override Expression GetOptions()
        {
            return JS.Object();
        }

        /// <summary>
        ///   Retrieves the main script of the plotter.
        /// </summary>
        /// <returns> The main script of the plotter.</returns>
        protected override Script GetScript()
        {
            return JS.Script(
                JS.Expression("google").Dot("load").Call("visualization", "1", JS.Object(new
                {
                    packages = JS.Array(JS.String("annotatedtimeline")),
                    callback = JS.Function().Do(GetSetOnLoadCallbackStatements())
                })));
        }

        #endregion

        #region Helper

        private List<Statement> GetSetOnLoadCallbackStatements()
        {
            List<Statement> statements = new List<Statement>();

            statements.Add(JS.Var(JS.Expression("data")).AssignWith(JS.New(JS.Expression("google").Dot("visualization").Dot("DataTable"))));
            statements.AddRange(GetColumns());
            statements.Add(GetRows());
            statements.Add(JS.New(JS.Expression("google").Dot("visualization").Dot("AnnotatedTimeLine"),
                           JS.Expression("document").Dot("getElementById").Call(ClientID)).Dot("draw").
                           Call(JS.Expression("data"), GetOptions()));

            return statements;

        }

        private List<Statement> GetColumns()
        {
            List<Statement> statements = new List<Statement>();
            statements.Add(JS.Expression("data").Dot("addColumn").Call("date", ""));
            int curves = Curves.Where(c => c.Points.Any()).Count();

            for (int i = 0; i < curves; i++)
            {
                statements.Add(JS.Expression("data").Dot("addColumn").Call("number", ""));
            }

            return statements;
        }

        private Statement GetRows()
        {
            Statement rows = JS.Expression("data").Dot("addRows").Call(GetCurves());
            return rows;
        }

        #endregion
    }
}