<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="CRMContact.aspx.cs" Inherits="AstralFFMS.CRMContact" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <%--    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.0/themes/base/jquery-ui.css">

      <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
     <script src="https://code.jquery.com/ui/1.12.0/jquery-ui.js"></script>--%>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js"></script>


    <style type="text/css">
        .autoComplete


        {

        width:auto;height:250px; 
        }


    .disabled
     {
         text-decoration:line-through;
     }
    .modalBackground
    {
        background-color: Black;
        filter: alpha(opacity=60);
        opacity: 0.6;
    }
    .modalPopup
    {
        background-color: #FFFFFF;
        width: 300px;
            border: 1px solid #aaaaaa !important;
        border-radius: 12px;
        padding:0;
            overflow-y: hidden !important;
      
    }
    .modalPopup .header
    {
            border: 1px solid #aaaaaa;
    background: #cccccc url(img/ui-bg_highlight-soft_75_cccccc_1x100.png) 50% 50% repeat-x;
    color: black !important;
    font-weight: bold;
      

        height: 35px;
        color: White;
        line-height: 30px;
        text-align: center;
           margin-right: 10px;
        border-top-left-radius: 6px;
        border-top-right-radius: 6px;
    }
    .modalPopup .body
    {
        min-height: 50px;
        line-height: 30px;
        text-align: center;
        /*font-weight: bold;*/
    }
    .modalPopup .footer
    {
          margin-right: 10px;
    }
    .modalPopup .modelbtn
    {
       
        color: White;
        line-height: 23px;
        text-align: center;
        font-weight: bold;
        cursor: pointer;
        border-radius: 4px;
            border: 1px solid #d3d3d3 !important;
    background: #e6e6e6 url(img/ui-bg_glass_75_e6e6e6_1x400.png) 50% 50% repeat-x;
    font-weight: normal;
    color: #555555;
    }
   
