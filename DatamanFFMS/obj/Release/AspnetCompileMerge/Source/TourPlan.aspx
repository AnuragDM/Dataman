<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="TourPlan.aspx.cs" Inherits="AstralFFMS.TourPlan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
                "order": [[0, "desc"]]
            });
        });
    </script>

    <script type="text/javascript">
        function pageLoad() {
            $('[id*=ddlArea]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=ddlDistributor]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=ddlPurposeVisit]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        };
    </script>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>
    <style type="text/css">
        .multiselect.dropdown-toggle {
            white-space: normal !important;
        }
        .multiselect-container.dropdown-menu {
        width: 100% !important;
        
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

        .completionList {
            border: solid 1px Gray;
            margin: 0px;
            padding: 3px;
            overflow: auto;
            overflow-y: scroll;
            background-color: #FFFFFF;
            max-height: 180px;
        }

        .listItem {
            color: #191919;
        }

        .itemHighlighted {
            background-color: #ADD6FF;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            display: table;
        }

        #ContentPlaceHolder1_approveStatusRadioButtonList td {
            padding: 3px;
        }

        #ContentPlaceHolder1_accStaffCheckBoxList td {
            padding: 3px;
        }

        #ContentPlaceHolder1_GridView1 th {
            text-align: center;
        }

        label {
            padding: 3px;
        }

        .containerStaff {
            border: 1px solid #ccc;
            max-height: 200px;
            overflow-y: scroll;
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
        $(document).ready(function () {
            //     $('#div1 :input').attr('disabled', false);
            var userID = $('#<%=userIDHiddenField.ClientID%>').val();
            var DocID = $('#<%=HiddenField1.ClientID%>').val();
            var tourData = $('#<%=chkTourDataHdf.ClientID%>').val();
            var IsCurrentUser = $('#<%=IsCurrUserHiddenField.ClientID%>').val();
            var appStatus = $('#<%=appStatusHiddenField.ClientID%>').val();
            if (userID != "" && DocID != "") {
                if (tourData == "1") {
                    if (IsCurrentUser == "1") {
                        document.getElementById('<%=conditonaldiv.ClientID%>').style.display = "block";
                        $('#div1 :input').attr('disabled', true);
                        $('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', true);
                        //document.getElementById("btnSubmit").style.visibility = "none";
                        //document.getElementById('btnSave').disabled = true;
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
                            //Added Nishu 02/03/2016
                            $('#Div2 :input').attr('disabled', false);
                            //
                            $('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', false);
                            document.getElementById("btnSubmit").style.visibility = "visible";
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
        function myFunction() {
            //      alert();
            document.getElementById('<%=conditonaldiv.ClientID%>').style.display = "block";
            $('#div1 :input').attr('disabled', true);
            $('#ContentPlaceHolder1_conditonaldiv :input').attr('disabled', true);
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
            if ($('#<%=calendarTextBox.ClientID%>').val() == "") {
                errormessage("Please select From Date.");
                return false;
            }
            if ($('#<%=toCalendarTextBox.ClientID%>').val() == "") {
                errormessage("Please select To Date.");
                return false;
            }
           <%-- if ($('#<%=DDLPurposeVisit.ClientID%>').val() == "0") {
                errormessage("Please select Visit Purpose.");
                return false;
            }
            if ($('#<%=DdlAreaName.ClientID%>').val() == "0") {
                errormessage("Please select the City Name.");
                return false;
            }
            if ($('#<%=txtDist.ClientID%>').val() != "") {
                if ($('#<%=hfDistId.ClientID%>').val() == "") {
                    errormessage("Please enter valid distributor");
                    return false;
                }
            }--%>
        }


    </script>
    <script type="text/javascript">
        function validatOnSubmit() {
            if ($('#<%=calendarTextBox.ClientID%>').val() == "") {
                errormessage("Please select From Date.");
                return false;
            }
            if ($('#<%=toCalendarTextBox.ClientID%>').val() == "") {
                errormessage("Please select To Date.");
                return false;
            }
            <%--if ($('#<%=TextArea1.ClientID%>').val() == "") {
                errormessage("Please enter remark.");
                return false;
            }--%>
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
   <%-- <script type="text/javascript">
        function SetContextKey() {
            $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey($get("<%=DdlAreaName.ClientID %>").value);
        }
    </script>--%>
   <%-- <script type="text/javascript">
        function ClientPartySelected(sender, e) {
            $get("<%=hfDistId.ClientID %>").value = e.get_value();
        }
    </script>--%>
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
    <script>
        function isNumber1(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (!(iKeyCode != 8))
                e.preventDefault();
            return false;
            return true;
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
                                <h3 class="box-title">Tour Plan</h3>
                                <div style="float: right">
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                        OnClick="btnFind_Click" />
                                    <asp:HiddenField ID="Hdate" runat="server" />
                                    <asp:HiddenField ID="chkTourDataHdf" runat="server" />
                                    <asp:HiddenField ID="checkTourDataHdf" runat="server" />
                                    <asp:HiddenField ID="appStatusHiddenField" runat="server" />
                                </div>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="row">
                                                <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                                    <div class="col-md-12 col-sm-12">

                                                        <div class="form-group">
                                                            <label for="exampleInputEmail1">Sales Person:</label>
                                                            <asp:DropDownList ID="DdlSalesPerson" Width="100%" CssClass="form-control" runat="server" OnSelectedIndexChanged="DdlSalesPerson_SelectedIndexChanged" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </div>

                                                    </div>
                                                </div>
                                                </div>
                                                <div class="clearfix"></div>
                                                
                                                <div class="row">
                                                    
                                                   <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                                    <div class="col-md-6 col-sm-6">
                                                        <div class="form-group">
                                                            <input id="TourPlanId" hidden="hidden" />
                                                            <label for="exampleInputEmail1">From Date:</label><label for="requiredFields" style="color: red;">*</label>
                                                            <asp:HiddenField ID="userIDHiddenField" runat="server" />
                                                            <asp:HiddenField ID="HiddenField1" runat="server" />
                                                            <asp:HiddenField ID="IsCurrUserHiddenField" runat="server" />
                                                            <asp:TextBox ID="calendarTextBox" class="form-control" runat="server"
                                                                Style="background-color: white;"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="calendarTextBox_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server"
                                                                BehaviorID="calendarTextBox_CalendarExtender"
                                                                TargetControlID="calendarTextBox"></ajaxToolkit:CalendarExtender>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6">
                                                        <div class="form-group">
                                                            <label for="exampleInputEmail1">To Date:</label><label for="requiredFields" style="color: red;">*</label>
                                                            <asp:TextBox ID="toCalendarTextBox" class="form-control" runat="server"
                                                                Style="background-color: white;"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" runat="server"
                                                                BehaviorID="CalendarExtender2"
                                                                TargetControlID="toCalendarTextBox"></ajaxToolkit:CalendarExtender>
                                                        </div>
                                                        <div class="form-group" hidden>
                                                            <label for="exampleInputEmail1">Purpose Of Visit:</label><label for="requiredFields" style="color: red;">*</label>
                                                            <asp:DropDownList ID="DDLPurposeVisit" Width="100%" CssClass="form-control" runat="server">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                     
                                                    <div class="col-md-6 col-sm-6">
                                                        <div class="form-group">
                                                            <asp:Button Style="margin-right: 5px; display: block;" type="button" ID="AddNewTour" runat="server" Text="Add Tour Plan" class="btn btn-primary" OnClick="AddNewTour_Click" OnClientClick="javascript:return validatOnSubmit();" />
                                                        </div>
                                                    </div>
                                            </div>
                                                    <div class="clearfix"></div>

                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box-body" id="Div2">

                                                                    <div class="table gvdataRes ">

                                                                        <asp:GridView ID="GridView1" runat="server" CssClass="gridtext" Width="100%" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound" HeaderStyle-Height="39px"
                                                                            HeaderStyle-VerticalAlign="Middle" HeaderStyle-BackColor="#3c8dbc" HeaderStyle-ForeColor="White">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="visible-lg visible-md" ItemStyle-Width="14%" ItemStyle-Wrap="true">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="tourPlanHdf" runat="server" Value='<%#Eval("TourPlanId") %>' />
                                                                                         <%--<asp:HiddenField ID="hiddocvid" runat="server" Value='<%#Eval("docid") %>' />--%>
                                                                                        <asp:Label ID="lblDat1" runat="server" Visible="false" Text='<%# string.Format("{0:dd/MMM/yyyy}", Eval("VDate"))%>'></asp:Label>
                                                                                        <div style="background: #3c8dbc; color: #fff; padding: 5px; text-align: center" class="hidden-lg hidden-md"><%# string.Format("{0:dd/MMM/yyyy}", Eval("VDate"))%></div>

                                                                                        <asp:Label ID="Label1" runat="server" CssClass="text-center visible-lg visible-md" Text='<%# string.Format("{0:dd/MMM/yyyy}", Eval("VDate"))%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="City *" HeaderStyle-CssClass="visible-lg visible-md" ItemStyle-Width="24%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCity" runat="server" Visible="false" Text='<%# Eval("MCityId")%>'></asp:Label>
                                                                                        <div style="background: #3c8dbc; color: #fff; padding: 5px; text-align: center" class="hidden-lg hidden-md">City</div>
                                                                                        <asp:ListBox ID="ddlArea" runat="server" SelectionMode="Multiple"  AutoPostBack="true" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged"></asp:ListBox>
                                                                                        <%--<asp:DropDownList ID="ddlArea" CssClass="form-control" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged">
                                                                                                </asp:DropDownList>--%>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Distributor Name" HeaderStyle-CssClass="visible-lg visible-md" ItemStyle-Width="24%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDist" runat="server" Visible="false" Text='<%# Eval("MDistId")%>'></asp:Label>
                                                                                        <div style="background: #3c8dbc; color: #fff; padding: 5px; text-align: center" class="hidden-lg hidden-md">Distributor Name</div>
                                                                                        <asp:ListBox ID="ddlDistributor" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Purpose Of Visit *" HeaderStyle-CssClass="visible-lg visible-md" ItemStyle-Width="24%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblPurp" runat="server" Visible="false" Text='<%# Eval("MPurposeId")%>'></asp:Label>
                                                                                        <div style="background: #3c8dbc; color: #fff; padding: 5px; text-align: center" class="hidden-lg hidden-md">Purpose Of Visit</div>
                                                                                        <asp:ListBox ID="ddlPurposeVisit" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Remarks" HeaderStyle-CssClass="visible-lg visible-md" ItemStyle-Width="24%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblRemark" runat="server" Visible="false" Text='<%# Eval("Remarks")%>'></asp:Label>
                                                                                        <div style="background: #3c8dbc; color: #fff; padding: 5px; text-align: center" class="hidden-lg hidden-md">Remarks</div>
                                                                                        <asp:TextBox ID="remarkTextBox" TextMode="MultiLine" Style="resize: none; width:100%;height: 100%;" cols="20" Rows="2" placeholder="Enter Remark" runat="server"></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div id="fRemark"  runat="server" style="display:none;">
                                                            <div class="col-md-12 col-sm-12">
                                                                <div class="form-group">
                                                                    <label for="exampleInputEmail1">Remark:</label>
                                                                    <textarea id="finalRemarkTextarea" class="form-control" runat="server" style="resize: none; height: 100%;width:100%;" cols="20" rows="2" maxlength="255" placeholder="Enter Final Remark"></textarea>
                                                                    </div>
                                                            </div>
                                                        </div>

                                                    
                                                </div>
                                                <div class="row" hidden>
                                                    
                                                </div>
                                                <div class="row" hidden>
                                                    
                                                </div>
                                                <div class="row" hidden>
                                                  
                                                </div>
                                                <div class="row" hidden>
                                                   
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                        <div class="box-body" id="conditonaldiv" runat="server" style="display: none; background-color: none; border: 1px dotted gray;">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Status:</label><label for="requiredFields" style="color: red;">*</label>
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
                                                <label for="exampleInputEmail1">Remark:</label><label for="requiredFields" style="color: red;">*</label>
                                                <textarea id="TextArea1" class="form-control" runat="server"
                                                    style="resize: none; height: 20%;" cols="20" rows="2" placeholder="Enter Remark"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="box-footer">
                                <asp:Button Style="margin-right: 5px; display: none;" type="button" ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary"
                                    OnClick="btnSubmit_Click" OnClientClick="javascript:return validatOnSubmit();" />
                                <asp:Button Style="margin-right: 5px; display: none;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary"
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
                            <h3 class="box-title">Tour Plan List</h3>
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
                                                <th>Date</th>
                                                <th>Document No.</th>
                                              <%--  <th>Sales Person</th>
                                                <th>City Name</th>
                                                <th>Purpose Of Visit</th>--%>
                                                 <th>Sales Person</th>
                                                <th>Status</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%-- <tr onclick="DoNav('<%#Eval("TourPlanId") %>');">--%>
                                     <tr onclick="DoNav('<%#Eval("TourPlanHId") %>');">
                                        <%--<asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("TourPlanId") %>' />--%>
                                         <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("TourPlanHId") %>' />
                                        <td><%#Eval("VDate","{0:dd/MMM/yyyy}") %></td>
                                        <td><%#Eval("DocId") %></td>
                                       <%-- <td><%#Eval("SMName") %></td>
                                        <td><%#Eval("AreaName") %></td>
                                        <td><%#Eval("PurposeName") %></td>--%>
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

