<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="TravelConveyanceLimit.aspx.cs" Inherits="AstralFFMS.TravelConveyanceLimit" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style type="text/css">
        .spinner {
            position: absolute;
            left: 50%;
            margin-left: -50px; /* half width of the spinner gif */
            margin-top: -50px; /* half height of the spinner gif */
            text-align: center;
            z-index: 999;
            overflow: auto;
            width: 100px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }

        #select2-ContentPlaceHolder1_ddlParentLoc-container {
            margin-top: -8px !important;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .modal-content {
            border-radius: 14px;
        }

        .modal-header {
            background-color: #367fa9;
            color: white;
            border-top-left-radius: 14px;
            border-top-right-radius: 14px;
        }
    </style>
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
        //$(function () {
        //    $(".select2").select2();
        //});
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

        $(document).ready(function () {

            $('#<%=btnDelete.ClientID%>').attr('style', 'visibility:hidden');

            var data = [];
            $('#example').DataTable({
                data: data,
                deferRender: true,
                "sScrollX": "100%",
                "scrollX": false,
                scrollY: "100%",
                scrollCollapse: false,
                scroller: true,
                "searching": true,   // Search Box will Be Disabled

                "ordering": false,    // Ordering (Sorting on Each Column)will Be Disabled

                "info": false,         // Will show "1 to n of n entries" Text at bottom

                "lengthChange": false,
                "bPaginate": false,
                "columnDefs": [
                     {
                         "targets": 0,
                         "visible": false
                     },
                     {
                         "targets": -1,
                         "data": null,
                         "defaultContent": "<a>Edit</a>"
                     }]
            });

            var myTable = $('#example').DataTable();


            $('#example tbody').on('click', 'a', function () {
                //UpdateNew(Id)
                console.log(myTable.row($(this).parents('tr')).data()[0]);
                //myTable.row($(this).parents('tr')).remove().draw();
            });

        });


        function convertTCToArrayObject() {

            var tcdetails = new Array();
            var cnt = 0;

            var parDT = $(".dtq").parent().attr('class');
            $('.' + parDT + ' .distancedesc').each(function () {
                cnt = cnt + 1;
            })
            // alert(cnt);
            var j = 0;
            for (var i = 1; i <= cnt; i++) {
                var tcd = new Object();

                var lid = "#Distance" + i;
                var tid = "#Amount" + i;

                //alert($(lid).text());
                //alert($(tid).val());

                tcd["Distance"] = $(lid).val();
                tcd["Amount"] = $(tid).val();

                tcdetails[j] = tcd;
                j++;
            }

            return tcdetails;
        }

        //function UpdateNew(Id) {
        //    if (Id != "") {
        //        document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'none';
        //        document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'block';
        //        $('#spinner').show();

        //        $.ajax({
        //            type: "POST",
        //            url: "TravelConveyanceLimit.aspx/get_data",
        //                contentType: "application/json; charset=utf-8",
        //                processData: false,

        //                data: '{Id: "' + Id + '"}',
        //                dataType: "json",
        //                success: function (data) {
        //                    console.log(JSON.parse(data.d));

        //                    var jsdata1 = JSON.parse(data.d);
        //                    console.log(jsdata1);

        //                    var t = $('#example').DataTable();
        //                    $.each(jsdata1, function (key1, value1) {

        //                        t.row.add([
        //                        value1.Id,
        //                    value1.CityType,
        //                    value1.Designation,
        //                    //value1.Ex,
        //                    value1.SyncId,
        //                    value1.Active1

        //                        ]).draw(false);
        //                    });


        //                },
        //                error: function (data) {
        //                    alert("1");
        //                    //document.getElementById("loadernew").style.display = "none";
        //                    // $("#save_add_quotation").prop('disabled', false);
        //                    console.log(data);
        //                }
        //            });


        //        }


        //    }


            function SaveNew() {
                $("#btnSave").prop('disabled', true);


                var checknumber = "0";
                if ($('#<%=ddlcitytype.ClientID%>').val() == "0") {

                errormessage("Please select City Type");
                checknumber = "1";
                //return false;
            }
            if (($('#<%=ddldesignation.ClientID%>').val() == "0") && checknumber == "0") {

                errormessage("Please select Designation");
                return false;
            }


            if (checknumber == "0") {
                var parDT = $(".dtq").parent().attr('class');
                var distanceval = ""; var amountval = "";

                $('.' + parDT + ' .distancedesc').each(function () {

                    var a = $(this).val();

                    if (a.length == 0) {

                        checknumber = "1";
                    }
                    distanceval += $(this).val() + ",";

                })


                if (checknumber == "1") {
                    errormessage("Please Enter Distance.");
                    return false;
                }
                distanceval = distanceval.substr(0, distanceval.length - 1);
            }

            if (checknumber == "0") {
                $('.' + parDT + ' .amountdesc').each(function () {
                    var a = $(this).val();

                    if (a.length == 0) {

                        checknumber = "1";
                    }
                    amountval += $(this).val() + ",";


                })


                if (checknumber == "1") {
                    errormessage("Please Enter Amount.");
                    return false;
                }

                amountval = amountval.substr(0, amountval.length - 1);
            }

            if (checknumber == "0") {

                var tcdetails = convertTCToArrayObject();

                var tcdetails1 = JSON.stringify(tcdetails);

                var Id = $('#<%=HiddenTravelId.ClientID%>').val();
                var checked1 = $('#<%= chkIsActive.ClientID %>').is(':checked');




                $.ajax({
                    type: "POST",
                    url: "TravelConveyanceLimit.aspx/save_data",
                    contentType: "application/json; charset=utf-8",
                    processData: false,
                    //chkIsActive
                    data: '{DistanceDetails:' + tcdetails1 + ',NighthaltAmt:"' + $('#<%=txtnighthalt.ClientID%>').val() + '",Id: "' + Id + '",CityType: "' + $('#<%=ddlcitytype.ClientID%>').val() + '",Designation: "' + $('#<%=ddldesignation.ClientID%>').val() + '",Remark: "' + $('#<%=Remarks.ClientID%>').val() + '",SyncId: "' + $('#<%=SyncId.ClientID%>').val() + '",Active: "' + checked1 + '"}',
                     dataType: "json",
                     success: function (data) {
                         $("#btnSave").prop('disabled', false);
                         var a = JSON.parse(data.d)
                         console.log(a.Msg);
                         if ((a.Msg == "Record Inserted Successfully.") || (a.Msg == "Record Updated Successfully.")) {

                             clearcontrols();
                             Successmessage(a.Msg);
                         }
                         else {

                             errormessage(a.Msg);
                         }


                         // location.reload();
                     },
                     error: function (data) {
                         console.log(data);
                     }
                });

             }
         }

         function clearcontrols() {

             document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
             document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';

             $('#<%=HiddenTravelId.ClientID%>').val('');
            $('#<%=HiddenCity.ClientID%>').val('');
            BindCity();
            $('#<%=HiddenDesignation.ClientID%>').val('');
            BindDesignation();


            var val2 = $('.dtq').length;
            if (val2 == 1) {
                $('#Distance1').val('');
                $('#Account1').val('');
            }
            else {

                $('.dtq').empty();
                var html;

                html = '<div class="clearfix"></div><div class="clearfix"></div><div class="dtq" id="divstagedistance1" >';
                html += '<div class="row"><div class="form-group"><div class="form-group col-md-5"><label for="exampleInputEmail1">Distance (In Km):</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>';
                html += '<input type="text" class="form-control numeric text-right distancedesc" onkeypress="javascript:return isNumber (event)" maxlength="4" placeholder="Enter Distance" tabindex="6" id="Distance1">';
                html += '</div>';
                html += '<div class="form-group col-md-5"><label for="exampleInputEmail1">Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><input type="text" class="form-control numeric text-right amountdesc" onkeypress="javascript:return isNumber (event)" maxlength="15" id="Amount1" placeholder="Enter Amount" tabindex="7"></div>';
                html += '<div class="form-group col-md-2"><img id="imgaddphone" style="height: 15px; width: 15px;margin-top: 30px;margin-left: -8px;" src="img/add1.png" alt="Add" onclick="AddData();" /></div></div></div>';
                // html += '<img src="img/substract.png" style="height: 15px;width: 15px;margin-top: -30px;margin-left: 37px;" alt="Remove" onclick="RemoveDistanceState(' + val2 + ',0);" />';
                html += '</div></div>';

                $("#divstagedistance" + $('.dtq').length).after(html);
            }


            $('#<%=Remarks.ClientID%>').val('');
            $('#<%=SyncId.ClientID%>').val('');
            $('#<%=txtnighthalt.ClientID%>').val('0');
            $('#<%= chkIsActive.ClientID %>').prop("checked", true);


        }
        function validate() {

            if ($('#<%=ddlcitytype.ClientID%>').val() == "0") {
                errormessage("Please select City Type");
                return false;
            }
            if ($('#<%=ddldesignation.ClientID%>').val() == "0") {
                errormessage("Please select Designation");
                return false;
            }



        }
        function AddData() {

            //alert("1");
            var val2 = $('.dtq').length + 1;
            //alert(val2);
            var html;

            html = '<div class="clearfix"></div><div class="clearfix"></div><div class="dtq" id="divstagedistance' + val2 + '" ><hr>';
            html += '<div class="row"><div class="form-group"><div class="form-group col-md-5"><label for="exampleInputEmail1">Distance (In Km):</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>';
            html += '<input type="text" class="form-control numeric text-right distancedesc" onkeypress="javascript:return isNumber (event)" maxlength="4" placeholder="Enter Distance" tabindex="6" id="Distance' + val2 + '">';
            html += '</div>';
            html += '<div class="form-group col-md-5"><label for="exampleInputEmail1">Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><input type="text" class="form-control numeric text-right amountdesc" onkeypress="javascript:return isNumber (event)" maxlength="15" id="Amount' + val2 + '" placeholder="Enter Amount" tabindex="7"></div>';
            html += '<div class="form-group col-md-2"><img id="imgaddphone" style="height: 15px; width: 15px;margin-top: 30px;margin-left: -8px;" src="img/add1.png" alt="Add" onclick="AddData();" /><img src="img/substract.png" style="height: 15px;width: 15px;margin-top:30px;margin-left: 10px;" alt="Remove" onclick="RemoveDistanceState(' + val2 + ',0);" /></div></div></div>';
            // html += '<img src="img/substract.png" style="height: 15px;width: 15px;margin-top: -30px;margin-left: 37px;" alt="Remove" onclick="RemoveDistanceState(' + val2 + ',0);" />';
            html += '</div></div>';

            $("#divstagedistance" + $('.dtq').length).after(html);

            //alert("a");
        }

        function RemoveDistanceState(id, stageid) {


            $('#<%=HiddenDistance.ClientID%>').val($('#Distance' + id).val());
            $('#<%=HiddenAmount.ClientID%>').val($('#Amount' + id).val());
            //$('.dtq').empty();
            if ($('#<%=HiddenDistance.ClientID%>').val() != '' && $('#<%=HiddenField2.ClientID%>').val() != '' && $('#<%=HiddenAmount.ClientID%>').val()) {
                $('.dtq').empty();
                getdtqdetails($('#<%=HiddenField2.ClientID%>').val(), $('#<%=HiddenDistance.ClientID%>').val(), $('#<%=HiddenAmount.ClientID%>').val());

            }
            else {
                $("#divstagedistance" + id).remove();
            }
           <%-- getdtqdetails($('#<%=HiddenField2.ClientID%>').val(), $('#<%=HiddenDistance.ClientID%>').val(), $('#<%=HiddenAmount.ClientID%>').val());--%>
            //$("#divstagedistance" + id).remove();
            // alert($('#<%=HiddenField2.ClientID%>').val());
            // edittraveldetails($('#<%=HiddenField2.ClientID%>').val());
        }
        function AddData_Old() {
            var duplicaterow = 0;
            var valid = 1;
            var distance = $('#Distance').val();
            //var table1 = $('#example').DataTable();
            //var data = table1.rows().data();
            //var id = data.length + 1;
            //for (var i = 0; i < data.length; i++) {
            //    if (distance == data[i][1]) {
            //        duplicaterow = 1;
            //        errormessage("Amount already added for this Distance.");
            //        return false;
            //    }

            //}

            if (distance == "") {
                // $('#spinnerfile').hide();
                errormessage("Please enter Distance.");
                return false;
            }

            else if ($('#Amount').val() == "") {
                // $('#spinnerfile').hide();
                errormessage("Please enter Amount.");
                return false;
            }

            if ((valid == 1) && (duplicaterow == 0)) {
                var Amount = $('#Amount').val();
                var t = $('#example').DataTable();

                t.row.add([
                    id,
                distance,
                Amount
                ]).draw(false);

                $('#Amount').val("");
                $('#Distance').val("");
            }
        }

        function getdistancedetails(id) {
            $.ajax({

                type: "POST",
                url: "TravelConveyanceLimit.aspx/get_data",
                contentType: "application/json; charset=utf-8",
                processData: false,
                data: '{Id: "' + id + '"}',
                dataType: "json",

                success: function (data) {
                    debugger;
                    //alert(1);
                    jsdata1 = JSON.parse(data.d);
                    console.log(jsdata1);

                    var output = "<table class='table table-striped'><tr><th>Distance (In Km)</th><th>Amount</th></tr>"
                    $.each(jsdata1, function (key1, value1) {

                        output += " <tr><td class='tdcls'>" + value1.Distance + " </td><td class='tdcls'>" + value1.Amount + " </td></tr> "

                    });
                    output += "</table>";
                    $("#Attachments1").html(output);
                    $("#modal-attachments1").modal('show');

                    // alert("1");

                },
                error: function (data) {
                    debugger;
                    // alert("2");
                    console.log(data);
                }
            });
        }

        function BindCity() {


            $('#<%=ddlcitytype.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "Get",
                url: '<%= ResolveUrl("And_Sync.asmx/GetCity_Travel") %>',

                  //   data: JSON.stringify(obj),
                  // contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  success: OnPopulated,
                  failure: function (response) {
                      alert(response);
                  }
              });
              function OnPopulated(response) {
                  console.log(response)
                  PopulateControl(response, $("#<%=ddlcitytype.ClientID %>"));
            }
            function PopulateControl(list, control) {
                if (list.length > 0) {
                    //  control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(list, function () {
                        control.append($("<option></option>").val(this['Id']).html(this['Name']));
                    });
                    var id = $('#<%=HiddenCity.ClientID%>').val();
                      //  alert(id);
                      if (id != "") {
                          control.val(id);
                      }
                  }
                  else {
                      control.empty().append('<option selected="selected" value="0">Not available<option>');
                  }

              }
          }

          function BindDesignation() {


              $('#<%=ddldesignation.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "Get",
                url: '<%= ResolveUrl("And_Sync.asmx/GetDesiignation_Travel") %>',

                //   data: JSON.stringify(obj),
                // contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response);
                }
            });
            function OnPopulated(response) {
                console.log(response)
                PopulateControl(response, $("#<%=ddldesignation.ClientID %>"));
              }
              function PopulateControl(list, control) {
                  if (list.length > 0) {
                      //  control.removeAttr("disabled");
                      control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                      $.each(list, function () {
                          control.append($("<option></option>").val(this['DesId']).html(this['DesName']));
                      });
                      var id = $('#<%=HiddenDesignation.ClientID%>').val();
                    //  alert(id);
                    if (id != "") {
                        control.val(id);
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">Not available<option>');
                }

            }
        }

        function edittraveldetails(id) {
            $.ajax({

                type: "POST",
                url: "TravelConveyanceLimit.aspx/getEditdata",
                contentType: "application/json; charset=utf-8",
                processData: false,
                data: '{Id: "' + id + '"}',
                dataType: "json",

                success: function (data) {
                    debugger;
                    //  alert(1
                    $('#<%=btnDelete.ClientID%>').attr('style', 'visibility:visible');
                     $('#<%=HiddenField2.ClientID%>').val(id);
                     document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                     document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                     $('#spinner').show();
                     jsdata1 = JSON.parse(data.d);
                     console.log("check");
                     console.log(jsdata1);
                     console.log(jsdata1[0].TravelDistanceConveyance);
                     $('#<%=HiddenCity.ClientID%>').val(jsdata1[0].CityTypeId);
                     BindCity();
                     $('#<%=HiddenDesignation.ClientID%>').val(jsdata1[0].DesId);
                     BindDesignation();
                     $('#<%=Remarks.ClientID%>').val(jsdata1[0].Remarks);
                     $('#<%=SyncId.ClientID%>').val(jsdata1[0].SyncId);
                     $('#<%=txtnighthalt.ClientID%>').val(jsdata1[0].NighthaltAmt);
                     var ac = jsdata1[0].Active;

                     // var ex = jsdata1[0].Ex;

                     if (ac == "Yes") {
                         $('#<%= chkIsActive.ClientID %>').prop("checked", true);
                     }
                     else {
                         $('#<%= chkIsActive.ClientID %>').prop("checked", false);
                     }

                     $('#<%=HiddenTravelId.ClientID%>').val(jsdata1[0].Id);
                     var i = 0;
                     $.each(jsdata1[0].TravelDistanceConveyance, function (key1, value1) {
                         if (i == 0) {
                             $('#Distance1').val(value1.Distance);
                             $('#Amount1').val(value1.Amount);
                         }
                         else {

                             var val2 = $('.dtq').length + 1;

                             var html;

                             html = '<div class="clearfix"></div><div class="clearfix"></div><div class="dtq" id="divstagedistance' + val2 + '" ><hr>';
                             html += '<div class="row"><div class="form-group"><div class="form-group col-md-5"><label for="exampleInputEmail1">Distance (In Km):</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>';
                             html += '<input type="text" class="form-control numeric text-right distancedesc" onkeypress="javascript:return isNumber (event)" maxlength="4" Value="' + value1.Distance + '" placeholder="Enter Distance" tabindex="6" id="Distance' + val2 + '">';
                             html += '</div>';
                             html += '<div class="form-group col-md-5"><label for="exampleInputEmail1">Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><input type="text" class="form-control numeric text-right amountdesc" onkeypress="javascript:return isNumber (event)" Value="' + value1.Amount + '" maxlength="15" id="Amount' + val2 + '" placeholder="Enter Amount" tabindex="7"></div>';
                             html += '<div class="form-group col-md-2"><img id="imgaddphone" style="height: 15px; width: 15px;margin-top: 30px;margin-left: -8px;" src="img/add1.png" alt="Add" onclick="AddData();" /><img src="img/substract.png" style="height: 15px;width: 15px;margin-top:30px;margin-left: 10px;" alt="Remove" onclick="RemoveDistanceState(' + val2 + ',0);" /></div></div></div>';
                             // html += '<img src="img/substract.png" style="height: 15px;width: 15px;margin-top: -30px;margin-left: 37px;" alt="Remove" onclick="RemoveDistanceState(' + val2 + ',0);" />';
                             html += '</div></div>';

                             $("#divstagedistance" + $('.dtq').length).after(html);
                         }
                         i = i + 1;

                     });
                     $('#spinner').hide();
                     //  document.getElementById('#<%= btnDelete.ClientID %>').style.visibility = "visible";
                     //   $('#<%=btnDelete.ClientID%>').attr('style', 'display:block');



                 },
                error: function (data) {
                    debugger;
                    // alert("2");
                    console.log(data);
                }
            });
        }

        function getdtqdetails(id, distance, amount) {
            $.ajax({

                type: "POST",
                url: "TravelConveyanceLimit.aspx/getDtqdata",
                contentType: "application/json; charset=utf-8",
                processData: false,
                data: '{Id: "' + id + '",Distance: "' + distance + '",Amount: "' + amount + '"}',
                dataType: "json",

                success: function (data) {
                    debugger;
                    //  alert(1

                    // $('#<%=HiddenField2.ClientID%>').val(id);
                    //document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                    //document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                    $('#spinner').show();
                    jsdata1 = JSON.parse(data.d);
                    var i = 0;
                    $.each(jsdata1[0].TravelDistanceConveyance, function (key1, value1) {
                        if (i == 0) {
                            // var val2 = $('.dtq').length + 1;
                            var val2 = i + 1;

                            var html;

                            html = '<div class="clearfix"></div><div class="clearfix"></div><div class="dtq" id="divstagedistance' + val2 + '" ><hr>';
                            html += '<div class="row"><div class="form-group"><div class="form-group col-md-5"><label for="exampleInputEmail1">Distance (In Km):</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>';
                            html += '<input type="text" class="form-control numeric text-right distancedesc" onkeypress="javascript:return isNumber (event)" maxlength="4" Value="' + value1.Distance + '" placeholder="Enter Distance" tabindex="6" id="Distance' + val2 + '">';
                            html += '</div>';
                            html += '<div class="form-group col-md-5"><label for="exampleInputEmail1">Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><input type="text" class="form-control numeric text-right amountdesc" onkeypress="javascript:return isNumber (event)" Value="' + value1.Amount + '" maxlength="15" id="Amount' + val2 + '" placeholder="Enter Amount" tabindex="7"></div>';
                            html += '<div class="form-group col-md-2"><img id="imgaddphone" style="height: 15px; width: 15px;margin-top: 30px;margin-left: -8px;" src="img/add1.png" alt="Add" onclick="AddData();" /></div></div></div>';
                            // html += '<img src="img/substract.png" style="height: 15px;width: 15px;margin-top: -30px;margin-left: 37px;" alt="Remove" onclick="RemoveDistanceState(' + val2 + ',0);" />';
                            html += '</div></div>';

                            $("#divstagedistance" + $('.dtq').length).after(html);
                        }
                        else {

                            var val2 = i + 1;

                            var html;

                            html = '<div class="clearfix"></div><div class="clearfix"></div><div class="dtq" id="divstagedistance' + val2 + '" ><hr>';
                            html += '<div class="row"><div class="form-group"><div class="form-group col-md-5"><label for="exampleInputEmail1">Distance (In Km):</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>';
                            html += '<input type="text" class="form-control numeric text-right distancedesc" onkeypress="javascript:return isNumber (event)" maxlength="4" Value="' + value1.Distance + '" placeholder="Enter Distance" tabindex="6" id="Distance' + val2 + '">';
                            html += '</div>';
                            html += '<div class="form-group col-md-5"><label for="exampleInputEmail1">Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><input type="text" class="form-control numeric text-right amountdesc" onkeypress="javascript:return isNumber (event)" Value="' + value1.Amount + '" maxlength="15" id="Amount' + val2 + '" placeholder="Enter Amount" tabindex="7"></div>';
                            html += '<div class="form-group col-md-2"><img id="imgaddphone" style="height: 15px; width: 15px;margin-top: 30px;margin-left: -8px;" src="img/add1.png" alt="Add" onclick="AddData();" /><img src="img/substract.png" style="height: 15px;width: 15px;margin-top:30px;margin-left: 10px;" alt="Remove" onclick="RemoveDistanceState(' + val2 + ',0);" /></div></div></div>';
                            // html += '<img src="img/substract.png" style="height: 15px;width: 15px;margin-top: -30px;margin-left: 37px;" alt="Remove" onclick="RemoveDistanceState(' + val2 + ',0);" />';
                            html += '</div></div>';

                            $("#divstagedistance" + $('.dtq').length).after(html);
                        }
                        i = i + 1;

                    });
                    $('#spinner').hide();


                },
                error: function (data) {
                    debugger;
                    // alert("2");
                    console.log(data);
                }
            });
        }
    </script>

    <script>
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (!(iKeyCode != 8)) {
                e.preventDefault();
                return false;
            }
            return true;
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
        function DoNav(Id) {
            if (Id != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', Id)
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
                            <%--<h3 class="box-title">Travel Conveyance  Limit </h3>--%>
                            <h3 class="box-title">
                                <asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                    OnClick="btnFind_Click" />

                                <%-- <input type="button" style="margin-right: 5px;" id="btnFind" value="Find" class="btn btn-primary" onclick="UpdateNew('0');"/>--%>

                                <%--  <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" runat="server" />--%>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">City Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlcitytype" Width="100%" CssClass="form-control" runat="server" TabIndex="2"></asp:DropDownList>

                                        <asp:HiddenField ID="HiddenCity" runat="server" />
                                        <asp:HiddenField ID="HiddenTravelId" runat="server" />
                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                        <asp:HiddenField ID="HiddenDistance" runat="server" />
                                        <asp:HiddenField ID="HiddenAmount" runat="server" />
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Designation:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddldesignation" Width="100%" CssClass="form-control" runat="server" TabIndex="4"></asp:DropDownList>
                                        <asp:HiddenField ID="HiddenDesignation" runat="server" />
                                    </div>
                                    <div class="distancestage">
                                        <div class="dtq" id="divstagedistance1">
                                            <div class="row">
                                                <div class="form-group">
                                                    <div class="form-group col-md-5">
                                                        <label for="exampleInputEmail1">Distance (In Km):</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                        <input type="text" class="form-control numeric text-right distancedesc" onkeypress="javascript:return isNumber (event)" maxlength="4" id="Distance1" placeholder="Enter Distance" tabindex="6">
                                                    </div>

                                                    <div class="form-group col-md-5">
                                                        <label for="exampleInputEmail1">Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                        <input type="text" class="form-control numeric text-right amountdesc" onkeypress="javascript:return isNumber (event)" maxlength="15" id="Amount1" placeholder="Enter Amount" tabindex="7">
                                                    </div>

                                                    <div class="form-group col-md-2">

                                                        <img id="imgaddphone" style="height: 15px; width: 15px; margin-top: 30px; margin-left: -8px;" src="img/add1.png" alt="Add" onclick="AddData();" />
                                                        <%--<i class="fa fa-plus" onclick="AddData();" style="font-size:36px;margin-right: 5px;margin-top: 20px;color:lightblue;"></i>--%>
                                                        <%--<button type="button" id="btnAddDistance" onclick="AddData();" class="btn btn-primary" style="margin-right: 5px;margin-top: 20px;width: 60px;">Add</button>--%>

                                                        <%--<asp:Button Style="margin-right: 5px;margin-top: 20px;width: 60px;" type="button" ID="btnAddDistance" runat="server" Text="Add" class="btn btn-primary"  OnClientClick="javascript:return validate();" />--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Night Halt Amount:</label>
                                        <input type="text" runat="server" class="form-control numeric text-right distancedesc" onkeypress="javascript:return isNumber (event)" maxlength="20" id="txtnighthalt" placeholder="Enter Remark" tabindex="8" value="0">
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Remark:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="500" id="Remarks" placeholder="Enter Remark" tabindex="9">
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Sync Id:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="20" id="SyncId" placeholder="Enter Sync Id" tabindex="10">
                                    </div>
                                    <div class="form-group">

                                        <div class="row">
                                            <div class="col-md-1">
                                                <label for="exampleInputEmail1">Active:</label>
                                            </div>
                                            <div class="col-md-4" style="margin-left: 1%;">
                                                <input id="chkIsActive" runat="server" type="checkbox" checked="checked" class="checkbox" tabindex="12" />
                                            </div>
                                        </div>
                                        <input id="HdnFldIsActive" hidden="hidden" value="N">
                                        <div class="row">
                                        </div>


                                    </div>


                                </div>

                                <%--                                 <div class="col-md-6">


                                <table id="example" class="table table-striped table-bordered" style="width:100%">
                                <thead>
                                    <tr>
                                        <th style="display:none;">Id</th>
                                        <th>Distance (In Km)
                                        </th>
                                        <th>Amount</th>
                                        <th></th>
                                    </tr>
                                </thead>
                            </table>
                                </div>--%>
                            </div>
                        </div>
                        <div class="box-footer">
                            <div class="row">
                                <div class="col-md-6">
                                    <%--<button runat="server" type="button" id="btnSave" onclick="SaveNew();" class="btn btn-primary">Save</button>--%>
                                    <input type="button" style="margin-right: 5px;" id="btnSave" value="Save" class="btn btn-primary" onclick="SaveNew();" />
                                    <%-- <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary"  OnClientClick="SaveNew();" TabIndex="28"/>--%>
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" TabIndex="30" OnClick="btnCancel_Click" />
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" TabIndex="32" OnClick="btnDelete_Click" Visible="true" />
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

        <div class="box-body" id="rptmain" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Travel Conveyance List</h3>
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
                                                <th>City Type</th>
                                                <th>Designation</th>
                                                <th>Night Halt Amount</th>
                                                <th>Sync ID</th>
                                                <th>Active</th>
                                                <th></th>
                                                <th></th>

                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("Id") %>' />

                                        <td><%#Eval("Name") %></td>
                                        <td><%#Eval("DesName") %></td>

                                        <td><%#Eval("NighthaltAmt") %></td>
                                        <td><%#Eval("SyncId") %></td>
                                        <td><%#Eval("Active1") %></td>
                                        <td><a data-toggle="tooltip" title="View" onclick="getdistancedetails('<%#Eval("Id") %>');">View
                                        </a></td>
                                        <td><a data-toggle="tooltip" title="Edit" onclick="edittraveldetails('<%#Eval("Id") %>');">Edit
                                        </a></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>


                        <div class="modal fade in" id="modal-attachments1" style="padding-right: 17px;">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">×</span></button>
                                        <h3 class="modal-title">Travel Distance Details</h3>
                                    </div>
                                    <div class="modal-body" id="Attachments1">
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>

                                    </div>
                                </div>

                            </div>

                        </div>


                        <!-- /.box-body -->
                    </div>
                    <!-- /.box -->

                </div>
                <!-- /.col -->
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
    </script>
</asp:Content>
