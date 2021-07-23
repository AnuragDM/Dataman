<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DailySalesReportL1.aspx.cs" Inherits="AstralFFMS.DailySalesReportL1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <script src="plugins/datatableGrouping.js" type="text/javascript"></script>
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
            $(document).ready(function () {
                $('#example1').dataTable({
                    "bLengthChange": false,
                    "bPaginate": false,
                    "bJQueryUI": true
                }).rowGrouping({
                    bExpandableGrouping: true,
                    bExpandSingleGroup: false,
                    iExpandGroupOffset: -1,
                    asExpandedGroups: [""],
                    iGroupingColumnIndex: 2,
                    sGroupingColumnSortDirection: "asc",
                    iGroupingOrderByColumnIndex: 2
                });
            });
        });
    </script>
    <style type="text/css">
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

        #ContentPlaceHolder1_CheckBoxList1 td {
            padding: 3px;
        }

        .multiselect-container > li {
            width: 212px;
        }
    </style>
    <style type="text/css">
        td.group {
            background-color: #d5eafd !important;
            border-bottom: 1px solid #94bafd;
            border-top: 1px solid #94bafd;
        }

        td.expanded-group {
            background: url("http://jquery-datatables-row-grouping.googlecode.com/svn/trunk/media/images/minus.jpg") no-repeat scroll left center transparent;
        }

        tr:hover td.expanded-group {
            background: url("http://jquery-datatables-row-grouping.googlecode.com/svn/trunk/media/images/minus.jpg") no-repeat scroll left center #c0e1ff !important;
        }

        td.collapsed-group {
            background: url("http://jquery-datatables-row-grouping.googlecode.com/svn/trunk/media/images/plus.jpg") no-repeat scroll left center transparent;
        }

        tr:hover td.collapsed-group {
            background: url("http://jquery-datatables-row-grouping.googlecode.com/svn/trunk/media/images/plus.jpg") no-repeat scroll left center #c0e1ff !important;
        }

        .DataTables_sort_wrapper span {
            float: right;
        }
    </style>
     <script type="text/javascript">
         var V1 = "";
         function errormessage(V1) {
             $("#messageNotification").jqxNotification({
                 width: 300, position: "top-right", opacity: 2,
                 autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "error"
             });
             $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <script type="text/javascript">
        var V = "";
        function Successmessage(V) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "success"
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
        <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Daily Sales Report (Level-1)</h3>
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
                                <div class="row">
                                    <div class="col-md-5 col-sm-5 col-xs-9">
                                        <div class="form-group">
                                            <%--<label for="exampleInputEmail1" style="visibility: hidden;">chklist:</label>--%>
                                            <asp:CheckBox ID="orderCheckBox" runat="server" Text="Order" />
                                            <asp:CheckBox ID="demoCheckBox" runat="server" Text="Demo" />
                                            <asp:CheckBox ID="fvCheckBox" runat="server" Text="FailedVisit" />
                                            <%--  <asp:CheckBoxList ID="CheckBoxList1" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Order" Value="Order"></asp:ListItem>
                                                <asp:ListItem Text="Demo" Value="Demo"></asp:ListItem>
                                                <asp:ListItem Text="FailedVisit" Value="FailedVisit"></asp:ListItem>
                                            </asp:CheckBoxList>--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 col-sm-4 col-xs-9">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-2 col-sm-4 col-xs-9">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
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
                            <div id="rptmain" runat="server" style="display: none;">
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="distreportrpt" runat="server">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <th>SNo.</th>
                                                        <th style="width: 18%;">VisitDate</th>
                                                        <th>Beat</th>
                                                        <th>Party</th>
                                                        <th>SType</th>
                                                        <th>Item</th>
                                                        <th>Qty</th>
                                                        <th>Value</th>
                                                        <th>CompleteAppDetail</th>
                                                        <th>Remark</th>
                                                        <th>AvailabilityShop</th>
                                                        <th>IsPartyConverted</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>
                                                <td style="text-align: left; width: 18%"><%#System.Convert.ToDateTime(Eval("visitDate")).ToString("dd/MMM/yyyy") %></td>
                                                <td><%#Eval("Beat") %></td>
                                                <td><%#Eval("Party") %></td>
                                                <td><%#Eval("SType") %></td>
                                                <td><%#Eval("Item") %></td>
                                                <td><%#Eval("Qty") %></td>
                                                <td><%#Eval("Value") %></td>
                                                <td><%#Eval("CompleteAppDetail") %></td>
                                                <td><%#Eval("Remarks") %></td>
                                                <td><%#Eval("AvailabilityShop") %></td>
                                                <td><%#Eval("IsPartyConverted") %></td>
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
