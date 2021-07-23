<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ItemPriceMaster.aspx.cs" Inherits="AstralFFMS.ItemPriceMaster" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%-- Developed By Akanksha Bais 16-04-2021 --%>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <link href="Content/bootstrap-responsive.min.css" rel="stylesheet">
    <link href="Content/bootstrap-responsive.css" rel="stylesheet">

   
    <section class="content">
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/waiting.gif" alt="Loading" /><br />
        </div>

        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body" id="Maindv">
            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-header">
                            <h4 class="box-title">ItemPriceList Master</h4>
                        </div>
                        <div class="box-body">
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label for="CoupanScheme">ItemName-Syncid:</label>
                                        <asp:TextBox ID="txtItem" runat="server" Enabled="false" CssClass="form-control " />
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label for="CoupanScheme">WefDate:</label>
                                        <asp:TextBox ID="txtWefDate" runat="server" Enabled="false" CssClass="form-control " />
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label for="CoupanScheme">PriceListApplicability:</label>
                                        <asp:TextBox ID="txtPriceApp" runat="server" Enabled="false" CssClass="form-control " />
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label for="CoupanScheme">Country/State/City/Dist SyncId:</label>
                                        <asp:TextBox ID="txtCSCD_Syncid" runat="server" Enabled="false" CssClass="form-control " />
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="row">

                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="CoupanScheme">MRP:</label>
                                        <asp:TextBox ID="txtMRP" runat="server" Enabled="true" CssClass="form-control numeric text-right" MaxLength="15" onkeypress="javascript:return isNumber (event)" placeholder="Enter MRP" TabIndex="1" />
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="CoupanScheme">RP:</label>
                                        <asp:TextBox ID="txtRP" runat="server" Enabled="true" CssClass="form-control numeric text-right" MaxLength="15" onkeypress="javascript:return isNumber (event)" placeholder="Enter Retailer Price" TabIndex="3" />
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="CoupanScheme">DP:</label>
                                        <asp:TextBox ID="txtDP" runat="server" Enabled="true" CssClass="form-control numeric text-right" MaxLength="15" onkeypress="javascript:return isNumber (event)" placeholder="Enter Distributor Price" TabIndex="2" />
                                    </div>
                                </div>

                            </div>



                        </div>
                        <asp:HiddenField ID="hiddenItemPriceId" runat="server" />
                        <input type="button" id="btnSave" value="Update" onclick="UpdatePriceList();" class="btn btn-primary" style="margin-left: 15px;" disabled />
                        <input type="button" id="btnCan" value="Cancel" onclick="location.reload();" class="btn btn-primary" style="margin-left: 15px;"  />
                        <hr class="striped-border" />
                        <div class="box-header">
                            <h4 class="box-title">ItemPrice List</h4>
                        </div>
                        <i class='fa fa-pencil'></i>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="tblItemPrice" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>

                                                <th>SNo</th>
                                                <th>Action</th>
                                                <th>ItemName</th>
                                                <th>ItemSyncId</th>
                                                <th>WefDate</th>
                                                <th>MRP</th>
                                                <th>DP</th>
                                                <th>RP</th>
                                                <th>PriceListApplicability</th>
                                                <th>Country/State/City/Dist SyncId</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>

                    </div>
                </div>
            </div>
        </div>

    </section>


    <style type="text/css">
        .striped-border {
            border: 1px dashed #000;
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

        .input-group .form-control {
            height: 34px;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            /*display: table;*/
        }

        .multiselect-container > li > a {
            white-space: normal;
        }

        .multiselect-container > li {
            width: 100%;
        }

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }

         .txtItem {
            width: 80%;
            float: right;
        }

        .txtItemnxt {
            width: 67%;
            float: right;
        }
    </style>

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
        var V1 = "";
        function errormessage(V1) {
            debugger;
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
        function Validate() {
            var Selectedapp = [];

            if (document.getElementById("ContentPlaceHolder1_txtMRP").value == "") {
                errormessage("Please Enter MRP.");
                return false;
            }
            if (document.getElementById("ContentPlaceHolder1_txtDP").value == "") {
                errormessage("Please Enter DP.");
                return false;
            }
            if (document.getElementById("ContentPlaceHolder1_txtRP").value == "") {
                errormessage("Please Enter RP.");
                return false;
            }
            if (parseFloat(document.getElementById("ContentPlaceHolder1_txtMRP").value) < parseFloat(document.getElementById("ContentPlaceHolder1_txtRP").value)) {
                errormessage("MRP can't smaller than RP.");
                return false;
            }
            if (parseFloat(document.getElementById("ContentPlaceHolder1_txtDP").value) > parseFloat(document.getElementById("ContentPlaceHolder1_txtRP").value)) {
                errormessage("DP can't greater than RP.");
                return false;
            }

            return true;
        }



        function UpdatePriceList() {

            if (Validate()) {
                debugger;
                $('#spinner').show();

                var obj = "";
                obj = '{"ItemPriceId":' + $('#<%=hiddenItemPriceId.ClientID %>').val() + ',"MRP":"' + document.getElementById("ContentPlaceHolder1_txtMRP").value + '","DP":"' + document.getElementById("ContentPlaceHolder1_txtDP").value + '","RP":"' + document.getElementById("ContentPlaceHolder1_txtRP").value + '"}';
                // debugger;
                $.ajax({
                    type: "POST",
                    url: 'ItemPriceMaster.aspx/UpdatePriceList',
                    data: obj,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        Successmessage("ItemPriceList Updated SuccessFully.");
                        ClearControls();


                    },
                    failure: function (response) {
                        console.log(response.d);

                    }
                });

            }
        }




        function ClearControls() {
            debugger;
            $('#spinner').hide();
            $('#<%=hiddenItemPriceId.ClientID %>').val('');
                document.getElementById('ContentPlaceHolder1_txtItem').value = '';
                document.getElementById('ContentPlaceHolder1_txtWefDate').value = '';
                document.getElementById("ContentPlaceHolder1_txtPriceApp").value = '';
                document.getElementById('ContentPlaceHolder1_txtCSCD_Syncid').value = '';
                document.getElementById('ContentPlaceHolder1_txtMRP').value = '';
                document.getElementById('ContentPlaceHolder1_txtDP').value = '';
                document.getElementById('ContentPlaceHolder1_txtRP').value = '';
          document.getElementById('btnSave').disabled = true;
                FillPriceList();
            }

            function FillPriceList() {
                $.ajax({
                    type: "POST",
                    url: 'ItemPriceMaster.aspx/FillPriceList',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: onSuccess,
                    failure: function (response) {
                        console.log(response.d);

                    }
                });

            }
            function onSuccess(response) {
                var data = JSON.parse(response.d);
                //  debugger;
                var table = $('#tblItemPrice').DataTable();
                table.destroy();
                $("#tblItemPrice").DataTable({
                    "order": [[0, "asc"]],
                    "ordering": true,
                    "aaData": data,
                    "aoColumns": [
                        { "mData": "Sno" },
                    {
                        "mData": "id",
                        "render": function (data, type, row, meta) {

                            return "<a onclick='FillData(" + data + ");'><i class='fa fa-pencil-alt' ></i></a>"
                        }
                    },
                    { "mData": "ItemName" },
                    { "mData": "SyncId" },
                    { "mData": "WEFDATE" },
                    { "mData": "MRP" },
                    { "mData": "DP" },
                     { "mData": "RP" },
                      { "mData": "PriceListApplicability" },
                      { "mData": "Country_State_City_Dist_SyncId" }
                    ]
                });
            }

            function FillData(ItemPriceId) {
                debugger;
                $('#spinner').show();
                $.ajax({
                    type: "POST",
                    url: 'ItemPriceMaster.aspx/GetItemPrice',
                    contentType: "application/json; charset=utf-8",
                    data: '{ItemPriceId:"' + ItemPriceId + '"}',
                    dataType: "json",
                    success: function (response) {
                        debugger;
                        document.getElementById('btnSave').disabled = false;
                        var data = JSON.parse(response.d);
                        document.getElementById('ContentPlaceHolder1_txtItem').value = data[0].ItemName + '-' + data[0].SyncId;
                        document.getElementById('ContentPlaceHolder1_txtWefDate').value = data[0].WEFDATE;
                        document.getElementById("ContentPlaceHolder1_txtPriceApp").value = data[0].PriceListApplicability
                        document.getElementById('ContentPlaceHolder1_txtCSCD_Syncid').value = data[0].Country_State_City_Dist_SyncId;
                        document.getElementById('ContentPlaceHolder1_txtMRP').value = data[0].MRP;
                        document.getElementById('ContentPlaceHolder1_txtDP').value = data[0].DP;
                        document.getElementById('ContentPlaceHolder1_txtRP').value = data[0].RP;
                        document.getElementById("ContentPlaceHolder1_hiddenItemPriceId").value = data[0].id
                        $('#spinner').hide();
                    },
                    failure: function (response) {
                        console.log(response.d);
                        $('#spinner').hide();
                    }
                });

            }



    </script>
    <script type="text/javascript">
        //$(function () {
        //    $('[id*=lstForApp]').multiselect({
        //        enableCaseInsensitiveFiltering: true,
        //        //buttonWidth: '200px',
        //        buttonWidth: '100%',
        //        includeSelectAllOption: true,
        //        maxHeight: 200,
        //        width: 215,
        //        enableFiltering: true,
        //        filterPlaceholder: 'Search'
        //    });
        //});
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#tblItemPrice").DataTable({});
            FillPriceList();

        });

    </script>
</asp:Content>
