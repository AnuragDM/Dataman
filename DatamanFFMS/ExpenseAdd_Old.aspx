<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ExpenseAdd_Old.aspx.cs" Inherits="AstralFFMS.ExpenseAdd_Old" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
          
      <script type="text/javascript">
          //$(document).ready(function () {
          //    $('#ContentPlaceHolder1_basicExample').timepicker({ 'timeFormat': 'H:i' });
          //    $('#ContentPlaceHolder1_basicExample1').timepicker({ 'timeFormat': 'H:i' });
          //    $("#ContentPlaceHolder1_basicExample").keypress(function (event) { event.preventDefault(); });
          //    $("#ContentPlaceHolder1_basicExample1").keypress(function (event) { event.preventDefault(); })

          //    $('#ContentPlaceHolder1_txtTrFromTime').timepicker({ 'timeFormat': 'H:i' });
          //    $('#ContentPlaceHolder1_txtTrToTime').timepicker({ 'timeFormat': 'H:i' });
          //    $("#ContentPlaceHolder1_txtTrFromTime").keypress(function (event) { event.preventDefault(); });
          //    $("#ContentPlaceHolder1_txtTrToTime").keypress(function (event) { event.preventDefault(); })
          //    //    $('#ContentPlaceHolder1_basicExample').attr('readonly', 'readonly');
          //});
       </script>
    <script lang="javascript" type="text/javascript">
        function ShowOptions(control, args) {
            control._completionListElement.style.zIndex = 10000001;
        }
</script> 
   
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
     .insidebtnmy {
         padding: 0 4px;
     }
        /*.tooltipDemo
        {           
            position: relative;
            display: inline;
            text-decoration: none;
            left: 10px;
            top: 0px; 
            
             display: block;
  width: 100%;
  height: 34px;
  padding: 6px 12px;
  font-size: 14px;
  line-height: 1.42857143;
  color: #555;
  background-color: #fff;
  background-image: none;
  border: 1px solid #ccc;
  border-radius: 4px;
  -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
          box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
  -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s;
       -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
          transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;    
        }*/

     #ContentPlaceHolder1_gdvprodgrp tbody th {
         background: #367FA9;
         color: white;
     }

     input[type=checkbox] {
         margin: 4px 4px 0 !important;
     }

     .modalBackground {
         background-color: Gray;
         filter: alpha(opacity=80);
         opacity: 0.8;
         z-index: 1000;
     }        

     @media (max-width: 600px) {
         #pnlpopup {
             width: 295px;
         }
     }

     @media (min-width: 600px) {
         #pnlpopup {
             width: 400px;
         }
     }

     @media (max-width: 600px) {
         #ContentPlaceHolder1_pnlpopup {
             width: 100%;
         }
     }

     @media (min-width: 600px) {
         #ContentPlaceHolder1_pnlpopup {
             width: 500px;
         }
     }

     @media (max-width: 600px) {
         #ContentPlaceHolder1_PnlTravel {
             width: 100%;
         }
     }

     @media (min-width: 600px) {
         #ContentPlaceHolder1_PnlTravel {
             width: 500px;
         }
     }

     @media (max-width: 600px) {
         #ContentPlaceHolder1_PnlConv {
             width: 100%;
         }
     }

     @media (min-width: 600px) {
         #ContentPlaceHolder1_PnlConv {
             width: 600px;
         }
     }


     @media (min-width: 400px) and (max-width:600px) {
         .gdvprodgpclass {
             width: 380px;
         }
     }

     @media (min-width: 200px) and (max-width:400px) {
         .gdvprodgpclass {
             width: 240px;
         }
     }

     @media (min-width: 600px) {
         .gdvprodgpclass {
             width: 520px;
         }
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
        function BindCity() {
            $('#<%=ddlCity.ClientID%>').empty();
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("DataBindService.asmx/PopulateCityByState") %>',
                contentType: "application/json; charset=utf-8",
                data: '{StateID: "' + $('#<%=ddlState.ClientID%>').val() + '" }',

                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);

                    $.each(jsdata1, function (key1, value1) {
                        $('#<%=ddlCity.ClientID%>').append($("<option></option>").val(value1.CityID).html(value1.CityName));
                     });
                 }
            });
             }
             function BindTr1City() {
                 $('#<%=ddltr1city.ClientID%>').empty();
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("DataBindService.asmx/PopulateCityByState") %>',
                    contentType: "application/json; charset=utf-8",
                    data: '{StateID: "' + $('#<%=ddltr1state.ClientID%>').val() + '" }',

                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);

                    $.each(jsdata1, function (key1, value1) {
                        $('#<%=ddltr1city.ClientID%>').append($("<option></option>").val(value1.CityID).html(value1.CityName));
                      });
                  }
                });
              }
              function BindTr2City() {
                  $('#<%=ddltr2city.ClientID%>').empty();
                 $.ajax({
                     type: "POST",
                     url: '<%= ResolveUrl("DataBindService.asmx/PopulateCityByState") %>',
                    contentType: "application/json; charset=utf-8",
                    data: '{StateID: "' + $('#<%=ddltr2state.ClientID%>').val() + '" }',

                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);

                    $.each(jsdata1, function (key1, value1) {
                        $('#<%=ddltr2city.ClientID%>').append($("<option></option>").val(value1.CityID).html(value1.CityName));
                      });
                  }
                });
              }
        function BindConvCity() {
           // alert($('#<%=ddlexpensetype.ClientID%>').find("option:selected").text())
                  $('#<%=ddlConvcity.ClientID%>').empty(); $('#<%=ddlparty.ClientID%>').empty();
                 $.ajax({
                     type: "POST",
                     url: '<%= ResolveUrl("DataBindService.asmx/PopulateCity") %>',
                    contentType: "application/json; charset=utf-8",
                    data: '{StateID: "' + $('#<%=ddlConvstate.ClientID%>').val() + '",ExptypeVal: "' + $('#<%=ddlexpensetype.ClientID%>').val() + '"}',
                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    $('#<%=ddlConvcity.ClientID%>').append($("<option></option>").val(0).html('--Select--'));
                    $.each(jsdata1, function (key1, value1) {
                        $('#<%=ddlConvcity.ClientID%>').append($("<option></option>").val(value1.CityID).html(value1.CityName));
                    });
                }
                });
            }
            function BindPartyDistributorBycity() {
                $('#<%=ddlparty.ClientID%>').empty();
                var CityAmt = 0;
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("DataBindService.asmx/PopulatePartyDistributorBycity") %>',
                    contentType: "application/json; charset=utf-8",
                    data: '{CityID: "' + $('#<%=ddlConvcity.ClientID%>').val() + '" ,ExptypeVal: "' + $('#<%=ddlexpensetype.ClientID%>').val() + '"}',

                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    $('#<%=ddlparty.ClientID%>').append($("<option></option>").val(0).html('--Select--'));
                    $.each(jsdata1, function (key1, value1) {
                        $('#<%=ddlparty.ClientID%>').append($("<option></option>").val(value1.PartyId).html(value1.PartyName));
                        CityAmt = value1.ConveyanceAmt;
                      
                    });
                    if ($('#<%=ddlexpensetype.ClientID%>').find("option:selected").text() == "CONVEYANCE") {
                       
                        $('#<%=ConvAmt.ClientID%>').val(Math.round(CityAmt, 2));
                        $('#<%=ConvClaimAmt.ClientID%>').val($('#<%=ConvAmt.ClientID%>').val());
                        $('#<%=ConvAmt.ClientID%>').attr("disabled", "disabled");
                        $('#<%=ConvClaimAmt.ClientID%>').attr("disabled", "disabled");
                    }
                }
                });
              
            }
        function GetAmtAllowedByCity() {
            if ($('#<%=ddlexpensetype.ClientID%>').find("option:selected").text() == "CONVEYANCE") {
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("DataBindService.asmx/GetAmtAllowedByCity") %>',
                    contentType: "application/json; charset=utf-8",
                    data: '{CityID: "' + $('#<%=ddlConvcity.ClientID%>').val() + '" }',

                    dataType: "json",
                    success: function (data) {
                        var amtcity=(data.d);
                        $('#<%=ConvAmt.ClientID%>').val(Math.round(amtcity, 2));
                        $('#<%=ConvClaimAmt.ClientID%>').val($('#<%=ConvAmt.ClientID%>').val());
                      
                    }
                });
            }
          
        }
        function SetPerKmRate() {
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("DataBindService.asmx/GetPerKmRate") %>',
                contentType: "application/json; charset=utf-8",
                data: '{TMID: "' + $('#<%=ddlconvtravelmode.ClientID%>').val() + '" }',

                dataType: "json",
                success: function (data) {
                    $('#<%=RatePerKm.ClientID%>').val(data.d);
                        if (parseFloat($('#<%=RatePerKm.ClientID%>').val()).toFixed(2) > 0) {
                            $('#divkmvisited').show(); $('#divrateperkm').show();
                            $('#<%=spntrmode.ClientID%>').attr('style', 'display:block');
                            $('#<%=ConvAmt.ClientID%>').val(0);
                            $('#<%=ConvClaimAmt.ClientID%>').val(0);
                            $('#<%=ConvAmt.ClientID%>').prop('disabled', true);
                            $('#<%=ConvClaimAmt.ClientID%>').prop('disabled', true);
                        }
                        else {
                            $('#divkmvisited').hide(); $('#divrateperkm').hide();
                            $('#<%=spntrmode.ClientID%>').attr('style', 'display:none');
                            $('#<%=ConvAmt.ClientID%>').val(0);
                            $('#<%=ConvClaimAmt.ClientID%>').val(0);
                            $('#<%=ConvAmt.ClientID%>').prop('disabled', false);
                            $('#<%=ConvClaimAmt.ClientID%>').prop('disabled', false);
                        }
                    }
            });
            }
        function GetPartyVal(){
            $("#<%=hdnparty.ClientID%>").val($('#<%=ddlparty.ClientID%>').find("option:selected").text());
        }
    </script>
    <script type="text/javascript">
        function ClientPartySelected(sender, e) {
            $get("<%=hdnparty.ClientID %>").value = e.get_value();
        }
    </script>
     <script type="text/javascript">
         function SetContextKey() {
             $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey($get("<%=ddlConvcity.ClientID %>").value);
          }
    </script>
  <section class="content">
       <div id="spinner"  runat="server"  class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
        </div>
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
                            <h3 class="box-title">Expense Summary</h3>
                                 <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Expense Group" class="btn btn-primary" PostBackUrl="~/ExpenseGrp.aspx"
                                 />
                            </div>
                                         </div>
                          <div class="box-header">
                              <div >
                                Expense Group:
                                  <label id="lblexpgrp" runat="server"></label>
                              
                        </div>
                              
                        <!-- /.box-header -->
                        <!-- form start -->
                                 <asp:Panel ID="pnexpdetails" runat="server">
                        <div class="box-body">
                        <div class="row">
                             <asp:HiddenField ID="HdnFldSMID" runat="server" />   
                                  <div class="col-md-12">
                                          <div class="col-md-4" style="margin-bottom: 5px;">
                                 <asp:DropDownList ID="ddlexpensetype" CssClass="form-control" runat="server"></asp:DropDownList>
                                              </div>
                                        <div class="col-md-4">
                              <asp:Button Style="margin-right: 10px;" type="button" ID="btnAddNewExpense" runat="server" Text="Add New Expense" class="btn btn-primary" OnClick="btnAddNewExpense_Click"/>
                                      </div></div></div>
                              <br />
                         
                              </asp:Panel>
