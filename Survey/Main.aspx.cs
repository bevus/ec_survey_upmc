using DataAccess;
using System;
using System.IO;
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
                    var sUrl = formGenerator.GenerateWebForm();
                    var dUrl = "";
                    sUrl = FormGenerationSettings.SurveyPath + sUrl;
                    if (!settings.UserNotGenerateDashboard)
                    {
                        var dashboardGenerator = new DashboardGenerator(
                            Page.MapPath("~" + FormGenerationSettings.SurveyPath),
                            poll, settings.UserDashboardFileName,
                            settings.UserDisableDataExtraction
                        );
                        dUrl = FormGenerationSettings.SurveyPath + dashboardGenerator.GenerateDashboard();
                    }
                    if (settings.UserAuthType == AuthentificationType.IdInUrl ||
                        settings.UserAuthType == AuthentificationType.HachedIdInUrl)
                    {
                        sUrl += "?" + settings.UserPersonIdArg + "=";
                    }
                    Response.Redirect($@"{FormGenerationSettings.EndGenerationUrl}?pollId={poll.Id}&pollName={poll.Name}&dUrl={dUrl}&sUrl={sUrl}&authMod={(int)settings.UserAuthType}&argName={settings.UserPersonIdArg}");
                }
            }
            else
            {
                InitConfigurationForm();
            }
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        private bool check(Label errorLabel, Func<bool> cond, string errorMessage)
        {
            if (cond())
            {
                errorLabel.Text = errorMessage;
                errorLabel.Visible = true;
                return false;
            }
            else
            {
                errorLabel.Text = string.Empty;
                errorLabel.Visible = true;
                return true;
            }
        }
        protected bool isValid()
        {
            var identifierErrorText = "invalide file name : must contain only alphabetic characters or digits, must start with an alphabetic character and less than 50 characters";
            var noPollSelected = "no poll selected";
            var fileExists = "there is already a file with the same name, please choose a different name";
            var authTypeError = "invalide authentification type";
            var sameNameError = "survey form and dashboard can not have the same name";

            var valid = true;
            
            valid = check(_surveyFileName,
            () => !Regex.IsMatch(surveyFileName.Text, "^[_a-zA-Z][_a-zA-Z0-9]{0,50}$"),
            identifierErrorText);

            if (valid)
            {
                valid = check(_surveyFileName, 
                () => surveyFileName.Text.Equals(dashboardFileName.Text),
                sameNameError);
            }

            if (valid)
            {
                valid = check(_surveyFileName,
                () => File.Exists(Page.MapPath(FormGenerationSettings.SurveyPath + surveyFileName.Text + ".aspx")),
                fileExists);
            }

            if (valid)
            {
                valid = check(_dashboardFileName,
                () => File.Exists(Page.MapPath(FormGenerationSettings.SurveyPath + dashboardFileName.Text + ".aspx")),
                fileExists);
            }

            if (valid)
            {
                valid = check(_dashboardFileName,
                () => !Regex.IsMatch(dashboardFileName.Text, @"^[_a-zA-Z][_a-zA-Z0-9]{0,50}$"),
                identifierErrorText);
            }
            if (valid)
            {
                valid = check(_authType,
                () => authType.SelectedIndex < 0 || authType.SelectedIndex > 2,
                authTypeError);
            }
            if (valid)
            {
                valid = check(_personIdArg,
                () => !Regex.IsMatch(personIdArg.Text, @"^[_a-zA-Z][_a-zA-Z0-9]{0,20}$"),
                identifierErrorText);
            }
            try
            {
                int.Parse(pollId.Value);
                _pollId.Visible = false;
            }
            catch (Exception)
            {
                _pollId.Text = noPollSelected;
                _pollId.Visible = true;
                valid = false;
            }

            return valid;
        }

        public void InitConfigurationForm()
        {
            surveyFileName.Text = "";
            dashboardFileName.Text = "";
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
            jsFileName.Text = FormGenerationSettings.JsFile;
            characterCounterText.Text = FormGenerationSettings.CharacterCounterText;
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
                UserCssFile = cssFileName.Text,
                UserJsFile = jsFileName.Text,
                UserCharacterCounterText = characterCounterText.Text
            };
        }
    }
}