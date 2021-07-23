<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="PendingDSRList.aspx.cs" Inherits="AstralFFMS.PendingDSRList" %>
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
                    <h3 class="box-title">DSR Approval List</h3>
                    <div style="float: right">
                        <asp:Button type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                    </div>
                </div>
                <div class="box-body table-responsive">
                    <asp:Repeater ID="rptDSR" runat="server">
                        <HeaderTemplate>
                            <table id="disttable" class="table table-bordered table-striped rpttable">
                                <thead>
                                    <tr>
                                        <th style="text-align: center;">S.No</th>
                                        <th style="text-align: center;">Date</th>
                                        <th style="text-align: center;">Document No.</th>
                                        <th style="text-align: center;">Name</th>
                                        <th style="text-align: center; visibility:hidden;">Details</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="text-align: center;"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>
                                <td style="text-align: center;"><%#Eval("vdate","{0:dd/MMM/yyyy}") %></td>
                                <td style="text-align: center;"><%#Eval("VisitDocId") %></td>
                                <td style="text-align: center;"><%#Eval("SMName") %></td>
                                <td style="text-align: center; width: 10%">
                                        <a href="<%# String.Format("/RptDailyWorkingApprovalL1.aspx?"+"SMId="+Eval("SMId")+"&VisDocId="+Eval("VisitDocId")+"&Page="+"PENDINGDSR") %>">Approve</a>
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
