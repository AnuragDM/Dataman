<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind=" .aspx.cs" Inherits="AstralFFMS.Rpt_ExportLogDBReport" EnableEventValidation="false" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <style type="text/css">
        .wrap{
            white-space:normal;
        }
        .Image
        {
 width: 21%;
        }
       
        #table-scroll { 
  height:150px;
  overflow:auto;  
  margin-top:20px;
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


     
    </style>
    <script type="text/javascript">
        $(function () {
            $('[id*=lstsales]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            
            
        });

    </script>
    <script type="text/javascript">

        function BindSalesPerson() {
            $('#<%=lstsales.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
             $.ajax({
                 type: "POST",
                 url: 'Rpt_ExportLogDBReport.aspx/BindSalesPerson',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: OnPopulated,
                 failure: function (response) {
                     alert(response.d);
                 }
             });
             function OnPopulated(response) {
                 $('#<%=lstsales.ClientID %>').empty();
                $.each(JSON.parse(response.d), function () {
                    $('#<%=lstsales.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');
                });
                $("#<%=lstsales.ClientID %>").multiselect('rebuild');
            }

        }
       
    </script>
    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }

    </script>
    
    <script type="text/javascript">       

         function loding() {
             $('#spinner').show();
         }
    </script>

    <script type="text/javascript">
       
        $(document).ready(function () {
            $('#itemsaleTable').DataTable();
           // BindSalesPerson();            
            //$('div[id$="rptmain"]').hide();
            
        });

    </script>

    <script type="text/javascript">


        
        function btnSubmitfunc() {

            var selectedValues = [];
            var selectedStatus = [];

            $("#<%=lstsales.ClientID %> :selected").each(function () {
                selectedValues.push($(this).val());
            });
            $("#<%=hidSalesPerson.ClientID %>").val(selectedValues);

           

            loding();
            //BindGridView();
        }

        function BindGridView() {

            $.ajax({

                type: "POST",
                url: "Rpt_ExportLogDBReport.aspx/getBackupData",
                data: '{fromdate:"' + $("#<%=txtfmDate.ClientID %>").val() + '",Todate:"' + $("#<%=txttodate.ClientID %>").val() + '", SMID:"' + $("#<%=hidSalesPerson.ClientID %>").val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess1,
                failure: function (response) {
                },
                error: function (response) { }
            });
        }

        function OnSuccess1(response1) {

            $('div[id$="rptmain"]').show();
            var data = JSON.parse(response1.d);

            var table = $('#itemsaleTable').DataTable();
            table.destroy();
            $("#itemsaleTable").DataTable({
                "order": [[0, "asc"]],
                "aaData": data,
                "aoColumns": [
                    { "mData": "SNo" },
                    { "mData": "date" },
                    { "mData": "smname" },
                    {
                        "mData": "logurl", "render": function (data, type, row, meta) {
                            return '<a href="' + data + '" style="cursor: pointer;" download>DownLoad Log </a>'
                        }
                    },
                    {
                        "mData": "dburl", "render": function (data, type, row, meta) {
                            return '<a href="' + data + '" style="cursor: pointer;" download>Download Db Backup</a>'
                        }
                    },
                    { "mData": "createddate" }
                ]
            });

            $('#spinner').hide();

        }

       


    </script>
    <script type="text/javascript">    
        function openImage(src) {
            //  alert("hello Image");
            if (src != "" && src != undefined && src!="NoImage")
            {
              //  alert(src);
                var format = '<img style="margin-left: 37%; margin-top: 10%;" class="Image" src=' + src + ' alt="N/A" />';
                $('#showImage').show();
                $('#showImage1').html(format);
            }        
            
        }
    </script>
    <script type="text/javascript">
        $("[id$=btnExport]").click(function (e) {
            alert("123");
            window.open('data:application/vnd.ms-excel,' + encodeURIComponent($('div[id$=rptmain]').html()));
            e.preventDefault();
        });
    </script>
    <section class="content">
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/waiting.gif" alt="Loading" /><br />
        </div>
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body">
            <!-- left column -->
            <!-- general form elements -->
            <div class="box box-primary">
                <div class="row">
                    <!-- left column -->
                    <div class="col-md-12">
                        <div id="InputWork">
                            <!-- general form elements -->

                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Exported Log And Database Backup Details Report</h3>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;display:none;">*</label>
                                                <asp:ListBox ID="lstsales" runat="server" SelectionMode="Multiple"></asp:ListBox> 
                                                <asp:HiddenField ID="hidSalesPerson" runat="server" />
                                            </div>

                                        </div>
                                  
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div id="DIV1" class="form-group">
                                                <label for="exampleInputEmail1">From Date:</label>
                                                <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;" ></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                            </div>
                                        </div>

                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">To Date:</label>
                                                <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;" ></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                            </div>
                                        </div>
                                       
                                        
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                                <div class="box-footer">
                                    <asp:Button Text="Go" ID="btnGo" OnClick="btnGo_Click" OnClientClick="javascript: btnSubmitfunc();" class="btn btn-primary" runat="server" />
                                    <asp:Button Text="Cancel" ID="btnCan" OnClick="btnCan_Click"  class="btn btn-primary" runat="server" />
                                   <%-- <input type="button" runat="server" onclick="btnSubmitfunc();" class="btn btn-primary" value="Go" />
                                    <input type="button" runat="server" onclick="location.reload();" class="btn btn-primary" value="Cancel" /> --%>                                   
                                </div>
                                <br />
                            </div>
                        </div>
                        <div id="rptmain" runat="server">
                            <div class="box-header table-responsive">
                                <asp:Repeater ID="distitemsalerpt" runat="server" OnItemCommand="distitemsalerpt_ItemCommand">
                                    <HeaderTemplate>
                                        <table id="itemsaleTable" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <th style="text-align: left; width: 1px !important;">S.No.</th>
                                                    <th style="text-align: left;">Date</th>
                                                    <th style="text-align: left;">Sales Person Name</th>
                                                    <th style="text-align: left;">Log Url</th>
                                                    <th style="text-align: left;">Database Url</th>
                                                    <th style="text-align: left;">Exported Date</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                         <tr>
                                             
                                                <td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>                              
                                          <td><%#String.Format("{0:dd/MMM/yyyy HH:mm:sss}", Eval("date"))%></td>
                                        <td><%#Eval("smname") %></td>                                       
                                             <td><asp:LinkButton id="LinkButton1" runat="server" Text="Download Log" CommandName="Download" CommandArgument='<%#Eval("logurl")%>' /></td>
                                        <td><asp:LinkButton id="url" runat="server" Text="Download DB" CommandName="Download" CommandArgument='<%#Eval("dburl")%>' /></td>
                                      <td><%#Eval("createddate") %></td>
                                    </tr>

                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>

                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>

                   
                       
                    </div>
                </div>
            </div>
        </div>
    </section>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.22/pdfmake.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/0.4.1/html2canvas.min.js"></script>
    <script type="text/javascript">
        function Export() {
            html2canvas(document.getElementById('itemsaleTable'), {
                onrendered: function (canvas) {
                    var data = canvas.toDataURL();
                    var docDefinition = {
                        content: [{
                            footer: function (currentPage, pageCount) { return currentPage.toString() + ' of ' + pageCount; },
                            header: function (currentPage, pageCount, pageSize) {
                                // you can apply any logic and return any valid pdfmake element

                                return [
                                  { text: 'simple text', alignment: (currentPage % 2) ? 'left' : 'right' },
                                  { canvas: [{ type: 'rect', x: 170, y: 32, w: pageSize.width - 170, h: 40 }] }
                                ]
                            },
                            image: data,
                            width: 500
                        }]
                    };
                    pdfMake.createPdf(docDefinition).download("Table.pdf");
                }
            });
        }
    </script>
</asp:Content>
