<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MISReport1.aspx.cs" Inherits="AstralFFMS.MISReport1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <%--   <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>

    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 10000, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
        function validate() {
            if ($('#<%=ddlSuggestion.ClientID%>').val() == "0" || $('#<%=ddlSuggestion.ClientID%>').val() == "") {
                errormessage("Please Select Sale Type");
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
                "order": [[1, "desc"]],
                "footerCallback": function (tfoot, data, start, end, display) {
                    var api = this.api();
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    debugger;
                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Sale Amt'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(1).footer()).html(searchTotal);

                    if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(1).footer()).html('0.0') }
                }
            });
        });
        $(function () {
            $("#example2").DataTable({
                "order": [[0, "desc"]],
                "footerCallback": function (tfoot, data, start, end, display) {

                    var api = this.api();
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    debugger;
                    console.log(this);
                    //var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Sale Amt'; }).first().index();
                    var costColumnIndex = 2;
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(2).footer()).html(searchTotal);

                    if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(2).footer()).html('0.0') }
                }
            });
        });


        $(function () {
            $("#example3").DataTable({
                "order": [[0, "desc"]],
                "footerCallback": function (tfoot, data, start, end, display) {

                    var api = this.api();
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    debugger;
                    console.log(this);
                    //var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Sale Amt'; }).first().index();
                    var costColumnIndex = 3;
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(3).footer()).html(searchTotal);

                    if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(3).footer()).html('0.0') }
                }
            });
        });
    </script>
    <script type="text/javascript">
        var V = "";
        function Successmessage(V) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 10000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <script type="text/javascript">
        $(function () {
            $('[id*=LstDepartment]').multiselect({
                enableCaseInsensitiveFiltering: true,
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
            $('[id*=LstCompNature]').multiselect({
                enableCaseInsensitiveFiltering: true,
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
            $('[id*=LstState]').multiselect({
                enableCaseInsensitiveFiltering: true,
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
            $('[id*=LstCity]').multiselect({
                enableCaseInsensitiveFiltering: true,
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
            $('[id*=LstItem]').multiselect({
                enableCaseInsensitiveFiltering: true,
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
            $('[id*=LstArea]').multiselect({
                enableCaseInsensitiveFiltering: true,
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
            $('[id*=ddlParty]').multiselect({
                enableCaseInsensitiveFiltering: true,
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
            $('[id*=ListBox1]').multiselect({
                enableCaseInsensitiveFiltering: true,
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
            $('[id*=matGrpListBox]').multiselect({
                enableCaseInsensitiveFiltering: true,
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
            $('[id*=productListBox]').multiselect({
                enableCaseInsensitiveFiltering: true,
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
            $("#example1").DataTable();
        });

        //function DispData(val) {
        //    if (val == 'SalesPerson') {
        //        $('#divsp').show();

        //        $('#divdist').hide();
        //    }
        //    else {

        //        $('#divsp').hide();
        //        $('#divdist').show();
        //    }
        //}
    </script>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
        $(document).ready(function () {

            //  BindState();
        });
        function go(id) {
            if (id.value == "P") {
                document.getElementById("divParty").style.display = 'none';
                document.getElementById("divDist").style.display = 'block';
            }
            else if (id.value == "S") {
                document.getElementById("divParty").style.display = 'block';
                document.getElementById("divDist").style.display = 'none';
            }
            else {
                document.getElementById("divParty").style.display = 'none';
                document.getElementById("divDist").style.display = 'none';
            }
        }


        function BindState() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'

            var obj = { StateId: 0 };
            $<%--('#<%=ddlstatelist.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');--%>
            $.ajax({
                type: "POST",
                url: pageUrl + '/PopulateState',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=LstState.ClientID %>"));
            }
            function PopulateControl(list, control) {
                debugger;
                if (list.length > 0) {
                    //  control.removeAttr("disabled");
                    control.append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(list, function () {
                        control.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });

                    var id = $('#<%=HiddenStateID.ClientID%>').val();
                    alert(id);
                    if (id != "") {
                        control.val(id);

                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">Not available<option>');
                }

            }
        }
        function BindCity() {
            $('#<%=LstCity.ClientID%>').empty();
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("DataBindService.asmx/PopulateCityByState") %>',
                contentType: "application/json; charset=utf-8",
                data: '{StateID: "' + $('#<%=LstState.ClientID%>').val() + '" }',

                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    $('#<%=LstCity.ClientID%>').empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(jsdata1, function (key1, value1) {
                        $('#<%=LstCity.ClientID%>').append($("<option></option>").val(value1.CityID).html(value1.CityName));
                    });
                }
            });
            }

            function BindArea() {
                $('#<%=LstArea.ClientID%>').empty();
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("DataBindService.asmx/PopulateAreaByCity") %>',
                contentType: "application/json; charset=utf-8",
                data: '{CityID: "' + $('#<%=LstCity.ClientID%>').val() + '" }',

                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    $('#<%=LstArea.ClientID%>').empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(jsdata1, function (key1, value1) {
                        $('#<%=LstArea.ClientID%>').append($("<option></option>").val(value1.areaId).html(value1.areaName));
                    });
                }
            });
            }
            $(document).ready(function () {
                var bn = $('#<%=ddlSuggestion.ClientID%>').val();
                if ($('#<%=ddlSuggestion.ClientID%>').val() == "P") {
                    document.getElementById("divParty").style.display = 'none';
                    document.getElementById("divDist").style.display = 'block';
                    //document.getElementById("ContentPlaceHolder1_ddlParty").style.display = 'none';
                    //document.getElementById("ContentPlaceHolder1_ListBox1").style.display = 'block';
                }
                else if ($('#<%=ddlSuggestion.ClientID%>').val() == "S") {
                document.getElementById("divParty").style.display = 'block';
                document.getElementById("divDist").style.display = 'none';
                //document.getElementById("ContentPlaceHolder1_ddlParty").style.display = 'none';
                //document.getElementById("ContentPlaceHolder1_ListBox1").style.display = 'block';
            }
            else {
                document.getElementById("divParty").style.display = 'none';
                document.getElementById("divDist").style.display = 'none';
            }
            });
    </script>

    <style>
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .multiselect-container > li > a {
            white-space: normal;
        }

        .multiselect-container > li {
            width: 100%;
        }

        .input-group .form-control {
            height: 34px;
        }

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }

        #example1_wrapper .row {
            margin-right: 0px !important;
            margin-left: 0px !important;
        }

            #example1_wrapper .row .col-sm-12 {
                overflow-x: scroll !important;
                padding-left: 0px !important;
                margin-bottom: 10px;
            }
    </style>
    <section class="content">
        <asp:HiddenField ID="HiddenStateID" runat="server" />
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
                                <h3 class="box-title">MIS Report</h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                    <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-3 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sale Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlSuggestion" runat="server" Width="100%" onchange="go(this)" CssClass="form-control">
                                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                                                <asp:ListItem Text="Primary" Value="P"></asp:ListItem>
                                                <asp:ListItem Text="Secondary" Value="S"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <%--onchange="go(this)"--%>
                                    </div>
                                    <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Product Group:</label>
                                            <asp:ListBox ID="matGrpListBox" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="matGrpListBox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                </div>
                                <div class="row">

                                    <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Product:</label>
                                            <asp:ListBox ID="productListBox" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-3 col-xs-12" id="divState">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">State:</label>
                                            <asp:ListBox ID="LstState" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">City:</label>
                                            <asp:ListBox ID="LstCity" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Area:</label>
                                            <asp:ListBox ID="LstArea" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>



                                <div class="row" id="divArea">
                                </div>

                                <div class="row">
                                    <div class="col-md-3 col-sm-3 col-xs-12" id="divParty">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Party:</label>
                                            <asp:ListBox ID="ddlParty" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-3 col-xs-12" id="divDist">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Distributor:</label>
                                            <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="box-footer">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" OnClientClick="javascript:return validate();" runat="server" Text="Go" class="btn btn-primary"
                                    OnClick="btnGo_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                    OnClick="btnCancel_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" Visible="false" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                    OnClick="btnExport_Click" />
                            </div>
                            <div class="box-body table-responsive" id="divDSRState" runat="server">
                                <asp:Repeater ID="rptDSRState" runat="server" OnItemDataBound="rptDSR_ItemDataBound" OnItemCommand="rpt_ItemCommand" OnItemCreated="myRepeaterDeals_ItemCreated">
                                    <HeaderTemplate>
                                        <table id="example1" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <%-- <th>Date</th>--%>
                                                    <th>State</th>
                                                    <th>Sale Amt</th>
                                                    <th>Details</th>
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                    <th colspan="1" style="text-align: left">Grand Total:</th>
                                                    <th style="text-align: left"></th>
                                                    <th style="text-align: left"></th>
                                                </tr>
                                            </tfoot>

                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <asp:HiddenField ID="hfAreaId" runat="server" Value='<%#Eval("AreaId") %>' />
                                            <%--   <asp:HiddenField ID="hfDistId" runat="server" Value='<%#Eval("DistId") %>' />--%>
                                            <asp:HiddenField ID="hfColumn" runat="server" Value='<%#Eval("ColumnName") %>' />
                                            <%--   <td><%#Convert.ToDateTime(Eval("VDate")).ToString("dd/MMM/yyyy") %></td>
                                            <asp:Label ID="lblVDate" runat="server" Visible="false" Text='<%# Eval("VDate")%>'></asp:Label>--%>
                                            <td><%#Eval("StateName") %>
                                                <asp:Label ID="lblState" runat="server" Visible="false" Text='<%# Eval("StateName")%>'></asp:Label></td>
                                            <td><%#Eval("OrderAmount","{0:0.00}")%>
                                                <asp:Label ID="lblBillAmount" runat="server" Visible="false" Text='<%# Eval("OrderAmount")%>'></asp:Label></td>
                                            <td>
                                                <asp:LinkButton CommandName="select" ID="lnkItem2"
                                                    CausesValidation="False" runat="server" Text="Details" ToolTip="Details"
                                                    Width="80px" Font-Underline="True" OnClientClick="window.document.forms[0].target='_blank'; setTimeout(function(){window.document.forms[0].target='';}, 500);" /></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="box-body table-responsive" id="divDSRCity" runat="server">
                                <asp:Repeater ID="rptDSRCity" runat="server" OnItemDataBound="rptDSR_ItemDataBound" OnItemCommand="rpt_ItemCommand" OnItemCreated="myRepeaterDeals_ItemCreated">
                                    <HeaderTemplate>
                                        <table id="example2" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <%-- <th>Date</th>--%>
                                                    <th>State</th>
                                                    <th>City</th>
                                                    <th>Sale Amt</th>
                                                    <th>Details</th>
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                    <th colspan="2" style="text-align: left">Grand Total:</th>
                                                    <th style="text-align: left"></th>
                                                    <th style="text-align: left"></th>
                                                </tr>
                                            </tfoot>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <asp:HiddenField ID="hfAreaId" runat="server" Value='<%#Eval("AreaId") %>' />
                                            <%--   <asp:HiddenField ID="hfDistId" runat="server" Value='<%#Eval("DistId") %>' />--%>
                                            <asp:HiddenField ID="hfColumn" runat="server" Value='<%#Eval("ColumnName") %>' />
                                            <%--   <td><%#Convert.ToDateTime(Eval("VDate")).ToString("dd/MMM/yyyy") %></td>
                                            <asp:Label ID="lblVDate" runat="server" Visible="false" Text='<%# Eval("VDate")%>'></asp:Label>--%>
                                            <td><%#Eval("StateName") %>
                                                <asp:Label ID="lblState" runat="server" Visible="false" Text='<%# Eval("StateName")%>'></asp:Label></td>
                                            <td><%#Eval("CityName") %>
                                                <asp:Label ID="lblCity" runat="server" Visible="false" Text='<%# Eval("CityName")%>'></asp:Label></td>
                                            <td><%#Eval("OrderAmount","{0:0.00}")%>
                                                <asp:Label ID="lblBillAmount" runat="server" Visible="false" Text='<%# Eval("OrderAmount")%>'></asp:Label></td>
                                            <td>
                                                <asp:LinkButton CommandName="select" ID="lnkItem1"
                                                    CausesValidation="False" runat="server" Text="Details" ToolTip="Details"
                                                    Width="80px" Font-Underline="True" OnClientClick="window.document.forms[0].target='_blank'; setTimeout(function(){window.document.forms[0].target='';}, 500);" /></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="box-body table-responsive" id="divDSRArea" runat="server">
                                <asp:Repeater ID="rptDSRArea" runat="server" OnItemDataBound="rptDSR_ItemDataBound" OnItemCommand="rpt_ItemCommand" OnItemCreated="myRepeaterDeals_ItemCreated">
                                    <HeaderTemplate>
                                        <table id="example3" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <%-- <th>Date</th>--%>
                                                    <th>State</th>
                                                    <th>City</th>
                                                    <th>Area</th>
                                                    <th>Sale Amt</th>
                                                    <th>Details</th>
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                    <th colspan="3" style="text-align: left">Grand Total:</th>
                                                    <th style="text-align: left"></th>
                                                    <th style="text-align: left"></th>
                                                </tr>
                                            </tfoot>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <asp:HiddenField ID="hfAreaId" runat="server" Value='<%#Eval("AreaId") %>' />
                                            <%--   <asp:HiddenField ID="hfDistId" runat="server" Value='<%#Eval("DistId") %>' />--%>
                                            <asp:HiddenField ID="hfColumn" runat="server" Value='<%#Eval("ColumnName") %>' />
                                            <td><%#Eval("StateName") %>
                                                <asp:Label ID="lblState" runat="server" Visible="false" Text='<%# Eval("StateName")%>'></asp:Label></td>
                                            <td><%#Eval("CityName") %>
                                                <asp:Label ID="lblCity" runat="server" Visible="false" Text='<%# Eval("CityName")%>'></asp:Label></td>
                                            <td><%#Eval("AreaName") %>
                                                <asp:Label ID="lblArea" runat="server" Visible="false" Text='<%# Eval("AreaName")%>'></asp:Label></td>
                                            <td><%#Eval("OrderAmount","{0:0.00}")%>
                                                <asp:Label ID="lblBillAmount" runat="server" Visible="false" Text='<%# Eval("OrderAmount")%>'></asp:Label></td>
                                            <td>
                                                <asp:LinkButton CommandName="select" ID="lnkItem"
                                                    CausesValidation="False" runat="server" Text="Details" ToolTip="Details"
                                                    Width="80px" Font-Underline="True" OnClientClick="window.document.forms[0].target='_blank'; setTimeout(function(){window.document.forms[0].target='';}, 500);" /></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="box-body table-responsive" style="display: none;">
                                <asp:Repeater ID="suggreportrpt" runat="server" OnItemDataBound="suggreportrpt_ItemDataBound">
                                    <HeaderTemplate>
                                        <table id="example10" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <th>Date</th>
                                                    <th>Suggestion By</th>
                                                    <th>Sync Id</th>
                                                    <th>Product</th>
                                                    <th id="thsname" runat="server">Party Type</th>
                                                    <th>Party Name</th>
                                                    <th>Department</th>
                                                    <th>Complaint Nature</th>
                                                    <th>New App Area</th>
                                                    <th>Tech Adv</th>
                                                    <th>Prod Better</th>
                                                    <th>Status</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>

                                            <td><%#Convert.ToDateTime(Eval("Date")).ToString("dd/MMM/yyyy") %></td>
                                            <asp:Label ID="DateLabel" runat="server" Visible="false" Text='<%# Eval("Date")%>'></asp:Label>
                                            <td><%#Eval("SuggestionBy") %></td>
                                            <asp:Label ID="SuggestionByLabel" runat="server" Visible="false" Text='<%# Eval("SuggestionBy")%>'></asp:Label>
                                            <td><%#Eval("SyncId") %></td>
                                            <asp:Label ID="SyncIdLabel" runat="server" Visible="false" Text='<%# Eval("SyncId")%>'></asp:Label>
                                            <td><%#Eval("Item") %></td>
                                            <asp:Label ID="ItemLabel" runat="server" Visible="false" Text='<%# Eval("Item")%>'></asp:Label>
                                            <td id="tdsname" runat="server"><%#Eval("PartyTypeName") %></td>
                                            <asp:Label ID="PartyTypeNameLabel" runat="server" Visible="false" Text='<%# Eval("PartyTypeName")%>'></asp:Label>
                                            <td><%#Eval("Distributor")%></td>
                                            <asp:Label ID="DistributorLabel" runat="server" Visible="false" Text='<%# Eval("Distributor")%>'></asp:Label>
                                            <td><%#Eval("DepName") %></td>
                                            <asp:Label ID="DepNameLabel" runat="server" Visible="false" Text='<%# Eval("DepName")%>'></asp:Label>
                                            <td><%#Eval("ComplaintNature") %></td>
                                            <asp:Label ID="ComplaintNatureLabel" runat="server" Visible="false" Text='<%# Eval("ComplaintNature")%>'></asp:Label>
                                            <td><%#Eval("NewAppArea") %></td>
                                            <asp:Label ID="NewAppAreaLabel" runat="server" Visible="false" Text='<%# Eval("NewAppArea")%>'></asp:Label>
                                            <td><%#Eval("TechAdv") %></td>
                                            <asp:Label ID="TechAdvLabel" runat="server" Visible="false" Text='<%# Eval("TechAdv")%>'></asp:Label>
                                            <td><%#Eval("ProdBetter") %></td>
                                            <asp:Label ID="ProdBetterLabel" runat="server" Visible="false" Text='<%# Eval("ProdBetter")%>'></asp:Label>
                                            <td><%# Eval("Status")%></td>
                                            <asp:Label ID="StatusLabel" runat="server" Visible="false" Text='<%# Eval("Status")%>'></asp:Label>
                                            <%--<td><%# Eval("Status").ToString().Equals("P") ? "Pending" : "Resolved" %>
                                            <asp:Label ID="StatusLabel" runat="server" Visible="false" Text='<%# Eval("Status").ToString().Equals("P") ? "Pending" : "Resolved" %>'></asp:Label>--%>
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
    </section>
</asp:Content>
