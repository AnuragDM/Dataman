<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/FFMS.Master" CodeBehind="TopProductSegmentDetails.aspx.cs" Inherits="AstralFFMS.TopProductSegmentDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

         <div class="box-body">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Product Segment Wise Sales Details</h3>
                           
                            </div>
                          <div class="clearfix"></div>
                                    <div class="row">
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div id="DIV1" class="form-group" style="margin-left: 8%;">
                                                <label for="exampleInputEmail1">From Date:</label>
                                                <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" onChange="showspinner();" AutoPostBack="true"  Style="background-color: white;" OnTextChanged="txtfmDate_TextChanged"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-md-1"></div>
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">To:</label>
                                                <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" onChange="showspinner();" AutoPostBack="true"  Style="background-color: white;" OnTextChanged="txttodate_TextChanged"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                            </div>
                                        </div>

                                    </div>
                           <div class="col-md-6 col-sm-6 col-xs-12">
                                
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Report For Product Segment:</label>                                                
                                            <asp:Label ID="lblprodct" runat="server" Text='<%# Eval("ItemName")%>'></asp:Label>
                                        </div>
                                        <%-- </div>--%>
                                
                                     <div class="row">
                                   
                                        <div class="col-md-5 col-sm-12 col-xs-12">
                                            <div id="DIV1" class="form-group">
                                                <label for="exampleInputEmail1">From Date:</label>
                                                <asp:Label ID="lblfmDate" runat="server" Text=""></asp:Label>
                                            
                                            </div>
                                        </div>
                                        
                                        <div class="col-md-5 col-sm-12 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">To Date:</label>
                                                 <asp:Label ID="lbltodate" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                       
                                 
                               </div>
                               </div>

                           
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                <div class="col-md-3 col-sm-5">
                                    <div class="form-group">
                                        
                                    </div>
                                </div></div>
                            </div>
                          
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="box-body" id="rptmain" runat="server" style="display: block;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <div style="float: right">
                          

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">

                         <asp:Repeater ID="Topsaleproduct" runat="server">
                                <HeaderTemplate>
                                    <div class="box-footer">
                                    
                                    <asp:Button  type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="btnExport"/>
                                  </div>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>

                                           <tr>  
                                                   <%-- <th style="text-align: left; width: 5%">S.No</th> --%>
                                                   <%-- <th style="text-align: left; width: 14%">PartyName</th>
                                                    <th style="text-align: right; width: 10%">Quantity</th>
                                                   <th style="text-align: right; width: 8%">Rate</th>
                                                    <th style="text-align: right; width: 8%">Amount</th>--%>
                                                    <th style="text-align: left; width: 14%">Date</th>
                                                    <th style="text-align: left; width: 14%">Sales Person Name</th>
                                                    <th style="text-align: left; width: 14%">Retailer Name</th>
                                                    <th style="text-align: left; width: 14%">Item Name</th>
                                                    <th style="text-align: right; width: 8%">Rate</th>
                                                    <th style="text-align: right; width: 10%">Quantity</th>
                                                    <th style="text-align: right; width: 8%">Amount</th>
                                                   
                                                </tr>
                                            </thead>
                                             <tfoot>
                                                <tr>
                                                    <th colspan="5" style="text-align: right">Grand Total:</th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th> 
                                                                                                       
                                                </tr>

                                            </tfoot>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                          
                                            <tr> 
                                           
                                             
                                          <%--  <td><%# Container.ItemIndex + 1 %></td>        --%>                    
                                            <%--<td><%# Eval("PartyName")%>
                                            <asp:Label ID="lblProductparty" runat="server" Visible="false" Text='<%# Eval("PartyName")%>'></asp:Label></td>
                                            <td style="text-align: right"><%# Eval("qty")%>
                                            <asp:Label ID="lblqty" runat="server" Visible="false" Text='<%# Eval("qty")%>'></asp:Label></td>                                 <td style="text-align: right"><%# Eval("rate")%>
                                            <asp:Label ID="lblrate" runat="server" Visible="false" Text='<%# Eval("rate")%>'></asp:Label></td>                
                                            <td style="text-align: right"><%# Eval("amount")%>
                                            <asp:Label ID="lblamount" runat="server" Visible="false" Text='<%# Eval("amount")%>'></asp:Label></td>  --%>
                                            <td><%#Convert.ToDateTime(Eval("VDate")).ToString("dd/MMM/yyyy") %>
                                            <asp:Label ID="lblvdate" runat="server" Visible="false" Text='<%#Convert.ToDateTime(Eval("VDate")).ToString("dd/MMM/yyyy") %>'></asp:Label></td>
                                            <td><%# Eval("SMName")%>
                                            <asp:Label ID="lblsmname" runat="server" Visible="false" Text='<%# Eval("SMName")%>'></asp:Label></td>               
                                            <td><%# Eval("PartyName")%>
                                            <asp:Label ID="lblpartyname" runat="server" Visible="false" Text='<%# Eval("PartyName")%>'></asp:Label></td>
                                            <td><%# Eval("ItemName")%>
                                            <asp:Label ID="lblitemname" runat="server" Visible="false" Text='<%# Eval("ItemName")%>'></asp:Label></td>
                                            <td style="text-align: right"><%# Eval("rate")%>
                                            <asp:Label ID="lblrate" runat="server" Visible="false" Text='<%# Eval("rate")%>'></asp:Label></td>
                                            <td style="text-align: right"><%# Eval("qty")%>
                                            <asp:Label ID="lblqty" runat="server" Visible="false" Text='<%# Eval("qty")%>'></asp:Label></td>                 
                                            <td style="text-align: right"><%# Eval("amount")%>
                                            <asp:Label ID="lblamount" runat="server" Visible="false" Text='<%# Eval("amount")%>'></asp:Label></td>
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
     <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#example1').dataTable({
                "order": [[1, "desc"]],
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
                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Quantity'; }).first().index();
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

                    $(api.column(5).footer()).html(searchTotal);
                    // $(api.column(3).footer()).html(searchTotal1);
                    $(api.column(6).footer()).html(searchTotal1);

                    if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(5).footer()).html('0.0') }
                    //if (searchTotal1 == 'NaN' || searchTotal1 == '') { $(api.column(3).footer()).html('0.0') }
                    if (searchTotal1 == 'NaN' || searchTotal1 == '') { $(api.column(6).footer()).html('0.0') }
                    //if (searchTotal2 == 'NaN' || searchTotal2 == '') { $(api.column(9).footer()).html('0.0') }
                }
            });
        });
    </script>
    <script type="text/javascript">
        function showspinner() {

            $("#spinner").show();

        };
        function hidespinner() {

            $("#spinner").hide();

        };

     </script> 
</asp:Content>
