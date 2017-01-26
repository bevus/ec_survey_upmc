<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard_B1AB75ED32D74914B015B05C2EB3081E.aspx.cs" Inherits="Dashboard_B1AB75ED32D74914B015B05C2EB3081E.Dashboard_B1AB75ED32D74914B015B05C2EB3081E" %>
                <!DOCTYPE html>
                <html xmlns="http://www.w3.org/1999/xhtml" >
                <head runat="server" >
                    <meta charset="UTF-8" />
                    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                    <link href='css/bootstrap.min.css'  rel='stylesheet' />
                    <link href='css/new.css'  rel='stylesheet' />
                    <link href='css/nav.css'  rel='stylesheet' />
                    <title> Dashboard - Template Survey</title>
                    <style>
                        body{
                            margin-top : 25px;      
                        }
                    </style>
                </head><body class='jumbotron'>
                                <div id = 'dashboard-container' class='dashboard-container'>
                                    <nav class='navbar  navbar-fixed-top'>
               <div class="container-fluid" id="navdiv">
                    <div class="navbar-header">
                        <button type = 'button' class='navbar-toggle' data-toggle='collapse' data-target='#myNavbar'>
                            <span class='icon-bar'></span>
                            <span class='icon-bar'></span>
                            <span class='icon-bar'></span>
                        </button>
                    </div>
                    <div class='collapse navbar-collapse' id='myNavbar'>
                        <ul class='nav navbar-nav navbar-left'>
                            <li>
                                <div class='btn-group' id='btn-group'>
                                    <form id="form" runat="server">
                                        <button type = 'button' class='btn btn-primary dropdown-toggle' data-toggle='dropdown'>
                                            Template Survey 
                                       </button>
                                        <button id = 'refresh' type='button' class='btn btn-primary'>Refresh</button>
                                        
                                        <asp:Button ID = "button1" runat="server" class='btn btn-primary' OnClick="ExtractDataStatistics" Text="Statistics" />
                                        <asp:Button ID = "button2" runat="server" class='btn btn-primary' OnClick="ExtractRawData" Text="Raw data" />
                                        <button id = 'toPdf' type='button' class='btn btn-primary'>Pdf</button>
                                    </form>                             
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav> 
                                    <div class="container-fluid" id="dashboard-content">
                    <div class="container" id="dashboard-title">
                        <div class="row">
                            <div class="col-md-12">
                                <p class="well">Template Survey version of <span id="current-date"></span></p>
                                <hr/>
                            </div>
                        </div>
                    </div>
                   <!-- contenue de Question -->
                   <div class="container">
                       <div class="row" id="questions">
                       </div>
                   </div>
                   <!-- Row Start workshop Questions -->
                   <div class="container">
                       <div class="col-lg-12 col-md-12" id="workshop">
                       </div>
                   </div>
                </div> 
                                </div>
                                 
<script src = "js/jquery-3.1.1.min.js"></script>
<script src = "js/bootstrap.min.js" ></script> 
<script src = "js/google-api.js" ></script>      
<script src = "js/jspdf.min.js" ></script>     
<script src = "js/rgbcolor.js" ></script>      
<script src = "js/StackBlur.js" ></script>       
<script src = "js/html2canvas.svg.min.js" ></script>        
<script src = "js/canvg.min.js" ></script>
          
<script src = "js/custom.min.js" ></script>      
<script src = "js/pdfGenerator.min.js" ></script>
<script type ="text/javascript" >
    google.load('visualization', '1', { packages: ['corechart'] });
    var $listQ; var $idPoll =6 ;
    $(function() {
        sendAjaxRequest($idPoll,"Dashboard_B1AB75ED32D74914B015B05C2EB3081E.aspx/getQuestions");
        sendDataAtelierQuestions($idPoll, 'workshop', "Dashboard_B1AB75ED32D74914B015B05C2EB3081E.aspx/getAtelierQuestions");
        $("#current-date").text(new Date());
    });
</script>
                              </body></html>