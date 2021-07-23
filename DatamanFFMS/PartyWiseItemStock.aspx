<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="PartyWiseItemStock.aspx.cs" Inherits="AstralFFMS.PartyWiseItemStock" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script type="text/javascript">
         var V1 = "";
         function errormessage(V1) {
             $("#messageNotification").jqxNotification({
                 width: 300, position: "top-right", opacity: 2,
                 autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "error"
             });
             $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
      <script type="text/javascript">
          function ClientItemSelected(sender, e) {
              $get("<%=hiditemid.ClientID %>").value = e.get_value();
            }
          var V = "";
          function Successmessage(V) {
              $("#messageNotification").jqxNotification({
                  width: 300, position: "top-right", opacity: 2,
                  autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "success"
              });
              $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <script>
        function valid() {
            if ($('#<%=ddlItem.ClientID%>').val() == "0" || $('#<%=ddlItem.ClientID%>').val() == "") {
                errormessage("Please select Material Group");
                return false;
            }
          
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
            //Added as per UAT - on 14 Dec 2015
            <%--if ($('#<%=txtTotalAmount.ClientID%>').val() <= 0) {
                errormessage("Please enter Order Amount greater than 0.");
                return false;
            }--%>
            //End

          
        }
    </script>
         <script type="text/javascript">
             $(function () {
                 $("[id*=trview] input[type=checkbox]").bind("click", function () {
                     var table = $(this).closest("table");
                     if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                         //Is Parent CheckBox
                         var childDiv = table.next();
                         var isChecked = $(this).is(":checked");
                         $("input[type=checkbox]", childDiv).each(function () {
                             if (isChecked) {
                                 $(this).prop("checked", "checked");
                             } else {
                                 $(this).removeAttr("checked");
                             }
                         });
                     } else {
                         //Is Child CheckBox
                         var parentDIV = $(this).closest("DIV");
                         if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                             $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                         } else {
                             $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                         }
                     }
                 });
             })
    </script>
      <script type="text/javascript">
          var specialKeys = new Array();
          specialKeys.push(8); //Backspace
          function IsNumeric(e) {
              var keyCode = e.which ? e.which : e.keyCode
              var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);

              return ret;
          }
    </script>
      <script type="text/javascript">
          function DoNav(depId) {
              if (depId != "") {
                  document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                  document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                  $('#spinner').show();
                  __doPostBack('', depId)
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
     <style type="text/css">
        .containerStaff {
            border: 1px solid #ccc;
            overflow-y: auto;
            min-height: 200px;
            width: 134%;
            overflow-x: auto;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .multiselect-container > li > a {
            white-space: normal;
        }
         .multiselect-container.dropdown-menu {
            width: 100% !important;
        }

        .input-group .form-control {
            height: 34px;
        }
    </style>
    <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body" id="mainDiv" runat="server" style="display:none;">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Party Item Template</h3>
                                <div class="form-group">
                                    <div class="col-sm-12">
                                  
                                        <div class="col-lg-2 col-md-1 col-sm-8 paddingleft0" style="float: right">
                                             <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary" OnClick="btnFind_Click" />
                                            <asp:HiddenField ID="HiddenField2" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:UpdatePanel ID="up" runat="server">
                                            <ContentTemplate>
                                                <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12" >
                                                    <label for="exampleInputEmail1">Select Item:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />
                                                    <asp:DropDownList ID="ddlItem" Width="100%" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    <asp:HiddenField ID="hiditemid" runat="server" />
                                                </div>
                                                <div class="form-group col-lg-8 col-md-8 col-sm-12 col-xs-12" style="margin-top:22px;">
                                                    <asp:Label ID="lblAvailability" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                               <%-- <div class="form-group col-lg-2 col-md-2 col-sm-12 col-xs-12">
                                                    <label for="exampleInputEmail1">Price:</label><br />
                                                    <asp:TextBox ID="txtPrice" onkeypress="return IsNumeric(event);" runat="server" Enabled="false" CssClass="form-control text-right"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-md-4 paddingleft0">
                                                <div class="block">
                                                    <label for="exampleInputEmail1">Availability C:</label><br />
                                                </div>
                                                        <asp:TextBox ID="txtC" onkeypress="return IsNumeric(event);" runat="server" Enabled="false" CssClass="form-control text-right"></asp:TextBox>
                                                 </div>
                                                <div class="form-group col-md-4 paddingleft0">
                                                <div class="block">
                                                     <label for="exampleInputEmail1">U:</label><br />
                                                </div>
                                                        <asp:TextBox ID="txtU" onkeypress="return IsNumeric(event);" runat="server" Enabled="false" CssClass="form-control text-right"></asp:TextBox>
                                                </div>--%>
                                                  <div class="clearfix"></div>
                                                <div class="form-group col-lg-4 col-md-4 col-sm-6 col-xs-6">
                                                    <label for="exampleInputEmail1">Case:</label><br />
                                                    <asp:TextBox ID="txtCase" onkeypress="return IsNumeric(event);" runat="server" CssClass="form-control text-right"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-lg-4 col-md-4 col-sm-6 col-xs-6">
                                                    <label for="exampleInputEmail1">Unit:</label><br />
                                                    <asp:TextBox ID="txtUnit" onkeypress="return IsNumeric(event);" runat="server" CssClass="form-control text-right"></asp:TextBox>
                                                </div>
                                                  <div class="form-group col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                                  </div>
                                                <div class="form-group">
                                                    
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlItem" EventName="SelectedIndexChanged" />
                                           <%--     <asp:AsyncPostBackTrigger ControlID="btnadd" EventName="Click" />--%>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                      
                           
                            <div class="box-footer">
                           <%--  <asp:Button Style="margin-right: 5px;" type="button" ID="btnadd" runat="server" Text="Save" OnClientClick="return valid();" class="btn btn-primary" ToolTip="Click Button To Add Item To Party" OnClick="btnadd_Click" />--%>
                             <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" Visible="false" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" TabIndex="32" OnClick="btnDelete_Click" />
                             <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" TabIndex="30" OnClick="btnCancel_Click"/>
                            </div>                            

                            
                            <br />
                            
                            <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Distributor Item Stock</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <div class="col-lg-9 col-md-9 col-sm-7 col-xs-9">
                                <div class="col-md-12 paddingleft0">
                                    <div id="DIV1" class="form-group col-md-3">
                                        <label for="exampleInputEmail1">Visit Date:</label>
                                        <asp:TextBox ID="txtmDate" runat="server" Style="background: white;" CssClass="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtmDate" runat="server" />
                                    </div>
                                    <div class="form-group col-md-4 " style="display:none;">
                                        <label for="exampleInputEmail1">Item :</label>
                                        <asp:DropDownList ID="ddlUnderItem" Width="100%" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                        <asp:Button type="button" ID="btnGo" runat="server"  Style="margin-right: 5px;"  Text="Go" class="btn btn-primary" OnClick="btnGo_Click" />
                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                          <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                        <div class="box-body table-responsive">
                            <asp:GridView runat="server" ID="itemforparty" AutoGenerateColumns="false" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White">
                                     <Columns>
                                         <asp:TemplateField HeaderText="S No" ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Item Name" ItemStyle-Width="30%">
                                                    <ItemTemplate>
                                                       <asp:Label runat="server" ID="lblitemname" Text='<%#Eval("itemname") %>'></asp:Label>
                                                        <asp:HiddenField ID="hfItemId" runat="server" Value='<%#Eval("ItemId") %>' />
                                                           <asp:HiddenField ID="hfSTKId" runat="server" Value='<%#Eval("STKId") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Unit" ItemStyle-Width="10%">
                                                    <ItemTemplate>
                                                       <asp:Label runat="server" ID="lblunit" Text='<%#Eval("unit") %>' CssClass="form-control numeric text-right"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rate" ItemStyle-Width="10%">
                                                    <ItemTemplate>
                                                       <asp:Label runat="server" ID="lblrate" Text='<%#Eval("mrp") %>' CssClass="form-control numeric text-right"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Std. Pack" ItemStyle-Width="10%">
                                                    <ItemTemplate>
                                                       <asp:Label runat="server" ID="lblstdpack" Text='<%#Eval("UnitFactor") %>' CssClass="form-control numeric text-right"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                       
                                             <asp:TemplateField HeaderText="Case Qty" ItemStyle-Width="10%">
                                                    <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtquantity" placeholder="Enter Case Quantity" AutoPostBack="true"   Text='<%#Eval("CaseQty") %>' CssClass="form-control numeric text-right" OnTextChanged="txtquantity_TextChanged"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               <asp:TemplateField HeaderText="Loose Qty" ItemStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtLooseQty" placeholder="Enter Loose Qty"  AutoPostBack="true"  CssClass="form-control numeric text-right" Text='<%#Eval("LooseQty") %>' OnTextChanged="txtCaseQty_TextChanged"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stock Qty" ItemStyle-Width="10%">
                                                    <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblStockQty" Text='<%#Eval("StockQty") %>' CssClass="form-control numeric text-right"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                           <%--  <asp:TemplateField HeaderText="Amount" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                      <asp:Label runat="server" ID="lblamount" CssClass="form-control numeric text-right" Text='<%#Eval("amt") %>'></asp:Label>
                                                    </ItemTemplate>
                                            </asp:TemplateField>--%>
                                     </Columns>
                                  </asp:GridView>
                        </div>
                           <div class="box-footer">
                               <asp:Button Style="margin-right: 5px;" type="button" ID="btnadd" runat="server" Text="Save" class="btn btn-primary" ToolTip="Click Button To Add Item Stock To Distributor" OnClick="btnadd_Click" />
                            </div>
                                      
                                        </ContentTemplate>
                                <Triggers>
                                     
                                      <asp:AsyncPostBackTrigger ControlID="btnadd" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                        <!-- /.box-body -->
                    </div>
                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>

        </div>
    </section>
</asp:Content>
