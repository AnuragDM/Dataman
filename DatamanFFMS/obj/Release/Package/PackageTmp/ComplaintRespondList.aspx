<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ComplaintRespondList.aspx.cs" Inherits="AstralFFMS.ComplaintRespondList" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style type="text/css">
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .Bdr {
            border:solid;border-color:#367FA9; height:309px;
        }
         .Bdr1 {
            border:solid;border-color:#367FA9; height:100px;
        }
         .Bdr2{border:solid;border-color:#367FA9; margin-bottom:10px;}
         .Frght{
             float:left;
             font-weight:bold;
         }
    </style>
 
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
    <script type="text/javascript">
        function validate() {
            if ($('#<%=txtremarks.ClientID%>').val() == "") {
              errormessage("Please enter Remarks");
              return false;
          }
        }
    </script>
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


        <div class="box-body" id="rptmain" runat="server">
                   <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <div class="col-md-7">
                                <h3 class="box-title"><asp:Label id="lblheading" runat="server"></asp:Label></h3>
                            </div>
                            <div class="col-md-5">
                            <span class="Frght">
                            <asp:Label id="lblDoctype" runat="server"  Font-Bold="true" ></asp:Label>:
                            <asp:Label runat="server" ID="lblCSID" Font-Bold="true" CssClass="right"></asp:Label>
                                </span>
                                </div>
                        </div>
                          <div class="box-header">
                            <div class="col-md-7"></div>
                            <div class="col-md-5">
                                 <span class="Frght">
                                    Current Status: <asp:Label ID="lblstatus" runat="server" Font-Bold="true"></asp:Label>
                                 </span>
                                </div>
                          </div>
<%--                        <div class="box-body">
                            
                        </div>--%>
                        <!-- /.box-header -->
                        
                          <div class="box-body">

                              <div class="col-md-12">
                       
                        
                                <div class="Bdr2 table-responsive">
                                    <table class="table table-bordered table-striped">
                                     
                                        <tr>
                                            <td>
                                                Date: 
                                            </td>
                                            <td>
                                                <label id="lbldate" runat="server"></label>
                                            </td>
                                            <td>Product: </td>
                                            <td><label id="lblproduct" runat="server"></label></td>
                                            </tr>
                                        <tr id="span_sm" runat="server">                                        

                                            <td>  
                                                SalesPerson:   
                                               
                                            </td><td><span> <label id="lblsm" runat="server"></label></td>
                                              <td colspan="2">&nbsp;
                                              </span></td>
                                           
                                        </tr>
                                         <tr>
                                            <td>
                                                Nature: 
                                            </td>
                                             <td>
                                                <label id="lblnature" runat="server"></label>&nbsp;
                                               </td> 
                                                <td>  Department: </td>
                                             <td><label id="lbldept" runat="server"></label>&nbsp;</td>                       
                                            
                                        </tr>
                                         <tr>
                                            <td>
                                               Party Type:
                                                
                                            </td>
                                             <td>
                                                  <label id="lblpartytype" runat="server"></label>                            
                                              
                                            </td>
                                              <td>  Party Name: </td><td>   <label id="lbldistributor" runat="server"></label>&nbsp;</td>
                                        </tr>
                                         <tr id="trc" runat="server" style="display:none;">
                                            <td> 
                                                Mfd.Date: 
                                                </td>
                                             <td>
                                                <label id="lblmfddate" runat="server"></label>&nbsp;</td>
                                             <td>
                                                Batch No:
                                                 </td>
                                             <td>
                                                  <label id="lblbatchno" runat="server"></label>&nbsp;</td>
                                             </tr>
                                        <tr id="trc1" runat="server" style="display:none;">
                                             <td colspan="2">
                                                Remark:  
                                                 </td>
                                            <td colspan="2">
                                                  <asp:TextBox TextMode="MultiLine" Height="80px" Width="250px" CssClass="text" ID="txtcomplaint" runat="server" Enabled="true" Wrap="true"></asp:TextBox></td>
                                      
                                        </tr>

                                        <tr id="trs" runat="server" style="display:none;">
                                            <td colspan="2">
                                            New Application Area:
                                                </td>
                                            <td colspan="2">
                                            <asp:TextBox TextMode="MultiLine" Height="50px" Width="250px" CssClass="text" ID="txtNAR" runat="server" Enabled="true" Wrap="true"></asp:TextBox>
                                                 
                                            </td>
                                   

                                            </tr>
                                        <tr id="trs1" runat="server" style="display:none;">
                                              <td colspan="2">
                                           Technical Advantage:
                                                  </td>
                                            <td colspan="2">
                                            <asp:TextBox TextMode="MultiLine" Height="50px" Width="250px" CssClass="text" ID="txtTA" runat="server" Enabled="true" Wrap="true"></asp:TextBox>
                                       </td>
                                 

                                            </tr>
                                         <tr id="trs2" runat="server" style="display:none;">
                                            <td colspan="2">
                                              
                                                Make Product Better: 
                                            </td>
                                             <td colspan="2">
                                              <asp:TextBox TextMode="MultiLine" Height="50px" Width="250px" CssClass="text" ID="txtMPB" runat="server" Enabled="true" Wrap="true"></asp:TextBox></td>
                              

                                            </tr>
                                           <tr>
                                           
                                            <td id="tdimg" runat="server">  <asp:LinkButton ID="lnkViewDemoImg" Text="View Image" runat="server" OnClick="lnkViewDemoImg_Click" ></asp:LinkButton> <asp:HiddenField ID="hdvImg" runat="server" /></td>
                                                <td></td>

                                        </tr>
                                    </table>
                           
                              
                                 </div>
                        
                        </div>
                              <div class="clearfix"></div>

                        <div class="col-md-12">
                       
                            
                          
                        </div>
                              
                                       
                                    
                           

                        <div class="">
                            <div class="col-md-6">
                                <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server" >
                                <HeaderTemplate>
                                    <table id="example1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Respond Date</th>
                                       <%--         <th>Complaint No.</th>--%>
                                                <th>Responded By</th>
                                                <th>Remark</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%#String.Format("{0:dd/MMM/yyyy}", Eval("RespondDate"))%></td>
                                      <%--   <td><%#Eval("CompDocID") %></td>--%>
                                        <td><%#Eval("Empname") %></td>
                                        <td><%#Eval("Remarks") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                            </div>
                            <div class="col-md-6" >
                                <div class="Bdr">
                                    <table class="table table-bordered table-striped">
                                           <tr>
                                            <td>  <asp:Label runat="server"  Font-Bold="true" Text="Action"></asp:Label> </td>
                                        </tr>
                                        <tr>
                                            <td> <asp:Label runat="server"  Text="Remarks"></asp:Label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label></td>
                                        </tr>
                                         <tr>
                                            <td>  <asp:TextBox TextMode="MultiLine"  Width="100%" Rows="5" CssClass="text" ID="txtremarks" runat="server" Wrap="true"></asp:TextBox></td>
                                        </tr>
                                      
                                         <tr>
                                            <td>  <asp:RadioButtonList ID="RdbStatus" RepeatDirection="Horizontal" runat="server">
                                                    <%--<asp:ListItem Selected="True" Value="P" Text="Pending"></asp:ListItem>--%>
                                                    <asp:ListItem Selected="True" Value="W" Text="WIP"></asp:ListItem>
                                                    <asp:ListItem Value="R" Text="Resolved"></asp:ListItem>                                                   
                                                  </asp:RadioButtonList> </td>
                                        </tr>
                                        <tr><td>
                                             
                                        <asp:Button type="button" ID="btnSave" OnClientClick="javascript:return validate();" runat="server"  OnClick="btnSave_Click" Text="Save" class="btn btn-primary"  />
                                        <asp:LinkButton ID ="lnkcancel" PostBackUrl="~/ComplainRespond.aspx" runat="server" Text="Cancel" class="btn btn-primary"></asp:LinkButton></td>
                                
                                        </tr>
                                    </table>
                           
                                 </div>
                                 </div>
                     
                            </div>
                        </div>
                                
                        <br />
                      <%--  <div class="col-md-3 col-sm-6 col-xs-12">
                                  
                                </div>--%>
                        <br/><br />
                         <br />
                                <div>
                                    <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                                </div>   <!-- /.box-body -->
                    </div>
                        
                    <!-- /.box -->

                </div> 
                <!-- /.col -->
            </div>
                                   <%--     </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
           
    </section>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>

     <script type="text/javascript">
         $(function () {
             $("#example1").DataTable({
                 "order": [[0, "desc"]],
                 "bFilter": false,
                 "bPaginate": false,
                 "bSort": false
             });
            
         });
    </script>


</asp:Content>