<asp:Button ID="btnShowPopup" runat="server" style="display:none" />
                            <asp:HiddenField ID="hidForModel" runat="server" />
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
CancelControlID="btnCancel" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>

<asp:Panel ID="pnlpopup" runat="server"  ScrollBars="Vertical"  BackColor="White"  style="margin-top:10px;height:100%;display:none">

        <div id="spinner1"  runat="server"  class="spinner" style="display: none;">
            <img id="img-spinner1" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
        </div>
<table width="100%"  height:"100%" cellpadding="6" class="table" cellspacing="0">
<tr style="background-color: #367FA9;">
<td colspan="2" style=" height:10%; margin-top:10px;color:White; font-weight:bold; font-size:larger; text-align: center;" align="center">Expense Details <br/><label id="lblExname" style="color:white;" runat="server" ></label></td>
</tr>
     <tr align="center"> <td>
       <label class="hidden" id="lblerrorAddExp" runat="server" style="color:red;"></label> </td></tr>
    <tr align="center">
    <td style="padding:5px 10px;">
<label id="" >State:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
        <asp:DropDownList ID="ddlState" CssClass="form-control" onChange="BindCity()" runat="server" TabIndex="2"></asp:DropDownList>
        </td><td style="padding:5px 10px;">
        <label id="" >City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
        <asp:DropDownList ID="ddlCity" TabIndex="3" CssClass="form-control" runat="server"></asp:DropDownList>
</td>
</tr>
     <tr align="center">
         
    <td style="padding:5px 10px;"  class="text-center" colspan="2">
<div class="col-sm-12 text-center">
    <label id="" >Supporting Attached:</label>
     <asp:CheckBox ID="chkSuppAtt" runat="server" TabIndex="4" CssClass="" />
</div>

    
</td>
         
</tr>
       <tr id="trStayRelative" runat="server" align="center">
           
    <td style="padding:5px 10px;" colspan="2">
        <div class="col-sm-12 text-center">
            <label id="" >Stay With Relatives:</label>
     <asp:CheckBox ID="chkStayWithRelative" runat="server"  TabIndex="5" CssClass="" />
        </div>
</td>
</tr>
    <tr>
        <td colspan="2">
            <div class="col-sm-12 text-center">
                <label id="" style="display:none;">Allow To Save With 0 Amount: &nbsp;</label>
        <asp:CheckBox ID="chkallowtosave" style="display:none;" runat="server"  CssClass="" TabIndex="6" />
           </div>
            </td>
    </tr>
    <tr align="center" runat="server" id="Tr_dateto">
        
    <td>
