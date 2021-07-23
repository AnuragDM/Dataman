<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="UploadArea.aspx.cs"  EnableEventValidation="false" Inherits="FFMS.UploadArea" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
        <script type="text/javascript" >
        function validate()
        {
            if (document.getElementById("<%=ddlArea.ClientID%>").value == "0") {
                alert("Please Select an Area Type")
                return false;
            }
            return true;
        }
           
    </script>
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
                            <h3 class="box-title">Import Location</h3>
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
                        </div>
                       
                    </div>
                </div>
               
            </div>
        </div>
    </section>
</asp:Content>
