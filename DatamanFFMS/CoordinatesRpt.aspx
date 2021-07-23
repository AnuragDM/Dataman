<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.master" AutoEventWireup="true" Inherits="CoordinatesRpt" Codebehind="CoordinatesRpt.aspx.cs" %>
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
        function validate() {
            if (document.getElementById("<%=ddlgroup.ClientID%>").value == "--Select--" || document.getElementById("<%=ddlgroup.ClientID%>").value == "0") {
                errormessage("Please Select Group Name");
                document.getElementById("<%=ddlgroup.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlperson.ClientID%>").value == "--Select--" || document.getElementById("<%=ddlperson.ClientID%>").value == "0") {
                errormessage("Please Select Person Name");
                document.getElementById("<%=ddlperson.ClientID%>").focus();
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
                          <%--  <h3 class="box-title">Manage Co-ordinates</h3>--%>
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
               
            <div class="clearfix"></div>
         <asp:Panel ID="UserReg" runat="server">
          <div class="col-md-9 col-sm-8 col-xs-11">
            <div class="row">
              <div class="form-group col-md-6 col-sm-6 col-xs-12" >    
                    <label for="requiredfield" class="back">*</label>                
                    <label for="Group" >Group :</label>
                    <asp:DropDownList ID="ddlgroup" CssClass="textbox form-control"  AutoPostBack="true"
                                Width="100%" runat="server" 
                                onselectedindexchanged="ddlgroup_SelectedIndexChanged">
                            </asp:DropDownList>
            </div>
                <div class="form-group col-md-6 col-sm-6 col-xs-12" > 
                    <label for="requiredfield" class="back">*</label>                
                    <label for="Person" >Person :</label>
                    <asp:DropDownList ID="ddlperson" CssClass="textbox form-control" Width="100%" runat="server" AutoPostBack="true"  onselectedindexchanged="ddlperson_SelectedIndexChanged">
                         </asp:DropDownList>
                 </div> 
                <div class="form-group col-md-6 col-sm-6 col-xs-12" >    
                                 
                    <label for="From Date" >From Date :</label>
                    <div class="clearfix"></div>
                    <div class="col-md-8 col-sm-8 col-xs-8 paddingleft0">
                    <asp:TextBox ID="txt_fromdate" Enabled="false" runat="server" OnTextChanged="txt_fromdate_TextChanged" AutoPostBack="true" CssClass="textbox form-control"></asp:TextBox>
                        </div>

                      <div class="col-md-1 col-xs-2 no-padding">
                        <a href="javascript:;" class="cal-icon" ID="img1" runat="server"><i class="fa fa-calendar" aria-hidden="true"></i></a>
                             
                             </div>
                   <%-- <asp:ImageButton ID="img1" runat="server" ImageUrl="~/img/Calendar.png" />--%>

                    <div class="col-md-3 col-sm-3 col-xs-6 paddingright0 margintop10-xs">
                            <asp:TextBox ID="Txt_FromTime" runat="server" CssClass="textbox form-control" MaxLength="5"></asp:TextBox></div>
                            <ajax:CalendarExtender ID="cc1" runat="server" TargetControlID="txt_fromdate" PopupButtonID="img1" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                </div>
                 
                 <div class="form-group col-md-6 col-sm-6 col-xs-12" > 
                            
                    <label for="To Date" >To Date :</label>

                     <div class="clearfix"></div>

                     <div class="col-md-8 col-sm-8 col-xs-8 paddingleft0 ">
                     <asp:TextBox ID="TextBox1" Enabled="false" runat="server" CssClass="textbox form-control" OnTextChanged="TextBox1_TextChanged" AutoPostBack="true"></asp:TextBox>
                     </div>
                     
                     <div class="col-md-1 col-xs-2 no-padding">
                        <a href="javascript:;" class="cal-icon" ID="ImageButton1" runat="server"><i class="fa fa-calendar" aria-hidden="true"></i></a>
                             
                             </div>
<%--                     <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/Calendar.png" />--%>

                      <div class="col-md-3 col-sm-3 col-xs-6 paddingright0 margintop10-xs">
                            <asp:TextBox ID="TextBox2" runat="server" CssClass="textbox form-control" MaxLength="5" ></asp:TextBox>

                          </div>
                            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TextBox1" PopupButtonID="ImageButton1" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                </div>
				<div class="clearfix"></div>
                <div class="form-group col-md-6 " style="margin-top:10px" > 
                    <asp:Button ID="Button3" runat="server" CssClass="btn btn-primary" 
                                 Text="Generate" OnClientClick="return validate()" onclick="Button3_Click" /> </td>
                                 <td colspan="4" valign="right">
                             <asp:Button ID="btndelete" runat="server" CssClass="btn btn-primary" Text="Delete"
                                         Visible="false" CausesValidation="false" onclick="btndelete_Click" />
                </div>
                </div></div><div class="clearfix"></div>
               
                <div class="table-responsive">
            <table class="table">
                    <tr>
                        <td colspan="4">
                            <div id="Div1" style="  float: left; margin-left: 0px; padding-top: 0px; background-color: #FFF;">
                                <asp:GridView ID="gvData" runat="server" class="table" AutoGenerateColumns="False" EmptyDataText="No Records Found!"
                                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                    CellPadding="3" GridLines="Vertical" Width="100%">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <EmptyDataRowStyle BackColor="#3c8dbc" ForeColor="Black" />
                                    <Columns>
                                      <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                               <div style="text-align:center;"> <asp:CheckBox ID="ckViewHead" runat="server" Text="Select" onclick="SelectAllByRow(this, 2)" /></div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ckView" runat="server" />
                                <asp:HiddenField ID="hid" runat="server" Value='<%# Bind("Id") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                                   <asp:BoundField DataField="Currentdate" HeaderText="Date Time" ItemStyle-Width="150px"
                                            DataFormatString="{0:dd/MMM/yyyy HH:mm}" />
                                        <asp:BoundField DataField="latitude"  HeaderText="Lat" />
                                        <asp:BoundField DataField="longitude" HeaderText="Long" />
                                          <asp:BoundField DataField="address" HeaderText="Address" />
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

