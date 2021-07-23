<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="BrandSaleReport.aspx.cs" Inherits="AstralFFMS.BrandSaleReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=ListBox1]').multiselect({
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
   <%-- <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();
        });
    </script>--%>
    <script type="text/javascript">
        $(function () {
            $('[id*=productClassListBox]').multiselect({
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
        var V = "";
        function Successmessage(V) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
  <%--  <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();
        });
    </script>--%>

   <script type="text/javascript">
       $(function () {
           $("[id*=trview] input[type=checkbox]").bind("click", function () {
               var table = $(this).closest("table");
               if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                   //Is Parent CheckBox
                   var childDiv = table.next();
                   var isChecked = $(this).is(":checked");
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

    <script type="text/javascript">
        $(document).ready(function () {
            //Hide the div          
            $('div[id$="rptDiv"]').hide();
            //conversely do the following to show it again if needed later
            //$('#showdiv').show();
        });

</script>

    <style type="text/css">
        .multiselect-container > li > a {
            white-space: normal;
        }
         .multiselect-container.dropdown-menu {
            width: 100% !important;
        }

        .input-group .form-control {
            height: 34px;
        }

        #ContentPlaceHolder1_CheckBoxList1 td {
            padding: 0 15px;
        }
        #ContentPlaceHolder1_viewasRadioButtonList td{
            padding: 0 15px;
        }
        #ContentPlaceHolder1_CheckBoxList1_2, #ContentPlaceHolder1_CheckBoxList1_1, #ContentPlaceHolder1_CheckBoxList1_0{
  margin:0 5px;
}
        #ContentPlaceHolder1_viewasRadioButtonList_0, #ContentPlaceHolder1_viewasRadioButtonList_1{
        margin:0 5px;
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
             $("#<%=trview.ClientID %> :checked").each(function () {
                  selectedvalue.push($(this).val());
              });
              if (selectedvalue == "") {
                  errormessage("Please Select Sales Person");
                  return false;
              }
            var selectedValues = [];
             $("#<%=ListBox1.ClientID %> :selected").each(function () {
              selectedValues.push($(this).val());
          });
          $("#hiddistributor").val(selectedValues);             
          if (selectedValues == "") {
              errormessage("Please Select Distributor");
              return false;
          }
          var productClassValues = [];
          $("#<%=productClassListBox.ClientID %> :selected").each(function () {
              productClassValues.push($(this).val());
              });
             $("#hidproductClass").val(productClassValues);

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

             var YearValues = [];
             $("[id*=CheckBoxList1] input:checked").each(function () {
                 YearValues.push($(this).val());
             });
             $("#hidyear").val(YearValues);

             if (YearValues.length  == "0") {
                 alert("Please select year.");
                 return false;
             }
            
             var checked_radio = $("[id*=viewasRadioButtonList] input:checked");
             var value = checked_radio.val();
             $("#hidview").val(value);
             

          loding();
          BindGridView();
      }

      function BindGridView() {
          $.ajax({
              type: "POST",
              url: "BrandSaleReport.aspx/GetBrandSale",
              data: '{Distid: "' + $("#hiddistributor").val() + '", ProductClass: "' + $("#hidproductClass").val() + '", ProductGroup: "' + $("#hidproductgroup").val() + '" , Product: "' + $("#hidproduct").val() + '", year: "' + $("#hidyear").val() + '", View: "' + $("#hidview").val() + '"}',
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
            $('div[id$="rptDiv"]').show();
            var data = JSON.parse(response.d);
            //alert(data);
            //var arr1 = data.length;
            //alert(arr1);
            var table = $('#ContentPlaceHolder1_rptDiv table').DataTable();
            table.destroy();
            $("#ContentPlaceHolder1_rptDiv table ").DataTable({

                "order": [[0, "asc"]],
                "aaData": data,
                "aoColumns": [
            { "mData": "SyncId" },
            { "mData": "distributor" }, // <-- which values to use inside object
            { "mData": "ProductClass" },
            { "mData": "MaterialGROUP" },
            { "mData": "Year" },
            { "mData": "t1Value" },
            { "mData": "t2Value" },
            { "mData": "t3Value" },
            { "mData": "t4Value" },
            { "mData": "t5Value" },
            { "mData": "t6Value" },
            { "mData": "t7Value" },
            { "mData": "t8Value" },
            { "mData": "t9Value" },
            { "mData": "t10Value" },
            { "mData": "t11Value" },
            { "mData": "t12Value" }
                ]
            });

            $('#spinner').hide();
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
        <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Brandwise Sale Report</h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                    <div class="col-md-4 col-sm-6 col-xs-12">
                                       <%-- <div class="form-group" hidden>
                                            <label for="exampleInputEmail1">Sales person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                             
                                            <asp:ListBox ID="salespersonListBox" runat="server" SelectionMode="Multiple"
                                                OnSelectedIndexChanged="salespersonListBox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>                                          
                                        </div>--%>
                                        <div class="form-group">
                                                <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>  
                                                <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged"></asp:TreeView>
                                            </div>
                                    </div>
                                     <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Distributor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                            
                                            <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                            <input type="hidden" id="hiddistributor" />
                                            <input type="hidden" id="hidproductClass" />
                                            <input type="hidden" id="hidproductgroup" />
                                            <input type="hidden" id="hidproduct" />
                                            <input type="hidden" id="hidyear" />
                                            <input type="hidden" id="hidview" />
                                        </div>
                                    </div>
                                </div>                             
                                <div class="row">
                                    <div class="col-md-2 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Product Class:</label>                                           
                                            <asp:ListBox ID="productClassListBox" runat="server" SelectionMode="Multiple"
                                                OnSelectedIndexChanged="productClassListBox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-md-1"></div>
                                      <div class="col-md-1"></div>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Product Group:</label>                                          
                                            <asp:ListBox ID="matGrpListBox" runat="server" SelectionMode="Multiple"
                                                OnSelectedIndexChanged="matGrpListBox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <%--<label for="exampleInputEmail1">Product:</label>   --%>                                       
                                            <asp:ListBox ID="productListBox" runat="server" SelectionMode="Multiple" Visible="false"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>
                                  <div class="row">                                 
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Year:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:CheckBoxList ID="CheckBoxList1" runat="server" RepeatDirection="Horizontal" >
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                        <div class="col-md-1"></div>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">View As:</label>
                                            <asp:RadioButtonList ID="viewasRadioButtonList" RepeatDirection="Horizontal" runat="server">
                                                <asp:ListItem Selected="True" Value="Quantity" Text="Quantity"></asp:ListItem>
                                                <asp:ListItem Value="Amount" Text="Amount"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                       
                                    <div class="col-md-3 col-sm-6 col-xs-12 hidden">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1" hidden>View By:</label>
                                            <asp:CheckBox ID="distCheckBox" runat="server" Text="View By Distributor" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="box-footer">
                                 <input type="button" runat="server" onclick="btnSubmitfunc();" class="btn btn-primary" value="Go" />  
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClientClick="loding()"  
                                    OnClick="btnGo_Click" Visible="false" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                    OnClick="btnCancel_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                    OnClick="ExportCSV" />
                            </div>                            


                            <div id="rptDiv" runat="server">
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="brandSalerpt" runat="server">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>  
                                                        <th style="text-align:left">Distributor Sync Id</th>                                                    
                                                        <th style="text-align:left">Distributor Name</th>
                                                        <th style="text-align:left">Product Class</th>
                                                        <th style="text-align:left">Product Group</th>
                                                        <th style="text-align:left">Year</th>
                                                        <th style="text-align: right">Jan</th>
                                                        <th style="text-align: right">Feb</th>
                                                        <th style="text-align: right">Mar</th>
                                                        <th style="text-align: right">Apr</th>
                                                        <th style="text-align: right">May</th>
                                                        <th style="text-align: right">Jun</th>
                                                        <th style="text-align: right">Jul</th>
                                                        <th style="text-align: right">Aug</th>
                                                        <th style="text-align: right">Sep</th>
                                                        <th style="text-align: right">Oct</th>
                                                        <th style="text-align: right">Nov</th>
                                                        <th style="text-align: right">Dec</th>
                                                       <%-- <th style="text-align: right">Grand Total</th>--%>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                        <%--  <tr>
                                             
                                                <td style="text-align: left;"><%#Eval("distributor") %></td>
                                               
                                        <asp:Label ID="distributorLabel" runat="server" Visible="false" Text='<%# Eval("distributor")%>'></asp:Label>
                                          
                                                <td style="text-align: left;"><%#Eval("ProductClass") %></td>
                                              
                                        <asp:Label ID="ProductClassLabel" runat="server" Visible="false" Text='<%# Eval("ProductClass")%>'></asp:Label>
                                          
                                                <td style="text-align: left;"><%#Eval("MaterialGROUP") %></td>
                                               
                                        <asp:Label ID="MaterialGROUPLabel" runat="server" Visible="false" Text='<%# Eval("MaterialGROUP")%>'></asp:Label>
                                        
                                                <td style="text-align: left;"><%#Eval("Year") %></td>
                                             
                                        <asp:Label ID="YearLabel" runat="server" Visible="false" Text='<%# Eval("Year")%>'></asp:Label>
                                          
                                                <td style="text-align: right;"><%#Eval("t1Value") %></td>
                                            
                                        <asp:Label ID="t1ValueLabel" runat="server" Visible="false" Text='<%# Eval("t1Value")%>'></asp:Label>
                                        
                                                <td style="text-align: right;"><%#Eval("t2Value") %></td>
                                             
                                        <asp:Label ID="t2ValueLabel" runat="server" Visible="false" Text='<%# Eval("t2Value")%>'></asp:Label>
                                         
                                                <td style="text-align: right;"><%#Eval("t3Value") %></td>
                                          
                                        <asp:Label ID="t3ValueLabel" runat="server" Visible="false" Text='<%# Eval("t3Value")%>'></asp:Label>
                                       
                                                <td style="text-align: right;"><%#Eval("t4Value") %></td>
                                              
                                        <asp:Label ID="t4ValueLabel" runat="server" Visible="false" Text='<%# Eval("t4Value")%>'></asp:Label>
                                      
                                                <td style="text-align: right;"><%#Eval("t5Value") %></td>
                                            
                                        <asp:Label ID="t5ValueLabel" runat="server" Visible="false" Text='<%# Eval("t5Value")%>'></asp:Label>
                                        
                                                <td style="text-align: right;"><%#Eval("t6Value") %></td>
                                            
                                        <asp:Label ID="t6ValueLabel" runat="server" Visible="false" Text='<%# Eval("t6Value")%>'></asp:Label>
                                       
                                                <td style="text-align: right;"><%#Eval("t7Value") %></td>
                                             
                                        <asp:Label ID="t7ValueLabel" runat="server" Visible="false" Text='<%# Eval("t7Value")%>'></asp:Label>
                                          
                                                <td style="text-align: right;"><%#Eval("t8Value") %></td>
                                          
                                        <asp:Label ID="t8ValueLabel" runat="server" Visible="false" Text='<%# Eval("t8Value")%>'></asp:Label>
                                         
                                                <td style="text-align: right;"><%#Eval("t9Value") %></td>
                                          
                                        <asp:Label ID="t9ValueLabel" runat="server" Visible="false" Text='<%# Eval("t9Value")%>'></asp:Label>
                                     
                                                <td style="text-align: right;"><%#Eval("t10Value") %></td>
                                        
                                        <asp:Label ID="t10ValueLabel" runat="server" Visible="false" Text='<%# Eval("t10Value")%>'></asp:Label>
                                        
                                                <td style="text-align: right;"><%#Eval("t11Value") %></td>
                                           
                                        <asp:Label ID="t11ValueLabel" runat="server" Visible="false" Text='<%# Eval("t11Value")%>'></asp:Label>
                                      
                                                <td style="text-align: right;"><%#Eval("t12Value") %></td>
                                          
                                        <asp:Label ID="t12ValueLabel" runat="server" Visible="false" Text='<%# Eval("t12Value")%>'></asp:Label>
                                     
                                                <td style="text-align: right;"><%#Convert.ToDecimal(Eval("t1Value"))+Convert.ToDecimal(Eval("t2Value"))
                                                                               +Convert.ToDecimal(Eval("t3Value"))+Convert.ToDecimal(Eval("t4Value"))
                                                                               +Convert.ToDecimal(Eval("t5Value"))+Convert.ToDecimal(Eval("t6Value"))
                                                                               +Convert.ToDecimal(Eval("t7Value"))+Convert.ToDecimal(Eval("t8Value"))
                                                                               +Convert.ToDecimal(Eval("t9Value"))+Convert.ToDecimal(Eval("t10Value"))
                                                                               +Convert.ToDecimal(Eval("t11Value"))+Convert.ToDecimal(Eval("t12Value"))%></td>
                                            
                                        <asp:Label ID="GrandTotalLabel" runat="server" Visible="false" Text='<%#Convert.ToDecimal(Eval("t1Value"))+Convert.ToDecimal(Eval("t2Value"))
                                                                               +Convert.ToDecimal(Eval("t3Value"))+Convert.ToDecimal(Eval("t4Value"))
                                                                               +Convert.ToDecimal(Eval("t5Value"))+Convert.ToDecimal(Eval("t6Value"))
                                                                               +Convert.ToDecimal(Eval("t7Value"))+Convert.ToDecimal(Eval("t8Value"))
                                                                               +Convert.ToDecimal(Eval("t9Value"))+Convert.ToDecimal(Eval("t10Value"))
                                                                               +Convert.ToDecimal(Eval("t11Value"))+Convert.ToDecimal(Eval("t12Value"))%>'></asp:Label>
                                          
                                            </tr>--%>
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
        </div>

    </section>
</asp:Content>
