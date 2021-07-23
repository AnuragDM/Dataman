<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="PurchaseOrderApproval.aspx.cs" Inherits="AstralFFMS.PurchaseOrderApproval" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
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
                            <h3 class="box-title">Orders</h3>
                        </div>

                        <div class="box-body">
                            <div class="col-md-12">
                                <div class="col-md-12 paddingleft0">
                                    <div id="DIV1" class="form-group col-md-2">
                                        <asp:HiddenField ID="hfCustomerId1" runat="server" />
                                        <label for="exampleInputEmail1">From Date:</label>
                                        <asp:TextBox ID="txtmDate" runat="server" CssClass="form-control" BackColor="White"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtmDate" runat="server" />
                                    </div>
                                    <div class="form-group col-md-2 ">
                                        <label for="exampleInputEmail1">To Date:</label>
                                        <asp:TextBox ID="txttodate" runat="server" CssClass="form-control" BackColor="White"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                    </div>
                                    <div class="form-group col-md-3 ">
                                        <label for="exampleInputEmail1">Distributor Name:</label>
                                        <%-- <asp:DropDownList ID="ddlDist" runat="server" Width="100%" CssClass="form-control select2"></asp:DropDownList>--%>
                                        <asp:TextBox ID="txtDist1" runat="server" class="form-control"></asp:TextBox>
                                        <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionListCssClass="completionList"
                                            OnClientItemSelected="ClientItemSelected1" EnableCaching="true" ServicePath="PurchaseOrderApproval.aspx"
                                            MinimumPrefixLength="3" ServiceMethod="SearchDistributor" TargetControlID="txtDist1">
                                        </ajaxToolkit:AutoCompleteExtender>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                            FilterType="LowercaseLetters, UppercaseLetters,Custom,Numbers" ValidChars=" "
                                            TargetControlID="txtDist1" />
                                    </div>
                                    <div class="form-group col-md-3 ">
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
                                    <div class="form-group col-md-2">
                                        <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                        <asp:Button type="button" ID="btnGo" runat="server" Text="Go" Style="padding: 3px 7px;" class="btn btn-primary" OnClick="btnGo_Click" />
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
                                                <th>Document No.</th>
                                                <th>Distributor Name</th>
                                                <th>Product Group & Dispatch To</th>
                                                <th>Order Status</th>   
                                                <%-- <th id="tdDownload1" runat="server" visible="false"></th>--%>
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
                                        <%--<td id="tdDownload" runat="server" visible="false">--%>
                                            
                                       <%--    <a id="Download" runat="server" type="text/plain" src='<%#Eval("Src") %>'>Download</a>--%>
                                        <%--    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Eval("Src") %>' Target="_blank">Download</asp:HyperLink>--%>
                                       <%-- </td>--%>
                                         <td>
                                              <asp:ImageButton ID="ImgDownload" runat="server" ImageUrl="~/img/downloads.png" OnClick="ImgDownload_Click"  Text="Dowload1" visible="false" />&nbsp;&nbsp;
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
    </section>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>

     <script type="text/javascript">
         $(function () {
             $("#example1").DataTable({
                 "order": [[0, "desc"]], "pageLength": 1000
             });
         });
    </script>


</asp:Content>
