<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ViewAllMessages.aspx.cs" Inherits="AstralFFMS.ViewAllMessages" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .messagelabel {
            /*background-color: #D0D0D0;*/
             background-color: #c7c5c6;
        }
         .messagelabelNew {
               
                color: black;                
                display: block;
                margin-bottom: -1px;
                padding: 10px 15px;
                position: relative;
                background-color: #c7c5c6;
            }
            .messagelabelNew1 input[type="submit"]{

                border-radius: 4px !important;
            }
            .messagelabelNew input[type="submit"]{

                border-radius: 4px !important;
            }
            @media (max-width: 400px) {
                .messagelabelNew1 input[type="submit"]{
                    margin-top: 6px !important;
                border-radius: 4px !important;
                margin-right: 5px !important;
                        }
            
            }
            @media (max-width: 400px) {
                .messagelabelNew input[type="submit"]{
                    margin-top: 6px !important;
                border-radius: 4px !important;
                margin-right: 5px !important;
                        }
            
            }

            .messagelabelNew1 {
                background-color: #ffffff;
                color: black;
               
                display: block;
                margin-bottom: -1px;
                padding: 10px 15px;
                position: relative;
            }
    </style>
    <section class="content">
        <script type="text/javascript">
            function callMyFunction1(val) {
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("Home.aspx/UpdateTransNotification") %>',
                    contentType: "application/json; charset=utf-8",
                    data: '{notID: "' + val + '"}',
                    dataType: "json",
                    success: function (savingStatus) {
                    }
                });
            }
        </script>
        <style type="text/css">
           
        </style>
        <div class="row">
            <!-- left column -->
            <div class="col-md-12">
                <div id="InputWork">
                    <!-- general form elements -->
                    <div class="box box-primary">
                        <div class="box-header with-border">
                            <h3 class="box-title">Notifications</h3>
                        </div>
                        <div class="box-body">
                            <div class="col-md-10 col-sm-10 col-xs-10">
                                <div class="form-group">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                                <ItemTemplate>
                                                    <ul class="list-group">
                                                        <li class="<%#setClassNew(Convert.ToInt32(Eval("Status")))%>">
                                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("NotiId") %>' />
                                                            <asp:Button ID="Button1" runat="server" CommandName="DeleteNot" Text="x" Style="float: left;" />
                                                            <%--     <button type="submit" class="close" data-dismiss="alert" aria-hidden="true" onclick='<%# "deleteNotification(" +Eval("NotiId") + " );" %>'>&times;</button>--%>
                                                            <a style="color: black; margin-left: 2%; margin-right:40px;" href="<%# String.Format("/"+ Eval("msgURL")+"&PageV="+"VIEWMSG") %>" onclick='<%# "callMyFunction1(" +Eval("NotiId") + " );" %>'></i><%# Eval("displayTitle") + " "%>
                                                            </a>
                                                            <%--<asp:Label ID="Label1" runat="server" Style="float: right;" Text='<%#(Eval("V1Date","{0:dd/MMM/yyyy}")==System.DateTime.Now.ToString("dd/MMM/yyyy") ? " ":Eval("V1Date","{0:M}"))+" "+Eval("V1Time") %>'></asp:Label>
                                                      --%>  
                                                              <asp:Label ID="Label1" runat="server" Style="float: right;" Text='<%#Eval("V1Date","{0:dd/MMM/yyyy}")+" "+Eval("V1Time") %>'></asp:Label>
                                                        </li>
                                                    </ul>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </section>

</asp:Content>
