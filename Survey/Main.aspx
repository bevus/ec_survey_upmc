<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="survey.Main" %>
<%@ Import Namespace="SurveyModel" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="Content/bootstrap.min.css"/>
    <link rel="stylesheet" href="Content/surveyStyle.css"/>
    <script src="scripts/jquery-3.1.1.js"></script>
    <script src="scripts/bootstrap.js"></script>
    <style type="text/css">
        .configurationOption {
            height: 119px;
        }
    </style>
</head>
<body>
    <div class="container">
        <form id="form1" runat="server">
        <h3>Survey list</h3>
        <table class="table">
            <thead>
                <tr>
                    <th>event id</th>
                    <th>Survey name</th>
                    <th>Survey description</th>
                    <th>Survey external id</th>
                </tr>
            </thead>
            <asp:Repeater ID="Repeater1" runat="server" DataSourceID="db_meta_survey" OnItemCommand="Repeater1_ItemCommand">
                <ItemTemplate>
                    <tr class="selectablePoll" data-poll-id='<%# Eval("Pol_id_poll") %>' data-poll_external-id='<%# Eval("POL_external_id") %>'>
                        <td><%# Eval("POL_id_event") %></td>
                        <td><%# Eval("POL_name") %></td>
                        <td><%# Eval("POL_description") %></td>
                        <td><%# Eval("POL_external_id") %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <asp:SqlDataSource ID="db_meta_survey" runat="server" ConnectionString="<%$ ConnectionStrings:meta_surveyConnectionString %>" SelectCommand="SELECT [POL_start_date], [POL_end_date], [POL_name], [POL_description], [POL_external_id], [POL_id_event], [POL_id_poll] FROM [POLL]"></asp:SqlDataSource>
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>
<div class="modal fade" id="configurationModal" tabindex="-1" role="dialog">
    <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title">Configuration</h4>
            </div>
            <div class="modal-body">
                <div class="configurationForm">
                    <asp:HiddenField ID="pollId" runat="server"/>
                    <asp:HiddenField ID="pollExternalId" runat="server"/>
                    <asp:HiddenField runat="server" ID="showModal"/>
                    <asp:Label ID="_pollId" runat="server" Visible="false" CssClass="has-error"/>
                    <legend>Generaton options</legend>
                    <div class="row">
                        <div class="form-group col-md-5">
                            <label class="control-label" for="">Survey file name</label>
                            <asp:TextBox placeholder="without extention and valide c# identifier" ID="surveyFileName" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Label ID="_surveyFileName" runat="server" Visible="false" CssClass="has-error"></asp:Label>
                        </div>
                        <div class="form-group col-md-5 col-md-offset-2">
                            <label class="control-label" for="">404 page</label>
                            <asp:TextBox ID="page404" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Label ID="_page404" runat="server" Visible="false" CssClass="has-error"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-5">
                            <label class="control-label" for="">Dashboard file name</label>
                            <asp:TextBox placeholder="without extention and valide c# identifier" ID="dashboardFileName" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Label ID="_dashboardFileName" runat="server" Visible="false" CssClass="has-error"></asp:Label>
                        </div>
                        <div class="form-group col-md-5 col-md-offset-2">
                            <label class="control-label" for="">Authentification Type</label>
                            <asp:DropDownList ID="authType" CssClass="form-control" runat="server">
                                <asp:ListItem value='<% = (int)AuthentificationType.IdInUrl %>'> id in url </asp:ListItem>
                                <asp:ListItem value='<% = (int)AuthentificationType.HashedIdinUrl %>'> hashed id in url </asp:ListItem>
                                <asp:ListItem value='<% = (int)AuthentificationType.IdInSession %>'> id in session </asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="_authType" runat="server" Visible="false" CssClass="has-error"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-5">
                            <div class="row">
                                <label class="control-label col-md-8" for="">disable data extraction</label>
                                <asp:CheckBox ID="noDataExtarction" CssClass="col-md-4" runat="server"></asp:CheckBox>
                                <asp:Label ID="_noDataExtarction" runat="server" Visible="False" CssClass="has-error"></asp:Label>
                            </div>
                            <div class="row">
                                <label class="control-label col-md-8" for="">do not generate dashboard</label>
                                <asp:CheckBox ID="noDashboard" CssClass="col-md-4" runat="server"></asp:CheckBox>
                                <asp:Label ID="_noDashboard" runat="server" Visible="False" CssClass="has-error"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-5 col-md-offset-2">
                            <label class="control-label" for="">Person id argument name</label>
                            <asp:TextBox ID="personIdArg" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Label ID="_personIdArg" runat="server" Visible="false" CssClass="has-error"></asp:Label>
                        </div>
                    </div>
                    <legend>Survey form options</legend>
                    <div class="row">
                        <div class="form-group col-md-5">
                            <label class="control-label" for="">Option question error message</label>
                            <asp:TextBox ID="optoinQuestionErrorMessage" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Label ID="_optoinQuestionErrorMessage" runat="server" Visible="false" CssClass="has-error"></asp:Label>
                        </div>
                        <div class="form-group col-md-5 col-md-offset-2">
                            <label class="control-label" for="">Text question error message</label>
                            <asp:TextBox ID="textQuestionErrorMessage" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Label ID="_textQuestionErrorMessage" runat="server" Visible="false" CssClass="has-error"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-5">
                            <label class="control-label" for="">Checkbox question error message</label>
                            <asp:TextBox ID="checkboxQuestionErrorMessage" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Label ID="_checkboxQuestionErrorMessage" runat="server" Visible="false" CssClass="has-error"></asp:Label>
                        </div>
                        <div class="form-group col-md-5 col-md-offset-2">
                            <label class="control-label" for="">Date time question error message</label>
                            <asp:TextBox ID="dateTimeQuestionErrorMessage" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Label ID="_dateTimeQuestionErrorMessage" runat="server" Visible="false" CssClass="has-error"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-5">
                            <label class="control-label" for="">Answer length error message</label>
                            <asp:TextBox ID="answerLengthErrorMessage" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Label ID="_answerLengthErrorMessage" runat="server" Visible="false" CssClass="has-error"></asp:Label>
                        </div>
                        <div class="form-group col-md-5 col-md-offset-2">
                            <label class="control-label" for="">Css file name</label>
                            <asp:TextBox ID="cssFileName" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Label ID="_cssFileName" runat="server" Visible="false" CssClass="has-error"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-5">
                            <label class="control-label" for="">Character counter text</label>
                            <asp:TextBox ID="characterCounterText" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Label ID="_characterCounterText" runat="server" Visible="false" CssClass="has-error"></asp:Label>
                        </div>
                        <%--<div class="form-group col-md-5 col-md-offset-2">
                            <label class="control-label" for="">Css file name</label>
                            <asp:TextBox ID="TextBox2" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Label ID="Label2" runat="server" Visible="false" CssClass="has-error"></asp:Label>
                        </div>--%>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-5">
                            <label class="control-label" for="">Submission button text</label>
                            <asp:TextBox ID="submissionButtonText" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Label ID="_submissionButtonText" runat="server" Visible="false" CssClass="has-error"></asp:Label>
                        </div>
                        <div class="form-group col-md-5 col-md-offset-2">
                            <label class="control-label" for="">Save button Text</label>
                            <asp:TextBox ID="saveButtonText" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Label ID="_saveButtonText" runat="server" Visible="false" CssClass="has-error"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 text-center">
                            <hr>
                            <button class="btn btn-primary">generate</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <asp:LinkButton runat="server">Genetated</asp:LinkButton>
</div>
    </form>
    </div>
    <script>
        $(function () {
            <% if(!string.IsNullOrEmpty(showModal.Value)) { %>
                $("#configurationModal").modal("show");
            <% } %>
        $(".selectablePoll")
                .click(function() {
                    $("#pollId").val($(this).attr("data-poll-id"));
                    $("#pollExternalId").val($(this).attr("data-poll_external-id"));
                    initConfigForm($(this).attr("data-poll_external-id"));
                    $("#configurationModal").modal("show");
                });
        });

        function initConfigForm(externalID) {
            $("#surveyFileName").val("Survey_" + externalID);
            $("#dashboardFileName").val("Dashboard_" + externalID);

        }
    </script>
    </body>
</html>
