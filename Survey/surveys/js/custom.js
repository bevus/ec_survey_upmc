﻿var barsVisualization;
var data;
var $respQ = new Object();
var $repAtelier = new Object();

$('#refresh').click(function () {
    location.reload();
});

function ajaxCallBack(returV) {
    $listQ = returV;
    drawQuestionsContener($listQ, '#questions');
    drawQuestionsVisualisation($listQ, '#questions');
}
function sendAjaxRequest($Poll, $methodCall) {
    $.ajax({
        type: "POST",
        url: $methodCall,
        data: JSON.stringify({ "idPoll": $Poll }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $("#questions").html("");
            ajaxCallBack(result.d);
        },
        error: function (result) { }
    });
}


function sendDataQAjaxRequest($idQuestion, $methodCall) {
    $.ajax({
        type: "POST",
        url: $methodCall,
        data: JSON.stringify({ "idQuestion": parseInt($idQuestion.replace(/[^0-9\.]/g, ''), 10) }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $respQ[$idQuestion] = result.d;

            if (result.d["ctr"] === "RadioButtonList") {
                drawChart(result.d["rep"], $idQuestion);
            } else if (result.d["ctr"] === "DropDownList") {
                drawChart(result.d["rep"], $idQuestion);
            } else if (result.d["ctr"] === "CheckBoxList") {
                drawVerticalvisChart(result.d["rep"], $idQuestion);
            }
        },
        error: function (result) { }
    });
}

function sendDataAtelierQuestions($idPoll, $idAtelierContener, $methodCall) {
    $.ajax({
        type: "POST",
        url: $methodCall,
        data: JSON.stringify({ "idPoll": $idPoll }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $("#" + $idAtelierContener + "").html("");
            $repAtelier = result.d;
            drawAtelierContener(result.d, $idAtelierContener);
        },
        error: function (result) { }
    });
}

$(window).resize(function () {
    if ($listQ !== null) {
         drawQuestionsVisualisation($listQ, '#questions');
    }
    if ($repAtelier !== null) {
        drawQuestionsVisualisation($repAtelier, '#questions');
    }
});

/*dessine les contenneur de visualisation*/
function drawQuestionsContener(data, idElement) {

    for (var i = 0 ; i < data.length; i++) {
        if (data[i]["qCategory"] === "General") {
            if (data[i]["type"] === "RadioButtonList" || data[i]["type"] === "DropDownList") {
                $(idElement).append(getPieContener(data[i]["ques"], data[i]["idq"]));
                drawChart(data[i]["rep"], data[i]["idq"]);
            } else if (data[i]["type"] === "CheckBoxList") {
                $(idElement).append(getCheckBoxContener(data[i]["ques"], data[i]["idq"]));
                drawVerticalvisChart(data[i]["rep"], data[i]["idq"]);
            }
        } else if (data[i]["qCategory"] == "Workshop") {
            str = getWSQContener(data[i]["ques"], data[i]["idq"], data[i]["rep"]);
            $("#workshop").append(str);
        } else if (data[i]["qCategory"] == "Activity") {
            str = getSSQContener(data[i]["ques"], data[i]["idq"], data[i]["rep"]);
            $("#workshop").append(str);

        } else if (data[i]["qCategory"] === "Meeting") {
            var dataMet = data[i]["rep"];
            for (var j = 0; j < dataMet.length; j++) {
                if (data[i]["rep"][j]["wsrep"] !== null) {
                    if (dataMet[j]["contrl"] === "RadioButtonList" || dataMet[j]["contrl"] === "DropDownList") {
                        $(idElement).append(getPieContener(dataMet[j]["sq"], "m" + dataMet[j]["sqid"]));
                    } else if (data[i]["type"] === "CheckBoxList") {
                        $(idElement).append(getCheckBoxContener(dataMet[j]["sq"], "m" + dataMet[j]["sqid"]));
                    }
                }
            }
            for (var k = 0; k < data[i]["rep"].length; k++) {
                if (data[i]["rep"][k]["contrl"] === "RadioButtonList" || data[i]["rep"][k]["contrl"] === "DropDownList") {
                    if (data[i]["rep"][k]["wsrep"] !== null) {
                        console.log("m" + data[i]["rep"][k]["sqid"]);
                        drawChart(data[i]["rep"][k]["wsrep"]["rep"], "m" + data[i]["rep"][k]["sqid"]);
                    }
                }
            }
        }
    }
}

