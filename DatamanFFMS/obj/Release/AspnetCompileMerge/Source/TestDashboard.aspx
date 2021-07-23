<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="TestDashBoard.aspx.cs" Inherits="AstralFFMS.TestDashBoard" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <script src="dist/js/demo.js" type="text/javascript"></script>
    <script src="dist/js/jquery.js"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <link href="dist/DataTables-1.10.16/css/jquery.dataTables.css" rel="stylesheet" />
    <link href="dist/DataTables-1.10.16/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="dist/DataTables-1.10.16/js/jquery.dataTables.js"></script>
    <script src="dist/DataTables-1.10.16/js/jquery.dataTables.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.13.0/moment.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datetimepicker/4.17.37/js/bootstrap-datetimepicker.min.js"></script>


    <script type="text/javascript">

        $.noConflict();  //Not to conflict with other scripts
        jQuery(document).ready(function ($) {
            $('#FromDate').datetimepicker({
                defaultDate: new Date(),
                format: 'DD-MMM-YYYY'
            });
            FillCount();
            FillProductiveCount();
            FillCharts();
            $("#FromDate").on("dp.change", function ($scope) {
                FillCount();
                FillProductiveCount();
                FillCharts();
            });


            $('#dvTotal').click(function (e) {
                window.open('DashboardTotalMember.aspx?Date=' + $('#FromDate').val(), '_blank');
            });

            $('#dvPresent').click(function (e) {
                window.open('DashboardPresentMember.aspx?Date=' + $('#FromDate').val(), '_blank');
            });

            $('#dvAbsent').click(function (e) {
                window.open('DashboardAbsentMember.aspx?Date=' + $('#FromDate').val(), '_blank');
            });

            $('#dvLeave').click(function (e) {
                window.open('DashboardTotalLeave.aspx?Date=' + $('#FromDate').val(), '_blank');
            });
        });

        //////////////////////////////////////////////

        function FillCount() {
            
            $('#pageloaddiv').show();
            $.ajax({
                type: "POST",
                url: "TestDashboard.aspx/FillMembers",
                data: "{FromDate:'" + $('#FromDate').val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    
                    var resdata = JSON.parse(response.d);
                    if (resdata != null) {
                        var totalmembers = parseInt(resdata[0].Person) + parseInt(resdata[1].Person) + parseInt(resdata[2].Person);
                        $('#<%=lblTotal.ClientID%>').text(totalmembers);
                        $('#<%=lblPresent.ClientID%>').text(resdata[0].Person);
                        $('#<%=lblAbsent.ClientID%>').text(resdata[1].Person);
                        $('#<%=lblLeave.ClientID%>').text(resdata[2].Person);
                        $('#pageloaddiv').hide();
                    }
                }
            });
        }


        function FillProductiveCount() {        
            $('#pageloaddiv').show();
            $.ajax({
                type: "POST",
                url: "TestDashboard.aspx/FillProductive",
                data: "{FromDate:'" + $('#FromDate').val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var resdata = JSON.parse(response.d);
                    if (resdata != null) {
                        $('#<%=lblProductivity.ClientID%>').text(resdata[0].Productivity);
                        $('#<%=lblVisited.ClientID%>').text(resdata[0].VisitedRetailer);
                        $('#<%=lblProductive.ClientID%>').text(resdata[0].ProductiveRetailer);
                        $('#<%=lblNewRetailer.ClientID%>').text(resdata[0].NewRetailer);
                        $('#pageloaddiv').hide();
                    }
                }
            });
        }


        function FillCharts() {
            $('#pageloaddiv').show();
            google.load("visualization", "1", { packages: ["corechart"], "callback": drawChart });
            google.setOnLoadCallback(drawChart);
            function drawChart() {
                $.ajax({
                    type: "POST",
                    url: "TestDashboard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#FromDate').val() + "',Type:'Primary'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var resdata = JSON.parse(response.d);
                        if (response != null) {
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'Name');
                            data.addColumn('number', 'Amount');
                            for (var i = 0; i < resdata.length; i++) {
                                data.addRows([[resdata[i].Name, resdata[i].Amount]]);
                            }
                            var options = {
                                title: '', chartArea: { width: "100%", height: "100%" }, tooltip: { trigger: 'selection' },
                                colors: ["#0d2713", "#8b9061", "#4cb274"]
                            };
                            var chart = new google.visualization.PieChart(document.getElementById('piechart'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    window.open('DashboardPrimary.aspx?Date=' + $('#FromDate').val() + '&Name='+ topping +'' , '_blank');
                                }
                            }
                            google.visualization.events.addListener(chart, 'select', selectHandler);
                            chart.draw(data, options);
                        }
                    }
                });
            }

            google.load("visualization", "1", { packages: ["corechart"], "callback": drawChart1 });
            google.setOnLoadCallback(drawChart1);
            function drawChart1() {
                $.ajax({
                    type: "POST",
                    url: "TestDashboard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#FromDate').val() + "',Type:'Secondary'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var resdata = JSON.parse(response.d);
                        if (response != null) {
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'Name');
                            data.addColumn('number', 'Amount');
                            for (var i = 0; i < resdata.length; i++) {
                                data.addRows([[resdata[i].Name, resdata[i].Amount]]);
                            }
                            var options = {
                                title: '', chartArea: { width: "100%", height: "100%" }, tooltip: { trigger: 'selection' },
                                colors: ["#0d2713", "#8b9061", "#4cb274"]
                            };
                            var chart = new google.visualization.PieChart(document.getElementById('piechart1'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    window.open('DashboardSecondary.aspx?Date=' + $('#FromDate').val() + '&Name=' + topping + '', '_blank');
                                }
                            }
                            google.visualization.events.addListener(chart, 'select', selectHandler);
                            chart.draw(data, options);
                        }
                    }
                });
            }

            google.load("visualization", "1", { packages: ["corechart"], "callback": drawChart2 });
            google.setOnLoadCallback(drawChart2);
            function drawChart2() {
                $.ajax({
                    type: "POST",
                    url: "TestDashboard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#FromDate').val() + "',Type:'UnApproved'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var resdata = JSON.parse(response.d);
                        if (response != null) {
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'Name');
                            data.addColumn('number', 'Amount');
                            for (var i = 0; i < resdata.length; i++) {
                                data.addRows([[resdata[i].Name, resdata[i].Amount]]);
                            }
                            var options = {
                                title: '', pieHole: 0.4, chartArea: { width: "100%", height: "100%" }, tooltip: { trigger: 'selection' },
                                colors: ["#0d2713", "#8b9061", "#4cb274", "#02a4cf", "#d78e5d", "#87bd16"]
                            };
                            var chart = new google.visualization.PieChart(document.getElementById('doughnutchart'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    if (topping == "DSR")
                                        window.open('DSRPendingList.aspx', '_blank');
                                    if (topping == "Tour")
                                        window.open('TourPlanApproval.aspx?Open=D', '_blank');
                                    if (topping == "BeatPlan")
                                        window.open('BeatPlanApproval.aspx?Open=D', '_blank');
                                    if (topping == "Leave")
                                        window.open('LeaveApproval.aspx?Open=D', '_blank');
                                }
                            }
                            google.visualization.events.addListener(chart, 'select', selectHandler);
                            chart.draw(data, options);
                        }
                    }
                });
            }

            google.load("visualization", "1", { packages: ["corechart"], "callback": drawChart3 });
            google.setOnLoadCallback(drawChart3);
            function drawChart3() {
                $.ajax({
                    type: "POST",
                    url: "TestDashboard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#FromDate').val() + "',Type:'Order'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var resdata = JSON.parse(response.d);
                        if (response != null) {
                            var data = new google.visualization.DataTable();
                            data.addColumn('number', 'workhour');
                            data.addColumn('number', 'OrdAmt');
                            for (var i = 0; i < resdata.length; i++) {
                                data.addRows([[resdata[i].workhour, resdata[i].OrdAmt]]);
                            }
                            var options = {
                                title: '', chartArea: { width: "80%", height: "80%" }, tooltip: { trigger: 'selection' },
                                colors: ["#0d2713", "#8b9061", "#4cb274", "#02a4cf", "#d78e5d", "#87bd16"]
                            };
                            var chart = new google.visualization.AreaChart(document.getElementById('areachart'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    //window.open('Charts/PrimaryChart/' + topping + '?FilterDate=' + $('#FromDate').val(), '_blank');
                                }
                            }
                            google.visualization.events.addListener(chart, 'select', selectHandler);
                            chart.draw(data, options);
                        }
                    }
                });
            }

            google.load("visualization", "1", { packages: ["corechart"], "callback": drawChart4 });
            google.setOnLoadCallback(drawChart4);
            function drawChart4() {
                $.ajax({
                    type: "POST",
                    url: "TestDashboard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#FromDate').val() + "',Type:'Intime'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var resdata = JSON.parse(response.d);
                        if (response != null) {
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'Name');
                            data.addColumn('number', 'InTime');
                            for (var i = 0; i < resdata.length; i++) {
                                data.addRows([[resdata[i].Name, resdata[i].InTime]]);
                            }
                            var options = {
                                title: '', chartArea: { width: "100%", height: "100%" }, tooltip: { trigger: 'selection' },
                                colors: ["#0d2713", "#8b9061", "#4cb274", "#02a4cf", "#d78e5d", "#87bd16"]
                            };
                            var chart = new google.visualization.PieChart(document.getElementById('piechart4'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    debugger
                                    window.open('DashboardInTime.aspx?Date=' + $('#FromDate').val() + '&Name=' + topping + '', '_blank');
                                }
                            }
                            google.visualization.events.addListener(chart, 'select', selectHandler);
                            chart.draw(data, options);
                        }
                    }
                });
            }

            google.load("visualization", "1", { packages: ["corechart"], "callback": drawChart5 });
            google.setOnLoadCallback(drawChart5);
            function drawChart5() {
                $.ajax({
                    type: "POST",
                    url: "TestDashboard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#FromDate').val() + "',Type:'NoSales'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var resdata = JSON.parse(response.d);
                        if (response != null) {
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'FVName');
                            data.addColumn('number', 'NoSalesCount');
                            for (var i = 0; i < resdata.length; i++) {
                                data.addRows([[resdata[i].FVName, resdata[i].NoSalesCount]]);
                            }
                            var options = {
                                title: '', chartArea: { width: "100%", height: "100%" }, tooltip: { trigger: 'selection' },
                                colors: ["#8b9061", "#4cb274", "#0d2713", "#02a4cf", "#d78e5d", "#87bd16"]
                            };
                            var chart = new google.visualization.PieChart(document.getElementById('piechart5'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    //window.open('Charts/PrimaryChart/' + topping + '?FilterDate=' + $('#FromDate').val(), '_blank');
                                }
                            }
                            google.visualization.events.addListener(chart, 'select', selectHandler);
                            chart.draw(data, options);
                        }
                    }
                });
            }

            google.load("visualization", "1", { packages: ["corechart"], "callback": drawChart6 });
            google.setOnLoadCallback(drawChart6);
            function drawChart6() {
                $.ajax({
                    type: "POST",
                    url: "TestDashboard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#FromDate').val() + "',Type:'CategorySales'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var resdata = JSON.parse(response.d);
                        if (response != null) {
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'itemname');
                            data.addColumn('number', 'Qty');
                            for (var i = 0; i < resdata.length; i++) {
                                data.addRows([[resdata[i].itemname, resdata[i].qty]]);
                            }
                            var options = {
                                title: '', chartArea: { width: "80%", height: "80%" }, bar: { groupWidth: '50%' }, tooltip: { trigger: 'selection' },
                                colors: ["#8b9061", "#4cb274", "#0d2713", "#02a4cf", "#d78e5d", "#87bd16"]
                            };
                            var chart = new google.visualization.ColumnChart(document.getElementById('columnchart1'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    //window.open('Charts/PrimaryChart/' + topping + '?FilterDate=' + $('#FromDate').val(), '_blank');
                                }
                            }
                            google.visualization.events.addListener(chart, 'select', selectHandler);
                            chart.draw(data, options);
                        }
                    }
                });
            }

            google.load("visualization", "1", { packages: ["corechart"], "callback": drawChart7 });
            google.setOnLoadCallback(drawChart7);
            function drawChart7() {
                $.ajax({
                    type: "POST",
                    url: "TestDashboard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#FromDate').val() + "',Type:'ASMSales'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var resdata = JSON.parse(response.d);
                        if (response != null) {
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'SMName');
                            data.addColumn('number', 'Qty');
                            for (var i = 0; i < resdata.length; i++) {
                                data.addRows([[resdata[i].SMName, resdata[i].Qty]]);
                            }
                            var options = {
                                title: '', chartArea: { width: "80%", height: "80%" }, bar: { groupWidth: '50%' }, tooltip: { trigger: 'selection' },
                                colors: ["#8b9061", "#4cb274", "#0d2713", "#02a4cf", "#d78e5d", "#87bd16"]
                            };
                            var chart = new google.visualization.ColumnChart(document.getElementById('columnchart2'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    //window.open('Charts/PrimaryChart/' + topping + '?FilterDate=' + $('#FromDate').val(), '_blank');
                                }
                            }
                            google.visualization.events.addListener(chart, 'select', selectHandler);
                            chart.draw(data, options);
                        }
                    }
                });
            }

            google.load("visualization", "1", { packages: ["corechart"], "callback": drawChart8 });
            google.setOnLoadCallback(drawChart8);
            function drawChart8() {
                $.ajax({
                    type: "POST",
                    url: "TestDashboard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#FromDate').val() + "',Type:'TopSKU'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var resdata = JSON.parse(response.d);
                        if (response != null) {
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'itemname');
                            data.addColumn('number', 'Qty');
                            for (var i = 0; i < resdata.length; i++) {
                                data.addRows([[resdata[i].itemname, resdata[i].Qty]]);
                            }
                            var options = {
                                title: '', chartArea: { width: "80%", height: "80%" }, bar: { groupWidth: '50%' }, tooltip: { trigger: 'selection' },
                                colors: ["#8b9061", "#4cb274", "#0d2713", "#02a4cf", "#d78e5d", "#87bd16"]
                            };
                            var chart = new google.visualization.ColumnChart(document.getElementById('columnchart3'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    //window.open('Charts/PrimaryChart/' + topping + '?FilterDate=' + $('#FromDate').val(), '_blank');
                                }
                            }
                            google.visualization.events.addListener(chart, 'select', selectHandler);
                            chart.draw(data, options);
                        }
                    }
                });
            }

            google.load("visualization", "1", { packages: ["corechart"], "callback": drawChart9 });
            google.setOnLoadCallback(drawChart9);
            function drawChart9() {
                $.ajax({
                    type: "POST",
                    url: "TestDashboard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#FromDate').val() + "',Type:'BottomSKU'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var resdata = JSON.parse(response.d);
                        if (response != null) {
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'itemname');
                            data.addColumn('number', 'Qty');
                            for (var i = 0; i < resdata.length; i++) {
                                data.addRows([[resdata[i].itemname, resdata[i].Qty]]);
                            }
                            var options = {
                                title: '', chartArea: { width: "80%", height: "80%" }, bar: { groupWidth: '50%' }, tooltip: { trigger: 'selection' },
                                colors: ["#8b9061", "#4cb274", "#0d2713", "#02a4cf", "#d78e5d", "#87bd16"]
                            };
                            var chart = new google.visualization.ColumnChart(document.getElementById('columnchart4'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    //window.open('Charts/PrimaryChart/' + topping + '?FilterDate=' + $('#FromDate').val(), '_blank');
                                }
                            }
                            google.visualization.events.addListener(chart, 'select', selectHandler);
                            chart.draw(data, options);
                        }
                    }
                });
            }

            google.load("visualization", "1", { packages: ["corechart"], "callback": drawChart10 });
            google.setOnLoadCallback(drawChart10);
            function drawChart10() {
                $.ajax({
                    type: "POST",
                    url: "TestDashboard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#FromDate').val() + "',Type:'TopSecondarySales'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var resdata = JSON.parse(response.d);
                        if (response != null) {
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'classname');
                            data.addColumn('number', 'Qty');
                            for (var i = 0; i < resdata.length; i++) {
                                data.addRows([[resdata[i].classname, resdata[i].Qty]]);
                            }
                            var options = {
                                title: '', chartArea: { width: "80%", height: "80%" }, bar: { groupWidth: '50%' }, tooltip: { trigger: 'selection' },
                                colors: ["#8b9061", "#4cb274", "#0d2713", "#02a4cf", "#d78e5d", "#87bd16"]
                            };
                            var chart = new google.visualization.ColumnChart(document.getElementById('columnchart5'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    //window.open('Charts/PrimaryChart/' + topping + '?FilterDate=' + $('#FromDate').val(), '_blank');
                                }
                            }
                            google.visualization.events.addListener(chart, 'select', selectHandler);
                            chart.draw(data, options);
                        }
                    }
                });
            }

            google.load("visualization", "1", { packages: ["corechart"], "callback": drawChart11 });
            google.setOnLoadCallback(drawChart11);
            function drawChart11() {
                $.ajax({
                    type: "POST",
                    url: "TestDashboard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#FromDate').val() + "',Type:'BottomSecondarySales'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var resdata = JSON.parse(response.d);
                        if (response != null) {
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'classname');
                            data.addColumn('number', 'Qty');
                            for (var i = 0; i < resdata.length; i++) {
                                data.addRows([[resdata[i].classname, resdata[i].Qty]]);
                            }
                            var options = {
                                title: '', chartArea: { width: "80%", height: "80%" }, bar: { groupWidth: '50%' }, tooltip: { trigger: 'selection' },
                                colors: ["#8b9061", "#4cb274", "#0d2713", "#02a4cf", "#d78e5d", "#87bd16"]
                            };
                            var chart = new google.visualization.ColumnChart(document.getElementById('columnchart6'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    //window.open('Charts/PrimaryChart/' + topping + '?FilterDate=' + $('#FromDate').val(), '_blank');
                                }
                            }
                            google.visualization.events.addListener(chart, 'select', selectHandler);
                            chart.draw(data, options);
                        }
                    }
                });
            }
            $('#pageloaddiv').hide();
        }

    </script>




    <section class="content">
        <div class="content" style="background-color: white; width: 100%">

            <div id="pageloaddiv" style="display: none; position: fixed; margin-left: 500px; margin-top: 200px; width: 100%; height: 100%; z-index: 100000;">
                <img src="img/loader.gif" />
            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-lg-3 col-xs-12" style="padding-top: 1em; padding-left: 3em">
                    <label for="datetimepicker2" class="lbl">For Date:</label>
                    <input type='text' class="form-control" id="FromDate" style="width: 50%" />

                    <div id="dv" style="margin-top: 10px;">
                    </div>
                </div>

            </div>
            <div class="row">

                <div class="col-lg-6 col-xs-6">
                    <!-- small box -->
                    <div class="small-box TotalEmp" style="background-color: #26C281">
                        <div class="inner" style="display: inline-block">
                            <div class="col-lg-3 col-md-3" id="dvTotal">
                                <span>
                                    <label id="lblTotal" runat="server" style="font-size: 32px"></label>
                                </span>
                                <label style="font-size: 14px; font-family: Arial">Total Members</label>
                            </div>
                            <div class="col-lg-3 col-md-3" id="dvPresent">
                                <span>
                                    <label id="lblPresent" runat="server" style="font-size: 32px"></label>
                                </span>
                                <label style="font-size: 14px; font-family: Arial">Present Members</label>
                            </div>
                            <div class="col-lg-3 col-md-3" id="dvAbsent">
                                <span>
                                    <label id="lblAbsent" runat="server" style="font-size: 32px"></label>
                                </span>
                                <label style="font-size: 14px; font-family: Arial">Absent Members</label>
                            </div>
                            <div class="col-lg-3 col-md-3" id="dvLeave">
                                <span>
                                    <label id="lblLeave" runat="server" style="font-size: 32px"></label>
                                </span>
                                <label style="font-size: 14px; font-family: Arial">Leave Members</label>
                            </div>
                        </div>
                        <div class="icon"><i class=" ion ion-person"></i></div>
                    </div>
                </div>

                <div class="col-lg-6 col-xs-6" id="">
                    <!-- small box -->
                    <div class="small-box TotalEmp" style="background-color: #5189d7">
                        <div class="inner" style="display: inline-block">
                            <div class="col-lg-3 col-md-3">
                                <span>
                                    <label id="lblProductivity" runat="server" style="font-size: 32px"></label></span><br />
                               <label style="font-size: 14px; font-family: Arial"> Productivity </label>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <span>
                                    <label id="lblVisited" runat="server" style="font-size: 32px"></label></span>
                                <label style="font-size: 14px; font-family: Arial">Visited Retailer</label>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <span>
                                    <label id="lblProductive" runat="server" style="font-size: 32px"></label></span>
                                <label style="font-size: 14px; font-family: Arial">Productive Retailer</label>
                            </div>
                            <div class="col-lg-3 col-md-3">
                                <span>
                                    <label id="lblNewRetailer" runat="server" style="font-size: 32px">00</label></span><br />
                                <label style="font-size: 14px; font-family: Arial">New Retailer</label>
                            </div>
                        </div>

                        <div class="icon">
                            <i class=" ion ion-person"></i>
                        </div>
                    </div>
                </div>

            </div>



            <div class="row">
                <div class="col-md-12 col-lg-12">
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading primaryHeading" id="Primary">Primary Sale
                                <div class="box-tools pull-right">
                                    <a href="Prim" data-toggle="collapse"><i class="fa fa-minus"></i></a>
                                </div>
                                </div>
                                <div id="Primarys" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px;overflow:hidden">
                                        <div id="piechart" style="height: 170px"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading secondaryHeading" id="Secondary">Secondary Sale
                                <div class="box-tools pull-right">
                                    <a href="Sec" data-toggle="collapse"><i class="fa fa-minus"></i></a>
                                </div>
                                </div>
                                <div id="secondarys" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height:200px;overflow:hidden">
                                        <div id="piechart1" style="height: 170px"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 col-lg-12">
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading UnApprovedHeading" id="">
                                    UnApproved Status
                                <div class="box-tools pull-right">
                                    <a href="UnApp" data-toggle="collapse"><i class="fa fa-minus"></i></a>
                                </div>
                                </div>
                                <div id="" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px;overflow:hidden">
                                        <div id="doughnutchart" style="height: 170px"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading OrderHeading" id="OrderDetail">
                                    Order Amount Detail
                                <div class="box-tools pull-right">
                                    <a href="Ord" data-toggle="collapse"><i class="fa fa-minus"></i></a>
                                </div>
                                </div>
                                <div id="" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px;overflow:hidden">
                                        <div id="areachart" style="height: 170px"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 col-lg-12">
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading primaryHeading" id="UnApproved">
                                    In-Time Statistics
                                <div class="box-tools pull-right">
                                    <a href="InTimwStats" data-toggle="collapse"><i class="fa fa-minus"></i></a>
                                </div>
                                </div>
                                <div id="UnApprovedchart" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px;overflow:hidden">
                                        <div id="piechart4" style="height: 170px"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading secondaryHeading" id="">
                                    No Sales Reason
                                <div class="box-tools pull-right">
                                    <a href="NoSale" data-toggle="collapse"><i class="fa fa-minus"></i></a>
                                </div>
                                </div>
                                <div id="Order" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px;overflow:hidden">
                                        <div id="piechart5" style="height: 170px"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 col-lg-12">
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading UnApprovedHeading" id="">
                                    Category Wise Sales
                                <div class="box-tools pull-right">
                                    <a href="categorysale" data-toggle="collapse"><i class="fa fa-minus"></i></a>
                                </div>
                                </div>
                                <div id="" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px;overflow:hidden">
                                        <div id="columnchart1" style="height: 170px"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading OrderHeading" id="">
                                    ASM Wise Sales
                                <div class="box-tools pull-right">
                                    <a href="ASMSale" data-toggle="collapse"><i class="fa fa-minus"></i></a>
                                </div>
                                </div>
                                <div id="" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px;overflow:hidden">
                                        <div id="columnchart2" style="height: 170px"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 col-lg-12">
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading primaryHeading" id="">
                                    Top 5 SKU
                                <div class="box-tools pull-right">
                                    <a href="TopSku" data-toggle="collapse"><i class="fa fa-minus"></i></a>
                                </div>
                                </div>
                                <div id="" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px;overflow:hidden">
                                        <div id="columnchart3" style="height: 170px"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading secondaryHeading" id="">
                                    Bottom 5 SKU
                                <div class="box-tools pull-right">
                                    <a href="bottomsku" data-toggle="collapse"><i class="fa fa-minus"></i></a>
                                </div>
                                </div>
                                <div id="" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px;overflow:hidden">
                                        <div id="columnchart4" style="height: 170px"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 col-lg-12">
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading UnApprovedHeading" id="">
                                    Top 5 Secondary Sale
                                <div class="box-tools pull-right">
                                    <a href="topsecsales" data-toggle="collapse"><i class="fa fa-minus"></i></a>
                                </div>
                                </div>
                                <div id="" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px;overflow:hidden">
                                        <div id="columnchart5" style="height: 170px"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading OrderHeading" id="">
                                    Bottom 5 Secondary Sale
                                <div class="box-tools pull-right">
                                    <a href="bottomsecsales" data-toggle="collapse"><i class="fa fa-minus"></i></a>
                                </div>
                                </div>
                                <div id="" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px;overflow:hidden">
                                        <div id="columnchart6" style="height: 170px"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>




    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(function () {
            //$("#example1").DataTable();

        });

        function showspinner() {

            $("#spinner").show();

        };
        function hidespinner() {

            $("#spinner").hide();

        };

    </script>
    <style>
        .primaryHeading {
            background-color: rgba(243,156,18,0.1);
        }

        .secondaryHeading {
            background-color: rgba(0,166,90,0.1);
        }

        .panel-heading.primaryHeading {
            background-color: rgb(0,166,90);
            color: white;
            font-size: 20px;
        }

        .panel-heading.secondaryHeading {
            background-color: rgba(243, 156, 18,0.8);
            border-color: black;
            color: white;
            font-size: 20px;
        }

        .panel-heading.UnApprovedHeading {
            background-color: rgba(0,192,239,1.0);
            border-color: black;
            color: white;
            font-size: 20px;
        }

        .panel-heading.OrderHeading {
            background-color: rgb(124, 12, 78);
            border-color: black;
            color: white;
            font-size: 20px;
        }

        .TotalEmp {
            background-color: rgb(149, 176, 219);
            /*border-style: solid;*/
            border-color: #9D2933;
            color: white;
        }

        .Present {
            background-color: rgba(0,141,76,0.3);
            color: white;
            /*border-style: solid;*/
            border-color: #26C281;
        }

        .Absent {
            background-color: rgba(211, 55, 36, 0.4);
            /*border-style: solid;*/
            border-color: #2980b9;
            color: white;
        }

        .Leave {
            background-color: rgba(255, 119, 1, 0.5);
            color: white;
            /*border-style: solid;*/
            border-color: #F3C13A;
        }


        .fa-minus::before {
            color: white;
            content: "";
        }
    </style>
    <style>
        /*.small-box > .small-box-footer {
    background: rgba(0, 0, 0, 0.1) none repeat scroll 0 0;
    color: rgba(255, 255, 255, 0.8);
    display: block;
    margin-top: 61px;
    padding: 3px 0;
    position: relative;
    text-align: center;
    text-decoration: none;
    z-index: 10;
}*/
        .inner > p {
            font-size: 27px;
            /*text-align: center;*/
        }

        .inner > span {
            font-size: 27px;
            /*padding-left: 124px;*/
        }

        .headerbottom {
            margin-bottom: 10px;
        }
    </style>
</asp:Content>
