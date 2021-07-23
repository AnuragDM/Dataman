<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/FFMS.master" CodeBehind="TourDaysReport.aspx.cs" Inherits="AstralFFMS.TourDaysReport" %>
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

            if (document.getElementById("<%=ddlyear.ClientID%>").value == "--Select--" || document.getElementById("<%=ddlyear.ClientID%>").value == "0") {
                 errormessage("Please Select a Year");
                 document.getElementById("<%=ddlyear.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=ddlmonth.ClientID%>").value == "--Select--" || document.getElementById("<%=ddlmonth.ClientID%>").value == "0") {
                 errormessage("Please Select a Month");
                 document.getElementById("<%=ddlmonth.ClientID%>").focus();
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
                            <h3 class="box-title">Tour Report</h3>
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
          <div class="col-md-9 col-sm-9 col-xs-10">
            <div class="row">
                <div class="form-group col-md-3 col-sm-4 col-xs-12 paddingleft0" style="display:none;" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Group Name" >Group :</label>
                    <asp:DropDownList ID="ddlType" CssClass="textbox form-control" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true" Width="100%" runat="server">
                            </asp:DropDownList>
                </div>
                
                <div class="form-group col-md-3 col-sm-4 col-xs-12" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Person" >Person :</label>
                    <asp:DropDownList ID="DropDownList2" CssClass="textbox form-control" Width="100%" runat="server"  AutoPostBack="true"  onselectedindexchanged="DropDownList2_SelectedIndexChanged">
                            </asp:DropDownList>
                </div>
               
				<div class="form-group col-md-3 col-sm-6 col-xs-6 " >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Year" >Year :</label>
                    <asp:DropDownList ID="ddlyear" CssClass="textbox form-control" runat="server">
                                <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="2016" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2017" Value="2"></asp:ListItem>
                                <asp:ListItem Text="2018" Value="3"></asp:ListItem>
                           <asp:ListItem Text="2019" Value="4"></asp:ListItem>
                                <asp:ListItem Text="2020" Value="5"></asp:ListItem>
                            </asp:DropDownList>
                               <asp:HiddenField ID="txtaccu" runat="server" />
                </div>
                <div class="form-group col-md-3 col-sm-4 col-xs-6" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Month" >Month :</label>
                    <asp:DropDownList ID="ddlmonth" CssClass="textbox form-control" runat="server">
                                <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="January" Value="1"></asp:ListItem>
                                <asp:ListItem Text="February" Value="2"></asp:ListItem>
                                <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                <asp:ListItem Text="December" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                </div>

                <div class="clearfix"></div>

                <div class="form-group col-md-6 col-sm-6 col-xs-12 margintop20" style="padding:15px;" >
                     <asp:Button ID="btngenerate" runat="server" CssClass="btn btn-primary" Text="Generate" OnClick="btngenerate_Click" OnClientClick="return validate()" />
                </div>
                </div></div>
               <div class="clearfix"></div>
                <div class="table-responsive">
            
                              <div id="Div1" style="  float: left; margin-left: 0px; padding-top: 0px; background-color: #FFF;">
                                <asp:GridView ID="gvData" class="table gridclass" runat="server" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                    CellPadding="3" GridLines="Vertical" Width="100%">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <EmptyDataRowStyle BackColor="#3c8dbc" ForeColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Cdate" HeaderText="Date"
                                            DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="days" HeaderText="Day of week" />
                                         <asp:BoundField DataField="distance" HeaderText="Distance Travelled (Km)" />
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
