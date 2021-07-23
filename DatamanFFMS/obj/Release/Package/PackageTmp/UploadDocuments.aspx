<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="UploadDocuments.aspx.cs" Inherits="AstralFFMS.UploadDocuments" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
     <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>

    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable({
                "order": [[0, "desc"]]
            });

        });
    </script>
     <script type="text/javascript">
         $(function () {
             $('[id*=ListBox1]').multiselect({
                 enableCaseInsensitiveFiltering: true,
                 //buttonWidth: '200px',
                 buttonWidth: '100%',
                 includeSelectAllOption: true,
                 maxHeight: 200,
                 width: 215,
                 enableFiltering: true,
                 filterPlaceholder: 'Search'
             });
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
        function Valild() {
            if ($('#<%=txttitle.ClientID%>').val() == '') {
                errormessage("Please enter the title");
                return false;
            }
            if ($('#<%=fileLabel.ClientID %>').text() == '' || $('#<%=fileLabel.ClientID %>').text() == 'Label') {
                var uploadcontrol = document.getElementById('<%=File1.ClientID%>').value;
                if (uploadcontrol.length == 0) {
                    errormessage("Please select the document file");
                    return false;
                }
            }         
        }
    </script>
     <script type="text/javascript">
         function showpreview(input) {
             var uploadcontrol = document.getElementById('<%=File1.ClientID%>').value;
            //var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
            if (uploadcontrol.length > 0) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $("#ContentPlaceHolder1_fileLabel").css('display', 'none');
                        }
                        reader.readAsDataURL(input.files[0]);
                    }
                
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
     <style type="text/css">
        .input-group .form-control {
            height: 34px;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            /*display: table;*/
        }

        .multiselect-container > li > a {
            white-space: normal;
        }

        .multiselect-container > li {
            width: 100%;
        }

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }

        .spinner {
        position: absolute;
        top: 50%;
        left: 50%;
        margin-left: -50px; /* half width of the spinner gif */
        margin-top: -50px; /* half height of the spinner gif */
        text-align: center;
        z-index: 999;
        overflow: auto;
        width: 100px; /* width of the spinner gif */
        height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
    }
    </style>
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
                                <%--<h3 class="box-title">Upload Documents</h3>--%>
                                 <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                                <div style="float: right">
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                        OnClick="btnFind_Click" />

                                </div>
                                <asp:HiddenField ID="hid" runat="server" />

                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body">

                                <div class="col-md-5">
                                    <div class="form-group paddingleft0">
                                        <div id="divdocid" runat="server" class="form-group">
                                            <label for="exampleInputEmail1">Document No:</label>
                                            <asp:TextBox ID="lbldocno" Enabled="false" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group paddingleft0">
                                        <label for="exampleInputEmail1">Title: </label>
                                        &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txttitle" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                    </div>

                                    <div class="form-group paddingleft0">
                                        <label for="exampleInputEmail1">Document For: </label>
                                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal" CssClass="radiogroup" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                                            <asp:ListItem Text="Sales Team" Value="Sales Team" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Distributor" Value="Distributor"></asp:ListItem>
                                            <asp:ListItem Text="Both" Value="Both"></asp:ListItem>
                                            <asp:ListItem Text="Specific SalesPerson"  Value="Specific SalesPerson"></asp:ListItem>
                                            <asp:ListItem Text="Specific Distributor" Value="Specific Distributor"></asp:ListItem>
                                        </asp:RadioButtonList>                                    
                                    </div>                    
                                     <div id="salesP" class="form-group" runat="server" style="display:none;">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                           
                                            <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>
                                     </div>
                                     <div id="divDistributor" class="form-group" runat="server" style="display:none;">
                                            <label for="exampleInputEmail1">Distributor:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                           
                                            <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                     </div>
                               
                                    <div class="form-group paddingleft0">
                                        <label for="exampleInputEmail1">Active: </label>
                                        <asp:CheckBox ID="chk" runat="server" Checked="true" />
                                    </div>

                                    <div class="form-group paddingleft0">
                                        <label for="exampleInputEmail1">Upload: </label>
                                        &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:FileUpload ID="File1" runat="server" onchange="showpreview(this);" />
                                        <asp:Label ID="fileLabel" runat="server" Text="Label" Style="display: none;"></asp:Label>
                                        <asp:Button ID="btnsave" runat="server" Style="margin-top: 5px;" Text="Save" class="btn btn-primary" OnClientClick="return Valild();" OnClick="btnsave_Click" />
                                        <asp:Button Style="margin-right: 5px; margin-top: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                            OnClick="btnCancel_Click" />
                                        <asp:Button Style="margin-right: 5px; margin-top: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary"
                                            OnClientClick="Confirm()" OnClick="btnDelete_Click" />                                     
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
        <div class="box-body" id="rptmain" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Document List</h3>
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
                                                <th>Title</th>
                                                <th>Link Url</th>
                                                <th>Active</th>
                                                <th>Document For</th>
                                                <th hidden>Delete</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("id") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("id") %>' />
                                        <asp:HiddenField ID="HiddenField2" runat="server" Value='<%#Eval("LinkURL") %>' />
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("DocDate"))%></td>
                                        <td><%#Eval("DocID") %></td>
                                        <td><%#Eval("Title") %></td>
                                        <td><%#Eval("LinkURL") %></td>
                                        <td><%#Eval("Active") %></td>
                                        <td><%#Eval("DocFor") %></td>
                                        <td hidden>
                                            <asp:LinkButton ID="lnkdelete" runat="server" OnClick="lnkdelete_Click" OnClientClick="">Delete</asp:LinkButton></td>
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

</asp:Content>
