<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/FFMS.master" CodeBehind="ActiveusersRpt.aspx.cs" Inherits="AstralFFMS.ActiveusersRpt" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%@ Register Src="ctlCalendar.ascx" TagName="Calendar" TagPrefix="ctl" %>
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
    <script type="text/javascript" language="javascript">
        function ConfirmOnDelete(item) {
            if (confirm("Are you sure you want to delete: " + item + "?") == true)
                return true;
            else
                return false;
        }
    </script>
        <script type="text/javascript">
            function validate1() {
               
                if (document.getElementById("<%=DropDownList2.ClientID%>").value == "--Select--" || document.getElementById("<%=DropDownList2.ClientID%>").value == "0") {
                errormessage("Please Select Person Name");
                <%--document.getElementById("<%=ddlgrp.ClientID%>").focus();--%>
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
            <div class="row" >
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                           <%-- <h3 class="box-title">Active Users Reports</h3>--%>
                             <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>     
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
                <div class="col-md-6"></div>
                <div class="col-md-6"><asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary" Text="Export" Visible="false" OnClick="btnExport_Click" /></div>
            <div class="clearfix"></div>
        <asp:Panel ID="Panel2" runat="server">
          <div class="col-md-10 col-sm-9 col-xs-11">
            <div class="row">
              <div class="form-group col-md-6" style="display:none;" >
                    <label for="requiredfield" class="back">*</label>
                    <label for="Group" >Group :</label>
                    <asp:DropDownList ID="ddlgrp" CssClass="textbox form-control" 
                                AutoPostBack="true"  runat="server" 
                                onselectedindexchanged="ddlgrp_SelectedIndexChanged">
                            </asp:DropDownList>
               
               <%--     <label for="Group" style="display:block; visibility:hidden;">button :</label>--%>
                   
                </div>
                 <div class="form-group col-md-6 col-sm-6 col-xs-12 paddingleft0" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Person" >Person :</label>
                    <asp:DropDownList ID="DropDownList2" CssClass="textbox form-control" Width="80%" runat="server"  AutoPostBack="true">
                            </asp:DropDownList>
                </div>
                <div class="clearfix"></div>
                <div class="col-md-6 margintop20 paddingleft0" style="padding:15px;">

                     <asp:Button ID="Button2" runat="server" CssClass="btn btn-primary" Text="Submit" 
                                OnClientClick="return validate1()" onclick="Button2_Click" />
                


                </div>


            </div>
              <div class="clearfix"></div>

                <div class="table-responsive">
            <table class="table">
                    <tr>
                        <td colspan="4">
                            <div style="width:100%; min-height:500px;">
                                <asp:GridView ID="GridView1" class="table table-bordered gridclass" runat="server" AutoGenerateColumns="False" BackColor="White" EmptyDataText="No Record Found !!"
                                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" GridLines="Vertical" OnPageIndexChanging="GridView1_PageIndexChanging" PagerSettings-Position="Bottom" PagerStyle-HorizontalAlign="Left" 
                        
                                    Width="100%">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <EmptyDataRowStyle BackColor="#0158a8" ForeColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Person" HeaderText="Person" />
                                           <asp:BoundField DataField="Mobile" HeaderText="Mobile" />
                                        <asp:BoundField DataField="Version" HeaderText="Version" />
                                        <asp:BoundField DataField="Versiondate" HeaderText="Version Date" />
                                    </Columns>
                                     <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <PagerStyle BackColor="#3c8dbc" ForeColor="White"  /> 
                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#3C8DBC" Font-Bold="True" ForeColor="White" />
                              <AlternatingRowStyle BackColor="#FFFFFF" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table></div>
                <div class="table-responsive">
            <table class="table">
                    <tr>
                    <td colspan="4">
                       <div class="hidden">
                    <asp:GridView ID="gdw" runat="server" class="table table-bordered" AutoGenerateColumns="true" BackColor="White" Visible="false"
                                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                    Width="100%">
                                          <Columns>
                                        <asp:TemplateField HeaderText="Sno.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Name" HeaderText="Group" />
                                        <asp:BoundField DataField="Person" HeaderText="Person" />
                                        <asp:BoundField DataField="status" HeaderText="Status" />
                                    </Columns>
                                    </asp:GridView>
                                    </div>
                    </td></tr>
                </table>
                    </div>
            </asp:Panel>
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
