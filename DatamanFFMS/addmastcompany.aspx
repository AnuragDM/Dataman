<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="addmastcompany.aspx.cs" Inherits="AstralFFMS.addmastcompany" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style type="text/css">
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
    <script>
        function isNumber(evt) {

            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (!(iKeyCode != 8)) {
                s
                e.preventDefault();
                return false;
            }
            return true;
        }

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



            if ($('#<%=Compname.ClientID%>').val() == "") {
                errormessage("Please Enter Company Name");
                return false;
            }
            if ($('#<%=Desc.ClientID%>').val() == "") {
                errormessage("Please Enter Description ");
                return false;
            }
            if ($('#<%=Phone.ClientID%>').val() == "") {
                errormessage("Please Enter Phone No");
                return false;
            }
            if ($('#<%=Url.ClientID%>').val() == "") {
                errormessage("Please Enter Company Url");
                return false;
            }
            if ($('#<%=add.ClientID%>').val() == "") {
                errormessage("Please Enter Company Add");
                return false;
            } 
           
            var btn = document.getElementById("<%= ddlcountry %>").value;
            if (btn="") {
                errormessage("Please Select Country");
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
        function DoNav(Tag_Id) {
            if (Tag_Id != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', Tag_Id)
            }
        }
    </script>

    <section class="content">
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
        </div>
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body" id="mainDiv" style="display: none;" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <%--<h3 class="box-title">Add Company </h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3> 
                            <div style="float: right">
                                <%-- <asp:Button Style="margin-right: 5px;" type="button" ID="btnback" runat="server" Text="Back" class="btn btn-primary"
                                    OnClick="btnback_Click" Visible="false" />--%>
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                    OnClick="btnFind_Click" />

                                <%--  <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" runat="server" />--%>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                                <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Company Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="30" id="Compname" placeholder="Enter Company Name" tabindex="2" />

                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Description:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="30" id="Desc" placeholder="Enter Description" tabindex="2" />

                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Phone:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control numeric text-left phonetext" maxlength="13" id="Phone" placeholder="Enter Phone" tabindex="2" />
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Url:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="75" id="Url" placeholder="Enter Url" tabindex="2" />
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Address:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="50" id="add" placeholder="Enter Address" tabindex="5" />
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1"></label>
                                        <input type="text" runat="server" class="form-control" maxlength="25" id="City" placeholder="Enter City" tabindex="5" />
                                        <br />
                                        <input type="text" runat="server" class="form-control" maxlength="25" id="State" placeholder="Enter State" tabindex="5" />
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1"></label>
                                        <input type="text" runat="server" class="form-control numeric text-left phonetext" maxlength="6" id="Zip" onkeypress="javascript:return isNumber (event)" placeholder="Enter Zip" tabindex="5" />
                                        <br />
                                        <asp:DropDownList ID="ddlcountry" runat="server" Width="100%" CssClass="form-control" TabIndex="7"></asp:DropDownList>
                                    </div>


                                    <div class="clearfix" style="padding-bottom: 10px;"></div>

                                    <div id="divCF" class="form-group">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClientClick="javascript:return validate();" TabIndex="28" OnClick="btnSave_Click" />
                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" TabIndex="30" OnClick="btnCancel_Click" />
                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Visible="false" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" TabIndex="32" OnClick="btnDelete_Click" />
                    </div>
                    <br />
                    <div>
                        <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                    </div>
                </div>
            </div>


        </div>



        <div class="box-body" id="rptmain" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Company List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnback_Click" />

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>

                                                <th>Company Name</th>
                                                <th>Description</th>
                                                <th>Phone</th>
                                                 <th>Url</th>
                                                 <th>Address</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("id") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("id") %>' />
                                        <td><%#Eval("CompName") %></td>
                                        <td><%#Eval("CompDesc") %></td>
                                        <td><%#Eval("CompPhone") %></td>
                                         <td><%#Eval("CompUrl") %></td>
                                         <td><%#Eval("CompAdd") %></td>

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
            $("#example1").DataTable();

        });
    </script>

</asp:Content>

