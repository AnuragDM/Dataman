<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ExpenseSheetsummary.aspx.cs" Inherits="AstralFFMS.ExpenseSheetsummary" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <script type="text/javascript">
        $(document).ready(function () {
            $("#dateTimeInput").jqxDateTimeInput({ width: '200px', height: '25px', theme: 'arctic', formatString: 'dd-MMM-yyyy' });
            var date = new Date();
            date.setDate(date.getDate())
            $("#dateTimeInput").jqxDateTimeInput('setMaxDate', date);
        });
    </script>
    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 250, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <script type="text/javascript">
        var V = "";
        function Successmessage(V) {
            $("#messageNotification").jqxNotification({
                width: 250, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <!-- left column -->

        <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- general form elements -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">Expense Sheet Summary</h3>
                        <div style="float: right">                            
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" Visible="false"/>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <div class="box-body">
                        <div class="row">
                            <div class='col-md-3'>
                                <asp:Label ID="lblSalesPersonName" runat="server" Text=""></asp:Label>
                            </div>
                            <div class='col-md-3'>
                            </div>
                            <div class='col-md-3'>
                                <b>Code:</b><asp:Label ID="lblCode" runat="server" Text=""></asp:Label>
                            </div>
                            <div class='col-md-3'>
                                <b>Voucher Number:</b><asp:Label ID="lblVoucherNo" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class='row'>
                            <div class='col-md-3'>
                                <b>City:</b>
                                <asp:Label ID="lblCity" runat="server" Text=""></asp:Label>
                            </div>
                            <div class='col-md-3'>
                                <b>State:</b>
                                <asp:Label ID="lblState" runat="server" Text=""></asp:Label>
                            </div>
                            <div class='col-md-3'>
                                 <b>Grade:</b><asp:Label ID="lblGrade" runat="server" Text=""></asp:Label>
                            </div>
                            <div class='col-md-3'>
                                <b>Designation:</b>:<asp:Label ID="lblDesignation" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class='row'>
                            <div class='col-md-3'>
                                 <b>Created:</b> 
                                <asp:Label ID="lblCreated" runat="server" Text=""></asp:Label>
                            </div>
                            <div class='col-md-3'>
                                 <b>Submitted:</b> 
                                <asp:Label ID="lblSubmitted" runat="server" Text=""></asp:Label>
                            </div>
                            <div class='col-md-3'>
                                <b>Total:</b><asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
                            </div>
                            <div class='col-md-3'>
                                <b>Total Approved:</b><asp:Label ID="lblTotalApproved" runat="server" Text="" ></asp:Label>
                            </div>
                        </div>
                        <div class='row'>
                            &nbsp;
                        </div>
                        <div class='row'>
                            <div class="table table-responsive">
                                <asp:table ID="tblDetails" runat="server" class="table table-bordered table-striped">
                                </asp:table>
                                
                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                        <div class="row">
                            <div class='col-md-3'>

                                <div class="float:left;">
                                    <asp:Button ID="btnPrint" runat="server" Text="Print" class="btn btn-primary" OnClick="btnPrint_Click" OnClientClick="target ='_blank';" Visible="false"/>
                                    <asp:Button ID="btnExcelExport" runat="server" Text="Export to Excel" class="btn btn-primary" OnClick="btnExcelExport_Click" />
                                </div>
                            </div>
                            <div class='col-md-9'>
                                &nbsp;
                            </div>

                        </div>



                    </div>
                </div>
            </div>
        </div>

    </section>   
</asp:Content>