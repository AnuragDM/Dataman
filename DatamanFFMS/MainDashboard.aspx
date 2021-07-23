<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MainDashBoard.aspx.cs" Inherits="AstralFFMS.MainDashBoard" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
     <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <style type="text/css">

        .spinner {
            position: absolute;
            top: 50%;
            left: 50%;
            margin-left: -50px; /* half width of the spinner gif */
            margin-top: -50px; /* half height of the spinner gif */
            text-align: center;
            z-index: 999;
            overflow: auto;
            width: 100px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }
        #select2-ContentPlaceHolder1_ddlParentLoc-container{
        margin-top:-8px !important;
        }
            .container {
        width: 100%;
        height: 200px;
        background: aqua;
        margin: auto;
        padding: 10px;
        }
        .one {
        width: 50%;
        height: 200px;
        background: red;
        float: left;
        }
        .two {
        width: 49%;
        margin-left: 51%;
        height: 200px;
        background: black;
        }
          .totalfont {
            /*font-size:21px;*/
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
                font-weight: normal;
            }

                .colornote li:last-child {
                    border: none;
                }

                .colornote li span {
                    font-weight: normal;
                    margin-left: 5px;
                }


                   


h4.great {
	background:rgba(66, 113, 244,0.6);
	margin: 0 0 0px 275px;
	padding: 7px 15px;
	color: #ffffff;
	font-size: 18px;
	font-weight: 600;
	border-radius: 11px;
	display: inline-block;
	-moz-box-shadow:    2px 4px 5px 0 #ccc;
  	-webkit-box-shadow: 2px 4px 5px 0 #ccc;
  	box-shadow:         2px 4px 5px 0 #ccc;
}



.price-slider {
	margin-bottom: 70px;
}

.price-slider span {
	font-weight: 200;
	display: inline-block;
	color:white;
	font-size: 17px;
}
    </style>
    
      <script type="text/javascript">
          $(document).ready(function () {

              pageLoad();
              Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandle);
              function endRequestHandle(sender, Args) {
                  pageLoad();
              };
          }
        );

    </script>
      <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
          .multiselect-container.dropdown-menu {
        width: 100% !important;
        }


        .select2-container {
            /*display: table;*/
        }
         .input-group .form-control {
            height: 34px;
        }

        .multiselect-container > li > a {
            white-space: normal;
        }
    </style>
    <script type="text/javascript">
        $(function () {
           
        });
    </script>

    <script type="text/javascript">
        function pageLoad() {

        };
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
     <script>
         function openTotalEmp() {
            var date = $('#<%=FromDate.ClientID%>').val();
             window.open("DashboardTotalMember.aspx?Date=" + date + "");
         }
         function openPresentEmp() {
             debugger;
             var date = $('#<%=FromDate.ClientID%>').val();
             window.open("DashboardPresentMember.aspx?Date=" + date + "");
         }
         function openAbsentEmp() {
             var date = $('#<%=FromDate.ClientID%>').val();
             window.open("DashboardAbsentMember.aspx?Date=" + date + "");
         }
         function openLeaveEmp() {
             var date = $('#<%=FromDate.ClientID%>').val();
             window.open("DashboardTotalLeave.aspx?Date=" + date + "");
         }
         function getName()
         {
             alert("NAme");
         }
    </script>
    <script type="text/javascript">
        function OnClientSeriesClicked(sender, args) {
            var theDataItem = args.get_dataItem();
            theDataItem.IsExploded = !theDataItem.IsExploded;
            sender.repaint();
            var date = $('#<%=FromDate.ClientID%>').val();
            //alert(theDataItem.category);
            window.open("DashboardPrimary.aspx?Date=" + date + "&Name=" + theDataItem.category + "");
           
        }
        function OnClientSeriesClicked1(sender, args) {
            var theDataItem = args.get_dataItem();
            theDataItem.IsExploded = !theDataItem.IsExploded;
            sender.repaint();
            var date = $('#<%=FromDate.ClientID%>').val();
            //alert(theDataItem.category);
            window.open("DashboardSecondary.aspx?Date=" + date + "&Name=" + theDataItem.category + "");

        }
        function OnClientSeriesClicked2(sender, args) {
            var theDataItem = args.get_dataItem();
            theDataItem.IsExploded = !theDataItem.IsExploded;
            sender.repaint();
            if (theDataItem.category == "DSR") window.open("DSRPendingList.aspx");
            else if (theDataItem.category == "Tour") window.open("TourPlanApproval.aspx?Open=D");
            else if (theDataItem.category == "BeatPlan") window.open("BeatPlanApproval.aspx?Open=D");
            else if (theDataItem.category == "Leave") window.open("LeaveApproval.aspx?Open=D");
            
        }
        function OnClientSeriesHovered(sender, args) {
            setTimeout(function () {
                if (args.get_dataItem().color == "#000000") {
                    document.getElementsByClassName("k-tooltip")[0].style.color = "rgb(222,11,11)";
                }
            }, 0);
        }
