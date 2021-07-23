<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="DSRReport.aspx.cs" Inherits="AstralFFMS.DSRReport" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <%--  <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/jquery.tableTotal.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>--%>
<%--      <script src="jqwidgets/MYScript.js"></script>
    <script src="jqwidgets/jquery.table2excel.js"></script>--%>
<%--     <link type="text/css" rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <link href="plugins/multiselect.css" rel="stylesheet" />
    <script src="plugins/multiselect.js"></script>--%>
    <style type="text/css">
        .excelbtn, #ContentPlaceHolder1_btnBack {
            background-color: #3c8dbc;
            border-color: #367fa9;
        }

        #excelExport {
            border-radius: 3px;
            -webkit-box-shadow: none;
            box-shadow: none;
            border: 1px solid transparent;
            background-color: #3c8dbc;
            border-color: #367fa9;
            color: white;
            height: 35px;
            padding: 2px 6px 4px 6px;
        }
         .WrapText {  
            width: 100%;  
            word-break: break-all;  
        }
         .colPad {
             padding-left:10px !important;
         }
        input[type=text].jqx-input {
            text-align: center !important;
        }
        .table tr td p {
    white-space: normal !important;
}
          #example1_wrapper .row {
            margin-right: 0px !important;
            margin-left: 0px !important;
        }

            #example1_wrapper .row .col-sm-12 {
                overflow-x: scroll !important;
                padding-left: 0px !important;
                margin-bottom: 10px;
            }

           /*.modalBackground {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }*/

        /*.modalPopup {
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
        }*/
    </style>
    <script type="text/javascript">
        $(function () {
            $('#cmodalClose').on('click', function () {
                $('#ContentPlaceHolder1_ItemDetail').hide();
            })
            $('#modalClose1').on('click', function () {
                $('#ContentPlaceHolder1_ItemDetail').hide();
            })
        });
    </script>
      <script type="text/javascript">

      function GetOrder(r) {
          debugger;
          var Query;
          var csvName;
          var HeaderName;
          var salesperson = document.getElementById('<%=saleRepName.ClientID %>').innerText; 
          var getUrl = getParams(window.location.href);
          var currentRow = $(r).parents("tr");
          //var lblSType = currentRow.find("span[id*='lblSType']").text();
          //var lblPartyName = currentRow.find("span[id*='lblPartyName']").text();
          //var lblCOMPTID = currentRow.find("span[id*='lblCOMPTID']").text();
          ////var lblParty = currentRow.find("span[id*='lblPartyId']").text();
          //var lblVisitDate = currentRow.find("span[id*='lblVisitDate']").text();
          
          var lblParty = $(r).parent().parent().find('[id*="hfParty"]').val();
          var lblSType = $(r).parent().parent().find('[id*="sTypeHdf"]').val();
          var lblCOMPTID = $(r).parent().parent().find('[id*="COMPTID"]').val();
          var lblVisitDate = $(r).parent().parent().find('[id*="hfVisitDate"]').val();
          var lblPartyName = $(r).parent().parent().find('[id*="hfPartyName"]').val();
          //var smidhidden = $(r).parent().parent().find('[id*="smIDHiddenField"]').val();
          var lblOrderTakenType = $(r).parent().parent().find('[id*="hfOrderTakenType"]').val();
          var lblExpectedDD = $(r).parent().parent().find('[id*="hfExpectedDD"]').val();
          //alert(lblOrderTakenType);
                  

          if (lblOrderTakenType != "")
          {            
              $("#faicon").css("visibility", "visible");             
          }
          else
          {              
              $("#faicon").css("visibility", "hidden");
          }

          if (lblSType.toLowerCase().replace(" ", "") == 'order')
          {             
              $("#divordertaken").css("visibility", "visible");
              $("#divestmdate").css("visibility", "visible");
          }
          else
          {             
              $("#divordertaken").css("visibility", "hidden");
              $("#divestmdate").css("visibility", "hidden");
          }
          
          
          var smidhidden = getUrl.SMID;
          //alert(smidhidden);
          
          //Label1.innerText = lblPartyName.val;
          var smId = getUrl.SMID;
          var lockType = getUrl.Recstatus;
          HeaderName = "";
          csvName = "";       
          $('#<%=Label1.ClientID%>').text(lblPartyName);
          $('#<%=lblExpectedDD.ClientID%>').text(lblExpectedDD);
          
          //$("#Label1").text(lblPartyName);
          if (lblSType.toLowerCase().replace(" ", "") == 'order') {
              //Query = "select cast(Smid as varchar)+'|'+ Smname +'|'+email+'|'+Mobile+'|'+cast(createddate as varchar) as [Smid|Salesperson Name|Email|Mobile|Creatyed Date]  from mastsalesrep";

              //Query = "select i.Itemname +'|'+ cast(os.Qty as varchar) +'|'+ Case when os.BaseUnitQty !=0 then cast(Isnull(os.BaseUnitQty,0) as varchar)+ ' ' +os.BaseUnit else ''  end + ' ' + Case when os.PrimaryUnitQty !=0 then cast(Isnull(os.PrimaryUnitQty,0) as varchar)+ ' ' +os.PrimaryUnit else ''  end + ' ' + Case when os.SecondaryUnitQty !=0 then cast(Isnull(os.SecondaryUnitQty,0) as varchar)+ ' ' +os.SecondaryUnit else ''  end  +'|'+ cast(os.Rate as varchar) +'|'+ cast(Isnull(os.MarginPercentage,0) as varchar) +'|'+ cast(Isnull(os.Discount,0) as varchar) +'|'+ ISNULL(os.DiscountType,'') +'|'+ cast(Isnull(os.DiscountAmount,0) as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) -cast(Isnull(os.DiscountAmount,0) as varchar) as varchar) as [ItemName|Qty|Description Qty|Rate|Margin|Discount%|DiscountType|DiscountAmount|Order Amount|Net Amount] from TransOrder1 os LEFT JOIN transorder os1 ON os.orddocid=os1.orddocid LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.OrdId=" + lblCOMPTID + " and os.smid=" + smidhidden + "" +
              //" group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount,MarginPercentage,DiscountAmount,os.BaseUnitQty,os.BaseUnit,os.PrimaryUnitQty,os.PrimaryUnit,os.SecondaryUnitQty,os.SecondaryUnit,os.DiscountType " +
              // "Union "+
              // "select i.Itemname +'|'+ cast(os.Qty as varchar) +'|'+ Case when os.BaseUnitQty !=0 then cast(Isnull(os.BaseUnitQty,0) as varchar)+ ' ' +os.BaseUnit else ''  end + ' ' + Case when os.PrimaryUnitQty !=0 then cast(Isnull(os.PrimaryUnitQty,0) as varchar)+ ' ' +os.PrimaryUnit else ''  end + ' ' + Case when os.SecondaryUnitQty !=0 then cast(Isnull(os.SecondaryUnitQty,0) as varchar)+ ' ' +os.SecondaryUnit else ''  end  +'|'+ cast(os.Rate as varchar) +'|'+ cast(Isnull(os.MarginPercentage,0) as varchar) +'|'+ cast(Isnull(os.Discount,0) as varchar) +'|'+ ISNULL(os.DiscountType,'') +'|'+ cast(Isnull(os.DiscountAmount,0) as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) -cast(Isnull(os.DiscountAmount,0) as varchar) as varchar) as [ItemName|Qty|Description Qty|Rate|Margin|Discount%|DiscountType|DiscountAmount|Order Amount|Net Amount] from Temp_TransOrder1 os LEFT JOIN Temp_transorder os1 ON os.orddocid=os1.orddocid LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId   left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.OrdId=" + lblCOMPTID + " and os.smid=" + smidhidden + "" +
              // " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount,MarginPercentage,DiscountAmount,os.BaseUnitQty,os.BaseUnit,os.PrimaryUnitQty,os.PrimaryUnit,os.SecondaryUnitQty,os.SecondaryUnit,os.DiscountType";

             // Query = "select i.Itemname +'|'+ Case when os.BaseUnitQty !=0 then cast(Isnull(os.BaseUnitQty,0) as varchar)+ ' ' +os.BaseUnit else ''  end + ' ' + Case when os.PrimaryUnitQty !=0 then cast(Isnull(os.PrimaryUnitQty,0) as varchar)+ ' ' +os.PrimaryUnit else ''  end + ' ' + Case when os.SecondaryUnitQty !=0 then cast(Isnull(os.SecondaryUnitQty,0) as varchar)+ ' ' +os.SecondaryUnit else ''  end  +'|'+ cast(os.Rate as varchar) +'|'+ cast(Isnull(os.MarginPercentage,0) as varchar) +'|'+ cast(Isnull(os.Discount,0) as varchar) +'|'+ ISNULL(os.DiscountType,'') +'|'+ cast(Isnull(os.DiscountAmount,0) as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) -cast(Isnull(os.DiscountAmount,0) as varchar) as varchar) as [ItemName|Description Qty|Rate|Margin|Discount%|DiscountType|DiscountAmount|Order Amount|Net Amount] from TransOrder1 os LEFT JOIN transorder os1 ON os.orddocid=os1.orddocid LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.OrdId=" + lblCOMPTID + " and os.smid=" + smidhidden + "" +
             //" group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount,MarginPercentage,DiscountAmount,os.BaseUnitQty,os.BaseUnit,os.PrimaryUnitQty,os.PrimaryUnit,os.SecondaryUnitQty,os.SecondaryUnit,os.DiscountType " +
             // "Union " +
             // "select i.Itemname +'|'+ Case when os.BaseUnitQty !=0 then cast(Isnull(os.BaseUnitQty,0) as varchar)+ ' ' +os.BaseUnit else ''  end + ' ' + Case when os.PrimaryUnitQty !=0 then cast(Isnull(os.PrimaryUnitQty,0) as varchar)+ ' ' +os.PrimaryUnit else ''  end + ' ' + Case when os.SecondaryUnitQty !=0 then cast(Isnull(os.SecondaryUnitQty,0) as varchar)+ ' ' +os.SecondaryUnit else ''  end  +'|'+ cast(os.Rate as varchar) +'|'+ cast(Isnull(os.MarginPercentage,0) as varchar) +'|'+ cast(Isnull(os.Discount,0) as varchar) +'|'+ ISNULL(os.DiscountType,'') +'|'+ cast(Isnull(os.DiscountAmount,0) as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) -cast(Isnull(os.DiscountAmount,0) as varchar) as varchar) as [ItemName|Description Qty|Rate|Margin|Discount%|DiscountType|DiscountAmount|Order Amount|Net Amount] from Temp_TransOrder1 os LEFT JOIN Temp_transorder os1 ON os.orddocid=os1.orddocid LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId   left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.OrdId=" + lblCOMPTID + " and os.smid=" + smidhidden + "" +
             // " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount,MarginPercentage,DiscountAmount,os.BaseUnitQty,os.BaseUnit,os.PrimaryUnitQty,os.PrimaryUnit,os.SecondaryUnitQty,os.SecondaryUnit,os.DiscountType";

              Query = "select i.Itemname +'|'+ Case when os.BaseUnitQty !=0 then cast(Isnull(os.BaseUnitQty,0) as varchar)+ ' ' +os.BaseUnit else ''  end + ' ' + Case when os.PrimaryUnitQty !=0 then cast(Isnull(os.PrimaryUnitQty,0) as varchar)+ ' ' +os.PrimaryUnit else ''  end + ' ' + Case when os.SecondaryUnitQty !=0 then cast(Isnull(os.SecondaryUnitQty,0) as varchar)+ ' ' +os.SecondaryUnit else ''  end  +'|'+ cast(os.Rate as varchar) +'|'+ cast(Isnull(os.MarginPercentage,0) as varchar) +'|'+ cast(Isnull(os.Discount,0) as varchar) +'|'+ ISNULL(os.DiscountType,'') +'|'+ cast(Isnull(os.DiscountAmount,0) as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) -cast(Isnull(os.DiscountAmount,0) as varchar) as varchar)+'|'+ os1.Remarks as [ItemName|Description Qty|Rate|Margin|Discount%|DiscountType|DiscountAmount|Order Amount|Net Amount|Remarks] from TransOrder1 os LEFT JOIN transorder os1 ON os.orddocid=os1.orddocid LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.OrdId=" + lblCOMPTID + " and os.smid=" + smidhidden + "" +
          " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount,MarginPercentage,DiscountAmount,os.BaseUnitQty,os.BaseUnit,os.PrimaryUnitQty,os.PrimaryUnit,os.SecondaryUnitQty,os.SecondaryUnit,os.DiscountType " +
           "Union " +
           "select i.Itemname +'|'+ Case when os.BaseUnitQty !=0 then cast(Isnull(os.BaseUnitQty,0) as varchar)+ ' ' +os.BaseUnit else ''  end + ' ' + Case when os.PrimaryUnitQty !=0 then cast(Isnull(os.PrimaryUnitQty,0) as varchar)+ ' ' +os.PrimaryUnit else ''  end + ' ' + Case when os.SecondaryUnitQty !=0 then cast(Isnull(os.SecondaryUnitQty,0) as varchar)+ ' ' +os.SecondaryUnit else ''  end  +'|'+ cast(os.Rate as varchar) +'|'+ cast(Isnull(os.MarginPercentage,0) as varchar) +'|'+ cast(Isnull(os.Discount,0) as varchar) +'|'+ ISNULL(os.DiscountType,'') +'|'+ cast(Isnull(os.DiscountAmount,0) as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) -cast(Isnull(os.DiscountAmount,0) as varchar) as varchar)+'|'+ os1.Remarks as [ItemName|Description Qty|Rate|Margin|Discount%|DiscountType|DiscountAmount|Order Amount|Net Amount|Remarks] from Temp_TransOrder1 os LEFT JOIN Temp_transorder os1 ON os.orddocid=os1.orddocid LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId   left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.OrdId=" + lblCOMPTID + " and os.smid=" + smidhidden + "" +
           " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount,MarginPercentage,DiscountAmount,os.BaseUnitQty,os.BaseUnit,os.PrimaryUnitQty,os.PrimaryUnit,os.SecondaryUnitQty,os.SecondaryUnit,os.DiscountType";

               HeaderName = "Order Detail";
               csvName = "Order Detail" + '/' + salesperson + '/' + lblPartyName + '/' + lblVisitDate;
          }
          else if (lblSType.toLowerCase().replace(" ", "") == 'demo')
          {
              Query = "select cast(ic.name as varchar) +'|'+ cast(ms.name as varchar) +'|'+i.ItemName+'|'+d.Remarks as [ProductClass|Segment|MaterialGroup|Remarks] from TransDemo d left JOIN MastItemClass ic ON d.ProductClassId=ic.Id LEFT JOIN mastitemsegment ms ON d.ProductSegmentId=ms.Id LEFT join MastItem i on i.ItemId=d.ProductMatGrp inner join MastParty p on p.PartyId=d.PartyId left join TransVisit vl1 on vl1.SMId=d.SMId AND d.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where DemoId=" + lblCOMPTID + " and d.smid=" + smidhidden + "" +
                " Union "+
              " select cast(ic.name as varchar) +'|'+ cast(ms.name as varchar) +'|'+i.ItemName+'|'+d.Remarks as [ProductClass|Segment|MaterialGroup|Remarks] from Temp_TransDemo d left JOIN MastItemClass ic ON d.ProductClassId=ic.Id LEFT JOIN mastitemsegment ms ON d.ProductSegmentId=ms.Id LEFT join MastItem i on i.ItemId=d.ProductMatGrp inner join MastParty p on p.PartyId=d.PartyId  left join TransVisit vl1 on vl1.SMId=d.SMId AND d.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where DemoId=" + lblCOMPTID + " and d.smid=" + smidhidden + "";
                HeaderName = "Demo Detail";
                csvName = "Demo Detail" + salesperson + lblPartyName + lblVisitDate;

          }
          else if (lblSType.toLowerCase().replace(" ", "") == 'non-productive') {
              Query = "select CONVERT (varchar,fv.Nextvisit,106)+'|'+fv.VisitTime+'|'+mfv.FVName+'|'+fv.Remarks as [NextVisitDate|NextVisitTime|Non-Productive Reason|Remarks]  from TransFailedVisit fv left join MastFailedVisitRemark mfv on fv.ReasonID=mfv.FVId inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.AreaId  left join TransVisit vl1 on vl1.SMId=fv.SMId and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where fv.FVId=" + lblCOMPTID + " and fv.smid=" + smidhidden + "" +
                " Union "+
              "select CONVERT (varchar,fv.Nextvisit,106)+'|'+fv.VisitTime+'|'+mfv.FVName+'|'+fv.Remarks as [NextVisitDate|NextVisitTime|Non-Productive Reason|Remarks] from Temp_TransFailedVisit fv left join MastFailedVisitRemark mfv on fv.ReasonID=mfv.FVId inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.AreaId  left join TransVisit vl1 on vl1.SMId=fv.SMId and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where fv.FVId=" + lblCOMPTID + " and fv.smid=" + smidhidden + "";
              HeaderName = "Party Non-Productive Detail";
              csvName = "Party Non-Productive Detail" + salesperson + lblPartyName + lblVisitDate;
          }
          else if (lblSType.toLowerCase().replace(" ", "") == 'competitor') {
              Query = "select tc.item +'|'+ cast(tc.Qty as varchar) +'|'+ cast(tc.Rate as varchar)+'|'+ cast(tc.Discount as varchar) +'|'+tc.CompName+'|'+tc.Remarks+'|'+ tc.BrandActivity+'|'+tc.MeetActivity+'|'+tc.RoadShow+'|'+tc.[Scheme/offers]+'|'+tc.OtherGeneralInfo as [Item|Qty|Rate|Discount|Competitor|Remarks|Brand Activity|MeetActivity|RoadShow|Scheme/offers|OtherGeneralInfo] from TransCompetitor tc left join MastParty p on p.PartyId=tc.PartyId left join MastArea b on b.AreaId=p.AreaId left join TransVisit vl1 on vl1.SMId=tc.SMId AND tc.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where TC.COMPTID=" + lblCOMPTID + " and TC.smid=" + smidhidden + "" +
              " Union "+
              "select tc.item +'|'+ cast(tc.Qty as varchar) +'|'+ cast(tc.Rate as varchar)+'|'+ cast(tc.Discount as varchar) +'|'+tc.CompName+'|'+tc.Remarks+'|'+ tc.BrandActivity+'|'+tc.MeetActivity+'|'+tc.RoadShow+'|'+tc.[Scheme/offers]+'|'+tc.OtherGeneralInfo as [Item|Qty|Rate|Discount|Competitor|Remarks|Brand Activity|MeetActivity|RoadShow|Scheme/offers|OtherGeneralInfo] from Temp_TransCompetitor tc left join MastParty p on p.PartyId=tc.PartyId left join MastArea b on b.AreaId=p.AreaId left join TransVisit vl1 on vl1.SMId=tc.SMId AND tc.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId  left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where TC.COMPTID=" + lblCOMPTID + " and TC.smid=" + smidhidden + "";
              HeaderName = "Competitor Detail";
              csvName = "Competitor Detail" + salesperson + lblPartyName + lblVisitDate;

          }
          else if (lblSType.toLowerCase().replace(" ", "") == 'retailercollection') {
              Query = "SELECT cast(sum(Convert(numeric(18,2), tc.Amount)) as varchar)+'|'+tc.Cheque_DDNo+'|'+case when tc.Cheque_DD_Date='1900-01-01 00:00:00.000'then '' else CONVERT (varchar,tc.Cheque_DD_Date,106) end+'|'+tc.Mode+'|'+tc.Bank+'|'+tc.Branch+'|'+max(tc.Remarks) AS [Amount|Cheque No.|Cheque Date|Collection Type|bank|Branch|Remarks] FROM TransCollection tc LEFT JOIN mastparty p ON tc.PartyId=p.PartyId left join MastArea b on b.AreaId=p.AreaId LEFT JOIN TransVisit vl1 on vl1.SMId=tc.SMId AND vl1.VDate=tc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where Convert(varchar,tc.PaymentDate,106)='" + lblVisitDate + "' and tc.PartyId=" + lblParty + " And tc.SMId=" + smId + " group by p.PartyId,p.partyName,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,tc.Amount,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date,tc.Cheque_DDNo,tc.Cheque_DD_Date,tc.Mode,tc.Bank,tc.Branch " +
              " Union "+
              " SELECT cast(sum(Convert(numeric(18,2), tc.Amount)) as varchar)+'|'+tc.Cheque_DDNo+'|'+case when tc.Cheque_DD_Date='1900-01-01 00:00:00.000'then '' else CONVERT (varchar,tc.Cheque_DD_Date,106) end+'|'+tc.Mode+'|'+tc.Bank+'|'+tc.Branch+'|'+max(tc.Remarks) AS [Amount|Cheque No.|Cheque Date|Collection Type|bank|Branch|Remarks] FROM Temp_TransCollection tc LEFT JOIN mastparty p ON tc.PartyId=p.PartyId left join MastArea b on b.AreaId=p.AreaId LEFT JOIN TransVisit vl1 on vl1.SMId=tc.SMId AND vl1.VDate=tc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where convert(varchar,tc.PaymentDate,106)='" + lblVisitDate + "' and tc.PartyId=" + lblParty + " And tc.SMId=" + smId + " group by tc.PaymentDate,p.PartyId,p.partyName,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,tc.Amount,tc.Latitude,tc.Longitude,tc.Address,tc.Mobile_Created_date,tc.Cheque_DDNo,tc.Cheque_DD_Date,tc.Mode,tc.Bank,tc.Branch  ";
              HeaderName = "RetailerCollection Detail";
              csvName = "RetailerCollection Detail" + salesperson + lblPartyName + lblVisitDate;
          }
          else if (lblSType.toLowerCase().replace(" ", "") == 'distributorcollection') {
              Query = "SELECT  cast(sum(CONVERT(numeric(18,2), dc.Amount)) as varchar)+'|'+dc.Cheque_DDNo+'|'+case when dc.Cheque_DD_Date='1900-01-01 00:00:00.000'then '' else CONVERT (varchar,dc.Cheque_DD_Date,106) end+'|'+dc.Mode+'|'+dc.Bank+'|'+dc.Branch+'|'+max(dc.Remarks) AS [Amount|Cheque No.|Cheque Date|Collection Type|bank|Branch|Remarks] FROM DistributerCollection Dc LEFT JOIN mastparty p ON Dc.DistId=p.PartyId left join MastArea b on b.AreaId=p.cityid LEFT JOIN TransVisit vl1 on vl1.SMId=dc.SMId   AND vl1.VDate=dc.PaymentDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId Where CONVERT (varchar,Dc.PaymentDate,106)='" + lblVisitDate + "' and PartyId=" + lblParty + " And dc.SMId=" + smId + " group by p.PartyId,p.Partyname,p.Address1,p.Mobile,p.ContactPerson,b.AreaName,Dc.Latitude,Dc.Longitude,Dc.Address,dc.Cheque_DDNo,dc.Cheque_DD_Date,dc.Mode,dc.Bank,dc.Branch";

              HeaderName = "DistributorCollection Detail";
              csvName = "DistributorCollection Detail" + salesperson + lblPartyName + lblVisitDate;
          }
          else if (lblSType.toLowerCase().replace(" ", "") == 'distributornon-productive') {
              Query = "select CONVERT (varchar,fv.Nextvisit,106)+'|'+fv.VisitTime+'|'+mfv.FVName+'|'+fv.Remarks as [NextVisitDate|NextVisitTime|Non-Productive Reason|Remarks] from TransFailedVisit fv left join MastFailedVisitRemark mfv on fv.ReasonID=mfv.FVId inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=fv.SMId and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where fv.FVId=" + lblCOMPTID + " and fv.smid=" + smidhidden + "" +
                " Union "+
                " select CONVERT (varchar,fv.Nextvisit,106)+'|'+fv.VisitTime+'|'+mfv.FVName+'|'+fv.Remarks as [NextVisitDate|NextVisitTime|Non-Productive Reason|Remarks] from Temp_TransFailedVisit fv left join MastFailedVisitRemark mfv on fv.ReasonID=mfv.FVId inner join MastParty p ON p.PartyId=fv.PartyId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=fv.SMId and fv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where fv.FVId=" + lblCOMPTID + " and fv.smid=" + smidhidden + "";

              HeaderName = "Distributor Non-Productive Detail";
              csvName = "Distributor Non-Productive Detail" + salesperson + lblPartyName + lblVisitDate;
          }
          else if (lblSType.toLowerCase().replace(" ", "") == 'distributordiscussion') {
              Query = "select CONVERT (varchar,tv.NextVisitDate,106)+'|'+tv.NextVisitTime+'|'+tv.remarkDist+'|'+tv.SpentfrTime+'|'+tv.SpentToTime as [NextVisitDate|NextVisitTime|Remarks|Spend Fr. Time|Spend To Time] from TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=tv.SMId and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where VisDistId=" + lblCOMPTID + " and tv.smid=" + smidhidden + "" +
                " Union " +
                " select CONVERT (varchar,tv.NextVisitDate,106)+'|'+tv.NextVisitTime+'|'+tv.remarkDist+'|'+tv.SpentfrTime+'|'+tv.SpentToTime as [NextVisitDate|NextVisitTime|Remarks|Spend Fr. Time|Spend To Time] from Temp_TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=tv.SMId and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where VisDistId=" + lblCOMPTID + " and tv.smid=" + smidhidden + "";
          
              HeaderName = "DistributorDiscussion Detail";
              csvName = "DistributorDiscussion Detail" + salesperson + lblPartyName + lblVisitDate;

          }
          else if (lblSType.toLowerCase().replace(" ", "") == 'retailerdiscussion') {
              Query = "select CONVERT (varchar,tv.NextVisitDate,106)+'|'+tv.NextVisitTime+'|'+tv.remarkDist+'|'+tv.SpentfrTime+'|'+tv.SpentToTime as [NextVisitDate|NextVisitTime|Remarks|Spend Fr. Time|Spend To Time]  from TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=tv.SMId and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where VisDistId=" + lblCOMPTID + " and tv.smid=" + smidhidden + "" +
              " Union " +
             " select CONVERT (varchar,tv.NextVisitDate,106)+'|'+tv.NextVisitTime+'|'+tv.remarkDist+'|'+tv.SpentfrTime+'|'+tv.SpentToTime as [NextVisitDate|NextVisitTime|Remarks|Spend Fr. Time|Spend To Time] from Temp_TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=tv.SMId and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where VisDistId=" + lblCOMPTID + " and tv.smid=" + smidhidden + "";
              
              HeaderName = "RetailerDiscussion Detail";
              csvName = "RetailerDiscussion Detail" + salesperson + lblPartyName + lblVisitDate;
          }
          else if (lblSType.toLowerCase().replace(" ", "") == 'distributorstock') {
              Query = "select i.Itemname+'|'+cast( os.Qty as varchar) +'|'+ mic.Name+'|'+mis.Name+'|'+ig.ItemName as [Item|Qty|Class|Segment|product Group]  FROM TransDistStock os LEFT Join Mastparty p on p.PartyId=os.DistId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join mastitem ig on i.Underid=ig.ItemId left join MastItemSegment mis on i.SegmentId=mis.id left join mastitemclass mic on i.ClassId=mic.Id left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where convert (varchar,os.VDate,106)='" + lblVisitDate + "' and PartyId=" + lblParty + " And os.SMId=" + smId + "" +
              " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, " + "os.VDate,os.DistId,i.Itemname,os.Qty,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.ImgUrl,mic.Name,mis.Name,ig.ItemName" +
                " Union  "+
                  "select i.Itemname+'|'+cast( os.Qty as varchar) +'|'+ mic.Name+'|'+mis.Name+'|'+ig.ItemName as [Item|Qty|Class|Segment|product Group] FROM Temp_TransDistStock os LEFT Join Mastparty p on p.PartyId=os.DistId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join mastitem ig on i.Underid=ig.ItemId left join MastItemSegment mis on i.SegmentId=mis.id left join mastitemclass mic on i.ClassId=mic.Id left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where convert (varchar,os.VDate,106)='" + lblVisitDate + "' and PartyId=" + lblParty + " And os.SMId=" + smId + "" +
              " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson, " + "os.VDate,os.DistId,i.Itemname,os.Qty,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.ImgUrl,mic.Name,mis.Name,ig.ItemName";
              HeaderName = "DistributorStock Detail";
              csvName = "DistributorStock Detail" + salesperson + lblPartyName + lblVisitDate;

          }
          else if (lblSType.toLowerCase().replace(" ", "") == 'sample') {
              //Query = "select i.Itemname +'|'+ cast(os.Qty as varchar) +'|'+ cast(os.Rate as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) as varchar) as [ItemName|Qty|Rate|Order Amount] from TransSample1 os LEFT JOIN transSample os1 ON os.SampleDocId=os1.SampleDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SampleId=" + lblCOMPTID + " and os.smid=" + smidhidden + "" +
              //" group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount"+
              //  " Union "+
              //"select  i.Itemname +'|'+ cast(os.Qty as varchar) +'|'+ cast(os.Rate as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) as varchar) as [ItemName|Qty|Rate|Order Amount] from Temp_TransSample1 os LEFT JOIN Temp_transSample os1 ON os.SampleDocId=os1.SampleDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SampleId=" + lblCOMPTID + " and os.smid=" + smidhidden + "" +
              //" group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount";

              Query = "select i.Itemname +'|'+ cast(os.Qty as varchar) +'|'+ cast(os.Rate as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) as varchar)+'|'+ os1.Remarks as [ItemName|Qty|Rate|Order Amount|Remarks] from TransSample1 os LEFT JOIN transSample os1 ON os.SampleDocId=os1.SampleDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SampleId=" + lblCOMPTID + " and os.smid=" + smidhidden + "" +
             " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount" +
               " Union " +
             "select  i.Itemname +'|'+ cast(os.Qty as varchar) +'|'+ cast(os.Rate as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) as varchar)+'|'+ os1.Remarks as [ItemName|Qty|Rate|Order Amount|Remarks] from Temp_TransSample1 os LEFT JOIN Temp_transSample os1 ON os.SampleDocId=os1.SampleDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId where os.SampleId=" + lblCOMPTID + " and os.smid=" + smidhidden + "" +
             " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount";

              HeaderName = "Sample Detail";
              csvName = "Sample Detail" + salesperson + lblPartyName + lblVisitDate;

          }
          else if (lblSType.toLowerCase().replace(" ", "") == 'salesreturn') {
              Query = "select i.Itemname +'|'+ cast(os.Qty as varchar) +'|'+ cast(os.Rate as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) as varchar)+'|'+max(os.MfDate) +'|'+max(os.BatchNo) + '|' + max(Isnull(rr.Description,''))+'|'+ os1.Remarks as [ItemName|Qty|Rate|Order Amount|MFDate|BatchNo|Reason|Remarks] from TransSalesReturn1 os LEFT JOIN TransSalesReturn os1 ON os.SRetDocId=os1.SRetDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId left join ReturnReason rr on os1.RRId=rr.RRId  where os.SRetId=" + lblCOMPTID + " and os.smid=" + smidhidden + "" +
              " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount"+
              " Union "+
              "select i.Itemname +'|'+ cast(os.Qty as varchar) +'|'+ cast(os.Rate as varchar) +'|'+cast((sum(CONVERT(numeric(18,2), os.Qty*os.Rate))) as varchar)+'|'+max(os.MfDate) +'|'+max(os.BatchNo) + '|' + max(Isnull(rr.Description,''))+'|'+ os1.Remarks as [ItemName|Qty|Rate|Order Amount|MFDate|BatchNo|Reason|Remarks] from Temp_TransSalesReturn1 os LEFT JOIN Temp_transSalesReturn os1 ON os.SRetDocId=os1.SRetDocId LEFT Join Mastparty p on p.PartyId=os.PartyId left join MastArea b on b.AreaId=p.AreaId left join mastitem i on i.ItemId=os.ItemId  left join TransVisit vl1 on vl1.SMId=os.SMId and os.VDate =vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 on cp1.SMId=vl1.nWithUserId left join ReturnReason rr on os1.RRId=rr.RRId  where os.SRetId=" + lblCOMPTID + " and os.smid=" + smidhidden + "" +
              " group by p.PartyName,p.Address1,p.Mobile,p.ContactPerson,os1.ImgUrl, os.VDate,os.PartyId,os1.Remarks,i.Itemname,os.Qty,os.Rate,p.PartyId,b.AreaName,cp.SMName,p.Created_Date,cp1.SMName,os.Latitude,os.Longitude,os.Address,os.Mobile_Created_date,os.Discount";
              HeaderName = "SalesReturn Detail";
              csvName = "SalesReturn Detail" + salesperson + lblPartyName + lblVisitDate;
          }
          else if (lblSType.toLowerCase().replace(" ", "") == 'prospectdistributor') {
              Query = "select cast(pd.partyid as varchar)+'|'+pd.PartyName+'|'+pd.Mobile+'|'+(pd.Address1+ ' - ' + pd.Address2 + ' - ' + cast(pd.pin as varchar))+'|'+pd.GSTIN+'|'+sp.SMName+'|'+cast(cast(pd.Created_Date as date) as varchar)+'|'+case when  ISNULL(pd.Approved_Rejected,'')='A' then 'Approved' when  ISNULL(pd.Approved_Rejected,'')='R' then 'Rejected' else 'Pending' end as [PartyId|PartyName|Mobile|Address|GSTIN|CreatedBy|CreatedDate|Pending]from MastProspect_Distributor pd left join MastSalesRep sp on pd.[Created UserId]=sp.UserId where sp.smid in (" + smidhidden + ")  and pd.partyid=" + lblCOMPTID + " ";

             // Query = "select CONVERT (varchar,tv.NextVisitDate,106)+'|'+tv.NextVisitTime+'|'+tv.remarkDist+'|'+tv.SpentfrTime+'|'+tv.SpentToTime as [NextVisitDate|NextVisitTime|Remarks|Spend Fr. Time|Spend To Time]  from TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=tv.SMId and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where VisDistId=" + lblCOMPTID + " and tv.smid=" + smidhidden + "" +
             // " Union " +
             //" select CONVERT (varchar,tv.NextVisitDate,106)+'|'+tv.NextVisitTime+'|'+tv.remarkDist+'|'+tv.SpentfrTime+'|'+tv.SpentToTime as [NextVisitDate|NextVisitTime|Remarks|Spend Fr. Time|Spend To Time] from Temp_TransVisitDist tv inner join MastParty p ON p.PartyId=tv.DistId left join MastArea b on b.AreaId=p.CityId  left join TransVisit vl1 on vl1.SMId=tv.SMId and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where VisDistId=" + lblCOMPTID + " and tv.smid=" + smidhidden + "";

              HeaderName = "Prodpect Distributor Detail";
              csvName = "ProdpectDistributor Detail" + salesperson + lblPartyName + lblVisitDate;
          }
          else
          {
              if (lockType.toLowerCase().replace(" ", "") == 'lock')
              {
                  Query = "select CONVERT (varchar,tv.SpentfrTime,106)+'|'+tv.SpentToTime+'|'+tv.remarkDist as [SpentfromTime|SpentToTime|Remarks] from TransVisitDist tv left join TransVisit vl1 on vl1.SMId=tv.SMId  and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId where VisDistId=" + lblCOMPTID + " and tv.smid=" + smidhidden + "";
                  }
              else{   
                  Query = " select CONVERT (varchar,tv.SpentfrTime,106)+'|'+tv.SpentToTime+'|'+tv.remarkDist as [SpentfromTime|SpentToTime|Remarks] from Temp_TransVisitDist tv left join TransVisit vl1 on vl1.SMId=tv.SMId  and tv.VDate=vl1.VDate left join MastSalesRep cp on cp.SMId=vl1.WithUserId left Join MastSalesRep cp1 ON cp1.SMId=vl1.nWithUserId  left join mastparty mp on mp.partyid=tv.distid where VisDistId=" + lblCOMPTID + " and tv.smid=" + smidhidden + "";
              }
              HeaderName = "OtherActivity Detail";
              csvName = "OtherActivity Detail" + '/' + salesperson + '/' + lblPartyName + '/' + lblVisitDate;
          }
          CRM(Query, csvName, HeaderName, lblSType, lblCOMPTID, smidhidden, lblVisitDate);
            //$find("popuptrail").show();
            ////  $("#hidopenpoufortrail").val('2');
            //$.ajax({
            //    type: "POST",
            //    url: "DSRReport.aspx/GetOrder",
            //    data: '{OrdId :"' + lblCOMPTID + '",Type:"' + lockType + '"}',
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: OnSuccess1,
            //    failure: function (response) {
            //        //alert(response.d);
            //    },
            //    error: function (response) {
            //        //alert(response.d);
            //    }
            //});
            //function OnSuccess1(response) {

            //    $('div[id$="divpopupTrail"]').show();
            //    var data = JSON.parse(response.d);
            //    var table = $('#divpopupTrail table').DataTable();

            //    table.destroy();

            //    $("#divpopupTrail table ").DataTable({
            //        "order": [[0, "asc"]],
            //        //"paging": false,
            //        "aaData": data,
            //        "aoColumns": [
            //     { "mData": "CompItem" },
            //   { "mData": "CompQty" },
            //     { "mData": "ComRate" },
            //    { "mData": "Value" }
            //        ]
            //    });
            //}

      }

      var getParams = function (url) {
          var params = {};
          var parser = document.createElement('a');
          parser.href = url;
          var query = parser.search.substring(1);
          var vars = query.split('&');
          for (var i = 0; i < vars.length; i++) {
              var pair = vars[i].split('=');
              params[pair[0]] = decodeURIComponent(pair[1]);
          }
          return params;
      };
