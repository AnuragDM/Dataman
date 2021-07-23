<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/FFMS.master" CodeBehind="MovementSummaryPersonWise.aspx.cs" Inherits="AstralFFMS.MovementSummaryPersonWise" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
            function ClientItemSelected(sender, e) {

                //  alert(e.get_value());
                $get("<%=hfpersonid.ClientID %>").value = e.get_value();
            }
    </script>
      <script type="text/javascript">
          function validate1() {
             <%-- if (document.getElementById("<%=DropDownList1.ClientID%>").value == "--Select--" || document.getElementById("<%=DropDownList1.ClientID%>").value == "0") {
                  errormessage("Please Select Group Name");
                  document.getElementById("<%=DropDownList1.ClientID%>").focus();
                  return false;
              }--%>
           <%-- if (document.getElementById("<%=DropDownList3.ClientID%>").value == "--Select--" || document.getElementById("<%=DropDownList3.ClientID%>").value == "0") {
                errormessage("Please Select Person Name");
                document.getElementById("<%=DropDownList3.ClientID%>").focus();
                return false;
            }--%>
             <%-- if (document.getElementById("<%=ddlperson.ClientID%>").value == "--Select--") {
                  errormessage("Please Select Atleast One Person");
                return false;
            }--%>
              if (document.getElementById("<%=TextBox4.ClientID%>").value == "" || document.getElementById("<%=TextBox4.ClientID%>").value == "00:00") {
                  errormessage("Your Time Format is incorrect. Please try again.");
                  document.getElementById("<%=TextBox4.ClientID%>").focus();
                return false;
            }
            var digits = /^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$/;
            var digitsid = document.getElementById("<%=TextBox4.ClientID %>").value;
            var digitsArray = digitsid.match(digits);
            var temp;
            if (digitsArray == null) {
                errormessage("Your Time Format is incorrect. Please try again.");
                document.getElementById("<%=TextBox4.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=TextBox6.ClientID%>").value == "" || document.getElementById("<%=TextBox6.ClientID%>").value == "00:00") {
                  errormessage("Your Time Format is incorrect. Please try again.");
                  document.getElementById("<%=TextBox6.ClientID%>").focus();
                return false;
            }
            var digits = /^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$/;
            var digitsid = document.getElementById("<%=TextBox6.ClientID %>").value;
            var digitsArray = digitsid.match(digits);
            var temp;
            if (digitsArray == null) {
                errormessage("Your Time seems incorrect. Please try again.");
                document.getElementById("<%=TextBox6.ClientID %>").focus();
                return false;
            }



            var start = document.getElementById("<%=TextBox4.ClientID %>").value;
              var end = document.getElementById("<%=TextBox6.ClientID %>").value;
              var dtStart = new Date("1/1/2007 " + start);
              var dtEnd = new Date("1/1/2007 " + end);
              if (Date.parse(dtStart) > Date.parse(dtEnd)) {
                  errormessage("To Time Should be greater then From Time");
                  document.getElementById("<%=TextBox6.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DropDownList4.ClientID%>").value == "--Select--" || document.getElementById("<%=DropDownList4.ClientID%>").value == "0") {
                  errormessage("Please Select Interval Min.");
                  document.getElementById("<%=DropDownList4.ClientID%>").focus();
                return false;
            }
        }

    </script>
    <section class="content">
        <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <asp:Image ID="Image1" ImageUrl="~/img/waiting.gif" AlternateText="Processing"
                runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
     <ajax:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
        PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />
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
                            <%--<h3 class="box-title">Movement Summary PersonWise</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            </div>
                    
                        <div class="clearfix"></div>
                        
                             <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Always">
        <ContentTemplate>
         <div class="container-fluid" style="min-height: 600px;">
             <div class="col-md-6"></div>
            <div class="col-md-6"><asp:Button ID="btnexport" runat="server" CssClass="btn btn-primary pull-right" Text="Export" Visible="false"
                            OnClick="btnexport_Click" /></div>
             <div class="clearfix"></div>
           <div class="col-md-12 col-sm-12 col-xs-11">
            <div class="row">
                <div class="form-group col-md-4 col-sm-6 col-xs-12 paddingleft0" style="display:none;" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Group Name" >Group :</label>
                    <asp:DropDownList ID="DropDownList1" CssClass="textbox form-control" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
                                AutoPostBack="true" Width="100%" runat="server">
                            </asp:DropDownList>
                </div>
                <div class="form-group col-md-4 col-sm-6 col-xs-12" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Person" >Person :</label>
                  <%--  <asp:DropDownList ID="DropDownList3" CssClass="textbox form-control" Width="100%" runat="server" AutoPostBack="true"  onselectedindexchanged="DropDownList3_SelectedIndexChanged">
                            </asp:DropDownList>--%>
                       <%--  <asp:DropDownCheckBoxes ID="ddlperson" runat="server" AddJQueryReference="false"
                                UseSelectAllNode = "false" CssClass="form-control">
 <Texts SelectBoxCaption="--Select All--" />  
    <Style2 SelectBoxWidth="100%"  DropDownBoxBoxHeight="200"  />   
