<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SalesPersonL1UC.ascx.cs" Inherits="AstralFFMS.SalesPersonL1UC" %>
<section class="content-header">

    <script src="dist/js/demo.js" type="text/javascript"></script>
    <!-- Custom Style Sheet  -->
    <link href="Content/style.css" rel="stylesheet" />


    <style>
        #dsrTable_filter {
            float: right;
        }

        .circle-cont1 {
            background: #6699ff none repeat scroll 0 0;
            border-radius: 100px;
            height: 113px !important;
            width: 118px !important;
            border-width: 0px;
            color: white;
            white-space: normal;
        }

        /*.quicklink-cont1 {
            background: #6699ff none repeat scroll 0 0;
            border-radius: 50px;
            height: 30px;
            min-width: 90%;
            border-width: 0px;
            color: white;
            white-space: normal;
            margin-left: 4px;
            margin-bottom: 15px;
        }*/
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
            padding: 4px;
        }

        .rcorners2 {
            border-radius: 25px;
            border: 1px solid #d9d3d6;
        }

        .right-links #rcorners12 {
            padding: 16.5px 10px;
        }

        .small-boxheight {
            height: 73px;
            padding-top: 13px;
        }
    </style>
    <h1>L1 -Dashboard
           <%-- <small>Dashboard</small>--%>
    </h1>
    <ol class="breadcrumb" hidden>
        <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
        <li class="active">Dashboard</li>
    </ol>
</section>

<style type="text/css">
    #ContentPlaceHolder1_SalesPersonL1UC_btnGo, #ContentPlaceHolder1_SalesPersonL1UC_btnGoSecSale {
        height: 30px;
        padding: 1px 15px;
    }