</script>
    
    <style>
        .btncss {
  background: none!important;
  border: none;
  padding: 0!important;
 
  font-family: arial, sans-serif;
  color: #069;
  text-decoration: underline;
  cursor: pointer;
}
    </style>
    <section class="content">
        <div class="row">          
            <div class="col-md-12">
                <div id="InputWork">                    
                    <div class="box box-primary">
                        <div class="box-header with-border">
                            <div style="text-align: center;display:none;">
                                <h3 class="box-title" style="color: #ff8000;">ASTRAL POLYTECHNIK LTD</h3>
                                <br />
                                <h4 style="color: #ff8000;">Ahmedabad</h4>
                                <br />
                                <h4 style="color: #ff8000;">Daily Working Summary L1</h4>
                            </div>
                            <%--<div class="col-md-3" style="text-align: left;">
                                <label>Sales Person:</label>&nbsp;&nbsp;<asp:Label ID="saleRepName" runat="server" Text="Label"></asp:Label>
                                <asp:HiddenField ID="dateHiddenField" runat="server" />
                                <asp:HiddenField ID="smIDHiddenField" runat="server" />   
                                <asp:HiddenField ID="beatIDHiddenField" runat="server" />
                                <asp:HiddenField ID="statusHiddenField" runat="server" />
                            </div>--%>
                            <div class="col-md-1 col-sm-1 col-xs-12" >      
                                       <asp:ImageButton ID="ImageMasterPage" runat="server" AlternateText="No Image"
                                                    Height="50px" Width="50px" class="img-circle" Style="cursor: pointer" OnClientClick="return LoadDiv(this.src);" />                                              
                            </div>
                            <div class="col-md-3 col-sm-3 col-xs-12" >
                                <label>Sales Person:</label>&nbsp;&nbsp;<asp:Label ID="saleRepName" runat="server" Text="Label"></asp:Label>
                                <asp:HiddenField ID="dateHiddenField" runat="server" />
                                <asp:HiddenField ID="smIDHiddenField" runat="server" />   
                                <asp:HiddenField ID="beatIDHiddenField" runat="server" />
                                <asp:HiddenField ID="statusHiddenField" runat="server" />
                            </div>
                            
                            <div class="col-md-3 col-sm-3 col-xs-12" >
                                <label>Report Date :</label>&nbsp;&nbsp;&nbsp;<asp:Label ID="currDateLabel" runat="server" Text="Label"></asp:Label>
                                <br />
                            </div>                           

                               <div class="col-md-5 col-sm-5 col-xs-12" >
                                    <label>Export View Type:</label>
                                   <asp:DropDownList ID="items" runat="server" CssClass="form-control" style="float: right;
    width: 75%;" >
                                          <asp:ListItem Selected="True" Value="1"> With Details </asp:ListItem>
                                <asp:ListItem Value="2"> WithOut Details </asp:ListItem>
                                   </asp:DropDownList>
                            
                                <br />
                            </div>
                             <%-- <div class="col-md-3">
                             <label>Selected City:</label>&nbsp;&nbsp;<asp:Label ID="lblcity" runat="server" Text="Label"></asp:Label> <br />
                            </div>--%>
                        </div>                       
                        <div class="box-body">                           
                          <b> Date:&nbsp; <asp:Label ID="dateLabel" runat="server" Text="Label" ForeColor="#6699ff"></asp:Label></b>&nbsp;&nbsp; | &nbsp;&nbsp;
                             <label>Selected City:</label>&nbsp;&nbsp;<asp:Label ID="lblcity" runat="server" Text="Label"></asp:Label> <br />
                            <div class="box-body table-responsive">
                                <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                                    <HeaderTemplate>
                                        <table id="example1" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>
                                                    <th>Time</th>
                                                   <%-- <th>Time</th>--%>
                                                    <th>Area - Beat</th>
                                                    <th>Party</th>
                                                    <th>Activity</th>                                   
                                                     <th>Latitude</th>
                                                      <th>Longitude</th>
                                                     <th>Address</th>
                                                    <th>View Image</th>
                                                   
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                   <%-- <th colspan="12" style="text-align: right">Total:</th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>
                                                    <th style="text-align: right"></th>--%>
                                                </tr>
                                            </tfoot>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <asp:HiddenField ID="linkHiddenField" runat="server" Value='<%#Eval("Image") %>' />
                                            <asp:HiddenField ID="sTypeHdf" runat="server" Value='<%#Eval("Stype") %>' />
                                            <asp:HiddenField ID="hfCOMPTID" runat="server" Value='<%#Eval("COMPTID") %>' />
                                            <asp:HiddenField ID="hfParty" runat="server" Value='<%#Eval("PartyId") %>' />  
                                            <asp:HiddenField ID="hfVisitDate" runat="server" Value='<%#Eval("VisitDate") %>' />
                                            <asp:HiddenField ID="hfPartyName" runat="server" Value='<%#Eval("Party") %>' />
                                            <asp:HiddenField ID="hfOrderTakenType" runat="server" Value='<%#Eval("OrderTakenType") %>' />
                                            <asp:HiddenField ID="hfExpectedDD" runat="server" Value='<%#Eval("ExpectedDD") %>' />                                             
                                          
                                            
                                         <%--   <td style="display:none;"><asp:Label ID="lblSType" runat="server" Text='<%# Eval("Stype")%>'></asp:Label></td>
                                             <td style="display:none;"><asp:Label ID="lblPartyName" runat="server" Text='<%# Eval("Party")%>'></asp:Label></td>
                                             <td style="display:none;"><asp:Label ID="lblCOMPTID" runat="server" Text='<%# Eval("COMPTID")%>'></asp:Label></td>
                                             <td style="display:none;"><asp:Label ID="lblPartyID" runat="server" Text='<%# Eval("PartyId")%>'></asp:Label></td>
                                             <td style="display:none;"><asp:Label ID="lblVisitDate" runat="server" Text='<%# Eval("VisitDate")%>'></asp:Label></td>--%>                                           
                                           <%--  <td><input type="button" class="btncss" runat="server" value="Details" onclick=" GetOrder(this);" /></td>--%>
                                              <td><input type="button" class="btncss" runat="server" value=<%#Eval("Mobile_Created_date")==DBNull.Value ? "" :System.Convert.ToDateTime(Eval("Mobile_Created_date")).ToString("HH:mm:ss")%> onclick=" GetOrder(this);" /></td>
                                         
                                         <%--   <td><%#Eval("Mobile_Created_date")==DBNull.Value ? "" :System.Convert.ToDateTime(Eval("Mobile_Created_date")).ToString("HH:mm:ss")%></td>--%>
                                            <td><%# Eval("Beat")%></td>
                                            
                                            <td><%# Eval("Party")%></td>
                                          
                                            <td><%# Eval("Stype")%></td>        
                                                                            
                                             <td><%# Eval("Latitude")%></td>
                                              <td><%# Eval("Longitude")%></td>
                                             <td><%# Eval("Address")%></td>
                                             <td>
                                             
                                                 <asp:LinkButton ID="lnkViewDemoImg" runat="server" OnClick="lnkViewDemoImg_Click" Visible="false">View Image</asp:LinkButton>


                                                 <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl='<%# Eval("Image")%>' AlternateText="No Image"
                                                    Width="25px" Height="25px" Style="cursor: pointer" OnClientClick="return LoadDiv(this.src);" />
                                             </td>
                                         <%--   <td><asp:LinkButton CommandName="select" ID="lnkEdit"
                                            CausesValidation="False" runat="server" Text='Details' ToolTip="Details"
                                            Width="80px" Font-Underline="True" OnClientClick="window.document.forms[0].target='_self'; setTimeout(function(){window.document.forms[0].target='';}, 500);"/></td>--%>
                                               
                                          
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>     </table>       
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>

                            <div style="float: left; margin-top: 5px;">
                               <%-- <input type="button" value="Export to Excel" id='excelExport' />--%>
                                 <asp:Button Style="margin-right: 5px;" type="button" ID="btnExport" runat="server" Text="Export" ToolTip="Export To Excel" class="btn btn-primary" OnClick="btnExport_Click" />
                                <asp:Button type="button" ID="btnBack" runat="server" Height="35px" Text="Back" class="btn btn-primary"
                                    OnClick="btnBack_Click" Visible="false" />
                            </div>

                        </div>

                    </div>
                </div>
            </div>
            <div id="divBackground" class="modal">
            <div id="divImage" >
                <img id="imgLoader" alt="" src="img/close.png" />
                
