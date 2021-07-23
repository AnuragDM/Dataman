<%@ Page Language="C#" MasterPageFile="~/FFMS.Master"  AutoEventWireup="true" CodeBehind="NotVisitedOutletReport_V2.aspx.cs" Inherits="AstralFFMS.NotVisitedOutletReport_V2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>

    <script type="text/javascript">
        $(function () {
            $('[id*=Lstareabox]').multiselect({
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
        $(function () {
            $('[id*=Lstbeatbox]').multiselect({
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
        $(function () {
            $('[id*=Lstpartybox]').multiselect({
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
        $(function () {
            $("#itemsaleTable").DataTable({
                "order": [[0, "asc"]]
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $('[id*=salespersonListBox]').multiselect({
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
        $(function () {
            $("[id*=trview] input[type=checkbox]").bind("click", function () {
                var table = $(this).closest("table");
                if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                    //Is Parent CheckBox
                    var childDiv = table.next();
                    var isChecked = $(this).is(":checked");
                    //alert(isChecked);
                    $("input[type=checkbox]", childDiv).each(function () {
                        if (isChecked) {
                            $(this).prop("checked", "checked");
                        } else {
                            $(this).removeAttr("checked");
                        }
                    });
                } else {
                    //Is Child CheckBox
                    var parentDIV = $(this).closest("DIV");
                    if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                        $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                    } else {
                        $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                    }
                }
            });
        })
    </script>
    <script type="text/javascript">
        //$(function () {
        //    $(".select2").select2();
        //});
    </script>
    <script type="text/javascript">

        //function postBackByObject() {
        //    var o = window.event.srcElement;
        //    if (o.tagName == "INPUT" && o.type == "checkbox") {
        //        __doPostBack("", "");
        //    }
        //}

        function fireCheckChanged(e) {
            var ListBox1 = document.getElementById('<%= trview.ClientID %>');
            var evnt = ((window.event) ? (event) : (e));
            var element = evnt.srcElement || evnt.target;

            if (element.tagName == "INPUT" && element.type == "checkbox") {
                __doPostBack("", "");
            }
        }

        function loding() {
            $('#spinner').show();
        }
    </script>
    <%-- <script type="text/javascript">
         function validate(persons) {
             if (document.getElementById("<%=ListBox1.ClientID%>").value == "" || document.getElementById("<%=ListBox1.ClientID%>").value == "0") {                
                       errormessage("Please Select Distributor");
                       document.getElementById("<%=ListBox1.ClientID%>").focus();                 
                       return false;
                   }                          
         
           
               }

    </script>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            //Hide the div          
            //  $('div[id$="rptmain"]').hide();
            //conversely do the following to show it again if needed later
            //$('#showdiv').show();
        });

    </script>
    <style type="text/css">
              .button1 {
box-shadow: 0px 2px 4px 2px #888888;
margin-left: 10px;
}
                    h2 {
                 font-size: 20px !important;
    font-weight: 600 !important;
    margin-left: 13px !important;
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

         input[type=checkbox], input[type=radio] {
            margin-right: 12px;
            margin-left: 12px;
        }

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }

           .box-header img {
            margin-top: -7px;
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
        function ValidateCheckBoxList() {
            var checkBoxList = document.getElementById("<%=trview.ClientID %>");
            var checkboxes = checkBoxList.getElementsByTagName("input");
            var isValid = false;
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    isValid = true;
                    break;
                }
                alert(isValid + "2");
            }
            args.IsValid = isValid;
        }
    </script>
    <script type="text/javascript">
        $("[id$=btnExport]").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + encodeURIComponent($('div[id$=rptmain]').html()));
            e.preventDefault();
        });
    </script>
    <section class="content">
        <div id="spinner" runat="server" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
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
                                     <img id="img-Header" src="img/Location/no-visiting.png" />
                                    <h2 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h2>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="row">

                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Sales Person:</label>
                                                &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                             <b>   <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged"></asp:TreeView></b>

                                            </div>
                                        </div>
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Area:</label>                                            
                                                <asp:ListBox ID="Lstareabox" runat="server" OnSelectedIndexChanged="Lstareabox_SelectedIndexChanged" AutoPostBack="true" SelectionMode="Multiple"></asp:ListBox>
                                               
                                            </div>                                           
                                        </div>  
                                          <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Beat:</label>                                            
                                                <asp:ListBox ID="Lstbeatbox" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="Lstbeatbox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                               
                                            </div>                                           
                                        </div>        
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="row">
                                      
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Outlet:</label>
                                                <asp:ListBox ID="Lstpartybox" runat="server" SelectionMode="Multiple"></asp:ListBox>

                                            </div>
                                        </div>  
                                             <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Outlet Status:</label>
                                            <asp:DropDownList ID="ddlPType" runat="server"  CssClass="form-control" Width="99%">
                                                <asp:ListItem Text="All" Value="All"></asp:ListItem>   
                                                <asp:ListItem Text="Active" Value="Active" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Not Active" Value="Not Active"></asp:ListItem>                                               
                                            </asp:DropDownList>
                                        </div>
                                    </div>      
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div id="DIV1" class="form-group">
                                                <label for="exampleInputEmail1">No. of Days:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:TextBox ID="txt_noofrecords" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;">99</asp:TextBox>
                                            </div>
                                        </div>                                 
                                    </div>
                                   
                                </div>
                                <div class="box-footer">
                                    <asp:Button type="button" ID="btnGo" runat="server" Text="Generate" class="btn btn-primary button1"
                                        OnClick="btnGo_Click" />
                                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary button1" OnClick="Cancel_Click" />
                                
                                    <%-- <input style="margin-right: 5px;" type="button" id="Go" value="Go" class="btn btn-primary" onc onclick="GetReport();" />--%>
                                </div>
                                <br />
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



    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <%--  <script type="text/javascript">
         $(function () {
             $('#itemsaleTable').dataTable({
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
                     $(api.column(4).footer()).html(searchTotal);

                     if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(4).footer()).html('0.0') }

                 }
             });
         });
    </script>--%>
</asp:Content>
