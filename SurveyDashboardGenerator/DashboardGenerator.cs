using SurveyModel;
using System;
using System.Text;
using Widgets;

namespace SurveyDashboardGenerator
{
    public class DashboardGenerator
    {
        public string Directory { get; set; }
        public Poll Poll { get; set; }
        public String Dashbord_name { get; set; }
        public Boolean AllowDataExtraction { get; set; }
        public DashboardUtils utils = new DashboardUtils();

        public DashboardGenerator(string directory, Poll poll, String dashboard_name, Boolean doNotAllowDataExtraction)
        {
            if (poll.Equals(null))
            {
                throw new Exception("Poll = null ");
            }
            this.Directory = directory;
            this.Poll = poll;
            this.Dashbord_name = dashboard_name;
            this.AllowDataExtraction = !doNotAllowDataExtraction;
        }

        public string GenerateDashboard()
        {
            return GenerateDashboard_V1();
        }

        // version 0
        public string GenerateDashboard_V0()
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
            return aspxFileName; ;
        }
        public String GetDashbordContener_V0(int poll_id)
        {
            return
                $@"<div class=""container-fluid"" id=""dashboard-content"">
                       {utils.GetGeneralQuestionContener(poll_id)}
                        <!-- Row Start workshop Questions -->
                        <div class=""container"">
                            <div class=""col-lg-12 col-md-12"" id=""atelier"">
                        </div>
                   </div>
                </div>";
        }
        public String AddJScript_V0(int idPoll)
        {
            return
$@"
<script src = ""js/jquery-3.1.1.min.js""></script>
<script src = ""js/bootstrap.min.js"" ></script> 
<script src = ""js/google-api.js"" ></script>      
<script src = ""js/jspdf.min.js"" ></script>     
<script src = ""js/rgbcolor.js"" ></script>      
<script src = ""js/StackBlur.js"" ></script>       
<script src = ""js/html2canvas.svg.min.js"" ></script>        
<script src = ""js/canvg.min.js"" ></script>
          
<script src = ""js/custom.min.js"" ></script>      
<script src = ""js/pdfGenerator.min.js"" ></script>
          
    <script type =""text/javascript"" >
        google.load('visualization', '1', {{ packages: ['corechart'] }});
        var $listQ;
        var $idPoll ={ idPoll } ;
    $(function() {{
            $(""#questions"").find(""div .gnrlques"").each(function () {{
                        sendDataQAjaxRequest($(this).attr('id'),""{Dashbord_name}.aspx/GetGnrlQuestionData"");       
            }});
            $(""#meeting"").find(""div .gnrlques"").each(function () {{
                        sendDataQAjaxRequest($(this).attr('id'),""{Dashbord_name}.aspx/GetQesMeetingData"");       
            }});
            sendDataAtelierQuestions($idPoll,'atelier',""{Dashbord_name}.aspx/getAtelierQuestions"");
   }});
