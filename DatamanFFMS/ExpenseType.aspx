<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ExpenseType.aspx.cs" Inherits="AstralFFMS.ExpenseType" EnableEventValidation="false" %>
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
        //$(function () {
        //    $(".select2").select2();
        //});
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
    <script type="text/javascript">
        function validate() {

            if ($('#<%=ExpName.ClientID%>').val() == "") {
                errormessage("Please enter Name");
                return false;
            }
          
        }
    </script>
    
      <script>
          function isNumber(evt) {
              var iKeyCode = (evt.which) ? evt.which : evt.keyCode
              if (!(iKeyCode != 8)) {
                  e.preventDefault();
                  return false;
              }
              return true;
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
        function DoNav(Id) {
            if (Id != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', Id)
            }
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
        <div class="box-body" id="mainDiv" style="display: none;" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <%--<h3 class="box-title">Expense Type Master</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                   OnClick="btnFind_Click" />

                                <%--  <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" runat="server" />--%>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                            <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                  <div class="form-group">
                                <label for="exampleInputEmail1">Expense Category:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                       <asp:DropDownList ID="ddlExpCat" runat="server" CssClass="form-control" tabindex="1">
                            <asp:ListItem Text="BOARDING" Value="BOARDING" Selected="True"></asp:ListItem>
                           <asp:ListItem Text="CAPITALPURCHASE" Value="CAPITALPURCHASE" ></asp:ListItem>
                           <asp:ListItem Text="COMMUNICATION" Value="COMMUNICATION" ></asp:ListItem>
                           <asp:ListItem Text="CONVEYANCE" Value="CONVEYANCE" ></asp:ListItem>
                           <asp:ListItem Text="CONVEYANCETRAVEL" Value="CONVEYANCETRAVEL" ></asp:ListItem>
                           <asp:ListItem Text="COURIER" Value="COURIER" ></asp:ListItem>
                           <asp:ListItem Text="ENTERTAINMENT" Value="ENTERTAINMENT" ></asp:ListItem>
                           <asp:ListItem Text="FOODSTAFF" Value="FOODSTAFF" ></asp:ListItem>
                           <asp:ListItem Text="LAUNDRY" Value="LAUNDRY" ></asp:ListItem>
                           <asp:ListItem Text="LODGING" Value="LODGING" ></asp:ListItem>
                           <asp:ListItem Text="OTHER" Value="OTHER" ></asp:ListItem>
                           <asp:ListItem Text="STATIONARY" Value="STATIONARY" ></asp:ListItem>
                           <asp:ListItem Text="TRAVEL" Value="TRAVEL" ></asp:ListItem>
                       </asp:DropDownList>
                            </div>  
                                   <div class="form-group">
                                <label for="exampleInputEmail1">Expense Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                              <input type="text" runat="server" class="form-control" maxlength="50" id="ExpName" placeholder="Enter Name" tabindex="2">
                            </div>
                            <div class="form-group">
                                <label for="exampleInputEmail1">Remark:</label>
                                <input type="text" runat="server" class="form-control" maxlength="20" id="Remarks" placeholder="Enter Remarks" tabindex="4">
                            </div>
                                 <div class="form-group">
                                <label for="exampleInputEmail1">Sync Id:</label>
                                <input type="text" runat="server" class="form-control" maxlength="20" id="SyncId" placeholder="Enter Sync Id" tabindex="6">
                            </div>
                            <div class="form-group">
                                <label for="exampleInputEmail1">Active:</label>
                                <input id="chkIsActive" runat="server" type="checkbox" checked="checked" class="checkbox" tabindex="12" />
                                <input id="HdnFldIsActive" hidden="hidden" value="N" />
                            </div>
                            </div></div>
                        </div>
                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary"  OnClientClick="javascript:return validate();" TabIndex="28" OnClick="btnSave_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" TabIndex="30" OnClick="btnCancel_Click"/>
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" TabIndex="32" OnClick="btnDelete_Click" />
                        </div>
                         <br />
                           <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
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
                            <h3 class="box-title">Expense Type List</h3>
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
                                                <th>Name</th>
                                                <th>Expense Category</th>
                                                 <th>Remark</th>
                                                <th>Sync ID</th>
                                                <th>Active</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("Id") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("Id") %>' />
                                    
                                        <td><%#Eval("Name") %></td>
                                        <td><%#Eval("ExpenseTypeCode") %></td>
                                            <td><%#Eval("Remarks") %></td>
                                        <td><%#Eval("SyncId") %></td>
                                        <td><%#Eval("Active1") %></td>
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
            $("#example1").DataTable();

        });
    </script>
</asp:Content>