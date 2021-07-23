<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="InvoiceSAPDetailsForRetailer.aspx.cs" Inherits="AstralFFMS.InvoiceSAPDetailsForRetailer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <%--  <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
    </style>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>
    <script type="text/javascript">
        //$(function () {
        //    $(".select2").select2();
        //});
    </script>
    <style type="text/css">
        .spinner {
            position: absolute;
            top: 50%;
            left: 50%;
            margin-left: -50px; /* half width of the spinner gif */
            margin-top: -50px; /* half height of the spinner gif */
            text-align: center;
            z-index: 999;
            overflow: hidden;
            width: 180px; /* width of the spinner gif */
            height: 100px; /*hight of the spinner gif +2px to fix IE8 issue */
        }

        .completionList {
            border: solid 1px Gray;
            margin: 0px;
            padding: 3px;
            overflow: auto;
            overflow-y: scroll;
            background-color: #FFFFFF;
            max-height: 180px;
        }

        .listItem {
            color: #191919;
        }

        .itemHighlighted {
            background-color: #ADD6FF;
        }

        ul#ContentPlaceHolder1_AutoCompleteExtender1_completionListElem li {
            padding: 4px 0 !important;
        }

        ul#ContentPlaceHolder1_gvDetails_ac_txtItem_0_completionListElem li {
            padding: 4px 0 !important;
        }
    </style>

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

  




    <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>

        <div class="box-body distributor-ul" id="mainDiv" runat="server">
            <div class="row">
                <!-- general form elements -->
                <div class="box box-primary primarybox-ul">
                    <div class="box-header with-border">
                        <h3 class="box-title">Your Invoice</h3>
                        
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="box-body">

                                
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <div class="row">                                                
                                                <div class="col-sm-12">
                                                    <asp:Label ID="lblDistName" runat="server" Text="" style="color: #169434; font-family: Arial; font-weight: bold;"></asp:Label><br />
                                                    <asp:Label ID="lblDistDetails" runat="server" Text=""></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-sm-3 col-xs-4" style="color: #169434; font-family: Arial; font-weight: bold;">
                                            <label for="exampleInputEmail1">Credit Limit:</label>
                                            <asp:Label ID="lblCreditLimit" runat="server" Text=""></asp:Label>
                                        </div>
                                        <div class="col-sm-3 col-xs-4" style="color: #169434; font-family: Arial; font-weight: bold;">
                                            <label for="exampleInputEmail1">Outstanding:</label>
                                            <asp:Label ID="lblOutstanding" runat="server" Text=""></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-xs-4" style="color: #169434; font-family: Arial; font-weight: bold;">
                                            <label for="exampleInputEmail1">Open Orders:</label>
                                            <asp:Label ID="lblOpenOrders" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                </div>
                               <div class="row">
                                    <div class="col-sm-12" style="color: #169434; font-family: Arial; font-weight: bold;">
                                         Invoice No. <asp:Label ID="lblOrderSt" runat="server" Text=""></asp:Label> | 
                                         Order No. <asp:Label ID="lblOrderNo" runat="server" Text=""></asp:Label> |
                                         Category <asp:Label ID="lblCategory" runat="server" Text="SAP"></asp:Label> | 
                                        Branch Code <asp:Label ID="lblBranchCode" runat="server" Text=""></asp:Label> |  <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="box-body table-responsive">
                                        <asp:GridView ID="gvDetails" ShowFooter="True" runat="server" class="table table-bordered table-striped" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:TemplateField HeaderText="ID" Visible="false">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemID" runat="server" Text='<%#Eval("ItemID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Name">

                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtItem" runat="server" Width="328px" Text='<%#Eval("ItemName") %>' Enabled="false" class="form-control distributor-ul" ></asp:TextBox>
                                                        <ajaxToolkit:AutoCompleteExtender ID="ac_txtItem" runat="server" CompletionListCssClass="completionList"
                                                            OnClientItemSelected="ClientItemIDSelected" EnableCaching="true" ServicePath="CreateNewPurchaseOrder.aspx"
                                                            MinimumPrefixLength="3" ServiceMethod="SearchItem" TargetControlID="txtItem">
                                                        </ajaxToolkit:AutoCompleteExtender>
                                                        <%-- <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_txtItem" runat="server"
                                                        FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars=" "
                                                        TargetControlID="txtItem" />--%>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemStyle Width="47%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Packing">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPacking" runat="server" Text='<%#Eval("StdPack") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Qty.">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQty" runat="server" MaxLength="8" Width="70px" class="form-control" Text='<%#Eval("Qty") %>' Enabled="false"></asp:TextBox>
                                                       
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Unit" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnit" runat="server" Text='<%#Eval("Unit") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />

                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rate" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRate" runat="server" Text='<%#Eval("Rate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />

                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Price Per Unit">
                                                    <FooterTemplate>
                                                        <b>Total</b>
                                                        <footerstyle horizontalalign="Center" font-weight="Bold" />
                                                    </FooterTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRate1" runat="server" Text='<%#Eval("Rate") %>'></asp:Label>/<asp:Label ID="lblUnit1" runat="server" Text='<%#Eval("Unit") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total">
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblAmtTotal" runat="server" Style="color: #169434; font-family: Arial; font-weight: bold;"></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Group">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPG" runat="server" Text='<%#Eval("PriceGroup") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRemarks" TabIndex="4" runat="server" Width="280px" Enabled="false" MaxLength="255" class="form-control" Text='<%#Eval("Remarks") %>'></asp:TextBox>
                                                       
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="25%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/img/delete.png" Enabled="false" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="3%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-xs-6">
                                        <div style="float: left;">
                                            <asp:Button ID="Add" runat="server" Text="Add New Item" class="btn btn-primary" Enabled="false"/>
                                        </div>
                                    </div>
                                  <div class="col-xs-6">
                                        <div  id="tbl" runat="server" style="float: right;">
                                           <%-- <table >
                                              
                                              
                                            </table>--%>
                                            <%--<asp:Button ID="Button1" runat="server" Text="Add New Item" class="btn btn-primary" Enabled="false"/>--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    &nbsp;
                                </div>
                                <div class="row">
                                    &nbsp;
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <label for="exampleInputEmail1">Project Type:</label>
                                        <asp:RadioButtonList ID="ProjectType" runat="server" RepeatDirection="Horizontal"  CssClass="radiogroup" Enabled="false">
                                            <asp:ListItem Selected="True" Value="N">Normal</asp:ListItem>
                                            <asp:ListItem Value="P">Project</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <br />
                                        <div id="ProjectDiv" runat="server" visible="false">
                                            <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control"></asp:DropDownList>
                                            <%--<asp:TextBox ID="txtProject" runat="server" class="form-control"></asp:TextBox>
                                            <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionListCssClass="completionList"
                                                OnClientItemSelected="ClientProjectSelected" EnableCaching="true" ServicePath="CreateNewPurchaseOrder.aspx"
                                                MinimumPrefixLength="3" ServiceMethod="SearchProject" TargetControlID="txtProject">
                                            </ajaxToolkit:AutoCompleteExtender>
                                            --%>
                                        </div>
                                        <label for="exampleInputEmail1">Transporter:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlTransporter" Width="100%" TabIndex="7" CssClass="form-control" runat="server" Enabled="false"></asp:DropDownList>
                                        <div id="TP" visible="false" runat="server">
                                            <asp:TextBox ID="Transporter" runat="server" class="form-control" placeholder="Enter Transporter" MaxLength="100"></asp:TextBox>
                                        </div>
                                        <br />
                                        <label for="exampleInputEmail1">Scheme Code:</label>
                                        <asp:DropDownList ID="ddlScheme" runat="server" Width="100%" CssClass="form-control" Enabled="false"></asp:DropDownList>
                                        <br />
                                        <label for="exampleInputEmail1">Remarks:</label>
                                        <asp:TextBox ID="Remarks" TabIndex="8" Width="100%" runat="server" class="form-control" TextMode="MultiLine" placeholder="Enter Remarks" Height="180px" ReadOnly="true"></asp:TextBox>

                                    </div>
                                    <div class="col-md-2">
                                        &nbsp;
                                    </div>
                                    <div class="col-md-6">
                                        <label for="exampleInputEmail1">Dispatch To:</label>&nbsp;
                                        <asp:CheckBox ID="chkDispatchTo" runat="server" Enabled="false"  />
                                        <asp:Label ID="lblCustomer" runat="server" Text="Same as Retailer"></asp:Label>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <label>Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:TextBox ID="DispName" runat="server" class="form-control" placeholder="Enter Dispatch Name" MaxLength="100" TabIndex="10" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <label>Address 1:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:TextBox ID="DispAdd1" runat="server" class="form-control" placeholder="Enter Address Line 1" MaxLength="100" TabIndex="11" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <label>Address 2:</label>&nbsp;
                                                <asp:TextBox ID="DispAdd2" runat="server" class="form-control" placeholder="Enter Address Line 2" MaxLength="100" TabIndex="12" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <label>City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:DropDownList ID="ddlCity" Width="100%" CssClass="form-control" runat="server" Enabled="false"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-6">
                                                <label>Pin:</label>&nbsp;
                                                <asp:TextBox ID="Pin" runat="server" class="form-control" placeholder="Enter Pin" MaxLength="6" TabIndex="15" ReadOnly="true"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_Pin" runat="server" FilterType="Numbers"
                                                    TargetControlID="Pin" />
                                            </div>

                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <label>State:</label>&nbsp;
                                                <%--<asp:DropDownList ID="ddlState" TabIndex="13" CssClass="form-control select2" runat="server" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                                                <%-- <asp:Label ID="lblState" runat="server" Text=""></asp:Label>--%>
                                                <asp:TextBox ID="txtState" runat="server" Style="background: white;" class="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>

                                            <div class="col-md-6">
                                                <label>Country:</label>&nbsp;
                                                <%--<asp:DropDownList ID="ddlCountry" CssClass="form-control select2" runat="server" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                                                <%--  <asp:Label ID="lblCountry" runat="server" Text=""></asp:Label>--%>
                                                <asp:TextBox ID="txtCountry" runat="server" Style="background: white;" class="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>

                                        </div>

                                        <div class="row">
                                            <div class="col-md-6">
                                                <label>Phone No:</label>&nbsp;
                                                <asp:TextBox ID="Phone" runat="server" class="form-control" placeholder="Enter Phone No" MaxLength="12" TabIndex="16" ReadOnly="true"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_Phone" runat="server" FilterType="Numbers"
                                                    TargetControlID="Phone" />
                                            </div>
                                            <div class="col-md-6">
                                                <label>Mobile No:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:TextBox ID="Mobile" runat="server" class="form-control" placeholder="Enter Mobile No" MaxLength="10" TabIndex="17" ReadOnly="true"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_Mobile" runat="server" FilterType="Numbers"
                                                    TargetControlID="Mobile" />
                                                <%--     <asp:RegularExpressionValidator ID="RegExp_Mobile" runat="server" ForeColor="Red"
                                                    ControlToValidate="Mobile" ErrorMessage="Please enter 10 digits Mobile No."
                                                    ValidationExpression="[0-9]{10}"></asp:RegularExpressionValidator>--%>
                                            </div>

                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <label>Email:</label>&nbsp;
                                                <asp:TextBox ID="Email" runat="server" class="form-control" placeholder="Enter Email" MaxLength="100" TabIndex="18" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                </div>
            </div>
        </div>
    </section>

</asp:Content>

