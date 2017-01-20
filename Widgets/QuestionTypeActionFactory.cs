using SurveyFormGenerator;
using SurveyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;

namespace Widgets
{
    public class QuestionTypeActionFactory
    {
        private static readonly Dictionary<string, Func<IQuestionTypeAction>> ActionMap = new Dictionary<string, Func<IQuestionTypeAction>>()
        {
            { QuestionType.General, () => new GeneralQuestionAction() },
            { QuestionType.Session, () => new SessionQuestionAction() },
            { QuestionType.Meeting, () => new MeetingQuestionAction() },
            { QuestionType.Workshop, () => new WorkshopQuestionAction() }
        };

        public static IQuestionTypeAction getActionByName(string actionName)
        {
            return ActionMap[actionName]();
        }
        public static string QuestionMarkup(Question q)
        {
            return $"<span class=\"question-text\">{q.Label}</span><br/>\n";
        }
        public static string SubBlockTitleMarkup(string title)
        {
            return $@"<span class=""sub-block-title"">{title}</span>";
        }
    }
    public interface IQuestionTypeAction
    {
        void DisplayAction(SurveyModel.Question q, Poll p, Control container, List<QuestionWebControl> ctls);
        string aspxCsAction(SurveyModel.Question q, Poll poll);
        string aspxDesignerCsAction(SurveyModel.Question q, Poll poll);
        string getTableName(Poll p);
        string getAnswerComlunName(Question q);
    }

    public class GeneralQuestionAction : IQuestionTypeAction
    {
        public void DisplayAction(SurveyModel.Question q, Poll poll, Control container, List<QuestionWebControl> ctls)
        {
            var manager = new Manager();
            q.Answer = manager.getAnswer(poll.Id, poll.TableName, q.Category,
                QuestionTypeActionFactory.getActionByName(q.Category).getAnswerComlunName(q), poll.PersonId);
            var qc = new QuestionWebControl(q);
            ctls.Add(qc);
            container.Controls.Add(qc);
        }

        public string aspxCsAction(SurveyModel.Question q, Poll poll)
        {
            throw new NotImplementedException();
        }

        public string aspxDesignerCsAction(SurveyModel.Question q, Poll poll)
        {
            throw new NotImplementedException();
        }

        public string getTableName(Poll p)
        {
            return p.TableName;
        }

        public string getAnswerComlunName(Question q)
        {
            return q.Column;
        }
    }

    public class MeetingQuestionAction : IQuestionTypeAction
    {
        public void DisplayAction(SurveyModel.Question q, Poll poll, Control container, List<QuestionWebControl> ctls)
        {
            if(poll.Meetings.Count == 0)
                return;
            container.Controls.Add(new LiteralControl(QuestionTypeActionFactory.QuestionMarkup(q)));
            foreach (var m in poll.Meetings)
            {
                container.Controls.Add(new LiteralControl($@"<span class=""sub-block-title"">
                    Meeting from {m.date_start} to {m.date_end} 
                    </span>"));
                foreach (var sq in q.SubQuestions)
                {
                    var csq = (SubQuestion)sq.Clone();
                    var manager = new Manager();
                    csq.Answer = manager.getAnswer(poll.Id, poll.TableMeetingName, csq.Category,
                        QuestionTypeActionFactory.getActionByName(csq.Category).getAnswerComlunName(csq),
                        poll.PersonId, m.id_meeting);
                    csq.Activity = m;
                    var qc = new QuestionWebControl(csq);
                    ctls.Add(qc);
                    container.Controls.Add(qc);
                }
            }
            container.Controls.Add(new LiteralControl("<br/>"));
        }

        public string aspxCsAction(SurveyModel.Question q, Poll poll)
        {
            throw new NotImplementedException();
        }

        public string aspxDesignerCsAction(SurveyModel.Question q, Poll poll)
        {
            throw new NotImplementedException();
        }

        public string getTableName(Poll p)
        {
            return p.TableMeetingName;
        }

        public string getAnswerComlunName(Question q)
        {
            return "SUM_" + q.Column;
        }
    }

    public class SessionQuestionAction : IQuestionTypeAction
    {
        public void DisplayAction(Question q, Poll poll, Control container, List<QuestionWebControl> ctls)
        {
            if (poll.Sessions.Count == 0)
                return;
            container.Controls.Add(new LiteralControl(QuestionTypeActionFactory.QuestionMarkup(q)));
            foreach (var m in poll.Sessions)
            {
                container.Controls.Add(new LiteralControl(QuestionTypeActionFactory.SubBlockTitleMarkup(m.theme)));
                foreach (var sq in q.SubQuestions)
                {
                    var csq = (SubQuestion)sq.Clone();
                    var manager = new Manager();
                    csq.Answer = manager.getAnswer(poll.Id, poll.TableSessionName, csq.Category,
                        QuestionTypeActionFactory.getActionByName(csq.Category).getAnswerComlunName(csq),
                        poll.PersonId, m.id_atelier);
                    csq.Activity = m;
                    var qc = new QuestionWebControl(csq);
                    ctls.Add(qc);
                    container.Controls.Add(qc);
                }
            }
            container.Controls.Add(new LiteralControl("<br/>"));
        }

        public string aspxCsAction(Question q, Poll poll)
        {
            throw new NotImplementedException();
        }

        public string aspxDesignerCsAction(Question q, Poll poll)
        {
            throw new NotImplementedException();
        }

        public string getTableName(Poll p)
        {
            return p.TableSessionName;
        }

        public string getAnswerComlunName(Question q)
        {
            return "SUB_" + q.Column;
        }
    }

    public class WorkshopQuestionAction : IQuestionTypeAction
    {
        public void DisplayAction(Question q, Poll poll, Control container, List<QuestionWebControl> ctls)
        {
            if (poll.Workshops.Count == 0)
                return;
            container.Controls.Add(new LiteralControl(QuestionTypeActionFactory.QuestionMarkup(q)));
            foreach (var m in poll.Workshops)
            {
                container.Controls.Add(new LiteralControl(QuestionTypeActionFactory.SubBlockTitleMarkup(m.theme)));
                foreach (var sq in q.SubQuestions)
                {
                    var csq = (SubQuestion)sq.Clone();
                    var manager = new Manager();
                    csq.Answer = manager.getAnswer(poll.Id, poll.TableWsName, csq.Category,
                        QuestionTypeActionFactory.getActionByName(csq.Category).getAnswerComlunName(csq),
                        poll.PersonId, m.id_atelier);
                    csq.Activity = m;
                    var qc = new QuestionWebControl(csq);
                    ctls.Add(qc);
                    container.Controls.Add(qc);
                }
            }
            container.Controls.Add(new LiteralControl("<br/>"));
        }

        public string aspxCsAction(Question q, Poll poll)
        {
            throw new NotImplementedException();
        }

        public string aspxDesignerCsAction(Question q, Poll poll)
        {
            throw new NotImplementedException();
        }

        public string getTableName(Poll p)
        {
            return p.TableWsName;
        }

        public string getAnswerComlunName(Question q)
        {
            return "SUB_" + q.Column;
        }
    }
}
