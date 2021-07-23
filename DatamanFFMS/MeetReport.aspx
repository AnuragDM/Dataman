<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="MeetReport.aspx.cs" Inherits="AstralFFMS.MeetReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
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
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
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
                             //$(this).removeAttr("checked");
                         }
                     });
                 } else {
                     //Is Child CheckBox
                     var parentDIV = $(this).closest("DIV");
                     if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                         $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                     } else {
                         //$("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                     }
                 }
             });
         })
    </script>

    <section class="content">
         <div id="messageNotification">
            <asp:Label ID="lblmasg" runat="server"></asp:Label>
        </div>
        <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                           <%-- <h3 class="box-title">Meet Report</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <div class="col-lg-12 col-md-12 col-sm-9 col-xs-11">
                                <div class="row">
                                 <div class="form-group col-md-5">
                                        <label for="exampleInputEmail1">Sales Person:</label>
                                       <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>

                                    </div>
                                    </div>
                                <div class="col-md-12 paddingleft0">
                                    <div class="form-group col-md-3">
                                        <label for="exampleInputEmail1">Status:</label>
                                        <asp:DropDownList ID="ddlstatus" Width="100%" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                            <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                                            <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
                                            <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                            <asp:ListItem Text="Cancel" Value="Cancel"></asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                    <div class="form-group col-md-3">
                                        <label for="exampleInputEmail1">Meet Type:</label>
                                        <asp:DropDownList ID="ddlmeetType" Width="100%" runat="server" CssClass="form-control">
                                        </asp:DropDownList>

                                    </div>
                                    <div class="form-group col-md-3" hidden>
                                        <label for="exampleInputEmail1">Sales Person:</label>
                                        <asp:DropDownList ID="ddllevel1" Width="100%" runat="server" CssClass="form-control">
                                        </asp:DropDownList>

                                    </div>
                                     <%-- <div class="form-group col-md-3">
                                        <label for="exampleInputEmail1">Sales Person:</label>
                                       <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>

                                    </div>--%>
                                </div>
                                <div class="col-md-12 paddingleft0">
                                    <div id="DIV1" class="form-group col-md-3">
                                        <label for="exampleInputEmail1">From Date:</label>
                                        <asp:TextBox ID="txtmDate" runat="server" Style="background: white;" CssClass="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtmDate" runat="server" />
                                    </div>
                                    <div class="form-group col-md-3">
                                        <label for="exampleInputEmail1">To Date:</label>
                                        <asp:TextBox ID="txttodate" runat="server" Style="background: white;" CssClass="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                    </div>
                                   <%-- <div class="form-group col-md-3">
                                        <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                        <asp:Button type="button" ID="btnGo" runat="server" Style="padding: 3px 14px;" Text="Go" class="btn btn-primary go-button-dsr" OnClick="btnGo_Click" />
                                        <asp:Button ID="btnexport" class="btn btn-primary" Style="padding: 3px 14px;" runat="server" Text="Export" OnClick="btnexport_Click" />
                                    </div>--%>
                                </div>
                            </div>
                            <div class="box-footer">
                                <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                        <asp:Button type="button" ID="btnGo" runat="server" Style="padding: 3px 14px;" Text="Go" class="btn btn-primary go-button-dsr" OnClick="btnGo_Click" />
                                        <asp:Button ID="btnexport" class="btn btn-primary" Style="padding: 3px 14px;" runat="server" Text="Export" OnClick="btnexport_Click" />
                                </div>
                     <%--   </div>--%>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                               <%-- <th>Sr.No.</th>--%>
                                                <th>Sales Person</th>
                                                <th>Sales Person Code</th>
                                                <th>Meet Date</th>
                                                <th>MeetName</th>
                                                <th>Venue</th>
                                                <th>Party Name</th>                                            
                                                <th>Distributor</th>
                                                <th>Distributor Code</th>                                   
                                                <th>City</th>
                                                <th>City Code</th>
                                                <th>City Type</th>
                                                <th>Planned Users</th>
                                            <%--<th>Actual Users</th>--%>
                                                <th>Type Of Gift</th>
                                                <th>Distributor’s Sharing % </th>
                                                <th>Astral’s Sharing % </th>
                                                <th>Product Class</th>
                                                <%--<th>Product Class</th>--%>
                                                <th>Approx Budget</th>
                                                <th>Approved Amount</th>
                                                <th>Expense Amount</th>
                                                <th>Expense Remark</th>
                                                <th>Expense Approved Amount</th>
                                                <th>Expense Approved Remarks</th>
                                                <th>Meet Status</th>
                                                <th>Approval/cancel Remark</th>
                                                <th>Approval/Cancel Date</th>
                                                <th>Meet Type</th>
                                                <th>No of Staff</th>
                                                <th>Comments</th>
                                                <th>Beat</th>
                                                <th>Qty Required</th>
                                                <th>Actual User Count</th>
                                                <th>Balance Qty</th>
                                                <th>Meet Image Upload</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                       <%-- <td><%#Container.ItemIndex+1 %>                             
                                        </td>--%>
                                        <td><%#Eval("SMName") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="SMNameLabel" runat="server" Visible="false" Text='<%# Eval("SMName")%>'></asp:Label>
                                            <%-- End --%>
                                           <td><%#Eval("SAPCode") %></td>
                                          <asp:Label ID="SAPCodeLabel" runat="server" Visible="false" Text='<%# Eval("SAPCode")%>'></asp:Label>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("MeetDate"))%></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="MeetDateLabel" runat="server" Visible="false" Text='<%# Eval("MeetDate")%>'></asp:Label>
                                            <%-- End --%>
                                        <td>
                                            <asp:LinkButton ID="lnkedit"  runat="server" Text='<%#Eval("MeetName") %>'  CommandName="MeetEdit" CommandArgument='<%#Eval("MeetPlanId")%>'></asp:LinkButton></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="MeetNameLabel" runat="server" Visible="false" Text='<%# Eval("MeetName")%>'></asp:Label>
                                            <%-- End --%>
                                        <td><%#Eval("Venue") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="VenueLabel" runat="server" Visible="false" Text='<%# Eval("Venue")%>'></asp:Label>
                                            <%-- End --%>
                                        <td><%#Eval("PartyName") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="PartyNameLabel" runat="server" Visible="false" Text='<%# Eval("PartyName")%>'></asp:Label>
                                          
                                            <td><%#Eval("Distributor1") %></td>
                                         <asp:Label ID="DistributorLabel" runat="server" Visible="false" Text='<%# Eval("Distributor1")%>'></asp:Label>
                                            <td><%#Eval("DisSyncld") %></td>
                                          
                                            <asp:Label ID="DisSyncldLabel" runat="server" Visible="false" Text='<%# Eval("DisSyncld")%>'></asp:Label>
                                            <td><%#Eval("City") %></td>
                                          <asp:Label ID="CityLabel" runat="server" Visible="false" Text='<%# Eval("City")%>'></asp:Label>
                                            <td><%#Eval("CityCode") %></td>
                                          <asp:Label ID="CityCodeLabel" runat="server" Visible="false" Text='<%# Eval("CityCode")%>'></asp:Label>
                                            <%-- End --%>
                                        <td><%#Eval("CityType") %></td>
                                          <asp:Label ID="lblCityType" runat="server" Visible="false" Text='<%# Eval("CityType")%>'></asp:Label>
                                        <td><%#Eval("NoOfUser") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="NoOfUserLabel" runat="server" Visible="false" Text='<%# Eval("NoOfUser")%>'></asp:Label>
                                            <%-- End --%>
                                       <%-- <td><%#Eval("Actualuser") %></td>                                       
                                        <asp:Label ID="Label3" runat="server" Visible="false" Text='<%# Eval("Actualuser")%>'></asp:Label>          --%>                                  
                                        <td><%#Eval("typeOfGiftEnduser") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="typeOfGiftEnduserLabel" runat="server" Visible="false" Text='<%# Eval("typeOfGiftEnduser")%>'></asp:Label>
                                            <%-- End --%>
                                         <td><%#Eval("ExpShareDist") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Eval("ExpShareDist")%>'></asp:Label>
                                            <%-- End --%>
                                         <td><%#Eval("ExpShareSelf") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="Label2" runat="server" Visible="false" Text='<%# Eval("ExpShareSelf")%>'></asp:Label>
                                            <%-- End --%>
                                         <td><%#Eval("meetproduct") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="meetproductLabel" runat="server" Visible="false" Text='<%# Eval("meetproduct")%>'></asp:Label>
                                            <%-- End --%>
                                        <%--<td><%#Eval("IndName") %></td>--%>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <%--<asp:Label ID="IndNameLabel" runat="server" Visible="false" Text='<%# Eval("IndName")%>'></asp:Label>--%>
                                            <%-- End --%>
                                        <td style="text-align: right;"><%#Eval("LambBudget") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="LambBudgetLabel" runat="server" Visible="false" Text='<%# Eval("LambBudget")%>'></asp:Label>
                                            <%-- End --%>
                                        <td style="text-align: right;"><%#Eval("AppAmount") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="AppAmountLabel" runat="server" Visible="false" Text='<%# Eval("AppAmount")%>'></asp:Label>
                                            <%-- End --%>
                                        <td style="text-align: right;"><%#Eval("ExpenseApprovedAmount") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="ExpenseApprovedAmountLabel" runat="server" Visible="false" Text='<%# Eval("ExpenseApprovedAmount")%>'></asp:Label>
                                            <%-- End --%>
                                        <td style="text-align: right;"><%#Eval("ExpenseApprovedRemark") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="ExpenseApprovedRemarkLabel" runat="server" Visible="false" Text='<%# Eval("ExpenseApprovedRemark")%>'></asp:Label>
                                            <%-- End --%>
                                        <td style="text-align: right;"><%#Eval("FinalApprovedAmount") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="FinalApprovedAmountLabel" runat="server" Visible="false" Text='<%# Eval("FinalApprovedAmount")%>'></asp:Label>
                                            <%-- End --%>
                                        <td style="text-align: right;"><%#Eval("FinalApprovedRemark") %></td>
                                    
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="FinalApprovedRemarkLabel" runat="server" Visible="false" Text='<%# Eval("FinalApprovedRemark")%>'></asp:Label>
                                            <%-- End --%>
                                            <td><%#Eval("AppStatus") %></td>
                                         <asp:Label ID="lblAppStatus" runat="server" Visible="false" Text='<%# Eval("AppStatus")%>'></asp:Label>
                                        <td><%#Eval("AppRemark") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="AppRemarkLabel" runat="server" Visible="false" Text='<%# Eval("AppRemark")%>'></asp:Label>
                                            <%-- End --%>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("Appdate"))%></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="AppdateLabel" runat="server" Visible="false" Text='<%# Eval("Appdate")%>'></asp:Label>
                                         <%-- End --%>

                                          <td><%#Eval("MeetTypeName") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="MeetTypeNameLabel" runat="server" Visible="false" Text='<%# Eval("MeetTypeName")%>'></asp:Label>
                                            <%-- End --%>
                                          <td><%#Eval("NoStaff") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="NoStaffLabel" runat="server" Visible="false" Text='<%# Eval("NoStaff")%>'></asp:Label>
                                            <%-- End --%>
                                          <td><%#Eval("Comments") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="CommentsLabel" runat="server" Visible="false" Text='<%# Eval("Comments")%>'></asp:Label>
                                            <%-- End --%>
                                          <td><%#Eval("BeatName") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="BeatNameLabel" runat="server" Visible="false" Text='<%# Eval("BeatName")%>'></asp:Label>
                                            <%-- End --%>
                                          <td><%#Eval("valueofRetailer") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="valueofRetailerLabel" runat="server" Visible="false" Text='<%# Eval("valueofRetailer")%>'></asp:Label>
                                            <%-- End --%>

                                           <td><%#Eval("Actualusercount") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="ActualusercountLabel" runat="server" Visible="false" Text='<%# Eval("Actualusercount")%>'></asp:Label>
                                            <%-- End --%>
                                           <td><%#Eval("BalanceQty") %></td>
                                        <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="BalanceQtyLabel" runat="server" Visible="false" Text='<%# Eval("BalanceQty")%>'></asp:Label>
                                            <%-- End --%>
                                         <td><%#Eval("imageupload") %></td>
                                        <asp:Label ID="lblImageUpload" runat="server" Visible="false" Text='<%# Eval("imageupload")%>'></asp:Label>


                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>

                        <div class="box-header">
                            <h3 class="box-title"></h3>
                        </div>

                        <div class="col-md-12">

                            <div class="form-group">
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="rpt_ItemCommand">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <th>Sr.No.</th>
                                                        <th>Meet Date</th>
                                                        <th>MeetName</th>
                                                        <th>Venue</th>
                                                        <th>Distributor Name</th>
                                                        <th>No of Users</th>
                                                        <th>Product Class</th>
                                                        <th>Approx Budget</th>
                                                        <th>Meet Status</th>
                                                        <th>Approval Remark</th>
                                                        <th>Approval Date</th>

                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Container.ItemIndex+1 %>                                          
                                                </td>

                                                <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("MeetDate"))%></td>
                                                <td><%#Eval("MeetName") %></td>
                                                <td><%#Eval("Venue") %></td>
                                                <td><%#Eval("PartyName") %></td>
                                                <td><%#Eval("NoOfUser") %></td>
                                                <td><%#Eval("IndName") %></td>
                                                <td style="text-align:right;"><%#Eval("LambBudget") %></td>
                                                <td><%#Eval("AppStatus") %></td>
                                                <td><%#Eval("AppRemark") %></td>
                                                <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("Appdate"))%></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>     </table>       
                                        </FooterTemplate>

                                    </asp:Repeater>
                                </div>

                            </div>

                            <div class="form-group">
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <div class="box-header">
                            <h3 class="box-title">
                                <asp:Label ID="lblnouser" runat="server" Text="No of Users"></asp:Label></h3>
                            <br />
                             <asp:Button ID="btnNOUExport" class="btn btn-primary" Style="padding: 3px 14px;" runat="server" Text="Export" OnClick="btnNouexport_Click" />
                        </div>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rptnoofusers" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Sr.No.</th>
                                                <th>Sales Person</th>
                                                <th>Meet Date</th>
                                                <th>Meet</th>
                                                <th>Area</th>
                                                <th>Beat</th>
                                                <th>Party Name</th>
                                                <th>Contact Person</th>
                                                <th>Mobile No</th>
                                                <th>EmailId</th>
                                                <th>Potential</th>
                                                <th>DOB</th>
                                                <th>Address</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>  <td><%#Container.ItemIndex+1 %>                             
                                        </td>
                                        <td><%#Eval("SMName")%></td>
                                                  <asp:Label ID="SMName" runat="server" Visible="false" Text='<%# Eval("SMName")%>'></asp:Label>
                                                  <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("MeetDate"))%></td>
                                                  <asp:Label ID="MeetDate" runat="server" Visible="false" Text='<%# Eval("MeetDate")%>'></asp:Label>
                                                  <td><%#Eval("MeetName")%></td>
                                                  <asp:Label ID="MeetName" runat="server" Visible="false" Text='<%# Eval("MeetName")%>'></asp:Label>
                                                  <td><%#Eval("AreaName")%></td>
                                                  <asp:Label ID="AreaName" runat="server" Visible="false" Text='<%# Eval("AreaName")%>'></asp:Label>
                                                  <td><%#Eval("BeatName")%></td>
                                                  <asp:Label ID="BeatName" runat="server" Visible="false" Text='<%# Eval("BeatName")%>'></asp:Label>
                                                  <td><%#Eval("Name")%></td>
                                                  <asp:Label ID="Name" runat="server" Visible="false" Text='<%# Eval("Name")%>'></asp:Label>
                                                  <td><%#Eval("ContactPersonName")%></td>
                                                  <asp:Label ID="ContactPersonName" runat="server" Visible="false" Text='<%# Eval("ContactPersonName")%>'></asp:Label>
                                                <td><%#Eval("MobileNo")%></td>
                                                     <asp:Label ID="MobileNo" runat="server" Visible="false" Text='<%# Eval("MobileNo")%>'></asp:Label>
                                                <td><%#Eval("EmailId")%></td>
                                                     <asp:Label ID="EmailId" runat="server" Visible="false" Text='<%# Eval("EmailId")%>'></asp:Label>
                                                  <td><%#Eval("Potential")%></td>
                                                     <asp:Label ID="Potential" runat="server" Visible="false" Text='<%# Eval("Potential")%>'></asp:Label>
                                                <td><%#Eval("DOB")%></td>
                                                     <asp:Label ID="DOB" runat="server" Visible="false" Text='<%# Eval("DOB")%>'></asp:Label>
                                                <td><%#Eval("Address")%></td>
                                                     <asp:Label ID="Address" runat="server" Visible="false" Text='<%# Eval("Address")%>'></asp:Label>


                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                      



                        <div class="box-header">
                            <h3 class="box-title">
                                <asp:Label ID="lblproductist" runat="server" Text="Product List"></asp:Label></h3>
                        </div>
                        <div class="col-md-12">

                            <div class="form-group">
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="rptproductList" runat="server" OnItemCommand="rpt_ItemCommand">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <th>Sr.No.</th>
                                                        <th>Product Class</th>
                                                        <th>Product Segment</th>
                                                        <th>Product Group</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Container.ItemIndex+1 %>                                          
                                                </td>
                                                <td><%#Eval("MatrialClass")%></td>
                                                <td><%#Eval("Segment")%></td>
                                                <td><%#Eval("ProdctGroup")%></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>     </table>       
                                        </FooterTemplate>

                                    </asp:Repeater>
                                </div>

                            </div>

                            <div class="form-group">
                            </div>
                        </div>
                              <div class="box-header">
                            <h3 class="box-title">
                                <asp:Label ID="lblImage" runat="server" Text="Image List"></asp:Label></h3>
                        </div>
                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="rptImageUpload" runat="server" OnItemCommand="rptImageUpload_ItemCommand">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <th>Sr.No.</th>
                                                        <th>Meet Name</th>
                                                         <th>View Image</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Container.ItemIndex+1 %>                                          
                                                </td>
                                                 <asp:Label ID="ImageId" runat="server" Visible="false" Text='<%# Eval("Id")%>' ></asp:Label>
                                                <td><%#Eval("Meet")%></td>
                                               <td><asp:LinkButton id="url" runat="server" Text="ViewImage" CommandName="ShowImage" CommandArgument='<%#Eval("Id")%>' /></td>
                                                <%--<td><%#Eval("ImgUrl")%></td>--%>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>     </table>       
                                        </FooterTemplate>

                                    </asp:Repeater>
                                </div>

                            </div>

                            <div class="form-group">
                            </div>
                        </div>
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
                //"order": [[1, "desc"]]
            });

        });
    </script>
</asp:Content>
