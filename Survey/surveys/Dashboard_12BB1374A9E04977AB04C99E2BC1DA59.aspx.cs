
using System;
using System.Collections.Generic;
using System.Web.Services;
using DataAccess;
using SurveyModel;
using SurveyDashboardGenerator;
using System.Web.Script.Services;
using SurveyDataExtractorGenerator;
using Excel = Microsoft.Office.Interop.Excel;

namespace Dashboard_12BB1374A9E04977AB04C99E2BC1DA59
{
    public partial class Dashboard_12BB1374A9E04977AB04C99E2BC1DA59  : System.Web.UI.Page
    {
        protected static Manager manager = new Manager();
        protected static DashboardUtils u = new DashboardUtils();
        protected static int id_poll = 1 ;
        protected void Page_Load(object sender, EventArgs e)
        {
            var manager = new Manager();
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static object GetGnrlQuestionData(int idQuestion)
        {
            var poll = manager.getPoll(id_poll);
            var questionData = new object();
            foreach (Question q in poll.Questions)
            {
                if (idQuestion.Equals(q.Id))
                {
                    questionData = u.GetQesDataResponse(q, poll.TableName);
                    break;
                }
            }
            return questionData;
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static object GetQesMeetingData(int idQuestion)
        {
            var poll = manager.getPoll(id_poll);
            var questionData = new object();
            foreach (Question q in poll.Questions)
            {
                if(q.Category=="Meeting"){
                     foreach (Question sq in q.SubQuestions){
                        if (idQuestion.Equals(sq.Id))
                        {
                             var repMeet = u.GetMeetingDataResponse(sq,poll.TableMeetingName);
                             if (repMeet != null) questionData=repMeet;
                            break;
                        }
                     }
                }
            }
            return questionData;
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static List<object> getQuestions(int idPoll)
        {
            var question = new List<object>();
            var poll = manager.getPoll(idPoll);
                
            foreach (Question q in poll.Questions)
            {
                switch (q.Category.ToString())
                {
                    case "General":
                        var repGnrl = u.GetGeneralResponse(q, poll.TableName);
                        if (repGnrl != null) question.Add(repGnrl);
                        break;
                    case "Meeting":
                        var repMeet = u.GetMeetingsubQuestion(q,poll.TableMeetingName);
                        if (repMeet != null) question.Add(repMeet);
                        break;
                    default:
                        break;
                }
            }
            return question;
        }
        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static List<object> getAtelierQuestions(int idPoll)
        {

            var question = new List<object>();
            var poll = manager.getPoll(idPoll);

            foreach (Question q in poll.Questions)
            {
                switch (q.Category.ToString())
                {
                    case "Activity":
                        var repSession = u.GetSessionsubQuestion(idPoll,q, poll.TableSessionName);
                        if (repSession != null) question.Add(repSession);
                        break;
                    case "Workshop":
                        var repWS = u.GetWSsubQuestion(idPoll,q, poll.TableWsName);
                        if (repWS != null) question.Add(repWS);
                        break;
                    default:
                        break;
                }
            }
            return question;
        }
         // Excel

        
        protected void ExtractDataWithDetails(object sender, EventArgs e)
        {
            var manager = new Manager();

            SurveyDataExtraction dataextraction = new SurveyDataExtraction();
            var poll = manager.getPoll(1);
            var questions = manager.getQuestions(poll.Id);


            string surveytable = poll.TableName;
            string meetingtable = poll.TableMeetingName;
            string sessiontable = poll.TableSessionName;
            string wstable = poll.TableWsName;
            var meetings = manager.getMeetings(poll.Id, meetingtable);
            //var attantedmeetings = dataextraction.getAttendedMeetings(poll.Id, meetingtable);
            var sessionAtelier = dataextraction.getSessionAtelier(poll.Id, sessiontable);
            var wsAtelier = dataextraction.getWsAtelier(poll.Id, wstable);

            var wb = dataextraction.Print_into_excel_file2(questions, surveytable, meetingtable, sessiontable, wstable, meetings, sessionAtelier, wsAtelier);
            string fullPath = "~/surveys/DataWithStatistics.xlsx";
            try
            {
                wb.SaveAs(Server.MapPath(fullPath));
                wb.Close();
            }
            catch (Exception) { }

            Response.Redirect(fullPath);
            System.IO.File.Delete(Server.MapPath(fullPath));
        }

        protected void ExtractDataWithStatistics(object sender, EventArgs e)
        {

            SurveyDataExtraction dataextraction = new SurveyDataExtraction();
            var manager = new Manager();
            var poll = manager.getPoll(1);
            var questions = poll.Questions;
            string surveytable = poll.TableName;
            string meetingtable = poll.TableMeetingName;
            string sessiontable = poll.TableSessionName;
            string wstable = poll.TableWsName;
            var attantedmeetings = dataextraction.getAttendedMeetings(poll.Id, meetingtable);
            var wb = dataextraction.Print_into_excel_file(questions, attantedmeetings, surveytable, meetingtable, sessiontable, wstable);
            string fullPath = "~/surveys/DataWithDetails.xlsx";
            try
            {
                wb.SaveAs(Server.MapPath(fullPath));
                wb.Close();
            }
            catch (Exception) { }

            Response.Redirect(fullPath);
            System.IO.File.Delete(Server.MapPath(fullPath));

        }

    }
}