<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="EmailTemplate.aspx.cs" Inherits="AstralFFMS.EmailTemplate" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script>
        new function ($) {
            $.fn.getCursorPosition = function () {
                var pos = 0;
                var el = $(this).get(0);
                // IE Support
                if (document.selection) {
                    el.focus();
                    var Sel = document.selection.createRange();
                    var SelLength = document.selection.createRange().text.length;
                    Sel.moveStart('character', -el.value.length);
                    pos = Sel.text.length - SelLength;
                }
                    // Firefox support
                else if (el.selectionStart || el.selectionStart == '0')
                    pos = el.selectionStart;
                return pos;
            }
        }(jQuery);
        $(document).ready(function () {
            $("#ContentPlaceHolder1_txtsubject").keypress(function () {
                $('#<%=hidcur.ClientID %>').val($("#ContentPlaceHolder1_txtsubject").getCursorPosition());
            $('#<%=HiddenField3.ClientID %>').val('1');
            //alert($("#ContentPlaceHolder1_txtemail").getCursorPosition());
        });

        $("#ContentPlaceHolder1_txtsubject").focus(function () {
            $('#<%=hidcur.ClientID %>').val($("#ContentPlaceHolder1_txtsubject").getCursorPosition());
            $('#<%=HiddenField3.ClientID %>').val('1');
            //alert($("#ContentPlaceHolder1_txtemail").getCursorPosition());
        });

        $("#ContentPlaceHolder1_txtemail").keypress(function () {
            $('#<%=hidcur.ClientID %>').val($("#ContentPlaceHolder1_txtemail").getCursorPosition());
            $('#<%=HiddenField3.ClientID %>').val('2');
            //alert($("#ContentPlaceHolder1_txtemail").getCursorPosition());
        });

        $("#ContentPlaceHolder1_txtemail").focus(function () {
            $('#<%=hidcur.ClientID %>').val($("#ContentPlaceHolder1_txtemail").getCursorPosition());
            $('#<%=HiddenField3.ClientID %>').val('2');
            //alert($("#ContentPlaceHolder1_txtemail").getCursorPosition());
        });




    });
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
            if ($('#<%=ddlkey.ClientID%>').val() == '0') {
                errormessage("Please Select the Key");
                return false;
            }
            if ($('#<%=txtsubject.ClientID%>').val() == '') {
                errormessage("Please enter the Subject");
                return false;
            }

            if ($('#<%=txtemail.ClientID%>').val() == '') {
                errormessage("Please enter the Email Body");
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
                __doPostBack('', depId)
            }
        }
    </script>


    <section class="content">

        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
                <asp:HiddenField ID="hidcur" runat="server" />
                <asp:HiddenField ID="HiddenField2" runat="server" />
                <asp:HiddenField ID="HiddenField3" runat="server" />
            </div>
        </div>
        <div class="box-body" id="mainDiv" style="display: none;" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Email Template</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                    OnClick="btnFind_Click" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <%-- <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>--%>

                        <div class="box-body">
                            <div class="col-md-5">

                                <div class="form-group">
                                    <label for="exampleInputEmail1">Key:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlkey" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlkey_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <div class="form-group">
                                    <label for="exampleInputEmail1">Subject:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:TextBox ID="txtsubject" runat="server" CssClass="form-control" placeholder="Enter Subject"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label for="exampleInputEmail1">Email Body:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:TextBox ID="txtemail" runat="server" Style="resize: none; height: 100px;" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Variables:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:ListBox ID="lstVariable" AutoPostBack="true" Style="resize: none; height: 150px;" OnSelectedIndexChanged="lstVariable_SelectedIndexChanged" CssClass="list-group-item form-control" runat="server"></asp:ListBox>

                                </div>
                                <div class="box-footer">
                                    <asp:Button Style="margin-right: 5px; margin-bottom: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                                    <asp:Button Style="margin-right: 5px; margin-bottom: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                                    <asp:Button Style="margin-right: 5px; margin-bottom: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" OnClientClick="Confirm();" class="btn btn-primary" OnClick="btnDelete_Click" />
                                </div>
                                <br />
                                <div>
                                    <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                                </div>
                            </div>
                        </div>

                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="box-body" id="rptmain" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Email Template List</h3>
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
                                                <th>Key</th>
                                                <th>Subject</th>
                                                <%-- <th>State Name</th>--%>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("Id") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("Id") %>' />
                                        <td><%#Eval("EmialKey") %></td>
                                        <td><%#Eval("Subject") %></td>
                                        <%--  <td><%#Eval("TemplateValue") %></td>--%>
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



