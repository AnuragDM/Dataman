<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="EditParty.aspx.cs" Inherits="AstralFFMS.EditParty" %>

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

        $("#dateTimeInput").jqxDateTimeInput({ width: '250px', height: '25px', theme: 'arctic', formatString: 'dd-MMM-yyyy' });

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

    <script>
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#Update').click(function () {
                if ($('#<%=ddlUnderParty.ClientID%>').val() == "0") {
                    errormessage("Please select the under party name.");
                    return;
                }
                if ($('#<%=ddlCity.ClientID%>').val() == "0") {
                    errormessage("Please select a City.");
                    return;
                }
                if ($('#<%=ddlArea.ClientID%>').val() == "0") {
                    errormessage("Please select an Area.");
                    return;
                }
                if ($('#<%=ddlBeat.ClientID%>').val() == "0") {
                    errormessage("Please select a Beat.");
                    return;
                }
                if ($('#<%=ddlpartytype.ClientID%>').val() == "0") {
                    errormessage("Please select Party Type.");
                    return;
                }
                //Added As per UAT - on 11-Dec-2015

                <%--  if ($('#<%=Pin.ClientID%>').val() == "") {
                    errormessage("Please enter PinCode");
                    return false;
                }--%>
                if ($('#<%=Address1.ClientID%>').val() == "") {
                    errormessage("Please enter address line 1");
                    return false;
                }
                //END

                if ($('#<%=Pin.ClientID%>').val() != "") {
                    varpnlLength = "";
                    varpnlLength = ($('#<%=Pin.ClientID%>').val().length);
                    if (varpnlLength < 6) {
                        errormessage("Please enter 6 digit Pincode");
                        return false;
                    }
                }
                if ($('#<%=Mobile.ClientID%>').val() == "") {
                    errormessage("Please enter Mobile No.");
                    return;
                }
                varmblLength = "";
                varmblLength = ($('#<%=Mobile.ClientID%>').val().length);
                if (varmblLength < 10) {
                    errormessage("Please enter 10 digit mobile No.");
                    return;
                }
                var checkedActive = ($("#<%=chkIsAdmin.ClientID%>").is(":checked")) ? 'true' : 'false';
                if (!checkedActive) {
                    if ($('#<%=BlockReason.ClientID%>').val() == "") {
                        errormessage("Please enter Blocked Reason.");
                        return;
                    }
                }
                var partyID = $('#<%=partyIDHiddenField.ClientID%>').val();
                
             
                <%--    var chkPartyDist = ($("#<%=chkPartyDist.ClientID%>").is(":checked")) ? 'true' : 'false';--%>


                //data1 = '{PartyId: "' + partyID + '" , PartyName: "' + $('#<%=PartyName.ClientID%>').val() + '" ,Address1: "' + $('#<%=Address1.ClientID%>').val() + '",Address2: "' + $('#<%=Address2.ClientID%>').val() + '",Pin: "' + $('#<%=Pin.ClientID%>').val() + '",AreaId: "' + $('#<%=ddlArea.ClientID%>').val() + '",BlockReason: "' +$('#<%=BlockReason.ClientID%>').val() + '",Mobile: "' + $('#<%=Mobile.ClientID%>').val() + '",IndId: "' + $('#<%=DdlIndustry.ClientID%>').val() + '",Potential: "' + $("#Potential").val() + '",Active: "' + checkedActive + '",BlockReason: "' + $('#<%=BlockReason.ClientID%>').val() + '",Remark: "' + $('#<%=Remark.ClientID%>').val() + '",PartyDist: "' + chkPartyDist + '",SyncId: "' + $('#<%=SyncId.ClientID%>').val() + '",UnderId: "' + $('#<%=ddlUnderParty.ClientID%>').val() + '",ContactPerson:"' + $('#<%=ContactPerson.ClientID%>').val() + '",CSTNo:"' + $('#<%=CSTNo.ClientID%>').val() + '",VatTin:"' + $('#<%=VatTin.ClientID%>').val() + '",ServiceTax:"' + $('#<%=ServiceTax.ClientID%>').val() + '",PanNo:"' + $('#<%=PanNo.ClientID%>').val() + '" }',

                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("EditParty.aspx/Partyedit") %>',
                    contentType: "application/json; charset=utf-8",
                    data: '{PartyId: "' + partyID + '" , PartyName: "' + $('#<%=PartyName.ClientID%>').val() + '" ,Address1: "' + $('#<%=Address1.ClientID%>').val() + '",Address2: "' + $('#<%=Address2.ClientID%>').val() + '",Pin: "' + $('#<%=Pin.ClientID%>').val() + '",AreaId: "' + $('#<%=ddlArea.ClientID%>').val() + '",BeatId: "' + $('#<%=ddlBeat.ClientID%>').val() + '",PartyType: "' + $('#<%=ddlpartytype.ClientID%>').val() + '",Mobile: "' + $('#<%=Mobile.ClientID%>').val() + '",IndId: "' + $('#<%=DdlIndustry.ClientID%>').val() + '",Potential: "' + $('#<%=Potential.ClientID%>').val() + '",Active: "' + checkedActive + '",BlockReason: "' + $('#<%=BlockReason.ClientID%>').val() + '",Remark: "' + $('#<%=Remark.ClientID%>').val() + '",SyncId: "' + $('#<%=SyncId.ClientID%>').val() + '",UnderId: "' + $('#<%=ddlUnderParty.ClientID%>').val() + '",ContactPerson:"' + $('#<%=ContactPerson.ClientID%>').val() + '",CSTNo:"' + $('#<%=CSTNo.ClientID%>').val() + '",VatTin:"' + $('#<%=VatTin.ClientID%>').val() + '",ServiceTax:"' + $('#<%=ServiceTax.ClientID%>').val() + '",PanNo:"' + $('#<%=PanNo.ClientID%>').val() + '" }',
                    dataType: "json",
                    success: function (savingStatus) {
                        var Message = JSON.stringify(savingStatus.d);
                        var lval = "" + "1" + ""
                        if (Message == lval) {
                            errormessage("This Party is contained in sub Party.");
                            document.getElementById("PartyName").focus();
                        }
                        else {
                            Successmessage("Record Updated Successfully.");
                            CLickAbc();
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $('#lblCommentsNotification').text("Error encountered while saving the comments.");
                    }
                });
                editrow = -1;
                var url = "PartyDashborad.aspx?PartyId=" + partyID;
                window.location.href = url;
            });
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=chkIsAdmin.ClientID%>").change(function () {
                if (this.checked) {
                    $('#divblock').addClass("hidden");
                }
                else { $('#divblock').removeClass("hidden"); }
            });
        });
    </script>
    <script type="text/javascript">
        function myPartyDash() {
            var partyID = $('#<%=partyIDHiddenField.ClientID%>').val();
            var url = "PartyDashborad.aspx?PartyId=" + partyID;
            window.location.href = url;
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
                            <div class="form-group">
                                <div>
                                    <asp:Label ID="partyName1" runat="server" CssClass="text" Text="Label" Font-Bold="true"></asp:Label>
                                    <div style="float: right">
                                        <asp:Button ID="BtnCancel" runat="server" Text="Back" Style="margin-right: 5px;" class="btn btn-primary" OnClick="BtnCancel_Click" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div>
                                    <asp:Label ID="address" runat="server" CssClass="text" Text="Label"></asp:Label>,&nbsp;
                                     <asp:Label ID="lblzipcode" runat="server" CssClass="text" Text=""></asp:Label>,&nbsp;
                                     <asp:Label ID="mobile1" runat="server" CssClass="text" Text="Label"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="box-header with-border">
                            <h3 class="box-title">Edit Party</h3>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="form-group formlay">
                                <input id="PartyId" hidden="hidden" />
                                <asp:HiddenField ID="partyIDHiddenField" runat="server" />
                                <label for="exampleInputEmail1">Party Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <input type="text" class="form-control" maxlength="150" id="PartyName" runat="server" tabindex="2" />
                            </div>

                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">Distributor:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddlUnderParty" CssClass="form-control" runat="server" TabIndex="18"></asp:DropDownList>
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">Address Line 1:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <input type="text" class="form-control" maxlength="150" id="Address1" runat="server" tabindex="1">
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">Pincode:</label>
                                <input type="text" class="form-control" maxlength="6" runat="server" id="Pin" onkeypress="javascript:return isNumber (event)" tabindex="20">
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">Address Line 2:</label>
                                <input type="text" class="form-control" maxlength="150" id="Address2" runat="server" tabindex="6">
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">Mobile:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <input type="text" class="form-control" maxlength="10" id="Mobile" runat="server" onkeypress="javascript:return isNumber (event)" tabindex="22">
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddlCity" CssClass="form-control" runat="server" TabIndex="8"></asp:DropDownList>
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">Industry:</label>
                                <asp:DropDownList ID="DdlIndustry" CssClass="form-control" runat="server" TabIndex="24"></asp:DropDownList>
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">Area:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddlArea" CssClass="form-control" runat="server" TabIndex="10"></asp:DropDownList>
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">Sync Id:</label>
                                <input type="text" class="form-control" maxlength="50" id="SyncId" runat="server">
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">Beat:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddlBeat" CssClass="form-control" runat="server" TabIndex="12"></asp:DropDownList>
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">Remark:</label>
                                <input type="text" class="form-control" maxlength="500" id="Remark" runat="server" tabindex="30">
                            </div>

                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">Potential:</label>
                                <input type="text" class="form-control" id="Potential" runat="server" onkeypress="javascript:return isNumber (event)" tabindex="13">
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">Contact Sales Person:</label>
                                <input type="text" class="form-control" maxlength="100" id="ContactPerson" runat="server" tabindex="14">
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">CST No:</label>
                                <input type="text" class="form-control" maxlength="50" id="CSTNo" runat="server" tabindex="15">
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">VAT TIN No.:</label>
                                <input type="text" class="form-control" maxlength="50" id="VatTin" runat="server" tabindex="16">
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">Service Tax Reg. No :</label>
                                <input type="text" class="form-control" maxlength="50" id="ServiceTax" runat="server" tabindex="17">
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">PAN No:</label>
                                <input type="text" class="form-control" maxlength="50" id="PanNo" runat="server" placeholder="Enter PAN No." tabindex="18">
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">Active:</label>
                                <input id="chkIsAdmin" type="checkbox" tabindex="33" runat="server" />
                                <input id="HdnFldIsAdmin" hidden="hidden" value="N" />
                                <span id="divblock" class="hidden">
                                    <label for="exampleInputEmail1" style="padding-left: 50%;">Blocked Reason:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input type="text" class="form-control" maxlength="100" id="BlockReason" runat="server" placeholder="Enter Block Reason"></span>
                            </div>
                            <div class="form-group formlay">
                                <label for="exampleInputEmail1">Party Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddlpartytype" CssClass="form-control" runat="server" TabIndex="15">
                                    <asp:ListItem Selected="True" Text="--Select Party Type--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Retailer" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="End User" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Dealer" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                        </div>
                        <div class="box-footer">
                            <input style="margin-right: 5px;" type="button" id="Update" value="Update" class="btn btn-primary" />
                            <input style="margin-right: 5px;" type="button" id="Cancel" onclick="myPartyDash()" value="Cancel" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
