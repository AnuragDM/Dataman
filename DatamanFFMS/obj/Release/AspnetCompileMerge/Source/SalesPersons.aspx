<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="SalesPersons.aspx.cs" Inherits="AstralFFMS.SalesRep_List" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <script src="plugins/jquery.numeric.min.js"></script>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".numeric").numeric({ decimal: false, negative: false });
        });
    </script>

    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

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

        @media (max-width: 600px) {
            .formlay {
                width: 100% !important;
                height: 60px;
            }
        }
    </style>
    <script type="text/javascript">
        /////////////BindUser
        $(document).ready(function () {
           // ClearControls();
            //BindUser();
            //BindReportTo();
            //BindCity();
            //BindDepartment();
            //BindeDesignation();
            //BindResCentre();
            //BindRole()
        }
         );
        /////////////BindUser

<%--        function BindUser() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
                  var obj = { EmpId: 0 };
                  $('#<%=ddlusers.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                  $.ajax({
                      type: "POST",
                      url: pageUrl + '/PopulateUser',
                      data: JSON.stringify(obj),
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      success: OnUserPopulated,
                      failure: function (response) {
                          alert(response.d);
                      }
                  });
                  function OnUserPopulated(response) {
                      PopulateUserControl(response.d, $("#<%=ddlusers.ClientID %>"));
                  }
                  function PopulateUserControl(list, control) {
                      if (list.length > 0) {
                          //  control.removeAttr("disabled");
                          control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                          $.each(list, function () {
                              control.append($("<option></option>").val(this['Value']).html(this['Text']));
                          });
                          var id = $('#<%=HiddenUserID.ClientID%>').val();
                          //  alert(id);
                          if (id != "") {
                              $('#<%=ddlusers.ClientID%>').val(id);
                          }
                      }
                      else {
                          control.empty().append('<option selected="selected" value="0">Not available<option>');
                      }

                  }
              }--%>



              ////////BindReportTo

<%--              function BindReportTo() {
                  var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
                    var obj = { ReportTo: 0 };
                    $('#<%=ddlUnderSales.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                    $.ajax({
                        type: "POST",
                        url: pageUrl + '/PopulateReportTo',
                        data: JSON.stringify(obj),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: OnReportPopulated,
                        failure: function (response) {
                            alert(response.d);
                        }
                    });
                    function OnReportPopulated(response) {
                        PopulateReportControl(response.d, $("#<%=ddlUnderSales.ClientID %>"));
                    }
                    function PopulateReportControl(list, control) {
                        if (list.length > 0) {
                            //   control.removeAttr("disabled");
                            control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                            $.each(list, function () {
                                control.append($("<option></option>").val(this['Value']).html(this['Text']));
                            });
                            var id = $('#<%=HiddenReportToID.ClientID%>').val();
                            if (id != "") {
                                $('#<%=ddlUnderSales.ClientID%>').val(id);
                            }
                        }
                        else {
                            control.empty().append('<option selected="selected" value="0">Not available<option>');
                        }
                    }
                    ////////BindReportTo

                }--%>

                ///////////BindDepartment

<%--                function BindDepartment() {
                    var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
                    var obj = { DeptId: 0 };
                    $('#<%=ddldept.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                    $.ajax({
                        type: "POST",
                        url: pageUrl + '/PopulateDepartment',
                        data: JSON.stringify(obj),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: OnDeptPopulated,
                        failure: function (response) {
                            alert(response.d);
                        }
                    });
                    function OnDeptPopulated(response) {
                        PopulateDeptControl(response.d, $("#<%=ddldept.ClientID %>"));
                    }
                    function PopulateDeptControl(list, control) {
                        if (list.length > 0) {
                            //  control.removeAttr("disabled");
                            control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                            $.each(list, function () {
                                control.append($("<option></option>").val(this['Value']).html(this['Text']));
                            });
                            var id = $('#<%=HiddenDeptID.ClientID%>').val();
                            //   alert(id);
                            if (id != "") {
                                $('#<%=ddldept.ClientID%>').val(id);
                            }
                        }
                        else {
                            control.empty().append('<option selected="selected" value="0">Not available<option>');
                        }
                       // $("#<%=ddldept.ClientID %>").attr("disabled", "disabled");
                    }

                }--%>
                /////////BindDepartment
                //////BindDesPopulateDesignation

