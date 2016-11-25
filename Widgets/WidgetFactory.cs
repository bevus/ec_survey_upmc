using SurveyModel;
using System;
using System.Linq;

namespace SurveyFormGenerator
{
    public class WidgetFactory
    {
        private const string RequiredValidatorErrorMessage = "Vous devez donnez une reponse à cette question";
        private const string RangeValidatorErrorMessage = "Vous avez depassez le nombre maximal de caractères pour ce champ";

        public string CreateTextBox(Question q, String value)
        {
            var code =$@"
<asp:TextBox CssClass=""form-control"" ID=""{q.ControlId}"" Text=""{value}"" runat=""server""></asp:TextBox>";
            return AddBootsrapLayout(QuestionMarkup(q), code, q);
        }

        public string CreateCommentsBox(Question q, String value)
        {
            var code =$@"
<asp:TextBox ID=""{q.ControlId}"" CssClass=""form-control"" runat=""server"" Text=""{value}"" Rows=""5"" Columns=""70"" TextMode=""MultiLine""></asp:TextBox>
";
            return AddBootsrapLayout(QuestionMarkup(q), code, q);
        }
        public  string CreateDropDownList(Question q, String value)
        {
            var code =$@"
<asp:DropDownList CssClass=""form-control"" ID=""{q.ControlId}"" RepeatLayout=""Flow"" RepeatDirection=""Horizontal"" TextAlign=""Left"" runat=""server"">
    {q.Choices.Aggregate("", (current, choice) => current + $"<asp:ListItem Selected=\"{value.Equals(choice.Value)}\" Value=\"{choice.Value}\">{choice.Label}</asp:ListItem>\n")}
</asp:DropDownList>";
            return AddBootsrapLayout(QuestionMarkup(q), code, q);
        }
        public  string CreateRadioButtonList(Question q, String value)
        {
            var code =$@"
<asp:RadioButtonList TextAlign=""Left"" ID=""{q.ControlId}"" RepeatLayout=""Flow"" RepeatDirection=""Horizontal"" runat=""server"">
    {q.Choices.Aggregate("", (current, choice) => current + $"<asp:ListItem Selected=\"{value.Equals(choice.Value)}\" Value=\"{choice.Value}\">{choice.Label}</asp:ListItem>\n")}
</asp:RadioButtonList>";
            return AddBootsrapLayout(QuestionMarkup(q), code, q);
        }
        public string CreateCheckBoxList(Question q, String value)
        {
            var code =$@"
<asp:CheckBoxList TextAlign=""Left"" ID=""{q.ControlId}"" RepeatLayout=""Flow"" RepeatDirection=""Horizontal"" runat=""server"">
    {q.Choices.Aggregate("", (current, choice) => current + $"<asp:ListItem Selected=\"{value.Equals(choice.Value)}\" Value=\"{choice.Value}\">{choice.Label}</asp:ListItem>\n")}
</asp:CheckBoxList>";
            return AddBootsrapLayout(QuestionMarkup(q), code, q);
        }
        public string CreateDateTime(Question q, string value)
        {
            var code =$@"
<asp:TextBox ID=""{q.ControlId}"" Text=""{value}"" TextMode=""DateTime"" runat=""server""></asp:TextBox>";
            return AddBootsrapLayout(QuestionMarkup(q), code, q);
        }
        private string QuestionMarkup(Question q)
        {
            return $@"
<span class=""question-number"">{q.Number}</span><span class=""question-text"">{q.Label}</span>";
        }
        private  string AddRequiredFieldValidator(Question q)
        {
            if (q.IsMandatory)
                return $@"
<asp:RequiredFieldValidator runat=""server"" ID=""rfv_{q.ControlId}"" ErrorMessage=""{RequiredValidatorErrorMessage}"" ControlToValidate=""{q.ControlId}"" ></asp:RequiredFieldValidator><br/>";
            return "";
        }
        private string AddRangeFieldValidator(Question q)
        {
            return "";
            if(q.MaxSize > 0)
                return $@"
<br/><asp:RegularExpressionValidator ValidationExpression=""^[a-zA-Z0-9'@&#.\s]{{1, {q.MaxSize}}}$"" runat =""server"" ID=""rv_{q.ControlId}"" ErrorMessage=""{RangeValidatorErrorMessage}"" ControlToValidate=""{q.ControlId}"" ></asp:RegularExpressionValidator ><br/>";
            return "";
        }

        private string AddBootsrapLayout(string label, string control, Question q)
        {
            return $@"<div class=""row question"">
                    <div class="" form-group"">
                        <Label for="""" class=""control-label col-md-12"">
                            {label}
                        </Label>
                        <div class=""col-md-10"">
                            {control}
                            {AddRequiredFieldValidator(q)}
                            {AddRangeFieldValidator(q)}
                        </div>
                    </div>
                </div>";
        }
    }
}
