<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true"  EnableEventValidation="false" CodeBehind="LeaveRequest.aspx.cs" Inherits="AstralFFMS.LeaveRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <%--<script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <script src="plugins/jquery.numeric.min.js"></script>
    <!-- SlimScroll -->
    <%-- decimal: false, --%>
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".numeric").numeric({ negative: false });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
                "order": [[0, "desc"]]
            });
        });
    </script>
    <script type="text/javascript">
        function myFunction() {
            //      alert();
            document.getElementById('<%=conditonaldiv.ClientID%>').style.display = "block";
            $('#div1 :input').attr('disabled', true);
            $('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', true);
        }
    </script>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>
    <style type="text/css">
        .spinner {
            position: absolute;
            top: 50%;
            left: 50%;
            margin-left: -50px; /* half width of the spinner gif */
            margin-top: -50px; /* half height of the spinner gif */
            text-align: center;
            z-index: 999;
            overflow: hidden;
            width: 180px; /* width of the spinner gif */
            height: 100px; /*hight of the spinner gif +2px to fix IE8 issue */
        }

        .form-control-custom {
            padding: 3px 12px;
            background: #fcfcfd none repeat scroll 0 0;
            border: 1px solid #bdc3d1;
            border-radius: 3px !important;
            height: 28px;
            border-radius: 0 !important;
            box-shadow: none;
            border-radius: 4px;
            color: #555;
            display: block;
            font-size: 14px;
            line-height: 1.42857;
            transition: border-color 0.15s ease-in-out 0s, box-shadow 0.15s ease-in-out 0s;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            /*display: table;*/
        }

        #ContentPlaceHolder1_approveStatusRadioButtonList td {
            padding: 3px;
        }

        #ContentPlaceHolder1_txttodate, #ContentPlaceHolder1_txtfmDate {
            padding: 3px 12px;
            height: 30px;
        }

        #ContentPlaceHolder1_btnGo, #ContentPlaceHolder1_btnCancel1 {
            height: 30px;
            padding: 1px 15px;
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
        $(document).ready(function () {
            //     $('#div1 :input').attr('disabled', false);
            var userID = $('#<%=userIDHiddenField.ClientID%>').val();
            var LVRQId = $('#<%=HiddenField1.ClientID%>').val();
            var leaveData = $('#<%=chkLeaveDataHdf.ClientID%>').val();
            var IsCurrentUser = $('#<%=IsCurrUserHiddenField.ClientID%>').val();
            var appStatus = $('#<%=appStatusHiddenField.ClientID%>').val();
            if (userID != "" && LVRQId != "") {
                if (leaveData == "1") {
                    if (IsCurrentUser == "1") {
                        document.getElementById('<%=conditonaldiv.ClientID%>').style.display = "block";
                        $('#div1 :input').attr('disabled', true); $('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', true);
                        document.getElementById('btnSave').disabled = true;
                    }
                    else {
                        if (appStatus == "Approve" || appStatus == "Reject") {
                            document.getElementById('<%=conditonaldiv.ClientID%>').style.display = "block";
                            $('#div1 :input').attr('disabled', true);
                            $('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', true);
                        }
                        else {
                            document.getElementById('<%=conditonaldiv.ClientID%>').style.display = "block";
                            $('#div1 :input').attr('disabled', true);
                            $('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', false);
                            document.getElementById('btnSave').disabled = true;
                        }
                    }
                }
                else {
                    errormessage("This request entry has been deleted by user.");
                }
            }
        });
    </script>

    <script type="text/javascript">
        function validate() {
            if ($('#<%=NoOfDays.ClientID%>').val() == "" && $('#<%=NoOfDays.ClientID%>').val() != "0") {
                errormessage("Please enter the No Of Days.");
                return false;
            }
            <%--if ($('#<%=calendarTextBox.ClientID%>').val() == "") {
                errormessage("Please select the Date.");
                return false;
            }
            if ($('#<%=Reason.ClientID%>').val() == "") {
                errormessage("Please enter the reason.");
                return false;
            }--%>
        }
    </script>
    <script type="text/javascript">
        function myFunctionNew() {
            //      alert();
            document.getElementById('<%=conditonaldiv.ClientID%>').style.display = "block";
            $('#div1 :input').attr('disabled', true);
            $('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', false);
        }
    </script>

     
    <script type="text/javascript">
        function DoNav(lvrQId) {
            if (lvrQId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', lvrQId)
            }
        }
    </script>
    <script>
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }
    </script>
    <script>
        function isNumber1(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (!(iKeyCode != 8))
                e.preventDefault();
            return false;
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
        <div class="box-body" id="mainDiv" style="display: none;" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                               <%-- <h3 class="box-title">Leave Request Entry</h3>--%>
                                <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                                <div style="float: right">
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                        OnClick="btnFind_Click" />
                                    <asp:HiddenField ID="appStatusHiddenField" runat="server" />
                                </div>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                           
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-md-10 col-sm-11 col-xs-11">
                                                    <label for="exampleInputEmail1">Application Date:</label>&nbsp;&nbsp;
                                        <asp:Label ID="lblappdate" runat="server"></asp:Label>&nbsp;&nbsp;
                                   <label for="exampleInputEmail1">Applied By:</label>&nbsp;&nbsp;
                                  <asp:Label ID="lblusername" runat="server"></asp:Label>&nbsp;&nbsp;
                                 <label for="exampleInputEmail1">Report To:</label>&nbsp;&nbsp;
                                  <asp:Label ID="lblapprby" runat="server"></asp:Label>&nbsp;&nbsp;
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-5 col-md-7 col-sm-9 col-xs-9">

                                                <div class="row">
                                                    <div class="col-md-12 col-sm-12">
                                                        <div class="form-group">
                                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                            <asp:DropDownList ID="DdlSalesPerson" Width="100%" OnSelectedIndexChanged="DdlSalesPerson_SelectedIndexChanged" AutoPostBack="true"
                                                                CssClass="form-control" runat="server">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6 col-sm-6">
                                                        <div class="form-group">
                                                            <input id="LVRQID" hidden="hidden" />
                                                            <asp:HiddenField ID="userIDHiddenField" runat="server" />
                                                            <asp:HiddenField ID="HiddenField1" runat="server" />
                                                            <asp:HiddenField ID="IsCurrUserHiddenField" runat="server" />
                                                            <asp:HiddenField ID="chkLeaveDataHdf" runat="server" />
                                                            <label for="exampleInputEmail1">No. of Days:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                            <asp:TextBox ID="NoOfDays" class="form-control numeric text-right" MaxLength="4" runat="server" OnTextChanged="NoOfDays_TextChanged" AutoPostBack="true" placeholder="Enter no of days"></asp:TextBox>
                                                            <%--  <input type="text" class="form-control" maxlength="3" id="NoOfDays" runat="server" onkeypress="javascript:return isNumber (event)" placeholder="Enter no of days">--%>
                                                        </div>
                                                    </div>


                                                    <div class="col-md-4 col-sm-4">
                                                    </div>

                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6 col-sm-6">
                                                        <div class="form-group">
                                                            <label for="exampleInputEmail1">From Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                            <asp:TextBox ID="calendarTextBox" class="form-control" runat="server" OnTextChanged="calendarTextBox_TextChanged" AutoPostBack="true" Style="background-color: white;"></asp:TextBox>
                                                            <%--<asp:Image ID="imgCal" runat="server" Width="20%" Height="30%" ImageUrl="~/img/calendar.png"/>--%>
                                                            <ajaxToolkit:CalendarExtender ID="calendarTextBox_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender"
                                                                TargetControlID="calendarTextBox"></ajaxToolkit:CalendarExtender>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6">
                                                        <div class="form-group">
                                                            <label for="exampleInputEmail1" style="color: white;">Leave Period</label>
                                                            <asp:DropDownList ID="ddlLF" runat="server" class="form-control" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlLF_SelectedIndexChanged">
                                                                <%--<asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="F" Selected="True">First Half</asp:ListItem>
                                                                <asp:ListItem Value="S">Second Half</asp:ListItem>
                                                                <asp:ListItem Value="C">Full</asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6 col-sm-6">
                                                        <div class="form-group">
                                                            <label for="exampleInputEmail1">To Date:</label>
                                                         <%--   <input type="text" class="form-control" style="background: white;" maxlength="100" readonly="readonly" id="Reason1" runat="server">--%>
                                                            
                                                            <asp:TextBox ID="Reason1" class="form-control" runat="server" OnTextChanged="calendarTextBox_TextChanged" AutoPostBack="true" Style="background-color: white;"></asp:TextBox>
                                                        </div>

                                                    </div>
                                                    <div class="col-md-6 col-sm-6">
                                                        <div class="form-group">
                                                            <label for="exampleInputEmail1" style="color: white;">Leave Period</label>
                                                            <asp:DropDownList ID="ddlLF1" runat="server" class="form-control" Visible="false">
                                                                <%--  <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="F" Selected="True">First Half</asp:ListItem>
                                                                <asp:ListItem Value="S">Second Half</asp:ListItem>
                                                                 <asp:ListItem Value="C">Full</asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                         
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 col-sm-12">
                                                        <div class="form-group">
                                                            <label for="exampleInputEmail1">Reason:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                            <textarea id="Reason" class="form-control" runat="server" style="resize: none; height: 20%" cols="20" rows="2" maxlength="255" placeholder="Enter Reason"></textarea>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                   <%-- </ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </div>
                            <div class="row">
                                <div class="col-lg-5 col-md-7 col-sm-9 col-xs-9">
                                    <div class="">
                                        <div class="box-body" id="conditonaldiv" runat="server" style="display: none; background-color: none; border: 1px dotted gray;">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Status:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:RadioButtonList ID="approveStatusRadioButtonList" RepeatDirection="Horizontal" runat="server">
                                                    <asp:ListItem Selected="True" Value="Approve" Text="Approve"></asp:ListItem>
                                                    <asp:ListItem Value="Reject" Text="Reject"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <%--   <asp:DropDownList ID="ddlApproveStatus" CssClass="form-control select2" runat="server">
                                        <asp:ListItem Selected="True" Value="Approve" Text="Approve"></asp:ListItem>
                                        <asp:ListItem Value="Reject" Text="Reject"></asp:ListItem>
                                    </asp:DropDownList>--%>
                                            </div>
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Remark:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" Style="height: 20%;" placeholder="Enter Remark" TextMode="MultiLine"></asp:TextBox>
                                                <%-- <textarea id="TextArea1" class="form-control" runat="server" style="resize: none" cols="20" rows="2" placeholder="Enter Remark"></textarea>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                           
                        </div>
                         <div class="row" id="RejectionDiv" runat="server" style="display: none;">
                                <div class="col-lg-5 col-md-7 col-sm-9 col-xs-9">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Rejection Remark:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtRejectionRemark" runat="server" CssClass="form-control" Style="height: 20%;" placeholder="Enter Rejection Remark" TextMode="MultiLine"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary"
                                OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                OnClick="btnCancel_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary"
                                OnClientClick="Confirm()" OnClick="btnDelete_Click" />
                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
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
                            <h3 class="box-title">Leave List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary"
                                    OnClick="btnBack_Click" />
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="row">
                                <div class="col-md-3 col-sm-6 col-xs-12">
                                    <div id="DIV1" class="form-group">
                                        <label for="exampleInputEmail1">From Date:</label>
                                        <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">To Date:</label>
                                        <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label for="complaintNature">Sales Person:</label>
                                        <asp:DropDownList ID="DropDownList1" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail11" style="visibility: hidden; display: block">From Date:</label>
                                        <asp:Button type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClick="btnGo_Click" />
                                        <asp:Button type="button" ID="btnCancel1" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel1_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Application Date</th>
                                                <th>Document No.</th>
                                                <th>From Date</th>
                                                <th>To Date</th>
                                                <th>Sales Person</th>
                                                <th>Days</th>
                                                <th>Reason</th>
                                                <th>Remark</th>
                                                <th>Status</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("LVRQId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("LVRQId") %>' />
                                        <td><%#Eval("VDate","{0:dd/MMM/yyyy}") %></td>
                                        <td><%#Eval("LVRDocId") %></td>
                                        <td><%#Eval("FromDate","{0:dd/MMM/yyyy}") %></td>
                                        <td><%#Eval("ToDate","{0:dd/MMM/yyyy}") %></td>
                                        <td><%#Eval("SMName") %></td>
                                        <%--<td><%#Convert.ToInt32(Eval("NoOfDays")) %></td>--%>
                                        <td><%#Eval("NoOfDays") %></td>
                                        <td><%#Eval("Reason") %></td>
                                        <td><%#Eval("AppRemark") %></td>
                                        <td><%#Eval("AppStatus") %></td>
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
</asp:Content>

