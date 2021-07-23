<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="CompOtherActivity.aspx.cs" Inherits="AstralFFMS.CompOtherActivity" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content">
         <div class="box-body" id="rptmain" runat="server" style="display: block;">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Competitor's other Activity List</h3>                            
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Branding Activity</th>
                                                <th>Meet Activity</th>                                                
                                                <th>Road Show</th>
                                                <th>Scheme/offers</th>
                                                <th>Other General Info</th>                                               
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>                                   
                                        
                                        <td><%#Eval("BrandActivity") %></td>
                                        <td><%#Eval("MeetActivity") %></td>
                                        <td><%#Eval("RoadShow") %></td>
                                        <td><%#Eval("Scheme/offers") %></td>
                                        <td><%#Eval("OtherGeneralInfo") %></td>                                       
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
        </section>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>

   
    </asp:Content>