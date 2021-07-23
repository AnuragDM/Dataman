<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ImportMeetUsers.aspx.cs" Inherits="FFMS.ImportMeetUsers" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
   <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <style>
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
    </style>
    <script type="text/javascript">
        //function pageLoad() {
        //    $(".select2").select2();
        //};
    </script>
    <script type="text/javascript">
        function validate() {

            if ($('#<%=ddlmeetType.ClientID%>').val() == '0') {
                 errormessage("Please select the Meet Type");
                 return false;
             }
            <%-- if ($('#<%=ddlMeet.ClientID%>').val() == '0') {
                 errormessage("Please select the Meet Name");
                 return false;
             }--%>
         }

    </script>
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
        <script type="text/javascript">
            $(function () {
                $('[id*=ddlPartyType]').multiselect({
                    enableCaseInsensitiveFiltering: true,
                    //buttonWidth: '200px',
                    buttonWidth: '100%',
                    includeSelectAllOption: true,
                    maxHeight: 200,
                    width: 215,
                    enableFiltering: true,
                    filterPlaceholder: 'Search'
                });
            });
            ////////function test() {
            ////////    $('[id*=ddlPartyType]').multiselect({
            ////////        enableCaseInsensitiveFiltering: true,
            ////////        //buttonWidth: '200px',
            ////////        buttonWidth: '100%',
            ////////        includeSelectAllOption: true,
            ////////        maxHeight: 200,
            ////////        width: 215,
            ////////        enableFiltering: true,
            ////////        filterPlaceholder: 'Search'
            ////////    });
            ////////}
    </script>
    <style type="text/css">
        .input-group .form-control {
            height: 34px;
        }

        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }

        .select2-container {
            /*display: table;*/
        }

        .multiselect-container > li > a {
            white-space: normal;
        }

        .multiselect-container > li {
            width: 100%;
        }

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }

        .spinner {
        position: absolute;
        top: 50%;
        left: 50%;
        margin-left: -50px; /* half width of the spinner gif */
        margin-top: -50px; /* half height of the spinner gif */
        text-align: center;
        z-index: 999;
        overflow: auto;
        width: 100px; /* width of the spinner gif */
        height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
    }
    </style>
    <section class="content">
        <script type="text/javascript">
            function load1() {

                $(".numeric").numeric({ negative: false });
            }

            $(window).load(function () {

                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(load1);

            });

            function LoadProductBatchByName() 
            {
                debugger;
                var name = document.getElementById("ContentPlaceHolder1_lastDiv");
                var name1 = document.getElementById("ContentPlaceHolder1_lblm");
                var values = "";
                var City = document.getElementById("ddlCity");
                var Beat = document.getElementById("ddlBeat");
                var PartyType = document.getElementById("ContentPlaceHolder1_ddlPartyType");
                var cityname = $('#ContentPlaceHolder1_ddlCity').find('option:selected').text();
                var beatname = $('#ContentPlaceHolder1_ddlBeat').find('option:selected').text();
                var na = 'Note(3)  City' + cityname + ',Beat' + beatname;
                var listLength = PartyType.options.length;

                for (var i = 0; i < PartyType.options.length; i++) {
                    if (PartyType.options[i].selected) {
                        values += PartyType.options[i].innerHTML + " ," ;
                    }
                }
                               
                $("#ContentPlaceHolder1_lastDiv").html("Note(3): User Can Download Meet User List  That Are Related To  City - " + cityname + ", Beat -" + beatname + ", Party Type - " + values)
                }
        </script>
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
                            <h3 class="box-title">Meet Import/Export User List</h3>
                            <div style="float: right">
                                <%--    <asp:Button Style="margin-right: 5px;" type="button" ID="btnFind" runat="server" Text="Find" class="btn btn-primary"
                                OnClick="btnFind_Click" />--%>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <!-- form start -->

                        <div class="box-body">
                            <div class="col-lg-9 col-md-11 col-sm-10 col-xs-10" style="border: 2px solid #A2A2A0  !important">
                                <%--   <div class="form-group col-md-5 paddingleft0">
                                        <label for="exampleInputEmail1">User:</label>&nbsp;&nbsp;<label for="requiredFields" style="color:red;">*</label>
                                        <asp:DropDownList ID="ddlunderUser"  runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>--%>

                               <%-- <asp:UpdatePanel ID="up" runat="server">
                                    <ContentTemplate>--%>

                                        <div class="form-group col-md-4 paddingleft0">
                                            <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlunderUser" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>

                                        <div class="form-group col-md-4 paddingright0">
                                            <label for="exampleInputEmail1">Meet Type:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlmeetType" AutoPostBack="true" OnSelectedIndexChanged="ddlmeetType_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-4 paddingright0">
                                            <label for="exampleInputEmail1">Meet Name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                            <asp:DropDownList ID="ddlMeet" runat="server" OnSelectedIndexChanged="ddlMeet_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                        </div>
                             </div>
                             <div class="col-lg-9 col-md-11 col-sm-10 col-xs-10" style="border: 2px solid #A2A2A0  !important; margin-top:5px;">
                                  <div class="form-group col-md-4 paddingleft0">
                                       <label for="exampleInputEmail1">Download:</label>
                                      </div>
                                <div class="clearfix"></div>
                                        <div class="form-group col-md-4 paddingleft0">
                                              
                                            <label for="exampleInputEmail1">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>&nbsp;&nbsp;<%--<label for="requiredFields" style="color: red;">*</label>--%>
                                            <asp:DropDownList ID="ddlCity" runat="server" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged"  AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                         <div class="form-group col-md-4 paddingright0">
                                            <label for="exampleInputEmail1">Beat Name:</label>&nbsp;&nbsp;<%--<label for="requiredFields" style="color: red;">*</label>--%>
                                            <asp:DropDownList ID="ddlBeat" runat="server" OnSelectedIndexChanged="ddlBeat_SelectedIndexChanged"  onchange="return LoadProductBatchByName();" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                         <div class="form-group col-md-4  paddingright0">
                                            <label for="withSales">Party Type:</label>
                                            <br />
                                            <asp:ListBox ID="ddlPartyType" runat="server" OnSelectedIndexChanged="ddlPartyType_SelectedIndexChanged" onchange="return LoadProductBatchByName();"   Width="100%" SelectionMode="Multiple"></asp:ListBox>  
                                          
                                        </div>