</script>";
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
                       </div>
                   </div>
                   <!-- Row Start workshop Questions -->
                   <div class=""container"">
                       <div class=""col-lg-12 col-md-12"" id=""workshop"">
                       </div>
                   </div>
                </div>";
        }
        public String AddJScript(int idPoll)
        {
            return
$@"
<script src = ""js/jquery-3.1.1.min.js""></script>
<script src = ""js/bootstrap.min.js"" ></script> 
<script src = ""js/google-api.js"" ></script>      
<script src = ""js/jspdf.min.js"" ></script>     
<script src = ""js/rgbcolor.js"" ></script>      
<script src = ""js/StackBlur.js"" ></script>       
<script src = ""js/html2canvas.svg.min.js"" ></script>        
<script src = ""js/canvg.min.js"" ></script>
          
<script src = ""js/custom.min.js"" ></script>      
<script src = ""js/pdfGenerator.min.js"" ></script>
<script type =""text/javascript"" >
    google.load('visualization', '1', {{ packages: ['corechart'] }});
    var $listQ; var $idPoll ={ idPoll } ;
    $(function() {{
            sendAjaxRequest($idPoll,""{Dashbord_name}.aspx/getQuestions"");
    }});
    $(function () {{
        sendDataAtelierQuestions($idPoll, 'workshop', ""{Dashbord_name}.aspx/getAtelierQuestions"")
    }});
</script>";
        }



        //other methodes
        public String GetCSCode(String aspxcsFileName)
        {
            var ButtonCode = "";

            if (AllowDataExtraction)
            {
                ButtonCode = $@"
        protected void ExtractDataWithDetails(object sender, EventArgs e)
        {{
            var manager = new Manager();

            var dataextraction = new SurveyDataExtractor{{
                PollId = id_poll
            }};
            var poll = manager.getPoll(id_poll);
            var questions = manager.getQuestions(poll.Id);
            string surveytable = poll.TableName;
            string meetingtable = poll.TableMeetingName;
            string sessiontable = poll.TableSessionName;
            string wstable = poll.TableWsName;

            var meetings = (meetingQuestionCount > 0) ? manager.getMeetings(poll.Id, meetingtable) : new List<Meeting>();
            //var attantedmeetings = dataextraction.getAttendedMeetings(poll.Id, meetingtable);
            var sessionAtelier = (sessionQuestionCount > 0) ? DataExtractionUtils.getSessionAtelier(poll.Id, sessiontable) : new List<Atelier>();
            var wsAtelier = (workshopQuestionCount > 0) ? DataExtractionUtils.getWsAtelier(poll.Id, wstable): new List<Atelier>();

            var wb = dataextraction.Print_into_excel_file2(questions, surveytable, meetingtable, sessiontable, wstable, meetings, sessionAtelier, wsAtelier);
            string fullPath = ""~{FormGenerationSettings.SurveyPath}DataWithStatistics_{Poll.ExternalId} "" + DateTime.Now.Millisecond + "".xlsx"";
            try
            {{
                wb.SaveAs(Server.MapPath(fullPath));
                wb.Close();
            }}
            catch (Exception) {{ }}
            DataExtractionUtils.DeleteGeneratedFile(fullPath, 2000);
            Response.Redirect(fullPath);
        }}
        protected void ExtractDataWithStatistics(object sender, EventArgs e)
        {{

            var dataextraction = new SurveyDataExtractor{{
                PollId = id_poll
            }};
            var manager = new Manager();
            var poll = manager.getPoll(id_poll);
            var questions = poll.Questions;
            string surveytable = poll.TableName;
            string meetingtable = poll.TableMeetingName;
            string sessiontable = poll.TableSessionName;
            string wstable = poll.TableWsName;

            var attantedmeetings = (meetingQuestionCount > 0) ? DataExtractionUtils.getAttendedMeetings(poll.Id, meetingtable) : new List<Meeting>();

            var wb = dataextraction.Print_into_excel_file(questions, attantedmeetings, surveytable, meetingtable, sessiontable, wstable);
            string fullPath = ""~{FormGenerationSettings.SurveyPath}DataWithDetails_{Poll.ExternalId} "" + DateTime.Now.Millisecond + "".xlsx"";
            try
            {{
                wb.SaveAs(Server.MapPath(fullPath));
                wb.Close();
            }}
            catch (Exception) {{ }}
            DataExtractionUtils.DeleteGeneratedFile(Page.MapPath(fullPath), {FormGenerationSettings.DeletionWaitTime});
            Response.Redirect(fullPath);
        }}";
            }
            var generalQuestionCount = 0;
            var sessionQuestionCount = 0;
            var workshopQuestionCount = 0;
            var meetingQuestionCount = 0;

            foreach (var question in Poll.Questions)
            {
                switch (question.Category)
                {
                    case QuestionType.General:
                        generalQuestionCount++;
                        break;
                    case QuestionType.Session:
                        sessionQuestionCount++;
                        break;
                    case QuestionType.Meeting:
                        meetingQuestionCount++;
                        break;
                    case QuestionType.Workshop:
                        workshopQuestionCount++;
                        break;
                }
            }
            return
$@"
using System;
using System.Collections.Generic;
using System.Web.Services;
using DataAccess;
using SurveyModel;
using SurveyDashboardGenerator;
using System.Web.Script.Services;
using SurveyDataExtraction;
using Excel = Microsoft.Office.Interop.Excel;

namespace {aspxcsFileName.Replace(".aspx.cs", "")}
{{
    public partial class {aspxcsFileName.Replace(".aspx.cs", "")}  : System.Web.UI.Page
    {{
        protected static Manager manager = new Manager();
        protected static DashboardUtils u = new DashboardUtils();
        protected static int id_poll = {Poll.Id} ;
        int generalQuestionCount = {generalQuestionCount};
        int sessionQuestionCount = {sessionQuestionCount};
        int workshopQuestionCount = {workshopQuestionCount};
        int meetingQuestionCount = {meetingQuestionCount};

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
        public static object GetQesMeetingData(int idQuestion)
        {{
            var poll = manager.getPoll(id_poll);
            var questionData = new object();
            foreach (Question q in poll.Questions)
            {{
                if(q.Category==""Meeting""){{
                     foreach (Question sq in q.SubQuestions){{
                        if (idQuestion.Equals(sq.Id))
                        {{
                             var repMeet = u.GetMeetingDataResponse(sq,poll.TableMeetingName);
                             if (repMeet != null) questionData=repMeet;
                            break;
                        }}
                     }}
                }}
            }}
            return questionData;
        }}

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static List<object> getQuestions(int idPoll)
        {{
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
                    case ""Meeting"":
                        var repMeet = u.GetMeetingsubQuestion(q,poll.TableMeetingName);
                        if (repMeet != null) question.Add(repMeet);
                        break;
                    default:
                        break;
                }}
            }}
            return question;
        }}
        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static List<object> getAtelierQuestions(int idPoll)
        {{

            var question = new List<object>();
            var poll = manager.getPoll(idPoll);

            foreach (Question q in poll.Questions)
            {{
                switch (q.Category.ToString())
                {{
                    case ""Activity"":
                        var repSession = u.GetSessionsubQuestion(idPoll,q, poll.TableSessionName);
                        if (repSession != null) question.Add(repSession);
                        break;
                    case ""Workshop"":
                        var repWS = u.GetWSsubQuestion(idPoll,q, poll.TableWsName);
                        if (repWS != null) question.Add(repWS);
                        break;
                    default:
                        break;
                }}
            }}
            return question;
        }}
         // Excel
        {ButtonCode}
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
                    <link href='css/bootstrap.min.css'  rel='stylesheet' />
                    <link href='css/new.css'  rel='stylesheet' />
                    <link href='css/nav.css'  rel='stylesheet' />
                    <title> Dashboard </title>
                </head>";
        }
        public String GetNavHeader(String surveyName)
        {
            var ButtonExtraction = "";
            if (AllowDataExtraction)
            {
                ButtonExtraction = $@" 
                                        <asp:Button ID = ""button1"" runat=""server"" class='btn btn-primary' OnClick=""ExtractDataWithDetails"" Text=""Excel"" />
                                        <asp:Button ID = ""button2"" runat=""server"" class='btn btn-primary' OnClick=""ExtractDataWithStatistics"" Text=""ExcelStat"" />
                                        <button id = 'toPdf' type='button' class='btn btn-primary'>Pdf</button>";
            }

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
                                    <form id=""form"" runat=""server"">
                                        <button type = 'button' class='btn btn-primary dropdown-toggle' data-toggle='dropdown'>
                                            { surveyName} 
                                       </button>
                                        <button id = 'refresh' type='button' class='btn btn-primary'>Refresh</button>
                                       {ButtonExtraction}
                                    </form>                             
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>";
        }

    }
}