<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/FFMS.master" CodeBehind="UserDashBoard.aspx.cs" Inherits="AstralFFMS.UserDashBoard" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link type="text/css" rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
<%--    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="http://cdn.jsdelivr.net/jquery.simpletip/1.3.1/jquery.simpletip-1.3.1.min.js"></script>--%>
    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "error"
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
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
        //$(function () {
        //    $('[id*=GridView1] tr').each(function () {
        //        var toolTip = $(this).attr("title");
        //        $(this).find("td").each(function () {
        //            $(this).simpletip({
        //                content: toolTip
        //            });
        //        });
        //        $(this).removeAttr("title");
        //    });
        //});
    </script>
    <script type="text/javascript">
        function validate() {
            var flag = false;
            var gridView = document.getElementById('<%= GridView1.ClientID %>');

           for (var i = 1; i < gridView.rows.length; i++) {
               var inputs = gridView.rows[i].getElementsByTagName('input');
               //var areas = gridView.rows[i].getElementsByTagName('textarea');
               if (inputs != null && inputs.length > 1 && inputs[0] != null ) {
                   if (inputs[0].type == "checkbox") {
                      
                       if (!inputs[0].checked ) {

                           flag = false;
                           break;
                       }
                       else {
                           flag = true
                       }
                   }
               }
           }
           if (!flag) {
               alert('Please Select Atleast One Receipient');

           }
           return flag;
        }
    </script>
    <style>
        .divScroll {
            overflow: scroll;
            height: 500px;
            width: 100%;
        }
        .textalign{
            text-align:right;
        }
    </style>
  
    <script type="text/javascript">
        function btnSubmitfunc() {
            var YearValues = [];
            $("[id*=chk] input:checked").each(function () {
                YearValues.push($(this).val());
            });
            if (YearValues == "") {
                //errormessage("Please Select Atleast One Receipient");
                return false;
            }
        }
     
        function popupWindow1(Value1) {
          <%--  var myHidden = document.getElementById('<%= hidurl.ClientID %>');
            alert(myHidden);--%>
            window.open(Value1, 'Area', "width=1200,height=700,align: center, scroll: no, status: no, toolbar: no, left=100, top=100");

            
        }

        function SetTarget() {

            document.forms[0].target = "_blank";

        }
    </script>

    <style type="text/css">
           #ctl00_ContentPlaceHolder1_GridView2 {
           width:100%;
          
        }

        .insidebtnmy {
            padding: 0 4px;
        }

        .aVis {
            display: none;
        }

        #ContentPlaceHolder1_gdvprodgrp tbody th {
            background: #367FA9;
            color: white;
        }

        input[type=checkbox] {
            margin: 4px 4px 0 !important;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 1;
            z-index: 10000;
        }

        @media (max-width: 600px) {
            #pnlpopup {
                width: 295px;
            }
        }

        @media (min-width: 600px) {
            #pnlpopup {
                width: 400px;
            }
        }

        @media (max-width: 600px) {
            #ContentPlaceHolder1_pnlpopup {
                width: 100%;
            }
        }

        @media (min-width: 600px) {
            #ContentPlaceHolder1_pnlpopup {
                width: 500px;
            }
        }

        @media (max-width: 600px) {
            #ContentPlaceHolder1_PnlTravel {
                width: 100%;
            }
        }

        @media (min-width: 600px) {
            #ContentPlaceHolder1_PnlTravel {
                width: 500px;
            }
        }

        @media (max-width: 600px) {
            #ContentPlaceHolder1_PnlConv {
                width: 100%;
            }
        }

        @media (min-width: 600px) {
            #ContentPlaceHolder1_PnlConv {
                width: 600px;
            }
        }


        @media (min-width: 400px) and (max-width:600px) {
            .gdvprodgpclass {
                width: 380px;
            }
        }

        @media (min-width: 200px) and (max-width:400px) {
            .gdvprodgpclass {
                width: 240px;
            }
        }

        @media (min-width: 600px) {
            .gdvprodgpclass {
                width: 520px;
            }
        }
    </style>

    <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" ForeColor="white" runat="server"></asp:Label>
            </div>
        </div>

        <div class="box-body" id="rptmain" runat="server">
          <asp:Label runat="server"  ID="lblprogress" Text="Processing...." Font-Bold="true" ForeColor="Green" Font-Size="35px" Visible="false"></asp:Label>
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Dashboard</h3>
                          <asp:Button runat="server" ID="btnrefe" Text="Refresh" OnClick="btnrefe_Click" CssClass="btn fl"   />
                            <asp:Button runat="server" Text="Export" ID="btnexport" OnClick="btnexport_Click" CssClass="btn btn-primary fl" />
                        </div>

                        <div class="box-body">

                           <div class="col-md-12 col-sm-12 col-xs-11">
            <div class="row">
              <div class="form-group col-md-2 col-sm-6 col-xs-8 paddingleft0" >
                    <label for="requiredfield" class="back">*</label>
                    <label for="UserName" >Status :</label>
                   <asp:DropDownList ID="ddlStatus" CssClass="textbox form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                <asp:ListItem Text="Activated" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="DeActivated" Value="1"></asp:ListItem>
                                <asp:ListItem Text="All" Value="2"></asp:ListItem>                                
                            </asp:DropDownList>
                </div>             
			
                </div></div>
                               <asp:HiddenField ID="hidurl" runat="server" />
                            <div id="result" style="background-color: #FFF;">
                                <asp:Image ID="img1" runat="server" Width="14" Height="14" src="img/greentime.png" />
                               <%-- <b>Below 15 Minutes</b>--%>
                                 <b>Below 60 Minutes</b>
                                <asp:Image ID="img2" runat="server" Width="14" Height="14" src="img/orangetime.png" />
                               <%-- <b>Between 16-60 Minutes</b>--%>
                                 <b>Between 61-180 Minutes</b>
                                <asp:Image ID="img3" runat="server" Width="14" Height="14" src="img/bluetime.png" />
                                <b>Above 181 Minutes</b>
                                <asp:Image ID="img4" runat="server" Width="14" Height="14" src="img/redtime.png" />
                                <b>Absent</b>
                            </div>
                            <div class="table-responsive">
                                <asp:GridView ID="GridView1" runat="server" class="table table-bordered gridclass" AutoGenerateColumns="False"
                                    OnPageIndexChanging="GridView1_PageIndexChanging"
                                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" PageSize="15" PagerSettings-Position="Bottom"
                                    PagerStyle-HorizontalAlign="Left"
                                    CellPadding="3" GridLines="Vertical" PagerStyle-BackColor="Aqua" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last"
                                    PagerSettings-PreviousPageText="<<" PagerSettings-NextPageText=">>" AllowPaging="false"
                                    Width="100%" DataKeyNames="DeviceNo,Title" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" 
                                    EmptyDataText="No Records Found!!">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <EmptyDataRowStyle BackColor="#3c8dbc" ForeColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno.">
                                            <ItemTemplate>
                                             <asp:HiddenField ID="hidcommentdata" runat="server" Value='<%# Bind("comments") %>' />
                                                 <asp:HiddenField ID="hidappinstalldate" runat="server" Value='<%# Bind("created_date") %>' />
                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="title" HeaderText="Group" />--%>
                                        <asp:TemplateField HeaderText="Person Name" HeaderStyle-Width="150" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                   <asp:HiddenField runat="server" ID="hiddeviceno" Value='<%# Bind("DeviceNo") %>'/>
                                                <asp:UpdatePanel ID="aa" runat="server">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="PersonID" runat="server" CausesValidation="false" Width="300px"
                                                            CommandName="Select" Text='<%# Bind("title") %>'  OnClientClick = "SetTarget();" ></asp:LinkButton>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="PersonID" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            <asp:BoundField DataField="CurrentDate" HeaderText="Last Message Received" ItemStyle-Width="200px" />  
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:UpdatePanel ID="upremark" runat="server" >
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="imagebtnremark" runat="server" ImageUrl='<%# Bind("Remakicon") %>' OnClick="imagebtnremark_Click" />
                                                       <%-- CommandArgument='<%# Bind("Mobile") %>'--%>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                         <asp:PostBackTrigger ControlID="imagebtnremark" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                     <%--     <asp:ImageField  HeaderText="Heart" DataAlternateTextField="Mobile" DataImageUrlField="~/img/todayRemarkIcon.png"/>--%>
                                        <asp:TemplateField HeaderText="Heart">
                                            <ItemTemplate>
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="imagebtnpush" runat="server" ImageUrl='<%# Bind("heartpath") %>'  />
                                                       <%-- CommandArgument='<%# Bind("Mobile") %>'--%>
                                                    </ContentTemplate>
                                                    
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                        <asp:BoundField DataField="version" HeaderText="Version" />
                                         <asp:BoundField DataField="Phone" HeaderText="Mobile-Modal" />
                                        <%--<asp:BoundField DataField="created_date" HeaderText="Version Date" />  ImageUrl='<%# Bind("Remakicon") %>' DataImageUrlField="HeartPath" --%>
                                        <asp:BoundField DataField="Mobile" HeaderText="Mobile" />                                  
                                        <asp:TemplateField>
                                             <HeaderTemplate>

                                                <asp:CheckBox ID="checkAll" runat="server" onclick = "checkAll(this);" />

                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chk" onclick = "Check_Click(this)" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="BtnsendPush" Text="Restart" BackColor="#3C8DBC" ForeColor="White" ToolTip="Restart Remote App" CommandArgument='<%# Bind("DeviceNo") %>' CausesValidation="false" CommandName="SendPush"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:BoundField DataField="Status" HeaderText="Status" />
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <PagerStyle BackColor="#3c8dbc" ForeColor="White" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#3C8DBC" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="#FFFFFF" />
                                </asp:GridView>
                            </div>
                            <div>
                                <asp:Button ID="btnmszpopup" runat="server" Text="Message" class="btn btn-primary" />
                                <cc1:ModalPopupExtender runat="server" ID="Modalpopupextender1" TargetControlID="btnmszpopup"
                                    PopupControlID="pnlmszpopup" BackgroundCssClass="Background" DropShadow="true" Y="10">
                                </cc1:ModalPopupExtender>
                            </div>
                            <asp:Panel runat="server" ID="pnlmszpopup" Height="300px" Width="130%" Style="display: none;" BackColor="#ffffff" CssClass="modalBackground" ScrollBars="Vertical">
                                <div >


                                                <div id="rptDiv" runat="server">
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="gridmsz" runat="server">
                                        <HeaderTemplate>
                                            <table id="tblmsz" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <%--<th style="text-align:center;width:6%;">SNo.</th>--%>
                                                        <th style="background-color:#3C8DBC;color:white">Enter Message</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                          <tr>                                             
                                                <td>
                                                
                                        <asp:Textbox ID="txtmsz" runat="server" Height="180px" Width="100%"  TextMode="MultiLine" Text='<%# Eval("msz")%>'></asp:Textbox>
                                           </td> </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>     </table>       
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <asp:Button ID="btnsend" runat="server" Text="Send" OnClick="btnsend_Click" Width="37%" class="btn btn-primary" />
                                     <asp:Button ID="ImageButton1" runat="server" Text="Close" style="float:right" Width="37%"  OnClick="ImageButton1_Click1" class="btn btn-primary" />
                                </div>
                            </div>
                                    
                              <%--  <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="false"
                                    AlternateText="Close" ImageAlign="Right"
                                     OnClick="ImageButton1_Click" class="btn btn-primary" />--%>
                                
