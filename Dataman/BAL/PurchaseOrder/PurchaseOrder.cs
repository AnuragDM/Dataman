using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;

namespace BAL.PurchaseOrder
{
    public class PurchaseOrder
    {
        public int InsertTP(string Transporter)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@TP", DbParameter.DbType.VarChar, 100, Transporter);          
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 4, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_InsertTransporter", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }

        public int Insert(DateTime Currdt, string docID, string UserID, string SMID, string CustID, 
            int TransPorterID, string DispName, string DispAdd1, string DispAdd2, string Country, string State,
            string City, string Pin, string Phone, string Mobile, string Email, DataTable dtItem, string Remarks, 
            string PType, int ProjectID, decimal decAmtTotal, int CityID,string TransPorter,int SchemeID,string IpAddr)
        {            
            DbParameter[] dbParam = new DbParameter[28];
            dbParam[0] = new DbParameter("@podocid", DbParameter.DbType.VarChar, 30, docID);
            dbParam[1] = new DbParameter("@vdate", DbParameter.DbType.DateTime, 50, Currdt);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, Convert.ToInt32(UserID));
            dbParam[3] = new DbParameter("@smid", DbParameter.DbType.Int, 4, Convert.ToInt32(SMID));
            dbParam[4] = new DbParameter("@distid", DbParameter.DbType.Int, 4, Convert.ToInt32(CustID));
            dbParam[5] = new DbParameter("@remarks", DbParameter.DbType.VarChar, 4000, Remarks);
            dbParam[6] = new DbParameter("@transporter", DbParameter.DbType.VarChar, 100, TransPorter);
            dbParam[7] = new DbParameter("@dispname", DbParameter.DbType.VarChar, 100, DispName);
            dbParam[8] = new DbParameter("@dispadd1", DbParameter.DbType.VarChar, 100, DispAdd1);
            dbParam[9] = new DbParameter("@dispadd2", DbParameter.DbType.VarChar, 100, DispAdd2);
            dbParam[10] = new DbParameter("@dispcity", DbParameter.DbType.VarChar, 50, City);
            dbParam[11] = new DbParameter("@disppin", DbParameter.DbType.VarChar, 6, Pin);
            dbParam[12] = new DbParameter("@dispstate", DbParameter.DbType.VarChar, 25, State);
            dbParam[13] = new DbParameter("@dispcountry", DbParameter.DbType.VarChar, 25, Country);
            dbParam[14] = new DbParameter("@dispphone", DbParameter.DbType.VarChar, 25, Phone);
            dbParam[15] = new DbParameter("@dispmobile", DbParameter.DbType.VarChar, 25, Mobile);
            dbParam[16] = new DbParameter("@dispemail", DbParameter.DbType.VarChar, 100, Email);
            dbParam[17] = new DbParameter("@orderstatus", DbParameter.DbType.VarChar, 1, "P");
            dbParam[18] = new DbParameter("@createddate", DbParameter.DbType.DateTime, 50, Currdt);
            dbParam[19] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[20] = new DbParameter("@POrdId", DbParameter.DbType.Int, 4, 0);
            dbParam[21] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[22] = new DbParameter("@projecttype", DbParameter.DbType.VarChar, 1, PType);
            dbParam[23] = new DbParameter("@projectid", DbParameter.DbType.Int, 4, ProjectID);
            dbParam[24] = new DbParameter("@city", DbParameter.DbType.Int, 4, CityID);
            dbParam[25] = new DbParameter("@ordervalue", DbParameter.DbType.Decimal, 18, decAmtTotal);
            dbParam[26] = new DbParameter("@SchemeID", DbParameter.DbType.Int, 4, SchemeID);
            dbParam[27] = new DbParameter("@IpAddr", DbParameter.DbType.VarChar, 50, IpAddr);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_InsertPOData", dbParam);
           

            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                DbParameter[] dbParam1 = new DbParameter[12];
                dbParam1[0] = new DbParameter("@pordid", DbParameter.DbType.Int, 4, dbParam[21].Value);
                dbParam1[1] = new DbParameter("@podocid", DbParameter.DbType.VarChar, 30, docID);
                dbParam1[2] = new DbParameter("@sno", DbParameter.DbType.Int, 4, (i + 1));
                dbParam1[3] = new DbParameter("@vdate", DbParameter.DbType.DateTime, 50, Currdt);
                dbParam1[4] = new DbParameter("@distid", DbParameter.DbType.Int, 4, Convert.ToInt32(CustID));
                dbParam1[5] = new DbParameter("@userid", DbParameter.DbType.Int, 4, Convert.ToInt32(UserID));
                dbParam1[6] = new DbParameter("@itemid", DbParameter.DbType.Int, 4, Convert.ToInt32(dtItem.Rows[i]["ItemID"]));
                dbParam1[7] = new DbParameter("@qty", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(dtItem.Rows[i]["Qty"]));
                dbParam1[8] = new DbParameter("@disc", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(0));
                dbParam1[9] = new DbParameter("@remarks", DbParameter.DbType.VarChar, 255, Convert.ToString(dtItem.Rows[i]["Remarks"]));
                dbParam1[10] = new DbParameter("@rate", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(dtItem.Rows[i]["Rate"]));
                dbParam1[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
                DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_InsertPurchaseorder", dbParam1);

            }
            return Convert.ToInt32(dbParam[21].Value);
        }

