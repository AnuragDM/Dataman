<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/FFMS.Master" CodeBehind="SaleInvoiceform.aspx.cs" Inherits="AstralFFMS.SaleInvoiceform" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>

    <link href="plugins/multiselect.css" rel="stylesheet" />


    <script src="plugins/multiselect.js"></script>
    <style type="text/css">
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
                .multiselect-container > li > a {
            white-space: normal;
        }

        .multiselect-container > li {
            width: 100%;
        }

        .multiselect-container.dropdown-menu {
            width: 100% !important;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $('[id*=ListArea]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 300,
                enableFiltering: true,
                filterPlaceholder: 'Search'
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $('[id*=ListBox1]').multiselect({
                enableCaseInsensitiveFiltering: true,
                //buttonWidth: '200px',
                buttonWidth: '100%',
                includeSelectAllOption: true,
                maxHeight: 200,
                width: 300,
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
        function openchargeModel() {
            $('#myModal3').modal('show');
        }
        function AddCharges() {
            if ( $('#lstothercharges').val()==0) {
                errormessage("Please Select Charges");
                return false;
            }
            if ($('#txtcharges').val()=="") {
                errormessage("Please enter Amount");
                return false;
            }
            
            var hid = $('#lstothercharges').val();
            var table1 = $('#tblothercharge').DataTable();
            var data = table1.rows().data();
            for (var i = 0; i < data.length; i++) {
                if (hid == data[i][0])
                {
                    duplicaterow=1;
                    errormessage("Charges already added.");
                    return false;
                }

            }


            
            var hid =  $('#lstothercharges').val();
            var Name =  $('#lstothercharges')[0].selectedOptions[0].innerText;
            var Amount = $('#txtcharges').val();
            //var uom = $('#txtquotationuom').val();
            //var moq = $('#txtquotationmoq').val();
            //var mrp = $('#txtquotationmrp').val();
            //var discount = $('#txtquotationdiscperc').val();
            //var unitprice = $('#txtquotationunitprice').val();

            table1.row.add( [
                hid,
            Name,
            Amount,
          
            ] ).draw( false );
            $('#lstothercharges').val(0);
            $('#txtcharges').val('');

            $('#myModal3').modal('hide');
            Calculategrandtotal();
        }

        function getothercharges() {
            $.getJSON('<%=ResolveUrl("~/and_sync.asmx/getexpensemast")%>', { }
      , function (result) {
        //  console.log(result);
          $('#lstothercharges').append($('<option>').text("--Select--").attr('value',0));
          $.each(result, function (key1, value1) {
              $('#lstothercharges').append($('<option>').text(value1.Name).attr('value', value1.Id));
          })
      })
        
        }
    </script>
    <script type="text/javascript">

        function convertTableToArrayObject() {
            var totaltaxamt=0;
            var totalorderamount=0;
            var idetails = new Object();
            idetails["distId"]=$('#hiddistid').val();
            idetails["orderId"]=$('#<%=lblDocid.ClientID%>').text();
            var invoiceitemlist=new Array();
            var otherchargeslist=new Array();
            var taxarr=new Array();
            var table = $('#Div2 table').DataTable();

            var data = table.rows().data();

            for (var i = 0; i < data.length; i++) {
                var itemd = new Object();
                taxarr=[];

                itemd["ItemId"] = data[i]["ItemId"];
                itemd["ItemName"] = data[i]["ItemName"];// data[i][7];
                itemd["Qty"] = table.cell(i, 3).nodes().to$().find('input').val();
                itemd["Rate"] = table.cell(i, 4).nodes().to$().find('input').val();

                var finditemtax=taxitemdetail.filter(x=>x.itemid==data[i]["ItemId"]);
                for (var j = 0; j < finditemtax.length; j++) {
                    var taxd = new Object();
                    taxd["taxType"] =finditemtax[j]["taxType"];
                    taxd["taxValue"] =finditemtax[j]["taxvalue"];
                    taxarr[j]=taxd;
                }

                itemd["tax"] =taxarr;
                itemd["CentralTaxPer"] = data[i]["CentralTaxPer"];
                itemd["StateTaxPer"] = data[i]["StateTaxPer"];
                itemd["IntegratedTaxPer"] = data[i]["IntegratedTaxPer"];
                itemd["disc"] =table.cell(i, 8).data();
                itemd["totalAmount"] =data[i]["OrderAmount"];
                invoiceitemlist[i] = itemd;

                totaltaxamt=totaltaxamt+parseFloat(table.cell(i, 6).nodes().to$().find('button').text());
                totalorderamount=totalorderamount+parseFloat(data[i]["OrderAmount"]);

            }
            var tablecharges = $('#tblothercharge').DataTable();
            var datacharges = tablecharges.rows().data();

            for (var i = 0; i < datacharges.length; i++) {
                var othercharegs = new Object();

                othercharegs["Name"]=datacharges[i][1];
                othercharegs["Amount"]=datacharges[i][2];
                otherchargeslist[i]=othercharegs;
                totalorderamount=totalorderamount+parseFloat(datacharges[i][2]);
            }


            idetails["items"]=invoiceitemlist;
            idetails["remark"]=$('#TextArea1').val();
            idetails["TotalTaxAmt"]=totaltaxamt;
            idetails["TotalBillAmount"]=totalorderamount;
            idetails["othersexpense"]=otherchargeslist;
            idetails["vehiclenumber"] = $('#Txtvechicle').val();
   

            return idetails;

       
        }
        function savesalesinvoice() {
            var saveinvoiceModel=convertTableToArrayObject(); 

            $.ajax({
                type: "POST",
                url: "And_sync.asmx/InsertSalesInvoiceData",
                data:{ "value":  JSON.stringify(saveinvoiceModel) },
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
                contentType: "application/x-www-form-urlencoded;",
                     responseType: "json",
                     success: function (response) {
                         console.log(response);
                         if (response !="") {
                             $('#myModal').modal('hide');
                             BindGridView();
                         }
                         //if (data.ResultMsg == "Success") {
                         //    Successmessage("Order dispatched Successfully")
                         //    $('#TextArea1').val("");
                         //    var table = $('#Div2 table').DataTable();
                         //    table.destroy();

                         
                         //}
                         //else {
                         //    errormessage("Something went wrong");
                         //}
                     },
                     failure: function (response) {
                         //   alert(response);
                     },
                     error: function (response) {
                         console.log(response);
                     }
                 });
         //   console.log(saveinvoiceModel)
        }
    </script>
    <script type="text/javascript">
        var itemdetail = [];
        var taxitemdetail = [];
        var data=[];
        var selectedrowindex;
        var totaltaxamount=0;
        $(document).ready(function () {
            BindState();
            getothercharges();
            $('#tblothercharge').DataTable( {
                data:           data,
                deferRender:    true,
                "sScrollX": "100%",
                "scrollX": true,
                scrollY:        "300",
                scrollCollapse: true,
                scroller:       true,
                "searching": false,   // Search Box will Be Disabled

                "ordering": false,    // Ordering (Sorting on Each Column)will Be Disabled

                "info": false,         // Will show "1 to n of n entries" Text at bottom

                "lengthChange": false ,
                "bPaginate": false,
                "columnDefs": [ 
                    {"targets": 0,
                        "visible": false
                    },
                    {
                        "targets": -1,
                        "data": null,
                        "defaultContent": "<a class='del'><i class='fa fa-trash'></i></a>"
                    } ]
            } );
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
            //$('#myModal').modal({
            //    backdrop: 'static',
            //    keyboard: false
            //})

            $('#modalClose2').on('click', function () {
      

                $('#myModal2').modal('hide');
            })

            $('#modalClose23').on('click', function () {
                $('#lsttaxtype').val('S')
                $('#lsttaxtype1').val('S')
                $('#perlblrow1').text('');
                $('#perlblamount1').text('');
      
      
                $('#perlblrow2').text('');
                $('#perlblamount2').text('');
                $('#myModal2').modal('hide');
            })

            $('#modalClose3').on('click', function () {
               
                $('#myModal3').modal('hide');
            })



            $('#modalCcancel').on('click', function () {
                $('#lstothercharges').val(0);
                $('#txtcharges').val('');
                $('#myModal3').modal('hide');
            })
            var myTable = $('#tblothercharge').DataTable();
 
          
            $('#tblothercharge tbody').on( 'click', 'a.del', function () {
               
                myTable.row( $(this).parents('tr') ).remove().draw();
        
            } );

        })
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            //alert(iKeyCode);
            if (iKeyCode != 9 && iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57)) {

                // if (iKeyCode != 8 && iKeyCode != 0 && (iKeyCode < 48 || iKeyCode > 57))

                return false;
            }
            return true;
        }
        function BindState() {
            $('#<%=ListArea.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            $.ajax({
                type: "POST",
                url: 'SaleInvoiceform.aspx/BindState',
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
                url: 'SaleInvoiceform.aspx/BindDistributor',
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

    </script>
    <script type="text/javascript">
        function btnSubmitfunc() {
          
            // selectedrowindex;
             totaltaxamount=0;
            // $("#hiddistributor").val(selectedvalue);
            var selectedValues = [];
            $("#<%=ListBox1.ClientID %> :selected").each(function () {
                selectedValues.push($(this).val());
            });
            $("#<%=hiddistributor.ClientID%>").val(selectedValues);



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

        function Calculategrandtotal() {
    
            var grandtotal=0;
            var table = $('#Div2 table').DataTable();
            var data = table.rows().data();
           
            for (var i = 0; i < data.length; i++) {
                grandtotal=parseFloat(grandtotal)+parseFloat(data[i]["OrderAmount"]);

            }


            var table = $('#tblothercharge').DataTable();
            var data = table.rows().data();
           
            for (var i = 0; i < data.length; i++) {
                grandtotal=parseFloat(grandtotal)+parseFloat(data[i][2]);

            }
            $('#<%=Labeltotal.ClientID%>').text(grandtotal.toFixed(2));
          
        }
        function Cancelfun() {
            window.location.href = "SaleInvoiceform.aspx";
        }

        function loding() {
            $('#spinner').show();
        }
        function BindGridView() {
            itemdetail = [];
            taxitemdetail = [];
            data=[];
            $.ajax({
                type: "POST",
                url: "And_sync.asmx/GetPurchorderforinvoice",
                data: '{Distid: "' + $("#<%=hiddistributor.ClientID%>").val() + '", Fromdate: "' + $('#<%=txtfmDate.ClientID%>').val() + '",Todate: "' + $('#<%=txttodate.ClientID%>').val() + '",status:"' + $('#<%=ddlstatus.ClientID%>').val() + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
                contentType: "application/json;",
               // async:false,
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
            var data = JSON.parse(response.d);
         //   console.log(data);
            //alert(data);
            //var arr1 = data.length;
            //alert(arr1);
            var table = $('#ContentPlaceHolder1_rptmain table').DataTable();
            table.destroy();
            $("#ContentPlaceHolder1_rptmain table ").DataTable({
                //"order": [[0, "desc"]],


                "aaData": data,
                "columnDefs": [
   {
       "targets": [0],
       "visible": false,

   }],
                "aoColumns": [
                    {
                        "mData":"invoicedocid"
                    },

         {
             "mData": "SNo",
             "render": function (data, type, row, meta) {

                 return $('<div>')
                        .attr('class', 'text-left')
                    // .attr('style', 'color:blue;font-weight: bold;')
                        .text(data)
                        .wrap('<div></div>')
                        .parent()
                        .html();

             }





         },
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
      //  { "mData": "Address" },
          { "mData": "Mobile" },
              {
                  "mData": "PODocId",
              },
               { "mData": "VDate" },



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
                   "mData": "Orderby",


               }, // <-- which values to use inside object

          //{ "mData": "Status" },
            {
                "mData": "PODocId",
                "render": function (data, type, row, meta) {
             //       console.log(row);
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
                    {
                        return "<a type='button' id='PDocId#" + data.replace(/\s+/g, '-') + "'  data-toggle='tooltip' title='Create Invoice'  onclick='openinvoiceform(this.id)' style='" + add + "'><img src='img/invoice.png' style='height: 22px;'></a>"; //class='btn btn-primary btn-sm'
                    }

                    else
                    {
                        return "<a type='button'  id='pdfbtn#" + row.invoicedocid.replace(/\s+/g, '-') + "' data-toggle='tooltip' title='Generate Pdf' onclick='GeneratePdf(this.id)'><i class='fa fa-file-pdf-o' style='font-size: large;'></i></a>";
                    }

                }
            }
                ]
            });

        $('#spinner').hide();
    }
    function updatetax() {
        var table = $('#Div2 table').DataTable();
        // $('a#rowa#' + selectedrowindex).text(totaltaxamount);
        table.cell(selectedrowindex, 6).nodes().to$().find('button').text(totaltaxamount);
        $('#myModal2').modal('hide');
        var data = table.rows().data();
        var arr=taxitemdetail.filter(x=>x.itemid==data[selectedrowindex]["ItemId"]);
        if (arr.length==0) {
    
            if ($('#lsttaxtype').val() !='S') {
    
            taxitemdetail.push({
                itemid:data[selectedrowindex]["ItemId"],
                taxType:$('#lsttaxtype').val(),
                taxvalue:$('#perlblamount1').text(),
                taxper:$('#perlblrow1').text()
            });

            }
            if ($('#lsttaxtype1').val() !='S') {
                taxitemdetail.push({
                    itemid:data[selectedrowindex]["ItemId"],
                    taxType:$('#lsttaxtype1').val(),
                    taxvalue:$('#perlblamount2').text(),
                    taxper:$('#perlblrow2').text()
                });
            }
        }
        else {
            arr[0].taxType=$('#lsttaxtype').val();
            
            arr[0].taxvalue=$('#perlblamount1').text();
            arr[0].taxper=$('#perlblrow1').text();
            arr[1].taxType=$('#lsttaxtype1').val();
            arr[1].taxvalue=$('#perlblamount2').text();
            arr[1].taxper=$('#perlblrow2').text();
        }
      //  console.log(taxitemdetail);

        $('#lsttaxtype').val('S')
        $('#lsttaxtype1').val('S')
        $('#perlblrow1').text('');
        $('#perlblamount1').text('');
      
      
        $('#perlblrow2').text('');
        $('#perlblamount2').text('');
        Calculateamt('tax#' + selectedrowindex);
        Calculategrandtotal();
        totaltaxamount=0;
        //table.cell(selectedrowindex, 6).data(totaltaxamount);
    }
    function GetPercentage(dropdown) {
       
        //var Discountper = table.cell(index, 6).nodes().to$().find('input').val();
        //var amount = parseFloat(parseFloat(Qty) * parseFloat(Rate))/// - parseFloat(Discount);

        //// amount=amount + parseFloat(tax);
        //var disamount = (amount * Discountper) / 100;
        //disamount = disamount.toFixed(2);
        //var totalamount = amount - parseFloat(disamount);
      //  console.log(itemdetail[0]);
        var amount = parseFloat(itemdetail[0].OrderQty)*parseFloat(itemdetail[0].rate);
        var totalamt=amount-parseFloat(itemdetail[0].Discount)
        var tax;
        var arr=taxitemdetail.filter(x=>x.taxType=="CGST" || x.taxType=="SGST");
        if (arr.length>0) {
            if(dropdown.value =="IGST")
            {
                errormessage("Please select correct tax type");
                dropdown.value="S";
                return false;
            }
        }

        var arr1=taxitemdetail.filter(x=>x.taxType=="IGST" );
        if (arr1.length>0) {
            if(dropdown.value =="CGST" || dropdown.value =="SGST")
            {
                errormessage("Please select correct tax type");
                dropdown.value="S";
                return false;
            }
        }
        if (dropdown.id == "lsttaxtype") {

            if (dropdown.value ==$('#lsttaxtype1').val()) {
                errormessage(dropdown.value + " already added");
                $('#lsttaxtype').val('S');
                return false;
            }
            if (dropdown.value =="CGST" && $('#lsttaxtype1').val()=="IGST") {
                errormessage("Please select correct tax type");
                $('#lsttaxtype').val('S');
                return false;
            }
            if (dropdown.value =="SGST" && $('#lsttaxtype1').val()=="IGST") {
                errormessage("Please select correct tax type");
                $('#lsttaxtype').val('S');
                return false;
            }
            if (dropdown.value =="IGST" && $('#lsttaxtype1').val()=="CGST") {
                errormessage("Please select correct tax type");
                $('#lsttaxtype').val('S');
                return false;
            }
            if (dropdown.value =="IGST" && $('#lsttaxtype1').val()=="SGST") {
                errormessage("Please select correct tax type");
                $('#lsttaxtype').val('S');
                return false;
            }
        }
        else {
            if (dropdown.value ==$('#lsttaxtype').val()) {
                errormessage(dropdown.value + " already added");
                $('#lsttaxtype1').val('S');
                return false;
            }
            if (dropdown.value =="CGST" && $('#lsttaxtype').val()=="IGST") {
                errormessage("Please select correct tax type");
                $('#lsttaxtype1').val('S');
                return false;
            }
            if (dropdown.value =="SGST" && $('#lsttaxtype').val()=="IGST") {
                errormessage("Please select correct tax type");
                $('#lsttaxtype1').val('S');
                return false;
            }
            if (dropdown.value =="IGST" && $('#lsttaxtype').val()=="CGST") {
                errormessage("Please select correct tax type");
                $('#lsttaxtype1').val('S');
                return false;
            }
            if (dropdown.value =="IGST" && $('#lsttaxtype').val()=="SGST") {
                errormessage("Please select correct tax type");
                $('#lsttaxtype1').val('S');
                return false;
            }
        }

        if (dropdown.value == "S") {
            if (dropdown.id == "lsttaxtype") {
                $('#perlblrow1').text('');
                $('#perlblamount1').text('');
            }
            else {
    
         
            $('#perlblrow2').text('');
            $('#perlblamount2').text('');
            }
      
       
        }
        else {
    
       
            if (dropdown.value == "CGST") {
                tax = (totalamt * itemdetail[0].CentralTaxPer) / 100;
                tax= tax.toFixed(2)
                if (dropdown.id == "lsttaxtype") {
                    $('#perlblrow1').text(itemdetail[0].CentralTaxPer+"%");
                    $('#perlblamount1').text(tax);
                }
                else {
                    $('#perlblrow2').text(itemdetail[0].CentralTaxPer + "%");
                    $('#perlblamount2').text(tax);
                }
                
            }
            else if (dropdown.value == "IGST") {
                tax = (totalamt * itemdetail[0].IntegratedTaxPer) / 100;
                tax = tax.toFixed(2)
                if (dropdown.id == "lsttaxtype") {
                    $('#perlblrow1').text(itemdetail[0].IntegratedTaxPer + "%");
                    $('#perlblamount1').text(tax);
                }
                else {
                    $('#perlblrow2').text(itemdetail[0].IntegratedTaxPer + "%");
                    $('#perlblamount2').text(tax);
                }
            }
            else {

                tax = (totalamt * itemdetail[0].StateTaxPer) / 100;
                tax = tax.toFixed(2)
                if (dropdown.id == "lsttaxtype") {
                    $('#perlblrow1').text(itemdetail[0].StateTaxPer + "%");
                    $('#perlblamount1').text(tax);
                }
                else {
                    $('#perlblrow2').text(itemdetail[0].StateTaxPer + "%");
                    $('#perlblamount2').text(tax);
                }

            }
        }
        totaltaxamount = totaltaxamount + parseFloat(tax);
        totaltaxamount=parseFloat(totaltaxamount.toFixed(2));
        //console.log(dropdown)
        //console.log(itemdetail[0]);
    }

    function opentaxmodel(a) {
        var index = a.split("#")[1]
        var table = $('#Div2 table').DataTable();
        var data = table.rows(index).data();
        itemdetail = data;
        selectedrowindex = index;
        var arr=taxitemdetail.filter(x=>x.itemid==itemdetail[0]["ItemId"]);
        if (arr.length==0) {
        }
        else {
            var amount = parseFloat(itemdetail[0].OrderQty)*parseFloat(itemdetail[0].rate);
            var totalamt=amount-parseFloat(itemdetail[0].Discount);
            var taxamt=(parseFloat(totalamt)*parseFloat(arr[0].taxper))/100;
            $('#lsttaxtype').val(arr[0].taxType);
            $('#perlblamount1').text(parseFloat(taxamt.toFixed(2)));
            $('#perlblrow1').text(arr[0].taxper);
            arr[0].taxvalue=parseFloat(taxamt.toFixed(2));
            taxamt=(parseFloat(totalamt)*parseFloat(arr[1].taxper))/100;
            $('#lsttaxtype1').val(arr[1].taxType);
            $('#perlblamount2').text(parseFloat(taxamt.toFixed(2)));
            $('#perlblrow2').text(arr[1].taxper);
            arr[1].taxvalue=parseFloat(taxamt.toFixed(2));
        }
        // selecteditemid =table.cell(index, 1).data();
        $('#myModal2').modal('show');
    }
    function Calculateamt(a) {

        var index = a.split("#")[1]
        var table = $('#Div2 table').DataTable();
        var data = table.rows().data();
        console.log(data);
        console.log(taxitemdetail);
        var Qty = table.cell(index, 3).nodes().to$().find('input').val();// data[i][7];
        var Rate = table.cell(index, 4).nodes().to$().find('input').val();
      //  var tax = table.cell(index, 6).nodes().to$().find('button').text();
        var Discountper = table.cell(index, 5).nodes().to$().find('input').val();
        var amount = parseFloat(parseFloat(Qty) * parseFloat(Rate))/// - parseFloat(Discount);
        var taxamount=0;
        var taxamount1=0;
       // amount=amount + parseFloat(tax);
        var disamount = (amount * Discountper) / 100;
        disamount = disamount.toFixed(2);
        var totalamount = amount - parseFloat(disamount);
        var arr=taxitemdetail.filter(x=>x.itemid==data[index]["ItemId"]);
        if (arr.length==0) {
        }
        else
        {
            taxamount=(parseFloat(totalamount.toFixed(2))*parseFloat(arr[0].taxper))/100;
            arr[0].taxvalue=parseFloat(taxamount.toFixed(2));
            taxamount1=(parseFloat(totalamount.toFixed(2))*parseFloat(arr[1].taxper))/100;
            arr[1].taxvalue=parseFloat(taxamount1.toFixed(2));
            taxamount=parseFloat(taxamount)+parseFloat(taxamount1);
        }

        totalamount= parseFloat(totalamount.toFixed(2)) + parseFloat(taxamount.toFixed(2));
        table.cell(index, 7).data(totalamount.toFixed(2));
        table.cell(index, 8).data(disamount);
        table.cell(index, 6).nodes().to$().find('button').text(parseFloat(taxamount.toFixed(2)));
      //  console.log(amount);
        Calculategrandtotal();
    }
    function GeneratePdf(a) {
        var docid =  a.split("#")[1]
        //var table = $('#ContentPlaceHolder1_rptmain table').DataTable();
        //var data = table.rows().data();
        //var docid=data[index]["invoicedocid"];
        $.getJSON('<%=ResolveUrl("~/and_sync.asmx/GetSalesInvoiceDataPdf")%>',{"DistInvDocId":  docid.split('-').join(' ')}
            , function (result) {

            //    console.log(result[0]["PdfPath"]);
                var link = document.createElement('a');
                var url="http:\\\\" + result[0]["PdfPath"];
             //   console.log(url);
                link.href =url// result[0]["SecondPdfPath"]// result[0]["SecondPdfPath"];
                var pieces =url.split(/[\s\\]+/); // result[0]["SecondPdfPath"].split(/[\s/]+/);
                link.download = pieces[pieces.length-1];
                link.dispatchEvent(new MouseEvent('click'));
                //window.location.href=result[0]["SecondPdfPath"];
            })

       <%-- $.ajax({
            type: "POST",
            url: "And_sync.asmx/GetSalesInvoiceDataPdf",
            data: '{DistInvDocId: "' + docid[1].split('-').join(' ') + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",
          contentType: "application/json;",
          responseType: "json",
          success: function (response) {
              console.log(JSON.parse(response.responseText));
          }
            ,
          failure: function (response) {
              //   alert(response);
          },
          error: function (response) {
              console.log(response);
          }
      });--%>
    }
    function openinvoiceform(a) {
     //   console.log(a);
        var docid = a.split("#")
    //    console.log(docid);
        $('#hiddocid').val(docid[1]);
        loding();
        BindModeldata(docid[1])
        Calculategrandtotal();


        // document.getElementById("ContentPlaceHolder1_pnlpopup").style.display = "block";
        //alert("D");
        // document.getElementById("divpanelpopup").style.display = "block";
        //document.getElementById("ContentPlaceHolder1_pnlpopup").style.height = "550px";


    }
    function BindModeldata(docid) {
        var table = $('#tblothercharge').DataTable();
 
        table.clear() .draw();
        $.ajax({
            type: "POST",
            url: "And_sync.asmx/Getpurchaseerorderdetail",
            data: '{docid: "' + docid + '"}',
                <%--data: "{'Distid':'" + $("#hidpersons").val() + "','Fromdate':'" + $('#<%=txtfmDate.ClientID%>').value + "','Todate':'" + $('#<%=txttodate.ClientID%>').val() + "'}",--%>
            contentType: "application/json;",
            async:false,
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
      //  console.log(data);
        itemdetail = data;
        $('#hiddistid').val(data[0]["DistId"]);
        $('#<%=distname.ClientID%>').text(data[0]["Distributor"]);
        $('#<%=lblDate.ClientID%>').text(data[0]["VDate"]);
        $('#<%=lblDocid.ClientID%>').text(data[0]["DocId"]);
        $('#<%=lblMob.ClientID%>').text(data[0]["Mobile"]);
        //alert(data);
        //var arr1 = data.length;
        //alert(arr1);
        var table1 = $('#Div2 table').DataTable();
        table1.destroy();



        $('#Div2 table').DataTable({
            //"scrollY": "300px",
            //"scrollX": true,
            "order": [[0, "desc"]],

            "aaData": data,
            "paging": false,
            "searching": false,
            "info": false,

            "columnDefs": [
     {
         "targets": [0],
         "visible": false,

     },
             {
                 "targets": [1],
                 "visible": false,

             },
               {
                   "targets": [8],
                   "visible": false,

               },
                 {
                     "targets": [3],
                     "width": "50px",

                 }
            ],
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
                   .attr('style', ' white-space:normal;width:200px;')
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
               "mData": "OrderQty",
               "render": function (data, type, row, meta) {

                //   console.log(row);
                   //console.log(data);
             //      console.log(meta.row);
                   //var did = row.DocId;
                   //console.log(did);

                   return "<input type='text'  id='row#" + meta.row + "' onfocus='this.select()' onchange='Calculateamt(this.id);'  onkeypress='return isNumber(event)' MaxLength='5'  Class='form-control numeric text-right' style='width:100px' Value='" + data + "'/>";


               }
           },
            {
                "mData": "rate",
                "render": function (data, type, row, meta) {
                    return "<input type='text'  id='row#" + meta.row + "' onfocus='this.select()' onchange='Calculateamt(this.id);'   onkeypress='return isNumber(event)' MaxLength='10'  Class='form-control numeric text-right' style='width:100px'  Value='" + data + "'/>";

                    //if (type === 'display') {
                    //    return $('<div>')
                    //       .attr('class', 'text-right')
                    //       .text(data)
                    //       .wrap('<div></div>')
                    //       .parent()
                    //       .html();

                    //} else {
                    //    return data;
                    //}
                }
            }, {
                "mData": "Discount",
                "render": function (data, type, row, meta) {
                    return "<input type='text'  id='row#" + meta.row + "' onfocus='this.select()' onchange='Calculateamt(this.id);'   onkeypress='return isNumber(event)' MaxLength='5'  Class='form-control numeric text-right'  style='width:80px'  Value='" + data + "'/>";

                    //if (type === 'display') {
                    //    return $('<div>')
                    //       .attr('class', 'text-right')
                    //       .text(data)
                    //       .wrap('<div></div>')
                    //       .parent()
                    //       .html();

                    //} else {
                    //    return data;
                    //}
                }
            }, {
                "mData": "Tax",
                "render": function (data, type, row, meta) {
              
                    return "  <div style='text-align: right;'><button type='button' id='rowa#" + meta.row + "' data-toggle='tooltip' title='Update Tax' style='border: none;background: transparent;color:#3c8dbc' onclick='opentaxmodel(this.id)'>0</button></div>";
                }
            },
                
                     //<th>ItemId</th>
                     //                                           <th>Item Name</th>
                     //                                           <th style="text-align: right; width: 8%">Order Qty</th>
                     //                                           <th style="text-align: right; width: 8%">Rate</th>
                     //                                           <th style="text-align: right; width: 8%">Discount</th>
                     //                                           <th style="text-align: right; width: 8%">Tax</th>
                     //                                           <th style="text-align: right; width: 10%">Order Amount</th>
                  
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
         "mData": "Discount",
         
     },

        //{
        //    "mData": "ItemId",
        //    "render": function (data, type, row, meta) {

        //        console.log(row);
        //        //console.log(data);
        //        console.log(meta.row);
        //        //var did = row.DocId;
        //        //console.log(did);

        //        return "<button type='button' id='ItemId#" + data + "'  data-toggle='tooltip' title='edit item' class='btn btn-primary btn-sm' onclick='openitemform(this.id)'>Edit</button>";


        //    }
        //}
            ]
        });

        $('#spinner').hide();
        $('#myModal').modal('show');
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
                                    <h3 class="box-title">List of Orders</h3>
                                </div>
                                <input type="hidden" id="hiddocid" />
                                <!-- /.box-header -->
                                <!-- form start -->
                                <div class="box-body">
                                    <div class="row">


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
                                                <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                <input type="hidden" id="hiddistributor" runat="server" />
                                                <input type="hidden" id="hidproductgroup" />
                                                <input type="hidden" id="hidproduct" />
                                            </div>

                                        </div>
                                             <div class="col-md-3 col-sm-6 col-xs-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Status:</label>
                                                <asp:DropDownList ID="ddlstatus" runat="server" CssClass="form-control ">
                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                      <asp:ListItem Value="All">All</asp:ListItem>
                                                    <asp:ListItem Value="Pending">Pending </asp:ListItem>
                                                    <asp:ListItem Value="InvoiceOrder">Invoice Order</asp:ListItem>
                                                  

                                                </asp:DropDownList>

                                            </div>

                                        </div>
                                      

                                    </div>
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

                                    <%--      
                                    <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClientClick="javascript:return btnSubmitfunc1();"
                                        OnClick="ExportCSV" />--%>
                                    <%-- <input style="margin-right: 5px;" type="button" id="Go" value="Go" class="btn btn-primary" onc onclick="GetReport();" />--%>
                                </div>
                                <br />
                            </div>
                        </div>
                        <div id="rptmain" runat="server" style="display: none;">
                            <div class="box-header table-responsive">
                                <asp:Repeater ID="orderpurchrpt" runat="server">
                                    <HeaderTemplate>
                                        <table id="itemsaleTable" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <th>invoice Docid</th>
                                                    <th style="text-align: left; width: 10%">SNo </th>

                                                    <th style="text-align: left; width: 20%">Distributor Name</th>
                                                    <th style="text-align: left; width: 10%">Distributor Mobile</th>

                                                    <th style="text-align: left; width: 20%">PODocId</th>

                                                    <th style="text-align: left; width: 20%">PODate</th>
                                                    <th style="text-align: right; width: 17%">Order Amount</th>
                                                    <th style="text-align: left; width: 8%">Order By</th>

                                                    <th style="text-align: left; width: 20%">
                                                        Action
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

                        </div>
                        <br />
                        <input type="hidden" id="hidordeprocesstype" />


                        <div>
                            <b>Note : It is mandatory to fill in all the required fields marked with asterisks(<span style="color: red">*</span>)</b>
                        </div>




                    </div>
                </div>
            </div>
        </div>



        <div>

            <!-- The Modal -->
            <div class="modal" id="myModal"  data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content" style="width: 130%; overflow-y: auto !important; height: 500px;">

                        <!-- Modal Header -->
                        <div class="modal-header" style="padding: 8px;">
                            <h4 class="modal-title" style="margin-left: 10px">Create Invoice</h4>
                            <button id="modalClose1" type="button" class="close" style="margin-top: -28px !important;">&times;</button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body">
                            <div class="box-body">



                                <div class="row box-body table-responsive">
                                    <table>
                                        <tr>

                                            <th>Distributor  : &nbsp;&nbsp;</th>
                                            <td>
                                                <asp:Label ID="distname" runat="server" Text=""></asp:Label>
                                                <input type="hidden" id="hiddistid" />
                                            </td>
                                            <th>&nbsp;&nbsp;&nbsp;Mobile : &nbsp;&nbsp;</th>
                                            <td>
                                                <asp:Label ID="lblMob" runat="server" Text=""></asp:Label>&nbsp;&nbsp; </td>
                                        </tr>

                                        <tr>

                                            <th>Order DocId : &nbsp;&nbsp;</th>
                                            <td>
                                                <asp:Label ID="lblDocid" runat="server" Text=""></asp:Label>&nbsp;&nbsp; </td>
                                            <th>&nbsp;&nbsp;&nbsp;Order Date : &nbsp;&nbsp;</th>
                                            <td>
                                                <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>&nbsp;&nbsp; </td>
                                        </tr>

                                        <tr>
                                            <th>Vehicle No. : &nbsp;&nbsp;</th>
                                            <td>
                                                <input type="text" id="Txtvechicle" class="form-control"
                                                    placeholder="Enter Vechicle Number" maxlength="15"/></td>
                                            <%-- <th>Order Amount : &nbsp;&nbsp;</th>
                                            <td>
                                                <asp:Label ID="lblOrderamt" runat="server" Text=""></asp:Label>&nbsp;&nbsp; </td>--%>
                                        </tr>
                                    </table>
                                </div>

                                <div class="row">
                                    <div id="Div2">
                                        <%-- <div class="col-lg-5 col-md-6 col-sm-8 col-xs-10">--%>
                                        <div class="box-body table-responsive" style="overflow-x: hidden">
                                            <asp:Repeater ID="rpt" runat="server">
                                                <HeaderTemplate>
                                                    <table id="example1" class="table table-bordered table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>Docid</th>
                                                                <th>ItemId</th>
                                                                <th>Item Name</th>
                                                                <th style="text-align: right;">Order Qty</th>
                                                                <th style="text-align: right;">Rate</th>
                                                                     <th style="text-align: right;">Disc%</th>
                                                                <th style="text-align: right;">Tax</th>
                                                           

                                                                <th style="text-align: right;">Order Amount</th>
                                                                <th>Discount</th>
                                                                <%--  <th style="text-align: Left;">Edit</th>--%>
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
                                <div class="row">

                                    <div class="col-md-6">
                                        Other Charges <a class="btn btn-primary btn-sm" style="float: right" onclick="openchargeModel();">Add</a>
                                        <div style="height: 100px;overflow-y: auto;overflow-x: hidden;    margin-top: 12px;">
                                        <table id="tblothercharge" class="table table-striped table-bordered" style="width: 110%;">
                                            <thead>
                                                <tr>
                                                    <th style="display: none;">Id</th>
                                                    <th>Name</th>
                                                    <th>Amount</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                        </table>
                                            </div>
                                    </div>
                                    <div class="col-md-6">
                                        <b>Remark:</b><br />
                                       <%-- //&nbsp;&nbsp;<label for="requiredFields" style="color: red;">*</label>--%>
                                        <textarea id="TextArea1" class="form-control" style="resize: none; height: 80px; width: 348px;" cols="20" rows="2"
                                            placeholder="Enter Remark"></textarea>
                                    </div>
                                    <%-- <table>

                                        <tr>
                                            <td></td>
                                            <td>
                                              
                                        </tr>


                                    </table>--%>
                                </div>

                                <div class="row">
                                    
                                <div class="row box-body table-responsive" style="margin-left: 1%;">
                                    <table>
                                        <tr>

                                            <th>Total Amount  : &nbsp;&nbsp;</th>
                                            <td>
                                                <asp:Label ID="Labeltotal" runat="server" Text=""></asp:Label>
                                               
                                            </td>
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
                            <button id="btnsave" type="button" class="btn btn-primary" onclick="savesalesinvoice()">Save</button>
                            <button id="modalClose" type="button" class="btn btn-danger">Close</button>
                        </div>

                    </div>
                </div>
            </div>
        </div>


        <div>

            <!-- The Modal -->
            <div class="modal" id="myModal2" data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content" style="width: 120%; overflow-y: auto !important;">

                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Update Tax</h4>
                            <button id="modalClose2" type="button" class="close" style="margin-top: -28px !important;">&times;</button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body">
                            <div class="box-body">



                                <div class="row box-body table-responsive">
                                    <table class="table table-bordered">
                                        <thead>
                                            <tr>
                                                <th>Tax </th>
                                                <th>Per </th>
                                                <th>Amount </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <select id="lsttaxtype" class="form-control" onchange="GetPercentage(this)">
                                                        <option selected="selected" value="S">--Select--</option>
                                                        <option value="CGST">CGST</option>
                                                        <option value="SGST">SGST</option>
                                                        <option value="IGST">IGST</option>
                                                    </select></td>
                                                <td>
                                                    <label id="perlblrow1"></label>
                                                </td>
                                                <td>
                                                    <label id="perlblamount1"></label>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <select id="lsttaxtype1" class="form-control" onchange="GetPercentage(this)">

                                                        <option selected="selected" value="S">--Select--</option>
                                                        <option value="CGST">CGST</option>
                                                        <option value="SGST">SGST</option>
                                                        <option value="IGST">IGST</option>

                                                    </select></td>
                                                <td>
                                                    <label id="perlblrow2"></label>
                                                </td>
                                                <td>
                                                    <label id="perlblamount2"></label>
                                                </td>

                                            </tr>
                                        </tbody>
                                    </table>
                                </div>


                            </div>
                        </div>

                        <!-- Modal footer -->
                        <div class="modal-footer">

                            <button id="btnsave3" type="button" class="btn btn-primary" onclick="updatetax()">Update Tax</button>
                            <button id="modalClose23" type="button" class="btn btn-danger">Close</button>
                        </div>

                    </div>
                </div>
            </div>



            <div class="modal" id="myModal3" data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog">
                    <div class="modal-content" style="width: 100%; overflow-y: auto !important;">

                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Other Charges</h4>
                            <button id="modalClose3" type="button" class="close" style="margin-top: -28px !important;">&times;</button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body">
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <select id="lstothercharges" class="form-control">
                                        </select>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <input type="text" id="txtcharges" class="form-control numeric text-right" maxlength="12" placeholder="Enter Amount" />
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="modal-footer">

                            <button id="btnadd" type="button" class="btn btn-primary" onclick="AddCharges()">Add</button>
                            <button id="modalCcancel" type="button" class="btn btn-danger">Cancel</button>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </section>

</asp:Content>