function drawAtelierContener(data, idElement) {
    for (var i = 0 ; i < data.length; i++) {
        if (data[i]["qCategory"] === "Workshop") {
            for (var k = 0; k < data[i]["rep"].length; k++) {
                
               
                if (data[i]["rep"][k]["wsrep"].length !== 0) {
                    for (var kk = 0; kk < data[i]["rep"][k]["wsrep"].length; kk++) {
                        str = getWSQContener(data[i]["rep"][k]["sq"], data[i]["rep"][k]["sqid"], data[i]["rep"]);
                        $("#" + idElement + "").append(str);
                        if (data[i]["rep"][k]["wsrep"].length !== 0) {
                            for (kk = 0; kk < data[i]["rep"][k]["wsrep"].length; kk++) {
                                if (data[i]["rep"][k]["sqid"] === data[i]["rep"][k]["wsrep"][kk]["idq"]) {
                                    drawDonutChart(data[i]["rep"][k]["wsrep"][kk]["rep"],
                                        "ws" + data[i]["rep"][k]["wsrep"][kk]["idevent"] + data[i]["rep"][k]["sqid"],
                                        data[i]["rep"][k]["wsrep"][kk]["theme"]);
                                }
                            }
                        }
                    }
                }
            }
        } else if (data[i]["qCategory"] === "Activity") {
            for (k = 0; k < data[i]["rep"].length; k++) {
                str = getSSQContener(data[i]["rep"][k]["sq"], data[i]["rep"][k]["sqid"], data[i]["rep"]);
                $("#" + idElement + "").append(str);
                if (data[i]["rep"][k]["wsrep"].length !== 0) {
                    for (kk = 0; kk < data[i]["rep"][k]["wsrep"].length; kk++) {    
                        if (data[i]["rep"][k]["sqid"] === data[i]["rep"][k]["wsrep"][kk]["idq"]) {
                            drawDonutChart(data[i]["rep"][k]["wsrep"][kk]["rep"],
                                "ss" + data[i]["rep"][k]["wsrep"][kk]["idevent"] + data[i]["rep"][k]["sqid"],
                                data[i]["rep"][k]["wsrep"][kk]["theme"]);
                        }
                    }
                }
            }

        }
    }

}

function drawQuestionsVisualisation(data, idElement) {
    for (var i = 0 ; i < data.length; i++) {
        if (data[i]["qCategory"] === "General") {
            if (data[i]["type"] === "RadioButtonList" || data[i]["type"] === "DropDownList") {
                drawChart(data[i]["rep"], data[i]["idq"]);
            } else if (data[i]["type"] === "CheckBoxList") {
                drawVerticalvisChart(data[i]["rep"], data[i]["idq"]);
            }
        }
        else if (data[i]["qCategory"] === "Workshop") {
            if (data[i]["type"] === "RadioButtonList" || data[i]["type"] === "DropDownList") {
                for (k = 0; k < data[i]["rep"].length; k++) {
                    if (data[i]["rep"][k]["wsrep"].length !== 0) {
                        for (kk = 0; kk < data[i]["rep"][k]["wsrep"].length; kk++) {
                            drawDonutChart(data[i]["rep"][k]["wsrep"][kk]["rep"],
                                "ws" + data[i]["rep"][k]["wsrep"][kk]["idevent"] + data[i]["rep"][k]["sqid"],
                                data[i]["rep"][k]["wsrep"][kk]["theme"]);
                        }
                    }
                }
            } else if (data[i]["type"] === "CheckBoxList") {
                for (k = 0; k < data[i]["rep"].length; k++) {
                    if (data[i]["rep"][k]["wsrep"].length !== 0) {
                        for (kk = 0; kk < data[i]["rep"][k]["wsrep"].length; kk++) {
                            drawVerticalvisChart(data[i]["rep"][k]["wsrep"][kk]["rep"],
                                "ws" + data[i]["rep"][k]["wsrep"][kk]["idevent"] + data[i]["rep"][k]["sqid"]);
                        }
                    }
                }   
            }
        } else if (data[i]["qCategory"] === "Activity") {
            if (data[i]["type"] === "RadioButtonList" || data[i]["type"] === "DropDownList") {
                for (k = 0; k < data[i]["rep"].length; k++) {
                    if (data[i]["rep"][k]["wsrep"].length !== 0) {
                        for (kk = 0; kk < data[i]["rep"][k]["wsrep"].length; kk++) {
                            drawDonutChart(data[i]["rep"][k]["wsrep"][kk]["rep"],
                                "ss" + data[i]["rep"][k]["wsrep"][kk]["idevent"] + data[i]["rep"][k]["sqid"],
                                data[i]["rep"][k]["wsrep"][kk]["theme"]);
                        }
                    }
                }
            }else if (data[i]["type"] === "CheckBoxList") {
                for (k = 0; k < data[i]["rep"].length; k++) {
                    if (data[i]["rep"][k]["wsrep"].length !== 0) {
                        for (kk = 0; kk < data[i]["rep"][k]["wsrep"].length; kk++) {
                            drawVerticalvisChart(data[i]["rep"][k]["wsrep"][kk]["rep"],
                                "ss" + data[i]["rep"][k]["wsrep"][kk]["idevent"] + data[i]["rep"][k]["sqid"]);
                        }
                    }
                }
            }
        } else if (data[i]["qCategory"] === "Meeting") {
            for (k = 0; k < data[i]["rep"].length; k++) {
                if (data[i]["rep"][k]["contrl"] === "RadioButtonList" || data[i]["rep"][k]["contrl"] === "DropDownList") {
                    if (data[i]["rep"][k]["wsrep"] !== null) {
                        drawChart(data[i]["rep"][k]["wsrep"]["rep"], "m" + data[i]["rep"][k]["sqid"]);
                    }
                }
            }
        }
    }
}


