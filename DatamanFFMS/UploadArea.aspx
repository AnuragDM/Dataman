<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="UploadArea.aspx.cs"  EnableEventValidation="false" Inherits="FFMS.UploadArea" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>

        <script type="text/javascript" >
        function validate()
        {
            if (document.getElementById("<%=ddlArea.ClientID%>").value == "0") {
                errormessage("Please Select an Area Type")
                return false;
            }
            return true;
        }
           
    </script>
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
    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
                "order": [[0, "desc"]]
            });

        });
    </script>
    <style>
        .matop {
                margin-top: 19px;
        }
    </style>
    <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <!-- left column -->
            <div class="col-md-6">
                <div id="InputWork">
                    <!-- general form elements -->
                    <div class="box box-primary">
                        <div class="box-header with-border">
                            <%--<h3 class="box-title">Import Location</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                      <div style="float:right">
                        <asp:HyperLink ID="hpldownload" runat="server" Visible="false" Text="Download Sample"></asp:HyperLink>
                       
                        </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <label id="lblcols" runat="server" style="color:red;font-family:Arial;font-size:small;font-weight:300;" visible="false"></label>
                            <div class="form-group">
                                <input id="Userid" hidden="hidden" />
                                <label for="exampleInputEmail1">Location Type:</label>
                      

                           &nbsp;&nbsp;   <asp:DropDownList ID="ddlArea" CssClass="" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged" >
                                  <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                  <asp:ListItem Text="Country" Value="1"></asp:ListItem>
                                  <asp:ListItem Text="Region" Value="2"></asp:ListItem>
                                  <asp:ListItem Text="State" Value="3"></asp:ListItem>
                                  <asp:ListItem Text="District" Value="4"></asp:ListItem>
                                  <asp:ListItem Text="City" Value="5"></asp:ListItem>
                                  <asp:ListItem Text="Area" Value="6"></asp:ListItem>
                                  <asp:ListItem Text="Beat" Value="7"></asp:ListItem>
                              </asp:DropDownList>
                            </div>

                            <div class="form-group">
                                <label for="exampleInputEmail1">File Name:</label>
                          
                                <asp:FileUpload runat="server" ID="Fupd" CssClass="btn btn-primary"  />
                            </div>
                        
                        <div class="form-group">
                            <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary" Text="Import"  OnClientClick="return validate();" OnClick="btnUpload_Click" />
                   
                        </div> 
                              <div class="form-group col-md-3 col-sm-6 col-xs-12" >
                                <label for="exampleInputEmail1">Search Date:</label>
                                      <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                  </div>
                             <div class="form-group col-md-3 col-sm-6 col-xs-12">
                                 <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary matop" Text="Search"  OnClientClick="return validate();" OnClick="btnSearch_Click" />
                                  </div>
                        </div>
                       
                    </div>
                </div>
               
            </div>
        </div>
         <div class="box-body" id="rptmain" runat="server" style="display:none;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                               
                                                <th>Row No</th>
                                                <%-- <th>City</th>--%>
                                                 <th>Column No</th>
                                                <th>Error</th>
                                               
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>                 
                                        <td><%#Eval("Row") %></td>
                                          <%--  <td><%#Eval("Parent") %></td>--%>
                                         <td><%#Eval("Column") %></td>
                                        <td><%#Eval("Error") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                         <div class="box-body table-responsive">
                            <asp:Repeater ID="rptDatabase" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                               
                                                <th>Date</th>
                                                <%-- <th>City</th>--%>
                                                 <th>Error</th>
                                           
                                               
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>                 
                                        <td><%#Eval("CreatedDate") %></td>
                                        <td><%#Eval("Error") %></td>
                                       
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                        </div>
                    </div>
                </div>
             </div>
    </section>
</asp:Content>
