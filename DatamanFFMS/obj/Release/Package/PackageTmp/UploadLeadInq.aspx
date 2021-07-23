<%@ Page Title="Lead/Inquiry Import" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="UploadLeadInq.aspx.cs" Inherits="FFMS.UploadLeadInq" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 350, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: false, template: "error"
                //autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 10000, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
        function errormessage1(V1) {
            $("#messageNotification").jqxNotification({
                width: 350, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
        function errormessage3(V1) {
            $("#messageNotification").jqxNotification({
                width: 400, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: false, template: "error"
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
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
        function validatefile() {
            if (document.getElementById('<%=Fupd.ClientID %>').value == "") {
                errormessage1("Please Select a File.");
                return false;
            } else {
                var file = $('#<%=Fupd.ClientID %>').val();
                var ext = file.split('.').pop().toLowerCase();
                var validExtensions = ['xls', 'xlsx', 'csv'];
                if ($.inArray(ext, validExtensions) == -1) {
                    errormessage1("Invalid file type");
                    return false;
                } else {
                    return true;
                }
            }
            return true;
        }
    </script>
    <style>
        #samplefiles ul li {
            display: inline-block;
        }

            #samplefiles ul li::after {
                content: " | ";
            }

            #samplefiles ul li:last-child::after {
                display: none;
            }

        #samplefiles > ul {
            margin: 0 auto;
        }

        #samplefiles ul li a {
            vertical-align: middle;
        }
    </style>
    <section class="content">
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" width="30%" height="50%" alt="Loading" /><br />
            <b>Loading Data....</b>
        </div>
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <!-- left column -->
            <div class="col-md-6">
                <div id="InputWork">
                    <!-- general form elements -->
                    <div class="box box-primary">
                        <div class="box-header with-border">
                           <%-- <h3 class="box-title" style="line-height: 40px;">Import Lead/Inq</h3>--%>
                             <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            <div style="float: right" id="samplefiles" class="text-right">
                                <span>Download Sample </span>
                                <ul>
                                    <li>
                                        <asp:HyperLink ID="hpl" runat="server" Text="xls Format" NavigateUrl="~/SampleImportSheets/LeadInq.xls"></asp:HyperLink></li>
                                    <li>
                                        <asp:HyperLink ID="hpl1" runat="server" Text="xlsx Format" NavigateUrl="~/SampleImportSheets/LeadInq.xlsx"></asp:HyperLink></li>
                                    <li>
                                        <asp:HyperLink ID="hpl2" runat="server" Text="csv Format" NavigateUrl="~/SampleImportSheets/LeadInq.CSV"></asp:HyperLink></li>
                                </ul>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <label id="lblcols" runat="server" style="color: red; font-family: Arial; font-size: small; font-weight: 300;" visible="false"></label>

                            <div class="form-group">
                                <label for="exampleInputEmail1">File Name:</label>

                                <asp:FileUpload runat="server" ID="Fupd" CssClass="btn btn-primary" />
                            </div>

                            <div class="form-group">
                                <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary" OnClientClick="return validatefile()" Text="Import" OnClick="btnUpload_Click" />

                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </section>
</asp:Content>
