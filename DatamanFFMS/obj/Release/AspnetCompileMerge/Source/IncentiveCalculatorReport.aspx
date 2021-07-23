<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="IncentiveCalculatorReport.aspx.cs" Inherits="AstralFFMS.IncentiveCalculatorReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
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
                            //$(this).removeAttr("checked");
                        }
                    });
                } else {
                    //Is Child CheckBox
                    var parentDIV = $(this).closest("DIV");
                    if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                        $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                    } else {
                        //$("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                    }
                }
            });
        })
    </script>
    <style type="text/css">
        .multiselect-container > li > a {
            white-space: normal;
        }

        .multiselect-container > li {
            width: 212px;
        }

        .input-group .form-control {
            height: 34px;
        }
         .multiselect-container.dropdown-menu {
            width: 100% !important;
        }
    </style>
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
                                <h3 class="box-title">Incentive Calculator Report</h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                            
                                            <asp:ListBox ID="ListBox1" runat="server" class="form-control" SelectionMode="Multiple" Visible="false"></asp:ListBox>
                                            <asp:DropDownList ID="DdlSalesPerson" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="form-group" hidden>
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                            
                                            <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>
                                        </div>                                     
                                    </div>
                                </div>
                                 <div class="row">                                   
                                      <div class="box-header">
                                      <label for="exampleInputEmail1">Month:</label>
                                     <asp:DropDownList ID="monthDDL" runat="server"></asp:DropDownList>&nbsp;&nbsp;<asp:DropDownList ID="yearDDL" runat="server"></asp:DropDownList>
                                     </div>  
                                                                                    
                                   <%-- <div class="clearfix"></div>--%>
                                    <div class="col-md-2 col-sm-6 col-xs-8">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Primary Sale Current Month:</label>
                                            <asp:TextBox ID="salesTextBox" class="numeric text-right form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>  
                                     <div class="col-md-2 col-sm-6 col-xs-12">                                    
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Proposed Growth%:</label>
                                            <asp:TextBox ID="growthTextBox" class="numeric text-right form-control" runat="server" Text="0" OnTextChanged="growthTextBox_TextChanged" AutoPostBack="true" MaxLength="3"></asp:TextBox>
                                        </div>
                                    </div>                                      
                                   
                                      <div class="col-md-2 col-sm-6 col-xs-12">                                    
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Proposed Incentive:</label>
                                            <asp:TextBox ID="txtincentiveAmount" class="numeric text-right form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>                                      
                                                             
                                </div>
                              <%--  <div class="row">
                                    <div class="col-md-2 col-sm-3 col-xs-5">                                    
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Growth%:</label>
                                            <asp:TextBox ID="growthTextBox" class="numeric text-right form-control" runat="server" OnTextChanged="growthTextBox_TextChanged" AutoPostBack="true" MaxLength="3"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>--%>
                            </div>

                            <div class="box-footer">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary"
                                    OnClick="btnGo_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                    OnClick="btnCancel_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                    OnClick="btnExport_Click"/>
                            </div>
                            
                                                    
                            <div id="rptmain">
                                <div class="box-body table-responsive">
                                    <div class="row" style="display:none;">
                                        <div class="col-md-5 col-sm-5 col-xs-9">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Current Incentive:</label>
                                                <asp:TextBox ID="currIncTextBox" class="numeric text-right" runat="server" ForeColor="Red" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-5 col-sm-5 col-xs-9">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Incentive as per growth:</label>
                                                <asp:TextBox ID="incgrowthTextBox" class="numeric text-right" runat="server" ForeColor="Red" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:Repeater ID="increportrpt" runat="server">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped rpttable">
                                                <thead>
                                                    <tr>
                                                       <%-- <th>SNo.</th>--%>
                                                        <th>Name</th>                                                        
                                                        <th>Product</th>
                                                        <th style="text-align:right">Last 6 Month</th>
                                                        <th style="text-align:right">Last Year (Current Month)</th>
                                                        <th style="text-align:right">Proposed Sale</th>
                                                        <th style="text-align:right">Current Month Sale</th>
                                                        <th style="text-align:right">GAP</th>
                                                       <%-- <th style="text-align:right">Incentive</th>--%>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <%--<td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>--%>
                                                <td><%#Eval("Name") %></td>
                                                <%-- Added 06-06-2016 - Nishu --%>
                                          <asp:Label ID="NameLabel" runat="server" Visible="false" Text='<%# Eval("Name")%>'></asp:Label>
                                            <%-- End --%>
                                               <%--  <td><%#Eval("SyncId") %></td>                                               
                                          <asp:Label ID="SyncIdLabel" runat="server" Visible="false" Text='<%# Eval("SyncId")%>'></asp:Label>  --%>                                         
                                                <td><%#Eval("ItemName") %></td>
                                                <%-- Added 06-06-2016 - Nishu --%>
                                          <asp:Label ID="materialGroupLabel" runat="server" Visible="false" Text='<%# Eval("ItemName")%>'></asp:Label>
                                            <%-- End --%>
                                                <td style="text-align:right;"><%#Eval("LAST6MonthAmount") %></td>
                                                <%-- Added 06-06-2016 - Nishu --%>
                                          <asp:Label ID="LAST6MonthAmountLabel" runat="server" Visible="false" Text='<%# Eval("LAST6MonthAmount")%>'></asp:Label>
                                            <%-- End --%>
                                                <td style="text-align:right;"><%#Eval("LAstYearAmount") %></td> 
                                                <%-- Added 06-06-2016 - Nishu --%>
                                          <asp:Label ID="LAstYearAmountLabel" runat="server" Visible="false" Text='<%# Eval("LAstYearAmount")%>'></asp:Label>
                                            <%-- End --%>                                               
                                                <td style="text-align:right;"><asp:TextBox ID="ProposedSaletxt" class="form-control numeric text-right" Width="50%" runat="server" Text='<%#Eval("ProposedSale") %>'  OnTextChanged="ProposedSaletxt_TextChanged" AutoPostBack="true"></asp:TextBox></td>
                                                <td style="text-align:right;"><%#Eval("CurrentMonthAmount") %></td>
                                                <%-- Added 06-06-2016 - Nishu --%>
                                          <asp:Label ID="CurrentMonthAmountLabel" runat="server" Visible="false" Text='<%# Eval("CurrentMonthAmount")%>'></asp:Label>
                                            <%-- End --%>
                                                <td style="text-align:right;"><%#Eval("GAP") %></td>
                                                <%-- Added 06-06-2016 - Nishu --%>
                                          <asp:Label ID="GAPLabel" runat="server" Visible="false" Text='<%# Eval("GAP")%>'></asp:Label>
                                            <%-- End --%>                                              
                                               <%-- <td style="text-align:right;">  <%# Eval("Incentive_1","{0:0.00}")%></td>                                              
                                                <asp:Label ID="IncentiveamountLabel" runat="server" Visible="false" Text='<%# Eval("Incentive_1","{0:0.00}")%>'></asp:Label>--%>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>     </table>       
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
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
