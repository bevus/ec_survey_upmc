using SurveyModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Manager
    {
        private SqlConnection dataBaseConnection;
        public Manager()
        {
            dataBaseConnection = ConnexionClasse.getConnexion();
        }
        public Poll getPoll(int idPoll)
        {
            Poll poll = new Poll();
            dataBaseConnection.Open();
            SqlCommand getPollQuery = new SqlCommand("sel_poll", dataBaseConnection);
            getPollQuery.CommandType = CommandType.StoredProcedure;
            getPollQuery.Parameters.AddWithValue("@poll_id", idPoll);
            var result = getPollQuery.ExecuteReader();
            while (result.Read())
            {
                poll.Id = idPoll;
                try
                {
                    poll.Name = result["POL_name"].ToString();
                    poll.Description = result["POL_description"].ToString();
                }
                catch(Exception e)
                {}
                poll.ExternalId = result["POL_external_id"].ToString();
                poll.TableName = result["POL_table_name"].ToString();
                poll.TableMeetingName = result["POL_table_meeting_name"].ToString();
                poll.TableWsName = result["POL_table_ws_name"].ToString();
                poll.TableSessionName = result["POL_table_session_name"].ToString();
            }
            dataBaseConnection.Close();
            poll.Questions = this.getQuestions(poll.Id);
            poll.Questions.Sort((q1, q2) => q1.Order.CompareTo(q2.Order));
            poll.Blocks = getBlocks(poll.Id);
            return poll;
        }
        public List<Question> getQuestions(int pollId)
        {
            List<Question> questions = new List<Question>();
            dataBaseConnection.Open();
            SqlCommand getQuestions = new SqlCommand("sel_questions", dataBaseConnection);
            getQuestions.CommandType = CommandType.StoredProcedure;
            getQuestions.Parameters.AddWithValue("@poll_id", pollId);

            var result = getQuestions.ExecuteReader();
            while (result.Read())
            {
                Question q = new Question();
                q.Id = Int32.Parse(result["QUE_id_question"].ToString());
                q.Label = result["QUE_label"].ToString();
                try
                {
                    q.Description = result["QUE_description"].ToString();
                }
                catch (Exception e) { }
                try
                {
                    q.Order = Int32.Parse(result["QUE_order"].ToString());
                }
                catch (Exception e) { }
                try
                {
                    q.MaxSize = Int32.Parse(result["QUE_max_size"].ToString());
                }
                catch (Exception e) { }
                try
                {
                    q.PageNumber = Int32.Parse(result["QUE_page_number"].ToString());
                }
                catch (Exception e) { }
                try
                {
                    q.Column = result["QUE_column"].ToString();
                }
                catch (Exception e) { }
                q.Number = result["QUE_number"].ToString();
                q.IsMandatory = Boolean.Parse(result["QUE_mandatory"].ToString());
                try
                {
                    q.ChoiceCount = Int32.Parse(result["QUE_nb_choice"].ToString());
                }
                catch(Exception e)
                {
                    q.ChoiceCount = 0;
                }
                try
                {
                    q.BlockNumber = Int32.Parse(result["QUE_id_block"].ToString());
                }catch(Exception e)
                {
                    q.BlockNumber = -1;
                }
                q.Category = result["QUET_question_type"].ToString();
                q.ControlType = result["CHOT_choice_type"].ToString();
                try
                {
                    q.Prefix = result["CHOT_prefix"].ToString().Trim();
                }catch(Exception e) { }
                questions.Add(q);
            }
            dataBaseConnection.Close();
            foreach (Question q in questions)
            {
                q.Choices = getChoices(q.Id);
            }
            return questions;
        }
        public Dictionary<int, string> getBlocks(int pollId)
        {
            var blocks = new Dictionary<int, string>();
            dataBaseConnection.Open();
            SqlCommand getQuestions = new SqlCommand("sel_blocks", dataBaseConnection);
            getQuestions.CommandType = CommandType.StoredProcedure;
            getQuestions.Parameters.AddWithValue("@poll_id", pollId);
            var result = getQuestions.ExecuteReader();
            while (result.Read())
            {
                blocks.Add(Int32.Parse(result["POLB_id_block"].ToString()), result["POLB_label"].ToString());
            }
            dataBaseConnection.Close();
            return blocks;
        }
        private List<Choice> getChoices(int id)
        {
            List<Choice> choices = new List<Choice>();
            dataBaseConnection.Open();
            SqlCommand getQuestionsChoices = new SqlCommand("sel_choices", dataBaseConnection);
            getQuestionsChoices.CommandType = CommandType.StoredProcedure;
            getQuestionsChoices.Parameters.AddWithValue("@question_id", id);
            var result = getQuestionsChoices.ExecuteReader();
            while (result.Read())
            {
                Choice r = new Choice();
                try
                {
                    r.Id = Int32.Parse(result["CHO_id_choice"].ToString());
                }
                catch (Exception e)
                {}
                try
                {
                    r.Label = result["CHO_choice_label"].ToString();
                }
                catch (Exception e)
                {}
                try
                {
                    r.Id = Int32.Parse(result["CHO_order"].ToString());
                }
                catch (Exception e)
                {}
                try
                {
                    r.Value = result["CHO_choice_value"].ToString();
                }
                catch (Exception e)
                {}
                choices.Add(r);
            }
            dataBaseConnection.Close();
            return choices;
        }   
    }
}
