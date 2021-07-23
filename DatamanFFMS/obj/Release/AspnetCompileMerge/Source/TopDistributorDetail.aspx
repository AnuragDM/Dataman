<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="TopDistributorDetail.aspx.cs" EnableEventValidation="false" Inherits="AstralFFMS.TopDistributorDetail" %>

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
                                <h3 class="box-title">Top Distributor Details</h3>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Distributor Name:</label>                                                
                                            <asp:Label ID="lbldist" runat="server" Text='<%# Eval("PartyName")%>'></asp:Label>
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

                         <asp:Repeater ID="TopDistributor" runat="server">
                                    <HeaderTemplate>
                                        <asp:Button Style="margin-right: 5px; margin-bottom:20px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="btnExport"/>
                                        <table id="itemsaleTable" class="table table-bordered table-striped">
                                            <thead>
                                                <tr> 
                                                    <th style="text-align: left; width: 10%">S.No</th>   
                                                    <th style="text-align: left; width: 17%">Product</th>
                                                    <%--<th style="text-align: left; width: 12%">ItemId</th>--%>
                                                    <th style="text-align: right; width: 8%">Qty</th>
                                                    <th style="text-align: right; width: 8%">Rate</th>
                                                    <th style="text-align: right; width: 10%">Amount</th>
                                                   
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
                                           
                                    </HeaderTemplate>
                                    <ItemTemplate>                                          
                                          <tr>  
                                            <td><%# Container.ItemIndex + 1 %></td>                                         
                                            <td><%# Eval("Product")%>
                                                 <asp:Label ID="lblProduct" runat="server" Visible="false" Text='<%# Eval("Product")%>'></asp:Label></td>                                            
                                            <%--<td><%# Eval("ItemId")%>
                                                <asp:Label ID="lblItemId" runat="server" Visible="false" Text='<%# Eval("ItemId")%>'></asp:Label></td>--%>   
                                            <td style="text-align: right"><%# Eval("Qty")%>
                                                <asp:Label ID="lblQty" runat="server" Visible="false" Text='<%# Eval("Qty")%>'></asp:Label></td>  
                                              <td style="text-align: right"><%# Eval("rate")%>
                                                <asp:Label ID="lblrate" runat="server" Visible="false" Text='<%# Eval("rate")%>'></asp:Label></td>   
                                            <td style="text-align: right"><%# Eval("Amount")%>
                                                <asp:Label ID="lblAmount" runat="server" Visible="false" Text='<%# Eval("Amount")%>'></asp:Label></td>
                                           
                                          </tr>                                            
                                     
                                    </ItemTemplate>
                                    <FooterTemplate>
                                     </tbody>       </table>       
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
            $('#itemsaleTable').dataTable({
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
</asp:Content>