<table style="height: 30%; width: 30%;align-content:center">
     <tr>
        <td align="center" valign="bottom">
         <img id="deletemodal" alt="" src="img/cross.jpg"  style="margin-left:100%;width:15px;height:15px"  onclick="HideDiv()" />
        </td>
    </tr>
    <tr>
        <td valign="middle" align="center">
            <img id="imgFull" alt="" src="" style="display: none; height: 300px; width: 300px" />
        </td>
    </tr>
   
</table>
</div>
</div>
        </div>
           

        <%-- <asp:Button ID="btntrailpopup" runat="server" Style="display: none" />
        <asp:HiddenField ID="HiddenField2" runat="server" />
        <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btntrailpopup" PopupControlID="panelpopuptrail" BehaviorID="popuptrail" BackgroundCssClass="modalBackground">
        </cc1:ModalPopupExtender>


        <asp:Panel runat="server" ID="panelpopuptrail" Style="width: 55%; height: 70%; border-radius: 4px; overflow: auto; display:none;" BackColor="White">
            <div>
                <img src="img/cross.jpg" onclick="CloseTrailPopup()" style="height: 16px; width: 16px" />
            </div>
            <div id="divpopupTrail" >

                <div class="form-group" style="position: relative">

                    <div class="box-body table-responsive">
                        <asp:Repeater ID="Repeater2" runat="server">
                            <HeaderTemplate>
                                <table id="exampleOrder" class="tablen table-bordered table-striped" width="100%">
                                    <thead>
                                        <tr style="height: 32px">
                                            <th>CompItem</th>     
                                            <th>CompQty</th>
                                            <th>ComRate</th>
                                            <th>Value</th>
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
                    <div class="box-footer">

                        <%--                        <div class="col-lg-4 col-md-8 col-xs-4">

                            <input type="button" value="Cancel" onclick="CloseTrailPopup();" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>--%>

 <button type="button" class="btn btn-primary" data-toggle="modal" data-backdrop="static" data-keyboard="false" data-target="#myModal" style="display:none;" id="btnmodal">
    Open modal
  </button>
                      

                            <div class="modal fade" id="myModal" >
                                <div class="modal-dialog modal-lg" style="width:82%">
                                    <div class="modal-content" style="overflow-x:auto;">                                   
                                        <div class="modal-header">
                                            <h4 class="modal-title" id="DivHeaderTitle"></h4>
                                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                                             <div class="col-md-12">
                                              <div class="col-md-4">
                                            <label for="exampleInputEmail1">Sales Person:</label>
                                            <asp:Label ID="lblSalesPerson" runat="server" Text=""></asp:Label>
                                            </div>                                        
                                            <div class="col-md-5">
                                            <label for="exampleInputEmail1">Retailer/Distributor:</label>
                                           <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                            </div>
                                             <div class="col-md-1" id="divordertaken" style="visibility:hidden">
                                            <label for="exampleInputEmail1">Order Taken Type:</label>   
                                                   <asp:Label ID="lblOrderTakenType" runat="server" Text=""></asp:Label>
                                                <i id="faicon" style="visibility:hidden" class="fa fa-phone" aria-hidden="true"></i>
                                            </div>
                                            <div class="col-md-2">
                                            <label for="exampleInputEmail1">Visit Date:</label>
                                            <asp:Label ID="lblVisitDate" runat="server" Text=""></asp:Label>                                          
                                            </div>
                                                 </div>
                                             <div class="col-md-12">
                                             <div class="col-md-4" id="divestmdate" style="visibility:hidden">
                                            <label for="exampleInputEmail1">Estimated Delivery Date:</label>
                                            <asp:Label ID="lblExpectedDD" runat="server" Text=""></asp:Label>                                          
                                            </div>
                                                  
                                                 </div>

                                        </div>
                                          <%--<div><label>Sales Person:</label>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblSalesPerson" runat="server" Text=""></asp:Label> <label>Visit Date:</label>&nbsp;&nbsp;<asp:Label ID="lblVisitDate" runat="server" Text=""></asp:Label></div>--%>
                                           
                                        <div class="modal-body" id="DivModalBody">    </div>

                                        <div class="modal-footer" style="text-align:left;">
                                           <%-- <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                                             <label for="exampleInputEmail1">Remark:</label>
                                            <label id="lblremark" style="FONT-WEIGHT: 400;FONT-SIZE: medium;" ></label>      
                                        </div>
                                    </div>
                                </div>
                                </div>

    </section>
    <style type="text/css">
