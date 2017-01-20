using SurveyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Widgets
{
    public class WidgetActionFactory
    {
        internal static int MinTextSize = 10;
        private static Dictionary<string, Func<IWidgetAction>> actionMap = new Dictionary<string, Func<IWidgetAction>>()
        {
            { "TextBox", () => new TextBoxAction() },
            { "DropDownList", () => new DropDownListAction() },
            { "RadioButtonList", () => new RadioButtonListAction() },
            { "CommentsBox", () => new CommentsBoxAction() },
            { "CheckBoxList", () => new CheckBoxListAction() },
            { "DateTime", () => new DateTimeAction() }
        };

        public static IWidgetAction getWidgetActionByName(string actionName)
        {
            return actionMap[actionName]();
        }
    }
    public interface IWidgetAction
    {
        string GetValueAction(QuestionWebControl qwc);
        WebControl QuestionWebControlAction(Question q);
        string MandatoryQuestionErrorMessage(FormGenerationSettings settings);
    }

    public class TextBoxAction : IWidgetAction
    {
        public string GetValueAction(QuestionWebControl qwc)
        {
            var tb = (TextBox) qwc.Control;
            return tb?.Text;
        }

        public WebControl QuestionWebControlAction(Question q)
        {
            var tb = new TextBox
            {
                CssClass = "form-control",
                Text = q.Answer
            };
            return tb;
        }

        public string MandatoryQuestionErrorMessage(FormGenerationSettings settings)
        {
            return settings.UserTextQuestionErrorMessage;
        }
    }

    public class DropDownListAction : IWidgetAction
    {

        public string GetValueAction(QuestionWebControl qwc)
        {
            var ddl = (DropDownList) qwc.Control;
            return ddl?.SelectedValue;
        }

        public WebControl QuestionWebControlAction(Question q)
        {
            var ddl = new DropDownList()
            {
                CssClass = "from-control"
            };
            foreach (var qChoice in q.Choices)
            {
                var li = new ListItem
                {
                    Text = qChoice.Label,
                    Value = qChoice.Value,
                    Selected = qChoice.Value.Equals(q.Answer)
                };
                ddl.Items.Add(li);
            }
            return ddl;
        }

        public string MandatoryQuestionErrorMessage(FormGenerationSettings settings)
        {
            return settings.UserOptionQuestionErrorMessage;
        }
    }

    public class RadioButtonListAction : IWidgetAction
    {

        public string GetValueAction(QuestionWebControl qwc)
        {
            var rbl = (RadioButtonList)qwc.Control;
            return rbl?.SelectedValue;
        }

        public WebControl QuestionWebControlAction(Question q)
        {
            var rbl = new RadioButtonList
            {
                TextAlign = TextAlign.Right,
                RepeatLayout = RepeatLayout.Flow,
                RepeatDirection = RepeatDirection.Horizontal
            };
            
            foreach (var qChoice in q.Choices)
            {
                var li = new ListItem
                {
                    Text = qChoice.Label,
                    Value = qChoice.Value,
                    Selected = qChoice.Value.Equals(q.Answer)
                };
                rbl.Items.Add(li);
            }
            return rbl;
        }

        public string MandatoryQuestionErrorMessage(FormGenerationSettings settings)
        {
            return settings.UserOptionQuestionErrorMessage;
        }
    }

    public class CommentsBoxAction : IWidgetAction
    {
        public string GetValueAction(QuestionWebControl qwc)
        {
            //return $"saveAnswers.Parameters.AddWithValue(\"@{q.Column}\", {q.ControlId}.Text);\n";
            var cb = (TextBox)qwc.Control;
            return cb?.Text;
        }

        public WebControl QuestionWebControlAction(Question q)
        {
            var cb = new TextBox()
            {
                CssClass = "from-control",
                Text = q.Answer,
                Rows = 5,
                Columns = 70,
                TextMode = TextBoxMode.MultiLine
            };
            return cb;
        }

        public string MandatoryQuestionErrorMessage(FormGenerationSettings settings)
        {
            return settings.UserTextQuestionErrorMessage;
        }
    }
    public class CheckBoxListAction : IWidgetAction
    {

        public string GetValueAction(QuestionWebControl qwc)
        {
            var cbl = (CheckBoxList) qwc.Control;
            if (cbl == null) return null;
            var selectedValues = new List<string>();
                foreach (ListItem i in cbl.Items)
                    if (i.Selected)
                        selectedValues.Add(i.Text); 
            return string.Join(";", selectedValues);
        }

        public WebControl QuestionWebControlAction(Question q)
        {
            var cbl = new CheckBoxList
            {
                TextAlign = TextAlign.Right,
                RepeatLayout = RepeatLayout.Flow,
                RepeatDirection = RepeatDirection.Horizontal
            };
            var selectedItems = q.Answer.Split(';');
            foreach (var qChoice in q.Choices)
            {
                var li = new ListItem
                {
                    Text = qChoice.Label,
                    Value = qChoice.Value,
                    Selected = selectedItems.Contains(qChoice.Value)
                };
                cbl.Items.Add(li);
            }
            return cbl;
        }

        public string MandatoryQuestionErrorMessage(FormGenerationSettings settings)
        {
            return settings.UserOptionQuestionErrorMessage;
        }
    }

    public class DateTimeAction : IWidgetAction
    {

        public string GetValueAction(QuestionWebControl qwc)
        {
            var dt = (TextBox)qwc.Control;
            return dt?.Text;
        }

        public WebControl QuestionWebControlAction(Question q)
        {
            var dt = new TextBox
            {
                Text = q.Answer,
                TextMode = TextBoxMode.DateTime
            };
            return dt;
        }

        public string MandatoryQuestionErrorMessage(FormGenerationSettings settings)
        {
            return settings.UserDateTimeQuestionErrorMessage;
        }
    }
}
