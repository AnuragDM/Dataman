<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ExpenseApproval.aspx.cs" Inherits="AstralFFMS.ExpenseApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style type="text/css">
        .spinner {
            position: absolute;
            left: 50%;
            margin-left: -50px; /* half width of the spinner gif */
            margin-top: -50px; /* half height of the spinner gif */
            text-align: center;
            z-index: 999;
            overflow: auto;
            width: 100px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }

        #select2-ContentPlaceHolder1_ddlParentLoc-container {
            margin-top: -8px !important;
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
        <div id="divData" runat="server">
            <div class="box-body" id="rptmain" runat="server">
                <div class="row">
                    <div class="col-md-12">

                        <div class="box">
                            <div class="box-header">
                                <h3 class="box-title">Pending Expenses Approval List</h3>
                            </div>
                            <div class="col-md-1">
                                <label>Search:</label>
                            </div>
                            <div class="clearfix"></div>
                            <div class="col-md-3 col-sm-6 form-group">
                                <label>Employee:</label>

                                <asp:TextBox ID="txtemployee" Width="100%" runat="server" class="form-control abc" TabIndex="1"></asp:TextBox>

                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="completionList"
                                    OnClientItemSelected="ClientEmpIDSelected" EnableCaching="true" ServicePath="ExpenseApproval.aspx"
                                    MinimumPrefixLength="3" ServiceMethod="SearchEmployee" TargetControlID="txtemployee">
                                </ajaxToolkit:AutoCompleteExtender>
                                <asp:HiddenField ID="hdnEmpId" runat="server" />
                                <%--  <asp:DropDownList ID="ddlemp" CssClass="form-control" runat="server" ></asp:DropDownList> --%>
                            </div>
                            <div class="col-md-3 col-sm-6 form-group">
                                <label>Status:</label>
                                <asp:DropDownList ID="ddlstatus" CssClass="form-control" runat="server"></asp:DropDownList>
                            </div>
                            <div class="clearfix"></div>
                            <div class="col-md-6 form-group">
                                <label>City:</label>
                                <asp:DropDownList ID="ddlcity" CssClass="form-control" runat="server"></asp:DropDownList>
                            </div>
                            <div class="clearfix"></div>
                            <div class="col-md-3 col-sm-6 ">
                                <label>From Date:</label>
                                <asp:TextBox ID="DateFrom" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox3_CalendarExtender"
                                    TargetControlID="DateFrom"></ajaxToolkit:CalendarExtender>

                            </div>
                            <div class="col-md-3 col-sm-6">
                                <label>To Date:</label>
                                <asp:TextBox ID="DateTo" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox4_CalendarExtender"
                                    TargetControlID="DateTo"></ajaxToolkit:CalendarExtender>
                            </div>
                            <div class="clearfix"></div>
                            <div class="form-group">
                                <asp:Button ID="btnsearch" runat="server" Text="Search" Style="margin: 16px;" OnClick="btnsearch_Click" CssClass="btn btn-primary" />
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <!-- /.box-header --> 
                        <div class="box-body" id="Div1" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                <HeaderTemplate>
                                    <table id="examplenewdt" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th style="display:none;">ID</th>
                                                <th>Sync Code</th>
                                                <th>Employee Name</th>
                                                <th>Expense Group</th>
                                                <th>Created On</th>
                                                <th>Submitted On</th>
                                                <th>Received Upto</th>
                                                <th>Status</th>
                                                <th style="text-align: right;">Claim Amount</th>
                                                <th style="text-align: right;">Approved Amount</th>
                                                <th>State</th>
                                                <th>City</th>
                                                <th style="visibility: hidden;">Details</th>
                                                <th style="visibility: hidden; ">Details</th>
                                                <th style="visibility: hidden; ">Details</th>
                                                <th style="visibility: hidden; ">Details</th>
                                                <th style="visibility: hidden; ">Details</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <asp:HiddenField ID="hdfExpGroupId" runat="server" Value='<%#Eval("ExpenseGroupId") %>' />

                                        <td style="display:none;"><%#Eval("Id") %></td>
                                        <td><%#Eval("EmpSyncID") %></td>
                                        <td><%#Eval("Name") %></td>
                                        <td><%#Eval("GroupName") %></td>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("CreatedOn"))%></td>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}",Eval("DateOfSubmission")) %></td>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}",Eval("maxBillDt")) %></td>
                                        <td><%#Eval("status") %></td>
                                        <td style="text-align: right;"><%#Eval("TCA") %></td>
                                        <td style="text-align: right;"><%#Eval("TotalApprovedAmount") %></td>
                                        <td><%#Eval("stateName") %></td>
                                        <td><%#Eval("cityName") %></td>
                                        <td >
                                            <asp:HyperLink runat="server" Target="_blank" ID="hplview"
                                                NavigateUrl='<%# String.Format("ExpenseApprovalDetails.aspx?ExpenseGroupId={0}&Status={1}", Eval("ExpenseGroupId"),("View")) %>'
                                                Text="View" ToolTip="View Expenses" /></td>
                                        <td >
                                            <asp:HyperLink runat="server" ID="hpledit" Target="_blank" NavigateUrl='<%# String.Format("ExpenseApprovalDetails.aspx?ExpenseGroupId={0}&Status={1}", Eval("ExpenseGroupId"),Eval("status")) %>'
                                                Text="Edit" ToolTip="Edit Expenses" /></td>
                                        <td style="width: 10px;">
                                            <asp:HyperLink ID="hpdwnld" runat="server" NavigateUrl='<%# Eval("ExpenseGroupId", "~/ExpenseExportExcel.aspx?ExpenseGroupId={0}") %>'
                                                Text="Download" ToolTip="Download" /></td>
                                        <td style="width: 10px;">
                                            <asp:HyperLink runat="server" Target="_blank" NavigateUrl='<%# Eval("ExpenseGroupId", "~/ExpenseSheetSummary.aspx?ExpenseGroupId={0}") %>'
                                                Text="R1" ToolTip="R1" /></td>
                                        <td style="width: 10px;">
                                            <asp:HyperLink runat="server" Target="_blank" NavigateUrl='<%# Eval("ExpenseGroupId", "~/ExpenseSheetsummary1.aspx?ExpenseGroupId={0}") %>'
                                                Text="R2" ToolTip="R2" /></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                    </div>
                    </div></div></div></div>

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
            $("#examplenewdt").DataTable({
                   "order": [[4, "asc"]]
                });

        });
       
        function ClientEmpIDSelected(sender, e) {
            $get("<%=hdnEmpId.ClientID %>").value = e.get_value();
        }
        $('.abc').keyup(function (e) {
        
         if (e.keyCode == 8 || e.keyCode == 46) {
                $('#<%=hdnEmpId.ClientID%>').val("");
             }
          });
    </script>
</asp:Content>
