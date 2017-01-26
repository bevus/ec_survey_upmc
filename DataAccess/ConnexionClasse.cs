using System;
using System.Data.SqlClient;
using System.Configuration;

public class ConnexionClasse
{
    public ConnexionClasse() { }
    public static SqlConnection getConnexion()
    {
        //return new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=meta_survey;Integrated Security=True");
        return new SqlConnection(ConfigurationManager.ConnectionStrings["meta_surveyConnectionString"].ConnectionString);
    }
}
