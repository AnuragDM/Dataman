<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/FFMS.Master" CodeBehind="rptMobileLogedUser.aspx.cs" Inherits="AstralFFMS.rptMobileLogedUser" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <%-- <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>--%>

   <%-- <script type="text/javascript">
        function pageLoad() {
            $("#example1").DataTable({ paging: false });
           
        };
    </script>--%>

    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <script type="text/javascript">
        //$(function () {
        //    $(".select2").select2();
        //});
    </script>
     <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=lstState]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '200px',
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
             $('[id*=lstCity]').multiselect({
                 enableCaseInsensitiveFiltering: true,
                 buttonWidth: '200px',
                 includeSelectAllOption: true,
                 maxHeight: 200,
                 width: 215,
                 enableFiltering: true,
                 filterPlaceholder: 'Search'
             });
         });
    </script>

    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            /*display: table;*/
        }

        #ContentPlaceHolder1_gvPartyData th {
            text-align: center;
        }

        #ContentPlaceHolder1_gvPartyData td, th {
            padding: 4px;
        }
    </style>
    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 250, position: "top-right", opacity: 2,
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
                width: 250, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <script type="text/javascript">
        function Validate() {
            if ($('#<%=ddlMonth.ClientID%>').val() == "" || $('#<%=ddlMonth.ClientID%>').val() == "--Please select--") {
                errormessage("Please select Month.");
                return false;
            }
            <%--if ($('#<%=ddlcountry.ClientID%>').val() == "" || $('#<%=ddlcountry.ClientID%>').val() == "--Please select--") {
                errormessage("Please select country.");
                return false;
            }
            if ($('#<%=ddlregion.ClientID%>').val() == "" || $('#<%=ddlregion.ClientID%>').val() == "--Please select--") {
                errormessage("Please select region.");
                return false;
            }     --%>      
        }
    </script>  
    

    <section class="content">
        <script type="text/javascript">
            $(function () {
                $("#example1 [id*=chkAll]").click(function () {
                    if ($(this).is(":checked")) {
                        $("#example1 [id*=chkItem]").prop("checked", "checked");
                    } else {
                        $("#example1 [id*=chkItem]").removeAttr("checked");
                    }
                });
                $("#example1 [id*=chkItem]").click(function () {
                    if ($("#example1 [id*=chkItem]").length == $("#tblCustomers [id*=chkItem]:checked").length) {
                        $("#example1 [id*=chkAll]").prop("checked", "checked");
                    } else {
                        $("#example1 [id*=chkAll]").removeAttr("checked");
                    }
                });
            });

</script>

        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <!-- left column -->
            <div class="col-md-12">
                <div id="InputWork">
                    <!-- general form elements -->
                    <div class="box box-primary">
                        <div class="box-header with-border">
                           <%-- <h3 class="box-title">Distributor/User Login</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            <div style="float: right">
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                                <div class="col-lg-9 col-md-8 col-sm-7 col-xs-9">
                                    <div class="row">
                                        <div class="form-group col-md-3" id="div2" runat="server" style="display: block;">
                                            <input type="hidden" id="hdnid" />
                                            <label for="exampleInputEmail1">Month:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <span class="">
                                                <asp:DropDownList Width="100%" ID="ddlMonth" class="form-control" runat="server">

                                                </asp:DropDownList>
                                            </span>
                                        </div>
                                  <%--  </div>--%>
                                   <%-- <div class="row">--%>
                                        <div class="form-group col-md-3" id="divparent" runat="server" style="display: block;">

                                            <label for="exampleInputEmail1">Loged:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlLoged" Width="100%" runat="server" class="form-control">                                              
                                                 <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                 <asp:ListItem Text="No" Value="2"></asp:ListItem>                                                   
                                            </asp:DropDownList>
                                        </div>
                                   </div>
                                     <div class="row">
                                        <div class="form-group col-md-3" id="div1" runat="server" style="display: block;">

                                            <label for="exampleInputEmail1">User:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlUser" Width="100%" runat="server" class="form-control">
                                                <asp:ListItem Text="Distributor" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Employee" Value="2"></asp:ListItem>   
                                            </asp:DropDownList>

                                        </div>                                        
                                        <div class="form-group col-md-4">
                                            <label for="exampleInputEmail1" style="display: block; visibility: hidden">GoBtn</label>

                                            <asp:Button ID="btnGo" class="btn btn-primary" runat="server" Style="padding: 3px 14px;" Text="GO"
                                                OnClientClick="return Validate();" OnClick="btnGo_Click" />
                                        </div>

                                    </div>
                                </div>
                                <div class="clearfix"></div>
                                <div class="box-body table-responsive" id="mainDiv" style="display: none;" runat="server">
                                    <%--<asp:GridView ID="gvPartyData" runat="server" AutoGenerateColumns="False" EmptyDataText="No Records Found"
                                        CssClass="tbl" PageSize="100" Width="100%" OnRowDataBound="gvPartyData_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkHeader" runat="server" onclick="SelectAllByRow(this, 0)" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkItem" runat="server" />
                                                    <asp:HiddenField ID="areaIdHiddenField" runat="server" Value='<%#Eval("areaId") %>' />                                                  
                                                </ItemTemplate>
                                                <ItemStyle Width="30px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" HeaderText="State">
                                                <ItemTemplate>
                                                    <asp:Label ID="stateLabel" runat="server" Text='<%#Eval("stateName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" HeaderText="District">
                                                <ItemTemplate>
                                                    <asp:Label ID="districtLabel" runat="server" Text='<%#Eval("districtName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" HeaderText="City">
                                                <ItemTemplate>
                                                    <asp:Label ID="cityLabel" runat="server" Text='<%#Eval("cityName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" HeaderText="Area">
                                                <ItemTemplate>
                                                    <asp:Label ID="areaLabel" runat="server" Text='<%#Eval("areaName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="70px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="AreaName" HeaderText="Area" ItemStyle-Wrap="false" ItemStyle-Width="70px" />
                                        </Columns>
                                        <FooterStyle BackColor="#EED690" />
                                        <EmptyDataRowStyle BackColor="#3c8dbc" ForeColor="White" />
                                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" BackColor="#3c8dbc" />
                                        <SelectedRowStyle BackColor="#008A8C" HorizontalAlign="Center" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#3c8dbc" HorizontalAlign="Right" Font-Bold="True" ForeColor="white" />
                                        <AlternatingRowStyle BackColor="#e8e8e8" />
                                        <AlternatingRowStyle CssClass="tbl1" />
                                    </asp:GridView>--%>

                                    <asp:Repeater ID="rpt" runat="server">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                       <th style="text-align: center; width: 6%">S.No</th>                                                     
                                                        <th>Name</th>
                                                        <th>Mobile</th>
                                                        <th>Login Count</th>                                                    
                                                        
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>     
                                                <td style="text-align: center; width: 6%"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>                                                                                     
                                                <td><%#Eval("name") %></td>
                                                <td><%#Eval("Mobile") %></td>
                                                <td><%#Eval("LoginCount") %></td>                                               
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>     
                                            </table>       
                                        </FooterTemplate>

                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>

                        <div class="box-footer">                          
                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>

            </div>
        </div>
         <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(function () {
            $("#example1").DataTable({
                "paging": false,                
                "order": [[3, "desc"]]

            });
        });

    </script>
    </section>
</asp:Content>
