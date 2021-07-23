<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/FFMS.Master" CodeBehind="NewRptDailyWorkingApprovalL3.aspx.cs" Inherits="AstralFFMS.NewRptDailyWorkingApprovalL3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <link href="plugins/multiselect.css" rel="stylesheet" />
      <script src="plugins/multiselect.js"></script>
      <script src="plugins/datatables/jquery.dataTables.min.js"></script>
     <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>

    <link type="text/css" rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>

     <script type="text/javascript">
         $(function () {
             $("#example1").DataTable({
                 "order": [[0, "desc"]]
             });
         });
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
    </script>

    <script type="text/javascript">

        $(document).on('keyup', $('#example1 .imgButton'), function () {
            $.widget('ui.dialog', jQuery.extend({}, jQuery.ui.dialog.prototype, {
            }));          
     
         //$(document).ready(function () {
         //    // Prototype to assign HTML to the dialog title bar.

         //    $.widget('ui.dialog', jQuery.extend({}, jQuery.ui.dialog.prototype, {
         //        _title: function (titleBar) {
         //            titleBar.html(this.options.title || '&#160;');
         //        }
         //    }));

             $('#example1 .imgButton').click(function () {
                 var currentRow = $(this).parents("tr");
                 var smidA = currentRow.find("span[id*='lblsmid']").text();
                 var vdateA = currentRow.find("span[id*='lblvdate']").text();
                 //alert(smidA);
                 var obj = {};
                 obj.Smid = smidA;
                 obj.vdate = vdateA;
                 $.ajax({
                     type: "POST",
                     contentType: "application/json; charset=utf-8",
                     url: "NewRptDailyWorkingApprovalL3.aspx/BindDatatable",
                     data: JSON.stringify(obj),
                     dataType: "json",
                     success: function (data) {

                         //$("#gvDetailstable").html('');

                         //if (data.d.length > 0) {
                         //    $("#gvDetailstable").append("<thead><tr><th>Total Party</th><th>Retailer Call Visited</th><th>Dist. Discuss</th><th>Dist. Failed Visit</th><th>Dist. Collection</th><th>Total Order</th><th>Order By Email</th><th>Order By Phone</th><th>Demo</th><th>Failed Visit</th><th>Competitor</th><th>Retailer Per Call Avg Cell</th><th>New Parties</th><th>Claim Exp.</th><th>Approved Exp.</th></tr></thead>");
                         //    $("#gvDetailstable").append("<tbody>");
                         //    for (var i = 0; i < data.d.length; i++) {
                         //        $("#gvDetailstable").append("<tr class='partyclass'><td>" +
                         //        data.d[i].totalparty + "</td> <td>" +
                         //        data.d[i].callvisited + "</td> <td>" +
                         //        data.d[i].distdiscuss + "</td> <td>" +
                         //        data.d[i].distfailedvisit + "</td> <td>" +
                         //        data.d[i].distributorcollection + "</td> <td>" +
                         //        data.d[i].totalorder + "</td> <td>" +
                         //        data.d[i].orderamountmail + "</td> <td>" +
                         //        data.d[i].orderamountphone + "</td> <td>" +
                         //        data.d[i].demo + "</td> <td>" +
                         //        data.d[i].failedvisit + "</td> <td>" +
                         //        data.d[i].competitor + "</td> <td>" +
                         //        data.d[i].percallavgcell + "</td> <td>" +
                         //        data.d[i].newparty + "</td> <td>" +
                         //        data.d[i].localexpenses + "</td> <td>" +

                         //        data.d[i].tourexpenses + "</td>  </tr>");
                         //    }
                         //    $("#gvDetailstable").append("</tbody>");

                         //}
                         if (data.d.length > 0) {                            
                             for (var i = 0; i < data.d.length; i++) {
                                 
                                 var totalparty = data.d[i].totalparty;
                                 var callvisited = data.d[i].callvisited;
                                 //alert(callvisit);
                                 var distdiscuss = data.d[i].distdiscuss;
                                 var distfailedvisit = data.d[i].distfailedvisit;
                                 var distributorcollection = data.d[i].distributorcollection;
                                 var totalorder = data.d[i].totalorder;
                                 var totalqty = data.d[i].totalqty;
                                 var orderamountmail = data.d[i].orderamountmail;
                                 var orderamountphone = data.d[i].orderamountphone;
                                 var demo = data.d[i].demo;
                                 var failedvisit = data.d[i].failedvisit;
                                 var competitor = data.d[i].competitor;
                                 var percallavgcell = data.d[i].percallavgcell;
                                 var newparty = data.d[i].newparty;
                                 var localexpenses = data.d[i].localexpenses;
                                 var tourexpenses = data.d[i].tourexpenses;

                                 var VisitType = data.d[i].VisitType;
                                 var Attendance = data.d[i].Attendance;
                                 var Expense = data.d[i].Expense;
                                 var ExpensesRemark = data.d[i].ExpensesRemark;

                                 $("#lbltotparty").text(totalparty);
                                 $("#lblrtlcallvisit").text(callvisited);
                                 $("#lblddisc").text(distdiscuss);
                                 $("#lbldfvt").text(distfailedvisit);
                                 $("#lbldcollection").text(distributorcollection);
                                 $("#lbltorder").text(totalorder);
                                 $("#lbltoQty").text(totalqty);
                                 $("#lblomail").text(orderamountmail);
                                 $("#lblophone").text(orderamountphone);
                                 $("#lbldemo").text(demo);
                                 $("#lblfvisit").text(failedvisit);
                                 $("#lblcomp").text(competitor);
                                 $("#lblrpcavgcell").text(percallavgcell);
                                 $("#lblnparty").text(newparty);
                                 $("#lblcexp").text(localexpenses);
                                 $("#lblaexp").text(tourexpenses);
                                 $("#lblVisitType").text(VisitType);
                                 $("#lblAttendance").text(Attendance);
                                 $("#lblExpense").text(Expense);
                                 $("#lblExpensesRemark").text(ExpensesRemark);
                             }
                             //$("#gvDetailstable").append("</tbody>");

                         }
                     },
                     error: function (result) {
                         // alert("Error login");

                     }
                 });

                 $("#popupWindow").dialog({
                     title: "DSR Approval",
                     width: 800,
                     height: 500,
                     modal: true,
                     closeText: '',
                     open: function (event, ui) {
                         $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                     },
                     buttons: {
                         Save: function () {
                             $("[id*=hndtxt_email]").val($("[id*=TextArea1]").val());
                             var checked_radio = $("[id*=approveStatusRadioButtonList] input:checked");
                             $("[id*=Save]").click();
                             var obj = {};
                             obj.smid = smidA;
                             obj.Vdate = vdateA;
                             obj.status = checked_radio.val();
                             obj.Remark = $.trim($("[id*=TextArea1]").val()); 
                             if ($('#<%=TextArea1.ClientID%>').val() == "") 
                             {
                                 alert("Please enter remak");
                                 return false;
                             }                           
                             $.ajax({
                                 type: "POST",
                                 url: "NewRptDailyWorkingApprovalL3.aspx/UpdateDSR",
                                 data: JSON.stringify(obj),
                                 contentType: "application/json; charset=utf-8",
                                 dataType: "json",
                                 success: function (data) {
                                     //alert("Record Updated Successfully");
                                     $('#TextArea1').val("");
                                     $(smidA).val('');
                                     $(vdateA).val('');
                                     //$(checked_radio).val('');
                                     var setval = document.getElementById("<%=hid.ClientID%>").value;
                                     window.location.href = 'NewRptDailyWorkingApprovalL3.aspx?hasval=' + setval + '&smIDStr=<%=ViewState["smIDStr"]%>&userid=<%=ViewState["UserId"]%>';
                                     alert("Record Updated Successfully");
                                    document.getElementById("<%=hid.ClientID%>").value("Y");
                                    setval = document.getElementById("<%=hid.ClientID%>").value;

                                }
                            });

                        },
                         Close: function () {
                             $("#lbltotparty").text("");
                             $("#lblrtlcallvisit").text("");
                             $("#lblddisc").text("");
                             $("#lbldfvt").text("");
                             $("#lbldcollection").text("");
                             $("#lbltorder").text("");
                             $("#lbltoQty").text("");
                             $("#lblomail").text("");
                             $("#lblophone").text("");
                             $("#lbldemo").text("");
                             $("#lblfvisit").text("");
                             $("#lblcomp").text("");
                             $("#lblrpcavgcell").text("");
                             $("#lblnparty").text("");
                             $("#lblcexp").text("");
                             $("#lblaexp").text("");
                             $("#lblVisitType").text("");
                             $("#lblAttendance").text("");
                             $("#lblExpense").text("");
                             $("#lblExpensesRemark").text("");
                            $(this).dialog('close');
                        }
                    }

                });
                return false;
            });

         });


        $(document).on('click', $('#example1 .imgButton'), function () {
            $.widget('ui.dialog', jQuery.extend({}, jQuery.ui.dialog.prototype, {
            }));

            $('#example1 .imgButton').click(function () {
                var currentRow = $(this).parents("tr");
                var smidA = currentRow.find("span[id*='lblsmid']").text();
                var vdateA = currentRow.find("span[id*='lblvdate']").text();
                //alert(smidA);
                var obj = {};
                obj.Smid = smidA;
                obj.vdate = vdateA;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "NewRptDailyWorkingApprovalL3.aspx/BindDatatable",
                    data: JSON.stringify(obj),
                    dataType: "json",
                    success: function (data) {

                        //$("#gvDetailstable").html('');

                        //if (data.d.length > 0) {
                        //    $("#gvDetailstable").append("<thead><tr><th>Total Party</th><th>Retailer Call Visited</th><th>Dist. Discuss</th><th>Dist. Failed Visit</th><th>Dist. Collection</th><th>Total Order</th><th>Order By Email</th><th>Order By Phone</th><th>Demo</th><th>Failed Visit</th><th>Competitor</th><th>Retailer Per Call Avg Cell</th><th>New Parties</th><th>Claim Exp.</th><th>Approved Exp.</th></tr></thead>");
                        //    $("#gvDetailstable").append("<tbody>");
                        //    for (var i = 0; i < data.d.length; i++) {
                        //        $("#gvDetailstable").append("<tr class='partyclass'><td>" +
                        //        data.d[i].totalparty + "</td> <td>" +
                        //        data.d[i].callvisited + "</td> <td>" +
                        //        data.d[i].distdiscuss + "</td> <td>" +
                        //        data.d[i].distfailedvisit + "</td> <td>" +
                        //        data.d[i].distributorcollection + "</td> <td>" +
                        //        data.d[i].totalorder + "</td> <td>" +
                        //        data.d[i].orderamountmail + "</td> <td>" +
                        //        data.d[i].orderamountphone + "</td> <td>" +
                        //        data.d[i].demo + "</td> <td>" +
                        //        data.d[i].failedvisit + "</td> <td>" +
                        //        data.d[i].competitor + "</td> <td>" +
                        //        data.d[i].percallavgcell + "</td> <td>" +
                        //        data.d[i].newparty + "</td> <td>" +
                        //        data.d[i].localexpenses + "</td> <td>" +

                        //        data.d[i].tourexpenses + "</td>  </tr>");
                        //    }
                        //    $("#gvDetailstable").append("</tbody>");

                        //}
                        if (data.d.length > 0) {
                            for (var i = 0; i < data.d.length; i++) {

                                var totalparty = data.d[i].totalparty;
                                var callvisited = data.d[i].callvisited;
                                //alert(callvisit);
                                var distdiscuss = data.d[i].distdiscuss;
                                var distfailedvisit = data.d[i].distfailedvisit;
                                var distributorcollection = data.d[i].distributorcollection;
                                var totalorder = data.d[i].totalorder;
                                var totalqty = data.d[i].totalqty;
                                var orderamountmail = data.d[i].orderamountmail;
                                var orderamountphone = data.d[i].orderamountphone;
                                var demo = data.d[i].demo;
                                var failedvisit = data.d[i].failedvisit;
                                var competitor = data.d[i].competitor;
                                var percallavgcell = data.d[i].percallavgcell;
                                var newparty = data.d[i].newparty;
                                var localexpenses = data.d[i].localexpenses;
                                var tourexpenses = data.d[i].tourexpenses;

                                var VisitType = data.d[i].VisitType;
                                var Attendance = data.d[i].Attendance;
                                var Expense = data.d[i].Expense;
                                var ExpensesRemark = data.d[i].ExpensesRemark;

                                $("#lbltotparty").text(totalparty);
                                $("#lblrtlcallvisit").text(callvisited);
                                $("#lblddisc").text(distdiscuss);
                                $("#lbldfvt").text(distfailedvisit);
                                $("#lbldcollection").text(distributorcollection);
                                $("#lbltorder").text(totalorder);
                                $("#lbltoQty").text(totalqty);
                                $("#lblomail").text(orderamountmail);
                                $("#lblophone").text(orderamountphone);
                                $("#lbldemo").text(demo);
                                $("#lblfvisit").text(failedvisit);
                                $("#lblcomp").text(competitor);
                                $("#lblrpcavgcell").text(percallavgcell);
                                $("#lblnparty").text(newparty);
                                $("#lblcexp").text(localexpenses);
                                $("#lblaexp").text(tourexpenses);

                                $("#lblVisitType").text(VisitType);
                                $("#lblAttendance").text(Attendance);
                                $("#lblExpense").text(Expense);
                                $("#lblExpensesRemark").text(ExpensesRemark);

                            }
                            //$("#gvDetailstable").append("</tbody>");

                        }
                    },
                    error: function (result) {
                        // alert("Error login");

                    }
                });

                $("#popupWindow").dialog({
                    title: "DSR Approval",
                    width: 800,
                    height: 500,
                    modal: true,
                    closeText: '',
                    open: function (event, ui) {
                        $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                    },
                    buttons: {
                        Save: function () {
                            $("[id*=hndtxt_email]").val($("[id*=TextArea1]").val());
                            var checked_radio = $("[id*=approveStatusRadioButtonList] input:checked");
                            $("[id*=Save]").click();
                            var obj = {};
                            obj.smid = smidA;
                            obj.Vdate = vdateA;
                            obj.status = checked_radio.val();
                            obj.Remark = $.trim($("[id*=TextArea1]").val());
                            if ($('#<%=TextArea1.ClientID%>').val() == "") {
                                alert("Please enter remak");
                                return false;
                            }                           
                            $.ajax({
                                type: "POST",
                                url: "NewRptDailyWorkingApprovalL3.aspx/UpdateDSR",
                                data: JSON.stringify(obj),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                    //alert("Record Updated Successfully");
                                    $('#TextArea1').val("");
                                    $(smidA).val('');
                                    $(vdateA).val('');
                                    //$(checked_radio).val('');
                                    var setval = document.getElementById("<%=hid.ClientID%>").value;
                                    window.location.href = 'NewRptDailyWorkingApprovalL3.aspx?hasval=' + setval + '&smIDStr=<%=ViewState["smIDStr"]%>&userid=<%=ViewState["UserId"]%>';
                                    alert("Record Updated Successfully");
                                    document.getElementById("<%=hid.ClientID%>").value("Y");
                                    setval = document.getElementById("<%=hid.ClientID%>").value;

                                }
                            });

                        },
                        Close: function () {
                            $("#lbltotparty").text("");
                            $("#lblrtlcallvisit").text("");
                            $("#lblddisc").text("");
                            $("#lbldfvt").text("");
                            $("#lbldcollection").text("");
                            $("#lbltorder").text("");
                            $("#lbltoQty").text("");
                            $("#lblomail").text("");
                            $("#lblophone").text("");
                            $("#lbldemo").text("");
                            $("#lblfvisit").text("");
                            $("#lblcomp").text("");
                            $("#lblrpcavgcell").text("");
                            $("#lblnparty").text("");
                            $("#lblcexp").text("");
                            $("#lblaexp").text("");

                            $("#lblVisitType").text("");
                            $("#lblAttendance").text("");
                            $("#lblExpense").text("");
                            $("#lblExpensesRemark").text("");
                            $(this).dialog('close');
                        }
                    }

                });
                return false;
            });

        });

        $(document).ready(function () {
            // Prototype to assign HTML to the dialog title bar.

            $.widget('ui.dialog', jQuery.extend({}, jQuery.ui.dialog.prototype, {
                //_title: function (titleBar) {
                //    titleBar.html(this.options.title || '&#160;');
                //}
            }));
            // ImageButton Click Event.
            //$('#example1 .imgButton').click(function () {
            //    // Get the Current Row and its values.
            //    alert('ok');
            //    return false;
            //});
            //var hdntxt = '';
            //$("input[name$=chkChild]:checked").each(function() {
            //    // Get Hidden Field Value from Gridview
            //    hdntxt += "," + $(this).next("input[name$=SMId]").val()
            //});

            $('#example1 .imgButton').click(function () {
                var currentRow = $(this).parents("tr");
                var smidA = currentRow.find("span[id*='lblsmid']").text();
                var vdateA = currentRow.find("span[id*='lblvdate']").text();
                //alert(smidA);
                var obj = {};
                obj.Smid = smidA;
                obj.vdate = vdateA;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "NewRptDailyWorkingApprovalL3.aspx/BindDatatable",
                    data: JSON.stringify(obj),
                    dataType: "json",
                    success: function (data) {

                        //$("#gvDetailstable").html('');

                        //if (data.d.length > 0) {
                        //    $("#gvDetailstable").append("<thead><tr><th>Total Party</th><th>Retailer Call Visited</th><th>Dist. Discuss</th><th>Dist. Failed Visit</th><th>Dist. Collection</th><th>Total Order</th><th>Order By Email</th><th>Order By Phone</th><th>Demo</th><th>Failed Visit</th><th>Competitor</th><th>Retailer Per Call Avg Cell</th><th>New Parties</th><th>Claim Exp.</th><th>Approved Exp.</th></tr></thead>");
                        //    $("#gvDetailstable").append("<tbody>");
                        //    for (var i = 0; i < data.d.length; i++) {
                        //        $("#gvDetailstable").append("<tr class='partyclass'><td>" +
                        //        data.d[i].totalparty + "</td> <td>" +
                        //        data.d[i].callvisited + "</td> <td>" +
                        //        data.d[i].distdiscuss + "</td> <td>" +
                        //        data.d[i].distfailedvisit + "</td> <td>" +
                        //        data.d[i].distributorcollection + "</td> <td>" +
                        //        data.d[i].totalorder + "</td> <td>" +
                        //        data.d[i].orderamountmail + "</td> <td>" +
                        //        data.d[i].orderamountphone + "</td> <td>" +
                        //        data.d[i].demo + "</td> <td>" +
                        //        data.d[i].failedvisit + "</td> <td>" +
                        //        data.d[i].competitor + "</td> <td>" +
                        //        data.d[i].percallavgcell + "</td> <td>" +
                        //        data.d[i].newparty + "</td> <td>" +
                        //        data.d[i].localexpenses + "</td> <td>" +

                        //        data.d[i].tourexpenses + "</td>  </tr>");
                        //    }
                        //    $("#gvDetailstable").append("</tbody>");

                        //}
                        if (data.d.length > 0) {
                            for (var i = 0; i < data.d.length; i++) {

                                var totalparty = data.d[i].totalparty;
                                var callvisited = data.d[i].callvisited;
                                //alert(callvisit);
                                var distdiscuss = data.d[i].distdiscuss;
                                var distfailedvisit = data.d[i].distfailedvisit;
                                var distributorcollection = data.d[i].distributorcollection;
                                var totalorder = data.d[i].totalorder;
                                var totalqty = data.d[i].totalqty;
                                var orderamountmail = data.d[i].orderamountmail;
                                var orderamountphone = data.d[i].orderamountphone;
                                var demo = data.d[i].demo;
                                var failedvisit = data.d[i].failedvisit;
                                var competitor = data.d[i].competitor;
                                var percallavgcell = data.d[i].percallavgcell;
                                var newparty = data.d[i].newparty;
                                var localexpenses = data.d[i].localexpenses;
                                var tourexpenses = data.d[i].tourexpenses;
                                var VisitType = data.d[i].VisitType;
                                var Attendance = data.d[i].Attendance;
                                var Expense = data.d[i].Expense;
                                var ExpensesRemark = data.d[i].ExpensesRemark;

                                $("#lbltotparty").text(totalparty);
                                $("#lblrtlcallvisit").text(callvisited);
                                $("#lblddisc").text(distdiscuss);
                                $("#lbldfvt").text(distfailedvisit);
                                $("#lbldcollection").text(distributorcollection);
                                $("#lbltorder").text(totalorder);
                                $("#lblomail").text(orderamountmail);
                                $("#lblophone").text(orderamountphone);
                                $("#lbldemo").text(demo);
                                $("#lblfvisit").text(failedvisit);
                                $("#lblcomp").text(competitor);
                                $("#lblrpcavgcell").text(percallavgcell);
                                $("#lblnparty").text(newparty);
                                $("#lblcexp").text(localexpenses);
                                $("#lblaexp").text(tourexpenses);

                                $("#lblVisitType").text(VisitType);
                                $("#lblAttendance").text(Attendance);
                                $("#lblExpense").text(Expense);
                                $("#lblExpensesRemark").text(ExpensesRemark);
                            }
                            //$("#gvDetailstable").append("</tbody>");

                        }
                    },
                    error: function (result) {
                        // alert("Error login");

                    }
                });

                $("#popupWindow").dialog({
                    title: "DSR Approval",
                    width: 800,
                    height: 500,
                    modal: true,
                    closeText: '',
                    open: function (event, ui) {
                        $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                    },
                    buttons: {
                        Save: function () {
                            $("[id*=hndtxt_email]").val($("[id*=TextArea1]").val());
                            var checked_radio = $("[id*=approveStatusRadioButtonList] input:checked");
                            $("[id*=Save]").click();
                            var obj = {};
                            obj.smid = smidA;
                            obj.Vdate = vdateA;
                            obj.status = checked_radio.val();
                            obj.Remark = $.trim($("[id*=TextArea1]").val());
                            if ($('#<%=TextArea1.ClientID%>').val() == "") {
                                alert("Please enter remak");
                                return false;
                            }                          
                            $.ajax({
                                type: "POST",
                                url: "RptDailyWorkingApprovalL3.aspx/UpdateDSR",
                                data: JSON.stringify(obj),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                    //alert("Record Updated Successfully");
                                    $('#TextArea1').val("");
                                    $(smidA).val('');
                                    $(vdateA).val('');
                                    //$(checked_radio).val('');
                                    var setval = document.getElementById("<%=hid.ClientID%>").value;
                                    window.location.href = 'NewRptDailyWorkingApprovalL3.aspx?hasval=' + setval + '&smIDStr=<%=ViewState["smIDStr"]%>&userid=<%=ViewState["UserId"]%>';
                                    alert("Record Updated Successfully");
                                    document.getElementById("<%=hid.ClientID%>").value("Y");
                                    setval = document.getElementById("<%=hid.ClientID%>").value;

                                }
                            });

                        },
                        Close: function () {
                            $("#lbltotparty").text("");
                            $("#lblrtlcallvisit").text("");
                            $("#lblddisc").text("");
                            $("#lbldfvt").text("");
                            $("#lbldcollection").text("");
                            $("#lbltorder").text("");
                            $("#lbltoQty").text("");
                            $("#lblomail").text("");
                            $("#lblophone").text("");
                            $("#lbldemo").text("");
                            $("#lblfvisit").text("");
                            $("#lblcomp").text("");
                            $("#lblrpcavgcell").text("");
                            $("#lblnparty").text("");
                            $("#lblcexp").text("");
                            $("#lblaexp").text("");

                            $("#lblVisitType").text("");
                            $("#lblAttendance").text("");
                            $("#lblExpense").text("");
                            $("#lblExpensesRemark").text("");
                            $(this).dialog('close');
                        }
                    }

                });
                return false;
            });

        });

   </script>



    <style>
        #gvDetailstable{
            margin-top:15px;
            border-collapse:collapse;
}
        #gvDetailstable {
        border: 1px solid #ccc;
        }
         #gvDetailstable tr{
        border: 1px solid #ccc;
        }
          #gvDetailstable td{
        border: 1px solid #ccc;
        }
           #gvDetailstable th{
        border: 1px solid #ccc;
        }
            body .ui-tooltip {
            padding: 0 5px;
            font-size:11px;
            font-weight:600;
        }
             .aVis {
            display: none;
        }
              @media (max-width: 600px) {
            .ui-dialog.ui-widget.ui-widget-content.ui-corner-all.ui-front.ui-draggable.ui-resizable {
                width: 100% !important;
            }
        }
        .table-approval-long table tr td {
            padding: 0 16px !important;
        }

        .table-approval-long table tr th {
            border-right: 1px solid #fff;
        }
    </style>
    <style type="text/css">
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }

        .modalPopup {
            background-color: #FFFFFF;
            width: 300px;
            border: 3px solid #0DA9D0;
            border-radius: 12px;
            padding: 0;
        }

            .modalPopup .header {
                background-color: #2FBDF1;
                height: 30px;
                color: White;
                line-height: 30px;
                text-align: center;
                font-weight: bold;
                border-top-left-radius: 6px;
                border-top-right-radius: 6px;
            }

        td, th {
            padding: 3px;
        }

        .multiselect-container > li > a {
            white-space: normal;
        }

        .input-group .form-control {
            height: 34px;
        }
    </style>
    <style type="text/css">
        .GridPager td {
            padding: 0 !important;
        }

        .GridPager a {
            display: block;
            height: 20px;
            width: 15px;
            background-color: #3c8dbc;
            color: #fff;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        .GridPager span {
            display: block;
            height: 20px;
            width: 15px;
            background-color: #fff;
            color: #3c8dbc;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        #ContentPlaceHolder1_gvData tr td {
            color: black;
        }
    </style>

      <script type="text/javascript">
          $(function () {
              $("[id*=trview] input[type=checkbox]").bind("click", function () {
                  var table = $(this).closest("table");
                  if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                      //Is Parent CheckBox
                      var childDiv = table.next();
                      var isChecked = $(this).is(":checked");
                      $("input[type=checkbox]", childDiv).each(function () {
                          if (isChecked) {
                              $(this).prop("checked", "checked");
                          } else {
                              $(this).removeAttr("checked");
                          }
                      });
                  } else {
                      //Is Child CheckBox
                      var parentDIV = $(this).closest("DIV");
                      if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                          $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                      } else {
                          $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                      }
                  }
              });
          })
    </script>

    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 250, position: "top-right", opacity: 2,
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
        function GetReport() {

            if ($('#<%=ListBox1.ClientID%>').val() == "0") {
                errormessage("Please select Sales Person.");
                return false;
            }

        }
    </script>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            display: table;
        }
    </style>

    <style>
        .jqx-grid-header {
            height: 40px !important;
            text-align: justify;
            white-space: normal !important;
        }

        .jqx-grid-column-header {
            padding-left: 3px;
            white-space: normal !important;
        }
    </style>
    <section class="content">

        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body">
            <!-- left column -->
            <!-- general form elements -->
            <div class="box box-primary">
                <div class="row">
                    <!-- left column -->
                    <div class="col-md-12">
                        <div id="InputWork">
                            <!-- general form elements -->
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Daily Working Approval L3</h3>
                                    <div style="float: right">
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" Visible="false" runat="server" Text="Back" class="btn btn-primary"
                                            OnClick="btnBack_Click" />
                                        <asp:HiddenField ID="hid" runat="server" Value="Y" />
                                    </div>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="col-md-8">
                                        <div class="row">
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Sales Person:</label>                                                   
                                                    <asp:ListBox ID="ListBox1" runat="server" Width="100%" SelectionMode="Multiple" Visible="false"></asp:ListBox>
                                                    <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>
                                                </div>
                                            </div>
                                        </div>                                      
                                    </div>
                                </div>
                                <div class="box-footer">
                                    <asp:Button type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClientClick="GetReport();"
                                        OnClick="btnGo_Click" />
                                    <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Button2_Click" />                                    
                                </div>
                            </div>
                        </div>
                        <div id="rptmain" runat="server" style="display: none;">                          
                             <div class="box-body table-responsive">    
                             <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                                <HeaderTemplate>                                    
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th style="display: none;">VDate</th>
                                                <th>Visit Date</th>
                                                <th>Sales Person</th>
                                                <th>Sync Id</th>
                                               <%-- <th style="display: none;">Employee</th>
                                                <th style="display: none;">Type</th>--%>
                                                <th style="max-width:200px !important;overflow-x:hidden;">Remarks</th>
                                                <th style="max-width:200px !important;overflow-x:hidden;">City Name</th>
                                                <%--<th style="text-align:right">Total Party</th>
                                                <th style="text-align:right">Retailer Calls Visited</th>
                                                <th style="text-align:right">Dist. Discuss</th>
                                                <th style="text-align:right">Dist. Failed Visit</th>
                                                <th style="text-align:right">Dist. Collection</th>
                                                <th style="text-align:right">Total Order</th>
                                                <th style="text-align: right">Order By Email</th>
                                                <th style="text-align: right">Order By Phone</th>
                                                <th style="text-align:right">Demo</th>
                                                <th style="text-align:right">Failed Visit</th>
                                                <th style="text-align:right">Competitor</th>
                                                <th style="text-align:right">Party Collection</th>
                                                <th style="text-align:right">Retailer Per Call Avg Sale</th>
                                                <th hidden>Pro-Calls</th>
                                                <th style="text-align:right">New Parties</th>
                                                <th hidden>Collections</th>
                                                <th style="text-align:right">Claim Exp.</th>
                                                <th style="text-align:right">Approved Exp.</th>--%>
                                                <th>Status</th>
                                                <%--<th>Approved Remarks</th>--%>
                                                <th>Location Tracker</th>  
                                                <th>Approve/Reject</th>                                              
                                            </tr>
                                        </thead>                                      

                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                         
                                                   
                                        <asp:HiddenField ID="hdnDate" runat="server" Value='<%#Eval("VDate","{0:dd/MMM/yyyy}")%> ' />
                                        <asp:HiddenField ID="hdnSMId" runat="server" Value='<%#Eval("SMId")%>' />
                                       <%-- <asp:HiddenField ID="hdnType" runat="server" Value='<%#Eval("Type")%>' /> --%>        
                                         <asp:HiddenField ID="hdnAType" runat="server" Value='<%#Eval("AType")%>' />         
                                        <td style="display: none;">  <asp:Label ID="lblsmid" runat="server" CssClass="aVis" Text='<%#Eval("SMId") %>'></asp:Label>
                                            <asp:Label ID="lblvdate" runat="server" CssClass="aVis" Text='<%#Eval("VDate","{0:dd/MMM/yyyy}")%> '></asp:Label></td>                       
                                        <td><asp:LinkButton CommandName="selectDate" ID="lnkEdit1"
                                                    CausesValidation="False" runat="server" OnClientClick="window.document.forms[0].target='_blank'; setTimeout(function(){window.document.forms[0].target='';}, 500);" 
                                                    Text='<%# System.Convert.ToDateTime(Eval("VDate")).ToString("dd/MMM/yyyy") %>'
                                                    Width="80px" Font-Underline="True" /></td>
                                        <td><%# Eval("Level1")%></td>
                                         <td><%# Eval("SyncId")%></td>
                                        <%--<td style="display: none;"><%# Eval("EmpName")%></td>                                       
                                        <td style="display: none;"><asp:Label ID="lblType" runat="server" Text='<%# Eval("Type")%>'></asp:Label></td>--%>
                                       
                                        <%--<%# Eval("Remarks")%>--%>
                                         <td style="max-width:200px !important;overflow-x:hidden;">
                                          <asp:Label ID="lblremarks" runat="server" Text='<%# Eval("Remark")%>' CssClass="aVis"></asp:Label>
                                        <asp:LinkButton CssClass="ShortDesc"  runat="server" ToolTip="Show Full Remarks" Text='<%# Eval("Remark")%>'></asp:LinkButton>
                                         </td>
                                         <td style="max-width:200px !important;overflow-x:hidden;"> 
                                        <asp:Label ID="lblCityname" runat="server" Text='<%# Eval("cityname")%>' CssClass="aVis"></asp:Label>
                                        <asp:LinkButton CssClass="ShortDescreption"  runat="server" ToolTip="Show Full City Name" Text='<%# Eval("cityname")%>'></asp:LinkButton>
                                     
                                       </td>
                                        <%--<td style="text-align:right"><asp:Label ID="lblTotalParty" runat="server" Text='<%# Eval("TotalParty")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblCallsVisited" runat="server" Text='<%# Eval("CallsVisited")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblTotalDistDiscuss" runat="server" Text='<%# Eval("DistDiscuss")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblDistFailedVisit" runat="server" Text='<%# Eval("DistFailVisit")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblTotalDistColl" runat="server" Text='<%# Eval("DistributorCollection")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblTotalOrder" runat="server" Text='<%# Eval("TotalOrder")%>'></asp:Label></td>  
                                        <td style="text-align:right"><asp:Label ID="lblOrderMail" runat="server" Text='<%# Eval("OrderAmountMail")%>'></asp:Label></td>
                                        <td style="text-align: right"><asp:Label ID="lblOrderPhone" runat="server" Text='<%# Eval("OrderAmountPhone")%>'></asp:Label></td>                                     
                                        <td style="text-align:right"><asp:Label ID="lblDemo" runat="server" Text='<%# Eval("Demo")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblFailedVisit" runat="server" Text='<%# Eval("FailedVisit")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblComp" runat="server" Text='<%# Eval("Competitor")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblTotalpartyColl" runat="server" Text='<%# Eval("PartyCollection")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblPerCallAvg" runat="server" Text='<%# Eval("PerCallAvgCell")%>'></asp:Label></td>
                                        <td hidden><asp:Label ID="lblRetProCalls" runat="server" Text='<%# Eval("RetailerProCalls")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblNewParty" runat="server" Text='<%# Eval("NewParties")%>'></asp:Label></td>
                                        <td hidden><asp:Label ID="lblCollections" runat="server" Text='<%# Eval("Collections")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblLocalExp" runat="server" Text='<%# Eval("LocalExpenses")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblTourExp" runat="server" Text='<%# Eval("TourExpenses")%>'></asp:Label></td>--%>
                                        <td><%# Eval("AType")%></td>
                                        <%--<td><%# Eval("AppRemark")%></td>--%>
                                       <%-- <td><asp:HyperLink ID="hpl" runat="server" NavigateUrl='<%# Eval("SMId", "~/LocationMap.aspx?Smid={0}") %>'
                                            Text="Details" Target="_blank" ToolTip="Location Tracker"></asp:HyperLink></td>--%>
                                        <td>
                                            <asp:HyperLink ID="hpl" runat="server" NavigateUrl='<%# string.Format("~/LocationMap.aspx?Smid={0}&VDate={1}",
