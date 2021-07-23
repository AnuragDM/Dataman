<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MeetPlanListL2.aspx.cs" Inherits="AstralFFMS.MeetPlanListL2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <style type="text/css">
  .modalBackground
{
background-color: Gray;
filter: alpha(opacity=80);
opacity: 0.8;
z-index: 10000;
}
        .table-responsive {
            border: 1px solid #fff;
        }
</style>


    <section class="content">
         <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Meet Plan List</h3>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                        <div class="col-lg-12 col-md-12 col-sm-8 col-xs-10">
                          <div class="col-md-12 paddingleft0">

                              <div    class="form-group col-md-4">
                                    <label for="exampleInputEmail1">User:</label>
                             <asp:DropDownList ID="ddlunderUser"  runat="server" CssClass="form-control"></asp:DropDownList>
                                   
                                </div> 

                                <div    class="form-group col-md-3">
                                    <label for="exampleInputEmail1">Status:</label>
                              <asp:DropDownList ID="ddlstatus" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                  <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                   <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
                              </asp:DropDownList>
                               
                                </div> 

                            <div id="DIV1"   class="form-group col-md-2">
                                    <label for="exampleInputEmail1">From Date:</label>
                               <asp:TextBox id="txtmDate" runat="server" style="background: white;" CssClass="form-control"></asp:TextBox>
                                 <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange"  Format="dd/MMM/yyyy" TargetControlID="txtmDate" runat="server" />
                                </div> 
                                <div  class="form-group col-md-2">
                                    <label for="exampleInputEmail1">To Date:</label>
                               <asp:TextBox id="txttodate" runat="server" style="background: white;" CssClass="form-control"></asp:TextBox>
                                 <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange"  Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                </div> 
                                <div  class="form-group col-md-1">
                                       <label for="exampleInputEmail1" style="display:block; visibility:hidden">zkjfhksj</label>
                                  <asp:Button type="button" ID="btnGo" runat="server" style="padding: 3px 14px;" Text="Go" class="btn btn-primary go-button-dsr" OnClick="btnGo_Click" />
                                </div> 
                                </div>   
                                </div></div> 
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Sr.No.</th>
                                                <th>Meet Date</th>
                                                <th>MeetName</th>
                                                <th>City</th>
                                                <th>Party Name</th>
                                                 <th>No of Users</th>
                                                 <th>Material Class</th>
                                                 <th>Approx Budget</th>
                                                 <th>Meet Status</th>
                                                 <th>Approval Remark</th>
                                                 <th>Approval Date</th>
                                                <th>Products</th>
                                                  <th>Edit</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                         <td> <%#Container.ItemIndex+1 %></td>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("MeetPlanId") %>' />
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("MeetDate"))%></td>
                                        <td><%#Eval("MeetName") %></td>
                                        <td><%#Eval("MeetLoaction") %></td>
                                        <td><%#Eval("PartyName") %></td>
                                          <td><%#Eval("NoOfUser") %></td>
                                          <td><%#Eval("IndName") %></td>
                                           <td><%#Eval("LambBudget") %></td>
                                           <td><%#Eval("AppStatus") %></td>
                                          <td><%#Eval("AppRemark") %></td>
                                       <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("Appdate"))%></td>
                                        
                                        <td>
                                            <asp:LinkButton ID="lnkedit" runat="server" Text="View Products" CommandName="MeetEdit" CommandArgument=<%#Eval("MeetPlanId")%>></asp:LinkButton></td>
                                         <td>
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Edit" CommandName="MeetEdit1" CommandArgument=<%#Eval("MeetPlanId")%>></asp:LinkButton></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>

                           <asp:Label ID="lblresult" runat="server" />

<asp:Button ID="btnShowPopup" runat="server" style="display:none" />
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
CancelControlID="btnCancel" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>

<asp:Panel ID="pnlpopup" runat="server" BackColor="White" Width="50%"  style="display:none">
    <div class="col-md-12">

            <div class="form-group">
                <asp:GridView ID="GridView2" runat="server" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr No.">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                     <%--   <asp:TemplateField HeaderText="Product Name">
                            <ItemTemplate>
                                <asp:HiddenField ID="hidProduct" runat="server" Value='<%#Eval("ProdctID")%>' />
                                <asp:Label ID="lblPName" runat="server" Text='<%#Eval("ProdctName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Product Group Name">
                            <ItemTemplate>
                                <asp:HiddenField ID="hidProductgroup" runat="server" Value='<%#Eval("ProdctGroupId")%>' />
                                <asp:Label ID="lblPGName" runat="server" Text='<%#Eval("ProdctGroup")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Material Class Name">
                            <ItemTemplate>
                                <asp:HiddenField ID="hidMaterialClass" runat="server" Value='<%#Eval("MatrialClassId")%>' />
                                <asp:Label ID="lbMaterialClass" runat="server" Text='<%#Eval("MatrialClass")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Segment Name">
                            <ItemTemplate>
                                <asp:HiddenField ID="hidSegment" runat="server" Value='<%#Eval("SegmentId")%>' />
                                <asp:Label ID="lblSegment" runat="server" Text='<%#Eval("Segment")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                      <%--  <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ControlStyle-CssClass="btn btn-primary" ButtonType="Button" />--%>
                    </Columns>
                </asp:GridView>
            </div>
        
            <div class="form-group">
               <%-- <asp:Button ID="btnUpdate" OnClientClick="return vali();" CommandName="Update" runat="server" class="btn btn-primary" Text="Save" OnClick="btnUpdate_Click" />--%>
                <asp:Button ID="btnCancel" class="btn btn-primary" runat="server" Text="Cancel" />
            </div>
        </div>
 
</asp:Panel>
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