<label id="" > From Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
       <asp:TextBox ID="fromdt" runat="server" CssClass="form-control"  tabindex="7"></asp:TextBox>
                               <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange"  Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox3_CalendarExtender"
                                        TargetControlID="fromdt"></ajaxToolkit:CalendarExtender>
          
    </td>
        <td><label>From Time: </label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label> 
               <asp:DropDownList ID="startTimeDDL" runat="server" CssClass="form-control"></asp:DropDownList>
            <input type="text" maxlength="7"  data-scroll-default="6:00 am" visible="false" placeholder="--Select Time--" Class="form-control"  id="basicExample" runat="server" autocomplete="off"></td>
        </tr>
    <tr align="center" runat="server" id="Tr_dateto1">
        <td>
        <label id="" >To Date: &nbsp;</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
        <asp:TextBox ID="todt" runat="server" CssClass="form-control"  tabindex="8"></asp:TextBox>
                               <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange"  Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox4_CalendarExtender"
                                        TargetControlID="todt"></ajaxToolkit:CalendarExtender></td>
        <td>
      <label>To Time:</label> &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>  
               <asp:DropDownList ID="endTimeDDL" runat="server" CssClass="form-control"></asp:DropDownList>
             <input type="text" maxlength="7" data-scroll-default="7:00 pm" visible="false" placeholder="--Select Time--" Class="form-control" id="basicExample1" runat="server" autocomplete="off">
</td>
</tr>    
       <tr align="center">
           <td style="padding:5px 10px;">
        <label id="" >Bill Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
              <asp:TextBox ID="BillDate" runat="server" CssClass="form-control"  tabindex="10"></asp:TextBox>
                               <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange"  Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender"
                                        TargetControlID="BillDate"></ajaxToolkit:CalendarExtender>
</td>
    <td style="padding:5px 10px;">
<label id="" >Bill No.:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
    <asp:TextBox ID="BillNumber" TabIndex="12" CssClass="form-control" MaxLength="10" runat="server"></asp:TextBox>
        </td>
           </tr>
    
     <tr align="center">
         <td style="padding:5px 10px;">
<label id="" >Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
            <asp:TextBox ID="Amount" MaxLength="10" Onblur="javascript:FillClaimAmt();" runat="server" CssClass="form-control numeric text-right" tabindex="14"></asp:TextBox>
        </td>
         <td style="padding:5px 10px;">
        <label id="" >Claim Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
       <asp:TextBox ID="ClaimAmount" MaxLength="10" runat="server" Onblur="javascript:chkClaimAmt();" CssClass="form-control numeric text-right" style="background-color:white;" tabindex="15"></asp:TextBox>
</td>
    
         </tr>
      
    <tr align="center" style="display:none;">
        <td></td>
    <td style="padding:5px 10px;">
        <label id="" >Sync ID:</label>&nbsp;&nbsp;
       <asp:TextBox ID="SyncId" MaxLength="10" runat="server" CssClass="form-control" style="background-color:white;" ></asp:TextBox>
</td>
</tr>
       <tr align="center">
           
    <td style="padding:5px 10px;" colspan="2">
        <label id="" >Remark:</label>
       <asp:TextBox ID="Remarks" runat="server" MaxLength="200" CssClass="form-control" TextMode="MultiLine" style="background-color:white;resize: none; height: 90%" cols="20" rows="2" tabindex="18"></asp:TextBox>
</td>
</tr>

<tr align="center">
    
<td colspan="2">
<asp:Button ID="btnSaveNewExp"  OnClientClick="return Validate();" TabIndex="20" runat="server" OnClick="btnSaveNewExp_Click" class="btn btn-primary" Text="Save" />
<asp:Button ID="btnCancel" class="btn btn-primary" runat="server" Text="Cancel" TabIndex="22"/>
</td>
</tr>
</table>
     <div style="height: 57px;">
 <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color:red">*</span>)</b>
        <br/></div>
</asp:Panel>

          <asp:Button ID="BtnTr" runat="server" style="display:none" />
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="BtnTr" PopupControlID="PnlTravel"
CancelControlID="btnTrcancel" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>

<asp:Panel ID="PnlTravel" runat="server" ScrollBars="Vertical" BackColor="White" Height="100%" style="display:none">

<table width="100%"  height:"100%" cellpadding="6" cellspacing="0">
<tr style="background-color: #367FA9;">
<td colspan="2" style=" height:10%; color:White; font-weight:bold; font-size:larger" align="center">Expense Details
     <label id="lblExName1" style="color:white;" runat="server" ></label>
</td>
</tr>
     <tr align="center"> <td>
       <label class="hidden" id="lblerrorTravel" runat="server" style="color:red;"></label> </td></tr>
   <%-- <tr align="center">
    <td style="padding:10px">
<label id="" >State :</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
        <asp:DropDownList ID="ddltravelstate" CssClass="form-control select2" AutoPostBack="true" runat="server" TabIndex="2"></asp:DropDownList>
        <label id="" >City :</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
        <asp:DropDownList ID="ddltravelcity" TabIndex="3" CssClass="form-control select2" runat="server"></asp:DropDownList>
</td>

</tr>--%>
     <tr align="center" id="zeroAmtNew" runat="server">
    <td style="padding:10px">
<label id="" >Supporting Attached:</label>
     <asp:CheckBox ID="chktrSuppAttc" runat="server" TabIndex="2" CssClass="" />
        </td><td style="padding:10px">
    <label id="" >Allow To Save With 0 Amount: &nbsp;</label>
        <asp:CheckBox ID="chltrAllowtosave" runat="server" CssClass="" TabIndex="4" />
</td>
</tr>
       <tr align="center">
           <td style="padding:10px">
        <label id="" >Bill Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
              <asp:TextBox ID="TrBillDt" runat="server" CssClass="form-control"  tabindex="6"></asp:TextBox>
                               <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange"  Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox2_CalendarExtender"
                                        TargetControlID="TrBillDt"></ajaxToolkit:CalendarExtender>
</td>
    <td style="padding:10px">
<label id="" >Bill No.:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
    <asp:TextBox ID="TrBillNum" TabIndex="7" CssClass="form-control" MaxLength="10" runat="server"></asp:TextBox>
        </td>
           </tr>
    
     <tr align="center">
    <td style="padding:10px">
<label id="" >Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
            <asp:TextBox ID="TrAmount" MaxLength="10" runat="server" Onblur="javascript:FillTrClaimAmt();" CssClass="form-control numeric text-right" tabindex="10"></asp:TextBox>
        </td>
         <td style="padding:10px">
        <label id="" >Claim Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
       <asp:TextBox ID="TrClaimAmount" MaxLength="10" runat="server" CssClass="form-control numeric text-right" Onblur="javascript:chkTrClaimAmt();" style="background-color:white;" tabindex="12"></asp:TextBox>
</td>
         </tr>
      
    <tr align="center">
    <td style="padding:10px">
<label id="" > From Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
       <asp:TextBox ID="TrDateFrom" runat="server" CssClass="form-control"  tabindex="14"></asp:TextBox>
                               <ajaxToolkit:CalendarExtender ID="CalendarExtender5" CssClass="orange"  Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox5_CalendarExtender"
                                        TargetControlID="TrDateFrom"></ajaxToolkit:CalendarExtender>
        </td>
        <td><label>From Time: </label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label> 
               <asp:DropDownList ID="starttimeddltr" runat="server" CssClass="form-control"></asp:DropDownList>
            <input type="text" maxlength="7"  data-scroll-default="6:00 am" visible="false" placeholder="--Select Time--" Class="form-control"  id="txtTrFromTime" runat="server" autocomplete="off"></td>
        </tr>
    <tr align="center">
        <td style="padding:10px">
    <label id="" >To Date: </label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
        <asp:TextBox ID="TrDateTo" runat="server" CssClass="form-control"  tabindex="14"></asp:TextBox>
                               <ajaxToolkit:CalendarExtender ID="CalendarExtender6" CssClass="orange"  Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox6_CalendarExtender"
                                        TargetControlID="TrDateTo"></ajaxToolkit:CalendarExtender>
