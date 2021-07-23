<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="DistSuggestion.aspx.cs" Inherits="AstralFFMS.DistSuggestion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
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
    </style>
    <script type="text/javascript">

        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 250, position: "top-right", opacity: 2,
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
                width: 250, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>


    <script type="text/javascript">
        function validate() {

            if ($('#<%=ddldept.ClientID%>').val() == "0") {
                errormessage("Please select department.");
                return false;
            }

            if ($('#<%=ddlComplaintNature.ClientID%>').val() == "0") {
                errormessage("Please select suggestion nature.");
                return false;
            }
           <%-- if ($('#<%=txtSearch.ClientID%>').val() == "") {
                errormessage("Please enter product.");
                return false;
            }--%>
            if ($('#<%=txtSearch.ClientID%>').val() != "") {
                if ($('#<%=hfItemId.ClientID%>').val() == "") {
                    errormessage("Please enter valid product");
                    return false;
                }
            }
            <%--if ($('#<%=TextArea1.ClientID%>').val() == "") {
                errormessage("Please enter new application area.");
                return false;
            }
            if ($('#<%=TextArea2.ClientID%>').val() == "") {
                errormessage("Please enter technical advantage.");
                return false;
            }
            if ($('#<%=TextArea3.ClientID%>').val() == "") {
                errormessage("Please enter suggestion to make product better.");
                return false;
            }--%>

            var uploadcontrol = document.getElementById('<%=sugImgFileUpload.ClientID%>').value;
            var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
            if (uploadcontrol.length > 0) {
                if (reg.test(uploadcontrol)) {
                    return true;
                }
                else {
                    errormessage("Only image files are allowed!");
                    return false;
                }
            }

        }
    </script>

    <script type="text/javascript">
        function showpreview(input) {
            var uploadcontrol = document.getElementById('<%=sugImgFileUpload.ClientID%>').value;
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
            }
        }

    </script>
    <script type="text/javascript">
        function DoNav(SuggId) {
            if (SuggId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', SuggId)
            }
        }
    </script>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfItemId.ClientID %>").value = e.get_value();
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
    <script>
        function isNumber(evt) {
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
                                <h3 class="box-title">Suggestion Entry</h3>
                                <div style="float: right">
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                        OnClick="btnFind_Click" />
                                </div>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="col-md-8">
                                            <div class="row">
                                                <div class="col-md-6 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Department:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                        <asp:DropDownList ID="ddldept" Width="100%" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddldept_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="complaintNature">Nature:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                        <asp:DropDownList ID="ddlComplaintNature" CssClass="form-control" Width="101%" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="product">Product:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                                        <asp:TextBox ID="txtSearch" runat="server" class="form-control a" placeholder="Enter Product"></asp:TextBox>
                                                        <ajaxToolkit:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" runat="server" CompletionListCssClass="completionList"
                                                            CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted"
                                                            BehaviorID="txtSearch_AutoCompleteExtender" FirstRowSelected="false" OnClientItemSelected="ClientItemSelected" CompletionInterval="0"
                                                            DelimiterCharacters="" ServiceMethod="SearchItem" ServicePath="~/TransSuggestion.aspx" MinimumPrefixLength="3" EnableCaching="true"
                                                            TargetControlID="txtSearch">
                                                        </ajaxToolkit:AutoCompleteExtender>
                                                        <asp:HiddenField ID="hfItemId" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="complaintNature">New Application Area:</label>
                                                        <%--&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>--%>
                                                        <textarea id="TextArea1" style="resize: none; height: 20%;" runat="server" placeholder="Enter New Application Area" class="form-control" cols="20" rows="2"></textarea>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">Technical Advantage:</label>
                                                        <%--&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>--%>
                                                        <textarea id="TextArea2" style="resize: none; height: 20%;" runat="server" placeholder="Enter Technical Advantage" class="form-control" cols="20" rows="2"></textarea>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6 col-sm-6">
                                                    <div class="form-group">
                                                        <label for="exampleInputEmail1">How To Make Product Better ?</label>
                                                        <%--&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>--%>
                                                        <textarea id="TextArea3" style="resize: none; height: 20%;" runat="server" placeholder="Enter How To Make Product Better" class="form-control" cols="20" rows="2"></textarea>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Image:</label>
                                                <asp:FileUpload ID="sugImgFileUpload" runat="server" onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" />
                                                <img id="imgpreview" height="200" width="200" src="" style="border-width: 0px; display: none;" runat="server" />
                                            </div>
                                             <div class="form-group">
                                                <tr><td colspan="2">
                      
                                <b>Note : Image size should be less than 1MB</b>
                       </td></tr>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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
                            <h3 class="box-title">Suggestion List</h3>
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
                                         <asp:HiddenField ID="docIDHdf" runat="server" />
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
                                        <label for="complaintNature">Nature:</label>
                                        <asp:DropDownList ID="DropDownList1" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail11" style="visibility: hidden; display: block">Go Btn:</label>
                                        <asp:Button type="button" ID="btnGo" runat="server" Style="padding: 3px 7px;" Text="Go" class="btn btn-primary" OnClick="btnGo_Click" />
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
                                                <th>DocId</th>
                                                <th>Nature</th>
                                                <th>Product</th>

                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("SuggId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("SuggId") %>' />
                                        <td><%#Eval("Vdate","{0:dd/MMM/yyyy}") %></td>
                                        <td><%#Eval("DocId") %></td>
                                        <td><%#Eval("Name") %></td>
                                        <td><%#Eval("ItemName") %></td>

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
            $('.a').keyup(function (e) {
                if (e.keyCode == 8) {
                    $('#<%=hfItemId.ClientID%>').val("");
            }
                });
            $("#example1").DataTable({
                "order": [[0, "desc"]]
            });
        });
    </script>
</asp:Content>
