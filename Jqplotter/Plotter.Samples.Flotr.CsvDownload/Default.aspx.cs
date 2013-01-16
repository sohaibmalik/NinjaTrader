#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

#endregion

namespace Plotter.Samples.Flotr.CsvDownload
{
    public partial class Default : Page
    {
        #region Properties

        private int CurrentPage
        {
            get
            {
                // Look for current page in ViewState
                object o = ViewState["CurrentPage"];
                if (o == null)
                {
                    return 1; // default page index of 1
                }
                return (int) o;
            }

            set { ViewState["CurrentPage"] = value; }
        }

        // Use ASP .NET Cache to boost paging performance
        private IList<Plot> Plots
        {
            get
            {
                if (Cache["Plots"] == null)
                {
                    // Read plots info from XML document
                    XDocument xdoc = XDocument.Load(MapPath("/App_Data/plots.xml"));
                    IList<Plot> plots =
                        xdoc.Descendants("plot").Select(
                            p =>
                            new Plot(
                                int.Parse(p.Attribute("id").Value),
                                p.Attribute("title").Value,
                                p.Attribute("dataUrl").Value,
                                p.Attribute("thumbnailUrl").Value,
                                true)).ToList();

                    Cache["Plots"] = plots;
                }
                return (IList<Plot>) Cache["Plots"];
            }
        }

        #endregion

        #region Methods

        protected void ImageButtonNext_Click(object sender, EventArgs e)
        {
            // Set viewstate variable to the next page
            CurrentPage++;

            // Reload control
            LoadPlots();
        }

        protected void ImageButtonPrevious_Click(object sender, EventArgs e)
        {
            // Set viewstate variable to the previous page
            CurrentPage--;

            // Reload control
            LoadPlots();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Reload control if the page is being rendered for the first time 
            // or isn't being loaded in response to a postback
            if (!IsPostBack)
            {
                LoadPlots();
            }
        }

        protected void RepeaterImages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("Plot"))
            {
                // Retrieve the plot Id from the command argument
                int plotId = int.Parse(e.CommandArgument.ToString());

                // Load the selected plot
                Plot plot = Plots.First(i => i.Id == plotId);

                // Plot the selected plot
                Plot(plot);

                // Display the Description View
                metadata.Visible = true;
            }
        }

        private void LoadPlots()
        {
            // Populate the repeater control with the plots collection
            // Indicate that the data should be paged
            // Set the number of plots you wish to display per page
            // Set the PagedDataSource's current page
            var pds = new PagedDataSource
                          {DataSource = Plots, AllowPaging = true, PageSize = 4, CurrentPageIndex = CurrentPage - 1};

            LabelCurrentPage.Text = "Page " + CurrentPage + " of " + pds.PageCount;

            // Hide Previous or Next buttons if necessary
            ImageButtonPrevious.Visible = !pds.IsFirstPage;
            ImageButtonNext.Visible = !pds.IsLastPage;

            // Hide the Description view
            metadata.Visible = false;

            // Bind data to the Plots Repeater
            RepeaterPlots.DataSource = pds;
            RepeaterPlots.DataBind();
        }

        private void Plot(Plot plot)
        {
            //
            // Load the plot lazily
            //
            if (!plot.IsLoaded) plot.Load();

            //
            // Populate the Flotr control with the curves
            //
            Flotr.Curves = plot.Curves;

            //
            // Bind the description view
            //
            LabelDescription.Text = string.IsNullOrEmpty(plot.Description)
                                        ? "No description available."
                                        : plot.Description;
        }

        #endregion
    }
}