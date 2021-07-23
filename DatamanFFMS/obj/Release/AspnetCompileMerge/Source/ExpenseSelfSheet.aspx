<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ExpenseSelfSheet.aspx.cs" Inherits="AstralFFMS.ExpenseSelfSheet" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <link type="text/css" rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            $('#ContentPlaceHolder1_basicExample').timepicker();
        });
    </script>
    <style type="text/css">
        .insidebtnmy {
            padding: 0 4px;
        }
          .aVis {
            display: none;
        }
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
            z-index: 10000;
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
         $(document).ready(function () {
             // Prototype to assign HTML to the dialog title bar.

             $.widget('ui.dialog', jQuery.extend({}, jQuery.ui.dialog.prototype, {
                 _title: function (titleBar) {
                     titleBar.html(this.options.title || '&#160;');
                 }
             }));
             // ImageButton Click Event.
             //$('#example1 .imgButton').click(function () {
             //    // Get the Current Row and its values.
             //    alert('ok');
             //    return false;
             //});
             $('#example1 .imgButton').click(function () {

                 // Get the Current Row and its values.
                 var currentRow = $(this).parents("tr");

                 var expname = currentRow.find("span[id*='lblname']").text();
                 var city = currentRow.find("span[id*='myid']").text();
                 var billdate = currentRow.find("span[id*='lblbilldt']").text();
                 var billNumber = currentRow.find("span[id*='lblbillnum']").text();
                 var billAmt = currentRow.find("span[id*='lblbillAmt']").text();
                 var claimAmt = currentRow.find("span[id*='lblclaimAmt']").text();
                 var SA = currentRow.find("span[id*='lblSA']").text();
                 var SWR = currentRow.find("span[id*='lblSWR']").text();
                 var fromstate = currentRow.find("span[id*='lblfromstate1']").text();
                 var tostate = currentRow.find("span[id*='lbltostate1']").text();
                 var tocity = currentRow.find("span[id*='lbltocity']").text();
                 var remark = currentRow.find("span[id*='lblremarks']").text();
                 var ExptypeCode = currentRow.find("span[id*='lblexpcode']").text();
                 var rateperkm = currentRow.find("span[id*='lblrateperkm']").text();
                 var kmvisited = currentRow.find("span[id*='lblkmvisit']").text();
                 var fromdate = currentRow.find("span[id*='lblfromdate']").text();
                 var todate = currentRow.find("span[id*='lbltodate']").text();
                 var travelmode = currentRow.find("span[id*='lbltravelmode']").text();
                 var fromtime = currentRow.find("span[id*='lblfromtime']").text();
                 var totime = currentRow.find("span[id*='lbltotime']").text();
                 var ExpdetailId = currentRow.find("span[id*='lblexpdetId']").text();
                 // var allowsave0 = currentRow.find("span[id*='lblallowtosave0']").text();

                 // Populate labels inside the dailog.
                 //for rest popups
                 $("#lblcity").text(city);
                 $("#lblbilldt").text(billdate);
                 $("#lblbillnum").text(billNumber);
                 $("#lblAmount").text(billAmt);
                 $("#lblclaimAmt").text(claimAmt);
                 $("#lblsuppatt").text(SA);
                 $("#lblstaywithrelative").text(SWR);
                 $("#lblstate").text(fromstate);
                 $("#lblremarks").text(remark);
                 $("#lblfromdate").text(fromdate);
                 $("#lblfromtime").text(fromtime);
                 $("#lbltodate").text(todate);
                 $("#lbltotime1").text(totime);

                 //for conv-travel popups
                 $("#lblconvcity").text(city);
                 $("#lblconvbilldt").text(billdate);
                 $("#lblconvbillnum").text(billNumber);
                 $("#lblconvAmount").text(billAmt);
                 $("#lblconvclaimAmt").text(claimAmt);
                 $("#lblconvsuppatt").text(SA);
                 $("#lblconvstate").text(fromstate);
                 $("#lblconvremarks").text(remark);
                 $("#lblconvMode").text(travelmode);
                 $("#lblconvrateperkm").text(rateperkm);
                 $("#lblconvkmvisited").text(kmvisited);
                 //for travel popups
                 $("#lblconvcity").text(city);
                 $("#lbltrbilldt").text(billdate);
                 $("#lbltrbillnum").text(billNumber);
                 $("#lbltrAmount").text(billAmt);
                 $("#lbltrclaimAmt").text(claimAmt);
                 $("#lbltrsuppatt").text(SA);
                 $("#lblfrmdt").text(fromdate);
                 $("#lblfrmtime").text(fromtime);
                 $("#lbltodt").text(todate);
                 $("#lbltotime").text(totime);
                 $("#lbltrstatefrom").text(fromstate);
                 $("#lbltrcityfrom").text(city);
                 $("#lbltrstateto").text(tostate);
                 $("#lbltrcityto").text(tocity);
                 $("#lbltrmode").text(travelmode);
                 //$("#lblstate").text(SWR);

                 //$.ajax({
                 //    type: "POST",
                 //    contentType: "application/json; charset=utf-8",
                 //    url: "ExpenseApprovalDetails.aspx/BindDatatable",
                 //    data: '{ExpenseDetailId: "' + ExpdetailId + '" }',
                 //    dataType: "json",
                 //    success: function (data) {

                 //        $("#gvDetailstable").html('');

                 //        if (data.d.length > 0) {
                 //     $("#gvDetailstable").append("<thead><tr><th>Party Name</th><th>Product Group</th><th>Remarks</th></tr></thead>");
                 //     $("#gvDetailstable").append("<tbody>");
                 //            for (var i = 0; i < data.d.length; i++) {
                 //                $("#gvDetailstable").append("<tr class='partyclass'><td>" +
                 //                data.d[i].partyname + "</td> <td>" +
                 //                data.d[i].productgroup + "</td> <td>" +
                 //                data.d[i].remarks + "</td> </tr>");
                 //            }
                 //            $("#gvDetailstable").append("</tbody>");

                 //        }
                 //    },
                 //    error: function (result) {
                 //        // alert("Error login");

                 //    }
                 //});


                 // Open the dialog.

                 if (ExptypeCode.toUpperCase() == "CONVEYANCETRAVEL" || ExptypeCode.toUpperCase() == "CONVEYANCE") {
                     if (ExptypeCode.toUpperCase() == "CONVEYANCE") {
                         $("#tr_convmode").hide();
                         $("#tr_kmrate").hide();

                     } else {
                         $("#tr_convmode").show();
                         if (parseFloat(rateperkm) > 0) { $("#tr_kmrate").show(); } else { $("#tr_kmrate").hide(); }


                     }

                     $("#popupdivconvtr").dialog({
                         title: "Details of <em>" + expname + "</em>",
                         width: 600,
                         height: 300,
                         modal: true,
                         closeText: ''
                         //buttons: {
                         //    Close: function () {
                         //        $(this).dialog('close');
                         //    }
                         //}
                     });
                 }
                 else if (ExptypeCode.toUpperCase() == "TRAVEL") {
                     $("#popupdivtravel").dialog({
                         title: "Details of <em>" + expname + "</em>",
                         width: 600,
                         height: 400,
                         modal: true,
                         closeText: ''
                         //buttons: {
                         //    Close: function () {
                         //        $(this).dialog('close');
                         //    }
                         //}
                     });
                 }
                 else {
                     if (ExptypeCode.toUpperCase() == "LODGING") {
                         $("#tr_swr").show();
                         $("#tr_fromdate").show();
                         $("#tr_fromtime").show();
                         $("#tr_todate").show();
                         $("#tr_totime").show();
                     }
                     else {
                         $("#tr_swr").hide();
                         $("#tr_fromdate").hide();
                         $("#tr_fromtime").hide();
                         $("#tr_todate").hide();
                         $("#tr_totime").hide();
                     }

                     $("#popupdiv").dialog({
                         title: "Details of <em>" + expname + "</em>",
                         width: 600,
                         height: 300,
                         modal: true,
                         closeText: ''
                         //buttons: {
                         //    Close: function () {
                         //        $(this).dialog('close');
                         //    }
                         //}
                     });
                 }
                 return false;
             });

         });
    </script>
    
     <div id="popupdiv" style="display: none">
        <table>
            <tbody>
                <tr>
                    <td>
                        <label class="">State:</label>

                    </td>
                    <td>
                        <label id="lblstate"></label>
                    </td>

                </tr>
                <tr>
                    <td>
                        <label>City:</label>
                    </td>
                    <td>
                        <label id="lblcity"></label>

                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">Supporting Attached:</label>
                    </td>
                    <td>
                        <label id="lblsuppatt"></label>
                    </td>
                </tr>
                <tr id="tr_swr">
                    <td>
                        <label class="">Stay with relative:</label>
                    </td>
                    <td>
                        <label id="lblstaywithrelative"></label>
                    </td>
                </tr>
                  <tr id="tr_fromdate">
                    <td>
                        <label class="">From Date:</label>
                    </td>
                    <td>
                        <label id="lblfromdate"></label>
                    </td>
                </tr>
                  <tr id="tr_fromtime">
                    <td>
                        <label class="">From Time:</label>
                    </td>
                    <td>
                        <label id="lblfromtime"></label>
                    </td>
                </tr>
                 <tr id="tr_todate">
                    <td>
                        <label class="">To Date:</label>
                    </td>
                    <td>
                        <label id="lbltodate"></label>
                    </td>
                </tr>
                <tr id="tr_totime">
                    <td>
                        <label class="">To Time:</label>
                    </td>
                    <td>
                        <label id="lbltotime1"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">Bill Date:</label>
                    </td>
                    <td>
                        <label id="lblbilldt"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">Bill No.:</label>
                    </td>
                    <td>
                        <label id="lblbillnum"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">Amount:</label>
                    </td>
                    <td>
                        <label id="lblAmount"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">Claim Amount:</label>
                    </td>
                    <td>
                        <label id="lblclaimAmt"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Remarks:</label>
                    </td>
                    <td>
                        <label id="lblremarks"></label>
                    </td>

                </tr>

            </tbody>
        </table>
    </div>

    <div id="popupdivconvtr" style="display: none;">
        <table style="width: 100%;">
            <tbody>
                <tr>
                    <td>
                        <label class="">State:</label>
                    </td>
                    <td>
                        <label id="lblconvstate"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>City:</label>
                    </td>
                    <td>
                        <label id="lblconvcity"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">Supporting Attached:</label>
                    </td>
                    <td>
                        <label id="lblconvsuppatt"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">Bill Date:</label>
                    </td>
                    <td>
                        <label id="lblconvbilldt"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">Bill No.:</label>
                    </td>
                    <td>
                        <label id="lblconvbillnum"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">Amount:</label>
                    </td>
                    <td>
                        <label id="lblconvAmount"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">Claim Amount:</label>
                    </td>
                    <td>
                        <label id="lblconvclaimAmt"></label>
                    </td>
                </tr>
                <tr id="tr_convmode">
                    <td>
                        <label class="">Conveyance Mode:</label>
                    </td>
                    <td>
                        <label id="lblconvMode"></label>

                    </td>
                </tr>
                <tr id="tr_kmrate">
                    <td>
                        <label class="">Rate per Km:</label>
                        <label id="lblconvrateperkm"></label>

                    </td>
                    <td>
                        <label class="">Km visited:</label>
                        <label id="lblconvkmvisited"></label>

                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Remarks:</label>
                    </td>
                    <td>
                        <label id="lblconvremarks"></label>
                    </td>

                </tr>
                <tr>
                    <td></td>
                </tr>
            </tbody>
        </table>
    </div>

    <div id="popupdivtravel" style="display: none">
        <table>
            <tbody>
                <tr>
                    <td>
                        <label class="">Supporting Attached:</label>
                    </td>
                    <td>
                        <label id="lbltrsuppatt"></label>
                    </td>
                </tr>

                <tr>
                    <td>
                        <label class="">Bill Date:</label>
                    </td>
                    <td>
                        <label id="lbltrbilldt"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">Bill No.:</label>
                    </td>
                    <td>
                        <label id="lbltrbillnum"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">Amount:</label>
                    </td>
                    <td>
                        <label id="lbltrAmount"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">Claim Amount:</label>
                    </td>
                    <td>
                        <label id="lbltrclaimAmt"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">From Date:</label>
                    </td>
                    <td>
                        <label id="lblfrmdt"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">From Time:</label>
                    </td>
                    <td>
                        <label id="lblfrmtime"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">To Date:</label>
                    </td>
                    <td>
                        <label id="lbltodt"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">To Time:</label>
                    </td>
                    <td>
                        <label id="lbltotime"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">From State:</label>
                    </td>
                    <td>
                        <label id="lbltrstatefrom"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>From City:</label>
                    </td>
                    <td>
                        <label id="lbltrcityfrom"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="">To State:</label>
                    </td>
                    <td>
                        <label id="lbltrstateto"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>To City:</label>
                    </td>
                    <td>
                        <label id="lbltrcityto"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Travel mode:</label>
                    </td>
                    <td>
                        <label id="lbltrmode"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Remarks:</label>
                    </td>
                    <td>
                        <label id="lbltrremarks"></label>
                    </td>
                </tr>

            </tbody>
        </table>
    </div>

    <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" ForeColor="white" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Self Expense Summary</h3>
                        </div>

                        <div class="box-header">
                            <div>
                                <div id="divData" runat="server">
                                    <div class="box-body" id="rptmain" runat="server">
                                        <div class="row">
                                            <div class="col-xs-12">

                                                <div class="box">
                                                    <div class="box-header">
                                                        <div class="row">
                                                            <div class="col-md-3 col-sm-6 ">
                                                                <label>Expense Group:</label>
                                                                <asp:DropDownList ID="ddlexpGrp" runat="server" CssClass="form-control"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3 col-sm-6 ">
                                                                <label>Expense:</label>
                                                                <asp:DropDownList ID="ddlexpType" runat="server" CssClass="form-control"></asp:DropDownList>
                                                            </div>
                                                            </div>
                                                             <div class="row">
                                                            <div class="col-md-3 col-sm-6 ">
                                                                <label>From Date:</label>
                                                                <asp:TextBox ID="DateFrom" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox3_CalendarExtender"
                                                                    TargetControlID="DateFrom"></ajaxToolkit:CalendarExtender>

                                                            </div>
                                                            <div class="col-md-3 col-sm-6">
                                                                <label>To Date:</label>
                                                                <asp:TextBox ID="DateTo" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox4_CalendarExtender"
                                                                    TargetControlID="DateTo"></ajaxToolkit:CalendarExtender>
                                                            </div>
                                                            <div class="col-md-3 col-sm-6" style="margin-top: 18px;">
                                                                <asp:Button ID="btnshow" runat="server" CssClass="btn btn-primary" OnClick="btnshow_Click" Text="Show" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="box-header">
                                                        <h3 class="box-title">
                                                        Expense List
                                                    </div>
                                                    <!-- /.box-header -->
                                                    <div class="box-body table-responsive">
                                                        <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <table id="example1" class="table table-bordered table-striped">
                                                                    <thead>
                                                                        <tr>
                                                                            <th>Details</th>
                                                                            <th>Expense Group</th>
                                                                            <th>Expense</th>
                                                                            <th>City</th>                                                                            
                                                                            <th>Bill No.</th>
                                                                            <th>Bill Date</th>
                                                                            <th style="text-align: right;">Bill Amount</th>
                                                                            <th style="text-align: right;">Claim Amount</th>
                                                                            <th style="text-align: right;">Approved Amount</th>
                                                                            <th>Status</th>
                                                                            <th>Supporting Attached</th>
                                                                            <th></th>
                                                                            <th></th> 
                                                                             <th class="aVis">ClmAmt</th>                                                                                
                                                                            <th class="aVis">ApprAmt</th>                                                                            
                                                                        </tr>
                                                                         <tr>
                                                                            <th></th>
                                                        <th></th>
                                                        <th></th>
                                                        <th></th>
                                                        <th></th>
                                                        <th></th>
                                                        <th></th>
                                                        <th></th>  
                                                                             <th></th>
                                                                             <th></th>
                                                                             <th></th>
                                                                             <th></th>
                                                                             <th></th>
                                                                             <th></th>
                                                                             <th></th>
                                                                             <th></th>                                                                                                                                                                                                                              
                                                                        </tr>
                                                                    </thead>
                                                                    <tfoot>
                                                                        <tr>
                                                                            <th colspan="7" style="text-align: right">Total:</th>
                                                                            <th style="text-align: right"></th>
                                                                            <th style="text-align: right"></th>
                                                                        </tr>
                                                                    </tfoot>
                                                                    <tbody>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <asp:HiddenField ID="hdfExpGroupId" runat="server" Value='<%#Eval("ExpenseGroupId") %>' />
                                                                    <asp:HiddenField ID="hdfExpDetailId" runat="server" Value='<%#Eval("ExpenseDetailId") %>' />
                                                                    <asp:HiddenField ID="hdfExpType" runat="server" Value='<%#Eval("ExpenseTypeCode") %>' />
                                                                    <td><asp:ImageButton ID="imbShowDetails" class="imgButton" Height="30" runat="server" ImageUrl="~/img/popup.png" /> </td>
                                                                    <td><%#Eval("GroupName") %></td>                                                                   
                                                                    <td><asp:Label ID="lblname" runat="server" Text='<%#Eval("Name") %>'></asp:Label></td>                                                                   
                                                                    <td> <asp:Label ID="myid" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FromcityName")%>'></asp:Label></td>                                                                    

                                                                   <%-- <td><%#Eval("BillNumber") %></td>
                                                                    <td><%#Eval("BillDate","{0:dd/MMM/yyyy}") %></td>--%>
                                                                    <%--<td style="text-align: right;"><%#Eval("BillAmount") %></td>--%>
                                                                    <%--<td style="text-align: right;"><%#Eval("ClaimAmount") %></td>--%>
                                                                      
                                                                    <td><asp:Label ID="lblbillnum" runat="server" Text='<%#Eval("BillNumber") %>'></asp:Label></td>
                                                                    <td><asp:Label ID="lblbilldt" runat="server" Text='<%#Eval("BillDate") %>'></asp:Label></td> 
                                                                    <td><asp:Label ID="lblbillAmt" runat="server" Text='<%#Eval("BillAmount") %>' Style="text-align: right;"></asp:Label></td>
                                                                    <td class="aVis" style="text-align: right;"><%#Eval("ClaimAmount") %></td>
                                                                    <td style="text-align: right;"><asp:Label ID="lblclaimAmt" Text='<%#Eval("ClaimAmount") %>' runat="server"></asp:Label></td>
                                                                                                                                                                                                                                                              
                                                                    <td style="text-align: right;">
                                                                        <asp:Label ID="lblappramt" Text='<%#Eval("ApprovedAmount") %>' runat="server"></asp:Label>
                                                                    </td>
                                                                    <td><%#Eval("Status") %></td>
                                                                    <td><%#Eval("IsSupportingAttached1") %></td>
                                                                    <td style="width: 10px;">
                                                                        <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("ExpenseSheetSummary.aspx?ExpenseGroupId={0}&Flag={1}", Eval("ExpenseGroupId"),("SE")) %>'
                                                                            Text="R1" ToolTip="R1" Target="_blank" /></td>
                                                                    <td style="width: 10px;">
                                                                        <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("ExpenseSheetSummary1.aspx?ExpenseGroupId={0}&Flag={1}", Eval("ExpenseGroupId"),("SE")) %>'
                                                                            Text="R2" ToolTip="R2" Target="_blank" /></td>                                                                   
                                                                     <td class="aVis"><%# Eval("ApprovedAmount")%></td>

                                                                      <td>
                                                    <asp:Label ID="lblfromstate1" runat="server" CssClass="aVis" Text='<%#Eval("fromstate") %>'></asp:Label>
                                                    <asp:Label ID="lbltostate1" runat="server" CssClass="aVis" Text='<%#Eval("tostate") %>'></asp:Label>
                                                    <asp:Label ID="lbltocity" runat="server" CssClass="aVis" Text='<%#Eval("TocItyName") %>'></asp:Label>
                                                    <asp:Label ID="lblremarks" runat="server" CssClass="aVis" Text='<%#Eval("remarks") %>'></asp:Label>
                                                    <asp:Label ID="lblfromtime" runat="server" CssClass="aVis" Text='<%#Eval("FromTime") %>'></asp:Label>
                                                    <asp:Label ID="lbltotime" runat="server" CssClass="aVis" Text='<%#Eval("ToTime") %>'></asp:Label>
                                                    <asp:Label ID="lblexpcode" runat="server" CssClass="aVis" Text='<%#Eval("expensetypecode") %>'></asp:Label>
                                                    <asp:Label ID="lblrateperkm" runat="server" CssClass="aVis" Text='<%#Eval("PreKilometerRate") %>'></asp:Label>
                                                    <asp:Label ID="lblkmvisit" runat="server" CssClass="aVis" Text='<%#Eval("KMVisit") %>'></asp:Label>
                                                    <asp:Label ID="lblfromdate" runat="server" CssClass="aVis" Text='<%#Eval("fromdate") %>'></asp:Label>
                                                    <asp:Label ID="lbltodate" runat="server" CssClass="aVis" Text='<%#Eval("todate") %>'></asp:Label>
                                                    <asp:Label ID="lbltravelmode" runat="server" CssClass="aVis" Text='<%#Eval("Travelconvmode") %>'></asp:Label>
                                                    <asp:Label ID="lblSA" runat="server" CssClass="aVis" Text='<%#Eval("IsSupportingAttached1") %>'></asp:Label>
                                                    <asp:Label ID="lblSWR" runat="server" CssClass="aVis" Text='<%#Eval("StayWithRelative1") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>                                                             

                                                                </tbody>     </table>       
                                                            </FooterTemplate>

                                                        </asp:Repeater>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <!-- /.box -->

                        </div>
                        <!-- /.col -->
                    </div>

                </div>
            </div>
        </div>


    </section>
     <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        //$(function () {
        //    $("#example1").DataTable();
        //});
        $(function () {
            $('#example1').dataTable({
                "order": [[0, "desc"]],               
                "footerCallback": function (tfoot, data, start, end, display) {
                    //$(tfoot).find('th').eq(0).html("Starting index is " + start);
                    var api = this.api();
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Claim Amount'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(7).footer()).html(searchTotal);
                    //$(api.column(6).footer()).html(
                    //    api.column(6).data().reduce(function (a, b) {
                    //        return intVal(a) + intVal(b);
                    //    }, 0)
                    //);
                    var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'ApprAmt'; }).first().index();
                    var totalData1 = api.column(costColumnIndex1).data();
                    var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                    var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                    var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    $(api.column(8).footer()).html(searchTotal1);
           
                    //$(api.column(7).footer()).html(
                    //    api.column(7).data().reduce(function (a, b) {
                    //        return intVal(a) + intVal(b);
                    //    }, 0)
                    //);

                    if (searchTotal1 == 'NaN' || searchTotal1 == '') { $(api.column(8).footer()).html('0.0') }
                }
            })

        });
    </script>
</asp:Content>
