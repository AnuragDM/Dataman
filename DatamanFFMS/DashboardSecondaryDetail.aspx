<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DashboardSecondaryDetail.aspx.cs" Inherits="AstralFFMS.DashboardSecondaryDetail" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
     <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
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
        #select2-ContentPlaceHolder1_ddlParentLoc-container{
        margin-top:-8px !important;
        }
            .container {
        width: 100%;
        height: 200px;
        background: aqua;
        margin: auto;
        padding: 10px;
        }
        .one {
        width: 50%;
        height: 200px;
        background: red;
        float: left;
        }
        .two {
        width: 49%;
        margin-left: 51%;
        height: 200px;
        background: black;
        }
          .totalfont {
            /*font-size:21px;*/
        }

         .colornote li {
            list-style: none;
        }

        .colornote {
            padding: 0;
        }

            .colornote li {
                list-style: none;
                padding: 12px 0;
                border-bottom: 1px solid #c1c1c1;
                font-weight: normal;
            }

                .colornote li:last-child {
                    border: none;
                }

                .colornote li span {
                    font-weight: normal;
                    margin-left: 5px;
                }


                   


h4.great {
	background:rgba(66, 113, 244,0.6);
	margin: 0 0 0px 275px;
	padding: 7px 15px;
	color: #ffffff;
	font-size: 18px;
	font-weight: 600;
	border-radius: 11px;
	display: inline-block;
	-moz-box-shadow:    2px 4px 5px 0 #ccc;
  	-webkit-box-shadow: 2px 4px 5px 0 #ccc;
  	box-shadow:         2px 4px 5px 0 #ccc;
}



.price-slider {
	margin-bottom: 70px;
}

