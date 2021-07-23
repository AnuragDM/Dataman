<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="SalesTargetFormLevel3.aspx.cs" Inherits="AstralFFMS.SalesTargetFormLevel3" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
    </script>
    <script>
        function Cal(ab) {
            var p = ab.id;
            var last = p.substr(p.length - 1);
            var apr = document.getElementById("ContentPlaceHolder1_GridView2_txtapr_" + last);
            var may = document.getElementById("ContentPlaceHolder1_GridView2_txtmay_" + last);
            var jun = document.getElementById("ContentPlaceHolder1_GridView2_txtjun_" + last);
            var jul = document.getElementById("ContentPlaceHolder1_GridView2_txtjul_" + last);
            var aug = document.getElementById("ContentPlaceHolder1_GridView2_txtaug_" + last);
            var spt = document.getElementById("ContentPlaceHolder1_GridView2_txtspt_" + last);
            var oct = document.getElementById("ContentPlaceHolder1_GridView2_txtoct_" + last);
            var nov = document.getElementById("ContentPlaceHolder1_GridView2_txtNov_" + last);
            var dec = document.getElementById("ContentPlaceHolder1_GridView2_txtDec_" + last);
            var jan = document.getElementById("ContentPlaceHolder1_GridView2_txtJan_" + last);
            var feb = document.getElementById("ContentPlaceHolder1_GridView2_txtFeb_" + last);
            var mar = document.getElementById("ContentPlaceHolder1_GridView2_txtMarch_" + last);
            var total = document.getElementById("ContentPlaceHolder1_GridView2_lbltargetnoofmeet_" + last);

            var a = 0; var b = 0; var c = 0; var d = 0; var e = 0; var f = 0; var g = 0; var h = 0; var i = 0; var j = 0; var k = 0; var l = 0; var m = 0;
            if (apr.value != '') { a = apr.value; }
            if (may.value != '') { b = may.value; }
            if (jun.value != '') { c = jun.value; }
            if (jul.value != '') { d = jul.value; }
            if (aug.value != '') { e = aug.value; }
            if (spt.value != '') { f = spt.value; }
            if (oct.value != '') { g = oct.value; }
            if (nov.value != '') { h = nov.value; }
            if (dec.value != '') { i = dec.value; }
            if (jan.value != '') { j = jan.value; }
            if (feb.value != '') { k = feb.value; }
            if (mar.value != '') { l = mar.value; }
            // var num = parseFloat(apr.value) + parseFloat(may.value) + parseFloat(jun.value) + parseFloat(jul.value) + parseFloat(aug.value) + parseFloat(spt.value) + parseFloat(oct.value) + parseFloat(nov.value) + parseFloat(dec.value) + parseFloat(feb.value) + parseFloat(mar.value);
            var num = parseFloat(a) + parseFloat(b) + parseFloat(c) + parseFloat(d) + parseFloat(e) + parseFloat(f) + parseFloat(g) + parseFloat(h) + parseFloat(i) + parseFloat(j) + parseFloat(k) + parseFloat(l);
            document.getElementById("ContentPlaceHolder1_GridView2_lbltargetnoofmeet_" + last).value = num;

            var PTotalAmount = 0;
            var qTotalAmount = 0;
            var rTotalAmount = 0;
            var sTotalAmount = 0;
            var tTotalAmount = 0;
            var uTotalAmount = 0;
            var vTotalAmount = 0;
            var wTotalAmount = 0;
            var xTotalAmount = 0;
            var yTotalAmount = 0;
            var zTotalAmount = 0;
            var nTotalAmount = 0;

            var final = "0";

            for (var z1 = 0; z1 < document.getElementById("ContentPlaceHolder1_GridView2").rows.length - 2; z1 += 1) {

                var apr1 = document.getElementById("ContentPlaceHolder1_GridView2_txtapr_" + z1);
                var may1 = document.getElementById("ContentPlaceHolder1_GridView2_txtmay_" + z1);
                var jun1 = document.getElementById("ContentPlaceHolder1_GridView2_txtjun_" + z1);
                var jul1 = document.getElementById("ContentPlaceHolder1_GridView2_txtjul_" + z1);
                var aug1 = document.getElementById("ContentPlaceHolder1_GridView2_txtaug_" + z1);
                var spt1 = document.getElementById("ContentPlaceHolder1_GridView2_txtspt_" + z1);
                var oct1 = document.getElementById("ContentPlaceHolder1_GridView2_txtoct_" + z1);
                var nov1 = document.getElementById("ContentPlaceHolder1_GridView2_txtNov_" + z1);
                var dec1 = document.getElementById("ContentPlaceHolder1_GridView2_txtDec_" + z1);
                var jan1 = document.getElementById("ContentPlaceHolder1_GridView2_txtJan_" + z1);
                var feb1 = document.getElementById("ContentPlaceHolder1_GridView2_txtFeb_" + z1);
                var mar1 = document.getElementById("ContentPlaceHolder1_GridView2_txtMarch_" + z1);

                var p = 0; var q = 0; var r = 0; var s = 0; var t = 0; var u = 0; var v = 0; var w = 0; var x = 0; var y = 0; var z = 0; var n = 0;

                if (apr1.value != '') { p = apr1.value; }
                if (may1.value != '') { q = may1.value; }
                if (jun1.value != '') { r = jun1.value; }
                if (jul1.value != '') { s = jul1.value; }
                if (aug1.value != '') { t = aug1.value; }
                if (spt1.value != '') { u = spt1.value; }
                if (oct1.value != '') { v = oct1.value; }
                if (nov1.value != '') { w = nov1.value; }
                if (dec1.value != '') { x = dec1.value; }
                if (jan1.value != '') { y = jan1.value; }
                if (feb1.value != '') { z = feb1.value; }
                if (mar1.value != '') { n = mar1.value; }

                PTotalAmount = parseFloat(p) + PTotalAmount;
                qTotalAmount = parseFloat(q) + qTotalAmount;
                rTotalAmount = parseFloat(r) + rTotalAmount;
                sTotalAmount = parseFloat(s) + sTotalAmount;
                tTotalAmount = parseFloat(t) + tTotalAmount;
                uTotalAmount = parseFloat(u) + uTotalAmount;
                vTotalAmount = parseFloat(v) + vTotalAmount;
                wTotalAmount = parseFloat(w) + wTotalAmount;
                xTotalAmount = parseFloat(x) + xTotalAmount;
                yTotalAmount = parseFloat(y) + yTotalAmount;
                zTotalAmount = parseFloat(z) + zTotalAmount;
                nTotalAmount = parseFloat(n) + nTotalAmount;


                document.getElementById("ContentPlaceHolder1_GridView2_txtapr1").value = PTotalAmount;
                document.getElementById("ContentPlaceHolder1_GridView2_txtmay1").value = qTotalAmount;
                document.getElementById("ContentPlaceHolder1_GridView2_txtjun1").value = rTotalAmount;
                document.getElementById("ContentPlaceHolder1_GridView2_txtjul1").value = sTotalAmount;
                document.getElementById("ContentPlaceHolder1_GridView2_txtaug1").value = tTotalAmount;
                document.getElementById("ContentPlaceHolder1_GridView2_txtspt1").value = uTotalAmount;
                document.getElementById("ContentPlaceHolder1_GridView2_txtoct1").value = vTotalAmount;
                document.getElementById("ContentPlaceHolder1_GridView2_txtNov1").value = wTotalAmount;
                document.getElementById("ContentPlaceHolder1_GridView2_txtDec1").value = xTotalAmount;
                document.getElementById("ContentPlaceHolder1_GridView2_txtJan1").value = yTotalAmount;
                document.getElementById("ContentPlaceHolder1_GridView2_txtFeb1").value = zTotalAmount;
                document.getElementById("ContentPlaceHolder1_GridView2_txtMarch1").value = nTotalAmount;
                // alert('aSA');

                // document.getElementById("ContentPlaceHolder1_GridView2_lbltargetnoofmeet1").value = final;

            }
            //alert('asdgasjd');
            final = PTotalAmount + qTotalAmount + rTotalAmount + sTotalAmount + tTotalAmount + uTotalAmount + vTotalAmount + wTotalAmount + xTotalAmount + yTotalAmount + zTotalAmount + nTotalAmount;

            // alert(final);

            document.getElementById("ContentPlaceHolder1_GridView2_lbltargetnoofmeet1").value = final;
        }
    </script>



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
                            <h3 class="box-title">Sales Target Entry <b>(In Lakhs)</b></h3>
                            <div style="float: right">
                                <%--       <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" />--%>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-lg-3 col-md-4 col-sm-4 col-xs-9">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Year:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="txtcurrentyear" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="txtcurrentyear_SelectedIndexChanged"></asp:DropDownList>
                                    <%-- <asp:TextBox ID="txtcurrentyear" runat="server"></asp:TextBox>--%>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="col-md-12">
                                <div class="form-group">
                                    <div class="table table-responsive">
                                        <!--<label for="exampleInputEmail1"></label>-->
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Product Group">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hidPartyTypeId" runat="server" Value='<%#Eval("Id") %>' />
                                                        <asp:Label ID="lblUserType" runat="server" Text='<%#Eval("Name") %>'></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Target from HO/Senior">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hidusertype" runat="server" Value='<%#Eval("Id") %>' />
                                                        <asp:HiddenField ID="lblUserTypeName" runat="server" Value='<%#Eval("Name") %>' />
                                                        <asp:Label ID="lblTargetFromHo" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkRegion" runat="server" CommandName="Region" CommandArgument='<%#Eval("Id")%>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkState" runat="server" CommandName="State" CommandArgument='<%#Eval("Id")%>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDistrict" runat="server" CommandName="District" CommandArgument='<%#Eval("Id")%>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkCity" runat="server" CommandName="City" CommandArgument='<%#Eval("Id")%>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%--  <asp:TemplateField HeaderText="Planned by Area Incharge">
                                               <ItemTemplate>
                                                   <asp:LinkButton ID="lnkArea" runat="server" CommandName="Area" CommandArgument=<%#Eval("Id")%> ></asp:LinkButton>
                                               </ItemTemplate>
                                           </asp:TemplateField>--%>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="table table-responsive">
                                        <h3 class="box-title">
                                            <asp:Label ID="lblPartTypeName" runat="server"></asp:Label></h3>
                                        <asp:Label ID="lblPartTypeID" Visible="false" runat="server" CssClass="form-control text-bold">
                                        </asp:Label>
                                        <asp:GridView ID="GridView2" runat="server" ShowFooter="true" AutoGenerateColumns="false" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" OnRowDataBound="GridView2_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Team">
                                                    <ItemTemplate>

                                                        <%--  <asp:HiddenField ID="hidusertype1" runat="server" Value=<%#Eval("PartyTypeName") %>/>--%>

                                                        <asp:HiddenField ID="PartyId" runat="server" Value='<%#Eval("SMId") %>' />
                                                        <asp:Label ID="lblUserType" runat="server" Text='<%#Eval("SMName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lbltotal" Text="Total" runat="server" CssClass="form-control text-bold"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Target No of Meet">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lbltargetnoofmeet" Width="80px" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <%--  <asp:Label ID="lbltargetnoofmeet" Text="0" runat="server" CssClass="control"></asp:Label>--%>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="lbltargetnoofmeet1" Width="80px" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>

                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Apr">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtapr" runat="server" Width="80px" onblur="Cal(this);" CssClass="form-control numeric text-right" MaxLength="8"> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtapr1" runat="server" Width="80px" onblur="Cal(this);" ReadOnly="true" CssClass="form-control numeric text-right" MaxLength="12"> </asp:TextBox>
                                                    </FooterTemplate>

                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="May">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtmay" runat="server" Width="80px" onblur="Cal(this);" CssClass="form-control numeric text-right" MaxLength="8"> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtmay1" runat="server" Width="80px" onblur="Cal(this);" ReadOnly="true" CssClass="form-control numeric text-right" MaxLength="12"> </asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Jun">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtjun" runat="server" Width="80px" onblur="Cal(this);" CssClass="form-control numeric text-right" MaxLength="8"> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtjun1" runat="server" Width="80px" onblur="Cal(this);" ReadOnly="true" CssClass="form-control numeric text-right" onkeypress="return isNumber(event)" MaxLength="12"> </asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Jul">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtjul" runat="server" Width="80px" onblur="Cal(this);" CssClass="form-control numeric text-right" MaxLength="8"> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtjul1" runat="server" Width="80px" onblur="Cal(this);" ReadOnly="true" CssClass="form-control numeric text-right" MaxLength="12"> </asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Aug">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtaug" runat="server" Width="80px" onblur="Cal(this);" CssClass="form-control numeric text-right" MaxLength="8"> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtaug1" runat="server" Width="80px" onblur="Cal(this);" ReadOnly="true" CssClass="form-control numeric text-right" MaxLength="12"> </asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Sept">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtspt" runat="server" Width="80px" onblur="Cal(this);" MaxLength="8" CssClass="form-control numeric text-right"> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtspt1" runat="server" Width="80px" onblur="Cal(this);" ReadOnly="true" CssClass="form-control numeric text-right" MaxLength="12"> </asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Oct">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtoct" runat="server" Width="80px" onblur="Cal(this);" MaxLength="8" CssClass="form-control numeric text-right"> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtoct1" runat="server" Width="80px" onblur="Cal(this);" ReadOnly="true" CssClass="form-control numeric text-right" MaxLength="12"> </asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nov">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNov" runat="server" Width="80px" onblur="Cal(this);" MaxLength="8" CssClass="form-control numeric text-right"> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtNov1" runat="server" Width="80px" onblur="Cal(this);" ReadOnly="true" CssClass="form-control numeric text-right" MaxLength="12"> </asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dec">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDec" runat="server" Width="80px" onblur="Cal(this);" MaxLength="8" CssClass="form-control numeric text-right"> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtDec1" runat="server" Width="80px" onblur="Cal(this);" ReadOnly="true" CssClass="form-control numeric text-right" MaxLength="12"> </asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Jan">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtJan" runat="server" Width="80px" onblur="Cal(this);" MaxLength="8" CssClass="form-control numeric text-right"> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtJan1" runat="server" Width="80px" onblur="Cal(this);" ReadOnly="true" CssClass="form-control numeric text-right" MaxLength="12"> </asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Feb">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFeb" runat="server" Width="80px" onblur="Cal(this);" MaxLength="8" CssClass="form-control numeric text-right"> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFeb1" runat="server" Width="80px" onblur="Cal(this);" ReadOnly="true" CssClass="form-control numeric text-right" MaxLength="12"> </asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mar">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMarch" onblur="Cal(this);" Width="80px" MaxLength="8" runat="server" CssClass="form-control numeric text-right"> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtMarch1" onblur="Cal(this);" Width="80px" ReadOnly="true" MaxLength="12" runat="server" CssClass="form-control numeric text-right"> </asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>


                                <div class="form-group">
                                    <div class="table table-responsive">
                                        <!-- <label for="exampleInputEmail1"></label>-->
                                        <h3 class="box-title">
                                            <asp:Label ID="lblpartyName1" runat="server"></asp:Label></h3>
                                        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" OnRowDataBound="GridView3_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Team">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="PartyId" runat="server" Value='<%#Eval("SMId") %>' />
                                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("UserId") %>' />
                                                        <asp:Label ID="lblUserType" runat="server" Text='<%#Eval("SMName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lbltotal" Text="Total" runat="server" CssClass="form-control text-bold"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Target from HO/Senior">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTargetFromHo" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Allocate to Team">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAllocatetoTeam" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Pending Target To Allocate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblpending" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>



                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>



                        </div>
                        <div class="box-footer">
                            <asp:Button ID="btnsave" OnClientClick="return valid();" CssClass="btn btn-primary" Text="Save" OnClick="btnsave_Click" runat="server" />
                            <asp:Button ID="Cancel" CssClass="btn btn-primary" Text="Cancel" OnClick="Cancel_Click" runat="server" />
                        </div>
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
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
    <script>

        function valid() {
            if ($('#<%=txtcurrentyear.ClientID %>').val() == '0') {
                errormessage("Please select the Financial Year");
                return false;
            }
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
