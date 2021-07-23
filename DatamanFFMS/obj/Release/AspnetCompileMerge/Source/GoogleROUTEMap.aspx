<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/FFMS.master" CodeBehind="GoogleROUTEMap.aspx.cs" Inherits="AstralFFMS.GoogleROUTEMap" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

    <script type="text/javascript" src="Scripts/simple-slider.js"></script>
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
  
      <script type="text/javascript">
          function validate() {
             <%-- if (document.getElementById("<%=ddlType.ClientID%>").value == "--Select--" || document.getElementById("<%=ddlType.ClientID%>").value == "0") {
                errormessage("Please Select Group Name");
                document.getElementById("<%=ddlType.ClientID%>").focus();
                return false;
            }--%>
            if (document.getElementById("<%=DropDownList2.ClientID%>").value == "--Select--" || document.getElementById("<%=DropDownList2.ClientID%>").value == "0") {
                errormessage("Please Select Person Name");
                document.getElementById("<%=DropDownList2.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txt_fromdate.ClientID%>").value == "" || document.getElementById("<%=txt_fromdate.ClientID%>").value == "0") {
                errormessage("Your Date Format is incorrect. Please try again.");
                document.getElementById("<%=txt_fromdate.ClientID%>").focus();
                return false;
            }
            var digits = /^((31(?!(Apr|Jun|Sep|Nov)))|((30|29)(?!Feb))|(29(?= Feb(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])[-](Jan|Feb|Mar|May|Apr|Jul|Jun|Aug|Oct|Sep|Nov|Dec)[-]((1[6-9]|[2-9]\d)\d{2})$/;
            var digitsid = document.getElementById("<%=txt_fromdate.ClientID %>").value;
            var digitsArray = digitsid.match(digits);
            var temp;
            if (digitsArray == null) {
                errormessage("Your Date Format is incorrect. Please try again.");
                document.getElementById("<%=txt_fromdate.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=TextBox1.ClientID%>").value == "" || document.getElementById("<%=TextBox1.ClientID%>").value == "0") {
                errormessage("Your Date Format is incorrect. Please try again.");
                document.getElementById("<%=TextBox1.ClientID%>").focus();
                return false;
            }
            var digits = /^((31(?!(Apr|Jun|Sep|Nov)))|((30|29)(?!Feb))|(29(?= Feb(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])[-](Jan|Feb|Mar|May|Apr|Jul|Jun|Aug|Oct|Sep|Nov|Dec)[-]((1[6-9]|[2-9]\d)\d{2})$/;
            var digitsid = document.getElementById("<%=TextBox1.ClientID %>").value;
            var digitsArray = digitsid.match(digits);
            var temp;
            if (digitsArray == null) {
                errormessage("Your Date Format is incorrect. Please try again.");
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

                     <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false&libraries=geometry"></script>
            <script type="text/javascript">
                var a;
                function updateSlider() {
                    stop();
                    a = document.getElementById('startpoint').value;
                    errormessage(a);
                }
            </script>
            <script type="text/javascript">
                var a;

                var map;
                var pointDistances;
                function initialize() {
                    var mapOptions = {
                        center: new google.maps.LatLng(10.881766, 101.626877),
                        zoom: 12,
                        mapTypeId: google.maps.MapTypeId.ROADMAP,
                        panControl: false,
                        scaleControl: false,
                        mapTypeControl: false,
                        streetViewControl: false,
                        overviewMapControl: false,
                        zoomControl: true,
                        zoomControlOptions: {
                            style: google.maps.ZoomControlStyle.SMALL,
                            position: google.maps.ControlPosition.RIGHT_TOP
                        }
                    };

                    map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
                    var markers = [];
                    var lat_lng = [];
                    var latlngbounds = "";
                    var lineCoordinates = new Array();
                    markers = JSON.parse('<%=ConvertDataTabletoString() %>');
                    var icon1;
                    var value = markers.length;
                    for (i = 0; i < markers.length; i++) {
                        var data = markers[i]
                        var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                        lineCoordinates[i] = myLatlng;

                        if (i == 0) { icon1 = "http://license.dataman.in/images/start.png"; }
                        else if (value - 1 == i) { icon1 = "http://license.dataman.in/images/current.png"; }
                        else { icon1 = "http://license.dataman.in/images/passed.png"; }
                        var marker = new google.maps.Marker({
                            position: myLatlng,
                            map: map,
                            icon: icon1
                        });
                    }
                    map.setCenter(lineCoordinates[0]);
                    // point distances from beginning in %
                    var sphericalLib = google.maps.geometry.spherical;

                    pointDistances = [];
                    var pointZero = lineCoordinates[0];
                    var wholeDist = sphericalLib.computeDistanceBetween(
                                        pointZero,
                                        lineCoordinates[lineCoordinates.length - 1]);

                    for (var i = 0; i < lineCoordinates.length; i++) {
                        pointDistances[i] = 100 * sphericalLib.computeDistanceBetween(
                                                        lineCoordinates[i], pointZero) / wholeDist;
                        console.log('pointDistances[' + i + ']: ' + pointDistances[i]);
                    }

                    // define polyline
                    var lineSymbol = {
                        path: google.maps.SymbolPath.FORWARD_OPEN_ARROW,
                        scale: 8,
                        strokeColor: '#4c4c4c'
                    };

                    line = new google.maps.Polyline({
                        path: lineCoordinates,
                        strokeColor: '#FF0000',
                        strokeOpacity: 1.0,
                        strokeWeight: 2,
                        icons: [{
                            icon: lineSymbol,
                            offset: '100%'
                        }],
                        map: map
                    });
                }


                var id;
                function animateCircle() {
                    var count = 0;
                    var offset;
                    var sentiel = -1;

                    id = window.setInterval(function () {
                        count = (count + 1) % 200;
                        offset = count / 2;

                        for (var i = pointDistances.length - 1; i > sentiel; i--) {
                            if (offset > pointDistances[i]) {
                                console.log('create marker');
                                var marker = new google.maps.Marker({
                                    icon: {
                                        url: "https://maps.gstatic.com/intl/en_us/mapfiles/markers2/measle_blue.png",
                                        size: new google.maps.Size(7, 7),
                                        anchor: new google.maps.Point(4, 4)
                                    },
                                    position: line.getPath().getAt(i),
                                    title: line.getPath().getAt(i).toUrlValue(6),
                                    map: map
                                });
                                sentiel++;
                                break;
                            }
                        }
                        // we have only one icon
                        var icons = line.get('icons');
                        icons[0].offset = (offset) + '%';
                        line.set('icons', icons);

                        if (line.get('icons')[0].offset == "99.5%") {
                            icons[0].offset = '100%';
                            line.set('icons', icons);
                            window.clearInterval(id);
                        }

                    }, 350);
                }


                google.maps.event.addDomListener(window, 'load', initialize);

            </script>
            <script type="text/javascript">

                function stop() {
                    window.clearInterval(id);
                    document.getElementById("BtnPlay").disabled = false;
                    document.getElementById("BtnFor").disabled = false;
                }


            </script>


            <script type="text/javascript">
                function play() {
                    window.clearInterval(id);
                    animateCircle();
                    document.getElementById("BtnPlay").disabled = 'true';
                    document.getElementById("BtnFor").disabled = 'false';
                }
            </script>

            <script type="text/javascript">
                function Reverse() {

                    document.getElementById("BtnPlay").disabled = 'false';
                    document.getElementById("BtnFor").disabled = 'true';
                    var count = 0;
                    var offset;
                    var sentiel = -1;

                    id = window.setInterval(function () {
                        count = (count + 1) % 200;
                        offset = count / 2;
                        errormessage("Hi");
                        for (var i = pointDistances.length - 1; i > sentiel; i--) {
                            if (offset > pointDistances[i]) {
                                console.log('create marker');
                                var marker = new google.maps.Marker({
                                    icon: {
                                        url: "https://maps.gstatic.com/intl/en_us/mapfiles/markers2/measle_blue.png",
                                        size: new google.maps.Size(7, 7),
                                        anchor: new google.maps.Point(4, 4)
                                    },
                                    position: line.getPath().getAt(i),
                                    title: line.getPath().getAt(i).toUrlValue(6),
                                    map: map,
                                });
                                sentiel++;
                                break;
                            }
                        }
                        // we have only one icon
                        var icons = line.get('icons');
                        icons[0].offset = (offset) + '%';
                        line.set('icons', icons);

                        if (line.get('icons')[0].offset == "99.5%") {
                            icons[0].offset = '100%';
                            line.set('icons', icons);
                            window.clearInterval(id);
                        }
                    }, 50);
                }


            </script>
<div class="container-fluid">   
           <div class="col-md-12"> <asp:Label ID="lblHeading" runat="server" CssClass="heading" Text=""><h3 class="box-title">Location Play</h3></asp:Label></div>
    <div class="clearfix"></div>
            <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="row">
                 <div class="form-group col-md-4 col-sm-4 col-xs-12" style="display:none;" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Group Name" >Group Name :</label>
                    <asp:DropDownList ID="ddlType" CssClass="textbox form-control" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true" Width="100%" runat="server">
                        </asp:DropDownList>
                </div>
                
               <div class="form-group col-md-4 col-sm-4 col-xs-12">
                   <label for="requiredfield" class="back">*</label>
                   <label for="Person Name">Person Name :</label>
                    <asp:DropDownList ID="DropDownList2" CssClass="textbox form-control" Width="79%" style="margin-left:5px;" runat="server" AutoPostBack="true"  onselectedindexchanged="DropDownList2_SelectedIndexChanged">
                        </asp:DropDownList>
                </div>
                

               <div class="form-group col-md-4 col-sm-4 col-xs-12">
                    <label for="requiredfield" class="back">*</label>
                    <label for="Location">Location :</label>
                    <asp:DropDownList ID="ddlloc" runat="server" CssClass="textbox form-control" AutoPostBack="true" 
                          onselectedindexchanged="ddlloc_SelectedIndexChanged" Width="79%" style="margin-left:5px;">
                 <asp:ListItem Text="Both" Value="0"></asp:ListItem>
                 <asp:ListItem  Selected="True" Text="GPS" Value="G"></asp:ListItem>
                 <asp:ListItem Text="Tower" Value="C"></asp:ListItem>
                 </asp:DropDownList>
                </div>
                
                
               <div class="form-group col-md-4 col-sm-4 col-xs-12">
                    <label for="requiredfield" class="back">*</label>
                <label for="Accuracy">Accuracy :</label></br>
                    <asp:TextBox ID="txtaccu" runat="server" CssClass="textbox form-control" Text="30" Width="80%" style="margin-left:4px;"></asp:TextBox>&nbsp;&nbsp;(Metres)
                          <ajax:FilteredTextBoxExtender ID="txtaccu_FilteredTextBoxExtender"
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtaccu">
                        </ajax:FilteredTextBoxExtender>

                </div>
                    </div>
                <div class="clearfix"></div>
               <div class="row" style="margin:-11px;">
               <div class="form-group col-md-4 col-sm-4 col-xs-12" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="From Date" >From Date :</label>

                      <div class="clearfix"></div>

                    <div class="col-md-7 col-sm-5 col-xs-7 no-padding">

                    <asp:TextBox ID="txt_fromdate" runat="server" CssClass="aspNetDisabled textbox form-control" width="80%" MaxLength="10" OnTextChanged="txt_fromdate_TextChanged" AutoPostBack="true"></asp:TextBox>
                        <%--<asp:ImageButton ID="img1" runat="server" ImageUrl="~/img/Calendar.png" />--%> </div>
                      
                         <div class="col-md-5 col-xs-5 col-sm-4 no-padding">
                        <a href="javascript:;" id="img1" class="cal-icon"><i class="fa fa-calendar" style="margin:-78px;" aria-hidden="true"></i></a>
                         <div class="col-md-3 col-sm-3 col-xs-3 paddingleft0">
                         <asp:TextBox ID="Txt_FromTime" runat="server" width="330%" CssClass="textbox form-control"  MaxLength="5"></asp:TextBox></div>
                         </div>
                         
                      <ajax:CalendarExtender ID="cc1" runat="server" TargetControlID="txt_fromdate" PopupButtonID="img1" PopupPosition="TopLeft" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                </div>
                  <div class="form-group col-md-4 col-sm-4 col-xs-12">
                   <label for="requiredfield" class="back">*</label>
                   <label for="To">To Date :</label>
                     <div class="clearfix"></div>
                      <div class="col-md-7 col-sm-5 col-xs-7 no-padding">
                    <asp:TextBox ID="TextBox1" runat="server" CssClass="aspNetDisabled textbox form-control" width="80%" OnTextChanged="txt_fromdate_TextChanged" AutoPostBack="true"></asp:TextBox>
                          <%--<asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/Calendar.png" />--%></div>
                  <div class="col-md-5 col-xs-5 col-sm-4 no-padding">
                        <a href="javascript:;" id="ImageButton1" class="cal-icon"><i class="fa fa-calendar" style="margin:-78px;" aria-hidden="true"></i></a>
                       <div class="col-md-3 col-sm-3 col-xs-3 paddingleft0">       
                       <asp:TextBox ID="TextBox2" runat="server" width="330%" CssClass="textbox form-control"  MaxLength="6"></asp:TextBox></div>  
                  </div>

                   
                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TextBox1" PopupButtonID="ImageButton1" PopupPosition="TopLeft" Format="dd-MMM-yyyy"></ajax:CalendarExtender>

                </div>
               
                <div class="clearfix"></div>
                <div class="form-group col-md-3 col-sm-5 col-xs-12" style="padding:15px;">
                        <asp:Button ID="Btn1" runat="server" Text="Generate" CssClass="btn btn-primary btnmrg" Width="100px" OnClientClick="return validate()" />
                </div>
                   </div>
                
            </div></div>
    <div class="clearfix"></div>
    <div class="col-md-6 col-sm-6 col-xs-12">        
       <input style="display:none;" type="text" runat="server" data-slider="true" data-slider-range="100,500" id="startpoint" data-slider-step="100" onchange="Javascript:updateSlider()" value="350" data-slider-highlight="true" data-slider-snap="true" />
        <br />
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6"><label for="Accuracy" class="pull-left">Fast</label></div>
            <div class="col-md-6 col-sm-6 col-xs-6"><label for="Accuracy" class="pull-right">Slow</label></div>
        </div>
        
    </div>
    <div class="col-md-3 col-sm-3 col-xs-6" class="startbutton"><input name='button' style='background: url(images/play.png) 0 0 no-repeat; border: none; width: 20px; height: 35px' value='' id="BtnPlay" onclick="javascript: play()" type='button' runat="server" /></div>
    <div class="col-md-3 col-sm-3 col-xs-6" class="startbutton"><input name='button' style='background: url(images/stop.png) 0 0 no-repeat; border: none; width: 20px; height: 35px' value='' id="BtnStop" onclick='javascript: stop();' type='button' runat="server" /></td></div>
    <div class="clearfix"></div>                   
    <div class="table-responsive ">
            
                
            
                </div>
            <div class="table-responsive ">
            <table class="table">
                <tr>
                    <td>
                        <div id="map_canvas" style="width: 100%; height: 500px;"></div>
                    </td>
                </tr>
            </table>
                </div>
    </div>
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="Btn1" />
            <asp:PostBackTrigger ControlID="BtnPlay" />
            <asp:PostBackTrigger ControlID="BtnStop" />
            <asp:PostBackTrigger ControlID="ddlType" />
        </Triggers>
    </asp:UpdatePanel>
                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>

        </div>
    </section>
   
    
  
</asp:Content>