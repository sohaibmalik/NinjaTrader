var columnchart;
$(document).ready(function () {
    columnchart = new Highcharts.Chart({
        chart: {
            renderTo: columnContainerVar,
            defaultSeriesType: 'column'
        },
        title: {
            text: columnTitleVar
        },
        xAxis: {
            categories: [
							'Jan',
							'Feb',
							'Mar',
							'Apr',
							'May',
							'Jun',
							'Jul',
							'Aug',
							'Sep',
							'Oct',
							'Nov',
							'Dec'
						]
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Ammount'
            }
        },
        tooltip: {
            formatter: function () {
                return '' +
								this.x + ': €' + this.y ;
            }
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: [%seriesString%]
    });


});