<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="AddNewMeetUser.aspx.cs" Inherits="AstralFFMS.AddNewMeetUser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style>
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .table-responsive {
            border: 1px solid #fff;
        }
    </style>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#ContentPlaceHolder1_basicExample').timepicker();
            $('#ContentPlaceHolder1_basicExample1').timepicker();
   
    </script>
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
    <script type="text/javascript">
        function load1() {
            $("#example1").DataTable();
            $(".numeric").numeric({ negative: false });
        }
        $(window).load(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(load1);
        });

    </script>
    <script type="text/javascript">
        function validate() {

            if ($('#<%=ddlmeetType.ClientID%>').val() == "0") {
                errormessage("Please select the Meet Type.");
                return false;
            }
            
          <%--  if ($('#<%=ddlmeetName.ClientID%>').val() == "0") {
                errormessage("Please select the Meet Name.");
                return false;
            }--%>

         <%--   if ($('#<%=ddlarea.ClientID%>').val() == "0") {
                errormessage("Please select the Area.");
                return false;
            }
            if ($('#<%=ddlbeat.ClientID%>').val() == "0") {
                errormessage("Please select the beat.");
                return false;
            }--%>
           <%-- if ($('#<%=txtName.ClientID%>').val() == '') {
                errormessage("Please enter the Name.");
                return false;
            }--%>
           <%-- if ($('#<%=ddlpartyType.ClientID%>').val() == "0") {
                errormessage("Please select the Party Type.");
                return false;
            }--%>
            if ($('#<%=txtContactperson.ClientID%>').val() == '') {
                errormessage("Please enter the Contact Person.");
                return false;
            }
            if ($('#<%=txtmobile.ClientID%>').val() == '') {
                errormessage("Please enter the Mobile.");
                return false;
            }
            varmblLength = "";
            varmblLength = ($('#<%=txtmobile.ClientID%>').val().length);
            if (varmblLength < 10) {
                errormessage("Please enter 10 digit mobile No.");
                return false;
            }
            if ($('#<%=txtemail.ClientID%>').val() != "") {
                var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
                var emailList = $('#<%=txtemail.ClientID%>').val();

                if (!(emailList.trim()).match(mailformat)) {
                    errormessage("Invalid Email To Address.");
                    return false;
                }
            }

           <%-- if ($('#<%=ddlnextcity.ClientID%>').val() == "0") {
                errormessage("Please select the next City.");
                return false;
            }
            if ($('#<%=txtdistName.ClientID%>').val() == '') {
                errormessage("Please enter the distributer Name.");
                return false;
            }
            if ($('#<%=hfCustomerId.ClientID%>').val() == '') {
                errormessage("Please enter the valid distributer Name.");
                return false;
            }
            if ($('#<%=hfCustomerId.ClientID%>').val() == 0) {
                errormessage("Please enter the valid distributer Name.");
                return false;
            }--%>
            
        }

    </script>

    <script type="text/javascript">
        function validateGoBtn() {
            
            if ($('#<%=ddlmeettype1.ClientID%>').val() == "0") {
                errormessage("Please select the Meet Type.");
                return false;
            }
        }
    </script>

    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to Delete?")) {
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


    <%--<script type = "text/javascript">
    function ClientItemSelected(sender, e) {
        $get("<%=hfCustomerId.ClientID %>").value = e.get_value();
    }
</script>--%>

    <%--<script type = "text/javascript">
    function SetContextKey() {
        $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey($get("<%=ddlcity.ClientID %>").value);
    }
