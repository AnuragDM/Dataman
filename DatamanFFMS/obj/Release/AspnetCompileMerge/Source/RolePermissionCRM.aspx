<%@ Page Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="RolePermissionCRM.aspx.cs" Inherits="AstralFFMS.RolePermissionCRM" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <%--  <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style>
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
    </style>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>

    <script type="text/javascript">
        function SelectAllByRow(ChK, cellno) {
            var gv = document.getElementById('<%= gvData.ClientID %>');
            for (var i = 1; i <= gv.rows.length - 1; i++) {
                var len = gv.rows[i].getElementsByTagName("input").length;
                if (gv.rows[i].getElementsByTagName("input")[cellno - 2].type == 'checkbox') {
                    gv.rows[i].getElementsByTagName("input")[cellno - 2].checked = ChK.checked
                }
            }
        }
    </script>
    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 250, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 1000, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
    </script>

    <script type="text/javascript">
        var V = "";
        function Successmessage(V) {
            $("#messageNotification").jqxNotification({
                width: 250, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 1000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>

    <script type="text/javascript">
        function validate() {
            if (document.getElementById("<%=ddlRole.ClientID%>").value == "0") {
                alert("Please Select a Role")
                return false;
            }
          <%--  if (document.getElementById("<%=ddlModule.ClientID%>").value == "0") {
                alert("Please Select a Module")
                return false;
            }--%>
            return true;
        }
    </script>
    <section class="content">
        <style type="text/css">
       
        .GridPager a
        {
            display:block;
            height:20px;
            width:15px;
            background-color:#3c8dbc;
            color:#fff;
            font-weight:bold;
           
            text-align:center;
            text-decoration:none;
        }
         .GridPager span
        {
            display:block;
            height:20px;
            width:15px;
            background-color:#fff;
            color:#3c8dbc;
            font-weight:bold;
           
            text-align:center;
            text-decoration:none;
        }
    </style>
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <!-- left column -->
            <div class="col-md-12">
                <div id="InputWork">
                    <!-- general form elements -->
                    <div class="box box-primary">
                        <div class="box-header with-border">
                            <h3 class="box-title">Role Wise CRM Permissions</h3>

                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-lg-9 col-md-8 col-sm-7 col-xs-9">
                            <label id="lblcols" runat="server" style="color: red; font-family: Arial; font-size: small; font-weight: 300;" visible="false"></label>
                            <div class="form-group col-md-4">
                                <input id="Userid" hidden="hidden" />
                                <label for="exampleInputEmail1">Role:</label>

                                
                                <asp:DropDownList ID="ddlRole" Width="100%" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                              
                                
                               
                            </div>
                             <div class="form-group col-md-4" style="display:none;">
                                <label for="exampleInputEmail1">Module:</label>

                                
                                <asp:DropDownList ID="ddlModule" Width="100%" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-md-4">
                                <label for="exampleInputEmail1" style="display:block; visibility:hidden">zkjfhksj</label>
                                
                                 <asp:Button ID="Button1" runat="server" style="padding: 3px 14px;" CssClass="btn btn-primary" Text="Show" OnClientClick="return validate();" OnClick="btnshow_Click" />
                            </div>
                             </div>
                            <div class="clearfix"></div>
                            <div class="form-group table-responsive"    style=" margin-left: 28px;">
                                <asp:GridView ID="gvData" class="table" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    EmptyDataText="No Records Found"
                                    CellPadding="3" BorderWidth="0" GridLines="Vertical" 
                                    Width="40%" DataKeyNames="ActivityId" >
                                    <RowStyle ForeColor="Black" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno." HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" CssClass="small-align" Width="40px"  />
                                            <HeaderStyle Font-Bold="true" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Activity" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="Activity" runat="server" Text='<%# Bind("Activity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" CssClass="small-align" Width="200px" />
                                                          <HeaderStyle Font-Bold="true" />
                                        </asp:TemplateField>
                                     
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ckViewHead" runat="server" Text="Allow" onclick="SelectAllByRow(this, 2)" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckAllow" Checked='<%# Bind("Allow") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                        </asp:TemplateField>
                                       <%-- <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hidactivityId" runat="server" Value='<%# Bind("ActivityId") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                   
                                     
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <PagerStyle HorizontalAlign="Center"  CssClass="GridPager" BackColor="#3c8dbc"  />
                                    <SelectedRowStyle BackColor="#008A8C" HorizontalAlign="Center" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#3c8dbc" HorizontalAlign="Right" Font-Bold="True" ForeColor="white" />
                                    <AlternatingRowStyle BackColor="#e8e8e8" />
                                </asp:GridView>

                            </div>

                            <div class="form-group"  style=" margin-left: 30px;" visible="false" runat="server" id="divbtns">
                                <asp:Button ID="btnSubmit" runat="server"
                                    Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                                &nbsp;&nbsp;
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" PostBackUrl="~/RolePermissionCRM.aspx" />

                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </section>
</asp:Content>
