<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="RoleMatrix.aspx.cs" Inherits="AstralFFMS.RoleMatrix" EnableEventValidation="false" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style type="text/css">
        .spinner {
            position: absolute;
            left: 50%;
            margin-left: -50px; /* half width of the spinner gif */
            margin-top: -50px; /* half height of the spinner gif */
            text-align: center;
            z-index: 999;
            overflow: auto;
            width: 100px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }
        #select2-ContentPlaceHolder1_ddlParentLoc-container {
        margin-top:-8px !important;
        }
    </style>
   
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
        <div id="divData" runat="server">
      
                                        <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-md-12">

                    <div class="box">
                        <div class="box-header">
                           <asp:Label ID="lblrolematrix" style="font-size:19px;" runat="server" Text='<%# Eval("DisplayName")%>'></asp:Label>
                       </div>
                        <!-- /.box-header --> 
                        <div class="box-body table-responsive">
                            <div class="col-md-3">
                                <label>Display Type:</label> 
                            <asp:DropDownList ID="ddltype" AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                           </div>
                            <div class="col-md-3">
                               <label>Role:</label>
                              <asp:DropDownList ID="ddlrole" runat="server" CssClass="form-control"></asp:DropDownList>
                           </div>
                            
                            <div class="col-md-3">
                                <label>Module:</label> 
                            <asp:DropDownList ID="ddlmodule" runat="server" CssClass="form-control"></asp:DropDownList>
                           </div>  
                               
                            <div class="col-md-2">
                                <label style="visibility:hidden;">sdfgg</label><br/>
                                 <asp:Button ID="btn" style="float:left;padding:3px 10px;" runat="server" CssClass="btn btn-primary" OnClick="btn_Click" Text="Show"/>
                           </div>      
                            
                             
                           <div class="clearfix"></div>
                            <br /><br />
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Role</th>
                                                <th>Module</th>
                                                <th>Page</th>
                                                <th>View</th>
                                                <th>Add</th>
                                                <th>Edit</th>
                                                <th>Delete</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                         <%--  <asp:HiddenField ID="hdfExpGroupId" runat="server" Value='<%#Eval("ExpenseGroupId") %>' />--%>
                                          
                                      <td><%#Eval("RoleName") %></td>
                                     <td><%#Eval("Module") %></td>
                                      <td><%#Eval("DisplayName") %></td>     
                                      <td><%#Eval("ViewP") %></td>     
                                         <td><%#Eval("AddP") %></td>     
                                         <td><%#Eval("EditP") %></td>     
                                         <td><%#Eval("DeleteP") %></td>     
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                           
                        </div></div>
                  
                        </div></div></div>
                                    </div>

        

    </section>
     
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();

        });
    </script>
</asp:Content>

