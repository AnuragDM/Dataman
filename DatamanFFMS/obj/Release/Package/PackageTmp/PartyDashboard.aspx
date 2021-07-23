<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="PartyDashboard.aspx.cs" Inherits="AstralFFMS.PartyDashboard" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function myCompetitor() {
            var partyID = $('#<%=HiddenField1.ClientID%>').val();
            var url = "Competitor.aspx?PartyId=" + partyID;
            window.location.href = url;
        }
    </script>
    <script type="text/javascript">
        function myFunction() {
            var partyID = $('#<%=HiddenField1.ClientID%>').val();

            var url = "FaildVisit.aspx?PartyId=" + partyID;
            window.location.href = url;
        }
    </script>
    <script type="text/javascript">
        function myPartyHist() {
            var partyID = $('#<%=HiddenField1.ClientID%>').val();
            var url = "PartyHistory.aspx?PartyId=" + partyID;
            window.location.href = url;
        }
    </script>
    <script type="text/javascript">
        function myCollection() {
            var partyID = $('#<%=HiddenField1.ClientID%>').val();
            var url = "PartyCollection.aspx?PartyId=" + partyID;
            window.location.href = url;
        }
    </script>
    <script type="text/javascript">
        function myOrderTemplate() {
            var partyID = $('#<%=HiddenField1.ClientID%>').val();
            var url = "OrderTemplate.aspx?PartyId=" + partyID;
            window.location.href = url;
        }
    </script>
    <script type="text/javascript">
        function myOrderEntry() {
            var partyID = $('#<%=HiddenField1.ClientID%>').val();
            var url = "OrderEntry.aspx?PartyId=" + partyID;
            window.location.href = url;
        }
    </script>
    <script type="text/javascript">
        function myEditParty() {
            var partyID = $('#<%=HiddenField1.ClientID%>').val();
            var url = "EditParty.aspx?PartyId=" + partyID;
            window.location.href = url;
        }
    </script>
    <script type="text/javascript">
        function DemoEntry() {
            var partyID = $('#<%=HiddenField1.ClientID%>').val();
            var url = "DemoEntry.aspx?PartyId=" + partyID;
            window.location.href = url;
        }
    </script>
    <style>
        #ContentPlaceHolder1_bookOrder, #ContentPlaceHolder1_demoEntry, #ContentPlaceHolder1_failedVisit, #ContentPlaceHolder1_collection, #ContentPlaceHolder1_partyHist, #ContentPlaceHolder1_editParty, #ContentPlaceHolder1_orderTemplate, #ContentPlaceHolder1_competitor, #ContentPlaceHolder1_btnCompetitor {
            width: 100% !important;
        }
    </style>
    <section class="content">
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
                        <%-- <div class="box-header with-border">
                            <h3 class="box-title">
                                <asp:Label ID="lblvdate" runat="server" CssClass="text" ></asp:Label>
                            </h3>

                             
                        </div>--%>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="form-group">
                                <%--<asp:Label ID="Label1" runat="server" Text="Label" Visible="false"></asp:Label>--%>
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Planned Beat:</label>
                                    <asp:Label ID="lblPlanedbeat" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-sm-12">
                                    <div class="col-lg-4 col-md-5 col-sm-8 paddingleft0">
                                        <asp:Label ID="partyName" runat="server" CssClass="text" Text="Label"></asp:Label></br>
                                        <asp:Label ID="address" runat="server" CssClass="text" Text="Label"></asp:Label>,&nbsp;
                                     <asp:Label ID="lblzipcode" runat="server" CssClass="text" Text=""></asp:Label>,&nbsp;
                                     <asp:Label ID="mobile" runat="server" CssClass="text" Text="Label"></asp:Label>&nbsp;
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

                                        <asp:Button ID="BtnBack" runat="server" Text="Back" Style="margin-right: 5px;" class="btn btn-primary" OnClick="BtnBack_Click" />
                                        <asp:HiddenField ID="hid" runat="server" />
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
                        <div class="box-footer alinctrbtn">
                            <div class="col-md-3"></div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Button ID="bookOrder" runat="server" Style="margin-right: 5px;" CssClass="btn btn-primary btn-block" Text="Discussion/Book Order" OnClick="btnOrder_Click" />
                                    <%-- <input style="margin-right: 5px;" type="button" id="bookOrder"  value="Book Order" onclick="myOrderEntry()" class="btn btn-primary" />--%>
                                </div>                               
                                <div class="form-group">
                                    <asp:Button ID="demoEntry" runat="server" Style="margin-right: 5px;" CssClass="btn btn-primary btn-block" Text="Demo Entry" OnClick="btnDemo_Click" />
                                    <%-- <input style="margin-right: 5px;" type="button" id="demoEntry"  value="Demo Entry" onclick="DemoEntry()" class="btn btn-primary" />--%>
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="failedVisit" runat="server" Style="margin-right: 5px;" CssClass="btn btn-primary btn-block" Text="Failed Visit" OnClick="failedVisit_Click" />
                                    <%--   <input style="margin-right: 5px;" type="button" id="failedVisit" onclick="myFunction()"   value="Failed Visit" class="btn btn-primary" />--%>
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="collection" runat="server" Style="margin-right: 5px;" CssClass="btn btn-primary btn-block" Text="Collection" OnClick="collection_Click" />
                                    <%--   <input style="margin-right: 5px;" type="button" id="collection" onclick="myCollection()"  value="Collection" class="btn btn-primary" />--%>
                                </div>                               
                                <div class="form-group">
                                    <asp:Button ID="btnCompetitor" runat="server" Style="margin-right: 5px;" CssClass="btn btn-primary btn-block" Text="Competitor's Activity" OnClick="btnCompetitor_Click" />
                                </div>
                                 <div class="form-group">
                                    <asp:Button ID="SaleReturn" runat="server" Style="margin-right: 5px;" CssClass="btn btn-primary btn-block" Text="Sales Return" OnClick="SaleReturn_Click" />
                                    <%-- <input style="margin-right: 5px;" type="button" id="bookOrder"  value="Book Order" onclick="myOrderEntry()" class="btn btn-primary" />--%>
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="editParty" runat="server" Style="margin-right: 5px;" CssClass="btn btn-primary btn-block" Text="Edit Party" OnClick="editParty_Click1" />
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="btnadditem" runat="server" Style="margin-right: 5px;" CssClass="btn btn-primary btn-block" Visible="false" Text="Item Template" OnClick="btnadditem_Click" />
                                </div>
                                 <div class="form-group">
                                    <asp:Button ID="partyHist" runat="server" Style="margin-right: 5px;" CssClass="btn btn-primary btn-block" Text="Party History" OnClick="partyHist_Click" />
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="btnActivityTrans" runat="server" Style="margin-right: 5px;" CssClass="btn btn-primary btn-block" Visible="true" Text="Activity" OnClick="btnActivityTrans_Click" />
                                </div>
                            </div>
                            <div class="col-md-3"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
