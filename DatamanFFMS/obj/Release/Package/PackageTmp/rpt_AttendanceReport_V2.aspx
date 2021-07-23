<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="rpt_AttendanceReport_V2.aspx.cs" Inherits="AstralFFMS.rpt_AttendanceReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>

    <script type="text/javascript">
        $(function () {
            $('[id*=ListBox1]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '200px',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
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
                             $(this).removeAttr("checked");
                         }
                     });
                 } else {
                     //Is Child CheckBox
                     var parentDIV = $(this).closest("DIV");
                     if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                         $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                     } else {
                         $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                     }
                 }
             });
         })
    </script>
    <style type="text/css">
        .multiselect-container > li > a {
            white-space: normal;
        }

        .input-group .form-control {
            height: 34px;
        }

        input[type=checkbox], input[type=radio] {
    margin-right: 12px !important;
    margin-left: 12px !important;
}
         .button1 {
box-shadow: 0px 2px 4px 2px #888888;
margin-left: 10px;
}
         h2 {
                 font-size: 20px !important;
    font-weight: 600 !important;
    margin-left: 13px !important;
         }
    </style>
    
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
        <div class="box-body" id="mainDiv" runat="server">
            <div class="row">
                <!-- left column -->
                <div class="col-md-12">
                    <div id="InputWork">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                   <img id="img-Header" src="img/Attendence.png" style="width: 50px;height: 50px;"/>
                               <%-- <h3 class="box-title">Attendance Report</h3>--%>
                                <h2 class="box-title"><asp:Label ID="lblPageHeader" runat="server" ></asp:Label></h2>    
                            </div>
                            <!-- /.box-header -->
                            <!-- form start -->
                            <div class="box-body" id="div1">
                               <div class="row">                                   
                                     <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Sales Person:</label>                                           
                                        <b>   <asp:TreeView ID="trview" runat="server" CssClass="table-responsive" ExpandDepth="10" ShowCheckBoxes="All"></asp:TreeView></b> 
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Month:</label>
                                            <asp:DropDownList ID="monthDDL" class="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                     <div class="col-md-4 col-sm-4 col-xs-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Year:</label>
                                            <asp:DropDownList ID="yearDDL" class="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="box-footer">
                          
                                 <asp:Button Style="margin-right: 5px;" type="button" ID="btnGo" runat="server" Text="Generate" class="btn btn-primary button1"
                                    OnClick="btnGo_Click"/>
                                <asp:Button Style="margin-right: 5px;" type="button" ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary button1"
                                    OnClick="btnCancel_Click" />
                             
                            </div>

                           
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
