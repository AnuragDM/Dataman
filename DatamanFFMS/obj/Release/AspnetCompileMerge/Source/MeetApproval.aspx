<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MeetApproval.aspx.cs" Inherits="AstralFFMS.MeetApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
     <script type="text/javascript">
         function pageLoad() {
             $("#example1").DataTable({
                 "order": [[0, "desc"]]
             });
         };
    </script>
    <style type="text/css">
        #ContentPlaceHolder1_GridView2 tr th, td {
            padding: 2px 4px;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .table-responsive {
            border: 1px solid #fff;
        }

        @media (max-width: 600px) {
            #ContentPlaceHolder1_pnlpopup {
                width: 100%;
            }
        }

        @media (min-width: 600px) {
            #ContentPlaceHolder1_pnlpopup {
                width: 500px;
            }
        }
    </style>
    <script>
        function vali() {
            if ($('#<%=txtRemarks.ClientID%>').val() == "") {
                $('#<%=lblerror.ClientID%>').val() = "Please enter the Remark";
                return false;
            }
        }
    </script>
    <script>
        function valiL1() {

        }

    </script>
    <script>
        function valiL1() {


        }

    </script>
    <%--      <script type = "text/javascript">
            function ClientItemSelected1(sender, e) {
                $get("<%=hfitemid.ClientID %>").value = e.get_value();
        }
