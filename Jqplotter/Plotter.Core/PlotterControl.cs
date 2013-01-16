#region

using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Adam.JSGenerator;

#endregion

[assembly: CLSCompliant(true)]

namespace Plotter.Core
{
    /// <summary>
    ///   Plotter Control.
    /// </summary>
    public class PlotterControl : Panel
    {
        #region Fields

        private BindingList<Curve> _curves;

        #endregion

        #region Constructors

        /// <summary>
        ///   Initialises a new instance of the *plotter control.
        /// </summary>
        public PlotterControl()
        {
            Curves = new BindingList<Curve>();
            Curves.ListChanged += CurvesListChanged;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Collection of curves.
        /// </summary>
        [Bindable(true)]
        [CLSCompliant(false)]
        public BindingList<Curve> Curves
        {
            get
            {
                if (EnableViewState)
                {
                    object o = ViewState[GetCurvesViewStateKey()];
                    return o == null ? new BindingList<Curve>() : (BindingList<Curve>)o;
                }
                return _curves;
            }
            set
            {
                if (EnableViewState) ViewState[GetCurvesViewStateKey()] = value;
                _curves = value;
            }
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        ///   Retrieves the curves' ViewState key.
        /// </summary>
        /// <returns> The curves' ViewState key. </returns>
        protected virtual string GetCurvesViewStateKey()
        {
            return null;
        }

        /// <summary>
        ///   Retrieves the Curves as a Javascript Expression.
        /// </summary>
        /// <returns> The Javascript expression of the curves. </returns>
        protected virtual Expression GetCurves()
        {
            return null;
        }

        /// <summary>
        ///   Retrieves the Options as a Javascript Expression.
        /// </summary>
        /// <returns> The Javascript expression of the options. </returns>
        protected virtual Expression GetOptions()
        {
            return null;
        }

        /// <summary>
        ///   Retrieves the main script of the plotter.
        /// </summary>
        /// <returns> The main script of the plotter. </returns>
        protected virtual Script GetScript()
        {
            return null;
        }

        #endregion

        #region Overridden methods

        /// <summary>
        /// Renders the contents of the Plotter control.
        /// </summary>
        /// <param name="output"> A HtmlTextWriter that represents the output stream to render HTML content on the client. </param>
        protected sealed override void RenderContents(HtmlTextWriter output)
        {
            Plot();
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Registers the startup script with the Plotter Control using a type, a key, and a script literal.
        /// </summary>
        private void Plot()
        {
            Type type = GetType();
            string key = Guid.NewGuid().ToString();
            string script = GetScript().ToString();
            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            bool isMsAjaxEnabled = scriptManager != null;

            if (isMsAjaxEnabled && scriptManager.IsInAsyncPostBack)
            {
                ScriptManager.RegisterClientScriptBlock(this, type, key, script, true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(type, key, script, true);
            }
        }

        #endregion

        #region Handlers

        /// <summary>
        ///   Occurs when the Curves collection or a curve changes.
        /// </summary>
        /// <param name="sender"> The Sender. </param>
        /// <param name="e"> The List Changed Event Arguments. </param>
        protected void CurvesListChanged(object sender, ListChangedEventArgs e)
        {
            if (EnableViewState) ViewState[GetCurvesViewStateKey()] = Curves;
        }

        #endregion
    }
}