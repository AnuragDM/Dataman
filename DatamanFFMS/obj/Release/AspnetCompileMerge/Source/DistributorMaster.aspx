<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DistributorMaster.aspx.cs" Inherits="AstralFFMS.DistributorMaster" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
    </style>
    <style type="text/css">
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

        @media (max-width: 600px) {
            .formlay {
                width: 100% !important;
                height: 60px;
            }
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            BindRole();
            BindCity();
            BindSalesPerson();
            BindPartyArea(0);
        });

    </script>      
     <script type="text/javascript">
         function BindSalesPerson() {
             var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
             var obj = { SalesID: 0 };
             $('#<%=ddlsalesperson.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                $.ajax({
                    type: "POST",
                    url: pageUrl + '/PopulateSalesMan',
                    data: JSON.stringify(obj),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnCityPopulated,
                    failure: function (response) {
                        //alert(response.d);
                    }
                });
                function OnCityPopulated(response) {
                    PopulateCityControl(response.d, $("#<%=ddlsalesperson.ClientID %>"));
             }
             function PopulateCityControl(list, control) {
                 if (list.length > 0) {
                     // control.removeAttr("disabled");
                     control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                     $.each(list, function () {
                         control.append($("<option></option>").val(this['Value']).html(this['Text']));
                     });
                     var id = $('#<%=HiddenSalesPersonID.ClientID%>').val();
                     //     //alert(id);
                     if (id != "") {
                        control.val(id);
                     }
                 }
                 else {
                     control.empty().append('<option selected="selected" value="0">Not available<option>');
                 }
             }

         }


         //////////////
         function BindCity() {
             var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
             var obj = { CityId: 0,RecType:'' };
                $('#<%=ddlCity.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
             $.ajax({
                 type: "POST",
                 url: pageUrl + '/PopulateCity',
                 data: JSON.stringify(obj),
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: OnCityPopulated,
                 failure: function (response) {
                     //alert(response.d);
                 }
             });
             function OnCityPopulated(response) {
                 PopulateCityControl(response.d, $("#<%=ddlCity.ClientID %>"));
             }
             function PopulateCityControl(list, control) {
                 if (list.length > 0) {
                     // control.removeAttr("disabled");
                     control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                     $.each(list, function () {
                         control.append($("<option></option>").val(this['Value']).html(this['Text']));
                     });
                     var id = $('#<%=HiddenCityID.ClientID%>').val();
                     //     //alert(id);
                     if (id != "") {
                         control.val(id);
                                  }
                              }
                              else {
                                  control.empty().append('<option selected="selected" value="0">Not available<option>');
                              }
                          }

         }

         //////Bind Party Area
         function BindPartyArea(j) {
             var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
                if (j == 1) {
                    var jcity = $('#<%=ddlCity.ClientID%>').val();
                }
                else {
                    jcity = $('#<%=HiddenCityID.ClientID%>').val();
                }
                //alert(jcity);

                var obj = { CityId: jcity, AreaID: 0 };
                $('#<%=ddlArea.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                $.ajax({
                    type: "POST",
                    url: pageUrl + '/PopulatePartyArea',
                    data: JSON.stringify(obj),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnPopulated,
                    failure: function (response) {
                        //alert(response.d);
                        //alert("aBHI");
                    }
                });
                function OnPopulated(response) {
                    PopulateControl(response.d, $("#<%=ddlArea.ClientID %>"));
            }
            function PopulateControl(list, control) {
                if (list.length > 0) {
                    // control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(list, function () {
                        control.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });
                    var id = $('#<%=HiddenDistributorArea.ClientID%>').val();
                       //alert(id);
                    if (id != "") {
                        if (j == 0) {
                            control.val(id);
                        }

                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">Not available<option>');
                }
            }

        }



         //////////////
           function BindRole() {
               var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
               var obj = { RoleType: 'Distributor' };
            $('#<%=ddlRole.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: pageUrl + '/PopulateRole',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    //alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ddlRole.ClientID %>"));
               }
               function PopulateControl(list, control) {
                   if (list.length > 0) {
                       //  control.removeAttr("disabled");
                       control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                       $.each(list, function () {
                           control.append($("<option></option>").val(this['Value']).html(this['Text']));
                       });
                       var id = $('#<%=HiddenRoleID.ClientID%>').val();
                    //  //alert(id);
                    if (id != "") {
                        control.val(id);
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">Not available<option>');
                }

            }
        }
</script>
     <script type="text/javascript">         
         function ddlCityChange() {           
            BindPartyArea(1);
        }

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
        function validate() {
            if ($('#<%=Distributor.ClientID%>').val() == "") {
                errormessage("Please enter Distributor Name");
                return false;
            }

            var value = ($('#<%=Distributor.ClientID%>').val().charAt(0));
            var chrcode = value.charCodeAt(0);
            if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                errormessage("Do not start Name with special characters")
                return false;
            }

            //Added As per UAT - on 11-Dec-2015
            if ($('#<%=Address1.ClientID%>').val() == "") {
                errormessage("Please enter address line 1");
                return false;
            }
            //END


            if ($('#<%=ddlCity.ClientID%>').val() == "0" || $('#<%=ddlCity.ClientID%>').val() == "") {
                errormessage("Please select City");
                return false;
            }

          
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
                return false;
            }
            varmblLength = "";
            varmblLength = ($('#<%=Mobile.ClientID%>').val().length);
            if (varmblLength < 10) {
                errormessage("Please enter 10 digit mobile No.");
                return false;
            }

            if ($('#<%=Email.ClientID%>').val() == "") {
                errormessage("Please enter Email ID.");
                return false;
            }
            if ($('#<%=Email.ClientID%>').val() != "") {
                var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
                var emailList = $('#<%=Email.ClientID%>').val();

                if (!(emailList.trim()).match(mailformat)) {
                    errormessage("Invalid Email To Address.");
                    return false;
                }
            }
            <%--if ($('#<%=SyncId.ClientID%>').val() == "") {
                errormessage("Please enter SyncID.");
                return false;
            }--%>
            if ($('#<%=ddlsalesperson.ClientID%>').val() == "0") {
                errormessage("Please select a SalesPerson.");
                return false;
            }
            if ($('#<%=Username.ClientID%>').val() == "") {
                errormessage("Please enter user name.");
                return false;
            }

            if ($('#<%=ddlRole.ClientID%>').val() == "0") {
                errormessage("Please select a Role.");
                return false;
            }            
            $('#<%=HiddenCityID.ClientID%>').val($('#<%=ddlCity.ClientID%>').val());
            $('#<%=HiddenRoleID.ClientID%>').val($('#<%=ddlRole.ClientID%>').val());
            $('#<%=HiddenSalesPersonID.ClientID%>').val($('#<%=ddlsalesperson.ClientID%>').val());     
            $('#<%=HiddenDistributorArea.ClientID%>').val($('#<%=ddlArea.ClientID%>').val());


          <%--  var checkedActive = $('#<%=chkIsAdmin.ClientID%>').val() ? true : false;
            ////alert($('#<%=chkIsAdmin.ClientID%>').val() );
            if (!checkedActive) {
                if ($('#<%=BlockReason.ClientID%>').val() == "") {
                    errormessage("Please enter Blocked Reason.");
                    return;
                }
            }
            else {
                $('#<%=BlockReason.ClientID%>').val("");
            }--%>
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            var valLength = "";
            $('#<%=Distributor.ClientID%>').keypress(function (key) {

                valLength = ($('#<%=Distributor.ClientID%>').val().length + 1);

                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
        });
    </script>
    <script>
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (!(iKeyCode != 8)) {
                s
                e.preventDefault();
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=chkIsAdmin.ClientID%>').change(function () {
                  if (this.checked) {
                      $('#<%=divblock.ClientID%>').addClass("hidden");
                              }
                              else { $('#<%=divblock.ClientID%>').removeClass("hidden"); }
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
        function DoNav(PartyId) {
            if (PartyId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();                
                __doPostBack('', PartyId)

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
                            <h3 class="box-title">Distributor Master</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" OnClick="btnFind_Click" runat="server" Text="Find" class="btn btn-primary" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-md-12">
                                <div class="form-group formlay">
                                    <input id="PartyId" hidden="hidden" />
                                    <label for="exampleInputEmail1">Distributor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input type="text" runat="server" class="form-control" maxlength="50" id="Distributor" placeholder="Enter Distributor Name" tabindex="2">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Distributor Name2:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="50" id="Distributor2" placeholder="Enter Distributor Name2" tabindex="3">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Contact Person:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="100" id="contactPerson" placeholder="Enter Contact Person" tabindex="4">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Address Line 1:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input type="text" runat="server" class="form-control" maxlength="150" id="Address1" tabindex="5" placeholder="Enter Address1">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Address Line 2:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="150" id="Address2" placeholder="Enter Address2" tabindex="6">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlCity" Width="100%" CssClass="form-control" runat="server" TabIndex="8" onchange="ddlCityChange();"></asp:DropDownList>
                                </div>
                                 <div class="form-group formlay" id="divarea" runat="server" >
                                    <label for="exampleInputEmail1">Area:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;" v>*</label>
                                    <asp:DropDownList ID="ddlArea" Width="100%" CssClass="form-control" runat="server" TabIndex="40"></asp:DropDownList>
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Pincode:</label>
                                    <input type="text" runat="server" class="form-control numeric text-right" maxlength="6" id="Pin" placeholder="Enter Pincode" onkeypress="javascript:return isNumber (event)" tabindex="10">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Mobile:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input type="text" runat="server" class="form-control numeric text-right" maxlength="10" id="Mobile" onkeypress="javascript:return isNumber (event)" placeholder="Enter Mobile" tabindex="12">
                                    <asp:HiddenField ID="hdnoldMobile" runat="server" />
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Phone:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                    <input type="text" runat="server" class="form-control" maxlength="15" id="Phone" onkeypress="javascript:return isNumber (event)" placeholder="Enter Mobile" tabindex="14">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Fax:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="15" id="Fax" placeholder="Enter Fax" tabindex="14">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Telex:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="15" id="Telex" placeholder="Enter Telex" tabindex="15">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Email ID:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input type="text" runat="server" class="form-control" maxlength="50" id="Email" placeholder="Enter Email" tabindex="16">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">CST No:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="50" id="CSTNo" placeholder="Enter CST No." tabindex="18">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">VAT TIN No.:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="50" id="VatTin" placeholder="Enter VATTIN" tabindex="20">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Service Tax Reg. No:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="50" id="ServiceTax" placeholder="Enter Service Tax Reg. No." tabindex="22">
                                </div>

                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">PAN No:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="50" id="PanNo" placeholder="Enter PAN No." tabindex="24">
                                </div>

                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Open Order:</label>
                                    <input type="text" runat="server" class="form-control numeric text-right" maxlength="12" id="OpenOrder" placeholder="Enter Open Order." tabindex="26">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Credit Limit:</label>
                                    <input type="text" runat="server" class="form-control numeric text-right" maxlength="12" id="CreditLimit" placeholder="Enter Credit Limit." tabindex="28">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Outstanding:</label>
                                    <input type="text" runat="server" class="form-control numeric text-right" maxlength="12" id="Outstanding" placeholder="Enter Outstanding." tabindex="30">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Credit Days:</label>
                                    <input type="text" runat="server" class="form-control numeric text-right" maxlength="4" id="CreditDays" placeholder="Enter Credit Days." tabindex="31">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Date of Anniversary:</label>
                                    <asp:TextBox ID="txtDOA" class="form-control" runat="server" Style="background-color: white;"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="txtDOA_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="txtDOA_CalendarExtender"
                                        TargetControlID="txtDOA"></ajaxToolkit:CalendarExtender>
                                </div>

                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Date of Birth:</label>
                                    <asp:TextBox ID="txtDOB" class="form-control" runat="server" Style="background-color: white;"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="txtDOB_CalendarExtender"
                                        TargetControlID="txtDOB"></ajaxToolkit:CalendarExtender>
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Sync Id:</label>
                                   <%-- &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>--%>
                                    <input type="text" runat="server" class="form-control" maxlength="150" id="SyncId" placeholder="Enter Sync Id" tabindex="33">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Remark:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="500" id="Remark" placeholder="Enter Remark" tabindex="35">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlsalesperson" Width="100%" runat="server" CssClass="form-control" TabIndex="36"></asp:DropDownList>
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">User Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <label style="">[Login Credentials]</label>
                                    <input type="text" runat="server" class="form-control" maxlength="25" id="Username" placeholder="Enter User Name" tabindex="38" />
                                </div>

                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Role:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlRole" Width="100%" CssClass="form-control" runat="server" TabIndex="40"></asp:DropDownList>
                                </div>                                
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Active:</label>
                                    <input id="chkIsAdmin" runat="server" type="checkbox" onchange="CheckVal(this.value);" checked="checked" class="checkbox" tabindex="42" />
                                    <input id="HdnFldIsAdmin" hidden="hidden" value="N" />
                                </div>
                                <div class="form-group formlay">

                                    <span id="divblock" runat="server" class="hidden">
                                        <label for="exampleInputEmail1" style="padding-left: 50%;">Blocked Reason:</label>&nbsp;&nbsp;<label for="requiredFields"></label>
                                        <input type="text" class="form-control" runat="server" maxlength="100" id="BlockReason" placeholder="Enter Block Reason" tabindex="50"></span>
                                </div>
                            </div>
                        </div>
                        <div class="box-footer">
                            <asp:HiddenField ID="HiddenRoleID" runat="server" />
                            <asp:HiddenField ID="HiddenCityID" runat="server" />
                            <asp:HiddenField ID="HiddenSalesPersonID" runat="server" />
                            <asp:HiddenField ID="HiddenDistributorArea" runat="server" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClientClick="javascript:return validate();" OnClick="btnSave_Click" TabIndex="52" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" TabIndex="54" OnClick="btnCancel_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" TabIndex="56" OnClick="btnDelete_Click" />
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
                            <h3 class="box-title">Distributor List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" OnClick="btnBack_Click" runat="server" Text="Back" class="btn btn-primary" />

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server" >
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Distributor Name</th>
                                                <th>City</th>
                                                <th>Area</th>
                                               <%-- <th runat="server" id="tblarea">Area</th>--%>
                                                <th>Mobile</th>
                                                <th>User Name</th>
                                                <th>Sales Person</th>
                                                <th>Sync ID</th>
                                                <th>Active</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("PartyId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("PartyId") %>' />
                                        <td><%#Eval("PartyName") %></td>
                                        <td><%#Eval("city") %></td>
                                        <td><%#Eval("Area") %></td>
                                        <td><%#Eval("Mobile") %></td>
                                        <td><%#Eval("UserName") %></td>
                                        <td><%#Eval("smname") %></td>
                                        <td><%#Eval("SyncId") %></td>
                                        <td><%#Eval("Active") %></td>
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
    <script type="text/javascript">
        $(document).ready(function () {
            var valLength = "";
            $("#PartyName").keypress(function (key) {

                valLength = ($("#PartyName").val().length + 1);

                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
        });
    </script>
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
