<%@ Page Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="BeatPlanVsActualReport_V2.aspx.cs" Inherits="AstralFFMS.BeatPlanVsActualReport_V2" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=ListBox1]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '300px',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
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
                 autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "error"
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
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
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
    </script>
    <style type="text/css">
        .input-group .form-control {
            height: 34px;
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


           .button1 {
box-shadow: 0px 2px 4px 2px #888888;
margin-left: 10px;
}

        .table1 {
font-weight:bold;
}

        input[type=checkbox] {
   margin-right: 12px !important;
    margin-left: 7px !important;
}

        h2 {
font-size: 20px !important;
font-weight: 600 !important;
margin-left: 13px !important;
}

    </style>
    <%-- <script type="text/javascript">
         $(function () {
             $("#tourvsActTable").DataTable();
         });
    </script>--%>
     <script type="text/javascript">
         //$(function () {
         //    $("#tourvsActTable").DataTable({
         //        "order": [[0, "desc"]]
         //    });
         //});

         function fireCheckChanged(e) {
             var ListBox1 = document.getElementById('<%= trview.ClientID %>');
             var evnt = ((window.event) ? (event) : (e));
             var element = evnt.srcElement || evnt.target;

             if (element.tagName == "INPUT" && element.type == "checkbox") {
                 __doPostBack("", "");
             }
         }

         function loding() {
             $('#spinner').show();
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
                                    <%--<h3 class="box-title">Beat Plan VS Actual</h3>--%>
                                     <img id="img-Header" src="img/check-list.png" style="width: 47px; height: 47px;" />
                                    <h2 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h2>    

                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="col-md-8">
                                        <div class="row">
                                           <div class="col-md-6">
                                           <%--  <div class="col-md-3 col-sm-6 col-xs-7">--%>
                                             <%--<div class="col-lg-4 col-md-4 col-sm-4 col-xs-9">--%>
                                                <div class="form-group" hidden>
                                                    <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                                     
                                                    <asp:ListBox ID="ListBox1" CssClass="form-control" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                </div>
                                                 <div class="form-group">
                                                    <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                                     
                                                    <asp:TreeView ID="trview" runat="server" CssClass="table-responsive table1" ExpandDepth="10" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged"  ShowCheckBoxes="All"></asp:TreeView>

                                                </div>
                                            </div>
                                        </div>
                              <div class="row">                                 
                                   <%-- <div class="col-md-3 col-sm-6 col-xs-7">--%>
                                   <div class="col-lg-4 col-md-4 col-sm-4 col-xs-9">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                        </div>
                                    </div>                                 
                                      <%-- <div class="col-md-3 col-sm-6 col-xs-7">--%>
                                   <div class="col-lg-4 col-md-4 col-sm-4 col-xs-9">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                        </div>
                                    </div>

                                </div>
                                        <div class="row" id="frmdate" runat="server" hidden>
                                            <div class="col-md-2 col-sm-3 col-xs-7">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Month:</label>
                                                    <asp:DropDownList ID="ddlMonthSecSale" class="form-control" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-2 col-sm-3 col-xs-7" hidden>
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Year:</label>
                                                    <asp:DropDownList ID="ddlYearSecSale" class="form-control" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3 col-sm-3 col-xs-7" hidden>
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Filter:</label>
                                                    <asp:DropDownList ID="ddlFilter" runat="server"  class="form-control">
                                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                        <asp:ListItem Text="Variance" Value="Variance"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box-footer">
                                   
                                      <asp:Button Style="margin-right: 5px;box-shadow: 0px 2px 4px 2px #888888;
margin-left: 10px;" type="button" ID="btnExport" runat="server" Text="Generate" ToolTip="Export To Excel" class="btn btn-primary"
                                    OnClick="btnExport_Click"/>
                                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary button1" OnClick="Cancel_Click" />
                                  
                                    <%-- <input style="margin-right: 5px;" type="button" id="Go" value="Go" class="btn btn-primary" onc onclick="GetReport();" />--%>
                                </div>
                                 
                            </div>
                        </div>
                       
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

