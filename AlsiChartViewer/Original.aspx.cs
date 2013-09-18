using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using WebApplication1.Properties;
using WebApplication1.Code;
using System.Globalization;


namespace WebApplication1
{
    public partial class Original : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // create a new stringbuilder for generating the highcharts javascript
            StringBuilder jqueryString = new StringBuilder();
            // create variables for simple editing of the chart. 
            // (I have substituted several hard coded settings with the variable name in the 
            //   javascript file containing the code for the graph)
            jqueryString.Append(" var lineContainerVar = 'linecontainer';");
            jqueryString.Append(" var lineTitleVar = 'Expenses';");
            // Read in the javascript file containing the javascript
            // The method of using resource files makes for cleaner code and easier editing of the graph javascript
            // Because of the use of brackets ({ }) in javascript you will find that you cannot use a string.Format()
            // to edit the javascript in code but have to resort to creating javascript parameters and Replace()
            jqueryString.Append(File.ReadAllText(Server.MapPath(Settings.Default.linechartlocation)));
            // Load in the values of the graph in the format [value,value,value,value],[value,value,value,value]
            jqueryString.Replace("[%seriesString%]", string.Concat("[", GetSeriesString(), "]"));
            // Add the javascript to the page load to load the graph at page startup
            Page.ClientScript.RegisterStartupScript(this.GetType(), "jquerylinechart", jqueryString.ToString(), true);


            jqueryString = new StringBuilder();
            jqueryString.Append(" var columnContainerVar = 'columncontainer';");
            jqueryString.Append(" var columnTitleVar = 'Expenses';");
            jqueryString.Append(File.ReadAllText(Server.MapPath(Settings.Default.columnchartlocation)));
            jqueryString.Replace("[%seriesString%]", string.Concat("[", GetSeriesString(), "]"));
            Page.ClientScript.RegisterStartupScript(this.GetType(), "jquerycolumnchart", jqueryString.ToString(), true);

            jqueryString = new StringBuilder();
            jqueryString.Append(" var pieContainerVar = 'piecontainer';");
            jqueryString.Append(" var pieTitleVar = 'Expenses';");
            jqueryString.Append(File.ReadAllText(Server.MapPath(Settings.Default.piechartlocation)));
            // Note that the seriesstring for the pie chart has a different format [description,value],[description,value] etc
            jqueryString.Replace("[%seriesString%]", string.Concat("[", GetPieChartSeriesString(), "]"));
            Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "jquerypiechart", jqueryString.ToString(), true);

        }


        private string GetSeriesString()
        {
            List<Expense> expenses = Expense.GetAmountRandomAmountList();

            List<string> expensesLinePoints = new List<string>();
            StringBuilder seriesString = new StringBuilder();

            foreach (Expense expenseItem in expenses)
            {
                expensesLinePoints.Add(expenseItem.Amount.ToString());

                if (expenseItem.Date.Month == 12)
                {
                    if (seriesString.Length > 0)
                    {
                        seriesString.Append(",");
                    }
                    seriesString.Append("{ ");
                    seriesString.AppendFormat(@"name: {0},
                            data: [{1}]", expenseItem.Date.Year, string.Join(",", expensesLinePoints.ToArray()));
                    seriesString.Append("  }");

                    expensesLinePoints = new List<string>();
                }
            }

            return seriesString.ToString();
        }

        private string GetPieChartSeriesString()
        {
            List<Expense> expenses = Expense.GetAmountRandomAmountList();

            List<string> expensesLinePoints = new List<string>();

            for (int index = 12; index < expenses.Count; index++)
            {
                expensesLinePoints.Add(string.Format("['{0}',{1}]",
                    expenses[index].Date.ToString("MMMM"),
                    expenses[index].Amount));

            }

            return string.Join(",", expensesLinePoints.ToArray());
        }

    }
}