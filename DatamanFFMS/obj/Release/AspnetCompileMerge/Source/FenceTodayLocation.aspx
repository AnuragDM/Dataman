<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/FFMS.master" CodeBehind="FenceTodayLocation.aspx.cs" Inherits="AstralFFMS.FenceTodayLocation" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%@ Register Src="ctlCalendar.ascx" TagName="Calendar" TagPrefix="ctl" %>
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
     <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js"></script>
   
      <script type="text/javascript">
          $( ".selector" ).datepicker({
              dateFormat: "yy-mm-dd"
          });
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
           $( ".selector" ).datepicker({
               dateFormat: "yy-mm-dd"
           });
           function validate() {
               <%--if (document.getElementById("<%=ddlType.ClientID%>").value == "--Select--" || document.getElementById("<%=ddlType.ClientID%>").value == "0") {
                   errormessage("Please select a Group Name");
                document.getElementById("<%=ddlType.ClientID%>").focus();
                return false;
            }--%>
            if (document.getElementById("<%=DropDownList2.ClientID%>").value == "--Select--" || document.getElementById("<%=DropDownList2.ClientID%>").value == "0") {
                errormessage("Please select a Person Name");
                document.getElementById("<%=DropDownList2.ClientID%>").focus();
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
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.js"></script>--%>
                <%--<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false&callback=initialise"></script>--%>
       <%-- <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=AIzaSyBHVJJpYxYwIl2eHXGVUpandmC8NbJTuAY&callback=initialize"></script>--%>
       <%--   <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=AIzaSyBvCa97dqF2Pao5NPBguRd-bNQgB4VAqLw&callback=initialize"></script>--%>
               <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBvCa97dqF2Pao5NPBguRd-bNQgB4VAqLw&region=IN"></script>

            <script type="text/javascript">
                var markers = [
                <asp:Repeater ID="rptMarkers" runat="server">
                <ItemTemplate>
                            {
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
                              {
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
                    var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
                    var icon1;
                    var value = markers.length;
                    //alert(value);
                    for (i = 0; i < markers.length; i++) {
                        var letter = String.fromCharCode("A".charCodeAt(0) + i - 1);
                        var data = markers[i]
                        var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                        lat_lng.push(myLatlng);
                        if (i == 0) {
                            icon1 = "http://license.dataman.in/images/start.png";
                        }
                        else if (value - 1 == i) {
                            icon1 = "http://license.dataman.in/images/current.png";
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
          //     var service = new google.maps.DirectionsService();  abhishek 29 march 17
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
                    for (var i = 0; i < lat_lng.length-1; i++) {
                      //  alert(i);
                        if ((i + 1) <= lat_lng.length) {
                            var src = lat_lng[i];
                            var des = lat_lng[i + 1];
                          //  alert(src);
                         //   alert("6");alert(des);
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

                    // Party Names
                    var lat_lng1 = [];
                    var latlngbounds1 = "";
                    lat_lng1 = new Array();
                    var value1 = markers1.length;
                    //alert(value1);
                   
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

                        icon1 = "http://license.dataman.in/images/yellow-circle.png";
                        var marker1 = new google.maps.Marker({ position: myLatlng1, map: map, icon: icon1 });
                        (function (marker1, data1) {
                            // Attaching a click event to the current marker
                            google.maps.event.addListener(marker1, "mouseover", function (e) {

                                infoWindow.setContent(data1.Title);
                                infoWindow.open(map, marker1);

                            });
                        })(marker1, data1);
                    }
                }
                google.maps.event.addDomListener(window, 'load', initialize);

            </script>
<div class="container-fluid box">
         <div class="box-header">
                            <h3 class="box-title">Today's Location With Party</h3>
                            </div>
            <div class="clearfix"></div>
          <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="row">
                <div class="form-group col-md-3 col-sm-4 col-xs-12" style="display:none;" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Group Name" >Group Name :</label>
                    <asp:DropDownList ID="ddlType" CssClass="textbox form-control" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" 
                            AutoPostBack="true" Width="100%" runat="server">
                        </asp:DropDownList>
                </div>
                  <div class="form-group col-md-3 col-sm-4 col-xs-12" > 
                    <label for="requiredfield" class="back">*</label>                  
                   <label for="Person Name" >Person Name :</label>
                    <asp:DropDownList ID="DropDownList2" CssClass="textbox form-control" Width="85%"
                            AutoPostBack="true" runat="server" 
                            onselectedindexchanged="DropDownList2_SelectedIndexChanged">
                        </asp:DropDownList>
                </div>
                
                <div class="form-group col-md-4 col-sm-4 col-xs-12 date-calender-img" >                   
                   <label for="Date" >Date :</label>

                    <div class="clearfix"></div>
                    <div class="col-sm-10 col-xs-10 no-padding">
                <asp:TextBox ID="txt_fromdate" runat="server"   CssClass="textbox form-control" MaxLength="10" Width="70%"></asp:TextBox>
                          <ajax:CalendarExtender ID="CalendarExtender5" runat="server"  TargetControlID="txt_fromdate" PopupButtonID="img1" Format="dd-MMM-yyyy"> </ajax:CalendarExtender>
                        </div>
                         <div class="col-md-1 col-sm-1 col-xs-1 no-padding">
                      <a href="javascript:;" class="cal-icon" ID="img1" runat="server"><i class="fa fa-calendar" style="margin:-88px;" aria-hidden="true"></i></a>
                            
                             </div>
                             <%--<asp:ImageButton ID="img1" runat="server" ImageUrl="~/img/Calendar.png" />--%>
                     
                       <%-- <ajax:CalendarExtender ID="cc1" runat="server" TargetControlID="txt_fromdate" PopupButtonID="img1"
                            PopupPosition="TopLeft" Format="dd-MMM-yyyy">
                        </ajax:CalendarExtender>--%>
                </div>

                <div class="clearfix"></div>
                 <div class="row" style="padding:15px;">
                <div class="form-group col-md-2 col-sm-2 col-xs-4" >                   
                   <label for="Time From" >Time From :</label>
                    <asp:TextBox ID="Txt_FromTime" runat="server" CssClass="textbox form-control"  MaxLength="5" Width="100%"></asp:TextBox>
                </div>
                <div class="form-group col-md-2 col-sm-2 col-xs-4" >                   
                   <label for="To" >To :</label>
                    <asp:TextBox ID="TextBox2" runat="server" CssClass="textbox form-control"  MaxLength="5" Width="100%"></asp:TextBox>
                </div>
              
              
                 <div class="form-group col-md-2 col-sm-2 col-xs-4" >                   
                   <label for="Party Area" >Party Area :</label><br/>
                    <div class="col-md-6 no-padding"><asp:TextBox ID="txtradius" runat="server" CssClass="textbox form-control" Text="50" MaxLength="4" ></asp:TextBox>
                     </div>
                     <div class="col-md-3 paddingright01">Km.</div>
                      
                      <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtradius">
                        </ajax:FilteredTextBoxExtender>
                </div>
                <div class="form-group col-md-2 col-sm-2 col-xs-4 paddingleft2">                            
                   <label for="Location" >Location :</label><br/>
                    <asp:DropDownList ID="ddlloc" runat="server" CssClass="textbox form-control locationSelect" 
                        onselectedindexchanged="ddlloc_SelectedIndexChanged" AutoPostBack="true" >
                   <asp:ListItem Text="All" Value="0"></asp:ListItem>
                 <asp:ListItem Text="GPS" Value="G" Selected="True"></asp:ListItem>
                 <asp:ListItem Text="Tower" Value="C"></asp:ListItem>
                 </asp:DropDownList>
                 <asp:HiddenField ID="hdnFlat" runat="server" />
                 <asp:HiddenField ID="hdnFlng" runat="server" />
                </div>
                 <div class="form-group col-md-2 col-sm-2 col-xs-4" >                            
                    <label for="Accuracy" >Accuracy :</label><br/>


                     <div class="col-md-4 no-padding">
                     <asp:TextBox ID="txtaccu" runat="server" Text="50"  CssClass="form-control"></asp:TextBox>
                     </div>
                     <div class="col-md-3 paddingright01">
                     
                     (Metres)</div>
                          <ajax:FilteredTextBoxExtender ID="txtaccu_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtaccu">
                        </ajax:FilteredTextBoxExtender>
                </div>
                     </div>
                

                <div class="form-group col-md-5 col-sm-5 col-xs-12" style="padding:15px;">
                                          
                    <asp:Button ID="Btn1" runat="server" Text="Generate" OnClick="Btn1_Click" CssClass="btn btn-primary"
                            OnClientClick="return validate()" />
                </div>
                <div class="clearfix"></div>

  
                                    <div id="map_canvas" style="width: 100%; height: 500px">
                                    </div>
                                
          <div class="table-responsive ">
            <table class="table ">
                <tr>
                    <%--<div style="height: auto; width: 950px;">--%>
                        <td colspan="4">
                            <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False" BackColor="White" Visible="true"
                                BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                Width="1000px" DataKeyNames="Id" EmptyDataText="No Records Found!!">
                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                 <EmptyDataRowStyle BackColor="#3c8dbc" ForeColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sno.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CDate" HeaderText="Date" />
                                    <asp:BoundField DataField="Latitude" HeaderText="Latitude" />
                                    <asp:BoundField DataField="Longitude" HeaderText="Longitude" />
                                    <asp:BoundField DataField="Accuracy" HeaderText="Accuracy" ItemStyle-Width="60px" />
                                    <asp:BoundField DataField="Area" HeaderText="Location(Area)" ItemStyle-Width="400px" />
                                </Columns>
                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <PagerStyle BackColor="#3c8dbc" ForeColor="White"  /> 
                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#3C8DBC" Font-Bold="True" ForeColor="White" />
                              <AlternatingRowStyle BackColor="#FFFFFF" />
                            </asp:GridView>
                   <%-- </div>--%>
                    </td>
                </tr>
            </table>
              </div></div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Btn1" />
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