<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="DemoEntry.aspx.cs" Inherits="AstralFFMS.DemoEntry" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
            $('#<%=lblmasg.ClientID %>').html('Record Inserted Successfully.');
            $("#messageNotification").jqxNotification("open");
        }
    </script>
    <script>
        function valid() {
            <%--if ($('#<%=txt.ClientID%>').val() == "") {
                errormessage("Please enter the Order Amount");
                return false;
            }--%>

            if ($('#<%=Remark.ClientID%>').val() == '') {
                errormessage("Please enter the Remark");
                return false;
            }
        }
    </script>
    <script >
        function Addvalid() {
            <%-- if ($('#<%=ddlproduct.ClientID%>').val() == "0") {
                errormessage("Please select the Product");
                return false;
            }--%>
            return true;
        }
    </script>

        <script type="text/javascript">
            function Confirm() {
                var confirm_value = document.createElement("INPUT");
                confirm_value.type = "hidden";
                confirm_value.name = "confirm_value";
                if (confirm("Are you sure to delete?")) {
                    confirm_value.value = "Yes";
                } else {
                    confirm_value.value = "No";
                }
                document.forms[0].appendChild(confirm_value);
            }
    </script>
        <script type="text/javascript">
            function DoNav(depId) {
                if (depId != "") {
                    document.getElementById("ContentPlaceHolder1_mainDiv").style.display = 'block';
                    document.getElementById("ContentPlaceHolder1_rptmain").style.display = 'none';
                    $('#spinner').show();
                    __doPostBack('', depId)
                }
            }
    </script>
        <script type="text/javascript">
            function ClientItemSelected(sender, e) {
                $get("<%=hfItemId.ClientID %>").value = e.get_value();
        }
    </script>

       <script type="text/javascript">
           $(document).ready(function () {
             
               $('#btnAdd').click(function () {
                   $.ajax({
                       type: "POST",
                       url: '<%= ResolveUrl("DemoEntry.aspx/AddRec") %>',
                       contentType: "application/json; charset=utf-8",
                       data: '{ItemID : "' + $('#<%=hfItemId.ClientID%>').val() + '"}',
                       dataType: "json",
                       success: function (data) {
                         
                           var row = $("[id*=ContentPlaceHolder1_gvDetails] tr:last-child").clone(true);
                           
                           $("[id*=ContentPlaceHolder1_gvDetails] tr").not($("[id*=ContentPlaceHolder1_gvDetails] tr:first-child")).remove();
                           for (var i = 0; i < data.d.length; i++) {
                                $("#ContentPlaceHolder1_gvDetails").append("<tr><td>" + data.d[i].Sr + "</td><td>" + data.d[i].ItemName + "</td><td class=hidden>" + data.d[i].ItemId + "</td><td><input type='button' class='btn btn-primary' value='Delete' onclick=deleteRowdeleteRow(this) id='" + data.d[i].ItemId + "' /></td></tr>");
                           }
                           $('#<%=hfItemId.ClientID%>').val(0);
                           $('#<%=txtSearch.ClientID%>').val("");
                       },
                       error: function (result) {
                           // alert("Error");
                          //alert(result);
                       }
                   });
               });
           });
      </script>  
          
    <section class="content">
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
           <div class="box-body" id="mainDiv" style="display: none;" runat="server">
        <div class="row">
            <!-- left column -->

            <div class="col-md-12">
                <div id="InputWork">
                    <!-- general form elements -->
                    <div class="box box-primary">
                        <div class="box-header with-border">
                            <div class="form-group paddingleft0">
                                <div>
                                    <asp:Label ID="partyName" runat="server" CssClass="text" Text="Label"></asp:Label>
                                    <div style="float: right">
                                         <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                    OnClick="btnFind_Click" />
                                        <asp:Button ID="BtnCancel" runat="server" Text="Back" Style="margin-right: 5px;" class="btn btn-primary" OnClick="BtnCancel_Click1" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group paddingleft0">
                                <div>
                                    <asp:Label ID="address" runat="server" CssClass="text" Text="Label"></asp:Label>,&nbsp;
                                     <asp:Label ID="lblzipcode" runat="server" CssClass="text" Text=""></asp:Label>,&nbsp;
                                     <asp:Label ID="mobile" runat="server" CssClass="text" Text="Label"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group paddingleft0">
                            <h3 >Demo Entry</h3>
                                <asp:HiddenField ID="hid" runat="server" />
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                       
                            <div class="box-body">
                            <div class="col-md-5">
                                <div class="col-md-12 paddingleft0">
                            <div class="form-group col-md-6 paddingleft0">
                                    <label for="complaintNature">Product:</label>
                                    <asp:TextBox ID="txtSearch" runat="server" class="form-control"></asp:TextBox>
                                    <ajaxToolkit:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" FirstRowSelected="false" OnClientItemSelected="ClientItemSelected"
                                        runat="server" BehaviorID="txtSearch_AutoCompleteExtender" CompletionListCssClass="completionList"
                                        CompletionListItemCssClass="listItem"
                                        CompletionListHighlightedItemCssClass="itemHighlighted"
                                        DelimiterCharacters="" ServiceMethod="SearchItem" ServicePath="DemoEntry.aspx" MinimumPrefixLength="3" EnableCaching="true" TargetControlID="txtSearch">
                                    </ajaxToolkit:AutoCompleteExtender>
                                    <asp:HiddenField ID="hfItemId" runat="server" />
                                </div>
                         
                                     <div id="divdocid"  runat="server" class="form-group col-md-6 paddingright0">
                                    <label for="exampleInputEmail1">Document No:</label>
                                    <asp:TextBox ID="lbldocno"  ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                    </div>
                                      <div class="form-group paddingleft0">
                               <input id="btnAdd" type="button" value="Add" onclick="return Addvalid();" class="btn btn-primary" />
                            </div>
                            
                            <div class="form-group">
                                <label for="exampleInputEmail1">Remark:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                <asp:TextBox ID="Remark" runat="server" class="form-control" cols="20" Rows="2" TextMode="MultiLine"></asp:TextBox>
                            </div>
                   <div class="box-body table-responsive">
                 <asp:GridView ID="gvDetails" runat="server" EmptyDataText="" class="table table-bordered table-striped">
                 </asp:GridView>
                        </div>
                        </div>
                      </div>
                    </div>
                </div>

                <div class="box-footer">
                    <asp:Button ID="btnsave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnsave_Click" OnClientClick="return valid();" />&nbsp;&nbsp;
                      <asp:Button ID="btnreset" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnreset_Click"  />&nbsp;&nbsp;
                     <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm();" OnClick="btnDelete_Click" />

                </div>
                <div class="box-footer">
                </div>
            </div>
        </div>
               </div>
         <div class="box-body" id="rptmain" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Order List</h3>
                            <div style="float: right">
                               <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />

                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Document No.</th>
                                                <th>Amount</th>
                                                <th>Remark</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("OrdId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("OrdId") %>' />
                                          <td><%#Eval("OrdDocId") %></td>
                                        <td><%#Eval("OrderAmount") %></td>
                                        <td><%#Eval("Remarks") %></td>
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
                <!-- /.col -->
            </div>

        </div>
    </section>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();

        });
    </script>


     <script>

         function deleteRowdeleteRow(p) {

             $.ajax({
                 type: "POST",
                 url: '<%= ResolveUrl("DemoEntry.aspx/DelRec") %>',
                contentType: "application/json; charset=utf-8",
                data: '{ItemID : "' + p.id + '" }',
                dataType: "json",
                success: function (data) {
                    var row = $("[id*=ContentPlaceHolder1_gvDetails] tr:last-child").clone(true);
                    $("[id*=ContentPlaceHolder1_gvDetails] tr").not($("[id*=ContentPlaceHolder1_gvDetails] tr:first-child")).remove();

                    for (var i = 0; i < data.d.length; i++) {
                        $("#ContentPlaceHolder1_gvDetails").append("<tr><td>" + data.d[i].Sr + "</td><td>" + data.d[i].ItemName + "</td><td class=hidden>" + data.d[i].ItemId + "</td><td><input type='button' class='btn btn-primary' value='Delete' onclick=deleteRowdeleteRow(this)   id='" + data.d[i].ItemId + "' /></td></tr>");
                    }
               
                    },
                error: function (result) {
                    // alert("Error");
                    alert(result);
                }
            });


            }
    </script>

</asp:Content>
