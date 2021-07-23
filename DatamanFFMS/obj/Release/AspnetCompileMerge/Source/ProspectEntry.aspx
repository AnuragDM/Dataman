<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ProspectEntry.aspx.cs" Inherits="AstralFFMS.ProspectEntry" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
   <script type="text/javascript">
       //function pageLoad() {
       //    $(".select2").select2();
       //};
    </script>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            /*display: table;*/
        }
    </style>
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
            if ($('#<%=txtName.ClientID%>').val() == "") {
                errormessage("Please enter the Name");
                return false;
            }
            if ($('#<%=txtAddress.ClientID%>').val() == "") {
                errormessage("Please enter the Address");
                return false;
            }
            if ($('#<%=ddlcountry.ClientID%>').val() == "0") {
                errormessage("Please select the Country");
                return false;
            }
            if ($('#<%=ddlstate.ClientID%>').val() == "0") {
                errormessage("Please select the State");
                return false;
            }
            if ($('#<%=ddlcity.ClientID%>').val() == "0") {
                errormessage("Please select the City");
                return false;
            }
            if ($('#<%=txtMobileNo.ClientID%>').val() == "") {
                errormessage("Please enter Mobile No.");
                return false;
            }
            varmblLength = "";
            varmblLength = ($('#<%=txtMobileNo.ClientID%>').val().length);
            if (varmblLength < 10) {
                errormessage("Please enter 10 digit mobile No.");
                return false;
            }
            if ($('#<%=txtEmail.ClientID%>').val() != "") {
                var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
                var emailList = $('#<%=txtEmail.ClientID%>').val();

                if (!(emailList.trim()).match(mailformat)) {
                    errormessage("Invalid Email To Address.");
                    return false;
                }
            }
            if ($('#<%=txtRemarks.ClientID%>').val() == "") {
                errormessage("Please enter the Remark");
                return false;
            }
    
                
            
        }

    </script>
     <script type="text/javascript">
         $(document).ready(function () {
             var valLength = "";
             $('#<%=txtName.ClientID%>').keypress(function (key) {

                valLength = ($('#<%=txtName.ClientID%>').val().length + 1);

                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
        });
    </script>
    <script type="text/javascript">
        function load1() {
            $(".numeric").numeric({decimal: false, negative: false });
        }
        $(window).load(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(load1);
        });
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
                            <h3 class="box-title">Prospect Entry</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                    OnClick="btnFind_Click" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-lg-5 col-md-7 col-sm-7 col-xs-11">
                                <%-- <div class="col-md-12 paddingleft0">
                                   <div id="DIVUnder"  runat="server" class="form-group col-md-6 paddingleft0">
                                    <label for="exampleInputEmail1">User:</label>
                                  <asp:DropDownList ID="ddlUndeUser" runat="server" AutoPostBack="true"   CssClass="form-control select2" ></asp:DropDownList>
                                </div>
                                        <div id="divdocid"  runat="server" class="form-group col-md-6 paddingright0">
                                    <label for="exampleInputEmail1">Document No:</label>
                                    <asp:TextBox ID="lbldocno" Enabled="false" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                </div>--%>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                 <div class="form-group">
                                <label for="withSales">Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                 <asp:TextBox ID="txtName" MaxLength="50" runat="server" CssClass="form-control" style="background-color:white;" placeholder="Enter Name"></asp:TextBox>
                            </div>
                                   <div class="form-group">
                                <label for="withSales">Address:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                 <asp:TextBox ID="txtAddress"  style="resize: none; height: 20%;" TextMode="MultiLine" runat="server" CssClass="form-control" placeholder="Enter Address"></asp:TextBox>
                            </div>
                                 <div class="form-group">
                                <label for="withSales">Country:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddlcountry" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlcountry_SelectedIndexChanged"  CssClass="form-control" runat="server"></asp:DropDownList>
                            </div>
                                 <div class="form-group">
                                <label for="withSales">State:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddlstate" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlstate_SelectedIndexChanged"  CssClass="form-control" runat="server"></asp:DropDownList>
                            </div>
                                  <div class="form-group">
                                <label for="withSales">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddlcity" Width="100%"  CssClass="form-control" runat="server"></asp:DropDownList>
                            </div>
                                         <div class="form-group">
                                <label for="withSales">Pin No:</label>
                                 <asp:TextBox ID="txtpin" MaxLength="6" runat="server" CssClass="form-control numeric text-right" placeholder="Enter Pincode"></asp:TextBox>
                            </div>
                                 <div class="form-group">
                                <label for="withSales">Email:</label>
                                 <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" style="background-color:white;" placeholder="Enter Email"></asp:TextBox>
                                   
                            </div>
                                 <div class="form-group">
                                <label for="withSales">Mobile No:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                 <asp:TextBox ID="txtMobileNo" runat="server" MaxLength="10" CssClass="form-control numeric text-right" style="background-color:white;" placeholder="Enter Mobile No."></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="exampleInputEmail1">Remark:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:TextBox ID="txtRemarks"  style="resize: none; height: 20%;" TextMode="MultiLine" runat="server" class="form-control" cols="20" rows="2" placeholder="Enter Remark"></asp:TextBox>
                            </div>
                                         </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                             <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm();" OnClick="btnDelete_Click" />
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
                            <h3 class="box-title">Prospects List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                         <div class="box-body">
                            <div class="col-md-10">
                         
                            <div id="DIV1"   class="form-group col-md-4">
                                    <label for="exampleInputEmail1">From Date:</label>
                               <asp:TextBox id="txtmDate" runat="server" CssClass="form-control"></asp:TextBox>
                                 <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange"  Format="dd/MMM/yyyy" TargetControlID="txtmDate" runat="server" />
                                </div> 
                                <div  class="form-group col-md-4 ">
                                    <label for="exampleInputEmail1">To Date:</label>
                               <asp:TextBox id="txttodate" runat="server" CssClass="form-control"></asp:TextBox>
                                 <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange"  Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                </div> 
                                <div  class="form-group col-md-4">
                                       <label for="exampleInputEmail1" style="display:block; visibility:hidden">zkjfhksj</label>
                                  <asp:Button type="button" ID="btnGo" style="padding: 3px 7px;" runat="server" Text="Go" class="btn btn-primary" OnClick="btnGo_Click" />
                                </div> 
                                
                                </div></div> 
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Date</th>
                                                <th>Name</th>
                                                <th>Email ID</th>
                                                <th style="text-align:right;">Mobile No.</th>
                                                <%--<th>Remark</th>--%>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("ProspectId") %>');">
                                         <td> <%#String.Format("{0:dd/MMM/yyyy}", Eval("Created_Date"))%></td>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("[ProspectId]") %>' />
                                        <td><%#Eval("Name") %></td>
                                        <td><%#Eval("Email") %></td>
                                        <td style="text-align:right;"><%#Eval("Mobile") %></td>
                                      <%-- <td><%#Eval("Remark") %></td>--%>
                                       
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
