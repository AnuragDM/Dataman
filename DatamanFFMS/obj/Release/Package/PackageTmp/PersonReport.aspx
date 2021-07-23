<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/FFMS.master" CodeBehind="PersonReport.aspx.cs" Inherits="AstralFFMS.PersonReport" %>
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

       @media (max-width: 400px) {
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
                <%--if (document.getElementById("<%=ddlType.ClientID%>").value == "--Select--" || document.getElementById("<%=ddlType.ClientID%>").value == "0") {
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

            var start = document.getElementById("<%=Txt_FromTime.ClientID %>").value;
            var end = document.getElementById("<%=txt_Totime.ClientID %>").value;
            var dtStart = new Date("1/1/2007 " + start);
            var dtEnd = new Date("1/1/2007 " + end);
            if (Date.parse(dtStart) > Date.parse(dtEnd)) {
                errormessage("To Time Should be greater then From Time");
                document.getElementById("<%=txt_Totime.ClientID %>").focus();
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
                            <%--<h3 class="box-title">Daily Activity Report</h3>--%>
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
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="col-md-6"></div>
                <div class="col-md-6"> <asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary pull-right" Text="Export" Visible="false" OnClick="btnExport_Click" /></div>
                <div class="clearfix"></div>
        <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="row">
                <div class="form-group col-md-3 col-sm-4 col-xs-12 paddingleft0" style="display:none;" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Group Name" >Group :</label>
                    <asp:DropDownList ID="ddlType" CssClass="textbox form-control" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true" Width="100%" runat="server">
                            </asp:DropDownList>
                </div>
             
                 <div class="form-group col-md-3 col-sm-4 col-xs-12 paddingleft0">
                   <label for="requiredfield" class="back">*</label>
                   <label for="Person">Person :</label>
                    <asp:DropDownList ID="DropDownList2" CssClass="textbox form-control" Width="96%" runat="server" AutoPostBack="true"  onselectedindexchanged="DropDownList2_SelectedIndexChanged">
                         </asp:DropDownList>
                  </div>
                <div class="clearfix"></div>
                <div class="row">
                <div class="form-group col-md-4 col-sm-4 col-xs-12 date-calender-img paddingleft10">
                
                   <label for="Date">Date :</label>
                    <div class="clearfix"></div>

                    <div class="col-md-9 col-xs-9 col-sm-8 paddingleft0">
                    <asp:TextBox ID="txt_fromdate" Enabled="false" runat="server" OnTextChanged="txt_fromdate_TextChanged" AutoPostBack="true" CssClass="textbox form-control" >

                    </asp:TextBox>
                    </div>
                    
                      <div class="col-md-1 col-sm-1 col-xs-1 no-padding">
                        <a href="javascript:;" class="cal-icon" ID="img1" runat="server"><i class="fa fa-calendar" style="margin:-10px;"aria-hidden="true"></i></a>
                             
                             </div>
                    
                   <%-- 
                    <asp:ImageButton ID="img1" runat="server" ImageUrl="~/img/Calendar.png" />--%>
                          
                            <ajax:CalendarExtender ID="cc1" runat="server" TargetControlID="txt_fromdate" PopupButtonID="img1" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                </div>
                 <div class="form-group col-md-7 col-sm-7 col-xs-12 paddingleft0" > 
                     <div class="col-md-3 col-xs-5 col-sm-3">            
                   <label for="Date" >Time From :</label>
                     <div class="clearfix"></div>
                   
                     <asp:TextBox ID="Txt_FromTime" runat="server" CssClass="textbox form-control" MaxLength="5" ></asp:TextBox>


                         </div>


                              <div class="col-md-3 col-xs-5 col-sm-3 paddingleft0">        
                   <label for="To" >To :</label>
                                   <div class="clearfix"></div>
                    <asp:TextBox ID="txt_Totime" runat="server" CssClass="textbox form-control"  MaxLength="5" ></asp:TextBox>
             </div>



                      <div class="col-md-3 col-xs-5 col-sm-3">       
                   <label for="Location" >Location :</label>
                    <asp:DropDownList ID="ddlloc" runat="server" CssClass="textbox form-control" AutoPostBack="true" 
                                onselectedindexchanged="ddlloc_SelectedIndexChanged">
                     <asp:ListItem Text="All" Value="0"></asp:ListItem>
                     <asp:ListItem Text="GPS" Value="G" Selected="True"></asp:ListItem>
                     <asp:ListItem Text="Tower" Value="C"></asp:ListItem>
                 </asp:DropDownList>
               </div>
                      <div class="col-md-3 col-xs-6 col-sm-3 paddingleft0">       
                   <label for="Accuracy" >Accuracy :</label>
                    <div class="clearfix"></div>
                          <div class="col-md-8 col-xs-10 col-sm-11 paddingleft0">
                    <asp:TextBox ID="txtaccu" runat="server" width="110%" Text="60" CssClass="form-control"></asp:TextBox></div><div class="col-md-1">(Metres)</div>
                          <ajax:FilteredTextBoxExtender ID="txtaccu_FilteredTextBoxExtender"
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtaccu">
                        </ajax:FilteredTextBoxExtender>
                </div>
                     </div>
                    </div>
                <div class="clearfix"></div>

                 <div class="form-group col-md-6 col-sm-6 col-xs-12 marginright20 paddingleft0" style="padding:15px;">
                     <asp:Button ID="btngenerate" runat="server" CssClass="btn btn-primary" Text="Generate" OnClick="btngenerate_Click" OnClientClick="return validate()" /> 
                 </div>
                           </div>   
                </div></div><div class="clearfix"></div>
            <asp:Panel ID="UserReg" runat="server">
                
                <div class="table-responsive">
             
                            <div id="Div1"  class="col-md-12 col-xs-12 no-padding gridclass" style="min-height: 600px;  float: left;     margin-left: 0px; padding-top: 0px; background-color: #FFF;">
                                <asp:GridView ID="gvData" class="table table-bordered" runat="server" AutoGenerateColumns="False"
                                    OnPageIndexChanging="gvData_PageIndexChanging"  PagerSettings-Position="Bottom" PagerStyle-HorizontalAlign="Left"                         
                                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                    CellPadding="3" GridLines="Vertical" Width="100%">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <EmptyDataRowStyle BackColor="#3c8dbc" ForeColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Person" HeaderText="Person" />
                                        <asp:BoundField DataField="address" HeaderText="Location Name"  />
                                      <asp:BoundField DataField="Cdate" HeaderText="Date"  />
                                              <asp:BoundField DataField="Time" HeaderText="Time"  />
                                            <%--  <asp:BoundField DataField="distance" HeaderText="distance" Visible="false"/>--%>
                                        <asp:BoundField DataField="Battery" HeaderText="Battery" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="Signal" HeaderText="SIG" />
                                        <asp:BoundField DataField="Accuracy" HeaderText="Accuracy" ItemStyle-HorizontalAlign="Right"/>
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

