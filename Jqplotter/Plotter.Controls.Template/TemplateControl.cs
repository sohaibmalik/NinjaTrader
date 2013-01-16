#region

using System;
using System.Drawing;
using System.Web.UI;
using Adam.JSGenerator;
using Plotter.Core;

#endregion

[assembly: TagPrefix("Plotter.Controls.Template", "asp")]
[assembly: CLSCompliant(true)]

//
// todo:add WebResources here
//

namespace Plotter.Controls.Template
{
    /// <summary>
    ///   Template control.
    /// </summary>
    [ToolboxData("<{0}:TemplateControl runat=server></{0}:TemplateControl>")]
    [ToolboxBitmap(typeof (TemplateControl), "TemplateControl.ico")]
    public class TemplateControl : PlotterControl
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        //
        // You can add the options here. 
        // Refer to this MSDN Documentation: http://msdn.microsoft.com/en-us/library/bb386519
        //

        #endregion

        #region Overridden Methods

        /// <summary>
        ///   Retrieves the curves' ViewState key.
        /// </summary>
        /// <returns> The curves' ViewState key. </returns>
        protected override string GetCurvesViewStateKey()
        {
            //
            // todo: Set the Curves' ViewState Key
            //
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Raises the PreRender event.
        /// </summary>
        /// <param name="e"> An EventArgs object that contains the event data. </param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //
            // todo: Add Script and CSS includes here, by using the Plotter.Helper.WebHelper
            //
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Retrieves the Curves as a Javascript Expression.
        /// </summary>
        /// <returns> The Javascript expression of the curves. </returns>
        protected override Expression GetCurves()
        {
            //
            // todo: implement this method with Adam.JSGenerator
            //
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Retrieves the Options as a Javascript Expression.
        /// </summary>
        /// <returns> The Javascript expression of the options. </returns>
        protected override Expression GetOptions()
        {
            //
            // todo: implement this method with Adam.JSGenerator
            //
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Retrieves the main script of the plotter.
        /// </summary>
        /// <returns> The main script of the plotter. </returns>
        protected override Script GetScript()
        {
            //
            // todo: implement this method with Adam.JSGenerator, GetCurves, and GetOptions
            //
            throw new NotImplementedException();
        }

        #endregion
    }
}