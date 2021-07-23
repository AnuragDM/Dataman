<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="RptDSRPartyWiseAnalysis_V2.aspx.cs" Inherits="AstralFFMS.RptDSRPartyWiseAnalysis_V2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--   <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <link type="text/css" rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <%--  <a href="RptDSRPartyWiseAnalysis.aspx">RptDSRPartyWiseAnalysis.aspx</a>--%>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <style>
        body .ui-tooltip {
            padding: 0 5px;
            font-size: 11px;
            font-weight: 600;
        }
    </style>
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
                            //$(this).removeAttr("checked");
                        }
                    });
                } else {
                    //Is Child CheckBox
                    var parentDIV = $(this).closest("DIV");
                    if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                        $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                    } else {
                        //$("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                    }
                }
            });
        })
    </script>
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

        function btnSubmitfunc() {


            var checked_radio = $("[id*=rblview] input:checked");
            var value = checked_radio.val();

            if (value == "SalesPerson") {
                var selectedvalue = [];
                $("#<%=trview.ClientID %> :checked").each(function () {
                    selectedvalue.push($(this).val());
                });
                if (value != "Party") {
                    if (selectedvalue == "") {
                        errormessage("Please Select Sales Person");
                        return false;
                    }
                }
            }
            else {
                var state = [];
                $("#<%=ddlState.ClientID %> :selected").each(function () {
                    state.push($(this).val());
                });
                if (state == "" || state == "0") {
                    errormessage("Please Select State");
                    return false;
                }
              
                var city = [];
                $("#<%=ddlCity.ClientID %> :selected").each(function () {
                    city.push($(this).val());
                });
                if (city == "" || city == "0") {
                    errormessage("Please Select City");
                    return false;
                }
               
                var partyId = [];
                $("#<%=ddlParty.ClientID %> :selected").each(function (i, selected) {
                    partyId[i] = $(selected).val();
                });

            }

            var parttype = [];
            $("#<%=ddlPType.ClientID %> :selected").each(function () {
                parttype.push($(this).val());
            });
      
            var salespersontype = [];
            $("#<%=ddlSType.ClientID %> :selected").each(function () {
                salespersontype.push($(this).val());
            });
           

            var dsrtype = [];
            $("#<%=ddlDsrType.ClientID %> :selected").each(function () {
                dsrtype.push($(this).val());
            });
           

            var status = [];
            $("#<%=ddlStatus.ClientID %> :selected").each(function () {
                status.push($(this).val());
            });
          


            var type = [];
            $("#<%=ddltype.ClientID %> :selected").each(function () {
                type.push($(this).val());
            });
            

            var checked_radio = $("[id*=rblview] input:checked");
            var value = checked_radio.val();
           

        }

    

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

    function loding() {
        $('#spinner').show();
    }
    </script>

    <style type="text/css">
     
        .input-group .form-control {
            height: 34px;
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

        @media (max-width: 600px) {
            .ui-dialog.ui-widget.ui-widget-content.ui-corner-all.ui-front.ui-draggable.ui-resizable {
                width: 100% !important;
            }
        }

           #rblview>input[type=radio] {
    margin-right: 12px !important;
    margin-left: 7px !important;
}

           /*#rblview{
    margin-right: 12px !important;
    margin-left: 7px !important;
}*/

                  input[type=checkbox] {
    margin-right: 12px ;
    margin-left: 7px ;
}
   
        .button1 {
            box-shadow: 0px 2px 4px 2px #888888;
            margin-left: 10px;
            margin-top: 7px;
            margin-right: 5px;
        }

        h2 {
            font-size: 20px !important;
            font-weight: 600 !important;
            margin-left: 13px !important;
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
            width: 330px;
        }

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }
    </style>
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

        function fireCheckChanged(e) {
            var ListBox1 = document.getElementById('<%= trview.ClientID %>');
            var evnt = ((window.event) ? (event) : (e));
            var element = evnt.srcElement || evnt.target;

            if (element.tagName == "INPUT" && element.type == "checkbox") {
                __doPostBack("", "");
            }
        }
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
            $('[id*=ddlState]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=ddlCity]').multiselect({
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
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            display: table;
        }

        margin {
            margin-left: 20px;
        }
    </style>
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
                    <div class="col-md-12">
                        <div class="box-header with-border">
                            <%-- <h3 class="box-title">DSR - Partywise Analysis</h3>--%>
                            <img id="img-Header" src="img/analysis_New.png" style="width: 47px; height: 47px;" />
                            <h2 class="box-title">
                                <asp:Label ID="lblPageHeader" runat="server"></asp:Label></h2>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">View As:</label>
                                        <asp:RadioButtonList ID="rblview" RepeatDirection="Horizontal" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblview_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="SalesPerson" Text="Sales Person Wise" class="margin"></asp:ListItem>
                                            <asp:ListItem Value="Party" Text="Party Wise" class="margin"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>

                                <%--<div class="row">--%>
                                <div id="divtrview" class="col-md-4 col-sm-4 col-xs-12" runat="server">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <b>
                                            <asp:TreeView ID="trview" runat="server" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>
                                        </b>
                                    </div>
                                </div>
                                <%--</div>--%>
                            </div>
                            <div id="Partyview"  runat="server" visible="false">
                                <div class="row">
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">State:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                          <%--  <asp:DropDownList ID="ddlState" runat="server" Width="99%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                            </asp:DropDownList>--%>
                                            <asp:ListBox ID="ddlState" runat="server" Width="100%" SelectionMode="Single" AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" CssClass="form-control" ></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                           <%-- <asp:DropDownList ID="ddlCity" runat="server" Width="99%" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                                            </asp:DropDownList>--%>
                                            <asp:ListBox ID="ddlCity" runat="server" Width="100%" SelectionMode="Multiple" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Party:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:ListBox ID="ddlParty" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                            <%-- <asp:DropDownList ID="ddlParty" runat="server" CssClass="form-control" Width="99%">                                                                                          
                                            </asp:DropDownList>--%>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Type:</label>
                                            <asp:DropDownList ID="ddltype" runat="server" Width="99%" CssClass="form-control">
                                                <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                                <asp:ListItem Text="Order" Value="Order"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Party Status:</label>
                                        <asp:DropDownList ID="ddlPType" runat="server" CssClass="form-control" Width="99%">
                                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                            <asp:ListItem Text="Active" Value="Active" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Not Active" Value="Not Active"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Sales Person Status:</label>
                                        <asp:DropDownList ID="ddlSType" runat="server" CssClass="form-control" Width="99%">
                                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                            <asp:ListItem Text="Active" Value="Active" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Not Active" Value="Not Active"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">DSR Type:</label>
                                        <asp:DropDownList ID="ddlDsrType" runat="server" CssClass="form-control" Width="99%">
                                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                            <asp:ListItem Text="Lock" Value="Lock"></asp:ListItem>
                                            <asp:ListItem Text="UnLock" Value="UnLock"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Status:</label>
                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" Width="99%">
                                            <asp:ListItem Text="All" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Approve" Value="Approve"></asp:ListItem>
                                            <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                            <asp:ListItem Text="Reject" Value="Reject"></asp:ListItem>
                                        </asp:DropDownList>
                                        <input type="hidden" id="hiddsrtype" />
                                        <input type="hidden" id="hidstatus" />
                                        <input type="hidden" id="hidpartytype" />
                                        <input type="hidden" id="hidsalesmantype" />
                                        <input type="hidden" id="hidstate" />
                                        <input type="hidden" id="hidcity" />
                                        <input type="hidden" id="hidparty" />
                                        <input type="hidden" id="hidview" />
                                        <input type="hidden" id="hidtype" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">From Date:</label>
                                        <asp:TextBox ID="frmTextBox" CssClass="form-control" runat="server"
                                            Style="background-color: white;"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="frmTextBox_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server"
                                            BehaviorID="frmTextBox_CalendarExtender"
                                            TargetControlID="frmTextBox"></ajaxToolkit:CalendarExtender>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">To Date:</label>
                                        <asp:TextBox ID="toTextBox" CssClass="form-control" runat="server"
                                            Style="background-color: white;"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="toTextBox_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server"
                                            BehaviorID="toTextBox_CalendarExtender"
                                            TargetControlID="toTextBox"></ajaxToolkit:CalendarExtender>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="box-footer">
                          
                            <asp:Button type="button" ID="btnGo" runat="server" Text="Generate" class="btn btn-primary button1" OnClientClick="GetReport();" Visible="true"
                                OnClick="btnGo_Click" />
                            <asp:Button type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary button1" OnClick="btnCancel_Click" />

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
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#example1').dataTable({
                "order": [[0, "desc"]],
                "pageLength": 500,
                "footerCallback": function (tfoot, data, start, end, display) {
                    //$(tfoot).find('th').eq(0).html("Starting index is " + start);
                    var api = this.api();
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Amount'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(12).footer()).html(searchTotal);

                    if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(13).footer()).html('0.0') }
                }
            });
        });
    </script>
</asp:Content>
