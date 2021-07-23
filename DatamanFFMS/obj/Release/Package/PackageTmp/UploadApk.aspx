<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="UploadApk.aspx.cs" Inherits="AstralFFMS.UploadApk" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
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
    <script type="text/javascript">
        $(document).ready(function () {

           // getProductFromLicense();
        })




        $(function () {
            $("#tblApp").DataTable({
                "order": [[0, "asc"]]
            });

        });
        $(function () {
            $('[id*=lstProduct]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=lstCompany]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=ddlAppname]').multiselect({
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

            var selectedProduct = [];
            var selectedCompany = [];

            $("#<%=lstProduct.ClientID %>: selected").each(function () {
                selectedProduct.push($(this).val());
            })
            if (selectedProduct == "")
            {
                errormessage("Please Select Product");
                return false;
            }

            $("#<%=lstCompany.ClientID %>: selected").each(function () {
                selectedCompany.push($(this).val());
            })
            if (selectedCompany == "") {
                errormessage("Please Select Company");
                return false;
            }

            if ($('#<%=txtVersionName.ClientID%>').val() == '') {
                errormessage("Please enter version name");
                return false;
            }
            if ($('#<%=txtversionCode.ClientID%>').val() == '') {
                errormessage("Please enter version code");
                return false;
            }
            if ($('#<%=fileLabel.ClientID %>').text() == '' || $('#<%=fileLabel.ClientID %>').text() == 'Label') {
                var uploadcontrol = document.getElementById('<%=File1.ClientID%>').value;
                if (uploadcontrol.length == 0) {
                    errormessage("Please select the document file");
                    return false;
                }
            }
            return true;
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

    <script type="text/javascript">

        function getProductFromLicense()
        {

            $.ajax({

                type: "POST",
                url: "UploadApk.aspx/getProductFromLicense",
                contentType: "application/json; charset=utf-8",
                dataType:"json",
                success: function (response) {
                   // debugger;
                    var data =JSON.parse(response.d);
                    $("#<%=lstProduct.ClientID %>").empty();
                    var i = 0;
                    $.each(data, function () {
                        $("#<%=lstProduct.ClientID %>").append('<option  value=' + this['ProductCode'] + ' > ' + this['productName'] + ' </option>'); 
                        i++;
                    })
                    console.log(i);
                    $("#<%=lstProduct.ClientID %>").multiselect('rebuild');

                }
            })

        }
        function BindCompany()
        {
            var selectedValues=[];
            $("#<%=lstProduct.ClientID %> :selected").each(function () {
                selectedValues.push("'"+$(this).val()+"'");
            })
            $("#<%=hiddenProduct.ClientID %>").val(selectedValues);
            console.log($("#<%=hiddenProduct.ClientID  %>").val());
           <%-- $.ajax({
                type: "POST",
                url: "UploadApk.aspx/getCompanyByProductFromLicense",
                data: '{ProductCode:"' + $("#<%=hiddenProduct.ClientID %>").val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = JSON.parse(response.d);
                    $("#<%=lstCompany.ClientID %>").empty();
                    $.each(data, function () {
                        $("#<%=lstCompany.ClientID %>").append('<option value=' + this['compcode'] + '>' + this['compname'] + '</option>');
                    })
                    $("#<%=lstCompany.ClientID %>").multiselect('rebuild');
                }
            })--%>
        }


    </script>


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
                                <h3 class="box-title">Upload Mobile App</h3>
                                <div style="float: right" hidden="hidden">
                                </div>
                                <asp:HiddenField ID="hid" runat="server" />

                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body">
                                <div class="form-group ">
                                    <div id="divdocid" runat="server" class="form-group">
                                        <label for="exampleInputEmail1">Document No:</label>
                                        <asp:TextBox ID="lbldocno" Enabled="false" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group ">
                                            <div runat="server" class="form-group">
                                                <label for="exampleInputEmail1">Product:</label><label for="requiredFields" style="color: red;">*</label>
                                                <asp:ListBox runat="server" ID="lstProduct" SelectionMode="Single" OnSelectedIndexChanged="lstProduct_SelectedIndexChanged"  AutoPostBack="true" onchange="BindCompany();"></asp:ListBox> 
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group ">
                                            <div runat="server" class="form-group">
                                                <label for="exampleInputEmail1">Company:</label><label for="requiredFields" style="color: red;">*</label>
                                                <asp:ListBox runat="server" ID="lstCompany" SelectionMode="Multiple" AutoPostBack="true"></asp:ListBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12" style="display:none;">
                                        <div class="form-group ">
                                            <label for="exampleInputEmail1">AppName: </label>
                                            &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:ListBox ID="ddlAppname" Width="100%" runat="server" class="form-control">
                                                <asp:ListItem Text="Goldiee_Distributor" Value="Goldiee_Distributor"></asp:ListItem>
                                                <asp:ListItem Text="Goldiee_Marketting" Value="Goldiee_Marketting"></asp:ListItem>
                                                <asp:ListItem Text="LalMahal_CRM" Value="LalMahal_CRM"></asp:ListItem>
                                            </asp:ListBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group ">
                                            <label for="exampleInputEmail1">Version Name: </label>
                                            &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtVersionName" runat="server" CssClass="form-control numeric" MaxLength="50"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group ">
                                            <label for="exampleInputEmail1">Version Code: </label>
                                            &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtversionCode" runat="server" CssClass="form-control numeric" MaxLength="50"></asp:TextBox>
                                        </div>
                                    </div>
                                  

                                </div>
                                <div class="row">
                                    <div class="col-md-1 col-sm-1 col-xs-12">
                                        <div class="form-group ">
                                            <label for="exampleInputEmail1">Active: </label>
                                            <asp:CheckBox ID="chk" runat="server" Checked="true" />
                                        </div>
                                    </div>
                                      <div class="col-md-2 col-sm-2 col-xs-12">
                                        <div class="form-group ">
                                            <label for="exampleInputEmail1">Upload: </label>
                                            &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:FileUpload ID="File1" runat="server" onchange="showpreview(this);" accept=".apk" />
                                            <asp:Label ID="fileLabel" runat="server" Text="Label" Style="display: none;"></asp:Label>
                                        </div>
                                    </div>
                                    <asp:HiddenField   ID="hiddenProduct" runat="server"  />
                                </div>
                            </div>


                            <div class="box-footer">
                                <asp:Button ID="btnsave" runat="server" Style="margin-top: 5px;" Text="Save" class="btn btn-primary" OnClientClick="return Valild();" OnClick="btnsave_Click" />

                            </div>

                            <div>
                                <asp:Repeater ID="rptapp" runat="server" OnItemCommand="rptapp_ItemCommand">
                                    <HeaderTemplate>
                                        <table id="tblApp" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <th style="text-align: left; width: 1px !important;">S.no</th>
                                                    <th style="text-align: left;">Date</th>
                                                    <th style="text-align: left;">Product Name</th>
                                                    <th style="text-align: left;">Company Name</th>
                                                    <th style="text-align: left;">Version Name</th>
                                                    <th style="text-align: left;">Version Code</th>
                                                      <th style="text-align: left;">Active</th>
                                                    <th style="text-align: left;">Download</th>
                                                </tr>                                              
                                            </thead>
                                           <tbody>                                           
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>  <td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>                              
                                          <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("DocDate"))%></td>
                                           
                                            <td><%#Eval("productname")%></td>
                                            <td><%#Eval("CompName")%></td>
                                            <td><%#Eval("versionname")%></td>
                                            <td><%#Eval("versionCode")%></td>
                                            <td><%#Eval("Active")%></td>
                                           <td><asp:LinkButton id="LinkButton1" runat="server" Text="Download APK" CommandName="Download" CommandArgument='<%#Eval("url")%>' /></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>

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
