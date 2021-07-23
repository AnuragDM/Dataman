<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ExpenseSheetSummary.aspx.cs" Inherits="AstralFFMS.ExpenseSheetSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link type="text/css" rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script src="Scripts/modernizr.js"></script>
    <script>      window.jQuery || document.write('<script src="js/libs/jquery-1.7.min.js">\x3C/script>')</script>

    <!-- FlexSlider -->
    <script defer src="Scripts/jquery.flexslider.js"></script>
    <style type="text/css">
        .alignAmt {
            text-align: right !important;
        }

        .imgstyl {
            float: right;
            height: 15px;
        }

        .ShowModal {
            cursor: pointer;
            color: black;
            font-size: 15px;
            font-weight: 700;
            text-decoration: underline;
        }

        .disp {
            display: none;
        }

        .Background {
            opacity: 0.6;
            background-color: #7d7d7d;
        }

         ::marker {
            color: white;
        }

       ol > li {
            direction: rtl;
            float: left;
            margin-left: 5px;
        }

        .flex-direction-nav > li {
            list-style: none;
        }

        .flex-prev {
            float: left;
            display: none;
        }
        /*ul{
            display: none;
        }*/
        ul > li > a {
            font-weight: 700;
            color: black;
        }

        ol > li > a {
            font-weight: 700;
            color: black;
            cursor: pointer;
        }

        .flex-next {
            float: right;
            margin-right: 38px;
            display: none;
        }
        /*.wraptext { word-wrap: break-word;}*/
    </style>
    <style type="text/css">
        .WrapText {
            width: 100%;
            word-break: break-all;
        }

        #ContentPlaceHolder1_mpePop_foregroundElement {
            width: 600px;
            left: 29% !important;
        }

        .colPad {
            padding-left: 10px !important;
            text-align: center !important;
            padding-right: 10px !important;
        }

        .table-bordered > tbody > tr > td {
            height: 35px;
        }

        .hght {
            margin: 5px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#dateTimeInput").jqxDateTimeInput({ width: '200px', height: '25px', theme: 'arctic', formatString: 'dd-MMM-yyyy' });
            var date = new Date();
            date.setDate(date.getDate())
            $("#dateTimeInput").jqxDateTimeInput('setMaxDate', date);
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
     <script type="text/javascript">
        $(function () {
            SyntaxHighlighter.all();
        });
        $(window).load(function () {
            $('.flexslider').flexslider({
                animation: "slide",
                start: function (slider) {
                    //$('body').removeClass('loading');
                }
            });
        });
    </script>
    <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <!-- left column -->

        <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- general form elements -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">Expense Sheet Summary</h3>
                        <div style="float: right">
                            <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" Visible="false" />
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <div class="box-body">
                        <div class="row hght">
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <b>Sales Person:</b>
                                    <asp:Label ID="lblSalesPersonName" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <b>Expense Group Name:</b>
                                    <asp:Label ID="lblExpenseGroupName" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <b>Voucher Number:</b><asp:Label ID="lblVoucherNo" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="row hght" style="display: none">
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <b>Verified By:</b>
                                    <asp:Label ID="lblVerifiedBy" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <b>Verified Date Time:</b>
                                    <asp:Label ID="lblVDT" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <b>Approved By:</b>
                                    <asp:Label ID="lblState" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="row hght">
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <b>Designation:</b>:<asp:Label ID="lblDesignation" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <b>Created:</b>
                                    <asp:Label ID="lblCreated" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <b>Created By:</b>
                                    <asp:Label ID="lblCreatedBy" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="row hght">
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <b>Submitted:</b>
                                <asp:Label ID="lblSubmitted" runat="server" Text=""></asp:Label>
                            </div>
                            <div id="ttl" runat="server" class="col-md-4 col-sm-4 col-xs-12">
                                <b>Total:</b><asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <b>Total Verified:</b><asp:Label ID="lblTotalVerified" runat="server" Text=""></asp:Label>
                            </div>
                            <div id="ttlapp" runat="server" class="col-md-3 col-sm-3 col-xs-12" >
                                <b>Total Approved:</b><asp:Label ID="lblTotalApproved" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <br />
                        <div class="clearfix"></div>
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvDetails" runat="server" class="table-bordered table-striped" AutoGenerateColumns="False" ShowFooter="true" OnRowDataBound="gvDetails_RowDataBound" OnRowCommand="gvDetails_RowCommand">
                                        <Columns>
                                            <asp:BoundField HeaderText="Expense Type Name" DataField="ExpenseTypeName" ItemStyle-CssClass="colPad" HeaderStyle-CssClass="colPad" />

                                            <asp:TemplateField HeaderText="Expense Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="billDateLbl" runat="server"
                                                        Text='<%#Eval("BillDate").ToString()!="" ? Convert.ToDateTime(Eval("BillDate")).ToString("dd/MMM/yyyy") : string.Empty %>'>                                                </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField HeaderText="From City" DataField="farea" ItemStyle-CssClass="colPad" HeaderStyle-CssClass="colPad" />
                                            <asp:TemplateField HeaderText="To City">
                                                <ItemTemplate>

                                                    <asp:Label ID="ibltarea" runat="server"
                                                        Text='<%#Eval("tarea")%>'></asp:Label>

                                                </ItemTemplate>
                                                <ItemStyle Width="100" CssClass="colPad" />
                                                <HeaderStyle Width="300px" CssClass="colPad" />

                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Distance Travelled (In Kms)" DataField="kms" ItemStyle-CssClass="colPad" HeaderStyle-CssClass="colPad">
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" CssClass="colPad" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Extra Travelled (In Kms)" DataField="exkms" ItemStyle-CssClass="colPad" HeaderStyle-CssClass="colPad">
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" CssClass="colPad" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Mode">
                                                <ItemTemplate>

                                                    <asp:Label ID="iblnme" runat="server"
                                                        Text='<%#Eval("Name")%>'></asp:Label>

                                                </ItemTemplate>
                                                <ItemStyle Width="100" CssClass="colPad" />
                                                <HeaderStyle Width="300px" CssClass="colPad" />
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Night Halt Amount" DataField="NighthaltAmt" ItemStyle-CssClass="alignAmt colPad" FooterStyle-CssClass="alignAmt" HeaderStyle-CssClass="colPad">
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" CssClass="colPad" />
                                            </asp:BoundField>

                                            <asp:BoundField HeaderText="DA" DataField="Da" ItemStyle-CssClass="alignAmt colPad" FooterStyle-CssClass="alignAmt" HeaderStyle-CssClass="colPad">
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" CssClass="colPad" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Fare/Exp. Amt." DataField="BillAmount" ItemStyle-CssClass="alignAmt colPad" FooterStyle-CssClass="alignAmt" HeaderStyle-CssClass="colPad">
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" CssClass="colPad" />
                                            </asp:BoundField>

                                            <asp:BoundField HeaderText="Return Journey" DataField="RetJnr" ItemStyle-CssClass="colPad" HeaderStyle-CssClass="colPad" />

                                            <asp:BoundField HeaderText="Claim Amount" DataField="ClaimAmount" ItemStyle-CssClass="alignAmt colPad" FooterStyle-CssClass="alignAmt" HeaderStyle-CssClass="colPad">
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" CssClass="colPad" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Verified Amount" DataField="VerifiedAmt" ItemStyle-CssClass="alignAmt colPad" HeaderStyle-CssClass="colPad">
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" CssClass="colPad" />
                                            </asp:BoundField>

                                            <asp:BoundField HeaderText="Approved Amount" DataField="ApprovedAmount" ItemStyle-CssClass="alignAmt colPad" HeaderStyle-CssClass="colPad">
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" CssClass="colPad" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>

                                                    <asp:Label ID="lblMainRemarks" runat="server"
                                                        Text='<%#Eval("MainRemarks")%>'></asp:Label>

                                                </ItemTemplate>
                                                <ItemStyle Width="100" CssClass="WrapText colPad" />
                                                <HeaderStyle Width="300px" CssClass="colPad" />
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Supporting" DataField="Enclosed" ItemStyle-CssClass="colPad disp" HeaderStyle-CssClass="colPad disp" />
                                            <asp:TemplateField HeaderText="View Attachment">
                                                <ItemTemplate>

                                                    <asp:LinkButton ID="LinkButton1" runat="server" class="btn btn-primary" CommandArgument='<%# Eval("ExpenseGroupId")+","+ Eval("ExpenseDetailID") %>'
                                                        CommandName="viewimg">View Attachment</asp:LinkButton>

                                                </ItemTemplate>
                                                <ItemStyle Width="100" CssClass="colPad" />
                                                <HeaderStyle Width="300px" CssClass="colPad" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                        <div class="row">
                            <div class="col-md-6 col-sm-6 col-xs-12">

                                <div class="float:left;">
                                    <asp:Button ID="btnPrint" runat="server" Text="Print" class="btn btn-primary" OnClick="btnPrint_Click" OnClientClick="target ='_blank';" Visible="false" />
                                    <asp:Button ID="btnExcelExport" runat="server" Text="Export to Excel" class="btn btn-primary" OnClick="btnExcelExport_Click" />
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="col-md-6 col-sm-6 col-xs-12 float:left;">
                                    <b>Total Adv. Amount:</b><asp:Label ID="lbladvamo" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12 float:right;">
                                    <b>Total Final Amount:</b><asp:Label ID="lblfnlamo" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:Button ID="Modalshow" runat="server" Style="display: none;" />
        <ajaxToolkit:ModalPopupExtender runat="server" ID="mpePop" TargetControlID="ModalShow"
            PopupControlID="pnlItem" BackgroundCssClass="Background" DropShadow="true" X="40" Y="30">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="pnlItem" runat="server" Style="display: none; background-color: White; padding: 1%; box-shadow: rgb(0 0 0) 5px 5px 5px; border: 1px solid rgb(185, 183, 183); overflow: hidden;" Height="600px">
            <div class="popupDiv row">
                <div class="box-header with-border">
                    <div class="col-md-6 col-sm-6 col-xs-12 headdiv">
                        <asp:Label ID="lblPerson" Font-Bold="true" runat="server" Text="Supporting Attachments"></asp:Label>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-6 headdiv">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/cross.jpg" CssClass="imgstyl" OnClick="ImageButton1_Click" />
                    </div>
                </div>

                <div class="modal-body">
                    <div id="container" class="cf">
                        <div id="main" role="main">
                            <div class="slider">
                                <div class="flexslider">
                                    <ul class="slides">
                                        <asp:Repeater runat="server" ID="Mylist">
                                            <ItemTemplate>
                                                <li>
                                                    <asp:Image ID="img" runat="server" ImageUrl='<%#Eval("imagename")%>' Height="500px"></asp:Image>
                                                </li>
                                            </ItemTemplate>

                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </section>
</asp:Content>