</style>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .tagWidth {
            width: 100% !important;
            height: 55px !important;
        }

        .ShortDesc {
            display: block;
            width: 150px !important;
            overflow: hidden;
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
          .modalBackground {
         background-color: Gray;
         filter: alpha(opacity=80);
         opacity: 0.8;
         z-index: 1000;
     }

    </style>
    <script type="text/javascript">
        function DoNav(Contact_Id) {

            if (Contact_Id != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', Contact_Id)
            }
        }
    </script>
    <script type="text/javascript">
        $(function () {
            $('[id*=ListBox1]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '200px',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
        function resetPosition1(object, args) {


            var tb = object._element; // tb.id is the associated textbox ID in the grid.

            // Get the position with scrolling. 
            var tbposition = [tb.offsetLeft + tb.offsetParent.offsetLeft, tb.offsetTop + tb.offsetParent.offsetTop];


            var xposition = tbposition[0] + 85 // -40 lined it up horizontally with the textbox in my circumstance


            var yposition = tbposition[1] + 170; // 22 = textbox height + no spacing

            var ex = object._completionListElement;
            if (ex)

                $common.setLocation(ex, new Sys.UI.Point(xposition, yposition));
        }
        function resetPosition(object, args) {


            var tb = object._element; // tb.id is the associated textbox ID in the grid.

            // Get the position with scrolling. 
            var tbposition = [tb.offsetLeft + tb.offsetParent.offsetLeft, tb.offsetTop + tb.offsetParent.offsetTop];


            var xposition = tbposition[0] + 75 // -40 lined it up horizontally with the textbox in my circumstance


            var yposition = tbposition[1] + 105 ; // 22 = textbox height + no spacing

            var ex = object._completionListElement;
            if (ex)

                $common.setLocation(ex, new Sys.UI.Point(xposition, yposition));
        }


        $(document).ready(
      function () {

          if ($('#<%=Hidemailvalues1.ClientID%>').val() != "") {
           
              var k = "";
              var tmp = null;
             <%-- $.ajax({
                  type: "POST",
                  url: '<%= ResolveUrl("CRMDBService.asmx/GetContactsType") %>',
                  contentType: "application/json; charset=utf-8",
                  data: '{Value: "Email"}',
                  dataType: "json",
                  success: function (data) {
                      jsdata1 = JSON.parse(data.d);
                      $.each(jsdata1, function (key1, value1) {
                          //////alert(jsdata1);
                          select11 += value1.Data + ",";
                          //////alert(select11);
                          //////alert("2");
                          k += '<option  value="' + value1.Data + '">"' + value1.Data + '"</option>';



                      });
                       //alert(k);
                      //   $('.Input').append(k);
                     // console.log(k + 'after loop');
                      tmp = k;
                  }

              });--%>
              // console.log(tmp + 'after ajax');.Value
              // ////alert($('#<%=Hidemailvalues1.ClientID%>').val());
              var email = $('#<%=Hidemailvalues1.ClientID%>').val();//email address
              var emailsplit = email.split(",");
              var emailddl = $('#<%=Hidemailddlvalues1.ClientID%>').val();
              var emailddlsplit = emailddl.split(",");
              var contact = $('#<%=HidEmailContName1.ClientID%>').val();
              var allemailtype = ($('#<%=Hidallemailddlvalues1.ClientID%>').val()).split(",");
              var contactsplit = contact.split(",");

              var emailId =$('#<%=HidEmailId1.ClientID%>').val().split(",")
            
              for (var i = 0; i < emailddlsplit.length; i++) {
              
                  var val1 = $('.et').length + 1;
                  //var select11 = "";
                  //var options = '';

                  //alert(contactsplit[i]);
                  //var select = ["", ""];
                  var html1 = '<div class="form-group et" id="divET' + val1 + '"> ';
                  html1 +='<div class="row"><div class="col-md-4 col-sm-4 col-xs-12">';
                  html1 +='<div class="form-group"><label for="exampleInputEmail1">Contact Name:</label>';
                  html1 +=' <input type="text" value="' + contactsplit[i] + '"  class="form-control EmailContact" maxalength="15" id="txtEmailcontact" placeholder="Enter Contact Name" tabindex="5" />';
                  html1 +='</div></div><div class="col-md-4 col-sm-4 col-xs-12"> <div class="form-group"><label for="exampleInputEmail1">Email:</label>';
                  html1 +=' <input type="text" value="' + emailsplit[i] + '"  class="form-control emailtext" maxlength="50" id="Email' + val1 + '" placeholder="Enter Email" tabindex="5" />';
                  html1 += ' <input type="hidden" class="hidEmailid"  value=' + emailId[i] + ' />';
                  html1 += '</div></div>';
                  //html1 += '<div class="col-md-4 col-sm-4 col-xs-12">';
                  //html1 += '<div class="form-group"><label for="exampleInputEmail1">Email:</label>';
                  //html1 += ' <input name="Email1" type="text" id="Email1" class="form-control emailtext" maxlength="50" placeholder="Enter Email" tabindex="5" onchange="ValidMail(this);">';
                  //html1 += ' </div>  </div>'
                      
                  //    <label for="exampleInputEmail1">Contact Name:</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;';
                  //html1 += '<label for="exampleInputEmail1">Email:</label><div class="clearfix"></div>';               
                  //html1 += '<div class="col-md-4 col-sm-4 col-xs-4" style="padding-left: 0"> <input type="text" value="' + contactsplit[i] + '"  class="form-control EmailContact" maxalength="15" id="txtEmailcontact" placeholder="Enter Contact Name" tabindex="5" /></div>'
                  //html1 += '<div class="col-md-4 col-sm-4 col-xs-4 paddingleft0"> <input type="text" value="' + emailsplit[i] + '"  class="form-control emailtext" maxlength="50" id="Email' + val1 + '" placeholder="Enter Email" tabindex="5" /></div>';            



                  html1 += '<div class="col-lg-8 col-md-3 col-xs-12" ><div class="row"><div class="col-lg-8"><div class="form-group"><label for="exampleInputEmail1">Type:</label><select id="ddlemailtype' + val1 + '" class="form-control emailddl" TabIndex="7">';
                 // html1 += '<select id="ddlphonetype' + val2 + '" class="form-control phoneddl" TabIndex="7">'
                 
                  for (var k = 0; k < allemailtype.length; k++)
                  {
                      debugger;
                   //   for (var m = 0; m < emailddlsplit.length; m++) {
                          if (allemailtype[k] == emailddlsplit[i])
                          {
                              html1 += '<option value="' + allemailtype[k] + '" selected>' + allemailtype[k] + '</option>';
                          }
                          else {
                              html1 += '<option value="' + allemailtype[k] + '">' + allemailtype[k] + '</option>';
                          }
                 
                     // }
                  }
                  html1 += '</select></div> </div><div  style="padding-left: 13px;margin-top: 26px; ">';
                  html1 += '<img id="imgaddphone" src="img/plus.png" alt="Add" onclick="CreateEmail();"> <img src="img/minus.png" alt="Remove" onclick="RemoveEmail(' + val1 + ',' + emailId[i] + ');" /></div></div></div></div>';
              
                  $("#divET" + $('.et').length).after(html1);
              } 
              //alert(html1);
          }
          if ($('#<%=Hidphonevalues1.ClientID%>').val() != "") {
             // alert("l");
              // ////alert($('#<%=Hidphonevalues1.ClientID%>').val());
              var Phone = $('#<%=Hidphonevalues1.ClientID%>').val();
              var phonesplit = Phone.split(",");
              var phoneddl = $('#<%=Hidphoneddlvalues1.ClientID%>').val();
              var phoneddlsplit = phoneddl.split(",");
              var contact = $('#<%=HidPhoneContName1.ClientID%>').val();
              var contactesplit = contact.split(",");
              var allphonetype = ($('#<%=Hidphoneallddlvalues1.ClientID%>').val()).split(",");
              var email = $('#<%=Hidemailvalues1.ClientID%>').val();//email address
              var emailsplit = email.split(",");

              var phoneId = $('#<%=HidPhoneId1.ClientID%>').val().split(",")
              for (var i = 0; i < phoneddlsplit.length; i++) {
                  var val2 = $('.pt').length + 1;
                  var select1 = ["work", "mobile", "home", "direct", "fax", "skype",
                                  "other"];
                  var select = ["", "", "", "", "", "",
                                  ""];
                  for (var k = 0; k < select1.length; k++) {
                      // ////alert(select1[k]+" select1");
                      //////alert(urlddlsplit[i] + " url");
                      if (select1[k] == phoneddlsplit[i]) {
                          // ////alert("success");
                          //select1[k] = +select1[k] + " " + "Selected"
                          var selectenter = "Selected";
                          //////alert(select1[k]);
                          Array.prototype.splice.apply(select, [k, selectenter.length].concat(selectenter));
                          //////alert(select)
                      }


                  }
                  var html = '<div class="form-group panel panel-default pt" id="divPT' + val2 + '"  style="padding-left: 10px;">';

                  html += '<div class="row"> <div class="col-md-6 col-sm-6 col-xs-12" ><div class="form-group">';
                  html += '<label for="exampleInputEmail1">Contact Name:</label>';
                  html += '<input type="text" value="' + contactesplit[i] + '"  class="form-control PhoneContact" maxalength="15" id="txtaddcontact" placeholder="Enter Contact Name" tabindex="5" onchange="Validphone(This);" /></div></div>';

                  html += '<div class="col-md-5 col-sm-5 col-xs-12"><div class="form-group"><label for="exampleInputEmail1">Job Title:</label><select id="ddlphonetype' + val2 + '" class="form-control phoneddl" TabIndex="7">';
                      //  //alert(allphonetype);
                      //alert(contactesplit[i]);
                  for (var k = 0; k < allphonetype.length; k++) {
                      if (allphonetype[k] == phoneddlsplit[i]) {
                          html += '<option value="' + allphonetype[k] + '" selected>' + allphonetype[k] + '</option>';
                      }
                      else {
                          html += '<option value="' + allphonetype[k] + '">' + allphonetype[k] + '</option>';
                      }  
                  }
                  // //alert(html);
                  html += '</select></div></div>';

                  html += '<div class="col-md-5 col-sm-5 col-xs-12 " ><div class="form-group">';
                  html += '<label for="exampleInputEmail1">Phone:</label>';
                  html += '<input type="text" value="' + phonesplit[i] + '"  class="form-control numeric text-left phonetext " maxlength="15" id="Phone' + val2 + '" placeholder="Enter Phone" tabindex="5" />';
                  html += '<input type="hidden" class="hidphoneid"  value=' + phoneId[i] + ' />'
                  html += '</div></div>';

                 

                  //html +='<label for="exampleInputEmail1">Contact Name:</label>';
                  //html +='<label for="exampleInputEmail1">Phone:</label>';
                  //html +='<div class="clearfix"></div><div class="col-md-4 col-sm-4 col-xs-4" style="padding-left: 0">';
                  //html +='<input type="text" value="' + contactesplit[i] + '"  class="form-control PhoneContact" maxalength="15" id="txtaddcontact" placeholder="Enter Contact Name" tabindex="5" />';
                  //html +='</div><div class="col-md-4 col-sm-4 col-xs-4 paddingleft0">';
                  //html +='<input type="text" value=' + phonesplit[i] + '  class="form-control numeric text-left phonetext " maxlength="15" id="Phone' + val2 + '" placeholder="Enter Phone" tabindex="5" /></div>';


               

                  html += '<div class="col-lg-6 col-md-6 col-xs-12"  ><div class="row">';
                  
                  html += '<div class="col-md-10 col-sm-10 col-xs-12">';
                  html += '<div class="form-group"><label for="exampleInputEmail1">Email:</label>';
                  html += ' <input name="Email' + val2 + '" value="' + emailsplit[i] + '" type="text" id="Email' + val1 + '" class="form-control emailtext" maxlength="50" placeholder="Enter Email" tabindex="5" onchange="ValidMail(this);">';
                  html += ' </div>  </div>'
                  html += ' <div  style="padding-left: 13px;margin-top: 26px; ">';
                  html += '<img id="imgaddphone" src="img/plus.png" alt="Add" onclick="CreatePhone();"> <img src="img/minus.png" alt="Remove" onclick="RemovePhone(' + val2 + ',' + phoneId[i] + ');" /></div></div></div></div> <script> $(".numeric").numeric() ';
                  //alert(html);
                  $("#divPT" + $('.pt').length).after(html);
              }
          }
          if ($('#<%=HidUrlvalues1.ClientID%>').val() != "") {
             
              //  ////alert($('#<%=HidUrlvalues1.ClientID%>').val());
             
              var AllUrlType = ($('#<%=Hidallurltype.ClientID%>').val()).split(",");
              var Url = $('#<%=HidUrlvalues1.ClientID%>').val();//web address
              var Urlddl = $('#<%=HidUrlddlvalues1.ClientID%>').val();//dropdown selected values
              var urlddlsplit = Urlddl.split(",");
              var Urlsplit = Url.split(",");

              var UrlId = ($('#<%=HidUrlId1.ClientID%>').val()).split(",");
              for (var i = 0; i < urlddlsplit.length; i++) {
                  var val = $('.ut').length + 1;
                  var select1 = ["Website", "Blog", "Twitter", "LinkedIn", "Facebook", "Google",
                                   "Other"];
                  var select = ["", "", "", "", "", "",
                                  ""];
                  // ////alert(select1[0]);
                  //////alert(select1.length);
                  for (var k = 0; k < select1.length; k++) {
                      // ////alert(select1[k]+" select1");
                      //////alert(urlddlsplit[i] + " url");
                      if (select1[k] == urlddlsplit[i]) {
                          // ////alert("success");
                          //select1[k] = +select1[k] + " " + "Selected"
                          var selectenter = "Selected";
                          //////alert(select1[k]);
                          Array.prototype.splice.apply(select, [k, selectenter.length].concat(selectenter));
                          // ////alert(select)
                      }


                  }
                  
                  var html = '<div class="form-group ut" id="divUT' + val + '"><div class="row"><div class="col-md-8 col-sm-5 col-xs-12"><div class="form-group"><label for="exampleInputEmail1">URL:</label><input type="text"  value=' + Urlsplit[i] + ' class="form-control urltext" maxlength="50" id="Url' + val + '" placeholder="Enter URL" tabindex="5" /><input type="hidden" class="hidurlid"  value=' + UrlId[i] + ' /></div></div>';
                  html += '<div class="col-lg-4 col-md-3 col-xs-12" ><div class="row"><div class="col-lg-8" style="display:none"><div class="form-group"><label for="exampleInputEmail1">Type:</label><select id="ddlurltype' + val + '" class="form-control urlddl" TabIndex="7">'
                  for (var k = 0; k < AllUrlType.length; k++) {
                      if (AllUrlType[k] == urlddlsplit[i]) {
                          html += '<option value="' + AllUrlType[k] + '" selected>' + AllUrlType[k] + '</option>';
                      }
                      else {
                          html += '<option value="' + AllUrlType[k] + '">' + AllUrlType[k] + '</option>';
                      }  
                  }
                  html += '</select></div></div> <div  style="padding-left:13px ;margin-top: 26px;"><div class="form-group"> <img id="imgaddphone" src="img/plus.png" alt="Add" onclick="CreateUrl();"><img src="img/minus.png" alt="Remove" onclick="RemoveUrl(' + val + ',' + UrlId[i] + ');" /> </div></div> </div></div>  </div></div>';
                  //////alert(html);
                  $("#divUT" + $('.ut').length).after(html);
              }
          }
        
          if ($('#<%=hidchk.ClientID%>').val() != "") {
              //////var img = document.getElementById('imgaddphone');
              //////img.style.visibility = "hidden";
              //////var img = document.getElementById('imgaddEmail');
              //////img.style.visibility = "hidden";
              //////var img = document.getElementById('imgaddurl');
              //////img.style.visibility = "hidden";
              // ////alert($('#<%=hidchk.ClientID%>').val());
              testingabhi();
          }
          else {
              // $('#spinner').show();
           
              GetCustomFields();
              // ////alert("GetCustomFields");
              //  $('#spinner').hide();
          }

      });
    </script>
    <script type="text/javascript">
        function FillAddress()
        {
            debugger;
            if ($('#ContentPlaceHolder1_chkaddress').is(":checked")) {
                $('#<%=Address.ClientID%>').val($('#<%=txtcompadd.ClientID%>').val());
                $('#<%=City.ClientID%>').val($('#<%=txtcompcity.ClientID%>').val());
                $('#<%=State.ClientID%>').val($('#<%=txtcompstate.ClientID%>').val());
                $('#<%=ddlcountry.ClientID%>').val($('#<%=ddlcompcountry.ClientID%>').val());
                $('#<%=Zip.ClientID%>').val($('#<%=txtcompzip.ClientID%>').val());
                $('#<%=hidcontactcountry.ClientID%>').val($('#<%=ddlcompcountry.ClientID%>').val());
            }
            else
            {
                $('#<%=Address.ClientID%>').val('');
                $('#<%=City.ClientID%>').val('');
                $('#<%=State.ClientID%>').val('');
                $('#<%=ddlcountry.ClientID%>').val(0);
                $('#<%=Zip.ClientID%>').val('');
            }
            
        }
        function FillComapnydetail()
        {
            debugger;
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/getcompanydetail") %>',
                contentType: "application/json; charset=utf-8",
                data: '{companyname:"' + $('#<%=Company.ClientID%>').val() + '"}',
                dataType: "json",
                success: function (data) {
                
                    jsdata1 = JSON.parse(data.d);
                    debugger;
                    $.each(jsdata1, function (key1, value1) {
                        debugger;
                        $('#<%=ddlcompcountry.ClientID%>').val(value1.Country);
                        $("#<%=hidcompcountry.ClientID%>").val(value1.Country)
                       
                        $('#<%=txtcompadd.ClientID%>').val(value1.Address);
                        $('#<%=txtcompcity.ClientID%>').val(value1.City);
                      
                        $('#<%=txtcompstate.ClientID%>').val(value1.StateId)
                        $('#<%=txtcompzip.ClientID%>').val(value1.zip);
                   <%--     $('#<%=txtcompdesc.ClientID%>').val(value1.Description);--%>
                        $('#<%=hidcompanyid.ClientID%>').val(value1.Comp_Id);
                        popupvalidation();
                    });
                }

            });
        }

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
    <script>
        function isNumber(evt) {

            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (!(iKeyCode != 8)) {
                s
                e.preventDefault();
                return false;
            }
            return true;
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
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function fillfirstcontact() {

            //txtaddcontact  txtaddcontact2  txtaddcontact
            var name;
            var name2;
            if ($('#<%=Fname.ClientID%>').val() != "") {
                name = $('#<%=Fname.ClientID%>').val();
            }
          <%--  if ($('#<%=Lname.ClientID%>').val() != "") {
                name2 = $('#<%=Lname.ClientID%>').val();
            }--%>

            // ////alert(name + " " + name2);
            ($('#<%=txtaddcontact1.ClientID%>').val(name + " " + name2));
            ($('#<%=txtEmailcontact1.ClientID%>').val(name + " " + name2));


        }
        function popupvalidation() {



            //$('divcompanydetails').val();
            var myvar = document.getElementById("divcompanydetails").style.display;
            // //alert(myvar);
            if (myvar == "block") {

                document.getElementById("divcompanydetails").style.display = "none"
            }
            if (myvar == "none") {
                ////alert("565");
                document.getElementById("divcompanydetails").style.display = "block"
            }



        }
        function validatetask()
        {
            

            if ($('#<%=txtnxtactndt.ClientID%>').val() == "") {
                errormessage("Please select the Date");
                return false;
            }
            if ($('#<%=txtnxtaction.ClientID%>').val() == "") {
                errormessage("Please enter action Name");
                return false;
            }
            if ($('#<%=ddlowner.ClientID%>').val() == "0") {
                errormessage("Please select the Assigned to.");
                return false;
            }
        }
        function validate() {
          
            var sel = document.getElementById('<%= ddltag.ClientID %>');
            var optsLength = sel.options.length;
            var Tags = "";
            for (var i = 0; i < optsLength; i++) {
                if (sel.options[i].selected) {
                    Tags += sel.options[i].value + ",";
                }
            }
            Tags = Tags.substr(0, Tags.length - 1);
            $('#<%=hidtags.ClientID %>').val(Tags);

            //get phone values
            var parPT = $(".pt").parent().attr('class');
            var phnval = ""; var phnddlval = ""; var phnCont = ""; var phnid = "";
            $('.' + parPT + ' .phonetext').each(function () {
                phnval += $(this).val() + ",";
            })
            phnval = phnval.substr(0, phnval.length - 1);
            $('#<%=Hidphonevalues.ClientID %>').val(phnval);

            $('.' + parPT + ' .phoneddl').each(function () {
                phnddlval += $(this).val() + ",";
            })
            // ////alert(phnddlval);
            phnddlval = phnddlval.substr(0, phnddlval.length - 1);
            $('#<%=Hidphoneddlvalues.ClientID %>').val(phnddlval);
            $('.' + parPT + ' .PhoneContact').each(function () {
                phnCont += $(this).val() + ",";
            })
            phnCont = phnCont.substr(0, phnCont.length - 1);

            $('#<%=HidPhoneContNamehtml.ClientID %>').val(phnCont);

            $('.' + parPT + ' .hidphoneid').each(function () {
                phnid += $(this).val() + ",";
            })
            phnid = phnid.substr(0, phnid.length - 1);
            $('#<%=HidPhoneID.ClientID %>').val(phnid);
           // alert($('#<%=HidPhoneContNamehtml.ClientID %>').val());
            //get email values


            var parET = $(".et").parent().attr('class');
            var emailval = ""; var emailddlval = ""; var EmailContact1 = ""; var EmailId = "";
            ////var valid = "true";

            ////$('.' + parET + ' .emailtext').each(function () {
            ////    debugger;
            ////    var testEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
            ////    if (this.value != "") {
            ////        if (testEmail.test(this.value)) {
            ////        }
            ////        else {
            ////            errormessage("Please enter valid Email");
            ////            valid = "false";
            ////            this.value = "";
            ////            $(this).focus();
            ////            return false;
            ////        };
            ////    }
            ////});
            ////debugger;
            ////if (valid == "false")
            ////{
            ////    return false;
            ////}
            $('.' + parPT + ' .emailtext').each(function () {
                emailval += $(this).val() + ",";
            })
            emailval = emailval.substr(0, emailval.length - 1);
            $('#<%=Hidemailvalues.ClientID %>').val(emailval);

            $('.' + parET + ' .emailddl').each(function () {
                emailddlval += $(this).val() + ",";
            })
            emailddlval = emailddlval.substr(0, emailddlval.length - 1);
            $('#<%=Hidemailddlvalues.ClientID %>').val(emailddlval);
            $('.' + parET + ' .EmailContact').each(function () {
                EmailContact1 += $(this).val() + ",";
            })
            EmailContact1 = EmailContact1.substr(0, EmailContact1.length - 1);
            $('#<%=HidEmilCt.ClientID %>').val(EmailContact1);


            $('.' + parET + ' .hidEmailid').each(function () {
                EmailId += $(this).val() + ",";
            })
            EmailId = EmailId.substr(0, EmailId.length - 1);
            $('#<%=HidEmailId.ClientID %>').val(EmailId);
            
            //get url values
            var parUT = $(".ut").parent().attr('class');
            var urlval = ""; var urlddlval = ""; var urlid = "";
            $('.' + parUT + ' .urltext').each(function () {
                urlval += $(this).val() + ",";
            })
            urlval = urlval.substr(0, urlval.length - 1);
            $('#<%=HidUrlvalues.ClientID %>').val(urlval);

            $('.' + parUT + ' .urlddl').each(function () {
                urlddlval += $(this).val() + ",";
            })
            urlddlval = urlddlval.substr(0, urlddlval.length - 1);
            $('#<%=HidUrlddlvalues.ClientID %>').val(urlddlval);

            $('.' + parUT + ' .hidurlid').each(function () {
                urlid += $(this).val() + ",";
            })
            urlid = urlid.substr(0, urlid.length - 1);
            $('#<%=HidurlId.ClientID %>').val(urlid);

            

            var splhidchk = $('#<%=hidchkid.ClientID %>').val().split('@'); 
          
          <%--  var splhidsel = $('#<%=hidselid.ClientID %>').val().split('@'); 
            for (var k = 0; k < splhidsel.length; k++) {
              $('#'+ splhidsel[k] + ' :selected').text();
            }--%>
            var cval = "", Ischk = "0";
            $('.customfieldgroup').each(function () {
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
                            ////alert(dt);
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


            })

            cval = cval.substr(0, cval.length - 1);
            $('#<%=hidcustomval.ClientID %>').val(cval);
            if ($('#<%=Company.ClientID%>').val() == "") {
                errormessage("Please enter Company Name");
                return false;
            }
            if ($('#<%=Fname.ClientID%>').val() == "") {
                errormessage("Please enter First Name");
                return false;
            }
            var value = ($('#<%=Fname.ClientID%>').val().charAt(0));
            var chrcode = value.charCodeAt(0);
            if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                errormessage("Do not start First Name with special characters")
                return false;
            }
          <%--  if ($('#<%=Lname.ClientID%>').val() == "") {
                errormessage("Please enter Last Name");
                return false;
            }--%>
            var value = ($('#<%=Lname.ClientID%>').val().charAt(0));
            var chrcode = value.charCodeAt(0);
            if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                errormessage("Do not start Last Name with special characters")
                return false;
            }
         <%--   if ($('#<%=JobTitle.ClientID%>').val() == "") {
                errormessage("Please enter Job Title");
                return false;
            }--%>

            if ($('#<%=Phone1.ClientID%>').val() == "") {
                errormessage("Please enter Phone");
                return false;
            }
            if ($('#<%=City.ClientID%>').val() == "") {
                errormessage("Please enter City");
                return false;
            }
            if ($('#<%=State.ClientID%>').val() == "") {
                errormessage("Please enter State");
                return false;
            }
            if ($('#<%=ddlcountry.ClientID%>').val() == "0" || $('#<%=ddlstatus.ClientID%>').val() == "") {
                errormessage("Please Select Country");
                return false;
            }
           
            if ($('#<%=ddlstatus.ClientID%>').val() == "0" || $('#<%=ddlstatus.ClientID%>').val() == "") {
                errormessage("Please Select Status");
                return false;
            }
           
            var flag = false;

            if ($('#<%=btnSave.ClientID%>').val() == "Save")
            {
                var test = Checkduplicate();
              //  alert(test);
                if (test=="N")
                {
                    errormessage("Record already exist");
                    return false;
                }
<%--                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("CRMDBService.asmx/Checkduplicate") %>',
                    contentType: "application/json; charset=utf-8",
                    data: '{firstname:"' + $('#<%=Fname.ClientID%>').val() + '",lastname:"' + $('#<%=Lname.ClientID%>').val() + '",company:"' + $('#<%=Company.ClientID%>').val() + '"}',
                    dataType: "json",
                    asyn:false,
                    success: function (data) {
                        var msgval = JSON.parse(data.d);
                        console.log(msgval);
                        if (msgval.SetMsg == "N") {
                            alert(msgval.SetMsg);
                            flag = "N";
                            // if (flag == "N") {
                            // errormessage("Record already exist");
                            //   errormessage("Record already exist for " + Fname.Value.Replace("'", "''") + Lname.Value.Replace("'", "''") + " ( " + Company.Value + " )'");
                           // return false;
                            //}

                        } else {
                            alert('dd')
                            return true;
                        }
                    }
                });--%>
               
            }
       //   alert('ss')
           // return flag;
         

        }
        function Checkduplicate() {
            var aa = "";
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/Checkduplicate") %>',
               contentType: "application/json; charset=utf-8",
                data: '{firstname:"' + $('#<%=Fname.ClientID%>').val() + '",lastname:"' + $('#<%=Lname.ClientID%>').val() + '",company:"' + $('#<%=Company.ClientID%>').val() + '"}',
                dataType: "json",
                async : false,
                success: function (data) {
                    var msgval = JSON.parse(data.d);
                    aa= msgval.SetMsg;
                 
                }
            });
            return aa
        }
    </script>


    <style type="text/css">
        /*.modalBackground {
            background-color: White;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }*/

        .modalPopup {
            border-radius: 11px;
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: #3c8dbc;
            padding-top: 10px;
            padding-left: 10px;
            width: 40%;
            height: 380px;
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
      
        <asp:Button ID="Button2" runat="server" style="display:none" />
                            <asp:HiddenField ID="HiddenField2" runat="server" />
            <cc1:ModalPopupExtender  ID="ModalPopupExtender2" runat="server" TargetControlID="Button2" PopupControlID="pnlmessage"
 BackgroundCssClass="modalBackground" ></cc1:ModalPopupExtender>

<asp:Panel ID="pnlmessage" runat="server"  CssClass="modalPopup"  BackColor="White"  style="margin-top:10px;height:22%;display:none;width:26%">
   
       <div class="header">
        Confirmation
    </div>
    <div class="body">
        Do you want to assign to team?
    </div>
    <div class="footer" style="text-align:right">
       <asp:Button ID="btnYes"   TabIndex="20" runat="server"  class="btn btn-primary modelbtn" Text="Yes" OnClick="btnYes_Click"/>
<asp:Button ID="btnno" class="btn btn-primary modelbtn" runat="server" Text="No" TabIndex="22" OnClick="btnno_Click"/>
    </div>
  

   
</asp:Panel>



        <asp:Button ID="btnShowPopup" runat="server" style="display:none" />
                            <asp:HiddenField ID="hidForModel" runat="server" />
    <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
 BackgroundCssClass="modalBackground"></cc1:ModalPopupExtender>

<asp:Panel ID="pnlpopup" runat="server"   CssClass="modalPopup"   BackColor="White"  style="margin-top:10px;height:70%;display:none;width:35%;">


 
                  <div class="header">
                           Add Action
                      </div>
        
     <div class="body">
          <table width="100%"  height:"100%" cellpadding="6" class="table" cellspacing="0">
    <tr>
         <td colspan="2">

                                    <div class="form-group">
                                          <label>Action: </label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtnxtaction" runat="server" TextMode="MultiLine" Rows="5" Columns="8" MaxLength="1000" CssClass="form-control" Height="50px"></asp:TextBox>
                           
                                    </div>
             </td>
        </tr>
       <tr>
         <td >

                                  
                                        <div class="form-group">
                                            <label>Date: </label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <div class="form-group">
                                                  <asp:TextBox ID="txtnxtactndt" runat="server" CssClass="form-control" Style="background-color: white;" ReadOnly="true" ></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender"
                                                TargetControlID="txtnxtactndt"></cc1:CalendarExtender>
                                        </div>                                            
                                            </div>
                                     </td>
              <td>
                                   
                                        <div class="form-group">
                                            <label>Assigned To: </label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <div class="clearfix"></div>
                                            <asp:DropDownList ID="ddlowner" runat="server" Width="100%" CssClass="form-control" TabIndex="7"></asp:DropDownList>
                                          
                                        </div>
                                </td>


           </tr>
   
       <tr>
         <td>
                                        <div class="form-group">
                                            <label>
                                                Set Task Status To :
                                            </label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:DropDownList ID="DropDownList1" runat="server" Width="100%" CssClass="form-control" TabIndex="7">
                                                      <asp:ListItem Value="o">Open</asp:ListItem>
                                                    <asp:ListItem value="c">Close</asp:ListItem>
                                                </asp:DropDownList>
                                        </div>
                                    </td>
         </tr>
           </table>
         </div>
  
                   <div class="footer" style="text-align:right">                    
                              
                                        <%--style="padding-bottom: 10px;"--%>
                                        <asp:Button ID="btnsavetask"   TabIndex="20" runat="server"  class="btn btn-primary modelbtn" Text="Save" OnClientClick="return validatetask();" OnClick="btnsavetask_Click" />
<asp:Button ID="btncancel" class="btn btn-primary modelbtn" runat="server" Text="cancel" TabIndex="22" OnClick="btncancel_Click"/>
                                     
          </div>                   

                      

     <div style="height: 30px;padding-left:15px;">
 <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color:red">*</span>)</b>
        <br/></div>    
</asp:Panel>




           





        <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Add Contact</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Back" class="btn btn-primary"
                                    OnClick="btnFind_Click" Visible="false" />
                                <asp:Button Style="margin-right: 5px;" type="btnfind1" ID="Button1" runat="server" Text="Find" class="btn btn-primary"
                                    OnClick="btnFind_Click1" />

                                <%--  <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" runat="server" />--%>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                                 
                                      </div>
                                <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                            <div class="form-group">
                                        <label for="exampleInputEmail1">Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="30" id="Fname" placeholder="Enter First Name" tabindex="2" />

                                    </div>
                                     <div class="form-group">

                                        <label for="exampleInputEmail1">Company:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="75" id="Company" placeholder="Enter Company" tabindex="2" onchange="FillComapnydetail();"   />
                                         <input type="button" style="background-color: Transparent;background-repeat:no-repeat;border: none; cursor:pointer;overflow: hidden;color:#3c8dbc" runat="server" ID="openpopup" Value="Add Company Details"  OnClick="popupvalidation()" />
                                    <input type="hidden" id="hidcompanyid" runat="server" value="0" />
                                     </div>
                                    
                                       <div id="divcompanydetails" style="display:none">
                                           <div class="panel panel-default">
                                               <div class="row">
                                                    <div class="col-lg-11 col-md-10 col-sm-7 col-xs-9" style="margin: 15px;">
                                                   
                                                   <div class="form-group" style="display: none;">
                                                       <label for="exampleInputEmail1">Company Phone:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                                       <input type="text" runat="server" class="form-control numeric text-left" maxlength="13" id="txtcompphone" placeholder="Enter Company Phone" tabindex="2" />
                                                   </div>

                                                   <div class="form-group">
                                                       <label for="exampleInputEmail1">Address:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                                       <input type="text" runat="server" class="form-control" maxlength="50" id="txtcompadd" placeholder="Enter Address" tabindex="2" />

                                                   </div>
                                                   <div class="form-group">
                                                       <label for="exampleInputEmail1">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                                       <asp:TextBox CssClass="form-control" runat="server" MaxLength="30" TabIndex="2" ID="txtcompcity"></asp:TextBox>
                                                     <%--   <input type="text" runat="server" class="form-control" maxlength="30" id="txtcompcity" placeholder="Enter City" tabindex="2" />--%>
                                                       <cc1:AutoCompleteExtender ServiceMethod="SearchCities" MinimumPrefixLength="2"
                                                           CompletionInterval="100" EnableCaching="false" CompletionSetCount="3"
                                                           TargetControlID="txtcompcity" 
                                                           ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" OnClientShown="resetPosition1" >
                                                       </cc1:AutoCompleteExtender>
                                                   </div>
                                                   <div class="form-group">
                                                       <label for="exampleInputEmail1">State:</label>
                                                       <%--<input type="text" runat="server" class="form-control" maxlength="30" id="txtcompstate" placeholder="Enter State" tabindex="2" />--%>
                                                          <asp:DropDownList ID="txtcompstate" runat="server" Width="100%" CssClass="form-control" TabIndex="2" onchange="Fillcountry1(this)"></asp:DropDownList>
                                                   </div>
                                                   <div class="form-group">
                                                       <label for="exampleInputEmail1">Country:</label>
                                                       <asp:DropDownList ID="ddlcompcountry" runat="server" Width="100%" CssClass="form-control" TabIndex="2"  Enabled="false"></asp:DropDownList>
                                                       <input type="hidden" id="hidcompcountry" runat="server" />
                                                   </div>
                                                   <div class="form-group">
                                                       <label for="exampleInputEmail1">Zip:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                                       <input type="text" runat="server" class="form-control numeric text-right" maxlength="6" id="txtcompzip" placeholder="Enter Zip" tabindex="2" />
                                                   </div>

                                                        <div class="form-group" style="display:none">
                                                       <label for="exampleInputEmail1">Remarks:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                                       <textarea runat="server" rows="5" cols="5" class="form-control" maxlength="200" id="txtcompdesc" placeholder="Enter Company Remarks" tabindex="2" />
                                                   </div>
                                               </div>
                                                    </div>
                                           </div>
                                     </div>
                            
                                    <div class="form-group" style="display:none">
                                        <label for="exampleInputEmail1">Last Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;display:none">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="30" id="Lname" placeholder="Enter Last Name" onblur="fillfirstcontact()" tabindex="2" />

                                    </div>
                                    <div class="form-group" style="display:none;">
                                        <label for="exampleInputEmail1">Job Title:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="40" id="JobTitle" placeholder="Enter Job Title" tabindex="2" />
                                    </div>
                                   
                                          <div class="form-group">
                                        <label for="exampleInputEmail1">Same as company address:</label>
                                         <input id="chkaddress" type="checkbox" runat="server"  tabindex="5" onchange="FillAddress();" />
                                       
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Address:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="50" id="Address" placeholder="Enter Address" tabindex="5" />
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">City:</label>
                                         &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>   
                                        <asp:TextBox   runat="server" class="form-control" maxlength="15" id="City" placeholder="Enter City" tabindex="5" ></asp:TextBox>
                                        <%--<input type="text" runat="server" class="form-control" maxlength="15" id="City" placeholder="Enter City" tabindex="5" />--%>
                                          <cc1:AutoCompleteExtender ServiceMethod="SearchCities" MinimumPrefixLength="2"
                                                           CompletionInterval="100" EnableCaching="false" CompletionSetCount="3"
                                                           TargetControlID="City"
                                                           ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" OnClientShown="resetPosition" >
                                                       </cc1:AutoCompleteExtender>
                                       <%-- <br />
                                        <input type="text" runat="server" class="form-control" maxlength="15" id="State" placeholder="Enter State" tabindex="5" />--%>
                                    </div>
                                    <div class="form-group">
                                         <label for="exampleInputEmail1">State:</label>
                                         &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>     
                                         <%--<input type="text" runat="server" class="form-control" maxlength="15" id="State" placeholder="Enter State" tabindex="5" />--%>
                                         <asp:DropDownList ID="State" runat="server" Width="100%" CssClass="form-control" TabIndex="5" onchange="FillCountry(this)"></asp:DropDownList>
                                    </div>
                                        <div class="form-group">
                                         <label for="exampleInputEmail1">Country:</label>
                                           &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>     
                                         <asp:DropDownList ID="ddlcountry" runat="server" Width="100%" CssClass="form-control" TabIndex="5" Enabled="false"></asp:DropDownList>
                                                <input type="hidden" id="hidcontactcountry" runat="server" />
                                    </div>

                                        <div class="form-group">
                                        <label for="exampleInputEmail1">Zip:</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="6" id="Zip" onkeypress="javascript:return isNumber (event)" placeholder="Enter Zip" tabindex="5" />
                                       <%-- <br />
                                        <asp:DropDownList ID="ddlcountry" runat="server" Width="100%" CssClass="form-control" TabIndex="7"></asp:DropDownList>--%>
                                    </div>

                                    <div class="ptwrap">
                                      
                                        <div class="form-group panel panel-default pt" id="divPT1"  style="padding-left: 10px;">
                                            <input type="hidden" runat="server" id="Hidphonevalues" />
                                            <input type="hidden" runat="server" id="Hidphoneddlvalues" />
                                            <input type="hidden" runat="server" id="HidPhoneContNamehtml" />
                                            <input type="hidden" runat="server" id="HidPhoneID" />


                                            <asp:HiddenField runat="server" ID="Hidphonevalues1" />
                                            <asp:HiddenField runat="server" ID="Hidphoneddlvalues1" />
                                            <asp:HiddenField runat="server" ID="Hidphoneallddlvalues1" />
                                            <asp:HiddenField runat="server" ID="HidPhoneContName1" />
                                            <asp:HiddenField runat="server" ID="HidPhoneId1" />
                                            <div class="row">
                                                <div class="col-md-6 col-sm-6 col-xs-12" >
                                              <div class="form-group">
                                                    
                                                   <label for="exampleInputEmail1">Contact Name:</label>
                                                <input type="text" runat="server" class="form-control PhoneContact" maxlength="25" id="txtaddcontact1" placeholder="Enter Contact Name"
                                                    tabindex="5"   onchange="Validphone(This);"/>
                                                  <input type="hidden" class="hidphoneid"  id="phoneid" runat="server" />
                                                     </div>
                                                    </div>

                                                  <div class="col-md-5 col-sm-5 col-xs-12">
                                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Job Title:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                    <select id="ddlphonetype1" runat="server" class="form-control phoneddl" tabindex="5">
                                                       
                                                    </select>
                                                            </div>
                                                </div>
                                                <div class="col-md-6 col-sm-6 col-xs-12" >
                                              <div class="form-group">
                                            <label for="exampleInputEmail1">Phone:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                   <input type="text" runat="server" class="form-control numeric text-left phonetext" maxlength="15" id="Phone1" onkeypress="javascript:return isNumber (event)" placeholder="Enter Phone"
                                                    tabindex="5" />
                                                    
                                                  </div>
                                                    </div>

                                                  
                                                  <div class="col-md-6 col-sm-6 col-xs-12" >
                                                      <div class="row">
                                                            <div class="col-md-10 col-sm-10 col-xs-12">
                                                         <div class="form-group">
                                            <label for="exampleInputEmail1">Email:</label>
                                                              <input type="text" runat="server" class="form-control emailtext" maxlength="50" id="Email1" placeholder="Enter Email"
                                                    tabindex="5"  onchange="ValidMail(this);"/>
                                                             </div>
                                                        </div>

                                                        
                                                  <div style="padding-left: 13px;margin-top: 26px; ">
                                                <img id="imgaddphone" src="img/plus.png" alt="Add" onclick="CreatePhone();" />
                                                </div>
                                                 </div>
                                                       </div>
                                                </div>
                                            
                                            <div class="clearfix"></div>
                                           <%-- <div class="col-md-4 col-sm-4 col-xs-4" style="padding-left: 0">
                                                <input type="text" runat="server" class="form-control PhoneContact" maxlength="25" id="txtaddcontact1" placeholder="Enter Contact Name"
                                                    tabindex="5" />

                                            </div>     --%>                                                                        
                                            <div class="col-md-4 col-sm-4 col-xs-4 paddingleft0">
                                               <%-- <input type="text" runat="server" class="form-control numeric text-left phonetext" maxlength="15" id="Phone1" onkeypress="javascript:return isNumber (event)" placeholder="Enter Phone"
                                                    tabindex="5" />--%>
                                            </div>
                                            <div class="row">
                                              <%--  <div class="col-md-3 col-sm-2 col-xs-2" style="padding-left: 0;display:none" >
                                                    <select id="ddlphonetype1" runat="server" class="form-control phoneddl" tabindex="7">
                                                       
                                                    </select>
                                                </div>--%>
                                                <div class="col-lg-1 col-md-1 col-xs-1 " style="padding-left: 13px; ">
                                                 <%--   <img id="imgaddphone" src="img/plus.png" alt="Add" onclick="CreatePhone();" />--%>
                                                </div>
                                            </div>
                                        </div>
                                            
                                    </div>
                                    <div class="etwrap" style="display:none;">
                                        <div class="form-group et" id="divET1">
                                            <input type="hidden" runat="server" id="Hidemailvalues" />
                                            <input type="hidden" runat="server" id="Hidemailddlvalues" />
                                            <input type="hidden" runat="server" id="HidEmilCt" />
                                            <input type="hidden" runat="server" id="HidEmailId" />

                                            <asp:HiddenField runat="server" ID="Hidemailvalues1" />
                                            <asp:HiddenField runat="server" ID="Hidemailddlvalues1" />
                                             <asp:HiddenField runat="server" ID="Hidallemailddlvalues1" />
                                            <asp:HiddenField runat="server" ID="HidEmailContName1" />
                                             <asp:HiddenField runat="server" ID="HidEmailId1" />
                                             <div class="row">
                                                <div class="col-md-4 col-sm-4 col-xs-12">
                                              <div class="form-group">
                                                    
                                                                         <label for="exampleInputEmail1">Contact Name:</label>
                                                  <input type="text" runat="server" class="form-control EmailContact" maxlength="25" id="txtEmailcontact1" placeholder="Enter Contact Name"
                                                    tabindex="5" />
                                                   <input type="hidden" class="hidEmailid"  id="emailid" runat="server" />
                                                  </div>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                                         <div class="form-group">
                                            <label for="exampleInputEmail1">Email:</label>
                                                              <input type="text" runat="server" class="form-control emailtext" maxlength="50" id="Email12" placeholder="Enter Email"
                                                    tabindex="5"  onchange="ValidMail(this);"/>
                                                             </div>
                                                        </div>
                                               
                                                 <div class="col-md-4 col-sm-2 col-xs-12" >
                                                     <div class="row">
                                                          <div class="col-md-8">
                                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Type:</label>&nbsp;&nbsp;
                                                    <select id="ddlemailtype1" runat="server" class="form-control emailddl" tabindex="5">
                                                        <%-- <option value="Home" selected="selected">Home</option>
                                                        <option value="Other">Other</option>--%>
                                                    </select>
                                                            </div>
                                                </div>


                                                   <div  style="    padding-left: 13px;    margin-top: 26px;">
                                                         <div class="form-group">
                                                                 <img id="imgaddEmail" src="img/plus.png" alt="Add" onclick="CreateEmail();" />
                                                             </div>
                                                       </div>
                                                       </div>
                                                 </div>
                                                             <%--&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>--%>
                                            <div class="clearfix"></div>
                                  

                                        </div>
                                    </div>
                                          </div>
                                    <div class="utwrap">
                                        <div class="form-group ut" id="divUT1">
                                            <input type="hidden" runat="server" id="HidUrlvalues" />
                                            <input type="hidden" runat="server" id="HidUrlddlvalues" />

                                                  <input type="hidden" runat="server" id="HidurlId" />

                                            <asp:HiddenField runat="server" ID="HidUrlvalues1" />
                                            <asp:HiddenField runat="server" ID="HidUrlddlvalues1" />
                                              <asp:HiddenField runat="server" ID="Hidallurltype" />
                                            <asp:HiddenField runat="server" ID="HidUrlId1" />
                                            <div class="row">
                                                 <div class="col-md-8 col-sm-5 col-xs-12 ">
                                                     <div class="form-group">
                                            <label for="exampleInputEmail1">URL:</label><%--&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>--%>
                                              <input type="text" runat="server" class="form-control urltext" maxlength="50" id="Url1" placeholder="Enter URL"
                                                    tabindex="5" />
                                                         <input type="hidden" class="hidurlid" id="urlid" runat="server"/>
                                                     </div>
                                                     </div>                                                 
                                                <div class="col-md-4 col-sm-3 col-xs-12" >
                                                    <div class="row">
                                                         <div class="col-md-8" style="display:none">
                                                  
                                                      <div class="form-group">
                                                    <label for="exampleInputEmail1">Type:</label>&nbsp;&nbsp;
                                                    <select id="ddlurltype1" runat="server" class="form-control urlddl" tabindex="5">
                                                       
                                                    </select>
                                                          </div>
                                                </div>

                                                <div  style="padding-left: 13px ;margin-top: 26px;">
                                                      <div class="form-group">
                                                    <img id="imgaddurl" src="img/plus.png" alt="Add" onclick="CreateUrl();" />
                                                          </div>
                                                </div>
                                            </div>
                                              </div>
                                            </div>

                                    </div>
                                        </div>
                                
                                   
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Status:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlstatus" runat="server" Width="100%" CssClass="form-control" TabIndex="9"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Lead Type:</label>
                                        <%-- <asp:DropDownList  Height="20%" Width="100%" multiple="multiple" CssClass="form-control" TabIndex="9"></asp:DropDownList>--%>
                                        <select id="ddltag" runat="server" name="ddltag" class="form-control" multiple="false" TabIndex="9" ></select>
                                        <input type="hidden" runat="server" id="hidtags" />
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Lead Source:</label>
                                        <asp:DropDownList ID="ddlleadsource" runat="server" Width="100%" CssClass="form-control" TabIndex="9"></asp:DropDownList>
                                    </div>
                                
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Remark:</label>
                                        <%--<input type="text" runat="server" class="form-control" maxlength="200" id="Background" placeholder="Enter Background"  tabindex="13" />--%>
                                        <asp:TextBox runat="server" ID="txt" MaxLength="200" placeholder="Enter Remark" TextMode="MultiLine" Height="32%" tabindex="9" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Owner:</label>
                                             <asp:DropDownList ID="ddlsp" runat="server" Width="100%" CssClass="form-control" TabIndex="9"></asp:DropDownList>
                                        <%--<asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>--%>
                                    </div>
                                            <div class="form-group">
                                        <label for="exampleInputEmail1">Active:</label>
                                         <input id="chkbxactive" type="checkbox" runat="server" checked="checked" tabindex="9" />
                                       
                                    </div>
                                    <div class="clearfix" style="padding-bottom: 10px;"></div>
                                    <div style="background-color: #3c8dbc; color: white;">
                                        <%--<strong>Custom Fields</strong>--%>
                                        <input id="hidcustomfields" type="hidden" runat="server" value="" />
                                        <input id="hidcustomval" type="hidden" runat="server" />
                                        <input id="hidchkid" type="hidden" runat="server" />
                                        <asp:HiddenField runat="server" ID="hidchk" />
                                        <asp:HiddenField runat="server" ID="hidContactid" />

                                    </div>

                                    <div id="divCF" class="form-group">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="box-footer">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" OnClientClick="return validate();" TabIndex="11" />
                            <asp:Button ID="btndelete" runat="server" Visible="false" CssClass="btn btn-primary" Text="Delete" OnClick="btndelete_Click1" OnClientClick="Confirm() ;" TabIndex="11" />
                            <input type="button" style="display: none" id="btnCancel" class="btn btn-primary" value="Cancel" onclick="btnCancel_Click" TabIndex="11" />
                            <asp:Button ID="btncancel1" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btncancel1_Click" TabIndex="11" />
                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>


            </div>


        <%--abhishek--%>

        <div class="box-body" id="rptmain" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">CONTACT DETAIL LIST</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClientClick="setjobtitle();" OnClick="btnBack_Click" />

                            </div>
                        </div>
                       
                        <div class="box-body">
                            <div class="col-lg-9 col-md-9 col-sm-7 col-xs-9">
                                <div class="col-md-12 paddingleft0">
                              <div class="form-group col-md-5">                         
                                <label for="exampleInputEmail1">Country :</label>                                 
                                <asp:DropDownList ID="ddlCountryList" runat="server" CssClass="form-control"></asp:DropDownList>
                               </div>
                             <div class="form-group col-md-5">                                  
                                <label for="exampleInputEmail1">State :</label>                            
                                <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>                                  
                             </div>
                             <div class="form-group col-md-5">   
                                 <label for="exampleInputEmail1">City :</label>                                              
                                 <asp:DropDownList ID="ddlcity" runat="server" CssClass="form-control"></asp:DropDownList>
                              </div>
                              <div class="form-group col-md-1">
                                        <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                        <asp:Button type="button" ID="btnGo" runat="server" Style="padding: 3px 14px;" Text="Go" class="btn btn-primary go-button-dsr" OnClick="btnGo_Click" />
                                    </div>
                           </div>
                       </div>
                </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>First Name</th>
                                                <th>Last Name</th>
                                               <%-- <th>Job Title</th>--%>
                                                <th>Company</th>
                                                <th>Phone</th>
                                                <th>Email</th>
                                                <th>Url</th>
                                                <th>Address</th>
                                                <th>Status</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("Contact_Id") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("Contact_Id") %>' />
                                        <td><%#Eval("FirstName") %></td>
                                        <td><%#Eval("Lastname") %></td>
                                       <%-- <td><%#Eval("Jobtitle") %></td>--%>
                                        <td><%#Eval("CompName") %></td>
                                        <td><%#Eval("Phone") %></td>
                                        <td><%#Eval("Email") %></td>
                                        <td>
                                            <%#Eval("Url") %>
                                        </td>
                                        <td><%#Eval("address") %></td>
                                        <td><%#Eval("Status") %></td>
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


        <%--abhishek--%>
    </section>
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script type="text/javascript">
        function ToggleDisplay(id) {
            var elem = document.getElementById('d' + id);
            if (elem) {
                if (elem.style.display != 'block') {
                    elem.style.display = 'block';
                    elem.style.visibility = 'visible';
                }
                else {
                    elem.style.display = 'none';
                    elem.style.visibility = 'hidden';
                }
            }
        }
    </script>

    <script type="text/javascript">
        //testjavascriptcode
        function testjavascriptcode(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
        function FillInDropBox(c) {
            //alert("Test56");
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetCustomStoreprocedureFieldsData_testing") %>',
                contentType: "application/json; charset=utf-8",
                data: '{AttrType: "Contact",DdlId:"' + c.id + '",DDLValue:"' + c.value + '"}',
                dataType: "json",
                success: function (data) {
                    console.log("fsdG")
                    jsdata1 = JSON.parse(data.d);
                    $.each(jsdata1, function (key1, value1) {
                        if (value1.AttributFieldType == "Sql Procedure") {
                            var select = document.getElementById(value1.AttributeField.replace(/\s+/g, ''));
                            console.log(select.length)
                            if (select) {
                                select.innerHTML = ""; 
                                //for (i = 0; i < select.length; i++) {
                                //        select.remove(i);
                                //}
                                var opt = document.createElement('option');
                                opt.value = 0;
                                opt.innerHTML = "Select";
                                select.appendChild(opt);
                                if (value1.AttributeData) {
                                    var arr = value1.AttributeData.split("*#");
                                    for (var j = 0; j < arr.length; j++) {
                                        var opt = document.createElement('option');
                                        opt.value = arr[j];
                                        opt.innerHTML = arr[j];
                                        select.appendChild(opt);
                                    }
                                }
                            }
                        }
                    });

                }
            });
        }
        function CreateOptionsVal(c) {
            //alert("Test1");
            var DDLValue = c.value;
            var DDLId = c.id
            var type = "Contact";
          //  alert('P1');
            //    ////alert(DDLValue + "," + DDLId);
            // GetCustomFields();
            // function GetCustomstoreprocFields()
            {
                //  ////alert("555");
                $('#spinner').show();
                var type2 = document.getElementById('<%= hidchk.ClientID%>').value;
                //hidcustomval
                // ////alert(type2.value);
                var type = "Contact";
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("CRMDBService.asmx/GetCustomStoreprocedureFieldsData_testing") %>',
                    contentType: "application/json; charset=utf-8",
                    data: '{AttrType: "' + type + '",DdlId:"' + DDLId + '",DDLValue:"' + DDLValue + '"}',
                    dataType: "json",
                    success: function (data) {
                        jsdata1 = JSON.parse(data.d);

                        var output = "<div class='col-lg-5 col-md-6 col-sm-8 col-xs-10'>";
                        var Cdata = ""; var cbid = ""; var selid = "";
                        $.each(jsdata1, function (key1, value1) {
                            //////alert(jsdata1);
                            Cdata += value1.AttributeField + "^";
                            // ////alert(Cdata);
                            if (value1.AttributFieldType == "Number") {
                                //////alert(value1.AttributeField);
                                output += "<div class='form-group'><label>" + value1.AttributeField + ": </label><input type='text' id='" + value1.AttributeField + "' palcehoider='l' class='form-control numeric text-right customfieldgroup' maxlength='10' onkeypress='return isNumberKey(event)'  tabindex='10'/> </div>";
                            }

                            else if (value1.AttributFieldType == "Dropdown" || value1.AttributFieldType == "Checkbox" || value1.AttributFieldType == "Sql View" || value1.AttributFieldType == "Sql Procedure") {
                                // ////alert(value1.AttributeField);
                                var arr = value1.AttributeData.split("*#");
                                //   ////alert(arr);
                                //   output += '<div class="form-group" id="' + value1.AttributeField + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                if (value1.AttributFieldType == "Checkbox") {
                                    cbid += "cb" + (value1.AttributeField).replace(/ +/g, "") + "@";
                                    output += '<div class="form-group customfieldgroup" id="cb' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                    for (var j = 0; j < arr.length; j++) {
                                        output += '<label><input type="checkbox"  value="' + arr[j] + '"  tabindex="10"/>' + arr[j] + '</label>&nbsp;&nbsp;&nbsp;'
                                        // ////alert(output)
                                    }
                                    output += '</div>';
                                    //////alert(output);
                                }
                                else {
                                    // ////alert(value1.AttributeField);
                                    var dd = value1.AttributFieldType;
                                    //////alert(c);
                                    output += '<div class="form-group" id="sel' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                    selid += "sel" + value1.AttributeField + "@";
                                    // CreateOptionsVal(c);
                                    //if (value1.AttributFieldType == "Sql View") {
                                    //    output += '<select class="form-control customfieldgroup" onchange="CreateOptionsVal(this);" id="' + (value1.AttributeField).replace(/ +/g, "") + '" name="' + value1.AttributeField + '" >'
                                    //    output += '<option value="0" >Select</option>'
                                    //    //for (var j = 0; j < arr.length; j++) {
                                    //    //    output += '<option  >' + arr[j] + '</option>'
                                    //    //}
                                    //    //for (var j = 0; j < arr.length; j++) {
                                    //    //    if (arr[j] == DDLValue) {
                                    //    //        output += '<option  selected>' + arr[j] + '</option>'
                                    //    //    }
                                    //    //    else {
                                    //    //        output += '<option  >' + arr[j] + '</option>'
                                    //    //    }
                                    //    //    // output += '<option  >' + arr[j] + '</option>'
                                    //    //}
                                    //    //output += '</select>';
                                    //}
                                    if (value1.AttributFieldType == "Sql Procedure") {
                                        output += '<select class="form-control customfieldgroup"  id="' + (value1.AttributeField).replace(/ +/g, "") + '" name="' + value1.AttributeField + '"  tabindex="10" >'
                                        //output += '<option value="0" >Select</option>'

                                        //for (var j = 0; j < arr.length; j++) {
                                        //    if (arr[j] == DDLValue) {
                                        //        output += '<option  selected>' + arr[j] + '</option>'
                                        //    }
                                        //    else {
                                        //        output += '<option  >' + arr[j] + '</option>'
                                        //    }

                                        //}
                                        //output += '</select>';
                                    }

                                    else {

                                        output += '<select class="form-control customfieldgroup" onchange="CreateOptionsVal(this);" id="' + (value1.AttributeField).replace(/ +/g, "") + '" name="' + value1.AttributeField + '"  tabindex="10" >'
                                        //  output += '<select class="form-control customfieldgroup"  id="' + (value1.AttributeField).replace(/ +/g, "") + '" name="' + value1.AttributeField + '" >'
                                        output += '<option value="0" >Select</option>'
                                        //for (var j = 0; j < arr.length; j++) {
                                        //    output += '<option  >' + arr[j] + '</option>'
                                        //}
                                        //for (var j = 0; j < arr.length; j++) {
                                        //    if (arr[j] == DDLValue) {
                                        //        output += '<option  selected>' + arr[j] + '</option>'
                                        //    }
                                        //    else {
                                        //        output += '<option  >' + arr[j] + '</option>'
                                        //    }
                                        //    // output += '<option  >' + arr[j] + '</option>'
                                        //}
                                        //output += '</select>';
                                    }
                                    for (var j = 0; j < arr.length; j++) {
                                        if (arr[j] == DDLValue) {
                                            output += '<option  selected>' + arr[j] + '</option>'
                                        }
                                        else {
                                            output += '<option  >' + arr[j] + '</option>'
                                        }
                                        // output += '<option  >' + arr[j] + '</option>'
                                    }
                                    output += '</select>';
                                    output += '</div>';
                                    //////alert(c);
                                }


                            }
                            else if (value1.AttributFieldType == "Date") {
                                output += "<div class='form-group'><label>" + value1.AttributeField + " </label><input type='text' id='" + value1.AttributeField + "'class='form-control customfieldgroup datepicker' maxlength='20' readonly='readonly'  tabindex='10' /></div>";
                                //output+=' <input  type="text" placeholder="click to show datepicker"  id="example1">';
                                //output += " $('.datepicker').datepicker({format: 'dd/mm/yyyy'})";

                            }

                            else if (value1.AttributFieldType == "Multiple Line Text") {
                                output += "<div class='form-group'><label>" + value1.AttributeField + " </label><textarea id='" + value1.AttributeField + "' value=''  class='form-control customfieldgroup'  tabindex='10'> </textarea></div>";
                            }
                            else {
                                output += "<div class='form-group'><label>" + value1.AttributeField + " </label><input type='text' id='" + value1.AttributeField + "' value='' class='form-control customfieldgroup' maxlength='20'  tabindex='10'/> </div>";
                            }

                        });
                        output += "</div> <script type='text/javascript'>  $(function () {var date = new Date(); date.setDate(date.getDate()); $('.datepicker').datepicker({ startDate: date, format: 'dd/M/yyyy', autoclose: true });});";

                        Cdata = Cdata.substr(0, Cdata.length - 1);
                        $('#<%=hidcustomfields.ClientID %>').val(Cdata);
                    cbid = cbid.substr(0, cbid.length - 1);
                    $('#<%=hidchkid.ClientID %>').val(cbid);
                    <%--     selid = selid.substr(0, selid.length - 1);
                     $('#<%=hidselid.ClientID %>').val(selid);--%>
                    //  ////alert(cbid); ////alert(selid);

                    //  ////alert($('#<%=hidcustomfields.ClientID %>').val())
                    // ////alert(output);
                    $('#divCF').html(output);
                }
                });
            $('#spinner').hide();
        }



            

            ////alert("On Change Event");

    }
        function testingabhi() { //V1
       
        var contactid = $('#<%=hidContactid.ClientID%>').val();
      var datafordynamicctrl = "";
      datafordynamicctrl = $('#<%=hidchk.ClientID%>').val();
        //datafordynamicctrl.split(",");
        //hidcustomval

     //   alert('P2');
            var type = "Contact";

            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetDataForCustomFields") %>',
                contentType: "application/json; charset=utf-8",
                data: '{AttrType: "' + type + '",Contactid: "' + contactid + '" }',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);

                    //var output = "<div class='col-lg-5 col-md-6 col-sm-8 col-xs-10'>";
                    var output = "<div>";
                    var Cdata = ""; var cbid = ""; var selid = "";
                    $.each(jsdata1, function (key1, value1) {
                        debugger;
                        //////alert(jsdata1);
                        Cdata += value1.AttributeField + "^";
                        // ////alert(Cdata);
                      //  alert("test2");
                        if (value1.AttributFieldType == "Number") {
                            //  ////alert(value1.AttributeField);
                            output += "<div class='form-group'><label>" + value1.AttributeField + ": </label><input type='text' id='" + value1.AttributeField + "' value='" + value1.ctrlvalue + "' class='form-control numeric text-right customfieldgroup' maxlength='10' onkeypress='return isNumberKey(event)' tabindex='10' /> </div>";
                           
                        }

                        else if (value1.AttributFieldType == "Dropdown" || value1.AttributFieldType == "Checkbox" || value1.AttributFieldType == "Sql View" || value1.AttributFieldType == "Sql Procedure") {
                            // ////alert(value1.AttributeField);
                            var arr = value1.AttributeData.split("*#");
                            //   ////alert(value1.AttributFieldType);
                            //   output += '<div class="form-group" id="' + value1.AttributeField + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                         //   alert(arr);
                            if (value1.AttributFieldType == "Checkbox") {
                                //  alert("test2");
                                debugger;
                                var arr2 = value1.ctrlvalue.split("-");
                                var f = true;
                                cbid += "cb" + (value1.AttributeField).replace(/ +/g, "") + "@";
                                output += '<div class="form-group customfieldgroup" id="cb' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                for (var j = 0; j < arr.length; j++) {
                                    for (var k = 0; k < arr2.length; k++) {
                                        if (arr[j] == arr2[k]) {
                                            //    alert(arr[j]);
                                            //output += '<label><input type="checkbox"  value="' + arr[j] + '" checked/>' + arr[j] + '</label>&nbsp;&nbsp;&nbsp;'
                                            //break;
                                            f = true;
                                            break;
                                        }
                                        else {
                                            f = false;
                                        }
                                    }
                                    if (f==true)
                                        output += '<label><input type="checkbox" tabindex="10"  value="' + arr[j] + '" checked/>' + arr[j] + ' </label>&nbsp;&nbsp;&nbsp;'
                                    else
                                        output += '<label><input type="checkbox" tabindex="10"  value="' + arr[j] + '"/>' + arr[j] + '</label>&nbsp;&nbsp;&nbsp;'
                                    // ////alert(output)
                                }
                                output += '</div>';

                            }
                            else if (value1.AttributFieldType == "Sql Procedure") {
                                {
                                    //  ////alert(value1.AttributeField);
                                    var dd = value1.AttributFieldType;
                                    //////alert(c);
                                    output += '<div class="form-group" id="sel' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                    selid += "sel" + value1.AttributeField + "@";
                                    // CreateOptionsVal(c);
                                    output += '<select class="form-control customfieldgroup"  id="' + (value1.AttributeField).replace(/ +/g, "") + '" name="' + value1.AttributeField + '" tabindex="10" >'
                                    // ////alert(output);
                                    output += '<option value="0" >Select</option>'
                                    //for (var j = 0; j < arr.length; j++) {
                                    output += '<option selected >' + value1.ctrlvalue + '</option>'
                                    //}
                                    output += '</select>'; output += '</div>';
                                    //////alert(c);
                                }
                            }
                            else {
                                // ////alert(value1.AttributeField);
                                // ////alert("check");
                                output += '<div class="form-group" id="sel' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                selid += "sel" + value1.AttributeField + "@";
                                output += '<select class="form-control customfieldgroup" onchange="FillInDropBox(this);" id="' + (value1.AttributeField).replace(/ +/g, "") + '" name="' + value1.AttributeField + '" tabindex="10" >'
                                output += '<option value="0" >Select</option>'
                                for (var j = 0; j < arr.length; j++) {
                                    if (arr[j] == value1.ctrlvalue) {
                                        output += '<option  selected>' + arr[j] + '</option>'
                                    }
                                    else {
                                        output += '<option  >' + arr[j] + '</option>'
                                    }
                                    // output += '<option  >' + arr[j] + '</option>'
                                }
                                output += '</select>'; output += '</div>';
                            }


                        }

                        else if (value1.AttributFieldType == "Date") {

                            // ////alert(value1.ctrlvalue);
                            if (value1.ctrlvalue != "") {
                                value1.ctrlvalue = value1.ctrlvalue.split(" ");
                                value1.ctrlvalue[0] = value1.ctrlvalue[0].split("/")
                                var monthNames = ["", "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                                                 "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
                                ];
                                // ////alert((value1.ctrlvalue[0])[0] + "/" + monthNames[(value1.ctrlvalue[0])[1]] + "/" + (value1.ctrlvalue[0])[2]);
                                if ((value1.ctrlvalue[0])[1] <= 10) {
                                    (value1.ctrlvalue[0])[1].split("0")// to get month as 1,2,3 instead of 01,02,03
                                }

                                if (((value1.ctrlvalue[0])[1]) >= 10) {
                                    var datec = (value1.ctrlvalue[0])[0] + "/" + monthNames[((value1.ctrlvalue[0])[1])] + "/" + (value1.ctrlvalue[0])[2];
                                }
                                else {
                                    var datec = (value1.ctrlvalue[0])[0] + "/" + monthNames[((value1.ctrlvalue[0])[1])[1]] + "/" + (value1.ctrlvalue[0])[2];
                                }
                                ////alert(value1.ctrlvalue[0]);
                                ////alert(datec);
                                output += "<div class='form-group'><label>" + value1.AttributeField + " </label><input type='text' id='" + value1.AttributeField + "' value='" + datec + "' class='form-control customfieldgroup datepicker' maxlength='10' readonly='readonly' tabindex='10'/></div>";
                                //output+=' <input  type="text" placeholder="click to show datepicker"  id="example1">';
                                //output += " $('.datepicker').datepicker({format: 'dd/mm/yyyy'})";
                                ////alert(output);
                            }
                            else
                            {
                                output += "<div class='form-group'><label>" + value1.AttributeField + " </label><input type='text' id='" + value1.AttributeField + "'class='form-control customfieldgroup datepicker' maxlength='20' readonly='readonly'  tabindex='10' /></div>";
                            }
                        }
                        else if (value1.AttributFieldType == "Multiple Line Text") {
                            output += "<div class='form-group'><label>" + value1.AttributeField + " </label><textarea id='" + value1.AttributeField + "' class='form-control customfieldgroup'  tabindex='10'>" + value1.ctrlvalue + "</textarea></div>";
                        }
                        else {
                            output += "<div class='form-group'><label>" + value1.AttributeField + " </label><input type='text' id='" + value1.AttributeField + "' value='" + value1.ctrlvalue + "'  class='form-control customfieldgroup' maxlength='20'  tabindex='10'/> </div>";
                        }

                    });
                   // output += "</div> <script> $('.datepicker').datepicker({ format: 'dd/M/yyyy',autoclose: true }); ";
                    output += "</div><script type='text/javascript'>  $(function () {var date = new Date(); date.setDate(date.getDate()); $('.datepicker').datepicker({ startDate: date, format: 'dd/M/yyyy', autoclose: true });});";
                    Cdata = Cdata.substr(0, Cdata.length - 1);
                    $('#<%=hidcustomfields.ClientID %>').val(Cdata);
                    cbid = cbid.substr(0, cbid.length - 1);
                    $('#<%=hidchkid.ClientID %>').val(cbid);
                    <%--     selid = selid.substr(0, selid.length - 1);
                     $('#<%=hidselid.ClientID %>').val(selid);--%>
                    //  ////alert(cbid); ////alert(selid);

                    //  ////alert($('#<%=hidcustomfields.ClientID %>').val())
                    // ////alert(output);
                    $('#divCF').html(output);
                }
            });
         //   $('#<%=lblmasg.ClientID %>').html(V1);
        $("#messageNotification").jqxNotification("open");
    }
        
    //testing end abhishek jaiswal
    function GetCustomFields() {
        //  ////alert("555");
        $('#spinner').show();
        var type2 = document.getElementById('<%= hidchk.ClientID%>').value;
            //hidcustomval
            // ////alert(type2.value);
            var type = "Contact";
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetCustomFieldsByAttrTable") %>',
                contentType: "application/json; charset=utf-8",
                data: '{AttrType: "' + type + '"}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    var output = "<div class='box-body'>";
                    output += " <div class='row'>";
                    //output += "<div class='col-lg-5 col-md-8 col-sm-8 col-xs-10'>";
                    output += "<div >";
                    var Cdata = ""; var cbid = ""; var selid = "";
                    $.each(jsdata1, function (key1, value1) {
                        //////alert(jsdata1);
                        Cdata += value1.AttributeField + "^";
                        // ////alert(Cdata);
                        if (value1.AttributFieldType == "Number") {
                            //////alert(value1.AttributeField);
                            // output += "<div class='form-group ><label>" + value1.AttributeField + ": </label><div class='col-md-6'><input type='text' id='" + value1.AttributeField + "' palcehoider='l' class='form-control numeric text-right customfieldgroup' maxlength='10' onkeypress='javascript:return isNumber (event)'/></div> </div>";
                            output += "<div class='form-group'><label>" + value1.AttributeField + ": </label><div><input type='text' id='" + value1.AttributeField + "' value='0'  tabindex='10'  class='form-control numeric text-right customfieldgroup' maxlength='10' onkeypress='return isNumberKey(event)'/> </div></div>";
                        }

                        else if (value1.AttributFieldType == "Dropdown" || value1.AttributFieldType == "Checkbox" || value1.AttributFieldType == "Sql View" || value1.AttributFieldType == "Sql Procedure") {
                            // ////alert(value1.AttributeField);
                            var arr = value1.AttributeData.split("*#");
                            //   ////alert(arr);
                            //   output += '<div class="form-group" id="' + value1.AttributeField + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                            if (value1.AttributFieldType == "Checkbox") {
                                cbid += "cb" + (value1.AttributeField).replace(/ +/g, "") + "@";
                                output += '<div class="form-group customfieldgroup" id="cb' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                for (var j = 0; j < arr.length; j++) {
                                    output += '<label><input type="checkbox" tabindex="10"  value="' + arr[j] + '"/>' + arr[j] + '</label>&nbsp;&nbsp;&nbsp;'
                                    // ////alert(output)
                                }
                                output += '</div>';
                                //////alert(output);
                            }
                            else if (value1.AttributFieldType == "Sql Procedure") {
                                {
                                    ////alert(value1.AttributeField);
                                    var dd = value1.AttributFieldType;
                                    //////alert(c);
                                    output += '<div class="form-group" id="sel' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                    selid += "sel" + value1.AttributeField + "@";
                                    // CreateOptionsVal(c);
                                    output += '<select class="form-control customfieldgroup"  id="' + (value1.AttributeField).replace(/ +/g, "") + '" name="' + value1.AttributeField + '" tabindex="10" >'
                                    // ////alert(output);
                                    output += '<option value="0" >Select</option>'
                                    //for (var j = 0; j < arr.length; j++) {
                                    //    output += '<option  >' + arr[j] + '</option>'
                                    //}
                                    output += '</select>'; output += '</div>';
                                    //////alert(c);
                                }
                            }
                            else if (value1.AttributFieldType == "Sql View") {
                                // ////alert(value1.AttributeField);
                                var dd = value1.AttributFieldType;
                                //////alert(c);
                                output += '<div class="form-group" id="sel' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                selid += "sel" + value1.AttributeField + "@";
                                // CreateOptionsVal(c);
                                //output += '<select class="form-control customfieldgroup" onchange="CreateOptionsVal(this);" id="' + (value1.AttributeField).replace(/ +/g, "") + '" name="' + value1.AttributeField + '" >'

                                output += '<select class="form-control customfieldgroup" onchange="FillInDropBox(this);" id="' + (value1.AttributeField).replace(/ +/g, "") + '" name="' + value1.AttributeField + '" tabindex="10"  >'
                                // ////alert(output);
                                output += '<option value="0" >Select</option>'
                                for (var j = 0; j < arr.length; j++) {
                                    output += '<option  >' + arr[j] + '</option>'
                                }
                                output += '</select>'; output += '</div>';
                                //////alert(c);
                            }
                            else {
                                // ////alert(value1.AttributeField);
                                var dd = value1.AttributFieldType;
                                //////alert(c);
                                output += '<div class="form-group" id="sel' + (value1.AttributeField).replace(/ +/g, "") + '"><label>' + value1.AttributeField + '</label>&nbsp;&nbsp;';
                                selid += "sel" + value1.AttributeField + "@";
                                // CreateOptionsVal(c);
                                output += '<select class="form-control customfieldgroup" id="' + (value1.AttributeField).replace(/ +/g, "") + '" name="' + value1.AttributeField + '" tabindex="10" >'
                                // ////alert(output);
                                output += '<option value="0" >Select</option>'
                                for (var j = 0; j < arr.length; j++) {
                                    output += '<option  >' + arr[j] + '</option>'
                                }
                                output += '</select>'; output += '</div>';
                                //////alert(c);
                            }


                        }
                        else if (value1.AttributFieldType == "Date") {
                            output += "<div class='form-group'><label>" + value1.AttributeField + " </label><input type='text' id='" + value1.AttributeField + "'class='form-control customfieldgroup datepicker' maxlength='20' readonly='readonly' tabindex='10' /></div>";
                            //output+=' <input  type="text" placeholder="click to show datepicker"  id="example1">';
                            //output += " $('.datepicker').datepicker({format: 'dd/mm/yyyy'})";

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
                    <%--     selid = selid.substr(0, selid.length - 1);
                     $('#<%=hidselid.ClientID %>').val(selid);--%>
                    //  ////alert(cbid); ////alert(selid);

                    //  ////alert($('#<%=hidcustomfields.ClientID %>').val())
                    // ////alert(output);
                    $('#divCF').html(output);
                }
            });
            $('#spinner').hide();
        }

        function CreateUrl() {
            debugger;
            var val = $('.ut').length + 1;
            var html = '<div class="row"  id="divUT' + val + '"><div class="col-md-8 col-sm-5 col-xs-12 "><div class="form-group ut"><label for="exampleInputEmail1">URL:</label> <input type="text"  class="form-control urltext" maxlength="50" id="Url' + val + '" placeholder="Enter URL" tabindex="5" /><input type="hidden" class="hidurlid"  value="" /></div></div><div class="col-md-4 col-sm-3 col-xs-12" ><div class="row"><div class="col-md-8" style="display:none"><label for="exampleInputEmail1">Type:</label>&nbsp;&nbsp;<select id="ddlurltype' + val + '" class="form-control urlddl" TabIndex="5">';
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetDataPhoneEmailUrl") %>',
                contentType: "application/json; charset=utf-8",
                data: '{AttrType: "WEB"}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);

                    $.each(jsdata1, function (key1, value1) {

                        //   output += '<label><input type="checkbox"  value="' + arr[j] + '"/>' + arr[j] + '</label>&nbsp;&nbsp;&nbsp;'
                        var split = value1.concatedvalue.split(",");


                        for (var i = 0; i < split.length; i++) {
                            html += '<option value="' + split[i] + '">' + split[i] + '</option>';
                            // html += '<option value="' + value1.Data + '">' + value1.Data + '</option>';
                        }
                        // ////alert(output)   
                        html += '</select></div> <div  style="padding-left:13px ;margin-top: 26px;"> <div class="form-group"><img id="imgaddurl" src="img/plus.png" alt="Add" onclick="CreateUrl();"> <img src="img/minus.png" alt="Remove" onclick="RemoveUrl(' + val + ',0);" /> </div> </div>  </div></div>';


                        $("#divUT" + $('.ut').length).after(html);
                    });


                }
            });

          
                                         
        }
        function RemoveUrl(id, urlid) {
            if (urlid == 0) {

            }
            else {
                if (confirm('Do you want to delete ?')) {
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("CRMDBService.asmx/removecontactdetail") %>',
                        contentType: "application/json; charset=utf-8",
                        data: '{Id:"' + urlid + '",Type:"U"}',
                        dataType: "json",
                        success: function (data) {
                            //  Successmessage("Deal Stage Deleted");

                        }
                    });
                }
            }
            $("#divUT" + id).remove();
        }
        function setjobtitle() {
            debugger;
            var parPT = $(".pt").parent().attr('class');
        

            $('.' + parPT + ' .phoneddl').each(function () {
                $(this).prop("selectedIndex", 0);
            })
        }
        function ValidMail(This)
        {
            var testEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
            if (testEmail.test(This.value)) {
            }
            else {
                errormessage("Please enter valid Email");
               
                This.value = "";
                $("#"+This.id).focus();
                return false;
            };
        }
        function Validphone(This) {
            var testEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
            if (This.value == "") {
            }
            else {
                errormessage("Please fill Contact Name");

                This.value = "";
                $("#" + This.id).focus();
                return false;
            };
        }
        function CreateEmail() {

            var val1 = $('.et').length + 1;
            //var html = '<div class="form-group ut" id="divUT' + val + '"><label for="exampleInputEmail1">URL:</label> <div class="clearfix"></div><div class="col-md-6 col-sm-8 col-xs-6 paddingleft0"><input type="text"  class="form-control urltext" maxlength="50" id="Url' + val + '" placeholder="Enter URL" tabindex="5" /> <br/><div class="row"><div class="col-lg-3 col-md-3 col-xs-3"><select id="ddlurltype' + val + '" class="form-control urlddl" TabIndex="7"><option selected="selected" value="Website">Website</option><option value="Blog">Blog</option><option value="Twitter">Twitter</option><option value="LinkedIn">LinkedIn</option><option value="Xing">Xing</option><option value="Facebook">Facebook</option><option value="Google+">Google+</option><option value="Other">Other</option></select></div> <div class="col-md-3 col-sm-3 col-xs-4"> <img src="img/plus.png" alt="Add" onclick="CreateUrl();" /> <img src="img/minus.png" alt="Remove" onclick="RemoveUrl(' + val + ');" /> </div>   </div>';
            var html = '<div class="form-group et" id="divET' + val1 + '">';


            html +='<div class="row"><div class="col-md-4 col-sm-4 col-xs-12">';
            html +='<div class="form-group"><label for="exampleInputEmail1">Contact Name:</label>';
            html +='<input type="text" runat="server" class="form-control EmailContact" maxlength="25" id="txtEmailcnt" placeholder="Enter Contact Name" tabindex="5" />';
            html +='</div></div><div class="col-md-4 col-sm-4 col-xs-12"> <div class="form-group"><label for="exampleInputEmail1">Email:</label>';
            html += '<input type="text"  class="form-control emailtext" maxlength="50" id="Email' + val1 + '" placeholder="Enter Email" tabindex="5" onchange="ValidMail(this);" />';
            html += ' <input type="hidden" class="hidEmailid"  value="" />';
            html += '</div></div>';
            html += '<div class="col-md-4 col-sm-2 col-xs-12" ><div class="row"><div class="col-md-8"><label for="exampleInputEmail1">Type:</label>&nbsp;&nbsp; <select id="ddlemailtype' + val1 + '" class="form-control emailddl" TabIndex="5">';
                
                
               




            //html +='<label for="exampleInputEmail1">Contact Name:</label><label for="exampleInputEmail1">Email:</label> ';
            //html +='<div class="clearfix"></div>';
            //html +='<div class="col-md-4 col-sm-4 col-xs-4" style="padding-left: 0">';
            //html +='<input type="text" runat="server" class="form-control EmailContact" maxlength="25" id="txtEmailconta" placeholder="Enter Contact Name" tabindex="5" /></div>';
            //html +='<div class="col-md-4 col-sm-4 col-xs-4 paddingleft0"><input type="text"  class="form-control emailtext" maxlength="50" id="Email' + val1 + '" placeholder="Enter Email" tabindex="5" /></div>';


           /// html += '<div class="row"><div class="col-md-3 col-sm-2 col-xs-2" style="padding-left: 0;display:none"><select id="ddlemailtype' + val1 + '" class="form-control emailddl" TabIndex="7">';

            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetDataPhoneEmailUrl") %>',
                contentType: "application/json; charset=utf-8",
                data: '{AttrType: "Email"}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);

                    $.each(jsdata1, function (key1, value1) {

                        //   output += '<label><input type="checkbox"  value="' + arr[j] + '"/>' + arr[j] + '</label>&nbsp;&nbsp;&nbsp;'
                        var split = value1.concatedvalue.split(",");


                        for (var i = 0; i < split.length; i++) {
                            html += '<option value="' + split[i] + '">' + split[i] + '</option>';
                            // html += '<option value="' + value1.Data + '">' + value1.Data + '</option>';
                        }

                        html += '</select></div><div>'; //<div class="col-lg-1 col-md-1 col-xs-1" style="padding-left: 13px;"> </div>  </div>
                        html +=' <div  style="    padding-left: 13px;    margin-top: 26px;">';
                        html += '<div class="form-group"><img id="imgaddEmail" src="img/plus.png" alt="Add" onclick="CreateEmail();"><img src="img/minus.png" alt="Remove" onclick="RemoveEmail(' + val1 + ',0);" /></div></div></div></div></div>';

                        $("#divET" + $('.et').length).after(html);
                    });


                }
            });

        }
        function RemoveEmail(id, emailId) {
            if (emailId == 0) {

            }
            else
            {
                if (confirm('Do you want to delete ?')) {
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("CRMDBService.asmx/removecontactdetail") %>',
                        contentType: "application/json; charset=utf-8",
                        data: '{Id:"' + emailId + '",Type:"E"}',
                        dataType: "json",
                        success: function (data) {
                          //  Successmessage("Deal Stage Deleted");

                        }
                    });
                }
            }
            $("#divET" + id).remove();
        }
        function FillCountry(ddl)
        {
            debugger;
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/getcountry") %>',
                contentType: "application/json; charset=utf-8",
                data: '{stateid:' + ddl.value + '}',
                dataType: "json",
                success: function (data) {
                //    alert('cccc');
                    jsdata1 = JSON.parse(data.d);
                    debugger;
                    $.each(jsdata1, function (key1, value1) {
                        debugger;
                        $('#<%=ddlcountry.ClientID%>').val(value1.countryid);
                        $('#<%=hidcontactcountry.ClientID%>').val(value1.countryid);

                    });
                    }
                    
            });
        }
        function Fillcountry1(ddl)
        {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/getcountry") %>',
                contentType: "application/json; charset=utf-8",
                data: '{stateid:' + ddl.value + '}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    debugger;
                    $.each(jsdata1, function (key1, value1) {
                        $('#<%=ddlcompcountry.ClientID%>').val(value1.countryid);
                        $('#<%=hidcompcountry.ClientID%>').val(value1.countryid);
                      
                    });
                }

            });
        }
        function CreatePhone() {
            var val2 = $('.pt').length + 1;

            var html;
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetDataPhoneEmailUrl") %>',
                contentType: "application/json; charset=utf-8",
                data: '{AttrType: "Phone"}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);

                    $.each(jsdata1, function (key1, value1) {
                        html = '<div class="form-group panel panel-default pt" id="divPT' + val2 + '"  style="padding-left: 10px;" >';


                        html += '<div class="row"> <div class="col-md-6 col-sm-6 col-xs-12"   ><div class="form-group">';
                        html +='<label for="exampleInputEmail1">Contact Name:</label>';
                        html += '<input type="text" runat="server" class="form-control PhoneContact" maxlength="25" id="ff1" placeholder="Enter Contact Name" tabindex="5"  onchange="Validphone(This);" /></div></div>';



                        html += ' <div class="col-md-5 col-sm-5 col-xs-12"><div class="form-group">';
                        html += '<label for="exampleInputEmail1">Job Title:</label>';
                        html +='<select id="ddlphonetype' + val2 + '" class="form-control phoneddl" TabIndex="5">';
                            
                            
                         
                        var split = value1.concatedvalue.split(",");

                        debugger;
                        for (var i = 0; i < split.length; i++) {
                            html += '<option value="' + split[i] + '">' + split[i] + '</option>';
                          
                        }
                        html += '</select></div></div>';


                      

                        html += '<div class="col-md-6 col-sm-6 col-xs-12">';

                        html += '<div class="form-group">';
                        html += '<label for="exampleInputEmail1">Phone:</label>';
                        html += '<input type="text" class="form-control numeric text-left phonetext" maxlength="15" id="Phone' + val2 + '"  placeholder="Enter Phone"  tabindex="5" />';
                        html += ' <input type="hidden" class="hidphoneid"  value="" />';   
                            
                                                    html += ' </div>  </div>'


                        html += ' <div class="col-md-6 col-sm-6 col-xs-12" ><div class="row">';
                        html += '<div class="col-md-10 col-sm-10 col-xs-12"><div class="form-group">';
                        html += '<label for="exampleInputEmail1">Email:</label>';
                          html += ' <input name="Email1" type="text" id="Email' + val2 + '" class="form-control emailtext" maxlength="50" placeholder="Enter Email" tabindex="5" onchange="ValidMail(this);">';

                        html += '</div></div>';

                        html += '<div  style="padding-left: 13px;margin-top: 26px; ">';
                        html += '<img id="imgaddphone" src="img/plus.png" alt="Add" onclick="CreatePhone();">';
                        html += ' <img src="img/minus.png" alt="Remove" onclick="RemovePhone(' + val2 + ',0);" /></div></div></div></div></div>';
                        html +='<script> $(".numeric").numeric() ';
                       // <div class="col-lg-1 col-md-1 col-xs-1" style="padding-left: 13px"> <img src="img/minus.png" alt="Remove" onclick="RemovePhone(' + val2 + ');" /></div>

                        $("#divPT" + $('.pt').length).after(html);
                    });



                }
            });






        }
        function RemovePhone(id, phoneId) {
            if (phoneId == 0) {

            }
            else {
                if (confirm('Do you want to delete ?')) {
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("CRMDBService.asmx/removecontactdetail") %>',
                        contentType: "application/json; charset=utf-8",
                        data: '{Id:"' + phoneId + '",Type:"P"}',
                        dataType: "json",
                        success: function (data) {
                            //  Successmessage("Deal Stage Deleted");

                        }
                    });
                }
            }
            debugger;
            $("#divPT" + id).remove();
        }
    </script>
    <!-- SlimScroll -->




    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();

        });
    </script>
    <script type="text/javascript">

        $(document).ready(function () {

            //$('#example1').datepicker({
            //    format: "dd/mm/yyyy"
            //});

            //$('.datepicker').datepicker({
            //    format: "dd/mm/yyyy"
            //});

        });
    </script>


                                                    </label>



</asp:Content>
