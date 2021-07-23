<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="OrderTemplates.aspx.cs" Inherits="AstralFFMS.OrderTemplates" %>
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
            if ($('#<%=hiditemid.ClientID%>').val() == "") {
                errormessage("Please Select the Item");
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
        <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Item Template</h3>
                                <div class="form-group">
                                    <div class="col-sm-12">
                                  
                                        <div class="col-lg-2 col-md-1 col-sm-8 paddingleft0" style="float: right">
                                            <asp:Button ID="btnBack1" runat="server" Text="Back" Style="margin-right: 5px; margin-bottom: 5px;" class="btn btn-primary" OnClick="btnBack_Click1" />

                                            <asp:HiddenField ID="HiddenField2" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">                                   
                                    <div class="col-md-4 col-sm-6 col-xs-12">
                                        <div class="form-group" >
                                            <label for="exampleInputEmail1">Select Item:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br /> 
                                                   <asp:TextBox ID="txtitem" runat="server" class="form-control a" placeholder="Select Item"   ></asp:TextBox>
                                            <ajaxtoolkit:autocompleteextender id="txtSearch_AutoCompleteExtender" runat="server" completionlistcssclass="completionList"
                                             completionlistitemcssclass="listItem" completionlisthighlighteditemcssclass="itemHighlighted" completioninterval="0"
                                             behaviorid="txtSearch_AutoCompleteExtender" firstrowselected="false" onclientitemselected="ClientItemSelected"
                                             delimitercharacters="" servicemethod="SearchItem" servicepath="~/OrderTemplates.aspx" minimumprefixlength="3" enablecaching="true"
                                             targetcontrolid="txtitem">
                                           </ajaxtoolkit:autocompleteextender>
                                              <asp:HiddenField ID="hiditemid" runat="server" /> 
                                        </div>
                                        <div class="form-group">
                                             <asp:Button Style="margin-right: 5px;" type="button" ID="btnadd" runat="server" Text="Save" OnClientClick="return valid();"  class="btn btn-primary" ToolTip="Click Button To Add Item To Party" OnClick="btnadd_Click" />
                                        </div>
                                    </div>                                   
                                </div>
                            </div>
                      
                              <div class="box-body table-responsive">
                                <asp:Repeater ID="itemforparty" runat="server">
                                    <HeaderTemplate>
                                        <table id="example1" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>                                                                                                                                               
                                                    <th >Item</th>
                                                   
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>                          
                                            <td><%#Eval("itemname") %></td>
                                            <td><asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%#Eval("id") %>' OnClientClick="Confirm();" OnCommand="LinkButton1_Command"><img src="img/delete.png" alt="Delete" height="22" width="22"></asp:LinkButton></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="box-footer">
                                <%--<asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Save" class="btn btn-primary"
                                     />--%>
                                <%--<asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                    OnClick="btnCancel_Click" />--%>
                                <%--<asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                    OnClick="btnExport_Click" />--%>
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
    </section>
</asp:Content>