</script>
    	
    <script src="dist/js/demo.js" type="text/javascript"></script>
    <section class="content">
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
        </div>
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
         <div class="content" style="background-color:white;width:100%">
        <asp:UpdatePanel ID="mainUp" runat="server">
            <ContentTemplate>
                <div class="row" style="margin-bottom: 15px;">
                   <%-- <div style="float: right;">--%>
                        <asp:Button Style="margin-right: 5px;" type="button" ID="AnalyticsBack" runat="server" Visible="false" Text="Show" class="btn btn-primary" OnClientClick="getData();" />
                    <%--</div>--%>
                    <div class="col-md-12 col-lg-12">
                        <div class="col-lg-3 col-xs-12 headerbottom">
                             <label for="exampleInputEmail1">For Date:</label>
                             <asp:TextBox ID="FromDate" runat="server" CssClass="form-control" AutoPostBack="true" onChange="showspinner();" OnTextChanged="FromDate_TextChanged" Style="background-color: white;"></asp:TextBox>
                           <ajaxToolkit:CalendarExtender ID="CalendarExtender5" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender12" TargetControlID="FromDate"></ajaxToolkit:CalendarExtender>
                        </div>
                         <div class="col-lg-4 col-xs-12 headerbottom" style="margin-top:25px;">
                              <asp:Button ID="btnShow" runat="server" class="btn btn-primary button" Visible="false" Text="Show" OnClientClick="showspinner();" OnClick="btnShow_Click" />
                             </div>
                        <div class="clearfix"></div>
                           <div class="col-lg-3 col-xs-6" id="Present" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box TotalEmp" style="height:140px;background-color:#19B5FE">
                                     <div class="inner">
                                          <p>Total Members</p>
                                         <span><label id="lblTotal" runat="server"></label></span>
                                    </div>
                                    <div class="icon">
                                        <i class=" ion ion-person"></i>
                                    </div>
                                    <a href="#" onclick="openTotalEmp();" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>      
                           <div class="col-lg-3 col-xs-6" id="Active" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box Present" style="height:140px;background-color:#2ecc71">
                                    <div class="inner">
                                        <p>Present</p>
                                         <span><label id="lblPresent" runat="server"></label></span>
                                       
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-person"></i>
                                    </div>
                                    <a href="#" onclick="openPresentEmp();"  class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>      
                        <div class="col-lg-3 col-xs-6" id="Absent" onclick="getData();">
                                <!-- small box -->
                                <div class="small-box Absent" style="height:140px;background-color:#e74c3c">
                                    <div class="inner">
                                        <p>Absent</p>
                                          <span><label id="lblAbsent" runat="server"></label></span>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-person"></i>
                                    </div>
                                    <a href="#" onclick="openAbsentEmp();" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>      
                        <div class="col-lg-3 col-xs-6" id="Leave" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box Leave" style="height:140px;background-color:#F4D03F">
                                    <div class="inner">
                                        <p>Leave</p>
                                         <span><label id="lblLeave" runat="server"></label></span>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-person"></i>
                                    </div>
                                    <a href="#" onclick="openLeaveEmp();" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>      
                    </div>

                </div>
                <div class="row">
                    <div class="col-md-12 col-lg-12">
                        <div class="col-md-12 col-lg-6">
                            <div class="panel-group">
                                <div class="panel panel-default">
                                    <div class="panel-heading primaryHeading" id="Primary">
                                        Primary Sale
                                      <div class="box-tools pull-right">
                                          <a href="#Primarys" data-toggle="collapse"><i class="fa fa-minus"></i></a>
                                      </div>
                                    </div>
                                    <div id="Primarys" class="panel-collapse collapse in">
                                        <div class="panel-body table-responsive">
                                            <telerik:RadHtmlChart runat="server" ID="PieChart1" OnClientSeriesClicked="OnClientSeriesClicked" Height="300" Transitions="false" >
                                                <ChartTitle>
                                                    <Appearance Align="Left" Position="Top">
                                                    </Appearance>
                                                </ChartTitle>
                                                <Legend>
                                                    <Appearance Position="Right" Visible="true">
                                                        <TextStyle FontSize="20" Color="#660066" />

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
                        </div>
                        <div class="col-md-12 col-lg-6">
                            <div class="panel-group">
                                <div class="panel panel-default">
                                    <div class="panel-heading secondaryHeading" id="Secondary">
                                        Secondary Sale
                                      <div class="box-tools pull-right">
                                          <a href="#secondarys" data-toggle="collapse"><i class="fa fa-minus"></i></a>
                                      </div>
                                    </div>
                                    <div id="secondarys" class="panel-collapse collapse in">
                                        <div class="panel-body table-responsive">
                                            <telerik:RadHtmlChart runat="server" ID="PieChart2" OnClientSeriesClicked="OnClientSeriesClicked1" Height="300" Transitions="false">
                                                <ChartTitle>
                                                    <Appearance Align="Left" Position="Top">
                                                    </Appearance>
                                                </ChartTitle>
                                                <Legend>
                                                    <Appearance Position="Right" Visible="true">
                                                        <TextStyle FontSize="20" Color="#660066" />

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
                        </div>
                        <div class="col-md-12 col-lg-6">
                            <div class="panel-group">
                                <div class="panel panel-default">
                                    <div class="panel-heading UnApprovedHeading" id="UnApproved">
                                        UnApproved Status
                                      <div class="box-tools pull-right">
                                          <a href="#UnApproved" data-toggle="collapse"><i class="fa fa-minus"></i></a>
                                      </div>
                                    </div>
                                    <div id="UnApprovedchart" class="panel-collapse collapse in">
                                        <div class="panel-body table-responsive" >
                                             <telerik:RadHtmlChart runat="server" ID="PieChart3" OnClientSeriesHovered="OnClientSeriesHovered" OnClientSeriesClicked="OnClientSeriesClicked2" Height="300" Transitions="false">
                                                <ChartTitle>
                                                    <Appearance Align="Left" Position="Top">
                                                    </Appearance>
                                                </ChartTitle>
                                                <Legend>
                                                    <Appearance Position="Right"  Visible="true">
                                                        <TextStyle FontSize="20" Color="#660066" />
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
                        </div>
                        <div class="col-md-12 col-lg-6">
                            <div class="panel-group">
                                <div class="panel panel-default" >
                                    <div class="panel-heading OrderHeading" id="OrderDetail">
                                        Order Amount Detail
                                      <div class="box-tools pull-right">
                                          <a href="#Order" data-toggle="collapse"><i class="fa fa-minus"></i></a>
                                      </div>
                                    </div>
                                    <div id="Order" class="panel-collapse collapse in" >
                                        <div class="panel-body table-responsive" style="height:335px;">
                                            <telerik:RadHtmlChart runat="server" ID="rhcOrder"  Height="300" Transitions="false">
                                                <ChartTitle>
                                                    <Appearance Align="Left" Position="Top">
                                                    </Appearance>
                                                </ChartTitle>
                                                <Legend>
                                                    <Appearance Position="Right" Visible="true">
                                                        <TextStyle FontSize="20" Color="#660066" />

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
                        </div>
                    </div>
                </div>
            </ContentTemplate>
              <Triggers>
               <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
               <asp:AsyncPostBackTrigger ControlID="FromDate" EventName="TextChanged" />
             </Triggers>
        </asp:UpdatePanel>

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

        function showspinner() {

            $("#spinner").show();

        };
        function hidespinner() {

            $("#spinner").hide();

        };
       
     </script> 
  <style>
       .primaryHeading {
           background-color:rgba(243,156,18,0.1);
       }
        .secondaryHeading {
           background-color:rgba(0,166,90,0.1);
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
             background-color:rgb(149, 176, 219);
           /*border-style: solid;*/
            border-color:#9D2933;
          color:white;
       }
       .Present {
           background-color: rgba(0,141,76,0.3);
          color:white;
            /*border-style: solid;*/
            border-color:#26C281;
       }
       .Absent {
            background-color:rgba(211, 55, 36, 0.4);
            /*border-style: solid;*/
            border-color:#2980b9;
          color:white;
       }
       .Leave {
            background-color:rgba(255, 119, 1, 0.5);
          color:white;
           /*border-style: solid;*/
            border-color:#F3C13A;
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
    font-size:27px;
    /*text-align: center;*/
}
       .inner > span {
    font-size: 27px;
    /*padding-left: 124px;*/
}
       .headerbottom {
           margin-bottom:10px;
       }
   </style>
</asp:Content>