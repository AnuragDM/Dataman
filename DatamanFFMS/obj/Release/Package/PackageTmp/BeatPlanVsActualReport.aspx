<%@ Page Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="BeatPlanVsActualReport.aspx.cs" Inherits="AstralFFMS.BeatPlanVsActualReport" %>

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

         #tourvsActTable_wrapper .row {
            margin-right: 0px !important;
            margin-left: 0px !important;
        }

            #tourvsActTable_wrapper .row .col-sm-12 {
                overflow-x: scroll !important;
                padding-left: 0px !important;
                margin-bottom: 10px;
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

    <script type="text/javascript">

        function btnSubmitfunc() {

            var selectedvalue = [];
            $("#<%=trview.ClientID %> :checked").each(function () {
                  selectedvalue.push($(this).val());
              });
              if (selectedvalue == "") {
                  errormessage("Please Select Sales Person");
                  return false;
              }

              loding();
              BindGridView();
          }

          function BindGridView() {
              $.ajax({
                  type: "POST",
                  url: "BeatPlanVsActualReport.aspx/GetBeatVsActual",
                  data: '{ Fromdate: "' + $('#<%=txtfmDate.ClientID%>').val() + '",Todate: "' + $('#<%=txttodate.ClientID%>').val() + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
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
                "order": [[0, "desc"]],

                "aaData": data,
                "aoColumns": [
            {
                "mData": "Date",
                "render": function (data, type, row, meta) {

                    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                    var date = new Date(data);
                    var day = date.getDate();
                    var month = date.getMonth();
                    var year = date.getFullYear();

                    var mname = monthNames[date.getMonth()]

                    var fdate = day + '/' + mname + '/' + year;

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
            { "mData": "Weekday" },
            { "mData": "SalesRepName" },
            { "mData": "SyncId" },
            { "mData": "BeatPlanBeat" },
            { "mData": "BeatPlansyncid" },
            { "mData": "VisitBeat" },
            { "mData": "VisitBeatsyncid" },
            { "mData": "Remark" }
                ]
            });

            $('#spinner').hide();
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
                                    <h3 class="box-title">
                                        <asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>

                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                                <%--  <div class="col-md-3 col-sm-6 col-xs-7">--%>
                                                <%--<div class="col-lg-4 col-md-4 col-sm-4 col-xs-9">--%>
                                              <%--  <div class="form-group" hidden>
                                                    <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                    <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                </div>--%>
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                    <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged" ShowCheckBoxes="All"></asp:TreeView>

                                                </div>
                                            </div>
                                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                                <div id="DIV1" class="form-group">
                                                    <label for="exampleInputEmail1">From Date:</label>
                                                    <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                                </div>
                                            </div>
                                            <%-- <div class="col-md-3 col-sm-6 col-xs-7">--%>
                                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">To Date:</label>
                                                    <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <%--  <div class="row">                                 
                                  
                                </div>--%>
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
                                                    <asp:DropDownList ID="ddlFilter" runat="server" class="form-control">
                                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                        <asp:ListItem Text="Variance" Value="Variance"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box-footer">
                                    <input type="button" runat="server" onclick="btnSubmitfunc();" class="btn btn-primary" value="Go" />
                                    <asp:Button type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClientClick="loding()"
                                        OnClick="btnGo_Click" Visible="false" />
                                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Cancel_Click" />
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                        OnClick="btnExport_Click" />
                                    <%-- <input style="margin-right: 5px;" type="button" id="Go" value="Go" class="btn btn-primary" onc onclick="GetReport();" />--%>
                                </div>

                            </div>
                        </div>
                        <div id="rptmain" runat="server">
                            <div class="box-header table-responsive">
                                <asp:Repeater ID="tourvsactrpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="tourvsActTable" class="table table-bordered table-striped rpttable">
                                            <thead>
                                                <tr>
                                                    <%-- <th style="text-align: left; width: 6%">S.No</th>--%>
                                                    <th style="text-align: left;">Date</th>
                                                    <%-- <th style="text-align: left; width: 13%">Day</th>--%>
                                                    <th style="text-align: left;">Day</th>
                                                    <th style="text-align: left;">Sales Person</th>
                                                    <th style="text-align: left;">Emp. Sync Id</th>
                                                    <th style="text-align: left;">Planned Beat</th>
                                                    <th style="text-align: left;">Planned Beat Sync Id</th>
                                                    <th style="text-align: left;">Visited Beat</th>
                                                    <th style="text-align: left;">Visited Beat Sync Id</th>
                                                    <th style="text-align: left;">Remark</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%-- <tr>                                         
                                            <td style="text-align: left; width: 5%"><%#Convert.ToDateTime(Eval("Date")).ToString("dd/MMM/yyyy") %></td>
                                            <asp:Label ID="DateLabel" runat="server" Visible="false" Text='<%#Convert.ToDateTime(Eval("Date")).ToString("dd/MMM/yyyy") %>'></asp:Label>
                                            <td style="text-align: left; width: 13%"><%#Eval("Date","{0:dddd}") %></td>
                                            <asp:Label ID="DayLabel" runat="server" Visible="false" Text='<%#Eval("Date","{0:dddd}") %>'></asp:Label>
                                            <th style="text-align: left; width: 13%"><%#Eval("SalesRepName") %></th>
                                            <asp:Label ID="SalesRepNameLabel" runat="server" Visible="false" Text='<%# Eval("SalesRepName")%>'></asp:Label>
                                            <th style="text-align: left; width: 13%"><%#Eval("SyncId") %></th>
                                            <asp:Label ID="SyncIdLabel" runat="server" Visible="false" Text='<%# Eval("SyncId")%>'></asp:Label>
                                            <th style="text-align: left; width: 18%"><%#Eval("BeatPlanBeat") %></th>
                                            <asp:Label ID="BeatPlanBeatLabel" runat="server" Visible="false" Text='<%# Eval("BeatPlanBeat")%>'></asp:Label>
                                            <th style="text-align: left; width: 18%"><%#Eval("BeatPlansyncid") %></th>
                                            <asp:Label ID="BeatPlansyncidLabel" runat="server" Visible="false" Text='<%# Eval("BeatPlansyncid")%>'></asp:Label>
                                            <td style="text-align: left; width: 18%"><%#Eval("VisitBeat") %></td>  
                                            <asp:Label ID="VisitBeatLabel" runat="server" Visible="false" Text='<%# Eval("VisitBeat")%>'></asp:Label> 
                                            <td style="text-align: left; width: 18%"><%#Eval("VisitBeatsyncid") %></td>      
                                            <asp:Label ID="VisitBeatsyncidLabel" runat="server" Visible="false" Text='<%# Eval("VisitBeatsyncid")%>'></asp:Label>                        
                                            <td style="text-align: left; width: 13%"><%#Eval("Remark") %></td>
                                            <asp:Label ID="RemarkLabel" runat="server" Visible="false" Text='<%# Eval("Remark")%>'></asp:Label>
                                        </tr>--%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
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
