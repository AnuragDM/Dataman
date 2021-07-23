<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="RetailerInvoiceReport.aspx.cs" Inherits="AstralFFMS.RetailerInvoiceReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>

    <script type="text/javascript">
        $(function () {
            $('[id*=ListBox1]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=ListState]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 300,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });

            $('[id*=ListCity]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 300,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });

            $('[id*=ListDistributor]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 300,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });


            $('[id*=Listsalesperson]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 300,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
    </script>
     <script type="text/javascript">
         $(function () {
             $('[id*=salespersonListBox]').multiselect({
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
        $(document).ready(function () {
            BindState();
            BindSalesPerson();
        })
        function BindSalesPerson()
        {
            $('#<%=Listsalesperson.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: 'RetailerInvoiceReport.aspx/BindSalesPerson',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                error: function (response) {
                    alert(response.d);
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=Listsalesperson.ClientID %>"));
            }
            function PopulateControl(list, control) {
                console.log(JSON.parse(list))
                var data = JSON.parse(list);
                $('#<%=Listsalesperson.ClientID %>').empty();
                $.each(data, function () {

                    $('#<%=Listsalesperson.ClientID %>').append('<option  value=' + this['Id'] + '>' + this['Name'] + '</option>');


                        });
                        var id = $('#<%=Hidden2.ClientID%>').val();
                //alert(id);
                var splittedArray = id.split(",");
                //  alert(splittedArray);

                if (id != "") {
                    //alert('c');
                    control.val(splittedArray);
                }
                $("#<%=Listsalesperson.ClientID %>").multiselect('rebuild');


                    }
        }

        function BindState() {
            $('#<%=ListState.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: 'RetailerInvoiceReport.aspx/BindState',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                error: function (response) {
                    alert(response.d);
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ListState.ClientID %>"));
            }
            function PopulateControl(list, control) {
                console.log(JSON.parse(list))
                var data = JSON.parse(list);
                $('#<%=ListState.ClientID %>').empty();
                        $.each(data, function () {

                            $('#<%=ListState.ClientID %>').append('<option  value=' + this['AreaId'] + '>' + this['AreaName'] + '</option>');


               });
               var id = $('#<%=hidState.ClientID%>').val();
                        //alert(id);
                        var splittedArray = id.split(",");
                        //  alert(splittedArray);

                        if (id != "") {
                            //alert('c');
                            control.val(splittedArray);
                        }
                        $("#<%=ListState.ClientID %>").multiselect('rebuild');


            }
        }


        function Bindcity() {

            var selectedState = [];

            $("#<%=ListState.ClientID %> :selected").each(function () {
                selectedState.push($(this).val());
            });
            $("#<%=hidState.ClientID %>").val(selectedState);

                    var obj = { stateids: $("#<%=hidState.ClientID %>").val() };

                    $('#<%=ListCity.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                    $.ajax({
                        type: "POST",
                        url: 'RetailerInvoiceReport.aspx/BindCity',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(obj),
                        success: OnPopulated,
                        error: function (response) {
                            alert(response.d);
                        },
                        failure: function (response) {
                            alert(response.d);
                        }
                    });
                    function OnPopulated(response) {
                        PopulateControl(response.d, $("#<%=ListCity.ClientID %>"));
            }
            function PopulateControl(list, control) {
                console.log(JSON.parse(list))
                var data = JSON.parse(list);
                $('#<%=ListCity.ClientID %>').empty();
                $.each(data, function () {

                    $('#<%=ListCity.ClientID %>').append('<option  value=' + this['AreaId'] + '>' + this['AreaName'] + '</option>');


                        });

                        $("#<%=ListCity.ClientID %>").multiselect('rebuild');


            }
        }



        function Binddistributor() {

            var selectedState = [];

            $("#<%=ListCity.ClientID %> :selected").each(function () {
                        selectedState.push($(this).val());
                    });
                    $("#<%=Hidcity.ClientID %>").val(selectedState);

                            var obj = { cityids: $("#<%=Hidcity.ClientID %>").val() };

                            $('#<%=ListDistributor.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                            $.ajax({
                                type: "POST",
                                url: 'RetailerInvoiceReport.aspx/Binddistributor',
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: JSON.stringify(obj),
                                success: OnPopulated,
                                error: function (response) {
                                    alert(response.d);
                                },
                                failure: function (response) {
                                    alert(response.d);
                                }
                            });
                            function OnPopulated(response) {
                                PopulateControl(response.d, $("#<%=ListDistributor.ClientID %>"));
            }
            function PopulateControl(list, control) {
                console.log(JSON.parse(list))
                var data = JSON.parse(list);
                $('#<%=ListDistributor.ClientID %>').empty();
                       $.each(data, function () {

                           $('#<%=ListDistributor.ClientID %>').append('<option  value=' + this['ID'] + '>' + this['PartyName'] + '</option>');


                });

                $("#<%=ListDistributor.ClientID %>").multiselect('rebuild');


                   }
               }
    </script>
   <%-- <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();
        });
    </script>--%>
    <%-- <script type="text/javascript">
         $(function () {
             $("#invtable").DataTable();
         });
    </script>--%>
    <script type="text/javascript">
        $(function () {
            $("[id*=trview] input[type=checkbox]").bind("click", function () {
                var table = $(this).closest("table");
                if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                    //Is Parent CheckBox
                    var childDiv = table.next();
                    var isChecked = $(this).is(":checked");
                    $("input[type=checkbox]", childDiv).each(function () {
                        if (isChecked) {
                            $(this).prop("checked", "checked");
                        } else {
                            $(this).removeAttr("checked");
                        }
                    });
                } else {
                    //Is Child CheckBox
                    var parentDIV = $(this).closest("DIV");
                    if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                        $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                    } else {
                        $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                    }
                }
            });
        })

        function loding() {
            $('#spinner').show();
        }
    </script>
     <script type="text/javascript">

         //function postBackByObject() {
         //    var o = window.event.srcElement;
         //    if (o.tagName == "INPUT" && o.type == "checkbox") {
         //        __doPostBack("", "");
         //    }
         //}

         function fireCheckChanged(e) {
             var ListBox1 = document.getElementById('<%= trview.ClientID %>');
             var evnt = ((window.event) ? (event) : (e));
             var element = evnt.srcElement || evnt.target;

             if (element.tagName == "INPUT" && element.type == "checkbox") {
                 __doPostBack("", "");
             }
         }

   </script>
    <script type="text/javascript">
        function FillRetailer()
        {
            var selectedState = [];
            var selectedsmids = [];
            $("#<%=Listsalesperson.ClientID %> :selected").each(function () {
                selectedsmids.push($(this).val());
            });
            $("#<%=Hidden2.ClientID %>").val(selectedsmids);

            $("#<%=ListDistributor.ClientID %> :selected").each(function () {
                 selectedState.push($(this).val());
             });
             $("#<%=Hidden1.ClientID %>").val(selectedState);

            var obj = { distids: $("#<%=Hidden1.ClientID %>").val(), smids: $("#<%=Hidden2.ClientID %>").val() };

            //$('#<%=ListBox1.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: 'RetailerInvoiceReport.aspx/BindRetailer',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(obj),
                success: OnPopulated,
                error: function (response) {
                    alert(response.d);
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ListDistributor.ClientID %>"));
                            }
                            function PopulateControl(list, control) {
                                console.log(JSON.parse(list))
                                var data = JSON.parse(list);
                                $('#<%=ListBox1.ClientID %>').empty();
                $.each(data, function () {

                    $('#<%=ListBox1.ClientID %>').append('<option  value=' + this['ID'] + '>' + this['PartyName'] + '</option>');


                       });

                       $("#<%=ListBox1.ClientID %>").multiselect('rebuild');


            }
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
        .multiselect-container > li {
            width: 270px;
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
            <img id="img-spinner" src="img/loader.gif" width="30%" height="50%" alt="Loading" /><br />
            <b>Loading Data....</b>
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
                                <%--<h3 class="box-title">Retailer Invoice Report</h3>--%>
                                <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">                                    
                                    <div class="col-md-4 col-sm-6 col-xs-12" hidden>
                                       
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                           
                                            <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All" ></asp:TreeView>
                                        </div>
                                    </div>    
                                    
                                    
                                       <div class="col-md-3 col-sm-6 col-xs-12">
                                       
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label> <br />                                   
                                            <asp:ListBox ID="Listsalesperson" runat="server" SelectionMode="Multiple" onchange="FillRetailer();"></asp:ListBox>
                                            <input type="hidden" id="Hidden2" runat="server" />
                                        </div>
                                    </div>    

                                    <div class="col-md-3 col-sm-6 col-xs-10">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">State:</label><br />
                                            <asp:ListBox ID="ListState" runat="server" SelectionMode="Multiple" onchange="Bindcity();"></asp:ListBox>
                                            <input type="hidden" id="hidState" runat="server" />
                                        </div>
                                    </div>

                                    <div class="col-md-3 col-sm-6 col-xs-10">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">City:</label><br />
                                            <asp:ListBox ID="ListCity" runat="server" SelectionMode="Multiple" onchange="Binddistributor();"></asp:ListBox>
                                            <input type="hidden" id="Hidcity" runat="server" />
                                        </div>
                                    </div>
                                     </div>
                                     <div class="row">
                                    <div class="col-md-3 col-sm-6 col-xs-10">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Distributor Name:</label><br />
                                            <asp:ListBox ID="ListDistributor" runat="server" SelectionMode="Multiple" onchange="FillRetailer();"></asp:ListBox>
                                            <input type="hidden" id="Hidden1" runat="server" />
                                        </div>
                                    </div>





                                                     
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Retailer Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                           
                                            <div class="">
                                                <asp:ListBox ID="ListBox1" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                                 <input type="hidden" id="hiddistributor" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                         </div>
                               
                                <div class="row">                                 
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                        </div>
                                    </div>  
                                                            
                                       <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <div class="box-footer">
                                 <input type="button" runat="server" onclick="btnSubmitfunc();" class="btn btn-primary" value="Go" />  
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClientClick="loding()"  
                                    OnClick="btnGo_Click" Visible="false" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                    OnClick="btnCancel_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                    OnClick="btnExport_Click" OnClientClick="javascript:return validate();" />
                            </div>                        
                             <div id="rptmain" runat="server">
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="distreportrpt" runat="server">
                                   <HeaderTemplate>
                                        <table id="invtable" class="table table-bordered table-striped rpttable">
                                            <thead>
                                                <tr>
                                                    <th style="text-align: left; width: 15%; text-wrap: normal;">Invoice No</th>
                                                    <th style="text-align: left; width: 18%">Invoice Dt</th>
                                                    <th style="text-align: left; width: 12%">Retailer Id</th>
                                                    <th style="text-align: left; width: 18%">Retailer Name</th>
                                                    <th style="text-align: left; width: 18%">Branch Name</th>
                                                    <th style="text-align: right; width: 28%">Amount</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                       <%--  <tr>  
                                                                                
                                              <td>
                                             <asp:HyperLink runat="server" target="_blank" Font-Underline="true"
                                            NavigateUrl='<%# String.Format("InvoiceSapDetails.aspx?DistInvDocId={0}", Eval("DistInvDocId")) %>' Text='<%#Eval("DistInvDocId") %>'/></td>
                                          
                                        <asp:Label ID="DistInvDocIdLabel" runat="server" Visible="false" Text='<%# Eval("DistInvDocId")%>'></asp:Label>
                                          
                                            <td style="text-align: left; width: 18%"><%#Convert.ToDateTime(Eval("VDate")).ToString("dd/MMM/yyyy") %></td>
                                         
                                        <asp:Label ID="VDateLabel" runat="server" Visible="false" Text='<%# Eval("VDate")%>'></asp:Label>
                                         
                                            <td style="text-align: left; width: 18%"><%#Eval("PartyId") %></td>
                                           
                                        <asp:Label ID="PartyIdLabel" runat="server" Visible="false" Text='<%# Eval("PartyId")%>'></asp:Label>
                                          
                                            <td style="text-align: left; width: 18%"><%#Eval("PartyName") %></td>
                                         
                                        <asp:Label ID="PartyNameLabel" runat="server" Visible="false" Text='<%# Eval("PartyName")%>'></asp:Label>
                                         
                                            <td style="text-align: left; width: 18%"><%#Eval("BranchName") %></td>
                                         
                                        <asp:Label ID="BranchNameLabel" runat="server" Visible="false" Text='<%# Eval("BranchName")%>'></asp:Label>
                                         
                                            <td style="text-align: right; width: 28%"><%#Eval("Amount") %></td>
                                        
                                        <asp:Label ID="AmountLabel" runat="server" Visible="false" Text='<%# Eval("Amount")%>'></asp:Label>
                                         
                                        </tr>--%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>       
                                 </div>                     
                            <br />
                            <div id="detailDiv" runat="server" style="display: none;">
                                <div class="box-body table-responsive">
                                     <asp:Repeater ID="detailDistrpt" runat="server">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <%--<th>SNo.</th>--%>
                                                        <th>Item No.</th>
                                                        <th>Item</th>
                                                        <th>Qty</th>
                                                        <th>Amount</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                               <%-- <td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>--%>
                                                <td><%#Eval("Item_Id") %></td>
                                                <td><%#Eval("Item") %></td>
                                                <td><%#Eval("Qty") %></td>
                                                <td style="text-align: right;"><%#Eval("Amount") %></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>     </table>       
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    
                                </div>
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
     <script type="text/javascript">

         function btnSubmitfunc() {

          <%--   var selectedvalue = [];
             $("#<%=trview.ClientID %> :checked").each(function () {
                 selectedvalue.push($(this).val());
             });
             if (selectedvalue == "") {
                 errormessage("Please Select Sales Person");
                 return false;
             }--%>
             var selectedValues = [];
             $("#<%=ListBox1.ClientID %> :selected").each(function () {
                selectedValues.push($(this).val());
            });
             $("#<%=hiddistributor.ClientID%>").val(selectedValues);
            if (selectedValues == "") {
                errormessage("Please Select Retailer");
                return false;
            }
            var Todate = new Date($('#<%=txttodate.ClientID%>').val())
            var Fromdate = new Date($('#<%=txtfmDate.ClientID%>').val())
             if (Fromdate > Todate) {
                 errormessage("From Date Should be less than To date");
                 return false;
             }
            loding();
            BindGridView();
        }
         function validate()
         {
             var selectedvalue = [];
         <%--    $("#<%=trview.ClientID %> :checked").each(function () {
                   selectedvalue.push($(this).val());
               });
               if (selectedvalue == "") {
                   errormessage("Please Select Sales Person");
                   return false;
               }--%>
               var selectedValues = [];
               $("#<%=ListBox1.ClientID %> :selected").each(function () {
                 selectedValues.push($(this).val());
             });
             $("#<%=hiddistributor.ClientID%>").val(selectedValues);
             if (selectedValues == "") {
                 errormessage("Please Select Retailer");
                 return false;
             }
             var Todate = new Date($('#<%=txttodate.ClientID%>').val())
            var Fromdate = new Date($('#<%=txtfmDate.ClientID%>').val())
             if (Fromdate > Todate) {
                 errormessage("From Date Should be less than To date");
                 return false;
             }
         }
        function BindGridView() {
            $.ajax({
                type: "POST",
                url: "RetailerInvoiceReport.aspx/GetDistributorInvice",
                data: '{Distid: "' + $("#<%=hiddistributor.ClientID%>").val() + '", Fromdate: "' + $('#<%=txtfmDate.ClientID%>').val() + '",Todate: "' + $('#<%=txttodate.ClientID%>').val() + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    //alert(response.d);
                },
                error: function (response) {
                    //alert(response.d);
                }
            });
        };



        function OnSuccess(response) {
            //  alert(JSON.stringify(response.d));
            //alert(response.d);
            $('div[id$="rptmain"]').show();
            var data = JSON.parse(response.d);
            //alert(data);
            var arr1 = data.length;
            //alert(arr1);
            //alert("007");
            var tablerpt = $('#invtable').DataTable();
            //alert("-12");

            tablerpt.destroy();

            //  table.empty();

            $("#ContentPlaceHolder1_rptmain table ").DataTable({
                "order": [[1, "desc"]],

                "aaData": data,
                "aoColumns": [
            {
                "mData": "RetInvDocId",
                "render": function (data, type, row, meta) {

                    if (type === 'display') {
                        return $('<a target="_blank">')
                           .attr('href', "InvoiceSapDetailsForRetailer.aspx?RetInvDocId=" + data)
                           .text(data)
                           .wrap('<div></div>')
                           .parent()
                           .html();

                    } else {
                        return data;
                    }
                }


            }, // <-- which values to use inside object
            {
                "mData": "VDate",
                "render": function (data, type, row, meta) {

                    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                    var date = new Date(data);
                    var day = date.getDate();
                    var month = date.getMonth();
                    var year = date.getFullYear();

                    var mname = monthNames[date.getMonth()]

                    var fdate = day + '/' + mname + '/' + year;

                    if (type === 'display') {
                        return $('<div>')
                           .attr('class', 'text')
                           .text(fdate)
                           .wrap('<div></div>')
                           .parent()
                           .html();

                    } else {
                        return data;
                    }
                }
            },
            { "mData": "PartyId" },
            { "mData": "PartyName" },
            { "mData": "BranchName" },
            {
                "mData": "Amount",
                "render": function (data, type, row, meta) {
                    if (type === 'display') {
                        return $('<div>')
                           .attr('class', 'text-right')
                           .text(parseFloat(data).toFixed(2))
                           .wrap('<div></div>')
                           .parent()
                           .html();

                    } else {
                        return data;
                    }
                }
            }
                ]
            });

            $('#spinner').hide();
        }
    </script>
</asp:Content>
