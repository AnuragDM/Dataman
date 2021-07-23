<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="MeetvsActualMeetReport.aspx.cs" Inherits="AstralFFMS.MeetvsActualMeetReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
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
                             //$(this).removeAttr("checked");
                         }
                     });
                 } else {
                     //Is Child CheckBox
                     var parentDIV = $(this).closest("DIV");
                     if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                         $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                     } else {
                         //$("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                     }
                 }
             });
         })
    </script>
     <script type="text/javascript">
         $(function () {
             $('[id*=listboxmonth]').multiselect({
                 enableCaseInsensitiveFiltering: true,
                 buttonWidth: '70%',
                 includeSelectAllOption: true,
                 maxHeight: 200,
                 width: 100,
                 //enableFiltering: true,
                 //filterPlaceholder: 'Search'
             });
         });
    </script>
    <style>
        @media screen and (max-width: 767px) {
            .table-responsive {
                border: 1px solid #fff !important;
            }
        }

        table tr td, table tr th {
            padding: 2px 8px;
        }
    </style>

    <section class="content">
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
                          <%--  <h3 class="box-title">Meet Target Vs Actual Meet </h3>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                            <div style="float: right">
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-9">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Year:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="txtcurrentyear" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                    <div class="form-group">
                                             <label for="exampleInputEmail1">Months:</label>
                                             <asp:ListBox ID="listboxmonth" CssClass="form-control" runat="server" SelectionMode="Multiple"></asp:ListBox>                                                  
                                     </div>
                                <div class="form-group" hidden>
                                    <label for="exampleInputEmail1">Sales Person:</label>
                                    <asp:DropDownList ID="ddlunderUser" runat="server" CssClass="form-control"></asp:DropDownList>

                                </div>
                                 <div class="form-group">
                                    <label for="exampleInputEmail1">Sales Person:</label>
                                    <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView>

                                </div>
                                <div class="form-group">

                                    <asp:Button ID="btnshow" CssClass="btn btn-primary" Text="Show" OnClick="btnshow_Click" runat="server" />
                                    <asp:Button ID="Cancel" CssClass="btn btn-primary" Text="Cancel" OnClick="Cancel_Click" runat="server" />
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnexport" runat="server" Text="Export" class="btn btn-primary" OnClick="btnexport_Click" />
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="col-md-12">
                                <div class="form-group">
                                    <div class="table table-responsive">
                                        <!--<label for="exampleInputEmail1"></label>-->
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" OnRowDataBound="GridView1_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Type of Meet">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hidPartyTypeId" runat="server" Value='<%#Eval("Id") %>' />

                                                        <asp:Label ID="lblUserType" runat="server" Text='<%#Eval("Name") %>'></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Target/Actual Meet">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hidusertype" runat="server" Value='<%#Eval("Id") %>' />
                                                        <asp:HiddenField ID="lblUserTypeName" runat="server" Value='<%#Eval("Name") %>' />
                                                        <asp:Label ID="lblTargetFromHo" runat="server"></asp:Label>
                                                        <asp:Label ID="Label1" runat="server" Text="Target Meet" CssClass="center-block"></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="Label2" runat="server" Text="Actual Meet" CssClass="center-block"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>

                                                        <asp:Label ID="lnkRegion" runat="server" CommandName="Region" CommandArgument='<%#Eval("Id")%>' CssClass="center-block"></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="lnkRegion1" runat="server" CommandName="Region" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnkState" runat="server" CommandName="State" CommandArgument='<%#Eval("Id")%>' CssClass="center-block"></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="lnkState1" runat="server" CommandName="State" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnkDistrict" runat="server" CommandName="District" CommandArgument='<%#Eval("Id")%>' CssClass="center-block"></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="lnkDistrict1" runat="server" CommandName="District" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnkCity" runat="server" CommandName="City" CommandArgument='<%#Eval("Id")%>' CssClass="center-block"></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="lnkCity1" runat="server" CommandName="City" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Area Incharge">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnkArea" runat="server" CommandName="Area" CommandArgument='<%#Eval("Id")%>' CssClass="center-block"></asp:Label>
                                                        <hr style="border-color: #808080; margin-top: 5px; margin-bottom: 5px" />
                                                        <asp:Label ID="lnkArea1" runat="server" CommandName="Area" CommandArgument='<%#Eval("Id")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div>
                                <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                            </div>
                        </div>
                        <div class="box-footer">
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </section>

    <script src="Scripts/jquery.numeric.min.js"></script>
    <script type="text/javascript">
        $(".numeric").numeric();
    </script>
<%--    <script>

        function valid() {
            if ($('#<%=txtcurrentyear.ClientID %>').val() == '0') {
                errormessage("Please select the Financial Year");
                return false;
            }
        }
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

    <script>
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 9 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57)) {

                // if (iKeyCode != 8 && iKeyCode != 0 && (iKeyCode < 48 || iKeyCode > 57))

                return false;
            }
            return true;
        }
    </script>
</asp:Content>



