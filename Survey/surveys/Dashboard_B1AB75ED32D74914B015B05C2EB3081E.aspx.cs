
using System;
using System.Collections.Generic;
using System.Web.Services;
using DataAccess;
using SurveyModel;
using SurveyDashboardGenerator;
using System.Web.Script.Services;
using SurveyDataExtraction;
using Excel = Microsoft.Office.Interop.Excel;

namespace Dashboard_B1AB75ED32D74914B015B05C2EB3081E
{
    public partial class Dashboard_B1AB75ED32D74914B015B05C2EB3081E  : System.Web.UI.Page
    {
        protected static Manager manager = new Manager();
        protected static DashboardUtils u = new DashboardUtils();
        protected static int id_poll = 6 ;
        int generalQuestionCount = 25;
        int sessionQuestionCount = 1;
        int workshopQuestionCount = 1;
        int meetingQuestionCount = 1;

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

            var dataextraction = new SurveyDataExtractor{
                PollId = id_poll
            };
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
            string fullPath = "~/surveys/DataWithStatistics_B1AB75ED32D74914B015B05C2EB3081E " + DateTime.Now.Millisecond + ".xlsx";
            try
            {
                wb.SaveAs(Server.MapPath(fullPath));
                wb.Close();
            }
            catch (Exception) { }
            DataExtractionUtils.DeleteGeneratedFile(fullPath, 2000);
            Response.Redirect(fullPath);
        }
        protected void ExtractDataWithStatistics(object sender, EventArgs e)
        {

            var dataextraction = new SurveyDataExtractor{
                PollId = id_poll
            };
            var manager = new Manager();
            var poll = manager.getPoll(id_poll);
            var questions = poll.Questions;
            string surveytable = poll.TableName;
            string meetingtable = poll.TableMeetingName;
            string sessiontable = poll.TableSessionName;
            string wstable = poll.TableWsName;

            var attantedmeetings = (meetingQuestionCount > 0) ? DataExtractionUtils.getAttendedMeetings(poll.Id, meetingtable) : new List<Meeting>();

            var wb = dataextraction.Print_into_excel_file(questions, attantedmeetings, surveytable, meetingtable, sessiontable, wstable);
            string fullPath = "~/surveys/DataWithDetails_B1AB75ED32D74914B015B05C2EB3081E " + DateTime.Now.Millisecond + ".xlsx";
            try
            {
                wb.SaveAs(Server.MapPath(fullPath));
                wb.Close();
            }
            catch (Exception) { }
            DataExtractionUtils.DeleteGeneratedFile(Page.MapPath(fullPath), 2000);
            Response.Redirect(fullPath);
        }
    }
}