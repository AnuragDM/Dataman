<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false"CodeBehind="RetailerNextVisit_V2.aspx.cs" Inherits="AstralFFMS.RetailerNxtVst_V2" %>

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


        $(function () {
            $('[id*=LstTyp]').multiselect({
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


        $(function () {
            $('[id*=ListArea]').multiselect({
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

        $(function () {
            $('[id*=ListDist]').multiselect({
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


        $(function () {
            $('[id*=ListBeat]').multiselect({
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
            $("#example1").DataTable();
        });

        function validatesale() {

            var selectedValues = [];
            $("#<%=LstType.ClientID %> :selected").each(function () {
                 selectedValues.push($(this).val());
             });
             if (selectedvalue == "") {
                 errormessage("Please Select Type");
                 return false;
             }
             var selectedvalue = [];
             $("#<%=trview.ClientID %> :checked").each(function () {
                selectedvalue.push($(this).val());
            });
            if (selectedvalue == "") {
                errormessage("Please Select Sales Person");
                return false;
            }

            if ($('#<%=txtfmDate.ClientID%>').val() == "") {
                errormessage("Please Select Date");
                return false;
            }

        }

        function validateDist() {



           <%-- if ($('#<%=ddlretailer.ClientID%>').val() == "0") {
                errormessage("Please Select Distributor.");
                return false;
            }--%>
            if ($('#<%=txtfmDate.ClientID%>').val() == "") {
                errormessage("Please Select Date");
                return false;
            }

        }


        function validatebeat() {



            <%--if ($('#<%=ddlbeat.ClientID%>').val() == "0") {
                errormessage("Please Select Beat.");
                return false;
            }--%>
            if ($('#<%=txtfmDate.ClientID%>').val() == "") {
                errormessage("Please Select Date");
                return false;
            }

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
        input[type=checkbox], input[type=radio] {
            margin-right: 12px;
            margin-left: 12px;
        }


        .button1 {
            box-shadow: 0px 2px 4px 2px #888888;
            margin-left: 10px;
        }

          .box-header img {
            margin-top: -7px;
        }

        h2 {
            font-size: 20px !important;
            font-weight: 600 !important;
            margin-left: 13px !important;
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
                                <%--<h3 class="box-title">Retailer Next Visit Report</h3>--%>
                                  <img src="img/Location/building.png" />
                                <h2 class="box-title">
                                    <asp:Label ID="lblPageHeader" runat="server"></asp:Label></h2>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">

                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:ListBox ID="LstType" runat="server" SelectionMode="Single" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="LstType_SelectedIndexChanged"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12" id="sprw" runat="server" visible="false">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                          <b><asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged"></asp:TreeView></b>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12" id="dstrw" runat="server" visible="false">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Distributor:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                             <asp:ListBox ID="ListDist" runat="server" SelectionMode="Multiple" ></asp:ListBox>
                                        </div>
                                    </div>

                                </div>
                                <div class="row" id="btrw" runat="server" visible="false">
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Area:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />
                                            <asp:ListBox ID="ListArea" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="ListArea_SelectedIndexChanged"></asp:ListBox>
                                            <input type="hidden" id="Hidden1" runat="server" />
                                        </div>
                                    </div>
                                    <div class=" col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Beat:</label>
                                                   <asp:ListBox ID="ListBeat" runat="server" SelectionMode="Multiple" ></asp:ListBox>
                                            <%--<input type="hidden" id="Hidden2" runat="server" />--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">Date:</label>
                                            <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                        </div>
                                    </div>
                                    <%--<div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                        </div>
                                    </div>--%>
                                </div>



                                <div class="box-footer">
                                    <%--<div class="col-md-12 col-sm-6 col-xs-7">--%>

                                    <asp:Button type="button" ID="btnsaleperson" runat="server" Text="Generate" class="btn btn-primary button1" OnClick="btnsaleperson_Click" />
                                <%--    <asp:Button Style="margin-right: 5px;" type="button" ID="btndist" runat="server" Text="Distributor wise" class="btn btn-primary" OnClick="btndist_Click" OnClientClick="javascript:return validateDist();" />
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnbeat" runat="server" Text="Beat wise" class="btn btn-primary"
                                        OnClick="btnbeat_Click" OnClientClick="javascript:return validatebeat();" />--%>

                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary button1"
                                        OnClick="btnCancel_Click" />
                                 <%--   <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                        OnClick="btnExport_Click" />--%>
                                    <%-- </div>--%>
                                </div>
                                <br />

                                <div class="box-body table-responsive" style="display:none">
                                    <asp:Repeater ID="leavereportrpt" runat="server">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <th>S.No.</th>
                                                        <th>Salesperson</th>
                                                        <th>Retailer</th>
                                                        <th>Address</th>
                                                        <th>Next Visit Date</th>
                                                        <th>Next Visit Time</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Eval("SrNo") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                                    <asp:Label ID="lblsno" runat="server" Visible="false" Text='<%# Eval("SrNo")%>'></asp:Label>
                                                    <%-- End --%>
                                                </td>
                                                <td><%#Eval("smname") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                                    <asp:Label ID="smnameLabel" runat="server" Visible="false" Text='<%# Eval("smname")%>'></asp:Label>
                                                    <%-- End --%>
                                                </td>
                                                <td><%#Eval("PartyName") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                                    <asp:Label ID="syncIdLabel" runat="server" Visible="false" Text='<%# Eval("PartyName")%>'></asp:Label>
                                                    <%-- End --%>
                                                </td>
                                                <td><%#Eval("address") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                                    <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Eval("address")%>'></asp:Label>
                                                    <%-- End --%>
                                                </td>
                                                <td><%#Eval("nextvisit","{0:dd/MMM/yyyy}") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                                    <asp:Label ID="nofdaysLabel" runat="server" Visible="false" Text='<%# Eval("nextvisit","{0:dd/MMM/yyyy}")%>'></asp:Label>
                                                    <%-- End --%>
                                                </td>
                                                <td><%#Eval("VisitTime") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                                    <asp:Label ID="fromDateLabel" runat="server" Visible="false" Text='<%# Eval("VisitTime")%>'></asp:Label>
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

