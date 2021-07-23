<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="UploadItemPriceDistWise.aspx.cs" Inherits="AstralFFMS.UploadItemPriceDistWise" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 350, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: false, template: "error"
                //autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 10000, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
        function errormessage1(V1) {
            $("#messageNotification").jqxNotification({
                width: 350, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
        function errormessage3(V1) {
            $("#messageNotification").jqxNotification({
                width: 400, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: false, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
    </script>

     <section class="content">
         <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" width="30%" height="50%" alt="Loading" /><br />
            <b>Loading Data....</b>
        </div>
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
                           <%-- <h3 class="box-title">Import Item Price</h3>--%>
                             <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                      <div style="float:right">
                        <asp:HyperLink ID="hpl" runat="server" Text="Download Sample" NavigateUrl="~/SampleImportSheets/ItemPriceDistWise.csv" ></asp:HyperLink>
                       
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
                            <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary" Text="Upload" OnClick="btnUpload_Click"   />
                   
                        </div> 
                        </div>
                       
                    </div>
                </div>
               
            </div>
        </div>
          <%--<div class="box-body" id="rptmain" runat="server" style="display: none;">--%>
         <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Error List</h3>
                            <div style="float: right">
                              <%--  <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />--%>

                            </div>
                        </div>
                        <!-- /.box-header -->
                         <div class="row" style="margin-left:1px;">                                   
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">Date:</label>
                                            <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                        </div>
                                    </div> 
                             </div>
                             <div class="row" style="margin-left:1px;">         
                             <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div id="DIV2" class="form-group">
                              <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary"
                                            OnClick="btnGo_Click" />      
                                             </div>
                                    </div>                            
                                  <%--  <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                        </div>
                                    </div>--%>
                                </div>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>                                                
                                                <th>Date</th>
                                                <th>Error Description</th>                                                
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>   
                                        <td><%#Eval("Created_At") %></td>  
                                         <td><%#Eval("Error_desc") %></td>                                     
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
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
