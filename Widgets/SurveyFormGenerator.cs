﻿using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using SurveyModel;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using Widgets;

namespace SurveyFormGenerator
{
    public class FormGenerator
    {
        private readonly string aspxFileName;
        private readonly string aspxcsFileName;
        private readonly string aspxdesinercsFileName;
        private readonly string storedProceduresFileName;
        private readonly FormGenerationSettings settings;

        public string SurveysDirectory {get; set;}
        public Poll Poll { get; set; }
        public FormGenerator(string surveysDirectory, Poll poll, FormGenerationSettings settings)
        {
            if(poll == null) { throw new Exception("poll is null");}
            if(poll.Questions.Count == 0) { throw new Exception("no quetions in poll");}
            SurveysDirectory = surveysDirectory;
            Poll = poll;
            this.settings = settings;
            aspxFileName = settings.UserSurveyFileName + ".aspx";
            aspxcsFileName = settings.UserSurveyFileName + ".aspx.cs";
            aspxdesinercsFileName = settings.UserSurveyFileName + ".aspx.designer.cs";
            storedProceduresFileName = settings.UserSurveyFileName + ".sql";
        }
        public string GenerateWebForm()
        {
            if (Poll.Questions.Count == 0)
                throw new Exception("Empty poll");
            System.IO.File.WriteAllText(SurveysDirectory + aspxcsFileName, generateAspxCsCode(), Encoding.UTF8);
            System.IO.File.WriteAllText(SurveysDirectory + aspxdesinercsFileName, generateAspxDesignerCsCode(), Encoding.UTF8);
            System.IO.File.WriteAllText(SurveysDirectory + aspxFileName, generateAspxCode(), Encoding.UTF8);
            System.IO.File.WriteAllText(SurveysDirectory + storedProceduresFileName, generateStoredProcedure(), Encoding.UTF8);
            ExecuteStoredProcedure(SurveysDirectory + storedProceduresFileName);
            System.IO.File.Delete(SurveysDirectory + storedProceduresFileName);
            return aspxFileName;
        }
        private void ExecuteStoredProcedure(string fileName)
        {
            string script = System.IO.File.ReadAllText(fileName);
            SqlConnection connexion = ConnexionClasse.getConnexion();
            connexion.Open();
            try
            {
                Server server = new Server(new ServerConnection(connexion));
                server.ConnectionContext.ExecuteNonQuery(script);
            }
            catch (Exception e)
            {
                throw e;
            }
            connexion.Close();
        }

