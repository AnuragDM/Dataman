<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="SendNotification.aspx.cs" Inherits="AstralFFMS.SendNotification" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .spinner {
            position: absolute;
            top: 50%;
            left: 50%;
            margin-left: -50px; /* half width of the spinner gif */
            margin-top: -50px; /* half height of the spinner gif */
            text-align: center;
            z-index: 999;
            overflow: auto;
            width: 100px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }
    </style>
    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }


        function showpreview(input) {
            var uploadcontrol = document.getElementById('<%=FileUpload1.ClientID%>').value;
            var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
            if (uploadcontrol.length > 0) {
                if (reg.test(uploadcontrol)) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $("#ContentPlaceHolder1_imgpreview").css('display', 'block');
                            $("#ContentPlaceHolder1_imgpreview").attr('src', e.target.result);
                        }
                        reader.readAsDataURL(input.files[0]);
                    }
                }
                else {
                    errormessage("Only image files are allowed!");
                    $("#ContentPlaceHolder1_imgpreview").css('display', 'none');
                    return false;
                }
            }
        }
    </script>
    <script type="text/javascript">
        var V = "";
        function Successmessage(V) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");
        }
    </script>
    <script type="text/javascript">
        function validate() {
            if ($('#<%=ddlmsgType.ClientID%>').val() == "" || $('#<%=ddlmsgType.ClientID%>').val() == "0") {
                errormessage("Please select Message Type");
                return false;
            }
            if ($('#<%=ddlCategory.ClientID%>').val() == "" || $('#<%=ddlCategory.ClientID%>').val() == "0") {
                errormessage("Please select Category");
                return false;
            }            

            if ($('#<%=txtMessage.ClientID%>').val() == "") {
                errormessage("Please enter Message");
                return false;
            }

            var value = ($('#<%=txtMessage.ClientID%>').val().charAt(0));
            var chrcode = value.charCodeAt(0);
            if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                errormessage("Do not start Name with special characters.")
              //  return false;
            }


        }
        function collateralselect()
        {
            if ($('#<%=ddlmsgType.ClientID%>').val() == "3") { //
                var img1 = document.getElementById('ContentPlaceHolder1_imgpreview'); 
                img1.style.visibility = 'visible';
                var FileUpload11 = document.getElementById('ContentPlaceHolder1_FileUpload1');
                FileUpload11.style.visibility = 'visible';
                var labl = document.getElementById('123');
                labl.style.visibility = 'visible';
            }
            else {
                var img = document.getElementById('ContentPlaceHolder1_imgpreview');
                img.style.visibility = 'hidden';
                var FileUpload1 = document.getElementById('ContentPlaceHolder1_FileUpload1');
                FileUpload1.style.visibility = 'hidden';
                var labl1 = document.getElementById('123');
                labl1.style.visibility = 'hidden';
            }
        }
    </script>   
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to delete?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <script type="text/javascript">
        function DoNav(Id) {
            if (Id != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', Id)
            }
        }
    </script>
    <section class="content">
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
        </div>
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <%--<h3 class="box-title">Send Notification</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            <%--<div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                    OnClick="btnFind_Click" />                              
                            </div>--%>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                             <div class="row">
                                 <div class="form-group col-md-2" id="div1" runat="server" style="display: block;">
                                            <label for="exampleInputEmail1">Message Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlmsgType" Width="100%" runat="server" class="form-control" Onchange="collateralselect()">   
                                                 <asp:ListItem Text="Select" Value="0"></asp:ListItem>                                           
                                                 <asp:ListItem Text="General" Value="1"></asp:ListItem>
                                                 <asp:ListItem Text="App Update" Value="2"></asp:ListItem>   
                                                 <asp:ListItem Text="Collateral" Value="3"></asp:ListItem>                                                
                                            </asp:DropDownList>
                                  </div>

                                 <div class="form-group col-md-2" id="divparent" runat="server" style="display: block;">
                                            <label for="exampleInputEmail1">Select Category:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlCategory" Width="100%" runat="server" class="form-control" Onchange="collateralselect()">   
                                                 <asp:ListItem Text="Select" Value="0"></asp:ListItem>                                           
                                                 <asp:ListItem Text="Distributors" Value="1"></asp:ListItem>
                                                 <asp:ListItem Text="Mkt. Staff" Value="2"></asp:ListItem>   
                                                 <asp:ListItem Text="FFMS" Value="3"></asp:ListItem>                                                
                                            </asp:DropDownList>
                                 </div>
                                 <div >

                                       <div class="form-group formlay">
                                       
                                             <img id="imgpreview" height="280" width="280" class="pull-right" src="" style="border-width: 0px; display: none; visibility:hidden" runat="server" />
                                    </div>                                                                                                      
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                    <div class="form-group">
                                        <input id="Id" hidden="hidden" />
                                        <label for="exampleInputEmail1">Message:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox class="form-control" ID="txtMessage" MaxLength="100" Height="85" placeholder="Enter Message" runat="server"></asp:TextBox>                                      
                                    </div>                                                                  
                                </div>
                                
                            </div>
                            <div class="row">
                                <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">

                                       <div class="form-group formlay">
                                        <label for="exampleInputEmail1" id="123" style=" visibility:hidden">Upload Image</label>
                                        <%--<input type="text" class="form-control numeric text-right" maxlength="3" tabindex="42" runat="server" id="Text1" placeholder="Enter DSR Allow Days">--%>
                                        <asp:FileUpload ID="FileUpload1"   runat="server" style=" visibility:hidden"  tabindex="44"  onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" />
                                           <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                                ControlToValidate="FileUpload1" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                           
                                    </div>                                                                                                      
                                </div>
                                 
                            </div>
                        </div>
                        <div style="height:30px">

                        </div>
                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Send Notification" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />                           
                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        
    </section>

    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
   
</asp:Content>