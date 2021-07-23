<%@ Page Title="Lead/Inquiry" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="TransLeadInq.aspx.cs" Inherits="AstralFFMS.TransLeadInq" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <script type="text/javascript">
        function pageLoad() {
            $('[id*=ddl]:not([id$="SalesPerson"])').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });

            $('[id$="SalesPerson"]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterBehavior: 'both',
                filterPlaceholder: 'Search'
            });
        };
    </script>
    <link href="Content/ajaxcalendar.css" rel="stylesheet" />
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

        #ContentPlaceHolder1_txttodate, #ContentPlaceHolder1_txtFindLeadInqDate {
            padding: 3px 12px;
            /*height: 30px;*/
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
                width: 250, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 10000, template: "error"
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
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>


    <script type="text/javascript">
        function validate() {
            if ($('#<%=txtLeadInqDate.ClientID%>').val() == "") {
                errormessage("Please select Inquiry/Lead Date.");
                return false;
            }
            if ($('#<%=ddlLeadInqType.ClientID%>').val() == null) {
                errormessage("Please select Inquiry/Lead Type.");
                return false;
            }
            if ($('#<%=ddlCallerType.ClientID%>').val() == null) {
                errormessage("Please select Caller Type.");
                return false;
            }

            if ($('#<%=ddlNature.ClientID%>').val() == null) {
                errormessage("Please select Nature.");
                return false;
            }
            if ($('#<%=txtLeadInqDesc.ClientID%>').val() == "") {
                errormessage("Please enter Inquiry/Lead Description.");
                return false;
            }

            if ($('#<%=txtContactPerson.ClientID%>').val() == "") {
                errormessage("Please enter Contact Person.");
                return false;
            }
            if ($('#<%=ddlCountry.ClientID%>').val() == null) {
                errormessage("Please select Country.");
                return false;
            }
            if ($('#<%=ddlState.ClientID%>').val() == null) {
                errormessage("Please select State.");
                return false;
            }
            if ($('#<%=ddlCity.ClientID%>').val() == null) {
                errormessage("Please select City.");
                return false;
            }
            if ($('#<%=txtAddress.ClientID%>').val() == "") {
                errormessage("Please enter Address.");
                return false;
            }
            if ($('#<%=txtMobile.ClientID%>').val() == "") {
                errormessage("Please enter Mobile.");
                return false;
            }
            if ($('#<%=txtEmail.ClientID%>').val() != "") {
                if (!isValidEmailAddress($('#<%=txtEmail.ClientID%>').val())) {
                    errormessage("Please enter Email in correct Format.");
                    return false;
                }
            }
            if ($('#<%=ddlSource.ClientID%>').val() == null) {               
                errormessage("Please select Source of Inquiry/Lead.");
                return false;
            }
            if ($('#<%=ddlSalesPerson.ClientID%>').val() == null) {              
                errormessage("Please select SalesPerson.");
                return false;
            }

        }
        function isValidEmailAddress(emailAddress) {
            var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
            return pattern.test(emailAddress);
        };
    </script>

    <script type="text/javascript">
        function showpreview(input) {
           <%-- var uploadcontrol = document.getElementById('<%=sugImgFileUpload.ClientID%>').value;
            var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
            if (uploadcontrol.length > 0) {
                if (reg.test(uploadcontrol)) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $("#ContentPlaceHolder1_imgpreview").css('display', 'block');
                            $("#ContentPlaceHolder1_imgpreview").attr('src', e.target.result);
                        }
                        reader.readAsDataURL(input.files[0]);
                    }
                }
                else {
                    errormessage("Only image files are allowed!");
                    $("#ContentPlaceHolder1_imgpreview").css('display', 'none');
                    return false;
                }
            }--%>
        }

    </script>
    <script type="text/javascript">
        function DoNav(LeadInqId) {
            if (LeadInqId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', LeadInqId)
            }
        }
    </script>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
           <%-- $get("<%=hfItemId.ClientID %>").value = e.get_value();--%>
        }
    </script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to delete?")) {
                confirm_value.value = "Yes";
                document.forms[0].appendChild(confirm_value);
                return true;
            } else {
                confirm_value.value = "No";
                return false;
            }
        }
    </script>
    <script>
        $('.a').keyup(function (e) {
            if (e.keyCode == 8) {
             <%--   $('#<%=hfItemId.ClientID%>').val("");--%>
            }
        });

        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (!(iKeyCode != 8))
                e.preventDefault();
            return false;
            return true;
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode != 46 && (charCode < 48 || charCode > 57)))
                return false;
            return true;
        }
        function setround(v) {
            v.value = parseFloat(Math.round(v.value * 100) / 100).toFixed(2);
        }
    </script>
    <style>
        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }

        .multiselect-item span.input-group-btn button {
            padding-bottom: 3px;
            padding-top: 3px;
        }

        .multiselect-item span.input-group-addon {
            padding-bottom: 2px;
            padding-top: 2px;
        }

        .text-right {
            text-align: right !important;
        }
    </style>
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
                                <h3 class="box-title">Lead/Inquiry Entry</h3>
                                <div style="float: right">
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary" OnClick="btnFind_Click" />
                                    <%--      <asp:Button Style="margin-right: 5px;" type="button" ID="Button1" runat="server" Text="Find" class="btn btn-primary" OnClick="btnFind_Click" />--%>
                                </div>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Inquiry/Lead Date:</label>
                                                        <label for="requiredFields" style="color: red;">*</label>
                                                        <asp:TextBox ID="txtLeadInqDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtLeadInqDate" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Inquiry/Lead Type:</label>
                                                        <label for="requiredFields" style="color: red;">*</label>
                                                        <asp:ListBox ID="ddlLeadInqType" runat="server" SelectionMode="Single"></asp:ListBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Caller Type:</label>
                                                        <label for="requiredFields" style="color: red;">*</label>
                                                        <asp:ListBox ID="ddlCallerType" runat="server" SelectionMode="Single"></asp:ListBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Nature:</label>
                                                        <label for="requiredFields" style="color: red;">*</label>
                                                        <asp:ListBox ID="ddlNature" runat="server" SelectionMode="Single"></asp:ListBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Product Type:</label>
                                                        <input type="text" runat="server" class="form-control" maxlength="100" id="txtProductType" placeholder="Enter Product Type">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Approx. Order Value:</label>
                                                        <input type="text" runat="server" class="form-control" maxlength="100" id="txtApprxOrderVal" placeholder="Enter Approx. Order Value">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Avg. Order Value in Lac:</label>
                                                        <input type="text" onkeypress="return isNumberKey(event)" onchange="setround(this)" runat="server" class="form-control text-right" maxlength="10" id="txtAvgOrderVal" placeholder="Enter Avg. Order Value in Lac">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Source of Information:</label>
                                                        <asp:ListBox ID="ddlSourceInfo" runat="server" SelectionMode="Single"></asp:ListBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Source of Inquiry/Lead:</label>
                                                        <label for="requiredFields" style="color: red;">*</label>
                                                        <asp:ListBox ID="ddlSource" runat="server" SelectionMode="Single"></asp:ListBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Sales Person:</label>
                                                        <label for="requiredFields" style="color: red;">*</label>
                                                        <asp:ListBox ID="ddlSalesPerson" runat="server" SelectionMode="Single"></asp:ListBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Inquiry/Lead Description:</label>
                                                        <label for="requiredFields" style="color: red;">*</label>
                                                        <asp:TextBox ID="txtLeadInqDesc" Style="resize: none; height: 20%;" TextMode="MultiLine" runat="server" class="form-control" cols="20" Rows="2" MaxLength="1000" placeholder="Enter Inquiry/Lead Description"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Contact Person:</label>
                                                        <label for="requiredFields" style="color: red;">*</label>
                                                        <input type="text" runat="server" class="form-control" maxlength="100" id="txtContactPerson" placeholder="Enter Contact Person">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Firm Name:</label>
                                                        <input type="text" runat="server" class="form-control" maxlength="100" id="txtFirmName" placeholder="Enter Firm Name">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="product">Country:</label>
                                                        <label for="requiredFields" style="color: red;">*</label>
                                                        <asp:ListBox ID="ddlCountry" runat="server" SelectionMode="Single" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">State:</label>
                                                        <label for="requiredFields" style="color: red;">*</label>
                                                        <asp:ListBox ID="ddlState" runat="server" SelectionMode="Single" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="complaintNature">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                        <asp:ListBox ID="ddlCity" runat="server" SelectionMode="Single"></asp:ListBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Address:</label>
                                                        <label for="requiredFields" style="color: red;">*</label>
                                                        <input type="text" runat="server" class="form-control" maxlength="250" id="txtAddress" placeholder="Enter Address">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Mobile:</label>
                                                        <label for="requiredFields" style="color: red;">*</label>
                                                        <input type="text" runat="server" class="form-control" maxlength="14" id="txtMobile" placeholder="Enter Mobile">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Phone:</label>
                                                        <input type="text" runat="server" class="form-control" maxlength="20" id="txtPhone" placeholder="Enter Phone">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Email:</label>
                                                        <input type="text" runat="server" class="form-control" maxlength="50" id="txtEmail" placeholder="Enter Email">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-8 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Fax:</label>
                                                        <input type="text" runat="server" class="form-control" maxlength="20" id="txtFax" placeholder="Enter Fax">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="box-footer">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click"
                                    OnClientClick="javascript:return validate();" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" OnClientClick="test()" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary"
                                    OnClientClick="return Confirm()" OnClick="btnDelete_Click" />
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
                            <h3 class="box-title">Lead/inq. List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Add New Lead/Inq" OnClick="btnBack_Click" class="btn btn-primary" />
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="row">
                                <div class="col-md-3 col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">From Date:</label>
                                        <asp:TextBox ID="txtFindLeadInqDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtFindLeadInqDate" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">To Date:</label>
                                        <asp:TextBox ID="txtFindLeadInqToDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtFindLeadInqToDate" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12" id="ContactPersonDiv" runat="server">
                                    <div class="form-group">
                                        <label for="lblContactPersonName">Contact Person Name:</label>
                                        <asp:ListBox ID="ddlFindContactPerson" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label for="lblContactPersonName">Inquiry/Lead Type:</label>
                                        <asp:ListBox ID="ddlFindLeadInqType" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12" id="CallertypeDiv" runat="server">
                                    <div class="form-group">
                                        <label for="lblContactPersonName">Caller Type:</label>
                                        <asp:ListBox ID="ddlFindCallerType" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label for="lblContactPersonName">Status:</label>
                                        <asp:ListBox ID="ddlFindStatus" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12" id="LeadInqDescDiv" runat="server">
                                    <div class="form-group">
                                        <label for="lblContactPersonName">Inquiry/Lead Description:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="100" id="txtFindLeadInqDesc" placeholder="Enter Inquiry/Lead Description">
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12" id="NatureDiv" runat="server">
                                    <div class="form-group">
                                        <label for="complaintNature">Nature:</label>
                                        <asp:ListBox ID="ddlFindNature" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12" id="ProductTypeDiv" runat="server">
                                    <div class="form-group">
                                        <label for="complaintNature">Product Type:</label>
                                        <asp:ListBox ID="ddlFindProductType" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12" id="SourceInfoDiv" runat="server">
                                    <div class="form-group">
                                        <label for="complaintNature">Source of Information:</label>
                                        <asp:ListBox ID="ddlFindSourceInfo" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12" id="SourceDiv" runat="server">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Source of Inquiry/Lead:</label>
                                        <asp:ListBox ID="ddlFindSource" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12" id="SalesPersonDiv" runat="server">
                                    <div class="form-group">
                                        <label for="complaintNature">Sales Person:</label>
                                        <asp:ListBox ID="ddlFindSalesPerson" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail11" style="visibility: hidden; display: block">Go Btn:</label>
                                        <asp:Button type="button" ID="btnGo" OnClick="btnGo_Click" runat="server" Text="Go" class="btn btn-primary" />
                                        <asp:Button type="button" ID="btnCancel1" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel1_Click" />
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To CSV" class="btn btn-primary"
                                OnClick="btnExport_Click" />
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
                                                <th>Lead/Inq Id</th>
                                                <th>Status</th>
                                                <th>Contact Person</th>
                                                <th>Firm Name</th>
                                                <th>Address</th>
                                                <th>City</th>
                                                <th>State</th>
                                                <th>Country</th>
                                                <th>Mobile</th>
                                                <th>PhoneNo</th>
                                                <th>Email</th>
                                                <th>Fax</th>
                                                <th>Lead/Inq Type</th>
                                                <th>Caller Type</th>
                                                <th>Description</th>
                                                <th>Nature</th>
                                                <th>Product Type</th>
                                                <th class="text-right">Apprx Order Value</th>
                                                <th class="text-right">Avg Order Value</th>
                                                <th>Source</th>
                                                <th>Source Info</th>
                                                <th>Sales Person</th>
                                                <th>Status Remarks</th>
                                                <th>Status Date</th>
                                                <th>Resolved Remarks</th>
                                                <th>Resolved Date</th>
                                                <th>Closed Remarks</th>
                                                <th>Closed Date</th>
                                                 <th>Order Value</th>
                                               <%-- <th>By Import</th>--%>
                                                <th>Action</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("LeadInqId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("LeadInqId") %>' />
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("Date"))%></td>
                                        <asp:Label ID="dateLabel" Visible="false" runat="server" Text='<%#String.Format("{0:dd/MMM/yyyy}", Eval("Date"))%>'></asp:Label>
                                        <td><%#Eval("Lead/Inq Id") %></td>
                                        <asp:Label ID="lblLeadId" runat="server" Visible="false" Text='<%# Eval("Lead/Inq Id")%>'></asp:Label>
                                        <td><%#Eval("Status") %></td>
                                        <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Eval("Status")%>'></asp:Label>
                                        <td><%#Eval("Contact Person") %></td>
                                        <asp:Label ID="lblCp" runat="server" Visible="false" Text='<%# Eval("Contact Person")%>'></asp:Label>
                                        <td><%#Eval("Firm Name") %></td>
                                        <asp:Label ID="lblFNm" runat="server" Visible="false" Text='<%# Eval("Firm Name")%>'></asp:Label>
                                        <td><%#Eval("Address") %></td>
                                        <asp:Label ID="lblAddress" runat="server" Visible="false" Text='<%# Eval("Address")%>'></asp:Label>
                                        <td><%#Eval("City") %></td>
                                        <asp:Label ID="lblCity" runat="server" Visible="false" Text='<%# Eval("City")%>'></asp:Label>
                                        <td><%#Eval("State") %></td>
                                        <asp:Label ID="lblState" runat="server" Visible="false" Text='<%# Eval("State")%>'></asp:Label>
                                        <td><%#Eval("Country") %></td>
                                        <asp:Label ID="lblCountry" runat="server" Visible="false" Text='<%# Eval("Country")%>'></asp:Label>
                                        <td><%#Eval("Mobile") %></td>
                                        <asp:Label ID="lblMobile" runat="server" Visible="false" Text='<%# Eval("Mobile")%>'></asp:Label>
                                        <td><%#Eval("PhoneNo") %></td>
                                        <asp:Label ID="lblPhone" runat="server" Visible="false" Text='<%# Eval("PhoneNo")%>'></asp:Label>
                                        <td><%#Eval("Email") %></td>
                                        <asp:Label ID="lblEmail" runat="server" Visible="false" Text='<%# Eval("Email")%>'></asp:Label>
                                        <td><%#Eval("Fax") %></td>
                                        <asp:Label ID="lblFax" runat="server" Visible="false" Text='<%# Eval("Fax")%>'></asp:Label>
                                        <td><%#Eval("Lead/Inq Type") %></td>
                                        <asp:Label ID="lblInqType" runat="server" Visible="false" Text='<%# Eval("Lead/Inq Type")%>'></asp:Label>
                                        <td><%#Eval("Caller Type") %></td>
                                        <asp:Label ID="lblCallerType" runat="server" Visible="false" Text='<%# Eval("Caller Type")%>'></asp:Label>
                                        <td><%#Eval("Description") %></td>
                                        <asp:Label ID="lblDescription" runat="server" Visible="false" Text='<%# Eval("Description")%>'></asp:Label>
                                        <td><%#Eval("Nature") %></td>
                                        <asp:Label ID="lblnature" runat="server" Visible="false" Text='<%# Eval("Nature")%>'></asp:Label>
                                        <td><%#Eval("Product Type") %></td>
                                        <asp:Label ID="lblproductType" runat="server" Visible="false" Text='<%# Eval("Product Type")%>'></asp:Label>
                                        <td class="text-right"><%#Eval("Apprx Order Value") %></td>
                                        <asp:Label ID="lblAproxOValue" runat="server" Visible="false" Text='<%# Eval("Apprx Order Value")%>'></asp:Label>
                                        <td class="text-right"><%#Eval("Avg Order Value") %></td>
                                        <asp:Label ID="lblAvgOValue" runat="server" Visible="false" Text='<%# Eval("Avg Order Value")%>'></asp:Label>
                                        <td><%#Eval("Source") %></td>
                                        <asp:Label ID="lblSource" runat="server" Visible="false" Text='<%# Eval("Source")%>'></asp:Label>
                                        <td><%#Eval("Source Info") %></td>
                                        <asp:Label ID="lblSourceInfo" runat="server" Visible="false" Text='<%# Eval("Source Info")%>'></asp:Label>
                                        <td><%#Eval("Sales Person") %></td>
                                        <asp:Label ID="lblSalesPerson" runat="server" Visible="false" Text='<%# Eval("Sales Person")%>'></asp:Label>
                                        <td><%#Eval("remarks") %></td>
                                        <asp:Label ID="lblRemarks" runat="server" Visible="false" Text='<%# Eval("remarks")%>'></asp:Label>                                       
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("RespondDate"))%></td>        
                                        <asp:Label ID="lblrespondDate" Visible="false" runat="server" Text='<%#String.Format("{0:dd/MMM/yyyy}", Eval("RespondDate"))%>'></asp:Label>
                                        <td><%#Eval("Resolvedremarks") %></td>
                                        <asp:Label ID="lblRRemarks" runat="server" Visible="false" Text='<%# Eval("Resolvedremarks")%>'></asp:Label>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("ResolvedRespondDate"))%></td>                                       
                                        <asp:Label ID="lblRRespondDate" runat="server" Visible="false" Text='<%#String.Format("{0:dd/MMM/yyyy}", Eval("ResolvedRespondDate"))%>'></asp:Label>
                                        <td><%#Eval("Closedremarks") %></td>
                                        <asp:Label ID="lblClosedRemarks" runat="server" Visible="false" Text='<%# Eval("Closedremarks")%>'></asp:Label>                                       
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("ClosedRespondDate"))%></td>   
                                        <asp:Label ID="lblClosedResponddate" runat="server" Visible="false" Text='<%#String.Format("{0:dd/MMM/yyyy}", Eval("ClosedRespondDate"))%>'></asp:Label>
                                        <td><%#Eval("orderValue") %></td>
                                        <asp:Label ID="lblorderValue" runat="server" Visible="false" Text='<%# Eval("orderValue")%>'></asp:Label>
                                       <%-- <td><%#Eval("By Import") %></td>--%>
                                        <td>
                                            <asp:HyperLink runat="server" ID="hpl"
                                                NavigateUrl='<%# String.Format("TransLeadInqList.aspx?val={0}", Eval("LeadInqId")) %>'
                                                Text="Action" Target="_blank" ToolTip="Action" /></td>
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
