<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DetailMatWisePurchase.aspx.cs" Inherits="AstralFFMS.DetailMatWisePurchase" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(function () {
             $("#detMattable").DataTable();
         });
    </script>
    <section class="content">
        <div class="box-body">
            <div class="box box-primary">
                <div class="box-header with-border">
                    <h3 class="box-title">Detail Material Wise Purchase</h3>
                    <div style="float: right">
                        <asp:Button type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                    </div>
                </div>
                <div class="box-body table-responsive">
                    <asp:Repeater ID="rptDetPurchase" runat="server">
                        <HeaderTemplate>
                            <table id="detMattable" class="table table-bordered table-striped rpttable">
                                <thead>
                                    <tr>
                                        <th style="text-align: center;">S.No</th>
                                        <th style="text-align: center;">Product</th>
                                        <th style="text-align: center;">Quantity</th>
                                        <th style="text-align: center;">Amount</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="text-align: center; width: 6%"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>
                                <td style="text-align: center;"><%#Eval("Item") %></td>
                                <td style="text-align: right;"><%#Eval("Qty") %></td>
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
    </section>
</asp:Content>
