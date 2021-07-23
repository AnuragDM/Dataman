<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="CRM_CustomFields.aspx.cs" EnableEventValidation="false" Inherits="AstralFFMS.CRM_CustomFields" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

        .btnEnable
        {
          cursor:pointer;
          cursor:hand;

        }
        .btnDisable
        {
            pointer-events: none;
            cursor: default;
        }
    </style>
    <script type="text/javascript">
     
        $(document).ready(function () {
       
            $("#btnadd").show();
            $("#adddata").hide();
           // SetPermissions();
            GetData();
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
        <div class="box-body" id="mainDiv" style="display: block;" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Add Custom Fields</h3>
                        
                     
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                         <div id="showdata" class="box-body">
                          
                              
                               
                          
                        </div>
                     
                        <div class="box-body">
                                 <a  id="btnadd" >Add Custom Fields</a>   <%----%>
                             
                                <ul id="adddata" style="list-style: none;padding-left:0px;cursor:pointer;cursor:hand">
								
								</ul>
                               
                          
                        </div>
                     </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>


            </div>

        

    </section>
     <script type="text/javascript">
         var myarray = new Array();
         var EditP;
         var DelP;
         var AddP;
         function isNumberKey(evt) {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 31 && (charCode < 48 || charCode > 57))
                 return false;
             return true;
         }
         function editOption(value) {
            $("#demo" + value).empty();
             var li = $("<div id='" + value + "' class='col-lg-5 col-md-6 col-sm-8 col-xs-10'>");
             li.html(GetDynamicControls(value));
             $("#demo" + value).append(li);

             $.ajax({
                 type: "POST",
                 url: '<%= ResolveUrl("CRMDBService.asmx/GetCustomFieldsDataById") %>',
                 contentType: "application/json; charset=utf-8",
                 data: '{Id: "' + value + '"}',
                 dataType: "json",
                 success: function (data) {
                     jsdata1 = JSON.parse(data.d);
                     var output = "<ul class='list-group'>";
                     $.each(jsdata1, function (key1, value1) {
                         $('#dynfname'+value).val(value1.AttributeField);
                         $('#dynddl' + value).val(value1.AttributeTable);
                         //alert(value1.AttributFieldType);
                        debugger;
                         //alert($('#dynftype' + value).val());
                         $('#dynftype' + value).val(value1.AttributFieldType);
                        // $('#chkIsActive' + value).val(value1.Active1);

                         $('#chkIsActive' + value).prop('checked', value1.Active1);
                         //$('#chkIsActive' + value).prop('checked', false);
                        // alert($('#dynftype' + value).val());
                         GenerateUpdDDlControls(value1.AttributeData, value1.AttributFieldType,value)
                     });
                 }
               });
         }
         function GenerateUpdDDlControls(arrData, UpdControlName,value) {
             debugger;
             if (arrData != "") {
                 if (UpdControlName == "Dropdown" || UpdControlName == "Checkbox") {
                     var arr = arrData.split("*#"); 
                     var creatediv = ""; var index = 1;
                     creatediv = '<div id="ddoptioncontainer"><div class="form-group" id="Edele0"><label>Add ' + UpdControlName + '  Values </label> </div> '
                     for (var j = 0; j < arr.length; j++) {
                         if (arr.length - 1 == 0) {
                             creatediv += '<div class="form-group" id="Edele' + index + '"><div class="row"><div class="col-lg-9"> <input type="text" maxlength="20" id="txtaddval' + index + '" value="' + arr[j] + '" class="form-control" disabled/></div><div class="col-lg-3"> <a id="eda' + index + '" onclick="EditDynamicOption(' + index + ')"><label class="col-dm-1 control-label"><i class="fa fa-plus"></i></label></a></div></div></div>'
                         }
                         else {
                             if (j == 0) {
                                 creatediv += '<div class="form-group" id="Edele' + index + '"><div class="row"><div class="col-lg-9"> <input type="text" maxlength="20" id="txtaddval' + index + '" value="' + arr[j] + '" class="form-control" disabled/></div></div></div>'
                             }
                             else if (arr.length -1 == j) {
                                 creatediv += '<div class="form-group" id="Edele' + index + '"><div class="row"><div class="col-lg-9"> <input type="text" maxlength="20" id="txtaddval' + index + '" value="' + arr[j] + '" class="form-control"  disabled/></div><div class="col-lg-3;" > <a  onclick="EditCloseOption(' + index + ')"><label class="col-dm-1 control-label"><i class="fa fa-minus-circle"></i></label></a>&nbsp;<a id="eda' + index + '" onclick="EditDynamicOption(' + index + ')"><label class="col-dm-1 control-label"><i class="fa fa-plus"></i></label></a></div></div></div>'
                             }
                             else { creatediv += '<div class="form-group" id="Edele' + index + '"><div class="row"><div class="col-lg-9"> <input type="text" maxlength="20" id="txtaddval' + index + '" value="' + arr[j] + '" class="form-control" disabled/></div><div class="class="col-lg-3"> <a  onclick="EditCloseOption(' + index + ')"><label class="col-dm-1 control-label"><i class="fa fa-minus-circle"></i></label></a></div></div></div>' }
                         }
                        
                         index++;
                     }
                     creatediv += '</div>';
                //     alert(creatediv);
                     $('#UpddynDivDDL' + value ).empty();
                     $('#UpddynDivDDL' + value).after(creatediv);
                 }
                 else if (UpdControlName == "Sql View")
                 {
                     var arr = arrData.split("*#");
                     var creatediv = ""; var index = 1;
                     for (var j = 0; j < arr.length; j++) {

                        
                         creatediv = '<div id="ddoptioncontainer"><div class="form-group col-md-4 col-sm-4" style="padding-left:0px"><label> View Name </label><input type="text" maxlength="30" id="txtaddval'  +value + '" class="form-control" value="' + arr[j] + '" disabled /></div>'

                     }
                     creatediv += '</div>';
                     //     alert(creatediv);
                     $('#UpddynDivDDL' + value).empty();
                     $('#UpddynDivDDL' + value).after(creatediv);
                 }
                 else if (UpdControlName =="Sql Procedure")

                 {
                     var arr = arrData.split("*#");
                     var creatediv = ""; var index = 1;
                    
                     creatediv = '<div id="ddoptioncontainer"><div class="form-group col-md-8 col-sm-8" style="padding-left:0px"><label>Enter Procedure Name :</label><input type="text" maxlength="30" id="txtaddval' + value + '" value="' + arr[0] + '" class="form-control"  disabled/></div><div class="form-group col-md-8 col-sm-8" style="padding-left:0px"><label for="exampleInputEmail1">Parameter Sorting No :</label><input type="text" maxlength="30" id="txtparameter"' + value + '"  placeholder="Enter Paramater Soting No. Like 1,2,3"  value="' + arr[1] + '" class="form-control" disabled/></div>';
                   
                     creatediv += '</div>';
                     //     alert(creatediv);
                     $('#UpddynDivDDL' + value).empty();
                     $('#UpddynDivDDL' + value).after(creatediv);
                     }
                 }
         }
         //////////////Permission
        
         function SetPermissions() {
          //   debugger;
             var pagename = location.pathname.substring(location.pathname.lastIndexOf("/") + 1)
             var username = '<%=Session["user_name"]%>';
             $.ajax({
                 type: "POST",
                 url: '<%= ResolveUrl("CRMDBService.asmx/SetPagePermission") %>',
                 contentType: "application/json; charset=utf-8",
                 data: '{Page: "' + pagename + '",User:"' + username + '"}',
                 dataType: "json",
                 async: false,
                 success: function (data) {
                     jsdata1 = JSON.parse(data.d);
                     $.each(jsdata1, function (key1, value1) {
                         myarray.push(value1.EditP, value1.DeleteP, value1.AddP);
                        // console.log(myarray["0"]);
                       //  console.log(myarray["1"]);
                        
                     });

                  //   console.log(myarray);
                   //  AddP = myarray["2"];
                   //  EditP = myarray["1"];
                    // DelP = myarray["0"];
                 }
                

             });
             debugger;
             
             //console.log(myarray);
             //console.log(myarray["0"]);
             //console.log(myarray[1]);
         // console.log(AddP);
          // return myarray;
         }





         //////////////////////Permission
         function GetData() {
              SetPermissions();
              //alert(myarray["1"]);
              //alert(myarray["2"]);
              //alert(myarray["0"]);
             var EditP = "";
             var DelP = "";
             debugger;
             console.log(myarray["0"]);
             if (myarray["0"] == "true") {

                 EditP = "enabled";
             }
             else {
                 EditP = "style='pointer-events:none;cursor:not-allowed'";
             }
             if (myarray["1"] == "true") {

                 DelP = "";
             }
             else {

                 DelP = "style='pointer-events:none;cursor:not-allowed'";
             }
             if (myarray["2"] == "true") {

                 $("#btnadd").addClass("btnEnable");
                 
                
             }
             else {

                 $("#btnadd").addClass("btnDisable");
             }
             $.ajax({
                 type: "POST",
                 url: '<%= ResolveUrl("CRMDBService.asmx/GetCustomFieldsData") %>',
                 contentType: "application/json; charset=utf-8",
                 data: '{ }',
                 dataType: "json",
                 success: function (data) {
                     jsdata1 = JSON.parse(data.d);
                   
                     var output = "<ul class='list-group'>";
                     $.each(jsdata1, function (key1, value1) {
                         //<a id='deleteOptions" + value1.Custom_Id + "'    data-target='#demo" + value1.Custom_Id + "' data-toggle='collapse'  onclick='return btnDeleteevent(" + value1.Custom_Id + ");' " + DelP + "><label class='col-dm-1 remove control-label'><i class='fa fa-fw fa-trash-o'></i></label></a>
                       //  alert('xx');
                         output += "<li class='clearfix list-group-item '>" + value1.CFdata + "<a id='editOptions" + value1.Custom_Id + "'  data-target='#demo" + value1.Custom_Id + "' data-toggle='collapse' onclick='return editOption(" + value1.Custom_Id + ");' " + EditP + "><label class='col-dm-1 control-label'><i class='fa fa-fw fa-pencil'></i></label></a>&nbsp;&nbsp;&nbsp;&nbsp;<label>" + value1.Status + "</label> <div id='demo" + value1.Custom_Id + "' class='collapse'></div></li>";
                     });
                     output += "</ul>";
                     $('#showdata').html(output);
                 }
             });
         }
         function GetDynamicControls(value) {
            
             debugger;
             return ' <div class="form-group"><label for="exampleInputEmail1">Table:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><select id="dynddl' + value + '" disabled="true" class="form-control select2" name="dynddl' + value + '" ><option  >Contact</option></select></div><div class="form-group"><label for="exampleInputEmail1">Field Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><input id="dynfname' + value + '" class="form-control" placeholder="Enter Field Name" name = "dynfname' + value + '" type="text" maxlength="20" disabled/></div><div class="form-group"><label for="exampleInputEmail1">Field Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><select id="dynftype' + value + '" disabled="true" class="form-control select2" name="dynftype' + value + '" ><option  >Single Line Text</option><option  >Multiple Line Text</option><option  >Number</option><option  >Dropdown</option><option  >Date</option><option  >Checkbox</option><option>Sql View</option><option>Sql Procedure</option></select></div><div id="UpddynDivDDL' + value + '" ></div><div class="clearfix" ></div> <div class="form-group col-md-4 col-sm-4"><label for="exampleInputEmail1">Active:</label><input id="chkIsActive' + value + '" type="checkbox" onchange="UpdateActive(' + value + ')"  checked="checked" class="checkbox" tabindex="48" /></div><div class="clearfix" ></div>  <div class="box-footer"><input type="button" class="btn btn-primary" onclick="btnUpdateevent(' + value + ')"  value="Update"  style="display:none"  /> &nbsp;&nbsp;<input type="button" class="btn btn-primary" onclick="Updclickcancel(' + value + ');" value="Cancel"  style="display:none" /></div>'
         }
         // add Custom Fields 
         $("#btnadd").click(function () {
             $("#adddata").show();
             var li = $("<li><div class='col-lg-5 col-md-6 col-sm-8 col-xs-10'>");
             li.html(CreateDynamicDivControls());
             $("#adddata").append(li);
             $("#btnadd").hide();
         });
        function clickcancel() {
            $("#btnadd").show(); $("#adddata").empty();
        };
        function Updclickcancel(valueId) {
            $("#demo" + valueId).empty();
        }
        function CreateOptionsVal() {
          //  alert("1");
            $("#dynDivDDL").empty();
            var type = $("#dynftype option:selected").val();
            if (type == "Dropdown" || type == "Checkbox") {
                var li = $("<div class='col-md-4 col-sm-4' id='ddoptioncontainer'>");
                li.html(CreateControlValues(type));
                $("#dynDivDDL").append(li);
            }
            //alert(type);
            if (type == "Sql View") {
              //  alert("121");
                var li = $("<div class='col-md-10 col-sm-10' id='ddoptioncontainer'>");
               // alert("type == SqlView");
                li.html(CreateSqlviewValues("Dropdown"));
               // alert("type == SqlView2");
                $("#dynDivDDL").append(li);
               // alert("type == SqlView2");
            }
          //  alert("1441");
            if (type == "Sql Procedure") {

                var li = $("<div class='col-md-10 col-sm-10' id='ddoptioncontainer'>");
               // alert("type == Procedure");
                li.html(CreateSqlProcedureValues("Dropdown"));
              //  alert("type == SqlView2");
                $("#dynDivDDL").append(li);
               // alert("type == SqlView2");
            }
        }
        function CreateControlValues(ControlName) {
            return '<div class="form-group" id="ele0"><label>Add ' + ControlName + '  Values </label> </div> <div class="form-group" id="ele1"><div class="row"><div class="col-lg-9"><input type="text" maxlength="20" id="txtaddval1" class="form-control"/></div><div class="col-lg-3"> <a id="a1" onclick="addOption(1)"><label class="col-dm-1 control-label"><i class="fa fa-plus"></i></label></a></div></div>'
        }
        function CreateSqlviewValues(ControlName) {
           
           // return '<div class="form-group" id="ele0"><label>Enter ' + ControlName + '  View Name </label> </div><div class="form-group" id="ele1"><div class="row"><div class="col-lg-9"><input type="text" maxlength="20" id="txtaddval1" class="form-control"/></div><div class="col-lg-3"> <a id="a1" onclick="addOption(1)"><label class="col-dm-1 control-label"><i class="fa fa-plus"></i></label></a></div></div>'
            return '<div class="form-group col-md-4 col-sm-4" style="padding-left:0px"><label>Enter View Name </label><input type="text" maxlength="30" id="txtaddval1" class="form-control"/></div>'
        }
        function CreateSqlProcedureValues(ControlName) {

           // return '<div class="row"><div class="col-md-4" id="ele0"><label>Enter ' + ControlName + '  Procedure Name </label> </div> <div class="row"><div class="col-lg-9"><input type="text" maxlength="30" id="txtaddval1" class="form-control"/></div><div class="col-lg-3"></div><label>Enter ' + ControlName + '  Filter Control Sorting No </label> </div> <div class="form-group" id="ele1"><div class="row"><div class="col-lg-9"><input type="text" maxlength="20" id="txtfiltersortingno" class="form-control"/></div><div class="col-lg-3"></div></div'
            return ' <div class="form-group col-md-4 col-sm-4" style="padding-left:0px"><label>Enter Procedure Name : </label><input type="text" maxlength="30" id="txtaddval1" class="form-control"/></div><div class="form-group col-md-4 col-sm-4"><label for="exampleInputEmail1">Parameter Sorting No :</label>&nbsp;&nbsp;<input type="text" maxlength="30" id="txtparameter"  placeholder="Enter Paramater Soting No. Like 1,2,3" class="form-control"/></div>'
        }
        function addOption(ind) {
            var id = $("#ddoptioncontainer > div").length;
            var html = '<div class="form-group" id="ele' + id + '"> <div class="row"><div class="col-lg-9"><input type="text" maxlength="20" id="txtaddval' + id + '" class="form-control"/></div><div class="col-lg-3"><a  onclick="CloseOption(' + id + ')"><label class="col-dm-1 control-label"><i class="fa fa-minus-circle"></i></label></a> &nbsp;&nbsp;<a id="a' + id + '" onclick="addOption(' + id + ')"><label class="col-dm-1 control-label"><i class="fa fa-plus"></i></label></a></div></div>';
            $("#a" + ind).hide();
            $("#ele" + ind).after(html);
        }
        function EditDynamicOption(edval) {
            var id = $("#ddoptioncontainer > div").length;
            $("#eda" + edval).hide();
            var html = '<div class="form-group" id="Edele' + id + '"> <div class="row"><div class="col-lg-9"><input type="text" maxlength="20" id="txtaddval' + id + '" class="form-control"/></div><div  style="padding-left: 1px;" class="col-lg-3"><a  onclick="EditCloseOption(' + id + ')"><label class="col-dm-1 control-label"><i class="fa fa-minus-circle"></i></label></a> &nbsp;&nbsp;<a id="eda' + id + '" onclick="EditDynamicOption(' + id + ')"><label class="col-dm-1 control-label"><i class="fa fa-plus"></i></label></a></div></div>';
         //   $("#a" + ind).hide();
            $("#Edele" + edval).after(html);
        }
        function CloseOption(N) {
            $("#ele" + N).remove();
        }
        function EditCloseOption(N) {
            $("#Edele" + N).remove();
        }
        function CreateControlValues2() {
        
            return '<div class="form-group col-md-4 col-sm-4"><input type="text" maxlength="20" id="txtaddval2" class="form-control"/> <a onclick="CreateControlValues2()"><label class="col-dm-1 control-label"><i class="fa fa-fw fa-pencil"></i></label></a></div></div>'
        }

         // Create main div for entry
         function CreateDynamicDivControls() {
             return '<div class="form-group col-md-2 col-sm-4"><label for="exampleInputEmail1">Table:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><select id="dynddl" class="form-control  select2" name="dynddl" ><option  >Contact</option></select></div><div class="form-group col-md-2 col-sm-4"><label for="exampleInputEmail1">Sorting Order:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><input id="dynsort" class="form-control numeric text-left" placeholder="Enter Sorting Order" name = "dynsort" type="number" min="0" max="9" maxlength="5" /></div><div class="form-group col-md-4 col-sm-4"><label for="exampleInputEmail1">Field Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><input id="dynfname" class="form-control" placeholder="Enter Field Name" name = "dynfname" type="text" maxlength="20" /></div><div class="form-group col-md-4 col-sm-4"><label for="exampleInputEmail1">Field Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><select id="dynftype" class="form-control  select2" name="dynftype" onchange="CreateOptionsVal();"><option  >Single Line Text</option><option  >Multiple Line Text</option><option  >Number</option><option  >Dropdown</option><option  >Date</option><option  >Checkbox</option><option  >Sql View</option><option  >Sql Procedure</option></select></div><div id="dynDivDDL" ></div><div class="clearfix" ></div> <div class="form-group col-md-4 col-sm-4"><label for="exampleInputEmail1">Active:</label><input id="chkIsActive"  type="checkbox" checked="checked" class="checkbox" tabindex="48" /></div><div class="clearfix" ></div> <div class="box-footer"><input type="button" class="btn btn-primary" value="Save" onclick="savenew();"/>  &nbsp;&nbsp;<input type="button" class="btn btn-primary" value="Cancel" onclick="clickcancel()"/></div></li>'
         }
         function savenew() {

             if ($('#dynfname').val() == "") {
                 errormessage("Please Enter Field Name");
                 return false;
             }
             if ($('#dynsort').val() == "") {
                 errormessage("Please Enter Sorting Order");
                 return false;
             }
             if ($('#txtfiltersortingno').val() == "") {
                 errormessage("Please Enter filter  Sorting No.");
                 return false;
             }
             if ($("#dynftype option:selected").text() == "Dropdown" || $("#dynftype option:selected").text() == "Checkbox") {
                 if ($("#txtaddval1").val()=="") {
                     errormessage("Please Enter one value for " + $("#dynftype option:selected").text() + "");
                     return false;
                 }
             }
             var data = "";
             var active = "";
             debugger;
             if ($('#chkIsActive').is(":checked"))
             {
                 active = true;
             }
             else
             {
                 active = false;

             }
             //for (var i = 1; i < $("#ddoptioncontainer > div").length; i++) {
             //    data += $("#txtaddval" + i).val() + "*#";
                
             //}
             $("#ddoptioncontainer :text").each(function () {
                 data += $(this).val() + "*#";
             });
             data = data.substr(0, data.length - 2);
           
             $.ajax({
                 type: "POST",
                 //txtfiltersortingno
                 url: '<%= ResolveUrl("~/CRMDBService.asmx/SaveCustomFieldsData") %>',
                     contentType: "application/json; charset=utf-8",
                     data: '{Table: "' + $("#dynddl option:selected").text() + '",Fieldname: "' + $("#dynfname").val() + '",Fieldtype: "' + $("#dynftype option:selected").text() + '",Data: "' + data + '",Sort: "' + $("#dynsort").val() + '",filterctrlno: "' + $("#txtfiltersortingno").val() + '",parametername: "' + $("#txtparameter").val() + '",Active: "' + active +  '" }',
                     dataType: "json",
                     success: function (data) {
                         var Message = JSON.stringify(data.d);

                         if (Message == -1) {
                             errormessage("This Record Already Exists.");
                         }
                         else { Successmessage("Record Saved Successfully"); GetData(); clickcancel(); }
                     }
                 });

             }
         function btnUpdateevent(val) {
            
             if ($('#dynfname' + val).val() == "") {
                 errormessage("Please enter Field Name");
                 return false;
             } 
             if ($("#dynftype" + val + " option:selected").text() == "Dropdown" || $("#dynftype" + val + " option:selected").text() == "Checkbox") {
                 if ($("#txtaddval1").val() == "") {
                     errormessage("Please enter one value for " + $("#dynftype" + val + " option:selected").text() + "");
                     return false;
                 }
            }
             var data = "";
             //for (var i = 1; i < $("#ddoptioncontainer > div").length; i++) {
             //    alert($("#ddoptioncontainer > div").length);
             //    data += $("#txtaddval" + i).val() + "*#";
             //    alert($("#txtaddval" + i).val());
             //}
             $("#ddoptioncontainer :text").each(function () {
                 data += $(this).val() + "*#";
             });
            
             data = data.substr(0, data.length - 2);
            // $('#dynddl' + value)
      
                 $.ajax({
                     type: "POST",
                     url: '<%= ResolveUrl("~/CRMDBService.asmx/UpdateCustomFields") %>',
                     contentType: "application/json; charset=utf-8",
                     data: '{Table: "' + $("#dynddl" + val + " option:selected").text() + '",Fieldname: "' + $('#dynfname' + val).val() + '",Data: "' + data + '",Id: "' + val + '" }',

                     dataType: "json",
                     success: function (data) {
                         var Message = JSON.stringify(data.d);

                         if (Message == 1) {
                             errormessage("This Record Already Exists.");
                         }
                         else { Successmessage("Record Updated Successfully"); GetData(); }
                     }
                 });
         }
         function UpdateActive(val)
         {
             var active = false;
             debugger;
             if ($('#chkIsActive' + val).is(":checked"))
             {
                 active = true;
             }
             else {active = false;   }

             $.ajax({
                 type: "POST",
                 url: '<%= ResolveUrl("~/CRMDBService.asmx/UpdateCustomFieldsActive") %>',
                 contentType: "application/json; charset=utf-8",
                 data: '{active: "' + active + '",Id: "' + val + '" }',

                 dataType: "json",
                 success: function (data) {
                     var Message = JSON.stringify(data.d);

                     jsdata1 = JSON.parse(data.d);
                     var output = "<ul class='list-group'>";
                     $.each(jsdata1, function (key1, value1) {
                         $('#LblActive' + val).text(value1.Status)
                     });
                     if (Message == 1) {
                         errormessage("This Record Already Exists.");
                     }
                     else { Successmessage("Record Updated Successfully"); GetData(); }
                 }
             });


         }
         function btnDeleteevent(val) {
             if (confirm("Deleting this field will remove all its data from respective tables.Are you sure to delete?")) {
                 $.ajax({
                     type: "POST",
                     url: '<%= ResolveUrl("~/CRMDBService.asmx/DeleteCustomFieldsById") %>',
                 contentType: "application/json; charset=utf-8",
                 data: '{Id: "' + val + '" }',
                 dataType: "json",
                 success: function (data) {
                     var Message = JSON.stringify(data.d);
                     GetData();
                 }
                  });
             }
             return false;
             }
        
    </script>
</asp:Content>
