<%@ Page Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ActivityTemplate.aspx.cs" Inherits="AstralFFMS.ActivityTemplate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js"></script>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            if ($('#<%=hidsave.ClientID%>').val() == "true") {
                document.getElementById("btnsave").disabled = false;
            }
            else {
                document.getElementById("btnsave").disabled = true;
            }


            $("#btnCancel").click(function () {
                $("#ContentPlaceHolder1_hdnHeader").val("");
            });
            
        });

        var fieldoldname = "";
        var rowindex = -1;
        $(function () {
            var date = new Date();
            date.setDate(date.getDate());
            $(".datepicker").datepicker({ startDate: date, format: 'dd/M/yyyy', autoclose: true });
            $("#FromDate").datepicker('setDate', date);
            $("#ToDate").datepicker('setDate', date);

        });

       
        function validateNUM(x)
        {          
            if (x==0||x>3)
            {
                document.getElementById("txtNumImg").value = "";
                errormessage("can not enter less then 1 and more than 3.")
            }
        }
        function CreateOptionsVal() {
            //  alert("1");
            $("#dynDivDDL").empty();
            var type = $("#dynftype option:selected").val();
            if (type == "Dropdown" || type == "Checkbox" || type == "RadioButtton") {
                var li = $("<div  id='ddoptioncontainer'>");
                li.html(CreateControlValues(type));
                $("#dynDivDDL").append(li);
            }
         if (type == "Gallery") {
                $("#DivNumImg").show();
         }
         else {
             $("#DivNumImg").hide();
         }

        }
        function CreateControlValues(ControlName) {
            //return ' <div class="col-md-4 col-sm-4 col-xs-12"><div class="form-group" id="ele0"><label>Add ' + ControlName + '  Values </label><br/> <input type="text" maxlength="20" id="txtaddval1" class="form-control" style="width: 92%;float:left;"/>&nbsp;&nbsp; <a id="a1" onclick="addOption(1)"><label class="form-group"><i class="fa fa-plus"></i></label></a></div></div>'
            return '<div class="form-group" id="ele0"><label>Add ' + ControlName + '  Values </label> </div> <div class="form-group" id="ele1"><div class="row"><div class="col-md-4 col-sm-4 col-xs-12"><input type="text" maxlength="20" id="txtaddval1" class="form-control"/></div><div class="col-md-2 col-sm-2 col-xs-12"> <a id="a1" onclick="addOption(1)"><label class="col-md-1 col-sm-1 col-xs-12 control-label"><i class="fa fa-plus"></i></label></a></div></div>'
        }
        function addOption(ind) {
            var id = $("#ddoptioncontainer > div").length;
            var html = '<div class="form-group" id="ele' + id + '"> <div class="row"><div class="col-md-4 col-sm-4 col-xs-12"><input type="text" maxlength="20" id="txtaddval' + id + '" class="form-control"/></div><div class="col-md-2 col-sm-2 col-xs-12"><a  onclick="CloseOption(' + id + ')"><label class="control-label"><i class="fa fa-minus-circle"></i></label></a> &nbsp;&nbsp;<a id="a' + id + '" onclick="addOption(' + id + ')"><label class="control-label"><i class="fa fa-plus"></i></label></a></div></div>';
            $("#a" + ind).hide();
            $("#ele" + ind).after(html);
        }
        function CloseOption(N) {
            $("#ele" + N).remove();
            debugger;
            N = N - 1;
            $("#a" + N).removeAttr("style");
        }


        function compare() {
            debugger;
            var startDate = new Date($("#FromDate").val());
            var endDate = new Date($("#ToDate").val());

            if (startDate.getTime() > endDate.getTime()) {
                $("#ToDate").val('');
                errormessage("From Date Should be less than To Date");
                return false;

            }
        }
        function onback() {
            clrcontrols();
            $("#mainDiv").show();
            $("#rptmain").hide();
           
        }
        function FillData() {
            debugger
            $.ajax({
                type: "POST",
                //txtfiltersortingno
                <%--url: '<%= ResolveUrl("~/CRMDBService.asmx/GetCustomFieldsForActivity") %>',--%>
                    url: '<%= ResolveUrl("~/CRMDBService.asmx/GetCustomFieldsForActivityNew") %>',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    console.log(jsdata1);
                    var Html = "";
                    $("#tblfinddata tbody").empty()
                    $.each(jsdata1, function (key1, value1) {
                        debugger
                        Html += "<tr onclick='GetCustomfieldLineData(" + value1.Header_Id + ");'><td>" + value1.Title + "</td><td>" + value1.Fromdate + "</td><td>" + value1.Todate + "</td><td>" + value1.active + "</td></tr>";
                        //<td>" + value1.Type + "</td>
                    });
                    $("#tblfinddata tbody").append(Html);

                }
            });

            $("#mainDiv").hide();
            $("#rptmain").show();
        }


        function GetCustomfieldLineData(HeaderId) {


            if ($('#<%=hidupdate.ClientID%>').val() == "true") {
                document.getElementById("btnsave").disabled = false;
            }
            else {
                document.getElementById("btnsave").disabled = true;
            }

            //alert(HeaderId);
            debugger
            $("#ContentPlaceHolder1_hdnHeader").val(HeaderId);
            $("#selectdisplaytype").prop("disabled", true)
            $("#FromDate").prop("disabled", true);
            $("#ToDate").prop("disabled", false);
            $.ajax({
                type: "POST",
                <%--url: '<%= ResolveUrl("~/CRMDBService.asmx/GetCustomfieldLineData") %>',--%>
                    url: '<%= ResolveUrl("~/CRMDBService.asmx/GetCustomfieldLineDataNew") %>',
                    contentType: "application/json; charset=utf-8",
                data: '{HeaderId: "' + HeaderId + '"}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    console.log(jsdata1);
                    $("#FromDate").val(jsdata1[0].Fromdate);
                    $("#ToDate").val(jsdata1[0].Todate);
                    var Html = "";
                    $("#tbldata tbody").empty();
                    $.each(jsdata1, function (key1, value1) {
                        debugger;//NoImgsForGallery
                        Html += "<tr><td>" + value1.AttributeField + "</td><td>" + value1.AttributFieldType + "</td><td>" + value1.sort + "</td><td>" + value1.AttributeData.replace(',', ',') + "</td><td>" + value1.Active + "</td><td>" + value1.required + "</td><td>" + value1.NoImgsForGallery + "</td><td><a onclick='editrow(this);'><i class='fa fa-fw fa-pencil'></i></a></td><td style='display:none'>" + value1.Custom_Id + "</td></tr>";
                        $("#dyntitle").val(value1.Title);
                        $("#selectdisplaytype").val(value1.Type);

                        
                        if (value1.tempActive == true) {
                            $('#chkIsTempActive').prop('checked', true);
                        }
                        else {
                            $('#chkIsTempActive').prop('checked', false);
                        }
                    });
                    $("#tbldata tbody").append(Html);
                    $('#showdata').show();

                }
            });
            $("#mainDiv").show();
            $("#rptmain").hide();
        }
        function Addtotable() {
            if ($("#dyntitle").val() == "") {
                errormessage("Please Enter Title");
                return false;
            }
            if ($("#FromDate").val() == "") {
                errormessage("Please Enter From Date");
                return false;
            }
            if ($("#ToDate").val() == "") {
                errormessage("Please Enter To Date");
                return false;
            }
            compare();
            if ($("#selectdisplaytype").val() == "") {
                errormessage("Please select display type");
                return false;
            }
            if ($('#dynfname').val() == "") {
                errormessage("Please Enter Field Name");
                return false;
            }
            if ($('#dynfname').val() != "") {
                if (rowindex == -1) {
                    var tr = $("#tbldata td:contains(" + $('#dynfname').val().toUpperCase() + ")").closest("tr").find("td");
                    if (tr.length != 0) {

                        errormessage("Duplicate field name is not allowed");
                        return false;
                    }
                }
            }
            if ($('#dynsort').val() == "") {
                errormessage("Please Enter Sorting Order");
                return false;
            }

            if ($("#dynftype option:selected").text() == "Dropdown" || $("#dynftype option:selected").text() == "Checkbox") {
                if ($("#txtaddval1").val() == "") {
                    errormessage("Please Enter one value for " + $("#dynftype option:selected").text() + "");
                    return false;
                }
            }
            if ($("#dynftype option:selected").text() == "Gallery" ) {
                if ($("#txtNumImg").val() == "" || $("#txtNumImg").val() == "0" || $("#txtNumImg").val() > "3") {
                    errormessage("Please Enter one value between 1 to 3 for " + $("#dynftype option:selected").text() + "");
                    return false;
                }
            }
            var data = "";
            var active = "", Required="";
            debugger;
            if ($('#chkIsActive').is(":checked")) {
                active = "true";
            }
            else {
                active = "false";

            }
            if ($('#chkIsRequired').is(":checked")) {
                Required = "true";
            }
            else {
                Required = "false";

            }
            //chkIsRequired
            $("#dynfname").prop("disabled", false);
            $("#dynftype").prop("disabled", false);
            $("#dynsort").prop("disabled", false);
            $("#FromDate").prop("disabled", false);
            $("#ToDate").prop("disabled", false);
            $("#selectdisplaytype").prop("disabled", false)
            $("#ddoptioncontainer :text").each(function () {
                data += $(this).val() + ",";
            });
            data = data.substr(0, data.length - 1);
            var noImg = $("#txtNumImg").val();
            if (rowindex != -1) {
                $("#tbldata tbody tr:eq(" + rowindex + ")").children("td:nth-child(1)").html($('#dynfname').val().toUpperCase());
                $("#tbldata tbody tr:eq(" + rowindex + ")").children("td:nth-child(2)").html($('#dynftype').val());
                $("#tbldata tbody tr:eq(" + rowindex + ")").children("td:nth-child(3)").html($('#dynsort').val());
                $("#tbldata tbody tr:eq(" + rowindex + ")").children("td:nth-child(4)").html(data);
                $("#tbldata tbody tr:eq(" + rowindex + ")").children("td:nth-child(5)").html(active);
                $("#tbldata tbody tr:eq(" + rowindex + ")").children("td:nth-child(6)").html(Required);
                $("#tbldata tbody tr:eq(" + rowindex + ")").children("td:nth-child(7)").html(noImg);
                
                $("#tbldata tbody tr:eq(" + rowindex + ")").children("td:nth-child(9)").html($('#hidcustomid').val());
               


            }
            else {
                var newRowContent = "<tr><td>" + $('#dynfname').val().toUpperCase() + "</td><td>" + $('#dynftype').val() + "</td><td>" + $('#dynsort').val() + "</td><td>" + data + "</td><td>" + active + "</td><td>" + Required + "</td><td>" + noImg + "</td><td><a onclick='editrow(this);'><i class='fa fa-fw fa-pencil'></i></a><a onclick='deleterow(this);'><i class='fa fa-fw fa-trash'></i></a></td><td style='display:none'>" + $('#hidcustomid').val() + "</td></tr>";
                $("#tbldata tbody").append(newRowContent);
                //$("#btnsave").show();
                //$("#btnCancel").show();

            }
      
            $("#dynDivDDL").empty();
            $('#dynfname').val('');
            $('#dynftype').val('Single Line Text');
            $('#dynsort').val('');
            $('#hidcustomid').val('');
            $('#chkIsActive').prop('checked', true);
            $('#chkIsRequired').prop('checked', true);
            $('#showdata').show();
            $("#txtNumImg").val('');
            rowindex = -1;
            fieldoldname = "";


        }
        function clrcontrols() {
            var date = new Date();
            date.setDate(date.getDate());
            $("#FromDate").datepicker('setDate', date);
            $("#ToDate").datepicker('setDate', date);
            $("#dyntitle").val('');
            $('#dynfname').val('');
            $('#dynftype').val('Single Line Text');
            $('#dynsort').val('');
            $('#hidcustomid').val('');
            $('#chkIsActive').prop('checked', true);
            $('#chkIsRequired').prop('checked', true);
            $("#selectdisplaytype").prop("disabled", false)
            $("#FromDate").prop("disabled", false);
            $("#ToDate").prop("disabled", false);
            $("#dynDivDDL").html('');
            $("#tbldata tbody").empty();
            $('#showdata').hide();
            $("#dynfname").prop("disabled", false);
            $("#dynftype").prop("disabled", false);
            $("#dynsort").prop("disabled", false);
            $("#txtNumImg").val('');
            //$("#btnsave").hide();
            //$("#btnCancel").hide();


        }
        function deleterow(rowdata) {
            $(rowdata).parent().parent().remove();
        }
        function editrow(rowdata) {
            debugger;
            $('#dynfname').val($(rowdata).parent().parent().children("td:nth-child(1)").html());
            $('#dynftype').val($(rowdata).parent().parent().children("td:nth-child(2)").html());
            $('#dynsort').val($(rowdata).parent().parent().children("td:nth-child(3)").html());
            $('#txtNumImg').val($(rowdata).parent().parent().children("td:nth-child(7)").html());
            $('#hidcustomid').val($(rowdata).parent().parent().children("td:nth-child(9)").html());
            rowindex = $(rowdata).parent().parent().index();
            var disable = "";
            if ($('#hidcustomid').val() != "") {
               
                $("#dynfname").prop("disabled", false);
                $("#dynftype").prop("disabled", true);
                $("#dynsort").prop("disabled", true);
                $("#FromDate").prop("disabled", true);
                $("#ToDate").prop("disabled", false);
                $("#selectdisplaytype").prop("disabled", true)
                disable = "disabled";

            }
            else {
                $("#dynfname").prop("disabled", false);
                $("#dynftype").prop("disabled", false);
                $("#dynsort").prop("disabled", false);
                $("#FromDate").prop("disabled", false);
                $("#ToDate").prop("disabled", false);
                $("#selectdisplaytype").prop("disabled", false)
                disable = "enabled";
            }
            fieldoldname = $('#dynfname').val();
            if ($(rowdata).parent().parent().children("td:nth-child(5)").html() == "true") {
                $('#chkIsActive').prop('checked', true);
            }
            else {
                $('#chkIsActive').prop('checked', false);
            }
            $("#dynDivDDL").empty();
            var type = $("#dynftype option:selected").val();
            if (type == "Gallery" ) {
                $("#DivNumImg").show();
            }
            if (type == "Dropdown" || type == "Checkbox") {
                //var data = $(rowdata).parent().parent().children("td:nth-child(4)").html().split(',');
                var data = $(rowdata).parent().parent().children("td:nth-child(4)").html().split('*');
                var html = "<div class='col-md-8 col-sm-4' id='ddoptioncontainer'><div class='form-group' id='ele0'><label>Add " + type + "  Values </label> </div>";
                for (var j = 0; j < data.length; j++) {
                    var id = $("#ddoptioncontainer > div").length;
                    if (data.length == 1) {

                        html += "<div class='form-group' id='ele" + id + "'><div class='row'><div class='col-lg-9'><input type='text' maxlength='20' id='txtaddval" + id + "' class='form-control' value=" + data[j] + " " + disable + "></div>";
                        html += "<div class='col-lg-3'> <a id='a1' onclick='addOption(" + id + ")' ><label class='col-dm-1 control-label'><i class='fa fa-plus'></i></label></a></div></div></div>";
                    }
                    else {
                        if (j == 0) {
                            html += "<div class='form-group' id='ele" + id + "'><div class='row'><div class='col-lg-9'><input type='text' maxlength='20' id='txtaddval" + id + "' class='form-control' value=" + data[j] + " " + disable + "></div>";
                            html += "<div class='col-lg-3'> <a id='a1' onclick='addOption(" + id + ")' style='display:none;' ><label class='col-dm-1 control-label'><i class='fa fa-plus'></i></label></a></div></div></div>";

                        }
                        else {
                            html += "<div class='form-group' id='ele" + id + "'> <div class='row'><div class='col-lg-9'><input type='text' maxlength='20' id='txtaddval" + id + "' class='form-control' value=" + data[j] + " " + disable + "></div>";
                            html += "<div class='col-lg-3'><a onclick='CloseOption(" + id + ")'><label class='col-dm-1 control-label'><i class='fa fa-minus-circle'></i></label></a> &nbsp;&nbsp;<a id='a2' onclick='addOption(" + id + ")'><label class='col-dm-1 control-label'><i class='fa fa-plus'></i></label></a></div></div></div>";
                        }
                    }



                }

                html += "</div>";
                $("#dynDivDDL").html(html);
            }
        }
        function savenew() {
            debugger
            var Fieldname = "";
            var Fieldtype = "";
            var sorting = "";
            var option = "";
            var Active = "",Required="";
            var Ids = "";
            var noImg = "";
            $("#tbldata tbody tr").each(function () {
                Fieldname += $(this).children("td:nth-child(1)").html() + ",";
                Fieldtype += $(this).children("td:nth-child(2)").html() + ",";
                sorting += $(this).children("td:nth-child(3)").html() + ",";
                option += $(this).children("td:nth-child(4)").html() + "*";
               // option += $(this).children("td:nth-child(4)").html().replace(",", "*#") + ",";
                Active += $(this).children("td:nth-child(5)").html() + ",";
                Required += $(this).children("td:nth-child(6)").html() + ",";
                noImg += $(this).children("td:nth-child(7)").html() + ",";
                Ids += $(this).children("td:nth-child(9)").html() + ",";
            });

            Fieldname = Fieldname.substr(0, Fieldname.length - 1);
            Fieldtype = Fieldtype.substr(0, Fieldtype.length - 1);
            sorting = sorting.substr(0, sorting.length - 1);
            option = option.substr(0, option.length - 1);
            Active = Active.substr(0, Active.length - 1);
            Required = Required.substr(0, Required.length - 1);
            noImg = noImg.substr(0, noImg.length - 1);
            Ids = Ids.substr(0, Ids.length - 1);
            var data = "";
            var active = "";
            debugger;
            if ($('#chkIsActive').is(":checked")) {
                active = true;
            }
            else {
                active = false;

            }
            
            if ($('#chkIsTempActive').is(":checked")) {
                active = true;
            }
            else {
                active = false;

            }

            $("#ddoptioncontainer :text").each(function () {
                if (data == "")
                    data = $(this).val();
                else if (data != "")
                    data +=  $(this).val() + ",";
            });
            data = data.substr(0, data.length - 2);
            var hid = $("#ContentPlaceHolder1_hdnHeader").val();
            debugger
            $.ajax({
                type: "POST",
                //txtfiltersortingno
                <%-- url: '<%= ResolveUrl("~/CRMDBService.asmx/SaveCustomFieldsForActivity") %>',--%>
                url: '<%= ResolveUrl("~/CRMDBService.asmx/SaveCustomFieldsForActivityNew") %>',
                contentType: "application/json; charset=utf-8",
                data: '{Table: "Activity",Title: "' + $("#dyntitle").val() + '",Fromdate: "' + $("#FromDate").val() + '",ToDate:"' + $("#ToDate").val() + '",For: "",Fieldname: "' + Fieldname + '",Fieldtype: "' + Fieldtype + '",Data: "' + option.toString() + '",Sorting: "' + sorting + '",filterctrlno: "",parametername: "",Active: "' + Active + '",TempActive: "' + active + '",Required: "' + Required + '",CustomIds:"' + Ids + '",HeadId:"' + hid + '",NoImgsForGallery:"' + noImg + '" }',
                dataType: "json",
                success: function (data) {
                    var Message = data.d;                   
                    if (Message == "-1") {                       
                        //errormessage("Duplicate Field Name");
                        errormessage("Duplicate Template.");
                    }
                    else if (Message == "-3") {                      
                        errormessage("First Click to Add Buttion");
                    }
                    else {
                        Successmessage("Record Saved Successfully");
                        //$("#dynDivDDL").empty();
                        //$('#dynfname').val('');
                        //$('#dynftype').val('Single Line Text');
                        //$('#dynsort').val('');
                        //$('#showdata').hide();
                        clrcontrols();
                        $("#ContentPlaceHolder1_hdnHeader").val("");
                    }
                }
          });

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
    <style type="text/css">
        #btnAdd{
                margin-top: 23px;
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
        <div class="box-body" id="mainDiv">

            <div class="row">
                <div class="col-md-12">
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Activity Template Master</h3>
                            <div style="float: right">
                                <input type="button" class="btn btn-primary" value="Find" onclick="FillData();" />

                                <%--  <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" runat="server" />--%>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <div class="box-body">                            
                                <div class="row">                                  
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Title:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <input id="dyntitle" class="form-control" placeholder="Enter Title" name="dynfname" type="text" maxlength="20">
                                                <input id="hidcustomid" type="hidden" />
                                            </div>

                                        </div>
                                      <div class="col-md-3 col-sm-3 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">From Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <input type='text' id="FromDate" class="form-control datepicker" maxlength="20" readonly="readonly" />

                                            </div>
                                        </div>
                                   <div class="col-md-3 col-sm-3 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">To Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <input type='text' id="ToDate" class="form-control datepicker" maxlength="20" readonly="readonly" onchange="compare();" />

                                            </div>
                                        </div>
                                   
                                      <%--  <div class="col-lg-6">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">For:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <select id="selectdisplaytype" name="selectdisplaytype" class="form-control">
                                                    <option value="Distributor">Distributor</option>
                                                    <option value="Retailer">Retailer</option>
                                                </select>
                                            </div>

                                        </div>--%>
                                   
                                </div>
                                
                                <div class="row">
                                    
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Field Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <textarea id="dynfname" class="form-control" placeholder="Enter Field Name" name="dynfname" style="height: 50px;resize:none">
                                                </textarea>
                                            </div>
                                        </div>
                                      <div class="col-md-3 col-sm-3 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Sorting Order:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <input id="dynsort" class="form-control numeric text-left" placeholder="Enter Sorting Order" name="dynsort" type="number" min="0" max="9" maxlength="5">
                                               
                                            </div>

                                        </div>
                                        <div class="col-md-3 col-sm-3 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Field Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <select id="dynftype" class="form-control  select2" name="dynftype" onchange="CreateOptionsVal();">
                                                    <option>Single Line Text</option>
                                                    <option>Multiple Line Text</option>
                                                    <option>Number</option>
                                                    <option>Dropdown</option>
                                                    <option>Date</option>
                                                    <option>Checkbox</option>
                                                     <option>RadioButtton</option>
                                                    <option>Camera</option>
                                                    <option>Gallery</option>
                                                </select>

                                            </div>
                                        </div>
                                    <div class="col-md-2 col-sm-3 col-xs-12">
                                            <div class="form-group" id="DivNumImg" style="display:none;">
                                                <label for="exampleInputEmail1">No Of Images Allow:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <input id="txtNumImg" class="form-control numeric text-left"  name="txtNumImg" type="text" oninput="validateNUM(this.value);" placeholder="Enter No of Images" maxlength="1">
                                                  
                                            </div>

                                        </div>
                                   
                                </div>
                                <div class="row">
                                     
                                   <div class="col-md-1 col-sm-1 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Active:</label>
                                                <input id="chkIsActive" type="checkbox" checked="checked" class="checkbox" tabindex="48">
                                            </div>
                                        </div>
                                   
                                      <div class="col-md-1 col-sm-1 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Required:</label>
                                                <input id="chkIsRequired" type="checkbox" checked="checked" class="checkbox" tabindex="48">
                                            </div>
                                        </div> 
                                       <div class="col-md-2 col-sm-2 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Active Template:</label>
                                                <input id="chkIsTempActive" type="checkbox" checked="checked" class="checkbox" tabindex="48">
                                            </div>
                                        </div>
                                </div>
                             <div class="row">        
                                  <div class="col-md-12 col-sm-12 col-xs-12">                            
                                        <div class="form-group">
                                            <div id="dynDivDDL"></div>

                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                   
                                     <div class="col-md-2 col-sm-2 col-xs-12">
                                            <div class="form-group">
                                               <input type="button" class="btn btn-primary" value="Add Field" id="btnAdd" onclick="Addtotable();" />
                                            </div>
                                        </div>
                                </div>
                               
                            
                        </div>
                        <div id="showdata" class="box-body" style="display: none;">

                            <table id="tbldata" class="table table-bordered table-striped">
                                <thead>
                                    <tr>
                                        <th style="text-align: left; width: 10%">Field Name</th>
                                        <th style="text-align: left; width: 20%">Field Type</th>
                                        <th style="text-align: left; width: 20%">Sorting order</th>
                                        <th style="text-align: left; width: 10%">Option</th>
                                        <th style="text-align: left; width: 10%">Active</th>
                                        <th style="text-align: left; width: 10%">Required</th>
                                        <th style="text-align: left; width: 10%">NO Of Images</th>
                                        <th style="text-align: left; width: 10%">Action</th>
                                        <th style="text-align: left; width: 10%; display: none">Id</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>


                        </div>


                        <div class="box-footer">
                            <asp:Button Text="Check" ID="btnCheck" runat="server" OnClick="btnCheck_Click" Visible="false"/>
                                 <input type="hidden" id="hidsave" runat="server" />
                                <input type="hidden" id="hidupdate" runat="server" />
                            <input type="button"  class="btn btn-primary" value="Save"  id="btnsave" onclick="savenew();" />
                            <input type="button" class="btn btn-primary" value="Cancel" id="btnCancel" onclick="clrcontrols();" />
                             <input type="hidden" id="hdnHeader" runat="server" />
                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="box-body" id="rptmain" style="display: none;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Activity List</h3>
                            <div style="float: right">
                                <input type="button" id="btnback" class="btn btn-primary" value="Back" onclick="onback();" />

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <table id="tblfinddata" class="table table-bordered table-striped">
                                <thead>
                                    <tr>
                                        <th>Title</th>
                                        <th>From Date</th>
                                        <th>To Date</th>
                                       <%-- <th>For</th>--%>
                                         <th>Active</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                        <!-- /.box-body -->
                    </div>
                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>

        </div>
    </section>
</asp:Content>
