<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="LeaveReport.aspx.cs" Inherits="AstralFFMS.LeaveReport" %>

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
            $("#example1").DataTable();
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
            width: 212px;
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
                                <%--<h3 class="box-title">Leave Report</h3>--%>
                                <h3 class="box-title">
                                    <asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                    <%-- <div class="col-md-3 col-sm-6 col-xs-7" hidden>
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:ListBox ID="ListBox1" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                    </div>--%>
                                    <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Status:</label>
                                            <asp:DropDownList ID="ddlLeavStatus" Width="100%"
                                                CssClass="form-control" runat="server">
                                                <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                <asp:ListItem Value="Approve" Text="Approved"></asp:ListItem>
                                                <asp:ListItem Value="Pending" Text="Pending"></asp:ListItem>
                                                <asp:ListItem Value="Reject" Text="Rejected"></asp:ListItem>
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
                                    <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                </div>



                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary"
                                            OnClick="btnGo_Click" />
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
                                                        <th>Name</th>
                                                        <th>Sync Id</th>
                                                        <th>For Days</th>
                                                        <th>From Date</th>
                                                        <th>To Date</th>
                                                        <th>Reason</th>
                                                        <th>Status</th>
                                                        <th>Approved By</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Eval("SMName") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                                    <asp:Label ID="smnameLabel" runat="server" Visible="false" Text='<%# Eval("SMName")%>'></asp:Label>
                                                    <%-- End --%>
                                                </td>
                                                <td><%#Eval("SyncId") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                                    <asp:Label ID="syncIdLabel" runat="server" Visible="false" Text='<%# Eval("SyncId")%>'></asp:Label>
                                                    <%-- End --%>
                                                </td>
                                                <td><%#Eval("NoOfDays") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                                    <asp:Label ID="nofdaysLabel" runat="server" Visible="false" Text='<%# Eval("NoOfDays")%>'></asp:Label>
                                                    <%-- End --%>
                                                </td>
                                                <td><%#Eval("FromDate","{0:dd/MMM/yyyy}") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                                    <asp:Label ID="fromDateLabel" runat="server" Visible="false" Text='<%# Eval("FromDate","{0:dd/MMM/yyyy}")%>'></asp:Label>
                                                    <%-- End --%>
                                                </td>
                                                <td><%#Eval("ToDate","{0:dd/MMM/yyyy}") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                                    <asp:Label ID="todateLabel" runat="server" Visible="false" Text='<%# Eval("ToDate","{0:dd/MMM/yyyy}")%>'></asp:Label>
                                                    <%-- End --%>
                                                </td>
                                                <td><%#Eval("Reason") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                                    <asp:Label ID="reasonLabel" runat="server" Visible="false" Text='<%# Eval("Reason")%>'></asp:Label>
                                                    <%-- End --%>
                                                </td>
                                                <td><%#Eval("AppStatus") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                                    <asp:Label ID="appstatusLabel" runat="server" Visible="false" Text='<%# Eval("AppStatus")%>'></asp:Label>
                                                    <%-- End --%>
                                                </td>
                                                <td><%#Eval("AppByName") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                                    <asp:Label ID="appbynameLabel" runat="server" Visible="false" Text='<%# Eval("AppByName")%>'></asp:Label>
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