HttpUtility.UrlEncode(Eval("SMId").ToString()), HttpUtility.UrlEncode(Eval("VDate").ToString())) %>'
                                            Text="Details" Target="_blank" ToolTip="Location Tracker"></asp:HyperLink>
                                        </td>
                                        <td>
                                           <%-- <asp:LinkButton CommandArgument='<%# Eval("SMId")+","+ Eval("VDate") %>' CommandName="select" ID="lnkEdit"
                                                    CausesValidation="false" runat="server" Width="80px" Visible="true" /> 
                                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="Button1" PopupControlID="Panel3"
                                            CancelControlID="Close" BackgroundCssClass="modalBackground">
                                           </ajaxToolkit:ModalPopupExtender>--%>
                                            <asp:LinkButton ID="lnkEdit" CssClass="imgButton" runat="server" Text="Approve/Reject" CommandArgument='<%# Eval("SMId")+","+ Eval("VDate") %>' Width="80px" Visible="true" /> 
                                            <%--<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="Button1" PopupControlID="Panel3"
                                            CancelControlID="Close" BackgroundCssClass="modalBackground">
                                           </ajaxToolkit:ModalPopupExtender>--%>
                                      </td>                                        
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                            </tbody>     </table>       
                                        </FooterTemplate>
                            </asp:Repeater>
                            </div>

                        </div>

                        <%--  <div id="Div1" runat="server" style="display: none;">                          
                             <div class="box-body table-responsive">    
                             <asp:Repeater ID="Repeater1" runat="server">
                                <HeaderTemplate>                                    
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>                                               
                                                <th style="text-align:right">Total Party</th>
                                                <th style="text-align:right">Retailer Calls Visited</th>
                                                <th style="text-align:right">Dist. Discuss</th>
                                                <th style="text-align:right">Dist. Failed Visit</th>
                                                <th style="text-align:right">Dist. Collection</th>
                                                <th style="text-align:right">Total Order</th>
                                                <th style="text-align: right">Order By Email</th>
                                                <th style="text-align: right">Order By Phone</th>
                                                <th style="text-align:right">Demo</th>
                                                <th style="text-align:right">Failed Visit</th>
                                                <th style="text-align:right">Competitor</th>                                               
                                                <th style="text-align:right">Retailer Per Call Avg Sale</th>                                               
                                                <th style="text-align:right">New Parties</th>                                                
                                                <th style="text-align:right">Claim Exp.</th>
                                                <th style="text-align:right">Approved Exp.</th>                                                                                        
                                            </tr>
                                        </thead>                                      

                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>                                                                                    
                                        <td style="text-align:right"><asp:Label ID="lblTotalParty" runat="server" Text='<%# Eval("TotalParty")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblCallsVisited" runat="server" Text='<%# Eval("CallsVisited")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblTotalDistDiscuss" runat="server" Text='<%# Eval("DistDiscuss")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblDistFailedVisit" runat="server" Text='<%# Eval("DistFailVisit")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblTotalDistColl" runat="server" Text='<%# Eval("DistributorCollection")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblTotalOrder" runat="server" Text='<%# Eval("TotalOrder")%>'></asp:Label></td>  
                                        <td style="text-align:right"><asp:Label ID="lblOrderMail" runat="server" Text='<%# Eval("OrderAmountMail")%>'></asp:Label></td>
                                        <td style="text-align: right"><asp:Label ID="lblOrderPhone" runat="server" Text='<%# Eval("OrderAmountPhone")%>'></asp:Label></td>                                     
                                        <td style="text-align:right"><asp:Label ID="lblDemo" runat="server" Text='<%# Eval("Demo")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblFailedVisit" runat="server" Text='<%# Eval("FailedVisit")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblComp" runat="server" Text='<%# Eval("Competitor")%>'></asp:Label></td>                                        
                                        <td style="text-align:right"><asp:Label ID="lblPerCallAvg" runat="server" Text='<%# Eval("PerCallAvgCell")%>'></asp:Label></td>                                        
                                        <td style="text-align:right"><asp:Label ID="lblNewParty" runat="server" Text='<%# Eval("NewParties")%>'></asp:Label></td>                                        
                                        <td style="text-align:right"><asp:Label ID="lblLocalExp" runat="server" Text='<%# Eval("LocalExpenses")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblTourExp" runat="server" Text='<%# Eval("TourExpenses")%>'></asp:Label></td>                                                                   
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                            </tbody>     </table>       
                                        </FooterTemplate>
                            </asp:Repeater>
                            </div>

                        </div>--%>

                        <asp:Button ID="Button1" runat="server" Text="Button" Style="display: none" />
                       <%-- <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="Button1" PopupControlID="Panel3"
                            CancelControlID="Close" BackgroundCssClass="modalBackground">
                        </ajaxToolkit:ModalPopupExtender>--%>

                        <asp:Panel ID="Panel3" runat="server" CssClass="modalPopup" Style="display: none">

                            <div id="popupWindow" style="margin-top: 0px; left: 10px;">
                                <div class="box-body">
                                    <div class="box-primary">                                     
                                        <div style="overflow: hidden;">
                                            <%--<table style="font-size: 12px;">
                                                <tr>
                                                    <td><b>Status:</b> &nbsp;</td>
                                                    <td>
                                                        <asp:RadioButtonList ID="approveStatusRadioButtonList" RepeatDirection="Horizontal" runat="server">
                                                            <asp:ListItem Selected="True" Value="Approve" Text="Approved"></asp:ListItem>
                                                            <asp:ListItem Value="Reject" Text="Rejected"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                      
                                                        <asp:HiddenField ID="visHdf" runat="server" />
                                                         <asp:HiddenField ID="dateHdf" runat="server" />
                                                </tr>
                                                <tr>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr>                                                  
                                                     <td><b>Remark:</b>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label></td>  
                                                    <td>
                                                        <textarea id="TextArea1" class="form-control" style="resize: none; height: 80px;" cols="20" rows="2" runat="server"
                                                            placeholder="Enter Remark"></textarea></td>        
                                                        
                                                </tr>                                             
                                            </table>
                                            <div class="table-responsive">
                                             <table class="table table-bordered" id="gvDetailstable">
                                             </table></div>--%>


                                            <table class="dsrstatus">
                                                   <tr>
                      <td><b>Status:</b> &nbsp;</td>
                       <td>
                            <asp:RadioButtonList ID="approveStatusRadioButtonList" RepeatDirection="Horizontal" runat="server">
                            <asp:ListItem Selected="True" Value="Approve" Text="Approve"></asp:ListItem>
                            <asp:ListItem Value="Reject" Text="Reject"></asp:ListItem>
                            </asp:RadioButtonList>                                                      
                            <asp:HiddenField ID="visHdf" runat="server" />
                            <asp:HiddenField ID="dateHdf" runat="server" />
                    </tr>
                                               
                <tr>                                                  
                      <td><b>Remark:</b>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label></td>  
                       <td>
                           <textarea id="TextArea1" class="form-control" style="resize: none; height: 80px;" cols="20" rows="2" runat="server"
                           placeholder="Enter Remark"></textarea></td>        
                </tr> 


                                            </table>


                                              <div class="liketable">

                                                <div class="col-md-4 col-sm-12 col-xs-12 paddingleft0">
                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding">
                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Dist. Discuss:</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lblddisc"></span>
                                                        </div>
                                                    </div>


                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding">
                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Total Qty:</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lbltoQty"></span>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding">
                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Total Order:</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lbltorder"></span>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding">
                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Demo:</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lbldemo"></span>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding" id="TotalParty" runat="server">
                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Total Party:</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lbltotparty"></span>
                                                        </div>
                                                    </div>



                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding">
                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Retailer Calls Visited:</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lblrtlcallvisit"></span>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding" id="VisitType" runat="server">
                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Visit Type:</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lblVisitType"></span>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding" id="ExpensesRemark" runat="server">
                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Expenses Remark:</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lblExpensesRemark"></span>
                                                        </div>
                                                    </div>


                                                </div>




                                                <div class="col-md-4 col-sm-12 col-xs-12 no-padding">
                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding">

                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Dist. Failed Visit</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lbldfvt"></span>
                                                        </div>
                                                    </div>




                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding">

                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Order By Email</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lblomail"></span>
                                                        </div>
                                                    </div>



                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding">

                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Failed Visit</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lblfvisit"></span>
                                                        </div>
                                                    </div>


                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding">

                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Ret.Per Call Avg. Sale</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lblrpcavgcell"></span>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding">

                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Claim Exp</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lblcexp"></span>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding" id="Attendance" runat="server">

                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Attendance</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lblAttendance"></span>
                                                        </div>
                                                    </div>

                                                </div>




                                                <div class="col-md-4 col-sm-12 col-xs-12 paddingright0">
                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding">
                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Dist. Collection:</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lbldcollection"></span>
                                                        </div>
                                                    </div>


                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding">
                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Order By Phone:</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lblophone"></span>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding">
                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Competitor:</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lblcomp"></span>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding">
                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">New Parties:</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lblnparty"></span>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding">
                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Approved Exp:</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lblaexp"></span>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-sm-12 col-xs-12 no-padding" id="Expense" runat="server">
                                                        <div class="col-md-8 col-sm-6 col-xs-6">
                                                            <label class="">Expense:</label>
                                                        </div>

                                                        <div class="col-md-4 col-sm-6 col-xs-6">
                                                            <span id="lblExpense"></span>
                                                        </div>
                                                    </div>

                                                </div>


                                            </div>


                                          
               </div>
            </div>
          </div>
        </div>
                            <asp:Button type="button" ID="Save" runat="server" Text="Save" class="btn btn-primary" />
                            <asp:Button type="button" ID="Close" runat="server" Text="Close" class="btn btn-primary" />                            
                                                 
                        </asp:Panel>

                    </div>
                </div>
            </div>
        </div>
    </section>
     <div id="popupdivremarks" style="display: none">
        <table style="width: 100%;">
            <tbody>
                <tr>
                    <td>
                        <label class="">Remarks:</label>

                    </td>
                    <td>
                        <label style="font-size:small;align-content:stretch;" id="lblshowremarks"></label>
                    </td>

                </tr>
               
            </tbody>
        </table>
    </div>
     <div id="popupdivcityname" style="display: none">
        <table style="width: 100%;">
            <tbody>
                <tr>
                    <td>
                        <label class="">City Name:</label>

                    </td>
                    <td>
                        <label style="font-size:small;align-content:stretch;" id="lblshowcityname"></label>
                    </td>

                </tr>
               
            </tbody>
        </table>
    </div>
       <script type="text/javascript">
           $(document).on('keyup', $('#example1 .ShortDesc'), function () {
               $.widget('ui.dialog', jQuery.extend({}, jQuery.ui.dialog.prototype, {
               }));

               $('#example1 .ShortDesc').click(function () {
                   var currentRow = $(this).parents("tr");
                   var remarks = currentRow.find("span[id*='lblremarks']").text();
                   $("#lblshowremarks").text(remarks);
                   $("#popupdivremarks").dialog({
                       title: "Remarks",
                       width: 500,
                       height: 200,
                       modal: true,
                       closeText: ''
                       //buttons: {
                       //    Close: function () {
                       //        $(this).dialog('close');
                       //    }
                       //}
                   });
                   return false;
               })

           });


           $(document).on('click', $('#example1 .ShortDesc'), function () {
               $.widget('ui.dialog', jQuery.extend({}, jQuery.ui.dialog.prototype, {
               }));

               $('#example1 .ShortDesc').click(function () {
                   var currentRow = $(this).parents("tr");
                   var remarks = currentRow.find("span[id*='lblremarks']").text();
                   $("#lblshowremarks").text(remarks);
                   $("#popupdivremarks").dialog({
                       title: "Remarks",
                       width: 500,
                       height: 200,
                       modal: true,
                       closeText: ''
                       //buttons: {
                       //    Close: function () {
                       //        $(this).dialog('close');
                       //    }
                       //}
                   });
                   return false;
               })

           });


           $(document).ready(function () {
               $.widget('ui.dialog', jQuery.extend({}, jQuery.ui.dialog.prototype, {
                   //_title: function (titleBar) {
                   //    titleBar.html(this.options.title || '&#160;');
                   //}
               }));


               $('#example1 .ShortDesc').click(function () {
                   var currentRow = $(this).parents("tr");
                   var remarks = currentRow.find("span[id*='lblremarks']").text();
                   $("#lblshowremarks").text(remarks);
                   $("#popupdivremarks").dialog({
                       title: "Remarks",
                       width: 500,
                       height: 200,
                       modal: true,
                       closeText: ''
                       //buttons: {
                       //    Close: function () {
                       //        $(this).dialog('close');
                       //    }
                       //}
                   });
                   return false;
               })
           });




           $(document).on('keyup', $('#example1 .ShortDesc'), function () {
               $.widget('ui.dialog', jQuery.extend({}, jQuery.ui.dialog.prototype, {
               }));

               $('#example1 .ShortDescreption').click(function () {
                   var currentRow = $(this).parents("tr");
                   var Cityname = currentRow.find("span[id*='lblCityname']").text();
                   $("#lblshowcityname").text(Cityname);
                   $("#popupdivcityname").dialog({
                       title: "City Name",
                       width: 500,
                       height: 200,
                       modal: true,
                       closeText: ''
                       //buttons: {
                       //    Close: function () {
                       //        $(this).dialog('close');
                       //    }
                       //}
                   });
                   return false;
               })


           });

           $(document).on('click', $('#example1 .ShortDesc'), function () {
               $.widget('ui.dialog', jQuery.extend({}, jQuery.ui.dialog.prototype, {
               }));


               $('#example1 .ShortDescreption').click(function () {
                   var currentRow = $(this).parents("tr");
                   var Cityname = currentRow.find("span[id*='lblCityname']").text();
                   $("#lblshowcityname").text(Cityname);
                   $("#popupdivcityname").dialog({
                       title: "City Name",
                       width: 500,
                       height: 200,
                       modal: true,
                       closeText: ''
                       //buttons: {
                       //    Close: function () {
                       //        $(this).dialog('close');
                       //    }
                       //}
                   });
                   return false;
               })

           });

           $(document).ready(function () {
               $.widget('ui.dialog', jQuery.extend({}, jQuery.ui.dialog.prototype, {
                   _title: function (titleBar) {
                       titleBar.html(this.options.title || '&#160;');
                   }
               }));


               $('#example1 .ShortDescreption').click(function () {
                   var currentRow = $(this).parents("tr");
                   var Cityname = currentRow.find("span[id*='lblCityname']").text();
                   $("#lblshowcityname").text(Cityname);
                   $("#popupdivcityname").dialog({
                       title: "City Name",
                       width: 500,
                       height: 200,
                       modal: true,
                       closeText: ''
                       //buttons: {
                       //    Close: function () {
                       //        $(this).dialog('close');
                       //    }
                       //}
                   });
                   return false;
               })
           });

    </script>
</asp:Content>
