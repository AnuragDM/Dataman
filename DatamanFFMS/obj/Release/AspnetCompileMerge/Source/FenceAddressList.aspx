<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.master" AutoEventWireup="true" Inherits="FenceAddressList" Codebehind="FenceAddressList.aspx.cs" %>
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
           
    <asp:UpdatePanel ID="Up1" runat="server" UpdateMode="Always">
         <ContentTemplate>
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Fence Address [List]</h3>
                            <div style="float: right">
                               <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Add" PostBackUrl="~/GeoFencing.aspx"/>

                            </div>
                            </div>
                    
                        <div class="clearfix"></div>
                                     <div class="box-header col-md-10">
                                  <div class="row">

                             <div class="form-group col-md-5">
                                   
                                <label for="requiredfield" class="back">*</label>

                    <label for="Group Name" >Created By :</label>
                                 <asp:DropDownList ID="ddlperson" runat="server" CssClass="dropdown form-control" Width="100%"></asp:DropDownList> 
                                 </div>
                             <div class="form-group col-md-4">
                                  
                                   <label for="Group Name" >Created Date :</label>
                       <div class="col-md-10 col-sm-5 col-xs-10 no-padding">
                                       <asp:TextBox ID="txt_createddate" runat="server" Enabled="false" OnTextChanged="txt_createddate_TextChanged" AutoPostBack="true"
                          CssClass="textbox form-control"  ></asp:TextBox>
                       
                           </div>
                           

                    <div class="col-md-1 col-xs-1 col-sm-1 no-padding">
                        <a href="javascript:;" class="cal-icon" ID="img1" runat="server"><i class="fa fa-calendar" aria-hidden="true"></i></a>
                          <!-- <asp:ImageButton ID="img11" runat="server" ImageUrl="~/img/Calendar.png" />--></div>
                            <ajax:CalendarExtender ID="cc1" runat="server" TargetControlID="txt_createddate" PopupButtonID="img1" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                 </div>
                                  <div class="col-md-2 no-padding margintop20">
                            <asp:Button ID="btnsearch" runat="server" Text="Search" 
                          CssClass="btn btn-primary" onclick="btnsearch_Click" /> 
                                   </div>
                                      </div>
                       </div>
                             <div class="clearfix"></div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                             <asp:GridView ID="gvData" runat="server" class="table table-bordered gridclass" AutoGenerateColumns="False" 
                            BackColor="White"
                            BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" PageSize="15" PagerSettings-Position="Bottom" PagerStyle-HorizontalAlign="Left"
                            CellPadding="3" GridLines="Vertical" EmptyDataText="No Record Found"
                           PagerStyle-BackColor="Aqua" 
                            Width="100%" DataKeyNames="Id" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last"
                            PagerSettings-PreviousPageText="<<" PagerSettings-NextPageText=">>"
                            onpageindexchanging="gvData_PageIndexChanging" AllowPaging="True" 
                            onrowcancelingedit="gvData_RowCancelingEdit" onrowediting="gvData_RowEditing" 
                            onrowupdating="gvData_RowUpdating" onrowdeleting="gvData_RowDeleting">
                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                             <EmptyDataRowStyle BackColor="#3c8dbc" ForeColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sno.">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Action">  
                    <ItemTemplate>  
                     <%--   <asp:ImageButton ID="" CommandName="Edit" AlternateText="Edit" ImageUrl="~/delete.png" ToolTip="Edit" />--%>
                        <div class="" style="position:relative">
                        <asp:Button ID="btn_Edit" runat="server" ForeColor="" Text="&#9997;" CommandName="Edit" CssClass="btn btn-success editgridbtn" ToolTip="Edit" />
                        <%--<div class="editgridicon"><i class="fa fa-pencil-square-o"></i></div>--%>
                             <asp:LinkButton CommandName="delete" ID="LinkButton1"  CausesValidation="false" runat="server" OnClientClick="javascript:if (!confirm('Are you sure you want to delete this record ?')) return false;"
                                           CssClass="btn btn-danger"  Text="Delete"><i class="fa fa-close"></i></asp:LinkButton>
                            </div>
                    </ItemTemplate>  
                                        
                    <EditItemTemplate>  
                        <asp:Button ID="btn_Update" runat="server" ForeColor="Blue" Text="Update" CommandName="Update"/>  
                        <asp:Button ID="btn_Cancel" runat="server" ForeColor="Blue" Text="Cancel" CommandName="Cancel"/>  
                    </EditItemTemplate>  
                </asp:TemplateField>   
                                         <%--  <asp:CommandField ShowEditButton="true" ItemStyle-ForeColor="Blue" ButtonType="Image" Visible="true"/>--%>
                    
                                     <asp:BoundField DataField="CLat" HeaderText="Latitude" ReadOnly="true"/>
                                     <asp:BoundField DataField="CLong" HeaderText="Longitude" ReadOnly="true" />
                                     <asp:BoundField DataField="Radius" HeaderText="Radius(Km)" ReadOnly="true"/>
                                <asp:TemplateField HeaderText="Fence Address">  
                    <ItemTemplate>  
                        <asp:Label ID="lbl_Address" runat="server"  Text='<%#Eval("Address") %>'></asp:Label>  
                        <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>' Visible="false"></asp:Label>
                      
                    </ItemTemplate>  
                    <EditItemTemplate>  
                        <asp:TextBox ID="txt_Address" runat="server" Width="100%" CssClass="EditGridTextbox" Text='<%#Eval("Address") %>'></asp:TextBox>  
                    </EditItemTemplate>  
                </asp:TemplateField>   
                                  <%--   <asp:BoundField DataField="Address" HeaderText="" />--%>
                                     <asp:BoundField DataField="Description" HeaderText="Fence Group" ReadOnly="true" />
                                     <asp:BoundField DataField="PersonName" HeaderText="Created By" ReadOnly="true"/>
                                     <asp:BoundField DataField="Createddate" HeaderText="Created Date" ItemStyle-Width="100px" ReadOnly="true"/>
                                       
                            </Columns>
                                 <%--<PagerSettings Mode="NextPreviousFirstLast" FirstPageText="First" PreviousPageText="Previous" NextPageText="Next" LastPageText="Last" />--%>
                                 <PagerSettings Mode="NumericFirstLast" PageButtonCount="10"  FirstPageText="First" LastPageText="Last"/>
                           <FooterStyle BackColor="#CCCCCC" ForeColor="Black"  CssClass="GridPaging"/>
                            <PagerStyle BackColor="#3c8dbc" ForeColor="White"  /> 
                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#3C8DBC" Font-Bold="True" ForeColor="White" />
                              <AlternatingRowStyle BackColor="#FFFFFF" />
                        </asp:GridView>
                        </div>
                        <!-- /.box-body -->
                    </div>
                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>
             </ContentTemplate>
        </asp:UpdatePanel>
        </div>
    </section>
   
    
  
</asp:Content>
