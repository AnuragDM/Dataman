<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ResCenMaster.aspx.cs" Inherits="AstralFFMS.ResCenMaster" %>

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
            var valLength = "";
            $('#<%=txtPlantName.ClientID%>').keypress(function (key) {

                 valLength = ($('#<%=txtPlantName.ClientID%>').val().length + 1);

                 if (valLength < 2) {
                     if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                 }
                 else {
                     return true;
                 }
             });

             var valLength = "";
             $('#<%=txtPlantCode.ClientID%>').keypress(function (key) {

                 valLength = ($('#<%=txtPlantCode.ClientID%>').val().length + 1);

                 if (valLength < 2) {
                     if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                 }
                 else {
                     return true;
                 }
             });
         });
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
            var value = ($('#<%=txtPlantName.ClientID%>').val().charAt(0));
             var chrcode = value.charCodeAt(0);
             if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                 errormessage("Do not start  Plant Name  with special characters ")
                 return false;
             }

             var value = ($('#<%=txtPlantCode.ClientID%>').val().charAt(0));
            var chrcode = value.charCodeAt(0);
            if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                errormessage("Do not start Plant Code with special characters")
                return false;
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
    <%--  <script type="text/javascript">
         $(document).ready(function () {
             var valLength = "";
             $('#<%=txtPlantName.ClientID%>').keypress(function (key) {

                valLength = ($('#<%=txtPlantName.ClientID%>').val().length + 1);

                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
        });
    </script>--%>
    <script type="text/javascript">
        function DoNav(ResCenId) {
            if (ResCenId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', ResCenId)
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
                            <h3 class="box-title">Warehouse Master</h3>
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
                                        <input id="ResCenId" hidden="hidden" />
                                        <label for="exampleInputEmail1">Plant Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox class="form-control" MaxLength="100" ID="txtPlantName" placeholder="Enter warehouse name" runat="server"></asp:TextBox>
                                        <%--  <input runat="server" type="text" class="form-control" maxlength="100" id="DesName" placeholder="Enter Designation Name">--%>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Plant Code:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox class="form-control" MaxLength="50" ID="txtPlantCode" placeholder="Enter warehouse code" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Plant Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:RadioButtonList ID="radPlantType" runat="server" CssClass="radiogroup" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="W" Selected="True">Warehouse</asp:ListItem>
                                            <asp:ListItem Value="F">Factory</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Order Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox class="form-control" MaxLength="50" ID="txtOrderType" placeholder="Enter order type" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Division Code:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox class="form-control" MaxLength="8" ID="txtDivisionCode" placeholder="Enter division code" runat="server"></asp:TextBox>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_txtDivisionCode" runat="server" FilterType="Numbers"
                                            TargetControlID="txtDivisionCode" />
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Sync Id:</label>
                                        <input runat="server" type="text" class="form-control" maxlength="50" id="SyncId" placeholder="Enter Sync Id">
                                    </div>
                                    <div class="form-group">

                                        <label for="exampleInputEmail1">Active:</label>
                                        <input id="chkIsActive" runat="server" type="checkbox" class="checkbox" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="validate();" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" OnClick="btnDelete_Click" />
                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
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
                            <h3 class="box-title">Warehouse List</h3>
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
                                                <th>Plant Name</th>
                                                <th>Plant Code</th>
                                                <th>Plant Type</th>
                                                <th>Order Type</th>
                                                <th style="text-align:right;">Division Code</th>
                                                <th>Sync ID</th>
                                                <th>Active</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("ResCenId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("ResCenId") %>' />
                                        <td><%#Eval("ResCenName") %></td>
                                        <td><%#Eval("PlantCode") %></td>
                                        <td><%#Eval("PlantType") %></td>
                                        <td><%#Eval("OrderType") %></td>
                                        <td style="text-align:right;"><%#Eval("DivisionCode") %></td>
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
