<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master"  AutoEventWireup="true" Inherits="timedistance" Codebehind="timedistance.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%@ Register Src="ctlCalendar.ascx" TagName="Calendar" TagPrefix="ctl" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
     <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>
    <style type="text/css">
  .btnhide{
      display:none;
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
          
        @media (max-width: 600px) {
            .formlay {
                width: 100% !important;
            }
        }
        .select2-container--default .select2-selection--single, .select2-selection .select2-selection--single {
            padding: 3px 12px;
        }
         .multiselect-container.dropdown-menu {
            width: 100% !important;
        }
    </style>
     
    <script type="text/javascript">
        function pageLoad() {
           
            $('[id*=ddlperson]').multiselect({
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        }
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
        function loding() {
            $('#spinner').show();
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
               function validate(persons) {
                   if (document.getElementById("<%=ddlType.ClientID%>").value == "--Select--" || document.getElementById("<%=ddlType.ClientID%>").value == "0") {
                       errormessage("Please Select Group Name");
                       document.getElementById("<%=ddlType.ClientID%>").focus();
                return false;
                   }
               if (persons=="") {
                       errormessage("Please Select Person Name");
                       document.getElementById("<%=ddlperson.ClientID%>").focus();
                 return false;
                   }
                   if (document.getElementById("<%=txtfromtime.ClientID%>").value == "" || document.getElementById("<%=txtfromtime.ClientID%>").value == "00:00") {
                       errormessage("Your Time Format is incorrect. Please try again.");
                       document.getElementById("<%=txtfromtime.ClientID%>").focus();
                return false;
            }
            var digits = /^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$/;
            var digitsid = document.getElementById("<%=txtfromtime.ClientID %>").value;
            var digitsArray = digitsid.match(digits);
            var temp;
            if (digitsArray == null) {
                errormessage("Your Time Format is incorrect. Please try again.");
                document.getElementById("<%=txtfromtime.ClientID %>").focus();
                return false;
            }
              if (document.getElementById("<%=txttotime.ClientID%>").value == "" || document.getElementById("<%=txttotime.ClientID%>").value == "00:00") {
              errormessage("Your Time Format is incorrect. Please try again.");
              document.getElementById("<%=txttotime.ClientID%>").focus();
              return false;
            }
            var digits = /^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$/;
            var digitsid = document.getElementById("<%=txttotime.ClientID %>").value;
            var digitsArray = digitsid.match(digits);
            var temp;
            if (digitsArray == null) {
                errormessage("Your Time seems incorrect. Please try again.");
                document.getElementById("<%=txttotime.ClientID %>").focus();
                return false;
            }
                   var start = document.getElementById("<%=txtfromtime.ClientID %>").value;
                   var end = document.getElementById("<%=txttotime.ClientID %>").value;
                   var dtStart = new Date("1/1/2007 " + start);
                   var dtEnd = new Date("1/1/2007 " + end);
                   if (Date.parse(dtStart) > Date.parse(dtEnd)) {
                       errormessage("To Time Should be greater then From Time");
                       document.getElementById("<%=txttotime.ClientID %>").focus();
                return false;
            }
               }
               
    </script>

     <script type="text/javascript">

         function btnSubmitfunc() {
             var selectedValues = [];
             $("#<%=ddlperson.ClientID %> :selected").each(function () {
               selectedValues.push($(this).val());
           });
           $("#hidpersons").val(selectedValues);
           validate($("#hidpersons").val());
           loding();
           BindGridView();          
       }

         function BindGridView() {             
            $.ajax({
                type: "POST",
                url: "timedistance.aspx/GetData",
                contentType: "application/json;charset=utf-8",
                data: "{'persons':'" + $("#hidpersons").val() + "','loc':'" + document.getElementById("<%=ddlloc.ClientID %>").value + "','accuracy':'" + document.getElementById("<%=txtaccu.ClientID %>").value + "','timeinterval':'" + document.getElementById("<%=txttimediff.ClientID %>").value + "','fromdate':'" + document.getElementById("<%=txt_fromdate.ClientID %>").value + "','todate':'" + document.getElementById("<%=txt_todate.ClientID %>").value + "','fromtime':'" + document.getElementById("<%=txtfromtime.ClientID %>").value + "','totime':'" + document.getElementById("<%=txttotime.ClientID %>").value + "'}",
               dataType: "json",
               success: OnSuccess,
               error: function (result) {

               }
           });
       }

       function OnSuccess(response) {
           //  alert(JSON.stringify(response.d));
           // alert(response.d);
           $('div[id$="rptmain1"]').show();
           var data = JSON.parse(response.d);
           //alert(data);
           var arr1 = data.length;
           //alert(arr1);

           var table = $('#ctl00_ContentPlaceHolder1_rptmain1 table').DataTable();
           table.destroy();          
           $("#ctl00_ContentPlaceHolder1_rptmain1 table ").DataTable({

           //var table = $('#ContentPlaceHolder1_rptmain1 table').DataTable();
           //table.destroy();
           //$("#ContentPlaceHolder1_rptmain1 table ").DataTable({

               //"order": [[0, "Asc"]],
               "aaData": data,
               "aoColumns": [
           {
               "mData": "Person",

           }, 
            { "mData": "empcode" },
           { "mData": "address" },
           { "mData": "Cdate" },
           { "mData": "Time" },
           { "mData": "TimeDiff" },
           { "mData": "distance" },
            { "mData": "speed" },
            { "mData": "Battery" },
            { "mData": "Signal" },
            { "mData": "Accuracy" },
            { "mData": "HomeFlag" }
               ]
               });
               $('#spinner').hide();

       }
      
    </script>

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
        <div class="box-body" id="rptmain" runat="server" >
            <div class="row">
                <div class="col-xs-12">

                    <div class="box">
                        <div class="box-header">
                            <h3 class="box-title">Time Distance Log Report</h3>
                            </div>
                    
                        <div class="clearfix"></div>
                           <%--  <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <asp:Image ID="Image1" ImageUrl="~/img/waiting.gif" AlternateText="Processing" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
   <%-- <ajax:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
        PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />--%>
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="container-fluid" style="min-height:600px;">
                <div class="col-md-6"></div>
                <div class=" col-md-6"><asp:Button ID="btnExport" runat="server" CssClass=" btn btn-primary pull-right" Text="Export" OnClick="btnExportCSV_Click"/>                   

                </div>
            <div class="clearfix"></div>
         <asp:Panel ID="UserReg" runat="server">
             
             
             <div class="form-group col-md-4 col-sm-6 col-xs-12" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Group Name" >Group Name :</label>
         
                          
                            <asp:DropDownList ID="ddlType" CssClass="textbox form-control"  AutoPostBack="true" 
                                 runat="server" 
                                onselectedindexchanged="ddlType_SelectedIndexChanged">
                            </asp:DropDownList>
                       
                  </div>
             
            <div class="form-group col-md-4 col-sm-6 col-xs-12" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="Person Name" >Person Name :</label>
                      <div class="clearfix"></div>
                       <asp:ListBox ID="ddlperson" runat="server" SelectionMode="Multiple"  ></asp:ListBox>
                      <%--   <asp:DropDownCheckBoxes ID="ddlperson" runat="server" AddJQueryReference="true"  onchange="onSelectionChange()"
                                UseSelectAllNode = "false"   ClientIDMode="Static" CssClass="textbox form-control">
 <Texts SelectBoxCaption="--Select All--" />  
    <Style2    DropDownBoxBoxHeight="200" />   
</asp:DropDownCheckBoxes>     --%>
<%-- <asp:ExtendedRequiredFieldValidator ID = "ExtendedRequiredFieldValidator1" runat = "server"
      ControlToValidate = "ddlperson" ErrorMessage = "*" ForeColor = "Red"></asp:ExtendedRequiredFieldValidator>  --%>

                   </div>
             <div class="clearfix"></div>

             <div class="form-group col-md-4 col-sm-6 col-xs-12" >
                   <label for="requiredfield" class="back">*</label>
                   <label for="From" >From Date :</label>
                   
                 <div class="clearfix"></div>

                    <div class="col-md-6 col-sm-8 col-xs-8 no-padding">
                 
                 <asp:TextBox ID="txt_fromdate" runat="server" OnTextChanged="txt_fromdate_TextChanged" AutoPostBack="true" CssClass="aspNetDisabled textbox form-control"></asp:TextBox>
                 </div>
                
<%--                 <asp:ImageButton ID="img1" runat="server" ImageUrl="~/img/Calendar.png" />
                 --%>    
                    
                      <div class="col-md-1 col-xs-2 col-sm-1 no-padding">
                        <a href="javascript:;" id="img1" class="cal-icon"><i class="fa fa-calendar" aria-hidden="true"></i></a>
                         </div>



                          <div class="col-md-5 col-sm-3 col-xs-6 paddingright0">
                              <asp:TextBox ID="txtfromtime" runat="server" MaxLength="6" CssClass="textbox form-control" ></asp:TextBox></div>
                     
                          <ajax:CalendarExtender ID="cc1" runat="server" TargetControlID="txt_fromdate" PopupButtonID="img1" Format="dd-MMM-yyyy">

                                                                       </ajax:CalendarExtender>
                
               
                         </div> 
                  

            <div class="form-group col-md-4 col-sm-6 col-xs-12" >&nbsp;&nbsp;&nbsp;
                   <label for="requiredfield" class="back">*</label>
                   <label for="To" >To Date :</label>
                     <div class="clearfix"></div>

                       <div class="col-md-6 col-sm-8 col-xs-8 no-padding">
                            <asp:TextBox ID="txt_todate" runat="server" OnTextChanged="txt_todate_TextChanged" AutoPostBack="true" CssClass="aspNetDisabled textbox form-control"></asp:TextBox>
                   </div>
                  <%-- <asp:ImageButton ID="img2" runat="server" ImageUrl="~/img/Calendar.png" />--%>
                            <div class="col-md-1 col-xs-2 col-sm-1 no-padding">
                        <a href="javascript:;" class="cal-icon" ID="img2" runat="server"><i class="fa fa-calendar" aria-hidden="true"></i></a>
                             
                             </div>
            
                    <div class="col-md-5 col-sm-3 col-xs-6 paddingright0">       <asp:TextBox ID="txttotime" runat="server" MaxLength="6" CssClass="textbox form-control"></asp:TextBox></div>
                         
                   <ajax:CalendarExtender ID="cc2" runat="server" TargetControlID="txt_todate" PopupButtonID="img2" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                   
                 </div>
               
           <div class="clearfix"></div>

           
                <div class="form-group col-md-4 col-sm-4 col-xs-12">
                    <label for="requiredfield" class="back">*</label>
                    <label for="Location">&nbsp;&nbsp;&nbsp; Location :</label>
               
                      <asp:DropDownList ID="ddlloc" runat="server" CssClass="textbox form-control"  AutoPostBack="true" 
                                onselectedindexchanged="ddlloc_SelectedIndexChanged">
                     <asp:ListItem Text="All" Value="0" ></asp:ListItem>
                     <asp:ListItem Text="GPS" Value="G" Selected="True"></asp:ListItem>
                     <asp:ListItem Text="Tower" Value="C"></asp:ListItem>
                 </asp:DropDownList>                  

                   </div>
                      <%--  <td style="display:none;">&nbsp; &nbsp; &nbsp;To Date :
                            <asp:TextBox ID="TextBox1" runat="server" CssClass="textbox" OnTextChanged="cmdSUGGdate_Click" AutoPostBack="true" Width="105px"></asp:TextBox><asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/Calendar.png" />
                            <asp:TextBox ID="TextBox2" runat="server" CssClass="textbox" MaxLength="5" Width="40px"></asp:TextBox>
                            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TextBox1" PopupButtonID="ImageButton1" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                        </td>--%>

              <div class="form-group col-md-2 col-sm-2 col-xs-12">
                    <label for="requiredfield" class="back">*</label>
                &nbsp;&nbsp;&nbsp;    <label for="Accuracy">Accuracy :</label><br>

                     <div class=""> 
                    <asp:TextBox ID="txtaccu" runat="server" Text="50"  CssClass="form-control"></asp:TextBox>
                          <ajax:FilteredTextBoxExtender ID="txtaccu_FilteredTextBoxExtender"
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtaccu">
                        </ajax:FilteredTextBoxExtender>
                  <input type="hidden" id="hidpersons" />(Metres)
                   </div>
                     

                   </div>
             
               <div class="form-group col-md-2 col-sm-2 col-xs-12">
                    <label for="requiredfield" class="back">*</label>
                &nbsp;&nbsp;&nbsp;    <label for="TimeDifference">Time Difference :</label><br>
                    
                     <div class=""> 
                    <asp:TextBox ID="txttimediff" runat="server" Text="20"  CssClass="form-control"></asp:TextBox>
                          <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txttimediff">
                        </ajax:FilteredTextBoxExtender> (Minutes)              
                   </div>
                     
                        

                   </div>
             



             <div class="clearfix"></div>

           <div class="form-group col-md-5 col-sm-5 col-xs-12">
             <%-- <input id = "btnSubmit" type="submit" class="btn btn-primary" value="Generate" onclick="btnSubmitfunc();"/>--%>
               <input type="button" runat="server" onclick="btnSubmitfunc();" class="btn btn-primary" value="Go" />  
              <asp:Button ID="btnshow" runat="server" CssClass="btn btn-primary" Visible="false"  Text="Generate" onclick="btnshow_Click" />                               
           </div>

             <div class="clearfix"></div>
             <br></br>
             <%-- Abhishek jaiswal start --%>
              <div id="rptmain1" runat="server">
                                <div class="box-body table-responsive">
                                    <asp:Repeater ID="salevaluerpt" runat="server">
                                        <HeaderTemplate>
                                            <table id="example1" class="table table-bordered table-striped">
                                                <thead>
                                                    <tr>
                                                       <%-- <th style="text-align: left; width: 6%">SNo.</th>--%>
                                                         <th style="text-align: left; width: 18%">Person</th>
                                                         <th style="text-align: left; width: 18%">Emp Code</th>
                                                        <th style="text-align: left; width: 18%">Address</th>
                                                         <th style="text-align: left; width: 18%">Date</th>
                                                        <th style="text-align: left; width: 18%">Time</th>
                                                        <th style="text-align: left; width: 18%">Time Diff</th>
                                                        <th style="text-align: left; width: 18%">Distance</th>
                                                        <th style="text-align: right; width: 18%">Speed</th>
                                                        <th style="text-align: right; width: 18%">Battery</th>
                                                        <th style="text-align: right; width: 18%">Signal</th>
                                                        <th style="text-align: right; width: 18%">Accuracy</th>
                                                        <th style="text-align: right; width: 18%">Home</th>
                                                        
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                           
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>     </table>       
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>

             <%-- abhishek jaiswal end --%>
               
                            <div id="Div1" class="container table-res" style="height: 700px; overflow-x: scroll; float: left; margin-left: 0px; padding-top: 0px; background-color: #FFF;">
                                <asp:GridView ID="gridtest" runat="server" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Person" HeaderText="Person" />
                                        <asp:BoundField DataField="empcode" HeaderText="Emp Code" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="address" HeaderText="Address" />
                                        <asp:BoundField DataField="Cdate" HeaderText="Date" />
                                        <asp:BoundField DataField="Time" HeaderText="Time" />
                                        <asp:BoundField DataField="TimeDiff" HeaderText="Time Diff(min.)" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="distance" HeaderText="Distance(km.)" DataFormatString="{0:F2}" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="speed" HeaderText="Speed(km/min)" DataFormatString="{0:F2}" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="Battery" HeaderText="Battery" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="Signal" HeaderText="SIG" />
                                        <asp:BoundField DataField="Accuracy" HeaderText="Accuracy" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="HomeFlag" HeaderText="Home" ItemStyle-HorizontalAlign="Center" />
                                    </Columns>
                                </asp:GridView>
                                <asp:GridView ID="gvData" Visible="false" runat="server" AutoGenerateColumns="False" CssClass="table gridclass"
                                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                    CellPadding="3" GridLines="Vertical" >
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                     <EmptyDataRowStyle BackColor="#0158a8" ForeColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Person" HeaderText="Person" />
                                         <asp:BoundField DataField="empcode" HeaderText="Emp Code" ItemStyle-HorizontalAlign="Center"/>
                                        <asp:BoundField DataField="address" HeaderText="Address"  />
                                      <asp:BoundField DataField="Cdate" HeaderText="Date"  />
                                              <asp:BoundField DataField="Time" HeaderText="Time"  />
                                              <asp:BoundField DataField="TimeDiff" HeaderText="Time Diff(min.)" ItemStyle-HorizontalAlign="Right" />
                                              <asp:BoundField DataField="distance" HeaderText="Distance(km.)" DataFormatString="{0:F2}" ItemStyle-HorizontalAlign="Right"/>
                                                 <asp:BoundField DataField="speed" HeaderText="Speed(km/min)" DataFormatString="{0:F2}"  ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="Battery" HeaderText="Battery" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="Signal" HeaderText="SIG" />
                                        <asp:BoundField DataField="Accuracy" HeaderText="Accuracy" ItemStyle-HorizontalAlign="Right"/>
                                           <asp:BoundField DataField="HomeFlag" HeaderText="Home" ItemStyle-HorizontalAlign="Center"/>
                                    </Columns>
                                     <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <PagerStyle BackColor="#3c8dbc" ForeColor="White"  /> 
                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#3C8DBC" Font-Bold="True" ForeColor="White" />
                              <AlternatingRowStyle BackColor="#FFFFFF" />
                                </asp:GridView>

                            <div id="grdDemo"> </div>
                            </div>
                         
            </asp:Panel>
               
                    
                       
                </div>
        
        </ContentTemplate>
       
    </asp:UpdatePanel>
                        <!-- /.box-body -->
                    </div>
                    <!-- /.box -->

                </div>
                <!-- /.col -->
            </div>        

    </div>
    </section>
  
</asp:Content>
