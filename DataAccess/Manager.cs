using SurveyModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class Manager
    {
        public Manager()
        {
            
        }

        public static SqlDataReader ExecuteStoredProcedure(SqlConnection connextion, string procedureName,
            Dictionary<string, object> arguments)
        {
            var commande = new SqlCommand(procedureName, connextion) {CommandType = CommandType.StoredProcedure};
            foreach (var arg in arguments.Keys)
            {
                commande.Parameters.AddWithValue(arg, arguments[arg]);
            }
            var result = commande.ExecuteReader();
            return result;
        }

        public Poll getPoll(int idPoll, int idPerson = 0)
        {
            var poll = new Poll {PersonId = idPerson};
            var dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();

            var result = ExecuteStoredProcedure(dataBaseConnection, "sel_poll", new Dictionary<string, object>()
            {
                {"@poll_id", idPoll}
            });
            while (result.Read())
            {
                poll.Id = idPoll;
                try
                {
                    poll.Name = result["POL_name"].ToString();
                    poll.Description = result["POL_description"].ToString();
                }
                catch (Exception)
                {
                }
                poll.ExternalId = result["POL_external_id"].ToString();
                poll.TableName = result["POL_table_name"].ToString();
                poll.TableMeetingName = result["POL_table_meeting_name"].ToString();
                poll.TableWsName = result["POL_table_ws_name"].ToString();
                poll.TableSessionName = result["POL_table_session_name"].ToString();
                try
                {
                    poll.SurveyId = int.Parse(result["POL_id_poll"].ToString());
                }
                catch (Exception)
                {
                }
                try
                {
                    poll.EventId = int.Parse(result["POL_id_event"].ToString());
                }
                catch (Exception)
                {
                    throw new Exception("no event id in poll table");
                }
            }
            dataBaseConnection.Close();
            poll.Questions = this.getQuestions(poll.Id);
            poll.Questions.Sort((q1, q2) => q1.Order.CompareTo(q2.Order));
            poll.Meetings = getMeetings(poll.Id, idPerson);
            poll.Sessions = getSessions(poll.Id, idPerson);
            poll.Workshops = getWorkshops(poll.Id, idPerson);
            poll.Blocks = getBlocks(poll.Id);
            return poll;
        }

        public List<Question> getQuestions(int pollId)
        {
            List<Question> questions = new List<Question>();
            var dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            var result = ExecuteStoredProcedure(dataBaseConnection, "sel_questions", new Dictionary<string, object>()
            {
                {"@poll_id", pollId}
            });
            while (result.Read())
            {
                Question q = new Question();
                try
                {
                    q.BlockNumber = Int32.Parse(result["QUE_id_block"].ToString());
                }
                catch (Exception e)
                {
                    throw new Exception("QUE_id_block is not an int");
                }
                try
                {
                    q.Category = result["QUET_question_type"].ToString();
                }
                catch (Exception)
                {
                    throw new Exception("Question type unknown");
                }
                mapQuestion(result, q);
                questions.Add(q);
            }
            dataBaseConnection.Close();
            foreach (Question q in questions)
            {
                switch (q.Category)
                {
                    case QuestionType.Session:
                    case QuestionType.Meeting:
                    case QuestionType.Workshop:
                        q.SubQuestions = getSubQuestions(q);
                        break;
                }
                q.Choices = getChoices(q);
            }
            return questions;
        }

        public Dictionary<int, string> getBlocks(int pollId)
        {
            var blocks = new Dictionary<int, string>();
            var dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            var result = ExecuteStoredProcedure(dataBaseConnection, "sel_blocks", new Dictionary<string, object>()
            {
                {"@poll_id", pollId}
            });
            while (result.Read())
            {
                blocks.Add(Int32.Parse(result["POLB_id_block"].ToString()), result["POLB_label"].ToString());
            }
            dataBaseConnection.Close();
            return blocks;
        }

        private List<Choice> getChoices(Question q)
        {
            var choices = new List<Choice>();
            var dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            SqlDataReader result = null;
            SqlCommand getQuestionsChoices;
            if (q.GetType() == typeof(SubQuestion))
            {
                result = ExecuteStoredProcedure(dataBaseConnection, "sel_sub_questions_choices",
                    new Dictionary<string, object>()
                    {
                        {"@question_id", q.Id},
                        {"@question_type", q.Category}
                    });
            }
            else
            {
                result = ExecuteStoredProcedure(dataBaseConnection, "sel_choices", new Dictionary<string, object>()
                {
                    {"@question_id", q.Id}
                });
            }

            while (result.Read())
            {
                var r = new Choice();
                try
                {
                    r.Id = Int32.Parse(result["choice_id"].ToString());
                    r.Label = result["choice_label"].ToString();
                    r.Id = Int32.Parse(result["choice_order"].ToString());
                    r.Value = result["choice_value"].ToString();
                }
                catch (Exception e)
                {
                }
                choices.Add(r);
            }
            dataBaseConnection.Close();
            return choices;
        }

        public List<SubQuestion> getSubQuestions(Question parentQuestion)
        {
            var questions = new List<SubQuestion>();
            var dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            var result = ExecuteStoredProcedure(dataBaseConnection, "sel_sub_questions",
                new Dictionary<string, object>()
                {
                    {"@question_id", parentQuestion.Id},
                    {"@question_type", parentQuestion.Category}
                });
            while (result.Read())
            {
                SubQuestion q = new SubQuestion();
                q.ParentQuestion = parentQuestion;
                q.Category = parentQuestion.Category;
                try
                {
                    mapQuestion(result, q);
                }
                catch (Exception e)
                {
                }
                questions.Add(q);
            }
            dataBaseConnection.Close();
            foreach (SubQuestion q in questions)
            {
                q.Choices = getChoices(q);
            }
            return questions;
        }

        private void mapQuestion(SqlDataReader result, Question q)
        {
            q.Answer = "";
            try
            {
                q.Id = Int32.Parse(result["question_id"].ToString());
            }
            catch (Exception e)
            {
                throw new Exception("id_question invalide");
            }
            q.Label = result["question_label"].ToString();
            q.Description = result["question_description"].ToString();
            try
            {
                q.Order = Int32.Parse(result["question_order"].ToString());
            }
            catch (Exception e)
            {
                throw new Exception("order question invalide");
            }
            try
            {
                q.MaxSize = Int32.Parse(result["question_max_size"].ToString());
            }
            catch (Exception)
            {
                q.MaxSize = 255;
            }
            q.Column = result["question_column"].ToString();
            q.Number = result["question_number"].ToString();
            try
            {
                q.IsMandatory = Boolean.Parse(result["question_mandatory"].ToString());
            }
            catch (Exception)
            {
                q.IsMandatory = false;
            }
            try
            {
                q.ChoiceCount = Int32.Parse(result["question_nb_choice"].ToString());
            }
            catch (Exception)
            {
                q.ChoiceCount = 0;
            }
            q.ControlType = result["CHOT_choice_type"].ToString();
            q.Prefix = result["CHOT_prefix"].ToString().Trim();
        }

        public List<Meeting> getMeetings(int eventId, int personId)
        {
            var meetings = new List<Meeting>();
            var dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            var result = ExecuteStoredProcedure(dataBaseConnection, "sel_meetings", new Dictionary<string, object>()
            {
                {"@id_person", personId},
                {"@id_event", eventId}
            });
            while (result.Read())
            {
                Meeting m = new Meeting();
                m.company_name = result["COM_company"].ToString();
                // owner
                // guests
                try
                {
                    m.id_company = int.Parse(result["MEE_id_company"].ToString());
                }
                catch (Exception)
                {
                }
                try
                {
                    m.date_start = Convert.ToDateTime(result["TIM_date_start"]);
                }
                catch (Exception)
                {
                }
                try
                {
                    m.id_meeting = int.Parse(result["MEE_id_meeting"].ToString());
                }
                catch (Exception)
                {
                }
                try
                {
                    m.date_end = Convert.ToDateTime(result["TIM_date_end"]);
                }
                catch (Exception)
                {
                }
                m.location_name = result["LOC_location"].ToString();
                meetings.Add(m);
            }
            dataBaseConnection.Close();
            return meetings;
        }

        public List<Session> getSessions(int eventId, int personId)
        {
            var sessions = new List<Session>();
            var dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            var result = ExecuteStoredProcedure(dataBaseConnection, "sel_sessions", new Dictionary<string, object>()
            {
                {"@id_person", personId},
                {"@id_event", eventId}
            });
            while (result.Read())
            {
                Session s = new Session();
                try
                {
                    s.etat = Int32.Parse(result["etat"].ToString());
                }
                catch (Exception)
                {
                }
                try
                {
                    s.id_atelier = Int32.Parse(result["id_atelier"].ToString());
                }
                catch (Exception)
                {
                    throw new Exception("id atelier not found");
                }
                try
                {
                    s.attended_date = Convert.ToDateTime(result["attended_date"]);
                }
                catch (Exception)
                {
                }
                try
                {
                    s.creation_date = Convert.ToDateTime(result["creation_date"]);
                }
                catch (Exception)
                {
                }
                s.theme = result["theme"].ToString();
                s.description = result["description"].ToString();
                sessions.Add(s);
            }
            dataBaseConnection.Close();
            return sessions;
        }

        public List<Workshop> getWorkshops(int eventId, int personId)
        {
            var workshps = new List<Workshop>();
            var dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            var result = ExecuteStoredProcedure(dataBaseConnection, "sel_workshops", new Dictionary<string, object>()
            {
                {"@id_person", personId},
                {"@id_event", eventId}
            });
            while (result.Read())
            {
                Workshop ws = new Workshop();
                try
                {
                    ws.etat = Int32.Parse(result["etat"].ToString());
                }
                catch (Exception)
                {
                }
                try
                {
                    ws.id_atelier = Int32.Parse(result["id_atelier"].ToString());
                }
                catch (Exception)
                {
                    throw new Exception("id atelier not found");
                }
                try
                {
                    ws.attended_date = Convert.ToDateTime(result["attended_date"]);
                }
                /*catch (Exception) { }
                try
                {
                    ws.creation_date = Convert.ToDateTime(result["creation_date"]);
                }*/
                catch (Exception)
                {
                }
                ws.theme = result["theme"].ToString();
                ws.description = result["description"].ToString();
                workshps.Add(ws);
            }
            dataBaseConnection.Close();
            return workshps;
        }

        public string getAnswer(int pollId, string tableName, string category, string column, int personId,
            int activityId = 0)
        {
            var dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            var result = ExecuteStoredProcedure(dataBaseConnection, "get_rep_person", new Dictionary<string, object>()
            {
                {"@poll_id", pollId},
                {"@person_id", personId},
                {"@table", tableName},
                {"@activity_id", activityId},
                {"@question_type", category},
                {"@column_name", column}
            });
            result.Read();
            var tmp = "";
            try
            {
                tmp = result[column].ToString();
            }
            catch (Exception)
            {
            }
            dataBaseConnection.Close();
            return tmp;
        }

        public static void SaveAnswer(
            List<Question> questions,
            Poll poll,
            string externalId,
            int personId,
            DateTime time
        )
        {
            var dataBaseConnection = ConnexionClasse.getConnexion();
            var meetingCommands = new Dictionary<int, SqlCommand>();
            var sessionCommands = new Dictionary<int, SqlCommand>();
            var workshopCommands = new Dictionary<int, SqlCommand>();

            foreach (var meeting in poll.Meetings)
            {
                var sqlc = new SqlCommand("I_MEETING_" + externalId, dataBaseConnection);
                sqlc.CommandType = CommandType.StoredProcedure;
                sqlc.Parameters.AddWithValue("@id_survey", poll.SurveyId);
                sqlc.Parameters.AddWithValue("@id_poll", poll.Id);
                sqlc.Parameters.AddWithValue("@id_meeting", meeting.id_meeting);
                sqlc.Parameters.AddWithValue("@id_person", personId);
                sqlc.Parameters.AddWithValue("@id_company", meeting.id_company);
                sqlc.Parameters.AddWithValue("@date_mod", time);
                meetingCommands.Add(meeting.id_meeting, sqlc);
            }
            foreach (var session in poll.Sessions)
            {
                var sqlc = new SqlCommand("I_SESSION_" + externalId, dataBaseConnection);
                sqlc.CommandType = CommandType.StoredProcedure;
                sqlc.Parameters.AddWithValue("@id_survey", poll.SurveyId);
                sqlc.Parameters.AddWithValue("@id_poll", poll.Id);
                sqlc.Parameters.AddWithValue("@id_person", personId);
                sqlc.Parameters.AddWithValue("@id_atelier", session.id_atelier);
                sqlc.Parameters.AddWithValue("@date_mod", time);
                sessionCommands.Add(session.id_atelier, sqlc);
            }
            foreach (var workshop in poll.Workshops)
            {
                var sqlc = new SqlCommand("I_WS_" + externalId, dataBaseConnection);
                sqlc.CommandType = CommandType.StoredProcedure;
                sqlc.Parameters.AddWithValue("@id_survey", poll.SurveyId);
                sqlc.Parameters.AddWithValue("@id_poll", poll.Id);
                sqlc.Parameters.AddWithValue("@id_person", personId);
                sqlc.Parameters.AddWithValue("@id_atelier", workshop.id_atelier);
                sqlc.Parameters.AddWithValue("@date_mod", time);
                workshopCommands.Add(workshop.id_atelier, sqlc);
            }
            var generalCommand = new SqlCommand("I" + externalId, dataBaseConnection);
            generalCommand.CommandType = CommandType.StoredProcedure;
            generalCommand.Parameters.AddWithValue("@person_id", personId);

            foreach (var q in questions)
            {
                switch (q.Category)
                {
                    case QuestionType.General:
                        generalCommand.Parameters.AddWithValue("@" + q.Column, q.Answer);
                        break;
                    case QuestionType.Meeting:
                        var m = (Meeting) ((SubQuestion) q).Activity;
                        meetingCommands[m.id_meeting].Parameters.AddWithValue("@" + q.Column, q.Answer);
                        break;
                    case QuestionType.Session:
                        var s = (Session) ((SubQuestion) q).Activity;
                        sessionCommands[s.id_atelier].Parameters.AddWithValue("@" + q.Column, q.Answer);
                        break;
                    case QuestionType.Workshop:
                        var ws = (Workshop) ((SubQuestion) q).Activity;
                        workshopCommands[ws.id_atelier].Parameters.AddWithValue("@" + q.Column, q.Answer);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            foreach (var key in meetingCommands.Keys)
            {
                dataBaseConnection.Open();
                meetingCommands[key].ExecuteReader();
                dataBaseConnection.Close();
            }
            foreach (var key in sessionCommands.Keys)
            {
                dataBaseConnection.Open();
                sessionCommands[key].ExecuteReader();
                dataBaseConnection.Close();
            }
            foreach (var key in workshopCommands.Keys)
            {
                dataBaseConnection.Open();
                workshopCommands[key].ExecuteReader();
                dataBaseConnection.Close();
            }
            dataBaseConnection.Open();
            generalCommand.ExecuteReader();
            dataBaseConnection.Close();
        }

        public static void SaveInPollSurvey(int poll_id, int person_id, DateTime time)
        {
            var dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            ExecuteStoredProcedure(dataBaseConnection, "i_poll_survey", new Dictionary<string, object>()
            {
                {"@poll_id", poll_id},
                {"@person_id", person_id},
                {"@saved_date", time}
            });
            dataBaseConnection.Close();
        }

        public static bool AlreadyAnswerd(int poll_id, int person_id)
        {
            var dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            var result = Manager.ExecuteStoredProcedure(dataBaseConnection, "sel_poll_survey",
                new Dictionary<string, object>()
                {
                    {"@poll_id", poll_id},
                    {"@person_id", person_id}
                });
            var count = 0;
            while (result.Read())
            {
                try
                {
                    count = int.Parse(result["c"].ToString());
                }
                catch (Exception)
                {
                }
            }
            dataBaseConnection.Close();
            return count > 0;
        }

        public static bool ExistPerson(int person_id)
        {
            var dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            var result = ExecuteStoredProcedure(dataBaseConnection, "sel_person", new Dictionary<string, object>()
            {
                {"@person_id", person_id}
            });
            var count = 0;
            while (result.Read())
            {
                try
                {
                    count = int.Parse(result["c"].ToString());
                }
                catch (Exception)
                {
                }
            }
            dataBaseConnection.Close();
            return count > 0;
        }

        public static int GetHashedId(string hash, int pollId)
        {
            var dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            var result = ExecuteStoredProcedure(dataBaseConnection, "get_hased_id", new Dictionary<string, object>()
            {
                {"@hash", hash},
                {"@poll_id", pollId}
            });
            var personId = -1;
            if (result.Read())
            {
                try
                {
                    personId = int.Parse(result["person_id"].ToString());
                }
                catch (Exception) {}
            }
            dataBaseConnection.Close();
            return personId;
        }
    }
}
