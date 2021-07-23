<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/FFMS.Master" CodeBehind="ItemPriceDistWise.aspx.cs" Inherits="AstralFFMS.ItemPriceDistWise" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>

    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "error"
            });
            $('#<%=lblmsg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");


        }

        function geek() {

            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "success"
            });
            $('#<%=lblmsg.ClientID %>').html("Records Updated Successfully.");
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
            $('#<%=lblmsg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");
        }
    </script>

    <script type="text/javascript">
        $(function () {
            $('[id*=ddlProductGroup]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
    </script>

    <script type="text/javascript">
        function ValidateData()
        {
            var distributor = $('#<%=ddlDistributor.ClientID %>').val();
            var productgroup = $('#<%=ddlProductGroup.ClientID %>').val();

            if (distributor == "0") {
                errormessage('Please Select Distributor.');
                return false;
            }

            if (productgroup == "0") {
                errormessage('Please Select Product Group.');
                return false;
            }

            //document.getElementById("ContentPlaceHolder1_Div1").style.display = "block";
            //document.getElementById("ContentPlaceHolder1_mainDiv").style.display = "none";
            
        }

        function editBtn()
        {

            ValidateData();
            <%--var txt = document.getElementById("<%=ContentPlaceHolder1_rpt_distPrice_3.ClientID%>");
            txt.readOnly = false;--%>

        }
        
    </script>

    <script type="text/javascript">
        function DoNav(Id) {
            if (Id != "") {
               // alert("DoNav");
                //document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                //document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                //$('#spinner').show();
                //__doPostBack('', Id)
            }
        }

        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            //alert(iKeyCode);
            if (iKeyCode != 9 && iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57)) {

                // if (iKeyCode != 8 && iKeyCode != 0 && (iKeyCode < 48 || iKeyCode > 57))

                return false;
            }
            return true;
        }

        function checkVal(a)
        {
            //console.log(a);
            var dp = $('#' + a).val();
            //alert(dp);
            if(dp == "")
            {
                //alert("hi");

                $('#' + a).val("0.00");
            }

            if ($('#' + a).val() != "0.00") {
                var value = parseFloat(dp).toFixed(2);
                $('#' + a).val(value);
            }
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {

        });
    </script>

     <style type="text/css">
        /*.input-group .form-control {
            height: 34px;
        }
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            /*display: table;*/
        }*/

        .multiselect-container > li > a {
            white-space: normal;
        }

        .multiselect-container > li {
            width: 100%;
        }

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }

       
    </style>

    <section class="content">

        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmsg" runat="server"></asp:Label>
            </div>
        </div>

        <div class="box-body" id="mainDiv" runat="server">
            <div class="row" style="background-color:white !important;">
                <!-- left column -->
                <div class="col-md-12">
                    <!-- general form elements -->
                    <div class="box box-default" style="box-shadow:none !important;">
                        <div class="box-header">
                            <%--<h3 class="box-title">Item Price Distributor Wise</h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                         </div>
                        
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="row">
                                <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Distributor:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlDistributor" Width="100%" CssClass="form-control" runat="server" AutoPostBack="True" onselectedindexchanged="ddlDistributor_SelectedIndexChanged">     <%--//AutoPostBack="true"  OnSelectedIndexChanged="ddlGeoType_SelectedIndexChanged"--%>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Product Group:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                       <%-- <asp:DropDownList ID="ddlProductGroup" Width="100%" CssClass="form-control" runat="server" AutoPostBack="True" onselectedindexchanged="ddlProductGroup_SelectedIndexChanged" ></asp:DropDownList>--%>
                                         <asp:ListBox ID="ddlProductGroup" runat="server" SelectionMode="Multiple"
                                                    OnSelectedIndexChanged="ddlProductGroup_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                    </div>
                                    <br />
                                    <div>
                                        <%--<asp:Button Style="margin-right: 5px;" type="button" ID="btnGetPrice" runat="server" Text="GetPrice" class="btn btn-primary" OnClientClick="return ValidateData();" OnClick="btnGetPrice_Click" />--%>
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click"/>
                                    </div>
                                </div>
                            </div>
                                 <br />
                             <div class="row">
                                 <div id="Div1" runat="server" style="display:none;">
                                    <div class="col-lg-7 col-md-8 col-sm-8 col-xs-10">
                                        <div class="box-body table-responsive">
                                            <asp:Repeater ID="rpt" runat="server" >
                                                <HeaderTemplate>
                                                    <table id="example1" class="table table-bordered table-striped">
                                                        <thead>
                                                            <tr>                                                              
                                                                <th>Item Name</th>
                                                                <th>Product Group</th>
                                                                <th>Item Price</th>
                                                                <th>Distributor Wise Item Price</th>
                                                                <th style="text-align: center;">                                                        
                                                                <asp:CheckBox ID="chkAll" runat="server" Text="Same as Item Master Price" OnCheckedChanged="chkAll_CheckedChanged" AutoPostBack="true" /></th>                                                               
                                                            </tr>
                                                        </thead>
                                                         <tbody>
                                                </HeaderTemplate>
                                                   
                                                <ItemTemplate>
                                                    <tr onclick="DoNav('<%#Eval("ItemId") %>');">
                                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("ItemId") %>' />                                                        
                                                        <td><%#Eval("ItemName") %></td>
                                                        <td><%#Eval("ProductGroup") %>
                                                            <asp:Label ID="lblProductgroupId" Visible="false" runat="server" Text='<%# Eval("ProductgroupId")%>'></asp:Label>
                                                        </td>                                                        
                                                        <td><asp:Label ID="lblItemPrice" runat="server" Text='<%# Eval("ItemDP")%>'></asp:Label></td>     
                                                        <td>
                                                            <input type="text" runat="server" id="distPrice" onfocus="this.select()" onchange="checkVal(this.id)" onkeypress="return isNumber(event)" MaxLength="12" Class="form-control numeric text-right" Value=<%#Eval("DistPrice") %>/>
                                                            <%--<asp:TextBox ID="distPrice" onfocus= "this.select()" AutoPostBack="true" OnTextChanged="distPrice_TextChanged" onkeypress="return isNumber(event)" MaxLength="12" runat="server" CssClass="form-control numeric text-right" Text=<%#Eval("DistPrice") %>> </asp:TextBox>--%>
                                                        </td> 
                                                        <td style="text-align: center;">
                                                        <asp:CheckBox ID="chkItem" runat="server" OnCheckedChanged="chkItem_CheckedChanged" AutoPostBack="true" />
                                                        </td>                                                      
                                                    </tr>
                                                </ItemTemplate>

                                                <FooterTemplate>
                                                        </tbody>     
                                                    </table>   
                                                 </FooterTemplate>    
                                           </asp:Repeater>
                                        </div>
                                        <br />
                        
                                        <asp:HiddenField ID="HiddenField2" runat="server"  />       
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnEdit" runat="server" Text="Save" class="btn btn-primary" OnClick="btnEdit_Click" OnClientClick="return editBtn();"/>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="box-footer">
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
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

    
</asp:Content>
