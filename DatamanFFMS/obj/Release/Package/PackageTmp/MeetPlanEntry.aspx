<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="MeetPlanEntry.aspx.cs" EnableEventValidation="true" Inherits="AstralFFMS.MeetPlanEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <style>
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
    </style>
   
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
    <script type="text/javascript">
        $(document).ready(function () {
         
        });
        $(function () {
         
            $('[id*=Ddldistributor]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',
                buttonWidth: '100%',
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });

      $(function () {
          $('[id*=ddlParty]').multiselect({
              enableCaseInsensitiveFiltering: true,
              //buttonWidth: '200px',
              buttonWidth: '100%',
              //includeSelectAllOption: true,
              maxHeight: 200,
              width: 215,
              enableFiltering: true,
              filterPlaceholder: 'Search'
          });
      });
   
        
        function showspinner() {
       
            $("#spinner").show();

        };
        function hidespinner() {

            $("#spinner").hide();

        };
        function openProduct() {
           
            $("#ContentPlaceHolder1_mainDiv").hide();
            $("#ContentPlaceHolder1_secDiv").show();

        };
      
        function closeProduct() {
            $('#spinner').show();
          

        };
    </script>
  
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
         <%--   $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "success"
            });
         <%--   $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");--%>

        }
    </script>


    <%--<script type = "text/javascript">
    function ClientItemSelected(sender, e) {
        $get("<%=hfCustomerId.ClientID %>").value = e.get_value();
    }
</script>
    <script type = "text/javascript">
        function SetContextKey() {
          $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey($get("<%=ddlbeat5.ClientID %>").value);
    }
</script>--%>

    <%--  <script type = "text/javascript">
        function ClientItemSelected1(sender, e) {
<%--            $get("<%=hfitemid.ClientID %>").value = e.get_value();
    }
