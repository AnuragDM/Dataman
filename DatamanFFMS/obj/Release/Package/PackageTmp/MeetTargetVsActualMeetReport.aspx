<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MeetTargetVsActualMeetReport.aspx.cs" Inherits="AstralFFMS.MeetTargetVsActualMeetReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <style type="text/css">
        .excelbtn, #ContentPlaceHolder1_btnBack {
            background-color: #3c8dbc;
            border-color: #367fa9;
        }

        #excelExport {
            border-radius: 3px;
            -webkit-box-shadow: none;
            box-shadow: none;
            border: 1px solid transparent;
            background-color: #3c8dbc;
            border-color: #367fa9;
            color: white;
            height: 33px;
            padding:0px 12px 3px 15px;
        }
        .jqx-input.jqx-widget-content.jqx-grid-pager-input.jqx-rc-all{
            text-align: center !important;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
           // var date = $('#<%=dateHiddenField.ClientID%>').val();
           var SMID = $('#<%=ddlunderUser.ClientID%>').val();
          <%--  var visID = $('#<%=vistIDHiddenField.ClientID%>').val();
            var beatID = $('#<%=beatIDHiddenField.ClientID%>').val();--%>
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("MeetTargetVsActualMeetReport.aspx/GetDailyWorkingReport") %>',
                contentType: "application/json; charset=utf-8",
                data: '{SMID: "' + SMID + '"}',
                dataType: "json",
                success: function (data) {

                    var source = {
                        localdata: data.d,
                        datatype: "json",
                        datafields:
                        [
                             { name: 'Total', type: "string" },
                            { name: 'MeetType', type: "string" },
                            { name: 'MeetName', type: "string" },
                            { name: 'Target', type: "string" }
                        ]
                          
                        //  id: "Beat",
                    }

                    var dataAdapter = new $.jqx.dataAdapter(source);
                    $("#jqxgrid").jqxGrid(
                     {
                         width: '990px',
                         autoheight: true,
                         source: dataAdapter,
                         sortable: true,
                         filterable: true,
                         autorowheight: true,
                         showstatusbar: true,
                         pageable: true,
                         groupable: true,
                         columnsresize: true,
                         showaggregates: true,
                         statusbarheight: 25,
                         columns: [
                             { text: "Target", datafield: "Total", width: 100 },
                             { text: "Meet Type Name", datafield: "MeetType", width: 100 },
                             { text: "Meet Name", datafield: "MeetName", width: 100 },
                                  { text: "", datafield: "Target", width: 100 },
                             
                         ],
                         groups: ['MeetType', 'Target']
                     });
                    $("#excelExport").jqxButton({ theme: theme });
                    $("#jqxgrid").jqxGrid('expandallgroups');
                    $("#excelExport").click(function () {
                        $("#jqxgrid").jqxGrid('exportdata', 'xls', 'jqxGrid');
                    });
                }
            });
        });
    </script>

      
            <script type="text/javascript">
                function show()
                {
            
            var SMID = $('#<%=ddlunderUser.ClientID%>').val();
         
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("MeetTargetVsActualMeetReport.aspx/GetDailyWorkingReport") %>',
                contentType: "application/json; charset=utf-8",
                data: '{SMID: "' + SMID + '"}',
                dataType: "json",
                success: function (data) {

                    var source = {
                        localdata: data.d,
                        datatype: "json",
                        datafields:
                        [
                             { name: 'Total', type: "string" },
                            { name: 'MeetType', type: "string" },
                            { name: 'MeetName', type: "string" },
                            { name: 'Target', type: "string" }


                        ]

                        //  id: "Beat",
                    }

                    var dataAdapter = new $.jqx.dataAdapter(source);
                    $("#jqxgrid").jqxGrid(
                     {
                         width: '990px',
                         autoheight: true,
                         source: dataAdapter,
                         sortable: true,
                         filterable: true,
                         autorowheight: true,
                         showstatusbar: true,
                         pageable: true,
                         groupable: true,
                         columnsresize: true,
                         showaggregates: true,
                         statusbarheight: 25,
                         columns: [
                             { text: "Target", datafield: "Total", width: 100 },
                             { text: "Meet Type Name", datafield: "MeetType", width: 100 },
                             { text: "Meet Name", datafield: "MeetName", width: 100 },
                                  { text: "", datafield: "Target", width: 100 },

                         ],
                         groups: ['MeetType', 'Target']
                     });
                    $("#excelExport").jqxButton({ theme: theme });
                    $("#jqxgrid").jqxGrid('expandallgroups');
                    $("#excelExport").click(function () {
                        $("#jqxgrid").jqxGrid('exportdata', 'xls', 'jqxGrid');
                    });
                }
            });
        });
    </script>


    <section class="content">
        <div class="row">
            <!-- left column -->
            <div class="col-md-12">
                <div id="InputWork">
                    <!-- general form elements -->
                    <div class="box box-primary">
                        <div class="box-header with-border">
                            <div style="text-align: center;">
                                <h3 class="box-title" style="color: #ff8000;">Dataman Computer Systems (P). Ltd</h3>
                                <br />
                                <h4 style="color: #ff8000;">Kanpur</h4>
                                <br />
                                <h4 style="color: #ff8000;">Meet Target v/s Actual Meet</h4>
                            </div>
                            <div class="col-md-3" style="text-align: left;">
                                <label>Sales Rep. Name :</label>
                                <asp:HiddenField ID="dateHiddenField" runat="server" />
                                <asp:HiddenField ID="smIDHiddenField" runat="server" />
                                <asp:HiddenField ID="beatIDHiddenField" runat="server" />
                                  <%--<div class="form-group col-md-5 paddingleft0">
                                        <label for="exampleInputEmail1">User:</label>&nbsp;&nbsp;<label for="requiredFields" style="color:red;">*</label>--%>
                                        <asp:DropDownList ID="ddlunderUser"  runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:Button Style="margin-right: 5px;margin-top:5px;" type="button" ID="btnshow" runat="server"  Text="Show" class="btn btn-primary" OnClientClick="Show"
                                  />
                                    <%--</div>--%>
                            </div>
                            <div class="col-md-3" style="float: right;">
                                <label>Report Date :</label>&nbsp;&nbsp;&nbsp;<asp:Label ID="currDateLabel" runat="server" Text="Label"></asp:Label>
                                <br />
                            </div>
                        </div>

                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                          <%--  <asp:Label ID="dateLabel" runat="server" Text="Label" ForeColor="#6699ff"></asp:Label>--%>
                           <div class="table table-responsive">
                            <div id="JqxGriddiv">
                                <div class="box box-primary">
                                    <div class="box-header with-border">
                                        <h3 class="box-title"></h3>
                                        <div id="jqxgrid"></div>
                                        <div style="float: left; margin-top: 5px;">
                                            <input type="button" value="Export to Excel" id='excelExport' />
                                        
                                        </div>
                                    </div>
                                </div>
                            </div></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>


