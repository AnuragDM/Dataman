<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="VarianceReportL1.aspx.cs" Inherits="AstralFFMS.VarianceReportL1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--  <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>

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
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
       <%-- function validate() {
            if ($('#<%=ListBox1.ClientID%>').val() == null) {
                errormessage("Please select Sales Person");
                return false;
            }
        }--%>
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
    <script type="text/javascript">
        //$(function () {
        //    $(".select2").select2();
        //});
    </script>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            display: table;
        }

        .input-group .form-control {
            height: 34px;
        }

        .multiselect-container > li > a {
            white-space: normal;
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

        #ContentPlaceHolder1_gvData tr th {
            border-right: 1px solid #fff;
            padding: 0 15px;
        }
    </style>
    <section class="content">
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" width="30%" height="50%" alt="Loading" /><br />
            <b>Loading Data....</b>
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
                                <%--<h3 class="box-title">Daily Variance Report L1</h3>--%>
                                <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                    <div class="col-md-2 col-sm-5 col-xs-9" hidden>
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>
                                           
                                            <asp:ListBox ID="ListBox1" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-md-2 col-sm-5 col-xs-9">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>                                           
                                            <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All" Width="779px"></asp:TreeView>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 col-sm-3 col-xs-5">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Month:</label>
                                            <asp:DropDownList ID="monthDDL" class="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-2 col-sm-3 col-xs-5">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Year:</label>
                                            <asp:DropDownList ID="yearDDL" class="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="box-footer">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary"
                                    OnClick="btnGo_Click" OnClientClick="javascript:return validate();" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                    OnClick="btnCancel_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To CSV" class="btn btn-primary"
                                    OnClick="btnExport_Click" />
                            </div>

                            <div class="box-body table-responsive">
                                <div id="noteDiv" runat="server" style="display: none;">
                                    <asp:Label ID="noteLabel" runat="server">
                                    <span style="font-weight: bold;" runat="server">Note* - </span><span style="color:red;">
                                    E/A - Dsr Entry Approved ,  E - Dsr Entry , L/A - Leave Approved ,  L - Leave , H - Holiday , Off - Week Off , E/R - Dsr Rejected , L/R - Leave Rejected,FHL - First Half Leave,FHL/A - First Half Leave Approved,FHL/R - First Half Leave Rejected,SHL - Second Half Leave,SHL/A - Second Half Leave Approved,SHL/R - Second Half Leave Rejected  </span> </asp:Label>&nbsp;&nbsp;
                                <br />
                                <asp:TextBox ID="TextBox1" Height="14px" Width="14px" runat="server" Enabled="false" BackColor="Red"></asp:TextBox>&nbsp;Holiday&nbsp;&nbsp;
                                <asp:TextBox ID="TextBox2" Height="14px" Width="14px" runat="server" Enabled="false" BackColor="#EED690"></asp:TextBox>&nbsp;Week Off&nbsp;&nbsp;
                                </div>
                                <asp:GridView runat="server" ID="gvData" Width="100%" EmptyDataText="No Records Found"
                                    AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="gvData_RowDataBound">
                                    <Columns>
                                        <asp:BoundField HeaderText="Name" DataField="Name" ItemStyle-Wrap="false"></asp:BoundField>
                                         <asp:BoundField HeaderText="Sync Id" DataField="SyncId" ItemStyle-Wrap="false"></asp:BoundField>
                                         <asp:BoundField HeaderText="Month" DataField="Month" ItemStyle-Wrap="false"></asp:BoundField>
                                         <asp:BoundField HeaderText="Year" DataField="Year" ItemStyle-Wrap="false"></asp:BoundField>
                                        <asp:BoundField DataField="EmpName" HeaderText="Emp Name" Visible="false" />
                                        <asp:BoundField DataField="d1" HeaderText=" 1"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d2" HeaderText="2"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d3" HeaderText="3"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d4" HeaderText="4"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d5" HeaderText="5"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d6" HeaderText="6"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d7" HeaderText="7"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d8" HeaderText="8"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d9" HeaderText="9"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d10" HeaderText="10"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d11" HeaderText="11"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d12" HeaderText="12"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d13" HeaderText="13"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d14" HeaderText="14"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d15" HeaderText="15"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d16" HeaderText="16"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d17" HeaderText="17"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d18" HeaderText="18"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d19" HeaderText="19"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d20" HeaderText="20"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d21" HeaderText="21"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d22" HeaderText="22"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d23" HeaderText="23"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d24" HeaderText="24"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d25" HeaderText="25"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d26" HeaderText="26"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d27" HeaderText="27"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d28" HeaderText="28"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d29" HeaderText="29"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d30" HeaderText="30"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="d31" HeaderText="31"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Enter" HeaderText="Entered">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Approve" HeaderText="Approved">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Beat Name" ItemStyle-Width="30%" ItemStyle-Wrap="true" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSMId" runat="server" Text='<%# Eval("SMId") %>' Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#EED690" />
                                    <PagerStyle HorizontalAlign="Center" CssClass="GridPager" BackColor="#3c8dbc" />
                                    <SelectedRowStyle BackColor="#008A8C" HorizontalAlign="Center" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#3c8dbc" HorizontalAlign="Right" Font-Bold="True" ForeColor="white" />
                                    <AlternatingRowStyle BackColor="#e8e8e8" />
                                    <AlternatingRowStyle CssClass="tbl1" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
