<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="PriceListMaster.aspx.cs" Inherits="AstralFFMS.PriceListMaster" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
      <script type="text/javascript">
          //$(function () {
          //    $(".select2").select2();
          //});
    </script>
    <script type="text/javascript">
        $(function () {
            $("#pricetable").DataTable();
        });
    </script>
    <style type="text/css">
         .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            display: table;
        }
    </style>
    <section class="content">
        <div class="box-body">
            <!-- left column -->
            <!-- general form elements -->
            <div class="box box-primary">
                <div class="box-header with-border">
                    <h3 class="box-title">Price List</h3>
                </div>
                <div class="row">
                     <div class="col-md-3 col-sm-7" style="padding:0 30px;">
                            <div class="form-group">
                                <label for="exampleInputEmail1">Date:</label>
                                <asp:DropDownList ID="ddlDate" runat="server" Width="100%" class="form-control" TabIndex="3"
                                    OnSelectedIndexChanged="ddlDate_SelectedIndexChanged" DataTextFormatString="{0:dd/MMM/yyyy}"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </div>
                </div>
                <div id="rptmain" runat="server" style="display: none;">
                    <div class="box-header table-responsive">
                        <asp:Repeater ID="pricelistrpt" runat="server">
                            <HeaderTemplate>
                                <table id="pricetable" class="table table-bordered table-striped rpttable">
                                    <thead>
                                        <tr>
                                            <th style="text-align: center; width: 6%">SNo.</th>
                                            <th style="text-align: center; width: 12%">Wef Date</th>
                                            <th style="text-align: center; width: 10%">Product</th>
                                            <th style="text-align: center; width: 10%">MRP</th>
                                            <th style="text-align: center; width: 10%">DP</th>
                                            <th style="text-align: center; width: 10%">RP</th>
                                         <%--   <th style="text-align: center; width: 10%">Created Date</th>--%>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                     <td style="text-align: center; width: 6%"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>
                                    <td style="text-align: right; width: 12%"><%#Eval("WefDate","{0:dd/MMM/yyyy}") %></td>
                                    <td style="text-align: left; width: 10%"><%#Eval("ItemName") %></td>
                                    <td style="text-align: right; width: 10%"><%#Eval("MRP") %></td>
                                    <td style="text-align: right; width: 10%"><%#Eval("DP") %></td>
                                    <td style="text-align: right; width: 10%"><%#Eval("RP") %></td>
                                    <%--<td style="text-align: center; width: 10%"><%#Eval("CreatedDate","{0:dd/MMM/yyyy}") %></td>--%>
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
</asp:Content>
