<%@ Page Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DistributorSaleReport.aspx.cs" Inherits="AstralFFMS.DistributorSaleReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>

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

 </style>

    <script type="text/javascript">
        $(function () {
            $('[id*=ListBox1]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 300,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });

            $('[id*=ListArea]').multiselect({
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
            $('[id*=listboxmonth]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 100,
                //enableFiltering: true,
                //filterPlaceholder: 'Search'
            });
        });
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


        function checkDate(sender, args) {


            if ($('#<%=txttodate.ClientID%>').val() != "") {
                var date = new Date($('#<%=txttodate.ClientID%>').val())
                console.log(date)
                if (sender._selectedDate > date) {
                    errormessage("From Date Should be less than To date");
                    sender._selectedDate = new Date();
                    sender._textbox.set_Value(sender._selectedDate.format(sender._format))
                }
            }
        }
        function checkDate1(sender, args) {
            if ($('#<%=txtfmDate.ClientID%>').val() != "") {
                var date = new Date($('#<%=txtfmDate.ClientID%>').val())
                if (sender._selectedDate < date) {
                    errormessage("To Date Should be greater than To date");
                    sender._selectedDate = new Date();
                    sender._textbox.set_Value(sender._selectedDate.format(sender._format))
                }
            }
        }
    </script>
    <script type="text/javascript">


        function btnSubmitfunc1() {


            var selectedValues = [];
            $("#<%=ListBox1.ClientID %> :selected").each(function () {
                selectedValues.push($(this).val());
            });
            $("#<%=hiddistributor.ClientID%>").val(selectedValues);
            // validate($("#hiddistributor").val());
            if (selectedValues == "") {
                errormessage("Please Select Distributor");
                return false;
            }

        }
        function btnSubmitfunc() {


            var selectedValues = [];
            $("#<%=ListBox1.ClientID %> :selected").each(function () {
                selectedValues.push($(this).val());
            });
            $("#<%=hiddistributor.ClientID%>").val(selectedValues);
            // validate($("#hiddistributor").val());
            if (selectedValues == "") {
                errormessage("Please Select Distributor");
                return false;
            }


            loding();
            BindGridView();
        }
        function BindGridView() {
            $.getJSON('<%=ResolveUrl("~/and_sync.asmx/getdistributorsale")%>', { "DistId": $("#<%=hiddistributor.ClientID%>").val(), "Fromdate": $('#<%=txtfmDate.ClientID%>').val(), "Todate": $('#<%=txttodate.ClientID%>').val() }
                , function (result) {
                    console.log(result);


                    var data = result;
                    // alert(data);
                    var arr1 = data.length;
                    //alert(arr1);
                    var table = $('#ContentPlaceHolder1_rptmain1 table').DataTable();
                    table.destroy();
                    $("#ContentPlaceHolder1_rptmain1 table ").DataTable({
                        "order": [[0, "asc"]],

                        "aaData": data,
                        "aoColumns": [
                              { "mData": "Distributor" },
                                { "mData": "Date" },
                    { "mData": "Item" },


                 {
                     "mData": "PrevoiusStk",
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
                       "mData": "CurrentStk",
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

                   ,
                   {
                       "mData": "totalinvstk",
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
                        ,
                   {
                       "mData": "totalsalebysalesperson",
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
                        ,
                   {
                       "mData": "totalsalebydistributor",
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
                    //alert(arr1);

                    $('#spinner').hide();

                });

           <%-- $.ajax({
                type: "POST",
                url: "And_sync.asmx/getdiststock",
                data: '{DistId: "' + $("#<%=hiddistributor.ClientID%>").val() + '",Fromdate: "' + $('#<%=txtfmDate.ClientID%>').val() + '",Todate: "' + $('#<%=txttodate.ClientID%>').val() + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    //alert(response.d);
                },
                error: function (response) {
                    //alert(response.d);
                }
            });--%>
        };

        function loding() {
            $('#spinner').show();
        }



        //function OnSuccess(response) {
        //    //  alert(JSON.stringify(response.d));
        //    // alert(response.d);
        //    //$('div[id$="rptmain1"]').show();
        //    var data = JSON.parse(response.d);
        //    // alert(data);
        //    var arr1 = data.length;
        //    //alert(arr1);
        //    var table = $('#ContentPlaceHolder1_rptmain1 table').DataTable();
        //    table.destroy();
        //    $("#ContentPlaceHolder1_rptmain1 table ").DataTable({
        //        "order": [[0, "asc"]],

        //        "aaData": data,
        //        "aoColumns": [
        //    { "mData": "ItemName" },
        //       { "mData": "PartyName" },

        // {
        //     "mData": "Totalqty",
        //        "render": function (data, type, row, meta) {
        //            if (type === 'display') {
        //                return $('<div>')
        //                   .attr('class', 'text-right')
        //                   .text(data)
        //                   .wrap('<div></div>')
        //                   .parent()
        //                   .html();

        //            } else {
        //                return data;
        //            }
        //        }
        //    }


        //        ]

        //    });
        //    //alert(arr1);

        //    $('#spinner').hide();
        //}


    </script>
  
    <style type="text/css">
        .excelbtn, #ContentPlaceHolder1_btnBack {
            background-color: #3c8dbc;
            border-color: #367fa9;
        }

        #excelExport {
            border-radius: 3px;
            -webkit-box-shadow: none;
            box-shadow: none;
            border: 1px solid transparent;
            background-color: #3c8dbc;
            border-color: #367fa9;
            color: white;
            height: 33px;
            padding: 0 0 3px 1px;
        }
    </style>
    <style>
        .alignright {
            text-align: right;
        }

        .qty.text-right {
            text-align: right !important;
        }

        table#ContentPlaceHolder1_targetRadioButtonList {
            width: 100%;
        }


        table#ContentPlaceHolder1_itemgroupRadioButtonlst {
            width: 100%;
        }

        #ContentPlaceHolder1_targetRadioButtonList_0 {
            margin-right: 5px !important;
            margin-top: 3px;
        }

        #ContentPlaceHolder1_targetRadioButtonList_1 {
            margin-right: 5px !important;
            margin-top: 3px;
        }

        #ContentPlaceHolder1_targetRadioButtonList td {
            padding: 3px;
        }


        #ContentPlaceHolder1_itemgroupRadioButtonlst_0 {
            margin-right: 5px !important;
            margin-top: 3px;
        }

        #ContentPlaceHolder1_itemgroupRadioButtonlst_1 {
            margin-right: 5px !important;
            margin-top: 3px;
        }

        #ContentPlaceHolder1_itemgroupRadioButtonlst td {
            padding: 3px;
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
        <div class="row">
            <!-- left column -->
            <div class="col-md-12">
                <div id="InputWork">
                    <!-- general form elements -->
                    <div class="box box-primary">
                        <div class="box-header with-border">
                           <%-- <h3 class="box-title">Distributor Stock Report </h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            <div style="float: right">
                            </div>
                        </div>
                        <div class="box-body">
                            <div>

                                <div class="form-group" hidden>
                                    <label for="exampleInputEmail1">Sales Person:</label>
                                    <asp:DropDownList ID="ddlunderUser" runat="server" CssClass="form-control"></asp:DropDownList>

                                    <%--<asp:HiddenField ID="dateHiddenField" runat="server" />
                                    <asp:HiddenField ID="smIDHiddenField" runat="server" />
                                    <asp:HiddenField ID="beatIDHiddenField" runat="server" />--%>
                                </div>
                                <div class="row">
                                  


                                    <div class="col-md-3 col-sm-6 col-xs-10">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Area:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />
                                            <asp:ListBox ID="ListArea" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="ListArea_SelectedIndexChanged"></asp:ListBox>
                                            <input type="hidden" id="Hidden1" runat="server" />
                                        </div>
                                    </div>
                                      <div class="col-md-3 col-sm-6 col-xs-10">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Distributor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />
                                            <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                            <input type="hidden" id="hiddistributor" runat="server" />
                                        </div>
                                    </div>

                                </div>

                                  <div class="row">
                                   
                                      <div class="col-md-3 col-sm-6 col-xs-10">
                                        <div id="DIV1" class="form-group">
                                            <label for="exampleInputEmail1">From Date:</label>
                                            <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" OnClientDateSelectionChanged="checkDate" />
                                        </div>
                                    </div>
                                
                                      <div class="col-md-3 col-sm-6 col-xs-10">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">To Date:</label>
                                            <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" OnClientDateSelectionChanged="checkDate1" />
                                        </div>
                                    </div>

                                </div>

                              
                               
                                <div class="form-group">
                                    <input Style="margin-top: 5px;" type="button" runat="server" onclick="btnSubmitfunc();" class="btn btn-primary" value="Show" />


                                 <%--   <asp:Button Style="margin-top: 5px;" type="button" ID="btnshow" runat="server" Text="Show" class="btn btn-primary" Visible="false" OnClick="btnshow_Click" OnClientClick="javascript:return btnSubmitfunc();" />--%>
                                    <asp:Button Style="margin-top: 5px;" ID="Cancel" CssClass="btn btn-primary" Text="Cancel" OnClick="Cancel_Click" runat="server" />
                                    <asp:Button Style="margin-right: 5px; margin-top: 5px;" type="button" ID="btnexport" runat="server" Text="Export" class="btn btn-primary" OnClick="btnexport_Click" OnClientClick="javascript:return btnSubmitfunc1();" />
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <%-- <div class="col-md-12">
                                <div class="form-group">
                                    <div class="table table-responsive">
                                        <!--<label for="exampleInputEmail1"></label>-->
                                        <asp:GridView ID="grd" runat="server" OnRowDataBound="grd_RowDataBound" AutoGenerateColumns="false" class="table table-bordered table-striped">
                                            <Columns>
                                                <%-- <asp:BoundField DataField="SMName" HeaderText="Name" />
                                                <asp:TemplateField HeaderText="Product Group">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hidPartyTypeId" runat="server" Value='<%#Eval("Id") %>' />
                                                        <asp:Label ID="lblMatGrpType" runat="server" Text='<%#Eval("MatGrp") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Target/Actual Sales">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTargetFromHo" runat="server"></asp:Label>
                                                        <asp:Label ID="Label1" runat="server" Text="Target Sales" CssClass="center-block"></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="Label2" runat="server" Text="Actual Sales" CssClass="center-block"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>

                                                        <asp:Label ID="lnkRegion" runat="server" CommandName="Region" CssClass="center-block" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="lnkRegion1" runat="server" CommandName="Region" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="qty text-right" />
                                                    <HeaderStyle CssClass="qty text-right" />
                                                </asp:TemplateField>

                                                <%--                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnkState" runat="server" CommandName="State" CssClass="center-block" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="lnkState1" runat="server" CommandName="State" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="qty text-right" />
                                                    <HeaderStyle CssClass="qty text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnkDistrict" runat="server" CommandName="District" CssClass="center-block" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="lnkDistrict1" runat="server" CommandName="District" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="qty text-right" />
                                                    <HeaderStyle CssClass="qty text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnkCity" runat="server" CommandName="City" CssClass="center-block" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="lnkCity1" runat="server" CommandName="City" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="qty text-right" />
                                                    <HeaderStyle CssClass="qty text-right" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Area Incharge (Rs. in Lakhs)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnkArea" runat="server" CommandName="Area" CssClass="center-block" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="lnkArea1" runat="server" CommandName="Area" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="qty text-right" />
                                                    <HeaderStyle CssClass="qty text-right" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="Total" HeaderText="Target" Visible="false" />
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <%--    <asp:HiddenField ID="hid" runat="server" Value='<%#Eval("PartyTypeId")%>' />
                                                        <asp:Label ID="lblActual" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>--%>

                            <div id="rptmain1" runat="server">
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="distreportrpt" runat="server">
                                        <HeaderTemplate>
                                            <table id="example11" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                        <%--  <th>SNo.</th>--%>
                                                          <th>Distributor</th>
                                                        <th>Date</th>
                                                        <th>Item</th>
                                                        <th style="text-align: right">Expected Stock</th>
                                                        <th style="text-align:right">Current Stock</th>
                                                         <th style="text-align:right">Invoice Stock</th>
                                                         <th style="text-align:right">Total Sale by Salesperson</th>
                                                        <th style="text-align:right">Total Sale by Distributor</th>
                                                        
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
                            <div>
                                <%--<b>Total Qty=</b>Distributor Stock-Salesperson order (if "order dispatched" then dispatch qty else if "Pending" Qty else 0)+Distributor Invoice Qty <br />--%>
                                <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
