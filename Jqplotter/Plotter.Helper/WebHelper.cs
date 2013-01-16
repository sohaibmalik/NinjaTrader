#region

using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

#endregion

namespace Plotter.Helper
{
    /// <summary>
    ///   Plotter Web Helper.
    /// </summary>
    public class WebHelper
    {
        #region Helpers

        /// <summary>
        ///   Forces IE8 rendering the webpage in IE7's Standards mode and IE9 rendering the webpage in IE9’s Standards mode.
        /// </summary>
        /// <param name="page"> Page object. </param>
        public static void EmulateIE(Page page)
        {
            string userAgent = page.Request.UserAgent;

            if (userAgent != null && (userAgent.Contains("MSIE") && page.Header != null))
            {
                page.Header.Controls.Add(new HtmlMeta
                                             {
                                                 HttpEquiv = "X-UA-Compatible",
                                                 Content = "IE=EmulateIE7; IE=EmulateIE9"
                                             });
            }
        }

        /// <summary>
        ///   Registers the client Web Ressource Excanvas script include with the Page object.
        /// </summary>
        /// <param name="page"> The Page object. </param>
        /// <param name="key"> The Script include key. </param>
        /// <param name="type"> The type of the client script include to register. </param>
        /// <param name="resourceName"> The excanvas ressource name. </param>
        public static void RegisterExcanvas(Page page, string key, Type type, string resourceName)
        {
            string userAgent = page.Request.UserAgent;

            // excanvas.js is required only for IE versions below 9
            if (userAgent != null && (userAgent.Contains("MSIE 6") || userAgent.Contains("MSIE 7") || userAgent.Contains("MSIE 8")))
            {
                RegisterClientScriptResourceInclude(page, key, type, resourceName);
            }
        }

        /// <summary>
        /// Registers the client Web Ressource script include with the Page object.
        /// </summary>
        /// <param name="page"> The Page object. </param>
        /// <param name="key"> The Script include key. </param>
        /// <param name="type"> The type of the client script include to register. </param>
        /// <param name="resourceName"> The ressource name. </param>
        public static void RegisterClientScriptResourceInclude(Page page, string key, Type type, string resourceName)
        {
            if (!page.ClientScript.IsClientScriptIncludeRegistered(key))
            {
                page.ClientScript.RegisterClientScriptInclude(type, key,
                                                              page.ClientScript.GetWebResourceUrl(type, resourceName));
            }
        }

        /// <summary>
        ///   Registers the client Web Ressource script include with the Page object.
        /// </summary>
        /// <param name="page"> The Page object. </param>
        /// <param name="key"> The Script include key. </param>
        /// <param name="type"> The type of the client script include to register. </param>
        /// <param name="resourceName"> The ressource name. </param>
        public static void RegisterClientScriptInclude(Page page, string key, Type type, string url)
        {
            if (!page.ClientScript.IsClientScriptIncludeRegistered(key))
            {
                page.ClientScript.RegisterClientScriptInclude(type, key, url);
            }
        }

        /// <summary>
        ///   Registers the client Web Ressource CSS include with the Page object.
        /// </summary>
        /// <param name="page"> The Page object. </param>
        /// <param name="type"> The type of the client script include to register. </param>
        /// <param name="resourceName"> The ressource name. </param>
        public static void RegisterCSSInclude(Page page, Type type, string resourceName)
        {
            if (page.Header != null)
            {
                page.Header.Controls.Add(
                    new LiteralControl(
                        "<link rel=\"stylesheet\" type=\"text/css\" href=\""
                        + page.ClientScript.GetWebResourceUrl(type, resourceName) + "\" />"));
            }
        }

        #endregion
    }
}