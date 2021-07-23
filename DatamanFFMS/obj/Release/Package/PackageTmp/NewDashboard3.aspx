<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="NewDashboard3.aspx.cs" Inherits="AstralFFMS.NewDashboard3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

    <%--<script src="plugins/fastclick/fastclick.min.js" type="text/javascript"></script>--%>
    <!-- AdminLTE App -->
    <%--<script src="dist/js/app.min.js" type="text/javascript"></script>--%>
    <!-- AdminLTE for demo purposes -->
    <script src="dist/js/demo.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="messageNotification">
        <asp:Label ID="lblmasg" runat="server"></asp:Label>
    </div>
    <script>
        var attObj //= new Array(31);
        function check()
        {
            debugger;
           
            var cmbPerson = document.getElementById("ContentPlaceHolder1_cmbPerson");
          
            var smId = cmbPerson.options[cmbPerson.selectedIndex].value;
          
            var calendar = $find("<%= RadCalendar1.ClientID %>");
            var month = calendar.get_focusedDate()[1];
            var year = calendar.get_focusedDate()[0];
            if (smId) {
                $.ajax({
                    type: "POST",
                    url: "NewDashboard3.aspx/GetDetail",
                    data: '{smId: ' + smId + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var mn = response.d;
                        console.log(mn);
                        $("#lblSMName").html(mn[0]);
                        $("#lblSMDName").html(mn[1]);
                        $("#lblReporting").html(mn[2]);
                        $("#lblEmail").html(mn[3]);
                        $("#lblMobile").html(mn[4]);
                        $("#lblIMEI").html(mn[5]);
                        $("#lblCity").html(mn[6]);
                        $("#lblAddress").html(mn[7]);
                        setAttObj(smId, month, year);
                    },
                    failure: function (response) {
                        alert(response.d);
                    },
                    error: function (response) {
                        alert(response.d);
                    }
                });
            }
        }
        function OnClientSelectedIndexChanged(sender, eventArgs) {     
            var cmbPerson = document.getElementById("ContentPlaceHolder1_cmbPerson");
            var smId = cmbPerson.options[cmbPerson.selectedIndex].value;
            var calendar = $find("<%= RadCalendar1.ClientID %>");
            var month = calendar.get_focusedDate()[1];
            var year = calendar.get_focusedDate()[0];
            if (smId) {
                $.ajax({
                    type: "POST",
                    url: "NewDashboard3.aspx/GetDetail",
                    data: '{smId: ' + smId + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var mn = response.d;
                        console.log(mn);
                        $("#lblSMName").html(mn[0]);
                        $("#lblSMDName").html(mn[1]);
                        $("#lblReporting").html(mn[2]);
                        $("#lblEmail").html(mn[3]);
                        $("#lblMobile").html(mn[4]);
                        $("#lblIMEI").html(mn[5]);
                        $("#lblCity").html(mn[6]);
                        $("#lblAddress").html(mn[7]);
                        setAttObj(smId, month, year);
                    },
                    failure: function (response) {
                        alert(response.d);
                    },
                    error: function (response) {
                        alert(response.d);
                    }
                });
            }
        }
        function setAttObj(smId, month, year) {
            $.ajax({
                type: "POST",
                url: "NewDashboard3.aspx/BindGrid",
                data: '{smId: ' + smId + ', Month: ' + month + ', year: ' + year + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var mn = response.d;
                    attObj = mn;
                    var calendar = $find("<%= RadCalendar1.ClientID %>");
                    calendar._moveToDate(calendar.get_focusedDate(), true);
                },
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
        }
        function OnDayRender(calendarInstance, args) {
            var date = new Date(args.get_date()[0], args.get_date()[1] - 1, args.get_date()[2]);
            //if ((date.getDate() == selectedDay) && (date.getMonth() == (calendarInstance.get_focusedDate()[1] - 1))) {
            console.log(attObj);
            if (attObj) {
                if ((attObj[date.getDate()-1] != null) && (date.getMonth() == (calendarInstance.get_focusedDate()[1] - 1))) {
                    var cssclass = "";
                    switch (attObj[date.getDate()-1]) {
                        case "P":
                            cssclass = "present";
                            break;
                        case "E":
                            cssclass = "dsrfilled";
                            break;
                        case "E, L/A":
                            cssclass = "dsrfilled";
                            break;
                        case "L/A":
                            cssclass = "leaveappr";
                            break;
                        case "L":
                            cssclass = "leave";
                            break;
                        case "H":
                            cssclass = "holiday";
                            break;
                        case "Off":
                            cssclass = "weekoff";
                            break;
                        case "L/R":
                            cssclass = "leaverjct";
                            break;
                        case "E/R":
                            cssclass = "dsrrjct";
                            break;
                        case "FHL":
                            cssclass = "firsthl";
                            break;
                        case "FHL/A":
                            cssclass = "firsthlappr";
                            break;
                        case "FHL/R":
                            cssclass = "firsthlrjct";
                            break;
                        case "SHL":
                            cssclass = "secondhl";
                            break;
                        case "SHL/A":
                            cssclass = "secondhlappr";
                            break;
                        case "SHL/R":
                            cssclass = "secondhlrjct";
                            break;
                    }
                    args.get_cell().innerHTML = '<div class="radTemplateDay_Special ' + cssclass + ' "><div class="rcTemplate rcDayDate">' + date.getDate() + "<br>" + attObj[date.getDate()-1] + '</div></div>';//date.getDate()+"<br>"+selectedDayText;
                    // args.get_cell().style.backgroundColor = selectedDayColor;
                }
            }
        }

        function CalendarViewChanged(sender, eventArgs) {
            var calendar = $find("<%= RadCalendar1.ClientID %>");
            var month = calendar.get_focusedDate()[1];
            var year = calendar.get_focusedDate()[0];
            var cmbPerson = document.getElementById("ContentPlaceHolder1_cmbPerson");
            var smId = cmbPerson.options[cmbPerson.selectedIndex].value;

<%--            var cmbPerson = $find("<%= cmbPerson.ClientID %>");
            var smId = cmbPerson.get_value();--%>
            if (smId)
                setAttObj(smId, month, year);
        }

    </script>
    <div class="content">
        <div class="row" style="margin-bottom: 15px;">
            <div class="col-md-4 col-lg-4">
                <%--<telerik:RadComboBox ID="cmbPerson" runat="server" DataValueField="SMId" DataTextField="SMName" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" DataSourceID="SqlDataSource1" MarkFirstMatch="true" Filter="Contains" EmptyMessage="Person"></telerik:RadComboBox>--%>
                 <asp:DropDownList ID="cmbPerson" runat="server" DataValueField="SMId" DataTextField="SMName" CssClass="form-control" onchange="check(this);" DataSourceID="SqlDataSource1"></asp:DropDownList>
            </div>
            <div style="float: right">
                <asp:Button Style="margin-right: 5px;" type="button" ID="AnalyticsBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click"  />
            </div>
        </div>
        <div class="row">
            <div class="col-md-4  col-lg-4">
                <div class="panel panel-default">
                    <div class="panel-heading"><span id="lblSMName">&nbsp;</span></div>
                    <div class="panel-body">
                        <ul class="colornote">
                            <li>Name: <span id="lblSMDName"></span></li>
                            <li>Reporting: <span id="lblReporting"></span></li>
                            <li>Email: <span id="lblEmail"></span></li>
                            <li>Mobile: <span id="lblMobile"></span></li>
                            <li>IMEI: <span id="lblIMEI"></span></li>
                            <li>City: <span id="lblCity"></span></li>
                            <li>Address: <span id="lblAddress"></span></li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="col-md-8  col-lg-8">
                <div class="demo-container no-bg" style="max-width: 757px" runat="server" id="containerDiv">
                    <telerik:RadCalendar RenderMode="Lightweight" ID="RadCalendar1" runat="server" AutoPostBack="false" Skin="Special"
                        EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" EnableMonthYearFastNavigation="false"
                        DayNameFormat="Short" ShowRowHeaders="false" ShowOtherMonthsDays="false">
                        <%--OnDefaultViewChanged="RadCalendar1_DefaultViewChanged"--%>
                        <ClientEvents OnDayRender="OnDayRender" OnCalendarViewChanged="CalendarViewChanged" />
                        <HeaderTemplate>
                            <asp:Image ID="HeaderImage" runat="server" Width="757" Height="94" Style="display: block"></asp:Image>
                        </HeaderTemplate>
                        <FooterTemplate>
                            <div class="container-fluid">
                                <div class="row colorpanel">
                                    <div class="col-lg-2 presentdiv"><span>P - Present</span></div>
                                    <div class="col-lg-2 dsrdiv">
                                        <span>E - Dsr Entry<br />
                                            E, L/A</span>
                                    </div>
                                    <div class="col-lg-2 leavediv">
                                        <span>L - Leave<br />
                                            FHL - Half Day Leave<br />
                                            SHL - Second Day Leave</span>
                                    </div>
                                    <div class="col-lg-2 offdiv">
                                        <span>H - Holiday<br />
                                            Off - Week Off</span>
                                    </div>
                                    <div class="col-lg-2 rejectdiv">
                                        <span>E/R - Dsr Rejected<br />
                                            L/R - Leave Rejected<br />
                                            FHL/R - Half Day Leave Rejected<br />
                                            SHL/R - Second Day Leave Rejected<br />
                                        </span>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </div>
                            <%--<asp:Image ID="FooterImage" runat="server" Width="757" Height="70" Style="display: block"></asp:Image>--%>
                        </FooterTemplate>
                        <%-- <SpecialDays>
                <telerik:RadCalendarDay Date="2009/06/12" Repeatable="DayInMonth" TemplateID="DateTemplate">
                </telerik:RadCalendarDay>
                <telerik:RadCalendarDay Date="2009/06/19" Repeatable="DayInMonth" TemplateID="DateTemplate">
                </telerik:RadCalendarDay>
                <telerik:RadCalendarDay Date="2009/06/17" Repeatable="DayInMonth" TemplateID="MortgageTemplate">
                </telerik:RadCalendarDay>
                <telerik:RadCalendarDay Date="2009/06/8" Repeatable="DayAndMonth" TemplateID="BirthdayTemplate">
                </telerik:RadCalendarDay>
                <telerik:RadCalendarDay Date="2009/08/7" Repeatable="DayAndMonth" TemplateID="BirthdayTemplate">
                </telerik:RadCalendarDay>
                <telerik:RadCalendarDay Date="2009/10/8" Repeatable="DayAndMonth" TemplateID="BirthdayTemplate">
                </telerik:RadCalendarDay>
                <telerik:RadCalendarDay Date="2009/12/23" Repeatable="DayAndMonth" TemplateID="BirthdayTemplate">
                </telerik:RadCalendarDay>
                <telerik:RadCalendarDay Date="2010/2/14" Repeatable="DayAndMonth" TemplateID="BirthdayTemplate">
                </telerik:RadCalendarDay>
            </SpecialDays>--%>
                        <CalendarDayTemplates>
                            <telerik:DayTemplate ID="DateTemplate" runat="server">
                                <Content>
                                    <div class="rcTemplate rcDayDate">
                                        date
                                    </div>
                                </Content>
                            </telerik:DayTemplate>
                            <telerik:DayTemplate ID="MortgageTemplate" runat="server">
                                <Content>
                                    <div class="rcTemplate rcDayMortgage">
                                        mortgage
                                    </div>
                                </Content>
                            </telerik:DayTemplate>
                            <telerik:DayTemplate ID="BirthdayTemplate" runat="server">
                                <Content>
                                    <div class="rcTemplate rcDayBirthday">
                                        birthday
                                    </div>
                                </Content>
                            </telerik:DayTemplate>
                        </CalendarDayTemplates>
                    </telerik:RadCalendar>
                </div>
            </div>
        </div>
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
    <style>
        /*Telerik RadCalendar Special skin*/
        body .rcMainTable {
            float: none !important;
        }

        div.RadCalendar_Special {
            border-collapse: separate;
            border: 0;
            background: #272727;
            color: #fff;
            font: 12px "segoe ui",arial,sans-serif;
            width: 757px;
        }

        /*titlebar*/

        .RadCalendar_Special .rcTitlebar {
            width: 100%;
            border: 0;
            padding: 10px 0 6px 0;
            background: 0 0 no-repeat url(images/rcTitlebar.gif);
        }

        .RadCalendar span.rcTitle {
            display: block;
        }

        .RadCalendar_Special .rcTitlebar {
            width: 100%;
            border-collapse: separate;
            border: 0;
            font: 14px "segoe ui",arial,sans-serif;
        }

            .RadCalendar_Special .rcTitlebar a {
                border: 0;
                padding: 0;
                text-align: center;
                vertical-align: middle;
            }

            .RadCalendar_Special .rcTitlebar .rcTitle {
                width: 100%;
                cursor: default;
                text-align: center;
            }

            .RadCalendar_Special .rcTitlebar .rcPrev,
            .RadCalendar_Special .rcTitlebar .rcNext,
            .RadCalendar_Special .rcTitlebar .rcFastPrev,
            .RadCalendar_Special .rcTitlebar .rcFastNext {
                display: block;
                width: 17px;
                height: 16px;
                overflow: hidden;
                margin: 0 6px;
                background: transparent url(images/specialsprite.gif) no-repeat;
                text-indent: -2222px;
                text-decoration: none;
                color: #ccc;
                float: left;
            }

            .RadCalendar_Special .rcTitlebar .rcNext,
            .RadCalendar_Special .rcTitlebar .rcFastNext {
                float: right;
            }

            .RadCalendar_Special .rcTitlebar .rcFastPrev {
                background-position: 0 0;
            }

                .RadCalendar_Special .rcTitlebar .rcFastPrev:hover {
                    background-position: 0 -50px;
                }

            .RadCalendar_Special .rcTitlebar .rcPrev {
                background-position: 0 -100px;
            }

                .RadCalendar_Special .rcTitlebar .rcPrev:hover {
                    background-position: 0 -150px;
                }

            .RadCalendar_Special .rcTitlebar .rcNext {
                background-position: 0 -200px;
            }

                .RadCalendar_Special .rcTitlebar .rcNext:hover {
                    background-position: 0 -250px;
                }

            .RadCalendar_Special .rcTitlebar .rcFastNext {
                background-position: 0 -300px;
            }

                .RadCalendar_Special .rcTitlebar .rcFastNext:hover {
                    background-position: 0 -350px;
                }

        .RadCalendar_Special .rcMain {
            width: 100%;
            border: 0;
            padding: 0;
        }

        .RadCalendar_Special .rcMainTable {
            border-collapse: separate;
            border: 0;
            width: 100%;
            font: 12px/17px "segoe ui",arial,sans-serif;
        }

        /*header, footer*/

        .RadCalendar_Special .rcHeader,
        .RadCalendar_Special .rcFooter {
            border: 0;
            padding: 0;
        }

        /*week numbers and days*/

        .RadCalendar_Special .rcWeek th,
        .RadCalendar_Special .rcRow th {
            border: 0;
            font-weight: normal;
            vertical-align: middle;
            cursor: default;
            color: #c8c8c8;
        }

        .RadCalendar_Special .rcWeek th {
            line-height: 28px;
            padding-bottom: 7px;
            background: 0 -600px repeat-x url(images/specialsprite.gif);
        }

        /*date cells*/

        .RadCalendar_Special .rcRow td {
            padding: 0 3px 3px 0;
            text-align: right;
        }

        .RadCalendar_Special .rcRow .rcOtherMonth {
            background: none;
            font: 1px/1px sans-serif;
        }

        .RadCalendar_Special .rcRow a,
        .RadCalendar_Special .rcRow span,
        .RadCalendar_Special .rcTemplate {
            display: block;
            width: 95px;
            height: 47px;
            padding: 8px 8px 0 0;
            background: 0 -400px no-repeat url(images/specialsprite.gif);
            text-decoration: none;
        }

        .RadCalendar_Special .rcTemplate {
            background: none;
            color: #f90;
        }

        .RadCalendar_Special .rcRow a {
            color: #fff;
        }

        .RadCalendar_Special .rcRow .rcHover a,
        .RadCalendar_Special .rcRow .rcSelected a {
            background-position: 0 -500px;
            color: #000;
        }

        /*special days*/

        .RadCalendar_Special .rcRow .rcHover .rcTemplate,
        .RadCalendar_Special .rcRow .rcSelected .rcTemplate {
            color: #fc0;
        }

        .RadCalendar_Special .rcRow .rcDayDate {
            background: 0 0 no-repeat url(images/t_date.gif);
        }

        .RadCalendar_Special .rcRow .rcDayMortgage {
            background: 0 0 no-repeat url(images/t_mortgage.gif);
        }

        .RadCalendar_Special .rcRow .rcDayBirthday {
            background: 0 0 no-repeat url(images/t_birthday.gif);
        }

        .colornote li {
            list-style: none;
        }

        .colornote {
            padding: 0;
        }

            .colornote li {
                list-style: none;
                padding: 12px 0;
                border-bottom: 1px solid #c1c1c1;
                font-weight: bold;
            }

                .colornote li:last-child {
                    border: none;
                }

                .colornote li span {
                    font-weight: normal;
                    margin-left: 5px;
                }

        .presentcol {
            background-color: #028712;
        }

        .present .rcTemplate {
            color: #fff;
        }

        .dsrfilledcol {
            background-color: #2395e0;
        }

        .dsrfilled .rcTemplate {
            color: #fff;
        }

        .leaveapprcol {
            background-color: #fcb102;
        }

        .leaveappr .rcTemplate {
            color: #fff;
        }

        .leavecol {
            background-color: #fcb102;
        }

        .leave .rcTemplate {
            color: #fff;
        }

        .holidaycol {
            background-color: #f4e5b0;
        }

        .holiday .rcTemplate {
            color: #fff;
        }

        .weekoffcol {
            background-color: #f4e5b0;
        }

        .weekoff .rcTemplate {
            color: #fff;
        }

        .leaverjctcol {
            background-color: #fc2537;
        }

        .leaverjct .rcTemplate {
            color: #fff;
        }

        .dsrrjctcol {
            background-color: #fc2537;
        }

        .dsrrjct .rcTemplate {
            color: #fff;
        }

        .firsthlcol {
            background-color: #fcb102;
        }

        .firsthl .rcTemplate {
            color: #fff;
        }

        .firsthlapprcol {
            background-color: #fcb102;
        }

        .firsthlappr .rcTemplate {
            color: #fff;
        }

        .firsthlrjctcol {
            background-color: #fc2537;
        }

        .firsthlrjct .rcTemplate {
            color: #fff;
        }

        .secondhlcol {
            background-color: #fcb102;
        }

        .secondhl .rcTemplate {
            color: #fff;
        }

        .secondhlapprcol {
            background-color: #fcb102;
        }

        .secondhlappr .rcTemplate {
            color: #fff;
        }

        .secondhlrjctcol {
            background-color: #fc2537;
        }

        .secondhlrjct .rcTemplate {
            color: #fff;
        }

        .leaveappr .rcDayDate, .leave .rcDayDate, .firsthl .rcDayDate, .secondhl .rcDayDate {
            background: 0 0 no-repeat url(images/t_leave_date.gif) !important;
        }

        .dsrfilled .rcDayDate {
            background: 0 0 no-repeat url(images/t_dsr_date.gif) !important;
        }

        .present .rcDayDate {
            background: 0 0 no-repeat url(images/t_present_date.gif) !important;
        }

        .leaverjct .rcDayDate, .dsrrjct .rcDayDate, .firsthlrjct .rcDayDate, .secondhlrjct .rcDayDate {
            background: 0 0 no-repeat url(images/t_reject_date.gif) !important;
        }

        .holiday .rcDayDate, .weekoff .rcDayDate {
            background: 0 0 no-repeat url(images/t_off_date.gif) !important;
        }

        .colorpanel > div {
            height: 115px;
            width: 20%;
        }

            .colorpanel > div.presentdiv {
                background-color: #189018;
            }

            .colorpanel > div.leavediv {
                background-color: #BF4900;
            }

            .colorpanel > div.offdiv {
                background-color: #858500;
            }

            .colorpanel > div.dsrdiv {
                background-color: #5757F2;
            }

            .colorpanel > div.rejectdiv {
                background-color: #FF1F1F;
            }

            .colorpanel > div > span {
                display: block;
                top: 20%;
                text-align: center;
                margin: 0 auto;
            }

            .colorpanel > div.presentdiv > span {
                margin-top: 48px;
            }

            .colorpanel > div.dsrdiv > span {
                margin-top: 39px;
            }

            .colorpanel > div.leavediv > span {
                margin-top: 24px;
            }

            .colorpanel > div.offdiv > span {
                margin-top: 42px;
            }

            .colorpanel > div.rejectdiv > span {
                margin-top: 6px;
            }
    </style>
</asp:Content>
