<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MastPage_V2.aspx.cs" Inherits="AstralFFMS.MastPage_V2" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .spinner {
            position: absolute;
            top: 50%;
            left: 50%;
            margin-left: -50px; /* half width of the spinner gif */
            margin-top: -50px; /* half height of the spinner gif */
            text-align: center;
            z-index: 999;
            overflow: auto;
            width: 100px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }

        #example1_wrapper .row {
            margin-right: 0px !important;
            margin-left: 0px !important;
        }

            #example1_wrapper .row .col-sm-12 {
                overflow-x: scroll !important;
                padding-left: 0px !important;
                margin-bottom: 10px;
            }

        select.form-control, .form-control {
            padding: 6px 12px !important;
            height: auto !important;
        }

        .contdiv a {
            position: absolute;
            top: 78%;
            left: 94%;
            transform: translate(-99%, -50%);
            -ms-transform: translate(-50%, -50%);
            background-color: #3c8dbc;
            color: white;
            padding: 1px 3px;
            cursor: pointer;
            border-radius: 5px;
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(
        function () {
            BindDistrict();
        });
    </script>
    <script type="text/javascript">
        function BindDistrict() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
            var obj = { ModuleId: 0 };
            $('#<%=ddlmodule.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: pageUrl + '/PopulateModule',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ddlmodule.ClientID %>"));
            }
            function PopulateControl(list, control) {
                if (list.length > 0) {
                    //  control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(list, function () {
                        control.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });
                    var id = $('#<%=HiddeModuleID.ClientID%>').val();
                    //  alert(id);
                    if (id != "") {
                        control.val(id);
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">Not available<option>');
                }

            }
        }

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
        //$(function () {
        //    $(".select2").select2();
        //});
    </script>
    <script type="text/javascript">
        function validate() {
            if ($('#<%=pgenme.ClientID%>').val() == "") {
                errormessage("Please enter Page Name");
                return false;
            }


            if ($('#<%=ddlmodule.ClientID%>').val() == "0" || $('#<%=ddlmodule.ClientID%>').val() == "") {
                errormessage("Please Select Module");
                return false;
            }
            if ($('#<%=dispnme.ClientID%>').val() == "") {
                errormessage("Please enter Display Name");
                return false;
            }

            if ($('#<%=prntId.ClientID%>').val() == "") {
                errormessage("Please enter Parent Id");
                return false;
            }
            <%-- if ($('#<%=ddlcityType.ClientID%>').val() == "0" || $('#<%=ddlcityType.ClientID%>').val() == "") {
                errormessage("Please Select City Type");
                return false;
            }
            if ($('#<%=ddlcityconveyancetype.ClientID%>').val() == "0" || $('#<%=ddlcityconveyancetype.ClientID%>').val() == "") {
                errormessage("Please Select City Conveyance Type");
                return false;
            }
            document.getElementById("<%=HiddeDisCitynUnderID.ClientID%>").value = $('#<%=ddlParentLoc.ClientID%>').val();
        }--%>

            document.getElementById("<%=HiddeModuleID.ClientID%>").value = $('#<%=ddlmodule.ClientID%>').val();
            document.getElementById("<%=HiddenModuleName.ClientID%>").value = $('#<%=ddlmodule.ClientID%> option:selected').text();
        }   
    </script>
    <script type="text/javascript">
       <%-- $(document).ready(function () {
            var valLength = "";
            $('#<%=Location.ClientID%>').keypress(function (key) {

                valLength = ($('#<%=Location.ClientID%>').val().length + 1);

                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
        });--%>
    </script>

    <script type="text/javascript">
        var k = 1;
        var z = 0;
        function showpreview(input) {
            var uploadcontrol = document.getElementById('<%=partyImgFileUpload.ClientID%>').value;
            var fi = document.getElementById('<%=partyImgFileUpload.ClientID%>');
            var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
            //alert(fi.files.length);
            if (fi.files.length > 0) {
                for (var i = 0; i <= fi.files.length - 1; i++) {
 
                    var fsize = fi.files.item(i).size;
                    var file = Math.round((fsize / 1024));
                    // The size of the file.
                    if (file > 1024) {
                        $('#note').show();
                    }  else {
                        $('#note').hide();
                    }
                }
            }
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
        function Showpic(value) {
            $get("ImgModal").src = value;
            $('#ShowPictureModal').modal('show');
        };
    </script>
    <script type="text/javascript">
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
        function DoNav(PageId) {
            if (PageId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', PageId)
            }
        }
    </script>

    <script type="text/javascript">

        function checkIDAvailability() {
            $.ajax({
                type: "POST",
                url: "MastPage_V2.aspx.cs/checkUserName",
                data: "{IDVal: '" + $("#<% =prntId.ClientID %>").val() + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: onSuccess,
                failure: function (AjaxResponse) {
                    errormessage("Not Exist in Parent Id");
                }
            });
            function onSuccess(AjaxResponse) {
                document.getElementById("Label1").innerHTML = AjaxResponse.d;
            }
        }
    </script>

    <script type="text/javascript">
        function onlyNumberKey(evt) {

            // Only ASCII charactar in that range allowed 
            var ASCIICode = (evt.which) ? evt.which : evt.keyCode
            if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
                return false;
            return true;
        }
    </script>

    <script>
        function visaCheck() {
            var textboxId = document.getElementById("ContentPlaceHolder1_prntId");
            var e = document.getElementById("ContentPlaceHolder1_ddlmodule");
            var strUser = e.options[e.selectedIndex].value;
            ContentPlaceHolder1_prntId.value = strUser;
            ContentPlaceHolder1_prntId.focus();
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
                            <%-- <h3 class="box-title">City Master</h3>--%>
                            <h3 class="box-title">
                                <asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>

                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary" OnClick="btnFind_Click" />

                                <%--  <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" runat="server" />--%>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <input id="PageId" hidden="hidden" />
                                        <label for="exampleInputEmail1">Page Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="100" id="pgenme" placeholder="Enter Page Name" autocomplete="off">
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Module:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlmodule" runat="server" Width="100%" CssClass="form-control" onchange="visaCheck()"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Display Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="100" id="dispnme" placeholder="Enter Display Name" autocomplete="off">
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Parent Id:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="50" id="prntId" placeholder="Enter Parent Id" autocomplete="off" onblur="checkIDAvailability();" onkeypress="return onlyNumberKey(event)">
                                    </div>
                                </div>

                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group " runat="server">
                                        <label for="exampleInputEmail1">Level Idx:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddltype" Width="100%" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="1"></asp:ListItem>
                                            <asp:ListItem Text="2"></asp:ListItem>
                                            <asp:ListItem Text="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Idx:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <%--<input type="text" runat="server" class="form-control" maxlength="50" id="idxtxt" placeholder="Enter Idx" autocomplete="off" readonly>--%>
                                        <asp:TextBox ID="idxtxt" runat="server" class="form-control" AutoCompleteType="Disabled" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>


                            </div>

                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Android Form:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="50" id="androidform" placeholder="Enter Android Form" autocomplete="off">
                                    </div>
                                </div>

                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group " runat="server">
                                        <label for="exampleInputEmail1">Menu Icon:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="50" id="menuicon" placeholder="Enter Menu Icon" autocomplete="off">
                                    </div>
                                </div>

                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">App:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="50" id="txtapp" placeholder="Enter App" autocomplete="off">
                                        <%-- <asp:TextBox ID="TextBox1" runat="server" class="form-control" AutoCompleteType="Disabled" ReadOnly="true"></asp:TextBox>--%>
                                    </div>
                                </div>


                            </div>

                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">

                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Image:</label>
                                        <%--<br>--%>
                                       

                                        <%--   <asp:FileUpload ID="comImgFileUpload" runat="server" onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" AllowMultiple="true" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                                ControlToValidate="comImgFileUpload" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                            <asp:HiddenField ID="hidimg" runat="server" Value="" />--%>


                                        <asp:FileUpload ID="partyImgFileUpload" runat="server" onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                            ControlToValidate="comImgFileUpload" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic"> 
                                        </asp:RegularExpressionValidator>
                                        <asp:HiddenField ID="hidimg" runat="server" Value="" />
                                         <b id="note" style="color: red; font-size: smaller;display:none">Note : Image size should be less than 1MB</b>
                                        <div class="col-md-4">
                                        </div>
                                    </div>

                                </div>

                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Display Y/N:</label>
                                            <input id="chkIsDisplay" runat="server" type="checkbox" checked="checked" class="checkbox" />
                                            <input id="HdnFldIsDisplay" hidden="hidden" value="Y" />
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Android Y/N:</label>
                                            <input id="chkIsAndroid" runat="server" type="checkbox" class="checkbox" />
                                            <input id="HdnFldIsAndroid" hidden="hidden" value="N" />
                                        </div>
                                    </div>
                                </div>



                                <%--          <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">City Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlcityType" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">City Conveyance Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlcityconveyancetype" runat="server" Width="100%" CssClass="form-control" autocomplete="off"></asp:DropDownList>
                                    </div>
                                </div>--%>
                            </div>


                            <div class="col-md-12 col-sm-12 col-xs-12">


                                <div class="form-group">
                                     <img id="imgpreview" width="200" src="" style="border-width: 0px; max-height: 60px !important; display: none; float: left;" runat="server" />
                                </div>
                            </div>

                        </div>

                        <div class="box-footer">
                            <asp:HiddenField ID="HiddeModuleID" runat="server" />
                            <asp:HiddenField ID="HiddenModuleName" runat="server" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClientClick="javascript:return validate();" OnClick="btnSave_Click"/>
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" OnClick="btnDelete_Click"/>
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
                            <h3 class="box-title">Page List</h3>
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
                                                <th>Display Name</th>
                                                <th>Module</th>
                                                <th>Page Name</th>
                                                <th>Display</th>
                                                <th>Level</th>
                                                <th>Sort</th>
                                                <th>Andriod</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("PageId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("PageId") %>' />
                                        <td><%#Eval("DisplayName") %></td>
                                        <td><%#Eval("Module") %></td>
                                        <td><%#Eval("PageName") %></td>
                                        <td><%#Eval("Display") %></td>
                                        <td><%#Eval("Level_Idx") %></td>
                                        <td><%#Eval("Idx") %></td>
                                        <td><%#Eval("Android") %></td>
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

        <!-- modals -->
        <%--  <div class="modal fade" id="ShowPictureModal" role="dialog">
            <div class="modal-dialog" style="width: 100%; max-width: 750px; min-width: 350px">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="container">
                            <img id="ImgModal" src="Uploads/Chrysanthemum.jpg" class="user-image" style="max-width: 700px; min-width: 300px" />
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>
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
