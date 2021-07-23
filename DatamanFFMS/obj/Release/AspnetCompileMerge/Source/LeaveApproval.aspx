<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="LeaveApproval.aspx.cs" Inherits="AstralFFMS.LeaveApproval" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
     <script type="text/javascript">
         $(function () {
             $("#example1").DataTable({
                 "order": [[0, "desc"]]
             });
         });
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
            /*display: table;*/
        }
    </style>
      <script type="text/javascript">
          function DoNav(msgUrl) {
              if (msgUrl != "") {
                  window.location.href = msgUrl + "&Page=" + 'LVRAPPROVAL';
              }
          }
    </script>
    <section class="content">

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
                                <h3 class="box-title">Leave Approval</h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                <div class="col-md-3 col-sm-5">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="DdlSalesPerson" width="220px" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div></div>
                            <div class="box-footer">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary"
                                    OnClick="btnGo_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                    OnClick="btnCancel_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="box-body" id="rptmain" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Leave Approval List</h3>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Application Date</th>
                                                <th>Document No.</th>
                                                <th>From Date</th>
                                                <th>To Date</th>
                                                <th>Sales Person</th>
                                                <th>Days</th>
                                                <th>Reason</th>
                                               <%-- <th>Remark</th>--%>
                                                <th>Status</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#"LeaveRequest.aspx?SMId="+Eval("SMId")+"&LVRQId="+Eval("LVRQId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("LVRQId") %>' />
                                        <td><%#Eval("VDate","{0:dd/MMM/yyyy}") %></td>
                                        <td><%#Eval("LVRDocId") %></td>
                                        <td><%#Eval("FromDate","{0:dd/MMM/yyyy}") %></td>
                                        <td><%#Eval("ToDate","{0:dd/MMM/yyyy}") %></td>
                                        <td><%#Eval("SMName") %></td>
                                        <td><%#Eval("NoOfDays") %></td>
                                        <td><%#Eval("Reason") %></td>
                                        <%--<td><%#Eval("AppRemark") %></td>--%>
                                        <td><%#Eval("AppStatus") %></td>
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
