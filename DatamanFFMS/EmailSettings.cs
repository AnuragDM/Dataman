using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;

namespace BAL.EmailSetting
{
    public class EmailSettings
    {
        public int Insert(string SenderEmailID, string Password, string Port, string PurchaseEmailID, string PurchaseEmailCC,
            string OrderApprovalEmailId, string OrderApprovalEmailCC, string ChangePasswordEmailId, string ChangePasswordEmailCC, string MailServer, string ExpenseAdminEmailID)
        {
            DbParameter[] dbParam = new DbParameter[14];
            dbParam[0] = new DbParameter("@SenderEmailId", DbParameter.DbType.VarChar, 50, SenderEmailID);
            dbParam[1] = new DbParameter("@SenderPassword", DbParameter.DbType.VarChar, 50, Password);
            dbParam[2] = new DbParameter("@Port", DbParameter.DbType.Int, 4, Port);
            dbParam[3] = new DbParameter("@PurchaseEmailId", DbParameter.DbType.VarChar, 2000, PurchaseEmailID);
            dbParam[4] = new DbParameter("@PurchaseEmailCC", DbParameter.DbType.VarChar, 2000, PurchaseEmailCC);
            dbParam[5] = new DbParameter("@ChangePasswordEmailId", DbParameter.DbType.VarChar, 1000, ChangePasswordEmailId);
            dbParam[6] = new DbParameter("@ChangePasswordCC", DbParameter.DbType.VarChar, 1000, ChangePasswordEmailCC);
            dbParam[7] = new DbParameter("@ForgetPasswordEmailId", DbParameter.DbType.VarChar, 1000, ChangePasswordEmailId);
            dbParam[8] = new DbParameter("@ForgetPasswordCC", DbParameter.DbType.VarChar, 1000, ChangePasswordEmailCC);
            dbParam[9] = new DbParameter("@OrderListEmailId", DbParameter.DbType.VarChar, 1000, OrderApprovalEmailId);
            dbParam[10] = new DbParameter("@OrderListEmailCC", DbParameter.DbType.VarChar, 1000, OrderApprovalEmailCC);
            dbParam[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[12] = new DbParameter("@MailServer", DbParameter.DbType.VarChar, 100, MailServer);
            dbParam[13] = new DbParameter("@ExpenseAdminEmailID", DbParameter.DbType.VarChar, 1000, ExpenseAdminEmailID);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UpdateEmailSettings", dbParam);
            return Convert.ToInt32(dbParam[11].Value);
        }
        public int InsertEnviro(string distributorsearch, string Itemsearch, string itemwisesecondary, string Areawisedistributor, 
            string DSREntry_WithWhom, string DSREntry_NextVisitWithWhom, string DSREntry_NextVisitDate, string DSREntry_RetailerOrderByEmail, 
            string DSREntry_RetailerOrderByPhone, string DSREntry_Remarks, string DSREntry_ExpensesFromArea, string DSREntry_VisitType,
            string DSREntry_Attendance, string DSREntry_OtherExpenses, string DSREntry_OtherExpensesRemarks, string DSRENTRY_ExpenseToArea,
            string DSRENTRY_Chargeable, string PrimaryDistributorDis_NextVisitDate, string PrimaryDistributorDis_Remark, string PrimaryFailedVisit_NextVisitDate,string PrimaryFailedVisit_Remark,
            string PrimaryCollection_Remark, string DSREntry_WithWhom_Active, string DSREntry_NextVisitWithWhom_Active, string DSREntry_NextVisitDate_Active, string DSREntry_RetailerOrderByEmail_Active, string DSREntry_RetailerOrderByPhone_Active,
            string DSREntry_Remarks_Active, string DSREntry_ExpensesFromArea_Active, string DSREntry_VisitType_Active, string DSREntry_Attendance_Active,
            string DSREntry_OtherExpenses_Active, string DSREntry_OtherExpensesRemarks_Active, string DSRENTRY_ExpenseToArea_req, string DSRENTRY_Chargeable_req,
            string PrimaryDistributorDis_NextVisitDate_Rq, string PrimaryDistributorDis_Remark_Rq, string PrimaryFailedVisit_NextVisitDate_Rq, string PrimaryFailedVisit_Remark_Rq,
            string PrimaryCollection_Remark_Rq,string bookodrRemarkItem,string bookodrRemarkItem_Rq,string bookodrRemark,string bookodrRemark_Rq  ,
            string DemoEntryRemark,string DemoEntryRemark_Rq,
            string SecFailedVisit_NextVisit ,string SecFailedVisit_NextVisit_Rq  ,string SecFailedVisitRemark ,
            string SecFailedVisitRemark_Rq, string CompetitorActivityRemark, string CompetitorActivityRemark_Rq, string AttBymanual, string AttBymanual_Rq, string AttByFirstLastOrder, string AttByFirstLastOrder_Rq, string AttByPhoto, string AttByPhoto_Rq, string BeatPlanMandatory, string BeatPlanMandatory_Rq, string UseCamera, string UseCamera_Rq)
        {
            DbParameter[] dbParam = new DbParameter[63]; 
            dbParam[0] = new DbParameter("@distributorsearch", DbParameter.DbType.VarChar, 1, distributorsearch);
            dbParam[1] = new DbParameter("@Itemsearch", DbParameter.DbType.VarChar, 1, Itemsearch);
            dbParam[2] = new DbParameter("@itemwisesecondary", DbParameter.DbType.VarChar, 1, itemwisesecondary);
            dbParam[3] = new DbParameter("@Areawisedistributor", DbParameter.DbType.VarChar, 1, Areawisedistributor);

           

            dbParam[4] = new DbParameter("@WithWhom", DbParameter.DbType.VarChar, 1, DSREntry_WithWhom);
            dbParam[5] = new DbParameter("@NextVisitWithWhom", DbParameter.DbType.VarChar, 1, DSREntry_NextVisitWithWhom);
            dbParam[6] = new DbParameter("@NextVisitDate", DbParameter.DbType.VarChar, 1, DSREntry_NextVisitDate);
            dbParam[7] = new DbParameter("@RetailerOrderByEmail", DbParameter.DbType.VarChar, 1, DSREntry_RetailerOrderByEmail);

            dbParam[8] = new DbParameter("@RetailerOrderByPhone", DbParameter.DbType.VarChar, 1, DSREntry_RetailerOrderByPhone);
            dbParam[9] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 1, DSREntry_Remarks);
            dbParam[10] = new DbParameter("@ExpensesFromArea", DbParameter.DbType.VarChar, 1, DSREntry_ExpensesFromArea);
            dbParam[11] = new DbParameter("@VisitType", DbParameter.DbType.VarChar, 1, DSREntry_VisitType);

            dbParam[12] = new DbParameter("@Attendance", DbParameter.DbType.VarChar, 1, DSREntry_Attendance);
            dbParam[13] = new DbParameter("@OtherExpenses", DbParameter.DbType.VarChar, 1, DSREntry_OtherExpenses);
            dbParam[14] = new DbParameter("@OtherExpensesRemarks", DbParameter.DbType.VarChar, 1, DSREntry_OtherExpensesRemarks);
            dbParam[15] = new DbParameter("@DSRENTRY_ExpenseToArea", DbParameter.DbType.VarChar, 1, DSRENTRY_ExpenseToArea);
            dbParam[16] = new DbParameter("@DSRENTRY_Chargeable ", DbParameter.DbType.VarChar, 1, DSRENTRY_Chargeable);

            dbParam[17] = new DbParameter("@PrimaryDistributorDis_NextVisitDate", DbParameter.DbType.VarChar, 1, PrimaryDistributorDis_NextVisitDate);
            dbParam[18] = new DbParameter("@PrimaryDistributorDis_Remark", DbParameter.DbType.VarChar, 1, PrimaryDistributorDis_Remark);
            dbParam[19] = new DbParameter("@PrimaryFailedVisit_NextVisitDate", DbParameter.DbType.VarChar, 1, PrimaryFailedVisit_NextVisitDate);
            dbParam[20] = new DbParameter("@PrimaryFailedVisit_Remark", DbParameter.DbType.VarChar, 1, PrimaryFailedVisit_Remark);
            dbParam[21] = new DbParameter("@PrimaryCollection_Remark", DbParameter.DbType.VarChar, 1, PrimaryCollection_Remark);
           





            dbParam[22] = new DbParameter("@DSREntry_WithWhom_Rq", DbParameter.DbType.Bit, 1,Convert.ToInt32(DSREntry_WithWhom_Active));
            dbParam[23] = new DbParameter("@DSREntry_NextVisitWithWhom_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(DSREntry_NextVisitWithWhom_Active));
            dbParam[24] = new DbParameter("@DSREntry_NextVisitDate_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(DSREntry_NextVisitDate_Active));
            dbParam[25] = new DbParameter("@DSREntry_RetailerOrderByEmail_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(DSREntry_RetailerOrderByEmail_Active));

            dbParam[26] = new DbParameter("@DSREntry_RetailerOrderByPhone_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(DSREntry_RetailerOrderByPhone_Active));
            dbParam[27] = new DbParameter("@DSREntry_Remarks_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(DSREntry_Remarks_Active));
            dbParam[28] = new DbParameter("@DSREntry_ExpensesFromArea_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(DSREntry_ExpensesFromArea_Active));
            dbParam[29] = new DbParameter("@DSREntry_VisitType_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(DSREntry_VisitType_Active));

            dbParam[30] = new DbParameter("@DSREntry_Attendance_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(DSREntry_Attendance_Active));
            dbParam[31] = new DbParameter("@DSREntry_OtherExpenses_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(DSREntry_OtherExpenses_Active));
            dbParam[32] = new DbParameter("@DSREntry_OtherExpensesRemarks_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(DSREntry_OtherExpensesRemarks_Active));
            dbParam[33] = new DbParameter("@DSRENTRY_ExpenseToArea_req", DbParameter.DbType.Bit, 1, Convert.ToInt32(DSRENTRY_ExpenseToArea_req));
            dbParam[34] = new DbParameter("@DSRENTRY_Chargeable_req", DbParameter.DbType.Bit, 1, Convert.ToInt32(DSRENTRY_Chargeable_req));

            dbParam[35] = new DbParameter("@PrimaryDistributorDis_NextVisitDate_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(PrimaryDistributorDis_NextVisitDate_Rq));
            dbParam[36] = new DbParameter("@PrimaryDistributorDis_Remark_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(PrimaryDistributorDis_Remark_Rq));
            dbParam[37] = new DbParameter("@PrimaryFailedVisit_NextVisitDate_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(PrimaryFailedVisit_NextVisitDate_Rq));
            dbParam[38] = new DbParameter("@PrimaryFailedVisit_Remark_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(PrimaryFailedVisit_Remark_Rq));
            dbParam[39] = new DbParameter("@PrimaryCollection_Remark_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(PrimaryCollection_Remark_Rq));

            dbParam[40] = new DbParameter("@bookodrRemarkItem", DbParameter.DbType.VarChar, 1, bookodrRemarkItem);
            dbParam[41] = new DbParameter("@bookodrRemarkItem_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(bookodrRemarkItem_Rq));

            dbParam[42] = new DbParameter("@bookodrRemark", DbParameter.DbType.VarChar, 1, bookodrRemark);
            dbParam[43] = new DbParameter("@bookodrRemark_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(bookodrRemark_Rq));

            dbParam[44] = new DbParameter("@DemoEntryRemark", DbParameter.DbType.VarChar, 1, DemoEntryRemark);
            dbParam[45] = new DbParameter("@DemoEntryRemark_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(DemoEntryRemark_Rq));

            dbParam[46] = new DbParameter("@SecFailedVisit_NextVisit", DbParameter.DbType.VarChar, 1, SecFailedVisit_NextVisit);
            dbParam[47] = new DbParameter("@SecFailedVisit_NextVisit_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(SecFailedVisit_NextVisit_Rq));

            dbParam[48] = new DbParameter("@SecFailedVisitRemark", DbParameter.DbType.VarChar, 1, SecFailedVisitRemark);
            dbParam[49] = new DbParameter("@SecFailedVisitRemark_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(SecFailedVisitRemark_Rq));

            dbParam[50] = new DbParameter("@CompetitorActivityRemark", DbParameter.DbType.VarChar, 1, CompetitorActivityRemark);
            dbParam[51] = new DbParameter("@CompetitorActivityRemark_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(CompetitorActivityRemark_Rq));
            dbParam[52] = new DbParameter("@AttBymanual", DbParameter.DbType.VarChar, 1, AttBymanual);
            dbParam[53] = new DbParameter("@AttBymanual_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(AttBymanual_Rq));
            dbParam[54] = new DbParameter("@AttByFirstLastOrder", DbParameter.DbType.VarChar, 1, AttByFirstLastOrder);
            dbParam[55] = new DbParameter("@AttByFirstLastOrder_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(AttByFirstLastOrder_Rq));
            dbParam[56] = new DbParameter("@AttByPhoto", DbParameter.DbType.VarChar, 1, AttByPhoto);
            dbParam[57] = new DbParameter("@AttByPhoto_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(AttByPhoto_Rq));
            dbParam[58] = new DbParameter("@BeatPlanMandatory", DbParameter.DbType.VarChar, 1, BeatPlanMandatory);
            dbParam[59] = new DbParameter("@BeatPlanMandatory_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(BeatPlanMandatory_Rq));
            dbParam[60] = new DbParameter("@UseCamera", DbParameter.DbType.VarChar, 1, UseCamera);
            dbParam[61] = new DbParameter("@UseCamera_Rq", DbParameter.DbType.Bit, 1, Convert.ToInt32(UseCamera_Rq));
            dbParam[62] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UpdateEnviroSettings", dbParam);
            return Convert.ToInt32(dbParam[62].Value);
        }
    }
}
