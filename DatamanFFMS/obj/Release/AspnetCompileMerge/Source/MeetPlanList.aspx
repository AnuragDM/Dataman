<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MeetPlanList.aspx.cs" Inherits="AstralFFMS.MeetPlanList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .table-responsive {
            border: 1px solid #fff;
        }
    </style>

        <script type="text/javascript">
            var V1 = "";
            function errormessage(V1) {
                $("#messageNotification").jqxNotification({
                    width: 300, position: "top-right", opacity: 2,
                    autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "error"
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
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <section class="content">
         <div id="messageNotification">
            <asp:Label ID="lblmasg" runat="server"></asp:Label>
        </div>
        <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Meet Plan List</h3>
                             <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnback" runat="server" Text="Back" class="btn btn-primary"
                                    OnClick="btnback_Click" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-8 col-xs-10">
                                    <div class="col-md-12 paddingleft0">

                                        <div class="form-group col-md-4">
                                            <label for="exampleInputEmail1">Sales Person:</label>
                                            <asp:DropDownList ID="ddlunderUser" runat="server" CssClass="form-control"></asp:DropDownList>

                                        </div>

                                        <div class="form-group col-md-3">
                                            <label for="exampleInputEmail1">Status:</label>
                                            <asp:DropDownList ID="ddlstatus" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                                                <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                                <asp:ListItem Text="Rejected" Value="Reject"></asp:ListItem>
                                            </asp:DropDownList>

                                        </div>

                                        <div id="DIV1" class="form-group col-md-2">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="txtmDate" runat="server" Style="background: white;" CssClass="form-control"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtmDate" runat="server" />
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Style="background: white;" CssClass="form-control"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                        </div>
                                        <div class="form-group col-md-1">
                                            <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                            <asp:Button type="button" ID="btnGo" runat="server" Style="padding: 3px 14px;" Text="Go" class="btn btn-primary go-button-dsr" OnClick="btnGo_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
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
                                                <th>Product Class</th>
                                                <th>Approx Budget</th>
                                                <th>Meet Status</th>
                                                <th>Approval Amount</th>
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
                                        <td><%#Container.ItemIndex+1 %></td>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("MeetPlanId") %>' />
                                        <asp:HiddenField ID="HiddenField2" runat="server" Value='<%#Eval("AppStatus") %>' />
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("MeetDate"))%></td>
                                        <td><%#Eval("MeetName") %></td>
                                        <td><%#Eval("AreaName") %></td>
                                        <td><%#Eval("PartyName") %></td>
                                        <td><%#Eval("NoOfUser") %></td>
                                        <td><%#Eval("IndName") %></td>
                                        <td style="text-align:right;"><%#Eval("LambBudget") %></td>
                                        <td><%#Eval("AppStatus") %></td>
                                        <td style="text-align:right;"><%#Eval("AppAmount") %></td>
                                        <td><%#Eval("AppRemark") %></td>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("Appdate"))%></td>

                                        <td>
                                            <asp:LinkButton ID="lnkedit" runat="server" Text="View Products" CommandName="MeetEdit" CommandArgument='<%#Eval("MeetPlanId")%>'></asp:LinkButton></td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Edit" CommandName="MeetEdit1" CommandArgument='<%#Eval("MeetPlanId")%>' Enabled='<%#Eval("AppStatus").ToString()=="Pending" ? true : false %>' ForeColor='<%#Eval("AppStatus").ToString()=="Pending" ? System.Drawing.ColorTranslator.FromHtml("#3c8dbc") : System.Drawing.Color.Black %>'></asp:LinkButton></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>

                        <asp:Label ID="lblresult" runat="server" />

                        <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
                            CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
                        </ajaxToolkit:ModalPopupExtender>

                        <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Width="50%" Style="display: none">
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
                                            <asp:TemplateField HeaderText="Product Class">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hidMaterialClass" runat="server" Value='<%#Eval("MatrialClassId")%>' />
                                                    <asp:Label ID="lbMaterialClass" runat="server" Text='<%#Eval("MatrialClass")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product Segment">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hidSegment" runat="server" Value='<%#Eval("SegmentId")%>' />
                                                    <asp:Label ID="lblSegment" runat="server" Text='<%#Eval("Segment")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product Group">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hidProductgroup" runat="server" Value='<%#Eval("ProdctGroupId")%>' />
                                                    <asp:Label ID="lblPGName" runat="server" Text='<%#Eval("ProdctGroup")%>'></asp:Label>
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

