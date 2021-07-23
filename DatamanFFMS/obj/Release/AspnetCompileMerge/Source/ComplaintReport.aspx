<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ComplaintReport.aspx.cs" Inherits="AstralFFMS.ComplaintReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
     <script type="text/javascript">
         $(function () {
             $("#example1").DataTable({
                 "order": [[0, "desc"]]
             });
         });
    </script>
    <script type="text/javascript">
        $(function () {
            $('[id*=ListBox1]').multiselect({
                enableCaseInsensitiveFiltering: true,
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
                $('[id*=DistListbox]').multiselect({
                    enableCaseInsensitiveFiltering: true,
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
             $('[id*=matGrpListBox]').multiselect({
                 enableCaseInsensitiveFiltering: true,
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
            $('[id*=productListBox]').multiselect({
                enableCaseInsensitiveFiltering: true,
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
            $('[id*=LstDepartment]').multiselect({
                enableCaseInsensitiveFiltering: true,
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
            $('[id*=LstCompNature]').multiselect({
                enableCaseInsensitiveFiltering: true,
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
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>
    <style>
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .input-group .form-control {
            height: 34px;
        }

        .multiselect-container > li > a {
            white-space: normal;
        }

        .multiselect-container > li {
            width: 100%;
        }
    </style>
    <script type="text/javascript">
        var V = "";
        function Successmessage(V) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <script type="text/javascript">
        $(function () {
            $('[id*=ListBox1]').multiselect({
                includeSelectAllOption: true
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();
        });
    </script>

    <script type="text/javascript">
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
                        } else {
                            $(this).removeAttr("checked");
                        }
                    });
                } else {
                    //Is Child CheckBox
                    var parentDIV = $(this).closest("DIV");
                    if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                        $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                    } else {
                        $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                    }
                }
            });
        })
    </script>
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
                                <h3 class="box-title">Complaint Report</h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                     <div class="col-lg-3 col-md-3 col-sm-4 col-xs-12">
                                     <div class="form-group">
                                    <label for="exampleInputEmail1">Complaint By:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlComplaint" runat="server" Width="100%" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlComplaint_SelectedIndexChanged">
                                        <asp:ListItem Value="0">-- Select --</asp:ListItem>                                        
                                        <asp:ListItem Text="Distributor" Value="Distributor"></asp:ListItem>
                                        <asp:ListItem Text="Sales Person" Value="SalesPerson"></asp:ListItem>                                        
                                    </asp:DropDownList>
                                    </div>
                               </div>
                                 <div class="col-md-3 col-sm-4 col-xs-12">
                                       <div class="form-group">
                                            <label for="exampleInputEmail1"> Status:</label>
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="100%" CssClass="form-control">
                                            <asp:ListItem Text="All" Value="A" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Pending" Value="P"></asp:ListItem>
                                            <asp:ListItem Text="Resolved" Value="R"></asp:ListItem>
                                            <asp:ListItem Text="WIP" Value="W"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div></div>                                 
                                    </div>
                                    <div class="row" id="divptype" style="display:none;"  runat="server">
                                  <div class="col-md-3 col-sm-4 col-xs-12" >
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Party Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                                    <asp:DropDownList ID="ddlpartytype" OnSelectedIndexChanged="ddlpartytype_SelectedIndexChanged" AutoPostBack="true" Width="100%" CssClass="form-control" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                       <div class="col-md-3 col-sm-4 col-xs-12" id="divpname">
                                                    <div class="form-group">
                                                              <label id="lblpartytypepersons"  runat="server">Party Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                                        <asp:DropDownList ID="ddlpartytypepersons" Width="100%" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                    </div>
                                  <div class="row">
                                     <div id="divsp" style="display:none;"  runat="server" class="col-md-4 col-sm-4 col-xs-12">
                                       <div class="form-group" hidden>
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                            
                                            <asp:ListBox ID="ListBox1" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                         <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                            
                                            <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>
                                        </div>
                                        </div>
                                       <div runat="server" id="divdist" style="display:none;" class="col-md-3 col-sm-4 col-xs-12">
                                             <div class="form-group">
                                            <label for="exampleInputEmail1">Party Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                            
                                            <asp:ListBox ID="DistListbox" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                        </div>
                                      </div>                                   
                                   <div class="row">
                                    <div class="col-md-3 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Product Group:</label>                                          
                                               <asp:ListBox ID="matGrpListBox" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="matGrpListBox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Product:</label>                                       
                                             <asp:ListBox ID="productListBox" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                    </div>
                                </div> 
                                
                                <div class="row">
                                    <div class="col-md-3 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Department:</label>                                           
                                               <asp:ListBox ID="LstDepartment" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="LstDepartment_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Complaint Nature:</label>                                      
                                             <asp:ListBox ID="LstCompNature" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>
                                                               
                                <div class="row">
                                    <div class="col-md-3 col-sm-4 col-xs-12">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <div class="box-footer">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary"
                                    OnClick="btnGo_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                    OnClick="btnCancel_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To CSV" class="btn btn-primary"
                                    OnClick="btnExport_Click"/>
                            </div>                            

                            <div class="box-body table-responsive">
                                <asp:Repeater ID="complreportrpt" runat="server" OnItemDataBound="complreportrpt_ItemDataBound">
                                    <HeaderTemplate>
                                        <table id="example1" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <th>Date</th>
                                                    <th>Complaint By</th>
                                                    <th>Sync Id</th>
                                                    <th>City</th>
                                                    <th id="thsname"  runat="server">Party Type</th>
                                                    <th>Party Name</th>
                                                    <th>Department</th>
                                                    <th>Complaint Nature</th>
                                                    <th>Product</th>
                                                    <th>Complaint</th>
                                                    <th>Status</th>   
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>

                                            <td><%#Convert.ToDateTime(Eval("CompDate")).ToString("dd/MMM/yyyy") %></td>
                                            <asp:Label ID="CompDateLabel" runat="server" Visible="false" Text='<%# Eval("CompDate")%>'></asp:Label>
                                            <td><%#Eval("CompBY") %></td>
                                            <asp:Label ID="CompBYLabel" runat="server" Visible="false" Text='<%# Eval("CompBY")%>'></asp:Label>
                                            <td><%#Eval("SyncId") %></td>
                                            <asp:Label ID="SyncIdLabel" runat="server" Visible="false" Text='<%# Eval("SyncId")%>'></asp:Label>
                                            <td><%#Eval("City") %></td>
                                            <asp:Label ID="CityLabel" runat="server" Visible="false" Text='<%# Eval("City")%>'></asp:Label>
                                            <td id="tdsname" runat="server"><%#Eval("PartyTypeName") %></td>
                                            <asp:Label ID="PartyTypeNameLabel" runat="server" Visible="false" Text='<%# Eval("PartyTypeName")%>'></asp:Label>
                                            <td><%#Eval("Distributor")%></td>
                                            <asp:Label ID="DistributorLabel" runat="server" Visible="false" Text='<%# Eval("Distributor")%>'></asp:Label>
                                            <td><%#Eval("DepName") %></td>
                                            <asp:Label ID="DepNameLabel" runat="server" Visible="false" Text='<%# Eval("DepName")%>'></asp:Label>
                                            <td><%#Eval("ComplaintNature") %></td>
                                            <asp:Label ID="ComplaintNatureLabel" runat="server" Visible="false" Text='<%# Eval("ComplaintNature")%>'></asp:Label>
                                            <td><%#Eval("Item") %></td>
                                            <asp:Label ID="ItemLabel" runat="server" Visible="false" Text='<%# Eval("Item")%>'></asp:Label>
                                            <td><%#Eval("Complaint") %></td>
                                            <asp:Label ID="ComplaintLabel" runat="server" Visible="false" Text='<%# Eval("Complaint")%>'></asp:Label>
                                            <td><%# Eval("Status")%>
                                            <asp:Label ID="StatusLabel" runat="server" Visible="false" Text='<%# Eval("Status") %>'></asp:Label>
                                            <%--<td><%# Eval("Status").ToString().Equals("P") ? "Pending" : "Resolved" %>
                                            <asp:Label ID="StatusLabel" runat="server" Visible="false" Text='<%# Eval("Status").ToString().Equals("P") ? "Pending" : "Resolved" %>'></asp:Label>--%>
                                            
                                           </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                             <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
