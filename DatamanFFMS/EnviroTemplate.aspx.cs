using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BAL;
using System.Reflection;
using BusinessLayer;
using System.Text.RegularExpressions;
using System.IO;

namespace AstralFFMS
{
    public partial class EnviroTemplate : System.Web.UI.Page
    {
        BAL.EmailSetting.EmailSettings dp = new BAL.EmailSetting.EmailSettings();       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
            string pageName = Path.GetFileName(Request.Path);            
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            if (btnSave.Text == "Save")
            {
                btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
        }

        private void LoadData()
        {
            string str = "Select DistSearchByName,ItemSearchByName,ItemwiseSeconadarySales,AreawiseDistributor,DSREntry_WithWhom,AttBymanual, AttByFirstLastOrder,AttByPhoto,BeatPlanMandatory,UseCamera,";
            str = str + " PrimaryDistributorDis_NextVisitDate,PrimaryDistributorDis_Remark,PrimaryFailedVisit_NextVisitDate,";
            str = str + " PrimaryFailedVisit_Remark,PrimaryCollection_Remark ,DSREntry_NextVisitWithWhom ,DSREntry_NextVisitDate ,DSREntry_RetailerOrderByEmail ,DSREntry_RetailerOrderByPhone ,DSREntry_Remarks ,DSREntry_ExpensesFromArea ,DSREntry_VisitType ,DSREntry_Attendance ,DSREntry_OtherExpenses ,DSREntry_OtherExpensesRemarks ,IsNull((DSREntry_WithWhom_Rq),0) as DSREntry_WithWhom_Rq,IsNull((DSREntry_NextVisitWithWhom_Rq),0) as DSREntry_NextVisitWithWhom_Rq ,IsNull((DSREntry_NextVisitDate_Rq),0) as DSREntry_NextVisitDate_Rq,IsNull((DSREntry_RetailerOrderByEmail_Rq),0) as DSREntry_RetailerOrderByEmail_Rq,IsNull((DSREntry_RetailerOrderByPhone_Rq),0) as DSREntry_RetailerOrderByPhone_Rq,IsNull((DSREntry_Remarks_Rq),0) as DSREntry_Remarks_Rq,IsNull((DSREntry_ExpensesFromArea_Rq),0) as DSREntry_ExpensesFromArea_Rq,IsNull((DSREntry_VisitType_Rq),0) as DSREntry_VisitType_Rq,IsNull((DSREntry_Attendance_Rq),0) as DSREntry_Attendance_Rq,IsNull((DSREntry_OtherExpenses_Rq),0) as DSREntry_OtherExpenses_Rq,IsNull((DSREntry_OtherExpensesRemarks_Rq),0) as DSREntry_OtherExpensesRemarks_Rq,IsNull((DSRENTRY_ExpenseToArea_req),0) as DSRENTRY_ExpenseToArea_req,IsNull((DSRENTRY_Chargeable_req),0) as DSRENTRY_Chargeable_req,IsNull((DSRENTRY_ExpenseToArea),0) as DSRENTRY_ExpenseToArea,IsNull((DSRENTRY_Chargeable),0) as DSRENTRY_Chargeable, ";
            str = str + "Isnull(PrimaryDistributorDis_NextVisitDate_Rq,0) As PrimaryDistributorDis_NextVisitDate_Rq, Isnull(PrimaryDistributorDis_Remark_Rq,0)  As PrimaryDistributorDis_Remark_Rq,";
            str = str + "Isnull(PrimaryFailedVisit_NextVisitDate_Rq,0) As PrimaryFailedVisit_NextVisitDate_Rq,Isnull(PrimaryFailedVisit_Remark_Rq,0) As PrimaryFailedVisit_Remark_Rq,";
            str = str + "Isnull(PrimaryCollection_Remark_Rq,0) As PrimaryCollection_Remark_Rq, ";
            str = str + "bookodrRemarkItem ,bookodrRemark,DemoEntryRemark ,SecFailedVisit_NextVisit ,SecFailedVisitRemark ,CompetitorActivityRemark ,";
            str = str + "Isnull(bookodrRemark_Rq,0) As bookodrRemark_Rq,Isnull(bookodrRemarkItem_Rq,0) As bookodrRemarkItem_Rq,Isnull(DemoEntryRemark_Rq ,0) As DemoEntryRemark_Rq ,";
            str = str + "Isnull(SecFailedVisit_NextVisit_Rq,0) As SecFailedVisit_NextVisit_Rq ,Isnull(SecFailedVisitRemark_Rq,0) As SecFailedVisitRemark_Rq,Isnull(CompetitorActivityRemark_Rq,0) As CompetitorActivityRemark_Rq, Isnull(AttBymanual_Rq,0) As AttBymanual_Rq ,Isnull(AttByFirstLastOrder_Rq,0) As AttByFirstLastOrder_Rq ,Isnull(AttByPhoto_Rq,0) As AttByPhoto_Rq ,Isnull(BeatPlanMandatory_Rq,0) As BeatPlanMandatory_Rq ,Isnull(UseCamera_Rq,0) As UseCamera_Rq  from MastEnviro";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt != null && dt.Rows.Count > 0)
            {
                ddlDistSearch.Text = dt.Rows[0]["DistSearchByName"].ToString();
                ddlItemSearch.Text = dt.Rows[0]["ItemSearchByName"].ToString();
                ddlItemwisesale.Text = dt.Rows[0]["ItemwiseSeconadarySales"].ToString();
                ddlAreawiseDistributor.Text = dt.Rows[0]["AreawiseDistributor"].ToString();

                ddlWithWhom.Text = dt.Rows[0]["DSREntry_WithWhom"].ToString();
                ddlNextVisitWithWhom.Text = dt.Rows[0]["DSREntry_NextVisitWithWhom"].ToString();
                ddlNextVisitDate.Text = dt.Rows[0]["DSREntry_NextVisitDate"].ToString();
                ddlRetailerOrderByEmail.Text = dt.Rows[0]["DSREntry_RetailerOrderByEmail"].ToString();
                ddlRetailerOrderByPhone.Text = dt.Rows[0]["DSREntry_RetailerOrderByPhone"].ToString();
                ddlRemarks.Text = dt.Rows[0]["DSREntry_Remarks"].ToString();
                ddlExpensesFromArea.Text = dt.Rows[0]["DSREntry_ExpensesFromArea"].ToString();
                ddlExpensesToArea.Text = dt.Rows[0]["DSRENTRY_ExpenseToArea"].ToString();
                ddlchargeable.Text = dt.Rows[0]["DSRENTRY_Chargeable"].ToString();
                ddlVisitType.Text = dt.Rows[0]["DSREntry_VisitType"].ToString();
                ddlAttendance.Text = dt.Rows[0]["DSREntry_Attendance"].ToString();
                ddlOtherExpenses.Text = dt.Rows[0]["DSREntry_OtherExpenses"].ToString();
                ddlOtherExpensesRemarks.Text = dt.Rows[0]["DSREntry_OtherExpensesRemarks"].ToString();
                ddlDistributorDisNextVisit.Text = dt.Rows[0]["PrimaryDistributorDis_NextVisitDate"].ToString();
                ddlDistributorDisRemarks.Text = dt.Rows[0]["PrimaryDistributorDis_Remark"].ToString();
                ddlFailedVisitNextVis.Text =dt.Rows[0]["PrimaryFailedVisit_NextVisitDate"].ToString();
                ddlFailedVisitRemark.Text  = dt.Rows[0]["PrimaryFailedVisit_Remark"].ToString();
                ddlCollectRemarks.Text = dt.Rows[0]["PrimaryCollection_Remark"].ToString();

                cbChargeable.Checked = Convert.ToBoolean(dt.Rows[0]["DSRENTRY_Chargeable_req"].ToString());
                cbExpensestoArea.Checked = Convert.ToBoolean(dt.Rows[0]["DSRENTRY_ExpenseToArea_req"].ToString());
                cbWithWhom.Checked = Convert.ToBoolean(dt.Rows[0]["DSREntry_WithWhom_Rq"].ToString());
                cbNextVisitWithWhom.Checked = Convert.ToBoolean(dt.Rows[0]["DSREntry_NextVisitWithWhom_Rq"].ToString());
                cbNextVisitDate.Checked = Convert.ToBoolean(dt.Rows[0]["DSREntry_NextVisitDate_Rq"].ToString());
                cbRetailerOrderByEmail.Checked = Convert.ToBoolean(dt.Rows[0]["DSREntry_RetailerOrderByEmail_Rq"].ToString());
                cbRetailerOrderByPhone.Checked = Convert.ToBoolean(dt.Rows[0]["DSREntry_RetailerOrderByPhone_Rq"].ToString());
                cbRemarks.Checked = Convert.ToBoolean(dt.Rows[0]["DSREntry_Remarks_Rq"].ToString());
                cbExpensesFromArea.Checked = Convert.ToBoolean(dt.Rows[0]["DSREntry_ExpensesFromArea_Rq"].ToString());
                cbVisitType.Checked = Convert.ToBoolean(dt.Rows[0]["DSREntry_VisitType_Rq"].ToString());
                cbAttendance.Checked = Convert.ToBoolean(dt.Rows[0]["DSREntry_Attendance_Rq"].ToString());
                cbOtherExpenses.Checked = Convert.ToBoolean(dt.Rows[0]["DSREntry_OtherExpenses_Rq"].ToString());
                cbOtherExpensesRemarks.Checked = Convert.ToBoolean(dt.Rows[0]["DSREntry_OtherExpensesRemarks_Rq"].ToString());

                ChkDistributorDisNextVisit.Checked = Convert.ToBoolean(dt.Rows[0]["PrimaryDistributorDis_NextVisitDate_Rq"].ToString());
                ChkDistributorDisRemarks.Checked = Convert.ToBoolean(dt.Rows[0]["PrimaryDistributorDis_Remark_Rq"].ToString());
                ChkFailedVisitNextVis.Checked = Convert.ToBoolean(dt.Rows[0]["PrimaryFailedVisit_NextVisitDate_Rq"].ToString());
                ChkFailedVisitRemark.Checked = Convert.ToBoolean(dt.Rows[0]["PrimaryFailedVisit_Remark_Rq"].ToString());
                ChkCollectRemarks.Checked = Convert.ToBoolean(dt.Rows[0]["PrimaryCollection_Remark_Rq"].ToString());
               


                ddlbookodrRemarkItem.Text= dt.Rows[0]["bookodrRemarkItem"].ToString();
                ChkOrderRemarkItemWise.Checked= Convert.ToBoolean(dt.Rows[0]["bookodrRemarkItem_Rq"].ToString());
                ddlbookodrRemark.Text = dt.Rows[0]["bookodrRemark"].ToString();
                ChkOrderRemark.Checked = Convert.ToBoolean(dt.Rows[0]["bookodrRemark_Rq"].ToString());
                ddlDemoEntryRemark.Text =dt.Rows[0]["DemoEntryRemark"].ToString();
                ChkDemoEntryRemark.Checked = Convert.ToBoolean(dt.Rows[0]["DemoEntryRemark_Rq"].ToString());

                ddlSecFailedVisit_NextVisit.Text =dt.Rows[0]["SecFailedVisit_NextVisit"].ToString();
                ChkSecFailedVuisit_NextVisit.Checked = Convert.ToBoolean(dt.Rows[0]["SecFailedVisit_NextVisit_Rq"].ToString());

                 ddlSecFailedVisitRemark.Text =dt.Rows[0]["SecFailedVisitRemark"].ToString();
                chkSecFailedVisitRemark.Checked = Convert.ToBoolean(dt.Rows[0]["SecFailedVisitRemark_Rq"].ToString());

                ddlCompetitorActivityRemark.Text=dt.Rows[0]["CompetitorActivityRemark"].ToString();
                chkCompetitorActivityRemark.Checked = Convert.ToBoolean(dt.Rows[0]["CompetitorActivityRemark_Rq"].ToString());

                //ddlattmanual.SelectedValue,cbattmanual.Checked ? "1":"0",ddlattbyorder.SelectedValue,cbattbyorder.Checked ? "1":"0",ddlattbyphoto.SelectedValue,cbattbyphoto.Checked ? "1":"0",ddlbeatplanman.SelectedValue,cbbeatplanman.Checked ? "1":"0",ddlCameragallery.SelectedValue,cbCameragallery.Checked ? "1":"0");
                ddlattmanual.Text = dt.Rows[0]["AttBymanual"].ToString();
                cbattmanual.Checked = Convert.ToBoolean(dt.Rows[0]["AttBymanual_Rq"].ToString());
                ddlattbyorder.Text = dt.Rows[0]["AttByFirstLastOrder"].ToString();
                cbattbyorder.Checked = Convert.ToBoolean(dt.Rows[0]["AttByFirstLastOrder_Rq"].ToString());
                ddlattbyphoto.Text = dt.Rows[0]["AttByPhoto"].ToString();
                cbattbyphoto.Checked = Convert.ToBoolean(dt.Rows[0]["AttByPhoto_Rq"].ToString());
                ddlbeatplanman.Text = dt.Rows[0]["BeatPlanMandatory"].ToString();
                cbbeatplanman.Checked = Convert.ToBoolean(dt.Rows[0]["BeatPlanMandatory_Rq"].ToString());
                ddlCameragallery.Text = dt.Rows[0]["UseCamera"].ToString();
                cbCameragallery.Checked = Convert.ToBoolean(dt.Rows[0]["UseCamera_Rq"].ToString());
                //string AttBymanual,string AttBymanual_Rq,string AttByFirstLastOrder,string AttByFirstLastOrder_Rq,string AttByPhoto,string AttByPhoto_Rq,string BeatPlanMandatory,string BeatPlanMandatory_Rq,string UseCamera,string UseCamera_Rq
            }
        }