<%--                function BindeDesignation() {
                    var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
             var obj = { DesigId: 0 };
             $('#<%=ddldesg.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
             $.ajax({
                 type: "POST",
                 url: pageUrl + '/PopulateDesignation',
                 data: JSON.stringify(obj),
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: OnDesignPopulated,
                 failure: function (response) {
                     alert(response.d);
                 }
             });
             function OnDesignPopulated(response) {
                 PopulateDesignControl(response.d, $("#<%=ddldesg.ClientID %>"));
            }
            function PopulateDesignControl(list, control) {
                if (list.length > 0) {
                    // control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(list, function () {
                        control.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });
                    var id = $('#<%=HiddenDesID.ClientID%>').val();
                     // alert(id);
                     if (id != "") {
                         $('#<%=ddldesg.ClientID%>').val(id);
                            }
                        }
                        else {
                            control.empty().append('<option selected="selected" value="0">Not available<option>');
                        }
                       // $("#<%=ddldesg.ClientID %>").attr("disabled", "disabled");
             }


         }--%>
         ////////////////BindDesignation
         //////BindPopulateResCentre
<%--         function BindResCentre() {
             var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
             var obj = { ResCenId: 0 };
             $('#<%=ddlResCenId.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
             $.ajax({
                 type: "POST",
                 url: pageUrl + '/PopulateResCentre',
                 data: JSON.stringify(obj),
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: OnResCentrePopulated,
                 failure: function (response) {
                     alert(response.d);
                 }
             });
             function OnResCentrePopulated(response) {
                 PopulateResCentreControl(response.d, $("#<%=ddlResCenId.ClientID %>"));
                          }
                          function PopulateResCentreControl(list, control) {
                              if (list.length > 0) {
                                  //control.removeAttr("disabled");
                                  control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                                  $.each(list, function () {
                                      control.append($("<option></option>").val(this['Value']).html(this['Text']));
                                  });
                                  var id = $('#<%=HiddenResCentre.ClientID%>').val();
                            // alert(id);
                            if (id != "") {
                                $('#<%=ddlResCenId.ClientID%>').val(id);
                            }
                        }
                        else {
                            control.empty().append('<option selected="selected" value="0">Not available<option>');
                        }
                    }

                }--%>

                /////BindResCentre
                ///////////Bind City
<%--          function BindCity() {
                    var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
              var obj = { CityId: 0,RecType:'' };
             $('#<%=ddlcity.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
             $.ajax({
                 type: "POST",
                 url: pageUrl + '/PopulateCity',
                 data: JSON.stringify(obj),
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: OnCityPopulated,
                 failure: function (response) {
                     alert(response.d);
                 }
             });
             function OnCityPopulated(response) {
                 PopulateCityControl(response.d, $("#<%=ddlcity.ClientID %>"));
             }
             function PopulateCityControl(list, control) {
                 if (list.length > 0) {
                     // control.removeAttr("disabled");
                     control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                     $.each(list, function () {
                         control.append($("<option></option>").val(this['Value']).html(this['Text']));
                     });
                     var id = $('#<%=HiddenCityID.ClientID%>').val();
                             //     alert(id);
                                  if (id != "") {
                                      $('#<%=ddlcity.ClientID%>').val(id);
                            }
                        }
                        else {
                            control.empty().append('<option selected="selected" value="0">Not available<option>');
                        }
                    }

                }--%>

                /////////////Bind City
                ///BindRole
<%--                function BindRole() {
                    var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
                    var obj = { RoleType: '' };
             $('#<%=ddlRole.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                      $.ajax({
                          type: "POST",
                          url: pageUrl + '/PopulateRole',
                           data: JSON.stringify(obj),
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          success: OnRolePopulated,
                          failure: function (response) {
                              alert(response.d);
                          }
                      });
                      function OnRolePopulated(response) {
                          PopulateRoleControl(response.d, $("#<%=ddlRole.ClientID %>"));
             }
             function PopulateRoleControl(list, control) {
                 if (list.length > 0) {
                     //control.removeAttr("disabled");
                     control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                     $.each(list, function () {
                         control.append($("<option></option>").val(this['Value']).html(this['Text']));
                     });
                     var id = $('#<%=HiddenRoleID.ClientID%>').val();

                     if (id != "") {
                         $('#<%=ddlRole.ClientID%>').val(id);
                                  }
                              }
                              else {
                                  control.empty().append('<option selected="selected" value="0">Not available<option>');
                              }
                          }

                      }--%>

                      ////BindRole
    </script>
  
    <script type="text/javascript">
        function FillUserDetail() {
            var pageUrl = '<%=ResolveUrl("SalesPersons.aspx/FillDetailUser")%>'
            var obj = { UserId: $('#<%=ddlusers.ClientID %>').val() };
            // $('#<%=ddlusers.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: pageUrl,
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (savingStatus) {
                    $('#<%=ddlRole.ClientID %>').val(savingStatus.d[0]["Role"]);
                    $('#<%=ddldept.ClientID %>').val(savingStatus.d[0]["Department"])///.selectBox("options", $('#<%=ddldept.ClientID %>').html());                  
                    $('#<%=ddldesg.ClientID %>').val(savingStatus.d[0]["Designation"])///.selectBox("options", $('#<%=ddldesg.ClientID %>').html());
                    $('#<%=HiddenRoleID.ClientID%>').val(savingStatus.d[0]["Role"]);
                    $('#<%=HiddenDeptID.ClientID%>').val(savingStatus.d[0]["Department"]);
                    $('#<%=HiddenDesID.ClientID%>').val(savingStatus.d[0]["Designation"]);
                    $('#<%=HiddenEmailID.ClientID%>').val(savingStatus.d[0]["Email"]);
                    $('#<%=HiddenEmployee.ClientID%>').val(savingStatus.d[0]["Employee"]);
                    $('#<%=EmpName.ClientID %>').val(savingStatus.d[0]["Employee"]);
                    $('#<%=Email.ClientID%>').val(savingStatus.d[0]["Email"]);
                    // alert( $('#<%=ddlRole.ClientID %>').val())
                    /// alert($('#<%=ddldept.ClientID %>').val(savingStatus.d[0]["Department"]));
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

    </script>
    <script type="text/javascript">
        function FillSMControls(i) {
            // alert(i);
            var pageUrl = '<%=ResolveUrl("SalesPersons.aspx/FillSMControls_Web")%>'
            var obj = { SmId: i };
            // $('#<%=ddlusers.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: pageUrl,
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (savingStatus) {
                     $('#<%=HiddenUserID.ClientID%>').val(savingStatus.d[0]["UserID"]);
                     $('#<%=HiddenRoleID.ClientID%>').val(savingStatus.d[0]["RoleID"]);
                     $('#<%=HiddenResCentre.ClientID%>').val(savingStatus.d[0]["ResCentre"]);
                     $('#<%=HiddenReportToID.ClientID%>').val(savingStatus.d[0]["ReportToID"]);
                     $('#<%=HiddenDeptID.ClientID%>').val(savingStatus.d[0]["DeptID"]);
                     $('#<%=HiddenDesID.ClientID%>').val(savingStatus.d[0]["DesID"]);
                     $('#<%=HiddenCityID.ClientID%>').val(savingStatus.d[0]["CityID"])
                     $('#<%=HiddenEmailID.ClientID%>').val(savingStatus.d[0]["Email"]);
                     $('#<%=HiddenEmployee.ClientID%>').val(savingStatus.d[0]["EmpName"]);
                     $('#<%=SMName.ClientID %>').val(savingStatus.d[0]["SMName"]);
                     $('#<%=SyncId.ClientID %>').val(savingStatus.d[0]["SyncId"]);
                     $('#<%=DOA.ClientID %>').val(savingStatus.d[0]["DOA"]);
                     $('#<%=Address1.ClientID %>').val(savingStatus.d[0]["Address1"]);
                     $('#<%=Address2.ClientID %>').val(savingStatus.d[0]["Address2"]);
                     $('#<%=EmpName.ClientID %>').val(savingStatus.d[0]["EmpName"]);
                     $('#<%=Pin.ClientID %>').val(savingStatus.d[0]["Pin"]);
                     $('#<%=Mobile.ClientID %>').val(savingStatus.d[0]["Mobile"]);
                     $('#<%=DeviceNo.ClientID %>').val(savingStatus.d[0]["DeviceNo"]);
                     $('#<%=DOB.ClientID %>').val(savingStatus.d[0]["DOB"]);
                     $('#<%=Email.ClientID %>').val(savingStatus.d[0]["Email"]);
                     $('#<%=Remarks.ClientID %>').val(savingStatus.d[0]["Remarks"]);
                     $('#<%=DSRAllowDays.ClientID %>').val(savingStatus.d[0]["DSRAllowDays"]);
                     //BindUser();
                     //BindReportTo();
                     //BindCity();
                     //BindDepartment();
                     //BindeDesignation();
                     //BindResCentre();
                     //BindRole()
                     <%--   $('#<%=ddlRole.ClientID %>').val(savingStatus.d[0]["RoleID"]);
                     $('#<%=ddldesg.ClientID %>').val(savingStatus.d[0]["DesigId"]);
                     $('#<%=ddldept.ClientID %>').val(savingStatus.d[0]["DeptId"]);
                     $('#<%=ddlUnderSales.ClientID %>').val(savingStatus.d[0]["UnderId"]);
                     $('#<%=ddlResCenId.ClientID %>').val(savingStatus.d[0]["ResCenId"]);
                     $('#<%=ddlcity.ClientID %>').val(savingStatus.d[0]["CityId"]);
                     $('#<%=ddlusers.ClientID %>').val(savingStatus.d[0]["UserId"]);  --%>
                     $('#<%=ddlSalesPerType.ClientID%>').val(savingStatus.d[0]["SalesRepType"]);
                     if (savingStatus.d[0]["chkhomecity"] == "true") {
                         $('#<%=chkhomecity.ClientID%>').prop('checked', true);
                     }
                     else {

                         $('#<%=chkhomecity.ClientID%>').prop('checked', false);
                     }
                     if (savingStatus.d[0]["chkIsActive"] == "true") {
                         $('#<%=chkIsActive.ClientID%>').prop('checked', true);
                         $('#<%=BlockReason.ClientID %>').val("");
                     }
                     else {
                         $('#<%=divblock.ClientID%>').removeClass("hidden");
                         $('#<%=BlockReason.ClientID %>').val(savingStatus.d[0]["BlockReason"]);
                         $('#<%=chkIsActive.ClientID%>').prop('checked', false);
                     }
                    $('#<%=btnSave.ClientID%>').prop('value', savingStatus.d[0]["save"]);
                    $('#<%=HiddenbtnTest.ClientID%>').val(savingStatus.d[0]["save"]);
                   $('#<%=btnDelete.ClientID%>').css("visibility", "visible");
                  //  alert("Tesdgd");
                   // $('#<%=btnDelete.ClientID%>').visible = true;
                    document.getElementById('ContentPlaceHolder1_btnDelete').style.visibility = 'visible';
                   // $('#<%=btnDelete.ClientID%>').show();
                 },
                 failure: function (response) {
                     alert(response.d);
                 }
             });
         }



    </script>
    <script type="text/javascript">
        function ClearControls() {
            $('#<%=SMName.ClientID%>').val("");
             $('#<%=Email.ClientID%>').val("");
             $('#<%=Mobile.ClientID%>').val("");
             $('#<%=Pin.ClientID%>').val("");
             $('#<%=Address1.ClientID%>').val("");
             $('#<%=Address2.ClientID%>').val("");
             $('#<%=DSRAllowDays.ClientID%>').val("");
             $('#<%=ddlRole.ClientID %>').val("0");
             $('#<%=ddldept.ClientID %>').val("0");
             $('#<%=ddlSalesPerType.ClientID %>').val("0");
             $('#<%=ddlUnderSales.ClientID %>').val("0");
             $('#<%=ddldesg.ClientID %>').val("0");
             $('#<%=ddlResCenId.ClientID %>').val("0");
             $('#<%=ddlcity.ClientID %>').val("0");
             $('#<%=DOA.ClientID %>').val("");
             $('#<%=DOB.ClientID %>').val("");
             $('#<%=Remarks.ClientID %>').val("");
             $('#<%=SyncId.ClientID %>').val("");
             $('#<%=ddlusers.ClientID %>').val("0");
             $('#<%=chkIsActive.ClientID%>').prop('checked', true);
             $('#<%=BlockReason.ClientID%>').val("");
             $('#<%=DeviceNo.ClientID%>').val("");
             $('#<%=EmpName.ClientID%>').val("");
         <%--    $('#<%=HiddenUserID.ClientID%>').val("");
             $('#<%=HiddenRoleID.ClientID%>').val("");
             $('#<%=HiddenResCentre.ClientID%>').val("");
             $('#<%=HiddenReportToID.ClientID%>').val("");
             $('#<%=HiddenDeptID.ClientID%>').val("");
             $('#<%=HiddenDesID.ClientID%>').val("");
             $('#<%=HiddenCityID.ClientID%>').val("");--%>
         }

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
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to delete?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <script type="text/javascript">
        function showpreview(input) {
            var uploadcontrol = document.getElementById('<%=FileUpload1.ClientID%>').value;
            var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
            if (uploadcontrol.length > 0) {
                if (reg.test(uploadcontrol)) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $("#ContentPlaceHolder1_imgpreview").css('display', 'block');
                            $("#ContentPlaceHolder1_imgpreview").attr('src', e.target.result);
                        }
                        reader.readAsDataURL(input.files[0]);
                    }
                }
                else {
                    errormessage("Only image files are allowed!");
                    $("#ContentPlaceHolder1_imgpreview").css('display', 'none');
                    return false;
                }
            }
        }

        //valdation check
        function validate() {
            if ($('#<%=SMName.ClientID%>').val() == "") {
                errormessage("Please Enter Employee Name.");
                return false;
            }
            if ($('#<%=txtusername.ClientID%>').val() == "") {
                errormessage("Please Enter User Name.");
                return false;
            }
            var value = ($('#<%=SMName.ClientID%>').val().charAt(0));
            var chrcode = value.charCodeAt(0);
            if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                errormessage("Do not start Name with special characters.")
                return false;
            }
            if ($('#<%=ddlRole.ClientID%>').val() == "0") {
                errormessage("Please select  Role.");
                return false;
            }
            <%--if ($('#<%=EmpName.ClientID%>').val() == "") {
                errormessage("Please Enter Employee Name.");
                return false;
            }--%>
          <%--  if ($('#<%=ddlSalesPerType.ClientID%>').val() == "0") {
                errormessage("Please select Sales Person Type.");
                return false;
            }--%>
            if ($('#<%=ddlUnderSales.ClientID%>').val() == "0") {
                errormessage("Please select Reporting Person.");
                return false;
            }
            if ($('#<%=ddlResCenId.ClientID%>').val() == "0") {
                errormessage("Please select Responsibility Centre.");
                return false;
            }
            if ($('#<%=ddldept.ClientID%>').val() == "0") {
                errormessage("Please select  Department.");
                return false;
            }
            if ($('#<%=ddldesg.ClientID%>').val() == "0") {
                errormessage("Please select Designation.");
                return false;
            }
            <%--if ($('#<%=ddlgrade.ClientID%>').val() == "0") {
                errormessage("Please select a Grade.");
                return false;
            }--%>
            if ($('#<%=ddldesg.ClientID%>').val() == "0") {
                errormessage("Please select a Designation.");
                return false;
            }

            if ($('#<%=ddlcity.ClientID%>').val() == "0") {
                errormessage("Please select a City.");
                return false;
            }
            if ($('#<%=Pin.ClientID%>').val() != "") {
                varpnlLength = "";
                varpnlLength = ($('#<%=Pin.ClientID%>').val().length);
                if (varpnlLength < 6) {
                    errormessage("Please Enter 6 digit Pincode");
                    return false;
                }
            }
            if ($('#<%=Mobile.ClientID%>').val() == "") {
                errormessage("Please Enter Mobile No.");
                return false;
            }
            varmblLength = "";
            varmblLength = ($('#<%=Mobile.ClientID%>').val().length);
            if (varmblLength < 10) {
                errormessage("Please Enter 10 digit mobile No.");
                return false;
            }
            <%--if ($('#<%=DeviceNo.ClientID%>').val() == "") {
                errormessage("Please Enter Unique Device No.");
                return false;
            }--%>
            if ($('#<%=Email.ClientID%>').val() == "") {
                errormessage("Please Enter Email ID.");
                return false;
            }
            if ($('#<%=Email.ClientID%>').val() != "") {
                var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
                var emailList = $('#<%=Email.ClientID%>').val();

                if (!(emailList.trim()).match(mailformat)) {
                    errormessage("Invalid Email Address.");
                    return false;
                    //}
                }
            }
           <%-- if ($('#<%=DSRAllowDays.ClientID%>').val() == "") {
                errormessage("Please Enter DSR Allow Days.");
                return false;
            }--%>
            <%--if ($('#<%=txtmeetallowdays.ClientID%>').val() == "") {
                errormessage("Please Enter Meet Allow Days.");
                return false;
            }--%>
           
            if ($('#<%=txtfromtime.ClientID%>').val() == "") {
                errormessage("Please Enter time");
                return false;
            }
            if (($('#<%=txtfromtime.ClientID%>').val() == "") || ($('#<%=txtfromtime.ClientID%>').val() == "00:00")) {
                errormessage("Please Enter Valid From Time");
                $('#<%=txtfromtime.ClientID%>').focus();
                return false;
            }
            var digits = /^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$/;
            var digitsid = $('#<%=txtfromtime.ClientID%>').val();
            var digitsArray = digitsid.match(digits);
            var temp;
            if (digitsArray == null) {
                errormessage("From Time Format(HH:MM) seems incorrect. Please try again.");
                $('#<%=txtfromtime.ClientID%>').focus();
                return false;
            }

            if (($('#<%=txttotime.ClientID%>').val() == "") || ($('#<%=txttotime.ClientID%>').val() == "00:00")) {
                errormessage("Please Enter Valid To Time");
                $('#<%=txttotime.ClientID%>').focus();
                return false;
            }
            var digits = /^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$/;
            var digitsid = $('#<%=txttotime.ClientID%>').val();
            var digitsArray = digitsid.match(digits);
            var temp;
            if (digitsArray == null) {
                errormessage("To Time Format(HH:MM) seems incorrect. Please try again.");
                $('#<%=txttotime.ClientID%>').focus();
                return false;
            }


            var start = $('#<%=txtfromtime.ClientID%>').val();
            var end = $('#<%=txttotime.ClientID%>').val();
            var dtStart = new Date("1/1/2007 " + start);
            var dtEnd = new Date("1/1/2007 " + end);
            if (Date.parse(dtStart) > Date.parse(dtEnd)) {
                errormessage("To Time Should be greater then From Time");
                $('#<%=txttotime.ClientID%>').focus();
                return false;
            }

          <%--  if ($('#<%=txttotime.ClientID%>').val() == "") {
                errormessage("Please Enter To Time");
                return false;
            }--%>
            if ($('#<%=txtrecordinterval.ClientID%>').val() == "") {
                errormessage("Please Enter Record Interval");
                return false;

            }
            if ($('#<%=txtrecordinterval.ClientID%>').val() == "0") {
                errormessage("Record Interval can not be zero");
                return false;

            }
            if (($('#<%=txtuploadinterval.ClientID%>').val() == "")||(($('#<%=txtuploadinterval.ClientID%>').val() == "0"))) {
                errormessage("Upload Interval should be minimum of 5 minutes");
                return false;
            }
            if (($('#<%=txtaccuracy.ClientID%>').val() == "")|($('#<%=txtaccuracy.ClientID%>').val() == "0")) {
                errormessage("Please Enter Accuracy");
                return false;
            }
           
            if ($('#<%=ddlusers.ClientID%>').val() == "" || $('#<%=ddlusers.ClientID%>').val() == "0") {
                errormessage("Please select a User.");
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=chkIsActive.ClientID%>').change(function () {
                if (this.checked) {
                    $('#<%=divblock.ClientID%>').addClass("hidden");
                }
                else { $('#<%=divblock.ClientID%>').removeClass("hidden"); }
            });
        });

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            var valLength = "";
            $('#<%=SMName.ClientID%>').keypress(function (key) {

                valLength = ($('#<%=SMName.ClientID%>').val().length + 1);

                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
        });
    </script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to delete?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <script type="text/javascript">
        function DoNav(SMId) {
            if (SMId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', SMId)
            }
        }
    </script>

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
        <div class="box-body" id="mainDiv" style="display: none;" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Employee Master</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary" OnClick="btnFind_Click" />                             
                                <%--  <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" runat="server" />--%>
                            </div>
                            <%--<div style="float: right">                                                         
                                <asp:CheckBox ID="chkuser" Style="margin-right: 20px;" runat="server" CssClass="checkbox" Text="Create user" OnCheckedChanged="chkuser_CheckedChanged" AutoPostBack="true" Checked="false" />
                            </div>--%>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                                <div class="col-md-12">
                                    
                                    <div class="form-group formlay">
                                        <input id="SMId" hidden="hidden" />
                                        <label for="exampleInputEmail1">Employee Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input runat="server" type="text" class="form-control" maxlength="100" id="SMName" placeholder="Enter Sales Person" tabindex="2">
                                    </div>
                                     <div class="form-group formlay">
                                        <label for="exampleInputEmail1">User Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlusers" OnSelectedIndexChanged="ddlusers_SelectedIndexChanged" Visible="false" runat="server" CssClass="form-control" TabIndex="6" onchange="FillUserDetail()"></asp:DropDownList>
                                            <input runat="server" type="text" class="form-control" maxlength="100" id="txtusername" placeholder="Enter User Name" tabindex="4">
                                    </div>
                                       <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Role:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlRole" Width="100%"  CssClass="form-control" runat="server" AutoPostBack="true" TabIndex="6" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="form-group formlay" id="divsalestype" runat="server" visible="false">
                                        <label for="exampleInputEmail1">Salesperson Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlSalesPerType" Width="100%"  CssClass="form-control" runat="server" TabIndex="8" >
                                         
                                        </asp:DropDownList>
                                    </div>
                                   
                                 
                                    <div class="form-group formlay" runat="server" visible="false">
                                        <label for="exampleInputEmail1">Employee Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input runat="server" type="text" disabled="disabled" class="form-control" maxlength="100" id="EmpName" placeholder="Enter Employee Name" tabindex="10">
                                    </div>
                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Reporting Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlUnderSales" Width="100%" CssClass="form-control" runat="server" TabIndex="12"></asp:DropDownList>
                                    </div>
                                    <div class="form-group formlay" runat="server" visible="false">
                                        <label for="exampleInputEmail1">Responsibility  Centre:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlResCenId" Width="100%" CssClass="form-control" runat="server" TabIndex="14">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Department:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddldept" Width="100%"  CssClass="form-control" runat="server" TabIndex="16"></asp:DropDownList>
                                    </div>
                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Designation:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddldesg" Width="100%"  CssClass="form-control" runat="server" TabIndex="18"></asp:DropDownList>
                                    </div>
                                    <div class="form-group formlay" runat="server" visible="false">
                                        <label for="exampleInputEmail1">Grade:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlgrade" CssClass="form-control" runat="server" TabIndex="20"></asp:DropDownList>
                                    </div>

                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Date Of Birth:</label>
                                        <asp:TextBox ID="DOB" runat="server" CssClass="form-control" Style="background-color: white;" TabIndex="22"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender"
                                            TargetControlID="DOB"></ajaxToolkit:CalendarExtender>
                                    </div>

                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Address Line 1:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="150" id="Address1" tabindex="24" placeholder="Enter Address1">
                                    </div>
                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Address Line 2:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="150" id="Address2" placeholder="Enter Address2" tabindex="26">
                                    </div>

                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlcity" Width="100%" CssClass="form-control" runat="server" TabIndex="28">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Pincode:</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="6" id="Pin" placeholder="Enter Pincode" tabindex="30">
                                    </div>

                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Mobile:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="10" id="Mobile" placeholder="Enter Mobile No" tabindex="32">
                                        <asp:HiddenField ID="hdnoldMobile" runat="server" />
                                    </div>

                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Device No:</label>
                                        <%--&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>--%>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="20" id="DeviceNo" placeholder="Enter Device No" tabindex="34">
                                        <asp:HiddenField ID="hidolddeviceforlicence" runat="server" />
                                    </div>
                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Email ID:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server"  class="form-control" maxlength="50" id="Email" placeholder="Enter Email" tabindex="36">
                                    </div>
                                    <div class="form-group formlay"  runat="server" visible="false">
                                        <label for="exampleInputEmail1">Date Of Anniversary:</label>
                                        <asp:TextBox ID="DOA" runat="server" CssClass="form-control" Style="background-color: white;" TabIndex="38"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox2_CalendarExtender"
                                            TargetControlID="DOA"></ajaxToolkit:CalendarExtender>
                                    </div>

                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Remark:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="500" id="Remarks" placeholder="Enter Remark" tabindex="40">
                                    </div>
                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Sync Id:</label>
                                        <%--&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>--%>
                                        <input type="text" runat="server" class="form-control" maxlength="50" id="SyncId" placeholder="Enter Sync Id" tabindex="42">
                                    </div>
                                    <div class="form-group formlay" runat="server" visible="false">
                                        <label for="exampleInputEmail1">DSR Allow Days:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" class="form-control numeric text-right" maxlength="3" tabindex="42" runat="server" id="DSRAllowDays" placeholder="Enter DSR Allow Days">
                                    </div>
                                      <div class="form-group formlay" runat="server" visible="false">
                                        <label for="exampleInputEmail1">Meet Allow Days:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" class="form-control numeric text-right" maxlength="3" tabindex="42" runat="server" id="txtmeetallowdays" placeholder="Enter Meet Allow Days" value="0">
                                    </div>
                                    <%--  <div class="form-group formlay" style="display: none;">
                                        <label for="exampleInputEmail1">User Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <label style="padding-left: 5%;">[Login Credentials]</label>
                                        <input type="text" runat="server" class="form-control" maxlength="25" id="Username" placeholder="Enter User Name" tabindex="44" />
                                    </div>--%>
                                     <div class="form-group formlay" runat="server" >
                                        <label for="exampleInputEmail1">From Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" class="form-control" maxlength="5" tabindex="43" runat="server" id="txtfromtime" placeholder="Enter From Time" >
                                    </div>
                                      <div class="form-group formlay" runat="server" >
                                        <label for="exampleInputEmail1">To Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" class="form-control" maxlength="5" tabindex="44" runat="server" id="txttotime" placeholder="Enter To Time" >
                                    </div>
                                      <div class="form-group formlay" runat="server">
                                        <label for="exampleInputEmail1">Record Interval (In Seconds):</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" class="form-control numeric" maxlength="3" tabindex="45" runat="server" id="txtrecordinterval" placeholder="Enter Record Interval" >
                                    </div>
                                      <div class="form-group formlay" runat="server" >
                                        <label for="exampleInputEmail1">Upload Interval (In Seconds):</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" class="form-control numeric" maxlength="3" tabindex="46" runat="server" id="txtuploadinterval" placeholder="Enter Upload Intreval" >
                                    </div>
                                    <div class="form-group formlay" runat="server" >
                                        <label for="exampleInputEmail1">Accuracy:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" class="form-control numeric" maxlength="3" tabindex="47" runat="server" id="txtaccuracy" placeholder="Enter Accuracy" >
                                    </div>
                                      <div class="form-group formlay" runat="server" >
                                        <label for="exampleInputEmail1">Push Notification:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                         <asp:DropDownList ID="ddlpushnotification" Width="100%"  CssClass="form-control" runat="server" TabIndex="48" >
                                         <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                              <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                     <div class="form-group formlay" runat="server" >
                                        <label for="exampleInputEmail1">Battery Recording:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                         <asp:DropDownList ID="ddlbattery" Width="100%"  CssClass="form-control" runat="server" TabIndex="49" >
                                         <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                              <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                       <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Upload Image</label>
                                        <%--<input type="text" class="form-control numeric text-right" maxlength="3" tabindex="42" runat="server" id="Text1" placeholder="Enter DSR Allow Days">--%>
                                        <asp:FileUpload ID="FileUpload1"   runat="server"  tabindex="44"  onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" />
                                           <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                                ControlToValidate="FileUpload1" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                            <img id="imgpreview" height="150" width="150" class="pull-right" src="" style="border-width: 0px; display: none;" runat="server" />
                                    </div>
                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Active:</label>
                                        <input id="chkIsActive" type="checkbox" runat="server" checked="checked" tabindex="46" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <label for="exampleInputEmail1">Allow to change home city:</label>
                                        <input id="chkhomecity" type="checkbox" runat="server" tabindex="50" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <label for="exampleInputEmail1">Allow to Mobile Access:</label>
                                        <input id="chkmobAccess" type="checkbox" runat="server" tabindex="52" />&nbsp;&nbsp;&nbsp;&nbsp;                                     
                                        <span id="divblock" runat="server" class="hidden">
                                            <label for="exampleInputEmail1">Blocked Reason:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <input type="text" runat="server" class="form-control" maxlength="100" id="BlockReason" placeholder="Enter Block Reason" tabindex="48"></span>
                                    </div>
                                    <div class="form-group formlay">
                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="box-footer">
                            <asp:HiddenField ID="HiddenUserID" runat="server" />
                            <asp:HiddenField ID="HiddenReportToID" runat="server" />
                            <asp:HiddenField ID="HiddenRoleID" runat="server" />
                            <asp:HiddenField ID="HiddenResCentre" runat="server" />
                            <asp:HiddenField ID="HiddenDeptID" runat="server" />
                            <asp:HiddenField ID="HiddenDesID" runat="server" />
                            <asp:HiddenField ID="HiddenCityID" runat="server" />
                            <asp:HiddenField ID="HiddenEmailID" runat="server" />
                            <asp:HiddenField ID="HiddenEmployee" runat="server" />
                             <asp:HiddenField ID="HiddenbtnTest" runat="server" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClientClick="javascript:return validate();" OnClick="btnSave_Click" TabIndex="52" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" OnClick="btnDelete_Click" TabIndex="54" />
                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>


            </div>
        </div>


        <div class="box-body" id="rptmain" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Sales Person List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Sales Person Name</th>
                                                <th>Sales Person Type</th>
                                                <th>Reporting Person</th>
                                                <th>Department</th>
                                                <th>Designation</th>
                                                <th>Grade</th>
                                                <th>Device No</th>
                                                <th>Mobile No</th>
                                                <th>City</th>
                                                <th>Email Id</th>
                                                <th>User Name</th>
                                                <th>Role</th>
                                                <th>SyncId</th>
                                                <th>Active</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("SMId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("SMId") %>' />
                                        <td><%#Eval("SMName") %></td>
                                        <td><%#Eval("SalesRepType") %></td>
                                        <td><%#Eval("Parent") %></td>
                                        <td><%#Eval("Depname") %></td>
                                        <td><%#Eval("DesName") %></td>
                                        <td><%#Eval("grade") %></td>
                                        <td><%#Eval("DeviceNo") %></td>
                                        <td><%#Eval("Mobile") %></td>
                                        <td><%#Eval("City") %></td>
                                        <td><%#Eval("Email") %></td>
                                        <td><%#Eval("Username") %></td>
                                        <td><%#Eval("RoleName") %></td>
                                        <td><%#Eval("SyncId") %></td>
                                        <td><%#Eval("Active") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                        <!-- /.box-body -->
                    </div>
                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>

        </div>

    </section>
    <%-- <script type="text/javascript">
        $(document).ready(function () {
            var valLength = "";
            $("#DepName").keypress(function (key) {

                valLength = ($("#DepName").val().length + 1);

                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
        });
    </script>--%>
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
