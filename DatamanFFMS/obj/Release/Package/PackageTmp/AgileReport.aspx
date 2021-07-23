<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="AgileReport.aspx.cs" Inherits="AstralFFMS.AgileReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>

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

        $(function () {
            $('[id*=LstYear]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',GetMonthlyItemSaleGetMonthlyItemSale
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
    </script>
    <%--<script type="text/javascript">
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
    </script>--%>
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
    <%--  <script type="text/javascript">
        var V = "";
        function Successmessage(V) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>--%>
    <script type="text/javascript">
        function btnSubmitfunc() {

         <%--   var selectedvalue = [];
            $("#<%=trview.ClientID %> :checked").each(function () {
                 selectedvalue.push($(this).val());
             });
             if (selectedvalue == "") {
                 errormessage("Please Select Sales Person");
                 return false;
             }--%>
            var selectedValues = [];
            $("#<%=ListBox1.ClientID %> :selected").each(function () {
                selectedValues.push($(this).val());
            });
            if (selectedValues == "") {
                errormessage("Please Select Distributor");
                return false;
            }
            $("#hiddistributor").val(selectedValues);


            loding();
            <%--BindGridView();--%>
        }
    </script>
    <script type="text/javascript">

        function loding() {
            $('#spinner').show();
        }
    </script>
    <%--<script type="text/javascript">

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

    </script>--%>
    <style type="text/css">
        .containerStaff {
            border: 1px solid #ccc;
            overflow-y: auto;
            min-height: 200px;
            width: 134%;
            overflow-x: auto;
        }

        .multiselect-container > li {
            width: auto;
        }

            .multiselect-container > li > a {
                white-space: normal;
            }

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }

        .input-group .form-control {
            height: 34px;
        }

        #invtable_wrapper .row {
            margin-right: 0px !important;
            margin-left: 0px !important;
        }

            #invtable_wrapper .row .col-sm-12 {
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

    <script type="text/javascript">

        function SetTarget() {

            document.forms[0].target = "_blank";

        }

    </script>

    <section class="content">
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" width="30%" height="50%" alt="Loading" /><br />
            <b>Loading Data....</b>
        </div>
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
                                <div class="box-header with-border">
                                    <%--<h3 class="box-title">Distributor Invoice Report</h3>--%>
                                    <h3 class="box-title">
                                        <asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->

                                <div class="box-body" id="div1">
                                    <div class="row">
                                        <%--<div class="col-md-3 col-sm-3 col-xs-12">
                                       
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged"></asp:TreeView>
                                        </div>
                                    </div>--%>
                                        <div class="col-md-3 col-sm-3 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Distributor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <div class="">
                                                    <asp:ListBox ID="ListBox1" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                                    <input type="hidden" id="hiddistributor" />
                                                </div>
                                            </div>
                                        </div>
                                        <%--<div class="col-md-3 col-sm-3 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Financial Year:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:ListBox ID="LstYear" runat="server" SelectionMode="Single" AutoPostBack="true" class="form-control"></asp:ListBox>
                                        </div>
                                    </div>--%>
                                        <%--  <div class="col-md-3 col-sm-3 col-xs-12">
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
                                    </div>--%>
                                    </div>
                                    <div class="row">
                                    </div>
                                </div>

                                <div class="box-footer">
                                    <%-- <input type="button" runat="server" onclick="btnSubmitfunc();" class="btn btn-primary" value="Go" style="display: none" />--%>
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClientClick="btnSubmitfunc()" OnClick="btnGo_Click" />
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click"/>
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="btnExport_Click"/>
                                </div>
                            </div>

                            <br />
                            <div id="detailDiv" runat="server">
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="detailDistrpt" runat="server" OnItemCommand="detailDistrpt_ItemCommand">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <th>Action</th>
                                                        <th>Party Name</th>
                                                        <th>(0-90) Days</th>
                                                        <th>(91-120) Days</th>
                                                        <th>(121-150) Days</th>
                                                        <th>(151-180) Days</th>
                                                        <th>(181-365) Days</th>
                                                        <th>(>=366) Days</th>
                                                        <th>Total</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <asp:HiddenField ID="hdnsyncid" runat="server" Value='<%#Eval("syncid")%>' />
                                                <asp:HiddenField ID="hdnparty" runat="server" Value='<%#Eval("PartyName")%>' />
                                                <td>
                                                    <asp:LinkButton CommandName="select" ID="lnkEdit"
                                                        CausesValidation="False" runat="server" Text="Aging Details" ToolTip="Aging Details"
                                                        Width="80px" Font-Underline="True" OnClientClick="window.document.forms[0].target='_blank'; setTimeout(function(){window.document.forms[0].target='';}, 500);" /></td>
                                                <td><%#Eval("PartyName") %>
                                                    <asp:Label ID="lblparty" runat="server" Visible="false" Text='<%# Eval("PartyName")%>'></asp:Label>
                                                </td>
                                                <td><%#Eval("Curr") %>
                                                    <asp:Label ID="lblcurr" runat="server" Visible="false" Text='<%# Eval("Curr")%>'></asp:Label>
                                                </td>
                                                <td><%#Eval("NinOT") %>
                                                    <asp:Label ID="lblninty" runat="server" Visible="false" Text='<%# Eval("NinOT")%>'></asp:Label>
                                                </td>
                                                <td><%#Eval("OtOF") %>
                                                    <asp:Label ID="lblonetwozero" runat="server" Visible="false" Text='<%# Eval("OtOF")%>'></asp:Label>
                                                </td>
                                                <td><%#Eval("OFOE") %>
                                                    <asp:Label ID="lblonefivezero" runat="server" Visible="false" Text='<%# Eval("OFOE")%>'></asp:Label>
                                                </td>
                                                <td><%#Eval("OELst") %>
                                                    <asp:Label ID="lbloneegtzero" runat="server" Visible="false" Text='<%# Eval("OELst")%>'></asp:Label>
                                                </td>
                                                <td><%#Eval("lastyr") %>
                                                    <asp:Label ID="lbllastyr" runat="server" Visible="false" Text='<%# Eval("lastyr")%>'></asp:Label>
                                                </td>
                                                <td><%#Eval("total") %>
                                                     <asp:Label ID="lbltotal" runat="server" Visible="false" Text='<%# Eval("total")%>'></asp:Label>
                                                </td>
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
        </div>
    </section>
</asp:Content>
