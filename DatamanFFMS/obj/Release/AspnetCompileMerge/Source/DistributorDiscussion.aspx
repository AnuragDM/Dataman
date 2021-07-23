<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DistributorDiscussion.aspx.cs" EnableEventValidation="false" Inherits="AstralFFMS.DistributorDiscussion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
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

            $('#ContentPlaceHolder1_txtDistFrTime').timepicker({ 'timeFormat': 'H:i' });
            $('#ContentPlaceHolder1_txtDistToTime').timepicker({ 'timeFormat': 'H:i' });
            $('#ContentPlaceHolder1_txtNVTime').timepicker({ 'timeFormat': 'H:i' });
            $("#ContentPlaceHolder1_txtDistFrTime").keypress(function (event) { event.preventDefault(); });
            $("#ContentPlaceHolder1_txtDistToTime").keypress(function (event) { event.preventDefault(); });
            $("#ContentPlaceHolder1_txtNVTime").keypress(function (event) { event.preventDefault(); });

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
            
            if ($('#<%=txtRemark.ClientID%>').val() == "") {
                errormessage("Please enter the Remark.");
                return false;
            }
            if ($('#<%=txnextVisitDate.ClientID%>').val() == "") {
                errormessage("Please enter the Next Visit Date.");
                return false;
            }
            if ($('#<%=txtNVTime.ClientID%>').val() == '') {
                alert('NextVisitTime cannot be Blank');
                errormessage("Please enter NextVisitTime");
                return false;
            }

            if ($('#<%=txtDistFrTime.ClientID%>').val() == '') {
                alert('Distributor Start Time cannot be Blank');
                errormessage("Please enter Distributor StartTime");
                return false;
            }

            if ($('#<%=txtDistToTime.ClientID%>').val() == '') {
                alert('Distributor End Time cannot be Blank');
                errormessage("Please enter Distributor EndTime");
                return false;
            }
            var frmTime = $('#<%=DistFrTimeDDL.ClientID%>').val();
            var toTime = $('#<%=DistToTimeDDL.ClientID%>').val();
            var regExp = /(\d{1,2})\:(\d{1,2})\:(\d{1,2})/;
            var newval = parseInt(toTime.replace(regExp, "$1$2$3"))
            var newval1 = parseInt(frmTime.replace(regExp, "$1$2$3"))
            if (parseInt(toTime.replace(regExp, "$1$2$3")) < parseInt(frmTime.replace(regExp, "$1$2$3"))) {
                //alert("End time Should be greater than Start Time");
                errormessage("End time Should be greater than Start Time");
                return false;
            }
        }


    </script>

     <script type="text/javascript">
         function showpreview(input) {
             var uploadcontrol = document.getElementById('<%=dsrImgFileUpload.ClientID%>').value;
             var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
             if (uploadcontrol.length > 0) {
                 if (reg.test(uploadcontrol)) {
                     if (input.files && input.files[0]) {
                         var reader = new FileReader();
                         reader.onload = function (e) {
                             $("#ContentPlaceHolder1_imgpreview").css('display', 'block');
                             $("#ContentPlaceHolder1_imgpreview").attr('src', e.target.result);
                         }
                         reader.readAsDataURL(input.files[0]);
                     }
                 }
                 else {
                     errormessage("Only image files are allowed!");
                     $("#ContentPlaceHolder1_imgpreview").css('display', 'none');
                     return false;
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
                            <h3 class="box-title">Discussion with Distributor:</h3>
                            <div style="float: right">
                                <asp:Button Style="margin-right: 5px; margin-bottom: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary" OnClick="btnFind_Click" />
                                <asp:Button Style="margin-right: 5px; margin-bottom: 5px;" type="button" ID="Back" runat="server" Text="Back" class="btn btn-primary" OnClick="Back_Click" />
                            </div>                    

                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                              <div class="row">
                              <%-- <h3>Distributor Discussion with Distributor</h3>--%>
                            <div class="col-lg-5 col-md-7 col-sm-7 col-xs-11">
                                <div class="form-group" style="display:none;">
                                    <label for="exampleInputEmail1">Reason:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlreason" Width="100%" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                </div>
                                <div id="divdocid" runat="server" class="form-group">
                                    <label for="exampleInputEmail1">Document No:</label>
                                    <asp:TextBox ID="lbldocno" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div>
                                <label for="exampleInputEmail1">Time Spent with Distributor -</label>
                                 </div>
                               
                                <div class="form-group col-md-6 paddingleft0">                                    
                                    <label for="exampleInputEmail1">Start Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <%--<input type="text" maxlength="7" data-scroll-default="6:00am" placeholder="--Select Time--" class="form-control" id="Text1" runat="server" autocomplete="off">--%>
                                     <asp:DropDownList ID="DistFrTimeDDL" runat="server" CssClass="form-control" Width="100%"></asp:DropDownList>
                                     <input type="text" data-scroll-default="6:00am" maxlength="7" width="60%" placeholder="--Select Time--" Class="form-control"  id="txtDistFrTime" runat="server" autocomplete="off" visible="false">
                                </div>
                                <div class="form-group col-md-6 paddingleft0 paddingright0">
                                    <label for="exampleInputEmail1">End Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <%--<input type="text" maxlength="7" data-scroll-default="6:00am" placeholder="--Select Time--" class="form-control" id="Text2" runat="server" autocomplete="off">--%>
                                    <asp:DropDownList ID="DistToTimeDDL" runat="server" CssClass="form-control" style="width:100%;"></asp:DropDownList>
                                    <input type="text" data-scroll-default="6:00am" maxlength="7" width="60%" placeholder="--Select Time--" Class="form-control"  id="txtDistToTime" runat="server" autocomplete="off" visible="false">
                                </div>
                                <div class="form-group col-md-6 paddingleft0">
                                    <label for="exampleInputEmail1">Next Visit Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:TextBox ID="txnextVisitDate" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="calendarTextBox_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender"
                                        TargetControlID="txnextVisitDate"></ajaxToolkit:CalendarExtender>
                                </div>

                                <div class="form-group col-md-6 paddingleft0 paddingright0">
                                    <label for="exampleInputEmail1">Next Visit Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <%--<input type="text" maxlength="7" data-scroll-default="6:00am" placeholder="--Select Time--" class="form-control" id="basicExample" runat="server" autocomplete="off">--%>
                                    <asp:DropDownList ID="NVTimeDDL" runat="server" CssClass="form-control" style="width:100%;"></asp:DropDownList>
                                    <input type="text" data-scroll-default="6:00am" maxlength="7" width="90%" placeholder="--Select Time--" Class="form-control"  id="txtNVTime" runat="server" autocomplete="off" visible="false">
                                </div>
                                 <div class="form-group col-md-6 paddingleft0 paddingright3">
                                    <input id="DepId" hidden="hidden" />
                                    <label for="exampleInputEmail1">Remark:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:TextBox ID="txtRemark" runat="server" Style="resize: none; height: 80%;" Width="100%" TextMode="MultiLine" class="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-6 paddingleft0 paddingright0">
                                    <label for="exampleInputEmail1">Image:</label>
                                    <asp:FileUpload ID="dsrImgFileUpload" runat="server" onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                       ControlToValidate ="comImgFileUpload" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic"> 
                                    </asp:RegularExpressionValidator>
                                   <img id="imgpreview" height="200" width="200"   src="" style="border-width: 0px; display: none;" runat="server" />
                               </div>
                               <div class="form-group col-md-6 paddingleft0 paddingright0">
                                  <tr><td colspan="2">                      
                                  <b>Note : Image size should be less than 1MB</b>
                                  </td></tr>
                              </div>
                                </div>
                            </div>
                        </div>
                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm()" OnClick="btnDelete_Click" />
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
                            <h3 class="box-title">Distributor Discussion List</h3>
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
                                                <th>Start Time.</th>
                                                <th>End Time.</th>
                                                <th>Next Visit Date</th>  
                                                <th>Next Visit Time.</th>                                            
                                                <th>Remark</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>  
                                    <tr onclick="DoNav('<%#Eval("VisDistId") %>');">                                 
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("VisDistId") %>' />
                                        <td><%#Eval("SpentfrTime") %></td>
                                        <td><%#Eval("SpentToTime") %></td>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}",Eval("NextVisitDate")) %></td>
                                        <td><%#Eval("NextVisitTime") %></td>                                       
                                        <td><%#Eval("RemarkDist") %></td>
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
        $(function () {
            $("#example1").DataTable();

        });
    </script>
</asp:Content>
