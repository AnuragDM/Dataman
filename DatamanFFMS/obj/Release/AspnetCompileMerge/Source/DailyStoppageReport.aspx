<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/FFMS.master" CodeBehind="DailyStoppageReport.aspx.cs" Inherits="AstralFFMS.DailyStoppageReport" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%@ Register Src="ctlCalendar.ascx" TagName="Calendar" TagPrefix="ctl" %>
    <%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
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
             <%--    if (document.getElementById("<%=ddlType.ClientID%>").value == "--Select--" || document.getElementById("<%=ddlType.ClientID%>").value == "0") {
                  errormessage("Please Select Group Name");
                  document.getElementById("<%=ddlType.ClientID%>").focus();
                return false;
            }--%>
            if (document.getElementById("<%=DropDownList2.ClientID%>").value == "--Select--" || document.getElementById("<%=DropDownList2.ClientID%>").value == "0") {
                  errormessage("Please Select Person Name");
                  document.getElementById("<%=DropDownList2.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=Txt_FromTime.ClientID%>").value == "" || document.getElementById("<%=Txt_FromTime.ClientID%>").value == "00:00") {
                  errormessage("Your Time Format is incorrect. Please try again.");
                  document.getElementById("<%=Txt_FromTime.ClientID%>").focus();
                return false;
            }
            var digits = /^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$/;
            var digitsid = document.getElementById("<%=Txt_FromTime.ClientID %>").value;
            var digitsArray = digitsid.match(digits);
            var temp;
            if (digitsArray == null) {
                errormessage("Your Time Format is incorrect. Please try again.");
                document.getElementById("<%=Txt_FromTime.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=TextBox2.ClientID%>").value == "" || document.getElementById("<%=TextBox2.ClientID%>").value == "00:00") {
                  errormessage("Your Time Format is incorrect. Please try again.");
                  document.getElementById("<%=TextBox2.ClientID%>").focus();
                return false;
            }
            var digits = /^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$/;
            var digitsid = document.getElementById("<%=TextBox2.ClientID %>").value;
            var digitsArray = digitsid.match(digits);
            var temp;
            if (digitsArray == null) {
                errormessage("Your Time seems incorrect. Please try again.");
                document.getElementById("<%=TextBox2.ClientID %>").focus();
                return false;
            }

            var start = document.getElementById("<%=Txt_FromTime.ClientID %>").value;
              var end = document.getElementById("<%=TextBox2.ClientID %>").value;
              var dtStart = new Date("1/1/2007 " + start);
              var dtEnd = new Date("1/1/2007 " + end);
              if (Date.parse(dtStart) > Date.parse(dtEnd)) {
                  errormessage("To Time Should be greater then From Time");
                  document.getElementById("<%=TextBox2.ClientID %>").focus();
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
                            <h3 class="box-title">Daily Stoppage Report</h3>
                            </div>
                    
                        <div class="clearfix"></div>
                             <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <asp:Image ID="Image1" ImageUrl="~/img/waiting.gif" AlternateText="Processing" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <ajax:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
        PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />
      <asp:UpdatePanel ID="up1" runat="server">
         <ContentTemplate>
             <div class="container-fluid" style="min-height:600px;">
                 <div class="col-md-6"><asp:Label ID="lblHeading" runat="server" CssClass="heading pull-left">Daily Stoppage Report</asp:Label></div>
                 <div class="col-md-6"><asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary pull-right" Text="Export" Visible="false" OnClick="btnExport_Click" /></div>
               <div class="clearfix"></div>
            <asp:Panel ID="UserReg" runat="server">
         <div class="col-md-12 col-sm-12 col-xs-11">
            <div class="row">
              <div class="form-group col-md-4 col-sm-4 col-xs-12 paddingleft0" style="display:none;" >
                    <label for="requiredfield" class="back">*</label>
                    <label for="Group" >Group :</label>
                    <asp:DropDownList ID="ddlType" CssClass="textbox form-control" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true" Width="100%" runat="server">
                            </asp:DropDownList>
                </div>
                <div class="form-group col-md-4 col-sm-4 col-xs-12" >
                    <label for="requiredfield" class="back">*</label>
                    <label for="Person" >Person :</label>
                    <asp:DropDownList ID="DropDownList2" CssClass="textbox form-control" Width="100%" runat="server" AutoPostBack="true"  onselectedindexchanged="DropDownList2_SelectedIndexChanged">
                         </asp:DropDownList>
                </div>
                <div class="clearfix"></div>
                 <div class="form-group col-md-4 col-sm-12 col-xs-12 no-padding" >
                     <label for="requiredfield" class="back">*</label>
                    <label for="Group" >From Date :</label>
                    <div class="clearfix"></div>
                      <div class="col-md-6 col-sm-6 col-xs-8">
                    <asp:TextBox ID="txt_fromdate" Enabled="false" runat="server" OnTextChanged="txt_fromdate_TextChanged" AutoPostBack="true" CssClass="textbox form-control"></asp:TextBox>
                         </div>
                      <div class="col-md-1 col-xs-2 col-sm-1 no-padding">
                        <a href="javascript:;" id="img1" class="cal-icon"><i class="fa fa-calendar" aria-hidden="true"></i></a>
                         </div> 
<%--                          <asp:ImageButton ID="img1" runat="server" ImageUrl="~/img/Calendar.png" />--%>
                            <div class="col-md-5 col-sm-3 col-xs-6"><asp:TextBox ID="Txt_FromTime" runat="server" CssClass="textbox form-control" MaxLength="5"></asp:TextBox></div>
                            <ajax:CalendarExtender ID="cc1" runat="server" TargetControlID="txt_fromdate" PopupButtonID="img1" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                </div>                 
                <div class="form-group col-md-4 col-sm-12 col-xs-12" >                    
                   <label for="To Date" >To Date :</label>

                      <div class="clearfix"></div>

                    <div class="col-md-6 col-sm-6 col-xs-8">

                    <asp:TextBox ID="TextBox1" Enabled="false" runat="server" CssClass="textbox form-control" OnTextChanged="txt_fromdate_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </div>

                         <div class="col-md-1 col-xs-2 col-sm-1 no-padding">
                         <a href="javascript:;" id="ImageButton1" class="cal-icon"><i class="fa fa-calendar" aria-hidden="true"></i></a>
                       <%-- <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/Calendar.png" />--%>
                         </div>
                        <div class="col-md-5 col-sm-3 col-xs-6">
                            <asp:TextBox ID="TextBox2" runat="server" CssClass="textbox form-control" MaxLength="5"></asp:TextBox></div>
                            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TextBox1" PopupButtonID="ImageButton1" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                </div>
                     <div class="clearfix"></div>
                <div class="form-group col-md-4 col-sm-4 col-xs-12 no-padding" >
                    <label for="requiredfield" class="back">*</label>
                    <label for="Stoppage Min." >Stoppage Min. :</label>
                     <asp:TextBox ID="txtstpg" runat="server" Width="128px" Text="10" MaxLength="4" CssClass="form-control" ></asp:TextBox>
                          <ajax:FilteredTextBoxExtender ID="txtstpg_FilteredTextBoxExtender"
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtstpg">
                        </ajax:FilteredTextBoxExtender>
                           <asp:HiddenField ID="txtaccu" runat="server" />
               </div>
                <div class="clearfix"></div>
             <div class="form-group col-md-4 col-sm-4 col-xs-12 no-padding" >
                    <asp:Button ID="btngen" runat="server" CssClass="btn btn-primary" Text="Generate" OnClick="btngen_Click" OnClientClick="return validate()" /> </td>
                </div>
                </div><div class="clearfix"></div>


               
                <div class="table-responsive">
            <table class="table">
                    <tr>
                        <td colspan="4">
                            <div id="Div1" style=" float: left; margin-left: 0px; padding-top: 0px; background-color: #FFF;">
                                <asp:GridView ID="gvData" runat="server" class="table table-bordered gridclass" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                    CellPadding="3" GridLines="Vertical" Width="100%"  PagerSettings-Position="Bottom" 
                                                   OnPageIndexChanging="gvData_PageIndexChanging"  PagerStyle-HorizontalAlign="Left"
                            PagerStyle-BackColor="Aqua" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last"
                            PagerSettings-PreviousPageText="<<" PagerSettings-NextPageText=">>" 
                                    onrowdatabound="gvData_RowDataBound">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                 <EmptyDataRowStyle BackColor="#3c8dbc" ForeColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        </asp:TemplateField>
                                       
                                        <asp:BoundField DataField="FromDate" HeaderText="From" ItemStyle-Width="100px"
                                            DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}" />
                                              <asp:BoundField DataField="ToDate" HeaderText="To" ItemStyle-Width="100px"
                                            DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}" />
                                       <%-- <asp:BoundField DataField="latitude" HeaderText="latitude" ItemStyle-Width="60px" />
                                        <asp:BoundField DataField="longitude" HeaderText="longitude" ItemStyle-Width="60px" />--%>
                                        <asp:BoundField DataField="description" HeaderText="Address" ItemStyle-Width="100px" />
                                        <asp:BoundField DataField="count" HeaderText="Stoppage Min." ItemStyle-Width="60px" />
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
