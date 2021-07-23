<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="LastLogin_V2.aspx.cs" Inherits="AstralFFMS.LastLogin_V2" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
<%--        <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
  <%--  <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
 <%--     <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
               
            });
        });
    </script>--%>
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
         /*For everyone */
        input[type=checkbox], input[type=radio] {
    margin-right: 12px !important;
    margin-left: 7px !important;
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
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
        </div>
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>

         <div class="box-body">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                 <img id="img-Header" src="img/lastlogin.png" style="width: 49px;height: 33px;"/>
                                <%--<h3 class="box-title">Last Login Report</h3>--%>
                                <h2 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h2>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Application:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="DdlSalesPerson" Width="100%" CssClass="form-control" runat="server" OnSelectedIndexChanged="DdlSalesPerson_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="Distributor" Text="Distributor"></asp:ListItem>
                                              <asp:ListItem Value="Field" Text="Field"></asp:ListItem>
                                              <asp:ListItem Value="Manager" Text="Manager"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                     </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                      <div class="form-group">
                                        <label for="exampleInputEmail1">Month:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="DropDownList1" Width="100%" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" >
                                             <asp:ListItem Value="01" Text="Jan"></asp:ListItem>
                                              <asp:ListItem Value="02" Text="Feb"></asp:ListItem>
                                              <asp:ListItem Value="03" Text="Mar"></asp:ListItem>
                                              <asp:ListItem Value="04" Text="Apr"></asp:ListItem>
                                              <asp:ListItem Value="05" Text="May"></asp:ListItem>
                                              <asp:ListItem Value="06" Text="Jun"></asp:ListItem>
                                              <asp:ListItem Value="07" Text="Jul"></asp:ListItem>
                                              <asp:ListItem Value="08" Text="Aug"></asp:ListItem>
                                              <asp:ListItem Value="09" Text="Sep"></asp:ListItem>
                                              <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                                              <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                                              <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
                                        </asp:DropDownList>
                                          
                                    </div>
                                </div></div>
                            </div>
                            <div class="box-footer">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Generate" class="btn btn-primary"
                                    OnClick="btnGo_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                    OnClick="btnCancel_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>        

    </section>
</asp:Content>
