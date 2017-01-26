<%@ Page Language="C#" AutoEventWireup="true" 
CodeFile="Survey_B1AB75ED32D74914B015B05C2EB3081E.aspx.cs" Inherits="Survey.surveys.Survey_B1AB75ED32D74914B015B05C2EB3081E"
MasterPageFile="../SurveyMasterPage.master"%>
    <asp:Content runat="server" contentplaceholderid="scripts">
        <link href="../Content/surveyStyle.css" rel="stylesheet"/>
        <script src="../scripts/jquery-3.1.1.js"></script>
        <script src="../scripts/jquery.validate.min.js"></script>
        <script src="../scripts/bootstrap.js"></script>
        <script src="../scripts/jquery.validation_1.15.0_additional-methods.js"></script>
        <script src="../scripts/textcounter.min.js"></script>
        <script src="../scripts/customScript.js"></script>
        <style>
            #toast {
                margin-left: -100px; 
                background-color: #999; 
                color: #fff; 
                width: 200px;
                text-align: center; 
                border-radius: 2px; 
                padding: 20px 0px; 
                position: fixed; 
                z-index: 1; 
                left: 50%; 
                bottom: 30px; 
                box-shadow: 0 0px 5px 0 #eee;
            }
        </style>
    </asp:Content>
    <asp:Content runat="server" contentplaceholderid="surveyFormPlaceHolder">
        <h1 class="page-header surveyName">Template Survey</h1>
        <form class="surveyForm" id="surveyForm" runat="server">
            <div id="questions" runat="server">
            </div>
        </form>
        <% if (showTaost){ %>
        <div id="toast">
            <i class="glyphicon glyphicon-saved"></i> Saved
        </div>
        <% } %>
        <script>
            function showToast() {
                $("#toast").fadeIn(200);
            }

            function hideToast(parameters) {
                $("#toast").fadeOut(200);
            }
            $(function () {
                <% if (showTaost){ %>
                setTimeout(hideToast, 1500);
            <% } %>
            $("textarea").addClass("form-control");
            $("select, input[type=text], input[type=datetime], input[type=date], input[type=email], input[type=telephone], input[type=password], input[type=number]").addClass("form-control");
            $("form input[type=submit]").wrapAll('<div class="col-md-12" id="buttons"/>');
            $("#buttons").wrapAll('<div class="row"/>')
            $.each($("input[type=radio], input[type=checkbox]"), function (i, e) {
                $(e).add($("label[for='" + e.id + "']")).wrapAll('<div class="choice"/>');
            });
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
