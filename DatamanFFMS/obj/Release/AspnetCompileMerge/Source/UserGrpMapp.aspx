<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.master" AutoEventWireup="true" Inherits="UserGrpMapp" Codebehind="UserGrpMapp.aspx.cs" %>

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
                 if (document.getElementById("<%=ddlType.ClientID%>").value == "--Select--" || document.getElementById("<%=ddlType.ClientID%>").value == "0") {
                     errormessage("Please Select User Name");
                     document.getElementById("<%=ddlType.ClientID%>").focus();
                     return false;
                 }
             }

             function showspinner() {

                 $("#spinner").show();

             };
             function hidespinner() {

                 $("#spinner").hide();

             };
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
                            <h3 class="box-title">User Group Mapping
</h3>
                            </div>
                    
                        <div class="clearfix"></div>
                             <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <asp:Image ID="Image1"  runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <ajax:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
        PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Always">
                <ContentTemplate>
<div class="container-fluid" style="min-height:600px;">     
  
    <div class="clearfix"></div>
    <div class="col-md-9 col-sm-8 col-xs-11">
            <div class="row">
              <div class="form-group col-md-6" >
                    
                    <label for="User Name" >User Name :</label>
                  <asp:DropDownList ID="ddlType" runat="server"  CssClass="textbox form-control" Width="100%">
                    </asp:DropDownList>
            </div>
                <div class="form-group col-md-6" style="margin-top:20px;" >
                   
                    <asp:Button ID="btnfill" runat="server" Text="Fill" CausesValidation="false" 
                            CssClass="btn btn-primary pull-left" onclick="btnfill_Click" />
                </div>
            </div></div><div class="clearfix"></div>

        <div class="table-responsive">
            <table class="table">
                
                <tr>
                    <td colspan="5">
                        <asp:GridView ID="gvData" runat="server" class="table table-bordered" AutoGenerateColumns="False" BackColor="White" 
                            BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                            CellPadding="3" GridLines="Vertical"
                            Width="100%" DataKeyNames="Code">
                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sno.">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Description" HeaderText="Group Name" />
                                <asp:TemplateField HeaderText="Allow" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="120px"></ItemStyle>
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
                <tr runat="server" style="display:none;" id="tr_sc">
                <td align="right" >
                   <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Save" OnClientClick="showspinner();" OnClick="btnSubmit_Click" />
                   <asp:Button ID="btncancel" runat="server" CssClass="btn btn-primary" Text="Cancel" 
                        onclick="btncancel_Click" />
                </td>
                </tr>
            </table>
            </div></div>
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

