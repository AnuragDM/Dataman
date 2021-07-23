<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="DistLedgerReport.aspx.cs" Inherits="AstralFFMS.DistLedgerReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=ListBox1]').multiselect({
                enableCaseInsensitiveFiltering: true,
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
        $(function () {
            $('[id*=salespersonListBox]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
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
        //$(function () {
        //    $("#example1").DataTable();
        //});
    </script>
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

         function loding() {
             $('#spinner').show();
         }
   </script>
    <script type="text/javascript">

        function btnSubmitfunc() {
            debugger;
            var me = getUrlVars()["DistId"];
            if (me == "") {
                var selectedvalue = [];
                $("#<%=trview.ClientID %> :checked").each(function () {
                    selectedvalue.push($(this).val());
                });
                if (selectedvalue == "") {
                    errormessage("Please Select Sales Person");
                    return false;
                }
            }
            // $("#hiddistributor").val(selectedvalue);
            var selectedValues = [];
            $("#<%=ListBox1.ClientID %> :selected").each(function () {
              selectedValues.push($(this).val());
          });
          $("#hiddistributor").val(selectedValues);
            // validate($("#hiddistributor").val());
          if (selectedValues == "") {
              errormessage("Please Select Distributor");
              return false;
          }

<%--          var productGroupValues = [];
          $("#<%=matGrpListBox.ClientID %> :selected").each(function () {
                productGroupValues.push($(this).val());
            });
            $("#hidproductgroup").val(productGroupValues);

            var productValues = [];
            $("#<%=productListBox.ClientID %> :selected").each(function () {
              productValues.push($(this).val());
          });
          $("#hidproduct").val(productValues);--%>

          loding();
          BindGridView();
      }

      function BindGridView() {
          $.ajax({
              type: "POST",
              url: "DistLedgerReport.aspx/getdistributorledger",
              data: '{Distid: "' + $("#hiddistributor").val() + '",Fromdate: "' + $('#<%=txtfmDate.ClientID%>').val() + '",Todate: "' + $('#<%=txttodate.ClientID%>').val() + '"}',
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
            // alert(response.d);
            //$('div[id$="rptmain1"]').show();
            var data = JSON.parse(response.d);
           // alert(data);
            var arr1 = data.length;
            //alert(arr1);
            var table = $('#ContentPlaceHolder1_rptmain1 table').DataTable();
            table.destroy();
            $("#ContentPlaceHolder1_rptmain1 table ").DataTable({
                "order": [[0, "asc"]],

                "aaData": data,
                "aoColumns": [                          
            { "mData": "Distributor"},
            {
                "mData": "Debit",
                "render": function (data, type, row, meta) {
                    if (type === 'display') {
                        return $('<div>')
                           .attr('class', 'text-right')
                           .text(data)
                           .wrap('<div></div>')
                           .parent()
                           .html();

                    } else {
                        return data;
                    }
                }
            },
            {
                "mData": "Credit",
                "render": function (data, type, row, meta) {
                    if (type === 'display') {
                        return $('<div>')
                           .attr('class', 'text-right')
                           .text(data)
                           .wrap('<div></div>')
                           .parent()
                           .html();

                    } else {
                        return data;
                    }
                }
            },
            {
                "mData": "Closing",
                "render": function (data, type, row, meta) {
                    if (data == 0)
                    {
                        posNum=''
                    }
                   else if (data < 0) {
                        var posNum = (data < 0) ? data * -1 : data;
                        posNum = posNum + ' Cr'
                    } else {
                        posNum = data + ' Dr'

                    }


                if (type === 'display') {
                    return $('<div>')
                       .attr('class', 'text-right')
                       .text(posNum)
                       .wrap('<div></div>')
                       .parent()
                       .html();

                } else {
                    return data;
                }
            }
    },
              {
                  "mData": "DistId",
                  "render": function (data, type, row, meta) {
                      <%--var link =  "DistLedgerReport.aspx?DistId=" + data + "&Fromdate=" + $('#<%=txtfmDate.ClientID%>').val() + "&Todate=" + $('#<%=txttodate.ClientID%>').val() ;--%>
                      var link = "DistLedgerReport.aspx?DistId=" + data ;
                      if (type === 'display') {
                          return $('<a>')
                             .attr('href', link)
                             .text('View Ledger')
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
            //alert(arr1);

            $('#spinner').hide();
        }

        function ValidateCheckBoxList() {
            var checkBoxList = document.getElementById("<%=trview.ClientID %>");
            var checkboxes = checkBoxList.getElementsByTagName("input");
            var isValid = false;
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    isValid = true;
                    break;
                }
                alert(isValid + "2");
            }
            args.IsValid = isValid;
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
                                <%--<h3 class="box-title">Distributor Ledger Report</h3>--%>
                                <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">                                   
                                    <div class="col-md-4 col-sm-6 col-xs-12">
                                        <div class="form-group" hidden style="display:none;">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br /> 
                                            <asp:ListBox ID="salespersonListBox" runat="server" SelectionMode="Multiple"
                                                OnSelectedIndexChanged="salespersonListBox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br /> 
                                            <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged"></asp:TreeView>
                                        </div>
                                    </div>
                                     
                                        
                                   
                                    
                                    <div class="col-md-3 col-sm-6 col-xs-10">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Distributor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />                                           
                                            <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                            <input type="hidden" id="hiddistributor" />
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
                                   <div class="col-md-1"></div>
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
                                <input type="button" runat="server" onclick="btnSubmitfunc();" class="btn btn-primary" value="Go" id="ledgergo" />  
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClientClick="loding()"  
                                    OnClick="btnGo_Click" Visible="false" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary"
                                    OnClick="btnCancel_Click" />
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                    OnClick="ExportCSV" />
                            </div>                            
                               <div id="rptmain1" runat="server">
                            <div  class="box-body table-responsive">
                                <asp:Repeater ID="distreportrpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="example11" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>                                                   
                                                  <%--  <th>SNo.</th>--%>                                                    
                                                    <th>Distributor Name</th>                                                   
                                                    <th style="text-align: right">Debit</th>
                                                    <th style="text-align: right">Credit</th>
                                                    <th style="text-align: right">Closing Balance</th>
                                                   <th style="text-align: right">View Ledger</th>                                                    
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                       <%-- <tr>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("DistId") %>' />
                                           <%-- <td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>--%>
                                            <%--<td><%#Eval("Distributor") %></td>--%>
                                             <%-- Added 06-06-2016 - Nishu --%>
                                        <%--<asp:Label ID="DistributorLabel" runat="server" Visible="false" Text='<%# Eval("Distributor")%>'></asp:Label>--%>
                                            <%-- End --%>                                        
                                            <%--<td style="text-align: right;"><%#Eval("dr") %></td>--%>
                                             <%-- Added 06-06-2016 - Nishu --%>
                                        <%--<asp:Label ID="drLabel" runat="server" Visible="false" Text='<%# Eval("dr")%>'></asp:Label>--%>
                                            <%-- End --%>
                                            <%--<td style="text-align: right;"><%#Convert.ToDecimal(Eval("Cr")) > 0 ? Eval("Cr") : (-1)*Convert.ToDecimal(Eval("Cr"))  %></td>--%>   
                                             <%-- Added 06-06-2016 - Nishu --%>
                                        <%--<asp:Label ID="CrLabel" runat="server" Visible="false" Text='<%#Convert.ToDecimal(Eval("Cr")) > 0 ? Eval("Cr") : (-1)*Convert.ToDecimal(Eval("Cr"))  %>'></asp:Label>--%>
                                            <%-- End --%>      
                                            <%--<td style="text-align: right;"><%#Convert.ToDecimal(Eval("cBalance")) > 0 ? Eval("cBalance")+ " Dr"  :(-1)* Convert.ToDecimal(Eval("cBalance")) + " Cr" %></td>--%>
                                             <%-- Added 06-06-2016 - Nishu --%>
                                        <%--<asp:Label ID="cBalanceLabel" runat="server" Visible="false" Text='<%#Convert.ToDecimal(Eval("cBalance")) > 0 ? Eval("cBalance")+ " Dr"  :(-1)* Convert.ToDecimal(Eval("cBalance")) + " Cr" %>'></asp:Label>--%>
                                            <%-- End --%>
                                            <%--<td><asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">View Ledger</asp:LinkButton></td>--%>
                                        <%--</tr>--%>
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
                                    <asp:Label ID ="lblDist"  runat="server"></asp:Label>
                                    <asp:Repeater ID="detailDistrpt" runat="server">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <th>SNo.</th>
                                                        <th>Date</th>
                                                        <th>VNo.</th>
                                                        <th>Narration</th>
                                                        <th style="text-align:right">Debit</th>
                                                        <th style="text-align:right">Credit</th>
                                                        <th style="text-align:right">Balance</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>
                                                <td><%#Convert.ToDateTime(Eval("vdate")).ToString("dd/MMM/yyyy") %></td>
                                                <td><%#Eval("DLDocId") %></td>
                                                <td><%#Eval("Narration") %></td>
                                                <td style="text-align:right;"><%#Eval("amtDr") %> </td>
                                                <td style="text-align:right;"><%#Convert.ToDecimal(Eval("amtCr")) > 0 ? Eval("amtCr") : (-1)*Convert.ToDecimal(Eval("amtCr")) %></td>
                                                <td style="text-align: right;"><%#Eval("Balance") %></td>
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
