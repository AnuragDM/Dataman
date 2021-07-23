<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DistributerDispatchOrderForm.aspx.cs" Inherits="AstralFFMS.DistributerDispatchOrderForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
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
            $('[id*=matGrpListBox]').multiselect({
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
            $('[id*=productListBox]').multiselect({
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
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");
        }
    </script>
    <script type="text/javascript">
        $(function () {
            //$("#itemsaleTable").DataTable({
            //    "order": [[0, "desc"]]
            //});   

            $('#modalClose').on('click', function () {
                $('#ContentPlaceHolder1_TextArea1').val("");
                $('#ContentPlaceHolder1_myModal').hide();
            })

            $('#modalClose1').on('click', function () {
  
                $('#ContentPlaceHolder1_TextArea1').val("");
                $('#ContentPlaceHolder1_myModal').hide();
            })

            $('#cmodalClose').on('click', function () {
                $('#ContentPlaceHolder1_TextArea2').val("");
                $('#ContentPlaceHolder1_myModal1').hide();
            })

            $('#cmodalClose1').on('click', function () {

                $('#ContentPlaceHolder1_TextArea2').val("");
                $('#ContentPlaceHolder1_myModal1').hide();
            })
          

        });

        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            //alert(iKeyCode);
            if (iKeyCode != 9 && iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57)) {

                // if (iKeyCode != 8 && iKeyCode != 0 && (iKeyCode < 48 || iKeyCode > 57))

                return false;
            }
            return true;
        }

        function checkVal(a) {
            //console.log(a);
            var dp = $('#' + a).val();
            //alert(dp);
            if (dp == "") {
                //alert("hi");

                $('#' + a).val("0.00");
            }

            var value = parseFloat(dp).toFixed(2);
            $('#' + a).val(value);

        }

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
        $(function () {
            $("[id*=trview] input[type=checkbox]").bind("click", function () {
                var table = $(this).closest("table");
                if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                    //Is Parent CheckBox
                    var childDiv = table.next();
                    var isChecked = $(this).is(":checked");
                    //alert(isChecked);
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
        //$(function () {
        //    $(".select2").select2();
        //});

        // Get the modal
       
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
         function validate(persons) {
             if (document.getElementById("<%=ListBox1.ClientID%>").value == "" || document.getElementById("<%=ListBox1.ClientID%>").value == "0") {
                 errormessage("Please Select Distributor");
                 document.getElementById("<%=ListBox1.ClientID%>").focus();
                       return false;
                   }


               }

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            //Hide the div          
            //document.getElementById("ContentPlaceHolder1_pnlpopup").style.display = "none";
            //$('div[id$="rptmain"]').hide();
            //conversely do the following to show it again if needed later
            //$('#showdiv').show();

            //var table = $('#ContentPlaceHolder1_rptmain table').DataTable();
            //$('#itemsaleTable tbody').on('click', 'button', function () {
            //    var data = table.row($(this).parents('tr')).data();
            //    console.log(data);
            //});
        });

</script>
    <style type="text/css">
        .input-group .form-control {
            height: 34px;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            /*display: table;*/
        }

        .multiselect-container > li > a {
            white-space: normal;
        }

        .multiselect-container > li {
            width: 100%;
        }

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }

         .modalBackground {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }

        .modalPopup {
            background-color: #FFFFFF;
            width: 300px;
            border: 1px solid #aaaaaa !important;
            border-radius: 12px;
            padding: 0;
            overflow-y: hidden !important;
        }

            .modalPopup .header {
                border: 1px solid #aaaaaa;
                background: #cccccc url(img/ui-bg_highlight-soft_75_cccccc_1x100.png) 50% 50% repeat-x;
                color: black !important;
                font-weight: bold;
                height: 35px;
                color: White;
                line-height: 30px;
                text-align: center;
                margin-right: 10px;
                border-top-left-radius: 6px;
                border-top-right-radius: 6px;
            }

            .modalPopup .body {
                min-height: 50px;
                line-height: 30px;
                text-align: center;
            }

            .modalPopup .footer {
                margin-right: 10px;
            }

            .modalPopup .modelbtn {
                color: White;
                line-height: 23px;
                text-align: center;
                font-weight: bold;
                cursor: pointer;
                border-radius: 4px;
                border: 1px solid #d3d3d3 !important;
                background: #e6e6e6 url(img/ui-bg_glass_75_e6e6e6_1x400.png) 50% 50% repeat-x;
                font-weight: normal;
                color: #555555;
            }

        .modalPopup {
            border-radius: 11px;
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: #3c8dbc;
            padding-top: 10px;
            padding-left: 10px;
            width: 40%;
            height: 380px;
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

      
}
    </style>
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
            // $("#hiddistributor").val(selectedvalue);
            var selectedValues = [];
            $("#<%=ListBox1.ClientID %> :selected").each(function () {
              selectedValues.push($(this).val());
          });
          $("#hiddistributor").val(selectedValues);
            // validate($("#hiddistributor").val());

            //not mandatory
          //if (selectedValues == "") {
          //    errormessage("Please Select Distributor");
          //    return false;
          //}

          var productGroupValues = [];
          $("#<%=matGrpListBox.ClientID %> :selected").each(function () {
                productGroupValues.push($(this).val());
            });
            $("#hidproductgroup").val(productGroupValues);

            var productValues = [];
            $("#<%=productListBox.ClientID %> :selected").each(function () {
              productValues.push($(this).val());
          });
          $("#hidproduct").val(productValues);

          loding();
          BindGridView();
      }

      function BindGridView() {
          $.ajax({
              type: "POST",
              url: "DistributerDispatchOrderForm.aspx/GetDistributorItemSale",
              data: '{Distid: "' + $("#hiddistributor").val() + '", ProductGroup: "' + $("#hidproductgroup").val() + '" , Product: "' + $("#hidproduct").val() + '", Fromdate: "' + $('#<%=txtfmDate.ClientID%>').val() + '",Todate: "' + $('#<%=txttodate.ClientID%>').val() + '"}',
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
            //var arr1 = data.length;
            //alert(arr1);
            var table = $('#ContentPlaceHolder1_rptmain table').DataTable();
            table.destroy();
            $("#ContentPlaceHolder1_rptmain table ").DataTable({
                "order": [[0, "desc"]],

                "aaData": data,
                "aoColumns": [
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



            }, // <-- which values to use inside object
            { "mData": "Syncid" },
            { "mData": "Distributor" },
           
            { "mData": "DocId" },
            {
                "mData": "TotalQty",
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
              
                "render": function (data, type, row,meta) {
                    //  console.log(row);
                    var did = row.DocId;
                    return '<button type="button" id=' + row.DocId.replace(/\s+/g, '-') + ' class="btn btn-primary" onclick="getDispatchDetails(this.id)">Dispatch</button> &nbsp;&nbsp;<button type="button" id=' + row.DocId.replace(/\s+/g, '-') + ' class="btn btn-primary" onclick="getCancelDetails(this.id)">Order Cancel</button>';
                }
            }
                ]
            });

            $('#spinner').hide();
        }

        function getDispatchDetails(a)
        {
            console.log(a);

           
           // document.getElementById("ContentPlaceHolder1_pnlpopup").style.display = "block";
            //alert("D");
           // document.getElementById("divpanelpopup").style.display = "block";
            //document.getElementById("ContentPlaceHolder1_pnlpopup").style.height = "550px";

           
        }

        function getCancelDetails(a) {
            console.log(a);
            //alert("C");
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
    <script type="text/javascript">
        $("[id$=btnExport]").click(function (e) {
           // alert("123");
            window.open('data:application/vnd.ms-excel,' + encodeURIComponent($('div[id$=rptmain]').html()));
            e.preventDefault();
        });
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
        <div class="box-body">
            <!-- left column -->
            <!-- general form elements -->
            <div class="box box-primary">
                <div class="row">
                    <!-- left column -->
                    <div class="col-md-12">
                        <div id="InputWork">
                            <!-- general form elements -->

                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Distributor Dispatch Order</h3>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="row">
                                         
                                       <%-- <div class="col-md-3 col-sm-6 col-xs-7" hidden>
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>  
                                                <asp:ListBox ID="salespersonListBox" runat="server" SelectionMode="Multiple"
                                                    OnSelectedIndexChanged="salespersonListBox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                            </div>
                                        </div>        --%>
                                         <div class="col-md-4 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>  
                                                <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All" OnTreeNodeCheckChanged="trview_TreeNodeCheckChanged" ></asp:TreeView>
                                             
                                            </div>
                                        </div>                                     
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Distributor name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;display:none;">*</label>                                                
                                                <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                                 <input type="hidden" id="hiddistributor" />
                                                <input type="hidden" id="hidproductgroup" />
                                                <input type="hidden" id="hidproduct" />
                                            </div>
                                           
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="row">
                                        <%-- <div class="col-md-8 col-sm-11">--%>
                                        
                                        
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1" style="display:none">Product Group:</label>                                                
                                                <asp:ListBox ID="matGrpListBox" runat="server" SelectionMode="Multiple"
                                                    OnSelectedIndexChanged="matGrpListBox_SelectedIndexChanged" AutoPostBack="true" Visible="false"></asp:ListBox>
                                            </div>
                                        </div>
                                        <div class="col-md-1"></div>
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1" style="display:none">Product:</label>                                               
                                                <asp:ListBox ID="productListBox" runat="server" SelectionMode="Multiple" Visible="false"></asp:ListBox>
                                            </div>
                                        </div>
                                        <%-- </div>--%>
                                    </div>
                                    <div class="clearfix"></div>
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
                                                <label for="exampleInputEmail1">To:</label>
                                                <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                            </div>
                                        </div>

                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                                <div class="box-footer">
                                    <%-- <input type="button" runat="server" onclick="btnSubmitfunc();" class="btn btn-primary" value="Go" />  --%>
                                    <asp:Button type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClientClick="loding()"  
                                         OnClick="btnGo_Click" Visible="true" />
                                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Cancel_Click" />
                                   <%-- <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                        OnClick="btnExport_Click" />--%>
                                     <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                      OnClick="btnExport_Click"/>
                                    <%-- <input style="margin-right: 5px;" type="button" id="Go" value="Go" class="btn btn-primary" onc onclick="GetReport();" />--%>
                                </div>
                                <br />                       
                            </div>
                        </div>
                        <div id="rptmain" runat="server">
                            <div class="box-header table-responsive">
                                <asp:Repeater ID="distitemsalerpt" runat="server" OnItemCommand="distitemsalerpt_ItemCommand">
                                    <HeaderTemplate>
                                        <table id="itemsaleTable" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>                                                  
                                                    <th style="text-align: left; width: 10%">Date</th>
                                                    <th style="text-align: left; width: 20%">Distributor Sync Id</th>
                                                    <th style="text-align: left; width: 20%">Distributor Name</th>
                                                    <th style="text-align: left; width: 17%">DocId</th>
                                                    <th style="text-align: right; width: 8%">Total Qty</th>
                                                    <th style="text-align: right; width: 20%">

                                                        <%--<input type="button" runat="server" class="btn btn-primary" value="Dispatch" />
                                                        <input type="button" runat="server" class="btn btn-primary" value="Cancel Order" />--%>
                                                    </th>

                                                    <%--<th style="text-align: right; width: 10%">Amount</th>--%>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>

                                        <tr>
   
                                        <asp:HiddenField ID="hdnDate" runat="server" Value='<%#Eval("VDate","{0:dd/MMM/yyyy}")%> ' />
                                            <asp:HiddenField ID="hdnDistributorName" runat="server" Value='<%#Eval("Distributor")%>' />
                                        <asp:HiddenField ID="hdnDocId" runat="server" Value='<%#Eval("DocId")%>' />
                                        <td><%#Eval("VDate","{0:dd/MMM/yyyy}")%></td>
                                         <td><%# Eval("SyncId")%></td>
                                            <td><%# Eval("Distributor")%></td>
                                            <td><%# Eval("DocId")%></td>
                                            <td style="text-align: right;"><%# Eval("TotalQty")%></td>
                                            <td><asp:LinkButton CommandName="selectDate" ID="LinkButton1"
                                                    CausesValidation="False" runat="server" OnClientClick="window.document.forms[0].target='_self'; setTimeout(function(){window.document.forms[0].target='';}, 500);" 
                                                    Text="Dispatch" 
                                                    Width="80px" Font-Underline="True"/> &nbsp;&nbsp; <asp:LinkButton CommandName="selectDate1" ID="LinkButton2"
                                                    CausesValidation="False" runat="server" OnClientClick="window.document.forms[0].target='_self'; setTimeout(function(){window.document.forms[0].target='';}, 500);" 
                                                    Text="Order Cancel" 
                                                    Width="80px" Font-Underline="True" /></td>
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

        <div>

             <!-- The Modal -->
                        <div class="modal" id="myModal" runat="server">
                          <div class="modal-dialog">
                            <div class="modal-content" style="height:550px !important;overflow-y:auto !important;">

                              <!-- Modal Header -->
                              <div class="modal-header">
                                <h4 class="modal-title">Distributor Dispatch Order Details</h4>
                                <button ID="modalClose1" type="button" class="close" style="margin-top:-28px !important;">&times;</button>
                              </div>

                              <!-- Modal body -->
                              <div class="modal-body">
                                <div class="box-body">
                           
                            <div class="row">
                               
                                        <div class="box-body table-responsive">
                                <table>
                                    <tr>

                                        <th>Distributor Name: &nbsp;&nbsp;</th>
                                        <td><asp:Label ID="distname" runat="server" Text="" ></asp:Label> &nbsp;&nbsp;  ( <asp:Label ID="vdate" runat="server" Text="" ></asp:Label>)  &nbsp;&nbsp; </td>
                                    </tr>
                                </table>
                                            </div>
                                </div>
                             <div class="row">
                                 <div id="Div2" runat="server" style="display:none;">
                                   <%-- <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">--%>
                                        <div class="box-body table-responsive">
                                            <asp:Repeater ID="rpt" runat="server" >
                                                <HeaderTemplate>
                                                    <table id="example1" class="table table-bordered table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>Item Name</th>
                                                                <th>Order Qty</th>
                                                                <th>Dispatch Qty</th>
                                                            </tr>
                                                        </thead>
                                                         <tbody>
                                                </HeaderTemplate>
                                                   
                                                <ItemTemplate>
                                                    <tr onclick="DoNav('<%#Eval("ItemId") %>');">
                                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("ItemId") %>' />
                                                         <asp:HiddenField ID="HiddenField2" runat="server" Value='<%#Eval("DocId") %>' />
                                                        <td><%#Eval("ItemName") %></td>
                                                        <td><%#Eval("OrderQty") %></td>
                                                        <td>
                                                           
                                                            <input type="text" runat="server" id="dispatchQty" onfocus="this.select()" onchange="checkVal(this.id)" onkeypress="return isNumber(event)" MaxLength="12" Class="form-control numeric text-right" Value=<%#Eval("OrderQty") %>/>
                                                            <%--<asp:TextBox ID="distPrice" onfocus= "this.select()" AutoPostBack="true" OnTextChanged="distPrice_TextChanged" onkeypress="return isNumber(event)" MaxLength="12" runat="server" CssClass="form-control numeric text-right" Text=<%#Eval("DistPrice") %>> </asp:TextBox>--%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>

                                                <FooterTemplate>
                                                        </tbody>     
                                                    </table>   
                                                 </FooterTemplate>    
                                           </asp:Repeater>
                                        </div>
                                       
                        
                                       <%-- <asp:HiddenField ID="HiddenField2" runat="server"  />       
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnEdit" runat="server" Text="Save" class="btn btn-primary" OnClick="btnEdit_Click" OnClientClick="return editBtn();"/>--%>
                                       
                                    <%--</div>--%>
                                </div>
                            </div>
                            <br />
                            <div class="row">

                                 <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                    <table>
                                               
                <tr>                                                  
                      <td><b>Remark:</b>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label></td>  
                       <td>
                           <textarea id="TextArea1" class="form-control" style="resize: none; height: 80px;width: 420px;" cols="20" rows="2" runat="server"
                           placeholder="Enter Remark"></textarea></td>        
                </tr> 


                                            </table>
                                </div>
                            </div>





                        </div>
                              </div>

                              <!-- Modal footer -->
                              <div class="modal-footer">
                                   <%--<asp:Button type="button" ID="Button3" runat="server" Text="Go" class="btn btn-primary" OnClientClick="loding()"  
                                         OnClick="btnGo_Click" Visible="true" />--%>
                                  <asp:Button type="button" ID="Button1" runat="server" Text="Save" class="btn btn-primary" OnClick="btnDispatchSave_Click" />
                                  
                                <button ID="modalClose" type="button" class="btn btn-danger">Close</button>
                              </div>

                            </div>
                          </div>
                        </div>
        </div>

        <div>

             <!-- The Modal -->
                        <div class="modal" id="myModal1" runat="server">
                          <div class="modal-dialog">
                            <div class="modal-content" style="height:320px !important;overflow-y:auto !important;">

                              <!-- Modal Header -->
                              <div class="modal-header">
                                <h4 class="modal-title">Distributor Dispatch Order Cancel Details</h4>
                                <button ID="cmodalClose1" type="button" class="close" style="margin-top:-28px !important;">&times;</button>
                              </div>

                              <!-- Modal body -->
                              <div class="modal-body">
                                <div class="box-body">
                           
                            <div class="row">
                               
                                        <div class="box-body table-responsive">
                                <table>
                                    <tr>

                                        <th>Distributor Name: &nbsp;&nbsp;<asp:HiddenField ID="HiddenField_ID" runat="server" /></th>
                                        <td><asp:Label ID="Label1" runat="server" Text="" ></asp:Label> &nbsp;&nbsp;  ( <asp:Label ID="Label2" runat="server" Text="" ></asp:Label>)  &nbsp;&nbsp; </td>
                                    </tr>
                                </table>
                                            </div>
                                </div>
                             
                            <br />
                            <div class="row">

                                 
                                    <table>
                                               
                <tr>                                                  
                      <td><b>Remark:</b>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label></td>  
                       <td>
                           <textarea id="TextArea2" class="form-control" style="resize: none; height: 80px;width: 420px;" cols="20" rows="2" runat="server"
                           placeholder="Enter Remark"></textarea></td>        
                </tr> 


                                            </table>
                               
                            </div>





                        </div>
                              </div>

                              <!-- Modal footer -->
                              <div class="modal-footer">
                                  <asp:Button type="button" ID="Button2" runat="server" Text="Save" class="btn btn-primary" OnClick="btnCancelOrderSave_Click"/>
                                <button ID="cmodalClose" type="button" class="btn btn-danger" >Close</button>
                              </div>

                            </div>
                          </div>
                        </div>
        </div>
        
      


    </section>
</asp:Content>


