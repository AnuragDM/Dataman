<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="SalesPersonLedgerReport.aspx.cs" Inherits="AstralFFMS.SalesPersonLedgerReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
      <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    
      <script type="text/javascript">
          $(function () {
              $('[id*=salespersonListBox]').multiselect({
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
    </script>
    <style type="text/css">
        .table-responsive{
            border: 1px solid #fff;
        }
         .multiselect-container > li > a {
            white-space: normal;
        }
           .multiselect-container.dropdown-menu {
            width: 100% !important;
        }

          .input-group .form-control {
            height:34px;
        }
    </style>
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
            $("#example1").DataTable();
            $("#example2").DataTable();
        });
    </script>
       <section class="content">
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
                                <h3 class="box-title">Sales Person Ledger </h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                    <%--<div class="col-md-3 col-sm-5 col-xs-9">--%>
                                    <div class="col-md-4 col-sm-6 col-xs-12">
                                        <div class="form-group" hidden>
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                            
                                            <asp:ListBox ID="salespersonListBox" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                            
                                            <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                  <%--  <div class="col-md-2 col-sm-4 col-xs-9">--%>
                                     <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                        </div>
                                    </div>
                                   <%-- <div class="col-md-2 col-sm-4 col-xs-9">--%>
                                     <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <div class="box-footer">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary"
                                    OnClick="btnGo_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                    OnClick="btnCancel_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                    OnClick="btnExport_Click" />
                            </div>                          

                            <div class="box-body table-responsive">
                                <asp:Repeater ID="salesreportrpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="example1" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                   <%-- <th>SNo.</th>--%>
                                                    <th>Sales Person</th>
                                                     <th>Sync Id</th>
                                                   <%-- <th>Opening Balance</th>--%>
                                                    <th style="text-align:right">Debit</th>
                                                    <th style="text-align:right">Credit</th>
                                                    <th style="text-align:right">Closing Balance</th>
                                                    <th style="visibility: hidden;">Details</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("SMId") %>' />
                                           <%-- <td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>--%>
                                            <td><%#Eval("smname") %></td>
                                             <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="smnameLabel" runat="server" Visible="false" Text='<%# Eval("smname")%>'></asp:Label>
                                            <%-- End --%>
                                            <td><%#Eval("SyncId") %></td>  
                                             <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="SyncIdLabel" runat="server" Visible="false" Text='<%# Eval("SyncId")%>'></asp:Label>
                                            <%-- End --%>                                         
                                            <td style="text-align: right;"><%#Eval("dr") %></td>
                                             <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="drLabel" runat="server" Visible="false" Text='<%# Eval("dr")%>'></asp:Label>
                                            <%-- End --%>
                                            <td style="text-align: right;"><%#Convert.ToDecimal(Eval("Cr")) > 0 ? Eval("Cr") : (-1)*Convert.ToDecimal(Eval("Cr"))  %></td> 
                                             <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="CrLabel" runat="server" Visible="false" Text='<%#Convert.ToDecimal(Eval("Cr")) > 0 ? Eval("Cr") : (-1)*Convert.ToDecimal(Eval("Cr"))  %>'></asp:Label>
                                            <%-- End --%>  
                                            <td style="text-align: right;"><%#Convert.ToDecimal(Eval("cBalance")) > 0 ? Eval("cBalance")+ " Dr"  : (-1)*Convert.ToDecimal(Eval("cBalance")) + " Cr" %></td>
                                             <%-- Added 06-06-2016 - Nishu --%>
                                        <asp:Label ID="cBalanceLabel" runat="server" Visible="false" Text='<%#Convert.ToDecimal(Eval("cBalance")) > 0 ? Eval("cBalance")+ " Dr"  : (-1)*Convert.ToDecimal(Eval("cBalance")) + " Cr" %>'></asp:Label>
                                            <%-- End --%>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Details</asp:LinkButton></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <br />
                            <div id="detailDiv" runat="server" style="display: none;">
                                <div class="box-body table-responsive">
                                    <asp:Label ID ="lblSmid"  runat="server"></asp:Label>
                                    <asp:Repeater ID="detailSalesrpt" runat="server">
                                        <HeaderTemplate>
                                            <table id="example2" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <th>SNo.</th>
                                                        <th>Date</th>
                                                        <th>VNo.</th>
                                                        <th>Narration</th>
                                                        <th style="text-align:right">Debit</th>
                                                        <th style="text-align:right">Credit</th>
                                                        <th style="text-align:right">Balance</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>
                                                <td><%#System.Convert.ToDateTime(Eval("vdate")).ToString("dd/MMM/yyyy") %></td>
                                                <td><%#Eval("SMLdocId") %></td>
                                                <td><%#Eval("Narration") %></td>
                                                <td><%#Eval("amtDr") %></td>
                                                <td><%#Convert.ToDecimal(Eval("amtCr")) > 0 ? Eval("amtCr") : (-1)*Convert.ToDecimal(Eval("amtCr")) %></td>
                                                <td style="text-align: right;"><%#Eval("Balance") %></td>
                                            </tr>
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
        </div>
    </section>
</asp:Content>