</div>
                            </asp:Panel>
                            <div>
                                <asp:Button ID="Modalshow" runat="server" Style="display: none;" />
                                <cc1:ModalPopupExtender runat="server" ID="mpePop" TargetControlID="ModalShow"
                                    PopupControlID="pnlItem" BackgroundCssClass="Background" DropShadow="true" Y="10">
                                </cc1:ModalPopupExtender>
                            </div>

                            <asp:Panel ID="pnlItem" runat="server" Style="display: none;" Height="500" CssClass="modalBackground" ScrollBars="Vertical">
                                <div class="popupDiv">
                                    <asp:Label ID="lblPerson" ForeColor="White" Font-Bold="true" runat="server" Style=""></asp:Label>
                                    <asp:ImageButton ID="imgclose" runat="server" CausesValidation="false"
                                        AlternateText="Close" ImageAlign="Right"
                                        ToolTip="close" OnClick="imgclose_Click" />
                                </div>
                                <div class="popupdiv">
                                    <table style="width: 100%;" class="table_responsoive">

                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                                    EmptyDataText="No Records Found!!">
                                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                    <EmptyDataRowStyle BackColor="#DB5F5F" ForeColor="White" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sno.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="CurrDate" HeaderText="Date" ItemStyle-Width="200px" />
                                                        <asp:BoundField DataField="title" HeaderText="Area" ItemStyle-Width="300px" />
                                                          <asp:BoundField DataField="Distance" HeaderText="Covered Distance(Kms.)" ItemStyle-Width="100px" HeaderStyle-Width="100px"  ItemStyle-CssClass="textalign" />
                                                        <%--<asp:BoundField DataField="Distance" HeaderText="Distance(Km)" ItemStyle-Width="100px" />--%>
                                                    </Columns>
                                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                    <PagerStyle BackColor="#3c8dbc" ForeColor="White" />
                                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#3C8DBC" Font-Bold="True" ForeColor="White" />
                                                    <AlternatingRowStyle BackColor="#FFFFFF" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>


                            <%-- Remark Panel --%>

                               <div>
                                <asp:Button ID="btnpopremark" runat="server" Style="display: none;" />
                                <cc1:ModalPopupExtender runat="server" ID="ModalPopupExtender2" TargetControlID="btnpopremark"
                                    PopupControlID="Panelremark" BackgroundCssClass="Background" DropShadow="true" Y="10">
                                </cc1:ModalPopupExtender>
                            </div>

                            <asp:Panel ID="Panelremark" runat="server" Style="display: none;background-color:white" Height="450" Width="550px"  CssClass="modalBackground"  ScrollBars="Vertical">
                                <div class="row popupDiv">
                                    <asp:HiddenField ID="HidGoTorow" runat="server" />
                                    <asp:Label ID="Label1" ForeColor="White" Font-Bold="true" runat="server" Style=""></asp:Label>
                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="false"
                                        AlternateText="Close" ImageAlign="Right"
                                        ToolTip="close" OnClick="ImageButton2_Click" style="padding-right: 16px;"/>
                                </div>
                               
                                <div >
                                    <div class="row">
                                  
                                    <div class="col-md-5">
                                     <asp:Label runat="server" ID="Label2" Text="Select Status :" ForeColor="#000000" ></asp:Label>
                                    </div>
                                 <div class="col-md-7">
                                    <asp:DropDownList runat="server" ID="ddlcallingstatus" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <asp:HiddenField runat="server" ID="hiddendeviceid" />
                                    <div class="col-md-5">
                                     <asp:Label runat="server" ID="lblremark" Text="Remark :" ForeColor="#000000" ></asp:Label>
                                    </div>
                                 <div class="col-md-7">
                                    <asp:TextBox runat="server" TextMode="MultiLine" ID="txtremark" style="width:100%"  placeholder="Enter Remark Here"></asp:TextBox>
                                    </div>
                                </div>
                                    <div class="row">
                                          <div class="col-md-5">
                                         <label for="From" >     Mark Leave :</label>
                                                </div>
                                    </div>
                                   
                                <div class="row">

                                        <div class="form-group col-md-4 col-sm-6 col-xs-12" >
                                            
                                               <label for="requiredfield" class="back">*</label>
                                               <label for="From" >From Date :</label>

                                               <div class="clearfix"></div>

                                                                    <div class="col-md-8 col-sm-8 col-xs-8 no-padding">
                                                                    <asp:TextBox ID="txt_fromdate" BackColor="#e9e9e9"  runat="server" CssClass="textbox form-control"  ></asp:TextBox>
                                                                    </div>
                        
                                                                  <div class="col-md-1 col-xs-2 col-sm-1 no-padding">
                                                                    <a href="javascript:;" id="img43" class="cal-icon"><i class="fa fa-calendar" aria-hidden="true"></i></a>
                                                                     </div>
                                                  <div class="col-md-5 col-sm-3 col-xs-6 paddingright0"></div>
                                                    <cc1:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txt_fromdate" PopupButtonID="img43" Format="dd-MMM-yyyy"> </cc1:CalendarExtender>
                                     </div>
                                     <div class="form-group col-md-4 col-sm-6 col-xs-12" >&nbsp;&nbsp;&nbsp;
                                               <label for="requiredfield" class="back">*</label>
                                               <label for="To" >To Date :</label>
                                            <div class="clearfix"></div>
                                            <div class="col-md-8 col-sm-8 col-xs-8 no-padding">
                                                  <asp:TextBox ID="TextBox5" BackColor="#e9e9e9" runat="server" CssClass="textbox form-control"></asp:TextBox>
                                           </div> 
                                           <div class="col-md-1 col-xs-2 col-sm-1 no-padding">
                                                 <a href="javascript:;" id="ImageButton3" class="cal-icon"><i class="fa fa-calendar" aria-hidden="true"></i></a>
                                           </div>
                                           <div class="col-md-5 col-sm-3 col-xs-6 paddingright0"></div>
                                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="TextBox5"
                                                    PopupButtonID="ImageButton3" Format="dd-MMM-yyyy">
                                                </cc1:CalendarExtender>
                                    </div>
                                     <div class="form-group col-md-4 col-sm-6 col-xs-12" >&nbsp;&nbsp;&nbsp;
                                                <asp:Button runat="server" ID="btnleave" CssClass="btn btn-primary" Text="Mark Leave" OnClick="btnleave_Click"/>
                                    </div>
                                </div>
                                <div >
                                    <asp:Button runat="server" ID="btnsaveremrk" CssClass="btn btn-primary" Text="Save Remark" OnClick="btnsaveremrk_Click"/>
                                </div>
                                <div style="height:20px">
                                </div>
                                    <%--<div class="table_responsive">--%>
                                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                                    EmptyDataText="No Records Found!!"  >
                                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                    <EmptyDataRowStyle BackColor="#DB5F5F" ForeColor="White" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sno.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="CallingRemark" HeaderText="Remark" ItemStyle-Width="200px" ItemStyle-Wrap="true" />
                                                    </Columns>
                                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                    <PagerStyle BackColor="#3c8dbc" ForeColor="White" />
                                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#3C8DBC" Font-Bold="True" ForeColor="White" />
                                                    <AlternatingRowStyle BackColor="#FFFFFF" />
                                            </asp:GridView>
                                  <%--  </div>--%>
                                </div>
                            </asp:Panel>

                            <%-- End remark  --%>
                        </div>

                    </div>
                </div>
            </div>

        </div>




    </section>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ScrollToRow(Id) {
          
        // $("#button").click(function () {
        $('html, body').animate({
            scrollTop: $(Id).offset().top
        }, 2000);
        //  });
        }

    </script>
    <script type = "text/javascript">
    function checkAll(objRef)

