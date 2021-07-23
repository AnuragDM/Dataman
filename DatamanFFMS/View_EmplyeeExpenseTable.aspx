<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="View_EmplyeeExpenseTable.aspx.cs" Inherits="AstralFFMS.View_EmplyeeExpenseTable" EnableEventValidation="false" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style type="text/css">
        .spinner {
            position: absolute;
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
        margin-top:-8px !important;
        }
    </style>
    <style>
        .alignright12{
       text-align:right!important;
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
        <div class="box-body" id="mainDiv"  runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <%--<h3 class="box-title">Welcome to Astral's Employee Self Service Module! </h3>--%>   
                             <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>                         
                       </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                            <div class="col-lg-5 col-md-8 col-sm-8 col-xs-10">
                                  <div class="form-group">
                                <input id="AreaId" hidden="hidden" />
                             <%--   <label for="exampleInputEmail1">Salesman:</label>--%>
                                  <asp:DropDownList ID="ddlsalesman" runat="server"  Visible ="false" width="100%" class="form-control" TabIndex="3"></asp:DropDownList>
                            </div>
                                   <div class="form-group">
                             <asp:Panel ID="pnlloadgibgboarding" runat="server">
                                     <label> Lodging & Boarding Limit </label>
                              
                                 <asp:GridView ID="gdvloadgibgboarding" runat="server" AutoGenerateColumns="False"
                            Width="100%" HeaderStyle-BackColor="#99ccff" EmptyDataRowStyle-BackColor="#99ccff" EmptyDataText ="No Record Found">
                                      <HeaderStyle  HorizontalAlign="Right" />
                                     <Columns>
                                         <asp:BoundField DataField="Name" HeaderText="City Type" />
                                         <asp:BoundField DataField="DesName" HeaderText="Designation" />
                                         <asp:BoundField DataField="Amount" HeaderText="Amount" HeaderStyle-CssClass="alignright12"  ItemStyle-HorizontalAlign="Right" />
                                       
                                       <%--  <asp:BoundField DataField="Remarks" HeaderText="Remarks" />--%>
                                     </Columns>
                                       
                                 </asp:GridView>
                             </asp:Panel>
                            </div>
                             <div class="form-group">
                             <asp:Panel ID="pnlLocalConveyanceLimt" runat="server">
                                     <label> Head Quarter (Local) Conveyance </label>
                              
                                 <asp:GridView ID="gdvLocalConveyanceLimt" runat="server" AutoGenerateColumns="False"
                            Width="100%" HeaderStyle-BackColor="#99ccff" EmptyDataRowStyle-BackColor="#99ccff" EmptyDataText ="No Record Found">
                                     <Columns>
                                         <asp:BoundField DataField="Name" HeaderText="City Type" />
                                         <asp:BoundField DataField="DesName" HeaderText="Designation" />
                                         <asp:BoundField DataField="Amount" HeaderText="Amount" HeaderStyle-CssClass="alignright12" ItemStyle-HorizontalAlign="Right"  />
                                       <%--  <asp:BoundField DataField="Remarks" HeaderText="Remarks" />--%>
                                     </Columns>

                                 </asp:GridView>
                             </asp:Panel>
                            </div>
                                  <div class="form-group">
                             <asp:Panel ID="pnlModeofTravel" runat="server">
                                   <%--  <label>Mode of Travel</label>--%>
                              
                                 <asp:GridView ID="gdvModeofTravel" runat="server" AutoGenerateColumns="False" EmptyDataText ="No Record Found" EmptyDataRowStyle-BackColor="#99ccff"
                            Width="100%" HeaderStyle-BackColor="#99ccff">
                                     <Columns>
                                         <asp:BoundField DataField="Name" HeaderText="Travel Mode" />
                                        <%-- <asp:BoundField DataField="PerKmRate" HeaderText="Per Km" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />--%>
                                     <%--    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />--%>
                                     </Columns>

                                 </asp:GridView>
                             </asp:Panel>
                            </div>
                                       
                                 <div class="form-group">
                             <asp:Panel ID="pnlConvMode" runat="server" >
                                    <%-- <label>Conveyance Mode</label>--%>
                              
                                 <asp:GridView ID="gdvConvMode" runat="server" AutoGenerateColumns="False"
                            Width="100%" HeaderStyle-BackColor="#99ccff" EmptyDataRowStyle-BackColor="#99ccff" EmptyDataText ="No Record Found">
                                     <Columns>
                                         <asp:BoundField DataField="Name" HeaderText="Conveyance Mode" />
                                          <asp:BoundField DataField="PerKmRate" HeaderText="Per Km" HeaderStyle-CssClass="alignright12" ItemStyle-HorizontalAlign="Right" />
                                     </Columns>

                                 </asp:GridView>
                             </asp:Panel>
                            </div>
                                 
                    
                                   
                     <div class="form-group">
                             <asp:Panel ID="pnlCityConveyanceType" runat="server"   Visible="false" >
                                     <label>City Conveyance Type</label>
                              
                                 <asp:GridView ID="gdvCityConveyanceType" runat="server" AutoGenerateColumns="False"
                            Width="100%" HeaderStyle-BackColor="#99ccff" EmptyDataRowStyle-BackColor="#99ccff" EmptyDataText ="No Record Found">
                                     <Columns>
                                         <asp:BoundField DataField="Name" HeaderText="City Conveyance Type" />
                                     </Columns>

                                 </asp:GridView>
                             </asp:Panel>
                            </div>
                                   <div class="form-group">
                             <asp:Panel ID="pnlmetrocity" runat="server" >
                                     <label>Metro City</label>
                              
                                 <asp:GridView ID="gdvmetrocity" runat="server" AutoGenerateColumns="False"
                            Width="100%" HeaderStyle-BackColor="#99ccff" EmptyDataRowStyle-BackColor="#99ccff" EmptyDataText ="No Record Found">
                                     <Columns>
                                         <asp:BoundField DataField="StateName" HeaderText="State" />
                                         <asp:BoundField DataField="CityName" HeaderText="City" />
                                     </Columns>

                                 </asp:GridView>
                             </asp:Panel>
                            </div>
                                   <div class="form-group">
                             <asp:Panel ID="pnlAcity" runat="server" >
                                     <label>City A</label>
                              
                                 <asp:GridView ID="gdvAcity" runat="server" AutoGenerateColumns="False"
                            Width="100%" HeaderStyle-BackColor="#99ccff" EmptyDataRowStyle-BackColor="#99ccff" EmptyDataText ="No Record Found">
                                     <Columns>
                                         <asp:BoundField DataField="StateName" HeaderText="State" />
                                         <asp:BoundField DataField="CityName" HeaderText="City" />
                                     </Columns>

                                 </asp:GridView>
                             </asp:Panel>
                            </div>
                                   <div class="form-group">
                             <asp:Panel ID="pnlBCity" runat="server"  >
                                     <label>City B</label>
                              
                                 <asp:GridView ID="gdvBcity" runat="server" AutoGenerateColumns="False"
                            Width="100%" HeaderStyle-BackColor="#99ccff" EmptyDataRowStyle-BackColor="#99ccff" EmptyDataText ="No Record Found">
                                     <Columns>
                                         <asp:BoundField DataField="StateName" HeaderText="State" />
                                         <asp:BoundField DataField="CityName" HeaderText="City" />
                                     </Columns>

                                 </asp:GridView>
                             </asp:Panel>
                            </div>
                            </div></div>
                        </div>
                      </div>
                    </div>
                </div>


            </div>

        

        

    </section>
    
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();

        });
    </script>
</asp:Content>
