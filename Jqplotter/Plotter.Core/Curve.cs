#region

using System;
using System.Collections.Generic;

#endregion

namespace Plotter.Core
{
    /// <summary>
    ///   Represents a curve.
    /// </summary>
    [Serializable]
    public class Curve
    {
        #region Constructors

        /// <summary>
        ///   Initializes a new instance of the Curve class.
        /// </summary>
        public Curve()
        {
            Points = new Point[] {};
        }

        /// <summary>
        ///   Initializes a new instance of the Curve class.
        /// </summary>
        /// <param name="label"> Label. </param>
        public Curve(string label)
            : this()
        {
            Label = label;
        }

        /// <summary>
        ///   Initializes a new instance of the Curve class.
        /// </summary>
        /// <param name="points"> Collection of points. </param>
        public Curve(List<Point> points)
        {
            Points = points.ToArray();
        }

        /// <summary>
        ///   Initializes a new instance of the Curve class.
        /// </summary>
        /// <param name="points"> Collection of points. </param>
        public Curve(Point[] points)
        {
            Points = points;
        }

        /// <summary>
        ///   Initializes a new instance of the Curve class.
        /// </summary>
        /// <param name="label"> Label. </param>
        /// <param name="points"> Collection of points. </param>
        public Curve(string label, List<Point> points)
            : this(label)
        {
            Points = points.ToArray();
        }

        /// <summary>
        ///   Initializes a new instance of the Curve class.
        /// </summary>
        /// <param name="label"> Label. </param>
        /// <param name="points"> Collection of points. </param>
        public Curve(string label, Point[] points)
            : this(label)
        {
            Points = points;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        ///   Collection of points.
        /// </summary>
        public Point[] Points { get; set; }

        #endregion
    }
}