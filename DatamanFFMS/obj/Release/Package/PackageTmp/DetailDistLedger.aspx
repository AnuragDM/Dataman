<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="DetailDistLedger.aspx.cs" Inherits="AstralFFMS.DetailDistLedger" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".rpttable").DataTable({ "bSort": false });
        });
    </script>

    <section class="content">
        <div class="box-body">
            <div class="box box-primary">
                <div class="box-header with-border">
                    <h3 class="box-title">Detail Distributor Ledger:</h3>
                    <asp:DropDownList ID="monthDropDownList" runat="server"></asp:DropDownList>&nbsp;&nbsp;<asp:DropDownList ID="yearDropDownList" runat="server">
                    </asp:DropDownList>&nbsp;&nbsp;<asp:Button type="button" ID="GetLedgerDeatil" runat="server" Text="Go" class="btn btn-primary"
                        OnClick="GetLedgerDeatil_Click" />
                    <div style="float: right">
                        <asp:Button type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                    </div>
                    <div>
                     <asp:Label ID ="lbldistname" runat ="server" Text="Distributor Name:" Font-Bold="true"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                      <asp:Label ID ="lblDist"  runat="server"></asp:Label>

                    </div>
                </div>
                <div class="box-body table-responsive">
                       <asp:Repeater ID="rptDistLedger" runat="server">
                        <HeaderTemplate>
                            <table id="disttable" class="table table-bordered table-striped rpttable">
                                <thead>
                                    <tr>
                                        <%--<th style="text-align: center;">S.No</th>--%>
                                        <th style="text-align: left;">Date</th>
                                        <th style="text-align: left;">Document No.</th>
                                        <th style="text-align: left;">Narration</th>
                                        <th style="text-align: right;">Debit</th>
                                        <th style="text-align: right;">Credit</th>
                                        <th style="text-align: right;">Balance</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                               <%-- <td style="text-align: center;"><%#Eval("row") %></td>--%>
                                <td style="text-align: left;"><%#Convert.ToDateTime(Eval("vdate")).ToString("dd/MMM/yyyy") %></td>
                                <td style="text-align: left;"><%#Eval("DLdocId") %></td>
                                <td style="text-align: left;"><%#Eval("Narration") %></td>
                                <td style="text-align: right;"><%#Eval("amtDr") %></td>
                                <td style="text-align: right;"><%#Convert.ToDecimal(Eval("amtCr")) > 0 ? Eval("amtCr") : (-1)*Convert.ToDecimal(Eval("amtCr")) %></td>
                                <td style="text-align: right;"><%#Eval("balance") %></td>
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
