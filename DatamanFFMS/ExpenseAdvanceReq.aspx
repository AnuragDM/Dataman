<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ExpenseAdvanceReq.aspx.cs" Inherits="AstralFFMS.ExpenseAdvanceReq" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
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
            margin-top: -8px !important;
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
    <script type="text/javascript">
        function validate() {

            if ($('#<%=EffDateFrom.ClientID%>').val() == "") {
                errormessage("Please select From Date.");
                return false;
            }

            if ($('#<%=txtFrTime.ClientID%>').val() == '') {
                errormessage("Please select From Time");
                return false;
            }

            if ($('#<%=EffDateTo.ClientID%>').val() == "") {
                errormessage("Please select To Date.");
                return false;
            }

            if ($('#<%=txtToTime.ClientID%>').val() == '') {
                errormessage("Please select To Time");
                return false;
            }

            if ($('#<%=txtAmount.ClientID%>').val() == "") {
                errormessage("Please enter Amount.");
                return false;
            }
            if ($('#<%=txtAmount.ClientID%>').val() <= 0) {
                errormessage("Please enter Amount greater than 0.");
                return false;
            }
            if ($('#<%=Remarks.ClientID%>').val() == "") {
                errormessage("Please enter Remarks.");
                return false;
            }

            if ($('#<%=lstState.ClientID%>').val() == "0") {
                errormessage("Please select State.");
                return false;
            }

            if ($('#<%=ListBox1.ClientID%>').val() == "0") {
                errormessage("Please select City to visit.");
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
        function DoNav(ID) {
            if (ID != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', ID)
            }
        }
    </script>

    <script type="text/javascript">
        function checkDate(sender, args) {

            var date = new Date(); // 2012-03-31
            date.setMonth(date.getMonth() - 1);
            if (sender._selectedDate < date) {
                errormessage("You cannot select a day less than one month from today!");
                sender._selectedDate = new Date();
                sender._textbox.set_Value("")
            }
        }
    </script>

    <script type="text/javascript">
        $(function () {
            $('[id*=ListBox1]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '290px',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 290,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {

            $('#ContentPlaceHolder1_txtFrTime').timepicker();
            $('#ContentPlaceHolder1_txtToTime').timepicker();

            //$("#ContentPlaceHolder1_basicExample").val("10:00am");
            //$("#ContentPlaceHolder1_basicExample1").val("6:00pm");

        });
    </script>

    <script type="text/javascript">
        $(function () {
            $('[id*=lstState]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '284px',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 284,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
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
                           <%-- <h3 class="box-title">Expense Advance Request</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                    OnClick="btnFind_Click" />

                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack1" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="">
                                <div class="col-md-2 paddingright0 form-group">
                                    <label for="exampleInputEmail1">From Date:</label>
                                    &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:TextBox ID="EffDateFrom" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox3_CalendarExtender"
                                        TargetControlID="EffDateFrom"></ajaxToolkit:CalendarExtender>
                                </div>
                                <div class="col-md-2 paddingright0 form-group">
                                    <label for="exampleInputEmail1">From Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input type="text" data-scroll-default="6:00am" maxlength="7" width="60%" placeholder="--Select Time--" class="form-control" id="txtFrTime" runat="server"
                                        autocomplete="off">
                                </div>

                                <div class="col-md-8 paddingright0 form-group">
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="">
                                <div class="col-md-2 paddingright0 form-group">
                                    <label for="exampleInputEmail1">To Date:</label>
                                    &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:TextBox ID="EffDateTo" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox4_CalendarExtender"
                                        TargetControlID="EffDateTo"></ajaxToolkit:CalendarExtender>

                                </div>
                                <div class="col-md-2 paddingright0 form-group">
                                    <label for="exampleInputEmail1">To Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input type="text" data-scroll-default="6:00am" maxlength="7" width="60%" placeholder="--Select Time--" class="form-control" id="txtToTime" runat="server" autocomplete="off">
                                </div>
                                <div class="col-md-8 paddingright0 form-group">
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="">
                                <div class="col-md-4 paddingright0 form-group">
                                    <label for="exampleInputEmail1">Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:TextBox ID="txtAmount" runat="server" MaxLength="8" class="form-control numeric text-right"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="flt_TB_txtAmount" runat="server" FilterType="Numbers,Custom" ValidChars="."
                                        TargetControlID="txtAmount" />
                                </div>
                                <div class="col-md-8 paddingright0 form-group">
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="">
                                <div class="col-md-4 paddingright0 form-group">
                                    <label for="exampleInputEmail1">Remark:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <textarea id="Remarks" runat="server" style="height: 50px;" placeholder="Enter Remarks" tabindex="7" class="form-control" maxlength="100"></textarea>
                                </div>
                                <div class="col-md-8 paddingright0 form-group">
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="" id="CityandState" runat="server" visible="false">
                                <div class="col-md-4 paddingright0 form-group">
                                    <div class="table table-responsive">
                                        <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" class="table table-bordered table-striped">
                                            <Columns>
                                                <asp:BoundField HeaderText="State" DataField="State" />
                                                <asp:BoundField HeaderText="City" DataField="City" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div class="" id="VisitState" runat="server">
                                <div class="col-md-4 paddingright0 form-group">
                                    <label for="exampleInputEmail1">State:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <%--<asp:DropDownList ID="ddlState" Width="100%" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged"></asp:DropDownList>--%>
                                    <asp:ListBox ID="lstState" runat="server" class="form-control" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="lstState_SelectedIndexChanged"></asp:ListBox>

                                </div>
                                <div class="col-md-8 paddingright0 form-group">
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="" id="VisitCity" runat="server">
                                <div class="col-md-4 paddingright0 form-group">
                                    <label for="exampleInputEmail1">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:ListBox ID="ListBox1"  runat="server" class="form-control" SelectionMode="Multiple"></asp:ListBox>

                                </div>
                                <div class="col-md-8 paddingright0 form-group">
                                </div>
                            </div>

                            <div class="clearfix" id="Approve" runat="server" visible="false">
                                <div class="clearfix">
                                    <div class="col-md-12 paddingright0 form-group">
                                        <h4>Advance Request Approval</h4>
                                    </div>
                                </div>
                                <div class="clearfix">

                                    <div class="col-md-4 paddingright0 form-group">

                                        <label for="exampleInputEmail1">Approved Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtAppAmt" runat="server" MaxLength="8" class="form-control numeric text-right"></asp:TextBox>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers,Custom" ValidChars="."
                                            TargetControlID="txtAppAmt" />
                                    </div>
                                    <div class="col-md-8 paddingright0 form-group">
                                    </div>
                                </div>
                                <div class="clearfix">
                                    <div class="col-md-4 paddingright0 form-group">
                                        <asp:Button ID="btnApprove" runat="server" Text="Approve" Style="margin-right: 5px;" type="button" class="btn btn-primary" OnClick="btnApprove_Click2" />
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnCanelApp" runat="server" Text="Cancel" class="btn btn-primary" TabIndex="30" OnClick="btnCanelApp_Click" />
                                    </div>
                                    <div class="col-md-8 paddingright0 form-group">
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClientClick="javascript:return validate();" TabIndex="28" OnClick="btnSave_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" TabIndex="30" OnClick="btnCancel_Click" />
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
                            <h3 class="box-title">Expense Advance Request Details</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />

                            </div>
                        </div>

                        <div class="box-body">
                            <div class="col-md-12">
                                <div class="col-md-12 paddingleft0">
                                    <div id="DIV1" class="form-group col-md-3">
                                        <label for="exampleInputEmail1">From Date:</label>
                                        <asp:TextBox ID="txtmDate" runat="server" CssClass="form-control" BackColor="White"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtmDate" runat="server" />
                                    </div>
                                    <div class="form-group col-md-3">
                                        <label for="exampleInputEmail1">To:</label>
                                        <asp:TextBox ID="txttodate" runat="server" CssClass="form-control" BackColor="White"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                    </div>
                                    <div class="form-group col-md-3">
                                        <label for="exampleInputEmail1">Status:</label>
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="100%" CssClass="form-control">
                                            <%-- <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>--%>
                                            <asp:ListItem Text="Pending" Value="Pending" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-md-3">
                                        <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                        <asp:Button type="button" ID="btnGo" runat="server" Text="Go" Style="padding: 3px 7px;" class="btn btn-primary" OnClick="btnGo_Click" />
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

                                                <th>DocID</th>
                                                <th>Employee Name</th>
                                                <th>Travel From Date</th>
                                                <th>Travel To Date</th>
                                                <th>Status</th>
                                                <th style="text-align: right;">Amount</th>
                                                <th style="text-align: right;">Approved Amount</th>
                                                <th style="display: none;">State</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("ID") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("ID") %>' />
                                        <td><%#Eval("DocId") %></td>
                                        <td><%#Eval("EmpName") %></td>
                                        <td><%#Eval("FromDate","{0:dd/MMM/yyyy}") %></td>
                                        <td><%#Eval("ToDate","{0:dd/MMM/yyyy}") %></td>
                                        <td><%#Eval("RecStatus") %></td>
                                        <td style="text-align: right;"><%#Eval("Amount") %></td>
                                        <td style="text-align: right;"><%#Eval("ApprovedAmt") %></td>
                                        <td style="display: none;"><%#Eval("StateName") %></td>

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
