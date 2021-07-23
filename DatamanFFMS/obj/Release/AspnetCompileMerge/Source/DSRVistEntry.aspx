<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DSRVistEntry.aspx.cs" Inherits="AstralFFMS.DSRVistEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--  <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>

    <%-- <script type="text/javascript">
        $(function () {
            $('[id*=ddlmaterialclass]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '200px',
                includeSelectAllOption: true,
                maxHeight: 150,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
    </script>--%>
   
     <script type="text/javascript">
         function pageLoad() {
             //Initialize Select2 Elements
             //$(".select2").select2();

             $('[id*=ddlcity]').multiselect({
                 enableCaseInsensitiveFiltering: true,
                 buttonWidth: '100%',
                 includeSelectAllOption: true,
                 maxHeight: 150,
                 width: 215,
                 enableFiltering: true,
                 filterPlaceholder: 'Search'
             });
             $('[id*=ddlmaterialclass]').multiselect({
                 enableCaseInsensitiveFiltering: true,
                 buttonWidth: '100%',
                 includeSelectAllOption: true,
                 maxHeight: 150,
                 width: 215,
                 enableFiltering: true,
                 filterPlaceholder: 'Search'
             });
         };
    </script>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
        .multiselect-container.dropdown-menu {
        width: 100% !important;
        }

        .select2-container {
            /*display: table;*/
        }

        .input-group .form-control {
            height: 34px;
        }

        .multiselect-container > li > a {
            white-space: normal;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#ContentPlaceHolder1_basicExample').timepicker({ 'timeFormat': 'H:i' });
            $('#ContentPlaceHolder1_basicExample1').timepicker({ 'timeFormat': 'H:i' });
            $("#ContentPlaceHolder1_basicExample").keypress(function (event) { event.preventDefault(); });
            $("#ContentPlaceHolder1_basicExample1").keypress(function (event) { event.preventDefault(); });
            //$("#ContentPlaceHolder1_basicExample").val("10:00am");
            //$("#ContentPlaceHolder1_basicExample1").val("6:00pm");

        });
    </script>
    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 500, position: "top-right", opacity: 2,
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
            if ($('#<%=txtVisitDate.ClientID%>').val() == "") {
                errormessage("Please select the visit Date");
                return false;
            }
            if ($("#ContentPlaceHolder1_basicExample").val() == "") {
                errormessage("Please select the Start Time");
                return false;
            }
            if ($("#ContentPlaceHolder1_basicExample1").val() == "") {
                errormessage("Please select the End Time");
                return false;
            }


            if ($('#<%=ddlcity.ClientID%>').val() == "0") {
                errormessage("Please select the City.");
                return false;
            }

           <%-- if ($('#<%=txtdistName.ClientID%>').val() == '') {
                errormessage("Please enter the distributer Name.");
                return false;
            }
            if ($('#<%=hfCustomerId.ClientID%>').val() == '') {
                errormessage("Please enter the valid distributer Name.");
                return false;
            }
            if ($('#<%=hfCustomerId.ClientID%>').val() == 0) {
                errormessage("Please enter the valid distributer Name.");
                return false;
            }--%>
            if ($('#<%=ddlmodeoftransport.ClientID%>').val() == "0") {
                errormessage("Please select the Mode of Transport.");
                return false;
            }
            if ($('#<%=ddlvehicle.ClientID%>').val() == "0") {
                errormessage("Please select the Vehicle Type");
                return false;
            }
            if ($('#<%=ddlnextcity.ClientID%>').val() == "0") {
                errormessage("Please select the next City.");
                return false;
            }
            if ($('#<%=txtRemarks.ClientID%>').val() == "") {
                errormessage("Please enter the Remark");
                return false;
            }
        }

    </script>
    <style type="text/css">
        .containerStaff {
            border: 1px solid #ccc;
            max-height: 200px;
            overflow-y: scroll;
        }
    </style>

    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to Lock?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
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
    <script type="text/javascript">
        function DoNav(depId) {
            if (depId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                __doPostBack('', depId)
            }
        }
    </script>


    <%--<script type = "text/javascript">
    function ClientItemSelected(sender, e) {
        $get("<%=hfCustomerId.ClientID %>").value = e.get_value();
    }
</script>
    
<script type = "text/javascript">
    function SetContextKey() {
        $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey($get("<%=ddlcity.ClientID %>").value);
    }
</script>--%>

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
                            <h3 class="box-title">DSR Entry L-3</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                    OnClick="btnFind_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnLock" runat="server" Text="Lock" class="btn btn-primary" OnClientClick="Confirm()"
                                    OnClick="btnLock_Click" />
                                 <asp:Button Style="margin-right: 5px;" type="button" ID="btnUnlock" runat="server" OnClientClick="ConfirmUnlock()" Text="UnLock" class="btn btn-primary"
                                    OnClick="btnUnlock_Click" Visible="false" />
                                <%--  <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" runat="server" />--%>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <asp:UpdatePanel ID="update" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="col-lg-5 col-md-7 col-sm-7 col-xs-9">

                                        <div id="DIVUnder" runat="server" class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>
                                            <asp:DropDownList ID="ddlUndeUser" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlUndeUser_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div id="divdocid" runat="server" class="form-group">
                                            <label for="exampleInputEmail1">Document No:</label>
                                            <asp:TextBox ID="lbldocno" Enabled="false" Style="background: white;" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>


                                        <div class="form-group col-lg-5 col-md-5 col-sm-12 paddingleft0">
                                            <input id="DepId" hidden="hidden" />
                                            <label for="exampleInputEmail1">Visit Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtVisitDate" runat="server" CssClass="form-control" Style="background-color: white;" OnTextChanged="txtVisitDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" runat="server" TargetControlID="txtVisitDate" />

                                        </div>

                                        <%-- </div>

                                 <div class="col-md-12 paddingleft0">--%>
                                        <div class="form-group col-lg-4 col-md-4 col-sm-12 paddingleft0">
                                            <div class="block">
                                                <label for="exampleInputEmail1">Start Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            </div>
                                             <asp:DropDownList ID="basicExampleDDL" runat="server" CssClass="form-control"></asp:DropDownList>
                                            <input type="text" maxlength="7" data-scroll-default="6:00am" placeholder="--Select Time--" class="form-control" id="basicExample" runat="server" autocomplete="off" visible="false">
                                        </div>
                                        <div id="divdoci" class="form-group col-lg-3 col-md-3 col-sm-12 paddingleft0 paddingright0">
                                            <div class="block">
                                                <label for="exampleInputEmail1">End Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            </div>
                                             <asp:DropDownList ID="basicExample1DDL" runat="server" CssClass="form-control"></asp:DropDownList>
                                            <input type="text" maxlength="7" data-scroll-default="7:00pm" placeholder="--Select Time--" class="form-control" id="basicExample1" runat="server" autocomplete="off" visible="false">
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="form-group">
                                            <label for="withSales">Today's City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                           <%-- <asp:DropDownList ID="ddlcity" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>--%>
                                            <asp:ListBox ID="ddlcity" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                            <asp:HiddenField ID="hdncitylock" runat="server" />                
                                             <asp:HiddenField ID="hdnCityId" runat="server" />                                                                                     
                                            <asp:HiddenField ID="hdnCityname" runat="server" />  
                                        </div>
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Mode Of Transport:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <%--<input type="text" class="form-control" maxlength="25" id="transport" maxlength="25" placeholder="Enter Mode Of Transport">--%>
                                            <asp:DropDownList ID="ddlmodeoftransport" Width="100%" runat="server" CssClass="form-control">
                                               <%-- <asp:ListItem Value="0">-- Select --</asp:ListItem>--%>
                                                <asp:ListItem Text="2 Wheeler" Value="2 Wheeler"></asp:ListItem>
                                                <asp:ListItem Text="3 Wheeler" Value="3 Wheeler"></asp:ListItem>
                                                <asp:ListItem Text="4 Wheeler" Value="4 Wheeler"></asp:ListItem>
                                                <asp:ListItem Text="Public Transport" Value="Public Transport"></asp:ListItem>
                                                <asp:ListItem Text="Company Vehicle" Value="Company Vehicle"></asp:ListItem>
                                                <asp:ListItem Text="Any Other" Value="Any Other"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Vehicle Used:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlvehicle" runat="server" Width="100%" CssClass="form-control">
                                               <%-- <asp:ListItem Value="0">-- Select --</asp:ListItem>--%>
                                                <asp:ListItem Text="Own" Value="Own"></asp:ListItem>
                                                <asp:ListItem Text="Distributor" Value="Distributor"></asp:ListItem>
                                                <asp:ListItem Text="Colleague" Value="Colleague"></asp:ListItem>
                                                 <asp:ListItem Text="Company Vehicle" Value="Company Vehicle"></asp:ListItem>
                                                <asp:ListItem Text="Other" Value="Other"></asp:ListItem>

                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group  col-md-6 paddingleft0">
                                            <label for="withSales">With Whom:</label>
                                            <asp:DropDownList ID="ddlWith" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>

                                        <div class="form-group col-md-6 paddingright0">
                                            <label for="withSales">Next Visit With Whom:</label>
                                            <asp:DropDownList ID="ddlnextwith" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>

                                        <div class="form-group col-md-6 paddingleft0">
                                            <label for="exampleInputEmail1">Next Visit Date:</label>
                                            <asp:TextBox ID="txtNextVisitDate" Width="100%" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtNextVisitDate" />

                                        </div>

                                        <div class="form-group col-md-6 paddingright0">
                                            <label for="withSales">Next City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlnextcity" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                        <%-- <div class="form-group">
                                <label for="withSales">Distributor:</label>
                                  <asp:TextBox ID="txtdistName" runat="server" MaxLength="6" class="form-control"  onkeyup = "SetContextKey();"></asp:TextBox>
                                      <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="SearchItem" ServicePath="DSREntryForm.aspx"
                      TargetControlID="txtdistName" UseContextKey = "true" FirstRowSelected="false" OnClientItemSelected ="ClientItemSelected" MinimumPrefixLength="2" EnableCaching="true"></ajaxToolkit:AutoCompleteExtender>
                                     <asp:HiddenField ID="hfCustomerId" runat="server" />
                            </div>--%>

                                        <div class="form-group" style="display:none">
                                            <label for="withSales">Product Class:</label>

                                            <asp:ListBox ID="ddlmaterialclass" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                            <%--      <asp:DropDownList ID="ddlmaterialclass" width="100%" CssClass="form-control select2" runat="server"></asp:DropDownList>--%>

                                            <%--   <div class="containerStaff">
                                       <asp:CheckBoxList ID="chkIndustry" RepeatDirection="Vertical" runat="server"></asp:CheckBoxList>
                                  </div>--%>
                                        </div>
                                         <div class="form-group  col-md-6 paddingleft0" hidden="hidden">
                                            <label for="exampleInputEmail1">Retailer's Order By Email:</label>
                                            <asp:TextBox ID="txtAmountEmail" runat="server" class=" form-control numeric text-right" Text="0.00" MaxLength="12"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-6 paddingright0" hidden="hidden">
                                            <label for="exampleInputEmail1">Retailer's Order By Phone:</label>
                                            <asp:TextBox ID="txtAmountPhone" runat="server" class=" form-control numeric text-right" Text="0.00" MaxLength="12"></asp:TextBox>
                                        </div>



                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Remark:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtRemarks" Style="resize: none; height: 20%;" TextMode="MultiLine" runat="server" class="form-control" cols="20" Rows="2" placeholder="Enter Remark"></asp:TextBox>
                                        </div>

                                    </div>
                                </ContentTemplate>
                                 <Triggers>                                  
                                  <asp:PostBackTrigger ControlID="txtVisitDate"/>                                  
                              </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;margin-top:5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                            <asp:Button Style="margin-right: 5px;margin-top:5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                            <asp:Button Style="margin-right: 5px;margin-top:5px;" type="button" ID="btnNext" runat="server" Text="Primary Sales" class="btn btn-primary" OnClick="btnNext_Click" OnClientClick="javascript:return validate();" />
                            <asp:Button Style="margin-right: 5px;margin-top:5px;" type="button" ID="btnSecondary" runat="server" Text="Secondary Sales" class="btn btn-primary" OnClick="btnSecondary_Click" OnClientClick="javascript:return validate();" />
                            <%-- <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" OnClick="btnDelete_Click" />--%>
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
                            <h3 class="box-title">DSR L-3 List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <div class="col-lg-9 col-md-8 col-sm-7 col-xs-9">
                                <div class="col-md-12 paddingleft0">
                                    <div id="DIV1" class="form-group col-md-3">
                                        <label for="exampleInputEmail1">From Date:</label>
                                        <asp:TextBox ID="txtmDate" runat="server" Style="background: white;" CssClass="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtmDate" runat="server" />
                                    </div>
                                    <div class="form-group col-md-3 ">
                                        <label for="exampleInputEmail1">To Date:</label>
                                        <asp:TextBox ID="txttodate" runat="server" Style="background: white;" CssClass="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender4" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                    </div>
                                    <div class="form-group col-md-5 ">
                                        <label for="exampleInputEmail1">Sales Person:</label>
                                        <asp:DropDownList ID="ddlUser1" Width="100%" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-md-1">
                                        <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                        <asp:Button type="button" ID="btnGo" runat="server" Style="padding: 3px 14px;" Text="Go" class="btn btn-primary go-button-dsr" OnClick="btnGo_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Visit Date</th>
                                                <th>Visit DocId</th>
                                                <th>Start Time</th>
                                                <th>End Time</th>
                                                <th>Remark</th>
                                                <th>Lock</th>
                                                <th>Status</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("VisId") %>');">
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("VDate"))%></td>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("VisId") %>' />
                                        <td><%#Eval("VisitDocId") %></td>
                                        <td><%#Eval("frTime1") %></td>
                                        <td><%#Eval("toTime1") %></td>
                                        <td><%#Eval("Remark") %></td>
                                        <td><%#(Boolean.Parse(Eval("Lock").ToString())) ? "Yes" : "No" %></td>
                                         <%--<td><%#(Eval("AppStatus").ToString()=="" || Eval("AppStatus").ToString()=="Pending") ? "Pending" : "Approve"  %></td>--%>
                                        <td><%#(Eval("AppStatus").ToString()=="" || Eval("AppStatus").ToString()=="Pending") ? "Pending" :  Eval("AppStatus").ToString()%></td>
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

