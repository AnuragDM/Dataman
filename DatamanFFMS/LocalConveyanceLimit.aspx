<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="LocalConveyanceLimit.aspx.cs" Inherits="AstralFFMS.LocalConveyanceLimit" EnableEventValidation="false" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
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
         .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
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

            if ($('#<%=ddlcitytype.ClientID%>').val() == "0") {
                errormessage("Please select City Type");
                return false;
            }
            if ($('#<%=ddldesignation.ClientID%>').val() == "0") {
                errormessage("Please select Designation");
                return false;
            }
            if ($('#<%=Amount.ClientID%>').val() == "") {
                errormessage("Please enter Amount");
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
                            <%--<h3 class="box-title">Local Conveyance Master</h3>--%>
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
                            <div class="col-lg-5 col-md-6 col-sm-8 col-xs-12">
                                     <div class="form-group">
                                <label for="exampleInputEmail1">City Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddlcitytype" width="100%" CssClass="form-control" runat="server" TabIndex="2"></asp:DropDownList>
                            </div>
                                    <div class="form-group">
                                <label for="exampleInputEmail1">Designation:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddldesignation" width="100%" CssClass="form-control" runat="server" TabIndex="4"></asp:DropDownList>
                            </div>
                                        <div class="form-group">
                                <label for="exampleInputEmail1">Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                        <input type="text" runat="server" class="form-control numeric text-right" onkeypress="javascript:return isNumber (event)" maxlength="15" id="Amount" placeholder="Enter Amount" tabindex="6">
                            </div>
                                  <div class="form-group">
                                <label for="exampleInputEmail1">Remark:</label>
                                <input type="text" runat="server" class="form-control" maxlength="500" id="Remarks" placeholder="Enter Remark" tabindex="8">
                            </div>
                            <div class="form-group">
                                <label for="exampleInputEmail1">Sync Id:</label>
                                <input type="text" runat="server" class="form-control" maxlength="20" id="SyncId" placeholder="Enter Sync Id" tabindex="10">
                            </div>
                            <div class="form-group">
                             
                                <div class="row">
                                    <div class="col-md-1">
                                        <label for="exampleInputEmail1">Active:</label>
                                    </div>
                                    <div class="col-md-4" style="margin-left: 1%;">
                                        <input id="chkIsActive" runat="server" type="checkbox" checked="checked" class="checkbox" tabindex="12" />
                                    </div>
                                </div>
                                <input id="HdnFldIsActive" hidden="hidden" value="N">
                                  <div class="row">
                                        <div class="col-md-1">
                                             <label for="exampleInputEmail1">Ex:</label>
                                        </div>
                                      <div class="col-md-4" style="margin-left: 1%;">
                                            <input id="ChkEx" runat="server" type="checkbox"  class="checkbox" tabindex="13" />
                                          </div>
                                  </div>
                              
                              
                            </div>
                                

                            </div></div>
                        </div>
                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary"  OnClientClick="javascript:return validate();" TabIndex="28" OnClick="btnSave_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" TabIndex="30" OnClick="btnCancel_Click"/>
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" TabIndex="32" OnClick="btnDelete_Click" />
                        </div>  <br />
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
                            <h3 class="box-title">Local Conveyance List</h3>
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
                                                <th>City Type</th>
                                                 <th>Designation</th>
                                                <th>Ex</th>
                                                <th>Amount</th>
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
                                            <td><%#Eval("DesName") %></td>
                                         <td><%#Eval("Ex") %></td>
                                        <td><%#Eval("Amount") %></td>
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
