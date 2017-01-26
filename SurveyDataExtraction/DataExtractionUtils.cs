using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Data.SqlClient;
using SurveyModel;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;

namespace SurveyDataExtraction
{
    public class DataExtractionUtils
    {

        public static List<Atelier> getSessionAtelier(int poll_id, string table)
        {
            var atelier = new List<Atelier>();
            SqlConnection conn;

            conn = ConnexionClasse.getConnexion();
            conn.Open();
            SqlCommand getSession_Atelier = new SqlCommand("sel_session_Atelier", conn);
            getSession_Atelier.CommandType = CommandType.StoredProcedure;
            getSession_Atelier.Parameters.AddWithValue("@id_poll", poll_id);
            getSession_Atelier.Parameters.AddWithValue("@table", table);
            var result = getSession_Atelier.ExecuteReader();
            while (result.Read())
            {
                Atelier ate = new Atelier();
                try
                {
                    ate.id_event = Int32.Parse(result["id_event"].ToString());
                }
                catch (Exception)
                {
                }
                try
                {
                    ate.id_atelier = Int32.Parse(result["id_atelier"].ToString());
                }
                catch (Exception)
                {
                    throw new Exception("id atelier not found");
                }

                ate.theme = result["theme"].ToString();
                ate.description = result["description"].ToString();
                atelier.Add(ate);
            }
            conn.Close();
            return atelier;
        }

        public static DataSet GetAnswersByColumn(string tableName, string columnName)
        {
            var conn = ConnexionClasse.getConnexion();
            var commande = new SqlCommand("sel_answers", conn);
            commande.CommandType = CommandType.StoredProcedure;
            commande.Parameters.AddWithValue("@table", tableName);
            commande.Parameters.AddWithValue("@column", columnName);
            var sqlAdapter = new SqlDataAdapter(commande);
            var dataSet = new DataSet();
            sqlAdapter.Fill(dataSet);
            return dataSet;
        }

        public static List<Atelier> getWsAtelier(int poll_id, string table)
        {
            var atelier = new List<Atelier>();
            SqlConnection conn;

            conn = ConnexionClasse.getConnexion();
            conn.Open();
            SqlCommand getWs_Atelier = new SqlCommand("sel_ws_Atelier", conn);
            getWs_Atelier.CommandType = CommandType.StoredProcedure;
            getWs_Atelier.Parameters.AddWithValue("@id_poll", poll_id);
            getWs_Atelier.Parameters.AddWithValue("@table", table);
            var result = getWs_Atelier.ExecuteReader();
            while (result.Read())
            {
                Atelier ate = new Atelier();
                try
                {
                    ate.id_event = Int32.Parse(result["id_event"].ToString());
                }
                catch (Exception)
                {
                }
                try
                {
                    ate.id_atelier = Int32.Parse(result["id_atelier"].ToString());
                }
                catch (Exception)
                {
                    throw new Exception("id atelier not found");
                }

                ate.theme = result["theme"].ToString();
                ate.description = result["description"].ToString();
                atelier.Add(ate);
            }
            conn.Close();
            return atelier;
        }

        public static int NumberResponse(string table, string nomC, string label, string category, int id_atelier,
            int id_person)
        {
            var clumnname = "";
            var ps = "";
            SqlConnection conn;

            conn = ConnexionClasse.getConnexion();

            if (category.Equals("Activity") || category.Equals("Workshop"))
            {
                clumnname = "SUB_" + nomC;
                ps = "sel_session_ws_responses";
            }
            else if (category.Equals("Meeting"))
            {
                clumnname = "SUM_" + nomC;
                ps = "sel_meeting_responses";
            }

            conn.Open();
            SqlCommand getNumberResponse = new SqlCommand(ps, conn);
            getNumberResponse.CommandType = CommandType.StoredProcedure;
            getNumberResponse.Parameters.AddWithValue("@column", clumnname);
            getNumberResponse.Parameters.AddWithValue("@label", label);
            getNumberResponse.Parameters.AddWithValue("@table", table);
            getNumberResponse.Parameters.AddWithValue("@id_atelier", id_atelier);
            getNumberResponse.Parameters.AddWithValue("@id_person", id_person);
            var result = getNumberResponse.ExecuteReader();
            var nb = 0;
            while (result.Read())
            {
                try
                {
                    nb = Int32.Parse(result["nb"].ToString());

                }
                catch (Exception)
                {
                }
            }
            conn.Close();


            return nb;
        }

        public static int NumberResponse_GeneralQuestion(string table, string nomC, string label)
        {
            SqlConnection conn;

            conn = ConnexionClasse.getConnexion();
            conn.Open();


            SqlCommand getNumberResponse = new SqlCommand("selReponse", conn);
            getNumberResponse.CommandType = CommandType.StoredProcedure;
            getNumberResponse.Parameters.AddWithValue("@table", table);
            getNumberResponse.Parameters.AddWithValue("@column", nomC);
            getNumberResponse.Parameters.AddWithValue("@label", label);

            var result = getNumberResponse.ExecuteReader();
            var nb = 0;
            while (result.Read())
            {
                try
                {
                    nb = Int32.Parse(result["nb"].ToString());

                }
                catch (Exception e)
                {
                }
            }
            conn.Close();


            return nb;
        }

        public static List<Meeting> getAttendedMeetings(int id_poll, string tablename)
        {
            var meetings = new List<Meeting>();

            SqlConnection conn;

            conn = ConnexionClasse.getConnexion();
            conn.Open();
            SqlCommand getMeetings = new SqlCommand("sel_attended_meetings", conn);
            getMeetings.CommandType = CommandType.StoredProcedure;
            getMeetings.Parameters.AddWithValue("@id_poll", id_poll);
            getMeetings.Parameters.AddWithValue("@table", tablename);
            var result = getMeetings.ExecuteReader();

            while (result.Read())
            {
                Meeting m = new Meeting();

                try
                {
                    m.id_meeting = Int32.Parse(result["SUM_id_meeting"].ToString());
                }
                catch (Exception)
                {
                }
                try
                {
                    m.id_company = Int32.Parse(result["SUM_id_company"].ToString());
                }
                catch (Exception)
                {
                }
                meetings.Add(m);
            }
            conn.Close();

            return meetings;
        }

        public static void DeleteGeneratedFile(string path, int sleepTime)
        {
            try
            {
                new Thread(() =>
                {
                    try
                    {
                        Thread.Sleep(sleepTime);
                        System.IO.File.Delete(path);
                    }
                        catch (Exception) { }
                }).Start();
            }
            catch (Exception)
            { }
        }
    }
}
