<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="TargetReportCrm.aspx.cs" Inherits="AstralFFMS.TargetReportCrm" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <style type="text/css">
        .grid td{
 height:40px;padding: 6px;text-align:right;
}
        .grid th{
 
  height:40px;padding: 6px;
}
           #ContentPlaceHolder1_GridView4{
        border: 1px solid #3c8dbc;
    }

           #ContentPlaceHolder1_gridhalfyearly{
        border: 1px solid #3c8dbc;
    }
           #ContentPlaceHolder1_gridquarterly{
        border: 1px solid #3c8dbc;
    }
                   #ContentPlaceHolder1_GridDealReport{
        border: 1px solid #3c8dbc;
    }
           GridViewFooterStyle
{
    background-color: red;
    font-weight: bold;
    color: White;
}
            .aligngridcell {
                text-align:right;
            }
 .FixedHeader {
            /*position: absolute;
            font-weight: bold;*/
            position:relative ;
top:expression(this.offsetParent.scrollTop);
z-index: 10;
        } 
    </style>
    <script type="text/javascript">
        function check() {
            //  alert("hota h");

            var table = document.getElementById("ContentPlaceHolder1_GridView4");
            var rows = table.getElementsByTagName("tr");
            for (var z = 1; z < rows.length; z++) {
                for (var y = 0; y < rows[z].cells.length; y++) {
                    if (rows[z].cells[y].style.backgroundColor == "red") {

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
                errormessage("Please Select the Financial Year");
                return false;
            }
            if ($('#<%=ddlroleforsp.ClientID %>').val() == '0') {
                errormessage("Please Select Role");
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
            //if ($("input[name*=Target]:checked").length > 0)
            //    //  alert("checked");
            //{ }
            //else
            //{
            //    errormessage("Please select Target For");
            //    return false;
            //}

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
                                    <label for="exampleInputEmail1">Period</label><br />
                                    <asp:DropDownList ID="ddlperiod" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Monthly" Value="1" Selected="True"></asp:ListItem>

                                        <asp:ListItem Text="Quarterly" Value="2"></asp:ListItem>

                                        <asp:ListItem Text="Half Yearly" Value="3"></asp:ListItem>

                                       
                                    </asp:DropDownList>
                                    <%-- <asp:TextBox ID="txtcurrentyear" runat="server"></asp:TextBox>--%>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-4 col-sm-4 col-xs-9">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Role</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label><br />
                                    <asp:DropDownList ID="ddlroleforsp" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlroleforsp_SelectedIndexChanged"></asp:DropDownList>
                                    <%-- <asp:TextBox ID="txtcurrentyear" runat="server"></asp:TextBox>--%>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-4 col-sm-4 col-xs-9">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Salesperson</label><br />
                                    <asp:DropDownList ID="ddlsp" runat="server" CssClass="form-control"></asp:DropDownList>
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
                            
                            <div class="col-lg-3 col-md-4 col-sm-4 col-xs-9">
                                <div class="form-group" style="margin-top:17px">
                                    <asp:Button ID="btngo" runat="server" CssClass="btn btn-primary" Text="Go" OnClick="btngo_Click"  OnClientClick="return valid();"  />
                                    <%-- <asp:TextBox ID="txtcurrentyear" runat="server"></asp:TextBox> OnClick="btngo_Click" OnClientClick="return valid();"--%>
                                </div>
                            </div>

                            <div class="clearfix"></div>
                            <div style="height: 50px"></div>

                            <div class="form-group">
                                <div class="table table-responsive">
                                    <!--<label for="exampleInputEmail1"></label>-->

                                    <%--Gridview For targte abhishek  --%>
                                    <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="false"
                                        CssClass="grid" ItemStyle-HorizontalAlign="right" OnRowDataBound="GridView4_RowDataBound"
                                        ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" ShowHeader="true" EmptyDataText="No records Found">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbltargetfor" CssClass="form-control" Font-Bold="true" Width="100%" ForeColor="#3c8dbc" Text='<%# Eval("total") %>'></asp:Label>
                                            
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Apr" DataField="Apr"  />
                                             <asp:BoundField HeaderText="May" DataField="May" />
                                             <asp:BoundField HeaderText="Jun" DataField="Jun" />
                                             <asp:BoundField HeaderText="Jul" DataField="Jul" />
                                             <asp:BoundField HeaderText="Aug" DataField="Aug" />
                                             <asp:BoundField HeaderText="Sep" DataField="Sep" />
                                             <asp:BoundField HeaderText="Oct" DataField="Oct" />
                                             <asp:BoundField HeaderText="Nov" DataField="Nov" />
                                             <asp:BoundField HeaderText="Dec" DataField="Dec" />
                                             <asp:BoundField HeaderText="Jan" DataField="Jan" />
                                             <asp:BoundField HeaderText="Feb" DataField="Feb" />
                                             <asp:BoundField HeaderText="Mar" DataField="Mar" />
                                             
                                        </Columns>
                                      <RowStyle HorizontalAlign="Right" />
                                        
                                    </asp:GridView>
                                    <div style="height:15px"></div>
          <asp:GridView ID="gridquarterly" runat="server" AutoGenerateColumns="false"
                                        CssClass="grid" OnRowDataBound="gridquarterly_RowDataBound"
                                        ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" ShowHeader="true" EmptyDataText="No records Found">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbltargetfor" CssClass="form-control" Font-Bold="true" Width="100%" ForeColor="#3c8dbc" Text='<%# Eval("total") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:BoundField HeaderText="Quarter1" DataField="Q1"  />
                                             <asp:BoundField HeaderText="Quarter2" DataField="Q2" />
                                             <asp:BoundField HeaderText="Quarter3" DataField="Q3" />
                                             <asp:BoundField HeaderText="Quarter4" DataField="Q4" />
                                        </Columns>
                                    </asp:GridView>


                                   <div style="height:15px"></div>
                                      <asp:GridView ID="gridhalfyearly" runat="server" AutoGenerateColumns="false" OnRowDataBound="gridhalfyearly_RowDataBound"
                                        CssClass="grid"
                                        ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" ShowHeader="true" EmptyDataText="No records Found">
                                        <Columns>
                                      
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbltargetfor" CssClass="form-control" Font-Bold="true" Width="100%" ForeColor="#3c8dbc" Text='<%# Eval("total") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:BoundField HeaderText="Half Year 1" DataField="Q1"  />
                                             <asp:BoundField HeaderText="Half Year 2" DataField="Q2" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView ID="gridstatuswise" runat="server" AutoGenerateColumns="false"
                                        CssClass="grid" ItemStyle-HorizontalAlign="right" ShowFooter="true" OnRowDataBound="gridstatuswise_RowDataBound"
                                        ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" ShowHeader="true" EmptyDataText="No records Found">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbltargetfor" CssClass="form-control" Font-Bold="true" Width="100%" ForeColor="#3c8dbc" Text='<%# Eval("status") %>'></asp:Label>
                                            
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Apr" DataField="Apr"  />
                                             <asp:BoundField HeaderText="May" DataField="May" />
                                             <asp:BoundField HeaderText="Jun" DataField="Jun" />
                                             <asp:BoundField HeaderText="Jul" DataField="Jul" />
                                             <asp:BoundField HeaderText="Aug" DataField="Aug" />
                                             <asp:BoundField HeaderText="Sep" DataField="Sep" />
                                             <asp:BoundField HeaderText="Oct" DataField="Oct" />
                                             <asp:BoundField HeaderText="Nov" DataField="Nov" />
                                             <asp:BoundField HeaderText="Dec" DataField="Dec" />
                                             <asp:BoundField HeaderText="Jan" DataField="Jan" />
                                             <asp:BoundField HeaderText="Feb" DataField="Feb" />
                                             <asp:BoundField HeaderText="Mar" DataField="Mar" />
                                             
                                        </Columns>
                                     <footerstyle backcolor="#3c8dbc" Font-Bold="true"  HorizontalAlign="Right" Font-Size="18px"
          forecolor="White"/>
                                    </asp:GridView>
                                     <asp:GridView ID="gridstatusqarterly" runat="server" AutoGenerateColumns="false" OnRowDataBound="gridstatusqarterly_RowDataBound"
                                        CssClass="grid" ItemStyle-HorizontalAlign="right" ShowFooter="true"
                                        ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" ShowHeader="true" EmptyDataText="No records Found">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbltargetfor" CssClass="form-control" Font-Bold="true" Width="100%" ForeColor="#3c8dbc" Text='<%# Eval("status") %>'></asp:Label>
                                            
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Apr" DataField="Q1"  />
                                             <asp:BoundField HeaderText="May" DataField="Q2" />
                                             <asp:BoundField HeaderText="Jun" DataField="Q3" />
                                             <asp:BoundField HeaderText="Jul" DataField="Q4" />
                                           
                                       
                                             
                                        </Columns>
                                     <footerstyle backcolor="#3c8dbc" Font-Bold="true"  HorizontalAlign="Right" Font-Size="18px"
          forecolor="White"/>
                                    </asp:GridView>

                                         <asp:GridView ID="gridstatuswisehalfyearly" runat="server" AutoGenerateColumns="false"
                                        CssClass="grid" ItemStyle-HorizontalAlign="right" ShowFooter="true" OnRowDataBound="gridstatuswisehalfyearly_RowDataBound"
                                        ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" ShowHeader="true" EmptyDataText="No records Found">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbltargetfor" CssClass="form-control" Font-Bold="true" Width="100%" ForeColor="#3c8dbc" Text='<%# Eval("status") %>'></asp:Label>
                                            
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Apr" DataField="Q1"  />
                                             <asp:BoundField HeaderText="May" DataField="Q2" />
                                           
                                        </Columns>
                                     <footerstyle backcolor="#3c8dbc" Font-Bold="true"  HorizontalAlign="Right" Font-Size="18px"
          forecolor="White"/>
                                    </asp:GridView>
                                    <div style="height:25px"></div>
                                    <div  style="max-height:300px;overflow-y:scroll;">


                                      <asp:GridView ID="GridDealReport" runat="server" AutoGenerateColumns="false"  OnRowDataBound="GridDealReport_RowDataBound"
                                        CssClass="grid" ItemStyle-HorizontalAlign="right" AllowSorting="True" HeaderStyle-CssClass="FixedHeader"
                                        ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="#3c8dbc" Width="100%" HeaderStyle-ForeColor="White" ShowHeader="true" EmptyDataText="No records Found">
                                       
                                          <Columns>
                                       
                                           
                                             <asp:BoundField HeaderText="Contact" DataField="contact" />
                                             <asp:BoundField HeaderText="Owner" DataField="owner" />
                                             <asp:BoundField HeaderText="Amount" DataField="amount" />
                                             <asp:BoundField HeaderText="Date" DataField="Date" />
                                             <asp:BoundField HeaderText="Closed Date" DataField="closedDate" />
                                             <asp:BoundField HeaderText="Status" DataField="status" />
                                             
                                        </Columns>
                                   
                                    </asp:GridView></div>
                                    <%--  --%>
                                </div>
                            </div>






                        </div>
                        <div class="box-footer">
                          
                            <asp:Button ID="Cancel" CssClass="btn btn-primary" Text="Cancel" runat="server" OnClick="Cancel_Click"  />
                                <asp:Button ID="btnexcel" CssClass="btn btn-primary" Text="Export Pending Deals CSV" runat="server" OnClick="btnexcel_Click"   Visible="false" />
                            <%--OnClick="btnsave_Click"  OnClick="Cancel_Click"--%>
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
