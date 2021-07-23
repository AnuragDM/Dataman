<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/FFMS.Master" CodeBehind="EnviroTemplate.aspx.cs" Inherits="AstralFFMS.EnviroTemplate" %>

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
             debugger;
             alert("123");
             if ($('#<%=ddlDistSearch.ClientID%>').val() == '0') {
                errormessage("Please Select the Distributor Search By Name");
                return false;
            }
            if ($('#<%=ddlItemSearch.ClientID%>').val() == '') {
                errormessage("Please Select the Item Search By Name");
                return false;
            }

            if ($('#<%=ddlItemwisesale.ClientID%>').val() == '') {
                errormessage("Please Select the Item Wise Secondary Sales");
                return false;
            }
             if ($('#<%=ddlAreawiseDistributor.ClientID%>').val() == '') {
                 errormessage("Please Select the Area Wise Distributor");
                 return false;
             }
             if (($('#<%=ddlattbyorder.ClientID%>').val() == 'N') &&($('#<%=ddlattbyphoto.ClientID%>').val() == 'N')&&&($('#<%=ddlattmanual.ClientID%>').val() == 'N'))

             { alert("123");
                 errormessage("Please Select Any One Attendance Parameter");
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
          <div class="box-body" id="buttonDiv" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-body">
                            <div class="col-lg-6 col-md-6 col-sm-8 col-xs-12">
                                <div class="col-lg-6 col-md-6 col-sm-12  col-xs-12">
                                     <asp:Button Style="margin-right: 5px;" type="button" ID="btngeneral" runat="server" Width="100%" Text="General" OnClick="btnGeneral_Click" class="btn btn-primary"/>
                                </div>
                                <div class="clearfix"></div>
                                 <div class="col-lg-6 col-md-6 col-sm-12  col-xs-12" style="margin-top:4px;">
                                     <asp:Button Style="margin-right: 5px;" type="button" ID="btnDSR" runat="server" Width="100%" Text="DSR Entry" OnClick="btnDSREntry_Click" class="btn btn-primary"/>
                                </div>
                                 <div class="clearfix"></div>
                                 <div class="col-lg-6 col-md-6 col-sm-12  col-xs-12" style="margin-top:4px;">
                                     <asp:Button Style="margin-right: 5px;" type="button" ID="btnPrimary" runat="server" Width="100%" Text="Primary" OnClick="btnPrimary_Click" class="btn btn-primary"/>
                                </div>
                                 <div class="clearfix"></div>
                                 <div class="col-lg-6 col-md-6 col-sm-12  col-xs-12" style="margin-top:4px;">
                                     <asp:Button Style="margin-right: 5px;" type="button" ID="btnSecondary" runat="server" Width="100%" Text="Secondary"  class="btn btn-primary" OnClick="btnSecondary_Click"/>
                                </div>
                            </div>
                        </div>
                        </div>
                    </div>
                </div>
              </div>
        <div class="box-body" id="mainDiv" style="display:none;" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <%--<h3 class="box-title">Enviro Settings</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            <div style="float: right">    
                                 <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />                           
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                                <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                    <div class="form-group">
                                        <input id="Id" hidden="hidden" />
                                        <label for="exampleInputEmail1">Distributor Search By Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlDistSearch" CssClass="form-control" runat="server">
                                          <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>   
                                          <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                        
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Item Search By Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlItemSearch" CssClass="form-control" runat="server">
                                             <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>   
                                          <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Item Wise Secondary sales:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlItemwisesale" CssClass="form-control" runat="server">
                                             <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>   
                                          <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Area Wise Distributor:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlAreawiseDistributor" CssClass="form-control" runat="server">
                                             <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>   
                                          <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>                                    
                                </div>
                            </div>
                        </div>
                        <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />

                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                    </div>
                </div>
            </div>
        </div>   
          <div class="box-body" id="DSREntryDiv" style="display:none;" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                            <h3 class="box-title">DSR Entry</h3>
                            <div style="float: right">    
                                 <asp:Button Style="margin-right: 5px;" type="button" ID="Button1" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBackDSR_Click" />                           
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-lg-6 col-md-6 col-sm-8 col-xs-12">
                                <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <label for="exampleInputEmail1">With Whom:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlWithWhom" CssClass="form-control" runat="server">
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                    <input type="checkbox" id="cbWithWhom" runat="server"/>&nbsp;&nbsp; <label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <label for="exampleInputEmail1">Next Visit With Whom:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlNextVisitWithWhom" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                        
                                </div>
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                   <input type="checkbox" id="cbNextVisitWithWhom" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <label for="exampleInputEmail1">Next Visit Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlNextVisitDate" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList> 
                                </div>
                               <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                    <input type="checkbox" id="cbNextVisitDate" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <label for="exampleInputEmail1">Retailer Order By Email:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlRetailerOrderByEmail" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList> 
                                </div>
                              <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                <input type="checkbox" id="cbRetailerOrderByEmail" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <label for="exampleInputEmail1">Retailer's Order By Phone:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlRetailerOrderByPhone" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                </div>

                               <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                   <input type="checkbox" id="cbRetailerOrderByPhone" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                   <label for="exampleInputEmail1">Remarks:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlRemarks" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                </div>
                               <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                   <input type="checkbox" id="cbRemarks" runat="server" />&nbsp;&nbsp; <label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                   <label for="exampleInputEmail1">Expenses From Area:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlExpensesFromArea" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                </div>
                              
                              <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                    <input type="checkbox" id="cbExpensesFromArea" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                  <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                   <label for="exampleInputEmail1">Expenses To Area:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlExpensesToArea" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                </div>
                                  
                              <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                    <input type="checkbox" id="cbExpensestoArea" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>  <div class="clearfix"></div>
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                     <label for="exampleInputEmail1">Visit Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlVisitType" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                </div>
                              <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                    <input type="checkbox" id="cbVisitType" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12" style="display:none">
                                    <label for="exampleInputEmail1">Attendance:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlAttendance" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                               </div>
                              <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;display:none">
                                  <input type="checkbox" id="cbAttendance" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>
                                <%-- aaaaaaaaaaaaaaaaaa --%>
                                  <div class="clearfix"></div>
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <label for="exampleInputEmail1">Attendance By Mannual Entry:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlattmanual" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlattmanual_SelectedIndexChanged">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                               </div>
                              <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                  <input type="checkbox" id="cbattmanual" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <label for="exampleInputEmail1">Attendance By First And last Order:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlattbyorder" CssClass="form-control" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="ddlattbyorder_SelectedIndexChanged">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                               </div>
                              <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                  <input type="checkbox" id="cbattbyorder" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <label for="exampleInputEmail1">Attendance By Photo:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlattbyphoto" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlattbyphoto_SelectedIndexChanged">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                               </div>
                              <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                  <input type="checkbox" id="cbattbyphoto" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <label for="exampleInputEmail1">Beat Plan Mandatory:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlbeatplanman" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                               </div>
                              <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                  <input type="checkbox" id="cbbeatplanman" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <label for="exampleInputEmail1">Use Camera:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlCameragallery" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                               </div>
                              <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                  <input type="checkbox" id="cbCameragallery" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>
                                <%-- 222222222222 --%>
                                  <div class="clearfix"></div>
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                   <label for="exampleInputEmail1">Other Expenses:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlOtherExpenses" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
          
                                </div>
                                <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                <input type="checkbox" id="cbOtherExpenses" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                   <label for="exampleInputEmail1">Other Expenses Remarks:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlOtherExpensesRemarks" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>

                                </div>
                             <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                <input type="checkbox" id="cbOtherExpensesRemarks" runat="server" />&nbsp;&nbsp; <label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                   <label for="exampleInputEmail1">Chargeable:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlchargeable" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>

                                </div>
                             <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                <input type="checkbox" id="cbChargeable" runat="server" />&nbsp;&nbsp; <label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                            </div>
                        </div>
                         <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDSRSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnDSRCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                       </div>
                        </div>
                    </div>
                </div>
              </div>
          
           <%-- Primary--%>
          <div class="box-body" id="DivPrimary" style="display:none;" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                           <%-- <h3 class="box-title">Primary</h3>--%>
                            <div style="float: right">    
                                 <asp:Button Style="margin-right: 5px;" type="button" ID="btnbackprimary" runat="server" Text="Back" class="btn btn-primary" OnClick="btnbackprimary_Click" />                           
                            </div>
                        </div>
                     
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                               <div class="col-md-8 col-sm-8 col-xs-12 ">
                        
                        <h2>Distributor Discussion</h2>

                    </div>
                          <div class="clearfix"></div>
                            
                            <div class="col-lg-6 col-md-6 col-sm-8 col-xs-12">
                               
                                
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <label for="exampleInputEmail1">Next Visit Date/Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlDistributorDisNextVisit" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList> 
                                </div>
                               <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                    <input type="checkbox" id="ChkDistributorDisNextVisit" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                               
                                
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                   <label for="exampleInputEmail1">Remarks:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlDistributorDisRemarks" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                </div>
                               <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                   <input type="checkbox" id="ChkDistributorDisRemarks" runat="server" />&nbsp;&nbsp; <label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                
                               
                              
                            </div>

                            <div class="clearfix"></div>

                                   <div class="col-md-8 col-sm-8 col-xs-12 ">
                        
                        <h2>Failed Visit</h2>

                    </div>
                          <div class="clearfix"></div>

                            <div class="col-lg-6 col-md-6 col-sm-8 col-xs-12">
                               
                                
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <label for="exampleInputEmail1">Next Visit Date/Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlFailedVisitNextVis" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList> 
                                </div>
                                <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                    <input type="checkbox" id="ChkFailedVisitNextVis" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                               
                                
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                   <label for="exampleInputEmail1">Remarks:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlFailedVisitRemark" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                </div>
                               <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                   <input type="checkbox" id="ChkFailedVisitRemark" runat="server" />&nbsp;&nbsp; <label for="exampleInputEmail1">Required</label>
                               </div>
                                  <div class="clearfix"></div>
                                
                               </div>
                              <div class="clearfix"></div>

                                   <div class="col-md-8 col-sm-8 col-xs-12 ">
                        
                        <h2>Distributor Collection</h2>

                    </div>
                          <div class="clearfix"></div>

                            <div class="col-lg-6 col-md-6 col-sm-8 col-xs-12">
                               
                                
                            
                                
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                   <label for="exampleInputEmail1">Remarks:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlCollectRemarks" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                </div>
                               <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                   <input type="checkbox" id="ChkCollectRemarks" runat="server" />&nbsp;&nbsp; <label for="exampleInputEmail1">Required</label>
                               </div>
                                  <div class="clearfix"></div>
                                
                               </div>
                              
                            </div>


                       
                         <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="Button3" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="Button4" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                       </div>
                        </div>
                    </div>
                </div>
              </div>

            <%-- Secondary--%>
        <div class="box-body" id="divSecondary" style="display:none;" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default">
                        <div class="box-header">
                           <%-- <h3 class="box-title">Primary</h3>--%>
                            <div style="float: right">    
                                 <asp:Button Style="margin-right: 5px;" type="button" ID="btnbackSecondary" runat="server" Text="Back" class="btn btn-primary" OnClick="btnbackSecondary_Click" />                           
                            </div>
                        </div>
                     
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                               <div class="col-md-8 col-sm-8 col-xs-12 ">
                        
                        <h3>Book Order</h3>

                    </div>
                          <div class="clearfix"></div>
                            
                            <div class="col-lg-6 col-md-6 col-sm-8 col-xs-12">
                               
                                
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <label for="exampleInputEmail1">Remark(Item Wise):</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlbookodrRemarkItem" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList> 
                                </div>
                               <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                    <input type="checkbox" id="ChkOrderRemarkItemWise" runat="server" />&nbsp;&nbsp;<label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                               
                                
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                   <label for="exampleInputEmail1">Remarks:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlbookodrRemark" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                </div>
                               <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                   <input type="checkbox" id="ChkOrderRemark" runat="server" />&nbsp;&nbsp; <label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                
                               
                              
                            </div>

                            <div class="clearfix"></div>

                                   <div class="col-md-8 col-sm-8 col-xs-12 ">
                        
                        <h3>Demo Entry</h3>

                    </div>
                          <div class="clearfix"></div>

                            <div class="col-lg-6 col-md-6 col-sm-8 col-xs-12">                              
                                                            
                                
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                   <label for="exampleInputEmail1">Remarks:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlDemoEntryRemark" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                </div>
                               <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                   <input type="checkbox" id="ChkDemoEntryRemark" runat="server" />&nbsp;&nbsp; <label for="exampleInputEmail1">Required</label>
                               </div>
                                  <div class="clearfix"></div>
                                
                               </div>
                              <div class="clearfix"></div>

                                   <div class="col-md-8 col-sm-8 col-xs-12 ">
                        
                        <h3>Failed Visit</h3>

                    </div>
                          <div class="clearfix"></div>

                            <div class="col-lg-6 col-md-6 col-sm-8 col-xs-12">
                               
                                
                              <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                   <label for="exampleInputEmail1">Next Visit Date/Time:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlSecFailedVisit_NextVisit" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                </div>
                               <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                   <input type="checkbox" id="ChkSecFailedVuisit_NextVisit" runat="server" />&nbsp;&nbsp; <label for="exampleInputEmail1">Required</label>
                               </div>
                                  <div class="clearfix"></div>
                                
                              
                                
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                   <label for="exampleInputEmail1">Remarks:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlSecFailedVisitRemark" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                </div>
                               <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                   <input type="checkbox" id="chkSecFailedVisitRemark" runat="server" />&nbsp;&nbsp; <label for="exampleInputEmail1">Required</label>
                               </div>
                                  <div class="clearfix"></div>
                                 </div>
                                   <div class="col-md-8 col-sm-8 col-xs-12 ">
                        
                        <h3>Competitor's Activity</h3>

                    </div>
                          <div class="clearfix"></div>
                            
                            <div class="col-lg-6 col-md-6 col-sm-8 col-xs-12">
                               
                                
                                
                                 <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                   <label for="exampleInputEmail1">Remarks:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlCompetitorActivityRemark" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                </div>
                               <div class="form-group col-lg-6 col-md-6 col-sm-6 col-xs-12 setmargindesk" style="Margin-top:22px;">
                                   <input type="checkbox" id="chkCompetitorActivityRemark" runat="server" />&nbsp;&nbsp; <label for="exampleInputEmail1">Required</label>
                                </div>
                                  <div class="clearfix"></div>
                                
                               
                              
                            </div>

                            <div class="clearfix"></div>
                               </div>
                              
                            </div>


                       
                         <div class="box-footer">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="Button6" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" OnClientClick="javascript:return validate();" />
                            <asp:Button Style="margin-right: 5px;" type="button" ID="Button7" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                       </div>
                        </div>
                    </div>
                </div>
              <%--</div>--%>

        <div class="container">
            <div class="row">
                <div class="row">
                    <h3>Skins:</h3>
                    <div class="col-lg-12" style="background:#000; text-align:center;">
                        <div class="col-md-12">
                            <asp:Button ID="skin1" runat="server" CssClass="mycls1 marg" Text="1" OnClick="changeCss1" />
                            <asp:Button ID="skin2" runat="server" CssClass="mycls2 marg" Text="2" OnClick="changeCss2" />
                            <asp:Button ID="skin3" runat="server" CssClass="mycls3 marg" Text="3" OnClick="changeCss3" />
                            <asp:Button ID="skin4" runat="server" CssClass="mycls4 marg" Text="4" OnClick="changeCss4" />
                            <asp:Button ID="skin5" runat="server" CssClass="mycls5 marg" Text="5" OnClick="changeCss5" />
                            <asp:Button ID="skin6" runat="server" CssClass="mycls6 marg" Text="6" OnClick="changeCss6" />
                            <asp:Button ID="skin7" runat="server" CssClass="mycls7 marg" Text="7" OnClick="changeCss7" />
                            <asp:Button ID="skin8" runat="server" CssClass="mycls8 marg" Text="8" OnClick="changeCss8" />
                            <asp:Button ID="skin9" runat="server" CssClass="mycls9 marg" Text="9" OnClick="changeCss9" />
                            <asp:Button ID="skin10" runat="server" CssClass="mycls10 marg" Text="10" OnClick="changeCss10" />
                            <asp:Button ID="skin11" runat="server" CssClass="mycls11 marg" Text="11" OnClick="changeCss11" />
                            <asp:Button ID="skin12" runat="server" CssClass="mycls12 marg" Text="12" OnClick="changeCss12" />
                        </div>
                        <style>
                            .marg{
                                margin: 10px 15px 10px 0px;
                            }
                            .mycls1{
                                background: url(img/blue_skin.png);
                                height: 47px;
                                width: 70px;
                                background-position: center;
                                text-indent: -9990px;
                            }
                            .mycls2{
                                background: url(img/lt_blue_skin.png);
                                height: 47px;
                                width: 70px;
                                background-position: center;
                                text-indent: -9990px;
                            }
                            .mycls3 {
                                background: url(img/red_skin.png);
                                height: 47px;
                                width: 70px;
                                background-position: center;
                                text-indent: -9990px;
                            }
                            .mycls4
                            {
                                background: url(img/lt_red_skin.png);
                                height: 47px;
                                width: 70px;
                                background-position: center;
                                text-indent: -9990px;
                            }
                            .mycls5
                            {
                                background: url(img/yellow_skin.png);
                                height: 47px;
                                width: 70px;
                                background-position: center;
                                text-indent: -9990px;
                            }
                            .mycls6
                            {
                                background: url(img/lt_yellow_skin.png);
                                height: 47px;
                                width: 70px;
                                background-position: center;
                                text-indent: -9990px;
                            }
                            .mycls7
                            {
                                background: url(img/green_skin.png);
                                height: 47px;
                                width: 70px;
                                background-position: center;
                                text-indent: -9990px;
                            }
                            .mycls8
                            {
                                background: url(img/lt_green_skin.png);
                                height: 47px;
                                width: 70px;
                                background-position: center;
                                text-indent: -9990px;
                            }
                            .mycls9
                            {
                                background: url(img/prpl_skin.png);
                                height: 47px;
                                width: 70px;
                                background-position: center;
                                text-indent: -9990px;
                            }
                            .mycls10
                            {
                                background: url(img/lt_prpl_skin.png);
                                height: 47px;
                                width: 70px;
                                background-position: center;
                                text-indent: -9990px;
                            }
                            .mycls11
                            {
                                background: url(img/blk_skin.png);
                                height: 47px;
                                width: 70px;
                                background-position: center;
                                text-indent: -9990px;
                            }
                                .mycls12{
                                background: url(img/lt_blk_skin.png);
                                height: 47px;
                                width: 70px;
                                background-position: center;
                                text-indent: -9990px;
                            }
                        </style>
                    </div> 
                </div>
            </div>
        </div>
        
        
    </section>

    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>   
</asp:Content>