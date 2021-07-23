<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="MISReportPW.aspx.cs" Inherits="AstralFFMS.MISReportPW" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <%-- <script type="text/javascript">
         $(function () {
             $("#example1").DataTable({
                 
             });
         });
    </script>--%>
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

          #example1_wrapper .row {
            margin-right: 0px !important;
            margin-left: 0px !important;
        }

            #example1_wrapper .row .col-sm-12 {
                overflow-x: scroll !important;
                padding-left: 0px !important;
                margin-bottom: 10px;
            }

        input[type=text].jqx-input {
            text-align: center !important;
        }
    </style>

    <section class="content">
        <div class="row">          
            <div class="col-md-12">
                <div id="InputWork">                    
                    <div class="box box-primary">
                         <div class="box-header with-border">
                                <h3 class="box-title">MIS Report Party/Distributor Wise</h3>
                            </div>
                        <div class="box-header with-border">
                            <div style="text-align: center;display:none;">
                                <h3 class="box-title" style="color: #ff8000;">ASTRAL POLYTECHNIK LTD</h3>
                                <br />
                                <h4 style="color: #ff8000;">Ahmedabad</h4>
                                <br />
                                <h4 style="color: #ff8000;">Daily Working Summary L1</h4>
                            </div>
                            <div class="col-md-3" style="text-align: left;">
                               <%-- <label>Sales Person:</label>&nbsp;&nbsp;<asp:Label ID="saleRepName" runat="server" Text="Label"></asp:Label>--%>
                                <asp:HiddenField ID="dateHiddenField" runat="server" />
                                 <asp:HiddenField ID="dateHiddenField1" runat="server" />
                                <asp:HiddenField ID="smIDHiddenField" runat="server" />
                                <asp:HiddenField ID="beatIDHiddenField" runat="server" />
                                <asp:HiddenField ID="statusHiddenField" runat="server" />
                            </div>
                            <div class="col-md-3" style="float: right;">
                                <label></label>&nbsp;&nbsp;&nbsp;<asp:Label ID="currDateLabel" Visible="false" runat="server" Text="Label"></asp:Label>
                                <br />
                            </div>
                        </div>                       
                        <div class="box-body">
                           <%-- <asp:Label ID="dateLabel" runat="server" Text="Label" ForeColor="#6699ff"></asp:Label>--%>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="rpt" runat="server" OnItemDataBound = "OnItemDataBound" OnItemCommand="rpt_ItemCommand">
                                     <HeaderTemplate>
                                        <table id="example1" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                   <%-- <th>Date</th>--%>
                                                    <th id="tdTableCell" runat="server">Distributor Name</th>
                                                    <th>Party</th>
                                                    <th runat="server">Mobile</th>
                                                    <th id="thArea" runat="server">Address</th>
                                                    <th style="text-align:right">Order Amount</th>
                                                    <th>Item Details</th>
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                    <th colspan="4" style="text-align: right">Grand Total:</th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>                                                    
                                                </tr>
                                            </tfoot>
                                            <tbody>
                                    </HeaderTemplate>
                                         <ItemTemplate>
                                        <tr>
                                            <asp:HiddenField ID="hfPartyId" runat="server" Value='<%#Eval("PartyId") %>' />
                                            <td id ="distname" runat="server"><%#Eval("Distributor") %></td>
                                            <asp:Label ID="lblDistributor" runat="server" Visible="false" Text='<%# Eval("Distributor")%>'></asp:Label>
                                            <td><%#Eval("PartyName") %></td>
                                            <asp:Label ID="lblParty" runat="server" Visible="false" Text='<%# Eval("PartyName")%>'></asp:Label>
                                            <td><%#Eval("Mobile") %></td>
                                            <asp:Label ID="lblMobile" runat="server" Visible="false" Text='<%# Eval("Mobile")%>'></asp:Label>
                                            <td><%#Eval("Address") %></td>
                                            <asp:Label ID="lblAddress" runat="server" Visible="false" Text='<%# Eval("Address")%>'></asp:Label>
                                            <td style="text-align:right"><%#Eval("OrderAmount","{0:0.00}")%></td>
                                            <asp:Label ID="lblOrderAmount" runat="server" Visible="false" Text='<%# Eval("OrderAmount")%>'></asp:Label>
                                             <td> <asp:LinkButton CommandName="select" ID="lnkItem2"
                                            CausesValidation="False" runat="server" Text="Item Details" ToolTip="Item Details"
                                            Width="80px" Font-Underline="True" OnClientClick="window.document.forms[0].target='_blank'; setTimeout(function(){window.document.forms[0].target='';}, 500);" /></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                                  
                                </asp:Repeater>
                            </div>

                            <div style="float: left; margin-top: 5px;">
                               <%-- <input type="button" value="Export to Excel" id='excelExport' />--%>
                                 <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" Visible="false" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="btnExport_Click" />
                                <asp:Button type="button" ID="btnBack" runat="server" Height="35px" Text="Back" class="btn btn-primary"
                                    OnClick="btnBack_Click" Visible="false" />
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div id="divBackground" class="modal">
            <div id="divImage" >
                <img id="imgLoader" alt="" src="img/close.png" />
                
