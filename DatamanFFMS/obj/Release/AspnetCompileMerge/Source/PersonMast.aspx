<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PersonMast.aspx.cs" Inherits="PersonMast" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<%@ Register Src="ctlCalendar.ascx" TagName="Calendar" TagPrefix="ctl" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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

        @media (max-width: 600px) {
            .formlay {
                width: 100% !important;
            }
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
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
            if (document.getElementById("<%=Txt_PersonName.ClientID%>").value == "" || document.getElementById("<%=Txt_PersonName.ClientID%>").value == "0") {
                 errormessage("Person Name field cannot be blank or Zero");
                 document.getElementById("<%=Txt_PersonName.ClientID%>").focus();
                return false;
            }
            var regAlpha = /[a-zA-Z ]*$/;
            var Pname = document.getElementById("<%=Txt_PersonName.ClientID %>").value;
            var ValidPname = Pname.match(regAlpha);
            if (ValidPname == "") {
                errormessage("Enter only alphabets");
                document.getElementById("<%=Txt_PersonName.ClientID%>").focus();
                return false;
            }

           <%-- if (document.getElementById("<%=txtName.ClientID%>").value == "" || document.getElementById("<%=txtName.ClientID%>").value == "0") {
                 errormessage("Device No field cannot be blank or Zero");
                 document.getElementById("<%=txtName.ClientID%>").focus();
                return false;
            }--%>

            if (document.getElementById("<%=Txt_FromTime.ClientID%>").value == "" || document.getElementById("<%=Txt_FromTime.ClientID%>").value == "" || document.getElementById("<%=Txt_FromTime.ClientID%>").value == "00:00") {
                 errormessage("Time is in not valid Format");
                 document.getElementById("<%=Txt_FromTime.ClientID%>").focus();
                return false;
            }
            var digits = /^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$/;
            var digitsid = document.getElementById("<%=Txt_FromTime.ClientID %>").value;
            var digitsArray = digitsid.match(digits);
            var temp;
            if (digitsArray == null) {
                errormessage("Time Format(HH:MM) seems incorrect. Please try again.");
                document.getElementById("<%=Txt_FromTime.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=Txt_ToTime.ClientID%>").value == "" || document.getElementById("<%=Txt_ToTime.ClientID%>").value == "" || document.getElementById("<%=Txt_ToTime.ClientID%>").value == "00:00") {
                 errormessage("Time is in not valid Format");
                 document.getElementById("<%=Txt_ToTime.ClientID%>").focus();
                return false;
            }
            var digits = /^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$/;
            var digitsid = document.getElementById("<%=Txt_ToTime.ClientID %>").value;
            var digitsArray = digitsid.match(digits);
            var temp;
            if (digitsArray == null) {
                errormessage("Time Format(HH:MM) seems incorrect. Please try again.");
                document.getElementById("<%=Txt_ToTime.ClientID %>").focus();
                return false;
            }
            var start = document.getElementById("<%=Txt_FromTime.ClientID %>").value;
             var end = document.getElementById("<%=Txt_ToTime.ClientID %>").value;
             var dtStart = new Date("1/1/2007 " + start);
             var dtEnd = new Date("1/1/2007 " + end);
             if (Date.parse(dtStart) > Date.parse(dtEnd)) {
                 errormessage("To Time Should be greater then From Time");
                 document.getElementById("<%=Txt_ToTime.ClientID %>").focus();
             return false;
         }

         if (document.getElementById("<%=Txt_min.ClientID%>").value == "" || document.getElementById("<%=Txt_min.ClientID%>").value == "0") {
                 errormessage("Record Interval field cannot be blank or Zero");
                 document.getElementById("<%=Txt_min.ClientID%>").focus();
                    return false;
                }
          
             if (parseInt(document.getElementById("<%=Txt_Upload.ClientID%>").value) < 5) {
                 errormessage("Upload Interval Should be minimum of 5 Min");
                 return false;
             }
             var valueUpld = parseInt(document.getElementById("<%=Txt_Upload.ClientID%>").value);

             if (document.getElementById("<%=Txt_Upload.ClientID%>").value == "" || document.getElementById("<%=Txt_Upload.ClientID%>").value == "0") {
                 errormessage("Upload Interval field cannot be blank or Zero");
                 document.getElementById("<%=Txt_Upload.ClientID%>").focus(); return false;
                }
            

             if (document.getElementById("<%=txtdegree.ClientID%>").value == "" || document.getElementById("<%=txtdegree.ClientID%>").value == "0") {
                 errormessage("Degree cannot be blank or zero");
                 document.getElementById("<%=txtdegree.ClientID%>").focus(); return false;
                }

         
             return true;
         }

    </script>
     
     <script type="text/javascript">
         function chkTime(val) {
             var isValid = false;
             if (val == "00:00")
                 errormessage("Invalid Time");
             else isValid = true;
             return isValid;
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
        

        <div class="box-body" id="mainDiv"  runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12 col-sm-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Person Master</h3>
                            <div style="float: right">
                              
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnback" OnClick="btnback_Click" runat="server" Text="Back" class="btn btn-primary" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                   
                        <asp:UpdatePanel ID="update" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <ContentTemplate>
                        <div class="box-body">
                            <div class="col-md-6 col-sm-8">
                             
                     <div class="form-group" >
                            <label for="requiredfield" class="back">*</label>
                            <label for="Person Name" >Person Name :</label>
                            <asp:TextBox ID="Txt_PersonName" runat="server" MaxLength="30" placeholder="Please Enter Person Name."
                          CssClass="textbox form-control" Width="100%" ></asp:TextBox>
                        </div>
                        <div class="form-group" >
                            <label for="requiredfield" class="back">*</label>
                            <label for="Mobile Number" >Mobile Number :</label>
                            <asp:TextBox ID="txtmblnumber" runat="server" MaxLength="10" placeholder="Please Enter Mobile Number."
                          CssClass="textbox form-control" Width="100%" ></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtmblnumber">
                        </ajax:FilteredTextBoxExtender>
                        </div>
                        <div class="form-group" >
                            <label for="requiredfield" class="back">*</label>
                            <label for="Senior Mobile Number" >Senior Mobile Number :</label>
                            <asp:TextBox ID="txtsrmblnumber" runat="server" MaxLength="10" placeholder="Please Enter Senior Mobile Number."
                          CssClass="textbox form-control" Width="100%" ></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender6"
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtsrmblnumber">
                        </ajax:FilteredTextBoxExtender>
                        </div>
                        <div class="form-group" >
                            <label for="requiredfield" class="back">*</label>
                            <label for="Device No" >Device No :</label>
                             <asp:TextBox ID="txtName" runat="server" CssClass="textbox form-control" MaxLength="30" placeholder="Please Enter Device No." Enabled="false" Width="100%"></asp:TextBox>
                        </div>
                        <div class="form-group" >
                            <%--<label for="requiredfield" class="back">*</label>--%>
                            <label for="Device No" >Employee Code :</label>
                            <asp:TextBox ID="txtempcode" runat="server" CssClass="textbox form-control" MaxLength="10" placeholder="Please Enter Employee Code." Width="100%"></asp:TextBox>
                        </div>
                        <div class="form-group" >
                            <label for="requiredfield" class="back">*</label>
                            <label for="From Time" >From Time :</label>
                            <asp:TextBox ID="Txt_FromTime" runat="server" MaxLength="5" CssClass="textbox form-control" OnChange="chkTime(this.value);" Width="100%"></asp:TextBox>
                        </div>
                        <div class="form-group" >
                            <label for="requiredfield" class="back">*</label>
                            <label for="To Time" >To Time :</label>
                            <asp:TextBox ID="Txt_ToTime" runat="server" CssClass="textbox form-control" MaxLength="5" OnChange="chkTime(this.value);" Width="100%"></asp:TextBox>
                        </div>
                        <div class="form-group" >
                            <label for="requiredfield" class="back">*</label>
                            <label for="To Time" >Record Interval (In Seconds):</label>
                            <asp:TextBox ID="Txt_min" runat="server" CssClass="textbox form-control" MaxLength="4" placeholder="" Width="100%"></asp:TextBox>
                        <ajax:FilteredTextBoxExtender ID="txtMobile_FilteredTextBoxExtender"
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="Txt_min">
                        </ajax:FilteredTextBoxExtender>
                        </div>
                        <div class="form-group" >
                            <label for="requiredfield" class="back">*</label>
                            <label for="Upload Interval" >Upload Interval (In Minutes):</label>
                            <asp:TextBox ID="Txt_Upload" runat="server" Text="30" CssClass="textbox form-control" MaxLength="3" placeholder="Enter Min" Width="100%"></asp:TextBox>
                        <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="Txt_Upload">
                        </ajax:FilteredTextBoxExtender>
                        </div>
                        <div class="form-group" style="display:none">
                            <label for="requiredfield" class="back">*</label>
                            <label for="GPS Retry Interval" >GPS Retry Interval :</label>
                            <asp:TextBox ID="Txt_retry" runat="server" CssClass="textbox form-control" Text="0" MaxLength="3" placeholder="Enter Min" Style="text-align: left" Width="100%"></asp:TextBox>
                        <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="Txt_retry">
                        </ajax:FilteredTextBoxExtender>
                        </div>
                        <div class="form-group">
                            <label for="requiredfield" class="back">*</label>
                            <label for=" Accuracy" >Accuracy :</label>
                            <asp:TextBox ID="txtdegree" runat="server" CssClass="textbox form-control" Text="200" 
                            MaxLength="4" placeholder="Degree" Style="text-align: left" Width="100%"></asp:TextBox>
                        <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtdegree">
                        </ajax:FilteredTextBoxExtender>
                        </div>
                        <div class="form-group">
                           <%-- <label for="requiredfield" class="back">*</label>--%>
                            <label for="Latitude" >Latitude :</label>
                            <asp:TextBox ID="txtlat" runat="server" CssClass="textbox form-control"  
                            MaxLength="15" placeholder="Latitude" Style="text-align: left" Width="100%"></asp:TextBox>
                        <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender7"
                            runat="server" Enabled="True" FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtlat">
                        </ajax:FilteredTextBoxExtender>
                        </div>
                         <div class="form-group">
                            <%--<label for="requiredfield" class="back">*</label>--%>
                            <label for="Longitude" >Longitude :</label>
                             <asp:TextBox ID="txtlong" runat="server" CssClass="textbox form-control"  
                            MaxLength="15" placeholder="Longitude" Style="text-align: left" Width="100%"></asp:TextBox>
                        <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" 
                            runat="server"  FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtlong">
                        </ajax:FilteredTextBoxExtender>
                        </div>
                        <div class="form-group" style="display:none;">
                           <%-- <label for="requiredfield" class="back">*</label>--%>
                            <label for="Send Alarm" >Send Alarm :</label>
                            <asp:DropDownList ID="ddlsendAlarm" CssClass="textbox locationSelect" runat="server" Width="100%" AutoPostBack="true" 
                            onselectedindexchanged="ddlsendAlarm_SelectedIndexChanged" >
                       <asp:ListItem Selected="True" Text="Yes" Value="Y"></asp:ListItem>
                       <asp:ListItem Text="No" Value="N"></asp:ListItem>
                       </asp:DropDownList>
                       &nbsp;<asp:TextBox ID="txtalarmmins" placeholder="Duration" runat="server" Width="80px" Visible="true"></asp:TextBox>
                       <span id="span_min" runat="server">(Min.)</span><ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtalarmmins">
                        </ajax:FilteredTextBoxExtender>
                        </div>
                        <div class="form-group" style="display:none;">
                          <%--  <label for="requiredfield" class="back">*</label>--%>
                            <label for="Send SMS" >Send SMS :</label>
                            <asp:DropDownList ID="ddlsendSMS" CssClass="textbox locationSelect form-control" runat="server"  Width="100%" >
                       <asp:ListItem Selected="True" Text="Yes" Value="Y"></asp:ListItem>
                       <asp:ListItem Text="No" Value="N"></asp:ListItem>
                       </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <%--<label for="requiredfield" class="back">*</label>--%>
                            <label for="Send SMS To Person" >Send SMS To Person :</label>
                            <asp:DropDownList ID="ddlsendsmsP" CssClass="textbox locationSelect form-control" runat="server"  Width="100%" >
                       <asp:ListItem Selected="True" Text="Yes" Value="Y"></asp:ListItem>
                       <asp:ListItem Text="No" Value="N"></asp:ListItem>
                       </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <%--<label for="requiredfield" class="back">*</label>--%>
                            <label for="Send SMS To Senior" >Send SMS To Senior :</label>
                            <asp:DropDownList ID="ddlsendsmsS" CssClass="textbox locationSelect form-control" runat="server"  Width="100%" >
                       <asp:ListItem Selected="True" Text="Yes" Value="Y"></asp:ListItem>
                       <asp:ListItem Text="No" Value="N"></asp:ListItem>
                       </asp:DropDownList>
                        </div>
                        <div class="form-group" style="display:none;">
                            <label for="requiredfield" class="back">*</label>
                            <label for="System Log" >System Log :</label>
                            <asp:CheckBox ID="chkSys_Flag" runat="server" />
                        </div>
                        <div class="form-group">
                            <label for="requiredfield" class="back"></label>
                            <label for="System Log" ></label>
                            <asp:CheckBox ID="chk1" runat="server" Checked="true" Text="GPS Location" 
                            Visible="False"   />
                        <asp:CheckBox ID="Chk2" runat="server" Checked="true" Text="Mobile Location" 
                            Visible="False"  />
                        </div>
                               </div>
                        </div>
                                </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="box-footer">
                           <asp:HiddenField ID="hdfCode" runat="server" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClientClick="javascript:return validate();" OnClick="btnSave_Click" TabIndex="52" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" TabIndex="54" OnClick="btnCancel_Click" />
                       <div class="clearfix"></div>   
                            
                             <div class="note">
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                              
                        </div>
                       
                    </div>
                </div>
            </div>
        </div>
    </section>
   
    
  
</asp:Content>