</td>
           <td><label>To Time: </label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                  <asp:DropDownList ID="endtimeddltr" runat="server" CssClass="form-control"></asp:DropDownList>
                <input type="text" maxlength="7"  data-scroll-default="6:00 am" visible="false" placeholder="--Select Time--" Class="form-control"  id="txtTrToTime" runat="server" autocomplete="off"></td>
</tr>     
      <tr align="center">
    <td style="padding:10px">
<label id="" >From State:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
        <asp:DropDownList ID="ddltr1state" CssClass="form-control" onChange="BindTr1City()" runat="server" TabIndex="18"></asp:DropDownList>
        </td><td style="padding:10px">
        <label id="" >From City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
        <asp:DropDownList ID="ddltr1city" TabIndex="20" CssClass="form-control" runat="server"></asp:DropDownList>
</td>

      </tr>
 <tr align="center">
    <td style="padding:10px">
<label id="" >To State:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
        <asp:DropDownList ID="ddltr2state" CssClass="form-control" onChange="BindTr2City()" runat="server" TabIndex="22"></asp:DropDownList>
        </td><td style="padding:10px">
        <label id="" >To City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
        <asp:DropDownList ID="ddltr2city" TabIndex="24" CssClass="form-control" runat="server"></asp:DropDownList>
</td>

 </tr>

        <tr align="center">
        <td style="padding:10px" colspan="2">
<label id="" >Travel Mode:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
        <asp:DropDownList ID="ddlTravelMode" CssClass="form-control" runat="server" TabIndex="26"></asp:DropDownList></td>
            
    </tr>
     
    
    <tr align="center" style="display:none;">
    <td style="padding:10px" colspan="2">
        <label id="" >Sync ID:</label>&nbsp;&nbsp;
       <asp:TextBox ID="TrSyncID" MaxLength="10" runat="server" CssClass="form-control" style="background-color:white;" tabindex="27"></asp:TextBox>
</td>
</tr>


       <tr align="center">
    <td style="padding:10px" colspan="2">
        <label id="" >Remark:</label>
       <asp:TextBox ID="TrRemarks" runat="server" height="80px" MaxLength="200" TextMode="MultiLine" CssClass="form-control" style="background-color:white;" tabindex="28"></asp:TextBox>

</td>
</tr>
    <%--<tr align="center" runat="server" id="trparty">
     
    
           <td style="padding:10px">
                 <div style="border:double;">
          Client/Party:
            <asp:DropDownList ID="ddlparty" CssClass="form-control" runat="server"></asp:DropDownList>
         
           Remarks:
            <textarea id="TrPartyremarks" CssClass="form-control" maxlength="200" runat="server" ></textarea>
        </div>
                      </td>
        </tr>--%>
<tr align="center" title="sd">

<td style="padding:20px" colspan="2">
<asp:Button ID="btnTrsave" OnClientClick="return Validate1();" TabIndex="30" runat="server" OnClick ="btnTrsave_Click"  class="btn btn-primary" Text="Save" />
<asp:Button ID="btnTrcancel" class="btn btn-primary" runat="server" Text="Cancel" TabIndex="32"/>
</td>
</tr>
</table>
       <div>
 <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color:red">*</span>)</b>
                            </div>
</asp:Panel>  
                              
                
         <asp:UpdatePanel ID="update" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <ContentTemplate>
       <asp:Button ID="BtnConv" runat="server" style="display:none" />
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="BtnConv" PopupControlID="PnlConv"
CancelControlID="btnConvcancel" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>

<asp:Panel ID="PnlConv" runat="server" ScrollBars="Vertical" BackColor="White" Height="100%" width="" style="display:none">
      
