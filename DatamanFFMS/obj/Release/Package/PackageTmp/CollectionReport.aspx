<%@ Page Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="CollectionReport.aspx.cs" Inherits="AstralFFMS.CollectionReport" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
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
        $(function () {
            //$("#itemsaleTable").DataTable({
            //    "order": [[0, "desc"]]
            //});   
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
                    //alert(isChecked);
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
        //$(function () {
        //    $(".select2").select2();
        //});
    </script>
    <script type="text/javascript">

        //function postBackByObject() {
        //    var o = window.event.srcElement;
        //    if (o.tagName == "INPUT" && o.type == "checkbox") {
        //        __doPostBack("", "");
        //    }
        //}

         <%--function fireCheckChanged(e) {
             var ListBox1 = document.getElementById('<%= trview.ClientID %>');
             var evnt = ((window.event) ? (event) : (e));
             var element = evnt.srcElement || evnt.target;

             if (element.tagName == "INPUT" && element.type == "checkbox") {
                 __doPostBack("", "");
             }
         }--%>

        function loding() {
            $('#spinner').show();

        }
    </script>

    <script type="text/javascript">
        function validate() {
            var selectedvalue = [];

            $("#<%=trview.ClientID %> :checked").each(function () {
                selectedvalue.push($(this).val());
            });
            if (selectedvalue == "") {
                errormessage("Please Select Sales Person");
                return false;
            }
            loding();

        }
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
            width: 100%;
        }

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }

        #itemsaleTable_wrapper .row {
            margin-right: 0px !important;
            margin-left: 0px !important;
        }

            #itemsaleTable_wrapper .row .col-sm-12 {
                overflow-x: scroll !important;
                padding-left: 0px !important;
                margin-bottom: 10px;
            }

        .spinner {
            position: absolute;
            top: 50%;
            left: 50%;
            margin-left: -50px; /* half width of the spinner gif */
            margin-top: -50px; /* half height of the spinner gif */
            text-align: center;
            z-index: 999;
            overflow: auto;
            width: 100px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }
    </style>

    <script type="text/javascript">
        $("[id$=btnExport]").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + encodeURIComponent($('div[id$=rptmain]').html()));
            e.preventDefault();
        });
    </script>
    <section class="content">
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
        </div>
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
                                    <%-- <h3 class="box-title">Collection Report</h3>--%>
                                    <h3 class="box-title">
                                        <asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="row">

                                        <div class="col-md-3 col-sm-3 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>

                                            </div>
                                        </div>
                                        <div class="col-md-3 col-sm-3 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Collection Type:</label>
                                                <asp:DropDownList ID="ddlCollectiontype" runat="server" Width="99%" CssClass="form-control">
                                                    <asp:ListItem Text="Primary" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Secondary" Value="1"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>

                                        </div>

                                         <div class="col-md-3 col-sm-3 col-xs-12">
                                            <div id="DIV1" class="form-group">
                                                <label for="exampleInputEmail1">From Date:</label>
                                                <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                            </div>
                                        </div>
                                        <%--<div class="col-md-1"></div>--%>
                                        <div class="col-md-3 col-sm-3 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">To:</label>
                                                <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                            </div>
                                        </div>

                                    </div>
                                    <div class="clearfix"></div>

                                </div>
                                <div class="box-footer">

                                    <asp:Button type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClientClick="return validate();"
                                        OnClick="btnGo_Click" />
                                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Cancel_Click" />
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                        OnClick="btnExport_Click" />
                                </div>
                                <br />
                            </div>
                        </div>
                        <div id="rptmain" runat="server">
                            <div class="box-header table-responsive">
                                <asp:Repeater ID="TopRetailer" runat="server">
                                    <HeaderTemplate>
                                        <table id="itemsaleTable" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <th style="text-align: left; width: 5%">SNo.</th>
                                                    <th style="text-align: left; width: 10%">Date</th>
                                                    <th style="text-align: left; width: 15%">Sales Person</th>
                                                    <th style="text-align: left; width: 8%">Retailer/Distributor</th>
                                                    <th style="text-align: left; width: 8%">Sync Id</th>
                                                    <th style="text-align: left; width: 8%">Mobile No.</th>
                                                    <th style="text-align: left; width: 10%">Payment Mode</th>
                                                    <th style="text-align: right; width: 10%">Amount</th>
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                    <th colspan="7" style="text-align: right">Total:</th>
                                                    <th style="text-align: right"></th>

                                                </tr>
                                            </tfoot>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <%--<asp:HiddenField ID="hdnitemid" runat="server" Value='<%#Eval("partyid")%>' />  --%>
                                            <td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>
                                            <td><%# Eval("VDate","{0: dd/MMM/yyyy}")%>
                                                <asp:Label ID="lblVDate" runat="server" Visible="false" Text='<%# Eval("VDate","{0: dd/MMM/yyyy}")%>'></asp:Label></td>
                                            <td><%# Eval("smname")%>
                                                <asp:Label ID="lblsmname" runat="server" Visible="false" Text='<%# Eval("smname")%>'></asp:Label></td>
                                            <td><%# Eval("partyName")%>
                                                <asp:Label ID="lblpartyName" runat="server" Visible="false" Text='<%# Eval("partyName")%>'></asp:Label></td>
                                            <td><%# Eval("SyncId")%>
                                                <asp:Label ID="lblSyncId" runat="server" Visible="false" Text='<%# Eval("SyncId")%>'></asp:Label></td>
                                            <td><%# Eval("Mobile")%>
                                                <asp:Label ID="lblMobile" runat="server" Visible="false" Text='<%# Eval("Mobile")%>'></asp:Label></td>
                                            <td><%# Eval("Mode")%>
                                                <asp:Label ID="lblMode" runat="server" Visible="false" Text='<%# Eval("Mode")%>'></asp:Label></td>
                                            <td style="text-align: right"><%# Eval("Amount","{0:0.00}")%>
                                                <asp:Label ID="lblAmount" runat="server" Visible="false" Text='<%# Eval("Amount","{0:0.00}")%>'></asp:Label>
                                        </tr>

                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>       </table>       
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



    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#itemsaleTable').dataTable({
                "order": [[1, "desc"]],
                "footerCallback": function (tfoot, data, start, end, display) {
                    //$(tfoot).find('th').eq(0).html("Starting index is " + start);
                    var api = this.api();
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    debugger;
                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Amount'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    $(api.column(7).footer()).html(searchTotal);

                    if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(4).footer()).html('0.0') }

                }
            });
        });
    </script>
</asp:Content>
