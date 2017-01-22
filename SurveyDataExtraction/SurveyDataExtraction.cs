using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Data.SqlClient;
using SurveyModel;
using System.Runtime.InteropServices;
using System.Drawing;

namespace SurveyDataExtractorGenerator
{
    public class SurveyDataExtraction
    {

        public  int getNbParticipants(int poll_id, string table)
        {
            SqlConnection conn;
            // 
            conn = ConnexionClasse.getConnexion();
            conn.Open();
            SqlCommand nbParticipants = new SqlCommand("sel_nb_participants", conn);
            nbParticipants.CommandType = CommandType.StoredProcedure;
            nbParticipants.Parameters.AddWithValue("@poll_id", poll_id);
            nbParticipants.Parameters.AddWithValue("@table", table);
            var result = nbParticipants.ExecuteReader();
            var nb = 0;
            while (result.Read())
            {
                try
                {
                    nb = Int32.Parse(result["nb"].ToString());

                }
                catch (Exception e) { }
            }
            conn.Close();


            return nb;
        }

        public string getMeetingParticipants(int id_meeting, int id_company)
        {
            SqlConnection conn;

            conn = ConnexionClasse.getConnexion();
            conn.Open();
            SqlCommand nbParticipants = new SqlCommand("sel_participantscompanies", conn);
            nbParticipants.CommandType = CommandType.StoredProcedure;
            nbParticipants.Parameters.AddWithValue("@id_meeting", id_meeting);
            nbParticipants.Parameters.AddWithValue("@id_company", id_company);
            var result = nbParticipants.ExecuteReader();
            var nb = "";
            while (result.Read())
            {
                try
                {
                    nb = result["participants"].ToString();

                }
                catch (Exception e) { }
            }
            conn.Close();


            return nb;
        }


        public  DataSet getGeneralAnswers(int id_poll, string tablename)
        {
            SqlConnection conn;

            conn = ConnexionClasse.getConnexion();

            SqlCommand Answers = new SqlCommand("sel_general_answers", conn);

            Answers.Parameters.AddWithValue("@id_poll", id_poll);
            Answers.Parameters.AddWithValue("@table", tablename);
            Answers.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dscmd = new SqlDataAdapter(Answers);
            dscmd.SelectCommand = Answers;
            DataSet ds = new DataSet();
            dscmd.Fill(ds);

            return ds;
        }

        string table;
        public DataSet getSessionWsAnswers(string category, string tablename)
        {
            if (category.Equals("Activity"))
                table = "ATELIER_SESSION";
            else
                if (category.Equals("Workshop"))
                table = "ATELIER_WS";
            SqlConnection conn;
            conn = ConnexionClasse.getConnexion();
            SqlCommand Answers = new SqlCommand("sel_session_ws_answers", conn);
            Answers.Parameters.AddWithValue("@table", tablename);
            Answers.Parameters.AddWithValue("@tableSorWS", table);
            Answers.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dscmd = new SqlDataAdapter(Answers);
            dscmd.SelectCommand = Answers;
            DataSet ds = new DataSet();
            dscmd.Fill(ds);

            return ds;
        }

        //****************************************************
        public  DataSet getMeetingAnswers(int id_meeting, int id_company, string tablename)
        {
            SqlConnection conn;

            conn = ConnexionClasse.getConnexion();
            SqlCommand Answers = new SqlCommand("sel_meeting_answers", conn);
            Answers.Parameters.AddWithValue("@table", tablename);
            Answers.Parameters.AddWithValue("@id_meeting", id_meeting);
            Answers.Parameters.AddWithValue("@id_company", id_company);
            Answers.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dscmd = new SqlDataAdapter(Answers);
            dscmd.SelectCommand = Answers;
            DataSet ds = new DataSet();
            dscmd.Fill(ds);

            return ds;
        }
      

        public List<Atelier> getSessionAtelier(int poll_id, string table)
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
                catch (Exception) { }
                try
                {
                    ate.id_atelier = Int32.Parse(result["id_atelier"].ToString());
                }
                catch (Exception) { throw new Exception("id atelier not found"); }

