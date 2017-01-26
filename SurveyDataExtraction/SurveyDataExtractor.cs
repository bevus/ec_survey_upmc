using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using SurveyModel;
using Excel = Microsoft.Office.Interop.Excel;

namespace SurveyDataExtraction
{
    public class SurveyDataExtractor
    {
        

        public int PollId { get; set; }
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
                catch (Exception) { }
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
      
        public  Excel.Workbook Print_into_excel_file(List<Question> Questions, List<Meeting> attendedmeetings, string surveytable, string meetingtable,
            string sessiontable, string wstable)
        {
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

                var position = 3;
                var meetingpos = 1;
                var sessionpos = 1;
                var wspos = 1;

                xl_General_Sheet.Cells[1, 1] = "Nombre de participants";
                xl_General_Sheet.Cells[1, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                xl_General_Sheet.Cells[2, 1] = getNbParticipants(PollId, surveytable);
                var columnG = PersonInfoColumns(position, xl_General_Sheet, QuestionType.General, surveytable);
                var columnM = PersonInfoColumns(meetingpos, xl_Meeting_Sheet, QuestionType.Meeting, meetingtable);
                var columnS = PersonInfoColumns(sessionpos, xl_Session_Sheet, QuestionType.Session, sessiontable);
                var columnWS = PersonInfoColumns(wspos, xl_Workshop_Sheet, QuestionType.Workshop, wstable);
                for (int i = 0, g = columnG, m = columnM, s = columnS, ws = columnWS; i < Questions.Count; i++)
                {
                    var q = Questions[i];
                    switch (q.Category)
                    {
                        case QuestionType.General:
                            DrawColomn(surveytable, xl_General_Sheet, position, g, q);
                            g++;
                            break;
                        case QuestionType.Meeting:
                            foreach (var sq in q.SubQuestions)
                            {
                                sq.Column = "SUM_" + sq.Column;
                                DrawColomn(meetingtable, xl_Meeting_Sheet, meetingpos, m, sq);
                                m++;
                            }
                            break;
                        case QuestionType.Session:
                            foreach (var sq in q.SubQuestions)
                            {
                                sq.Column = "SUB_" + sq.Column;
                                DrawColomn(sessiontable, xl_Session_Sheet, sessionpos, s, sq);
                                s++;
                            }
                            break;
                        case QuestionType.Workshop:
                            foreach (var sq in q.SubQuestions)
                            {
                                sq.Column = "SUB_" + sq.Column;
                                DrawColomn(wstable, xl_Workshop_Sheet, wspos, ws, sq);
                                ws++;
                            }
                            break;
                    }
                }
                xl_General_Sheet.Columns.AutoFit();
                xl_Meeting_Sheet.Columns.AutoFit();
                xl_Session_Sheet.Columns.AutoFit();
                xl_Workshop_Sheet.Columns.AutoFit();

                wb.Worksheets[5].delete();
              }

            finally
            {
                Marshal.ReleaseComObject(_excel);
            }
            return wb;

        }

        private void DrawColomn(string table, Excel.Worksheet sheet, int line, int column, Question q)
        {
            sheet.Cells[line, column] = q.Label;
            sheet.Cells[line, column].Interior.Color = ColorTranslator.ToOle(Color.LightGreen);
            var ds = DataExtractionUtils.GetAnswersByColumn(table, q.Column);
            var tmp = line + 1;
            for (var j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                sheet.Cells[tmp, column] = ds.Tables[0].Rows[j][q.Column];
                tmp++;
            }
        }

