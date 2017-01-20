using System;
using System.Collections.Generic;
using System.Web.Services;
using DataAccess;
using SurveyModel;
using SurveyDashboardGenerator;
using System.Web.Script.Services;
namespace Dashboard_DFF183EFD77E46868537F45923EAD693
{
    public partial class Dashboard_DFF183EFD77E46868537F45923EAD693  : System.Web.UI.Page
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
        public static List<object> getQuestions(int idPoll)
        {
            DashboardUtils u = new DashboardUtils();
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
                case "Activity":
                    var repSession = u.GetSessionsubQuestion(q, poll.TableSessionName);
                    if (repSession != null) question.Add(repSession);
                    break;
                case "Meeting":
                    var repMeet = u.GetMeetingsubQuestion(q,poll.TableMeetingName);
                    if (repMeet != null) question.Add(repMeet);
                    break;
                case "Workshop":
                    var repWS = u.GetWSsubQuestion(q, poll.TableWsName);
                    if (repWS != null) question.Add(repWS);
                    break;
                default:
                    break;
            }
            }
            return question;
        }
    }
}