<table width="100%"  height:"100%" cellpadding="6" cellspacing="0">
    <tr style="background-color: #367FA9;">
        <td colspan="2" style=" height:10%; color:White; font-weight:bold; font-size:larger" align="center">Expense Details
            <label id="lblExNameConv" style="color:white;" runat="server" ></label>
        </td>
    </tr>
     <tr align="center">
          <td>
            <label class="hidden" id="lblConverror" runat="server" style="color:red;"></label> 
          </td>
         <td></td>
     </tr>
    <tr align="center">
        <td style="padding:5px 20px;" colspan="2">
            <div class="row">
                <div class="col-md-6">
                <label id="" >State:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
            <asp:DropDownList ID="ddlConvstate" width="100%"  CssClass="form-control" onChange="BindConvCity();" runat="server" TabIndex="2"></asp:DropDownList>
                </div>
                <div class="col-md-6">
                    <label id="" >City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                    <asp:DropDownList ID="ddlConvcity" TabIndex="3"  onChange="BindPartyDistributorBycity();" width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                </div>
            </div>
         </td>
    </tr>
    <tr align="center" id="AmtZero" runat="server">
        <td style="padding:5px 20px;">
            <label id="" >Supporting Attached:</label>
            <asp:CheckBox ID="chkConvSA" runat="server" TabIndex="4" CssClass="" />
        </td>
        <td style="padding:5px 20px;">
            <label id="" >Allow To Save With 0 Amount: &nbsp;</label>
            <asp:CheckBox ID="chkConvAllow0" runat="server"  CssClass="" TabIndex="4" />
        </td>
    </tr>
    <tr align="center">
        <td style="padding:5px 20px;" colspan="2">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                <label id="" >Bill Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
            <asp:TextBox ID="ConvBillDt" runat="server" CssClass="form-control"  width="100%" tabindex="8"></asp:TextBox>
                               <ajaxToolkit:CalendarExtender ID="CalendarExtender7" CssClass="orange"  Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox7_CalendarExtender"
                                        TargetControlID="ConvBillDt"></ajaxToolkit:CalendarExtender>
                </div>
                <div class="col-md-6 col-sm-6">
                    <label id="" >Bill No.:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                    <asp:TextBox ID="ConvBillNum" TabIndex="10" width="100%" CssClass="form-control" MaxLength="10" runat="server"></asp:TextBox>
                </div>
              
            </div> 
       </td>
     </tr>
     <tr align="center" id="trconv" runat="server" style="display:none;">
        <td style="padding:5px 20px;" colspan="2">
            <div class="row">
                <div class="col-md-4 col-sm-4 col-xs-12">
                    <label id="" >Conveyance Mode:</label><label for="requiredFields" style="color: red;">*</label>
                    <asp:DropDownList ID="ddlconvtravelmode" width="100%" onChange="SetPerKmRate();" CssClass="form-control" TabIndex="12" runat="server" ></asp:DropDownList>
                </div>
                <span id="spntrmode" style="display:none" runat="server">
                <div class="col-md-4 col-sm-4 col-xs-6" id="divrateperkm">          
                       <label>Rate Per Km: </label>
                     <asp:TextBox ID="RatePerKm" MaxLength="10" runat="server"  Onblur="javascript:FillRateAmt();"  CssClass="form-control numeric text-right" style="background-color:white;" tabindex="13" Enabled="false"></asp:TextBox>
                </div>
                <div class="col-md-4 col-sm-4 col-xs-6" id="divkmvisited">
                    <label>Km Visited:</label>
                       <asp:TextBox ID="KmVisited" MaxLength="5" runat="server"  Onblur="javascript:FillRateAmt();"  CssClass="form-control numeric text-right" style="background-color:white;" tabindex="14" ></asp:TextBox>
                 </div>
                </span>
            </div>
                </td>
        </tr>
    
    <tr><td style="padding:5px 20px;" colspan="2">
            <div class="row">  <div class="col-md-6 col-sm-6">
                    <label id="" >Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                    <asp:TextBox ID="ConvAmt" MaxLength="10" width="100%" runat="server" Onblur="javascript:FillConvClaimAmt();" CssClass="form-control numeric text-right" tabindex="15"></asp:TextBox>
                </div>
                <div class="col-md-6 col-sm-6">
                    <label id="" >Claim Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                    <asp:TextBox ID="ConvClaimAmt" MaxLength="10" runat="server" Onblur="javascript:chkConvClaimAmt();" CssClass="form-control numeric text-right" style="background-color:white;" tabindex="16"></asp:TextBox>
                </div></div></td></tr>
   
    
    <tr align="center">
        <td style="padding:5px 20px;" colspan="2">
            <label id="" >Remark:</label>
            <asp:TextBox ID="ConvRemarks" runat="server" height="80px" MaxLength="200" TextMode="MultiLine" CssClass="form-control" style="background-color:white;" tabindex="22"></asp:TextBox>
        </td> 
          
    </tr>
    
    <tr>
         <td style="padding:5px 20px;" colspan="2">
            <label id="" style="font-size:medium;" >Client Visited :</label>
            <label id="lblvalidate" style="color:red" runat="server"></label>
         </td>
        
    </tr>

    <tr>
        <td style="padding:5px 20px;" colspan="2">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                <label>Client/Party Name:</label><label for="requiredFields" style="color: red;">*</label>
                <asp:DropDownList ID="ddlparty" CssClass="form-control" onchange="GetPartyVal();" runat="server" Visible="false" tabindex="24"></asp:DropDownList>
                    <%--<asp:TextBox ID="txtDist" runat="server" class="form-control" placeholder="Enter Client/Party" onkeyup="SetContextKey();"></asp:TextBox>--%>
                   <asp:TextBox ID="txtDist" runat="server"  class="form-control" placeholder="Please enter any 3 character of Client/Party of selected city" onkeyup="SetContextKey();"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="true" OnClientItemSelected="ClientPartySelected" 
                     ServiceMethod="SearchDist" ServicePath="ExpenseAdd.aspx"  OnClientShown="ShowOptions" MinimumPrefixLength="3" EnableCaching="true" 
                    TargetControlID="txtDist"></ajaxToolkit:AutoCompleteExtender>
                    <asp:HiddenField ID="hdnparty" runat="server" />

                 
                </div>
                <div class="col-md-6 col-sm-6">
                <label>Remark:</label><br>                   
                    <textarea id="ConvPartyremarks" runat="server" cssclass="form-control" style="width: 100%;" tabindex="26"></textarea>                    
            </div>
           <div>
           <b>Note : Please enter any 3 character of Client/Party of selected city</b>
           </div>

            </div>
        </td>           
    </tr>
      
    <tr>
        <td style="padding:5px 20px;" colspan="2">
           <div class="row">
            <div class="col-md-12 col-sm-12">
            <label id="" >Item Group:</label> <label for="requiredFields" style="color: red;">*</label>
            <input type="button" class="" value="Select All" id="btnselectall"  /> 
            <input type="button" class="" value="De-Select All" id="btnunselectall"  /> 
            <asp:Panel ID="pnlprodgrp" runat="server" Height="150" Width="100%" BorderStyle="Groove" ScrollBars="Both">
            <asp:CheckBoxList ID="chkbxprodgrp" runat="server" TabIndex="28">
            </asp:CheckBoxList>
            </asp:Panel>
            <asp:Button ID="Btnadd" runat="server" style="margin:10px 10px 10px 0;" class="btn btn-primary" OnClientClick="return ValidateProdGrp();" Text="Add" OnClick="Btnadd_Click" />
       
            </div>
            <div class="col-md-12 col-sm-12">
            <div class="table table-responsive">
            <div class="gdvprodgpclass" style="max-height: 201px;overflow: scroll;white-space: nowrap;">
            <asp:GridView ID="gdvprodgrp" class="table" runat="server" OnRowDeleting="gdvprodgrp_RowDeleting" Width="100%" BorderColor="#6699ff" BorderStyle="Groove"  AutoGenerateColumns="false" EmptyDataText="No Record Found" >
            <Columns>
            <asp:BoundField DataField="Client"  HeaderText="Client" />
            <asp:BoundField DataField="ProdGrp"  HeaderText="Item Group" ItemStyle-Wrap="true" />
            <asp:BoundField DataField="remarks"  HeaderText="Remark" ItemStyle-Wrap="true" />
            <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ControlStyle-CssClass="btn btn-primary insidebtnmy" ButtonType="Button" />
            </Columns>
            </asp:GridView>
            </div></div>
        </div>
    </div>
        </td>
    </tr>
 
<tr align="center" title="sd">
<td style="padding:20px" colspan="2">
<asp:Button ID="btnConvSave"  OnClientClick="return Validate2();" TabIndex="30" runat="server" class="btn btn-primary" Text="Save" OnClick="btnConvSave_Click" />
<asp:Button ID="btnConvCancel" class="btn btn-primary" runat="server" Text="Cancel" TabIndex="32"/>
</td>
</tr>
</table>
     <div>
 <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color:red">*</span>)</b>
                            </div>
</asp:Panel>                         
                                                  
