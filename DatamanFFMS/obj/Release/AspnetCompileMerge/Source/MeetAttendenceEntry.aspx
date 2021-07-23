<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="MeetAttendenceEntry.aspx.cs" Inherits="AstralFFMS.MeetAttendenceEntry" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
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
    <style>
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .table-responsive {
            border: 1px solid #fff;
        }

        .alignright {
            text-align: right !important;
        }

        .aligncenter {
            text-align: center !important;
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
            $('#<%=lblmasg.ClientID %>').html('Record Inserted Successfully.');
            $("#messageNotification").jqxNotification("open");
        }
    </script>
    <script>
        function valid() {

            if ($('#<%=ddlmeetTye.ClientID%>').val() == '0') {
                errormessage("Please select the Meet Type");
                return false;
            }

            if ($('#<%=ddlMeet.ClientID%>').val() == '0') {
                errormessage("Please select Meet Name");
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

    <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <div class="box-body" id="mainDiv" runat="server">
                    <div class="row">
                        <!-- left column -->


                        <div class="col-md-12">
                            <div id="InputWork">
                                <!-- general form elements -->
                                <div class="box box-primary">
                                    <div class="box-header with-border">
                                    </div>
                                    <div class="form-group paddingleft0">
                                        <h3>Meet Attendance  Entry</h3>
                                        <asp:HiddenField ID="hid" runat="server" />
                                    </div>
                                    <!-- /.box-header -->
                                    <!-- form start -->

                                    <div class="box-body">
                                        <div class="col-md-6">

                                            <div class="form-group col-md-6 paddingleft0">
                                                <label for="exampleInputEmail1">Meet Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:DropDownList ID="ddlmeetTye" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlmeetTye_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="form-group col-md-6 paddingleft0">
                                                <label for="exampleInputEmail1">Meet Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:DropDownList ID="ddlMeet" Width="100%" runat="server" CssClass="form-control"></asp:DropDownList>
                                            </div>

                                            <div id="divdocid" runat="server" class="form-group col-md-6 paddingleft0">

                                                <asp:Button ID="btnsave" runat="server" Text="Show" class="btn btn-primary" Style="margin-top: 20px;" OnClick="btnsave_Click" OnClientClick="return valid();" />&nbsp;&nbsp;
                                        
                                        <asp:Button ID="btncancel" runat="server" Text="Cancel" class="btn btn-primary" Style="margin-top: 20px;" OnClick="btncancel_Click" />&nbsp;&nbsp;
                                            </div>

                                            <div id="div1" runat="server" class="form-group col-md-4 col-sm-12 paddingright0">
                                                <label for="requiredFields" style="color: red;">&nbsp;</label>

                                            </div>
                                            <div id="div2" runat="server" class="form-group col-md-4 col-sm-12 paddingright0">
                                                <label for="requiredFields" style="color: red;">&nbsp;</label>

                                            </div>



                                        </div>
                                    </div>

                                </div>
                            </div>
                            <!--<div class="box-footer">
                </div>-->
                        </div>

                    </div>
                </div>
                <div class="box-body" id="rptmain" runat="server">
                    <div class="row">
                        <div class="col-xs-12">

                            <div class="box">
                                <div class="box-header">
                                    <h3 class="box-title">Party List</h3>
                                    <div style="float: right">
                                    </div>
                                </div>
                                <!-- /.box-header -->
                                <div class="box-body table-responsive">

                                    <div class="table table-responsive">

                                        <%--Added--%>
                                        <asp:Repeater ID="rpt" runat="server">
                                            <HeaderTemplate>
                                                <table id="example1" class="table table-bordered table-striped">
                                                    <thead>
                                                        <tr>
                                                            <th style="display: none;">Sr No.</th>
                                                            <th>Party Name</th>
                                                            <th>Address</th>
                                                            <th>Contact Person</th>
                                                            <th style="text-align:center;">Mobile</th>
                                                           <%-- <th>Remark</th>--%>
                                                            <th style="text-align: center;">
                                                                <asp:CheckBox ID="chkAll" runat="server" OnCheckedChanged="chkAll_CheckedChanged" AutoPostBack="true" /></th>
                                                             <th>Remark</th>
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
                                                    <td><%#Eval("Address") %></td>
                                                    <td><%#Eval("ContactpersonName") %></td>
                                                    <td style="text-align:right;"><%#Eval("MobileNo") %></td>
                                                   <%-- <td><asp:TextBox ID="txtremark" runat="server" Height="44px" Width="155px" TextMode="MultiLine" Text='<%#Eval("PresentRemark")%>' CssClass="form-control"></asp:TextBox></td>--%>
                                                    <td style="text-align: center;">
                                                        <asp:CheckBox ID="chkRow" Checked='<%# Eval("PresentStatus") %>' runat="server" />
                                                    </td>
                                                     <td><asp:TextBox ID="txtremark" runat="server" Height="44px" Width="155px" Text='<%#Eval("PresentRemark")%>' CssClass="form-control"></asp:TextBox></td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>     </table>       
                                            </FooterTemplate>

                                        </asp:Repeater>
                                        <%--End--%>

                                        <%--<asp:GridView ID="GridView1" runat="server" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" AutoGenerateColumns="False" EmptyDataText="No Record Found!">
                                            <Columns>

                                                <asp:TemplateField HeaderText="Sr No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Party Name">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hidParty" runat="server" Value='<%#Eval("Id")%>' />
                                                        <asp:Label ID="lblPName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Address">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("Address")%>'></asp:Label>
                                                    </ItemTemplate>

                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mobile" ItemStyle-CssClass="alignright" HeaderStyle-CssClass="aligncenter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblmobile" runat="server" Text='<%#Eval("MobileNo")%>'></asp:Label>
                                                    </ItemTemplate>

                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Remark">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtremark" runat="server" Height="44px" Width="115px" TextMode="MultiLine" Text='<%#Eval("PresentRemark")%>' CssClass="form-control"></asp:TextBox>
                                                    </ItemTemplate>

                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkAll" runat="server" onclick="grdHeaderCheckBox(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkRow" Checked='<%# Eval("PresentStatus") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>--%>
                                    </div>
                                    <div class="box-footer">

                                        <asp:Button ID="btnsubmit" runat="server" Text="Submit" class="btn btn-primary" Style="margin-top: 20px;" OnClick="btnsubmit_Click" OnClientClick="return valid();" />&nbsp;&nbsp;
                                    </div>

                                </div>

                                <!-- /.box-body -->
                            </div>   
                            </div>
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
                            <!-- /.box -->

                      <%--  </div>
                        <!-- /.col -->
                    </div>

                </div>--%>
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>


    </section>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>


    <%--<script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
                "pageLength": 500,
                "order": [[0, "desc"]]
            });

        });
    </script>--%>
</asp:Content>

