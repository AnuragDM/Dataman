<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ItemMaster.aspx.cs" Inherits="AstralFFMS.ItemMaster" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--  <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
     <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <style>
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
    </style>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>
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
         #example1_wrapper .row {
            margin-right: 0px !important;
            margin-left: 0px !important;
        }

        #example1_wrapper .row .col-sm-12 {
            overflow-x: scroll !important;
            padding-left: 0px !important;
            margin-bottom: 10px;
        }

        .tdedit {
            cursor: pointer;
            color: #037fff;
            text-align: center !important;
        }

        .tdeditic {
            font-family: 'FontAwesome';
        }


        @media (max-width: 600px) {
            .formlay {
                width: 100% !important;
                height: 60px;
            }
        }
    </style>
    <script type="text/javascript">
        $(document).ready(
            function () {
                BindItemSegment();
                BindItemClass();
                BindItemGroup();
                Bindunit();
            });
    </script>
    <script type="text/javascript">
        function Bindunit() {

            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
            // var obj = { SegmentId: 0 };
            $('#<%=ddlunit.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $('#<%=ddlprimaryunit.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $('#<%=ddlsecondaryunit.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: pageUrl + '/PopulateMastunit',
                //   data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ddlMastSegment.ClientID %>"));
               }
               function PopulateControl(list, control) {
                   if (list.length > 0) {
                       //  control.removeAttr("disabled");
                       $('#<%=ddlunit.ClientID %>').empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $('#<%=ddlprimaryunit.ClientID %>').empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $('#<%=ddlsecondaryunit.ClientID %>').empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(list, function () {
                        $('#<%=ddlunit.ClientID %>').append($("<option></option>").val(this['Value']).html(this['Text']));
                        $('#<%=ddlprimaryunit.ClientID %>').append($("<option></option>").val(this['Value']).html(this['Text']));
                        $('#<%=ddlsecondaryunit.ClientID %>').append($("<option></option>").val(this['Value']).html(this['Text']));
                    });
                    var id = $('#<%=HiddenUnit.ClientID%>').val();
                    //  alert(id);
                    if (id != "") {
                        $('#<%=ddlunit.ClientID%>').val(id);
                    }

                    var id1 = $('#<%=HiddenPriUnit.ClientID%>').val();
                    //  alert(id);
                    if (id1 != "") {
                        $('#<%=ddlprimaryunit.ClientID%>').val(id1);
                    }
                    var id2 = $('#<%=HiddenSecUnit.ClientID%>').val();
                    //  alert(id);
                    if (id2 != "") {
                        $('#<%=ddlsecondaryunit.ClientID%>').val(id2);
                    }
                }
                else {
                    $('#<%=ddlunit.ClientID %>').empty().append('<option selected="selected" value="0">Not available<option>');
                    $('#<%=ddlprimaryunit.ClientID %>').empty().append('<option selected="selected" value="0">Not available<option>');
                    $('#<%=ddlsecondaryunit.ClientID %>').empty().append('<option selected="selected" value="0">Not available<option>');
                }

            }
        }
    </script>
    <script type="text/javascript">
        function BindItemSegment() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
            var obj = { SegmentId: 0 };
            $('#<%=ddlMastSegment.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
               $.ajax({
                   type: "POST",
                   url: pageUrl + '/PopulateItemSegment',
                   data: JSON.stringify(obj),
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   success: OnPopulated,
                   failure: function (response) {
                       alert(response.d);
                   }
               });
               function OnPopulated(response) {
                   PopulateControl(response.d, $("#<%=ddlMastSegment.ClientID %>"));
            }
            function PopulateControl(list, control) {
                if (list.length > 0) {
                    //  control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(list, function () {
                        control.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });
                    var id = $('#<%=HiddenSegmentID.ClientID%>').val();
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
        /////////////////ItemClass

        function BindItemClass() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
            var obj = { ClassId: 0 };
            $('#<%=ddlMastClass.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: pageUrl + '/PopulateItemClass',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ddlMastClass.ClientID %>"));
               }
               function PopulateControl(list, control) {
                   if (list.length > 0) {
                       //  control.removeAttr("disabled");
                       control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                       $.each(list, function () {
                           control.append($("<option></option>").val(this['Value']).html(this['Text']));
                       });
                       var id = $('#<%=HiddenClassID.ClientID%>').val();
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
        /////Item Group

        function BindItemGroup() {
            var pageUrl = '<%=ResolveUrl("~/DataBindService_New.asmx")%>'
            var obj = { ParentId: 0 };
            $('#<%=ddlUnderItem.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: pageUrl + '/PopulateItemGroup',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ddlUnderItem.ClientID %>"));
            }
            function PopulateControl(list, control) {
                if (list.length > 0) {
                    //  control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">-- Select --</option>');
                    $.each(list, function () {
                        control.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });
                    var id = $('#<%=HiddenGroupID.ClientID%>').val();
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
    </script>
    <script type="text/javascript">
        function changeunit(dropdwon) {
            if ($("#<%=ddlprimaryunit.ClientID%>").val() == dropdwon) {
                errormessage("Plese select different unit");
                $("#<%=ddlunit.ClientID%>").val(0);
                $("#<%=ddlunit.ClientID%>").focus();
                return false;

            }


            if ($("#<%=ddlsecondaryunit.ClientID%>").val() == dropdwon) {
                errormessage("Plese select different unit");
                $("#<%=ddlunit.ClientID%>").val(0);
                $("#<%=ddlunit.ClientID%>").focus();
                return false;

            }

        }

        function changepriunit(dropdwon) {

            if (dropdwon == $("#<%=ddlunit.ClientID%>").val()) {
                errormessage("Plese select different unit");
                $("#<%=ddlprimaryunit.ClientID%>").val(0);
                $("#<%=ddlprimaryunit.ClientID%>").focus();
                return false;

            }
            if (dropdwon == $("#<%=ddlsecondaryunit.ClientID%>").val()) {
                errormessage("Plese select different unit");
                $("#<%=ddlprimaryunit.ClientID%>").val(0);
                $("#<%=ddlprimaryunit.ClientID%>").focus();
                return false;

            }
        }

        function changesecunit(dropdwon) {
            if (dropdwon == $("#<%=ddlunit.ClientID%>").val()) {
                errormessage("Plese select different unit");
                $("#<%=ddlsecondaryunit.ClientID%>").val(0);
                $("#<%=ddlsecondaryunit.ClientID%>").focus();
                return false;

            }
            if (dropdwon == $("#<%=ddlprimaryunit.ClientID%>").val()) {
                errormessage("Plese select different unit");
                $("#<%=ddlsecondaryunit.ClientID%>").val(0);
                $("#<%=ddlsecondaryunit.ClientID%>").focus();
                return false;

            }
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
            if ($('#<%=ItemName.ClientID%>').val() == "") {
                errormessage("Please enter Item Name");
                return false;
            }

           <%-- var value = ($('#<%=ItemName.ClientID%>').val().charAt(0));
            var chrcode = value.charCodeAt(0);
            if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                errormessage("Do not start Name with special characters")
                return false;
            }--%>
            if ($('#<%=ddlUnderItem.ClientID%>').val() == "0" || $('#<%=ddlUnderItem.ClientID%>').val() == "") {
                errormessage("Please select Material Group");
                return false;
            }
            if ($('#<%=ddlMastClass.ClientID%>').val() == "0" || $('#<%=ddlMastClass.ClientID%>').val() == "") {
                errormessage("Please select Item Class");
                return false;
            }

            if ($('#<%=ddlMastSegment.ClientID%>').val() == "0" || $('#<%=ddlMastSegment.ClientID%>').val() == "") {
                errormessage("Please select Item Segment");
                return false;
            }
          <%--  if ($('#<%=Itemcode.ClientID%>').val() == "") {
                errormessage("Please enter Item Code");
                return false;
            }--%>
            if ($('#<%=ddlunit.ClientID%>').val() == "0" || $('#<%=ddlunit.ClientID%>').val() == "") {
                errormessage("Please enter Unit");
                return false;
            }
            if ($('#<%=StdPack.ClientID%>').val() == "") {
                errormessage("Please enter Standard Packing");
                return false;
            }
            if ($('#<%=PriceGroup.ClientID%>').val() == "") {
                errormessage("Please enter Price Group");
                return false;
            }
            var uploadcontrol = document.getElementById('<%=comImgFileUpload.ClientID%>').value;
            var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
            if (uploadcontrol.length > 0) {
                if (reg.test(uploadcontrol)) {
                    return true;
                }
                else {
                    errormessage("Only image files are allowed!");
                    return false;
                }
            }
            $('#<%=HiddenGroupID.ClientID%>').val($('#<%=ddlUnderItem.ClientID%>').val());
            $('#<%=HiddenClassID.ClientID%>').val($('#<%=ddlMastClass.ClientID%>').val());
            $('#<%=HiddenSegmentID.ClientID%>').val($('#<%=ddlMastSegment.ClientID%>').val());

            $('#<%=HiddenUnit.ClientID%>').val($('#<%=ddlunit.ClientID%>').val());
            $('#<%=HiddenPriUnit.ClientID%>').val($('#<%=ddlprimaryunit.ClientID%>').val());
            $('#<%=HiddenSecUnit.ClientID%>').val($('#<%=ddlsecondaryunit.ClientID%>').val());
        }
    </script>


    <script type="text/javascript">
        var z = 0;
        function Showpic(value) {
            $get("ImgModal").src = value;
            $('#ShowPictureModal').modal('show');
        };
        function showpreview(input) {
            console.log($('#ContentPlaceHolder1_comImgFileUpload'));
            var Max_size = 1048576;
            //  $('#<%=divimsg.ClientID%>').empty();
             if ($('#<%=divimsg.ClientID%>')[0].childNodes.length == 0)
                 k = $('#<%=divimsg.ClientID%>')[0].childNodes.length + 1;
            else
                k = parseInt($('#<%=divimsg.ClientID%>')[0].childNodes[$('#<%=divimsg.ClientID%>')[0].childNodes.length - 1].id.split('_')[1]) + 1;

            console.log($('#<%=divimsg.ClientID%>')[0].childNodes.length)
             if ($('#<%=divimsg.ClientID%>')[0].childNodes.length + input.files.length > 5) {
                 errormessage("Only 5 files are allowed!");
                 //   $("#ContentPlaceHolder1_imgpreview").css('display', 'none');
                 return false;
             }

             var uploadcontrol = document.getElementById('<%=comImgFileUpload.ClientID%>').value;
             var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
             if (uploadcontrol.length > 0) {

                 if (reg.test(uploadcontrol)) {
                     if (input.files) {
                         if (input.files.length > 5) {
                             errormessage("Only 5 files are allowed!");
                             //   $("#ContentPlaceHolder1_imgpreview").css('display', 'none');
                             return false;
                         }
                         var selectedfile;
                         for (var i = 0; i < input.files.length; i++) {
                             console.log(input.files[0].size)
                             selectedfile = input.files[0];
                             //if(selectedfile.size>Max_size‬)
                             //   {
                             //       errormessage("image size should be less than 1MB");
                             //       $("#ContentPlaceHolder1_imgpreview").css('display', 'none');
                             //       return false;
                             //   }
                             // else
                             //   {
                             var reader = new FileReader();
                             reader.onload = function (e) {
                                 console.log(e)
                                 // $('#<%=divimsg.ClientID%>').html('<div class="row"><img id="theImg ' + i + ' src=' + e.target.result + ' width=150 /></div>')
                                $('<div />').attr({
                                    'class': 'col-md-2',
                                    'id': 'div_' + k,
                                    'style': 'margin-top:10px'
                                }).appendTo('#<%=divimsg.ClientID%>');
                                // $('<input type="button" id="btn#'+k+'" class="btn btn-primary"  value="Remove" onclick="Remove('+ k +',this.id,'+z+')" style="margin-right:10px"/>').appendTo('#div_' + k);
                                $('<img />').attr({
                                    // 'class': 'row',
                                    //'runat': 'server',
                                    // 'id': 'ContentPlaceHolder1_theImg' + k,
                                    'src': e.target.result,
                                    'width': 60,
                                    'height': 60,
                                    'onclick': 'Showpic(this.src)'

                                }).appendTo('#div_' + k);
                                $('<a id="btn#' + k + '" onclick="Remove(' + k + ',this.id,' + z + ')" style="margin-left:10%"> <i class="fa fa-trash-o" style="font-size:large;"></i></a>').appendTo('#div_' + k);



                                if ($('#<%=hidimg.ClientID%>').val() == "") {
                                    $('#<%=hidimg.ClientID%>').val(e.target.result)
                                    }
                                    else {
                                        $('#<%=hidimg.ClientID%>').val($('#<%=hidimg.ClientID%>').val() + "@#dataman$&" + e.target.result)
                                    }
                                z = z + 1;
                                k = k + 1;

                            }
                                reader.readAsDataURL(input.files[i]);

                            }

                        }
                    }
                    else {
                        errormessage("Only image files are allowed!");
                        $("#ContentPlaceHolder1_imgpreview").css('display', 'none');
                        return false;
                    }
                }
                $('#ContentPlaceHolder1_comImgFileUpload').val('');
                console.log($('#<%=hidimg.ClientID%>').val());


            }
            function Remove(j, id, i) {
                console.log(id.split("#")[0])
                if (id.split("#")[0] == "btn") {
                    $('#div_' + j).remove();
                    console.log($('#<%=hidimg.ClientID%>').val().split('@#dataman$&'))
                var arr = $('#<%=hidimg.ClientID%>').val().split('@#dataman$&');
                $('#<%=hidimg.ClientID%>').val('');
                if (i != -1) {
                    for (var k = 0; k < arr.length; k++) {
                        if (i == k) {

                        }
                        else {
                            if ($('#<%=hidimg.ClientID%>').val() == "") {
                                $('#<%=hidimg.ClientID%>').val(arr[k])
                            }
                            else {
                                $('#<%=hidimg.ClientID%>').val($('#<%=hidimg.ClientID%>').val() + "@#dataman$&" + arr[k])
                            }
                        }
                    }
                }
            }
            else {
                $('#' + id)[0].parentElement.remove();

                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("ItemMaster.aspx/Deleteitemimage") %>',
                    contentType: "application/json; charset=utf-8",
                    data: '{id:"' + id + '"}',
                    dataType: "json",
                    success: function (data) {
                        //Successmessage("Note Deleted");
                        //if ($('#listid').val() == "Notes") {
                        //    GetTaskNotes(0);
                        //}
                        //else {
                        //    GetCompleteHis(0);
                        //}

                        //$("#hidnoteId").val('');
                        //clrNotes();

                    }
                });
            }
            // alert(j)
        }
    </script>
    <%--  <script type="text/javascript">
        $(document).ready(function () {
            var valLength = "";
            $('#<%=ItemName.ClientID%>').keypress(function (key) {

                valLength = ($('#<%=ItemName.ClientID%>').val().length + 1);

                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
        });
    </script>--%>
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
        function DoNav(ItemId) {
            if (ItemId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', ItemId)
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
                            <%-- <h3 class="box-title">Product Master</h3>--%>
                            <h3 class="box-title">
                                <asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                    OnClick="btnFind_Click" />

                                <%--  <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" runat="server" />--%>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group col-md-6">
                                        <input id="ItemId" hidden="hidden" />
                                        <label for="exampleInputEmail1">Product Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="50" id="ItemName" placeholder="Enter Item Name" >
                                    </div>
                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Product Group:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlUnderItem" Width="100%" CssClass="form-control" runat="server" ></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Product Class:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlMastClass" Width="100%" CssClass="form-control" runat="server" ></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Product Segment:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlMastSegment" Width="100%" CssClass="form-control" runat="server" ></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Product Code:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="20" id="Itemcode" placeholder="Enter Item Code" >
                                    </div>

                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">MRP:</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="15" id="MRP" onkeypress="javascript:return isNumber (event)" placeholder="Enter MRP" >
                                    </div>

                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Retailer Price:</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="15" id="RP" onkeypress="javascript:return isNumber (event)" placeholder="Enter Retailer Price" >
                                    </div>

                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Distributor Price:</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="15" id="DP" onkeypress="javascript:return isNumber (event)"
                                            placeholder="Enter Distributor Price" >
                                    </div>

                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Standard Packing:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="15" id="StdPack" onkeypress="javascript:return isNumber (event)" placeholder="Enter Standard Packing" >
                                    </div>
                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Price Group:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="15" id="PriceGroup" placeholder="Enter Price Group" >
                                    </div>
                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Sync Id:</label>
                                        <input type="text" runat="server" class="form-control" maxlength="150" id="SyncId" placeholder="Enter Sync Id" >
                                    </div>



                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Unit:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlunit" Width="100%" CssClass="form-control" runat="server"  onchange="changeunit(this.value);"></asp:DropDownList>
                                        <%--<input type="text" runat="server" class="form-control" maxlength="20" id="Unit" placeholder="Enter Unit" tabindex="23">--%>
                                    </div>
                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Primary Unit:</label>
                                        <asp:DropDownList ID="ddlprimaryunit" Width="100%" CssClass="form-control" runat="server" onchange="changepriunit(this.value);"></asp:DropDownList>
                                        <%--<input type="text" runat="server" class="form-control" maxlength="20" id="txtprimaryunit" placeholder="Enter Unit" tabindex="24">--%>
                                    </div>

                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Primary Unit Conversion Factor:</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="15" onkeypress="javascript:return isNumber (event)" id="txtprimarycon" placeholder="Enter Primary Unit Factor" >
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Secondary Unit:</label>
                                        <asp:DropDownList ID="ddlsecondaryunit" Width="100%" CssClass="form-control" runat="server"  onchange="changesecunit(this.value);"></asp:DropDownList>
                                        <%--<input type="text" runat="server" class="form-control" maxlength="20" id="txtSecondaryunit" placeholder="Enter Unit" tabindex="26">--%>
                                    </div>
                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Secondary Unit Conversion Factor:</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="15" onkeypress="javascript:return isNumber (event)" id="txtSecondarycon" placeholder="Enter Secondary Unit Factor" >
                                    </div>
                                     <div class="clearfix"></div>
                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">MOQ:</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="15" onkeypress="javascript:return isNumber (event)" id="txtminimumorderquantity" placeholder="Enter Minimum Order Quantity" >
                                    </div>
                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">CGST Per:</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="15" onkeypress="javascript:return isNumber (event)" id="txtcgstper" placeholder="Enter CGST Percentage" >
                                    </div>
                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">SGST Per:</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="15" onkeypress="javascript:return isNumber (event)" id="txtsgstper" placeholder="Enter SGST Percentage" >
                                    </div>
                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">IGST Per:</label>
                                        <input type="text" runat="server" class="form-control numeric text-right" maxlength="15" onkeypress="javascript:return isNumber (event)" id="txtigstper" placeholder="Enter IGST Percentage" >
                                    </div>
                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Is Promoted:</label>
                                        <input id="chkispromoted" runat="server" type="checkbox" class="checkbox"  />
                                        <input id="HdnFldIspromoted" hidden="hidden" value="N" />
                                    </div>
                                    <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1">Active:</label>
                                        <input id="chkIsAdmin" runat="server" type="checkbox" checked="checked" class="checkbox" />
                                        <input id="HdnFldIsAdmin" hidden="hidden" value="N" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <%--<label for="exampleInputEmail1">Image:</label>--%>
                                        <asp:FileUpload ID="comImgFileUpload" runat="server" onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" AllowMultiple="true" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                            ControlToValidate="comImgFileUpload" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                        </asp:RegularExpressionValidator>
                                        <asp:HiddenField ID="hidimg" runat="server" Value="" />
                                        <img id="imgpreview" height="200" width="200" src="" style="border-width: 0px; display: none;" runat="server" />
                                    </div>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-md-8">
                                    <div id="divimsg" runat="server"></div>
                                </div>
                            </div>
                        </div>
                        <div class="box-footer">
                            <asp:HiddenField ID="HiddenSegmentID" runat="server" />
                            <asp:HiddenField ID="HiddenClassID" runat="server" />
                            <asp:HiddenField ID="HiddenGroupID" runat="server" />
                            <asp:HiddenField ID="HiddenUnit" runat="server" />
                            <asp:HiddenField ID="HiddenPriUnit" runat="server" />
                            <asp:HiddenField ID="HiddenSecUnit" runat="server" />
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
                            <h3 class="box-title">Product List</h3>
                            <div style="float: right">
                                <asp:Button type="button" ID="btnExport" OnClick="btnExport_Click" runat="server" Text="Export" Visible="true" class="btn btn-primary" />
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
                                                <th>Action</th>
                                                <th>Product Name</th>
                                                <th>Product Group</th>
                                                <th>Product Code</th>
                                                <th>Unit</th>
                                                <th>Product Segment</th>
                                                <th>Product Class</th>
                                                <th>MRP</th>
                                                <th>DP</th>
                                                <th>RP</th>
                                                <th>Std.Pack</th>
                                                <th>Price Group</th>
                                                <th>Sync ID</th>
                                                <th>Active</th>
                                                <th>Images</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>

                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("ItemId") %>' />
                                        <td onclick="DoNav('<%#Eval("ItemId") %>');" class="tdedit"><i class="fa fa-pencil tdeditic" aria-hidden="true"></i></td>
                                        <td><%#Eval("ItemName") %></td>
                                        <%--onclick="DoNav('<%#Eval("ItemId") %>');"--%>
                                        <td><%#Eval("MaterialGroup") %></td>
                                        <td><%#Eval("ItemCode") %></td>
                                        <td><%#Eval("Unit") %></td>
                                        <td><%#Eval("Segment") %></td>
                                        <td><%#Eval("ProductClass") %></td>
                                        <td><%#Eval("Mrp") %></td>
                                        <td><%#Eval("Dp") %></td>
                                        <td><%#Eval("Rp") %></td>
                                        <td><%#Eval("StdPack") %></td>
                                        <td><%#Eval("PriceGroup") %></td>
                                        <td><%#Eval("SyncId") %></td>
                                        <td><%#Eval("Active") %></td>
                                        <td><%#Eval("Imagehtml") %></td>
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
        <div class="modal fade" id="ShowPictureModal" role="dialog">
            <div class="modal-dialog" style="width: 100%; max-width: 750px; min-width: 350px">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="container">
                            <img id="ImgModal" src="Uploads/Chrysanthemum.jpg" class="user-image" style="max-width: 700px; min-width: 300px" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <script type="text/javascript">
        $(document).ready(function () {
            var valLength = "";
            $("#ItemName").keypress(function (key) {

                valLength = ($("#ItemName").val().length + 1);

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

