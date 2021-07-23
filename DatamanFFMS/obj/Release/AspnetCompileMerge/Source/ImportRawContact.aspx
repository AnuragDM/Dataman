<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ImportRawContact.aspx.cs" Inherits="AstralFFMS.ImportRawContact" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                            <h3 class="box-title">Import Raw Contact</h3>
                      <div style="float:right">
                      
                       <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Download CSV" ToolTip="Download Sample CSV" class="btn btn-primary"
                                OnClick="btnExport_Click" />
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
                            <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary" Text="Import" OnClick="btnUpload_Click"   />
                   
                        </div> 
                        </div>
                       
                    </div>
                </div>
               
            </div>
        </div>
    </section>
</asp:Content>
