﻿<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" Inherits="AstralFFMS.DashBoard" EnableEventValidation="false" %>
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
            $('[id*=listboxmonth]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=lstDState]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=ddlCity]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=ddlDistrict]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=ddlDistributor]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=ddlSalesPerson]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=lstTransState]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
           
            $('[id*=categoryddlState]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=categoryddlCity]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=categoryddlDistrict]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=categoryddlDistributor]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });

            $('[id*=AnalyticsddlState]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=AnalyticsddlCity]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=AnalyticsddlDistrict]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=AnalyticsddlDistributor]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        };
        $(function () {
            $('[id*=lstState]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });

        $(function () {
            $('[id*=lstUPSate]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
        $(function () {
            $('[id*=lstL1]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
        $(function () {
            $('[id*=lstL2]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
        $(function () {
            $('[id*=lstL3]').multiselect({
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
     <script type="text/javascript">
         function validate() {

             if ($('#<%=txtFromtDate.ClientID%>').val() == "") {
                 errormessage("Please select the From Date");
                 return false;
             }
             if ($('#<%=txtTo.ClientID%>').val() == "") {
                 errormessage("Please select the To Date");
                 return false;
             }

         }
         
         
         function categoryvalidate() {

             if ($('#<%=categorytxtFromDate.ClientID%>').val() == "") {
                 errormessage("Please select the From Date");
                 return false;
             }
             if ($('#<%=categorytxtTo.ClientID%>').val() == "") {
                 errormessage("Please select the To Date");
                 return false;
             }

         }

         function Analyticsvalidate() {

             if ($('#<%=AnalyticstxtFromDate.ClientID%>').val() == "") {
                 errormessage("Please select the From Date");
                 return false;
             }
             if ($('#<%=AnalyticstxtTo.ClientID%>').val() == "") {
                 errormessage("Please select the To Date");
                 return false;
             }

         }

         function TLightValidate() {

             if ($('#<%=txtVisitDate.ClientID%>').val() == "") {
                 errormessage("Please select the  Date");
                 return false;
             }
             if ($('#<%=ddlSalesPerson.ClientID%>').val() == '0') {
                 errormessage("Please select the Sales Person");
                 return false;
             }

         }

     function UPValidate() {

             if ($('#<%=txtUPDate.ClientID%>').val() == "") {
                 errormessage("Please select the  Date");
                 return false;
             }
             if ($('#<%=lstUPSate.ClientID%>').val() == '0') {
                 errormessage("Please select the State");
                 return false;
             }

         }

         function OnClientTileClicked(tileList, args) {
            
             var tile = args.get_tile();
             var url = args.get_oldValue();
             var name = args._tile._name;
             if (name == 'Channel') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_CWSDiv").style.display = 'block';
             }
             if (name == 'Category') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_CategorySaleDiv").style.display = 'block';
             }
             if (name == 'Analytics') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_AnalyticsDiv").style.display = 'block';
             }
             if (name == 'GrowthByRegion') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_GrowthByRegionDiv").style.display = 'block';
             }
             if (name == 'GrowthByArea') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_GrowthByAreaDiv").style.display = 'block';
             }
             if (name == 'PrimaryVsSecondary') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_SalesDiv").style.display = 'block';
             }
             if (name == 'Trend') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_TrendDiv").style.display = 'block';
             }
             if (name == 'TransRegion') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_TransAmtZoneDiv").style.display = 'block';
             }
             if (name == 'TransState') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_TransStateDiv").style.display = 'block';
             }
             if (name == 'Stock') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_StockDashBoardDiv").style.display = 'block';
             }
             if (name == 'OutletsCoverage') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_OutletsCoverageDashBoardDiv").style.display = 'block';
             }
             if (name == 'TargetAchievement') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_TargetAchievementDiv").style.display = 'block';
             }
             if (name == 'Variance') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_VarianceDiv").style.display = 'block';
             }
             if (name == 'ELL') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_ELLDiv").style.display = 'block';
             }
             if (name == 'TLight') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_TLightDiv").style.display = 'block';
                
             }
             if (name == 'UserPerformance') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_UserPerformanceDiv").style.display = 'block';

             }
             if (name == 'Zone') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_ZoneDiv").style.display = 'block';

             }
             if (name == 'Attendance') {
                 document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                 document.getElementById("ContentPlaceHolder1_AttendanceDiv").style.display = 'block';

             }
         }
         function back()
         {
           
             document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
             document.getElementById("ContentPlaceHolder1_CWSDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_CWSD").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_CategorySaleDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_CategorySaleDashboard").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_AnalyticsDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_AnalyticsDashboard").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_GrowthByRegionDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_GrowthByRegionDashBoard").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_GrowthByAreaDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_GrowthByAreaDashBoard").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_SalesDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_SalesDashBoard").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_TrendDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_TrendDashBoard").style.display = 'none';

             document.getElementById("ContentPlaceHolder1_TransAmtZoneDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_TransAmtDashboard").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_TransStateDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_TransStateDashBoard").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_StockDashBoardDiv").style.display = 'none'; 
             document.getElementById("ContentPlaceHolder1_OutletsCoverageDashBoardDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_TargetAchievementDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_VarianceDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_TLightDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_UserPerformanceDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_ZoneDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_ZoneDashboard").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_AttendanceDiv").style.display = 'none';
         }
         function StateWiseAmt() {
           
             document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_TransStateDiv").style.display = 'block';
             document.getElementById("ContentPlaceHolder1_TransAmtZoneDiv").style.display = 'none';
             document.getElementById("ContentPlaceHolder1_TransAmtDashboard").style.display = 'none';
         }
        </script>
      <script type="text/javascript">
          function OnClientSelectedIndexChanged(sender, eventArgs) {
              updateStats();
          }
          function OnDateSelected(sender, eventArgs) {
              updateStats();
          }
          function updateStats() {
              debugger;
              var cmbPerson = $find("<%= cmbPerson.ClientID %>");
              var smId = cmbPerson.get_value();
              var calendar = $find("<%= txtDate.ClientID %>");
              var dt = new Date(calendar.val);

              dt.getFullYear() + "/" + (dt.getMonth() + 1) + "/" + dt.getDate();
              var date1 = calendar.get_selectedDate();
              if (smId && date1 != null) {
                  var day = date1.getDate();
                  var month = date1.getMonth() + 1;
                  var year = date1.getFullYear();

                 
              }
          }
          function openDiv(nName)
          {
              var name = $(nName).attr("id");
              if (name == 'Channel') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_CWSDiv").style.display = 'block';
              }
              if (name == 'Category') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_CategorySaleDiv").style.display = 'block';
              }
              if (name == 'Analytics') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_AnalyticsDiv").style.display = 'block';
              }
              if (name == 'GrowthByRegion') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_GrowthByRegionDiv").style.display = 'block';
              }
              if (name == 'GrowthByArea') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_GrowthByAreaDiv").style.display = 'block';
              }
              if (name == 'PrimaryVsSecondary') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_SalesDiv").style.display = 'block';
              }
              if (name == 'Trend') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_TrendDiv").style.display = 'block';
              }
              if (name == 'TransRegion') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_TransAmtZoneDiv").style.display = 'block';
              }
              if (name == 'TransState') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_TransStateDiv").style.display = 'block';
              }
              if (name == 'Stock') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_StockDashBoardDiv").style.display = 'block';
              }
              if (name == 'OutletsCoverage') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_OutletsCoverageDashBoardDiv").style.display = 'block';
              }
              if (name == 'TargetAchievement') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_TargetAchievementDiv").style.display = 'block';
              }
              if (name == 'Variance') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_VarianceDiv").style.display = 'block';
              }
              if (name == 'ELL') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_ELLDiv").style.display = 'block';
              }
              if (name == 'TLight') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_TLightDiv").style.display = 'block';

              }
              if (name == 'UserPerformance') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_UserPerformanceDiv").style.display = 'block';

              }
              if (name == 'Zone') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  document.getElementById("ContentPlaceHolder1_ZoneDiv").style.display = 'block';

              }
              if (name == 'Attendance1') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  //document.getElementById("ContentPlaceHolder1_AttendanceDiv").style.display = 'block';
                  window.location = '<%= ResolveUrl("~/NewDashboard4.aspx") %>';
                  //window.location.href("NewDashboard4.aspx");
                  //windows.location.href = "NewDashboard4.aspx"
                  //return false;
                  //window.open("NewDashboard4.aspx");

              }
              if (name == 'Attendance') {

                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  window.location = '<%= ResolveUrl("~/NewDashboard3.aspx") %>';


              }
              if (name == 'OrderDetails') {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
                  window.location = '<%= ResolveUrl("~/DistWiseODDashboard.aspx") %>';
              } 
          }
          function FillState()
          {
              var smId = "09";
              $.ajax({
                  type: "POST",
                  url: "Dashboard.aspx/FillState",
                  data: "{}",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  success: function (response) {
                      var mn = response.d;
                      attObj = mn;
                  },
                  failure: function (response) {
                      alert(response.d);
                  },
                  error: function (response) {
                      alert(response.d);
                  }
              });
          }
          openclose()
          {
               
          }
    </script>

    <style>
       .small-box > .small-box-footer {
    background: rgba(0, 0, 0, 0.1) none repeat scroll 0 0;
    color: rgba(255, 255, 255, 0.8);
    display: block;
    font-size: 18px;
    margin-top: 78px;
    padding: 3px 0;
    position: relative;
    text-align: center;
    text-decoration: none;
    z-index: 10;
}
        .ion-stats-bars::before {
    content: "";
    margin-top: 35px;
}
        .inner > p {
    font-size: 21px;
}
       .small-box.bg-light-blue {
}
.bg-light-blue, .label-primary, .modal-primary .modal-body {
    background-color: rgba(60, 141, 188, 0.55);
}

