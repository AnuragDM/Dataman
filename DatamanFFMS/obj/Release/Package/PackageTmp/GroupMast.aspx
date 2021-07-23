<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.master" AutoEventWireup="true" Inherits="GroupMast" Codebehind="GroupMast.aspx.cs" %>
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
            if ($('#<%=txtType.ClientID%>').val() == "") {
                errormessage("Please enter Group Name");
                return false;
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
        

        <div class="box-body" id="mainDiv"  runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">Group Master</h3>
                            <div style="float: right">
                              
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnback" OnClick="btnback_Click" runat="server" Text="Back" class="btn btn-primary" />
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                   
                        <asp:UpdatePanel ID="update" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <ContentTemplate>
                        <div class="box-body">
                            <div class="col-md-6">
                             
                <div class="form-group col-md-6 col-sm-6 col-xs-12">
                    <label for="requiredfield" class="back">*</label>
                    <label for="Group Name" >Group Name :</label>
                    <asp:TextBox ID="txtType" runat="server" CssClass="textbox form-control" MaxLength="40" placeholder="Please enter Group Name." Width="100%"></asp:TextBox>
                </div>
                <div class="form-group col-md-6 col-sm-6 col-xs-12">
                    <label for="requiredfield" class="back">*</label>
                    <label for="Mobile No." >Mobile No. :</label>
                    <asp:TextBox ID="txtmobile" runat="server" CssClass="textbox form-control" MaxLength="10" placeholder="Please enter Mobile No." Width="100%"></asp:TextBox>
                           <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtmobile">
                        </ajax:FilteredTextBoxExtender>
                </div>
                <div class="form-group" style="display:none;">
                    <label for="requiredfield" class="back">*</label>
                    <label for="Mobile No." >Marker ToolTip :</label>
                    <asp:RadioButton GroupName="marker" runat="server" Checked="true" ID="rdbAddress" Text="Address" />
                       <asp:RadioButton GroupName="marker" runat="server"  ID="rdbtime" Text="Time" />
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
