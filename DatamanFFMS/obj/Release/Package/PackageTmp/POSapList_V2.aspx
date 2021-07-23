<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="POSapList_V2.aspx.cs" Inherits="AstralFFMS.POSapList_V2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

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
   </script>
    <script type="text/javascript">
        $(function () {
            //$("#example1").DataTable({
            //    "order": [[1, "desc"]]
            //});
        });
    </script>
    <style type="text/css">
        .containerStaff {
            border: 1px solid #ccc;
            overflow-y: auto;
            min-height: 200px;
            width: 134%;
            overflow-x: auto;
        }
        .multiselect-container > li {
            width: 240px;
        }
        .multiselect-container > li > a {
            white-space: normal;
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

         input[type=checkbox], input[type=radio] {
    margin-right: 6px ;
    margin-left: 6px ;
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

          function btnSubmitfunc() {
              var selectedvalue = [];
              $("#<%=trview.ClientID %> :checked").each(function () {
                selectedvalue.push($(this).val());
            });
            if (selectedvalue == "") {
                errormessage("Please Select Sales Person");
                return false;
            }
            // $("#hiddistributor").val(selectedvalue);
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
          return true;
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
        <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <%--<h3 class="box-title"> Pending Orders</h3>--%>
                            <img id="img-Header" src="img/not-working.png" style="width: 50px;height: 43px;"/>
                            <h2 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h2>
                        </div>

                         <div class="box-body" id="div1">
                                <div class="row">                                    
                                     <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div class="form-group" hidden>
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />                                           
                                            <asp:ListBox ID="salespersonListBox" runat="server" SelectionMode="Multiple"
                                                OnSelectedIndexChanged="salespersonListBox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                         <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />                                           
                                          <b>  <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged"></asp:TreeView></b>
                                        </div>
                                    </div>
                                     <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Distributor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>           
                                                <asp:ListBox ID="ListBox1" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                                <input type="hidden" id="hiddistributor" />
                                                
                                        </div>
                                    </div>
                                      <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                        </div>
                                    </div>                                 
                                     <div class="col-md-3 col-sm-3 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                        </div>
                                    </div>
                                </div>
                              
                            </div>

                            <div class="box-footer">
                                
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Generate" class="btn btn-primary button1" OnClientClick="javascript:return btnSubmitfunc()" 
                                    OnClick="btnGo_Click"  />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary button1"
                                    OnClick="btnCancel_Click" />
                                
                            </div>


                        <!-- /.box-header -->
                        
                         <br />
                                <div>
                                    <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                                </div>
                        <!-- /.box-body -->
                    </div>
                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>

        </div>
    </section>
   

</asp:Content>
