<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="AgingDetail.aspx.cs" Inherits="AstralFFMS.AgingDetail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .excelbtn, #ContentPlaceHolder1_btnBack {
            background-color: #3c8dbc;
            border-color: #367fa9;
        }

        #excelExport {
            border-radius: 3px;
            -webkit-box-shadow: none;
            box-shadow: none;
            border: 1px solid transparent;
            background-color: #3c8dbc;
            border-color: #367fa9;
            color: white;
            height: 35px;
            padding: 2px 6px 4px 6px;
        }

        .WrapText {
            width: 100%;
            word-break: break-all;
        }

        .colPad {
            padding-left: 10px !important;
        }

        input[type=text].jqx-input {
            text-align: center !important;
        }

        .table tr td p {
            white-space: normal !important;
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
    </style>


    <style>
        .btncss {
            background: none !important;
            border: none;
            padding: 0 !important;
            font-family: arial, sans-serif;
            color: #069;
            text-decoration: underline;
            cursor: pointer;
        }
    </style>
    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div id="InputWork">
                    <div class="box box-primary">
                        <div class="box-header with-border">
                            <div class="box-header with-border">
                                <%--<h3 class="box-title">Distributor Invoice Report</h3>--%>
                                <h3 class="box-title">
                                    <asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            </div>
                            <asp:HiddenField ID="syncHiddenField" runat="server" Value='<%#Eval("syncid")%>' />
                            <%--   <div class="col-md-1 col-sm-1 col-xs-12" >      
                                       <asp:ImageButton ID="ImageMasterPage" runat="server" AlternateText="No Image"
                                                    Height="50px" Width="50px" class="img-circle" Style="cursor: pointer" OnClientClick="return LoadDiv(this.src);" />                                              
                            </div>
                            <div class="col-md-3 col-sm-3 col-xs-12" >
                                <label>Sales Person:</label>&nbsp;&nbsp;<asp:Label ID="saleRepName" runat="server" Text="Label"></asp:Label>
                                <asp:HiddenField ID="syncHiddenField" runat="server" />
                            </div>--%>

                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <label>Account :</label>&nbsp;&nbsp;&nbsp;<asp:Label ID="currDateLabel" runat="server" Text="Label"></asp:Label>
                                <br />
                            </div>

                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <label>From :</label>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblrng" runat="server" Text="Label"></asp:Label>
                                <br />
                            </div>

                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <label>Bill Status as on :</label>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblstatus" runat="server" Text="Label"></asp:Label>
                                <br />
                            </div>

                            <%-- <div class="col-md-5 col-sm-5 col-xs-12" >
                                    <label>Export View Type:</label>
                                   <asp:DropDownList ID="items" runat="server" CssClass="form-control" style="float: right;
    width: 75%;" >
                                          <asp:ListItem Selected="True" Value="1"> With Details </asp:ListItem>
                                <asp:ListItem Value="2"> WithOut Details </asp:ListItem>
                                   </asp:DropDownList>
                            
                                <br />
                            </div>--%>
                            <%-- <div class="col-md-3">
                             <label>Selected City:</label>&nbsp;&nbsp;<asp:Label ID="lblcity" runat="server" Text="Label"></asp:Label> <br />
                            </div>--%>
                        </div>
                        <div class="box-body">
                            <div class="box-body table-responsive">
                                <div id="detailDiv" runat="server">
                                    <div class="box-body table-responsive">
                                        <asp:Repeater ID="detailDistrpt" runat="server">
                                            <HeaderTemplate>
                                                <table id="example1" class="table table-bordered table-striped">
                                                    <thead>
                                                        <tr>
                                                            <th>Ref No</th>
                                                            <th>Type</th>
                                                            <th>Dated</th>
                                                            <th>Due Date</th>
                                                            <th>Total Amt</th>
                                                            <th>Pending Amt</th>
                                                            <th>(0-90) Days</th>
                                                            <th>(91-120) Days</th>
                                                            <th>(121-150) Days</th>
                                                            <th>(151-180) Days</th>
                                                            <th>(181-365) Days</th>
                                                            <th>(>=366) Days</th>
                                                            <%--<th>(Total) Days</th>--%>
                                                        </tr>
                                                    </thead>
                                                    <tfoot>
                                                        <th colspan="4" style="text-align: right">Total:</th>
                                                         <th style="text-align: right"></th>
                                                        <th style="text-align: right"></th>
                                                        <th style="text-align: right"></th>
                                                        <th style="text-align: right"></th>
                                                        <th style="text-align: right"></th>
                                                        <th style="text-align: right"></th>
                                                        <th style="text-align: right"></th>
                                                        <th style="text-align: right"></th>
                                                    </tfoot>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <%--    <asp:HiddenField ID="hdnsyncid" runat="server" Value='<%#Eval("syncid")%>' />
                                                <asp:HiddenField ID="hdnparty" runat="server" Value='<%#Eval("PartyName")%>' />--%>
                                                    <%-- <td>
                                                    <asp:LinkButton CommandName="select" ID="lnkEdit"
                                                        CausesValidation="False" runat="server" Text="Edit" ToolTip="Aging Details"
                                                        Width="80px" Font-Underline="True" OnClientClick="window.document.forms[0].target='_blank'; setTimeout(function(){window.document.forms[0].target='';}, 500);" /></td>--%>
                                                    <td><%#Eval("RecNo") %></td>
                                                    <td><%#Eval("Type") %></td>
                                                    <td><%#Eval("Date") %></td>
                                                    <td><%#Eval("DueDate") %></td>
                                                    <td><%#Eval("Total") %></td>
                                                    <td><%#Eval("Pending") %></td>
                                                    <td><%#Eval("Curr") %></td>
                                                    <td><%#Eval("NinOT") %></td>
                                                    <td><%#Eval("OtOF") %></td>
                                                    <td><%#Eval("OFOE") %></td>
                                                    <td><%#Eval("OELst") %></td>
                                                    <td><%#Eval("lastyr") %></td>
                                                    <%--<td><%#Eval("total") %></td>--%>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>     </table>       
                                            </FooterTemplate>
                                        </asp:Repeater>

                                    </div>
                                </div>
                            </div>

                            <div style="float: left; margin-top: 5px;">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="btnExport_Click"/>
                                <asp:Button type="button" ID="btnBack" runat="server" Height="35px" Text="Back" class="btn btn-primary"
                                    Visible="false" />
                            </div>

                        </div>

                    </div>
                </div>
            </div>
            <div id="divBackground" class="modal">
                <div id="divImage">
                    <img id="imgLoader" alt="" src="img/close.png" />

                    <table style="height: 30%; width: 30%; align-content: center">
                        <tr>
                            <td align="center" valign="bottom">
                                <img id="deletemodal" alt="" src="img/cross.jpg" style="margin-left: 100%; width: 15px; height: 15px" onclick="HideDiv()" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="center">
                                <img id="imgFull" alt="" src="" style="display: none; height: 300px; width: 300px" />
                            </td>
                        </tr>

                    </table>
                </div>
            </div>
        </div>




    </section>
    <style type="text/css">
        body {
            margin: 0;
            padding: 0;
            height: 100%;
        }

        #divImage {
            /*display: none;
    z-index: 1000;
    position: fixed;
    align-content:center;
    top: 0;
    left: 0;
    background-color: White;
    height: 300px;
    width: 300px;
    padding: 3px;
    border: solid 1px black;*/
            display: none;
            z-index: 1000;
            position: fixed;
            /* align-content: center; */
            top: 16% !important;
            left: 38% !important;
            /*background-color: White;*/
            height: 300px;
            width: 300px;
            /*padding: 3px;*/
            /*border: solid 1px black;*/
            opacity: 1;
        }
    </style>

    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>



    <script src="jqwidgets/MYScript.js"></script>
    <script src="jqwidgets/jquery.table2excel.js"></script>
    <script type="text/javascript">
        //$(function () {
        //    $("#example1").DataTable();
        //});
    </script>

    <script type="text/javascript">
        $(function () {
            $('#example1').dataTable({
                "order": [[0, "desc"]],
                "footerCallback": function (tfoot, data, start, end, display) {
                    //$(tfoot).find('th').eq(0).html("Starting index is " + start);
                    var api = this.api();
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Total Amt'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(4).footer()).html(searchTotal);

                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Pending Amt'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(5).footer()).html(searchTotal);

                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == '(0-90) Days'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(6).footer()).html(searchTotal);


                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == '(91-120) Days'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(7).footer()).html(searchTotal);

                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == '(121-150) Days'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(8).footer()).html(searchTotal);

                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == '(151-180) Days'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(9).footer()).html(searchTotal);

                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == '(181-365) Days'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(10).footer()).html(searchTotal);

                    var costColumnIndex = $('th').filter(function (i) { return $(this).text() == '(>=366) Days'; }).first().index();
                    var totalData = api.column(costColumnIndex).data();
                    var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                    var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                    var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                    var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                    $(api.column(11).footer()).html(searchTotal);
                   

                    if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(3).footer()).html('0.0') }
                }
            })

        });
    </script>

</asp:Content>