body
{
    margin: 0;
    padding: 0;
    height: 100%;
}
/*.modal
{
    display: none;
    margin-bottom:110px;
    position: absolute;
    top: 0px;
    left: 0px;
    
    z-index: 100;
    opacity: 1;
    filter: alpha(opacity=60);
    -moz-opacity:.8;
    min-height: 100%;
}*/
#divImage
{
    /*display: none;
    z-index: 1000;
    position: fixed;
    align-content:center;
    top: 0;
    left: 0;
    background-color: White;
    height: 300px;
    width: 300px;
    padding: 3px;
    border: solid 1px black;*/
        display: none;
    z-index: 1000;
    position: fixed;
    /* align-content: center; */
    top: 16% !important;
    left: 38% !important;
    /*background-color: White;*/
    height: 300px;
    width: 300px;
    /*padding: 3px;*/
    /*border: solid 1px black;*/
       opacity: 1;
}
</style>
    <script type="text/javascript">
        function LoadDiv(url) {
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
        function HideDiv() {
            var bcgDiv = document.getElementById("divBackground");
            var imgDiv = document.getElementById("divImage");
            var imgFull = document.getElementById("imgFull");
            if (bcgDiv != null) {
                bcgDiv.style.display = "none";
                imgDiv.style.display = "none";
                imgFull.style.display = "none";
            }
        }
</script>
      <script src="plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
     <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>

      

     <script src="jqwidgets/MYScript.js"></script>
    <script src="jqwidgets/jquery.table2excel.js"></script>
    <%-- <script src="plugins/datatables/jquery.dataTables.min.js"></script>--%>
    <%--<script src="plugins/jquery.tableTotal.js"></script>--%>
   <%-- <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>--%>
     
     <script type="text/javascript">
      
       

         //$(function () {
         //    $('#exampleDistStock').dataTable({
         //        "order": [[0, "asc"]],
         //        "footerCallback": function (tfoot, data, start, end, display) {
         //            //$(tfoot).find('th').eq(0).html("Starting index is " + start);
         //            var api = this.api();
         //            var intVal = function (i) {
         //                return typeof i === 'string' ?
         //                    i.replace(/[\$,]/g, '') * 1 :
         //                    typeof i === 'number' ?
         //                    i : 0;
         //            };
         //            debugger;
         //            var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Qty'; }).first().index();
         //            var totalData = api.column(1).data();
         //            var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
         //            var pageTotalData = api.column(1, { page: 'current' }).data();
         //            var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
         //            var searchTotalData = api.column(1, { 'filter': 'applied' }).data();
         //            var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

         //            $(api.column(1).footer()).html(searchTotal);

         //        }
         //    });
         //});

         //$(function () {
         //    $('#examplePartyCollection').dataTable({
         //        "order": [[0, "asc"]],
         //        "footerCallback": function (tfoot, data, start, end, display) {
         //            //$(tfoot).find('th').eq(0).html("Starting index is " + start);
         //            var api = this.api();
         //            var intVal = function (i) {
         //                return typeof i === 'string' ?
         //                    i.replace(/[\$,]/g, '') * 1 :
         //                    typeof i === 'number' ?
         //                    i : 0;
         //            };
         //            debugger;
         //            var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Qty'; }).first().index();
         //            var totalData = api.column(1).data();
         //            var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
         //            var pageTotalData = api.column(1, { page: 'current' }).data();
         //            var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
         //            var searchTotalData = api.column(1, { 'filter': 'applied' }).data();
         //            var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

         //            $(api.column(1).footer()).html(searchTotal);

         //        }
         //    });
         //});
         //$(function () {
         //    $('#exampleDistributorCollection').dataTable({
         //        "order": [[0, "asc"]],
         //        "footerCallback": function (tfoot, data, start, end, display) {
         //            //$(tfoot).find('th').eq(0).html("Starting index is " + start);
         //            var api = this.api();
         //            var intVal = function (i) {
         //                return typeof i === 'string' ?
         //                    i.replace(/[\$,]/g, '') * 1 :
         //                    typeof i === 'number' ?
         //                    i : 0;
         //            };
         //            debugger;
         //            var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Qty'; }).first().index();
         //            var totalData = api.column(1).data();
         //            var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
         //            var pageTotalData = api.column(1, { page: 'current' }).data();
         //            var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
         //            var searchTotalData = api.column(1, { 'filter': 'applied' }).data();
         //            var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

         //            $(api.column(1).footer()).html(searchTotal);

         //        }
         //    });
         //});

         //$(function () {
         //    $('#exampleOrder').dataTable({
         //        "order": [[0, "asc"]],
         //        "footerCallback": function (tfoot, data, start, end, display) {
         //            //$(tfoot).find('th').eq(0).html("Starting index is " + start);
         //            var api = this.api();
         //            var intVal = function (i) {
         //                return typeof i === 'string' ?
         //                    i.replace(/[\$,]/g, '') * 1 :
         //                    typeof i === 'number' ?
         //                    i : 0;
         //            };
         //            debugger;
         //            var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Qty'; }).first().index();
         //            var totalData = api.column(1).data();
         //            var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
         //            var pageTotalData = api.column(1, { page: 'current' }).data();
         //            var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
         //            var searchTotalData = api.column(1, { 'filter': 'applied' }).data();
         //            var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

         //            $(api.column(1).footer()).html(searchTotal);
         //            var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Qty'; }).first().index();
         //            var totalData = api.column(3).data();
         //            var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
         //            var pageTotalData = api.column(3, { page: 'current' }).data();
         //            var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
         //            var searchTotalData = api.column(3, { 'filter': 'applied' }).data();
         //            var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

         //            $(api.column(3).footer()).html(searchTotal);

         //        }
         //    });
         //});
         //$(function () {
         //    $('#exampleCompetitor').dataTable({
         //        "order": [[0, "asc"]],
         //        "footerCallback": function (tfoot, data, start, end, display) {
         //            //$(tfoot).find('th').eq(0).html("Starting index is " + start);
         //            var api = this.api();
         //            var intVal = function (i) {
         //                return typeof i === 'string' ?
         //                    i.replace(/[\$,]/g, '') * 1 :
         //                    typeof i === 'number' ?
         //                    i : 0;
         //            };
         //            debugger;
         //            var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Qty'; }).first().index();
         //            var totalData = api.column(1).data();
         //            var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
         //            var pageTotalData = api.column(1, { page: 'current' }).data();
         //            var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
         //            var searchTotalData = api.column(1, { 'filter': 'applied' }).data();
         //            var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

         //            $(api.column(1).footer()).html(searchTotal);
         //            var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Qty'; }).first().index();
         //            var totalData = api.column(3).data();
         //            var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
         //            var pageTotalData = api.column(3, { page: 'current' }).data();
         //            var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
         //            var searchTotalData = api.column(3, { 'filter': 'applied' }).data();
         //            var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

         //            $(api.column(3).footer()).html(searchTotal);

         //        }
         //    });
         //});
     //$(function () {
     //    $('#example1').dataTable({
     //        "order": [[0, "asc"]],
     //             "footerCallback": function (tfoot, data, start, end, display) {
     //                 //$(tfoot).find('th').eq(0).html("Starting index is " + start);
     //                 var api = this.api();
     //                 var intVal = function (i) {
     //                     return typeof i === 'string' ?
     //                         i.replace(/[\$,]/g, '') * 1 :
     //                         typeof i === 'number' ?
     //                         i : 0;
     //                 };
     //                 debugger;
     //                 var costColumnIndex = $('th').filter(function (i) { return $(this).text() == 'Amount'; }).first().index();
     //                 var totalData = api.column(costColumnIndex).data();
     //                 var total = totalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
     //                 var pageTotalData = api.column(costColumnIndex, { page: 'current' }).data();
     //                 var pageTotal = pageTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
     //                 var searchTotalData = api.column(costColumnIndex, { 'filter': 'applied' }).data();
     //                 var searchTotal = searchTotalData.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);                   


     //                 var costColumnIndex1 = $('th').filter(function (i) { return $(this).text() == 'Qty'; }).first().index();
     //                 var totalData1 = api.column(costColumnIndex1).data();
     //                 var total1 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
     //                 var pageTotalData1 = api.column(costColumnIndex1, { page: 'current' }).data();
     //                 var pageTotal1 = pageTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
     //                 var searchTotalData1 = api.column(costColumnIndex1, { 'filter': 'applied' }).data();
     //                 var searchTotal1 = searchTotalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

     //                 //var costColumnIndex2 = $('th').filter(function (i) { return $(this).text() == 'Rate'; }).first().index();
     //                 //var totalData2 = api.column(costColumnIndex2).data();
     //                 //var total2 = totalData1.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
     //                 //var pageTotalData2 = api.column(costColumnIndex2, { page: 'current' }).data();
     //                 //var pageTotal2 = pageTotalData2.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
     //                 //var searchTotalData2 = api.column(costColumnIndex2, { 'filter': 'applied' }).data();
     //                 //var searchTotal2 = searchTotalData2.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

     //                 var costColumnIndex3 = $('th').filter(function (i) { return $(this).text() == 'Stock'; }).first().index();
     //                 var totalData3 = api.column(costColumnIndex3).data();
     //                 var total3 = totalData3.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
     //                 var pageTotalData3 = api.column(costColumnIndex3, { page: 'current' }).data();
     //                 var pageTotal3 = pageTotalData3.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);
     //                 var searchTotalData3 = api.column(costColumnIndex3, { 'filter': 'applied' }).data();
     //                 var searchTotal3 = searchTotalData3.reduce(function (a, b) { return intVal(a) + intVal(b); }, 0).toFixed(2);

     //                 $(api.column(15).footer()).html(searchTotal);
     //                 $(api.column(13).footer()).html(searchTotal1);
     //                 //$(api.column(9).footer()).html(searchTotal2);
     //                 $(api.column(12).footer()).html(searchTotal3);
     //                 if (searchTotal == 'NaN' || searchTotal == '') { $(api.column(14).footer()).html('0.0') }
     //                 if (searchTotal1 == 'NaN' || searchTotal1 == '') { $(api.column(12).footer()).html('0.0') }
     //                 if (searchTotal3 == 'NaN' || searchTotal3 == '') { $(api.column(11).footer()).html('0.0') }
     //                 //if (searchTotal2 == 'NaN' || searchTotal2 == '') { $(api.column(9).footer()).html('0.0') }
     //             }
     //         });
     //});

     //function CloseTrailPopup() {
     //    $find("popuptrail").hide();
     //}
    
    </script>
       <script type="text/javascript">
           $(function () {
               $("#example1").DataTable({

               });
           });

    </script>
      <script type="text/javascript">
    $(document).ready(function() {
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
        alert("dsd");
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
    } );
          </script>
</asp:Content>
