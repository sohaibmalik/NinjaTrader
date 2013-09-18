using System.Text;

namespace AlsiCharts //http://www.highcharts.com/demo/
{
    public class MultiAxis_2 : Chart
    {

        public Series Series_A { get; set; }
        public Series Series_B { get; set; }
      



        public MultiAxis_2()
        {
            #region Set Script

            this.Script =
           @"<!DOCTYPE HTML>
<html>
	<head>
		<meta http-equiv=*Content-Type* content=*text/html; charset=utf-8*>
		<title>%WEB_TAB_TITLE%</title>

		<script type=*text/javascript* src=*http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js*></script>
		<script type=*text/javascript*>
$(function () {
    var chart;
    $(document).ready(function() {
        chart = new Highcharts.Chart({
            chart: {
                renderTo: 'container',
                zoomType: 'xy'
            },
            title: {
                text: '%TITLE%'
            },
            subtitle: {
                text: '%SUBTITLE%'
            },
            xAxis: [{
              
                categories: [%X_LABELS%]
            }],
            yAxis: [{ // Primary yAxis
                labels: {
                    formatter: function() {
                        return this.value +' %A_YAXIS_UNIT%';
                    },
                    style: {
                        color: '%A_YAXIX_UNIT_COLOR%'
                    }
                },
                title: {
                    text: '%A_SERIES_NAME%',
                    style: {
                        color: '%A_YAXIX_TITLE_COLOR%'
                    }
                },
                opposite: %A_OPPOSITE%
    
            }, { // Secondary yAxis
                gridLineWidth: 0,
                title: {
                    text: '%B_SERIES_NAME%',
                    style: {
                        color: '%B_YAXIX_UNIT_COLOR%'
                    }
                },
                labels: {
                    formatter: function() {
                        return this.value +' %B_YAXIS_UNIT%';
                    },
                    style: {
                        color: '%B_YAXIX_TITLE_COLOR%'
                    }
                }, opposite: %B_OPPOSITE%
    
            }],
            tooltip: {shared:%SHARED_TOOLTIP%},         
                            
            legend: {
             
                backgroundColor: '%LEGEND_BACK_COLOR%',
              
            },
            series: [
               {
                name: '%A_SERIES_NAME%',
                color: '%A_YAXIX_TITLE_COLOR%',
                yAxis:%A_AXISNUM%,
                type: '%A_LINETYPE%',
                data: [%A_DATA%],
                dashStyle: '%A_DASHSTYLE%',
                marker: {enabled: false},
                tooltip:{ valueSuffix: ' %A_YAXIS_UNIT%'}
            },{
                name: '%B_SERIES_NAME%',
                color: '%B_YAXIX_TITLE_COLOR%',
                type: '%B_LINETYPE%',
                yAxis: %B_AXISNUM%,
                data: [%B_DATA%],
                dashStyle: '%B_DASHSTYLE%',
                marker: {enabled: false},
               tooltip:{ valueSuffix: ' %B_YAXIS_UNIT%'}              
            }]
        });
    });
    
});
		</script>
	</head>
	<body>
<script src=*http://code.highcharts.com/highcharts.js*></script>
<script src=*http://code.highcharts.com/modules/exporting.js*></script>

<div id=*container* style=*min-width: %WIDTH_PX%px; height: %HEIGHT_PX%px; margin: 0 auto*></div>

	</body>
</html>

";
            #endregion

            Series_A = new Series();
            Series_B = new Series();
            

        }

        public override void PopulateScript()
        {
            StringBuilder s = new StringBuilder(Script);
            s.Replace("%WEB_TAB_TITLE%", this.WebTabTitle);
            s.Replace("%TITLE%", this.Title);
            s.Replace("%SUBTITLE%", this.Subtitle);
            s.Replace("%WIDTH_PX%", this.Width.ToString());
            s.Replace("%HEIGHT_PX%", this.Height.ToString());
            s.Replace("%X_LABELS%", this.MakeXaxisLabels());
            s.Replace("%LEGEND_BACK_COLOR%", LegendBackColorHEX);
            s.Replace("%SHARED_TOOLTIP%", SharedTootltip.ToString().ToLower());

            s.Replace("%A_AXISNUM%", this.Series_A.YaxisNumber.ToString());
            s.Replace("%B_AXISNUM%", this.Series_B.YaxisNumber.ToString());
         

            s.Replace("%A_OPPOSITE%", Series_A.AxisOppositeSide.ToString().ToLower());
            s.Replace("%B_OPPOSITE%", Series_B.AxisOppositeSide.ToString().ToLower());
           

            s.Replace("%A_YAXIS_UNIT%", Series_A.Unit);
            s.Replace("%B_YAXIS_UNIT%", Series_B.Unit);
            

            s.Replace("%A_SERIES_NAME%", Series_A.YaxixLabel);
            s.Replace("%B_SERIES_NAME%", Series_B.YaxixLabel);
           

            s.Replace("%A_LINETYPE%", Series_A.LineStyle.ToString());
            s.Replace("%B_LINETYPE%", Series_B.LineStyle.ToString());
          

            s.Replace("%A_DATA%", MakeYaxisData(Series_A.Data));
            s.Replace("%B_DATA%", MakeYaxisData(Series_B.Data));
           

            s.Replace("%A_YAXIX_UNIT_COLOR%", Series_A.YaxisUnitColorHEX);
            s.Replace("%A_YAXIX_TITLE_COLOR%", Series_A.YaxisTitleColorHEX);

            s.Replace("%B_YAXIX_UNIT_COLOR%", Series_B.YaxisUnitColorHEX);
            s.Replace("%B_YAXIX_TITLE_COLOR%", Series_B.YaxisTitleColorHEX);


            s.Replace("%A_DASHSTYLE%", Series_A.DashStyle.ToString());
            s.Replace("%B_DASHSTYLE%", Series_B.DashStyle.ToString());
          

            Script = s.ToString();
        }






    }
}
