<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ExpenseApprovalDetails.aspx.cs" Inherits="AstralFFMS.ExpenseApprovalDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <link type="text/css" rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <style type="text/css">
        .aVis {
            display: none;
        }

         /*#gvDetailstable{
            margin-top:15px;
            border-collapse:collapse;
}*/

        .partyclass {
            width: 100%;
        }

            .partyclass td, .partyclass th {
                border: 1px solid #444;
            }

                .partyclass td:nth-child(even) {
                    background: #f3f3f3;
                }

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

        @media (max-width: 600px) {
            .ui-dialog.ui-widget.ui-widget-content.ui-corner-all.ui-front.ui-draggable.ui-resizable {
                width: 100% !important;
            }
        }

        @media (min-width: 600px) {
            #pnlpopup {
                width: 600px !important;
            }
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#example1 .passamt').focusout(function () {
                var currentRow1 = $(this).parents("tr");
                var passamtval = currentRow1.find(".passamt").val();
                var claimamtval = currentRow1.find("span[id*='lblclaimAmt']").text();
               
                if (parseFloat(passamtval) > parseFloat(claimamtval)) {
                  
                    errormessage("Approved amount cannot be greater than Claim amount.");  
                  <%--  $('#<%=btnUnsubmit.ClientID%>').hide();
                    $('#<%=btnUnapprove.ClientID%>').hide();
                    $('#<%=btnSaveExpSheet.ClientID%>').hide();
                    $('#<%=lnkSubmit.ClientID%>').hide();--%>
                    currentRow1.find(".passamt").val(claimamtval);
                }
                else {
                    <%--$('#<%=btnUnsubmit.ClientID%>').show();
                    $('#<%=btnUnapprove.ClientID%>').show();
                    $('#<%=btnSaveExpSheet.ClientID%>').show();
                    $('#<%=lnkSubmit.ClientID%>').show();--%>
                }
            })
        
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
                var smid = currentRow.find("span[id*='lblsmid']").text();
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
                $("#lbltrremarks").text(remark);
                var obj = {};
                obj.Smid = smid;
                obj.Billdate = billdate;
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

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "ExpenseApprovalDetails.aspx/BindAttendenceStatus",                    
                    data: JSON.stringify(obj),
                    dataType: "json",
                    success: function (data) {
                        //if (ExptypeCode.toUpperCase() == "CONVEYANCETRAVEL" || ExptypeCode.toUpperCase() == "CONVEYANCE")
                        //{
                            $("#gvDetailstable").html('');
                            if (data.d.length > 0) {
                                $("#gvDetailstable").append("<thead><tr><th>DSR Status Details:</th></tr></thead>");
                                $("#gvDetailstable").append("<tbody>");
                                for (var i = 0; i < data.d.length; i++) {
                                    $("#gvDetailstable").append("<tr class='partyclass1'><td>" +
                                    data.d[i].DSRStatus + "</td> </tr>");
                                }
                                $("#gvDetailstable").append("</tbody>");

                            }
                        //}
                        //else if (ExptypeCode.toUpperCase() == "TRAVEL") {
                        //    $("#gvDetailstable1").html('');
                        //    if (data.d.length > 0) {
                        //        $("#gvDetailstable1").append("<thead><tr><th>DSR Status Details:</th></tr></thead>");
                        //        $("#gvDetailstable1").append("<tbody>");
                        //        for (var i = 0; i < data.d.length; i++) {
                        //            $("#gvDetailstable1").append("<tr class='partyclass'><td>" +
                        //            data.d[i].DSRStatus + "</td> </tr>");
                        //        }
                        //        $("#gvDetailstable1").append("</tbody>");

                        //    }

                        //}
                        //else {
                           
                        //        $("#gvDetailstable2").html('');
                        //        if (data.d.length > 0) {
                        //            $("#gvDetailstable2").append("<thead><tr><th>DSR Status Details:</th></tr></thead>");
                        //            $("#gvDetailstable2").append("<tbody>");
                        //            for (var i = 0; i < data.d.length; i++) {
                        //                $("#gvDetailstable2").append("<tr class='partyclass'><td>" +
                        //                data.d[i].DSRStatus + "</td> </tr>");
                        //            }
                        //            $("#gvDetailstable2").append("</tbody>");

                        //        }

                            
                        //}                          

                    },
                    error: function (result) {
                        // alert("Error login");

                    }
                });


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
<%--        <div id="gvDetailstable">
            <asp:GridView ID="gvDetails" runat="server" class="ttttttttt" BorderColor="#6699ff" BorderStyle="Groove" AutoGenerateColumns="false" EmptyDataText="No Record Found">
            </asp:GridView>
         </div>--%>
        <%--<table class="table-responsive" id="gvDetailstable">
            </table>--%>
        <table class="table-responsive table" id="gvDetailstable">
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
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
        </div>
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div id="divData" runat="server">
            <div id="Div1">
                <div class="box-body" id="rptmain" runat="server">
                    <div class="row">
                        <div class="col-xs-12">

                            <div class="box">
                                <div class="box-header">
                                    <h3 class="box-title">Expense Details List</h3>
                                </div>
                                <!-- /.box-header -->
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="rpt" runat="server" EnableViewState="true" OnItemDataBound="rpt_ItemDataBound">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <th>Details</th>
                                                        <th>Employee Name</th>
                                                        <th>Expense Type</th>
                                                        <th>City</th>
                                                        <th>Bill Date</th>
                                                        <th>Bill No.</th>
                                                        <th style="text-align: right;">Bill Amt.</th>
                                                        <th style="text-align: right;">Claimed Amt.</th>
                                                        <th style="text-align: right;">Approve Amt.</th>
                                                        <th>Approve/Reject Remarks</th>
                                                        <th>Supporting Attached</th>
                                                        <th>Supporting Received</th>
                                                        <th style="text-align: right;">Tax Code
                                                           <%-- <asp:DropDownList ID="ddlsettaxcode" runat="server" CssClass="form-control" Width="100px"></asp:DropDownList>--%>
                                                        </th>
                                                        <th>GSTIN Registered</th>
                                                        <th>GSTIN No.</th>
                                                        <th>Vendor/Party</th>
                                                        <th>CGST Amt.</th>
                                                        <th>SGST Amt.</th>
                                                        <th>IGST Amt.</th>
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
                                                        <th style="padding: 0;">
                                                            <asp:Button ID="btnunset" OnClick="btnunset_Click" runat="server" Text="UnSet" />
                                                            <asp:Button ID="btnset" OnClick="btnset_Click" runat="server" Text="Set" /></th>
                                                        <th></th>
                                                        <th></th>
                                                        <th></th>
                                                        <th style="padding: 0;">
                                                            <asp:Button ID="btnUnsetTxtcode" OnClick="btnUnsetTxtcode_Click" runat="server" Text="UnSet" />
                                                            <asp:Button ID="btnSetTxtcode" OnClick="btnSetTxtcode_Click" runat="server" Text="Set" />
                                                            <%--<asp:DropDownList ID="ddlsettaxcode" runat="server" CssClass="form-control" Width="100px"></asp:DropDownList>--%>
                                                        </th>
                                                          
                                                        <th></th>
                                                         <th></th>  
                                                         <th></th>  
                                                         <th></th>  
                                                         <th></th>
                                                        <th></th>                                                       

                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><asp:ImageButton ID="imbShowDetails" class="imgButton" Height="30" runat="server" ImageUrl="~/img/popup.png" /></td>
                                                <td><%#Eval("EmpName") %></td>
                                                <td>
                                                    <asp:Label ID="lblname" runat="server" Text='<%#Eval("Name") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdfExpDetailId" runat="server" Value='<%#Eval("ExpenseDetailID") %>' />
                                                    <asp:HiddenField ID="hdftaxcode" runat="server" Value='<%#Eval("taxcode") %>' />
                                                    <asp:Label ID="lblexpdetId" runat="server" CssClass="aVis" Text='<%#Eval("ExpenseDetailID") %>'></asp:Label>
                                                </td>
                                                <td><asp:Label ID="myid" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FromcityName")%>'></asp:Label></td>
                                                <%--<td><%#Eval("AreaName") %></td>--%>                                        
                                                <td><asp:Label ID="lblbilldt" runat="server" Text='<%#Eval("BillDate") %>'></asp:Label></td>
                                                <td><asp:Label ID="lblbillnum" runat="server" Text='<%#Eval("BillNumber") %>'></asp:Label></td>
                                                <td><asp:Label ID="lblbillAmt" runat="server" Text='<%#Eval("BillAmount") %>' Style="text-align: right;"></asp:Label></td>
                                                <td><asp:Label ID="lblclaimAmt" runat="server" Text='<%#Eval("ClaimAmount") %>' Style="text-align: right;"></asp:Label></td>
                                                <td><asp:TextBox ID="txtPassAmt" MaxLength="12" Width="100px" ClientIDMode="Static" placeholder="Pass Amount" class="form-control numeric text-right passamt" Text='<%# Eval("SavedAmt") %>' runat="server" Style="text-align: right;"></asp:TextBox></td>
                                                <td><asp:TextBox ID="txtremarks" MaxLength="200" placeholder="Approve/Reject Remarks" Text='<%# Eval("ApproverRemarks") %>' class="form-control" runat="server"></asp:TextBox></td>
                                                <td><asp:CheckBox ID="chkSA" Enabled="false" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSupportingAttached")%>'/></td>                                               
                                                <td>
                                                    <asp:CheckBox ID="chkPSA" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "issupportingapproved")%>' />                                                   
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
                                                    <asp:Label ID="lblsmid" runat="server" CssClass="aVis" Text='<%#Eval("smid") %>'></asp:Label>
                                                </td>
                                               <%-- <td><asp:TextBox ID="txtTextCode" MaxLength="12" Width="100px" ClientIDMode="Static" class="form-control numeric text-right" Text='<%# Eval("GSTNINTextCode") %>' runat="server" Style="text-align: right;"></asp:TextBox></td>--%>
                                               <td><asp:DropDownList ID="ddlTaxCode" runat="server" CssClass="form-control" Width="100px"></asp:DropDownList></td>
                                               <td><asp:CheckBox ID="chkGstIN" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsGSTNNo")%>'/></td>   
                                              <%-- <td><asp:Label ID ="lblGstn" runat="server" Text='<%#Eval("gstno") %>' ></asp:Label></td>--%>
                                               <td><asp:TextBox ID="txtGstinNo" MaxLength="15" Width="100px" class="form-control"  Text='<%# Eval("gstno") %>' runat="server"></asp:TextBox></td>
                                              <%-- <td><asp:Label ID ="lblVendor" runat="server" Text='<%#Eval("vendor") %>'></asp:Label></td>--%>                                                 
                                                 <td><asp:TextBox ID="txtVendor" MaxLength="200" Width="100px" class="form-control"  Text='<%# Eval("vendor") %>' runat="server"></asp:TextBox></td>
                                              <%-- <td><asp:Label ID ="lblCgst" runat="server" Text='<%#Eval("CGSTAmt") %>' Style="text-align: right;" ></asp:Label></td>--%>
                                                <td><asp:TextBox ID="txtCGSTAmt" MaxLength="12" Width="100px" ClientIDMode="Static" class="form-control numeric text-right" Text='<%# Eval("CGSTAmt") %>' runat="server" Style="text-align: right;"></asp:TextBox></td>
                                             <%--  <td><asp:Label ID ="lblSgst" runat="server" Text='<%#Eval("SGSTAmt") %>' Style="text-align: right;"></asp:Label></td>--%>
                                                <td><asp:TextBox ID="txtSGSTAmt" MaxLength="12" Width="100px" ClientIDMode="Static" class="form-control numeric text-right" Text='<%# Eval("SGSTAmt") %>' runat="server" Style="text-align: right;"></asp:TextBox></td>
                                            <%--   <td><asp:Label ID ="Label1" runat="server" Text='<%#Eval("IGSTAmt") %>' Style="text-align: right;"></asp:Label></td>--%>
                                                <td><asp:TextBox ID="txtIGSTAmt" MaxLength="12" Width="100px" ClientIDMode="Static" class="form-control numeric text-right" Text='<%# Eval("IGSTAmt") %>' runat="server" Style="text-align: right;"></asp:TextBox></td>
                                                  
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        </tbody></table>       
                                        </FooterTemplate>

                                    </asp:Repeater>


                                    <div style="float: right;">
                                        <asp:Button ID="btnUnsubmit" Text="UnSubmit(Send back to Employee)" OnClick="btnUnsubmit_Click" runat="server" CssClass="btn btn-primary" OnClientClick="ConfirmUnSubmit();" />
                                        <asp:Button ID="btnUnapprove" Text="UnApprove(Send back to Approval)" OnClick="btnUnapprove_Click" runat="server" CssClass="btn btn-primary" OnClientClick="ConfirmUnApproval();" />
                                        <asp:Button ID="btnSaveExpSheet" Text="Save Expense Sheet" OnClick="btnSaveExpSheet_Click" runat="server" CssClass="btn btn-primary" OnClientClick="ConfirmSaveExpenseSheet();" />
                                        <asp:Button ID="lnkSubmit" runat="server" OnClick="lnkSubmit_Click" class="btn btn-primary demo1" Text="Approve" OnClientClick="ConfirmSubmit();" />
                                        <asp:LinkButton ID="btncancel" runat="server" PostBackUrl="~/ExpenseApproval.aspx" CssClass="btn btn-primary" Text="Cancel"></asp:LinkButton>
                                    </div>


                                </div>
                            </div>
                        </div>
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
        $(function () {
            $("#example1").DataTable({
                "bLengthChange": false,
                "bPaginate": false,
                "bJQueryUI": true,
                "bFilter": false,
                "bSort": true,
                "bInfo": true
            });

        });
     <%--   function Validate2() {
            $('#<%=lblerror.ClientID%>').removeClass('hidden');
            $('#<%=lblerror.ClientID%>').val("");

            if ($('#<%=RejRemarks.ClientID%>').val() == "") {
                $('#<%=lblerror.ClientID%>').html("* Please enter Rejection Remarks.");
                return false;
            }
        }--%>
        function ConfirmSubmit() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to Approve Expense Sheet ?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
        function ConfirmUnSubmit() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to UnSubmit Expense Sheet ?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
        function ConfirmSaveExpenseSheet() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to Save Expense Sheet ?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
        function ConfirmUnApproval() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to Unapprove Expense Sheet ?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <script type="text/javascript">
        function myFunction() {
            //      alert();

            //$('#Div1 :input').attr('disabled', true);

            //$('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', true);
        }
    </script>
</asp:Content>
