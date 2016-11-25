using DataAccess;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using SurveyModel;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using SurveyFormGenerator;

namespace survey
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var manager = new Manager();
            var formGenerator = new FormGenerator(Page.MapPath("~/surveys/"), manager.getPoll(0));
            surveyLink.Target = formGenerator.GenerateWebForm();
        }
    }
}