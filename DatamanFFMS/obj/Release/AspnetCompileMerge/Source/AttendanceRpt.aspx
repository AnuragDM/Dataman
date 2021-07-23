<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/FFMS.master" CodeBehind="AttendanceRpt.aspx.cs" Inherits="AstralFFMS.AttendanceRpt" %>
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
                            <h3 class="box-title">Attendance Report</h3>
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
              
                <div class="col-md-10"><asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary pull-right" Text="Export" Visible="false" OnClick="btnExport_Click" /></div>
            <div class="clearfix"></div>
         <asp:Panel ID="UserReg" runat="server">
        <div class="col-md-8 col-sm-8 col-xs-11 ">
             
              <div class="form-group col-md-6 col-sm-6 col-xs-12 paddingleft0" style="display:none;" >
                    <label for="requiredfield" class="back">*</label>
                    <label for="Group" >Group :</label>
                    <asp:DropDownList ID="ddlType" CssClass="textbox form-control" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true" Width="100%" runat="server">
                            </asp:DropDownList>
            </div>
              
                <div class="form-group col-md-6 col-sm-6 col-xs-12" >
                    <label for="requiredfield" class="back">*</label>
                    <label for="Person" >Person :</label>
                    <asp:DropDownList ID="DropDownList2" CssClass="textbox form-control" Width="80%" runat="server" AutoPostBack="true"  onselectedindexchanged="DropDownList2_SelectedIndexChanged">
                         </asp:DropDownList>
            </div>
                     
               
             <div class="form-group col-md-6 date-calender-img col-sm-6 col-xs-12" >
                        <label for="requiredfield" class="back">*</label>
                    <label for="From Date" >From Date :</label>
                 <div class="col-md-11 col-xs-11 col-sm-11 no-padding">

                    <asp:TextBox ID="txt_fromdate" Enabled="false" runat="server"  Width="86%" OnTextChanged="txt_fromdate_TextChanged" AutoPostBack="true" CssClass="textbox form-control"></asp:TextBox>
                     
                     </div>
                     
                 <div class="col-md-1 col-sm-1 col-xs-1 no-padding">
                        <a href="javascript:;" class="cal-icon" ID="img1" runat="server"><i class="fa fa-calendar" style="margin:-35px;" aria-hidden="true"></i></a>
                             
                             </div>

                 <%--    <asp:ImageButton ID="img1" runat="server" ImageUrl="~/img/Calendar.png" />
                 --%>           
                            <ajax:CalendarExtender ID="cc1" runat="server" TargetControlID="txt_fromdate" PopupButtonID="img1" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
            </div>
            
            <div class="form-group col-md-6 date-calender-img col-sm-6 col-xs-12" >
                    <label for="requiredfield" class="back">*</label>
                    <label for="To Date" >To Date :</label>

               

                 <div class="col-md-11 col-xs-11 col-sm-11 no-padding">

                    <asp:TextBox ID="TextBox1" Enabled="false" runat="server"  Width="87%" CssClass="textbox form-control" OnTextChanged="TextBox1_TextChanged" AutoPostBack="true"></asp:TextBox>
                </div>
                
               <%-- <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/Calendar.png" />--%>


                <div class="col-md-1 col-sm-1 col-xs-1 no-padding">
                        <a href="javascript:;" class="cal-icon" ID="ImageButton1" runat="server"><i class="fa fa-calendar" style="margin:-33px;" aria-hidden="true"></i></a>
                             
                             </div>
                       
                            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TextBox1" PopupButtonID="ImageButton1" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                       <asp:HiddenField ID="txtaccu" runat="server" />
         
                </div>
             
			
          <div class="form-group col-md-6 margintop20"  style="padding:15px;">
                    <asp:Button ID="btngen" runat="server" CssClass="btn btn-primary" Text="Generate" OnClick="btngen_Click" OnClientClick="return validate()" /> </td>
            </div>
             </div>
              </div></div><div class="clearfix"></div>
                <div class="table-responsive">
         
                            <div id="Div1"  class="" style=" float: left; margin-left: 0px; padding-top: 0px; background-color: #FFF;">
                                <asp:GridView ID="gvData" class="table table-bordered gridclass" runat="server" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"  PagerSettings-Position="Bottom" 
                                                   OnPageIndexChanging="gvData_PageIndexChanging"  PagerStyle-HorizontalAlign="Left"
                            PagerStyle-BackColor="Aqua" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last"
                            PagerSettings-PreviousPageText="<<" PagerSettings-NextPageText=">>" 
                                    CellPadding="3" GridLines="Vertical" Width="100%">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <EmptyDataRowStyle BackColor="#0158a8" ForeColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PersonName" HeaderText="Person Name" ItemStyle-Width="100px" />
                                         <asp:BoundField DataField="DeviceNo" HeaderText="Device No" ItemStyle-Width="60px" />
                                        <asp:BoundField DataField="StartTime" HeaderText="First Record Time" ItemStyle-Width="100px"
                                            DataFormatString="{0:dd/MMM/yyyy HH:mm:ss}" />
                                              <asp:BoundField DataField="EndTime" HeaderText="Last Record Time" ItemStyle-Width="100px"
                                            DataFormatString="{0:dd/MMM/yyyy HH:mm:ss}" />
                                        <asp:BoundField DataField="Mobile" HeaderText="Mobile" ItemStyle-Width="60px" />
                                            <asp:BoundField DataField="remark" HeaderText="Remark" ItemStyle-Width="60px" />
                                      
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