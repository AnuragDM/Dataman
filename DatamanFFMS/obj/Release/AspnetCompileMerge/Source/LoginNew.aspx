<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginNew.aspx.cs" Inherits="AstralFFMS.LoginNew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>


<!DOCTYPE html>
<!--[if lt IE 7]> <html class="lt-ie9 lt-ie8 lt-ie7" lang="en"> <![endif]-->
<!--[if IE 7]> <html class="lt-ie9 lt-ie8" lang="en"> <![endif]-->
<!--[if IE 8]> <html class="lt-ie9" lang="en"> <![endif]-->
<!--[if gt IE 8]><!--> <html lang="en"> <!--<![endif]-->
<head runat="server">
  <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>Login Form</title>
      <link href="Content/bootstrap.css" rel="stylesheet" />
  <link rel="stylesheet" href="css/styleNewlogin.css">
    <%--<script type="text/javascript">
      
        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lbltxt.ClientID %>").style.display = "none";
        }, seconds * 1000);
        };
       
        function anchor_test() {         
            document.getElementById('divhelp').style.visibility = 'visible';
        }
        function hidediv() {
          
            document.getElementById('divhelp').style.visibility = 'hidden';
        }
       
       
</script>--%>

  <!--[if lt IE 9]><script src="//html5shim.googlecode.com/svn/trunk/html5.js"></script><![endif]-->
</head>
<body>

    <script type="text/javascript">

        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lbltxt.ClientID %>").style.display = "none";
            }, seconds * 1000);
        };

        function anchor_test() {
            document.getElementById('divhelp').style.visibility = 'visible';
        }
        function hidediv() {

            document.getElementById('divhelp').style.visibility = 'hidden';
        }


</script>

    <nav class="navbar navbar-default navbar-ffms">
  <div class="container-fluid">
    <div class="navbar-header"><asp:Image ID="Image1" runat="server" alt="Logo"/>
    </div>
    
  </div>
</nav>
    <div class="container body-content">  
<div class="row">
    <div class="col-md-8 loginbox-container">
        <span class="login-text">Log in</span>
        <section>
              	<form class="form-horizontal" runat="server" defaultfocus="txtUserID">
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
                <div class="form-group inputGroup">
                    <label class="login-label" for="Email">USERNAME</label>
                    <div class="">
                        <input class="form-control" type="text" runat="server" data-val="true" placeholder="Enter Username Here" data-val-required="The Email field is required." ID="txtuser" name="login" value="">
                        <span class="text-danger field-validation-valid" data-valmsg-for="txtUserID" data-valmsg-replace="true"></span>
                    </div>
                </div>

                <div class="form-group inputGroup">
                    <label class="login-label" for="Password">PASSWORD</label>
                    <div class="">
                        <input class="form-control" type="password" data-val="true" data-val-required="The Password field is required." runat="server" ID="txtpwd" placeholder="Enter Password Here" name="password">
                        <span class="text-danger field-validation-valid" data-valmsg-for="txtPassword" data-valmsg-replace="true"></span>
                    </div>
                </div>
                <div class="form-group inputGroup">
                    <div class="">
						<asp:Button ID="btnSubmit" runat="server" Text="Log in" class="bttn login-btn" ValidationGroup="login" OnClick="btnSubmit_Click" />
                    </div>
                </div>               
                       <p>
                     <label class="login-label" for="Password" id="lbltxt" runat="server" visible="false" style="color:red"></label>
                    </p>
                <p>
                    <a href="pedforgot.aspx">Forgot your password?</a>
                    </p>
                     <div class="form-group inputGroup">                   
                           <asp:LinkButton ID="lnkReset" runat="server" ForeColor="#337ab7" Font-Underline="true" OnClick="lnkReset_Click"  CausesValidation="False" Visible="False">ReSet Data</asp:LinkButton>
                            <br /><br />
                            <asp:LinkButton ID="lnkHelp" runat="server" ForeColor="#337ab7" Font-Underline="true"  OnClick="lnkHelp_Click" CausesValidation="False" Visible="False">Help</asp:LinkButton>
                    </div>
                     <p>
                    <%--<a href="logout.aspx?Reset='Yes'">Reset</a>--%>
                    </p>
                       <p>
                   <%-- <a HREF="javascript:anchor_test()">Help</a>--%>
                    </p><div class="text-danger validation-summary-valid" style="color:red;" data-valmsg-summary="true"><ul><li style="display:none"></li>
</ul></div>
                <p></p>         

         <div class="col-md-12">
           <div class="form-group">
             <div class="table table-responsive">           
                <asp:Panel ID="pnlData" runat="server" Style="max-height: 900px; height: auto; width: 98%; border: 1px solid; display: none;">
                <div style="overflow: auto; height: auto; max-height: 900px; border: 1px solid; min-width: 600px; width: 98%;">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:GridView ID="gvData" runat="server" Width="100%" class="table table-bordered table-striped" AutoGenerateColumns="False">                                   
                                    <Columns>                                        
                                        <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                        <asp:BoundField DataField="Password" HeaderText="Password" />
                                        <asp:BoundField DataField="Role" HeaderText="Role Type" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnClose" runat="server" Text="Close" class="btn btn-primary"  Width="80px" Style="float: right;" OnClick="btnClose_Click" CausesValidation="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="Modalshow" runat="server" Style="display: none;" />
                                <AjaxToolkit:ModalPopupExtender runat="server" ID="mpePop" TargetControlID="Modalshow" PopupControlID="pnlData" DropShadow="true">
                                </AjaxToolkit:ModalPopupExtender>
                            </td>
                        </tr>
                    </table>
                </div>  
               </asp:Panel>
             </div>
          </div>
        </div>            
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
