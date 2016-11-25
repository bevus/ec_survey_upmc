using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using SurveyModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyFormGenerator
{
    public class FormGenerator
    {
        private const string FORM_NAME = "form1";
        public string SurveysDirectory {get; set;}
        public Poll Poll { get; set; }
        public FormGenerator(string surveysDirectory, Poll Poll)
        {
            this.SurveysDirectory = surveysDirectory;
            this.Poll = Poll;
        }
        public string GenerateWebForm()
        {
            var aspxFileName = "Survey_" + Poll.ExternalId + ".aspx";
            var aspxcsFileName = "Survey_" + Poll.ExternalId + ".aspx.cs";
            var aspxdesinercsFileName = "Survey_" + Poll.ExternalId + ".aspx.designer.cs";
            var widgetFactory = new WidgetFactory();
            var aspxCode = new StringBuilder();
            aspxCode.Append(
$@"<%@ Page Language=""C#"" AutoEventWireup=""true"" CodeBehind=""{aspxcsFileName}"" Inherits=""{aspxcsFileName.Replace(".aspx.cs", "")}.{aspxcsFileName.Replace(".aspx.cs", "")}"" %>
<!DOCTYPE html> 
<html xmlns=""http://www.w3.org/1999/xhtml"">
    <head runat=""server"" > 
        <meta http-equiv = ""Content-Type"" content=""text/html; charset=utf-8""/>
        <title></title>
        <link href=""../Content/bootstrap.min.css"" rel=""stylesheet""/>
        <link href=""../Content/surveyStyle.css"" rel=""stylesheet""/>
        <script src = ""../scripts/jquery-3.1.1.min.js""></script>
        <script src = ""../scripts/bootstrap.min.js""></script>
    </head>
    <body>
        <form id=""{FORM_NAME}"" runat=""server"">
            <div class=""container"">{new Func<string>(() => {
    var blocksCode = new Dictionary<int, string>();
    foreach (var i in Poll.Blocks.Keys)
        blocksCode.Add(i, $@"
                <div class=""question-block"" id=""block-{i}"">
                    <h3 class=""question-block-title"">{Poll.Blocks[i]}</h3>
                    <div class=""question-block-questions"">");
    blocksCode.Add(-1, $@"
                    <div class=""question-block"">
                        <div class=""question-block-questions"">");
    foreach (var q in Poll.Questions)
        switch (q.ControlType)
        {
            case "TextBox":
                blocksCode[q.BlockNumber] += widgetFactory.CreateTextBox(q, "");
                break;
            case "DropDownList":
                blocksCode[q.BlockNumber] += widgetFactory.CreateDropDownList(q, "");
                break;
            case "RadioButtonList":
                blocksCode[q.BlockNumber] += widgetFactory.CreateRadioButtonList(q, "");
                break;
            case "CommentsBox":
                blocksCode[q.BlockNumber] += widgetFactory.CreateCommentsBox(q, "");
                break;
            case "CheckBoxList":
                blocksCode[q.BlockNumber] += widgetFactory.CreateCheckBoxList(q, "");
                break;
            case "DateTime":
                blocksCode[q.BlockNumber] += widgetFactory.CreateDateTime(q, "");
                break;
        }
    var code = "";
    foreach (var i in blocksCode.Keys)
    {
        code += blocksCode[i] + $@"
                        </div>
                    </div>";
    }
    return code;
})()}
                <hr/>
                <asp:Button  CausesValidation=""true"" ID=""confirm"" Text=""valider"" runat=""server"" CssClass=""btn btn-primary""/>
            </div>
        </form>
</body>
</html>
");
            var aspxdesignercsCode =
$@"

namespace {aspxcsFileName.Replace(".aspx.cs", "")}
{{
    public partial class {aspxcsFileName.Replace(".aspx.cs", "")}
    {{
            protected global::System.Web.UI.HtmlControls.HtmlForm {FORM_NAME};
            {new Func<String>(() => {
    var code = "";
    foreach (var q in Poll.Questions)
    {
        code += "\t\tprotected global::";
        switch (q.ControlType)
        {
            case "TextBox":
                code += "System.Web.UI.WebControls.TextBox ";
                break;
            case "DropDownList":
                code += "System.Web.UI.WebControls.DropDownList ";
                break;
            case "RadioButtonList":
                code += "System.Web.UI.WebControls.RadioButtonList ";
                break;
            case "CommentsBox":
                code += "System.Web.UI.WebControls.TextBox ";
                break;
            case "CheckBoxList":
                code += "System.Web.UI.WebControls.CheckBoxList ";
                break;
            case "DateTime":
                code += "System.Web.UI.WebControls.TextBox ";
                break;
        }
        code += q.ControlId + ";\n";
    }
    return code;
})()}
    }}
}}
";
            var aspxcsCode =
$@"using System;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace {aspxcsFileName.Replace(".aspx.cs", "")}
{{
    public partial class {aspxcsFileName.Replace(".aspx.cs", "")} : Page
    {{
        
        protected void Page_Load(object sender, EventArgs e)
        {{
            if (IsPostBack)
            {{
                Page.Validate();
                if (Page.IsValid)
                {{
                    var dataBaseConnection = ConnexionClasse.getConnexion();
                    dataBaseConnection.Open();
                    SqlCommand saveAnswers = new SqlCommand(""I{Poll.ExternalId}"", dataBaseConnection);
                    saveAnswers.CommandType = CommandType.StoredProcedure;
                    saveAnswers.Parameters.AddWithValue(""@person_id"", {Poll.Id});
                    {
                new Func<string>(() => {
                    var code = "";
                    foreach (var q in Poll.Questions)
                    {
                        string property = "";
                        switch (q.ControlType)
                        {
                            case "TextBox":
                            case "DateTime":
                            case "CommentsBox":
                                property = "Text";
                                break;
                            case "DropDownList":
                            case "RadioButtonList":
                            case "CheckBoxList":
                                property = "SelectedValue";
                                break;
                        }
                        code += $"saveAnswers.Parameters.AddWithValue(\"@{q.Column}\", {q.ControlId}.{property});\n";
                    }
                    return code;
                })()}
                    saveAnswers.ExecuteReader();
                    dataBaseConnection.Close();
                }}
            }}

        }}
    }}
}}";
            System.IO.File.WriteAllText(SurveysDirectory + aspxcsFileName, aspxcsCode, Encoding.UTF8);
            System.IO.File.WriteAllText(SurveysDirectory + aspxdesinercsFileName, aspxdesignercsCode, Encoding.UTF8);
            System.IO.File.WriteAllText(SurveysDirectory + aspxFileName, aspxCode.ToString(), Encoding.UTF8);
            GenerateProcedureStocke();
            return SurveysDirectory + aspxcsFileName;
        }

        private void GenerateProcedureStocke()
        {
            var procedureStocke = "Survey_" + Poll.ExternalId + ".sql";

            var content = new StringBuilder();
            content.Append(
$@"CREATE PROCEDURE [dbo].[sel{Poll.ExternalId}]
(
	@person_id int
)
AS
	SELECT * 
 	FROM Poll_SURVEY_{Poll.ExternalId}
 	WHERE id_person=@person_id 
GO

CREATE PROCEDURE [dbo].[I{Poll.ExternalId}]
(
	@person_id int,
");
            var paramsProcedure = "";
            foreach (var q in Poll.Questions)
            {

                if (q.MaxSize == 0)
                {
                    paramsProcedure += $"\t@{q.Column}  nvarchar(255),\n";
                }
                else
                {
                    paramsProcedure += $"\t@{q.Column}  nvarchar({q.MaxSize}),\n";
                }
            }
            if(paramsProcedure.Length > 2)
                paramsProcedure = paramsProcedure.Substring(0, paramsProcedure.Length - 2);
            content.Append(paramsProcedure).
                    Append(
                    $@"
)
AS
if not exists(SELECT id_person FROM Poll_SURVEY_{Poll.ExternalId}	WHERE id_person = @person_id )
BEGIN
	INSERT INTO Poll_SURVEY_{Poll.ExternalId} (id_person) VALUES(@person_id)
end
	UPDATE  [dbo].[Poll_SURVEY_{Poll.ExternalId}]
SET
");
            var tableEnry = "";
            foreach (var q in Poll.Questions)
            {
                tableEnry += $@"	[{q.Column}] = @{q.Column},
";
            }
            tableEnry = tableEnry.Substring(0, tableEnry.Length - 2);
            content.Append(tableEnry);
            content.Append(@"
WHERE id_person = @person_id 
GO
");
            content.Append(
$@"
CREATE PROCEDURE [dbo].[IS{Poll.ExternalId}]
(
	@person_id int,
	@clicked BIT,
	@clicked_date DATETIME,
	@saved BIT,
	@saved_date DATETIME,
	@comment VARCHAR(4000),
	@modified_date DATETIME,
	@page VARCHAR(4000)
)
AS
    if not exists(SELECT SUR_id_person FROM Poll_SURVEY WHERE SUR_id_person = @person_id )
BEGIN
	INSERT INTO Poll_SURVEY(SUR_id_person) VALUES(@person_id)
end
	UPDATE  [dbo].[Poll_SURVEY]
    SET
        [SUR_clicked] =  @clicked,
        [SUR_clicked_date] =  @clicked_date, 
        [SUR_saved] = @saved, 
        [SUR_saved_date] = @saved_date,
        [SUR_comments] = @comment,
        [SUR_date_mod] = @modified_date,
        [SUR_page] = @page
    WHERE SUR_id_person = @person_id 
GO
");
            System.IO.File.WriteAllText(SurveysDirectory + procedureStocke, content.ToString());
            ExecuteProcedureStock(procedureStocke);

        }

        private void ExecuteProcedureStock(String fileName)
        {
            string script = System.IO.File.ReadAllText(SurveysDirectory + fileName);
            SqlConnection connexion = ConnexionClasse.getConnexion();
            connexion.Open();
            try
            {
                Server server = new Server(new ServerConnection(connexion));
                server.ConnectionContext.ExecuteNonQuery(script);
            }
            catch (Exception)
            {

            }
            connexion.Close();
        }
    }
}
