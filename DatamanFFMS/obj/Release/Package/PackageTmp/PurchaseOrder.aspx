<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="PurchaseOrder.aspx.cs" Inherits="FFMS.PurchaseOrder" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
    </style>
    
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
    </style>

     <style>
        .alignright{
        text-align:right;
        }

        .qty.text-right {text-align:right!important;
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

    <script type="text/javascript">
        function DoNav(POID) {
            if (POID != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                //GetRadioButtonSelectedValue();
                __doPostBack('', POID)
            }
        }
    </script>
    <script type="text/javascript">
        function DoNav1() {
            if (POID != "") {
                alert(POID);
                $('#"<%=HiddenField2.ClientID %>"').value = POID;
            }
        }
    </script>


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
        function Confirm1() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to cancel?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>

    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfCustomerId.ClientID %>").value = e.get_value();
        }
    </script>

    <script type="text/javascript">
        function ClientItemSelected1(sender, e) {
            $get("<%=hfCustomerId1.ClientID %>").value = e.get_value();
         }
    </script>

    <script type="text/javascript">
        function ClientProjectSelected(sender, e) {
            $get("<%=hfProjectID.ClientID %>").value = e.get_value();
        }
    </script>

    <script type="text/javascript">
        function ClientItemIDSelected(sender, e) {
            $get("<%=hfItemId.ClientID %>").value = e.get_value();
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

        <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- general form elements -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                      <%--  <h3 class="box-title">Modify Your Order</h3>--%>
                        <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                        <div style="float: right">
                            <%--   <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary" Visible="false"
                                        OnClick="btnFind_Click" />--%>
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="box-body">

                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            &nbsp;
                                           
                                        </div>
                                        <div class="col-sm-6">
                                            <div style="float: right; color: #169434; font-family: Arial; font-weight: bold;">
                                                <%--  <label for="exampleInputEmail1">Date:</label>--%>
                                                Order No.(<asp:Label ID="lblOrderSt" runat="server" Text="New"></asp:Label>) |
                                                <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <div class="col-md-12">
                                                <label for="exampleInputEmail1">Distributor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:TextBox ID="txtDist" runat="server" class="form-control" OnTextChanged="txtDist_TextChanged" AutoPostBack="true" TabIndex="1"></asp:TextBox>

                                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="completionList"
                                                    OnClientItemSelected="ClientItemSelected" EnableCaching="true" ServicePath="PurchaseOrder.aspx"
                                                    MinimumPrefixLength="3" ServiceMethod="SearchDistributor" TargetControlID="txtDist">
                                                </ajaxToolkit:AutoCompleteExtender>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_txtDist" runat="server"
                                                    FilterType="LowercaseLetters, UppercaseLetters,Custom,Numbers" ValidChars=" "
                                                    TargetControlID="txtDist" />
                                                <asp:HiddenField ID="hfCustomerId" runat="server" />
                                                <asp:HiddenField ID="hfCustomerId1" runat="server" />
                                                <asp:HiddenField ID="hfname" runat="server" />
                                                <asp:HiddenField ID="hfItemId" runat="server" />
                                                <asp:HiddenField ID="hfProjectID" runat="server" />
                                            </div>
                                            <div class="col-sm-12">
                                                <asp:Label ID="lblDistDetails" runat="server" Text=""></asp:Label>
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
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtItem" runat="server" Width="354px" Text='<%#Eval("ItemName") %>' class="form-control" AutoPostBack="true" OnTextChanged="txtItem_TextChanged" TabIndex="2"></asp:TextBox>
                                                        <ajaxToolkit:AutoCompleteExtender ID="ac_txtItem" runat="server" CompletionListCssClass="completionList"
                                                            OnClientItemSelected="ClientItemIDSelected" EnableCaching="true" ServicePath="PurchaseOrder.aspx"
                                                            MinimumPrefixLength="3" ServiceMethod="SearchItem" TargetControlID="txtItem">
                                                        </ajaxToolkit:AutoCompleteExtender>
                                                        <%-- <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_txtItem" runat="server"
                                                        FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars=" "
                                                        TargetControlID="txtItem" />--%>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemStyle Width="37%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Packing">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPacking" runat="server" Text='<%#Eval("StdPack") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="qty text-right" />
                                                    <ItemStyle Width="5%" CssClass="qty text-right"/>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQty" runat="server" MaxLength="8" Width="70px" class="form-control numeric text-right" Text='<%#Eval("Qty")%>' AutoPostBack="true" OnTextChanged="txtQty_TextChanged" TabIndex="3"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_txtQty" runat="server" FilterType="Numbers,Custom" 
                                                            TargetControlID="txtQty" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="qty text-right" />
                                                    <ItemStyle Width="5%" CssClass="qty text-right" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Unit" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnit" runat="server" Text='<%#Eval("Unit") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rate" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRate" runat="server" Text='<%#Eval("Rate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Price Per Unit">
                                                    <FooterTemplate>
                                                        <b>Total</b>
                                                     
                                                    </FooterTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRate1" runat="server" Text='<%#Eval("Rate") %>'></asp:Label>/<asp:Label ID="lblUnit1" runat="server" Text='<%#Eval("Unit") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="10%" />
                                                       <footerstyle CssClass="qty text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total">
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblAmtTotal" runat="server" Style="color: #169434; font-family: Arial; font-weight: bold;"></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="qty text-right" />
                                                    <ItemStyle Width="10%" CssClass="qty text-right" />
                                                    <FooterStyle CssClass="qty text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Group">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPG" runat="server" Text='<%#Eval("PriceGroup") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="qty text-right" />
                                                    <ItemStyle Width="5%" CssClass="qty text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remark">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRemarks" TabIndex="4" runat="server" Width="230px"  MaxLength="255" class="form-control" Text='<%#Eval("Remarks") %>'></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_txtRemarks" runat="server"
                                                            FilterType="LowercaseLetters, UppercaseLetters,Custom,Numbers" ValidChars=" "
                                                            TargetControlID="txtRemarks" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="25%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/img/delete.png" OnClick="ImgDelete_Click" TabIndex="5" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="3%" />
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div style="float: left;">
                                            <asp:Button ID="Add" runat="server" Text="Add New Product" class="btn btn-primary" OnClick="Add_Click" TabIndex="6" />
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
                                        <asp:RadioButtonList ID="ProjectType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="ProjectType_SelectedIndexChanged" CssClass="radiogroup">
                                            <asp:ListItem Selected="True" Value="N">Normal</asp:ListItem>
                                            <asp:ListItem Value="P">Project</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <br />
                                        <div id="ProjectDiv" runat="server" visible="false">
                                            <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control"></asp:DropDownList>
                                            <%--<asp:TextBox ID="txtProject" runat="server" class="form-control"></asp:TextBox>
                                            <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionListCssClass="completionList"
                                                OnClientItemSelected="ClientProjectSelected" EnableCaching="true" ServicePath="PurchaseOrder.aspx"
                                                MinimumPrefixLength="3" ServiceMethod="SearchProject" TargetControlID="txtProject">
                                            </ajaxToolkit:AutoCompleteExtender>--%>
                                        </div>
                                        <label for="exampleInputEmail1">Transporter:</label><%--&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>--%><asp:DropDownList ID="ddlTransporter" Width="100%" TabIndex="7" CssClass="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTransporter_SelectedIndexChanged"></asp:DropDownList>
                                        <div id="TP" visible="false" runat="server">
                                            <asp:TextBox ID="Transporter" runat="server" class="form-control" placeholder="Enter Transporter" MaxLength="100"></asp:TextBox>
                                        </div>
                                        <br />
                                        <label for="exampleInputEmail1">Scheme Code:</label>
                                        <asp:DropDownList ID="ddlScheme" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                        <br />
                                        <label for="exampleInputEmail1">Remark:</label>
                                        <asp:TextBox ID="Remarks" TabIndex="8" runat="server" class="form-control" TextMode="MultiLine" placeholder="Enter Remark" Height="180px"></asp:TextBox>

                                    </div>
                                    <div class="col-md-2">
                                        &nbsp;
                                    </div>
                                    <div class="col-md-6">
                                        <label for="exampleInputEmail1">Dispatch To:</label>&nbsp;
                                        <asp:CheckBox ID="chkDispatchTo" runat="server" AutoPostBack="True" OnCheckedChanged="chkDispatchTo_CheckedChanged" TabIndex="9" />
                                        <asp:Label ID="lblCustomer" runat="server" Text="Same as Distributor"></asp:Label>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <label>Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:TextBox ID="DispName" runat="server" class="form-control" placeholder="Enter Dispatch Name" MaxLength="100" TabIndex="10"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <label>Address 1:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:TextBox ID="DispAdd1" runat="server" class="form-control" placeholder="Enter Address Line 1" MaxLength="100" TabIndex="11"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <label>Address 2:</label>&nbsp;
                                                <asp:TextBox ID="DispAdd2" runat="server" class="form-control" placeholder="Enter Address Line 2" MaxLength="100" TabIndex="12"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <label>City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:DropDownList ID="ddlCity" Width="100%" CssClass="form-control" runat="server" TabIndex="14" AutoPostBack="True" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-6">
                                                <label>Pincode:</label>&nbsp;
                                                <asp:TextBox ID="Pin" runat="server" class="form-control" placeholder="Enter Pin" MaxLength="6" TabIndex="15"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_Pin" runat="server" FilterType="Numbers"
                                                    TargetControlID="Pin" />
                                            </div>

                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <label>State:</label>&nbsp;
                                                <%--<asp:DropDownList ID="ddlState" TabIndex="13" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                                                <%-- <asp:Label ID="lblState" runat="server" Text=""></asp:Label>--%>
                                                <asp:TextBox ID="txtState" runat="server" Style="background: white;" class="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>

                                            <div class="col-md-6">
                                                <label>Country:</label>&nbsp;
                                                <%--<asp:DropDownList ID="ddlCountry" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                                                <%--  <asp:Label ID="lblCountry" runat="server" Text=""></asp:Label>--%>
                                                <asp:TextBox ID="txtCountry" runat="server" Style="background: white;" class="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>

                                        </div>

                                        <div class="row">
                                            <div class="col-md-6">
                                                <label>Phone No:</label>&nbsp;
                                                <asp:TextBox ID="Phone" runat="server" class="form-control" placeholder="Enter Phone No" MaxLength="12" TabIndex="16"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_Phone" runat="server" FilterType="Numbers"
                                                    TargetControlID="Phone" />
                                            </div>
                                            <div class="col-md-6">
                                                <label>Mobile No:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:TextBox ID="Mobile" runat="server" class="form-control" placeholder="Enter Mobile No" MaxLength="10" TabIndex="17"></asp:TextBox>
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
                                                <asp:TextBox ID="Email" runat="server" class="form-control" placeholder="Enter Email" MaxLength="100" TabIndex="18"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="box-footer">
                        <div class="col-md-9">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" TabIndex="19" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" TabIndex="20" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" OnClick="btnDelete_Click" TabIndex="21" />
                        </div>
                        <div class="col-md-3">
                            <div style="float: right;">
                                <asp:Button Style="margin-right: 5px;margin-top:5px;" Visible="false" type="button" ID="CancelOrder" runat="server" Text="Cancel Order" class="btn btn-primary" OnClick="CancelOrder_Click" OnClientClick="Confirm1()" />
                            </div>
                        </div>
                      
                    </div>
                      <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                </div>
            </div>
        </div>
        
				 

        <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Orders</h3>
                            <div style="float: right">
                                <%--<asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />--%>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="col-md-12">
                                <div class="col-md-12 paddingleft0">
                                    <div id="DIV1" class="form-group col-md-2 col-sm-7 col-xs-10">
                                        <label for="exampleInputEmail1">From Date:</label>
                                        <asp:TextBox ID="txtmDate" runat="server" CssClass="form-control" BackColor="White" ></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtmDate" runat="server" />
                                    </div>
                                    <div class="form-group col-md-2 col-sm-7 col-xs-10">
                                        <label for="exampleInputEmail1">To Date:</label>
                                        <asp:TextBox ID="txttodate" runat="server" CssClass="form-control" BackColor="White" ></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                    </div>
                                    <div class="form-group col-md-3 col-sm-7 col-xs-10">
                                        <label for="exampleInputEmail1">Distributor Name:</label>
                                        <%--<asp:DropDownList ID="ddlDist" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>--%>
                                        <asp:TextBox ID="txtDist1" runat="server" class="form-control"></asp:TextBox>
                                        <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionListCssClass="completionList"
                                            OnClientItemSelected="ClientItemSelected1" EnableCaching="true" ServicePath="PurchaseOrder.aspx"
                                            MinimumPrefixLength="3" ServiceMethod="SearchDistributor" TargetControlID="txtDist1">
                                        </ajaxToolkit:AutoCompleteExtender>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                            FilterType="LowercaseLetters, UppercaseLetters,Custom,Numbers" ValidChars=" "
                                            TargetControlID="txtDist1" />
                                    </div>
                                    <div class="form-group col-md-3 col-sm-7 col-xs-10">
                                        <label for="exampleInputEmail1">Status:</label>
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="100%" CssClass="form-control">
                                            <%-- <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>--%>
                                            <asp:ListItem Text="Open" Value="P" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Processed" Value="M"></asp:ListItem>
                                            <asp:ListItem Text="CustomerCanceled" Value="R"></asp:ListItem>
                                            <asp:ListItem Text="CompanyCanceled" Value="C"></asp:ListItem>
                                            <asp:ListItem Text="OnHold" Value="H"></asp:ListItem>
                                            <asp:ListItem Text="All" Value="A"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-md-2 col-sm-7 col-xs-10">
                                        <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                        <asp:Button type="button" ID="btnGo" runat="server" Text="Go" Style="padding: 3px 7px;" class="btn btn-primary" OnClick="btnGo_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /.box-header -->
                     
                        
                        <div class="box-body table-responsive">
                            <asp:HiddenField ID="HiddenField2" runat="server" />
                            <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Date</th>
                                                <th>Document No.</th>
                                                <th>Distributor Name</th>
                                                <th>Product Group & Dispatch To</th>
                                                <th>Order Status</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("POrdId") %>' />
                                       
                                        <td onclick="DoNav('<%#Eval("POrdId") %>');"><%#Convert.ToDateTime(Eval("VDate")).ToString("dd/MMM/yyyy")%></td>
                                        <td onclick="DoNav('<%#Eval("POrdId") %>');"><%#Eval("PODocId") %></td>
                                        <td onclick="DoNav('<%#Eval("POrdId") %>');"><%#Eval("PartyName") %></td>
                                        <td onclick="DoNav('<%#Eval("POrdId") %>');"><%#Eval("IGandDispatchTo") %></td>
                                        <td onclick="DoNav('<%#Eval("POrdId") %>');"><%#Eval("OrderStatus") %></td>
                                         <td>
                                            <asp:ImageButton ID="ImgPrint" OnClientClick="target ='_blank';" runat="server" ImageUrl="~/img/icon_print.gif" OnClick="ImgPrint_Click" TabIndex="5" />
                                        </td>
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

                     
        <%--                <div class="box-body table-responsive" runat="server" id="report">
                    <asp:PlaceHolder ID="PlcHldrReport" runat="server"></asp:PlaceHolder>
                </div>--%>
    </section>



    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script type="text/javascript">
       
      $(function () {
          $("#example1").DataTable({
              "order": [[0, "desc"]]
          });
      });
    

        //$(function load2() {
        //    $("#example1").DataTable({
        //        "order": [[0, "desc"]]
        //    });
        //});
        //$(window).load(function () {

        //    //Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(load1);
        //    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(load2);

        //});


    </script>


</asp:Content>

