var linechart;
$(document).ready(function () {
    linechart = new Highcharts.Chart({
        chart: {
            renderTo: lineContainerVar,
            defaultSeriesType: 'line',
            marginRight: 130,
            marginBottom: 25
        },
        title: {
            text: lineTitleVar,
            x: -20 //center
        },
        xAxis: {
            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
            'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
        },
        yAxis: {
            title: {
                text: 'Ammount'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.series.name + '</b><br/>' +
               this.x + ':  € ' + this.y;
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 100,
            borderWidth: 0
        },
        series: [%seriesString%]
    });


});