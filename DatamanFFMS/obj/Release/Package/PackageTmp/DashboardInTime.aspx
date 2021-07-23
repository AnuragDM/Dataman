<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DashboardInTime.aspx.cs" Inherits="AstralFFMS.DashboardInTime" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
      <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>

     <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
     
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
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
        #select2-ContentPlaceHolder1_ddlParentLoc-container{
        margin-top:-8px !important;
        }
            .container {
        width: 100%;
        height: 200px;
        background: aqua;
        margin: auto;
        padding: 10px;
        }
        .one {
        width: 50%;
        height: 200px;
        background: red;
        float: left;
        }
        .two {
        width: 49%;
        margin-left: 51%;
        height: 200px;
        background: black;
        }
          .totalfont {
            /*font-size:21px;*/
        }

         .colornote li {
            list-style: none;
        }

        .colornote {
            padding: 0;
        }

            .colornote li {
                list-style: none;
                padding: 12px 0;
                border-bottom: 1px solid #c1c1c1;
                font-weight: normal;
            }

                .colornote li:last-child {
                    border: none;
                }

                .colornote li span {
                    font-weight: normal;
                    margin-left: 5px;
                }


                   


h4.great {
	background:rgba(66, 113, 244,0.6);
	margin: 0 0 0px 275px;
	padding: 7px 15px;
	color: #ffffff;
	font-size: 18px;
	font-weight: 600;
	border-radius: 11px;
	display: inline-block;
	-moz-box-shadow:    2px 4px 5px 0 #ccc;
  	-webkit-box-shadow: 2px 4px 5px 0 #ccc;
  	box-shadow:         2px 4px 5px 0 #ccc;
}



.price-slider {
	margin-bottom: 70px;
}