</asp:DropDownCheckBoxes> --%>
                      <asp:TextBox ID="txtSearch" runat="server" width="86%" class="form-control" placeholder="Enter L-10 Name" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
                                     <ajax:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" FirstRowSelected="false" OnClientItemSelected="ClientItemSelected"
                                      runat="server" BehaviorID="txtSearch_AutoCompleteExtender" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem" 
                                      CompletionInterval="0" CompletionListHighlightedItemCssClass="itemHighlighted" DelimiterCharacters="" ServiceMethod="SearchItem" 
                                      ServicePath="~/MovementSummaryPersonWise.aspx" MinimumPrefixLength="3" EnableCaching="true" TargetControlID="txtSearch">
                                     </ajax:AutoCompleteExtender>
                                     <asp:HiddenField ID="hfpersonid" runat="server" />
                </div>
                <div class="clearfix"></div>
                <div class="row" style="margin:11px; padding:3px;">
                <div class="form-group col-md-4 col-sm-6 col-xs-12 paddingleft0" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="From" >From Date :</label>
                    <div class="clearfix"></div>
                    <div class="col-md-7 col-sm-5 col-xs-7 paddingleft0">
                    <asp:TextBox ID="TextBox3" Enabled="false" width="80%" runat="server" OnTextChanged="TextBox3_TextChanged" AutoPostBack="true"
                             CssClass="textbox form-control"></asp:TextBox>
                        </div>
                       
                     <div class="col-md-5 col-sm-5 col-xs-4 no-padding">
                        <a href="javascript:;" class="cal-icon" ID="ImageButton2" runat="server"><i class="fa fa-calendar" style="margin:-90px" aria-hidden="true"></i></a>
                      <div class="col-md-3 col-sm-3 col-xs-5 paddingright0">
                      
                            <asp:TextBox ID="TextBox4" runat="server" width="250%" MaxLength="6" CssClass="textbox form-control movementmrg"></asp:TextBox>
                    </div>
                     </div>
                        
                       <%-- <asp:ImageButton
                                    ID="ImageButton2" runat="server" ImageUrl="~/img/Calendar.png" />--%>
    

                     
                         
                                 <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TextBox3"
                                PopupButtonID="ImageButton2" Format="dd-MMM-yyyy">
                            </ajax:CalendarExtender>
                </div>
                <div class="form-group col-md-4 col-sm-6 col-xs-12 paddingleft0" >
                 <label for="requiredfield" class="back">*</label>
                   <label for="To" >To Date :</label>
                    <div class="clearfix"></div>
                    <div class="col-md-7 col-sm-5 col-xs-7 paddingleft0">
                    <asp:TextBox ID="TextBox5" Enabled="false" width="80%" runat="server" CssClass="textbox form-control" OnTextChanged="TextBox5_TextChanged" AutoPostBack="true"
                        ></asp:TextBox>
                        
                        </div>

                    <div class="col-md-5 col-sm-5 col-xs-4 no-padding">
                        <a href="javascript:;" class="cal-icon" ID="ImageButton3" runat="server"><i class="fa fa-calendar" style="margin:-90px" aria-hidden="true"></i></a>
                      <div class="col-md-3 col-sm-3 col-xs-5 paddingright0">

                            <asp:TextBox ID="TextBox6" runat="server" width="250%" MaxLength="6" CssClass="textbox form-control" ></asp:TextBox>

                         </div>
                    </div>

<%--                        <asp:ImageButton ID="ImageButton3"
                                    runat="server" ImageUrl="~/img/Calendar.png" />--%>

                     

                            <ajax:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="TextBox5"
                                PopupButtonID="ImageButton3" Format="dd-MMM-yyyy">
                            </ajax:CalendarExtender>
                </div>
                     
                <div class="form-group col-md-2 col-sm-2 col-xs-6 paddingleft0" >
             <label for="requiredfield" class="back">*</label>
                   <label for="Interval" >Interval :</label>
                    <asp:DropDownList ID="DropDownList4" CssClass="textbox form-control" runat="server">
                                <asp:ListItem Text="30 Min" Value="30"></asp:ListItem>
                                <asp:ListItem Text="60 Min" Selected="True" Value="60"></asp:ListItem>
                              <%--  <asp:ListItem Text="90 Min" Value="90"></asp:ListItem>--%>
                            </asp:DropDownList>
                            <asp:HiddenField ID="txtaccu" runat="server" />
                </div>
                </div>
                 <div class="clearfix"></div>    
                <div class="form-group col-md-6 col-sm-6 col-xs-12" style="padding:-3px" >
                    <asp:Button ID="Submit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="Submit_Click"
                                OnClientClick="return validate1()" />
                </div>

                  
         </div></div>
                 <div class="clearfix"></div>    
                <div class="table-responsive">
                <table class="table">
                    <tr>
                        <td colspan="4">

                            <div style=" display:none;">
                                <asp:GridView ID="GridView1" class="table" runat="server" AutoGenerateColumns="False" >
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                    </Columns>
                                 
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    </div>
                    <div class="table-responsive">
                <table class="table">
                    <tr>
                    <td colspan="4">
                       <div style=" width: 100%;">
                    <asp:GridView ID="gdw" class="table table-bordered gridclass" runat="server" AutoGenerateColumns="true" BackColor="White" 
                                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"  PagerSettings-Position="Bottom" 
                                                      OnPageIndexChanging="gdw_PageIndexChanging"       PagerStyle-HorizontalAlign="Left" PagerStyle-BackColor="Aqua" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last"
                            PagerSettings-PreviousPageText="<<" PagerSettings-NextPageText=">>" 
                               CellPadding="3" GridLines="Vertical"
                                    Width="100%" onrowdatabound="gdw_RowDataBound">

                                      <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <PagerStyle BackColor="#3c8dbc" ForeColor="White"  /> 
                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#3C8DBC" Font-Bold="True" ForeColor="White" />
                              <AlternatingRowStyle BackColor="#FFFFFF" />
                                    </asp:GridView>
                                    </div>
                    </td></tr>
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