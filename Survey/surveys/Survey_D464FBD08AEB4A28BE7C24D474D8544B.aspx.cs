using System;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Survey_D464FBD08AEB4A28BE7C24D474D8544B
{
    public partial class Survey_D464FBD08AEB4A28BE7C24D474D8544B : Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Page.Validate();
                if (Page.IsValid)
                {
                    var dataBaseConnection = ConnexionClasse.getConnexion();
                    dataBaseConnection.Open();
                    SqlCommand saveAnswers = new SqlCommand("ID464FBD08AEB4A28BE7C24D474D8544B", dataBaseConnection);
                    saveAnswers.CommandType = CommandType.StoredProcedure;
                    saveAnswers.Parameters.AddWithValue("@person_id", 0);
                    saveAnswers.Parameters.AddWithValue("@rbl1_a", rbl0.SelectedValue);
saveAnswers.Parameters.AddWithValue("@rbl2_a", rbl3.SelectedValue);
saveAnswers.Parameters.AddWithValue("@rbl1_b", rbl1.SelectedValue);
saveAnswers.Parameters.AddWithValue("@rbl2_b", rbl4.SelectedValue);
saveAnswers.Parameters.AddWithValue("@area1_c", area2.Text);

                    saveAnswers.ExecuteReader();
                    dataBaseConnection.Close();
                }
            }

        }
    }
}