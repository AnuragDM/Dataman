<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="UploadIndustry.aspx.cs" Inherits="FFMS.UploadIndustry" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <section class="content">
    <div class="row">
            <!-- left column -->
            <div class="col-md-6">
                <div id="InputWork">
                    <!-- general form elements -->
                    <div class="box box-primary">
                        <div class="box-header with-border">
                            <h3 class="box-title">Import Industry</h3>
                      <div style="float:right">
                        <asp:HyperLink ID="hpl" runat="server" Text="Download Sample" NavigateUrl="~/SampleImportSheets/Industry.csv" ></asp:HyperLink>
                       
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
                            <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary" Text="Upload" OnClick="btnUpload_Click"  />
                   
                        </div> 
                        </div>
                       
                    </div>
                </div>
               
            </div>
        </div>
         </section>
</asp:Content>