        public int Update(int POID,DateTime Currdt, string docID, string UserID, string SMID, string CustID,
            int TransPorterID, string DispName, string DispAdd1, string DispAdd2, string Country,
            string State, string City, string Pin, string Phone, string Mobile, string Email, DataTable dtItem, string Remarks, string PType, int ProjectID, decimal decAmtTotal, int CityID, string TransPorter, int SchemeID, string IpAddr)
        {

            DbParameter[] dbParam = new DbParameter[28];
            dbParam[0] = new DbParameter("@podocid", DbParameter.DbType.VarChar, 30, docID);
            dbParam[1] = new DbParameter("@vdate", DbParameter.DbType.DateTime, 50, Currdt);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, Convert.ToInt32(UserID));
            dbParam[3] = new DbParameter("@smid", DbParameter.DbType.Int, 4, Convert.ToInt32(SMID));
            dbParam[4] = new DbParameter("@distid", DbParameter.DbType.Int, 4, Convert.ToInt32(CustID));
            dbParam[5] = new DbParameter("@remarks", DbParameter.DbType.VarChar, 4000, Remarks);
            dbParam[6] = new DbParameter("@transporter", DbParameter.DbType.VarChar, 100, TransPorter);
            dbParam[7] = new DbParameter("@dispname", DbParameter.DbType.VarChar, 100, DispName);
            dbParam[8] = new DbParameter("@dispadd1", DbParameter.DbType.VarChar, 100, DispAdd1);
            dbParam[9] = new DbParameter("@dispadd2", DbParameter.DbType.VarChar, 100, DispAdd2);
            dbParam[10] = new DbParameter("@dispcity", DbParameter.DbType.VarChar, 50, City);
            dbParam[11] = new DbParameter("@disppin", DbParameter.DbType.VarChar, 6, Pin);
            dbParam[12] = new DbParameter("@dispstate", DbParameter.DbType.VarChar, 25, State);
            dbParam[13] = new DbParameter("@dispcountry", DbParameter.DbType.VarChar, 25, Country);
            dbParam[14] = new DbParameter("@dispphone", DbParameter.DbType.VarChar, 25, Phone);
            dbParam[15] = new DbParameter("@dispmobile", DbParameter.DbType.VarChar, 25, Mobile);
            dbParam[16] = new DbParameter("@dispemail", DbParameter.DbType.VarChar, 100, Email);
            dbParam[17] = new DbParameter("@orderstatus", DbParameter.DbType.VarChar, 1, "P");
            dbParam[18] = new DbParameter("@createddate", DbParameter.DbType.DateTime, 50, Currdt);
            dbParam[19] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "");
            dbParam[20] = new DbParameter("@POrdId", DbParameter.DbType.Int, 4, POID);
            dbParam[21] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[22] = new DbParameter("@projecttype", DbParameter.DbType.VarChar, 1, PType);
            dbParam[23] = new DbParameter("@projectid", DbParameter.DbType.Int, 4, ProjectID);
            dbParam[24] = new DbParameter("@city", DbParameter.DbType.Int, 4, CityID);
            dbParam[25] = new DbParameter("@ordervalue", DbParameter.DbType.Decimal, 18, decAmtTotal);
            dbParam[26] = new DbParameter("@SchemeID", DbParameter.DbType.Int, 4, SchemeID);
            dbParam[27] = new DbParameter("@IpAddr", DbParameter.DbType.VarChar, 50, IpAddr);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_InsertPOData", dbParam);

            DbParameter[] dbParamDel = new DbParameter[1];
            dbParamDel[0] = new DbParameter("@POrdId", DbParameter.DbType.Int, 4, POID);
            //dbParamDel[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DeletePurchaseOrderItemData", dbParamDel);

            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                DbParameter[] dbParam1 = new DbParameter[12];
                dbParam1[0] = new DbParameter("@pordid", DbParameter.DbType.Int, 4, POID);
                dbParam1[1] = new DbParameter("@podocid", DbParameter.DbType.VarChar, 30, docID);
                dbParam1[2] = new DbParameter("@sno", DbParameter.DbType.Int, 4, (i + 1));
                dbParam1[3] = new DbParameter("@vdate", DbParameter.DbType.DateTime, 50, Currdt);
                dbParam1[4] = new DbParameter("@distid", DbParameter.DbType.Int, 4, Convert.ToInt32(CustID));
                dbParam1[5] = new DbParameter("@userid", DbParameter.DbType.Int, 4, Convert.ToInt32(UserID));
                dbParam1[6] = new DbParameter("@itemid", DbParameter.DbType.Int, 4, Convert.ToInt32(dtItem.Rows[i]["ItemID"]));
                dbParam1[7] = new DbParameter("@qty", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(dtItem.Rows[i]["Qty"]));
                dbParam1[8] = new DbParameter("@disc", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(0));
                dbParam1[9] = new DbParameter("@remarks", DbParameter.DbType.VarChar, 255, Convert.ToString(dtItem.Rows[i]["Remarks"]));
                dbParam1[10] = new DbParameter("@rate", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(dtItem.Rows[i]["Rate"]));
                dbParam1[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
                DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_InsertPurchaseorder", dbParam1);

            }
            return Convert.ToInt32(dbParam[21].Value);
        }

        public int delete(int POID)
        {
            DbParameter[] dbParam= new DbParameter[2];
            dbParam[0] = new DbParameter("@POrdId", DbParameter.DbType.Int, 4, POID);
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DeletePOData", dbParam);

            DbParameter[] dbParamDel = new DbParameter[1];
            dbParamDel[0] = new DbParameter("@POrdId", DbParameter.DbType.Int, 4, POID);
            //dbParamDel[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DeletePurchaseOrderItemData", dbParamDel);

            return Convert.ToInt32(dbParam[1].Value);
        }

        public int CancelOrder(int POID)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@POrdId", DbParameter.DbType.Int, 4, POID);
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CancelOrder_PO", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }

        public int HoldOrder(int POID)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@POrdId", DbParameter.DbType.Int, 4, POID);
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_OnHoldOrder_PO", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }

        public int ConfirmOrder(int POID, DataTable dtItem, DataTable dt,DateTime VDate,int UserID,int SMID)
        {  
            DbParameter[] dbParam = new DbParameter[19];
            dbParam[0] = new DbParameter("@podocid", DbParameter.DbType.VarChar, 30, Convert.ToString(dt.Rows[0]["PODocId"]));
            dbParam[1] = new DbParameter("@vdate", DbParameter.DbType.DateTime, 50, VDate);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserID);
            dbParam[3] = new DbParameter("@smid", DbParameter.DbType.Int, 4, Convert.ToInt32(dt.Rows[0]["SMId"]));
            dbParam[4] = new DbParameter("@distid", DbParameter.DbType.Int, 4, Convert.ToInt32(dt.Rows[0]["DistId"]));
            dbParam[5] = new DbParameter("@remarks", DbParameter.DbType.VarChar, 4000, Convert.ToString(dt.Rows[0]["Remarks"]));
            dbParam[6] = new DbParameter("@dispname", DbParameter.DbType.VarChar, 100, Convert.ToString(dt.Rows[0]["DispName"]));
            dbParam[7] = new DbParameter("@dispadd1", DbParameter.DbType.VarChar, 100, Convert.ToString(dt.Rows[0]["DispAdd1"]));
            dbParam[8] = new DbParameter("@dispadd2", DbParameter.DbType.VarChar, 100, Convert.ToString(dt.Rows[0]["DispAdd2"]));
            dbParam[9] = new DbParameter("@dispcity", DbParameter.DbType.VarChar, 50, Convert.ToString(dt.Rows[0]["DispCity"]));
            dbParam[10] = new DbParameter("@disppin", DbParameter.DbType.VarChar, 6, Convert.ToString(dt.Rows[0]["DispPin"]));
            dbParam[11] = new DbParameter("@dispstate", DbParameter.DbType.VarChar, 25, Convert.ToString(dt.Rows[0]["DispState"]));
            dbParam[12] = new DbParameter("@dispcountry", DbParameter.DbType.VarChar, 25, Convert.ToString(dt.Rows[0]["DispCountry"]));
            dbParam[13] = new DbParameter("@dispphone", DbParameter.DbType.VarChar, 25, Convert.ToString(dt.Rows[0]["DispPhone"]));
            dbParam[14] = new DbParameter("@dispmobile", DbParameter.DbType.VarChar, 25, Convert.ToString(dt.Rows[0]["DispMobile"]));
            dbParam[15] = new DbParameter("@dispemail", DbParameter.DbType.VarChar, 100, Convert.ToString(dt.Rows[0]["DispEmail"]));
            dbParam[16] = new DbParameter("@orderstatus", DbParameter.DbType.VarChar, 1, "M");         
            dbParam[17] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[18] = new DbParameter("@pordid", DbParameter.DbType.Int, 4, POID);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_InsertConfirmPOData", dbParam);
           

            decimal discount = 0M;

            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                if(dtItem.Rows[i]["Discount"].ToString()!="")
                {
                    discount=Convert.ToDecimal(dtItem.Rows[i]["Discount"]);
                }
                string strDocID = "";
                strDocID = Convert.ToString(dt.Rows[0]["PODocId"]);

                strDocID += "/"+Convert.ToString(i + 1);
                DbParameter[] dbParam1 = new DbParameter[14];
                dbParam1[0] = new DbParameter("@pordid", DbParameter.DbType.Int, 4, POID);
                dbParam1[1] = new DbParameter("@podocid", DbParameter.DbType.VarChar, 30, strDocID);
                dbParam1[2] = new DbParameter("@sno", DbParameter.DbType.Int, 4, (i + 1));
                dbParam1[3] = new DbParameter("@vdate", DbParameter.DbType.DateTime, 50, VDate);
                dbParam1[4] = new DbParameter("@distid", DbParameter.DbType.Int, 4, Convert.ToInt32(dt.Rows[0]["DistId"]));
                dbParam1[5] = new DbParameter("@userid", DbParameter.DbType.Int, 4,UserID);
                dbParam1[6] = new DbParameter("@itemid", DbParameter.DbType.Int, 4, Convert.ToInt32(dtItem.Rows[i]["ItemID"]));
                dbParam1[7] = new DbParameter("@qty", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(dtItem.Rows[i]["ConfQty"]));
                dbParam1[8] = new DbParameter("@disc", DbParameter.DbType.Decimal, 18, discount);               
                dbParam1[9] = new DbParameter("@rate", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(dtItem.Rows[i]["Rate"]));
                dbParam1[10] = new DbParameter("@location", DbParameter.DbType.Int, 4, Convert.ToInt32(dtItem.Rows[i]["LocationID"]));
                dbParam1[11] = new DbParameter("@remarks", DbParameter.DbType.VarChar, 255, Convert.ToString(dtItem.Rows[i]["Remarks"]));
                dbParam1[12] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
                dbParam1[13] = new DbParameter("@pord1id", DbParameter.DbType.Int, 4, dbParam[17].Value);
                DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_InsertPOConfirm_Child", dbParam1);
                

            }

            DbParameter[] dbParam2 = new DbParameter[2];
            dbParam2[0] = new DbParameter("@POrdId", DbParameter.DbType.Int, 4, POID);
            dbParam2[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ConfirmOrder_PO", dbParam2);

            return Convert.ToInt32(dbParam[17].Value);



        }

        public int InsertItemForCart(int ItemID, string ItemName, int Packing, int Qty, decimal Price, string Unit,
            decimal Total, string PriceGroup, string Remarks, int UserId, int SMID, DateTime CreatedDate,int DistID)
        {           
            DbParameter[] dbParam = new DbParameter[15];
            dbParam[0] = new DbParameter("@itemid", DbParameter.DbType.Int, 4, ItemID);
            dbParam[1] = new DbParameter("@itemname", DbParameter.DbType.VarChar, 50, ItemName);
            dbParam[2] = new DbParameter("@packing", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(Packing));
            dbParam[3] = new DbParameter("@qty", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(Qty));
            dbParam[4] = new DbParameter("@price", DbParameter.DbType.Decimal, 18, Price);
            dbParam[5] = new DbParameter("@unit", DbParameter.DbType.VarChar, 20, Unit);
            dbParam[6] = new DbParameter("@total", DbParameter.DbType.Decimal, 18, Total);
            dbParam[7] = new DbParameter("@pricegroup", DbParameter.DbType.VarChar, 50, PriceGroup);
            dbParam[8] = new DbParameter("@remarks", DbParameter.DbType.VarChar, 255, Remarks);
            dbParam[9] = new DbParameter("@userid", DbParameter.DbType.Int, 4, UserId);
            dbParam[10] = new DbParameter("@smid", DbParameter.DbType.Int, 4, SMID);
            dbParam[11] = new DbParameter("@createddate", DbParameter.DbType.DateTime, 50, CreatedDate);
            dbParam[12] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[14] = new DbParameter("@distid", DbParameter.DbType.Int, 4, DistID);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_InsertDistributorItemDetails", dbParam);        
            return Convert.ToInt32(dbParam[13].Value);			  
		
        }
        public int UpdateItemForCart(int ItemID, string ItemName, int Packing, int Qty, decimal Price, string Unit,
          decimal Total, string PriceGroup, string Remarks, int UserId, int SMID, DateTime CreatedDate, int DistID)
        {
            DbParameter[] dbParam = new DbParameter[15];
            dbParam[0] = new DbParameter("@itemid", DbParameter.DbType.Int, 4, ItemID);
            dbParam[1] = new DbParameter("@itemname", DbParameter.DbType.VarChar, 50, ItemName);
            dbParam[2] = new DbParameter("@packing", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(Packing));
            dbParam[3] = new DbParameter("@qty", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(Qty));
            dbParam[4] = new DbParameter("@price", DbParameter.DbType.Decimal, 18, Price);
            dbParam[5] = new DbParameter("@unit", DbParameter.DbType.VarChar, 20, Unit);
            dbParam[6] = new DbParameter("@total", DbParameter.DbType.Decimal, 18, Total);
            dbParam[7] = new DbParameter("@pricegroup", DbParameter.DbType.VarChar, 50, PriceGroup);
            dbParam[8] = new DbParameter("@remarks", DbParameter.DbType.VarChar, 255, Remarks);
            dbParam[9] = new DbParameter("@userid", DbParameter.DbType.Int, 4, UserId);
            dbParam[10] = new DbParameter("@smid", DbParameter.DbType.Int, 4, SMID);
            dbParam[11] = new DbParameter("@createddate", DbParameter.DbType.DateTime, 50, CreatedDate);
            dbParam[12] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[14] = new DbParameter("@distid", DbParameter.DbType.Int, 4, DistID);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_InsertDistributorItemDetails", dbParam);
            return Convert.ToInt32(dbParam[13].Value);

        }

        public int deleteCartItem(int CartID)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@ID", DbParameter.DbType.Int, 4, CartID);
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DeleteCartData", dbParam);       

            return Convert.ToInt32(dbParam[1].Value);
        }

        public int InsertPlaceOrder(DateTime Currdt, string docID, string UserID, string SMID, string CustID,
            int TransPorterID, string DispName, string DispAdd1, string DispAdd2, string Country, string State,
            string City, string Pin, string Phone, string Mobile, string Email, DataTable dtItem, string Remarks, string PType, int ProjectID, decimal decAmtTotal, int CityID, string TransPorter, int SchemeID, string IpAddr)
        {
            DbParameter[] dbParam = new DbParameter[28];
            dbParam[0] = new DbParameter("@podocid", DbParameter.DbType.VarChar, 30, docID);
            dbParam[1] = new DbParameter("@vdate", DbParameter.DbType.DateTime, 50, Currdt);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, Convert.ToInt32(UserID));
            dbParam[3] = new DbParameter("@smid", DbParameter.DbType.Int, 4, Convert.ToInt32(SMID));
            dbParam[4] = new DbParameter("@distid", DbParameter.DbType.Int, 4, Convert.ToInt32(CustID));
            dbParam[5] = new DbParameter("@remarks", DbParameter.DbType.VarChar, 4000, Remarks);
            dbParam[6] = new DbParameter("@transporter", DbParameter.DbType.VarChar, 100, TransPorter);
            dbParam[7] = new DbParameter("@dispname", DbParameter.DbType.VarChar, 100, DispName);
            dbParam[8] = new DbParameter("@dispadd1", DbParameter.DbType.VarChar, 100, DispAdd1);
            dbParam[9] = new DbParameter("@dispadd2", DbParameter.DbType.VarChar, 100, DispAdd2);
            dbParam[10] = new DbParameter("@dispcity", DbParameter.DbType.VarChar, 50, City);
            dbParam[11] = new DbParameter("@disppin", DbParameter.DbType.VarChar, 6, Pin);
            dbParam[12] = new DbParameter("@dispstate", DbParameter.DbType.VarChar, 25, State);
            dbParam[13] = new DbParameter("@dispcountry", DbParameter.DbType.VarChar, 25, Country);
            dbParam[14] = new DbParameter("@dispphone", DbParameter.DbType.VarChar, 25, Phone);
            dbParam[15] = new DbParameter("@dispmobile", DbParameter.DbType.VarChar, 25, Mobile);
            dbParam[16] = new DbParameter("@dispemail", DbParameter.DbType.VarChar, 100, Email);
            dbParam[17] = new DbParameter("@orderstatus", DbParameter.DbType.VarChar, 1, "P");
            dbParam[18] = new DbParameter("@createddate", DbParameter.DbType.DateTime, 50, Currdt);
            dbParam[19] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[20] = new DbParameter("@POrdId", DbParameter.DbType.Int, 4, 0);
            dbParam[21] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[22] = new DbParameter("@projecttype", DbParameter.DbType.VarChar, 1, PType);
            dbParam[23] = new DbParameter("@projectid", DbParameter.DbType.Int, 4, ProjectID);
            dbParam[24] = new DbParameter("@city", DbParameter.DbType.Int, 4, CityID);
            dbParam[25] = new DbParameter("@ordervalue", DbParameter.DbType.Decimal, 18, decAmtTotal);
            dbParam[26] = new DbParameter("@SchemeID", DbParameter.DbType.Int, 4, SchemeID);
            dbParam[27] = new DbParameter("@IpAddr", DbParameter.DbType.VarChar, 50, IpAddr);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_InsertPOData", dbParam);


            for (int i = 0; i < dtItem.Rows.Count; i++)
            {
                DbParameter[] dbParam1 = new DbParameter[12];
                dbParam1[0] = new DbParameter("@pordid", DbParameter.DbType.Int, 4, dbParam[21].Value);
                dbParam1[1] = new DbParameter("@podocid", DbParameter.DbType.VarChar, 30, docID);
                dbParam1[2] = new DbParameter("@sno", DbParameter.DbType.Int, 4, (i + 1));
                dbParam1[3] = new DbParameter("@vdate", DbParameter.DbType.DateTime, 50, Currdt);
                dbParam1[4] = new DbParameter("@distid", DbParameter.DbType.Int, 4, Convert.ToInt32(CustID));
                dbParam1[5] = new DbParameter("@userid", DbParameter.DbType.Int, 4, Convert.ToInt32(UserID));
                dbParam1[6] = new DbParameter("@itemid", DbParameter.DbType.Int, 4, Convert.ToInt32(dtItem.Rows[i]["ItemID"]));
                dbParam1[7] = new DbParameter("@qty", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(dtItem.Rows[i]["Qty"]));
                dbParam1[8] = new DbParameter("@disc", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(0));
                dbParam1[9] = new DbParameter("@remarks", DbParameter.DbType.VarChar, 255, Convert.ToString(dtItem.Rows[i]["Remarks"]));
                dbParam1[10] = new DbParameter("@rate", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(dtItem.Rows[i]["Rate"]));
                dbParam1[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
                DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_InsertPurchaseorder", dbParam1);

            }


            //Delete Item Inside Cart According to User
            DbParameter[] dbParamDel = new DbParameter[1];
            dbParamDel[0] = new DbParameter("@DistID", DbParameter.DbType.Int, 4, Convert.ToInt32(CustID));
            //dbParamDel[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DeleteCartItemData", dbParamDel);


            return Convert.ToInt32(dbParam[21].Value);
        }

        public int CancelOwnOrder(int POID)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@POrdId", DbParameter.DbType.Int, 4, POID);
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CancelOrder_PO", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }



        public int CancelOwnOrder_Dist(int POID)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@POrdId", DbParameter.DbType.Int, 4, POID);
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CancelOwnOrder_PO", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }

        public string InsertPurchaseImportData(string PODocId, string Vdate, string EmployeeSyncId, string DistSyncId, 
                                              string PortalNo, string ItemSyncID, string Qty,
                                                string LocationSyncID, string ItemRemarks, string ItemwiseTotal, string PendingQty, string Rate, string ShippingQty)
        {          
         
            DbParameter[] dbParam = new DbParameter[14];
            dbParam[0] = new DbParameter("@PODocId", DbParameter.DbType.VarChar, 30, PODocId);
            dbParam[1] = new DbParameter("@Vdate", DbParameter.DbType.DateTime, 50, Convert.ToDateTime(Vdate));
            dbParam[2] = new DbParameter("@EmployeeSyncId", DbParameter.DbType.VarChar, 50, EmployeeSyncId);           
            dbParam[3] = new DbParameter("@DistSyncId", DbParameter.DbType.VarChar, 50, DistSyncId);           
            dbParam[4] = new DbParameter("@PortalNo", DbParameter.DbType.VarChar, 50, PortalNo);
            dbParam[5] = new DbParameter("@ItemSyncID", DbParameter.DbType.VarChar, 50, ItemSyncID);
            dbParam[6] = new DbParameter("@Qty", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(Qty));          
            dbParam[7] = new DbParameter("@LocationSyncID", DbParameter.DbType.VarChar, 50, LocationSyncID);
            dbParam[8] = new DbParameter("@ItemRemarks", DbParameter.DbType.VarChar, 255, ItemRemarks);
            dbParam[9] = new DbParameter("@ItemwiseTotal", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(ItemwiseTotal));
            dbParam[10] = new DbParameter("@PendingQty", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(PendingQty));         
            dbParam[11] = new DbParameter("@ReturnVal", DbParameter.DbType.VarChar, 100, ParameterDirection.Output);
            dbParam[12] = new DbParameter("@Rate", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(Rate));
            dbParam[13] = new DbParameter("@ShippingQty", DbParameter.DbType.Decimal, 18, Convert.ToDecimal(ShippingQty));  

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_InsertImportPOData", dbParam);

            return Convert.ToString(dbParam[11].Value);
        }
        
       
    }
}