<asp:Button ID="Button1" runat="server" style="display:none" />
  
 </ContentTemplate>
                   <%--   <Triggers ><asp:AsyncPostBackTrigger ControlID="btnConvSave" /> </Triggers>--%>
     
                 
           </asp:UpdatePanel>              
                                    <div id="divData" runat="server">
                                        <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Expense List
                       </div>
                        <!-- /.box-header --> 
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Expense</th>
                                                <th>City</th>
                                                <th>Bill No.</th>
                                                <th>Bill Date</th>
                                                <th style="text-align:right;">Bill Amount</th>
                                                <th style="text-align:right;">Claim Amount</th>
                                                <th>Supporting Attached</th>
                                                <th>Edit</th>
                                                <th>Delete</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                           <asp:HiddenField ID="hdfExpDetailId" runat="server" Value='<%#Eval("ExpenseDetailId") %>' />
                                              <asp:HiddenField ID="hdfExpType" runat="server" Value='<%#Eval("ExpenseTypeCode") %>' />
                                        <asp:HiddenField ID="hdfExpName" runat="server" Value='<%#Eval("Name") %>' />
                                         <td><%#Eval("Name") %></td>
                                     <td><%#Eval("AreaName") %></td>
                                            <td><%#Eval("BillNumber") %></td>
                                             <td><%#Eval("BillDate","{0:dd/MMM/yyyy}") %></td>
                                         
                                            <td style="text-align:right;"><%#Eval("BillAmount") %></td>
                                            <td style="text-align:right;"><%#Eval("ClaimAmount") %></td>
                                        <td><%#Eval("IsSupportingAttached1") %></td>
                                        <td><asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Edit</asp:LinkButton></td>
                                         <td><asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click" OnClientClick="ConfirmDelete();">Delete</asp:LinkButton></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <%-- <tr><td></td>
                                                <td style="font-weight: 800;color: rgba(0, 41, 255, 0.76); text-align:right;">Total</td><td></td><td></td>
                                                <td class="" style="font-weight: 800;color: rgba(0, 41, 255, 0.76);text-align:right;"><%#runningBillTotal%>
                                                   </td>
                                                <td style="font-weight: 800;color: rgba(0, 41, 255, 0.76);text-align:right;"><%#runningClaimTotal%>
                                                    </td>
                                                <td></td><td></td><td></td>
                                            </tr>--%>
                                        
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div></div>
                      <span style="float:right;" ><asp:LinkButton runat="server" Text="Submit For Approval" OnClick="lnksubmit_Click" ID="lnksubmit" CssClass="btn btn-primary"  OnClientClick="ConfirmSubmit()"></asp:LinkButton> 
                          </span>
                        </div></div></div>
                                    </div>

                         

                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>

        </div>
             </div></div> 
     

    </section>
       <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({ "bFilter": false, "bPaginate": false });

        });
        function ConfirmSubmit() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to Submit Expenses ?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
        function ConfirmDelete() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to delete this expense ?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
        function Validate() {
           
            $('#<%=lblerrorAddExp.ClientID%>').removeClass('hidden');
            $('#<%=lblerrorAddExp.ClientID%>').val("");
            if ($('#<%=ddlState.ClientID%>').val() == "" || $('#<%=ddlState.ClientID%>').val() == "0") {
                $('#<%=lblerrorAddExp.ClientID%>').html("* Please select State.");
                return false;
            }
            if ($('#<%=ddlCity.ClientID%>').val() == "" || $('#<%=ddlCity.ClientID%>').val() == "0") {
                $('#<%=lblerrorAddExp.ClientID%>').html("* Please select City.");
                return false;
            }
            var contentWrapper = document.getElementById("<%=trStayRelative.ClientID%>");
            if (contentWrapper.getAttribute("style") == null || contentWrapper.getAttribute("style") == "") {
                if ($('#<%=fromdt.ClientID%>').val() == "") {
                    $('#<%=lblerrorAddExp.ClientID%>').html("* Please enter From Date.");
                    return false;
                }
                else
                    if ($('#<%=todt.ClientID%>').val() == "") {
                        $('#<%=lblerrorAddExp.ClientID%>').html("* Please enter To Date.");
                        return false;
                    }
                if ($('#<%=startTimeDDL.ClientID%>').val() == "00:00") {
                    $('#<%=lblerrorAddExp.ClientID%>').html("* Please select From Time.");
                     return false;
                }
                if ($('#<%=endTimeDDL.ClientID%>').val() == "00:00") {
                    $('#<%=lblerrorAddExp.ClientID%>').html("* Please select To Time.");
                    return false;
                }
              <%--  if ($("#ContentPlaceHolder1_basicExample").val() == '') {
                    $('#<%=lblerrorAddExp.ClientID%>').html("* Please select From Time.");
                    return false;
                }--%>
              <%--  if ($("#ContentPlaceHolder1_basicExample1").val() == '') {
                    $('#<%=lblerrorAddExp.ClientID%>').html("* Please select To Time.");
                    return false;
                }--%>
            }
            if ($('#<%=BillNumber.ClientID%>').val() == "") {
                $('#<%=lblerrorAddExp.ClientID%>').html("* Please enter Bill No.");
                return false;
            }
            if ($('#<%=BillDate.ClientID%>').val() == "") {
                $('#<%=lblerrorAddExp.ClientID%>').html("* Please select Bill Date.");
                return false;
            }

            if ($('#<%=Amount.ClientID%>').val() == "") {
                $('#<%=lblerrorAddExp.ClientID%>').html("* Please enter Amount.");
                return false;
            }
            //Added on 22-12-2015
            if ($('#<%=Amount.ClientID%>').val() == "0") {
                $('#<%=lblerrorAddExp.ClientID%>').html("* Please enter Amount greater than 0.");
                 return false;
             }

            <%--var amtWrapper = document.getElementById("<%=AmtZero.ClientID%>");
            var zeroamtWrapper = document.getElementById("<%=zeroAmtNew.ClientID%>");
            if ((amtWrapper.getAttribute("style") == null || amtWrapper.getAttribute("style") == "") && (zeroamtWrapper.getAttribute("style") == null || zeroamtWrapper.getAttribute("style") == "")) {
                if ($('#<%=Amount.ClientID%>').val() == "0") {
                    $('#<%=lblerrorAddExp.ClientID%>').html("* Please enter Amount greater than 0.");
                    return false;
                }
            }--%>
            //End
            if ($('#<%=ClaimAmount.ClientID%>').val() == "") {
                $('#<%=lblerrorAddExp.ClientID%>').html("* Please enter Claim Amount.");
                return false;
            }
            if (parseInt($('#<%=ClaimAmount.ClientID%>').val()) > parseInt($('#<%=Amount.ClientID%>').val())) {

                $('#<%=lblerrorAddExp.ClientID%>').html("Claim Amount should not be greater than Bill Amount.");
                return false;
            }
        }
        function Validate1() {
            $('#<%=lblerrorTravel.ClientID%>').removeClass('hidden');
            $('#<%=lblerrorTravel.ClientID%>').val("");

            if ($('#<%=TrBillNum.ClientID%>').val() == "") {
                $('#<%=lblerrorTravel.ClientID%>').html("* Please enter Bill No.");
                return false;
            }
            if ($('#<%=TrBillDt.ClientID%>').val() == "") {
                $('#<%=lblerrorTravel.ClientID%>').html("* Please select Bill Date.");
                return false;
            }
        

            if ($('#<%=TrAmount.ClientID%>').val() == "") {
                $('#<%=lblerrorTravel.ClientID%>').html("* Please enter Amount.");
                return false;
            }
            //Added on 22-12-2015
            var checked = $('#<%= chltrAllowtosave.ClientID %>').is(':checked');
            if (checked == false) {
                if ($('#<%=TrAmount.ClientID%>').val() == "0") {
                    $('#<%=lblerrorTravel.ClientID%>').html("* Please enter Amount greater than 0.");
                    return false;
                }
            }
            //End
            if ($('#<%=TrClaimAmount.ClientID%>').val() == "") {
                $('#<%=lblerrorTravel.ClientID%>').html("* Please enter Claim Amount.");
                return false;
            }
            if (parseInt($('#<%=TrClaimAmount.ClientID%>').val()) > parseInt($('#<%=TrAmount.ClientID%>').val())) {

                $('#<%=lblerrorTravel.ClientID%>').html("Claim Amount should not be greater than Bill Amount.");
                return false;
            }
            if ($('#<%=TrDateFrom.ClientID%>').val() == "") {
                $('#<%=lblerrorTravel.ClientID%>').html("* Please enter From Date.");
                return false;
            }
            <%--if ($("#ContentPlaceHolder1_txtTrFromTime").val() == '') {
                $('#<%=lblerrorTravel.ClientID%>').html("* Please select From Time.");
                   return false;
               }--%>
            if ($('#<%=starttimeddltr.ClientID%>').val() == "00:00") {
                $('#<%=lblerrorTravel.ClientID%>').html("* Please select From Time.");
                 return false;
             }
            if ($('#<%=TrDateTo.ClientID%>').val() == "") {
                $('#<%=lblerrorTravel.ClientID%>').html("* Please enter To Date.");
                return false;
            }
            if ($('#<%=endtimeddltr.ClientID%>').val() == "00:00") {
                $('#<%=lblerrorTravel.ClientID%>').html("* Please select To Time.");
                  return false;
              }
               <%-- if ($("#ContentPlaceHolder1_txtTrToTime").val() == '') {
                    $('#<%=lblerrorTravel.ClientID%>').html("* Please select To Time.");
                return false;
            }--%>
            if ($('#<%=ddltr1state.ClientID%>').val() == "" || $('#<%=ddltr1state.ClientID%>').val() == "0") {
                $('#<%=lblerrorTravel.ClientID%>').html("* Please select From State.");
                return false;
            }

            if ($('#<%=ddltr1city.ClientID%>').val() == "" || $('#<%=ddltr1city.ClientID%>').val() == "0") {
                $('#<%=lblerrorTravel.ClientID%>').html("* Please select From City.");
                return false;
            }
            if ($('#<%=ddltr2state.ClientID%>').val() == "" || $('#<%=ddltr2state.ClientID%>').val() == "0") {
                $('#<%=lblerrorTravel.ClientID%>').html("* Please select To State.");
                return false;
            }
            if ($('#<%=ddltr2city.ClientID%>').val() == "" || $('#<%=ddltr2city.ClientID%>').val() == "0") {
                $('#<%=lblerrorTravel.ClientID%>').html("* Please select To City.");
                return false;
            }
            if ($('#<%=ddlTravelMode.ClientID%>').val() == "") {
                $('#<%=lblerrorTravel.ClientID%>').html("* Please select Travel Mode.");
                return false;
            }
        }
        function Validate2() {
            $('#<%=lblConverror.ClientID%>').removeClass('hidden');
            $('#<%=lblConverror.ClientID%>').val("");
            if ($('#<%=ddlConvstate.ClientID%>').val() == "" || $('#<%=ddlConvstate.ClientID%>').val() == "0") {
                $('#<%=lblConverror.ClientID%>').html("* Please select State.");
                return false;
            }
            
            if ($('#<%=ddlConvcity.ClientID%>').val() == "" || $('#<%=ddlConvcity.ClientID%>').val() == "0") {
                $('#<%=lblConverror.ClientID%>').html("* Please select City.");
                return false;
            }
            if ($('#<%=ConvBillDt.ClientID%>').val() == "") {
                $('#<%=lblConverror.ClientID%>').html("* Please select Bill Date.");
                return false;
            }
            if ($('#<%=ConvBillNum.ClientID%>').val() == "") {
                $('#<%=lblConverror.ClientID%>').html("* Please enter Bill No.");
                return false;
            }
            var contentWrapper1 = document.getElementById("<%=trconv.ClientID%>");
            if (contentWrapper1.getAttribute("style") == null || contentWrapper1.getAttribute("style") == "") {
                if ($('#<%=ddlconvtravelmode.ClientID%>').val() == "" || $('#<%=ddlconvtravelmode.ClientID%>').val() == "0") {
                    $('#<%=lblConverror.ClientID%>').html("* Please select a Conveyance Mode.");
                    return false;
                }
            }
            if ($('#<%=ConvAmt.ClientID%>').val() == "") {
                $('#<%=lblConverror.ClientID%>').html("* Please enter Amount.");
                return false;
            }
            //Added on 22-12-2015
            var checked = $('#<%= chkConvAllow0.ClientID %>').is(':checked');
            if (checked == false) {
                if ($('#<%=ConvAmt.ClientID%>').val() == "0") {
                    $('#<%=lblConverror.ClientID%>').html("* Please enter Amount greater than 0.");
                    return false;
                }
            }
            //End
            if ($('#<%=ConvClaimAmt.ClientID%>').val() == "") {
                $('#<%=lblConverror.ClientID%>').html("* Please enter Claim Amount.");
                return false;
            }
            if (parseInt($('#<%=ConvClaimAmt.ClientID%>').val()) > parseInt($('#<%=ConvAmt.ClientID%>').val())) {
                $('#<%=lblConverror.ClientID%>').html("Claim Amount should not be greater than Bill Amount.");
               return false;
           }
           

            //Added on 19-12-2015
            <%--if ($('#<%=ddlparty.ClientID%>').val() == "" || $('#<%=ddlparty.ClientID%>').val() == "0") {
                $('#<%=lblConverror.ClientID%>').html("* Please select a Client.");
                return false;
            }--%>
            //End
        }
    </script>
    <script type="text/javascript">
        function load1() {

            $('#btnselectall').click(function () {

                $("[id*=chkbxprodgrp] input").prop("checked", "checked");
            });
            $('#btnunselectall').click(function () {
                $("[id*=chkbxprodgrp] input").removeAttr("checked");
            });



            $('#<%=chkallowtosave.ClientID%>').change(function () {
                if (this.checked) {
                    $('#<%=Amount.ClientID%>').val("0");
                    $('#<%=ClaimAmount.ClientID%>').val("0");
                    $('#<%=Amount.ClientID%>').attr("disabled", "disabled");
                    $('#<%=ClaimAmount.ClientID%>').attr("disabled", "disabled");
                }
                else {
                    $('#<%=Amount.ClientID%>').removeAttr("disabled");
                    $('#<%=ClaimAmount.ClientID%>').removeAttr("disabled");
                }
            });
            $('#<%=chkConvAllow0.ClientID%>').change(function () {
                if (this.checked) {
                    $('#<%=ConvAmt.ClientID%>').val("0");
                    $('#<%=ConvClaimAmt.ClientID%>').val("0");
                    $('#<%=KmVisited.ClientID%>').val('');
                    $('#<%=ConvAmt.ClientID%>').attr("disabled", "disabled");
                    $('#<%=ConvClaimAmt.ClientID%>').attr("disabled", "disabled");
                    $('#<%=KmVisited.ClientID%>').attr("disabled", "disabled");
                }
                else {
                    if ($('#<%=ddlexpensetype.ClientID%>').find("option:selected").text() != "CONVEYANCE") {
                        $('#<%=ConvAmt.ClientID%>').removeAttr("disabled");
                    $('#<%=ConvClaimAmt.ClientID%>').removeAttr("disabled");
                }
                   <%-- $('#<%=ConvAmt.ClientID%>').val("<%= ConvAmtVal%>");
                    $('#<%=ConvClaimAmt.ClientID%>').val("<%= ConvAmtVal%>");--%>
             <%--       $('#<%=ConvAmt.ClientID%>').removeAttr("disabled");
                    $('#<%=ConvClaimAmt.ClientID%>').removeAttr("disabled");--%>
                    $('#<%=KmVisited.ClientID%>').removeAttr("disabled");
                    GetAmtAllowedByCity();
                }
                });
            $('#<%=chltrAllowtosave.ClientID%>').change(function () {
                if (this.checked) {
                    $('#<%=TrAmount.ClientID%>').val("0");
                    $('#<%=TrClaimAmount.ClientID%>').val("0");
                    $('#<%=TrAmount.ClientID%>').attr("disabled", "disabled");
                    $('#<%=TrClaimAmount.ClientID%>').attr("disabled", "disabled");
                }
                else {
                    $('#<%=TrAmount.ClientID%>').removeAttr("disabled");
                    $('#<%=TrClaimAmount.ClientID%>').removeAttr("disabled");
                }
            });

        }

        $(window).load(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(load1);

        });


    </script>
     <script type="text/javascript">
       function ValidateProdGrp() {
             $('#<%=lblConverror.ClientID%>').removeClass('hidden');
             if ($('#<%=hdnparty.ClientID%>').val() == "" || $('#<%=hdnparty.ClientID%>').val() == "0") {             
                 $('#<%=lblConverror.ClientID%>').html("* Please select correct Client/Party Name.");
                 return false;
             }

         }       
         $(function () {
             $('#btnselectall').click(function () {

                 $("[id*=chkbxprodgrp] input").prop("checked", "checked");
             });
             $('#btnunselectall').click(function () {
                 $("[id*=chkbxprodgrp] input").removeAttr("checked");
             });

         });

         function chkClaimAmt() {

             $('#<%=lblerrorAddExp.ClientID%>').removeClass('hidden');
             $('#<%=lblerrorAddExp.ClientID%>').val("");
             if ($('#<%=Amount.ClientID%>').val() != "" && $('#<%=ClaimAmount.ClientID%>').val() != "") {
                 if (parseInt($('#<%=ClaimAmount.ClientID%>').val()) > parseInt($('#<%=Amount.ClientID%>').val())) {

                     $('#<%=lblerrorAddExp.ClientID%>').html("Claim Amount should not be greater than Bill Amount.");
                     return false;
                 }
                 else {
                     $('#<%=lblerrorAddExp.ClientID%>').html("");
                     $('#<%=lblerrorAddExp.ClientID%>').addClass('hidden');
                 }
             }
         }
         function chkTrClaimAmt() {

             $('#<%=lblerrorTravel.ClientID%>').removeClass('hidden');
             $('#<%=lblerrorTravel.ClientID%>').val("");
             if ($('#<%=TrAmount.ClientID%>').val() != "" && $('#<%=TrClaimAmount.ClientID%>').val() != "") {
                 if (parseInt($('#<%=TrClaimAmount.ClientID%>').val()) > parseInt($('#<%=TrAmount.ClientID%>').val())) {

                     $('#<%=lblerrorTravel.ClientID%>').html("Claim Amount should not be greater than Bill Amount.");
                     return false;
                 }
                 else {
                     $('#<%=lblerrorTravel.ClientID%>').html("");
                     $('#<%=lblerrorTravel.ClientID%>').addClass('hidden');
                 }
             }
         }
         function chkConvClaimAmt() {

             $('#<%=lblConverror.ClientID%>').removeClass('hidden');
             $('#<%=lblConverror.ClientID%>').val("");
             if ($('#<%=ConvAmt.ClientID%>').val() != "" && $('#<%=ConvClaimAmt.ClientID%>').val() != "") {
                 if (parseInt($('#<%=ConvClaimAmt.ClientID%>').val()) > parseInt($('#<%=ConvAmt.ClientID%>').val())) {

                     $('#<%=lblConverror.ClientID%>').html("Claim Amount should not be greater than Bill Amount.");
                     return false;
                 }
                 else {
                     $('#<%=lblConverror.ClientID%>').html("");
                     $('#<%=lblConverror.ClientID%>').addClass('hidden');
                 }
             }
         }
         function FillConvClaimAmt() {
             $('#<%=ConvClaimAmt.ClientID%>').val($('#<%=ConvAmt.ClientID%>').val());
         }
         function FillTrClaimAmt() {
             $('#<%=TrClaimAmount.ClientID%>').val($('#<%=TrAmount.ClientID%>').val());
         }
         function FillClaimAmt() {
             $('#<%=ClaimAmount.ClientID%>').val($('#<%=Amount.ClientID%>').val());
         }
         function FillRateAmt() {
             if ($('#<%=KmVisited.ClientID%>').val() != "") {
              $('#<%=ConvClaimAmt.ClientID%>').val(Math.round(parseFloat($('#<%=RatePerKm.ClientID%>').val()).toFixed(4) * parseFloat($('#<%=KmVisited.ClientID%>').val()).toFixed(4)));
                 $('#<%=ConvAmt.ClientID%>').val(Math.round(parseFloat($('#<%=RatePerKm.ClientID%>').val()).toFixed(4) * parseFloat($('#<%=KmVisited.ClientID%>').val()).toFixed(4)));
                 
             } else {
                 $('#<%=ConvClaimAmt.ClientID%>').val("0"); $('#<%=ConvAmt.ClientID%>').val("0");
             }
         }
