<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="SaleStoreRanking.aspx.cs" Inherits="AstralFFMS.SaleStoreRanking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
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
            $('[id*=ListBox1]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',
                buttonWidth: '100%',
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
    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
                "order": [[7, "desc"]]
            });
        });
   
        function validatesale() {

            
            var selectedvalue = [];
            $("#<%=trview.ClientID %> :checked").each(function () {
                selectedvalue.push($(this).val());
            });
            if (selectedvalue == "") {
                errormessage("Please Select Sales Person");
                return false;
            }

            if ($('#<%=txtfmDate.ClientID%>').val() == "") {
                errormessage("Please Select From Date");
                return false;
            }
            if ($('#<%=txttodate.ClientID%>').val() == "") {
                errormessage("Please Select To Date");
                return false;
            }
        }

        function validatestore() {


            <%--var selectedvalue = [];
            $("#<%=trview.ClientID %> :checked").each(function () {
                selectedvalue.push($(this).val());
            });
            if (selectedvalue == "") {
                errormessage("Please Select Sales Person");
                return false;
            }--%>
            if ($('#<%=ddlretailer.ClientID%>').val() == "0") {
                errormessage("Please Select Retailer.");
                return false;
            }
            if ($('#<%=txtfmDate.ClientID%>').val() == "") {
                errormessage("Please Select From Date");
                return false;
            }
            if ($('#<%=txttodate.ClientID%>').val() == "") {
                errormessage("Please Select To Date");
                return false;
            }
        }
    </script>
      <script type="text/javascript">
          //$(function () {
          //    $('[id*=listretailer]').multiselect({
          //        enableCaseInsensitiveFiltering: true,
          //        //buttonWidth: '200px',
          //        buttonWidth: '100%',
          //        includeSelectAllOption: true,
          //        maxHeight: 200,
          //        width: 215,
          //        enableFiltering: true,
          //        filterPlaceholder: 'Search'
          //    });
          //});
    </script>
    <style type="text/css">
        .input-group .form-control {
            height: 34px;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            /*display: table;*/
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
                                <h3 class="box-title">Sale And Store Ranking Report</h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">     
                                     <%--<div class="col-md-4 col-sm-6 col-xs-12">--%><div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>
                                        </div>
                                    </div>
                                   <%-- <div class="col-lg-2 col-md-3 col-sm-4 col-xs-12">--%><div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Retailer:</label>
                                            <asp:DropDownList runat="server" CssClass="form-control" ID="ddlretailer"></asp:DropDownList>
                                              <%--<asp:ListBox ID="listretailer" runat="server" SelectionMode="Multiple"
                                                    OnSelectedIndexChanged="listretailer_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">                                   
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                        </div>
                                    </div>                                   
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                        </div>
                                    </div>
                                </div>

                                <div style="height:20px"></div>

                                <div class="row">
                                    <div class="col-md-12 col-sm-6 col-xs-7">
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnsalespersonranking" runat="server" Text="Salesperson Ranking" class="btn btn-primary"
                                            OnClick="btnsalespersonranking_Click"  OnClientClick="javascript:return validatesale();"/>
                                          <asp:Button Style="margin-right: 5px;" type="button" ID="btnstoreranking" runat="server" Text="Store Ranking" class="btn btn-primary"
                                            OnClick="btnstoreranking_Click"   OnClientClick="javascript:return validatestore();"/>
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                            OnClick="btnCancel_Click" />
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                            OnClick="btnExport_Click" />
                                    </div>
                                </div>
                                <br />

                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="leavereportrpt" runat="server">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                         <th>S.No.</th>
                                                        <th>Salesperson/Retailer</th>
                                                         <th>Total Party Visited</th>
                                                         <th>Total Party Orderd</th>
                                                        <th>Total Order</th>
                                                        <th>Failed Visit</th>
                                                          <th>Demo</th>
                                                         <th>Efficiency (in %age)</th>
                                                         <%--<th>To Date</th>--%>
                                                        <%--<th>Reason</th>
                                                        <th>Status</th>
                                                        <th>Approved By</th>           --%>                                            
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Eval("SrNo") %>
                                                     <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="lblsrno" runat="server" Visible="false" Text='<%# Eval("SrNo")%>'></asp:Label>
                                            <%-- End --%>
                                                </td>
                                                <td><%#Eval("SMName") %>
                                                     <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="smnameLabel" runat="server" Visible="false" Text='<%# Eval("SMName")%>'></asp:Label>
                                            <%-- End --%>
                                                </td>
                                                <td><%#Eval("TotalVisitedParty") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="syncIdLabel" runat="server" Visible="false" Text='<%# Eval("TotalVisitedParty")%>'></asp:Label>
                                            <%-- End --%>
                                                </td>
                                                <td><%#Eval("orderdparty") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Eval("orderdparty")%>'></asp:Label>
                                            <%-- End --%>
                                                </td>
                                                <td><%#Eval("OrderAmount") %>
                                                      <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="nofdaysLabel" runat="server" Visible="false" Text='<%# Eval("OrderAmount")%>'></asp:Label>
                                            <%-- End --%>
                                                </td>
                                                <td><%#Eval("FailedVisit") %>
                                                     <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="fromDateLabel" runat="server" Visible="false" Text='<%# Eval("FailedVisit")%>'></asp:Label>
                                            <%-- End --%>
                                                </td>
                                                <td><%#Eval("demo") %>
                                                     <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="lbldemo" runat="server" Visible="false" Text='<%# Eval("demo")%>'></asp:Label>
                                            <%-- End --%>
                                                </td>
                                                <td><%#Eval("efficiency") %>
                                                     <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="EfficiencyLabel" runat="server" Visible="false" Text='<%# Eval("efficiency")%>'></asp:Label>
                                            <%-- End --%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>     </table>       
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                                <br />
                                <div>
                                    <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </section>
</asp:Content>

