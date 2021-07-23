<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DistOutstandingReport.aspx.cs" Inherits="AstralFFMS.DistOutstandingReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
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
            $("#dsrTable").DataTable();
            $("#disttable").DataTable();
        });
    </script>
    <style type="text/css">
        .table-responsive{
            border: 1px solid #fff;
        }
        .containerStaff {
            border: 1px solid #ccc;
            overflow-y: auto;
            min-height: 200px;
            width: 134%;
            overflow-x: auto;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .multiselect-container > li > a {
            white-space: normal;
        }

        .input-group .form-control {
            height: 34px;
        }
    </style>
    <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <%--<h3 class="box-title">Distributor Outstanding</h3>--%>
                                <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                    <div class="col-md-3 col-sm-5 col-xs-9">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Salesperson:</label>
                                            <%-- <asp:DropDownList ID="DdlSalesPerson" Width="100%"
                                                CssClass="form-control select2" runat="server">
                                            </asp:DropDownList>--%>
                                            <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="box-footer">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary"
                                    OnClick="btnGo_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                    OnClick="btnCancel_Click" />
                            </div>

                            <div class="box-body table-responsive">
                                <asp:Repeater ID="rptDistOutRep" runat="server">
                                    <HeaderTemplate>
                                        <table id="dsrTable" class="table table-bordered table-striped rpttable">
                                            <thead>
                                                <tr>
                                                    <th style="text-align: center; width: 6%">S.No</th>
                                                    <th style="text-align: left; width: 35%">Distributor Name</th>
                                                    <th style="text-align: right; width: 35%">Outstanding Balance</th>
                                                    <th style="visibility: hidden; width: 10%">Details</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("DistId") %>' />
                                            <td style="text-align: center; width: 6%"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>
                                            <td style="text-align: left; width: 35%"><%#Eval("partyname") %></td>
                                            <%--<td style="text-align: right; width: 35%"><%#Eval("Balance") %></td>--%>
                                            <td style="text-align: right; width: 35%"><%#Convert.ToDecimal(Eval("Balance")) > 0 ? Eval("Balance")+ " Dr"  : Convert.ToDecimal(Eval("Balance")) + "Cr" %></td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click1">Details</asp:LinkButton></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <br />
                            <div id="detailDistOutDiv" runat="server" style="display: none;">
                                <div class="box-body table-responsive">
                                     <asp:Label ID ="lblDist" runat="server"></asp:Label>

                                    <asp:Repeater ID="rptDistLedger" runat="server">
                                        <HeaderTemplate>
                                            <table id="disttable" class="table table-bordered table-striped rpttable">
                                                <thead>
                                                    <tr>
                                                        <th style="text-align: center;">S.No</th>
                                                        <th style="text-align: center;">Date</th>
                                                        <th style="text-align: center;">Document No.</th>
                                                        <th style="text-align: center;">Narration</th>
                                                        <th style="text-align: right;">Debit</th>
                                                        <th style="text-align: right;">Credit</th>
                                                        <th style="text-align: right;">Balance</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="text-align: center;"><%#Eval("row") %></td>
                                                <td><%#Convert.ToDateTime(Eval("vdate")).ToString("dd/MMM/yyyy") %></td>
                                                <td style="text-align: center;"><%#Eval("DLdocId") %></td>
                                                <td style="text-align: center;"><%#Eval("Narration") %></td>
                                                <td style="text-align: right;"><%#Eval("amtDr") %></td>
                                                <td style="text-align: right;"><%#Eval("amtCr") %></td>
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
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
