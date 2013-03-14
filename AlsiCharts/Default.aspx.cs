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
using System.Linq;
using System.Data.Linq;

namespace WebApplication1
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
         
            StringBuilder jqueryString = new StringBuilder();
           
            String jstring = @" $(function () {
            var chart;
            $(document).ready(function () {
                chart = new Highcharts.Chart({
                    chart: {
                        renderTo: 'linecontainer',
                        zoomType: 'xy'
                    },
                    title: {
                        text: 'Alsi Trade System Profit'
                    },
                    subtitle: {
                        text: 'Source: WorldClimate.com'
                    },
                    xAxis: [{
                        categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
                    'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
                    }],
                    yAxis: [{ // Primary yAxis
                        labels: {
                            formatter: function () {
                                return this.value + '°C';
                            },
                            style: {
                                color: '#89A54E'
                            }
                        },
                        title: {
                            text: 'Temperature',
                            style: {
                                color: '#89A54E'
                            }
                        },
                        opposite: true

                    }, { // Secondary yAxis
                        gridLineWidth: 0,
                        title: {
                            text: 'Rainfall',
                            style: {
                                color: '#4572A7'
                            }
                        },
                        labels: {
                            formatter: function () {
                                return this.value + ' mm';
                            },
                            style: {
                                color: '#4572A7'
                            }
                        }

                    }, { // Tertiary yAxis
                        gridLineWidth: 0,
                        title: {
                            text: 'Sea-Level Pressure',
                            style: {
                                color: '#AA4643'
                            }
                        },
                        labels: {
                            formatter: function () {
                                return this.value + ' mb';
                            },
                            style: {
                                color: '#AA4643'
                            }
                        },
                        opposite: true
                    }],
                    tooltip: {
                        formatter: function () {
                            var unit = {
                                'Rainfall': 'mm',
                                'Temperature': '°C',
                                'Sea-Level Pressure': 'mb'
                            }[this.series.name];

                            return '' +
                        this.x + ': ' + this.y + ' ' + unit;
                        }
                    },
                    legend: {
                        layout: 'vertical',
                        align: 'left',
                        x: 120,
                        verticalAlign: 'top',
                        y: 80,
                        floating: true,
                        backgroundColor: '#FFFFFF'
                    },
                    series: [{
                        name: 'Rainfall',
                        color: '#4572A7',
                        type: 'column',
                        yAxis: 1,
                        data:[J01]

                    }, {
                        name: 'Sea-Level Pressure',
                        type: 'spline',
                        color: '#AA4643',
                        yAxis: 2,
                        data: [J02],
                        marker: {
                            enabled: false
                        },
                        dashStyle: 'shortdot'

                    }, {
                        name: 'Temperature',
                        color: '#89A54E',
                        type: 'spline',
                        data: [J03]
                    }]
                });
            });

        });";



            
            AlsiUtils.DataBase.SetConnectionString(@"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True");
            var dc = new AlsiUtils.AlsiDBDataContext();
            string data = "";
            foreach (var d in dc.MasterMinutes.Where(z=>z.Stamp.Year==2013 && z.Stamp.Month==1 && z.Stamp.Day>25)) data += d.C.ToString() + ",";
            

            


            StringBuilder jstringb = new StringBuilder(jstring);       
            jstringb.Replace("J01", "49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4");
            jstringb.Replace("J02", data+"36000");
            jstringb.Replace("J03", "7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6");
            // Add the javascript to the page load to load the graph at page startup
            Page.ClientScript.RegisterStartupScript(this.GetType(), "jquerylinechart", jstringb.ToString(), true);
            
      

        }



    }
}