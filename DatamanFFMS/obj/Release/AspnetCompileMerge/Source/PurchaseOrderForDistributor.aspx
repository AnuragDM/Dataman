<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="PurchaseOrderForDistributor.aspx.cs" EnableEventValidation="false" Inherits="AstralFFMS.PurchaseOrderForDistributor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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
        function ClientItemIDSelected(sender, e) {
            $get("<%=hfItemId.ClientID %>").value = e.get_value();
        }
    </script>



    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <section class="content">
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
                                <h3 class="box-title">Create New Order</h3>
                                <div style="float: right">
                                 <%--   <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Cart" class="btn btn-primary" 
                                        OnClick="btnFind_Click" />--%>
                                    <a href="CartItemDetails.aspx" class="btn btn-primary" >Cart <span class="badge" id="CartItemCount" runat="server"></span></a>
                                </div>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-sm-4">
                                          <%--  <label for="exampleInputEmail1">Distributor:</label>--%>
                                         <b>
                                                <asp:Label ID="lblDistName" runat="server" Text="" ></asp:Label></b>
                                            <br />
                                            <asp:Label ID="lblDistDetails" runat="server" Text=""></asp:Label>

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
                                    <div class="col-sm-12">
                                        &nbsp;
                                    </div>
                                </div>
                                <div class="row">            

                                    <div class="table-responsive">          
  <table class="table">
    <thead>
      <tr>
        <th><label>Product:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label></th>
        <th style="text-align:right;"><label>Packing:</label></th>
        <th style="text-align:right;"><label>Qty.:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label></th>
        <th><label>Price Per Unit:</label></th>
        <th style="text-align:right;"><label>Total:</label></th>
        <th style="text-align:right;"><label>Group:</label></th>
          <th><label>Remark:</label></th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>
            <asp:HiddenField ID="hfItemId" runat="server" />
            <asp:TextBox ID="txtItem" runat="server" class="form-control" AutoPostBack="True" TabIndex="1" OnTextChanged="txtItem_TextChanged" Width="300px"></asp:TextBox>
            <ajaxToolkit:AutoCompleteExtender ID="ac_txtItem" runat="server" CompletionListCssClass="completionList" OnClientItemSelected="ClientItemIDSelected" EnableCaching="true" ServicePath="PurchaseOrderForDistributor.aspx" MinimumPrefixLength="3" ServiceMethod="SearchItem" TargetControlID="txtItem"> </ajaxToolkit:AutoCompleteExtender>
                                                   
        </td>
        <td>
             <asp:TextBox ID="lblPacking" class="form-control numeric text-right" runat="server" Text="" ReadOnly="true"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtQty" runat="server" MaxLength="8" Width="70px" class="form-control numeric text-right" Text="" AutoPostBack="true" TabIndex="3" OnTextChanged="txtQty_TextChanged"></asp:TextBox>
   <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_txtQty" runat="server" FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtQty" />
        </td>
        <td>
            <asp:TextBox ID="lblRate1" class="form-control" runat="server" Text="" ReadOnly="true" Visible="False"></asp:TextBox><asp:TextBox ReadOnly="true" class="form-control" ID="lblUnit1" runat="server" Text="" Visible="false"></asp:TextBox>
          <asp:TextBox ReadOnly="true" class="form-control" ID="txtPricePerUnit" runat="server" Text=""></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="lblAmount" runat="server" Text="" class="form-control numeric text-right" ReadOnly="true"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="lblPG" runat="server" Text="" class="form-control numeric text-right" ReadOnly="true"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtRemarks" TabIndex="4" runat="server" Width="160px" class="form-control"  MaxLength="255"></asp:TextBox>
      <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_txtRemarks" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Custom,Numbers" ValidChars=" " TargetControlID="txtRemarks" />
        </td>
      </tr>
    </tbody>
  </table>
  </div>
                                </div>

                            </div>
                            <div class="box-footer">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="AddToCart" runat="server" Text="Add To Cart" class="btn btn-primary" OnClick="AddToCart_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                            </div>
                            <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                        </div>
                    </div>
                </div>


                <%--<div class="box-body" id="rptmain" runat="server" style="display: none;">
                    <div class="row">
                        <div class="col-xs-12">

                            <div class="box">
                                <div class="box-header">
                                    <h3 class="box-title">Purchase Order List</h3>
                                    <div style="float: right">
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />

                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="col-md-12">
                                        <div class="col-md-12 paddingleft0">
                                            <div id="DIV1" class="form-group col-md-3 paddingleft0">
                                                <label for="exampleInputEmail1">From Date:</label>
                                                <asp:TextBox ID="txtmDate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtmDate" runat="server" />
                                            </div>
                                            <div class="form-group col-md-3 ">
                                                <label for="exampleInputEmail1">To Date:</label>
                                                <asp:TextBox ID="txttodate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                            </div>
                                            <div class="form-group col-md-3 ">
                                                <label for="exampleInputEmail1">Distributor:</label>
                                                <asp:DropDownList ID="ddlDist" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="form-group col-md-3">
                                                <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                                <asp:Button type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClick="btnGo_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- /.box-header -->
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <th>Date</th>
                                                        <th>DocID</th>
                                                        <th>Distributor</th>
                                                        <th>Order Status</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr onclick="DoNav('<%#Eval("POrdId") %>');">
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("POrdId") %>' />
                                                <td><%#String.Format("{0:dd/MM/yyyy}", Eval("VDate"))%></td>
                                                <td><%#Eval("PODocId") %></td>
                                                <td><%#Eval("PartyName") %></td>
                                                <td><%#Eval("OrderStatus") %></td>
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

                </div>--%>
            </section>

        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
