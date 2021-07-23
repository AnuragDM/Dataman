<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MeetPlanEntryL2.aspx.cs" Inherits="AstralFFMS.MeetPlanEntryL2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--<script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style>
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
    </style>
    <script type="text/javascript">
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        function IsNumeric(e) {
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);

            return ret;
        }
    </script>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>
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
        function SetContextKey() {
            $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey($get("<%=hdfCityIdStr.ClientID %>").value);
        }
    </script>
    <script type="text/javascript">
        function ClientPartySelected(sender, e) {
            $get("<%=hfDistId.ClientID %>").value = e.get_value();
        }
    </script>


    <%--<script type = "text/javascript">
    function ClientItemSelected(sender, e) {
        $get("<%=hfCustomerId.ClientID %>").value = e.get_value();
    }
</script>
    <script type = "text/javascript">
        function SetContextKey() {
          $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey($get("<%=ddlbeat5.ClientID %>").value);
    }
</script>--%>

    <%--  <script type = "text/javascript">
        function ClientItemSelected1(sender, e) {
<%--            $get("<%=hfitemid.ClientID %>").value = e.get_value();
    }
</script>--%>


    <script type="text/javascript">
        function ClientItemSelectedParty(sender, e) {

            $get("<%=Hidparty.ClientID %>").value = e.get_value();
        }
    </script>
    <script type="text/javascript">
        function SetContextKeyParty() {

            $find('<%=AutoCompleteExtender3.ClientID%>').set_contextKey($get("<%=ddlbeat5.ClientID %>").value);
        }
    </script>
    <script>
        function Validation() {
            if ($('#<%=txtMeetDate.ClientID%>').val() == '') {
                errormessage("Please enter the Meet Date");
                return false;
            }
            if ($('#<%=ddlmeetType.ClientID%>').val() == '0') {
                errormessage("Please select the Meet Type");
                return false;
            }
            if ($('#<%=ddlindrustry.ClientID%>').val() == '0') {
                errormessage("Please select the Material Class");
                return false;
            }
           <%-- if ($('#<%=Hidparty.ClientID%>').val() == "0") {
                errormessage("Please select the valid Party");
                return false;
            }
          if ($('#<%=Hidparty.ClientID%>').val() == '') {
                errormessage("Please select the Party");
                return false;
            }
            if ($('#<%=ddlbeat5.ClientID%>').val() == "0") {
                errormessage("Please select the beat");
                return false;
            }--%>

            if ($('#<%=txtNoOfUsers.ClientID%>').val() == '') {
                errormessage("Please enter the No of Users");
                return false;
            }
            if ($('#<%=txtNoOfUsers.ClientID%>').val() == "0") {
                errormessage("No of Users should be greater than 0");
                return false;
            }
            if ($('#<%=ddlmeetCity.ClientID%>').val() == "0") {
                errormessage("Please select The City");
                return false;
            }
            if ($('#<%=txtVenue.ClientID%>').val() == '') {
                errormessage("Please enter the Venue");
                return false;
            }

            if ($("#ContentPlaceHolder1_txtComments").val() == '') {
                errormessage("Please Enter the Comments");
                return false;
            }


            if ($('#<%=txtApproxBudget.ClientID%>').val() == '') {
                errormessage("Please enter the approx budget");
                return false;
            }

        }
    </script>
    <section class="content">
        <script type="text/javascript">
            function load1() {

                $(".numeric").numeric({ negative: false });
            }

            $(window).load(function () {

                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(load1);

            });


        </script>
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <asp:UpdatePanel ID="updatePa" runat="server">
                <ContentTemplate>
                    <!-- left column -->
                    <div class="col-md-12">
                        <div id="InputWork">
                            <!-- general form elements -->

                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <%--<h3 class="box-title">Meet Plan Entry</h3>--%>
                                    <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                                    <div style="float: right">
                                        <%--    <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                OnClick="btnFind_Click" />--%>
                                    </div>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="col-lg-10 col-md-12 col-sm-10 col-xs-10">
                                        <div class="form-group col-md-5 paddingleft0">
                                            <label for="exampleInputEmail1">User:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlunderUser" AutoPostBack="true" OnSelectedIndexChanged="ddlunderUser_SelectedIndexChanged" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-3 paddingleft0 paddingright0">
                                            <label for="visitDate">Meet Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtMeetDate" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="orange" Format="dd/MMM/yyyy"
                                                BehaviorID="calendarTextBox_CalendarExtender" TargetControlID="txtMeetDate"></ajaxToolkit:CalendarExtender>
                                        </div>
                                        <div class="form-group col-md-4 paddingright0">
                                            <label for="exampleInputEmail1">Meet Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlmeetType" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                        </div>


                                        <%-- <div class="form-group col-md-6 paddingleft0">
                                    <label for="exampleInputEmail1">Distributer:</label>&nbsp;&nbsp;<label for="requiredFields" style="color:red;">*</label>
                                    <asp:TextBox ID="txtdistName" runat="server" MaxLength="6" class="form-control" onkeyup="SetContextKey();"></asp:TextBox>
                                    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="SearchItem" ServicePath="MeetPlanEntry.aspx"
                                        TargetControlID="txtdistName" UseContextKey="true" FirstRowSelected="false"  OnClientItemSelected="ClientItemSelected"  MinimumPrefixLength="3" EnableCaching="true">
                                    </ajaxToolkit:AutoCompleteExtender>
                                    <asp:HiddenField ID="hfCustomerId" runat="server" />
                              </div>--%>

                                        <div class="form-group col-md-6 paddingleft0">
                                            <label for="exampleInputEmail1">Beat:</label>
                                            <asp:DropDownList ID="ddlbeat5" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlbeat5_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>

                                        <div class="form-group col-md-6 paddingright0">
                                            <label for="exampleInputEmail1">Party:</label>
                                            <asp:TextBox ID="txtParty" runat="server" MaxLength="8" class="form-control" onkeyup="SetContextKeyParty();"></asp:TextBox>
                                            <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" ServiceMethod="SearchParty" ServicePath="MeetPlanEntry.aspx"
                                                TargetControlID="txtParty" UseContextKey="true" FirstRowSelected="false" OnClientItemSelected="ClientItemSelectedParty" MinimumPrefixLength="3" EnableCaching="true">
                                            </ajaxToolkit:AutoCompleteExtender>
                                            <asp:HiddenField ID="Hidparty" runat="server" />
                                        </div>


                                        <div class="form-group col-md-5 paddingleft0">
                                            <label for="exampleInputEmail1">Material Class:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlindrustry" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                        </div>


                                        <div class="form-group col-md-2">
                                            <label for="exampleInputEmail1">No of Users:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtNoOfUsers" onkeypress="return IsNumeric(event);" runat="server" MaxLength="5" placeholder="" CssClass="form-control  text-right"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label for="exampleInputEmail1">No Of Staff:</label>
                                            <asp:TextBox ID="txtNoofStaf" onkeypress="return IsNumeric(event);" runat="server" MaxLength="5" CssClass="form-control  text-right"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-3 paddingright0">
                                            <label for="exampleInputEmail1">Meet City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlmeetCity" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlmeetCity_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="form-group col-md-6 paddingleft0">
                                            <label for="exampleInputEmail1">Venue:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtVenue" runat="server" placeholder="Enter the Venue" Rows="6" Cols="3" Height="50%" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-6 paddingright0">
                                            <label for="exampleInputEmail1">Comments:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtComments" runat="server" placeholder="Enter the Comments" Rows="6" Cols="3" Height="50%" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-5 paddingleft0">
                                            <label for="exampleInputEmail1">Type Of Gifts:</label>
                                            <asp:TextBox ID="txttypeofGiftsforenduser" runat="server" MaxLength="255" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group  col-md-2 paddingright0">
                                            <label for="exampleInputEmail1">Quantity:</label>
                                            <asp:TextBox ID="txtgiftqty" MaxLength="12" runat="server" CssClass="form-control numeric text-right"></asp:TextBox>
                                        </div>
                                        <div class="form-group  col-md-5 paddingright0">
                                            <label for="exampleInputEmail1">Value:</label>
                                            <asp:TextBox ID="txtvalueforenduser" MaxLength="12" runat="server" CssClass="form-control numeric text-right"></asp:TextBox>
                                        </div>
                                        <%-- <div class="form-group col-md-7 paddingleft0">
                                <label for="exampleInputEmail1">Type Of Gifts Identified for Retailers:</label>
                                 <asp:TextBox ID="txttypeofGiftsforretailer" runat="server" MaxLength="255"  CssClass="form-control"></asp:TextBox>
                            </div>
                              <div class="form-group col-md-5 paddingright0">
                                <label for="exampleInputEmail1">Value:</label>
                                 <asp:TextBox ID="txtvalueforretailer"  runat="server" MaxLength="12"  CssClass="form-control numeric"></asp:TextBox>
                            </div>--%>

                                        <div class="form-group col-md-3 paddingleft0">
                                            <label for="exampleInputEmail1">Meet Approx Budget:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtApproxBudget" runat="server" MaxLength="12" CssClass="form-control numeric text-right"></asp:TextBox>
                                        </div>

                                        <div class="form-group col-md-3 paddingleft0 paddingright0">
                                            <label for="exampleInputEmail1">Astral Sharing %:</label>
                                            <asp:TextBox ID="txtastralSharing" MaxLength="3" runat="server" CssClass="form-control numeric text-right"></asp:TextBox>
                                        </div>

                                        <!--Added As Per UAT 07-12-2015-->

                                        <div class="form-group col-md-3 paddingleft0 paddingright0" style="padding-left:18px;">
                                            <label for="exampleInputEmail1">Distributor:</label>
                                            <asp:HiddenField ID="hdfCityIdStr" runat="server" />
                                            <%-- <asp:DropDownList ID="Ddldistributor" CssClass="form-control select2" runat="server"></asp:DropDownList>--%>
                                            <asp:TextBox ID="txtDist" runat="server" class="form-control" placeholder="Enter Distributor" onkeyup="SetContextKey();"></asp:TextBox>
                                            <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="completionList"
                                                CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted" UseContextKey="true" FirstRowSelected="false"
                                                OnClientItemSelected="ClientPartySelected"
                                                DelimiterCharacters="" ServiceMethod="SearchDist" ServicePath="~/MeetPlanEntryL2.aspx" MinimumPrefixLength="3" EnableCaching="true"
                                                TargetControlID="txtDist">
                                            </ajaxToolkit:AutoCompleteExtender>
                                            <asp:HiddenField ID="hfDistId" runat="server" />
                                        </div>
                                        <!--End-->

                                        <div class="form-group col-md-3 paddingright0">
                                            <label for="exampleInputEmail1">Distributor Sharing %:</label>
                                            <asp:TextBox ID="txtDistributerSharing" runat="server" MaxLength="3" CssClass="form-control numeric text-right"></asp:TextBox>
                                        </div>

                                        <%-- <div class="form-group col-md-4 paddingleft0">
                                <label for="exampleInputEmail1">Senior:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                 <asp:DropDownList ID="ddlsenior" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>--%>

                                        <div class="form-group col-md-3 paddingright0">
                                            <label for="exampleInputEmail1">Scheme Code:</label>
                                            <asp:DropDownList ID="ddlscheme" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <%--   <div class="form-group">
                                <label for="exampleInputEmail1"></label>
                                 <asp:TextBox ID="TextBox2" Visible="false" runat="server" MaxLength="5"    CssClass="form-control numeric"></asp:TextBox>
                            </div>--%>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="box-footer">
                                        <asp:Button ID="btnsave" CssClass="btn btn-primary" OnClientClick="return Validation();" runat="server" Text="Save" OnClick="btnsave_Click" />
                                        <asp:Button ID="Cancel" CssClass="btn btn-primary" runat="server" Text="Cancel" OnClick="Cancel_Click" />
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </section>

</asp:Content>