.price-slider span {
	font-weight: 200;
	display: inline-block;
	color:white;
	font-size: 17px;
}
    </style>
    
     
      <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
          .multiselect-container.dropdown-menu {
        width: 100% !important;
        }


        .select2-container {
            /*display: table;*/
        }
         .input-group .form-control {
            height: 34px;
        }

        .multiselect-container > li > a {
            white-space: normal;
        }
    </style>
    

    <%--<script type="text/javascript">
        function pageLoad() {
           
        };
    </script>--%>
   <%-- <script type="text/javascript">
         $(function () {
             //$("#example").DataTable({
             //    "order": [[0, "desc"]]
            // });   
         });
    </script>--%>
    <%-- <script type="text/javascript">
         function pageLoad() {
             $("#example1").DataTable({
                 "order": [[0, "desc"]]
             });
         };
    </script>--%>
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
    
    <script src="dist/js/demo.js" type="text/javascript"></script>
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
         <div class="content">
        <asp:UpdatePanel ID="mainUp" runat="server">
            <ContentTemplate>
                <div class="box-body" id="rptmain" runat="server">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title" runat="server" id="lblHeading"></h3>
                            <div style="float: right">
                               
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <div class="col-lg-9 col-md-9 col-sm-7 col-xs-9">
                                <div class="col-md-12 paddingleft0">
                                    <div id="DIV1" class="form-group col-md-3 paddingleft0">
                                        <label for="exampleInputEmail1">From Date</label>
                                       <asp:TextBox ID="FromDate" runat="server" CssClass="form-control" onChange="showspinner();" AutoPostBack="true" OnTextChanged="FromDate_TextChanged" Style="background-color: white;"></asp:TextBox>
                                      <ajaxToolkit:CalendarExtender ID="CalendarExtender5" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender12" TargetControlID="FromDate"></ajaxToolkit:CalendarExtender>
                                    </div>
                                     <div id="DIVTo" class="form-group col-md-3 paddingleft0">
                                        <label for="exampleInputEmail2">To Date:</label>
                                       <asp:TextBox ID="ToDate" runat="server" CssClass="form-control" onChange="showspinner();" AutoPostBack="true" OnTextChanged="ToDate_TextChanged" Style="background-color: white;"></asp:TextBox>
                                      <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="calendarTextBox_CalendarExtender13" TargetControlID="ToDate"></ajaxToolkit:CalendarExtender>
                                    </div>
                                    <div id="DIVMode" runat="server" class="form-group col-md-3">
                                            <label for="exampleInputEmail1">Mode:</label>
                                             <asp:DropDownList ID="ddlmode" Width="100%" runat="server" onChange="showspinner();" AutoPostBack="true" OnSelectedIndexChanged="ddlMode_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                <div id="DIVFailedVisit" runat="server" class="form-group col-md-3">
                                            <label for="exampleInputEmail1">Non-Productive:</label>
                                     <%--  <asp:ListBox ID="lstUndeUser" CssClass="form-control" Width="100%" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="lstUndeUser_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>--%>
                                            <asp:DropDownList ID="ddlFailedVisit" Width="100%" runat="server" onChange="showspinner();" AutoPostBack="true" OnSelectedIndexChanged="FailedVisit_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                        </div> 
                                     </div>
                                
                                 <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="btnExport_Click"/>
                                
                                 <asp:Button Style="margin-right: 5px;" type="button" ID="btnexportvisit" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="btnexportvisit_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btncollection" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="btncollection_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btndemo" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="btndemo_Click" />
                                <asp:Button Style="margin-right: 5px; width:70px;" type="button" ID="btnCompetitor" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="btnCompetitor_Click" />
                           </div>
                        </div>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rptTotalOrder" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Date</th>
                                                <th>Sale Person</th>
                                                <th>Party</th>
                                                <th>Item</th>
                                                <th style="text-align: right">Qty</th>
                                                <th style="text-align: right">Amount</th>
                                                
                                            </tr>
                                        </thead>
                                         <tfoot>
                                                <tr>
                                                    <th colspan="4" style="text-align: right">Grand Total:</th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>                                                    
                                                </tr>
                                            </tfoot>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("SMId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("SMId") %>' />
                                         <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("created_date"))%>
                                        <asp:Label ID="lbldate" runat="server" Visible="false" Text='<%#String.Format("{0:dd/MMM/yyyy}", Eval("created_date"))%>'></asp:Label></td>
                                        <td><%#Eval("smname") %>
                                        <asp:Label ID="lblsmname" runat="server" Visible="false" Text='<%# Eval("smname")%>'></asp:Label></td>
                                        <td><%#Eval("partyname") %>
                                         <asp:Label ID="lblpartyname" runat="server" Visible="false" Text='<%# Eval("partyname")%>'></asp:Label></td>
                                        <td><%#Eval("itemname") %>
                                         <asp:Label ID="lblitem" runat="server" Visible="false" Text='<%# Eval("itemname")%>'></asp:Label></td>
                                        <td style="text-align: right"><%#Eval("qty") %>
                                         <asp:Label ID="lblqty" runat="server" Visible="false" Text='<%# Eval("qty")%>'></asp:Label></td>
                                         <td style="text-align: right"><%#Eval("amount") %>
                                         <asp:Label ID="lblamount" runat="server" Visible="false" Text='<%# Eval("amount")%>'></asp:Label></td>
                                        
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rptCollection" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                 <th>Date</th>
                                                 <th>Sale Person</th>
                                                 <th>Party</th>
                                                 <th>Mode</th>
                                                 <th>Cheque No.</th>
                                                 <th>Cheque Date</th>
                                                 <th>Bank</th>
                                                 <th>Branch</th>
                                                 <th>Amount</th>
                                                 <th>Payment Date</th>
                                                 <th>Remarks</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("SMId") %>');">
                                       
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("SMId") %>' />
                                         <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("created_date"))%>
                                         <asp:Label ID="lbldate" runat="server" Visible="false" Text='<%#String.Format("{0:dd/MMM/yyyy}", Eval("created_date"))%>'></asp:Label></td>
                                        <td><%#Eval("smname") %>
                                        <asp:Label ID="lblsmname" runat="server" Visible="false" Text='<%# Eval("smname")%>'></asp:Label></td>
                                        <td><%#Eval("partyname") %>
                                        <asp:Label ID="lblpartyname" runat="server" Visible="false" Text='<%# Eval("partyname")%>'></asp:Label></td>
                                        <td><%#Eval("mode") %>
                                        <asp:Label ID="lblmode" runat="server" Visible="false" Text='<%# Eval("mode")%>'></asp:Label></td>
                                        <td><%#Eval("Cheque_DDNo") %>
                                        <asp:Label ID="lblcheckdd" runat="server" Visible="false" Text='<%# Eval("Cheque_DDNo")%>'></asp:Label></td>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}",Eval("Cheque_DD_Date")) %>
                                        <asp:Label ID="lblcheckdate" runat="server" Visible="false" Text='<%#String.Format("{0:dd/MMM/yyyy}", Eval("Cheque_DD_Date"))%>'></asp:Label></td>
                                        <td><%#Eval("bank") %>
                                        <asp:Label ID="lblbank" runat="server" Visible="false" Text='<%# Eval("bank")%>'></asp:Label></td>
                                        <td><%#Eval("Branch") %>
                                        <asp:Label ID="lblbranch" runat="server" Visible="false" Text='<%# Eval("Branch")%>'></asp:Label></td>
                                        <td><%#Eval("amount") %>
                                        <asp:Label ID="lblamount" runat="server" Visible="false" Text='<%# Eval("amount")%>'></asp:Label></td>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}",Eval("PaymentDate")) %>
                                        <asp:Label ID="lblpaymentdate" runat="server" Visible="false" Text='<%#String.Format("{0:dd/MMM/yyyy}", Eval("PaymentDate"))%>'></asp:Label></td>
                                        <td><%#Eval("remarks") %>
                                        <asp:Label ID="lblremark" runat="server" Visible="false" Text='<%# Eval("remarks")%>'></asp:Label></td>
                                       
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rptFaildvisit" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Date</th>
                                                <th>Sale Person</th>
                                                <th>Party</th>
                                                <th>Reason</th>
                                                <th>Remarks</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("SMId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("SMId") %>' />
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("created_date"))%>
                                       <asp:Label ID="lbldte" runat="server" Visible="false" Text='<%#String.Format("{0:dd/MMM/yyyy}", Eval("created_date"))%>'></asp:Label></td>
                                        <td><%#Eval("smname") %>
                                        <asp:Label ID="lblsname" runat="server" Visible="false" Text='<%# Eval("smname")%>'></asp:Label></td>
                                        <td><%#Eval("partyname") %>
                                        <asp:Label ID="lblptyname" runat="server" Visible="false" Text='<%# Eval("partyname")%>'></asp:Label></td>
                                        <td><%#Eval("Reason") %>
                                       <asp:Label ID="lblreason" runat="server" Visible="false" Text='<%# Eval("Reason")%>'></asp:Label></td>
                                        <td><%#Eval("remarks") %>
                                       <asp:Label ID="lblremark" runat="server" Visible="false" Text='<%# Eval("remarks")%>'></asp:Label></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rptDemo" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Date</th>
                                                <th>Sale Person</th>
                                                <th>Party</th>
                                                <th>Product Class</th>
                                                <th>Product Segment</th>
                                                <th>Product Group</th>
                                                <th>Remarks</th>
                                              
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("SMId") %>');">
                                       
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("SMId") %>' />
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("created_date"))%>
                                        <asp:Label ID="lbldate" runat="server" Visible="false" Text='<%#String.Format("{0:dd/MMM/yyyy}", Eval("created_date"))%>'></asp:Label></td>
                                        <td><%#Eval("smname") %>
                                        <asp:Label ID="lblsmname" runat="server" Visible="false" Text='<%# Eval("smname")%>'></asp:Label></td>
                                        <td><%#Eval("partyname") %>
                                        <asp:Label ID="lblpartyname" runat="server" Visible="false" Text='<%# Eval("partyname")%>'></asp:Label></td>
                                        <td><%#Eval("ProdClass") %>
                                        <asp:Label ID="lblprodclass" runat="server" Visible="false" Text='<%# Eval("ProdClass")%>'></asp:Label></td>
                                        <td><%#Eval("ProdSegment") %>
                                        <asp:Label ID="lblsegment" runat="server" Visible="false" Text='<%# Eval("ProdSegment")%>'></asp:Label></td>
                                        <td><%#Eval("ProductGrp") %>
                                        <asp:Label ID="lblgrp" runat="server" Visible="false" Text='<%# Eval("ProductGrp")%>'></asp:Label></td>
                                        <td><%#Eval("remarks") %>
                                        <asp:Label ID="lblremark" runat="server" Visible="false" Text='<%# Eval("remarks")%>'></asp:Label></td>
                                         
                                       
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rptCompetitor" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Date</th>
                                                <th>Sale Person</th>
                                                <th>Party</th>
                                                <th>Item</th>
                                                <th>Std. Packing</th>
                                                <th>Rate</th>
                                                <th>Discount %</th>
                                                <th>Remarks</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("SMId") %>');">
                                       
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("SMId") %>' />
                                       <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("created_date"))%>
                                        <asp:Label ID="lbldate" runat="server" Visible="false" Text='<%#String.Format("{0:dd/MMM/yyyy}", Eval("created_date"))%>'></asp:Label></td>
                                        <td><%#Eval("smname") %>
                                        <asp:Label ID="lblsmname" runat="server" Visible="false" Text='<%# Eval("smname")%>'></asp:Label></td>
                                        <td><%#Eval("partyname") %>
                                        <asp:Label ID="lblpartyname" runat="server" Visible="false" Text='<%# Eval("partyname")%>'></asp:Label></td>
                                        <td><%#Eval("item") %>
                                        <asp:Label ID="lblitem" runat="server" Visible="false" Text='<%# Eval("item")%>'></asp:Label></td>
                                        <td><%#Eval("qty") %>
                                        <asp:Label ID="lblqty" runat="server" Visible="false" Text='<%# Eval("qty")%>'></asp:Label></td>
                                        <td><%#Eval("rate") %>
                                        <asp:Label ID="lblrate" runat="server" Visible="false" Text='<%# Eval("rate")%>'></asp:Label></td>
                                        <td><%#Eval("discount") %>
                                        <asp:Label ID="lbldiscount" runat="server" Visible="false" Text='<%# Eval("discount")%>'></asp:Label></td>
                                        <td><%#Eval("remarks") %>
                                        <asp:Label ID="lblremark" runat="server" Visible="false" Text='<%# Eval("remarks")%>'></asp:Label></td>
                                       
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
                </div>
                <!-- /.col -->
            </div>

        </div>
            </ContentTemplate>
              <Triggers>
              <asp:PostBackTrigger ControlID="FromDate"/>
               <asp:PostBackTrigger ControlID="ToDate"/>
               <asp:PostBackTrigger ControlID="ddlmode"/>
               <asp:PostBackTrigger ControlID="ddlFailedVisit"/>
             </Triggers>
        </asp:UpdatePanel>

    </div>
       </section>
   


    <script type="text/javascript">
           function showspinner() {

            $("#spinner").show();

        };
        function hidespinner() {

            $("#spinner").hide();

        };
       
     </script> 
  <style>
       .primaryHeading {
           background-color:rgba(243,156,18,0.1);
       }
        .secondaryHeading {
           background-color:rgba(0,166,90,0.1);
       }
    .panel-heading.primaryHeading {
    background-color: rgb(0,166,90);
    color: white;
    font-size: 20px;
}
    .panel-heading.secondaryHeading {
    background-color: rgba(243, 156, 18,0.8);
    border-color: black;
    color: white;
    font-size: 20px;
}
    .panel-heading.UnApprovedHeading {
    background-color: rgba(0,192,239,1.0);
    border-color: black;
    color: white;
    font-size: 20px;
}
       .TotalEmp {
             background-color:rgb(149, 176, 219);
           border-style: solid;
            border-color:rgb(98, 132, 186);
          color:white;
       }
       .Present {
           background-color: rgba(0,141,76,0.3);
          color:white;
            border-style: solid;
            border-color:rgb(57, 173, 105);
       }
       .Apsent {
            background-color:rgba(211, 55, 36, 0.4);
            border-style: solid;
            border-color:rgb(193, 131, 125);
          color:white;
       }
       .Leave {
            background-color:rgba(255, 119, 1, 0.5);
          color:white;
           border-style: solid;
            border-color:rgb(219, 148, 87);
       }

       