<table style="height: 30%; width: 30%;align-content:center">
     <tr>
        <td align="center" valign="bottom">
         <img id="deletemodal" alt="" src="img/cross.jpg"  style="margin-left:100%;width:15px;height:15px"  onclick="HideDiv()" />
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
body
{
    margin: 0;
    padding: 0;
    height: 100%;
}
.modal
{
    display: none;
    margin-bottom:110px;
    position: absolute;
    top: 0px;
    left: 0px;
    
    z-index: 100;
    opacity: 1;
    filter: alpha(opacity=60);
    -moz-opacity:.8;
    min-height: 100%;
}
#divImage
{
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
    <script type="text/javascript">
        function LoadDiv(url) {
            var img = new Image();
            var bcgDiv = document.getElementById("divBackground");
            var imgDiv = document.getElementById("divImage");
            var imgFull = document.getElementById("imgFull");
            var imgLoader = document.getElementById("imgLoader");
            imgLoader.style.display = "block";
            img.onload = function () {
                imgFull.src = img.src;
                imgFull.style.display = "block";
                imgLoader.style.display = "none";
            };
            img.src = url;
            var width = document.body.clientWidth;
            if (document.body.clientHeight > document.body.scrollHeight) {
                bcgDiv.style.height = document.body.clientHeight + "px";
            }
            else {
                bcgDiv.style.height = document.body.scrollHeight + "px";
            }
            imgDiv.style.left = (width - 650) / 2 + "px";
            imgDiv.style.top = "20px";
            bcgDiv.style.width = "100%";

            bcgDiv.style.display = "block";
            imgDiv.style.display = "block";
            return false;
        }
        function HideDiv() {
            var bcgDiv = document.getElementById("divBackground");
            var imgDiv = document.getElementById("divImage");
            var imgFull = document.getElementById("imgFull");
            if (bcgDiv != null) {
                bcgDiv.style.display = "none";
                imgDiv.style.display = "none";
                imgFull.style.display = "none";
            }
        }
</script>
      <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
     <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(function () {
             $('#example1').dataTable({
                 "order": [[4, "desc"]],
                 "footerCallback": function (tfoot, data, start, end, display) {
                     //$(tfoot).find('th').eq(0).html("Starting index is " + start);
                     var api = this.api();
                     var intVal = function (i) {
                         return typeof i === 'string' ?
                             i.replace(/[\$,]/g, '') * 1 :
                             typeof i === 'number' ?
                             i : 0;
                     };
                     debugger;
                     var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Order Amount'; }).first().index();
                     var totalData = api.column(costColumnIndex).data();
                     var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                     var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                     var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                     var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                     var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                     //var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Amount'; }).first().index();
                     //var totalData1 = api.column(costColumnIndex1).data();
                     //var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                     //var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                     //var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                     //var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                     //var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);



                     $(api.column(4).footer()).html(searchTotal);
                     //$(api.column(3).footer()).html(searchTotal1);

                     if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(4).footer()).html('0.0') }
                     //if (searchTotal1 == 'NaN' || searchTotal1 == '') { $(api.column(3).footer()).html('0.0') }

                     //if (searchTotal2 == 'NaN' || searchTotal2 == '') { $(api.column(9).footer()).html('0.0') }
                 }
             });
         });
    </script> 
</asp:Content>
