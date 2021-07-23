<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MeetImageUpload.aspx.cs" Inherits="AstralFFMS.MeetImageUpload" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--  <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
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

            if ($('#<%=ddlmeetType.ClientID%>').val() == '0') {
                errormessage("Please select the Meet Type");
                return false;
            }

            if ($('#<%=ddlmeet.ClientID%>').val() == '0') {
                errormessage("Please select Meet Name");
                return false;
            }

            var uploadcontrol = document.getElementById('<%=FileUpload1.ClientID%>').value;
            if (uploadcontrol.length == 0) {
                errormessage("Please select the Image");
                return false;
            }
            var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
            if (uploadcontrol.length > 0) {
                if (reg.test(uploadcontrol)) {
                    return true;
                }
                else {
                    errormessage("Only image files are allowed!");
                    return false;
                }
            }
        }
    </script>

    <script type="text/javascript">
        function load1() {
            $(".numeric").numeric({ decimal: false, negative: false });
            $("#example1").DataTable();
        }
        $(window).load(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(load1);
        });
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

    <section class="content">

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
                          <%--  <h3 class="box-title">Meet Image Upload</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            <div style="float: right">
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <asp:UpdatePanel ID="update" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="col-md-5">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlunderUser" AutoPostBack="true" OnSelectedIndexChanged="ddlunderUser_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Meet Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlmeetType" AutoPostBack="true" OnSelectedIndexChanged="ddlmeetType_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>

                                        <div class="form-group">
                                            <label for="withSales">Meet Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlmeet" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlmeet_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Choose File:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:FileUpload ID="FileUpload1" runat="server" />
                                        </div>
                                               <div class="form-group">
                                                <tr><td colspan="2">
                      
                                <b>Note : Image size should be less than 1MB</b>
                       </td></tr>
                                            </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="Button1" runat="server" Text="Show" class="btn btn-primary" OnClick="Button1_Click" />
                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>


            </div>

        </div>

        <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">

                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Sr. No</th>
                                                <th>Image Name</th>
                                                <th>Image</th>
                                                <th>Delete</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%#Container.ItemIndex+1 %></td>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("Id") %>' />
                                        <td><%#Eval("ImgName") %></td>
                                        <td>
                                            <asp:Image ID="img" runat="server" Height="200" Width="200" ImageUrl='<%#Eval("ImgUrl") %>' /></td>
                                        <td>
                                            <asp:LinkButton ID="lnk" runat="server" Text="Delete" OnClientClick="Confirm();" OnClick="lnk_Click"></asp:LinkButton></td>
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
    <script type="text/javascript">
        $(document).ready(function () {
            var valLength = "";
            $("#DepName").keypress(function (key) {

                valLength = ($("#DepName").val().length + 1);

                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
        });
    </script>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();

        });
    </script>
</asp:Content>
