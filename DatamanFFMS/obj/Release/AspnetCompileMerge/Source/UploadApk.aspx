<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="UploadApk.aspx.cs" Inherits="AstralFFMS.UploadApk" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
                "order": [[0, "desc"]]
            });

        });
    </script>    

    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 250, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");
        }
    </script>
    <script type="text/javascript">
        var V = "";
        function Successmessage(V) {
            $("#messageNotification").jqxNotification({
                width: 250, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");
        }
    </script>
    <script>
        function Valild() {          
            if ($('#<%=txtVersionName.ClientID%>').val() == '') {
                errormessage("Please enter version name");
                return false;
            }
            if ($('#<%=txtversionCode.ClientID%>').val() == '') {
                errormessage("Please enter version code");
                return false;
            }
            if ($('#<%=fileLabel.ClientID %>').text() == '' || $('#<%=fileLabel.ClientID %>').text() == 'Label') {
                var uploadcontrol = document.getElementById('<%=File1.ClientID%>').value;
                if (uploadcontrol.length == 0) {
                    errormessage("Please select the document file");
                    return false;
                }
            }
        }
    </script>
     <script type="text/javascript">
         function showpreview(input) {
             var uploadcontrol = document.getElementById('<%=File1.ClientID%>').value;
             //var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
             if (uploadcontrol.length > 0) {
                 if (input.files && input.files[0]) {
                     var reader = new FileReader();
                     reader.onload = function (e) {
                         $("#ContentPlaceHolder1_fileLabel").css('display', 'none');
                     }
                     reader.readAsDataURL(input.files[0]);
                 }

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
        function DoNav(depId) {
            if (depId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', depId)
            }
        }
    </script>
     <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body" id="mainDiv" style="display: none;" runat="server">
            <div class="row">
                <!-- left column -->

                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Upload Mobile App</h3>
                                <div style="float: right" hidden="hidden">                                  

                                </div>
                                <asp:HiddenField ID="hid" runat="server" />

                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body">

                                <div class="col-md-5">
                                    <div class="form-group paddingleft0">
                                        <div id="divdocid" runat="server" class="form-group">
                                            <label for="exampleInputEmail1">Document No:</label>
                                            <asp:TextBox ID="lbldocno" Enabled="false" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group paddingleft0">
                                        <label for="exampleInputEmail1">AppName: </label>
                                        &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlAppname" Width="100%" runat="server" class="form-control">   
                                                 <asp:ListItem Text="Goldiee_Distributor" Value="Goldiee_Distributor"></asp:ListItem>                                           
                                                 <asp:ListItem Text="Goldiee_Marketting" Value="Goldiee_Marketting"></asp:ListItem> 
                                                 <asp:ListItem Text="LalMahal_CRM" Value="LalMahal_CRM"></asp:ListItem>                                                                                                 
                                       </asp:DropDownList>
                                    </div>
                                     <div class="form-group paddingleft0">
                                        <label for="exampleInputEmail1">Version Name: </label>
                                        &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtVersionName" runat="server" CssClass="form-control numeric" MaxLength="50"></asp:TextBox>
                                    </div>
                                     <div class="form-group paddingleft0">
                                        <label for="exampleInputEmail1">Version Code: </label>
                                        &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtversionCode" runat="server" CssClass="form-control numeric" MaxLength="50"></asp:TextBox>
                                    </div>
                                    <div class="form-group paddingleft0">
                                        <label for="exampleInputEmail1">Active: </label>
                                        <asp:CheckBox ID="chk" runat="server" Checked="true" />
                                    </div>

                                    <div class="form-group paddingleft0">
                                        <label for="exampleInputEmail1">Upload: </label>
                                        &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:FileUpload ID="File1" runat="server" onchange="showpreview(this);" />
                                        <asp:Label ID="fileLabel" runat="server" Text="Label" Style="display: none;"></asp:Label>

                                        <asp:Button ID="btnsave" runat="server" Style="margin-top: 5px;" Text="Save" class="btn btn-primary" OnClientClick="return Valild();" OnClick="btnsave_Click" />
                                                                       
                                    </div>
                                </div>

                            </div>
                            <br />
                            <div>
                                <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                            </div>
                        </div>
                    </div>                   
                </div>
            </div>
        </div>       
    </section>

</asp:Content>
