<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpenseSheetSummaryPrint.aspx.cs" Inherits="AstralFFMS.ExpenseSheetSummaryPrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
 <%--<link rel="stylesheet" type="text/css" href="Content/Styles.css" />
    <link rel="stylesheet" type="text/css" href="Content/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="Content/bootstrap.css" />--%>
    <style>
        #to-print-table table {
             filter: progid:DXImageTransform.Microsoft.BasicImage(Rotation=3);
        }
        #to-print-table table tr td {
            font-size: 12px;
        }

        #to-print-table table tr th {
            font-size: 12px;
            padding: 3px 3px;
            text-align: center;
        }

        table.table-indo tr td {
            text-align: center;
            padding: 5px;
        }
    </style>
</head>
<body>
    <div class="container" style="width: 600px" id="to-print-table">
        <form id="form1" runat="server">
            <div class="text-center"><strong>Expense Sheet Summary</strong></div>
            <div class="box-body table-responsive">
                <asp:Panel ID="pnl1" runat="server">
                    <table class="table-hover;" id="tbl1" runat="server">
                        <tr>
                            <td style="padding: 10px 2px;">
                                <asp:Label ID="lblSalesPersonName" runat="server" Text=""></asp:Label>
                            </td>
                            <td>&nbsp;
                            </td>
                            <td style="padding: 10px 2px;"><b>Code:</b><asp:Label ID="lblCode" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="padding: 10px 2px;"><b>Voucher Number:</b><asp:Label ID="lblVoucherNo" runat="server" Text=""></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="padding: 10px 2px;">
                                <b>City:</b>
                                <asp:Label ID="lblCity" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="padding: 10px 2px;">
                                <b>State:</b>
                                <asp:Label ID="lblState" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="padding: 10px 2px;"><b>Grade:</b><asp:Label ID="lblGrade" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="padding: 10px 2px;"><b>Designation:</b>:<asp:Label ID="lblDesignation" runat="server" Text=""></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="padding: 10px 2px;">
                                <b>Created:</b>
                                <asp:Label ID="lblCreated" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="padding: 10px 2px;">
                                <b>Submitted:</b>
                                <asp:Label ID="lblSubmitted" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="padding: 10px 2px;"><b>Total:</b><asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="padding: 10px 2px;"><b>Total Approved:</b><asp:Label ID="lblTotalApproved" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Table ID="tblDetails" runat="server" class="table table-bordered table-striped">
                                </asp:Table>
                            </td>
                        </tr>

                    </table>
                </asp:Panel>
            </div>
        </form>
    </div>
</body>
</html>