        public int  PersonInfoColumns(int pos, Excel.Worksheet worksheet, string questionType, string tableName)
        {
            var column = 1;
            var generalPersonInfo = new Dictionary<string, string>
            {
                {"PER_id_person", "Id_person" },
                {"PER_id_company", "Id_company" },
                {"PER_first_name", "First name" },
                {"PER_last_name", "Last name" },
            };

            var sessionPersonalInfo = new Dictionary<string, string>
            {
                {"PER_id_person", "Id_person" },
                {"PER_id_company", "Id_company" },
                {"PER_first_name", "First name" },
                {"PER_last_name", "Last name" },
                {"theme", "theme" }
            };

            var wsPersonalInfo = new Dictionary<string, string>
            {
                {"PER_id_person", "Id_person" },
                {"PER_id_company", "Id_company" },
                {"PER_first_name", "First name" },
                {"PER_last_name", "Last name" },
                {"theme", "theme" }

            };

            var meetingPersonalInfo = new Dictionary<string, string>
            {
                {"MEE_id_person", "Id_person" },
                {"PER_id_company", "Id_company" },
                {"PER_first_name", "First name" },
                {"PER_last_name", "Last name" },
                {"MEE_id_meeting", "id_meeting" },
                { "MEE_owner", "owner" }
            };
            Dictionary<string, string> keys;
            switch (questionType)
            {
                case QuestionType.General:
                    keys = generalPersonInfo;
                    break;
                case QuestionType.Meeting:
                    keys = meetingPersonalInfo;
                    break;
                case QuestionType.Workshop:
                    keys = wsPersonalInfo;
                    break;
                case QuestionType.Session:
                    keys = sessionPersonalInfo;
                    break;
                default:
                    keys = null;
                    break;
            }
            foreach (var key in keys.Keys)
            {
                worksheet.Cells[pos, column] = keys[key];
                worksheet.Cells[pos, column++].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                worksheet.Columns.AutoFit();
            }
            if (questionType == QuestionType.Meeting)
            {
                worksheet.Cells[pos, column] = "Participants";
                worksheet.Cells[pos, column++].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                worksheet.Columns.AutoFit();
            }
            pos++;
            var dataSet = DataExtractionUtils.GetAnswersByColumn(tableName, "person_info");
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                column = 1;
                foreach (var columnName in keys.Keys)
                {
                    worksheet.Cells[pos, column++] = dataSet.Tables[0].Rows[i][columnName];
                }
                if (questionType == QuestionType.Meeting)
                {
                    var companies = getMeetingParticipants(
                        int.Parse(dataSet.Tables[0].Rows[i]["MEE_id_meeting"].ToString()),
                        int.Parse(dataSet.Tables[0].Rows[i]["PER_id_company"].ToString())
                    );
                    worksheet.Cells[pos, column++] = companies;
                }
                pos++;
            }
            worksheet.Columns.AutoFit();
            return column;
        }
        public void drawquestions2(int pos, DataSet dsAnswers, Excel.Worksheet worksheet, List<int> ignoredIndexes )
        {
            for (int j = 0; j < dsAnswers.Tables[0].Rows.Count; j++)
            {
                int k = 0;
                for (int i = 0; i < dsAnswers.Tables[0].Columns.Count; i++)
                {
                    if (ignoredIndexes.Contains(i))
                        continue;
                    worksheet.Cells[j + pos + 1, k + 1] = dsAnswers.Tables[0].Rows[j].ItemArray[i].ToString();
                    worksheet.Columns.AutoFit();
                    k++;
                }
            }
        }


