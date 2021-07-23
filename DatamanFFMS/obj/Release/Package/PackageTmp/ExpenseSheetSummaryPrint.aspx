<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpenseSheetSummaryPrint.aspx.cs" Inherits="AstralFFMS.ExpenseSheetSummaryPrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--  <link rel="stylesheet" type="text/css" href="Content/Styles.css">
    <link rel="stylesheet" type="text/css" href="Content/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="Content/bootstrap.css">--%>
    <style>
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
            <div class="row">
               <%-- <div style="width: 30%">
                    <asp:Image runat="server" ID="imggoldiee" ImageUrl="~/img/GoldieeLogo.png" />
                </div>--%>
                <div style="width: 100%">
                    <div class="text-center"><strong>Expense Sheet Summary</strong></div>
                </div>
            </div>
            <%--<div style="width: 10px"></div>--%>
            <div class="box-body table-responsive">
                <asp:Panel ID="pnl1" runat="server">
                    <table class="table-hover;" id="tbl1" runat="server">
                        <tr>

                            <td style="padding: 10px 2px;">
                                <b>Sales Person:</b>
                                <asp:Label ID="lblSalesPersonName" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="padding: 10px 2px;">
                                <b>Expense Group Name:</b>
                                <asp:Label ID="lblGroupName" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>

                            <td style="padding: 10px 2px;">
                                <b>Voucher Number:</b><asp:Label ID="lblVoucherNo" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="padding: 10px 2px;">
                                <b>Verified By:</b>
                                <asp:Label ID="lblVerified" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>

                            <td style="padding: 5px 2px;">
                                <b>Verified DateTime:</b>
                                <asp:Label ID="lblVerfiedDatetIme" runat="server" Text="dfdgffgfgf"></asp:Label>
                            </td>
                            <td style="padding: 5px 2px;">
                                <b>Approved By:</b>
                                <asp:Label ID="lblState" runat="server" Text=""></asp:Label>
                            </td>


                        </tr>
                        <tr>
                            <td style="padding: 5px 2px;">
                                <b>Designation:</b>:<asp:Label ID="lblDesignation" runat="server" Text=""></asp:Label>
                            </td>


                            <td style="padding: 5px 2px;">
                                <b>Created:</b>
                                <asp:Label ID="lblCreated" runat="server" Text=""></asp:Label>
                            </td>


                        </tr>
                        <tr>
                            <td style="padding: 5px 2px;">
                                <b>Created By:</b>

                                <asp:Label ID="lblCreatedBy" runat="server" Text="dfdfgfhgh"></asp:Label>

                            </td>

                            <td style="padding: 5px 2px;">
                                <b>Submitted:</b>
                                <asp:Label ID="lblSubmitted" runat="server" Text=""></asp:Label>
                            </td>

                        </tr>


                        <tr>
                            <td style="padding: 5px 2px;">
                                <b>Total:</b><asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
                            </td>

                            <td style="padding: 5px 2px;">
                                <b>Total Verified:</b><asp:Label ID="lblTotalVerified" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 5px 2px;">
                                <b>Total Approved:</b><asp:Label ID="lblTotalApproved" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lblexpgp" runat="server" visable="false" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" ShowFooter="true">
                                    <%--<Columns>
                                         <asp:BoundField HeaderText="Expense Type" DataField="ExpenseTypeName" />
                                         <asp:TemplateField HeaderText="Exp Dt.">
                                            <ItemTemplate>
                                                <asp:Label ID="billDateLbl" runat="server"
                                                    Text='<%#Eval("BillDate").ToString()!="" ? Convert.ToDateTime(Eval("BillDate")).ToString("dd/MMM/yyyy") : string.Empty %>'>                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="From City" DataField="farea" />
                                         <asp:BoundField HeaderText="To City" DataField="tarea" />
                                         <asp:BoundField HeaderText="Mode" DataField="Name" />   
                                            <asp:BoundField HeaderText="DA" DataField="Da" ItemStyle-CssClass="alignAmt" FooterStyle-CssClass="alignAmt">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Fare" DataField="BillAmount" ItemStyle-CssClass="alignAmt" FooterStyle-CssClass="alignAmt">
                                            <ItemStyle HorizontalAlign="Right"  />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Claim Amt." DataField="ClaimAmount" ItemStyle-CssClass="alignAmt" FooterStyle-CssClass="alignAmt">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>

                                         <asp:BoundField HeaderText="Verified Amount" DataField="VerifiedAmt" ItemStyle-CssClass="alignAmt" FooterStyle-CssClass="alignAmt">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="App Amt." DataField="ApprovedAmount" ItemStyle-CssClass="alignAmt" FooterStyle-CssClass="alignAmt">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Advance Amt." ItemStyle-CssClass="alignAmt">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                         <asp:BoundField HeaderText="Final Amt." ItemStyle-CssClass="alignAmt">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                              <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>

                                                <asp:Label ID="lblMainRemarks" runat="server"
                                                    Text='<%#Eval("MainRemarks")%>'></asp:Label>
                                                
                                            </ItemTemplate>
                                               <ItemStyle Width="100" CssClass="WrapText colPad" />  
                                                 <HeaderStyle Width="300px" CssClass="colPad" /> 
                                        
                                        </asp:TemplateField>
                                    </Columns>--%>
                                    <Columns>
                                        <asp:BoundField HeaderText="Expense Type Name" DataField="ExpenseTypeName" ItemStyle-CssClass="colPad" HeaderStyle-CssClass="colPad" />

                                        <asp:TemplateField HeaderText="Expense Date">
                                            <ItemTemplate>
                                                <asp:Label ID="billDateLbl" runat="server"
                                                    Text='<%#Eval("BillDate").ToString()!="" ? Convert.ToDateTime(Eval("BillDate")).ToString("dd/MMM/yyyy") : string.Empty %>'>                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField HeaderText="From City" DataField="farea" ItemStyle-CssClass="colPad" HeaderStyle-CssClass="colPad" />
                                        <asp:TemplateField HeaderText="To City">
                                            <ItemTemplate>

                                                <asp:Label ID="ibltarea" runat="server"
                                                    Text='<%#Eval("tarea")%>'></asp:Label>

                                            </ItemTemplate>
                                            <ItemStyle Width="100" CssClass="WrapText colPad" />
                                            <HeaderStyle Width="300px" CssClass="colPad" />
                                            <%-- <ItemStyle Wrap="true" />--%>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Distance Travelled (In Kms)" DataField="kms" ItemStyle-CssClass="colPad" HeaderStyle-CssClass="colPad" />
                                        <asp:TemplateField HeaderText="Mode">
                                            <ItemTemplate>

                                                <asp:Label ID="ibltarea" runat="server"
                                                    Text='<%#Eval("Name")%>'></asp:Label>

                                            </ItemTemplate>
                                            <ItemStyle Width="100" CssClass="WrapText colPad" />
                                            <HeaderStyle Width="300px" CssClass="colPad" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Night Halt Amount" DataField="NighthaltAmt" ItemStyle-CssClass="alignAmt colPad" FooterStyle-CssClass="alignAmt" HeaderStyle-CssClass="colPad">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" CssClass="colPad" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="DA" DataField="Da" ItemStyle-CssClass="alignAmt colPad" FooterStyle-CssClass="alignAmt" HeaderStyle-CssClass="colPad">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" CssClass="colPad" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Fare" DataField="BillAmount" ItemStyle-CssClass="alignAmt colPad" FooterStyle-CssClass="alignAmt" HeaderStyle-CssClass="colPad">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" CssClass="colPad" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Claim Amount" DataField="ClaimAmount" ItemStyle-CssClass="alignAmt colPad" FooterStyle-CssClass="alignAmt" HeaderStyle-CssClass="colPad">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" CssClass="colPad" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Verified Amount" DataField="VerifiedAmt" ItemStyle-CssClass="alignAmt colPad" HeaderStyle-CssClass="colPad">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" CssClass="colPad" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Approved Amount" DataField="ApprovedAmount" ItemStyle-CssClass="alignAmt colPad" HeaderStyle-CssClass="colPad">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" CssClass="colPad" />
                                        </asp:BoundField>
                                        <%--<asp:BoundField HeaderText="Advance Amount" ItemStyle-CssClass="alignAmt colPad" HeaderStyle-CssClass="colPad">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" CssClass="colPad" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Final Amount" ItemStyle-CssClass="alignAmt colPad" HeaderStyle-CssClass="colPad">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" CssClass="colPad" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>

                                                <asp:Label ID="lblMainRemarks" runat="server"
                                                    Text='<%#Eval("MainRemarks")%>'></asp:Label>

                                            </ItemTemplate>
                                            <ItemStyle Width="100" CssClass="WrapText colPad" />
                                            <HeaderStyle Width="300px" CssClass="colPad" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 5px 2px;">
                                <b>Total Adv. Amount:</b><asp:Label ID="lbladvamo" runat="server" Text=""></asp:Label>
                            </td>

                            <td style="padding: 5px 2px;">
                                <b>Total Final Amount:</b><asp:Label ID="lblfnlamo" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </form>
    </div>
</body>
</html>