        protected void changeCss1(Object sender, EventArgs e)
        {
            try
            {
                string str1 = skin1.Text;
                string qry1 = "update theme set isActive= 0";
                string qry2 = "update theme set isActive =1 where ID = " + str1;
                DbConnectionDAL.ExecuteQuery(qry1);
                DbConnectionDAL.ExecuteQuery(qry2);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception exc)
            {

            }

        }
        protected void changeCss2(Object sender, EventArgs e)
        {
            try
            {
                string str1 = skin2.Text;
                string qry1 = "update theme set isActive= 0";
                string qry2 = "update theme set isActive =1 where ID = " + str1;
                DbConnectionDAL.ExecuteQuery(qry1);
                DbConnectionDAL.ExecuteQuery(qry2);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception exc)
            {

            }

        }
        protected void changeCss3(Object sender, EventArgs e)
        {
            try
            {
                string str1 = skin3.Text;
                string qry1 = "update theme set isActive= 0";
                string qry2 = "update theme set isActive =1 where ID = " + str1;
                DbConnectionDAL.ExecuteQuery(qry1);
                DbConnectionDAL.ExecuteQuery(qry2);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception exc)
            {

            }

        }
        protected void changeCss4(Object sender, EventArgs e)
        {
            try
            {
                string str1 = skin4.Text;
                string qry1 = "update theme set isActive= 0";
                string qry2 = "update theme set isActive =1 where ID = " + str1;
                DbConnectionDAL.ExecuteQuery(qry1);
                DbConnectionDAL.ExecuteQuery(qry2);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception exc)
            {

            }

        }
        protected void changeCss5(Object sender, EventArgs e)
        {
            try
            {
                string str1 = skin5.Text;
                string qry1 = "update theme set isActive= 0";
                string qry2 = "update theme set isActive =1 where ID = " + str1;
                DbConnectionDAL.ExecuteQuery(qry1);
                DbConnectionDAL.ExecuteQuery(qry2);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception exc)
            {

            }

        }
        protected void changeCss6(Object sender, EventArgs e)
        {
            try
            {
                string str1 = skin6.Text;
                string qry1 = "update theme set isActive= 0";
                string qry2 = "update theme set isActive =1 where ID = " + str1;
                DbConnectionDAL.ExecuteQuery(qry1);
                DbConnectionDAL.ExecuteQuery(qry2);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception exc)
            {

            }

        }
        protected void changeCss7(Object sender, EventArgs e)
        {
            try
            {
                string str1 = skin7.Text;
                string qry1 = "update theme set isActive= 0";
                string qry2 = "update theme set isActive =1 where ID = " + str1;
                DbConnectionDAL.ExecuteQuery(qry1);
                DbConnectionDAL.ExecuteQuery(qry2);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception exc)
            {

            }

        }
        protected void changeCss8(Object sender, EventArgs e)
        {
            try
            {
                string str1 = skin8.Text;
                string qry1 = "update theme set isActive= 0";
                string qry2 = "update theme set isActive =1 where ID = " + str1;
                DbConnectionDAL.ExecuteQuery(qry1);
                DbConnectionDAL.ExecuteQuery(qry2);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception exc)
            {

            }

        }
        protected void changeCss9(Object sender, EventArgs e)
        {
            try
            {
                string str1 = skin9.Text;
                string qry1 = "update theme set isActive= 0";
                string qry2 = "update theme set isActive =1 where ID = " + str1;
                DbConnectionDAL.ExecuteQuery(qry1);
                DbConnectionDAL.ExecuteQuery(qry2);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception exc)
            {

            }

        }
        protected void changeCss10(Object sender, EventArgs e)
        {
            try
            {
                string str1 = skin10.Text;
                string qry1 = "update theme set isActive= 0";
                string qry2 = "update theme set isActive =1 where ID = " + str1;
                DbConnectionDAL.ExecuteQuery(qry1);
                DbConnectionDAL.ExecuteQuery(qry2);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception exc)
            {

            }

        }
        protected void changeCss11(Object sender, EventArgs e)
        {
            try
            {
                string str1 = skin11.Text;
                string qry1 = "update theme set isActive= 0";
                string qry2 = "update theme set isActive =1 where ID = " + str1;
                DbConnectionDAL.ExecuteQuery(qry1);
                DbConnectionDAL.ExecuteQuery(qry2);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception exc)
            {

            }

        }
        protected void changeCss12(Object sender, EventArgs e)
        {
            try
            {
                string str1 = skin12.Text;
                string qry1 = "update theme set isActive= 0";
                string qry2 = "update theme set isActive =1 where ID = " + str1;
                DbConnectionDAL.ExecuteQuery(qry1);
                DbConnectionDAL.ExecuteQuery(qry2);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception exc)
            {

            }

        }





        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                InsertEnviro();
                mainDiv.Style.Add("display", "none");
                DSREntryDiv.Style.Add("display", "none");
                DivPrimary.Style.Add("display", "none");
                divSecondary.Style.Add("display", "none");
                buttonDiv.Style.Add("display", "block");
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }

        public void setattendance()
        {
           
        }
        private void InsertEnviro()
        {
             if( (ddlattbyphoto.SelectedValue == "N") && (ddlattmanual.SelectedValue == "N") && (ddlattbyorder.SelectedValue == "N"))
                     {
                         System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Is Not Saved,Please Select Yes From AnyOne Attendance Parameters ');", true);
                         return;
                     }

            int retsave = dp.InsertEnviro(ddlDistSearch.SelectedValue, ddlItemSearch.SelectedValue, ddlItemwisesale.SelectedValue, ddlAreawiseDistributor.SelectedValue, ddlWithWhom.SelectedValue, ddlNextVisitWithWhom.SelectedValue, ddlNextVisitDate.SelectedValue, ddlRetailerOrderByEmail.SelectedValue, ddlRetailerOrderByPhone.SelectedValue, ddlRemarks.SelectedValue, ddlExpensesFromArea.SelectedValue, ddlVisitType.SelectedValue, ddlAttendance.SelectedValue, ddlOtherExpenses.SelectedValue, ddlOtherExpensesRemarks.SelectedValue, ddlExpensesToArea.SelectedValue, ddlchargeable.SelectedValue,ddlDistributorDisNextVisit.SelectedValue,ddlDistributorDisRemarks.SelectedValue
                ,ddlFailedVisitNextVis.SelectedValue,ddlFailedVisitRemark.SelectedValue,ddlCollectRemarks.SelectedValue
                , cbWithWhom.Checked ? "1" : "0", cbNextVisitWithWhom.Checked ? "1" : "0", cbNextVisitDate.Checked ? "1" : "0", cbRetailerOrderByEmail.Checked ? "1" : "0", cbRetailerOrderByPhone.Checked ? "1" : "0", cbRemarks.Checked ? "1" : "0", cbExpensesFromArea.Checked ? "1" : "0", cbVisitType.Checked ? "1" : "0", cbAttendance.Checked ? "1" : "0", cbOtherExpenses.Checked ? "1" : "0", cbOtherExpensesRemarks.Checked ? "1" : "0", cbExpensestoArea.Checked ? "1" : "0"
                , cbChargeable.Checked ? "1" : "0",ChkDistributorDisNextVisit.Checked ? "1":"0",ChkDistributorDisRemarks.Checked ? "1":"0",ChkFailedVisitNextVis.Checked ? "1":"0",ChkFailedVisitRemark.Checked ? "1":"0",ChkCollectRemarks.Checked ? "1":"0"
                ,ddlbookodrRemarkItem .SelectedValue,ChkOrderRemarkItemWise.Checked ? "1":"0",ddlbookodrRemark.SelectedValue,ChkOrderRemark.Checked ? "1":"0"
                ,ddlDemoEntryRemark.SelectedValue,ChkDemoEntryRemark.Checked ? "1":"0",ddlSecFailedVisit_NextVisit.SelectedValue,ChkSecFailedVuisit_NextVisit.Checked ? "1":"0"
                ,ddlSecFailedVisitRemark.SelectedValue,chkSecFailedVisitRemark.Checked ? "1":"0",ddlCompetitorActivityRemark.SelectedValue,chkCompetitorActivityRemark.Checked ? "1":"0", ddlattmanual.SelectedValue,cbattmanual.Checked ? "1":"0",ddlattbyorder.SelectedValue,cbattbyorder.Checked ? "1":"0",ddlattbyphoto.SelectedValue,cbattbyphoto.Checked ? "1":"0",ddlbeatplanman.SelectedValue,cbbeatplanman.Checked ? "1":"0",ddlCameragallery.SelectedValue,cbCameragallery.Checked ? "1":"0");

          //  System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
            if (retsave != 0)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Is Not Saved');", true);
                return;
            }
        }  

       protected void btnCancel_Click(object sender, EventArgs e)
       {           
            Response.Redirect("~/EnviroTemplate.aspx");
        }

       protected void btnBack_Click(object sender, EventArgs e)
       {
           mainDiv.Style.Add("display", "none");
           buttonDiv.Style.Add("display", "block");

       }
       protected void btnDSREntry_Click(object sender, EventArgs e)
       {
           DSREntryDiv.Style.Add("display", "block");
           buttonDiv.Style.Add("display", "none");

       }
       protected void btnGeneral_Click(object sender, EventArgs e)
       {
           mainDiv.Style.Add("display", "block");
           buttonDiv.Style.Add("display", "none");

       }
       protected void btnBackDSR_Click(object sender, EventArgs e)
       {
           DSREntryDiv.Style.Add("display", "none");
           buttonDiv.Style.Add("display", "block");
        }

       protected void btnPrimary_Click(object sender, EventArgs e)
       {
           DivPrimary.Style.Add("display", "block");
           buttonDiv.Style.Add("display", "none");
       }

       protected void btnSecondary_Click(object sender, EventArgs e)
       {
           divSecondary.Style.Add("display", "block");
           buttonDiv.Style.Add("display", "none");
       }
        protected void btnbackprimary_Click(object sender,EventArgs e)
       {
           DivPrimary.Style.Add("display", "none");
           buttonDiv.Style.Add("display", "block");
       }
        protected void btnbackSecondary_Click(object sender, EventArgs e)
        {

            divSecondary.Style.Add("display", "none");
            buttonDiv.Style.Add("display", "block");
        }

        protected void ddlattbyorder_SelectedIndexChanged(object sender, EventArgs e)
        {
           
          
            if (ddlattbyorder.SelectedValue == "Y")
            {
                ddlattbyphoto.SelectedValue = "N";
                ddlattmanual.SelectedValue = "N";
                cbattmanual.Checked = false;
                cbattbyphoto.Checked = false;
            }
        }

        protected void ddlattmanual_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlattmanual.SelectedValue == "Y") 
            {
                ddlattbyphoto.SelectedValue = "N";
                ddlattbyorder.SelectedValue = "N";
                cbattbyorder.Checked = false;
                cbattbyphoto.Checked = false;
            }
        }

        protected void ddlattbyphoto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlattbyphoto.SelectedValue == "Y") 
            {
                ddlattmanual.SelectedValue = "N";
                ddlattbyorder.SelectedValue = "N";
                cbattbyorder.Checked = false;
                cbattmanual.Checked = false;
            }
        }
    }
}