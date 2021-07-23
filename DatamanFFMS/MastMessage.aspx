<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MastMessage.aspx.cs" Inherits="AstralFFMS.MastMessage" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            BindRole();
            BindGeolocation();
        });
    </script>
    <script type="text/javascript">
        function BindRole() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
            var obj = { RoleType: '' };
             $('#<%=ddlrole.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
               $.ajax({
                   type: "POST",
                   url: pageUrl + '/PopulateRole',
                   data: JSON.stringify(obj),
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   success: OnPopulated,
                   failure: function (response) {
                       alert(response.d);
                   }
               });
               function OnPopulated(response) {
                   PopulateControl(response.d, $("#<%=ddlrole.ClientID %>"));
            }
            function PopulateControl(list, control) {
                if (list.length > 0) {
                    //  control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(list, function () {
                        control.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });
                    var id = $('#<%=HiddenRoleID.ClientID%>').val();
                    //  alert(id);
                    if (id != "") {
                        control.val(id);
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">Not available<option>');
                }

            }
        }
        /////////////////ItemClass

        function BindGeolocation() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
           // var obj = { ClassId: 0 };
            $('#<%=ddlstate.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: pageUrl + '/PopulateGeolocation',
              //  data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ddlstate.ClientID %>"));
               }
               function PopulateControl(list, control) {
                   if (list.length > 0) {
                       //  control.removeAttr("disabled");
                       control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                       $.each(list, function () {
                           control.append($("<option></option>").val(this['Value']).html(this['Text']));
                       });
                       var id = $('#<%=HiddenGeolocationID.ClientID%>').val();
                    //  alert(id);
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

            if ($('#<%=ddlrole.ClientID%>').val() == "0") {
                errormessage("Please select Role.");
                return false;
            }


            if ($('#<%=ddlstate.ClientID%>').val() == "0") {
                errormessage("Please select Geo Location.");
                return false;
            }

            if ($('#<%=txtmessage.ClientID%>').val() == "") {
                errormessage("Please enter message.");
                return false;
            }
            $('#<%=HiddenRoleID.ClientID%>').val($('#<%=ddlrole.ClientID%>').val());
            $('#<%=HiddenGeolocationID.ClientID%>').val($('#<%=ddlstate.ClientID%>').val());
      
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
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                           <%-- <h3 class="box-title">Message Template</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                    OnClick="btnFind_Click" />

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                                <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Role:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlrole" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Geo Location:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlstate" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Message:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtmessage" runat="server" Style="resize: none; height: 20%;" TextMode="MultiLine" CssClass="form-control" placeholder="Enter Remark"></asp:TextBox>
                                    </div>
                                    <%--<div class="form-group">--%>
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Active:</label>
                                            <input id="chkIsAdmin" type="checkbox" runat="server" class="checkbox" />
                                        </div>
                                    </div>
                                    </div>
                                    </div>
                                    <div class="box-footer">
                                        <asp:HiddenField ID="HiddenRoleID" runat="server" />
                                        <asp:HiddenField ID="HiddenGeolocationID" runat="server" />
                                        <asp:Button Style="margin-right: 5px; margin-bottom: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                                        <asp:Button Style="margin-right: 5px; margin-bottom: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                                        <asp:Button Style="margin-right: 5px; margin-bottom: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" OnClientClick="Confirm()" class="btn btn-primary" OnClick="btnDelete_Click" />
                                    </div>
                                    <br />
                                    <div>
                                       <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                                       
                                    </div>
                                <%--</div>--%>
                            <%--</div>--%>
                        </div>
                    </div>
                </div>
           <%-- </div>--%>
        </div>
        <div class="box-body" id="rptmain" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Message Template List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Role Name</th>
                                                <th>Message</th>
                                                <th>Active</th>
                                                <%--<th>State Name</th>--%>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("Id") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("Id") %>' />
                                        <td><%#Eval("RoleName") %></td>
                                        <td><%#Eval("Message") %></td>
                                        <td><%#Eval("active") %></td>
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
</asp:Content>


