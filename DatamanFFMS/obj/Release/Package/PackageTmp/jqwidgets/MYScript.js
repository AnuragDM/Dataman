function CRM(Query, ExportCsVName, Headertitle,Stype,compititorid, smid, visitdate) {
    var Final;
    var orderTQty=0;
    var orderTAmount = 0;
    var orderNetAmount = 0;
        //alert(smid);
    //alert(visitdate);
    $.ajax({
        type: "POST",
        url: "And_Sync.asmx/GetAllDetails",
        data: '{Query: "' + Query + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
           
            var V = JSON.parse(data.d);
            var ColNM = V[0].ColNm;

            var remkcol;
            document.getElementById('lblremark').innerHTML = "";

            //alert(ColNM);
            // $('#divHtml').remove();
            var Table = '<table id="tblId" class="table table-bordered table-striped">';
            //var Table = '<table id="tblId" class="table table-striped">';
            //var Table = '<table id="tblId" class="table table-striped">';
            var CountCol = ColNM.split('|');
            //Table += '<thead><tr class="myHeading"><th>Name</th><th>Vikram</th></tr><tr>';
            Table += '<thead><tr>';
        //    alert(Table);
            for (var colD = 0; colD < CountCol.length; colD++)
            {
                if (CountCol[colD].trim() == 'Remarks') {
                    remkcol = colD;

                }
                else {
                    Table += '<th>' + CountCol[colD].trim() + '</th>';
                }

              
              //  Table += '<th>' + CountCol[colD].trim() + '</th>';
            }
            
           // Table += '</tr></thead><tbody>';

            Table += '</tr></thead><tbody>';
          
            //Table += '</tr>';
            for (var XdD = 0; XdD < V.length; XdD++) {
                Table += '<tr>';
               
                if(V[XdD].Value=== "")
                {
                    for (var colD = 0; colD < CountCol.length; colD++) {
                        if (CountCol[colD].trim() == 'Remarks') {
                            //ok                     
                        }
                        else {
                            Table += '<td></td>';
                        }
                       // Table += '<td></td>';
                    }
                }
                else
                    {
                    var Rowdata = V[XdD].Value.split('|');                 
              
                    for (var RoCount = 0; RoCount < Rowdata.length; RoCount++)
                    {
                        if (RoCount == 0 && RoCount != remkcol) {
                            Table += '<td><p>' + Rowdata[RoCount].trim() + '</p></td>';
                        }
                        else if (RoCount != remkcol) {
                            Table += '<td><p>' + Rowdata[RoCount].trim() + '</p></td>';
                        }
                        else if (remkcol != 0 && RoCount == remkcol) {
                            document.getElementById('lblremark').innerHTML = Rowdata[remkcol].trim();
                        }

                    //if(Rowdata[RoCount]=='Yes')
                   // Table += '<td><p>' + Rowdata[RoCount].trim() + '</p></td>';
                    //Table += '<td class="WrapText colPad">' + Rowdata[RoCount].trim() + '</td>';
                    
                    if (Stype.toLowerCase().replace(" ", "") == 'competitor' || Stype.toLowerCase().replace(" ", "") == 'sample') {
                        if (RoCount == 1) orderTQty = (parseInt(orderTQty) + parseInt(Rowdata[RoCount].trim()));
                        if (RoCount == 3) orderTAmount = (parseInt(orderTAmount) + parseInt(Rowdata[RoCount].trim()));
                        //alert(orderTQty);
                        //alert(orderTAmount);
                    }
                    else if (Stype.toLowerCase().replace(" ", "") == 'order') {
                        //if (RoCount == 1) orderTQty = (parseInt(orderTQty) + parseInt(Rowdata[RoCount].trim()));
                        //if (RoCount == 7) orderTAmount = (parseInt(orderTAmount) + parseInt(Rowdata[RoCount].trim()));
                        if (RoCount == 7) orderTAmount = (parseFloat(orderTAmount) + parseFloat(Rowdata[RoCount].trim()));
                        if (RoCount == 8) orderNetAmount = (parseFloat(orderNetAmount) + parseFloat(Rowdata[RoCount].trim()));
                        //alert(orderTQty);
                        //alert(orderTAmount);
                    }
                    else if (Stype.toLowerCase().replace(" ", "") == 'distributorstock') if (RoCount == 1) orderTQty = (parseInt(orderTQty) + parseInt(Rowdata[RoCount].trim()));
                }
                  
            }
           
                Table += '</tr>';
                //alert(Stype);
            }
          
         
           
            if (Stype.toLowerCase().replace(" ", "") == 'competitor' || Stype.toLowerCase().replace(" ", "") == 'sample') {
                Table += '</tbody><tfoot><tr><td style="text-align: right;font-weight:bold;">Total:</td><td><b>' + orderTQty + '</b></td><td></td><td><b>' + orderTAmount + '</b></td></tr></tfoot>';
            }
            else if (Stype.toLowerCase().replace(" ", "") == 'order') {
                //Table += '</tbody><tfoot><tr><td style="text-align: right;font-weight:bold;">Total:</td><td>' + orderTQty + '</td><td></td><td></td><td></td><td></td><td></td><td></td><td>' + orderTAmount + '</td><td>' + orderNetAmount + '</td></tr></tfoot>';
                Table += '</tbody><tfoot><tr><td style="text-align: right;font-weight:bold;">Total:</td><td></td><td></td><td></td><td></td><td></td><td></td><td><b>' + orderTAmount + '</b></td><td><b>' + orderNetAmount + '</b></td></tr></tfoot>';
            }
            else if (Stype.toLowerCase().replace(" ", "") == 'distributorstock') {
                Table += '</tbody><tfoot><tr><td style="text-align: right;font-weight:bold;">Total:</td><td><b>' + orderTQty + '</b></td></tr></tfoot>';
            }
            else {
                Table += '</tbody>';
            }
            //alert(Table);
            //Table += '</tbody><tfoot><tr><th colspan="1" style="text-align: right">Total:</th><th style="text-align: left"></th><th style="text-align: left"></th>              <th style="text-align: left"></th></tr></tfoot>';
            //Table += '</tbody>';

            //Table += '</table><div class="row" style="width:100%; text-align: right; margin-top:3px;">';
            Table += '</table><div class="row" style="width:100%; margin-top:3px;">';
           
            //$('#divabhi').innerHTML(Table);
            // Final = '<div id="divHtml"> ' + Table;  
            var images = '<div class="col-md-8"  style=" text-align: Left;">';
            if (Stype.toLowerCase().replace(" ", "") == 'competitor') {
                $.ajax({
                    type: "POST",
                    url: "And_Sync.asmx/Getcompetitorimage",
                    async: false,
                    //data: '{compid: ' + compititorid + ', smid: ' + smid + '}',
                    data: "{'compid':" + compititorid + ",'smid':'" + smid + "','visitdate':'" + visitdate + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        var img = JSON.parse(data.d);
                        console.log(JSON.parse(data.d));
                        for (var i = 0; i < img.length; i++) {
                            images = images + " <img src='" + img[i].Imgurl + "' alt='No image' height='150' width='150' >";
                        }


                    }
                })
                images = images + '</div>';
                Table += images;


            }          
            Table += '<div class=" col-md-12 " style="text-align: right;"><button id="btnExcelsssss" class="btn btn-primary" onclick="ExpExcel(' + "'" + '' + ExportCsVName + '' + "'" + ');" type="button">Excel</button></div></div>';
            Final = '<div class="modal-body">  ' + Table + '  </div>' 
            //css({"background-color": "yellow", "font-size": "200%"});    
            // $('#divHtml').css({ 'background-color': "yellow", 'font-size': '200%' });
           
            $("#DivModalBody").html(Table);

          
            //document.getElementById('divabhi').style.display = "block";
            document.getElementById("btnmodal").click()
          //  $("#DivModalBody table ").DataTable({});

          
            $("#DivModalBody table ").DataTable({});//DivHeaderTitle
            document.getElementById('DivHeaderTitle').innerHTML = Headertitle;
           
           // $(Final).dialog({ "width": "80%" });
        },
        failure: function (response) {
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
function Loaddiv(url)
{
    var img = new Image();
    var bcgDiv = document.getElementById("divBackground");
    var imgDiv = document.getElementById("divImage");
    var imgFull = document.getElementById("imgFull");
    var imgLoader = document.getElementById("imgLoader");
    imgLoader.style.display = "block";
    img.onload = function () {
        imgFull.src = img.src;
        imgFull.style.display = "block";
        imgLoader.style.display = "none";
    };
    img.src = url;
    var width = document.body.clientWidth;
    if (document.body.clientHeight > document.body.scrollHeight) {
        bcgDiv.style.height = document.body.clientHeight + "px";
    }
    else {
        bcgDiv.style.height = document.body.scrollHeight + "px";
    }
    imgDiv.style.left = (width - 650) / 2 + "px";
    imgDiv.style.top = "20px";
    bcgDiv.style.width = "100%";

    bcgDiv.style.display = "block";
    imgDiv.style.display = "block";
    return false;
}
$(function () {
  
    debugger;
$('#tblId').DataTable({
    "footerCallback": function (row, data, start, end, display) {
        var api = this.api(), data;

        // Remove the formatting to get integer data for summation
        var intVal = function (i) {
            return typeof i === 'string' ?
                i.replace(/[\$,]/g, '') * 1 :
                typeof i === 'number' ?
                i : 0;
        };
        alert("Name1");
        // Total over all pages
        total = api
            .column(1)
            .data()
            .reduce(function (a, b) {
                return intVal(a) + intVal(b);
            }, 0);

        // Total over this page
        pageTotal = api
            .column(1, { page: 'current' })
            .data()
            .reduce(function (a, b) {
                return intVal(a) + intVal(b);
            }, 0);

        // Update footer
        $(api.column(1).footer()).html(
            '$' + pageTotal + ' ( $' + total + ' total)'
        );
    }
});
});
function ExpExcel(ExportCsVName) {
    //if (TableId == '')
    //    return false;
    let ros = $('#' + $('#tblId').attr('id') + ' > tbody > tr ');
    for (let inX = 0; inX < ros.length; inX++) {
        let hasRo2 = $(ros[inX]).hasClass('HidRo2');
        if (hasRo2 == true)
            $(ros[inX]).addClass('HidRo1');
    }

    $('#tblId').table2excel({
        //filename: 'ExcExp',
        filename: ExportCsVName + '.xls',
        exclude: '.HidRo1'
    });

    return false;
}


