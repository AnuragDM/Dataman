<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.master" AutoEventWireup="true" Inherits="GeoFencing" Codebehind="GeoFencing.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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
        <div class="box-body" id="rptmain" runat="server" >
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                       
                    
                        <div class="clearfix"></div>
                            

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
<script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>
<link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/blitzer/jquery-ui.css"
rel="stylesheet" type="text/css" />
     
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <asp:Image ID="Image1" ImageUrl="~/images/waiting.gif" AlternateText="Processing"
                runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <ajax:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
        PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=AIzaSyBvCa97dqF2Pao5NPBguRd-bNQgB4VAqLw&callback=initialize"></script>
          <%-- <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?v=3.6&sensor=false"></script>--%>
    <script src="Scripts/utility-0.2.3.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function () {

            LoadMap();
        });
        var map;
        function LoadMap() {

            var center_pt = new google.maps.LatLng(26.4432949, 80.2874216);
            var mapOptions = {
                zoom: 8,
                center: center_pt,
                mapTypeControl: false,
                streetViewControl: true,
                zoomControlOptions: {
                    style: google.maps.ZoomControlStyle.SMALL
                }

            };
            map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);

            map.setMapTypeId(google.maps.MapTypeId.ROADMAP);
            var styles = [
                                                                   {
                                                                       elementType: "geometry",
                                                                       stylers: [
                                                                               { hue: "#00ffe6" },
                                                                               { saturation: -10 }

                                                                       ]
                                                                   }, {
                                                                       featureType: "road",
                                                                       elementType: "geometry",
                                                                       stylers: [
                                                                               { lightness: 100 },
                                                                               { visibility: "simplified" }
                                                                       ]
                                                                   }, {
                                                                       featureType: "road",
                                                                       elementType: "labels",
                                                                       stylers: [
                                                                               { visibility: "off" }
                                                                       ]
                                                                   }
            ];

            map.setOptions({ styles: styles });
            // map.setOptions(mapOptions);
            initWidget(map);
        }
    </script>
    <script type="text/javascript">
        $(function () {
            $("#btnShow").click(function () {

                $("#dialog").dialog({

                    modal: true,
                    title: "Fence Address",
                    width: 600,
                    height: 250,
                    buttons: {
                        Save: function () {
                            if (document.getElementById("<%=txtFaddr.ClientID%>").value == "" || document.getElementById('<%=ddlgrp.ClientID%>').value == "0" || document.getElementById('<%=ddlgrp.ClientID%>').value == "") {
                                alert("Fields marked with * are mandatory");

                            }
                            else {
                                var pageUrl = '<%=ResolveUrl("~/WebService2.asmx")%>';
                                $.ajax({
                                    type: "POST",
                                    url: pageUrl + '/SaveFenceAddress',
                                    contentType: "application/json; charset=utf-8",
                                    data: "{'Fradius':'" + document.getElementById("<%=hdnradius.ClientID %>").value + "','Faddr':'" + document.getElementById("<%=txtFaddr.ClientID %>").value + "','FgroupId':'" + document.getElementById("<%=ddlgrp.ClientID %>").value + "'}",
                                    dataType: "json",
                                    success: function (data) {
                                        alert(data.d);
                                        $("#dialog").dialog('close');
                                    },
                                    error: function (requestObject, error, errorThrown) {
                                        alert(error);
                                    }

                                });
                            }
                        }
                    },
                    open: function () {
                        if (document.getElementById("<%=hdnlatlng.ClientID%>").value == "") {
                            alert("Please Select a Point for Address");
                            $(this).dialog('close');
                        }
                        else {
                            var pageUrl = '<%=ResolveUrl("~/WebService2.asmx")%>';
                            $('#<%=lbladdr.ClientID%>').html("");
                            document.getElementById("<%=txtFaddr.ClientID%>").value = "";
                            $.ajax({
                                type: "POST",
                                url: pageUrl + '/GetAddressFence',
                                contentType: "application/json; charset=utf-8",
                                data: "{'hdnlatlng':'" + document.getElementById("<%=hdnlatlng.ClientID %>").value + "'}",
                                dataType: "json",
                                success: function (data) {
                                    $('#<%=lbladdr.ClientID%>').html(data.d);

                                },
                                error: function (requestObject, error, errorThrown) {
                                    alert(error);
                                }

                            });
                            }
                    }
                });
            });
        });
</script>
       <div id="divInfo" style="font-family: Arial; font-size: 16px; color: Red;"></div>
   <input id = "btnShow" type="button" value="Show Map Address" class="btn btn-primary" style="width:180px"/>
   
  <span style="float:right"> <asp:Button ID="btnlist" runat="server" Text="List" CssClass="btn btn-primary" Width="80px"  PostBackUrl="~/FenceAddressList.aspx" /></span>
<div id="dialog" style="display:none";>
<div id="dvMap" style="width:300px">
<table style="width:500px">
<tr> 
<td><strong>Address : </strong></td>
<td colspan="4"><strong>  <asp:Label ID="lbladdr" runat="server"></asp:Label></strong></td>
</tr>
<tr>
<td><strong><span class="back">*</span>Fence Address : </strong></td>
<td colspan="4"><asp:TextBox ID="txtFaddr" runat="server" Width="300px" TextMode="MultiLine"></asp:TextBox></td>
</tr>
<tr>
<td><strong> <span class="back">*</span>Group : </strong></td>
<td colspan="4"><asp:DropDownList ID="ddlgrp" runat="server" Width="120px"></asp:DropDownList></td>
</tr>
</table>




