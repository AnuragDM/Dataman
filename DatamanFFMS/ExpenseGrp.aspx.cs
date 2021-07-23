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
using System.IO;

namespace AstralFFMS
{
    public partial class ExpenseGrp : System.Web.UI.Page
    {
        ExpenseBAL EB = new ExpenseBAL();
        ExpensesGroupBAL EXG = new ExpensesGroupBAL();
        string parameter = "";
       protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Settings.Instance.RoleID))
            {
                //string RoleType = DbConnectionDAL.GetScalarValue(CommandType.Text, "select RoleType from MastRole where RoleId="
                //    + Settings.Instance.RoleID + "").ToString();
                //if (RoleType.ToUpper() == "Admin".ToUpper()) { Response.Redirect("~/Login.aspx"); }
                    }
            else { Response.Redirect("~/Login.aspx"); }
            //string createText = " Exp Grp smid = '" + Settings.Instance.SMID + "'";

            //using (System.IO.StreamWriter TextFileCID = new System.IO.StreamWriter(Server.MapPath("TextFileFolder/Expgrp.txt"), true))
            //{
            //    TextFileCID.WriteLine(createText);
            //    TextFileCID.Close();
            //}
            Page.Form.DefaultButton = btnSave.UniqueID;
            this.Form.DefaultButton = this.btnSave.UniqueID;
            parameter = Request["__EVENTARGUMENT"];
            EffDateFrom.Attributes.Add("readonly", "readonly");
            EffDateTo.Attributes.Add("readonly", "readonly");
            if (parameter != "")
            {
                Session["ExpenseGroupId"] = parameter;
                FillExpGrpControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            //Ankita - 10/may/2016- (For Optimization)
            string pageName = Path.GetFileName(Request.Path);           
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;
            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            if (btnSave.Text == "Save")
            {
                // btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                //btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
                btnSave.CssClass = "btn btn-primary";
            }

            //btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            btnDelete.CssClass = "btn btn-primary";
           

            if (!IsPostBack)
            { 
                btnDelete.Visible = false;
                mainDiv.Style.Add("display", "none");
                rptmain.Style.Add("display", "block");
                fillRepeter();
                if (Request.QueryString["ExpenseGroupId"] != null)
                {
                    FillExpGrpControls(Convert.ToInt32(Request.QueryString["ExpenseGroupId"]));
                }

                CalendarExtender3.StartDate = Settings.GetUTCTime().AddMonths(-1);
                CalendarExtender3.EndDate = Settings.GetUTCTime();
                //CalendarExtender4.StartDate = DateTime.UtcNow;
                //CalendarExtender4.EndDate = DateTime.UtcNow.AddMonths(1);
                
            }
        }
       
       
        private void fillRepeter()
       {//Ankita - 10/may/2016- (For Optimization)
            //string str = @"select *,replace(convert(NVARCHAR, fromdate, 106), ' ', '/') + ' -- ' + replace(convert(NVARCHAR, todate, 106), ' ', '/') as datefromto  from ExpenseGroup where SMID=" + Settings.Instance.SMID + " and (ISNULL(IsDeactivated,0)<>1 and ISNULL(IsSubmitted,0) <>1)";
           string str = @"select ExpenseGroupId,GroupName,Remarks,replace(convert(NVARCHAR, fromdate, 106), ' ', '/') + ' -- ' + replace(convert(NVARCHAR, todate, 106), ' ', '/') as datefromto  from ExpenseGroup where SMID=" + Settings.Instance.SMID + " and (ISNULL(IsDeactivated,0)<>1 and ISNULL(IsSubmitted,0) <>1)";
            DataTable ExpGrpdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            rpt.DataSource = ExpGrpdt;
            rpt.DataBind();
        }

        protected void EffDateFrom_TextChanged(object sender, EventArgs e)
        {
            string val = hdnEnviroValue.Value;
            if(val == "1")
            {
              //  string fromdate = EffDateFrom.Text;
                DateTime origDT = Convert.ToDateTime(EffDateFrom.Text);
                DateTime lastDate = new DateTime(origDT.Year, origDT.Month, 1).AddMonths(1).AddDays(-1);
                DateTime StartDate = new DateTime(origDT.Year, origDT.Month, 1);
                EffDateFrom.Text = StartDate.ToString("dd/MMM/yyyy");
                EffDateTo.Text = lastDate.ToString("dd/MMM/yyyy");
            }
        }
        private void FillExpGrpControls(int ExpenseGroupId)
        {
            try
            {//Ankita - 10/may/2016- (For Optimization)
                //string ExpGrpQry = @"select * from ExpenseGroup where ExpenseGroupId=" + ExpenseGroupId;
                string ExpGrpQry = @"select GroupName,Remarks,FromDate,ToDate from ExpenseGroup where ExpenseGroupId=" + ExpenseGroupId;
                DataTable ExpGrpValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, ExpGrpQry);
                if (ExpGrpValueDt.Rows.Count > 0)
                {
                    Name.Value = ExpGrpValueDt.Rows[0]["GroupName"].ToString();
                    Remarks.Value = ExpGrpValueDt.Rows[0]["Remarks"].ToString();
                    EffDateFrom.Text = Convert.ToDateTime(ExpGrpValueDt.Rows[0]["FromDate"]).ToString("dd/MMM/yyyy");
                    EffDateTo.Text = Convert.ToDateTime(ExpGrpValueDt.Rows[0]["ToDate"]).ToString("dd/MMM/yyyy");
                    // Nishu 25/07/2016  UAT Mail on 25/07/2016
                    //EffDateFrom.Enabled = false; EffDateTo.Enabled = false;
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while showing the records');", true);
            }
        }
       
        private void InsertExpenseGroup()
        {
            if (Convert.ToDateTime(EffDateFrom.Text) > Convert.ToDateTime(EffDateTo.Text))
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                return;
            }            
            if ((Convert.ToDateTime(EffDateTo.Text).Subtract(Convert.ToDateTime(EffDateFrom.Text)).TotalDays) >= 30)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date and To Date difference should be less than or equal to thirty days');", true);
                return;
            }
            DateTime Currdt = Settings.GetUTCTime();
            string docID = Settings.GetDocID("EXGRP", Currdt);
            Settings.SetDocID("EXGRP", docID);
            string VoucherNo = docID.Substring(docID.Length - 8).TrimStart(new Char[] { '0' });

            int retval = EXG.InsertUpdateExpenseGroup(0, Name.Value, Remarks.Value, Convert.ToDateTime(EffDateFrom.Text), Convert.ToDateTime(EffDateTo.Text), Convert.ToInt32(BusinessLayer.Settings.Instance.UserID), Convert.ToInt32(BusinessLayer.Settings.Instance.SMID),docID,Convert.ToInt32(VoucherNo));
            if (retval == -1)
            {               
                Name.Focus();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Group Name Exists');", true);
                return;
            }
          
            else
            {
              
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                rptmain.Style.Add("display", "block");
                mainDiv.Style.Add("display", "none");
                ClearControls();
                //string strdel = "delete from [TempExpense] where SMID=" + Settings.Instance.SMID;
                //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strdel);
                //string strins = "Insert into [TempExpense](SMID,ExpGrp) values (" + Settings.Instance.SMID + "," + retval + ")";
                //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strins);
                //Session["ExpenseGroupId"] = retval;
                Response.Redirect("ExpenseAdd.aspx?ExpenseGroupId=" + retval + "");
               // return;
            }
        }
        private void UpdateExpenseGroup()
        {
            if (Convert.ToDateTime(EffDateFrom.Text) > Convert.ToDateTime(EffDateTo.Text))
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                return;
            }
            if ((Convert.ToDateTime(EffDateTo.Text).Subtract(Convert.ToDateTime(EffDateFrom.Text)).TotalDays) >= 30)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date and To Date difference should be less than or equal to thirty days');", true);
                return;
            }
            int retval = EXG.InsertUpdateExpenseGroup(Convert.ToInt32(Session["ExpenseGroupId"]), Name.Value, Remarks.Value, Convert.ToDateTime(EffDateFrom.Text), Convert.ToDateTime(EffDateTo.Text), Convert.ToInt32(BusinessLayer.Settings.Instance.UserID), Convert.ToInt32(BusinessLayer.Settings.Instance.SMID), "", 0);

               if (retval == -1)
            {              
                Name.Focus();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Group Name Exists');", true);
                return;
            }
            else
            {

                ClearControls();
                Name.Focus();
                btnDelete.Visible = false;
                btnSave.Text = "Save";
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "success", "alert('Record Updated Successfully'); setInterval(function(){location.href='ExpenseGrp.aspx';},100);", true);
                rptmain.Style.Add("display", "block");
                mainDiv.Style.Add("display", "none");
             //   ClearControls();
              //  Session["ExpenseGroupId"] = retval;
                //string strdel = "delete from [TempExpense] where SMID=" + Settings.Instance.SMID;
                //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strdel);
                //string strins = "Insert into [TempExpense](SMID,ExpGrp) values (" + Settings.Instance.SMID + "," + retval + ")";
                //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strins);
                Response.Redirect("ExpenseAdd.aspx?ExpenseGroupId="+retval+"");
                //return;
            }
        }
        private void ClearControls()
        {
            Name.Value = "";
            Remarks.Value = "";
            EffDateFrom.Text = "";
            EffDateTo.Text = "";
            EffDateFrom.Enabled = true; EffDateTo.Enabled = true;
           // Response.Redirect("~/ExpenseGrp.aspx");
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none"); EffDateFrom.Enabled = true; EffDateTo.Enabled = true;
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = EXG.delete(Convert.ToString(Session["ExpenseGroupId"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    Name.Focus();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                   // ClearControls();
                    Name.Focus();
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/ExpenseGrp.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //try
            //{
                if (btnSave.Text == "Update")
                {
                    UpdateExpenseGroup();
                }
                else
                {
                    InsertExpenseGroup();
                }
            //}
            //catch (Exception ex)
            //{
            //    ex.ToString();
            //    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            //}
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            ClearControls();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");

        }

        protected void lnksubmit_Click(object sender, EventArgs e)
        {
             string confirmValue = Request.Form["confirm_value"];
             if (confirmValue == "Yes")
             { }
        }

        ////protected void lnkgrp_Command(object sender, CommandEventArgs e)
        ////{
        ////    var grpId = e.CommandArgument; 
        ////    string strdel = "delete from [TempExpense] where SMID="+Settings.Instance.SMID;
        ////    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strdel);
        ////    string strins = "Insert into [TempExpense](SMID,ExpGrp) values (" + Settings.Instance.SMID + "," + grpId + ")";
        ////    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strins);
            
        ////    //Session["ExpenseGroupId"] = grpId; 
        ////    Response.Redirect("ExpenseAdd.aspx");
        ////}
       
    }
}