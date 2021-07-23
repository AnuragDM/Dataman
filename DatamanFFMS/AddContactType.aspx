<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="AddContactType.aspx.cs" Inherits="AstralFFMS.AddContactType" %>

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
            
           

            if ($('#<%=Contactstype.ClientID%>').val() == "") {
                errormessage("Please Enter Contact Type");
                return false;
            }
            var value = ($('#<%=Contactstype.ClientID%>').val().charAt(0));
            var chrcode = value.charCodeAt(0);
            if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                errormessage("Do not start Name with special characters")
                return false;
            }

            if ($('#<%=Txtsort.ClientID%>').val() == "") {
                errormessage("Please Enter Sorting No.");
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
                            <%--<h3 class="box-title">Contacts Type Master</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            <div style="float: right">
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
                                           <label >Contacts Group:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;"></label>
                                          <asp:DropDownList runat="server" ID="ddlphonetype1" class="form-control" >
                                              <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                                              <asp:ListItem Text="Phone" Value="Phone"></asp:ListItem>
                                              <asp:ListItem Text="Url" Value="Url"></asp:ListItem>
                                            
                                        </asp:DropDownList>
                                      </div>
                                              
                                      
                                          
                                    
                                        <div class="form-group">
                                        <label >Contacts Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server" class="form-control" maxlength="15" id="Contactstype" placeholder="Enter Contact Type" tabindex="2" />
                                         
                                    </div>
                                          
                                          <div class="form-group">
                                        <label >Sorting Order</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <input type="text" runat="server"  Class="form-control numeric text-right" maxlength="3" id="Txtsort" placeholder="Enter Sorting No." onkeypress="javascript:return isNumber (event)" tabindex="2" />
                                         
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
                            <h3 class="box-title">Contact Type List</h3>
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

                                                <th>Contact Group</th>
                                                <th>Contact Type</th>
                                                 <th>Sorting Order</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("id") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("id") %>' />
                                        <td><%#Eval("Value") %></td>
                                        <td><%#Eval("Data") %></td>
                                        <td><%#Eval("Sort") %></td>
                                       
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
