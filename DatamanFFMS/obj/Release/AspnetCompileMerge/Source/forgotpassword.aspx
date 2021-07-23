<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forgotpassword.aspx.cs" Inherits="AstralFFMS.forgotpassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password</title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <!-- Bootstrap 3.3.4 -->
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="Content/style.css" rel="stylesheet" />

    <!-- Theme style -->
    <link href="dist/css/AdminLTE.css" rel="stylesheet" />
    <!-- AdminLTE Skins. Choose a skin from the css/skins
         folder instead of downloading all of them to reduce the load. -->
    <link href="dist/css/skins/_all-skins.min.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <body>
        <form runat="server" id="form1" defaultfocus="txtLoginId">>
            <body class="login-page">
                <div class="login-box">
                    <div class="login-logo">
                    </div>
                    <!-- /.login-logo -->
                    <div class="login-box-body">
                        <div id="ffmslogin1">
                          <%--  <img src="img/logo-astral.png" width="75%" /><br />--%>
                             <asp:Image ID="Image1" runat="server" width="17%" height="50%" class="img-circle" alt="User Image"/>         
                            <a href="#"><b>
                                <h3>Forgot Password</h3>
                            </b></a>
                            <%--<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>--%>
                        </div>
                        <div class="form-group has-feedback">
                            <div id="BootstrapErrorMessage" runat="server" class="alert alert-danger">
                                <asp:Label ID="errormsglabel" runat="server" Text="Label"></asp:Label>
                            </div>

                        </div>
                        <div class="form-group has-feedback">
                            <div id="BootstrapSuccessMessage" runat="server" class="alert alert-success">
                                <asp:Label ID="successLabel1" runat="server" Text="Label"></asp:Label>
                            </div>
                        </div>
                        <asp:Label ID="Label1" runat="server" Text="Label" ForeColor="Wheat"></asp:Label>
                        <div class="form-group has-feedback">
                        <asp:TextBox ID="txtLoginId" runat="server" placeholder="Enter Your Login Id" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLoginId" ForeColor="Red"
                            ErrorMessage="Login id is required!" SetFocusOnError="True" Display="Dynamic" CssClass="alert-danger" ValidationGroup="pwd" />
                        <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                    </div>

                        <div class="form-group has-feedback">
                            <asp:TextBox ID="txtEmailId" runat="server" placeholder="Enter your email" class="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="newPwdReq" runat="server" ControlToValidate="txtEmailId" ForeColor="Red"
                                ErrorMessage="Email Id is required!" SetFocusOnError="True" Display="Dynamic" CssClass="alert-danger" ValidationGroup="pwdchange" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmailId"
                                ErrorMessage="Invalid Email Address" Display="Dynamic" CssClass="alert-danger" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                ValidationGroup="pwdchange"></asp:RegularExpressionValidator>
                            <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                        </div>

                        <div class="row">
                            <div class="col-xs-6">
                                <asp:Button ID="btnchangePassword" runat="server" Text="Enter" class="btn btn-primary btn-block btn-flat"
                                    ValidationGroup="pwdchange" OnClick="btnchangePassword_Click" />
                            </div>
                            <div class="col-xs-6">
                                <asp:Button ID="Button1" runat="server" Text="Cancel" class="btn btn-primary btn-block btn-flat" OnClick="Button1_Click" />
                            </div>

                        </div>

                    </div>
            </body>
        </form>
    </body>
</body>
</html>