.price-slider span {
	font-weight: 200;
	display: inline-block;
	color:white;
	font-size: 17px;
}
    </style>
    
    
      <script type="text/javascript">
          $(document).ready(function () {
              pageLoad();

              Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandle);
              function endRequestHandle(sender, Args) {
                  pageLoad();
              };
          });

          //var prm = Sys.WebForms.PageRequestManager.getInstance();
          //if (prm != null) {
          //    prm.add_endRequest(function (sender, e) {
          //        if (sender._postBackSettings.panelsToUpdate != null) {
          //            pageLoad();

          //            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandle);
          //            //function endRequestHandle(sender, Args) {
          //            //    pageLoad();
          //        }
          //    });
          //};
    </script>
      <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
          .multiselect-container.dropdown-menu {
        width: 100% !important;
        }


        .select2-container {
            /*display: table;*/
        }
         .input-group .form-control {
            height: 34px;
        }

        .multiselect-container > li > a {
            white-space: normal;
        }
    </style>
 

    <script type="text/javascript">
        function pageLoad() {
            //$('[id*=lstUnderUser]').multiselect({
            //    enableCaseInsensitiveFiltering: true,
            //    buttonWidth: '100%',
            //    includeSelectAllOption: true,
            //    maxHeight: 200,
            //    width: 215,
            //    enableFiltering: true,
            //    filterPlaceholder: 'Search'
            //});

        };
    </script>
        <script type="text/javascript">
            $(function () {
                $("#example1").DataTable({
                    "order": [[1, "asc"]]
                });

            });
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
    <script>
        function getQueryVariable(variable) {
            var query = window.location.search.substring(1);
            var vars = query.split("&");
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split("=");
                if (pair[0] == variable) {
                    return pair[1];
                }
            }
            alert('Query Variable ' + variable + ' not found');
        }

        function DoNav(SMId) {
            if (SMId != "") {
                var name = getQueryVariable("Name");
                var date = $("#<%=FromDate.ClientID%>").val();
               // var date = getQueryVariable("Date");
                window.open("DashboardSecondaryDetail.aspx?Date=" + date + "&Name=" + name + "&SMId=" + SMId + "");
            }
        }
    </script>
    <script src="dist/js/demo.js" type="text/javascript"></script>
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
         <div class="content">
        <asp:UpdatePanel ID="mainUp" runat="server">
            <ContentTemplate>
                <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title" runat="server" id="lblHeading"></h3>
                            <div style="float: right">
                               
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <div class="col-lg-9 col-md-9 col-sm-7 col-xs-9">
                                <div class="col-md-12 paddingleft0">
                                    <div id="DIVFrom" class="form-group col-md-3 paddingleft0">
                                        <label for="exampleInputEmail1">From Date:</label>
                                       <asp:TextBox ID="FromDate" runat="server" CssClass="form-control" onChange="showspinner();" AutoPostBack="true" OnTextChanged="FromDate_TextChanged" Style="background-color: white;"></asp:TextBox>
                                      <ajaxToolkit:CalendarExtender ID="CalendarExtender5" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender12" TargetControlID="FromDate"></ajaxToolkit:CalendarExtender>
                                    </div>
                                    <div id="DIVTo" class="form-group col-md-3 paddingleft0">
                                        <label for="exampleInputEmail2">To Date:</label>
                                       <asp:TextBox ID="ToDate" runat="server" CssClass="form-control" onChange="showspinner();" AutoPostBack="true" OnTextChanged="ToDate_TextChanged" Style="background-color: white;"></asp:TextBox>
                                      <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender13" TargetControlID="ToDate"></ajaxToolkit:CalendarExtender>
                                    </div>
                                  <div id="DIVUnder" runat="server" class="form-group col-md-3">
                                            <label for="exampleInputEmail1">Sales Person:</label>
                                     <%--  <asp:ListBox ID="lstUndeUser" CssClass="form-control" Width="100%" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="lstUndeUser_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>--%>
                                            <asp:DropDownList ID="lstUndeUser" Width="100%" runat="server" onChange="showspinner();" AutoPostBack="true" OnSelectedIndexChanged="lstUndeUser_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                        </div> 
                                </div>
                                 <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="btnExport_Click" />
                            </div>
                        </div>
                      
                  
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rptt" runat="server">
                                            <HeaderTemplate>
                                                <table id="example1" class="table table-bordered table-striped">
                                                    <thead>
                                                        <tr>
                                                            <th>Name</th>
                                                            <th>Date</th>
                                                            <th>In-Time</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                               <%-- <tr onclick="DoNav('<%#Eval("SMId") %>');">--%>
                                                 <tr>
                                                    <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("SMId") %>' />
                                                    <td><%#Eval("smname") %>
                                                      <asp:Label ID="lblsmname" runat="server" Visible="false" Text='<%# Eval("smname")%>'></asp:Label></td>
                                                    <td><%#Convert.ToDateTime(Eval("date")).ToString("dd/MMM/yyyy")%>
                                                      <asp:Label ID="lbldate" runat="server" Visible="false" Text='<%#Convert.ToDateTime(Eval("date")).ToString("dd/MMM/yyyy")%>'></asp:Label></td>
                                                    <td><%#Eval("time") %>
                                                      <asp:Label ID="lbltime" runat="server" Visible="false" Text='<%# Eval("time")%>'></asp:Label></td>
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
            </ContentTemplate>
              <Triggers>
               <asp:PostBackTrigger ControlID="FromDate"/>
                <asp:PostBackTrigger ControlID="ToDate"/>
               <asp:PostBackTrigger ControlID="lstUndeUser"/>
             </Triggers>
        </asp:UpdatePanel>

    </div>
       </section>
   


    <script type="text/javascript">
        function showspinner() {

            $("#spinner").show();

        };
        function hidespinner() {

            $("#spinner").hide();

        };

     </script> 
  <style>
       .primaryHeading {
           background-color:rgba(243,156,18,0.1);
       }
        .secondaryHeading {
           background-color:rgba(0,166,90,0.1);
       }
    .panel-heading.primaryHeading {
    background-color: rgb(0,166,90);
    color: white;
    font-size: 20px;
}
    .panel-heading.secondaryHeading {
    background-color: rgba(243, 156, 18,0.8);
    border-color: black;
    color: white;
    font-size: 20px;
}
    .panel-heading.UnApprovedHeading {
    background-color: rgba(0,192,239,1.0);
    border-color: black;
    color: white;
    font-size: 20px;
}
       .TotalEmp {
             background-color:rgb(149, 176, 219);
           border-style: solid;
            border-color:rgb(98, 132, 186);
          color:white;
       }
       .Present {
           background-color: rgba(0,141,76,0.3);
          color:white;
            border-style: solid;
            border-color:rgb(57, 173, 105);
       }
       .Apsent {
            background-color:rgba(211, 55, 36, 0.4);
            border-style: solid;
            border-color:rgb(193, 131, 125);
          color:white;
       }
       .Leave {
            background-color:rgba(255, 119, 1, 0.5);
          color:white;
           border-style: solid;
            border-color:rgb(219, 148, 87);
       }

       
