<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="changepassword.aspx.cs" Inherits="AstralFFMS.changepassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Password</title>
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
<body>
    <form runat="server" id="form1" defaultfocus="txtNewPassword">
        <body class="login-page">
            <div class="login-box">
                <div class="login-logo">
                </div>
                <!-- /.login-logo -->
                <div class="login-box-body">
                    <div id="ffmslogin1">
                       <%-- <img src="img/logo-astral.png" width="75%" /><br />--%>
                         <asp:Image ID="Image1" runat="server" width="17%" height="50%" class="img-circle" alt="User Image"/>         
                        <a href="#"><b>
                            <h3>Change Password</h3>
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
                            <asp:Label ID="successmsglabel" runat="server" Text="Label"></asp:Label>
                        </div>
                    </div>
                    <asp:Label ID="Label1" runat="server" Text="Label" ForeColor="Wheat"></asp:Label>
                    <div class="form-group has-feedback">
                        <asp:TextBox ID="txtNewPassword" runat="server" placeholder="Enter new password" class="form-control" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="newPwdReq" runat="server" ControlToValidate="txtNewPassword" ForeColor="Red"
                            ErrorMessage="New Password is required!" SetFocusOnError="True" Display="Dynamic" CssClass="alert-danger" ValidationGroup="pwd" />
                        <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                    </div>
                    <div class="form-group has-feedback">
                        <asp:TextBox ID="txtConfirmPassword" runat="server" placeholder="Confirm password" class="form-control" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="confirmPwdReq" runat="server" ControlToValidate="txtConfirmPassword" ForeColor="Red" ValidationGroup="pwd"
                            ErrorMessage="Password is required!" Display="Dynamic" SetFocusOnError="True" CssClass="alert-danger" />
                        <asp:CompareValidator ID="comparePasswords" runat="server" ControlToCompare="txtNewPassword"
                            ControlToValidate="txtConfirmPassword" ErrorMessage="Your passwords do not match up!" ForeColor="Red" ValidationGroup="pwd"
                            Display="Dynamic" CssClass="alert-danger" />
                        <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                    </div>
                    <div class="row">
                        <div class="col-xs-6">
                            <asp:Button ID="btnchangePassword" runat="server" Text="Change Password" class="btn btn-primary btn-block btn-flat"
                                OnClick="btnchangePassword_Click" ValidationGroup="pwd" />
                        </div>
                        <div class="col-xs-6">
                            <asp:Button ID="Button1" runat="server" Text="Cancel" class="btn btn-primary btn-block btn-flat"  /><br />
                            <div style="float:right;">
                             <asp:LinkButton ID="LinkButton2" runat="server" ForeColor="White"  Font-Underline="true" PostBackUrl="~/Home.aspx">Back!</asp:LinkButton>
                        </div>
                        </div>
                        
                    </div>

                </div>
        </body>
    </form>
</body>
</html>
