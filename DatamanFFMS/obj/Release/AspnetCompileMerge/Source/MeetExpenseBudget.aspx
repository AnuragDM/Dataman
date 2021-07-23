<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MeetExpenseBudget.aspx.cs" Inherits="AstralFFMS.MeetExpEntry" %>

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
    <script type="text/javascript">
        function validate() {
            if ($('#<%=ddlMeetType.ClientID%>').val() == "0") {
                errormessage("Please Select the Meet Type");
                return false;
            }
           
        }

    </script>

     <script type="text/javascript">
         function cal() {
             var qty = document.getElementById("ContentPlaceHolder1_txttotalQty").value;
             var ApproxCost = document.getElementById("ContentPlaceHolder1_txtAPPROXcOST").value;
             if (qty != "" || ApproxCost != "") {
                 document.getElementById("ContentPlaceHolder1_txtEstimatedCost").value = ApproxCost * qty;
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
        function DoNav(depId) {
            if (depId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                __doPostBack('', depId)
            }
        }
    </script>
    <section class="content">
       
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body" id="mainDiv" style="display: none;" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Meet Expense Budget Master</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                    OnClick="btnFind_Click" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                 <div class="form-group">
                                <label for="withSales">Meet Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddlMeetType"  CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                                 <div class="form-group">
                                <label for="withSales">Area Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddlAreaType"  CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                                 <div class="form-group">
                                <label for="withSales">Expense Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddlexpenseName"  CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                                   <div class="form-group">
                                <label for="withSales">Approx Cost Per Head:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                 <asp:TextBox ID="txtAPPROXcOST" runat="server"  onchange="cal();"   MaxLength="12" rows="6" Cols="3" CssClass="form-control numeric"></asp:TextBox>
                            </div>
                                <div class="form-group">
                                <label for="withSales">Total Quantity:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                 <asp:TextBox ID="txttotalQty" runat="server"  onchange="cal();"   MaxLength="12" rows="6" Cols="3" CssClass="form-control numeric"></asp:TextBox>
                            </div>
                                <div class="form-group">
                                <label for="withSales">Estimated Costy:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                 <asp:TextBox ID="txtEstimatedCost" runat="server" ReadOnly="true"  MaxLength="12" rows="6" Cols="3" CssClass="form-control numeric"></asp:TextBox>
                            </div>
                            </div>
                        </div>
                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                             <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm();" OnClick="btnDelete_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="box-body" id="rptmain" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Meet Expense Budget List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Meet Name</th>
                                                <th>Area</th>
                                                 <th>Expense Name</th>
                                                 <th>Approx Cost</th>
                                                <th>Total Qty</th>
                                                 <th>Estimated Cost</th>
                                           
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("Id") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("Id") %>' />
                                        <td><%#Eval("MeetName") %></td>
                                        <td><%#Eval("Area") %></td>
                                         <td><%#Eval("ExpenseName") %></td>
                                          <td><%#Eval("ApproxCost") %></td>
                                          <td><%#Eval("TotalQty") %></td>
                                          <td><%#Eval("EstimatedCost") %></td>
                                       
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
                "order": [[0, "desc"]]
            });

        });
    </script>

</asp:Content>


