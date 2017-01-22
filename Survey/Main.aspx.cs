using DataAccess;
using System;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using SurveyDashboardGenerator;
using SurveyFormGenerator;
using SurveyModel;
using Widgets;

namespace survey
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (!isValid())
                {
                    showModal.Value = "true";
                }
                else
                {
                    var settings = GetSettings();
                    var manager = new Manager();
                    var poll = manager.getPoll(int.Parse(pollId.Value));
                    var formGenerator = new FormGenerator(Page.MapPath("~" + FormGenerationSettings.SurveyPath), poll,
                        settings);
                    var dashboardGenerator = new DashboardGenerator(Page.MapPath("~" + FormGenerationSettings.SurveyPath), poll,settings.UserDashboardFileName,
                        true);
                    var sUrl = formGenerator.GenerateWebForm();
                    var dUrl = (!settings.UserNotGenerateDashboard)? dashboardGenerator.GenerateDashboard() : "";
                    sUrl = FormGenerationSettings.SurveyPath + sUrl;
                    dUrl = FormGenerationSettings.SurveyPath + dUrl;
                    if (settings.UserAuthType == AuthentificationType.IdInUrl ||
                        settings.UserAuthType == AuthentificationType.HashedIdinUrl)
                    {
                        sUrl += "?" + settings.UserPersonIdArg + "=";
                    }
                    Response.Redirect($@"/EndGeneration.aspx?pollName={poll.Name}&dUrl={dUrl}&sUrl={sUrl}&authMod={(int)settings.UserAuthType}&argName={settings.UserPersonIdArg}");
                }
            }
            else
            {
                initConfigurationForm();
            }
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected bool isValid()
        {
            var identifierErrorText = "invalide file name : must contain only alphabetic characters or digits, must start with an alphabetic character and less than 50 characters";
            var valid = true;
            if (!Regex.IsMatch(surveyFileName.Text, "^[_a-zA-Z][_a-zA-Z0-9]{0,50}$"))
            {
                _surveyFileName.Text = identifierErrorText;
                _surveyFileName.Visible = true;
                valid = false;
            }
            else
            {
                _surveyFileName.Visible = false;
            }
            if (!Regex.IsMatch(dashboardFileName.Text, @"^[_a-zA-Z][_a-zA-Z0-9]{0,50}$"))
            {
                _dashboardFileName.Text = identifierErrorText;
                _dashboardFileName.Visible = true;
                valid = false;
            }
            else
            {
                _dashboardFileName.Visible = false;
            }
            if (authType.SelectedIndex < 0 || authType.SelectedIndex > 2)
            {
                _authType.Text = "invalide authentification type";
                _authType.Visible = true;
                valid = false;
            }
            else
            {
                _authType.Visible = false;
            }
            if (!Regex.IsMatch(personIdArg.Text, @"^[_a-zA-Z][_a-zA-Z0-9]{0,20}$"))
            {
                _personIdArg.Text = identifierErrorText;
                _personIdArg.Visible = true;
                valid = false;
            }
            else
            {
                _personIdArg.Visible = false;
            }
            try
            {
                int.Parse(pollId.Value);
                _pollId.Visible = false;
            }
            catch (Exception)
            {
                _pollId.Text = "no poll selected";
                _pollId.Visible = true;
                valid = false;
            }

            return valid;
        }

        public void initConfigurationForm()
        {
            surveyFileName.Text = "Survey_XXX";
            dashboardFileName.Text = "Dashboard_XXX";
            authType.SelectedIndex = (int)FormGenerationSettings.DefaultAuthType;
            page404.Text = FormGenerationSettings.Page404;
            optoinQuestionErrorMessage.Text = FormGenerationSettings.OptionQuestionErrorMessage;
            checkboxQuestionErrorMessage.Text = FormGenerationSettings.CheckboxesQuestionErrorMessage;
            textQuestionErrorMessage.Text = FormGenerationSettings.TextQuestionErrorMessage;
            dateTimeQuestionErrorMessage.Text = FormGenerationSettings.DateTimeQuestionErrorMessage;
            answerLengthErrorMessage.Text = FormGenerationSettings.AnswerLengthErrorMessage;
            submissionButtonText.Text = FormGenerationSettings.SurveyFormSubmitButtonText;
            saveButtonText.Text = FormGenerationSettings.SurveyFormSaveButtonText;
            personIdArg.Text = FormGenerationSettings.PersonIdArg;
            cssFileName.Text = FormGenerationSettings.CssFile;
        }

        public FormGenerationSettings GetSettings()
        {
            return new FormGenerationSettings
            {
                UserSurveyFileName = surveyFileName.Text,
                UserDashboardFileName = dashboardFileName.Text,
                UserAuthType = (AuthentificationType)authType.SelectedIndex,
                UserPage404 = page404.Text,
                UserOptionQuestionErrorMessage = optoinQuestionErrorMessage.Text,
                UserCheckboxQuestionErrorMessage = checkboxQuestionErrorMessage.Text,
                UserTextQuestionErrorMessage = textQuestionErrorMessage.Text,
                UserDateTimeQuestionErrorMessage = dateTimeQuestionErrorMessage.Text,
                UserAnswerLengthErrorMessage = answerLengthErrorMessage.Text,
                UserSurveyFormSubmitButtonText = submissionButtonText.Text,
                UserSurveyFormSaveButtonText = saveButtonText.Text,
                UserDisableDataExtraction = noDataExtarction.Checked,
                UserNotGenerateDashboard = noDashboard.Checked,
                UserPersonIdArg = personIdArg.Text,
                UserCssFile = cssFileName.Text
            };
        }
    }
}