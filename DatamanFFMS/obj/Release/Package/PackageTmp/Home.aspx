<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Home.aspx.cs" Inherits="AstralFFMS.Home" %>

<%@ Register Src="~/DistributorUC.ascx" TagPrefix="uc1" TagName="DistributorUC" %>
<%@ Register Src="~/SalesPersonL1UC.ascx" TagPrefix="uc1" TagName="SalesPersonL1UC" %>

<%@ Register Src="~/SalesPersonL2UC.ascx" TagPrefix="uc1" TagName="SalesPersonL2UC" %>
<%@ Register Src="~/SalesPersonL3UC.ascx" TagPrefix="uc1" TagName="SalesPersonL3UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <head>
        <meta charset="UTF-8">
        <title>Home</title>
        <!-- Tell the browser to be responsive to screen width -->
        <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
        <!-- Bootstrap 3.3.4 -->
        <link href="Content/bootstrap.css" rel="stylesheet" />
        <link href="Content/bootstrap.min.css" rel="stylesheet" />
        <script src="plugins/jQuery/jQuery-2.1.4.min.js"></script>
        <script src="dist/js/bootstrap.min.js"></script>
        <link href="plugins/datatables/dataTables.bootstrap.css" rel="stylesheet" />

        <!-- Font Awesome Icons -->
        <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
        <!-- Ionicons -->
        <link href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" rel="stylesheet" type="text/css" />


        <!-- Theme style -->
        <link href="dist/css/AdminLTE.css" rel="stylesheet" />
        <!-- AdminLTE Skins. Choose a skin from the css/skins
         folder instead of downloading all of them to reduce the load. -->
        <link href="dist/css/skins/_all-skins.min.css" rel="stylesheet" type="text/css" />

        <!-- jQuery 2.1.4 -->
        <script src="plugins/jQuery/jQuery-2.1.4.min.js" type="text/javascript"></script>
        <script src="plugins/datatables/jquery.dataTables.min.js"></script>

        <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
        <!-- SlimScroll -->
        <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>

        <link href="plugins/multiselect.css" rel="stylesheet" />
        <script src="plugins/multiselect.js"></script>

        <%-- Added--%>
        <link href="jqwidgets/styles/jqx.base.css" rel="stylesheet" />
        <script src="jqwidgets/jqxcore.js" type="text/javascript"></script>
        <script type="text/javascript" src="jqwidgets/jqxnotification.js"></script>
        <%-- End--%>

        <script type="text/javascript">
            $(function () {
                $(".rpttable").DataTable();
                $(".invrpttable").DataTable({
                    "order": [[1, "desc"]]
                });
                $(".porderrpttable").DataTable({
                    "order": [[1, "desc"]]
                });
                $(".Tourplanrpttable").DataTable({
                    "order": [[1, "desc"]]
                });
                $(".rpttable1").DataTable();
                $(".rpttableLeave").DataTable();
                $(".dsrTable").DataTable();
            });
        </script>
        <script type="text/javascript">
            $(function () {
                $('[id*=ContentPlaceHolder1_SalesPersonL2UC_salespersonListBox]').multiselect({
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
            $(function () {
                $('[id*=ContentPlaceHolder1_SalesPersonL2UC_LstSalesperson]').multiselect({
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
            $(function () {
                $('[id*=ContentPlaceHolder1_SalesPersonL3UC_LstSalesperson]').multiselect({
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
            $(function () {
                $('[id*=ContentPlaceHolder1_SalesPersonL3UC_salespersonListBox]').multiselect({
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
            var V1 = "";
            function errormessage(V1) {
                $("#messageNotification").jqxNotification({
                    width: 300, position: "top-right", opacity: 2,
                    autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "error"
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
                    autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "success"
                });
                $('#<%=lblmasg.ClientID %>').html(V);
                $("#messageNotification").jqxNotification("open");

            }
        </script>
        <style type="text/css">
            .quicklink-cont1 {
                background: #f3f3f3 none repeat scroll 0 0;
                border-radius: 50px;
                height: 30px;
                min-width: 90%;
                border-width: 0px;
                color: #444;
                white-space: normal;
                margin-left: 4px;
                margin-bottom: 15px;
            }
        </style>
        <!-- Bootstrap 3.3.2 JS -->

        <%--   <script src="Scripts/bootstrap.min.js"></script>--%>
        <!-- FastClick -->

        <script src="plugins/fastclick/fastclick.min.js" type="text/javascript"></script>
        <!-- AdminLTE App -->
        <script src="dist/js/app.min.js" type="text/javascript"></script>
        <!-- AdminLTE for demo purposes -->
        <script src="dist/js/demo.js" type="text/javascript"></script>



    </head>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="messageNotification">
        <asp:Label ID="lblmasg" runat="server"></asp:Label>
    </div>
    <uc1:DistributorUC runat="server" ID="DistributorUC" style="display:none"/>
    <uc1:SalesPersonL1UC runat="server" ID="SalesPersonL1UC" style="display:none" />
    <uc1:SalesPersonL2UC runat="server" ID="SalesPersonL2UC"  style="display:none"/>
    <uc1:SalesPersonL3UC runat="server" ID="SalesPersonL3UC"  style="display:none"/>
</asp:Content>
