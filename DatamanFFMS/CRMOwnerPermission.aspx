<%@ Page Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true"  EnableEventValidation="false" CodeBehind="CRMOwnerPermission.aspx.cs" Inherits="AstralFFMS.CRMOwnerPermission" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <%--  <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style>
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
             .modalBackground {
         background-color: Gray;
         filter: alpha(opacity=80);
         opacity: 0.8;
         z-index: 1000;
     }
    </style>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>

    <script type="text/javascript">

        function validatetask() {


           
            if ($('#<%=ddlSales.ClientID%>').val() == "0") {
                errormessage("Please select the Manager");
                return false;
            }
        }
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
                width: 250, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 1000, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
     <script type="text/javascript">
         var V1 = "";
         function ManagerNotification(V1) {
             $("#messageNotification").jqxNotification({
                 width: 250, position: "top-right", opacity: 2,
                 autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 1000, template: "error"
             });
             $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
    </script>

    <script type="text/javascript">
        var V = "";
        function Successmessage(V) {
         //   alert('xx');
            $("#messageNotification").jqxNotification({
                width: 250, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 1000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>

 
    <section class="content">
        <style type="text/css">
       
        .GridPager a
        {
            display:block;
            height:20px;
            width:15px;
            background-color:#3c8dbc;
            color:#fff;
            font-weight:bold;
           
            text-align:center;
            text-decoration:none;
        }
         .GridPager span
        {
            display:block;
            height:20px;
            width:15px;
            background-color:#fff;
            color:#3c8dbc;
            font-weight:bold;
           
            text-align:center;
            text-decoration:none;
        }
    </style>
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>

          <asp:Button ID="Button2" runat="server" style="display:none" />
                            <asp:HiddenField ID="HiddenField2" runat="server" />
            <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="Button2" PopupControlID="pnlmessage"
 BackgroundCssClass="modalBackground" ></cc1:ModalPopupExtender>

<asp:Panel ID="pnlmessage" runat="server"  ScrollBars="Vertical"  BackColor="White"  style="margin-top:10px;height:15%;display:none;width:30%">

        <div id="Div1"  runat="server"  class="spinner" style="display: none;">
            <img id="img-spinner2" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
        </div>
  
<table width="100%"  height:"100%" cellpadding="6" class="table" cellspacing="0">
<tr>
    <td colspan="2" style="
    text-align: center;
    text-decoration: solid;
    font-weight: bolder;">
        Do you want to delete Manager
        </td>
</tr>

<tr align="center">
    
<td colspan="2" style="
    text-align: center;">
<asp:Button ID="btnYes"   TabIndex="20" runat="server"  class="btn btn-primary" Text="Yes" OnClick="btnYes_Click"/>
<asp:Button ID="btnno" class="btn btn-primary" runat="server" Text="No" TabIndex="22" OnClick="btnno_Click" />
</td>
</tr>
</table>
   
</asp:Panel>


        

        <asp:Button ID="btnShowPopup" runat="server" style="display:none" />
                            <asp:HiddenField ID="hidForModel" runat="server" />
    <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
 BackgroundCssClass="modalBackground"></cc1:ModalPopupExtender>

<asp:Panel ID="pnlpopup" runat="server"  ScrollBars="Vertical"  BackColor="White"  style="margin-top:10px;height:60%;display:none;width:50%;">

        <div id="spinner1"  runat="server"  class="spinner" style="display: none;">
            <img id="img-spinner1" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
        </div>
 <table width="100%"  height:"100%" cellpadding="6" class="table" cellspacing="0">
     <tr>
         <td colspan="2">
                            <h3>Select new Manager
                            </h3>
             </td>
         </tr>
  
       <tr>
         <td >

                                   <div class="form-group">
                                            <label>Sales Persons: </label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <div class="clearfix"></div>
                                            <asp:DropDownList ID="ddlSales" runat="server" Width="100%" CssClass="form-control" TabIndex="7"></asp:DropDownList>
                                          
                                        </div>
                                     </td>
              <td>
                                   
                                       
                                </td>


           </tr>
   
     
       <tr>
           <td>
                                   
                                    <div class="col-md-6 paddingleft0">
                                        <%--style="padding-bottom: 10px;"--%>
                                        <asp:Button ID="btnsavemanager"   TabIndex="20" runat="server"  class="btn btn-primary" Text="Save" OnClientClick="return validatetask();" OnClick="btnsavemanager_Click" />
<asp:Button ID="btncancel1" class="btn btn-primary" runat="server" Text="cancel" TabIndex="22" OnClick="btnCancel1_Click"/>
                                     
                                    </div>
               </td>
           </tr>
</table>

                      

     <div style="height: 30px;padding-left:15px;">
 <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color:red">*</span>)</b>
        <br/></div>    
</asp:Panel>


        <div class="row">
            <!-- left column -->
            <div class="col-md-12">
                <div id="InputWork">
                    <!-- general form elements -->
                    <div class="box box-primary">
                        <div class="box-header with-border">
                            <h3 class="box-title">Owner Permissions</h3>

                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-md-4">
                                <%--<div class="col-md-4 col-sm-5 col-xs-6" style="float:left">--%>
                                <%--<h4 class="userh1"><label id="lblcrmusername"></label></h4>--%>
                                 <asp:label Id="LblLead" style="font-size:31px;" runat="server"  ></asp:label>
                                
                                <%--   <p style="font-size:31px;"><label id="lblcrmusername"></label>
        </p>--%>
                            </div>
                             <div class="col-md-8" style="text-align:right;">
                                <%--<div class="col-md-4 col-sm-5 col-xs-6" style="float:left">--%>
                                <%--<h4 class="userh1"><label id="lblcrmusername"></label></h4>--%>
                                  <asp:Button ID="Button1" runat="server" Text="Back" CssClass="btn btn-primary" PostBackUrl="~/CRMTask.aspx" />
                                
                                <%--   <p style="font-size:31px;"><label id="lblcrmusername"></label>
        </p>--%>
                            </div>
                           <%-- <div class="col-lg-9 col-md-8 col-sm-7 col-xs-9">
                            <label id="lblcols" runat="server" style="color: red; font-family: Arial; font-size: small; font-weight: 300;" visible="false"></label>
                            <div class="form-group col-md-4">
                                <input id="Userid" hidden="hidden" />
                                <label for="exampleInputEmail1">Role:</label>

                                
                                <asp:DropDownList ID="ddlRole" Width="100%" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                              
                                
                               
                            </div>
                             <div class="form-group col-md-4">
                                <label for="exampleInputEmail1">Module:</label>

                                
                                <asp:DropDownList ID="ddlModule" Width="100%" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-md-4">
                                <label for="exampleInputEmail1" style="display:block; visibility:hidden">zkjfhksj</label>
                                
                                 <asp:Button ID="Button1" runat="server" style="padding: 3px 14px;" CssClass="btn btn-primary" Text="Show" OnClientClick="return validate();" OnClick="btnshow_Click" />
                            </div>
                             </div>--%>
                            <div class="clearfix"></div>
                            <div class="form-group table-responsive">
                                <asp:GridView ID="gvData" class="table" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    EmptyDataText="No Records Found"
                                    CellPadding="3" BorderWidth="0" GridLines="Vertical" 
                                    Width="100%" DataKeyNames="SMID"  OnSelectedIndexChanged="gvData_SelectedIndexChanged" OnRowCommand="gvData_RowCommand" OnRowUpdated="gvData_RowUpdated" >
                                    <RowStyle ForeColor="Black" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno." HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" CssClass="small-align" Width="100px" />
                                        </asp:TemplateField>
                                          <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ckViewHead" runat="server" Text="Permission" onclick="SelectAllByRow(this, 2)"  />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckView" Checked='<%# Bind("Owner") %>' runat="server" AutoPostBack="true" OnCheckedChanged="ckView_CheckedChanged"  />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField  DataField="SMName" HeaderText="Sales Persons" ItemStyle-CssClass="small-align" />
                                      
                                      
                                      
                                           
                                          
                                        <%--  <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate> 
                        <asp:CheckBox ID="ckExportHead" runat="server" Text="Export" onclick="SelectAllByRow(this, 6)" />
                    </HeaderTemplate > 
                    <ItemTemplate>
                        <asp:CheckBox ID="ckExport" Checked='<%# Bind("ExportP") %>' runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>--%>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <PagerStyle HorizontalAlign="Center"  CssClass="GridPager" BackColor="#3c8dbc"  />
                                    <SelectedRowStyle BackColor="#008A8C" HorizontalAlign="Center" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#3c8dbc" HorizontalAlign="Right" Font-Bold="True" ForeColor="white" />
                                    <AlternatingRowStyle BackColor="#e8e8e8" />
                                </asp:GridView>

                            </div>

                            <div class="form-group" style="float: right" visible="false" runat="server" id="divbtns">
                                <asp:Button ID="btnSubmit" runat="server"
                                    Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click"/>
                                &nbsp;&nbsp;
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" PostBackUrl="~/CRMTask.aspx" />

                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </section>
</asp:Content>
