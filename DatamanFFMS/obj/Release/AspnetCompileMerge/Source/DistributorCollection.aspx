<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="DistributorCollection.aspx.cs" Inherits="AstralFFMS.DistributorCollection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--  <script src="plugins/select2/select2.js"></script>
    <link href="plugins/select2/select2.css" rel="stylesheet" />--%>
    <script type="text/javascript">
        //$(function () {
        //    $(".select2").select2();
        //});
    </script>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            /*display: table;*/
        }
    </style>
    <style type="text/css">
        .completionList {
            border: solid 1px Gray;
            margin: 0px;
            padding: 3px;
            overflow: auto;
            overflow-y: scroll;
            background-color: #FFFFFF;
            max-height: 180px;
        }

        .listItem {
            color: #191919;
        }

        .itemHighlighted {
            background-color: #ADD6FF;
        }
    </style>
    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "error"
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
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");
        }
    </script>
    <script type="text/javascript">
        function validate() {
            if ($('#<%=txtdocumentdate.ClientID%>').val() == "") {
                errormessage("Please selec the Document Date.");
                return false;
            }
            if ($('#<%=txtdistName.ClientID%>').val() == "") {
                errormessage("Please enter Distributer Name.");
                return false;
            }
            if ($('#<%=hfCustomerId.ClientID%>').val() == "") {
                errormessage("Please select the Distributer Name from list.");
                return false;
            }
            if ($('#<%=txtAmount.ClientID%>').val() == "") {

                errormessage("Please enter the Amount.");
                return false;


            }
            if ($('#<%=txtAmount.ClientID%>').val() <= 0) {
                errormessage("Please enter Amount greater than 0.");
                return false;
            }

            var AspRadio = document.getElementById('<%= RadioButtonList1.ClientID %>');
            var AspRadio_ListItem = document.getElementsByTagName('input');
            for (var i = 0; i < AspRadio_ListItem.length; i++) {

                if (AspRadio_ListItem[i].checked) {
                    var lblAspradiobuttonValue = document.getElementById('<%= RadioButtonList1.ClientID %>');

                    if (AspRadio_ListItem[i].value == "Cash") {
                        if ($('#<%=txtRemark.ClientID%>').val() == "") {
                            errormessage("Please enter the Remark");
                            return false;
                        }

                    }
                    else {
                        if ($('#<%=txtCHDDNO.ClientID%>').val() == "") {
                            errormessage("Please enter the Instrument No.");
                            return false;
                        }
                        if ($('#<%=txCheqDate.ClientID%>').val() == "") {
                            errormessage("Please enter the Instrument date.");
                            return false;
                        }
                        if ($('#<%=txtbank.ClientID%>').val() == "") {
                            errormessage("Please enter the Bank Name.");
                            return false;
                        }
                        if ($('#<%=txtbranch.ClientID%>').val() == "") {
                            errormessage("Please enter the Branch Name.");
                            return false;
                        }
                        if ($('#<%=txtRemark.ClientID%>').val() == "") {
                            errormessage("Please enter the Remark");
                            return false;
                        }

                    }
                }
                <%-- if ($('#<%=txtRemark.ClientID%>').val() == "") {
                    errormessage("Please enter the Remark");
                    return false;
                }--%>



                <%--  var value = ($('#<%=txtdistName.ClientID%>').val().charAt(0));
                var chrcode = value.charCodeAt(0);
                if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                    errormessage("Do not start Name with special characters.")
                    return false;
                }--%>
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            document.getElementById("DivDate").style.display = "";
            document.getElementById("DivChNo").style.display = "";
            document.getElementById("DivBank").style.display = "";
            document.getElementById("DivBranch").style.display = "";
            GetRadioButtonSelectedValue();
            var valLength = "";
            $('#<%=txtdistName.ClientID%>').keypress(function (key) {

                valLength = ($('#<%=txtdistName.ClientID%>').val().length + 1);

                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
        });
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
        function DoNav(depId) {
            if (depId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                GetRadioButtonSelectedValue();
                __doPostBack('', depId)
            }
        }
    </script>

    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfCustomerId.ClientID %>").value = e.get_value();
    }
    </script>
    <script type="text/javascript">
        function SetContextKey() {
            $find('<%=txtdistName_AutoCompleteExtender.ClientID%>').set_contextKey($get("<%=ddlUndeUser.ClientID %>").value);
          }
    </script>

    <script type="text/javascript">
        function GetRadioButtonSelectedValue() {

            var AspRadio = document.getElementById('<%= RadioButtonList1.ClientID %>');
           var AspRadio_ListItem = document.getElementsByTagName('input');
           for (var i = 0; i < AspRadio_ListItem.length; i++) {

               if (AspRadio_ListItem[i].checked) {
                   var lblAspradiobuttonValue = document.getElementById('<%= RadioButtonList1.ClientID %>');

                   if (AspRadio_ListItem[i].value == "Cash") {
                       document.getElementById("DivDate").style.display = "none";
                       document.getElementById("DivChNo").style.display = "none";
                       document.getElementById("DivBank").style.display = "none";
                       document.getElementById("DivBranch").style.display = "none";
                   }
                   else {
                       document.getElementById("DivDate").style.display = "";
                       document.getElementById("DivChNo").style.display = "";
                       document.getElementById("DivBank").style.display = "";
                       document.getElementById("DivBranch").style.display = "";
                   }
               }
           } //end if

       } // end for


    </script>

    <section class="content">

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
                            <h3 class="box-title">Distributor Collection</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                    OnClick="btnFind_Click" />

                                <asp:Button Style="margin-right: 5px;" type="button" ID="Button1" runat="server" Text="Back" class="btn btn-primary"
                                    OnClick="Button1_Click" />
                            </div>
                            <br />
                            <br />
                            <div style="text-align: right">
                                <asp:HyperLink ID="HypLink" runat="server" NavigateUrl="https://www.google.co.in" Target="_blank">HSBC BANK PAYMENT PORTAL LINK</asp:HyperLink>
                                <%--  <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" runat="server" />--%>
                            </div>

                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                                <div class="col-lg-5 col-md-7 col-sm-7 col-xs-11">
                                    <div id="DIVUnder" runat="server" class="form-group">
                                        <label for="exampleInputEmail1">Sales Person:</label>
                                        <asp:DropDownList ID="ddlUndeUser" Width="100%" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>

                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Document Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtdocumentdate" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtdocumentdate" runat="server" />
                                    </div>

                                    <div id="divdocid" runat="server" class="form-group">
                                        <label for="exampleInputEmail1">Document No:</label>
                                        <asp:TextBox ID="lbldocno" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <input id="DepId" hidden="hidden" />
                                        <label for="exampleInputEmail1">Distributor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtdistName" runat="server" onkeyup="SetContextKey();" MaxLength="6" class="form-control"></asp:TextBox>
                                        <ajaxToolkit:AutoCompleteExtender ID="txtdistName_AutoCompleteExtender" CompletionListCssClass="completionList"
                                            CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted" runat="server" FirstRowSelected="false" OnClientItemSelected="ClientItemSelected"
                                            UseContextKey="true" DelimiterCharacters="" ServiceMethod="SearchItem" ServicePath="DistributorCollection.aspx" MinimumPrefixLength="2" EnableCaching="true" TargetControlID="txtdistName">
                                        </ajaxToolkit:AutoCompleteExtender>
                                        <asp:HiddenField ID="hfCustomerId" runat="server" />
                                        <asp:HiddenField ID="hfname" runat="server" />
                                    </div>

                                    <div class="form-group col-md-6 small paddingleft0">
                                        <label for="exampleInputEmail1">Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtAmount" MaxLength="12" runat="server" CssClass="form-control numeric  text-right"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-md-6 small paddingleft0">
                                        <label for="exampleInputEmail1">Payment Mode:</label>
                                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal" onchange="GetRadioButtonSelectedValue();" CssClass="radiogroup">
                                            <asp:ListItem Text="Cheque" Value="Cheque" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="DD" Value="DD"></asp:ListItem>
                                            <asp:ListItem Text="Cash" Value="Cash"></asp:ListItem>
                                            <asp:ListItem Text="RTGS" Value="RTG"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div id="DivChNo" style="display: none" class="form-group col-md-6 paddingleft0">
                                        <label for="exampleInputEmail1">Instrument No.:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtCHDDNO" MaxLength="25" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div id="DivDate" style="display: none" class="form-group col-md-6 paddingright0">
                                        <label for="exampleInputEmail1">Instrument Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txCheqDate" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="calendarTextBox_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender"
                                            TargetControlID="txCheqDate"></ajaxToolkit:CalendarExtender>
                                    </div>
                                    <div id="DivBank" style="display: none" class="form-group">
                                        <label for="exampleInputEmail1">Bank:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtbank" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div id="DivBranch" style="display: none" class="form-group">
                                        <label for="exampleInputEmail1">Branch:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtbranch" MaxLength="100" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Remark:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtRemark" Style="resize: none; height: 20%;" MaxLength="150" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" OnClick="btnDelete_Click" />
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
                            <h3 class="box-title">Distributor Collection List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <div class="col-lg-9 col-md-8 col-sm-7 col-xs-9">

                                <div id="DIV1" class="form-group col-md-4">
                                    <label for="exampleInputEmail1">From Date:</label>
                                    <asp:TextBox ID="txtmDate" runat="server" Style="background: white;" CssClass="form-control"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtmDate" runat="server" />
                                </div>
                                <div class="form-group col-md-4 ">
                                    <label for="exampleInputEmail1">To Date:</label>
                                    <asp:TextBox ID="txttodate" runat="server" Style="background: white;" CssClass="form-control"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                </div>
                                <div class="form-group col-md-4">

                                    <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                    <asp:Button type="button" ID="btnGo" runat="server" Text="Go" Style="padding: 3px 14px;" class="btn btn-primary" OnClick="btnGo_Click" />
                                </div>

                            </div>
                        </div>

                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Document Date</th>
                                                <th>Document No.</th>
                                                <th>Distributor Name</th>
                                                <th>Amount</th>
                                                <th>Mode</th>
                                                <th>Instrument No</th>
                                                <th>Instrument Date</th>
                                                <th>Print</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("CollId") %>' />
                                        <td onclick="DoNav('<%#Eval("CollId") %>');"><%#String.Format("{0:dd/MMM/yyyy}",Eval("PaymentDate")) %></td>
                                        <td onclick="DoNav('<%#Eval("CollId") %>');"><%#Eval("CollDocId") %></td>
                                        <td onclick="DoNav('<%#Eval("CollId") %>');"><%#Eval("PartyName") %></td>
                                        <td onclick="DoNav('<%#Eval("CollId") %>');"><%#Eval("Amount") %></td>

                                        <td onclick="DoNav('<%#Eval("CollId") %>');"><%#Eval("Mode") %></td>
                                        <td onclick="DoNav('<%#Eval("CollId") %>');"><%#Eval("Cheque_DDNo") %></td>
                                        <td onclick="DoNav('<%#Eval("CollId") %>');"><%#String.Format("{0:dd/MMM/yyyy}", Eval("Cheque_DD_Date"))%></td>
                                        <td>
                                            <asp:ImageButton ID="ImgPrint" runat="server" ImageUrl="~/img/icon_print.gif" OnClick="ImgPrint_Click" TabIndex="5" />
                                        </td>
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
            $("#example1").DataTable({
                "order": [[0, "desc"]]
            });

        });
    </script>
</asp:Content>
