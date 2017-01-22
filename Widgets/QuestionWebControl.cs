using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SurveyModel;

namespace Widgets
{
    [ToolboxData("<{0}:QuestionWebControl runat=server></{0}:QuestionWebControl>")]
    public class QuestionWebControl : CompositeControl
    {
        private static int _questionCount = 0;
        public Question Question { get; }

        public QuestionWebControl(Question q)
        {
            Question = q;
        }
        
        protected override void OnInit(EventArgs e)
        {
            Control = WidgetActionFactory.getWidgetActionByName(Question.ControlType).QuestionWebControlAction(Question);
            ErrorLabel = new Label
            {
                Visible = false,
                CssClass = "has-error server-control"
            };
            base.OnInit(e);
        }

        public WebControl Control { get; private set; }

        public Label ErrorLabel{ get;set; }

        public string Value => WidgetActionFactory.getWidgetActionByName(Question.ControlType).GetValueAction(this);

        protected override void CreateChildControls()
        {
            Controls.Add(Control);
            Controls.Add(ErrorLabel);
        }

        protected override void Render(HtmlTextWriter output)
        {
            output.Write(
$@"<div class=""row question"">
     <div class="" form-group"">
        <Label for="""" class=""control-label col-md-12"">
            <span class=""question-number"">
                {(Question.GetType() == typeof(SubQuestion) ? "" : Question.Number)}
            </span>
            <span class=""question-text"">
                {Question.Label}
            </span>
            <br/>
        </Label>
            <div class=""col-md-10"">");
            Control.CssClass = "QuestionControl " + Question.ControlType + " " + (Question.IsMandatory ? "Mandatory" : "");
            Control.Attributes.Add("data-max", Question.MaxSize.ToString());
            Control.Attributes.Add("data-nbChoices", Question.ChoiceCount.ToString());
            Control.RenderControl(output);
            ErrorLabel?.RenderControl(output);
            output.Write(
@"          </div>
    </div>
</div>"
);
        }
    }
}
