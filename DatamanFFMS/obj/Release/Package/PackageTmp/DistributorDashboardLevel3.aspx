<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DistributorDashboardLevel3.aspx.cs" Inherits="AstralFFMS.DistributorDashboardLevel3" %>
   <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
 <script type="text/javascript">
     $(document).ready(function () {

         $('#ContentPlaceHolder1_basicExample').timepicker({ 'timeFormat': 'H:i' });
         $('#ContentPlaceHolder1_txtDistFrTime').timepicker({ 'timeFormat': 'H:i' });
         $('#ContentPlaceHolder1_txtDistToTime').timepicker({ 'timeFormat': 'H:i' });
         $('#ContentPlaceHolder1_txtNVTime').timepicker({ 'timeFormat': 'H:i' });
         $("#ContentPlaceHolder1_basicExample").keypress(function (event) { event.preventDefault(); });
         $("#ContentPlaceHolder1_txtDistFrTime").keypress(function (event) { event.preventDefault(); });
         $("#ContentPlaceHolder1_txtDistToTime").keypress(function (event) { event.preventDefault(); });
         $("#ContentPlaceHolder1_txtNVTime").keypress(function (event) { event.preventDefault(); });

     });
       </script>
        <script type="text/javascript">
            $(function () {
                //$("#example1").DataTable({ "bPaginate": false });
                $("#example1").DataTable();
            });
    </script>
 <style type="text/css">
     .modalBackground {
         background-color: Gray;
         filter: alpha(opacity=80);
         opacity: 0.8;
         z-index: 10000;
     }
 </style>
   <script>
       function vali() {

           if ($('#<%=txtdiscussion.ClientID%>').val() == '') {
               alert('Discussion cannot be Blank');
               errormessage("Please enter the Discussion");
               return false;
           }

           if ($('#<%=txtNextVisitDateDist.ClientID%>').val() == '') {
               alert('NextVisitDate cannot be Blank');
               errormessage("Please enter NextVisitDate");
               return false;
           }

           if ($('#<%=txtNVTime.ClientID%>').val() == '') {
               alert('NextVisitTime cannot be Blank');
               errormessage("Please enter NextVisitTime");
               return false;
           }

           if ($('#<%=txtDistFrTime.ClientID%>').val() == '') {
               alert('Distributor Start Time cannot be Blank');
               errormessage("Please enter DistributorStartTime");
               return false;
           }

           if ($('#<%=txtDistToTime.ClientID%>').val() == '') {
               alert('Distributor End Time cannot be Blank');
               errormessage("Please enter DistributorEndTime");
               return false;
           }
           var frmTime = $('#<%=DistFrTimeDDL.ClientID%>').val();
           var toTime = $('#<%=DistToTimeDDL.ClientID%>').val();
           var regExp = /(\d{1,2})\:(\d{1,2})\:(\d{1,2})/;
           var newval = parseInt(toTime.replace(regExp, "$1$2$3"))
           var newval1 = parseInt(frmTime.replace(regExp, "$1$2$3"))
           if (parseInt(toTime.replace(regExp, "$1$2$3")) < parseInt(frmTime.replace(regExp, "$1$2$3"))) {
               alert("End time Should be greater than Start Time");
               errormessage("End time Should be greater than Start Time");
               return false;
           }
       }
   </script> 
     <script type="text/javascript">
         function showpreview(input) {
             var uploadcontrol = document.getElementById('<%=dsrImgFileUpload.ClientID%>').value;
             var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
             if (uploadcontrol.length > 0) {
                 if (reg.test(uploadcontrol)) {
                     if (input.files && input.files[0]) {
                         var reader = new FileReader();
                         reader.onload = function (e) {
                             $("#ContentPlaceHolder1_imgpreview").css('display', 'none');
                             $("#ContentPlaceHolder1_imgpreview").attr('src', e.target.result);
                             $("#ContentPlaceHolder1_HypUploadStatus").css('display', 'none');
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
    <script>
        function valiL1() {

            if ($('#<%=txtDiscussionwithL1.ClientID%>').val() == '') {
                alert('Discussion with L1 cannot be Blank');
                errormessage("Please enter the Discussion with L1");
                return false;
            }
        }
   </script>
       <script>
           function valiAreaPlan() {

               if ($('#<%=txtAreaPlan.ClientID%>').val() == '') {
                   alert('Area plan cannot be Blank');
                   errormessage("Please enter the Area Plan");
                   return false;
               }
           }
   </script>

       <script type="text/javascript">
           function validateFV() {
               $('#<%=lblerrorFV.ClientID%>').removeClass('hidden');
               $('#<%=lblerrorFV.ClientID%>').val("");

               if ($('#<%=ddlReason.ClientID%>').val() == "0") {

                   $('#<%=lblerrorFV.ClientID%>').html("* Please select the Reason.");
                   return false;
               }
               if ($('#<%=TextBox1.ClientID%>').val() == "") {
                   $('#<%=lblerrorFV.ClientID%>').html("* Please enter the Remark.");
                 return false;
             }
             if ($('#<%=txtNextVisitdate.ClientID%>').val() == "") {
                   $('#<%=lblerrorFV.ClientID%>').html("* Please enter the Next Visit Date.");
                 return false;
             }

             if ($("#ContentPlaceHolder1_basicExample").val() == "") {
                 $('#<%=lblerrorFV.ClientID%>').html("* Please select Time.");
                 return false;
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
       <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body" id="mainDiv"  runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Distributor Screen L-3</h3>
                            <div style="float: right">
                              <asp:Button Style="margin-right: 5px;" type="button" ID="Button4" runat="server" Text="Back" class="btn btn-primary" OnClick="Button4_Click"  />
                            </div>
                        </div>
                         <div class="col-md-12 paddingleft0">
                        <div class="form-group col-md-4">
                                            <label for="exampleInputEmail1">Planned Beat:</label>
                                            <asp:Label ID="lblPlanedbeat" runat="server" Text=""></asp:Label>
                                        </div>
                                </div>
                         <div class="box-body">
                            <div class="col-lg-9 col-md-8 col-sm-7 col-xs-9">
                          <div class="col-md-12 paddingleft0">
                            <div id="DIV1"   class="form-group col-md-4">
                                    <label for="exampleInputEmail1">Distributor Name:</label>
                               <asp:TextBox id="txtsearch" runat="server" style="background: white;" CssClass="form-control"></asp:TextBox>

                                </div> 
                                <div  class="form-group col-md-4 ">
                                     <label for="exampleInputEmail1" style="display:block; visibility:hidden">zkjfhksj</label>
                              <asp:Button type="button" ID="Button8" runat="server" style="padding: 3px 14px;" Text="Go" class="btn btn-primary" OnClick="Button5_Click" />
                              
                                </div> 
                                <div  class="form-group col-md-4">
                                      
                                 
                                </div> 
                                </div>   
                                </div></div> 
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-md-12">
 <div class="box-body table-responsive">
     <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand" OnItemDataBound="rpt_ItemDataBound">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Code</th>
                                                <th>Distributor Name</th>
                                                <th>City</th>
                                                <th>Discussion</th>
                                                 <th>Failed Visit</th>
                                                 <th>Collection</th>
                                                  <th>Stock Item Template</th>
                                                 <th>Opening Stock</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                       <asp:HiddenField ID="did" runat="server" Value=<%#Eval("PartyId")%>/> 
                                   <td><%#Eval("SyncId") %></td>
                                        <td><%#Eval("PartyName") %></td>
                                        <td><%#Eval("CityName") %></td>
                                        <%--<td id="discLinkCell" runat="server"> <asp:LinkButton ID="imgRptDisc" Text="Discussion" OnClick="imgRptDisc_Click" runat="server" CommandArgument=<%#Eval("PartyId")%>></asp:LinkButton></td>--%>
                                        <td id="discLinkCell" runat="server"> <asp:LinkButton ID="imgRptDisc" Text="Discussion" runat="server"  CommandArgument=<%#Eval("PartyId")%> CommandName="DDisc"></asp:LinkButton></td>
                                        <td id="discFailVCell" runat="server"><asp:LinkButton ID="imgRptFail1" Text="Failed Visit" OnClick="imgRptFail1_Click" runat="server" CommandArgument=<%#Eval("PartyId")%>></asp:LinkButton></td>   
                                        <td id="collLinkCell" runat="server"><asp:LinkButton ID="lnkRptDist" Text="Collection" CommandArgument=<%#Eval("PartyId")%> CommandName="Coll"  runat="server"></asp:LinkButton></td>
                                         <td id="cloDisTemplate" runat="server"> <asp:LinkButton ID="lnkDistTemplate"  Text="Stock Template" runat="server"  CommandArgument=<%#Eval("PartyId")%> CommandName="StockTemplate"></asp:LinkButton></td>
                                        <td id="colStock" runat="server"> <asp:LinkButton ID="lnkStock"  Text="Stock" runat="server"  CommandArgument=<%#Eval("PartyId")%> CommandName="Stock"></asp:LinkButton></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
<%--<asp:GridView runat="server" ID="gvdetails" OnRowDataBound="gvdetails_RowDataBound"   OnRowCommand="gvdetails_RowCommand" CssClass="table table-bordered table-striped"  AutoGenerateColumns="false">
<Columns>
     <asp:BoundField DataField="SyncId" HeaderText="Code" />
<asp:BoundField DataField="PartyName" HeaderText="Distributor Name" />
    <asp:BoundField DataField="CityName" HeaderText="City" />
  
  <asp:TemplateField HeaderText="Discussion">
<ItemTemplate>
    <asp:HiddenField ID="did" runat="server" Value=<%#Eval("PartyId")%>/> 
    <asp:LinkButton ID="imgbtn" Text="Discussion"   OnClick="imgbtn_Click1" runat="server"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>

      <asp:TemplateField HeaderText="Failed Visit">
<ItemTemplate>
    <asp:LinkButton ID="imgbtn1" Text="Failed Visit"   OnClick="imgbtn_Click2" runat="server"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>

     <asp:TemplateField HeaderText="Collection">
<ItemTemplate>
    <asp:LinkButton ID="lnkDist" Text="Collection" CommandArgument=<%#Eval("PartyId")%> CommandName="Coll"  runat="server"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>


</Columns>
</asp:GridView>--%>
     </div>
    <div class="box-footer">
     <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Next" class="btn btn-primary" OnClick="btnGo_Click"  />
                           
                        </div>
   <asp:Label ID="lblresult" runat="server" />

<asp:Button ID="btnShowPopup" runat="server" style="display:none" />
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
CancelControlID="btnCancel" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>

<asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="520px" Width="394px"  style="display:none">
<table width="95%"  height="100%" cellpadding="6" cellspacing="0" style="margin-left:10px;">
  <tr style="background-color: #367FA9;">
        <td colspan="2" style=" height:10%; color:White; font-weight:bold; font-size:larger">Discussion with Distributor</td>
</tr>
   

<tr>
<td align="right">
<%--Disributor Name :--%>
</td>
<td>
<asp:Label ID="lblusername" runat="server" Visible="false"></asp:Label>
    <asp:HiddenField ID="hidDisID" runat="server" />
</td>
</tr>
    <tr>
    <td style="padding:10px" colspan="2">
<asp:TextBox ID="txtdiscussion" CssClass="form-control" style="resize: none; height: 70px;" Width="100%" TextMode="MultiLine" runat="server"/>
</td>
        
           

</tr>
    
    <tr>
<td  align="left">
<label for="withSales">Time Spent with Distributor -</label>
</td>
        </tr>
    <tr>
        <td style="width:50%;"><label for="withSales">Start Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label></td>
        <td>
<label for="withSales">End Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
</td>
    </tr>

    <tr>
 <td align="left" >
     <asp:DropDownList ID="DistFrTimeDDL" runat="server" CssClass="form-control" Width="70%"></asp:DropDownList>
  <input type="text" data-scroll-default="6:00am" maxlength="7" width="60%" placeholder="--Select Time--" Class="form-control"  id="txtDistFrTime" runat="server"
       autocomplete="off" visible="false">
</td> 
    <td align="left" style="padding-left:2px;">
         <asp:DropDownList ID="DistToTimeDDL" runat="server" CssClass="form-control" style="width:90%;"></asp:DropDownList>
  <input type="text" data-scroll-default="6:00am" maxlength="7" width="60%" placeholder="--Select Time--" Class="form-control"  id="txtDistToTime" runat="server" autocomplete="off" visible="false">
</td>
</tr>

    <tr>
<td align="left" >
<label for="withSales">Next Visit Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
</td>
        <td align="left" style="padding:10px">
<label for="withSales">Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
</td>
    </tr>
    <tr>
<td align="left" >
<asp:TextBox ID="txtNextVisitDateDist" CssClass="form-control"  width="90%" runat="server"/>
 <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"  CssClass="orange"  Format="dd/MMM/yyyy"   TargetControlID="txtNextVisitDateDist"/>
</td>
    <td align="left" >
         <asp:DropDownList ID="NVTimeDDL" runat="server" CssClass="form-control" style="width:90%;"></asp:DropDownList>
  <input type="text" data-scroll-default="6:00am" maxlength="7" width="90%" placeholder="--Select Time--" Class="form-control"  id="txtNVTime" runat="server" autocomplete="off" visible="false">
</td>
</tr>
 <tr><td>  <div >
                                        <label for="exampleInputEmail1">Image:</label>
                                        <asp:FileUpload ID="dsrImgFileUpload" runat="server" onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                            ControlToValidate="comImgFileUpload" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                        </asp:RegularExpressionValidator>
                                      <%--  <asp:Label id="UploadStatusLabel" runat="server" style="display: none;"></asp:Label>  --%>
       <asp:HyperLink ID="HypUploadStatus" runat="server" Target="_blank"></asp:HyperLink>
                                        <img id="imgpreview" height="200" width="200" src="" style="border-width: 0px; display: none;" runat="server" />
                                    </div>
</td></tr>
     <tr><td colspan="2">
                      
                                <b>Note : Image size should be less than 1MB</b>
                       </td></tr>
 
<tr align="center">

<td style="padding:20px">
<asp:Button ID="btnUpdate" OnClientClick="return vali();" CommandName="Update" runat="server" class="btn btn-primary" Text="Save" onclick="btnUpdate_Click"/>
<asp:Button ID="btnCancel" class="btn btn-primary" runat="server" Text="Cancel" onClick="btnCancel_Click"/>
</td>
</tr>
      <tr><td colspan="2">
                      
                                <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                       </td></tr>
</table>
</asp:Panel>


                                <asp:Button ID="Button5" runat="server" style="display:none" />
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="Button5" PopupControlID="Panel2"
CancelControlID="btnCancel" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>

<asp:Panel ID="Panel2" runat="server" BackColor="White" Height="180px" Width="250px"  style="display:none">

<table width="100%"  height:"100%" cellpadding="6" cellspacing="0">
<tr style="background-color: #367FA9;">
<td colspan="2" style=" height:10%; color:White; font-weight:bold; font-size:larger" align="center">Discussion with L1</td>
</tr>

<tr>
<td align="right">
<%--Disributor Name :--%>
</td>
<td>
<asp:Label ID="Label1" runat="server"></asp:Label>
    <asp:HiddenField ID="HiddenField1" runat="server" />
</td>
</tr>
<tr align="center">
<!--<td style="padding:10px">
Discussion with L1
</td>-->

</tr>
    <tr align="center">
    <td style="padding:10px">
<asp:TextBox ID="txtDiscussionwithL1" CssClass="form-control" style="resize: none; height: 100%;" Width="80%" TextMode="MultiLine" runat="server"/>
</td>

</tr>
 
<tr align="center">

<td style="padding:20px">
<asp:Button ID="btnDisWithL1" OnClientClick="return valiL1();" CommandName="Update" runat="server" class="btn btn-primary" Text="Save" onclick="btnDisWithL1_Click"/>
<asp:Button ID="Button7" class="btn btn-primary" runat="server" Text="Cancel" />
</td>
</tr>
</table>
</asp:Panel>


         <asp:Button ID="Button6" runat="server" style="display:none" />
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="Button6" PopupControlID="Panel3"
CancelControlID="btnCancel" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>

<asp:Panel ID="Panel3" runat="server" BackColor="White" Height="180px" Width="250px"  style="display:none">

<table width="100%"  height:"100%" cellpadding="6" cellspacing="0">
<tr style="background-color: #367FA9;">
<td colspan="2" style=" height:10%; color:White; font-weight:bold; font-size:larger" align="center">Area Plan</td>
</tr>

<tr>
<td align="right">

</td>
<td>
<asp:Label ID="Label2" runat="server"></asp:Label>
    <asp:HiddenField ID="HiddenField2" runat="server" />
</td>
</tr>
<tr align="center">
<!--<td style="padding:10px">
Area Plan
</td>-->

</tr>
    <tr align="center">
    <td style="padding:10px">
<asp:TextBox ID="txtAreaPlan" CssClass="form-control" style="resize: none; height: 100%;" Width="80%" TextMode="MultiLine" runat="server"/>
</td>

</tr>
 
<tr align="center">

<td style="padding:20px">
<asp:Button ID="btnAreaPlan" OnClientClick="return valiAreaPlan();" CommandName="Update" runat="server" class="btn btn-primary" Text="Save" onclick="btnAreaPlan_Click"/>
<asp:Button ID="Button9" class="btn btn-primary" runat="server" Text="Cancel" />
</td>
</tr>
</table>
</asp:Panel>



                                
<asp:Button ID="Button1" runat="server" style="display:none" />
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="Button1" PopupControlID="Panel1"
CancelControlID="btnCancel" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>

<asp:Panel ID="Panel1" runat="server" BackColor="White" Height="380px" Width="300px"  style="display:none">
    <div  style="height:10%; background-color: #367FA9; color:white; font-weight:bold; font-size:larger" align="center">
        Failed Visit
    </div>
    <div style="margin-left:10px;margin-right:10px">
<table style="padding-left:5px" width="100%" height:"100%" cellpadding="0" cellspacing="0">
<tr style="background-color: #367FA9;">
<td colspan="2" style=" height:10%; color:White; font-weight:bold; font-size:larger" align="center"></td>
</tr>
 <tr>
        <td align="left" colspan="2">
           <label class="hidden" id="lblerrorFV" runat="server" style="color:red;"></label>
        </td>
    </tr>
<tr>
<td align="left">
<label for="withSales">Reason:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
</td>
    <td></td>
    </tr>
<tr>
<td align="left" colspan="2">
   <asp:DropDownList ID="ddlReason" CssClass="form-control" runat="server"></asp:DropDownList>
</td>
</tr>
<tr>
<td align="left">
<label for="withSales">Remark:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
</td>
    <td></td>
    </tr>
    <tr>
<td align="left" colspan="2">
<asp:TextBox ID="TextBox1"  style="resize: none; width: 100%;height: 100%;"  TextMode="MultiLine" runat="server"/>
</td>
</tr>
    <tr>
<td align="left">
<label for="withSales">Next Visit Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
</td>
<td align="left">
<label for="withSales">Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
</td>
    </tr>

<tr>
<td align="left">
<asp:TextBox ID="txtNextVisitdate" class="form-control"  width="90%"  runat="server"/>
 <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server"  CssClass="orange"  Format="dd/MMM/yyyy"   TargetControlID="txtNextVisitdate"/>
</td>
<td align="left">
         <asp:DropDownList ID="basicExampleDDL" runat="server" CssClass="form-control"></asp:DropDownList>
  <input type="text" data-scroll-default="6:00am" placeholder="--Select Time--" class="form-control"  width="90%"  id="basicExample" runat="server" autocomplete="off" visible="false">
</td>
</tr>
    
<tr  align="left" style="padding:8px">
    <td height="50px" style="padding:8px"><asp:Button ID="Button2" CommandName="Update" runat="server" style="float: right;" class="btn btn-primary" Text="Save" onclick="Button2_Click" OnClientClick="javascript:return validateFV();"/></td>
<td height="50px" style="padding:8px">

<asp:Button ID="Button3" runat="server" class="btn btn-primary" Text="Cancel" />
</td>
</tr>
      <tr><td colspan="2">
                      
                                <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                       </td></tr>
</table>
    </div>


</asp:Panel>
           <!-- /.box-body -->
                    </div>
                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>

        </div>
                </div></div>
     

    </section>
</asp:Content>