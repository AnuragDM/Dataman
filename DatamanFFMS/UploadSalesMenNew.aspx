<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="UploadSalesMenNew.aspx.cs" Inherits="FFMS.UploadSalesMenNew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
                "order": [[0, "asc"]]
            });

        });
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
    <style>
        .matop {
                margin-top: 19px;
        }
        .btnGenrate {
            font-weight: 900;
            font-family: "Font Awesome 5 Free";
            color: #3c8dbc;
            background-color: transparent;
            border: none;
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
            <div class="col-md-12">
                <div id="InputWork">
                    <!-- general form elements -->
                    <div class="box box-primary">
                        <div class="box-header with-border">
                           <%-- <h3 class="box-title">Import Sales Person</h3>--%>
                             <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                      <div style="float:right">
                        <asp:HyperLink ID="hpl" runat="server" CssClass="btnGenrate" Text="Download Sample" NavigateUrl="~/SampleImportSheets/SalesPersonImport.xlsx" ></asp:HyperLink>
                          
                       
                        </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <label id="lblcols" runat="server" style="color:red;font-family:Arial;font-size:small;font-weight:300;" visible="false"></label>
                           

                            <div class="form-group">
                                <label for="exampleInputEmail1">File Name:</label>
                          
                                <asp:FileUpload runat="server" ID="Fupd" CssClass="btn btn-primary"  />
                            </div>
                        
                        <div class="form-group">
                            <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary" Text="Import" OnClick="btnUpload_Click"  />
                   
                        </div>
                             <div class="form-group col-md-3 col-sm-6 col-xs-12"  style="display:none;">
                                <label for="exampleInputEmail1">Search Date:</label>
                                      <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                  </div>
                             <div class="form-group col-md-3 col-sm-6 col-xs-12"  style="display:none;">
                                 <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary matop" Text="Search"  OnClick="btnSearch_Click" />
                                  </div> 
                        </div>
                         <div class="box-body" id="rptmain" runat="server" style="display:none;">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                         <div class="box-body table-responsive">
                            <asp:Repeater ID="rptDatabase" runat="server">
                                <HeaderTemplate>
                                    <div style="float:right;"><asp:Button Text="Download Imported Response File" ID="btnDownload" CssClass="btnGenrate" OnClick="btnDownload_Click" runat="server" Visible="true" /></div>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>SNo</th>
                                               <th>Result</th>
                                                 <th>ErrorType</th>                                             
                                                <th>Remark</th>    
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>                 
                                        <td> <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.ItemIndex+1 %>'></asp:Label></td>
                                        <td><%#Eval("Result") %></td>
                                       <td><%#Eval("ResultType") %></td>
                                    <td><div style="white-space:normal;width:100%;" ><%#Eval("Remark") %></div></td>
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
                    </div>
                </div>
               
            </div>
        </div>
       
    </section>
</asp:Content>
