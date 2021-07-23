<%@ Page Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ItemDistributorWise_V2.aspx.cs" Inherits="AstralFFMS.ItemDistributorWise_V2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
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

            var productGroupValues = [];
            $("#<%=matGrpListBox.ClientID %> :selected").each(function () {
                productGroupValues.push($(this).val());
            });
           

            var productValues = [];
            $("#<%=productListBox.ClientID %> :selected").each(function () {
              productValues.push($(this).val());
          });
            return true;

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

         function loding() {
             $('#spinner').show();
         }
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
        }f

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
         /*For everyone */
        input[type=checkbox], input[type=radio] {
    margin-right: 12px ;
    margin-left: 12px ;
}
        .button1 {
box-shadow: 0px 2px 4px 2px #888888;
margin-left: 10px;
}
         h2 {
                 font-size: 20px !important;
    font-weight: 600 !important;
        margin-left: 13px !important;
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
                 alert(isValid+"2");
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
                     
                                <div class="box-header with-border">
                                   <%-- <h3 class="box-title">Product Sell Distributer Wise </h3>--%>
                                     <img id="img-Header" src="img/sell.png" style="width: 50px;height: 50px;"/>
                                    <h2 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h2>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="row">
                                    
                                            <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>  
                                                <b><asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged" ></asp:TreeView></b>
                                             
                                            </div>
                                        </div>                                     
                                        <div class="col-md-4 col-sm-4 col-xs-12">
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
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Product Group:</label>                                                
                                                <asp:ListBox ID="matGrpListBox" runat="server" SelectionMode="Multiple"
                                                    OnSelectedIndexChanged="matGrpListBox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                                 

                                            </div>
                                        </div>
                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Product:</label>                                               
                                                <asp:ListBox ID="productListBox" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                            </div>
                                        </div>
                                    </div>
                                   
                                    <div class="row">
                                       <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div id="DIV1" class="form-group">
                                                <label for="exampleInputEmail1">From Date:</label>
                                                <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                            </div>
                                        </div>
                                      
                                       <div class="col-md-4 col-sm-4 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">To:</label>
                                                <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                            </div>
                                        </div>
                                    </div>                                    
                                </div>
                                <div class="box-footer">
                                    
                                    <asp:Button type="button" ID="btnGo" runat="server" Text="Generate" class="btn btn-primary button1" OnClientClick="javascript:return btnSubmitfunc();"  
                                         OnClick="btnGo_Click" />
                                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary button1" OnClick="Cancel_Click" /> 
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

</asp:Content>