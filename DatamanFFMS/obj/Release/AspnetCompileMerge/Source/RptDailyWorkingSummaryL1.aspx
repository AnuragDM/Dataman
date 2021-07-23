<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="RptDailyWorkingSummaryL1.aspx.cs" Inherits="AstralFFMS.RptDailyWorkingSummaryL1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--   <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
     <link type="text/css" rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
        <style>
       body .ui-tooltip {
            padding: 0 5px;
            font-size:11px;
            font-weight:600;
        }

    </style>
     <script type="text/javascript">
         $(function () {
             $("[id*=trview] input[type=checkbox]").bind("click", function () {
                 var table = $(this).closest("table");
                 if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                     //Is Parent CheckBox
                     var childDiv = table.next();
                     var isChecked = $(this).is(":checked");
                     $("input[type=checkbox]", childDiv).each(function () {
                         if (isChecked) {
                             $(this).prop("checked", "checked");
                         } else {
                             $(this).removeAttr("checked");
                         }
                     });
                 } else {
                     //Is Child CheckBox
                     var parentDIV = $(this).closest("DIV");
                     if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                         $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                     } else {
                         $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                     }
                 }
             });
         })
    </script>
    <script type="text/javascript">
        $(function () {
            $('[id*=ListBox1]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '200px',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
    </script>
    <script type="text/javascript">
        //$(function () {
        //    $(".select2").select2();
        //});
    </script>
    <%-- <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();
        });
    </script>--%>
    <style type="text/css">
      .ShortDesc1 {
    display: block;
    width: 150px !important;
     overflow: hidden;
     }
  .ShortDesc {
    display: block;
    width: 150px !important;
     overflow: hidden;
     }
        .aVis {
            display: none;
        }

        .GridPager td {
            padding: 0 !important;
        }

        .GridPager a {
            display: block;
            height: 20px;
            width: 15px;
            background-color: #3c8dbc;
            color: #fff;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
            padding: 0px;
        }

        .GridPager span {
            display: block;
            height: 20px;
            width: 15px;
            background-color: #fff;
            color: #3c8dbc;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        .input-group .form-control {
            height: 34px;
        }
        
        @media (max-width: 600px) {
            .ui-dialog.ui-widget.ui-widget-content.ui-corner-all.ui-front.ui-draggable.ui-resizable {
                width: 100% !important;
            }
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
   <%-- <script type="text/javascript">
        function GetReport() {

            if ($('#<%=ListBox1.ClientID%>').val() == "0") {
                errormessage("Please select Sales Person.");
                return false;
            }

        }
    </script>--%>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            display: table;
        }
    </style>
    <script type="text/javascript">
        function DoNav(lvrQId) {
            if (lvrQId != "") {
                var url = "DSRReport.aspx?" + lvrQId;
                window.location.href = url;
            }
        }
    </script>

    <script type="text/javascript">

        function SetTarget() {

            document.forms[0].target = "_blank";

        }

    </script>

    <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body">
            <!-- left column -->
            <!-- general form elements -->
            <div class="box box-primary">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box-header with-border">
                            <h3 class="box-title">Daily Working Summary L1</h3>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-md-8">
                                <div class="row">
                                   <%-- <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>
                                            <asp:ListBox ID="ListBox1" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                    </div>--%>
                                     <div id="divtrview" class="col-md-6">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>
                                            <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>
                                        </div>
                                    </div>
                                     <div class="col-md-3 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">DSR Type:</label>
                                            <asp:DropDownList ID="ddlDsrType" runat="server" Width="99%">
                                                <asp:ListItem Text="All" Value="All"></asp:ListItem>   
                                                <asp:ListItem Text="Lock" Value="Lock"></asp:ListItem>
                                                <asp:ListItem Text="UnLock" Value="UnLock"></asp:ListItem>                                               
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Type:</label>
                                            <asp:DropDownList ID="ddlType" runat="server" Width="99%">
                                                <asp:ListItem Text="All" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="DSR" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="Leave" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="Expense" Value="7"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Status:</label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="99%">
                                                <asp:ListItem Text="All" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Approve" Value="Approve"></asp:ListItem>
                                                <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                                <asp:ListItem Text="Reject" Value="Reject"></asp:ListItem>

                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="frmTextBox" class="form-control" runat="server"
                                                Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="frmTextBox_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server"
                                                BehaviorID="frmTextBox_CalendarExtender"
                                                TargetControlID="frmTextBox"></ajaxToolkit:CalendarExtender>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="toTextBox" class="form-control" runat="server"
                                                Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="toTextBox_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server"
                                                BehaviorID="toTextBox_CalendarExtender"
                                                TargetControlID="toTextBox"></ajaxToolkit:CalendarExtender>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="box-footer">
                            <asp:Button type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClientClick="GetReport();"
                                OnClick="btnGo_Click" />
                            <asp:Button type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To CSV" class="btn btn-primary"
                                OnClick="btnExport_Click" />

                        </div>
                    </div>
                </div>




                <div id="rptmain" runat="server" style="display: none;">                 
                    <div class="box-body table-responsive">
                        <label for="workingdays">Total DSR Count</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblTotalWorkingdays" runat="server" Font-Bold="True"></asp:Label>
                        <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound" OnItemCommand="rpt_ItemCommand">
                            <HeaderTemplate>
                                <table id="example1" class="table table-bordered table-striped">
                                    <thead>
                                        <tr>
                                            <th style="display: none;">VDate</th>
                                            <th>Visit Date</th>
                                            <th>Sales Person</th>
                                           <%-- <th>Sync Id</th>--%>
                                            <th>Employee</th>
                                            <th>Type</th>
                                            <th style="max-width:200px !important;overflow-x:hidden;">Remarks</th>
                                            <%--overflow-x:hidden;--%>
                                            <th style="max-width:200px !important;overflow-x:hidden;">Lock-City Name</th>
                                            <th style="text-align: right">Total Party</th>
                                            <th style="text-align: right">Retailer Calls Visited</th>
                                            <th style="text-align: right">Dist. Discuss</th>
                                             <th style="text-align: right">Re. Discuss</th>
                                            <th style="text-align: right">Dist. Failed Visit</th>
                                            <th style="text-align: right">Dist. Collection</th>
                                            <th style="text-align: right">Total Order</th>
                                            <th style="text-align: right" hidden="hidden">Order By Email</th>
                                            <th style="text-align: right" hidden="hidden">Order By Phone</th>
                                            <th style="text-align: right">Demo</th>
                                            <th style="text-align: right">Failed Visit</th>
                                            <th style="text-align: right">Competitor</th>
                                            <th style="text-align: right">Party Collection</th>
                                            <th style="text-align: right">Retailer Per Call Avg Sale</th>
                                            <th style="text-align: right">New Parties</th>
                                            <th style="text-align: right">Claim Exp.</th>
                                            <th style="text-align: right">Approved Exp.</th>
                                           <th>Sync Id</th>
                                            <th>Status</th>
                                            <th>DSR Type</th>
                                            <th>Approved Remarks</th>
                                            <th>Location Tracker</th>
                                            <th class="aVis">RCV</th>
                                            <th class="aVis">TO</th>
                                            <th class="aVis">PCA</th>
                                             <th class="aVis">OrdBymail</th>
                                            <th class="aVis">OrdByPhn</th>
                                        </tr>
                                    </thead>
                                    <tfoot>
                                        <tr>
                                            <th colspan="6" style="text-align: right">Total:</th>
                                            <th style="text-align: right"></th>
                                            <th style="text-align: right"></th>
                                            <th style="text-align: right"></th>
                                            <th style="text-align: right"></th>
                                            <th style="text-align: right"></th>
                                            <th style="text-align: right"></th>
                                           <%-- <th style="text-align: right"></th>              --%>                              
                                            <th style="text-align: right"></th>
                                            <th style="text-align: right"></th>
                                            <th style="text-align: right"></th>
                                            <th style="text-align: right"></th>
                                            <th style="text-align: right"></th>
                                            <th style="text-align: right"></th>
                                            <th style="text-align: right"></th>
                                            <th style="text-align: right"></th>
                                            <th style="text-align: right"></th>
                                            <th style="text-align: right"></th>
                                            <th style="text-align: right"></th>

                                        </tr>
                                    </tfoot>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <asp:HiddenField ID="hdnDate" runat="server" Value='<%#Eval("VDate","{0:dd/MMM/yyyy}")%> ' />
                                    <asp:HiddenField ID="hdnSMId" runat="server" Value='<%#Eval("Id")%>' />
                                    <asp:HiddenField ID="hdnType" runat="server" Value='<%#Eval("Type")%>' />
                                     <asp:HiddenField ID="hdnDsrType" runat="server" Value='<%#Eval("lock")%>' />
                                    <td style="display: none;"><%# System.Convert.ToDateTime(Eval("VDate")).ToString("dd/MMM/yyyy HH:MM:SS")%>
                                         <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="dateLabel" Visible="false" runat="server" Text='<%# System.Convert.ToDateTime(Eval("VDate")).ToString("dd/MMM/yyyy")%>'></asp:Label>
                                         <%-- End --%>
                                    </td>
                                    <td>
                                        <asp:LinkButton CommandName="select" ID="lnkEdit"
                                            CausesValidation="False" runat="server" Text='<%# System.Convert.ToDateTime(Eval("VDate")).ToString("dd/MMM/yyyy") %>' ToolTip="Order Details"
                                            Width="80px" Font-Underline="True" OnClientClick="window.document.forms[0].target='_blank'; setTimeout(function(){window.document.forms[0].target='';}, 500);" /></td>
                                    <td><%# Eval("Level1")%>
                                        <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="levelLabel" runat="server" Visible="false" Text='<%# Eval("Level1")%>'></asp:Label>
                                        <%-- End --%>
                                    </td>
                                    <td><%# Eval("EmpName")%>                                      
                                        <asp:Label ID="empNameLabel" runat="server" Visible="false" Text='<%# Eval("EmpName")%>'></asp:Label>                                       
                                    </td>
                                    <td>
                                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type")%>'></asp:Label></td>
                                    <td style="max-width:200px !important;overflow-x:hidden; "> 
                                        <asp:Label ID="lblremarks" runat="server" Text='<%# Eval("Remarks")%>' CssClass="aVis"></asp:Label>
                                        <asp:LinkButton CssClass="ShortDesc"  runat="server" ToolTip="Show Full Remarks" Text='<%# Eval("Remarks")%>'></asp:LinkButton>
                                     
                                       </td>
                                      <td style="max-width:200px !important;overflow-x:hidden; "> 
                                        <asp:Label ID="lblCityname" runat="server" Text='<%# Eval("cityname")%>'  CssClass="aVis" ></asp:Label>
                                        <asp:LinkButton Class="ShortDesc1"  runat="server"  ToolTip="Show Full City Name" Text='<%# Eval("cityname")%>'></asp:LinkButton>
                                     
                                       </td>
                                    <td style="text-align: right"><%# Eval("TotalParty")%>
                                         <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="totalPartyLabel" runat="server" Visible="false" Text='<%# Eval("TotalParty")%>'></asp:Label>
                                            <%-- End --%>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCallsVisited" runat="server" Text='<%# Eval("CallsVisited")%>'></asp:Label></td>
                                    <td style="text-align: right"><%# Eval("DistDiscuss")%>
                                         <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="distDiscussLabel" runat="server" Visible="false" Text='<%# Eval("DistDiscuss")%>'></asp:Label>
                                            <%-- End --%>
                                    </td>
                                      <td style="text-align: right"><%# Eval("retailerdiscuss")%>
                                         <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="retailerdiscuss" runat="server" Visible="false" Text='<%# Eval("retailerdiscuss")%>'></asp:Label>
                                            <%-- End --%>
                                    </td>
                                    <td style="text-align: right"><%# Eval("DistFailVisit")%>
                                         <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="distFailVisitLabel" runat="server" Visible="false" Text='<%# Eval("DistFailVisit")%>'></asp:Label>
                                            <%-- End --%>
                                    </td>
                                    <td style="text-align: right"><%# Eval("DistributorCollection")%>
                                         <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="distCollectionLabel" runat="server" Visible="false" Text='<%# Eval("DistributorCollection")%>'></asp:Label>
                                            <%-- End --%>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTotalOrder" runat="server" Text='<%# Eval("TotalOrder")%>'></asp:Label></td>
                                     <td style="text-align: right" hidden="hidden">
                                        <asp:Label ID="lblOrderMail" runat="server" Text='<%# Eval("OrderAmountMail")%>'></asp:Label></td>
                                     <td style="text-align: right" hidden="hidden">
                                        <asp:Label ID="lblOrderPhone" runat="server" Text='<%# Eval("OrderAmountPhone")%>'></asp:Label></td>
                                    <td style="text-align: right"><%# Eval("Demo")%>
                                         <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="demoLabel" runat="server" Visible="false" Text='<%# Eval("Demo")%>'></asp:Label>
                                            <%-- End --%>
                                    </td>
                                    <td style="text-align: right"><%# Eval("FailedVisit")%>
                                          <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="failvistLabel" runat="server" Visible="false" Text='<%# Eval("FailedVisit")%>'></asp:Label>
                                            <%-- End --%>
                                    </td>
                                    <td style="text-align: right"><%# Eval("Competitor")%>
                                          <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="competitorLabel" runat="server" Visible="false" Text='<%# Eval("Competitor")%>'></asp:Label>
                                            <%-- End --%>
                                    </td>
                                    <td style="text-align: right"><%# Eval("PartyCollection")%>
                                         <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="partyCollectionLabel" runat="server" Visible="false" Text='<%# Eval("PartyCollection")%>'></asp:Label>
                                            <%-- End --%>
                                    </td>
                                    <td style="text-align: right">
                                        <%# Eval("PerCallAvgCell","{0:0.00}")%>
                                         <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="percallSaleLabel" runat="server" Visible="false" Text='<%# Eval("PerCallAvgCell","{0:0.00}")%>'></asp:Label>
                                            <%-- End --%>
                                    </td>
                                    <td style="text-align: right"><%# Eval("NewParties")%>
                                         <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="newPartyLabel" runat="server" Visible="false" Text='<%# Eval("NewParties")%>'></asp:Label>
                                            <%-- End --%>
                                    </td>
                                    <td style="text-align: right"><%# Eval("LocalExpenses")%>
                                           <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="localExpenseLabel" runat="server" Visible="false" Text='<%# Eval("LocalExpenses")%>'></asp:Label>
                                            <%-- End --%>
                                    </td>
                                    <td style="text-align: right"><%# Eval("TourExpenses")%>
                                         <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="tourExpenseLabel" runat="server" Visible="false" Text='<%# Eval("TourExpenses")%>'></asp:Label>
                                            <%-- End --%>
                                    </td>
                                    <td><%# Eval("SyncId")%>
                                           <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="syncIdLabel" runat="server" Visible="false" Text='<%# Eval("SyncId")%>'></asp:Label>
                                            <%-- End --%>
                                    </td>
                                    <td><%# Eval("AType")%>
                                         <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="atypeLabel" runat="server" Visible="false" Text='<%# Eval("AType")%>'></asp:Label>
                                            <%-- End --%>
                                    </td>
                                    <td><%# Eval("lock")%>
                                         <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="lockLabel" runat="server" Visible="false" Text='<%# Eval("lock")%>'></asp:Label>
                                            <%-- End --%>
                                    </td>
                                    <td><%# Eval("AppRemark")%> 
                                        <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="appRemarkLabel" runat="server" Visible="false" Text='<%# Eval("AppRemark")%>'></asp:Label>
                                            <%-- End --%></td>
                                    <td>
                                     <%--   <asp:HyperLink ID="hpl" runat="server" NavigateUrl='<%# Eval("Id", "~/LocationMap.aspx?Smid={0}") %>'
                                            Text="Details" Target="_blank" ToolTip="Location Tracker"></asp:HyperLink>--%>
                                         <asp:HyperLink ID="hpl" runat="server" NavigateUrl='<%# string.Format("~/LocationMap.aspx?Smid={0}&VDate={1}",
HttpUtility.UrlEncode(Eval("Id").ToString()), HttpUtility.UrlEncode(Eval("VDate").ToString())) %>'
                                            Text="Details" Target="_blank" ToolTip="Location Tracker"></asp:HyperLink>
                                    </td>
                                    <td class="aVis"><%# Eval("CallsVisited")%></td>
                                    <td class="aVis"><%# Eval("TotalOrder")%></td>
                                    <td class="aVis"><%# Eval("PerCallAvgCell","{0:0.00}")%></td>
                                       <td class="aVis"><%# Eval("OrderAmountMail","{0:0.00}")%></td>
                                       <td class="aVis"><%# Eval("OrderAmountPhone","{0:0.00}")%></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>                              
                                    
                                   
                                          </tbody>     </table>   
                                       
                            </FooterTemplate>

                        </asp:Repeater>                       
                    </div>

                </div>
            </div>
        </div>
    </section>

    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <div id="popupdivremarks" style="display: none">
        <table style="width: 100%;">
            <tbody>
                <tr>
                    <td>
                        <label class="">Remarks:</label>

                    </td>
                    <td>
                        <label style="font-size:small;align-content:stretch;" id="lblshowremarks"></label>
                    </td>

                </tr>
               
            </tbody>
        </table>
    </div>
     <div id="popupdivcityname" style="display: none">
        <table style="width: 100%;">
            <tbody>
                <tr>
                    <td>
                        <label class="">City Name:</label>

                    </td>
                    <td>
                        <label style="font-size:small;align-content:stretch;" id="lblshowcityname"></label>
                    </td>

                </tr>
               
            </tbody>
        </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $.widget('ui.dialog', jQuery.extend({}, jQuery.ui.dialog.prototype, {
                _title: function (titleBar) {
                    titleBar.html(this.options.title || '&#160;');
                }
            }));


            $('#example1 .ShortDesc').click(function () {
                var currentRow = $(this).parents("tr");
                var remarks = currentRow.find("span[id*='lblremarks']").text();
                $("#lblshowremarks").text(remarks);
            $("#popupdivremarks").dialog({
                title: "Remarks",
                width: 500,
                height: 200,
                modal: true,
                closeText: ''
                //buttons: {
                //    Close: function () {
                //        $(this).dialog('close');
                //    }
                //}
            });
            return false;
            })
        });

       

        $(document).ready(function () {
            $.widget('ui.dialog', jQuery.extend({}, jQuery.ui.dialog.prototype, {
                _title: function (titleBar) {
                    titleBar.html(this.options.title || '&#160;');
                }
            }));


            $('#example1 .ShortDesc1').click(function () {
                var currentRow = $(this).parents("tr");
                var Cityname = currentRow.find("span[id*='lblCityname']").text();
                $("#lblshowcityname").text(Cityname);
                $("#popupdivcityname").dialog({
                    title: "City Name",
                    width: 500,
                    height: 200,
                    modal: true,
                    closeText: ''
                    //buttons: {
                    //    Close: function () {
                    //        $(this).dialog('close');
                    //    }
                    //}
                });
                return false;
            })
        });

       
        $(function () {
            $('#example1').dataTable({
                "order": [[0, "desc"]],
                "footerCallback": function (tfoot, data, start, end, display) {
                    //$(tfoot).find('th').eq(0).html("Starting index is " + start);
                    var api = this.api();
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Total Party'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(6).footer()).html(searchTotal);

                    var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Dist. Discuss'; }).first().index();
                    var totalData1 = api.column(costColumnIndex1).data();
                    var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                    var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                    var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    $(api.column(8).footer()).html(searchTotal1);

                    var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Dist. Failed Visit'; }).first().index();
                    var totalData1 = api.column(costColumnIndex1).data();
                    var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                    var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                    var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    $(api.column(9).footer()).html(searchTotal1);

                    var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Dist. Collection'; }).first().index();
                    var totalData1 = api.column(costColumnIndex1).data();
                    var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                    var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                    var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    $(api.column(10).footer()).html(searchTotal1);

                    //var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'OrdBymail'; }).first().index();
                    //var totalData1 = api.column(costColumnIndex1).data();
                    //var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    //var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                    //var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    //var searchTotalData = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                    //var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    //$(api.column(12).footer()).html(searchTotal);

                    //var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'OrdByPhn'; }).first().index();
                    //var totalData1 = api.column(costColumnIndex1).data();
                    //var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    //var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                    //var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    //var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                    //var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    //$(api.column(13).footer()).html(searchTotal1);

                    var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Demo'; }).first().index();
                    var totalData1 = api.column(costColumnIndex1).data();
                    var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                    var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                    var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    $(api.column(12).footer()).html(searchTotal1);

                    var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Failed Visit'; }).first().index();
                    var totalData1 = api.column(costColumnIndex1).data();
                    var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                    var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                    var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    $(api.column(13).footer()).html(searchTotal1);

                    var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Competitor'; }).first().index();
                    var totalData1 = api.column(costColumnIndex1).data();
                    var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                    var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                    var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    $(api.column(14).footer()).html(searchTotal1);

                    var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Party Collection'; }).first().index();
                    var totalData1 = api.column(costColumnIndex1).data();
                    var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                    var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                    var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    $(api.column(15).footer()).html(searchTotal1);

                    var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'New Parties'; }).first().index();
                    var totalData1 = api.column(costColumnIndex1).data();
                    var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                    var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                    var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    $(api.column(17).footer()).html(searchTotal1);

                    var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Claim Exp.'; }).first().index();
                    var totalData1 = api.column(costColumnIndex1).data();
                    var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                    var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                    var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    $(api.column(18).footer()).html(searchTotal1);

                    var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Approved Exp.'; }).first().index();
                    var totalData1 = api.column(costColumnIndex1).data();
                    var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                    var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                    var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    $(api.column(19).footer()).html(searchTotal1);

                    var RCVColumnIndex = $('th').filter(function (i) { return $(this).text() == 'RCV'; }).first().index();
                    var totalDataRCV = api.column(RCVColumnIndex).data();
                    var totalRCV = totalDataRCV.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalDataRCV = api.column(RCVColumnIndex, { page: 'current' }).data();
                    var pageTotalRCV = pageTotalDataRCV.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalDataRCV = api.column(RCVColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotalRCV = searchTotalDataRCV.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    $(api.column(7).footer()).html(searchTotalRCV);

                    var TOColumnIndex = $('th').filter(function (i) { return $(this).text() == 'TO'; }).first().index();
                    var totalDataTO = api.column(TOColumnIndex).data();
                    var totalTO = totalDataTO.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalDataTO = api.column(TOColumnIndex, { page: 'current' }).data();
                    var pageTotalTO = pageTotalDataTO.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalDataTO = api.column(TOColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotalTO = searchTotalDataTO.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    $(api.column(11).footer()).html(searchTotalTO);

                    var PCAColumnIndex = $('th').filter(function (i) { return $(this).text() == 'PCA'; }).first().index();
                    var totalDataPCA = api.column(PCAColumnIndex).data();
                    var totalPCA = totalDataPCA.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalDataPCA = api.column(PCAColumnIndex, { page: 'current' }).data();
                    var pageTotalPCA = pageTotalDataPCA.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalDataPCA = api.column(PCAColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotalPCA = searchTotalDataPCA.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    $(api.column(16).footer()).html(searchTotalPCA);

                    if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(7).footer()).html('0.0') }
                }
            })

        });
    </script>
</asp:Content>
