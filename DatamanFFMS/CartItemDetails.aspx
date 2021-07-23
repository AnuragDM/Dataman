<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="CartItemDetails.aspx.cs" EnableEventValidation="false" Inherits="AstralFFMS.CartItemDetails" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to delete?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>

    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 250, position: "top-right", opacity: 2,
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
                width: 250, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>

        <style>
        .alignright{
        text-align:right;
        }

        .qty.text-right {text-align:right!important;
        }

    </style>
    <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <%--<h3 class="box-title">Cart Item Details</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>

                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary"
                                    OnClick="btnBack_Click" />
                            </div>
                        </div>
                        <div class="box-body" id="mainDiv" runat="server">
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-sm-4">
                                           <%-- <label for="exampleInputEmail1">Distributor:</label>--%>
                                        <b>
                                                <asp:Label ID="lblDistName" runat="server" Text="" ></asp:Label></b>
                                            <br />
                                            <asp:Label ID="lblDistDetails" runat="server" Text=""></asp:Label>

                                        </div>

                                        <div class="col-sm-3" style="color: #169434; font-family: Arial; font-weight: bold;">
                                            <label for="exampleInputEmail1">Credit Limit:</label>
                                            <asp:Label ID="lblCreditLimit" runat="server" Text=""></asp:Label>
                                        </div>
                                        <div class="col-sm-3" style="color: #169434; font-family: Arial; font-weight: bold;">
                                            <label for="exampleInputEmail1">Outstanding:</label>
                                            <asp:Label ID="lblOutstanding" runat="server" Text=""></asp:Label>
                                        </div>
                                        <div class="col-sm-2" style="color: #169434; font-family: Arial; font-weight: bold;">
                                            <label for="exampleInputEmail1">Open Orders:</label>
                                            <asp:Label ID="lblOpenOrders" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        &nbsp;
                                    </div>
                                </div>
                                <div class="row">
                                    <!-- /.box-header -->
                                    <div class="box-body table-responsive">
                                       <%-- <asp:GridView ID="gvCartItemDetails" ShowFooter="true" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Item ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCartID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                        <asp:Label ID="lblItemID" runat="server" Text='<%# Eval("ItemID") %>'></asp:Label>
                                                        <asp:Label ID="lblPrice" runat="server" Text='<%# Eval("Price") %>'></asp:Label>
                                                        <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Product" DataField="ItemName" />                                                
                                                <asp:BoundField HeaderText="Packing" DataField="Packing" ItemStyle-CssClass="qty text-right" HeaderStyle-CssClass="qty text-right"/>
                                                <asp:BoundField HeaderText="Qty" DataField="Qty" ItemStyle-CssClass="qty text-right" HeaderStyle-CssClass="qty text-right"/>
                                                <asp:BoundField HeaderText="Price Per Unit" DataField="PricePerUnit" />
                                                <asp:TemplateField HeaderText="Total">
                                                    <FooterTemplate>
                                                        <b>Total:
                                                            <asp:Label ID="lblAmtTotal" runat="server"></asp:Label></b>                                                        
                                                    </FooterTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmt" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
                                                    </ItemTemplate>
                                                  <HeaderStyle CssClass="qty text-right"/>		
                                         <ItemStyle CssClass="qty text-right"/>	
                                                    <FooterStyle CssClass="qty text-right"/>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Group" DataField="PriceGroup" ItemStyle-CssClass="qty text-right" HeaderStyle-CssClass="qty text-right"/>
                                                <asp:BoundField HeaderText="Remark" DataField="Remarks" />                                              
                                                   
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/img/delete.png" OnClientClick="Confirm()" OnClick="ImgDelete_Click" TabIndex="5" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="3%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>--%>
                                         <asp:GridView runat="server" ID="gvCartItemDetails" Width="100%" ShowFooter="true" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="ID" PageSize="25"
                                         class="table table-bordered table-striped" OnPageIndexChanging="gvCartItemDetails_PageIndexChanging" OnRowEditing="gvCartItemDetails_RowEditing" OnRowUpdating="gvCartItemDetails_RowUpdating">                                             
                                         <Columns>
                                         <asp:TemplateField HeaderText="Item ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCartID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                        <asp:Label ID="lblItemID" runat="server" Text='<%# Eval("ItemID") %>'></asp:Label>
                                                        <asp:Label ID="lblPrice" runat="server" Text='<%# Eval("Price") %>'></asp:Label>
                                                        <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                                                    </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Product" DataField="ItemName" ReadOnly="true" />          
                                        <%--<asp:BoundField HeaderText="Packing" DataField="Packing" ItemStyle-CssClass="qty text-right" HeaderStyle-CssClass="qty text-right" ReadOnly="true"/>--%>
                                         <asp:TemplateField HeaderText="Packing">
                                                 <ItemTemplate>
                                                     <asp:Label ID ="lblPacking" runat="server" Text='<%# Eval("Packing") %>' ></asp:Label>
                                                 </ItemTemplate>
                                             </asp:TemplateField>         
                                       <asp:TemplateField HeaderText="Qty" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="qty" HeaderStyle-Width="200px">
                                           <EditItemTemplate>
                                                      <asp:TextBox CssClass="textbox" Width="60px" ID="txtQty" runat="server" Text='<%# Bind("Qty") %>' OnTextChanged="txtQty_TextChanged" AutoPostBack="true" ValidationGroup="saa"> </asp:TextBox> 
                                                      <asp:RequiredFieldValidator ID="rfvQty" runat="server" ControlToValidate="txtQty" ValidationGroup="saa">*</asp:RequiredFieldValidator>                               
                                           </EditItemTemplate>
                                    <ItemTemplate>
                                          <asp:Label ID="LabelQty" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                                   </ItemTemplate>
                        </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Price Per Unit">
                                                 <ItemTemplate>
                                                     <asp:Label ID ="lblPPUnit" runat="server" Text='<%# Eval("PricePerUnit") %>' ></asp:Label>
                                                 </ItemTemplate>
                                             </asp:TemplateField>                                          
                                          <asp:TemplateField HeaderText="Total">
                                                   <FooterTemplate>
                                                    <b>Total:
                                                       <asp:Label ID="lblAmtTotal" runat="server"></asp:Label></b>                                                        
                                                    </FooterTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmt" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
                                                    </ItemTemplate>
                                                  <HeaderStyle CssClass="qty text-right"/>		
                                         <ItemStyle CssClass="qty text-right"/>	
                                                    <FooterStyle CssClass="qty text-right"/>
                                                </asp:TemplateField>
                       <asp:BoundField HeaderText="Group" DataField="PriceGroup" ItemStyle-CssClass="qty text-right" HeaderStyle-CssClass="qty text-right" ReadOnly="true"/>
                                                <asp:BoundField HeaderText="Remark" DataField="Remarks" ReadOnly="true" />     
                                        
                        <asp:CommandField ShowEditButton="true" ItemStyle-HorizontalAlign="Center" ShowCancelButton="false" ValidationGroup="saa" HeaderText="Edit" HeaderStyle-Width="150px">
                            <HeaderStyle Width="150px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:CommandField>
                        <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/img/delete.png" OnClientClick="Confirm()" OnClick="ImgDelete_Click" TabIndex="5" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="3%" />
                                                </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                                    </div>
                                </div>
                                <div class="row">
                                    &nbsp;
                                </div>
                                <div class="row">
                                    &nbsp;
                                </div>
                                <div class="row">
                                    &nbsp;
                                </div>
                                <div class="row">
                                    &nbsp;
                                </div>
                            </div>
                            <div class="box-footer">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCheckOut" runat="server" Text="Check Out" class="btn btn-primary" OnClick="btnCheckOut_Click" />

                            </div>
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
