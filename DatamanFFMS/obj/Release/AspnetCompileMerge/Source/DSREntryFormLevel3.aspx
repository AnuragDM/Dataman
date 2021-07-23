<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DSREntryFormLevel3.aspx.cs" Inherits="AstralFFMS.DSREntryFormLevel3" %>

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
            if ($('#<%=txtVisitDate.ClientID%>').val() == "") {
                errormessage("Please select the visit Date");
                return false;
            }
            if ($("#ContentPlaceHolder1_basicExample").val() == '') {
                errormessage("Please select the from Time");
                return false;
            }
            if ($("#ContentPlaceHolder1_basicExample1").val() == '') {
                errormessage("Please select the To Time");
                return false;
            }
            if ($('#<%=ddlDSRType.ClientID%>').val() == "0") {
                errormessage("Please select the DSR Type.");
                return false;
            }
            if ($('#<%=DdlDSRNarrtion.ClientID%>').val() == "0") {
                errormessage("Please select the Narration.");
                return false;
            }
            if ($('#<%=txtremarks.ClientID%>').val() == "") {
                errormessage("Please enter the Remark");
                return false;
            }
            if ($('#<%=ddlcity.ClientID%>').val() == "0") {
                errormessage("Please select the City.");
                return false;
            }

            if ($('#<%=txtdistName.ClientID%>').val() == '') {
                errormessage("Please enter the distributor Name.");
                return false;
            }
            if ($('#<%=hfCustomerId.ClientID%>').val() == '') {
                errormessage("Please enter the valid distributor Name.");
                return false;
            }
            if ($('#<%=hfCustomerId.ClientID%>').val() == 0) {
                errormessage("Please enter the valid distributor Name.");
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
        <%--function validate() {
            if ($('#<%=basicExample.ClientID%>').val() == '') {
                errormessage("Please select From Time.");
                return false;
            }
            if ($('#<%=basicExample.ClientID%>').val() == '') {
                errormessage("Please To Time");
                return false;
            }

            if ($('#<%=ddlDSRType.ClientID%>').val() == "0") {
                errormessage("Please select the DSR Type");
                return false;
            }
            if ($('#<%=DdlDSRNarrtion.ClientID%>').val() == "0") {
                errormessage("Please select the DSR Narration");
                return false;
            }
            if ($('#<%=txtremarks.ClientID%>').val() == "0") {
                errormessage("Please enter teh Remark");
                return false;
            }
        }--%>
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
        function ClientItemSelected(sender, e) {
            $get("<%=hfCustomerId.ClientID %>").value = e.get_value();
        }
    </script>

    <script type="text/javascript">
        function SetContextKey() {
            $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey($get("<%=ddlcity.ClientID %>").value);
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
                            <h3 class="box-title">DSR L-3 Remark Entry</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                    OnClick="btnFind_Click" />

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-lg-5 col-md-7 col-sm-7 col-xs-9">

                                <div id="DIVUnder" runat="server" class="form-group">
                                    <label for="exampleInputEmail1">Sales Person:</label>
                                    <asp:DropDownList ID="ddlUndeUser" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlUndeUser_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div id="divdocid" runat="server" class="form-group">
                                    <label for="exampleInputEmail1">Document No:</label>
                                    <asp:TextBox ID="lbldocno" Enabled="false" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>



                                <div class="form-group col-lg-5 col-md-5 col-sm-12 paddingleft0">
                                    <input id="DepId" hidden="hidden" />
                                    <label for="exampleInputEmail1">Visit Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:TextBox ID="txtVisitDate" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" runat="server" TargetControlID="txtVisitDate" />

                                </div>

                                <%-- </div>

                                 <div class="col-md-12 paddingleft0">--%>
                                <div class="form-group col-lg-4 col-md-4 col-sm-12 paddingleft0">
                                    <div class="block">
                                        <label for="exampleInputEmail1">Start Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    </div>
                                    <asp:DropDownList ID="basicExampleDDL" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <input type="text" data-scroll-default="6:00am" placeholder="--Select Time--" class="form-control" id="basicExample" runat="server" autocomplete="off"  visible="false">
                                </div>
                                <div id="divdoci" class="form-group col-lg-3 col-md-3 col-sm-12 paddingleft0 paddingright0">
                                    <div class="block">
                                        <label for="exampleInputEmail1">End Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    </div>
                                      <asp:DropDownList ID="basicExample1DDL" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <input type="text" data-scroll-default="7:00pm" placeholder="--Select Time--" class="form-control" id="basicExample1" runat="server" autocomplete="off" visible="false">
                                </div>


                                <asp:UpdatePanel ID="update1" runat="server">
                                    <ContentTemplate>


                                        <div class="form-group">
                                            <label for="exampleInputEmail1">DSR Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlDSRType" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlDSRType_SelectedIndexChanged" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>

                                        <div class="form-group">
                                            <label for="exampleInputEmail1">DSR Narration:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="DdlDSRNarrtion" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>

                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Remarks:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtremarks" runat="server" Style="resize: none; height: 20%;" TextMode="MultiLine" CssClass="form-control" placeholder="Enter Remark"></asp:TextBox>
                                        </div>

                                        <div id="DSRMarket" runat="server">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">City Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:DropDownList ID="ddlcity" CssClass="form-control" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="form-group">
                                                <label for="withSales">Distributor:</label>
                                                <asp:TextBox ID="txtdistName" runat="server" MaxLength="6" class="form-control" onkeyup="SetContextKey();"></asp:TextBox>

                                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtdistName" ServiceMethod="SearchItem" UseContextKey="true" FirstRowSelected="false" OnClientItemSelected="ClientItemSelected" MinimumPrefixLength="2" EnableCaching="true"></ajaxToolkit:AutoCompleteExtender>
                                                <asp:HiddenField ID="hfCustomerId" runat="server" />
                                                <%--      <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="SearchItem" ServicePath="DSREntryFormLevel3.aspx.cs"
                      TargetControlID="txtdistName" UseContextKey = "true" FirstRowSelected="false" OnClientItemSelected ="ClientItemSelected" MinimumPrefixLength="2" EnableCaching="true"></ajaxToolkit:AutoCompleteExtender>
                                     <asp:HiddenField ID="hfCustomerId" runat="server" />--%>
                                            </div>
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Accompanying Colleague:</label>
                                                <div class="containerStaff">
                                                    <asp:CheckBoxList ID="accStaffCheckBoxList" RepeatDirection="Vertical" runat="server"></asp:CheckBoxList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Distributor Rep:</label>
                                                <asp:TextBox ID="txtDistributorRep" runat="server" CssClass="form-control" placeholder="Enter Distributor Rep"></asp:TextBox>
                                            </div>
                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <div class="box-footer">
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />

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
        <div class="box-body" id="rptmain" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">DSR L-3 List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <div class="col-lg-9 col-md-7 col-sm-7 col-xs-9">
                                <div class="col-md-12 paddingleft0">
                                    <div id="DIV1" class="form-group col-md-4">
                                        <label for="exampleInputEmail1">From Date:</label>
                                        <asp:TextBox ID="txtmDate" runat="server" Style="background: white;" CssClass="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtmDate" runat="server" />
                                    </div>
                                    <div class="form-group col-md-4 ">
                                        <label for="exampleInputEmail1">To Date:</label>
                                        <asp:TextBox ID="txttodate" runat="server" Style="background: white;" CssClass="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                    </div>
                                    <div class="form-group col-md-4">
                                        <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                        <asp:Button type="button" ID="btnGo" Style="padding: 3px 14px;" runat="server" Text="Go" class="btn btn-primary" OnClick="btnGo_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Visit Date</th>
                                                <th>DSR Type</th>
                                                <th>From Time</th>
                                                <th>To Time</th>

                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("DSRL3Id") %>');">
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("VDate"))%></td>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("DSRL3Id") %>' />
                                        <td><%#Eval("NarrationType") %></td>

                                        <td><%#Eval("FromTime") %></td>
                                        <td><%#Eval("ToTime") %></td>


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
