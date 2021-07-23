<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DailyDashboard.aspx.cs" Inherits="AstralFFMS.DailyDashBoard" EnableEventValidation="false" %>

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
            // Getcountofinstitutnalparty
            $.ajax({
                type: "POST",
                url: "DailyDashBoard.aspx/Getcountofinstitutnalparty",
                //  data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    var resdata = JSON.parse(response.d);
                    console.log(resdata);
                    if (resdata == 'P') {
                        $('#Insdiv').show();
                    }
                        <%-- if (resdata != null) {
                        var totalmembers = parseInt(resdata[0].Person) + parseInt(resdata[1].Person) + parseInt(resdata[2].Person);
                        $('#<%=lblTotal.ClientID%>').text(resdata[0].Person);
                        $('#<%=lblPresent.ClientID%>').text(resdata[1].Person);
                        $('#<%=lblAbsent.ClientID%>').text(resdata[2].Person);
                        $('#<%=lblLeave.ClientID%>').text(resdata[3].Person);
                     <%--   $('#<%=lblInActive.ClientID%>').text(resdata[3].Person);
                        $('#pageloaddiv').hide();
                    }--%>
                }
            });
            FillCount();
            FillTotalPrimarySales();
            FillTotalSecondarySales();
            FillTotalInstitutnalSales();
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
            })

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
                        $('#<%=lblreject.ClientID%>').text(resdata[2].Person);
                        $('#<%=lblAbsent.ClientID%>').text(resdata[3].Person);
                        $('#<%=lblLeave.ClientID%>').text(resdata[4].Person);
                        $('#<%=lblTotalLeave.ClientID%>').text(resdata[5].Person);
                        <%--   $('#<%=lblInActive.ClientID%>').text(resdata[3].Person);--%>
                        $('#pageloaddiv').hide();
                    }
                }
            });
        }

        function FillTotalPrimarySales() {

            $('#pageloaddiv').show();
            $.ajax({
                type: "POST",
                url: "DailyDashBoard.aspx/FillTotalPrimarysale",
                data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    var resdata = JSON.parse(response.d);
                    if (resdata != null) {
                        var num = parseInt(resdata[0].totalPrimAmt);
                        var n = num.toFixed(0);
                        $('#<%=lblPrimAmount.ClientID%>').text(n);
                        $('#pageloaddiv').hide();
                    }
                }
            });
        }
        function FillTotalInstitutnalSales() {

            $('#pageloaddiv').show();
            $.ajax({
                type: "POST",
                url: "DailyDashBoard.aspx/FillTotalInsPrimarysale",
                data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    var resdata = JSON.parse(response.d);
                    if (resdata != null) {
                        var num = parseInt(resdata[0].totalPrimAmt);
                        var n = num.toFixed(0);
                        $('#<%=lblInsAmount.ClientID%>').text(n);
                        $('#pageloaddiv').hide();
                    }
                }
            });
        }

        function FillTotalSecondarySales() {

            $('#pageloaddiv').show();
            $.ajax({
                type: "POST",
                url: "DailyDashBoard.aspx/FillTotalSecondarysale",
                data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    var resdata = JSON.parse(response.d);
                    if (resdata != null) {
                        var num = parseInt(resdata[0].totalSecAmt);
                        var n = num.toFixed(0);
                        $('#<%=lblSecAmount.ClientID%>').text(n);
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
                                    //window.open('DashboardPrimary.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val() + '&Name=' + topping + '', '_blank');
                                    window.open('DashboardPrimary.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val() + '&Name=' + topping + '&PT=P ', '_blank');
                                }
                            }
                            google.visualization.events.addListener(chart, 'select', selectHandler);
                            chart.draw(data, options);
                        }
                    }
                });
            }

            //////ins


            google.load("visualization", "1", { packages: ["corechart"], "callback": drawChartins });
            google.setOnLoadCallback(drawChartins);
            function drawChartins() {
                $.ajax({
                    type: "POST",
                    url: "DailyDashBoard.aspx/FillAllRecord",
                    data: "{FromDate:'" + $('#ContentPlaceHolder1_FromDate').val() + "',Type:'Institutnal'}",
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
                            var chart = new google.visualization.PieChart(document.getElementById('piechart3'));
                            function selectHandler() {
                                var selectedItem = chart.getSelection()[0];
                                if (selectedItem) {
                                    var topping = data.getValue(selectedItem.row, 0);
                                    //window.open('DashboardPrimary.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val() + '&Name=' + topping + '', '_blank');                                  
                                    window.open('DashboardPrimary.aspx?Date=' + $('#ContentPlaceHolder1_FromDate').val() + '&Name=' + topping + '&PT=I', '_blank');
                                }
                            }
                            google.visualization.events.addListener(chart, 'select', selectHandler);
                            chart.draw(data, options);
                        }
                    }
                });
            }
            /////ins


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
                                colors: ["#87CEFA", "#DC143C", "#000080", "#87bd16", "#40E0D0", "#FF4500"]
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

                                title: '', chartArea: { width: "80%", height: "70%" }, bar: { groupWidth: '25%' }, tooltip: { trigger: 'hover' },

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
    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/modules/data.js"></script>
    <script src="https://code.highcharts.com/modules/series-label.js"></script>


    <!-- Additional files for the Highslide popup effect -->
    <script src="https://www.highcharts.com/media/com_demo/js/highslide-full.min.js"></script>
    <script src="https://www.highcharts.com/media/com_demo/js/highslide.config.js" charset="utf-8"></script>
    <link rel="stylesheet" type="text/css" href="https://www.highcharts.com/media/com_demo/css/highslide.css" />

    <link rel="stylesheet" type="text/css" href="css/custom.css" />


    <script>
        function retDate() {
            date = $('#<%=FromDate.ClientID%>').val();

            mon = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
            if (date == "") {
                var d = new Date();
                date = new Date(d);
                month = mon[d.getMonth()],
                day = d.getDate(),
                year = d.getFullYear();
                if (month.length < 2) month = '0' + month;
                if (day.length < 2) day = '0' + day;

                date = [day, month, year].join('/');
            }
            return date;
        }
        //alert(retDate());
    </script>


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
                <div class="col-lg-6 col-xs-12" style="padding-left: 3em; font-size: large; font-size: 21px; font-weight: bolder; text-align: center; text-transform: uppercase;">
                    <span style="color: #b71c1c">D</span>aily <span style="color: #b71c1c">D</span>ashboard                   
                </div>

            </div>

            <%--  <div class="row" style="margin-bottom: 15px;">                
                <div class="col-lg-12 col-xs-12" style="padding-left: 3em; font-size:21px; font-weight: bolder; text-align: center;">               
                
                      </div>                  
            </div>  --%>

            <hr>

            <div class="row">
                <div class="col-md-12">
                    <div class="col-lg-4 col-xs-12">
                        <!-- small box -->
                        <div class="small-box TotalEmp" style="background-image: linear-gradient(-90deg,#40E0D0, #3c8dbc); border-radius: 15px;">
                            <div class="inner" style="display: inline-block">
                                <div class="col-lg-2 col-md-2 col-xs-12" id="dvTotal" style="padding: 0px !important;">
                                    <span>
                                        <label id="lblTotal" runat="server" style="font-size: 25px"></label>
                                    </span>
                                    <label style="font-size: 12px; font-family: Arial">Total Members</label>
                                </div>
                                <div class="col-lg-2 col-md-2 col-xs-12" id="dvPresent" style="padding: 0px !important;">
                                    <span>
                                        <label id="lblPresent" runat="server" style="font-size: 25px"></label>
                                        
                                    </span><br />
                                    <label style="font-size: 12px; font-family: Arial">Present </label>
                                </div>
                                <div class="col-lg-3 col-md-3 col-xs-12" id="dvAbsent" style="padding: 0px !important;">
                                    <span>
                                        <label id="lblAbsent" runat="server" style="font-size: 25px"></label> /
                                        <label id="lblreject" runat="server" style="font-size: 25px"></label>
                                    </span>
                                    <br />
                                    <label style="font-size: 12px; font-family: Arial">Absent / Rejected DSR</label>
                                </div>
                                <div class="col-lg-3 col-md-3 col-xs-12" id="dvLeave" style="padding: 0px !important;">
                                    <span>
                                        <label id="lblLeave" runat="server" style="font-size: 25px"></label>
                                        /
                                        <label id="lblTotalLeave" runat="server" style="font-size: 25px"></label>
                                    </span>
                                    <label style="font-size: 12px; font-family: Arial">Approved / Total Leave</label>
                                </div>
                            </div>
                            <div class="icon"><i class=" ion ion-person"></i></div>
                        </div>
                    </div>

                    <div class="col-lg-4 col-xs-12">
                        <!-- small box -->
                        <div class="small-box TotalEmp" style="background-image: linear-gradient(-90deg,#40E0D0, #3c8dbc); border-radius: 15px;">
                            <div class="inner" style="display: inline-block">
                                <div class="col-lg-3 col-md-3 col-xs-12">
                                    <span>
                                        <label id="lblProduct" runat="server" style="font-size: 25px"></label>
                                        <b style="font-size: medium">%</b></span><br />
                                    <label style="font-size: 12px; font-family: Arial">Productivity </label>
                                </div>
                                <div class="col-lg-3 col-md-3 col-xs-12" id="dvvisiteddist">
                                    <span>
                                        <label id="lblVisitedDist" runat="server" style="font-size: 25px"></label>
                                    </span>
                                    <label style="font-size: 12px; font-family: Arial">Visited Distributor</label>
                                </div>
                                <div class="col-lg-3 col-md-3 col-xs-12" id="dvproductivedist">
                                    <span>
                                        <label id="lblProductiveDist" runat="server" style="font-size: 25px"></label>
                                    </span>
                                    <label style="font-size: 11px; font-family: Arial">Productive Distributor</label>
                                </div>

                            </div>

                            <div class="icon" style="opacity: 0.15; padding-top: 5px;">
                                <%--<i class=" ion ion-person"></i>--%>
                                <img src="img/dist.png" width="85" style="vertical-align: initial;" />
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-4 col-xs-12">
                        <!-- small box -->
                        <div class="small-box TotalEmp" style="background-image: linear-gradient(-90deg,#40E0D0, #3c8dbc); border-radius: 15px;">
                            <div class="inner" style="display: inline-block">
                                <div class="col-lg-3 col-md-3 col-xs-12" style="padding: 0px !important;">
                                    <span>
                                        <label id="lblProductivity" runat="server" style="font-size: 25px"></label>
                                        <b style="font-size: medium">%</b></span><br />
                                    <label style="font-size: 12px; font-family: Arial">Productivity </label>
                                </div>
                                <div class="col-lg-2 col-md-2 col-xs-12" id="dvisited" style="padding: 0px !important;">
                                    <span>
                                        <label id="lblVisited" runat="server" style="font-size: 25px"></label>
                                    </span>
                                    <label style="font-size: 12px; font-family: Arial">Visited Retailer</label>
                                </div>
                                <div class="col-lg-3 col-md-3 col-xs-12" id="dvproductiveretailer" style="padding: 0px !important;">
                                    <span>
                                        <label id="lblProductive" runat="server" style="font-size: 25px"></label>
                                    </span>
                                    <label style="font-size: 12px; font-family: Arial">Productive Retailer</label>
                                </div>
                                <div class="col-lg-2 col-md-2 col-xs-12" id="dvnewretailer" style="padding: 0px !important;">
                                    <span>
                                        <label id="lblNewRetailer" runat="server" style="font-size: 25px">00</label></span><br />
                                    <label style="font-size: 12px; font-family: Arial">New Retailer</label>
                                </div>
                            </div>

                            <div class="icon" style="opacity: 0.15; padding-top: 5px;">
                                <%--<i class=" ion ion-person"></i>--%>
                                <img src="img/ret.png" width="75" style="vertical-align: initial;" />
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
                                <div class="panel-heading primaryHeading" style="background-image: linear-gradient(-90deg,#FF7F50, #20B2AA);" id="Primary">
                                    Primary Sale
                                <div class="box-tools pull-right">
                                    <%--<a href="Prim" data-toggle="collapse"><i class="fa fa-minus"></i></a>--%>
                                    <%--  <span style="color:#2e92a9">Total Primary Sale:</span>    --%>
                                    <label id="Label2" runat="server" style="font-size: 22px">₹</label>
                                    <label id="lblPrimAmount" runat="server" style="font-size: 22px"></label>
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
                                    <%-- <span style="color:#2e92a9">Total Secondary Sale:</span>   
                                    --%>

                                    <label id="Label1" runat="server" style="font-size: 22px">₹</label>
                                    <label id="lblSecAmount" runat="server" style="font-size: 22px"></label>
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
                                        <div id="piechart1" style="height: 170px">
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="Insdiv" style="display: none">
                <div class="col-md-12 col-lg-12">
                    <div class="col-md-12 col-lg-6">
                        <div class="panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading primaryHeading" style="background-image: linear-gradient(-90deg,#FF7F50, #20B2AA);" id="Primary">
                                    Institutional Sale
                                <div class="box-tools pull-right">
                                    <%--<a href="Prim" data-toggle="collapse"><i class="fa fa-minus"></i></a>--%>
                                    <%--  <span style="color:#2e92a9">Total Primary Sale:</span>    --%>
                                    <label id="Label3" runat="server" style="font-size: 22px">₹</label>
                                    <label id="lblInsAmount" runat="server" style="font-size: 22px"></label>
                                </div>
                                </div>
                                <div id="Institunal" class="panel-collapse collapse in">
                                    <div class="panel-body table-responsive" style="height: 200px; overflow: hidden">
                                        <div id="piechart3" style="height: 170px"></div>


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
                                        <div id="doughnutchart" style="height: 171px; margin-top: 13px;"></div>
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
                                    <button style="border: none; background-image: linear-gradient(-90deg,#FFC0CB,#FA8072);" onclick="ASMWiseSales('T')">Show More!</button>
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
                                        <div id="piechart4" style="margin-left: -20px"></div>

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
                                    <button style="border: none; background-image: linear-gradient(-90deg,#4682B4,SkyBlue);" onclick="ProductClassWiseSales('T')">Show More!</button>
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
                                    <button style="border: none; background-image: linear-gradient(-90deg,#880e4f,#ff80ab);" onclick="ProductSegmentWiseSales('T')">Show More!</button>
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
                                    <button style="border: none; background-image: linear-gradient(-90deg,#DD4477,#FFA500);" onclick="SKU('T')">Show More!</button>
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
                                    <button style="border: none; background-image: linear-gradient(-90deg,#87CEFA,#EE82EE);" onclick="BottomSKU('B')">Show More!</button>
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
                                    <button style="border: none; background-color: #b3ecff;" onclick="ProductGroupWiseSales('T')">Show More!</button>
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
                                    <button style="border: none; background-image: linear-gradient(-90deg,#FF4500,#2F4F4F	);" onclick="BottomProductGroupWiseSales('B')">Show More!</button>
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

            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <div class="panel-heading UnApprovedHeading" style="background-image: linear-gradient(-90deg,#E5E5BE,#003973);" id="">
                        Sales person Daily Working
                    <div class="box-tools pull-right">
                        <%--<a href="topsecsales" data-toggle="collapse"><i class="fa fa-minus"></i></a>--%>
                        <%--<button style="border:none; background-image:linear-gradient(-90deg,#FF4500,#2F4F4F	);" onclick="BottomProductGroupWiseSales('B')">Show More!</button>--%>
                    </div>
                    </div>

                    <div class="table-responsive sale_tab panel-group" style="max-height: 500px; overflow-y: scroll; margin-top: 10px; min-height: 250px;">
                        <table class="table table-bordered table-stripped">
                            <thead>
                                <tr class="bg-primary">
                                    <th>Sales Person</th>
                                    <th>06:00-06:30</th>
                                    <th>06:30-07:00</th>
                                    <th>07:00-07:30</th>
                                    <th>07:30-08:00</th>
                                    <th>08:00-08:30</th>
                                    <th>08:30-09:00</th>
                                    <th>09:00-09:30</th>
                                    <th>09:30-10:00</th>
                                    <th>10:00-10:30</th>
                                    <th>10:30-11:00</th>
                                    <th>11:00-11:30</th>
                                    <th>11:30-12:00</th>
                                    <th>12:00-12:30</th>
                                    <th>12:30-13:00</th>
                                    <th>13:00-13:30</th>
                                    <th>13:30-14:00</th>
                                    <th>14:00-14:30</th>
                                    <th>14:30-15:00</th>
                                    <th>15:00-15:30</th>
                                    <th>15:30-16:00</th>
                                    <th>16:00-16:30</th>
                                    <th>16:30-17:00</th>
                                    <th>17:00-17:30</th>
                                    <th>17:30-18:00</th>
                                    <th>18:00-18:30</th>
                                    <th>18:30-19:00</th>
                                    <th>19:00-19:30</th>
                                    <th>19:30-20:00</th>
                                    <th>20:00-20:30</th>
                                    <th>20:30-21:00</th>
                                </tr>
                            </thead>
                            <tbody id="sales_activity_report">
                                <script>
                                    function getClass(i) {
                                        if (i % 5 == 0) {
                                            cls = "text text-success";
                                        }
                                        else if (i % 5 == 1) {
                                            cls = "text text-warning";
                                        }
                                        else if (i % 5 == 2) {
                                            cls = "text text-primary";
                                        }
                                        else if (i % 5 == 3) {
                                            cls = "text text-secondary";
                                        }
                                        else if (i % 5 == 4) {
                                            cls = "text text-info";
                                        }
                                        return cls;
                                    }



                                    $(document).ready(function () {

                                        var htmlString = "";
                                        mydate = retDate();

                                        var act_type = [];
                                        var smName = [];
                                        var time = [];
                                        var per_time = [];
                                        var per_act = [];
                                       


                               <%--       //$.ajax({
                                          type: "POST",
                                          url: '<%= ResolveUrl("And_Sync.asmx/Dashboard_PriarySecondarydata_xml") %>',
                                          contentType: "application/json; charset=utf-8",
                                          data: '{date: "' + mydate + '"}',
                                          dataType: "json",
                                          success: function (data) {       XML FORMAT         --%>

                                      $.getJSON("<%= ResolveUrl("And_Sync.asmx/Dashboard_PriarySecondarydata_json") %>", { date: mydate }).done(function (data) {
                                          $.each(data, function (key1, value1) {
                                              //alert(data[key1].time)
                                              time[key1] = data[key1].time;
                                              act_type[key1] = data[key1].Activitytype;
                                              per_time = time[key1].split(",");
                                              per_act = act_type[key1].split(",");
                                              
                                              //htmlString += "<tr><td class='name_bold'> <a target='_blank' style='cursor:pointer;' href='dsrreport.aspx?SMID=" + data[key1].smId + "&date=" + mydate + "&rec_status=Unlock' class='" + getClass(key1) + "' title='Click to View Details'> " + data[key1].SMName + "</a></td>";
                                              debugger;
                                              htmlString += "<tr><td class='name_bold'> <a target='_blank' style='cursor:pointer;' href='dsrreport.aspx?SMID=" + data[key1].smId + "&date=" + mydate + "&Recstatus=" + data[key1].Status + "' class='" + getClass(key1) + "' title='Click to View Details'> " + data[key1].SMName + "</a></td>";

                                              var time_6 = ["-"]; var time_6_3 = ["-"], time_7 = ["-"]; var time_7_3 = ["-"], time_8 = ["-"]; var time_8_3 = ["-"], time_9 = ["-"], time_9_3 = ["-"], time_10 = ["-"],
                                                  time_10_3 = ["-"], time_11 = ["-"], time_11_3 = ["-"], time_12 = ["-"],
                                                  time_12_3 = ["-"], time_13 = ["-"], time_13_3 = ["-"], time_14 = ["-"],
                                                  time_14_3 = ["-"], time_15 = ["-"], time_15_3 = ["-"], time_16 = ["-"],
                                                  time_16_3 = ["-"], time_17 = ["-"], time_17_3 = ["-"], time_18 = ["-"],
                                                  time_18_3 = ["-"], time_19 = ["-"], time_19_3 = ["-"], time_20 = ["-"], time_20_3 = ["-"];

                                              for (i = 0; i < per_time.length; i++) {
                                                  //alert(per_time[i].substring(0, per_time[i].indexOf(":")));
                                                  //debugger;



                                                  switch (parseInt(per_time[i].substring(0, per_time[i].indexOf(":")))) {
                                                      case 6:
                                                          if (parseInt(per_time[i].substring(per_time[i].indexOf(":") + 1)) <= 30) {
                                                              time_6[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          else {
                                                              time_6_3[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          break;
                                                      case 7:
                                                          if (parseInt(per_time[i].substring(per_time[i].indexOf(":") + 1)) <= 30) {
                                                              time_7[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          else {
                                                              time_7_3[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          break;

                                                      case 8:
                                                          if (parseInt(per_time[i].substring(per_time[i].indexOf(":") + 1)) <= 30) {
                                                              time_8[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          else {
                                                              time_8_3[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          break;
                                                      case 9:
                                                          if (parseInt(per_time[i].substring(per_time[i].indexOf(":") + 1)) <= 30) {
                                                              time_9[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          else {
                                                              time_9_3[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          break;
                                                      case 10:
                                                          if (parseInt(per_time[i].substring(per_time[i].indexOf(":") + 1)) <= 30) {
                                                              time_10[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          else {
                                                              time_10_3[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          break;
                                                      case 11:
                                                          if (parseInt(per_time[i].substring(per_time[i].indexOf(":") + 1)) <= 30) {
                                                              time_11[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          else {
                                                              time_11_3[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          break;
                                                      case 12:
                                                          if (parseInt(per_time[i].substring(per_time[i].indexOf(":") + 1)) <= 30) {
                                                              time_12[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          else {
                                                              time_12_3[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          break;
                                                      case 13:
                                                          if (parseInt(per_time[i].substring(per_time[i].indexOf(":") + 1)) <= 30) {
                                                              time_13[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          else {
                                                              time_13_3[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          break;
                                                      case 14:
                                                          if (parseInt(per_time[i].substring(per_time[i].indexOf(":") + 1)) <= 30) {
                                                              time_14[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          else {
                                                              time_14_3[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          break;
                                                      case 15:
                                                          if (parseInt(per_time[i].substring(per_time[i].indexOf(":") + 1)) <= 30) {
                                                              time_15[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          else {
                                                              time_15_3[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          break;
                                                      case 16:
                                                          if (parseInt(per_time[i].substring(per_time[i].indexOf(":") + 1)) <= 30) {
                                                              time_16[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          else {
                                                              time_16_3[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          break;
                                                      case 17:
                                                          if (parseInt(per_time[i].substring(per_time[i].indexOf(":") + 1)) <= 30) {
                                                              time_17[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          else {
                                                              time_17_3[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          break;
                                                      case 18:
                                                          if (parseInt(per_time[i].substring(per_time[i].indexOf(":") + 1)) <= 30) {
                                                              time_18[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          else {
                                                              time_18_3[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          break;
                                                      case 19:
                                                          if (parseInt(per_time[i].substring(per_time[i].indexOf(":") + 1)) <= 30) {
                                                              time_19[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          else {
                                                              time_19_3[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          break;
                                                      case 20:
                                                          if (parseInt(per_time[i].substring(per_time[i].indexOf(":") + 1)) <= 30) {
                                                              time_20[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          else {
                                                              time_20_3[i] = per_time[i] + "  " + per_act[i];
                                                          }
                                                          break;

                                                      default:
                                                          break;
                                                  }

                                              }

                                              if (time_6.length > 1) {
                                                  while (time_6[0] == "-" || time_6[0] == null) {
                                                      time_6.shift();
                                                  }
                                                  if (time_6.length > 1) {
                                                      var str = "";
                                                      str_6 = "<td style='position:relative;'>" + time_6[0] + "<ul>";
                                                      for (j = 0; j < time_6.length; j++) {
                                                          str += "<li>" + time_6[j] + "</li>";
                                                      }
                                                      str_6 += str + "</ul></td>";
                                                  } else {
                                                      str_6 = "<td>" + time_6[0] + "</td>";
                                                  }
                                              } else {
                                                  str_6 = "<td>" + time_6[0] + "</td>";
                                              }
                                              //alert(str_8)

                                              if (time_6_3.length > 1) {
                                                  while (time_6_3[0] == "-" || time_6_3[0] == null) {
                                                      time_6_3.shift();
                                                  }
                                                  if (time_6_3.length > 1) {
                                                      var str = "";
                                                      str_6_3 = "<td style='position:relative;'>" + time_6_3[0] + "<ul>";
                                                      for (j = 0; j < time_6_3.length; j++) {
                                                          str += "<li>" + time_6_3[j] + "</li>";
                                                      }
                                                      str_6_3 += str + "</ul></td>";
                                                  } else {
                                                      str_6_3 = "<td>" + time_6_3[0] + "</td>";
                                                  }
                                              } else {
                                                  str_6_3 = "<td>" + time_6_3[0] + "</td>";
                                              }

                                              if (time_7.length > 1) {
                                                  while (time_6=7[0] == "-" || time_7[0] == null) {
                                                      time_7.shift();
                                                  }
                                                  if (time_7.length > 1) {
                                                      var str = "";
                                                      str_7 = "<td style='position:relative;'>" + time_7[0] + "<ul>";
                                                      for (j = 0; j < time_7.length; j++) {
                                                          str += "<li>" + time_7[j] + "</li>";
                                                      }
                                                      str_7 += str + "</ul></td>";
                                                  } else {
                                                      str_7 = "<td>" + time_7[0] + "</td>";
                                                  }
                                              } else {
                                                  str_7 = "<td>" + time_7[0] + "</td>";
                                              }
                                              //alert(str_8)

                                              if (time_7_3.length > 1) {
                                                  while (time_7_3[0] == "-" || time_7_3[0] == null) {
                                                      time_7_3.shift();
                                                  }
                                                  if (time_7_3.length > 1) {
                                                      var str = "";
                                                      str_7_3 = "<td style='position:relative;'>" + time_7_3[0] + "<ul>";
                                                      for (j = 0; j < time_7_3.length; j++) {
                                                          str += "<li>" + time_7_3[j] + "</li>";
                                                      }
                                                      str_7_3 += str + "</ul></td>";
                                                  } else {
                                                      str_7_3 = "<td>" + time_7_3[0] + "</td>";
                                                  }
                                              } else {
                                                  str_7_3 = "<td>" + time_7_3[0] + "</td>";
                                              }

                                              if (time_8.length > 1) {
                                                  while (time_8[0] == "-" || time_8[0] == null) {
                                                      time_8.shift();
                                                  }
                                                  if (time_8.length > 1) {
                                                      var str = "";
                                                      str_8 = "<td style='position:relative;'>" + time_8[0] + "<ul>";
                                                      for (j = 0; j < time_8.length; j++) {
                                                          str += "<li>" + time_8[j] + "</li>";
                                                      }
                                                      str_8 += str + "</ul></td>";
                                                  } else {
                                                      str_8 = "<td>" + time_8[0] + "</td>";
                                                  }
                                              } else {
                                                  str_8 = "<td>" + time_8[0] + "</td>";
                                              }
                                              //alert(str_8)

                                              if (time_8_3.length > 1) {
                                                  while (time_8_3[0] == "-" || time_8_3[0] == null) {
                                                      time_8_3.shift();
                                                  }
                                                  if (time_8_3.length > 1) {
                                                      var str = "";
                                                      str_8_3 = "<td style='position:relative;'>" + time_8_3[0] + "<ul>";
                                                      for (j = 0; j < time_8_3.length; j++) {
                                                          str += "<li>" + time_8_3[j] + "</li>";
                                                      }
                                                      str_8_3 += str + "</ul></td>";
                                                  } else {
                                                      str_8_3 = "<td>" + time_8_3[0] + "</td>";
                                                  }
                                              } else {
                                                  str_8_3 = "<td>" + time_8_3[0] + "</td>";
                                              }


                                              if (time_9.length > 1) {
                                                  while (time_9[0] == "-" || time_9[0] == null) {
                                                      time_9.shift();
                                                  }
                                                  if (time_9.length > 1) {
                                                      var str = "";
                                                      str_9 = "<td style='position:relative;'>" + time_9[0] + "<ul>";
                                                      for (j = 0; j < time_9.length; j++) {
                                                          str += "<li>" + time_9[j] + "</li>";
                                                      }
                                                      str_9 += str + "</ul></td>";
                                                  } else {
                                                      str_9 = "<td>" + time_9[0] + "</td>";
                                                  }
                                              } else {
                                                  str_9 = "<td>" + time_9[0] + "</td>";
                                              }


                                              if (time_9_3.length > 1) {
                                                  while (time_9_3[0] == "-" || time_9_3[0] == null) {
                                                      time_9_3.shift();
                                                  }
                                                  if (time_9_3.length > 1) {
                                                      var str = "";
                                                      str_9_3 = "<td style='position:relative;'>" + time_9_3[0] + "<ul>";
                                                      for (j = 0; j < time_9_3.length; j++) {
                                                          str += "<li>" + time_9_3[j] + "</li>";
                                                      }
                                                      str_9_3 += str + "</ul></td>";
                                                  } else {
                                                      str_9_3 = "<td>" + time_9_3[0] + "</td>";
                                                  }
                                              } else {
                                                  str_9_3 = "<td>" + time_9_3[0] + "</td>";
                                              }


                                              if (time_10.length > 1) {
                                                  while (time_10[0] == "-" || time_10[0] == null) {
                                                      time_10.shift();
                                                  }
                                                  if (time_10.length > 1) {
                                                      var str = "";
                                                      str_10 = "<td style='position:relative;'>" + time_10[0] + "<ul>";
                                                      for (j = 0; j < time_10.length; j++) {
                                                          str += "<li>" + time_10[j] + "</li>";
                                                      }
                                                      str_10 += str + "</ul></td>";
                                                  } else {
                                                      str_10 = "<td>" + time_10[0] + "</td>";
                                                  }
                                              } else {
                                                  str_10 = "<td>" + time_10[0] + "</td>";
                                              }


                                              if (time_10_3.length > 1) {
                                                  while (time_10_3[0] == "-" || time_10_3[0] == null) {
                                                      time_10_3.shift();
                                                  }
                                                  if (time_10_3.length > 1) {
                                                      var str = "";
                                                      str_10_3 = "<td style='position:relative;'>" + time_10_3[0] + "<ul>";
                                                      for (j = 0; j < time_10_3.length; j++) {
                                                          str += "<li>" + time_10_3[j] + "</li>";
                                                      }
                                                      str_10_3 += str + "</ul></td>";
                                                  } else {
                                                      str_10_3 = "<td>" + time_10_3[0] + "</td>";
                                                  }
                                              } else {
                                                  str_10_3 = "<td>" + time_10_3[0] + "</td>";
                                              }

                                              if (time_11.length > 1) {
                                                  while (time_11[0] == "-" || time_11[0] == null) {
                                                      time_11.shift();
                                                  }
                                                  if (time_11.length > 1) {
                                                      var str = "";
                                                      str_11 = "<td style='position:relative;'>" + time_11[0] + "<ul>";
                                                      for (j = 0; j < time_11.length; j++) {
                                                          str += "<li>" + time_11[j] + "</li>";
                                                      }
                                                      str_11 += str + "</ul></td>";
                                                  } else {
                                                      str_11 = "<td>" + time_11[0] + "</td>";
                                                  }
                                              } else {
                                                  str_11 = "<td>" + time_11[0] + "</td>";
                                              }


                                              if (time_11_3.length > 1) {
                                                  while (time_11_3[0] == "-" || time_11_3[0] == null) {
                                                      time_11_3.shift();
                                                  }
                                                  if (time_11_3.length > 1) {
                                                      var str = "";
                                                      str_11_3 = "<td style='position:relative;'>" + time_11_3[0] + "<ul>";
                                                      for (j = 0; j < time_11_3.length; j++) {
                                                          str += "<li>" + time_11_3[j] + "</li>";
                                                      }
                                                      str_11_3 += str + "</ul></td>";
                                                  } else {
                                                      str_11_3 = "<td>" + time_11_3[0] + "</td>";
                                                  }
                                              } else {
                                                  str_11_3 = "<td>" + time_11_3[0] + "</td>";
                                              }


                                              if (time_12.length > 1) {
                                                  while (time_12[0] == "-" || time_12[0] == null) {
                                                      time_12.shift();
                                                  }
                                                  if (time_12.length > 1) {
                                                      var str = "";
                                                      str_12 = "<td style='position:relative;'>" + time_12[0] + "<ul>";
                                                      for (j = 0; j < time_12.length; j++) {
                                                          str += "<li>" + time_12[j] + "</li>";
                                                      }
                                                      str_12 += str + "</ul></td>";
                                                  } else {
                                                      str_12 = "<td>" + time_12[0] + "</td>";
                                                  }
                                              } else {
                                                  str_12 = "<td>" + time_12[0] + "</td>";
                                              }


                                              if (time_12_3.length > 1) {
                                                  while (time_12_3[0] == "-" || time_12_3[0] == null) {
                                                      time_12_3.shift();
                                                  }
                                                  if (time_12.length > 1) {
                                                      var str = "";
                                                      str_12_3 = "<td style='position:relative;'>" + time_12_3[0] + "<ul>";
                                                      for (j = 0; j < time_12_3.length; j++) {
                                                          str += "<li>" + time_12_3[j] + "</li>";
                                                      }
                                                      str_12_3 += str + "</ul></td>";
                                                  } else {
                                                      str_12_3 = "<td>" + time_12_3[0] + "</td>";
                                                  }
                                              } else {
                                                  str_12_3 = "<td>" + time_12_3[0] + "</td>";
                                              }


                                              if (time_13.length > 1) {
                                                  while (time_13[0] == "-" || time_13[0] == null) {
                                                      time_13.shift();
                                                  }
                                                  if (time_13.length > 1) {
                                                      var str = "";
                                                      str_13 = "<td style='position:relative;'>" + time_13[0] + "<ul>";
                                                      for (j = 0; j < time_13.length; j++) {
                                                          str += "<li>" + time_13[j] + "</li>";
                                                      }
                                                      str_13 += str + "</ul></td>";
                                                  } else {
                                                      str_13 = "<td>" + time_13[0] + "</td>";
                                                  }
                                              } else {
                                                  str_13 = "<td>" + time_13[0] + "</td>";
                                              }



                                              if (time_13_3.length > 1) {
                                                  while (time_13_3[0] == "-" || time_13_3[0] == null) {
                                                      time_13_3.shift();
                                                  }
                                                  if (time_13_3.length > 1) {
                                                      var str = "";
                                                      str_13_3 = "<td style='position:relative;'>" + time_13_3[0] + "<ul>";
                                                      for (j = 0; j < time_13_3.length; j++) {
                                                          str += "<li>" + time_13_3[j] + "</li>";
                                                      }
                                                      str_13_3 += str + "</ul></td>";
                                                  } else {
                                                      str_13_3 = "<td>" + time_13_3[0] + "</td>";
                                                  }
                                              } else {
                                                  str_13_3 = "<td>" + time_13_3[0] + "</td>";
                                              }


                                              if (time_14.length > 1) {
                                                  while (time_14[0] == "-" || time_14[0] == null) {
                                                      time_14.shift();
                                                  }
                                                  //alert(time_14.length)
                                                  if (time_14.length > 1) {
                                                      var str = "";
                                                      str_14 = "<td style='position:relative;'>" + time_14[0] + "<ul>";
                                                      for (j = 0; j < time_14.length; j++) {
                                                          str += "<li>" + time_14[j] + "</li>";
                                                      }
                                                      str_14 += str + "</ul></td>";
                                                  } else {
                                                      str_14 = "<td>" + time_14[0] + "</td>";
                                                  }
                                              } else {
                                                  str_14 = "<td>" + time_14[0] + "</td>";
                                              }



                                              if (time_14_3.length > 1) {
                                                  while (time_14_3[0] == "-" || time_14_3[0] == null) {
                                                      time_14_3.shift();
                                                  }
                                                  if (time_14_3.length > 1) {
                                                      var str = "";
                                                      str_14_3 = "<td style='position:relative;'>" + time_14_3[0] + "<ul>";
                                                      for (j = 0; j < time_14_3.length; j++) {
                                                          str += "<li>" + time_14_3[j] + "</li>";
                                                      }
                                                      str_14_3 += str + "</ul></td>";
                                                  } else {
                                                      str_14_3 = "<td>" + time_14_3[0] + "</td>";
                                                  }
                                              } else {
                                                  str_14_3 = "<td>" + time_14_3[0] + "</td>";
                                              }
                                              //alert(str_14_3);


                                              if (time_15.length > 1) {
                                                  while (time_15[0] == "-" || time_15[0] == null) {
                                                      time_15.shift();
                                                  }
                                                  if (time_15.length > 1) {
                                                      var str = "";
                                                      str_15 = "<td style='position:relative;'>" + time_15[0] + "<ul>";
                                                      for (j = 0; j < time_15.length; j++) {
                                                          str += "<li>" + time_15[j] + "</li>";
                                                      }
                                                      str_15 += str + "</ul></td>";
                                                  } else {
                                                      str_15 = "<td>" + time_15[0] + "</td>";
                                                  }
                                              } else {
                                                  str_15 = "<td>" + time_15[0] + "</td>";
                                              }


                                              if (time_15_3.length > 1) {
                                                  while (time_15_3[0] == "-" || time_15_3[0] == null) {
                                                      time_15_3.shift();
                                                  }
                                                  if (time_15_3.length > 1) {
                                                      var str = "";
                                                      str_15_3 = "<td style='position:relative;'>" + time_15_3[0] + "<ul>";
                                                      for (j = 0; j < time_15_3.length; j++) {
                                                          str += "<li>" + time_15_3[j] + "</li>";
                                                      }
                                                      str_15_3 += str + "</ul></td>";
                                                  } else {
                                                      str_15_3 = "<td>" + time_15_3[0] + "</td>";
                                                  }
                                              } else {
                                                  str_15_3 = "<td>" + time_15_3[0] + "</td>";
                                              }


                                              if (time_16.length > 1) {
                                                  while (time_16[0] == "-" || time_16[0] == null) {
                                                      time_16.shift();
                                                  }
                                                  if (time_16.length > 1) {
                                                      var str = "";
                                                      str_16 = "<td style='position:relative;'>" + time_16[0] + "<ul>";
                                                      for (j = 0; j < time_16.length; j++) {
                                                          str += "<li>" + time_16[j] + "</li>";
                                                      }
                                                      str_16 += str + "</ul></td>";
                                                  } else {
                                                      str_16 = "<td>" + time_16[0] + "</td>";
                                                  }
                                              } else {
                                                  str_16 = "<td>" + time_16[0] + "</td>";
                                              }



                                              if (time_16_3.length > 1) {
                                                  while (time_16_3[0] == "-" || time_16_3[0] == null) {
                                                      time_16_3.shift();
                                                  }
                                                  if (time_16_3.length > 1) {
                                                      var str = "";
                                                      str_16_3 = "<td style='position:relative;'>" + time_16_3[0] + "<ul>";
                                                      for (j = 0; j < time_16_3.length; j++) {
                                                          str += "<li>" + time_16_3[j] + "</li>";
                                                      }
                                                      str_16_3 += str + "</ul></td>";
                                                  } else {
                                                      str_16_3 = "<td>" + time_16_3[0] + "</td>";
                                                  }
                                              } else {
                                                  str_16_3 = "<td>" + time_16_3[0] + "</td>";
                                              }



                                              if (time_17.length > 1) {
                                                  while (time_17[0] == "-" || time_17[0] == null) {
                                                      time_17.shift();
                                                  }
                                                  debugger;
                                                  if (time_17.length > 1) {
                                                      var str = "";
                                                      str_17 = "<td style='position:relative;'>" + time_17[0] + "<ul>";
                                                      for (j = 0; j < time_17.length; j++) {
                                                          str += "<li>" + time_17[j] + "</li>";
                                                      }
                                                      str_17 += str + "</ul></td>";
                                                  } else {
                                                      str_17 = "<td>" + time_17[0] + "</td>";
                                                  }
                                              } else {
                                                  str_17 = "<td>" + time_17[0] + "</td>";
                                              }



                                              if (time_17_3.length > 1) {
                                                  while (time_17_3[0] == "-" || time_17_3[0] == null) {
                                                      time_17_3.shift();
                                                  }
                                                  if (time_17_3.length > 1) {
                                                      var str = "";
                                                      str_17_3 = "<td style='position:relative;'>" + time_17_3[0] + "<ul>";
                                                      for (j = 0; j < time_17_3.length; j++) {
                                                          str += "<li>" + time_17_3[j] + "</li>";
                                                      }
                                                      str_17_3 += str + "</ul></td>";
                                                  } else {
                                                      str_17_3 = "<td>" + time_17_3[0] + "</td>";
                                                  }
                                              } else {
                                                  str_17_3 = "<td>" + time_17_3[0] + "</td>";
                                              }


                                              if (time_18.length > 1) {
                                                  while (time_18[0] == "-" || time_18[0] == null) {
                                                      time_18.shift();
                                                  }
                                                  if (time_18.length > 1) {
                                                      var str = "";
                                                      str_18 = "<td style='position:relative;'>" + time_18[0] + "<ul>";
                                                      for (j = 0; j < time_18.length; j++) {
                                                          str += "<li>" + time_18[j] + "</li>";
                                                      }
                                                      str_18 += str + "</ul></td>";
                                                  } else {
                                                      str_18 = "<td>" + time_18[0] + "</td>";
                                                  }
                                              } else {
                                                  str_18 = "<td>" + time_18[0] + "</td>";
                                              }


                                              if (time_18_3.length > 1) {
                                                  while (time_18_3[0] == "-" || time_18_3[0] == null) {
                                                      time_18_3.shift();
                                                  }
                                                  if (time_18_3.length > 1) {
                                                      var str = "";
                                                      str_18_3 = "<td style='position:relative;'>" + time_18_3[0] + "<ul>";
                                                      for (j = 0; j < time_18_3.length; j++) {
                                                          str += "<li>" + time_18_3[j] + "</li>";
                                                      }
                                                      str_18_3 += str + "</ul></td>";
                                                  } else {
                                                      str_18_3 = "<td>" + time_18_3[0] + "</td>";
                                                  }
                                              } else {
                                                  str_18_3 = "<td>" + time_18_3[0] + "</td>";
                                              }



                                              if (time_19.length > 1) {
                                                  while (time_19[0] == "-" || time_19[0] == null) {
                                                      time_19.shift();
                                                  }
                                                  if (time_19.length > 1) {
                                                      var str = "";
                                                      str_19 = "<td style='position:relative;'>" + time_19[0] + "<ul>";
                                                      for (j = 0; j < time_19.length; j++) {
                                                          str += "<li>" + time_19[j] + "</li>";
                                                      }
                                                      str_19 += str + "</ul></td>";
                                                  } else {
                                                      str_19 = "<td>" + time_19[0] + "</td>";
                                                  }
                                              } else {
                                                  str_19 = "<td>" + time_19[0] + "</td>";
                                              }


                                              if (time_19_3.length > 1) {
                                                  while (time_19_3[0] == "-" || time_19_3[0] == null) {
                                                      time_19_3.shift();
                                                  }
                                                  if (time_19_3.length > 1) {
                                                      var str = "";
                                                      str_19_3 = "<td style='position:relative;'>" + time_19_3[0] + "<ul>";
                                                      for (j = 0; j < time_19_3.length; j++) {
                                                          str += "<li>" + time_19_3[j] + "</li>";
                                                      }
                                                      str_19_3 += str + "</ul></td>";
                                                  } else {
                                                      str_19_3 = "<td>" + time_19_3[0] + "</td>";
                                                  }
                                              } else {
                                                  str_19_3 = "<td>" + time_19_3[0] + "</td>";
                                              }





                                              if (time_20.length > 1) {
                                                  while (time_20[0] == "-" || time_20[0] == null) {
                                                      time_20.shift();
                                                  }
                                                  if (time_20.length > 1) {
                                                      var str = "";
                                                      str_20 = "<td style='position:relative;'>" + time_20[0] + "<ul>";
                                                      for (j = 0; j < time_20.length; j++) {
                                                          str += "<li>" + time_20[j] + "</li>";
                                                      }
                                                      str_20 += str + "</ul></td>";
                                                  } else {
                                                      str_20 = "<td>" + time_20[0] + "</td>";
                                                  }
                                              } else {
                                                  str_20 = "<td>" + time_20[0] + "</td>";
                                              }


                                              if (time_20_3.length > 1) {
                                                  while (time_20_3[0] == "-" || time_20_3[0] == null) {
                                                      time_20_3.shift();
                                                  }
                                                  if (time_20_3.length > 1) {
                                                      var str = "";
                                                      str_20_3 = "<td style='position:relative;'>" + time_20_3[0] + "<ul>";
                                                      for (j = 0; j < time_20_3.length; j++) {
                                                          str += "<li>" + time_20_3[j] + "</li>";
                                                      }
                                                      str_20_3 += str + "</ul></td>";
                                                  } else {
                                                      str_20_3 = "<td>" + time_20_3[0] + "</td>";
                                                  }
                                              } else {
                                                  str_20_3 = "<td>" + time_20_3[0] + "</td>";
                                              }

                                              //alert(time_12_3);
                                              //console.log(time_15);

                                              htmlString += str_6 + str_6_3 + str_7 + str_7_3 + str_8 + str_8_3 + str_9 + str_9_3 + str_10 + str_10_3 + str_11 + str_11_3 + str_12 + str_12_3 + str_13 + str_13_3 +
                                                  str_14 + str_14_3 + str_15 + str_15_3 + str_16 + str_16_3 + str_17 + str_17_3 + str_18 + str_18_3 + str_19 + str_19_3 + str_20 + str_20_3 + "</tr>";

                                          });

                                          //console.log(per_time);
                                          //console.log(per_act);

                                          $("#sales_activity_report").html(htmlString);
                                          //alert(htmlString);
                                          //}, error: function (data) {
                                          //    alert("Error: "+data);
                                          //}
                                      });

                                  });

                                </script>
                            </tbody>
                        </table>
                    </div>
                </div>

            </div>


            <%--            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <div class="panel-heading UnApprovedHeading" style="background-image: linear-gradient(90deg,#a31684,#00d4ff);" id="">
                        Covered Area
                        <div class="box-tools pull-right">                           
                        </div>
                    </div>
                    <style>
                        /* Set the size of the div element that contains the map */
                        #map {
                        height: 400px;  /* The height is 400 pixels */
                        width: 100%;  /* The width is the width of the web page */
                        }
                    </style>
                    <div id="map"></div>
                    <script>



                        var loc = [];
                        var img = [];
                        var partyName = [];
                        var mobile = [];
                        var address = [];

                        //var contentString = [];
                        $.ajax({
                            type: "POST",
                            url: '<%= ResolveUrl("And_Sync.asmx/Dashboard_Retailermasterdata") %>',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                jsdata1 = JSON.parse(data.d);
                                $.each(jsdata1, function (key1, value1) {
                                    loc.push({ position: { lat: parseFloat(value1.Latitude), lng: parseFloat(value1.Longitude) }, type: "lost" });
                                    img[key1] = value1.imgpath;
                                    partyName[key1] = value1.Partyname;
                                    mobile[key1] = value1.Mobile;
                                    address[key1] = value1.GoogleAddress;
                                });
                                //alert("fine");
                                initMap(loc);
                            }, error: function (data) {
                                //alert(data);
                            }
                        });

                        var map;
                        function initMap(loc) {
                            map = new google.maps.Map(
                                document.getElementById('map'),
                                { center: new google.maps.LatLng(23.563857, 77.515132), zoom: 4 });

                            var image = {
                                url: '<%= ResolveUrl("img/mark2.png") %>',
                            };
                            var features = loc;
                            var infoWindow = new google.maps.InfoWindow();
                            // Create markers.
                            for (var i = 0; i < features.length; i++) {
                                var marker = new google.maps.Marker({
                                    position: features[i].position,
                                    map: map,
                                    icon: image
                                    //title: "Party Name: " + partyName[i] + '<br><img src=' + img[i] + ' alt="Image" height="100" width="100"><br>Mobile: ' + mobile[i] + "<br>Address: " + address[i]
                                });
                                (function (marker) {
                                    var contentString = '<div style="display:inline-flex"><img src=' + img[i] + ' alt="Image" style="padding:10px;padding-bottom:5px;padding-right:0;width:100px;"></div><div style="float:right;"> <div style="font-weight:bold;font-size:15px;text-align:left;width:100%;padding:15px 10px 0 10px; padding-bottom:0;">' + partyName[i] + '</div><div style="padding-top:5px;padding-left:10px">Contact Number:</div><div style="font-size:12px;font-weight:bold;padding-left:10px">' + mobile[i] + '</div><div style="padding-left:10px">Address:</div><div style="font-size:12px;font-weight:bold;padding-left:10px;max-width:350px;padding-bottom:10px">' + address[i] + "</div></div></div>";
                                    // Attaching a click event to the current marker
                                    google.maps.event.addListener(marker, "mouseover", function (e) {
                                        infoWindow.setContent(contentString);
                                        infoWindow.open(map, marker);

                                    });

                                    google.maps.event.addListener(marker, "mouseout", function (e) {
                                        infoWindow.close();

                                    });

                                })(marker);


                                //var infowindow = new google.maps.InfoWindow({
                                //    content: contentString
                                //});
                                //marker.addListener('mouseover', function () {
                                //    infowindow.open(map, marker);
                                //});
                                //marker.addListener('mouseout', function () {
                                //    infowindow.close();
                                //});
                                //console.log(Object.keys(icons));
                            };


                        }
                    </script>

                    <!--Load the API from the specified URL
                    * The async attribute allows the browser to render the page while the API loads
                    * The key parameter will contain your own API key (which is not needed for this tutorial)
                    * The callback parameter executes the initMap() function
                    -->
                    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBvCa97dqF2Pao5NPBguRd-bNQgB4VAqLw&callback=initMap"></script>
                </div>
            </div>--%>
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


        .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td {
            border-color: #91b3dc;
        }

        table.table-bordered {
            border: 2px solid #91b3dc;
        }

        td:hover {
            background-color: #3dea3d;
            color: #fff;
        }
    </style>

</asp:Content>
