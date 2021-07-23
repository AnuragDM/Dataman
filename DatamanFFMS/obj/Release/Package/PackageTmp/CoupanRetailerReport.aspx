<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/FFMS.Master" CodeBehind="CoupanRetailerReport.aspx.cs" Inherits="AstralFFMS.CoupanRetailerReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style>
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .table-responsive {
            border: 1px solid #fff;
        }
    </style>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }
    </style>
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
     <script type="text/javascript">
         function validate() {

             if ($('#<%=ddlScheme.ClientID%>').val() == "0") {
                 errormessage("Please select Coupon Scheme.");
                 return false;
             }
         }

    </script>    

    <section class="content">
         <div id="messageNotification">
            <asp:Label ID="lblmasg" runat="server"></asp:Label>
        </div>
        <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Coupon Retailer Report</h3>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <div class="col-lg-12 col-md-12 col-sm-9 col-xs-11">                               
                                <div class="col-md-12 paddingleft0">
                                    <div class="form-group col-md-3">
                                        <label for="exampleInputEmail1">Scheme:</label>
                                        <asp:DropDownList ID="ddlScheme" Width="100%" runat="server" CssClass="form-control">                                           
                                        </asp:DropDownList>

                                    </div>
                                    <div class="form-group col-md-3">
                                        <label for="exampleInputEmail1">Zone:</label>
                                        <asp:DropDownList ID="ddlZone" Width="100%" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged">
                                        </asp:DropDownList>

                                    </div>                                                                   
                                </div>
                                <div class="col-md-12 paddingleft0">
                                    <div class="form-group col-md-3">
                                        <label for="exampleInputEmail1">Prefix:</label>
                                        <asp:DropDownList ID="ddlPrefix" Width="100%" runat="server" CssClass="form-control">
                                        </asp:DropDownList>                                   
                                    </div>
                                     <div class="form-group col-md-3">
                                        <label for="exampleInputEmail1">Distributor:</label>
                                        <asp:DropDownList ID="ddlDistributor" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>                                                                        
                                    </div>                                                                  
                                </div>
                                <div class="col-md-12 paddingleft0">  
                                <div class="form-group col-md-3">
                                           <label for="withSales">Issue Coupon(In Range):</label>
                                           <asp:TextBox ID="txtstartCoupon"  CssClass="form-control numeric text-right" runat="server" placeholder="Start Coupon No."></asp:TextBox>
                                 </div>
                                 <div class="form-group col-md-3">  
                                         <label for="withSales">Issue Coupon(In Range):</label>                                        
                                         <asp:TextBox ID="txtEndCoupon"  CssClass="form-control numeric text-right" runat="server" placeholder="End Coupon No."></asp:TextBox>
                                 </div>
                              </div>
                            </div>
                            <div class="box-footer">
                                <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                        <asp:Button type="button" ID="btnGo" runat="server" Style="padding: 3px 14px;" Text="Search" class="btn btn-primary go-button-dsr" OnClick="btnGo_Click" OnClientClick="javascript:return validate();"  />
                                        <asp:Button ID="btnexport" class="btn btn-primary" Style="padding: 3px 14px;" runat="server" Text="Export" Visible="false" OnClick="btnexport_Click" />
                                </div>    
                             <div class="box-body table-responsive">
                            <asp:Repeater ID="Repeater2" runat="server" OnItemCommand="Repeater2_ItemCommand">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>                                             
                                                <th>Distributor Name</th>
                                                <th>Mobile No.</th>                                                  
                                                <th>Area</th>    
                                                <th>Available Coupons</th>   
                                                                                       
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                      <td><%#Eval("Distributor") %></td>
                                          <td><%#Eval("mobile") %></td>
                                          <td><%#Eval("areaname") %></td>                                        
                                        <%--  <td><%#Eval("IssuedCoupan") %></td>  --%>                                   
                                        <td><asp:LinkButton ID="lnkedit"  runat="server" Text='<%#Eval("AvailableCoupons") %>'  CommandName="coupanAvailable" CommandArgument='<%#Eval("distributorid")%>'></asp:LinkButton></td>                      
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>               
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>                                             
                                                <th>Retailer Name</th>
                                                <th>Mobile No.</th>                                                  
                                                <th>Area</th>    
                                                <th>Issued Coupons</th>   
                                                                                       
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                      <td><%#Eval("RetailerName") %></td>
                                          <td><%#Eval("MobileNo") %></td>
                                          <td><%#Eval("Area") %></td>                                        
                                        <%--  <td><%#Eval("IssuedCoupan") %></td>  --%>                                   
                                        <td><asp:LinkButton ID="lnkedit"  runat="server" Text='<%#Eval("IssuedCoupan") %>'  CommandName="coupanissued" CommandArgument='<%#Eval("Id")%>'></asp:LinkButton></td>                      
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>

                        <div class="box-header">
                            <h3 class="box-title"></h3>
                        </div>

                        <div class="col-md-12">

                            <div class="form-group">
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="Repeater1" runat="server">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <th>Sr.No.</th>
                                                        <th>Retailername</th>
                                                        <th>Coupon No.</th>
                                                        <th>Issue Date</th>                                                       
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Container.ItemIndex+1 %>                                          
                                                </td>                                              
                                                <td><%#Eval("RetailerName") %></td>
                                                <td><%#Eval("coupanNoWithPrefix") %></td>                                               
                                                <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("retailerissuedate"))%></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>     </table>       
                                        </FooterTemplate>

                                    </asp:Repeater>
                                </div>

                            </div>

                            <div class="form-group">
                            </div>
                        </div>
                                   <div class="col-md-12">

                            <div class="form-group">
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="Repeater3" runat="server">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <th>Sr.No.</th>
                                                        <th>Retailername</th>
                                                        <th>Coupon No.</th>
                                                        <th>Issue Date</th>                                                       
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Container.ItemIndex+1 %>                                          
                                                </td>                                              
                                                <td><%#Eval("RetailerName") %></td>
                                                <td><%#Eval("coupanNoWithPrefix") %></td>                                               
                                                <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("retailerissuedate"))%></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>     </table>       
                                        </FooterTemplate>

                                    </asp:Repeater>
                                </div>

                            </div>

                            <div class="form-group">
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <div class="box-header">
                            <h3 class="box-title">
                                <asp:Label ID="lblnouser" runat="server" Text="No of Users"></asp:Label></h3>
                            <br />
                             <asp:Button ID="btnNOUExport" class="btn btn-primary" Style="padding: 3px 14px;" runat="server" Text="Export" OnClick="btnNouexport_Click" />
                        </div>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rptnoofusers" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Sr.No.</th>
                                                <th>Sales Person</th>
                                                <th>Meet Date</th>
                                                <th>Meet</th>
                                                <th>Area</th>
                                                <th>Beat</th>
                                                <th>Party Name</th>
                                                <th>Contact Person</th>
                                                <th>Mobile No</th>
                                                <th>EmailId</th>
                                                <th>Potential</th>
                                                <th>DOB</th>
                                                <th>Address</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>  <td><%#Container.ItemIndex+1 %>                             
                                        </td>
                                        <td><%#Eval("SMName")%></td>
                                                  <asp:Label ID="SMName" runat="server" Visible="false" Text='<%# Eval("SMName")%>'></asp:Label>
                                                  <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("MeetDate"))%></td>
                                                  <asp:Label ID="MeetDate" runat="server" Visible="false" Text='<%# Eval("MeetDate")%>'></asp:Label>
                                                  <td><%#Eval("MeetName")%></td>
                                                  <asp:Label ID="MeetName" runat="server" Visible="false" Text='<%# Eval("MeetName")%>'></asp:Label>
                                                  <td><%#Eval("AreaName")%></td>
                                                  <asp:Label ID="AreaName" runat="server" Visible="false" Text='<%# Eval("AreaName")%>'></asp:Label>
                                                  <td><%#Eval("BeatName")%></td>
                                                  <asp:Label ID="BeatName" runat="server" Visible="false" Text='<%# Eval("BeatName")%>'></asp:Label>
                                                  <td><%#Eval("Name")%></td>
                                                  <asp:Label ID="Name" runat="server" Visible="false" Text='<%# Eval("Name")%>'></asp:Label>
                                                  <td><%#Eval("ContactPersonName")%></td>
                                                  <asp:Label ID="ContactPersonName" runat="server" Visible="false" Text='<%# Eval("ContactPersonName")%>'></asp:Label>
                                                <td><%#Eval("MobileNo")%></td>
                                                     <asp:Label ID="MobileNo" runat="server" Visible="false" Text='<%# Eval("MobileNo")%>'></asp:Label>
                                                <td><%#Eval("EmailId")%></td>
                                                     <asp:Label ID="EmailId" runat="server" Visible="false" Text='<%# Eval("EmailId")%>'></asp:Label>
                                                  <td><%#Eval("Potential")%></td>
                                                     <asp:Label ID="Potential" runat="server" Visible="false" Text='<%# Eval("Potential")%>'></asp:Label>
                                                <td><%#Eval("DOB")%></td>
                                                     <asp:Label ID="DOB" runat="server" Visible="false" Text='<%# Eval("DOB")%>'></asp:Label>
                                                <td><%#Eval("Address")%></td>
                                                     <asp:Label ID="Address" runat="server" Visible="false" Text='<%# Eval("Address")%>'></asp:Label>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>            
                      </div>
                        <!-- /.box-body -->
                    </div>
                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>

        </div>
    </section>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
                //"order": [[1, "desc"]]
            });

        });
    </script>
</asp:Content>
