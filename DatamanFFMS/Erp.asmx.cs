using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services.Protocols;
using BusinessLayer;
using System.Xml;
using System.Data;
using System.IO;
using DAL;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Collections;
using BAL;
using System.Globalization;
using System.IO.Compression;
using BAL.AdvanceReq;
using AstralFFMS.ServiceReferenceDMTracker;
using System.Net.Mime;
using System.Drawing;
using System.Diagnostics;
using System.Dynamic;
using AstralFFMS;

namespace AstralFFMS
{
    /// <summary>
    /// Summary description for Erp
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Erp : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        public class mszList
        {
            [DataMember]
            public string msz { get; set; }

        }        

        public class PendingOrderList
        {
            public string po_docid { get; set; }
            public string po_date { get; set; }
            public string po_distributor_syncid { get; set; }
            public string po_item_syncid { get; set; }
            public string po_qty { get; set; }
            public string po_item_remarks { get; set; }
            public string po_pendingqty { get; set; }
            public string po_shippingqty { get; set; }
            public string po_amount { get; set; }

        }

        public class Lcation_D
        {
            public string DeviceNo { get; set; }           

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void JInsertPendingOrder(string POrders)
        {
            string Save = "N";         
            List<mszList> rst = new List<mszList>();
            try
            {  
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<PendingOrderList>>(POrders);
                    for (int i = 0; i < objResponse.Count; i++)
                    {
                        string Insertpo = "";
                        int distid = DbConnectionDAL.GetIntScalarVal("Select partyid from mastparty where syncid = '" + objResponse[i].po_distributor_syncid.ToString() + "'");
                        int itemid = DbConnectionDAL.GetIntScalarVal("Select itemid from mastitem where syncid = '" + objResponse[i].po_item_syncid.ToString() + "'");
                        if ((distid.ToString() != "0") || (itemid.ToString() != "0"))
                        {
                            Insertpo = "INSERT INTO dbo.PurchaseOrderImport (PODocId,VDate,DistID,ItemID,Qty,ItemRemarks,PendingQty,ShippingQty,Amount) " +
                            " VALUES ('" + objResponse[i].po_docid.ToString() + "',  '" + objResponse[i].po_date.ToString() + "', " + distid + ", " + itemid + " , '" + objResponse[i].po_qty.ToString() + "', '" + objResponse[i].po_item_remarks.ToString() + "', " + objResponse[i].po_pendingqty.ToString() + ", '" + objResponse[i].po_shippingqty.ToString() + "', " + objResponse[i].po_amount.ToString() + ")";
                        }
                        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, Insertpo);
                    }
                    Save = "Y";
                }
                rst.Add(
                        new mszList
                        {
                            msz = Save
                        }
                    );

                Context.Response.Write(JsonConvert.SerializeObject(rst));
            }
            catch (Exception ex)
            {
                Context.Response.Write(JsonConvert.SerializeObject(rst));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void PendingOrderTransfer(string Location_D)
        {
            if (Location_D != "")
            {
                using (WebClient client = new WebClient())
                {
                    var objResponse = JsonConvert.DeserializeObject<List<Lcation_D>>(Location_D);
                }
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertDistributorLedger(string DLDocId, string VDate, string distributorSyncId, decimal amount, decimal AmtCr, decimal AmtDr, string Narration)
        {
            string message = "Data Not Save";
            List<mszList> rst = new List<mszList>();
            try
            {
                int retVal = 0;               
                int distid = DbConnectionDAL.GetIntScalarVal("Select partyid from mastparty where syncid = '" + distributorSyncId + "'");


                string Insertpo = "INSERT INTO dbo.TransDistributerLedger (DLDocId,VDate,DistID,Amount,AmtCr,AmtDr,Narration) " +
                " VALUES ('" + DLDocId + "',  '" + VDate + "', " + distid + ", " + amount + " , " + AmtCr + ", " + AmtDr + ", '" + Narration + "')";

                retVal = DbConnectionDAL.GetIntScalarVal(Insertpo);
                if (retVal == 0)
                    message = "Data Save";

                rst.Add(
                        new mszList
                        {
                            msz = message
                        }
                    );

                Context.Response.Write(JsonConvert.SerializeObject(rst));

            }
            catch (Exception ex)
            {
                Context.Response.Write(JsonConvert.SerializeObject(rst));
            }
        }

        public class DistributorInvoiceHeader
        {
            [DataMember]
            public string Id { get; set; }
            [DataMember]
            public string msz { get; set; }         
           
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertDistributorInvoiceHeader(string DocId, string VDate, string distributorSyncId, decimal taxamount, decimal BillAmt)
        {
            string message = "Data Not Save";
            List<DistributorInvoiceHeader> rst = new List<DistributorInvoiceHeader>();

            try
            {
                int retVal = 0;
                int invid = 0;
                int distid = DbConnectionDAL.GetIntScalarVal("Select partyid from mastparty where syncid = '" + distributorSyncId + "' and partydist=1");


                string Insertpo = "INSERT INTO dbo.TransDistInv (DistInvDocId,VDate,DistID,taxamt1,BillAmount) " +
                " VALUES ('" + DocId + "',  '" + VDate + "', " + distid + ", " + taxamount + " , " + BillAmt + ")";               
                retVal = DbConnectionDAL.GetIntScalarVal(Insertpo);
                if (retVal == 0)               
                    message = "Data Save";               
                invid = DbConnectionDAL.GetIntScalarVal("SELECT max(Distinvid) AS invid FROM TransDistInv");          
               
                    rst.Add(
                        new DistributorInvoiceHeader
                        {
                            Id = invid.ToString(),
                            msz = message                           
                        }
                    );
               
                Context.Response.Write(JsonConvert.SerializeObject(rst));
            }
            catch (Exception ex)
            {
                Context.Response.Write(JsonConvert.SerializeObject(rst));
            }
        }       

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertDistributorInvoiceLine(int headerinvid, string DocId, int SNo, string VDate, string distributorSyncId, string itemSyncid, decimal qty, decimal rate, decimal amount, decimal taxtamt, decimal netTotalAmt, decimal QtyInKg)
        {
            string message = "Data Not Save";
            List<mszList> rst = new List<mszList>();

            try
            {
                int retVal = 0;

                int distid = DbConnectionDAL.GetIntScalarVal("Select partyid from mastparty where syncid = '" + distributorSyncId + "' and partydist=1");
                int itemid = DbConnectionDAL.GetIntScalarVal("Select itemid from mastitem where syncid = '" + itemSyncid + "'");

                string Insertpo = "INSERT INTO dbo.TransDistInv1 (DistInvId,DistInvDocId,Sno,VDate,DistId,ItemId,Qty,Rate,Amount,Tax_Amt,Net_Total,QtyInKg) " +
                " VALUES (" + headerinvid + ",'" + DocId + "', " + SNo + ", '" + VDate + "', " + distid + "," + itemid + ", " + qty + " , " + rate + "," + amount + "," + taxtamt + "," + netTotalAmt + "," + QtyInKg + ")";

                retVal = DbConnectionDAL.GetIntScalarVal(Insertpo);
                if (retVal == 0)                
                    message = "Data Save";                           

                rst.Add(
                       new mszList
                       {
                           msz = message
                       }
                   );
                Context.Response.Write(JsonConvert.SerializeObject(rst));
            }
            catch (Exception ex)
            {
                Context.Response.Write(JsonConvert.SerializeObject(rst));
            }
        }
    }
}
