<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="DSRReport.aspx.cs" Inherits="AstralFFMS.DSRReport" %>

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
                            <div style="text-align: center;display:none;">
                                <h3 class="box-title" style="color: #ff8000;">ASTRAL POLYTECHNIK LTD</h3>
                                <br />
                                <h4 style="color: #ff8000;">Ahmedabad</h4>
                                <br />
                                <h4 style="color: #ff8000;">Daily Working Summary L1</h4>
                            </div>
                            <div class="col-md-3" style="text-align: left;">
                                <label>Sales Person:</label>&nbsp;&nbsp;<asp:Label ID="saleRepName" runat="server" Text="Label"></asp:Label>
                                <asp:HiddenField ID="dateHiddenField" runat="server" />
                                <asp:HiddenField ID="smIDHiddenField" runat="server" />
                                <asp:HiddenField ID="beatIDHiddenField" runat="server" />
                                <asp:HiddenField ID="statusHiddenField" runat="server" />
                            </div>
                            <div class="col-md-3" style="float: right;">
                                <label>Report Date :</label>&nbsp;&nbsp;&nbsp;<asp:Label ID="currDateLabel" runat="server" Text="Label"></asp:Label>
                                <br />
                            </div>
                        </div>                       
                        <div class="box-body">
                            <asp:Label ID="dateLabel" runat="server" Text="Label" ForeColor="#6699ff"></asp:Label>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="rpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="example1" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <th>Date</th>
                                                    <th>City</th>
                                                    <th>PartyId</th>
                                                    <th>Party</th>
                                                    <th>Address</th>
                                                    <th>Mobile</th>
                                                    <th>Contact Person</th>
                                                    <th>Stype</th>
                                                    <th>Product Class</th>
                                                    <th>Product Segment</th>
                                                    <th>Product Group</th>                                                                                                       
                                                    <th>Item</th>
                                                    <th style="text-align:right">Stock</th>                                                     
                                                    <th style="text-align:right">Qty</th>
                                                    <th style="text-align:right">Rate</th>
                                                    <th style="text-align:right">Amount</th>
                                                    <th>NextVisitDate</th>
                                                    <th>NextVisitTime</th>
                                                    <th>Competitor Name</th>
                                                    <th>Discount</th>
                                                    <%--<th>Brand Activity</th>
                                                    <th>Meet Activity</th>--%>
                                                    <th>Other Activity</th>
                                                    <%--<th>Other GeneralInfo</th>
                                                    <th>Road Show</th>
                                                    <th>Scheme/offers</th>--%>
                                                    <th>Remark</th>
                                                     <th>Longitude</th>
                                                     <th>Latitude</th>
                                                     <th>Address</th>
                                                   <%-- <th>Download Image</th>--%>
                                                    <th>View Image</th>
                                                    
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                    <th colspan="12" style="text-align: right">Total:</th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                </tr>
                                            </tfoot>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <asp:HiddenField ID="linkHiddenField" runat="server" Value='<%#Eval("Image") %>' />
                                            <asp:HiddenField ID="sTypeHdf" runat="server" Value='<%#Eval("Stype") %>' />
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("COMPTID") %>' />
                                          <%--  <td><%# Eval("Mobile_Created_date")%></td>--%>
                                           <%-- <td><%# System.Convert.ToDateTime(Eval("Mobile_Created_date")).ToString("dd/MMM/yyyy HH:mm:ss")%>--%>
                                            <td><%#Eval("Mobile_Created_date")==DBNull.Value ? "" : Eval("Mobile_Created_date")%>
                                            <td><%# Eval("Beat")%></td>
                                            <td><%# Eval("PartyId")%></td>
                                            <td><%# Eval("Party")%></td>
                                             <td><%# Eval("Address1")%></td>
                                             <td><%# Eval("Mobile")%></td>
                                              <td><%# Eval("ContactPerson")%></td>
                                            <td><%# Eval("Stype")%></td>
                                            <td><%# Eval("productClass")%></td>
                                            <td><%# Eval("Segment")%></td>
                                            <td><%# Eval("MaterialGroup")%></td>                                           
                                            <td><%# Eval("CompItem")%></td>
                                            <td><%# Eval("Stock")%></td>
                                            <td style="text-align:right"><%# Eval("CompQty")%></td>
                                            <td style="text-align:right"><%# Eval("ComRate")%></td>
                                            <td style="text-align:right"><%# Eval("Value")%></td>
                                            <td><%# Eval("NextVisitDate")%></td>
                                            <td><%# Eval("NextVisitTime")%></td>
                                            <td><%# Eval("CompName")%></td>
                                            <td><%# Eval("Discount")%></td>
                                            <%--<td><%# Eval("BrandActivity")%></td>
                                            <asp:Label ID="BrandActivityLabel" runat="server" Visible="false" Text='<%# Eval("BrandActivity")%>'></asp:Label> 
                                             <td><%# Eval("MeetActivity")%></td>
                                            <asp:Label ID="MeetActivityLabel" runat="server" Visible="false" Text='<%# Eval("MeetActivity")%>'></asp:Label> --%>
                                            <%-- <td><%# Eval("OtherActivity")%></td>
                                            <asp:Label ID="OtherActivityLabel" runat="server" Visible="false" Text='<%# Eval("OtherActivity")%>'></asp:Label> --%>
                                            <td><asp:HyperLink ID="hpl" runat="server" NavigateUrl= '<%# Eval("ComptId", "~/CompOtherActivity.aspx?ComptId={0}") %>' Text='<%#Eval("OtherActivity") %>' Target="_blank"></asp:HyperLink></td>
                                            

                                            <td><%# Eval("Remarks")%></td>
                                             <td><%# Eval("Longitude")%></td>
                                             <td><%# Eval("Latitude")%></td>
                                             <td><%# Eval("Address")%></td>
                                             <td><asp:LinkButton ID="lnkViewDemoImg" runat="server" OnClick="lnkViewDemoImg_Click" Visible="false">View Image</asp:LinkButton>
                                                 <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl='<%# Eval("Image")%>' AlternateText="No Image"
                                                    Width="25px" Height="25px" Style="cursor: pointer" OnClientClick="return LoadDiv(this.src);" />
                                             </td>
                                            
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>

                            <div style="float: left; margin-top: 5px;">
                               <%-- <input type="button" value="Export to Excel" id='excelExport' />--%>
                                 <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="btnExport_Click" />
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
             "order": [[0, "asc"]],
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
                      var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Amount'; }).first().index();
                      var totalData = api.column(costColumnIndex).data();
                      var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                      var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                      var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                      var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                      var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);                   


                      var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Qty'; }).first().index();
                      var totalData1 = api.column(costColumnIndex1).data();
                      var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                      var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                      var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                      var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                      var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                      //var costColumnIndex2 = $('th').filter(function (i) { return $(this).text() == 'Rate'; }).first().index();
                      //var totalData2 = api.column(costColumnIndex2).data();
                      //var total2 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                      //var pageTotalData2 = api.column(costColumnIndex2, { page: 'current' }).data();
                      //var pageTotal2 = pageTotalData2.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                      //var searchTotalData2 = api.column(costColumnIndex2, { 'filter': 'applied' }).data();
                      //var searchTotal2 = searchTotalData2.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                      var costColumnIndex3 = $('th').filter(function (i) { return $(this).text() == 'Stock'; }).first().index();
                      var totalData3 = api.column(costColumnIndex3).data();
                      var total3 = totalData3.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                      var pageTotalData3 = api.column(costColumnIndex3, { page: 'current' }).data();
                      var pageTotal3 = pageTotalData3.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                      var searchTotalData3 = api.column(costColumnIndex3, { 'filter': 'applied' }).data();
                      var searchTotal3 = searchTotalData3.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

                      $(api.column(15).footer()).html(searchTotal);
                      $(api.column(13).footer()).html(searchTotal1);
                      //$(api.column(9).footer()).html(searchTotal2);
                      $(api.column(12).footer()).html(searchTotal3);
                      if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(14).footer()).html('0.0') }
                      if (searchTotal1 == 'NaN' || searchTotal1 == '') { $(api.column(12).footer()).html('0.0') }
                      if (searchTotal3 == 'NaN' || searchTotal3 == '') { $(api.column(11).footer()).html('0.0') }
                      //if (searchTotal2 == 'NaN' || searchTotal2 == '') { $(api.column(9).footer()).html('0.0') }
                  }
              });
     });
    </script>
</asp:Content>
