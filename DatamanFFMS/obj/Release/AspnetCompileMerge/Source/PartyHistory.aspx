<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="PartyHistory.aspx.cs" Inherits="AstralFFMS.PartyHistory" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    <style>
        .locked-unlocked {
            font-size: 1.32em;
            font-weight: 600;
            color: #585858;
    </style>

    <section class="content">
       
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                      <div class="box-header with-border">
                           <div class="form-group">
                                <%--<asp:Label ID="Label1" runat="server" Text="Label" Visible="false"></asp:Label>--%>
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                <div class="col-sm-12">
                                    <div class="col-lg-4 col-md-5 col-sm-8 paddingleft0">
                                   <asp:Label ID="partyName" runat="server" CssClass="text"  Text="Label"></asp:Label></br>
                                         <asp:Label ID="address" runat="server" CssClass="text"  Text="Label"></asp:Label>,&nbsp;
                                     <asp:Label ID="lblzipcode" runat="server" CssClass="text"  Text=""></asp:Label>,&nbsp;
                                     <asp:Label ID="mobile" runat="server" CssClass="text"  Text="Label"></asp:Label>&nbsp;
                                        </div>
                                       <div class="col-lg-2 col-md-2 col-sm-8 paddingleft0">
                                             <asp:Label ID="lblVisitdate1" runat="server" CssClass="text" Text="Visit Date"></asp:Label></br>
                                           <asp:Label ID="lblVisitDate5" runat="server" CssClass="text" Text="Visit Date"></asp:Label>&nbsp;
                                                </div>
                                    <div class="col-lg-2 col-md-2 col-sm-8 paddingleft0">
                                             <asp:Label ID="lblAreaName1" runat="server" CssClass="text" Text="Area Name"></asp:Label></br>
                                        <asp:Label ID="lblBeatName5" runat="server" CssClass="text" Text="Beat Name"></asp:Label>&nbsp;
                                                </div>
                                     <div class="col-lg-2 col-md-2 col-sm-8 paddingleft0">
                                             <asp:Label ID="lblBeatName1" runat="server" CssClass="text" Text="Beat Name"></asp:Label></br>
                                         <asp:Label ID="lblAreaName5" runat="server" CssClass="text" Text="Area Name"></asp:Label>&nbsp;
                                                </div>
                                     
                                    <div class="col-lg-2 col-md-1 col-sm-8 paddingleft0" style="float: right">
                                      
                           <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                             <asp:HiddenField id="hid" runat="server"/>
                            </div>
                                </div>
                            </div>
                           
                           
                                  <div class="form-group">
                                <div class="col-sm-12">
                                      <div class="col-sm-5 paddingleft0">
                                    <%-- <input id="address" class="form-control" type="text" readonly="readonly">--%>
                                    <%--<asp:TextBox ID="address" runat="server" ReadOnly="true"></asp:TextBox>--%>
                                   
                                          </div>
                                       <div class="col-sm-2 paddingleft0">
                                             
                                                </div>
                                     <div class="col-sm-2 paddingleft0">
                                             
                                                </div>
                                      <div class="col-sm-2 paddingleft0">
                                             
                                                </div>
                                     
                                </div>
                            </div>
                        </div>
                         <div class="box-header with-border">
                            <div class="form-group paddingleft0">
                                <div>
                                      <h3>Party History</h3>
                                </div>
                                </div>

                              <div class="form-group paddingleft0">

                        <div class="locked-unlocked">Unlocked Entries</div>
                            </div>
                        <div class="box-header">
                            
                        <h3 class="box-title">Order History</h3>
                        <div class="box-body table-responsive">
                         <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Document Date</th>
                                                <th>Document No.</th>
                                                <th>Amount</th>
                                                <th>Remark</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("OrdId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("OrdId") %>' />
                                         <td> <%#String.Format("{0:dd/MMM/yyyy}", Eval("VDate"))%></td>
                                          <td><%#Eval("OrdDocId") %></td>
                                        <td style="text-align:right;"><%#Eval("OrderAmount") %></td>
                                        <td><%#Eval("Remarks") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                            </div>
                            </div>

                                <div class="box-header">
                     <h3 class="box-title">Demo History</h3>
                        <div class="box-body table-responsive">
                             
                                    
                            <asp:Repeater ID="rptDemo" runat="server">
                              <HeaderTemplate>
                                    <table id="example2" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Document Date</th>
                                                <th>Document No.</th>
                                                <th>Product Class</th>
                                                <th>Product Segment</th>
                                                <th>Product Group</th>
                                                <th>Remark</th>
                                                <th>Image</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("DemoId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("DemoId") %>' />
                                        <asp:HiddenField ID="linkHiddenField" runat="server" Value='<%#Eval("ImgURL") %>' />
                                              <td> <%#String.Format("{0:dd/MMM/yyyy}", Eval("VDate"))%></td>
                                          <td><%#Eval("DemoDocId") %></td>
                                          <td><%#Eval("Classname") %></td>
                                         <td><%#Eval("SegmentName") %></td>
                                           <td><%#Eval("GroupName") %></td>
                                        <td><%#Eval("Remarks") %></td>
                                         <td><asp:LinkButton ID="lnkViewDemoImg" runat="server" OnClick="lnkViewDemoImg_Click" >View Image</asp:LinkButton></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                                     </div>
                                    </div>


                              <div class="box-header" style="display:none;">
                        <h3 class="box-title">Collection History</h3>
                        <div class="box-body table-responsive" >
                            <asp:Repeater ID="rptCollection" runat="server">
                                <HeaderTemplate>
                                    <table id="example3" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Document Date</th>
                                                <th>Document No.</th>
                                                <th>Name</th>
                                                <th>Amount</th>
                                                <th>Mode</th>
                                                <th>Instrument No</th>
                                                <th>Instrument Date</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("CollId") %>' />
                                        <td><%#String.Format("{0:dd/MMM/yyyy}",Eval("PaymentDate")) %></td>
                                         <td><%#Eval("CollDocId") %></td>
                                        <td><%#Eval("PartyName") %></td>
                                        <td style="text-align:right;"><%#Eval("Amount") %></td>
                                        <td><%#Eval("Mode") %></td>
                                       <td><%#Eval("Cheque_DDNo") %></td>
                                        <td> <%#String.Format("{0:dd/MMM/yyyy}", Eval("Cheque_DD_Date"))%></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                                  </div>


                              <div class="box-header">
                        <h3 class="box-title">Failed Visit History</h3>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rptFailedVisit" runat="server">
                                <HeaderTemplate>
                                    <table id="example4" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                 <th>Next Visit Date</th>
                                                <th>Document No.</th>
                                                 <th>Remark</th>
                                                 <th>Reason</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("FVId") %>' />
                                        <td><%#String.Format("{0:dd/MMM/yyyy}",Eval("Nextvisit")) %></td>
                                          <td><%#Eval("FVDocId") %></td>
                                          <td><%#Eval("Remarks") %></td>
                                        <td><%#Eval("Reason") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                                  </div>

                              <div class="box-header">
                        <h3 class="box-title">Competitor  History</h3>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rptCompetitor" runat="server">
                                <HeaderTemplate>
                                    <table id="example19" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Visit Date</th>
                                                <th>Competitor</th>
                                                <th>Document No.</th>
                                                <th>Item</th>
                                                <th>Std. Packing</th>
                                                <th>Rate</th>
                                                <th style="text-align:right">Discount</th>
                                                <th>Remark</th>
                                                <th>Image</th>
                                                <th>Other Activity</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr">
                                         <asp:HiddenField ID="linkHdFComp" runat="server" Value='<%#Eval("ImgURL") %>' />
                                         <td><%#String.Format("{0:dd/MMM/yyyy}",Eval("VDate")) %></td>
                                          <td><%#Eval("CompName") %></td>
                                          <td><%#Eval("DocId") %></td>
                                          <td><%#Eval("Item") %></td>
                                          <td style="text-align:right;"><%#Eval("Qty") %></td>
                                          <td style="text-align:right;"><%#Eval("Rate") %></td>
                                          <td style="text-align:right;"><%#Eval("Discount") %></td>
                                          <td><%#Eval("Remarks") %></td>
                                          <td><asp:LinkButton ID="lnkViewCompImg" runat="server" OnClick="lnkViewCompImg_Click" >View Image</asp:LinkButton></td>
                                         <%--<td><%#Eval("OtherActivity") %></td>--%>
                                        <%--<td><asp:LinkButton CommandName="select" ID="lnkEdit" CausesValidation="False" runat="server" Text='<%#Eval("OtherActivity") %>' Width="80px" 
                                         OnClientClick="window.document.forms[0].target='_blank'; setTimeout(function(){window.document.forms[0].target='';}, 500);" PostBackUrl="~/RptDailyWorkingSummaryL1.aspx" /></td>--%>
                                        <td><asp:HyperLink ID="hpl" runat="server" NavigateUrl='<%# Eval("ComptId", "~/CompOtherActivity.aspx?ComptId={0}") %>' Text='<%#Eval("OtherActivity") %>' Target="_blank"></asp:HyperLink></td>
                                          
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                                  </div>


                        </div>
                        <div class="box-header">
                        <div class="form-group paddingleft0">

                        <div class="locked-unlocked">Locked Entries</div>
                            </div></div>

                           <div class="box-header">
                        <h3 class="box-title">Order History</h3>
                        <div class="box-body table-responsive">
                         <asp:Repeater ID="Repeater1" runat="server">
                                <HeaderTemplate>
                                    <table id="example11" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Document Date</th>
                                                <th>Document No.</th>
                                                <th>Amount</th>
                                                <th>Remark</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("OrdId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("OrdId") %>' />
                                         <td> <%#String.Format("{0:dd/MMM/yyyy}", Eval("VDate"))%></td>
                                          <td><%#Eval("OrdDocId") %></td>
                                        <td style="text-align:right;"><%#Eval("OrderAmount") %></td>
                                        <td><%#Eval("Remarks") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                            </div>
                            </div>

                                <div class="box-header">
                     <h3 class="box-title">Demo History</h3>
                        <div class="box-body table-responsive">
                             
                                    
                            <asp:Repeater ID="Repeater2" runat="server">
                              <HeaderTemplate>
                                    <table id="example12" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Document Date</th>
                                                <th>Document No.</th>
                                                <th>Product Class</th>
                                                <th>Product Segment</th>
                                                 <th>Product Group</th>
                                                <th>Remark</th>
                                                <th>Image</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("DemoId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("DemoId") %>' />
                                          <asp:HiddenField ID="linkLockHdF" runat="server" Value='<%#Eval("ImgURL") %>' />
                                          <td> <%#String.Format("{0:dd/MMM/yyyy}", Eval("VDate"))%></td>
                                          <td><%#Eval("DemoDocId") %></td>
                                          <td><%#Eval("Classname") %></td>
                                         <td><%#Eval("SegmentName") %></td>
                                         <td><%#Eval("GroupName") %></td>
                                        <td><%#Eval("Remarks") %></td>
                                          <td><asp:LinkButton ID="lnkViewDemoLockImg" runat="server" OnClick="lnkViewDemoLockImg_Click" >View Image</asp:LinkButton></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                                     </div>
                                    </div>


                              <div class="box-header" style="display:none;">
                        <h3 class="box-title">Collection History</h3>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="Repeater3" runat="server">
                                <HeaderTemplate>
                                    <table id="example13" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Document Date</th>
                                                <th>Document No.</th>
                                                <th>Name</th>
                                                <th>Amount</th>
                                                <th>Mode</th>
                                                <th>Instrument No</th>
                                                <th>Instrument Date</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("CollId") %>' />
                                        <td><%#String.Format("{0:dd/MMM/yyyy}",Eval("PaymentDate")) %></td>
                                         <td><%#Eval("CollDocId") %></td>
                                        <td><%#Eval("PartyName") %></td>
                                        <td style="text-align:right;"><%#Eval("Amount") %></td>
                                        <td><%#Eval("Mode") %></td>
                                       <td><%#Eval("Cheque_DDNo") %></td>
                                        <td> <%#String.Format("{0:dd/MMM/yyyy}", Eval("Cheque_DD_Date"))%></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                                  </div>


                              <div class="box-header">
                        <h3 class="box-title">Failed Visit History</h3>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="Repeater4" runat="server">
                                <HeaderTemplate>
                                    <table id="example14" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                  <th>Next Visit Date</th>
                                                <th>Document No.</th>
                                              
                                                 <th>Remark</th>
                                                 <th>Reason</th>
                                            
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("FVId") %>' />
                                      <td><%#String.Format("{0:dd/MMM/yyyy}",Eval("Nextvisit")) %></td>
                                          <td><%#Eval("FVDocId") %></td>
                                          
                                          <td><%#Eval("Remarks") %></td>
                                         <td><%#Eval("Reason") %></td>
                                        
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                                  </div>
                        

                              <div class="box-header">
                        <h3 class="box-title">Competitor  History</h3>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="Repeater5" runat="server">
                                <HeaderTemplate>
                                    <table id="example110" class="table table-bordered table-striped">
                                         <thead>
                                            <tr>
                                                <th>Comp ID</th>
                                                <th>Visit Date</th>
                                                <th>Competitor</th>
                                                <th>Document No.</th>
                                                <th>Item</th>
                                                <th>Std. Packing</th>
                                                <th>Rate</th>
                                                <th style="text-align:right">Discount</th>
                                                <th>Remark</th>
                                                <th>Image</th>
                                                <th>Other Activity</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                         <asp:HiddenField ID="linkHdFLockComp" runat="server" Value='<%#Eval("ImgURL") %>' />
                                        <td><%#Eval("ComptId") %></td>                                         
                                         <td><%#String.Format("{0:dd/MMM/yyyy}",Eval("VDate")) %></td>
                                          <td><%#Eval("CompName") %></td>
                                          <td><%#Eval("DocId") %></td>
                                          <td><%#Eval("Item") %></td>
                                          <td style="text-align:right;"><%#Eval("Qty") %></td>
                                          <td style="text-align:right;"><%#Eval("Rate") %></td>
                                         <td style="text-align:right;"><%#Eval("Discount") %></td>
                                         <td><%#Eval("Remarks") %></td>
                                         <td><asp:LinkButton ID="lnkViewDemoCompLockImg" runat="server" OnClick="lnkViewDemoCompLockImg_Click" >View Image</asp:LinkButton></td>
                                       <%-- <td><%#Eval("OtherActivity") %></td>--%>
                                       <%-- <td><asp:LinkButton CommandName="select" ID="lnkEdit" CausesValidation="False" runat="server" Text='<%#Eval("OtherActivity") %>' Width="80px" 
                                         OnClientClick="window.document.forms[0].target='_blank'; setTimeout(function(){window.document.forms[0].target='';}, 500);" /></td>--%>
                                        <td><asp:HyperLink ID="hpl" runat="server" NavigateUrl= '<%# Eval("ComptId", "~/CompOtherActivity.aspx?ComptId={0}") %>' Text='<%#Eval("OtherActivity") %>' Target="_blank"></asp:HyperLink></td>
                                        
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                   </tbody>   
                                </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                                  </div>
                        <!-- /.box-body -->
                 
                    <!-- /.box -->

                </div>
                <!-- /.col -->
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
            $("#example1").DataTable({
                "order": [[0, "desc"]]
            });

        });

        $(function () {
            $("#example2").DataTable({
                "order": [[0, "desc"]]
            });

        });
        $(function () {
            $("#example3").DataTable({
                "order": [[0, "desc"]]
            });

        });

        $(function () {
            $("#example4").DataTable({
                "order": [[0, "desc"]]
            });

        });

        $(function () {
            $("#example11").DataTable({
                "order": [[0, "desc"]]
            });

        });

        $(function () {
            $("#example12").DataTable({
                "order": [[0, "desc"]]
            });

        });
        $(function () {
            $("#example13").DataTable({
                "order": [[0, "desc"]]
            });

        });

        $(function () {
            $("#example14").DataTable({
                "order": [[0, "desc"]]
            });

        });
        $(function () {
            $("#example19").DataTable({
                "order": [[0, "desc"]]
            });

        });
        $(function () {
            $("#example110").DataTable({
                "order": [[0, "desc"]]
            });

        });
    </script>
</asp:Content>
