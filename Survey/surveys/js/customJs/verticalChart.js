	var $border_color = "#efefef";
	var $grid_color = "#ddd";
	var $default_black = "#666";
	var $green = "#8ecf67";
	var $yellow = "#fac567";
	var $orange = "#F08C56";
	var $blue = "#1e91cf";
	var $red = "#f74e4d";
	var $teal = "#28D8CA";
	var $grey = "#999999";
	var $dark_blue = "#0D4F8B";


	var data, $chartOptionsv;

	data = [{
		label: 'Windows',
		data: [[1325376000000, 800]]
	}, {
		label: 'Android',
		data: [[1325376000000, 200]]
	}, {
		label: 'Apple',
		data: [[1325376000000, 1650]]
	}];

	$chartOptionsV = {
	    xaxis: {
	      
			tickSize: [2, "month"],
			monthNames: ["Jan"],
			tickLength: 0
		},
		grid:{
        hoverable: true,
        clickable: false,
        borderWidth: 1,
		tickColor: $border_color,
        borderColor: $grid_color,
        },
	    bars: {
			show: true,
			barWidth: 300,
			fill: true,
			lineWidth: 1,
			order: true,
			lineWidth: 1,
			fillColor: { colors: [ { opacity: 1 }, { opacity: 1 } ] }
	    },
	    shadowSize: 0,
	    tooltip: true,
	    tooltipOpts: {
		    content: '%s: %y'
	    },
	    colors: [$green, $blue, $yellow, $teal, $yellow, $green],
	}
	$(function () {
	var holder = $('#vertical-chart');

	if (holder.length) {
		$.plot(holder, data, $chartOptionsV );
	}
});