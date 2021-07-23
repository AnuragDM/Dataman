<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="StoreAgeingReport_V2.aspx.cs" Inherits="AstralFFMS.StoreAgeingReport_V2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 15000, template: "error"
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
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 15000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <script type="text/javascript">
        $(function () {
            $('[id*=ListBox1]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',
                buttonWidth: '100%',
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

        function validateDist() {


            if ($('#<%=ddlretailer.ClientID%>').val() == "0") {
                errormessage("Please Select Distributor.");
                return false;
            }

        }

        function validateBeat() {



            if ($('#<%=ddlbeat.ClientID%>').val() == "0") {
                errormessage("Please Select Beat.");
                return false;
            }

        }


        function validateItem() {



            if ($('#<%=ddlitem.ClientID%>').val() == "0") {
                errormessage("Please Select Item.");
                return false;
            }


        }
    </script>
    <style type="text/css">
        .input-group .form-control {
            height: 34px;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            /*display: table;*/
        }

        .multiselect-container > li > a {
            white-space: normal;
        }

        .multiselect-container > li {
            width: 212px;
        }

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }
        .button1 {
box-shadow: 0px 2px 4px 2px #888888;
margin-left: 10px;
    margin-top: 7px;
    margin-right: 5px;
}
         h2 {
                 font-size: 20px !important;
    font-weight: 600 !important;
        margin-left: 13px !important;
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
                <!-- left column -->
                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <img id="img-Header" src="img/StroeAgeing.png" style="width: 49px;height: 33px;"/>
                                <%--<h3 class="box-title">Store Ageing Report</h3>--%>
                                <h2 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h2>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">   
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Distributor:</label>
                                            <asp:DropDownList runat="server" ID="ddlretailer" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                     <div class=" col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Beat:</label>
                                            <asp:DropDownList runat="server" ID="ddlbeat"  CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class=" col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Item:</label>
                                            <asp:DropDownList runat="server" ID="ddlitem"  CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div> 

                                <div class="box-footer">                              
                                 
                                        <asp:Button type="button" ID="btnsaleperson" runat="server" Text="Distributor wise" class="btn btn-primary button1"  
                                            OnClick="btnsaleperson_Click"   OnClientClick="javascript:return validateDist();"/>

                                        <asp:Button type="button" ID="btndist" runat="server" Text="Beat wise" class="btn btn-primary button1"  
                                            OnClick="btndist_Click"   OnClientClick="javascript:return validateBeat();"/>

                                        <asp:Button type="button" ID="btnbeat" runat="server" Text="Item wise" class="btn btn-primary button1"
                                            OnClick="btnbeat_Click"   OnClientClick="javascript:return validateItem();"/>                                        

                                       <asp:Button type="button" ID="btnDistributor" runat="server" Text="Last Order (Distributor)" class="btn btn-primary button1"  
                                            OnClick="btnDistributor_Click"/>

                                     <asp:Button type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary button1"
                                            OnClick="btnCancel_Click" />
                                       
                                </div>
                                <br />                              

                                <br />
                                <div>
                                    <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </section>
</asp:Content>