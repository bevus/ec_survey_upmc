<%@ Page Title="Title" Language="C#" MasterPageFile="SurveyMasterPage.master" %>
<%@ Import Namespace="SurveyModel" %>
<script runat="server">

    private void viewSurveyAction(object sender, EventArgs e)
    {
        var url = Request.QueryString["sUrl"];
        var authMod = -1;
        try
        {
            authMod = int.Parse(Request.QueryString["authMod"]);
        }
        catch (Exception){}
        switch ((AuthentificationType)authMod)
        {
            case AuthentificationType.IdInUrl:
                url += personId.Text;
                break;
            case AuthentificationType.HachedIdInUrl:
                url += personId.Text;
                break;
            case AuthentificationType.IdInSession:
                Session[Request.QueryString["argName"]] = personId.Text;
                break;
            default:
                return;
        }
        Response.Redirect(url);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["dUrl"]))
        {
            dashboardLink.NavigateUrl = Request.QueryString["dUrl"];
            dashboardLink.Visible = true;
        }
        var authMod = -1;
        try
        {
            authMod = int.Parse(Request.QueryString["authMod"]);
        }
        catch (Exception){}
        switch ((AuthentificationType)authMod)
        {
            case AuthentificationType.IdInUrl:
                idLabel.Text = Request.QueryString["argName"] + " (id in url)";
                break;
            case AuthentificationType.HachedIdInUrl:
                idLabel.Text = Request.QueryString["argName"] + " (hached id in url)";
                break;
            case AuthentificationType.IdInSession:
                idLabel.Text = Request.QueryString["argName"] + " (id in session)";
                break;
        }
    }

</script>
<asp:Content runat="server" ID="Scripts" ContentPlaceHolderID="scripts">
    <style>
        #personId {
            width: 40%;
        }
    </style>
</asp:Content>
<asp:Content runat="server" ID="SurveyForm" ContentPlaceHolderID="surveyFormPlaceHolder">
    <h1 class="page-header">Generation completed</h1>
    <h3>Poll "<%= Request.QueryString["pollName"] %>" is generated</h3>
    <h4><asp:HyperLink Visible="False" runat="server" ID="dashboardLink">view dashboard</asp:HyperLink></h4>
    <form runat="server" class="container">
        <div class="form-group">
            <asp:Label runat="server" ID="idLabel"></asp:Label>
            <asp:TextBox placeholder="personid" CssClass="form-controt" runat="server" ID="personId"></asp:TextBox>
            <asp:Button OnClick="viewSurveyAction" CssClass="btn btn-success" runat="server" Text="View survey form" ID="viewSurvey"/>
        </div>
    </form>
</asp:Content>

