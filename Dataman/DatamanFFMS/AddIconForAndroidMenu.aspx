<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="AddIconForAndroidMenu.aspx.cs" Inherits="AstralFFMS.AddIconForAndroidMenu" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


     <section class="content">
            <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>

         <div class="box-body" id="mainDiv"  runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Add Icon For Android Menu</h3>
                            <div style="float: right">
                              
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                       
                                <div class="box-body">
                                    <div class="col-lg-5 col-md-6 col-sm-7 col-xs-9">

                                        <div class="form-group paddingleft0">
                                            <label for="exampleInputEmail1">Form Name</label>
                                            <asp:DropDownList ID="ddlform" AutoPostBack="true"  runat="server" CssClass="form-control"></asp:DropDownList>

                                        </div>

                                        <div class="form-group col-md-6 paddingleft0">
                                            <label for="withSales">Image</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                          <asp:FileUpload ID="FileUpload1" runat="server" />
                                        </div>
                                    </div>
                                </div>

                                <div class="box-footer">
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                                   <%-- <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" Enabled="false" />
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" OnClick="btnDelete_Click" Enabled="false" />--%>
                                </div>
                           
                    
                    </div>
                </div>
            </div>
        </div>
         </section>

</asp:Content>
