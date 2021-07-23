<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs" Inherits="AstralFFMS.LogIn" %>

<!DOCTYPE html>

<head>
    <meta charset="UTF-8">
    <title>LogIn</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <!-- Bootstrap 3.3.4 -->
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-2.1.4.min.js"></script>


    <!-- Font Awesome Icons -->
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <!-- Ionicons -->
    <link href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" rel="stylesheet" type="text/css" />

    <!-- Custom Style Sheet  -->
    <link href="Content/style.css" rel="stylesheet" />

    <!-- Theme style -->
    <link href="dist/css/AdminLTE.css" rel="stylesheet" />
    <!-- AdminLTE Skins. Choose a skin from the css/skins
         folder instead of downloading all of them to reduce the load. -->
    <link href="dist/css/skins/_all-skins.min.css" rel="stylesheet" type="text/css" />

    <!-- jQuery 2.1.4 -->
    <script src="plugins/jQuery/jQuery-2.1.4.min.js" type="text/javascript"></script>
    <!-- Bootstrap 3.3.2 JS -->

    <script src="Scripts/bootstrap.min.js"></script>
    <!-- FastClick -->

    <script src="plugins/fastclick/fastclick.min.js" type="text/javascript"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/app.min.js" type="text/javascript"></script>
    <!-- AdminLTE for demo purposes -->
    <script src="dist/js/demo.js" type="text/javascript"></script>
      
</head>
<form runat="server" defaultfocus="txtUserID">
    <body class="login-page">
        <div class="login-box">
            <div class="login-logo">
            </div>
            <!-- /.login-logo -->
            <div class="login-box-body">
                <div id="ffmslogin1">
                    <%--img src="img/logo-astral.png" width="75%" />--%>
                    <%--For Astral--%>
                    <%--<asp:Image ID="Image1" runat="server" width="75%"/>--%>
                    <%--For Mohani--%>
                     <asp:Image ID="Image1" runat="server" width="17%" height="50%" class="img-circle" alt="User Image"/>            
                 </div>

                <div class="form-group has-feedback">
                    <div id="BootstrapErrorMessage" runat="server" class="alert alert-danger">
                        <asp:Label ID="errormsglabel" runat="server" Text="Label"></asp:Label>
                    </div>
                </div>
                <div class="form-group has-feedback">
                    <asp:TextBox ID="txtUserID" runat="server" placeholder="User ID" class="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="Please enter username" CssClass="alert-danger"
                        Display="Static" SetFocusOnError="true" ForeColor="Red" ValidationGroup="login" ControlToValidate="txtUserID"></asp:RequiredFieldValidator>
                    <span class="glyphicon glyphicon-user form-control-feedback"></span>
                </div>
                <div class="form-group has-feedback">
                    <asp:TextBox ID="txtPassword" runat="server" placeholder="Password" class="form-control" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPwd" runat="server" ErrorMessage="Please enter password" CssClass="alert-danger"
                        Display="Static" SetFocusOnError="true" ForeColor="Red" ValidationGroup="login" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                    <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                    <br />
                    <asp:LinkButton ID="LinkButton2" runat="server" ForeColor="White" Font-Underline="true" PostBackUrl="~/forgotpassword.aspx">Forgot Password!</asp:LinkButton>
                </div>
                <div class="col-xs-6">
                </div>
                <div class="row">
                    <div class="col-xs-8"></div>
                    <div class="col-xs-4">
                        <asp:Button ID="btnSubmit" runat="server" Text="Sign In" class="btn btn-primary btn-block btn-flat" ValidationGroup="login" OnClick="btnSubmit_Click" />

                    </div>
                    <!-- /.col -->
                    <%--<div class="col-xs-6">
                        <asp:LinkButton ID="LinkButton1" runat="server" ForeColor="White" Font-Underline="true">Forgot Password!</asp:LinkButton>
                    </div>--%>
                    <!-- /.col -->
                </div>

            </div>
    </body>
</form>

