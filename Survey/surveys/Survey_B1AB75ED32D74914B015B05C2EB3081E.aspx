<%@ Page Language="C#" AutoEventWireup="true" 
CodeFile="Survey_B1AB75ED32D74914B015B05C2EB3081E.aspx.cs" Inherits="Survey.surveys.Survey_B1AB75ED32D74914B015B05C2EB3081E"
MasterPageFile="../SurveyMasterPage.master"%>
    <asp:Content runat="server" contentplaceholderid="scripts">
        <link href="../Content/surveyStyle2.css" rel="stylesheet"/>
        <script src="../scripts/jquery-3.1.1.js"></script>
        <script src="../scripts/jquery.validate.min.js"></script>
        <script src="../scripts/bootstrap.js"></script>
        <script src="../scripts/jquery.validation_1.15.0_additional-methods.js"></script>
        <script src="../scripts/textcounter.min.js"></script>
        <script src="../scripts/customScript.js"></script>
    </asp:Content>
    <asp:Content runat="server" contentplaceholderid="surveyFormPlaceHolder">
        <h1 class="page-header">Template Survey</h1>
        <form id="surveyForm" runat="server">
            <div id="questions" runat="server">
            </div>
        </form>
        <script>
        $(function () {
            $.each($("textarea.QuestionControl.CommentsBox"),
            function(i, e) {
                $(e)
                    .textcounter({
                        max: parseInt($(e).attr("data-max")),
                        countDown: true,
                        countDownText: "Remaining: %d"//**********
                    });
            });
            var rules = {};
            var messages = {};
            var checkboxClassCount = 0;
            var checkboxGroupClass = "checkboxGroup";

            $.each($(".QuestionControl.CommentsBox.Mandatory, .QuestionControl.TextBox.Mandatory"), function (i, e) {
                rules[$(e).attr('name')] = { required: true, maxlength: parseInt($(e).attr("data-max")) };
                messages[$(e).attr('name')] = {required: "Please write some text", maxlenght: "Answer too long"}
            });

            $.each($(".QuestionControl.RadioButtonList.Mandatory"), function (i, e) {
                $.each($(e).find("input[type=radio]"), function (j, r) {
                    rules[$(r).attr('name')] = {required: true};
                    messages[$(r).attr('name')] = {required: "Please choose a value"};
                });
            });

            $.each($(".QuestionControl.CheckBoxList.Mandatory"), function (i, e) {
                var currentGroupClass = checkboxGroupClass + checkboxClassCount;
                checkboxClassCount++;
                $(e).find("input[type=checkbox]").addClass(currentGroupClass);
                var isSet = false;
                $.each($(e).find("input[type=checkbox]"), function (j, c) {
                    rules[$(c).attr('name')] = { require_from_group: [parseInt($(e).attr("data-nbchoices")), "." + currentGroupClass] };
                    if (!isSet) {
                        var str = "Please choose at least %d option"
                        messages[$(c).attr('name')] = { require_from_group: str.replace("%d", $(e).attr("data-nbchoices")) };
                        isSet = true;
                    } else {
                        messages[$(c).attr('name')] = { require_from_group: "" };
                    }
                });
            });

            console.log(rules);

            $("#surveyForm").validate({
                errorClass: "has-error",
                validClass: "has-success",
                rules: rules,
                messages: messages,
                errorPlacement: function (error, element) {
                    if (element.is(":radio") || element.is(":checkbox")) {
                        error.appendTo(element.parent().parent());
                    }
                    else {
                        error.insertAfter(element);
                    }
                }
            });
        });
        </script>
    </asp:Content>
