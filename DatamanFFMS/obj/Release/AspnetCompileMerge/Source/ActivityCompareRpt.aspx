<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/FFMS.master" CodeBehind="ActivityCompareRpt.aspx.cs" Inherits="AstralFFMS.ActivityCompareRpt" %>

<%@ Register Src="ctlCalendar.ascx" TagName="Calendar" TagPrefix="ctl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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
    <script type="text/javascript" language="javascript">
        function ConfirmOnDelete(item) {
            if (confirm("Are you sure you want to delete: " + item + "?") == true)
                return true;
            else
                return false;
        }
    </script>
      <script type="text/javascript">
          function validate() {
            <%--  if (document.getElementById("<%=ddlfgrp.ClientID%>").value == "--Select--" || document.getElementById("<%=ddlfgrp.ClientID%>").value == "0") {
                  errormessage("Please select  Group Name");
                document.getElementById("<%=ddlfgrp.ClientID%>").focus();
                return false;
            }--%>

            if (document.getElementById("<%=ddlfirstperson.ClientID%>").value == "--Select--" || document.getElementById("<%=ddlfirstperson.ClientID%>").value == "0") {
                errormessage("Please select First Person Name");
                document.getElementById("<%=ddlfirstperson.ClientID%>").focus();
                return false;
            }
          
            if (document.getElementById("<%=txt_fromdate.ClientID%>").value == "" || document.getElementById("<%=txt_fromdate.ClientID%>").value == "0") {
                errormessage("Time is not in valid Format");
                document.getElementById("<%=txt_fromdate.ClientID%>").focus();
                return false;
            }
            var digits = /^((31(?!(Apr|Jun|Sep|Nov)))|((30|29)(?!Feb))|(29(?= Feb(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])[-](Jan|Feb|Mar|May|Apr|Jul|Jun|Aug|Oct|Sep|Nov|Dec)[-]((1[6-9]|[2-9]\d)\d{2})$/;
            var digitsid = document.getElementById("<%=txt_fromdate.ClientID %>").value;
            var digitsArray = digitsid.match(digits);
            var temp;
            if (digitsArray == null) {
                errormessage("Incorrect Date Format.Please try again.");
                document.getElementById("<%=txt_fromdate.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=TextBox1.ClientID%>").value == "" || document.getElementById("<%=TextBox1.ClientID%>").value == "0") {
                errormessage("Time is not in valid Format");
                document.getElementById("<%=txt_fromdate.ClientID%>").focus();
                return false;
            }
            var digits = /^((31(?!(Apr|Jun|Sep|Nov)))|((30|29)(?!Feb))|(29(?= Feb(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])[-](Jan|Feb|Mar|May|Apr|Jul|Jun|Aug|Oct|Sep|Nov|Dec)[-]((1[6-9]|[2-9]\d)\d{2})$/;
            var digitsid = document.getElementById("<%=TextBox1.ClientID %>").value;
            var digitsArray = digitsid.match(digits);
            var temp;
            if (digitsArray == null) {
                errormessage("Incorrect Date Format.Please try again.");
                document.getElementById("<%=TextBox1.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=Txt_FromTime.ClientID%>").value == "" || document.getElementById("<%=Txt_FromTime.ClientID%>").value == "00:00") {
                errormessage("Your Time Format is incorrect. Please try again.");
                document.getElementById("<%=Txt_FromTime.ClientID%>").focus();
                return false;
            }
            var digits = /^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$/;
            var digitsid = document.getElementById("<%=Txt_FromTime.ClientID %>").value;
            var digitsArray = digitsid.match(digits);
            var temp;
            if (digitsArray == null) {
                errormessage("Your Time Format is incorrect. Please try again.");
                document.getElementById("<%=Txt_FromTime.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=TextBox2.ClientID%>").value == "" || document.getElementById("<%=TextBox2.ClientID%>").value == "00:00") {
                errormessage("Your Time Format is incorrect. Please try again.");
                document.getElementById("<%=TextBox2.ClientID%>").focus();
                return false;
            }
            var digits = /^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$/;
            var digitsid = document.getElementById("<%=TextBox2.ClientID %>").value;
            var digitsArray = digitsid.match(digits);
            var temp;
            if (digitsArray == null) {
                errormessage("Your Time Format is incorrect. Please try again.");
                document.getElementById("<%=TextBox2.ClientID %>").focus();
                return false;
            }
            var start = document.getElementById("<%=Txt_FromTime.ClientID %>").value;
            var end = document.getElementById("<%=TextBox2.ClientID %>").value;
            var dtStart = new Date("1/1/2007 " + start);
            var dtEnd = new Date("1/1/2007 " + end);
            if (Date.parse(dtStart) > Date.parse(dtEnd)) {
                errormessage("To Time Should be greater then From Time");
                document.getElementById("<%=TextBox2.ClientID %>").focus();
                return false;
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
        <div class="box-body" id="rptmain" runat="server" >
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                     
                    
                        <div class="clearfix"></div>
                                 <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
        <%--    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>--%>
               <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBvCa97dqF2Pao5NPBguRd-bNQgB4VAqLw&region=IN"></script>
            <script type="text/javascript">
                var markers = [
                <asp:Repeater ID="rptMarkers" runat="server">
                <ItemTemplate>
                            { "Person": '<%# Eval("Person") %>',
                                "Title": '<%# Eval("Title") %>',
                                "lat": '<%# Eval("lat") %>',
                                "lng": '<%# Eval("lng") %>',
                            }
                </ItemTemplate>
                <SeparatorTemplate>
                    ,
                </SeparatorTemplate>
            </asp:Repeater>
                ];
            </script>

            <script type="text/javascript">
                var markers1 = [
                <asp:Repeater ID="rptMarkers1" runat="server">
                <ItemTemplate>
                            { "Person": '<%# Eval("Person") %>',
                                "Title": '<%# Eval("Title") %>',
                                "lat": '<%# Eval("lat") %>',
                                "lng": '<%# Eval("lng") %>',
                            }
                </ItemTemplate>
                <SeparatorTemplate>
                    ,
                </SeparatorTemplate>
            </asp:Repeater>
                ];
            </script>

            <script type="text/javascript">
                var markers2 = [
                <asp:Repeater ID="rptMarkers2" runat="server">
                <ItemTemplate>
                            {
                                "Person": '<%# Eval("Person") %>',
                                            "Title": '<%# Eval("Title") %>',
                                            "lat": '<%# Eval("lat") %>',
                                            "lng": '<%# Eval("lng") %>',
                                        }
                </ItemTemplate>
                <SeparatorTemplate>
                    ,
                </SeparatorTemplate>
            </asp:Repeater>
                            ];
            </script>



            <script type="text/javascript">
                var directionsService = new google.maps.DirectionsService();
                //***********Map Draw****************//
                function initialize() {
                    var directionsDisplay = new google.maps.DirectionsRenderer();
                    var lat_lng = [];
                    var latlngbounds = "";
                    
                    var mapOptions = {center: new google.maps.LatLng(markers[0].lat, markers[0].lng),zoom: 10,mapTypeId: google.maps.MapTypeId.ROADMAP};
                    lat_lng = new Array();
                    latlngbounds = new google.maps.LatLngBounds();
                    var infoWindow = new google.maps.InfoWindow();
                    var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
                    var icon1;
                    var value = markers.length;


                    //***********ROUTING****************//
                    //Intialize the Path Array
                    var path = new google.maps.MVCArray();
                    //Intialize the Direction Service
                    var service = new google.maps.DirectionsService();
                    var lineSymbol = { path: google.maps.SymbolPath.CIRCLE };
                    //Set the Path Stroke Color

                    //================For First Person============================
                    for (i = 0; i < markers.length; i++) {
                        //var letter = String.fromCharCode("A".charCodeAt(0) + i - 1);
                        var data = markers[i]
                        var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                        lat_lng.push(myLatlng);
                        if (i == 0) {icon1 = "http://license.dataman.in/images/start.png";}
                        else if (value - 1 == i) {icon1 = "http://license.dataman.in/images/current.png";}
                        else {icon1 = "http://license.dataman.in/images/passed.png";}
                        var marker = new google.maps.Marker({position: myLatlng,map: map,icon: icon1});
                        (function (marker, data) {
                            // Attaching a click event to the current marker
                            google.maps.event.addListener(marker, "click", function (e) {
                                if (i == 0){
                                    infoWindow.setContent(data.Person); infoWindow.open(map, marker);}
                                if(i=(markers2.length -1)){
                                    infoWindow.setContent(data.Person); infoWindow.open(map, marker);}
                                else{
                                    infoWindow.setContent(data.Title);
                                    infoWindow.open(map, marker);}
                            });
                        })(marker, data);

                    }

                    var poly = new google.maps.Polyline({
                        // path: lineCoordinates,
                        icons: [{
                            icon: {
                                path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW,
                                strokeColor: '#E86209',
                                fillColor: '#E86209',
                                fillOpacity: 1
                            },
                            repeat: '100px',
                            path: []
                        }],
                        map: map
                    });



                    //Loop1 and Draw Path Route between the Points on MAP
                    for (var i = 0; i <= lat_lng.length; i++) {
                        if ((i + 1) <= lat_lng.length) {
                            var src = lat_lng[i];
                            var des = lat_lng[i + 1];
                            path.push(src);
                            poly.setPath(path);
                            service.route({
                                origin: src,
                                destination: des,
                                travelMode: google.maps.DirectionsTravelMode.DRIVING
                            }, function (result, status) {
                                if (status == google.maps.DirectionsStatus.OK) {
                                    for (var i = 0, len = result.routes[0].overview_path.length; i < len; i++) {
                                        directionsDisplay.setDirections(response);
                                        path.push(result.routes[0].overview_path[i]);
                                    }
                                }
                            });

                        }
                    }

                    //================For Second Person============================
                    var lat_lng1 = [];
                    var latlngbounds1 = "";                    
                    lat_lng1 = new Array();
                    var value1 = markers1.length;

                    //Intialize the Path Array
                    var path1 = new google.maps.MVCArray();
                    //Intialize the Direction Service
                    var service1 = new google.maps.DirectionsService();
                    var lineSymbol1 = { path1: google.maps.SymbolPath.CIRCLE };
                    //Set the Path Stroke Color

                    for (i = 0; i < markers1.length; i++) {
                        var data1 = markers1[i]
                        var myLatlng1 = new google.maps.LatLng(data1.lat, data1.lng);
                        lat_lng1.push(myLatlng1);
                        if (i == 0) {icon1 = "http://license.dataman.in/images/start1.png";}
                        else if (value1 - 1 == i) {icon1 = "http://license.dataman.in/images/current1.png";}
                        else {icon1 = "http://license.dataman.in/images/passed1.png";}
                        var marker1 = new google.maps.Marker({position: myLatlng1,map: map,icon: icon1});
                        (function (marker1, data1) {
                            // Attaching a click event to the current marker
                            google.maps.event.addListener(marker1, "click", function (e) {
                                if (i == 0){
                                    infoWindow.setContent(data1.Person);infoWindow.open(map, marker1);}
                                if(i=(markers2.length -1)){
                                    infoWindow.setContent(data1.Person);infoWindow.open(map, marker1);}
                                else{
                                    infoWindow.setContent(data1.Title);
                                    infoWindow.open(map, marker1);}
                            });
                        })(marker1, data1);
                    }

        
                           
                    poly = new google.maps.Polyline({
                        // path: lineCoordinates,
                        strokeColor: '#BC456F',
                        icons: [{
                            icon: {
                                path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW,strokeColor: '#009900',fillColor: '#009900',fillOpacity: 1},
                            repeat: '100px',
                            path1: []
                        }],
                        map: map
                    });
                    
                    
                    //Loop2 and Draw Path Route between the Points on MAP
                    
                    for (var i = 0; i <= lat_lng1.length; i++) {
                        if ((i + 1) <= lat_lng1.length) {
                            var src = lat_lng1[i];
                            var des = lat_lng1[i + 1];
                            path1.push(src);
                            poly.setPath(path1);                           
                            service.route({
                                origin: src,
                                destination: des,
                                travelMode: google.maps.DirectionsTravelMode.DRIVING
                            }, function (result, status) {
                                if (status == google.maps.DirectionsStatus.OK) {
                                    for (var i = 0, len = result.routes[1].overview_path.length; i < len; i++) {
                                        directionsDisplay.setDirections(response);
                                        path1.push(result.routes[1].overview_path[i]);
                                    }
                                }
                            });

                        }
                    }




                    //================For Third Person============================
                    var lat_lng2 = [];
                    var latlngbounds2 = "";                    
                    lat_lng2 = new Array();
                    var value2 = markers2.length;

                    //Intialize the Path Array
                    var path2 = new google.maps.MVCArray();
                    //Intialize the Direction Service
                    var service2 = new google.maps.DirectionsService();
                    var lineSymbol2 = { path2: google.maps.SymbolPath.CIRCLE };
                    //Set the Path Stroke Color

                    for (i = 0; i < markers2.length; i++) {
                        var data2 = markers2[i]
                        var myLatlng2 = new google.maps.LatLng(data2.lat, data2.lng);
                        lat_lng2.push(myLatlng2);
                        if (i == 0) {icon1 = "http://license.dataman.in/images/start2.png";}
                        else if (value2 - 1 == i) {icon1 = "http://license.dataman.in/images/current2.png";}
                        else {icon1 = "http://license.dataman.in/images/passed2.png";}
                        var marker2 = new google.maps.Marker({position: myLatlng2,map: map,icon: icon1});
                        (function (marker2, data2) {
                            // Attaching a click event to the current marker
                            google.maps.event.addListener(marker2, "click", function (e) {
                                if (i == 0){
                                    infoWindow.setContent(data2.Person);infoWindow.open(map, marker2);}
                                if(i=(markers2.length -1)){
                                    infoWindow.setContent(data2.Person);infoWindow.open(map, marker2);}
                                else
                                { infoWindow.setContent(data2.Title);
                                    infoWindow.open(map, marker2);}
                            });
                        })(marker2, data2);
                    }

        
                           
                    poly = new google.maps.Polyline({
                        // path: lineCoordinates,
                        strokeColor: '#3B3BCE',
                        icons: [{
                            icon: {
                                path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW,strokeColor: '#140D0D',fillColor: '#009900',fillOpacity: 1},
                            repeat: '100px',
                            path2: []
                        }],
                        map: map
                    });
                    
                    
                    //Loop2 and Draw Path Route between the Points on MAP
                    
                    for (var i = 0; i <= lat_lng2.length; i++) {
                        if ((i + 1) <= lat_lng2.length) {
                            var src = lat_lng2[i];
                            var des = lat_lng2[i + 1];
                            path2.push(src);
                            poly.setPath(path2);                           
                            service.route({
                                origin: src,
                                destination: des,
                                travelMode: google.maps.DirectionsTravelMode.DRIVING
                            }, function (result, status) {
                                if (status == google.maps.DirectionsStatus.OK) {
                                    for (var i = 0, len = result.routes[2].overview_path.length; i < len; i++) {
                                        directionsDisplay.setDirections(response);
                                        path2.push(result.routes[2].overview_path[i]);
                                    }
                                }
                            });

                        }
                    }

                                             

                }
                google.maps.event.addDomListener(window, 'load', initialize);

            </script>
            <div class="container-fluid">
                <div class="box-header">
                            <h3 class="box-title">Activity Compare Map</h3>
                        
                            </div>       
                <div class="clearfix"></div>
             <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="row">
                <div class="form-group col-md-4 col-sm-4 col-xs-12" style="display:none;" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Group Name" >Group :</label>
                    <asp:DropDownList ID="ddlfgrp" runat="server" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlfgrp_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                </div>
                 <div class="row" style="margin:-17px ; padding:12px">
                <div class="form-group col-md-4 col-sm-4 col-xs-12" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="First Person Name" >First Person Name :</label>
                    <asp:DropDownList ID="ddlfirstperson" CssClass="textbox form-control"
                           Width="90%" runat="server">
                       </asp:DropDownList>
                </div>
                
               
                <div class="form-group col-md-4 col-sm-4 col-xs-12" style="display:none;" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Group" >Group :</label>
                    <asp:DropDownList ID="ddlSgrp" runat="server" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlSgrp_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                </div>
                
                <div class="form-group col-md-4 col-sm-4 col-xs-12" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Second Person Name" >Second Person Name :</label>
                    <asp:DropDownList ID="ddlsecondperson" CssClass="textbox form-control" Width="90%" runat="server">
                        </asp:DropDownList>
                </div>
                    
                
                <div class="form-group col-md-4 col-sm-4 col-xs-12" style="display:none;" >                   
                   <label for="Group" >Group :</label>
                    <asp:DropDownList ID="ddltgrp" runat="server" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddltgrp_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-md-4 col-sm-4 col-xs-12" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Third Person Name" >Third Person Name :</label>
                    <asp:DropDownList ID="ddlthirdperson" CssClass="textbox form-control" Width="90%" runat="server">
                        </asp:DropDownList>
                </div>
                     </div>
                <div class="clearfix"></div>
                <div class="row" style="padding:8px;">
                 <div class="form-group col-md-4 col-sm-4 col-xs-12" >
                   
                   <label for="From Date" >From Date :</label>
                     <div class="clearfix"></div>
                     <div class="col-md-7 col-sm-5 col-xs-7 no-padding">
                    <asp:TextBox ID="txt_fromdate" Enabled="false" runat="server" width="80%" CssClass="textbox form-control fdate" MaxLength="10" OnTextChanged="txt_fromdate_TextChanged" AutoPostBack="true" ></asp:TextBox>
                    </div>
                         <div class="col-md-5 col-xs-5 col-sm-4 no-padding">
                        <a href="javascript:;" class="cal-icon" ID="img1" runat="server"><i class="fa fa-calendar" style="margin:-40px;" aria-hidden="true"></i></a>
                      
                         <%-- <asp:ImageButton ID="img1" runat="server" ImageUrl="~/img/Calendar.png" />--%>
                      <div class="col-md-5 col-xs-3 col-sm-3 paddingleft0" style="margin-right:-80px">
             <asp:TextBox ID="Txt_FromTime" runat="server" width="240%" CssClass="textbox form-control time"  MaxLength="5"></asp:TextBox>
                          </div>
                             </div>
                        <ajax:CalendarExtender ID="cc1" runat="server" TargetControlID="txt_fromdate" PopupButtonID="img1" PopupPosition="TopLeft" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                </div>
                <div class="form-group col-md-4 col-sm-4 col-xs-12">
                   
                   <label for="To Date" >To Date :</label>
                    <div class="clearfix"></div>
                     <div class="col-md-7 col-sm-5 col-xs-7 no-padding">
                 <asp:TextBox ID="TextBox1" Enabled="false" runat="server" width="80%"  CssClass="textbox form-control fdate" MaxLength="10" OnTextChanged="TextBox1_TextChanged" AutoPostBack="true" ></asp:TextBox>
                    </div>
                       <div class="col-md-5 col-xs-5 col-sm-4 no-padding">
                        <a href="javascript:;" class="cal-icon" ID="ImageButton1" runat="server"><i class="fa fa-calendar" style="margin:-40px;" aria-hidden="true"></i></a>
                      <div class="col-md-5 col-xs-3 col-sm-3 paddingleft0 " style="margin-right:-80px">
                           <asp:TextBox ID="TextBox2" runat="server"  width="240%" CssClass="textbox form-control time "  MaxLength="5"></asp:TextBox>
                       </div>
                       </div>

                     <%--<asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/Calendar.png" />--%>
                          

                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TextBox1" PopupButtonID="ImageButton1" PopupPosition="TopLeft" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                </div>
                 
                <div class="form-group col-md-4 col-sm-4 col-xs-12" >
                   
                   <label for="Accuracy" >Accuracy :</label><br/>
                   <div class="col-md-2 no-padding"> <asp:TextBox ID="txtaccu" runat="server" Text="200" Width="60px" CssClass="form-control"></asp:TextBox></div>
                    <div class="col-md-2 paddingleft0">(Metres)</div>
                          <ajax:FilteredTextBoxExtender ID="txtaccu_FilteredTextBoxExtender"
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtaccu">
                        </ajax:FilteredTextBoxExtender>
                </div>
                   
                <div class="clearfix"></div>

                <div class="form-group col-md-4 col-sm-4 col-xs-12" style="margin-top:10px" >
                    <asp:Button ID="Btn1" runat="server" Text="Generate" CssClass="btn btn-primary" Width="100px" OnClick="Btn1_Click" OnClientClick="return validate();" />
                </div>
                    </div>
                  <div class="clearfix"></div>
                </div></div>
                <div class="clearfix"></div>

                      
                                    <div id="map_canvas" style="width: 100%; height: 600px">
                                    </div>
                </div>
                              
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Btn1" />
        </Triggers>
    </asp:UpdatePanel>
                    </div>
                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>

        </div>
    </section>
   
    
  
</asp:Content>