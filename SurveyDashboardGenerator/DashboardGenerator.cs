using SurveyModel;
using System;
using System.Text;

namespace SurveyDashboardGenerator
{
    public class DashboardGenerator
    {
        public string Directory { get; set; }
        public Poll Poll { get; set; }
        public String Dashbord_name { get; set; }
        public Boolean AllowDataExtraction { get; set; }
        public DashboardUtils utils = new DashboardUtils();

        public DashboardGenerator(string directory, Poll poll,String dashboard_name,Boolean allowDataExtraction )
        {
            if (poll.Equals(null))
            {
                throw new Exception("Poll = null ");
            }
            this.Directory = directory;
            this.Poll = poll;
            this.Dashbord_name = dashboard_name;
            this.AllowDataExtraction = allowDataExtraction;
        }
      
        public string GenerateDashboard()
        {
            return GenerateDashboard_V1();
        }
        // version 1
        public string GenerateDashboard_V1()
        {
            if (Poll.Equals(null)) { throw new Exception("Poll = null "); }
            var utils = new DashboardUtils();

            var aspxFileName = Dashbord_name + ".aspx";
            var aspxcsFileName = Dashbord_name + ".aspx.cs";
            var aspxdesinercsFileName = Dashbord_name + ".aspx.designer.cs";

            String aspxCode = GetHeader(aspxcsFileName) +
                              $@"<body class='jumbotron'>
                                <div id = 'dashboard-container' class='dashboard-container'>
                                    {GetNavHeader(Poll.Name)} 
                                    {GetDashbordContener(Poll.Id)} 
                                </div>
                                 {AddJScript(Poll.Id)}
                              </body></html>";

            String aspxdesignercsCode = GetDesignerCSCode(aspxcsFileName);
            String aspxcsCode = GetCSCode(aspxcsFileName);

            //write file
            System.IO.File.WriteAllText(Directory + aspxcsFileName, aspxcsCode, Encoding.UTF8);
            System.IO.File.WriteAllText(Directory + aspxdesinercsFileName, aspxdesignercsCode, Encoding.UTF8);
            System.IO.File.WriteAllText(Directory + aspxFileName, aspxCode.ToString(), Encoding.UTF8);
            //GenerateProcedureStocke();
            return aspxFileName; ;
        }
        public String GetDashbordContener(int poll_id)
        {
            return
                $@"<div class=""container-fluid"" id=""dashboard-content"">
                   <!-- contenue de Question -->
                   <div class=""container"">
                       <div class=""row"" id=""questions"">
                        <div><h3>Général Question's</h3></div>
                       </div>
                   </div>
                   <!-- Row Start workshop Questions -->
                   <div class=""container"">
                       <div class=""col-lg-12 col-md-12"" id=""accordion"">
                       </div>
                   </div>
                </div>";
        }
        public String AddJScript(int idPoll)
        {
            return
$@"<!--Google Visualization JS --> 
<script src =""js/google-api.js"" ></script>
<script src =""js/jquery.js"" ></script>
<script src =""js/bootstrap.min.js"" ></script>

<script src =""js/customJs/custom.js"" ></script>
<script src =""js/pdf/mypdf.js""></script>

<script src =""js/pdf/jspdf.min.js"" ></script>
<script src =""js/pdf/html2canvas.min.js"" ></script>
<script src =""js/pdf/html2canvas.svg.js"" ></script>
<script type =""text/javascript"" >
    google.load('visualization', '1', {{ packages: ['corechart'] }});
    var $listQ; var $idPoll ={ idPoll } ;
    $(function() {{
            sendAjaxRequest($idPoll,""{Poll.ExternalId}"");
    }});
</script>";
        }