.fa-minus::before {
    color: white;
    content: "";
}
   </style>
    <style>
       /*.small-box > .small-box-footer {
    background: rgba(0, 0, 0, 0.1) none repeat scroll 0 0;
    color: rgba(255, 255, 255, 0.8);
    display: block;
    margin-top: 61px;
    padding: 3px 0;
    position: relative;
    text-align: center;
    text-decoration: none;
    z-index: 10;
}*/
       .inner > p {
    font-size:27px;
    /*text-align: center;*/
}
       .inner > span {
    font-size: 27px;
    /*padding-left: 124px;*/
}
       .headerbottom {
           margin-bottom:10px;
       }
   </style>
  <%-- <script type="text/javascript">
        $(function () {
           

        });
    </script>--%>

     <script type="text/javascript">
         $(function () {
             $('#example1').dataTable({
                 "order": [[0, "asc"]],
                 "footerCallback": function (tfoot, data, start, end, display) {
                     //$(tfoot).find('th').eq(0).html("Starting index is " + start);
                     var api = this.api();
                     var intVal = function (i) {
                         return typeof i === 'string' ?
                             i.replace(/[\$,]/g, '') * 1 :
                             typeof i === 'number' ?
                             i : 0;
                     };
                     debugger;
                     var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Qty'; }).first().index();
                     var totalData = api.column(costColumnIndex).data();
                     var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                     var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
                     var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                     var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
                     var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);


                     var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Amount'; }).first().index();
                     var totalData1 = api.column(costColumnIndex1).data();
                     var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                     var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
                     var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
                     var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
                     var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);



                     $(api.column(4).footer()).html(searchTotal);
                     $(api.column(5).footer()).html(searchTotal1);

                     if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(4).footer()).html('0.0') }
                     if (searchTotal1 == 'NaN' || searchTotal1 == '') { $(api.column(5).footer()).html('0.0') }

                     //if (searchTotal2 == 'NaN' || searchTotal2 == '') { $(api.column(9).footer()).html('0.0') }
                 }
             });
         });
    </script>

     
</asp:Content>