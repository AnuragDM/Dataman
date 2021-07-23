using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;
namespace BAL
{
    public class DistributorBAL
    {
        public int InsertDistributors(string DistName, string Address1, string Address2, string CityId, string Pin, string Email, string Mobile, string Remark, string SyncId, string BlockReason, string UserName, bool Active, string Phone,
           int RoleID, string ContactPerson, string CSTNo, string VatTin, string PanNo, decimal OpenOrder, decimal CreditLimit, decimal OutStanding, int CreditDays, int UserId, string Telex, string Fax, string Distributor2, Int32 SMID, string DOA, string DOB, int Areid, string Partygstin = "", string imageurl = "", int Partytype = 0, string DistType = "0", string SuperDistID = "0", string ServiceTax = "")
        {
            DbParameter[] dbParam = new DbParameter[39];
            dbParam[0] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, DistName);
            dbParam[1] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
            dbParam[2] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
            dbParam[3] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
            dbParam[4] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
            dbParam[5] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
            dbParam[6] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
            dbParam[7] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
            dbParam[8] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
            dbParam[9] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[10] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
            dbParam[11] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
            dbParam[12] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
            dbParam[13] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
            dbParam[14] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);
            dbParam[15] = new DbParameter("@OpenOrder", DbParameter.DbType.Decimal, 20, OpenOrder);
            dbParam[16] = new DbParameter("@CreditLimit", DbParameter.DbType.Decimal, 20, CreditLimit);
            dbParam[17] = new DbParameter("@OutStanding", DbParameter.DbType.Decimal, 20, OutStanding);
            dbParam[18] = new DbParameter("@CreditDays", DbParameter.DbType.Int, 10, CreditDays);
            if (!Active)
            {
                dbParam[19] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, UserId);
                dbParam[20] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DateTime.Now.ToUniversalTime().AddSeconds(19800));
                dbParam[21] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
            }
            else
            {
                dbParam[19] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
                dbParam[20] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DBNull.Value);
                dbParam[21] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
            }
            dbParam[22] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
            dbParam[23] = new DbParameter("@UserName", DbParameter.DbType.VarChar, 50, UserName);
            dbParam[24] = new DbParameter("@RoleID", DbParameter.DbType.Int, 10, RoleID);
            dbParam[25] = new DbParameter("@Telex", DbParameter.DbType.VarChar, 50, Telex);
            dbParam[26] = new DbParameter("@Fax", DbParameter.DbType.VarChar, 10, Fax);
            dbParam[27] = new DbParameter("@DistributorName2", DbParameter.DbType.VarChar, 100, Distributor2);
            dbParam[28] = new DbParameter("@SMID", DbParameter.DbType.Int, 10, SMID);

            dbParam[29] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            if (DOA != "")
            {
                dbParam[30] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOA));
            }
            else
            {
                dbParam[30] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
            }

            if (DOB != "")
            {
                dbParam[31] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOB));
            }
            else
            {
                dbParam[31] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
            }
            dbParam[32] = new DbParameter("@AreaID", DbParameter.DbType.Int, 10, Areid);



            if (Partytype == 0)
            {
                dbParam[33] = new DbParameter("@Partytype", DbParameter.DbType.Int, 12, DBNull.Value);
            }
            else
            {
                dbParam[33] = new DbParameter("@Partytype", DbParameter.DbType.Int, 12, Partytype);
            }

            if (Partygstin == "")
            {
                dbParam[34] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, DBNull.Value);
            }
            else
            {
                dbParam[34] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, Partygstin);
            }

            dbParam[35] = new DbParameter("@imgurl", DbParameter.DbType.VarChar, 300, imageurl);
            dbParam[36] = new DbParameter("@Createduserid", DbParameter.DbType.Int, 12, UserId);
            if (DistType == "0")
            {
                dbParam[37] = new DbParameter("@DistType", DbParameter.DbType.VarChar, 20, DBNull.Value);
            }
            else
            {
                dbParam[37] = new DbParameter("@DistType", DbParameter.DbType.VarChar, 20, DistType);
            }

            if (SuperDistID == "0")
            {
                dbParam[38] = new DbParameter("@SD_ID", DbParameter.DbType.Int, 12, DBNull.Value);
            }
            else
            {
                dbParam[38] = new DbParameter("@SD_ID", DbParameter.DbType.Int, 12, Convert.ToInt32(SuperDistID));
            }

            //dbParam[33] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, Partygstin);
            //dbParam[34] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertDistributorForm", dbParam);
            return Convert.ToInt32(dbParam[29].Value);
        }

        public int UpdateDistributors(Int32 DistId, string DistName, string Address1, string Address2, string CityId, string Pin, string Email, string Mobile, string Remark, string SyncId, string BlockReason, string UserName, bool Active, string Phone, int RoleID, string ContactPerson, string CSTNo, string VatTin, string PanNo, decimal OpenOrder,
           decimal CreditLimit, decimal OutStanding, int CreditDays, int UserId, string Telex, string Fax, string Distributor2,
           Int32 SMID, string DOA, string DOB, int AreaID, string Partygstin = "", string imageurl = "",
           int Partytype = 0, string DistType = "0", string SuperDistID = "0", string ServiceTax = "")
        {
            DbParameter[] dbParam = new DbParameter[39];
            dbParam[0] = new DbParameter("@DistId", DbParameter.DbType.Int, 10, DistId);
            dbParam[1] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, DistName);
            dbParam[2] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
            dbParam[3] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
            dbParam[4] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
            dbParam[5] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
            dbParam[6] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
            dbParam[7] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
            dbParam[8] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
            dbParam[9] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
            dbParam[10] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[11] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
            dbParam[12] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
            dbParam[13] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
            dbParam[14] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
            dbParam[15] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);
            dbParam[16] = new DbParameter("@OpenOrder", DbParameter.DbType.Decimal, 20, OpenOrder);
            dbParam[17] = new DbParameter("@CreditLimit", DbParameter.DbType.Decimal, 20, CreditLimit);
            dbParam[18] = new DbParameter("@OutStanding", DbParameter.DbType.Decimal, 20, OutStanding);
            dbParam[19] = new DbParameter("@CreditDays", DbParameter.DbType.Int, 10, CreditDays);
            if (!Active)
            {
                dbParam[20] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, UserId);
                dbParam[21] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DateTime.Now.ToUniversalTime().AddSeconds(19800));
                dbParam[22] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
            }
            else
            {
                dbParam[20] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
                dbParam[21] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DBNull.Value);
                dbParam[22] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
            }
            dbParam[23] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
            dbParam[24] = new DbParameter("@RoleID", DbParameter.DbType.Int, 10, RoleID);
            dbParam[25] = new DbParameter("@Telex", DbParameter.DbType.VarChar, 50, Telex);
            dbParam[26] = new DbParameter("@Fax", DbParameter.DbType.VarChar, 10, Fax);
            dbParam[27] = new DbParameter("@DistributorName2", DbParameter.DbType.VarChar, 100, Distributor2);
            dbParam[28] = new DbParameter("@SMID", DbParameter.DbType.Int, 10, SMID);
            dbParam[29] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            if (DOA != "")
            {
                dbParam[30] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOA));
            }
            else
            {
                dbParam[30] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
            }

            if (DOB != "")
            {
                dbParam[31] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOB));
            }
            else
            {
                dbParam[31] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
            }

            dbParam[32] = new DbParameter("@AreaID", DbParameter.DbType.Int, 10, AreaID);



            if (Partytype == 0)
            {
                dbParam[33] = new DbParameter("@Partytype", DbParameter.DbType.Int, 12, DBNull.Value);
            }
            else
            {
                dbParam[33] = new DbParameter("@Partytype", DbParameter.DbType.Int, 12, Partytype);
            }

            if (Partygstin == "")
            {
                dbParam[34] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, DBNull.Value);
            }
            else
            {
                dbParam[34] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, Partygstin);
            }

            dbParam[35] = new DbParameter("@imgurl", DbParameter.DbType.VarChar, 300, imageurl);
            dbParam[36] = new DbParameter("@Createduserid", DbParameter.DbType.Int, 12, UserId);

            if (DistType == "0")
            {
                SuperDistID = "0";
                dbParam[37] = new DbParameter("@DistType", DbParameter.DbType.VarChar, 20, DBNull.Value);
            }
            else
            {
                dbParam[37] = new DbParameter("@DistType", DbParameter.DbType.VarChar, 20, DistType);
            }

            if (SuperDistID == "0")
            {
                dbParam[38] = new DbParameter("@SD_ID", DbParameter.DbType.Int, 12, DBNull.Value);
            }
            else
            {
                dbParam[38] = new DbParameter("@SD_ID", DbParameter.DbType.Int, 12, Convert.ToInt32(SuperDistID));
            }
            //dbParam[33] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, Partygstin);
            //dbParam[34] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_UpdateDistributorForm", dbParam);
            return Convert.ToInt32(dbParam[29].Value);
        }


        public int InsertDistributors_Tally(string DistName, string Address1, string Address2, string CityId, string Pin, string Email, string Mobile, string Remark, string SyncId, string BlockReason, string UserName, bool Active, string Phone,
       int RoleID, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, decimal OpenOrder, decimal CreditLimit, decimal OutStanding, int CreditDays, int UserId, string Telex, string Fax, string Distributor2, Int32 SMID, string DOA, string DOB, int Areid, string TCountryName, string TStateName, int countryid,
          int regionid, int stateid, int distictid, int citytypeid, int cityconveyancetype, string Partygstin = "", string compcode = "")
        {
            DbParameter[] dbParam = new DbParameter[43];
            dbParam[0] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, DistName);
            dbParam[1] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
            dbParam[2] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
            dbParam[3] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
            dbParam[4] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
            dbParam[5] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
            dbParam[6] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
            dbParam[7] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
            dbParam[8] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
            dbParam[9] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[10] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
            dbParam[11] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
            dbParam[12] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
            dbParam[13] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
            dbParam[14] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);
            dbParam[15] = new DbParameter("@OpenOrder", DbParameter.DbType.Decimal, 20, OpenOrder);
            dbParam[16] = new DbParameter("@CreditLimit", DbParameter.DbType.Decimal, 20, CreditLimit);
            dbParam[17] = new DbParameter("@OutStanding", DbParameter.DbType.Decimal, 20, OutStanding);
            dbParam[18] = new DbParameter("@CreditDays", DbParameter.DbType.Int, 10, CreditDays);
            if (!Active)
            {
                dbParam[19] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, UserId);
                dbParam[20] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DateTime.Now.ToUniversalTime().AddSeconds(19800));
                dbParam[21] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
            }
            else
            {
                dbParam[19] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
                dbParam[20] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DBNull.Value);
                dbParam[21] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
            }
            dbParam[22] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
            dbParam[23] = new DbParameter("@UserName", DbParameter.DbType.VarChar, 50, UserName);
            dbParam[24] = new DbParameter("@RoleID", DbParameter.DbType.Int, 10, RoleID);
            dbParam[25] = new DbParameter("@Telex", DbParameter.DbType.VarChar, 50, RoleID);
            dbParam[26] = new DbParameter("@Fax", DbParameter.DbType.VarChar, 10, Fax);
            dbParam[27] = new DbParameter("@DistributorName2", DbParameter.DbType.VarChar, 100, Distributor2);
            dbParam[28] = new DbParameter("@SMID", DbParameter.DbType.Int, 10, SMID);

            dbParam[29] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            if (DOA != "")
            {
                dbParam[30] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOA));
            }
            else
            {
                dbParam[30] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
            }

            if (DOB != "")
            {
                dbParam[31] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOB));
            }
            else
            {
                dbParam[31] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
            }
            dbParam[32] = new DbParameter("@AreaID", DbParameter.DbType.Int, 10, Areid);
            dbParam[33] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, Partygstin);
            dbParam[34] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
            dbParam[35] = new DbParameter("@TCountryName", DbParameter.DbType.VarChar, 100, TCountryName);
            dbParam[36] = new DbParameter("@TStateName", DbParameter.DbType.VarChar, 100, TStateName);
            dbParam[37] = new DbParameter("@countryid", DbParameter.DbType.Int, 10, countryid);
            dbParam[38] = new DbParameter("@regionid", DbParameter.DbType.Int, 10, regionid);
            dbParam[39] = new DbParameter("@distictid", DbParameter.DbType.Int, 10, distictid);


            dbParam[40] = new DbParameter("@citytypeid", DbParameter.DbType.Int, 10, citytypeid);
            dbParam[41] = new DbParameter("@cityconveyancetype", DbParameter.DbType.Int, 10, cityconveyancetype);
            dbParam[42] = new DbParameter("@stateid", DbParameter.DbType.Int, 10, stateid);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertDistributorForm_Tally", dbParam);
            return Convert.ToInt32(dbParam[29].Value);
        }

        public int UpdateDistributors_Tally(string DistName, string Address1, string Address2, string CityId, string Pin, string Email, string Mobile, string Remark, string SyncId, string BlockReason, string UserName, bool Active, string Phone,
                 int RoleID, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, decimal OpenOrder, decimal CreditLimit, decimal OutStanding, int CreditDays, int UserId, string Telex, string Fax, string Distributor2, Int32 SMID, string DOA, string DOB, int AreaID, string TCountryName, string TStateName, int countryid,
          int regionid, int stateid, int distictid, int citytypeid, int cityconveyancetype, string Partygstin = "", string compcode = "")
        {
            DbParameter[] dbParam = new DbParameter[42];

            dbParam[0] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, DistName);
            dbParam[1] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
            dbParam[2] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
            dbParam[3] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
            dbParam[4] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
            dbParam[5] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
            dbParam[6] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
            dbParam[7] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
            dbParam[8] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
            dbParam[9] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[10] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
            dbParam[11] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
            dbParam[12] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
            dbParam[13] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
            dbParam[14] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);
            dbParam[15] = new DbParameter("@OpenOrder", DbParameter.DbType.Decimal, 20, OpenOrder);
            dbParam[16] = new DbParameter("@CreditLimit", DbParameter.DbType.Decimal, 20, CreditLimit);
            dbParam[17] = new DbParameter("@OutStanding", DbParameter.DbType.Decimal, 20, OutStanding);
            dbParam[18] = new DbParameter("@CreditDays", DbParameter.DbType.Int, 10, CreditDays);
            if (!Active)
            {
                dbParam[19] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, UserId);
                dbParam[20] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DateTime.Now.ToUniversalTime().AddSeconds(19800));
                dbParam[21] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
            }
            else
            {
                dbParam[19] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
                dbParam[20] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DBNull.Value);
                dbParam[21] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
            }
            dbParam[22] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
            dbParam[23] = new DbParameter("@RoleID", DbParameter.DbType.Int, 10, RoleID);
            dbParam[24] = new DbParameter("@Telex", DbParameter.DbType.VarChar, 50, Telex);
            dbParam[25] = new DbParameter("@Fax", DbParameter.DbType.VarChar, 10, Fax);
            dbParam[26] = new DbParameter("@DistributorName2", DbParameter.DbType.VarChar, 100, Distributor2);
            dbParam[27] = new DbParameter("@SMID", DbParameter.DbType.Int, 10, SMID);
            dbParam[28] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            if (DOA != "")
            {
                dbParam[29] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOA));
            }
            else
            {
                dbParam[29] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
            }

            if (DOB != "")
            {
                dbParam[30] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOB));
            }
            else
            {
                dbParam[30] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
            }

            dbParam[31] = new DbParameter("@AreaID", DbParameter.DbType.Int, 10, AreaID);
            dbParam[32] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, Partygstin);
            dbParam[33] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
            dbParam[34] = new DbParameter("@TCountryName", DbParameter.DbType.VarChar, 100, TCountryName);
            dbParam[35] = new DbParameter("@TStateName", DbParameter.DbType.VarChar, 100, TStateName);
            dbParam[36] = new DbParameter("@countryid", DbParameter.DbType.Int, 10, countryid);
            dbParam[37] = new DbParameter("@regionid", DbParameter.DbType.Int, 10, regionid);
            dbParam[38] = new DbParameter("@distictid", DbParameter.DbType.Int, 10, distictid);


            dbParam[39] = new DbParameter("@citytypeid", DbParameter.DbType.Int, 10, citytypeid);
            dbParam[40] = new DbParameter("@cityconveyancetype", DbParameter.DbType.Int, 10, cityconveyancetype);
            dbParam[41] = new DbParameter("@stateid", DbParameter.DbType.Int, 10, stateid);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_UpdateDistributorForm_Tally", dbParam);
            return Convert.ToInt32(dbParam[28].Value);
        }

        public int insertdistledger_Tally(Int32 DistId, string Docid, string VDate, decimal amtdr, decimal amtcr, decimal amt, string narr, string VID, string VTYPE, string compcode = "")
        {
            DbParameter[] dbParam = new DbParameter[11];
            dbParam[0] = new DbParameter("@DistId", DbParameter.DbType.Int, 10, DistId);

            dbParam[1] = new DbParameter("@Docid", DbParameter.DbType.VarChar, 30, Docid);
            dbParam[2] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(VDate));
            dbParam[3] = new DbParameter("@amtdr", DbParameter.DbType.Decimal, 20, amtdr);
            dbParam[4] = new DbParameter("@amtcr", DbParameter.DbType.Decimal, 20, amtcr);
            dbParam[5] = new DbParameter("@amt", DbParameter.DbType.Decimal, 20, amt);
            dbParam[6] = new DbParameter("@Narration", DbParameter.DbType.VarChar, 300, narr);
            dbParam[7] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
            dbParam[8] = new DbParameter("@VTYPE", DbParameter.DbType.VarChar, 30, VTYPE);
            dbParam[9] = new DbParameter("@VID", DbParameter.DbType.VarChar, 30, VID);
            dbParam[10] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertDistributorLedgerForm_Tally", dbParam);
            return Convert.ToInt32(dbParam[10].Value);
        }
        public int insertsaleinvheader_Tally(string Docid, string VDate, decimal taxamt, decimal Billamount, decimal Roundoff, string SyncId, string compcode = "")
        {

            DbParameter[] dbParam = new DbParameter[8];


            dbParam[0] = new DbParameter("@Docid", DbParameter.DbType.VarChar, 30, Docid);
            dbParam[1] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(VDate));
            dbParam[2] = new DbParameter("@taxamt", DbParameter.DbType.Decimal, 20, taxamt);
            dbParam[3] = new DbParameter("@Billamount", DbParameter.DbType.Decimal, 20, Billamount);
            dbParam[4] = new DbParameter("@Roundoff", DbParameter.DbType.Decimal, 20, Roundoff);
            dbParam[5] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
            dbParam[6] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
            dbParam[7] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_Insertsaleinvoiceheader_Tally", dbParam);
            return Convert.ToInt32(dbParam[7].Value);
        }

        //public int insertsaleinvdetail_Tally(int DistInvId,int sno , string Docid, string VDate, decimal taxamt, decimal Qty, decimal Rate,decimal Amount, string SyncId,string itemname, string compcode = "")
        //{



        //    DbParameter[] dbParam = new DbParameter[12];

        //      dbParam[0] = new DbParameter("@DistInvId", DbParameter.DbType.Int, 10, DistInvId);
        //     dbParam[1] = new DbParameter("@sno", DbParameter.DbType.Int, 10, sno);
        //    dbParam[2] = new DbParameter("@Docid", DbParameter.DbType.VarChar, 30, Docid);
        //    dbParam[3] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(VDate));
        //    dbParam[4] = new DbParameter("@taxamt", DbParameter.DbType.Decimal, 20, taxamt);
        //    dbParam[5] = new DbParameter("@Qty", DbParameter.DbType.Decimal, 20, Qty);
        //    dbParam[6] = new DbParameter("@Rate", DbParameter.DbType.Decimal, 20, Rate);
        //       dbParam[7] = new DbParameter("@Amount", DbParameter.DbType.Decimal, 20, Amount);
        //    dbParam[8] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
        //          dbParam[9] = new DbParameter("@itemname", DbParameter.DbType.VarChar, 150, itemname);
        //    dbParam[10] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
        //    dbParam[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
        //    DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_Insertsaleinvoicedetail_Tally", dbParam);
        //    return Convert.ToInt32(dbParam[11].Value);
        //}

        public int insertsaleinvdetail_Tally(int DistInvId, int sno, string Docid, string VDate, decimal taxamt, decimal Qty, decimal Rate, decimal Amount, string SyncId, string itemname, string ItemMasterid, string compcode = "")
        {



            DbParameter[] dbParam = new DbParameter[13];

            dbParam[0] = new DbParameter("@DistInvId", DbParameter.DbType.Int, 10, DistInvId);
            dbParam[1] = new DbParameter("@sno", DbParameter.DbType.Int, 10, sno);
            dbParam[2] = new DbParameter("@Docid", DbParameter.DbType.VarChar, 30, Docid);
            dbParam[3] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(VDate));
            dbParam[4] = new DbParameter("@taxamt", DbParameter.DbType.Decimal, 20, taxamt);
            dbParam[5] = new DbParameter("@Qty", DbParameter.DbType.Decimal, 20, Qty);
            dbParam[6] = new DbParameter("@Rate", DbParameter.DbType.Decimal, 20, Rate);
            dbParam[7] = new DbParameter("@Amount", DbParameter.DbType.Decimal, 20, Amount);
            dbParam[8] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
            dbParam[9] = new DbParameter("@itemname", DbParameter.DbType.VarChar, 150, itemname);
            dbParam[10] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
            dbParam[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[12] = new DbParameter("@ItemId", DbParameter.DbType.VarChar, 10, ItemMasterid);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_Insertsaleinvoicedetail_Tally", dbParam);
            return Convert.ToInt32(dbParam[11].Value);
        }

        public int delete(string PartyId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@DistID", DbParameter.DbType.Int, 1, Convert.ToInt32(PartyId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteDistributors", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
        public int InsertDItemStock(string STKDocId, int UserId, string VDate, int VisitId, int SMId, int DistId, string DistCode, int AreaId, int ItemId, decimal Qty, string Android_Id, string Created_date, decimal unit, decimal cases)
        {
            DbParameter[] dbParam = new DbParameter[17];
            dbParam[0] = new DbParameter("@STKId", DbParameter.DbType.Int, 4, 0);
            dbParam[1] = new DbParameter("@STKDocId", DbParameter.DbType.VarChar, 30, STKDocId);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
            dbParam[3] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
            dbParam[4] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
            dbParam[5] = new DbParameter("@DistId", DbParameter.DbType.Int, 4, DistId);
            dbParam[6] = new DbParameter("@DistCode", DbParameter.DbType.VarChar, 50, DistCode);
            dbParam[7] = new DbParameter("@AreaId", DbParameter.DbType.Int, 4, AreaId);
            dbParam[8] = new DbParameter("@ItemId", DbParameter.DbType.Int, 4, ItemId);
            dbParam[9] = new DbParameter("@Qty", DbParameter.DbType.Decimal, 20, Qty);
            dbParam[10] = new DbParameter("@Android_Id", DbParameter.DbType.VarChar, 150, Android_Id);
            dbParam[11] = new DbParameter("@Created_date", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(Created_date));
            dbParam[12] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[13] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisitId);
            dbParam[14] = new DbParameter("@unit", DbParameter.DbType.Decimal, 20, unit);
            dbParam[15] = new DbParameter("@cases", DbParameter.DbType.Decimal, 20, cases);
            dbParam[16] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransDistStock_Ins", dbParam);
            return Convert.ToInt32(dbParam[16].Value);
        }
        public int InsertDItemStock(int STKId, string STKDocId, int UserId, string VDate, int SMId, int DistId, string DistCode, int AreaId, int ItemId, decimal Qty, string Android_Id, string Created_date)
        {
            int visId = 0;
            DbParameter[] dbParam = new DbParameter[15];
            dbParam[0] = new DbParameter("@STKId", DbParameter.DbType.Int, 4, STKId);
            dbParam[1] = new DbParameter("@STKDocId", DbParameter.DbType.VarChar, 30, STKDocId);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
            dbParam[3] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
            dbParam[4] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
            dbParam[5] = new DbParameter("@DistId", DbParameter.DbType.Int, 4, DistId);
            dbParam[6] = new DbParameter("@DistCode", DbParameter.DbType.VarChar, 150, DistCode);
            dbParam[7] = new DbParameter("@AreaId", DbParameter.DbType.Int, 4, AreaId);
            dbParam[8] = new DbParameter("@ItemId", DbParameter.DbType.Int, 4, ItemId);
            dbParam[9] = new DbParameter("@Qty", DbParameter.DbType.Decimal, 20, Qty);
            dbParam[10] = new DbParameter("@Android_Id", DbParameter.DbType.VarChar, 150, Android_Id);
            dbParam[11] = new DbParameter("@Created_date", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(Created_date));
            dbParam[12] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[13] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, visId);
            dbParam[14] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransDistStock_Ins", dbParam);
            return Convert.ToInt32(dbParam[14].Value);
        }
        public int delete(int Id)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Convert.ToInt32(Id));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransDistStock_Del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
        public int insertpendingorder_Aahar(string Docid, string VDate, decimal Amount, decimal Qty, decimal PendingQty, decimal ShippingQty, string ItemRemarks, string DistSyncId, string ItemSyncId, string comp_code)
        {
            DbParameter[] dbParam = new DbParameter[11];

            dbParam[0] = new DbParameter("@PODocId", DbParameter.DbType.VarChar, 30, Docid);
            dbParam[1] = new DbParameter("@VDate", DbParameter.DbType.VarChar, 30, VDate);
            dbParam[2] = new DbParameter("@Amount", DbParameter.DbType.Decimal, 20, Amount);
            dbParam[3] = new DbParameter("@Qty", DbParameter.DbType.Decimal, 20, Qty);
            dbParam[4] = new DbParameter("@PendingQty", DbParameter.DbType.Decimal, 20, PendingQty);
            dbParam[5] = new DbParameter("@ShippingQty", DbParameter.DbType.Decimal, 20, ShippingQty);
            dbParam[6] = new DbParameter("@ItemRemarks", DbParameter.DbType.VarChar, 255, ItemRemarks);
            dbParam[7] = new DbParameter("@DistSyncId", DbParameter.DbType.VarChar, 50, DistSyncId);
            dbParam[8] = new DbParameter("@ItemSyncId", DbParameter.DbType.VarChar, 50, ItemSyncId);

            dbParam[9] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[10] = new DbParameter("@Comp_code", DbParameter.DbType.VarChar, 10, comp_code);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertPurchaseOrderImport_Aahar", dbParam);
            return Convert.ToInt32(dbParam[9].Value);
        }

        public int insertdistledger_Aahar(Int32 DistId, string Docid, string VDate, decimal amtdr, decimal amtcr, decimal amt, string narr, string VID, string VTYPE, int sno, string compcode = "")
        {
            DbParameter[] dbParam = new DbParameter[12];
            dbParam[0] = new DbParameter("@DistId", DbParameter.DbType.Int, 10, DistId);

            dbParam[1] = new DbParameter("@Docid", DbParameter.DbType.VarChar, 30, Docid);
            dbParam[2] = new DbParameter("@VDate", DbParameter.DbType.VarChar, 30, VDate);
            dbParam[3] = new DbParameter("@amtdr", DbParameter.DbType.Decimal, 20, amtdr);
            dbParam[4] = new DbParameter("@amtcr", DbParameter.DbType.Decimal, 20, amtcr);
            dbParam[5] = new DbParameter("@amt", DbParameter.DbType.Decimal, 20, amt);
            dbParam[6] = new DbParameter("@Narration", DbParameter.DbType.VarChar, 300, narr);
            dbParam[7] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 10, compcode);
            dbParam[8] = new DbParameter("@VTYPE", DbParameter.DbType.VarChar, 30, VTYPE);
            dbParam[9] = new DbParameter("@VID", DbParameter.DbType.VarChar, 30, VID);
            dbParam[10] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[11] = new DbParameter("@SNo", DbParameter.DbType.Int, 1, sno);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertDistributorLedgerForm_Aahar", dbParam);
            return Convert.ToInt32(dbParam[10].Value);
        }

        public int InsertDistributors_Aahar(string DistName, string Address1, string Address2, string CityId, string Pin, string Email, string Mobile, string Remark, string SyncId, string BlockReason, string UserName, bool Active, string Phone,
    int RoleID, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, decimal OpenOrder, decimal CreditLimit, decimal OutStanding, int CreditDays, int UserId, string Telex, string Fax, string Distributor2, Int32 SMID, string DOA, string DOB, int Areid
            , int stateid, string StateName, string CityName, int createduserid, string Comp_code, string Partygstin = "")
        {
            DbParameter[] dbParam = new DbParameter[39];
            dbParam[0] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, DistName);
            dbParam[1] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
            dbParam[2] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
            dbParam[3] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
            dbParam[4] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
            dbParam[5] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 35, Mobile);
            dbParam[6] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
            dbParam[7] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
            dbParam[8] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
            dbParam[9] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[10] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
            dbParam[11] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
            dbParam[12] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
            dbParam[13] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
            dbParam[14] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);
            dbParam[15] = new DbParameter("@OpenOrder", DbParameter.DbType.Decimal, 20, OpenOrder);
            dbParam[16] = new DbParameter("@CreditLimit", DbParameter.DbType.Decimal, 20, CreditLimit);
            dbParam[17] = new DbParameter("@OutStanding", DbParameter.DbType.Decimal, 20, OutStanding);
            dbParam[18] = new DbParameter("@CreditDays", DbParameter.DbType.Int, 10, CreditDays);
            if (!Active)
            {
                dbParam[19] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, UserId);
                dbParam[20] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DateTime.Now.ToUniversalTime().AddSeconds(19800));
                dbParam[21] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
            }
            else
            {
                dbParam[19] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
                dbParam[20] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DBNull.Value);
                dbParam[21] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
            }
            dbParam[22] = new DbParameter("@Email", DbParameter.DbType.VarChar, 250, Email);
            dbParam[23] = new DbParameter("@UserName", DbParameter.DbType.VarChar, 50, UserName);
            dbParam[24] = new DbParameter("@RoleID", DbParameter.DbType.Int, 10, RoleID);
            dbParam[25] = new DbParameter("@Telex", DbParameter.DbType.VarChar, 50, RoleID);
            dbParam[26] = new DbParameter("@Fax", DbParameter.DbType.VarChar, 35, Fax);
            dbParam[27] = new DbParameter("@DistributorName2", DbParameter.DbType.VarChar, 100, Distributor2);
            dbParam[28] = new DbParameter("@SMID", DbParameter.DbType.Int, 10, SMID);

            dbParam[29] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            if (DOA != "")
            {
                dbParam[30] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOA));
            }
            else
            {
                dbParam[30] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
            }

            if (DOB != "")
            {
                dbParam[31] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOB));
            }
            else
            {
                dbParam[31] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
            }
            dbParam[32] = new DbParameter("@AreaID", DbParameter.DbType.Int, 10, Areid);
            dbParam[33] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, Partygstin);

            dbParam[34] = new DbParameter("@CityName", DbParameter.DbType.VarChar, 100, CityName);
            dbParam[35] = new DbParameter("@StateName", DbParameter.DbType.VarChar, 100, StateName);
            dbParam[36] = new DbParameter("@stateid", DbParameter.DbType.Int, 10, stateid);
            dbParam[37] = new DbParameter("@createduserid", DbParameter.DbType.Int, 10, createduserid);
            dbParam[38] = new DbParameter("@Comp_code", DbParameter.DbType.VarChar, 10, Comp_code);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertDistributorForm_Aahar", dbParam);
            return Convert.ToInt32(dbParam[29].Value);
        }

        public int UpdateDistributors_Aahar(string DistName, string Address1, string Address2, string CityId, string Pin, string Email, string Mobile, string Remark, string SyncId, string BlockReason, string UserName, bool Active, string Phone,
    int RoleID, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, decimal OpenOrder, decimal CreditLimit, decimal OutStanding, int CreditDays, int UserId, string Telex, string Fax, string Distributor2, Int32 SMID, string DOA, string DOB, int Areid
            , int stateid, string StateName, string CityName, string Comp_code, string Partygstin = "")
        {


            DbParameter[] dbParam = new DbParameter[38];
            dbParam[0] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, DistName);
            dbParam[1] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
            dbParam[2] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
            dbParam[3] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
            dbParam[4] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
            dbParam[5] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 35, Mobile);
            dbParam[6] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
            dbParam[7] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
            dbParam[8] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
            dbParam[9] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[10] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
            dbParam[11] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
            dbParam[12] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
            dbParam[13] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
            dbParam[14] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);
            dbParam[15] = new DbParameter("@OpenOrder", DbParameter.DbType.Decimal, 20, OpenOrder);
            dbParam[16] = new DbParameter("@CreditLimit", DbParameter.DbType.Decimal, 20, CreditLimit);
            dbParam[17] = new DbParameter("@OutStanding", DbParameter.DbType.Decimal, 20, OutStanding);
            dbParam[18] = new DbParameter("@CreditDays", DbParameter.DbType.Int, 10, CreditDays);
            if (!Active)
            {
                dbParam[19] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, UserId);
                dbParam[20] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DateTime.Now.ToUniversalTime().AddSeconds(19800));
                dbParam[21] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
            }
            else
            {
                dbParam[19] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
                dbParam[20] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DBNull.Value);
                dbParam[21] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
            }
            dbParam[22] = new DbParameter("@Email", DbParameter.DbType.VarChar, 250, Email);
            dbParam[23] = new DbParameter("@UserName", DbParameter.DbType.VarChar, 50, UserName);
            dbParam[24] = new DbParameter("@RoleID", DbParameter.DbType.Int, 10, RoleID);
            dbParam[25] = new DbParameter("@Telex", DbParameter.DbType.VarChar, 50, RoleID);
            dbParam[26] = new DbParameter("@Fax", DbParameter.DbType.VarChar, 35, Fax);
            dbParam[27] = new DbParameter("@DistributorName2", DbParameter.DbType.VarChar, 100, Distributor2);
            dbParam[28] = new DbParameter("@SMID", DbParameter.DbType.Int, 10, SMID);

            dbParam[29] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            if (DOA != "")
            {
                dbParam[30] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOA));
            }
            else
            {
                dbParam[30] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
            }

            if (DOB != "")
            {
                dbParam[31] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOB));
            }
            else
            {
                dbParam[31] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
            }
            dbParam[32] = new DbParameter("@AreaID", DbParameter.DbType.Int, 10, Areid);
            dbParam[33] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, Partygstin);

            dbParam[34] = new DbParameter("@CityName", DbParameter.DbType.VarChar, 100, CityName);
            dbParam[35] = new DbParameter("@StateName", DbParameter.DbType.VarChar, 100, StateName);
            dbParam[36] = new DbParameter("@stateid", DbParameter.DbType.Int, 10, stateid);
            dbParam[37] = new DbParameter("@Comp_code", DbParameter.DbType.VarChar, 10, Comp_code);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_UpdateDistributorForm_Aahar", dbParam);
            return Convert.ToInt32(dbParam[29].Value);


        }
        public int insertsaleinvheader_Aahar(string Docid, string VDate, decimal CMN_Addition, decimal Billamount, decimal Roundoff, string SyncId, string compcode = "")
        {

            DbParameter[] dbParam = new DbParameter[8];


            dbParam[0] = new DbParameter("@Docid", DbParameter.DbType.VarChar, 30, Docid);
            dbParam[1] = new DbParameter("@VDate", DbParameter.DbType.VarChar, 30, VDate);

            dbParam[2] = new DbParameter("@Billamount", DbParameter.DbType.Decimal, 20, Billamount);
            dbParam[3] = new DbParameter("@Roundoff", DbParameter.DbType.Decimal, 20, Roundoff);

            dbParam[4] = new DbParameter("@CMN_Addition", DbParameter.DbType.Decimal, 20, CMN_Addition);
            dbParam[5] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);

            dbParam[6] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[7] = new DbParameter("@Comp_code", DbParameter.DbType.VarChar, 10, compcode);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_Insertsaleinvoiceheader_Aahar", dbParam);
            return Convert.ToInt32(dbParam[6].Value);
        }


        public int insertsaleinvdetail_Aahar(int DistInvId, int itemid, int sno, string Docid, string VDate, decimal taxamt, decimal Qty, decimal Rate, decimal Amount, string SyncId, decimal Net_Total, decimal QtyInKg, decimal IGST_Amt, decimal SGST_Amt, decimal CGST_Amt, decimal UGST_Amt)
        {



            DbParameter[] dbParam = new DbParameter[17];

            dbParam[0] = new DbParameter("@DistInvId", DbParameter.DbType.Int, 10, DistInvId);
            dbParam[1] = new DbParameter("@sno", DbParameter.DbType.Int, 10, sno);
            dbParam[2] = new DbParameter("@Docid", DbParameter.DbType.VarChar, 30, Docid);
            dbParam[3] = new DbParameter("@VDate", DbParameter.DbType.VarChar, 30, VDate);
            dbParam[4] = new DbParameter("@taxamt", DbParameter.DbType.Decimal, 20, taxamt);
            dbParam[5] = new DbParameter("@itemid", DbParameter.DbType.Int, 10, itemid);
            dbParam[6] = new DbParameter("@Qty", DbParameter.DbType.Decimal, 20, Qty);
            dbParam[7] = new DbParameter("@Rate", DbParameter.DbType.Decimal, 20, Rate);
            dbParam[8] = new DbParameter("@Amount", DbParameter.DbType.Decimal, 20, Amount);
            dbParam[9] = new DbParameter("@Net_Total", DbParameter.DbType.Decimal, 20, Net_Total);

            dbParam[10] = new DbParameter("@QtyInKg", DbParameter.DbType.Decimal, 20, QtyInKg);
            dbParam[11] = new DbParameter("@IGST_Amt", DbParameter.DbType.Decimal, 20, IGST_Amt);
            dbParam[12] = new DbParameter("@SGST_Amt", DbParameter.DbType.Decimal, 20, SGST_Amt);
            dbParam[13] = new DbParameter("@CGST_Amt", DbParameter.DbType.Decimal, 20, CGST_Amt);
            dbParam[14] = new DbParameter("@UGST_Amt", DbParameter.DbType.Decimal, 20, UGST_Amt);
            dbParam[15] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
            //  dbParam[9] = new DbParameter("@itemname", DbParameter.DbType.VarChar, 150, itemname);
            //   dbParam[10] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
            dbParam[16] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_Insertsaleinvoicedetail_Aahar", dbParam);
            return Convert.ToInt32(dbParam[16].Value);
        }
        public int insertsaleinvexpensedetail_Aahar(string docid, string description, decimal Amount)
        {

            DbParameter[] dbParam = new DbParameter[4];


            dbParam[0] = new DbParameter("@Docid", DbParameter.DbType.VarChar, 30, docid);
            dbParam[1] = new DbParameter("@description", DbParameter.DbType.VarChar, 150, description);
            dbParam[2] = new DbParameter("@Amount", DbParameter.DbType.Decimal, 20, Amount);


            dbParam[3] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_insertsaleinvexpensedetail_Aahar", dbParam);
            return Convert.ToInt32(dbParam[3].Value);
        }
        public int insertsaleinvexpensedetail_Tally(string docid, string description, decimal Amount)
        {

            DbParameter[] dbParam = new DbParameter[4];


            dbParam[0] = new DbParameter("@Docid", DbParameter.DbType.VarChar, 30, docid);
            dbParam[1] = new DbParameter("@description", DbParameter.DbType.VarChar, 150, description);
            dbParam[2] = new DbParameter("@Amount", DbParameter.DbType.Decimal, 20, Amount);


            dbParam[3] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_insertsaleinvexpensedetail_Tally", dbParam);
            return Convert.ToInt32(dbParam[3].Value);
        }
    }
}
