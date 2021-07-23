<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="RptDSRPartyWiseAnalysis.aspx.cs" Inherits="AstralFFMS.RptDSRPartyWiseAnalysis" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--   <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
     <link type="text/css" rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
  <%--  <a href="RptDSRPartyWiseAnalysis.aspx">RptDSRPartyWiseAnalysis.aspx</a>--%>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
        <style>
       body .ui-tooltip {
            padding: 0 5px;
            font-size:11px;
            font-weight:600;
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
                             //$(this).removeAttr("checked");
                         }
                     });
                 } else {
                     //Is Child CheckBox
                     var parentDIV = $(this).closest("DIV");
                     if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                         $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                     } else {
                         //$("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                     }
                 }
             });
         })
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

        function btnSubmitfunc() {
           
           
            var checked_radio = $("[id*=rblview] input:checked");
            var value = checked_radio.val();
           
            if (value == "SalesPerson") {
                var selectedvalue = [];
                $("#<%=trview.ClientID %> :checked").each(function () {
                    selectedvalue.push($(this).val());
                });
                if (value != "Party") {
                    if (selectedvalue == "") {
                        errormessage("Please Select Sales Person");
                        return false;
                    }
                }
            }
            else {
                var state = [];
                $("#<%=ddlState.ClientID %> :selected").each(function () {
                    state.push($(this).val());
                });
                if (state =="" || state == "0") {
                    errormessage("Please Select State");
                    return false;
                }
                $("#hidstate").val(state);

                var city = [];
                $("#<%=ddlCity.ClientID %> :selected").each(function () {
                    city.push($(this).val());
                });
                if (city == "" || city =="0") {
                    errormessage("Please Select City");
                    return false;
                }
                $("#hidcity").val(city);
                var partyId = [];
                $("#<%=ddlParty.ClientID %> :selected").each(function (i, selected) {
                    partyId[i] = $(selected).val();
                });


                $("#hidparty").val(partyId);
               <%-- var party = [];
                $("#<%=ddlParty.ClientID %> :selected").each(function () {
                    party.push($(this).val());
                });
                $("#hidparty").val(party);--%>
            }
                   
            var parttype = [];
            $("#<%=ddlPType.ClientID %> :selected").each(function () {
                parttype.push($(this).val());
            });           
            $("#hidpartytype").val(parttype);

            var salespersontype = [];
            $("#<%=ddlSType.ClientID %> :selected").each(function () {
                salespersontype.push($(this).val());
            });
            $("#hidsalesmantype").val(salespersontype);

            var dsrtype = [];
            $("#<%=ddlDsrType.ClientID %> :selected").each(function () {
              dsrtype.push($(this).val());
            });
            $("#hiddsrtype").val(dsrtype);

            var status = [];
            $("#<%=ddlStatus.ClientID %> :selected").each(function () {
                status.push($(this).val());
          });
            $("#hidstatus").val(status);


            var type = [];
            $("#<%=ddltype.ClientID %> :selected").each(function () {
                    type.push($(this).val());
                });
            $("#hidtype").val(type);

            var checked_radio = $("[id*=rblview] input:checked");
            var value = checked_radio.val();           
            $("#hidview").val(value);       


            loding();
            BindGridView();
      }

        function BindGridView() {
          
          $.ajax({
              type: "POST",
              url: "RptDSRPartyWiseAnalysis.aspx/Getpartywisedetails",
              data: '{View: "' + $("#hidview").val() + '",StateId: "' + $("#hidstate").val() + '" ,CityId: "' + $("#hidcity").val() + '" ,PartyId: "' + $("#hidparty").val() + '" ,PartyType: "' + $("#hidpartytype").val() + '" ,SalesPersonType: "' + $("#hidsalesmantype").val() + '" , DsrType: "' + $("#hiddsrtype").val() + '" , Status: "' + $("#hidstatus").val() + '", Fromdate: "' + $('#<%=frmTextBox.ClientID%>').val() + '",Todate: "' + $('#<%=toTextBox.ClientID%>').val() + '",type: "' + $("#hidtype").val() + '"}',                
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    //alert(response.d);
                },
                error: function (response) {
                    //alert(response.d);
                }
            });
        };

        function OnSuccess(response) {
            //  alert(JSON.stringify(response.d));
            // alert(response.d);
            $('div[id$="rptmain"]').show();
            var data = JSON.parse(response.d);
            //alert(data);
            //var arr1 = data.length;
            //alert(arr1);
            var table = $('#ContentPlaceHolder1_rptmain table').DataTable();
            table.destroy();
            $("#ContentPlaceHolder1_rptmain table ").DataTable({
                "order": [[0, "asc"]],

                "aaData": data,
                "aoColumns": [
            {
                "mData": "Mobile_Created_date",
                "render": function (data, type, row, meta) {

                    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                    var date = new Date(data);
                    var day = date.getDate();
                    var month = date.getMonth();
                    var year = date.getFullYear();
                    var hrs = date.getHours();
                    //alert(hrs);
                    var minute = date.getMinutes();
                    //alert(minute);
                    var second = date.getSeconds();
                    //alert(second);

                    var mname = monthNames[date.getMonth()]

                   // var fdate = day + '/' + mname + '/' + year;
                    var fdate = day + '/' + mname + '/' + year + ' ' + hrs + ':' + minute + ':' + second;
                  //  alert(fdate);
                    if (type === 'display') {
                        return $('<div>')
                           .attr('class', 'text')
                           .text(fdate)
                           .wrap('<div></div>')
                           .parent()
                           .html();

                    } else {
                        return data;
                    }
                }

            }, // <-- which values to use inside object
            { "mData": "SyncId" },
            { "mData": "Smname" },
            { "mData": "SActive" },
            { "mData": "Beat" },
            { "mData": "PartyId" },
            { "mData": "Party" },
            { "mData": "Address1" },
            { "mData": "PActive" },
            { "mData": "Stype" },
            { "mData": "productClass" },
            { "mData": "Segment" },
            { "mData": "MaterialGroup" },
            { "mData": "Value" },
            { "mData": "CompItem" },
            { "mData": "CompQty" },
            { "mData": "ComRate" },
            { "mData": "NextVisitDate" },
            { "mData": "NextVisitTime" },
            { "mData": "CompName" },
            { "mData": "BrandActivity" },
            { "mData": "MeetActivity" },
            { "mData": "OtherActivity" },
            { "mData": "OtherGeneralInfo" },
            { "mData": "RoadShow" },            
            { "mData": "Scheme" },
            { "mData": "Discount" },
            { "mData": "Remarks" },
            { "mData": "latitude" },
            { "mData": "longitude" },
            { "mData": "address" },
            {
                "mData": "Image",
                "render": function (data, type, row, meta) {
                      <%--var link =  "DistLedgerReport.aspx?DistId=" + data + "&Fromdate=" + $('#<%=txtfmDate.ClientID%>').val() + "&Todate=" + $('#<%=txttodate.ClientID%>').val() ;--%>
                    var link = "RptDSRPartyWiseAnalysis.aspx?Image=" + data;
                      if (type === 'display') {
                          return $('<a>')
                             .attr('href', link)
                             .text('View Image')
                             .wrap('<div></div>')
                             .parent()
                             .html();

                      } else {
                          return data;
                      }
                  }


            }

                ]
            });

            $('#spinner').hide();
        }

        function ValidateCheckBoxList() {
            var checkBoxList = document.getElementById("<%=trview.ClientID %>");
            var checkboxes = checkBoxList.getElementsByTagName("input");
            var isValid = false;
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    isValid = true;
                    break;
                }
                alert(isValid + "2");
            }
            args.IsValid = isValid;
        }

        function loding() {
            $('#spinner').show();
        }
    </script>
   
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
            padding: 0px;
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

        .input-group .form-control {
            height: 34px;
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
        
        @media (max-width: 600px) {
            .ui-dialog.ui-widget.ui-widget-content.ui-corner-all.ui-front.ui-draggable.ui-resizable {
                width: 100% !important;
            }
        }

    </style>
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

        function fireCheckChanged(e) {
            var ListBox1 = document.getElementById('<%= trview.ClientID %>');
             var evnt = ((window.event) ? (event) : (e));
             var element = evnt.srcElement || evnt.target;

             if (element.tagName == "INPUT" && element.type == "checkbox") {
                 __doPostBack("", "");
             }
         }
    </script>
   <script type="text/javascript">
       $(function () {
           $('[id*=ddlParty]').multiselect({
               enableCaseInsensitiveFiltering: true,
               buttonWidth: '100%',
               includeSelectAllOption: true,
               maxHeight: 200,
               width: 215,
               enableFiltering: true,
               filterPlaceholder: 'Search'
           });
       });
    </script>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            display: table;
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
        <div class="box-body">
            <!-- left column -->
            <!-- general form elements -->
            <div class="box box-primary">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box-header with-border">
                            <h3 class="box-title">DSR - Partywise Analysis</h3>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-md-8">
                                <div class="row">   
                                     <div class="col-md-4 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">View As:</label>
                                            <asp:RadioButtonList ID="rblview" RepeatDirection="Horizontal" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblview_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="SalesPerson" Text="Sales Person Wise"></asp:ListItem>
                                                <asp:ListItem Value="Party" Text="Party Wise"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                 </div>   
                                <div class="row">                               
                                     <div id="divtrview" class="col-md-6" runat="server">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>
                                            <asp:TreeView ID="trview" runat="server" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>                                            
                                        </div>
                                    </div>
                                  </div>  
                                
                                <div id="Partyview" class="row" runat="server" visible="false">
                                    <div class="col-md-3 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">State:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>  
                                            <asp:DropDownList ID="ddlState" runat="server" Width="99%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">                                                                                           
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>  
                                            <asp:DropDownList ID="ddlCity" runat="server" Width="99%" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">                                                                                       
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Party:</label>
                                             <asp:ListBox ID="ddlParty" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                           <%-- <asp:DropDownList ID="ddlParty" runat="server" CssClass="form-control" Width="99%">                                                                                          
                                            </asp:DropDownList>--%>
                                        </div>
                                    </div>
                                     <div class="col-md-3 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Type:</label>
                                           <asp:DropDownList ID="ddltype" runat="server" Width="99%"  CssClass="form-control" >    
                                                   <asp:ListItem Text="Select" Value="Select"></asp:ListItem>           
                                                   <asp:ListItem Text="Order" Value="Order"></asp:ListItem>   
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                     <div class="col-md-3 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Party Status:</label>
                                            <asp:DropDownList ID="ddlPType" runat="server"  CssClass="form-control" Width="99%">
                                                <asp:ListItem Text="All" Value="All"></asp:ListItem>   
                                                <asp:ListItem Text="Active" Value="Active" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Not Active" Value="Not Active"></asp:ListItem>                                               
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                     <div class="col-md-3 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person Status:</label>
                                            <asp:DropDownList ID="ddlSType" runat="server" CssClass="form-control" Width="99%">
                                                <asp:ListItem Text="All" Value="All"></asp:ListItem>   
                                                <asp:ListItem Text="Active" Value="Active" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Not Active" Value="Not Active"></asp:ListItem>                                               
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">DSR Type:</label>
                                            <asp:DropDownList ID="ddlDsrType" runat="server" CssClass="form-control" Width="99%">
                                                <asp:ListItem Text="All" Value="All"></asp:ListItem>   
                                                <asp:ListItem Text="Lock" Value="Lock"></asp:ListItem>
                                                <asp:ListItem Text="UnLock" Value="UnLock"></asp:ListItem>                                               
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Status:</label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" Width="99%">
                                                <asp:ListItem Text="All" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Approve" Value="Approve"></asp:ListItem>
                                                <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                                <asp:ListItem Text="Reject" Value="Reject"></asp:ListItem>
                                            </asp:DropDownList>                                           
                                             <input type="hidden" id="hiddsrtype" />
                                             <input type="hidden" id="hidstatus" />
                                             <input type="hidden" id="hidpartytype" />
                                             <input type="hidden" id="hidsalesmantype" />
                                             <input type="hidden" id="hidstate" />
                                             <input type="hidden" id="hidcity" />
                                             <input type="hidden" id="hidparty" />
                                             <input type="hidden" id="hidview" />
                                              <input type="hidden" id="hidtype" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="frmTextBox" CssClass="form-control" runat="server"
                                                Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="frmTextBox_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server"
                                                BehaviorID="frmTextBox_CalendarExtender"
                                                TargetControlID="frmTextBox"></ajaxToolkit:CalendarExtender>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="toTextBox" CssClass="form-control" runat="server"
                                                Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="toTextBox_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server"
                                                BehaviorID="toTextBox_CalendarExtender"
                                                TargetControlID="toTextBox"></ajaxToolkit:CalendarExtender>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="box-footer">
                            <input type="button" runat="server" onclick="btnSubmitfunc();" class="btn btn-primary" value="Go" /> 
                            <asp:Button type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClientClick="GetReport();" Visible="false"
                                OnClick="btnGo_Click" />
                            <asp:Button type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To CSV" class="btn btn-primary"
                                OnClick="btnExport_Click" />

                        </div>
                    </div>
                </div>

                <div id="rptmain" runat="server" style="display: none;">         
                              <div class="box-body table-responsive">
                                <asp:Repeater ID="rpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="example1" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <th>DSR Date</th>
                                                    <th>Emp Code</th>
                                                    <th>Sales Person</th>
                                                    <th>Active</th>
                                                    <th>City</th>
                                                    <th>PartyId</th>
                                                    <th>Party</th>
                                                    <th>Address</th>
                                                    <th>Active</th>
                                                    <th>Stype</th>
                                                    <th>Product Class</th>
                                                    <th>Product Segment</th>
                                                    <th>Product Group</th>
                                                    <th style="text-align:right">Amount</th>
                                                    <th>Item</th>
                                                    <th style="text-align:right">Qty</th>
                                                    <th style="text-align:right">Rate</th>
                                                    <th>NextVisitDate</th>
                                                    <th>NextVisitTime</th>
                                                    <th>Competitor Name</th>
                                                    <th>Brand Activity</th>
                                                    <th>Meet Activity</th>
                                                    <th>Other Activity</th>
                                                    <th>Other GeneralInfo</th>
                                                    <th>Road Show</th>
                                                    <th>Scheme/offers</th>
                                                     <th>Discount</th>
                                                    <th>Remark</th>
                                                    <th>View Image</th>
                                                      <th>Latitude</th>
                                                    <th>Longitude</th>
                                                    <th>Geo Address</th>
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                    <th colspan="13" style="text-align: right">Total:</th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                     <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>


                                                </tr>
                                            </tfoot>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%--<tr>
                                             <asp:HiddenField ID="linkHiddenField" runat="server" Value='<%#Eval("Image") %>' />
                                               <asp:HiddenField ID="sTypeHdf" runat="server" Value='<%#Eval("Stype") %>' />
                                           <td><%# System.Convert.ToDateTime(Eval("VisitDate")).ToString("dd/MMM/yyyy")%></td>  
                                            <asp:Label ID="VisitDateLabel" Visible="false" runat="server" Text='<%# System.Convert.ToDateTime(Eval("VisitDate")).ToString("dd/MMM/yyyy")%>'></asp:Label>                               
                                            <td><%# Eval("SyncId")%></td>  
                                            <asp:Label ID="SyncIdlevelLabel" runat="server" Visible="false" Text='<%# Eval("SyncId")%>'></asp:Label> 
                                            <td><%# Eval("Smname")%></td>  
                                            <asp:Label ID="SmnameLabel" runat="server" Visible="false" Text='<%# Eval("Smname")%>'></asp:Label>                               
                                            <td><%# Eval("Beat")%></td>
                                            <asp:Label ID="BeatLabel" runat="server" Visible="false" Text='<%# Eval("Beat")%>'></asp:Label> 
                                            <td><%# Eval("PartyId")%></td>
                                            <asp:Label ID="PartyIdLabel" runat="server" Visible="false" Text='<%# Eval("PartyId")%>'></asp:Label> 
                                            <td><%# Eval("Party")%></td>
                                            <asp:Label ID="PartyLabel" runat="server" Visible="false" Text='<%# Eval("Party")%>'></asp:Label> 
                                            <td><%# Eval("Stype")%></td>
                                            <asp:Label ID="StypeLabel" runat="server" Visible="false" Text='<%# Eval("Stype")%>'></asp:Label> 
                                            <td><%# Eval("productClass")%></td>
                                            <asp:Label ID="productClassLabel" runat="server" Visible="false" Text='<%# Eval("productClass")%>'></asp:Label> 
                                            <td><%# Eval("Segment")%></td>
                                            <asp:Label ID="SegmentLabel" runat="server" Visible="false" Text='<%# Eval("Segment")%>'></asp:Label> 
                                            <td><%# Eval("MaterialGroup")%></td>
                                            <asp:Label ID="MaterialGroupLabel" runat="server" Visible="false" Text='<%# Eval("MaterialGroup")%>'></asp:Label> 
                                            <td style="text-align:right"><%# Eval("Value")%></td>
                                            <asp:Label ID="ValueLabel" runat="server" Visible="false" Text='<%# Eval("Value")%>'></asp:Label> 
                                            <td><%# Eval("CompItem")%></td>
                                            <asp:Label ID="CompItemLabel" runat="server" Visible="false" Text='<%# Eval("CompItem")%>'></asp:Label> 
                                            <td style="text-align:right"><%# Eval("CompQty")%></td>
                                            <asp:Label ID="CompQtyLabel" runat="server" Visible="false" Text='<%# Eval("CompQty")%>'></asp:Label> 
                                            <td style="text-align:right"><%# Eval("ComRate")%></td>
                                            <asp:Label ID="ComRateLabel" runat="server" Visible="false" Text='<%# Eval("ComRate")%>'></asp:Label> 
                                            <td><%# Eval("NextVisitDate")%></td>
                                            <asp:Label ID="NextVisitDateLabel" runat="server" Visible="false" Text='<%# Eval("NextVisitDate")%>'></asp:Label> 
                                            <td><%# Eval("NextVisitTime")%></td>
                                            <asp:Label ID="NextVisitTimeLabel" runat="server" Visible="false" Text='<%# Eval("NextVisitTime")%>'></asp:Label> 
                                            <td><%# Eval("CompName")%></td>
                                            <asp:Label ID="CompNameLabel" runat="server" Visible="false" Text='<%# Eval("CompName")%>'></asp:Label> 

                                             <td><%# Eval("BrandActivity")%></td>
                                            <asp:Label ID="BrandActivityLabel" runat="server" Visible="false" Text='<%# Eval("BrandActivity")%>'></asp:Label> 
                                             <td><%# Eval("MeetActivity")%></td>
                                            <asp:Label ID="MeetActivityLabel" runat="server" Visible="false" Text='<%# Eval("MeetActivity")%>'></asp:Label> 
                                             <td><%# Eval("OtherActivity")%></td>
                                            <asp:Label ID="OtherActivityLabel" runat="server" Visible="false" Text='<%# Eval("OtherActivity")%>'></asp:Label> 
                                             <td><%# Eval("OtherGeneralInfo")%></td>
                                            <asp:Label ID="OtherGeneralInfoLabel" runat="server" Visible="false" Text='<%# Eval("OtherGeneralInfo")%>'></asp:Label> 
                                             <td><%# Eval("RoadShow")%></td>
                                            <asp:Label ID="RoadShowLabel" runat="server" Visible="false" Text='<%# Eval("RoadShow")%>'></asp:Label> 
                                             <td><%# Eval("Scheme")%></td>
                                            <asp:Label ID="SchemeLabel" runat="server" Visible="false" Text='<%# Eval("Scheme")%>'></asp:Label> 

                                            <td><%# Eval("Remarks")%></td>
                                            <asp:Label ID="RemarksLabel" runat="server" Visible="false" Text='<%# Eval("Remarks")%>'></asp:Label> 
                                             <td><asp:LinkButton ID="lnkViewDemoImg" runat="server" OnClick="lnkViewDemoImg_Click">View Image</asp:LinkButton></td>

                                        </tr>--%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
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
            $('#example1').dataTable({
                "order": [[0, "desc"]],
                "pageLength": 500,
                "footerCallback": function (tfoot, data, start, end, display) {
                    //$(tfoot).find('th').eq(0).html("Starting index is " + start);
                    var api = this.api();
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Amount'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(12).footer()).html(searchTotal);

                    if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(13).footer()).html('0.0') }
                }
            });
        });       
    </script>
</asp:Content>