</div>
</div>
  
    <asp:HiddenField ID="hdnlatlng" runat="server" />
    <asp:HiddenField ID="hdnradius" runat="server" />
    <br />
    <br />
    <div id="map_canvas" style="width: auto;height: 700px; ">
    </div> 
            <script type="text/javascript">
                //Draw Widget Circle
                function initWidget(map) {
                    var distanceWidget = new DistanceWidget(map);

                    google.maps.event.addListener(distanceWidget, 'distance_changed', function () {
                        displayInfo(distanceWidget); //Put you core filter logic here
                    });

                    google.maps.event.addListener(distanceWidget, 'position_changed', function () {
                        displayInfo(distanceWidget); //Put you core filter logic here          
                    });
                }
                //For display center and distance
                function displayInfo(widget) {
                    var info = document.getElementById('divInfo');
                    document.getElementById("<%=hdnlatlng.ClientID %>").value = "";
     document.getElementById("<%=hdnlatlng.ClientID %>").value = widget.get('position');
    document.getElementById("<%=hdnradius.ClientID %>").value = widget.get('distance');

    info.innerHTML = 'Position: ' + widget.get('position') + ', Distance (in Km): ' +
    widget.get('distance');
}

/*------------------------------------Create Distance Widget--------------------*/
function DistanceWidget(map) {

    this.set('map', map);
    this.set('position', map.getCenter());
    //Anchored image
    var image = {
        url: 'Images/redtime.png'
        //       size: new google.maps.Size(24, 24), origin: new google.maps.Point(0,0),   
        //        anchor: new google.maps.Point(12, 12)
    };

    //Cnter Marker
    var marker = new google.maps.Marker({
        draggable: true,
        icon: image,
        title: 'Drag to move into new location!',
        raiseOnDrag: false
    });
    marker.bindTo('map', this);
    marker.bindTo('position', this);
    //Radius Widget
    var radiusWidget = new RadiusWidget();
    radiusWidget.bindTo('map', this);
    radiusWidget.bindTo('center', this, 'position');
    this.bindTo('distance', radiusWidget);
    this.bindTo('bounds', radiusWidget);
}
DistanceWidget.prototype = new google.maps.MVCObject();

/*------------------------------Create Radius widget-------------------------*/
function RadiusWidget() {
    var circleOptions = {
        fillOpacity: 0.05,
        fillColor: '#686868',
        strokeColor: '#686868',
        strokeWeight: 1,
        strokeOpacity: 0.8
    };
    var circle = new google.maps.Circle(circleOptions);

    this.set('distance', 40);
    this.bindTo('bounds', circle);
    circle.bindTo('center', this);
    circle.bindTo('map', this);
    circle.bindTo('radius', this);
    // Add the sizer marker
    this.addSizer_();
}
RadiusWidget.prototype = new google.maps.MVCObject();
//Distance has changed event handler.      
RadiusWidget.prototype.distance_changed = function () {
    this.set('radius', this.get('distance') * 1000);
};

//Sizer handler
RadiusWidget.prototype.addSizer_ = function () {
    var image = {
        url: 'Resize.png',
        size: new google.maps.Size(24, 24),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(12, 12)
    };

    var sizer = new google.maps.Marker({
        draggable: true,
        icon: image,
        cursor: 'ew-resize',
        title: 'Drag to resize the cicle!',
        raiseOnDrag: false
    });

    sizer.bindTo('map', this);
    sizer.bindTo('position', this, 'sizer_position');

    var me = this;
    google.maps.event.addListener(sizer, 'drag', function () {
        me.setDistance();
    });

    google.maps.event.addListener(sizer, 'dragend', function () {
        me.fitCircle();
    });
};

//Center changed handler
RadiusWidget.prototype.center_changed = function () {
    var bounds = this.get('bounds');

    if (bounds) {
        var lng = bounds.getNorthEast().lng();
        var position = new google.maps.LatLng(this.get('center').lat(), lng);
        this.set('sizer_position', position);

    }
};

//Distance calculator
RadiusWidget.prototype.distanceBetweenPoints_ = function (p1, p2) {
    if (!p1 || !p2) {
        return 0;
    }

    var R = 6371; // Radius of the Earth in km
    var dLat = (p2.lat() - p1.lat()) * Math.PI / 180;
    var dLon = (p2.lng() - p1.lng()) * Math.PI / 180;
    var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
    Math.cos(p1.lat() * Math.PI / 180) * Math.cos(p2.lat() * Math.PI / 180) *
    Math.sin(dLon / 2) * Math.sin(dLon / 2);
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    var d = R * c;

    //Limit max 100m and min 10 mt
    if (d > 0.10) {
        d = 0.10;
    }
    if (d < 0.01) {
        d = 0.01;
    }
    return d;

};

//Set distance
RadiusWidget.prototype.setDistance = function () {
    var pos = this.get('sizer_position');
    var center = this.get('center');
    var distance = this.distanceBetweenPoints_(center, pos);
    this.set('distance', distance);
    var bounds = this.get('bounds');
    if (bounds) {
        var lng = bounds.getNorthEast().lng();
        var position = new google.maps.LatLng(this.get('center').lat(), lng);
        this.set('sizer_position', position);
    }
};

//Fit circle when changed
RadiusWidget.prototype.fitCircle = function () {

    var bounds = this.get('bounds');

    if (bounds) {
        map.fitBounds(bounds);

        var lng = bounds.getNorthEast().lng();
        var position = new google.maps.LatLng(this.get('center').lat(), lng);
        this.set('sizer_position', position);
    }
};

    </script>

        </ContentTemplate>
        <Triggers>
           
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

