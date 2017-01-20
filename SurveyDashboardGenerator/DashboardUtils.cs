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
        private SqlConnection dataBaseConnection;
        public DashboardUtils()
        {
            dataBaseConnection = ConnexionClasse.getConnexion();
        }



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
                        d.Add(new object[] { c.Label, NumberResponseMultipleChoice(q.Column, c.Label) });
                        // d.Add(new { label = c.Label, data = new int[]   { 10, NumberResponseMultipleChoice(q.Column, c.Label) } } );
                    }
                    return new { ques = q.Label.ToString(), qCategory = "General", type = q.ControlType, idq = q.Id, rep = d };

                default:
                    return null;
            }
        }

        public List<object> GetQesDataResponse(Question q, String tableName)
        {
            var d = new List<object>();
            { d.Add(new List<object> { "label", "data" }); }
            switch (q.ControlType.ToString())
            {
                case "DropDownList":
                    foreach (Choice c in q.Choices)
                    { d.Add(new List<object> { c.Label, NumberResponseGeneral(q.Column, c.Label, tableName) }); }
                    return d;

                case "RadioButtonList":
                    foreach (Choice c in q.Choices)
                    { d.Add(new List<object> { c.Label, NumberResponseGeneral(q.Column, c.Label, tableName) }); }
                    return d ;

                case "CheckBoxList":
                    d = new List<object>();
                    foreach (Choice c in q.Choices)
                    {
                        d.Add(new object[] { c.Label, NumberResponseMultipleChoice(q.Column, c.Label) });
                        // d.Add(new { label = c.Label, data = new int[]   { 10, NumberResponseMultipleChoice(q.Column, c.Label) } } );
                    }
                    return  d ;

                default:
                    return null;
            }
        }

        public int NumberResponseGeneral(String nomC, String label, String table_name)
        {
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
                    throw new InvalidOperationException("invalide parse number !!");
                }
            }
            dataBaseConnection.Close();
            return nb;
        }

        public int NumberResponseMultipleChoice(String nomC, String label)
        {
            dataBaseConnection.Open();
            SqlCommand getNumberResponse = new SqlCommand("selReponseCheckBoxList", dataBaseConnection);
            getNumberResponse.CommandType = CommandType.StoredProcedure;
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
                catch (Exception)
                {
                    dataBaseConnection.Close();
                    throw new InvalidOperationException("invalide parse number !!");
                }
            }
            dataBaseConnection.Close();
            return nb;
        }




        public int NumberResponseWS(String nomC, String label, int id_atelier, String table_name)
        {
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

        public object GetWSsubQResponse(Question q, String table_name)
        {
            var atelier = new List<object>();



            switch (q.ControlType.ToString())
            {
                case "DropDownList":
                    foreach (int num_atelier in get_list_atelier(table_name))
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
                    foreach (int num_atelier in get_list_atelier(table_name))
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

        public object GetWSsubQuestion(Question q, String table_name)
        {
            var d = new List<object>();
            foreach (Question sq in q.SubQuestions)
            {
                d.Add(new { sq = sq.Label, sqid = sq.Id, contrl = sq.ControlType, wsrep = GetWSsubQResponse(sq, table_name) });


            }

            return new { ques = q.Label.ToString(), qCategory = q.Category.ToString(), idq = q.Id, rep = d }; ;
        }

        public List<int> get_list_atelier(String table_name)
        {
            dataBaseConnection.Open();
            SqlCommand getNumberResponse = new SqlCommand("selWS", dataBaseConnection);
            getNumberResponse.CommandType = CommandType.StoredProcedure;
            getNumberResponse.Parameters.AddWithValue("@table",table_name);
            var result = getNumberResponse.ExecuteReader();
            var atelier_Event = new List<int>();
            while (result.Read())
            {
                try
                {
                    atelier_Event.Add(int.Parse(result["listevent"].ToString()));
                }
                catch (Exception) { }
            }
            dataBaseConnection.Close();
            return atelier_Event;
        }


        // Draw Conteners for Data

        // contener of data
        public String getPieContener(String titre, String idContenerChart)
        {
            return "<div class=' col-lg-6 col-sm-12 '>\n" +
                        "<div class='widget'>\n" +
                            "<div class='widget-header'>\n" +
                                "<div >" + titre + "<a id='q" + idContenerChart + "'></a></div>\n" +
                            "</div>\n" +
                            "<div class='widget-body'>\n" +
                                "<div id='" + idContenerChart + "' class='gnrlques'></div>\n" +
                            "</div>\n" +
                        "</div>\n" +
                    "</div>\n";
        }
        public String getCheckBoxContener(String titre, String idContenerChart)
        {

            return "<div class=' col-md-6 col-sm-12 '>\n" +
                        "<div class='widget'>\n" +
                            "<div class='widget-header'>\n" +
                                "<div >" + titre + "<a id=q'" + idContenerChart + "' ></a></div>\n" +
                            "</div>\n" +
                            "<div class='widget-body'>\n" +
                                "<div id=" + idContenerChart + "></div>\n" +
                            "</div>\n" +
                        "</div>\n" +
                    "</div>\n";
        }
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
        public String getWSQContener(String ques, List<object> rep)
        {

            String str = "<div class='col-lg-12 col-sm-12'>" +
                            "<div class='widget'>" +
                                "<div class='widget-header'>" +
                                    "<div >" + ques + "</div>" +
                             "</div>";

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
    
           str += "</div></div>";
            return str;
        }

        public String GetGeneralQuestionContener(int poll_id)
        {
            var question = new List<object>();
            var manager = new Manager();
            var poll = manager.getPoll(poll_id);
            String qContener = "";
            foreach (Question q in poll.Questions)
            {
                if (q.Category.ToString().Equals("General"))
                {
                    if (q.ControlType.ToString().Equals("DropDownList"))
                    {
                        qContener += getPieContener(q.Label, q.Id.ToString());
                    }
                    else if (q.ControlType.ToString().Equals("RadioButtonList"))
                    {
                        qContener += getPieContener(q.Label, q.Id.ToString());
                    }
                    else if (q.ControlType.ToString().Equals("CheckBoxList"))
                    {
                        qContener += getCheckBoxContener(q.Label, q.Id.ToString());
                    }
                    else { }
                }
            }
            return qContener;
        }
    }
}
