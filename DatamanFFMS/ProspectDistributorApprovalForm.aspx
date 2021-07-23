<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="ProspectDistributorApprovalForm.aspx.cs" Inherits="AstralFFMS.ProspectDistributorApprovalForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <link href="plugins/multiselect.css" rel="stylesheet" />


    <script src="plugins/multiselect.js"></script>
    <script type="text/javascript">
        $(function () {

            $('[data-toggle="tooltip"]').tooltip()

            $('div[id$="rptmain"]').hide();

            $('#itemsaleTable').DataTable({
                "paging": false,
                "ordering": false,
                "info": false,
                "searching": true
            });
            debugger;
            if (('<%=Request.QueryString["partyid"]%>') !="")
            {
                debugger;
                var party = ('<%=Request.QueryString["partyid"]%>');
                $.ajax({
                    type: "POST",
                    url: "ProspectDistributorApprovalForm.aspx/bindgridbyPartyid",
                    data: '{partyid: "' + party + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                },
                error: function (response) {
                }
                 });
            }
            
        });

        

        $(function () {
            $('[id*=lstSalesP]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=lstState]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
            $('[id*=lstCity]').multiselect({
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
    </script>



    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3800, template: "error"
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

            $('#modalClose').on('click', function () {
                $('#ContentPlaceHolder1_myModal').hide();
                $('#spinner').hide();
                $('#spinner1').hide();

            })

            $('#modalClose1').on('click', function () {
                $('#ContentPlaceHolder1_myModal').hide();
                $('#spinner').hide();
                $('#spinner1').hide();

            })

        

        });

    
    </script>
    <script type="text/javascript">


        function loding() {
            $('#spinner').show();
        }
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

        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }

        .modalPopup {
            background-color: #FFFFFF;
            width: 300px;
            border: 1px solid #aaaaaa !important;
            border-radius: 12px;
            padding: 0;
            overflow-y: hidden !important;
        }

            .modalPopup .header {
                border: 1px solid #aaaaaa;
                background: #cccccc url(img/ui-bg_highlight-soft_75_cccccc_1x100.png) 50% 50% repeat-x;
                color: black !important;
                font-weight: bold;
                height: 35px;
                color: White;
                line-height: 30px;
                text-align: center;
                margin-right: 10px;
                border-top-left-radius: 6px;
                border-top-right-radius: 6px;
            }

            .modalPopup .body {
                min-height: 50px;
                line-height: 30px;
                text-align: center;
            }

            .modalPopup .footer {
                margin-right: 10px;
            }

            .modalPopup .modelbtn {
                color: White;
                line-height: 23px;
                text-align: center;
                font-weight: bold;
                cursor: pointer;
                border-radius: 4px;
                border: 1px solid #d3d3d3 !important;
                background: #e6e6e6 url(img/ui-bg_glass_75_e6e6e6_1x400.png) 50% 50% repeat-x;
                font-weight: normal;
                color: #555555;
            }

        .modalPopup {
            border-radius: 11px;
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: #3c8dbc;
            padding-top: 10px;
            padding-left: 10px;
            width: 40%;
            height: 380px;
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
            width: 150px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }

         .spinner1 {
            
            position: absolute;
            top: 50%;
            left: 50%;
            margin-left: -50px; /* half width of the spinner gif */
            margin-top: -50px; /* half height of the spinner gif */
            text-align: center;
            z-index: 999;
            overflow: auto;
            width: 150px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }
      .clsLabel{
          font-weight:600 !important;
          color:#3498DB;
      }
      .img{
        
    font-size: 20px;
    background-color: transparent;
    border-style: none;  
    color: deepskyblue;
    float:right;

      }

    </style>
    <script type="text/javascript">

        function btnSubmitfunc() {
            var selectedvalue = [];
            $("#<%=lstSalesP.ClientID %> :checked").each(function () {
                selectedvalue.push($(this).val());
            });
            if (selectedvalue == "") {
                errormessage("Please Select Sales Person");
                return false;
            }

            loding();
            BindGridView();
        }

        function BindGridView() {
            $.ajax({
                type: "POST",
                url: "ProspectDistributorApprovalForm.aspx/bindgrid",
                data: '{smid: "' + $('#<%=lstSalesP.ClientID%>').val() + '", stateid: "' + $('#<%=lstState.ClientID%>').val() + '" , cityid: "' + $('#<%=lstCity.ClientID%>').val() + '", Fromdate: "' + $('#<%=txtfmDate.ClientID%>').val() + '",Todate: "' + $('#<%=txttodate.ClientID%>').val() + '",status:"' + $('#<%=ddlStatus.ClientID%>').val() + '"}',
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: OnSuccess,
              failure: function (response) {
                  //alert(response.d);
              },
              error: function (response) {
                  //alert(response.d);
              }
          });
      };



      function OnSuccess(response) {

          $('div[id$="rptmain"]').show();
          var data = JSON.parse(response.d);

          var table = $('#itemsaleTable').DataTable();
          table.destroy();
          debugger;
          $("#itemsaleTable").DataTable({
              "ordering": true,
              "aaData": data,
              "aoColumns": [
          { "mData": "sno" },
          { "mData": "PartyName" },
          { "mData": "Mobile" },
          {  "mData": "addres",
              "render":function(data,type,row,meta)
              {
                  if (type === 'display') {
                      return $('<div>')
                         .attr('style', 'white-space:normal')
                         .text(data)
                         .wrap('<div></div>')
                         .parent()
                         .html();

                  }
                  else {
                      return data;
                  }

              }
          },
           { "mData": "GSTIN" },
          { "mData": "createdBy" },
          {
              "mData": "Created_Date",
                "render": function (data, type, row, meta) {

                    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                    var date = new Date(data);
                    var day = date.getDate();
                    var month = date.getMonth();
                    var year = date.getFullYear();

                    var mname = monthNames[date.getMonth()]

                    var fdate = day + '/' + mname + '/' + year;

                    if (type === 'display') {
                        return $('<div>')
                           .attr('class', 'text')
                           .text(fdate)
                           .wrap('<div></div>')
                           .parent()
                           .html();

                    } else {
                        return data;
                    }
                }

          },
          {
              "mData": "status",
              "render": function (data, type, row, meta) {
                  if (data === 'Rejected') {
                      return $('<div>')
                         .attr('style', 'color:#F14310;')
                         .text(data)
                         .wrap('<div></div>')
                         .parent()
                         .html();
                  }
                  else if (data === 'Approved') {
                      return $('<div>')
                         .attr('style', 'color:#0CBF4D;')
                         .text(data)
                         .wrap('<div></div>')
                         .parent()
                         .html();
                  }
                  else {
                      return data;
                  }

              }
          },
          {
              "mData": "partyid",

              "render": function (data, type, row, meta) {
                  var did = data;
                  var rows = row.sno;
                  return '<button type="button" id=' + did + ' class="fa fa-info-circle" style="color:#EFF4F1;border: none;background-color: #0C99BF;font-weight: 800;font-size:25px;font-family:"Font Awesome 5 Pro";" onclick="getApprovDetails(this.id,' + rows + ')" data-toggle="tooltip" data-placement="bottom" title="Click to ViewDetails"></button>';
              }
          }
              ]
          });
          $('#spinner').hide();
      }





      function getApprovDetails(partyid, rowid) {
          loding();
          debugger;
          $('#rowid').val(rowid);
          $.ajax({
              type: "POST",
              url: "ProspectDistributorApprovalForm.aspx/bindApprovalGrid",
              data: '{partyid: "' + partyid + '"}',
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: OnSuccessApprove,
              failure: function (response) {
                  //alert(response.d);
              },
              error: function (response) {
                  //alert(response.d);
              }
          });

          document.getElementById("ContentPlaceHolder1_myModal").style.display = "block";
      }

      function OnSuccessApprove(response) {
          debugger;
          var data = JSON.parse(response.d);
          document.getElementById("distnm").innerHTML = data[0].PartyName;
          document.getElementById("add").innerHTML = data[0].addres;
          document.getElementById("city").innerHTML = data[0].AreaName;
          document.getElementById("email").innerHTML = data[0].Email;
          document.getElementById("mb").innerHTML = data[0].Mobile;
          document.getElementById("GSTN").innerHTML = data[0].GSTIN;
          document.getElementById("remk").innerHTML = data[0].Remark;
          document.getElementById("pan").innerHTML = data[0].panno;
          document.getElementById("by").innerHTML = data[0].createdBy;

          document.getElementById("adhaar").innerHTML = data[0].adhaar;
          document.getElementById("contact").innerHTML = data[0].contactPersonName;
          document.getElementById("officePhone").innerHTML = data[0].officePhone;
          document.getElementById("ResidencePhone").innerHTML = data[0].ResidencePhone;
          document.getElementById("bNature").innerHTML = data[0].bNature;
          document.getElementById("dirName").innerHTML = data[0].dirName;
          document.getElementById("invPurpose").innerHTML = data[0].invPurpose;
          document.getElementById("storageFacility").innerHTML = data[0].storageFacility;
          document.getElementById("storageFacilitySqrFit").innerHTML = data[0].storageFacilitySqrFit;

          document.getElementById("empSales").innerHTML = data[0].empSales;
          document.getElementById("empOther").innerHTML = data[0].empOther;
          document.getElementById("mnthlyTurnOver").innerHTML = data[0].mnthlyTurnOver;
          document.getElementById("systemComputing").innerHTML = data[0].systemComputing;
          document.getElementById("distU").innerHTML = data[0].distU;
          document.getElementById("retialerU").innerHTML = data[0].retialerU;
          document.getElementById("newsPaperPublished").innerHTML = data[0].newsPaperPublished;
          document.getElementById("FSSAINo").innerHTML = data[0].FSSAINo;
          document.getElementById("Bank").innerHTML = data[0].Bank;

          document.getElementById("Branch").innerHTML = data[0].Branch;
          document.getElementById("ACCNO").innerHTML = data[0].ACCNO;
          document.getElementById("otherBranch").innerHTML = data[0].otherBranch;
          var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
          var date = new Date(data[0].Created_Date);
          var day = date.getDate();
          var month = date.getMonth();
          var year = date.getFullYear();
          var mname = monthNames[date.getMonth()]
          var fdate = day + '/' + mname + '/' + year;
          document.getElementById("crdt").innerHTML = fdate;
          document.getElementById("status").innerHTML = data[0].status;
         
          $("#<%=TextArea1.ClientID%>").val(data[0].ARremk);
          $('#partyid').val(data[0].PartyId);

          $('#GSTINDOC').val(data[0].GSTINDOC);
          $('#PANDOC').val(data[0].PANDOC);
          $('#ADHAARDOC').val(data[0].ADHAARDOC);
          $('#FSSAILisenceDOC').val(data[0].FSSAILisenceDOC);
          $('#BankDOC').val(data[0].BankDOC);



          var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
          var date = new Date(data[0].Approved_RejectedDate);
          var day = date.getDate();
          var month = date.getMonth();
          var year = date.getFullYear();
          var mname = monthNames[date.getMonth()]
          var fdate = day + '/' + mname + '/' + year;

          document.getElementById("Approved_RejectedDate").style.display = "block";
          document.getElementById("Approved_RejectedBy").style.display = "block";
          document.getElementById("Approved_RejectedDatetxt").style.display = "block";
          document.getElementById("Approved_RejectedBytxt").style.display = "block";

          if (data[0].status == "Approved")
          {

              document.getElementById("Approved_RejectedDate").innerHTML = fdate;
              document.getElementById("Approved_RejectedDate").style.color = "#58D68D";
              document.getElementById("Approved_RejectedBy").innerHTML = data[0].Approved_RejectedBy;
              document.getElementById("Approved_RejectedBy").style.color = "#58D68D";
              document.getElementById("status").style.color = "#58D68D";
              document.getElementById("btnApprove").disabled = true;
              document.getElementById("btnReject").disabled = true;

          }
          else if (data[0].status == "Rejected")
          {
              document.getElementById("Approved_RejectedDate").innerHTML = fdate;
              document.getElementById("Approved_RejectedDate").style.color = "#C0392B";
              document.getElementById("Approved_RejectedBy").innerHTML = data[0].Approved_RejectedBy;
              document.getElementById("Approved_RejectedBy").style.color = "#C0392B";

              document.getElementById("status").style.color = "#C0392B";
              document.getElementById("btnApprove").disabled = true;
              document.getElementById("btnReject").disabled = true;
          }
          else if (data[0].status == "Pending")
          {
              document.getElementById("Approved_RejectedDate").style.display = "none";
              document.getElementById("Approved_RejectedBy").style.display = "none";
              document.getElementById("Approved_RejectedDatetxt").style.display = "none";
              document.getElementById("Approved_RejectedBytxt").style.display = "none";

              document.getElementById("status").style.color = "#3498DB";
              document.getElementById("btnApprove").disabled = false;
              document.getElementById("btnReject").disabled = false;
              
          }
          $('#spinner').hide();
      }

     

      function approveRejectDist(ARflg)
      {
          debugger;
          $('#spinner1').show();
          document.getElementById("btnApprove").disabled = true;
          document.getElementById("btnReject").disabled = true;
          var partyid = $('#partyid').val();
          var rowid = $('#rowid').val();
          var tbl = document.getElementById("itemsaleTable");
          var text,color;
          var text1 = "";
          if (ARflg == "A")
          {
              text = "Approved";
              color = "#0CBF4D ";

          }
          else
          {
              text = "Rejected";
              color = "#F14310 ";
          }
          $.ajax({
              type: "POST",
              url: "ProspectDistributorApprovalForm.aspx/approveRejectDist",
              data: '{partyid: "' + partyid + '",ARremk:"' + $('#<%=TextArea1.ClientID %>').val() + '",AR:"' + ARflg + '"}',
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (response) {

                  debugger;
                  var res = response.d;
                  console.log(res);
                  if (ARflg == 'A')
                  {
                      if (res.includes('can not be approved'))
                      {
                          errormessage(response.d);
                          $('#ContentPlaceHolder1_myModal').hide();
                          document.getElementById("btnApprove").disabled = false;
                          document.getElementById("btnReject").disabled = false;
                          $('#spinner1').hide();
                      }
                      else
                      {
                          Successmessage(response.d);
                          tbl.rows[rowid].cells[7].innerHTML = text;
                          tbl.rows[rowid].cells[7].style.color = color;
                      }
                  }
                  
                  
                  if (ARflg == 'R')
                  {
                      Successmessage(response.d);
                      tbl.rows[rowid].cells[7].innerHTML = text;
                      tbl.rows[rowid].cells[7].style.color = color;

                      tbl.rows[rowid].style.textDecoration = "line-through";
                      tbl.rows[rowid].style.textDecorationColor = "red";
                  }
                  $('#ContentPlaceHolder1_myModal').hide();
                  document.getElementById("btnApprove").disabled = false;
                  document.getElementById("btnReject").disabled = false;
                  $('#spinner1').hide();
                 
              },
              failure: function (response) {                  
                  errormessage(response.d);
                  $('#ContentPlaceHolder1_myModal').hide();
                  document.getElementById("btnApprove").disabled = false;
                  document.getElementById("btnReject").disabled = false;
                  $('#spinner1').hide();
              },
              error: function (response) {
                  errormessage(response.d);
                  $('#ContentPlaceHolder1_myModal').hide();
                  document.getElementById("btnApprove").disabled = false;
                  document.getElementById("btnReject").disabled = false;
                  $('#spinner1').hide();
                 
              }

          });

         
      }

        function showDoc(docType)
        {
            debugger;
            var src=""
            if (docType.toLowerCase() == 'gstn')
            {
                src = $('#GSTINDOC').val();
            }
            else if (docType.toLowerCase() == 'pan') {
                src = $('#PANDOC').val();
            }
            else if (docType.toLowerCase() == 'adhar') {
                src = $('#ADHAARDOC').val();
            }
            else if (docType.toLowerCase() == 'fssai') {
                src = $('#FSSAILisenceDOC').val();
            }
            else if (docType.toLowerCase() == 'bank') {
                src = $('#BankDOC').val();
            }

            if (src == "" || src == undefined)
            {
                errormessage("No Document Found.");
            }
            else
            {
                window.open(src, '_blank');
            }
        }
       
    </script>
    <script type="text/javascript">
        $("[id$=btnExport]").click(function (e) {
          
            window.open('data:application/vnd.ms-excel,' + encodeURIComponent($('div[id$=rptmain]').html()));
            e.preventDefault();
        });
    </script>
    <section class="content">
       
        <div id="spinner" class="spinner" style="display: none;">
            <img id="img-spinner" src="img/Waiting.gif" alt="Loading" /><br />
            <%--Loading Data....--%>
        </div>
        <div id="messageNotification">
            <div>
                <asp:Label ID="lblmasg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="box-body">
            <!-- left column -->
            <!-- general form elements -->
            <div class="box box-primary">
                <div class="row">
                    <!-- left column -->
                    <div class="col-md-12">
                        <div id="InputWork">
                            <!-- general form elements -->

                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <%-- <h3 class="box-title">Distributor Dispatch Order</h3>--%>
                                    <h3 class="box-title">
                                        <asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:ListBox ID="lstSalesP" runat="server" SelectionMode="Multiple" AutoPostBack="true"></asp:ListBox>
                                            </div>
                                        </div>

                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">State:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red; display: none;">*</label>
                                                <asp:ListBox ID="lstState" runat="server" SelectionMode="Multiple" OnSelectedIndexChanged="lstState_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                            </div>

                                        </div>
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red; display: none;">*</label>
                                                <asp:ListBox ID="lstCity" runat="server" SelectionMode="Multiple" AutoPostBack="true"></asp:ListBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="row">
                                        <div class="col-md-4 col-sm-6 col-xs-12">
                                            <div id="divxy" class="form-group">
                                                  <label for="exampleInputEmail1">Status:</label>
                                               <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true"  CssClass="form-control">
                                                   <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                   <asp:ListItem Value="A">Approved</asp:ListItem>
                                                   <asp:ListItem Value="R">Reject</asp:ListItem>
                                               </asp:DropDownList>

                                            </div>
                                        </div>
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div id="DIV1" class="form-group">
                                                <label for="exampleInputEmail1">From Date:</label>
                                                <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">To:</label>
                                                <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                                <div class="box-footer">
                                    <input type="button" runat="server" onclick="btnSubmitfunc();" class="btn btn-primary" value="Go" />

                                    <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Cancel_Click" />
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                        OnClick="btnExport_Click" Visible="true" />
                                </div>
                                <br />
                            </div>
                        </div>
                        <div id="rptmain" runat="server">
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="distitemsalerpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="itemsaleTable" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <th style="text-align: left; width: 5%">S.No</th>
                                                    <th style="text-align: left; width: 20%">Party Name</th>
                                                    <th style="text-align: left; width: 20%">Mobile</th>
                                                    <th style="text-align: left; width: 15%">Address</th>
                                                    <th style="text-align: left; width: 8%">GSTIN</th>
                                                    <th style="text-align: left; width: 8%">CreatedBy</th>
                                                    <th style="text-align: left; width: 20%">CreatedDate</th>
                                                    <th style="text-align: left; width: 20%">Status</th>
                                                    <th style="text-align: left; width: 20%">Action</th>
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
                        <br />
                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div>
            <!-- The Modal  for Prospect Distributor Approval Modal -->
            <div class="modal" id="myModal" runat="server">
                <div class="modal-dialog">
                    <div class="modal-content" style="height: 550px !important; overflow-y: auto !important; width:1044px !important; margin-left: -22%;">
                         <div id="spinner1" class="spinner" style="display: none;">
            <img id="img-spinner1" src="img/Waiting.gif" alt="Loading" /><br />
        
        </div>
                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Prospect Distributor Details</h4>
                            <button id="modalClose1" type="button" class="close" style="margin-top: -28px !important;">&times;</button>
                        </div>
                        <!-- Modal body <i class="fas fa-image"></i> -->
                        <div class="modal-body" style="width:1000px;">
                            <div class="box-body">
                                <div class="row">
                                    <div id="Div2" runat="server" >
                                        <table class="table table-bordered table-striped">
                                            <tr><td><label style="text-align: left;">Name</label></td><td><label class="clsLabel"  id="distnm"></label></td>
                                            <td> <label style="text-align: left;">GSTIN &nbsp&nbsp</label><button id="gstindoc"  type="button" onclick="showDoc('gstn')" class="fas fa-image  img" data-toggle="tooltip" data-placement="top" title="Click to View GSTIN DOC" ></button></td><td><label class="clsLabel" id="GSTN" ></label></td></tr>
                                             <tr><td> <label style="text-align: left; ">Address</label></td><td><label class="clsLabel" id="add" ></label></td>
                                            <td> <label style="text-align: left;">PANNo &nbsp&nbsp </label> <button id="Pandoc" type="button" onclick="showDoc('PAN')" class="fas fa-image img" data-toggle="tooltip" data-placement="top" title="Click to View PANNo DOC" ></button></td><td><label class="clsLabel" id="pan" ></label></td></tr>
                                             <tr><td> <label style="text-align: left; ">City</label></td><td><label class="clsLabel" id="city" ></label></td>
                                            <td><label style="text-align: left; ">ADHAAR NO &nbsp&nbsp </label> <button id="Adhardoc"  type="button" onclick="showDoc('adhar')" class="fas fa-image img" data-toggle="tooltip" data-placement="top" title="Click to View Adhaar DOC" ></button></td><td><label class="clsLabel" id="adhaar" ></label></td></tr>
                                             <tr><td> <label style="text-align: left; ">Email</label></td><td><label class="clsLabel" id="email" ></label></td>
                                            <td><label style="text-align: left;">Contact Person Name</label></td><td><label class="clsLabel" id="contact" ></label></td></tr>
                                             <tr><td><label style="text-align: left; ">Mobile</label></td><td><label class="clsLabel" id="mb" ></label></td>
                                            <td><label style="text-align: left; ">Phone No Office</label></td><td><label class="clsLabel" id="officePhone" ></label></td></tr>
                                             <tr><td> <label style="text-align: left;">Phone No Residence</label></td><td><label class="clsLabel" id="ResidencePhone" ></label></td>
                                            <td><label style="text-align: left;">Business Nature</label></td><td><label class="clsLabel" id="bNature" ></label></td></tr>
                                             <tr><td> <label style="text-align: left;">Partner Propector Director Name</label></td><td><label class="clsLabel" id="dirName" ></label></td>
                                            <td><label style="text-align: left; ">Investment Proposed</label></td><td><label class="clsLabel" id="invPurpose" ></label></td></tr>

                                            <tr><td><label style="text-align: left; ">Storage Facility</label></td><td><label class="clsLabel" id="storageFacility" ></label></td>
                                                <td> <label style="text-align: left; ">Storage Facility Square fit</label></td><td><label class="clsLabel" id="storageFacilitySqrFit" ></label></td></tr>
                                             <tr><td><label style="text-align: left; ">No Of Emp SalesPerson</label></td><td><label class="clsLabel" id="empSales" ></label></td>
                                            <td><label style="text-align: left; ">No Of Emp Others</label></td><td><label class="clsLabel" id="empOther" ></label></td></tr>
                                             <tr><td> <label style="text-align: left; ">Monthly Expected Turnover</label></td><td><label class="clsLabel" id="mnthlyTurnOver" ></label></td>
                                            <td><label style="text-align: left; ">No Of System Computing</label></td><td><label class="clsLabel" id="systemComputing" ></label></td></tr>
                                             <tr><td> <label style="text-align: left; ">Distributor Under You</label></td><td><label class="clsLabel" id="distU" ></label></td>
                                            <td><label style="text-align: left; ">Retailer Under You</label></td><td><label class="clsLabel" id="retialerU" ></label></td></tr>

                                            <tr><td> <label style="text-align: left; ">NewsPaper Published</label></td><td><label class="clsLabel" id="newsPaperPublished" ></label></td>
                                            <td><label style="text-align: left;">Festival-fairs In Your Area Timings</label></td><td><label class="clsLabel" id="festTime" ></label></td></tr>
                                             <tr><td> <label style="text-align: left;">FSSAI License No &nbsp&nbsp</label> <button id="FSSAIdoc"  type="button" onclick="showDoc('FSSAI')" class="fas fa-image img" data-toggle="tooltip" data-placement="top" title="Click to View FSSAI License DOC" ></button></td><td><label class="clsLabel" id="FSSAINo" ></label></td>
                                            <td><label style="text-align: left;">Bank Name &nbsp&nbsp </label><button id="Bankdoc"  type="button" onclick="showDoc('Bank')" class="fas fa-image img" data-toggle="tooltip" data-placement="top" title="Click to View Bank DOC" ></button></td><td><label class="clsLabel" id="Bank" ></label></td></tr>
                                             <tr><td><label style="text-align: left; ">Branch Name</label></td><td><label class="clsLabel" id="Branch" ></label></td> <td><label style="text-align: left; ">Other Branch Name</label></td><td><label class="clsLabel" id="otherBranch" ></label></td></tr>
                                            <tr>
                                            <td><label style="text-align: left; ">Account No</label></td><td><label class="clsLabel" id="ACCNO" ></label></td><td> <label style="text-align: left;">Created By</label></td><td><label class="clsLabel" id="by"  > </label></td></tr>
                                             <tr><td> <label style="text-align: left; ">Status</label></td><td><label class="clsLabel" id="status" ></label></td><td> <label style="text-align: left; ">Created Date</label></td><td><label class="clsLabel" id="crdt" ></label></td></tr>
                                             <tr>
                                            <td><label style="text-align: left;">Remark</label></td><td><label class="clsLabel" id="remk" ></label></td><td><label id="Approved_RejectedBytxt" style="text-align: left;">Approved/Rejected By</label></td><td><label class="clsLabel" id="Approved_RejectedBy" ></label></td></tr>
                                             <tr>
                                                 <td><label id="Approved_RejectedDatetxt" style="text-align: left; ">Approved/Rejected Date</label></td><td><label class="clsLabel" id="Approved_RejectedDate" ></label></td></tr>
                                              <tr>  <td style="text-align: left; width: 20%"><b>Approval/Rejection Remark:</b>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label></td>
                                                <td>
                                                    <textarea id="TextArea1" class="form-control" style="resize: none; height: 80px; width: 420px;" cols="20" rows="2" runat="server" placeholder="Enter Remark"></textarea></td>
                                            </tr>
                                        </table>
                                        <input  type="hidden" id="partyid" />
                                         <input  type="hidden" id="rowid" />

                                         <input  type="hidden" id="GSTINDOC" />
                                         <input  type="hidden" id="PANDOC" />
                                         <input  type="hidden" id="ADHAARDOC" />
                                         <input  type="hidden" id="FSSAILisenceDOC" />
                                          <input  type="hidden" id="BankDOC" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Modal footer -->
                        <div class="modal-footer">
                            
                             <button id="btnApprove" type="button" class="btn btn-success" data-toggle="tooltip" data-placement="top" title="Click to Approve" onclick="approveRejectDist('A')" runat="server">Approve</button>&nbsp;&nbsp;
                             <button id="btnReject" type="button" class="btn btn-danger" data-toggle="tooltip" data-placement="top" title="Click to Reject" onclick="approveRejectDist('R')" runat="server">Reject</button>&nbsp;&nbsp;
                            <button id="modalClose" type="button" class="btn btn-danger" >Close</button>
                        </div>

                       

                    </div>
                </div>
            </div>


             
             
        </div>
    </section>
</asp:Content>