/* contener of data*/
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
function getWSQColapseContener($ques, $rep) {

    var $str = "<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'>" +
               "<a data-toggle='collapse' data-parent='#accordion' href='#collapseTwo'>" + $ques + "</a></h4>" +
               "</div>" + "<div id='collapseTwo' class='panel-collapse collapse'><div class='panel-body'>";


    for (var j = 0 ; j < $rep.length; j++) {
        if ($rep[j] !== null) {
            $str += "<div id=ws" + $rep[j]["idq"] + "></div>";
        }
    }
    $str += "</div></div></div>";
    return $str;
}
function getWSQContener($ques,$idq, $rep) {

    var $str = "<div class='row'>" +
                    "<div class='widget'>" +
                        "<div class='widget-header'>" +
                            "<div >" + $ques + "</div>" +
                        "</div><div class='row'>";

    for (j = 0 ; j < $rep.length; j++) {
        if ($rep[j]["wsrep"].length !== 0) {
            for (var kk = 0; kk < $rep[j]["wsrep"].length; kk++) {
                if ($rep[j]["wsrep"] !== null && $idq === $rep[j]["sqid"]) {
                    $str += "<div class='col-md-3 nopadding'><div class='widget-body' id=ws" + $rep[j]["wsrep"][kk]["idevent"] + $rep[j]["sqid"] + "></div></div>";
                }
            }
        }
    }

    $str += "</div></div></div>";
    return $str;
}
function getSSQContener($ques,$idq, $rep) {

    var $str = "<div class='row'>" +
                    "<div class='widget'>" +
                        "<div class='widget-header'>" +
                            "<div >" + $ques + "</div>" +
                        "</div><div class='row' >";

    for (var j = 0 ; j < $rep.length; j++) {
        if ($rep[j]["wsrep"].length !== 0) {
            for (var kk = 0; kk < $rep[j]["wsrep"].length; kk++) {
                if ($rep[j]["wsrep"] !== null && $idq === $rep[j]["sqid"]) {
                    $str += "<div class='col-md-3 nopadding'><div class='widget-body' id=ss" + $rep[j]["wsrep"][kk]["idevent"] + $rep[j]["sqid"] + "></div></div>";
                    console.log($rep[j]["wsrep"][kk]["idevent"] + $rep[j]["sqid"]);
                }
            }
        }
    }

    $str += "</div></div></div>";
    return $str;
}
/*populate Data*/
function drawChart(data, idElement) {
    data = google.visualization.arrayToDataTable(data);

    chart = new google.visualization.PieChart(document.getElementById(idElement));
    chart.draw(data, null);
}
function drawDonutChart(data, idElement, title) {
    data = google.visualization.arrayToDataTable(data);
    $optionsDonut = {
        title: title,
        pieHole: 0.8,
        legend: { position: 'bottom', maxLines: 2 }
    };
    chart = new google.visualization.PieChart(document.getElementById(idElement));
    chart.draw(data, $optionsDonut);

}
function drawVerticalvisChart(dataV, idq) {
    var data = new google.visualization.DataTable();
    data.addColumn('string', '');
    data.addColumn('number', '');
    data.addRows(dataV);

    barsVisualization = new google.visualization.ColumnChart(document.getElementById(idq));
    barsVisualization.draw(data, null);
}
