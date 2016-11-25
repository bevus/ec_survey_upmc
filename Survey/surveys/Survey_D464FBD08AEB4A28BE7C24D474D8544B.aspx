<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Survey_D464FBD08AEB4A28BE7C24D474D8544B.aspx.cs" Inherits="Survey_D464FBD08AEB4A28BE7C24D474D8544B.Survey_D464FBD08AEB4A28BE7C24D474D8544B" %>
<!DOCTYPE html> 
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server" > 
        <meta http-equiv = "Content-Type" content="text/html; charset=utf-8"/>
        <title></title>
        <link href="../Content/bootstrap.min.css" rel="stylesheet"/>
        <link href="../Content/surveyStyle.css" rel="stylesheet"/>
        <script src = "../scripts/jquery-3.1.1.min.js"></script>
        <script src = "../scripts/bootstrap.min.js"></script>
    </head>
    <body>
        <form id="form1" runat="server">
            <div class="container">
                <div class="question-block" id="block-0">
                    <h3 class="question-block-title">Earth Quiz: Do You Really Know Your Planet?</h3>
                    <div class="question-block-questions"><div class="row question">
                    <div class=" form-group">
                        <Label for="" class="control-label col-md-12">
                            
<span class="question-number">1.a</span><span class="question-text">What’s Earth’s true shape? </span>
                        </Label>
                        <div class="col-md-10">
                            
<asp:RadioButtonList TextAlign="Left" ID="rbl0" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server">
    <asp:ListItem Selected="False" Value="Flat">Flat</asp:ListItem>
<asp:ListItem Selected="False" Value="Oblate spheroid">Oblate spheroid</asp:ListItem>
<asp:ListItem Selected="False" Value="Sphere">Sphere</asp:ListItem>

</asp:RadioButtonList>
                            
<asp:RequiredFieldValidator runat="server" ID="rfv_rbl0" ErrorMessage="Vous devez donnez une reponse à cette question" ControlToValidate="rbl0" ></asp:RequiredFieldValidator><br/>
                            
                        </div>
                    </div>
                </div><div class="row question">
                    <div class=" form-group">
                        <Label for="" class="control-label col-md-12">
                            
<span class="question-number">1.b</span><span class="question-text">2. 67,000 mph (107,826 km/h) represents what speed? </span>
                        </Label>
                        <div class="col-md-10">
                            
<asp:RadioButtonList TextAlign="Left" ID="rbl1" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server">
    <asp:ListItem Selected="False" Value="A">Earth’s movement through the Milky Way</asp:ListItem>
<asp:ListItem Selected="False" Value="B">Earth’s orbit around the sun</asp:ListItem>
<asp:ListItem Selected="False" Value="C">Earth’s rotation about its axis</asp:ListItem>

</asp:RadioButtonList>
                            
                            
                        </div>
                    </div>
                </div><div class="row question">
                    <div class=" form-group">
                        <Label for="" class="control-label col-md-12">
                            
<span class="question-number">1.c</span><span class="question-text">Any comments about the quiz? </span>
                        </Label>
                        <div class="col-md-10">
                            
<asp:TextBox ID="area2" CssClass="form-control" runat="server" Text="" Rows="5" Columns="70" TextMode="MultiLine"></asp:TextBox>

                            
<asp:RequiredFieldValidator runat="server" ID="rfv_area2" ErrorMessage="Vous devez donnez une reponse à cette question" ControlToValidate="area2" ></asp:RequiredFieldValidator><br/>
                            
                        </div>
                    </div>
                </div>
                        </div>
                    </div>
                <div class="question-block" id="block-1">
                    <h3 class="question-block-title">Solar System Quiz</h3>
                    <div class="question-block-questions"><div class="row question">
                    <div class=" form-group">
                        <Label for="" class="control-label col-md-12">
                            
<span class="question-number">2.a</span><span class="question-text">How many days does it take the earth to travel around the sun?</span>
                        </Label>
                        <div class="col-md-10">
                            
<asp:RadioButtonList TextAlign="Left" ID="rbl3" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server">
    <asp:ListItem Selected="False" Value="1">1</asp:ListItem>
<asp:ListItem Selected="False" Value="30">30</asp:ListItem>
<asp:ListItem Selected="False" Value="365">365</asp:ListItem>

</asp:RadioButtonList>
                            
                            
                        </div>
                    </div>
                </div><div class="row question">
                    <div class=" form-group">
                        <Label for="" class="control-label col-md-12">
                            
<span class="question-number">2.b</span><span class="question-text">Which planet is the smallest</span>
                        </Label>
                        <div class="col-md-10">
                            
<asp:RadioButtonList TextAlign="Left" ID="rbl4" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server">
    <asp:ListItem Selected="False" Value="Pluto">Pluto</asp:ListItem>
<asp:ListItem Selected="False" Value="Mercury">Mercury</asp:ListItem>
<asp:ListItem Selected="False" Value="Mars">Mars</asp:ListItem>

</asp:RadioButtonList>
                            
<asp:RequiredFieldValidator runat="server" ID="rfv_rbl4" ErrorMessage="Vous devez donnez une reponse à cette question" ControlToValidate="rbl4" ></asp:RequiredFieldValidator><br/>
                            
                        </div>
                    </div>
                </div>
                        </div>
                    </div>
                    <div class="question-block">
                        <div class="question-block-questions">
                        </div>
                    </div>
                <hr/>
                <asp:Button  CausesValidation="true" ID="confirm" Text="valider" runat="server" CssClass="btn btn-primary"/>
            </div>
        </form>
</body>
</html>
