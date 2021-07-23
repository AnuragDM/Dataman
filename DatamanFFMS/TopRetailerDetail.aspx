<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="TopRetailerDetail.aspx.cs" EnableEventValidation="false" Inherits="AstralFFMS.TopRetailerDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        #example1_wrapper .row {
            margin-right: 0px !important;
            margin-left: 0px !important;
        }

            #example1_wrapper .row .col-sm-12 {
                overflow-x: scroll !important;
                padding-left: 0px !important;
                margin-bottom: 10px;
            }

            input[type=checkbox], input[type=radio] {
    margin: 0px 8px 0;
    /*margin-top: 1px \9;
    line-height: normal;*/
}
    </style>
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

        <div class="box-body" style="padding-bottom: 0px;">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary" style="margin-bottom: 0px">
                            <div class="box-header with-border">
                                <h3 class="box-title">Top Retailer Details</h3>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Retailer Name :</label>&nbsp;&nbsp;
                                          <b>  
                                            <asp:Label ID="lbldist" runat="server" Text='<%# Eval("PartyName")%>'></asp:Label></b>
                                        </div>
                                        <%-- </div>--%>
                                    </div>
                                </div>
                                <div class="row">

                                    <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">From Date :</label>&nbsp;&nbsp;
                                            <asp:Label ID="lblfmDate" runat="server" Text=""></asp:Label>

                                        </div>
                                    </div>

                                    <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date :</label>&nbsp;&nbsp;
                                            <asp:Label ID="lbltodate" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkItemWise" runat="server" Checked="true" AutoPostBack="true" Text="Item wise retailer report" OnCheckedChanged="chkItemWise_CheckedChanged" />
                                        </div>

                                    </div>
                                    <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkDatewise" runat="server" AutoPostBack="true" Text="Date wise retailer report" OnCheckedChanged="chkDatewise_CheckedChanged" />
                                        </div>

                                    </div>
                                </div>
                              <%--  <div class="row">
                                    
                                </div>--%>
                               <%-- <div class="row">
                                    <div class="col-md-5 col-sm-12 col-xs-12">
                                        <div class="form-group">--%>
                                            <%--<asp:Button Style="margin-right: 5px; margin-bottom:20px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="btnExport"/>--%>
                                            <%--<asp:Button Style="margin-right: 5px; margin-bottom:20px;" type="button" ID="Button1" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="Button1"/>                                           --%>
                                        <%--</div>

                                    </div>
                                </div>--%>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                    <div class="col-md-3 col-sm-5">
                                        <div class="form-group">
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="box-body" id="rptmain" runat="server" style="display: block;padding-top: 0px;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box" style="border-top: 1px solid #f4f4f4;">
                        <div class="box-header">
                            <div style="float: right">
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div id="divrepeater" runat="server" class="box-body table-responsive">

                            <asp:Repeater ID="Topsaleproduct" runat="server">
                                <HeaderTemplate>
                                    <asp:Button Style="margin-right: 5px; margin-bottom: 20px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="btnExport" />
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>

                                            <tr>
                                                <th style="text-align: left; width: 10%">S.No</th>
                                                <th style="text-align: left; width: 10%">Product</th>
                                                <th style="text-align: right; width: 10%">Qty</th>
                                                <th style="text-align: right; width: 10%">Avg.Rate</th>
                                                <th style="text-align: right; width: 8%">Amount</th>

                                            </tr>
                                        </thead>
                                        <tfoot>
                                            <tr>
                                                <th colspan="2" style="text-align: right">Grand Total:</th>
                                                <th style="text-align: right"></th>
                                                <th style="text-align: right"></th>
                                                <th style="text-align: right"></th>
                                            </tr>

                                        </tfoot>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>

                                    <tr>


                                        <td><%# Container.ItemIndex + 1 %></td>
                                        <td><%# Eval("Product")%>
                                            <asp:Label ID="lblProduct" runat="server" Visible="false" Text='<%# Eval("Product")%>'></asp:Label></td>
                                        <td style="text-align: right"><%# Eval("Qty")%>
                                            <asp:Label ID="lblqty" runat="server" Visible="false" Text='<%# Eval("Qty")%>'></asp:Label></td>
                                        <td style="text-align: right"><%# String.Format("{0:#.00}", Eval("rate"))%>
                                            <asp:Label ID="lblrate" runat="server" Visible="false" Text='<%# String.Format("{0:#.00}", Eval("rate"))%>'></asp:Label></td>
                                        <td style="text-align: right"><%# Eval("Amount")%>
                                            <asp:Label ID="lblamount" runat="server" Visible="false" Text='<%# Eval("Amount")%>'></asp:Label></td>

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


                        <div id="divrepeater1" runat="server" class="form-group" style="display: none;">
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                    <HeaderTemplate>
                                        <asp:Button Style="margin-right: 5px; margin-bottom: 20px;" type="button" ID="Button1" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="Button1" />
                                        <table id="example2" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <th style="text-align: left; width: 10%">S.No</th>
                                                    <th style="text-align: left; width: 10%">Product</th>
                                                    <th style="text-align: right; width: 10%">Qty</th>
                                                    <th style="text-align: right; width: 10%">Rate</th>
                                                    <th style="text-align: right; width: 8%">Amount</th>
                                                    <th style="text-align: right; width: 8%">Order Date</th>
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                    <th colspan="2" style="text-align: right">Grand Total:</th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                </tr>

                                            </tfoot>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Container.ItemIndex + 1 %></td>
                                            <td><%# Eval("Product")%>
                                                <asp:Label ID="lblProduct" runat="server" Visible="false" Text='<%# Eval("Product")%>'></asp:Label></td>

                                            <td style="text-align: right"><%# Eval("Qty")%>
                                                <asp:Label ID="lblqty" runat="server" Visible="false" Text='<%# Eval("Qty")%>'></asp:Label></td>
                                            <td style="text-align: right"><%# Eval("rate")%>
                                                <asp:Label ID="lblrate" runat="server" Visible="false" Text='<%# Eval("rate")%>'></asp:Label></td>
                                            <td style="text-align: right"><%# Eval("Amount")%>
                                                <asp:Label ID="lblamount" runat="server" Visible="false" Text='<%# Eval("Amount")%>'></asp:Label></td>
                                            <td style="text-align: right"><%#String.Format("{0:dd/MMM/yyyy}", Eval("VDate"))%>
                                                <asp:Label ID="lblvdate" runat="server" Visible="false" Text='<%# System.Convert.ToDateTime(Eval("VDate")).ToString("dd/MMM/yyyy")%>'></asp:Label></td>


                                        </tr>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>

                                </asp:Repeater>
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
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#example1').dataTable({
                "order": [[4, "desc"]],
                "footerCallback": function (tfoot, data, start, end, display) {
                    //$(tfoot).find('th').eq(0).html("Starting index is " + start);
                    var api = this.api();
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    debugger;
                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Qty'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);


                    var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Amount'; }).first().index();
                    var totalData1 = api.column(costColumnIndex1).data();
                    var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                    var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                    var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(2).footer()).html(searchTotal);
                    // $(api.column(3).footer()).html(searchTotal1);
                    $(api.column(4).footer()).html(searchTotal1);

                    if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(2).footer()).html('0.0') }
                    //if (searchTotal1 == 'NaN' || searchTotal1 == '') { $(api.column(3).footer()).html('0.0') }
                    if (searchTotal1 == 'NaN' || searchTotal1 == '') { $(api.column(4).footer()).html('0.0') }
                    //if (searchTotal2 == 'NaN' || searchTotal2 == '') { $(api.column(9).footer()).html('0.0') }
                }
            });
        });
    </script>

    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#example2').dataTable({
                "order": [[5, "desc"]],
                "footerCallback": function (tfoot, data, start, end, display) {
                    //$(tfoot).find('th').eq(0).html("Starting index is " + start);
                    var api = this.api();
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    debugger;
                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Qty'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);


                    var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Amount'; }).first().index();
                    var totalData1 = api.column(costColumnIndex1).data();
                    var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                    var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                    var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(2).footer()).html(searchTotal);
                    // $(api.column(3).footer()).html(searchTotal1);
                    $(api.column(4).footer()).html(searchTotal1);

                    if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(2).footer()).html('0.0') }
                    //if (searchTotal1 == 'NaN' || searchTotal1 == '') { $(api.column(3).footer()).html('0.0') }
                    if (searchTotal1 == 'NaN' || searchTotal1 == '') { $(api.column(4).footer()).html('0.0') }
                    //if (searchTotal2 == 'NaN' || searchTotal2 == '') { $(api.column(9).footer()).html('0.0') }
                }
            });
        });
    </script>
    <script runat="server"> 
        public override void VerifyRenderingInServerForm(Control control)
        {
        }
    </script>
</asp:Content>
