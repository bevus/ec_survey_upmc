using System;
using System.Collections.Generic;
using System.Web.Services;
using DataAccess;
using SurveyModel;
using SurveyDashboardGenerator;
using System.Web.Script.Services;

namespace survey
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected static Manager manager = new Manager();
        protected static DashboardUtils  u = new DashboardUtils();


        protected void Page_Load(object sender, EventArgs e)
        {
            var manager = new Manager();
            //
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





        protected void Button1_Click(object sender, EventArgs e)
        {
            // String content = "<html><body>< div class='widget'><div class='widget-header'><div>2. 67,000 mph(107,826 km/h) represents what speed? </div></div><div class='widget-body'><div id = '1' class='chart-height-md' style='padding: 0px;'><canvas class='base' width='689' height='139' style='width: 518px; height: 138px;'></canvas><canvas class='overlay' width='689' height='139' style='position: absolute; left: 0px; top: 0px; width: 518px; height: 138px;'></canvas><div class='legend'><div style = 'position: absolute; width: 248px; height: 66px; top: 5px; right: 5px; background-color: rgb(255, 255, 255); opacity: 0.85;' > </ div >< table style='position:absolute;top:5px;right:5px;;font-size:smaller;color:#545454'><tbody><tr><td class='legendColorBox'><div style = 'border:1px solid #ccc;padding:1px' >< div style='width:4px;height:0;border:5px solid rgb(92,184,92);overflow:hidden'></div></div></td><td class='legendLabel'>Earth’s movement through the Milky Way</td></tr><tr><td class='legendColorBox'><div style = 'border:1px solid #ccc;padding:1px' >< div style='width:4px;height:0;border:5px solid rgb(5,141,199);overflow:hidden'></div></div></td><td class='legendLabel'>Earth’s orbit around the sun</td></tr><tr><td class='legendColorBox'><div style = 'border:1px solid #ccc;padding:1px' >< div style='width:4px;height:0;border:5px solid rgb(153,153,153);overflow:hidden'></div></div></td><td class='legendLabel'>Earth’s rotation about its axis</td></tr></tbody></table></div></div></div></div></body></html>";
            // PdfDocument d = createPdf(content);
        }


    }
}
