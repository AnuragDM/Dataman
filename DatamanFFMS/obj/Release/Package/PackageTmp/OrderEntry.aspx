<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="OrderEntry.aspx.cs" Inherits="AstralFFMS.OrderEntry" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    <script>
        function valid() {
           <%-- if ($('#<%=txtTotalAmount.ClientID%>').val() == "") {
                errormessage("Please enter the Order Amount");
                return false;
            }--%>

            //Added as per UAT - on 14 Dec 2015
            <%--if ($('#<%=txtTotalAmount.ClientID%>').val() <= 0) {
                errormessage("Please enter Order Amount greater than 0.");
                return false;
            }--%>
            //End

            if ($('#<%=Remark.ClientID%>').val() == '') {
                errormessage("Please enter Order Discussion");
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure to delete?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <script type="text/javascript">
        function DoNav(depId) {
            if (depId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', depId)
            }
        }
    </script>

    <%--<script type="text/javascript">
           $(document).ready(function () {
               $('#btnAdd').click(function () {
                   $.ajax({
                       type: "POST",
                       url: '<%= ResolveUrl("MeetApproval.aspx/AddRec") %>',
                       contentType: "application/json; charset=utf-8",
                       data: '{ItemID : "' + $('#<%=ddlproduct.ClientID%>').val() + '",meetplanID: "' + $('#<%=vistIDHiddenField.ClientID%>').val() + '" }',
                       dataType: "json",
                       success: function (data) {
                           var row = $("[id*=ContentPlaceHolder1_gvDetails] tr:last-child").clone(true);
                           $("[id*=ContentPlaceHolder1_gvDetails] tr").not($("[id*=ContentPlaceHolder1_gvDetails] tr:first-child")).remove();

                           for (var i = 0; i < data.d.length; i++) {
                               $("#ContentPlaceHolder1_gvDetails").append("<tr><td>" + data.d[i].Sr + "</td><td>" + data.d[i].ItemName + "</td><td class=hidden>" + data.d[i].ItemId + "</td><td><input type='button' class='btn btn-primary' value='Delete' onclick=deleteRowdeleteRow(this) id='" + data.d[i].ItemId + "' /></td></tr>");
                           }
                           $('#<%=ddlproduct.ClientID%>').val(0);
                       },
                       error: function (result) {
                           // alert("Error");
                           alert(result);
                       }
                   });
               });
           });
      </script>--%>

    <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body" id="mainDiv" style="display: none;" runat="server">
            <div class="row">
                <!-- left column -->

                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">

                                <div class="form-group">

                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                    <div class="col-sm-12">
                                        <div class="col-lg-4 col-md-5 col-sm-8 paddingleft0">
                                            <asp:Label ID="partyName" runat="server" CssClass="text" Text="Label"></asp:Label><br />
                                            <asp:Label ID="address" runat="server" CssClass="text" Text="Label"></asp:Label>,&nbsp;
                                     <asp:Label ID="lblzipcode" runat="server" CssClass="text" Text=""></asp:Label>,&nbsp;
                                     <asp:Label ID="mobile" runat="server" CssClass="text" Text="Label"></asp:Label>&nbsp;
                                        </div>
                                        <div class="col-lg-2 col-md-2 col-sm-8 paddingleft0">
                                            <asp:Label ID="lblVisitdate1" runat="server" CssClass="text" Text="Visit Date"></asp:Label><br />
                                            <asp:Label ID="lblVisitDate5" runat="server" CssClass="text" Text="Visit Date"></asp:Label>&nbsp;
                                        </div>
                                        <div class="col-lg-2 col-md-2 col-sm-8 paddingleft0">
                                            <asp:Label ID="lblAreaName1" runat="server" CssClass="text" Text="Area Name"></asp:Label><br />
                                            <asp:Label ID="lblBeatName5" runat="server" CssClass="text" Text="Beat Name"></asp:Label>&nbsp;
                                        </div>
                                        <div class="col-lg-2 col-md-2 col-sm-8 paddingleft0">
                                            <asp:Label ID="lblBeatName1" runat="server" CssClass="text" Text="Beat Name"></asp:Label><br />
                                            <asp:Label ID="lblAreaName5" runat="server" CssClass="text" Text="Area Name"></asp:Label>&nbsp;
                                        </div>

                                        <div class="col-lg-2 col-md-1 col-sm-8 paddingleft0" style="float: right">

                                            <asp:Button Style="margin-right: 5px; margin-bottom: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                                OnClick="btnFind_Click" />
                                            <asp:Button ID="btnBack1" runat="server" Text="Back" Style="margin-right: 5px; margin-bottom: 5px;" class="btn btn-primary" OnClick="btnBack_Click1" />

                                            <asp:HiddenField ID="HiddenField2" runat="server" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <div class="col-sm-4 paddingleft0">
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
                            <div class="form-group paddingleft0">
                                <h3>Order Entry</h3>
                                <asp:HiddenField ID="hid" runat="server" />
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->

                            <div class="box-body">
                                <div class="col-lg-4 col-md-6 col-sm-8 col-xs-10">

                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Total Order Amount:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                        <asp:TextBox ID="txtTotalAmount" runat="server" class=" form-control numeric text-right" Text="0.00" MaxLength="12"></asp:TextBox>
                                    </div>
                                    <div id="divdocid" runat="server" class="form-group">
                                        <label for="exampleInputEmail1">Document No:</label>
                                        <asp:TextBox ID="lbldocno" Enabled="false" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>


                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Order Discussion:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="Remark" runat="server" Style="resize: none; height: 20%;" class="form-control" cols="20" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="box-footer">
                                <asp:Button ID="btnsave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnsave_Click" OnClientClick="return valid();" />&nbsp;&nbsp;
                      <asp:Button ID="btnreset" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnreset_Click" />&nbsp;&nbsp;
                     <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm();" OnClick="btnDelete_Click" />
                                <br /><br />
                                <div>
                                    <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="box-body" id="rptmain" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Order List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Document Date</th>
                                                <th>Document No.</th>
                                                <th>Amount</th>
                                                <th>Order Discussion</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("OrdId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("OrdId") %>' />
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("VDate"))%></td>
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
                "order": [[0, "desc"]]
            });

        });
    </script>
</asp:Content>
