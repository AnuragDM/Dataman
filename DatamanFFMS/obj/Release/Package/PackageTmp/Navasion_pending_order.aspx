<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="Navasion_pending_order.aspx.cs" Inherits="AstralFFMS.Navasion_pending_order" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <style type="text/css">
       td{
           padding-left:4px;
           padding-right:4px;
       }
       button{
           margin-left: 15px;
            
       }
    </style>
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
 <div class="box-body" id="rptmain" runat="server" style="display: block;background-color:white">
        <%--    <div class="row">--%>
                <!-- left column -->
                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Order Not Downloaded At Navision Report</h3>
                            </div>
                            
                        </div>
                    </div>
                </div>
      <%--      </div>--%>
            <div class="box-header with-border">
                <button onclick="exportTableToCSV('DistrinbutorInvoiceEntryCount.csv')" class="btn  btn-primary" style="margin-bottom: 15px;">Export</button>
               <button onClick="document.location.reload(true)" class="btn  btn-primary" style="margin-bottom: 15px;float:right;margin-right:12px;">Refresh</button>
                </div>
     
                <div class="col-xs-12">

                  <%--  <div class="box">--%>
                        <!-- /.box-header -->
                      <%--  <div class="box-body table-responsive">--%>
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table-responsive table-striped  table-bordered" style="width:100%;">
                                        <thead>
                                            <tr style="height:35px">
                                                <th>S.No</th>
                                                <th>Party Name</th>
                                                <th>PODocID</th>
                                                <th>Order Date</th>                                         
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr >
                                       <td><%# Container.ItemIndex + 1 %></td>
                                        <td><%#Eval("PartyName") %></td>
                                        <td><%#Eval("PODocId") %></td>
                                        <td><%#Eval("order date") %></td>
                                        </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                     <%--   </div>--%>
                        
       <%--             </div>--%>
                    
                </div>
        

        </div>
     
        

    </section>
     <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable(
                {
                    "paging": false
                }
                );

        });
    </script>
    <script type="text/javascript">
        function downloadCSV(csv, filename) {
            var csvFile;
            var downloadLink;

            // CSV file
            csvFile = new Blob([csv], { type: "text/csv" });

            // Download link
            downloadLink = document.createElement("a");

            // File name
            downloadLink.download = filename;

            // Create a link to the file
            downloadLink.href = window.URL.createObjectURL(csvFile);

            // Hide download link
            downloadLink.style.display = "none";

            // Add the link to DOM
            document.body.appendChild(downloadLink);

            // Click download link
            downloadLink.click();
        }


        function exportTableToCSV(filename) {
            var csv = [];
            var rows = document.querySelectorAll("table tr");

            for (var i = 0; i < rows.length; i++) {
                var row = [], cols = rows[i].querySelectorAll("td, th");

                for (var j = 0; j < cols.length; j++)
                    row.push(cols[j].innerText);

                csv.push(row.join(","));
            }

            // Download CSV file
            downloadCSV(csv.join("\n"), filename);
        }
    </script>
</asp:Content>




