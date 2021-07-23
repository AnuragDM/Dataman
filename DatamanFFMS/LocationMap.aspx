<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/FFMS.Master" CodeBehind="LocationMap.aspx.cs" Inherits="AstralFFMS.LocationMap" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <%--  <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?sensor=false"></script>--%>
     <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBvCa97dqF2Pao5NPBguRd-bNQgB4VAqLw&region=IN"></script>
<script type="text/javascript">
    var markers = [
    <asp:Repeater ID="rptMarkers" runat="server">
    <ItemTemplate>
                {              

                  "title": '<%# Eval("Title") %>',
                    "lat": '<%# Eval("lat") %>',
                    "lng": '<%# Eval("lng") %>'
                 
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
                   center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
                   zoom: 13,
                   mapTypeId: google.maps.MapTypeId.poly
                   //  marker:true
                     
               };
               lat_lng = new Array();
               latlngbounds = new google.maps.LatLngBounds();
               var infoWindow = new google.maps.InfoWindow();
               var map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);
               var icon1;
               var value = markers.length;
               for (i = 0; i < markers.length; i++) {
                   var letter = String.fromCharCode("A".charCodeAt(0) + i - 1);
                   var data = markers[i]
                   var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                   lat_lng.push(myLatlng);
                   if (i == 0) {
                       icon1 ="http://license.dataman.in/images/start.png";
                   }
                   else if (value - 1 == i) {
                       icon1 =  "http://license.dataman.in/images/current.png";
                   }
                   else {
                       icon1 = "http://license.dataman.in/images/passed.png";
                   }
                   var marker = new google.maps.Marker({
                       position: myLatlng,
                       map: map,
                       icon: icon1
                   });
                  
                   (function (marker, data) {
                       // Attaching a click event to the current marker
                       google.maps.event.addListener(marker, "click", function (e) {
                           infoWindow.setContent(data.Title);
                           infoWindow.open(map, marker);
                       });
                   })(marker, data);

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
           google.maps.event.addDomListener(window, 'load', initialize);

            </script>

    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>

      <script type="text/javascript">
          var V1 = "";
          function errormessage(V1) {
              $("#messageNotification").jqxNotification({
                  width: 250, position: "top-right", opacity: 2,
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
        $(function () {
            $('[id*=ListBox1]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '200px',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();
        });
    </script>
     <style type="text/css">
        .multiselect-container > li > a {
            white-space: normal;
        }

        .input-group .form-control {
            height: 34px;
        }
    </style>
  <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
  <div class="box-body" id="mainDiv" runat="server">
 <div class="row">
      <div class="col-md-12">
          <div id="InputWork">
               <div class="box box-primary">
                    <div class="box-header with-border">
                                <h3 class="box-title">Location Tracking Report</h3>
                            </div>
       <div class="box-body" id="div1">
                                <div class="row">
                                    <div class="col-md-3 col-sm-5 col-xs-9" >
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>
                                            <label for="exampleInputEmail1" id="spname" runat="server"></label>                                            
                                            <asp:ListBox ID="ListBox1" Visible="false" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 col-sm-4 col-xs-9">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-2 col-sm-4 col-xs-9">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                        </div>
                                    </div>
                                     <div class="form-group col-md-1 col-sm-2 col-xs-6" >                   
                   <label for="exampleInputEmail1" >Start Time (24:00 Hour Format):</label>
                    <asp:TextBox ID="Txt_FromTime" runat="server" CssClass="textbox"  MaxLength="5" Width="100%"></asp:TextBox>
                </div>
                <div class="form-group col-md-1 col-sm-2 col-xs-6" >                   
                   <label for="exampleInputEmail1" >End Time (24:00 Hour Format):</label>
                    <asp:TextBox ID="TextBox2" runat="server" CssClass="textbox"  MaxLength="5" Width="100%"></asp:TextBox>
                </div>

                                </div>
                            </div>
                      <div class="box-footer">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary"
                                    OnClick="btnGo_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                    OnClick="btnCancel_Click" />
                            </div>
                   
           
                                    <div id="dvMap" style="width: 100%; height: 400px">
                                    </div>

                    <div id="detailDistOutDiv" runat="server" style="display: none;">
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="distlocrpt" runat="server">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>                                                     
                                                       <%-- <th>Current Date</th>                                                     
                                                        <th>Device No.</th>
                                                        <th>Latitude</th>
                                                        <th>Longitude</th>
                                                        <th>Description</th>--%>
                                                        <th>Current Date</th>
                                                        <th>Person</th>
                                                        <th>Latitude</th>
                                                        <th>Longitude</th>
                                                        <th>Address</th>
                                                        <th>SIG</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>                                              
                                                <%-- <td><%#Eval("currentDate")%></td>                                             
                                                <td><%#Eval("deviceNo") %></td>
                                                <td><%#Eval("latitude") %></td>
                                                <td><%#Eval("Longitude") %></td>
                                                <td><%#Eval("Description") %></td>--%>                                                
                                                <td><%#Convert.ToDateTime(Eval("dateC")).ToString("dd/MMM/yyyy HH:mm:ss") %></td>
                                                <td><%#Eval("Person") %></td>
                                                <td><%#Eval("latitude") %></td>
                                                <td><%#Eval("Longitude") %></td>
                                                <td><%#Eval("address") %></td>
                                                <td><%#Eval("Signal") %></td>

                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>     </table>       
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>

                            </div>
                                
                    
           </div>
      </div>
      </div>
      </div>
      </div>
    </section>
   </asp:Content>

