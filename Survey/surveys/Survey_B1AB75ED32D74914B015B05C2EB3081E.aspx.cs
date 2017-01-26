using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using SurveyModel;
using System.Threading;
using Widgets;

namespace Survey.surveys
{
    public partial class Survey_B1AB75ED32D74914B015B05C2EB3081E : Page
    {
        private List<QuestionWebControl> _questionControls;
        private const int PollId = 6;
        private Poll _poll;
        private int _personId;
        private const string ExternalId = "B1AB75ED32D74914B015B05C2EB3081E";
        public bool showTaost = false;
        private readonly FormGenerationSettings settings = new FormGenerationSettings
        {
            UserSurveyFileName = "Survey_B1AB75ED32D74914B015B05C2EB3081E",
            UserDashboardFileName = "Dashboard_B1AB75ED32D74914B015B05C2EB3081E",
            UserAuthType = AuthentificationType.IdInUrl,
            UserPage404 = "pages/Page404.aspx",
            UserOptionQuestionErrorMessage = "Please choose a value",
            UserTextQuestionErrorMessage = "Please write some text",
            UserDateTimeQuestionErrorMessage = "Please choose a value",
            UserAnswerLengthErrorMessage = "Answer too long",
            UserSurveyFormSubmitButtonText = "Submit",
            UserSurveyFormSaveButtonText = "Save",
            UserDisableDataExtraction = false,
            UserNotGenerateDashboard = false
        };
        private Button saveButton = new Button
        {
            Text = "Save",
            CssClass = "saveButton btn btn-success",
        };
        private Button submitButton = new Button
        {
            Text = "Submit",
            CssClass = "saveButton btn btn-primary",
        };
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _personId = int.Parse(Request.QueryString["id"]);
                if (!Manager.ExistPerson(_personId))
                {
                    Response.Redirect("pages/Page404.aspx");
                    return;
                }

                if (Manager.AlreadyAnswerd(PollId, _personId))
                {
                    Response.Redirect("pages/EndSurvey.aspx");
                    return;
                }
            }
            catch (ThreadAbortException){}
            catch (Exception)
            {
                Response.Redirect("pages/Page404.aspx");
            }
            var manager = new Manager();
            _poll = manager.getPoll(PollId, _personId);
            _questionControls = SurveyUtils.RenderForm(_poll, questions, settings);

            submitButton.Click += submitAnswers;
            surveyForm.Controls.Add(submitButton);

            saveButton.Attributes.Add("formnovalidate", "true");
            saveButton.Click += saveAnswers;
            surveyForm.Controls.Add(saveButton);
        } 

        protected void Page_Load(object sender, EventArgs e)
        {
            showTaost = false;
        }
        protected void saveAnswers(object sender, EventArgs e)
        {
            var time = DateTime.Now;
            Manager.SaveAnswer(SurveyUtils.QuestionsWebControlToQuestions(_questionControls), _poll, ExternalId, _personId, time);
            //Response.Redirect("pages/saved.aspx");
            showTaost = true;
        }
        protected void submitAnswers(object sender, EventArgs e)
		{
		    if (!SurveyUtils.Valid(_questionControls, settings)) return;
            var time = DateTime.Now;
            Manager.SaveAnswer(SurveyUtils.QuestionsWebControlToQuestions(_questionControls), _poll, ExternalId, _personId, time);
            Manager.SaveInPollSurvey(PollId, _personId, time);
            Response.Redirect("pages/EndSurvey.aspx");
		}
    }
}
