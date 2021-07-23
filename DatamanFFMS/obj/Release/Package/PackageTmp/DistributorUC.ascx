<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DistributorUC.ascx.cs" Inherits="AstralFFMS.DistributorUC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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

        #newpartyTable_filter, #example1_filter, #invtable_filter, #dsrTable_filter {
            float: right;
        }

        @media screen and (min-width: 500px) and (max-width: 2500px) {
            .circular-distributor {
                padding: 10px;
            }
        }

        @media screen and (min-width: 200px) and (max-width: 500px) {
            .circular-distributor {
                padding: 20px;
            }
        }

        @media screen and (min-width: 200px) and (max-width: 500px) {
            .circular-distributor-in {
                padding: 0px;
            }
        }

        @media screen and (min-width: 200px) and (max-width: 1197px) {
            .circular-distributor {
            }
        }

        @media screen and (min-width: 1197px) and (max-width: 2000px) {
            .circular-distributor {
                height: 288px !important;
            }
        }

        @media screen and (min-width: 200px) and (max-width: 1197px) {
            #rcornersMsg {
            }
        }

        @media screen and (min-width: 1197px) and (max-width: 2000px) {
            #rcornersMsg {
                height: 295px !important;
            }
        }
    </style>
    <style type="text/css">
        #ContentPlaceHolder1_DistributorUC_btnGo, #ContentPlaceHolder1_DistributorUC_btnNewParty {
            height: 30px;
            padding: 1px 15px;
        }
    </style>
    <h1>Customer Dashboard<%--Dealer/Distributor--%>
        <%-- <small>Dashboard</small>--%>
    </h1>
    <ol class="breadcrumb" hidden>
        <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
        <li class="active">Dashboard</li>
    </ol>
</section>