</style>
<section class="content">

    <div class="row" id="dashboard" style="display: none;">
        <div class="col-lg-3 col-xs-6 bxwidth">
            <!-- small box -->
            <div class="small-box bg-aqua small-boxheight">
                <div class="inner">
                    <%--  <asp:Button ID="Primary" Style="margin-right: 5px;" runat="server"
                        Text="Primary Sales Month" OnClick="Primary_Click" />--%>
                </div>
            </div>
        </div>
        <!-- ./col -->
        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-green small-boxheight">
                <div class="inner">
                    <%-- <asp:Button ID="Secondary" Style="margin-right: 5px;" runat="server"
                        Text="Secondary Sales Month" OnClick="Secondary_Click" />--%>
                </div>
            </div>
        </div>
        <!-- ./col -->
        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-yellow small-boxheight">
                <div class="inner">
                    <%-- <asp:Button ID="BeatPlan" Style="margin-right: 5px;" runat="server"
                        Text="Beat Paln For The Month" OnClick="BeatPlan_Click" />--%>
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
                    <%--<asp:Button ID="Expenses" Style="margin-right: 5px;" runat="server"
                        Text="Expenses" OnClick="Expenses_Click" />--%>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-6 bxwidth">
            <!-- small box -->
            <div class="small-box bg-purple small-boxheight">
                <div class="inner">
                    <%-- <asp:Button ID="DSRREMARK" Style="margin-right: 5px;" runat="server"
                        Text="DSR Remark" OnClick="DSRREMARK_Click" />--%>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-6 bxwidth">
            <!-- small box -->
            <div class="small-box bg-olive small-boxheight">
                <div class="inner">
                    <%--<asp:Button ID="LeaveReq" Style="margin-right: 5px;" runat="server"
                        Text="Leave Request" OnClick="LeaveReq_Click" />--%>
                </div>
            </div>
        </div>
        <div class="clearfix mo-clearfix"></div>
        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-teal small-boxheight">

                <div class="inner">
                    <%--<asp:Button ID="OutstandingReport" Style="margin-right: 5px;" runat="server"
                        Text="Distributor Wise Outstanding Report" OnClick="OutstandingReport_Click" />--%>
                </div>

            </div>
        </div>
        <!-- Small boxes (Stat box) -->

    </div>

    <div class="box-body">
        <div class="row">
            <div class="col-md-9">
                <div id="rcorners2" style="background-color: white; padding: 0 0px 17px 0px; margin-bottom: 5px;">
                    <div class="row">
                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 text-center">
                            <!-- small box -->
                            <div>
                                <div>
                                    <h3></h3>
                                    <%--   <asp:Button ID="Primary" Style="margin-right: 5px;" runat="server" CssClass="circle-cont1"
                                    Text="Primary Sales Month" OnClick="Primary_Click" />--%>
                                    <asp:Button ID="Primary" runat="server" CssClass="circle-cont1"
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
                                    <%-- <asp:Button ID="Secondary" Style="margin-right: 5px;" runat="server" CssClass="circle-cont1"
                                    Text="Secondary Sales Month" OnClick="Secondary_Click" />--%>
                                    <asp:Button ID="Secondary" runat="server" CssClass="circle-cont1"
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
                                    <%-- <asp:Button ID="BeatPlan" Style="margin-right: 5px;" runat="server" CssClass="circle-cont1"
                                    Text="Beat Paln For The Month" OnClick="BeatPlan_Click" />--%>
                                    <asp:Button ID="BeatPlan" runat="server" CssClass="circle-cont1"
                                        Text="Approved Beat Plan Details" OnClick="BeatPlan_Click" />
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 text-center">
                            <!-- small box -->
                            <div>
                                <div>
                                    <h3></h3>
                                    <%-- <asp:Button ID="OutstandingReport" Style="margin-right: 5px;" runat="server" CssClass="circle-cont1"
                                    Text="Distributor Wise Outstanding Report" OnClick="OutstandingReport_Click" />--%>
                                    <asp:Button ID="OutstandingReport" runat="server" CssClass="circle-cont1"
                                        Text="Distributor Wise Outstanding" OnClick="OutstandingReport_Click" />
                                </div>
                            </div>
                        </div>


                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 text-center">
                            <div>
                                <div>
                                    <h3></h3>
                                    <%-- <asp:Button ID="Expenses" Style="margin-right: 5px;" runat="server" CssClass="circle-cont1"
                                    Text="Expenses" OnClick="Expenses_Click" />--%>
                                    <asp:Button ID="Expenses" runat="server" CssClass="circle-cont1"
                                        Text="Expense Details" OnClick="Expenses_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 text-center">
                            <div>
                                <div>
                                    <h3></h3>
                                    <%-- <asp:Button ID="DSRREMARK" Style="margin-right: 5px;" runat="server" CssClass="circle-cont1"
                                    Text="DSR Remark" OnClick="DSRREMARK_Click" />--%>
                                    <asp:Button ID="DSRREMARK" runat="server" CssClass="circle-cont1"
                                        Text="DSR Remark" OnClick="DSRREMARK_Click" />
                                </div>
                            </div>
                        </div>
                        <!-- ./col -->
                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 text-center">
                            <div>
                                <div>
                                    <h3></h3>
                                    <%-- <asp:Button ID="LeaveReq" Style="margin-right: 5px;" runat="server" CssClass="circle-cont1"
                                    Text="Leave Request" OnClick="LeaveReq_Click" />--%>
                                    <asp:Button ID="LeaveReq" runat="server" CssClass="circle-cont1"
                                        Text="Leave Details" OnClick="LeaveReq_Click" />
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <div class="col-md-3 right-links">
                <div id="rcorners12" style="background-color: white;">
                    <div class="row-fluid" style="text-align: center;">
                        <label for="quickLink"><b>Quick Links</b></label>
                    </div>
                    <div class="row-fluid text-center">
                        <asp:Button ID="DSREntry" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="DSR Entry" OnClick="DSREntry_Click" />
                        <%-- <input style="margin-right: 5px;" type="button" id="DSREntry" value="DSR Entry" class="quicklink-cont12" /><br />--%>
                    </div>
                    <div class="row-fluid text-center">
                        <asp:Button ID="DistOrder" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="Distributor Order" OnClick="DistOrder_Click" />
                        <%--  <input style="margin-right: 5px;" type="button" id="DistOrder" value="Distributor Order" class="quicklink-cont12" />--%>
                    </div>
                    <div class="row-fluid text-center">
                        <asp:Button ID="BeatEntry" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="Beat Plan Entry" OnClick="BeatEntry_Click" />
                        <%-- <input style="margin-right: 5px;" type="button" id="BeatEntry" value="Beat Plan Entry" class="quicklink-cont12" />--%>
                    </div>

                    <div class="row-fluid text-center">
                        <asp:Button ID="LocalExp" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="Expense Entry" OnClick="LocalExp_Click" />
                        <%-- <input style="margin-right: 5px;" type="button" id="LocalExp" value="Local Expense" class="quicklink-cont12" />--%>
                    </div>
                    <div class="row-fluid text-center">
                        <asp:Button ID="Downloads" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="Downloads" OnClick="Downloads_Click" />
                        <%-- <input style="margin-right: 5px;" type="button" id="Downloads" value="Downloads" class="quicklink-cont12" />--%>
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
                                                    <th>Distributor Name</th>
                                                    <th>Product Class</th>
                                                    <th>Product Group</th>
                                                    <th style="text-align: right">Last 6 Month Avg.</th>
                                                    <th style="text-align: right">Last Yr.Curr. Month Amt.</th>
                                                    <th style="text-align: right">Curr. Month Amt.</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <%--<td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>--%>
                                            <td><%#Eval("PartyName") %></td>
                                            <td><%#Eval("ItemClass") %></td>
                                            <td><%#Eval("materialGroup") %></td>
                                            <td style="text-align: right;"><%#Eval("MonthAvg") %></td>
                                            <td style="text-align: right;"><%#Eval("PreAmt") %></td>
                                            <td style="text-align: right;"><%#Eval("Amount") %></td>
                                            <%--  <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server">Details</asp:LinkButton></td>--%>
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
                                                    <%--<th style="text-align: center; width: 6%">SNo</th>--%>
                                                    <th style="text-align: left; width: 46%">Party Name</th>
                                                    <th style="text-align: right; width: 10%">Amount</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <%-- <td style="text-align: center; width: 6%"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>--%>
                                            <td style="text-align: left; width: 40%"><%#Eval("PartyName") %></td>
                                            <td style="text-align: right; width: 10%; padding-right: 20px;"><%#Eval("OrderAmount") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>


                    <div class="rcorners123" id="BeatJQX" style="display: none; padding: 5px; background-color: white;" runat="server">
                        <div class="box-header">
                            <h3 class="box-title"><b>Approved Beat Plan Details:<%--Beat Plan For The Month--%></b></h3>
                            <%-- <asp:DropDownList ID="ddlMonthBeatplan" runat="server"></asp:DropDownList>&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlbeatYearPlan" runat="server">
                    </asp:DropDownList>&nbsp;&nbsp;<asp:Button type="btnGoBeatplan" ID="Button1" runat="server" Text="Go"
                        class="btn btn-primary" OnClick="btnGoBeatplan_Click" />--%>
                        </div>
                        <div>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="beatplanrpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="beatTable" class="table table-bordered table-striped rpttable">
                                            <thead>
                                                <tr>
                                                    <th>Date</th>
                                                    <th>Beat</th>
                                                    <th>No. Of Parties</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#Eval("PlannedDate","{0:dd/MMM/yyyy}") %></td>
                                            <td><%#Eval("AreaName") %></td>
                                            <td style="text-align: center"><%#Eval("CustCount") %></td>
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
                                                    <%--<th style="text-align: center; width: 6%">S.No</th>--%>
                                                    <th style="text-align: left; width: 35%">Distributor Name</th>
                                                    <th style="text-align: right;">Outstanding Balance</th>
                                                    <th style="text-align: center;">View Ledger</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <%--<td style="text-align: center; width: 6%"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>--%>
                                            <td style="text-align: left; width: 35%"><%#Eval("partyname") %></td>
                                            <%--<td style="text-align: right; width: 15%"><%#Eval("Balance")+ " Dr" %></td>--%>
                                            <td style="text-align: right;"><%#Convert.ToDecimal(Eval("Balance")) > 0 ? Eval("Balance")+ " Dr"  :(-1)* Convert.ToDecimal(Eval("Balance")) + " Cr" %></td>
                                            <td style="text-align: center;">
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

                    <div class="rcorners123" id="ExpenseJQX" style="display: none; padding: 5px; background-color: white;" runat="server">
                        <div class="box-header">
                            <h3 class="box-title">Expense Details:</h3>
                            <asp:DropDownList ID="ddlMonthExpense" runat="server"></asp:DropDownList>&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlyearExpenses" runat="server">
                    </asp:DropDownList>&nbsp;&nbsp;<asp:Button type="btnGoExpenses" ID="Button2" runat="server" Text="Go"
                        class="btn btn-primary" OnClick="btnGoExpenses_Click" />
                        </div>
                        <div>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="exprpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="expenseTable" class="table table-bordered table-striped rpttable">
                                            <thead>
                                                <tr>
                                                    <%-- <th style="text-align: center; width: 6%">S.No</th>--%>
                                                    <th style="text-align: left; width: 12%">Date</th>
                                                    <%-- <th style="text-align: center; width: 7%">Local Exp.</th>
                                                    <th style="text-align: center; width: 7%">Passed Amt.</th>--%>
                                                    <th style="text-align: left; width: 18%">Sales Person</th>
                                                     <th style="text-align: center; width: 18%">Expense Name</th>
                                                    <th style="text-align: right; width: 18%">Bill Amount</th>
                                                    <th style="text-align: right; width: 18%">Claim Amount</th>
                                                    <th style="text-align: right; width: 20%">Approved Amount</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <%--<td style="text-align: center; width: 6%"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%>
                                            </td>--%>
                                            <td style="text-align: left; width: 12%"><%#Eval("BillDate","{0:dd/MMM/yyyy}") %></td>
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


                    <div class="rcorners123" id="DSRJQX" style="display: none; padding: 5px; background-color: white;" runat="server">
                        <div class="box-header">
                            <h3 class="box-title">DSR Remark:</h3>
                            <asp:DropDownList ID="ddlMonthRemark" runat="server"></asp:DropDownList>&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlyearRemark" runat="server">
                    </asp:DropDownList>&nbsp;&nbsp;<asp:Button type="btnGoDsrRemark" ID="Button3" runat="server" Text="Go"
                        class="btn btn-primary" OnClick="btnGoDsrRemark_Click" />
                        </div>
                        <div>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="dsrrpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="dsrTable" class="table table-bordered table-striped dsrTable">
                                            <thead>
                                                <tr>
                                                    <%--<th style="text-align: center; width: 6%">S.No</th>--%>
                                                    <th style="text-align: left; width: 18%">Visit Date</th>
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
                                            <td style="text-align: left; width: 18%"><%#Eval("VDate","{0:dd/MMM/yyyy}") %></td>
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
                                                    <th style="text-align: left; width: 18%">Date</th>
                                                    <%--  <th style="text-align: center;">Name</th>--%>
                                                    <th style="text-align: left; width: 18%">From Date</th>
                                                    <th style="text-align: left; width: 18%">To Date</th>
                                                    <th style="text-align: right; width: 7%">Days</th>
                                                    <th style="text-align: left;">Reason</th>
                                                    <th style="text-align: left; width: 13%">Status</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <%--  <td style="text-align: center; width: 6%"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%>
                                            </td>--%>
                                            <td style="text-align: left; width: 18%"><%#Eval("VDate","{0:dd/MMM/yyyy}") %></td>
                                            <%--  <th style="text-align: left; font-weight:normal;"><%#Eval("SMName") %></th>--%>
                                            <td style="text-align: left; width: 18%"><%#Eval("FromDate","{0:dd/MMM/yyyy}") %></td>
                                            <td style="text-align: left; width: 18%"><%#Eval("ToDate","{0:dd/MMM/yyyy}") %></td>
                                            <td style="text-align: right; width: 7%"><%#Eval("NoOfDays") %></td>
                                            <td style="text-align: left; width: 30%"><%#Eval("Reason") %></td>
                                            <td style="text-align: left; width: 13%"><%#Eval("AppStatus") %></td>
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
