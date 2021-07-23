<%@ Page Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="TopPotentialcustomer.aspx.cs" Inherits="AstralFFMS.TopPotentialcustomer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=Lstbeatbox]').multiselect({
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
            $('[id*=CityListbox]').multiselect({
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
            $('[id*=AreaListbox]').multiselect({
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
            $('[id*=salespersonListBox]').multiselect({
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

        function fireCheckChanged(e) {
            var ListBox1 = document.getElementById('<%= trview.ClientID %>');
            var evnt = ((window.event) ? (event) : (e));
            var element = evnt.srcElement || evnt.target;

            if (element.tagName == "INPUT" && element.type == "checkbox") {
                __doPostBack("", "");
            }
        }

        function loding() {
            $('#spinner').show();
        }
    </script>
    <%-- <script type="text/javascript">
         function validate(persons) {
             if (document.getElementById("<%=ListBox1.ClientID%>").value == "" || document.getElementById("<%=ListBox1.ClientID%>").value == "0") {                
                       errormessage("Please Select Distributor");
                       document.getElementById("<%=ListBox1.ClientID%>").focus();                 
                       return false;
                   }                                
         
           
               }

    </script>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            //Hide the div          
            //  $('div[id$="rptmain"]').hide();
            //conversely do the following to show it again if needed later
            //$('#showdiv').show();
        });

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
        function ValidateCheckBoxList() {
            var checkBoxList = document.getElementById("<%=trview.ClientID %>");
            var checkboxes = checkBoxList.getElementsByTagName("input");
            var isValid = false;
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    isValid = true;
                    break;
                }
                alert(isValid + "2");
            }
            args.IsValid = isValid;
        }
    </script>
    <script type="text/javascript">
        $("[id$=btnExport]").click(function (e) {
            alert("123");
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
                                    <%-- <h3 class="box-title">Top Potential Customer</h3>--%>
                                    <h3 class="box-title">
                                        <asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="row">

                                        <%-- <div class="col-md-3 col-sm-6 col-xs-7" hidden>
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>  
                                                <asp:ListBox ID="salespersonListBox" runat="server" SelectionMode="Multiple"
                                                    OnSelectedIndexChanged="salespersonListBox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                            </div>
                                        </div>        --%>
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged"></asp:TreeView>

                                            </div>
                                        </div>
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Beat:</label>
                                                <asp:ListBox ID="Lstbeatbox" runat="server" SelectionMode="Multiple"></asp:ListBox>

                                            </div>

                                        </div>
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">No. of Recods:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:TextBox ID="txt_noofrecords" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;">99</asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>

                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div id="DIV1" class="form-group">
                                                <label for="exampleInputEmail1">From Date:</label>
                                                <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                            </div>
                                        </div>

                                        <div class="col-md-4 col-sm-4 col-xs-12">
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

                                    <asp:Button type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClientClick="loding()"
                                        OnClick="btnGo_Click" />
                                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Cancel_Click" />
                                    <%-- <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                        OnClick="btnExport_Click" />--%>
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                        OnClick="btnExport_Click" />
                                    <%-- <input style="margin-right: 5px;" type="button" id="Go" value="Go" class="btn btn-primary" onc onclick="GetReport();" />--%>
                                </div>
                                <br />
                            </div>
                        </div>
                        <div id="rptmain" runat="server">
                            <div class="box-header table-responsive">
                                <asp:Repeater ID="rptTopPotentials" runat="server">
                                    <HeaderTemplate>
                                        <table id="itemsaleTable" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <th style="text-align: left; width: 10%">Retailer</th>
                                                    <th style="text-align: left; width: 8%">State</th>
                                                    <th style="text-align: left; width: 8%">City</th>
                                                    <th style="text-align: left; width: 15%">Area</th>
                                                    <th style="text-align: left; width: 15%">Beat</th>
                                                    <th style="text-align: left; width: 8%">SalesPerson</th>
                                                    <th style="text-align: right; width: 10%">Potential</th>
                                                    <th style="text-align: right; width: 10%">Total Sales</th>
                                                    <th style="text-align: right; width: 10%">Diff</th>
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                    <th colspan="6" style="text-align: right">Total:</th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>

                                                </tr>
                                            </tfoot>
                                    </HeaderTemplate>
                                    <ItemTemplate>

                                        <tr>
                                            <td><%# Eval("CustName")%>
                                                <asp:Label ID="lblCustName" runat="server" Visible="false" Text='<%# Eval("CustName")%>'></asp:Label></td>
                                            <td><%# Eval("State")%>
                                                <asp:Label ID="lblState" runat="server" Visible="false" Text='<%# Eval("State")%>'></asp:Label></td>
                                            <td><%# Eval("City")%>
                                                <asp:Label ID="lblCity" runat="server" Visible="false" Text='<%# Eval("City")%>'></asp:Label></td>
                                            <td><%# Eval("area")%>
                                                <asp:Label ID="lblarea" runat="server" Visible="false" Text='<%# Eval("area")%>'></asp:Label></td>
                                            <td><%# Eval("Beat")%>
                                                <asp:Label ID="lblBeat" runat="server" Visible="false" Text='<%# Eval("Beat")%>'></asp:Label></td>
                                            <td><%# Eval("SalePer")%>
                                                <asp:Label ID="lblSalePer" runat="server" Visible="false" Text='<%# Eval("SalePer")%>'></asp:Label></td>
                                            <td style="text-align: right"><%# Eval("Potential")%>
                                                <asp:Label ID="lblPotential" runat="server" Visible="false" Text='<%# Eval("Potential")%>'></asp:Label></td>
                                            <td style="text-align: right"><%# string.Format("{0:n2}",Eval("TotSale"))%>
                                                <asp:Label ID="lblTotSale" runat="server" Visible="false" Text='<%# string.Format("{0:n2}",Eval("TotSale"))%>'></asp:Label></td>
                                            <td style="text-align: right"><%# Eval("Diff")%>
                                                <asp:Label ID="lblDiff" runat="server" Visible="false" Text='<%# Eval("Diff")%>'></asp:Label></td>
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
                "order": [[6, "desc"]],
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
                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Potential'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);


                    var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Total Sales'; }).first().index();
                    var totalData1 = api.column(costColumnIndex1).data();
                    var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                    var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                    var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);


                    var costColumnIndex2 = $('th').filter(function (i) { return $(this).text() == 'Diff'; }).first().index();
                    var totalData2 = api.column(costColumnIndex1).data();
                    var total2 = totalData2.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData2 = api.column(costColumnIndex2, { page: 'current' }).data();
                    var pageTotal2 = pageTotalData2.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData2 = api.column(costColumnIndex2, { 'filter': 'applied' }).data();
                    var searchTotal2 = searchTotalData2.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);



                    $(api.column(6).footer()).html(searchTotal);
                    $(api.column(7).footer()).html(searchTotal1);
                    $(api.column(8).footer()).html(searchTotal2);

                    if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(6).footer()).html('0.0') }
                    if (searchTotal1 == 'NaN' || searchTotal1 == '') { $(api.column(7).footer()).html('0.0') }
                    if (searchTotal2 == 'NaN' || searchTotal1 == '') { $(api.column(8).footer()).html('0.0') }

                    //if (searchTotal2 == 'NaN' || searchTotal2 == '') { $(api.column(9).footer()).html('0.0') }
                }
            });
        });
    </script>
</asp:Content>