</script>--%>


    <script type="text/javascript">
        function ClientItemSelectedParty(sender, e) {

            $get("<%=Hidparty.ClientID %>").value = e.get_value();
        }
    </script>
    <script type="text/javascript">
        function SetContextKeyParty() {

            $find('<%=AutoCompleteExtender3.ClientID%>').set_contextKey($get("<%=ddlParty.ClientID %>").value);
        }
    </script>
    <script>
        function Validation() {

            if ($('#<%=txtMeetDate.ClientID%>').val() == '') {
                errormessage("Please enter the Meet Date");
                return false;
            }
            if ($('#<%=ddlmeetType.ClientID%>').val() == '0') {
                errormessage("Please select the Meet Type");
                return false;
              
            }
           <%-- if ($('#<%=Ddldistributor.ClientID%>').val() == "") {                
                errormessage("Please select the Distributor");
                return false;

            }--%>
           
            if ($('#<%=ddlTOG.ClientID%>').val() == '0') {
                errormessage("Please select the Type of Gift");
                return false;

            } 
            if ($('#<%=ddlindrustry.ClientID%>').val() == '0') {
                errormessage("Please select the Product Class");
                return false;
            }

         
            if ($('#<%=txtNoOfUsers.ClientID%>').val() == '') {
                errormessage("Please enter the No of Users");
                return false;
            }
            if ($('#<%=txtNoOfUsers.ClientID%>').val() == "0") {
                errormessage("No of Users should be greater than 0");
                return false;
            }
            if ($('#<%=ddlmeetCity.ClientID%>').val() == "0") {
                errormessage("Please select The City");
                return false;
            }

            if ($('#<%=txtVenue.ClientID%>').val() == '') {
                errormessage("Please enter the Venue");
                return false;
            }
            
            if ($('#<%=txtgiftqty.ClientID%>').val() == '') {
                errormessage("Please enter the Qty Required");
                return false;
            }
            if ($('#<%=ddlTOG.ClientID%>').val() != "1") {
                if ($('#<%=txtgiftqty.ClientID%>').val() == '0') {
                    errormessage("Please enter the Qty Required greater then Zero");
                    return false;
                }
            }
            if ($("#ContentPlaceHolder1_txtComments").val() == '') {
                errormessage("Please Enter the Comments");
                return false;
            }


            if ($('#<%=txtApproxBudget.ClientID%>').val() == '') {
                errormessage("Please enter the approx budget");
                return false;
            }

            if ($('#<%=txtApproxBudget.ClientID%>').val() <=0) {
                errormessage("Please enter approx budget value greater than 0");
                return false;
            }
            if (document.getElementById("<%=Ddldistributor.ClientID%>").value == "" || document.getElementById("<%=Ddldistributor.ClientID%>").value == "0") {
                errormessage("Please Select Distributor");
                return false;

            }
         
          
            var count = 0;
            var gridView1 = $("#GridView2");
            var l = gridView1.length;
            if (l == 0)
            {
                errormessage("Please enter Meet Product");
                $("#ContentPlaceHolder1_mainDiv").hide();
                $("#ContentPlaceHolder1_secDiv").show();
                return false;
            }
          
           $('#spinner').show();
        }
    </script>

      <script type="text/javascript">
          function SetContextKey() {
          <%--    $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey($get("<%=hdfCityIdStr.ClientID %>").value);--%>
        }
    </script>
    <script type="text/javascript">
        function ClientPartySelected(sender, e) {
            $get("<%=hfDistId.ClientID %>").value = e.get_value();
        }
    </script>

    <script type="text/javascript">
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        function IsNumeric(e) {
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);

            return ret;
        }
    </script>
    <script type="text/javascript">
        
    </script>
    <section class="content">
        <script type="text/javascript">
            function load1() {

                $(".numeric").numeric({ negative: false });
            }

            $(window).load(function () {

                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(load1);

            });


        </script>
         <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/loader.gif" alt="Loading" /><br />
            Loading Data....
        </div>
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
         <div class="box-body" id="secDiv" runat="server">
            <div class="row">
         <div class="col-xs-12">
            <div class="box">
                <div class="box-header">
                    <h3 class="box-title">Meet Products</h3>
                      <div style="float: right">
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnback" runat="server" Text="Back" class="btn btn-primary" OnClientClick="closeProduct()" />
                            </div>
                </div>
              
                <div class="box-body">
                    <div class="col-md-12">
                        <asp:UpdatePanel ID="up" runat="server">
                            <ContentTemplate>
                                <div class="col-md-5">
                                    <div class="form-group" style="display:none;" >
                                        <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="form-group" style="display:none;"  >
                                        <label for="exampleInputEmail1">Meet Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="DropDownList2" AutoPostBack="true" OnSelectedIndexChanged="ddlmeetType1_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="form-group" style="display:none;">
                                        <label for="exampleInputEmail1">Meet Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:DropDownList ID="ddlmeet" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlmeet_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                       <%-- <label for="exampleInputEmail1">Product to be demonstrate/introduce :</label>--%>
                                        <div class="form-group paddingleft0">
                                            <label for="exampleInputEmail1">Product Class: </label>
                                            <asp:DropDownList ID="ddlclass" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="form-group paddingleft0">
                                            <label for="exampleInputEmail1">Product Segment:</label>
                                            <asp:DropDownList ID="ddlsegment" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="form-group paddingleft0">
                                            <label for="exampleInputEmail1">Product Group:</label>
                                            <asp:DropDownList ID="ddlgroup" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>


                                    <div class="box-footer">
                                        <asp:Button ID="btnAdd" runat="server" class="btn btn-primary" Text="Add" OnClick="btnAdd_Click" OnClientClick="showspinner();" />

                                    </div>
                                </div>
                                <div class="form-group col-md-12 table-responsive">
                                    <asp:GridView ID="GridView2" runat="server" ClientIDMode="Static" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" AutoGenerateColumns="False" OnRowDeleting="GridView2_RowDeleting">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr No.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Product Class">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hidMaterialClass" runat="server" Value='<%#Eval("MatrialClassId")%>' />
                                                    <asp:Label ID="lbMaterialClass" runat="server" Text='<%#Eval("MatrialClass")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product Segment">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hidSegment" runat="server" Value='<%#Eval("SegmentId")%>' />
                                                    <asp:Label ID="lblSegment" runat="server" Text='<%#Eval("Segment")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product Group">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hidProductgroup" runat="server" Value='<%#Eval("ProdctGroupId")%>' />
                                                    <asp:Label ID="lblPGName" runat="server" Text='<%#Eval("ProdctGroup")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ControlStyle-CssClass="btn btn-primary" ButtonType="Button" />
                                        </Columns>
                                    </asp:GridView>
                                    <div class="box-footer">
                                        <asp:Button ID="btnsave1" runat="server" class="btn btn-primary" Visible="false" Text="Save" OnClick="btnsave1_Click" />
                                        <asp:Button ID="btncancel" runat="server" class="btn btn-primary" Text="Cancel" Visible="false" OnClientClick="closeProduct();" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

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
          <div class="box-body" id="mainDiv" runat="server">
        <div class="row" >
         <%--   <asp:UpdatePanel ID="updatePa" runat="server" UpdateMode="Conditional">
                <ContentTemplate>--%>
                    <!-- left column -->
                    <div class="col-md-12">
                        <div id="InputWork">
                            <!-- general form elements -->

                            <div class="box box-primary">
                                <div class="box-header with-border">
                                   <%-- <h3 class="box-title">Meet Plan Entry</h3>--%>
                                    <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                                    <div style="float: right">
                                      <div style="float: right">
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                        OnClick="btnFind_Click" />
                                </div>
                                    </div>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="col-lg-10 col-md-12 col-sm-10 col-xs-10">
                                        <div class="form-group col-md-5 paddingleft0">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlunderUser" AutoPostBack="true" OnSelectedIndexChanged="ddlunderUser_SelectedIndexChanged" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-3 paddingleft0 paddingright0">
                                            <label for="visitDate">Meet Date:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtMeetDate" runat="server" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="orange" Format="dd/MMM/yyyy"
                                                BehaviorID="calendarTextBox_CalendarExtender" TargetControlID="txtMeetDate"></ajaxToolkit:CalendarExtender>
                                        </div>
                                        <div class="form-group col-md-4 paddingright0">
                                            <label for="exampleInputEmail1">Meet Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlmeetType" runat="server" Width="100%" CssClass="form-control" AppendDataBoundItems="false" OnSelectedIndexChanged="ddlmeetType_SelectedIndexChanged"></asp:DropDownList>
                                        </div>

                                          <div class="form-group col-md-5 paddingleft0">
                                            <label for="exampleInputEmail1">Product Class:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlindrustry" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                        </div>


                                        <div class="form-group col-md-2 paddingleft0 paddingright0">
                                            <label for="exampleInputEmail1">No of Users:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtNoOfUsers" onkeypress="return IsNumeric(event);" runat="server" MaxLength="5" AutoPostBack="false"  placeholder="" CssClass="form-control text-right"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-2 paddingright0">
                                            <label for="exampleInputEmail1">No of Staff:</label>
                                            <asp:TextBox ID="txtNoofStaf" onkeypress="return IsNumeric(event);" runat="server" MaxLength="5" CssClass="form-control text-right"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-3 paddingright0">
                                            <label for="exampleInputEmail1">Meet City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                           <%-- <asp:DropDownList ID="ddlmeetCity" Width="100%" runat="server" CssClass="form-control select2"></asp:DropDownList>--%>
                                             <asp:DropDownList ID="ddlmeetCity" Width="100%" runat="server" CssClass="form-control" 
                                                 OnSelectedIndexChanged="ddlmeetCity_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>

                                        <div class="form-group col-md-6 paddingleft0">
                                            <label for="exampleInputEmail1">Beat:</label>
                                            <asp:DropDownList ID="ddlbeat5" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlbeat5_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>

                                        <div class="form-group col-md-6 paddingright0">
                                            <label for="exampleInputEmail1">Party:</label>
                                            <asp:TextBox ID="txtParty" runat="server" MaxLength="8" class="form-control" onkeyup="SetContextKeyParty();" Visible="false"></asp:TextBox>
                                            <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" ServiceMethod="SearchParty" ServicePath="MeetPlanEntry.aspx"
                                                TargetControlID="txtParty" UseContextKey="true" FirstRowSelected="false" OnClientItemSelected="ClientItemSelectedParty" MinimumPrefixLength="3" EnableCaching="true">
                                            </ajaxToolkit:AutoCompleteExtender>
                                          <%--  <asp:DropDownList ID="ddlParty" AutoPostBack="true" Width="100%" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                             <asp:ListBox ID="ddlParty" runat="server"   Width="100%" SelectionMode="Multiple"></asp:ListBox>  
                                            <asp:HiddenField ID="Hidparty" runat="server" />
                                        </div>


                                        <div class="form-group col-md-6 paddingleft0">
                                            <label for="exampleInputEmail1">Venue:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtVenue" runat="server" placeholder="Enter the Venue" Rows="6" Cols="3" Height="50%" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-6 paddingright0">
                                            <label for="exampleInputEmail1">Comments:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtComments" runat="server" placeholder="Enter the Comments" Rows="6" Cols="3" Height="50%" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-5 paddingleft0">
                                            <label for="exampleInputEmail1">Type of Gifts(Kit) :</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                           <asp:DropDownList ID="ddlTOG" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTOG_SelectedIndexChanged"  CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="form-group  col-md-2 paddingright0">
                                            <label for="exampleInputEmail1">Quantity Required:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtgiftqty" MaxLength="12" runat="server" CssClass="form-control numeric text-right"></asp:TextBox>
                                        </div>
                                        <div class="form-group  col-md-5 paddingright0">
                                            <label for="exampleInputEmail1">Value:</label>
                                            <asp:TextBox ID="txtvalueforenduser" MaxLength="12" runat="server" CssClass="form-control numeric text-right"></asp:TextBox>
                                        </div>
                              
                                        <div class="form-group col-md-3 paddingleft0">
                                            <label for="exampleInputEmail1">Meet Approx Budget:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:TextBox ID="txtApproxBudget" runat="server" MaxLength="12" CssClass="form-control numeric text-right"></asp:TextBox>
                                        </div>

                                        <div class="form-group col-md-3 paddingleft0 paddingright0">
                                            <label for="exampleInputEmail1">DATAMAN Sharing %:</label>
                                            <asp:TextBox ID="txtastralSharing" MaxLength="3" runat="server" CssClass="form-control numeric text-right"></asp:TextBox>
                                        </div>

                                        <!--Added As Per UAT 07-12-2015-->
                                        <div class="form-group col-md-3 paddingright0 ">
                                            <label for="exampleInputEmail1">Scheme Code:</label>
                                            <asp:DropDownList ID="ddlscheme" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                        </div>


                                        
                                        <!--End-->

                                        <div class="form-group col-md-3 paddingright0">
                                            <label for="exampleInputEmail1">Distributor Sharing %:</label>
                                            <asp:TextBox ID="txtDistributerSharing" runat="server" MaxLength="3" CssClass="form-control numeric text-right"></asp:TextBox>
                                        </div>

                                        <div class="form-group col-md-6 paddingleft0" style="">
                                            <label for="exampleInputEmail1">Distributor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:HiddenField ID="hdfCityIdStr" runat="server" />
                                             <asp:DropDownList ID="Ddldistributor" Width="100%" runat="server"  CssClass="form-control"></asp:DropDownList>
                                           <%--  <asp:ListBox ID="Ddldistributor" runat="server"   Width="100%"></asp:ListBox>--%>  
                                            <asp:HiddenField ID="hfDistId" runat="server" />
                                        </div>
                                        
                                      
                                    </div>
                                    <div class="clearfix"></div>
                                       <asp:UpdatePanel ID="updatePa" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                                    <div class="box-footer">
                                        <asp:Button ID="btnsave" CssClass="btn btn-primary" OnClientClick="return Validation();" runat="server" Text="Save" OnClick="btnsave_Click" />
                                        <asp:Button ID="Cancel" CssClass="btn btn-primary" runat="server"  OnClientClick="return Validation();" Text="Cancel" OnClick="Cancel_Click" />
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="load()" OnClick="btnDelete_Click" />
                                        <asp:Button ID="btnMeetprdct" CssClass="btn btn-primary" runat="server" Text="Meet Product" OnClientClick="openProduct();" /><span style="color: red; font-size:large; ">*</span>
                                    </div>
 </ContentTemplate>
            </asp:UpdatePanel>
                                    <br />
                                    <div>
                                        <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b><br />
                                        <b>Note : Meet product entry is mandatory to fill (<span style="color: red">*</span>)</b>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
              <%-- </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
         </div>
      
    </section>
    
</asp:Content>