                ate.theme = result["theme"].ToString();
                ate.description = result["description"].ToString();
                atelier.Add(ate);
            }
            conn.Close();
            return atelier;
        }
        public List<Atelier> getWsAtelier(int poll_id, string table)
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
                catch (Exception) { }
                try
                {
                    ate.id_atelier = Int32.Parse(result["id_atelier"].ToString());
                }
                catch (Exception) { throw new Exception("id atelier not found"); }

                ate.theme = result["theme"].ToString();
                ate.description = result["description"].ToString();
                atelier.Add(ate);
            }
            conn.Close();
            return atelier;
        }

       

        public static string clumnname;
        public static string ps;
        public int NumberResponse(string table, string nomC, string label, string category, int id_atelier, int id_person)
        {
            SqlConnection conn;

            conn = ConnexionClasse.getConnexion();

            if (category.Equals("Activity"))
            {
                clumnname = "SUB_" + nomC;
                ps = "sel_sessionANDws_responses";
            }
            else
            if (category.Equals("Meeting"))
            {
                clumnname = "SUM_" + nomC;
                ps = "sel_meeting_responses";
            }
            else
            if (category.Equals("Workshop"))
            {
                clumnname = "SUB_" + nomC;
                ps = "sel_sessionANDws_responses";
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
                catch (Exception e) { }
            }
            conn.Close();


            return nb;
        }

        public int NumberResponse_GeneralQuestion(string table, string nomC, string label)
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
                catch (Exception e) { }
            }
            conn.Close();


            return nb;
        }
        public List<Meeting> getAttendedMeetings(int id_poll, string tablename)
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
                catch (Exception) { }
                try
                {
                    m.id_company = Int32.Parse(result["SUM_id_company"].ToString());
                }
                catch (Exception) { }
                meetings.Add(m);
            }
            conn.Close();

            return meetings;
        }

        public  Excel.Workbook Print_into_excel_file(List<Question> Questions, List<Meeting> attendedmeetings, string surveytable, string meetingtable,
            string sessiontable, string wstable)
        {
            string data = null;

            var _excel = new Excel.Application();
            var wb = _excel.Workbooks.Add();
            try
            {


               
                var collection = new Microsoft.Office.Interop.Excel.Worksheet[5];


                collection[0] = wb.Worksheets.Add();
                collection[0].Name = String.Format("Workshop");
                collection[1] = wb.Worksheets.Add();
                collection[1].Name = String.Format("Session");
                collection[2] = wb.Worksheets.Add();
                collection[2].Name = String.Format("Meeting");
                collection[3] = wb.Worksheets.Add();
                collection[3].Name = String.Format("General");

                Excel.Worksheet xl_Workshop_Sheet = collection[0];
                Excel.Worksheet xl_Session_Sheet = collection[1];
                Excel.Worksheet xl_Meeting_Sheet = collection[2];
                Excel.Worksheet xl_General_Sheet = collection[3];

                int position = 3;
                int meetingpos = 1;
                int sessionpos = 1;
                int wspos = 1;

                xl_General_Sheet.Cells[1, 1] = "Nombre de participants";
                xl_General_Sheet.Cells[1, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                xl_General_Sheet.Cells[2, 1] = getNbParticipants(1, surveytable);
                DataSet dsAnswers = getGeneralAnswers(1, surveytable);
                for (int j = 0; j <= Questions.Count - 1; j++)
                {
                    Question q = Questions[j];
                    List<SubQuestion> subQuestion = q.SubQuestions;
                    if (q.Category.Equals("General"))
                    {
                        drawEntete(position, xl_General_Sheet);
                        data = q.Label;
                        xl_General_Sheet.Cells[3, j + 5] = data;
                        xl_General_Sheet.Cells[3, j + 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                        xl_General_Sheet.Columns.AutoFit();
                        drawquestions2(position, dsAnswers, xl_General_Sheet, q.Category);

                    }
                    else
                        if (q.Category.Equals("Meeting"))
                    {

                        xl_Meeting_Sheet.Cells[meetingpos, 5] = "Id_meeting";
                        xl_Meeting_Sheet.Cells[meetingpos, 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                        xl_Meeting_Sheet.Columns.AutoFit();
                        xl_Meeting_Sheet.Cells[meetingpos, 6] = "Owner";
                        xl_Meeting_Sheet.Cells[meetingpos, 6].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                        xl_Meeting_Sheet.Columns.AutoFit();
                        xl_Meeting_Sheet.Cells[meetingpos, 7] = "Participants";
                        xl_Meeting_Sheet.Cells[meetingpos, 7].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                        xl_Meeting_Sheet.Columns.AutoFit();

                        for (int s = 0; s <= subQuestion.Count - 1; s++)
                        {
                            data = subQuestion[s].Label;
                            xl_Meeting_Sheet.Cells[meetingpos, 8 + s] = data;
                            xl_Meeting_Sheet.Cells[meetingpos, 8 + s].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                            xl_Meeting_Sheet.Columns.AutoFit();

                        }
                        drawEntete(meetingpos, xl_Meeting_Sheet);

                        foreach (Meeting meeting in attendedmeetings)
                        {
                            DataSet ds = getMeetingAnswers(meeting.id_meeting, meeting.id_company, meetingtable);
                            drawquestions2(meetingpos + 1, ds, xl_Meeting_Sheet, q.Category);
                            meetingpos += 1;
                            xl_Meeting_Sheet.Cells[meetingpos, 7] = getMeetingParticipants(meeting.id_meeting, meeting.id_company);
                            xl_Meeting_Sheet.Columns.AutoFit();
                        }
                    }


                    else
                        if (q.Category.Equals("Activity"))
                    {
                        xl_Session_Sheet.Cells[sessionpos, 5] = "theme";
                        xl_Session_Sheet.Cells[sessionpos, 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                        xl_Session_Sheet.Columns.AutoFit();
                        for (int s = 0; s <= subQuestion.Count - 1; s++)
                        {
                            data = subQuestion[s].Label;
                            xl_Session_Sheet.Cells[sessionpos, 6 + s] = data;
                            xl_Session_Sheet.Cells[sessionpos, 6 + s].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                            xl_Session_Sheet.Columns.AutoFit();
                        }
                        drawEntete(sessionpos, xl_Session_Sheet);

                        DataSet ds = getSessionWsAnswers(q.Category, sessiontable);
                        drawquestions2(sessionpos + 1, ds, xl_Session_Sheet, q.Category);
                    }
                    else
                        if (q.Category.Equals("Workshop"))
                    {
                        xl_Workshop_Sheet.Cells[wspos, 5] = "theme";
                        xl_Workshop_Sheet.Cells[wspos, 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                        xl_Workshop_Sheet.Columns.AutoFit();
                        for (int s = 0; s <= subQuestion.Count - 1; s++)
                        {
                            data = subQuestion[s].Label;
                            xl_Workshop_Sheet.Cells[wspos, 6 + s] = data;
                            xl_Workshop_Sheet.Cells[wspos, 6 + s].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                            xl_Workshop_Sheet.Columns.AutoFit();
                        }

                        drawEntete(wspos, xl_Workshop_Sheet);
                        DataSet ds = getSessionWsAnswers(q.Category, wstable);
                        drawquestions2(wspos + 1, ds, xl_Workshop_Sheet, q.Category);
                        wspos += 5;


                    }
                }

                wb.Worksheets[5].delete();
              }


            finally
            {
                Marshal.ReleaseComObject(_excel);
            }
            return wb;

        }

        public void drawEntete(int pos, Excel.Worksheet worksheet)
        {
            worksheet.Cells[pos, 1] = "Id_Person";
            worksheet.Cells[pos, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
            worksheet.Columns.AutoFit();
            worksheet.Cells[pos, 2] = "Id_Company";
            worksheet.Cells[pos, 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
            worksheet.Columns.AutoFit();
            worksheet.Cells[pos, 3] = "FirstName";
            worksheet.Cells[pos, 3].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
            worksheet.Columns.AutoFit();
            worksheet.Cells[pos, 4] = "LastName";
            worksheet.Cells[pos, 4].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
            worksheet.Columns.AutoFit();


        }
        public void drawquestions2(int pos, DataSet dsAnswers, Excel.Worksheet worksheet, string category)
        {

            for (int j = 0; j <= dsAnswers.Tables[0].Rows.Count - 1; j++)
            {
                int k = 0;
                if (category.Equals("General"))
                {

                    for (int i = 0; i <= dsAnswers.Tables[0].Columns.Count - 1; i++)
                    {
                        if (i == 4 || i == 5)
                            continue;
                        worksheet.Cells[j + pos + 1, k + 1] = dsAnswers.Tables[0].Rows[j].ItemArray[i].ToString();
                        worksheet.Columns.AutoFit();
                        k++;

                    }
                }
                else
                    if (category.Equals("Meeting"))
                {
                    for (int i = 0; i <= dsAnswers.Tables[0].Columns.Count - 1; i++)
                    {
                        if (i == 6 || i == 7 || i == 8 || i == 9 || i == 10)
                            continue;
                        worksheet.Cells[pos + j, k + 1] = dsAnswers.Tables[0].Rows[j].ItemArray[i].ToString();
                        worksheet.Columns.AutoFit();
                        k++;

                    }
                }
                else
                {

                    for (int i = 0; i <= dsAnswers.Tables[0].Columns.Count - 1; i++)
                    {
                        if (i == 5 || i == 6 || i == 7 || i == 8 || i == 9 || i == 10)
                            continue;
                        worksheet.Cells[pos + j, k + 1] = dsAnswers.Tables[0].Rows[j].ItemArray[i].ToString();
                        worksheet.Columns.AutoFit();
                        k++;
                    }
                }

            }
        }


        public Excel.Workbook Print_into_excel_file2(List<Question> Questions, string surveytable, string meetingtable, string sessiontable, string wstable,
            List<Meeting> Meetings, List<Atelier> sessionAtelier, List<Atelier> wsAtelier)
        {
            int position = 3;
            int meetingpos = 1;
            int sessionpos = 1;
            int wspos = 1;

            var _excel = new Excel.Application();
            var wb = _excel.Workbooks.Add();
            try
            {


               // var wb = _excel.Workbooks.Add();
                var collection = new Microsoft.Office.Interop.Excel.Worksheet[5];


                collection[0] = wb.Worksheets.Add();
                collection[0].Name = String.Format("Workshop");
                collection[1] = wb.Worksheets.Add();
                collection[1].Name = String.Format("Session");
                collection[2] = wb.Worksheets.Add();
                collection[2].Name = String.Format("Meeting");
                collection[3] = wb.Worksheets.Add();
                collection[3].Name = String.Format("General");

                Excel.Worksheet xl_Workshop_Sheet = collection[0];
                Excel.Worksheet xl_Session_Sheet = collection[1];
                Excel.Worksheet xl_Meeting_Sheet = collection[2];
                Excel.Worksheet xl_General_Sheet = collection[3];

                

                xl_General_Sheet.Cells[1, 1] = "Nombre de participants";
                xl_General_Sheet.Cells[1, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                xl_General_Sheet.Cells[2, 1] = getNbParticipants(1, surveytable);

                for (int j = 0; j < Questions.Count; j++)
                {

                    Question q = Questions[j];
                    if (q.ChoiceCount > 0)
                    {
                        xl_General_Sheet.Cells[position, 1] = q.Label;
                        xl_General_Sheet.Cells[position, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                        xl_General_Sheet.Cells[position + 1, 1] = "Nombre de réponses";
                        xl_General_Sheet.Columns.AutoFit();
                        int i;
                        int cpt = 0;

                        List<Choice> Choices = new List<Choice>();
                        Choices = q.Choices;
                        for (i = 0; i <= Choices.Count - 1; i++)
                        {

                            int nbreponse = NumberResponse_GeneralQuestion(surveytable, q.Column, Choices[i].Label);
                            xl_General_Sheet.Cells[position, i + 2] = Choices[i].Label;
                            xl_General_Sheet.Cells[position, i + 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                            xl_General_Sheet.Columns.AutoFit();
                            xl_General_Sheet.Cells[position + 1, i + 2] = nbreponse;
                            cpt += nbreponse;

                        }
                        xl_General_Sheet.Cells[position, i + 2] = "Total";
                        xl_General_Sheet.Cells[position, i + 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                        xl_General_Sheet.Columns.AutoFit();
                        xl_General_Sheet.Cells[position + 1, i + 2] = cpt;
                        cpt = 0;
                        position = position + 3;
                    }
                }

                for (int d = 0; d <= Meetings.Count - 1; d++)
                {
                    Meeting meeting = Meetings[d];
                    string guests = "";
                    foreach (Person p in meeting.guests)
                        guests += p.FirstName + "(" + p.CompanyName + ");";
                    xl_Meeting_Sheet.Cells[meetingpos, 1] = guests;
                    xl_Meeting_Sheet.Cells[meetingpos, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                    xl_Meeting_Sheet.Columns.AutoFit();
                    for (int j = 0; j < Questions.Count; j++)
                    {
                        Question q = Questions[j];
                        if (q.Category.Equals("Meeting"))
                            drawQuestions(q, meetingpos + 1, xl_Meeting_Sheet, meeting.guests, 0, meetingtable, "", "");
                    }
                    meetingpos = meetingpos + 4;
                }

                for (int d = 0; d <= sessionAtelier.Count - 1; d++)
                {
                    Atelier session = sessionAtelier[d];
                    xl_Session_Sheet.Cells[sessionpos, 1] = session.theme;
                    xl_Session_Sheet.Cells[sessionpos, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                    xl_Session_Sheet.Columns.AutoFit();
                    for (int j = 0; j < Questions.Count; j++)
                    {
                        Question q = Questions[j];
                        if (q.Category.Equals("Activity"))
                            drawQuestions(q, sessionpos + 1, xl_Session_Sheet, null, session.id_atelier, "", sessiontable, "");
                    }
                    sessionpos = sessionpos + 4;
                }

                for (int d = 0; d <= wsAtelier.Count - 1; d++)
                {
                    Atelier workshop = wsAtelier[d];
                    xl_Workshop_Sheet.Cells[wspos, 1] = workshop.theme;
                    xl_Workshop_Sheet.Cells[wspos, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                    xl_Workshop_Sheet.Columns.AutoFit();
                    for (int j = 0; j < Questions.Count; j++)
                    {
                        Question q = Questions[j];
                        if (q.Category.Equals("Workshop"))
                            drawQuestions(q, wspos + 1, xl_Workshop_Sheet, null, workshop.id_atelier, "", "", wstable);
                    }
                    wspos = wspos + 4;
                }
                wb.Worksheets[5].delete();
               
            }

            finally
            {
                Marshal.ReleaseComObject(_excel);
            }
            return wb;
        }

        private void drawQuestions(Question q, int position, Excel.Worksheet xlWorkSheet, List<Person> guests, int id_atelier
            , string meetingtable, string sessiontable, string wstable)
        {

            
            List<SubQuestion> subQuestions = q.SubQuestions; ;

            for (int s = 0; s <= subQuestions.Count - 1; s++)
            {
                Question qq = subQuestions[s];
                if (qq.ChoiceCount > 0)
                {

                    xlWorkSheet.Cells[position, 1] = qq.Label;
                    xlWorkSheet.Cells[position, 1].Interior.Color = ColorTranslator.ToOle(Color.LightGreen);
                    xlWorkSheet.Columns.AutoFit();
                    xlWorkSheet.Cells[position + 1, 1] = "Nombre de réponses";
                    xlWorkSheet.Columns.AutoFit();
                    int i;
                    int cpt = 0;

                    List<Choice> Choices2 = new List<Choice>();
                    Choices2 = qq.Choices;
                    for (i = 0; i <= Choices2.Count - 1; i++)
                    {
                        int nbreponse = 0;
                        if (qq.Category.Equals("Meeting"))
                            foreach (Person p in guests)
                                nbreponse += NumberResponse(meetingtable, qq.Column, Choices2[i].Label, qq.Category, 0, p.Id);
                        else
                          if (qq.Category.Equals("Activity"))
                            nbreponse += NumberResponse(sessiontable, qq.Column, Choices2[i].Label, qq.Category, id_atelier, 0);
                        else
                              if (qq.Category.Equals("Workshop"))
                            nbreponse += NumberResponse(wstable, qq.Column, Choices2[i].Label, qq.Category, id_atelier, 0);

                        xlWorkSheet.Cells[position, i + 2] = Choices2[i].Label;
                        xlWorkSheet.Cells[position, i + 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                        xlWorkSheet.Columns.AutoFit();
                        xlWorkSheet.Cells[position + 1, i + 2] = nbreponse;
                        cpt += nbreponse;

                    }


                    xlWorkSheet.Cells[position, i + 1] = "Total";
                    xlWorkSheet.Cells[position, i + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                    xlWorkSheet.Columns.AutoFit();
                    xlWorkSheet.Cells[position + 1, i + 1] = cpt;
                    cpt = 0;

                }
            }
        }
    }
}
