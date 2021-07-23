<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="BrandSaleReport_V2.aspx.cs" Inherits="AstralFFMS.BrandSaleReport_V2" %>

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
        $(function () {
            $('[id*=LstYear]').multiselect({
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
        /*For everyone */
        input[type=checkbox], input[type=radio] {
    margin-right: 12px;
    margin-left: 7px ;
}
        .button1 {
box-shadow: 0px 2px 4px 2px #888888;
margin-left: 10px;
    margin-top: 7px;
    margin-right: 5px;
}
         h2 {
                 font-size: 20px !important;
    font-weight: 600 !important;
        margin-left: 13px !important;
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
           
          if (selectedValues == "") {
              errormessage("Please Select Distributor");
              return false;
          }
          var productClassValues = [];
          $("#<%=productClassListBox.ClientID %> :selected").each(function () {
              productClassValues.push($(this).val());
              });
           

          var productGroupValues = [];
          $("#<%=matGrpListBox.ClientID %> :selected").each(function () {
                productGroupValues.push($(this).val());
            });
          

             var YearValues = [];
             $("#<%=LstYear.ClientID %> :selected").each(function () {
                 YearValues.push($(this).val());
             });

             if (YearValues.length  == "") {
                 errormessage("Please select year.");
                 return false;
             }
             return true;
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
                                <%--<h3 class="box-title">Brandwise Sale Report</h3>--%>
                                <img id="img-Header" src="img/sale-report.png" style="width:50px;height:50px;"/>
                                <h2 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h2>

                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                       
                                        <div class="form-group">
                                                <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>  
                                              <b>  <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged"></asp:TreeView></b>
                                            </div>
                                    </div>
                                     <div class="col-md-4 col-sm-4 col-xs-12">
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
                                    
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Financial Year:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                              <asp:ListBox ID="LstYear" runat="server" SelectionMode="Single" AutoPostBack="true" class="form-control"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>                             
                                <div class="row">
                                     <div class="col-md-4 col-sm-4 col-xs-12">
                                          <div class="form-group">
                                            <label for="exampleInputEmail1">View As:</label>
                                            <asp:RadioButtonList ID="viewasRadioButtonList" RepeatDirection="Horizontal" runat="server">
                                                <asp:ListItem Selected="True" Value="Quantity" Text="Quantity"></asp:ListItem>
                                                <asp:ListItem Value="Amount" Text="Amount"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                      </div>
                                     <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Product Class:</label>                                           
                                            <asp:ListBox ID="productClassListBox" runat="server" SelectionMode="Multiple"
                                               AutoPostBack="true"></asp:ListBox>
                                        </div>
                                    </div>
                                    
                                   <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Product Group:</label>                                          
                                            <asp:ListBox ID="matGrpListBox" runat="server" SelectionMode="Multiple"
                                                 AutoPostBack="true"></asp:ListBox>
                                        </div>
                                    </div>
                                     
                                    
                                  
                                </div>
                                
                            </div>
                            <div class="box-footer">
                               
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Generate" class="btn btn-primary button1" OnClientClick="javascript:return btnSubmitfunc();"  
                                    OnClick="btnGo_Click"  />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary button1"
                                    OnClick="btnCancel_Click" />
                               
                            </div>
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
