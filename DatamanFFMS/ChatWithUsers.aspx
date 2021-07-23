<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="ChatWithUsers.aspx.cs" Inherits="AstralFFMS.ChatWithUsers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content">
        <script src="plugins/slimScroll/jquery.slimscroll.min.js"></script>
        <!-- Construct the box with style you want. Here we are using box-danger -->
        <!-- Then add the class direct-chat and choose the direct-chat-* contexual class -->
        <!-- The contextual class should match the box, so we are using direct-chat-danger -->
        <div class="box box-blue direct-chat direct-chat-danger" style="height: 100%">

            <!-- /.box-header -->
            <div class="box-body">
                <!-- ds-->
                <div>
                    <div class="mob-view collapse in" id="chat-mob" style="float: left; width: 24%; padding: 1.2rem;border-right: 1px solid #eee;background-color: #03a9f4 !important;">
                      <div class="search">
				        <i class="fa fa-search pull-left search-style" aria-hidden="true"></i>
				        <div class="margin-left2em">
				        <asp:TextBox runat="server" class="form-control search-input" ID="txtsearchcontacts" AutoPostBack="true" Font-Bold="true" Font-Size="15px" placeholder="Search Contacts.." BorderStyle="None" OnTextChanged="txtsearchcontacts_TextChanged"></asp:TextBox>
				        </div>
				      </div>
                       
                        <%--<ajaxtoolkit:autocompleteextender id="txtSearch_AutoCompleteExtender" runat="server" completionlistcssclass="completionList"
                                             completionlistitemcssclass="listItem" completionlisthighlighteditemcssclass="itemHighlighted" completioninterval="0"
                                             behaviorid="txtSearch_AutoCompleteExtender" firstrowselected="false" onclientitemselected="ClientItemSelected"
                                             delimitercharacters="" servicemethod="SearchContacts" servicepath="~/ChatWithUsers.aspx" minimumprefixlength="3" enablecaching="true"
                                             targetcontrolid="txtsearchcontacts">
                                           </ajaxtoolkit:autocompleteextender>--%>
                        <u style="text-decoration-color: red;"></u>
                        <div style="height: 20px"></div>
                        <%--<asp:Timer ID="Timer2" runat="server" OnTick="Timer2_Tick" Interval="5000" />--%>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" style="height: 441px; overflow-y: scroll; overflow-x: hidden;">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                            </Triggers>
                            <ContentTemplate>

                                <asp:GridView runat="server" ID="gridcontacts" AutoGenerateColumns="false" ShowHeader="false" BorderStyle="None" Width="100%" OnRowCommand="grd_RowCommand">
                                    <Columns>
                                        <%--<img  src="../dist/img/user1-128x128.jpg" alt="Contact Avatar" height="39px" width="39px" style="border-radius: 50%;">--%>

                                        <asp:TemplateField>
                                            <ItemStyle BorderStyle="None" Height="48px" />
                                            <ItemTemplate>
                                                <img src="../dist/img/user1-128x128.jpg" alt="Contact Avatar" height="39px" width="39px" style="border-radius: 50%;">
                                                <asp:LinkButton runat="server" ID="linksmid" Text='<%#Eval("salesperson") %>' ForeColor="#ffffff" style="font-weight:bold;padding-left:9px;" CommandName="Click" CommandArgument='<%#Eval("smid") %>'></asp:LinkButton>

                                                <div class="box-tools pull-right <%#Eval("mszunreaddivClass") %>">
                                                    <span data-toggle="tooltip" title="<%#Eval("unreadmsz") %> New Messages From <%#Eval("salesperson") %>" class="badge bg-red"><%#Eval("unreadmsz") %></span>

                                                </div>
                                                <asp:HiddenField ID="hidsearchcontactsmid" runat="server" Value='<%#Eval("smid") %>' />
                                                <!--<hr style="color: #ccc;">-->
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                    </Columns>
                                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="#800000" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                
                       
                    <%--style="float:left;width:74%;height:530px;overflow-y:scroll;overflow-x: hidden;position:relative"--%>
                    <%--         <div id="divmszmain" style="float: left; width: 74%; height: 475px; overflow-y: scroll; overflow-x: hidden;">--%>
                    <div id="divmszmain" runat="server" style="float: left;width: 74%;height: auto;">
                        <%--style="width:400px;height:400px;overflow-x:hidden;overflow:-y:scroll;--%>

                        <%--<asp:UpdatePanel ID="StockPricePanel" runat="server" UpdateMode="Conditional" style="height: 441px;overflow: auto;">--%>
                        <asp:UpdatePanel ID="StockPricePanel" runat="server" UpdateMode="Conditional" style="height: 441px;overflow-y:hidden;">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                            </Triggers>
                            <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
                              <ContentTemplate>
                        <div class="top-login-style">
                             <img src="../dist/img/user1-128x128.jpg" alt="Contact Avatar" height="39px" width="39px">
                             <h3 class="box-title">
                                  <asp:Label runat="server" ID="LblTo"></asp:Label>
                                 <a href="#chat-mob" class="btn btn-info pull-right visible-sm visible-xs" data-toggle="collapse" style=""><i class="fa fa-bars" aria-hidden="true"></i></a>
                             </h3>
                             
                        </div></ContentTemplate>
                              </asp:UpdatePanel>
                                <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="1000" ClientIDMode="AutoID" />
                                <%--Stock price is
                                <asp:Label ID="StockPrice" runat="server"></asp:Label><br />
                                as of
                                <asp:Label ID="TimeOfPrice" runat="server"></asp:Label>--%>
                               
                                <asp:Label runat="server" ID="tomszname" Visible="false"></asp:Label>
                                <div id="chat-box" style="overflow-y:scroll;height:366px;">
                                <!-- Conversations are loaded here -->
                                <asp:Repeater runat="server" ID="rpt" OnItemDataBound="rpt_ItemDataBound" >
                                    <ItemTemplate>
                                        <div class="direct-chat-messages">
                                            <!-- Message. Default to the left -->
                                            <div class="direct-chat-msg <%#Eval("dateDirection") %>">
                                                <div class="direct-chat-info clearfix">
                                                    <!--<span class="direct-chat-name <%#Eval("mszDirection") %>"><%--pull-left--%>
                                                        <%#Eval("fsmname") %></span>-->
                                                    <span class="direct-chat-timestamp <%#Eval("dateDirection") %>"><%#Eval("createddate") %></span><%--pull-right--%>
                                                </div>
                                                <!-- /.direct-chat-info -->
                                                <img class="direct-chat-img <%#Eval("dateDirection") %>" src="<%#Eval("ImgUrl") %>" alt="message user image"><!-- /.direct-chat-img -->
                                               <%-- <div class="direct-chat-text <%#Eval("MszBackColor") %> <%#Eval("mszDirection") %> <%#Eval("focusmsz") %>">
                                                    
                                                    <%#Eval("msz") %>
                                                </div>--%>
                                                <div class="direct-chat-text <%#Eval("MszBackColor") %>  <%#Eval("mszDirection") %> <%#Eval("focusmsz") %>  <%#Eval("MszLinkVisible") %>">
                                                 
                                                    <%#Eval("msz") %>
                                                </div>
                                                 <div  class="direct-chat-text <%#Eval("MszBackColor") %> <%#Eval("mszDirection") %> <%#Eval("focusmsz") %> <%#Eval("LinkVisible") %>">

                                                    <asp:UpdatePanel  runat="server" ID="up2" UpdateMode="Conditional">
                                                        <Triggers>
                                                             <asp:PostBackTrigger ControlID="LinkButton1"  />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                             <asp:LinkButton ID="LinkButton1" runat="server" Text="Download File"  ForeColor="White" Font-Underline="true"
                                                        CommandArgument='<%#Eval("msz") %>' CommandName="Click" OnCommand="LinkButton1_Command" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                   
                                                </div>
                                                <!-- /.direct-chat-text -->
                                            </div>
                                            <!-- /.direct-chat-msg -->
                                        </div>
                                        <!--/.direct-chat-messages-->
                                        <div class="clearfix"></div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="box-footer" style="position: relative; bottom: 0px;padding:1.2rem;">
                                    <div class="input-group">
                                        <input type="text" name="message" id="txtmsz" runat="server" cssclass="form-control" placeholder="Type Message Here ..." class="form-control width98" >
                                        <span class="input-group-btn" style="background-color: #03a9f4;">
                                            <asp:ImageButton runat="server" ID="btnimgsend" ImageUrl="~/img/sent-mail.png"  CssClass="btn" OnClick="btnimgsend_Click" ToolTip="Send Message" Style="border-radius: 50%; padding-left: 8px" />
                                        </span>
                                        <span class="image-upload">
                                            <label for="upload1">
                                                <img src="/img/clip.png" style="width: 22px;cursor:pointer;" title="Send File" />
                                            </label>
                                       <%--     <input id="file-input" type="file"/>--%>
                                              <asp:FileUpload ID="upload1" runat="server" ClientIDMode="Static" onchange="this.form.submit()" />
                                        </span>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                       
                    <!-- Contacts are loaded here -->
                    <!-- /.direct-chat-pane -->
                </div>

            </div>
            <!-- /.box-body -->

            <!-- /.box-footer-->
        </div>
        <!--/.direct-chat -->

    </section>
    <script runat="server">
       
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            //StockPrice.Text = GetStockPrice();
            //TimeOfPrice.Text = DateTime.Now.ToLongTimeString();
            if (!string.IsNullOrEmpty(Convert.ToString(Session["Tosmid"])))
            {
                string s = Convert.ToString(Session["Tosmid"]);
                FillChat(Session["Tosmid"].ToString());
            }
          
            FillContacts(txtsearchcontacts.Text);
        }
    </script>

    <script type="text/javascript" src="js/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="js/chat.js"> </script>
    <script type="text/javascript">
        function validate() {
            if ($('#<%=txtmsz.ClientID%>').val() == "") {
                errormessage("Please Enter Message.");
                return false;
            }
        }
        $(document).ready(function () {
        $(function () {
            $('#chat-box').slimScroll({
                height: '380px'
            });
        });
        $(function () {
            $('#ContentPlaceHolder1_UpdatePanel1').slimScroll({
                height: '425px'
            });
        });
        });
        $(document).ready(function () {
            if ($(window).width() <= 768) {
                $("#chat-mob").removeClass("in");
            }
        });
        $(document).ready(function () {
            // alert("1243");
            ScrollToBottom();
           // EnbleDisableSend();
        });

        function ScrollToBottom() {
            debugger;
            // alert("1243");
            var d = document.getElementById("chat-box");
            var y = d.scrollHeight;
            d.scrollTop = y;
            //alert(y);
        }
      
    </script>
   
    <style>
        hr {
            display: normal;

            margin-top: 0.5em;
            margin-bottom: 0.5em;
            margin-left: auto;
            margin-right: auto;
            border-style: inset;
            border-width: 1px;
        }
    </style>
    
</asp:Content>
