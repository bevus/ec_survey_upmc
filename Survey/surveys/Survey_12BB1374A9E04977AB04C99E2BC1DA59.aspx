<%@ Page Language="C#" AutoEventWireup="true" 
CodeFile="Survey_12BB1374A9E04977AB04C99E2BC1DA59.aspx.cs" Inherits="Survey.surveys.Survey_12BB1374A9E04977AB04C99E2BC1DA59"
MasterPageFile="../SurveyMasterPage.master"%>
    <asp:Content runat="server" contentplaceholderid="scripts">
        <script src="..\scripts\jquery-3.1.1.js"></script>
        <script src="..\scripts\jquery.validate.min.js"></script>
        <script src="..\scripts\bootstrap.js"></script>
    </asp:Content>
    <asp:Content runat="server" contentplaceholderid="surveyFormPlaceHolder">
        <h1 class="page-header">Simple Survey</h1>
        <form id="surveyForm" runat="server">
            <div id="questions" runat="server">
            </div>
        </form>
    </asp:Content>
