google.charts.load("current", { packages: ["corechart"] });
google.charts.setOnLoadCallback(drawChart);

function drawChart() {
    var data = google.visualization.arrayToDataTable([
        ['Dinosaur', 'Length'],
        ['Acrocanthosaurus (top-spined lizard)', 12.2],
        ['Albertosaurus (Alberta lizard)', 0.5]]);

    var options = {
        title: 'Lengths of dinosaurs, in meters',
        legend: { position: 'none' },
    };

    var chart = new google.visualization.Histogram(document.getElementById('chart_div'));
    chart.draw(data, options);
}