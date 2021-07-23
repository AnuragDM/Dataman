<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SalesPersonL3UC.ascx.cs" Inherits="AstralFFMS.SalesPersonL3UC" %>
<section class="content-header">

    <script src="dist/js/demo.js" type="text/javascript"></script>
    <!-- Custom Style Sheet  -->
    <link href="Content/style.css" rel="stylesheet" />
    <style>
        .circle-cont1 {
            background: #6699ff none repeat scroll 0 0;
            border-radius: 100px;
            height: 113px !important;
            width: 118px !important;
            border-width: 0px;
            color: white;
            white-space: normal;
        }

        .quicklink-cont12 {
            background: #6699ff none repeat scroll 0 0;
            border-radius: 50px;
            height: 30px;
            min-width: 90%;
            border-width: 0px;
            color: white;
            white-space: normal;
            margin-left: 4px;
            margin-bottom: 15px;
        }

        #rcorners2, #rcorners12, #rcornersMsg, .rcorners123 {
            border-radius: 25px;
            border: 1px solid #d9d3d6;
        }

        .right-links #rcorners12 {
            padding: 18px 10px;
        }

        .rcorners2 {
            border-radius: 25px;
            border: 1px solid #d9d3d6;
        }

        .multiselect-container > li {
            width: 212px;
        }

            .multiselect-container > li > a {
                white-space: normal;
            }

        .input-group .form-control {
            height: 34px;
        }
        /*.small-boxheight {
            height: 73px;
            padding-top: 13px;
        }*/
    </style>
    <style type="text/css">
        #ContentPlaceHolder1_SalesPersonL1UC_btnGo, #ContentPlaceHolder1_SalesPersonL1UC_btnGoSecSale {
            height: 30px;
            padding: 1px 15px;
        }
    </style>
    <h1>L3 -Dashboard
        <%--<small>Dashboard</small>--%>
    </h1>
    <ol class="breadcrumb" hidden>
        <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
        <li class="active">Dashboard</li>
    </ol>
</section>

