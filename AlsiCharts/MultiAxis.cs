using System.Text;

namespace AlsiCharts
{
    public class MultiAxis : Chart
    {

        public Series Series_A { get; set; }
        public Series Series_B { get; set; }
        public Series Series_C { get; set; }



        public MultiAxis()
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
                        color: '#89A54E'
                    }
                },
                title: {
                    text: '%A_SERIES_NAME%',
                    style: {
                        color: '#89A54E'
                    }
                },
                opposite: true
    
            }, { // Secondary yAxis
                gridLineWidth: 0,
                title: {
                    text: '%B_SERIES_NAME%',
                    style: {
                        color: '#4572A7'
                    }
                },
                labels: {
                    formatter: function() {
                        return this.value +' %B_YAXIS_UNIT%';
                    },
                    style: {
                        color: '#4572A7'
                    }
                }
    
            }, { // Tertiary yAxis
                gridLineWidth: 0,
                title: {
                    text: '%C_SERIES_NAME%',
                    style: {
                        color: '#AA4643'
                    }
                },
                labels: {
                    formatter: function() {
                        return this.value +' %C_YAXIS_UNIT%';
                    },
                    style: {
                        color: '#AA4643'
                    }
                },
                opposite: true
            }],
            tooltip: {
                formatter: function() {
                    var unit = {
                        '%B_SERIES_NAME%': '%B_YAXIS_UNIT%',
                        '%A_SERIES_NAME%': '%A_YAXIS_UNIT%',
                        '%C_SERIES_NAME%': '%C_YAXIS_UNIT%'
                    }[this.series.name];
    
                    return ''+
                        this.x +': '+ this.y +' '+ unit;
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
                name: '%B_SERIES_NAME%',
                color: '#4572A7',
                type: 'column',
                yAxis: 1,
                data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4]
    
            }, {
                name: '%C_SERIES_NAME%',
                type: 'spline',
                color: '#AA4643',
                yAxis: 2,
                data: [%C_DATA%],
                marker: {
                    enabled: false
                },
                dashStyle: 'shortdot'
    
            }, {
                name: '%A_SERIES_NAME%',
                color: '#89A54E',
                type: 'spline',
                data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
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
            Series_C = new Series();

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

            s.Replace("%A_YAXIS_UNIT%", Series_A.Unit);
            s.Replace("%B_YAXIS_UNIT%", Series_B.Unit);
            s.Replace("%C_YAXIS_UNIT%", Series_C.Unit);

            s.Replace("%A_SERIES_NAME%", Series_A.YaxixLabel);
            s.Replace("%B_SERIES_NAME%", Series_B.YaxixLabel);
            s.Replace("%C_SERIES_NAME%", Series_C.YaxixLabel);

            s.Replace("C_DATA", MakeYaxisData(Series_C.Data));





            Script = s.ToString();
        }






    }
}