<%--                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>

                                  <div class="form-group col-md-4 paddingright0">
                                       <asp:LinkButton ID="btnLink" runat="server" Text="Download" OnClick="lnkdownload_Click" OnClientClick="javascript:return validate();"  ></asp:LinkButton>
                                      </div>
                                  </div>
                              <div class="col-lg-9 col-md-11 col-sm-10 col-xs-10" style="border: 2px solid #A2A2A0  !important; margin-top:5px;">
                                <div class="form-group col-md-4 paddingright0">
                                    <label for="exampleInputEmail1">Upload:</label>
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                </div>
                                  </div>
                          
                            <div class="clearfix"></div>
                            <div class="box-footer">
                                <asp:Button ID="btnsave" OnClientClick="return validate();" CssClass="btn btn-primary" runat="server" Text="Upload" OnClick="btnsave_Click" />
                                <asp:Button ID="Cancel" CssClass="btn btn-primary" runat="server" Text="Cancel" OnClick="Cancel_Click" />
                                
                               <%-- <asp:HyperLink ID="hpl" runat="server" Text="Download Sample" NavigateUrl="~/FileUploads/MeetUserList.xls"></asp:HyperLink>--%>

                            </div>
                            <br />
                            
                           <asp:Label ID="lblm" runat="server"></asp:Label>
                                <div><b>Note(1) : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b></div>
                            <br />
                            <div>
                                Note(2) : If you don't have mobile number, enter 9999999999
                                <br />
                               
                            </div>
                            <div id="lastDiv"  runat="server">
                              
                              
                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </section>
    
</asp:Content>


