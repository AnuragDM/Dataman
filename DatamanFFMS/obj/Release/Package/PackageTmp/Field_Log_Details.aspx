<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Field_Log_Details.aspx.cs" Inherits="AstralFFMS.Field_Log_Details" %>--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Field_Log_Details.aspx.cs" Inherits="AstralFFMS.Field_Log_Details" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--   <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <link type="text/css" rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
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
    <%--<script type="text/javascript">
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
    </script>--%>
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
        //$(function () {
        //    $(".select2").select2();
        //});
    </script>
    <%-- <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();
        });
    </script>--%>
    <style type="text/css">
        .ShortDesc1 {
            display: block;
            width: 150px !important;
            overflow: hidden;
        }

        .ShortDesc {
            display: block;
            width: 150px !important;
            overflow: hidden;
        }

        .aVis {
            display: none;
        }

        .GridPager td {
            padding: 0 !important;
        }

        .GridPager a {
            display: block;
            height: 20px;
            width: 15px;
            background-color: #3c8dbc;
            color: #fff;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
            padding: 0px;
        }

        .GridPager span {
            display: block;
            height: 20px;
            width: 15px;
            background-color: #fff;
            color: #3c8dbc;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        .input-group .form-control {
            height: 34px;
        }

        @media (max-width: 600px) {
            .ui-dialog.ui-widget.ui-widget-content.ui-corner-all.ui-front.ui-draggable.ui-resizable {
                width: 100% !important;
            }
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
    </script>
    <script type="text/javascript">
        function GetReport() {

            var selectedvalue = [];
            $("#<%=trview.ClientID %> :checked").each(function () {
                 selectedvalue.push($(this).val());
             });
             if (selectedvalue == "") {
                 errormessage("Please Select Sales Person");
                 return false;
             }
         }
    </script>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            display: table;
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
    <%--<script type="text/javascript">
        function DoNav(lvrQId) {
            if (lvrQId != "") {
                var url = "DSRReport.aspx?" + lvrQId;
                window.location.href = url;
            }
        }
    </script>--%>

    <%--<script type="text/javascript">

        function SetTarget() {

            document.forms[0].target = "_blank";

        }

    </script>--%>

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
                    <div class="col-md-12">
                        <div class="box-header with-border">
                            <%-- <h3 class="box-title">Daily Working Summary L1</h3>--%>
                            <h3 class="box-title">
                                <asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-md-12">
                                <div class="row">
                                    <div id="divtrview" class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>
                                            <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="frmTextBox" class="form-control" runat="server"
                                                Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="frmTextBox_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server"
                                                BehaviorID="frmTextBox_CalendarExtender"
                                                TargetControlID="frmTextBox"></ajaxToolkit:CalendarExtender>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="toTextBox" class="form-control" runat="server"
                                                Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="toTextBox_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server"
                                                BehaviorID="toTextBox_CalendarExtender"
                                                TargetControlID="toTextBox"></ajaxToolkit:CalendarExtender>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="box-footer">
                                <asp:Button type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" 
                                    OnClick="btnGo_Click" />
                                <asp:Button type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To CSV" class="btn btn-primary"
                                    OnClick="btnExport_Click" />

                            </div>
                        </div>





                        <div id="rptmain" runat="server" style="display: none;">
                            <div class="row">
                                <div class="col-xs-12">


                                    <div class="box-body table-responsive">
                                        <%--<label for="workingdays">Total DSR Count</label>--%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblTotalWorkingdays" runat="server" Font-Bold="True"></asp:Label>
                                        <asp:Repeater ID="rpt" runat="server">
                                            <HeaderTemplate>
                                                <table id="example1" class="table table-bordered table-striped">
                                                    <thead>
                                                        <tr>
                                                            
                                                            <th>SMID</th>
                                                            <th>Sales Person</th>
                                                            <th>Visit ID</th>
                                                            <th>Visit Date</th>
                                                            <th>Status</th>
                                                            <th>Log Date Time</th>
                                                        </tr>
                                                    </thead>
                                                    <tfoot>
                                                    </tfoot>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    
                                                    <td><%# Eval("SMID")%></td>
                                                    <td><%# Eval("SalePerson")%></td>
                                                    <td><%# Eval("VisitId")%></td>
                                                    <td><%# System.Convert.ToDateTime(Eval("VisDate")).ToString("dd/MMM/yyyy")%></td>
                                                    <td><%# Eval("StatusMaster")%></td>
                                                    <td><%# System.Convert.ToDateTime(Eval("CurrDte")).ToString("dd/MMM/yyyy HH:mm:ss")%></td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>     </table>   
                                       
                                            </FooterTemplate>

                                        </asp:Repeater>

                                    </div>
                                </div>
                            </div>

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
            $("#example1").DataTable();

        });
    </script>
</asp:Content>