.ion-pie-graph::before {
    content: "";
    margin-top: 43px;
}

.ion-ios-pulse::before {
    content: "";
    margin-top: 37px;
}

.ion-ios-crop-strong::before {
    content: "";
    margin-top: 38px;
}
.ion-android-menu::before {
    content: "";
    margin-top: 40px;
}
.trafficLight {
    color:white;
    background-color: rgba(227, 54, 117, 0.46);
}
.PVS {
    color:white;
    background-color: rgba(66, 21, 59, 0.41);
}
        .atta {
            color:white;
          background-color: rgb(8, 58, 36);
        }
    </style>
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
      
        
        <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                         <%--   <h3 class="box-title">Dashboard</h3>--%>
                            <div style="float: right">
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <asp:UpdatePanel ID="mainUp" runat="server">
                                <ContentTemplate>
                               <div class="col-lg-3 col-xs-6" id="Attendance1" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box atta" style="height:170px;">
                                    <div class="inner">
                                        <p>Resource status</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-pie-graph"></i>
                                    </div>
                                    <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>      
                           
                               
                                <div class="col-lg-3 col-xs-6" id="Analytics" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box bg-yellow" style="height:170px;">
                                    <div class="inner">
                                    <p>Analytics Sales Trend</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion-ios-pulse"></i>
                                    </div>
                                    <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                                <div class="col-lg-3 col-xs-6" id="GrowthByRegion" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box bg-red" style="height:170px;">
                                    <div class="inner">
                                        <p>Growth By Region</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion-ios-crop-strong"></i>
                                    </div>
                                    <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                            
                                  <div class="col-lg-3 col-xs-6" id="Attendance" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box atta" style="height:170px;">
                                    <div class="inner">
                                        <p>Attendence status</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-pie-graph"></i>
                                    </div>
                                    <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>  
                                     <div class="col-lg-3 col-xs-6"  id="Channel" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box bg-green" style="height:170px;">
                                    <div class="inner">
                                        <p>Distribution Channel Wise</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion-android-menu"></i>
                                    </div>
                                    <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                                <div class="col-lg-3 col-xs-6"  id="GrowthByArea" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box bg-orange" style="height:170px;">
                                    <div class="inner">
                                        <p>Growth By Area</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion-ios-crop-strong"></i>
                                    </div>
                                    <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                                <div class="col-lg-3 col-xs-6" id="PrimaryVsSecondary" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box PVS" style="height:170px;">
                                    <div class="inner">
                                        <p>Primary Vs. Secondary Analysis</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-stats-bars"></i>
                                    </div>
                                    <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                                <div class="col-lg-3 col-xs-6" id="Trend" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box bg-light-blue-gradient" style="height:170px;">
                                    <div class="inner">
                                    <p>Category Wise Trend DashBoard</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-stats-bars"></i>
                                    </div>
                                    <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                                     <div class="col-lg-3 col-xs-6" id="Category" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box bg-light-blue" style="height:170px;">
                                    <div class="inner">
                                        <p>Distribution Category Wise Sales</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-pie-graph"></i>
                                    </div>
                                    <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                                <div class="col-lg-3 col-xs-6" id="TransRegion" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box bg-purple" style="height:170px;">
                                    <div class="inner">
                                        <p>Zone Wise Transaction</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion-ios-pulse"></i>
                                    </div>
                                    <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                                <div class="col-lg-3 col-xs-6"  id="TLight" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box trafficLight" style="height:170px;">
                                    <div class="inner">
                                        <p>Traffic Light DashBoard</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-stats-bars"></i>
                                    </div>
                                    <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                                <div class="col-lg-3 col-xs-6" id="UserPerformance" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box bg-green-gradient" style="height:170px;">
                                    <div class="inner">
                                        <p>User Performance DashBoard</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-stats-bars"></i>
                                    </div>
                                    <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                                <div class="col-lg-3 col-xs-6" id="Zone" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box bg-maroon-gradient" style="height:170px;">
                                    <div class="inner">
                                    <p>Zone Wise Interactive DashBoard</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-pie-graph"></i>
                                    </div>
                                    <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                                <div class="col-lg-3 col-xs-6" id="OrderDetails" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box bg-green-gradient" style="height:170px;">
                                    <div class="inner">
                                    <p>Distributor Wise Order Details</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-pie-graph"></i>
                                    </div>
                                    <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                                        <div class="col-lg-3 col-xs-6" id="OutletsCoverage" onclick="openDiv(this);">
                                <!-- small box -->
                                <div class="small-box bg-green-gradient" style="height:170px;">
                                    <div class="inner">
                                    <p>Outlets Coverage</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-pie-graph"></i>
                                    </div>
                                    <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                            <telerik:RadTileList RenderMode="Lightweight" Visible="false" runat="server"  OnClientTileClicked="OnClientTileClicked" AutoPostBack="true"   ID="RadTileList1" Height="500px" OnTileClick="OnTileClick"  Width="100%" TileRows="1" SelectionMode="Multiple" EnableDragAndDrop="true">
                                <Groups>
                                    <telerik:TileGroup Title="Dashboard">
                                        <telerik:RadTextTile ID="Tile1" CssClass="small-box bg-green" runat="server"  Name="Channel" Target="_blank"  Shape="Wide" Width="17.14285714em" Height="7.71428571em" Font-Size="18px" Font-Bold="true" Text="Distribution Channel Wise">
                                        </telerik:RadTextTile>
                                                
                                               
                                            <telerik:RadTextTile ID="RadTextTile1" runat="server"  Name="Category" Target="_blank"  Shape="Wide" Width="17.14285714em" Height="7.71428571em" Font-Size="18px" Font-Bold="true"
                                            Text="Distribution Category Wise Sales">
                                        </telerik:RadTextTile>
                                        <telerik:RadTextTile ID="RadTextTile2" runat="server"  Name="Analytics" Target="_blank"  Shape="Wide" Width="17.14285714em" Height="7.71428571em" Font-Size="18px" Font-Bold="true"
                                            Text="Analytics Sales Trend">
                                        </telerik:RadTextTile>

                                        <telerik:RadTextTile ID="RadTextTile3" runat="server"  Name="GrowthByRegion" Target="_blank"  Shape="Wide" Width="17.14285714em" Height="7.71428571em" Font-Size="18px" Font-Bold="true"
                                            Text="Growth By Region">
                                        </telerik:RadTextTile>
                                            <telerik:RadTextTile ID="RadTextTile4" runat="server"  Name="GrowthByArea" Target="_blank"  Shape="Wide" Width="17.14285714em" Height="7.71428571em" Font-Size="18px" Font-Bold="true"
                                            Text="Growth By Area">
                                        </telerik:RadTextTile>
                                               
                                            <telerik:RadTextTile ID="RadTextTile5" runat="server"  Name="PrimaryVsSecondary" Target="_blank"  Shape="Wide" Width="17.14285714em" Height="7.71428571em" Font-Size="18px" Font-Bold="true"
                                            Text="Primary Vs. Secondary Analysis">
                                        </telerik:RadTextTile>
                                            <telerik:RadTextTile ID="RadTextTile6" runat="server"  Name="Trend" Target="_blank"  Shape="Wide" Width="17.14285714em" Height="7.71428571em" Font-Size="18px" Font-Bold="true"
                                            Text="Category Wise Trend Comparison DashBoard">
                                        </telerik:RadTextTile>
                                        <telerik:RadTextTile ID="RadTextTile7" runat="server"  Name="TransRegion" Target="_blank"  Shape="Wide" Width="17.14285714em" Height="7.71428571em" Font-Size="18px" Font-Bold="true"
                                            Text="Zone Wise Transaction DashBoard">
                                        </telerik:RadTextTile>
                                        <telerik:RadTextTile ID="RadTextTile8" runat="server"  Name="TLight" Target="_blank"  Shape="Wide" Width="17.14285714em" Height="7.71428571em" Font-Size="18px" Font-Bold="true"
                                            Text="Traffic Light DashBoard">
                                        </telerik:RadTextTile>
                                            <telerik:RadTextTile ID="RadTextTile9" runat="server"  Name="UserPerformance" Target="_blank"  Shape="Wide" Width="17.14285714em" Height="7.71428571em" Font-Size="18px" Font-Bold="true"
                                            Text="User Performance DashBoard">
                                        </telerik:RadTextTile>
                                            <telerik:RadTextTile ID="RadTextTile10" runat="server"  Name="Zone" Target="_blank"  Shape="Wide" Width="17.14285714em" Height="7.71428571em" Font-Size="18px" Font-Bold="true"
                                            Text="Zone Wise Interactive DashBoard">
                                        </telerik:RadTextTile>
                                            <telerik:RadImageAndTextTile Shape="Wide" runat="server" Name="Stock" Target="_blank" Text="Stock DashBoard" CssClass="text-center text-bold" ImageUrl="img/download.png" BackColor="#00b37d" Visible="false">
                                            <PeekTemplate>
                                                <img src="img/images(1).png" alt="This have Stock DashBoard Information" style="display: block;" />
                                            </PeekTemplate>
                                            <PeekTemplateSettings Animation="Slide" AnimationDuration="800" Easing="easeInOutBack" ShowInterval="5000" CloseDelay="5000" ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                        </telerik:RadImageAndTextTile>
                                            <telerik:RadImageAndTextTile Shape="Wide" runat="server" Name="OutletsCoverage" Target="_blank" Text="Outlets Coverage DashBoard" CssClass="text-center text-bold" AutoPostBack="true" ImageUrl="img/download.png" OnClick="OutletsCoverage_Click" BackColor="#00b37d" Visible="false">
                                            <PeekTemplate>
                                                <img src="img/images(1).png" alt="This have Outlets Coverage DashBoard Information" style="display: block;" />
                                            </PeekTemplate>
                                            <PeekTemplateSettings Animation="Slide" AnimationDuration="800" Easing="easeInOutBack" ShowInterval="5000" CloseDelay="5000" ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                        </telerik:RadImageAndTextTile>
                                        <telerik:RadImageAndTextTile Shape="Wide" runat="server" Name="TargetAchievement" Target="_blank" Text="Target vs Achievement DashBoard" CssClass="text-center text-bold" AutoPostBack="true" ImageUrl="img/download.png" OnClick="OutletsCoverage_Click" BackColor="#00b37d" Visible="false">
                                            <PeekTemplate>
                                                <img src="img/images(1).png" alt="This have Target vs Achievement DashBoard Information" style="display: block;" />
                                            </PeekTemplate>
                                            <PeekTemplateSettings Animation="Slide" AnimationDuration="800" Easing="easeInOutBack" ShowInterval="5000" CloseDelay="5000" ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                        </telerik:RadImageAndTextTile>
                                            <telerik:RadImageAndTextTile Shape="Wide" runat="server" Name="Variance" Target="_blank" Text="Variance DashBoard" CssClass="text-center text-bold" AutoPostBack="true" ImageUrl="img/download.png" OnClick="OutletsCoverage_Click" BackColor="#00b37d" Visible="false">
                                            <PeekTemplate>
                                                <img src="img/images(1).png" alt="This have Variance DashBoard Information" style="display: block;" />
                                            </PeekTemplate>
                                            <PeekTemplateSettings Animation="Slide" AnimationDuration="800" Easing="easeInOutBack" ShowInterval="5000" CloseDelay="5000" ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                        </telerik:RadImageAndTextTile>
                                            <telerik:RadImageAndTextTile Shape="Wide" runat="server" Name="ELL" Target="_blank" Text="ELL Reporting System DashBoard" CssClass="text-center text-bold" AutoPostBack="true" ImageUrl="img/download.png" OnClick="OutletsCoverage_Click" BackColor="#00b37d" Visible="false">
                                            <PeekTemplate>
                                                <img src="img/images(1).png" alt="This have ELL Reporting System DashBoard Information" style="display: block;" />
                                            </PeekTemplate>
                                            <PeekTemplateSettings Animation="Slide" AnimationDuration="800" Easing="easeInOutBack" ShowInterval="5000" CloseDelay="5000" ShowPeekTemplateOnMouseOver="true" HidePeekTemplateOnMouseOut="true" />
                                        </telerik:RadImageAndTextTile>

                                    <telerik:RadTextTile ID="RadTextTile11" runat="server" OnClick="RadTextTile11_Click"  Name="Attendance1" Target="_self"   Shape="Wide" Width="17.14285714em" Height="7.71428571em" Font-Size="18px" Font-Bold="true"
                                            Text="Attendance DashBoard">
                                        </telerik:RadTextTile>
                                               
                                    </telerik:TileGroup>
                                </Groups>
                            </telerik:RadTileList>

                               </ContentTemplate>
                                   <Triggers>
                                       <asp:AsyncPostBackTrigger ControlID="RadTileList1" EventName="TileClick" />
                                   </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
             </div>
       
     

         <div class="content" id="AnalyticsDiv" runat="server" style="display: none;">
            <div class="row" style="float: right;">
                <asp:Button Style="margin-right: 5px;" type="button" ID="AnalyticsBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" OnClientClick="back()" />
            </div>
            <div class="row">
                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                    <ContentTemplate>
                        <div class="col-md-4  col-lg-4">
                            <div class="panel panel-default">
                                <div class="panel-heading">Analytics Sales Trend Dashboard</div>
                                <div class="panel-body">

                                    <ul class="colornote">
                                        <li>View By:
                                                    <asp:RadioButtonList ID="Analyticsrbl" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Text="Product Group" Value="PG"></asp:ListItem>
                                                    <asp:ListItem Text="Product Class" Value="PC"></asp:ListItem>
                                                    <asp:ListItem Text="Product Segment" Value="PS"></asp:ListItem>
                                                </asp:RadioButtonList></li>
                                        <li>From Date:  
                                                 <asp:TextBox ID="AnalyticstxtFromDate" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender5" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender12" TargetControlID="AnalyticstxtFromDate"></ajaxToolkit:CalendarExtender>
                                        </li>
                                        <li>To: 
                                                   <asp:TextBox ID="AnalyticstxtTo" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender6" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender13" TargetControlID="AnalyticstxtTo"></ajaxToolkit:CalendarExtender>
                                        </li>
                                        <li>State:
                                                   <asp:ListBox ID="AnalyticsddlState" CssClass="form-control" Width="100%" runat="server" OnSelectedIndexChanged="AnalyticsddlState_SelectedIndexChanged" AutoPostBack="true" SelectionMode="Multiple"></asp:ListBox></li>
                                        <li>District:
                                                      <asp:ListBox ID="AnalyticsddlDistrict" CssClass="form-control" Width="100%" runat="server" OnSelectedIndexChanged="AnalyticsddlDistrict_SelectedIndexChanged" AutoPostBack="true" SelectionMode="Multiple"></asp:ListBox></li>
                                        <li>City: 
                                                    <asp:ListBox ID="AnalyticsddlCity" CssClass="form-control" Width="100%" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="AnalyticsddlCity_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox></li>
                                        <li>Distributor:
                                                     <asp:ListBox ID="AnalyticsddlDistributor" CssClass="form-control" Width="100%" runat="server" SelectionMode="Multiple"></asp:ListBox></li>
                                        <li>
                                            <asp:Button ID="btnAnalytics" runat="server" class="btn btn-primary button" Visible="True" Text="Show" OnClientClick="showspinner();" OnClick="AnalyticsbtnShow_Click" /></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-8  col-lg-8" runat="server" id="AnalyticsDashboard" style="display: none;">
                            <div class="demo-container no-bg" runat="server">
                                <div id="Div18" runat="server">
                                    <telerik:RadHtmlChart runat="server" ID="LineChart" Width="100%" Height="500" Transitions="false"></telerik:RadHtmlChart>
                                </div>
                                <div class="price-slider" runat="server" id="AnalyticsTotal" style="display: none;">
                                    <h4 class="great">Total Amount : <span>  <label id="lblAnalytics" class="totalfont" runat="server"></label></span></h4>
                                    </div>
                                </div>
                            </div>
                    </ContentTemplate>
                    <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="AnalyticsddlState" EventName="SelectedIndexChanged" />
                      <asp:AsyncPostBackTrigger ControlID="AnalyticsddlDistrict" EventName="SelectedIndexChanged" />
                      <asp:AsyncPostBackTrigger ControlID="AnalyticsddlCity" EventName="SelectedIndexChanged" />
                     <asp:AsyncPostBackTrigger ControlID="AnalyticsddlDistributor" EventName="SelectedIndexChanged" />
                     <asp:AsyncPostBackTrigger ControlID="AnalyticsBack" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
            <div class="content" id="CWSDiv" runat="server" style="display: none;">
            <div class="row" style="float: right;">
                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" OnClientClick="back()" />
            </div>
            <div class="row">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="col-md-4  col-lg-4">
                            <div class="panel panel-default">
                                <div class="panel-heading">Distribution : Channel Wise Sale Dashboard</div>
                                <div class="panel-body">

                                    <ul class="colornote">
                                        <li>View By:
                                            <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True" Text="Product Group" Value="PG"></asp:ListItem>
                                            <asp:ListItem Text="Product Class" Value="PC"></asp:ListItem>
                                            <asp:ListItem Text="Product Segment" Value="PS"></asp:ListItem>
                                        </asp:RadioButtonList></li>
                                        <li>From Date:  
                                               <asp:TextBox ID="txtFromtDate" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender" TargetControlID="txtFromtDate"></ajaxToolkit:CalendarExtender>
                                        </li>
                                        <li>To: 
                                        <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender1" TargetControlID="txtTo"></ajaxToolkit:CalendarExtender>
                                        </li>
                                        <li>State:
                                                  <asp:ListBox ID="lstDState" CssClass="form-control select2" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged"  SelectionMode="Multiple"></asp:ListBox></li>
                                        <li>District:
                                                      <asp:ListBox ID="ddlDistrict" CssClass="form-control select2" Width="100%" runat="server" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" AutoPostBack="true" SelectionMode="Multiple"></asp:ListBox></li>
                                        <li>City: 
                                                    <asp:ListBox ID="ddlCity" CssClass="form-control select2" Width="100%" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        <li>Distributor:
                                                     <asp:ListBox ID="ddlDistributor" CssClass="form-control select2" Width="100%" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        <li>
                                             <asp:Button ID="btnsave1" runat="server" class="btn btn-primary button" Visible="True" Text="Show" OnClientClick="showspinner();" OnClick="btnShow_Click" /></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-8  col-lg-8" runat="server" id="CWSD" style="display: none;">
                            <div class="demo-container no-bg" runat="server">
                                <div id="Div6" runat="server">
                                    <telerik:RadHtmlChart runat="server" ID="rhc" Width="100%" Height="500" Transitions="false"></telerik:RadHtmlChart>
                                </div>
                                <div class="price-slider" runat="server" id="ShowDiv" style="display: none;">
                                    <h4 class="great">Total Amount : <span>  <label id="lblTotal" class="totalfont" runat="server"></label></span></h4>
                                    </div>
                                </div>
                            </div>
                    </ContentTemplate>
                    <Triggers>
                   <asp:AsyncPostBackTrigger ControlID="lstDState" EventName="SelectedIndexChanged" />
                   <asp:AsyncPostBackTrigger ControlID="ddlDistrict" EventName="SelectedIndexChanged" />
                   <asp:AsyncPostBackTrigger ControlID="ddlCity" EventName="SelectedIndexChanged" />
                   <asp:AsyncPostBackTrigger ControlID="ddlDistributor" EventName="SelectedIndexChanged" />
                   <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        
        <div class="content" id="GrowthByRegionDiv" runat="server" style="display: none;">
            <div class="row" style="float: right;">
                <asp:Button Style="margin-right: 5px;" type="button" ID="Button2" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" OnClientClick="back()" />
            </div>
            <div class="row">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <div class="col-md-4  col-lg-4">
                            <div class="panel panel-default">
                                <div class="panel-heading">Growth By Region DashBoard</div>
                                <div class="panel-body">

                                    <ul class="colornote">
                                        <li>Year:
                                                   <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                                                      <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                                                      <asp:ListItem Text="2017" Value="2017" Selected="True"></asp:ListItem>
                                                      <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                                                      <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                                                    <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                                                </asp:DropDownList></li>
                                        <li>Region:  
                                                 <asp:DropDownList ID="ddlRegion" runat="server" CssClass="form-control select2" AutoPostBack="True"  OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged"></asp:DropDownList>
                                        </li>
                                       
                                        <li>
                                             <asp:Button ID="btnGrowthByRegion" runat="server" class="btn btn-primary button" Visible="True" Text="Show" OnClientClick="showspinner"  OnClick="btnGrowthByRegion_Click" /></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-8  col-lg-8" runat="server" id="GrowthByRegionDashBoard" style="display: none;">
                            <div class="demo-container no-bg" runat="server">
                                <div id="Div5" runat="server">
                                    <telerik:RadHtmlChart runat="server" ID="AreaChart" Width="100%" Height="500" Transitions="false"></telerik:RadHtmlChart>
                                </div>
                                <div class="price-slider" runat="server" id="amtDiv" style="display: none;">
                                    <h4 class="great">Total Amount : <span>  <label id="lblGrowthByRegionDiv" class="totalfont" runat="server"></label></span></h4>
                                    </div>
                                </div>
                            </div>
                    </ContentTemplate>
                    <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="ddlRegion" EventName="SelectedIndexChanged" />
                     <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click" />
                     <asp:AsyncPostBackTrigger ControlID="btnGrowthByRegion" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

          <div class="content" id="GrowthByAreaDiv" runat="server" style="display: none;">
            <div class="row" style="float: right;">
                <asp:Button Style="margin-right: 5px;" type="button" ID="Button3" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" OnClientClick="back()" />
            </div>
            <div class="row">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <div class="col-md-4  col-lg-4">
                            <div class="panel panel-default">
                                <div class="panel-heading">Growth By Area DashBoard</div>
                                <div class="panel-body">

                                    <ul class="colornote">
                                        <li>Year:
                                             <asp:DropDownList ID="ddlGBAYear" runat="server" CssClass="form-control" AutoPostBack="true">
                                            <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                                            <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                                            <asp:ListItem Text="2017" Value="2017" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                                            <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                                            <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                                        </asp:DropDownList></li>
                                        <li>Region:  
                                                   <asp:DropDownList ID="ddlGBAR" runat="server" CssClass="form-control select2" AutoPostBack="true"></asp:DropDownList>
                                        </li>
                                         <li>View By:
                                            <asp:RadioButtonList ID="rblGBA" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True" Text="Product Group" Value="PG"></asp:ListItem>
                                            <asp:ListItem Text="Product Class" Value="PC"></asp:ListItem>
                                            <asp:ListItem Text="Product Segment" Value="PS"></asp:ListItem>
                                        </asp:RadioButtonList></li>
                                       
                                        <li>
                                             <asp:Button ID="Button4" runat="server" class="btn btn-primary button" Visible="True" Text="Show" OnClientClick="showspinner();" OnClick="btnGrowthByArea_Click" /></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-8  col-lg-8" runat="server" id="GrowthByAreaDashBoard" style="display: none;">
                            <div class="demo-container no-bg" runat="server">
                                <div id="Div4" runat="server">
                                   <telerik:RadHtmlChart runat="server" ID="AAreaChart" Width="100%" Height="500" Transitions="true" Skin="Silk"></telerik:RadHtmlChart>
                                </div>
                                <div class="price-slider" runat="server" id="GrowthByAreaDivTotal" style="display: none;">
                                    <h4 class="great">Total Amount : <span>  <label id="lblGBA" class="totalfont" runat="server"></label></span></h4>
                                    </div>
                                </div>
                            </div>
                    </ContentTemplate>
                    <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="ddlRegion" EventName="SelectedIndexChanged" />
                     <asp:AsyncPostBackTrigger ControlID="Button3" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

          <div class="content" id="SalesDiv" runat="server" style="display: none;">
            <div class="row" style="float: right;">
                <asp:Button Style="margin-right: 5px;" type="button" ID="Button5" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" OnClientClick="back()" />
            </div>
            <div class="row">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <div class="col-md-4  col-lg-4">
                            <div class="panel panel-default">
                                <div class="panel-heading">Sales Disribution vs. Consumption -Region Wise DashBoard</div>
                                <div class="panel-body">

                                    <ul class="colornote">
                                        <li>Year:
                                             <asp:DropDownList ID="ddlSales" runat="server" CssClass="form-control" AutoPostBack="true">
                                            <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                                            <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                                            <asp:ListItem Text="2017" Value="2017" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                                            <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                                            <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                                        </asp:DropDownList></li>
                                        <li>Region:  
                                                 <asp:DropDownList ID="ddlSalesRegion" runat="server" CssClass="form-control select2" AutoPostBack="true"></asp:DropDownList>
                                        </li>
                                       
                                        <li>
                                             <asp:Button ID="Button6" runat="server" class="btn btn-primary button" Visible="True" Text="Show" OnClientClick="showspinner();" OnClick="btnSales_Click" />
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-8  col-lg-8" runat="server" id="SalesDashBoard" style="display: none;">
                            <div class="demo-container no-bg" runat="server">
                                  <telerik:RadHtmlChart runat="server" CssClass="table table-bordered table-striped" ID="rhcPrimary" Width="100%" Height="500" Transitions="false" Skin="Silk"></telerik:RadHtmlChart>
                                </div>                               <%-- <div id="Div2" class="col-md-4  col-lg-4" runat="server">--%>
                                   <telerik:RadHtmlChart runat="server" ID="rhcSecondry" Width="100%" Height="500" Transitions="false" Visible="false" Skin="Silk"></telerik:RadHtmlChart>
                                <%--</div>--%>
                                <div class="price-slider" runat="server" id="salesTotalDiv" style="display: none;">
                                    <h4 class="great">Total Amount : <span>  <label id="lblSales" class="totalfont" runat="server"></label></span></h4>
                                    </div>
                                </div>
                    </ContentTemplate>
                    <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="ddlRegion" EventName="SelectedIndexChanged" />
                     <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click" />
                     <asp:AsyncPostBackTrigger ControlID="Button6" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

         <div class="content" id="TrendDiv" runat="server" style="display: none;">
            <div class="row" style="float: right;">
                <asp:Button Style="margin-right: 5px;" type="button" ID="Button7" runat="server" Text="Back" class="btn btn-primary" OnClientClick="back()" OnClick="btnBack_Click" />
            </div>
            <div class="row">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <div class="col-md-4  col-lg-4">
                            <div class="panel panel-default">
                                <div class="panel-heading">Category Wise Trend Comparison DashBoard</div>
                                <div class="panel-body">

                                    <ul class="colornote">
                                        <li>Year:
                                             <asp:DropDownList ID="ddlTrendYear" runat="server" CssClass="form-control" AutoPostBack="true">
                                            <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                                            <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                                            <asp:ListItem Text="2017" Value="2017" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                                            <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                                            <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                                        </asp:DropDownList></li>
                                        <li>Region:  
                                                 <asp:DropDownList ID="ddlTrendregion" runat="server" CssClass="form-control select2" AutoPostBack="true" ></asp:DropDownList>
                                        </li>
                                         <li>View By:
                                             <asp:RadioButtonList ID="rblTrend" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Text="Product Group" Value="PG"></asp:ListItem>
                                                    <asp:ListItem Text="Product Class" Value="PC"></asp:ListItem>
                                                    <asp:ListItem Text="Product Segment" Value="PS"></asp:ListItem>
                                                </asp:RadioButtonList></li>
                                       
                                        <li>
                                             <asp:Button ID="Button8" runat="server" class="btn btn-primary button" Visible="True" Text="Show"  OnClick="btnTrend_Click" />
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-8  col-lg-8" runat="server" id="TrendDashBoard" style="display: none;">
                            <div class="demo-container no-bg" runat="server">
                                <div id="Div7" runat="server" class="row table-responsive">
                                    <telerik:RadDockLayout runat="server" ID="RadDockLayout1">
                                        <telerik:RadDockZone ID="RadDockZone2" runat="server" Orientation="Vertical" Width="100%" Height="500px">
                                        </telerik:RadDockZone>
                                    </telerik:RadDockLayout>
                                </div>
                                <div class="price-slider" runat="server" id="TrendDivTotal" style="display: none;">
                                    <h4 class="great">Total Amount : <span>  <label id="lblTrend" class="totalfont" runat="server"></label></span></h4>
                                    </div>
                                </div>
                            </div>
                    </ContentTemplate>
                    <Triggers>
                   <asp:AsyncPostBackTrigger ControlID="ddlRegion" EventName="SelectedIndexChanged" />
                   <asp:AsyncPostBackTrigger ControlID="Button7" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
      
           <div class="content" id="TransAmtZoneDiv" runat="server" style="display: none;">
            <div class="row" style="float: right;">
                <asp:Button Style="margin-right: 5px;" type="button" ID="Button9" runat="server" Text="Back" class="btn btn-primary" OnClientClick="back()" OnClick="btnBack_Click" />
                <asp:Button Style="margin-right: 5px;" type="button" ID="Button13" runat="server" Text="Amounts State Wise" class="btn btn-primary"   OnClick="btnAmtState_Click" />
            </div>
            <div class="row">
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <div class="col-md-4  col-lg-4">
                            <div class="panel panel-default">
                                <div class="panel-heading">Transaction Amount Zone Wise DashBoard</div>
                                <div class="panel-body">

                                    <ul class="colornote">
                                        <li>Year:
                                                  <asp:DropDownList ID="ddlTransYear" runat="server" CssClass="form-control" AutoPostBack="true">
                                                    <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                                                      <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                                                      <asp:ListItem Text="2017" Value="2017" Selected="True"></asp:ListItem>
                                                      <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                                                      <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                                                    <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                                                </asp:DropDownList></li>
                                        <li>Region:  
                                            <asp:DropDownList ID="ddlTransRegion" runat="server" CssClass="form-control select2" AutoPostBack="true" ></asp:DropDownList>
                                        </li>
                                       
                                        <li>
                                            <asp:Button ID="Button10" runat="server" class="btn btn-primary button" Visible="True" Text="Show"  OnClick="btnTransRegion_Click" />
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-8  col-lg-8" runat="server" id="TransAmtDashboard" style="display: none;">
                            <div class="demo-container no-bg" runat="server">
                                <div id="Div8" runat="server" class="row table-responsive">
                                    <telerik:RadDockLayout runat="server" ID="rdlTransRegion">
                                        <telerik:RadDockZone runat="server" ID="rdzTransRegion" MinWidth="250" MinHeight="340"
                                            Style="margin-top: 15px; max-width: 100%" CssClass="col-lg-12">
                                        </telerik:RadDockZone>
                                    </telerik:RadDockLayout>
                                </div>
                                <div class="price-slider" runat="server" id="TransRegionDiv" style="display: none;">
                                    <h4 class="great">Total Amount : <span>  <label id="lblTransRegion" class="totalfont" runat="server"></label></span></h4>
                                    </div>
                                </div>
                            </div>
                    </ContentTemplate>
                    <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlTransYear" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="Button9" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="Button13" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="Button10" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>


          <div class="content" id="TransStateDiv" runat="server" style="display: none;">
            <div class="row" style="float: right;">
                <asp:Button Style="margin-right: 5px;" type="button" ID="Button11" runat="server" Text="Back" class="btn btn-primary"  OnClick="btnTransBack_Click" />
            </div>
            <div class="row">
                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                    <ContentTemplate>
                        <div class="col-md-4  col-lg-4">
                            <div class="panel panel-default">
                                <div class="panel-heading">Transaction Amounts State Wise DashBoard</div>
                                <div class="panel-body">

                                    <ul class="colornote">
                                        <li>State:
                                                  <asp:ListBox ID="lstTransState" CssClass="form-control select2" Width="100%" runat="server" SelectionMode="Multiple"></asp:ListBox></li>
                                       
                                        <li>
                                           <asp:Button ID="Button12" runat="server" class="btn btn-primary button" Visible="True" Text="Show"  OnClick="btnTransState_Click" />
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-8  col-lg-8" runat="server" id="TransStateDashBoard" style="display: none;">
                            <div class="demo-container no-bg" runat="server">
                                <div id="Div9" runat="server" class="row table-responsive">
                                    <telerik:RadDockLayout runat="server" ID="rdlTransState">
                                        <telerik:RadDockZone runat="server" ID="rdzTransState" MinWidth="250" MinHeight="340" Style="margin-top: 15px; max-width: 100%; max-height: 100%" CssClass="col-lg-12">
                                        </telerik:RadDockZone>

                                    </telerik:RadDockLayout>
                                </div>
                                <div class="price-slider" runat="server" id="TransStateTotalDiv" style="display: none;">
                                    <h4 class="great">Total Amount : <span>  <label id="lblTransState" class="totalfont" runat="server"></label></span></h4>
                                    </div>
                                </div>
                            </div>
                    </ContentTemplate>
                    <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlTransYear" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="Button9" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="Button13" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="Button10" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
 
            <div class="content" id="TLightDiv" runat="server" style="display: none;">
            <div class="row" style="float: right;">
              <asp:Button Style="margin-right: 5px;" type="button" ID="Button21" runat="server" Text="Back" class="btn btn-primary" OnClientClick="back()" OnClick="btnBack_Click" />
            </div>
            <div class="row">
                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                    <ContentTemplate>
                        <div class="col-md-4  col-lg-4">
                            <div class="panel panel-default">
                                <div class="panel-heading">Traffic Light DashBoard</div>
                                <div class="panel-body">

                                    <ul class="colornote">
                                        <li>SalesPerson:
                                                   <asp:ListBox ID="ddlSalesPerson" CssClass="form-control select2" Width="100%" runat="server" SelectionMode="Multiple"></asp:ListBox></li>
                                        <li>Date:
                                                 <asp:TextBox ID="txtVisitDate" runat="server" ClientIDMode="Static" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                               <ajaxToolkit:CalendarExtender ID="CalendarExtender02" CssClass="orange"  Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender02" TargetControlID="txtVisitDate"></ajaxToolkit:CalendarExtender></li>
                                       
                                        <li>
                                           <asp:Button ID="Button22" runat="server" class="btn btn-primary button" Visible="True" Text="Update" OnClientClick="TLightValidate();" OnClick="btnUpdate_Click" />
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-8  col-lg-8" runat="server" id="Div3" style="display: none;">
                            <div class="demo-container no-bg" runat="server">
                                <div id="Div10" runat="server" class="row table-responsive">
                                   <asp:Repeater ID="rgTLight" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Date</th>
                                                 <th>Sale Person</th>
                                                <th>Beat</th>
                                                 <th>Mobile</th>                                                 
                                                 <th>Total Party</th>
                                                 <th>Calls Visited</th>
                                                 <th>Productive Call</th>
                                                 <th>Total Order</th>                                              
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("vdate"))%></td>
                                           <td><%#Eval("SRName") %></td>
                                           <td><%#Eval("Beat") %></td>
                                           <td><%#Eval("Mobile") %></td>
                                           <td><%#Eval("totalparty") %></td>
                                           <td><%#Eval("callsVisited") %></td>
                                           <td><%#Eval("ProductiveCall") %></td>
                                           <td><%#Eval("TotalOrder") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                                </div>
                                </div>
                            </div>
                    </ContentTemplate>
                    <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlSalesPerson" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="Button22" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="Button21" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>


          <div class="box-body" id="StockDashBoardDiv" runat="server" style="display:none">
                    <div class="row">
                        <!-- left column -->
                        <div class="col-md-12">
                            <!-- general form elements -->
                            <div class="box box-default">
                                <div class="box-header">
                                    <h3 class="box-title">Stock DashBoard</h3>
                                    <div style="float: right">
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="Button14" runat="server" Text="Back" class="btn btn-primary"  OnClick="btnBack_Click" />
                                         
                                    </div>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                             
                                        <div class="col-md-12">
                                             
                                            <div class="form-group col-lg-3 col-md-3 col-sm-12 col-xs-12"  style="height:100px; color:white; background-color:chocolate;">
                                               <label for="exampleInputEmail1" style="color:white">OutLets Covered</label>
                                              
                                            </div>
                                              <div class="form-group col-lg-3 col-md-3 col-sm-12 col-xs-12" style="height:100px; background-color:blue;">
                                               <label for="exampleInputEmail1" style="color:white">Outlets Ordered</label>
                                              
                                            </div>
                                              <div class="form-group col-lg-3 col-md-3 col-sm-12 col-xs-12" style="height:100px; background-color:darkblue;">
                                               <label for="exampleInputEmail1" style="color:white">Outlets Where Stocks Is Present & Not Ordered</label>
                                              
                                            </div>
                                              <div class="form-group col-lg-3 col-md-3 col-sm-12 col-xs-12" style="height:100px; background-color:lightblue">
                                               <label for="exampleInputEmail1" style="color:white">Outlets Where Stocks Is Not Present & Not Ordered</label>
                                              
                                            </div>
                                        </div>
                                   
                                <div class="box-footer">
                                    <asp:HiddenField ID="HiddenField9" runat="server" />
                                    <asp:Button ID="Button15" runat="server" class="btn btn-primary button" Visible="True" Text="Show"  OnClick="btnTransState_Click" />
                                    <label for="exampleInputEmail1" style="margin-left: 30px;">Total Sale:</label>
                                    <label id="Label1" runat="server"></label>
                                </div>
                                   <div class="form-group col-md-12 table-responsive">
                                     <telerik:RadGrid RenderMode="Lightweight" ID="rgStockDashboard" runat="server" AllowPaging="True" AllowSorting="True"></telerik:RadGrid>
                                  </div>
                                <br />
                                <div>
                                    <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                                </div>
                            </div>
                        </div>


                    </div>
               
        </div>
          <div class="box-body" id="OutletsCoverageDashBoardDiv" runat="server" style="display:none">
                    <div class="row">
                        <!-- left column -->
                        <div class="col-md-12">
                            <!-- general form elements -->
                            <div class="box box-default">
                                <div class="box-header">
                                    <h3 class="box-title">Outlets Coverage DashBoard</h3>
                                    <div style="float: right">
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="Button16" runat="server" Text="Back" class="btn btn-primary"  OnClick="btnBack_Click" />
                                         
                                    </div>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                             
                                        <div class="col-md-12">
                                             
                                            <div class="form-group col-lg-3 col-md-3 col-sm-12 col-xs-12"  style="height:150px; color:white; background-color:chocolate; border-right-color:blue;">
                                               <label for="exampleInputEmail1" style="color:black">OutLets Listed</label>
                                               
                                            </div>
                                              <div class="form-group col-lg-3 col-md-3 col-sm-12 col-xs-12" id="ActiveOutlets" style="height:150px; background-color:white; border-right-color:blue;">
                                               <label for="exampleInputEmail1" style="color:black">Active Outlets</label>
                                               <telerik:RadRadialGauge runat="server" ID="rrgActiveOutlets" CssClass="RadRadialGaugeHeight"  ClientIDMode="Static" Width="300px" Height="150px"></telerik:RadRadialGauge>
                                            </div>
                                            
                                              <div class="form-group col-lg-3 col-md-3 col-sm-12 col-xs-12" style="height:150px; background-color:White; border-right-color:grey;">
                                               <label for="exampleInputEmail1" style="color:black">Buyers</label>
                                             <telerik:RadRadialGauge runat="server" ClientIDMode="Static" ID="rrgBuyers" CssClass="RadRadialGaugeHeight" Height="150px" Width="300px"></telerik:RadRadialGauge>
                                            </div>

                                              <div class="form-group col-lg-3 col-md-3 col-sm-12 col-xs-12" style="height:150px; background-color:White;border-right-color:grey;">
                                               <label for="exampleInputEmail1" style="color:black">Outlets Not Visited</label>
                                               <telerik:RadRadialGauge runat="server" ClientIDMode="Static" ID="rrgOutletsNotVisited" CssClass="RadRadialGaugeHeight" Width="300px" Height="150px"></telerik:RadRadialGauge>
                                            </div>
                                        </div>
                                   
                                <div class="box-footer">
                                    <asp:HiddenField ID="HiddenField10" runat="server" />
                                    <asp:Button ID="Button17" runat="server" class="btn btn-primary button" Visible="True" Text="Show"  OnClick="btnTransState_Click" />
                                    <label for="exampleInputEmail1" style="margin-left: 30px;">Total Sale:</label>
                                    <label id="Label2" runat="server"></label>
                                </div>
                                   <div class="form-group col-md-12 table-responsive">
                                     <telerik:RadGrid RenderMode="Lightweight" ID="rgOutletsCoverage" runat="server" AllowPaging="True" AllowSorting="True"></telerik:RadGrid>
                                  </div>
                                <br />
                                <div>
                                    <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                                </div>
                            </div>
                        </div>


                    </div>
               
        </div>
          <div class="box-body" id="TargetAchievementDiv" runat="server" style="display: none">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Target vs Achievement</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="Button18" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->

                        <div class="col-md-12">

                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12" style="height: 100px; color: white; background-color: chocolate;">
                                <label for="exampleInputEmail1" style="color: white">Target</label>

                            </div>
                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12" style="height: 100px; background-color: blue;">
                                <label for="exampleInputEmail1" style="color: white">Total Sales Up To Date</label>

                            </div>
                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12" style="height: 100px; background-color: darkblue;">
                                <label for="exampleInputEmail1" style="color: white">Diffrences</label>

                            </div>
                          
                        </div>

                        <div class="box-footer">                          
                            <label for="exampleInputEmail1" style="margin-left: 30px; text-align:center">Beat Wise Report</label>
                            
                        </div>
                        <div class="form-group col-md-12 table-responsive">
                            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowPaging="True" AllowSorting="True"></telerik:RadGrid>
                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>


            </div>

        </div>
          <div class="box-body" id="VarianceDiv" runat="server" style="display: none">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Variance</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="Button19" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->

                        <div class="col-md-12">

                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12" style="height: 100px; color: white; background-color: chocolate;">
                                <label for="exampleInputEmail1" style="color: white">Max Variance</label>

                            </div>
                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12" style="height: 100px; background-color: blue;">
                                <label for="exampleInputEmail1" style="color: white">Min Variance</label>

                            </div>
                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12" style="height: 100px; background-color: darkblue;">
                                <label for="exampleInputEmail1" style="color: white">Average Variance</label>

                            </div>

                            <div class="form-group col-lg-3 col-md-3 col-sm-12 col-xs-12" style="height: 170px; color: white; background-color:#ffe6ff;">
                                <label for="exampleInputEmail1" style="color: white">Outlets Transacted</label>
                                 <telerik:RadRadialGauge runat="server" ID="rrgOutletsTransacted" CssClass="RadRadialGaugeHeight"  ClientIDMode="Static" Width="300px" Height="150px"></telerik:RadRadialGauge>
                            </div>
                            <div class="form-group col-lg-3 col-md-3 col-sm-12 col-xs-12" style="height: 170px; background-color: #fff2e6;">
                                <label for="exampleInputEmail1" style="color: white">Productive Transactions</label>
                                  <telerik:RadRadialGauge runat="server" ID="rrgProductiveTransactions" CssClass="RadRadialGaugeHeight"  ClientIDMode="Static" Width="300px" Height="150px"></telerik:RadRadialGauge>
                            </div>
                            <div class="form-group col-lg-3 col-md-3 col-sm-12 col-xs-12" style="height: 170px; background-color: #ffe6cc;">
                                <label for="exampleInputEmail1" style="color: white">In-Store Productivity</label>
                                  <telerik:RadRadialGauge runat="server" ID="rrgProductivity" CssClass="RadRadialGaugeHeight"  ClientIDMode="Static" Width="300px" Height="150px"></telerik:RadRadialGauge>
                            </div>
                              <div class="form-group col-lg-3 col-md-3 col-sm-12 col-xs-12" style="height: 170px; background-color:#b3ecff;">
                                <label for="exampleInputEmail1" style="color: white">Remote Productivity</label>
                                 <telerik:RadRadialGauge runat="server" ID="rrgRemoteProductivity" CssClass="RadRadialGaugeHeight"  ClientIDMode="Static" Width="300px" Height="150px"></telerik:RadRadialGauge>
                            </div>
                        </div>

                        <div class="form-group col-md-12 table-responsive">
                            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid2" runat="server" AllowPaging="True" AllowSorting="True"></telerik:RadGrid>
                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>


            </div>

        </div>
          <div class="box-body" id="ELLDiv" runat="server" style="display: none">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">ELL Reporting System</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="Button20" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->

                        <div class="col-md-12">

                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12" style="height: 100px; color: white; background-color: #ccccb3;">
                                <label for="exampleInputEmail1" style="color: white">Max Variance</label>

                            </div>
                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12" style="height: 100px; background-color: #b3b3ff;">
                                <label for="exampleInputEmail1" style="color: white">Min Variance</label>

                            </div>
                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12" style="height: 100px; background-color: #ffccff;">
                                <label for="exampleInputEmail1" style="color: white">Average Variance</label>

                            </div>

                            <div class="form-group col-lg-3 col-md-3 col-sm-12 col-xs-12" style="height: 170px; color: white; background-color:#ffe6ff;">
                                <label for="exampleInputEmail1" style="color: white">Outlets Visited</label>
                                 <telerik:RadRadialGauge runat="server" ID="rrgOutletsVisited" CssClass="RadRadialGaugeHeight"  ClientIDMode="Static" Width="300px" Height="150px"></telerik:RadRadialGauge>
                            </div>
                            <div class="form-group col-lg-3 col-md-3 col-sm-12 col-xs-12" style="height: 170px; background-color: #fff2e6;">
                                <label for="exampleInputEmail1" style="color: white">ProductiveTransactions</label>
                                  <telerik:RadRadialGauge runat="server" ID="rrgEProductiveTransactions" CssClass="RadRadialGaugeHeight"  ClientIDMode="Static" Width="300px" Height="150px"></telerik:RadRadialGauge>
                            </div>
                            <div class="form-group col-lg-3 col-md-3 col-sm-12 col-xs-12" style="height: 170px; background-color: #ffe6cc;">
                                <label for="exampleInputEmail1" style="color: white">In-Store Productivity</label>
                                  <telerik:RadRadialGauge runat="server" ID="rrgEProductivity" CssClass="RadRadialGaugeHeight"  ClientIDMode="Static" Width="300px" Height="150px"></telerik:RadRadialGauge>
                            </div>
                              <div class="form-group col-lg-3 col-md-3 col-sm-12 col-xs-12" style="height: 170px; background-color:#b3ecff;">
                                <label for="exampleInputEmail1" style="color: white">Remote Productivity</label>
                                 <telerik:RadRadialGauge runat="server" ID="rrgERemoteProductivity" CssClass="RadRadialGaugeHeight"  ClientIDMode="Static" Width="300px" Height="150px"></telerik:RadRadialGauge>
                            </div>
                        </div>

                        <div class="form-group col-md-12 table-responsive">
                            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid3" runat="server" AllowPaging="True" AllowSorting="True"></telerik:RadGrid>
                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>


            </div>

        </div>
       



          <div class="box-body" id="UserPerformanceDiv" runat="server" style="display: none">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">User Performance DashBoard</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnUPBack" runat="server" Text="Back" class="btn btn-primary" OnClientClick="back()" OnClick="btnBack_Click" />
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                <label for="name">Region :</label>
                                <asp:DropDownList ID="ddlUPRegion" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="ddlUPRegion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </div>
                             <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                <label for="exampleInputEmail1">Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:TextBox ID="txtUPDate" runat="server" ClientIDMode="Static" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender7" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender03" TargetControlID="txtUPDate"></ajaxToolkit:CalendarExtender>
                            </div>
                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                <label for="exampleInputEmail1">State:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:ListBox ID="lstUPSate" CssClass="form-control select2" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstUPSate_SelectedIndexChanged" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                <label for="exampleInputEmail1">L3:</label>
                                <asp:ListBox ID="lstL3" CssClass="form-control select2" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstL3_SelectedIndexChanged" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                <label for="exampleInputEmail1">L2:</label>
                                <asp:ListBox ID="lstL2" CssClass="form-control select2" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstL2_SelectedIndexChanged" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                <label for="exampleInputEmail1">L1:</label>
                                <asp:ListBox ID="lstL1" CssClass="form-control select2" Width="100%" runat="server"  SelectionMode="Multiple"></asp:ListBox>
                            </div>
                              <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12">
                             </div>
                             <div class="clearfix"></div>
                             <div class="form-group col-lg-6 col-md-6 col-sm-12 col-xs-12"  style="height:100px; text-align:center; font-size:medium; background-color:rgba(79, 127, 245,0.75);color:white;">
                                   <asp:Label  runat="server">Orders</asp:Label>
                                 <br />
                                 <asp:Label ID="lblUPOrders" CssClass="lbl" runat="server"></asp:Label>
                                
                            </div>
                            <div class="form-group col-lg-6 col-md-6 col-sm-12 col-xs-12" style="height:100px; text-align:center; font-size:medium; background-color:rgba(245, 89, 79,0.76);color:white;">
                                  <asp:Label  runat="server">TC/PC</asp:Label>  
                                <br />
                                <asp:Label ID="lblUPTC" runat="server" CssClass="lbl"></asp:Label>
                                <br />
                                  <asp:Label ID="lblUPPC" runat="server" CssClass="lbl"></asp:Label>
                            </div>
                           
                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12" title="Unique PC" style="height:100px; text-align:center; font-size:medium; background-color:#894FF5; color:white; display:none;">
                                 <asp:Label  runat="server">Unique PC</asp:Label>
                                <asp:Label ID="lblUPUinquePC" runat="server"></asp:Label>
                            </div>
                        </div>
                        
                        <div class="box-footer">
                            <asp:Button ID="Button24" runat="server" class="btn btn-primary button" Visible="True" Text="Show" OnClientClick="UPValidate();" OnClick="btnUPUpdate_Click" />
                        </div>
                        <br />
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rptUP" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Date</th>
                                                <th>Sale Person</th>
                                                <th>Mobile</th>
                                                <th>Beat</th>
                                                <th>Total Party</th>
                                                <th>Calls Visited</th>
                                                <th>Productive Call</th>
                                                <th>Total Order</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("vdate"))%></td>
                                        <td><%#Eval("SRName") %></td>
                                        <td><%#Eval("Beat") %></td>
                                        <td><%#Eval("Mobile") %></td>
                                        <td><%#Eval("totalparty") %></td>
                                        <td><%#Eval("callsVisited") %></td>
                                        <td><%#Eval("ProductiveCall") %></td>
                                        <td><%#Eval("TotalOrder") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>


                        </div>
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>


            </div>

        </div>

         <div class="box-body" id="ZoneDiv" runat="server" style="display: none">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Zone Wise Interactive DashBoard</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="Button23" runat="server" Text="Back" class="btn btn-primary" OnClientClick="back()" OnClick="btnBack_Click" />
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                <label for="exampleInputEmail1">Year:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddlZoneYear" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                                    <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                                    <asp:ListItem Text="2017" Value="2017" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                                    <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                                    <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                <label for="name">Zone :</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddlZoneRegion" runat="server" CssClass="form-control select2"></asp:DropDownList>
                            </div>
                        </div>
                        
                      <%--  <div class="box-footer">
                            <asp:Button ID="Button25" runat="server" class="btn btn-primary button" Visible="True" Text="Show"  OnClick="btnZoneShow_Click" />
                        </div>--%>
                         <div class="form-group col-lg-4 col-md-4 col-sm-6 col-xs-6">
                            <asp:Button ID="Button25" runat="server" class="btn btn-primary button" Visible="True" Text="Show"  OnClick="btnZoneShow_Click" />
                        </div>
                        <div class="form-group col-lg-4 col-md-4 col-sm-6 col-xs-6">
                            <!-- small box -->
                            <div class="small-box bg-light-blue" runat="server" id="ZoneShowDiv" style="display: none;">
                                <div class="inner">
                                    <p>Total Sale</p>
                                    <label id="lblZoneShowTotal" class="totalfont" runat="server"></label>
                                </div>
                            </div>
                        </div>
                                <div class="clearfix"></div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                         
                    </div>
                </div>
            </div>

        </div>

          <div class="row" id="ZoneDashboard"  runat="server" style="display: none;">
                <div class="col-xs-12">
                   
                        <div id="Div13" class="row table-responsive" runat="server">
                            <telerik:RadDockLayout runat="server" ID="rdlZone">
                                <telerik:RadDockZone runat="server" ID="rdzZone" MinWidth="250" MinHeight="340" Style="margin-top: 15px; max-width: 100%; max-height:100%" CssClass="col-lg-12">
                                </telerik:RadDockZone>
                                <telerik:RadDockZone runat="server" ID="rdzPieZone" MinWidth="250" MinHeight="340" Style="margin-top: 15px; max-width: 100%; max-height:100%" CssClass="col-lg-12">
                                </telerik:RadDockZone>
                                <telerik:RadDockZone runat="server" ID="rdzBarZone" MinWidth="250" MinHeight="340" Style="margin-top: 15px; max-width: 100%; max-height:100%" CssClass="col-lg-12">
                                </telerik:RadDockZone>
                            </telerik:RadDockLayout>
                                             
                        </div>
                        <!-- /.box-body -->
                
                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>

            <div class="box-body" id="AttendanceDiv" runat="server" style="display: none">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Target vs Achievement</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="Button26" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->

                        <div class="box-body">
                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12">
                               <label for="exampleInputEmail1">Sales Person:</label>
                                                <asp:DropDownList ID="cmbPerson" runat="server" DataValueField="SMId" DataTextField="SMName" CssClass="form-control select2" AutoPostBack="True"  OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" DataSourceID="SqlDataSource1"></asp:DropDownList>
                            </div>
                            <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                <label for="exampleInputEmail1">Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender8" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender112" TargetControlID="txtDate"></ajaxToolkit:CalendarExtender>
                            </div>
                         
                           <asp:Button runat="server" ID="btn1" ClientIDMode="Static" OnClick="btn1_Click" Style="border: medium none; background: transparent none repeat scroll 0% 0%; color: transparent;" />
    <input id="chartselector" type="text" runat="server" value="" style="border: medium none; background: transparent none repeat scroll 0% 0%; color: transparent;" />
                        </div>
 <div class="box-footer">
                                            <div class="row">
                                                <div class="col-md-12 col-lg-12">
                                                    <div class="demo-container size-wide">
                                                        <telerik:RadHtmlChart  runat="server" ID="PieChart1" Height="500">
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
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="box-body table-responsive" id="maingrid" runat="server">
                                                    <telerik:RadGrid ID="levelGrid1"
                                                        OnColumnCreated="levelGrid1_ColumnCreated"
                                                        AutoGenerateColumns="true"
                                                        runat="server" OnDetailTableDataBind="levelGrid1_DetailTableDataBind" DataSourceID="SqlDataSource3">
                                                        <MasterTableView HierarchyLoadMode="Client">
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
          <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
        </div>
         <div class="content" id="CategorySaleDiv" runat="server" style="display: none;">
            <div class="row" style="float: right;">
                <asp:Button Style="margin-right: 5px;" type="button" ID="categoryBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" OnClientClick="back()" />
            </div>
            <div class="row">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="col-md-4  col-lg-4">
                            <div class="panel panel-default">
                                <div class="panel-heading">Distribution : Category Wise Sale Dashboard</div>
                                <div class="panel-body">

                                    <ul class="colornote">
                                        <li>View By:
                                                    <asp:RadioButtonList ID="Categoryrbl" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Selected="True" Text="Product Group" Value="PG"></asp:ListItem>
                                                        <asp:ListItem Text="Product Class" Value="PC"></asp:ListItem>
                                                        <asp:ListItem Text="Product Segment" Value="PS"></asp:ListItem>
                                                    </asp:RadioButtonList></li>
                                        <li>From Date:  
                                                    <asp:TextBox ID="categorytxtFromDate" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender9" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender2" TargetControlID="categorytxtFromDate"></ajaxToolkit:CalendarExtender>
                                        </li>
                                        <li>To: 
                                                    <asp:TextBox ID="categorytxtTo" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender10" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender3" TargetControlID="categorytxtTo"></ajaxToolkit:CalendarExtender>
                                        </li>
                                        <li>State:
                                                    <asp:ListBox ID="categoryddlState" CssClass="form-control" Width="100%" runat="server" OnSelectedIndexChanged="categoryddlState_SelectedIndexChanged" AutoPostBack="true" SelectionMode="Multiple"></asp:ListBox></li>
                                        <li>District:
                                                    <asp:ListBox ID="categoryddlDistrict" CssClass="form-control" Width="100%" runat="server" OnSelectedIndexChanged="categoryddlDistrict_SelectedIndexChanged" AutoPostBack="true" SelectionMode="Multiple"></asp:ListBox></li>
                                        <li>City: 
                                                    <asp:ListBox ID="categoryddlCity" CssClass="form-control" Width="100%" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="categoryddlCity_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox></li>
                                        <li>Distributor:
                                                    <asp:ListBox ID="categoryddlDistributor" CssClass="form-control" Width="100%" runat="server" SelectionMode="Multiple"></asp:ListBox></li>
                                        <li>
                                            <asp:Button ID="Button1" runat="server" class="btn btn-primary button" Visible="True" Text="Show" OnClientClick="showspinner();" OnClick="categorybtnShow_Click" /></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-8  col-lg-8" runat="server" id="CategorySaleDashboard" style="display: none;">
                            <div class="demo-container no-bg" runat="server">
                                <div id="Div1" runat="server">
                                    <telerik:RadHtmlChart runat="server" ID="pieChart" Width="100%" Height="500" Transitions="false"></telerik:RadHtmlChart>
                                </div>
                                <div class="price-slider" runat="server" id="CategoryDiv" style="display: none;">
                                    <h4 class="great">Total Amount : <span> <label id="lblCategory"  runat="server"></label></span></h4>
                                    </div>
                                </div>
                            </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="categoryddlState" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="categoryddlDistrict" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="categoryddlCity" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="categoryddlDistributor" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="categoryBack" EventName="Click" />
                    <%--     <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />--%>
                    <%--    <asp:PostBackTrigger ControlID="Button1"/>--%>
                    </Triggers>
                </asp:UpdatePanel>
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

        function showspinner() {

            $("#spinner").show();

        };
        function hidespinner() {

            $("#spinner").hide();

        };
       
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
    <style>
        .ddleft {
            margin-left: 10px;
        }

        .RadDockZone {
            box-sizing: border-box;
        }
         
             .RadDock.linedock .rdContent > div {
            padding: 5px 0;
        }
              .managemargin .RadDock .rdContent {
            margin-left: 0;
            margin-right: 0;
        }
        .lbl {
            font-size:medium;
            text-align:center;
           color:white;
         
        }
    </style>
    <style>
     
        .RadHtmlChart {
            width: 100% !important;
        }
        .RadRadialGaugeHeight {
            margin-top:1px;
          
        }
        .divborder {
    border: 2px solid #a1a1a1;
    padding: 10px 40px; 
    background: #dddddd;
    border-radius: 25px;
}
    </style>
</asp:Content>