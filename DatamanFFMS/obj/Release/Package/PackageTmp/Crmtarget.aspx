<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="Crmtarget.aspx.cs" Inherits="AstralFFMS.Crmtarget" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<style>
    #ContentPlaceHolder1_GridView4{
        border: 1px solid #3c8dbc;
    }
     

</style>
    <script type="text/javascript">
        function check()
        {
          //  alert("hota h");
      
        var table = document.getElementById("ContentPlaceHolder1_GridView4");
        var rows = table.getElementsByTagName("tr");
        for (var z = 1; z < rows.length; z++) {
            for (var y = 0; y < rows[z].cells.length; y++) {
                if (rows[z].cells[y].style.backgroundColor == "red")
                {
                 
                    //var b = document.getElementById("ContentPlaceHolder1_btnsave"); b.disabled = true;
                    errormessage("Please Enter 7 digit before decimal like 999999.99");
                    //return "true";
                }
                //else
                //{
                //    var b = document.getElementById("ContentPlaceHolder1_btnsave"); b.disabled = false;
                //}
            }
        }
        }
        function allnumeric(txtquantity) {
           
            
            //hidtochkvalidation = "";//var ret = "data-123".replace('data-','');
            var inputVal = document.getElementById(txtquantity.id);
            var b = document.getElementById("ContentPlaceHolder1_btnsave");
            if (txtquantity.value.includes("."))
            {
                //alert("decimal wala");
            var chktargetbeforedecimal = txtquantity.value.split(".")[0];
            var chktargetafterdec = txtquantity.value.split(".")[1];
            if (chktargetbeforedecimal.length > 7)
            {
                errormessage("Please Enter 7 digit before decimal like 999999.99"); 
                inputVal.style.backgroundColor = "red";
                b.disabled = true;
                return false;
            }

            else if (chktargetafterdec.length > 2)
            {
                errormessage("Please Enter 2 digit after decimal like 999999.99"); inputVal.style.backgroundColor = "red";
                b.disabled = true;
                return false;
            } 
            else {
                inputVal.style.backgroundColor = "transparent";
                var test = check();
                if (check == true)
                    b.disabled = true;
                else
                    b.disabled = false;
            }
              }
            else if (txtquantity.value.length > 7) {
                //alert("without decimal wala");
                errormessage("Please Enter 7 digit before decimal like 999999.99");
                inputVal.style.backgroundColor = "red";
                b.disabled = true;
                return false;
            }
            else
            {
                inputVal.style.backgroundColor = "transparent";
                var test = check();
                if (check == true)
                    b.disabled = true;
                else
                    b.disabled = false;
            }
            var test = check();
            if (test == "true") {
                b.disabled = true;
            }
            else {
              //  alert(test);
                b.disabled = false;
            }
        }
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 900, autoClose: true, autoCloseDelay: 3000, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
        var V = "";
        function Successmessage(V) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 900, autoClose: true, autoCloseDelay: 3000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");

        }
        function valid() {
            if ($('#<%=ddlyear.ClientID %>').val() == '0') {
                    errormessage("Please select the Financial Year");
                    return false;
            }

            //if ($("input[name='Target'][value='1']").prop("checked"))
            //{
               
            //}
            //else
            //{
            //    errormessage("Please select Target for");
            //    return false;
            //}
            if ($("input[name*=Target]:checked").length > 0)
                //  alert("checked");
                {}
            else
            {
                errormessage("Please select Target For");
                return false;
            }

        }


    </script>
    <style type="text/css">
        .grid td{
  text-align:center;height:40px;padding: 6px;
}
        .grid th{
  text-align:center;
  height:40px;padding: 6px;
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
                            <%--<h3 class="box-title">Sales Target Entry</h3>--%> <%--<b>(In Lakhs)</b>--%>
                            <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>

                            <div style="float: right">
                                <%--       <input style="margin-right: 5px;" type="button" id="Find" value="Find" class="btn btn-primary" />--%>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->
                        <div class="box-body">
                            <div class="col-lg-3 col-md-4 col-sm-4 col-xs-9">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Year</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                    <asp:DropDownList ID="ddlyear" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <%-- <asp:TextBox ID="txtcurrentyear" runat="server"></asp:TextBox>--%>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-4 col-sm-4 col-xs-9">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Target For</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />
                                    <asp:RadioButton ID="rbRolewise" Text="RoleWise" runat="server" GroupName="Target" AutoPostBack="true"  OnCheckedChanged="rbRolewise_CheckedChanged"/>
                                    <asp:RadioButton ID="rbpersonwise" Text="Person Wise"  AutoPostBack="true" runat="server" GroupName="Target" OnCheckedChanged="rbpersonwise_CheckedChanged" />
                                    <%-- <asp:TextBox ID="txtcurrentyear" runat="server"></asp:TextBox>--%>
                                </div>
                            </div><br />
                            <div id="divforsp" class="col-md-12" style="padding-left:0px" runat="server" visible="false">
                                <div class="col-lg-3 col-md-4 col-sm-4 col-xs-9">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Role</label><br />
                                    <asp:DropDownList ID="ddlroleforsp" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <%-- <asp:TextBox ID="txtcurrentyear" runat="server"></asp:TextBox>--%>
                                </div>
                            </div>
                                <div class="col-lg-3 col-md-4 col-sm-4 col-xs-9">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">State</label><br />
                                 <asp:DropDownList ID="ddlstate" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlstate_SelectedIndexChanged"></asp:DropDownList>
                                    <%-- <asp:TextBox ID="txtcurrentyear" runat="server"></asp:TextBox>--%>
                                </div>
                            </div>
                             <div class="col-lg-3 col-md-4 col-sm-4 col-xs-9">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">City</label><br />
                                    <asp:DropDownList ID="ddlcity" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <%-- <asp:TextBox ID="txtcurrentyear" runat="server"></asp:TextBox>--%>
                                </div>
                            </div>
                                </div>
                            <div class="col-lg-3 col-md-4 col-sm-4 col-xs-9" >
                                <div class="form-group">
                                    <asp:Button ID="btngo" runat="server" CssClass="btn btn-primary" Text="Go" OnClick="btngo_Click"  OnClientClick="return valid();"/>
                                    <%-- <asp:TextBox ID="txtcurrentyear" runat="server"></asp:TextBox>--%>
                                </div>
                            </div>

                            <div class="clearfix"></div>
                            <div style="height:50px"></div>
                           
                                <div class="form-group">
                                    <div class="table table-responsive">
                                        <!--<label for="exampleInputEmail1"></label>-->

                                        <%--Gridview For targte abhishek  --%>
                                        <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="false"
                                              CssClass="grid" 
                                                ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White"  ShowHeader="true" EmptyDataText="No records Found" >
                                            <Columns>
                                                         <asp:TemplateField >
                                                    <HeaderTemplate>
                                                        Target For
                                             <asp:DropDownList ID="ddlname" runat="server" AutoPostBack="true" width="100px" Visible="false" AppendDataBoundItems="true" >
                                                  </asp:DropDownList>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lbltargetfor" CssClass="form-control" Font-Bold="true" Width="100%"  ForeColor="#3c8dbc"  Text= '<%# Eval("smname") %>'></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hidval" value=' <%# Eval("value") %>'/>
                                                         <%--<asp:HiddenField runat="server" ID="hidtochkvalidation" Value="1"/>--%>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField HeaderText="Apr"  >
                                                     <ItemTemplate>
                                                              <asp:TextBox ID="txtApr" runat="server" Text='<%# Eval("Apr") %>'  Width="80px" MaxLength="8" CssClass="form-control numeric text-right"  value="0.00" ToolTip="Enter Target For Apr"/>
                                                     </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="May"  >
                                                     <ItemTemplate>
                                                              <asp:TextBox ID="txtMay" runat="server" Text='<%# Eval("May") %>'  Width="80px"  MaxLength="8" CssClass="form-control numeric text-right" value="0.00" ToolTip="Enter Target For May"/>
                                                     </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Jun"  >
                                                     <ItemTemplate>
                                                              <asp:TextBox ID="txtJun" runat="server" Text='<%# Eval("Jun") %>'  Width="80px" MaxLength="8"  CssClass="form-control numeric text-right" value="0.00" ToolTip="Enter Target For Jun"/>
                                                     </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Jul">
                                                     <ItemTemplate>
                                                              <asp:TextBox ID="txtJul" runat="server" Text='<%# Eval("July") %>' Width="80px"  MaxLength="8" CssClass="form-control numeric text-right" value="0.00" ToolTip="Enter Target For Jul"/>
                                                     </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Aug"  >
                                                     <ItemTemplate>
                                                              <asp:TextBox ID="txtAug" runat="server" Text='<%# Eval("Aug") %>'  Width="80px" MaxLength="8"  CssClass="form-control numeric text-right" value="0.00" ToolTip="Enter Target For Aug"/>
                                                     </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sep" >
                                                     <ItemTemplate>
                                                              <asp:TextBox ID="txtSep" runat="server" Text='<%# Eval("Sep") %>'  Width="80px"  MaxLength="8" CssClass="form-control numeric text-right" value="0.00" ToolTip="Enter Target For Sep"/>
                                                     </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Oct"  >
                                                     <ItemTemplate>
                                                              <asp:TextBox ID="txtOct" runat="server" Text='<%# Eval("Oct") %>'  Width="80px"  MaxLength="8" CssClass="form-control numeric text-right" value="0.00" ToolTip="Enter Target For Oct"/>
                                                     </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nov"  >
                                                     <ItemTemplate>
                                                              <asp:TextBox ID="txtNov" runat="server" Text='<%# Eval("Nov") %>' Width="80px"  MaxLength="8" CssClass="form-control numeric text-right" value="0.00" ToolTip="Enter Target For Nov"/>
                                                     </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dec"  >
                                                     <ItemTemplate>
                                                              <asp:TextBox ID="txtDec" runat="server" Text='<%# Eval("Dec") %>' Width="80px"  MaxLength="8" CssClass="form-control numeric text-right" value="0.00" ToolTip="Enter Target For Dec"/>
                                                     </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Jan"  >
                                                     <ItemTemplate>
                                                              <asp:TextBox ID="txtJan" runat="server" Text='<%# Eval("Jan") %>'  Width="80px"  MaxLength="8" CssClass="form-control numeric text-right" value="0.00" ToolTip="Enter Target For Jan"/>
                                                     </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Feb"  >
                                                     <ItemTemplate>
                                                              <asp:TextBox ID="txtFeb" runat="server" Text='<%# Eval("Feb") %>'  Width="80px"  MaxLength="8" CssClass="form-control numeric text-right" value="0.00" ToolTip="Enter Target For Feb"/>
                                                     </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Mar"  >
                                                     <ItemTemplate>
                                                              <asp:TextBox ID="txtMar" runat="server" Text='<%# Eval("Mar") %>'   Width="80px" MaxLength="8" CssClass="form-control numeric text-right" value="0.00" ToolTip="Enter Target For Mar"/>
                                                         
                                                     </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <%--  --%>

                                     
                                    </div>
                                </div>

                               
                          



                        </div>
                        <div class="box-footer">
                            <asp:Button ID="btnsave" CssClass="btn btn-primary" Text="Save" Visible="false" OnClick="btnsave_Click"  runat="server" />
                            <asp:Button ID="Cancel" CssClass="btn btn-primary" Text="Cancel" runat="server" OnClick ="Cancel_Click" />
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
</asp:Content>