<section class="content">
    <!-- Small boxes (Stat box) -->
    <div class="row" id="dashboard" style="display: none;">
        <div class="col-lg-3 col-xs-6 bxwidth">
            <!-- small box -->
            <div class="small-box bg-aqua small-boxheight">
                <div class="inner">
                    <%--<asp:Button ID="Primary" Style="margin-right: 5px;" runat="server"
                        Text="Primary Sales Month" OnClick="Primary_Click" />--%>
                </div>
            </div>
        </div>
        <!-- ./col -->
        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-green small-boxheight">
                <div class="inner">
                    <%--<asp:Button ID="Secondary" Style="margin-right: 5px;" runat="server"
                        Text="Secondary Sales Month" OnClick="Secondary_Click" />--%>
                </div>
            </div>
        </div>
        <!-- ./col -->
        <div class="col-lg-3 col-xs-6 bxwidth">
            <!-- small box -->
            <div class="small-box bg-yellow small-boxheight">
                <div class="inner">
                    <%-- <asp:Button ID="LeaveReq" Style="margin-right: 5px;" runat="server"
                        Text="Leave Request" OnClick="LeaveReq_Click" />--%>
                </div>
            </div>
        </div>
        <!-- ./col -->
        <div class="clearfix mo-clearfix"></div>

        <!-- ./col -->

        <div class="col-lg-3 col-xs-6 bxwidth">
            <!-- small box -->
            <div class="small-box bg-red small-boxheight">
                <div class="inner">
                    <%--<asp:Button ID="ExpensesL3" Style="margin-right: 5px;" runat="server"
                        Text="Expenses of L3" OnClick="ExpensesL3_Click" />--%>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-6 bxwidth">
            <!-- small box -->
            <div class="small-box bg-purple small-boxheight">
                <div class="inner">
                    <%-- <asp:Button ID="PendingDSRApp" Style="margin-right: 5px;" runat="server"
                        Text="Pending DSR Approval" OnClick="PendingDSRApp_Click" />--%>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-6 bxwidth">
            <!-- small box -->
            <div class="small-box bg-olive small-boxheight">
                <div class="inner">
                    <%-- <asp:Button ID="TodayL2Loc" Style="margin-right: 5px;" runat="server"
                        Text="Today's L2 Location" OnClick="TodayL2Loc_Click" />--%>
                </div>
            </div>
        </div>

        <div class="clearfix mo-clearfix"></div>
        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-teal small-boxheight">
                <div class="inner">
                    <%-- <asp:Button ID="OutstandingReport" Style="margin-right: 5px;" runat="server"
                        Text="Distributor Wise Outstanding Report" OnClick="OutstandingReport_Click" />--%>
                </div>

            </div>
        </div>
        <!-- Small boxes (Stat box) -->

    </div>

    <div class="box-body">
        <div class="row">
            <div class="col-md-9">
                <div id="rcorners2" style="background-color: white; padding: 0px 0px 18px 0px; margin-bottom: 5px;">
                    <div class="row">
                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 text-center">
                            <!-- small box -->
                            <div>
                                <div>
                                    <h3></h3>
                                    <%--  <input style="margin-right: 5px;" type="button" id="Primary " value="Primary Sales Month" class="circle-cont1" onclick="callDiv()" />--%>
                                    <asp:Button ID="Primary" runat="server" class="circle-cont1"
                                        Text="Primary Sales" OnClick="Primary_Click" />
                                </div>
                            </div>
                        </div>
                        <!-- ./col -->
                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 text-center">
                            <!-- small box -->
                            <div>
                                <div>
                                    <h3></h3>
                                    <%--<input style="margin-right: 5px;" type="button" id="Secondary" value="Secondary Sales For The Month" class="circle-cont1" 
                                        onclick="callSecDiv()" />--%>
                                    <asp:Button ID="Secondary" runat="server" class="circle-cont1"
                                        Text="Secondary Sales" OnClick="Secondary_Click" />
                                </div>
                            </div>
                        </div>
                        <!-- ./col -->
                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 text-center">
                            <!-- small box -->
                            <div>
                                <div>
                                    <h3></h3>
                                    <%--<input style="margin-right: 5px;" type="button" id="LeaveRequest" value="LeaveRequest" class="circle-cont1" 
                                        onclick="callLeaveDiv()" />--%>
                                    <asp:Button ID="LeaveReq" runat="server" class="circle-cont1"
                                        Text="Leave Details" OnClick="LeaveReq_Click" />
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 text-center">
                            <!-- small box -->
                            <div>
                                <div>
                                    <h3></h3>
                                    <%-- <input style="margin-right: 5px;" type="button" id="OutstandingReport" value="Distributor Wise Outstanding Report" 
                                        class="circle-cont1" onclick="callOutReportDiv()" />--%>
                                    <asp:Button ID="OutstandingReport" runat="server" class="circle-cont1"
                                        Text="Distributor Wise Outstanding" OnClick="OutstandingReport_Click" />
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 text-center">
                            <div>
                                <div>
                                    <h3></h3>
                                    <%--<input style="margin-right: 5px;" type="button" id="Expenses" value="Expenses Of L3" class="circle-cont1" 
                                        onclick="callExpenseDiv()" />--%>
                                    <asp:Button ID="ExpensesL3" runat="server" class="circle-cont1"
                                        Text="Expense Details" OnClick="ExpensesL3_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 text-center">
                            <div>
                                <div>
                                    <h3></h3>
                                    <%-- <input style="margin-right: 5px;" type="button" id="PendingDSR" value="Pending DSR Approval of L2" class="circle-cont1" 
                                        onclick="callPendingDSRDiv()" />--%>
                                    <asp:Button ID="PendingDSRApp" runat="server" class="circle-cont1"
                                        Text="DSR Pending For Approval Of L2" OnClick="PendingDSRApp_Click" />
                                </div>
                            </div>
                        </div>
                        <!-- ./col -->
                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 text-center">
                            <div>
                                <div>
                                    <h3></h3>
                                    <%-- <input style="margin-right: 5px;" type="button" id="LeaveReq" value="Today's L2 Location" class="circle-cont1" 
                                        onclick="callL2LocationDiv()" />--%>
                                    <asp:Button ID="TodayL2Loc" runat="server" class="circle-cont1"
                                        Text="Planned Today's City (DSR)" OnClick="TodayL2Loc_Click" />
                                </div>
                            </div>
                        </div>

                        <%-- Added 08-12-2015 --%>
                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 text-center">
                            <div>
                                <div>
                                    <h3></h3>
                                    <asp:Button ID="DSRREMARK" runat="server" class="circle-cont1"
                                        Text="DSR Remark" OnClick="DSRREMARK_Click" />
                                </div>

                            </div>
                        </div>
                        <%-- End --%>
                    </div>
                </div>
            </div>
            <div class="col-md-3 right-links">
                <div id="rcorners12" style="background-color: white; min-height: 286px;">
                    <div class="row-fluid" style="text-align: center;">
                        <label for="quickLink"><b>Quick Links</b></label>
                    </div>
                    <div class="row-fluid text-center">
                        <asp:Button ID="DSRLevel3" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="DSR Entry" OnClick="DSRLevel3_Click" />
                    </div>
                     <div class="row-fluid text-center">
                        <asp:Button ID="L3RemarkEntry" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="DSR L-3 Remark Entry" OnClick="L3RemarkEntry_Click" />
                    </div>
                    <div class="row-fluid text-center">
                        <asp:Button ID="TourExpL3" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="Expense Entry" OnClick="TourExpL3_Click" />
                    </div>
                    <div class="row-fluid text-center">
                        <asp:Button ID="BeatPlanAppL3" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="Beat Plan Approval" OnClick="BeatPlanAppL3_Click" />
                    </div>
                    <div class="row-fluid text-center">
                        <asp:Button ID="LeaveApprvL2" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="Leave Approval" OnClick="LeaveApprvL2_Click" />
                    </div>
                    <div class="row-fluid text-center" style="display: none;">
                        <asp:Button ID="LocalExpenseL3" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="Local Expense" OnClick="LocalExpenseL3_Click" />
                    </div>
                    <div class="row-fluid text-center">
                        <asp:Button ID="btnLocationTracker" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="Location Tracker" OnClientClick="window.document.forms[0].target='_blank'; setTimeout(function(){window.document.forms[0].target='';}, 500);" OnClick="btnLocationTracker_Click" />
                        <%--<asp:HyperLink ID="HypLocationTracker" Style="margin-right: 5px;" CssClass="quicklink-cont12" runat="server" NavigateUrl="http://follow.astral.dataman.net.in/" Target="_blank">Location Tracker</asp:HyperLink>--%>
                    </div>
                    <div class="row-fluid text-center">
                        <asp:Button ID="DownloadsL3" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="Downloads" OnClick="DownloadsL3_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-9 col-lg-9" style="margin-top: 5px;">
                <div id="reportcontainer" class="box-body" style="display: none; padding: 0;" runat="server">


                    <div class="rcorners123" id="PrimaryJQX" style="display: none; padding: 5px; background-color: white;" runat="server">
                        <div class="box-header">
                            <h3 class="box-title">Primary Sales:</h3>
                            <asp:DropDownList ID="monthDDL" runat="server"></asp:DropDownList>&nbsp;&nbsp;<asp:DropDownList ID="yearDDL" runat="server">
                            </asp:DropDownList>&nbsp;&nbsp;<asp:Button type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClick="btnGo_Click" />
                        </div>
                        <div>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="rptprimsale" runat="server">
                                    <HeaderTemplate>
                                        <table id="primarySaleTable" class="table table-bordered table-striped rpttable">
                                            <thead>
                                                <tr>
                                                    <%--<th>SNo</th>--%>
                                                    <th>Sales Person</th>
                                                    <th>Distributor Name</th>
                                                     <th>Product Class</th>
                                                    <th>Product Group</th>
                                                    <th>Last 6 Month Avg.</th>
                                                    <th>Last Yr.Curr. Month Amt.</th>
                                                    <th>Curr. Month Amt.</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <%--<td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>--%>
                                            <td><%#Eval("SMName") %></td>
                                            <td><%#Eval("PartyName") %></td>
                                             <td><%#Eval("ItemClass") %></td>
                                            <td><%#Eval("materialGroup") %></td>
                                            <td style="text-align: right;"><%#Eval("MonthAvg") %></td>
                                            <td style="text-align: right;"><%#Eval("PreAmt") %></td>
                                            <td style="text-align: right;"><%#Eval("Amount") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>


                    <div class="rcorners123" id="SecondaryJQX" style="display: none; padding: 5px; background-color: white;" runat="server">
                        <div class="box-header">
                            <h3 class="box-title">Secondary Sales:</h3>
                            <asp:DropDownList ID="ddlMonthSecSale" runat="server"></asp:DropDownList>&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlYearSecSale" runat="server">
                    </asp:DropDownList>&nbsp;&nbsp;<asp:Button type="button" ID="btnGoSecSale" runat="server" Text="Go"
                        class="btn btn-primary" OnClick="btnGoSecSale_Click" />
                        </div>
                        <div>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="secsalerpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="secondarySaleTable" class="table table-bordered table-striped rpttable">
                                            <thead>
                                                <tr>
                                                    <%-- <th style="text-align: center; width: 6%">SNo</th>--%>
                                                    <th style="text-align: left; width: 30%">Sales Person</th>
                                                     <th style="text-align: left; width: 30%">Party</th>
                                                    <th style="text-align: left; width: 30%" hidden>Employee Name</th>
                                                    <th style="text-align: right; width: 20%">Amount</th>
                                                    <th style="text-align: center; display: none;">Pri. Sale</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <%--  <td style="text-align: center; width: 6%"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>--%>
                                            <td style="text-align: left; width: 30%"><%#Eval("SMName") %></td>
                                               <td style="text-align: left; width: 30%"><%#Eval("PartyName") %></td>
                                            <td style="text-align: left; width: 30%" hidden><%#Eval("EmpName") %></td>
                                            <td style="text-align: right; width: 20%"><%#Eval("OrderAmount") %></td>
                                            <td style="text-align: right; display: none;"><%#Eval("Sale") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>


                    <div class="rcorners123" id="LeaveJQX" style="display: none; padding: 5px; background-color: white;" runat="server">
                        <div class="box-header">
                             <h3 class="box-title">Leave Details:</h3>
                            <br /><br />
                            <div class="col-md-4 col-sm-4">
                                <b>Pending Leave Request :</b>&nbsp;&nbsp;
                        <asp:Label ID="LeavesLabel" runat="server" Text="Label" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-md-4 col-sm-5">
                                <b>Approved Leave Request :</b>&nbsp;&nbsp;
                              <asp:Label ID="TotalLeavesLabel" runat="server" Text="Label" Font-Bold="true"></asp:Label>
                            </div>
                            <br />
                        </div>
                        <div>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="leaverpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="leaveTable" class="table table-bordered table-striped rpttableLeave">
                                            <thead>
                                                <tr>
                                                    <%--<th style="text-align: center; width: 6%">S.No</th>--%>
                                                    <th style="text-align: center; width: 18%">Date</th>
                                                    <th style="text-align: center;width: 28%"">Sales Person</th>
                                                    <th style="text-align: center; width: 18%">From Date</th>
                                                    <th style="text-align: center; width: 18%">To Date</th>
                                                    <th style="text-align: center; width: 7%">Days</th>
                                                    <th style="text-align: left;">Reason</th>
                                                    <th style="text-align: center; width: 13%">Status</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <%--<td style="text-align: center; width: 6%"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>--%>
                                            <td style="text-align: center; width: 18%"><%#Eval("VDate","{0:dd/MMM/yyyy}") %></td>
                                            <th style="text-align: left;"><%#Eval("SMName") %></th>
                                            <td style="text-align: center; width: 18%"><%#Eval("FromDate","{0:dd/MMM/yyyy}") %></td>
                                            <td style="text-align: center; width: 18%"><%#Eval("ToDate","{0:dd/MMM/yyyy}") %></td>
                                            <td style="text-align: right; width: 7%"><%#Eval("NoOfDays") %></td>
                                            <td style="text-align: left; width: 30%"><%#Eval("Reason") %></td>
                                            <td style="text-align: center; width: 13%"><%#Eval("AppStatus") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>


                    <div class="rcorners123" id="DistJQX" style="display: none; padding: 5px; background-color: white;" runat="server">
                        <div class="box-header">
                            <h3 class="box-title">Distributor Wise Outstanding:</h3>
                        </div>
                        <div>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="rptDistOutRep" runat="server">
                                    <HeaderTemplate>
                                        <table id="dsrTable" class="table table-bordered table-striped rpttable">
                                            <thead>
                                                <tr>
                                                    <%-- <th style="text-align: center; width: 6%">S.No</th>--%>
                                                    <th style="text-align: left; width: 35%">Distributor Name</th>
                                                    <th style="text-align: right; width: 40%">Outstanding Balance</th>
                                                    <th style="text-align:center;">View Ledger</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <%-- <td style="text-align: center; width: 6%"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>--%>
                                            <td style="text-align: left; width: 35%"><%#Eval("partyname") %></td>
                                            <%--<td style="text-align: right; width: 15%"><%#Eval("Balance") %></td>--%>
                                            <td style="text-align: right;width: 40%"><%#Convert.ToDecimal(Eval("Balance")) > 0 ? Eval("Balance")+ " Dr"  :(-1)* Convert.ToDecimal(Eval("Balance")) + " Cr" %></td>
                                            <td style="text-align: center; ">
                                                <a href="<%# String.Format("/DetailDistLedger.aspx?"+"DistId="+Eval("DistId")) %>">View Ledger</a>
                                                <%--   <asp:LinkButton ID="LinkButton1" runat="server">Details</asp:LinkButton>--%></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>

                    <%-- Added 08-12-2015 --%>
                    <div class="rcorners123" id="DSRJQX" style="display: none; padding: 5px; background-color: white;" runat="server">
                        <div class="box-header">
                            <h3 class="box-title">DSR Remark:</h3>

                            <asp:DropDownList ID="ddlDSRMonth" runat="server"></asp:DropDownList>&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlDSRYear" runat="server">
                    </asp:DropDownList><br />
                            <label for="exampleInputEmail1">Sales Person:</label><br />
                            <asp:ListBox ID="LstSalesperson" runat="server" SelectionMode="Multiple"></asp:ListBox>
                            &nbsp;&nbsp;<asp:Button type="button" ID="btnSalesperson" runat="server" Text="Go"
                                class="btn btn-primary" OnClick="btnSalesperson_Click" />

                        </div>
                        <div>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="dsrrpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="dsrTable1" class="table table-bordered table-striped dsrTable">
                                            <thead>
                                                <tr>
                                                    <%--<th style="text-align: center; width: 6%">S.No</th>--%>
                                                    <th style="text-align: center; width: 18%">Visit Date</th>
                                                    <th style="text-align: left;">Sales Person</th>
                                                    <th style="text-align: left;">DSR Remark</th>
                                                    <th style="text-align: left;">DSR Approved Remarks</th>
                                                    <th style="text-align: left;">Status</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <%--<td style="text-align: center; width: 6%"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>--%>
                                            <td style="text-align: center; width: 18%"><%#Eval("VDate","{0:dd/MMM/yyyy}") %></td>
                                            <td style="text-align: left;"><%#Eval("SMName") %></td>
                                            <td style="text-align: left;"><%#Eval("Remark") %></td>
                                            <td style="text-align: left;"><%#Eval("AppRemark") %></td>
                                            <td style="text-align: left;"><%#Eval("AppStatus") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                    <%-- End --%>

                    <div class="rcorners123" id="ExpenseJQX" style="display: none; padding: 5px; background-color: white;" runat="server">
                        <div class="box-header">
                            <h3 class="box-title">Expense Details:</h3>
                            <asp:DropDownList ID="ddlMonthExp" runat="server"></asp:DropDownList>&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlYearExp" runat="server">
                    </asp:DropDownList><br />
                            <label for="exampleInputEmail1">Sales Person:</label><br />

                            <asp:ListBox ID="salespersonListBox" runat="server" SelectionMode="Multiple"></asp:ListBox>
                            &nbsp;&nbsp;<asp:Button type="button" ID="btnExpGo" runat="server" Text="Go"
                                class="btn btn-primary" OnClick="btnExpGo_Click" />

                        </div>
                        <div>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="exprpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="expenseTable" class="table table-bordered table-striped rpttable">
                                            <thead>
                                                <tr>

                                                    <th style="text-align: center; width: 12%">Date</th>
                                                    <%-- <th style="text-align: center; width: 7%">Local Exp.</th>
                                                    <th style="text-align: center; width: 7%">Passed Amt.</th>--%>
                                                    <th style="text-align: center; width: 18%">Sales Person</th>
                                                      <th style="text-align: center; width: 18%">Expense Name</th>
                                                    <th style="text-align: center; width: 18%">Bill Amount</th>
                                                    <th style="text-align: center; width: 18%">Claim Amount</th>
                                                    <th style="text-align: right; width: 20%">Approved Amount</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>

                                            <td style="text-align: center; width: 12%"><%#Eval("BillDate","{0:dd/MMM/yyyy}") %></td>
                                            <%-- <td style="text-align: right; width: 7%"></td>--%>
                                            <td style="text-align: left; width: 18%"><%#Eval("SMName") %></td>
                                            <td style="text-align: left; width: 18%"><%#Eval("ExpenseName") %></td>
                                            <td style="text-align: right; width: 18%"><%#Eval("billamount") %></td>
                                            <td style="text-align: right; width: 18%"><%#Eval("claimamount") %></td>
                                            <td style="text-align: right; width: 20%"><%#Eval("ApprovedAmount") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>


                    <div class="rcorners123" id="PendingDSRJQX" style="display: none; padding: 5px; background-color: white;" runat="server">
                        <div class="box-header"><%--Pending DSR Approval of L2:--%>
                            <h3 class="box-title">DSR Pending For Approval Of L2:</h3>
                        </div>
                        <div>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="penddsrrpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="tourTable" class="table table-bordered table-striped rpttable">
                                            <thead>
                                                <tr>
                                                    <%--<th style="text-align: center; width: 6%">S.No</th>--%>
                                                    <th style="text-align: left; width: 35%">L2 Name</th>
                                                    <th style="text-align: left; width: 35%">Sales Person</th>
                                                    <th style="text-align: right; width: 15%">Pending DSR</th>
                                                    <th style="visibility: hidden; width: 10%">Details</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <%--<td style="text-align: center; width: 6%"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>--%>
                                            <td style="text-align: left; width: 35%"><%#Eval("SMName") %></td>
                                            <td style="text-align: left; width: 35%"><%#Eval("EmpName") %></td>
                                            <td style="text-align: right; width: 15%"><%#Eval("Pending") %></td>
                                            <td style="text-align: center; width: 10%">
                                                <%-- <a href="<%# String.Format("/PendingDSRList.aspx?"+"SMId="+Eval("SMId")) %>">Details</a>--%>
                                                <a href="<%# String.Format("/NewRptDailyWorkingApprovalL2.aspx?"+"SMId="+Eval("SMId")) %>" target="_blank">Details</a>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>

                    <div class="rcorners123" id="L2LocationJQX" style="display: none; padding: 5px; background-color: white;" runat="server">
                        <div class="box-header">
                            <h3 class="box-title">Planned Today's City (DSR):</h3>
                        </div>
                        <div>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="l2locrpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="tourTable" class="table table-bordered table-striped rpttable">
                                            <thead>
                                                <tr>
                                                    <%-- <th style="text-align: center; width: 6%">S.No</th>--%>
                                                    <th style="text-align: left; width: 35%">Sales Person</th>
                                                    <th style="text-align: left; width: 15%">City</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <%--   <td style="text-align: center; width: 6%"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>--%>
                                            <td style="text-align: left; width: 35%"><%#Eval("SMName") %></td>
                                            <td style="text-align: left; width: 15%"><%#Eval("AreaName") %></td>
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
            </div>
            <div class="col-md-3" style="margin-top: 5px;">
                <div class="right-links">
                    <div id="rcornersMsg" style="background-color: white; min-height: 300px;">
                        <div class="row-fluid" style="text-align: center;">
                            <label for="quickLink"><b>Messages</b></label>
                        </div>

                        <marquee onmouseover="this.stop()" onmouseout="this.start()" scrollamount="2" direction="up" width="100%" height="247px" align="center">
                                  <div class="row">
	    		                        <div class="col-lg-11">
	    			
	    				                        <ul id="nt-title">
                                                     <asp:Repeater ID="Repeater1" runat="server">
                                                <ItemTemplate>
                                                    <li >
                                                       <p><%#Eval("Message") %></p>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
		    				                       <%--<li>
		    					                        Dear All, Welcome to our new portal.
		    				                        </li>
		    				                        <li>
		    					                        Provides hight flexibility thanks to numerous callbacks & methods.
		    				                        </li>
		    				                        <li>
		    					                        Fully customizable to every kind of customer needs.
		    				                        </li>
		    				                        <li>
		    					                        Light-weight and optimized web application.
		    				                        </li>--%>
	    				                        </ul>
	    		                        </div><!-- /col-lg-8 -->
	                                    	</div><!-- /row -->
                                    </marquee>
                    </div>
                </div>
            </div>
        </div>
    </div>

</section>
<!-- right col -->
