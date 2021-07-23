<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DistributerPartyList.aspx.cs" EnableEventValidation="false" Inherits="AstralFFMS.DistributerPartyList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
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
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
    </style>
     <script type="text/javascript">
         //function pageLoad() {
         //    $(".select2").select2();
         //};
             </script>
      <script type="text/javascript">
          //$(function () {
          //    $(".select2").select2();
          //});
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
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to delete?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <script type="text/javascript">
        function DoNav(depId, AreaId) {
            if (depId != "") {
               
                var url = "PartyDashboard.aspx?PartyId=" + depId + "&AreaId=" + AreaId + "&CityID=" + $('#<%=hidDist.ClientID%>').val() + "&VisitID=" + $('#<%=hidVis.ClientID%>').val() + "&Level=" + $('#<%=hidlevel.ClientID%>').val() + "&PageView=" + $('#<%=hidPageName.ClientID%>').val(); 
                window.location.href = url;
            }
        }
    </script>
    <section class="content">

        


       <asp:UpdatePanel ID="updatepa" runat="server" UpdateMode="Conditional">
           <ContentTemplate>
           
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
     
        <div class="box-body" id="mainDiv"  runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Beat Coverage</h3>
                            <div style="float: right">
                                
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click1" />

                             <asp:HiddenField ID="hidDist" runat="server"/>
                                 <asp:HiddenField ID="hidVis" runat="server"/>
                                 <asp:HiddenField ID="hidlevel" runat="server"/>
                                 <asp:HiddenField ID="hidPageName" runat="server"/>

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-md-5">
                                 <div class="form-group">
                                    <label for="exampleInputEmail1">Area:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                  <asp:DropDownList ID="ddlarea" width="100%" OnSelectedIndexChanged="ddlarea_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                 <div class="form-group beat-select">
                                    <label for="exampleInputEmail1">Beat:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                     
                                    <asp:DropDownList ID="ddlbeat" width="100%" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlbeat_SelectedIndexChanged"></asp:DropDownList>
                                         
                                </div>
                        </div>
                       </div>
                        <div class="box-footer">
                           <%-- <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClick="btnGo_Click" />--%>
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnClear" runat="server" Text="Clear" class="btn btn-primary" OnClick="btnClear_Click" />
                            <br />
                          
                        </div>
                    </div>
                </div>


            </div>

        </div>

        <div class="box-body"  runat="server" >
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Party List</h3>
                            <div style="float: right">
                              <asp:Button Style="margin-right: 5px;" type="button" ID="btnAddnewParty" runat="server" Text="Add New Party" class="btn btn-primary" OnClick="btnAddnewParty_Click" />

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" OnItemDataBound="rpt_ItemDataBound" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Party Name</th>
                                                 <th>Contact Person</th>
                                                 <th>Mobile</th>
                                              
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("PartyId")%>','<%#Eval("AreaId")%>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("PartyId") %>' />
                                        <td><%#Eval("PartyName") %></td>
                                         <td><%#Eval("ContactPerson") %></td>
                                         <td><%#Eval("Mobile") %></td>
                                     
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
               <br />
                            <div>
                                <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color:red">*</span>)</b>
                            </div>
        </div>
             
               </ContentTemplate>
       </asp:UpdatePanel>

    </section>
     <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function load1() {
            $("#example1").DataTable();

        }

        $(window).load(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(load1);

        });


    </script>
</asp:Content>
