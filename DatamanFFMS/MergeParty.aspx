<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MergeParty.aspx.cs" Inherits="AstralFFMS.MergeParty" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>

    <style type="text/css">
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        #rcorners2, #rcorners12 {
            border-radius: 25px;
            border: 1px solid #d9d3d6;
            padding: 4px;
        }

        .rcorners2 {
            border-radius: 25px;
            border: 1px solid #d9d3d6;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            /*display: table;*/
        }

        #ContentPlaceHolder1_gvPartyData th {
            text-align: center;
        }

        #ContentPlaceHolder1_gvPartyData td, th {
            padding: 13px;
        }

    </style>
    <%-- <script type="text/javascript">
        function pageLoad() {
            //Initialize Select2 Elements
            $(".select2").select2();
            //var config = {
            //    '.chosen-select': {}

            //}
            //for (var selector in config) {
            //    $(selector).chosen(config[selector]);
            //}
        };
    </script>--%>
    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "error"
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
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <script type="text/javascript">
        $(function () {
            $("#tblParty").DataTable({
                "bSort": false
            });
        });
    </script>
    <script type="text/javascript">
       
    </script>
    <script type="text/javascript">
        function validate() {
            if ($('#<%=ddlOldCity.ClientID%>').val() == "0") {
                errormessage("Please select the City.");
                return false;
            }
            if ($('#<%=ddlOldArea.ClientID%>').val() == "0") {
                errormessage("Please select the Area.");
                return false;
            }
            if ($('#<%=ddlOldBeat.ClientID%>').val() == "0") {
                errormessage("Please select the Beat.");
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function validateOnUpdate() {
            if ($('#<%=ddlNewCity.ClientID%>').val() == "0") {
                errormessage("Please select New City.");
                return false;
            }
            if ($('#<%=ddlNewArea.ClientID%>').val() == "0") {
                errormessage("Please select New Area.");
                return false;
            }
            if ($('#<%=ddlNewBeat.ClientID%>').val() == "0") {
                errormessage("Please select New Beat.");
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function SelectAllByRow(ChK, cellno) {
            var gv = document.getElementById('<%= gvPartyData.ClientID %>');
            for (var i = 1; i <= gv.rows.length - 1; i++) {
                var len = gv.rows[i].getElementsByTagName("input").length;
                if (gv.rows[i].getElementsByTagName("input")[cellno].type == 'checkbox') {
                    gv.rows[i].getElementsByTagName("input")[cellno].checked = ChK.checked
                }
            }
        }
    </script>
    <%--  <script type="text/javascript">
        $(function dr() {
            $("#tblParty [id*=chkHeader]").click(function () {
                if ($(this).is(":checked")) {
                    $("#tblParty [id*=chkRow]").attr("checked", "checked");
                } else {
                    $("#tblParty [id*=chkRow]").removeAttr("checked");
                }
            });
            $("#tblParty [id*=chkRow]").click(function () {
                if ($("#tblParty [id*=chkRow]").length == $("#tblParty [id*=chkRow]:checked").length) {
                    $("#tblParty [id*=chkHeader]").attr("checked", "checked");
                } else {
                    $("#tblParty [id*=chkHeader]").removeAttr("checked");
                }
            });
        });
    </script>--%>
    <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body">
            <div class="box box-primary">
                <div class="box-header with-border">
                  <%--  <h3 class="box-title">Merge Party</h3>--%>
                    <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                </div>
                <div id="" style="background-color: white; padding: 0 17px 19px 5px;">
                    <div class="row">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="col-md-3 col-sm-7">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Old City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlOldCity" runat="server" Width="100%" class="form-control" TabIndex="3"  
                                            OnSelectedIndexChanged="ddlOldCity_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-7">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Old Area:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlOldArea" runat="server" Width="100%" class="form-control" TabIndex="3" 
                                            OnSelectedIndexChanged="ddlOldArea_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-7">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Old Beat:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlOldBeat" runat="server" Width="100%" class="form-control" TabIndex="3"></asp:DropDownList>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="box-footer">
                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnShow" OnClientClick="javascript:return validate();" runat="server" Text="Show" class="btn btn-primary"
                            OnClick="btnShow_Click" />
                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary"
                            OnClick="btnSubmit_Click" />
                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                            OnClick="btnCancel_Click" />
                    </div>

                </div>
                <div class="box-body table-responsive" id="mainDiv" style="display: none;" runat="server">
                    <asp:GridView ID="gvPartyData" runat="server" AutoGenerateColumns="False"
                        CssClass="tbl" Width="100%" >
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkHeader" runat="server" onclick="SelectAllByRow(this, 0)" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkItem" runat="server" />
                                    <asp:HiddenField ID="partyIdHiddenField" runat="server" Value='<%#Eval("PartyID") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="30px" />
                            </asp:TemplateField>
                            <%--  <asp:BoundField DataField="PartyID" HeaderText="ID" ItemStyle-Width="50px" Visible="false">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="PartyName" HeaderText="Party Name" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="Mobile" HeaderText="Mobile No" />
                            <asp:BoundField DataField="Address1" HeaderText="Address1" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="City" HeaderText="City" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="Area" HeaderText="Area" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="Beat" HeaderText="Beat" ItemStyle-Wrap="false" />
                        </Columns>
                        <FooterStyle BackColor="#EED690" />
                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" BackColor="#3c8dbc" />
                        <SelectedRowStyle BackColor="#008A8C" HorizontalAlign="Center" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#3c8dbc" HorizontalAlign="Right" Font-Bold="True" ForeColor="white" />
                        <AlternatingRowStyle BackColor="#e8e8e8" />
                        <AlternatingRowStyle CssClass="tbl1" />
                    </asp:GridView>
                    <%--<asp:Repeater ID="partyrpt" runat="server">
                        <HeaderTemplate>
                            <table id="tblParty" class="table table-bordered table-striped">
                                <thead>
                                    <tr>
                                        <th style="width: 5%">
                                            <asp:CheckBox ID="chkHeader" runat="server" onclick="dr()" /></th>
                                        <th>Party</th>
                                        <th>Mobile No.</th>
                                        <th>Address1</th>
                                        <th>City</th>
                                        <th>Area</th>
                                        <th>Beat</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("PartyID") %>' />
                                <td style="width: 5%">
                                    <asp:CheckBox ID="chkRow" runat="server" />
                                </td>
                                <td><%#Eval("PartyName") %></td>
                                <td style="text-align: right;"><%#Eval("Mobile") %></td>
                                <td style="text-align: right;"><%#Eval("Address1") %></td>
                                <td style="text-align: right;"><%#Eval("City") %></td>
                                <td style="text-align: right;"><%#Eval("Area") %></td>
                                <td style="text-align: right;"><%#Eval("Beat") %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody>     </table>       
                        </FooterTemplate>
                    </asp:Repeater>--%>
                </div>

                <div id="conditionalDiv" style="display: none; background-color: white; padding: 0 17px 19px 5px;" runat="server">
                    <div class="row">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="col-md-3 col-sm-7">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">New City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlNewCity" runat="server" Width="100%" class="form-control" TabIndex="3" OnSelectedIndexChanged="ddlNewCity_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-7">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">New Area:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlNewArea" runat="server" Width="100%" class="form-control" TabIndex="3"
                                            OnSelectedIndexChanged="ddlNewArea_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-7">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">New Beat:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlNewBeat" runat="server" Width="100%" class="form-control" TabIndex="3"></asp:DropDownList>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="box-footer">
                        <asp:Button type="button" ID="btnUpdate" runat="server" OnClientClick="javascript:return validateOnUpdate();" Text="Update" class="btn btn-primary"
                            OnClick="btnUpdate_Click" />
                    </div>
                </div>
                <br />
                <div>
                    <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