        public Excel.Workbook Print_into_excel_file2(List<Question> Questions, string surveytable, string meetingtable, string sessiontable, string wstable,
            List<Meeting> Meetings, List<Atelier> sessionAtelier, List<Atelier> wsAtelier)
        {
            int position = 1;
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

                

                //xl_General_Sheet.Cells[1, 1] = "Nombre de participants";
                //xl_General_Sheet.Cells[1, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                //xl_General_Sheet.Cells[2, 1] = getNbParticipants(PollId, surveytable);

                for (int j = 0; j < Questions.Count; j++)
                {

                    Question q = Questions[j];
                    if (isPredifinedChoices(q))
                    {
                        xl_General_Sheet.Cells[position, 1] = q.Label;
                        xl_General_Sheet.Cells[position, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                        xl_General_Sheet.Cells[position + 1, 1] = "Nombre de réponses";
                        xl_General_Sheet.Columns.AutoFit();
                        int i;
                        int cpt = 0;

                        List<Choice> Choices = new List<Choice>();
                        Choices = q.Choices;
                        for (i = 0; i < Choices.Count; i++)
                        {

                            int nbreponse = DataExtractionUtils.NumberResponse_GeneralQuestion(surveytable, q.Column, Choices[i].Label);
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
                    int nbquestion = 0;
                    for (int j = 0; j < Questions.Count; j++)
                    {
                        Question q = Questions[j];
                        
                        if (q.Category.Equals("Meeting"))
                        {
                            drawQuestions(q, meetingpos + 1, xl_Meeting_Sheet, meeting.guests, 0, meetingtable, "", "");
                            nbquestion++;
                        }
                    }
                    meetingpos = meetingpos + 5* nbquestion;
                }

                for (int d = 0; d <= sessionAtelier.Count - 1; d++)
                {
                    Atelier session = sessionAtelier[d];
                    xl_Session_Sheet.Cells[sessionpos, 1] = session.theme;
                    xl_Session_Sheet.Cells[sessionpos, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                    xl_Session_Sheet.Columns.AutoFit();
                    int nbquestion = 0;
                    for (int j = 0; j < Questions.Count; j++)
                    {
                        Question q = Questions[j];
                        
                        if (q.Category.Equals("Activity"))
                        {
                            drawQuestions(q, sessionpos + 1, xl_Session_Sheet, null, session.id_atelier, "",
                                sessiontable, "");
                            nbquestion++;
                        }
                    }
                    sessionpos = sessionpos + 5 * nbquestion;
                }

                for (int d = 0; d <= wsAtelier.Count - 1; d++)
                {
                    Atelier workshop = wsAtelier[d];
                    xl_Workshop_Sheet.Cells[wspos, 1] = workshop.theme;
                    xl_Workshop_Sheet.Cells[wspos, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                    xl_Workshop_Sheet.Columns.AutoFit();
                    int nbquestion = 0;
                    for (int j = 0; j < Questions.Count; j++)
                    {
                        Question q = Questions[j];
                        if (q.Category.Equals("Workshop"))
                        {
                            drawQuestions(q, wspos + 1, xl_Workshop_Sheet, null, workshop.id_atelier, "", "", wstable);
                            nbquestion++;
                        }
                    }
                    wspos = wspos + 5* nbquestion;
                }
                wb.Worksheets[5].delete();
            }
            finally
            {
                Marshal.ReleaseComObject(_excel);
            }
            return wb;
        }

        private bool isPredifinedChoices(Question question)
        {
            return question.ControlType.Equals("DropDownList") ||
                   question.ControlType.Equals("RadioButtonList") ||
                   question.ControlType.Equals("CheckBoxList");
        }

        private void drawQuestions(Question q, int position, Excel.Worksheet xlWorkSheet, List<Person> guests, int id_atelier
            , string meetingtable, string sessiontable, string wstable)
        {

            
            List<SubQuestion> subQuestions = q.SubQuestions; ;

            for (int s = 0; s <= subQuestions.Count - 1; s++)
            {
                Question qq = subQuestions[s];
                if (isPredifinedChoices(qq))
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
                    for (i = 0; i < Choices2.Count; i++)
                    {
                        int nbreponse = 0;
                        if (qq.Category.Equals("Meeting"))
                            foreach (Person p in guests)
                                nbreponse += DataExtractionUtils.NumberResponse(meetingtable, qq.Column, Choices2[i].Label, qq.Category, 0, p.Id);
                        else
                          if (qq.Category.Equals("Activity"))
                            nbreponse += DataExtractionUtils.NumberResponse(sessiontable, qq.Column, Choices2[i].Label, qq.Category, id_atelier, 0);
                        else
                              if (qq.Category.Equals("Workshop"))
                            nbreponse += DataExtractionUtils.NumberResponse(wstable, qq.Column, Choices2[i].Label, qq.Category, id_atelier, 0);

                        xlWorkSheet.Cells[position, i + 2] = Choices2[i].Label;
                        xlWorkSheet.Cells[position, i + 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                        xlWorkSheet.Columns.AutoFit();
                        xlWorkSheet.Cells[position + 1, i + 2] = nbreponse;
                        cpt += nbreponse;

                    }


                    xlWorkSheet.Cells[position, i + 2] = "Total";
                    xlWorkSheet.Cells[position, i + 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                    xlWorkSheet.Columns.AutoFit();
                    xlWorkSheet.Cells[position + 1, i + 2] = cpt;
                    cpt = 0;
                    position += 2;
                }
            }
        }
    }
}
