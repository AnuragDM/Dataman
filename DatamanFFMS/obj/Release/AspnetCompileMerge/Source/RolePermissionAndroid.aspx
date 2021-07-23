<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="RolePermissionAndroid.aspx.cs" Inherits="AstralFFMS.RolePermissionAndroid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
            if (document.getElementById("<%=ddlModule.ClientID%>").value == "0") {
                alert("Please Select a Module")
                return false;
            }
            if (document.getElementById("<%=ddlapp.ClientID%>").value == "None") {
                alert("Please Select a App")
                return false;
            }
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
                            <h3 class="box-title">Role Permissions For Android</h3>

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
                                <div class="form-group col-md-4">
                                <label for="exampleInputEmail1">App:</label>

                                
                                <asp:DropDownList ID="ddlapp" Width="100%" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlapp_SelectedIndexChanged">
                                     <asp:ListItem Text="-Select-" Value="None"></asp:ListItem>
                                    <asp:ListItem Text="Grahaak-Field" Value="Field"></asp:ListItem>
                                    <asp:ListItem Text="Grahaak-Manager" Value="Manager"></asp:ListItem>
                                       <asp:ListItem Text="Grahaak-Distributor" Value="Distributor"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                             <div class="form-group col-md-4">
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
                            <div class="form-group table-responsive">
                                <asp:GridView ID="gvData" class="table" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    EmptyDataText="No Records Found"
                                    CellPadding="3" BorderWidth="0" GridLines="Vertical" 
                                    Width="100%" DataKeyNames="PageId" >
                                    <RowStyle ForeColor="Black" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno." HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" CssClass="small-align" Width="100px" />
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="DisplayName" HeaderText="Page Name" ItemStyle-CssClass="small-align" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ckViewHead" runat="server" Text="View" onclick="SelectAllByRow(this, 2)" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckView" Checked='<%# Bind("ViewP") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ckAddHead" runat="server" Text="Add" onclick="SelectAllByRow(this, 3)" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckAdd" Checked='<%# Bind("AddP") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ckEditHead" runat="server" Text="Edit" onclick="SelectAllByRow(this, 4)" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckEdit" Checked='<%# Bind("EditP") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ckDeleteHead" runat="server" Text="Delete" onclick="SelectAllByRow(this, 5)" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckDelete" Checked='<%# Bind("DeleteP") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ckExportHead" runat="server" Text="Export" onclick="SelectAllByRow(this, 6)" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckExport" Checked='<%# Bind("ExportP") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                      
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <PagerStyle HorizontalAlign="Center"  CssClass="GridPager" BackColor="#3c8dbc"  />
                                    <SelectedRowStyle BackColor="#008A8C" HorizontalAlign="Center" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#3c8dbc" HorizontalAlign="Right" Font-Bold="True" ForeColor="white" />
                                    <AlternatingRowStyle BackColor="#e8e8e8" />
                                </asp:GridView>

                            </div>

                            <div class="form-group" style="float: right" visible="false" runat="server" id="divbtns">
                                <asp:Button ID="btnSubmit" runat="server"
                                    Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                                &nbsp;&nbsp;
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" PostBackUrl="~/RolePermission.aspx" />

                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </section>

</asp:Content>
