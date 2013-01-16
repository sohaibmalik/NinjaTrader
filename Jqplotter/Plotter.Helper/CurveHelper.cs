#region

using System;
using System.Collections.Generic;
using Plotter.Core;

#endregion

namespace Plotter.Helper
{
    /// <summary>
    ///   Plotter Helper.
    /// </summary>
    public class CurveHelper
    {
        /// <summary>
        /// Creates a curve with random values.
        /// </summary>
        /// <param name="numberOfPoints">Number of points.</param>
        /// <returns>A random curve.</returns>
        public static Curve GetRandomCurve(int numberOfPoints)
        {
            List<Point> points = new List<Point>();
            Random random = new Random();
            DateTime startDate = new DateTime(1970, 1, 1);

            for (int i = 0; i < numberOfPoints; i++)
                points.Add(new Point(startDate.AddDays(i), (float)Math.Pow(random.NextDouble() - random.NextDouble(), 7)));

            Curve curve = new Curve("Curve " + Guid.NewGuid().ToString(), points);

            return curve;
        }

        /// <summary>
        /// Creates a curve with random values with error bands.
        /// </summary>
        /// <param name="numberOfPoints">Number of points.</param>
        /// <returns>A random curve with error bands.</returns>
        public static Curve GetRandomCurveWithErrorBands(int numberOfPoints)
        {
            List<Point> points = new List<Point>();
            Random random = new Random();
            DateTime startDate = new DateTime(1970, 1, 1);

            for (int i = 0; i < numberOfPoints; i++)
                points.Add(new Point(startDate.AddDays(i)
                    , (float)Math.Pow(random.NextDouble() - random.NextDouble(), 7)
                    , (float)Math.Pow(random.NextDouble() - random.NextDouble(), 7)
                    , (float)Math.Pow(random.NextDouble() - random.NextDouble(), 7)));

            Curve curve = new Curve(points);

            return curve;
        }
    }
}