</script>--%>

    <script type="text/javascript">
        function checkDate(sender, args) {
            if (sender._selectedDate > new Date()) {
                errormessage("You cannot select a day greater than today!");
                sender._selectedDate = new Date();
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
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
                            <h3 class="box-title">Add Meet User</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                    OnClick="btnFind_Click" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <asp:UpdatePanel ID="update1" runat="server">
                            <ContentTemplate>
                                <div class="box-body">
                                    <div class="col-lg-5 col-md-6 col-sm-7 col-xs-9">

                                        <div class="form-group paddingleft0">
                                            <label for="exampleInputEmail1">Sales Person:</label>
                                            <asp:DropDownList ID="ddlunderUser" AutoPostBack="true" OnSelectedIndexChanged="ddlunderUser_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>

                                        </div>

                                        <div class="form-group col-md-6 paddingleft0">
                                            <label for="withSales">Meet Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlmeetType" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlmeetType_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div id="DIVUnder" class="form-group col-md-6 paddingright0" runat="server">
                                            <label for="exampleInputEmail1">Meet Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlmeetName" Width="100%" runat="server" OnSelectedIndexChanged="ddlmeetName_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div runat="server" class="form-group col-md-6 paddingleft0">
                                            <label for="exampleInputEmail1">Area:</label>
                                            <asp:DropDownList ID="ddlarea" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlarea_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                        </div>

                                        <div class="form-group col-md-6 paddingright0">
                                            <label for="withSales">Beat:</label>
                                            <asp:DropDownList ID="ddlbeat" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-6 paddingleft0">
                                       <label for="exampleInputEmail1">Party Name:</label>
                                            <asp:TextBox ID="txtName" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                                        </div>

                                        <div class="form-group col-md-6  paddingright0">
                                       <label for="exampleInputEmail1">DOB:</label>
                                       <asp:TextBox ID="txtDOB" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;" ></asp:TextBox>
                                       <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange"  Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender" TargetControlID="txtDOB"></ajaxToolkit:CalendarExtender>
                                        </div>

                                        <div class="form-group col-md-6 paddingright0" hidden>
                                            <label for="exampleInputEmail1">Party Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlpartyType" Width="100%" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Address:</label>
                                            <asp:TextBox ID="txtaddress" runat="server" MaxLength="500" Style="resize: none; height: 20%;" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>

                                        </div>
                                        <div class="form-group col-md-6 paddingleft0">
                                            <label for="exampleInputEmail1">Contact Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtContactperson" runat="server" MaxLength="50" CssClass="form-control" Style="background-color: white;"></asp:TextBox>

                                        </div>
                                        <div class="form-group col-md-6 paddingright0">
                                            <label for="exampleInputEmail1">Mobile No:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtmobile" runat="server" MaxLength="10" CssClass="form-control numeric" Style="background-color: white;"></asp:TextBox>
                                        
                                        </div>
                                        <div class="form-group col-md-6 paddingleft0">
                                            <label for="exampleInputEmail1">Email:</label>
                                            <asp:TextBox ID="txtemail" runat="server" MaxLength="50" CssClass="form-control" Style="background-color: white;"></asp:TextBox>

                                        </div>
                                        <div class="form-group col-md-6 paddingright0">
                                            <label for="exampleInputEmail1">Potential:</label>
                                            <asp:TextBox ID="txtPotential" MaxLength="12" runat="server" CssClass="form-control numeric" Style="background-color: white;"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="box-footer">
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"  OnClick="btnCancel_Click" />
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" OnClick="btnDelete_Click" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <div>If you don't have mobile number, enter 9999999999</div>
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
                            <h3 class="box-title">User List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <div class="col-lg-9 col-md-7 col-sm-7 col-xs-9">
                                <div class="col-md-12 paddingleft0">

                                    <div class="form-group col-md-6 paddingleft0">
                                        <label for="withSales">Meet Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlmeettype1" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlmeettype1_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>


                                    <div id="DIV1" class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Meet Name:</label>
                                        <asp:DropDownList ID="ddlmeet1" Width="100%" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-md-4 ">
                                        <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                        <asp:Button type="button" ID="btnGo" runat="server" Style="padding: 3px 14px;" Text="Go" class="btn btn-primary go-button-dsr" OnClick="btnGo_Click" OnClientClick="javascript:return validateGoBtn();" />
                                    </div>
                                    <div class="form-group col-md-4">
                                        <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Meet Name</th>
                                                <th>Area Name</th>
                                                <th>Party Name</th>
                                                <th>Contact Name</th>
                                                <th>Mobile No</th>

                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("Id") %>');">
                                        <td><%#Eval("MeetName") %></td>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("Id") %>' />
                                        <td><%#Eval("AreaName") %></td>
                                        <td><%#Eval("Name") %></td>
                                        <td><%#Eval("ContactPersonName") %></td>
                                        <td><%#Eval("MobileNo") %></td>


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

