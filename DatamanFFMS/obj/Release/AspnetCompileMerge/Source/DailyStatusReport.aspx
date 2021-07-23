<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/FFMS.master" CodeBehind="DailyStatusReport.aspx.cs" Inherits="AstralFFMS.DailyStatusReport" %>
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
             function validate() {
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
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Daily Status Report</h3>
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
                <div class="col-md-6"><asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary pull-right" Text="Export" Visible="false" OnClick="btnExport_Click" /></div>
            <div class="clearfix"></div>
        <asp:Panel ID="UserReg" runat="server">
          <div class="col-md-12 col-sm-12 col-xs-11">
            <div class="row">
              <div class="form-group col-md-4 col-sm-4 col-xs-12 paddingleft0" style="display:none;" >
                    <label for="requiredfield" class="back">*</label>
                    <label for="Group" >Group :</label>
                    <asp:DropDownList ID="ddlType" CssClass="textbox form-control"  Width="100%" runat="server" 
                             AutoPostBack="true"   onselectedindexchanged="ddlType_SelectedIndexChanged">
                            </asp:DropDownList>
                </div>
                <div class="row">
                 <div class="form-group col-md-4 col-sm-6 col-xs-12" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Person" >Person :</label>
                    <asp:DropDownList ID="DropDownList2" CssClass="textbox form-control" Width="75%" runat="server"  AutoPostBack="true">
                            </asp:DropDownList>
                </div>
                <div class="form-group col-md-4 col-sm-4 col-xs-12" >
                     <label for="requiredfield" class="back">*</label>
                    <label for="Status" >Status :</label>
                    <asp:DropDownList ID="ddlstatus" CssClass="textbox form-control"  Width="75%" runat="server">
                        <asp:ListItem Text="Data Not Received" Value="1"></asp:ListItem>
                        <asp:ListItem Text="GPS Off" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Data Off" Value="3"></asp:ListItem>
                            </asp:DropDownList></td>
                </div>
                <div class="form-group col-md-4 date-calender-img col-sm-4 col-xs-12" style="margin-left:0px;">
                     <label for="requiredfield" class="back">*</label>
                    <label for="Date" >Date :</label>
                    <div class="clearfix"></div>
                    <div class="col-md-9 col-xs-9 col-sm-9 no-padding">
                    <asp:TextBox ID="txt_fromdate" Enabled="false" runat="server" OnTextChanged="txt_fromdate_TextChanged" AutoPostBack="true" CssClass="textbox form-control" ></asp:TextBox>
                     </div>
                        <div class="col-md-1 col-sm-1 col-xs-1 no-padding">
                        <a href="javascript:;" class="cal-icon" ID="img1" runat="server"><i class="fa fa-calendar" style="margin:7px;" aria-hidden="true"></i></a>
                             
                             </div>
                     
                     <%--<asp:ImageButton ID="img1" runat="server" ImageUrl="~/img/Calendar.png" />--%>
                            
                            <ajax:CalendarExtender ID="cc1" runat="server" TargetControlID="txt_fromdate"  PopupButtonID="img1" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                </div>
                   </div>
				<div class="clearfix"></div>
                 <div class="form-group col-md-6 paddingleft0" style="padding:8px;">                    
                    <asp:Button ID="btngen" runat="server" CssClass="btn btn-primary" Text="Generate" OnClick="btngen_Click" OnClientClick="return validate()" /> </td>
                </div>

             </div></div><div class="clearfix"></div>
             <%--   <div id="divdata" runat="server"></div>--%>
                 
                <div class="table-responsive">
                           
                            <div id="Div1" class="col-md-12 col-sm-12 col-xs-12 no-padding" style="  float: left; margin-left: 0px; padding-top: 0px; background-color: #FFF;">
                                <asp:GridView ID="gvData" runat="server" class="table-bordered table gridclass" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" OnPageIndexChanging="gvData_PageIndexChanging" PagerSettings-Position="Bottom" PagerStyle-HorizontalAlign="Left" 
                         
                                    CellPadding="3" GridLines="Vertical" Width="100%">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                          <EmptyDataRowStyle BackColor="#3c8dbc" ForeColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno." ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PersonName" HeaderText="Person Name" ItemStyle-Width="100px" />
                                         <asp:BoundField DataField="DeviceNo" HeaderText="Device No" ItemStyle-Width="60px" />
                                        <asp:BoundField DataField="CurrentDate" HeaderText="CurrentDate" ItemStyle-Width="60px"  DataFormatString="{0:dd/MMM/yyyy HH:mm}" />
                                        <asp:BoundField DataField="Mobile" HeaderText="Mobile" ItemStyle-Width="60px" />
                                       <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="60px" />
                                    </Columns>
                                     <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <PagerStyle BackColor="#3c8dbc" ForeColor="White"  /> 
                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#3C8DBC" Font-Bold="True" ForeColor="White" />
                              <AlternatingRowStyle BackColor="#FFFFFF" />
                                </asp:GridView>
                            </div>
                       
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
