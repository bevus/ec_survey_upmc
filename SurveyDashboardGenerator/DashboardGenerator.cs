using SurveyModel;
using System;
using System.Text;

namespace SurveyDashboardGenerator
{
    public class DashboardGenerator
    {
        public string SurveysDirectory { get; set; }
        public Poll Poll { get; set; }
        public DashboardUtils utils = new DashboardUtils();

        public DashboardGenerator(string surveyDirectory, Poll poll)
        {
            this.SurveysDirectory = surveyDirectory;
            this.Poll = poll;
        }

        public string GenerateDashboard()
        {
            var aspxFileName = "Dashboard_" + Poll.ExternalId + ".aspx";
            var aspxcsFileName = "Dashboard_" + Poll.ExternalId + ".aspx.cs";
            var aspxdesinercsFileName = "Dashboard_" + Poll.ExternalId + ".aspx.designer.cs";
            var utils = new DashboardUtils();

            //String aspxdesignercsCode ;

            String aspxCode = GetHeader(aspxcsFileName) +
                              "<body class='jumbotron'>\n" +
                              "<link href='css/bootstrap.css'  rel='stylesheet' />\n" +
                              "<link href='css/new.css'  rel='stylesheet' />\n"+
                              "<link href='css/nav.css'  rel='stylesheet' />\n"+
                              "  <div id = 'dashboard-container' class='dashboard-container'>\n" +
                              "     " + GetNavHeader(Poll.Name) +
                              "     " + GetDashbordContener(Poll.Id) +
                              "  </div>\n" +
                              " " + AddJScript(Poll.Id) +
                              "</body></html>";

            String aspxdesignercsCode =
                "namespace " + aspxcsFileName.Replace(".aspx.cs", "") + "{\n" +
                "       public partial class " + aspxcsFileName.Replace(".aspx.cs", "") + "{\n" +
                "       }\n" +
                "}";

            String aspxcsCode =
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
        public List<object> getGnrlQuestionData(int idQuestion)
        {{
        var poll = manager.getPoll(id_poll);
        var questionData = new List<object>();
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
                var repGnrl = u.GetGeneralResponse(q, poll.TableName.ToString());
                if (repGnrl != null) question.Add(repGnrl);
                break;
            case ""Activity"":
                break;
            case ""Meeting"":
                break;
            case ""Workshop"":
                var repWS = u.GetWSsubQuestion(q, poll.TableWsName.ToString());
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



            //write file
            System.IO.File.WriteAllText(SurveysDirectory + aspxcsFileName, aspxcsCode, Encoding.UTF8);
            System.IO.File.WriteAllText(SurveysDirectory + aspxdesinercsFileName, aspxdesignercsCode, Encoding.UTF8);
            System.IO.File.WriteAllText(SurveysDirectory + aspxFileName, aspxCode.ToString(), Encoding.UTF8);
            //GenerateProcedureStocke();
            return SurveysDirectory + aspxcsFileName;
        }


        public String GetHeader(String aspxcsFileName)
        {
            return
                $@"<%@ Page Language=""C#"" AutoEventWireup=""true"" CodeBehind=""{aspxcsFileName}"" Inherits=""{aspxcsFileName.Replace(".aspx.cs", "")}.{aspxcsFileName.Replace(".aspx.cs", "")}"" %>
                <!DOCTYPE html>\n
                <html xmlns=""http://www.w3.org/1999/xhtml"" >
                <head runat=""server"" >
                    <meta charset=""UTF-8"" />
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
                         
                    <title> Dashboard </title>
                </head>";
        }

        public String GetDashbordContener(int poll_id)
        {
            return
                "<div class='container' id='dashboard-content'>\n" +
                "   <!-- contenue de Question -->\n" +
                "   <div class='container'>\n" +
                "       <div class='row' id='questions'>\n" +
                "       "+utils.GetGeneralQuestionContener(poll_id)+
                "       </div>\n" +
                "   </div>\n" +
                "   <!-- Row Start Faq's Questions -->\n" +
                "   <div class='container'>\n" +
                "       <div class='col-lg-12 col-md-12' id='accordion'>\n" +
                "       </div>\n" +
                "   </div>\n" +
                "</div>\n";
        }

        public String GetNavHeader(String surveyName)
        {
            return
            "<nav class='navbar  navbar-fixed-top'>\n" +
            "   <div class='container-fluid'>\n" +
            "        <div class='navbar-header'>\n" +
            "            <button type = 'button' class='navbar-toggle' data-toggle='collapse' data-target='#myNavbar'>\n" +
            "                <span class='icon-bar'></span>\n" +
            "                <span class='icon-bar'></span>\n" +
            "                <span class='icon-bar'></span>\n" +
            "            </button>\n" +
            "        </div>\n" +
            "        <div class='collapse navbar-collapse' id='myNavbar'>\n" +
            "            <ul class='nav navbar-nav navbar-left'>\n" +
            "                <li>\n" +
            "                    <div class='btn-group' id='btn-group'>\n" +
            "                        <button type = 'button' class='btn btn-primary dropdown-toggle' data-toggle='dropdown'>\n" +
            "                            " + surveyName +
            "                            \n<span class='caret'></span>\n" +
            "                        </button>\n" +
            "                        <button type = 'button' class='btn btn-primary'>Excel</button>\n" +
            "                        <button type = 'button' class='btn btn-primary'>Csv</button>\n" +
            "                        <button id = 'toPdf' type='button' class='btn btn-primary'>Pdf</button>\n" +
            "                    </div>\n" +
            "                </li>\n" +
            "            </ul>\n" +
            "        </div>\n" +
            "    </div>\n" +
            "</nav>\n";
        }

        public String AddJScript(int idPoll)
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
    var $listQ; var $idPoll =" + idPoll + ";\n" +
    $@"$(function() {{
            $(""#questions"").find(""div .gnrlques"").each(function () {{
                        sendDataQAjaxRequest($(this).attr('id')); 
                }});
}});
</script>";
        }
    }
}
