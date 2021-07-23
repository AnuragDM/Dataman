<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ConfirmPurchaseOrder.aspx.cs" Inherits="FFMS.ConfirmPurchaseOrder" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
      @media print {
           body {font-size:11px}
             body  {margin-left:-65px!important; width:112%!important}
            body .forprint th, body .forprint td {
                max-width: 50px!important; 
                
                 white-space: normal !important; 
                 word-wrap:break-all!important;
                 padding:5px!important;
                 
            }

            body .forprint th:nth-child(1), body .forprint td:nth-child(1) {
            width:70px!important;
            
            }

              body .forprint th:nth-child(2), body .forprint td:nth-child(2),  body .forprint th:nth-child(9), body .forprint td:nth-child(9),body .forprint th:nth-child(10), body .forprint td:nth-child(10),body .forprint td:nth-child(3) {width:25px!important}
                body .forprint th:nth-child(8) { width:100px!important;
                }

            body .forprint td input[type='text'],     body .forprint td select{width:100%!important;padding:0px!important; border:0px!important; font-size:11px; height:auto!important}
            .qty {width:20px!important}
      

       }

        </style>

    <style>
        .alignright{
        text-align:right;
        }

        .qty.text-right {text-align:right!important;
        }

    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#dateTimeInput").jqxDateTimeInput({ width: '200px', height: '25px', theme: 'arctic', formatString: 'dd-MMM-yyyy' });
            var date = new Date();
            date.setDate(date.getDate())
            $("#dateTimeInput").jqxDateTimeInput('setMaxDate', date);
        });
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
    
     <script type="text/javascript">
         function DoNav(POID) {
             if (POID != "") {
                 __doPostBack('', POID)
             }
         }
    </script>
   
    <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <!-- left column -->

        <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- general form elements -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">Confirm Purchase Order</h3>
                        <div style="float: right">
                            <asp:Button ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="box-body">
                                <div class="row">
                                    <div class='col-xs-3'>
                                        <div style="float: left; color: #169434; font-family: Arial; font-weight: bold;">
                                            Sales Order Details
                                        </div>
                                    </div>
                                    <div class='col-xs-3'>
                                        <div style="float: right; color: #169434; font-family: Arial; font-weight: bold;">
                                            Order No.:
                                        <asp:Label ID="lblOrderNo" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                    <div class='col-xs-3'>
                                        <div style="float: right; color: #169434; font-family: Arial; font-weight: bold;">
                                            Order Date:
                                        <asp:Label ID="lblOrderDate" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                    <div class='col-xs-3'>
                                        <div style="float: right; color: #169434; font-family: Arial; font-weight: bold;">
                                            Created Date:
                                        <asp:Label ID="lblCreatedDate" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class='col-xs-4'>
                                        <b>Bill To:
                                    <asp:Label ID="lblBillTo" runat="server" Text="" Style="color: #169434; font-family: Arial; font-weight: bold;"></asp:Label></b>
                                        <br />
                                        <asp:Label ID="lblBillToAdd" runat="server" Text=""></asp:Label>
                                        <br />
                                        <b>Remark:</b><asp:Label ID="lblRemarks" runat="server" ></asp:Label>
                                    </div>
                                     <div class='col-xs-4'>
                                         &nbsp;
                                         </div>
                                    <div class='col-xs-4'>
                                        <div class="text-right">
                                            <b>Dispatch To:
                                        <asp:Label ID="lblDispatchTo" runat="server" Text="" Style="color: #169434; font-family: Arial; font-weight: bold;"></asp:Label></b>
                                            <br />
                                             <asp:Label ID="lblDispatchToAdd" runat="server" Text=""></asp:Label>

                                        </div>
                                        
                                    </div>
                                </div>
                                <div class="row">
                                    &nbsp; &nbsp;
                                </div>
                                <div class="row">
                                    <div class='col-xs-4'>
                                        <label>Transporter:</label>&nbsp;&nbsp;
                                         <asp:Label ID="lblTransporter" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class='col-xs-2'>
                                        &nbsp;&nbsp;
                                    </div>
                                    <div class='col-xs-1'>
                                        &nbsp;&nbsp;
                                    </div>
                                    <div class='col-sm-5 col-xs-5'>
                                        <div style="float:right;">
                                            <div class="col-md-6 col-sm-6">
                                                <label>Discount:</label>
                                                <asp:TextBox ID="txtDiscount" runat="server" class="numeric text-right form-control" Width="100px" PlaceHolder="0.00" MaxLength="6"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_txtDiscount" runat="server" FilterType="Numbers,Custom" ValidChars="."
                                                TargetControlID="txtDiscount" />  
                                            </div>
                                            <div class="col-md-6 col-sm-6">
                                                <label for="exampleInputEmail1" style="display:block; visibility:hidden">zkjfhksj</label>
                                                <asp:Button ID="btnApply" runat="server" style="margin:0;" Text="Apply" class="btn btn-primary" OnClick="btnApply_Click" />
                                            </div>
                                            
                                                                      
                                            
                                        
                                        </div>
                                    </div>
                                    
                                    
                                </div>


                                <div class="row">
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                </div>

                                <div class="row">
                                    <div class='col-xs-12'>
                                        <div class="table table-responsive forprint">
                                            <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped" OnPageIndexChanging="gvDetails_PageIndexChanging" ShowFooter="true" AllowPaging="false" PageSize="10">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Product">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                    <asp:BoundField HeaderText="Pkg." DataField="CartonQty" ItemStyle-CssClass="qty text-right" HeaderStyle-CssClass="qty text-right">                                                       
                                                    </asp:BoundField>
                                                    <%-- <asp:BoundField HeaderText="Ord Qty" DataField="OrdQty" ItemStyle-HorizontalAlign="Right" />--%>
                                                    <asp:TemplateField HeaderText="Ord Qty">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOrdQty" runat="server" Text='<%# Eval("OrdQty") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblOriQty" runat="server" Text='<%# Eval("OrdQty") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" CssClass="qty text-right"/>
                                                         <HeaderStyle CssClass="qty text-right"/>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Conf Qty">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtConfirmQty" width="80px" runat="server" class="form-control numeric text-right" Text='<%# Eval("OrdQty") %>' PlaceHolder="Enter Qty" MaxLength="8" AutoPostBack="true" OnTextChanged="txtConfirmQty_TextChanged"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_txtConfirmQty" runat="server" FilterType="Numbers,Custom" ValidChars="."
                                                                TargetControlID="txtConfirmQty" />
                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Style="display: none;"></asp:Label>
                                                            <asp:Label ID="lblItemID" runat="server" Text='<%# Eval("ItemID") %>' Style="display: none;"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle CssClass="qty text-right"/>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Rate" DataField="Rate" ItemStyle-CssClass="qty text-right" HeaderStyle-CssClass="qty text-right">                                                    
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Disc 2">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDiscount" width="120px" runat="server" OnTextChanged="txtDiscount_TextChanged" AutoPostBack="true" class="numeric text-right form-control" PlaceHolder="0.00" MaxLength="6"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_txtDiscount" runat="server" FilterType="Numbers,Custom" ValidChars="."
                                                                TargetControlID="txtDiscount" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <HeaderStyle CssClass="qty text-right"/>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Location">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlLocation" width="150px" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true"> 
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Remark" DataField="Remarks" NullDisplayText=" "/>
                                                    <asp:BoundField HeaderText="Group Code" DataField="GroupCode" />
                                                    <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                    <asp:TemplateField HeaderText="Amount">
                                                        <FooterTemplate>
                                                            Total: <asp:Label ID="lblAmtTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAmt" runat="server" Text='<%# Eval("Amount") %>' Visible="false"></asp:Label>
                                                              <asp:Label ID="lblOriAmount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>                                                          
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="qty text-right" Font-Bold="true"/>
                                                        <HeaderStyle CssClass="qty text-right"/>
                                                        <ItemStyle HorizontalAlign="Right" CssClass="qty text-right"/>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ori Amount" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOriginalAmount" runat="server" Text='<%# Bind("Amount") %>' CssClass="alignright"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnOK" runat="server" Text="OK" class="btn btn-primary" OnClick="btnOK_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                                <div class="row">
                                    <div class='col-xs-6'>
                                        <label>Outstanding:</label>&nbsp;&nbsp;
                                         <asp:Label ID="lblOutstanding" runat="server" Text=""></asp:Label>&nbsp;|&nbsp;
                                  <label>Credit Limit:</label>&nbsp;&nbsp;
                                         <asp:Label ID="lblCreditLimit" runat="server" Text=""></asp:Label>&nbsp;|&nbsp;
                                  <label>Open Orders:</label>&nbsp;&nbsp;
                                         <asp:Label ID="lblOpenOrders" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class='col-xs-6'>
                                        <div style="float: right;">
                                            <%--<label>Amount Total:</label>&nbsp;&nbsp;
                                         <asp:Label ID="lblAmountTotal" runat="server" Text=""></asp:Label>--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                                <div class="row">
                                    &nbsp; &nbsp;
                                </div>
                                <b>
                                    <asp:Label ID="Label1" runat="server" Text="Order According to Location" Style="color: #169434; font-family: Arial; font-weight: bold;"></asp:Label></b>
                                <div class="table table-responsive forprint">
                                <asp:GridView ID="gvLocDataDetails" ShowHeader="false" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped" OnRowDataBound="gvLocDataDetails_RowDataBound">

                                    <Columns>
                                        <asp:TemplateField HeaderText="ItemID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocationID" runat="server" Text='<%# Eval("LocationID") %>'></asp:Label>
                                                    <asp:Label ID="lblGroupCode" runat="server" Text='<%# Eval("GroupCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lbllocName" runat="server" Style="color: blue; font-family: Arial;"></asp:Label>
                                                <asp:GridView ID="gvLocwiseData" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped" ShowFooter="true">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Product" DataField="ItemName" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty"  ItemStyle-Width="10%" ItemStyle-CssClass="qty text-right" HeaderStyle-CssClass="qty text-right"/>
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="10%" ItemStyle-CssClass="qty text-right" HeaderStyle-CssClass="qty text-right"/>
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="10%" ItemStyle-CssClass="qty text-right" HeaderStyle-CssClass="qty text-right"/>
                                                        <asp:BoundField HeaderText="Remark" NullDisplayText=" " DataField="Remarks" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%"/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <%--<asp:BoundField HeaderText="Amount" DataField="Amount" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="10%" ItemStyle-CssClass="qty text-right"/>--%>

                                                    <asp:TemplateField HeaderText="Amount">
                                                        <FooterTemplate>
                                                            Sub Total: <asp:Label ID="lblAmtTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>                                                            
                                                              <asp:Label ID="lblOriAmount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>                                                          
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="qty text-right" Font-Bold="true"/>
                                                        <ItemStyle HorizontalAlign="Right" CssClass="qty text-right"/>
                                                    </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>

                                </div>
                                <div class="row" id="DynamicRow" runat="server" visible="false">
                                    <div class='col-xs-12'>
                                        <b>
                                            <asp:Label ID="lblOrderAccLoc" runat="server" Text="Order According to Location" Style="color: #169434; font-family: Arial; font-weight: bold;"></asp:Label></b>
                                        <div id="divLoc1" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation1" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc1" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <div id="divLoc2" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation2" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive">
                                                <asp:GridView ID="gvDataLoc2" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <div id="divLoc3" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation3" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc3" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <div id="divLoc4" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation4" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc4" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <div id="divLoc5" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation5" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc5" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <div id="divLoc6" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation6" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc6" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <div id="divLoc7" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation7" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc7" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <div id="divLoc8" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation8" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc8" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <div id="divLoc9" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation9" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc9" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <div id="divLoc10" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation10" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc10" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <div id="divLoc11" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation11" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc11" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>


                                        <div id="divLoc12" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation12" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc12" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <div id="divLoc13" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation13" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc13" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>


                                        <div id="divLoc14" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation14" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc14" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <div id="divLoc15" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation15" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc15" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <div id="divLoc16" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation16" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc16" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>


                                        <div id="divLoc17" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation17" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc17" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks"  NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <div id="divLoc18" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation18" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc18" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" " />
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <div id="divLoc19" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation19" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc19" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <div id="divLoc20" runat="server" visible="false">
                                            <asp:Label ID="lblOrderLocation20" runat="server" Text="" Style="color: blue; font-family: Arial;"></asp:Label>
                                            <div class="table table-responsive forprint">
                                                <asp:GridView ID="gvDataLoc20" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Item Name" DataField="ItemName" />
                                                        <asp:BoundField HeaderText="UOM" DataField="Unit" />
                                                        <asp:BoundField HeaderText="Qunatity" DataField="ConfQty" />
                                                        <asp:BoundField HeaderText="Sale Rate" DataField="Rate" />
                                                        <asp:BoundField HeaderText="Discount" DataField="Discount" />
                                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" NullDisplayText=" "/>
                                                        <asp:BoundField HeaderText="Excise" DataField="Excise" Visible="false" />
                                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class='col-xs-6'>
                                        &nbsp;
                                    </div>
                                    <div class='col-xs-6'>
                                        <div style="float: right;">
                                            <label>Grand Total:</label>&nbsp;&nbsp;
                                         <asp:Label ID="lblAmountTotal" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class='col-xs-12'>
                                        &nbsp;
                                    </div>
                                </div>
                               <%--  <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server" Visible="false">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Date</th>
                                                <th>Document No.</th>
                                                <th>Distributor Name</th>
                                                <th>Product Group & Dispatch To</th>
                                                <th>Order Status</th>                                                  
                                                  <th ></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("POrdId") %>' />
                                         <asp:HiddenField ID="HiddenField2" runat="server" Value='<%#Eval("PODocId") %>' />
                                        <td onclick="DoNav('<%#Eval("POrdId") %>');"><%#Convert.ToDateTime(Eval("VDate")).ToString("dd/MMM/yyyy")%></td>
                                        <td onclick="DoNav('<%#Eval("POrdId") %>');"><%#Eval("PODocId") %></td>
                                        <td onclick="DoNav('<%#Eval("POrdId") %>');"><%#Eval("PartyName") %></td>
                                        <td onclick="DoNav('<%#Eval("POrdId") %>');"><%#Eval("IGandDispatchTo") %></td>
                                        <td onclick="DoNav('<%#Eval("POrdId") %>');"><%#Eval("OrderStatus") %></td>                                       
                                         <td>
                                              <asp:ImageButton ID="ImgDownload" runat="server" ImageUrl="~/img/downloads.png" Text="Dowload1" visible="false" />&nbsp;&nbsp;                                            
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>--%>
                                <%--                                <div class="row">
                                    <div class='col-xs-6'>

                                        <label for="exampleInputEmail1">Status:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlStatus" CssClass="form-control select2" runat="server">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                            <asp:ListItem Value="M" Text="Confirm"></asp:ListItem>
                                            <asp:ListItem Value="C" Text="Cancel"></asp:ListItem>
                                            <asp:ListItem Value="H" Text="OnHold"></asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                    <div class='col-xs-6'>
                                        &nbsp;&nbsp;
                                    </div>
                                </div>--%>
                                <div class="row">
                                    &nbsp;
                                </div>


                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="box-footer">
                        <div class="row">
                            <div class='col-md-3'>

                                <div class="float:left;">
                                    <asp:Button ID="btnConfirm" runat="server" Text="Confirm" class="btn btn-primary" OnClick="btnConfirm_Click" />
                                     <asp:Button ID="btnexport" runat="server" Text="Export to Excel" class="btn btn-primary" OnClick="btnexport_Click" Visible="false" />

                                </div>
                            </div>
                            <div class='col-md-6'>
                                &nbsp;
                            </div>
                            <div class='col-md-3'>
                                <div class="float:right;">
                                    <asp:Button ID="Cancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="Cancel_Click" />&nbsp;Or&nbsp;
                                      <asp:Button ID="btnOnHold" runat="server" Text="On Hold" class="btn btn-primary" OnClick="btnOnHold_Click" />
                                </div>
                            </div>
                        </div>



                    </div>
                </div>
            </div>
        </div>

    </section>

</asp:Content>