</script>--%>
    <section class="content">
        <script type="text/javascript">
            function load1() {
                $(".numeric").numeric({ negative: false });
            }

            $(window).load(function () {

                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(load1);

            });


        </script>

        <%--<script type="text/javascript">
              function Confirm() {
                  var confirm_value = document.createElement("INPUT");
                  confirm_value.type = "hidden";
                  confirm_value.name = "confirm_value";
                  if (confirm("Are you sure to Approve/Reject?")) {
                      confirm_value.value = "Yes";
                  } else {
                      confirm_value.value = "No";
                  }
                  document.forms[0].appendChild(confirm_value);
              }
    </script>--%>

        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <div class="box-body" id="mainDiv" runat="server">
                    <div class="row">
                        <!-- left column -->
                        <div class="col-md-12">
                            <!-- general form elements -->
                            <div class="box box-default">
                                <div class="box-header">
                                    <h3 class="box-title">Meet Approval Entry</h3>
                                    <div style="float: right">
                                    </div>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="col-md-12">
                                        <div class="box-body table-responsive">
                                            <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Meet Date</th>
                                                <th>Sales Person</th>
                                                <th>Document No.</th>
                                                <th>Meet Name</th>
                                                <th>Venue</th>
                                                <th>No of Users</th>
                                                <th>Approx Budget</th>
                                                <th>Distributor</th>
                                                <th>Distributor Sharing %</th>
                                                <th>Astral Sharing %</th>
                                                <th>Scheme code</th>
                                                <th>Comments</th>
                                               
                                                
                                                <th>Meet Type</th>
                                                <th>No of Staff</th>
                                           
                                                <th>Beat</th>
                                                <th>Qty Required</th>
                                                <th>Type Of Gift</th>
                                                <th>City</th>
                                                <th>City Code</th>
                                                 <th>Party Name</th>
                                                 <th>Status</th>
                                                <th>Approve/Reject</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <asp:HiddenField ID="did" runat="server" Value='<%#Eval("MeetPlanId")%>' />
                                        <td><%#Eval("MeetDate","{0:dd/MMM/yyyy}") %></td>
                                        <td><%#Eval("SMName") %></td>
                                        <td><%#Eval("MeetPlanDocId") %></td>
                                        <td><%#Eval("MeetName") %></td>
                                        <td><%#Eval("VenueId") %></td>
                                        <td><%#Eval("NoOfUser") %></td>
                                        <td><%#Eval("LambBudget") %></td>
                                        <td><%#Eval("Distributor") %></td>
                                        <td><%#Eval("ExpShareDist") %></td>
                                        <td><%#Eval("ExpShareSelf") %></td>
                                        <td><%#Eval("Scheme") %></td>
                                        <td><%#Eval("Comments") %></td>
                                       
                                        <td><%#Eval("meetproduct") %></td>
                                        <td><%#Eval("NoStaff") %></td>
                                     
                                         <td><%#Eval("BeatName") %></td>
                                        <td><%#Eval("valueofRetailer") %></td>
                                        <td><%#Eval("typeOfGiftEnduser") %></td>
                                         <td><%#Eval("CityName") %></td>
                                        <td><%#Eval("CityCode") %></td>
                                        <td><%#Eval("PartyName") %></td>
                                         <td><%#Eval("AppStatus") %></td>
                                        <td>  <asp:LinkButton ID="lnkDistRpt" runat="server" Text="Approve/Reject" CommandName="Coll" OnClick="lnkDistRpt_Click" CommandArgument='<%#Eval("MeetPlanId")%>' Enabled='<%#Eval("AppStatus").ToString()=="Pending" ? true : false %>' ForeColor='<%#Eval("AppStatus").ToString()=="Pending" ? System.Drawing.ColorTranslator.FromHtml("#3c8dbc") : System.Drawing.Color.Black %>'></asp:LinkButton></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>
                            </asp:Repeater>
                                            <%--<asp:GridView runat="server" ID="gvdetails" CssClass="table table-bordered table-striped" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:BoundField DataField="MeetDate" HeaderText="Meet Date" DataFormatString="{0:dd/MMM/yyyy}" />
                                                    <asp:BoundField DataField="MeetPlanDocId" HeaderText="Document No" />
                                                    <asp:BoundField DataField="MeetName" HeaderText="Meet Name" />
                                                    <asp:BoundField DataField="VenueId" HeaderText="Venue" />
                                                    <asp:BoundField DataField="NoOfUser" HeaderText="No of Users" />
                                                    <asp:BoundField DataField="LambBudget" HeaderText="Approx Budget" />
                                                    <asp:BoundField DataField="Comments" HeaderText="Comments" />
                                                    <asp:TemplateField HeaderText="Approve/Reject">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="did" runat="server" Value='<%#Eval("MeetPlanId")%>' />
                                                            <asp:LinkButton ID="lnkDist" Text="Approve/Reject" CommandArgument='<%#Eval("MeetPlanId")%>' CommandName="Coll" OnClick="lnkDist_Click" runat="server"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>--%>
                                        </div>
                                        <asp:Label ID="lblresult" runat="server" />

                                        <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
                                            CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
                                        </ajaxToolkit:ModalPopupExtender>

                                        <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Width="" Style="display: none">
                                            <div class="col-md-12">
                                                <div>
                                                    <h3>Meet Approval</h3>
                                                    <asp:Label ID="lblerror" runat="server" Style="color: red;"></asp:Label>
                                                </div>
                                                <div>
                                                    <div class="form-group col-md-4 col-sm-4 col-xs-6">
                                                        <label for="exampleInputEmail1">Planned Budget:</label>
                                                        <asp:TextBox ID="txtPlannedBudget" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:TextBox ID="txtmeetId" ReadOnly="true" runat="server" Visible="false" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group col-md-4 col-sm-4 col-xs-6">
                                                        <label for="exampleInputEmail1">Approve Budget:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                        <asp:TextBox ID="txtApprovedBudget" MaxLength="12" runat="server" CssClass="form-control numeric"></asp:TextBox>
                                                    </div>

                                                    <div class="form-group col-md-4 col-sm-4 col-xs-12">
                                                        <label for="exampleInputEmail1">Approve/Reject:</label>
                                                        <asp:DropDownList ID="ddlApp" runat="server" CssClass="form-control select2">
                                                            <asp:ListItem Text="Approved" Value="Approved"> </asp:ListItem>
                                                            <asp:ListItem Text="Reject" Value="Rejected"> </asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="clearfix"></div>

                                                    <div class="form-group col-md-12 col-sm-12 col-xs-12">
                                                        <label for="exampleInputEmail1">Remark:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                        <asp:TextBox ID="txtRemarks" runat="server" Rows="2" Cols="3" Height="10%" MaxLength="255" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="clearfix"></div>
                                                    <div class="form-group col-md-12" style="display:none;">
                                                        <label for="exampleInputEmail1">Product to be demonstrate/introduce :</label>
                                                    </div>
                                                    <div class="clearfix"></div>
                                                    <div class="form-group col-md-6  col-sm-6 col-xs-6" style="display:none;">
                                                        <label for="exampleInputEmail1">Product Class: </label>
                                                        <asp:DropDownList ID="ddlclass" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-6 col-sm-6 col-xs-6" style="display:none;">
                                                        <label for="exampleInputEmail1">Product Segment:</label>
                                                        <asp:DropDownList ID="ddlsegment" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                                    </div>
                                                    <div class="clearfix"></div>
                                                    <div class="form-group col-md-12  col-sm-12 col-xs-12" style="display:none;">
                                                        <label for="exampleInputEmail1">Product Group:</label>
                                                        <asp:DropDownList ID="ddlgroup" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                                    </div>
                                                    <div class="clearfix"></div>
                                                    <%--  <div class="form-group paddingleft0">
                    <label for="exampleInputEmail1">Item Name:</label>
                    <asp:TextBox ID="txtItem" onkeyup="SetContextKey1();" runat="server" CssClass="form-control paddingleft0"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" ServicePath="MeetPlanEntry.aspx" ServiceMethod="SearchItem1"
                        TargetControlID="txtItem" UseContextKey="true" FirstRowSelected="false" OnClientItemSelected="ClientItemSelected1" MinimumPrefixLength="3" EnableCaching="true">
                    </ajaxToolkit:AutoCompleteExtender>
                    <asp:HiddenField ID="hfitemid" runat="server" />

                </div>--%>

                                                    <div class="form-group col-md-12">
                                                        <asp:Button ID="btnAdd" runat="server" class="btn btn-primary" Visible="false" Text="Add" OnClick="btnAdd_Click" />
                                                    </div>
                                                    <div class="clearfix"></div>
                                                    <div class="form-group">
                                                        <asp:GridView ID="GridView2" runat="server" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" AutoGenerateColumns="False" OnRowDeleting="GridView2_RowDeleting">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sr No.">
                                                                    <ItemTemplate>
                                                                        <%#Container.DataItemIndex+1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Product Class">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hidMaterialClass" runat="server" Value='<%#Eval("MatrialClassId")%>' />
                                                                        <asp:Label ID="lbMaterialClass" runat="server" Text='<%#Eval("MatrialClass")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Product Segment">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hidSegment" runat="server" Value='<%#Eval("SegmentId")%>' />
                                                                        <asp:Label ID="lblSegment" runat="server" Text='<%#Eval("Segment")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Product Group">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hidProductgroup" runat="server" Value='<%#Eval("ProdctGroupId")%>' />
                                                                        <asp:Label ID="lblPGName" runat="server" Text='<%#Eval("ProdctGroup")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                                <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" Visible="false" ControlStyle-CssClass="btn btn-primary" ButtonType="Button" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="form-group ">
                                                            <asp:Button ID="btnUpdate" CommandName="Update" runat="server" class="btn btn-primary" Text="Save" OnClick="btnUpdate_Click" />
                                                            <asp:Button ID="btnCancel" class="btn btn-primary" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div>
                                                        <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <!-- /.box-body -->
                                </div>
                                <!-- /.box -->

                            </div>
                            <!-- /.col -->
                        </div>

                    </div>
                </div>
                </div>
     
            </ContentTemplate>
        </asp:UpdatePanel>
    </section>
</asp:Content>

