<%@ Page Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ActivityMast.aspx.cs" Inherits="AstralFFMS.ActivityMast" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js"></script>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function isNumberKey(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode == 46) {
                return true;
            }
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        

</script>

    <script>
        //alert($("input:text").length);
        $(document).ready(function () {
            var distpartytype = "";
            var distid = GetParameterValues('DistID');
            var partyid = GetParameterValues('PartyID');
            if (distid != null)
                distpartytype = "Distributor";
            if (partyid != null)
                distpartytype = "Retailer";
            $.ajax({
                contentType: "application/json; charset=utf-8",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetActivityTransTitleList") %>',
                dataType: "json",
                type: "POST",
                data: '{ForType: "' + distpartytype + '"}',
                success: function (response) {

                    if (response.d.length > 0) {
                        var res = JSON.parse(response.d);
                        var items = [];
                        items.push("<option value=''>Select</option>");
                        for (var i = 0; i < res.length; i++) {
                            items.push("<option value=" + res[i].header_id + ">" + res[i].title + "</option>");
                        }
                        //$("#ddlTitle").html(items.join(' '));
                        $("#ddlTitle").html(items.join(' '));
                    }
                }
            });

            $("#ddlTitle").change(function () {
                //$("#ddlTitle").change(function () {
                GetData();
            });

            $("#btnFind").click(function () {
                $('#spinner').show();
                $("#mainDiv").hide();
                $("#ContentPlaceHolder1_dvFind").show();
                var str = GetParameterValues('VisitID');
                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    url: '<%= ResolveUrl("CRMDBService.asmx/GetActivityTransactionList") %>',
                    dataType: "json",
                    type: "POST",
                    data: '{VisId: "' + str + '"}',
                    success: function (response) {
                        var tr;
                        $("#tblfinddata tbody").empty();
                        if (response.d.length > 0) {
                            var res = JSON.parse(response.d);
                            $('#tblfinddata tbody > tr').remove();
                            for (i = 0; i < res.length; i++) {
                                tr = $('<tr onclick="DoNav(' + "'" + res[i].ForType + "'" + ',' + "'" + res[i].Fromdate + "'" + ',' + "'" + res[i].Todate + "'" + ',' + res[i].Activity_Id + ',' + res[i].HeaderId + ')" />');
                                //tr = $('<tr onclick="DoNav(' + res[i].VisId + ')" />');
                                tr.append("<td>" + res[i].Fromdate + "</td>");
                                tr.append("<td>" + res[i].Todate + "</td>");
                                tr.append("<td>" + res[i].ForType + "</td>");
                                tr.append("<td>" + res[i].Title + "</td>");
                                tr.append("<td style='display: none'>" + res[i].Activity_Id + "</td>");
                                tr.append("<td style='display: none'>" + res[i].VisId + "</td>");
                                $("#tblfinddata tbody").append(tr);

                            }
                        }
                        else if (response.d.length == 0) {
                            $('#tblfinddata thead > tr').remove();
                            $('#tblfinddata tbody > tr').remove();
                            tr = $('<tbody><tr class="odd"><td colspan="4" class="dataTables_empty" valign="top">No matching records found</td></tr></tbody>');
                            $("#tblfinddata tbody").append(tr);
                        }
                    }

                });
                $('#spinner').hide();
            });
        });


        function GetData() {

            $('#spinner').show();
            var distpartytype = "";
           
            var distid = GetParameterValues('DistID');
            var partyid = GetParameterValues('PartyID');
            if (distid != null)
                distpartytype = "Distributor";
            if (partyid != null)
                distpartytype = "Retailer";
            $("#ContentPlaceHolder1_hdnValue").val($("#ddlTitle").val());
            var str = GetParameterValues('VisitID');
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetCustomFieldsforactivityT") %>',
                contentType: "application/json; charset=utf-8",
                //data: '{AttrType: "' + distpartytype + '",HeaderId: "' + $("#ddlTitle").val() + '"}',
                data: '{AttrType: "' + distpartytype + '",HeaderId: "' + $("#ddlTitle").val() + '"}',
                //data: '{AttrType: "' + distpartytype + '",Fromdate: "' + $('#ContentPlaceHolder1_FromDate').val() + '",todate: "' + $('#ContentPlaceHolder1_ToDate').val() + '"}',
                dataType: "json",
                success: function (data) {

                    jsdata1 = JSON.parse(data.d);
                    var output = "<div class='box-body'>";
                    output += " <div class='row'>";
                    output += "<div >";
                    var Cdata = ""; var cbid = ""; var selid = "";
                    $.each(jsdata1, function (key1, value1) {

                        $('#ContentPlaceHolder1_lbltitle').text(value1.Title);
                        Cdata += value1.AttributeField + "^";
                        if (value1.AttributFieldType == "Number") {
                            output += "<div class='form-group'><label>" + value1.AttributeField + ": </label><div><input type='text' id='" + value1.AttributeField + "' value='0'  tabindex='10'  class='form-control numeric text-right customfieldgroup' maxlength='10' onkeypress='return isNumberKey(event)'/> </div></div>";
                        }
                        else if (value1.AttributFieldType == "Dropdown" || value1.AttributFieldType == "Checkbox" || value1.AttributFieldType == "Sql View" || value1.AttributFieldType == "Sql Procedure") {
                            //var arr = value1.AttributeData.split("*#");
                            var arr = value1.AttributeData.split(",");                          
                            if (value1.AttributFieldType == "Checkbox") {
                                cbid += "cb" + (value1.AttributeField).replace(/ +/g, "") + "@";
                                output += '<div class="form-group customfieldgroup" id="cb' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                for (var j = 0; j < arr.length; j++) {
                                    output += '<label><input type="checkbox" tabindex="10" id="' + arr[j] + '"  value="' + arr[j] + '"/>' + arr[j] + '</label>&nbsp;&nbsp;&nbsp;'
                                }
                                output += '</div>';
                            }
                            else if (value1.AttributFieldType == "Sql Procedure") {
                                {
                                    var dd = value1.AttributFieldType;
                                    output += '<div class="form-group" id="sel' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                    selid += "sel" + value1.AttributeField + "@";
                                    output += '<select class="form-control customfieldgroup"  id="' + (value1.AttributeField).replace(/ +/g, "") + '" name="' + value1.AttributeField + '" tabindex="10" >'
                                    output += '<option value="0" >Select</option>'
                                    output += '</select>'; output += '</div>';
                                }
                            }
                            else if (value1.AttributFieldType == "Sql View") {
                                var dd = value1.AttributFieldType;
                                output += '<div class="form-group" id="sel' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                selid += "sel" + value1.AttributeField + "@";
                                output += '<select class="form-control customfieldgroup" onchange="FillInDropBox(this);" id="' + (value1.AttributeField).replace(/ +/g, "") + '" name="' + value1.AttributeField + '" tabindex="10"  >'
                                output += '<option value="0" >Select</option>'
                                for (var j = 0; j < arr.length; j++) {
                                    output += '<option  >' + arr[j] + '</option>'
                                }
                                output += '</select>'; output += '</div>';
                            }
                            else {

                                var dd = value1.AttributFieldType;                              
                                output += '<div class="form-group" id="sel' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                selid += "sel" + value1.AttributeField + "@";                               
                                //output += '<select class="form-control customfieldgroup" id="' + (value1.AttributeField).replace(/ +/g, "") + '" name="' + value1.AttributeField + '" tabindex="10" >'
                                output += '<select class="form-control customfieldgroup" id="' + value1.AttributeField + '" name="' + value1.AttributeField + '" tabindex="10" >'
                                output += '<option value="0" >Select</option>'
                                for (var j = 0; j < arr.length; j++) {
                                    output += '<option  >' + arr[j] + '</option>'
                                }
                                output += '</select>'; output += '</div>';
                            }


                        }
                        else if (value1.AttributFieldType == "Date") {
                            output += "<div class='form-group'><label>" + value1.AttributeField + " </label><input type='text' id='" + value1.AttributeField + "'class='form-control customfieldgroup datepicker' maxlength='20' readonly='readonly' tabindex='10' /></div>";
                        }
                        else if (value1.AttributFieldType == "Multiple Line Text") {
                           
                            output += "<div class='form-group'><label>" + value1.AttributeField + " </label><textarea id='" + value1.AttributeField + "' value=''  class='form-control customfieldgroup' tabindex='10'></textarea> </div>";
                        }
                        else {                         
                         
                            output += "<div class='form-group'><label>" + value1.AttributeField + " </label><input type='text' id='" + value1.AttributeField + "' value='' class='form-control customfieldgroup' maxlength='20' tabindex='10'/> </div>";
                        }

                    });
                    output += "</div> <script type='text/javascript'>  $(function () {var date = new Date(); date.setDate(date.getDate()); $('.datepicker').datepicker({ startDate: date, format: 'dd/M/yyyy', autoclose: true });});";

                <%--    Cdata = Cdata.substr(0, Cdata.length - 1);
                    $('#<%=hidcustomfields.ClientID %>').val(Cdata);
                    cbid = cbid.substr(0, cbid.length - 1);
                    $('#<%=hidchkid.ClientID %>').val(cbid);--%>

                    $('#divCF').html(output);                   
                }
            });
            $('#spinner').hide();
            //$('#spinner').show();
            debugger
          
            $("#ContentPlaceHolder1_hdnValue").val($('#ddlTitle').val());
            //$("#ContentPlaceHolder1_hdnActivity").val(Activity_Id);
            $.ajax({
                contentType: "application/json; charset=utf-8",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetActivityTransactionFill") %>',
           dataType: "json",
           type: "POST",
           data: '{VisId: "' + str + '",HeaderId: "' + $('#ddlTitle').val() + '"}',
           success: function (data) {
               debugger
               var resdata = JSON.parse(data.d); var Cdata = ""; var cbid = "";
               for (i = 0; i < resdata.length; i++) {
                   Cdata += resdata[i].AttributeField + "^";
                   if (resdata[i].AttributFieldType == "Dropdown" || resdata[i].AttributFieldType == "Checkbox") {

                       if (resdata[i].AttributFieldType == "Dropdown") {
                           $('#' + resdata[i].AttributeField).val(resdata[i].AttributeValue);
                       }
                       else if (resdata[i].AttributFieldType == "Checkbox") {
                           cbid += "cb" + (resdata[i].AttributeField).replace(/ +/g, "") + "@";
                           if (resdata[i].AttributeValue != null) {
                               //var arr = resdata[i].AttributeValue.split(",");
                               var arr = resdata[i].AttributeValue.split("^");
                               for (var j = 0; j < arr.length; j++) {
                                   $('#' + arr[j]).attr('checked', true);
                               }
                           }
                       }
                   }

                   else {

                       $("#" + resdata[i].AttributeField).val(resdata[i].AttributeValue);
                       if (resdata[i].AttributFieldType == "Date") {
                           debugger
                           var valuedate = resdata[i].AttributeValue;
                           if (valuedate="01/01/1900 12:00:00 AM")
                               valuedate = "";
                           $("#" + resdata[i].AttributeField).val(valuedate);
                       }
                       else {
                           $("#" + resdata[i].AttributeField).val(resdata[i].AttributeValue);
                       }
                   }
               }

               Cdata = Cdata.substr(0, Cdata.length - 1);
               $('#ContentPlaceHolder1_hidcustomfields').val(Cdata);
               //alert(Cdata);
               cbid = cbid.substr(0, cbid.length - 1);
               $('#<%=hidchkid.ClientID %>').val(cbid);
           }

       });


       //$('#spinner').hide();
   }

   function DoNav(ForType, Fromdate, Todate, Activity_Id, HeaderId) {
       $('#spinner').show();
       document.getElementById("mainDiv").style.display = 'block';
       document.getElementById("ContentPlaceHolder1_dvFind").style.display = 'none';
       var str = GetParameterValues('VisitID');

       $.ajax({
           contentType: "application/json; charset=utf-8",
           url: '<%= ResolveUrl("CRMDBService.asmx/GetCustomFieldsforactivityT") %>',
                dataType: "json",
                type: "POST",
                data: '{AttrType: "' + ForType + '",HeaderId: "' + HeaderId + '"}',
                //data: '{VisId: "' + str + '"}',
                success: function (response) {
                    var res = JSON.parse(response.d);
                    $('#ddlTitle').val(HeaderId);
                    $("#ContentPlaceHolder1_hdnValue").val(HeaderId);
                    $("#ContentPlaceHolder1_hdnActivity").val(Activity_Id);
                    var output = "<div class='box-body'>";
                    output += " <div class='row'>";
                    output += "<div >";
                    var Cdata = ""; var chkid = ""; var ddllid = ""; var attvalu = "";
                    for (i = 0; i < res.length; i++) {
                        if (res[i].AttributFieldType == "Single Line Text") {

                            var attfield = res[i].AttributeField;
                            output += "<div class='form-group'><label>" + attfield + " </label><input type='text' id='" + attfield + "' value='" + attvalu + "' class='form-control customfieldgroup' maxlength='20' tabindex='10'/> </div>";
                        }
                        else if (res[i].AttributFieldType == "Multiple Line Text") {
                            output += "<div class='form-group'><label>" + res[i].AttributeField + " </label><textarea id='" + res[i].AttributeField + "' value=''  class='form-control customfieldgroup' tabindex='10'></textarea> </div>";
                        }
                        else if (res[i].AttributFieldType == "Number") {
                            output += "<div class='form-group'><label>" + res[i].AttributeField + ": </label><div><input type='text' id='" + res[i].AttributeField + "' value='0'  tabindex='10'  class='form-control numeric text-right customfieldgroup' maxlength='10' onkeypress='return isNumberKey(event)'/> </div></div>";
                           
                        }
                        else if (res[i].AttributFieldType == "Date") {
                            output += "<div class='form-group'><label>" + res[i].AttributeField + " </label><input type='text' id='" + res[i].AttributeField + "'class='form-control customfieldgroup datepicker' maxlength='20' readonly='readonly' tabindex='10' /></div>";
                        }
                        else if (res[i].AttributFieldType == "Dropdown" || res[i].AttributFieldType == "Checkbox" || res[i].AttributFieldType == "Sql View" || res[i].AttributFieldType == "Sql Procedure") {
                            if (res[i].AttributFieldType == "Dropdown") {
                                var arr = res[i].AttributeData.split(",");
                                var dd = res[i].AttributFieldType;
                                output += '<div class="form-group" id="sel' + (res[i].AttributeField).replace(/ +/g, "") + '"><label>' + res[i].AttributeField + '</label>&nbsp;&nbsp;';
                                ddllid += "sel" + res[i].AttributeField + "@";
                                output += '<select class="form-control customfieldgroup" id="' + (res[i].AttributeField).replace(/ +/g, "") + '" name="' + res[i].AttributeField + '" tabindex="10" >'
                                output += '<option value="0" >Select</option>'
                                for (var j = 0; j < arr.length; j++) {
                                    output += '<option  >' + arr[j] + '</option>'
                                }
                                output += '</select>'; output += '</div>';
                            }
                            else if (res[i].AttributFieldType == "Checkbox") {
                                var arr = res[i].AttributeData.split(",");
                                chkid += "cb" + (res[i].AttributeField).replace(/ +/g, "") + "@";
                                output += '<div class="form-group customfieldgroup" id="cb' + (res[i].AttributeField).replace(/ +/g, "") + '"><label>' + res[i].AttributeField + '</label>&nbsp;&nbsp;';
                                for (var j = 0; j < arr.length; j++) {
                                    output += '<label><input type="checkbox" tabindex="10" id="' + arr[j] + '"  value="' + arr[j] + '"/>' + arr[j] + '</label>&nbsp;&nbsp;&nbsp;'
                                }
                                output += '</div>';                               
                            }
                            else if (res[i].AttributFieldType == "Sql View") {
                                var dd = res[i].AttributFieldType;
                                output += '<div class="form-group" id="sel' + (res[i].AttributeField).replace(/ +/g, "") + '"><label>' + res[i].AttributeField + '</label>&nbsp;&nbsp;';
                                ddllid += "sel" + res[i].AttributeField + "@";
                                output += '<select class="form-control customfieldgroup" onchange="FillInDropBox(this);" id="' + (res[i].AttributeField).replace(/ +/g, "") + '" name="' + res[i].AttributeField + '" tabindex="10"  >'
                                output += '<option value="0" >Select</option>'
                                for (var j = 0; j < arr.length; j++) {
                                    output += '<option  >' + arr[j] + '</option>'
                                }
                                output += '</select>'; output += '</div>';
                            }
                            else if (res[i].AttributFieldType == "Sql Procedure") {
                                var dd = res[i].AttributFieldType;
                                output += '<div class="form-group" id="sel' + (res[i].AttributeField).replace(/ +/g, "") + '"><label>' + res[i].AttributeField + '</label>&nbsp;&nbsp;';
                                selid += "sel" + res[i].AttributeField + "@";
                                output += '<select class="form-control customfieldgroup"  id="' + (res[i].AttributeField).replace(/ +/g, "") + '" name="' + res[i].AttributeField + '" tabindex="10" >'
                                output += '<option value="0" >Select</option>'
                                output += '</select>'; output += '</div>';
                            }
                        }
                        $('#divCF').html(output);
                    }
                }

            });

            $.ajax({
                contentType: "application/json; charset=utf-8",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetActivityTransactionFill") %>',
                dataType: "json",
                type: "POST",
                data: '{VisId: "' + str + '",HeaderId: "' + HeaderId + '",ActivityId:"' + Activity_Id + '"}',
                success: function (data) {

                    var resdata = JSON.parse(data.d); var Cdata = ""; var cbid = "";
                    for (i = 0; i < resdata.length; i++) {
                        Cdata += resdata[i].AttributeField + "^";
                        if (resdata[i].AttributFieldType == "Dropdown" || resdata[i].AttributFieldType == "Checkbox") {

                            if (resdata[i].AttributFieldType == "Dropdown") {
                                $('#' + resdata[i].AttributeField).val(resdata[i].AttributeValue);
                            }
                            else if (resdata[i].AttributFieldType == "Checkbox") {
                                cbid += "cb" + (resdata[i].AttributeField).replace(/ +/g, "") + "@";
                                if (resdata[i].AttributeValue != null) {
                                    var arr = resdata[i].AttributeValue.split(",");
                                    for (var j = 0; j < arr.length; j++) {
                                        $('#' + arr[j]).attr('checked', true);
                                    }
                                }
                            }
                        }

                        else {
                            $("#" + resdata[i].AttributeField).val(resdata[i].AttributeValue);
                        }
                    }

                    Cdata = Cdata.substr(0, Cdata.length - 1);
                   // $('#ContentPlaceHolder1_hidcustomfields').val(Cdata);

                    cbid = cbid.substr(0, cbid.length - 1);
                    $('#<%=hidchkid.ClientID %>').val(cbid);
                }

            });


            //GetData();
            //GetCustomFields();

            $('#spinner').hide();
        }

        function GetParameterValues(param) {

            var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < url.length; i++) {
                var urlparam = url[i].split('=');
                if (urlparam[0] == param) {
                    return urlparam[1];
                }
            }
        }
    </script>



    <script type="text/javascript">



        $(function () {

            var date = new Date();
            date.setDate(date.getDate());
            $(".datepicker").datepicker({ startDate: date, format: 'dd/M/yyyy', autoclose: true });
            $("#FromDate").datepicker('setDate', date);
            $("#ToDate").datepicker('setDate', date);
            //$("#mainDiv").show();
            //$("#ContentPlaceHolder1_dvFind").hide();
        });


        //function Find() {
        //    ;
        //    $("#mainDiv").hide();
        //    $("#btnFind").show();
        //}

        function validate() {           
            
            var splhidchk = $('#<%=hidchkid.ClientID %>').val().split('@');            
            var cval = "", Ischk = "0";
            $('.customfieldgroup').each(function () {

                //if (($("#ContentPlaceHolder1_hdnActivity").val == 0) || ($("#ContentPlaceHolder1_hdnActivity").val == "")) {
                if ($(this).attr('id').startsWith('cb')) {
                    cbval = "";
                    for (var i = 0; i < splhidchk.length; i++) {
                        if ($(this).attr('id') == splhidchk[i]) {
                            cbval += splhidchk[i] + ":";
                            $("#" + splhidchk[i]).find("input:checked").each(function (i, ob) {
                                cbval += $(ob).val() + "^"; Ischk = "1";
                            });
                        }
                    }
                    if ($(this).hasClass('datepicker')) {
                        if ($(this).val() != "") {
                            var dt = Date.parse($(this).val());
                        }
                        cval += $(this).attr('id') + ":" + $(this).val() + "&";
                    }
                    if (Ischk == "1") {
                        cbval = cbval.substr(0, cbval.length - 1);
                    }

                    cbval = cbval.substr(2, cbval.length);
                    cval += cbval + "&";
                }
                else { cval += $(this).attr('id') + ":" + $(this).val() + "&"; }

                // }
            })

            cval = cval.substr(0, cval.length - 1);           
            $('#<%=hidcustomval.ClientID %>').val(cval);
           
        }



        function GetCustomFields() {

            $('#spinner').show();
            var distpartytype = "";
            var distid = GetParameterValues('DistID');
            var partyid = GetParameterValues('PartyID');
            if (distid != null)
                distpartytype = "Distributor";
            if (partyid != null)
                distpartytype = "Retailer";
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetCustomFieldsforactivityT") %>',
                contentType: "application/json; charset=utf-8",
                data: '{AttrType: "' + distpartytype + '",Fromdate: "' + $('#ContentPlaceHolder1_FromDate').val() + '",todate: "' + $('#ContentPlaceHolder1_ToDate').val() + '"}',
                dataType: "json",
                success: function (data) {

                    jsdata1 = JSON.parse(data.d);
                    var output = "<div class='box-body'>";
                    output += " <div class='row'>";
                    output += "<div >";
                    var Cdata = ""; var cbid = ""; var selid = "";
                    $.each(jsdata1, function (key1, value1) {

                        $('#ContentPlaceHolder1_lbltitle').text(value1.Title);
                        Cdata += value1.AttributeField + "^";
                        if (value1.AttributFieldType == "Number") {
                            output += "<div class='form-group'><label>" + value1.AttributeField + ": </label><div><input type='text' id='" + value1.AttributeField + "' value='0'  tabindex='10'  class='form-control numeric text-right customfieldgroup' maxlength='10' onkeypress='return isNumberKey(event)'/> </div></div>";
                        }
                        else if (value1.AttributFieldType == "Dropdown" || value1.AttributFieldType == "Checkbox" || value1.AttributFieldType == "Sql View" || value1.AttributFieldType == "Sql Procedure") {
                            //var arr = value1.AttributeData.split("*#");
                            var arr = value1.AttributeData.split(",");
                            if (value1.AttributFieldType == "Checkbox") {
                                cbid += "cb" + (value1.AttributeField).replace(/ +/g, "") + "@";
                                output += '<div class="form-group customfieldgroup" id="cb' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                for (var j = 0; j < arr.length; j++) {
                                    output += '<label><input type="checkbox" tabindex="10"  value="' + arr[j] + '"/>' + arr[j] + '</label>&nbsp;&nbsp;&nbsp;'
                                }
                                output += '</div>';
                            }
                            else if (value1.AttributFieldType == "Sql Procedure") {
                                {
                                    var dd = value1.AttributFieldType;
                                    output += '<div class="form-group" id="sel' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                    selid += "sel" + value1.AttributeField + "@";
                                    output += '<select class="form-control customfieldgroup"  id="' + (value1.AttributeField).replace(/ +/g, "") + '" name="' + value1.AttributeField + '" tabindex="10" >'
                                    output += '<option value="0" >Select</option>'
                                    output += '</select>'; output += '</div>';
                                }
                            }
                            else if (value1.AttributFieldType == "Sql View") {
                                var dd = value1.AttributFieldType;
                                output += '<div class="form-group" id="sel' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                selid += "sel" + value1.AttributeField + "@";
                                output += '<select class="form-control customfieldgroup" onchange="FillInDropBox(this);" id="' + (value1.AttributeField).replace(/ +/g, "") + '" name="' + value1.AttributeField + '" tabindex="10"  >'
                                output += '<option value="0" >Select</option>'
                                for (var j = 0; j < arr.length; j++) {
                                    output += '<option  >' + arr[j] + '</option>'
                                }
                                output += '</select>'; output += '</div>';
                            }
                            else {

                                var dd = value1.AttributFieldType;
                                output += '<div class="form-group" id="sel' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                selid += "sel" + value1.AttributeField + "@";
                                output += '<select class="form-control customfieldgroup" id="' + (value1.AttributeField).replace(/ +/g, "") + '" name="' + value1.AttributeField + '" tabindex="10" >'
                                output += '<option value="0" >Select</option>'
                                for (var j = 0; j < arr.length; j++) {
                                    output += '<option  >' + arr[j] + '</option>'
                                }
                                output += '</select>'; output += '</div>';
                            }


                        }
                        else if (value1.AttributFieldType == "Date") {
                            output += "<div class='form-group'><label>" + value1.AttributeField + " </label><input type='text' id='" + value1.AttributeField + "'class='form-control customfieldgroup datepicker' maxlength='20' readonly='readonly' tabindex='10' /></div>";
                        }
                        else if (value1.AttributFieldType == "Multiple Line Text") {
                            output += "<div class='form-group'><label>" + value1.AttributeField + " </label><textarea id='" + value1.AttributeField + "' value=''  class='form-control customfieldgroup' tabindex='10'></textarea> </div>";
                        }
                        else {
                            output += "<div class='form-group'><label>" + value1.AttributeField + " </label><input type='text' id='" + value1.AttributeField + "' value='' class='form-control customfieldgroup' maxlength='20' tabindex='10'/> </div>";
                        }

                    });
                    output += "</div> <script type='text/javascript'>  $(function () {var date = new Date(); date.setDate(date.getDate()); $('.datepicker').datepicker({ startDate: date, format: 'dd/M/yyyy', autoclose: true });});";

                    Cdata = Cdata.substr(0, Cdata.length - 1);
                    $('#<%=hidcustomfields.ClientID %>').val(Cdata);
                    cbid = cbid.substr(0, cbid.length - 1);
                    $('#<%=hidchkid.ClientID %>').val(cbid);

                    $('#divCF').html(output);


                    $('#spinner').hide();
                }
            });
          


        }


        function GetParameterValues(param) {
            var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < url.length; i++) {
                var urlparam = url[i].split('=');
                if (urlparam[0] == param) {
                    return urlparam[1];
                }
            }
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
    <section class="content">

        <div id="spinner" class="spinner" style="display: none;"><img id="img-spinner" src="img/loader.gif" alt="Loading" /><br />Loading Data....</div>
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
                            <%--<h3 class="box-title">Activity</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>    
                            <div style="float: right">
                                <%-- <input type="button" class="btn btn-primary" value="Find" id="btnFind" />--%>

                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBck" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBck_Click" />
                            </div>

                        </div>

                        <div class="box-body">
                            <div class="col-lg-3 col-md-5 col-sm-7 col-xs-11">

                                <div class="form-group">
                                    <label for="exampleInputEmail1">Title:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <select id="ddlTitle" class="form-control"></select>
                                    <asp:HiddenField runat="server" ID="hdnValue"></asp:HiddenField>
                                    <asp:HiddenField runat="server" ID="hdnActivity"></asp:HiddenField>
                                </div>


                                <input type="hidden" id="hidcustomfields" runat="server" />
                                <input type="hidden" id="hidchkid" runat="server" />

                                <input id="hidcustomval" type="hidden" runat="server" />                               

                                <div id="divCF" class="form-group">
                                </div>

                            </div>
                        </div>

                        <div class="box-footer">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClientClick="return validate();" TabIndex="11" OnClick="btnSave_Click" />
                            <asp:Button ID="btndelete" runat="server" Visible="false" CssClass="btn btn-primary" Text="Delete" OnClientClick="Confirm() ;" TabIndex="11" />
                            <input type="button" style="display: none" id="btnCancel" class="btn btn-primary" value="Cancel" tabindex="11" />
                            <asp:Button ID="btncancel1" runat="server" CssClass="btn btn-primary" Text="Cancel" TabIndex="11" />
                            
                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="box-body" id="dvFind" runat="server" style="display: none">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Activity List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" />

                            </div>
                        </div>


                        <div class="box-body table-responsive">
                            <table id="tblfinddata" class="table table-bordered table-striped">
                                <thead>
                                    <tr>
                                        <%--<th>Title</th>--%>
                                        <th>From Date</th>
                                        <th>To Date</th>
                                        <th>For</th>
                                        <th>Title</th>
                                        <th style="display: none">ActivityId</th>
                                        <th style="display: none">VisitId</th>
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
