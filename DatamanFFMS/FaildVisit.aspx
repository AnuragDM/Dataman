<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FaildVisit.aspx.cs" Inherits="AstralFFMS.FaildVisit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
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
        $(document).ready(function () {

            $('#ContentPlaceHolder1_basicExample').timepicker();

        });
    </script>
    <script type="text/javascript">
        $(function () {
            //Initialize Select2 Elements
            $(".select2").select2();
        });
    </script>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            /*display: table;*/
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
            if ($('#<%=ddlreason.ClientID%>').val() == "0") {
                errormessage("Please select the Reason.");
                return false;
            }
            if ($('#<%=txtRemark.ClientID%>').val() == "") {
                errormessage("Please enter the Remark.");
                return false;
            }
            if ($('#<%=txnextVisitDate.ClientID%>').val() == "") {
                errormessage("Please enter the Next Visit Date.");
                return false;
            }
        }


    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            document.getElementById("DivDate").style.display = "";
            document.getElementById("DivChNo").style.display = "";
            document.getElementById("DivBank").style.display = "";
            document.getElementById("DivBranch").style.display = "";

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
        function DoNav(depId) {
            if (depId != "") {
                document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                $('#spinner').show();

                __doPostBack('', depId)
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
                    <div class="box box-primary">
                        <div class="box-header">
                            <div class="box-header with-border">

                                <div class="form-group">

                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                    <div class="col-sm-12">
                                        <div class="col-lg-4 col-md-5 col-sm-8 paddingleft0">
                                            <asp:Label ID="partyName" runat="server" CssClass="text" Text="Label"></asp:Label><br />
                                            <asp:Label ID="address" runat="server" CssClass="text" Text="Label"></asp:Label>,&nbsp;
                                     <asp:Label ID="lblzipcode" runat="server" CssClass="text" Text=""></asp:Label>,&nbsp;
                                     <asp:Label ID="mobile" runat="server" CssClass="text" Text="Label"></asp:Label>&nbsp;
                                        </div>
                                        <div class="col-lg-2 col-md-2 col-sm-8 paddingleft0">
                                            <asp:Label ID="lblVisitdate1" runat="server" CssClass="text" Text="Visit Date"></asp:Label><br />
                                            <asp:Label ID="lblVisitDate5" runat="server" CssClass="text" Text="Visit Date"></asp:Label>&nbsp;
                                        </div>
                                        <div class="col-lg-2 col-md-2 col-sm-8 paddingleft0">
                                            <asp:Label ID="lblAreaName1" runat="server" CssClass="text" Text="Area Name"></asp:Label><br />
                                            <asp:Label ID="lblBeatName5" runat="server" CssClass="text" Text="Beat Name"></asp:Label>&nbsp;
                                        </div>
                                        <div class="col-lg-2 col-md-2 col-sm-8 paddingleft0">
                                            <asp:Label ID="lblBeatName1" runat="server" CssClass="text" Text="Beat Name"></asp:Label><br />
                                            <asp:Label ID="lblAreaName5" runat="server" CssClass="text" Text="Area Name"></asp:Label>&nbsp;
                                        </div>

                                        <div class="col-lg-2 col-md-1 col-sm-8 paddingleft0" style="float: right">

                                            <asp:Button Style="margin-right: 5px; margin-bottom: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                                OnClick="btnFind_Click" />
                                            <asp:Button Style="margin-right: 5px; margin-bottom: 5px;" type="button" ID="Back" runat="server" Text="Back" class="btn btn-primary"
                                                OnClick="Back_Click" />

                                            <asp:HiddenField ID="hid" runat="server" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <div class="col-sm-4 paddingleft0">
                                        </div>
                                        <div class="col-sm-2 paddingleft0">
                                        </div>
                                        <div class="col-sm-2 paddingleft0">
                                        </div>
                                        <div class="col-sm-2 paddingleft0">
                                        </div>

                                    </div>
                                </div>
                            </div>
                         
                           <%-- <div style="float: right">--%>


                                <%--  <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" runat="server" />--%>
                      <%--      </div>--%>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body" style="margin-top:-40px !important;">
                               <h3>Failed Visit</h3>
                            <div class="col-md-5 col-sm-6 col-xs-7">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Reason:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlreason" Width="100%" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                </div>
                                <div id="divdocid" runat="server" class="form-group">
                                    <label for="exampleInputEmail1">Document No:</label>
                                    <asp:TextBox ID="lbldocno" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <input id="DepId" hidden="hidden" />
                                    <label for="exampleInputEmail1">Remark:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:TextBox ID="txtRemark" runat="server" Style="resize: none; height: 80%;" TextMode="MultiLine" class="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-6 paddingleft0">
                                    <label for="exampleInputEmail1">Next Visit Date:</label>
                                    <asp:TextBox ID="txnextVisitDate" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="calendarTextBox_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender"
                                        TargetControlID="txnextVisitDate"></ajaxToolkit:CalendarExtender>
                                </div>

                                <div class="form-group col-md-6 paddingleft0 paddingright0">
                                    <label for="exampleInputEmail1">Next Visit Time:</label>
                                    <input type="text" maxlength="7" data-scroll-default="6:00am" placeholder="--Select Time--" class="form-control" id="basicExample" runat="server" autocomplete="off">
                                </div>

                            </div>
                        </div>
                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                            <%--  <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" OnClick="btnDelete_Click" />--%>
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
                            <h3 class="box-title">Faild Visit List</h3>
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
                                                <th>Document No.</th>
                                                <th>Next Visit Date</th>
                                                <th>Reason</th>
                                                <th>Remark</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("FVId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("FVId") %>' />
                                        <td><%#Eval("FVDocId") %></td>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}",Eval("Nextvisit")) %></td>
                                        <td><%#Eval("FVName") %></td>
                                        <td><%#Eval("Remarks") %></td>
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
