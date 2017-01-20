//Donut Chart Variables
var $border_color_Donut = "#F5F8FA";
var $grid_color_Donut = "#F5F8FA";
var $default_black_Donut = "#666";
var $chartOptions_Donut = {
    series: {
        pie: {
            show: true,
            innerRadius: .0,
            stroke: {
                width: 0,
            }
        }
    },
    shadowSize: 0,
    legend: {
        position: 'se',
        show: true,
        //tickColor: $border_color,
        //borderColor: $grid_color,

    },

    tooltip: false,

    tooltipOpts: {
        content: '%s: %y'
    },

    grid: {
        hoverable: true,
        clickable: false,
        borderWidth: 2,
        //tickColor: $border_color,
        //borderColor: $grid_color,
    },
    shadowSize: 0,
    colors: ['#99cf16', '#1E3084', '#999999', '#CCCCCC'],
};
var $optionsChart = {
    width: 'auto',
    height: 'auto',
    backgroundColor: 'transparent',
    //colors: [$blue, $red, $teal, $green, $orange, $yellow],
    tooltip: {
        textStyle: {
            color: '#666666',
            fontSize: 11
        },
        showColorCode: true
    },
    legend: {
        position: 'bottom left',
        textStyle: {
            color: 'black',
            fontSize: 12
        }
    },
    chartArea: {
        left: 0,
        top: 10,
        width: "100%",
        height: "100%"
    }
};

// Vertical Chart Variables

var $border_color_VC = "#efefef";
var $grid_color_VC = "#ddd";
var $default_black_VC = "#666";
var $green_VC = "#8ecf67";
var $yellow_VC = "#fac567";
var $orange_VC = "#F08C56";
var $blue_VC = "#1e91cf";
var $red_VC = "#f74e4d";
var $teal_VC = "#28D8CA";
var $grey_VC = "#999999";
var $dark_blue_VC = "#0D4F8B";
var $chartOptions_VC = {
    xaxis: {
	        tickSize: [2, "month"],
	        monthNames: ["  " ],
	        tickLength: 0
    },
    
	    grid: {
	        hoverable: true,
	        clickable: false,
	        borderWidth: 1,
	        tickColor: $border_color_VC,
	        borderColor: $grid_color_VC,
	    },
	    bars: {
	        show: true,
	        barWidth: 300,
	        fill: true,
	        lineWidth: 1,
	        order: true,
	        lineWidth: 1,
	        fillColor: { colors: [{ opacity: 1 }, { opacity: 1 }] }
	    },
	    shadowSize: 0,
	    tooltip: true,
	    tooltipOpts: {
	        content: '%s: %y'
	    },
	    colors: [$green_VC, $blue_VC, $yellow_VC, $teal_VC, $yellow_VC, $green_VC],
	}

var barsVisualization;
var data;

// request Ajax
function ajaxCallBack(returV) {
    $listQ = returV;
}

function sendAjaxRequest($Poll) {
    $.ajax({
        type: "POST",
        url: "/Dashboard.aspx/getQuestions",
        data: JSON.stringify({ "idPoll": $Poll }),
        dataType: "json",
        async: false, // <- this turns it into synchronous
        contentType: "application/json; charset=utf-8",
        success: function (result) {

            $("#questions").html("");
            ajaxCallBack(result.d);
        },
        error: function (result) { }
    });
}

$(window).resize(function () {
    //drawQuestionsVisualisation($listQ, "#questions");

});

// contener of data
function getPieContener($titre, $idContenerChart) {

        return "<div class=' col-md-6 col-sm-12 '>" +
                    "<div class='widget'>" +
                        "<div class='widget-header'>" +
                            "<div >" + $titre + "<a id=q" + $idContenerChart + "></a></div>" +
                        "</div>" +
                        "<div class='widget-body'>" +
                            "<div id=" + $idContenerChart + "></div>" +
                        "</div>" +
                    "</div>" +
                "</div>";
}
function getCheckBoxContener($titre, $idContenerChart) {

    return "<div class=' col-md-6 col-sm-12 '>" +
                "<div class='widget'>" +
                    "<div class='widget-header'>" +
                        "<div >" + $titre + "<a id=q" + $idContenerChart + "></a></div>" +
                    "</div>" +
                    "<div class='widget-body'>" +
                        "<div id=" + $idContenerChart + "></div>" +
                    "</div>" +
                "</div>" +
            "</div>";
}
function getWSQColapseContener($ques, $rep){

    var $str = "<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'>" +
               "<a data-toggle='collapse' data-parent='#accordion' href='#collapseTwo'>" + $ques + "</a></h4>"+
               "</div>" + "<div id='collapseTwo' class='panel-collapse collapse'><div class='panel-body'>";
   
       
    for (var j = 0 ; j < $rep.length; j++) {
        if ($rep[j] != null) {
            $str += "<div id=ws"+$rep[j]["idq"]+ "></div>";
        }
    }
    $str += "</div></div></div>";
    return $str;
}
function getWSQContener($ques, $rep) {

    var $str = "<div class='row'>" +
                    "<div class='widget'>" +
                        "<div class='widget-header'>" +
                            "<div >" + $ques + "</div>" +
                        "</div><div class='row'>" ;

    for (var j = 0 ; j < $rep.length; j++) {
        if ($rep[j]["wsrep"].length != 0) {
            for (var kk = 0; kk < $rep[j]["wsrep"].length; kk++) {
                if ($rep[j]["wsrep"] != null) {
                    $str += "<div class='col-md-3 nopadding'><div class='widget-body' id=ws" + $rep[j]["wsrep"][kk]["idevent"] + "></div></div>";
                }
            }
        }
    }
    
           $str += "</div></div></div>";
    return $str;
}

