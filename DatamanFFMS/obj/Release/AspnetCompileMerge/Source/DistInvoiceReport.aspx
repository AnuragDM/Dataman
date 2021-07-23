<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DistInvoiceReport.aspx.cs" Inherits="AstralFFMS.DistInvoiceReport" %>

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
    <style type="text/css">
        .containerStaff {
            border: 1px solid #ccc;
            overflow-y: auto;
            min-height: 200px;
            width: 134%;
            overflow-x: auto;
        }
        .multiselect-container > li {
            width: 212px;
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
                                <h3 class="box-title">Distributor Invoice Report</h3>
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                                <div class="row">                                    
                                    <div class="col-md-4 col-sm-6 col-xs-12">
                                        <div class="form-group" hidden>
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                           
                                            <asp:ListBox ID="salespersonListBox" runat="server" SelectionMode="Multiple"
                                                OnSelectedIndexChanged="salespersonListBox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                           
                                            <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged"></asp:TreeView>
                                        </div>
                                    </div>                                   
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Distributor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>                                           
                                            <div class="">
                                                <asp:ListBox ID="ListBox1" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                                 <input type="hidden" id="hiddistributor" />
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
                                    <div class="col-md-1" ></div>                              
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
                                    OnClick="btnExport_Click"/>
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
                                                    <th style="text-align: left; width: 12%">Distributor Id</th>
                                                    <th style="text-align: left; width: 18%">Distributor Name</th>
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

             var selectedvalue = [];
             $("#<%=trview.ClientID %> :checked").each(function () {
                selectedvalue.push($(this).val());
            });
            if (selectedvalue == "") {
                errormessage("Please Select Sales Person");
                return false;
            }
            var selectedValues = [];
            $("#<%=ListBox1.ClientID %> :selected").each(function () {
                selectedValues.push($(this).val());
            });
            $("#hiddistributor").val(selectedValues);
            if (selectedValues == "") {
                errormessage("Please Select Distributor");
                return false;
            }

            loding();
            BindGridView();
        }

        function BindGridView() {
            $.ajax({
                type: "POST",
                url: "DistInvoiceReport.aspx/GetDistributorInvice",
                data: '{Distid: "' + $("#hiddistributor").val() + '", Fromdate: "' + $('#<%=txtfmDate.ClientID%>').val() + '",Todate: "' + $('#<%=txttodate.ClientID%>').val() + '"}',
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
              "mData": "DistInvDocId",
              "render": function (data, type, row, meta) {

                  if (type === 'display') {
                      return $('<a target="_blank">')
                         .attr('href', "InvoiceSapDetails.aspx?DistInvDocId=" + data)
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
                         .text(data)
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
