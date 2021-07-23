<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="CRMAllContacts.aspx.cs" Inherits="AstralFFMS.CRMAllContacts" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />
    <style type="text/css">
        .spinner {
            position: absolute;
            left: 50%;
            margin-left: -50px; /* half width of the spinner gif */
            margin-top: -50px; /* half height of the spinner gif */
            text-align: center;
            z-index: 999;
            overflow: auto;
            width: 100px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }

        #select2-ContentPlaceHolder1_ddlParentLoc-container {
            margin-top: -8px !important;
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
        <div id="divData" runat="server">
            <div class="box-body" id="rptmain" runat="server">
                <div class="row">
                    <div class="col-md-12">

                        <div class="box">
                            <div class="box-header">
                                <h3 class="box-title">All Contacts</h3>
                            </div>
                               <div class="clearfix"></div>
                         </div>
                     
                        <!-- /.box-header --> 
                        <div class="box-body" id="Div1" runat="server" >
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-body table-responsive">
                            <asp:Repeater ID="rpt" runat="server">
                                <HeaderTemplate>
                                    <table id="examplenewdt" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th style="display:none;">ID</th>
                                                <th>Name </th>
                                                <th>Company</th>
                                                <th>Phone</th>
                                                <th>Email</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                      
                                        <td style="display:none;"><%#Eval("Contact_Id") %></td>
                                         <td >
                                            <asp:HyperLink runat="server" Target="_blank" ID="hplcont"
                                                NavigateUrl='<%# String.Format("CRMTask.aspx?Contact_Id={0}", Eval("Contact_Id")) %>'
                                                Text='<%#Eval("Name") %>'  /></td>
                                     
                                        <td><%#Eval("Compname") %></td>
                                         <td><%#Eval("Mobile") %></td>
                                        <td> <%#Eval("Email") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>     </table>       
                                </FooterTemplate>

                            </asp:Repeater>
                        </div>
                    </div>
                    </div></div></div>

                </div>
            </div>
        </div>
           </div>



    </section>

    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(function () {
            $("#examplenewdt").DataTable({
                "order": [[1, "asc"]]
            });

        });
    </script>
</asp:Content>
