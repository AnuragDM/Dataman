<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="SaleValueBreakupReport.aspx.cs" Inherits="AstralFFMS.SaleValueBreakupReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
 <%--   <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
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
   
   <%-- <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({ "order": [[0, "desc"]] });
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
        function loding() {
            $('#spinner').show();
        }
      </script> 
    <style type="text/css">
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
            width: 212px;
        }
         .multiselect-container.dropdown-menu {
            width: 100% !important;
        }


        .input-group .form-control {
            height: 34px;
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
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
        function fireCheckChanged(e) {
            var ListBox1 = document.getElementById('<%= trview.ClientID %>');
            var evnt = ((window.event) ? (event) : (e));
            var element = evnt.srcElement || evnt.target;

            if (element.tagName == "INPUT" && element.type == "checkbox") {
                __doPostBack("", "");
            }
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
            // validate($("#hiddistributor").val());
             if (selectedValues == "") {
                 errormessage("Please Select Distributor");
                 return false;
             }
            var selectedValues = [];
            $("#<%=matGrpListBox.ClientID %> :selected").each(function () {
                selectedValues.push($(this).val());
            });
            $("#hidproductgroup").val(selectedValues);           
            
            //if (selectedValues == "") {
            //    errormessage("Please Select Product group");
            //    return false;
            //}
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
                url: "SaleValueBreakupReport.aspx/GetSaleValue",
                <%--data: '{ ProductGroup: "' + $("#hidproductgroup").val() + '" , Fromdate: "' + $('#<%=txtfmDate.ClientID%>').val() + '",Todate: "' + $('#<%=txttodate.ClientID%>').val() + '"}',                --%>
                data: '{Distid: "' + $("#hiddistributor").val() + '", ProductGroup: "' + $("#hidproductgroup").val() + '" ,Product: "' + $("#hidproduct").val() + '", Fromdate: "' + $('#<%=txtfmDate.ClientID%>').val() + '",Todate: "' + $('#<%=txttodate.ClientID%>').val() + '"}',                
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
          $("#ContentPlaceHolder1_rptmain table ").DataTable({
              "order": [[0, "desc"]],

              "aaData": data,
              "aoColumns": [
          {
              "mData": "VDate",
              "render": function (data, type, row, meta) {

                  var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                  var date = new Date(data);
                  var day = date.getDate();
                  var month = date.getMonth();
                  var year = date.getFullYear();

                  var mname = monthNames[date.getMonth()]

                  var fdate = day + '/' + mname + '/' + year;

                  if (type === 'display') {
                      return $('<div>')
                         .attr('class', 'text')
                         .text(fdate)
                         .wrap('<div></div>')
                         .parent()
                         .html();

                  } else {
                      return data;
                  }
              }
          }, // <-- which values to use inside object
           { "mData": "Docid" },
          //{ "mData": "Name" },
          //{ "mData": "SyncId" },
          { "mData": "DistributorName" },
          { "mData": "MaterialGroup" },
          { "mData": "ItemName" },
           { "mData": "Item_Syncid" },
          //{
          //    "mData": "CurrentMonthAmount",
          //    "render": function (data, type, row, meta) {
          //        if (type === 'display') {
          //            return $('<div>')
          //               .attr('class', 'text-right')
          //               .text(data)
          //               .wrap('<div></div>')
          //               .parent()
          //               .html();

          //        } else {
          //            return data;
          //        }
          //    }
          //},
          //{
          //    "mData": "Discount",
          //    "render": function (data, type, row, meta) {
          //        if (type === 'display') {
          //            return $('<div>')
          //               .attr('class', 'text-right')
          //               .text(data)
          //               .wrap('<div></div>')
          //               .parent()
          //               .html();

          //        } else {
          //            return data;
          //        }
          //    }
          //},
          {
              "mData": "NetAmount",
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
                              <%--  <h3 class="box-title">Sale Value Breakup Report</h3>--%>
                                <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">                                  
                                     <div class="col-md-4 col-sm-6 col-xs-12">
                                       <%-- <div class="form-group" hidden>
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />                                            
                                            <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </div>--%>
                                          <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />                                            
                                            <asp:TreeView ID="trview" runat="server" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged"  CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>
                                        </div>
                                    </div> 
                                      <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Distributor name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                                
                                                <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                 <input type="hidden" id="hiddistributor" />
                                                <input type="hidden" id="hidproductgroup" />
                                                <input type="hidden" id="hidproduct" />
                                            </div>                                           
                                        </div>                                                                     
                                    
                                </div>
                                 <div class="row">                                   
                                     <div class="col-md-3 col-sm-6 col-xs-12">
                                        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                             <ContentTemplate>--%>
                                         <div class="form-group">
                                             <label for="exampleInputEmail1">Product Group:</label>                                                  
                                             <asp:ListBox ID="matGrpListBox" runat="server" SelectionMode="Multiple"
                                                 OnSelectedIndexChanged="matGrpListBox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>                                                                                     
                                       </div>
                                             <%-- </ContentTemplate>
                                          </asp:UpdatePanel>--%>
                                    </div> 
                                        <div class="col-md-1"></div>
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Product:</label>                                               
                                                <asp:ListBox ID="productListBox" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                            </div>
                                        </div>                                       
                                    </div>
                           
                                <div class="row">
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-1"></div>
                                   <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <div class="box-footer">
                                <input type="button" runat="server" onclick="btnSubmitfunc();" class="btn btn-primary" value="Go" />  
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClientClick="loding()"  
                                    OnClick="btnGo_Click" Visible="false"/>
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                    OnClick="btnCancel_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                    OnClick="btnExport_Click" />
                            </div>
                          
                       
                            <div id="rptmain" runat="server">
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="salevaluerpt" runat="server">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                       <%-- <th style="text-align: left; width: 6%">SNo.</th>--%>
                                                         <th style="text-align: left; width: 18%">Date</th>
                                                         <th style="text-align: left; width: 18%">Doc Id</th>
                                                       <%-- <th style="text-align: left; width: 18%">Sales Person</th>
                                                         <th style="text-align: left; width: 18%">Sync Id</th>--%>
                                                        <th style="text-align: left; width: 18%">Distributor Name</th>
                                                        <th style="text-align: left; width: 18%">Product Group</th>
                                                        <th style="text-align: left; width: 18%">Product</th>
                                                        <th style="text-align: right; width: 18%">Item Syncid</th>
                                                        <th style="text-align: right; width: 18%">Net Amount</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%--<tr>                                               
                                                 <td style="text-align: left; width: 18%"><%#Convert.ToDateTime(Eval("VDate")).ToString("dd/MMM/yyyy") %></td>
                                              
                                          <asp:Label ID="VDateLabel" runat="server" Visible="false" Text='<%# Eval("VDate")%>'></asp:Label>
                                        
                                                <td style="text-align: left; width: 18%"><%#Eval("Name") %></td>
                                               
                                        <asp:Label ID="NameLabel" runat="server" Visible="false" Text='<%# Eval("Name")%>'></asp:Label>
                                        
                                                 <td style="text-align: left; width: 18%"><%#Eval("SyncId") %></td>
                                              
                                        <asp:Label ID="SyncIdLabel" runat="server" Visible="false" Text='<%# Eval("SyncId")%>'></asp:Label>
                                         
                                                <th style="text-align: left; width: 18%"><%#Eval("DistributorName") %></th>
                                            
                                        <asp:Label ID="DistributorNameLabel" runat="server" Visible="false" Text='<%# Eval("DistributorName")%>'></asp:Label>
                                          
                                                <td style="text-align: left; width: 18%"><%#Eval("MaterialGroup") %></td>
                                               
                                        <asp:Label ID="MaterialGroupLabel" runat="server" Visible="false" Text='<%# Eval("MaterialGroup")%>'></asp:Label>
                                        
                                                <td style="text-align: left; width: 18%"><%#Eval("ItemName") %></td>
                                            
                                        <asp:Label ID="ItemNameLabel" runat="server" Visible="false" Text='<%# Eval("ItemName")%>'></asp:Label>
                                           
                                                <td style="text-align: right; width: 18%"><%#Eval("CurrentMonthAmount") %></td>
                                              
                                        <asp:Label ID="CurrentMonthAmountLabel" runat="server" Visible="false" Text='<%# Eval("CurrentMonthAmount")%>'></asp:Label>
                                        
                                                <td style="text-align: right; width: 18%"><%#Eval("Discount") %></td>
                                               
                                        <asp:Label ID="DiscountLabel" runat="server" Visible="false" Text='<%# Eval("Discount")%>'></asp:Label>
                                        
                                                <td style="text-align: right; width: 18%"><%#Eval("NetAmount") %></td>
                                              
                                        <asp:Label ID="NetAmountLabel" runat="server" Visible="false" Text='<%# Eval("NetAmount")%>'></asp:Label>
                                         
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
