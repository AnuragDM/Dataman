<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MastHeadquarter.aspx.cs" Inherits="AstralFFMS.MastHeadquarter" EnableEventValidation="false" %>
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
            if ($('#<%=HeadQuarterName.ClientID%>').val() == "") {
                errormessage("Please enter Headquarter Name.");
                return false;
            }
            if ($('#<%=Address.ClientID%>').val() == "") {
                errormessage("Please enter Address.");
                return false;
            }

            if ($('#<%=ddlCountry.ClientID%>').val() == "0" || $('#<%=ddlCountry.ClientID%>').val() == "") {
                errormessage("Please Select Country");
                return false;
            }

            if ($('#<%=ddlState.ClientID%>').val() == "0" || $('#<%=ddlState.ClientID%>').val() == "") {
                errormessage("Please Select State");
                return false;
            }

            if ($('#<%=ddlCity.ClientID%>').val() == "0" || $('#<%=ddlCity.ClientID%>').val() == "") {
                errormessage("Please Select City");
                return false;
            }

            if ($('#<%=txtPincode.ClientID%>').val() == "") {
                errormessage("Please enter Pincode.");
                return false;
            }

            var pincode = $('#<%=txtPincode.ClientID%>').val();
            if (pincode.length < 6)
            {
                errormessage("Enter valid Pincode.");
                return false;
            }


            var value = ($('#<%=HeadQuarterName.ClientID%>').val().charAt(0));
            var chrcode = value.charCodeAt(0);
            if ((chrcode < 97 || chrcode > 122) && (chrcode < 65 || chrcode > 90)) {
                errormessage("Do not start Name with special characters.")
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            var valLength = "";
            $('#<%=HeadQuarterName.ClientID%>').keypress(function (key) {

                valLength = ($('#<%=HeadQuarterName.ClientID%>').val().length + 1);

                if (valLength < 2) {
                    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && key.charCode != 32) return false;
                }
                else {
                    return true;
                }
            });
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
    <script type="text/javascript">
        function DoNav(Id) {
            if (Id != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();
                __doPostBack('', Id)
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
                            <h3 class="box-title">Headquarter Details</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary"
                                    OnClick="btnBack_Click" tabindex="10"/>

                                <%--  <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" runat="server" />--%>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                            <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                <div class="form-group">
                                    <input id="DepId" hidden="hidden" />
                                    <label for="exampleInputEmail1">Headquarter Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input runat="server" type="text" class="form-control" maxlength="100" id="HeadQuarterName" placeholder="Enter Headquarter Name" tabindex="1">
                                </div>
                            </div></div>
                             <div class="row">
                            <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                <div class="form-group">
                                    
                                    <label for="exampleInputEmail1">Address:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input runat="server" type="text" class="form-control" maxlength="150" id="Address" placeholder="Enter Address" tabindex="2">
                                </div>
                            </div></div>

                             <div class="row">
                            <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                <div class="form-group">
                                    
                                    <label for="exampleInputEmail1">Country:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                     <asp:DropDownList ID="ddlCountry" runat="server" Width="100%" CssClass="form-control" TabIndex="3" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                            </div></div>
                            

                            <div class="row">
                            <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                <div class="form-group">
                                    
                                    <label for="exampleInputEmail1">State:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                     <asp:DropDownList ID="ddlState" runat="server" Width="100%" CssClass="form-control" TabIndex="4" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                            </div></div>

                             <div class="row">
                            <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                <div class="form-group">
                                    
                                    <label for="exampleInputEmail1">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                     <asp:DropDownList ID="ddlCity" runat="server" Width="100%" CssClass="form-control" TabIndex="5"></asp:DropDownList>
                                </div>
                            </div></div>

                            <div class="row">
                            <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                <div class="form-group">
                                    
                                    <label for="exampleInputEmail1">Pincode:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <input runat="server" type="text" class="form-control numeric" maxlength="6" id="txtPincode" placeholder="Enter Pincode" tabindex="6">
                                </div>
                            </div></div>

                        </div>
                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate(); "  tabindex="7"/>
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" tabindex="8"/>
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" OnClick="btnDelete_Click" tabindex="9"/>
                        </div>
                          <br />
                           <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
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
                            <h3 class="box-title">Headquarter List</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnAdd" runat="server" Text="Add Headquarter" class="btn btn-primary" OnClick="btnAdd_Click" tabindex="14"/>

                            </div>
                        </div>
                        <!-- /.box-header -->

                        <div class="box-body" style="display:none;">
                            <div class="col-lg-9 col-md-9 col-sm-7 col-xs-9">
                                <div class="col-md-12 paddingleft0">
                              

                             <div class="form-group col-md-5">   
                                 <label for="exampleInputEmail1">Headquarter :</label>                                              
                                 <asp:DropDownList ID="ddlHeadquarterlist" runat="server" CssClass="form-control"  TabIndex="11"></asp:DropDownList>
                                 <asp:HiddenField ID="HiddenHeadquarterlist" runat="server" />
                                     </div>
                                
                             
                             
                           </div>

                                 <div class="col-md-12 paddingleft0">
                              

                      
                             
                              <div class="form-group col-md-6">
                                        <label for="exampleInputEmail1" style="display: block; visibility: hidden">zkjfhksj</label>
                                        <asp:Button type="button" ID="Button1" runat="server" Style="padding: 3px 14px;" Text="Go" class="btn btn-primary go-button-dsr" OnClick="btnSearch_Click"   TabIndex="12"/>
                                  <asp:Button type="button" ID="Button2" runat="server" Style="padding: 3px 14px;" Text="Reset" class="btn btn-primary go-button-dsr" OnClick="btnResetSearch_Click"   TabIndex="13"/>
                                  
                                    </div>
                                     
                           </div>
                       </div>
                </div>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                 
                                                <th>Headquarter</th>
                                                  <th>Address</th>
                                                <th>Country</th>
                                                <th>State</th>
                                                <th>City</th>
                                                  
                                                <th>Pincode</th>
                                                <th>Action</th>
                                                 
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("Id") %>' />
                                    
                                        <td><%#Eval("Name") %></td>
                                        <td><%#Eval("Address") %></td>
                                         <td><%#Eval("CountryName") %></td>
                                        <td><%#Eval("StateName") %></td>
                                          <td><%#Eval("CityName") %></td>
                                        
                                        <td><%#Eval("Pincode") %></td>
                                        <td><a href="#"  data-toggle="tooltip" title="Edit" onclick="DoNav('<%#Eval("Id") %>');" >Edit</a></td>
                                       
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
