using System;
using System.Data.SqlClient;

public class ConnexionClasse
{
    public ConnexionClasse() { }
    public static SqlConnection getConnexion()
    {
        return new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=meta_survey;Integrated Security=True;");
    }
}
