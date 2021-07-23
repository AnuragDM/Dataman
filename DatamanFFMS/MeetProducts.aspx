<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MeetProducts.aspx.cs" Inherits="AstralFFMS.MeetProducts" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .table-responsive {
            border: 1px solid #fff;
        }

        #ContentPlaceHolder1_GridView2 tr td, th {
            padding: 2px 4px;
        }
    </style>
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

    <script type="text/javascript">
        function validate() {

            if ($('#<%=ddlmeetType.ClientID%>').val() == "0") {
                errormessage("Please select the Meet Type.");
                return false;
            }
            if ($('#<%=ddlmeet.ClientID%>').val() == "0") {
                errormessage("Please select the Meet Name.");
                return false;
            }


        }
    </script>

    <section class="content">
        <div id="messageNotification">
            <asp:Label ID="lblmasg" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12">
            <div class="box">
                <div class="box-header">
                   <%-- <h3 class="box-title">Meet Products</h3>--%>
                    <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                      <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnback" runat="server" Text="Back" class="btn btn-primary"
                                    OnClick="btnback_Click" />
                            </div>
                </div>
                <!-- /.box-header -->
                <div class="box-body">
                    <div class="col-md-12">
                        <asp:UpdatePanel ID="up" runat="server">
                            <ContentTemplate>
                                <div class="col-md-5">



                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlunderUser" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Meet Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlmeetType" AutoPostBack="true" OnSelectedIndexChanged="ddlmeetType_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Meet Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlmeet" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlmeet_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Product to be demonstrate/introduce :</label>
                                        <div class="form-group paddingleft0">
                                            <label for="exampleInputEmail1">Product Class: </label>
                                            <asp:DropDownList ID="ddlclass" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="form-group paddingleft0">
                                            <label for="exampleInputEmail1">Product Segment:</label>
                                            <asp:DropDownList ID="ddlsegment" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="form-group paddingleft0">
                                            <label for="exampleInputEmail1">Product Group:</label>
                                            <asp:DropDownList ID="ddlgroup" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>


                                    <div class="box-footer">
                                        <asp:Button ID="btnAdd" runat="server" class="btn btn-primary" Text="Add" OnClick="btnAdd_Click" OnClientClick="javascript:return validate();" />

                                    </div>
                                </div>
                                <div class="form-group col-md-12 table-responsive">
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
                                            <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ControlStyle-CssClass="btn btn-primary" ButtonType="Button" />
                                        </Columns>
                                    </asp:GridView>
                                    <div class="box-footer">
                                        <asp:Button ID="btnsave" runat="server" class="btn btn-primary" Text="Save" OnClick="btnsave_Click" />
                                        <asp:Button ID="btncancel" runat="server" class="btn btn-primary" Text="Cancel" OnClick="btncancel_Click" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>


                        <%--         <div class="form-group">
                               <label for="exampleInputEmail1">Item Name:</label>
                    <asp:TextBox ID="txtItem" onkeyup="SetContextKey1();" runat="server" CssClass="form-control paddingleft0"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" ServicePath="MeetPlanEntry.aspx" ServiceMethod="SearchItem1" CompletionListCssClass="completionList"
                        TargetControlID="txtItem"  FirstRowSelected="false" OnClientItemSelected="ClientItemSelected1" MinimumPrefixLength="3" EnableCaching="true">
                    </ajaxToolkit:AutoCompleteExtender>
                    <asp:HiddenField ID="hfitemid" runat="server" />
                            </div>--%>
                    </div>

                    <br />
                    <div>
                        <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                    </div>

                </div>




                <!-- /.box-body -->
            </div>
            <!-- /.box -->

        </div>

        <!-- /.col -->
    </section>



    <div class="clearfix"></div>





</asp:Content>