</script>
      <script type="text/javascript">
          $(document).ready(function () {
           
              $('#<%=chkallowtosave.ClientID%>').change(function () {
                  if (this.checked) {
                      $('#<%=Amount.ClientID%>').val("0");
                      $('#<%=ClaimAmount.ClientID%>').val("0");
                      $('#<%=Amount.ClientID%>').attr("disabled", "disabled");
                      $('#<%=ClaimAmount.ClientID%>').attr("disabled", "disabled");
                  }
                  else {
                      $('#<%=Amount.ClientID%>').removeAttr("disabled");
                      $('#<%=ClaimAmount.ClientID%>').removeAttr("disabled");
                  }
              });
              $('#<%=chkConvAllow0.ClientID%>').change(function () {
                  if (this.checked) {
                      $('#<%=ConvAmt.ClientID%>').val("0");
                      $('#<%=ConvClaimAmt.ClientID%>').val("0");
                      $('#<%=ConvAmt.ClientID%>').attr("disabled", "disabled");
                      $('#<%=ConvClaimAmt.ClientID%>').attr("disabled", "disabled");
                  }
                  else {
                      if ($('#<%=ddlexpensetype.ClientID%>').find("option:selected").text() != "CONVEYANCE") {
                          $('#<%=ConvAmt.ClientID%>').removeAttr("disabled");
                          $('#<%=ConvClaimAmt.ClientID%>').removeAttr("disabled");
                      }
                    
                  }
              });
              $('#<%=chltrAllowtosave.ClientID%>').change(function () {
                  if (this.checked) {
                      $('#<%=TrAmount.ClientID%>').val("0");
                      $('#<%=TrClaimAmount.ClientID%>').val("0");
                      $('#<%=TrAmount.ClientID%>').attr("disabled", "disabled");
                      $('#<%=TrClaimAmount.ClientID%>').attr("disabled", "disabled");
                  }
                  else {
                      $('#<%=TrAmount.ClientID%>').removeAttr("disabled");
                      $('#<%=TrClaimAmount.ClientID%>').removeAttr("disabled");
                  }
              });
          });
    </script>
</asp:Content>
