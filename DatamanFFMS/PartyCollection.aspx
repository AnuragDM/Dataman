<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="PartyCollection.aspx.cs" EnableEventValidation="false" Inherits="AstralFFMS.PartyCollection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                    }
                }
                <%-- if ($('#<%=txtRemark.ClientID%>').val() == "") {
                    errormessage("Please enter the Remark");
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
            var valLength = "";
            GetRadioButtonSelectedValue();
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
                $('#spinner').show();
                GetRadioButtonSelectedValue();
                __doPostBack('', depId)
            }
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
                    <div class="box box-primary">
                        <div class="box-header">

                         <%--  <div class="box-header with-border">--%>

                                <div class="form-group">

                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                    <div class="col-sm-12">
                                        <div class="col-lg-4 col-md-5 col-sm-8 paddingleft0">
                                            <asp:Label ID="partyName" runat="server" CssClass="text" Text="Label"></asp:Label><br />
                                            <asp:Label ID="address" runat="server" CssClass="text" Text="Label"></asp:Label>,&nbsp;
                                     <asp:Label ID="lblzipcode" runat="server" CssClass="text" Text=""></asp:Label>,&nbsp;
                                     <asp:Label ID="mobile" runat="server" CssClass="text" Text="Label"></asp:Label>&nbsp;
                                        </div>
                                        <div class="col-lg-2 col-md-2 col-sm-8 paddingleft0">
                                            <asp:Label ID="lblVisitdate1" runat="server" CssClass="text" Text="Visit Date"></asp:Label><br />
                                            <asp:Label ID="lblVisitDate5" runat="server" CssClass="text" Text="Visit Date"></asp:Label>&nbsp;
                                        </div>
                                        <div class="col-lg-2 col-md-2 col-sm-8 paddingleft0">
                                            <asp:Label ID="lblAreaName1" runat="server" CssClass="text" Text="Area Name"></asp:Label><br />
                                            <asp:Label ID="lblBeatName5" runat="server" CssClass="text" Text="Beat Name"></asp:Label>&nbsp;
                                        </div>
                                        <div class="col-lg-2 col-md-2 col-sm-8 paddingleft0">
                                            <asp:Label ID="lblBeatName1" runat="server" CssClass="text" Text="Beat Name"></asp:Label><br />
                                            <asp:Label ID="lblAreaName5" runat="server" CssClass="text" Text="Area Name"></asp:Label>&nbsp;
                                        </div>

                                        <div class="col-lg-2 col-md-1 col-sm-8 paddingleft0" style="float: right">

                                            <asp:Button Style="margin-right: 5px; margin-bottom: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                                OnClick="btnFind_Click" />

                                            <asp:Button Style="margin-right: 5px; margin-bottom: 5px;" type="button" ID="Button1" runat="server" Text="Back" class="btn btn-primary"
                                                OnClick="Button1_Click" />

                                            <asp:HiddenField ID="hid" runat="server" />
                                        </div>
                                    </div>
                                      
                                </div>
                               
                            <%--</div>--%>
                             
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body" style="margin-top:-20px !important;">
                            <h3>Party Collection</h3>
                            <div class="col-lg-5 col-md-7 col-sm-8 col-xs-11">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Document Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:TextBox ID="txtdocumentdate" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>

                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MM/yyyy" TargetControlID="txtdocumentdate" runat="server" />
                                </div>

                                <div id="divdocid" runat="server" class="form-group">
                                    <label for="exampleInputEmail1">Document No:</label>
                                    <asp:TextBox ID="lbldocno" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>

                                <%-- <div class="form-group">
                                    <input id="DepId" hidden="hidden" />
                                    <label for="exampleInputEmail1">Distributor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtdistName" runat="server" MaxLength="6" class="form-control"></asp:TextBox>
                                    <ajaxtoolkit:autocompleteextender ID="txtdistName_AutoCompleteExtender" runat="server" BehaviorID="txtdistName_AutoCompleteExtender" FirstRowSelected="false" OnClientItemSelected = "ClientItemSelected"
                                        DelimiterCharacters="" ServiceMethod="SearchItem" ServicePath="DistributerCollection.aspx" MinimumPrefixLength="2" EnableCaching="true" TargetControlID="txtdistName">
                                    </ajaxtoolkit:autocompleteextender>
                                     <asp:HiddenField ID="hfCustomerId" runat="server" />
                                     <asp:HiddenField ID="hfname" runat="server" />
                                </div>--%>

                                <div class="form-group col-md-6 small paddingleft0">
                                    <label for="exampleInputEmail1">Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:TextBox ID="txtAmount" MaxLength="12" runat="server" CssClass="form-control numeric  text-right"></asp:TextBox>

                                </div>


                                <div class="form-group col-md-6 small paddingleft0">
                                    <label for="exampleInputEmail1">Payment Mode</label>
                                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal" onchange="GetRadioButtonSelectedValue();" CssClass="radiogroup">
                                        <asp:ListItem Text="Cheque" Value="Cheque" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="DD" Value="DD"></asp:ListItem>
                                        <asp:ListItem Text="Cash" Value="Cash"></asp:ListItem>
                                        <asp:ListItem Text="RTGS" Value="RTG"></asp:ListItem>

                                    </asp:RadioButtonList>
                                </div>
                                <div id="DivChNo" style="display: none" class="form-group col-md-6 paddingleft0">
                                    <label for="exampleInputEmail1">Instrument No:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:TextBox ID="txtCHDDNO" MaxLength="25" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div id="DivDate" style="display: none" class="form-group col-md-6 paddingright0">
                                    <label for="exampleInputEmail1">Instrument Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:TextBox ID="txCheqDate" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="calendarTextBox_CalendarExtender" CssClass="orange" Format="dd/MM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender"
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
                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" OnClick="btnDelete_Click" />
                            <br />
                            <br />
                            <div>
                                <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                            </div>
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
                            <h3 class="box-title">Party Collection List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Document Date</th>
                                                <th>Document No.</th>
                                                <th>Name</th>
                                                <th>Amount</th>
                                                <th>Mode</th>
                                                <th>Instrument No</th>
                                                <th>Instrument Date</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("CollId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("CollId") %>' />
                                        <td><%#String.Format("{0:dd/MMM/yyyy}",Eval("PaymentDate")) %></td>
                                        <td><%#Eval("CollDocId") %></td>
                                        <td><%#Eval("PartyName") %></td>
                                        <td><%#Eval("Amount") %></td>
                                        <td><%#Eval("Mode") %></td>
                                        <td><%#Eval("Cheque_DDNo") %></td>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("Cheque_DD_Date"))%></td>
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
            $("#example1").DataTable({ "order": [[0, "desc"]] });

        });
    </script>
</asp:Content>
