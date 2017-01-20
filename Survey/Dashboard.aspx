<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="survey.Dashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="Content/bootstrap.css" runat="server" rel="stylesheet" />
    <link href="Scripts/bluemoon/css/new.css" runat="server" rel="stylesheet" />  
    <link href="Content/nav.css" runat="server" rel="stylesheet" />
    <title>Dashboard</title>
</head>

<body class="jumbotron">
    <div id="dashboard-container" class="dashboard-container">

        <nav class="navbar  navbar-fixed-top">
            <div class="container-fluid">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbar">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                </div>
                <div class="collapse navbar-collapse" id="myNavbar">
                    <ul class="nav navbar-nav navbar-left">
                        <li>
                            <div class="btn-group" id="btn-group">
                                <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown">
                                    List Surveys 
                                    <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu" role="menu">
                                    <li><a href="#">Survey 1</a></li>
                                    <li><a href="#">Survey 2</a></li>
                                    <li><a href="#">Survey 3</a></li>
                                </ul>
                                <button type="button" class="btn btn-primary">Excel</button>
                                <button type="button" class="btn btn-primary">Csv</button>
                                <button id="toPdf" type="button" class="btn btn-primary">Pdf</button>
                            </div>
                        </li>
                    </ul>

                    <ul class="nav navbar-nav navbar-right">
                        <li><a href="#"></li>
                    </ul>
                </div>
            </div>
        </nav>

        <!-- contenue du Dashboard -->
        <div class="container" id="dashboard-content">
            <div class="col-md-12 col-sm-12"><h3> Général Question's </h3> </div>
            <!-- contenue de Question -->      
            <div class="container">
                <div class="row" id="questions">
                </div>
            </div>

            <!-- Row Start Faq's Questions -->
            <div class="container">
                <div class="col-lg-12 col-md-12" id="accordion">
                </div>
            </div>

             <!-- contenue de test --> 
            <div class="container">
                <div class="well">
                    <!-- Row Start -->
                    <div class="row gutter">
                        <div class="col-md-6 col-md-6">
                            <div id="widget" class="widget">
                                <div class="widget-header">
                                    <div class="title">
                                        Temps de Réponses
                                    </div>
                                    <span class="tools">
                                        <i class="fa fa-cogs"></i>
                                    </span>
                                </div>
                                <div class="widget-body">
                                    <div id="flot-placeholder" class="chart-height"></div>
                                </div>
                            </div>
                        </div>

                        <!-- vertical Chart -->
                        <div class="row gutter">
                            <div class="col-md-6 col-md-6">
                                <div class="widget">
                                    <div class="widget-header">
                                        <div class="title">
                                            Vertical Chart
                                        </div>
                                    </div>
                                    <div class="widget-body">
                                        <div id="vertical-chart" class="chart-height"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- en vertical Chart -->
                    </div>
                    <!-- Row End -->

                </div>
            </div>

        </div>
    </div>
    <!-- Row End -->

    <!-- Google Visualization JS -->
    <script src="Scripts/bluemoon/js/google-api.js"></script>
    <script src="Scripts/bluemoon/js/jquery.js"></script>
    <script src="Scripts/bluemoon/js/bootstrap.min.js"></script>
     <!-- Custom JS -->
    <script src="Scripts/customJs/custom.js"></script>
    <script src="Scripts/customJs/customPdf.js"></script>
    
    <script type="text/javascript" src="Scripts/pdf/jspdf.min.js"></script>
    <script type="text/javascript" src="Scripts/pdf/html2canvas.min.js"></script>
    <script type="text/javascript" src="Scripts/pdf/html2canvas.svg.js"></script>

    <script type="text/javascript">

        //Google Visualization 
        google.load('visualization', '1', { packages: ['corechart'] });

        var $datadonut = [
          ['Task', 'Hours per Day'],
          ['Work', 11],
          ['Eat', 2],
          ['Commute', 2],
          ['Watch TV', 2],
          ['Sleep', 7]
        ];
        var $listQ;
        var $idPoll = 1;
       
        $(function () {

            drawDonutChart($datadonut, 'vertical-chart');
      
            sendAjaxRequest($idPoll);

            drawQuestionsContener($listQ, "#questions");
            drawQuestionsVisualisation($listQ, "#questions");

           
        });



    </script>
</body>
</html>