        public string generateAspxCode()
        {
            return
$@"<%@ Page Language=""C#"" AutoEventWireup=""true"" 
CodeFile=""{aspxcsFileName}"" Inherits=""Survey.surveys.{settings.UserSurveyFileName}""
MasterPageFile=""../SurveyMasterPage.master""%>
    <asp:Content runat=""server"" contentplaceholderid=""scripts"">
        <link href=""{settings.UserCssFile}"" rel=""stylesheet""/>
        <script src=""../scripts/jquery-3.1.1.js""></script>
        <script src=""../scripts/jquery.validate.min.js""></script>
        <script src=""../scripts/bootstrap.js""></script>
        <script src=""../scripts/jquery.validation_1.15.0_additional-methods.js""></script>
        <script src=""../scripts/textcounter.min.js""></script>
        <script src=""{settings.UserJsFile}""></script>
        <style>
            #toast {{
                margin-left: -100px; 
                background-color: #999; 
                color: #fff; 
                width: 200px;
                text-align: center; 
                border-radius: 2px; 
                padding: 20px 0px; 
                position: fixed; 
                z-index: 1; 
                left: 50%; 
                bottom: 30px; 
                box-shadow: 0 0px 5px 0 #eee;
            }}
        </style>
    </asp:Content>
    <asp:Content runat=""server"" contentplaceholderid=""surveyFormPlaceHolder"">
        <h1 class=""page-header surveyName"">{Poll.Name}</h1>
        <form class=""surveyForm"" id=""{FormGenerationSettings.SurveyFormId}"" runat=""server"">
            <div id=""{FormGenerationSettings.QuestionContainerId}"" runat=""server"">
            </div>
        </form>
        <% if (showTaost){{ %>
        <div id=""toast"">
            <i class=""glyphicon glyphicon-saved""></i> {FormGenerationSettings.ToastSaveText}
        </div>
        <% }} %>
        <script>
            function showToast() {{
                $(""#toast"").fadeIn(200);
            }}

            function hideToast(parameters) {{
                $(""#toast"").fadeOut(200);
            }}
            $(function () {{
                <% if (showTaost){{ %>
                setTimeout(hideToast, 1500);
            <% }} %>
            $(""textarea"").addClass(""form-control"");
            $(""select, input[type=text], input[type=datetime], input[type=date], input[type=email], input[type=telephone], input[type=password], input[type=number]"").addClass(""form-control"");
            $(""form input[type=submit]"").wrapAll('<div class=""col-md-12"" id=""buttons""/>');
            $(""#buttons"").wrapAll('<div class=""row""/>')
            $.each($(""input[type=radio], input[type=checkbox]""), function (i, e) {{
                $(e).add($(""label[for='"" + e.id + ""']"")).wrapAll('<div class=""choice""/>');
            }});
            $.each($(""textarea.QuestionControl.CommentsBox""),
            function(i, e) {{
                $(e)
                    .textcounter({{
                        max: parseInt($(e).attr(""data-max"")),
                        countDown: true,
                        countDownText: ""{settings.UserCharacterCounterText}""//**********
                    }});
            }});
            var rules = {{}};
            var messages = {{}};
            var checkboxClassCount = 0;
            var checkboxGroupClass = ""checkboxGroup"";

            $.each($("".QuestionControl.CommentsBox.Mandatory, .QuestionControl.TextBox.Mandatory""), function (i, e) {{
                rules[$(e).attr('name')] = {{ required: true, maxlength: parseInt($(e).attr(""data-max"")) }};
                messages[$(e).attr('name')] = {{required: ""{settings.UserTextQuestionErrorMessage}"", maxlenght: ""{settings.UserAnswerLengthErrorMessage}""}}
            }});

            $.each($("".QuestionControl.RadioButtonList.Mandatory""), function (i, e) {{
                $.each($(e).find(""input[type=radio]""), function (j, r) {{
                    rules[$(r).attr('name')] = {{required: true}};
                    messages[$(r).attr('name')] = {{required: ""{settings.UserOptionQuestionErrorMessage}""}};
                }});
            }});

            $.each($("".QuestionControl.CheckBoxList.Mandatory""), function (i, e) {{
                var currentGroupClass = checkboxGroupClass + checkboxClassCount;
                checkboxClassCount++;
                $(e).find(""input[type=checkbox]"").addClass(currentGroupClass);
                var isSet = false;
                $.each($(e).find(""input[type=checkbox]""), function (j, c) {{
                    rules[$(c).attr('name')] = {{ require_from_group: [parseInt($(e).attr(""data-nbchoices"")), ""."" + currentGroupClass] }};
                    if (!isSet) {{
                        var str = ""{settings.UserCheckboxQuestionErrorMessage}""
                        messages[$(c).attr('name')] = {{ require_from_group: str.replace(""%d"", $(e).attr(""data-nbchoices"")) }};
                        isSet = true;
                    }} else {{
                        messages[$(c).attr('name')] = {{ require_from_group: """" }};
                    }}
                }});
            }});

            console.log(rules);

            $(""#surveyForm"").validate({{
                errorClass: ""has-error"",
                validClass: ""has-success"",
                rules: rules,
                messages: messages,
                errorPlacement: function (error, element) {{
                    if (element.is("":radio"") || element.is("":checkbox"")) {{
                        error.appendTo(element.parent().parent());
                    }}
                    else {{
                        error.insertAfter(element);
                    }}
                }}
            }});
        }});
        </script>
    </asp:Content>
";
        }

        public string generateAspxCsCode()
        {
            var getPersonId = "_personId = ";
            switch (settings.UserAuthType)
            {
                case AuthentificationType.IdInUrl:
                    getPersonId += $@"int.Parse(Request.QueryString[""{settings.UserPersonIdArg}""]);";
                    break;
                case AuthentificationType.HachedIdInUrl:
                    getPersonId += $@"Manager.GetHashedId(Request.QueryString[""{settings.UserPersonIdArg}""], PollId);";
                    break;
                case AuthentificationType.IdInSession:
                    getPersonId += $@"Convert.ToInt32(Session[""{settings.UserPersonIdArg}""]);";
                    break;
            }
            return
$@"using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using SurveyModel;
using System.Threading;
using Widgets;

namespace Survey.surveys
{{
    public partial class {settings.UserSurveyFileName} : Page
    {{
        private List<QuestionWebControl> _questionControls;
        private const int PollId = {Poll.Id};
        private Poll _poll;
        private int _personId;
        private const string ExternalId = ""{Poll.ExternalId}"";
        public bool showTaost = false;
        private readonly FormGenerationSettings settings = new FormGenerationSettings
        {{
            UserSurveyFileName = ""{settings.UserSurveyFileName}"",
            UserDashboardFileName = ""{settings.UserDashboardFileName}"",
            UserAuthType = AuthentificationType.{settings.UserAuthType},
            UserPage404 = ""{settings.UserPage404}"",
            UserOptionQuestionErrorMessage = ""{settings.UserOptionQuestionErrorMessage}"",
            UserTextQuestionErrorMessage = ""{settings.UserTextQuestionErrorMessage}"",
            UserDateTimeQuestionErrorMessage = ""{settings.UserDateTimeQuestionErrorMessage}"",
            UserAnswerLengthErrorMessage = ""{settings.UserAnswerLengthErrorMessage}"",
            UserSurveyFormSubmitButtonText = ""{settings.UserSurveyFormSubmitButtonText}"",
            UserSurveyFormSaveButtonText = ""{settings.UserSurveyFormSaveButtonText}"",
            UserDisableDataExtraction = {settings.UserDisableDataExtraction.ToString().ToLower()},
            UserNotGenerateDashboard = {settings.UserNotGenerateDashboard.ToString().ToLower()}
        }};
        private Button saveButton = new Button
        {{
            Text = ""{settings.UserSurveyFormSaveButtonText}"",
            CssClass = ""saveButton btn btn-success"",
        }};
        private Button submitButton = new Button
        {{
            Text = ""{settings.UserSurveyFormSubmitButtonText}"",
            CssClass = ""saveButton btn btn-primary"",
        }};
        protected override void OnInit(EventArgs e)
        {{
            try
            {{
                {getPersonId}
                if (!Manager.ExistPerson(_personId))
                {{
                    Response.Redirect(""{settings.UserPage404}"");
                    return;
                }}

                if (Manager.AlreadyAnswerd(PollId, _personId))
                {{
                    Response.Redirect(""{FormGenerationSettings.EndSurveyPage}"");
                    return;
                }}
            }}
            catch (ThreadAbortException){{}}
            catch (Exception)
            {{
                Response.Redirect(""{settings.UserPage404}"");
            }}
            var manager = new Manager();
            _poll = manager.getPoll(PollId, _personId);
            _questionControls = SurveyUtils.RenderForm(_poll, {FormGenerationSettings.QuestionContainerId}, settings);

            submitButton.Click += submitAnswers;
            surveyForm.Controls.Add(submitButton);

            saveButton.Attributes.Add(""formnovalidate"", ""true"");
            saveButton.Click += saveAnswers;
            surveyForm.Controls.Add(saveButton);
        }} 

        protected void Page_Load(object sender, EventArgs e)
        {{
            showTaost = false;
        }}
        protected void saveAnswers(object sender, EventArgs e)
        {{
            var time = DateTime.Now;
            Manager.SaveAnswer(SurveyUtils.QuestionsWebControlToQuestions(_questionControls), _poll, ExternalId, _personId, time);
            //Response.Redirect(""pages/saved.aspx"");
            showTaost = true;
        }}
        protected void submitAnswers(object sender, EventArgs e)
		{{
		    if (!SurveyUtils.Valid(_questionControls, settings)) return;
            var time = DateTime.Now;
            Manager.SaveAnswer(SurveyUtils.QuestionsWebControlToQuestions(_questionControls), _poll, ExternalId, _personId, time);
            Manager.SaveInPollSurvey(PollId, _personId, time);
            Response.Redirect(""{FormGenerationSettings.EndSurveyPage}"");
		}}
    }}
}}
";
        }

        public string generateAspxDesignerCsCode()
        {
            return 
$@"
namespace Survey.surveys {{
    public partial class {settings.UserSurveyFileName} {{
        protected global::System.Web.UI.HtmlControls.HtmlForm surveyForm;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl questions;
    }}
}}
";
        }

        public string generateStoredProcedure()
        {
            var listParamSession = "";
            var listColumnSession = "";
            var listParamMeeting = "";
            var listColumnMeeting = "";
            var listParamWs = "";
            var listColumnWs = "";
            var listParamGeneral = "";
            var listColumnGeneral = "";
            var sessionCount = 0;
            var meetingCount = 0;
            var wsCount = 0;
            var qCount = 0;    

            foreach (var q in Poll.Questions)
            {
                if (q.Category == QuestionType.Session)
                foreach (var sq in q.SubQuestions)
                {
                    sessionCount++;
                    listParamSession += "\t@" + sq.Column + " nvarchar(" + sq.MaxSize + "),\n";
                    listColumnSession += "\tSUB_" + sq.Column + " = @" + sq.Column + ",\n";
                }
                if (q.Category == QuestionType.Meeting)
                foreach (var sq in q.SubQuestions)
                {
                    meetingCount++;
                    listParamMeeting += "\t@" + sq.Column + " nvarchar(" + sq.MaxSize + "),\n";
                    listColumnMeeting += "\tSUM_" + sq.Column + " = @" + sq.Column + ",\n";
                }
                if (q.Category == QuestionType.Workshop)
                foreach (var sq in q.SubQuestions)
                {
                    wsCount++;
                    listParamWs += "\t@" + sq.Column + " nvarchar(" + sq.MaxSize + "),\n";
                    listColumnWs += "\tSUB_" + sq.Column + " = @" + sq.Column + ",\n";
                }
                if (q.Category == QuestionType.General)
                {
                    qCount++;
                    listParamGeneral += "\t@" + q.Column + " nvarchar(" + q.MaxSize + "),\n";
                    listColumnGeneral += "\t" + q.Column + " = @" + q.Column + ",\n";
                }
            }
            if (listParamSession.Length > 0)
            {
                listParamSession = listParamSession.Remove(listParamSession.Length - 2, 1);
                listColumnSession = listColumnSession.Remove(listColumnSession.Length - 2, 1);
            }
            if (listParamMeeting.Length > 0)
            {
                listParamMeeting = listParamMeeting.Remove(listParamMeeting.Length - 2, 1);
                listColumnMeeting = listColumnMeeting.Remove(listColumnMeeting.Length - 2, 1);
            }
            if (listParamWs.Length > 0)
            {
                listParamWs = listParamWs.Remove(listParamWs.Length - 2, 1);
                listColumnWs = listColumnWs.Remove(listColumnWs.Length - 2, 1);
            }
            if (listParamGeneral.Length > 0)
            {
                listParamGeneral = listParamGeneral.Remove(listParamGeneral.Length - 2, 1);
                listColumnGeneral = listColumnGeneral.Remove(listColumnGeneral.Length - 2, 1);
            }
            
            var i_general = $@"
IF OBJECT_ID('empty_survey_{Poll.ExternalId}', 'P') IS NOT NULL
DROP PROC empty_survey_{Poll.ExternalId}
GO
CREATE PROCEDURE empty_survey_{Poll.ExternalId}
as
begin
	delete from POLL_SURVEY WHERE POLL_SURVEY.SUR_id_poll = {Poll.Id};
	truncate table {Poll.TableName};
	truncate table {Poll.TableMeetingName};
	truncate table {Poll.TableSessionName};
	truncate table {Poll.TableWsName};
end
GO
IF OBJECT_ID('I{Poll.ExternalId}', 'P') IS NOT NULL
DROP PROC I{Poll.ExternalId}
GO
CREATE PROCEDURE I{Poll.ExternalId}
(
    @person_id int,
{listParamGeneral}
)
AS
if not exists(SELECT id_person FROM Poll_SURVEY_{Poll.ExternalId} WHERE id_person = @person_id )
BEGIN
    INSERT INTO Poll_SURVEY_{Poll.ExternalId} (id_person) VALUES(@person_id)
end
    UPDATE  Poll_SURVEY_{Poll.ExternalId}
SET
{listColumnGeneral}
WHERE id_person = @person_id
";

            var i_session = $@"
IF OBJECT_ID('I_SESSION_{Poll.ExternalId}', 'P') IS NOT NULL
DROP PROC I_SESSION_{Poll.ExternalId}
GO
CREATE PROCEDURE I_SESSION_{Poll.ExternalId}
(
	@id_survey int,
	@id_poll int,
	@id_person int,
	@id_atelier int,
	@date_mod datetime,
{listParamSession}
)
AS
SET IDENTITY_INSERT POLL_SURVEY_SESSION_{Poll.ExternalId} ON;
if not exists(SELECT SUB_id_person FROM POLL_SURVEY_SESSION_{Poll.ExternalId} WHERE
	SUB_id_survey  = @id_survey 	and 
	SUB_id_poll    = @id_poll   	and
	SUB_id_person  = @id_person 	and
	SUB_id_atelier = @id_atelier)
BEGIN
	INSERT INTO POLL_SURVEY_SESSION_{Poll.ExternalId} (
		SUB_id_survey,
		SUB_id_poll,
		SUB_id_person,
		SUB_id_atelier
	) 
	VALUES(
		@id_survey, 
		@id_poll,
		@id_person,
		@id_atelier
	)
end
	UPDATE  POLL_SURVEY_SESSION_{Poll.ExternalId}
SET
	SUB_date_mod = @date_mod,
    SUB_attended = 1,
{listColumnSession}
WHERE 
	SUB_id_survey  = @id_survey 	and 
	SUB_id_poll    = @id_poll   	and
	SUB_id_person  = @id_person 	and
	SUB_id_atelier = @id_atelier
";
            var i_meeting = $@"
IF OBJECT_ID('I_MEETING_{Poll.ExternalId}', 'P') IS NOT NULL
DROP PROC I_MEETING_{Poll.ExternalId}
GO
CREATE PROCEDURE I_MEETING_{Poll.ExternalId}
(
    @id_survey int,
    @id_poll int,
    @id_meeting int,
    @id_person int,
    @id_company int,
    @date_mod datetime,
{listParamMeeting}
)
AS
SET IDENTITY_INSERT POLL_SURVEY_MEETING_{Poll.ExternalId} ON;
if not exists(SELECT SUM_id_person FROM POLL_SURVEY_MEETING_{Poll.ExternalId} WHERE
    SUM_id_survey  = @id_survey     and 
    SUM_id_poll    = @id_poll       and
    SUM_id_meeting = @id_meeting    and
    SUM_id_person  = @id_person     and
    SUM_id_company = @id_company)
BEGIN
    INSERT INTO POLL_SURVEY_MEETING_{Poll.ExternalId} (
        SUM_id_survey,
        SUM_id_poll,
        SUM_id_meeting,
        SUM_id_person, 
        SUM_id_company
    ) 
    VALUES(
        @id_survey, 
        @id_poll,  
        @id_meeting, 
        @id_person, 
        @id_company
    )
end
    UPDATE  POLL_SURVEY_MEETING_{Poll.ExternalId}
SET
    SUM_date_mod = @date_mod,
{listColumnMeeting}
WHERE 
    SUM_id_survey  = @id_survey     and 
    SUM_id_poll    = @id_poll       and
    SUM_id_person  = @id_person     and
    SUM_id_company = @id_company;
";

            var i_ws = $@"
IF OBJECT_ID('I_WS_{Poll.ExternalId}', 'P') IS NOT NULL
DROP PROC I_WS_{Poll.ExternalId}
GO
CREATE PROCEDURE I_WS_{Poll.ExternalId}
(
    @id_survey int,
    @id_poll int,
    @id_person int,
    @id_atelier int,
    @date_mod datetime,
{listParamWs}
)
AS
SET IDENTITY_INSERT POLL_SURVEY_WS_{Poll.ExternalId} ON;
if not exists(SELECT SUB_id_person FROM POLL_SURVEY_WS_{Poll.ExternalId} WHERE
    SUB_id_survey  = @id_survey     and 
    SUB_id_poll    = @id_poll       and
    SUB_id_person  = @id_person     and
    SUB_id_atelier = @id_atelier)

BEGIN
    INSERT INTO POLL_SURVEY_WS_{Poll.ExternalId} (
        SUB_id_survey,
        SUB_id_poll,
        SUB_id_person,
        SUB_id_atelier
    ) 
    VALUES(
        @id_survey, 
        @id_poll,
        @id_person, 
        @id_atelier
    )
end
    UPDATE  POLL_SURVEY_WS_{Poll.ExternalId}
SET
    SUB_date_mod = @date_mod,
    SUB_attended = 1,
{listColumnWs}
WHERE 
    SUB_id_survey  = @id_survey     and 
    SUB_id_poll    = @id_poll       and
    SUB_id_person  = @id_person     and
    SUB_id_atelier = @id_atelier
";
            if (qCount == 0) i_general = "";
            if (meetingCount == 0) i_meeting = "";
            if (sessionCount == 0) i_session = "";
            if (wsCount == 0) i_ws = "";
            return i_general + "\nGO\n" + i_meeting + "\nGO\n" + i_session + "\nGO\n" + i_ws + "\nGo";
        }
    }
}
