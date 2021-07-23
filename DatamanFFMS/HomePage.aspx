<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.master" AutoEventWireup="true" Inherits="HomePage" Codebehind="HomePage.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
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
        

        <div class="box-body" id="mainDiv"  runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12 col-sm-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                  
                   <asp:UpdatePanel ID="up2" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="True">
        <ContentTemplate>
          <%--  <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>--%>
             <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=AIzaSyBvCa97dqF2Pao5NPBguRd-bNQgB4VAqLw&callback=initialize"></script>
            <script type="text/javascript">
                var markers = [
                <asp:Repeater ID="rptMarkers" runat="server">
                <ItemTemplate>
                
                            {
                                "Title": '<%# Eval("Title") %>',
                                "lat": '<%# Eval("lat") %>',
                                "lng": '<%# Eval("lng") %>',
                                "DeviceNo":'<%#Eval("DeviceNo") %>'
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
                function initialize() {

                
                    var directionsDisplay = new google.maps.DirectionsRenderer();
                    var lat_lng = [];
                    var latlngbounds = "";
                    var mapOptions = {

                        center: new google.maps.LatLng(21.0000, 78.0000),
                        zoom: 5,
                        mapTypeId: google.maps.MapTypeId.poly
                        //  marker:true
                    };

                    lat_lng = new Array();
                    latlngbounds = new google.maps.LatLngBounds();
                    var infoWindow = new google.maps.InfoWindow();
                    var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
                    for (i = 0; i < markers.length; i++) {
                        var letter = String.fromCharCode("A".charCodeAt(0) + i - 1);
                        var data = markers[i]
                        var myLatlng = new google.maps.LatLng(data.lat, data.lng);


                        { lat_lng.push(myLatlng); }
                        // alert(myLatlng);
                        var marker = new google.maps.Marker({
                            position: myLatlng,
                            map: map,
                            icon: {
                                url: "http://license.dataman.in/images/red-dot.png"
                            }
                        });

                        (function (marker, data) {
                            // Attaching a click event to the current marker
                            google.maps.event.addListener(marker, "mouseover", function (e) {
                                infoWindow.setContent(data.Title);
                                infoWindow.open(map, marker);

                            });
                            google.maps.event.addListener(marker, "click", function (e) {
                                $('#<%=lblPerson.ClientID %>').text(data.Title+" :Last Updated");
                                GetRouteByPerson(data.DeviceNo); ShowModalPopup();
                            });

                        })(marker, data);

                        }
                 
                    //***********ROUTING****************//
                        function routing() {
                     
                            //Intialize the Path Array
                            var path = new google.maps.MVCArray();

                            //Intialize the Direction Service
                            var service = new google.maps.DirectionsService();
                            var lineSymbol = {
                                path: google.maps.SymbolPath.CIRCLE
                            };

                            //Set the Path Stroke Color
                       
                        
                            var poly = new google.maps.Polyline({
                                // path: lineCoordinates,
                                icons: [{
                                    icon: { path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW, strokeColor: '#E86209', fillColor: '#E86209', fillOpacity: 1 },
                                    repeat: '100px',
                                    path: []
                                }],
                                map: map
                            });

                            //Loop and Draw Path Route between the Points on MAP
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
                        }
                    }
                    google.maps.event.addDomListener(window, 'load', initialize);
               
            </script>
<%--Java Scripts For Route--%>
            <script type="text/javascript">
                var directionsService = new google.maps.DirectionsService();
                function initializeRoute(jsondata) {
                    var directionsDisplay = new google.maps.DirectionsRenderer();
                    var lat_lng = [];
                    var latlngbounds = "";
                    var mapOptions = {
                        center: new google.maps.LatLng(21.0000, 78.0000),
                        zoom: 5,
                        mapTypeId: google.maps.MapTypeId.poly
                        //  marker:true
                    };
                    
                    lat_lng1 = new Array();
                    latlngbounds = new google.maps.LatLngBounds();
                    var infoWindow = new google.maps.InfoWindow();
                    var map = new google.maps.Map(document.getElementById("Div_Route"), mapOptions);
                    var json = jsondata;                  
                    for (var i = 0; i < JSON.parse(json).length; i += 1) {
                        var obj = JSON.parse(json)[i];
                        var myLatlng1 = new google.maps.LatLng(obj.Lat, obj.Lng);
                        lat_lng1.push(myLatlng1);
                        if (i == 0) {  icon1 = "http://license.dataman.in/images/start.png"; }
                        else if (JSON.parse(json).length -1 == i) {   icon1 = "http://license.dataman.in/images/current.png"; }
                        else {  icon1 = "http://license.dataman.in/images/passed.png"; }

                        var marker2 = new google.maps.Marker({
                            position: myLatlng1,
                            map: map,
                            icon: icon1
                        });
                        (function (marker2, obj) {
                            // Attaching a click event to the current marker
                            google.maps.event.addListener(marker2, "mouseover", function (e) {
                                infoWindow.setContent(obj.Time);
                                infoWindow.open(map, marker2);

                            });

                        })(marker2, obj);
                          
                    }
                    //***********ROUTING****************//
                    //Intialize the Path Array
                    var path = new google.maps.MVCArray();
                    //Intialize the Direction Service
                    var service = new google.maps.DirectionsService();
                    var lineSymbol = { path: google.maps.SymbolPath.CIRCLE };
                        
                    //Set the Path Stroke Color
                    var poly = new google.maps.Polyline({
                        // path: lineCoordinates,
                        icons: [{
                            icon: { path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW, strokeColor: '#E86209', fillColor: '#E86209', fillOpacity: 1 },
                            repeat: '100px',
                            path: []
                        }],
                        map: map
                    });

                    //Loop and Draw Path Route between the Points on MAP
                    for (var i = 0; i <= lat_lng1.length; i++) { 
                        if ((i + 1) <= lat_lng1.length) {
                            var src = lat_lng1[i];
                            var des = lat_lng1[i + 1];
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
                }
                google.maps.event.addDomListener(window, 'load', initialize);

            </script>

            <script type="text/javascript">
                function ShowModalPopup() {
                    $find('<%= mpePop.ClientID %>').show();
                    return false;
                }
                function HideModalPopup() {
                    $find('<%= mpePop.ClientID %>').hide();
                    return false;
                }
</script>
            <script type="text/javascript">


                function GetRouteByPerson(DeviceNo) {
                    var pageUrl = '<%=ResolveUrl("~/WebService2.asmx")%>';
     
                    $.ajax({
                        type: "POST",
                        url: pageUrl + '/GetRouteByPerson',
                        contentType: "application/json; charset=utf-8",
                        data: "{'DeviceNo':'" + DeviceNo + "'}",
                        dataType: "json",
                        success: function (data) {
                            initializeRoute(data.d);

                        },
                        error: function (requestObject, error, errorThrown) {
                            alert(error);
                        }

                    });
       
                }


 
    </script>
           
            <table >
                <tr>
                    <td>
                       
                        <table>
                   
                            <tr>
                           <td>
                           <div id="map_canvas" style="width: 1000px; height: 650px">
                          
                                    </div>
                                  
                                </td>
                            </tr>

                        </table>
                        </td>
                        <td>
            <asp:Button ID="Modalshow" runat="server" Style="display: none;" />
                <ajax:modalpopupextender runat="server" ID="mpePop" TargetControlID="ModalShow"
                    PopupControlID="pnlItem" BackgroundCssClass="Background" DropShadow="true" X="90" Y="90">
                </ajax:modalpopupextender>
         </td>
                        </tr>

         
            </table>
            
            <asp:Panel ID="pnlItem" runat="server" Style="display: none; background-color:White;" Width="945px" Height="600" ScrollBars="Vertical">
                      <div class="popupDiv" style="overflow:hidden;">
               <asp:Label ID="lblPerson" ForeColor="White" Font-Bold="true"  runat="server"></asp:Label>
              <img id="imgclose" style="float:right;" src="img/delete.png" onclick="HideModalPopup();" />
               
                       </div>  
                        <div class="popupdiv">
                            <table>
                            <tr>

                           <td>
                          <div id="Div_Route" style="width: 1000px; height: 650px"></div>
                                  
                                </td>
                            </tr>

                        </table>
                        </div>
                        </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
                       
                    </div>
                </div>
            </div>
        </div>
    </section>

</asp:Content>

