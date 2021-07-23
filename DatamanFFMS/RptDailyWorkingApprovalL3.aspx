<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="RptDailyWorkingApprovalL3.aspx.cs" Inherits="AstralFFMS.RptDailyWorkingApprovalL3" %>
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

            $('#example1 .imgButton').click(function () {
                var currentRow = $(this).parents("tr");
                var smidA = currentRow.find("span[id*='lblsmid']").text();
                var vdateA = currentRow.find("span[id*='lblvdate']").text();
                //alert(smidA);
                $("#popupWindow").dialog({
                    title: "DSR Approval",
                    width: 400,
                    height: 300,
                    modal: true,
                    closeText: '',
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
                                    $(checked_radio).val('');
                                    var setval = document.getElementById("<%=hid.ClientID%>").value;
                                    window.location.href = 'RptDailyWorkingApprovalL3.aspx?hasval=' + setval + '&smIDStr=<%=ViewState["smIDStr"]%>&userid=<%=ViewState["UserId"]%>';
                                    document.getElementById("<%=hid.ClientID%>").value("Y");
                                    setval = document.getElementById("<%=hid.ClientID%>").value;

                                }
                            });

                        },
                        Close: function () {
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
                $("#popupWindow").dialog({
                    title: "DSR Approval",
                    width: 400,
                    height: 300,
                    modal: true,
                    closeText: '',
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
                                    $(checked_radio).val('');
                                    var setval = document.getElementById("<%=hid.ClientID%>").value;
                                    window.location.href = 'RptDailyWorkingApprovalL3.aspx?hasval=' + setval + '&smIDStr=<%=ViewState["smIDStr"]%>&userid=<%=ViewState["UserId"]%>';
                                    document.getElementById("<%=hid.ClientID%>").value("Y");
                                    setval = document.getElementById("<%=hid.ClientID%>").value;

                                }
                            });

                        },
                        Close: function () {
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
                $("#popupWindow").dialog({
                    title: "DSR Approval",
                    width: 400,
                    height: 300,
                    modal: true,
                    closeText: '',
                    buttons: {
                        Save: function () {

                            //alert($("[id*=TextArea1]").val());                           
                            //$("[id*=visHdf]").val($("[id*=approveStatusRadioButtonList]").val());

                            $("[id*=hndtxt_email]").val($("[id*=TextArea1]").val());

                            var checked_radio = $("[id*=approveStatusRadioButtonList] input:checked");
                            //alert(checked_radio.val());
                            //$("[id*=hndtxt_email]").val($("[id*=TextArea1]").val()); 

                            $("[id*=Save]").click();
                            var obj = {};
                            obj.smid = smidA;
                            obj.Vdate = vdateA;
                            obj.status = checked_radio.val();
                            obj.Remark = $.trim($("[id*=TextArea1]").val());
                            //alert(JSON.stringify(obj));
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
                                    $(checked_radio).val('');

                                    var setval = document.getElementById("<%=hid.ClientID%>").value;
                                    window.location.href = 'RptDailyWorkingApprovalL3.aspx?hasval=' + setval + '&smIDStr=<%=ViewState["smIDStr"]%>&userid=<%=ViewState["UserId"]%>';
                                    document.getElementById("<%=hid.ClientID%>").value("Y");
                                    setval = document.getElementById("<%=hid.ClientID%>").value;


                                }
                            });

                        },
                        Close: function () {
                            $(this).dialog('close');
                        }
                    }

                });
                return false;
            });

        });

   </script>



    <style>
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
                                    <%--<h3 class="box-title">Daily Working Approval L3</h3>--%>
                                    <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
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
                                            <div class="col-md-6">
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
                             <asp:Repeater ID="rpt" runat="server"  OnItemDataBound="rpt_ItemDataBound" OnItemCommand="rpt_ItemCommand">
                                <HeaderTemplate>                                    
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                 <th style="display: none;">VDate</th>
                                                <th>Visit Date</th>
                                                <th>Sales Person</th>
                                                  <th>Sync Id</th>
                                                 <th>Employee</th>
                                                 <th>Type</th>
                                                <th>Remarks</th>
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
                                                <th style="text-align:right">Party Collection</th>
                                                <th style="text-align:right">Retailer Per Call Avg Sale</th>
                                                <th hidden>Pro-Calls</th>
                                                <th style="text-align:right">New Parties</th>
                                                <th hidden>Collections</th>
                                                <th style="text-align:right">Claim Exp.</th>
                                                <th style="text-align:right">Approved Exp.</th>
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
                                        <asp:HiddenField ID="hdnType" runat="server" Value='<%#Eval("Type")%>' />         
                                         <asp:HiddenField ID="hdnAType" runat="server" Value='<%#Eval("AType")%>' />         
                                        <td style="display: none;">  <asp:Label ID="lblsmid" runat="server" CssClass="aVis" Text='<%#Eval("SMId") %>'></asp:Label>
                                            <asp:Label ID="lblvdate" runat="server" CssClass="aVis" Text='<%#Eval("VDate","{0:dd/MMM/yyyy}")%> '></asp:Label></td>                       
                                        <td><asp:LinkButton CommandName="selectDate" ID="lnkEdit1"
                                                    CausesValidation="False" runat="server" OnClientClick="window.document.forms[0].target='_blank'; setTimeout(function(){window.document.forms[0].target='';}, 500);" 
                                                    Text='<%# System.Convert.ToDateTime(Eval("VDate")).ToString("dd/MMM/yyyy") %>'
                                                    Width="80px" Font-Underline="True" /></td>
                                        <td><%# Eval("Level1")%></td>
                                         <td><%# Eval("SyncId")%></td>
                                        <td><%# Eval("EmpName")%></td>                                       
                                        <td><asp:Label ID="lblType" runat="server" Text='<%# Eval("Type")%>'></asp:Label></td>
                                        <td>
                                        <%--<%# Eval("Remarks")%>--%>
                                          <asp:Label ID="lblremarks" runat="server" Text='<%# Eval("Remarks")%>' CssClass="aVis"></asp:Label>
                                        <asp:LinkButton CssClass="ShortDesc"  runat="server" ToolTip="Show Full Remarks" Text='<%# Eval("Remarks")%>'></asp:LinkButton>
                                    </td>
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
                                        <td style="text-align:right"><asp:Label ID="lblTotalpartyColl" runat="server" Text='<%# Eval("PartyCollection")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblPerCallAvg" runat="server" Text='<%# Eval("PerCallAvgCell")%>'></asp:Label></td>
                                        <td hidden><asp:Label ID="lblRetProCalls" runat="server" Text='<%# Eval("RetailerProCalls")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblNewParty" runat="server" Text='<%# Eval("NewParties")%>'></asp:Label></td>
                                        <td hidden><asp:Label ID="lblCollections" runat="server" Text='<%# Eval("Collections")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblLocalExp" runat="server" Text='<%# Eval("LocalExpenses")%>'></asp:Label></td>
                                        <td style="text-align:right"><asp:Label ID="lblTourExp" runat="server" Text='<%# Eval("TourExpenses")%>'></asp:Label></td>
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
                                            <asp:LinkButton ID="lnkEdit" CssClass="imgButton" runat="server" CommandArgument='<%# Eval("SMId")+","+ Eval("VDate") %>' Width="80px" Visible="true" /> 
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
                        <asp:Button ID="Button1" runat="server" Text="Button" Style="display: none" />
                       <%-- <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="Button1" PopupControlID="Panel3"
                            CancelControlID="Close" BackgroundCssClass="modalBackground">
                        </ajaxToolkit:ModalPopupExtender>--%>

                        <asp:Panel ID="Panel3" runat="server" CssClass="modalPopup" Style="display: none">

                            <div id="popupWindow" style="margin-top: 0px; left: 10px;">
                                <div class="box-body">
                                    <div class="box-primary">
                                      <%--  <div class="header">DSR Approval</div>--%>
                                        <div style="overflow: hidden;">
                                            <table style="font-size: 12px;">
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
                                                   <%-- <td>Remark:&nbsp;</td>--%>
                                                     <td><b>Remark:</b>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label></td>  
                                                    <td>
                                                        <textarea id="TextArea1" class="form-control" style="resize: none; height: 80px;" cols="20" rows="2" runat="server"
                                                            placeholder="Enter Remark"></textarea></td>        
                                                        
                                                </tr>
                                               <%-- <tr>
                                                    <td></td>
                                                    <td style="padding-top: 10px;">
                                                        <asp:Button type="button" ID="Save" runat="server" Text="Save" class="btn btn-primary"
                                                            OnClick="Save_Click" />
                                                        <asp:Button type="button" ID="Close" runat="server" Text="Close" class="btn btn-primary" OnClick="Close_Click" />
                                                        
                                                </tr>--%>
                                            </table>
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
    <script type="text/javascript">
        $(document).ready(function () {
            $.widget('ui.dialog', jQuery.extend({}, jQuery.ui.dialog.prototype, {
                _title: function (titleBar) {
                    titleBar.html(this.options.title || '&#160;');
                }
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
                _title: function (titleBar) {
                    titleBar.html(this.options.title || '&#160;');
                }
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
    </script>
</asp:Content>
