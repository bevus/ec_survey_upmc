using DataAccess;
using System;

using SurveyFormGenerator;
using SurveyDashboardGenerator;

namespace survey
{
    public partial class MainV0 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //var manager = new Manager();
            //var poll = manager.getPoll(1);
            //var formGenerator = new FormGenerator(Page.MapPath("~/surveys/"), poll);
            //var dashbordGenerator = new DashboardGenerator(Page.MapPath("~/surveys/"), poll,true);
            //surveyLink.Target = formGenerator.GenerateWebForm();
           // dashboardLink.Target = dashbordGenerator.GenerateDashboard();
        }
    }
}