.fa-minus::before {
    color: white;
    content: "";
}
   </style>
    <style>
       /*.small-box > .small-box-footer {
    background: rgba(0, 0, 0, 0.1) none repeat scroll 0 0;
    color: rgba(255, 255, 255, 0.8);
    display: block;
    margin-top: 61px;
    padding: 3px 0;
    position: relative;
    text-align: center;
    text-decoration: none;
    z-index: 10;
}*/
       .inner > p {
    font-size:27px;
    /*text-align: center;*/
}
       .inner > span {
    font-size: 27px;
    /*padding-left: 124px;*/
}
       .headerbottom {
           margin-bottom:10px;
       }
   </style>

</asp:Content>

<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
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

        #select2-ContentPlaceHolder1_ddlParentLoc-container {
            margin-top: -8px !important;
        }

        .container {
            width: 100%;
            height: 200px;
            background: aqua;
            margin: auto;
            padding: 10px;
        }

        .one {
            width: 50%;
            height: 200px;
            background: red;
            float: left;
        }

        .two {
            width: 49%;
            margin-left: 51%;
            height: 200px;
            background: black;
        }

        .totalfont {
            /*font-size:21px;*/
        }

        .colornote li {
            list-style: none;
        }

        .colornote {
            padding: 0;
        }

            .colornote li {
                list-style: none;
                padding: 12px 0;
                border-bottom: 1px solid #c1c1c1;
                font-weight: normal;
            }

                .colornote li:last-child {
                    border: none;
                }

                .colornote li span {
                    font-weight: normal;
                    margin-left: 5px;
                }





        h4.great {
            background: rgba(66, 113, 244,0.6);
            margin: 0 0 0px 275px;
            padding: 7px 15px;
            color: #ffffff;
            font-size: 18px;
            font-weight: 600;
            border-radius: 11px;
            display: inline-block;
            -moz-box-shadow: 2px 4px 5px 0 #ccc;
            -webkit-box-shadow: 2px 4px 5px 0 #ccc;
            box-shadow: 2px 4px 5px 0 #ccc;
        }



        .price-slider {
            margin-bottom: 70px;
        }

            .price-slider span {
                font-weight: 200;
                display: inline-block;
                color: white;
                font-size: 17px;
            }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {

            pageLoad();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandle);
            function endRequestHandle(sender, Args) {
                pageLoad();
            };
        }
      );

    </script>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }


        .select2-container {
            /*display: table;*/
        }

        .input-group .form-control {
            height: 34px;
        }

        .multiselect-container > li > a {
            white-space: normal;
        }
    </style>
    <script type="text/javascript">
        $(function () {

        });
    </script>

    <script type="text/javascript">
        function pageLoad() {

        };
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

    <script src="dist/js/demo.js" type="text/javascript"></script>
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
        <div class="content">
            <asp:UpdatePanel ID="mainUp" runat="server">
                <ContentTemplate>
                    <div class="box-body" id="rptmain" runat="server">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="box">
                                    <div class="box-header">
                                        <h3 class="box-title">Present List</h3>
                                        <div style="float: right">
                                        </div>
                                    </div>
                                    <!-- /.box-header -->
                                    <div class="box-body">
                                        <div class="col-lg-9 col-md-9 col-sm-7 col-xs-9">
                                            <div class="col-md-12 paddingleft0">
                                                <div id="DIV1" class="form-group col-md-3">
                                                    <label for="exampleInputEmail1">For Date:</label>
                                                    <asp:TextBox ID="FromDate" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="FromDate_TextChanged" Style="background-color: white;"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender5" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender12" TargetControlID="FromDate"></ajaxToolkit:CalendarExtender>
                                                </div>


                                                <div class="col-md-2 box-body1">
                                                    <label class="lbl">Sales Person:</label>
                                               
                                                   <asp:DropDownList ID="ddlSalesPerson" Width="100%" runat="server" onChange="showspinner();" AutoPostBack="true" OnSelectedIndexChanged="ddlSalesPerson_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                                 
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="box-body table-responsive">
                                        <asp:Repeater ID="rpt" runat="server">
                                            <HeaderTemplate>
                                                <table id="example1" class="table table-bordered table-striped">
                                                    <thead>
                                                        <tr>
                                                            <th>Name</th>
                                                            <th>Date</th>
                                                            <th>In-Time</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr onclick="DoNav('<%#Eval("SMId") %>');">

                                                    <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("SMId") %>' />
                                                    <td><%#Eval("smname") %></td>
                                                    <td><%#Eval("date") %></td>
                                                    <td><%#Eval("time") %></td>

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
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="FromDate" />
                </Triggers>
            </asp:UpdatePanel>

        </div>
    </section>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>


    <script type="text/javascript">

        function showspinner() {

            $("#spinner").show();

        };
        function hidespinner() {

            $("#spinner").hide();

        };

    </script>
    <style>
        .primaryHeading {
            background-color: rgba(243,156,18,0.1);
        }

        .secondaryHeading {
            background-color: rgba(0,166,90,0.1);
        }

        .panel-heading.primaryHeading {
            background-color: rgb(0,166,90);
            color: white;
            font-size: 20px;
        }

        .panel-heading.secondaryHeading {
            background-color: rgba(243, 156, 18,0.8);
            border-color: black;
            color: white;
            font-size: 20px;
        }

        .panel-heading.UnApprovedHeading {
            background-color: rgba(0,192,239,1.0);
            border-color: black;
            color: white;
            font-size: 20px;
        }

        .TotalEmp {
            background-color: rgb(149, 176, 219);
            border-style: solid;
            border-color: rgb(98, 132, 186);
            color: white;
        }

        .Present {
            background-color: rgba(0,141,76,0.3);
            color: white;
            border-style: solid;
            border-color: rgb(57, 173, 105);
        }

        .Apsent {
            background-color: rgba(211, 55, 36, 0.4);
            border-style: solid;
            border-color: rgb(193, 131, 125);
            color: white;
        }

        .Leave {
            background-color: rgba(255, 119, 1, 0.5);
            color: white;
            border-style: solid;
            border-color: rgb(219, 148, 87);
        }


        .fa-minus::before {
            color: white;
            content: "";
        }
    </style>
    <style>
        /*.small-box > .small-box-footer {
    background: rgba(0, 0, 0, 0.1) none repeat scroll 0 0;
    color: rgba(255, 255, 255, 0.8);
    display: block;
    margin-top: 61px;
    padding: 3px 0;
    position: relative;
    text-align: center;
    text-decoration: none;
    z-index: 10;
}*/
        .inner > p {
            font-size: 27px;
            /*text-align: center;*/
        }

        .inner > span {
            font-size: 27px;
            /*padding-left: 124px;*/
        }

        .headerbottom {
            margin-bottom: 10px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
                "order": [[0, "asc"]]
            });

        });
    </script>
</asp:Content>--%>
