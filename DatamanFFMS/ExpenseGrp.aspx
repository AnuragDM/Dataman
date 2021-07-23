<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ExpenseGrp.aspx.cs" Inherits="AstralFFMS.ExpenseGrp" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <%--  <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style type="text/css">

        .spinner {
            position: absolute;
            left: 50%;
            margin-left: -50px; /* half width of the spinner gif */
            margin-top: -50px; /* half height of the spinner gif */
            text-align: center;
            z-index: 999;
            overflow: auto;
            width: 100px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }
        #select2-ContentPlaceHolder1_ddlParentLoc-container {
        margin-top:-8px !important;
        }
    </style>
    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 6800, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <script type="text/javascript">
        //$(function () {
        //    $(".select2").select2();
        //});
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
        function validate() {
            if ($('#<%=Name.ClientID%>').val() == "") {
                errormessage("Please enter Expense Group Name");
                return false;
            }

            var value = ($('#<%=Name.ClientID%>').val().charAt(0));
            var chrcode = value.charCodeAt(0);
            if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                errormessage("Do not start Name with special characters")
                return false;
            }
            if ($('#<%=EffDateFrom.ClientID%>').val() == "") {
                errormessage("Please select Effective Date From.");
                  return false;
              }
              if ($('#<%=EffDateTo.ClientID%>').val() == "") {
                  errormessage("Please select Effective Date To.");
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            checkforexpensegroup();
            var valLength = "";
            $('#<%=Name.ClientID%>').keypress(function (key) {

                valLength = ($('#<%=Name.ClientID%>').val().length + 1);

                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
        });

        function checkforexpensegroup() {

            $.ajax({
                type: "GET",
                url: '<%= ResolveUrl("And_sync.asmx/JSGetEnviroSetting") %>',
                success: function (data) {

                    jsdata1 = JSON.parse(data);
                    console.log(jsdata1);
                    $.each(jsdata1, function (key1, value1) {
                        var dt = value1.ExpenseGroupMonthWise
                        if (dt == "True") {
                            $('#<%=hdnEnviroValue.ClientID%>').val('1');
                           
                            $('#<%=EffDateTo.ClientID %>').attr("disabled", "disabled");

                        }
                        else {
                            $('#<%=hdnEnviroValue.ClientID%>').val('0');
                           // $('#<%=EffDateTo.ClientID %>').removeAttr("disabled");

                        }

                    });
                },
                error: function (data) {

                    console.log(data);
                }
            });


        }

    </script>
      <script>
          function isNumber(evt) {
              var iKeyCode = (evt.which) ? evt.which : evt.keyCode
              if (!(iKeyCode != 8)) {
                  e.preventDefault();
                  return false;
              }
              return true;
          }
    </script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to delete?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
      
    </script>
    <script type="text/javascript">
        function DoNav(ExpenseGroupId) {
            if (ExpenseGroupId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', ExpenseGroupId)
            }
        }
    </script>

     <script type="text/javascript">
         function checkDate(sender, args) {

             var date = new Date(); // 2012-03-31
             date.setMonth(date.getMonth() - 1);
             if (sender._selectedDate < date) {
                 errormessage("You cannot select a day less than one month from today!");
                 sender._selectedDate = new Date();
                 sender._textbox.set_Value("")
             }
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
        <div class="box-body" id="mainDiv" style="display: none;" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <%--<h3 class="box-title">Add Expense Group</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                   OnClick="btnFind_Click" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                            <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                  <div class="form-group">
                                <input id="ExpenseGroupId" hidden="hidden" />
                                       <asp:HiddenField ID="hdnEnviroValue" runat="server" />
                                <label for="exampleInputEmail1">Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <input type="text" runat="server" class="form-control" maxlength="50" id="Name" placeholder="Enter Expense Group Name" tabindex="2">
                            </div>
                                   
                                       <div class="col-md-6 form-group paddingleft0">
                                            <label for="exampleInputEmail1">Effective Date From:</label>
                                                &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:TextBox ID="EffDateFrom" runat="server" CssClass="form-control"  tabindex="4" OnTextChanged="EffDateFrom_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange"  Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox3_CalendarExtender"
                                                TargetControlID="EffDateFrom"></ajaxToolkit:CalendarExtender>
                                       </div>
                                       <div class="col-md-6 paddingright0 form-group">
                                            <label for="exampleInputEmail1"> To:</label>
                                               &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:TextBox ID="EffDateTo" runat="server" CssClass="form-control"  tabindex="6"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange"  Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox4_CalendarExtender"
                                                TargetControlID="EffDateTo"></ajaxToolkit:CalendarExtender>

                                       </div>
                                
                                          
                           
                          <div class="form-group">
                                <label for="exampleInputEmail1">Remark:</label>
                              <textarea  id="Remarks" runat="server" style="height:50px;" placeholder="Enter Remark" tabindex="7" class="form-control" maxlength="100" ></textarea>
                               </div>
                           
                            </div></div>
                        </div>
                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary"  OnClientClick="javascript:return validate();" TabIndex="28" OnClick="btnSave_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" TabIndex="30" OnClick="btnCancel_Click"/>
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" TabIndex="32" OnClick="btnDelete_Click" />
                        </div>
                          <br />
                           <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>


            </div>

        </div>

        <div class="box-body" id="rptmain" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Expense Active Sheet</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Add Group" class="btn btn-primary" OnClick="btnBack_Click" />

                            </div>
                        </div>
                        <!-- /.box-header --> 
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                               
                                                <th>Group Name</th>
                                                 <th>Effective Date From - To</th>
                                                <th>Remark</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr >
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("ExpenseGroupId") %>' />
                                    <td>
                                       <%-- <asp:LinkButton ID="lnkgrp" runat="server" Text='<%# Eval("GroupName") %>' OnCommand="lnkgrp_Command"
            CommandArgument='<%# Eval("ExpenseGroupId") %>' ToolTip="Add Expenses" ></asp:LinkButton>--%>
                                         <asp:HyperLink runat="server" NavigateUrl='<%# Eval("ExpenseGroupId", "~/ExpenseAdd.aspx?ExpenseGroupId={0}") %>'
                    Text='<%# Eval("GroupName") %>' ToolTip="Add Expenses"/>
                                    </td>
                                       <%-- <td><%#Eval("GroupName") %></td>--%>
                                            <td onclick="DoNav('<%#Eval("ExpenseGroupId") %>');"><%#Eval("datefromto") %></td>
                                        <td onclick="DoNav('<%#Eval("ExpenseGroupId") %>');"><%#Eval("Remarks") %></td>

                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                        <!-- /.box-body -->
                    </div>
                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>

        </div>

    </section>
    
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();
        });
    </script>
</asp:Content>
