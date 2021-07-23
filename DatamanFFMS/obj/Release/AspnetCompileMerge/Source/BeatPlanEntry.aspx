<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="BeatPlanEntry.aspx.cs" Inherits="AstralFFMS.BeatPlanEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
  <%--  <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style type="text/css">
        #ContentPlaceHolder1_approveStatusRadioButtonList_0 {
            margin-right: 5px !important;
            margin-top: 3px;
        }

        #ContentPlaceHolder1_approveStatusRadioButtonList_1 {
            margin-right: 5px !important;
            margin-top: 3px;
        }

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

        #ContentPlaceHolder1_GridView1 th {
            text-align: center;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .gridtext {
            white-space: nowrap;
        }

            .gridtext td {
                white-space: nowrap;
            }

            .gridtext th {
                white-space: nowrap;
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


        @media (max-width: 980px) {
            .gvdataRes {
                padding: 5px;
            }

            body .gvdataRes td, body .gvdataRes th {
                display: block;
                width: 100% !important;
            }

            body .select2-container .select2-selection--single, body .select2-selection__rendered {
                white-space: normal !important;
            }

            body .select2-container .select2-selection--single {
                height: auto !important;
            }

            body .gvdataRes tr {
                border-bottom: 10px solid #fff;
                border-right: 1px solid #fff;
                border-left: 1px solid #fff;
            }
        }
    </style>
    <script type="text/javascript">
        function myFunction() {
            //      alert();
            document.getElementById('<%=conditonaldiv.ClientID%>').style.display = "block";
            $('#Div1 :input').attr('disabled', true);
            $('#Div2 :input').attr('disabled', true);
            $('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', true);
        }
    </script>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
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
    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
                "order": [[1, "desc"]]
            });
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.demo').hide();
            $('.demo1').attr("disabled", false);
            var userID = $('#<%=userIDHiddenField.ClientID%>').val();
            var docId = $('#<%=docIDHiddenField.ClientID%>').val();
            var IsCurrentUser = $('#<%=IsCurrUserHiddenField.ClientID%>').val();
            var appStatus = $('#<%=appStatusHiddenField.ClientID%>').val();
            var datepresent = $('#<%=datepresentHiddenField.ClientID%>').val();
            var showcancelbtn = $('#<%=cancelBtnHiddenField.ClientID%>').val();
            if (showcancelbtn == "1") {
                $('.democancel').show();
            }
            var beatData = $('#<%=chkBeatDataHdf.ClientID%>').val();
            if (datepresent == "1" && appStatus == "Reject") {
               
                $('.democancel').show();
                $('#Div1 :input').attr('disabled', true);
                $('#Div2 :input').attr('disabled', true); $('.demo').hide();
                $('.demo1').show(); $('.demo1').attr("disabled", true);
                document.getElementById('<%=conditonaldiv.ClientID%>').style.display = "block";
                $('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', true);
            }
            if (datepresent == "1" && appStatus == "Approve") {
              
                $('.democancel').show();
                $('#Div1 :input').attr('disabled', true);
                $('#Div2 :input').attr('disabled', true); $('.demo').hide();
                $('.demo1').show(); $('.demo1').attr("disabled", true);
                document.getElementById('<%=conditonaldiv.ClientID%>').style.display = "block";
                $('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', true);
            }

            if (userID != "" && docId != "") {
               
                if (beatData == "1") {
                   
                    if (IsCurrentUser == "1") {
                        if (appStatus == "Approve" || appStatus == "Reject") {
                           
                            $('.democancel').show();
                            $('#Div1 :input').attr('disabled', true);
                            $('#Div2 :input').attr('disabled', true); $('.demo').hide();
                            $('.demo1').show(); $('.demo1').attr("disabled", true);
                            document.getElementById('<%=conditonaldiv.ClientID%>').style.display = "block";
                            $('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', true);
                        }
                        else {
                            $('.democancel').show();
                            $('#Div1 :input').attr('disabled', true); $('#Div2 :input').attr('disabled', true); $('.demo').show();
                            $('.demo1').hide(); $('.demo1').attr("disabled", true); $('.demo').attr("disabled", true);
                            $('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', true);
                            document.getElementById('<%=conditonaldiv.ClientID%>').style.display = "block";
                        }
                    }
                    else {
                        if (appStatus == "Approve" || appStatus == "Reject") {
                          
                            $('.democancel').show();
                            document.getElementById('<%=conditonaldiv.ClientID%>').style.display = "block";
                            $('#Div1 :input').attr('disabled', true); $('.demo').show(); $('.demo1').hide();
                            $('#Div2 :input').attr('disabled', true);
                            $('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', true)
                        }
                        else {
                            
                            $('.democancel').show();
                            document.getElementById('<%=conditonaldiv.ClientID%>').style.display = "block";
                            $('#Div1 :input').attr('disabled', true); $('.demo').show(); $('.demo1').hide();
                            $('#Div2 :input').attr('disabled', false);
                            $('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', false)
                        }

                    }
                }
                else {
                    errormessage("This request entry has been deleted by user.");
                }
            }
        });
    </script>
    <%-- <script type="text/javascript">
        $(function () {
            //Initialize Select2 Elements
            $(".select2").select2();
        });
    </script>--%>

    <script type="text/javascript">
        var V1 = "";
        function errormessage(Msg) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(Msg);
            $("#messageNotification").jqxNotification("open");

        }

        function errormessage2() {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html("Beat Plan can be planned starting from Monday!");
            $("#messageNotification").jqxNotification("open");

        }

        var V1 = "";
        function errormessageNew(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

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
        function errormessage1() {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html("Please Fill Date.");
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <%-- <script type="text/javascript">
        function Successmessage() {
            $("#messageNotification").jqxNotification({
                width: 250, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html("Record Inserted Successfully");
            $("#messageNotification").jqxNotification("open");

        }
    </script>--%>
    <script type="text/javascript">
        function validatOnSubmit() {
            if ($('#<%=RemarkArea.ClientID%>').val() == "") {
                errormessageNew("Please enter remark.");
                return false;
            }
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
                <asp:HiddenField ID="chkBeatDataHdf" runat="server" />
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
                                <h3 class="box-title">Beat Plan Entry</h3>
                                <div style="float: right">
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                        OnClick="btnFind_Click" />
                                </div>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body">
                                <div class="col-md-4 col-sm-6">
                                    <div class="row" id="Div1">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="DdlSalesPerson" Width="80%" CssClass="form-control" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <asp:HiddenField ID="Hdate" runat="server" />
                                            <label for="exampleInputEmail1">Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>&nbsp;&nbsp;
                                <table>
                                    <tr>
                                        <td>
                                            <%--  <div id="dateTimeInput"></div>--%>
                                            <asp:TextBox ID="calendarTextBox" class="form-control" Width="100%" runat="server"
                                                Style="background-color: white;"></asp:TextBox>
                                            <%--<asp:Image ID="imgCal" runat="server" Width="20%" Height="30%" ImageUrl="~/img/calendar.png"/>--%>
                                            <ajaxToolkit:CalendarExtender ID="calendarTextBox_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender"
                                                TargetControlID="calendarTextBox"></ajaxToolkit:CalendarExtender>
                                            <asp:HiddenField ID="userIDHiddenField" runat="server" />
                                            <asp:HiddenField ID="docIDHiddenField" runat="server" />
                                            <asp:HiddenField ID="startDateHiddenField" runat="server" />
                                            <asp:HiddenField ID="IsCurrUserHiddenField" runat="server" />
                                            <asp:HiddenField ID="appStatusHiddenField" runat="server" />
                                            <asp:HiddenField ID="datepresentHiddenField" runat="server" />
                                            <asp:HiddenField ID="cancelBtnHiddenField" runat="server" />
                                        </td>
                                        <td>&nbsp;&nbsp;
                                            <asp:Button ID="fill" runat="server" OnClick="fill_Click" class="btn btn-primary" Text="Fill" /></td>
                                    </tr>
                                </table>
                                        </div>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box-body" id="Div2">

                                                    <div class="table gvdataRes">

                                                        <asp:GridView ID="GridView1" runat="server" CssClass="gridtext" Width="100%" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound" HeaderStyle-Height="39px"
                                                            HeaderStyle-VerticalAlign="Middle" HeaderStyle-BackColor="#3c8dbc" HeaderStyle-ForeColor="White">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Day" HeaderStyle-CssClass="visible-lg visible-md" ItemStyle-Width="14%" ItemStyle-Wrap="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDat1" runat="server" Visible="false" Text='<%# string.Format("{0:dd/MMM/yyyy}", Eval("Date"))%>'></asp:Label>
                                                                        <div style="background: #3c8dbc; color: #fff; padding: 5px; text-align: center" class="hidden-lg hidden-md"><%# string.Format("{0:dd/MMM/yyyy - ddd }", Eval("Date"))%></div>

                                                                        <asp:Label ID="Label1" runat="server" CssClass="visible-lg visible-md" Text='<%# string.Format("{0:dd/MMM/yyyy - ddd }", Eval("Date"))%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <%-- <asp:TemplateField HeaderText="City Name" ItemStyle-Width="20%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCity" runat="server" Text='<%# Eval("CityName") %>' Visible="false" />
                                                                    <asp:DropDownList ID="ddlCity" CssClass="form-control select2" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="Area Name" HeaderStyle-CssClass="visible-lg visible-md" ItemStyle-Width="24%">
                                                                    <ItemTemplate>
                                                                     <%--   <asp:Label ID="lblArea" runat="server" Text='<%# Eval("AreaName") %>' Visible="false" />--%>
                                                                        <div style="background: #3c8dbc; color: #fff; padding: 5px; text-align: center" class="hidden-lg hidden-md">Area Name</div>
                                                                        <asp:DropDownList ID="ddlArea" CssClass="form-control" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Beat Name" HeaderStyle-CssClass="visible-lg visible-md" ItemStyle-Width="30%" ItemStyle-Wrap="true">
                                                                    <ItemStyle Width="120px" Wrap="true"></ItemStyle>
                                                                    <ItemTemplate>
                                                                     <%--   <asp:Label ID="lblBeat" runat="server" Text='<%# Eval("BeatName") %>' Visible="false" />--%>
                                                                        <div style="background: #3c8dbc; color: #fff; padding: 5px; text-align: center" class="hidden-lg hidden-md">Beat Name</div>
                                                                        <asp:DropDownList ID="ddlBeat" CssClass="form-control" Width="100%" OnDataBound="ddlBeat_DataBound" runat="server">
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>

                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="box-body" id="conditonaldiv" runat="server" style="display: none; background-color: none; border: 1px dotted gray;">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Status:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:RadioButtonList ID="approveStatusRadioButtonList" RepeatDirection="Horizontal" runat="server">
                                                    <asp:ListItem Selected="True" Value="Approve" Text="Approve"></asp:ListItem>
                                                    <asp:ListItem Value="Reject" Text="Reject"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <%-- <asp:DropDownList ID="ddlApproveStatus" CssClass="form-control select2" runat="server">
                                        <asp:ListItem Value="Approve" Text="Approve"></asp:ListItem>
                                        <asp:ListItem Value="Reject" Text="Reject"></asp:ListItem>

                                    </asp:DropDownList>--%>
                                            </div>
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Remark:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <textarea id="RemarkArea" class="form-control" style="resize: none; height: 40%;" cols="20" rows="2" placeholder="Enter Remark" runat="server"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box-footer">
                                    <asp:Button ID="Btnsave" runat="server" OnClick="Btnsave_Click" class="btn btn-primary demo1" Text="Save" />
                                    <asp:Button ID="BtnSubmit" runat="server" class="btn btn-primary demo" Text="Submit" OnClick="BtnSubmit_Click" OnClientClick="javascript:return validatOnSubmit();" />
                                    <asp:Button ID="BtnCancel" runat="server" class="btn btn-primary democancel" Text="Cancel" OnClick="BtnCancel_Click" />
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
        </div>
        <div class="box-body" id="rptmain" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Beat Plan List</h3>
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
                                                <th>Sr.No.</th>
                                                <th>Date</th>
                                                <th>Document No.</th>
                                                <th>Sales Person</th>
                                                <th>Status</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("DocId") %>');">
                                        <td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>
                                        <td><%#Eval("StartDate","{0:dd/MMM/yyyy}") %></td>
                                        <td><%#Eval("DocId") %></td>
                                        <td><%#Eval("SMName") %></td>
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