//populate Data
function drawChart(data, idElement) {
    var data = google.visualization.arrayToDataTable(data);
    
    var chart = new google.visualization.PieChart(document.getElementById(idElement));
    chart.draw(data, $optionsChart);
}
function drawDonutChart(data, idElement) {
    var data = google.visualization.arrayToDataTable(data);
    var $optionsDonut = {
       // title: 'Test',
        pieHole: 0.8,
        legend: { position: 'bottom', maxLines: 2 }
    };
    var chart = new google.visualization.PieChart(document.getElementById(idElement));
    chart.draw(data, $optionsDonut);

}
function  drawVerticalChart($donneeVC, $idq){
    
    for (var j = 0 ; j < $donneeVC.length; j++) {
        $donneeVC[j]["data"][1] = parseInt($donneeVC[j]["data"][1]);
    }
    var holder = $('#' + $idq);
    if (holder.length) {
        if ($donneeVC.length == 0) {
            $donneeVC = [{ label: "  ", data: [100, 100] }];
        }
        $.plot(holder, $donneeVC, $chartOptionsV);
    }
}
function drawVerticalvisChart(dataV, idq) {
    var data = new google.visualization.DataTable();
    data.addColumn('string', '');
    data.addColumn('number', 'Score');
    data.addRows(dataV);

    barsVisualization = new google.visualization.ColumnChart(document.getElementById(idq));
    barsVisualization.draw(data, null);

    // Add our over/out handlers.
    google.visualization.events.addListener(barsVisualization, 'onmouseover', barMouseOver);
    google.visualization.events.addListener(barsVisualization, 'onmouseout', barMouseOut);
}

//dessine les contenneur de visualisation
function drawQuestionsContener(data,idElement) {
           
            for (var i = 0 ; i < data.length; i++) {
                if (data[i]["qCategory"] == "General") {
                    if (data[i]["type"] == "RadioButtonList" || data[i]["type"] == "DropDownList") {
                        $(idElement).append(getPieContener(data[i]["ques"], data[i]["idq"]));
                    } else if (data[i]["type"] == "CheckBoxList") {
                        $(idElement).append(getCheckBoxContener(data[i]["ques"], data[i]["idq"]));                 
                    }
                }
                else if (data[i]["qCategory"] == "Workshop") {
                    str = getWSQContener(data[i]["ques"], data[i]["rep"]);
                    $("#accordion").html("");
                    $("#accordion").append(str);
                }
            }
}
function drawQuestionsVisualisation(data, idElement) {

    for (var i = 0 ; i < data.length; i++) {
        if (data[i]["qCategory"] == "General") {
            if (data[i]["type"] == "RadioButtonList" || data[i]["type"] == "DropDownList") {
                 drawChart(data[i]["rep"], data[i]["idq"]);
            } else if (data[i]["type"] == "CheckBoxList") {
                drawVerticalvisChart(data[i]["rep"], data[i]["idq"]);
            }
        }
        else if (data[i]["qCategory"] == "Workshop") {
            for (var k = 0; k < data[i]["rep"].length; k++) {   
                if (data[i]["rep"][k]["wsrep"].length != 0) {      
                    for (var kk = 0; kk < data[i]["rep"][k]["wsrep"].length; kk++) {
                        drawDonutChart(data[i]["rep"][k]["wsrep"][kk]["rep"], "ws" + data[i]["rep"][k]["wsrep"][kk]["idevent"]);
                    }
                }
            }
        }
    }
}

//bareMouseHover
/*function drawMouseoverVisualization(dataV) {
    var data = new google.visualization.DataTable();
    data.addColumn('string', '');
    data.addColumn('number', 'Score');
    data.addRows([
      ['Satisfait', 3.6],
      ['pluto Satisfait', 4.1],
      ['normal', 3.8],
      ['je sais pas', 4.6]
    ]);

    barsVisualization = new google.visualization.ColumnChart(document.getElementById('vertical-chart'));
    barsVisualization.draw(data, null);

    // Add our over/out handlers.
    google.visualization.events.addListener(barsVisualization, 'onmouseover', barMouseOver);
    google.visualization.events.addListener(barsVisualization, 'onmouseout', barMouseOut);
}
function barMouseOver(e) {
    //barsVisualization.setSelection([e]);
}
function barMouseOut(e) {
    //barsVisualization.setSelection([{ 'row': null, 'column': null }]);
}
*/


//function drawQuestionContener(idPoll) {
//    $.ajax({
//        type: "POST",
//        url: "/Dashboard.aspx/getQuestions",
//        data: JSON.stringify({ "idPoll": idPoll }),
//        dataType: "json",
//        contentType: "application/json; charset=utf-8",
//        success: function (result) {
//            $("#questions").html("");
//            data = result.d;
//            for (var i = 0 ; i < data.length; i++) {
//                if (data[i]["qCategory"] == "General") {
//                    if (data[i]["type"] == "RadioButtonList" || data[i]["type"] == "DropDownList") {
//                        $("#questions").append(getPieContener(data[i]["ques"], data[i]["idq"]));
//                        drawChart(data[i]["rep"], data[i]["idq"]);
//                    } else if (data[i]["type"] == "CheckBoxListe") {
//                        $("#questions").append(getCheckBoxContener(data[i]["ques"], data[i]["idq"]));
//                        //drawVerticalChart(data[i]["rep"], data[i]["idq"]);
//                        // fin CheckBoxList
//                    }
//                }
//                else if (data[i]["qCategory"] == "Workshop") {
//                    str = getWSQContener(data[i]["ques"], data[i]["rep"]);
//                    $("#accordion").html("");
//                    $("#accordion").append(str);
//                }
//            }
//        },
//        error: function (result) { }
//    });
//}
