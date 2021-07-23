<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ComplaintNatureMaster.aspx.cs" Inherits="AstralFFMS.ComplaintNatureMaster" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
        $(document).ready(function () {
            BindDept();
        }
        );
    </script>
    <script type="text/javascript">
        function BindDept() {
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
                            var id = $('#<%=HiddenUnderID.ClientID%>').val();
                            //   alert(id);
                            if (id != "") {
                                $('#<%=ddldept.ClientID%>').val(id);
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
                            <h3 class="box-title">Complaint Nature Master</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                    OnClick="btnFind_Click" />

                                <%--  <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" runat="server" />--%>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                            <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                <div class="form-group">
                                    <input id="Id" hidden="hidden" />
                                    <label for="exampleInputEmail1">Nature:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input runat="server" type="text" class="form-control" maxlength="100" id="ComplNatName" placeholder="Enter Complaint Nature name">
                                    <%--  <input runat="server" type="text" class="form-control" maxlength="100" id="DesName" placeholder="Enter Designation Name">--%>
                                </div>
                                 <div class="form-group">
                                    <label for="exampleInputEmail1">Department:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddldept" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                </div>
                                 <div class="form-group">
                                    <label for="exampleInputEmail1">Nature Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlNatreType" Width="100%" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="0" Text="--Select Nature--"></asp:ListItem>
                                         <asp:ListItem Value="Complaint" Text="Complaint"></asp:ListItem>
                                         <asp:ListItem Value="Suggestion" Text="Suggestion"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Email To:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input type="text" class="form-control" maxlength="50" id="EmailTo" placeholder="Enter Email To" runat="server">
                                </div>
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Email CC:</label>
                                    <input type="text" class="form-control" maxlength="50" id="EmailCC" placeholder="Enter Email CC" runat="server"/>
                                </div>                                
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Sync Id:</label>
                                    <input runat="server" type="text" class="form-control" maxlength="50" id="SyncId" placeholder="Enter Sync Id">
                                </div>
                                <div class="form-group">

                                    <label for="exampleInputEmail1">Active:</label>
                                    <input id="chkIsActive" runat="server" type="checkbox" class="checkbox" />
                                </div>
                            </div></div>
                        </div>
                        <div class="box-footer">
                            <asp:HiddenField ID="HiddenUnderID" runat="server" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" OnClick="btnDelete_Click" />
                        </div>
                         <br />
                            <br />
                            <div>
                                <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color:red">*</span>)</b>
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
                            <h3 class="box-title">Complaint Nature List</h3>
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
                                                <th>Name</th>
                                                <th>Nature Type</th>
                                                <th>Department</th>
                                                <th>Email To</th>
                                                <th>Email CC</th>                                             
                                                <th>Sync ID</th>
                                                <th>Active</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("Id") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("Id") %>' />
                                        <td><%#Eval("Name") %></td>
                                          <td><%#Eval("NatureType") %></td>
                                         <td><%#Eval("DepName") %></td>
                                        <td><%#Eval("EmailTo") %></td>
                                        <td><%#Eval("EmailCC") %></td> 
                                        <td><%#Eval("SyncId") %></td>
                                        <td><%#Eval("Active") %></td>
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
            $("#example1").DataTable();

        });
    </script>
</asp:Content>

