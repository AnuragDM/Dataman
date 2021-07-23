<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="LastLogin.aspx.cs" Inherits="AstralFFMS.LastLogin" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--        <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
  <%--  <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
 <%--     <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
               
            });
        });
    </script>--%>
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
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Last Login Report</h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                <div class="col-md-3 col-sm-5">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Application:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="DdlSalesPerson" width="220px" CssClass="form-control" runat="server" OnSelectedIndexChanged="DdlSalesPerson_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="Distributor" Text="Distributor"></asp:ListItem>
                                              <asp:ListItem Value="Field" Text="Field"></asp:ListItem>
                                              <asp:ListItem Value="Manager" Text="Manager"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                      <div class="form-group">
                                        <label for="exampleInputEmail1">Month:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="DropDownList1" width="220px" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" >
                                             <asp:ListItem Value="01" Text="Jan"></asp:ListItem>
                                              <asp:ListItem Value="02" Text="Feb"></asp:ListItem>
                                              <asp:ListItem Value="03" Text="Mar"></asp:ListItem>
                                              <asp:ListItem Value="04" Text="Apr"></asp:ListItem>
                                              <asp:ListItem Value="05" Text="May"></asp:ListItem>
                                              <asp:ListItem Value="06" Text="Jun"></asp:ListItem>
                                              <asp:ListItem Value="07" Text="Jul"></asp:ListItem>
                                              <asp:ListItem Value="08" Text="Aug"></asp:ListItem>
                                              <asp:ListItem Value="09" Text="Sep"></asp:ListItem>
                                              <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                                              <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                                              <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
                                        </asp:DropDownList>
                                          
                                    </div>
                                </div></div>
                            </div>
                           <%-- <div class="box-footer">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary"
                                    OnClick="btnGo_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                    OnClick="btnCancel_Click" />
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="box-body" id="rptmain" runat="server" style="display: block;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <div style="float: right">
                           <%--     <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />--%>

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>S.No</th>
                                                <th>Sales Person</th>
                                                <th>Application</th>
                                                 <th>Version</th>
                                                <th>Login Date & Time</th>
                                               
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr >
                                        <td><%# Container.ItemIndex + 1 %></td>
                                        <td><%#Eval("EmpName") %></td>
                                          <td><%#Eval("App") %></td>
                                         <td><%#Eval("Version") %></td>
                                        <td> <%#Eval("LastLogin","{0:dd/MMM/yyyy  HH:mm:ss}") %></td>
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
</asp:Content>
