<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="UserListForMeet.aspx.cs" Inherits="AstralFFMS.UserListForMeet" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function pageLoad() {
            $("#example1").DataTable({
                    "pageLength": 500                
                });
        };
    </script>

    
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style>
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .table-responsive {
            border: 1px solid #fff;
        }

        #ContentPlaceHolder1_GridView1 tr td, th {
            padding: 2px 4px;
        }
    </style>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>

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
        function grdHeaderCheckBox(objRef) {
            var grd = objRef.parentNode.parentNode.parentNode;
            var inputList = grd.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
            }
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

            if ($('#<%=ddlmeetType.ClientID%>').val() == "0") {
                errormessage("Please select the Meet Type.");
                return false;
            }
            if ($('#<%=ddlmeetName.ClientID%>').val() == "0") {
                errormessage("Please select the Meet.");
                return false;
            }
          <%--  if ($('#<%=txtdistName.ClientID%>').val() == '') {
                errormessage("Please enter the distributer Name.");
                return false;
            }
            if ($('#<%=hfCustomerId.ClientID%>').val() == '') {
                errormessage("Please enter the valid distributer Name.");
                return false;
            }
            if ($('#<%=hfCustomerId.ClientID%>').val() == 0) {
                errormessage("Please enter the valid distributer Name.");
                return false;
            }--%>
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
                __doPostBack('', depId)
            }
        }
    </script>

    <script type="text/javascript">
        function checkDate(sender, args) {
            if (sender._selectedDate > new Date()) {
                errormessage("You cannot select a day greater than today!");
                sender._selectedDate = new Date();
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
        }
    </script>
    <section class="content">
        <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="messageNotification">
                    <div>
                        <asp:Label ID="lblmasg" runat="server"></asp:Label>
                    </div>
                </div>

                <div class="box-body" id="mainDiv" runat="server">
                    <div class="row">
                        <!-- left column -->
                        <div class="col-md-12">
                            <!-- general form elements -->
                            <div class="box box-default">
                                <div class="box-header">
                                   <%-- <h3 class="box-title">User List</h3>--%>
                                     <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                                    <div style="float: right">
                                    </div>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="col-md-5">
                                        <div class="form-group col-md-6 paddingleft0">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlunderUser" AutoPostBack="true" OnSelectedIndexChanged="ddlunderUser_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>

                                        <div class="form-group col-md-6 paddingright0">
                                            <label for="exampleInputEmail1">Meet Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlmeetType" AutoPostBack="true" OnSelectedIndexChanged="ddlmeetType_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>

                                        <div class="row">
                                            <div id="DIVUnder" runat="server" class="form-group col-md-12">
                                                <label for="exampleInputEmail1">Meet Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:DropDownList ID="ddlmeetName" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                            <div id="divdocid" runat="server" class="form-group col-md-12 paddingright0">
                                                <%-- <label for="exampleInputEmail1">Document No:</label>
                                    <asp:TextBox ID="lbldocno" Enabled="false" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>--%>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-6 paddingleft0">
                                            <label for="withSales">Area:</label>
                                            <asp:DropDownList ID="ddlArea" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>

                                        <div class="form-group col-md-6 paddingright0">
                                            <label for="withSales">Beat:</label>
                                            <asp:DropDownList ID="ddlbeat" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-6 paddingleft0">
                                            <label for="exampleInputEmail1">Name:</label>
                                            <asp:TextBox ID="txtName" runat="server" class="form-control" placeholder="Enter Contact Person"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-6 paddingright0">
                                            <label for="withSales">Party Type:</label>
                                            <asp:DropDownList ID="ddlpartyType" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>

                                    </div>
                                </div>
                                <div class="box-footer">
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="Button1" runat="server" Text="Show" class="btn btn-primary" OnClientClick="javascript:return validate();" OnClick="Button1_Click" />
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="Button2" runat="server" Text="Cancel" class="btn btn-primary" OnClick="Button2_Click" />
                                    <%--<asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />--%>
                                    <%--<asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm();" OnClick="btnDelete_Click" />--%>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="box-body" id="rptmain" runat="server">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box">
                                <div class="box-header">
                                    <h3 class="box-title">Meet User List</h3>
                                    <div style="float: right">
                                        <%-- <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />--%>
                                    </div>
                                </div>
                                <!-- /.box-header -->
                                <%--       <div class="box-body table-responsive">
                        </div>--%>


                                <div class="table table-responsive">

                                    <%--Added--%>
                                    <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <th style="display: none;">Sr No.</th>
                                                        <th>Party Name</th>
                                                        <th>Industry</th>
                                                        <th>Contact Person</th>
                                                        <th>Mobile</th>
                                                        <th>Address</th>
                                                        <th style="text-align:center;"> <asp:CheckBox ID="chkAll" runat="server" OnCheckedChanged="chkAll_CheckedChanged" AutoPostBack="true" /></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("Id")%>' />
                                                <asp:HiddenField ID="hidParty" runat="server" Value='<%#Eval("Id")%>' />
                                                <td style="display: none;"><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>
                                                <td><%#Eval("Name") %></td>
                                                <td><%#Eval("PartyTypeName") %></td>
                                                <td><%#Eval("ContactPersonName") %></td>
                                                <td><%#Eval("MobileNo") %></td>
                                                <td><%#Eval("Address") %></td>
                                                <td style="text-align:center;">
                                                    <asp:CheckBox ID="chkRow" Checked='<%# Eval("MeetActive")!=DBNull.Value ? Eval("MeetActive") :false %>' runat="server" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>     </table>       
                                        </FooterTemplate>

                                    </asp:Repeater>
                                    <%--End--%>

                                    <%--<asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" AutoGenerateColumns="False">
                                        <Columns>

                                            <asp:TemplateField HeaderText="Sr No.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Party Name">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("Id")%>' />
                                                    <asp:HiddenField ID="hidParty" runat="server" Value='<%#Eval("Id")%>' />
                                                    <asp:Label ID="lblPName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Industry">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIndName" runat="server" Text='<%#Eval("PartyTypeName")%>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mobile">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblmobile" runat="server" Text='<%#Eval("MobileNo")%>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Address">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("Address")%>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>


                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAll" runat="server" onclick="grdHeaderCheckBox(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRow" Checked='<%# Eval("MeetActive")!=DBNull.Value ? Eval("MeetActive") :false %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>--%>
                                </div>

                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                                <!-- /.box-body -->
                              
                            </div>
                            <!-- /.box -->

                        </div>

                        <!-- /.col -->
                    </div>

                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
           <asp:Button Style="margin-right: 5px;" type="button" ID="btnexport" runat="server" Text="Export -To-Excel" class="btn btn-primary" OnClick="btnexport_Click" />
        <br />
        <br />
        <div>
            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
        </div>
       
    </section>
    <!-- SlimScroll -->

</asp:Content>
