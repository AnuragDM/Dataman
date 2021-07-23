using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FFMS
{
    public class ImportParty
    {
        public string InsertParties(string PartyName, string Address1, string Address2, string Pin, string Mobile, string Industry, string Potential, string Remark, string DistributorSynchId, string SyncID, string active, string partyType,string State,string District, string city, string area, string beat, string contactPerson, string phone, string cstNo, string vatTIN, string serviceTax, string panNo,string PartyId, string EMail,string GSTINNo)
        {
            string result = ""; //Sp_InsertParties
            string sqlCommand = "Sp_AstralInsertParties";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PartyId", PartyId);
            cmd.Parameters.AddWithValue("@PartyName", PartyName.ToUpper());
            cmd.Parameters.AddWithValue("@Address1", Address1.ToUpper());
            cmd.Parameters.AddWithValue("@Address2", Address2.ToUpper());
            cmd.Parameters.AddWithValue("@Pin", Pin);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@Industry", Industry);
            if (Potential != "")
            {
                cmd.Parameters.AddWithValue("@Potential", Convert.ToDecimal(Potential));
            }
            else
            {
                cmd.Parameters.AddWithValue("@Potential", DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@Remark", Remark);
            cmd.Parameters.AddWithValue("@DistributorSynchId", DistributorSynchId);
            cmd.Parameters.AddWithValue("@StateName", State);
            cmd.Parameters.AddWithValue("@DistrictName", District);
            cmd.Parameters.AddWithValue("@CityName", city);
            cmd.Parameters.AddWithValue("@AreaName", area);
            cmd.Parameters.AddWithValue("@BeatName", beat);
            cmd.Parameters.AddWithValue("@ContactPerson", contactPerson);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@CST ", cstNo);
            cmd.Parameters.AddWithValue("@VAT", vatTIN);
            cmd.Parameters.AddWithValue("@SERTAX", serviceTax);
            cmd.Parameters.AddWithValue("@PAN", panNo);
            cmd.Parameters.AddWithValue("@Active", active);
            cmd.Parameters.AddWithValue("@UserID", Settings.Instance.UserID);
            cmd.Parameters.AddWithValue("@SyncID", SyncID);
            cmd.Parameters.AddWithValue("@PartyType", partyType);
            cmd.Parameters.AddWithValue("@EMail", EMail);
            cmd.Parameters.AddWithValue("@GSTINNo", GSTINNo);
            SqlParameter OutputParam = new SqlParameter("@OutputParam", SqlDbType.VarChar, 500);
            OutputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(OutputParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
                result = OutputParam.Value.ToString();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
        }
        public string ImportDistributors(string DistributorName, string Address1, string Address2, string City, string area, string Pin, string Email, string Mobile, string Phone, string CST, string VAT, string SERTAX, string PAN, string SyncID, string UserName, string RoleName, bool Active, decimal CreditLimit, decimal Outstanding, int Creditdays, decimal OpenOrder, string Remarks, string EmployeeSyncId, string DistributorName2, string ContactPerson, string Telex, string Fax, string DistType = "0", string SuperDistID = "0")
        {
            string result = "";
            string sqlCommand = "Sp_InsertDistributors";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DistName", DistributorName.Trim());
            cmd.Parameters.AddWithValue("@Address1", Address1.Trim());
            cmd.Parameters.AddWithValue("@Address2", Address2.Trim());
            cmd.Parameters.AddWithValue("@City", City.Trim());
            cmd.Parameters.AddWithValue("@AreaNm", area.Trim());
            cmd.Parameters.AddWithValue("@Pin", Pin.Trim());
            cmd.Parameters.AddWithValue("@Email", Email.Trim());
            cmd.Parameters.AddWithValue("@Mobile", Mobile.Trim());
            cmd.Parameters.AddWithValue("@Phone", Phone.Trim());
            cmd.Parameters.AddWithValue("@CST", CST.Trim());
            if (!string.IsNullOrEmpty(VAT))
                cmd.Parameters.AddWithValue("@VAT", VAT);
            else
                cmd.Parameters.AddWithValue("@VAT", 0);
            if (!string.IsNullOrEmpty(SERTAX))
                cmd.Parameters.AddWithValue("@SERTAX", SERTAX);
            else
                cmd.Parameters.AddWithValue("@SERTAX", 0);
            cmd.Parameters.AddWithValue("@Pan", PAN);
            cmd.Parameters.AddWithValue("@SyncID", SyncID.Trim());
            cmd.Parameters.AddWithValue("@RoleName", RoleName.Trim());
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@CreditLimit", CreditLimit);
            cmd.Parameters.AddWithValue("@Outstanding", Outstanding);
            cmd.Parameters.AddWithValue("@Creditdays", Creditdays);
            cmd.Parameters.AddWithValue("@OpenOrder", OpenOrder);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            cmd.Parameters.AddWithValue("@EmployeeSyncId", EmployeeSyncId);
            cmd.Parameters.AddWithValue("@CreatedUserID", Settings.Instance.UserID);
            cmd.Parameters.AddWithValue("@DistributorName2", DistributorName2);
            cmd.Parameters.AddWithValue("@ContactPerson", ContactPerson);
            cmd.Parameters.AddWithValue("@Telex", Telex);
            cmd.Parameters.AddWithValue("@Fax", Fax);
            if (DistType == "0")
            {               
                cmd.Parameters.AddWithValue("@DistType", DBNull.Value);
            }
            else
            {               
                cmd.Parameters.AddWithValue("@DistType", DistType);
            }
            if (SuperDistID == "0")
            {
                cmd.Parameters.AddWithValue("@SD_ID", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@SD_ID", SuperDistID);
            }
            SqlParameter OutputParam = new SqlParameter("@OutputParam", SqlDbType.VarChar, 100);
            OutputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(OutputParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
                result = OutputParam.Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
        }
        public int InsertDistributors(string PartyName, string Address1, string Address2, string CityId, string AreaId, string Pin, string Email, string Mobile, string Remark, string SyncId, string UserName, bool Active,
            string BlockReason, string RoleID, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, string CreditLimit, string OutStanding)
        {
            int result = 0;
            string sqlCommand = "Sp_InsertDistributorForm";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PartyName", PartyName.ToUpper());
            cmd.Parameters.AddWithValue("@Address1", Address1.ToUpper());
            cmd.Parameters.AddWithValue("@Address2", Address2.ToUpper());
            cmd.Parameters.AddWithValue("@CityId",Convert.ToInt32(CityId));
            cmd.Parameters.AddWithValue("@AreaId", AreaId);
            cmd.Parameters.AddWithValue("@Pin", Pin);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@Remarks", Remark.ToUpper());
            cmd.Parameters.AddWithValue("@SyncId", SyncId);
            cmd.Parameters.AddWithValue("@UserName", UserName.ToUpper());
            cmd.Parameters.AddWithValue("@RoleID", Convert.ToInt32(RoleID));
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@ContactPerson", ContactPerson);
            cmd.Parameters.AddWithValue("@CSTNo", CSTNo);
            if(!string.IsNullOrEmpty(VatTin))
            cmd.Parameters.AddWithValue("@VatTin", VatTin);
            else
            cmd.Parameters.AddWithValue("@VatTin", 0);
            if (!string.IsNullOrEmpty(ServiceTax))
            cmd.Parameters.AddWithValue("@ServiceTax", ServiceTax);
            else
                cmd.Parameters.AddWithValue("@ServiceTax", 0);
            cmd.Parameters.AddWithValue("@PanNo", PanNo);
            if(!string.IsNullOrEmpty(CreditLimit))
            cmd.Parameters.AddWithValue("@CreditLimit", CreditLimit);
            else
                cmd.Parameters.AddWithValue("@CreditLimit", 0);
            if(!string.IsNullOrEmpty(OutStanding))
            cmd.Parameters.AddWithValue("@OutStanding", OutStanding);
            else
                cmd.Parameters.AddWithValue("@OutStanding", 0);
            if (!Active)
            {
                cmd.Parameters.AddWithValue("@BlockBy", Settings.Instance.UserID);
                cmd.Parameters.AddWithValue("@BlockDate", DateTime.Now.ToUniversalTime().AddSeconds(19800));
                cmd.Parameters.AddWithValue("@BlockReason", BlockReason);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BlockBy",DBNull.Value);
                cmd.Parameters.AddWithValue("@BlockDate", DBNull.Value);
                cmd.Parameters.AddWithValue("@BlockReason", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@CreatedUserId", Settings.Instance.UserID);
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToUniversalTime().AddSeconds(19800));
          
            SqlParameter retParam = cmd.CreateParameter();
            retParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(retParam.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
        }

        public int UploadDistributors(string PartyName, string Address1, string Address2, string CityId, string AreaId, string Pin, string Email, string Mobile, string SyncId, string UserName, string RoleID, string CreditLimit, string OutStanding)
        {
            int result = 0;
            string sqlCommand = "Sp_UploadDistributorForm";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PartyName", PartyName.ToUpper());
            cmd.Parameters.AddWithValue("@Address1", Address1.ToUpper());
            cmd.Parameters.AddWithValue("@Address2", Address2.ToUpper());
            cmd.Parameters.AddWithValue("@CityId", Convert.ToInt32(CityId));
            cmd.Parameters.AddWithValue("@AreaId", AreaId);
            cmd.Parameters.AddWithValue("@Pin", Pin);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);           
            cmd.Parameters.AddWithValue("@SyncId", SyncId);
            cmd.Parameters.AddWithValue("@UserName", UserName.ToUpper());
            cmd.Parameters.AddWithValue("@RoleID", Convert.ToInt32(RoleID));            
            cmd.Parameters.AddWithValue("@CreditLimit", CreditLimit);
            cmd.Parameters.AddWithValue("@OutStanding", OutStanding);

           

            cmd.Parameters.AddWithValue("@CreatedUserId", Settings.Instance.UserID);
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToUniversalTime().AddSeconds(19800));

            SqlParameter retParam = cmd.CreateParameter();
            retParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(retParam.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
        }


        public int UpdateDistributors(int PartyId,string PartyName, string Address1, string Address2,string CityId, string AreaId, string Pin, string Email, string Mobile, string Remark, string SyncId, bool Active, string BlockReason,
            string RoleID, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo)
        {
            int result = 0;
            string sqlCommand = "Sp_UpdateDistributorForm";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DistId", PartyId);
            cmd.Parameters.AddWithValue("@PartyName", PartyName.ToUpper());
            cmd.Parameters.AddWithValue("@Address1", Address1.ToUpper());
            cmd.Parameters.AddWithValue("@Address2", Address2.ToUpper());
            cmd.Parameters.AddWithValue("@CityId", Convert.ToInt32(CityId));
            cmd.Parameters.AddWithValue("@AreaId", AreaId);
            cmd.Parameters.AddWithValue("@Pin", Pin);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@Remarks", Remark);
            cmd.Parameters.AddWithValue("@SyncId", SyncId);
            cmd.Parameters.AddWithValue("@RoleID", Convert.ToInt32(RoleID));
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@ContactPerson", ContactPerson.ToUpper());
            cmd.Parameters.AddWithValue("@CSTNo", CSTNo.ToUpper());
            cmd.Parameters.AddWithValue("@VatTin", VatTin);
            cmd.Parameters.AddWithValue("@ServiceTax", ServiceTax);
            cmd.Parameters.AddWithValue("@PanNo", PanNo);
            if (!Active)
            {
                cmd.Parameters.AddWithValue("@BlockBy", Settings.Instance.UserID);
                cmd.Parameters.AddWithValue("@BlockDate", DateTime.Now.ToUniversalTime().AddSeconds(19800));
                cmd.Parameters.AddWithValue("@BlockReason", BlockReason);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BlockBy", DBNull.Value);
                cmd.Parameters.AddWithValue("@BlockDate", DBNull.Value);
                cmd.Parameters.AddWithValue("@BlockReason", DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToUniversalTime().AddSeconds(19800));
          

            SqlParameter retParam = cmd.CreateParameter();
            retParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(retParam.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
        }
        public int DeleteDistributors(Int32 DistID)
        {
            int result = 0;
            string sqlCommand = "Sp_DeleteDistributors";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DistID", DistID);
            SqlParameter retParam = cmd.CreateParameter();
            retParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
        }

        public int InsertPartyForm(string PartyName, string Address1, string Address2,Int32 CityId, Int32 AreaId,Int32 BeatId, string Pin, string Mobile, string Remark, string SyncId, Int32 UnderID,Int32 IndId,decimal Potential, bool Active,string BlockReason,
            int PartyType, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo)
        {
            int result = 0;
            string sqlCommand = "Sp_InsertPartyForm";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PartyName", PartyName);
            cmd.Parameters.AddWithValue("@Address1", Address1);
            cmd.Parameters.AddWithValue("@Address2", Address2);
            cmd.Parameters.AddWithValue("@CityId", CityId);
            cmd.Parameters.AddWithValue("@AreaId", AreaId);
            cmd.Parameters.AddWithValue("@BeatId", BeatId);
            cmd.Parameters.AddWithValue("@Pin", Pin);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@Remarks", Remark);
            cmd.Parameters.AddWithValue("@SyncId", SyncId);
            cmd.Parameters.AddWithValue("@UnderId", UnderID);
            cmd.Parameters.AddWithValue("@IndId", IndId);
            cmd.Parameters.AddWithValue("@Potential", Potential);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@PartyType", PartyType);
            cmd.Parameters.AddWithValue("@ContactPerson", ContactPerson);
            cmd.Parameters.AddWithValue("@CSTNo", CSTNo);
            cmd.Parameters.AddWithValue("@VatTin", VatTin);
            cmd.Parameters.AddWithValue("@ServiceTax", ServiceTax);
            cmd.Parameters.AddWithValue("@PanNo", PanNo);
            if (!Active)
            {
                cmd.Parameters.AddWithValue("@BlockBy", Settings.Instance.UserID);
                cmd.Parameters.AddWithValue("@BlockDate", DateTime.Now.ToUniversalTime().AddSeconds(19800));
                cmd.Parameters.AddWithValue("@BlockReason", BlockReason);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BlockBy", DBNull.Value);
                cmd.Parameters.AddWithValue("@BlockDate", DBNull.Value);
                cmd.Parameters.AddWithValue("@BlockReason", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@CreatedUserId", Settings.Instance.UserID);
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToUniversalTime().AddSeconds(19800));

            SqlParameter retParam = cmd.CreateParameter();
            retParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(retParam.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
        }

        public int UpdatePartyForm(int PartyId, string PartyName, string Address1, string Address2, Int32 CityId, Int32 AreaId, Int32 BeatId, string Pin, string Mobile, string Remark, string SyncId, Int32 UnderID, Int32 IndId, decimal Potential, bool Active, string BlockReason,
            string PartyType, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo)
        {
            int result = 0;
            string sqlCommand = "Sp_UpdatePartyForm";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PartyId", PartyId);
            cmd.Parameters.AddWithValue("@PartyName", PartyName);
            cmd.Parameters.AddWithValue("@Address1", Address1);
            cmd.Parameters.AddWithValue("@Address2", Address2);
            cmd.Parameters.AddWithValue("@CityId", CityId);
            cmd.Parameters.AddWithValue("@AreaId", AreaId);
            cmd.Parameters.AddWithValue("@BeatId", BeatId);
            cmd.Parameters.AddWithValue("@Pin", Pin);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@Remarks", Remark);
            cmd.Parameters.AddWithValue("@SyncId", SyncId);
            cmd.Parameters.AddWithValue("@UnderID", UnderID);
            cmd.Parameters.AddWithValue("@IndId", IndId);
            cmd.Parameters.AddWithValue("@Potential", Potential);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@PartyType",Convert.ToInt32(PartyType));
            cmd.Parameters.AddWithValue("@ContactPerson", ContactPerson);
            cmd.Parameters.AddWithValue("@CSTNo", CSTNo);
            cmd.Parameters.AddWithValue("@VatTin", VatTin);
            cmd.Parameters.AddWithValue("@ServiceTax", ServiceTax);
            cmd.Parameters.AddWithValue("@PanNo", PanNo);
            if (!Active)
            {
                cmd.Parameters.AddWithValue("@BlockBy", Settings.Instance.UserID);
                cmd.Parameters.AddWithValue("@BlockDate", DateTime.Now.ToUniversalTime().AddSeconds(19800));
                cmd.Parameters.AddWithValue("@BlockReason", BlockReason);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BlockBy", DBNull.Value);
                cmd.Parameters.AddWithValue("@BlockDate", DBNull.Value);
                cmd.Parameters.AddWithValue("@BlockReason", DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToUniversalTime().AddSeconds(19800));
            SqlParameter retParam = cmd.CreateParameter();
            retParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(retParam.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
        }

        public string ImportDistributorInvoice(string DistInvDocId, string VDate, string SyncId, string PriceList, string ItemId, decimal Qty, decimal Rate,
            decimal Amount, decimal taxPer, decimal Tax_Amt, decimal ExciseAmount, decimal Net_Total, string EmployeeSyncId, string LocationSyncID, string ItemRemarks, string Porder, string Category, decimal QtyinKg)
        {
            string result = "";
            string sqlCommand = "Sp_InsertDistributorsInvoice";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DistInvDocId", DistInvDocId.Trim());
            cmd.Parameters.AddWithValue("@VDate", Convert.ToDateTime(VDate));
            cmd.Parameters.AddWithValue("@SyncId", SyncId.Trim());
            cmd.Parameters.AddWithValue("@PriceList", PriceList.Trim());
            cmd.Parameters.AddWithValue("@Itemcode", ItemId.Trim());
            cmd.Parameters.AddWithValue("@Qty", Qty);
            cmd.Parameters.AddWithValue("@Rate", Rate);
            cmd.Parameters.AddWithValue("@Amount", Amount);
            cmd.Parameters.AddWithValue("@taxPer", taxPer);
            cmd.Parameters.AddWithValue("@Tax_Amt", Tax_Amt);
            cmd.Parameters.AddWithValue("@ExciseAmount", ExciseAmount);
            cmd.Parameters.AddWithValue("@Net_Total", Net_Total);
            cmd.Parameters.AddWithValue("@EmployeeSyncId", EmployeeSyncId);
            cmd.Parameters.AddWithValue("@LocationSyncID", LocationSyncID);
            cmd.Parameters.AddWithValue("@ItemRemarks", ItemRemarks);
            cmd.Parameters.AddWithValue("@Porder", Porder);
            cmd.Parameters.AddWithValue("@Category", Category);
            cmd.Parameters.AddWithValue("@QtyinKg", QtyinKg);
            SqlParameter OutputParam = new SqlParameter("@OutputParam", SqlDbType.VarChar, 100);
            OutputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(OutputParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
                result = OutputParam.Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
        }

        public int InsertDistributors(string PartyName, string Address1, string Address2, string AreaId, string Pin, string Email, string Mobile, string Remark, string SyncId, string UserName, bool Active,
           string BlockReason, string RoleID, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo)
        {
            int result = 0;
            string sqlCommand = "Sp_InsertDistributorForm";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PartyName", PartyName.ToUpper());
            cmd.Parameters.AddWithValue("@Address1", Address1.ToUpper());
            cmd.Parameters.AddWithValue("@Address2", Address2.ToUpper());
            cmd.Parameters.AddWithValue("@AreaId", AreaId);
            cmd.Parameters.AddWithValue("@Pin", Pin);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@Remarks", Remark.ToUpper());
            cmd.Parameters.AddWithValue("@SyncId", SyncId);
            cmd.Parameters.AddWithValue("@UserName", UserName.ToUpper());
            cmd.Parameters.AddWithValue("@RoleID", Convert.ToInt32(RoleID));
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@ContactPerson", ContactPerson);
            cmd.Parameters.AddWithValue("@CSTNo", CSTNo);
            cmd.Parameters.AddWithValue("@VatTin", VatTin);
            cmd.Parameters.AddWithValue("@ServiceTax", ServiceTax);
            cmd.Parameters.AddWithValue("@PanNo", PanNo);
            if (!Active)
            {
                cmd.Parameters.AddWithValue("@BlockBy", Settings.Instance.UserID);
                cmd.Parameters.AddWithValue("@BlockDate", DateTime.Now.ToUniversalTime().AddSeconds(19800));
                cmd.Parameters.AddWithValue("@BlockReason", BlockReason);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BlockBy", DBNull.Value);
                cmd.Parameters.AddWithValue("@BlockDate", DBNull.Value);
                cmd.Parameters.AddWithValue("@BlockReason", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@CreatedUserId", Settings.Instance.UserID);
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToUniversalTime().AddSeconds(19800));

            SqlParameter retParam = cmd.CreateParameter();
            retParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(retParam.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
        }

        public int DeleteParty(Int32 PartyID)
        {
            int result = 0;
            string sqlCommand = "Sp_DeleteParty";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PartyID", PartyID);
            SqlParameter retParam = cmd.CreateParameter();
            retParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
        }

        public int DeleteInvoiceData(DataTable dtrows)
        {
            int result = 0;

            for(int i=0;i<dtrows.Rows.Count;i++)
            {
                string sqlCommand = "DELETE FROM TransDistInv WHERE DistInvDocId='"+Convert.ToString(dtrows.Rows[i]["DistInvDocId*"])+"'";
                SqlConnection con = Connection.Instance.GetConnection();
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.CommandType = CommandType.Text;

                string sqlCommand1 = "DELETE FROM TransDistInv1 WHERE DistInvDocId='" + Convert.ToString(dtrows.Rows[i]["DistInvDocId*"]) + "'";
                SqlCommand cmd1 = new SqlCommand(sqlCommand1, con);
                cmd1.CommandType = CommandType.Text;
                try
                {
                    Connection.Instance.OpenConnection(con);
                    result = cmd.ExecuteNonQuery();
                    result = cmd1.ExecuteNonQuery();
                    result = 1;
                }
                catch (Exception ex)
                {
                    result = 0;
                    break;
                }
                finally
                {
                    Connection.Instance.CloseConnection(con);
                }
            }
           
            return result;
        }
    }
}