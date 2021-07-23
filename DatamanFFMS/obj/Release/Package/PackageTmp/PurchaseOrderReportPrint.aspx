<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderReportPrint.aspx.cs" Inherits="AstralFFMS.PurchaseOrderReportPrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="Content/Styles.css">
    <link rel="stylesheet" type="text/css" href="Content/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="Content/bootstrap.css">
    <style>
        #to-print-table table tr td {
            font-size: 12px;
        }

        #to-print-table table tr th {
            font-size: 12px;
            padding: 1px 1px;
            text-align: center;
        }

        table.table-indo tr td {
            text-align: left;
            padding: 5px;
        }

       
        .alignright{
        text-align:right;
        }

        .qty.text-right {text-align:right!important;
        }
       @media print
{
  table { page-break-after:auto }
  tr    { page-break-inside:avoid; page-break-after:auto }
  td    { page-break-inside:avoid; page-break-after:auto }
  thead { display:table-header-group }
  tfoot { display:table-footer-group }
}
    </style>
</head>
<body>
    <div class="container" style="width: 600px;" id="to-print-table">
        <form id="form1" runat="server">
            <div class="text-center"><strong>Sales Order</strong></div>
            <div class="box-body table-responsive">
                <asp:Panel ID="pnl1" runat="server">
                <table class="table-hover;">
                    <tr>

                        <%--<td style="padding: 4px 0px;">Status:&nbsp;<asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
                        </td>--%>
                        <td style="padding: 4px 0px;">Order No:&nbsp;<asp:Label ID="lblOrderNo" runat="server" Text=""></asp:Label>
                        </td>
                          <td style="padding: 4px 0px;"></td>
                        <td style="padding: 4px 0px;text-align: right;">Order Date:&nbsp;<asp:Label ID="lblOrderDate" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 4px 0px;">Bill To:&nbsp;<asp:Label ID="lblBillTo" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="padding: 4px 0px;"></td>
                        <td style="padding: 4px 0px;text-align: right;">Dispatch To:&nbsp;<asp:Label ID="lblDispatchTo" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 4px 0px;">
                            <asp:Label ID="lblBillToAddress" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="padding: 4px 0px;"></td>
                        <td style="padding: 4px 0px;text-align: right;">
                            <asp:Label ID="lblDispatchToAddress" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 4px 0px;">Product Group:&nbsp;<asp:Label ID="lblItemGroup" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="padding: 4px 0px;"></td>
                        <td style="padding: 4px 0px;text-align: right;">Transporter:&nbsp;<asp:Label ID="lblTransporter" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                     <tr>
                        <td style="padding: 4px 0px;">Dealer Code:&nbsp;<asp:Label ID="lblDistCode" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="padding: 4px 0px;"></td>
                        <td style="padding: 4px 0px;"></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:GridView ID="gvData" runat="server" class="table-indo" AutoGenerateColumns="False" Width="100%" Style="padding: 15px;">
                                <Columns>
                                    <asp:BoundField HeaderText="Product" DataField="ItemName" />
                                    <asp:BoundField HeaderText="Packing" DataField="Packing" HeaderStyle-CssClass="qty text-right"  ItemStyle-CssClass="qty text-right"/>
                                    <asp:BoundField HeaderText="Quantity" DataField="Qunatity" HeaderStyle-CssClass="qty text-right"  ItemStyle-CssClass="qty text-right"/>
                                    <asp:BoundField HeaderText="Price/Unit" DataField="PricePerUnit" />
                                    <asp:BoundField HeaderText="Discount" DataField="Discount" Visible="false" />
                                    <asp:BoundField HeaderText="Remark" DataField="Remarks" />
                                    <asp:BoundField HeaderText="Total" DataField="Total" HeaderStyle-CssClass="qty text-right" ItemStyle-CssClass="qty text-right"/>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 4px 0px;">Remark:&nbsp;<asp:Label ID="lblRemarks" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="padding: 4px 0px;"></td>
                        <td style="padding: 4px 0px;text-align:right">Total:&nbsp;<asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 4px 0px;">Created:&nbsp;<asp:Label ID="lblCreated" runat="server" Text=""></asp:Label>
                        </td>
                          <td style="padding: 4px 0px;"></td>
                       <%-- <td style="padding: 4px 0px;display:none;">Modified:&nbsp;<asp:Label ID="lblModified" runat="server" Text=""></asp:Label></td>--%>
                        <td style="padding: 4px 0px;text-align:right">Printed:&nbsp;<asp:Label ID="lblPrinted" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: right;">
                            <asp:Button ID="btnPint" runat="server" Text="Print" OnClick="btnPint_Click" />
                        </td>
                    </tr>
                </table>
                    </asp:Panel>
            </div>
        </form>
    </div>
</body>
</html>
