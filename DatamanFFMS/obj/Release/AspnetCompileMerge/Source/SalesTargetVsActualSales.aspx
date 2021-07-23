<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="SalesTargetVsActualSales.aspx.cs" EnableEventValidation="false" Inherits="AstralFFMS.SalesTargetVsActualSales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>

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
                            $(this).removeAttr("checked");
                        }
                    });
                } else {
                    //Is Child CheckBox
                    var parentDIV = $(this).closest("DIV");
                    if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                        $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                    } else {
                        $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                    }
                }
            });
        })
    </script>
    <script type="text/javascript">
        $(function () {
            $('[id*=listboxmonth]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '70%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 100,
                //enableFiltering: true,
                //filterPlaceholder: 'Search'
            });
        });
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

    <style type="text/css">
        .excelbtn, #ContentPlaceHolder1_btnBack {
            background-color: #3c8dbc;
            border-color: #367fa9;
        }

        #excelExport {
            border-radius: 3px;
            -webkit-box-shadow: none;
            box-shadow: none;
            border: 1px solid transparent;
            background-color: #3c8dbc;
            border-color: #367fa9;
            color: white;
            height: 33px;
            padding: 0 0 3px 1px;
        }
    </style>
     <style>
        .alignright{
        text-align:right;
        }

        .qty.text-right {text-align:right!important;
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
                            <h3 class="box-title">Sales Target v/s Actual Sale </h3>
                            <div style="float: right">
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-9">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Year:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="txtcurrentyear" runat="server" CssClass="form-control select2" AutoPostBack="True" OnSelectedIndexChanged="txtcurrentyear_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="form-group" hidden>
                                    <label for="exampleInputEmail1">Sales Person:</label>
                                    <asp:DropDownList ID="ddlunderUser" runat="server" CssClass="form-control"></asp:DropDownList>

                                    <%--<asp:HiddenField ID="dateHiddenField" runat="server" />
                                    <asp:HiddenField ID="smIDHiddenField" runat="server" />
                                    <asp:HiddenField ID="beatIDHiddenField" runat="server" />--%>

                                </div>
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Sales Person:</label>
                                    <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All" ></asp:TreeView>

                                    <asp:HiddenField ID="dateHiddenField" runat="server" />
                                    <asp:HiddenField ID="smIDHiddenField" runat="server" />
                                    <asp:HiddenField ID="beatIDHiddenField" runat="server" />

                                </div>
                                 <div class="row">
                                    <div class="col-md-12 col-sm-6 col-xs-12">
                                         <div class="form-group">
                                             <label for="exampleInputEmail1">Months:</label>
                                             <asp:ListBox ID="listboxmonth" CssClass="form-control" runat="server" SelectionMode="Multiple"></asp:ListBox>                                                  
                                             </div>
                                        </div>
                                </div>
                                <div class="form-group">

                                    <asp:Button Style="margin-top:5px;" type="button" ID="btnshow" runat="server" Text="Show" class="btn btn-primary" OnClick="btnshow_Click" />
                                    <asp:Button Style="margin-top:5px;" ID="Cancel" CssClass="btn btn-primary" Text="Cancel" OnClick="Cancel_Click" runat="server" />
                                    <asp:Button Style="margin-right: 5px;margin-top:5px;" type="button" ID="btnexport" runat="server" Text="Export" class="btn btn-primary" OnClick="btnexport_Click" />
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="col-md-12">
                                <div class="form-group">
                                    <div class="table table-responsive">
                                        <!--<label for="exampleInputEmail1"></label>-->
                                        <asp:GridView ID="grd" runat="server" OnRowDataBound="grd_RowDataBound" AutoGenerateColumns="false" class="table table-bordered table-striped">
                                            <Columns>
                                               <%-- <asp:BoundField DataField="SMName" HeaderText="Name" />--%>
                                                <asp:TemplateField HeaderText="Product Group">
                                                    <ItemTemplate>
                                                       <asp:HiddenField ID="hidPartyTypeId" runat="server" Value='<%#Eval("Id") %>' />
                                                        <asp:Label ID="lblMatGrpType" runat="server" Text='<%#Eval("MatGrp") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Target/Actual Sales">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTargetFromHo" runat="server"></asp:Label>
                                                        <asp:Label ID="Label1" runat="server" Text="Target Sales" CssClass="center-block"></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="Label2" runat="server" Text="Actual Sales" CssClass="center-block"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>

                                                        <asp:Label ID="lnkRegion" runat="server" CommandName="Region"  CssClass="center-block" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="lnkRegion1" runat="server" CommandName="Region" CommandArgument='<%#Eval("Id")%>' ></asp:Label>
                                                    </ItemTemplate>
                                                     <ItemStyle CssClass="qty text-right"/>
                                                    <HeaderStyle CssClass="qty text-right"/>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnkState" runat="server" CommandName="State" CssClass="center-block" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="lnkState1" runat="server" CommandName="State" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                    </ItemTemplate>
                                                      <ItemStyle CssClass="qty text-right"/>
                                                    <HeaderStyle CssClass="qty text-right"/>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnkDistrict" runat="server" CommandName="District"  CssClass="center-block" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="lnkDistrict1" runat="server" CommandName="District" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                    </ItemTemplate>
                                                      <ItemStyle CssClass="qty text-right"/>
                                                    <HeaderStyle CssClass="qty text-right"/>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnkCity" runat="server" CommandName="City"  CssClass="center-block" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="lnkCity1" runat="server" CommandName="City"  CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                    </ItemTemplate>
                                                      <ItemStyle CssClass="qty text-right"/>
                                                    <HeaderStyle CssClass="qty text-right"/>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Area Incharge (Rs. in Lakhs)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnkArea" runat="server" CommandName="Area"  CssClass="center-block" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="lnkArea1" runat="server" CommandName="Area" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                    </ItemTemplate>
                                                      <ItemStyle CssClass="qty text-right"/>
                                                    <HeaderStyle CssClass="qty text-right"/>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="Total" HeaderText="Target" Visible="false"/>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                    <%--    <asp:HiddenField ID="hid" runat="server" Value='<%#Eval("PartyTypeId")%>' />--%>
                                                        <asp:Label ID="lblActual" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div>
                                <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                            </div>                             
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>


