
<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="LivePosition.aspx.cs" Inherits="AstralFFMS.LivePosition" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--            <script type="text/javascript" src="http://code.jquery.com/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>--%>
        <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <style>
        .dvContainer {
  position: relative;
  float:right;
  display: inline-block;
  width: 73%;
  height: 400px;
  background-color: #ccc;
  margin-top:25px
}
.dvContainerleft{
      position: relative;
  float:left;
  display: inline-block;
  width: 25%;
  background-color: White;
  margin-top:25px;
  overflow:auto;

  border:solid .5px  #3c8dbc;
 
}
.markerheightnew{
    height:15px;
}
#style-3::-webkit-scrollbar-track
{
    -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
    background-color: #F5F5F5;
}

#style-3::-webkit-scrollbar
{
    width: 6px;
    background-color: #F5F5F5;
}

#style-3::-webkit-scrollbar-thumb
{
    background-color: #3c8dbc;
}
.dvInsideTL {
  position: absolute;
  right: 8px;
  top: 8px;
  width: 150px;
  height: 80px;
  background-color: white;
    cursor: pointer;
    font-size:12px;

}
.fontcolorred{
    color:red;
    font-weight:bold;   
}

.fontcolorgreen{
    color:green;font-weight:bold;  
}
    </style>



     <script type="text/javascript">

         $(window).load(function () {
             // executes when complete page is fully loaded, including all frames, objects and images
             getLocation();
         });
         function getLocation() {
             if (navigator.geolocation) {
                 navigator.geolocation.getCurrentPosition(showPosition);
             }
         }
         $(function () {
             $("[id*=trview] input[type=checkbox]").bind("click", function () {
                 var table = $(this).closest("table");
                 if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                     //Is Parent CheckBox
                     var childDiv = table.next();
                     var isChecked = $(this).is(":checked");
                     $("input[type=checkbox]", childDiv).each(function () {
                         if (isChecked) {
                             $(this).prop("checked", "checked");
                             //$(this).removeAttr("checked");
                         } else {
                             $(this).removeAttr("checked");
                         }
                     });
                 } else {
                     //Is Child CheckBox
                     var parentDIV = $(this).closest("DIV");
                     if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                         $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                       //  $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                     } else {
                         $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                     }
                 }
             });
         })

         $('.trview').on('change', function() {
             $('.trview').not(this).prop('checked', false);  
         });
         function showPosition(position) {
          
             var lat = document.getElementById('<%= hidlat.ClientID %>');
             var long = document.getElementById('<%= hidlong.ClientID %>');
             lat.value = position.coords.latitude;
             long.value = position.coords.longitude;
             //alert("lat.value");alert(long.value);
         }
         function validate() {

             if ($('#<%=frmTextBox.ClientID%>').val() == "") {
                 errormessage("Please select Date");
                 return false;
             }
             var selectedvalue = [];
             $("#<%=trview.ClientID %> :checked").each(function () {
                 selectedvalue.push($(this).val());
             });
             if (selectedvalue == "") {
                 errormessage("Please Select Sales Person");
                 return false;
             }
         }
         function Visble(){

             if (document.getElementById("divfilter").style.display == "none")
             {
                 document.getElementById("divfilter").style.display = "block";  
             }

             else if (document.getElementById("divfilter").style.display == "block")
             {
                 document.getElementById("divfilter").style.display = "none";
             }     

         }
         var V1 = "";
         function errormessage(V1) {
             $("#messageNotification").jqxNotification({
                 width: 250, position: "top-right", opacity: 2,
                 autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "error"
             });
             $('#<%=lblmasg.ClientID %>').html(V1);
             $("#messageNotification").jqxNotification("open");

         }
         var V1 = "";
         function errormessage(V1) {
             $("#messageNotification").jqxNotification({
                 width: 250, position: "top-right", opacity: 2,
                 autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "error"
             });
             $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }

         function client_OnTreeNodeChecked(event)
         {
             var treeNode = event.srcElement || event.target ;
             if (treeNode.tagName == "INPUT" && treeNode.type == "checkbox")
             {
                 if(treeNode.checked)
                 {
                     uncheckOthers(treeNode.id);
                 }
             }
         }

         function uncheckOthers(id)
         {
             var elements = document.getElementsByTagName('input');
             // loop through all input elements in form
             for(var i = 0; i < elements.length; i++)
             {
                 if(elements.item(i).type == "checkbox")
                 {
                     if(elements.item(i).id!=id)
                     {
                         elements.item(i).checked=false;
                     }
                 }
             }
         }  

         
    </script>