{

    var GridView = objRef.parentNode.parentNode.parentNode;

    var inputList = GridView.getElementsByTagName("input");

    for (var i=0;i<inputList.length;i++)

    {

        //Get the Cell To find out ColumnIndex

        var row = inputList[i].parentNode.parentNode;

        if(inputList[i].type == "checkbox"  && objRef != inputList[i])

        {

            if (objRef.checked)

            {

                //If the header checkbox is checked

                //check all checkboxes

                //and highlight all rows

               // row.style.backgroundColor = "aqua";

                inputList[i].checked=true;

            }

            else

            {

                //If the header checkbox is checked

                //uncheck all checkboxes

                //and change rowcolor back to original

                if(row.rowIndex % 2 == 0)

                {

                   //Alternating Row Color

                 //  row.style.backgroundColor = "#C2D69B";

                }

                else

                {

                   //row.style.backgroundColor = "white";

                }

                inputList[i].checked=false;

            }

        }

    }

}
    function Check_Click(objRef) {

        //Get the Row based on checkbox

        var row = objRef.parentNode.parentNode;

        if (objRef.checked) {

            //If checked change color to Aqua

            row.style.backgroundColor = "aqua";

        }

        else {

            //If not checked change back to original color

            if (row.rowIndex % 2 == 0) {

                //Alternating Row Color

                row.style.backgroundColor = "#C2D69B";

            }

            else {

                row.style.backgroundColor = "white";

            }

        }



        //Get the reference of GridView

        var GridView = row.parentNode;



        //Get all input elements in Gridview

        var inputList = GridView.getElementsByTagName("input");



        for (var i = 0; i < inputList.length; i++) {

            //The First element is the Header Checkbox

            var headerCheckBox = inputList[0];



            //Based on all or none checkboxes

            //are checked check/uncheck Header Checkbox

            var checked = true;

            if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {

                if (!inputList[i].checked) {

                    checked = false;

                    break;

                }

            }

        }

        headerCheckBox.checked = checked;



    }



  
</script> 
      <style type="text/css">
    .tooltip
    {
        position: absolute;
        top: 0;
        left: 0;
        z-index: 3;
        display: none;
        background-color: yellowgreen;
        color: White;
        padding: 5px;
        font-size: 10pt;
        font-family: Arial;
    }
    td
    {
        cursor: pointer;
    }

    .fl{
        float:right;
    }
</style>
</asp:Content>
