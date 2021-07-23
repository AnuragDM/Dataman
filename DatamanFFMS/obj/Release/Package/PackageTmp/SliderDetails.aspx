<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="SliderDetails.aspx.cs" Inherits="AstralFFMS.SliderDetails" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <script src="plugins/datatables/jquery.dataTables.min.js"></script>
   
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style>
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
    </style>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>
    <style type="text/css">
        .clstd
        {
            display:none;
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
        #divImage
{
    /*display: none;
    z-index: 1000;
    position: fixed;
    align-content:center;
    top: 0;
    left: 0;
    background-color: White;
    height: 300px;
    width: 300px;
    padding: 3px;
    border: solid 1px black;*/
        display: none;
    z-index: 1000;
    position: fixed;
    /* align-content: center; */
    top: 16% !important;
    left: 38% !important;
    /*background-color: White;*/
    height: 300px;
    width: 300px;
    /*padding: 3px;*/
    /*border: solid 1px black;*/
       opacity: 1;
}

        .multiselect-container.dropdown-menu {
    width: 550px !important;
}
       /*.box-footer {
    border-top-left-radius: 0;
    border-top-right-radius: 0;
    border-bottom-right-radius: 3px;
    border-bottom-left-radius: 3px;
    border-top: 1px solid #f4f4f4;
    padding: 4px;
    margin-left: 36px;
    background-color: #ffffff;
}*/
   .cur
    {
        cursor: pointer;
    }
    </style>
    <script type="text/javascript">
        $(document).ready(function ()
        {
          
        }

        );

       
    </script>
   


     <script type="text/javascript">
         $(function () {
             $('[id*=ListBox1]').multiselect({
                 enableCaseInsensitiveFiltering: true,
                 buttonWidth: '100%',
                 includeSelectAllOption: true,
                 maxHeight: 200,
                 //width: 606,
                 minWidth:700,
                 enableFiltering: true,
                 filterPlaceholder: 'Search'
             });
         });
    </script>
       <script type="text/javascript">
           function BindState() {
          
               var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'

               var obj = { StateId: 0 };
               $.ajax({
                   type: "POST",
                   url: pageUrl + '/PopulateState',
                   data: JSON.stringify(obj),
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   success: OnPopulated,
                   failure: function (response) {
                       alert(response.d);
                   }
               });
               function OnPopulated(response) {
                   PopulateControl(response.d, $("#<%=lstState.ClientID %>"));
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
           function BindCity() {
               var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'

               var selectedState = [];

               $("#<%=lstState.ClientID %> :selected").each(function () {
                   selectedState.push($(this).val());
               });
               $("#<%=HiddenState.ClientID %>").val(selectedState);

               var obj = { StateID: $("#<%=HiddenState.ClientID %>").val() };
               $.ajax({
                   type: "POST",
                   url: pageUrl + '/PopulateCityByMultiState',
                   data: JSON.stringify(obj),
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   success: OnPopulated,
                   failure: function (response) {
                       alert(response.d);
                   }
               });
               function OnPopulated(response) {
                   PopulateControl(response.d, $("#<%=lstCity.ClientID %>"));
               }
               function PopulateControl(list, control) {
                   console.log(list)
                   var lstCustomers = $("[id*=lstCity]");
                   $('#<%=lstCity.ClientID %>').empty();
                   $.each(list, function () {

                       $('#<%=lstCity.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');

                   });
                   $("#<%=lstCity.ClientID %>").multiselect('rebuild');
                   console.log($('#<%=lstCity.ClientID %> ul'));
               }
           }
           function BindDistributor() {
               var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'

               var selectedCity = [];

               $("#<%=lstCity.ClientID %> :selected").each(function () {
                   selectedCity.push($(this).val());
               });
               $("#<%=HiddenCity.ClientID %>").val(selectedCity);

               var obj = { CityID: $("#<%=HiddenCity.ClientID %>").val() };
               $.ajax({
                   type: "POST",
                   url: pageUrl + '/PopulateDistributorByMultiCity',
                   data: JSON.stringify(obj),
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   success: OnPopulated,
                   failure: function (response) {
                       alert(response.d);
                   }
               });
               function OnPopulated(response) {
                   PopulateControl(response.d, $("#<%=ListBox1.ClientID %>"));
               }
               function PopulateControl(list, control) {
                   console.log(list)
                   var lstCustomers = $("[id*=lstCity]");
                   $('#<%=ListBox1.ClientID %>').empty();
                   $.each(list, function () {

                       $('#<%=ListBox1.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');

                   });
                   $("#<%=ListBox1.ClientID %>").multiselect('rebuild');
                   console.log($('#<%=ListBox1.ClientID %> ul'));
               }
           }
    </script>

    <script type="text/javascript">
        $(function () {
            $('[id*=lstState]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 606,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $('[id*=lstCity]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 606,
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
        function validate(btnvalue) {
           
            var startDate = new Date($('#<%=txtfmDate.ClientID%>').val());
            var endDate = new Date($('#<%=txttodate.ClientID%>').val());
            if (startDate > endDate) {
                errormessage("From Date Should be less than To Date");
                return false;
            }
            var selectedState = [];

            $("#<%=lstState.ClientID %> :selected").each(function () {
                selectedState.push($(this).val());
            });
            if (selectedState == "") {
                errormessage("Please Select State.");
                return false;
            }

            var selectedCity = [];

            $("#<%=lstCity.ClientID %> :selected").each(function () {
                selectedCity.push($(this).val());
            });
            if (selectedCity == "") {
                errormessage("Please Select City.");
                return false;
            }


            var selectedDist = [];

            $("#<%=ListBox1.ClientID %> :selected").each(function () {
                selectedDist.push($(this).val());
            });
            if (selectedDist == "") {
                errormessage("Please Enter Distributor.");
                return false;
            }
            if ($('#<%=txtRemarks.ClientID%>').val() == "") {
                errormessage("Please Enter the Remark");
                return false;
            }
            if (btnvalue == "Save") {
                if (document.getElementById('<%=FileUpload1.ClientID%>').value == "") {
                    errormessage("Please Select Image.");
                    return false;
                }

            }
          
            $("#<%=HiddenDistributor.ClientID %>").val(selectedDist);
           
        }

        function CheckDate() {
            var today = new Date();
            var startDate = new Date($('#<%=txtfmDate.ClientID%>').val());
            if (startDate < today)
            {
                errormessage("From Date Should be Greater than or Equal To Today Date");
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            //BindState();
        });
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
          <%--  debugger;
                if ($('#<%=txtDeleteRemarks.ClientID%>').val() == "") {
                    errormessage("Please Enter Delete Remarks Name.");
                    return false;
                }--%>
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
        function DoNav(AreaId) {
            if (AreaId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                //document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', AreaId)
            }
        }
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
    </script>
      <script type="text/javascript">
          function LoadDiv(url) {
              var img = new Image();
              var bcgDiv = document.getElementById("divBackground");
              var imgDiv = document.getElementById("divImage");
              var imgFull = document.getElementById("imgFull");
              var imgLoader = document.getElementById("imgLoader");
              imgLoader.style.display = "block";
              img.onload = function () {
                  imgFull.src = img.src;
                  imgFull.style.display = "block";
                  imgLoader.style.display = "none";
              };
              img.src = url;
              var width = document.body.clientWidth;
              if (document.body.clientHeight > document.body.scrollHeight) {
                  bcgDiv.style.height = document.body.clientHeight + "px";
              }
              else {
                  bcgDiv.style.height = document.body.scrollHeight + "px";
              }
              imgDiv.style.left = (width - 650) / 2 + "px";
              imgDiv.style.top = "20px";
              bcgDiv.style.width = "100%";

              bcgDiv.style.display = "block";
              imgDiv.style.display = "block";
              return false;
          }
          function HideDiv() {
              var bcgDiv = document.getElementById("divBackground");
              var imgDiv = document.getElementById("divImage");
              var imgFull = document.getElementById("imgFull");
              if (bcgDiv != null) {
                  bcgDiv.style.display = "none";
                  imgDiv.style.display = "none";
                  imgFull.style.display = "none";
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
                            <h3 class="box-title">Slider Details</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" Visible="false" ID="btnFind" runat="server" Text="Find" class="btn btn-primary" 
                                   OnClick="btnFind_Click" />

                               
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="col-md-12 col-sm-9 col-xs-11">
                                  <div class="row">
                                      <div class="col-md-4 col-sm-12 col-xs-12">
                                         <div class="form-group">
                                              <label for="exampleInputEmail1">From Date:</label>
                                        <asp:TextBox ID="txtfmDate" runat="server" Width="100%" onchange="CheckDate();"  CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                          </div>
                                     </div>

                                          <div class="col-md-4 col-sm-12 col-xs-12">
                                         <div class="form-group">
                                             <label for="exampleInputEmail1">To Date:</label>
                                      <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                          </div>
                                     </div>
                                        <div class="col-md-4 col-sm-12 col-xs-12">
                                         <div class="form-group">
                                           <label for="exampleInputEmail1">State:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />                                           
                                            <asp:ListBox ID="lstState" runat="server" onChange="BindCity();" SelectionMode="Multiple"></asp:ListBox>
                                           <%-- <input type="hidden" id="HiddenState" />--%>
                                               <asp:HiddenField ID="HiddenState" runat="server"  /> 
                                             </div>
                                           </div>
                                        <div class="col-md-4 col-sm-12 col-xs-12">
                                         <div class="form-group">
                                           <label for="exampleInputEmail1">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />                                           
                                            <asp:ListBox ID="lstCity" onchange="BindDistributor();" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                           <%-- <input type="hidden" id="HiddenCity" />--%>
                                                <asp:HiddenField ID="HiddenCity" runat="server"  /> 
                                             </div>
                                           </div>
                                       <div class="col-md-4 col-sm-12 col-xs-12">
                                         <div class="form-group">
                                           <label for="exampleInputEmail1">Distributor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />                                           
                                            <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        <%--    <input type="hidden" id="hiddistributor" />--%>
                                               <asp:HiddenField ID="HiddenDistributor" runat="server"  /> 
                                             </div>
                                           </div>
                                     
                                        <%--<div class="clearfix"></div>--%>
                                       <div class="col-md-4 col-sm-12 col-xs-12">
                                         <div class="form-group">
                                              <label for="exampleInputEmail1">Remarks:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />
                                       <textarea runat="server" type="text" class="form-control" maxlength="498" style=" height:36px;" id="txtRemarks" placeholder="Enter Additional Information" tabindex="17" />  
                                             </div>
                                           </div>
                              
                                          <div class="col-md-4 col-sm-12 col-xs-12"  id="deleteDiv" runat="server" style="display:none;">
                                         <div class="form-group">
                                               <label for="exampleInputEmail1">Delete Remarks:</label>
                                       <textarea runat="server" type="text" class="form-control" maxlength="498" style=" height:36px;"  id="txtDeleteRemarks" placeholder="Enter Additional Information" tabindex="17" /> 
                                             </div>
                                           </div>
                                   
                                          <div class="col-md-4 col-sm-12 col-xs-12">
                                         <div class="form-group">
                                               <label for="exampleInputEmail1">Upload:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />
                                      <asp:FileUpload ID="FileUpload1"   runat="server"  tabindex="44"  onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" />
                                           <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                                ControlToValidate="FileUpload1" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                            
                                             </div>
                                             </div>
                                <%--      <div class="col-md-4 col-sm-12 col-xs-12">
                                          </div>--%>
                                         <div class="col-md-8 col-sm-12 col-xs-12">
                                         <div class="form-group">
                                           <img id="imgpreview" height="140" width="500"  class="pull-right" src="" style="border-width: 0px; display: none;padding-left: 15px;" runat="server" />
                                             </div>
                                            </div>
                                  </div>
                                 <%-- <div class="clearfix"></div>--%>
                                  
                                   
                            </div>
                        </div>
                      <%--  <div class="box-body">
                     <div class="col-lg-12 col-md-12 col-sm-9 col-xs-11">                               
                                <div class="col-md-12 paddingleft0">
                                    <div class="form-group col-md-3">
                                        <label for="exampleInputEmail1">From Date:</label>
                                        <asp:TextBox ID="txtfmDate" runat="server" Width="100%" onchange="CheckDate();"  CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                    </div>
                                    <div class="form-group col-md-3">
                                        <label for="exampleInputEmail1">To Date:</label>
                                      <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                    </div>                                                                   
                                </div>
                            <div class="col-md-6 col-sm-5 col-xs-10">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Distributor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />                                           
                                            <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                            <input type="hidden" id="hiddistributor" />
                                        </div>
                                    </div>
                           <div class="clearfix"></div>
                         <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Remarks:</label>
                                       <textarea runat="server" type="text" class="form-control" maxlength="498" style="height: 100px;width:731px" id="txtRemarks" placeholder="Enter Additional Information" tabindex="17" />                                                               
                                    </div>     
                     
                                <div class="col-md-12" id="deleteDiv" runat="server" style="display:none;">
                                 <label for="exampleInputEmail1">Delete Remarks:</label>
                                       <textarea runat="server" type="text" class="form-control" maxlength="498" style="height: 100px;width:731px" id="txtDeleteRemarks" placeholder="Enter Additional Information" tabindex="17" />    
                                 </div>
                        
                                <div class="col-md-12 paddingleft0">
                                    <div class="form-group col-md-3">
                                        <label for="exampleInputEmail1">Upload:</label>
                                      <asp:FileUpload ID="FileUpload1"   runat="server"  tabindex="44"  onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" />
                                           <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                                ControlToValidate="FileUpload1" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                                                      
                                    </div>
                                      <div class="clearfix"></div>
                                                                                                  
                                </div>
                                  <div class="col-md-12 paddingleft0">
                                    <div class="form-group col-md-8">
                                         <img id="imgpreview" height="150" width="600" class="pull-right" src="" style="border-width: 0px; display: none;" runat="server" />  
                                        </div>
                                      </div>
                            </div>
                        </div>--%>
                         
                        <div class="box-footer">
                             <asp:HiddenField ID="HiddenConUnderID" runat="server"  />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary"  OnClientClick="javascript:return validate(this.value);" TabIndex="28" OnClick="btnSave_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" TabIndex="30" OnClick="btnCancel_Click"/>
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" TabIndex="32" OnClick="btnDelete_Click" />
                        </div>
                        <br />
                            <div>
                                <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color:red">*</span>)</b>
                            </div>
                    </div>
                </div>

                <div id="divBackground" class="modal">
                    <div id="divImage">
                        <img id="imgLoader" alt="" src="img/close.png" />

                        <table style="height: 30%; width: 30%; align-content: center">
                            <tr>
                                <td align="center" valign="bottom">
                                    <img id="deletemodal" alt="" src="img/cross.jpg" style="margin-left: 100%; width: 15px; height: 15px" onclick="HideDiv()" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="center">
                                    <img id="imgFull" alt="" src="" style="display: none; height: 300px; width: 300px" />
                                </td>
                            </tr>

                        </table>
                    </div>
                </div>
                  <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">                      
                        <!-- /.box-header --> 
                     <div  class="box-body table-responsive">
                          
                              
           
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                              
                                                <th>S.No</th>
                                                <th>FromDate</th>
                                                <th>To date</th>
                                                <th>Remarks</th>
                                                <th>Image</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                              <%--      <tr onclick="DoNav('<%#Eval("Id") %>');">--%>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("Id") %>' />
                                        <td><asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" /></td>
                                        <td id="AreaName"><%#Eval("FromDate") %></td>
                                        <td id="Parent"><%#Eval("Todate") %></td>
                                    <td class="cur" onclick="DoNav('<%#Eval("Id") %>');"><asp:Label ID="testLbl" runat="server" Text='<%# Eval("Remarks") %>'  ToolTip='Click Here'></asp:Label></td>
                                        <%--<td class="cur" onclick="DoNav('<%#Eval("Id") %>');" id="SyncId"> <%#Eval("Remarks") %></td>--%>
                                        <td>
                                                 <asp:LinkButton ID="lnkViewDemoImg" runat="server" OnClick="lnkViewDemoImg_Click" Visible="false">View Image</asp:LinkButton>
                                                 <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl='<%# Eval("ImagePath")%>' AlternateText="No Image"
                                                    Width="25px" Height="25px" Style="cursor: pointer" OnClientClick="return LoadDiv(this.src);" />
                                             </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                       </div>
                        <!-- /.box---%>
                    </div>
                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>

        </div>
        </div>
            </div>

        </div>
        
          <%-- <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Sync Credit Debit Data</h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                               
                                <div class="row">
                                   <asp:Label ID="lblAm" runat="server"></asp:Label>
                                      <div class="col-md-4 col-sm-6 col-xs-12">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="txtfmDate" runat="server" Width="50%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                        </div>
                                    </div>
                                   <div class="col-md-1"></div>
                                      <div class="col-md-4 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="50%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                        </div>
                                    </div>
                                       <div class="col-md-4 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            </div>
                                           </div>
                                    </div>
                                 <div class="row">
                                     <div class="col-md-4 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                           <label for="exampleInputEmail1">Distributor:</label>
                                           <asp:ListBox ID="ListBox1" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                           <input type="hidden" id="hiddistributor" runat="server" />
                                        </div>
                                    </div>
                                     </div>
                                 <div class="row">
                                
                                     <div class="col-md-4 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                           <label for="exampleInputEmail1">Remarks:</label>
                                       <textarea runat="server" type="text" class="form-control" maxlength="498" style="height: 100px;width:731px" id="txtRemarks" placeholder="Enter Additional Information" tabindex="17" />                                                  
                                        </div>
                                    </div>
                                     </div>
                                 <div class="row">
                                      <div class="col-md-4 col-sm-6 col-xs-12" style="display:none;" id ="deleteDiv" runat="server">
                                        <div class="form-group">
                                      <label for="exampleInputEmail1">Delete Remarks:</label>
                                       <textarea runat="server" type="text" class="form-control" maxlength="498" style="height: 100px;width:731px" id="txtDeleteRemarks" placeholder="Enter Additional Information" tabindex="17" />                                                   
                                        </div>
                                    </div>
                                     </div>
                                 <div class="row">
                                       <div class="col-md-4 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                             <label for="exampleInputEmail1">Upload:</label>
                                      <asp:FileUpload ID="FileUpload1"   runat="server"  tabindex="44"  onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" />
                                           <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                                ControlToValidate="FileUpload1" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                          
                                            </div>
                                </div>
                                    
                            </div>
                                 <div class="row">
                                      <div class="col-md-4 col-sm-6 col-xs-12 paddingleft0">
                                        <div class="form-group">
                                              <img id="imgpreview" height="150" width="400" class="pull-right" src="" style="border-width: 0px; display: none;" runat="server" />
                                            </div>
                                         </div>
                                     </div>
                            <div class="box-footer">
                                 <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary"  OnClientClick="javascript:return validate(this);" TabIndex="28" OnClick="btnSave_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" TabIndex="30" OnClick="btnCancel_Click"/>
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" TabIndex="32" OnClick="btnDelete_Click" />
                              
                            </div>  
                        </div>
                    </div>
                        <div id="divBackground" class="modal">
                            <div id="divImage">
                                <img id="imgLoader" alt="" src="img/close.png" />

                                <table style="height: 30%; width: 30%; align-content: center">
                                    <tr>
                                        <td align="center" valign="bottom">
                                            <img id="deletemodal" alt="" src="img/cross.jpg" style="margin-left: 100%; width: 15px; height: 15px" onclick="HideDiv()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="center">
                                            <img id="imgFull" alt="" src="" style="display: none; height: 300px; width: 300px" />
                                        </td>
                                    </tr>

                                </table>
                            </div>
                        </div>
                 
                </div>
            </div>
        </div>
               </div>--%>
        
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
