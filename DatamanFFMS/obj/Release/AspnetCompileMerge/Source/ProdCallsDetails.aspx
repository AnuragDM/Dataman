<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ProdCallsDetails.aspx.cs" Inherits="AstralFFMS.Prod_Calls_Details" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>

    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
                "order": [[1, "desc"]]
            });
        });
    </script>
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
                                <h3 class="box-title">Prod Calls Details</h3>
                           
                            </div>
                          <%--<div class="clearfix"></div>
                                    <div class="row">
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div id="DIV1" class="form-group" style="margin-left: 8%;">
                                                <label for="exampleInputEmail1">From Date:</label>
                                                <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" onChange="showspinner();" AutoPostBack="true"  Style="background-color: white;" OnTextChanged="txtfmDate_TextChanged"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-md-1"></div>
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">To:</label>
                                                <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" onChange="showspinner();" AutoPostBack="true"  Style="background-color: white;" OnTextChanged="txttodate_TextChanged"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                            </div>
                                        </div>--%>

                                   
                           <div class="col-md-6 col-sm-6 col-xs-12">
                                
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Report For Prod Calls:</label>                                                
                                            <asp:Label ID="lbltotalparty" runat="server" Text='<%# Eval("SMName")%>'></asp:Label>
                                        </div>
                                        <%-- </div>--%>
                                
                                     <div class="row">
                                   
                                        <div class="col-md-5 col-sm-12 col-xs-12">
                                            <div id="DIV1" class="form-group">
                                                <label for="exampleInputEmail1">From Date:</label>
                                                <asp:Label ID="lblformDate" runat="server" Text=""></asp:Label>
                                            
                                            </div>
                                        </div>
                                        
                                        <div class="col-md-5 col-sm-12 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">To Date:</label>
                                                 <asp:Label ID="lbltdate" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                       
                                 
                               </div>
                             
                                </div>
                           
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">
                                <div class="col-md-3 col-sm-5">
                                    <div class="form-group">
                                        
                                    </div>
                                </div></div>
                            </div>
                          
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="box-body" id="rptmain" runat="server" style="display: block;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <div style="float: right">
                          

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">

                         <asp:Repeater ID="Topprodcalls" runat="server">
                                <HeaderTemplate>
                                    <div class="box-footer">
                                    
                                   <asp:Button  type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="btnExport"/>
                                  </div>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>

                                           <tr>  
                                                   <%-- <th style="text-align: left; width: 5%">S.No</th> --%>
                                                   <%-- <th style="text-align: left; width: 14%">PartyName</th>
                                                    <th style="text-align: right; width: 10%">Quantity</th>
                                                   <th style="text-align: right; width: 8%">Rate</th>
                                                    <th style="text-align: right; width: 8%">Amount</th>--%>
                                                  <th>Retailer</th>
                                                    <th>Address</th>
                                                    <th>Mobile</th>
                                                    <th>Visited Sales Person</th>
                                                   
                                                </tr>
                                            </thead>
                                             
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                          
                                            <tr> 
                                           
                                             
                                          <%--  <td><%# Container.ItemIndex + 1 %></td>        --%>                    
                                            <%--<td><%# Eval("PartyName")%>
                                            <asp:Label ID="lblProductparty" runat="server" Visible="false" Text='<%# Eval("PartyName")%>'></asp:Label></td>
                                            <td style="text-align: right"><%# Eval("qty")%>
                                            <asp:Label ID="lblqty" runat="server" Visible="false" Text='<%# Eval("qty")%>'></asp:Label></td>                                 <td style="text-align: right"><%# Eval("rate")%>
                                            <asp:Label ID="lblrate" runat="server" Visible="false" Text='<%# Eval("rate")%>'></asp:Label></td>                
                                            <td style="text-align: right"><%# Eval("amount")%>
                                            <asp:Label ID="lblamount" runat="server" Visible="false" Text='<%# Eval("amount")%>'></asp:Label></td>  --%>
                                              
                                                <%--  --%>
                                                <td><%# Eval("PartyName")%>
                                                <asp:Label ID="lblName" runat="server" Visible="false" Text='<%# Eval("PartyName")%>'></asp:Label> </td>      
                                                <td><%# Eval("Address1")%>
                                                <asp:Label ID="lblAddress" runat="server" Visible="false" Text='<%# Eval("Address1")%>'></asp:Label></td>   
                                                <td><%# Eval("Mobile")%>
                                                <asp:Label ID="lblMobile" runat="server" Visible="false" Text='<%# Eval("Mobile")%>'></asp:Label></td>    
                                                <td><%# Eval("SMName")%>
                                                <asp:Label ID="lblvisitedperson" runat="server" Visible="false" Text='<%# Eval("SMName")%>'></asp:Label></td>    
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
   
    <script type="text/javascript">
        function showspinner() {

            $("#spinner").show();

        };
        function hidespinner() {

            $("#spinner").hide();

        };

     </script> 
</asp:Content>