        // version 0
        public string GenerateDashboard_V0()
        {
            if (Poll.Equals(null)) { throw new Exception("Poll = null "); }
            var utils = new DashboardUtils();

            var aspxFileName = "Dashboard_" + Poll.ExternalId + ".aspx";
            var aspxcsFileName = "Dashboard_" + Poll.ExternalId + ".aspx.cs";
            var aspxdesinercsFileName = "Dashboard_" + Poll.ExternalId + ".aspx.designer.cs";

            String aspxCode = GetHeader(aspxcsFileName) +
                              $@"<body class='jumbotron'>
                                <div id = 'dashboard-container' class='dashboard-container'>
                                    {GetNavHeader(Poll.Name)} 
                                    {GetDashbordContener_V0(Poll.Id)} 
                                </div>
                                 {AddJScript_V0(Poll.Id)}
                              </body></html>";

            String aspxdesignercsCode = GetDesignerCSCode(aspxcsFileName);
            String aspxcsCode = GetCSCode(aspxcsFileName);

            //write file
            System.IO.File.WriteAllText(Directory + aspxcsFileName, aspxcsCode, Encoding.UTF8);
            System.IO.File.WriteAllText(Directory + aspxdesinercsFileName, aspxdesignercsCode, Encoding.UTF8);
            System.IO.File.WriteAllText(Directory + aspxFileName, aspxCode.ToString(), Encoding.UTF8);
            //GenerateProcedureStocke();
            return Directory + aspxcsFileName; ;
        }
        public String GetDashbordContener_V0(int poll_id)
        {
            return
                $@"<div class=""container-fluid"" id=""dashboard-content"">
                       {utils.GetGeneralQuestionContener(poll_id)}
                </div>";
        }
        public String AddJScript_V0(int idPoll)
        {
            return
$@"<!--Google Visualization JS --> 
<script src =""js/google-api.js"" ></script>
<script src =""js/jquery.js"" ></script>
<script src =""js/bootstrap.min.js"" ></script>

<script src =""js/customJs/custom.js"" ></script>
<script src =""js/customJs/customPdf.js""></script>

<script src =""js/pdf/jspdf.min.js"" ></script>
<script src =""js/pdf/html2canvas.min.js"" ></script>
<script src =""js/pdf/html2canvas.svg.js"" ></script>
<script type =""text/javascript"" >
    google.load('visualization', '1', {{ packages: ['corechart'] }});
    var $listQ; var $idPoll ={ idPoll } ;
    $(function() {{
            $(""#questions"").find(""div .gnrlques"").each(function () {{
                        sendDataQAjaxRequest($(this).attr('id'),""{Poll.ExternalId}""); 
                }});
        }});
</script>";
        }


        //other methodes
        public String GetCSCode(String aspxcsFileName)
        {
            return
$@"using System;
using System.Collections.Generic;
using System.Web.Services;
using DataAccess;
using SurveyModel;
using SurveyDashboardGenerator;
using System.Web.Script.Services;
namespace {aspxcsFileName.Replace(".aspx.cs", "")}
{{
    public partial class {aspxcsFileName.Replace(".aspx.cs", "")}  : System.Web.UI.Page
    {{
        protected static Manager manager = new Manager();
        protected static DashboardUtils u = new DashboardUtils();
        protected static int id_poll = {Poll.Id} ;
        protected void Page_Load(object sender, EventArgs e)
        {{
            var manager = new Manager();
        }}

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static object GetGnrlQuestionData(int idQuestion)
        {{
            var poll = manager.getPoll(id_poll);
            var questionData = new object();
            foreach (Question q in poll.Questions)
            {{
                if (idQuestion.Equals(q.Id))
                {{
                    questionData = u.GetQesDataResponse(q, poll.TableName);
                    break;
                }}
            }}
            return questionData;
        }}

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static List<object> getQuestions(int idPoll)
        {{
            DashboardUtils u = new DashboardUtils();
            var question = new List<object>();
            var poll = manager.getPoll(idPoll);
                
            foreach (Question q in poll.Questions)
            {{
            switch (q.Category.ToString())
            {{
                case ""General"":
                    var repGnrl = u.GetGeneralResponse(q, poll.TableName);
                    if (repGnrl != null) question.Add(repGnrl);
                    break;
                case ""Activity"":
                    var repSession = u.GetSessionsubQuestion(q, poll.TableSessionName);
                    if (repSession != null) question.Add(repSession);
                    break;
                case ""Meeting"":
                    var repMeet = u.GetMeetingsubQuestion(q,poll.TableMeetingName);
                    if (repMeet != null) question.Add(repMeet);
                    break;
                case ""Workshop"":
                    var repWS = u.GetWSsubQuestion(q, poll.TableWsName);
                    if (repWS != null) question.Add(repWS);
                    break;
                default:
                    break;
            }}
            }}
            return question;
        }}
    }}
}}";

        }
        public String GetDesignerCSCode(String aspxcsFileName)
        {
            return
$@"namespace {aspxcsFileName.Replace(".aspx.cs", "") } {{
        public partial class {aspxcsFileName.Replace(".aspx.cs", "")} {{

        }}
}}";

        }
        public String GetHeader(String aspxcsFileName)
        {
            return
                $@"<%@ Page Language=""C#"" AutoEventWireup=""true"" CodeFile=""{aspxcsFileName}"" Inherits=""{aspxcsFileName.Replace(".aspx.cs", "")}.{aspxcsFileName.Replace(".aspx.cs", "")}"" %>
                <!DOCTYPE html>
                <html xmlns=""http://www.w3.org/1999/xhtml"" >
                <head runat=""server"" >
                    <meta charset=""UTF-8"" />
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
                    <link href='css/bootstrap.css'  rel='stylesheet' />
                    <link href='css/new.css'  rel='stylesheet' />
                    <link href='css/nav.css'  rel='stylesheet' />
                    <title> Dashboard </title>
                </head>";
        }
        public String GetNavHeader(String surveyName)
        {
            return
           $@"<nav class='navbar  navbar-fixed-top'>
               <div class=""container-fluid"" id=""navdiv"">
                    <div class=""navbar-header"">
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
                                        { surveyName} 
                                       
                                   </button>
                                    <button type = 'button' class='btn btn-primary'>Excel</button>
                                    <button id = 'toPdf' type='button' class='btn btn-primary'>Pdf</button>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>";
        }

    }
}
