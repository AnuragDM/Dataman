<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="TourPlanVsActualReport.aspx.cs" Inherits="AstralFFMS.TourPlanVsActualReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
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
     <script type="text/javascript">
         $(function () {
             $("[id*=trview] input[type=checkbox]").bind("click", function () {
                 var table = $(this).closest("table");
                 if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                     //Is Parent CheckBox
                     var childDiv = table.next();
                     var isChecked = $(this).is(":checked");
                     $("input[type=checkbox]", childDiv).each(function () {
                         if (isChecked) {
                             $(this).prop("checked", "checked");
                         } else {
                             $(this).removeAttr("checked");
                         }
                     });
                 } else {
                     //Is Child CheckBox
                     var parentDIV = $(this).closest("DIV");
                     if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                         $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                     } else {
                         $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                     }
                 }
             });
         })
    </script>
    <style type="text/css">
        .input-group .form-control {
            height: 34px;
        }

        .multiselect-container > li > a {
            white-space: normal;
        }

        .multiselect-container > li {
            width: 212px;
        }

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#tourvsActTable").DataTable();
        });
    </script>
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
                                    <h3 class="box-title">Tour Plan VS Actual</h3>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="col-md-8">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group" hidden>
                                                    <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                    <%--<asp:DropDownList ID="DdlSalesPerson" CssClass="form-control select2" runat="server"></asp:DropDownList>--%>
                                                    <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                </div>
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                                     
                                                    <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>
                                                </div>
                                            </div>
                                            </div>
                                             <div class="row">
                                            <div class="col-md-3 col-sm-6 col-xs-12">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Filter:</label>
                                                    <asp:DropDownList ID="ddlFilter" runat="server" class="form-control">
                                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                        <asp:ListItem Text="Variance" Value="Variance"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" id="frmdate" runat="server">
                                            <div class="col-md-3 col-sm-6 col-xs-12">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Month:</label>
                                                    <asp:DropDownList ID="ddlMonthSecSale" class="form-control" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3 col-sm-6 col-xs-12">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Year:</label>
                                                    <asp:DropDownList ID="ddlYearSecSale" class="form-control" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <%--  <div class="col-md-2 col-sm-3 col-xs-7">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Filter:</label>
                                                    <asp:DropDownList ID="ddlFilter" runat="server"  class="form-control">
                                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                        <asp:ListItem Text="Variance" Value="Variance"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="box-footer">
                                    <asp:Button type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary"
                                        OnClick="btnGo_Click" />
                                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Cancel_Click" />
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                        OnClick="btnExport_Click" />
                                    <%-- <input style="margin-right: 5px;" type="button" id="Go" value="Go" class="btn btn-primary" onc onclick="GetReport();" />--%>
                                </div>

                            </div>
                        </div>
                        <div id="rptmain" runat="server" style="display: none;">
                            <div class="box-header table-responsive">
                                <asp:Repeater ID="tourvsactrpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="tourvsActTable" class="table table-bordered table-striped rpttable">
                                            <thead>
                                                <tr>
                                                    <%-- <th style="text-align: left; width: 6%">S.No</th>--%>
                                                    <th style="text-align: left; width: 5%">Date</th>
                                                    <th style="text-align: left; width: 13%">Day</th>
                                                    <th style="text-align: left; width: 13%">Sales Person</th>
                                                    <th style="text-align: left; width: 13%">Sync Id</th>
                                                    <th style="text-align: left; width: 18%">Planned Distributor Name</th>
                                                    <th style="text-align: left; width: 18%">Planned City</th>
                                                    <th style="text-align: left; width: 18%">Purpose Of Visit</th>
                                                    <th style="text-align: left; width: 18%">Tour Remark</th>
                                                    <th style="text-align: left; width: 7%">Visited Distributor Name</th>
                                                    <th style="text-align: left;">Visited City</th>
                                                    <th style="text-align: left; width: 13%">Sales Person Worked With</th>
                                                    <th style="text-align: left; width: 18%">Remarks</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <%-- <td style="text-align: center; width: 6%"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%>
                                            </td>--%>
                                            <%--<td style="text-align: left; width: 5%"><%#Eval("Date","{0:dd}") %></td>--%>
                                            <td style="text-align: left; width: 5%"><%#Convert.ToDateTime(Eval("Date")).ToString("dd/MMM/yyyy") %></td>
                                            <asp:Label ID="DateLabel" runat="server" Visible="false" Text='<%# Eval("Date")%>'></asp:Label>
                                            <td style="text-align: left; width: 13%"><%#Eval("Date","{0:dddd}") %></td>
                                            <asp:Label ID="DayLabel" runat="server" Visible="false" Text='<%#Eval("Date","{0:dddd}")%>'></asp:Label>
                                            <th style="text-align: left; width: 18%"><%#Eval("SalesRepName") %></th>
                                            <asp:Label ID="SalesRepNameLabel" runat="server" Visible="false" Text='<%# Eval("SalesRepName")%>'></asp:Label>
                                            <th style="text-align: left; width: 18%"><%#Eval("SyncId") %></th>
                                            <asp:Label ID="SyncIdLabel" runat="server" Visible="false" Text='<%# Eval("SyncId")%>'></asp:Label>
                                            <th style="text-align: left; width: 18%"><%#Eval("TourDistributor") %></th>
                                            <asp:Label ID="TourDistributorLabel" runat="server" Visible="false" Text='<%# Eval("TourDistributor")%>'></asp:Label>
                                            <td style="text-align: left; width: 18%"><%#Eval("TourCity") %></td>
                                            <asp:Label ID="TourCityLabel" runat="server" Visible="false" Text='<%# Eval("TourCity")%>'></asp:Label>
                                            <td style="text-align: left; width: 18%"><%#Eval("Purpose") %></td>
                                            <asp:Label ID="PurposeLabel" runat="server" Visible="false" Text='<%# Eval("Purpose")%>'></asp:Label>
                                            <td style="text-align: left; width: 18%"><%#Eval("TourRemark") %></td>
                                            <asp:Label ID="TourRemarkLabel" runat="server" Visible="false" Text='<%# Eval("TourRemark")%>'></asp:Label>
                                            <td style="text-align: left; width: 7%"><%#Eval("VisitDistributor") %></td>
                                            <asp:Label ID="VisitDistributorLabel" runat="server" Visible="false" Text='<%# Eval("VisitDistributor")%>'></asp:Label>
                                            <td style="text-align: left; width: 30%"><%#Eval("VisitCity") %></td>
                                            <asp:Label ID="VisitCityLabel" runat="server" Visible="false" Text='<%# Eval("VisitCity")%>'></asp:Label>
                                            <td style="text-align: left; width: 13%"><%#Eval("Srep") %></td>
                                            <asp:Label ID="SrepLabel" runat="server" Visible="false" Text='<%# Eval("Srep")%>'></asp:Label>
                                            <td style="text-align: left; width: 13%"><%#Eval("Remarks") %></td>
                                            <asp:Label ID="RemarksLabel" runat="server" Visible="false" Text='<%# Eval("Remarks")%>'></asp:Label>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
