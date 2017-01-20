<%@ Page Title="Title" Language="C#" MasterPageFile="SurveyMasterPage.master" %>
<%@ Import Namespace="SurveyModel" %>
<script runat="server">

    private void viewSurveyAction(object sender, EventArgs e)
    {
        var url = Request.QueryString["sUrl"];
        switch ((AuthentificationType)int.Parse(Request.QueryString["authMod"]))
        {
            case AuthentificationType.IdInUrl:
                url += personId.Text;
                break;
            case AuthentificationType.HashedIdinUrl:
                url += personId.Text;
                break;
            case AuthentificationType.IdInSession:
                Session[Request.QueryString["argName"]] = personId.Text;
                var id = Session[Request.QueryString["argName"]];
                break;
            default:
                return;
        }
        Response.Redirect(url);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        switch ((AuthentificationType)int.Parse(Request.QueryString["authMod"]))
        {
            case AuthentificationType.IdInUrl:
                idLabel.Text = Request.QueryString["argName"] + " (id in url)";
                break;
            case AuthentificationType.HashedIdinUrl:
                idLabel.Text = Request.QueryString["argName"] + " (hashed id in url)";
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
    <h4><a href='<%= Request.QueryString["dUrl"] %>'>view dashboard</a></h4>
    <form runat="server" class="container">
        <div class="form-group">
            <asp:Label runat="server" ID="idLabel"></asp:Label>
            <asp:TextBox placeholder="personid" CssClass="form-controt" runat="server" ID="personId"></asp:TextBox>
            <asp:Button OnClick="viewSurveyAction" CssClass="btn btn-success" runat="server" Text="View survey form" ID="viewSurvey"/>
        </div>
    </form>
</asp:Content>
