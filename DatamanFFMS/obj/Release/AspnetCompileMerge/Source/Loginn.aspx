<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Loginn.aspx.cs" Inherits="AstralFFMS.Loginn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>

    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!--===============================================================================================-->
    <%--	<link rel="icon" type="image/png" href="LoginAssets/images/icons/favicon.ico"/>--%>
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="LoginAssets/vendor/bootstrap/css/bootstrap.min.css">
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="LoginAssets/fonts/font-awesome-4.7.0/css/font-awesome.min.css">
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="LoginAssets/fonts/Linearicons-Free-v1.0.0/icon-font.min.css">
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="LoginAssets/vendor/animate/animate.css">
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="LoginAssets/vendor/css-hamburgers/hamburgers.min.css">
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="LoginAssets/vendor/animsition/css/animsition.min.css">
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="LoginAssets/vendor/select2/select2.min.css">
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="LoginAssets/vendor/daterangepicker/daterangepicker.css">
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="LoginAssets/css/util.css">
    <link rel="stylesheet" type="text/css" href="LoginAssets/css/main.css">
    <!--===============================================================================================-->


    <style>
        .input:-webkit-autofill, textarea:-webkit-autofill, select:-webkit-autofill {
            background-color: #fff !important;
            background-image: none !important;
            color: rgb(255,255,255) !important;
        }
    </style>
</head>

<body>

    <div class="limiter">
        <div class="container-login100" style="background-image: url('LoginAssets/images/home_bg.gif'); background-size: cover;">
            <div class="wrap-login100 p-t-30 p-b-50">
                <span class="login100-form-title p-b-41">
                    <img src="LoginAssets/images/grahaak_logo_web.png" />
                </span>
                <form class="login100-form validate-form p-b-33 p-t-5" runat="server" defaultfocus="username">

                    <div class="wrap-input100 validate-input" data-validate="Enter username">
                        <%--<input  type="text" name="username" placeholder="User name"  required/>--%>
                        <input class="input100" type="text"  data-val="true" runat="server" placeholder="Enter Username Here" data-val-required="The Email field is required." id="username" name="username" value=""/>
                        <span class="focus-input100" data-placeholder="&#xe82a;"></span>
                    </div>

                    <div class="wrap-input100 validate-input" data-validate="Enter password">
                        <%--<input type="password" name="pass" placeholder="Password" required />--%>
                         <input class="input100"  type="password" data-val="true" data-val-required="The Password field is required." runat="server" id="txtpwd" placeholder="Enter Password Here" name="password"/>
                        <span class="focus-input100" data-placeholder="&#xe80f;"></span>
                    </div>
                     <div class="container-login100-form-btn m-t-10">
                          <label class="login-label" for="Password" id="lbltxt" runat="server" visible="false" style="color:red"></label>
                    </div>
                   
                    <div class="container-login100-form-btn m-t-10">
                       <%-- <button class="login100-form-btn">
                            Login
                        </button>--%>
                        <%--	<asp:Button  runat="server" ID="btnSubmit" Text="Log in" class="login100-form-btn"  OnClick="btnSubmit_Click1" />--%>
                        <asp:Button runat="server" ID="btnsub" CssClass="login100-form-btn" OnClick="btnsub_Click" Text="Login" />
                         <button type="button" hidden="hidden" class="login100-form-btn " style="margin-left:10px;" data-toggle="modal" data-target="#myModal">Demo Login Info</button>
                    </div>
                     
                    <div class="container-login100-form-btn m-t-10">
                        <a href="pedforgot.aspx">Forgot your password?</a>
                        <asp:LinkButton ID="lnkReset" runat="server" CssClass="linkbutton" ForeColor="#337ab7" Font-Underline="true" OnClick="lnkReset_Click"  CausesValidation="False" Visible="false" >Reset Data</asp:LinkButton>
                    </div>
                </form>
            </div>
        </div>
    </div>

     <!-- Modal -->
  <div class="modal fade" id="myModal" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <h4 class="modal-title">Demo Login Info</h4>
        </div>
        <div class="modal-body">
            <table class="table table-condensed">
                <thead>
                    <tr >
                        <th>User Name</th>
                        <th>Password</th>
                        <th>Role</th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="info">
                        <td>SA</td>
                        <td>admin@123</td>
                        <td>ADMIN</td>
                    </tr>
                    <tr class="info">
                        <td>L3UP</td>
                        <td>12345678</td>
                        <td>State Head</td>
                    </tr>
                    <tr class="info">
                        <td>L2KNP</td>
                        <td>12345678</td>
                        <td>District Head</td>
                    </tr>
                    <tr class="info">
                        <td>L1NORTHKNP</td>
                        <td>12345678</td>
                        <td>Area Incharge</td>
                    </tr>
                     <tr class="info">
                        <td>dataman</td>
                        <td>12345678</td>
                        <td>Distributor</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
    <!--===============================================================================================-->
    <script src="LoginAssets/vendor/jquery/jquery-3.2.1.min.js"></script>
    <!--===============================================================================================-->
    <script src="LoginAssets/vendor/animsition/js/animsition.min.js"></script>
    <!--===============================================================================================-->
    <script src="LoginAssets/vendor/bootstrap/js/popper.js"></script>
    <script src="LoginAssets/vendor/bootstrap/js/bootstrap.min.js"></script>
    <!--===============================================================================================-->
    <script src="LoginAssets/vendor/select2/select2.min.js"></script>
    <!--===============================================================================================-->
    <script src="LoginAssets/vendor/daterangepicker/moment.min.js"></script>
    <script src="LoginAssets/vendor/daterangepicker/daterangepicker.js"></script>
    <!--===============================================================================================-->
    <script src="LoginAssets/vendor/countdowntime/countdowntime.js"></script>
    <!--===============================================================================================-->
    <script src="LoginAssets/js/main.js"></script>
     <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
</body>
</html>
