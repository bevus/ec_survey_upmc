<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard_DFF183EFD77E46868537F45923EAD693.aspx.cs" Inherits="Dashboard_DFF183EFD77E46868537F45923EAD693.Dashboard_DFF183EFD77E46868537F45923EAD693" %>
                <!DOCTYPE html>
                <html xmlns="http://www.w3.org/1999/xhtml" >
                <head runat="server" >
                    <meta charset="UTF-8" />
                    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                    <link href='css/bootstrap.css'  rel='stylesheet' />
                    <link href='css/new.css'  rel='stylesheet' />
                    <link href='css/nav.css'  rel='stylesheet' />
                    <title> Dashboard </title>
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
                                    <button type = 'button' class='btn btn-primary dropdown-toggle' data-toggle='dropdown'>
                                        Simple Survey 
                                       
                                   </button>
                                    <button type = 'button' class='btn btn-primary'>Excel</button>
                                    <button id = 'toPdf' type='button' class='btn btn-primary'>Pdf</button>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav> 
                                    <div class="container-fluid" id="dashboard-content">
                   <!-- contenue de Question -->
                   <div class="container">
                       <div class="row" id="questions">
                        <div><h3>Général Question's</h3></div>
                       </div>
                   </div>
                   <!-- Row Start workshop Questions -->
                   <div class="container">
                       <div class="col-lg-12 col-md-12" id="accordion">
                       </div>
                   </div>
                </div> 
                                </div>
                                 <!--Google Visualization JS --> 
<script src ="js/google-api.js" ></script>
<script src ="js/jquery.js" ></script>
<script src ="js/bootstrap.min.js" ></script>

<script src ="js/customJs/custom.js" ></script>
<script src ="js/pdf/mypdf.js"></script>

<script src ="js/pdf/jspdf.min.js" ></script>
<script src ="js/pdf/html2canvas.min.js" ></script>
<script src ="js/pdf/html2canvas.svg.js" ></script>
<script type ="text/javascript" >
    google.load('visualization', '1', { packages: ['corechart'] });
    var $listQ; var $idPoll =1 ;
    $(function() {
            sendAjaxRequest($idPoll,"DFF183EFD77E46868537F45923EAD693");
    });
</script>
                              </body></html>