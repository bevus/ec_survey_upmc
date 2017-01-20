using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SurveyModel;
using DataAccess;

namespace SurveyDashboardGenerator
{
    public class DashboardUtils
    {
       
        public DashboardUtils()
        {
        }


        //Général
        public object GetGeneralResponse(Question q, String tableName)
        {
            var d = new List<object>();
            { d.Add(new List<object> { "label", "data" }); }
            switch (q.ControlType.ToString())
            {
                case "DropDownList":
                    foreach (Choice c in q.Choices)
                    { d.Add(new List<object> { c.Label, NumberResponseGeneral(q.Column, c.Label, tableName) }); }
                    return new { ques = q.Label.ToString(), qCategory = "General", type = q.ControlType, idq = q.Id, rep = d };

                case "RadioButtonList":
                    foreach (Choice c in q.Choices)
                    { d.Add(new List<object> { c.Label, NumberResponseGeneral(q.Column, c.Label, tableName) }); }
                    return new { ques = q.Label.ToString(), qCategory = "General", type = q.ControlType, idq = q.Id, rep = d };

                case "CheckBoxList":
                    d = new List<object>();
                    foreach (Choice c in q.Choices)
                    {
                        d.Add(new object[] { c.Label, NumberResponseMultipleChoice(q.Column, c.Label, tableName) });
                        // d.Add(new { label = c.Label, data = new int[]   { 10, NumberResponseMultipleChoice(q.Column, c.Label) } } );
                    }
                    return new { ques = q.Label.ToString(), qCategory = "General", type = q.ControlType, idq = q.Id, rep = d };

                default:
                    return null;
            }
        }
        public object GetQesDataResponse(Question q, String tableName)
        {
            var d = new List<object>();
            { d.Add(new List<object> { "label", "data" }); }
            switch (q.ControlType.ToString())
            {
                case "DropDownList":
                    foreach (Choice c in q.Choices)
                    { d.Add(new List<object> { c.Label, NumberResponseGeneral(q.Column, c.Label, tableName) }); }
                    return new { ctr = q.ControlType, rep = d };

                case "RadioButtonList":
                    foreach (Choice c in q.Choices)
                    { d.Add(new List<object> { c.Label, NumberResponseGeneral(q.Column, c.Label, tableName) }); }
                    return new { ctr = q.ControlType, rep = d };

                case "CheckBoxList":
                    d = new List<object>();
                    foreach (Choice c in q.Choices)
                    {
                        d.Add(new object[] { c.Label, NumberResponseMultipleChoice(q.Column, c.Label, tableName) });
                        // d.Add(new { label = c.Label, data = new int[]   { 10, NumberResponseMultipleChoice(q.Column, c.Label) } } );
                    }
                    return new { ctr = q.ControlType, rep = d };

                default:
                    return null;
            }
        }
        public int NumberResponseGeneral(String nomC, String label, String table_name)
        {
            SqlConnection dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            SqlCommand getNumberResponse = new SqlCommand("selReponseGeneral", dataBaseConnection);
            getNumberResponse.CommandType = CommandType.StoredProcedure;
            getNumberResponse.Parameters.AddWithValue("@column", nomC);
            getNumberResponse.Parameters.AddWithValue("@label", label);
            getNumberResponse.Parameters.AddWithValue("@table_name", table_name);
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
                    dataBaseConnection.Close();
                    throw new InvalidOperationException("invalide parse number !!");
                }
            }
            dataBaseConnection.Close();
            return nb;
        }
        public int NumberResponseMultipleChoice(String nomC, String label, String table_name)
        {
            SqlConnection dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            SqlCommand getNumberResponse = new SqlCommand("selReponseCheckBoxList", dataBaseConnection);
            getNumberResponse.CommandType = CommandType.StoredProcedure;
            getNumberResponse.Parameters.AddWithValue("@column", nomC);
            getNumberResponse.Parameters.AddWithValue("@label", label);
            getNumberResponse.Parameters.AddWithValue("@table_name", table_name);
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
                    dataBaseConnection.Close();
                    throw new InvalidOperationException("invalide parse number !!");
                }
            }
            dataBaseConnection.Close();
            return nb;
        }

        //WorkShop     
        public object GetWSsubQuestion(Question q, String table_name)
        {
            var d = new List<object>();
            foreach (Question sq in q.SubQuestions)
            {
                d.Add(new { sq = sq.Label, sqid = sq.Id, contrl = sq.ControlType, wsrep = GetWSsubQResponse(sq, table_name) });
            }
            return new { ques = q.Label.ToString(), qCategory = q.Category.ToString(), idq = q.Id, rep = d }; ;
        }
        public object GetWSsubQResponse(Question q, String table_name)
        {
            var atelier = new List<object>();
            var listAtelier = Get_list_ws_atelier(table_name);
            switch (q.ControlType.ToString())
            {
                case "DropDownList":
                    foreach (int num_atelier in listAtelier)
                    {
                        var d = new List<object>();
                        d.Add(new List<object> { "label", "data" });
                        var x = 0;
                        var att = 0;
                        foreach (Choice c in q.Choices)
                        {
                            att = NumberResponseWS(q.Column, c.Label, num_atelier, table_name);
                            x += att;
                            d.Add(new List<object> { c.Label, att });
                        }
                        if (x != 0) { atelier.Add(new { idevent = num_atelier, idq = q.Id, rep = d }); }
                    }
                    break;
                case "RadioButtonList":
                    foreach (int num_atelier in listAtelier)
                    {
                        var d = new List<object>();
                        d.Add(new List<object> { "label", "data" });
                        foreach (Choice c in q.Choices)
                        { d.Add(new List<object> { c.Label, NumberResponseWS(q.Column, c.Label, num_atelier, table_name) }); }
                        if (d.Count != 0) { atelier.Add(new { idevent = num_atelier, idq = q.Id, rep = d }); }
                    }
                    break;
                case "CheckBoxList":
                    //d = new List<object>();
                    //foreach (Choice c in q.Choices)
                    //{
                    //    // d.Add(new object[] { c.Label, NumberResponseMultipleChoice(q.Column, c.Label) });
                    //    // d.Add(new { label = c.Label, data = new int[]   { 10, NumberResponseMultipleChoice(q.Column, c.Label) } } );
                    //}
                    //atelier.Add( new { ques = q.Label.ToString(), type = q.ControlType, idq = q.Id, rep = d });
                    break;

                default:
                    break;
            }

            return atelier;
        }
        public int NumberResponseWS(String nomC, String label, int id_atelier, String table_name)
        {
            SqlConnection dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            SqlCommand getNumberResponse = new SqlCommand("selReponseWS", dataBaseConnection);
            getNumberResponse.CommandType = CommandType.StoredProcedure;
            getNumberResponse.Parameters.AddWithValue("@column", nomC);
            getNumberResponse.Parameters.AddWithValue("@label", label);
            getNumberResponse.Parameters.AddWithValue("@id_atelier", id_atelier);
            getNumberResponse.Parameters.AddWithValue("@table_ws_name", table_name);
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
                    dataBaseConnection.Close();
                    throw new InvalidOperationException("invalide parse number !!");
                }
            }
            dataBaseConnection.Close();
            return nb;

        }
        public List<int> Get_list_ws_atelier(String table_name)
        {
            SqlConnection dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            SqlCommand getNumberResponse = new SqlCommand("selWS", dataBaseConnection);
            getNumberResponse.CommandType = CommandType.StoredProcedure;
            getNumberResponse.Parameters.AddWithValue("@table", table_name);
            var result = getNumberResponse.ExecuteReader();
            var atelier_Event = new List<int>();
            while (result.Read())
            {
                try
                {
                    atelier_Event.Add(int.Parse(result["listevent"].ToString()));
                }
                catch (Exception) { dataBaseConnection.Close(); }
            }
            dataBaseConnection.Close();
            return atelier_Event;
        }

        //Session
        public object GetSessionsubQuestion(Question q, String table_name)
        {
            var d = new List<object>();
            foreach (Question sq in q.SubQuestions)
            {
                d.Add(new { sq = sq.Label, sqid = sq.Id, contrl = sq.ControlType, wsrep = GetSessionsubQResponse(sq, table_name) });
            }
            return new { ques = q.Label.ToString(), qCategory = q.Category.ToString(), idq = q.Id, rep = d }; ;
        }
        public object GetSessionsubQResponse(Question q, String table_name)
        {
            var atelier = new List<object>();
            var listAtelier = Get_list_session_atelier(table_name);
            switch (q.ControlType.ToString())
            {
                case "DropDownList":
                    foreach (int num_atelier in listAtelier)
                    {
                        var d = new List<object>();
                        d.Add(new List<object> { "label", "data" });
                        var x = 0;
                        var att = 0;
                        foreach (Choice c in q.Choices)
                        {
                            att = NumberResponseSession(q.Column, c.Label, num_atelier, table_name);
                            x += att;
                            d.Add(new List<object> { c.Label, att });
                        }
                        if (x != 0) { atelier.Add(new { idevent = num_atelier, idq = q.Id, rep = d }); }
                    }
                    break;
                case "RadioButtonList":
                    foreach (int num_atelier in listAtelier)
                    {
                        var d = new List<object>();
                        d.Add(new List<object> { "label", "data" });
                        foreach (Choice c in q.Choices)
                        { d.Add(new List<object> { c.Label, NumberResponseSession(q.Column, c.Label, num_atelier, table_name) }); }
                        if (d.Count != 0) { atelier.Add(new { idevent = num_atelier, idq = q.Id, rep = d }); }
                    }
                    break;
                case "CheckBoxList":
                    //d = new List<object>();
                    //foreach (Choice c in q.Choices)
                    //{
                    //    // d.Add(new object[] { c.Label, NumberResponseMultipleChoice(q.Column, c.Label) });
                    //    // d.Add(new { label = c.Label, data = new int[]   { 10, NumberResponseMultipleChoice(q.Column, c.Label) } } );
                    //}
                    //atelier.Add( new { ques = q.Label.ToString(), type = q.ControlType, idq = q.Id, rep = d });
                    break;

                default:
                    break;
            }

            return atelier;
        }
        public int NumberResponseSession(String nomC, String label, int id_atelier, String table_name)
        {
            SqlConnection dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            SqlCommand getNumberResponse = new SqlCommand("selReponseSession", dataBaseConnection);
            getNumberResponse.CommandType = CommandType.StoredProcedure;
            getNumberResponse.Parameters.AddWithValue("@column", nomC);
            getNumberResponse.Parameters.AddWithValue("@label", label);
            getNumberResponse.Parameters.AddWithValue("@id_atelier", id_atelier);
            getNumberResponse.Parameters.AddWithValue("@table_ss_name", table_name);
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
                    dataBaseConnection.Close();
                    throw new InvalidOperationException("invalide parse number !!");
                }
            }
            dataBaseConnection.Close();
            return nb;

        }
        public List<int> Get_list_session_atelier(String table_name)
        {
            SqlConnection dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            SqlCommand getNumberResponse = new SqlCommand("selSESSION", dataBaseConnection);
            getNumberResponse.CommandType = CommandType.StoredProcedure;
            getNumberResponse.Parameters.AddWithValue("@table", table_name);
            var result = getNumberResponse.ExecuteReader();
            var atelier_session = new List<int>();
            while (result.Read())
            {
                try
                {
                    atelier_session.Add(int.Parse(result["listevent"].ToString()));
                }
                catch (Exception) { dataBaseConnection.Close(); }
            }
            dataBaseConnection.Close();
            return atelier_session;
        }

        //Meeting
        public object GetMeetingsubQuestion(Question q, String table_name)
        {
            var d = new List<object>();
            foreach (Question sq in q.SubQuestions)
            {
                d.Add(new { sq = sq.Label, sqid = sq.Id, contrl = sq.ControlType, wsrep = GetQMeetingesDataResponse(sq, table_name) });
            }
            return new { ques = q.Label.ToString(), qCategory = q.Category.ToString(), idq = q.Id, rep = d }; ;
        }
        public object GetQMeetingesDataResponse(Question q, String tableName)
        {
            var d = new List<object>();
            { d.Add(new List<object> { "label", "data" }); }
            switch (q.ControlType.ToString())
            {
                case "DropDownList":
                    foreach (Choice c in q.Choices)
                    { d.Add(new List<object> { c.Label, NumberResponseMeeting(q.Column, c.Label, tableName) }); }
                    return new { ctr = q.ControlType, rep = d };
                case "RadioButtonList":
                    foreach (Choice c in q.Choices)
                    { d.Add(new List<object> { c.Label, NumberResponseMeeting(q.Column, c.Label, tableName) }); }
                    return new { ctr = q.ControlType, rep = d };
                case "CheckBoxList":
                    d = new List<object>();
                    foreach (Choice c in q.Choices)
                    {
                        d.Add(new object[] { c.Label, NumberResponseMultipleChoiceMeeting(q.Column, c.Label, tableName) });
                    }
                    return new { ctr = q.ControlType, rep = d };
                default:
                    return null;
            }
        }
        public int NumberResponseMeeting(String nomC, String label, String table_met_name)
        {
            SqlConnection dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            SqlCommand getNumberResponse = new SqlCommand("selReponseMeeting", dataBaseConnection);
            getNumberResponse.CommandType = CommandType.StoredProcedure;
            getNumberResponse.Parameters.AddWithValue("@column", nomC);
            getNumberResponse.Parameters.AddWithValue("@label", label);
            getNumberResponse.Parameters.AddWithValue("@table_met_name", table_met_name);
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
                    dataBaseConnection.Close();
                    throw new InvalidOperationException("invalide parse number !!");
                   
                }
            }
            dataBaseConnection.Close();
            return nb;
        }
        public int NumberResponseMultipleChoiceMeeting(String nomC, String label, String table_met_name)
        {
            SqlConnection dataBaseConnection = ConnexionClasse.getConnexion();
            dataBaseConnection.Open();
            SqlCommand getNumberResponse = new SqlCommand("selReponseCheckBoxListMeeting", dataBaseConnection);
            getNumberResponse.CommandType = CommandType.StoredProcedure;
            getNumberResponse.Parameters.AddWithValue("@column", nomC);
            getNumberResponse.Parameters.AddWithValue("@label", label);
            getNumberResponse.Parameters.AddWithValue("@table_met_name", table_met_name);
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
                    dataBaseConnection.Close();
                    throw new InvalidOperationException("invalide parse number !!");
                }
            }
            dataBaseConnection.Close();
            return nb;
        }

        // Draw Conteners for Data
        public String getPieContener(String titre, String idContenerChart)
        {
            return $@"<div class=""row gutter"">
                        <div class="" col-md-6 col-sm-12 "">
                            <div class=""widget"">
                                <div class=""widget-header"">
                                    <div >{ titre} <a id=""q{idContenerChart} ""></a></div>
                                </div>
                                <div class=""widget-body"">
                                    <div id=""{idContenerChart} "" class=""gnrlques""></div>
                                </div>
                            </div>
                        </div>
                    </div";
        }
        public String getCheckBoxContener(String titre, String idContenerChart)
        {

            return $@"
                      <div class=""row gutter"">
                        <div class=""col-md-6 col-sm-12 "">
                            <div class=""widget"">
                                <div class=""widget-header"">
                                    <div > {titre} <a id=""q{idContenerChart}"" ></a></div>
                                </div>
                                <div class=""widget-body"">
                                    <div id=""{idContenerChart}"" class=""gnrlques""></div>
                                </div>
                            </div>
                        </div>
                    </div>";
        }
        public String getWSQContener(String ques, List<object> rep)
        {

            String str = $@"<div class=""row gutter"">
                                <div class=""col-lg-12 col-sm-12"">
                                    <div class=""widget"">
                                        <div class=""widget-header"">
                                            <div >{ ques}</div>
                                        </div>";

            //for (var j = 0; j < rep.length; j++) {
            //    if (rep[j]["wsrep"].length != 0) {
            //        console.log($rep[j]["wsrep"]);
            //        for (var kk = 0; kk < $rep[j]["wsrep"].length; kk++) {
            //            if ($rep[j]["wsrep"] != null) {
            //        $str += "<div class='col-lg-4 col-sm-6'><div class='widget-body' id=ws" + $rep[j]["wsrep"][kk]["idevent"] + "></div></div>";
            //            }
            //        }
            //    }
            //}

            str += "</div> </div></div>";
            return str;
        }
        public String GetGeneralQuestionContener(int poll_id)
        {
            var grlQuestionContener =
                $@"<!-- contener of General Question -->
                   <div class=""container"">
                       <div class=""General Question's""></div>
                       <div class=""row"" id=""questions"">";


            var workshopContener =
                 $@"<!-- Contener Workshop Questions -->
                   <div class=""container"">
                       <div class=""col-lg-12 col-md-12"" id=""workshop"">
                       </div>
                   </div>";

            var meetingContener =
                $@"<!-- Contener Meetings Questions -->
                   <div class=""container"">
                       <div class=""col-lg-12 col-md-12"" id=""meeting"">
                       </div>
                   </div>";

            var question = new List<object>();
            var manager = new Manager();
            var poll = manager.getPoll(poll_id);
            foreach (Question q in poll.Questions)
            {
                if (q.Category.Equals("General"))
                {
                    if (q.ControlType.Equals("DropDownList"))
                    {
                        grlQuestionContener += getPieContener(q.Label, q.Id.ToString());
                    }
                    else if (q.ControlType.Equals("RadioButtonList"))
                    {
                        grlQuestionContener += getPieContener(q.Label, q.Id.ToString());
                    }
                    else if (q.ControlType.Equals("CheckBoxList"))
                    {
                        grlQuestionContener += getCheckBoxContener(q.Label, q.Id.ToString());
                    }
                    else { }
                }
                else if (q.Category.Equals("Workshop"))
                {
                    workshopContener +=
                        $@"<div class='row'>
                             <div class='widget'>
                                <div class='widget-header'>
                                    <div > {q.Label}</div>
                        </div><div class='row'>";

                }
                else if (q.Category.Equals("Meeting"))
                {
                    foreach (Question sq in q.SubQuestions)
                    {
                        if (sq.ControlType.Equals("DropDownList"))
                        {
                            grlQuestionContener += getPieContener(sq.Label, sq.Id.ToString());
                        }
                        else if (sq.ControlType.Equals("RadioButtonList"))
                        {
                            grlQuestionContener += getPieContener(sq.Label, sq.Id.ToString());
                        }
                        else if (sq.ControlType.Equals("CheckBoxList"))
                        {
                            grlQuestionContener += getCheckBoxContener(sq.Label, sq.Id.ToString());
                        }
                        else { }
                    }
                }
            }
            return grlQuestionContener + "</div></div>";

            //    public String getWSQColapseContener(String ques, String rep)
            //    {

            //        var $str = "<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'>" +
            //                   "<a data-toggle='collapse' data-parent='#accordion' href='#collapseTwo'>" + $ques + "</a></h4>" +
            //                   "</div>" + "<div id='collapseTwo' class='panel-collapse collapse'><div class='panel-body'>";


            //        for (var j = 0; j < $rep.length; j++) {
            //            if ($rep[j] != null) {
            //        $str += "<div id=ws" +$rep[j]["idq"] + "></div>";
            //            }
            //        }
            //$str += "</div></div></div>";
            //        return $str;
            //    }
        }
    }
}
