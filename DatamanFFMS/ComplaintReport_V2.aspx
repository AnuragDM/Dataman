<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ComplaintReport_V2.aspx.cs"
    Inherits="AstralFFMS.ComplaintReport_V2" %>

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
            $('[id*=DistListbox]').multiselect({
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
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>
    <style>
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .input-group .form-control {
            height: 34px;
        }

        .multiselect-container > li > a {
            white-space: normal;
        }

        select.form-control {
            padding: 6px 12px !important;
        }

        .multiselect-container > li {
            width: 100%;
        }

        input[type=checkbox], input[type=radio] {
            margin-right: 12px;
            margin-left: 12px;
        }


        .button1 {
            box-shadow: 0px 2px 4px 2px #888888;
            margin-left: 10px;
        }


        .table1 {
            font-weight: bold;
        }

        input[type=radio] {
            margin-right: 12px !important;
            margin-left: 7px !important;
        }

        h2 {
            font-size: 20px !important;
            font-weight: 600 !important;
            margin-left: 13px !important;
        }
    </style>
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
                includeSelectAllOption: true
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();
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
    <section class="content">
         <div id="spinner" runat="server" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
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
                                <%--<h3 class="box-title">Complaint Report</h3>--%>
                                <img id="img-Header" src="img/review.png" style="width: 47px; height: 47px;" />
                                <h2 class="box-title">
                                    <asp:Label ID="lblPageHeader" runat="server"></asp:Label></h2>

                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Complaint By:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlComplaint" runat="server" Width="100%" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlComplaint_SelectedIndexChanged">
                                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                                                <asp:ListItem Text="Distributor" Value="Distributor"></asp:ListItem>
                                                <asp:ListItem Text="Sales Person" Value="SalesPerson"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Status:</label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="100%" CssClass="form-control">
                                                <asp:ListItem Text="All" Value="A" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Pending" Value="P"></asp:ListItem>
                                                <asp:ListItem Text="Resolved" Value="R"></asp:ListItem>
                                                <asp:ListItem Text="WIP" Value="W"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12" id="divptype" runat="server" visible="false">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Party Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:ListBox ID="DistListbox" CssClass="form-control" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="divsp" runat="server" visible="false">
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <%--  <div class="form-group" hidden>
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:ListBox ID="ListBox1" CssClass="form-control" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                        </div>--%>
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <b>
                                                <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>
                                            </b>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Party Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                            <asp:DropDownList ID="ddlpartytype" OnSelectedIndexChanged="ddlpartytype_SelectedIndexChanged" AutoPostBack="true" Width="100%" CssClass="form-control" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label id="lblpartytypepersons" runat="server">Party Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                            <asp:DropDownList ID="ddlpartytypepersons" Width="100%" CssClass="form-control" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                </div>

                                <div class="row">
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Product Group:</label>
                                            <asp:ListBox ID="matGrpListBox" CssClass="form-control" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="matGrpListBox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Product:</label>
                                            <asp:ListBox ID="productListBox" CssClass="form-control" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Department:</label>
                                            <asp:ListBox ID="LstDepartment" CssClass="form-control" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="LstDepartment_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Complaint Nature:</label>
                                            <asp:ListBox ID="LstCompNature" CssClass="form-control" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <div class="box-footer">

                                <asp:Button type="button" ID="btnExport" runat="server" Text="Generate" ToolTip="Export To CSV" class="btn btn-primary button1" OnClick="btnExport_Click" />
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
        </div>
    </section>
</asp:Content>
