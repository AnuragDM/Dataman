<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="EmployeeWiseConvLimit.aspx.cs" Inherits="AstralFFMS.EmployeeWiseConvLimit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <%--<script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style type="text/css">
        .spinner {
            position: absolute;
            left: 50%;
            margin-left: -50px; /* half width of the spinner gif */
            margin-top: -50px; /* half height of the spinner gif */
            text-align: center;
            z-index: 999;
            overflow: auto;
            width: 100px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }
        #select2-ContentPlaceHolder1_ddlParentLoc-container {
        margin-top:-8px !important;
        }
    </style>
    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
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
                 width: 300, position: "top-right", opacity: 2,
                 autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "success"
             });
             $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");
        }
    </script>
    <section class="content">
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
        </div>
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div id="divData" runat="server">
                                        <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-md-12">

                    <div class="box">
                        <div class="box-header">
                           <%-- <h3 class="box-title">Employee Wise Conveyance Limit</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                       </div>
                       
                        <div class="clearfix"></div>
                        <div class="col-md-3 col-sm-6 form-group">   
                            <label>Search Employee:</label> <label for="requiredFields" style="color: red;">*</label>

                               <asp:TextBox ID="txtsalespersons" Width="100%" runat="server" class="form-control a"  TabIndex="1"></asp:TextBox>

                                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="completionList"
                                                    OnClientItemSelected="ClientEmpIDSelected" EnableCaching="true" ServicePath="EmployeeWiseConvLimit.aspx"
                                                    MinimumPrefixLength="3" ServiceMethod="SearchSalesPerson" TargetControlID="txtsalespersons">
                                                </ajaxToolkit:AutoCompleteExtender>
                            <asp:HiddenField ID="hdnSMId" Value="" runat="server" />
                      
                         </div>
                     
                      <div class="clearfix"></div>
                        <div class="col-md-6 form-group">
                            <label>City:</label><label for="requiredFields" style="color: red;">*</label>
                            <asp:DropDownList ID ="ddlcity" TabIndex="2" CssClass="form-control" runat="server"></asp:DropDownList>
                        </div> 
                         
                       
                          <div class="col-md-6 form-group">
                            <label>Conveyance Amount:</label><label for="requiredFields" style="color: red;">*</label>
                      <input type="text" class="form-control numeric text-right" style="width:150px;" maxlength="10" tabindex="3" runat="server" id="convamt" placeholder="Conveyance Amount">
                        </div> 
                          <div class="form-group">
                            <asp:Button ID="btnadd" runat="server" Text="Add" OnClientClick="return Validate();" style="margin:16px;" OnClick="btnadd_Click" CssClass="btn btn-primary" />
                               <asp:Button ID="btnCancel" runat="server" Text="Cancel"  OnClick="btnCancel_Click" CssClass="btn btn-primary" />
                               <asp:Button ID="btnsearch" runat="server" Text="Search" style="margin:16px;" OnClick="btnsearch_Click" CssClass="btn btn-primary" />
                               
                        </div>
                      
                     
                        <div class="clearfix"></div>
                        </div>
                       <div class="box-body" id="divrpt" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Employee</th>
                                                 <th>City</th>
                                                <th>Conveyance Amt.</th>
                                                <th>Delete</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("Id") %>' />
                                        <td><%#Eval("smname") %></td>
                                        <td><%#Eval("displayname") %></td>
                                        <td><%#Eval("ConveyanceAmt") %></td>
                                         <td><asp:LinkButton ID="lnkdelete" runat="server" OnClick="lnkdelete_Click" OnClientClick="ConfirmDelete();">Delete</asp:LinkButton></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                      
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div></div>
                  </div></div></div></div>
                        </div></div></div>
                             

        

    </section>
    
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        $('.a').keyup(function (e) {
            if (e.keyCode == 8) {
                $('#<%=hdnSMId.ClientID%>').val(""); 
            }
        });


        $(function () {
            $("#example1").DataTable();

        });
        function ClientEmpIDSelected(sender, e) {
            $get("<%=hdnSMId.ClientID %>").value = e.get_value();
        }
        function ConfirmDelete() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to delete this record ?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

        function Validate() {
         
            if ($('#<%=hdnSMId.ClientID%>').val() == "") {
                errormessage("Please select correct Employee Name");
                return false;
            }
         if ($('#<%=ddlcity.ClientID%>').val() == "" || $('#<%=ddlcity.ClientID%>').val() == "0") {
                    errormessage("Please select City");
                return false;
            }
            if ($('#<%=convamt.ClientID%>').val() == "") {
                errormessage("Please enter conveyance amount");
                return false;
            } 
        }
    </script>
</asp:Content>
