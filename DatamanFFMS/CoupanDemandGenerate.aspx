<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/FFMS.Master" EnableEventValidation="false" CodeBehind="CoupanDemandGenerate.aspx.cs" Inherits="AstralFFMS.CoupanDemandGenerate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
  
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
    </script>
    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();
            $("#example11").DataTable();
            
        });
    </script>   

     <script type="text/javascript">

         //function postBackByObject() {
         //    var o = window.event.srcElement;
         //    if (o.tagName == "INPUT" && o.type == "checkbox") {
         //        __doPostBack("", "");
         //    }
         //}        

         function loding() {
             $('#spinner').show();
         }
   </script>

    <style type="text/css">
        .containerStaff {
            border: 1px solid #ccc;
            overflow-y: auto;
            min-height: 200px;
            width: 134%;
            overflow-x: auto;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .multiselect-container > li > a {
            white-space: normal;
        }
         .multiselect-container.dropdown-menu {
            width: 100% !important;
        }

        .input-group .form-control {
            height: 34px;
        }

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
        <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Coupan Demand Generation</h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                               <%--  <div class="col-md-10">--%>
                                <div class="row">                                 
                                   <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Distributor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br /> 
                                            <asp:DropDownList ID="ListBox1" class="form-control" runat="server"></asp:DropDownList>                                       
                                          <%--  <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>--%>
                                            <input type="hidden" id="hiddistributor" />
                                        </div>
                                    </div>
                                 </div>                                
                                    <div class="row">   
                                     <div class="col-md-2 col-sm-6 col-xs-12">
                                         <div class="form-group">
                                             <label for="exampleInputEmail1">Months:</label>
                                              <asp:DropDownList ID="listboxmonth" class="form-control" runat="server"></asp:DropDownList>
                                            <%-- <asp:ListBox ID="listboxmonth" runat="server" SelectionMode="Multiple"></asp:ListBox>            --%>     
                                                                      
                                                  
                                        </div>
                                     </div>
                                         </div> 
                                   <div class="row">      
                                    <div class="col-md-2 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Year:</label>  
                                             <asp:DropDownList ID="YearListBox" class="form-control" runat="server"> </asp:DropDownList>                                        
                                           <%-- <asp:ListBox ID="YearListBox" runat="server" SelectionMode="Multiple"></asp:ListBox>--%>
                                          
                                        </div>
                                    </div>                             
                                </div>   
                                 
                           <%-- </div> --%>                           
                        </div>
                                

                            <div class="box-footer">                               
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClientClick="loding()"  
                                    OnClick="btnGo_Click" Visible="true" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                    OnClick="btnCancel_Click" Visible="false" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                    OnClick="btnExport_Click" />
                            </div>                            
                               <div id="rptmain1" runat="server">
                            <div  class="box-body table-responsive">
                                <asp:Repeater ID="distreportrpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="example11" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>                                                                                             
                                                        <th>Retailer</th>
                                                        <th style="text-align: right">Sale</th>
                                                        <th style="text-align: right">Coupan Value</th>
                                                        <th style="text-align: right">Coupan Qty.</th>                                                                                                                                                                               
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>                                           
                                             <asp:HiddenField ID="hdnDistId" runat="server" Value='<%#Eval("DistId") %>' />
                                             <asp:HiddenField ID="hdnMonth" runat="server" Value='<%#Eval("Month") %>' />
                                             <asp:HiddenField ID="hdnYear" runat="server" Value='<%#Eval("Year") %>' />
                                             <asp:HiddenField ID="hdnRetailerId" runat="server" Value='<%#Eval("Id") %>' />
                                            <td><%#Eval("RetailerName") %>                                         
                                            <asp:Label ID="lblRetailerName" runat="server" Visible="false" Text='<%# Eval("RetailerName")%>'></asp:Label></td>                                          
                                            
                                            <td style="text-align: right"><%#Eval("SaleAmount","{0:#.##}") %>                                    
                                            <asp:Label ID="lblSaleAmount" runat="server" Visible="false" Text='<%# Eval("SaleAmount")%>'></asp:Label></td>
                                            
                                            <td style="text-align: right"><%#Eval("CoupanValue","{0:#.##}") %>                                         
                                            <asp:Label ID="lblCoupanValue" runat="server" Visible="false" Text='<%# Eval("CoupanValue")%>'></asp:Label></td>
                                            
                                            <td style="text-align: right"><%#Eval("CoupanQty","{0:#.##}") %></td>                                           
                                            <asp:Label ID="lblCoupanQty" runat="server" Visible="false" Text='<%# Eval("CoupanQty")%>'></asp:Label>                                                                                                                                            
                                           
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                                   </div>
                            <br />                            
                        <div class="box-footer">
                               <%-- <input type="button" runat="server" onclick="btnSubmitfunc();" class="btn btn-primary" value="Process" id="ledgergo" />  --%>
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGenerate" runat="server" Text="Generate" class="btn btn-primary" OnClick="btnGenerate_Click" Visible="false" />                                
                            </div>    
                            <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script>
        function getUrlVars() {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;

        }

        var second = getUrlVars()["DistId"];

        if (window.location.href.indexOf(second) > -1) {
            $("#ContentPlaceHolder1_ledgergo").trigger("click");

        }

    </script>

</asp:Content>
