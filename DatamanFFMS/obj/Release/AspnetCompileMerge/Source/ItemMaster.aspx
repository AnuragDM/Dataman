<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ItemMaster.aspx.cs" Inherits="AstralFFMS.ItemMaster" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <%--  <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style>
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
    </style>
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
            overflow: auto;
            width: 100px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }

        @media (max-width: 600px) {
            .formlay {
                width: 100% !important;
                height: 60px;
            }
        }
    </style>
    <script type="text/javascript">
        $(document).ready(
            function () {
                BindItemSegment();
                BindItemClass();
                BindItemGroup();
            });
    </script>
    <script type="text/javascript">
        function BindItemSegment() {
               var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
               var obj = { SegmentId: 0 };
               $('#<%=ddlMastSegment.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: pageUrl + '/PopulateItemSegment',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ddlMastSegment.ClientID %>"));
            }
            function PopulateControl(list, control) {
                if (list.length > 0) {
                    //  control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(list, function () {
                        control.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });
                    var id = $('#<%=HiddenSegmentID.ClientID%>').val();
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
        /////////////////ItemClass

        function BindItemClass() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
            var obj = { ClassId: 0 };
            $('#<%=ddlMastClass.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
               $.ajax({
                   type: "POST",
                   url: pageUrl + '/PopulateItemClass',
                   data: JSON.stringify(obj),
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   success: OnPopulated,
                   failure: function (response) {
                       alert(response.d);
                   }
               });
               function OnPopulated(response) {
                   PopulateControl(response.d, $("#<%=ddlMastClass.ClientID %>"));
            }
            function PopulateControl(list, control) {
                if (list.length > 0) {
                    //  control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(list, function () {
                        control.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });
                    var id = $('#<%=HiddenClassID.ClientID%>').val();
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
        /////Item Group

        function BindItemGroup() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
            var obj = { ParentId: 0 };
                    $('#<%=ddlUnderItem.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: pageUrl + '/PopulateItemGroup',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ddlUnderItem.ClientID %>"));
               }
               function PopulateControl(list, control) {
                   if (list.length > 0) {
                       //  control.removeAttr("disabled");
                       control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                       $.each(list, function () {
                           control.append($("<option></option>").val(this['Value']).html(this['Text']));
                       });
                       var id = $('#<%=HiddenGroupID.ClientID%>').val();
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
        function validate() {
            if ($('#<%=ItemName.ClientID%>').val() == "") {
                errormessage("Please enter Item Name");
                return false;
            }

           <%-- var value = ($('#<%=ItemName.ClientID%>').val().charAt(0));
            var chrcode = value.charCodeAt(0);
            if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                errormessage("Do not start Name with special characters")
                return false;
            }--%>
            if ($('#<%=ddlUnderItem.ClientID%>').val() == "0" || $('#<%=ddlUnderItem.ClientID%>').val() == "") {
                errormessage("Please select Material Group");
                return false;
            }
            if ($('#<%=ddlMastClass.ClientID%>').val() == "0" || $('#<%=ddlMastClass.ClientID%>').val() == "") {
                errormessage("Please select Item Class");
                return false;
            }

            if ($('#<%=ddlMastSegment.ClientID%>').val() == "0" || $('#<%=ddlMastSegment.ClientID%>').val() == "") {
                errormessage("Please select Item Segment");
                return false;
            }
          <%--  if ($('#<%=Itemcode.ClientID%>').val() == "") {
                errormessage("Please enter Item Code");
                return false;
            }--%>
            if ($('#<%=Unit.ClientID%>').val() == "") {
                errormessage("Please enter Unit");
                return false;
            }
            if ($('#<%=StdPack.ClientID%>').val() == "") {
                errormessage("Please enter Standard Packing");
                return false;
            }
            if ($('#<%=PriceGroup.ClientID%>').val() == "") {
                errormessage("Please enter Price Group");
                return false;
            }
            $('#<%=HiddenGroupID.ClientID%>').val($('#<%=ddlUnderItem.ClientID%>').val());
            $('#<%=HiddenClassID.ClientID%>').val($('#<%=ddlMastClass.ClientID%>').val());
            $('#<%=HiddenSegmentID.ClientID%>').val($('#<%=ddlMastSegment.ClientID%>').val());

           
        }
    </script>
    <%--  <script type="text/javascript">
        $(document).ready(function () {
            var valLength = "";
            $('#<%=ItemName.ClientID%>').keypress(function (key) {

                valLength = ($('#<%=ItemName.ClientID%>').val().length + 1);

                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
        });
    </script>--%>
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
        function DoNav(ItemId) {
            if (ItemId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', ItemId)
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
                            <h3 class="box-title">Product Master</h3>
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
                                <div class="col-md-12">
                                    <div class="form-group formlay">
                                        <input id="ItemId" hidden="hidden" />
                                        <label for="exampleInputEmail1">Product Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="50" id="ItemName" placeholder="Enter Item Name" tabindex="2">
                                    </div>
                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Product Group:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlUnderItem" Width="100%" CssClass="form-control" runat="server" TabIndex="   4"></asp:DropDownList>
                                    </div>
                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Product Class:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlMastClass" Width="100%" CssClass="form-control" runat="server" TabIndex="6"></asp:DropDownList>
                                    </div>
                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Product Segment:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlMastSegment" Width="100%" CssClass="form-control" runat="server" TabIndex="8"></asp:DropDownList>
                                    </div>
                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Product Code:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="20" id="Itemcode" placeholder="Enter Item Code" tabindex="10">
                                    </div>
                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Unit:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="20" id="Unit" placeholder="Enter Unit" tabindex="12">
                                    </div>


                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">MRP:</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="15" id="MRP" onkeypress="javascript:return isNumber (event)" placeholder="Enter MRP" tabindex="14">
                                    </div>

                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Retailer Price:</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="15" id="RP" onkeypress="javascript:return isNumber (event)" placeholder="Enter Retailer Price" tabindex="16">
                                    </div>

                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Distributor Price:</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="15" id="DP" onkeypress="javascript:return isNumber (event)"
                                            placeholder="Enter Distributor Price" tabindex="18">
                                    </div>

                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Standard Packing:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="15" id="StdPack" onkeypress="javascript:return isNumber (event)" placeholder="Enter Standard Packing" tabindex="20">
                                    </div>
                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Price Group:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="15" id="PriceGroup" placeholder="Enter Price Group" tabindex="20">
                                    </div>
                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Sync Id:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="150" id="SyncId" placeholder="Enter Sync Id" tabindex="22">
                                    </div>

                                    <div class="form-group formlay">
                                        <label for="exampleInputEmail1">Active:</label>
                                        <input id="chkIsAdmin" runat="server" type="checkbox" checked="checked" class="checkbox" tabindex="26" />
                                        <input id="HdnFldIsAdmin" hidden="hidden" value="N" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="box-footer">
                             <asp:HiddenField ID="HiddenSegmentID" runat="server"  />  
                             <asp:HiddenField ID="HiddenClassID" runat="server"  />  
                             <asp:HiddenField ID="HiddenGroupID" runat="server"  />     
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" TabIndex="28" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" TabIndex="30" />
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
                            <h3 class="box-title">Product List</h3>
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
                                                <th>Product Name</th>
                                                <th>Product Group</th>
                                                <th>Product Code</th>
                                                <th>Unit</th>
                                                <th>Product Segment</th>
                                                <th>Product Class</th>
                                                <th>MRP</th>
                                                <th>DP</th>
                                                <th>RP</th>
                                                <th>Std.Pack</th>
                                                <th>Price Group</th>
                                                <th>Sync ID</th>
                                                <th>Active</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("ItemId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("ItemId") %>' />
                                        <td><%#Eval("ItemName") %></td>
                                        <td><%#Eval("MaterialGroup") %></td>
                                        <td><%#Eval("ItemCode") %></td>
                                        <td><%#Eval("Unit") %></td>
                                        <td><%#Eval("Segment") %></td>
                                        <td><%#Eval("ProductClass") %></td>
                                        <td><%#Eval("Mrp") %></td>
                                        <td><%#Eval("Dp") %></td>
                                        <td><%#Eval("Rp") %></td>
                                        <td><%#Eval("StdPack") %></td>
                                        <td><%#Eval("PriceGroup") %></td>
                                        <td><%#Eval("SyncId") %></td>
                                        <td><%#Eval("Active") %></td>
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
    <script type="text/javascript">
        $(document).ready(function () {
            var valLength = "";
            $("#ItemName").keypress(function (key) {

                valLength = ($("#ItemName").val().length + 1);

                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
        });
    </script>
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

