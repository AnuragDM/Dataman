<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DailyDashboard.aspx.cs" Inherits="AstralFFMS.DailyDashBoard"  EnableEventValidation="false" %>

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
    <link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datetimepicker/4.17.47/css/bootstrap-datetimepicker.css" rel="stylesheet" />
    <%--<link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datetimepicker/4.17.47/css/bootstrap-datetimepicker-standalone.css" rel="stylesheet" />--%>
    <%--<link href="Content/ajaxcalendar.css" rel="stylesheet" />--%>    
    <script type="text/javascript">

        $.noConflict();  //Not to conflict with other scripts
        jQuery(document).ready(function ($) {

            //var date = new Date();
            //date.setDate(date.getDate());
            //date.format('dd/MMM/yyyy');
            //$(".datepicker").datetimepicker({ defaultDate: new Date(), format: 'DD/MMM/YYYY', autoclose: true });
            //$(".datepicker").datepicker('setDate', date);
            //$("#FromDatee").datepicker('setDate', date);
            //$("#ToDatee").datepicker('setDate', date);
            //$("#ContentPlaceHolder1_FromDate").val($("#FromDatee").val());
            //$("#ContentPlaceHolder1_ToDate").val($("#ToDatee").val());

            var date = $("#ContentPlaceHolder1_FromDate").val();
            if (date == "") {
                $('#FromDatee').datetimepicker({
                    defaultDate: new Date(),
                    format: 'DD/MMM/YYYY'
                });
            }

            if (date == "") {
                $("#ContentPlaceHolder1_FromDate").val($("#FromDatee").val());
            }
            //else if (date != "") {
            //}


            FillCount();
            FillProductiveCount();
            FillProductCount();
            FillCharts();
            $("#ContentPlaceHolder1_FromDate").change(function ($scope) {

                $('#FromDatee').val($("#ContentPlaceHolder1_FromDate").val());
                FillCount();
                FillProductiveCount();
                FillProductCount();
                FillCharts();

                //$("#FromDatee").datepicker('setDate', date);
                //$("#ToDatee").datepicker('setDate', date);
                //$("#ContentPlaceHolder1_FromDate").val($("#FromDatee").val());
                //$("#ContentPlaceHolder1_ToDate").val($("#ToDatee").val());
            });


            $('#dvTotal').click(function (e) {
                window.open('DashboardTotalMember.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val(), '_blank');
            });

            $('#dvPresent').click(function (e) {
                window.open('DashboardPresentMember.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val(), '_blank');
            });

            $('#dvAbsent').click(function (e) {
                window.open('DashboardAbsentMember.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val(), '_blank');
            });

            $('#dvLeave').click(function (e) {
                window.open('DashboardTotalLeave.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val(), '_blank');
            });

            $('#dvisited').click(function (e) {
                window.open('VisitedRetailer.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val(), '_blank');
            });
            $('#dvproductiveretailer').click(function (e) {
                window.open('ProductiveRetailer.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val(), '_blank');
            });
            $('#dvnewretailer').click(function (e) {
                window.open('NewRetailerReport.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val(), '_blank');
            });
            $('#dvvisiteddist').click(function (e) {
                window.open('VisitedDistributor.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val(), '_blank');
            });
            $('#dvproductivedist').click(function (e) {
                window.open('ProductiveDistributor.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val(), '_blank');
            });
            //$('#FromDate').keypress(function (e) {
            //    var regex = new RegExp("^[a-zA-Z0-9-]+$");
            //    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
            //    if (regex.test(str)) {
            //        return true;
            //    }

            //    e.preventDefault();
            //    return false;
            //});
        });

        //////////////////////////////////////////////

        function FillCount() {

            $('#pageloaddiv').show();
            $.ajax({
                type: "POST",
                url: "DailyDashBoard.aspx/FillMembers",
                data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    var resdata = JSON.parse(response.d);
                    if (resdata != null) {
                        var totalmembers = parseInt(resdata[0].Person) + parseInt(resdata[1].Person) + parseInt(resdata[2].Person);
                        $('#<%=lblTotal.ClientID%>').text(resdata[0].Person);
                        $('#<%=lblPresent.ClientID%>').text(resdata[1].Person);
                        $('#<%=lblAbsent.ClientID%>').text(resdata[2].Person);
                        $('#<%=lblLeave.ClientID%>').text(resdata[3].Person);
                     <%--   $('#<%=lblInActive.ClientID%>').text(resdata[3].Person);--%>
                        $('#pageloaddiv').hide();
                    }
                }
            });
        }


        function FillProductiveCount() {
            $('#pageloaddiv').show();
            $.ajax({
                type: "POST",
                url: "DailyDashBoard.aspx/FillProductive",
                data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "'}",
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

        function FillProductCount() {
            $('#pageloaddiv').show();
            $.ajax({
                type: "POST",
                url: "DailyDashBoard.aspx/FillProduct",
                data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var resdata = JSON.parse(response.d);
                    if (resdata != null) {
                        $('#<%=lblProduct.ClientID%>').text(resdata[0].Productivity);
                        $('#<%=lblVisitedDist.ClientID%>').text(resdata[0].VisitedDistributor);
                        $('#<%=lblProductiveDist.ClientID%>').text(resdata[0].ProductiveDistributor);
                    <%--  $('#<%=lblNew.ClientID%>').text(resdata[3].NewRetailer);--%>
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
                    url: "DailyDashBoard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "',Type:'Primary'}",
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
                                //tooltip: {isHtml: true},
                                title: '', chartArea: { width: "100%", height: "100%" }, tooltip: { trigger: 'hover', position: 'outside' },
                                colors: ["#02a4cf", "#8b9061", "#4cb274"]
                            };
                            var chart = new google.visualization.PieChart(document.getElementById('piechart'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    window.open('DashboardPrimary.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val() + '&Name=' + topping + '', '_blank');
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
                    url: "DailyDashBoard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "',Type:'Secondary'}",
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
                                title: '', chartArea: { width: "100%", height: "100%" }, tooltip: { trigger: 'hover' },
                                //colors: ["#0d2713", "#8b9061", "#4cb274"]
                                colors: ["#1e88e5", "#d50000", "#ffca28", "#87bd16"]
                            };
                            var chart = new google.visualization.PieChart(document.getElementById('piechart1'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    window.open('DashboardSecondary.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val() + '&Name=' + topping + '', '_blank');
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
                    url: "DailyDashBoard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "',Type:'UnApproved'}",
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
                                title: '', pieHole: 0.4, chartArea: { width: "100%", height: "100%" }, tooltip: { trigger: 'hover' },
                                colors: ["#FF7F50", "#4682B4", "#008080", "#BC8F8F", "#d78e5d", "#87bd16"]
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
                    url: "DailyDashBoard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "',Type:'Order'}",
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
                                title: '', chartArea: { width: "80%", height: "80%" }, tooltip: { trigger: 'hover' },
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
                    url: "DailyDashBoard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "',Type:'Intime'}",
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
                                title: '', chartArea: { width: "80%", height: "80%" }, tooltip: { trigger: 'hover' },
                                colors: ["#303f9f", "#ef9a9a", "#ce93d8", "#02a4cf", "#d78e5d", "#87bd16"]
                            };
                            var chart = new google.visualization.PieChart(document.getElementById('piechart4'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    window.open('DashboardInTime.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val() + '&Name=' + topping + '', '_blank');
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
                    url: "DailyDashBoard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "',Type:'NoSales'}",
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
                                title: '', chartArea: { width: "100%", height: "100%" }, tooltip: { trigger: 'hover' },
                                colors:["#87CEFA", "#DC143C", "#000080", "#87bd16", "#40E0D0", "#FF4500"]
                            };
                            var chart = new google.visualization.PieChart(document.getElementById('piechart5'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    window.open('DashboardNoSalesReason.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val() + '&Name=' + topping + '', '_blank');
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
                    url: "DailyDashBoard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "',Type:'ProductClassWise'}",
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
                                title: '', chartArea: { width: "80%", height: "70%" }, bar: { groupWidth: '25%' }, tooltip: { trigger: 'hover' },
                                colors: ["#5574A6", "#8b9061", "#0d2713", "#02a4cf", "#d78e5d", "#87bd16"], xAxis: { rotation: '-45' }, yAxis: { text: 'Population (millions)' }
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


            google.load("visualization", "1", { packages: ["corechart"], "callback": drawChart12 });
            google.setOnLoadCallback(drawChart12);
            function drawChart12() {
                $.ajax({
                    type: "POST",
                    url: "DailyDashBoard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "',Type:'ProductSegmentWise'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var resdata = JSON.parse(response.d);
                        if (response != null) {
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'segmentname');
                            data.addColumn('number', 'Qty');
                            for (var i = 0; i < resdata.length; i++) {
                                data.addRows([[resdata[i].segmentname, resdata[i].Qty]]);
                            }
                            var options = {
                                title: '', chartArea: { width: "80%", height: "70%" }, bar: { groupWidth: '25%' }, tooltip: { trigger: 'hover' },
                                colors: ["#DD4477", "#4cb274", "#0d2713", "#02a4cf", "#d78e5d", "#87bd16"], xAxis: { rotation: '-45' }, yAxis: { text: 'Population (millions)' }
                            };
                            var chart = new google.visualization.ColumnChart(document.getElementById('columnchart7'));
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
                    url: "DailyDashBoard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "',Type:'ASMSales'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var resdata = JSON.parse(response.d);
                        if (response != null) {
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'SMName');
                            data.addColumn('number', 'Qty');
                            for (var i = 0; i < resdata.length; i++) {
                                data.addRows([[resdata[i].smname, resdata[i].qty]]);
                            }
                            var options = {
                                title: '', chartArea: { width: "80%", height: "70%" }, bar: { groupWidth: '25%' }, tooltip: { trigger: 'hover' },
                                colors: ["#FA8072", "#FA8072", "#FA8072", "#FA8072", "#FA8072", "#FA8072"], xAxis: { rotation: '-45' }, yAxis: { text: 'Population (millions)' }
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
                    url: "DailyDashBoard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "',Type:'TopSKU'}",
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

                                title: '', chartArea: { width: "80%", height: "70%" }, bar: { groupWidth: '25%'}, tooltip: { trigger: 'hover' },

                                colors: ["#E67300", "#4cb274", "#0d2713", "#02a4cf", "#d78e5d", "#87bd16"], xAxis: { rotation: '-45' }, yAxis: { text: 'Population (millions)' }
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
                    url: "DailyDashBoard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "',Type:'BottomSKU'}",
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
                                title: '', chartArea: { width: "80%", height: "70%" }, bar: { groupWidth: '25%' }, tooltip: { trigger: 'hover' },
                                colors: ["#EE82EE", "#4cb274", "#0d2713", "#02a4cf", "#d78e5d", "#EE82EE"], xAxis: { rotation: '-45' }, yAxis: { text: 'Population (millions)' }
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
                    url: "DailyDashBoard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "',Type:'TopSecondarySales'}",
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
                                title: '', chartArea: { width: "80%", height: "70%" }, bar: { groupWidth: '25%' }, tooltip: { trigger: 'hover' },
                                colors: ["1E90FF", "#4cb274", "#0d2713", "#02a4cf", "#d78e5d", "1E90FF"], xAxis: { rotation: '-45' }, yAxis: { text: 'Population (millions)' }
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
                    url: "DailyDashBoard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "',Type:'BottomSecondarySales'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var resdata = JSON.parse(response.d);
                        if (response != null) {
                            debugger
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'itemname');
                            data.addColumn('number', 'Qty');
                            for (var i = 0; i < resdata.length; i++) {
                                data.addRows([[resdata[i].itemname, resdata[i].qty]]);
                            }
                            var options = {
                                title: '', chartArea: { width: "80%", height: "78%" }, bar: { groupWidth: '25%' }, tooltip: { trigger: 'hover' },
                                colors: ["#2F4F4F", "#2F4F4F", "#2F4F4F", "#2F4F4F", "#2F4F4F", "#2F4F4F"], xAxis: { rotation: '-45' }, yAxis: { text: 'Population (millions)' }
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


        function Validate(event) {
            var regex = new RegExp("^[0-9-/?]");
            var key = String.fromCharCode(event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        }
        function ASMWiseSales(value) {                
            window.open('ASMWiseSale.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val() + '&type=' + value + '', '_blank');
        }
        function ProductClassWiseSales(value) {
            window.open('TopProductClass.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val() + '&type=' + value + '', '_blank');
        }
        function ProductSegmentWiseSales(value) {
            window.open('TopProductSegment.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val() + '&type=' + value + '', '_blank');
        }
        function SKU(value) {
            window.open('TopProductReport.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val() + '&type=' + value + '', '_blank');

        }
        function BottomSKU(value) {
            window.open('BottomProduct.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val() + '&type=' + value + '', '_blank');

        }
        function ProductGroupWiseSales(value) {
            window.open('TopProductGroup.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val() + '&type=' + value + '', '_blank');
        }
        function BottomProductGroupWiseSales(value) {
            window.open('BottomProductGroup.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val() + '&type=' + value + '', '_blank');
        }
    </script>
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>

    
    <section class="content">
        <div class="content" style="background-color: white; width: 100%">

            <div id="pageloaddiv" style="display: none; position: fixed; margin-left: 500px; margin-top: 200px; width: 100%; height: 100%; z-index: 100000;">
                <img src="img/loader.gif" />
            </div>


            <input type='text' id="FromDatee" class="form-control datepicker" maxlength="20" style="display: none" readonly="readonly" />
            <%--<input type='text' id="ToDatee" class="form-control datepicker" maxlength="20" style="display: none" readonly="readonly" />--%>

            <div class="row" style="">
                <div class="col-lg-3 col-xs-12" style="padding-top: 1em; padding-left: 3em">              
                    <label for="datetimepicker2" class="lbl">Date:</label>
                    <asp:TextBox ID="FromDate" runat="server" CssClass="form-control" onChange="showspinner();" AutoPostBack="true" Style="background-color: white;" onkeypress="return Validate(event);"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender5" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender5" TargetControlID="FromDate"></ajaxToolkit:CalendarExtender>
                    <%-- <input type='text' class="form-control" id="FromDate" style="width: 50%" onkeypress="return Validate(event);" />--%>

                    <div id="dv" style="margin-top: 10px;">
                    </div>
                </div>

            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-lg-12 col-xs-12" style="padding-left: 3em; font-size:large; font-size:21px; font-weight: bolder; text-align: center; text-transform: uppercase;">
                    <span style="color:#b71c1c">D</span>aily <span style="color:#b71c1c">D</span>ashboard
                </div>                

            </div>

            <hr>

          
             <div class="col-md-12 row">
                <div class="col-lg-4 col-xs-12">
                    <!-- small box -->
                    <div class="small-box TotalEmp" style="background-image: linear-gradient(-90deg,#40E0D0, #3c8dbc); border-radius: 15px;">
                        <div class="inner" style="display: inline-block">
                            <div class="col-lg-2 col-md-2" id="dvTotal">
                                <span>
                                    <label id="lblTotal" runat="server" style="font-size: 25px"></label>
                                </span>
                                <label style="font-size: 12px; font-family: Arial">Total Members</label>
                            </div>
                            <div class="col-lg-3 col-md-3" id="dvPresent">
                                <span>
                                    <label id="lblPresent" runat="server" style="font-size: 25px"></label>
                                </span>
                                <label style="font-size: 12px; font-family: Arial">Present Members</label>
                            </div>
                            <div class="col-lg-2 col-md-2" id="dvAbsent">
                                <span>
                                    <label id="lblAbsent" runat="server" style="font-size: 25px"></label>
                                </span>
                                <label style="font-size: 12px; font-family: Arial">Absent Members</label>
                            </div>
                            <div class="col-lg-3 col-md-3" id="dvLeave">
                                <span>
                                    <label id="lblLeave" runat="server" style="font-size: 25px"></label>
                                </span>
                                <label style="font-size: 12px; font-family: Arial">Leave Members</label>
                            </div>                            
                        </div>
                        <div class="icon"><i class=" ion ion-person"></i></div>
                    </div>
                </div>

                 <div class="col-lg-4 col-xs-12">
                    <!-- small box -->
                    <div class="small-box TotalEmp" style="background-image: linear-gradient(-90deg,#40E0D0, #3c8dbc); border-radius: 15px;">
                        <div class="inner" style="display: inline-block">
                            <div class="col-lg-3 col-md-3">
                                <span>
                                    <label id="lblProduct" runat="server" style="font-size: 25px"></label>
                                    <b style="font-size: medium">%</b></span><br />
                                <label style="font-size: 12px; font-family: Arial">Productivity </label>
                            </div>
                            <div class="col-lg-3 col-md-3" id="dvvisiteddist">
                                <span>
                                    <label id="lblVisitedDist" runat="server" style="font-size: 25px"></label>
                                </span>
                                <label style="font-size: 12px; font-family: Arial">Visited Distributor</label>
                            </div>
                            <div class="col-lg-3 col-md-3" id="dvproductivedist">
                                <span>
                                    <label id="lblProductiveDist" runat="server" style="font-size: 25px"></label>
                                </span>
                                <label style="font-size: 11px; font-family: Arial">Productive Distributor</label>
                            </div>
                          
                        </div>

                        <div class="icon">
                            <i class=" ion ion-person"></i>
                        </div>
                    </div>
                </div>

                <div class="col-lg-4 col-xs-12">
                    <!-- small box -->
                    <div class="small-box TotalEmp" style="background-image: linear-gradient(-90deg,#40E0D0, #3c8dbc); border-radius: 15px;">
                        <div class="inner" style="display: inline-block">
                            <div class="col-lg-3 col-md-4">
                                <span>
                                    <label id="lblProductivity" runat="server" style="font-size: 25px"></label>
                                    <b style="font-size: medium">%</b></span><br />
                                <label style="font-size: 12px; font-family: Arial">Productivity </label>
                            </div>
                            <div class="col-lg-2 col-md-4" id="dvisited">
                                <span>
                                    <label id="lblVisited" runat="server" style="font-size: 25px"></label>
                                </span>
                                <label style="font-size: 12px; font-family: Arial">Visited Retailer</label>
                            </div>
                            <div class="col-lg-3 col-md-4" id="dvproductiveretailer">
                                <span>
                                    <label id="lblProductive" runat="server" style="font-size: 25px"></label>
                                </span>
                                <label style="font-size: 12px; font-family: Arial">Productive Retailer</label>
                            </div>
                            <div class="col-lg-2 col-md-2" id="dvnewretailer">
                                <span>
                                    <label id="lblNewRetailer" runat="server" style="font-size: 25px">00</label></span><br />
                                <label style="font-size: 12px; font-family: Arial">New Retailer</label>
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
                                <div class="panel-heading primaryHeading" style="background-image: linear-gradient(-90deg,#FF7F50, #20B2AA);" id="Primary">
                                    Primary Sale
                                <div class="box-tools pull-right">
                                    <%--<a href="Prim" data-toggle="collapse"><i class="fa fa-minus"></i></a>--%>
                                </div>
                                </div>
                                <div id="Primarys" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px; overflow: hidden">
                                         <div id="piechart" style="height: 170px"></div>
                                     
                                       
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-16 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading secondaryHeading" style="background-image: linear-gradient(-90deg,coral, #3c8dbc);" id="Secondary">
                                    Secondary Sale
                                <div class="box-tools pull-right">
                                    <%--<a href="Sec" data-toggle="collapse"><i class="fa fa-minus"></i></a>--%>
                                </div>
                                </div>
                                <div id="secondarys" class="panel-collapse collapse in">

                                    <div class="panel-body table-responsive" style="height: 200px; overflow: hidden">
                                       <%-- <div id="chat" style="margin-top:-38px;"></div>
                                        <script type="text/javascript">
                                            // Load google charts
                                            google.charts.load('current', { 'packages': ['corechart'] });
                                            google.charts.setOnLoadCallback(drawChart);

                                            // Draw the chart and set the chart values
                                            function drawChart() {
                                                var data = google.visualization.arrayToDataTable([
                                                ['Task', 'Hours per Day'],
                                                ['Total Order', 8],
                                                ['Failed Visit', 2],
                                                ['Demo', 2],
                                                ]);

                                                // Optional; add a title and set the width and height of the chart
                                                var options = { 'width': 500, 'height': 260, 'margin-top':30 };

                                                // Display the chart inside the <div> element with id="piechart"
                                                var chart = new google.visualization.PieChart(document.getElementById('chat'));
                                                chart.draw(data, options);
                                            }
</script>--%> 
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
                                <div class="panel-heading UnApprovedHeading" style="background-image: linear-gradient(-90deg,#BC8F8F, #4682B4);" id="">
                                    UnApproved Status
                                <div class="box-tools pull-right">
                                    <%--<a href="UnApp" data-toggle="collapse"><i class="fa fa-minus"></i></a>--%>
                                </div>
                                </div>
                                <div id="" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px; overflow: hidden">
                                        <div id="doughnutchart" style="height:171px; margin-top:13px;"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading OrderHeading" style="background-image: linear-gradient(-90deg,#FFC0CB,#FA8072	);" id="">
                                    ASM Wise Sales (In Qty)
                                <div class="box-tools pull-right">
                                    <%--<a href="bottomsecsales" data-toggle="collapse"><i class="fa fa-minus"></i></a>--%>
                                    <%--<a href='DashboardNoSalesReason.aspx?Date=" + $("#ContentPlaceHolder1_FromDate").val() + "&Name=" + topping + "" data-toggle="collapse">Name</a>--%>
                                   <button style="border:none; background-image:linear-gradient(-90deg,#FFC0CB,#FA8072);" onclick="ASMWiseSales('T')">Show More!</button>
                                </div>
                                </div>
                                <div id="" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px; overflow: hidden">
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
                                <div class="panel-heading primaryHeading" id="UnApproved" style="background-image: linear-gradient(-90deg,#303f9f,#ef9a9a);">
                                    In-Time Statistics
                                <div class="box-tools pull-right">
                                    <%-- <a href="InTimwStats" data-toggle="collapse"><i class="fa fa-minus"></i></a>--%>
                                </div>
                                </div>
                                <div id="UnApprovedchart" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px; overflow: hidden">
                                        <div id="piechart4" style= "margin-left:-20px"> </div>
                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
               
                   
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading secondaryHeading" style="background-image: linear-gradient(-90deg,#FF4500,#000080);" id="">
                                    No Sales Reason
                                <div class="box-tools pull-right">
                                    <%-- <a href="NoSale" data-toggle="collapse"><i class="fa fa-minus"></i></a>--%>
                                </div>
                                </div>
                                <div id="Order" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px; overflow: hidden">   
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
                                <div class="panel-heading UnApprovedHeading" style="background-image: linear-gradient(-90deg,#4682B4,SkyBlue);" id="">
                                    Product Class Wise Sales (In Qty)
                                <div class="box-tools pull-right">
                                    <%--<a href="categorysale" data-toggle="collapse"><i class="fa fa-minus"></i></a>--%>
                                     <button style="border:none; background-image:linear-gradient(-90deg,#4682B4,SkyBlue);" onclick="ProductClassWiseSales('T')">Show More!</button>
                                </div>
                                </div>
                                <div id="" class="panel-collapse collapse in">
                               
                                    <div class="panel-body table-responsive" style="height: 200px; overflow: hidden">                      
    
                                        <div id="columnchart1" style="height: 170px;"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
               
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading UnApprovedHeading" style="background-image: linear-gradient(-90deg,#880e4f,#ff80ab);" id="">
                                    Product Segment Wise Sales (In Qty)
                                <div class="box-tools pull-right">
                                     <button style="border:none; background-image:linear-gradient(-90deg,#880e4f,#ff80ab);" onclick="ProductSegmentWiseSales('T')">Show More!</button>
                                    <%--<a href="categorysale" data-toggle="collapse"><i class="fa fa-minus"></i></a>--%>
                                </div>
                                </div>
                                <div id="" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px; overflow: hidden">
                                       

    
    
                                          <div id="columnchart7" style="height: 170px"></div>
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
                                <div class="panel-heading primaryHeading" id="" style="background-image: linear-gradient(-90deg,#DD4477,#FFA500);">
                                    Top 5 SKU (In Qty)
                                <div class="box-tools pull-right">
                                     <button style="border:none; background-image:linear-gradient(-90deg,#DD4477,#FFA500);" onclick="SKU('T')">Show More!</button>
                                </div>
                                </div>
                                <div id="" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px; overflow: hidden">
                                        <div id="columnchart3" style="height: 170px"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                     <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading secondaryHeading" style="background-image: linear-gradient(-90deg,#87CEFA,#EE82EE);" id="">
                                    Bottom 5 SKU (In Qty)
                                <div class="box-tools pull-right">
                                     <button style="border:none; background-image:linear-gradient(-90deg,#87CEFA,#EE82EE);" onclick="BottomSKU('B')">Show More!</button>
                                </div>
                                </div>
                                <div id="" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px; overflow: hidden">
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
                                <div class="panel-heading UnApprovedHeading" style="background-image: linear-gradient(-90deg,#2F4F4F,FFD700	);" id="">
                                    Top 5 Product Group Wise Sales (In Qty)
                                <div class="box-tools pull-right">
                                  <%-- <a href="topsecsales" data-toggle="collapse"><i class="fa fa-minus"></i></a>--%>
                                     <button style="border:none; background-color:#b3ecff;" onclick="ProductGroupWiseSales('T')">Show More!</button>
                                </div>
                                </div>
                                <div id="" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px; overflow: hidden">
                                        <div id="columnchart5" style="height: 170px"></div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading UnApprovedHeading" style="background-image: linear-gradient(-90deg,#FF4500,#2F4F4F	);" id="">
                                    Bottom 5 ProductGroup Wise Sales (In Qty)
                                <div class="box-tools pull-right">
                                   <%--<a href="topsecsales" data-toggle="collapse"><i class="fa fa-minus"></i></a>--%>
                                     <button style="border:none; background-image:linear-gradient(-90deg,#FF4500,#2F4F4F	);" onclick="BottomProductGroupWiseSales('B')">Show More!</button>
                                </div>
                                </div>
                                <div id="" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px; overflow: hidden">
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
            /*background-color: rgba(0,192,239,1.0);*/
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
            /*background-color: rgb(124, 12, 78);*/
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

        .col-lg-12 col-xs-122 {
	margin: 1em 0 0.5em 0;
	color: #343434;
	font-weight: normal;
	font-family: 'Ultra', sans-serif;   
	font-size: 36px;
	line-height: 42px;
	text-transform: uppercase;
	text-shadow: 0 2px white, 0 3px #777;
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
