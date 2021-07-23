<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.master" AutoEventWireup="True"  CodeBehind="GroupMaster.aspx.cs" Inherits="GroupMaster" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<%@ Register Src="ctlCalendar.ascx" TagName="Calendar" TagPrefix="ctl" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Group List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnAdd" OnClick="btnAdd_Click" runat="server" Text="Add" class="btn btn-primary" />

                            </div>
                            </div>
                    
                        <div class="clearfix"></div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                             <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False" class="table table-bordered gridclass" 
                            BorderColor="#3c8dbc" BorderStyle="Solid" BorderWidth="1px" OnRowDeleting="gvData_RowDeleting"
                            CellPadding="3" GridLines="Vertical"   AllowPaging="true" OnPageIndexChanging="gvData_PageIndexChanging" 
                                 PagerSettings-Position="Bottom" PagerStyle-HorizontalAlign="Left" 
                         PageSize="15"   Width="100%" DataKeyNames="Code" OnSelectedIndexChanged="gvData_SelectedIndexChanged">
                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                 <EmptyDataRowStyle BackColor="#3C8DBC" ForeColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sno.">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                  <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" HeaderText="Action">
                                    <ItemTemplate>
                                      <asp:LinkButton  CommandName="select" ID="LinkButton2" CausesValidation="false" runat="server" CssClass="btn btn-success"
                                            Text="Edit" ><i class="fa fa-pencil-square-o"></i></asp:LinkButton>
                                       <asp:LinkButton  CommandName="delete" ID="LinkButton1" CausesValidation="false" runat="server" OnClientClick="return ConfirmOnDelete('');"
                                            CssClass="btn btn-danger"  Text="Delete" ><i class="fa fa-close"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="120px"></ItemStyle>
                                </asp:TemplateField>
                               <asp:BoundField DataField="Description" HeaderText="Group Name" />
                                     <asp:BoundField DataField="Mobile" HeaderText="Mobile" />

                            </Columns>
                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
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

        </div>
    </section>
   
    
  
</asp:Content>