<section class="content">
   
    <div class="row" id="dashboard" style="display: none;">
        <div class="col-lg-3 col-xs-6 bxwidth">
            <!-- small box -->
            <div class="small-box bg-aqua small-boxheight">
                <div class="inner">
                    <%-- <asp:Button ID="MaterialMaster" Style="margin-right: 5px;" runat="server"
                        Text="Pending Orders" OnClick="MaterialMaster_Click" />--%>
                </div>
            </div>
        </div>
        <!-- ./col -->
        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-red small-boxheight">
                <div class="inner">
                    <%--<asp:Button ID="Invoice" Style="margin-right: 5px;" runat="server"
                        Text="Invoice" OnClick="Invoice_Click" />--%>
                </div>
            </div>
        </div>
        <!-- ./col -->
        <div class="col-lg-3 col-xs-6" style="display: none;">
            <!-- small box -->
            <div class="small-box bg-yellow small-boxheight">
                <div class="inner">
                    <%--  <asp:Button ID="PurForMonth" Style="margin-right: 5px;" runat="server"
                        Text="Purchase For Month" OnClick="PurForMonth_Click" />--%>
                </div>

            </div>
        </div>
        <!-- ./col -->
        <div class="clearfix mo-clearfix"></div>

        <!-- ./col -->

        <div class="col-lg-3 col-xs-6 bxwidth">
            <!-- small box -->
            <div class="small-box bg-green small-boxheight">
                <div class="inner">
                    <%--<asp:Button ID="NewParty" Style="margin-right: 5px;" runat="server"
                        Text="New Parties" OnClick="NewParty_Click" />--%>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-6 bxwidth">
            <!-- small box -->
            <div class="small-box bg-yellow small-boxheight">
                <div class="inner">
                    <%-- <asp:Button ID="LedgerPos" Style="margin-right: 5px;" runat="server"
                        Text="Ledger Position" OnClick="LedgerPos_Click" />--%>
                </div>
            </div>
        </div>

        <div class="clearfix mo-clearfix"></div>
        <!-- Small boxes (Stat box) -->

    </div>

    <div class="box-body">
        <div class="row">
            <div class="col-lg-9 col-md-10 col-sm-12" style="margin-bottom: 5px;">
                <div id="rcorners2" class="circular-distributor" style="background-color: white; padding: 0px 0px 17px 0px;">
                    <div class="row" style="height: 90%;">
                        <%--<div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 circular-distributor-in text-center">                           
                            <div>
                                <div>
                                    <h3></h3>
                                    <asp:Button ID="MaterialMaster" runat="server" class="circle-cont1"
                                        Text="Purchases For Month " OnClick="MaterialMaster_Click" Visible="false" />
                                </div>
                            </div>
                        </div>--%>
                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 circular-distributor-in text-center">
                            <!-- small box -->
                            <div>
                                <div>
                                    <h3></h3>
                                    <%--  <input style="margin-right: 5px;" type="button" id="Primary " value="Primary Sales Month" class="circle-cont1" onclick="callDiv()" />--%>
                                    <asp:Button ID="PendingOrders" runat="server" class="circle-cont1"
                                        Text="Pending Orders" OnClick="PendingOrders_Click" />
                                </div>
                            </div>
                        </div>
                        <!-- ./col -->
                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 circular-distributor-in text-center">
                            <!-- small box -->
                            <div>
                                <div>
                                    <h3></h3>
                                    <%--<input style="margin-right: 5px;" type="button" id="Secondary" value="Secondary Sales For The Month" class="circle-cont1" 
                                        onclick="callSecDiv()" />--%>
                                    <asp:Button ID="Invoice" runat="server" class="circle-cont1"
                                        Text="Invoice" OnClick="Invoice_Click" />
                                </div>
                            </div>
                        </div>
                        <!-- ./col -->
                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 circular-distributor-in text-center">
                            <!-- small box -->
                            <div>
                                <div>
                                    <h3></h3>
                                    <asp:Button ID="NewParty" runat="server" class="circle-cont1"
                                        Text="Party Details" OnClick="NewParty_Click" />
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 circular-distributor-in text-center">
                            <!-- small box -->
                            <div>
                                <div>
                                    <h3></h3>

                                    <asp:Button ID="LedgerPos" runat="server" class="circle-cont1"
                                        Text="Ledger Position" OnClick="LedgerPos_Click" />
                                </div>
                            </div>
                        </div>
                          <%--  <div class="col-lg-3 col-xs-6 col-sm-4 col-md-3 circular-distributor-in text-center">
                            <!-- small box -->
                            <div>
                                <div>
                                    <h3></h3>

                                    <asp:Button ID="btnsecondaryorder" runat="server" class="circle-cont1"
                                        Text="Secondaryorder" OnClick="btnsecondaryorder_Click" />
                                </div>
                            </div>
                        </div>--%>
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-md-10 col-sm-12 right-links">
                <div id="rcorners12" style="background-color: white; height: 320px;">
                    <div class="row-fluid" style="text-align: center;">
                        <label for="quickLink"><b>Quick Links</b></label>
                    </div>
                    <div class="row-fluid text-center">
                        <asp:Button ID="OrderEntry" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="Purchase Order Entry" OnClick="OrderEntry_Click" />
                    </div>

                    <div class="row-fluid text-center">
                        <asp:Button ID="DistComplaint" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="Complaint" OnClick="DistComplaint_Click" />
                    </div>
                    <div class="row-fluid text-center">
                        <asp:Button ID="DistSuggestion" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="Suggestion" OnClick="DistSuggestion_Click" />
                    </div>
                      <div class="row-fluid text-center">
                        <asp:Button ID="Diststock" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="Stock" OnClick="Diststock_Click"/>
                    </div>
                        <div class="row-fluid text-center">
                        <asp:Button ID="btnsecondaryorder" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="Secondary Order" OnClick="btnsecondaryorder_Click"/>
                    </div>
                    <div class="row-fluid text-center">
                        <asp:Button ID="DownloadsDist" Style="margin-right: 5px;" runat="server" CssClass="quicklink-cont12"
                            Text="Downloads" OnClick="DownloadsDist_Click" />
                    </div>
                </div>
            </div>
        </div>
        <!-- Small boxes (Stat box) -->
        <div class="row">
            <div class="col-lg-9 col-md-10 col-sm-12" style="margin-top: 5px;">
                <div id="reportcontainer" class="box-body" style="display: none; padding: 0;" runat="server">

                    <div class="rcorners123" id="MaterialMasterJQX" style="display: none; padding: 5px; background-color: white;" runat="server">
                        <div class="box-header">
                            <h3 class="box-title">Purchases For Month:</h3>
                            <asp:DropDownList ID="monthDDL" runat="server"></asp:DropDownList>&nbsp;&nbsp;<asp:DropDownList ID="yearDDL" runat="server">
                            </asp:DropDownList>&nbsp;&nbsp;
                            <asp:Button type="button" ID="btnGoPurch" runat="server" Text="Go" class="btn btn-primary" OnClick="btnGoPurch_Click" />
                        </div>
                        <div>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="matmasterRep" runat="server">
                                    <HeaderTemplate>
                                        <table id="mattable" class="table table-bordered table-striped rpttable">
                                            <thead>
                                                <tr>
                                                    <th style="text-align: center; width: 6%" hidden>Year</th>
                                                    <th style="text-align: center; width: 6%" hidden>Month</th>
                                                    <th style="text-align: center; width: 6%" hidden>S.No</th>
                                                    <th style="text-align: left;">Material Group</th>
                                                    <th style="text-align: right; width: 22%;">Amount</th>
                                                    <th style="text-align: center; visibility: hidden">Details</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="text-align: center; width: 6%" hidden><%#Eval("Year")%></td>
                                            <td style="text-align: center; width: 6%" hidden><%#Eval("Month")%></td>
                                            <td style="text-align: center; width: 6%" hidden><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>
                                            <td style="text-align: left;"><%#Eval("MaterialGroup") %></td>
                                            <td style="text-align: right; width: 22%"><%#Eval("Amt") %></td>
                                            <td style="text-align: center; width: 10%">
                                                <a href="<%# String.Format("/DetailMatWisePurchase.aspx?"+"DistId="+Eval("DistId"))+"&UnderId="+Eval("MatGrpId")+""+"&Year="+Eval("Year")+""+"&Month="+Eval("Month")+"" %>">Details</a></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                    <div class="rcorners123" id="PendingJQX" style="display: none; padding: 5px; background-color: white;" runat="server">
                        <div class="box-header">
                            <h3 class="box-title">Pending Orders:</h3>
                            <div class="col-md-12 paddingleft0">
                                <div id="DIV1" class="form-group col-md-2 col-sm-3" style="padding: 2px;">
                                    <asp:HiddenField ID="hfCustomerId1" runat="server" />
                                    <label for="exampleInputEmail1">From Date:</label>
                                    <asp:TextBox ID="txtmDate" runat="server" CssClass="form-control" BackColor="White" Width="120px"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtmDate" runat="server" />
                                </div>
                                <div class="form-group col-md-2 col-sm-3" style="padding: 2px 13px;">
                                    <label for="exampleInputEmail1">To Date:</label>
                                    <asp:TextBox ID="txttodate" runat="server" CssClass="form-control" BackColor="White" Width="120px"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                </div>
                                <div class="form-group col-md-2">
                                    <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                    <asp:Button type="button" ID="Button1" runat="server" Text="Go" Style="padding: 4px 7px;" class="btn btn-primary" OnClick="btnGo_Click" />
                                </div>
                                <div class="form-group col-md-6 ">
                                </div>
                            </div>
                        </div>
                        <div>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                                    <HeaderTemplate>
                                        <table id="example1" class="table table-bordered table-striped porderrpttable">
                                            <thead>
                                                <tr>
                                                    <th >Order No</th>
                                                    <th>Order Date</th>
                                                    <th>Portal No</th>
                                                    <th>Branch Name</th>
                                                    <th style="text-align: right">Net Amount</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("PODocId") %>' />
                                            <asp:HiddenField ID="HiddenField2" runat="server" Value='<%#Eval("PODocId") %>' />
                                            <td>
                                                <asp:HyperLink runat="server" Target="_blank" Font-Underline="true"
                                                    NavigateUrl='<%# String.Format("POSapDetails.aspx?PODocId={0}", Eval("PODocId")) %>'
                                                    Text='<%#Eval("PODocId") %>' />
                                            </td>
                                            <td><%#Convert.ToDateTime(Eval("VDate")).ToString("dd/MMM/yyyy")%></td>
                                            <td><%#Eval("PortalNo") %></td>
                                            <td><%#Eval("ResCenName") %></td>
                                            <td style="text-align: right"><%#Eval("ItemwiseTotal") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>

                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                    <div class="rcorners123" id="InvoiceJQX" style="display: none; padding: 5px; background-color: white;" runat="server">
                        <div class="box-header">
                            <h3 class="box-title">Invoice:</h3>
                            <div class="col-md-12 paddingleft0">
                                <div id="DIV1" class="form-group col-md-2 col-sm-3" style="padding: 2px;">
                                    <asp:HiddenField ID="HiddenField3" runat="server" />
                                    <label for="exampleInputEmail1">From Date:</label>
                                    <asp:TextBox ID="txtmDate1" runat="server" CssClass="form-control" BackColor="White" Width="120px"></asp:TextBox>&nbsp;&nbsp;
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtmDate1" runat="server" />
                                </div>
                                <div class="form-group col-md-2 col-sm-3" style="padding: 2px 13px;">
                                    <label for="exampleInputEmail1">To Date:</label>
                                    <asp:TextBox ID="txttodate1" runat="server" CssClass="form-control" BackColor="White" Width="120px"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate1" runat="server" />
                                </div>
                                <div class="form-group col-md-2">
                                    <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                    <asp:Button type="button" ID="Button2" runat="server" Text="Go" Style="padding: 4px 7px;" class="btn btn-primary" OnClick="Button2_Click" />
                                </div>
                            </div>
                        </div>
                        <div>


                            <div class="box-body table-responsive">
                                <asp:Repeater ID="invrpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="invtable" class="table table-bordered table-striped invrpttable">
                                            <thead>
                                                <tr>
                                                    <th style="text-align: left; width: 20%; text-wrap: normal;">Invoice No</th>
                                                    <th style="text-align: left; width: 18%">Invoice Date</th>
                                                    <th style="text-align: left; width: 18%">Branch Name</th>
                                                    <th style="text-align: right; width: 28%">Amount</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <%--  <td style="text-align: center; width: 20%"><%#Eval("DistInvDocId")%></td>--%>
                                            <td>
                                                <asp:HyperLink runat="server" Target="_blank" Font-Underline="true"
                                                    NavigateUrl='<%# String.Format("InvoiceSapDetails.aspx?DistInvDocId={0}", Eval("DistInvDocId")) %>'
                                                    Text='<%#Eval("DistInvDocId") %>' />
                                            <td style="text-align: left; width: 18%"><%#Convert.ToDateTime(Eval("VDate")).ToString("dd/MMM/yyyy") %></td>
                                            <td style="text-align: left; width: 18%"><%#Eval("BranchName") %></td>
                                            <td style="text-align: right; width: 28%"><%#Eval("Amount") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                    <div class="rcorners123" id="LedgerPosJQX" style="display: none; padding: 5px; background-color: white;" runat="server">
                        <div class="box-header">
                            <h3 class="box-title">View Ledger:</h3>
                        </div>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rptLedgerRep" runat="server">
                                <HeaderTemplate>
                                    <table id="dsrTable" class="table table-bordered table-striped rpttable">
                                        <thead>
                                            <tr>
                                                <th style="text-align: center; width: 6%">S.No</th>
                                                <th style="text-align: right;">Ledger Balance</th>
                                                <th style="text-align: right;">Credit Limit</th>
                                                <th style="text-align: right;">Credit Days</th>
                                                <th style="text-align: center;">View Ledger</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("DistId") %>' />
                                        <td style="text-align: center;"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>
                                        <%--<td style="text-align: right;"><%#Eval("Balance") %></td>--%>
                                        <td style="text-align: right;"><%#Convert.ToDecimal(Eval("Balance")) > 0 ? Eval("Balance")+ " Dr"  : (-1)* Convert.ToDecimal(Eval("Balance")) + " Cr" %></td>
                                        <td style="text-align: right;"><%#Eval("CreditLimit") %></td>
                                        <td style="text-align: right;"><%#Eval("CreditDays") %></td>
                                        <td style="text-align: center;">
                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">View Ledger</asp:LinkButton></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                        <hr style="height: 3px" />
                        <div id="detailDiv" runat="server" style="display: none;">
                            <div class="box-header">
                                <h3 class="box-title">Ledger Details:</h3>
                                <asp:DropDownList ID="monthDropDownList" runat="server"></asp:DropDownList>&nbsp;&nbsp;<asp:DropDownList ID="yearDropDownList" runat="server">
                                </asp:DropDownList>&nbsp;&nbsp;<asp:Button type="button" ID="GetLedgerDeatil" runat="server" Text="Go" class="btn btn-primary"
                                    OnClick="GetLedgerDeatil_Click" />
                            </div>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="detailDistrpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="example1" class="table table-bordered table-striped rpttable">
                                            <thead>
                                                <tr>
                                                    <th>SNo.</th>
                                                    <th>Date</th>
                                                    <th>VNo.</th>
                                                    <th>Narration</th>
                                                    <th>Debit</th>
                                                    <th>Credit</th>
                                                    <th>Balance</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>
                                            <td><%#Convert.ToDateTime(Eval("vdate")).ToString("dd/MMM/yyyy") %></td>
                                            <td><%#Eval("DLDocId") %></td>
                                            <td><%#Eval("Narration") %></td>
                                            <td><%#Eval("amtDr") %></td>
                                            <td><%#Eval("amtCr") %></td>
                                            <td style="text-align: right;"><%#Eval("Balance") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                    <div class="rcorners123" id="NewPartyJQX" style="display: none; padding: 5px; background-color: white;" runat="server">
                        <div class="box-header">
                            <h3 class="box-title">Party List:</h3>
                            <asp:DropDownList ID="ddltypeFilter" runat="server" OnSelectedIndexChanged="ddltypeFilter_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                <asp:ListItem Text="Old" Value="Old"></asp:ListItem>
                                <asp:ListItem Text="All" Value="All"></asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;&nbsp;<asp:Button type="button" ID="btnNewParty" runat="server" Text="Go" Visible="false" class="btn btn-primary" OnClick="btnNewParty_Click" />
                        </div>
                        <div>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="newpartyrpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="newpartyTable" class="table table-bordered table-striped rpttable">
                                            <thead>
                                                <tr>
                                                    <th style="text-align: left; width: 16%">Party</th>
                                                    <th style="text-align: left; width: 16%">ContactPerson</th>
                                                    <th style="text-align: left; width: 16%">City</th>
                                                    <th style="text-align: left; width: 16%">Area</th>
                                                    <th style="text-align: left; width: 16%">Beat</th>
                                                    <th style="text-align: center; width: 18%">Address</th>
                                                    <th style="text-align: center;">Contact No.</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="text-align: left; width: 16%"><%#Eval("PartyName")%></td>
                                            <td style="text-align: left; width: 16%"><%#Eval("ContactPerson")%></td>
                                            <td style="text-align: left; width: 16%"><%#Eval("City")%></td>
                                            <td style="text-align: left; width: 16%"><%#Eval("Area")%></td>
                                            <td style="text-align: left; width: 16%"><%#Eval("Beat")%></td>
                                            <td style="text-align: center;"><%#Eval("Address") %></td>
                                            <td style="text-align: center; width: 18%"><%#Eval("Mobile") %></td>
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
            <div class="col-lg-3 col-md-10 col-sm-12" style="margin-top: 5px;">
                <div class="right-links">
                    <div id="rcornersMsg" style="background-color: white;">
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