<script type="text/javascript">

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
      
   <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=AIzaSyBvCa97dqF2Pao5NPBguRd-bNQgB4VAqLw&region=IN"></script>
<script type="text/javascript">
    var markers = [
    <asp:Repeater ID="rptMarkers1" runat="server">
    <ItemTemplate>
                {
                    "title": '<%# Eval("Title") %>',
                    "lat": '<%# Eval("lat") %>',
                    "lng": '<%# Eval("lng") %>',
                    "description": '<%# Eval("address1")  %>',
                    "type": '<%# Eval("type") %>',
                    "sno": '<%# Eval("Sno") %>',
                    "time": '<%# Eval("time") %>'
                }
</ItemTemplate>
<SeparatorTemplate>
    ,
</SeparatorTemplate>
</asp:Repeater>
    ];
</script>
<script type="text/javascript">
    $(document).ready(function () {
        if (!$.browser.webkit) {
            $('.wrapper').html('<p>Sorry! Non webkit users. :(</p>');
        }
    });
    var markers1 = [];
    function initialize() {
        // Create a new StyledMapType object, passing it an array of styles,
        // and the name to be displayed on the map type control.
        var styledMapType = new google.maps.StyledMapType(
            [
              {elementType: 'geometry', stylers: [{color: '#ebe3cd'}]},
              {elementType: 'labels.text.fill', stylers: [{color: '#523735'}]},
              {elementType: 'labels.text.stroke', stylers: [{color: '#f5f1e6'}]},
              {
                  featureType: 'administrative',
                  elementType: 'geometry.stroke',
                  stylers: [{color: '#c9b2a6'}]
              },
              {
                  featureType: 'administrative.land_parcel',
                  elementType: 'geometry.stroke',
                  stylers: [{color: '#dcd2be'}]
              },
              {
                  featureType: 'administrative.land_parcel',
                  elementType: 'labels.text.fill',
                  stylers: [{color: '#ae9e90'}]
              },
              {
                  featureType: 'landscape.natural',
                  elementType: 'geometry',
                  stylers: [{color: '#dfd2ae'}]
              },
              {
                  featureType: 'poi',
                  elementType: 'geometry',
                  stylers: [{color: '#dfd2ae'}]
              },
              {
                  featureType: 'poi',
                  elementType: 'labels.text.fill',
                  stylers: [{color: '#93817c'}]
              },
              {
                  featureType: 'poi.park',
                  elementType: 'geometry.fill',
                  stylers: [{color: '#a5b076'}]
              },
              {
                  featureType: 'poi.park',
                  elementType: 'labels.text.fill',
                  stylers: [{color: '#447530'}]
              },
              {
                  featureType: 'road',
                  elementType: 'geometry',
                  stylers: [{color: '#f5f1e6'}]
              },
              {
                  featureType: 'road.arterial',
                  elementType: 'geometry',
                  stylers: [{color: '#fdfcf8'}]
              },
              {
                  featureType: 'road.highway',
                  elementType: 'geometry',
                  stylers: [{color: '#f8c967'}]
              },
              {
                  featureType: 'road.highway',
                  elementType: 'geometry.stroke',
                  stylers: [{color: '#e9bc62'}]
              },
              {
                  featureType: 'road.highway.controlled_access',
                  elementType: 'geometry',
                  stylers: [{color: '#e98d58'}]
              },
              {
                  featureType: 'road.highway.controlled_access',
                  elementType: 'geometry.stroke',
                  stylers: [{color: '#db8555'}]
              },
              {
                  featureType: 'road.local',
                  elementType: 'labels.text.fill',
                  stylers: [{color: '#806b63'}]
              },
              {
                  featureType: 'transit.line',
                  elementType: 'geometry',
                  stylers: [{color: '#dfd2ae'}]
              },
              {
                  featureType: 'transit.line',
                  elementType: 'labels.text.fill',
                  stylers: [{color: '#8f7d77'}]
              },
              {
                  featureType: 'transit.line',
                  elementType: 'labels.text.stroke',
                  stylers: [{color: '#ebe3cd'}]
              },
              {
                  featureType: 'transit.station',
                  elementType: 'geometry',
                  stylers: [{color: '#dfd2ae'}]
              },
              {
                  featureType: 'water',
                  elementType: 'geometry.fill',
                  stylers: [{color: '#b9d3c2'}]
              },
              {
                  featureType: 'water',
                  elementType: 'labels.text.fill',
                  stylers: [{color: '#92998d'}]
              }
            ],
            {name: 'Styled Map'});

        var mapOptions = {
            center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
            zoom: 12,
            mapTypeId:google.maps.MapTypeId.ROADMAP
           
        };//alert(mapOptions);

        var map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);
        var infoWindow = new google.maps.InfoWindow();
        var lat_lng = new Array();
        var latlngbounds = new google.maps.LatLngBounds();
        for (i = 0; i < markers.length; i++) {
            var data = markers[i]
            var myLatlng = new google.maps.LatLng(data.lat, data.lng);
            var icon = "";
            switch (data.type) {
                case "T":
                    icon = "2";
                    break;
                //case "VIT":
                //    icon = "1";
                //    break;
                case "O":
                    icon = "5";
                    break;
                case "F":
                    icon = "2";
                    break;
                case "D":
                    icon = "3";
                    break;
                //case "FV":
                //    icon = "3";
                //    break;
            }
            icon = "img/" + icon + ".png";
       
            lat_lng.push(myLatlng);
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: data.title,
                animation: google.maps.Animation.DROP,
                icon: new google.maps.MarkerImage(icon)
            });
            latlngbounds.extend(marker.position);
            debugger;
            (function (marker, data) {
                var contentString = '<div id="content" style="font-size:11px;"  ><span style="color:blue;font-weight:bold">'+''+data.title+''+
          '</span><div id="bodyContent">'+data.description+'<br>Visit Type : Regular</br>Time:'+data.time+'</br>Sequence : '+data.sno+'</div>'+'</div>';
               
                google.maps.event.addListener(marker, "click", function (e) {
                     infoWindow.setContent(contentString);
                    // infoWindow.setContent(locations[i][0]);
                    infoWindow.open(map, marker);
                });
            })(marker, data);
            // Push the marker to the 'markers' array
            markers1.push(marker);
        }
        map.setCenter(latlngbounds.getCenter());
        map.fitBounds(latlngbounds);
 
        //***********ROUTING****************//
 
        //Initialize the Path Array
        var path = new google.maps.MVCArray();
 
        //Initialize the Direction Service
        var service = new google.maps.DirectionsService();
 
        //Set the Path Stroke Color
        var poly = new google.maps.Polyline({ map: map, strokeColor: '#4986E7' });
 
        //Loop and Draw Path Route between the Points on MAP
        for (var i = 0; i < lat_lng.length; i++) {
            if ((i + 1) < lat_lng.length) {
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
                            path.push(result.routes[0].overview_path[i]);
                        }
                    }
                });
            }
        }
    }
    google.maps.event.addDomListener(window, 'load', initialize);
    function myClick(id){
       // alert(id);
        google.maps.event.trigger(markers1[id], 'click');
    }




    //2222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222

    //window.onload = function () {
    //    var mapOptions = {
    //        center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
    //        zoom: 10,
    //        mapTypeId: google.maps.MapTypeId.ROADMAP
    //    };
    //    var map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);
    //    var infoWindow = new google.maps.InfoWindow();
    //    var lat_lng = new Array();
    //    var latlngbounds = new google.maps.LatLngBounds();
    //    for (i = 0; i < markers.length; i++) {
    //        var data = markers[i]
    //        var myLatlng = new google.maps.LatLng(data.lat, data.lng);
    //        lat_lng.push(myLatlng);
    //        var marker = new google.maps.Marker({
    //            position: myLatlng,
    //            map: map,
    //            title: data.title
    //        });
    //        latlngbounds.extend(marker.position);
    //        (function (marker, data) {
    //            google.maps.event.addListener(marker, "click", function (e) {
    //                infoWindow.setContent(data.description);
    //                infoWindow.open(map, marker);
    //            });
    //        })(marker, data);
    //    }
    //    map.setCenter(latlngbounds.getCenter());
    //    map.fitBounds(latlngbounds);
 
    //    //***********ROUTING****************//
 
    //    //Initialize the Path Array
    //    var path = new google.maps.MVCArray();
 
    //    //Initialize the Direction Service
    //    var service = new google.maps.DirectionsService();
 
    //    //Set the Path Stroke Color
    //    var poly = new google.maps.Polyline({ map: map, strokeColor: '#4986E7' });
 
    //    //Loop and Draw Path Route between the Points on MAP
    //    for (var i = 0; i < lat_lng.length; i++) {
    //        if ((i + 1) < lat_lng.length) {
    //            var src = lat_lng[i];
    //            var des = lat_lng[i + 1];
    //            path.push(src);
    //            poly.setPath(path);
    //            service.route({
    //                origin: src,
    //                destination: des,
    //                travelMode: google.maps.DirectionsTravelMode.DRIVING
    //            }, function (result, status) {
    //                if (status == google.maps.DirectionsStatus.OK) {
    //                    for (var i = 0, len = result.routes[0].overview_path.length; i < len; i++) {
    //                        path.push(result.routes[0].overview_path[i]);
    //                    }
    //                }
    //            });
    //        }
    //    }
    //}


    //111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111
    //window.onload = function () {
 
    //    var mapOptions = {
    //        center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
    //        zoom: 8,
    //        mapTypeId: google.maps.MapTypeId.ROADMAP
    //    };
    //    var infoWindow = new google.maps.InfoWindow();
    //    var latlngbounds = new google.maps.LatLngBounds();
    //    var map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);
    //    var i = 0;
    //    var interval = setInterval(function () {
    //        var data = markers[i]
    //        var myLatlng = new google.maps.LatLng(data.lat, data.lng);
    //        var icon = "";
    //        switch (data.type) {
    //            case "NV":
    //                icon = "red";
    //                break;
    //            case "VIT":
    //                icon = "green";
    //                break;
    //            case "VOT":
    //                icon = "yellow";
    //                break;
    //            case "O":
    //                icon = "orange";
    //                break;
    //            case "FV":
    //                icon = "purple";
    //                break;
    //        }
    //        icon = "http://maps.google.com/mapfiles/ms/icons/" + icon + ".png";
    //        var marker = new google.maps.Marker({
    //            position: myLatlng,
    //            map: map,
    //            title: data.title,
    //            animation: google.maps.Animation.DROP,
    //            icon: new google.maps.MarkerImage(icon)
    //        });
            
    //        (function (marker, data) {
    //            var contentString = '<div id="content"  ><span style="color:blue">'+''+data.title+''+
    //  '</span><div id="bodyContent">'+data.description+'<br>Visit Type : Regular</br>Time:12.02 Pm</br>Sequence : 1</div>'+'</div>';
    //            google.maps.event.addListener(marker, "click", function (e) {
    //                infoWindow.setContent(contentString);
                   
    //                infoWindow.open(map, marker);
    //            });
    //        })(marker, data);
    //        latlngbounds.extend(marker.position);
    //        i++;
    //        if (i == markers.length) {
    //            clearInterval(interval);
    //            var bounds = new google.maps.LatLngBounds();
    //            map.setCenter(latlngbounds.getCenter());
    //            map.fitBounds(latlngbounds);
    //        }
    //    }, 80);
    //}
