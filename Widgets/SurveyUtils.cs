using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using DataAccess;
using SurveyModel;

namespace Widgets
{
    public class SurveyUtils
    {
        public static List<Question> QuestionsWebControlToQuestions(List<QuestionWebControl> ctrls)
        {
            var questions = new List<Question>();
            foreach (var ctrl in ctrls)
            {
                ctrl.Question.Answer = ctrl.Value;
                questions.Add(ctrl.Question);
            }
            return questions;
        }

        public static bool Valid(List<QuestionWebControl> questionControls, FormGenerationSettings settings)
        {
            var valid = true;
            foreach (var qc in questionControls)
            {
                if (!qc.Question.IsMandatory) continue;
                if (string.IsNullOrEmpty(qc.Value))
                {
                    qc.ErrorLabel.Text =
                        WidgetActionFactory.getWidgetActionByName(qc.Question.ControlType).MandatoryQuestionErrorMessage(settings);
                    qc.ErrorLabel.Visible = true;
                    valid = false;
                }
                else if (qc.Value.Length > qc.Question.MaxSize)
                {
                    qc.ErrorLabel.Text = settings.UserAnswerLengthErrorMessage;
                    qc.ErrorLabel.Visible = true;
                    valid = false;
                }
            }
            return valid;
        }

        public static List<QuestionWebControl> RenderForm(Poll poll, Control questions, FormGenerationSettings settings)
        {
            var questionsList = new List<QuestionWebControl>();
            var blocks = new Dictionary<int, BlockWebControl>();
            foreach (var blocksKey in poll.Blocks.Keys)
            {
                var block = new BlockWebControl(poll.Blocks[blocksKey]);
                questions.Controls.Add(block);
                blocks.Add(blocksKey, block);
            }
            foreach (var q in poll.Questions)
            {
                QuestionTypeActionFactory.getActionByName(q.Category)
                    .DisplayAction(q, poll, blocks[q.BlockNumber], questionsList);
            }
            questions.Controls.Add(new LiteralControl(generateJsValidationCode(questionsList, settings)));
            return questionsList;
        }

        public static string generateJsValidationCode(List<QuestionWebControl> qs, FormGenerationSettings settings)
        {
            var rules = "";
            var messages = "";

            foreach (var qc in qs)
            {
                qc.ClientIDMode = ClientIDMode.Static;
                var q = qc.Question;
                if (q.IsMandatory)
                {
                    var uniqueId = qc.UniqueID + "$ctl00";
                    rules += $"{uniqueId} : {{ required : true, maxlength : {q.MaxSize} }},";
                    messages += $@"{uniqueId} : {{ required : '{
                        WidgetActionFactory.getWidgetActionByName(q.ControlType).MandatoryQuestionErrorMessage(settings)
                        }', maxlength : '{settings.UserAnswerLengthErrorMessage}' }},";
                }
            }
            return $@"
<script>
    $(function(){{
        $(""#{FormGenerationSettings.SurveyFormId}"").validate({{
            errorClass: ""has-error"",
            validClass: ""has-success"",
            rules:{{
{rules}
            }},
            messages: {{
{messages}                
            }},
            errorPlacement: function(error, element) {{  
                if ( element.is("":radio"") || element.is("":checkbox"")) {{  
                    error.appendTo( element.parent().parent());  
                }}    
                else {{ // This is the default behavior   
                    error.insertAfter(element);  
                }}
            }}
        }});
    }});
</script>
";
        }
    }
}
