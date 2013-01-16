#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml.Linq;
using Plotter.Core;

#endregion

namespace Plotter.Samples.Flotr.CsvDownload
{
    public class Plot
    {
        #region Constructors and Destructors

        public Plot(int id, string title, string dataPath, string thumbnailPath, bool isRelativePath)
        {
            Id = id;
            Title = title;
            DataPath = dataPath;
            ThumbnailPath = thumbnailPath;
            IsRelativePath = isRelativePath;
            Description = string.Empty;
            Curves = new BindingList<Curve>();
            IsLoaded = false;
        }

        #endregion

        #region Public Properties

        public BindingList<Curve> Curves { get; private set; }

        public string DataPath { get; private set; }

        public string Description { get; private set; }

        public int Id { get; private set; }

        public bool IsLoaded { get; private set; }

        public bool IsRelativePath { get; private set; }

        public string ThumbnailPath { get; private set; }

        public string Title { get; private set; }

        #endregion

        #region Public Methods

        public void Load()
        {
            // If HttpContext is null and paths are relative, Throw an exception
            if (IsRelativePath && HttpContext.Current == null)
            {
                throw new Exception("HttpContext is null, you cannot use relative paths.");
            }

            // load the curves from the XML file
            XDocument data =
                XDocument.Load(IsRelativePath ? HttpContext.Current.Server.MapPath(DataPath) : DataPath);

            // Initialize the Curves collection
            Curves = new BindingList<Curve>();

            // Set current thread culture to en-US for float parsing
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            // Retrieve the plot current plot element from the XML file
            XElement xplot = data.Element("plot");

            // Retrieve the current Description<
            Description = xplot.Element("description").Value;

            // Retrieve the current curves
            IEnumerable<XElement> xcurves = xplot.Descendants("curve");

            // Enumerate curves
            // Build a the Curve object 
            // and Add it to the current Curves collection
            foreach (XElement xcurve in xcurves)
            {
                // Retrive the label
                string label = xcurve.Element("label").Value;

                // Initialize a new Set
                // Retrieve the points from the XML file
                IEnumerable<XElement> xpoints = xcurve.Descendants("point");

                // Enumerate the points
                // Parse and add their values to a new Point object
                // Build the Set
                List<Point> set = (from xpoint in xpoints
                                   let x = DateTime.Parse(xpoint.Attribute("x").Value)
                                   let y = Single.Parse(xpoint.Attribute("y").Value)
                                   select new Point(x, y)).ToList();

                // Add the curve to the Curves collection
                Curves.Add(new Curve(label, set));
            }

            // Set data as loaded
            IsLoaded = true;
        }

        #endregion
    }
}