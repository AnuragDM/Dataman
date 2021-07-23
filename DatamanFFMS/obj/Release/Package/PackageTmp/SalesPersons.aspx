<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="SalesPersons.aspx.cs" Inherits="AstralFFMS.SalesRep_List" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <script src="plugins/jquery.numeric.min.js"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
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

        .hiden {
            cursor: pointer;
            color: black;
            font-size: 15px;
            font-weight: 700;
            text-decoration: underline;
        }

        .multiselect-container.dropdown-menu {
            width: auto !important;
        }

        .shown {
            cursor: pointer;
            color: black;
            font-size: 15px;
            font-weight: 700;
            text-decoration: underline;
        }

        .ShowModal {
            cursor: pointer;
            color: black;
            font-size: 15px;
            font-weight: 700;
            text-decoration: underline;
        }

        .tdedit {
            cursor: pointer;
            color: #037fff;
            text-align: center !important;
        }

        .tdeditic {
            font-family: 'FontAwesome';
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

        #example1_wrapper .row {
            margin-right: 0px !important;
            margin-left: 0px !important;
        }

        #example2_wrapper .row {
            margin-right: 0px !important;
            margin-left: 0px !important;
        }

        #example1_wrapper .row .col-sm-12 {
            overflow-x: scroll !important;
            padding-left: 0px !important;
            margin-bottom: 10px;
        }

        #example2_wrapper .row .col-sm-12 {
            overflow-x: scroll !important;
            padding-left: 0px !important;
            margin-bottom: 10px;
            height: 310px;
        }

        select.form-control, .form-control {
            padding: 6px 12px !important;
            height: auto !important;
        }

        #ContentPlaceHolder1_mpePop_foregroundElement {
            width: 98%;
            left: 12px !important;
            top: 5px !important;
        }

        @media (max-width: 600px) {
            .formlay {
                width: 100% !important;
                height: 60px;
            }
        }
    </style>


    <script type="text/javascript">
        $(document).ready(function () {
            // #region GetSetStateData
            var obj;

            $.ajax({
                type: "POST",
                url: 'SalesPersons.aspx/PopulateState',
                contentType: "application/json; charset=utf-8",

                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {

                var data = JSON.parse(response.d);
                PopulateControl(data, $("#<%=lstState.ClientID %>"));
            }
            function PopulateControl(list, control) {

                console.log(list)
                var lstCustomers = $("[id*=lstState]");
                $('#<%=lstState.ClientID %>').empty();
                $.each(list, function () {

                    $('#<%=lstState.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');

                });

                $("#<%=lstState.ClientID %>").multiselect('rebuild');

                console.log($('#<%=lstState.ClientID %> ul'));
            }
        });
    </script>

    <script type="text/javascript">
        function BindState() {
            var obj;

            $.ajax({
                type: "POST",
                url: 'ActivityTemplateMapping.aspx/PopulateState',
                contentType: "application/json; charset=utf-8",

                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {

                var data = JSON.parse(response.d);
                PopulateControl(data, $("#<%=lstState.ClientID %>"));
            }
            function PopulateControl(list, control) {

                console.log(list)
                var lstCustomers = $("[id*=lstState]");
                $('#<%=lstState.ClientID %>').empty();
                $.each(list, function () {

                    $('#<%=lstState.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');

                });

                $("#<%=lstState.ClientID %>").multiselect('rebuild');

                console.log($('#<%=lstState.ClientID %> ul'));
            }
        }
    </script>

    <script type="text/javascript">
        function GetSMNAME(SMid) {
            $.ajax({
                type: "POST",
                url: "SalesPersons.aspx/PopulateSmname",
                data: '{SMID: "' + SMid + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSMSuccess,
                failure: function (response) {
                    //alert(response.d);
                },
                error: function (response) {
                    //alert(response.d);
                }
            });
        };

        function OnSMSuccess(response) {
            var data = JSON.parse(response.d);
            //if(response == true)
            //document.getElementById('#<%= lblPersonName.ClientID %>').value = data[0].SMName;
            //$("#<%=lblPersonName.ClientID %>").val(data[0].SMName)

            $("#<%=lblPersonName.ClientID %>").val(data[0].SMName);
   <%-- else
           document.getElementById('<%= lblPersonName.ClientID %>').value = 'Already in user!';--%>
        }
    </script>

    <script type="text/javascript">
        function GetSelected() {
            //Reference the Table.
            var grid = document.getElementById("example2");

            //Reference the CheckBoxes in Table.
            var checkBoxes = grid.getElementsByTagName("INPUT");
            var message = "";

            //Loop through the CheckBoxes.
            for (var i = 1; i < checkBoxes.length; i++) {
                if (checkBoxes[i].checked) {
                    //var row = checkBoxes[i].parentNode.parentNode;
                    //alert(checkBoxes[i].value);
                    if (message == "")
                    { message = checkBoxes[i].value + ","; }
                    else {
                        message += checkBoxes[i].value + ",";
                    }
                    //message += "   " + row.cells[2].innerHTML;
                    //message += "   " + row.cells[3].innerHTML;
                    //message += ",";
                }
            }

            //Display selected Row data in Alert Box.
            //alert(message);
            $("#<%=HiddenarIds.ClientID %>").val(message);
        }
    </script>

    <script type="text/javascript">

        function BindCitySearch() {

            var lstCity = $("[id*=lstCity]");
            var selectedState = [];

            $("#<%=lstState.ClientID %> :selected").each(function () {
                selectedState.push($(this).val());
            });
            $("#<%=HiddenState.ClientID %>").val(selectedState);
            if ($("#<%=HiddenState.ClientID %>").val() == "") { $("#<%=HiddenState.ClientID %>").val(0); }


            var obj = { StateID: $("#<%=HiddenState.ClientID %>").val() };
            $.ajax({
                type: "POST",
                url: 'SalesPersons.aspx/PopulateCityByMultiState',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                async: false,
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    //alert(response.d);
                }
            });
            function OnPopulated(response) {
                var data = JSON.parse(response.d);
                PopulateControl(data, $("#<%=lstCity.ClientID %>"));
            }
            function PopulateControl(list, control) {
                console.log(list)

                $('#<%=lstCity.ClientID %>').empty();
                $.each(list, function () {

                    $('#<%=lstCity.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');

                });
                $("#<%=lstCity.ClientID %>").multiselect('rebuild');
                var id = $('#<%=HiddenCty.ClientID%>').val();
                if (id != "") {
                    control.val(id);
                }
                //  
                console.log($('#<%=lstCity.ClientID %> ul'));
            }
            //BindDistributorSearch("S");
            //BindSalesPersonSearch("S");


        }
    </script>

    <script type="text/javascript">

        function BindAreaSearch() {

            var lstArea = $("[id*=lstArea]");
            var selectedCity = [];

            $("#<%=lstCity.ClientID %> :selected").each(function () {
                selectedCity.push($(this).val());
            });
            $("#<%=HiddenCty.ClientID %>").val(selectedCity);
            if ($("#<%=HiddenCty.ClientID %>").val() == "") { $("#<%=HiddenCty.ClientID %>").val(0); }


            var obj = { CityID: $("#<%=HiddenCty.ClientID %>").val() };
            $.ajax({
                type: "POST",
                url: 'SalesPersons.aspx/PopulateAreaByMultiState',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                async: false,
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    //alert(response.d);
                }
            });

            function OnPopulated(response) {
                var data = JSON.parse(response.d);
                PopulateControl(data, $("#<%=lstArea.ClientID %>"));
            }
            function PopulateControl(list, control) {
                console.log(list)

                $('#<%=lstArea.ClientID %>').empty();
                $.each(list, function () {

                    $('#<%=lstArea.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');

                });
                $("#<%=lstArea.ClientID %>").multiselect('rebuild');
                var id = $('#<%=HiddenArea.ClientID%>').val();
                if (id != "") {
                    control.val(id);
                }
                //  
                console.log($('#<%=lstArea.ClientID %> ul'));
            }
            //BindDistributorSearch("S");
            //BindSalesPersonSearch("S");


        }
    </script>

    <script type="text/javascript">

        function btnSubmitfunc() {
            var selectedValues = [];
            $("#<%=lstState.ClientID %> :selected").each(function () {
                selectedValues.push($(this).val());
            });

            if (selectedValues == "") {
                //alert(selectedValues);
                errormessage("Please Select State Name");
                $find('<%= mpePop.ClientID %>').hide();
                //$find('<%= assgn.ClientID %>').;
                return false;
            }
            //errormessage("Please Enter State Name.");
            //return false;
            $("#<%=HiddenState.ClientID %>").val(selectedValues);
            var selectedValues = [];
            $("#<%=lstCity.ClientID %> :selected").each(function () {
                selectedValues.push($(this).val());
            });
            $("#<%=HiddenCty.ClientID %>").val(selectedValues);
            var selectedValues = [];
            $("#<%=lstArea.ClientID %> :selected").each(function () {
                selectedValues.push($(this).val());
            });
            $("#<%=HiddenArea.ClientID %>").val(selectedValues);
            BindGridView();
        }

        function BindGridView() {
            $.ajax({
                type: "POST",
                url: "SalesPersons.aspx/GetDetail",
                data: '{StateId: "' + $("#<%=HiddenState.ClientID %>").val() + '",Cityid: "' + $("#<%=HiddenCty.ClientID %>").val() + '",Area: "' + $("#<%=HiddenArea.ClientID %>").val() + '",SMID: "' + $("#<%=HiddenSMID.ClientID %>").val() + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    //alert(response.d);
                },
                error: function (response) {
                    //alert(response.d);
                }
            });
        };

        function OnSuccess(response) {


            $('div[id$="rptDiv"]').show();
            var data = JSON.parse(response.d);
            //stateName") 
            //districtName
            //cityName") %
            //areaName") %
            debugger;
            var table = $('#example2').DataTable();
            table.destroy();
            $("#example2").DataTable({
                "order": [[0, "asc"]],
                //"short": false,
                //"lengthChange": false,
                "paging": false,
                //"searching": false,
                //"info": false,
                //"ordering": false,

                "aaData": data,
                "aoColumns": [
            {
                "mData": "LinkCde",
                "render": function (data, type, row, meta) {

                    if (data == "true")
                    { return ("<input type='checkbox' name='ForCheck' value='" + row["areaId"] + "' checked/>"); }
                    else {
                        return ("<input type='checkbox' name='ForCheck' value='" + row["areaId"] + "'/>");
                    }
                }
            },
            { "mData": "stateName" },
            { "mData": "districtName" },
            { "mData": "cityName" },
            { "mData": "areaName" }

                ]
            });


            $('#spinner').hide();
        }
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
            //$(".ShowModal").show();
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


        <%-- function BindState_New() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
            var obj = { CityId: 0 };
            $('#<%=ddlCity.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: pageUrl + '/PopulatePartyCity',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnCityPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnCityPopulated(response) {
                PopulateCityControl(response.d, $("#<%=ddlCity.ClientID %>"));
            }
            function PopulateCityControl(list, control) {
                if (list.length > 0) {
                    // control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(list, function () {
                        control.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });
                    var id = $('#<%=HiddenPartyCity.ClientID%>').val();
                    //     alert(id);
                    if (id != "") {
                        control.val(id);
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">Not available<option>');
                }
            }

        }--%>
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
            if (($('#<%=txtuploadinterval.ClientID%>').val() == "") || (($('#<%=txtuploadinterval.ClientID%>').val() == "0"))) {
                errormessage("Upload Interval should be minimum of 5 minutes");
                return false;
            }
            if (($('#<%=txtaccuracy.ClientID%>').val() == "") | ($('#<%=txtaccuracy.ClientID%>').val() == "0")) {
                errormessage("Please Enter Accuracy");
                return false;
            }

            if ($('#<%=ddlusers.ClientID%>').val() == "" || $('#<%=ddlusers.ClientID%>').val() == "0") {
                errormessage("Please select a User.");
                return false;
            }

            if ($('#<%=ddlHeadquarter.ClientID%>').val() == "0") {
                errormessage("Please select  Head Quarter.");
                return false;
            }
        }

        function getval() {

        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#example2').DataTable({
                'order': [0, 'asc']
            });
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

    <script type="text/javascript">
        $(document).ready(function () {
            $(".hiden").click(function () {
                $("#batrec").hide(700);
                $("#pshnot").hide(700);
                $("#rcrdint").hide(700);
                $("#upldint").hide(700);
                $("#accrcy").hide(700);
                $("#todiv").hide(700);
                $("#frmdiv").hide(700);
                $(".hiden").hide(700);
                $(".shown").show(700);
            });
            $(".shown").click(function () {
                $("#batrec").show(700);
                $("#pshnot").show(700);
                $("#rcrdint").show(700);
                $("#upldint").show(700);
                $("#accrcy").show(700);

                $("#todiv").show(700);
                $("#frmdiv").show(700);
                $(".shown").hide(700);
                $(".hiden").show(700);
            });
            $("#batrec").hide();
            $("#pshnot").hide();
            $("#rcrdint").hide();
            $("#upldint").hide();
            $("#accrcy").hide();
            $("#todiv").hide();
            $("#frmdiv").hide();
            $(".hiden").hide();
            //$(".ShowModal").hide();
            return false;
        });

        function DoMod(SMId) {
            //$("#<%=lblPersonName.ClientID %>").val(SMId);lblPersonName
            GetSMNAME(SMId);
            BindState();
            $('#<%=lstCity.ClientID %> > option').remove();
            $('#example2').dataTable().fnClearTable();
            $('#example2').dataTable().fnDraw();
            $('#example2').dataTable().fnDestroy();
            $("#<%=HiddenSMID.ClientID %>").val(SMId);
            $find('<%= mpePop.ClientID %>').show();
            return false;
        }

       <%-- $(document).ready(function () {
            $(".ShowModal").click(function () {

                $find('<%= mpePop.ClientID %>').show();
                return false;


            });
        });--%>

        function HideModalPopup() {
            //$('#<%=lstState.ClientID %>').empty();
            $find('<%= mpePop.ClientID %>').hide();
            return false;
        }

    </script>

    <script type="text/javascript">
        //$(function () {
        //    $('[id*=lstState]').multiselect({
        //        enableCaseInsensitiveFiltering: true,
        //        buttonWidth: '200px',GetMonthlyItemSaleGetMonthlyItemSale
        //        buttonWidth: '100%',
        //        includeSelectAllOption: true,
        //        maxHeight: 200,
        //        width: 215,
        //        enableFiltering: true,
        //        filterPlaceholder: 'Search'
        //    });
        //});
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

            $('[id*=lstCity]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });

            $('[id*=lstArea]').multiselect({
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
          function CompareDates(source, args) {
              var str1 = document.getElementById('<%=txtJoin.ClientID%>');
            var str2 = document.getElementById('<%=txtLeav.ClientID%>');
            var FromDate = str1.value.split('/');
            var EndDate = str2.value.split('/');
            var val = 'false';
            if (parseInt(FromDate[2]) < parseInt(EndDate[2])) {
                val = 'true';
                return true;
            }
            else if (parseInt(FromDate[2]) == parseInt(EndDate[2])) {
                if (parseInt(FromDate[0]) < parseInt(EndDate[0])) {
                    val = 'true';
                    return true;
                }
                else if (parseInt(FromDate[0]) == parseInt(EndDate[0])) {
                    if (parseInt(FromDate[1]) < parseInt(EndDate[1])) {
                        val = 'true';
                        return true;
                    }
                }
            }
            if (val == "false") {
                document.getElementById('<%=txtLeav.ClientID%>').value = "";
                errormessage("Leaving Date should not be greater than Joining Date");
                return false;
            }
        }
    </script>

    <script type="text/javascript">
        function CompareDate(source, args) {
            var today = new Date();
            var monthNames = ["Jan", "Feb", "Mar", "Apr",
                      "May", "Jun", "Jul", "Aug",
                      "Sep", "Oct", "Nov", "Dec"];
            var str1 = document.getElementById('<%=txtJoin.ClientID%>');
            var day = today.getDate();

            var monthIndex = today.getMonth();
            var monthName = monthNames[monthIndex];

            var year = today.getFullYear();

            var inpDate = day + "/" + monthName + "/" + year;
            var str2 = inpDate;
            var FromDate = str1.value.split('/');
            var EndDate = inpDate.split('/');

            var val = 'false';
            if (parseInt(FromDate[2]) < parseInt(EndDate[2])) {
                val = 'true';
                return true;
            }
            else if (parseInt(FromDate[2]) == parseInt(EndDate[2])) {
                if (parseInt(FromDate[0]) <= parseInt(EndDate[0])) {
                    val = 'true';
                    return true;
                }
                else if (parseInt(FromDate[0]) == parseInt(EndDate[0])) {
                    if (parseInt(FromDate[1]) < parseInt(EndDate[1])) {
                        val = 'true';
                        return true;
                    }
                }
            }

            if (val == "false") {
                document.getElementById('<%=txtJoin.ClientID%>').value = "";
                errormessage("Joining Date should not be greater than Today's Date");
                return false;
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
                            <%--<h3 class="box-title">Employee Master</h3>--%>
                            <h3 class="box-title">
                                <asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
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
                                <div class="col-md-4 col-sm-4 col-xs-12">

                                    <div class="form-group">
                                        <input id="SMId" hidden="hidden" />
                                        <label for="exampleInputEmail1">Employee Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input runat="server" type="text" class="form-control" maxlength="100" id="SMName" placeholder="Enter Sales Person" autocomplete="off">
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">User Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlusers" OnSelectedIndexChanged="ddlusers_SelectedIndexChanged" Visible="false" runat="server" CssClass="form-control" TabIndex="6" onchange="FillUserDetail()"></asp:DropDownList>
                                        <input runat="server" type="text" class="form-control" maxlength="100" id="txtusername" placeholder="Enter User Name" autocomplete="off">
                                    </div>
                                </div>
                                <%--  <asp:UpdatePanel ID="updatepnl" runat="server">
                                    <ContentTemplate>--%>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Role:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlRole" Width="100%" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group" id="divsalestype" runat="server">
                                        <label for="exampleInputEmail1">Salesperson Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlSalesPerType" Width="100%" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <%-- </ContentTemplate>
                                </asp:UpdatePanel>--%>
                                <div class="col-md-4 col-sm-4 col-xs-12" style="display: none">

                                    <div class="form-group" runat="server" visible="false">
                                        <label for="exampleInputEmail1">Employee Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input runat="server" type="text" disabled="disabled" class="form-control" maxlength="100" id="EmpName" placeholder="Enter Employee Name" autocomplete="off">
                                    </div>
                                </div>

                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Reporting Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlUnderSales" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Department:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddldept" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                            </div>

                            <div class="row">

                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Designation:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddldesg" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12" style="display: none">
                                    <div class="form-group" runat="server" visible="false">
                                        <label for="exampleInputEmail1">Responsibility  Centre:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlResCenId" Width="100%" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12" style="display: none">
                                    <div class="form-group formlay" runat="server" visible="false">
                                        <label for="exampleInputEmail1">Grade:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlgrade" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Date Of Birth:</label>
                                        <asp:TextBox ID="DOB" runat="server" CssClass="form-control" Style="background-color: white;" AutoCompleteType="Disabled"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender"
                                            TargetControlID="DOB"></ajaxToolkit:CalendarExtender>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Address Line 1:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="150" id="Address1" placeholder="Enter Address1" autocomplete="off">
                                    </div>
                                </div>

                            </div>

                            <div class="row">
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Address Line 2:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="150" id="Address2" placeholder="Enter Address2" autocomplete="off">
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlcity" Width="100%" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Pincode:</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="6" id="Pin" placeholder="Enter Pincode" autocomplete="off">
                                    </div>
                                </div>

                            </div>

                            <div class="row">
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Mobile:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="10" id="Mobile" placeholder="Enter Mobile No" autocomplete="off">
                                        <asp:HiddenField ID="hdnoldMobile" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Device No:</label>
                                        <%--&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>--%>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="20" id="DeviceNo" placeholder="Enter Device No" autocomplete="off">
                                        <asp:HiddenField ID="hidolddeviceforlicence" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Email ID:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="50" id="Email" placeholder="Enter Email" autocomplete="off">
                                    </div>
                                </div>

                                <div class="col-md-4 col-sm-4 col-xs-12" style="display: none">
                                    <div class="form-group" runat="server" visible="false">
                                        <label for="exampleInputEmail1">Date Of Anniversary:</label>
                                        <asp:TextBox ID="DOA" runat="server" CssClass="form-control" Style="background-color: white;" AutoCompleteType="Disabled"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox2_CalendarExtender"
                                            TargetControlID="DOA"></ajaxToolkit:CalendarExtender>
                                    </div>
                                </div>

                            </div>
                            <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>--%>
                            <div class="row">
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Remark:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="500" id="Remarks" placeholder="Enter Remark" autocomplete="off">
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Sync Id:</label>
                                        <%--&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>--%>
                                        <input type="text" runat="server" class="form-control" maxlength="50" id="SyncId" placeholder="Enter Sync Id" autocomplete="off">
                                    </div>
                                </div>

                                <div id="frmdiv" class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group" runat="server">
                                        <label for="exampleInputEmail1">From Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" class="form-control" maxlength="5" runat="server" id="txtfromtime" placeholder="Enter From Time" autocomplete="off" value="10:00">
                                    </div>
                                </div>
                                <div id="todiv" class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group" runat="server">
                                        <label for="exampleInputEmail1">To Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" class="form-control" maxlength="5" runat="server" id="txttotime" placeholder="Enter To Time" autocomplete="off" value="19:00">
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12" style="display: none">
                                    <div class="form-group" runat="server" visible="false">
                                        <label for="exampleInputEmail1">DSR Allow Days:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" class="form-control numeric text-right" maxlength="3" runat="server" id="DSRAllowDays" placeholder="Enter DSR Allow Days" autocomplete="off" value="1">
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12" style="display: none">
                                    <div class="form-group" runat="server" visible="false">
                                        <label for="exampleInputEmail1">Meet Allow Days:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" class="form-control numeric text-right" maxlength="3" runat="server" id="txtmeetallowdays" placeholder="Enter Meet Allow Days" value="0" autocomplete="off">
                                    </div>
                                </div>

                                <div id="rcrdint" class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group" runat="server">
                                        <label for="exampleInputEmail1">Record Interval (In Seconds):</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" class="form-control numeric" maxlength="3" runat="server" id="txtrecordinterval" placeholder="Enter Record Interval" autocomplete="off" value="100">
                                    </div>
                                </div>
                                <div id="upldint" class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group" runat="server">
                                        <label for="exampleInputEmail1">Upload Interval (In Seconds):</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" class="form-control numeric" maxlength="3" runat="server" id="txtuploadinterval" placeholder="Enter Upload Intreval" autocomplete="off" value="100">
                                    </div>
                                </div>
                                <div id="accrcy" class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group" runat="server">
                                        <label for="exampleInputEmail1">Accuracy:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" class="form-control numeric" maxlength="3" runat="server" id="txtaccuracy" placeholder="Enter Accuracy" autocomplete="off" value="100">
                                    </div>
                                </div>

                                <div id="pshnot" class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group" runat="server">
                                        <label for="exampleInputEmail1">Push Notification:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlpushnotification" Width="100%" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="batrec" class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group" runat="server">
                                        <label for="exampleInputEmail1">Battery Recording:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlbattery" Width="100%" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                  <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Head Quarter:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlHeadquarter" Width="100%" CssClass="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>

                                     <div class="col-md-4 col-sm-4 col-xs-12">
                                    <%--Anurag--%>
                                    <div class="form-group" runat="server">
                                        <label for="exampleInputEmail1">Joining Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtJoin" class="form-control" runat="server" Style="background-color: white;" AutoCompleteType="Disabled"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="txtJoin_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="txtJoin_CalendarExtender"
                                            TargetControlID="txtJoin"></ajaxToolkit:CalendarExtender>
                                        <asp:CustomValidator ClientValidationFunction="CompareDate" ID="cvfrmDate" runat="server"
                                            ErrorMessage="Joining date cannot be greater than Todays date" ControlToValidate="txtJoin"></asp:CustomValidator>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group" runat="server">
                                        <label for="exampleInputEmail1">Leaving Date:</label>

                                        <asp:TextBox ID="txtLeav" class="form-control" runat="server" Style="background-color: white;" AutoCompleteType="Disabled"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="txtLeav_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="txtLeav_CalendarExtender"
                                            TargetControlID="txtLeav"></ajaxToolkit:CalendarExtender>
                                        <asp:CustomValidator ClientValidationFunction="CompareDates" ID="cvTodate" runat="server"
                                            ErrorMessage="Leaving date cannot be Less than Joining date" ControlToValidate="txtLeav"></asp:CustomValidator>
                                    </div>
                                </div>

                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <%--end --%>

                                    <div class="form-group">
                                        <br />
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <label for="exampleInputEmail1">Active:</label>
                                            <input id="chkIsActive" type="checkbox" runat="server" checked="checked" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-12">
                                            <label for="exampleInputEmail1">Allow to change home city:</label>
                                            <input id="chkhomecity" type="checkbox" runat="server" />
                                        </div>
                                        <%--<label for="exampleInputEmail1">Field App:</label>
                                        <input id="chkmobAccess" type="checkbox" runat="server" tabindex="52" />&nbsp;&nbsp;&nbsp;&nbsp; 
                                        <div id="dvchk" runat="server" visible="false">
                                        <label for="exampleInputEmail1">Manager App:</label>
                                        <input id="chkmanager" type="checkbox" runat="server" tabindex="53" />&nbsp;&nbsp;&nbsp;&nbsp; 
                                        </div>       
                                        <label for="exampleInputEmail1">DMT App:</label>
                                        <input id="chkDMT" type="checkbox" runat="server" tabindex="54" />&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                    </div>
                                </div>
                                <div class="clearfix"></div>



                            </div>

                            <%--  <div class="form-group formlay" style="display: none;">
                                        <label for="exampleInputEmail1">User Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <label style="padding-left: 5%;">[Login Credentials]</label>
                                        <input type="text" runat="server" class="form-control" maxlength="25" id="Username" placeholder="Enter User Name" tabindex="44" />
                                    </div>--%>

                            <%--   </ContentTemplate>
                            </asp:UpdatePanel>--%>
                            <div class="row">
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Upload Image</label>
                                        <%--<input type="text" class="form-control numeric text-right" maxlength="3" tabindex="42" runat="server" id="Text1" placeholder="Enter DSR Allow Days">--%>
                                        <asp:FileUpload ID="FileUpload1" runat="server" onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                            ControlToValidate="FileUpload1" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                        </asp:RegularExpressionValidator>

                                    </div>
                                </div>

                                <div class="col-md-4 col-sm-4 col-xs-12">

                                    <div class="form-group">
                                        <br />
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <label for="exampleInputEmail1">Field App:</label>
                                            <input id="chkmobAccess" type="checkbox" runat="server" />
                                        </div>
                                        <%--<div id="dvchk" runat="server" visible="false">--%>
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <label for="exampleInputEmail1">DMT App:</label>
                                            <input id="chkDMT" type="checkbox" runat="server" />
                                        </div>
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <label id="lblmanager" visible="false" runat="server" for="exampleInputEmail1">Manager App:</label>
                                            <input id="chkmanager" visible="false" type="checkbox" runat="server" />
                                        </div>
                                        <%-- </div> --%>
                                    </div>
                                </div>

                                <div class="col-md-4 col-sm-4 col-xs-12">

                                    <div class="form-group">
                                        <br />
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <label for="exampleInputEmail1">Record Log:</label>
                                            <input id="chklog" type="checkbox" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                    <span id="divblock" runat="server" class="hidden">
                                        <label for="exampleInputEmail1">Blocked Reason:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="100" id="BlockReason" placeholder="Enter Block Reason" autocomplete="off"></span>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <img id="imgpreview" height="150" width="150" src="" style="border-width: 0px; display: none;" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="clearfix"></div>
                            <div class="row">
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <a class="hiden">Tracker Details  <i class="fa fa-minus-circle" aria-hidden="true"></i></a>
                                    <a class="shown">Tracker Details  <i class="fa fa-plus-circle" aria-hidden="true"></i></a>
                                </div>
                            </div>

                            <div class="row" id="assgn" runat="server">
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <%--<button id="imgclose" onclick="ShowModalPopup();" />--%>
                                    <%--<a class="ShowModal">Assign Area to Sales Person  <i class="fa fa-caret-right" aria-hidden="true"></i></a>--%>
                                    <%--<a class="shown">Tracker Details  <i class="fa fa-plus-circle" aria-hidden="true"></i></a>--%>
                                </div>
                            </div>

                            <%--  <div class="form-group formlay">
                                <label for="exampleInputEmail1">Upload Image</label>--%>
                            <%--<input type="text" class="form-control numeric text-right" maxlength="3" tabindex="42" runat="server" id="Text1" placeholder="Enter DSR Allow Days">--%>
                            <%--  <asp:FileUpload ID="FileUpload1" runat="server" TabIndex="44" onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                    ControlToValidate="FileUpload1" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                </asp:RegularExpressionValidator>
                                <img id="imgpreview" height="150" width="150" class="pull-right" src="" style="border-width: 0px; display: none;" runat="server" />
                            </div>--%>
                            <%--<div class="form-group formlay">--%>
                            <%--       <label for="exampleInputEmail1">Active:</label>
                                <input id="chkIsActive" type="checkbox" runat="server" checked="checked" tabindex="46" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <label for="exampleInputEmail1">Allow to change home city:</label>
                                <input id="chkhomecity" type="checkbox" runat="server" tabindex="50" />&nbsp;&nbsp;&nbsp;&nbsp;--%>
                            <%--<label for="exampleInputEmail1">Field App:</label>
                                        <input id="chkmobAccess" type="checkbox" runat="server" tabindex="52" />&nbsp;&nbsp;&nbsp;&nbsp; 
                                        <div id="dvchk" runat="server" visible="false">
                                        <label for="exampleInputEmail1">Manager App:</label>
                                        <input id="chkmanager" type="checkbox" runat="server" tabindex="53" />&nbsp;&nbsp;&nbsp;&nbsp; 
                                        </div>       
                                        <label for="exampleInputEmail1">DMT App:</label>
                                        <input id="chkDMT" type="checkbox" runat="server" tabindex="54" />&nbsp;&nbsp;&nbsp;&nbsp;--%>
                            <%--<span id="divblock" runat="server" class="hidden">
                                    <label for="exampleInputEmail1">Blocked Reason:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input type="text" runat="server" class="form-control" maxlength="100" id="BlockReason" placeholder="Enter Block Reason" tabindex="48"></span>--%>
                            <%--</div>--%>
                            <div class="clearfix"></div>
                            <%--    <div class="form-group formlay">
                                <label for="exampleInputEmail1">Field App:</label>
                                <input id="chkmobAccess" type="checkbox" runat="server" tabindex="52" />&nbsp;&nbsp;&nbsp;&nbsp; --%>
                            <%--<div id="dvchk" runat="server" visible="false">--%>
                            <%--  <label id="lblmanager" visible="false" runat="server" for="exampleInputEmail1">Manager App:</label>
                                <input id="chkmanager" visible="false" type="checkbox" runat="server" tabindex="53" />&nbsp;&nbsp;&nbsp;&nbsp; --%>
                            <%-- </div> --%>
                            <%--  <label for="exampleInputEmail1">DMT App:</label>
                                <input id="chkDMT" type="checkbox" runat="server" tabindex="54" />&nbsp;&nbsp;&nbsp;&nbsp;                                                                   
                                      
                            </div>--%>

                            <%-- <div class="form-group formlay">
                            </div>--%>
                        </div>
                        <div class="box-footer">
                            <asp:HiddenField ID="HiddenarIds" runat="server" />
                            <asp:HiddenField ID="HiddenSMID" runat="server" />
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
                                <asp:Button type="button" ID="btnExport" OnClick="btnExport_Click" runat="server" Text="Export" Visible="true" class="btn btn-primary" />
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
                                              <th>Action</th>
                                                <th>Sales Person Name</th>
                                                <th>Sales Person Type</th>
                                                <th>Reporting Person</th>
                                                <th>Department</th>
                                                <th>Designation</th>
                                                 <th>HeadQuater</th>
                                                <th>Grade</th>
                                                <th>Device No</th>
                                                <th>Mobile No</th>
                                                <th>State</th>
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
                                    <tr>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("SMId") %>' />
                                        <td style="text-align: center;"><a onclick="DoNav('<%#Eval("SMId") %>');" class="tdedit"><i class="fa fa-pencil tdeditic" aria-hidden="true"></i></a>&nbsp;&nbsp;<img src="img/2.png" onclick="DoMod('<%#Eval("SMId") %>');" class="ShowModal tdedit" data-toggle="tooltip" title="Assign Area" style="height: 15px;"><%--<i class="fa fa-area-chart" aria-hidden="true"></i>--%></img></td>
                                        <td><%#Eval("SMName") %></td>
                                        <td><%#Eval("SalesRepType") %></td>
                                        <td><%#Eval("Parent") %></td>
                                        <td><%#Eval("Depname") %></td>
                                        <td><%#Eval("DesName") %></td>
                                        <td><%#Eval("HeadquarterName") %></td>
                                        <td><%#Eval("grade") %></td>
                                        <td><%#Eval("DeviceNo") %></td>
                                        <td><%#Eval("Mobile") %></td>
                                        <td><%#Eval("StateName") %></td>
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
          <asp:Button ID="Modalshow" runat="server" Style="display: none;" />
        <ajaxToolkit:ModalPopupExtender runat="server" ID="mpePop" TargetControlID="ModalShow"
            PopupControlID="pnlItem" BackgroundCssClass="Background" DropShadow="true" X="40" Y="30">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="pnlItem" runat="server" Style="display: none; background-color: White; padding: 1%; box-shadow: rgb(0 0 0) 5px 5px 5px; border: 2px solid rgb(185, 183, 183);" Height="640px">
            <div class="popupDiv row">
                <div class="box-header with-border">
                    <div class="col-md-6 col-sm-6 col-xs-12 headdiv">

                        <asp:Label ID="lblPerson" Font-Bold="true" runat="server" Text="Salesman Area"></asp:Label>
                        <b>
                            <input runat="server" type="text" class="form-control" maxlength="100" id="lblPersonName" placeholder="Enter Employee Name" autocomplete="off" style="float: right; width: 84%; border: none; margin-top: -6px;"></b>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-12 headdiv">
                        <img id="imgclose" style="float: right; height: 15px;" src="img/cross.jpg" onclick="HideModalPopup();" />
                    </div>
                </div>

                <div class="box-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <div class="row">

                                <div class="col-md-4 col-sm-4 col-xs-12" id="div2" runat="server" style="display: block;">

                                    <label for="exampleInputEmail1">State:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:ListBox ID="lstState" runat="server" onChange="BindCitySearch();" SelectionMode="Multiple"></asp:ListBox>
                                    <asp:HiddenField ID="HiddenState" runat="server" />
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">City:</label>
                                        <asp:ListBox ID="lstCity" runat="server" onChange="BindAreaSearch();" SelectionMode="Multiple"></asp:ListBox>
                                        <asp:HiddenField ID="HiddenCty" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Area:</label>
                                        <asp:ListBox ID="lstArea" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        <asp:HiddenField ID="HiddenArea" runat="server" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <label for="exampleInputEmail1" style="display: block; visibility: hidden">GoBtn</label>
                                <input type="button" runat="server" onclick="btnSubmitfunc();" class="btn btn-primary" value="Go" />
                                <%-- <asp:Button ID="btnGo" class="btn btn-primary" runat="server" Style="padding: 3px 14px;" Text="GO" OnClick="btnSubmitfunc();" />--%>
                            </div>

                        </div>
                        <div class="clearfix"></div>
                        <div id="rptDiv" runat="server">
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="rptchk" runat="server">
                                    <HeaderTemplate>
                                        <table id="example2" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <th style="text-align: left; width: 1% !important">
                                                        <asp:CheckBox ID="ckViewHead" runat="server" Text="" onclick="SelectAllByRow(this, 2)" />CheckAll</th>
                                                    <th>State</th>
                                                    <th>District</th>
                                                    <th>City</th>
                                                    <th>Area</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%--<tr>--%>

                                        <%--  <asp:HiddenField ID="areaIdHiddenField" runat="server" Value='<%#Eval("areaId") %>' />
                                                <asp:Label ID="lblHF" runat="server" Text='<%#Eval("areaId") %>' Visible="false"></asp:Label>--%>
                                        <%--<td style="text-align: center;">
                                                    <asp:CheckBox ID="chkItem" runat="server" />
                                                </td>
                                                <td><%#Eval("stateName") %></td>
                                                <td><%#Eval("districtName") %></td>
                                                <td><%#Eval("cityName") %></td>
                                                <td><%#Eval("areaName") %></td>--%>

                                        <%--</tr>--%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>

                                </asp:Repeater>
                                <%--  <div class="box-footer">--%>
                                <%--<input type="button" value="Get Selected" onclick="GetSelected()" />--%>
                                <asp:Button ID="btngrdsve" runat="server" CssClass="btn btn-primary" Text="Save" OnClientClick="return GetSelected();"
                                    OnClick="btngrdsve_Click" />
                                <%--<asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btncancel_Click" />--%>
                                <%--</div>--%>
                            </div>

                        </div>

                    </div>
                </div>
            </div>
        </asp:Panel>

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

        //$('#example2').dataTable({
        //    "paging": false
        //});

        function SelectAllByRow(ChK, cellno) {
            var gv = document.getElementById('example2');
            for (var i = 1; i <= gv.rows.length - 1; i++) {
                var len = gv.rows[i].getElementsByTagName("input").length;
                if (gv.rows[i].getElementsByTagName("input")[0].type == 'checkbox') {
                    gv.rows[i].getElementsByTagName("input")[0].checked = ChK.checked
                }
            }
        }
    </script>
</asp:Content>
