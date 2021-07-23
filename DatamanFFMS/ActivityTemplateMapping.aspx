<%@ Page Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ActivityTemplateMapping.aspx.cs" Inherits="AstralFFMS.ActivityTemplateMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js"></script>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <script type="text/javascript">
        var EditFlg = '<%= Request.QueryString["EditFlg"]%>';
        var opt1 = '<%= Request.QueryString["TempId"]%>';

        $(document).ready(function () {


            if ($('#<%=hidsave.ClientID%>').val() == "true") {
                document.getElementById("btnsave").disabled = false;
            }
            else {
                document.getElementById("btnsave").disabled = true;
            }


            $("#btnCancel").click(function () {
             $("#ContentPlaceHolder1_hdnHeader").val("");
            });
            if (EditFlg == '1')
            {
                document.getElementById("btnFind").style.display = "none";
                PageLoad();
            }
            else
            {
                BindState();
                BindTitle();
              
            }
            
        });

        function PageLoad()
        {
            var obj;
            var getMainData; var getTitleData; var getStateData; var getCityData; var getDistData;var getSMIdData;var distid;var  smid; var state = ""; var city = "";
            debugger;
            if (EditFlg >= "" && opt1 >= "")
            {
                //#region GetTemplateDataByid

                $.ajax({
                    type: "POST",
                    url: 'ActivityTemplateMapping.aspx/GetMappedActivityTemplateById',
                    data: '{TempId:' + opt1 + '}',
                    contentType: "application/json; charset=utf-8",
                    //asyn: false,
                    dataType: "json",
                    success: function (response) {
                        getMainData = JSON.parse(response.d);
                        distid = getMainData[0].distid.trim();
                        smid = getMainData[0].smid.trim();                        

                        if (distid != "") {
                            state = getMainData[0].DistState.trim();
                            city = getMainData[0].Distcityid.trim();
                            $("#<%=HiddenState.ClientID %>").val(state);
                            $("#<%=HiddenCity.ClientID %>").val(city);
                            $("#<%=HiddenDist.ClientID %>").val(getMainData[0].distid);
                        }
                        else if (smid != "") {
                            state = getMainData[0].sPState.trim();
                            city = getMainData[0].SMIDcityid.trim();
                            $("#<%=HiddenState.ClientID %>").val(state);
                            $("#<%=HiddenCity.ClientID %>").val(city);
                            $("#<%=HiddenSalesp.ClientID %>").val(getMainData[0].smid);
                        }
                        $("#txtRemark").val(getMainData[0].Remark);
                        $("#FromDate").val(getMainData[0].FromDate);
                        $("#ToDate").val(getMainData[0].ToDate);
                        document.getElementById("btnsave").value = "Update";
                        if ($('#<%=hidupdate.ClientID%>').val() == "true") {
                   
                            document.getElementById("btnsave").disabled = false;
                        }
                        else {
                            document.getElementById("btnsave").disabled = true;
                        }



                        // #region GetSetTitleData
                        $.ajax({
                            type: "POST",
                            url: 'ActivityTemplateMapping.aspx/BindTitle',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                            success: function (response1) {
                                getTitleData = JSON.parse(response1.d);

                                $('#<%=LstTitle.ClientID %>').empty();
                                $.each(getTitleData, function () {
                                    $('#<%=LstTitle.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');
                                });
                                $("#<%=LstTitle.ClientID %>").val(getMainData[0].TemplateHeaderId);
                                $("#<%=HiddenTitle.ClientID %>").val(getMainData[0].TemplateHeaderId);
                                $("#<%=LstTitle.ClientID %>").multiselect('rebuild');
                               
                            }
                        });// #endregion
                        

                        // #region GetSetStateData
                        $.ajax({
                            type: "POST",
                            url: 'ActivityTemplateMapping.aspx/PopulateState',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                            success: function (response2) {
                                 getStateData = JSON.parse(response2.d);
                                $('#<%=lstState.ClientID %>').empty();
                                $.each(getStateData, function () {
                                    $('#<%=lstState.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');
                                });

                                var values = state;
                                $.each(values.split(","), function (i, e) {
                                    $("#ContentPlaceHolder1_lstState option[value='" + e.trim() + "']").prop("selected", true);
                                });                          
                                $("#<%=lstState.ClientID %>").multiselect('rebuild');

                            }
                        }); // #endRegion
                        
                        // #region BindCity
                        obj = { StateID: state };
                        $.ajax({
                            type: "POST",
                            url: 'ActivityTemplateMapping.aspx/PopulateCityByMultiState',
                            data: JSON.stringify(obj),
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                            success: function (response3) {
                                getCityData = JSON.parse(response3.d);

                                $('#<%=lstCity.ClientID %>').empty();

                                $.each(getCityData, function () {
                                    $('#<%=lstCity.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');
                                });

                                var values = city;
                                $.each(values.split(","), function (i, e) {
                                    $("#ContentPlaceHolder1_lstCity option[value='" + e.trim() + "']").prop("selected", true);
                                });

                                $("#<%=lstCity.ClientID %>").multiselect('rebuild');

                            }
                        });
                        // #endregion

                        //#region BindSalesPerson
                        obj = { AreaId: $("#<%=HiddenCity.ClientID %>").val(), Status: 'C' };
                        $.ajax({
                            type: "POST",
                            url: 'ActivityTemplateMapping.aspx/PopulateSalesPersonSearch',
                            data: JSON.stringify(obj),
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                            success:function (response5) {
                                getSMIdData = JSON.parse(response5.d);

                                $('#<%=lstSalesp.ClientID %>').empty();
                                $.each(getSMIdData, function () {
                                    $('#<%=lstSalesp.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');
                                });
                                var values = smid;
                                $.each(values.split(","), function (i, e) {
                                    $("#ContentPlaceHolder1_lstSalesp option[value='" + e.trim() + "']").prop("selected", true);
                                });
                                $("#<%=HiddenSalesp.ClientID %>").val(smid);
                                $("#<%=lstSalesp.ClientID %>").multiselect('rebuild');
                            }
                        });
                        //#endregion

                        // #region BindDistributor
                        obj = { AreaId: $("#<%=HiddenCity.ClientID %>").val(), Status: 'C' };
                        $.ajax({
                            type: "POST",
                            url: 'ActivityTemplateMapping.aspx/PopulatePartyDistributorSearch',
                            data: JSON.stringify(obj),
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                            success: function (response4) {
                                getDistData = JSON.parse(response4.d)

                                $('#<%=lstDistributor.ClientID %>').empty();
                                $.each(getDistData, function () {
                                    $('#<%=lstDistributor.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');
                                });

                                var values = distid;
                                $.each(values.split(","), function (i, e) {
                                    $("#ContentPlaceHolder1_lstDistributor option[value='" + e.trim() + "']").prop("selected", true);
                                });
                                $("#<%=HiddenDist.ClientID %>").val(distid);
                                $("#<%=lstDistributor.ClientID %>").multiselect('rebuild');
                            }
                        });

                        // #endregion

                    }
                }); //#endregion
            }
        }
      
        var fieldoldname = "";
        var rowindex = -1;
        $(function () {
            var date = new Date();
            date.setDate(date.getDate());
            $(".datepicker").datepicker({ startDate: date, format: 'dd/M/yyyy', autoclose: true });
            $("#FromDate").datepicker('setDate', date);
            $("#ToDate").datepicker('setDate', date);

        });


        function compare() {

            var startDate = new Date($("#FromDate").val());
            var endDate = new Date($("#ToDate").val());

            if (startDate.getTime() > endDate.getTime()) {
                $("#ToDate").val('');
                errormessage("From Date Should be less than To Date");
                return false;

            }
        }
        function onback() {
            clrcontrols();
            $("#mainDiv").show();
            $("#rptmain").hide();

        }
        function FillData() {

            $.ajax({
                type: "POST",
                url: 'ActivityTemplateMapping.aspx/GetMappedActivityTemplate',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess
            });

            $("#mainDiv").hide();
            $("#rptmain").show();
        }


        function OnSuccess(response) {
            $('div[id$="rptmain"]').show();
            var data = JSON.parse(response.d);
            var table = $('#example1').DataTable();
            table.destroy();
            $("#example1").DataTable({

                "aaData": data,
                "aoColumns": [
            {
                "mData": "SNO"
            },
            { "mData": "Title" },
            { "mData": "Fromdate" },
            { "mData": "Todate" },
            {"mData":"remark"},
           {
               "mData": "ID",
               "render": function (data, type, row, meta) {
                   return ("<a onclick='editrow(" + data + ");'><i class='fa fa fa-pencil-square-o'></i></a>");
               }
           }
                ]
            });

            $('#spinner').hide();
        }

        function clrcontrols() {
            var date = new Date();
            date.setDate(date.getDate());
            $("#txtRemark").val('');
            $("#FromDate").datepicker('setDate', date);
            $("#ToDate").datepicker('setDate', date);
            $("<%= HiddenTitle.ClientID%>").val('');
            $("<%= HiddenDist.ClientID%>").val('');
            $("<%= HiddenSalesp.ClientID%>").val('');
            BindState();
            BindTitle();
            $('#<%=lstCity.ClientID %>').empty();
            $("#<%=lstCity.ClientID %>").multiselect('rebuild');
            $('#<%=lstSalesp.ClientID %>').empty();
            $("#<%=lstSalesp.ClientID %>").multiselect('rebuild');
            $('#<%=lstDistributor.ClientID %>').empty();
            $("#<%=lstDistributor.ClientID %>").multiselect('rebuild');
        }
        function deleterow(rowdata) {
            $(rowdata).parent().parent().remove();
        }

        function editrow(rowdata) {
            var URL = window.location.href;
            URL = URL + "?EditFlg=1&TempId=" + rowdata;
            window.open(URL);
        }

        function savenew() {
            debugger
            var obj;
            if (validate()) {
                debugger
                if (EditFlg == '1') {
                    obj = '{TempHeaderId: "' + $("#<%= HiddenTitle.ClientID%>").val() + '",fromdate: "' + $("#FromDate").val() + '",todate:"' + $("#ToDate").val() + '",visibleDistid: "' + $("#<%= HiddenDist.ClientID%>").val() + '",visibleSMID: "' + $("#<%= HiddenSalesp.ClientID%>").val() + '",EditFlg: "1",MapId: "' + opt1 + '",Remark:"' + $("#txtRemark").val() + '"}';
                }
                else
                {
                    obj = '{TempHeaderId: "' + $("#<%= HiddenTitle.ClientID%>").val() + '",fromdate: "' + $("#FromDate").val() + '",todate:"' + $("#ToDate").val() + '",visibleDistid: "' + $("#<%= HiddenDist.ClientID%>").val() + '",visibleSMID: "' + $("#<%= HiddenSalesp.ClientID%>").val() + '",EditFlg: "0",MapId: "0",Remark:"' + $("#txtRemark").val() + '"}';
                }
                $.ajax({
                    type: "POST",
                    url: 'ActivityTemplateMapping.aspx/SaveActivityMapping',
                    contentType: "application/json; charset=utf-8",
                    data: obj,
                    dataType: "json",
                    success: function (data) {
                        var Message = data.d;
                        if (Message == "-1") {
                            errormessage("Selected Template can be mapped only within template date range.");
                        }
                        else if (Message == "-2") {
                            errormessage("Selected Template is already mapped with selected SalesPerson or Distributor within given time range.");
                        }
                        else if (Message == "0") {
                            errormessage("Error While Template Mapping");
                        }
                        else {
                            Successmessage("Template Mapped Successfully");
                            clrcontrols();
                        }
                    }
                });
            }


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
        $(function () {
            $('[id*=lstState]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });

            $('[id*=lstCity]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=lstDistributor]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });

            $('[id*=lstSalesp]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });

            $('[id*=LstTitle]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 538,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
    </script>

    <script type="text/javascript">

        function BindCitySearch() {

            var lstCity = $("[id*=lstCity]");
            var selectedState = [];
          
                $("#<%=lstState.ClientID %> :selected").each(function () {
                    selectedState.push($(this).val());
                });
                $("#<%=HiddenState.ClientID %>").val(selectedState);
                if ($("#<%=HiddenState.ClientID %>").val() == "") { $("#<%=HiddenState.ClientID %>").val(0); }
            

            var obj = { StateID: $("#<%=HiddenState.ClientID %>").val() };
            $.ajax({
                type: "POST",
                url: 'ActivityTemplateMapping.aspx/PopulateCityByMultiState',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                async:false,
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    //alert(response.d);
                }
            });
            function OnPopulated(response) {
                var data = JSON.parse(response.d);
                PopulateControl(data, $("#<%=lstCity.ClientID %>"));
                 }
                 function PopulateControl(list, control) {
                     console.log(list)

                     $('#<%=lstCity.ClientID %>').empty();
                     $.each(list, function () {

                         $('#<%=lstCity.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');

                     });
                     $("#<%=lstCity.ClientID %>").multiselect('rebuild');
                     var id = $('#<%=HiddenCity.ClientID%>').val();
                     if (id != "") {
                         control.val(id);
                     }
                     //  
                     console.log($('#<%=lstCity.ClientID %> ul'));
                 }
                 BindDistributorSearch("S");
                 BindSalesPersonSearch("S");


             }

             function BindSalesDistSearch() {
                 var selectedState = [];
               
                     $("#<%=lstCity.ClientID %> :selected").each(function () {
                      selectedState.push($(this).val());
                  });
                  $("#<%=HiddenCity.ClientID %>").val(selectedState);

                 if ($("#<%=HiddenCity.ClientID %>").val() == "") {
                     $("#<%=HiddenCity.ClientID %>").val(0);
                 }
              BindDistributorSearch("C");
              BindSalesPersonSearch("C");
          }
          function CheckDist() {
              var selectedSmid = [];
              var selectDist = [];

              $("#<%=lstSalesp.ClientID %> :selected").each(function () {
                selectedSmid.push($(this).val());
            });
            $("#<%=lstDistributor.ClientID %> :selected").each(function () {
                selectDist.push($(this).val());
            });
            if (selectedSmid != "") {
                alert("Whether SalesPerson Or Distributor can mapped at a time.");
                $("#<%=lstDistributor.ClientID %> option:selected").prop("selected", false);
                $("#<%=lstDistributor.ClientID %>").multiselect('refresh');

            }
        }

        function CheckSales() {
            var selectedSmid = [];
            var selectDist = [];

            $("#<%=lstSalesp.ClientID %> :selected").each(function () {
                selectedSmid.push($(this).val());
            });
            $("#<%=lstDistributor.ClientID %> :selected").each(function () {
                selectDist.push($(this).val());
            });
            if (selectDist != "") {
                alert("Whether SalesPerson Or Distributor can mapped at a time.");
                $("#<%=lstSalesp.ClientID %> option:selected").prop("selected", false);
                $("#<%=lstSalesp.ClientID %>").multiselect('refresh');

            }
        }

        function BindDistBySmid() {
            var selectedState = [];

            $("#<%=lstSalesp.ClientID %> :selected").each(function () {
                selectedState.push($(this).val());
            });
            $("#<%=HiddenSalesp.ClientID %>").val(selectedState);

            if ($("#<%=HiddenSalesp.ClientID %>").val() == "") {
                $("#<%=HiddenSalesp.ClientID %>").val(0);
            }
            BindDistributorSearch('SP');

        }

        function BindDistributorSearch(fStatus) {
            var obj;



            if (fStatus === "S") obj = { AreaId: $("#<%=HiddenState.ClientID %>").val(), Status: fStatus };
            else if (fStatus === "C") obj = { AreaId: $("#<%=HiddenCity.ClientID %>").val(), Status: fStatus };
               else if (fStatus === "SP") obj = { AreaId: $("#<%=HiddenSalesp.ClientID %>").val(), Status: fStatus };
           $.ajax({
               type: "POST",
               url: 'ActivityTemplateMapping.aspx/PopulatePartyDistributorSearch',
               data: JSON.stringify(obj),
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: OnPopulated,
               failure: function (response) {
                   alert(response.d);
               }
           });
           function OnPopulated(response) {
               var data = JSON.parse(response.d);
               PopulateControl(data, $("#<%=lstDistributor.ClientID %>"));
         }
         function PopulateControl(list, control) {
             debugger;
             console.log(list)
             var lstCustomers = $("[id*=lstDistributor]");
             $('#<%=lstDistributor.ClientID %>').empty();
             $.each(list, function () {

                 $('#<%=lstDistributor.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');

                 });
                 $("#<%=lstDistributor.ClientID %>").multiselect('rebuild');
          
         }
     }
     function BindSalesPersonSearch(fStatus) {
         var obj;

         if (fStatus === "S") obj = { AreaId: $("#<%=HiddenState.ClientID %>").val(), Status: fStatus };
            else if (fStatus === "C") obj = { AreaId: $("#<%=HiddenCity.ClientID %>").val(), Status: fStatus };
                $.ajax({
                    type: "POST",
                    url: 'ActivityTemplateMapping.aspx/PopulateSalesPersonSearch',
                    data: JSON.stringify(obj),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnPopulated,
                    failure: function (response) {
                        alert(response.d);
                    }
                });
                function OnPopulated(response) {
                    var data = JSON.parse(response.d);
                    PopulateControl(data, $("#<%=lstSalesp.ClientID %>"));
           }
           function PopulateControl(list, control) {
               debugger;
               console.log(list)
               var lstCustomers = $("[id*=lstSalesp]");
               $('#<%=lstSalesp.ClientID %>').empty();
             $.each(list, function () {

                 $('#<%=lstSalesp.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');

             });

             var id = $('#<%=HiddenSalesp.ClientID%>').val();
             if (id != "") {
                 control.val(id);
             }
             //
             $("#<%=lstSalesp.ClientID %>").multiselect('rebuild');
             console.log($('#<%=lstSalesp.ClientID %> ul'));
         }
     }
     function BindState() {
         var obj;

         $.ajax({
             type: "POST",
             url: 'ActivityTemplateMapping.aspx/PopulateState',
             contentType: "application/json; charset=utf-8",
             
             dataType: "json",
             success: OnPopulated,
             failure: function (response) {
                 alert(response.d);
             }
         });
         function OnPopulated(response) {

             var data = JSON.parse(response.d);
             PopulateControl(data, $("#<%=lstState.ClientID %>"));
            }
            function PopulateControl(list, control) {

                console.log(list)
                var lstCustomers = $("[id*=lstState]");
                $('#<%=lstState.ClientID %>').empty();
               $.each(list, function () {

                   $('#<%=lstState.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');

             });

             $("#<%=lstState.ClientID %>").multiselect('rebuild');

               console.log($('#<%=lstState.ClientID %> ul'));
           }
       }



        function BindTitle() {
            var obj;
            $.ajax({
                type: "POST",
                url: 'ActivityTemplateMapping.aspx/BindTitle',
                contentType: "application/json; charset=utf-8",
                // async:false,
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {

                var data = JSON.parse(response.d);
                PopulateControl(data, $("#<%=LstTitle.ClientID %>"));
           }
           function PopulateControl(list, control) {

               console.log(list)
               $('#<%=LstTitle.ClientID %>').empty();
               $.each(list, function () {
                   $('#<%=LstTitle.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');
               });
               $("#<%=LstTitle.ClientID %>").multiselect('rebuild');
               
                console.log($('#<%=LstTitle.ClientID %> ul'));

            }
        }
       

        function validate() {

            var startDate = new Date($("#FromDate").val());
            var endDate = new Date($("#ToDate").val());
            if (startDate > endDate) {
                errormessage("From Date Should be less than To Date");
                return false;
            }
            var selectedDist = [];

            $("#<%=lstDistributor.ClientID %> :selected").each(function () {
                 selectedDist.push($(this).val());
             });


             $("#<%=HiddenDist.ClientID %>").val(selectedDist);

            var selectedSMID = [];
            $("#<%=lstSalesp.ClientID %> :selected").each(function () {
                 selectedSMID.push($(this).val());
             });
             $("#<%=HiddenSalesp.ClientID %>").val(selectedSMID);

            if (selectedDist == "" && selectedSMID == "") {
                errormessage("Please Select Distributor or Sales Person.");
                return false;
            }

            var selectedTitle = [];
            $("#<%=LstTitle.ClientID %> :selected").each(function () {
                 selectedTitle.push($(this).val());
             });

             $("#<%=HiddenTitle.ClientID %>").val(selectedTitle);

            if (selectedTitle == "") {
                errormessage("Please select Title.");
                return false;
            }
            if ($("#txtRemark").val() == "")
            {
                errormessage("Please Enter Remark.");
                return false;
            }

            return true;

        }
    </script>
    <style type="text/css">
        #btnAdd {
            margin-top: 23px;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            /*display: table;*/
        }

        .multiselect-container > li > a {
            white-space: normal;
        }

        /*.multiselect-container > li {
            width: 239px;
        }*/

        .multiselect-container > li input {
            height: auto;
        }

        /*height: auto;*/

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }
    </style>

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
        <div class="box-body" id="mainDiv">

            <div class="row">
                <div class="col-md-12">
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Activity Template Mapping</h3>
                            <div style="float: right">
                                <input type="button" class="btn btn-primary" value="Find" id="btnFind" onclick="FillData();" />

                                <%--  <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" runat="server" />--%>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <div class="box-body">
                            <div class="row">
                                <div class="col-md-3 col-sm-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">State:</label><br />
                                        <asp:ListBox ID="lstState" runat="server" onChange="BindCitySearch();" SelectionMode="Multiple"></asp:ListBox>
                                        <%-- <input type="hidden" id="HiddenState" />--%>
                                        <asp:HiddenField ID="HiddenState" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">City:</label><br />
                                        <asp:ListBox ID="lstCity" onchange="BindSalesDistSearch();" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        <%-- <input type="hidden" id="HiddenCity" />--%>
                                        <asp:HiddenField ID="HiddenCity" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Sales Person:</label>
                                        <asp:ListBox ID="lstSalesp" onchange="CheckSales();" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        <asp:HiddenField ID="HiddenSalesp" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Distributor:</label>
                                        <asp:ListBox ID="lstDistributor" runat="server" OnChange="CheckDist();" SelectionMode="Multiple"></asp:ListBox>
                                        <asp:HiddenField ID="HiddenDist" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Title:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:ListBox ID="LstTitle" runat="server" SelectionMode="Single"></asp:ListBox>
                                        <asp:HiddenField ID="HiddenTitle" runat="server" />


                                    </div>

                                </div>
                                <div class="col-md-3 col-sm-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">From Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type='text' id="FromDate" class="form-control datepicker" maxlength="20" readonly="readonly" />

                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">To Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type='text' id="ToDate" class="form-control datepicker" maxlength="20" readonly="readonly" onchange="compare();" />

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Remark:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type='text' id="txtRemark" class="form-control" maxlength="150" />
                                        <asp:HiddenField ID="HiddenField1" runat="server" />


                                    </div>

                                </div>
                            </div>



                        </div>


                        <div class="box-footer">
                            <input type="hidden" id="hidsave" runat="server" />
                            <input type="hidden" id="hidupdate" runat="server" />
                             <asp:Button Text="Check" ID="btnCheck" runat="server" OnClick="btnCheck_Click" Visible="false" />
                            <input type="button"  class="btn btn-primary" value="Save" id="btnsave" onclick="savenew();" disabled="" />
                            <input type="button" class="btn btn-primary" value="Cancel" id="btnCancel" onclick="clrcontrols();" />
                            <asp:HiddenField ID="HiddenMapId" runat="server" />
                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="box-body" id="rptmain" style="display: none;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Activity Mapped List</h3>
                            <div style="float: right">
                                <input type="button" id="btnback" class="btn btn-primary" value="Back" onclick="onback();" />

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>S.No</th>
                                                <th>FromDate</th>
                                                <th>To date</th>
                                                <th>Title</th>
                                                <th>Remark</th>
                                                <th>Action</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
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
</asp:Content>
