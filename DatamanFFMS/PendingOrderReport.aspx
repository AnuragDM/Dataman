<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="PendingOrderReport.aspx.cs" Inherits="AstralFFMS.PendingOrderReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
  <%--  <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
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
        $(function () {
            $("#pendingOrderTable").DataTable();
        });
    </script>
    <script type="text/javascript">
        //$(function () {
        //    $(".select2").select2();
        //});
    </script>
    <style type="text/css">
        .input-group .form-control {
            height: 34px;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            display: table;
        }

        .multiselect-container > li > a {
            white-space: normal;
        }

        .multiselect-container > li {
            width: 212px;
        }
    </style>
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
                    <!-- left column -->
                    <div class="col-md-12">
                        <div id="InputWork">
                            <!-- general form elements -->
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Pending Order Report</h3>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="col-md-8">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Distributor:</label>
                                                    <%--<asp:DropDownList ID="DdlSalesPerson" CssClass="form-control select2" runat="server"></asp:DropDownList>--%>
                                                    <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-7">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Material Group</label>
                                                    <asp:DropDownList ID="ddlMatGrp" Width="100%" OnSelectedIndexChanged="ddlMatGrp_SelectedIndexChanged" AutoPostBack="true"
                                                        CssClass="form-control" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-7">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Product</label>
                                                    <asp:DropDownList ID="ddlProduct" Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box-footer">
                                    <asp:Button type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary"
                                        OnClick="btnGo_Click" />
                                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Cancel_Click" />
                                    <%-- <input style="margin-right: 5px;" type="button" id="Go" value="Go" class="btn btn-primary" onc onclick="GetReport();" />--%>
                                </div>
                            </div>
                        </div>
                        <div id="rptmain" runat="server" style="display: none;">
                            <div class="box-header table-responsive">
                                <asp:Repeater ID="pendingorderrpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="pendingOrderTable" class="table table-bordered table-striped rpttable">
                                            <thead>
                                                <tr>
                                                    <th style="text-align: left; width: 18%">Distributor</th>
                                                    <th style="text-align: left; width: 18%">Date</th>
                                                    <th style="text-align: left; width: 18%">Order No</th>
                                                    <th style="text-align: left; width: 18%">Product Id</th>
                                                    <th style="text-align: left; width: 18%">Product</th>
                                                    <th style="text-align: center; width: 18%">Unit</th>
                                                    <th style="text-align: center; width: 7%">Rate</th>
                                                    <th style="text-align: center;">Qty</th>
                                                    <th style="text-align: center; width: 13%">Supp.Qty</th>
                                                    <th style="text-align: center; width: 18%">Pending Qty</th>
                                                    <th style="text-align: center; width: 18%">Pending Amount</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="text-align: left; width: 18%"><%#Eval("DistributorName") %></td>
                                            <td style="text-align: left; width: 18%"><%#Eval("OrderDate","{0:dd/MMM/yyyy}") %></td>
                                            <td style="text-align: left; width: 18%"><%#Eval("OrderNo") %></td>
                                            <td style="text-align: left; width: 18%"><%#Eval("ItemId") %></td>
                                            <th style="text-align: left; width: 18%"><%#Eval("ItemName") %></th>
                                            <td style="text-align: right; width: 18%"><%#Eval("Unit") %></td>
                                            <td style="text-align: right; width: 18%"><%#Eval("Rate") %></td>
                                            <td style="text-align: right; width: 7%"><%#Eval("Qty") %></td>
                                            <td style="text-align: right; width: 30%"><%#Eval("SuppliedQty") %></td>
                                            <td style="text-align: right; width: 13%"><%#Eval("PendingQty") %></td>
                                            <td style="text-align: right; width: 13%"><%#Eval("PendingAmt") %></td>
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
        </div>
    </section>
</asp:Content>
