<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="UserMaster.aspx.cs" Inherits="AstralFFMS.UserMaster"  EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();
        });
    </script>
    <%-- <script type="text/javascript">
        $(function () {
            //Initialize Select2 Elements
            $(".select2").select2();
        });
    </script>--%>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            BindRole();
            BindDepartment();
            BindDesignation();
        }
            );
    </script>
    <script type="text/javascript">
        function BindDepartment() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
                     var obj = { DeptId: 0 };
                     $('#<%=ddldept.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                    $.ajax({
                        type: "POST",
                        url: pageUrl + '/PopulateDepartment',
                        data: JSON.stringify(obj),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: OnDeptPopulated,
                        failure: function (response) {
                            alert(response.d);
                        }
                    });
                    function OnDeptPopulated(response) {
                        PopulateDeptControl(response.d, $("#<%=ddldept.ClientID %>"));
                    }
                    function PopulateDeptControl(list, control) {
                        if (list.length > 0) {
                            //  control.removeAttr("disabled");
                            control.empty().append('<option selected="selected" value="0">-- Select Department --</option>');
                            $.each(list, function () {
                                control.append($("<option></option>").val(this['Value']).html(this['Text']));
                            });
                            var id = $('#<%=HiddenDeptID.ClientID%>').val();
                            //   alert(id);
                            if (id != "") {
                                control.val(id);
                            }
                        }
                        else {
                            control.empty().append('<option selected="selected" value="0">Not available<option>');
                        }
                      
                    }

                }
        /////////BindDepartment
        function BindRole() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
               var obj = { RoleType: '' };
               $('#<%=ddlRole.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                    $.ajax({
                        type: "POST",
                        url: pageUrl + '/PopulateRole',
                        data: JSON.stringify(obj),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: OnRolePopulated,
                        failure: function (response) {
                            alert(response.d);
                        }
                    });
                    function OnRolePopulated(response) {
                        PopulateRoleControl(response.d, $("#<%=ddlRole.ClientID %>"));
                      }
                      function PopulateRoleControl(list, control) {
                          if (list.length > 0) {
                              //control.removeAttr("disabled");
                              control.empty().append('<option selected="selected" value="0">-- Select Role --</option>');
                              $.each(list, function () {
                                  control.append($("<option></option>").val(this['Value']).html(this['Text']));
                              });
                              var id = $('#<%=HiddenRoleID.ClientID%>').val();

                     if (id != "") {
                         control.val(id);
                     }
                 }
                 else {
                     control.empty().append('<option selected="selected" value="0">Not available<option>');
                 }
             }

         }
                //////BindDesPopulateDesignation

                function BindDesignation() {
                    var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
                    var obj = { DesigId: 0 };
                    $('#<%=ddldesg.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
             $.ajax({
                 type: "POST",
                 url: pageUrl + '/PopulateDesignation',
                 data: JSON.stringify(obj),
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: OnDesignPopulated,
                 failure: function (response) {
                     alert(response.d);
                 }
             });
             function OnDesignPopulated(response) {
                 PopulateDesignControl(response.d, $("#<%=ddldesg.ClientID %>"));
             }
             function PopulateDesignControl(list, control) {
                 if (list.length > 0) {
                     // control.removeAttr("disabled");
                     control.empty().append('<option selected="selected" value="0">-- Select Designation --</option>');
                     $.each(list, function () {
                         control.append($("<option></option>").val(this['Value']).html(this['Text']));
                     });
                     var id = $('#<%=HiddenDesID.ClientID%>').val();
                    // alert(id);
                    if (id != "") {
                        control.val(id);
                     }
                 }
                 else {
                     control.empty().append('<option selected="selected" value="0">Not available<option>');
                 }
             
            }


        }

    </script>

    <style type="text/css">
        .spinner {
            position: absolute;
            top: 50%;
            left: 50%;
            margin-left: -50px; /* half width of the spinner gif */
            margin-top: -50px; /* half height of the spinner gif */
            text-align: center;
            z-index: 999;
            overflow: hidden;
            width: 180px; /* width of the spinner gif */
            height: 100px; /*hight of the spinner gif +2px to fix IE8 issue */
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            display: table;
        }
    </style>
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
    <script type="text/javascript">
        function chkPassword() {
            var pass1 = document.getElementById("ContentPlaceHolder1_PasswordText").value;
            var pass2 = document.getElementById("ContentPlaceHolder1_ConfPasswordText").value;
            var ok = true;
            if (pass1 != pass2) {
                //alert("Passwords Do not match");
                errormessage("Password Did Not Match.");
                $('#<%=ConfPasswordText.ClientID%>').val("");
            }
            return ok;
        }
    </script>
    <script type="text/javascript">
        function validate() {
            var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
            var Email = document.getElementById("ContentPlaceHolder1_email").value;
            if ($('#<%=UserName.ClientID%>').val() == "") {
                errormessage("Please Enter LoginID.");
                return false;
            }

            var value1 = ($('#<%=UserName.ClientID%>').val().charAt(0));
            var chrcode1 = value1.charCodeAt(0);
            if ((chrcode1 < 97 || chrcode1 > 122) && (chrcode1 < 65 || chrcode1 > 90) && (chrcode1 < 48 || chrcode1 > 57)) {
                errormessage("Do not start LoginID with special characters.")
                return false;
            }

            if ($('#<%=EmpName.ClientID%>').val() == "") {
                errormessage("Please Enter employee name.");
                return false;
            }

            <%--var value = ($('#<%=EmpName.ClientID%>').val().charAt(0));
            var chrcode = value.charCodeAt(0);
            if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90) && (chrcode1 < 48 || chrcode1 > 57)) {
                errormessage("Do not start Employee Name with special characters.")
                return false;
            }--%>

            if ($('#<%=PasswordText.ClientID%>').val() == "") {
                errormessage("Please Enter Password.");
                return false;
            }
            varPwdLength = "";
            varPwdLength = ($('#<%=PasswordText.ClientID%>').val().length);
            if (varPwdLength < 8) {
                errormessage("Please enter password of minimum 8 characters.");
                return false;
            }
            if ($('#<%=ConfPasswordText.ClientID%>').val() == "") {
                errormessage("Please Enter Confirm Password.");
                return false;
            }
            if ($('#<%=email.ClientID%>').val() == "") {
                errormessage("Please Enter Email Id.");
                return false;
            }
            <%--if ($('#<%=email.ClientID%>').val() != "") {
                if (!Email.match(mailformat)) {
                    errormessage("Invalid Email Address.");
                    return false;
                }
            }--%>

            if ($('#<%=ddlRole.ClientID%>').val() == "0") {
                errormessage("Please select Role.");
                return false;
            }
            //     alert($('#<%=ddlRole.ClientID%>').find('option:selected').text())
            if ($('#<%=ddlRole.ClientID%>').find('option:selected').text() != "Distributor") {
                if ($('#<%=ddldept.ClientID%>').val() == "0") {
                    errormessage("Please select a Department.");
                    return false;
                }
                if ($('#<%=ddldesg.ClientID%>').val() == "0") {
                    errormessage("Please select a Designation.");
                    return false;
                }
            }

            //Added
            if ($('#<%=CostCentre1.ClientID%>').val() == "") {
                errormessage("Please enter Cost Centre.");
                return false;
            }

            <%--if ($('#<%=EmpSyncId.ClientID%>').val() == "") {
                errormessage("Please enter Sync Id.");
                return false;
            }--%>
            $('#<%=HiddenDeptID.ClientID%>').val($('#<%=ddldept.ClientID%>').val());
            $('#<%=HiddenRoleID.ClientID%>').val($('#<%=ddlRole.ClientID%>').val());
            $('#<%=HiddenDesID.ClientID%>').val($('#<%=ddldesg.ClientID%>').val());
            //End
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            var valLength = "";
            $('#<%=EmpName.ClientID%>').keypress(function (key) {

                valLength = ($('#<%=EmpName.ClientID%>').val().length + 1);

                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
        });
    </script>
 <%--   <script type="text/javascript">
        $(document).ready(function () {
            var valLength1 = "";
            $('#<%=UserName.ClientID%>').keypress(function (key) {

                valLength1 = ($('#<%=UserName.ClientID%>').val().length + 1);

                if (valLength1 < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
        });
    </script>--%>
       <script type="text/javascript">
           function Validate(txt) {
               var valLength1 = "";
               valLength1 = ($('#<%=UserName.ClientID%>').val().length + 1);
               if (valLength1 < 3)
               {
                   txt.value = txt.value.replace(/[^a-zA-Z 0-9\n\r]+/g, '').replace('.', '');
               }
           }
    </script>
    <script>
        function ddlRole_SelectedIndex()
        {
            if ( $('#<%=ddlRole.ClientID%>').html() == "Distributor" )
            {
                $("#<%=ddldept.ClientID %>").val(0);
                $("#<%=ddldesg.ClientID %>").val(0);
                $("#<%=ddldept.ClientID %>").attr("disabled", "disabled");
                $("#<%=ddldesg.ClientID %>").attr("disabled", "disabled");
            }
            else
            {
                $("#<%=ddldept.ClientID %>").attr("enabled", "enabled");
                $("#<%=ddldesg.ClientID %>").attr("enabled", "enabled");
            }
        }
    </script>
    <script type="text/javascript">
        function DoNav(FVId) {
            if (FVId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', FVId)
            }
        }
    </script>
    <script type="text/javascript">
        function myFunction() {
            $('#Div1 :input').attr('disabled', true);
            $('#Div2 :input').attr('disabled', true);
            //$('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', true);
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
    <section class="content">
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" width="30%" height="50%" alt="Loading" /><br />
            <b>Loading Data....</b>
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
                    <div class="row">
                        <div id="InputWork">
                            <!-- general form elements -->
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                   <%-- <h3 class="box-title">User Master</h3>--%>
                                     <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                                    <div style="float: right">
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnbacksales" runat="server" Text="Back" class="btn btn-primary"
                                    OnClick="btnbacksales_Click" />
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                            OnClick="btnFind_Click" />
                                    </div>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-6 col-xs-9">
                                            <div class="form-group">
                                                <input id="Userid" hidden="hidden" />
                                                <label for="exampleInputEmail1">LoginID:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <input type="text" class="form-control" maxlength="50" id="UserName" runat="server" placeholder="Enter LoginID" onkeyup = "Validate(this)">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4 col-sm-6 col-xs-9">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Employee Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <input type="text" class="form-control" maxlength="50" id="EmpName" runat="server" placeholder="Enter employee name">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4 col-sm-6 col-xs-9">
                                            <div class="form-group">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Password:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                    <asp:TextBox ID="PasswordText" TextMode="Password" class="form-control" MaxLength="20" runat="server" placeholder="Enter Password"></asp:TextBox>
                                                    <%--  <input type="password" class="form-control" maxlength="20" id="Password" runat="server" placeholder="Enter Password">--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4 col-sm-6 col-xs-9">
                                            <div class="form-group">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Confirm Password:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                    <asp:TextBox ID="ConfPasswordText" TextMode="Password" MaxLength="20" class="form-control" onchange="chkPassword()" placeholder="Enter Password" runat="server"></asp:TextBox>
                                                    <%--<input type="password" class="form-control" maxlength="20" id="ConfPassword" runat="server" onchange="chkPassword()" placeholder="Enter Password">--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4 col-sm-6 col-xs-9">
                                            <div class="form-group">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Email Id:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                    <input type="email" class="form-control" maxlength="50" id="email" runat="server" placeholder="Enter Email">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-md-4 col-sm-6 col-xs-9">
                                                    <div class="form-group">
                                                        <div class="form-group">
                                                            <label for="exampleInputEmail1">Role:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                            <asp:DropDownList ID="ddlRole" Width="100%" CssClass="form-control" runat="server"
                                                               onchange="ddlRole_SelectedIndex();" ><%-- OnSelectedIndexChanged="ddlRole_SelectedIndexChanged" AutoPostBack="true">--%>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" id="Div1">
                                                <div class="col-md-4 col-sm-6 col-xs-9">
                                                    <div class="form-group">
                                                        <div class="form-group">
                                                            <label for="exampleInputEmail1">Department:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                            <asp:DropDownList ID="ddldept" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" id="Div2">
                                                <div class="col-md-4 col-sm-6 col-xs-9">
                                                    <div class="form-group">
                                                        <div class="form-group">
                                                            <label for="exampleInputEmail1">Designation:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                            <asp:DropDownList ID="ddldesg" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-md-4 col-sm-6 col-xs-9">
                                                    <div class="form-group">
                                                        <div class="form-group">
                                                            <label for="exampleInputEmail1">Cost Centre:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                            <%--<asp:TextBox class="form-control" maxlength="50" id="CostCentre" runat="server" placeholder="Enter Cost Centre"></asp:TextBox>--%>
                                                            <asp:TextBox ID="CostCentre1" runat="server" class="form-control" placeholder="Enter Cost Centre"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_CostCentre" runat="server" FilterType="Numbers,Custom" ValidChars="."
                                                                TargetControlID="CostCentre1" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                     <!--Added-->
                                    <div class="row">
                                        <div class="col-md-4 col-sm-6 col-xs-9">
                                            <div class="form-group">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">EmpSync Id:</label>
                                                <%--    &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>--%>
                                                    <input type="text" runat="server" class="form-control" maxlength="50" id="EmpSyncId" placeholder="Enter Employee Sync Id">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--End-->

                                    <div class="row">
                                        <div class="col-md-4 col-sm-6 col-xs-9">
                                            <div class="form-group">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Active:</label>
                                                    <input id="chkIsAdmin" type="checkbox" runat="server" class="checkbox" disabled="disabled" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                   

                                </div>
                                <div class="box-footer">
                                    <asp:HiddenField ID="HiddenDeptID" runat="server" />
                                      <asp:HiddenField ID="HiddenDesID" runat="server" />
                                      <asp:HiddenField ID="HiddenRoleID" runat="server" />

                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary"
                                        OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                        OnClick="btnCancel_Click" />
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary"
                                        OnClientClick="Confirm()" OnClick="btnDelete_Click" />
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
        </div>
        <div class="box-body" id="rptmain" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">User List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary"
                                    OnClick="btnBack_Click" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>LoginID</th>
                                                <th>Employee Name</th>
                                                <th>Email Id</th>
                                                <th>Role Name</th>
                                                <th>Department</th>
                                                <th>Designation</th>
                                                <th>Emp SyncId</th>
                                                <th>Active</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("Id") %>');">
                                        <td><%#Eval("Name") %></td>
                                        <td><%#Eval("EmpName") %></td>
                                        <td><%#Eval("Email") %></td>
                                        <td><%#Eval("RoleName") %></td>
                                        <td><%#Eval("DepName") %></td>
                                        <td><%#Eval("DesName") %></td>
                                        <td><%#Eval("EmpSyncId") %></td>
                                        <td><%#Eval("admin") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                        <!-- /.box-body -->
                    </div>
                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>

        </div>

    </section>
</asp:Content>
