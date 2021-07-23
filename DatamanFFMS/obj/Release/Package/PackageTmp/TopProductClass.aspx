<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/FFMS.Master" CodeBehind="TopProductClass.aspx.cs" Inherits="AstralFFMS.TopProductClass" %>

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
            $('[id*=matGrpListBox]').multiselect({
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
            $('[id*=productListBox]').multiselect({
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
            //$("#itemsaleTable").DataTable({
            //    "order": [[0, "desc"]]
            //});   
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
       

</script>
    <style type="text/css">
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

        function btnSubmitfunc() {
            var selectedvalue = [];
            debugger;
            $("#<%=trview.ClientID %> :checked").each(function () {
                debugger;

                selectedvalue.push($(this).val());
                // alert($(this).val());
            });
            //$("#hidsalesperson").val(selectedvalue);
            //alert(selectedvalue);
            if (selectedvalue == "") {
                errormessage("Please Select Sales Person");
                return false;
            }

            // $("#hiddistributor").val(selectedvalue);
        <%--  var selectedValues = [];
            $("#<%=ListBox1.ClientID %> :selected").each(function () {
                 selectedValues.push($(this).val());
             });
            $("#hiddistributor").val(selectedValues);--%>
            // validate($("#hiddistributor").val());


            var productGroupValues = [];
            $("#<%=matGrpListBox.ClientID %> :selected").each(function () {
                productGroupValues.push($(this).val());
            });
            $("#hidproductgroup").val(productGroupValues);

            var productValues = [];
            $("#<%=productListBox.ClientID %> :selected").each(function () {
                productValues.push($(this).val());
            });
            $("#hidproduct").val(productValues);

            loding();
            BindGridView();
        }

        function BindGridView() {
            $.ajax({
                type: "POST",
                url: "TopProductReport.aspx/GetTopproduct",
                data: '{ProductGroup: "' + $("#hidproductgroup").val() + '" , Product: "' + $("#hidproduct").val() + '", Fromdate: "' + $('#<%=txtfmDate.ClientID%>').val() + '",Todate: "' + $('#<%=txttodate.ClientID%>').val() + '",noofrecords: "' + $('#<%=txt_noofrecords.ClientID%>').val() + '" ,salesPerson: "' + $('#ContentPlaceHolder1_hidsalesperson').val() + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: OnSuccess,
              failure: function (response) {
                  //alert(response.d);
              },
              error: function (response) {
                  //alert(response.d);
              }
          });
      };



      function OnSuccess(response) {
          //  alert(JSON.stringify(response.d));
          // alert(response.d);
          $('div[id$="rptmain"]').show();
          var data = JSON.parse(response.d);
          //alert(data);
          //var arr1 = data.length;
          //alert(arr1);
          var table = $('#ContentPlaceHolder1_rptmain table').DataTable();
          table.destroy();
          debugger;
          $("#ContentPlaceHolder1_rptmain table ").DataTable({
              //"order": [[0, "desc"]],

              "aaData": data,
              "aoColumns": [


          { "mData": "ProductGroup" },
          { "mData": "Product" },
          {
              "mData": "Qty",
              "render": function (data, type, row, meta) {
                  if (type === 'display') {
                      return $('<div>')
                         .attr('class', 'text-right')
                         .text(data)
                         .wrap('<div></div>')
                         .parent()
                         .html();

                  } else {
                      return data;
                  }
              }
          },
          {
              "mData": "Amount",
              "render": function (data, type, row, meta) {
                  if (type === 'display') {
                      return $('<div>')
                         .attr('class', 'text-right')
                         .text(data)
                         .wrap('<div></div>')
                         .parent()
                         .html();

                  } else {
                      return data;
                  }
              }
          }
              ]
          });

          $('#spinner').hide();
      }

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
            alert("123");
            window.open('data:application/vnd.ms-excel,' + encodeURIComponent($('div[id$=rptmain]').html()));
            e.preventDefault();
        });
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
                                    <%--<h3 class="box-title">Top Products Class</h3>--%>
                                    <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="row">
                                         
                                       <%-- <div class="col-md-3 col-sm-6 col-xs-7" hidden>
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>  
                                                <asp:ListBox ID="salespersonListBox" runat="server" SelectionMode="Multiple"
                                                    OnSelectedIndexChanged="salespersonListBox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                            </div>
                                        </div>        --%>
                                         <div class="col-md-4 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>  
                                                <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged" ></asp:TreeView>
                                             
                                            </div>
                                        </div>       
                                          <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">No. of Recods:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                                
                                            <asp:TextBox ID="txt_noofrecords" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;">99</asp:TextBox>
                                                
                                           
                                        </div>                              
                                        
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="row">
                                        <%-- <div class="col-md-8 col-sm-11">--%>
                                        
                                        
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group" style="margin-left: 8%;">
                                                <label for="exampleInputEmail1">Product Class:</label>                                                
                                                <asp:ListBox ID="matGrpListBox" runat="server" SelectionMode="Multiple"
                                                    OnSelectedIndexChanged="matGrpListBox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                                  <input type="hidden" id="hidproductgroup" />
                                                 <input type="hidden" id="hidsalesperson" runat="server"/>
                                                <input type="hidden" id="hidproduct" />

                                            </div>
                                        </div>
                                        <div class="col-md-1"></div>
                                        <div class="col-md-3 col-sm-6 col-xs-12" hidden="hidden">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Product:</label>                                               
                                                <asp:ListBox ID="productListBox" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                            </div>
                                        </div>
                                        <%-- </div>--%>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="row">
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div id="DIV1" class="form-group" style="margin-left: 8%;">
                                                <label for="exampleInputEmail1">From Date:</label>
                                                <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-md-1"></div>
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">To:</label>
                                                <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                            </div>
                                        </div>

                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                                <div class="box-footer">
                                  
                                    <asp:Button type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClientClick="loding()"  
                                         OnClick="btnGo_Click"  />
                                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Cancel_Click" />
                                   <%-- <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                        OnClick="btnExport_Click" />--%>
                                     <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                      OnClick="btnExport_Click"/>
                                    <%-- <input style="margin-right: 5px;" type="button" id="Go" value="Go" class="btn btn-primary" onc onclick="GetReport();" />--%>
                                </div>
                                <br />                       
                            </div>
                        </div>
                        <div id="rptmain" runat="server">
                            <div class="box-header table-responsive">
                                <asp:Repeater ID="Topsaleproduct" runat="server" OnItemCommand="Topsaleproduct_ItemCommand">     
                                    <HeaderTemplate>
                                        <table id="itemsaleTable" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>                                                      
                                                    <th style="text-align: left; width: 10%">Product Class</th>
                                                   <%-- <th style="text-align: left; width: 15%">Product</th>--%>
                                                    <th style="text-align: right; width: 8%">Order Qty</th>
                                                    <th style="text-align: right; width: 10%">Amount</th>
                                                    <th style="text-align: left; width: 10%">View Details</th> 
                                                </tr>
                                            </thead>
                                             <tfoot>
                                                <tr>
                                                    <th colspan="1" style="text-align: right">Grand Total:</th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>                                                    
                                                </tr>
                                            </tfoot>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                          
                                            <tr> 
                                           
                                             <asp:HiddenField ID="hdnitemid" runat="server" Value='<%#Eval("ClassId")%>' />
                                                                           
                                            <td><%# Eval("ProductClass")%>
                                            <asp:Label ID="lblProductGroup" runat="server" Visible="false" Text='<%# Eval("ProductClass")%>'></asp:Label></td>
                                           <%-- <td><%# Eval("Product")%>
                                            <asp:Label ID="lblProduct" runat="server" Visible="false" Text='<%# Eval("Product")%>'></asp:Label></td>  --%>                                            
                                            <td style="text-align: right"><%# Eval("Qty")%>
                                            <asp:Label ID="lblQty" runat="server" Visible="false" Text='<%# Eval("Qty")%>'></asp:Label></td>     
                                            <td style="text-align: right"><%# Eval("Amount")%>
                                            <asp:Label ID="lblAmount" runat="server" Visible="false" Text='<%# Eval("Amount")%>'></asp:Label></td> 
                                                <td>
                                           <asp:LinkButton CommandName="select" ID="lnkEdit" Text="View Details"
                                            CausesValidation="False" runat="server"  ToolTip="Order Details"
                                            Width="80px" Font-Underline="True" OnClientClick="window.document.forms[0].target='_blank'; setTimeout(function(){window.document.forms[0].target='';}, 500);" /></td>  
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



     <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
     <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(function () {
             $('#itemsaleTable').dataTable({
                 "order": [[2, "desc"]],
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
                     var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Order Qty'; }).first().index();
                     var totalData = api.column(costColumnIndex).data();
                     var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                     var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                     var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                     var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                     var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);


                     var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Amount'; }).first().index();
                     var totalData1 = api.column(costColumnIndex1).data();
                     var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                     var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                     var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                     var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                     var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);



                     $(api.column(1).footer()).html(searchTotal);
                     $(api.column(2).footer()).html(searchTotal1);

                     if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(1).footer()).html('0.0') }
                     if (searchTotal1 == 'NaN' || searchTotal1 == '') { $(api.column(2).footer()).html('0.0') }

                     //if (searchTotal2 == 'NaN' || searchTotal2 == '') { $(api.column(9).footer()).html('0.0') }
                 }
             });
         });
    </script>
</asp:Content>