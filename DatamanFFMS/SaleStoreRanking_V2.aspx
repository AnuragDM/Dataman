﻿<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="SaleStoreRanking_V2.aspx.cs" Inherits="AstralFFMS.SaleStoreRanking_V2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <script type="text/javascript">
        var V = "";and
        function Successmessage(V) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>

      <script type="text/javascript">
        $(function () {
            $('[id*=lstAreaBox]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',GetMonthlyItemSaleGetMonthlyItemSale
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $('[id*=Lstbeatbox]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',GetMonthlyItemSaleGetMonthlyItemSale
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
    </script>

     <script type="text/javascript">
        $(function () {
            $('[id*=lstretailer]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',GetMonthlyItemSaleGetMonthlyItemSale
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $('[id*=ListBox1]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
    </script>
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


          function loding() {
              $('#spinner').show();
          }
    </script>

    <%-- <script type="text/javascript">
         function fireCheckChanged(e) {
             var ListBox1 = document.getElementById('<%= trview.ClientID %>');
             var evnt = ((window.event) ? (event) : (e));
             var element = evnt.srcElement || evnt.target;

             if (element.tagName == "INPUT" && element.type == "checkbox") {
                 __doPostBack("", "");
             }
         }
         //$(document).ready(function () {
         //    //Hide the div          
         //    $('div[id$="rptDiv"]').hide();
         //    //conversely do the following to show it again if needed later
         //    //$('#showdiv').show();
         //});

</script>--%>
    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
                "order": [[7, "desc"]]
            });
        });
   
        function validatesale() {

            
            var selectedvalue = [];
            $("#<%=trview.ClientID %> :checked").each(function () {
                selectedvalue.push($(this).val());
            });
            if (selectedvalue == "") {
                errormessage("Please Select Sales Person");
                return false;
            }
           

            if ($('#<%=txtfmDate.ClientID%>').val() == "") {
                errormessage("Please Select From Date");
                return false;
            }
            if ($('#<%=txttodate.ClientID%>').val() == "") {
                errormessage("Please Select To Date");
                return false;
            }
        }

        function validatestore() {


            <%--var selectedvalue = [];
            $("#<%=trview.ClientID %> :checked").each(function () {
                selectedvalue.push($(this).val());
            });
            if (selectedvalue == "") {
                errormessage("Please Select Sales Person");
                return false;
            }--%>
            <%--  if ($('#<%=ddlretailer.ClientID%>').val() == "0") {
                errormessage("Please Select Retailer.");
                return false;
            }--%>
            var selectedValues = [];
            $("#<%=lstAreaBox.ClientID %> :selected").each(function () {
                 selectedValues.push($(this).val());
             });
            $("#hidarea").val(selectedValues);
             if (selectedValues == "") {
                 errormessage("Please Select Area");
                 return false;
             }

            var selectedValues = [];
            $("#<%=Lstbeatbox.ClientID %> :selected").each(function () {
                 selectedValues.push($(this).val());
             });
            $("#hidbeat").val(selectedValues);
            //if (selectedValues == "") {
            //    errormessage("Please Select Beat");
            //    return false;
            //}

            var selectedValues = [];
            $("#<%=lstretailer.ClientID %> :selected").each(function () {
                 selectedValues.push($(this).val());
             });
            $("#hidretailer").val(selectedValues);
             // if (selectedValues == "") {
             //    errormessage("Please Select Retailer");
             //    return false;
             //}

            if ($('#<%=txtfmDate.ClientID%>').val() == "") {
                errormessage("Please Select From Date");
                return false;
            }
            if ($('#<%=txttodate.ClientID%>').val() == "") {
                errormessage("Please Select To Date");
                return false;
            }
        }
    </script>
      <script type="text/javascript">
          //$(function () {
          //    $('[id*=listretailer]').multiselect({
          //        enableCaseInsensitiveFiltering: true,
          //        //buttonWidth: '200px',
          //        buttonWidth: '100%',
          //        includeSelectAllOption: true,
          //        maxHeight: 200,
          //        width: 215,
          //        enableFiltering: true,
          //        filterPlaceholder: 'Search'
          //    });
          //});
    </script>
    <style type="text/css">
        .input-group .form-control {
            height: 34px;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            /*display: table;*/
        }

        .multiselect-container > li > a {
            white-space: normal;
        }

        .multiselect-container > li {
            width: 212px;
        }

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }

          input[type=checkbox], input[type=radio] {
            margin-right: 12px ;
            margin-left: 12px ;
        }


        .button1 {
            box-shadow: 0px 2px 4px 2px #888888;
            margin-left: 10px;
        }

         .box-header img {
            margin-top: -7px;
        }

        h2 {
            font-size: 20px !important;
            font-weight: 600 !important;
            margin-left: 13px !important;
        }
    </style>
    <section class="content">
         <div id="spinner" runat="server" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
        </div>
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                  <img src="img/Location/military-rank.png" />
                               <%-- <h3 class="box-title">Sale And Store Ranking Report</h3>--%>
                                <h2 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h2>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">     
                                     <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <b><asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All" ></asp:TreeView></b>
                                        </div>
                                    </div>
                                     <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Area:</label>
                                            &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:ListBox ID="lstAreaBox" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="lstAreaBox_SelectedIndexChanged" class="form-control"></asp:ListBox>
                                     <input type="hidden" id="hidarea"/>
                                              <input type="hidden" id="hidbeat"/>
                                            <input type="hidden" id="hidretailer"/>
                                               <%--    <input type="hidden" id="hidprdgrp" />
                                          
                                            <input type="hidden" id="hidyear" />
                                            <input type="hidden" id="hidmonth" />
                                            <input type="hidden" id="hidview" />
                                            <input type="hidden" id="hidmonthtext" />--%>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Beat:</label>
                                            
                                            <asp:ListBox ID="Lstbeatbox" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="Lstbeatbox_SelectedIndexChanged" class="form-control"></asp:ListBox>

                                        </div>
                                    </div>
                                    </div>

                                <div class="row">
                                  <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Retailer:</label>
                                            
                                            <asp:ListBox ID="lstretailer" runat="server" SelectionMode="Multiple" AutoPostBack="true" class="form-control"></asp:ListBox>
                                            <%--<asp:DropDownList runat="server" CssClass="form-control" ID="ddlretailer"></asp:DropDownList>--%>
                                              <%--<asp:ListBox ID="listretailer" runat="server" SelectionMode="Multiple"
                                                    OnSelectedIndexChanged="listretailer_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>--%>
                                        </div>
                                    </div>
                                      <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                        </div>
                                    </div>                                   
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                        </div>
                                    </div>
                                </div>
                              
                                <div style="height:20px"></div>

                                <div class="row">
                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnsalespersonranking" runat="server" Text="Sales Person Ranking Excel" class="btn btn-primary button1"
                                            OnClick="btnsalespersonranking_Click"  OnClientClick="javascript:return validatesale(); "/>
                                          <asp:Button Style="margin-right: 5px;" type="button" ID="btnstoreranking" runat="server" Text="Store Ranking Excel" class="btn btn-primary button1"
                                            OnClick="btnstoreranking_Click"   OnClientClick="javascript:return validatestore();"/>
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary button1"
                                            OnClick="btnCancel_Click" />
                                   <%--     <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                            OnClick="btnExport_Click" Visible="false"/>--%>
                                    </div>
                                </div>
                                <br />

                                <div id="rptDiv" runat="server" class="box-body table-responsive" visible="false">
                                    <asp:Repeater ID="leavereportrpt" runat="server">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                         <th>S.No.</th>
                                                        <th>Salesperson/Retailer</th>
                                                         <th>Total Party Visited</th>
                                                         <th>Total Party Orderd</th>
                                                        <th>Total Order</th>
                                                        <th>Failed Visit</th>
                                                          <th>Demo</th>
                                                         <th>Efficiency (in %age)</th>
                                                         <%--<th>To Date</th>--%>
                                                        <%--<th>Reason</th>
                                                        <th>Status</th>
                                                        <th>Approved By</th>           --%>                                            
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Eval("SrNo") %>
                                                     <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="lblsrno" runat="server" Visible="false" Text='<%# Eval("SrNo")%>'></asp:Label>
                                            <%-- End --%>
                                                </td>
                                                <td><%#Eval("SMName") %>
                                                     <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="smnameLabel" runat="server" Visible="false" Text='<%# Eval("SMName")%>'></asp:Label>
                                            <%-- End --%>
                                                </td>
                                                <td><%#Eval("TotalVisitedParty") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="syncIdLabel" runat="server" Visible="false" Text='<%# Eval("TotalVisitedParty")%>'></asp:Label>
                                            <%-- End --%>
                                                </td>
                                                <td><%#Eval("orderdparty") %>
                                                    <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Eval("orderdparty")%>'></asp:Label>
                                            <%-- End --%>
                                                </td>
                                                <td><%#Eval("OrderAmount") %>
                                                      <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="nofdaysLabel" runat="server" Visible="false" Text='<%# Eval("OrderAmount")%>'></asp:Label>
                                            <%-- End --%>
                                                </td>
                                                <td><%#Eval("FailedVisit") %>
                                                     <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="fromDateLabel" runat="server" Visible="false" Text='<%# Eval("FailedVisit")%>'></asp:Label>
                                            <%-- End --%>
                                                </td>
                                                <td><%#Eval("demo") %>
                                                     <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="lbldemo" runat="server" Visible="false" Text='<%# Eval("demo")%>'></asp:Label>
                                            <%-- End --%>
                                                </td>
                                                <td><%#Eval("efficiency") %>
                                                     <%-- Added 06-06-2016 - Abhishek --%>
                                        <asp:Label ID="EfficiencyLabel" runat="server" Visible="false" Text='<%# Eval("efficiency")%>'></asp:Label>
                                            <%-- End --%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>     </table>       
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                                <br />
                                <div>
                                    <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </section>
</asp:Content>
