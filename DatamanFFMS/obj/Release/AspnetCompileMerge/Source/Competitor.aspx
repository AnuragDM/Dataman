<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="Competitor.aspx.cs" EnableEventValidation="false" Inherits="AstralFFMS.Competitor" %>

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
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");
        }
    </script>    
    <script>
        function valid() {

            if ($('#<%=compTextBox.ClientID%>').val() == "") {
                errormessage("Please enter the Competitor");
                return false;
            }

            if ($('#<%=txtitem.ClientID%>').val() == "") {
                errormessage("Please enter the Item");
                return false;
            }

            <%--if ($('#<%=txtQuantity.ClientID%>').val() == "") {
                errormessage("Please enter the Std. Packing");
                return false;
            }--%>
           <%-- if ($('#<%=txtQuantity.ClientID%>').val() == "0.00") {
                errormessage("Please enter the Std. Packing greater than 0");
                return false;
            }--%>
            if ($('#<%=txtRate.ClientID%>').val() == "") {
                errormessage("Please enter the Rate");
                return false;
            }
            if ($('#<%=txtRate.ClientID%>').val() == "0.00") {
                errormessage("Please enter the Rate greater than 0");
                return false;
            }
            if ($('#<%=Remark.ClientID%>').val() == "") {
                errormessage("Please enter the Remark");
                return false;
            }
            if ($('#<%=txtDiscount.ClientID%>').val() > 100) {
                errormessage("Discount cannot be greater than 100");
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function showpreview(input) {
            var uploadcontrol = document.getElementById('<%=dsrImgFileUpload.ClientID%>').value;
            var reg = /(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/;
            if (uploadcontrol.length > 0) {
                if (reg.test(uploadcontrol)) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $("#ContentPlaceHolder1_imgpreview").css('display', 'block');
                            $("#ContentPlaceHolder1_imgpreview").attr('src', e.target.result);
                        }
                        reader.readAsDataURL(input.files[0]);
                    }
                }
                else {
                    errormessage("Only image files are allowed!");
                    $("#ContentPlaceHolder1_imgpreview").css('display', 'none');
                    return false;
                }
            }
        }
    </script>
     <script type="text/javascript">
         function ClientPartySelected(sender, e) {
             $get("<%=hfCompId.ClientID %>").value = e.get_value();
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
    
<%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
<script type="text/javascript">
    $(function () {
        $("#chkotheractvity").click(function () {
            if ($(this).is(":checked")) {

                //$("#dvbrand").show();
                //$("#dvmeet").show();
                //$("#dvroadshow").show();
                //$("#dvscheme").show();
                //$("#dvotherinfo").show();
                $('div[id$="dvbrand"]').show();
                $('div[id$="dvmeet"]').show();
                $('div[id$="dvroadshow"]').show();
                $('div[id$="dvscheme"]').show();
                $('div[id$="dvotherinfo"]').show();
            } else {
                //$("#dvbrand").hide();
                //$("#dvmeet").hide();
                //$("#dvroadshow").hide();
                //$("#dvscheme").hide();
                //$("#dvotherinfo").hide();
                $('div[id$="dvbrand"]').hide();
                $('div[id$="dvmeet"]').hide();
                $('div[id$="dvroadshow"]').hide();
                $('div[id$="dvscheme"]').hide();
                $('div[id$="dvotherinfo"]').hide();
               <%-- $("#<%=txtbrand.ClientID%>").val("");
                $("#<%=txtmeet.ClientID%>").val("");
                $("#<%=txtroadshow.ClientID%>").val("");
                $("#<%=txtscheme.ClientID%>").val("");
                $("#<%=txtother.ClientID%>").val("");--%>

            }

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

                                <div class="form-group">

                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                    <div class="col-sm-12">
                                        <div class="col-lg-4 col-md-5 col-sm-8 paddingleft0">
                                            <asp:Label ID="partyName" runat="server" CssClass="text" Text="Label"></asp:Label><br />
                                            <asp:Label ID="address" runat="server" CssClass="text" Text="Label"></asp:Label>,&nbsp;
                                     <asp:Label ID="lblzipcode" runat="server" CssClass="text" Text=""></asp:Label>,&nbsp;
                                     <asp:Label ID="mobile" runat="server" CssClass="text" Text="Label"></asp:Label>&nbsp;
                                        </div>
                                        <div class="col-lg-2 col-md-2 col-sm-8 paddingleft0">
                                            <asp:Label ID="lblVisitdate1" runat="server" CssClass="text" Text="Visit Date"></asp:Label><br />
                                            <asp:Label ID="lblVisitDate5" runat="server" CssClass="text" Text="Visit Date"></asp:Label>&nbsp;
                                        </div>
                                        <div class="col-lg-2 col-md-2 col-sm-8 paddingleft0">
                                            <asp:Label ID="lblAreaName1" runat="server" CssClass="text" Text="Area Name"></asp:Label><br />
                                            <asp:Label ID="lblBeatName5" runat="server" CssClass="text" Text="Beat Name"></asp:Label>&nbsp;
                                        </div>
                                        <div class="col-lg-2 col-md-2 col-sm-8 paddingleft0">
                                            <asp:Label ID="lblBeatName1" runat="server" CssClass="text" Text="Beat Name"></asp:Label><br />
                                            <asp:Label ID="lblAreaName5" runat="server" CssClass="text" Text="Area Name"></asp:Label>&nbsp;
                                        </div>

                                        <div class="col-lg-2 col-md-1 col-sm-8 paddingleft0" style="float: right">

                                            <asp:Button Style="margin-right: 5px; margin-bottom: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                                OnClick="btnFind_Click" />
                                            <asp:Button ID="btnBack1" runat="server" Text="Back" Style="margin-right: 5px; margin-bottom: 5px;" class="btn btn-primary" OnClick="btnBack_Click1" />

                                            <asp:HiddenField ID="HiddenField2" runat="server" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <div class="col-sm-4 paddingleft0">
                                        </div>
                                        <div class="col-sm-2 paddingleft0">
                                        </div>
                                        <div class="col-sm-2 paddingleft0">
                                        </div>
                                        <div class="col-sm-2 paddingleft0">
                                        </div>

                                    </div>
                                </div>

                            </div>
                            <div class="form-group paddingleft0">
                                <h3>Competitor's Activity</h3>
                                <asp:HiddenField ID="hid" runat="server" />
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->

                            <div class="box-body">
                                <div class="col-md-5 col-sm-6 col-xs-9">
                                    <div class="form-group col-md-7 paddingleft0">
                                        <label for="exampleInputEmail1">Competitor Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="compTextBox" runat="server" class="form-control" placeholder="Enter Competitor "></asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" FirstRowSelected="false" OnClientItemSelected="ClientItemSelected"
                                                        runat="server" BehaviorID="txtSearch_AutoCompleteExtender" CompletionListCssClass="completionList"
                                                        CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted"
                                                        DelimiterCharacters="" ServiceMethod="SearchCompetitor" ServicePath="~/Competitor.aspx" MinimumPrefixLength="3" EnableCaching="true" TargetControlID="compTextBox">
                                                    </ajaxToolkit:AutoCompleteExtender>
                                                    <asp:HiddenField ID="hfCompId" runat="server" />
                                    </div>
                                    <div class="form-group col-md-7 paddingleft0">
                                        <label for="exampleInputEmail1">Item:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtitem" CssClass="form-control" runat="server" MaxLength="100"></asp:TextBox>
                                    </div>
                                    <div id="divdocid" runat="server" class="form-group col-md-5 paddingright0">
                                        <label for="exampleInputEmail1">Document No:</label>
                                        <asp:TextBox ID="lbldocno" Enabled="false" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>


                                    <div class="form-group col-md-7 paddingleft0">
                                        <label for="exampleInputEmail1">Std. Packing:</label>
                                       <%-- &nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>--%>
                                        <asp:TextBox ID="txtQuantity" runat="server" class="form-control numeric text-right" Text="0.00" MaxLength="9"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-md-7 paddingleft0">
                                        <label for="exampleInputEmail1">Rate:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="txtRate" runat="server" class="form-control numeric text-right" Text="0.00" MaxLength="9"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-md-7 paddingleft0">
                                        <label for="exampleInputEmail1">Discount %:</label>
                                        <asp:TextBox ID="txtDiscount" runat="server" class="form-control numeric text-right" Text="0.00" MaxLength="5"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-md-7 paddingleft0">
                                        <label for="exampleInputEmail1">Remark:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                        <asp:TextBox ID="Remark" runat="server" Style="resize: none; height: 20%;" class="form-control" cols="20" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                    </div>   
                                      <div class="form-group col-md-7 paddingleft0">
                                        <label for="exampleInputEmail1">Other Activity:</label>&nbsp;&nbsp;
                                        <asp:CheckBox ID ="chkotheractvity" runat="server" ClientIDMode="Static"/>                                          
                                           <%--<input type="checkbox" id="chkotheractvity" class="checkbox" />--%>                                         
                                    </div>  
                                    <div id="dvbrand" class="form-group col-md-7 paddingleft0" runat="server" style="display:none" >
                                        <label for="exampleInputEmail1">Branding Activity:</label>
                                          <asp:TextBox ID="txtbrand" runat="server" Style="resize: none; height: 20%;" class="form-control" cols="20" Rows="2" TextMode="MultiLine" placeholder="Enter Branding Activity " ></asp:TextBox>
                                           <%--  <input type="text" id="txtbrand" aria-multiline="true" class="form-control" style="height:55px" runat="server" placeholder="Enter Branding Activity " /> --%>
                                                                                                             
                                    </div>
                                     <div id="dvmeet" class="form-group col-md-7 paddingleft0" runat="server" style="display:none" >
                                        <label for="exampleInputEmail1">Meet Activity:</label>
                                        <asp:TextBox ID="txtmeet" runat="server" Style="resize: none; height: 20%;" class="form-control" cols="20" Rows="2" TextMode="MultiLine" placeholder="Enter Meet Activity " ></asp:TextBox>
                                      <%--   <input type="text" id="txtmeet" class="form-control" style="height:55px"   runat="server" placeholder="Enter Meet Activity " />                                                                                               --%>   
                                    </div>
                                    <div id="dvroadshow" class="form-group col-md-7 paddingleft0" runat="server" style="display:none">
                                        <label for="exampleInputEmail1">Road Show:</label>
                                        <asp:TextBox ID="txtroadshow" runat="server" Style="resize: none; height: 20%;" class="form-control" cols="20" Rows="2" TextMode="MultiLine" placeholder="Enter Road Show " ></asp:TextBox>
                                      <%--  <input type="text" id="txtroadshow" class="form-control" style="height:55px" runat="server" placeholder="Enter Road Show " />  --%>
                                    </div>   
                                    <div id="dvscheme" class="form-group col-md-7 paddingleft0" runat="server" style="display:none">
                                        <label for="exampleInputEmail1">Scheme/offers:</label>
                                       <asp:TextBox ID="txtscheme" runat="server" Style="resize: none; height: 20%;" class="form-control" cols="20" Rows="2" TextMode="MultiLine" placeholder="Enter Scheme / offers " ></asp:TextBox>
                                      <%--  <input type="text" id="txtscheme" class="form-control" style="height:55px" runat="server" placeholder="Enter Scheme / offers " />  --%>
                                    </div>   
                                    <div id="dvotherinfo" class="form-group col-md-7 paddingleft0" runat="server" style="display:none">
                                        <label for="exampleInputEmail1">Other General Info:</label>
                                       <asp:TextBox ID="txtother" runat="server" Style="resize: none; height: 20%;" class="form-control" cols="20" Rows="2" TextMode="MultiLine" placeholder="Enter Other General Info " ></asp:TextBox>
                                      <%--  <input type="text" id="txtother" class="form-control" style="height:55px" runat="server" placeholder="Enter Other General Info " /> --%> 
                                    </div>                                                                        
                                    <div class="form-group col-md-7 paddingleft0">
                                        <label for="exampleInputEmail1">Image:</label>
                                        <asp:FileUpload ID="dsrImgFileUpload" runat="server" onchange="showpreview(this);" accept=".png,.jpg,.jpeg,.gif" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Visible="false" runat="server" ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$"
                                            ControlToValidate="comImgFileUpload" ErrorMessage="Please select only image file." ForeColor="Red" Display="Dynamic">
                                        </asp:RegularExpressionValidator>
                                        <img id="imgpreview" height="200" width="200" src="" style="border-width: 0px; display: none;" runat="server" />
                                    </div>
                                  
                                </div>

                            </div>
                            <div class="form-group">                                                            
                                <b>Note : Image size should be less than 1MB</b>                       
                             </div>
                            <div class="box-footer">
                                <asp:Button ID="btnsave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnsave_Click" OnClientClick="return valid();" />&nbsp;&nbsp;
                                <asp:Button ID="btnreset" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnreset_Click" />&nbsp;&nbsp;
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnDelete" runat="server" Text="Delete" class="btn btn-primary" OnClientClick="Confirm();" OnClick="btnDelete_Click" />                                
                                <br />
                                <br />
                                <div>
                                    <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="box-body" id="rptmain" runat="server" style="display: none;">
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Competitor's Activity List</h3>
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
                                                <th>Document Date</th>
                                                <th>Competitor</th>
                                                <th>Document No.</th>
                                                <th>Item</th>
                                                <th style="text-align: right;">Rate</th>
                                                <th style="text-align: right;">Std. Packing</th>
                                                <th style="text-align: right;">Discount</th>
                                                <th>Other Activity</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr onclick="DoNav('<%#Eval("ComptId") %>');">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("ComptId") %>' />
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("VDate"))%></td>
                                        <td><%#Eval("CompName") %></td>
                                        <td><%#Eval("DocId") %></td>
                                        <td><%#Eval("Item") %></td>
                                        <td style="text-align: right;"><%#Eval("Rate") %></td>
                                        <td style="text-align: right;"><%#Eval("Qty") %></td>
                                        <td style="text-align: right;"><%#Eval("Discount") %></td>
                                        <td><%#Eval("OtherActivity") %></td>
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
            $("#example1").DataTable({
                "order": [[0, "desc"]]
            });

        });
    </script>
</asp:Content>
