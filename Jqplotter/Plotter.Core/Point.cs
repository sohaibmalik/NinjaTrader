#region

using System;

#endregion

namespace Plotter.Core
{
    /// <summary>
    ///   Represents a point.
    /// </summary>
    [Serializable]
    public class Point
    {
        #region Constructors

        /// <summary>
        ///   Initializes a new instance of the Point class.
        /// </summary>
        /// <param name="x"> X Coordinate. </param>
        /// <param name="y"> Y Coordinate. </param>
        public Point(DateTime x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        ///   Initializes a new instance of the Point class.
        /// </summary>
        /// <param name="x"> X Coordinate. </param>
        /// <param name="y"> Y Coordinate. </param>
        /// <param name="ymin"> Y Min Coordinate. </param>
        /// <param name="ymax"> Y Max Coordinate. </param>
        public Point(DateTime x, float y, float ymin, float ymax)
            : this(x, y)
        {
            YMin = ymin;
            YMax = ymax;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   X Coordinate.
        /// </summary>
        public DateTime X { get; set; }

        /// <summary>
        ///   Y Coordinate.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        ///   Y Min Coordinate (Optional).
        /// </summary>
        public float YMin { get; set; }

        /// <summary>
        ///   Y Max Coordinate (Optional).
        /// </summary>
        public float YMax { get; set; }

        #endregion
    }
}