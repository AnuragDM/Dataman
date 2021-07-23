<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MeetSummaryScreenL1.aspx.cs" Inherits="AstralFFMS.MeetSummaryScreenL1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .table-responsive {
            border: 1px solid #fff;
        }
    </style>

    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
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
        $(function () {
            $("#example11").DataTable();

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
 <%--   <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
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
    <style>
        .meetsummary-L1 table tr th {
            padding: 7px;
        }

        .meetsummary-L1 table tr td {
            padding: 7px;
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
                        <div class="box-header with-border">
                            <h3 class="box-title">MEET SUMMARY SCREEN</h3>
                            <div style="float: right">
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                                 <div class="form-group col-md-4 col-sm-6">
                                <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>
                            </div>
                            </div>
                            <div class="form-group col-md-3 col-sm-4">
                                <label for="exampleInputEmail1">Year:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="txtcurrentyear" Width="100%" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="form-group col-md-3 col-sm-4" hidden>
                                <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:DropDownList ID="ddlunderUser" Width="100%" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <%-- <div class="form-group col-md-3 col-sm-4">
                                <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>
                            </div>--%>

                           <%-- <div class="form-group col-md-6 col-sm-4">
                                <label for="exampleInputEmail1" style="visibility: hidden;">gregege</label><br />
                                <asp:Button ID="btnshow" CssClass="btn btn-primary" Text="Show" OnClick="btnshow_Click" runat="server" />
                                <asp:Button ID="btncancel1" CssClass="btn btn-primary" Text="Reset" OnClick="btncancel1_Click" runat="server" />
                            </div>        --%>  
                             <div class="box-footer">
                                <label for="exampleInputEmail1" style="visibility: hidden;">gregege</label><br />
                                <asp:Button ID="btnshow" CssClass="btn btn-primary" Text="Show" OnClick="btnshow_Click" runat="server" />
                                <asp:Button ID="btncancel1" CssClass="btn btn-primary" Text="Reset" OnClick="btncancel1_Click" runat="server" />
                                </div>                 

                           <%-- <div class="clearfix"></div>--%>
                            <div class="form-group">
                                <div class="table meetsummary-L1 table-responsive">
                                    <label for="exampleInputEmail1"></label>
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Type of Meet">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hidPartyTypeId" runat="server" Value='<%#Eval("Id") %>' />
                                                    <asp:LinkButton ID="lblUserType" runat="server" CommandName="Meet" CommandArgument='<%#Eval("Id") %>' Text='<%#Eval("Name") %>'></asp:LinkButton>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Target">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hidusertype" runat="server" Value='<%#Eval("Id") %>' />
                                                    <asp:HiddenField ID="lblUserTypeName" runat="server" Value='<%#Eval("Name") %>' />
                                                    <asp:Label ID="lblTargetFromHo" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Plan Submitted">
                                                <ItemTemplate>
                                                    <asp:Label ID="lnkTarget" runat="server" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Rejected">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRejected" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Actual Meet Done">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblActualmeet" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Approx. Budget">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblexpenses" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pending for Approval">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpendingforapproval" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Remaining Target">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblremaining" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>





                            <label for="exampleInputEmail1">
                                <asp:Label ID="lblPartTypeName" runat="server" CssClass="text-bold"></asp:Label>
                                <asp:Label ID="lblPartTypeID" Visible="false" runat="server" CssClass="form-control text-bold"></asp:Label></label>
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                                    <HeaderTemplate>
                                        <table id="example11" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <th>Sr.No.</th>
                                                    <th>Meet Date</th>
                                                    <th>MeetName</th>
                                                    <th>City</th>
                                                    <th>Venue</th>
                                                    <th>Party Name</th>
                                                    <th>No of Users</th>
                                                    <th>Product Class</th>
                                                    <th>Approx Budget</th>
                                                    <th>Meet Status</th>
                                                    <th>Approval Remark</th>
                                                    <th>Approval Date</th>
                                                    <th>Products</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#Container.ItemIndex+1 %></td>
                                            <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("MeetDate"))%></td>
                                            <td><%#Eval("MeetName") %></td>

                                            <td><%#Eval("Location") %></td>
                                            <td><%#Eval("Venue") %></td>
                                            <td><%#Eval("PartyName") %></td>
                                            <td><%#Eval("NoOfUser") %></td>
                                            <td><%#Eval("IndName") %></td>
                                            <td><%#Eval("LambBudget") %></td>
                                            <td><%#Eval("AppStatus") %></td>
                                            <td><%#Eval("AppRemark") %></td>
                                            <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("Appdate"))%></td>

                                            <td>
                                                <asp:LinkButton ID="lnkedit" runat="server" Text="View Products" CommandName="MeetEdit" CommandArgument='<%#Eval("MeetPlanId")%>'></asp:LinkButton></td>

                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>  
                                       </table>       
                                    </FooterTemplate>

                                </asp:Repeater>
                            </div>

                            <asp:Label ID="lblresult" runat="server" />

                            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
                                CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
                            </ajaxToolkit:ModalPopupExtender>

                            <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Width="50%" Style="display: none">
                                <div class="col-md-12">

                                    <div class="form-group">
                                        <asp:GridView ID="GridView2" runat="server" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Product Group">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hidProductgroup" runat="server" Value='<%#Eval("ProdctGroupId")%>' />
                                                        <asp:Label ID="lblPGName" runat="server" Text='<%#Eval("ProdctGroup")%>'></asp:Label>
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
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                    <div class="form-group">
                                        <asp:Button ID="btnCancel" class="btn btn-primary" runat="server" Text="Cancel" />
                                    </div>
                                </div>

                            </asp:Panel>
                        </div>


                        <div class="box-footer">
                            <%--  <asp:Button ID="Cancel" CssClass="btn btn-primary" Text="Cancel" OnClick="Cancel_Click" runat="server" />--%>
                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </section>


</asp:Content>