</script>
       <div style="border-bottom:1px solid #3c8dbc; padding-bottom:10px; width:100% ">
 <h3> <span class="pull-left" id="ContentPlaceHolder1_lblpagename">Live Map</span></h3>
  <h3><span class="pull-left" runat="server" id="ContentPlaceHolder1_lblspname">(SalesPerson Name)</span><span class="pull-right" runat="server" id="Span1"><a href="#" onclick='Visble()'><i class="fa fa-filter" aria-hidden="true"></i></a></span></h3>

    <div class="clearfix"></div>
 </div>
      <div class="row" style="padding:0;" >
          

            <div class="col-md-12" style="padding-left:0;">
                <div id="divfilter" style="display:none">
                <div class="col-md-8" id="divTohide"  runat="server">
                                <div class="row" >                                   
                                     <div id="divtrview" class="col-md-6" runat="server" >
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>
                                            <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>
                                            <asp:PlaceHolder ID ="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="hidlat" runat="server" />
                                     <asp:HiddenField ID="hidlong" runat="server" />
                                </div>
                                <div class="row">
                                    <div class="col-md-4 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Date:</label>
                                            <asp:TextBox ID="frmTextBox" class="form-control" runat="server" 
                                                Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="frmTextBox_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server"
                                                BehaviorID="frmTextBox_CalendarExtender"
                                                TargetControlID="frmTextBox"></ajaxToolkit:CalendarExtender>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 " style="padding-top:20px" >
                                       
                                    
                                            <asp:Button ID="btngo" runat="server" CssClass="btn btn btn-primary" Text="Go"  OnClick="btngo_Click" OnClientClick="javascript:return validate();"/>
                                    
                                    </div>
                                </div>
                            </div>
           
                    </div>
                </div>
          <div class="col-md-12" style="background-color:white; display:none">
       <div style="width:100%;font-weight:bold; border-radius: 8px;padding-top:20px">
              <div class="col-md-12">
                  <div class="col-md-4" style="padding-bottom:10px">Day Start Info</div>
                   <div class="col-md-4"  style="padding-bottom:10px" id="divBeat" runat="server"></div>
                   <div class="col-md-4" id="divTime"  style="padding-bottom:10px" runat="server" ></div>
               
                  </div></div>
          </div>
          <div class="col-md-12">
        <div id="wrapper">

        <div class="dvContainerleft scrollbar" id="style-3">   
              <div class="box-body table-responsive force-overflow">
                                    <asp:Repeater ID="leavereportrpt" runat="server"  >
                                        <HeaderTemplate>
                                            <table id="example1" class="table ">
                                                <thead>
                                                    <tr style="border-radius:4px">
                                                        <th style="background-color:#3c8dbc;color:white">Retailer</th>
                                                         <th style="background-color:#3c8dbc;color:white">Time</th>                                                 
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>                             
                                            <tr>
                                                <td>                                        
                                                 <a href="#" onclick="myClick(<%#Container.ItemIndex %>);"><%#Eval("contact") %></a>                                                 
                                                 <td><%#Eval("time") %>                                                
                                                </td>                                              
                                            </tr>                                                
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>  </table>  
                                            <center> 
                                            <div id="dvNoRecords" runat="server" visible="false" style="text-align: center; color: white;height:40%;background-color:#3c8dbc;width:40%;margin-top:10px">  
                                                 <font>  
                                                     <b>  
                                                       No records to display.  
                                                     </b>  
                                                  </font>  
                                             </div>  
                                                <div id="Div1" runat="server" visible="true" style="text-align: center; color: white;height:40%;background-color:#3c8dbc;width:40%;margin-top:10px">  
                                                 <font>  
                                                     <b>  
                                                       Wait we are fetching records...........
                                                     </b>  
                                                  </font>  
                                             </div> 
                                  </center>                              </FooterTemplate>
                                    </asp:Repeater>
               
                                </div>

        </div></div>
        <div class="dvContainer">
  <table style="width:100%;">
    <tr>

     
      <td>  <div id="dvMap" style="width: 100%; height: 400px;border-style: inset;">
           
        </div></td>
    </tr>
   
  </table>
  <div class="dvInsideTL" id="divContent">

     <%-- <img alt="" class="markerheightnew" src="img/2.png" / >
        Action<br />      
        <img alt="" src="img/4.png" class="markerheightnew" />
        Call/Email<br />
       <img alt="" src="img/3.png" class="markerheightnew" />
       Note<br />
        <img alt="" src="img/5.png" class="markerheightnew" />
       Deal<br />   --%>
       <img alt="" class="markerheightnew" src="img/5.png" / >
        Order<br />      
        <img alt="" src="img/3.png" class="markerheightnew" />
        Demo<br />
       <img alt="" src="img/2.png" class="markerheightnew" />
       Faield Visit<br />
        <img alt="" src="img/4.png" class="markerheightnew" />
       Distributor Discussion<br />   
          
  </div>
</div>

          
         
          </div>
            </div>
      
        
         
    </section>

</asp:Content>
