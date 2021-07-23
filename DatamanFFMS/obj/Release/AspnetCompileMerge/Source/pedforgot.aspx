<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pedforgot.aspx.cs" Inherits="AstralFFMS.pedforgot" %>


<!DOCTYPE html>
<!--[if lt IE 7]> <html class="lt-ie9 lt-ie8 lt-ie7" lang="en"> <![endif]-->
<!--[if IE 7]> <html class="lt-ie9 lt-ie8" lang="en"> <![endif]-->
<!--[if IE 8]> <html class="lt-ie9" lang="en"> <![endif]-->
<!--[if gt IE 8]><!--> <html lang="en"> <!--<![endif]-->
<head>
  <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>Forgot Password</title>
      <link href="Content/bootstrap.css" rel="stylesheet" />
  <link rel="stylesheet" href="css/styleNewlogin.css">
       <script type="text/javascript">
           function HideLabel() {
               var seconds = 5;
               setTimeout(function () {
                   document.getElementById("<%=lbltxt.ClientID %>").style.display = "none";
            }, seconds * 1000);
        };
</script>
  <!--[if lt IE 9]><script src="//html5shim.googlecode.com/svn/trunk/html5.js"></script><![endif]-->
</head>
<body>



    <nav class="navbar navbar-default navbar-ffms">
  <div class="container-fluid">
    <div class="navbar-header"><asp:Image ID="Image1" runat="server" alt="Logo"/>
    </div>
    
  </div>
</nav>
    <div class="container body-content">  
<div class="row">
    <div class="col-md-8 loginbox-container">
        <span class="login-text">Forgot Password</span>
        <section>
              	<form class="form-horizontal" runat="server" defaultfocus="txtUserID">
                <div class="form-group inputGroup">
                    <label class="login-label" for="Email">USERNAME</label>
                    <div class="">
                        <%--<input class="form-control" type="text" data-val="true" placeholder="Enter Username Here" data-val-required="The Email field is required." ID="txtuser" name="login">--%>
                         <asp:TextBox ID="txtLoginId" runat="server" placeholder="Enter Your Login Id" class="form-control" data-val="true" data-val-required="The Email field is required."  name="login"></asp:TextBox>
                        <span class="text-danger field-validation-valid" data-valmsg-for="txtUserID" data-valmsg-replace="true"></span>
                    </div>
                </div>

                <div class="form-group inputGroup">
                    <label class="login-label" for="Password">EMAIL</label>
                    <div class="">
                        <%--<input class="form-control" type="password" data-val="true" data-val-required="The Password field is required." runat="server" ID="txtpwd" placeholder="Enter Password Here" name="password">--%>
                         <asp:TextBox ID="txtEmailId" runat="server" placeholder="Enter your email" class="form-control" data-val="true"></asp:TextBox>
                        <span class="text-danger field-validation-valid" data-valmsg-for="txtPassword" data-valmsg-replace="true"></span>
                    </div>
                </div>
                <div class="form-group inputGroup">
                    <div class="">
						<%--<asp:Button ID="btnSubmit" runat="server" Text="Sign In" class="bttn login-btn" ValidationGroup="login" OnClick="btnSubmit_Click" />--%>
                         <asp:Button ID="btnchangePassword" runat="server" Text="Enter" class="bttn login-btn"
                                    ValidationGroup="pwdchange" OnClick="btnchangePassword_Click" />
                           <asp:Button ID="Button1" runat="server" Text="Cancel" class="bttn login-btn" OnClick="Button1_Click" />
                    </div>
                </div>
                      <p>
                     <label class="login-label" for="Password" id="lbltxt" runat="server" visible="false" style="color:red"></label>
                    </p>
              <%--  <p>
                    <a href="forgotpassword.aspx">Forgot your password?</a>
                    </p>--%><div class="text-danger validation-summary-valid" style="color:red;" data-valmsg-summary="true"><ul><li style="display:none"></li>
</ul></div>
                <p></p>
           
			</form>
            </section>
    </div>
</div>
    </div>
    <footer class="footer">
      <div class="container">
        <span class="text-muted">&copy; Version&ndash; 1.0 
      <a href="http://dataman.in/" target="_blank">DATAMAN COMPUTER SYSTEMS (P) LTD. KANPUR, (INDIA)
                    All rights reserved.</a>.</span>
      </div>
    </footer>
    <script src="dist/js/bootstrap.min.js"></script>
</body>
</html>

