<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/FFMS.Master" CodeBehind="CoupanZoneWise.aspx.cs" Inherits="AstralFFMS.CoupanZoneWise" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#ContentPlaceHolder1_basicExample').timepicker({ 'timeFormat': 'H:i' });
            $('#ContentPlaceHolder1_basicExample1').timepicker({ 'timeFormat': 'H:i' });
            $("#ContentPlaceHolder1_basicExample").keypress(function (event) { event.preventDefault(); });
            $("#ContentPlaceHolder1_basicExample1").keypress(function (event) { event.preventDefault(); });


        });
    </script>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
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
            <%--if ($('#<%=txtIssueDate.ClientID%>').val() == "") {
                errormessage("Please select the visit Date");
                return false;
            }--%>            
            if ($('#<%=ddlScheme.ClientID%>').val() == "0") {
                errormessage("Please select Coupan Scheme.");
                return false;
            }
            if ($('#<%=ddlZone.ClientID%>').val() == "0") {
                errormessage("Please select Zone.");
                return false;
            }
            if ($('#<%=ddlPrefix.ClientID%>').val() == "0") {
                errormessage("Please select Prefix.");
                return false;
            }
            if ($('#<%=txtstartCoupan.ClientID%>').val() == "") {
                errormessage("Please select Start Coupan No.");
                return false;
            }
            if ($('#<%=txtEndCoupan.ClientID%>').val() == "") {
                errormessage("Please select End Coupan No.");
                return false;
            }          


        }

    </script>
    <style type="text/css">
        .containerStaff {
            border: 1px solid #ccc;
            max-height: 200px;
            overflow-y: scroll;
        }
    </style>

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
    
    <script type="text/javascript">
        //$(function () {
        //    $(".select2").select2();
        //});
    </script>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>


    <script type="text/javascript">
        <%--function ClientItemSelected(sender, e) {
            $get("<%=hfCustomerId.ClientID %>").value = e.get_value();
        }--%>
    </script>

    <script type="text/javascript">
        <%--function SetContextKey() {
            $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey($get("<%=ddlcity.ClientID %>").value);
        }--%>
    </script>

    <section class="content">

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
                            <h3 class="box-title">Create Coupon</h3>                           
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-lg-5 col-md-7 col-sm-7 col-xs-9">                                

                                 <div id="DIV2" runat="server" class="form-group">
                                    <label for="exampleInputEmail1">Coupon Scheme:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlScheme" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div id="DIV1" runat="server" class="form-group">
                                    <label for="exampleInputEmail1">Zone:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlZone" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div id="DIV3" runat="server" class="form-group">
                                    <label for="exampleInputEmail1">Prefix:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlPrefix" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div id="DIVUnder" runat="server" class="form-group" hidden="hidden">
                                    <label for="exampleInputEmail1">Distributor:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlDistributor" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                </div>
                                 <div class="form-group" hidden="hidden">                                           
                                            <label for="exampleInputEmail1">Issue Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtIssueDate" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                           <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtIssueDate" runat="server" />
                                 </div>
                                 <div class="form-group  col-md-6 paddingleft0">
                                           <label for="withSales">Issue Coupan(In Range):</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                           <asp:TextBox ID="txtstartCoupan"  CssClass="form-control numeric text-right" runat="server" placeholder="Start Coupan No."></asp:TextBox>
                                        </div>
                                 <div class="form-group col-md-6 paddingright0">    
                                         <label for="withSales">Issue Coupan(In Range):</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                       
                                         <asp:TextBox ID="txtEndCoupan"  CssClass="form-control numeric text-right" runat="server" placeholder="End Coupan No."></asp:TextBox>
                                 </div>                            
                                
                                 <div class="box-footer">
                                    <asp:Button type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                                    <asp:Button type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                                </div>
                            </div>
                        </div>
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


    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
                "order": [[0, "desc"]]
            });

        });
    </script>
</asp:Content>
