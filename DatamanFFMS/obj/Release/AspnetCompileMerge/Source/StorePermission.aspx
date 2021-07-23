<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.master" AutoEventWireup="true" Inherits="StorePermission" Codebehind="StorePermission.aspx.cs" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
    <style type="text/css">
        .spinner {
            position: absolute;
            top: 50%;
            left: 50%;
            margin-left: -50px; /* half width of the spinner gif */
            margin-top: -50px; /* half height of the spinner gif */
            text-align: center;
            z-index: 999;
            overflow: auto;
            width: 100px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }

        @media (max-width: 600px) {
            .formlay {
                width: 100% !important;
            }
        }
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
    </style>
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
   
         <script type="text/javascript">
             function validate() {
                 if (document.getElementById("<%=ddlUserId.ClientID%>").value == "--Select--" || document.getElementById("<%=ddlUserId.ClientID%>").value == "0") {
                     errormessage("Please Select User Name");
                     document.getElementById("<%=ddlUserId.ClientID%>").focus();
                return false;
            }
        }
    </script>
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
        <div class="box-body" id="rptmain" runat="server" >
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Set Permissions</h3>
                            </div>
                    
                        <div class="clearfix"></div>
                             <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <asp:Image ID="Image1" ImageUrl="~/img/waiting.gif" AlternateText="Processing" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <ajax:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
        PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Always">
        <ContentTemplate>
             <div class="container-fluid" style="min-height:600px;"> 
             
           <div class="col-md-12 col-sm-12 col-xs-11">
            <div class="row">
              <div class="form-group col-md-4 col-sm-4 col-xs-12 paddingleft0" >
                    <label for="requiredfield" class="back">*</label>
                    <label for="UserName" >User Name :</label>
                   <asp:DropDownList ID="ddlUserId"  runat="server" CssClass="textbox form-control"  >
            </asp:DropDownList>
                </div>
               
               <div class="form-group col-md-4 col-sm-4 col-xs-12 margintop20" >    
                     <asp:Button ID="btnfill" runat="server" CssClass="btn btn-primary" CausesValidation="false" 
                    Text="Fill" onclick="btnfill_Click" />
                </div>
			
                </div></div><div class="clearfix"></div>

                 <div class="table-responsive">
            <table class="table"> 
        
        <tr>
            <td colspan="5">
                <asp:GridView ID="gvData" runat="server" class="table" AutoGenerateColumns="False" BackColor="White"
                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                    CellPadding="3" GridLines="Vertical"
                    Width="100%" DataKeyNames="Page_Id">
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                      <EmptyDataRowStyle BackColor="#0158a8" ForeColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="SNo." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="DisplayName" HeaderText="Page" />
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
                            <PagerStyle BackColor="#3c8dbc" ForeColor="White"  /> 
                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#3C8DBC" Font-Bold="True" ForeColor="White" />
                              <AlternatingRowStyle BackColor="#FFFFFF" />
                </asp:GridView>
            </td>
        </tr>
        <tr id="tr_btns" runat="server" style="display:none;">
        <td align="left">
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Save"
                    OnClick="btnSubmit_Click" />

                    <asp:Button ID="btncancel" runat="server" CausesValidation="false" 
                    CssClass="btn btn-primary" Text="Cancel" onclick="btncancel_Click" />
                    </td>
        </tr>
    </table>
        </div>
</div>
        </ContentTemplate>
    </asp:UpdatePanel>
                        <!-- /.box-body -->
                    </div>
                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>

        </div>
    </section>
   
    
  
</asp:Content>

