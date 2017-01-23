using System.Configuration;
using SurveyModel;

namespace Widgets
{
    public class FormGenerationSettings
    {
        public static string EndGenerationUrl = ConfigurationManager.AppSettings["endGenerationPage"];
        public static string SurveyPrefix = ConfigurationManager.AppSettings["SurveyPrefix"];
        public static string PersonIdArg = ConfigurationManager.AppSettings["PersonIdArgName"];
        public static string Page404 = ConfigurationManager.AppSettings["404Page"];
        public static string AlreadyAnswerdPage = ConfigurationManager.AppSettings["AlreadyAnsweredPage"];
        public static string EndSurveyPage = ConfigurationManager.AppSettings["EndSuveyPage"];
        public static string SavedPage = ConfigurationManager.AppSettings["savedSuveyPage"];
        public static string FormGenerationErrorMessage = "<h3 class='text-danger'>unknown user, please check if you typed the right url</h3>";
        public static string OptionQuestionErrorMessage = ConfigurationManager.AppSettings["optionQuestionErrorMessage"];
        public static string CheckboxesQuestionErrorMessage = ConfigurationManager.AppSettings["checkboxesQuestionErrorMessage"];
        public static string TextQuestionErrorMessage = ConfigurationManager.AppSettings["textQuestionErrorMessage"];
        public static string DateTimeQuestionErrorMessage = ConfigurationManager.AppSettings["dateTimeQuestionErrorMessage"];
        public static string AnswerLengthErrorMessage = ConfigurationManager.AppSettings["answerLengthErrorMessage"];
        public static string CharacterCounterText = ConfigurationManager.AppSettings["characterCounterText"];
        public static string SurveyFormSubmitButtonText = ConfigurationManager.AppSettings["surveyFormSubmitButtonText"];
        public static string SurveyFormId = ConfigurationManager.AppSettings["surveyFormId"];
        public static string QuestionContainerId = ConfigurationManager.AppSettings["questionContainerId"];
        public static string SurveyFormSaveButtonText = ConfigurationManager.AppSettings["saveButtonText"];
        public static string SurveyPath = ConfigurationManager.AppSettings["surveyPath"];
        public static string CssFile = ConfigurationManager.AppSettings["cssFile"];
        public static string JsFile = ConfigurationManager.AppSettings["jsFile"];
        public static AuthentificationType DefaultAuthType = (AuthentificationType)int.Parse(ConfigurationManager.AppSettings["authType"]);
        public static int DeletionWaitTime = int.Parse(ConfigurationManager.AppSettings["fileDeletionSleepTime"]);

        public string UserSurveyFileName { get; set; }
        public string UserDashboardFileName { get; set; }
        public string UserPage404 { get; set; }
        public string UserOptionQuestionErrorMessage { get; set; }
        public string UserCheckboxQuestionErrorMessage { get; set; }
        public string UserTextQuestionErrorMessage { get; set; }
        public string UserDateTimeQuestionErrorMessage { get; set; }
        public string UserAnswerLengthErrorMessage { get; set; }
        public string UserCharacterCounterText { get; set; }
        public string UserSurveyFormSubmitButtonText { get; set; }
        public string UserSurveyFormSaveButtonText { get; set; }
        public AuthentificationType UserAuthType { get; set; }
        public  string UserPersonIdArg { get; set; }
        public bool UserNotGenerateDashboard { get; set; }
        public bool UserDisableDataExtraction { get; set; }
        public string UserCssFile { get; set; }
        public string UserJsFile { get; set; }
    }
}
