<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="NewDashboard4.aspx.cs" Inherits="AstralFFMS.NewDashboard4" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="UTF-8">
    <title>Home</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <!-- Bootstrap 3.3.4 -->
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
        <link href="Content/bootstrap-responsive.css" rel="stylesheet" />
        <link href="Content/bootstrap-responsive.min.css" rel="stylesheet" />
        <!-- Tell the browser to be responsive to screen width -->
        <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
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
     <link href="Content/style.css" rel="stylesheet" />
    <%-- Added--%>
    <link href="jqwidgets/styles/jqx.base.css" rel="stylesheet" />
    <script src="jqwidgets/jqxcore.js" type="text/javascript"></script>
      <link href="jqwidgets/styles/jqtimepicker.css" rel="stylesheet" />
        <script src="jqwidgets/jqtimepicker.js" type="text/javascript"></script>
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
 <%--   <script src="dist/js/demo.js" type="text/javascript"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <script>
        function updateStats() {
            debugger;
            var cmbPerson = document.getElementById("ContentPlaceHolder1_cmbPerson");
            var smId = cmbPerson.options[cmbPerson.selectedIndex].value;
            //var smId = cmbPerson.get_value();
            var calendar = document.getElementById("ContentPlaceHolder1_txtDate");
            var dt = new Date(calendar.value);
           
            //var date1 = calendar.get_selectedDate();
            if (smId && dt != null) {
                var day = dt.getDate();
                var month = dt.getMonth() + 1;
                var year = dt.getFullYear();

                $.ajax({
                    type: "POST",
                    url: "NewDashboard4.aspx/GetAttendanceStats",
                    data: '{smId: ' + smId + ', day: ' + day + ', month: ' + month + ', year: ' + year + ', chartselector: "' + $(<%= chartselector.ClientID %>).val() + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var t = JSON.parse(response.d);
                        var newDataSource = t.data;
                        var RadHtmlChart1 = $find('<%=PieChart1.ClientID %>');
                        RadHtmlChart1.set_dataSource(newDataSource);
                        RadHtmlChart1.set_transitions(true);
                        RadHtmlChart1.repaint();
                      
                        if (t.noData == true) {
                            var lk = document.getElementById("ctl00_ContentPlaceHolder1_levelGrid1");
                            var levelGrid1 = $find('<%=levelGrid1.ClientID %>');
                            $find("<%=levelGrid1.ClientID%>").get_element().style.display = "none";
                            levelGrid1.set_visible(false);
                        } else {
                            if ($(<%= chartselector.ClientID %>).val() != "") {
                                $("#btn1").click();
                            }
                        }
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
           
            updateStats();
           
        }
        function OnDateSelected(sender, eventArgs) {
            updateStats();
          
        }
    </script>
    <style>
        .RadAjax_MyCustomSkin .raDiv
        {
        background-image: url('Ajax/loading.gif');
        }
 
        .RadAjax_MyCustomSkin .raColor
        {
        background-color: #dbeddc;
        color: black;
        }
 
        .RadAjax_MyCustomSkin .raTransp
        {
        opacity: 0.7;
        -moz-opacity: 0.7;
        filter: alpha(opacity=70);
        }

        div.RadGrid_MySkin th.rgHeader 
    { 
        background-image: none; 
        background-color: rgb(60,141,188); 
    } 
        .RadGrid .rgHeader, .RadGrid th.rgResizeCol {
    border-bottom: 1px solid transparent;
    color: white;
    font-weight: normal;
    padding-bottom: 7px;
    padding-top: 8px;
    text-align: left;
}
        /*.RadGrid_Office2007 .rgRow td, .RadGrid_Office2007 .rgAltRow td, .RadGrid_Office2007 .rgEditRow td, .RadGrid_Office2007 .rgFooter td
      {
          border-bottom-style: none !important;
      }
      .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol
      {
          border-bottom-style: none !important;
      }*/
    </style>
    <div id="chart"></div>
    <script>
       
    </script>
    <section class="content">
        <div id="messageNotification">
            <asp:Label ID="lblmasg" runat="server"></asp:Label>
        </div>
        <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Attendance Status DashBoard</h3>
                        </div>
                        <div class="box-body" id="TransStateDiv" runat="server">
                            <div class="row">
                                <!-- left column -->
                                <div class="col-md-12">
                                    <!-- general form elements -->
                                    <div class="box box-default">
                                        <div class="box-header">
                                            <div style="float: right">
                                              <asp:Button Style="margin-right: 5px;" type="button" ID="Button11" runat="server" Text="Back" class="btn btn-primary" OnClick="btnAttaBack_Click" />

                                            </div>
                                        </div>
                                        <!-- /.box-header -->
                                        <!-- form start -->

                                        <div class="box-body">

                                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <label for="exampleInputEmail1">Sales Person:</label>
                                                <asp:DropDownList ID="cmbPerson" runat="server" DataValueField="SMId" DataTextField="SMName" CssClass="form-control"  OnClientSelectedIndexChanged="OnClientSelectedIndexChanged"  DataSourceID="SqlDataSource1"></asp:DropDownList>
                                            </div>
                                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                <label for="exampleInputEmail1">Date:</label>
                                                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" OnClientDateSelectionChanged="OnDateSelected" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender2" TargetControlID="txtDate"></ajaxToolkit:CalendarExtender>
                                            </div>
                                        </div>

                                        <div class="box-footer">
                                            <div class="row">
                                                <div class="col-md-12 col-lg-12">
                                                    <div class="demo-container size-wide">
                                                        <telerik:RadHtmlChart OnClientSeriesClicked="test" runat="server" ID="PieChart1" Height="500" Transitions="false">
                                                            <ChartTitle>
                                                                <Appearance Align="Left" Position="Top">
                                                                </Appearance>
                                                            </ChartTitle>
                                                            <Legend>
                                                                <Appearance Position="Right" Visible="true">
                                                                    <TextStyle FontSize="40" Color="#660066" />

                                                                </Appearance>

                                                            </Legend>
                                                            <PlotArea>
                                                                <Series>
                                                                </Series>
                                                            </PlotArea>
                                                        </telerik:RadHtmlChart>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="box-body table-responsive" id="maingrid" runat="server">
                                                    <telerik:RadGrid ID="levelGrid1" EnableEmbeddedSkins="false" Skin="MySkin"
                                                        OnColumnCreated="levelGrid1_ColumnCreated" AlternatingItemStyle-BackColor="#E8E8E8"
                                                        AutoGenerateColumns="true"
                                                        runat="server" OnDetailTableDataBind="levelGrid1_DetailTableDataBind" DataSourceID="SqlDataSource3">
                                                        <MasterTableView HierarchyLoadMode="Client">
                                                            <HeaderStyle ForeColor="White" />
                                                            <ExpandCollapseColumn HeaderStyle-ForeColor ="White"></ExpandCollapseColumn>
                                                            <DetailTables>
                                                                <telerik:GridTableView Width="100%" DataSourceID="SqlDataSource2"
                                                                    runat="server" CommandItemSettings-ShowAddNewRecordButton="false">
                                                                </telerik:GridTableView>
                                                            </DetailTables>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>

                                                </div>
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
                </div>
            </div>
        </div>
    </section>
    <div class="content" style="display:none;">
        <div class="row" style="margin-bottom: 15px;">
            <div class="col-md-12 col-lg-12">
              <%--  <telerik:RadComboBox ID="cmbPerson" runat="server" DataValueField="SMId" DataTextField="SMName" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" DataSourceID="SqlDataSource1" MarkFirstMatch="true" Filter="Contains" EmptyMessage="Person"></telerik:RadComboBox>--%>
               <%-- <telerik:RadDatePicker RenderMode="Lightweight" ID="RadDatePicker1" ClientEvents-OnDateSelected="OnDateSelected" runat="server" DateInput-DateFormat="dd/MMM/yyyy" DateInput-DisplayDateFormat="dd/MMM/yyyy">
                </telerik:RadDatePicker>--%>
            </div>
        </div>
       <%-- <div class="row">
            <div class="col-md-12 col-lg-12">
                <div class="demo-container size-wide">
                    <telerik:RadHtmlChart OnClientSeriesClicked="test" runat="server" ID="PieChart1" Height="500" >
                        <ChartTitle Text="Attendance Status">
                            <Appearance Align="Left" Position="Top">
                            </Appearance>
                        </ChartTitle>
                        <Legend>
                            <Appearance Position="Right" Visible="true">
                                <TextStyle FontSize="40" Color="#660066" />

                            </Appearance>

                        </Legend>
                        <PlotArea>
                            <Series>
                            </Series>
                        </PlotArea>
                    </telerik:RadHtmlChart>
                </div>
            </div>
        </div>--%>
        
    <telerik:RadAjaxManager ID="ajaxMgr" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="maingrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="levelGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btn1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="levelGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" IsSticky="False"
                        Skin="Web20" Style="position: absolute; top: 0px; left: 0px; width: 100%; height: 100%;"
                        EnableSkinTransparency="true" Transparency="0">
                    </telerik:RadAjaxLoadingPanel>
     <%--   <div class="row">
            <div class="col-md-12 col-lg-12" id="maingrid" runat="server">
                <telerik:RadGrid ID="levelGrid1"
                    OnColumnCreated="levelGrid1_ColumnCreated"
                   
                    AutoGenerateColumns="true" 
                    runat="server"  OnDetailTableDataBind="levelGrid1_DetailTableDataBind" DataSourceID="SqlDataSource3">
                    <MasterTableView  HierarchyLoadMode="Client">
                        <DetailTables>
                            <telerik:GridTableView Width="100%" DataSourceID="SqlDataSource2"
                                runat="server" CommandItemSettings-ShowAddNewRecordButton="false">
                            </telerik:GridTableView>
                        </DetailTables>
                    </MasterTableView>
                </telerik:RadGrid>

            </div>
        </div>--%>
    </div>
    <asp:Button runat="server" ID="btn1" ClientIDMode="Static" OnClick="btn1_Click" Style="border: medium none; background: transparent none repeat scroll 0% 0%; color: transparent;" />
    <input id="chartselector" type="text" runat="server" value="" style="border: medium none; background: transparent none repeat scroll 0% 0%; color: transparent;" />
    <script>

        function test(sender, eventArgs) {
            var htmlElement = eventArgs.get_category(); 
            $(<%= chartselector.ClientID %>).val(htmlElement);
            $("#btn1").click();
        }

        function focusgrid() {

            $('html,body').animate({
                scrollTop: $(<%= levelGrid1.ClientID %>).offset().top
            },
       'slow');
        }

    </script>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="System.Data.SqlClient"></asp:SqlDataSource>

    <%--      <telerik:RadAjaxManager runat="server" ID="theAjaxMaanger">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btn1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="levelGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" Skin="Silk">
    </telerik:RadAjaxLoadingPanel>--%>
</asp:Content>
