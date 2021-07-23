<%@ Page Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="Retailerdispatchorderform.aspx.cs" Inherits="AstralFFMS.Retailerdispatchorderform" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <%-- <script src="<%=ResolveUrl("~")%>plugins/select2/select2.js"></script>
    <link href="<%=ResolveUrl("~")%>plugins/select2/select2.css" rel="stylesheet" />--%>
    <link href="plugins/multiselect.css" rel="stylesheet" />


    <script src="plugins/multiselect.js"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=ListBox1]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',
                buttonWidth: '100%',
                includeSelectAllOption: false,
                maxHeight: 200,
                width: 215,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });

        $(function () {
            $('[id*=ListBox2]').multiselect({
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

  <%--  <script type="text/javascript">
        $(function () {
            $('[id*=ddlcity]').multiselect({
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

    </script>--%>

    <script type="text/javascript">
        $(function () {
            $('[id*=ListArea]').multiselect({
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
            //$("#itemsaleTable").DataTable({
            //    "order": [[0, "desc"]]
            //});   

            $('#modalClose').on('click', function () {
                var table = $('#Div2 table').DataTable();

                table.column(0).visible(true);
                table.column(1).visible(true);
          

                $('#TextArea1').val("");
                $('#myModal').modal('hide');
            })

            $('#modalClose1').on('click', function () {
                var table = $('#Div2 table').DataTable();

                table.column(0).visible(true);
                table.column(1).visible(true);
                $('#TextArea1').val("");
                $('#myModal').modal('hide');
            })
            $('#modalClose3').on('click', function () {
                var table = $('#Div12 table').DataTable();

                table.column(0).visible(true);
                table.column(1).visible(true);
                $('#myModal3').modal('hide');
            })

            $('#modalClose4').on('click', function () {

                var table = $('#Div12 table').DataTable();

                table.column(0).visible(true);
                table.column(1).visible(true);
                $('#myModal3').modal('hide');
            })

            $('#modalClose5').on('click', function () {
                $('#TextArea3').val("");
                $('#myModal4').modal('hide');
            })

            $('#modalClose6').on('click', function () {

                $('#TextArea3').val("");
                $('#myModal4').modal('hide');
            })
            $('#cmodalClose').on('click', function () {
                var table = $('#Div13 table').DataTable();

                table.column(0).visible(true);
                table.column(1).visible(true);
                $('#TextArea2').val("");
                $('#myModal1').modal('hide');
            })

            $('#cmodalClose1').on('click', function () {
                var table = $('#Div13 table').DataTable();

                table.column(0).visible(true);
                table.column(1).visible(true);
                $('#TextArea2').val("");
                $('#myModal1').modal('hide');
            })
            console.log("Modal called");

        });

        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            //alert(iKeyCode);
            if (iKeyCode != 9 && iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57)) {

                // if (iKeyCode != 8 && iKeyCode != 0 && (iKeyCode < 48 || iKeyCode > 57))

                return false;
            }
            return true;
        }

        function checkVal(a) {

            console.log(a);
            var index = a.split("#")[1]

            var table = $('#example1').DataTable();

            var data = table.rows().data();



            var orderqty = data[index]["OrderQty"];
            var dispatchqty = table.cell(index, 7).nodes().to$().find('input').val();// data[i][7];


            var value = parseFloat(dispatchqty);
            if (value > parseFloat(orderqty)) {
                errormessage("Dispatch quantity should be less than or equal to order quantity");
                table.cell(index, 7).nodes().to$().find('input').val('');
                table.cell(index, 7).nodes().to$().find('input').focus();
                return false;
            }
            $('#row#' + index).val(value);

        }

    </script>
 <%--   <script type="text/javascript">
        $(function () {
            $('[id*=salespersonListBox]').multiselect({
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
    </script>--%>
  <%--  <script type="text/javascript">
        $(function () {
            $("[id*=trview] input[type=checkbox]").bind("click", function () {
                var table = $(this).closest("table");
                if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                    //Is Parent CheckBox
                    var childDiv = table.next();
                    var isChecked = $(this).is(":checked");
                    //alert(isChecked);
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
    </script>--%>

    <script type="text/javascript">

        //  var table = $('#itemsaleTable').DataTable();
        //$('#itemsaleTable').on('click', 'tbody td, thead th:first-child', function (e) {
        //    $(this).parent().find('input[type="checkbox"]').trigger('click');
        //});

        //// Handle click on "Select all" control
        //$('thead input[name="select_all"]').on('click', function (e) {
        //    if (this.checked) {
        //        $('#itemsaleTable tbody input[type="checkbox"]:not(:checked)').trigger('click');
        //    } else {
        //        $('#itemsaleTable tbody input[type="checkbox"]:checked').trigger('click');
        //    }

        //    // Prevent click event from propagating to parent
        //    e.stopPropagation();
        //});

        function openmodeldispatchordermulti(a) {




            var table = $('#itemsaleTable').DataTable();

            var data = table.rows().data();
            var rowcollection = table.$(".call-checkbox:checked", { "page": "all" });
            if (rowcollection.length > 0) {
                var idetails = new Array();

                var itemObjects = [];
                var table = $('#itemsaleTable').DataTable();

                var data = table.rows().data();
                var rowcollection = table.$(".call-checkbox:checked", { "page": "all" });


                var j = 0;
                for (var i = 0; i < data.length; i++) {


                    if (data[i]["Status"] == "Pending" && table.cell(i, 0).nodes().to$().find('input')[0].checked == true) {
                        var itemd = new Object();
                        itemd["docid"] = data[i]["Docid"];
                        idetails[j] = itemd;
                        j++;
                    }


                }

                if (idetails.length == 0) {
                    errormessage("These are already dispatched or cancelled");
                    return false;
                }
            }
            else {
                errormessage("Please select at least one record");
                return false;
            }
            //rowcollection.each(function(index,elem){
            //    var checkbox_value = $(elem).val();
            //    console.log(checkbox_value);
            //    if(checkbox_value=="")
            //    {
            //        errormessage("Please selesct at least one record");
            //        return false;
            //    }
            //    //Do something with 'checkbox_value'
            //});
            ////for (var i = 0; i < data.length; i++) {
            //    console.log(table.cell(i, 0).nodes().to$().find('input:checked').val());

            //}

            $('#hidordeprocesstype').val(a);
            $('#myModal4').modal('show');

        }

        function dispatchordermulti() {
            if ($('#TextArea3').val() == "") {
                errormessage('Please enter Remark');
                return false;
            }
            var idetails = new Array();

            var itemObjects = [];
            var table = $('#itemsaleTable').DataTable();

            var data = table.rows().data();
            var rowcollection = table.$(".call-checkbox:checked", { "page": "all" });


            var j = 0;
            for (var i = 0; i < data.length; i++) {


                if (data[i]["Status"] == "Pending" && table.cell(i, 0).nodes().to$().find('input')[0].checked == true) {
                    var itemd = new Object();
                    itemd["docid"] = data[i]["Docid"];
                    idetails[j] = itemd;
                    j++;
                }
                console.log(table.cell(i, 0).nodes().to$().find('input').val());

            }
            console.log(idetails);


            $.ajax({
                type: "POST",
                url: "And_sync.asmx/Multipledispatchorder",
                data: JSON.stringify({ "userid": '<%=BusinessLayer.Settings.Instance.UserID%>', "Dispatchcanceltype": $('#hidordeprocesstype').val(), "remarks": $('#TextArea3').val(), "docidlist": idetails }),
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
                contentType: "application/json;",
                responseType: "json",
                success: function (response) {
                    console.log(response.d)
                    var data = JSON.parse(response.d);
                    if (data.ResultMsg == "Success") {
                        if ($('#hidordeprocesstype').val() == "D")
                            Successmessage("Order dispatched Successfully")
                        else
                            Successmessage("Order Cancelled Successfully")
                        $('#TextArea3').val("");


                        $('#myModal4').modal('hide');
                        BindGridView();
                    }
                    else {
                        errormessage("Something went wrong");
                    }
                },
                failure: function (response) {
                    //   alert(response);
                },
                error: function (response) {
                    console.log(response);
                }
            });

            //public string userid;
            //public string Dispatchcanceltype;
            //public string remarks;
            //public string latitude { get; set; }
            //public string longitude { get; set; }
            //public List<docidlist> docidlist;
        }
    </script>


    <script type="text/javascript">

        //function postBackByObject() {
        //    var o = window.event.srcElement;
        //    if (o.tagName == "INPUT" && o.type == "checkbox") {
        //        __doPostBack("", "");
        //    }
        //}

      <%--   function fireCheckChanged(e) {
             var ListBox1 = document.getElementById('<%= trview.ClientID %>');
             var evnt = ((window.event) ? (event) : (e));
             var element = evnt.srcElement || evnt.target;

             if (element.tagName == "INPUT" && element.type == "checkbox") {
                 __doPostBack("", "");
             }
         }--%>

        function loding() {
            $('#spinner').show();
        }
    </script>
    <script type="text/javascript">
        function validate(persons) {
            if (document.getElementById("<%=ListBox1.ClientID%>").value == "" || document.getElementById("<%=ListBox1.ClientID%>").value == "0") {
                errormessage("Please Select Distributor");
                document.getElementById("<%=ListBox1.ClientID%>").focus();
                return false;
            }


        }

    </script>
    <script type="text/javascript">

        function viewDetails(a) {
            console.log(a);
            var docid = a.split("#")
            console.log(docid);
            $('#hiddocid').val(docid[1]);
            loding();

            $.ajax({
                type: "POST",
                url: "And_sync.asmx/Getretailerorderdetail",
                data: '{docid: "' + docid[1] + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
                contentType: "application/json;",
                responseType: "json",
                success: OnSuccess2,
                failure: function (response) {
                    //   alert(response);
                },
                error: function (response) {
                    console.log(response);
                }
            });



        }


        function OnSuccess2(response) {
            //  console.log(response);
            //  alert(JSON.stringify(response.d));
            //alert(response.d);
            $('div[id$="Div12"]').show();
            var data = JSON.parse(response.d);
            console.log(data);

            $('#<%=Label3.ClientID%>').text(data[0]["PartyName"]);
            $('#<%=Label5.ClientID%>').text(data[0]["remarks"]);
            $('#<%=Label4.ClientID%>').text(data[0]["vdate"]);
            $('#<%=Label6.ClientID%>').text(data[0]["dispatchcancelremark"]);
            $('#<%=Label7.ClientID%>').text(data[0]["Status"]);
            $('#<%=Label9.ClientID%>').text(data[0]["Address"]);
            $('#<%=Label12.ClientID%>').text(data[0]["DocId"]);
            $('#<%=hiddendocid.ClientID%>').val(data[0]["DocId"]);
            $('#<%=Label11.ClientID%>').text(data[0]["Doneby"]);
            $('#<%=Label13.ClientID%>').text(data[0]["dispatchcanceldatetime"]);
           <%-- $('#<%=lblOrderTakenType.ClientID%>').text(data[0]["OrderTakenType"]);--%>
            //alert(data[0]["OrderTakenType"]);
            var lblOrderTakenType = data[0]["OrderTakenType"];
            if (lblOrderTakenType != "") {
                //$(".faicon").css('visibility', 'visible');
                $("#faicon").css("visibility", "visible");
            }
            else
            {
                $("#faicon").css("visibility", "hidden");
            }

            //alert(data);
            //var arr1 = data.length;
            //alert(arr1);
            var table3 = $('#Div12 table').DataTable();
            table3.destroy();



            $("#Div12 table ").DataTable({

                "footerCallback": function ( row, data, start, end, display ) {
                    var api = this.api(), data;
                    console.log(api
                        .column(11)
                        .data())
                  
 
                    // Total over all pages
                    //total = api
                    //    .column(11)
                    //    .data()
                    //    .reduce( function (a, b) {
                    //        return a + b;
                    //    }, 0 );
 
                    // Total over this page
                    dispatchqtyTotal = api
                        .column( 11, { page: 'current'} )
                        .data()
                        .reduce( function (a, b) {
                            return a + b;
                        }, 0);

                    NetAmountTotal = api
                     .column(10, { page: 'current' })
                     .data()
                     .reduce(function (a, b) {
                         return a + b;
                     }, 0);

                    OrderAmountTotal = api
                   .column(9, { page: 'current' })
                   .data()
                   .reduce(function (a, b) {
                       return a + b;
                   }, 0);


                    disAmountTotal = api
                 .column(8, { page: 'current' })
                 .data()
                 .reduce(function (a, b) {
                     return a + b;
                 }, 0);
                    $(api.column(8).footer()).html(
                                       disAmountTotal.toFixed(2) //+' ( $'+ total +' total)'
                                   );

                    $(api.column(9).footer()).html(
                                      OrderAmountTotal.toFixed(2) //+' ( $'+ total +' total)'
                                  );

                    $(api.column(10).footer()).html(
                                         NetAmountTotal.toFixed(2) //+' ( $'+ total +' total)'
                                     );
                    // Update footer
                    $( api.column(11).footer() ).html(
                        dispatchqtyTotal.toFixed(2) //+' ( $'+ total +' total)'
                    );
                },

                //"scrollY": '20vh',
                //"scrollCollapse": true,
                "order": [[0, "desc"]],
                "columnDefs": [
          {
              "targets": [0],
              "visible": false,

          },
                  {
                      "targets": [1],
                      "visible": false,

                  },

             
    { "width":200, "targets": [2] }
           
                ],
                "aaData": data,
             "paging": false,
                "searching": false,
                "info": false,
                "aoColumns": [


 {
     "mData": "DocId"
 },
 {
     "mData": "ItemId"
 },
        {
            "mData": "ItemName"
            , "render": function (data, type, row, meta) {
                if (type === 'display') {
                    return $('<div>')
                       .attr('style', ' white-space:normal;')
                       .text(data)
                       .wrap('<div></div>')
                       .parent()
                       .html();

                } else {
                    return data;
                }
            }
        },

            //{
            //    "mData": "OrderQty",
            //    "render": function (data, type, row, meta) {
            //        if (type === 'display') {
            //            return $('<div>')
            //               .attr('class', 'text-right')
            //               .text(data)
            //               .wrap('<div></div>')
            //               .parent()
            //               .html();

            //        } else {
            //            return data;
            //        }
            //    }
            //},
            {
                "mData": "QtyDescription",
                "render": function (data, type, row, meta) {
                    if (type === 'display') {
                        return $('<div>')
                           .attr('class', 'text-right')
                           .text(data)
                           .wrap('<div></div>')
                           .parent()
                           .html();

                    } else {
                        return data;
                    }
                }
            },
                {
                    "mData": "rate",
                    "render": function (data, type, row, meta) {
                        if (type === 'display') {
                            return $('<div>')
                               .attr('class', 'text-right')
                               .text(data)
                               .wrap('<div></div>')
                               .parent()
                               .html();

                        } else {
                            return data;
                        }
                    }
                },

                {
                    "mData": "Margin",
                    "render": function (data, type, row, meta) {
                        if (type === 'display') {
                            return $('<div>')
                               .attr('class', 'text-right')
                               .text(data)
                               .wrap('<div></div>')
                               .parent()
                               .html();

                        } else {
                            return data;
                        }
                    }
                },
                     {
                         "mData": "Discount",
                         "render": function (data, type, row, meta) {
                             if (type === 'display') {
                                 return $('<div>')
                                    .attr('class', 'text-right')
                                    .text(data)
                                    .wrap('<div></div>')
                                    .parent()
                                    .html();

                             } else {
                                 return data;
                             }
                         }
                     },
                     {
                         "mData": "DiscountType",
                         "render": function (data, type, row, meta) {
                            if (type === 'display') {
                              return $('<div>')
                                .attr('style', ' white-space:normal;')
                                .text(data)
                                .wrap('<div></div>')
                                .parent()
                                .html();

                             } else {
                                 return data;
                             }
                          }
                     },
                     {
                         "mData": "DiscountAmount",
                         "render": function (data, type, row, meta) {
                             if (type === 'display') {
                                 return $('<div>')
                                    .attr('class', 'text-right')
                                    .text(data)
                                    .wrap('<div></div>')
                                    .parent()
                                    .html();

                             } else {
                                 return data;
                             }
                         }
                     },
                       {
                           "mData": "OrderAmount",
                           "render": function (data, type, row, meta) {
                               if (type === 'display') {
                                   return $('<div>')
                                      .attr('class', 'text-right')
                                      .text(data)
                                      .wrap('<div></div>')
                                      .parent()
                                      .html();

                               } else {
                                   return data;
                               }
                           }
                       },
                       {
                           "mData": "NetAmount",
                           "render": function (data, type, row, meta) {
                               if (type === 'display') {
                                   return $('<div>')
                                      .attr('class', 'text-right')
                                      .text(data)
                                      .wrap('<div></div>')
                                      .parent()
                                      .html();

                               } else {
                                   return data;
                               }
                           }
                       },


            {
                "mData": "DispatchQty",
                "render": function (data, type, row, meta) {
                    if (type === 'display') {
                        return $('<div>')
                           .attr('class', 'text-right')
                             // .attr('style', 'width:120px')
                           .text(data)
                           .wrap('<div></div>')
                           .parent()
                           .html();

                    } else {
                        return data;
                    }
                }
            }
                ]
            });

            $('#spinner').hide();
            $('#myModal3').modal('show');
        }
    </script>
    <script type="text/javascript">
        var QryDocId = "0";
        var distId = 0;
        if ('<%= Request.QueryString["Docid"]%>' != "") {
            QryDocId = '<%= Request.QueryString["Docid"]%>';
            // GetDatabyDocId(DocId);
        }
        if ('<%= Request.QueryString["DistId"]%>' != "") {
            distId = '<%= Request.QueryString["DistId"]%>';
            // GetDatabyDocId(DocId);
        }
        $(document).ready(function () {
            BindState();
            loding();

            console.log($('#<%=hidaddpermission.ClientID%>').val())
            console.log($('#<%=hiddeletepermission.ClientID%>').val())
            console.log($('#<%=hidviewpermission.ClientID%>').val())
            if ($('#<%=hidaddpermission.ClientID%>').val() == "true")
                $('#btnmultidispatch').attr('style', 'visibility: visible;');
            else
                $('#btnmultidispatch').attr('style', 'visibility: hidden;');



            if ($('#<%=hiddeletepermission.ClientID%>').val() == "true")
                $('#btnmulticancel').attr('style', 'visibility: visible;');
            else
                $('#btnmulticancel').attr('style', 'visibility: hidden;');
            if (QryDocId != "0") {
                $.ajax({
                    type: "POST",
                    url: "And_sync.asmx/Getretailerordersbydocid",
                    data: '{docid: "' + QryDocId + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
                    contentType: "application/json;",
                    responseType: "json",
                    success: function (response) {
                        //     console.log(response);
                        //  alert(JSON.stringify(response.d));
                        //alert(response.d);
                        $('div[id$="rptmain"]').show();
                        var data = JSON.parse(response.d);
                        console.log(data);
                        //alert(data);
                        //var arr1 = data.length;
                        //alert(arr1);
                        var table = $('#ContentPlaceHolder1_rptmain table').DataTable();
                        table.destroy();
                        $("#ContentPlaceHolder1_rptmain table ").DataTable({
                            "order": [[0, "desc"]],

                            "aaData": data,
                            //"columnDefs": [{
                            //    "orderable": false,
                            //    "className": 'select-checkbox',
                            //    "targets": 0
                            //}],
                            "aoColumns": [

     {
         "render": function (data, type, row, meta) {

             return '<input type="checkbox" class="call-checkbox"  >';
         }
     }, {
         "mData": "Docid",
         "render": function (data, type, row, meta) {
             if (row.Status == "Pending") {
                 return $('<div>')
                        .attr('class', 'text-left')
                     .attr('style', 'color:blue;font-weight: bold;')
                        .text(data)
                        .wrap('<div></div>')
                        .parent()
                        .html();
             }
             else if (row.Status == "Cancelled") {
                 return $('<div>')
                       .attr('class', 'text-left')
                    .attr('style', 'color:red;font-weight: bold;')
                       .text(data)
                       .wrap('<div></div>')
                       .parent()
                       .html();
             }
             else {
                 return $('<div>')
                     .attr('class', 'text-left')
                  .attr('style', 'color:green;font-weight: bold;')
                     .text(data)
                     .wrap('<div></div>')
                     .parent()
                     .html();
             }
         }


     },
                    { "mData": "PartyName" },
              //      { "mData": "Address" },
                      { "mData": "Mobile" },
                    {
                        "mData": "Distributor", "render": function (data, type, row, meta) {
                            if (type === 'display') {
                                return $('<div>')
                                   .attr('style', ' white-space:normal;width:200px')
                                   .text(data)
                                   .wrap('<div></div>')
                                   .parent()
                                   .html();

                            } else {
                                return data;
                            }
                        }
                    },
                      { "mData": "SMName" },
                    {
                        "mData": "orderdate1",
                        //"render": function (data, type, row, meta) {

                        //    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                        //    var date = new Date(data);
                        //    var day = date.getDate();
                        //    var month = date.getMonth();
                        //    var year = date.getFullYear();

                        //    var mname = monthNames[date.getMonth()]

                        //    var fdate = day + '/' + mname + '/' + year;

                        //    if (type === 'display') {
                        //        return $('<div>')
                        //           .attr('class', 'text')
                        //           .text(fdate)
                        //           .wrap('<div></div>')
                        //           .parent()
                        //           .html();

                        //    } else {
                        //        return data;
                        //    }
                        //}



                    }, // <-- which values to use inside object


                        {
                            "mData": "totalqty",
                            "render": function (data, type, row, meta) {
                                if (type === 'display') {
                                    return $('<div>')
                                       .attr('class', 'text-right')
                                       .text(data)
                                       .wrap('<div></div>')
                                       .parent()
                                       .html();

                                } else {
                                    return data;
                                }
                            }
                        },
                            {
                                "mData": "totalamount",
                                "render": function (data, type, row, meta) {
                                    if (type === 'display') {
                                        return $('<div>')
                                           .attr('class', 'text-right')
                                           .text(data)
                                           .wrap('<div></div>')
                                           .parent()
                                           .html();

                                    } else {
                                        return data;
                                    }
                                }
                            },
                      //{ "mData": "Status" },
                        {
                            "mData": "Docid",
                            "render": function (data, type, row, meta) {
                                //     console.log(row);
                                //     console.log(data);
                                //     console.log(meta);
                                //var did = row.DocId;
                                //console.log(did);
                                var view;
                                var add;
                                var delete1;
                                if ($('#<%=hidviewpermission.ClientID%>').val() == "true") {
                                    view = "visibility: visible;"
                                }
                                else {
                                    view = "visibility: hidden;"

                                }


                                if ($('#<%=hidaddpermission.ClientID%>').val() == "true") {
                                    add = "visibility: visible;"
                                }
                                else {
                                    add = "visibility: hidden;"

                                }
                                if ($('#<%=hidaddpermission.ClientID%>').val() == "true") {
                                    delete1 = "visibility: visible;"
                                }
                                else {
                                    delete1 = "visibility: hidden;"

                                }
                                if (row.Status == "Pending")
                                    return "<button type='button' id='dispatch#" + data.replace(/\s+/g, '-') + "'  data-toggle='tooltip' title='Dispatch' class='btn btn-primary btn-sm' onclick='getDispatchDetails(this.id)' style='" + add + "'><i class='fa fa-truck' aria-hidden='true'></i></button>&nbsp;<button type='button'  id='Cancel#" + data.replace(/\s+/g, '-') + "'  data-toggle='tooltip' title='Cancel' class='btn btn-danger btn-sm' onclick='getCancelDetails(this.id)' style='" + delete1 + "'><i class='fa fa-times-circle'></i></button>&nbsp;<button type='button'  id='detail#" + data.replace(/\s+/g, '-') + "' data-toggle='tooltip' title='View Details' class='btn btn-success btn-sm' onclick='viewDetails(this.id)' style='" + view + "'><i class='fa fa-info-circle' aria-hidden='true'></i></button>";

                                else
                                    return "<button type='button'  id='detail#" + data.replace(/\s+/g, '-') + "' data-toggle='tooltip' title='View Details' class='btn btn-success btn-sm' onclick='viewDetails(this.id)' style='" + view + "'><i class='fa fa-info-circle' aria-hidden='true'></i></button>";

                            }
                        }
                            ]
                        });

                        $('#spinner').hide();
                    },
                    failure: function (response) {
                        //   alert(response);
                    },
                    error: function (response) {
                        //   console.log(response);
                    }
                });
        }
        else {
            $('#spinner').hide();

        }

        });

    function checkedallitem(e) {
        console.log(e.target.value);


        if (e.target.checked == true) {
            $('#itemsaleTable tbody input[type="checkbox"]:not(:checked)').trigger('click');
        } else {
            $('#itemsaleTable tbody input[type="checkbox"]:checked').trigger('click');
        }
    }
    </script>
    <script type="text/javascript">
        function BindState() {
            $('#<%=ListArea.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: 'Retailerdispatchorderform.aspx/BindState',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ListArea.ClientID %>"));
            }
            function PopulateControl(list, control) {
                //       console.log(list)

                $('#<%=ListArea.ClientID %>').empty();
                $.each(list, function () {

                    $('#<%=ListArea.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');
                      var id = $('#<%=Hidden1.ClientID%>').val();
                      //alert(id);
                      var splittedArray = id.split(",");
                      //  alert(splittedArray);

                      if (id != "") {
                          //alert('c');
                          control.val(splittedArray);
                      }

                  });
                  $("#<%=ListArea.ClientID %>").multiselect('rebuild');
                //    console.log($('#<%=ListArea.ClientID %> ul'));

            }
        }

        function Binddistributor() {
            var selectedState = [];

            $("#<%=ListArea.ClientID %> :selected").each(function () {
                selectedState.push($(this).val());
            });
            $("#<%=Hidden1.ClientID %>").val(selectedState);

            var obj = { CityId: $("#<%=Hidden1.ClientID %>").val() };

            $('#<%=ListBox1.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: 'Retailerdispatchorderform.aspx/BindDistributor',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(obj),
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ListBox1.ClientID %>"));
            }
            function PopulateControl(list, control) {
                //  console.log(list)

                $('#<%=ListBox1.ClientID %>').empty();
                $.each(list, function () {

                    $('#<%=ListBox1.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');
                    var id = $('#<%=hiddistributor.ClientID%>').val();
                    //alert(id);
                    var splittedArray = id.split(",");
                    //  alert(splittedArray);

                    if (id != "") {
                        //alert('c');
                        control.val(splittedArray);
                    }

                });
                $("#<%=ListBox1.ClientID %>").multiselect('rebuild');
                // console.log($('#<%=ListBox1.ClientID %> ul'));

            }
        }
        <%--    function BindCity() {

            var selectedState = [];

            $("#<%=ddlstate.ClientID %> :selected").each(function () {
                selectedState.push($(this).val());
            });
            $("#<%=hidstate.ClientID %>").val(selectedState);

            var obj = { StateID: $("#<%=hidstate.ClientID %>").val() };
            $('#<%=ListBox1.ClientID %>').empty();
            $('#<%=ddlcity.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: 'Retailerdispatchorderform.aspx/PopulateCityByMultiState',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj),
                dataType: "json",
                success: OnPopulated,
                failure: function (response) {
                    alert(response.d);
                }
            });
            function OnPopulated(response) {
                PopulateControl(response.d, $("#<%=ddlcity.ClientID %>"));
            }
            function PopulateControl(list, control) {
                // console.log(list)

                $('#<%=ddlcity.ClientID %>').empty();
                  $.each(list, function () {

                      $('#<%=ddlcity.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');
                    var id = $('#<%=hidcity.ClientID%>').val();
                    //alert(id);
                    var splittedArray = id.split(",");
                    //  alert(splittedArray);

                    if (id != "") {
                        //alert('c');
                        control.val(splittedArray);
                    }

                });
                $("#<%=ddlcity.ClientID %>").multiselect('rebuild');


              }
          }--%>


        function Bindretailer() {
            var selectedState = [];

            $("#<%=ListArea.ClientID %> :selected").each(function () {
                selectedState.push($(this).val());
            });
            $("#<%=Hidden1.ClientID %>").val(selectedState);

           // alert( $("#<%=Hidden1.ClientID %>").val());

            var selectedValues = [];
            $("#<%=ListBox1.ClientID %> :selected").each(function () {
                selectedValues.push($(this).val());
            });
            $("#<%=hiddistributor.ClientID%>").val(selectedValues);

          //  alert($("#<%=hiddistributor.ClientID %>").val());
            
            //var obj = { AreaId: $("#<%=Hidden1.ClientID %>").val(), DisId: $("#<%=hiddistributor.ClientID %>").val() };

            $('#<%=ListBox2.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');

        
            $.post('<%=ResolveUrl("~/and_sync.asmx/BindRetailer")%>', {"AreaId": $("#<%=Hidden1.ClientID %>").val(), "DistId": $("#<%=hiddistributor.ClientID %>").val()  }
            , function (result) {
                $('#<%=ListBox2.ClientID %>').empty();
                console.log(JSON.parse(result))

                var data = JSON.parse(result);
                for (var i = 0; i < data.length; i++) {
                    $('#<%=ListBox2.ClientID %>').append('<option  value=' + data[i]['Id'] + '>' + data[i]['Name'] + '</option>');

                  }

                var id = $('#<%=hidretailer.ClientID%>').val();
                //alert(id);
                var splittedArray = id.split(",");
                //  alert(splittedArray);

                if (id != "") {
                    //alert('c');
                    control.val(splittedArray);
                }

                $("#<%=ListBox2.ClientID %>").multiselect('rebuild');
                // console.log($('#<%=ListBox1.ClientID %> ul'));

        })

          <%--    $.ajax({
                 type: "POST",
                 url: 'Retailerdispatchorderform.aspx/BindRetailer',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 data: JSON.stringify({ "AreaId": $("#<%=Hidden1.ClientID %>").val(), "DistId": $("#<%=hiddistributor.ClientID %>").val() }),//JSON.stringify(obj),
                 success: OnPopulated,
                 failure: function (response) {
                     alert(response.d);
                 }
             });
             function OnPopulated(response) {
                 PopulateControl(response.d, $("#<%=ListBox2.ClientID %>"));
            }
            function PopulateControl(list, control) {
                console.log(list)

                $('#<%=ListBox2.ClientID %>').empty();
            <%--  $.each(list, function () {

                    $('#<%=ListBox2.ClientID %>').append('<option  value=' + this['Value'] + '>' + this['Text'] + '</option>');
                    var id = $('#<%=hidretailer.ClientID%>').val();
                    //alert(id);
                    var splittedArray = id.split(",");
                    //  alert(splittedArray);

                    if (id != "") {
                        //alert('c');
                        control.val(splittedArray);
                    }

              });--%>



<%--                for (var i = 0; i < list.length; i++) {
                    $('#<%=ListBox2.ClientID %>').append('<option  value=' + list[i]['Value'] + '>' + list[i]['Text'] + '</option>');
                
                }

                var id = $('#<%=hidretailer.ClientID%>').val();
                //alert(id);
                var splittedArray = id.split(",");
                //  alert(splittedArray);

                if (id != "") {
                    //alert('c');
                    control.val(splittedArray);
                }

                $("#<%=ListBox2.ClientID %>").multiselect('rebuild');
                // console.log($('#<%=ListBox1.ClientID %> ul'));

            }--%>
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
            width: 100px; /* width of the spinner gif */
            height: 102px; /*hight of the spinner gif +2px to fix IE8 issue */
        }

        }
    </style>
    <script type="text/javascript">
        <%--   function checkDate(sender, args) {
            const month = date.toLocaleString('default', { month: 'long' });
            var selecteddate;
            if (sender._selectedDate.getMonth()<10)
                 selecteddate = sender._selectedDate.getDate().toString() + "/0" + (sender._selectedDate.getMonth() + 1) + "/" + sender._selectedDate.getFullYear();
            else
                selecteddate = sender._selectedDate.getDate().toString() + "/" + (sender._selectedDate.getMonth() + 1) + "/" + sender._selectedDate.getFullYear();
            var seldate = new Date(selecteddate)
            if ($('#<%=txttodate.ClientID%>').val() != "") {
                var date = new Date($('#<%=txttodate.ClientID%>').val())
                console.log(date)
                if (seldate > date) {
                    errormessage("From Date Should be less than To date");
                    sender._selectedDate = new Date();
                    sender._textbox.set_Value(sender._selectedDate.format(sender._format))
                }
            }
        }
        function checkDate1(sender, args) {
            if ($('#<%=txtfmDate.ClientID%>').val() != "") {
                var date = new Date($('#<%=txtfmDate.ClientID%>').val())
                if (sender._selectedDate < date) {
                    errormessage("To Date Should be greater than To date");
                    sender._selectedDate = new Date();
                    sender._textbox.set_Value(sender._selectedDate.format(sender._format))
                }
            }
           }--%>
        function btnSubmitfunc1() {


            var selectedValues = [];
            $("#<%=ListBox1.ClientID %> :selected").each(function () {
                selectedValues.push($(this).val());
            });
            $("#<%=hiddistributor.ClientID%>").val(selectedValues);

            var selectedRetValues = [];
            $("#<%=ListBox2.ClientID %> :selected").each(function () {
                 selectedRetValues.push($(this).val());
             });
            $("#<%=hidretailer.ClientID%>").val(selectedRetValues);

            // validate($("#hiddistributor").val());
            if (selectedValues == "") {
                errormessage("Please Select Distributor");
                return false;
            }

        }
        function btnSubmitfunc() {

            // $("#hiddistributor").val(selectedvalue);
            var selectedValues = [];
            $("#<%=ListBox1.ClientID %> :selected").each(function () {
                selectedValues.push($(this).val());
            });
            $("#<%=hiddistributor.ClientID%>").val(selectedValues);

            var selectedRetValues = [];
            $("#<%=ListBox2.ClientID %> :selected").each(function () {
                selectedRetValues.push($(this).val());
            });
            $("#<%=hidretailer.ClientID%>").val(selectedRetValues);





            if (selectedValues == "") {
                errormessage("Please Select Distributor");
                return false;
            }
            var Todate = new Date($('#<%=txttodate.ClientID%>').val())
            var Fromdate = new Date($('#<%=txtfmDate.ClientID%>').val())
            if (Fromdate > Todate) {
                errormessage("From Date Should be less than To Date");
                return false;
            }
      

            loding();
            BindGridView();
        }


        function ExportFunction() {
            var selectedValues = [];
            $("#<%=ListBox1.ClientID %> :selected").each(function () {
                selectedValues.push($(this).val());
            });
            $("#<%=hiddistributor.ClientID%>").val(selectedValues);


            $.ajax({
                type: "POST",
                url: "And_sync.asmx/ExportCSV",
                data: '{Distid: "' + $("#<%=hiddistributor.ClientID%>").val() + '", Fromdate: "' + $('#<%=txtfmDate.ClientID%>').val() + '",Todate: "' + $('#<%=txttodate.ClientID%>').val() + '",RetId:"' + $('#<%=hidretailer.ClientID%>').val() + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
                contentType: "application/json;",
                responseType: "json",
                success: OnSuccess,
                failure: function (response) {
                    //   alert(response);
                },
                error: function (response) {
                    //   console.log(response);
                }
            });
        }

        function Cancelfun() {
            window.location.href = "Retailerdispatchorderform.aspx";
        }

        function BindGridView() {
            $.ajax({
                type: "POST",
                url: "And_sync.asmx/Getretailerorders_new",
                data: '{Distid: "' + $("#<%=hiddistributor.ClientID%>").val() + '", Fromdate: "' + $('#<%=txtfmDate.ClientID%>').val() + '",Todate: "' + $('#<%=txttodate.ClientID%>').val() + '",Status:"' + $('#<%=ddlstatus.ClientID%>').val() + '",RetId:"' + $('#<%=hidretailer.ClientID%>').val() + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
                contentType: "application/json;",
                responseType: "json",
                success: OnSuccess,
                failure: function (response) {
                    //   alert(response);
                },
                error: function (response) {
                    //   console.log(response);
                }
            });
        }



        function OnSuccess(response) {
            //     console.log(response);
            //  alert(JSON.stringify(response.d));
            //alert(response.d);
            $('div[id$="rptmain"]').show();
            console.log("Grid fill start");
            var data = JSON.parse(response.d);
            console.log(data);
            //alert(data);
            //var arr1 = data.length;
            //alert(arr1);
            var table = $('#ContentPlaceHolder1_rptmain table').DataTable();
            table.destroy();
            $("#ContentPlaceHolder1_rptmain table ").DataTable({
                "order": [[0, "desc"]],


                "aaData": data,
                "aoColumns": [

         {

             "render": function (data, type, row, meta) {

                 return '<input type="checkbox" class="call-checkbox"  >';
             }


         }, {
             "mData": "Docid",
             "render": function (data, type, row, meta) {
                 if (row.Status == "Pending") {
                     return $('<div>')
                            .attr('class', 'text-left')
                         .attr('style', 'color:blue;font-weight: bold;')
                            .text(data)
                            .wrap('<div></div>')
                            .parent()
                            .html();
                 }
                 else if (row.Status == "Cancelled") {
                     return $('<div>')
                           .attr('class', 'text-left')
                        .attr('style', 'color:red;font-weight: bold;')
                           .text(data)
                           .wrap('<div></div>')
                           .parent()
                           .html();
                 }
                 else {
                     return $('<div>')
                         .attr('class', 'text-left')
                      .attr('style', 'color:green;font-weight: bold;')
                         .text(data)
                         .wrap('<div></div>')
                         .parent()
                         .html();
                 }
             }





         },
        { "mData": "PartyName" },
      //  { "mData": "Address" },
          { "mData": "Mobile" },
              {
                  "mData": "Distributor", "render": function (data, type, row, meta) {
                      if (type === 'display') {
                          return $('<div>')
                             .attr('style', ' white-space:normal;width:200px')
                             .text(data)
                             .wrap('<div></div>')
                             .parent()
                             .html();

                      } else {
                          return data;
                      }
                  }
              },
               { "mData": "SMName" },
        {
            //"mData": "orderdate1",
            "mData": "mobiledate",

            //"render": function (data, type, row, meta) {

            //    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            //    var date = new Date(data);
            //    var day = date.getDate();
            //    var month = date.getMonth();
            //    var year = date.getFullYear();

            //    var mname = monthNames[date.getMonth()]

            //    var fdate = day + '/' + mname + '/' + year;

            //    if (type === 'display') {
            //        return $('<div>')
            //           .attr('class', 'text')
            //           .text(fdate)
            //           .wrap('<div></div>')
            //           .parent()
            //           .html();

            //    } else {
            //        return data;
            //    }
            //}



        }, // <-- which values to use inside object


            {
                "mData": "totalqty",
                "render": function (data, type, row, meta) {
                    if (type === 'display') {
                        return $('<div>')
                           .attr('class', 'text-right')
                           .text(data)
                           .wrap('<div></div>')
                           .parent()
                           .html();

                    } else {
                        return data;
                    }
                }
            },
                {
                    "mData": "totalamount",
                    "render": function (data, type, row, meta) {
                        if (type === 'display') {
                            return $('<div>')
                               .attr('class', 'text-right')
                               .text(data)
                               .wrap('<div></div>')
                               .parent()
                               .html();

                        } else {
                            return data;
                        }
                    }
                },
          //{ "mData": "Status" },
            {
                "mData": "Docid",
                "render": function (data, type, row, meta) {
                    //     console.log(row);
                    //     console.log(data);
                    //     console.log(meta);
                    //var did = row.DocId;
                    //console.log(did);
                    var view;
                    var add;
                    var delete1;
                    if ($('#<%=hidviewpermission.ClientID%>').val() == "true") {
                        view = "visibility: visible;"
                    }
                    else {
                        view = "visibility: hidden;"

                    }


                    if ($('#<%=hidaddpermission.ClientID%>').val() == "true") {
                        add = "visibility: visible;"
                    }
                    else {
                        add = "visibility: hidden;"

                    }
                    if ($('#<%=hidaddpermission.ClientID%>').val() == "true") {
                        delete1 = "visibility: visible;"
                    }
                    else {
                        delete1 = "visibility: hidden;"

                    }
                    if (row.Status == "Pending")
                        return "<button type='button' id='dispatch#" + data.replace(/\s+/g, '-') + "'  data-toggle='tooltip' title='Dispatch' class='btn btn-primary btn-sm' onclick='getDispatchDetails(this.id)' style='" + add + "'><i class='fa fa-truck' aria-hidden='true'></i></button>&nbsp;<button type='button'  id='Cancel#" + data.replace(/\s+/g, '-') + "'  data-toggle='tooltip' title='Cancel' class='btn btn-danger btn-sm' onclick='getCancelDetails(this.id)' style='" + delete1 + "'><i class='fa fa-times-circle'></i></button>&nbsp;<button type='button'  id='detail#" + data.replace(/\s+/g, '-') + "' data-toggle='tooltip' title='View Details' class='btn btn-success btn-sm' onclick='viewDetails(this.id)' style='" + view + "'><i class='fa fa-info-circle' aria-hidden='true'></i></button>";

                    else
                        return "<button type='button'  id='detail#" + data.replace(/\s+/g, '-') + "' data-toggle='tooltip' title='View Details' class='btn btn-success btn-sm' onclick='viewDetails(this.id)' style='" + view + "'><i class='fa fa-info-circle' aria-hidden='true'></i></button>";

                }
            }
                ]
            });
            console.log("Grid fill stop");
        $('#spinner').hide();
    }


    function getDispatchDetails(a) {
        console.log(a);
        var docid = a.split("#")
        console.log(docid);
        $('#hiddocid').val(docid[1]);
        loding();
        BindModeldata(docid[1])



        // document.getElementById("ContentPlaceHolder1_pnlpopup").style.display = "block";
        //alert("D");
        // document.getElementById("divpanelpopup").style.display = "block";
        //document.getElementById("ContentPlaceHolder1_pnlpopup").style.height = "550px";


    }
    function BindModeldata(docid) {
        $.ajax({
            type: "POST",
            url: "And_sync.asmx/Getretailerorderdetail",
            data: '{docid: "' + docid + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
            contentType: "application/json;",
            responseType: "json",
            success: OnSuccess1,
            failure: function (response) {
                //   alert(response);
            },
            error: function (response) {
                console.log(response);
            }
        });
    }
    function OnSuccess1(response) {
        //  console.log(response);
        //  alert(JSON.stringify(response.d));
        //alert(response.d);
        $('div[id$="Div2"]').show();
        var data = JSON.parse(response.d);
        console.log(data);

        $('#<%=distname.ClientID%>').text(data[0]["PartyName"]);
      $('#<%=lblRemarkViwPunch.ClientID%>').text(data[0]["remarks"]);
      $('#<%=vdate.ClientID%>').text(data[0]["vdate"]);
      $('#<%=Label8.ClientID%>').text(data[0]["Address"]);
      //alert(data);
      //var arr1 = data.length;
      //alert(arr1);
      var table1 = $('#Div2 table').DataTable();
      table1.destroy();



      $('#Div2 table').DataTable({


          "footerCallback": function (row, data, start, end, display) {
              var api = this.api(), data;
              console.log(api
                  .column(11)
                  .data())


              // Total over all pages
              //total = api
              //    .column(11)
              //    .data()
              //    .reduce( function (a, b) {
              //        return a + b;
              //    }, 0 );

              // Total over this page
              dispatchqtyTotal = api
                  .column(11, { page: 'current' })
                  .data()
                  .reduce(function (a, b) {
                      return a + b;
                  }, 0);

              NetAmountTotal = api
               .column(10, { page: 'current' })
               .data()
               .reduce(function (a, b) {
                   return a + b;
               }, 0);

              OrderAmountTotal = api
             .column(9, { page: 'current' })
             .data()
             .reduce(function (a, b) {
                 return a + b;
             }, 0);


              disAmountTotal = api
           .column(8, { page: 'current' })
           .data()
           .reduce(function (a, b) {
               return a + b;
           }, 0);
              $(api.column(8).footer()).html(
                                 disAmountTotal.toFixed(2) //+' ( $'+ total +' total)'
                             );

              $(api.column(9).footer()).html(
                                OrderAmountTotal.toFixed(2) //+' ( $'+ total +' total)'
                            );

              $(api.column(10).footer()).html(
                                   NetAmountTotal.toFixed(2) //+' ( $'+ total +' total)'
                               );
              // Update footer
              $(api.column(11).footer()).html(
                  dispatchqtyTotal.toFixed(2) //+' ( $'+ total +' total)'
              );
          },


          "order": [[0, "desc"]],

          "aaData": data,
          "paging": false,
          "searching": false,
          "info": false,

          //scrollY:        "300px",
          //scrollX:        true,
          //scrollCollapse: true,
          //paging:         false,
          //"columnDefs": [
             
          //],
          "fixedColumns": true,
          "aoColumns": [


{
    "mData": "DocId"
},
{
    "mData": "ItemId"
},
  {
      "mData": "ItemName", "render": function (data, type, row, meta) {
          if (type === 'display') {
              return $('<div>')
                 .attr('style', ' white-space:normal;')
                 .text(data)
                 .wrap('<div></div>')
                 .parent()
                 .html();

          } else {
              return data;
          }
      }
  },

      //{
      //    "mData": "OrderQty",
      //    "render": function (data, type, row, meta) {
      //        if (type === 'display') {
      //            return $('<div>')
      //               .attr('class', 'text-right')
      //               .text(data)
      //               .wrap('<div></div>')
      //               .parent()
      //               .html();

      //        } else {
      //            return data;
      //        }
      //    }
      //},
      {
          "mData": "QtyDescription",
          "render": function (data, type, row, meta) {
              if (type === 'display') {
                  return $('<div>')
                     .attr('class', 'text-right')
                     .text(data)
                     .wrap('<div></div>')
                     .parent()
                     .html();

              } else {
                  return data;
              }
          }
      },
          {
              "mData": "rate",
              "render": function (data, type, row, meta) {
                  if (type === 'display') {
                      return $('<div>')
                         .attr('class', 'text-right')
                         .text(data)
                         .wrap('<div></div>')
                         .parent()
                         .html();

                  } else {
                      return data;
                  }
              }
          },

          {
              "mData": "Margin",
              "render": function (data, type, row, meta) {
                  if (type === 'display') {
                      return $('<div>')
                         .attr('class', 'text-right')
                         .text(data)
                         .wrap('<div></div>')
                         .parent()
                         .html();

                  } else {
                      return data;
                  }
              }
          },
               {
                   "mData": "Discount",
                   "render": function (data, type, row, meta) {
                       if (type === 'display') {
                           return $('<div>')
                              .attr('class', 'text-right')
                              .text(data)
                              .wrap('<div></div>')
                              .parent()
                              .html();

                       } else {
                           return data;
                       }
                   }
               },
               {
                   "mData": "DiscountType",
                   "render": function (data, type, row, meta) {
                       if (type === 'display') {
                           return $('<div>')
                             .attr('style', ' white-space:normal;')
                             .text(data)
                             .wrap('<div></div>')
                             .parent()
                             .html();

                       } else {
                           return data;
                       }
                   }
               },

               {
                   "mData": "DiscountAmount",
                   "render": function (data, type, row, meta) {
                       if (type === 'display') {
                           return $('<div>')
                              .attr('class', 'text-right')
                              .text(data)
                              .wrap('<div></div>')
                              .parent()
                              .html();

                       } else {
                           return data;
                       }
                   }
               },
                 {
                     "mData": "OrderAmount",
                     "render": function (data, type, row, meta) {
                         if (type === 'display') {
                             return $('<div>')
                                .attr('class', 'text-right')
                                .text(data)
                                .wrap('<div></div>')
                                .parent()
                                .html();

                         } else {
                             return data;
                         }
                     }
                 },
                 {
                     "mData": "NetAmount",
                     "render": function (data, type, row, meta) {
                         if (type === 'display') {
                             return $('<div>')
                                .attr('class', 'text-right')
                                .text(data)
                                .wrap('<div></div>')
                                .parent()
                                .html();

                         } else {
                             return data;
                         }
                     }
                 },


      {
          "mData": "OrderQty",
          "render": function (data, type, row, meta) {

              console.log(row);
              //console.log(data);
              console.log(meta.row);
              //var did = row.DocId;
              //console.log(did);

              return "<input type='text'  id='row#" + meta.row + "' onfocus='this.select()' onchange='checkVal(this.id)' onkeypress='return isNumber(event)' MaxLength='12'  Class='form-control numeric text-right' Value='" + data + "'/>";


          }
      }
          ],
          "columnDefs": [
   {
       "targets": [0],
       "visible": false,

   },
           {
               "targets": [1],
               "visible": false,

           }, { "width": 200, "targets": [2] }],
      });

      $('#spinner').hide();
      $('#myModal').modal('show');
  }
  function getCancelDetails(a) {

      console.log(a);
      var docid = a.split("#")
      console.log(docid);
      $('#hiddocid').val(docid[1]);

      $.ajax({
          type: "POST",
          url: "And_sync.asmx/Getretailerorderdetail",
          data: '{docid: "' + $('#hiddocid').val() + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
              contentType: "application/json;",
              responseType: "json",
              success: function (response) {
                  var data = JSON.parse(response.d);
                  console.log(data);

                  $('#<%=Label1.ClientID%>').text(data[0]["PartyName"]);
                    $('#<%=lblRemarkViewOrderCancel.ClientID%>').text(data[0]["remarks"]);
                    $('#<%=Label2.ClientID%>').text(data[0]["vdate"]);


                    $('#<%=Label10.ClientID%>').text(data[0]["Address"]);

                    var table2 = $('#Div13 table').DataTable();
                    table2.destroy();



                    $("#Div13 table ").DataTable({


                        "footerCallback": function (row, data, start, end, display) {
                            var api = this.api(), data;
                            console.log(api
                                .column(11)
                                .data())


                            // Total over all pages
                            //total = api
                            //    .column(11)
                            //    .data()
                            //    .reduce( function (a, b) {
                            //        return a + b;
                            //    }, 0 );

                            // Total over this page
                            dispatchqtyTotal = api
                                .column(11, { page: 'current' })
                                .data()
                                .reduce(function (a, b) {
                                    return a + b;
                                }, 0);

                            NetAmountTotal = api
                             .column(10, { page: 'current' })
                             .data()
                             .reduce(function (a, b) {
                                 return a + b;
                             }, 0);

                            OrderAmountTotal = api
                           .column(9, { page: 'current' })
                           .data()
                           .reduce(function (a, b) {
                               return a + b;
                           }, 0);


                            disAmountTotal = api
                         .column(8, { page: 'current' })
                         .data()
                         .reduce(function (a, b) {
                             return a + b;
                         }, 0);
                            $(api.column(8).footer()).html(
                                               disAmountTotal.toFixed(2) //+' ( $'+ total +' total)'
                                           );

                            $(api.column(9).footer()).html(
                                              OrderAmountTotal.toFixed(2) //+' ( $'+ total +' total)'
                                          );

                            $(api.column(10).footer()).html(
                                                 NetAmountTotal.toFixed(2) //+' ( $'+ total +' total)'
                                             );
                            // Update footer
                            $(api.column(11).footer()).html(
                                dispatchqtyTotal.toFixed(2) //+' ( $'+ total +' total)'
                            );
                        },


                        "order": [[0, "desc"]],
                        "columnDefs": [
                  {
                      "targets": [0],
                      "visible": false,

                  },
                          {
                              "targets": [1],
                              "visible": false,

                          }, { "width": 200, "targets": [2] }],
                        "aaData": data,
                        "paging": false,
                        "searching": false,
                        "info": false,
                        "aoColumns": [


         {
             "mData": "DocId",

         },
         {
             "mData": "ItemId"
         },
                {
                    "mData": "ItemName", "render": function (data, type, row, meta) {
                        if (type === 'display') {
                            return $('<div>')
                               .attr('style', ' white-space:normal;')
                               .text(data)
                               .wrap('<div></div>')
                               .parent()
                               .html();

                        } else {
                            return data;
                        }
                    }
                },

                    //{
                    //    "mData": "OrderQty",
                    //    "render": function (data, type, row, meta) {
                    //        if (type === 'display') {
                    //            return $('<div>')
                    //               .attr('class', 'text-right')
                    //               .text(data)
                    //               .wrap('<div></div>')
                    //               .parent()
                    //               .html();

                    //        } else {
                    //            return data;
                    //        }
                    //    }
                    //},
                    {
                        "mData": "QtyDescription",
                        "render": function (data, type, row, meta) {
                            if (type === 'display') {
                                return $('<div>')
                                   .attr('class', 'text-right')
                                   .text(data)
                                   .wrap('<div></div>')
                                   .parent()
                                   .html();

                            } else {
                                return data;
                            }
                        }
                    },
                        {
                            "mData": "rate",
                            "render": function (data, type, row, meta) {
                                if (type === 'display') {
                                    return $('<div>')
                                       .attr('class', 'text-right')
                                       .text(data)
                                       .wrap('<div></div>')
                                       .parent()
                                       .html();

                                } else {
                                    return data;
                                }
                            }
                        },
                        {
                            "mData": "Margin",
                            "render": function (data, type, row, meta) {
                                if (type === 'display') {
                                    return $('<div>')
                                       .attr('class', 'text-right')
                                       .text(data)
                                       .wrap('<div></div>')
                                       .parent()
                                       .html();

                                } else {
                                    return data;
                                }
                            }
                        },
                             {
                                 "mData": "Discount",
                                 "render": function (data, type, row, meta) {
                                     if (type === 'display') {
                                         return $('<div>')
                                            .attr('class', 'text-right')
                                            .text(data)
                                            .wrap('<div></div>')
                                            .parent()
                                            .html();

                                     } else {
                                         return data;
                                     }
                                 }
                             },
                             {
                                 "mData": "DiscountType",
                                 "render": function (data, type, row, meta) {
                                     if (type === 'display') {
                                         return $('<div>')
                                           .attr('style', ' white-space:normal;')
                                           .text(data)
                                           .wrap('<div></div>')
                                           .parent()
                                           .html();

                                     } else {
                                         return data;
                                     }
                                 }
                             },
                             {
                                 "mData": "DiscountAmount",
                                 "render": function (data, type, row, meta) {
                                     if (type === 'display') {
                                         return $('<div>')
                                            .attr('class', 'text-right')
                                            .text(data)
                                            .wrap('<div></div>')
                                            .parent()
                                            .html();

                                     } else {
                                         return data;
                                     }
                                 }
                             },
                               {
                                   "mData": "OrderAmount",
                                   "render": function (data, type, row, meta) {
                                       if (type === 'display') {
                                           return $('<div>')
                                              .attr('class', 'text-right')
                                              .text(data)
                                              .wrap('<div></div>')
                                              .parent()
                                              .html();

                                       } else {
                                           return data;
                                       }
                                   }
                               },
                               {
                                   "mData": "NetAmount",
                                   "render": function (data, type, row, meta) {
                                       if (type === 'display') {
                                           return $('<div>')
                                              .attr('class', 'text-right')
                                              .text(data)
                                              .wrap('<div></div>')
                                              .parent()
                                              .html();

                                       } else {
                                           return data;
                                       }
                                   }
                               },


                    {
                        "mData": "DispatchQty",
                        "render": function (data, type, row, meta) {
                            if (type === 'display') {
                                return $('<div>')
                                   .attr('class', 'text-right')
                                //    .attr('style', 'width:120px')
                                   .text(data)
                                   .wrap('<div></div>')
                                   .parent()
                                   .html();

                            } else {
                                return data;
                            }
                        }
                    }
                        ]
                    });



                    $('#myModal1').modal('show');
                },
              failure: function (response) {
                  //   alert(response);
              },
              error: function (response) {
                  console.log(response);
              }
          });



        }


        function convertTableToArrayObject() {
            var idetails = new Array();
           
            var itemObjects = [];
            var table = $('#example1').DataTable();

            var data = table.rows().data();

            for (var i = 0; i < data.length; i++) {
                var itemd = new Object();
              

                itemd["itemid"] = data[i]["ItemId"];
                itemd["Dispatchqty"] = table.cell(i, 11).nodes().to$().find('input').val();// data[i][7];
                            
                if (itemd["Dispatchqty"] == '') {                   
                    idetails = [];
                    table.cell(i, 12).nodes().to$().find('input').focus();
                    return idetails
                }
                itemd["userid"] = '<%=BusinessLayer.Settings.Instance.UserID%>';
              itemd["Docid"] = $('#hiddocid').val();
              itemd["Remark"] = $('#TextArea1').val();            

              idetails[i] = itemd;             
          }

          return idetails;

          //var heads = [];
          //$("#example thead").find("th").each(function () {
          //    heads.push($(this).text().trim());
          //});
          //var rows = [];
          //$("#example tbody tr").each(function () {
          //    cur = {};
          //    $(this).find("td").each(function(i, v) {
          //        cur[heads[i]] = $(this).text().trim();
          //    });
          //    rows.push(cur);
          //    cur = {};
          //});

          //return rows;
      }
      function dispatchorder() {
          if ($('#TextArea1').val() == "") {
              errormessage("Please enter Remark");
              return false;
          }
          var itemdetails = convertTableToArrayObject();         
          if (itemdetails.length == 0) {
              errormessage("Please Fill dispatchqty ");
              return false;
          }        
          var idetails1 = JSON.stringify(itemdetails);

              $.ajax({
              type: "POST",
              url: "And_sync.asmx/Dispatchretailerorder",
              data: JSON.stringify({ "orderdetail": itemdetails }),
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
                contentType: "application/json;",
                responseType: "json",
                success: function (response) {
                    var data = JSON.parse(response.d);
                    if (data.ResultMsg == "Success") {
                        Successmessage("Order dispatched Successfully")
                        $('#TextArea1').val("");
                        var table = $('#Div2 table').DataTable();
                        table.destroy();

                        $('#myModal').modal('hide');
                        BindGridView();
                    }
                    else {
                        errormessage("Something went wrong");
                    }
                },
                failure: function (response) {
                    //   alert(response);
                },
                error: function (response) {
                    console.log(response);
                }
            });
        }
        function Cancelrder() {
            if ($('#TextArea2').val() == "") {
                errormessage("Please enter Remark");
                return false;
            }


            $.ajax({
                type: "POST",
                url: "And_sync.asmx/Cancelretailerorder",
                data: '{docid: "' + $('#hiddocid').val() + '" , remark:"' + $('#TextArea2').val() + '",  userid:"' + '<%=BusinessLayer.Settings.Instance.UserID %>' + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
              contentType: "application/json;",
              responseType: "json",
              success: function (response) {
                  var data = JSON.parse(response.d);
                  if (data.ResultMsg == "Success") {
                      Successmessage("Order cancelled Successfully")
                      $('#myModal1').modal('hide');
                      BindGridView();
                  }
                  else {
                      errormessage("Something went wrong");
                  }
              },
              failure: function (response) {
                  //   alert(response);
              },
              error: function (response) {
                  console.log(response);
              }
          });
      }

    </script>
    <script type="text/javascript">
        $("[id$=btnExport]").click(function (e) {
            // alert("123");
            window.open('data:application/vnd.ms-excel,' + encodeURIComponent($('div[id$=rptmain]').html()));
            e.preventDefault();
        });
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
        <div class="box-body">
            <!-- left column -->
            <!-- general form elements -->
            <div class="box box-primary">
                <div class="row">
                    <!-- left column -->
                    <div class="col-md-12">
                        <div id="InputWork">
                            <!-- general form elements -->
                            <input type="hidden" id="hidaddpermission" runat="server" />
                            <input type="hidden" id="hiddeletepermission" runat="server" />
                            <input type="hidden" id="hidviewpermission" runat="server" />
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                <%--    <h3 class="box-title">Retailer Dispatch Order</h3>--%>
                                    <h3 class="box-title"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></h3>
                                </div>
                                <input type="hidden" id="hiddocid" />
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="row">

                                        <%-- <div class="col-md-3 col-sm-6 col-xs-7" hidden>
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Sales Person:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>  
                                                <asp:ListBox ID="salespersonListBox" runat="server" SelectionMode="Multiple"
                                                    OnSelectedIndexChanged="salespersonListBox_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                            </div>
                                        </div>        --%>


                                        <%--    <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">State:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red; display: none;">*</label>
                                                <asp:ListBox ID="ddlstate" runat="server" SelectionMode="Multiple" onChange="BindCity();"></asp:ListBox>
                                                <input type="hidden" id="hidstate" runat="server" />

                                            </div>

                                        </div>--%>
                                        <div class="col-md-3 col-sm-6 col-xs-10">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Area:</label><br />
                                                <asp:ListBox ID="ListArea" runat="server" SelectionMode="Multiple" onChange="Binddistributor();"></asp:ListBox>
                                                <input type="hidden" id="Hidden1" runat="server" />
                                            </div>
                                        </div>
                                        <%--   <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">City:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red; display: none;">*</label>
                                                <asp:ListBox ID="ddlcity" runat="server" SelectionMode="Multiple" onChange="Binddistributor();"></asp:ListBox>
                                                <input type="hidden" id="hidcity" runat="server" />

                                            </div>

                                        </div>--%>
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Distributor name:</label>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>
                                                <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple" onChange="Bindretailer();"></asp:ListBox>
                                                <input type="hidden" id="Hidden2" runat="server" />
                                                <input type="hidden" id="hiddistributor" runat="server" />
                                                <input type="hidden" id="hidproductgroup" />
                                                <input type="hidden" id="hidproduct" />
                                            </div>

                                        </div>

                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Retailer name:</label>
                                                <asp:ListBox ID="ListBox2" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                <input type="hidden" id="hidretailer" runat="server" />
                                            </div>

                                        </div>

                                        <div class="col-md-2 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Status:</label>
                                                <asp:DropDownList ID="ddlstatus" runat="server" CssClass="form-control ">
                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                    <asp:ListItem Value="Pending">Pending </asp:ListItem>
                                                    <asp:ListItem Value="Dispatched">Dispatched</asp:ListItem>
                                                    <asp:ListItem Value="Cancelled">Cancelled</asp:ListItem>

                                                </asp:DropDownList>

                                            </div>

                                        </div>



                                        <div class="col-md-1 col-sm-6 col-xs-12">
                                            <i class="fa fa-square" aria-hidden="true" style="color: green"></i>&nbsp;&nbsp;Dispatched<br />
                                            <i class="fa fa-square" aria-hidden="true" style="color: red"></i>&nbsp;&nbsp;Cancelled<br />
                                            <i class="fa fa-square" aria-hidden="true" style="color: blue"></i>&nbsp;&nbsp;Pending<br />
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <%--  <div class="row">
                                        <%-- <div class="col-md-8 col-sm-11">
                                        
                                        
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1" style="display:none">Product Group:</label>                                                
                                                <asp:ListBox ID="matGrpListBox" runat="server" SelectionMode="Multiple"
                                                    OnSelectedIndexChanged="matGrpListBox_SelectedIndexChanged" AutoPostBack="true" Visible="false"></asp:ListBox>
                                            </div>
                                        </div>
                                        <div class="col-md-1"></div>
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1" style="display:none">Product:</label>                                               
                                                <asp:ListBox ID="productListBox" runat="server" SelectionMode="Multiple" Visible="false"></asp:ListBox>
                                            </div>
                                        </div>
                                        <%-- </div>
                                    </div>--%>
                                    <div class="clearfix"></div>
                                    <div class="row">
                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div id="DIV1" class="form-group">
                                                <label for="exampleInputEmail1">From Date:</label>
                                                <asp:TextBox ID="txtfmDate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txtfmDate" runat="server" />
                                            </div>
                                        </div>

                                        <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">To Date:</label>
                                                <asp:TextBox ID="txttodate" runat="server" Width="100%" CssClass="form-control" Style="background-color: white;"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" CssClass="orange" Format="dd/MMM/yyyy" TargetControlID="txttodate" runat="server" />
                                            </div>
                                        </div>

                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                                <div class="box-footer">
                                    <input type="button" runat="server" onclick="btnSubmitfunc();" class="btn btn-primary" value="Go" />


                                    <input type="button" runat="server" onclick="Cancelfun();" class="btn btn-primary" value="Cancel" />

                                    <%--<input type="button" runat="server" onclick="ExportFunction();" class="btn btn-primary" value="Export" />--%>
                                    <%--<asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Cancel_Click" />--%>
                                    <%--      <asp:Button type="button" ID="btnGo" runat="server" Text="Go" class="btn btn-primary" OnClientClick="loding()"  
                                         OnClick="btnGo_Click" Visible="true" />--%>
                                    <%--<asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Cancel_Click" />--%>
                                    <%-- <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary"
                                        OnClick="btnExport_Click" />--%>
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClientClick="javascript:return btnSubmitfunc1();"
                                        OnClick="ExportCSV" />
                                    <%-- <input style="margin-right: 5px;" type="button" id="Go" value="Go" class="btn btn-primary" onc onclick="GetReport();" />--%>
                                </div>
                                <br />
                            </div>
                        </div>
                        <div id="rptmain" runat="server" style="display: none;">
                            <div class="box-header table-responsive">
                                <asp:Repeater ID="distitemsalerpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="itemsaleTable" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <th style="text-align: left; width: 10%">
                                                        <input name="select_all" type="checkbox" onclick="checkedallitem(event)"></th>
                                                    <th style="text-align: left; width: 20%">DocId</th>
                                                    <th style="text-align: left; width: 10%">Party Name</th>
                                                    <%--<th style="text-align: left; width: 20%">Address</th>--%>
                                                    <th style="text-align: left; width: 20%">Mobile</th>
                                                    <th>Distributor</th>
                                                    <th style="text-align: left; width: 20%">Sales Person</th>
                                                    <th style="text-align: left; width: 17%">Order Date</th>
                                                    <th style="text-align: right; width: 8%">Total Qty</th>
                                                    <th style="text-align: right; width: 8%">Total Amount</th>
                                                    <%--   <th style="text-align: right; width: 8%">Status</th>--%>
                                                    <th style="text-align: right; width: 20%">

                                                        <%--<input type="button" runat="server" class="btn btn-primary" value="Dispatch" />
                                                        <input type="button" runat="server" class="btn btn-primary" value="Cancel Order" />--%>
                                                    </th>

                                                    <%--<th style="text-align: right; width: 10%">Amount</th>--%>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>

                                        <%--  <tr>
   
                                        <asp:HiddenField ID="hdnDate" runat="server" Value='<%#Eval("VDate","{0:dd/MMM/yyyy}")%> ' />
                                            <asp:HiddenField ID="hdnDistributorName" runat="server" Value='<%#Eval("Distributor")%>' />
                                        <asp:HiddenField ID="hdnDocId" runat="server" Value='<%#Eval("DocId")%>' />
                                        <td><%#Eval("VDate","{0:dd/MMM/yyyy}")%></td>
                                         <td><%# Eval("SyncId")%></td>
                                            <td><%# Eval("Distributor")%></td>
                                            <td><%# Eval("DocId")%></td>
                                            <td style="text-align: right;"><%# Eval("TotalQty")%></td>
                                            <td style="text-align: right;"><%# Eval("Amount")%></td>
                                            <td><asp:LinkButton CommandName="selectDate" ID="LinkButton1"
                                                    CausesValidation="False" runat="server" OnClientClick="window.document.forms[0].target='_self'; setTimeout(function(){window.document.forms[0].target='';}, 500);" 
                                                    Text="Dispatch" 
                                                    Width="80px" Font-Underline="True"/> &nbsp;&nbsp; <asp:LinkButton CommandName="selectDate1" ID="LinkButton2"
                                                    CausesValidation="False" runat="server" OnClientClick="window.document.forms[0].target='_self'; setTimeout(function(){window.document.forms[0].target='';}, 500);" 
                                                    Text="Order Cancel" 
                                                    Width="80px" Font-Underline="True" /></td>
                                        </tr>
                                          
                                        --%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>

                            <div class="row" style="margin-left: 1%;">
                                <%--<asp:Button type="button" ID="Button3" runat="server" Text="Go" class="btn btn-primary" OnClientClick="loding()"  
                                         OnClick="btnGo_Click" Visible="true" />--%>
                                <%--    <asp:Butto/n type="button" ID="Button1" runat="server" Text="Save" class="btn btn-primary" OnClick="btnDispatchSave_Click" />
                                --%>
                                <button id="btnmultidispatch" type="button" class="btn btn-primary" onclick="openmodeldispatchordermulti('D')">Dispatch</button>
                                <button id="btnmulticancel" type="button" class="btn btn-danger" onclick="openmodeldispatchordermulti('C')">order cancel</button>
                            </div>
                        </div>
                        <br />
                        <input type="hidden" id="hidordeprocesstype" />

                        </br>

                                <div>
                                    <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                                </div>




                    </div>
                </div>
            </div>
        </div>

        <div>

            <!-- The Modal -->
            <div class="modal" id="myModal">
                <div class="modal-dialog" style="margin-left: 5%;">
                    <div class="modal-content" style="width: 205%; overflow-y: auto !important;">

                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Retailer Dispatch Order Details</h4>
                            <button id="modalClose1" type="button" class="close" style="margin-top: -28px !important;">&times;</button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body">
                            <div class="box-body">



                                <div class="row box-body table-responsive">
                                    <table>
                                        <tr>

                                            <th>Party Name: &nbsp;&nbsp;</th>
                                            <td>
                                                <asp:Label ID="distname" runat="server" Text=""></asp:Label>
                                                &nbsp;&nbsp;  (
                                                    <asp:Label ID="vdate" runat="server" Text=""></asp:Label>)  &nbsp;&nbsp; </td>
                                        </tr>
                                        <tr>

                                            <th>Address: &nbsp;&nbsp;</th>
                                            <td>
                                                <asp:Label ID="Label8" runat="server" Text=""></asp:Label>&nbsp;&nbsp; </td>
                                        </tr>
                                        <tr>

                                            <th> Remark While Punch Order : &nbsp;&nbsp;</th>
                                            <td>
                                                <asp:Label ID="lblRemarkViwPunch" runat="server" Text=""></asp:Label>&nbsp;&nbsp; </td>
                                        </tr>
                                    </table>
                                </div>

                                <div class="row">
                                    <div id="Div2" style="width:1200px;">
                                        <%-- <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">--%>
                                        <div class="box-body table-responsive" style="overflow-x: hidden">
                                            <asp:Repeater ID="rpt" runat="server">
                                                <HeaderTemplate>
                                                    <table id="example1" class="table table-bordered table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th >Docid</th>
                                                                <th>ItemId</th>
                                                                <th style="text-align: left; ">Item Name</th>
                                                               <%-- <th style="text-align: right; width: 8%">Order Qty</th>--%>
                                                                 <th style="text-align: left;">Order Qty Description</th>
                                                                <th style="text-align: right; ">Rate</th>
                                                                 <th style="text-align: right;">Margin</th>
                                                                <th style="text-align: right;">Discount%</th>
                                                                 <th style="text-align: right;">Discount Type</th>
                                                                  <th style="text-align: right;">DiscountAmount</th>

                                                                <th style="text-align: right;">Order Amount</th>
                                                                 <th style="text-align: right; ">Net Amount</th>
                                                                <th style="text-align: right;">Dispatch Qty</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <%--  <tr onclick="DoNav('<%#Eval("ItemId") %>');">
                                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("ItemId") %>' />
                                                         <asp:HiddenField ID="HiddenField2" runat="server" Value='<%#Eval("DocId") %>' />
                                                        <td><%#Eval("ItemName") %></td>
                                                        <td><%#Eval("OrderQty") %></td>
                                                        <td><%#Eval("OrderAmount") %></td>
                                                        <td>
                                                           
                                                            <input type="text" runat="server" id="dispatchQty" onfocus="this.select()" onchange="checkVal(this.id)" onkeypress="return isNumber(event)" MaxLength="12" Class="form-control numeric text-right" Value=<%#Eval("OrderQty") %>/>
                                                            <%--<asp:TextBox ID="distPrice" onfocus= "this.select()" AutoPostBack="true" OnTextChanged="distPrice_TextChanged" onkeypress="return isNumber(event)" MaxLength="12" runat="server" CssClass="form-control numeric text-right" Text=<%#Eval("DistPrice") %>> </asp:TextBox>
                                                        </td>
                                                    </tr>--%>
                                                </ItemTemplate>

                                                <FooterTemplate>
                                                    </tbody>   
                                                    
                                                       <tfoot>
            <tr>
                <th colspan="8" style="text-align:right">Total:</th>
                <th style="text-align: right;"></th>
                 <th style="text-align: right;"></th>
                 <th style="text-align: right;"></th>
                 <th style="text-align: right;"></th>
            </tr>
        </tfoot>    
                                                    </table>   
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>


                                        <%-- <asp:HiddenField ID="HiddenField2" runat="server"  />       
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnEdit" runat="server" Text="Save" class="btn btn-primary" OnClick="btnEdit_Click" OnClientClick="return editBtn();"/>--%>

                                        <%--</div>--%>
                                    </div>
                                </div>
                                <br />
                                <div class="row box-body table-responsive">


                                    <table>

                                        <tr>
                                            <td><b>Remark:</b>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label></td>
                                            <td>
                                                <textarea id="TextArea1" class="form-control" style="resize: none; height: 80px; width: 420px;" cols="20" rows="2"
                                                    placeholder="Enter Remark"></textarea></td>
                                        </tr>


                                    </table>

                                </div>





                            </div>
                        </div>

                        <!-- Modal footer -->
                        <div class="modal-footer">
                            <%--<asp:Button type="button" ID="Button3" runat="server" Text="Go" class="btn btn-primary" OnClientClick="loding()"  
                                         OnClick="btnGo_Click" Visible="true" />--%>
                            <%--    <asp:Button type="button" ID="Button1" runat="server" Text="Save" class="btn btn-primary" OnClick="btnDispatchSave_Click" />
                            --%>
                            <button id="btnsave" type="button" class="btn btn-primary" onclick="dispatchorder()">Dispatch</button>
                            <button id="modalClose" type="button" class="btn btn-danger">Close</button>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div>

            <!-- The Modal -->
            <div class="modal" id="myModal4">
                <div class="modal-dialog">
                    <div class="modal-content" style="overflow-y: auto !important;">

                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Dispatch/Cancel Order Details</h4>
                            <button id="modalClose6" type="button" class="close" style="margin-top: -28px !important;">&times;</button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body">
                            <div class="box-body">


                                <div class="row">

                                    <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">
                                        <table>

                                            <tr>
                                                <td><b>Remark:</b>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label></td>
                                                <td>
                                                    <textarea id="TextArea3" class="form-control" style="resize: none; height: 80px; width: 420px;" cols="20" rows="2"
                                                        placeholder="Enter Remark"></textarea></td>
                                            </tr>


                                        </table>
                                    </div>
                                </div>





                            </div>
                        </div>

                        <!-- Modal footer -->
                        <div class="modal-footer">
                            <%--<asp:Button type="button" ID="Button3" runat="server" Text="Go" class="btn btn-primary" OnClientClick="loding()"  
                                         OnClick="btnGo_Click" Visible="true" />--%>
                            <%--    <asp:Button type="button" ID="Button1" runat="server" Text="Save" class="btn btn-primary" OnClick="btnDispatchSave_Click" />
                            --%>
                            <button id="btnmultisavesave" type="button" class="btn btn-primary" onclick="dispatchordermulti()">Save</button>
                            <button id="modalClose5" type="button" class="btn btn-danger">Close</button>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div>
            <div class="modal" id="myModal3">
                <div class="modal-dialog" style="margin-left:5%;">
                    <div class="modal-content" style="width: 205%; overflow-y: auto !important;">

                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Order Details</h4>
                            <button id="modalClose3" type="button" class="close" style="margin-top: -28px !important;">&times;</button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body">
                            <div class="box-body">



                                <div class="row box-body table-responsive">
                                    <table>
                                        <tr>

                                            <th>Party Name: &nbsp;&nbsp;</th>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                                                &nbsp;&nbsp;  (
                                                    <asp:Label ID="Label4" runat="server" Text=""></asp:Label>)  &nbsp;&nbsp;

                                                 <asp:HiddenField ID="hiddendocid" runat="server" />
                                            </td>
                                           
                                        </tr>
                                        <tr>

                                            <th>Address: &nbsp;&nbsp;</th>
                                            <td>
                                                <asp:Label ID="Label9" runat="server" Text=""></asp:Label>&nbsp;&nbsp; </td>
                                        </tr>
                                        <tr>

                                            <th> Remark While Punch Order : &nbsp;&nbsp;</th>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text=""></asp:Label>&nbsp;&nbsp; </td>
                                        </tr>
                                          <tr>

                                            <th>DocId : &nbsp;&nbsp;</th>
                                            <td>
                                                <asp:Label ID="Label12" runat="server" Text=""></asp:Label>&nbsp;&nbsp; </td>
                                        </tr>
                                         <tr>

                                            <th>Order Taken Type : &nbsp;&nbsp;</th>
                                            <td>
                                                <asp:Label ID="lblOrderTakenType" runat="server" Text=""></asp:Label>&nbsp;&nbsp;                                              
                                                <i id="faicon" style="visibility:hidden" class="fa fa-phone" aria-hidden="true"></i>
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                                <div class="row">
                                    <div id="Div12" style="width:1200px;">
                                        <%-- <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">--%>
                                        <div class="box-body table-responsive" style="overflow-x: hidden">
                                            <asp:Repeater ID="Repeater1" runat="server">
                                                <HeaderTemplate>
                                                    <table id="example1" class="table table-bordered table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th >Docid</th>
                                                                <th >ItemId</th>
                                                                <th style="text-align: Left;">Item Name</th>
                                                                <%--<th style="text-align: right; width: 8%">Order Qty</th>--%>
                                                                 <th style="text-align: left;">Order Qty Description</th>
                                                                <th style="text-align: right; ">Rate</th>
                                                                <th style="text-align: right;">Margin</th>
                                                                <th style="text-align: right;">Discount%</th>
                                                                 <th style="text-align: right; ">Discount Type</th>
                                                                  <th style="text-align: right; ">Discount Amount</th>
                                                                <th style="text-align: right; ">Order Amount</th>
                                                                 <th style="text-align: right;">Net Amount</th>
                                                                <th style="text-align: right;">Dispatch Qty</th>

                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <%--  <tr onclick="DoNav('<%#Eval("ItemId") %>');">
                                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("ItemId") %>' />
                                                         <asp:HiddenField ID="HiddenField2" runat="server" Value='<%#Eval("DocId") %>' />
                                                        <td><%#Eval("ItemName") %></td>
                                                        <td><%#Eval("OrderQty") %></td>
                                                        <td><%#Eval("OrderAmount") %></td>
                                                        <td>
                                                           
                                                            <input type="text" runat="server" id="dispatchQty" onfocus="this.select()" onchange="checkVal(this.id)" onkeypress="return isNumber(event)" MaxLength="12" Class="form-control numeric text-right" Value=<%#Eval("OrderQty") %>/>
                                                            <%--<asp:TextBox ID="distPrice" onfocus= "this.select()" AutoPostBack="true" OnTextChanged="distPrice_TextChanged" onkeypress="return isNumber(event)" MaxLength="12" runat="server" CssClass="form-control numeric text-right" Text=<%#Eval("DistPrice") %>> </asp:TextBox>
                                                        </td>
                                                    </tr>--%>
                                                </ItemTemplate>

                                                <FooterTemplate>
                                                    </tbody>   
                                                    
                                                     <tfoot>
            <tr>
                <th colspan="8" style="text-align:right">Total:</th>
                <th style="text-align: right;"></th>
                 <th style="text-align: right;"></th>
                 <th style="text-align: right;"></th>
                 <th style="text-align: right;"></th>
            </tr>
        </tfoot>  
                                                    </table>   
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>


                                        <%-- <asp:HiddenField ID="HiddenField2" runat="server"  />       
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnEdit" runat="server" Text="Save" class="btn btn-primary" OnClick="btnEdit_Click" OnClientClick="return editBtn();"/>--%>

                                        <%--</div>--%>
                                    </div>
                                </div>
                                <br />
                                <div class="row box-body table-responsive">


                                    <table>

                                        <tr>
                                            <td><b>Remark:</b>&nbsp;&nbsp;<asp:Label ID="Label6" runat="server" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Status:</b>&nbsp;&nbsp;<asp:Label ID="Label7" runat="server" Text=""></asp:Label>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Done By:</b>&nbsp;&nbsp;<asp:Label ID="Label11" runat="server" Text=""></asp:Label>

                                            </td>
                                        </tr>
                                          <tr>
                                            <td>
                                                <b>Date:</b>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text=""></asp:Label>

                                            </td>
                                        </tr>
                                        
                                    </table>

                                </div>





                            </div>
                        </div>

                        <!-- Modal footer -->
                        <div class="modal-footer">
                            <%--<asp:Button type="button" ID="Button3" runat="server" Text="Go" class="btn btn-primary" OnClientClick="loding()"  
                                         OnClick="btnGo_Click" Visible="true" />--%>
                            <%--    <asp:Button type="button" ID="Button1" runat="server" Text="Save" class="btn btn-primary" OnClick="btnDispatchSave_Click" />
                            --%>
                                 <asp:Button ID="btnexportdetail" runat="server" CssClass="btn btn-primary" Text="Export" OnClick="btnexportdetail_Click"/>
                            <button id="modalClose4" type="button" class="btn btn-danger">Close</button>
                        </div>

                    </div>
                </div>
            </div>
        </div>



        <div>

            <!-- The Modal -->
            <div class="modal" id="myModal1">
                <div class="modal-dialog" style="margin-left:5%;">
                    <div class="modal-content" style="width: 205%; overflow-y: auto !important;">

                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Retailer Dispatch Order Cancel Details</h4>
                            <button id="cmodalClose1" type="button" class="close" style="margin-top: -28px !important;">&times;</button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body">
                            <div class="box-body">



                                <div class="row box-body table-responsive">
                                    <table>
                                        <tr>

                                            <th>Party Name: &nbsp;&nbsp;<asp:HiddenField ID="HiddenField_ID" runat="server" />
                                            </th>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                                &nbsp;&nbsp;  (
                                                    <asp:Label ID="Label2" runat="server" Text=""></asp:Label>)  &nbsp;&nbsp; </td>
                                        </tr>
                                        <tr>

                                            <th>Address: &nbsp;&nbsp;</th>
                                            <td>
                                                <asp:Label ID="Label10" runat="server" Text=""></asp:Label>&nbsp;&nbsp; </td>
                                        </tr>
                                        <tr>

                                            <th>
                                            Remark While Punch Order: 
                                        <td>
                                            <asp:Label ID="lblRemarkViewOrderCancel" runat="server" Text=""></asp:Label></td>
                                        </tr>
                                    </table>
                                </div>


                                <br />
                                <div class="row">
                                    <div id="Div13" style="width:1200px;">
                                        <%-- <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">--%>
                                        <div class="box-body table-responsive" style="overflow-x: hidden">
                                            <asp:Repeater ID="Repeater2" runat="server">
                                                <HeaderTemplate>
                                                    <table id="example1" class="table table-bordered table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th >Docid</th>
                                                                <th >ItemId</th>
                                                                <th style="text-align: left; ">Item Name</th>
                                                               <%-- <th style="text-align: right; width: 8%">Order Qty</th>--%>
                                                                <th style="text-align: left;">Order Qty Description</th>
                                                                <th style="text-align: right;">Rate</th>
                                                                 <th style="text-align: right;">Margin</th>
                                                                <th style="text-align: right; ">Discount%</th>
                                                                  <th style="text-align: right;">Discount Type</th>
                                                                <th style="text-align: right;">Discount Amount</th>
                                                                <th style="text-align: right;">Order Amount</th>
                                                                <th style="text-align: right;">Net Amount</th>
                                                                <th style="text-align: right;">Dispatch Qty</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <%--  <tr onclick="DoNav('<%#Eval("ItemId") %>');">
                                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("ItemId") %>' />
                                                         <asp:HiddenField ID="HiddenField2" runat="server" Value='<%#Eval("DocId") %>' />
                                                        <td><%#Eval("ItemName") %></td>
                                                        <td><%#Eval("OrderQty") %></td>
                                                        <td><%#Eval("OrderAmount") %></td>
                                                        <td>
                                                           
                                                            <input type="text" runat="server" id="dispatchQty" onfocus="this.select()" onchange="checkVal(this.id)" onkeypress="return isNumber(event)" MaxLength="12" Class="form-control numeric text-right" Value=<%#Eval("OrderQty") %>/>
                                                            <%--<asp:TextBox ID="distPrice" onfocus= "this.select()" AutoPostBack="true" OnTextChanged="distPrice_TextChanged" onkeypress="return isNumber(event)" MaxLength="12" runat="server" CssClass="form-control numeric text-right" Text=<%#Eval("DistPrice") %>> </asp:TextBox>
                                                        </td>
                                                    </tr>--%>
                                                </ItemTemplate>

                                                <FooterTemplate>
                                                    </tbody>  
                                                    
                                                    
                                                         <tfoot>
            <tr>
                <th colspan="8" style="text-align:right">Total:</th>
                <th style="text-align: right;"></th>
                 <th style="text-align: right;"></th>
                 <th style="text-align: right;"></th>
                 <th style="text-align: right;"></th>
            </tr>
        </tfoot>     
                                                    </table>   
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>


                                        <%-- <asp:HiddenField ID="HiddenField2" runat="server"  />       
                                        <asp:Button Style="margin-right: 5px;" type="button" ID="btnEdit" runat="server" Text="Save" class="btn btn-primary" OnClick="btnEdit_Click" OnClientClick="return editBtn();"/>--%>

                                        <%--</div>--%>
                                    </div>
                                </div>


                                <br />
                                <div class="row box-body table-responsive">


                                    <table>

                                        <tr>
                                            <td><b>Remark:</b>&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label></td>
                                            <td>
                                                <textarea id="TextArea2" class="form-control" style="resize: none; height: 80px; width: 420px;" cols="20" rows="2"
                                                    placeholder="Enter Remark"></textarea></td>
                                        </tr>


                                    </table>

                                </div>

                            </div>
                        </div>

                        <!-- Modal footer -->
                        <div class="modal-footer">
                            <button id="Button2" type="button" class="btn btn-primary" onclick="Cancelrder()">Order Cancel</button>
                            <%--   <asp:Button type="button" ID="Button2" runat="server" Text="Save" class="btn btn-primary" OnClick="btnCancelOrderSave_Click"/>--%>
                            <button id="cmodalClose" type="button" class="btn btn-danger">Close</button>
                        </div>

                    </div>
                </div>
            </div>
        </div>




    </section>
</asp:Content>


