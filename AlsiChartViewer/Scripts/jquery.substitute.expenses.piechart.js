var piechart;
$(document).ready(function () {
    piechart = new Highcharts.Chart({
        chart: {
            renderTo: pieContainerVar,
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: pieTitleVar
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.point.name + '</b>: €' + this.y + ' ';
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    color: '#000000',
                    connectorColor: '#000000',
                    formatter: function () {
                        return '<b>' + this.point.name + '</b>: €' + this.y + ' ';
                    }
                }
            }
        },
        series: [{
            type: 'pie',
            name: 'Browser share',
            data: [%seriesString%]
        }]
    });
});