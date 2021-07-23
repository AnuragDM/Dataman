<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MeetExpenseEntry.aspx.cs" Inherits="AstralFFMS.MeetExpense" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style>
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .table-responsive {
            border: 1px solid #fff;
        }
    </style>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            var valLength = "";
            $("#ContentPlaceHolder1_txtName").keypress(function (key) {
                valLength = ($("#ContentPlaceHolder1_txtName").val().length + 1);
                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90)) return false;
                }

            });
        });
    </script>
    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "error"
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
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");
        }
    </script>
    <script type="text/javascript">
        function validate() {

            if ($('#<%=ddlmeetType.ClientID%>').val() == '0') {
                errormessage("Please select the Meet Type");
                return false;
            }
            if ($('#<%=ddlmeet.ClientID%>').val() == '0') {
                errormessage("Please select the Meet");
                return false;
            }

            if ($('#<%=txtfinalbudget.ClientID%>').val() == "") {
                errormessage("Please enter Final Amount.");
                return false;
            }

           <%-- if ($('#<%=txtfinalbudget.ClientID%>').val() <= 0) {
                errormessage("Please enter Final Budget Amount greater than 0.");
                return false;
            }--%>

            if ($('#<%=txtfinalRemark.ClientID%>').val() == "") {
                errormessage("Please enter Final Remark.");
                return false;
            }

        }

    </script>
    <script type="text/javascript">
        function load1() {
            $(".numeric").numeric({ decimal: false, negative: false });
            $("#example1").DataTable();
        }
        $(window).load(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(load1);
        });
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
        <asp:UpdatePanel ID="update" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
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
                                    <h3 class="box-title">Meet Expense Entry</h3>
                                    <div style="float: right">
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                            OnClick="btnFind_Click" />
                                    </div>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">

                                        <div class="form-group paddingleft0">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlunderUser" AutoPostBack="true" OnSelectedIndexChanged="ddlunderUser_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>

                                        </div>

                                        <div class="form-group col-md-6 paddingleft0">
                                            <label for="withSales">Meet Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlmeetType" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlmeetType_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>

                                        <div class="form-group col-md-6 paddingright0">
                                            <label for="withSales">Meet Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlmeet" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlmeet_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-6 paddingleft0">
                                            <label for="withSales">Meet Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtmeetdate" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-6 paddingright0">
                                            <label for="withSales">Approx Budget:</label>
                                            <asp:TextBox ID="txtapproxBudget" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="withSales">Approved Budget:</label>
                                            <asp:TextBox ID="txtapprovedBudget" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="withSales">Approval Remark:</label>
                                            <asp:TextBox ID="txtapprovalremark" runat="server" TextMode="MultiLine" style="resize: none; height: 20%" cols="20" rows="2" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-6 paddingleft0">
                                            <label for="withSales">Approved Date:</label>
                                            <asp:TextBox ID="txtapprovedDate" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        </div>

                                        <div class="form-group col-md-6 paddingright0">
                                            <label for="withSales">Actual Expense:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtfinalbudget" runat="server" MaxLength="12" CssClass="form-control numeric"></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            <label for="withSales">Final Remark:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtfinalRemark" runat="server" TextMode="MultiLine" style="resize: none; height: 20%" cols="20" rows="2" CssClass="form-control"></asp:TextBox>
                                        </div>

                                    </div>
                                </div>
                                <div class="box-footer">
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
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

                <div class="box-body" id="rptmain" runat="server" style="display: none;">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box">
                                <div class="box-header">
                                    <h3 class="box-title">Meet Expense List</h3>
                                    <div style="float: right">
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                                    </div>
                                </div>
                                <!-- /.box-header -->
                                <%--    <div class="box-body">
                            <div class="col-lg-9 col-md-7 col-sm-7 col-xs-9">
                          <div class="col-md-12 paddingleft0">
                            <div id="DIV1"   class="form-group col-md-4">
                                    <label for="exampleInputEmail1">From Date:</label>
                               <asp:TextBox id="txtmDate" runat="server" style="background: white;" CssClass="form-control"></asp:TextBox>
                                 <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange"  Format="dd/MMM/yyyy" TargetControlID="txtmDate" runat="server" />
                                </div> 
                                <div  class="form-group col-md-4 ">
                                    <label for="exampleInputEmail1">To:</label>
                               <asp:TextBox id="txttodate" runat="server" style="background: white;" CssClass="form-control"></asp:TextBox>
                                 <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange"  Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                </div> 
                                <div  class="form-group col-md-4">
                                       <label for="exampleInputEmail1" style="display:block; visibility:hidden">zkjfhksj</label>
                                  <asp:Button type="button" ID="btnGo" runat="server" style="padding: 3px 14px;" Text="Go" class="btn btn-primary go-button-dsr" OnClick="btnGo_Click" />
                                </div> 
                                </div>   
                                </div></div> --%>
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="rpt" runat="server">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <th>Meet Date</th>
                                                        <th>Meet Name</th>
                                                        <th>Approx Budget</th>
                                                        <th>Approved Amount</th>
                                                        <th>Final Budget Amount</th>
                                                        <th>Final Budget Remark</th>
                                                          <th>Status</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr onclick="DoNav('<%#Eval("TMeetExpId") %>');">
                                                <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("MeetDate"))%></td>
                                                <td><%#Eval("MeetName") %></td>
                                                <td style="text-align:right;"><%#Eval("LambBudget") %></td>
                                                <td style="text-align:right;"><%#Eval("AppAmount") %></td>
                                                <td style="text-align:right;"><%#Eval("ExpenseApprovedAmount") %></td>
                                                <td>
                                                    <div class="Remarks"><%#Eval("ExpenseApprovedRemark") %></div>
                                                </td>
                                                  <td><%#Eval("EStatus") %></td>
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

            </ContentTemplate>
        </asp:UpdatePanel>

    </section>


</asp:Content>



