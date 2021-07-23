<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="UnlockDSR.aspx.cs" Inherits="AstralFFMS.UnlockDSR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>

  <script type="text/javascript">
      $(function () {
          $('[id*=lstSalesPerson]').multiselect({
              enableCaseInsensitiveFiltering: true,
              buttonWidth: '100%',
              includeSelectAllOption: true,
              maxHeight: 200,
              width: 215,
              enableFiltering: true,
              filterPlaceholder: 'Search'
          });
      });
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
            width: 100%;
        }

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }

      
    </style>

     <script type="text/javascript">
         var V1 = "";
         function errormessage(V1) {
             $("#messageNotification").jqxNotification({
                 width: 250, position: "top-right", opacity: 2,
                 autoOpen: false, animationOpenDelay: 1500, autoClose: true, autoCloseDelay: 3800, template: "error"
             });
             $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
    </script>

    <script type="text/javascript">
        var V = "";
        function Successmessage(V) {
            $("#messageNotification").jqxNotification({
                width: 250, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 1500, autoClose: true, autoCloseDelay: 3800, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>

    <script type="text/javascript">
        function checkDate(sender, args) {
            if (sender._selectedDate > new Date()) {
                errormessage("You cannot select a day greater than today!");
                sender._selectedDate = new Date();
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
        }
    </script>

     <script type="text/javascript">        

        //valdation check
         function validate() {           
             if ($('#<%=lstSalesPerson.ClientID%>').val() == "" || $('#<%=lstSalesPerson.ClientID%>').val() == null) {               
                errormessage("Please select Sales Person");
                return false;
            }

            if ($('#<%=txtfmDate.ClientID%>').val() == "") {               
                errormessage("Please select Date");
                return false;
            }

             ConfirmUnlock();
        }
    </script>

     <script type="text/javascript">
         function ConfirmUnlock() {
             var confirm_value = document.createElement("INPUT");
             confirm_value.type = "hidden";
             confirm_value.name = "confirm_value";
             if (confirm("Are you sure to UNLock?")) {
                 confirm_value.value = "Yes";
             } else {
                 confirm_value.value = "No";
             }
             document.forms[0].appendChild(confirm_value);
         }
    </script>

    <section class="content">       
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <!-- left column -->
            <div class="col-md-12">
                <div id="InputWork">
                    <!-- general form elements -->
                    <div class="box box-primary">
                        <div class="box-header with-border">
                            <h3 class="box-title">Unlock DSR</h3>
                            <div style="float: right">
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                                <div class="col-lg-9 col-md-8 col-sm-7 col-xs-9">
                                    <div class="row">
                                         <div class="col-md-4 col-sm-6 col-xs-12" >
                                            <input type="hidden" id="hdnid" />
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <span class="">
                                               <%-- <asp:DropDownList Width="100%" ID="ddlprimecode" class="form-control" runat="server"></asp:DropDownList>--%>
                                                 <asp:ListBox ID="lstSalesPerson" runat="server" SelectionMode="Single"></asp:ListBox>
                                            </span>

                                        </div>
                                         <div class="col-md-4 col-sm-6 col-xs-12" id="divparent" runat="server" style="display: block;">

                                            <label for="exampleInputEmail1">DSR Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>     
                                               <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" OnClientDateSelectionChanged="checkDate" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />                              

                                        </div>                                       
                                    </div>                                    
                                </div>                                                  

                            </div>
                        </div>

                        <div class="box-footer">
                            <asp:Button ID="btnUnlock" runat="server" CssClass="btn btn-primary" Text="Unlock DSR" OnClick="btnUnlock_Click" OnClientClick="javascript:return validate();"/>
                            <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn btn-primary"  />
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
    </asp:Content>