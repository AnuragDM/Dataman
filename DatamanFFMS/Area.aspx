<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="Area.aspx.cs" Inherits="AstralFFMS.Area" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
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

        #select2-ContentPlaceHolder1_ddlParentLoc-container {
            margin-top: -8px !important;
        }

        select.form-control,.form-control {
            padding: 6px 12px !important;
        }

         #example1_wrapper .row{
            margin-right: 0px !important; 
            margin-left: 0px !important; 
        }

        #example1_wrapper .row .col-sm-12{
            overflow-x: scroll !important;
            padding-left: 0px !important;
            margin-bottom: 10px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            BindCity();
        }
      );
    </script>
    <script type="text/javascript">
        function BindCity() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
            var obj = { CityId: 0, RecType: '' };
            $('#<%=ddlParentLoc.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: pageUrl + '/PopulateCity',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnCityPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnCityPopulated(response) {
                PopulateCityControl(response.d, $("#<%=ddlParentLoc.ClientID %>"));
            }
            function PopulateCityControl(list, control) {
                if (list.length > 0) {
                    // control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(list, function () {
                        control.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });
                    var id = $('#<%=HiddenCityUnderID.ClientID%>').val();
                    //     alert(id);
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
            if ($('#<%=Location.ClientID%>').val() == "") {
                errormessage("Please enter Area Name");
                return false;
            }

            var value = ($('#<%=Location.ClientID%>').val().charAt(0));
            var chrcode = value.charCodeAt(0);
            if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                errormessage("Do not start Name with special characters")
                return false;
            }
            if ($('#<%=ddlParentLoc.ClientID%>').val() == "0" || $('#<%=ddlParentLoc.ClientID%>').val() == "") {
                errormessage("Please Select City");
                return false;
            }
            document.getElementById("<%=HiddenCityUnderID.ClientID%>").value = $('#<%=ddlParentLoc.ClientID%>').val();
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
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
        });
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
        //$(function () {
        //    $(".select2").select2();
        //});
    </script>
    <script type="text/javascript">
        function DoNav(AreaId) {
            if (AreaId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', AreaId)
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
                            <%--  <h3 class="box-title">Area Master</h3>--%>
                            <h3 class="box-title">
                                <asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
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
                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                    <div class="form-group">
                                        <input id="AreaId" hidden="hidden" />
                                        <label for="exampleInputEmail1">Area:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="100" id="Location" placeholder="Enter Area Name" autocomplete="off">
                                    </div>

                                </div>
                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlParentLoc" runat="server" Width="100%" class="form-control" ></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Sync Id:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="50" id="SyncId" placeholder="Enter Sync Id" autocomplete="off">
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Active:</label>
                                        <input id="chkIsActive" runat="server" type="checkbox" checked="checked" class="checkbox"/>
                                        <input id="HdnFldIsActive" hidden="hidden" value="N" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="box-footer">
                            <asp:HiddenField ID="HiddenCityUnderID" runat="server" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClientClick="javascript:return validate();"  OnClick="btnSave_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"  OnClick="btnCancel_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" OnClick="btnDelete_Click" />
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
                            <h3 class="box-title">Area List</h3>
                            <div style="float: right">
                                <asp:Button type="button" ID="btnExport" OnClick="btnExport_Click" runat="server" Text="Export" Visible="true" class="btn btn-primary" />
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

                                                <th>Area</th>
                                                <%-- <th>City</th>--%>
                                                <th>Parent</th>
                                                <th>Sync ID</th>
                                                <th>Active</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("AreaId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("AreaId") %>' />
                                        <td><%#Eval("AreaName") %></td>
                                        <%--  <td><%#Eval("Parent") %></td>--%>
                                        <td><%#Eval("DisplayName1") %></td>
                                        <td><%#Eval("SyncId") %></td>
                                        <td><%#Eval("Active1") %></td>
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
