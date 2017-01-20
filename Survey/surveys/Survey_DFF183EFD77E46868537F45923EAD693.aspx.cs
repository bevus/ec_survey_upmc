using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using SurveyModel;
using Widgets;

namespace Survey.surveys
{
    public partial class Survey_DFF183EFD77E46868537F45923EAD693 : Page
    {
        private List<QuestionWebControl> _questionControls;
        private const int PollId = 1;
        private Poll _poll;
        private int _personId;
        private const string ExternalId = "DFF183EFD77E46868537F45923EAD693";
        private readonly FormGenerationSettings settings = new FormGenerationSettings
        {
            UserSurveyFileName = "Survey_DFF183EFD77E46868537F45923EAD693",
            UserDashboardFileName = "DashboardSimple",
            UserAuthType = AuthentificationType.IdInUrl,
            UserPage404 = "Page404.aspx",
            UserOptionQuestionErrorMessage = "Please choose a value",
            UserTextQuestionErrorMessage = "Please write some text",
            UserDateTimeQuestionErrorMessage = "Please choose a value",
            UserAnswerLengthErrorMessage = "Answer too long",
            UserSurveyFormSubmitButtonText = "Submit",
            UserSurveyFormSaveButtonText = "Save",
            UserDisableDataExtraction = false,
            UserNotGenerateDashboard = false
        };

        protected override void OnInit(EventArgs e)
        {
            try
            {
                _personId = int.Parse(Request.QueryString["id"]);
                if (!Manager.ExistPerson(_personId))
                {
                    Response.Redirect("Page404.aspx");
                    return;
                }

                if (Manager.AlreadyAnswerd(PollId, _personId))
                {
                    Response.Redirect("EndSurvey.aspx");
                    return;
                }
            }
            catch (Exception)
            {
                Response.Redirect("Page404.aspx");
            }
            var manager = new Manager();
            _poll = manager.getPoll(PollId, _personId);
            _questionControls = SurveyUtils.RenderForm(_poll, questions, settings);
            surveyForm.Controls.Add(new Button
            {
                Text = "Submit",
                CssClass = "btn btn-primary"
            });
            var saveButton = new Button
            {
                Text = "Save",
                CssClass = "saveButton btn btn-success"
            };
            saveButton.Attributes.Add("formnovalidate", "true");
            saveButton.Click += saveAnswers;
            surveyForm.Controls.Add(saveButton);
        } 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) return;
            if (!SurveyUtils.Valid(_questionControls, settings)) return;
            var time = DateTime.Now;
            Manager.SaveAnswer(SurveyUtils.QuestionsWebControlToQuestions(_questionControls), _poll, ExternalId, _personId, time);
            Manager.SaveInPollSurvey(PollId, _personId, time);
            Response.Redirect("EndSurvey.aspx");
        }
        protected void saveAnswers(object sender, EventArgs e)
        {
            var time = DateTime.Now;
            Manager.SaveAnswer(SurveyUtils.QuestionsWebControlToQuestions(_questionControls), _poll, ExternalId, _personId, time);
            Response.Redirect("saved.aspx");
        }
    }
}
