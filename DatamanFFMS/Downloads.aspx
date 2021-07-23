<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="Downloads.aspx.cs" Inherits="AstralFFMS.Downloads" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
         <div class="box-body" id="rptmain" runat="server" >
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Document List</h3>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="downloadrpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>SNo.</th>
                                                <th>Title</th>
                                                <th>Link Url</th>
                                                  <th style="visibility:hidden;">Download</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("id") %>' />
                                        <asp:HiddenField ID="linkHiddenField" runat="server" Value='<%#Eval("LinkURL") %>' />
                                          <td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>
                                        <td><%#Eval("Title") %></td>
                                          <td><%#Eval("LinkURL") %></td>
                                        <td style="text-align:center;"><asp:LinkButton ID="lnkdownload" runat="server" OnClick="lnkdownload_Click" >Download</asp:LinkButton></td>
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
        $(function () {
            $("#example1").DataTable();
        });
    </script>
</asp:Content>
