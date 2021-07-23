<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="CRMTask.aspx.cs" Inherits="AstralFFMS.CRMTask" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">

    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js"></script>

    <script src="plugins/jquery.MultiFile.js"></script>



    <style type="text/css">
        /*td.disabled:before
        {
              position:relative;
    content: '';
    background: darkgray;
    display: block;
    width: 75%;sa
    height: 3px;
    -webkit-transform: rotate(-45deg);
    transform: rotate(-45deg);
    left: 0;
    right: 0;
    top: 11px;
    bottom: 0;
    margin: auto;
        }*/
        /*td.disabled:after
        {
           position:relative;
    content: '';
    background: darkgray;
    display: block;
    width: 75%;
    height: 3px;
    -webkit-transform: rotate(45deg);
    transform: rotate(45deg);
    left: 0;
    right: 0;
    top: -11px;
    bottom: 0;
    margin: auto;
    
        }*/
        .disabled
        {
            text-decoration:line-through;
        }
        .modal-content{
            border-radius:14px;
        }
        .modal-header {
          
            background-color: #367fa9;
            color: white;
            border-top-left-radius:14px;
            border-top-right-radius:14px;
        }
        i.glyphicon.glyphicon-info-sign.viewdetail {
            top: 3px;
            LEFT: 8px;
        }
        .box-view
        {
          border: 1px solid green;
          position:absolute;
          color: white;
          top: 19px;
          left: 30px;
          background-color: black;
        }
        .hide
        {
            display:none;
        }
        .paddingtop {
            padding-top: 10px;
        }

        @media screen and (max-width: 480px) {
            .paddingtop {
                padding-top: 0px;
            }
        }

        .bottom10 {
            padding-bottom: 10px;
        }

        .bigcontainer {
        }


        .tr {
        }

            .tr:hover {
                background-color: #e7ecef;
            }



        .datebgimage1 {
            background: url(img/bg.png) no-repeat;
            padding-right: 34px;
            color: white;
            padding-left: 18px;
            font-size: 11px;
            width: 30px;
            /*background-size: 100px 100px;*/
        }

        .datebgimage2 {
            background: url(img/bgorange.png) no-repeat;
            padding-right: 34px;
            color: white;
            padding-left: 18px;
            font-size: 11px;
            width: 30px;
            /*background-size: 80px 80px;*/
        }

        .datebgimage3 {
            background: url(img/bgred.png) no-repeat;
            /*background: url(img/bgorange.png) no-repeat;*/
            padding-right: 34px;
            color: white;
            padding-left: 18px;
            font-size: 11px;
            width: 30px;
            /*background-size: 80px 80px;*/
        }

        .background {
            margin-top: 12px;
        }

        .aVis {
            display: none;
        }

        .showbg {
            background-color: rgb(189, 228, 190);
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
            width: 150px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }
        .spinnerfile
        {
             position: absolute;
            top: 80%;
            left: 10%;
            margin-left: -10px; /* half width of the spinner gif */
            margin-top: -80px; /* half height of the spinner gif */
            text-align: left;
            z-index: 999;
            overflow: auto;
            width: 150px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }

        .tdcls {
            padding: 3px;
        }


        td {
            border: 0px solid !important;
            padding: 0px;
        }

        .bgwhite {
            padding: 8px;
            background-color: white;
            margin-bottom: 10px;
        }

        .paddingleft0 {
            padding-left: 0px;
        }

        .paddingright0 {
            padding-right: 0px;
        }

        .padding0 {
            padding: 0px;
        }

        .leadinfo_fontfamily {
            font-family: sans-serif;
            font-size: 12px;
        }

        .bold {
            font-weight: 600;
            font-family: sans-serif;
        }

        .spanaction {
            font-weight: 600;
            padding: 5px;
            display: inline-block;
            background-color: #fff;
            border-radius: 4px;
            margin-bottom: 12px;
        }
    </style>
       <script type="text/javascript">
           
           $(window).load(function () {
               //$('body').css('display', 'block');
               //alert('qwe');
               startTime();
           });
           function startTime() {

               var date = new Date();
               var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
               var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
               var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
            //   alert(hours);
               time = hours + ":" + minutes ;
               var lblTime = document.getElementById("txttime");
               lblTime.defaultValue = time;
               //t = setTimeout(function() {
               //    startTime()
               //}, 500);
           }
           function checkTime(i) {
               if (i < 10) {
                   i = "0" + i;
               }
               return i;
           }
           //radio button for email and phone
           function MyAlert()  
           {  
              
               //var divcallemail = document.getElementById("divcallemail");
               //var divemail = document.getElementById("divemail");
               if (document.getElementById("rdcall").checked == true )  
               {  
                   document.getElementById("divemail").style.display = "none";
                   document.getElementById("divcallemail").style.display = "block";
               }  
               if (document.getElementById("rdemail").checked == true )  
               {  
                   document.getElementById("divemail").style.display = "block";
                   document.getElementById("divcallemail").style.display = "none ";
                   document.getElementById("divcallemail").style.height = "0px important";
               } 
               
               //var chkYes = document.getElementById("rdcall");
               //var dvPassport = document.getElementById("rdemail");
               //dvPassport.style.display = chkYes.checked ? "block" : "none";
           }  
           function generatepreview(input)
           {
               debugger;
               selDiv = document.querySelector("#selectedfile");
               //  //debugger;
               var filename="";
               var uploadedFiles = $('#ContentPlaceHolder1_uploader')[0].files;
               if (uploadedFiles.length > 0) {
                   //var formData = new FormData();
                   for (var i = 0; i < uploadedFiles.length; i++) {
                       filename=uploadedFiles[i].name ;
                       selDiv.innerHTML += uploadedFiles[i].name + '&nbsp;&nbsp;<img src="img/cross.jpg" alt="Remove" style=" max-width: 15px;" onclick="removepic(' + i + ');" /><br/>';
                   }
                   
                
               }
           }
           function removepic(j)
           {
               debugger;
               var uploadedFiles = $('#ContentPlaceHolder1_uploader')[0].files;
               for (var i = 0; i < uploadedFiles.length; i++) {
                   if (uploadedFiles[i].name == uploadedFiles[j].name)
                   {
                       uploadedFiles[i].clear();
                   }
               }
           }
           var arr;
           var DealID;
           var arrfilename="";
           var filelogicalname="";
         $(document).ready(function () {
               $('#btnUpload').click(function (event) {
                   debugger;
                  //}).fadeIn(1000);
               });
           });
       </script>
   
    <script type="text/javascript">
    
     
                 function checkhistory(c) {
                     //debugger;
                     $('#listid').val(c.id);
                     //alert($('#listid').val());
                     var DDLValue = c.id;
                     //  alert(DDLValue);
                     var li = document.getElementById(DDLValue)
                     // alert(li.textContent);
                     if (DDLValue == "All") {
                         //alert("11");
                         //alert(DDLValue+"1");
                         $('#taskdetailstetsing1').html('');
                         $('#taskdetailstetsing4').html('');
                         $('#taskdetailstetsing2').html('');
                         $('#taskdetailstetsing3').html('');
                         GetCompleteHis(0);
                         //GetTaskDetails(0);
                         //alert(DDLValue+"2"); 
                         //GetTaskNotes(0);
                         //alert(DDLValue+"3");
                         //GetTaskDeals(0);
                         //GetTaskCall(0);
                     }
                     if (DDLValue == "Notes") {

                         $('#taskdetailstetsing2').html('');
                         $('#taskdetailstetsing3').html('');
                         GetTaskNotesCall(0);
                         //GetTaskNotes(0);
                         //GetTaskCall(0);
                     }
                     if (DDLValue == "Deals") {

                         $('#taskdetailstetsing1').html('');
                         $('#taskdetailstetsing4').html('');
                         $('#taskdetailstetsing2').html('');
                         GetTaskDeals(0);
                     }
                     if (DDLValue == "Actions") {

                         $('#taskdetailstetsing1').html('');
                         $('#taskdetailstetsing4').html('');
                         $('#taskdetailstetsing3').html('');
                         GetTaskDetails(0);
                     }
                 }
                 function GetDatabyDocId(DocId)
                 {
                     DocId= DocId.split(" ").join("") ;
                     
                     $.ajax({
                         type: "POST",
                         url: '<%= ResolveUrl("CRMDBService.asmx/gettaskbydocid") %>',
                         contentType: "application/json; charset=utf-8",
                         data: '{DocId : "' + DocId + '"}',
                         dataType: "json",
                         success: function (data) {
                             jsdata1 = JSON.parse(data.d);
                             var output = '<table class="table"><tbody>';
                             output += '<tr class="tr" style="font-weight:Bold;color:#005580;font-size:16px"><td>Lead</td><td>Task Description</td><td>Owner</td><td>Lead Status</td><td>Task Status</td><td>Tag</td>';
                             output += '<tr class="tr" style="width:1px;background-color:#f5f5f5"><td></td><td> </td><td></td><td> </td><td> </td><td></td>';
                             $.each(jsdata1, function (key1, value1) {
                                 var colr = "Green";
                                 if (value1.Tstatus == "Close") {
                                     colr = "Red";
                                 }
                       
                                 var dt = new Date(value1.assgndate1);
                                 var nextWeek = new Date(value1.weekdt);
                                 var curr = new Date(value1.currentdt);
                                 var datebgimage4 = "datebgimage2";
                                 var colrdt = "orange";
                              
                                 if (value1.currentdt > value1.assgndate1)
                                 {
                                     colrdt = "black";
                                     datebgimage4 = "datebgimage3";
                                 }
                                 else if (value1.currentdt < value1.assgndate1)
                                 {
                                     datebgimage4 = "datebgimage1";
                                 }
                                 else
                                 {
                                     datebgimage4 = "datebgimage2";
                                 }
                                 if (value1.Flag == "T") 
                                 {
                                     output += '<tr class="tr"> <td style="cursor:pointer"> <a onclick="calltabtask(' + value1.contact_id + ');" ><span><img src="img/star.png">&nbsp; <b>' + value1.compname + ' </b>&nbsp;(' + value1.NAME + ')</span></a></td><td><span class=' + datebgimage4 + '><span class="tabledate"> ' + value1.assgndate + '</span></span style="color:' + colrdt + '">' + value1.Task + '</td><td><b>' + value1.Towner + '</b></td><td></td><td><label style="color:' + colr + '">' + value1.Tstatus + '</label></td> <td><label ></label></td></tr>';
                                 }
                                 else 
                                 {
                                     output += '<tr class="tr"> <td style="cursor:pointer"> <a onclick="calltab(' + value1.contact_id + ');" ><span><img src="img/star.png">&nbsp; <b>' + value1.compname + ' </b>&nbsp;(' + value1.NAME + ')</span></a></td><td><span class=' + datebgimage4 + '><span class="tabledate"> ' + value1.assgndate + '</span></span style="color:' + colrdt + '">' + value1.Task + '</td><td><b>' + value1.Towner + '</b></td><td>' + value1.status + '</td><td><label style="color:' + colr + '">' + value1.Tstatus + '</label></td> <td><label >' + value1.Tag + '</label></td></tr>';
                                 }
                             });
                             output += " </tbody></table>";
                             $('#showdata').html(output);
                             $("#showdata td a").click(function () {

                                 $('.box').css('display', 'none');
                        
                                 $('.spinner').show();
                                 $('#spinnerfile').hide();

                                 $(document).ajaxComplete(function () {
                                     $('.spinner').hide();
                                     $('.box').css('display', 'block');
                                     $("#divbody").hide();
                                 });
                             });


                         }
                     });
                     
                     
                 }
                 $(document).ready(function () {
                     var value = "0";
                     debugger;
                   
                     GetData(0,0,DocId); 
                    
                     
                     
                     GetOwners(); GetUserForFilter();
                     var username = '<%=Session["user_name"]%>';

                      
                         var leadallow = SetPermissions("Add Lead", "");
                         var Perallow = SetPermissions("Permission", "");
                         var Taskallow= SetPermissions("Add Task", "");
             
                         if (leadallow == "true")
                         {

                         }
                         else
                         {
                       
                             $("#btnaddlead").addClass("hide");
                             $("#btnaddlead1").addClass("hide");
                           
                         }
                         if (Perallow == "true")
                         {

                         }
                         else
                         {
                             $("#btnPermission").addClass("hide");
                         }
                         if(Taskallow == "true" )
                         {

                         }
                         else
                         {
                             $("#btntask").addClass("hide");
                         }
                     //}
                     var Msz = '<%= Session["Insertmsg"] %>';
         
             if ((Msz != 'undefined') && (Msz != "")) {
             
                 var Msz1 = null;
                 '<%Session["Insertmsg"] = "' + Msz1 + '"; %>';
                 ('<%=Session["Insertmsg"] %>');
             }



         });

        $(function () {
            var date = new Date();
            date.setDate(date.getDate());
            $(".datepicker").datepicker({ startDate: date, format: 'dd/M/yyyy', autoclose: true });
            $("#txtnxtactndt1").datepicker('setDate', date);
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



       


        function NewTask()
        {
            //debugger;
            $('#divLead').hide();
            $('#divnav').show();
            $('.nav.nav-tabs li').hide();
            //$('.nav.nav-tabs li:last-child()').show();
            activaTab('add_action_show');
          //  $('.nav.nav-tabs li:last-child()').addClass('active')
            $('#divhis').hide();
            $('#divcontactDetail').hide();
            RefId = 'T';
          
            $("#table").hide();
            $("#details").show();
             $("#div3").hide();
          
             $("#divbody").css("background-color", "#fffff");///#ecf0f5
             $("#divbody").hide();
            $("#user_profile_off").hide();
            $("#user_profile_on").show();
            $("#user_menu").show();
            $("#bg_conatct_created").show();
           // $("#divhis").show();
           // $("#divtask").show();
            $("#taskdetailstetsing").show();
            //$("#background_comment").show();
            $(".bigcontainer").show();
            $("#compamy_members").hide();
            // $("#divbody").hide();
            $("#conatct_created").hide();
            $("#more").show();

        }
    </script>
    <%-- <script>
        $(document).ready(function () {
            $('#lblcrmcompname').mouseover(function () {
                $('#div_descdisplay').css("Display", "Block");
            });
            $('#lblcrmcompname').mouseout(function () {
                $('#div_descdisplay').css("background-color", "lightgray");
            });
        });
</script>--%>
    <script>
        function Visible_Desc(x) {
     var div = document.getElementById('div_descdisplay')
     div.style.display = "Block";
 }

            
    function Hide_desc(x) {
            
        var div = document.getElementById('div_descdisplay')
            
        div.style.display = "None";
            
    }
            
    </script>
    <section class="content">
        <!-- Content Header (Page header) -->
         <div id="spinnerfile" class="spinner" style="display: none;">
            <img id="img-spinnerfile" src="img/loader.gif" alt="Loading" /><br />
            Uploading Files....
        </div>
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
        </div>
        
        <%--<div class="box box-body" style="width:80%;background-color:#ecf0f5">--%>
       
        <div class="box box-body" style="width: 100%;" id="divbody">
            <%--background-color:#ecf0f5--%>
                 <div id="table" style="display: block;width:100%">
                <div class="clearfix"></div>
                <div class="col-md-2 pull-left" >
                    <a href="CRMTask.aspx"><i style="padding-left: 9px;" class="fa fa-refresh" aria-hidden="true">&nbsp;Refresh</i> </a>
                    <br>
                </div> 
                     <div class="col-md-3 col-xs-12 pull-left" >
   
      
                     <div class="col-md-2 col-xs-12 pull-left" style="
    /* float: right !important; */
    /* margin-left: 20px; */
">
                  <label>Date:</label>
                </div>
                     <div class="col-md-8  pull-left">
                         <input type="text" id="txtnxtactndt1" class="form-control datepicker" onchange="GetData(0,this,DocId)">
                     </div>
                             </div> 
                     <div class="col-md-4 col-xs-12 pull-left">
                         <div class="col-md-2" >
                             <label>Owner:</label>


                         </div>
                         <div class="col-md-6" >
                             <select id="lstuser" class="form-control"  onchange="GetData(this,0,DocId)">
                             </select>
                         </div>
                     </div>  
             <div class="col-md-3 col-xs-12 pull-right" style="margin-bottom:12px">
                    <a href="CRMContact.aspx" onclick="location.href=this.href+'?Contact_Id='+opt;return false;" id="btnaddlead1" class="btn btn-primary">Add Contact
                       <%-- <asp:Button type="btnfind1" ID="Button1" runat="server" Text="Add Lead" class="btn btn-primary" />--%>
                    </a>
                     
                 <a onclick="NewTask();" id="btntask"  class="btn btn-primary">Add Task
                   <%--  <asp:Button ID="Button4" runat="server" Text="Add Task" class="btn btn-primary"  OnClientClick="return false;"/>--%>
                 </a>
                    
                </div>
                     
                      <div class="clearfix"></div>
                     <div class="table-responsive" id="showdata">
                     </div>
            </div>
            <!--complete table -->

        
              
            </div>


            <!--table responsive end -->

            <!--details portion -->
            <div id="messageNotification">
                <div>
                    <asp:Label ID="lblmasg" runat="server"></asp:Label>
                </div>
            </div>

            <div style="display: none;" id="details">

                <div class="col-md-12 col-sm-12 col-xs-12 bgwhite" id="div1">
                    <div class="col-md-3 col-sm-2 col-xs-2 paddingleft0">
                        <%-- <div style="background-color:white; min-height:29px;" class="col-lg-12 col-md-9 bg no-padding">
           <div style="padding:3px 5px;" class="col-lg-2 col-sm-2 col-md-2 pull-left">--%>
                        <asp:Button runat="server" ID="btnbacktest" Text="Back" class="btn btn-primary" />
                        <%--<button id="backBtn" class="glyphicon glyphicon-share-alt btnicon btn-primary"></button>--%>
                    </div>
                    <div class="col-md-3 col-sm-10 col-xs-10 pull-right" id="div3">
                        <%--<div style="padding:3PX 27PX;" class="col-lg-2 col-md-3 col-sm-3 col-md-3 pull-right ">--%>
                        <%-- <button class="glyphicon glyphicon-chevron-up btnicon btn-primary"></button>
              <button class="glyphicon glyphicon-chevron-down btnicon btn-primary"></button>--%>
                        <a href="CRMContact.aspx" onclick="location.href=this.href+'?Contact_Id='+opt;return false;"  class="btn btn-primary" id="btnaddlead">Add Contact
                        <%--    <asp:Button type="btnfind1" ID="Button2" runat="server" Text="Add Lead" class="btn btn-primary "  />--%>
                        </a>

                           <a href="CRMOwnerPermission.aspx" onclick="location.href=this.href+'?Contact_Id='+opt1;return false;" class="btn btn-primary" id="btnPermission">Permission  <%--''' onclick="location.href=CRMOwnerPermission.aspx+'?Contact_Id= $('#hidcontactId').val();return false;"--%>
                          <%--  <asp:Button type="btnfind1" ID="Button3" runat="server" Text="Permission" class="btn btn-primary " />--%>
                        </a>
                        <br>
                    </div>

                </div>





                <div class="clerfix"></div>
                <br>
                <br/>


                <div style="display: block;" id="user_profile_off">
                    <div class="col-md-2 col-sm-3 col-xs-6">

                        <img src="img/unknown.jpg" width="123px">
                    </div>

                    <div style="padding-bottom: 10px;" class="col-md-4 col-sm-5 col-xs-6">
                        <h2 class="userh1"><b>USER Profile for Off Switch</b> <span>
                            <img src="img/star.png"><br>
                            <span>
                                <h5 style="color: #b1b0ae"><i>Compay Createc on 2aug 2016 latest activity , october 2oct </i></h5>
                            </span></span></h2>
                        <br>
                        <address>This is Addres area  </address>
                        <!-- user more info -->
                        <span><a data-toggle="modal" data-target="#company_description" href="javascript();">Add Company Description</a></span>
                        <!-- user more info -->
                        <div class="clearfix"></div>


                        <div class="col-md-6 col-sm-6 no-padding">
                            <div class="col-md-12 no-padding">Total Won </div>
                            <div class="col-md-12">Rs.234</div>
                        </div>
                        <div class="col-md-6 col-sm-6 no-padding">
                            <div class="col-md-12 no-padding">Total Pending </div>
                            <div class="col-md-12">675</div>
                        </div>
                        <div class="clearfix"></div>

                    </div>
                    <!-- danish -->

                </div>




                <div id="user_profile_on" style="display: none;">
                </div>
                <!-- danish -->
                <!--  <div class="col-md-2 col-sm-3 col-xs-6">

          <img src="img/unknown.jpg" width="123px">
          
          
      </div>
       -->
                <div>

                    <div class="col-md-12 bgwhite" id="divLead">
                        <div class="row">
                            <div class="col-md-4">
                                <%--<div class="col-md-4 col-sm-5 col-xs-6" style="float:left">--%>
                                <%--<h4 class="userh1"><label id="lblcrmusername"></label></h4>--%>
                                <p id="p1" style="font-size: 31px;    margin-left: 20px;">
                                </p>
                                <p id="contactadd" style="    margin-left: 20px;"></p>
                            
                                <%--   <p style="font-size:31px;"><label id="lblcrmusername"></label>
        </p>--%>
                            </div>
                            <div class="col-md-8 paddingleft0 pull-right">
                                <div class="col-md-3 col-xs-11" style="float: right;">
                                    <div class="form-group">
                                        <label>
                                            Status:
                                        </label>
                                        <select id="myselectStatus" style="width: 140px; height: 25px;" name="myselectStatus" class="form-control select2" onchange="changestatus(this.value)"></select>
                                        <input type="hidden" id="hidcontactId" />
                                    </div>
                                </div>


                                <div class="col-md-3  col-xs-11" style="float: right;">
                                    <div class="form-group">
                                        <label>
                                            Tag:
                                        </label>
                                        <select id="myselectTag" style="width: 140px; height: 25px;" name="myselectTag" class="form-control select2" onchange="changeTag(this.value)"></select>
                                        <input type="hidden" id="hidcontactId1" />
                                    </div>
                                </div>

                                <div class="col-md-4  col-xs-11" style="float: right;">
                                    <div class="form-group">
                                        <label>
                                            Owner:
                                        </label>
                                        <select id="myOwnerList" style="width: 200px; height: 25px;" name="myOwnerList" class="form-control select2" onchange="changeOwner(this.value)"></select>
                                        <input type="hidden" id="hidcontactId2" />
                                    </div>
                                </div>
                            </div>


                        </div>
                    </div>

                    <div class="col-md-12  bgwhite" id="divcontactDetail">
                        <div class="col-md-5 col-sm-4" style="padding: 28px 0px">
                            <%--               
         <div class="col-md-3">
    <span class="leadinfo_fontfamily bold">Lead Name :
    </span> 
  </div>  
  <div class="col-md-6">
    <span class="leadinfo_fontfamily"> Mr. Sanjeev Gupta 
    </span>
  </div>--%>
                            <div class="clearfix">
                            </div>
                            <div class="col-md-12">
                                <span class="leadinfo_fontfamily bold">Company   :
                                </span>
<%--<%--                          </div>--%>
                        <%--    <div class="col-md-6 paddingleft0">--%>
                                <%--<h5 style="color: #337ab7" onmouseover="Visible_Desc(this)" onmouseout="Hide_desc(this)">--%>
                                <span class="leadinfo_fontfamily" style="margin-left:14px;">
                                    <label id="lblcrmcompname" title="Company Name" style="margin: 0px"></label>
                                </span>
                                <%--</h5>--%>
                            </div>
                            <div class="clearfix"></div>
                          <%--  <div class="col-md-2">--%>
                              <div class="col-md-12">
                                <span class="leadinfo_fontfamily bold">Address   :
                                </span>
                          <%--  </div>--%>
                         <%--   <div class="col-md-6 paddingleft0">--%>
                                <%--<h5 style="color: #337ab7" onmouseover="Visible_Desc(this)" onmouseout="Hide_desc(this)">--%>
                                <span class="leadinfo_fontfamily" style="margin-left:20px;">
                                    <label id="lblcrmadd" title="Address" style="margin: 0px"></label>
                                </span>
                                <%--</h5>--%>
                            </div>

                            <div class="clearfix"></div>
                            <div class="col-md-12">
                                 <span class="leadinfo_fontfamily bold">City   :
                                </span>
                                <span class="leadinfo_fontfamily" style="margin-left: 45px;">
                                    <label id="lblcrmcity" title="City" style="margin: 0px"></label>
                                </span>
                            </div>
                             <div class="clearfix"></div>
                           
                              <div class="col-md-12">
                                     <span class="leadinfo_fontfamily bold">State   :
                                </span>
                                    <span class="leadinfo_fontfamily" style="margin-left: 37px;">
                                  <label id="lblcrmstate" title="State" style="margin: 0px"></label>
                                        </span>
                                  </div>
                              <div class="clearfix"></div>
                            
                              <div class="col-md-12">
                                    <span class="leadinfo_fontfamily bold">Country   :
                                </span>
                                    <span class="leadinfo_fontfamily" style="margin-left: 20px;">
                                  <label id="lblcrmcountry" title="Country" style="margin: 0px"></label>
                                          </span>
                                  </div>
                            <%--<div class="col-md-6">
          <span class="leadinfo_fontfamily bold">Contact No. :
          </span>
      </div>--%>
                            <%--<div class="col-md-6">--%>
                            <%--<h5 style="color: #C70039">--%>
                            <%--  <span class="leadinfo_fontfamily">
              <label id="lblcrmmbl" title="Contact No." style="color:#C70039"></label>
            </span>--%>
                            <%--</h5>--%>
                            <%--</div>
       <div class="col-md-6">
          <span class="leadinfo_fontfamily bold">Email :
          </span>
      </div>
      <div class="col-md-6">--%>
                            <%--<h5 style="color: #E91E63">--%>
                            <%--  <span class="leadinfo_fontfamily">
              <label id="lblcrmemail" title="Email Address" style="color: #E91E63"></label>
            </span>--%>
                            <%--</h5>--%>
                            <%-- </div>--%>
                            <div class="col-md-2" style="display:none">
                                <span class="leadinfo_fontfamily bold">Website :
                                </span>
                            </div>
                            <div class="col-md-6 paddingleft0" style="display:none">
                                <%--<h5 style="color: #FF336E">--%>
                                <span class="leadinfo_fontfamily">
                                    <a id="webadd" href="" target="_blank">

                                        <%--  style="color: #C70039"--%>

                                        <label id="lblcrmurl" title="Click To Open Website" style="margin: 0px"></label>
                                    </a><%--</h5>--%>
                                </span>
                            </div>
                            <%--<i class="glyphicon glyphicon-print"></i>&nbsp;Vcard<br><br>--%>
                            <!-- user more info -->
                            <div id="address" style="display: none;" class="address expandable item">
                                <div class="inside">
                                    <span>
                                        <!-- react-text: 59 -->
                                        25 Mason Street<!-- /react-text --><br>
                                    </span><span>
                                        <!-- react-text: 62 -->
                                        San Francisco<!-- /react-text --><br>
                                    </span><span>
                                        <!-- react-text: 65 -->
                                        CA 94102<!-- /react-text --><br>
                                    </span><span>United States</span><img alt="Search in Google Maps" src="https://cdn-assets.onepagecrm.com/assets/map-c38d05423b129eaaea2c07630a62997ce2b0343cad436d2a85dc9908c68d1bd0.png">
                                </div>
                            </div>

                            <!-- end user more info -->
                            <%--</div>--%>
                            <%--</div>--%>
                        </div>

                        <div class="col-md-3 col-sm-4">
                            <div id="divtable" class="table-responsive">
                            </div>
                        </div>
                         <div class="col-md-2 col-sm-2">
                            <div id="divtable1" class="table-responsive">
                            </div>
                        </div>
                         <div class="col-md-2 col-sm-2">
                            <div id="divurls" class="table-responsive">
                            </div>
                        </div>



                        <div id="div_descdisplay" class="col-md-4 col-sm-5 col-xs-6" style="float: left; display: none">
                            <h4><b>Company Description</b></h4>
                            <h5 class="userh1" style="color: #E91E63" id="lblcompdesc"></h5>
                        </div>
                    </div>

                    <%--</div> <!-- danish -->--%>





                    <div style="padding-top: 27px; display: none" class="col-md-4 col-sm-3 col-xs-12 paddingleft0 paddingright0 pull-right">
                        <ul style="padding: 0 40px" class="list-inline">
                            <li><a data-toggle="modal" data-target="#myModal" href="javascript();"><i class="glyphicon glyphicon-pencil"></i>Edit</a></li>
                            <li class="btn-group" id="toggle_event_editing">

                                <button id="on" type="button" class="btn unlocked_active btn-info">
                                    <img src="img/view.png" /></button>



                                <button id="off" type="button" class="btn locked_inactive btn-default">off</button>
                            </li>
                        </ul>

                        <%--  <span class="col-md-11 pull-right" id="more" style="display: none;"><a href="javascript:;"><span class="caret pull-leftMore"></span>More</a></span>--%>
                    </div>

                </div>
                <div class="clerfix"></div>
                <div id="user_menu" class="col-md-12 col-sm-12 col-xs-12 padding0" style="display: none;">


                    <div class="clearfix"></div>
                    <%--  Set Status To :
         <select id="myselectStatus" style="width:140px;height:25px;" name="myselectStatus" class="form-control select2" onchange="changestatus(this.value)"></select>
        <input type="hidden" id="hidcontactId" />--%>

                    <%-- <ul style="border-top:2px solid #e0deda;padding-top: 10px;" class="list-inline">--%>
                    <%--   <li class="dropdown">
                         <a href="" class="dropdown-toggle" data-toggle="dropdown">
                          Username
                            <span class="caret pull-left"></span>
                         </a>
                         <!-- Sub menu -->
                           <ul class="dropdown-menu animated fadeInUp">

                            <li><a id="usernames" href="javascript:;">All (3)</a></li>
                            <li><a href="#">Me (5)</a></li>
                            <li><a href="#">Sanjeev (3)</a></li>
                            <li><a href="#">Add User</a></li>

                          </ul>

                            </li>--%>

                    <%--	 <li class="dropdown">
                            <a href="" class="dropdown-toggle" data-toggle="dropdown">
                             Set Status To:
                            <span class="caret pull-left"></span>
                            </a>
                         <!-- Sub menu -->
                           <ul id="ulstatus" class="dropdown-menu animated fadeInUp">

                         <%--   <li><a href="#">All (3)</a></li>
                            <li><a href="#">Me (5)</a></li>
                            <li><a href="#">Sanjeev (3)</a></li>
                            <li><a href="#">Add User</a></li>--%>

                    <%--  </ul>--%>

                    <%--</li>--%>
                    <%-- <li class="dropdown">
                             <a id="tags" href="" class="dropdown-toggle" data-toggle="dropdown">
                              Tags
                            <span class="caret pull-left"></span>
                             </a>
                           

                            </li>--%>
                    <%-- </ul>--%>

                    <div class="clearfix"></div>


                    <%--<div style="display:none;" id="formtagform">
	    <form>
		
		<input style="margin-bottom:5px;" type="text" class="form-control col-md-12- col-sm-12 col- xs-12"><br>
		<button type="button" id="tagsave" value="close" class="btn btn-primary"> save</button>
		
		</form>
</div>--%>
                    <%--<br>--%>
                    <!-- profiel middle se tion -->
                    <div class="clearfix"></div>
                    <%--<div id="edit_content" style="background-color:#d7dfe4">
        <div style="max-width:730px; margin:0 auto;"><br>
           <div class="col-md-3 pull-left">
               <b class="spannext">Next action
			   </b>

           </div>

            <div class="col-md-2 pull-right"><a class="" id="edit_content_button"><i class="glyphicon glyphicon-pencil"></i></a> &nbsp;&nbsp; <button id="btn_add_question" class="btn btn-sm btn-primary btn-circle"><i class="fa fa-plus"></i></button>

               </div>

            <div class="clearfix"></div><br>

           


             <div class="col-md-12 col-sm-9 col-xs-9"><span style="font-weight:bold; font-size:16px;"> Meeting Scheduled for further discussions</span> 
            

          </div>

            <div class="clearfix"></div>
            <br>



             </div><!-- max width 730px-->

    </div>--%><!-- background color -->


                    <!-- edit content show -->

                    <div class="col-md-12 col-sm-12 col-xs-12 bgwhite " style="display:none;">
                        <%--<div id="edit_show" style="background-color:#b1d1e4; display:block;">--%>

                        <%--  <div class="clearfix"></div><br>--%>
                        <div class="col-md-12" style="background: #f4f4f4; border-radius: 9px;">
                            <%--<div style="max-width:730px;height:175px; margin:0 auto;"><br>--%>
                            <div class="col-md-4">
                                <%--<div class="col-md-2 pull-left">--%>
                                <h3>Action
                                </h3>
                            </div>

                            <%--<div class="col-md-6 paddingleft0 pull-right">--%>
                            <%--        <div class="col-md-4" style="float: right;">
                    <div class="form-group">
                        <%--<div class="col-md-3 pull-right">
                            Set Task Status To:                             
                    <select id="myselectTaskStatus" style="width: 140px; height: 25px;" name="myselectTaskStatus" class="form-control select2" onchange="changeTaskstatus(this.value)">
                        <option selected="selected" value="o">Open</option>
                        <option value="c">Close</option>
                    </select>
                       </div>
                  </div>--%>


                            <div class="clearfix"></div>
                            <%--<input >--%>
                        </div>
                    </div>




                    <%--</div><!-- max width 730px-->--%>

                    <!-- background color -->

                    <!--  END EDIT CONTENT SHOW -->


                    <!-- Show Add Question-->

                    <div id="add_question" style="background-color: #d7dfe4; display: none; margin-top: 10px;">
                        <div class="clearfix"></div>
                        <br>
                        <div style="max-width: 730px; margin: 0 auto;">
                            <br>

                            <form>

                                <input style="margin-bottom: 5px;" type="text" class="form-control col-md-12- col-sm-12 col- xs-12"><br>


                                <button type="button" id="save_add_question" value="close" class="btn btn-primary">save</button>
                                <button type="button" id="cancel_add_question" value="close" class="btn btn-primary">Cancel</button>

                                <br>
                            </form>





                        </div>
                        <!-- max width 730px-->

                    </div>
                    <!-- background color -->

                    <!-- end Show Add Question-->

                    <!-- Show username next action-->

                    <div id="username_show_next_action" style="background-color: #d7dfe4; display: none; margin-top: 10px;">
                        <div class="clearfix"></div>
                        <br>
                        <div style="max-width: 730px; margin: 0 auto;">
                            <br>

                            <form>

                                <label>Next Action or <a id="closing_username" href="javascript:;">Close</a></label>
                                <textarea rows="3" style="margin-bottom: 5px;" class="form-control col-md-12- col-sm-12 col- xs-12"> </textarea><br>

                                <span class="pull-right">Assign to<a href="#">me</a></span>
                                <br>





                                <button type="button" id="cancel_username" value="close" class="btn btn-primary">save</button>
                                <button type="button" id="cancel_username" value="close" class="btn btn-primary">Cancel</button>

                                <br>

                                <br>
                            </form>





                        </div>
                        <!-- max width 730px-->

                    </div>
                    <!-- background color -->



                    <!-- end Show Add Call-->

                    <!-- Show Add username next action-->

                    <!-- Show username next action-->

                    <div id="closing_username_show" style="background-color: #d7dfe4; display: none; margin-top: 10px;">
                        <div class="clearfix"></div>
                        <br>
                        <div style="max-width: 730px; margin: 0 auto;">
                            <br>

                            <form>

                                <label>closing <a id="closing_username_btn" href="javascript:;">Next Action</a></label>
                                <textarea rows="3" style="margin-bottom: 5px;" class="form-control col-md-12- col-sm-12 col- xs-12"> </textarea><br>

                                <span class="pull-right">Assign to<a href="#">me</a></span>
                                <br>





                                <button type="button" id="cancel_username" value="close" class="btn btn-primary">save</button>
                                <button type="button" id="cancel_username" value="close" class="btn btn-primary">Cancel</button>

                                <br>

                                <br>
                            </form>





                        </div>
                        <!-- max width 730px-->

                    </div>
                    <!-- background color -->
                </div>
                <div class="col-md-12 bgwhite"  id="divnav">
                    <ul class="nav nav-tabs">
                        <li>
                            <a class="btn btn-primary" href="#add_action_show" data-toggle="tab"><i class="glyphicon glyphicon-plus"></i>Add Action
                            </a>
                        </li>
                         <li>
                            <a class="btn btn-primary" href="#add_call_show" data-toggle="tab"><i class="glyphicon glyphicon-plus"></i>Add Call
                            </a>
                        </li>
                          <li >
                            <a class="btn btn-primary" href="#add_note_show" data-toggle="tab"><i class="glyphicon glyphicon-plus"></i>Add Note
                            </a>
                        </li>
                         <li>
                            <a class="btn btn-primary" href="#add_call_deal" data-toggle="tab"><i class="glyphicon glyphicon-plus"></i>Add Deal
                            </a>
                        </li>                         

                    </ul>
               
                    <div class="tab-content" id="tabs">

                        <div class="tab-pane" id="add_note_show">
                            <%--style="background-color:#f4f4f4; margin-top:10px;"--%>
                            <h3>Add Note
                            </h3>
                            <%--  <div class="clearfix"></div><br>
        <div style="max-width:730px; margin:0 auto;"><br>
                            --%>
                            <div class="col-md-12" style="background: #f4f4f4; padding: 20px; border-radius: 9px;">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <%--<label style="font-family:'Baskerville Old Face';font-size:medium;"> Add Note</label>--%>

                                        <textarea id="txtaddnote" rows="3" style="margin-bottom: 5px; height: 50px;" maxlength="1000" class="form-control col-md-12- col-sm-12 col- xs-12"> </textarea><br>
                                        <input type="hidden" id="hidnoteId" />
                                    </div>
                                    <%--	
		<select style="margin-bottom:5px;" class="form-control col-md-3">
		<option>select</option>
		<option>name1</option>
		<option>name2</option>
		</select>'''''''''''''''
<%--		<span class="pull-right">Notify other <a href="#">users</a></span>--%>
                                    <%--<br>--%>
                                    <div class="col-md-6 paddingleft0">
                                        <button type="button" id="save_add_note" onclick="SaveTaskNote();" class="btn btn-primary">Save</button>
                                        <button type="button" id="cancel_add_note1" onclick="clrNotes();" class="btn btn-primary">Reset</button>
                                    </div>
                                </div>
                            </div>
                            <!-- max width 730px-->

                        </div>

                        <!-- background color -->
                        <div class="clearfix"></div>

                        <div class="tab-pane" id="add_call_deal">
                            <%--style="background-color:#f4f4f4; margin-top:10px;"--%>
                            <h3>Add Deal
                            </h3>
                            <%--   <div class="clearfix"></div><br>
        <div style="max-width:730px; margin:0 auto;">
	
		<label style="font-family:'Baskerville Old Face';font-size:medium;"> Add Deal</label>--%>
                            <div class="col-md-12 paddingleft0 paddingright0" style="background: #f4f4f4; padding: 20px; border-radius: 9px;">
                                <div class="col-md-12">
                                    <div class="col-md-4 paddingleft0 " style="display:none">
                                        <div class="form-group">
                                            <label style="font-size: small">Deal Name :</label><input type="text" style="width: 100%" id="txtdealname" maxlength="150" class="form-control" />
                                            <input type="hidden" id="hidTdealId" />
                                        </div>
                                    </div>
                                    <%-- <div class="clearfix"></div>--%>
                                    <div class="col-md-2 paddingleft0">
                                        <div class="form-group">
                                            <%--  <span class="pull-right">--%>
                                            <label style="font-size: small">Deal date: &nbsp;&nbsp; &nbsp;</label>
                                            <input type="text" style="width: 100%" id="txtdealdate" class="datepicker form-control" readonly="readonly" />

                                            <%-- </span>
                <br />--%>
                                        </div>
                                    </div>
                                    <div class="col-md-2 paddingleft0">
                                        <div class="form-group">
                                            <label style="font-size: small">Exp.close date: </label>
                                            <input type="text" style="width: 100%" id="txtexpclsdate" class="datepicker form-control" readonly="readonly" />
                                        </div>

                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="dealstage">
                                        <div class="dt" id="divstage1">
                                    <div class="col-md-2 paddingleft0">
                                        <div class="form-group">
                                            <label style="font-size: small">Mile Stone: </label>
                                            <input type="text" style="width: 100%" id="txtdealstate" maxlength="150" class="form-control dealtext" />
                                            <%--&nbsp;&nbsp; &nbsp;&nbsp; --%>
                                           
                                            <%--  &nbsp;  &nbsp;   --%>
                                        </div>
                                    </div>
                                     <div class="col-md-1 paddingleft0">
                                        <div class="form-group">
                                            <label style="font-size: small">Qty: </label>
                                             <input type="text" onblur="javascript:getTV()" id="txtQty" maxlength="10" class="numeric text-right form-control dealQty"  value="0"  onchange="calamt(' ')" onkeypress="return isNumberKey(event)">
                                            <%--&nbsp;&nbsp; &nbsp;&nbsp; --%>
                                           
                                            <%--  &nbsp;  &nbsp;   --%>
                                        </div>
                                    </div>
                                               <div class="col-md-1 paddingleft0">
                                        <div class="form-group">
                                            <label style="font-size: small">Rate: </label>
                                             <input type="text" onblur="javascript:getTV()" id="txtRate" maxlength="10" class="numeric text-right form-control dealRate"  value="0"  onchange="calamt(' ')" onkeypress="return isNumberKey(event)">
                                            <%--&nbsp;&nbsp; &nbsp;&nbsp; --%>
                                           
                                            <%--  &nbsp;  &nbsp;   --%>
                                        </div>
                                    </div>
                                    <div class="col-md-2 paddingleft0">
                                        
                                            <div class="form-group">
                                            <div class="col-md-8 paddingleft0">
                                                    <label style="font-size: small">Amount (Rs): &nbsp;&nbsp; &nbsp;</label>
                                                    <input type="text" onblur="javascript:getTV()" id="txtamt" maxlength="10" class="numeric text-right form-control dealamount" value="0" onchange="calcommamt();">
                                                    <input type="hidden" value="" class="hiddenid"/>
                                               </div>
                                                
                                                 <div class="col-md-4 paddingleft0" style="margin-top: 25px !important;">
                                                    <img id="imgaddphone" style="height: 15px;width: 15px;" src="img/add1.png" alt="Add" onclick="CreateDealState();">
                                              
                                           
                                              
 </div>
                                            </div>
                                       


                                    </div>


                                       <%--     <div class="col-md-4 paddingleft0">
                                        <div class="col-md-12 paddingleft0">
                                            <div class="form-group">
                                                <div class="col-md-6 paddingleft0">
                                                    <label style="font-size: small">Date: &nbsp;&nbsp; &nbsp;</label>
                                                  <input type="text" style="width: 100%" id="dd" class="datepicker form-control paydate" readonly="readonly">
                                                </div>

                                              
                                            </div>
                                        </div>


                                    </div>--%>
                                            </div>
                                    </div>
                                    <div class="clearfix"></div>
                                     <select id="selstage" onclick="ShowDealMsg('f')" class="form-control" style="display:none">

                                                <option selected="selected">Pending 10%</option>
                                                <option>10% (Qualification)</option>
                                                <option>25%</option>
                                                <option>50%(Decision)</option>
                                                <option>75%</option>
                                                <option>90%(Negotiation)</option>
                                            </select>
    <div class="form-group paddingtop" style="display:none;">
                                                        <span id="spandeal" style="font-size: 13px; display: none">
                                                            <div class="col-md-3 paddingleft0 bottom10">
                                                                <label>X</label>&nbsp;
                                                            </div>
                                                            <div class="col-md-3 padding0" style="position: relative; bottom: 4px;">
                                                                <input type="text" style="width: 50px" onblur="javascript:getTV()" value="0.0" id="txtmonthdeal" maxlength="2" class="numeric text-right form-control">
                                                            </div>
                                                            <div class="clearfix"></div>
                                                            months (Total value Rs.
                                                            <label id="dealtv">0.0</label>)</span>
                                                        <a id="dealamtlnk" onclick="ShowMD();">


                                                            <label id="dealhead">Multi-month deal?</label>
                                                        </a>
                                                    </div>
                                    <%-- <div class="clearfix"></div>--%>


                                    <div class="col-md-4">
                                        <br />
                                        <%--<span id="awon"><a onclick="ShowDealMsg('w')">Won</a> &nbsp;</span>
                <span id="aloss"><a onclick="ShowDealMsg('l')">Loss</a></span>--%>
                                    </div>



                                      <div class="clearfix"></div>

                                         <div class="col-md-1 paddingleft0">
                                        <div class="form-group">
                                            <label style="font-size: small">Comm %: </label>
                                             <input type="text" onblur="javascript:getTV()" id="txtComm" maxlength="10" class="numeric text-right form-control dealQty"  value="0" onchange="calcommamt();">
                                            <%--&nbsp;&nbsp; &nbsp;&nbsp; --%>
                                           
                                            <%--  &nbsp;  &nbsp;   --%>
                                        </div>
                                    </div>
                                               <div class="col-md-2 paddingleft0">
                                        <div class="form-group">
                                            <label style="font-size: small">Commission Amount: </label>
                                             <input type="text" onblur="javascript:getTV()" id="txtcommamt" maxlength="10" class="numeric text-right form-control dealRate"  value="0">
                                            <%--&nbsp;&nbsp; &nbsp;&nbsp; --%>
                                           
                                            <%--  &nbsp;  &nbsp;   --%>
                                        </div>
                                        
                                    </div>
                                    <div class="col-md-2 paddingleft0">
                                     <div class="form-group">
                                            <label style="font-size: small">Total Amount: </label>
                                             <input type="text"  id="txttotaldealamt" maxlength="10" class="numeric text-right form-control "  value="0">
                                        </div></div>
                                    <div class="clearfix"></div>
                                    <div class="col-md-6 paddingleft0" style="margin-top: 10px;">
                                        <div class="form-group">
                                            <label style="font-size: small">Deal Note:</label>
                                            <textarea rows="8" id="txtdealnote" style="margin-bottom: 5px; height: 70px;" class="form-control col-md-12- col-sm-12 col- xs-12"> </textarea>
                                            <%--<form action="/CRMDBService.asmx/UploadFile" enctype="multipart/form-data" id="att-form" method="post" name="att-form" role="form">--%>

                                                   <asp:FileUpload ID="uploader" runat="server" class="multi" />
                                    <%--        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.doc|.docx|.pdf|.png|.jpeg|.xlxs|.xls|.jpg)$"
    ControlToValidate="uploader" runat="server" ForeColor="Red" ErrorMessage="Please select a valid Word,PDF,Excel,Images File."
    Display="Dynamic" />--%>
                                            <div id="selectedfile"></div>
                                            <input type="file" id="fupload" name="uploadFile" style="display:none" />
                                             <input type="button" id="btnUpload" value="upload" class="btn btn-primary" style="visibility:hidden"/>
                                        
                                              <div style="width:500px">
            <div id="progress-bar" style="position: relative; display: none">
                <span id="progressbar-label" style="position: absolute; left: 30%; top: 20%;">Please Wait...</span>
            </div>
        </div>
                                        </div>
                                    </div>
                                    <%--<asp:Button ID="btnUpload" runat="server" cssClass="button" Text="Upload Selected File" />--%>


                                    <%--     </form>--%>
                                    <%--<select style="margin-bottom:5px;" class="form-control col-md-3">
		<option>select</option>
		<option>name1</option>
		<option>name2</option>
		</select>''''''''''''
		<%--<span class="pull-right">Notify other <a href="#">users</a></span>--%>
                                    <%--<br>--%>
                                    <div class="clearfix"></div>
                                    <div class="col-md-6 paddingleft0">
                                        <button type="button" id="save_add_deal" onclick="SaveDeal();" class="btn btn-primary">Save</button>
                                        <button type="button" id="cancel_add_deal1" onclick="clrdeals(0);" class="btn btn-primary">Reset</button>
                                    </div>
                                    <%--<br>--%>
                                    <div id="divwondeal" style="display: none; background-color: #fff1a8; color: black;">
                                        Congratulations. You can record details on how or why you won the deal, such as price reduction or other incentive.
                                    </div>
                                    <div id="divlossdeal" style="display: none; background-color: #fff1a8; color: black;">
                                        Make sure you record details on how you lost the deal, or to which competitor you lost it to.
                                    </div>
                                </div>

                            </div>
                            <!-- max width 730px-->

                        </div>
                        <!-- background color -->

                        <!-- commented by jyoti awasthi-->
                        <!-- end Show Add Deal-->
                        <!-- right side -->

                        <div class="clearfix"></div>
                        <!--start here-->

                        <div class="tab-pane" id="add_call_show">
                            
                            <%--style="background-color:#f4f4f4;margin-top:10px;"--%>
                            <h3>Add Call/Email
                            </h3>
                            <div id="divcall" runat="server">
                            <%--   <div class="clearfix"></div><br>
        <div style="max-width:730px; margin:0 auto;"><br>
          
		
		<label style="font-family:'Baskerville Old Face';font-size:medium;"> Add Call</label>
            <br />--%>
                                  <input type="radio" name="phoneemail" id="rdcall" value="Call" style="font-size:15px" checked onclick = "MyAlert()">Call
                                  <input type="radio" name="phoneemail" id="rdemail" value="Email" style="font-size:15px" onclick = "MyAlert()">Email<br>
                            <div class="col-md-12" id="divcallemail" style="background:#f4f4f4; padding:20px; border-radius: 9px;">
                              
                                <div class="col-md-12"  >
                                    <div class="col-md-2 paddingleft0">
                                        <div class="form-group">
                                            <label style="font-size: small">Phone:</label>
                                            <select id="myselectcall" name="myselectcall" class="form-control"></select>
                                        </div>
                                    </div>
                             <%--       <div class="col-md-2 paddingleft0">
                                        <div class="form-group">
                                            <label style="font-size: small">Result:</label>
                                            <select id="myselectcallresult" name="myselectcallresult" class="form-control">
                                                <option>Not interested </option>
                                                <option>Left message </option>
                                                <option>No answer </option>
                                                <option>Other</option>
                                            </select>
                                        </div>
                                    </div>--%>
                                    <input type="hidden" id="hidTcallId" />

                                    <div class="form-group">
                                       
                                        <textarea rows="3" id="txtcallnote" style="margin-bottom: 5px; height: 105px;" class=" form-control" placeholder="Call Note"> </textarea>
                                   
                                        <span class="pull-right" style="display: none">Notify other <a href="#">users</a></span>
                                    </div>

                                    <div class="col-md-6 paddingleft0">
                                        <button type="button" id="save_add_call1" onclick="SaveCall();" class="btn btn-primary">Save</button>
                                        <button type="button" id="cancel_add_call1" onclick="clrCall();" class="btn btn-primary">Reset</button>

                                    </div>

                                </div>
                                <!-- max width 730px-->
                                </div>
                            <div id="divemail" style="display:none"  >
                            <div class="col-md-12" style="background:#f4f4f4; padding:20px; border-radius: 9px;">
                                <div class="col-md-12">
                                    <div class="col-md-2 paddingleft0">
                                        <div class="form-group">
                                            <label style="font-size: small">Email:</label>
                                    
                                        </div>
                                           <select id="myselectemailall" name="myselectemailall" class="form-control" onchange="fillemail();">
                                               <option value="0">-Select--</option>
                                           </select>
                                        <br/>
                                             <div id="myselectemail" name="myselectemail" runat="server"   class="col-md-5 paddingleft0"></div> 
                                          </div>
                                          
                                           <div class="form-group">
                                       
                                        <textarea rows="3" id="txtaddemailnote" style="margin-bottom: 5px; height: 105px;" class=" form-control" placeholder="Call Note"> </textarea>
                                   
                                        <span class="pull-right" style="display: none">Notify other <a href="#">users</a></span>
                                    </div>
                                        <div class="col-md-6 paddingleft0">
                                        <button type="button" id="save_add_Email" onclick="SaveEmail();" class="btn btn-primary">Save</button>
                                        <button type="button" id="cancel_add_Email" onclick="clrCall();" class="btn btn-primary">Reset</button>

                                    </div>
                                  <%--  <input type="hidden" id="hidTcallId" />--%>

                              
                                <!-- max width 730px-->

                            </div>
                            </div>
                            </div> 
                            </div>
                            </div>
                        <div class="clearfix"></div>

                        <div class="tab-pane" id="add_action_show">
                            <h3>Action
                            </h3>
                            <div class="col-md-12" style="background: #f4f4f4; padding: 20px; border-radius: 9px;">
                                <div class="col-md-12">

                                    <div class="form-group">
                                        <textarea name="txtnxtaction" id="txtnxtaction" style="margin-bottom: 5px; height: 50px;" maxlength="1000" class="form-control col-md-12- col-sm-12 col- xs-12"></textarea>
                                        <input type="hidden" id="hidref_docId" />
                                        <input type="hidden" id="hid_TdocId" />
                                        <%--   <input type="hidden" id="hidref_sno"/>--%>
                                        <%--<input type="hidden" id="hid_crmstatus"/>--%>
                                    </div>
                                 

                                    <div class="col-md-3 paddingleft0">
                                        <div class="form-group">
                                            <label>Date: </label>
                                            <div class="form-group">
                                                <input type="text" id="txtnxtactndt" class="datepicker form-control" readonly="readonly" />
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <div class="col-md-2 paddingleft0">
                                        <div class="form-group">
                                            <label>Time: </label>
                                            <div class="form-group">
                                                  <input type="time" name="usr_time"  class="form-control" id="txttime" >
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3 paddingleft0">
                                        <div class="form-group">
                                            <label>Assigned To: </label>
                                            <div class="clearfix"></div>
                                            <select id="lstowners" class="form-control select2">
                                                <%--class="selown"--%>
                                            </select>

                                            <input type="hidden" id="HidOwner" />
                                        </div>
                                    </div>
                                    <div class="col-md-3 paddingleft0">
                                        <div class="form-group">
                                            <label>
                                                Set Task Status To :
                                            </label>
                                            <select id="myselectTaskStatus" style="width: 140px; height: 25px;" name="myselectTaskStatus" class="form-control select2"> <%--> onchange="changeTaskstatus(this.value)"--%>
                                                <option selected="selected" value="o">Open</option>
                                                <option value="c">Close</option>
                                            </select>
                                        </div>
                                    </div>
                                   
                                    <div class="col-md-6 paddingleft0">
                                        <%--style="padding-bottom: 10px;"--%>

                                        <input type="button" id="Add_Action" value="Save" onclick="SaveNextAction();" class="btn btn-primary" />
                                        <%--<button type="button" id="cancel_edit_show_content" value="close" class="btn btn-primary"> Cancel</button>--%>
                                        <input type="button" value="Cancel" onclick="cleartask();" class="btn btn-primary" />
                                    </div>

                                </div>
                            </div>

                        </div>

                    </div>
                
                      </div>
                
                    <br />

                    <!-- commented by jyoti awasthi-->

                    <div id="bg_contact_id" style="padding: 10px; background-color: #d7dfe4; display: none /* box-shadow: 2px 2px 2px red; */" class="col-md-12">

                        <div id="bg_conatct_created" style="display: none;">

                            <div class="col-lg-4 cl-md-4 col-sm-3 col-xs-6 pull-left">
                                <i class="fa fa-user"></i>Background &nbsp; 
			   <a class="" id="bg_edit"><i class="fa fa-pencil"></i></a>
                            </div>
                            <div class="col-md-6 col-sm-8 col-xs-12 pull-right">
                                <%--Contact created Aug 11, 2016 from <a class="#" href="#">Email or Web</a>
					  <span> lead source</span>--%>
                            </div>
                        </div>

                        <!--  edit background-->
                        <div style="display: block;" id="conatct_created">

                            <div class="col-lg-1 col-md-2 col-sm-2 col-xs-4">
                                <i class="fa fa-user"></i>Contact&nbsp; 
			
                            </div>
                            <div class="col-lg-1 col-md-2 col-sm-2 col-xs-4 no-padding">
                                <a class="" id="bg_edit"><i class="fa fa-plus"></i>Add Contact</a>
                            </div>


                            <div class="col-lg-1 col-md-3 col-sm-3 col-xs-4 pull-left">
                                <i class="fa fa-link"></i>Link&nbsp; 
			 
                            </div>

                            <div class="col-md-5 col-sm-5 col-xs-12 pull-right">
                                Contact created Aug 11, 2016 from <a class="#" href="#">Email or Web</a>
                                <span>lead source</span>

                            </div>
                        </div>




                        <div id="edit_background" style="background-color: #d7dfe4; display: none; margin-top: 10px;">
                            <div class="clearfix"></div>
                            <br>
                            <div style="width: 90%;">
                                <br>
                                <form>
                                    <textarea id="txtbg" rows="3" style="margin-bottom: 5px; height: 50px;" maxlength="1000" class="form-control col-md-12- col-sm-12 col- xs-12"> </textarea><br>


                                    <br>

                                    <button type="button" id="save_add_bg" onclick="SaveBg();" class="btn btn-primary">Save</button>
                                    <button type="button" id="cancel_add_bg" class="btn btn-primary">Cancel</button>

                                    <br>

                                    <br>
                                </form>





                            </div>
                            <!-- max width 730px-->

                        </div>
                        <!-- background color -->

                        <!-- end edit background-->


                    </div>
                    <!-- commented by jyoti awasthi-->
                    <div id="background_comment" class="container-fluid" style="padding: 7px; background-color: rgb(240, 240, 240); display: none;">
                        <label for="lblbg"></label>
                    </div>
                    <!-- commented by jyoti awasthi-->


                    <!-- last  container -->
                    <!-- comapny members -->
                    <!-- commented by jyoti awasthi-->
                    <div style="display: block;" id="compamy_members">
                        <div style="padding: 10px;" class="col-md-2 col-sm-3 col-xs-6">

                            <img src="img/unknown.jpg" width="123px">
                        </div>

                        <div class="col-md-4 col-sm-5 col-xs-6">
                            <h2 class="userh1"><b>Company member</b> <span>
                                <img src="img/star.png"><span><h5 style="color: #b1b0ae">Dataman computer Sytemm It Lab</h5>
                                </span></span></h2>
                            User1 at user1<br>

                            <a>user1@gmail.com</a><br>
                            <i class="glyphicon glyphicon-print"></i>&nbsp;Vcard<br>
                            <br>
                            <!-- user more info -->
                            <div id="address" style="display: none;" class="address expandable item">
                                <div class="inside">
                                    <span>
                                        <!-- react-text: 59 -->
                                        25 Mason Street<!-- /react-text --><br>
                                    </span><span>
                                        <!-- react-text: 62 -->
                                        San Francisco<!-- /react-text --><br>
                                    </span><span>
                                        <!-- react-text: 65 -->
                                        CA 94102<!-- /react-text --><br>
                                    </span><span>United States</span><img alt="Search in Google Maps" src="https://cdn-assets.onepagecrm.com/assets/map-c38d05423b129eaaea2c07630a62997ce2b0343cad436d2a85dc9908c68d1bd0.png">
                                </div>
                            </div>

                            <!-- end user more info -->
                        </div>

                        <div class="col-md-3 COL-SM-2 col-xs-12 pull-right">
                            <ul style="padding-top: 10px; padding-top: 10px;" class="list-inline">
                                <li class="dropdown">
                                    <a href="" class="dropdown-toggle" data-toggle="dropdown">Options
                            <span class="caret pull-left"></span>
                                    </a>
                                    <!-- Sub menu -->
                                    <ul class="dropdown-menu animated fadeInUp">

                                        <li><a id="option 1" href="javascript:;">option 1</a></li>
                                        <li><a href="#">option 2</a></li>


                                    </ul>

                                </li>
                            </ul>
                        </div>


                        <!-- end option -->

                    </div>

                    <!-- commented by jyoti awasthi-->

                    <%--<div class="clearfix"></div>--%>

                    <!-- end company member  -->
                    <!-- commented by jyoti awasthi-->
                    <%-- <div class="bigcontainer">--%>
                    <!--  <div style="padding-bottom:20px;" class="col-md-4">

        <ul class="list-unstyled">

            <li><h3 style="color:#db2b5d;" class="h3">
			
			<i class="fa fa-clock-o fa-2" ></i>&nbsp;Pending Deal &nbsp;&nbsp;<a data-toggle="modal" data-target="#pending_edit_comment" href="javascript();">
			<i class="fa fa-trash-o fa-1"></i> </a>&nbsp;&nbsp;<a data-toggle="modal" data-target="#pending_delete_comment" href="javascript();">
			<i class="fa fa-pencil fa-1"></i></a></h3></li>
            <li>₨5,13010% chance it will close Sep 15, 2016</li>
          <li> <b>August deal -</b> Sent him estimate today.. fingers crossed!</li> 
          <li> <b>August deal -</b> Sent him estimate today.. fingers crossed!</li> 
        </ul>--->


                    <%--  </div>--%>
                    <!-- commented by jyoti awasthi-->


                    <!---end here-->

             

                <!-- end Show Add Call-->

                <!-- Show closing username next action-->





                <!-- commented by jyoti awasthi-->
                <%--         <div style="">
        <div  style="max-width:730px; margin:0 auto;"><br>
           <div id="taskdetails" class="col-md-12 col-xs-12 col-sm-12 pull-left">
             <%--  <ul>
                   <li>juio - queued for mei&nbsp; &nbsp;<a data-toggle="modal" data-target="#editcomment" href="javascript();"><i class="fa fa-trash-o fa-1"></i></a> &nbsp;&nbsp; <a data-toggle="modal" data-target="#delcomment" href="javascript();"><i class="fa fa-pencil fa-1"></i></a> </li>
                   <li>juio - queued for mei&nbsp; &nbsp;danish<i class="fa fa-trash-o fa-1"></i> &nbsp;&nbsp; <i class="fa fa-pencil fa-1"></i> </li>

                   <li>juio - queued for mei&nbsp; &nbsp;<i class="fa fa-trash-o fa-1"></i> &nbsp;&nbsp; <i class="fa fa-pencil fa-1"></i> </li>
               </ul>...........

           </div>

         


            <br>



             </div>--%>
                <!-- commented by jyoti awasthi-->
                <%--<div class="clearfix"></div>--%>
                <!-- max width 730px section-->
                <!-- commented by jyoti awasthi-->
                <%--     <div style="max-width:730px; margin:0 auto;" class="col-md-6 col-xs-12 col-sm-12 pull-left">
              <ul class="list-inline">
                 <li class="btnbottompading"> <button id="add_note" type="button" class="btn btn-primary"><i class="glyphicon glyphicon-plus"></i> Add Note</button></li>
                 <li class="btnbottompading"> <button id="add_deal" type="button" class="btn btn-primary"><i class="glyphicon glyphicon-plus"></i> Add Deal</button></li>
                  <li class="btnbottompading"> <button id="add_call" type="button" class="btn btn-primary"><i class="glyphicon glyphicon-plus"></i> Add Call</button></li>
                  </ul>

               </div>
              <!-- commented by jyoti awasthi-->
    </div><!-- background color -->---%>

                <!-- Show Add Note-->
                <%--<div class="clearfix"></div>--%>
                <%--previous background color#db2b5d--%>
                <!-- commented by jyoti awasthi-->
                <%--<div id="add_note_show" style="background-color:#b1d1e4;display:none;  margin-top:10px;">
	   <div class="clearfix"></div><br>
        <div style="max-width:730px; margin:0 auto;"><br>
          
            
		<label style="font-family:'Baskerville Old Face';font-size:medium;"> Add Note</label>
		
		<textarea id="txtaddnote" rows="3" style="margin-bottom:5px;height:50px;" maxlength="1000" class="form-control col-md-12- col-sm-12 col- xs-12"> </textarea><br>
            <input type="hidden" id="hidnoteId" />
	<%--	
		<select style="margin-bottom:5px;" class="form-control col-md-3">
		<option>select</option>
		<option>name1</option>
		<option>name2</option>
		</select>'''''''''''''''
<%--		<span class="pull-right">Notify other <a href="#">users</a></span>'''''''''''''
		<br>
		
		<button type="button" id="save_add_note" onclick="SaveTaskNote();" class="btn btn-primary"> Save</button>
		<button type="button" id="cancel_add_note"  class="btn btn-primary"> Cancel</button>
		
		<br>
		
		<br>
	
		
             </div><!-- max width 730px-->

    </div><!-- background color -->--%>
                <!-- commented by jyoti awasthi-->
                <%--    <div class="clearfix"></div>
           <!-- commented by jyoti awasthi-->
      <ul class="list-unstyled"> <li><h4 style="color:#db2b5d;background: #d7dfe4;padding: 3px;"">
			&nbsp;Notes &nbsp;&nbsp;</h4></li></ul>
         <div  style="max-width:730px; margin:0 auto;"><br>
           
		<div id="divnotes" class="col-md-12 col-xs-12 col-sm-12 pull-left">
          
        
		</div>
             </div>--%>
                <!-- commented by jyoti awasthi-->
                <!-- end Show Add Note-->
                <%--<div class="clearfix"></div>--%>
                <!-- Show Add call-->

                <!-- background color -->

                <!-- end Show Add Call-->

                <!-- Show Add Deal-->
                <!-- commented by jyoti awasthi-->
                <%--<div id="add_call_deal" style="background-color:#b1d1e4;display:none;  margin-top:10px;">
	   <div class="clearfix"></div><br>
        <div style="max-width:730px; margin:0 auto;">
	
		<label style="font-family:'Baskerville Old Face';font-size:medium;"> Add Deal</label>
            <span class="pull-right"> <label  style="font-size:small">Deal date: </label>  <input type="text" style="width:100px" id="txtdealdate" class="datepicker"/></span>
                <br />
        <label style="font-size:small"> Deal Name :</label><input type="text" style="width:500px" id="txtdealname" maxlength="150" class="form-control" />
             <input type="hidden" id="hidTdealId" />
       <br />
               <label style="font-size:small"> Amount (Rs): &nbsp;&nbsp; &nbsp;</label><input type="text" style="width:150px" onblur="javascript:getTV()" id="txtamt" maxlength="10" class="numeric text-right" />
          <span id="spandeal" style="display:none">   <label >X</label>&nbsp;<input type="text" style="width:50px" onblur="javascript:getTV()" value="0.0" id="txtmonthdeal" maxlength="4" class="numeric text-right" />&nbsp;months&nbsp;(Total value Rs. <label id="dealtv">0.0</label>) </span> <a id="dealamtlnk" onclick="ShowMD();" ><label id="dealhead">Multi-month deal?</label> </a>
         <br />
               <label style="font-size:small">Exp.close date: </label>  <input type="text" style="width:150px" id="txtexpclsdate" class="datepicker"/>
         <br />
                  <label style="font-size:small">Deal Stage: </label> &nbsp;&nbsp; &nbsp;&nbsp; <select id="selstage"  onclick="ShowDealMsg('f')">
                   
                        <option selected="selected" >Pending 10%</option>
                      <option  >10% (Qualification)</option>
                      <option >25%</option>
                      <option >50%(Decision)</option>
                      <option >75%</option>
                      <option >90%(Negotiation)</option>
                     </select>   &nbsp;  &nbsp;              
          <span  id="awon">  <a onclick="ShowDealMsg('w')" >Won</a> &nbsp;</span> 
            <span id="aloss"><a  onclick="ShowDealMsg('l')"  >Loss</a></span>

            <br />
           
		<label style="font-size:small">Deal Note:</label>
             <textarea rows="3" id="txtdealnote" style="margin-bottom:5px;height:50px;" class="form-control col-md-12- col-sm-12 col- xs-12"> </textarea>
           <%--<form action="/CRMDBService.asmx/UploadFile" enctype="multipart/form-data" id="att-form" method="post" name="att-form" role="form">--%>

                <%--<asp:FileUpload ID="fupload" runat="server"  onchange='prvimg.UpdatePreview(this)' />''''''''''''
          
         <input type="file" id="fupload" name="uploadFile"/>
<input type="button" id="btnUpload" value="Submit" />    
            <%--<asp:Button ID="btnUpload" runat="server" cssClass="button" Text="Upload Selected File" />'''''''''''''''                   
          

               </form>
		<%--<select style="margin-bottom:5px;" class="form-control col-md-3">
		<option>select</option>
		<option>name1</option>
		<option>name2</option>
		</select>''''''''''''
		<%--<span class="pull-right">Notify other <a href="#">users</a></span>''''''''''''
		<br>
		
	
		<button type="button" id="save_add_deal" onclick="SaveDeal();" class="btn btn-primary"> Save</button>
		<button type="button" id="cancel_add_deal" onclick="clrdeals();" class="btn btn-primary"> Cancel</button>
		
		<br>
		<div id="divwondeal" style="display:none;background-color:#fff1a8;color:black;">
            Congratulations. You can record details on how or why you won the deal, such as price reduction or other incentive.
		</div>
            <div id="divlossdeal" style="display:none;background-color:#fff1a8;color:black;">
           Make sure you record details on how you lost the deal, or to which competitor you lost it to.
		</div>
		<br>
		
             </div><!-- max width 730px-->

    </div><!-- background color -->--%>
                <!-- commented by jyoti awasthi-->
                <!-- end Show Add Deal-->
                <%--  </div> <!-- right side -->
                --%>
                <!-- commented by jyoti awasthi-->
                <br />

                <%--<div class="clearfix"></div>--%>
                <%--<div style="padding-bottom:20px; background-color:white;" class="col-md-12 pd_deal">

        <ul class="list-unstyled">

            <li><h4 style="color:#db2b5d;background: #d7dfe4;padding: 3px;"">
			
			<i class="fa fa-clock-o fa-2"></i>&nbsp;Pending Deal &nbsp;&nbsp;</h4></li>
          
        </ul>
           <div  style="max-width:730px; margin:0 auto;"><br>
           
		<div id="divdeals" class="col-md-12 col-xs-12 col-sm-12 pull-left">
          
        
		</div>
             </div>

    </div>--%>
                <!-- commented by jyoti awasthi-->
                <%--<div class="clearfix"></div>--%>
                <%--  <div id="add_call_show" style="background-color:#b1d1e4;display:none;margin-top:10px;">
	   <div class="clearfix"></div><br>
        <div style="max-width:730px; margin:0 auto;"><br>
          
		
		<label style="font-family:'Baskerville Old Face';font-size:medium;"> Add Call</label>
            <br />
                 <label style="font-size:small">Phone:</label>
              <select id="myselectcall" style="width:140px;height:25px;" name="myselectcall" class="form-control select2" ></select>
		     <label style="font-size:small">Result:</label>
		  <select id="myselectcallresult" style="width:140px;height:25px;" name="myselectcallresult" class="form-control select2" >
              <option>Interested </option>
              <option>Not interested </option>
              <option>Left message </option>
              <option>No answer </option>
              <option>Other</option>
		  </select>
            <input type="hidden" id="hidTcallId" />
            <label style="font-size:small">Call Note:</label>
             <textarea rows="3" id="txtcallnote" style="margin-bottom:5px;height:50px;" class="form-control col-md-12- col-sm-12 col- xs-12"> </textarea>
		<span class="pull-right">Notify other <a href="#">users</a></span>
		<br>
		
		<button type="button" id="save_add_call" onclick="SaveCall();"  class="btn btn-primary"> Save</button>
		<button type="button" id="cancel_add_call"  class="btn btn-primary"> Cancel</button>
		
		<br>
		
		<br>
		

             </div><!-- max width 730px-->

    </div>--%>
                <!-- commented by jyoti awasthi-->
                <%--   <div style="padding-bottom:20px; background-color:white;" class="col-md-12 pd_deal">

        <ul class="list-unstyled">

            <li><h4 style="color:#db2b5d;background: #d7dfe4;padding: 3px;"">
			
			<i class="fa fa-clock-o fa-2"></i>&nbsp;Call &nbsp;&nbsp;</h4></li>
          
        </ul>
           <div  style="max-width:730px; margin:0 auto;"><br>
           
		<div id="divcall" class="col-md-12 col-xs-12 col-sm-12 pull-left">
          
        
		</div>
             </div>

    </div>--%>
                <!-- commented by jyoti awasthi-->
                <br />
                <%--<div class="clearfix"></div>--%>
                <!-- commented by jyoti awasthi-->
                <%--<div id="bg_contact_id" style="padding:10px; background-color:#d7dfe4; /* box-shadow: 2px 2px 2px red; */" class="col-md-12">
        
      <div id="bg_conatct_created" style="display: none;">	

			 <div class="col-lg-4 cl-md-4 col-sm-3 col-xs-6 pull-left">
			   <i class="fa fa-user"></i>Background &nbsp; 
			   <a class="" id="bg_edit"><i class="fa fa-pencil"></i></a>
			   </div>
			  <div class="col-md-6 col-sm-8 col-xs-12 pull-right">
				  <%--Contact created Aug 11, 2016 from <a class="#" href="#">Email or Web</a>
					  <span> lead source</span>''''''''''''''
				  
				 </div>
		   </div>
		   
		   <!--  edit background-->
		  <div style="display: block;" id="conatct_created">	

			 <div class="col-lg-1 col-md-2 col-sm-2 col-xs-4">
			   <i class="fa fa-user"></i>Contact&nbsp; 
			
			   </div>
			    <div class="col-lg-1 col-md-2 col-sm-2 col-xs-4 no-padding">
			      <a class="" id="bg_edit"><i class="fa fa-plus"></i>Add Contact</a>
			   </div>
			   
			   
			    <div class="col-lg-1 col-md-3 col-sm-3 col-xs-4 pull-left">
			   <i class="fa fa-link"></i>Link&nbsp; 
			 
			   </div>

			  <div class="col-md-5 col-sm-5 col-xs-12 pull-right">
				  Contact created Aug 11, 2016 from <a class="#" href="#">Email or Web</a>
					  <span> lead source</span>
				  
				 </div>
		   </div>
		   
		   
		   
	
	<div id="edit_background" style="background-color:#d7dfe4;display:none;  margin-top:10px;">
	   <div class="clearfix"></div><br>
        <div style="width:90%;"><br>
        <form>	
		<textarea id="txtbg" rows="3" style="margin-bottom:5px;height:50px;" maxlength="1000" class="form-control col-md-12- col-sm-12 col- xs-12"> </textarea><br>
		
		
		<br>
	
		<button type="button" id="save_add_bg" onclick="SaveBg();" class="btn btn-primary"> Save</button>
		<button type="button" id="cancel_add_bg"  class="btn btn-primary"> Cancel</button>
		
		<br>
		
		<br>
		
		
		
		</form>

           
           


             </div><!-- max width 730px-->

    </div><!-- background color -->
	
    <!-- end edit background-->
	
	
     </div>--%>
                <!-- commented by jyoti awasthi-->
                <%--<div id="background_comment" style="padding: 7px; background-color: rgb(240, 240, 240); display: none;" class="container-fluid">
    <label for="lblbg"></label>
    </div>--%>
                <!-- commented by jyoti awasthi-->
                <%--<div class="clearfix"></div><br>--%>

                <!-- last  container -->
                <!-- comapny members -->
                <!-- commented by jyoti awasthi-->
                <%--<div style="display: block;" id="compamy_members">
        <div style="padding:10px;" class="col-md-2 col-sm-3 col-xs-6">

          <img src="img/unknown.jpg" width="123px">
          
          
      </div>
      
      <div class="col-md-4 col-sm-5 col-xs-6">
          <h2 class="userh1"><b>Company member</b> <span><img src="img/star.png"><span><h5 style="color:#b1b0ae">Dataman computer Sytemm It Lab</h5></span></span></h2>
           User1 at user1<br>

          <a> user1@gmail.com</a><br>
           <i class="glyphicon glyphicon-print"></i>&nbsp;Vcard<br><br>
		   <!-- user more info -->
		    <div id="address" style="display:none;" class="address expandable item">
			<div class="inside"><span><!-- react-text: 59 -->25 Mason Street<!-- /react-text --><br></span><span><!-- react-text: 62 -->San Francisco<!-- /react-text --><br></span><span><!-- react-text: 65 -->CA 94102<!-- /react-text --><br></span><span>United States</span><img alt="Search in Google Maps" src="https://cdn-assets.onepagecrm.com/assets/map-c38d05423b129eaaea2c07630a62997ce2b0343cad436d2a85dc9908c68d1bd0.png"></div></div>

			 <!-- end user more info -->
             </div>
			 
			 <div class="col-md-3 COL-SM-2 col-xs-12 pull-right">
            <ul style="padding-top: 10px; padding-top: 10px;" class="list-inline">
        <li class="dropdown">
                         <a href="" class="dropdown-toggle" data-toggle="dropdown">
                         Options
                            <span class="caret pull-left"></span>
                         </a>
                         <!-- Sub menu -->
                           <ul class="dropdown-menu animated fadeInUp">

                            <li><a id="option 1" href="javascript:;">option 1</a></li>
                            <li><a href="#">option 2</a></li>
                           

                          </ul>

                            </li>
							</ul>
      </div>

			 
			 <!-- end option -->
			 
	</div>--%>
                <!-- commented by jyoti awasthi-->

                <%--<div class="clearfix"></div>--%>

                <!-- end company member  -->
                <!-- commented by jyoti awasthi-->
                <%--    <div class="bigcontainer">
     <!--  <div style="padding-bottom:20px;" class="col-md-4">

      


    </div> -->--%>
                <!-- commented by jyoti awasthi-->

                <div id="divhis" class="col-md-12 col-sm-12 col-xs-12 bgwhite" style="display: none;">
                    <div class="col-md-2 col-sm-2 col-xs-12 paddingleft0">
                        <%-- <b style="font-size:16px;">--%>
                        <h2>History</h2>

                    </div>
                    <div class="col-md-6 col-sm-8 col-xs-12 paddingright0 pull-right paddingleft0" style="margin-top: 20px; margin-bottom: 10px;">
                        <input type="hidden" id="listid" />
                        <ul class="list-inline pull-right">
                            <li id="All" onclick="checkhistory(this)" style="cursor: pointer; color: #3c8dbc">
                                <span class="bg-green spanaction">All
                                </span>
                            </li>
                            <li id="Notes" onclick="checkhistory(this)" style="cursor: pointer; color: #3c8dbc">
                                <span class="bg-blue spanaction">Notes & Calls
                                </span>
                            </li>
                            <li id="Deals" onclick="checkhistory(this)" style="cursor: pointer; color: #3c8dbc">
                                <span class="bg-red spanaction">Deals
                                </span>
                            </li>
                            <li id="Actions" onclick="checkhistory(this)" style="cursor: pointer; color: #3c8dbc">
                                <span class="bg-yellow spanaction">Action
                                </span>
                            </li>
                            <%-- <li>Emals</li>
               <li>Updates&nbsp;<i class="fa fa-twitter"></i></li>--%>
                        </ul>

                    </div>




                </div>


           
                <div id="taskdetailstetsing" class="col-md-12 col-xs-12 col-sm-12 pull-left" style="display: none;">
            
                </div>
                <div id="divtask" class="col-md-12 col-sm-12 col-xs-12 bgwhite" style="display: none;">
                    <div class="row">
                        <div class='col-md-12'>
                            <div id="taskdetailstetsing1" class="col-md-12 col-xs-12 col-sm-12 pull-left">
                            </div>
                            <div id="taskdetailstetsing2" class="col-md-12 col-xs-12 col-sm-12 pull-left">
                            </div>
                            <div id="taskdetailstetsing3" class="col-md-12 col-xs-12 col-sm-12 pull-left">
                            </div>
                            <div id="taskdetailstetsing4" class="col-md-12 col-xs-12 col-sm-12 pull-left">
                            </div>
                        </div>
                    </div>
                </div>
            </div>




            <!-- danish -->
        <div class="modal fade in" id="modal-attachments" style="padding-right: 17px;">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">×</span></button>
                <h3 class="modal-title">Attachments</h3>
              </div>
              <div class="modal-body" id="Attachments">
           
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
             
              </div>
            </div>
        
          </div>
       
        </div>


        <!---deal detail -->

        <div class="modal fade in" id="modal-default" style="padding-right: 17px;">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">×</span></button>
                <h3 class="modal-title">Deal Detail</h3>
              </div>
              <div class="modal-body" id="dealstage">
           
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                <button type="button" class="btn btn-primary" data-dismiss="modal" onclick="getattachments();">Attachments</button>
              </div>
            </div>
        
          </div>
       
        </div>
        <!---deal detail -->

            <!-- modals -->
            <div class="modal fade" id="myModal" role="dialog">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">×</button>
                            <h4 class="modal-title">Edit Contact</h4>
                        </div>
                        <div class="modal-body">
                            <form>
                                <lable>Name</lable>
                                <input type="text" class="form-control"></form>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>

                </div>
            </div>


            <!-- modals -->




            <!-- modal editcomment -->
            <div class="modal fade" id="editcomment" role="dialog">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">×</button>
                            <h4 class="modal-title">Edit comment</h4>
                        </div>
                        <div class="modal-body">
                            <form>
                                <input type="text" class="form-control"></form>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>

                </div>
            </div>


            <!-- modals -->



            <!-- modal Company description -->
            <div class="modal fade" id="company_description" role="dialog">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">×</button>
                            <h4 class="modal-title">Edit comment</h4>
                        </div>
                        <div class="modal-body">
                            <form>
                                <input type="text" class="form-control"></form>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>

                </div>
            </div>


            <!-- end Company description -->



            <!-- modal delete comment -->
            <div class="modal fade" id="delcomment" role="dialog">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">×</button>
                            <h4 class="modal-title">Delete comment</h4>
                        </div>
                        <div class="modal-body">
                            <form>
                                <input type="text" class="form-control"></form>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>

                </div>
            </div>


            <!-- modals -->


            <!-- pending comment -->


            <!-- modal pending editcomment -->
            <div class="modal fade" id="pending_edit_comment" role="dialog">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">×</button>
                            <h4 class="modal-title">Edit comment</h4>
                        </div>
                        <div class="modal-body">
                            <form>
                                <input type="text" class="form-control"></form>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>

                </div>
            </div>


            <!-- modals -->



            <!-- modal pending delete comment -->
            <div class="modal fade" id="pending_delete_comment" role="dialog" aria-hidden="true" style="display: none;">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">×</button>
                            <h4 class="modal-title">Delete comment</h4>
                        </div>
                        <div class="modal-body">
                            <form>
                                <input type="text" class="form-control"></form>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>

                </div>
            </div>



            <!-- modals -->
            <!-- end pending details model -->
            <script src="plugins/alljs.js" type="text/javascript"></script>

            <!--Script Section Ankita-->
            <script type="text/javascript">
                var myarray = new Array();
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!
        var yyyy = today.getFullYear();

        if (dd < 10) {
            dd = '0' + dd
        }

        if (mm < 10) {
            mm = '0' + mm
        }

        today = mm + '/' + dd + '/' + yyyy;
        //alert(today);
        //  var curr = new Date(today);
        var opt1 = 0;
        var opt = 0;
        var RefId = "";
        var DocId = 0;
        if ('<%= Request.QueryString["DocId"]%>' != "") {
             DocId = '<%= Request.QueryString["DocId"]%>';
           // GetDatabyDocId(DocId);
        }
        if ('<%= Request.QueryString["Contact_Id"]%>' != "") {
            opt = '<%= Request.QueryString["Contact_Id"]%>';
        }
                function GetData(user, date,docid) {
                    debugger;
                    if (docid == undefined)
                    {
                        docid="0";
                    }
                    if (docid !="0" )
                    {
                        GetDatabyDocId(DocId);
                      return false;
                    }
                    else
                    {
                        
                        debugger;
                        var DDLValue = "";
                        var Adate = "";
                        if (user == "0")
                        {
                            if ($('#lstuser').val() != null)
                            {
                                DDLValue = $('#lstuser').val();
                            }
                            else
                            {
                                DDLValue ="0";
                            }
                        }
                        else
                        {
                            DDLValue = user.value;
                            // alert(DDLValue);
                        }

                        if (date == "0")
                        {
                        

                           
                            Adate = $('#txtnxtactndt1').val();
                            var d = new Date();
                        }
                        else
                        {
                            Adate = date.value;
                        }
                        //  alert(DDLValue);
                        $.ajax({
                            type: "POST",
                            url: '<%= ResolveUrl("CRMDBService.asmx/GetTasksData") %>',
                            contentType: "application/json; charset=utf-8",
                            data: '{contactId : ' + opt + ',username:"' + DDLValue + '",Adate:"' + Adate + '"}',
                            dataType: "json",
                            success: function (data) {
                                jsdata1 = JSON.parse(data.d);
                                var output = '<table class="table"><tbody>';
                                output += '<tr class="tr" style="font-weight:Bold;color:#005580;font-size:16px"><td>Lead</td><td>Task Description</td><td>Owner</td><td>Lead Status</td><td>Task Status</td><td>Tag</td>';
                                output += '<tr class="tr" style="width:1px;background-color:#f5f5f5"><td></td><td> </td><td></td><td> </td><td> </td><td></td>';
                                $.each(jsdata1, function (key1, value1) {
                                    var colr = "Green";
                                    if (value1.Tstatus == "Close") {
                                        colr = "Red";
                                    }
                       
                                    //  var curr = new Date(value1.currentdt.getFullYear(),value1.currentdt.getMonth(),value1.currentdt.getDate());
                                    //  var dt = new Date(value1.assgndate.getFullYear(), value1.assgndate.getMonth(),value1.assgndate.getDate());
                                    //  var nextWeek = new Date(value1.weekdt.getFullYear(),value1.weekdt.getMonth(),value1.weekdt.getDate());
                                    var dt = new Date(value1.assgndate1);
                                    var nextWeek = new Date(value1.weekdt);
                                    var curr = new Date(value1.currentdt);

                                    //background: url(img / bg.png)
                                    var datebgimage4 = "datebgimage2";
                                    var colrdt = "orange";
                                   
                                    if (value1.currentdt > value1.assgndate1)
                                    {
                                        colrdt = "black";
                                        datebgimage4 = "datebgimage3";
                                    }
                                    else if (value1.currentdt < value1.assgndate1)
                                    {
                                        datebgimage4 = "datebgimage1";
                                    }
                                    else
                                    {
                                        datebgimage4 = "datebgimage2";
                                    }
                                    if (value1.Flag == "T") {
                                        output += '<tr class="tr"> <td style="cursor:pointer"> <a onclick="calltabtask(' + value1.contact_id + ');" ><span><img src="img/star.png">&nbsp; <b>' + value1.compname + ' </b>&nbsp;(' + value1.NAME + ')</span></a></td><td><span class=' + datebgimage4 + '><span class="tabledate"> ' + value1.assgndate + '</span></span style="color:' + colrdt + '">' + value1.task + '</td><td><b>' + value1.Towner + '</b></td><td></td><td><label style="color:' + colr + '">' + value1.Tstatus + '</label></td> <td><label ></label></td></tr>';
                                    }
                                    else {
                                        output += '<tr class="tr"> <td style="cursor:pointer"> <a onclick="calltab(' + value1.contact_id + ');" ><span><img src="img/star.png">&nbsp; <b>' + value1.compname + ' </b>&nbsp;(' + value1.NAME + ')</span></a></td><td><span class=' + datebgimage4 + '><span class="tabledate"> ' + value1.assgndate + '</span></span style="color:' + colrdt + '">' + value1.task + '</td><td><b>' + value1.Towner + '</b></td><td>' + value1.status + '</td><td><label style="color:' + colr + '">' + value1.Tstatus + '</label></td> <td><label >' + value1.Tag + '</label></td></tr>';
                                    }
                                });
                                output += " </tbody></table>";
                                $('#showdata').html(output);
                                $("#showdata td a").click(function () {

                                    $('.box').css('display', 'none');
                        
                                    $('.spinner').show();
                                    $('#spinnerfile').hide();

                                    $(document).ajaxComplete(function () {
                                        $('.spinner').hide();
                                        $('.box').css('display', 'block');
                                        $("#divbody").hide();
                                    });
                                });



                                $(window).load(function () {
                                    //$('body').css('display', 'block');

                                });
                            }
                        });
                    }
        }


        function activaTab(tab) {
            $('.nav-tabs a[href="#' + tab + '"]').tab('show');
            //alert('ddd');
        };

       
        function calltab(contactId) {
      
            //debugger;
            var username = '<%=Session["user_name"]%>';
            //if (username == "sa") {

            //}
            //else {
                SetPermissions("Add Action", "Add_Action");
                SetPermissions("Add Call", "save_add_call1");
                SetPermissions("Add Deal", "save_add_deal");
                SetPermissions("Add Note", "save_add_note");
                //SetPermissions("Add Lead", "ContentPlaceHolder1_Button2");
                //SetPermissions("Permission", "ContentPlaceHolder1_Button3");
          //  }
            $("#hidcontactId").val(contactId);
            $("#hidcontactId1").val(contactId);
            $("#hidcontactId2").val(contactId);
            GetContactDetails();
            GetContacts();
            GetContactsMail();
            getContactUrls();
           opt1 = contactId;
            //SetPermissions();
           activaTab('add_action_show');
            $("#table").hide();
            $("#details").show();
            $("#divbody").css("background-color", "#fffff");
           // alert($("#hidref_docId").val());
           
            $("#user_profile_off").hide();
            $("#user_profile_on").show();
            $("#user_menu").show();
            $("#bg_conatct_created").show();
            $("#divhis").show();
            $("#divtask").show();
            $("#taskdetailstetsing").show();
            //$("#background_comment").show();
            $(".bigcontainer").show(); 
            $("#compamy_members").hide();
           // $("#divbody").hide();
            $("#conatct_created").hide();
            $("#more").show();
            $("#divbody").hide();
        }

        function calltabtask(contactId) {

            //debugger;
            var username = '<%=Session["user_name"]%>';
            //if (username == "sa") {

            //}
            //else {
                SetPermissions("Add Action", "Add_Action");
                SetPermissions("Add Call", "save_add_call1");
                SetPermissions("Add Deal", "save_add_deal");
                SetPermissions("Add Note", "save_add_note");
              //  SetPermissions("Add Lead", "ContentPlaceHolder1_Button2");
             //   SetPermissions("Permission", "ContentPlaceHolder1_Button3");
           // }
            $("#hidcontactId").val(contactId);
            $("#hidcontactId1").val(contactId);
            $("#hidcontactId2").val(contactId);
            GetContactDetails();
            GetContacts();
            GetContactsMail();
            getContactUrls();
            opt1 = contactId;
            //SetPermissions();
          //  activaTab('add_call_deal');
            $("#table").hide();
            $("#details").show();
            $("#divbody").css("background-color", "#fffff");
            $("#divcontactDetail").hide();
            $('.nav.nav-tabs li').hide();
            //$('.nav.nav-tabs li:last-child()').show();
            activaTab('add_action_show');
            $("#user_profile_off").hide();
            $("#user_profile_on").show();
            $("#user_menu").show();
            $("#bg_conatct_created").show();
            $("#divhis").show();
            $("#divtask").show();
            $("#taskdetailstetsing").show();
            //$("#background_comment").show();
            $(".bigcontainer").show();
            $("#compamy_members").hide();
            // $("#divbody").hide();
            $("#conatct_created").hide();
            $("#more").show();
            $("#divbody").hide();
            $("#All").hide();
            $("#Notes").hide();
            $("#Deals").hide();
            $("#divLead").hide();
            GetCompleteHis(0);
        }
        </script>
            <script type="text/javascript">
        function GetContactDetails() {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetContactDataByID") %>',
                contentType: "application/json; charset=utf-8",
                data: '{contactId : ' + $("#hidcontactId").val() + '}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    var html="";
                    $.each(jsdata1, function (key1, value1) {

                        document.getElementById('p1').innerHTML = value1.Name
                        html = value1.Add1 + "<br>" + value1.contactCity + "<br>" + value1.contactstate + "<br>" + value1.contactcountry;
                        document.getElementById('contactadd').innerHTML = html
                   

                        document.getElementById('lblcrmcompname').innerHTML = value1.CompName
                        document.getElementById('lblcrmadd').innerHTML = value1.compadd
                        document.getElementById('lblcrmcity').innerHTML = value1.compcity
                        document.getElementById('lblcrmstate').innerHTML = value1.compstate

                        document.getElementById('lblcrmcountry').innerHTML = value1.compcountry
                        //document.getElementById('lblcrmemail').innerHTML = value1.Email
                        //document.getElementById('lblcrmmbl').innerHTML = value1.Phone
                        document.getElementById('lblcrmurl').innerHTML = value1.Url
                        document.getElementById('lblcompdesc').innerHTML = value1.Description
                        var a = document.getElementById('webadd')
                        a.href = "https://" + value1.Url
                        // alert(a.href);
                        $("#hidref_docId").val(value1.CTDocId);
                        //alert($("#hidref_docId").val());
                        $("#txtbg").val(value1.Background);
                        $("label[for='lblbg']").html(value1.Background);
                      //  alert($("#hidref_docId").val());
                        GetAllTag(value1.Tag_Id)
                        GetAllOwner(value1.manager)
                        //alert(value1.Manager)
                        GetAllStatus(value1.Status_Id);
                        // GetTaskDetails();
                        //  GetTaskNotes(value1.CTDocId); //GetTaskDeals(0);
                        Getcallnumbers(); //GetTaskCall(0);
                      //  GetEmailcontactname() ;
                        $("#myselectTag").val(value1.Tag_Id);
                        GetCompleteHis(0);
                        GetEmailcontactname() ;
                    })

                }
            });
        }

        function GetOwners() {
            $.ajax({
                type: "POST",
                <%--  url: '<%= ResolveUrl("CRMDBService.asmx/GetOwners") %>',--%>
                url: '<%= ResolveUrl("CRMDBService.asmx/GetAllOwner") %>',
                contentType: "application/json; charset=utf-8",
                data: '{}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    $("#lstowners").empty();
                    for (var i = 0; i < jsdata1.length; i++) {

                        var option = $("<option>");

                        for (var key in jsdata1[i]) {
                            // There should only be two keys, if its a number its ID, else option name
                            if (typeof jsdata1[i][key] === "number") {
                                option.attr("value", jsdata1[i][key]);
                            } else {
                                option.html(jsdata1[i][key]);
                            }
                        }
                        //$("#lstowners").empty();
                        $("#lstowners").append(option);

                    }


                    //$.each(jsdata1, function (key1, value1) {
                    //    alert(key1);
                    //    $('#lstowners').append($('<option>').text(value1.SMName).attr('value', value1.SMId));
                    //})
                    //$('.selown').multiselect({
                    //    enableCaseInsensitiveFiltering: true,
                    //    buttonWidth: '200px',
                    //    includeSelectAllOption: true,
                    //    maxHeight: 200,
                    //    width: 215,
                    //    enableFiltering: true,
                    //    filterPlaceholder: 'Search'
                    //})
                }
            });
        }
        function GetUserForFilter() {
            $.ajax({
                type: "POST",
                <%--  url: '<%= ResolveUrl("CRMDBService.asmx/GetOwners") %>',--%>
                url: '<%= ResolveUrl("CRMDBService.asmx/GetAllOwnerForFilter") %>',
                contentType: "application/json; charset=utf-8",
                data: '{}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    $.each(jsdata1, function (key1, value1) {
                        $('#lstuser').append($('<option>').text(value1.SMName).attr('value', value1.SMId));
                    })
                    //$('.selown').multiselect({
                    //    enableCaseInsensitiveFiltering: true,
                    //    buttonWidth: '200px',
                    //    includeSelectAllOption: true,
                    //    maxHeight: 200,
                    //    width: 215,
                    //    enableFiltering: true,
                    //    filterPlaceholder: 'Search'
                    //})
                }
            });
        }
        function GetAllStatus(selstatusId) {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetAllStatus") %>',
                contentType: "application/json; charset=utf-8",
                data: '{}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    $.each(jsdata1, function (key1, value1) {
                        if (value1.Status_Id == selstatusId) {
                            $('#myselectStatus').append($('<option selected="selected">').text(value1.Status).attr('value', value1.Status_Id));
                        }
                        else {
                            $('#myselectStatus').append($('<option>').text(value1.Status).attr('value', value1.Status_Id));
                        }
                    })
                }
            });
        }

        function GetAllTag(seltagId) {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetAllTag") %>',
                contentType: "application/json; charset=utf-8",
                data: '{}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    $.each(jsdata1, function (key1, value1) {
                        if (value1.Tag_Id == seltagId) {
                            $('#myselectTag').append($('<option selected="selected">').text(value1.Tag).attr('value', value1.Tag_Id));
                        }
                        else {
                            $('#myselectTag').append($('<option>').text(value1.Tag).attr('value', value1.Tag_Id));
                        }
                    })
                }
            });
        }
        function GetAllOwner(ManagerId)
        {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetAllOwner") %>',
                contentType: "application/json; charset=utf-8",
                data: '{}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    $.each(jsdata1, function (key1, value1) {
                        if (value1.SMID == ManagerId) {
                            //alert(value1.SMName);
                            $('#myOwnerList').append($('<option selected="selected">').text(value1.SMName).attr('value', value1.SMID));

                          //  $('#lstowners').append($('<option selected="selected">').text(value1.SMName).attr('value', value1.SMID));
                        }
                        else {
                            //alert(value1.SMName);
                            $('#myOwnerList').append($('<option>').text(value1.SMName).attr('value', value1.SMID));

                         //   $('#lstowners').append($('<option>').text(value1.SMName).attr('value', value1.SMID));
                        }
                    })
                }
            });
        }
        function changestatus(NstatusId) {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/UpdateStatus") %>',
                contentType: "application/json; charset=utf-8",
                data: '{status_Id:' + NstatusId + ', contactId:' + $("#hidcontactId").val() + '}',
                dataType: "json",
                success: function (data) {
                    Successmessage("Contact Status Updated");
                    //jsdata1 = JSON.parse(data.d);
                    //$.each(jsdata1, function (key1, value1) {
                    //    if (value1.Status_Id == NstatusId) {
                    //        $('#myselectStatus').append($('<option selected="selected">').text(value1.Status).attr('value', value1.Status_Id));
                    //    }
                    //    else {
                    //        $('#myselectStatus').append($('<option>').text(value1.Status).attr('value', value1.Status_Id));
                    //    }
                    //})
                }
            });
        }
        function changeOwner(OwnerId)
        {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/UpdateOwner") %>',
                contentType: "application/json; charset=utf-8",
                data: '{Owner_Id:' + OwnerId + ', contactId2:' + $("#hidcontactId2").val() + '}',
                dataType: "json",
                success: function (data) {
                    Successmessage("Owner Updated");

                }
            });
        }
        function changeTag(NtagId) {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/UpdateTag") %>',
                contentType: "application/json; charset=utf-8",
                data: '{Tag_Id:' + NtagId + ', contactId1:' + $("#hidcontactId1").val() + '}',
                dataType: "json",
                success: function (data) {
                    Successmessage("Contact Tag Updated");
                
                }
            });
        }
        function changeTaskstatus(NTstatus) {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/UpdateStatusTask") %>',
                contentType: "application/json; charset=utf-8",
                data: '{status: "' + NTstatus + '",Tdocid:"' + $("#hidref_docId").val() + '"}',
                dataType: "json",
                success: function (data) {
                    Successmessage("Task Status Updated");
                    //jsdata1 = JSON.parse(data.d);
                    //$.each(jsdata1, function (key1, value1) {
                    //    if (value1.Status_Id == NstatusId) {
                    //        $('#myselectStatus').append($('<option selected="selected">').text(value1.Status).attr('value', value1.Status_Id));
                    //    }
                    //    else {
                    //        $('#myselectStatus').append($('<option>').text(value1.Status).attr('value', value1.Status_Id));
                    //    }
                    //})
                }
            });
        }

        function SaveNextAction() {
            if ($('#txtnxtaction').val() == "") {
                errormessage("Please enter a task");
                return false;
            }
            if ($('#txtnxtactndt').val() == "") {
                errormessage("Please select a date");
                return false;
            }

            if ($('#lstowners').val() == null) {
                errormessage("Please select a Person");
                return false;
            }
            var DocId = "0";
            if ($('#hid_TdocId').val() != "") {
                DocId = $('#hid_TdocId').val();
            }
            if (RefId == 'T')
            {
                $("#hidref_docId").val('');
            }
           
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("~/CRMDBService.asmx/SaveNxtTask") %>',
                contentType: "application/json; charset=utf-8",
                data: '{TDocId: "' + DocId + '",time:"'+ $("#txttime").val()+'",task: "' + $("#txtnxtaction").val() + '",contactId: "' + $("#hidcontactId").val() + '",Asgndate: "' + $("#txtnxtactndt").val() + '",AsgnTo: "' + $('#lstowners').val() + '",Ref_DocId: "' + $("#hidref_docId").val() + '" ,Status: "' + $("#myselectTaskStatus").val() + '"  }',
                dataType: "json",
                success: function (data) {
                    var FdocId = JSON.parse(data.d);
                    if (FdocId != null) {
                        $("#hidref_docId").val(FdocId.SetMsg);
                    }
                    Successmessage("Task Saved");
                    cleartask();
                    debugger;
                    var username = '<%=Session["user_name"]%>';
                    //if (username == "sa") {

                    //}
                    //else {
                        SetPermissions("Add Action", "Add_Action");

                   // }
                    $('#taskdetailstetsing1').html('');
                    $('#taskdetailstetsing4').html('');
                    $('#taskdetailstetsing2').html('');
                    $('#taskdetailstetsing3').html('');
                    if ($('#listid').val() == "Actions")
                    {
                        GetTaskDetails();
                    }
                    else {
                        GetCompleteHis(0);
                    }
                   
                    // GetTaskDetails();
                }
            });

        }
                function editTask(value) {



                    activaTab('add_action_show');
                    $('#Add_Action').show();

            $('#hid_TdocId').val(value);
         //   cleartask();
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetTaskDetailByID") %>',
                contentType: "application/json; charset=utf-8",
                data: '{DocId: "' + value + '"}',
                dataType: "json",
                asyn:false,
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    $.each(jsdata1, function (key1, value1) {

                        $("#txtnxtaction").val(value1.Task);

                        $("#txtnxtactndt").val(value1.Adate);
                        $('#myselectTaskStatus').val(value1.Status);
                       // $('#txttime').defaultValue(value1.time);
                        var lblTime = document.getElementById("txttime");
                       
                           value1.time=  value1.time.replace("AM", "");
                           value1.time=  value1.time.replace("PM", "");
                           lblTime.defaultValue =value1.time; 
                        $("#lstowners").val(value1.AssignedToId);
                        //alert(value1.time);
                    });
                }
            });
        }

        function deleteTask(value) {
            if (confirm('Do you want to delete ?')) {
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("CRMDBService.asmx/DeleteTask") %>',
                    contentType: "application/json; charset=utf-8",
                    data: '{Tdocid:"' + value + '"}',
                    dataType: "json",
                    success: function (data) {
                        var msgval = JSON.parse(data.d);
                       // Successmessage("Task Deleted");
                        console.log(msgval);
                        debugger;
                        if (msgval.SetMsg == "Y") {
                            Successmessage("Task Deleted");
                            if ($('#listid').val() == "Actions") {
                                GetTaskDetails(0);
                            }
                            else {
                                GetCompleteHis(0);
                            }
                            cleartask();
                            debugger;
                            if (value == $("#hidref_docId").val().split(" ").join(""))
                            {
                                $("#hidref_docId").val('');
                            }


                            //  GetTaskDetails();
                        }
                        else {
                            errormessage("Please delete child task first");
                        }
                       

                    }
                });
            }

        }
        function cleartask() {
            $('#txtnxtaction').val(''); $('#txtnxtactndt').val('');
            $('#hid_TdocId').val('');
         
            $("#lstowners option").removeAttr("selected");
          
        }
        function GetTaskDetails(x) {

            var Editallow = SetPermissions("Edit", "");
            var delallow = SetPermissions("Delete", "");
            var EditP = "";
            var DelP = "";

            if (Editallow == "true") {
             
              //  EditP = "enabled";
                EditP="";
            }
            else {
                //EditP = "disabled";
                EditP="visibility: hidden;";
            }
            if (delallow == "true") {
              
                //  DelP = "enabled";
                DelP="";
            }
            else {
             
               // DelP = "disabled";
                DelP="style="+"\"visibility: hidden;\"";
            }
            $('#taskdetailstetsing1').html('');
            $('#taskdetailstetsing4').html('');
            $('#taskdetailstetsing2').html('');
            $('#taskdetailstetsing3').html('');
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetTaskDetails") %>',
                contentType: "application/json; charset=utf-8",
                data: '{Ref_DocId:"' + $("#hidref_docId").val() + '"}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    var output = "<ul class='timeline'  style='margin:0px;'>";
                    var Previous = "";
                    $.each(jsdata1, function (key1, value1) {
                        if ($("#hidref_docId").val().split(" ").join("") == value1.DocId) {
                            $('#myselectTaskStatus').val(value1.Status);
                        }
                        
                        if (value1.DisplayDate != Previous) {
                            output += "<li class='time-label'><span class='bg-red'>" + value1.DisplayDate + "</span></li>";
                        }
                        
                        output += " <li style='margin:0px;'><span class='bg-yellow' style='padding: 3px 13px;margin-left: 5px;border-radius: 0px 45px 45px 0px;'>Action</span><div class='timeline-item'>  <div class='timeline-body'><span style='color:#0073b7'><b>Task - </b></span>" + value1.Task + " <div class='clearfix'></div><span style='color:#0073b7'><b>By - </b></span>" + value1.Smname.replace(".", "Admin") + "<span style='color:#0073b7'> <b>, To - </b></span>" + value1.AssignedTo + "  &nbsp;&nbsp; " + value1.Adate + "<div class='clearfix'></div> </div><div class='timeline-footer'><a class='btn btn-primary btn-xs' onclick=deleteTask('" + value1.DocId + "'); " + DelP + ">Delete</a><a class='btn btn-danger btn-xs' style='margin-left:5px; " + EditP + "' onclick=editTask('" + value1.DocId + "');>Edit</a></div></li>";
                     
                        Previous = value1.DisplayDate;
                    });
                    output += "</ul>";


                    $('#taskdetailstetsing2').html(output);


                }
            });
        }

        function GetTaskNotesCall(x)
        {


            var Editallow = SetPermissions("Edit", "");
            var delallow = SetPermissions("Delete", "");
            var EditP = "";
            var DelP = "";

            if (Editallow == "true") {
               
              //  EditP = "enabled";
                EditP="";
            }
            else {
               // EditP = "disabled";
                EditP="visibility: hidden;";
            }
            if (delallow == "true") {
              
              //  DelP = "enabled";
                DelP="";
            }
            else {
               
               // DelP = "disabled";
                DelP="style="+"\"visibility: hidden;\"";
            }

            $('#taskdetailstetsing1').html('');
            $('#taskdetailstetsing4').html('');
            $('#taskdetailstetsing2').html('');
            $('#taskdetailstetsing3').html('');
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetTaskNoteCall") %>',
                contentType: "application/json; charset=utf-8",
                data: '{ref_docid:"' + $("#hidref_docId").val() + '"}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    var output = "<ul class='timeline'  style='margin:0px;'>";
                    var Previous = "";
                    $.each(jsdata1, function (key1, value1) {

                        if (value1.DisplayDate != Previous) {
                            output += "<li class='time-label'><span class='bg-red'>" + value1.DisplayDate + "</span></li>";
                        }
                        if (value1.Type == "N") {
                            output += " <li style='margin:0px;'><span class='bg-blue' style='padding: 3px 13px;margin-left: 5px;border-radius: 0px 45px 45px 0px;'>Notes</span> <div class='timeline-item'>  <div class='timeline-body'>" + value1.Notes + "<div class='clearfix'></div><span style='color:#0073b7'><b>By - </b></span>" + value1.username + "  &nbsp;&nbsp; " + value1.DisplayDate + "</div><div class='timeline-footer'><a class='btn btn-primary btn-xs' onclick=deleteNote('" + value1.Task_NoteID + "'); " + DelP + ">Delete</a><a class='btn btn-danger btn-xs' style='margin-left:5px;" + EditP + "' onclick=editNote('" + value1.Task_NoteID + "'); >Edit</a></div></li>";
                        }
                        else if (value1.Type == "C") {
                          
                            output += " <li style='margin:0px;'><span class='bg-green' style='padding: 3px 13px;margin-left: 5px;border-radius: 0px 45px 45px 0px;'>Calls</span><div class='timeline-item'>  <div class='timeline-body'><span style='color:#0073b7'><b>Phone - </b></span>" + value1.Phone + '<div class="clearfix"></div><div class="clearfix"></div><span style="color:#0073b7"><b>Call Notes - </b></span>' + value1.CallNotes + "<div class='clearfix'></div><span style='color:#0073b7'><b>By - </b></span>" + value1.username + "  &nbsp;&nbsp; " + value1.DisplayDate + "</div><div class='timeline-footer'><a class='btn btn-primary btn-xs' onclick=deleteCall('" + value1.Task_CallID + "'); " + DelP + ">Delete</a><a class='btn btn-danger btn-xs' style='margin-left: 5px;" + EditP + "' onclick=editCall('" + value1.Task_CallID + "'); >Edit</a></div></li>";
                        }
                        else if (value1.Type == "E") {
                          
                            
                            alert( "E")
                            output += " <li style='margin:0px;'><span class='bg-green' style='padding: 3px 13px;margin-left: 5px;border-radius: 0px 45px 45px 0px;'>Emails</span><div class='timeline-item'>  <div class='timeline-body'><span style='color:#0073b7'><b>Email To - </b></span>" + value1.Phone + '<div class="clearfix"></div><div class="clearfix"></div><span style="color:#0073b7"><b>Email Notes - </b></span>' + value1.CallNotes + "<div class='clearfix'></div><span style='color:#0073b7'><b>By - </b></span>" + value1.username + "  &nbsp;&nbsp; " + value1.DisplayDate + "</div><div class='timeline-footer'><a class='btn btn-primary btn-xs' onclick=deleteCall('" + value1.Task_CallID + "'); " + DelP + ">Delete</a><a class='btn btn-danger btn-xs' style='margin-left: 5px;" + EditP + "' onclick=editCall('" + value1.Task_CallID + "'); >Edit</a></div></li>";
                        }
                       

                        Previous = value1.DisplayDate;
                    });
                    output += "</ul>";


                    $('#taskdetailstetsing2').html(output);


                }
            });

        }

        function GetTaskNotes(TaskNoteId, x) {
            $('#taskdetailstetsing1').html('');
            $('#taskdetailstetsing4').html('');
            $('#taskdetailstetsing2').html('');
            $('#taskdetailstetsing3').html('');
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetTaskNote") %>',
                contentType: "application/json; charset=utf-8",
                data: '{TaskDocId: "' + $("#hidref_docId").val() + '",TaskNoteId:"' + TaskNoteId + '"}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    //var output = "<div class='col-md-1' style='s=width:4px'>Notes</div><div class='col-md-11 ' style='padding-left:0px'><ul>";
                    //<div class='col-md-12' >


                    var output = "<ul class='timeline' style='margin:0px;'>";
                    var Previous = "";
                    $.each(jsdata1, function (key1, value1) {
                        //output += "<li>" + value1.Notes + "<a onclick=deleteNote('" + value1.Task_NoteID + "');><label class='col-dm-1 remove control-label'><i class='fa fa-fw fa-trash-o'></i></label></a><a onclick=editNote('" + value1.Task_NoteID + "');><label class='col-dm-1 control-label'><i class='fa fa-fw fa-pencil'></i></label></a></li>";
                        if (value1.DisplayDate != Previous) {
                            output += "<li class='time-label'><span class='bg-red'>" + value1.DisplayDate + "</span></li>";
                        }
                        output += " <li style='margin:0px;'><span class='bg-blue' style='padding: 3px 13px;margin-left: 5px;border-radius: 0px 45px 45px 0px;'>Notes</span><div class='timeline-item'>  <div class='timeline-body'>" + value1.Notes + "</div><div class='timeline-footer'><a class='btn btn-primary btn-xs' onclick=deleteNote('" + value1.Task_NoteID + "');>Delete</a><a class='btn btn-danger btn-xs' style='margin-left:5px;' onclick=editNote('" + value1.Task_NoteID + "');>Edit</a></div></li>";
                        Previous = value1.DisplayDate;
                    });
                    output += "</ul>";

                    // $('#divnotes').html(output);

                    $('#taskdetailstetsing1').html(output);


                }
            });
        }
                function SaveTaskNote() {

                    if ($("#hidref_docId").val() =="")
                    {
                        errormessage("Please Add Action first");
                        clrNotes();
                        return false;
                    }
                    if ($('#txtaddnote').val() == "") {
                        errormessage("Please enter a  Note");
                        return false;
                    }                   
                   
            
            if ($.trim($('#txtaddnote').val()).length > 0) {
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("~/CRMDBService.asmx/InsertUpdTaskNote") %>',
                    contentType: "application/json; charset=utf-8",
                    data: '{TaskDocID: "' + $("#hidref_docId").val() + '",Note: "' + $("#txtaddnote").val() + '",NoteId: "' + $("#hidnoteId").val() + '"}',
                    dataType: "json",
                    success: function (data) {
                        if ($.trim($("#hidnoteId").val()) != "" && $("#hidnoteId").val() != "0") {
                            Successmessage("Note Updated");
                        }
                        else
                            Successmessage("Note Saved");
                        $('#txtaddnote').val('');// $("#add_note_show").hide();
                        $("#hidnoteId").val('');
                        var username = '<%=Session["user_name"]%>';
                        //if (username == "sa") {

                        //}
                        //else {

                            SetPermissions("Add Note", "save_add_note");

                    //    }
                        //GetTaskNotes(0);
                        $('#taskdetailstetsing1').html('');
                        $('#taskdetailstetsing4').html('');
                        $('#taskdetailstetsing2').html('');
                        $('#taskdetailstetsing3').html('');
                        if ($('#listid').val() == "Notes")
                        {
                            //GetTaskNotes(0);
                        }
                        else
                        {
                            GetCompleteHis(0);
                        }
                       
                      ///  $("#hidnoteId").val(0);
                    }
                });
            } else {
                errormessage("Please enter a note");
            }
        }
        function editNote(TaskNoteId) {
            //alert("111");
            activaTab('add_note_show');
            $("#save_add_note").show();
            cleartask();
            $("#hidnoteId").val(TaskNoteId);
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetTaskNote") %>',
                contentType: "application/json; charset=utf-8",
                data: '{TaskDocId: "' + $("#hidref_docId").val() + '",TaskNoteId:"' + TaskNoteId + '"}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    $.each(jsdata1, function (key1, value1) {
                        $("#txtaddnote").val(value1.Notes);
                        //$("#add_note_show").show();
                    });
                }
            });
        }
        function deleteNote(TaskNoteId) {
            if (confirm('Do you want to delete ?'))
            {

                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("CRMDBService.asmx/DeleteNote") %>',
                    contentType: "application/json; charset=utf-8",
                    data: '{TaskNoteId:"' + TaskNoteId + '"}',
                    dataType: "json",
                    success: function (data) {
                        Successmessage("Note Deleted");
                        if ($('#listid').val() == "Notes") {
                            GetTaskNotes(0);
                        }
                        else {
                            GetCompleteHis(0);
                        }
                      
                        $("#hidnoteId").val('');
                        clrNotes();

                    }
                });
            }

        }


                function GetTaskDeals(TaskDealId, x) {


                    var dealdetail= SetPermissions("Deal Detail","");
                    var Editallow = SetPermissions("Edit", "");
                    var delallow = SetPermissions("Delete", "");
                    var EditP = "";
                    var DelP = "";
                    var viewP="";
                    if (dealdetail == "true")
                    {
                        viewP="";
                    }
                    else{
                        viewP="style="+"\"visibility: hidden;\"";
                    }
            
                    if (Editallow == "true") {
                       
                        //EditP = "enabled";
                        EditP="";
                    }
                    else {
                       // EditP = "disabled";
                        EditP="visibility: hidden;";
                    }
                    if (delallow == "true") {
                       
                       // DelP = "enabled";
                        DelP="";
                    }
                    else {
                       
                     //   DelP = "disabled";
                        DelP="style="+"\"visibility: hidden;\"";
                    }
            $('#taskdetailstetsing1').html('');
            $('#taskdetailstetsing4').html('');
            $('#taskdetailstetsing2').html('');
            $('#taskdetailstetsing3').html('');
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetTaskDeal") %>',
                contentType: "application/json; charset=utf-8",
                data: '{TaskDocId: "' + $("#hidref_docId").val() + '",TaskDealId:"' + TaskDealId + '"}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);


                    var output = "<ul class='timeline'  style='margin:0px;'>";    
                    var Previous = "";
                    $.each(jsdata1, function (key1, value1) {
                       
                        if (value1.DisplayDate != Previous)
                        {
                            output += "<li class='time-label'><span class='bg-red'>" + value1.DisplayDate + "</span></li>";
                        }
                        output += " <li style='margin:0px;'><span class='bg-red' style='padding: 3px 13px;margin-left: 5px;border-radius: 0px 45px 45px 0px;'>Deals</span><div class='timeline-item'>  <div class='timeline-body'><span style='color:#0073b7'><b>Deal Name-</b></span>" + value1.DealName + "<div class='clearfix'></div> <span style='color:#0073b7'> <b>Deal date- </b></span>" + value1.DealDate + "<div class='clearfix'></div> <span style='color:#0073b7'> <b>Exp date- </b></span>" + value1.ExpClsDt + "<div class='clearfix'></div><span style='color:#0073b7'><b>Deal Note- </b></span>" + value1.DealNote + "<a class='viewanchor' title='View Detail' onclick=viewdetail('" + value1.Task_DealID + "'); " + viewP + "><i class='glyphicon glyphicon-info-sign viewdetail'></i></a><div class='clearfix'></div> <div class='timeline-footer'><a class='btn btn-primary btn-xs' onclick=deleteDeal('" + value1.Task_DealID + "'); " + DelP + ">Delete</a><a class='btn btn-danger btn-xs' style='margin-left: 5px; " + EditP + "' onclick=editDeal('" + value1.Task_DealID + "'); >Edit</a></div></li>";
                      
                        Previous = value1.DisplayDate;
                  
                    });
                    output += "</ul>";
                    $('#taskdetailstetsing3').html(output);
                }
            });
        }

        function ShowDealMsg(val) {
            if (val == "w") {
                $("#divwondeal").show();
                $("#divlossdeal").hide();
                $("#awon").addClass('showbg');
                $("#aloss").removeClass('showbg');
                $("#selstage").removeClass('showbg');
            }
            else if (val == "l") {
                $("#divlossdeal").show();
                $("#divwondeal").hide();
                $("#aloss").addClass('showbg');
                $("#awon").removeClass('showbg');
                $("#selstage").removeClass('showbg');
            }
            else {
                $("#divlossdeal").hide();
                $("#divwondeal").hide();
                $("#selstage").addClass('showbg');
                $("#aloss").removeClass('showbg');
                $("#awon").removeClass('showbg');
            }
        }
        $('#txtComm').keyup(function(){
            if ($(this).val() > 100){
                alert("Commission can not be greater than 100");
                $(this).val('100');
            }
        });
        function ShowMD() {
            if ($("#dealhead").html() == "Revert") {
                $("#spandeal").hide();
                $("#txtmonthdeal").val('');
                $('#dealtv').html('0.0')
                $("#dealhead").html('Multi-month deal?');
            }
            else {
                $("#spandeal").show();
                $("#dealhead").html('Revert');
            }
        }

        function clrNotes()
        {
            $("#txtaddnote").val('');
            $("#hidnoteId").val('');
        }

        function clrCall()
        {
            $('#txtcallnote').val('');
            $("#hidTcallId").val('');
            $('#txtaddemailnote').val('');
            $('#myselectemailall').val('0');
        }
        function getTV() {
            $('#dealtv').html((parseFloat($('#txtamt').val()) * parseFloat($('#txtmonthdeal').val())).toFixed(3));
            calcommamt();
        }
        function calcommamt()
        {
            debugger;
            var totalamt=0;
            var commissionamt=0;
            var remainingamt=0;
            var parDT = $(".dt").parent().attr('class');
            $('.' + parDT + ' .dealamount').each(function () {

                totalamt= totalamt + Number($(this).val()) ;
            })
            // alert(totalamt);
            
            commissionamt = totalamt * $('#txtComm').val()/100;
            remainingamt = totalamt - commissionamt;

           // alert(remainingamt);
            $('#txttotaldealamt').val(remainingamt);
        }
        function SaveDeal() {
            
            var valid = "1";
            if ($("#hidref_docId").val() =="")
            {
                errormessage("Please Add Action first");
                clrdeals(0);
                return false;
            }
            //else if ($.trim($('#txtdealname').val()) == "") {
            //    errormessage("Please enter Deal name");
            //    return false;
            //}
            else if ($('#txtdealdate').val() == "") {
                errormessage("Please enter Deal date");
                return false;
            }           
            else if ($('#txtexpclsdate').val() == "") {
                errormessage("Please enter Exp. close date");
                return false;
            }
            else if($('#txtdealstate').val() =="" )
            {
                errormessage("Please enter Deal Stage");
                return false;
            }
            else if ($('#txtamt').val() == "") {
                errormessage("Please enter Amount");
                return false;
            }
            else if ($('#dd').val() == "") {
                errormessage("Please enter payment date");
                return false;
            }
            else if ($('#txtdealnote').val() == "") {
                errormessage("Please enter Deal Note");
                return false;
            }         
          

            if (valid == "1") {

                var amt = 0, dealstage = "p";
                if ($('#dealtv').html() != "" && $('#dealtv').html() != "0.0") {
                    if (parseInt($('#dealtv').html())) {
                        amt = $('#dealtv').html();
                    }
                    else {
                        amt = $('#txtamt').val();
                    }
                } else { amt = $('#txtamt').val(); }

                if ($("#awon").hasClass('showbg')) {
                    dealstage = "w";
                }
                else if ($("#aloss").hasClass('showbg')) {
                    dealstage = "l";
                }
                // var fileNames ="" ;
                debugger;
                var z=0;
                var parDT = $(".dt").parent().attr('class');
                var dealval = ""; var dealamt = "";var stageid="";var paydate="";var qty="";var rate="";
                $('.' + parDT + ' .dealtext').each(function () {
                    dealval += $(this).val() + ",";
                    z=z+1;
                })
                dealval = dealval.substr(0, dealval.length - 1);
                console.log(dealval);
             //   alert(z);
                $('.' + parDT + ' .dealamount').each(function () {
                    dealamt += $(this).val() + ",";
                })
                dealamt = dealamt.substr(0, dealamt.length - 1);
                console.log(dealamt);
                $('.' + parDT + ' .hiddenid').each(function () {
                    stageid += $(this).val() + ",";
                })
                stageid = stageid.substr(0, stageid.length - 1);


                $('.' + parDT + ' .dealQty').each(function () {
                    qty += $(this).val() + ",";
                })
                qty = qty.substr(0, qty.length - 1);


                
                $('.' + parDT + ' .dealRate').each(function () {
                    rate += $(this).val() + ",";
                })
                rate = rate.substr(0, rate.length - 1);
                //$('.' + parDT +' .paydate').each(function(){
                //    paydate+= $(this).val() + ",";
                //})
                //paydate = paydate.substr(0,paydate.length - 1);
                /////////////Upload File

                $("#selstage").val('');
                arrfilename=""
                $('#spinnerfile').show();
                var uploadedFiles = $('[name="ctl00$ContentPlaceHolder1$uploader"]');
                if (uploadedFiles.length > 0) {
                    var formData = new FormData();
                    for (var i = 0; i < uploadedFiles.length; i++) {
                        if (uploadedFiles[i].files.length>0)
                        {
                            formData.append(uploadedFiles[i].files[0].name, uploadedFiles[i].files[0]);
                            arrfilename +=uploadedFiles[i].files[0].name +",";
                        }
                    }
                   
                    formData.append('SMID', <%=BusinessLayer.Settings.Instance.SMID%>);
                     
                    console.log(formData); 
                      
                }
                var progressbarLabel = $('#progressbar-label');
                var progressbarDiv = $('#progress-bar');
                debugger;
                $.ajax
                    ({
                        url: 'Upload.ashx',
                        method: 'post',
                        contentType: false,
                        dataType: "json",                          
                        processData: false,
                        data: formData,
                        responseType: "json",
                         
                        success: function (data) {
                            debugger;
                            if( data != null){
                                debugger;
                                arr = data;
                                alert("Uploaded Successfully");   
                                $.ajax({
                                    type: "POST",
                                    url: '<%= ResolveUrl("~/CRMDBService.asmx/SaveDeal") %>',
                                    contentType: "application/json; charset=utf-8",
                                    data: '{Task_DealID: "' + $("#hidTdealId").val() + '",CRM_TaskDocID: "' + $("#hidref_docId").val() + '",DealName: " ",Amount: "' + $('#txttotaldealamt').val() + '",DealDate: "' + $("#txtdealdate").val() + '",ExpClsDt: "' + $("#txtexpclsdate").val() + '",DealStage: "' + dealstage + '",DealStagePerc: "' + $("#selstage").val() + '",DealNote: "' + $("#txtdealnote").val() + '",MonthlyDeal: "' + $("#txtmonthdeal").val() + '",TotalDealAmt: "' + amt + '",commper: "' + $("#txtComm").val() + '",commamt: "' + $("#txtcommamt").val() + '"}',
                                    dataType: "json",
                                    asyn:false,
                                    success: function (data) {
                                        debugger;
                                        Successmessage("Deal Saved");
                                        console.log(data);
                                        DealID = data.d;
                                       





                                        debugger;                       
                                        console.log(arr);
                                        if (arr != undefined && arrfilename !=undefined)
                                        {
                                            $.ajax({
                                                type: "POST",
                                                url: '<%= ResolveUrl("~/CRMDBService.asmx/SaveFilePath") %>',
                                                contentType: "application/json; charset=utf-8",    
                                                processData: false,                           
                                                data: '{Task_DealID: "' + DealID + '", FilePath: "' + arr.replace(/\\/g, "\\\\") + '",Filename:"' + arrfilename + '"}',
                                                dataType: "json",                           
                                                success: function (data) {
                                                    //  //debugger;
                                                    console.log("Deal Saved");                      
                                
                                                }
                                            });
                                        }
                                     ///   alert(z);
                                        savedealstage(DealID,dealval,dealamt,z,stageid,qty,rate);
                                        $('#taskdetailstetsing1').html('');
                                        $('#taskdetailstetsing4').html('');
                                        $('#taskdetailstetsing2').html('');
                                        $('#taskdetailstetsing3').html('');
                                        GetCompleteHis(0);
                                        clrdeals(z);
                                        var username = '<%=Session["user_name"]%>';
                                        //if (username == "sa") {

                                        //}
                                        //else {

                                        SetPermissions("Add Deal", "save_add_deal");

                                        //}
                                        //$("#add_call_deal").hide();
                                    }
                                });
                                   
                            }
                            $('#spinnerfile').hide();
                          
                            progressbarLabel.text('Uploaded Successfully');
                            progressbarDiv.fadeOut(2000);
                             
                        },
                        error: function (err) {
                            debugger;
                               
                                $.ajax({
                                    type: "POST",
                                    url: '<%= ResolveUrl("~/CRMDBService.asmx/SaveDeal") %>',
                                    contentType: "application/json; charset=utf-8",
                                    data: '{Task_DealID: "' + $("#hidTdealId").val() + '",CRM_TaskDocID: "' + $("#hidref_docId").val() + '",DealName: " ",Amount: "' + $('#txttotaldealamt').val() + '",DealDate: "' + $("#txtdealdate").val() + '",ExpClsDt: "' + $("#txtexpclsdate").val() + '",DealStage: "' + dealstage + '",DealStagePerc: "' + $("#selstage").val() + '",DealNote: "' + $("#txtdealnote").val() + '",MonthlyDeal: "' + $("#txtmonthdeal").val() + '",TotalDealAmt: "' + amt + '",commper: "' + $("#txtComm").val() + '",commamt: "' + $("#txtcommamt").val() + '"}',
                                    dataType: "json",
                                    asyn:false,
                                    success: function (data) {
                                        ////debugger;
                                        Successmessage("Deal Saved");
                                        console.log(data);
                                        DealID = data.d;
                                        debugger;                       
                                        console.log(arr);
                                        if (arr != undefined && arrfilename !=undefined)
                                        {
                                            $.ajax({
                                                type: "POST",
                                                url: '<%= ResolveUrl("~/CRMDBService.asmx/SaveFilePath") %>',
                                                contentType: "application/json; charset=utf-8",    
                                                processData: false,                           
                                                data: '{Task_DealID: "' + DealID + '", FilePath: "' + arr.replace(/\\/g, "\\\\") + '",Filename:"' + arrfilename + '"}',
                                                dataType: "json",                           
                                                success: function (data) {
                                                    //  //debugger;
                                                    console.log("Deal Saved");                      
                                
                                                }
                                            });
                                        }
                                        debugger;
                                        savedealstage(DealID,dealval,dealamt,z,stageid,qty,rate);
                                        $('#taskdetailstetsing1').html('');
                                        $('#taskdetailstetsing4').html('');
                                        $('#taskdetailstetsing2').html('');
                                        $('#taskdetailstetsing3').html('');
                                        GetCompleteHis(0);
                                        clrdeals(z);
                                        var username = '<%=Session["user_name"]%>';
                                        //if (username == "sa") {

                                        //}
                                        //else {

                                        SetPermissions("Add Deal", "save_add_deal");

                                        //}
                                        //$("#add_call_deal").hide();
                                    }
                                });
                                   
                            
                            $('#spinnerfile').hide();
                          
                            progressbarLabel.text('Uploaded Successfully');
                            progressbarDiv.fadeOut(2000);
                        }
                    });
                progressbarLabel.text('Uploading...');
           
                console.log(arr);
        
            }
         
        }
            
                function editDeal(TaskDealId) {
                    $('#save_add_deal').show();
                    debugger;   
            activaTab('add_call_deal');
            $("#hidTdealId").val(TaskDealId);
         //   cleartask();
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetTaskDeal") %>',
                contentType: "application/json; charset=utf-8",
                data: '{TaskDocId: "' + $("#hidref_docId").val() + '",TaskDealId:"' + TaskDealId + '"}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    $.each(jsdata1, function (key1, value1) {
                        if (value1.DealStage == "Won") {
                            $("#awon").addClass('showbg')
                        }
                        else if (value1.DealStage == "Lost") {
                            $("#aloss").addClass('showbg')
                        }
                        else {
                            $("#selstage").addClass('showbg')
                        }
                        if (parseInt(value1.MonthlyDeal) > 0) {
                            $("#spandeal").show(); $("#dealhead").html('Revert');
                        } else {
                            $("#spandeal").hide(); $("#dealhead").html('Multi-month deal?');
                        }
                        //$("#txtdealname").val(value1.DealName);
                      //  $("#txtamt").val(value1.Amount);
                        $("#txtdealdate").val(value1.DealDate);
                        $("#txtexpclsdate").val(value1.ExpClsDt);
                        $("#selstage").val(value1.DealStagePerc);
                        $("#txtdealnote").val(value1.DealNote);
                        $("#txtmonthdeal").val(value1.MonthlyDeal);
                        $('#dealtv').html(value1.TotalDealAmt);
                        $('#txtComm').val(value1.commper);
                        $('#txtcommamt').val(value1.commamt);
                        $('#txttotaldealamt').val(value1.Amount);
                        //$("#add_call_deal").show();
                    });
                    selDiv = document.querySelector("#ContentPlaceHolder1_uploader_wrap_list");
                   // Fileuploader = document.querySelector("#ContentPlaceHolder1_uploader");
                    selDiv.innerHTML ="";
                    var str="";
                    var str1="";
                    var fileID="";
                    var i=1;
                    fileID="#ContentPlaceHolder1_uploader";
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("~/CRMDBService.asmx/getFilePath") %>',
                        contentType: "application/json; charset=utf-8",    
                        processData: false,                           
                        data: '{Task_DealID: "' + TaskDealId + '"}',
                        dataType: "json",                           
                        success: function (data) {
                            jsdata1 = JSON.parse(data.d);
                            $.each(jsdata1, function (key1, value1) {
                                str="";
                                str +="<div class='MultiFile-label' id='divfile" + i + "'><a  onclick='removefile(divfile" + i + "," + TaskDealId + ",\"" + value1.filepath.replace(/\\/g, "\\\\") + "\");'><img src='img/delete.png' /></a> ";
                                str +="<a class='MultiFile-title' href="+ value1.filepath + " target='_blank' >"+ value1.filename +"</a></div>";                             
                                selDiv.innerHTML +=str;
                               
                                i++;
                            });                
                                
                        }
                    });

                    $("#dealstage").html('');
                    var val2 = $('.dt').length ;
                    var k=0;
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("~/CRMDBService.asmx/getdealstage") %>',
                        contentType: "application/json; charset=utf-8",    
                        processData: false,                           
                        data: '{Task_DealID: "' + TaskDealId + '"}',
                        dataType: "json",                           
                        success: function (data) {
                            jsdata1 = JSON.parse(data.d);
                            
                            var html1="";
                            $.each(jsdata1, function (key1, value1) {
                                debugger;
                              
                             
                                
                                if (k==0)
                                {                            
     
                                    html1 += '<div class="clearfix"></div><div class="dt" id="divstage' + val2 + '" >';
                                    html1 +='<div class="col-md-2 paddingleft0"><div class="form-group">';
                                    html1 +='<label style="font-size: small">Mile Stone: </label>';
                                    html1 +=' <input type="text" value="'+ value1.DealText +'" style="width: 100%" id="txtdealstate" maxlength="150" class="form-control dealtext" /></div></div>';
                                   
                                    html1 +=' <div class="col-md-1 paddingleft0"> <div class="form-group"><label style="font-size: small">Qty: </label>';
                                    html1 +='<input type="text" onblur="javascript:getTV()" id="txtQty" maxlength="10" class="numeric text-right form-control dealQty" value="'+ value1.Qty +'" onchange="calamt('+ val2 + ')" onkeypress="return isNumberKey(event)">';
                                    html1 +='</div></div><div class="col-md-1 paddingleft0"><div class="form-group">';
                                    html1 +='<label style="font-size: small">Rate: </label> <input type="text" onblur="javascript:getTV()" id="txtRate" maxlength="10" class="numeric text-right form-control dealRate" value="'+ value1.Rate +'"  onchange="calamt('+ val2 + ')" onkeypress="return isNumberKey(event)"> </div> </div>';



                                    html1 +='<div class="col-md-2 paddingleft0"> <div class="form-group">';
                                    html1 +='<div class="col-md-8 paddingleft0"> <label style="font-size: small">Amount (Rs): &nbsp;&nbsp; &nbsp;</label>';
                                    html1 +='<input type="text" value="'+ value1.DealAmt +'" onblur="javascript:getTV()" id="txtamt" maxlength="10" class="numeric text-right form-control dealamount" onchange="calcommamt();" onkeypress="return isNumberKey(event)">';
                                    html1 +='<input type="hidden" value="' + value1.Id + '" class="hiddenid"/>';
                                    html1 +='</div></div>';   
                                    html1 +=' <div class="col-md-4 paddingleft0" style="margin-top: 25px !important;"> <img id="imgaddphone" style="height: 15px;width: 15px;" src="img/add1.png" alt="Add" onclick="CreateDealState();">';
                                                    
                                    html1 +=' </div></div> </div> </div>';

                                    //html1 +='  <div class="col-md-4 paddingleft0"> <div class="col-md-12 paddingleft0"><div class="form-group">';
                                    //html1 +=' <div class="col-md-6 paddingleft0">  <label style="font-size: small">Date: &nbsp;&nbsp; &nbsp;</label>';
                                    //html1 +='  <input type="text" style="width: 100%" id="dd" class="datepicker form-control paydate" readonly="readonly" value="' + value1.paydate + '"> </div>';
                                    //html1 +=' <div class="col-md-6 paddingleft0" style="margin-top: 25px !important;"> <img id="imgaddphone" style="height: 15px;width: 15px;" src="img/add1.png" alt="Add" onclick="CreateDealState();">';
                                                    
                                    //html1 +=' </div></div> </div> </div>';
                                 
                                    val2++;

                                }
                                else
                                {
                                    html1 += '<div class="clearfix"></div><div class="dt" id="divstage' + val2 + '" >';
                                    html1 +='<div class="col-md-2 paddingleft0"><div class="form-group">';
                                    html1 +='<label style="font-size: small">Mile Stone: </label>';
                                    html1 +=' <input type="text" value="'+ value1.DealText +'" style="width: 100%" id="txtdealstate1" maxlength="150" class="form-control dealtext" /></div></div>';
                                   
                                    html1 +=' <div class="col-md-1 paddingleft0"> <div class="form-group"><label style="font-size: small">Qty: </label>';
                                    html1 +='<input type="text" onblur="javascript:getTV()" id="txtQty' + val2 + '"  maxlength="10" class="numeric text-right form-control dealQty" value="'+ value1.Qty +'" onchange="calamt('+ val2 + ')" onkeypress="return isNumberKey(event)">';
                                    html1 +='</div></div><div class="col-md-1 paddingleft0"><div class="form-group">';
                                    html1 +='<label style="font-size: small">Rate: </label> <input type="text" onblur="javascript:getTV()" id="txtRate' + val2 + '"  maxlength="10" class="numeric text-right form-control dealRate" value="'+ value1.Rate +'"  onchange="calamt('+ val2 + ')" onkeypress="return isNumberKey(event)"> </div> </div>';

                                    
                                    html1 +='<div class="col-md-2 paddingleft0"><div class="form-group">';
                                    html1 +='<div class="col-md-8 paddingleft0"> <label style="font-size: small">Amount (Rs): &nbsp;&nbsp; &nbsp;</label>';
                                    html1 +='<input type="text" value="'+ value1.DealAmt +'" onblur="javascript:getTV()" id="txtamt1" maxlength="10" class="numeric text-right form-control dealamount" onchange="calcommamt();" onkeypress="return isNumberKey(event)">';
                                    html1 +='<input type="hidden" value="' + value1.Id + '" class="hiddenid"/>';
                                    html1 +='</div></div>';    
                                    html1 +=' <div class="col-md-4 paddingleft0" style="margin-top: 25px !important;"> <img id="imgaddphone" style="height: 15px;width: 15px;" src="img/add1.png" alt="Add" onclick="CreateDealState();">';
                                    html1 +=' <img src="img/substract.png" style="height: 15px;width: 15px;margin-left: 10px;" alt="Remove" onclick="RemoveDealState(' + val2 + ',' + value1.Id + ');" />'               
                                    html1 +=' </div></div> </div> </div>';

                                    //html1 +='  <div class="col-md-4 paddingleft0"> <div class="col-md-12 paddingleft0"><div class="form-group">';
                                    //html1 +=' <div class="col-md-6 paddingleft0">  <label style="font-size: small">Date: &nbsp;&nbsp; &nbsp;</label>';
                                    //html1 +=' <input type="text" style="width: 100%" id="dd1" class="datepicker form-control paydate" readonly="readonly" value="' + value1.paydate + '"> </div>';
                                    //html1 +=' <div class="col-md-6 paddingleft0" style="margin-top: 25px !important;"> <img id="imgaddphone" style="height: 15px;width: 15px;" src="img/add1.png" alt="Add" onclick="CreateDealState();">';
                                    //html1 +=' <img src="img/substract.png" style="height: 15px;width: 15px;margin-left: 10px;" alt="Remove" onclick="RemoveDealState(' + val2 + ',' + value1.Id + ');" />'               
                                    //html1 +=' </div></div> </div> </div>';

                             

                                    val2++;
                                }
                               
                               
                                    k++;
                            
                            }); 
                            // $("#dealstage").html('');
                            html1 +="<script type='text/javascript'>  $(function () {var date = new Date(); date.setDate(date.getDate()); $('.datepicker').datepicker({ startDate: date, format: 'dd/M/yyyy', autoclose: true });});";
                            $(".dealstage").html(html1);
                          //  $("#divstage" + $('.dt').length).after(html);
                        }
                    });
                }
            });
                }

                function removefile(id,dealID,FilePath)
                {
                 
                    debugger;
                        $.ajax({
                            type: "POST",
                            url: '<%= ResolveUrl("CRMDBService.asmx/DeleteFile") %>',
                            contentType: "application/json; charset=utf-8",
                            data: '{TaskDealId:"' + dealID + '",FilePath:"' + FilePath.replace(/\\/g, "\\\\") + '"}',
                            dataType: "json",
                            success: function (data) {
                               
                                $(id).remove();
                            }
                        });
                  
                }
                function clrdeals(i) {
                    //debugger;
                    //$("#txtdealname").val('');
                    $("#txtdealstate").val('');
                    $("#txtamt").val('0');
                    $("#txtComm").val('0');
                    $("#txtcommamt").val('0');
           $("#txtdealdate").val('');
           $("#txtexpclsdate").val('');
           $("#selstage").val('Pending 10%');
           $("#txtdealnote").val('');
           $("#txtmonthdeal").val('');
           $('#dealtv').html(0.0);
           $("#aloss").removeClass('showbg');
           $("#awon").removeClass('showbg');
           $("#selstage").removeClass('showbg');
           $("#hidTdealId").val('');
           $("#txtmonthdeal").val('0');
           $("#txtQty").val('0');
           $("#txtRate").val('0');
           $("#spandeal").hide(); $("#dealhead").html('Multi-month deal?');
           //if (i==0)
           //{
               var html='<div class="dt" id="divstage1"> <div class="col-md-2 paddingleft0">';
               html +='<div class="form-group"><label style="font-size: small">Mile Stone: </label><input type="text" style="width: 100%" id="txtdealstate" maxlength="150" class="form-control dealtext" />';
               html +='</div></div>';
               html +=' <div class="col-md-1 paddingleft0"> <div class="form-group"><label style="font-size: small">Qty: </label>';
               html +='<input type="text" onblur="javascript:getTV()" id="txtQty" maxlength="10" class="numeric text-right form-control dealQty" value="0" onchange="calamt()" onkeypress="return isNumberKey(event)">';
               html +='</div></div><div class="col-md-1 paddingleft0"><div class="form-group">';
               html +='<label style="font-size: small">Rate: </label> <input type="text" onblur="javascript:getTV()" id="txtRate" maxlength="10" class="numeric text-right form-control dealRate" value="0"  onchange="calamt()" onkeypress="return isNumberKey(event)"> </div> </div>';


               html +='<div class="col-md-2 paddingleft0"><div class="col-md-12 paddingleft0"><div class="form-group">';
               html +='<div class="col-md-8 paddingleft0"><label style="font-size: small">Amount (Rs): &nbsp;&nbsp; &nbsp;</label>';
               html +='<input type="text" onblur="javascript:getTV()" id="txtamt" maxlength="10" onchange="calcommamt();" class="numeric text-right form-control dealamount">';
               html +='<input type="hidden" value="" class="hiddenid"/></div> </div>';
               html +=' <div class="col-md-4 paddingleft0" style="margin-top: 25px !important;"> <img id="imgaddphone" style="height: 15px;width: 15px;" src="img/add1.png" alt="Add" onclick="CreateDealState();">';                                   
               html +=' </div></div> </div> </div>';

               //html +='  <div class="col-md-4 paddingleft0"> <div class="col-md-12 paddingleft0"><div class="form-group">';
               //html +=' <div class="col-md-6 paddingleft0">  <label style="font-size: small">Date: &nbsp;&nbsp; &nbsp;</label>';
               //html +='  <input type="text" style="width: 100%" id="dd" class="datepicker form-control paydate" readonly="readonly"> </div>';
               //html +=' <div class="col-md-6 paddingleft0" style="margin-top: 25px !important;"> <img id="imgaddphone" style="height: 15px;width: 15px;" src="img/add1.png" alt="Add" onclick="CreateDealState();">';                                   
               //html +=' </div></div> </div> </div>';
          
               //html +="<script type='text/javascript'>  $(function () {var date = new Date(); date.setDate(date.getDate()); $('.datepicker').datepicker({ startDate: date, format: 'dd/M/yyyy', autoclose: true });});";           
               $('.dealstage').html(html);
           //}
           var link = $('.MultiFile-remove');
           if (link.length > 0) {
               //   var formData = new FormData();
               for (var i = 0; i < link.length; i++) {
                   link[i].click();
               }
           }
           $("#ContentPlaceHolder1_uploader_wrap_list").html('');
       }

       function deleteDeal(TaskDealId) {
           if (confirm('Do you want to delete ?')) {
               $.ajax({
                   type: "POST",
                   url: '<%= ResolveUrl("CRMDBService.asmx/DeleteDeal") %>',
                   contentType: "application/json; charset=utf-8",
                   data: '{TaskDealId:"' + TaskDealId + '"}',
                   dataType: "json",
                   success: function (data) {
                       Successmessage("Deal Deleted");
                       if ($('#listid').val() == "Deals")
                       {
                           GetTaskDeals(0);
                       }
                       else
                       {
                           GetCompleteHis(0);
                       }
                       $("#hidTdealId").val('');
                       clrdeals(0);
                   }
               });
           }
       }
       function SaveBg() {
           if ($('#txtbg').val() == "") {
               errormessage("Please enter a background");
               return false;
           }
           $.ajax({
               type: "POST",
               url: '<%= ResolveUrl("~/CRMDBService.asmx/SaveBG") %>',
                contentType: "application/json; charset=utf-8",
                data: '{Tdocid: "' + $("#hidref_docId").val() + '",bg: "' + $("#txtbg").val() + '",ContactId:"' + $("#hidcontactId").val() + '"}',
                dataType: "json",
                success: function (data) {

                    Successmessage("Background Updated");
                    $("#edit_background").hide();
                    $("label[for='lblbg']").html($('#txtbg').val());

                }
            });
        }
        function Getcallnumbers() {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetCallNumbers") %>',
                contentType: "application/json; charset=utf-8",
                data: '{ContactId:"' + $("#hidcontactId").val() + '"}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    $.each(jsdata1, function (key1, value1) {
                        $('#myselectcall').append($('<option>').text(value1.contactnumber).attr('value', value1.contact_Id));
                    })
                }
            });
        }
             function GetEmailcontactname() {
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("CRMDBService.asmx/GetEmailContactNo") %>',
                        contentType: "application/json; charset=utf-8",
                        data: '{ContactId:"' + $("#hidcontactId").val() + '"}',
                        dataType: "json",
                        success: function (data) {
                            jsdata1 = JSON.parse(data.d);
                            var output = "";
                            var Previous = "";
                            $.each(jsdata1, function (key1, value1) {
                                $('#myselectemailall').append($('<option>').text(value1.con).attr('value', value1.cmbl_id));
                              //  output +="<div><label style='font-size: small' class='col-md-6'>"+value1.con+"</label></div><div><a href='mailto:'>"+value1.contactemail+"</a></div><div class='clearfix'></div>";
                               
                            })

                           // $('#ContentPlaceHolder1_myselectemail').html(output);
                        }
                    });
             }
                function fillemail() { 
                    var x = document.getElementById("myselectemailall").value;
                   
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("CRMDBService.asmx/GetEmail") %>',
                        contentType: "application/json; charset=utf-8",
                        data: '{taskid: "' +x + '"}',
                        dataType: "json",
                        success: function (data) {
                            jsdata1 = JSON.parse(data.d);
                            var output = "";
                            var Previous = "";
                            $.each(jsdata1, function (key1, value1) {
                                //$('#myselectemailall').append($('<option>').text(value1.con).attr('value', value1.CMbl_Id));
                                output +="<div><a href='mailto:'>"+value1.contactemail+"</a></div><div class='clearfix'></div>";
                               
                            })

                             $('#ContentPlaceHolder1_myselectemail').html(output);
                        }
                    });
                }
                function SaveCall() {
                  //  debugger;
                    if ($("#hidref_docId").val() =="")
                    {
                        errormessage("Please Add Action first");
                        clrcalls();
                        return false;
                    }
                    if ($('#txtcallnote').val() == "" || $('#txtcallnote').val() == " ") {
                        errormessage("Please enter a Call Note");
                        return false;
                    }
                    
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("~/CRMDBService.asmx/SaveCall") %>',
                contentType: "application/json; charset=utf-8",
                data: '{TaskDocID: "' + $("#hidref_docId").val() + '",TCallID: "' + $("#hidTcallId").val() + '",Phone: "' + $("#myselectemailall option:selected").text() + '",Result: "0",CallNotes: "' + $("#txtcallnote").val() + '"}',
                dataType: "json",
                success: function (data) {

                    Successmessage("Call Saved");
                    $("#hidTcallId").val('')
                    clrcalls();
                    var username = '<%=Session["user_name"]%>';
                    // if (username == "sa") {

                    //}
                    //else {
                      
                        SetPermissions("Add Call", "save_add_call1");
                       
                  //  }
                    $('#taskdetailstetsing1').html('');
                    $('#taskdetailstetsing4').html('');
                    $('#taskdetailstetsing2').html('');
                    $('#taskdetailstetsing3').html('');
                    GetCompleteHis(0);
                    //GetTaskCall(0);

                }
            });
                }
                function SaveEmail() {
                    //  debugger;
                    if ($("#hidref_docId").val() =="")
                    {
                        errormessage("Please Add Action first");
                        clrcalls();
                        return false;
                    }
                    if ($('#txtaddemailnote').val() == "" || $('#txtaddemailnote').val() == " ") {
                        errormessage("Please enter a Email Note");
                        return false;
                    }
                    
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("~/CRMDBService.asmx/SaveEmail") %>',
                        contentType: "application/json; charset=utf-8",
                        data: '{TaskDocID: "' + $("#hidref_docId").val() + '",TCallID: "' + $("#hidTcallId").val() + '",Phone: "' + $("#myselectemailall option:selected").text() + '",Result: "0",CallNotes: "' + $("#txtaddemailnote").val() + '"}',
                        dataType: "json",
                        success: function (data) {

                            Successmessage("Call Saved");
                            $("#hidTcallId").val('')
                            clrcalls();
                            var username = '<%=Session["user_name"]%>';
                            // if (username == "sa") {

                            //}
                            //else {
                      
                            SetPermissions("Add Call", "save_add_call1");
                       
                            //  }
                            $('#taskdetailstetsing1').html('');
                            $('#taskdetailstetsing4').html('');
                            $('#taskdetailstetsing2').html('');
                            $('#taskdetailstetsing3').html('');
                            GetCompleteHis(0);
                            //GetTaskCall(0);

                        }
                    });
                }
                function isNumberKey(evt) {
                    var charCode = (evt.which) ? evt.which : event.keyCode
                    if (charCode > 31 && (charCode < 48 || charCode > 57))
                        return false;
                    return true;
                }
                function calamt(id)
                {
                    var amt =$('#txtQty' + id).val() * $('#txtRate' + id).val() ;
                    $('#txtamt' + id).val(amt);
                    calcommamt();

                }
                function CreateDealState() {
                    var val2 = $('.dt').length + 1;

                    var html;
     
                    html = '<div class="clearfix"></div><div class="dt" id="divstage' + val2 + '" >';


                    html +='<div class="col-md-2 paddingleft0"><div class="form-group">';
                    html +='<label style="font-size: small">Mile Stone: </label>';
                    html +=' <input type="text" style="width: 100%" id="txtdealstate1" maxlength="150" class="form-control dealtext" /></div></div>';

                    html +=' <div class="col-md-1 paddingleft0"> <div class="form-group"><label style="font-size: small">Qty: </label>';
                    html +='<input type="text" onblur="javascript:getTV()" id="txtQty'+ val2 + '" maxlength="10" class="numeric text-right form-control dealQty" value="0" onchange="calamt('+ val2 + ')" onkeypress="return isNumberKey(event)">';
                    html +='</div></div><div class="col-md-1 paddingleft0"><div class="form-group">';
                    html +='<label style="font-size: small">Rate: </label> <input type="text" onblur="javascript:getTV()" id="txtRate' + val2 +'" maxlength="10" class="numeric text-right form-control dealRate" value="0"  onchange="calamt('+ val2 + ')" onkeypress="return isNumberKey(event)"> </div> </div>';


                    html +='<div class="col-md-2 paddingleft0"><div class="form-group">';
                    html +=' <div class="col-md-8 paddingleft0"><label style="font-size: small">Amount (Rs): &nbsp;&nbsp; &nbsp;</label>';

                    html +='<input type="text" onblur="javascript:getTV()" id="txtamt' + val2 + '" maxlength="10" onchange="calcommamt();" class="numeric text-right form-control dealamount" onkeypress="return isNumberKey(event)" value="0">';
                    html +='<input type="hidden" value="" class="hiddenid"/>';
                    html +='</div></div>';
                    html +=' <div class="col-md-4 paddingleft0" style="margin-top: 25px !important;"> <img id="imgaddphone" style="height: 15px;width: 15px;" src="img/add1.png" alt="Add" onclick="CreateDealState();">';
                    html +='<img src="img/substract.png" style="height: 15px;width: 15px;margin-left:10px" alt="Remove" onclick="RemoveDealState(' + val2 + ',0);" />';                         
                    html +=' </div></div> </div> ';

                    //html +='  <div class="col-md-4 paddingleft0"> <div class="col-md-12 paddingleft0"><div class="form-group">';
                    //html +=' <div class="col-md-6 paddingleft0">  <label style="font-size: small">Date: &nbsp;&nbsp; &nbsp;</label>';
                    //html +='  <input type="text" style="width: 100%" id="dd1" class="datepicker form-control paydate" readonly="readonly"> </div>';
                    //html +=' <div class="col-md-6 paddingleft0" style="margin-top: 25px !important;"> <img id="imgaddphone" style="height: 15px;width: 15px;" src="img/add1.png" alt="Add" onclick="CreateDealState();">';
                    //html +='<img src="img/substract.png" style="height: 15px;width: 15px;margin-left:10px" alt="Remove" onclick="RemoveDealState(' + val2 + ',0);" />';                         
                    //html +=' </div></div> </div> </div>';

                                       


                    html +="<script type='text/javascript'>  $(function () {var date = new Date(); date.setDate(date.getDate()); $('.datepicker').datepicker({ startDate: date, format: 'dd/M/yyyy', autoclose: true });});";           

                    $("#divstage" + $('.dt').length).after(html);
                   
                }


                function RemoveDealState(id,stageid) {
                    debugger;
                    if (stageid =="0")
                    {

                    }
                    else
                    {
                        if (confirm('Do you want to delete ?')) {
                            $.ajax({
                                type: "POST",
                                url: '<%= ResolveUrl("CRMDBService.asmx/removedealstage") %>',
                                contentType: "application/json; charset=utf-8",
                                data: '{dealstageid:"' + stageid + '"}',
                                dataType: "json",
                                success: function (data) {
                                    Successmessage("Deal Stage Deleted");
                                   
                                }
                            });
                        }
                    }
                    $("#divstage" + id).remove();
                    calcommamt();
                }
        function GetContacts()
        {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetContacts") %>',
                contentType: "application/json; charset=utf-8",
                data: '{contactId : ' + $("#hidcontactId").val() + '}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    var output = "   <table class='table'><tr><th> Name </th><th>  Phone No.</th></tr>";
                    $.each(jsdata1, function (key1, value1) {
                           output += " <tr><td class='tdcls' style='padding:3px !important;'>" + value1.ContactName + "</td><td style='padding:3px !important;'>" + value1.Phone + "</td></tr>";

                    });
                    output += "</table>";
                    $('#divtable').html(output);
                }
            });


        }



                function getContactUrls()
                {
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("CRMDBService.asmx/GetContactsUrls") %>',
                        contentType: "application/json; charset=utf-8",
                        data: '{contactId : ' + $("#hidcontactId").val() + '}',
                        dataType: "json",
                        success: function (data) {
                            jsdata1 = JSON.parse(data.d);
                            var output = "   <table class='table'><tr><th> Websites  </th></tr>";
                            $.each(jsdata1, function (key1, value1) {
                                output += " <tr><td style='padding:3px !important;'>" + value1.Url + "</td></tr>";
                            });
                            output += "</table>";
                            $('#divurls').html(output);
                        }
                    });

                    
                }
                function GetContactsMail()
                {
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("CRMDBService.asmx/GetContactsMail") %>',
                        contentType: "application/json; charset=utf-8",
                        data: '{contactId : ' + $("#hidcontactId").val() + '}',
                        dataType: "json",
                        success: function (data) {
                            jsdata1 = JSON.parse(data.d);

                            
                            var output = "   <table class='table'><tr><th> Email  </th></tr>";
                            $.each(jsdata1, function (key1, value1) {
                                output += " <tr><td style='padding:3px !important;'>" + value1.email + "</td></tr>";

                            });
                            output += "</table>";
                            $('#divtable1').html(output);

                        }
                    });


                }
   
        function GetTaskCall(TaskCallId) {

            $('#taskdetailstetsing1').html('');
            $('#taskdetailstetsing4').html('');
            $('#taskdetailstetsing2').html('');
            $('#taskdetailstetsing3').html('');
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetTaskCalls") %>',
                contentType: "application/json; charset=utf-8",
                data: '{TaskDocId: "' + $("#hidref_docId").val() + '",TaskCallId:"' + TaskCallId + '"}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);

                    var output = "<ul class='timeline'  style='margin:0px;'>";
                    var Previous = "";
                    $.each(jsdata1, function (key1, value1) {
                        //alert(Previous)
                        if (value1.DisplayDate != Previous) {
                            output += "<li class='time-label'><span class='bg-red'>" + value1.DisplayDate + "</span></li>";
                        }
                        output += " <li style='margin:0px;'><span class='bg-green' style='padding: 3px 13px;margin-left: 5px;border-radius: 0px 45px 45px 0px;'>Calls</span><div class='timeline-item'>  <div class='timeline-body'><span style='color:#0073b7'><b>Phone - </b></span>" + value1.Phone + '<div class="clearfix"></div><span style="color:#0073b7"><b>Result - </b></span>' + value1.Result + '<div class="clearfix"></div><span style="color:#0073b7"><b>Call Notes - </b></span>' + value1.CallNotes + "</div><div class='timeline-footer'><a class='btn btn-primary btn-xs' onclick=deleteCall('" + value1.Task_CallID + "');>Delete</a><a class='btn btn-danger btn-xs' style='margin-left: 5px;' onclick=editCall('" + value1.Task_CallID + "');>Edit</a></div></li>";
                        Previous = value1.DisplayDate;
                    });
                    output += "</ul>";
                    $('#taskdetailstetsing4').html(output);
                }
            });
        }

                //To check
        function editCall(TaskCallId) {
            activaTab('add_call_show');
            $('#save_add_call1').show();
            $("#hidTcallId").val(TaskCallId);
            debugger;
            cleartask();
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("CRMDBService.asmx/GetTaskCalls") %>',
                contentType: "application/json; charset=utf-8",
                data: '{TaskDocId: "' + $("#hidref_docId").val() + '",TaskCallId:"' + TaskCallId + '"}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    $.each(jsdata1, function (key1, value1) {
                        if(value1.mailedpersonid!='')
                        {
                            var output = "";
                            // alert(value1.mailedpersonemail)
                            document.getElementById("rdemail").checked == true 
                            document.getElementById("divemail").style.display = "block";
                            document.getElementById("divcallemail").style.display = "none";
                            $("#txtaddemailnote").val(value1.CallNotes);
                            output +="<div><a href='mailto:'>"+value1.mailedpersonemail+"</a></div><div class='clearfix'></div>";
                            $('#ContentPlaceHolder1_myselectemail').html(output);
                            $('#myselectemailall').attr('value', value1.mailedpersonid)
                            document.getElementById("myselectemailall").value=value1.mailedpersonid;
                        }
                        else
                        {
                            document.getElementById("rdcall").checked == true 
                            document.getElementById("divemail").style.display = "none";
                            document.getElementById("divcallemail").style.display = "block";
                            $("#txtcallnote").val(value1.CallNotes);
                       
                            $('#myselectcall').attr('value', value1.Phone)
                        }
                      
                     
                       // $("#myselectcallresult").val(value1.Result);
                       
                    });
                }
            });
        }
        function deleteCall(TaskCallId) {
            if (confirm('Do you want to delete ?'))
            {
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("CRMDBService.asmx/DeleteCall") %>',
                    contentType: "application/json; charset=utf-8",
                    data: '{TaskCallId:"' + TaskCallId + '"}',
                    dataType: "json",
                    success: function (data) {
                        Successmessage("Call Deleted");
                      
                        GetCompleteHis(0);
                      
                        $("#hidTcallId").val('');
                        clrCall();

                    }
                });
            }
        
        }
        function clrcalls() {
            $("#txtcallnote").val('');
           // $("#myselectcallresult").val('Not interested');
            $("#hidTcallId").val('');
        }
            </script>
        <script type="text/javascript">

            function savedealstage(ID,val,amt,i,stageid,qty,rate)
            {
                debugger;
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("~/CRMDBService.asmx/Savedealstage") %>',
                    contentType: "application/json; charset=utf-8",    
                    processData: false,                           
                    data: '{Task_DealID: "' + ID + '", dealtext: "' + val + '",dealamt:"' + amt + '",stageid:"' + stageid + '",qty:"' + qty + '",rate:"' + rate + '",totalamt:"'+$('#txttotaldealamt').val()+'"}',
                    dataType: "json",                           
                    success: function (data) {
                        debugger;
                        for (var j = i; j >1; j--) {
                            RemoveDealState(j,0);
                        }             
                        $('#txtdealstate').val('');
                        $('#txtamt').val('');
                        $('#dd').val('');
                    }
                });

            }
        </script>
  <script type="text/javascript">
     
      function SetPermissions(activity, id) {
debugger;
          var username = '<%=Session["user_name"]%>';
          var Allow = "aa";
          $.ajax({
              type: "POST",
              url: '<%= ResolveUrl("CRMDBService.asmx/SetPagePermissionActivity") %>',
              contentType: "application/json; charset=utf-8",
              data: '{Activity: "' + activity + '",User:"' + username + '"}',
              async: false,
              dataType: "json",
              success: function (data) {
                  jsdata1 = JSON.parse(data.d);
                  //debugger;
                  $.each(jsdata1, function (key1, value1) {

                      if (value1.Allow == "false") {
                       
                          $("#" + id).hide();
                      }
                      else {                         
                         
                          $("#" + id).show();

                      }
                      Allow = value1.Allow;

                  });

              }

          });


          return Allow;
      }

      function viewdetail(id)
      {
          $.ajax({
              type: "POST",
              url: '<%= ResolveUrl("~/CRMDBService.asmx/getdealstage") %>',
              contentType: "application/json; charset=utf-8",    
              processData: false,                           
              data: '{Task_DealID: "' + id + '"}',
              dataType: "json",                           
              success: function (data) {
                  jsdata1 = JSON.parse(data.d);
                            
                  var output = "<table class='table table-striped'><tr><th> Mile Stone </th><th>  Amount</th><th> Qty</th><th> Rate</th></tr>"
                  $.each(jsdata1, function (key1, value1) {
                      debugger;
                      output += " <tr><td class='tdcls'>" + value1.DealText + "</td><td >" + value1.DealAmt + "</td><td>" + value1.Qty + "</td><td>" + value1.Rate + "</td></tr>";

                  }); 
                  output +="</table>";
                  output +="<input type='hidden' id ='hiddealid' value='" + id + "'/>"
                  $("#dealstage").html(output);
                  $("#modal-default").modal('show');
               
              }
          });
      
     
      }
      function getattachments()
      {
          $.ajax({
              type: "POST",
              url: '<%= ResolveUrl("~/CRMDBService.asmx/getFilePath") %>',
              contentType: "application/json; charset=utf-8",    
              processData: false,                           
              data: '{Task_DealID: "' + $("#hiddealid").val() + '"}',
              dataType: "json",                           
              success: function (data) {
                  jsdata1 = JSON.parse(data.d);

                  var output = "<table class='table table-striped'><tr><th>Attachments </th></tr>"
                  $.each(jsdata1, function (key1, value1) {
                      debugger;
                              
                      output += " <tr><td class='tdcls'><a class='MultiFile-title' href="+ value1.filepath + " target='_blank' >"+ value1.filename +"</a></td></tr>";        
                   
                  });                
                  output +="</table>" ;   
                  $("#Attachments").html(output);
                  $("#modal-attachments").modal('show');
              }
          });
      }
       function GetCompleteHis(X) {


           var dealdetail= SetPermissions("Deal Detail","");
           var Editallow = SetPermissions("Edit", "");
           var delallow = SetPermissions("Delete", "");
           var EditP = "";
           var DelP = "";
           var viewP="";
           if (dealdetail == "true")
           {
               viewP="";
           }
      else{
               viewP="style="+"\"visibility: hidden;\"";
      }
           if (Editallow == "true") {
                         
             
                       EditP="";
                    }
                    else
                    {
             
                      EditP="visibility: hidden;";
                    }
           if (delallow == "true") {
                          DelP="";
                      
                      
                    }
                    else {
                       
         
                          DelP="style="+"\"visibility: hidden;\"";
                    }
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("CRMDBService.asmx/GetCompleteHistory") %>',
                        contentType: "application/json; charset=utf-8",
                        data: '{Ref_DocId:"' + $("#hidref_docId").val() + '"}',
                        dataType: "json",
                        success: function (data) {
                            jsdata1 = JSON.parse(data.d);
                            var output = "<ul class='timeline'  style='margin:0px;'>";
                            var Previous = "";
                            $.each(jsdata1, function (key1, value1) {

                                if (value1.DisplayDate != Previous) {
                                    output += "<li class='time-label'><span class='bg-red'>" + value1.DisplayDate + "</span></li>";
                                }
                                if (value1.Type == "N") {
                              
                                    output += " <li style='margin:0px;'><span class='bg-blue' style='padding: 3px 13px;margin-left: 5px;border-radius: 0px 45px 45px 0px;'>Notes</span> <div class='timeline-item'>  <div class='timeline-body'>" + value1.Notes + "<div class='clearfix'></div><span style='color:#0073b7'><b>By - </b></span>" + value1.username + "  &nbsp;&nbsp;" + value1.DisplayDate + "</div><div class='timeline-footer'><a class='btn btn-primary btn-xs' onclick=deleteNote('" + value1.Task_NoteID + "'); " + DelP + ">Delete</a><a class='btn btn-danger btn-xs' style='margin-left:5px; " + EditP + "' onclick=editNote('" + value1.Task_NoteID + "');>Edit</a></div></li>";
                                 
                                }
                                else if (value1.Type == "C") {
                               
                                    output += " <li style='margin:0px;'><span class='bg-green' style='padding: 3px 13px;margin-left: 5px;border-radius: 0px 45px 45px 0px;'>Calls</span><div class='timeline-item'>  <div class='timeline-body'><span style='color:#0073b7'><b>Phone - </b></span>" + value1.Phone + '<div class="clearfix"></div><span style="color:#0073b7"><b>Result - </b></span>' + value1.Result + '<div class="clearfix"></div><span style="color:#0073b7"><b>Call Notes - </b></span>' + value1.CallNotes + "<div class='clearfix'></div><span style='color:#0073b7'><b>By - </b></span>" + value1.username + "  &nbsp;&nbsp; " + value1.DisplayDate + "</div><div class='timeline-footer'><a class='btn btn-primary btn-xs' onclick=deleteCall('" + value1.Task_CallID + "'); " + DelP + " >Delete</a><a class='btn btn-danger btn-xs' style='margin-left: 5px;" + EditP + "' onclick=editCall('" + value1.Task_CallID + "'); >Edit</a></div></li>";
                                  
                                }
                                else if (value1.Type == "E") {
                                
                                    output += " <li style='margin:0px;'><span class='bg-green' style='padding: 3px 13px;margin-left: 5px;border-radius: 0px 45px 45px 0px;'>Emails</span><div class='timeline-item'>  <div class='timeline-body'><span style='color:#0073b7'><b>Email To- </b></span>" + value1.Phone + '<div class="clearfix"></div><div class="clearfix"></div><span style="color:#0073b7"><b>Email Notes - </b></span>' + value1.CallNotes + "<div class='clearfix'></div><span style='color:#0073b7'><b>By - </b></span>" + value1.username + "  &nbsp;&nbsp; " + value1.DisplayDate + "</div><div class='timeline-footer'><a class='btn btn-primary btn-xs' onclick=deleteCall('" + value1.Task_CallID + "'); " + DelP + " >Delete</a><a class='btn btn-danger btn-xs' style='margin-left: 5px;" + EditP + "' onclick=editCall('" + value1.Task_CallID + "'); >Edit</a></div></li>";
                                  
                                }
                                else if (value1.Type == "D") {
                                    output += " <li style='margin:0px;'><span class='bg-red' style='padding: 3px 13px;margin-left: 5px;border-radius: 0px 45px 45px 0px;'>Deals</span><div class='timeline-item'>  <div class='timeline-body'><div class='clearfix'></div> <span style='color:#0073b7'> <b>Deal date- </b></span>" + value1.DealDate + "<div class='clearfix'></div> <span style='color:#0073b7'> <b>Exp date- </b></span>" + value1.ExpClsDt + "<div class='clearfix'></div><span style='color:#0073b7'><b>Deal Note- </b></span>" + value1.DealNote + "<a class='viewanchor' title='View Detail' onclick=viewdetail('" + value1.Task_DealID + "'); " + viewP + "><i class='glyphicon glyphicon-info-sign viewdetail'></i></a><div class='clearfix'></div> <div class='timeline-footer'><a class='btn btn-primary btn-xs' onclick=deleteDeal('" + value1.Task_DealID + "'); " + DelP + ">Delete</a><a class='btn btn-danger btn-xs' style='margin-left: 5px; " + EditP + "' onclick=editDeal('" + value1.Task_DealID + "'); >Edit</a></div></li>";

                                  
                                }
                                else {
                                
                                    output += " <li style='margin:0px;'><span class='bg-yellow' style='padding: 3px 13px;margin-left: 5px;border-radius: 0px 45px 45px 0px;'>Action</span><div class='timeline-item'>  <div class='timeline-body'><span style='color:#0073b7'><b>Task - </b></span>" + value1.Task + " <div class='clearfix'></div><span style='color:#0073b7'><b>By - </b></span>" + value1.Smname.replace(".", "Admin") + "<span style='color:#0073b7'> <b>, To - </b></span>" + value1.AssignedTo + "  &nbsp;&nbsp; " + value1.Adate + "<div class='clearfix'></div> </div><div class='timeline-footer'><a class='btn btn-primary btn-xs' onclick=deleteTask('" + value1.DocId + "'); " + DelP + ">Delete</a><a class='btn btn-danger btn-xs' style='margin-left:5px; " + EditP + "' onclick=editTask('" + value1.DocId + "');>Edit</a></div></li>";
                                  
                                 
                                }

                                Previous = value1.DisplayDate;
                            });
                            output += "</ul>";


                            $('#taskdetailstetsing2').html(output);


                        }
                    });

                }

        </script>
        
                    
            
    </section>
</asp:Content>
