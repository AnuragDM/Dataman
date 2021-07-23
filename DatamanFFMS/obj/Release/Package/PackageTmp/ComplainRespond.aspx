<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ComplainRespond.aspx.cs" Inherits="AstralFFMS.ComplainRespond" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
            .dtwidth{
               /* width:100px;*/
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
    <script type="text/javascript">
        function DoNav(POID) {
            if (POID != "") {
                __doPostBack('', POID)
            }
        }
    </script>
    <script type="text/javascript">
        function ClientItemSelected1(sender, e) {
            $get("<%=hfCustomerId1.ClientID %>").value = e.get_value();
        }
    </script>
     <script type="text/javascript">
         var V1 = "";
         function errormessage(V1) {
             $("#messageNotification").jqxNotification({
                 width: 250, position: "top-right", opacity: 2,
                 autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 15000, template: "error"
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
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 15000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <script type="text/javascript">
        function validate() {

            if ($('#<%=ddlcompSugg.ClientID%>').val() == "0") {
                errormessage("Please select type Complaint or Suggestion");
                return false;
            }
            if ($('#<%=ddlStatus.ClientID%>').val() == "0") {
                errormessage("Please select status");
                return false;
            }
        }
        function DispData(val) {
            if (val == 'S') {
                $('#divptype').show();
                $('#divpname').show();
                $('#divdist').hide();
            }
            else {
                $('#divptype').hide();
                $('#divpname').hide();
                $('#divdist').show();
            }
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


        <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <%--<h3 class="box-title">Pending Complaint/Suggestion Action</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>

                        </div>

                        <div class="box-body">
                            <div class="col-md-8">
                                <div class="">
                                    <div id="DIV1" class="form-group col-md-4">
                                        <asp:HiddenField ID="hfCustomerId1" runat="server" />
                                        <label for="exampleInputEmail1">From Date:</label>
                                        <asp:TextBox ID="txtmDate" runat="server" CssClass="form-control dtwidth" BackColor="White"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtmDate" runat="server" />
                                    </div>
                                    <div class="form-group col-md-4 ">
                                        <label for="exampleInputEmail1">To Date:</label>
                                        <asp:TextBox ID="txttodate" runat="server" CssClass="form-control dtwidth" BackColor="White"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                    </div>
                                         <div class="form-group col-md-4 ">
                                        <label for="exampleInputEmail1">Type:</label>
                                             &nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                        <asp:DropDownList ID="ddlcompSugg" runat="server" Width="100%" CssClass="form-control" OnSelectedIndexChanged="ddlcompSugg_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Complaint" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Suggestion" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                         <div class="form-group col-md-6 ">
                                        <label for="exampleInputEmail1">Status:</label>
                                             &nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="100%" CssClass="form-control">
                                            <%-- <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>--%>
                                            <asp:ListItem Text="Pending" Value="P" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Resolved" Value="R"></asp:ListItem>
                                             <asp:ListItem Text="WIP" Value="W"></asp:ListItem>
                                         
                                        </asp:DropDownList>
                                    </div>
                                     <div class="form-group col-md-6 ">
                                        <label for="exampleInputEmail1">Department:</label>
                                             &nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                        <asp:DropDownList ID="ddldept" runat="server" Width="100%" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                      <div class="form-group col-md-6 ">
                                        <label for="exampleInputEmail1">Compl/Sugg Nature:</label>
                                             &nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                        <asp:DropDownList ID="ddlcompNature" runat="server" Width="100%" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                     <div class="form-group col-md-6 ">
                                        <label for="exampleInputEmail1">Compl/Sugg By:</label>
                                             &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlcomplby" runat="server" onchange="DispData(this.value);" Width="100%" CssClass="form-control">
                                            <%-- <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>--%>
                                            <asp:ListItem Text="Sales Person" Value="S" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Distributor" Value="D"></asp:ListItem>
                                         
                                        </asp:DropDownList>
                                    </div>
                                   <div class="form-group col-md-6" id="divptype">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Party Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                                    <asp:DropDownList ID="ddlpartytype" OnSelectedIndexChanged="ddlpartytype_SelectedIndexChanged" AutoPostBack="true" Width="100%" CssClass="form-control" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        
                                     <div class="form-group col-md-6 " id="divpname">
                                                    <div class="form-group">
                                                              <label id="lblpartytypepersons"  runat="server">Party Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                                        <asp:DropDownList ID="ddlpartytypepersons" Width="100%" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                             <div class="form-group col-md-12 " id="divdist" style="display:none;">
                                            
                                                    <div class="form-group">
                                                              <label id="Label1"  runat="server">Party Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                                        <asp:DropDownList ID="ddldistributor" Width="100%" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                    <%--<div class="form-group col-md-3 ">
                                        <label for="exampleInputEmail1">Distributor Name:</label>
                                        <%-- <asp:DropDownList ID="ddlDist" runat="server" Width="100%" CssClass="form-control select2"></asp:DropDownList>
                                        <asp:TextBox ID="txtDist1" runat="server" class="form-control"></asp:TextBox>
                                        <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionListCssClass="completionList"
                                            OnClientItemSelected="ClientItemSelected1" EnableCaching="true" ServicePath="PurchaseOrderApproval.aspx"
                                            MinimumPrefixLength="3" ServiceMethod="SearchDistributor" TargetControlID="txtDist1">
                                        </ajaxToolkit:AutoCompleteExtender>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                            FilterType="LowercaseLetters, UppercaseLetters,Custom,Numbers" ValidChars=" "
                                            TargetControlID="txtDist1" />
                                    </div>--%>
                                  
                                    <div class="form-group col-md-12">
                                        <label ></label>
                                           <div class="col-md-6 col-sm-6 col-xs-6">
                                        <asp:Button type="button" ID="btnGo" runat="server" Text="Go" Style="padding: 3px 7px;" OnClick="btnGo_Click" class="btn btn-primary pull-right" OnClientClick="javascript:return validate();" />
                                              </div><div class="col-md-6 col-sm-6 col-xs-6 ">
                                               <asp:Button ID="btnclr" OnClick="btnclr_Click" runat="server" Text="Clear" Style="padding: 3px 7px;"  class="btn btn-primary pull-left"/>

                                  </div>  </div>
                                 </div>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound" >
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Date</th>
                                                <th>Document No.</th>
                                                <th id="thsname"  runat="server">SalesPerson Name</th>
                                                <th>Party Name</th>
                                                <th>Department</th>
                                                <th>Compl/Sugg Nature</th>   
                                                <th>Status</th>   
                                                <th>Action</th>   
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("VDate"))%></td>
                                         <td><%#Eval("DocId") %></td>
                                        <td id="tdsname" runat="server"><%#Eval("SMName") %></td>
                                        <td><%#Eval("PartyName") %></td>
                                        <td><%#Eval("Depname") %></td>
                                        <td><%#Eval("Name") %></td>
                                        <td><%# Eval("Status")%>      
                                        <%--<td><%# Eval("Status").ToString().Equals("P") ? "Pending" : "Resolved" %>      --%>                                 
                                           </td>
                                        <td >
                                            <asp:HyperLink runat="server" ID="hpl"
                                                NavigateUrl='<%# String.Format("ComplaintRespondList.aspx?val={0}&DocId={1}&CStatus={2}", Eval("val"),Eval("DocId"),Eval("Status")) %>'
                                                Text="Action" Target="_blank" ToolTip="Action" /></td>
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
    </section>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
   <%-- <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>--%>

     <script type="text/javascript">
         $(function () {
             $("#example1").DataTable({
                 "order": [[0, "desc"]]
             });
         });
    </script>


</asp:Content>
