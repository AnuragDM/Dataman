<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="PartyMasterAstral.aspx.cs" Inherits="AstralFFMS.PartyMasterAstral" EnableEventValidation="false" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
            }
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
    </style>
     <script type="text/javascript">
         function showpreview(input) {
             var uploadcontrol = document.getElementById('<%=partyImgFileUpload.ClientID%>').value;
             var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
             if (uploadcontrol.length > 0) {
                 if (reg.test(uploadcontrol)) {
                     if (input.files && input.files[0]) {
                         var reader = new FileReader();
                         reader.onload = function (e) {
                             $("#ContentPlaceHolder1_imgpreview").css('display', 'block');
                             $("#ContentPlaceHolder1_imgpreview").attr('src', e.target.result);
                         }
                         reader.readAsDataURL(input.files[0]);
                     }
                 }
                 else {
                     errormessage("Only image files are allowed!");
                     $("#ContentPlaceHolder1_imgpreview").css('display', 'none');
                     return false;
                 }
             }
         }
    </script>
     <script type="text/javascript">

         function btnSubmitfunc() {
            
            var selectedValues = [];
            $("#<%=ddlstatelist.ClientID %> :selected").each(function () {
              selectedValues.push($(this).val());
          });
             $("#hidstate").val(selectedValues);
             // validate($("#hiddistributor").val());
            // alert(selectedValues);
          if (selectedValues == "") {
              errormessage("Please Select State");
              return false;
          }

          var cityValues = [];
          $("#<%=ddlcitylist.ClientID %> :selected").each(function () {
              cityValues.push($(this).val());
            });
             $("#hidcity").val(cityValues);
             //alert(cityValues);

            var areaValues = [];
            $("#<%=ddlAreaPartylist.ClientID %> :selected").each(function () {
                areaValues.push($(this).val());
          });
             $("#hidarea").val(areaValues);
             //alert(areaValues);

          loding();
          BindGridView();
      }

      function BindGridView() {
          $.ajax({
              type: "POST",
              url: "PartyMasterAstral.aspx/GetPartyDetails",
              data: '{State: "' + $("#hidstate").val() + '" , City: "' + $("#hidcity").val() + '", Area: "' + $("#hidarea").val() + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    //alert(response.d);
                },
                error: function (response) {
                    //alert(response.d);
                }
            });
        };       
       
    </script>
     <script type="text/javascript">
         $(document).ready(
             function () {
                 BindPartyType();
                 BindCity_New();
                 BindIndustry();
                 BindPartyDistributor();
                 BindState();
                 //BindPartyArea(0);              
                 //BindPartyBeat(0);                 
             }
          );

    </script>
    <script type="text/javascript">
        function BindState() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'                 

            var obj = { StateId: 0 };
                $<%--('#<%=ddlstatelist.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');--%>
            $.ajax({
                type: "POST",
                url: pageUrl + '/PopulateState',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ddlstatelist.ClientID %>"));
                  }
                  function PopulateControl(list, control) {
                      if (list.length > 0) {
                          //  control.removeAttr("disabled");
                          //control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                          $.each(list, function () {
                              control.append($("<option></option>").val(this['Value']).html(this['Text']));
                          });
                          var id = $('#<%=HiddenStateID.ClientID%>').val();
                          //  alert(id);
                          if (id != "") {
                              control.val(id);
                          }
                      }
                      else {
                          control.empty().append('<option selected="selected" value="0">Not available<option>');
                      }

                  }
              }

        function BindPartyType() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
            //   var obj = { SalesID: 0 };
            $('#<%=ddlpartytype.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: pageUrl + '/PopulatePartyType',
                // data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ddlpartytype.ClientID %>"));
             }
             function PopulateControl(list, control) {
                 if (list.length > 0) {
                     // control.removeAttr("disabled");
                     control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                     $.each(list, function () {
                         control.append($("<option></option>").val(this['Value']).html(this['Text']));
                     });
                     var id = $('#<%=HiddemPartyType.ClientID%>').val();
                        //     alert(id);
                        if (id != "") {
                            control.val(id);
                        }
                    }
                    else {
                        control.empty().append('<option selected="selected" value="0">Not available<option>');
                    }
                }
            }


            function BindCity_New() {
                var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
            var obj = { CityId: 0 };
            $('#<%=ddlCity.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
              $.ajax({
                  type: "POST",
                  url: pageUrl + '/PopulatePartyCity',
                  data: JSON.stringify(obj),
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  success: OnCityPopulated,
                  failure: function (response) {
                      alert(response.d);
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
                        var id = $('#<%=HiddenPartyCity.ClientID%>').val();
                     //     alert(id);
                     if (id != "") {
                         control.val(id);
                     }
                 }
                 else {
                     control.empty().append('<option selected="selected" value="0">Not available<option>');
                 }
             }

         }
         function BindIndustry() {
             var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
            var obj = { IndId: 0 };
            $('#<%=DdlIndustry.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: pageUrl + '/PopulateIndustry',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=DdlIndustry.ClientID %>"));
              }
              function PopulateControl(list, control) {
                  if (list.length > 0) {
                      // control.removeAttr("disabled");
                      control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                      $.each(list, function () {
                          control.append($("<option></option>").val(this['Value']).html(this['Text']));
                      });
                      var id = $('#<%=HiddenPartyIndustry.ClientID%>').val();
                        //     alert(id);
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
                if (j == 1)
                {
                    var jcity = $('#<%=ddlCity.ClientID%>').val();
                }
                else
                {
                    jcity = $('#<%=HiddenPartyCity.ClientID%>').val();
                }

            
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
                    alert(response.d);
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
                    var id = $('#<%=HiddenPartyArea.ClientID%>').val();
                 //   alert(id);
                    if (id != "") {
                        if (j == 0)
                        {
                            control.val(id);
                        }
                        
                      }
                  }
                  else {
                      control.empty().append('<option selected="selected" value="0">Not available<option>');
                  }
              }

          }
          ///////Bind Party Beat
          function BindPartyBeat(i) {
              var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
              if (i == 1)
              {
                  var jArea = $('#<%=ddlArea.ClientID%>').val();
              }
              else
              {
                  jArea = $('#<%=HiddenPartyArea.ClientID%>').val();
              }
              var obj = { AreaId: jArea, BeatId: 0 };
            $('#<%=ddlBeat.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: pageUrl + '/PopulatePartyBeat',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ddlBeat.ClientID %>"));
              }
              function PopulateControl(list, control) {
                  if (list.length > 0) {
                      // control.removeAttr("disabled");
                      control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                      $.each(list, function () {
                          control.append($("<option></option>").val(this['Value']).html(this['Text']));
                      });
                      var id = $('#<%=HiddenPartyBeat.ClientID%>').val();
                    //     alert(id);
                      if (id != "") {
                          if (i == 0) {
                              control.val(id);
                          }
                       
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">Not available<option>');
                }
            }

        }

        function BindPartyDistributor() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
            var obj = { CityId: $('#<%=ddlCity.ClientID%>').val(), DistId: 0 };
            $('#<%=ddldistributor.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: pageUrl + '/PopulatePartyDistributor',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ddldistributor.ClientID %>"));
            }
            function PopulateControl(list, control) {
                if (list.length > 0) {
                    // control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(list, function () {
                        control.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });
                    var id = $('#<%=HiddenPartyDistributor.ClientID%>').val();
                      //     alert(id);
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
        function ddlAreaChange() {
            //$('#<%=HiddenPartyArea.ClientID%>').val($('#<%=ddlArea.ClientID%>').val());       
            BindPartyBeat(1);
        }
        function ddlCityChange() {

            //$('#<%=HiddenPartyCity.ClientID%>').val($('#<%=ddlCity.ClientID%>').val());        
            BindPartyArea(1);
        
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
        function validate() {
            if ($('#<%=PartyName.ClientID%>').val() == "") {
                errormessage("Please enter Party Name");
                return false;
            }

            var value = ($('#<%=PartyName.ClientID%>').val().charAt(0));
            var chrcode = value.charCodeAt(0);
            if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                errormessage("Do not start Name with special characters")
                return false;
            }
            if ($('#<%=ddlpartytype.ClientID%>').val() == "0" || $('#<%=ddlpartytype.ClientID%>').val() == "") {
                errormessage("Please select Party Type");
                return false;
            }

            if ($('#<%=ddlCity.ClientID%>').val() == "0") {
                errormessage("Please select City.");
                return false;
            }

            if ($('#<%=ddlArea.ClientID%>').val() == "0" || $('#<%=ddlArea.ClientID%>').val() == "") {
                errormessage("Please select Area.");
                return false;
            }
            if ($('#<%=ddlBeat.ClientID%>').val() == "0" || $('#<%=ddlBeat.ClientID%>').val() == "") {
                errormessage("Please select Beat.");
                return false;
            }
        <%--    if ($('#<%=ddldistributor.ClientID%>').val() == "0" || $('#<%=ddldistributor.ClientID%>').val() == "") {
                errormessage("Please select Distributor.");
                return false;
            }--%>
            if ($('#<%=DdlIndustry.ClientID%>').val() == "0" || $('#<%=DdlIndustry.ClientID%>').val() == "") {
                errormessage("Please select Industry.");
                return false;
            }

            //Added As per UAT - on 11-Dec-2015

            if ($('#<%=Pin.ClientID%>').val() == "") {
                errormessage("Please enter PinCode");
                return false;
            }

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
                return false;
            }
            varmblLength = "";
            varmblLength = ($('#<%=Mobile.ClientID%>').val().length);
            if (varmblLength < 10) {
                errormessage("Please enter 10 digit mobile No.");
                return false;
            }
            <%-- if ($('#<%=SyncId.ClientID%>').val() == "") {
                errormessage("Please enter SyncId");
                return false;
            }--%>

            if ($('#<%=Email.ClientID%>').val() != "") {
                var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
                var emailList = $('#<%=Email.ClientID%>').val();

                if (!(emailList.trim()).match(mailformat)) {
                    errormessage("Invalid Email To Address.");
                    return false;
                }
            }

            $('#<%=HiddenPartyArea.ClientID%>').val($('#<%=ddlArea.ClientID%>').val());
            $('#<%=HiddemPartyType.ClientID%>').val($('#<%=ddlpartytype.ClientID%>').val());
            $('#<%=HiddenPartyBeat.ClientID%>').val($('#<%=ddlBeat.ClientID%>').val());
            $('#<%=HiddenPartyCity.ClientID%>').val($('#<%=ddlCity.ClientID%>').val());
            $('#<%=HiddenPartyIndustry.ClientID%>').val($('#<%=DdlIndustry.ClientID%>').val());
            $('#<%=HiddenPartyDistributor.ClientID%>').val($('#<%=ddldistributor.ClientID%>').val());
            $('#<%=HiddenStateID.ClientID%>').val($('#<%=ddlstatelist.ClientID%>').val());

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

        function loding() {
            $('#spinner').show();
        }
      
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            var valLength = "";
            $('#<%=PartyName.ClientID%>').keypress(function (key) {

                valLength = ($('#<%=PartyName.ClientID%>').val().length + 1);

                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
        });
    </script>
    <%-- <script>
          function isNumber(evt) {
              var iKeyCode = (evt.which) ? evt.which : evt.keyCode
              if (!(iKeyCode != 8)) {s
                  e.preventDefault();
                  return false;
              }
              return true;
          }
    </script>--%>
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
        function BindCity() {
            $('#<%=ddlcitylist.ClientID%>').empty();
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("DataBindService.asmx/PopulateCityByState") %>',
                contentType: "application/json; charset=utf-8",
                data: '{StateID: "' + $('#<%=ddlstatelist.ClientID%>').val() + '" }',

                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    $('#<%=ddlcitylist.ClientID%>').empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(jsdata1, function (key1, value1) {
                        $('#<%=ddlcitylist.ClientID%>').append($("<option></option>").val(value1.CityID).html(value1.CityName));
                    });
                }
            });
        }

        function BindArea() {
            $('#<%=ddlAreaPartylist.ClientID%>').empty();
             $.ajax({
                 type: "POST",
                 url: '<%= ResolveUrl("DataBindService.asmx/PopulateAreaByCity") %>',
                contentType: "application/json; charset=utf-8",
                data: '{CityID: "' + $('#<%=ddlcitylist.ClientID%>').val() + '" }',

                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);
                    $('#<%=ddlAreaPartylist.ClientID%>').empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(jsdata1, function (key1, value1) {
                        $('#<%=ddlAreaPartylist.ClientID%>').append($("<option></option>").val(value1.areaId).html(value1.areaName));
                    });
                }
            });
            }
       <%-- function BindState() {
         
            $('#<%=ddlstatelist.ClientID%>').empty();
              $.ajax({
                  type: "POST",
                  url: '<%= ResolveUrl("DataBindService.asmx/PopulateState") %>',
                contentType: "application/json; charset=utf-8",
                data: '{}',

                dataType: "json",
                success: function (data) {
                    jsdata1 = JSON.parse(data.d);

                    $.each(jsdata1, function (key1, value1) {
                      
                        $('#<%=ddlstatelist.ClientID%>').append($("<option></option>").val(value1.StateID).html(value1.StateName));
                    });
                }
            });
            }--%>
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
                            <h3 class="box-title">Party Master</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind"  OnClick="btnFind_Click" runat="server" Text="Find" class="btn btn-primary" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="Button1" OnClick="Button1_Click" runat="server" Text="Back" class="btn btn-primary" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                       <%-- <asp:UpdatePanel ID="update" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <ContentTemplate>--%>
                        <div class="box-body">
                            <div class="col-md-12">
                                <div class="form-group formlay">
                                    <input id="PartyId" hidden="hidden" />
                                    <label for="exampleInputEmail1">Party Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input type="text" runat="server" class="form-control" maxlength="50" id="PartyName" placeholder="Enter Party Name" tabindex="1">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Party Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlpartytype" Width="100%" CssClass="form-control" runat="server" TabIndex="2">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlCity" Width="100%"   CssClass="form-control" runat="server" TabIndex="3"  onchange="ddlCityChange();"></asp:DropDownList>  <%--AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged"--%>
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Area:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlArea" Width="100%"  CssClass="form-control" runat="server" TabIndex="4"  onchange="ddlAreaChange();"></asp:DropDownList> <%--AutoPostBack="true" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged"--%>
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Beat:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlBeat" Width="100%" CssClass="form-control" runat="server" TabIndex="5"></asp:DropDownList>
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Distributor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                    <asp:DropDownList ID="ddldistributor" Width="100%" CssClass="form-control" runat="server" TabIndex="6"></asp:DropDownList>
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Industry:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="DdlIndustry" Width="100%" CssClass="form-control" runat="server" TabIndex="7"></asp:DropDownList>
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Contact Person:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="100" id="ContactPerson" placeholder="Enter Contact Person" tabindex="8">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Address Line 1:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input type="text" runat="server" class="form-control" maxlength="150" id="Address1" tabindex="9" placeholder="Enter Address1">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Address Line 2:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="150" id="Address2" placeholder="Enter Address2" tabindex="10">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Pincode:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input type="text" runat="server" class="form-control numeric text-right" maxlength="6" id="Pin" placeholder="Enter Pincode" onkeypress="javascript:return isNumber (event)" tabindex="11">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Mobile:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input type="text" runat="server" class="form-control numeric text-right" maxlength="10" id="Mobile" onkeypress="javascript:return isNumber (event)" placeholder="Enter Mobile" tabindex="12">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Phone:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                    <input type="text" runat="server" class="form-control numeric text-right" maxlength="10" id="Phone" onkeypress="javascript:return isNumber (event)" placeholder="Enter Mobile" tabindex="13">
                                </div>
                                 <div class="form-group formlay">
                                    <label for="exampleInputEmail1">GSTIN No:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="50" id="GSTINNo" placeholder="Enter GSTIN No." tabindex="14">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">CST No:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="50" id="CSTNo" placeholder="Enter CST No." tabindex="15">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">VAT TIN No.:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="50" id="VatTin" placeholder="Enter VATTIN" tabindex="16">
                                </div>

                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Service Tax Reg. No:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="50" id="ServiceTax" placeholder="Enter Service Tax Reg. No." tabindex="17">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Potential:</label>
                                    <input type="text" runat="server" class="form-control numeric text-right" maxlength="8" id="Potential" placeholder="Enter Potential" tabindex="18" />
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">PAN No:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="50" id="PanNo" placeholder="Enter PAN No." tabindex="19">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Date of Anniversary:</label>
                                    <asp:TextBox ID="txtDOA" class="form-control" runat="server" Style="background-color: white;" TabIndex="20"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="txtDOA_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="txtDOA_CalendarExtender"
                                        TargetControlID="txtDOA"></ajaxToolkit:CalendarExtender>
                                </div>

                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Date of Birth:</label>
                                    <asp:TextBox ID="txtDOB" class="form-control" runat="server" Style="background-color: white;" TabIndex="21"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="txtDOB_CalendarExtender"
                                        TargetControlID="txtDOB"></ajaxToolkit:CalendarExtender>
                                </div>


                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Sync Id:</label><%--&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>--%><input type="text" runat="server" class="form-control" maxlength="50" id="SyncId" placeholder="Enter Sync Id" tabindex="44">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Remark:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="500" id="Remark" placeholder="Enter Remark" tabindex="22">
                                </div>
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Email ID:</label>
                                    <input type="text" runat="server" class="form-control" maxlength="50" id="Email" placeholder="Enter Email" tabindex="23">
                                </div>                                  
                                <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Active:</label>
                                    <input id="chkIsAdmin" runat="server" type="checkbox" checked="checked" class="checkbox" tabindex="24" />
                                    <input id="HdnFldIsAdmin" hidden="hidden" value="N" />
                                </div>                           
                               
                                 <div class="form-group formlay">
                                    <label for="exampleInputEmail1">Image:</label> <br> <b>Note : Image size should be less than 1MB</b>
                                    <asp:FileUpload ID="partyImgFileUpload" runat="server" onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                       ControlToValidate ="comImgFileUpload" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic"> 
                                    </asp:RegularExpressionValidator>
                                   <img id="imgpreview" width="200"  src="" style="border-width: 0px; min-height:132px !important; display: none;" runat="server" />
                               </div>
                                 <div class="form-group formlay">
                                    <span id="divblock" runat="server" class="hidden">
                                        <label for="exampleInputEmail1">Blocked Reason:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" class="form-control" runat="server" maxlength="100" id="BlockReason" placeholder="Enter Block Reason" tabindex="25"></span>
                                </div>
                              <%--   <div class="fform-group formlay">
                                    <label for="exampleInputEmail1">Image:</label> <br> <b>Note : Image size should be less than 1MB</b>
                                    <asp:FileUpload ID="partyImgFileUpload" runat="server" onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                       ControlToValidate ="comImgFileUpload" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic"> 
                                    </asp:RegularExpressionValidator>
                                   <img id="imgpreview" height="200" width="200"   src="" style="border-width: 0px; display: none;" runat="server" />
                               </div>--%>                              
                            </div>
                        </div>
                            <%--    </ContentTemplate>
                        </asp:UpdatePanel>--%>
                        <div class="box-footer">
                             <asp:HiddenField ID="HiddemPartyType" runat="server" />
                            <asp:HiddenField ID="HiddenPartyCity" runat="server" />
                            <asp:HiddenField ID="HiddenPartyIndustry" runat="server" />
                            <asp:HiddenField ID="HiddenPartyArea" runat="server" />
                            <asp:HiddenField ID="HiddenPartyBeat" runat="server" />
                            <asp:HiddenField ID="HiddenPartyDistributor" runat="server" />
                            <asp:HiddenField ID="HiddenStateID" runat="server" />
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

        <%--<div class="box-body" id="rptmain" runat="server" style="display: none;">--%>
        <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div id="spinner1" class="spinner" style="display: none;">
            <img id="img-spinner1" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
        </div>
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Party List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" OnClick="btnBack_Click" runat="server" Text="Back" class="btn btn-primary" />

                            </div>
                            </div>
                              <div class="box-header col-md-8">
                                 <div class="row">
                             <div class="form-group col-md-5">                                   
                               <%-- State :--%>
                                <label for="exampleInputEmail1">State :</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddlstatelist" runat="server" onChange="BindCity();" CssClass="form-control"></asp:DropDownList>
                                  <input type="hidden" id="hidstate" />
                                  <input type="hidden" id="hidcity" />
                                  <input type="hidden" id="hidarea" />
                             </div>
                             <div class="form-group col-md-5">   
                                 <label for="exampleInputEmail1">City :</label>                             
                                 <asp:DropDownList ID="ddlcitylist" runat="server" onChange="BindArea();"  CssClass="form-control"></asp:DropDownList>
                              </div>
                              <div class="form-group col-md-5">                         
                                <label for="exampleInputEmail1">Area :</label>      
                                <asp:DropDownList ID="ddlAreaPartylist" runat="server" CssClass="form-control"></asp:DropDownList>
                               </div>
                               <div class="col-md-2">
                                   <label style="display:block;visibility:hidden;margin:0;">fgk</label>
                                   <input type="button" runat="server" onclick="btnSubmitfunc();" class="btn btn-primary" value="Go" />  
                                    <asp:Button Style="margin:0;padding: 3px 18px;" type="button" ID="btnGo" OnClick="btnGo_Click" runat="server" Text="Go" Visible="false" class="btn btn-primary" OnClientClick="loding()"   />
                               </div>
                               </div>
                       </div>
                        <div class="clearfix"></div>
                        <!-- /.box-header -->
                        <div id="rptparty" class="box-body table-responsive">
                            <asp:Repeater ID="rpt12" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Party Name</th>
                                                  
                                                <th>Distributor Name</th>
                                                <th>City</th>
                                                <th>Area</th>
                                                <th>Beat</th>
                                                <th>Contact Person</th>
                                                <th>Party Type</th>
                                                <th>Industry</th>
                                                <th>Mobile</th>
                                                <th>Sync ID</th>
                                                <th>Active</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%-- <tr onclick="DoNav('<%#Eval("PartyId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("PartyId") %>' />
                                        <td><%#Eval("PartyName") %></td>
                                        <td><%#Eval("Distributor") %></td>
                                        <td><%#Eval("city") %></td>
                                        <td><%#Eval("Area") %></td>
                                        <td><%#Eval("Beat") %></td>
                                        <td><%#Eval("ContactPerson") %></td>
                                        <td><%#Eval("PartyTypeName") %></td>
                                        <td><%#Eval("IndName") %></td>
                                        <td><%#Eval("Mobile") %></td>
                                        <td><%#Eval("SyncId") %></td>
                                        <td><%#Eval("Active") %></td>
                                    </tr>--%>
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
                alert('ok1;');
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
    <script type="text/javascript">
        $(document).ready(function () {
            var valLength = "";
            $("#SyncId").keypress(function (key) {
                alert('ok;');
                valLength = ($("#SyncId").val().length + 1);

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


   <%-- <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();

        });
    </script>--%>
    <script>
        function OnSuccess(response) {
            //alert(JSON.stringify(response.d));
            //console.log(response.d);
            //$('div[id$="rptmain"]').show();
            var data = JSON.parse(response.d);
            //alert(data);
            //var arr1 = data.length;
            //alert(arr1);
          //  console.log(data);
            var table = $('#rptparty table').DataTable();
            if (table) {
                table.destroy();
                // $("#rptparty table").empty();
                $('#rptparty table tbody > tr').remove();
            }
           
            if (typeof dataTable === 'undefined') {
               
        $("#rptparty table").DataTable({
                    "order": [[0, "asc"]],

                    "aaData": data,
                    "aoColumns": [
                 {
                     "mData": "partyname",

                 },
                { "mData": "Distributor" },
                { "mData": "city" },
                { "mData": "Area" },
                { "mData": "beat" },
                { "mData": "ContactPerson" },
                { "mData": "PartyTypeName" },
                { "mData": "IndName" },
                { "mData": "Mobile" },
                { "mData": "SyncId" },
                { "mData": "Active" }

                    ]
        });
       
      
            }
            $('#rptparty table').on('click', 'tr', function () {
               
                var data1 = $('#rptparty table').DataTable().row(this).data();
              
              // console.log(data1);
               console.log('You clicked on ' + data1['partyid'] + '\'s row');
               var PartyId = data1['partyid'];
               if (PartyId != "")
               {
                    document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                    document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                    $('#spinner').show();
                    __doPostBack('', PartyId)
               }
            });

            //alert('a');
            $('#spinner').hide();
        }

    </script>
</asp:Content>


