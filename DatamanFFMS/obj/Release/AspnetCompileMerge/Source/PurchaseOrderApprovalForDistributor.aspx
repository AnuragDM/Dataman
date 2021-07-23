<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="PurchaseOrderApprovalForDistributor.aspx.cs" Inherits="AstralFFMS.PurchaseOrderApprovalForDistributor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function DoNav(POID) {
            if (POID != "") {
                __doPostBack('', POID)
            }
        }
    </script>
    <section class="content">
        <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Orders</h3>
                        </div>

                         <div class="box-body">
                                    <div class="col-md-12">
                                        <div class="col-md-12 paddingleft0">
                                            <div id="DIV1" class="form-group col-md-3">
                                                <label for="exampleInputEmail1">From Date:</label>
                                                <asp:TextBox ID="txtmDate" runat="server" CssClass="form-control" BackColor="White"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtmDate" runat="server" />
                                            </div>
                                            <div class="form-group col-md-3 ">
                                                <label for="exampleInputEmail1">To Date:</label>
                                                <asp:TextBox ID="txttodate" runat="server" CssClass="form-control" BackColor="White"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                            </div>
                                           <%-- <div class="form-group col-md-3 ">
                                                <label for="exampleInputEmail1">Distributor:</label>
                                                <asp:DropDownList ID="ddlDist" runat="server" width="100%" CssClass="form-control select2"></asp:DropDownList>
                                            </div>--%>
                                             <div class="form-group col-md-3 ">
                                                <label for="exampleInputEmail1">Status:</label>
                                                <asp:DropDownList ID="ddlStatus" runat="server" width="100%" CssClass="form-control">
                                                   <%-- <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>--%>
                                                      <asp:ListItem Text="Open" Value="P" Selected="True"></asp:ListItem>
                                                      <asp:ListItem Text="Processed" Value="M"></asp:ListItem>
                                                      <asp:ListItem Text="CustomerCanceled" Value="R"></asp:ListItem>
                                                      <asp:ListItem Text="CompanyCanceled" Value="C"></asp:ListItem>  
                                                        <asp:ListItem Text="OnHold" Value="H"></asp:ListItem>  
                                                      <asp:ListItem Text="All" Value="A" ></asp:ListItem>                                                    
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group col-md-3">
                                                <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                                <asp:Button type="button" ID="btnGo" runat="server" Text="Go" style="padding:3px 7px;" class="btn btn-primary" OnClick="btnGo_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- /.box-header -->
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <th>Date</th>
                                                        <th>DocID</th>
                                                        <th>Item Group & Dispatch To</th>
                                                        <th>Order Status</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr onclick="DoNav('<%#Eval("POrdId") %>');">
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("POrdId") %>' />
                                                <td><%#String.Format("{0:dd/MM/yyyy}", Eval("VDate"))%></td>
                                                <td><%#Eval("PODocId") %></td>
                                                <td><%#Eval("IGandDispatchTo") %></td>
                                                <td><%#Eval("OrderStatus") %></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>     </table>       
                                        </FooterTemplate>

                                    </asp:Repeater>
                                </div>
                        <!-- /.box-body -->
                    </div>
                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>

        </div>
    </section>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
  <script type="text/javascript">
      function load1() {
          $("#example1").DataTable();

      }

      $(function load2() {
          $("#example1").DataTable({
              "order": [[1, "desc"]]
          });
      });
      $(window).load(function () {

          Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(load1);
          Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(load2);

      });


    </script>
</asp:Content>
