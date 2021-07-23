<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="TransLeadInqList.aspx.cs" Inherits="AstralFFMS.TransLeadInqList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .Bdr {
            border: solid;
            border-color: #367FA9;
            /*height: 309px;*/
        }

        .Bdr1 {
            border: solid;
            border-color: #367FA9;
            height: 100px;
        }

        .Bdr2 {
            border: solid;
            border-color: #367FA9;
            margin-bottom: 10px;
        }

        .Frght {
            float: left;
            font-weight: bold;
        }
    </style>

    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 250, position: "top-right", opacity: 2,
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
                width: 250, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <script type="text/javascript">
        function validate() {
            if ($('#<%=txtremarks.ClientID%>').val() == "") {
                errormessage("Please enter Remarks");
                return false;
            }
        }
    </script>
    <section class="content">
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" width="30%" height="50%" alt="Loading" /><br />
            <b>Loading Data....</b>
        </div>
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>


        <div class="box-body" id="rptmain" runat="server">
            <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <div class="col-md-7">
                                <h3 class="box-title">
                                    <asp:Label ID="lblheading" runat="server">Lead/Inq Details</asp:Label></h3>
                            </div>
                            <div class="col-md-5">
                                <span class="Frght">
                                    <asp:Label ID="lblDoctype" runat="server" Font-Bold="true" style="font-weight: bold; display: inline-block; min-width: 105px;">Lead/Inq Doc ID:</asp:Label>
                            <asp:Label runat="server" ID="lblCSID" Font-Bold="true" CssClass="right"></asp:Label>
                                </span>
                            </div>
                        </div>
                        <div class="box-header">
                            <div class="col-md-7"></div>
                            <div class="col-md-5">
                                <span class="Frght">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="true" style="font-weight: bold; display: inline-block; min-width: 105px;">Current Status:</asp:Label>
                                    <asp:Label ID="lblstatus" runat="server" Font-Bold="true"></asp:Label>
                                </span>
                            </div>
                        </div>
                        <%--                        <div class="box-body">
                            
                        </div>--%>
                        <!-- /.box-header -->

                        <div class="box-body">

                            <div class="col-md-12">


                                <div class="Bdr2 table-responsive">
                                    <table class="table table-bordered table-striped">                                        
                                        <tr>
                                            <td>Date:</td>
                                            <td><label id="lbldate" runat="server"></label></td>
                                            <td>Contact Person Name:</td>
                                            <td><label id="lblContactPerson" runat="server"></label></td>
                                        </tr>                                     
                                        <tr>
                                            <td>Inquiry/Lead Type:</td>
                                            <td><label id="lblLeadInqType" runat="server"></label></td>
                                            <td>Address</td>
                                            <td><label id="lblAddress" runat="server"></label></td>
                                        </tr>
                                        <tr>
                                            <td>Caller Type:</td>
                                            <td><label id="lblCallertype" runat="server"></label></td>
                                            <td>City:</td>
                                            <td><label id="lblCity" runat="server"></label></td>
                                        </tr>                                     
                                        <tr>
                                            <td>Nature:</td>
                                            <td><label id="lblNature" runat="server"></label></td>
                                            <td>State:</td>
                                            <td><label id="lblState" runat="server"></label></td>
                                        </tr>
                                        <tr>
                                            <td>Product type:</td>
                                            <td><label id="lblProductType" runat="server"></label></td>
                                            <td>Country:</td>
                                            <td><label id="lblCountry" runat="server"></label></td>
                                        </tr>                                     
                                        <tr>
                                            <td>Apprx. Odrer Value:</td>
                                            <td><label id="lblApprxOrderVal" runat="server"></label></td>
                                            <td>Mobile:</td>
                                            <td><label id="lblMobile" runat="server"></label></td>
                                        </tr>
                                        <tr>
                                            <td>Avg.. Odrer Value in Lac.:</td>
                                            <td><label id="lblAvgOrderInLac" runat="server"></label></td>
                                            <td>Phone No:</td>
                                            <td><label id="lblPhoneNo" runat="server"></label></td>
                                        </tr>                                     
                                        <tr>
                                            <td>Sales Person:</td>
                                            <td><label id="lblSalesPerson" runat="server"></label></td>
                                            <td>Email:</td>
                                            <td><label id="lblEmail" runat="server"></label></td>
                                        </tr>
                                        <tr>
                                            <td>Inquiry/Lead Description:</td>
                                            <td><label id="lblLeadInqDesc" runat="server"></label></td>
                                            <td>Fax:</td>
                                            <td><label id="lblFax" runat="server"></label></td>
                                        </tr>
                                        <tr>
                                            <td>Source of Inquiry/Lead:</td>
                                            <td><label id="lblSource" runat="server"></label></td>
                                            <td>Source of Information:</td>
                                            <td><label id="lblSourceInfo" runat="server"></label></td>
                                        </tr>
                                        <tr>
                                            <td>Firm Name:</td>
                                            <td colspan="3"><label id="lblFirmName" runat="server"></label></td>
                                        </tr>
                                    </table>


                                </div>

                            </div>
                            <div class="clearfix"></div>

                            <div class="col-md-12">
                            </div>





                            <div class="">
                                <div class="col-md-6">
                                    <div class="box-body table-responsive">
                                        <asp:Repeater ID="rpt" runat="server">
                                            <HeaderTemplate>
                                                <table id="example1" class="table table-bordered table-striped">
                                                    <thead>
                                                        <tr>
                                                <th>Respond Date</th>
                                       <%--         <th>Complaint No.</th>--%>
                                                <th>Responded By</th>
                                                <th>Remark</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("RespondDate"))%></td>
                                      <%--   <td><%#Eval("CompDocID") %></td>--%>
                                        <td><%#Eval("Empname") %></td>
                                        <td><%#Eval("Remarks") %></td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>     </table>       
                                            </FooterTemplate>

                                        </asp:Repeater>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="Bdr">
                                        <table class="table table-bordered table-striped">
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" Font-Bold="true" Text="Action"></asp:Label>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td>
                                                    <asp:Label runat="server" Text="Order Value Received"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox  Width="20%"  CssClass="text" ID="txtOrderValue" runat="server"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" Text="Remarks"></asp:Label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox TextMode="MultiLine" Width="100%" Rows="5" CssClass="text" ID="txtremarks" runat="server" Wrap="true"></asp:TextBox></td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="RdbStatus" RepeatDirection="Horizontal" runat="server">
                                                    <asp:ListItem Selected="True" Value="WIP" Text="WIP"></asp:ListItem>
                                                    <asp:ListItem Value="Resolved" Text="Resolved"></asp:ListItem>     
                                                    <asp:ListItem Value="Closed" Text="Closed"></asp:ListItem>   
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    
                                                    <asp:Button type="button" ID="btnSave" OnClientClick="javascript:return validate();" runat="server" OnClick="btnSave_Click" Text="Save" class="btn btn-primary" />
                                                    <asp:Button type="button" PostBackUrl="~/TransLeadInq.aspx"  ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" />
                                                    <%--<asp:LinkButton ID="lnkcancel" PostBackUrl="~/TransLeadInq.aspx" runat="server" Text="Cancel" class="btn btn-primary"></asp:LinkButton></td>--%>

                                            </tr>
                                        </table>

                                    </div>
                                </div>

                            </div>
                        </div>

                        <br />
                        <%--  <div class="col-md-3 col-sm-6 col-xs-12">
                                  
                                </div>--%>
                        <br />
                        <br />
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                        <!-- /.box-body -->
                    </div>

                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>
            <%--     </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>

    </section>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
                "order": [[0, "desc"]],
                "bFilter": false,
                "bPaginate": false,
                "bSort": false
            });

        });
    </script>


</asp:Content>
