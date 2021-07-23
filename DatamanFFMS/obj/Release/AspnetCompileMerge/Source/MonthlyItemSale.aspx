<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MonthlyItemSale.aspx.cs" Inherits="AstralFFMS.MonthlyItemSale" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=ListBox2]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',GetMonthlyItemSaleGetMonthlyItemSale
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
    </script>
  <%--  <script type="text/javascript">
        $(function () {
            $("#monthlyItemSale").DataTable();
        });
    </script>--%>
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
        

          function loding() {
              $('#spinner').show();
          }
    </script>

     <script type="text/javascript">
         function fireCheckChanged(e) {
             var ListBox1 = document.getElementById('<%= trview.ClientID %>');
                var evnt = ((window.event) ? (event) : (e));
                var element = evnt.srcElement || evnt.target;

                if (element.tagName == "INPUT" && element.type == "checkbox") {
                    __doPostBack("", "");
                }
            }
         $(document).ready(function () {
             //Hide the div          
             $('div[id$="rptDiv"]').hide();
             //conversely do the following to show it again if needed later
             //$('#showdiv').show();
         });

</script>
    <style type="text/css">
        #ContentPlaceHolder1_approveStatusRadioButtonList_0 {
            margin-right: 5px !important;
            margin-top: 3px;
        }

        #ContentPlaceHolder1_approveStatusRadioButtonList_1 {
            margin-right: 5px !important;
            margin-top: 3px;
        }

        #ContentPlaceHolder1_approveStatusRadioButtonList td {
            padding: 3px;
        }
        #ContentPlaceHolder1_CheckBoxList1 td {
            padding: 0 15px;
        }
         #ContentPlaceHolder1_CheckBoxList1_2, #ContentPlaceHolder1_CheckBoxList1_1, #ContentPlaceHolder1_CheckBoxList1_0 {
             margin:0 5px;
        }

        .input-group .form-control {
            height: 34px;
        }
        .table-bordered > tbody > tr > td {
            white-space:nowrap;
        }
         .table-bordered > thead > tr > th {
            white-space:nowrap;
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
             $('[id*=listboxmonth]').multiselect({
                 enableCaseInsensitiveFiltering: true,
                 buttonWidth: '70%',
                 includeSelectAllOption: true,
                 maxHeight: 200,
                 width: 100,
                 //enableFiltering: true,
                 //filterPlaceholder: 'Search'
             });
         });
    </script>
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
             $("#<%=ListBox2.ClientID %> :selected").each(function () {
                 selectedValues.push($(this).val());
             });
             $("#hiddistributor").val(selectedValues);
             if (selectedValues == "") {
                 errormessage("Please Select Distributor");
                 return false;
             }
            var YearValues = [];
            $("[id*=CheckBoxList1] input:checked").each(function () {
                YearValues.push($(this).val());
            });
            $("#hidyear").val(YearValues);

            if (YearValues.length == "0") {
                errormessage("Please select year.");                
                return false;
            }
            if (YearValues.length > "1") {
                errormessage("Please select only one year.");
                return false;
            }

            var selectedValues = [];
            $("#<%=listboxmonth.ClientID %> :selected").each(function () {
               selectedValues.push($(this).val());
           });
            $("#hidmonth").val(selectedValues);

            var selectedValues1 = [];
            $("#<%=listboxmonth.ClientID %> :selected").each(function () {
                 selectedValues1.push($(this).text());
             });
            $("#hidmonthtext").val(selectedValues1);

            var checked_radio = $("[id*=approveStatusRadioButtonList] input:checked");
            var value = checked_radio.val();
            //alert(value);
            $("#hidview").val(value);

          loding();
          BindGridView();
      }

      function BindGridView() {
          $.ajax({
              type: "POST",
              url: "MonthlyItemSale.aspx/GetMonthlyItemSale",
              data: '{Distid: "' + $("#hiddistributor").val() + '", year: "' + $("#hidyear").val() + '",  MonthText: "' + $("#hidmonthtext").val() + '",Month: "' + $("#hidmonth").val() + '", View: "' + $("#hidview").val() + '"}',
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
            // alert(JSON.stringify(response.d));
            // alert(response.d);
            $('div[id$="rptDiv"]').show();
             var data = JSON.parse(response.d);
            //alert(data);
            var arr1 = data.length;
            //alert(arr1);

            var table = $('#ContentPlaceHolder1_rptDiv table').DataTable();
            table.destroy();

            $("#ContentPlaceHolder1_rptDiv table ").DataTable({
                "order": [[0, "asc"]],

                "aaData": data,
                "aoColumns": [
            { "mData": "SyncId" },
            { "mData": "distributor" }, // <-- which values to use inside object
            { "mData": "MaterialGROUP" },
            { "mData": "Item" },
            { "mData": "Year" },
            { "mData": "Jan" },
            { "mData": "Feb" },
            { "mData": "Mar" },
            { "mData": "Apr" },
            { "mData": "May" },
            { "mData": "Jun" },
            { "mData": "Jul" },
            { "mData": "Aug" },
            { "mData": "Sep" },
            { "mData": "Oct" },
            { "mData": "Nov" },
            { "mData": "Dec" },
             { "mData": "d" }
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
                                <h3 class="box-title">Monthly Item Sale</h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">                                   
                                    <div class="col-md-4 col-sm-6 col-xs-12">
                                        <div class="form-group" hidden>
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                          
                                            <asp:ListBox ID="ListBox1" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                          
                                            <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged"  ShowCheckBoxes="All"></asp:TreeView>                                             
                                             
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Distributor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                            
                                            <asp:ListBox ID="ListBox2" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                            <input type="hidden" id="hiddistributor" /> 
                                            <input type="hidden" id="hidyear" />
                                            <input type="hidden" id="hidmonth" />
                                            <input type="hidden" id="hidview" />
                                            <input type="hidden" id="hidmonthtext" />                                           
                                        </div>
                                    </div>
                                </div>                               
                                <div class="row">
                                     <div class="col-md-4 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Financial Year:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:CheckBoxList ID="CheckBoxList1" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged" >
                                            </asp:CheckBoxList> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <%--<asp:DropDownList ID="ddlMonth" runat="server"></asp:DropDownList>       --%>                                    
                                        </div>
                                         
                                    </div>                                              

                                    <div class="col-md-4 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">View As:</label>
                                            <asp:RadioButtonList ID="approveStatusRadioButtonList" RepeatDirection="Horizontal" runat="server">
                                                <asp:ListItem Selected="True" Value="Quantity" Text="Quantity"></asp:ListItem>
                                                <asp:ListItem Value="Amount" Text="Amount"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4 col-sm-6 col-xs-12">
                                         <div class="form-group">
                                             <label for="exampleInputEmail1">Months:</label>
                                             <asp:ListBox ID="listboxmonth" runat="server" SelectionMode="Multiple"
                                                ></asp:ListBox>
                                             <%--<asp:DropDownList ID="ddlMonth" runat="server"></asp:DropDownList>--%>       
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
                                    OnClick="btnExport_Click" />
                            </div>
                            
                            <div id="rptDiv" runat="server">
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="monthlyItemRpt" runat="server">
                                        <HeaderTemplate>
                                            <table id="monthlyItemSale" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <%--<th style="text-align:center;width:6%;">SNo.</th>--%>
                                                        <th style="text-align:left">Distributor Sync Id</th>   
                                                        <th>Distributor Name</th>
                                                        <th>Product Group</th>
                                                        <th>Product</th>
                                                        <th>Year</th>
                                                        <th>April</th>
                                                        <th>May</th>
                                                        <th>Jun</th>
                                                        <th>Jul</th>
                                                        <th>Aug</th>
                                                        <th>Sep</th>
                                                        <th>Oct</th>
                                                        <th>Nov</th>
                                                        <th>Dec</th>
                                                        <th>Jan</th>
                                                        <th>Feb</th>
                                                        <th>Mar</th>
                                                        <th>Total</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                          <%--  <tr>                                             
                                                <td><%#Eval("distributor") %></td>
                                                
                                        <asp:Label ID="distributorLabel" runat="server" Visible="false" Text='<%# Eval("distributor")%>'></asp:Label>
                                        
                                                <td><%#Eval("MaterialGROUP") %></td>
                                               
                                        <asp:Label ID="MaterialGROUPLabel" runat="server" Visible="false" Text='<%# Eval("MaterialGROUP")%>'></asp:Label>
                                       
                                                <td><%#Eval("Item") %></td>
                                            
                                        <asp:Label ID="ItemLabel" runat="server" Visible="false" Text='<%# Eval("Item")%>'></asp:Label>
                                            <
                                                <td><%#Eval("Year") %></td>
                                               
                                        <asp:Label ID="YearLabel" runat="server" Visible="false" Text='<%# Eval("Year")%>'></asp:Label>
                                         
                                                <td><%#Eval("t1Value") %></td>
                                                
                                        <asp:Label ID="t1ValueLabel" runat="server" Visible="false" Text='<%# Eval("t1Value")%>'></asp:Label>
                                           
                                                <td><%#Eval("t2Value") %></td>
                                            
                                        <asp:Label ID="t2ValueLabel" runat="server" Visible="false" Text='<%# Eval("t2Value")%>'></asp:Label>
                                         
                                                <td><%#Eval("t3Value") %></td>
                                           
                                        <asp:Label ID="t3ValueLabel" runat="server" Visible="false" Text='<%# Eval("t3Value")%>'></asp:Label>
                                      
                                                <td><%#Eval("t4Value") %></td>
                                         
                                        <asp:Label ID="t4ValueLabel" runat="server" Visible="false" Text='<%# Eval("t4Value")%>'></asp:Label>
                                          
                                                <td><%#Eval("t5Value") %></td>
                                             
                                        <asp:Label ID="t5ValueLabel" runat="server" Visible="false" Text='<%# Eval("t5Value")%>'></asp:Label>
                                        
                                                <td><%#Eval("t6Value") %></td>
                                          
                                        <asp:Label ID="t6ValueLabel" runat="server" Visible="false" Text='<%# Eval("t6Value")%>'></asp:Label>
                                      
                                                <td><%#Eval("t7Value") %></td>
                                              
                                        <asp:Label ID="t7ValueLabel" runat="server" Visible="false" Text='<%# Eval("t7Value")%>'></asp:Label>
                                     
                                                <td><%#Eval("t8Value") %></td>
                                              
                                        <asp:Label ID="t8ValueLabel" runat="server" Visible="false" Text='<%# Eval("t8Value")%>'></asp:Label>
                                         
                                                <td><%#Eval("t9Value") %></td>
                                            
                                        <asp:Label ID="t9ValueLabel" runat="server" Visible="false" Text='<%# Eval("t9Value")%>'></asp:Label>
                                
                                                <td><%#Eval("t10Value") %></td>
                                              
                                        <asp:Label ID="t10ValueLabel" runat="server" Visible="false" Text='<%# Eval("t10Value")%>'></asp:Label>
                                        
                                                <td><%#Eval("t11Value") %></td>
                                            
                                        <asp:Label ID="t11ValueLabel" runat="server" Visible="false" Text='<%# Eval("t11Value")%>'></asp:Label>
                                           
                                                <td><%#Eval("t12Value") %></td>
                                            
                                        <asp:Label ID="t12ValueLabel" runat="server" Visible="false" Text='<%# Eval("t12Value")%>'></asp:Label>
                                    
                                                <td style="text-align: right;"><%#Convert.ToDecimal(Eval("t1Value"))+Convert.ToDecimal(Eval("t2Value"))
                                                                               +Convert.ToDecimal(Eval("t3Value"))+Convert.ToDecimal(Eval("t4Value"))
                                                                               +Convert.ToDecimal(Eval("t5Value"))+Convert.ToDecimal(Eval("t6Value"))
                                                                               +Convert.ToDecimal(Eval("t7Value"))+Convert.ToDecimal(Eval("t8Value"))
                                                                               +Convert.ToDecimal(Eval("t9Value"))+Convert.ToDecimal(Eval("t10Value"))
                                                                               +Convert.ToDecimal(Eval("t11Value"))+Convert.ToDecimal(Eval("t12Value"))%></td>
                                           
                                        <asp:Label ID="TotalLabel" runat="server" Visible="false" Text='<%#Convert.ToDecimal(Eval("t1Value"))+Convert.ToDecimal(Eval("t2Value"))
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
